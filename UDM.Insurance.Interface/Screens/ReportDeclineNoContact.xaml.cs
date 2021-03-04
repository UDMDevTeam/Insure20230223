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
    /// <summary>
    /// Interaction logic for ReportLeadStatusScreen.xaml
    /// </summary>
    public partial class ReportDeclineNoContact
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
        private bool? _reportContentOnSinglePageSheet;
        //string _batchIDs;
        //private DateTime _fromDate = DateTime.Now;
        //private DateTime _toDate = DateTime.Now;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        #endregion Private Members

        #region Constructors

        public ReportDeclineNoContact()
        {
            InitializeComponent();
            LoadCampaignInfo();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

        private void EnableAllControls(bool isEnabled)
        {
            btnReport.IsEnabled = isEnabled;
            btnClose.IsEnabled = isEnabled;
            cmbCampaign.IsEnabled = isEnabled;
            xdgBatches.IsEnabled = isEnabled;
            //xdgCampaigns.IsEnabled = true;
            //Cal1.IsEnabled = true;
            //Cal2.IsEnabled = true;
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

                //DataTable dt = Methods.GetTableData("SELECT [ID] AS [CampaignID], [Name] AS [CampaignName], [Code] AS [CampaignCode] FROM [INCampaign] ORDER BY [Name] ASC");

                //DataSet dsLookups = UDM.Insurance.Business.Insure.INGetSalaryReportScreenLookups(2);
                UDM.Insurance.Interface.Data.CommonControlData.PopulateCampaignComboBox(cmbCampaign, true /*dsLookups.Tables[1]*/);

                //DataTable dt = 
                //cmbCampaign.Populate(dt, "CampaignName", "CampaignID");
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
                    btnReport.IsEnabled = true;
                    return;
                }
                btnReport.IsEnabled = false;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        #region Report Operations Variant 1: Each batch on a separate sheet 

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Declarations 


                long campaignID = Convert.ToInt32(_campaign.ItemArray[0]);
                string campaignName = _campaign.ItemArray[1].ToString();
                string reportSubtitle = String.Empty;

                #endregion Declarations

                #region Setup excel documents

                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                string filePathAndName = String.Format("{0}No Contact Report ({1}), {2}.xlsx", GlobalSettings.UserFolder, campaignName, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                Uri uri = new Uri("/Templates/ReportTemplateNoContact.xlsx", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    wbTemplate = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }

                Worksheet wsTemplate = wbTemplate.Worksheets["Revised"];
                Worksheet wsNewReportSheet = null;

                //Worksheet wsReport = wbReport.Worksheets.Add(campaignName);
                //worksheetCount++;

                #endregion Setup excel documents

                foreach (DataRecord record in _lstSelectedBatches)
                {
                    #region Get report data from database

                    //long batchID = Convert.ToInt64(record.Cells["ID"].Value);
                    string batchCode = record.Cells["Batch"].Value.ToString();
                    //long campaignID;
                    //Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    //{
                    //    campaignID = Convert.ToInt64(cmbCampaign.SelectedValue);

                    //});
                    DataTable dtAllReportData = UDM.Insurance.Business.Insure.INGetReportNoContactReportData(batchCode, campaignID);
                    DataTable dtNoContactReportData = new DataTable();
                    DataTable dtDeclinedUpdatedDetailsData = new DataTable();
                    if (dtAllReportData.AsEnumerable().Where(x => Convert.ToBoolean(x["IsDecline"]) != true).Count() > 0)
                    {
                        dtNoContactReportData = dtAllReportData.AsEnumerable().Where(x => Convert.ToBoolean(x["IsDecline"]) != true).CopyToDataTable();
                    }
                    if (dtAllReportData.AsEnumerable().Where(x => Convert.ToBoolean(x["IsDecline"]) == true).Count() > 0)
                    {
                        dtDeclinedUpdatedDetailsData = dtAllReportData.AsEnumerable().Where(x => Convert.ToBoolean(x["IsDecline"]) == true).CopyToDataTable();
                    }




                    #endregion Get report data from database

                    //AddNewWorkSheet(wsNewReportSheet, wsTemplate, wbReport, record, dtNoContactReportData, false);

                    AddNewWorkSheet(wsNewReportSheet, wsTemplate, wbReport, record, dtDeclinedUpdatedDetailsData, true);



                }

                if (wbReport.Worksheets.Count > 0)
                {
                    //Save excel document
                    wbReport.Save(filePathAndName);

                    //Display excel document
                    Process.Start(filePathAndName);
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), String.Format("None of the selected batches of the {0} campaign yielded any results.", campaignName), "No Data", ShowMessageType.Information);
                    });
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

        public void AddNewWorkSheet(Worksheet wsNewReportSheet, Worksheet wsTemplate, Workbook wbReport, DataRecord record, DataTable dtNoContactReportData, bool isDeclinedSheet)
        {
            int reportRow = 0;
            int reportTemplateRowIndex = 0;
            string batchDescription = record.Cells["Batch"].Value.ToString();
            batchDescription = (batchDescription.Replace(".2", "")).Replace(".3", "");
            if (isDeclinedSheet)
            {
                batchDescription = batchDescription + "_DeclinedUpdatedDetails";
            }
            wsNewReportSheet = wbReport.Worksheets.Add(Methods.ParseWorksheetName(wbReport, batchDescription));

            #region Report Data

            if (dtNoContactReportData.Rows.Count > 0)
            {
                #region Adding a new worksheet for the current batch and copy the options

                reportRow = 0;

                Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsNewReportSheet, true, true, true, true, false, false, false, false, true, true, true, true, true, true, true, true, true);
                Methods.CopyExcelRegion(wsTemplate, 0, 0, 3, 85, wsNewReportSheet, reportRow, 0);
                reportRow = 4;

                #endregion Adding a new worksheet for the current batch and copy the options

                for (int i = 0; i < dtNoContactReportData.Rows.Count; i++)
                {
                    #region Copy the template formatting, based on the number of rows in the data table, and set the report template row index

                    #region The data table only contains 1 row

                    if (dtNoContactReportData.Rows.Count == 1)
                    {
                        reportTemplateRowIndex = 7;
                    }

                    #endregion The data table only contains 1 row

                    #region The data table contains 2 rows

                    else if (dtNoContactReportData.Rows.Count == 2)
                    {
                        if (i == 1)
                        {
                            reportTemplateRowIndex = 10;
                        }
                        else
                        {
                            reportTemplateRowIndex = 11;
                        }
                    }

                    #endregion The data table contains 2 rows

                    #region The data table contains 3 rows

                    else if (dtNoContactReportData.Rows.Count == 3)
                    {
                        if (i == 0)
                        {
                            reportTemplateRowIndex = 17;
                        }
                        else if (i == 1)
                        {
                            reportTemplateRowIndex = 18;
                        }
                        else
                        {
                            reportTemplateRowIndex = 19;
                        }
                    }

                    #endregion The data table contains 3 rows

                    #region The data table contains 3 or more rows

                    else if (dtNoContactReportData.Rows.Count > 3)
                    {
                        if (i == 0)
                        {
                            reportTemplateRowIndex = 17;
                        }
                        else if ((i > 0) && (i < dtNoContactReportData.Rows.Count - 1))
                        {
                            reportTemplateRowIndex = 18;
                        }
                        else
                        {
                            reportTemplateRowIndex = 19;
                        }
                    }

                    #endregion The data table contains 3 or more rows

                    Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, 85, wsNewReportSheet, reportRow, 0);

                    #endregion Copy the template formatting, based on the number of rows in the data table, and set the report template row index

                    #region Populate the report values

                    wsNewReportSheet.GetCell(String.Format("A{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["CampaignName"];
                    wsNewReportSheet.GetCell(String.Format("B{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["CampaignCode"];
                    wsNewReportSheet.GetCell(String.Format("C{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["BatchCode"];
                    wsNewReportSheet.GetCell(String.Format("D{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["UDMBatchCode"];
                    wsNewReportSheet.GetCell(String.Format("E{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["ImportDate"];
                    wsNewReportSheet.GetCell(String.Format("F{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["ReferenceNumber"];
                    wsNewReportSheet.GetCell(String.Format("G{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["GiftReceived"];
                    wsNewReportSheet.GetCell(String.Format("H{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["AllocatedTo"];
                    wsNewReportSheet.GetCell(String.Format("I{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["AllocationDate"];
                    wsNewReportSheet.GetCell(String.Format("J{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["IsPrinted"];
                    wsNewReportSheet.GetCell(String.Format("K{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["LeadFeedback"];
                    wsNewReportSheet.GetCell(String.Format("L{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["SalesConsultant"];
                    wsNewReportSheet.GetCell(String.Format("M{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Workstation"];
                    wsNewReportSheet.GetCell(String.Format("N{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Date"];
                    //wsNewReportSheet.GetCell(String.Format("O{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Time"];
                    wsNewReportSheet.GetCell(String.Format("O{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Time"] != DBNull.Value ? dtNoContactReportData.Rows[i]["Time"].ToString() : null;
                    wsNewReportSheet.GetCell(String.Format("P{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["ContactNumber"];
                    wsNewReportSheet.GetCell(String.Format("Q{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Title"];
                    wsNewReportSheet.GetCell(String.Format("R{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Intials"];
                    wsNewReportSheet.GetCell(String.Format("S{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["FirstName"];
                    wsNewReportSheet.GetCell(String.Format("T{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["LastName"];
                    wsNewReportSheet.GetCell(String.Format("U{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["IDNumber"];
                    wsNewReportSheet.GetCell(String.Format("V{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["DateOfBirth"];
                    wsNewReportSheet.GetCell(String.Format("W{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["WorkPhone"];
                    wsNewReportSheet.GetCell(String.Format("X{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["HomePhone"];
                    wsNewReportSheet.GetCell(String.Format("Y{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["CellPhone"];
                    wsNewReportSheet.GetCell(String.Format("Z{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Address"];
                    wsNewReportSheet.GetCell(String.Format("AA{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Address1"];
                    wsNewReportSheet.GetCell(String.Format("AB{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Address2"];
                    wsNewReportSheet.GetCell(String.Format("AC{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Address3"];
                    wsNewReportSheet.GetCell(String.Format("AD{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Address4"];
                    wsNewReportSheet.GetCell(String.Format("AE{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["AreaCode"];
                    wsNewReportSheet.GetCell(String.Format("AF{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Email"];
                    wsNewReportSheet.GetCell(String.Format("AG{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["ReferrorName"];
                    wsNewReportSheet.GetCell(String.Format("AH{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["ReferrorRelationship"];
                    wsNewReportSheet.GetCell(String.Format("AI{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["LifeAssured2DateOfBirth"];
                    wsNewReportSheet.GetCell(String.Format("AJ{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["LifeAssured2Initials"];
                    wsNewReportSheet.GetCell(String.Format("AK{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["LifeAssured2FirstName"];
                    wsNewReportSheet.GetCell(String.Format("AL{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["LifeAssured2Surname"];
                    wsNewReportSheet.GetCell(String.Format("AM{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary1Title"];
                    wsNewReportSheet.GetCell(String.Format("AN{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary1FirstName"];
                    wsNewReportSheet.GetCell(String.Format("AO{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary1Surname"];
                    wsNewReportSheet.GetCell(String.Format("AP{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary1DateOfBirth"];
                    wsNewReportSheet.GetCell(String.Format("AQ{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary1Gender"];
                    wsNewReportSheet.GetCell(String.Format("AR{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary1Relationship"];
                    wsNewReportSheet.GetCell(String.Format("AS{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary1Percentage"];
                    wsNewReportSheet.GetCell(String.Format("AT{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary2Title"];
                    wsNewReportSheet.GetCell(String.Format("AU{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary2FirstName"];
                    wsNewReportSheet.GetCell(String.Format("AV{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary2Surname"];
                    wsNewReportSheet.GetCell(String.Format("AW{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary2DateOfBirth"];
                    wsNewReportSheet.GetCell(String.Format("AX{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary2Gender"];
                    wsNewReportSheet.GetCell(String.Format("AY{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary2Relationship"];
                    wsNewReportSheet.GetCell(String.Format("AZ{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary2Percentage"];
                    wsNewReportSheet.GetCell(String.Format("BA{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary3Title"];
                    wsNewReportSheet.GetCell(String.Format("BB{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary3FirstName"];
                    wsNewReportSheet.GetCell(String.Format("BC{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary3Surname"];
                    wsNewReportSheet.GetCell(String.Format("BD{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary3DateOfBirth"];
                    wsNewReportSheet.GetCell(String.Format("BE{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary3Gender"];
                    wsNewReportSheet.GetCell(String.Format("BF{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary3Relationship"];
                    wsNewReportSheet.GetCell(String.Format("BG{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary3Percentage"];
                    wsNewReportSheet.GetCell(String.Format("BH{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary4Title"];
                    wsNewReportSheet.GetCell(String.Format("BI{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary4FirstName"];
                    wsNewReportSheet.GetCell(String.Format("BJ{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary4Surname"];
                    wsNewReportSheet.GetCell(String.Format("BK{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary4DateOfBirth"];
                    wsNewReportSheet.GetCell(String.Format("BL{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary4Gender"];
                    wsNewReportSheet.GetCell(String.Format("BM{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary4Relationship"];
                    wsNewReportSheet.GetCell(String.Format("BN{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Beneficiary4Percentage"];
                    wsNewReportSheet.GetCell(String.Format("BO{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["PolicyID"];
                    wsNewReportSheet.GetCell(String.Format("BP{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["PolicyNotes"];
                    wsNewReportSheet.GetCell(String.Format("BQ{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["OriginalCommenceDate"];
                    wsNewReportSheet.GetCell(String.Format("BR{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["CommencementDate"];
                    wsNewReportSheet.GetCell(String.Format("BS{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["ChildCover"];
                    wsNewReportSheet.GetCell(String.Format("BT{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["DebitDay"];
                    wsNewReportSheet.GetCell(String.Format("BU{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Offer"];
                    wsNewReportSheet.GetCell(String.Format("BV{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["Premium"];
                    wsNewReportSheet.GetCell(String.Format("BW{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["TotalPremium"];
                    wsNewReportSheet.GetCell(String.Format("BX{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["LifeAssured1AccidentalDeathCover"];
                    wsNewReportSheet.GetCell(String.Format("BY{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["LifeAssured1DisabilityCover"];
                    wsNewReportSheet.GetCell(String.Format("BZ{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["LifeAssured1FuneralCover"];
                    wsNewReportSheet.GetCell(String.Format("CA{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["LifeAssured2AccidentalDeathCover"];
                    wsNewReportSheet.GetCell(String.Format("CB{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["LifeAssured2DisabilityCover"];
                    wsNewReportSheet.GetCell(String.Format("CC{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["LifeAssured2FuneralCover"];
                    //NOK
                    wsNewReportSheet.GetCell(String.Format("CD{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["NOKFirstName"];
                    wsNewReportSheet.GetCell(String.Format("CE{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["NOKSurname"];
                    wsNewReportSheet.GetCell(String.Format("CF{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["NOKRelationship"];
                    wsNewReportSheet.GetCell(String.Format("CG{0}", reportRow + 1)).Value = dtNoContactReportData.Rows[i]["NOKTelContact"];

                    reportRow++;

                    #endregion Populate the report values
                }

            }
            #endregion Report Data
        }

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";

            EnableAllControls(true);
        }

        #endregion Report Operations Variant 1: Each batch on a separate sheet

        #region Report Operations Variant 2: All batches on a single sheet

        private void ReportSingleSheet(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                int rowIndex = 0;
                int worksheetCount = 0;
                long campaignID = Convert.ToInt32(_campaign.ItemArray[0]);
                string campaignName = _campaign.ItemArray[1].ToString();
                string campaignDescription = String.Format("{0} ({1})", _campaign.ItemArray[1], _campaign.ItemArray[2]);
                string reportSubtitle = String.Empty; //String.Format(@"{0} - {1} / {2}", );

                DataTable dtLeadStatusData;

                #region Setup excel documents

                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                string filePathAndName = String.Format("{0}Lead Search Report ({1}), {2}.xlsx", GlobalSettings.UserFolder, campaignName, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                Uri uri = new Uri("/Templates/ReportTemplateLeadSearch.xlsx", UriKind.Relative);
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
                WorksheetMergedCellsRegion mergedRegion;
                //Worksheet wsReport = wbReport.Worksheets.Add(campaignName);
                //worksheetCount++;

                wsReport = wbReport.Worksheets.Add("Lead Search Report");
                worksheetCount++;

                #endregion Setup excel documents

                #region Add the report heading from the report template - including the next 4 rows

                Methods.CopyExcelRegion(wsTemplate, rowIndex, 0, 4, 17, wsReport, rowIndex, 0);

                #endregion Add the report heading from the report template - including the next 3 rows

                #region Add the report date

                rowIndex += 2;
                mergedRegion = wsReport.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 17);
                mergedRegion.CellFormat.BottomBorderStyle = CellBorderLineStyle.None;
                mergedRegion.CellFormat.LeftBorderStyle = CellBorderLineStyle.None;
                mergedRegion.CellFormat.RightBorderStyle = CellBorderLineStyle.None;
                mergedRegion.CellFormat.TopBorderStyle = CellBorderLineStyle.None;

                mergedRegion.Value = String.Format("Report Date: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                mergedRegion.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                mergedRegion.CellFormat.Alignment = HorizontalCellAlignment.Left;
                mergedRegion.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                mergedRegion.CellFormat.WrapText = ExcelDefaultableBoolean.True;

                rowIndex++;
                Methods.CopyExcelRegion(wsTemplate, 5, 0, 0, 17, wsReport, rowIndex, 0);
                rowIndex++;

                #endregion Add the report date


                // Loop through each selected batch
                foreach (DataRecord record in _lstSelectedBatches)
                {
                    #region Get report data from database

                    long batchID = Convert.ToInt64(record.Cells["ID"].Value);
                    string batchDescription = record.Cells["Batch"].Value.ToString();

                    reportSubtitle = String.Format(@"{0} - {1} / {2}", campaignName, record.Cells["UDMCode"].Value, record.Cells["Code"].Value);

                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@CampaignID", campaignID);
                    parameters[1] = new SqlParameter("@BatchID", batchID);

                    DataSet dsLeadStatusData = Methods.ExecuteStoredProcedure("spINLeadSearchReport", parameters);

                    #endregion Get report data from database

                    if (dsLeadStatusData.Tables.Count < 1)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), String.Format("There is no data for the {0} campaign.", campaignName), "No Data", ShowMessageType.Information);
                        });
                    }
                    else
                    {
                        #region Report Data

                        dtLeadStatusData = dsLeadStatusData.Tables[0];

                        if (dtLeadStatusData.Rows.Count > 0)
                        {
                            //rowIndex = 7;

                            //wsReport = wbReport.Worksheets.Add(batchDescription);
                            //worksheetCount++;

                            //Methods.CopyExcelRegion(wsTemplate, 0, 0, 6, 17, wsReport, 0, 0);

                            #region Adding the details

                            //wsReport.GetCell("A3").Value = reportSubtitle;
                            //wsReport.GetCell("A4").Value = String.Format("Report Date: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            #region Adding the report subtitle

                            //mergedRegion = wsReport.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 16);
                            //mergedRegion.CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
                            //mergedRegion.CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            //mergedRegion.CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            //mergedRegion.CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;

                            //mergedRegion.Value = reportSubtitle;

                            //mergedRegion.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                            //mergedRegion.CellFormat.Alignment = HorizontalCellAlignment.Center;
                            //mergedRegion.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                            //mergedRegion.CellFormat.WrapText = ExcelDefaultableBoolean.True;

                            //mergedRegion.CellFormat.Font.Height = 280;

                            //rowIndex += 2;

                            #endregion Adding the report subtitle

                            #region Adding the column headings from the report template

                            //Methods.CopyExcelRegion(wsTemplate, 5, 0, 0, 17, wsReport, rowIndex, 0);
                            //++rowIndex;

                            #endregion Adding the column headings from the report template

                            #endregion Adding the details

                            foreach (DataRow dr in dtLeadStatusData.Rows)
                            {
                                rowIndex++;

                                Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, 17, wsReport, rowIndex - 1, 0);

                                wsReport.GetCell("A" + rowIndex).Value = dr["Batch Number"].ToString();
                                wsReport.GetCell("B" + rowIndex).Value = dr["PL Reference Number"].ToString();
                                wsReport.GetCell("C" + rowIndex).Value = dr["Name"].ToString();
                                wsReport.GetCell("D" + rowIndex).Value = dr["Surname"].ToString();
                                wsReport.GetCell("E" + rowIndex).Value = dr["Sale Status"].ToString();
                                wsReport.GetCell("F" + rowIndex).Value = dr["Allocation Date"].ToString();
                                wsReport.GetCell("G" + rowIndex).Value = dr["Date of Sale"].ToString();
                                wsReport.GetCell("H" + rowIndex).Value = dr["Original Premium Sold"].ToString();
                                wsReport.GetCell("I" + rowIndex).Value = dr["Final Premium Sold"].ToString();
                                wsReport.GetCell("J" + rowIndex).Value = dr["Decline Status"].ToString();
                                wsReport.GetCell("K" + rowIndex).Value = dr["Date of decline"].ToString();
                                wsReport.GetCell("L" + rowIndex).Value = dr["Decline Reason"].ToString();
                                wsReport.GetCell("M" + rowIndex).Value = dr["Cancellation Status"].ToString();
                                wsReport.GetCell("N" + rowIndex).Value = dr["Date of Cancellation"].ToString();
                                wsReport.GetCell("O" + rowIndex).Value = dr["TSR Assigned To"].ToString();
                                wsReport.GetCell("P" + rowIndex).Value = dr["TSR Sold By"].ToString();
                                wsReport.GetCell("Q" + rowIndex).Value = dr["Confirmation Agent"].ToString();

                            }
                        }

                        //rowIndex++;

                        #endregion Report Data
                    }
                }

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

        private void ReportSingleSheetCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";

            EnableAllControls(true);

            //btnReport.IsEnabled = true;
            //btnClose.IsEnabled = true;
            //IsReportRunning = false;
            //xdgCampaigns.IsEnabled = true;
            //Cal1.IsEnabled = true;
            //Cal2.IsEnabled = true;
        }

        #endregion Report Operations Variant 2: All batches on a single sheet

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;
            btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnReport.ToolTip = btnReport.Content;
        }

        private bool HasAllInputParametersBeenSpecified()
        {

            if (cmbCampaign.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select a campaign.", "No campaign selected", ShowMessageType.Error);
                EnableAllControls(true);
                return false;
            }

            //_batches = xdgBatches.Records;

            var lstTemp = (from r in xdgBatches.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            _lstSelectedBatches = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Batch"].Value));
            //_batches = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

            if (_lstSelectedBatches.Count == 0)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 batch from the list.", "No batches selected", ShowMessageType.Error);
                //EnableAllControls(true);
                return false;
            }


            //_batchIDs = _batches.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");

            //if (_batchIDs.Length > 0)
            //{
            //    _batchIDs = _batchIDs.Substring(0, _batchIDs.Length - 1);
            //}
            //else
            //{
            //    _batchIDs = String.Empty;
            //}

            _reportContentOnSinglePageSheet = chkSinglePage.IsChecked;

            return true;

        }

        #endregion Private Methods

        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MenuManagementScreen menuManagementScreen = new MenuManagementScreen(ScreenDirection.Reverse);
            OnClose(menuManagementScreen);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                EnableAllControls(false);

                if (!HasAllInputParametersBeenSpecified())
                {
                    EnableAllControls(true);
                    return;
                }

                if (_reportContentOnSinglePageSheet != null)
                {
                    if (_reportContentOnSinglePageSheet.Value == false)
                    {
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += Report;
                        worker.RunWorkerCompleted += ReportCompleted;
                        worker.RunWorkerAsync();
                        dispatcherTimer1.Start();
                    }
                    else
                    {
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += ReportSingleSheet;
                        worker.RunWorkerCompleted += ReportSingleSheetCompleted;
                        worker.RunWorkerAsync();
                        dispatcherTimer1.Start();
                    }
                }
                else
                {
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += Report;
                    worker.RunWorkerCompleted += ReportCompleted;
                    worker.RunWorkerAsync();
                    dispatcherTimer1.Start();
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

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

                    DataTable dt = Insure.GetBatchesForNoContactReport(Convert.ToInt64(cmbCampaign.SelectedValue));
                    DataColumn column = new DataColumn("Select", typeof(bool));
                    column.DefaultValue = false;
                    dt.Columns.Add(column);

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

        #endregion Event Handlers

    }

}
