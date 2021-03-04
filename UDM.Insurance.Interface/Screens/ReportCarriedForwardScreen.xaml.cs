using System.Linq;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Infragistics.Windows.Editors.Events;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using static UDM.Insurance.Interface.Screens.ReportConfirmationStatsScreen;
using Infragistics.Windows.DataPresenter;
//using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Windows.Controls;
using Infragistics.Windows.DataPresenter.ExcelExporter;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportCarriedForwardScreen
    {

        #region Class AgentFilterScreen

        public class AgentFilterScreen
        {
            public string ConsultantName { get; set; }
            public string ReferenceNumber { get; set; }
            public string DateOfSale { get; set; }
            //public string ConfWorkDate { get; set; }
            public string Campaign { get; set; }
            public string TSR { get; set; }
            public string Status { get; set; }
            //public string ConfirmedStatus { get; set; }
            public long FKINImportID { get; set; }
        }

        #endregion Class AgentFilterScreen

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion INotifyPropertyChanged implementation

        #region Private Members

        private DateTime _fromDate = DateTime.Now;
        private DateTime _toDate = DateTime.Now;
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;
        private bool? _isReportRunning = false;

        List<AgentFilterScreen> _agentFilters = new List<AgentFilterScreen>();

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        //private RecordCollectionBase _campaigns;
        long UserTypeID;
        //private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        //private int _timer1;
        private byte _reportScope = 1;
        private bool _insertSingleSheetWithAllData = false;

        private List<Record> _lstSelectedConfirmationAgents;
        private string _fkUserIDs;

        #endregion Private Members

        #region Publicly Encapsulated Properties
        public bool? IsReportRunning
        {
            get
            {
                return _isReportRunning;
            }
            set
            {
                _isReportRunning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReportRunning"));
            }
        }

        #endregion Publicly Encapsulated Properties

        #region Constructors

        public ReportCarriedForwardScreen(long userTypeID)
        {
            InitializeComponent();

            UserTypeID = userTypeID;
            LoadAgentInfo();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

        private bool IsAllInputParametersSpecifiedAndValid()
        {

            #region Ensuring that the From Date was specified

            if (calFromDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'From Date'.", @"No 'From Date' specified", ShowMessageType.Error);
                return false;
            }
            else
            {
                _fromDate = calFromDate.SelectedDate.Value;
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the To Date was specified

            if (calToDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'To Date'.", @"No 'To Date' specified", Embriant.Framework.ShowMessageType.Error);
                return false;
            }

            else
            {
                _toDate = calToDate.SelectedDate.Value;
            }

            #endregion Ensuring that the To Date was specified

            #region Ensuring that the date range is valid

            if (_fromDate > _toDate)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Invalid date range specified: The 'From Date' can not be greater than the 'To Date'.", "Invalid date range", Embriant.Framework.ShowMessageType.Error);
                return false;
            }

            //else
            //{
            //    _dateRanges = new List<Tuple<DateTime, DateTime, byte>>();
            //    _dateRanges = DetermineDateRanges(_week134CutOverDate, _reportStartDate, _reportEndDate);
            //}


            #endregion Ensuring that the date range is valid

            // Otherwise, if all is well, proceed:
            return true;
        }

        private void InsertCarriedForwardReportSheets(Workbook wbCarriedForwardReportTemplate, Workbook wbCarriedForwardSalesReport, DataSet dsCarriedForwardReportData, string reportScope)
        {
            #region Partition the given dataset

            DataTable dtSheetDataFilters = dsCarriedForwardReportData.Tables[0];
            DataTable dtReportData;
            DataTable dtTotalsData;
            //DataTable dtCurrentDataSheetPartitions;
            //DataTable dtCurrentDataSheetPartitionData;
            DataTable dtExcelSheetDataTableColumnMappings; // = dsConfirmedSalesReportData.Tables[4];
            DataTable dtExcelSheetTotalsTableColumnMappings;
            //DataTable dtExcelCellTotalsFormulasMappings = dsConfirmedSalesReportData.Tables[5];

            #endregion Partition the given dataset

            #region Declarations & Initializations

            int reportRow = 6;
            //int formulaStartRow = 0;

            byte dataTableIndex = 0;
            byte excelDataTableColumnMappingsTableIndex = 0;
            byte totalsTableIndex = 0;
            byte excelTotalsTableColumnMappingsTableIndex = 0;
            //string reportDataOrderByString = String.Empty;

            string templateSheetName;
            string reportSheetName;

            string reportTitleCell = "A1";
            string reportTitle;
            string reportSubtitle;
            string reportSubHeadingCell;
            string reportDateCell;

            byte templateDataSheetRowSpan;
            byte templateColumnSpan;
            byte templateRowIndex;

            byte templateTotalsSheetRowSpan;
            byte templateTotalsColumnSpan;
            byte templateTotalsRowIndex;

            string columnsToBeHidden;

            #endregion Declarations & Initializations

            foreach (DataRow row in dtSheetDataFilters.Rows)
            {
                dataTableIndex = Convert.ToByte(row["DataTableIndex"]);
                excelDataTableColumnMappingsTableIndex = Convert.ToByte(row["ExcelDataTableColumnMappingsTableIndex"]);

                //if (Convert.ToInt32(row["DataTableIndex"]) == 3)
                //{
                    DataRow drTotalsSheetDataFilters = dtSheetDataFilters.Select("[ID] = 1").FirstOrDefault();

                    totalsTableIndex = Convert.ToByte(drTotalsSheetDataFilters["DataTableIndex"]);

                    excelTotalsTableColumnMappingsTableIndex = Convert.ToByte(drTotalsSheetDataFilters["ExcelDataTableColumnMappingsTableIndex"]);

                    dtExcelSheetTotalsTableColumnMappings = dsCarriedForwardReportData.Tables[excelTotalsTableColumnMappingsTableIndex];
                if (Convert.ToInt32(row["DataTableIndex"]) == 3)
                {
                    dtExcelSheetTotalsTableColumnMappings.Rows[0].Delete();
                    dtExcelSheetTotalsTableColumnMappings.AcceptChanges();
                }
                    
                //}
                    

                

                #region Initialize

                dtReportData = dsCarriedForwardReportData.Tables[dataTableIndex];
                dtTotalsData = dsCarriedForwardReportData.Tables[Convert.ToInt32(drTotalsSheetDataFilters["DataTableIndex"])];
                dtExcelSheetDataTableColumnMappings = dsCarriedForwardReportData.Tables[excelDataTableColumnMappingsTableIndex];

                templateSheetName = row["TemplateSheetName"].ToString();
                reportSheetName = row["ReportSheetName"].ToString();
                reportTitle = row["ReportTitle"].ToString();
                reportSubtitle = row["ReportSubtitle"].ToString();
                reportSubHeadingCell = row["ReportSubtitleCell"].ToString();
                reportDateCell = row["ReportDateCell"].ToString();

                templateDataSheetRowSpan = Convert.ToByte(row["TemplateDataSheetRowSpan"]);
                templateTotalsSheetRowSpan = Convert.ToByte(drTotalsSheetDataFilters["TemplateDataSheetRowSpan"]);
                //templateColumnPartitionColumnHeadingsRowSpan = Convert.ToByte(row["TemplateColumnPartitionColumnHeadingsRowSpan"]);
                //templateColumnPartitionColumnHeadingsRowIndex = Convert.ToByte(row["TemplateColumnPartitionColumnHeadingsRowIndex"]);

                templateColumnSpan = Convert.ToByte(row["TemplateColumnSpan"]);
                templateRowIndex = Convert.ToByte(row["TemplateRowIndex"]);

                templateTotalsColumnSpan = Convert.ToByte(drTotalsSheetDataFilters["TemplateColumnSpan"]);
                templateTotalsRowIndex = Convert.ToByte(drTotalsSheetDataFilters["TemplateRowIndex"]);
                //totalsTemplateRowIndex = Convert.ToByte(row["totalsTemplateRowIndex"]);

                columnsToBeHidden = row["ColumnsToBeHidden"].ToString();

                #endregion Initialize

                #region Add the worksheet

                Worksheet wsReportTemplate = wbCarriedForwardReportTemplate.Worksheets[templateSheetName];
                Worksheet wsReport = wbCarriedForwardSalesReport.Worksheets.Add(reportSheetName);
                Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                #endregion Add the worksheet

                #region Populating the report details

                Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
                wsReport.GetCell(reportTitleCell).Value = reportTitle;
                wsReport.GetCell(reportSubHeadingCell).Value = reportSubtitle;
                wsReport.GetCell(reportDateCell).Value = String.Format("Date generated: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                //if (reportSheetName.Contains("Summary"))
                //{
                //    wsReport.GetCell(reportTitleCell).Value = "Call Monitoring Tracking Report - " + reportScope;
                //}
                //else if (reportSheetName.Contains("Data Sheet"))
                //{
                //    wsReport.GetCell(reportTitleCell).Value = "Call Monitoring Tracking Report - " + reportScope;
                //}
                //else if (reportSheetName.Contains("By Call-Monitoring Agent"))
                //{
                //    wsReport.GetCell(reportTitleCell).Value = "Call Monitored By - " + reportScope;
                //}

                #endregion Populating the report details

                #region Hiding all the unnecessary columns

                if (!String.IsNullOrEmpty(columnsToBeHidden))
                {
                    string[] columns = columnsToBeHidden.Split(new char[] { ',' });

                    foreach (string currentColumn in columns)
                    {
                        byte currentColumnIndex = Convert.ToByte(currentColumn);
                        wsReport.Columns[currentColumnIndex].Hidden = true;
                    }
                }

                #endregion Hiding all the unnecessary columns

                #region Add the report data

                reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtReportData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                #region Add the totals
                if (Convert.ToInt32(row["DataTableIndex"]) == 3)
                {
                    reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtTotalsData, dtExcelSheetTotalsTableColumnMappings, templateTotalsRowIndex + 1, 0, 0, templateTotalsColumnSpan, wsReport, reportRow, 0);
                }


                #endregion Add the totals

                reportRow = 6;

                #endregion Add the report data

            }
        }

        //private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    dispatcherTimer1.Stop();
        //    _timer1 = 0;
        //    btnCombinedReport.Content = "Report";

        //    //EnableAllControls(true);
        //    EnableDisableControls(true);
        //}

        private void EnableDisableExportButton()
        {
            try
            {
                if ((calFromDate.SelectedDate != null && (calToDate.SelectedDate != null))) //&& (calEndDate.SelectedDate >= Cal1.SelectedDate)
                {
                    if (_xdgHeaderPrefixAreaCheckbox != null && (_xdgHeaderPrefixAreaCheckbox.IsChecked == true || _xdgHeaderPrefixAreaCheckbox.IsChecked == null))
                    {
                        //EnableDisableControls(true);
                        btnCombinedReport.IsEnabled = true;
                        btnBaseReport.IsEnabled = true;
                        btnUpgradeReport.IsEnabled = true;
                        btnIGReport.IsEnabled = true;
                        return;
                    }
                }
                //EnableDisableControls(false);
                btnCombinedReport.IsEnabled = false;
                btnBaseReport.IsEnabled = false;
                btnUpgradeReport.IsEnabled = false;
                btnIGReport.IsEnabled = false;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;

            btnCombinedReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnCombinedReport.ToolTip = btnCombinedReport.Content;
        }

        private bool? AllRecordsSelected()
        {
            try
            {
                bool allSelected = true;
                bool noneSelected = true;

                if (xdgAgents.DataSource != null)
                {
                    foreach (DataRow dr in ((DataView)xdgAgents.DataSource).Table.Rows)
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

        private void LoadAgentInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataTable dt = Business.Insure.INGetCallMonitoringTrackingReportLookups();
                xdgAgents.DataSource = dt.DefaultView;
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

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            //btnReport.Content = "Report";

            switch (_reportScope)
            {
                case 1:
                    btnCombinedReport.Content = "Combined Report";
                    break;
                case 2:
                    btnBaseReport.Content = "Base Report";
                    break;
                case 3:
                    btnUpgradeReport.Content = "Upgrade Report";
                    break;
            }

            //btnCombinedReport.IsEnabled = true;
            //btnBaseReport.IsEnabled = true;
            //btnUpgradeReport.IsEnabled = true;
            //btnClose.IsEnabled = true;
            //xdgAgents.IsEnabled = true;
            //calStartDate.IsEnabled = true;
            //calEndDate.IsEnabled = true;

            EnableDisableControls(true);

            dgConfirmationStats.DataSource = _agentFilters;
            dgConfirmationStats.FieldLayouts[0].Fields["FKINImportID"].Visibility = Visibility.Collapsed;
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup excel documents

                string reportScopeIdentifier = String.Empty;
                switch (_reportScope)
                {
                    case 1:
                        reportScopeIdentifier = "Combined";
                        break;
                    case 2:
                        reportScopeIdentifier = "Base";
                        break;
                    case 3:
                        reportScopeIdentifier = "Upgrade";
                        break;
                    case 4:
                        reportScopeIdentifier = "IG";
                        break;
                }

                string filePathAndName = String.Empty;
                if (_fromDate == _toDate)
                {
                    filePathAndName = String.Format("{0}Call Monitoring Tracking Report - {1} ({2}), {3}.xlsx", GlobalSettings.UserFolder, reportScopeIdentifier, _fromDate.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));
                }
                else
                {
                    filePathAndName = String.Format("{0}Call Monitoring Tracking Report - {1} ({2} - {3}), {4}.xlsx", GlobalSettings.UserFolder, reportScopeIdentifier, _fromDate.ToString("yyyy-MM-dd"), _toDate.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));
                }

                Workbook wbConfirmedSalesReportTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateCarriedForward.xlsx");
                Workbook wbConfirmedSalesReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup excel documents

                #region Setup Excel document - Old Carried Forward

                //string filePathAndName = String.Format("{0}Carried Forward Report ~ {1}.xlsx",
                //    GlobalSettings.UserFolder,
                //    DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                //Workbook wbConfirmedSalesReportTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateCarriedForward.xlsx");
                //Workbook wbConfirmedSalesReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup Excel document

                #region Get the data

                DataSet dsCarriedForwardReportData = Business.Insure.INReportCarriedForward(_fkUserIDs, _fromDate, _toDate, _reportScope);

                //if (dsCarriedForwardReportData.Tables[2].Rows.Count == 0)
                if (dsCarriedForwardReportData.Tables[5].Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                #endregion Get the data

                InsertCarriedForwardReportSheets(wbConfirmedSalesReportTemplate, wbConfirmedSalesReport, dsCarriedForwardReportData, reportScopeIdentifier);

                #region Finally, save, and display the resulting workbook

                if (wbConfirmedSalesReport.Worksheets.Count > 0)
                {
                    wbConfirmedSalesReport.Save(filePathAndName);

                    //Display excel document
                    Process.Start(filePathAndName);
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "There is no data available for the criteria you have specified. Please change some of them and try again.", "No data available", ShowMessageType.Information);
                    });
                }
                #endregion Finally, save, and display the resulting workbook
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

        #region Obsolete

        //private void InsertConfirmationStatsReportSummarySheet(Workbook wbTemplate, Workbook wbReport, DataSet dsConfirmationStatsReportData)
        //{
        //    if (dsConfirmationStatsReportData.Tables[0].Rows.Count > 0)
        //    {
        //        #region Partition the given dataset

        //        //string orderByString = drCurrentConfirmationAgent["OrderByString"].ToString();
        //        DataTable dtSummarySheetData = dsConfirmationStatsReportData.Tables[0];
        //        DataTable dtExcelSheetDataTableColumnMappings = dsConfirmationStatsReportData.Tables[1];
        //        DataTable dtExcelSheetTotalsAndAverageColumnMappings = dsConfirmationStatsReportData.Tables[2];

        //        #endregion Partition the given dataset

        //        #region Declarations & Initializations

        //        int reportRow = 6;
        //        int formulaStartRow = reportRow;

        //        byte templateDataSheetRowSpan = 5;
        //        byte templateColumnSpan = 21;
        //        byte templateRowIndex = 6;
        //        byte totalsTemplateRowIndex = 7;

        //        string reportScopeIdentifier = String.Empty;
        //        switch (_reportScope)
        //        {
        //            case 1:
        //                reportScopeIdentifier = "Combined";
        //                break;
        //            case 2:
        //                reportScopeIdentifier = "Base";
        //                break;
        //            case 3:
        //                reportScopeIdentifier = "Upgrade";
        //                break;
        //        }

        //        string reportHeadingCell = "A1";
        //        string reportSubHeadingCell = "A3";
        //        string reportDateCell = "U4";

        //        string reportTitle = String.Format("{0} Confirmation Statistics Report - Summary", reportScopeIdentifier);
        //        string reportSubTitle = dsConfirmationStatsReportData.Tables[0].Rows[0]["ReportSubTitle"].ToString();

        //        #endregion Declarations & Initializations

        //        #region Add the worksheet

        //        Worksheet wsReportTemplate = wbTemplate.Worksheets["Summary"];
        //        Worksheet wsReport = wbReport.Worksheets.Add("Summary");
        //        Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

        //        #endregion Add the worksheet

        //        #region Populating the report details

        //        Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
        //        wsReport.GetCell(reportHeadingCell).Value = reportTitle;
        //        wsReport.GetCell(reportSubHeadingCell).Value = reportSubTitle;
        //        wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //        #endregion Populating the report details

        //        #region Add the data

        //        reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtSummarySheetData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

        //        #endregion Add the data

        //        #region Add the totals / averages

        //        reportRow = Methods.MapTemplatizedExcelFormulas(wsReportTemplate, dtExcelSheetTotalsAndAverageColumnMappings, totalsTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0, formulaStartRow, reportRow - 1);

        //        #endregion Add the totals / averages
        //    }
        //}

        //private void InsertCumulativeStatsSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsConfirmationStatsReportData)
        //{
        //    if (dsConfirmationStatsReportData.Tables[3].Rows.Count > 0)
        //    {
        //        #region Partition the given dataset

        //        string orderByString = dsConfirmationStatsReportData.Tables[0].Rows[0]["OrderByString"].ToString();
        //        DataTable dtCumulativeConfirmationAgentData = dsConfirmationStatsReportData.Tables[3];
        //        DataTable dtExcelSheetDataTableColumnMappings = dsConfirmationStatsReportData.Tables[4];

        //        #endregion Partition the given dataset

        //        #region Declarations & Initializations

        //        //int reportRow = 6;

        //        //byte templateDataSheetRowSpan = 5;
        //        //byte templateColumnSpan = 17;
        //        //byte templateRowIndex = 6;

        //        byte templateDataSheetRowSpan = 4;
        //        byte templateColumnSpan = 17;
        //        byte templateRowIndex = 7;

        //        byte templateReportDataColumnHeadingsRowIndex = 5;
        //        byte templateReportDataColumnHeadingsRowSpan = 1;

        //        int reportRow = 5;

        //        string worksheetTabName = "All Confirmation Agents";
        //        string campaignDataSheetTemplateName = "Report";

        //        string reportHeadingCell = "A1";
        //        string reportSubHeadingCell = "A3";
        //        string reportDateCell = "Q4";

        //        string nonConfirmedFilterString = "[IsConfirmed] = 0";
        //        string confirmedFilterString = "[IsConfirmed] = 1";

        //        string reportScopeIdentifier = String.Empty;
        //        switch (_reportScope)
        //        {
        //            case 1:
        //                reportScopeIdentifier = "Combined";
        //                break;
        //            case 2:
        //                reportScopeIdentifier = "Base";
        //                break;
        //            case 3:
        //                reportScopeIdentifier = "Upgrade";
        //                break;
        //        }

        //        string reportTitle = String.Format("{0} Confirmation Statistics Report - All Confirmation Agents", reportScopeIdentifier);
        //        string reportSubTitle = dsConfirmationStatsReportData.Tables[0].Rows[0]["ReportSubTitle"].ToString();

        //        #endregion Declarations & Initializations

        //        #region Add the worksheet

        //        Worksheet wsReportTemplate = wbTemplate.Worksheets[campaignDataSheetTemplateName];
        //        Worksheet wsReport = wbReport.Worksheets.Add(worksheetTabName);
        //        Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

        //        #endregion Add the worksheet

        //        #region Populating the report details

        //        Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
        //        wsReport.GetCell(reportHeadingCell).Value = reportTitle;
        //        wsReport.GetCell(reportSubHeadingCell).Value = reportSubTitle;
        //        wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //        #endregion Populating the report details

        //        #region Add the non-confirmed lead data

        //        var filteredNonConfirmedRows = dtCumulativeConfirmationAgentData.Select(nonConfirmedFilterString).AsEnumerable();
        //        if (filteredNonConfirmedRows.Any())
        //        {
        //            //Insert the columm headings:
        //            Methods.CopyExcelRegion(wsReportTemplate, templateReportDataColumnHeadingsRowIndex, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, reportRow, 0);
        //            wsReport.GetCell(String.Format("A{0}", reportRow + 1)).Value = "Non-Confirmed Policies";
        //            reportRow += 2;

        //            // Get the data
        //            DataTable dtCurrentConfirmationAgentNonConfirmedData = dtCumulativeConfirmationAgentData.Select(nonConfirmedFilterString, orderByString).CopyToDataTable();

        //            reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentConfirmationAgentNonConfirmedData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);
        //            reportRow++;
        //        }

        //        #endregion Add the non-confirmed lead data

        //        #region Add the confirmed lead data

        //        var filteredConfirmedRows = dtCumulativeConfirmationAgentData.Select(nonConfirmedFilterString).AsEnumerable();
        //        if (filteredConfirmedRows.Any())
        //        {
        //            //Insert the columm headings:
        //            Methods.CopyExcelRegion(wsReportTemplate, templateReportDataColumnHeadingsRowIndex, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, reportRow, 0);
        //            wsReport.GetCell(String.Format("A{0}", reportRow + 1)).Value = "Confirmed Policies";
        //            reportRow += 2;

        //            // Get the data
        //            DataTable dtCurrentConfirmationAgentConfirmedData = dtCumulativeConfirmationAgentData.Select(confirmedFilterString, orderByString).CopyToDataTable();

        //            // Insert the data
        //            reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentConfirmationAgentConfirmedData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);
        //            reportRow++;
        //        }

        //        #endregion Add the non-confirmed lead data

        //        //#region Add the data

        //        //reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentConfirmationAgentData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

        //        //#endregion Add the data

        //    }
        //}

        //private void InsertIndividualConfirmationStatsReportSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsConfirmationStatsReportData, DataRow drCurrentConfirmationAgent)
        //{
        //    string filterString = drCurrentConfirmationAgent["FilterString"].ToString();

        //    var filteredRows = dsConfirmationStatsReportData.Tables[3].Select(filterString).AsEnumerable();
        //    if (filteredRows.Any())
        //    {
        //        #region Partition the given dataset

        //        string orderByString = drCurrentConfirmationAgent["OrderByString"].ToString();
        //        DataTable dtCurrentConfirmationAgentData = dsConfirmationStatsReportData.Tables[3].Select(filterString, orderByString).CopyToDataTable();
        //        DataTable dtExcelSheetDataTableColumnMappings = dsConfirmationStatsReportData.Tables[4];

        //        #endregion Partition the given dataset

        //        #region Declarations & Initializations

        //        byte templateDataSheetRowSpan = 4;
        //        byte templateColumnSpan = 17;
        //        byte templateRowIndex = 7;

        //        byte templateReportDataColumnHeadingsRowIndex = 5;
        //        byte templateReportDataColumnHeadingsRowSpan = 1;

        //        int reportRow = 5;

        //        string confirmationConsultant = drCurrentConfirmationAgent["ConfirmationConsultant"].ToString();
        //        string worksheetTabName = Methods.ParseWorksheetName(wbReport, confirmationConsultant);
        //        string campaignDataSheetTemplateName = "Report";

        //        string reportHeadingCell = "A1";        //dtReportConfigs.Rows[0]["ReportHeadingCell"].ToString();
        //        string reportSubHeadingCell = "A3";     //dtReportConfigs.Rows[0]["ReportSubHeadingCell"].ToString();
        //        string reportDateCell = "Q4";           //dtReportConfigs.Rows[0]["ReportDateCell"].ToString();

        //        string reportTitle = drCurrentConfirmationAgent["ReportTitle"].ToString();
        //        string reportSubTitle = drCurrentConfirmationAgent["ReportSubTitle"].ToString();

        //        string nonConfirmedFilterString = drCurrentConfirmationAgent["NonConfirmedFilterString"].ToString();
        //        string confirmedFilterString = drCurrentConfirmationAgent["ConfirmedFilterString"].ToString();

        //        #endregion Declarations & Initializations

        //        #region Add the worksheet

        //        Worksheet wsReportTemplate = wbTemplate.Worksheets[campaignDataSheetTemplateName];
        //        Worksheet wsReport = wbReport.Worksheets.Add(worksheetTabName);
        //        Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

        //        #endregion Add the worksheet

        //        #region Populating the report details

        //        Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
        //        wsReport.GetCell(reportHeadingCell).Value = reportTitle;
        //        wsReport.GetCell(reportSubHeadingCell).Value = reportSubTitle;
        //        wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //        #endregion Populating the report details

        //        #region Add the non-confirmed lead data

        //        var filteredNonConfirmedRows = dtCurrentConfirmationAgentData.Select(nonConfirmedFilterString).AsEnumerable();
        //        if (filteredNonConfirmedRows.Any())
        //        {
        //            //Insert the columm headings:
        //            Methods.CopyExcelRegion(wsReportTemplate, templateReportDataColumnHeadingsRowIndex, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, reportRow, 0);
        //            wsReport.GetCell(String.Format("A{0}", reportRow + 1)).Value = "Non-Confirmed Policies";
        //            reportRow += 2;

        //            // Get the data
        //            DataTable dtCurrentConfirmationAgentNonConfirmedData = dtCurrentConfirmationAgentData.Select(nonConfirmedFilterString, orderByString).CopyToDataTable();

        //            reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentConfirmationAgentNonConfirmedData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);
        //            reportRow++;
        //        }

        //        #endregion Add the non-confirmed lead data

        //        #region Add the confirmed lead data

        //        var filteredConfirmedRows = dtCurrentConfirmationAgentData.Select(confirmedFilterString).AsEnumerable();
        //        if (filteredConfirmedRows.Any())
        //        {
        //            //Insert the columm headings:
        //            Methods.CopyExcelRegion(wsReportTemplate, templateReportDataColumnHeadingsRowIndex, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, reportRow, 0);
        //            wsReport.GetCell(String.Format("A{0}", reportRow + 1)).Value = "Confirmed Policies";
        //            reportRow += 2;

        //            // Get the data
        //            DataTable dtCurrentConfirmationAgentConfirmedData = dtCurrentConfirmationAgentData.Select(confirmedFilterString, orderByString).CopyToDataTable();

        //            // Insert the data
        //            reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentConfirmationAgentConfirmedData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);
        //            reportRow++;
        //        }

        //        #endregion Add the non-confirmed lead data

        //        #region Populate _agentFilters

        //        foreach (DataRow agentRow in dtCurrentConfirmationAgentData.Rows)
        //        {
        //            AgentFilterScreen agentFilter = new AgentFilterScreen();
        //            agentFilter.ConsultantName = agentRow["ConfirmationConsultant1"].ToString();
        //            agentFilter.ReferenceNumber = agentRow["RefNo"].ToString();
        //            agentFilter.DateOfSale = agentRow["DateOfSale"].ToString();
        //            //agentFilter.ConfWorkDate = agentRow["ConfWorkDate"].ToString();
        //            agentFilter.Campaign = agentRow["CampaignCode"].ToString();
        //            agentFilter.TSR = agentRow["SalesConsultant"].ToString();
        //            agentFilter.Status = agentRow["LeadStatus"].ToString();
        //            //agentFilter.ConfirmedStatus = agentRow["ConfirmedStatus"].ToString();
        //            agentFilter.FKINImportID = Convert.ToInt64(agentRow["FKINImportID"]);

        //            _agentFilters.Add(agentFilter);
        //        }

        //        #endregion Populate _agentFilters
        //    }
        //}

        #endregion Obsolete

        #endregion Private Methods

        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //ShowMessageBox(new INMessageBoxWindow1(), "This report is still undergoing development, but it will be available soon.", "Under Construction", ShowMessageType.Information);

                if (IsAllInputParametersSpecifiedAndValid())
                {
                    //EnableAllControls(false);
                    //EnableDisableControls(false);

                    PreReportGenerationOperations(1);

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

        //private void EnableAllControls(bool isEnabled)
        //{
        //    btnClose.IsEnabled = isEnabled;
        //    btnCombinedReport.IsEnabled = isEnabled;
        //    calFromDate.IsEnabled = isEnabled;
        //    calToDate.IsEnabled = isEnabled;
        //}

        public void EnableDisableControls(bool enabled)
        {
            btnClose.IsEnabled = enabled;
            btnCombinedReport.IsEnabled = enabled;
            btnBaseReport.IsEnabled = enabled;
            btnUpgradeReport.IsEnabled = enabled;
            btnIGReport.IsEnabled = enabled;
            xdgAgents.IsEnabled = enabled;
            calFromDate.IsEnabled = enabled;
            calToDate.IsEnabled = enabled;
            btnExportToExcel.IsEnabled = enabled;
            btnLoadLeads.IsEnabled = enabled;
        }

        public void PreReportGenerationOperations(byte reportScope)
        {

            var lstTemp = (from r in xdgAgents.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            _lstSelectedConfirmationAgents = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["AgentName"].Value));

            if (_lstSelectedConfirmationAgents.Count == 0)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 confirmation agent from the list.", "No confirmation agent selected", ShowMessageType.Error);
                return;
            }
            else
            {
                _fkUserIDs = _lstSelectedConfirmationAgents.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["AgentID"].Value + ",");
                _fkUserIDs = _fkUserIDs.Substring(0, _fkUserIDs.Length - 1);
            }

            if (chkIncludeCumulativeSheet.IsChecked.HasValue)
            {
                if (chkIncludeCumulativeSheet.IsChecked.Value)
                {
                    _insertSingleSheetWithAllData = true;
                }
                else
                {
                    _insertSingleSheetWithAllData = false;
                }
            }
            else
            {
                _insertSingleSheetWithAllData = false;
            }

            EnableDisableControls(false);

            _reportScope = reportScope;
        }

        private void btnBaseReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsAllInputParametersSpecifiedAndValid())
                {
                    PreReportGenerationOperations(2);

                    //IEnumerable<DataRecord> campaigns = xdgAgents.Records.Cast<DataRecord>().ToArray();

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

        private void btnUpgradeReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsAllInputParametersSpecifiedAndValid())
                {
                    PreReportGenerationOperations(3);

                    //IEnumerable<DataRecord> campaigns = xdgAgents.Records.Cast<DataRecord>().ToArray();

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

        private void btnIGReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsAllInputParametersSpecifiedAndValid())
                {
                    PreReportGenerationOperations(4);

                    //IEnumerable<DataRecord> campaigns = xdgAgents.Records.Cast<DataRecord>().ToArray();

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
                DataTable dt = ((DataView)xdgAgents.DataSource).Table;

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
                DataTable dt = ((DataView)xdgAgents.DataSource).Table;

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

        private void Cal1_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calFromDate.SelectedDate.ToString(), out _fromDate);
            EnableDisableExportButton();
        }

        private void Cal2_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calToDate.SelectedDate.ToString(), out _toDate);
            EnableDisableExportButton();
        }

        private void btnExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            //string filePathAndName = GlobalSettings.UserFolder + "Agent Summary  Report ~ " + DateTime.Now.Millisecond + ".xlsx";
            string filePathAndName = String.Format("{0}Agent Summary Report ~ {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmss")); //GlobalSettings.UserFolder + "Agent Summary Report ~ " + DateTime.Now.Millisecond + ".xlsx";
            DataPresenterExcelExporter exporter = new DataPresenterExcelExporter();
            exporter.Export(dgConfirmationStats, filePathAndName, WorkbookFormat.Excel2007);
            Process.Start(filePathAndName);
        }
        private void dgConfirmationStats_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRecordCellArea drca = Infragistics.Windows.Utilities.GetAncestorFromType(e.OriginalSource as DependencyObject, typeof(DataRecordCellArea), false) as DataRecordCellArea;

            if (drca != null)
            {
                if (dgConfirmationStats.ActiveRecord != null && dgConfirmationStats.ActiveRecord.FieldLayout.Description == "AgentFilterScreen")
                {
                    DataRecord record = (DataRecord)dgConfirmationStats.ActiveRecord;

                    long? fkINImportID = Int64.Parse(record.Cells["FKINImportID"].Value.ToString());

                    #region Determining whether or not the lead was allocated

                    bool hasLeadBeenAllocated = Business.Insure.HasLeadBeenAllocated(fkINImportID);

                    if (!hasLeadBeenAllocated)
                    {
                        INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                        ShowMessageBox(messageWindow, @"This lead cannot be loaded, because it has not been allocated yet. Please consult your supervisor.", "Lead not allocated", Embriant.Framework.ShowMessageType.Exclamation);
                        return;
                    }

                    #endregion Determining whether or not the lead was allocated

                    #region Checking if the lead to be loaded has a status of "DO NOT CONTACT"

                    bool clientCanBeContacted = Business.Insure.CanClientBeContacted(fkINImportID);

                    if (!clientCanBeContacted)
                    {
                        INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                        ShowMessageBox(messageWindow, @"This lead cannot be loaded in its entirety, because the client has requested not to be contacted again.", "DO NOT CONTACT CLIENT", Embriant.Framework.ShowMessageType.Exclamation);
                    }

                    #endregion Checking if the lead to be loaded has a status of "DO NOT CONTACT"

                    #region Determining whether or not the lead has a status of cancelled
                    // Please see https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/204734160/comments
                    // Please see https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/211742618/comments

                    //bool hasLeadBeenCancelled = Business.Insure.HasLeadBeenCancelled(fkINImportID);

                    //if (hasLeadBeenCancelled)
                    //{
                    //    INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                    //    ShowMessageBox(messageWindow, @"This lead cannot be loaded, because the policy has been cancelled by the client. Please consult your supervisor.", "Cancelled Policy", Embriant.Framework.ShowMessageType.Exclamation);
                    //    return;
                    //}

                    #endregion Determining whether or not the lead has a status of cancelled

                    LeadApplicationScreen leadApplicationScreen = new LeadApplicationScreen(long.Parse(record.Cells["FKINImportID"].Value.ToString()), new Data.SalesScreenGlobalData());
                    ShowDialog(leadApplicationScreen, new INDialogWindow(leadApplicationScreen));
                    //leadApplicationScreen.ShowNotes(Int64.Parse(record.Cells["ImportID"].Value.ToString()));
                }
            }
        }

        private void btnLoadLeads_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                PreReportGenerationOperations(1);

                DataSet dsCarriedForwardReportData = Business.Insure.INReportCarriedForward(_fkUserIDs, _fromDate, _toDate, _reportScope);

                #region Populate _agentFilters

                if (dsCarriedForwardReportData.Tables[5].Rows.Count > 0)
                {
                    foreach (DataRow agentRow in dsCarriedForwardReportData.Tables[5].Rows)
                    {
                        AgentFilterScreen agentFilter = new AgentFilterScreen();
                        agentFilter.ConsultantName = agentRow["CallMonitoringAgent"].ToString();
                        agentFilter.ReferenceNumber = agentRow["RefNo"].ToString();
                        agentFilter.DateOfSale = agentRow["DateOfSale"].ToString();
                        //agentFilter.ConfWorkDate = agentRow["ConfWorkDate"].ToString();
                        agentFilter.Campaign = agentRow["CampaignCode"].ToString();
                        agentFilter.TSR = agentRow["SalesConsultant"].ToString();
                        agentFilter.Status = agentRow["LeadStatus"].ToString();
                        //agentFilter.ConfirmedStatus = agentRow["ConfirmedStatus"].ToString();
                        agentFilter.FKINImportID = Convert.ToInt64(agentRow["FKINImportID"]);

                        _agentFilters.Add(agentFilter);
                    }

                    dgConfirmationStats.DataSource = _agentFilters;
                    dgConfirmationStats.FieldLayouts[0].Fields["FKINImportID"].Visibility = Visibility.Collapsed;
                }

                #endregion Populate _agentFilters

                EnableDisableControls(true);
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

        private void calFromDate_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calFromDate.SelectedDate.ToString(), out _fromDate);
            EnableDisableExportButton();
        }

        private void calToDate_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calToDate.SelectedDate.ToString(), out _toDate);
            EnableDisableExportButton();
        }

        
    }
}
