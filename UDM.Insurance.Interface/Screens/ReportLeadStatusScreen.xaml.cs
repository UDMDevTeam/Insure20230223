using System.Data.SqlClient;
using System.Windows.Resources;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using Orientation = Infragistics.Documents.Excel.Orientation;
using System.Linq;
using System.Collections.Generic;
using UDM.Insurance.Business;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportLeadStatusScreen
    {

        #region Constants

        //private string _fontName = "Calibri";
        //private const int _fontSize = 10;
        //private const int _pointsToTwipsFactor = 20;
        //private const int _fontHeight = _fontSize * _pointsToTwipsFactor;

        #endregion

        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        private DataRow _campaign;
        private long _campaignID;
        private RecordCollectionBase _batches;
        private List<Record> _lstSelectedBatches;
        //string _batchIDs;
        //private DateTime _fromDate = DateTime.Now;
        //private DateTime _toDate = DateTime.Now;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        private readonly DispatcherTimer dispatcherTimer2 = new DispatcherTimer();
        private int _timer2;

        private INBatch _inBatch;
        private int _noDataBatchCount = 0;

        #endregion Private Members

        #region Constructors

        public ReportLeadStatusScreen()
        {
            InitializeComponent();
            LoadCampaignInfo();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);

            dispatcherTimer2.Tick += Timer2;
            dispatcherTimer2.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

        private void EnableAllControls(bool isEnabled)
        {
            btnLeadStatusReport.IsEnabled = isEnabled;
            btnLatentLeadsReport.IsEnabled = isEnabled;
            btnClose.IsEnabled = isEnabled;
            cmbCampaign.IsEnabled = isEnabled;
            xdgBatches.IsEnabled = isEnabled;
        }

        private bool? AllRecordsSelected()
        {
            try
            {
                bool allSelected = true;
                bool noneSelected = true;

                if (xdgBatches.DataSource != null)
                {
                    foreach (DataRow dr in ((DataView)xdgBatches.DataSource).Table.Rows)
                    {
                        allSelected = allSelected && (bool)dr["Select"];
                        noneSelected = noneSelected && !(bool)dr["Select"];
                    }
                }

                if (allSelected)
                {
                    return true;
                }

                if (noneSelected)
                {
                    return false;
                }

                return null;
            }

            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        //private void LoadCampaignInfo()
        //{
        //    try
        //    {
        //        SetCursor(Cursors.Wait);

        //        DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign");
        //        DataColumn column = new DataColumn("Select", typeof(bool));
        //        column.DefaultValue = false;
        //        dt.Columns.Add(column);
        //        dt.DefaultView.Sort = "CampaignName ASC";
        //        xdgCampaigns.DataSource = dt.DefaultView;
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }

        //    finally
        //    {
        //        SetCursor(Cursors.Arrow);
        //    }
        //}

        private void LoadCampaignInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataTable dt = Methods.GetTableData("SELECT [ID] AS [CampaignID], [Name] AS [CampaignName], [Code] AS [CampaignCode] FROM [INCampaign] ORDER BY [Name] ASC");
                cmbCampaign.Populate(dt, "CampaignName", "CampaignID");
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SetCursor(Cursors.Arrow);
            }
        }

        private void EnableDisableExportButton()
        {
            try
            {
                if (_xdgHeaderPrefixAreaCheckbox != null && (_xdgHeaderPrefixAreaCheckbox.IsChecked == true || _xdgHeaderPrefixAreaCheckbox.IsChecked == null))
                {
                    btnLeadStatusReport.IsEnabled = true;
                    return;
                }
                btnLeadStatusReport.IsEnabled = false;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void LeadStatusReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnLeadStatusReport.Content = "Lead Status Report";
            EnableAllControls(true);
        }

        private void LeadStatusReport(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                int rowIndex;
                int summaryPageRowIndex = 6;

                int worksheetCount = 0;
                long campaignID = Convert.ToInt32(_campaign.ItemArray[0]);
                string campaignName = _campaign.ItemArray[1].ToString();
                string campaignDescription = String.Format("{0} ({1})", _campaign.ItemArray[1], _campaign.ItemArray[2]);

                DataTable dtLeadStatusData;
                DataTable dtLeadStatusDataSummary;

                #region Setup excel documents

                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                string filePathAndName = String.Format("{0}Lead Status Report ({1}), {2}.xlsx", GlobalSettings.UserFolder, campaignName, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                Uri uri = new Uri("/Templates/ReportTemplateLeadStatus.xlsx", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    wbTemplate = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }

                Worksheet wsTemplate = wbTemplate.Worksheets["Status"];
                Worksheet wsReport;

                Worksheet wsSummaryTemplate = wbTemplate.Worksheets["Summary"];
                Worksheet wsSummary = wbReport.Worksheets.Add("Summary");

                Methods.CopyExcelRegion(wsSummaryTemplate, 0, 0, 5, 12, wsSummary, 0, 0);

                wsSummary.GetCell("B3").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                wsSummary.GetCell("B4").Value = campaignName;

                //++summaryPageRowIndex;

                #endregion Setup excel documents

                foreach (DataRecord record in _lstSelectedBatches)
                {
                    #region Get report data from database

                    long batchID = Convert.ToInt64(record.Cells["ID"].Value);
                    string batchDescription = record.Cells["Batch"].Value.ToString();
                    
					SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@BatchID", batchID);

                    DataSet dsLeadStatusData = Methods.ExecuteStoredProcedure("spINReportLeadStatus", parameters);
                    
                    #endregion Get report data from database

                    if (dsLeadStatusData.Tables.Count > 0)
                    {
                        #region Summary Page

                        dtLeadStatusDataSummary = dsLeadStatusData.Tables[0];

                        Methods.CopyExcelRegion(wsSummaryTemplate, 6, 0, 0, 12, wsSummary, summaryPageRowIndex, 0);

                        wsSummary.GetCell("A" + (summaryPageRowIndex + 1)).Value = dtLeadStatusDataSummary.Rows[0]["Batch Number"];
                        wsSummary.GetCell("B" + (summaryPageRowIndex + 1)).Value = dtLeadStatusDataSummary.Rows[0]["TotalLeads"];
                        wsSummary.GetCell("C" + (summaryPageRowIndex + 1)).Value = dtLeadStatusDataSummary.Rows[0]["Contacts"];
                        wsSummary.GetCell("D" + (summaryPageRowIndex + 1)).Value = dtLeadStatusDataSummary.Rows[0]["PercContactsToLeads"];
                        wsSummary.GetCell("E" + (summaryPageRowIndex + 1)).Value = dtLeadStatusDataSummary.Rows[0]["Yes"];
                        wsSummary.GetCell("F" + (summaryPageRowIndex + 1)).Value = dtLeadStatusDataSummary.Rows[0]["PercSalesToLeads"];
                        wsSummary.GetCell("G" + (summaryPageRowIndex + 1)).Value = dtLeadStatusDataSummary.Rows[0]["PercSalesToContacts"];
                        wsSummary.GetCell("H" + (summaryPageRowIndex + 1)).Value = dtLeadStatusDataSummary.Rows[0]["Declines"];
                        wsSummary.GetCell("I" + (summaryPageRowIndex + 1)).Value = dtLeadStatusDataSummary.Rows[0]["PercDeclinesToLeads"];
                        wsSummary.GetCell("J" + (summaryPageRowIndex + 1)).Value = dtLeadStatusDataSummary.Rows[0]["Diaries"];
                        wsSummary.GetCell("K" + (summaryPageRowIndex + 1)).Value = dtLeadStatusDataSummary.Rows[0]["PercDiariesToLeads"];
                        wsSummary.GetCell("L" + (summaryPageRowIndex + 1)).Value = dtLeadStatusDataSummary.Rows[0]["StillToContact"];
                        wsSummary.GetCell("M" + (summaryPageRowIndex + 1)).Value = dtLeadStatusDataSummary.Rows[0]["PercStillToContact"];

                        ++summaryPageRowIndex;


                        #endregion Summary Page

                        #region Report Data

                        dtLeadStatusData = dsLeadStatusData.Tables[1];
                        
                        if (dtLeadStatusData.Rows.Count > 0)
                        {
                            rowIndex = 7;

                            wsReport = wbReport.Worksheets.Add(batchDescription);
                            worksheetCount++;

                            Methods.CopyExcelRegion(wsTemplate, 0, 0, 6, 20, wsReport, 0, 0);

                            #region Adding the details

                            wsReport.GetCell("B3").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            wsReport.GetCell("B4").Value = campaignDescription;

                            #endregion Adding the details

                            foreach (DataRow dr in dtLeadStatusData.Rows)
                            {
                                Methods.CopyExcelRegion(wsTemplate, 6, 0, 1, 20, wsReport, rowIndex - 1, 0);

                                wsReport.GetCell("A" + rowIndex).Value = dr["Batch Number"].ToString();
                                wsReport.GetCell("B" + rowIndex).Value = dr["PL Reference Number"].ToString();
                                wsReport.GetCell("C" + rowIndex).Value = dr["Name"].ToString();
                                wsReport.GetCell("D" + rowIndex).Value = dr["Surname"].ToString();
                                wsReport.GetCell("E" + rowIndex).Value = dr["Contact numbers"].ToString();
                                wsReport.GetCell("F" + rowIndex).Value = dr["Status"].ToString();
                                wsReport.GetCell("G" + rowIndex).Value = dr["Date of Sale"].ToString();
                                wsReport.GetCell("H" + rowIndex).Value = dr["Status Classification"].ToString();
                                wsReport.GetCell("I" + rowIndex).Value = dr["Reason"].ToString();
                                wsReport.GetCell("J" + rowIndex).Value = dr["Sales Agent"].ToString();
                                wsReport.GetCell("K" + rowIndex).Value = dr["Last Save Date"].ToString();
                                wsReport.GetCell("L" + rowIndex).Value = dr["ID Number"].ToString();
                                wsReport.GetCell("M" + rowIndex).Value = dr["DateOfBirth"].ToString();
                                wsReport.GetCell("N" + rowIndex).Value = dr["Address"].ToString();
                                wsReport.GetCell("O" + rowIndex).Value = dr["Postal Code"].ToString();
                                wsReport.GetCell("P" + rowIndex).Value = dr["Referror Lead"].ToString();
                                wsReport.GetCell("Q" + rowIndex).Value = dr["Referror Relationship"].ToString();
                                wsReport.GetCell("R" + rowIndex).Value = dr["GiftReceived"].ToString();
                                wsReport.GetCell("S" + rowIndex).Value = dr["GiftRedeemedDate"].ToString();
                                wsReport.GetCell("T" + rowIndex).Value = dr["GiftDeliveryDate"].ToString();
                                wsReport.GetCell("U" + rowIndex).Value = dr["Option"].ToString();

                                rowIndex++;
                            }

                        }
                        #endregion Report Data
                       
                    }
                }

                #region Finally, add the totals and averages to the summary page

                Methods.CopyExcelRegion(wsSummaryTemplate, 7, 0, 0, 12, wsSummary, summaryPageRowIndex, 0);

                wsSummary.GetCell(String.Format("B{0}", summaryPageRowIndex + 1)).ApplyFormula(String.Format("=SUM(B7:B{0})", summaryPageRowIndex));
                wsSummary.GetCell(String.Format("C{0}", summaryPageRowIndex + 1)).ApplyFormula(String.Format("=SUM(C7:C{0})", summaryPageRowIndex));
                wsSummary.GetCell(String.Format("D{0}", summaryPageRowIndex + 1)).ApplyFormula(String.Format("=AVERAGE(D7:D{0})", summaryPageRowIndex));
                wsSummary.GetCell(String.Format("E{0}", summaryPageRowIndex + 1)).ApplyFormula(String.Format("=SUM(E7:E{0})", summaryPageRowIndex));
                wsSummary.GetCell(String.Format("F{0}", summaryPageRowIndex + 1)).ApplyFormula(String.Format("=AVERAGE(F7:F{0})", summaryPageRowIndex));
                wsSummary.GetCell(String.Format("G{0}", summaryPageRowIndex + 1)).ApplyFormula(String.Format("=AVERAGE(G7:G{0})", summaryPageRowIndex));
                wsSummary.GetCell(String.Format("H{0}", summaryPageRowIndex + 1)).ApplyFormula(String.Format("=SUM(H7:H{0})", summaryPageRowIndex));
                wsSummary.GetCell(String.Format("I{0}", summaryPageRowIndex + 1)).ApplyFormula(String.Format("=AVERAGE(I7:I{0})", summaryPageRowIndex));
                wsSummary.GetCell(String.Format("J{0}", summaryPageRowIndex + 1)).ApplyFormula(String.Format("=SUM(J7:J{0})", summaryPageRowIndex));
                wsSummary.GetCell(String.Format("K{0}", summaryPageRowIndex + 1)).ApplyFormula(String.Format("=AVERAGE(K7:K{0})", summaryPageRowIndex));
                wsSummary.GetCell(String.Format("L{0}", summaryPageRowIndex + 1)).ApplyFormula(String.Format("=SUM(L7:L{0})", summaryPageRowIndex));
                wsSummary.GetCell(String.Format("M{0}", summaryPageRowIndex + 1)).ApplyFormula(String.Format("=AVERAGE(M7:M{0})", summaryPageRowIndex));

                #endregion Finally, add the totals and averages to the summary page

                //Save excel document
                wbReport.Save(filePathAndName);

                //Display excel document
                Process.Start(filePathAndName);
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SetCursor(Cursors.Arrow);
            }
        }

        #region Conversion Batches - Related Functionalities
        private void LatentLeadsReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer2.Stop();
            _timer2 = 0;
            btnLatentLeadsReport.Content = "Conversion Batches";
            EnableAllControls(true);

            #region Finally, if there were any batches that were skipped, display a message

            if (_noDataBatchCount > 0)
            {
                string message = String.Empty;
                string messageHeading = String.Empty;

                if (_noDataBatchCount == _lstSelectedBatches.Count)
                {
                    message = "The conversion batch workbook could not be generated for any of the selected batch(es), because none of them contain latent leads.";
                    messageHeading = "No data";
                }
                else
                {
                    message = String.Format("Conversion batch workbook successfully generated.{0}{0} Note: {1} out of {2} selected batches were omitted, either because they did not contain any latent leads, or the leads were not allocated, or the user chose to skip the batch.",
                        Environment.NewLine,
                        _noDataBatchCount,
                        _lstSelectedBatches.Count);
                    messageHeading = "Skipped Batches";
                }

                ShowMessageBox(new INMessageBoxWindow1(), message, messageHeading, ShowMessageType.Information);
            }

            #endregion Finally, if there were any campaigns from which no leads were allocated, display a message
        }

        private void LatentLeadsReport(object sender, DoWorkEventArgs e)
        {

            try
            {
                SetCursor(Cursors.Wait);

                bool result = false;

                #region Loop through the data table and generate a latent leads workbook for each row - if there is any data

                foreach (DataRecord record in _lstSelectedBatches)
                {
                    long batchID = Convert.ToInt64(record.Cells["ID"].Value);
                    string batch = record.Cells["Batch"].Value.ToString();

                    _inBatch = new INBatch(batchID);
                    if (!_inBatch.ContainsLatentLeads.HasValue)
                    {
                        GenerateLatentLeadsWorkbook(batchID);
                    }

                    //The ContainsLatentLeads is set to true
                    else if (_inBatch.ContainsLatentLeads.Value)
                    {
                        string message = String.Format("Batch {0} is currently indicated as having latent leads. Would you like to generate a workbook for this batch anyway?", batch);
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            INMessageBoxWindow2 messageWindow = new INMessageBoxWindow2();
                            messageWindow.buttonOK.Content = "Yes";
                            messageWindow.buttonCancel.Content = "No";
                            result = (bool)ShowMessageBox(messageWindow, message, "Latent Leads in Batch", ShowMessageType.Information);
                        });

                        if (result)
                        {
                            GenerateLatentLeadsWorkbook(batchID);
                        }
                        else
                        {
                            _noDataBatchCount++;
                            continue;
                        }
                    }
                    else
                    {
                        GenerateLatentLeadsWorkbook(batchID);
                    }
                }

                #endregion Loop through the data table and generate a latent leads workbook for each row - if there is any data

            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SetCursor(Cursors.Arrow);
            }
        }

        private void AddLatentLeadsSummarySheet(Workbook workbook, Worksheet wsTemplate, DataTable dataTable, string worksheetHeader)
        {

            int summaryRow = 4;

            #region Setup Summary Page

            Worksheet wsSummary = workbook.Worksheets.Add("Summary");
            //Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsSummary, true, true, true, true, true, true, true, false, true, true, true, true, true, true, true, true, false);
            Methods.CopyExcelRegion(wsTemplate, 0, 0, 32, 7, wsSummary, 0, 0);

            wsSummary.PrintOptions.Header = "&C&B" + String.Format("{0} - Conversion Leads", worksheetHeader);
            wsSummary.GetCell("B2").Value = "Conversion Leads Summary";
            wsSummary.GetCell("E4").Value = "Most Recent Allocation Date";

            //wsSummary.PrintOptions.Footer = "&C&B" + nextMonday.ToString("yyyy/MM/dd");
            Methods.SetExcelStandardWSOptions(wsSummary, Orientation.Landscape);

            //wsSummary.MoveToIndex(0);

            #endregion Setup Summary Page

            foreach (DataRow row in dataTable.Rows)
            {
                if (summaryRow < summaryRow + 18)
                {
                    wsSummary.GetCell(String.Format("B{0}", summaryRow + 1)).Value = row["SalesConsultantFirstName"];
                    wsSummary.GetCell(String.Format("C{0}", summaryRow + 1)).Value = row["SalesConsultantSurname"];
                    wsSummary.GetCell(String.Format("D{0}", summaryRow + 1)).Value = row["BatchCode"];
                    wsSummary.GetCell(String.Format("E{0}", summaryRow + 1)).Value = (Convert.ToDateTime(row["MostRecentAllocationDate"])).ToString("yyyy/MM/dd"); //(Convert.ToDateTime(drAgent["AllocationDate"])).ToString("yyyy/MM/dd");
                    wsSummary.GetCell(String.Format("F{0}", summaryRow + 1)).Value = row["LatentLeadCount"]; //drAgent["Leads2Print"];

                    summaryRow++;
                }
            }
        }

        private void GenerateLatentLeadsWorkbook(long fkINBatchID)
        {
            try
            {
                #region Make special provision for the elite campaigns - and eventually the redeemed & non-redeemed campaigns

                string campaignCode = _campaign["CampaignCode"].ToString();

                if ((campaignCode.Trim() == "PMCBE") ||
                    (campaignCode.Trim() == "PLCBE") ||
                    (campaignCode.Trim() == "PLMMBE") ||
                    (campaignCode.Trim() == "PLBMMBElite") ||
                    (campaignCode.Trim() == "PLFDB"))
                {
                    GenerateLatentLeadsWorkbookElite(fkINBatchID);
                    return;
                }

                #endregion Make special provision for the elite campaigns - and eventually the redeemed & non-redeemed campaigns

                #region Get data from the database

                DataTable dtBatchAssignees = Insure.INGetLatentLeadsReportBatchAssignees(fkINBatchID);

                #endregion Get data from the database

                #region Loop through the data table and generate a latent leads workbook for each row - if there is any data

                if (dtBatchAssignees.Rows.Count > 0)
                {
                    #region Declarations & initializations

                    //string campaignCode = dtBatchAssignees.Rows[0]["CampaignCode"].ToString();
                    string batchCode = dtBatchAssignees.Rows[0]["BatchCode"].ToString();
                    string leadBookName = String.Format("{0} Batch {1}", campaignCode, batchCode);
                    string filePathAndName = String.Format("{0}Conversion Leads - {1} ~ {2}.xlsx",
                            GlobalSettings.UserFolder,
                            leadBookName,
                            DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
                    byte latentLeadReportType = Convert.ToByte(dtBatchAssignees.Rows[0]["LatentLeadReportType"]);

                    #endregion Declarations & initializations

                    #region Setup Excel Workbook
                    Workbook wbTemplate;
                    Workbook wbLatentLeadsReport = new Workbook(WorkbookFormat.Excel2007);

                    Uri uri;
                    int templateLines;
                    int leadsPerPage;
                    int leadColumnWidth;
                    int coverColumns;
                    int coverRows;

                    int upgradeCoverSheetColumnIndex;
                    int upgradeCoverSheetRowIndex;
                    int verticalSpacingBetweenLeads;

                    bool isUpgradeCampaign = false;

                    //switch (campaign.FKINCampaignGroupID)
                    switch (latentLeadReportType)
                    {
                        //case (long)lkpINCampaignGroup.Rejuvenation:
                        //case (long)lkpINCampaignGroup.Defrosted:
                        //case (long)lkpINCampaignGroup.ReDefrost:
                        //case (long)lkpINCampaignGroup.Reactivation:
                        case 2:
                            uri = new Uri("/Templates/PrintTemplate.xlsx", UriKind.Relative);
                            templateLines = 5;
                            leadsPerPage = 4;
                            leadColumnWidth = 16;
                            coverColumns = 15;
                            coverRows = 29;

                            upgradeCoverSheetColumnIndex = 6;
                            upgradeCoverSheetRowIndex = 0;
                            verticalSpacingBetweenLeads = 1;

                            break;

                        case 4:
                            uri = new Uri("/Templates/PrintTemplateUpgrade2.xlsx", UriKind.Relative);
                            templateLines = 15;
                            leadsPerPage = 3;
                            leadColumnWidth = 55;
                            coverColumns = 58;
                            coverRows = 34; //51;
                            isUpgradeCampaign = true;

                            upgradeCoverSheetColumnIndex = 26;
                            upgradeCoverSheetRowIndex = 2;
                            verticalSpacingBetweenLeads = 1;
                            break;

                        default:
                            uri = new Uri("/Templates/PrintTemplateCancerBase.xlsx", UriKind.Relative);
                            templateLines = 4;
                            leadsPerPage = 5;
                            leadColumnWidth = 16;
                            coverColumns = 15;
                            coverRows = 29;

                            upgradeCoverSheetColumnIndex = 6;
                            upgradeCoverSheetRowIndex = 0;
                            verticalSpacingBetweenLeads = 1;
                            break;
                    }

                    StreamResourceInfo info = Application.GetResourceStream(uri);
                    if (info != null)
                    {
                        wbTemplate = Workbook.Load(info.Stream, true);
                    }
                    else
                    {
                        return;
                    }

                    Worksheet wsSummaryTemplate = wbTemplate.Worksheets["Summary"];
                    Worksheet wsLeadTemplate = wbTemplate.Worksheets["Lead"];

                    string leadTemplateSheetName = String.Empty;

                    if (isUpgradeCampaign)
                    {
                        leadTemplateSheetName = "Cover And Lead";
                    }
                    else
                    {
                        leadTemplateSheetName = "Lead";
                    }

                    Worksheet wsCoverAndLeadTemplate = wbTemplate.Worksheets[leadTemplateSheetName];


                    #endregion Setup Excel Workbook

                    #region Add the summary sheet

                    AddLatentLeadsSummarySheet(wbLatentLeadsReport, wsSummaryTemplate, dtBatchAssignees, leadBookName);

                    #endregion Add the summary sheet

                    foreach (DataRow row in dtBatchAssignees.Rows)
                    {
                        #region Getting the needed values from the given data row

                        long fkUserID = Convert.ToInt64(row["FKUserID"]);

                        string salesConsultantFirstName = row["SalesConsultantFirstName"].ToString();
                        string salesConsultantSurname = row["SalesConsultantSurname"].ToString();
                        string salesConsultant = String.Format("{0} {1}", salesConsultantFirstName, salesConsultantSurname); //row["SalesConsultantName"].ToString();

                        DateTime mostRecentAllocationDate = Convert.ToDateTime(row["MostRecentAllocationDate"]);
                        int latentLeadCount = Convert.ToInt32(row["LatentLeadCount"]);
                        DateTime nextMonday = Methods.NextWeekDay(mostRecentAllocationDate, DayOfWeek.Monday);

                        #endregion Getting the needed values from the given data row

                        #region Determining which agents received leads from the batch

                        DataTable dtLatentLeadReportData = Insure.INGetLatentLeadsReportData(fkINBatchID, fkUserID);

                        #endregion Determining which agents received leads from the batch

                        if (dtLatentLeadReportData.Rows.Count > 0)
                        {
                            #region Setup the Leads Sheet

                            string strAgent = Methods.ParseWorksheetName(wbLatentLeadsReport, salesConsultant); //drAgent["SalesAgent"].ToString().Length > 31 ? drAgent["SalesAgent"].ToString().Substring(0, 31) : drAgent["SalesAgent"].ToString();
                            Worksheet wsLeads = wbLatentLeadsReport.Worksheets.Add(strAgent);

                            if (isUpgradeCampaign)
                            {
                                CopyWorkSheetOptionsFromTemplate(wsCoverAndLeadTemplate, wsLeads);
                            }
                            else
                            {
                                Methods.CopyWorksheetOptionsFromTemplate(wsLeadTemplate, wsLeads, true, true, true, true, true, true, true, false, true, true, true, true, true, true, true, true, false);
                            }

                            #endregion Setup the Leads Sheet

                            #region Populate Leads Worksheet

                            wsLeads.PrintOptions.StartPageNumber = 1;
                            wsLeads.PrintOptions.PageNumbering = PageNumbering.UseStartPageNumber;
                            wsLeads.PrintOptions.Header = "&L&B" + strAgent + "&C&B" + String.Format("{0} - Conversion Leads", leadBookName) + "&R&BPage &P of &N";
                            wsLeads.PrintOptions.Footer = "&C&B" + nextMonday.ToString("yyyy/MM/dd");

                            //populate leadsheet
                            int[] lead = new int[1]; //array is needed so that the variable can be used in dispatcher
                            int leadRow = 0;
                            int leadsToPrint = Convert.ToInt32(dtLatentLeadReportData.Rows.Count);

                            for (lead[0] = 1; lead[0] <= leadsToPrint; lead[0]++)
                            {
                                wbLatentLeadsReport.NamedReferences.Clear();

                                if (isUpgradeCampaign)
                                {
                                    Methods.CopyExcelRegion(wsCoverAndLeadTemplate, 40, 0, templateLines + 1, leadColumnWidth, wsLeads, leadRow, 0);

                                    #region Setting the values of the cells
                                    wsLeads.GetCell("RefNo").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["RefNo"]);
                                    wsLeads.GetCell("Title").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["Title"]);
                                    wsLeads.GetCell("FirstName").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["FirstName"]);
                                    wsLeads.GetCell("Surname").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["Surname"]);
                                    wsLeads.GetCell("IDNo").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["IDNo"]);
                                    wsLeads.GetCell("Address").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["Address"]);
                                    wsLeads.GetCell("PostalCode").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["PostalCode"]);
                                    wsLeads.GetCell("TelWork").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["TelWork"]);
                                    wsLeads.GetCell("TelHome").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["TelHome"]);
                                    wsLeads.GetCell("TelCell").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["TelCell"]);
                                    wsLeads.GetCell("ContractPremium").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["ContractPremium"]);
                                    wsLeads.GetCell("Offer").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["Offer"]);
                                    wsLeads.GetCell("NewTotalPremium").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["NewTotalPremium"]);
                                    wsLeads.GetCell("DebitDay").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["DebitDay"]);
                                    wsLeads.GetCell("Option").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["Option"]);
                                    wsLeads.GetCell("ChildCover").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["ChildCover"]);
                                    wsLeads.GetCell("LifeAssured2FirstName").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["LifeAssured2FirstName"]);
                                    wsLeads.GetCell("LifeAssured2Surname").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["LifeAssured2Surname"]);
                                    wsLeads.GetCell("LifeAssured2DateOfBirth").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["LifeAssured2DateOfBirth"]);
                                    wsLeads.GetCell("LA1CancerCover").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["LA1CancerCover"]);
                                    wsLeads.GetCell("LA1DisabilityCover").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["LA1DisabilityCover"]);
                                    wsLeads.GetCell("LA1AccidentalDeathCover").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["LA1AccidentalDeathCover"]);
                                    wsLeads.GetCell("LA1FuneralCover").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["LA1FuneralCover"]);
                                    wsLeads.GetCell("LA2CancerCover").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["LA2CancerCover"]);
                                    wsLeads.GetCell("LA2DisabilityCover").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["LA2DisabilityCover"]);
                                    wsLeads.GetCell("LA2AccidentalDeathCover").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["LA2AccidentalDeathCover"]);
                                    wsLeads.GetCell("LA2FuneralCover").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["LA2FuneralCover"]);
                                    wsLeads.GetCell("Beneficiary1FirstName").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["Beneficiary1FirstName"]);
                                    wsLeads.GetCell("Beneficiary1Surname").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["Beneficiary1Surname"]);
                                    wsLeads.GetCell("Beneficiary1DateOfBirth").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["Beneficiary1DateOfBirth"]);
                                    wsLeads.GetCell("Beneficiary1Relationship").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["Beneficiary1Relationship"]);
                                    wsLeads.GetCell("Beneficiary1Percentage").Value = Convert.ToString(dtLatentLeadReportData.Rows[lead[0] - 1]["Beneficiary1Percentage"]);

                                    #endregion Setting the values of the cells

                                    wbLatentLeadsReport.NamedReferences.Clear();
                                }
                                else
                                {
                                    Methods.CopyExcelRegion(wsLeadTemplate, 0, 0, templateLines + 1, leadColumnWidth, wsLeads, leadRow, 0);

                                    for (int currentRow = 0; currentRow <= templateLines; currentRow++)
                                    {

                                        for (int column = 0; column <= 15; column++)
                                        {
                                            WorksheetCell targetCell = wsLeads.Rows[leadRow + currentRow].Cells[column];
                                            switch (currentRow)
                                            {
                                                case 1:
                                                    targetCell.Value = dtLatentLeadReportData.Rows[lead[0] - 1][column].ToString();
                                                    break;

                                                // Force Insure to display
                                                case 3:
                                                    //if (!(campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.Rejuvenation
                                                    //    || campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.Defrosted
                                                    //    || campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.ReDefrost
                                                    //    || campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.Reactivation))
                                                    if (latentLeadReportType == 2)
                                                    {
                                                        targetCell = wsLeads.Rows[leadRow + currentRow].Cells[15];
                                                        targetCell.Value = dtLatentLeadReportData.Rows[lead[0] - 1]["ContractPremium"].ToString();
                                                    }
                                                    break;
                                            }
                                        }

                                        //if (campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.Rejuvenation
                                        //    || campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.Defrosted
                                        //    || campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.ReDefrost
                                        //    || campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.Reactivation)
                                        if (latentLeadReportType == 2)
                                        {
                                            for (int column = 17; column <= 23; column++)
                                            {
                                                WorksheetCell targetCell = null;

                                                switch (column)
                                                {
                                                    case 17:
                                                        targetCell = wsLeads.Rows[leadRow + currentRow + 1].Cells[0];
                                                        break;

                                                    case 18:
                                                        targetCell = wsLeads.Rows[leadRow + currentRow + 1].Cells[4];
                                                        break;

                                                    case 19:
                                                        targetCell = wsLeads.Rows[leadRow + currentRow + 1].Cells[7];
                                                        break;

                                                    case 20:
                                                        targetCell = wsLeads.Rows[leadRow + currentRow + 1].Cells[10];
                                                        break;

                                                    case 21:
                                                        targetCell = wsLeads.Rows[leadRow + currentRow + 1].Cells[12];
                                                        break;

                                                    case 22:
                                                        targetCell = wsLeads.Rows[leadRow + currentRow + 1].Cells[14];
                                                        break;

                                                    case 23:
                                                        targetCell = wsLeads.Rows[leadRow + currentRow + 3].Cells[15];
                                                        break;
                                                }

                                                switch (currentRow)
                                                {
                                                    case 1:
                                                        if (targetCell != null) targetCell.Value = dtLatentLeadReportData.Rows[lead[0] - 1][column].ToString();
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }

                                //update interface
                                leadRow = leadRow + templateLines + verticalSpacingBetweenLeads;
                            }

                            #endregion Populate Leads Worksheet
                        }
                    }

                    #region Save & display Excel document

                    //Embriant.Framework.Data.Database.BeginTransaction(null, IsolationLevel.Snapshot);

                    if (!_inBatch.ContainsLatentLeads.HasValue)
                    {
                        //_inBatch.ContainsLatentLeads = true;
                        //_inBatch.Save(_validationResult);
                        Insure.IndicateBatchAsContainingLatentLeads(fkINBatchID, true);
                    }
                    else if (!_inBatch.ContainsLatentLeads.Value)
                    {
                        //_inBatch.ContainsLatentLeads = true;
                        //_inBatch.Save(_validationResult);
                        Insure.IndicateBatchAsContainingLatentLeads(fkINBatchID, true);
                    }

                    //CommitTransaction(null);

                    wbLatentLeadsReport.Save(filePathAndName);

                    //display excel document
                    Process.Start(filePathAndName);

                    #endregion Save & display Excel document

                }
                else
                {
                    _noDataBatchCount++;
                }

                #endregion Loop through the data table and generate a latent leads workbook for each row - if there is any data
            }

            catch (Exception ex)
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "An error has occurred.\nThe latent lead printing job will now be cancelled.", "Latent Leads Error", ShowMessageType.Error);
                });

                HandleException(ex);
            }
        }

        private void GenerateLatentLeadsWorkbookElite(long fkINBatchID)
        {
            // See

            #region Get data from the database

            DataTable dtBatchAssignees = Insure.INGetLatentLeadsReportBatchAssignees(fkINBatchID);

            if (dtBatchAssignees.Rows.Count == 0)
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    ShowMessageBox(new INMessageBoxWindow1(), @"None of the sales consultants received leads from one of more of the selected batches.", "No Lead Recipients", ShowMessageType.Information);
                });

                return;
            }

            #endregion Get data from the database

            #region Variable Definitions

            long fkINCampaign = _campaignID;
            string campaignName = _campaign["CampaignName"].ToString();
            string campaignCode = _campaign["CampaignCode"].ToString();

            string batchCode = dtBatchAssignees.Rows[0]["BatchCode"].ToString();
            string leadBookName = String.Format("{0} Batch {1}", campaignCode, batchCode);
            string leadBookFileName = String.Format("{0}Conversion Leads - {1} ~ {2}.xlsx",
                    GlobalSettings.UserFolder,
                    leadBookName,
                    DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
            byte latentLeadReportType = Convert.ToByte(dtBatchAssignees.Rows[0]["LatentLeadReportType"]);

            string templateWorkbookName = String.Empty;

            DateTime nextMonday = Methods.NextWeekDay(DateTime.Now, DayOfWeek.Monday);

            #endregion Variable Definitions

            #region Knowing we're only printing for 1 campaign at a time, use row 1 column 1 of dtSelectedItemsToPrint to get the configs

            DataTable dtLeadBookConfiguration = Insure.INGetLeadbookConfigurationByCampaignID(fkINCampaign);
            templateWorkbookName = dtLeadBookConfiguration.Rows[0]["TemplateWorkbookName"].ToString();

            byte summarySheetMaxEntries = Convert.ToByte(dtLeadBookConfiguration.Rows[0]["SummarySheetMaxEntries"]);

            //if (dtSelectedItemsToPrint.Rows.Count > summarySheetMaxEntries)
            //{
            //    bool result = false;

            //    Dispatcher.Invoke(DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate
            //    {
            //        string message = String.Format("You have {0} lead books to print. Only {1} of those items will appear on the summary sheet, even though all {0} lead books will be printed. Would you like to continue printing anyway?",
            //        dtSelectedItemsToPrint.Rows.Count,
            //        summarySheetMaxEntries);
            //        INMessageBoxWindow2 messageBox = new INMessageBoxWindow2();
            //        messageBox.buttonOK.Content = "Yes";
            //        messageBox.buttonCancel.Content = "No";

            //        var showMessageBox = ShowMessageBox(messageBox, message, "Too many items selected", ShowMessageType.Exclamation);
            //        result = showMessageBox != null && (bool)showMessageBox;
            //    });

            //    if (!result)
            //    {
            //        Database.CancelTransactions();
            //        return;
            //    }
            //}

            #endregion Knowing we're only printing for 1 campaign at a time, use row 1 column 1 of dtSelectedItemsToPrint to get the configs

            #region Define the Excel workbooks (the template and the resulting lead book) and which template sheets to use for the summary, cover, actual leads and coversion

            Workbook wbTemplate = Methods.DefineTemplateWorkbook(String.Format("/Templates/{0}", templateWorkbookName));
            Workbook wbPrint = new Workbook(WorkbookFormat.Excel2007);

            #endregion Define the Excel workbooks (the template and the resulting lead book) and which template sheets to use for the summary, cover, actual leads and coversion

            #region Add the summary sheet

            AddSummarySheet(wbTemplate, wbPrint, dtBatchAssignees, dtLeadBookConfiguration.Rows[0], leadBookName);

            #endregion Add the summary sheet

            #region For each selected item, generate the cover, leads and conversion sheets

            foreach (DataRow drAgent in dtBatchAssignees.AsEnumerable())
            {
                #region Add the current sales consultant's cover sheet and leads

                AddIndividualSalesConsultantCoverAndLeads(wbPrint, drAgent, dtLeadBookConfiguration.Rows[0], leadBookName);

                #endregion Add the current sales consultant's cover sheet and leads

                wbPrint.NamedReferences.Clear();
            }
            #endregion For each selected item, generate the cover, leads and conversion sheets

            #region Save and open the resulting workbook - and indicate the batch as having latent leads

            string filePathAndName = leadBookFileName; //String.Format("{0}{1}.xlsx", GlobalSettings.UserFolder, leadBookFileName);

            //See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/209145461/comments#341417643
            if (filePathAndName.Contains(Environment.NewLine))
            {
                filePathAndName = filePathAndName.Replace(Environment.NewLine, "");
            }

            if (wbPrint.Worksheets.Count > 1)
            {
                wbPrint.Save(filePathAndName);
                Process.Start(filePathAndName);

                if (!_inBatch.ContainsLatentLeads.HasValue)
                {
                    Insure.IndicateBatchAsContainingLatentLeads(fkINBatchID, true);
                }
                else if (!_inBatch.ContainsLatentLeads.Value)
                {
                    Insure.IndicateBatchAsContainingLatentLeads(fkINBatchID, true);
                }
            }

            #endregion Save and open the resulting workbook - and indicate the batch as having latent leads
        }

        private void AddSummarySheet(Workbook wbTemplate, Workbook wbPrint, DataTable dtSelectedItemsToPrint, DataRow drLeadBookConfiguration, string heading)
        {
            #region Variable declarations and initializations

            string summaryTemplateWorksheetName = drLeadBookConfiguration["SummaryTemplateWorksheetName"].ToString();
            byte summaryTemplateRowSpan = Convert.ToByte(drLeadBookConfiguration["SummaryTemplateRowSpan"]);
            byte summaryTemplateColumnSpan = Convert.ToByte(drLeadBookConfiguration["SummaryTemplateColumnSpan"]);
            byte summarySheetMaxEntries = Convert.ToByte(drLeadBookConfiguration["SummarySheetMaxEntries"]);

            byte summarySheetEntryTemplateRowSpan = Convert.ToByte(drLeadBookConfiguration["SummarySheetEntryTemplateRowSpan"]);
            byte summarySheetEntryTemplateColumnSpan = Convert.ToByte(drLeadBookConfiguration["SummarySheetEntryTemplateColumnSpan"]);

            byte summarySheetEntryTemplateOriginRow = Convert.ToByte(drLeadBookConfiguration["SummarySheetEntryTemplateOriginRow"]);
            byte summarySheetEntryTemplateOriginColumn = Convert.ToByte(drLeadBookConfiguration["SummarySheetEntryTemplateOriginColumn"]);

            int summarySheetRowIndex = summarySheetEntryTemplateOriginRow;
            int rowCount = 0;

            #endregion Variable declarations and initializations

            #region Defining the summary worksheet

            Worksheet wsSummary = wbPrint.Worksheets.Add("Summary");
            Worksheet wsSummaryTemplate = wbTemplate.Worksheets[summaryTemplateWorksheetName];
            Methods.CopyWorksheetOptionsFromTemplate(wsSummaryTemplate, wsSummary);
            wsSummary.PrintOptions.Header = "&C&B" + String.Format("{0} - Conversion Leads", heading);
            wsSummary.GetCell("B2").Value = "Conversion Leads Summary";
            wsSummary.GetCell("E4").Value = "Most Recent Allocation Date";

            #endregion Defining the summary worksheet

            #region Apply the entire summary sheet template formatting to the newly-added sheet

            //wbPrint.NamedReferences.Clear();
            Methods.CopyExcelRegion(wsSummaryTemplate, 0, 0, summaryTemplateRowSpan, summaryTemplateColumnSpan, wsSummary, 0, 0);
            wbPrint.NamedReferences.Clear();

            #endregion Apply the entire summary sheet template formatting to the newly-added sheet

            #region Add the contents

            foreach (DataRow row in dtSelectedItemsToPrint.Rows)
            {
                rowCount++;
                if (rowCount <= summarySheetMaxEntries)
                {

                    Methods.CopyExcelRegion(wsSummaryTemplate,
                        summarySheetEntryTemplateOriginRow,
                        summarySheetEntryTemplateOriginColumn,
                        summarySheetEntryTemplateRowSpan,
                        summarySheetEntryTemplateColumnSpan,
                        wsSummary,
                        summarySheetRowIndex - 1,
                        summarySheetEntryTemplateOriginColumn);

                    wsSummary.GetCell(String.Format("B{0}", summarySheetRowIndex)).Value = row["SalesConsultantFirstName"];
                    wsSummary.GetCell(String.Format("C{0}", summarySheetRowIndex)).Value = row["SalesConsultantSurname"];
                    wsSummary.GetCell(String.Format("D{0}", summarySheetRowIndex)).Value = row["BatchCode"];
                    wsSummary.GetCell(String.Format("E{0}", summarySheetRowIndex)).Value = (Convert.ToDateTime(row["MostRecentAllocationDate"])).ToString("yyyy/MM/dd");
                    wsSummary.GetCell(String.Format("F{0}", summarySheetRowIndex)).Value = row["LatentLeadCount"];

                    //rowCount++;
                    summarySheetRowIndex++;
                }
                else
                {
                    continue;
                }

                wbPrint.NamedReferences.Clear();
            }

            #endregion Add the contents
        }

        private void AddIndividualSalesConsultantCoverAndLeads(Workbook wbPrint, DataRow drSelectedEntryPrint, DataRow drLeadBookConfiguration, string header)
        {
            #region Variable declarations and initializations

            // TODO: I had to hard-code this, because with redeemed gifts, lead books are configured by batch and not per campaign. ARGHHHHH!!!!!
            string templateWorkbookName = drLeadBookConfiguration["TemplateWorkbookName"].ToString();
            bool isRedeemedGiftBatch = Convert.ToBoolean(drSelectedEntryPrint["IsRedeemedGiftBatch"]);

            string campaignCode = drSelectedEntryPrint["CampaignCode"].ToString();
            if ((campaignCode == "PLCBE") ||
                (campaignCode == "PLFDB") ||
                (campaignCode == "PLMMBE"))
            {
                if (isRedeemedGiftBatch)
                {
                    templateWorkbookName = "PrintTemplateEliteRedeemedGifts.xlsx";
                }
                else
                {
                    templateWorkbookName = "PrintTemplateEliteNonRedeemedGifts.xlsx";
                }
            }
            else
            {
                templateWorkbookName = "PrintTemplateElite.xlsx";
            }

            Workbook wbTemplate = Methods.DefineTemplateWorkbook(String.Format("/Templates/{0}", templateWorkbookName));

            long fkINBatchID = Convert.ToInt64(drSelectedEntryPrint["FKINBatchID"]);
            long fkUserID = Convert.ToInt64(drSelectedEntryPrint["FKUserID"]);
            DateTime mostRecentAllocationDate = Convert.ToDateTime(drSelectedEntryPrint["MostRecentAllocationDate"]);
            DateTime nextMonday = Methods.NextWeekDay(mostRecentAllocationDate, DayOfWeek.Monday);

            //// Cover-Specific Configuration Values
            //string coverTemplateWorksheetName = drLeadBookConfiguration["CoverTemplateWorksheetName"].ToString();
            //byte coverTemplateColumnSpan = Convert.ToByte(drLeadBookConfiguration["CoverTemplateColumnSpan"]);
            //byte coverTemplateRowSpan = Convert.ToByte(drLeadBookConfiguration["CoverTemplateRowSpan"]);
            //byte coverOriginRow = Convert.ToByte(drLeadBookConfiguration["CoverOriginRow"]);
            //byte coverOriginColumn = Convert.ToByte(drLeadBookConfiguration["CoverOriginColumn"]);
            //byte coverFirstValueRowIndex = Convert.ToByte(drLeadBookConfiguration["CoverFirstValueRowIndex"]);

            // Lead-Specific Configuration Values
            string leadTemplateWorksheetName = drLeadBookConfiguration["LeadTemplateWorksheetName"].ToString();
            byte leadTemplateRowSpan = Convert.ToByte(drLeadBookConfiguration["LeadTemplateRowSpan"]);
            byte leadTemplateColumnSpan = Convert.ToByte(drLeadBookConfiguration["LeadTemplateColumnSpan"]);
            byte leadTemplateOriginColumn = Convert.ToByte(drLeadBookConfiguration["LeadTemplateOriginColumn"]);
            byte leadTemplateOriginRow = Convert.ToByte(drLeadBookConfiguration["LeadTemplateOriginRow"]);

            byte leadsPerPage = Convert.ToByte(drLeadBookConfiguration["LeadsPerPage"]);
            byte pageBreakRowIndex = Convert.ToByte(drLeadBookConfiguration["PageBreakRowIndex"]);
            //byte firstLeadRowIndex = Convert.ToByte(drLeadBookConfiguration["FirstLeadRowIndex"]);
            byte verticalSpacingBetweenLeads = Convert.ToByte(drLeadBookConfiguration["VerticalSpacingBetweenLeads"]);
            int additionalRowsToAddAfterBottomLeadOnSheet = Convert.ToInt16(drLeadBookConfiguration["AdditionalRowsToAddAfterBottomLeadOnSheet"]);

            string salesAgent = String.Format("{0} {1}", drSelectedEntryPrint["SalesConsultantFirstName"].ToString().Trim(), drSelectedEntryPrint["SalesConsultantSurname"].ToString().Trim());
            string leadsWorksheetName = Methods.ParseWorksheetName(wbPrint, salesAgent);

            int rowIndex = 0; //firstLeadRowIndex;
            int insertedLeadCount = 0;

            #endregion Variable declarations and initializations

            #region Defining the cover worksheet

            //Worksheet wsCoverTemplate = wbTemplate.Worksheets[coverTemplateWorksheetName];
            Worksheet wsLeadTemplate = wbTemplate.Worksheets[leadTemplateWorksheetName];
            Worksheet wsLeads = wbPrint.Worksheets.Add(leadsWorksheetName);

            wsLeads.PrintOptions.StartPageNumber = 1;
            wsLeads.PrintOptions.PageNumbering = PageNumbering.UseStartPageNumber;
            wsLeads.PrintOptions.Header = "&L&B" + salesAgent + "&C&B" + String.Format("{0} - Conversion Leads", header) + "&R&BPage &P of &N";
            wsLeads.PrintOptions.Footer = "&C&B" + nextMonday.ToString("yyyy/MM/dd");
            //wsLeads.PrintOptions.PageNumbering = PageNumbering.UseStartPageNumber;
            //wsLeads.PrintOptions.Header = "&L&B" + salesAgent + "&C&B" + header.Replace("#DATE#", (Convert.ToDateTime(drSelectedEntryPrint["MostRecentAllocationDate"])).ToString("yyyy-MM-dd")) + "&R&BPage &P of &N";
            //wsLeads.PrintOptions.Footer = "&C&B" + (Convert.ToDateTime(drSelectedEntryPrint["SalesStartDate"])).ToString("yyyy/MM/dd");

            #endregion Defining the cover worksheet

            #region Apply the entire summary sheet template formatting to the newly-added sheet

            Methods.CopyWorksheetOptionsFromTemplate(wsLeadTemplate, wsLeads, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, true);

            #endregion Apply the entire summary sheet template formatting to the newly-added sheet

            #region Get the lead data from the database

            DataTable dtLeadData = Insure.INGetLatentLeadsReportData(fkINBatchID, fkUserID);

            #endregion Get the lead data from the database

            #region Insert the leads

            foreach (DataRow row in dtLeadData.Rows)
            {
                Methods.CopyExcelRegion(wsLeadTemplate, leadTemplateOriginRow, leadTemplateOriginColumn, leadTemplateRowSpan, leadTemplateColumnSpan, wsLeads, rowIndex, leadTemplateOriginColumn);
                long fkINImportID = Convert.ToInt64(row["ImportID"]);

                foreach (DataColumn column in dtLeadData.Columns)
                {
                    // Omissions:
                    if (column.ColumnName.Trim() != "ImportID")
                    {
                        wsLeads.GetCell(column.ColumnName).Value = row[column.ColumnName];
                    }
                }

                insertedLeadCount++;
                //lblLeadNo.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { lblLeadNo.Text = String.Format("Printing Lead #: {0}", insertedLeadCount); }));

                wbPrint.NamedReferences.Clear();
                rowIndex = rowIndex + leadTemplateRowSpan + verticalSpacingBetweenLeads;

                if ((insertedLeadCount % leadsPerPage) == 0)
                {
                    rowIndex += additionalRowsToAddAfterBottomLeadOnSheet;
                }
            }

            #endregion Insert the leads

        }

        #endregion Conversion Batches - Related Functionalities

        public void CopyWorkSheetOptionsFromTemplate(Worksheet wsTemplate, Worksheet wsTarget)
        {
            wsTarget.DisplayOptions.ShowGridlines = false;
            wsTarget.PrintOptions.PaperSize = wsTemplate.PrintOptions.PaperSize; //PaperSize.A4;
            wsTarget.PrintOptions.Orientation = wsTemplate.PrintOptions.Orientation;  //Orientation.Landscape;
            wsTarget.PrintOptions.LeftMargin = wsTemplate.PrintOptions.LeftMargin;
            wsTarget.PrintOptions.TopMargin = wsTemplate.PrintOptions.TopMargin;
            wsTarget.PrintOptions.RightMargin = wsTemplate.PrintOptions.RightMargin;
            wsTarget.PrintOptions.BottomMargin = wsTemplate.PrintOptions.BottomMargin;
            wsTarget.PrintOptions.FooterMargin = wsTemplate.PrintOptions.FooterMargin;
            wsTarget.PrintOptions.HeaderMargin = wsTemplate.PrintOptions.HeaderMargin;
            wsTarget.PrintOptions.ScalingType = wsTemplate.PrintOptions.ScalingType;
            wsTarget.PrintOptions.ScalingFactor = wsTemplate.PrintOptions.ScalingFactor;

            wsTarget.PrintOptions.CenterHorizontally = true;
            //ws.PrintOptions.ScalingType = ScalingType.UseScalingFactor;
            //ws.PrintOptions.ScalingFactor = 60;
            wsTarget.DisplayOptions.MagnificationInPageLayoutView = 100;
            wsTarget.DisplayOptions.View = WorksheetView.PageLayout;
            wsTarget.DefaultRowHeight = 240;

        }

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;
            btnLeadStatusReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnLeadStatusReport.ToolTip = btnLeadStatusReport.Content;
        }

        private void Timer2(object sender, EventArgs e)
        {
            _timer2++;
            btnLatentLeadsReport.Content = TimeSpan.FromSeconds(_timer2).ToString();
            btnLatentLeadsReport.ToolTip = btnLatentLeadsReport.Content;
        }

        private bool HasAllInputParametersBeenSpecified()
        {

            if (cmbCampaign.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select a campaign.", "No campaign selected", ShowMessageType.Error);
                EnableAllControls(true);
                return false;
            }

            var lstTemp = (from r in xdgBatches.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            _lstSelectedBatches = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Batch"].Value));

            if (_lstSelectedBatches.Count == 0)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 batch from the list.", "No batches selected", ShowMessageType.Error);
                return false;
            }

            return true;

        }

        #endregion Private Methods

        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void btnLeadStatusReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EnableAllControls(false);

                if (!HasAllInputParametersBeenSpecified())
                {
                    EnableAllControls(true);
                    return;
                }

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += LeadStatusReport;
                worker.RunWorkerCompleted += LeadStatusReportCompleted;
                worker.RunWorkerAsync();

                dispatcherTimer1.Start();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnLatentLeadsReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EnableAllControls(false);

                if (!HasAllInputParametersBeenSpecified())
                {
                    EnableAllControls(true);
                    return;
                }

                _noDataBatchCount = 0;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += LatentLeadsReport;
                worker.RunWorkerCompleted += LatentLeadsReportCompleted;
                worker.RunWorkerAsync();

                dispatcherTimer2.Start();                
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }



        #region XamDataGrid - Related Event Handlers

        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = ((DataView)xdgBatches.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = true;
                }

                EnableDisableExportButton();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = ((DataView)xdgBatches.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = false;
                }

                EnableDisableExportButton();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_xdgHeaderPrefixAreaCheckbox != null)
                {
                    _xdgHeaderPrefixAreaCheckbox.IsChecked = AllRecordsSelected();
                }

                EnableDisableExportButton();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _xdgHeaderPrefixAreaCheckbox = (CheckBox)sender;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        #endregion XamDataGrid - Related Event Handlers

        #region ComboBox - Related Event Handlers

        private void cmbCampaign_Loaded(object sender, RoutedEventArgs e)
        {
            cmbCampaign.Focus();
        }

        private void cmbCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _campaignID = Convert.ToInt64(cmbCampaign.SelectedValue);
            _campaign = (cmbCampaign.SelectedItem as DataRowView).Row;

            try
            {
                if (cmbCampaign.SelectedIndex > -1)
                {
                    SetCursor(Cursors.Wait);

                    DataTable dt = Methods.GetTableData(String.Format("SELECT [ID], [Code] AS [Batch] FROM [INBatch] WHERE [FKINCampaignID] = {0} AND [Code] NOT LIKE 'MM%' ORDER BY [Code] DESC", cmbCampaign.SelectedValue));
                    DataColumn column = new DataColumn("Select", typeof(bool));
                    column.DefaultValue = false;
                    dt.Columns.Add(column);
                    //dt.DefaultView.Sort = "UDMBatchCode ASC";

                    xdgBatches.DataSource = dt.DefaultView;
                }
                else
                {
                    xdgBatches.DataSource = null;
                }

            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SetCursor(Cursors.Arrow);
            }
        }

        #endregion ComboBox - Related Event Handlers

        #endregion Event Handlers
        
    }
}
