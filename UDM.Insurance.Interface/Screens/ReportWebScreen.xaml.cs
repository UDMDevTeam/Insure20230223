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
    /// Interaction logic for ReportWebScreen.xaml
    /// </summary>
    public partial class ReportWebScreen 
    {

        #region Private members
        private CheckBox _xdgHeaderPrefixAreaCheckbox;

        private DataRow _campaign;
        private long _campaignID;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();

        private bool PermenantBool = false;
        private bool TemporaryBool = false;
        private bool Temp_PermBool = false;

        private DateTime _startDate = DateTime.Parse("04-02-2020");
        private DateTime _endDate = DateTime.Parse("06-02-2020");
        private List<Record> _campaigns;

        private int _timer1;

        #endregion

        #region Constructor
        public ReportWebScreen()
        {
            InitializeComponent();
            LoadCampaignInfo();

        }
        #endregion

        #region LoadData
        private void LoadCampaignInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataTable dt = Methods.GetTableData("SELECT [ID] AS [CampaignID], [Name] AS [CampaignName], [Code] AS [CampaignCode] FROM [INCampaign] WHERE [IsActive] = 1 ORDER BY [Name] ASC");
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
        #endregion


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

                    DataTable dt = Methods.GetTableData(String.Format("SELECT [ID], [Code] + ' (' + [UDMCode] + ')' AS [Batch], [Code], [UDMCode] FROM [INBatch] WHERE [FKINCampaignID] = {0}  AND [Code] LIKE '%R%'  ORDER BY [Code] DESC, [UDMCode] DESC", cmbCampaign.SelectedValue));
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }



        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var lstTemp = (from r in xdgBatches.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                _campaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["ID"].Value));

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += Report;
                worker.RunWorkerCompleted += ReportCompleted;
                worker.RunWorkerAsync();

                dispatcherTimer1.Start();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                int summarySheetRowIndex = 6;
                int summarySheetNewRowIndex = 0;
                int allAllocationsSheetRowIndex = 7;
                int allAllocationsTotalsFromRow = 8;
                int allAllocationsSheetTotalsRow = 8;
                int allAllocationsSheetNewRowIndex = 0;

                #region Setup excel documents

                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateWebRedeem.xlsx");

                string filePathAndName = String.Format("{0}Web Redeem Report ({1} - {2}) ~ {3}.xlsx",
                    GlobalSettings.UserFolder,
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                Worksheet wsTemplate = wbTemplate.Worksheets["Report"];
                //Worksheet wsAllAllocationsReportSheetTemplate = wbReport.Worksheets["AllAllocations"];

                Worksheet wsAllAllocationsReportSheetTemplate = wbTemplate.Worksheets["All Web Redeems"];

                #endregion Setup excel documents

                DataSet dsLeadAllocationReportData = null;
                int templateColumnSpan = 23;
                Worksheet wsAllAllocationsReportSheet = wbReport.Worksheets.Add("All Web Redeems");

                Methods.CopyExcelRegion(wsAllAllocationsReportSheetTemplate, 0, 0, 7, templateColumnSpan, wsAllAllocationsReportSheet, 0, 0);
                wsAllAllocationsReportSheet.GetCell("A1").Value = "Web Redeem Report - All Web Redeems";
                wsAllAllocationsReportSheet.GetCell("B5").Value = Insure.GetLoggedInUserNameAndSurname();
                wsAllAllocationsReportSheet.GetCell("Q5").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                foreach (DataRecord record in _campaigns)
                {
                    if ((bool)record.Cells["Select"].Value)
                    {
                        int campaignID = Convert.ToInt32(record.Cells["ID"].Value);
                        string campaignName = record.Cells["Batch"].Value.ToString();

                        #region Get report data from database

                        SqlParameter[] parameters = new SqlParameter[3];
                        parameters[0] = new SqlParameter("@CampaignID", campaignID);
                        parameters[1] = new SqlParameter("@_endDate", _endDate);
                        parameters[2] = new SqlParameter("@_startDate", _startDate);

                        dsLeadAllocationReportData = Methods.ExecuteStoredProcedure2("[dbo].[spGetWebRedeem]", parameters, IsolationLevel.Snapshot, 300);
                        //dsLeadAllocationReportData = Insure.INGetWebRedeemReportData(campaignID, _endDate, _startDate);

                        if (dsLeadAllocationReportData.Tables.Count == 8)
                        {
                            if (dsLeadAllocationReportData.Tables[0].Rows.Count == 0)
                            {
                                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                                {
                                    ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + campaignName + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
                                });

                                continue;
                            }
                        }
                        else
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + campaignName + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
                            });

                            continue;
                        }

                        #endregion Get report data from database

                        #region Update the summary sheet

                        summarySheetNewRowIndex = UpdateSummaryWorksheet(wbTemplate, wbReport, dsLeadAllocationReportData, campaignName, summarySheetRowIndex);
                        summarySheetRowIndex = summarySheetNewRowIndex;
                        //summarySheetNewRowIndex += 4;

                        #endregion Update the summary sheet

                        #region Update All Allocations Sheet

                        allAllocationsSheetNewRowIndex = UpdateAllAllocationsWorksheet(wbReport, wsAllAllocationsReportSheetTemplate, dsLeadAllocationReportData, allAllocationsSheetRowIndex, wsAllAllocationsReportSheet);
                        allAllocationsSheetRowIndex = allAllocationsSheetNewRowIndex;

                        #endregion Update All Allocations Sheet

                        #region Main Report Data


                        AddLeadAllocationReportWorksheet(wbReport, wsTemplate, dsLeadAllocationReportData, campaignName);

                        #endregion Main Report Data
                    }
                }

                //calculate the totals

                DataTable dtExcelFormulaMappings = dsLeadAllocationReportData.Tables[7];
                 Methods.MapTemplatizedExcelFormulas(wsAllAllocationsReportSheetTemplate, dtExcelFormulaMappings, allAllocationsSheetTotalsRow, 0, 0, templateColumnSpan, wsAllAllocationsReportSheet, allAllocationsSheetNewRowIndex, 0, allAllocationsTotalsFromRow, allAllocationsSheetNewRowIndex - 1);
                wsAllAllocationsReportSheet.MoveToIndex(1);
                #region Save the workbook and open it - if there is at least 1 worksheet in the workbook

                if (wbReport.Worksheets.Count > 0)
                {
                    //Save excel document
                    wbReport.Save(filePathAndName);

                    //Display excel document
                    Process.Start(filePathAndName);
                }

                #endregion Save the workbook and open it - if there is at least 1 worksheet in the workbook
            }

            catch (Exception ex)
            {
                //HandleException(ex);
            }

            finally
            {
                SetCursor(Cursors.Arrow);
            }
        }

        private void AddLeadAllocationReportWorksheet(Workbook wbReport, Worksheet wsTemplate, DataSet dsReportData, string campaignName)
        {
            #region Skip this method if there is no data for the current campaign

            if (dsReportData.Tables[0].Rows.Count == 0)
            {
                return;
            }

            #endregion Skip this method if there is no data for the current campaign

            #region Partition the given dataset

            DataTable dtReportData = dsReportData.Tables[0];
            DataTable dtExcelColumnDataTableColumnMappings = dsReportData.Tables[2];
            DataTable dtExcelFormulaMappings = dsReportData.Tables[3];

            #endregion Partition the given dataset

            #region Declarations & Initializations

            int reportRow = 7;
            int formulaStartRow = 7;

            int templateColumnSpan = 22;
            int dataTemplateRowIndex = 7;
            int totalsTemplateRowIndex = 8;
            string newWorksheetDescription = Methods.ParseWorksheetName(wbReport, campaignName).Replace("'", "");
            Worksheet wsNewReportSheet = wbReport.Worksheets.Add(newWorksheetDescription);
            Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsNewReportSheet, true, true, true, true, true, false, false, false, true, true, true, true, true, true, true, true, false);

            #endregion Declarations & Initializations

            #region Add the report headings and populate the details

            Methods.CopyExcelRegion(wsTemplate, 0, 0, 6, templateColumnSpan, wsNewReportSheet, 0, 0);
            wsNewReportSheet.GetCell("A1").Value = String.Format("Web Redeem Report - {0}", campaignName);
            //wsNewReportSheet.GetCell("A3").Value = String.Format("For the period between {0} and {1}", _startDate.ToString("dddd, d MMMM yyyy"), _endDate.ToString("dddd, d MMMM yyyy"));
            wsNewReportSheet.GetCell("B5").Value = Insure.GetLoggedInUserNameAndSurname();
            wsNewReportSheet.GetCell("P5").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            #endregion Add the report headings and populate the details

            #region Add the values

            reportRow = Methods.MapTemplatizedExcelValues(wsTemplate, dtReportData, dtExcelColumnDataTableColumnMappings, dataTemplateRowIndex, 0, 0, templateColumnSpan, wsNewReportSheet, reportRow, 0);
            //reportRow++;

            #endregion Add the values

            #region Totals & Averages

            reportRow = Methods.MapTemplatizedExcelFormulas(wsTemplate, dtExcelFormulaMappings, totalsTemplateRowIndex, 0, 0, templateColumnSpan, wsNewReportSheet, reportRow, 0, formulaStartRow, reportRow - 1);

            #endregion Totals & Averages

        }

        private int UpdateAllAllocationsWorksheet(Workbook wbReport, Worksheet wsTemplate, DataSet dsReportData, int currentRowIndex, Worksheet wsNewReportSheet)
        {

            #region Skip this method if there is no data for the current campaign

            if (dsReportData.Tables[0].Rows.Count == 0)
            {
                return currentRowIndex;
            }

            #endregion Skip this method if there is no data for the current campaign

            #region Partition the given dataset

            DataTable dtReportData = dsReportData.Tables[0];
            DataTable dtExcelColumnDataTableColumnMappings = dsReportData.Tables[6];
            //DataTable dtExcelFormulaMappings = dsReportData.Tables[3];

            #endregion Partition the given dataset

            #region Declarations & Initializations

            //int reportRow = 7;
            //int formulaStartRow = 7;

            int templateColumnSpan = 23;
            int dataTemplateRowIndex = 7;
            //int totalsTemplateRowIndex = 8;
            //string newWorksheetDescription = Methods.ParseWorksheetName(wbReport, campaignName).Replace("'", "");
            //Worksheet wsNewReportSheet = wbReport.Worksheets.Add(newWorksheetDescription);
            //Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsNewReportSheet, true, true, true, true, true, false, false, false, true, true, true, true, true, true, true, true, false);

            #endregion Declarations & Initializations

            #region Add the report headings and populate the details
            //only need to do most of this stuff once in the beginning




            //Methods.CopyExcelRegion(wsTemplate, 0, 0, 6, templateColumnSpan, wsNewReportSheet, 0, 0);
            //wsNewReportSheet.GetCell("A1").Value = String.Format("Lead Allocation Report - {0}", campaignName);
            //wsNewReportSheet.GetCell("A3").Value = String.Format("For the period between {0} and {1}", _startDate.ToString("dddd, d MMMM yyyy"), _endDate.ToString("dddd, d MMMM yyyy"));
            //wsNewReportSheet.GetCell("B5").Value = Insure.GetLoggedInUserNameAndSurname();
            //wsNewReportSheet.GetCell("P5").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            #endregion Add the report headings and populate the details

            #region Add the values

            currentRowIndex = Methods.MapTemplatizedExcelValues(wsTemplate, dtReportData, dtExcelColumnDataTableColumnMappings, dataTemplateRowIndex, 0, 0, templateColumnSpan, wsNewReportSheet, currentRowIndex, 0);
            //reportRow++;

            #endregion Add the values

            #region Totals & Averages

            //only need to do this at the end.
            return currentRowIndex++;

            #endregion Totals & Averages
        }

        private int UpdateSummaryWorksheet(Workbook wbTemplate, Workbook wbResultingWorkbook, DataSet dsReportData, string campaignName, int currentRowIndex)
        {
            #region Partition the given dataset

            DataTable dtSummarySheetData = dsReportData.Tables[1];
            DataTable dtExcelColumnDataTableColumnMappings = dsReportData.Tables[4];
            DataTable dtExcelFormulaMappings = dsReportData.Tables[5];

            #endregion Partition the given dataset

            #region Declarations & Initializations

            Worksheet wsTemplate;
            Worksheet wsSummarySheet;

            string summarySheetTemplateWorksheetName = "Summary";
            byte summarySheetTemplateColumnSpan = 6;
            int summaryEntryTemplateRowIndex = 8;
            int summaryEntryTemplateTotalsRowIndex = 9;
            int currentSummarySheetRowIndex = currentRowIndex;
            int formulaStartRow = 0;

            #endregion Declarations & Initializations

            #region Defining the template sheet

            wsTemplate = wbTemplate.Worksheets[summarySheetTemplateWorksheetName];

            #endregion Defining the template sheet

            #region Adding the Lead Details Sheet, if it does not exist yet

            if (!Methods.WorksheetExistsInWorkbook(wbResultingWorkbook, "Summary"))
            {
                wsSummarySheet = wbResultingWorkbook.Worksheets.Add("Summary");
                Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsSummarySheet, true, true, true, true, true, false, false, false, true, true, true, true, true, true, true, true, false);
                Methods.CopyExcelRegion(wsTemplate, 0, 0, 4, summarySheetTemplateColumnSpan, wsSummarySheet, 0, 0);
                //wsSummarySheet.GetCell("A3").Value = String.Format("{0} to {1}", _startDate.ToString("yyyy-MM-dd"), _endDate.ToString("yyyy-MM-dd"));
                wsSummarySheet.GetCell("B5").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                wsSummarySheet = wbResultingWorkbook.Worksheets["Summary"];
            }

            #endregion Adding the Lead Details Sheet, if it does not exist yet

            #region Populate the body of the summary sheet

            if (dtSummarySheetData.Rows.Count > 0)
            {
                #region Insert the column headings and add the campaign name

                Methods.CopyExcelRegion(wsTemplate, 6, 0, 1, summarySheetTemplateColumnSpan, wsSummarySheet, currentSummarySheetRowIndex, 0);
                wsSummarySheet.GetCell(String.Format("A{0}", currentSummarySheetRowIndex + 1)).Value = campaignName;
                currentSummarySheetRowIndex += 2;

                #endregion Insert the column headings and add the campaign name

                #region Add the values

                formulaStartRow = currentSummarySheetRowIndex;
                currentSummarySheetRowIndex = Methods.MapTemplatizedExcelValues(wsTemplate, dtSummarySheetData, dtExcelColumnDataTableColumnMappings, summaryEntryTemplateRowIndex, 0, 0, summarySheetTemplateColumnSpan, wsSummarySheet, currentSummarySheetRowIndex, 0);

                #endregion Add the values
            }

            #endregion Populate the body of the summary sheet

            #region Totals & Averages

            if (dtSummarySheetData.Rows.Count > 0)
            {
                currentSummarySheetRowIndex = Methods.MapTemplatizedExcelFormulas(wsTemplate, dtExcelFormulaMappings, summaryEntryTemplateTotalsRowIndex, 0, 0, summarySheetTemplateColumnSpan, wsSummarySheet, currentSummarySheetRowIndex, 0, formulaStartRow, currentSummarySheetRowIndex - 1);
                currentSummarySheetRowIndex++;
            }

            #endregion Totals & Averages

            return currentSummarySheetRowIndex;
        }

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";
        }

            private bool IsAllInputParametersSpecifiedAndValid()
        {
            #region BatchesSelected

            #endregion

            #region EmployeeTypeSelected

            #endregion

            return true;
        }

        #region dataGrid
        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;
                DataTable dt = ((DataView)xdgBatches.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = true;
                }

            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {

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

        private void HeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Unchecking checkboxes
        private void chkPermenantEmployee_Checked(object sender, RoutedEventArgs e)
        {
            PermenantBool = true;
            TemporaryBool = false;
            Temp_PermBool = false;

            if(chkPermenantEmployee.IsChecked == true)
            {
                chkTemporaryEmployee.IsChecked = false;
                chkBothEmployee.IsChecked = false;
            }
            if(chkTemporaryEmployee.IsChecked == true)
            {
                chkPermenantEmployee.IsChecked = false;
                chkBothEmployee.IsChecked = false;
            }
            if(chkBothEmployee.IsChecked == true)
            {
                chkPermenantEmployee.IsChecked = false;
                chkTemporaryEmployee.IsChecked = false;
            }
        }

        private void chkTemporaryEmployee_Checked(object sender, RoutedEventArgs e)
        {
            PermenantBool = false;
            TemporaryBool = true;
            Temp_PermBool = false;

            if (chkPermenantEmployee.IsChecked == true)
            {
                chkTemporaryEmployee.IsChecked = false;
                chkBothEmployee.IsChecked = false;
            }
            else if (chkTemporaryEmployee.IsChecked == true)
            {
                chkPermenantEmployee.IsChecked = false;
                chkBothEmployee.IsChecked = false;
            }
            else if (chkBothEmployee.IsChecked == true)
            {
                chkPermenantEmployee.IsChecked = false;
                chkTemporaryEmployee.IsChecked = false;
            }
        }

        private void chkBothEmployee_Checked(object sender, RoutedEventArgs e)
        {
            PermenantBool = false;
            TemporaryBool = false;
            Temp_PermBool = true;

            if (chkPermenantEmployee.IsChecked == true)
            {
                chkTemporaryEmployee.IsChecked = false;
                chkBothEmployee.IsChecked = false;
            }
            else if (chkTemporaryEmployee.IsChecked == true)
            {
                chkPermenantEmployee.IsChecked = false;
                chkBothEmployee.IsChecked = false;
            }
            else if (chkBothEmployee.IsChecked == true)
            {
                chkPermenantEmployee.IsChecked = false;
                chkTemporaryEmployee.IsChecked = false;
            }
        }
        #endregion



    }
}
