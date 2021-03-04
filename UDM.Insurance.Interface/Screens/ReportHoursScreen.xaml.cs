using System.Linq;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Resources;
using System.Windows.Threading;
using Infragistics.Windows.Editors.Events;
using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportHoursScreen
    {


        #region HoursReportType Enumerator

        public enum HoursReportType
        {
            AdministrativePersonnel = 1,
            SalesConsultants = 2
        }

        #endregion HoursReportType Enumerator

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

        //private DateTime? _reportStartDate;
        //private DateTime? _reportEndDate;
        //private string _reportStartDateLongFormat;
        //private string _reportEndDateLongFormat;

        //private string _strTodaysDate;
        //private string _strTodaysDateIncludingColons;

        byte _hoursReportType;

        //private DataRow _campaignCluster;
        //private long? _campaignID;
        //string _agentIDs = String.Empty;

        private string _liveDebugTestIndicator = String.Empty;

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

        public ReportHoursScreen()
        {
            InitializeComponent();

            //LoadCampaignClusterInfo();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);

            #if DEBUG
                _liveDebugTestIndicator = "DEBUG";
            #elif TESTBUILD
                _liveDebugTestIndicator = "TEST";
            #else
                 _liveDebugTestIndicator = String.Empty;
            #endif
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
                //_reportStartDateLongFormat = calFromDate.SelectedDate.Value.ToString("dddd, dd MMMM yyyy");
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the From Date was specified


            if (calToDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'To Date'.", @"No 'To Date' specified", ShowMessageType.Error);
                return false;
            }
            else
            {
                _toDate = calToDate.SelectedDate.Value;
                //_reportEndDateLongFormat = calToDate.SelectedDate.Value.ToString("dddd, dd MMMM yyyy");
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the date range is valid

            if (_fromDate > _toDate)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Invalid date range specified: The 'From Date' can not be greater than the 'To Date'.", "Invalid date range", ShowMessageType.Error);
                return false;
            }

            #endregion Ensuring that the date range is valid

            // Otherwise, if all is well, proceed:
            return true;
        }

        //private void ReportOLD(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        SetCursor(Cursors.Wait);

        //        #region Setup Excel document
        //        int worksheetCount = 0;

        //        var wbSalaryReport = new Workbook();
        //        wbSalaryReport.SetCurrentFormat(WorkbookFormat.Excel2007);

        //        string filePathAndName = String.Format("{0}Hours Report {1} - {2} ~ {3}.xlsx",
        //            GlobalSettings.UserFolder,
        //            _fromDate.ToString("yyyy-MM-dd"),
        //            _toDate.ToString("yyyy-MM-dd"),
        //            DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

        //        Workbook wbTemplate;
        //        Uri uri = new Uri("/Templates/ReportTemplateHoursReport.xlsx", UriKind.Relative);
        //        StreamResourceInfo info = Application.GetResourceStream(uri);
        //        if (info != null)
        //        {
        //            wbTemplate = Workbook.Load(info.Stream, true);
        //        }
        //        else
        //        {
        //            return;
        //        }

        //        Worksheet wsHoursReportSummarySheetTemplate;
        //        Worksheet wsHoursReportIndividualSheetTemplate;

        //        Worksheet wsHoursReportSummarySheet;
        //        Worksheet wsHoursReportIndividualSheet;

        //        if (_hoursReportType == HoursReportType.AdministrativePersonnel)
        //        {
        //            wsHoursReportSummarySheetTemplate = wbTemplate.Worksheets["Summary - Admin"];
        //            wsHoursReportIndividualSheetTemplate = wbTemplate.Worksheets["Administrative Personnel"];
        //        }
        //        else
        //        {
        //            wsHoursReportSummarySheetTemplate = wbTemplate.Worksheets["Summary - Sales"];
        //            wsHoursReportIndividualSheetTemplate = wbTemplate.Worksheets["Sales Consultants"];
        //        }

        //        //wsHoursReportSummarySheet = wbSalaryReport.Worksheets.Add("Summary");

        //        #endregion Setup Excel document

        //        string excelFormulaColumn;

        //        #region Get the data for the summary page from database

        //        SqlParameter[] parameters = new SqlParameter[3];
        //        parameters[0] = new SqlParameter("@ReportType", (byte)_hoursReportType);
        //        parameters[1] = new SqlParameter("@FromDate", _fromDate);
        //        parameters[2] = new SqlParameter("@ToDate", _toDate);

        //        DataTable dtHoursReportSummaryPageDataTable = Methods.ExecuteStoredProcedure("spINReportHoursSummaryPage", parameters).Tables[0]; 

        //        #endregion Get the data for the summary page from database

        //        #region Adding the summary sheet

        //        wsHoursReportSummarySheet = wbSalaryReport.Worksheets.Add("Summary");

        //        worksheetCount++;

        //        #endregion Adding the summary sheet

        //        if (dtHoursReportSummaryPageDataTable.Rows.Count > 0)
        //        {
        //            int reportRow = 7;

        //            #region Copy the summary template formatting

        //            Methods.CopyExcelRegion(wsHoursReportSummarySheetTemplate, 0, 0, 8, dtHoursReportSummaryPageDataTable.Columns.Count - 1, wsHoursReportSummarySheet, 0, 0);

        //            #endregion Copy the summary template formatting

        //            #region Populating the details of the Summary sheet

        //            wsHoursReportSummarySheet.GetCell("A3").Value = String.Format("{0} to {1}", _fromDate.ToString("d MMMM yyyy"), _toDate.ToString("d MMMM yyyy"));
        //            wsHoursReportSummarySheet.GetCell("A5").Value = String.Format("Date: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        //            #endregion Populating the details of the Summary sheet

        //            for (int a = 0; a < dtHoursReportSummaryPageDataTable.Rows.Count; a++)
        //            {
        //                #region Loop through each row in the data table

        //                Methods.CopyExcelRegion(wsHoursReportSummarySheetTemplate, 7, 0, 0, dtHoursReportSummaryPageDataTable.Columns.Count - 1, wsHoursReportSummarySheet, reportRow, 0);

        //                for (int summaryDataTableColumnIndex = 1; summaryDataTableColumnIndex < dtHoursReportSummaryPageDataTable.Columns.Count; summaryDataTableColumnIndex++)
        //                {
        //                    excelFormulaColumn = ExcelUtils.GetExcelColumnNameFromColumnIndex(summaryDataTableColumnIndex);
        //                    wsHoursReportSummarySheet.GetCell(excelFormulaColumn + (reportRow + 1).ToString()).Value = dtHoursReportSummaryPageDataTable.Rows[a][summaryDataTableColumnIndex];
        //                }

        //                ++reportRow;

        //                #endregion Loop through each row in the data table

        //                #region Adding a separate sheet for each employee

        //                long currentUserID = (long)dtHoursReportSummaryPageDataTable.Rows[a]["FKUserID"];

        //                #region Get the data for the individual page from database

        //                SqlParameter[] individualReportParameters = new SqlParameter[4];
        //                individualReportParameters[0] = new SqlParameter("@UserID", currentUserID);
        //                individualReportParameters[1] = new SqlParameter("@ReportType", (byte)_hoursReportType);
        //                individualReportParameters[2] = new SqlParameter("@FromDate", _fromDate);
        //                individualReportParameters[3] = new SqlParameter("@ToDate", _fromDate);

        //                DataTable dtIndividualHoursDataTable = Methods.ExecuteStoredProcedure("spINReportHoursPerUser", individualReportParameters).Tables[0];

        //                #endregion Get the data for the individual page from database

        //                if (dtIndividualHoursDataTable.Rows.Count > 0)
        //                {
        //                    int individualReportRow = 7;

        //                    #region Adding the individual sheet

        //                    string userName = dtHoursReportSummaryPageDataTable.Rows[a]["AgentName"].ToString();

        //                    if (userName.Length > 31)
        //                    {
        //                        wsHoursReportIndividualSheet = wbSalaryReport.Worksheets.Add(String.Format("{0}...", userName.Substring(0, 28)));
        //                    }
        //                    else
        //                    {
        //                        wsHoursReportIndividualSheet = wbSalaryReport.Worksheets.Add(userName);
        //                    }

        //                    worksheetCount++;

        //                    #endregion Adding the individual sheet

        //                    #region Copy the individual template formatting

        //                    Methods.CopyExcelRegion(wsHoursReportIndividualSheetTemplate, 0, 0, 8, dtIndividualHoursDataTable.Columns.Count - 1, wsHoursReportIndividualSheet, 0, 0);

        //                    #endregion Copy the individual template formatting

        //                    #region Populating the details of the Individual sheet

        //                    wsHoursReportIndividualSheet.GetCell("A1").Value = String.Format("Hours Report - {0}", userName);
        //                    wsHoursReportIndividualSheet.GetCell("A3").Value = String.Format("{0} to {1}", _fromDate.ToString("d MMMM yyyy"), _toDate.ToString("d MMMM yyyy"));
        //                    wsHoursReportIndividualSheet.GetCell("A5").Value = String.Format("Date: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        //                    #endregion Populating the details of the Individual sheet

        //                    for (int b = 0; b < dtIndividualHoursDataTable.Rows.Count; b++)
        //                    {
        //                        #region Loop through each row in the data table

        //                        Methods.CopyExcelRegion(wsHoursReportIndividualSheetTemplate, 7, 0, 0, dtIndividualHoursDataTable.Columns.Count - 1, wsHoursReportIndividualSheet, individualReportRow, 0);

        //                        for (int individualDataTableColumnIndex = 0; individualDataTableColumnIndex < dtIndividualHoursDataTable.Columns.Count; individualDataTableColumnIndex++)
        //                        {
        //                            excelFormulaColumn = ExcelUtils.GetExcelColumnNameFromColumnIndex(individualDataTableColumnIndex + 1);

        //                            switch (dtIndividualHoursDataTable.Columns[individualDataTableColumnIndex].ColumnName)
        //                            {
        //                                case "MorningShiftStartTime":
        //                                case "MorningShiftEndTime":

        //                                case "NormalShiftStartTime":
        //                                case "NormalShiftEndTime":

        //                                case "EveningShiftStartTime":
        //                                case "EveningShiftEndTime":

        //                                case "PublicHolidayWeekendShiftStartTime":
        //                                case "PublicHolidayWeekendShiftEndTime":
        //                                    wsHoursReportIndividualSheet.GetCell(excelFormulaColumn + (individualReportRow + 1).ToString()).CellFormat.FormatString = "HH:mm";
        //                                    break;
        //                            }

        //                            wsHoursReportIndividualSheet.GetCell(excelFormulaColumn + (individualReportRow + 1).ToString()).Value = dtIndividualHoursDataTable.Rows[b][individualDataTableColumnIndex];


        //                            //switch (individualDataTableColumnIndex)
        //                            //{
        //                            //    case 4:
        //                            //    case 5:
        //                            //    case 6:
        //                            //    case 7:
        //                            //    case 8:
        //                            //    case 9:
        //                            //    case 10:
        //                            //        wsHoursReportIndividualSheet.GetCell(excelFormulaColumn + (individualReportRow + 1).ToString()).CellFormat.FormatString = "HH:mm";
        //                            //        break;
        //                            //}
        //                        }

        //                        ++individualReportRow;

        //                        #endregion Loop through each row in the data table
        //                    }

        //                    #region Adding the "Totals" row for the current individual hours sheet

        //                    Methods.CopyExcelRegion(wsHoursReportIndividualSheetTemplate, 8, 0, 0, dtIndividualHoursDataTable.Columns.Count - 1, wsHoursReportIndividualSheet, individualReportRow, 0);

        //                    for (int individualHoursDataTableColumnIndexTotals = dtIndividualHoursDataTable.Columns.Count - 3; individualHoursDataTableColumnIndexTotals < dtIndividualHoursDataTable.Columns.Count; individualHoursDataTableColumnIndexTotals++)
        //                    {
        //                        excelFormulaColumn = ExcelUtils.GetExcelColumnNameFromColumnIndex(individualHoursDataTableColumnIndexTotals + 1);

        //                        wsHoursReportIndividualSheet.GetCell(excelFormulaColumn + (individualReportRow + 1).ToString()).ApplyFormula(String.Format("=SUM({0}{1}:{0}{2})", excelFormulaColumn, 8, individualReportRow));
        //                    }

        //                    #endregion Adding the "Totals" row for the current individual hours sheet
        //                }

        //                #endregion Adding a separate sheet for each employee
        //            }

        //            #region Adding the "Totals" row for the Summary sheet

        //            Methods.CopyExcelRegion(wsHoursReportSummarySheetTemplate, 8, 0, 0, dtHoursReportSummaryPageDataTable.Columns.Count - 1, wsHoursReportSummarySheet, reportRow, 0);

        //            for (int summaryDataTableColumnIndexTotals = dtHoursReportSummaryPageDataTable.Columns.Count - 6; summaryDataTableColumnIndexTotals < dtHoursReportSummaryPageDataTable.Columns.Count; summaryDataTableColumnIndexTotals++)
        //            {
        //                excelFormulaColumn = ExcelUtils.GetExcelColumnNameFromColumnIndex(summaryDataTableColumnIndexTotals);
        //                //wsHoursReportSummarySheet.GetCell(excelFormulaColumn + (reportRow + 1).ToString()).Value = dtHoursReportSummaryPageDataTable.Rows[0][summaryDataTableColumnIndexTotals];
        //                wsHoursReportSummarySheet.GetCell(excelFormulaColumn + (reportRow + 1).ToString()).ApplyFormula(String.Format("=SUM({0}{1}:{0}{2})", excelFormulaColumn, 8, reportRow));
        //            }

        //            //++reportRow;


        //            //Methods.CopyExcelRegion(wsSalaryReportTemplate, reportTemplateRowIndex, 0, 0, dtSalaryReportDataTable.Columns.Count, wsSalaryReport, reportRow, 0);


        //            //for (int b = 1; b < dtSalaryReportDataTable.Columns.Count; b++)
        //            //{
        //            //    excelFormulaColumn = ExcelUtils.GetExcelColumnNameFromColumnIndex(b + 1);
        //            //    wcSalaryReportCell = wsSalaryReport.Rows[reportRow].Cells[b];

        //            //    wcSalaryReportCell.ApplyFormula(String.Format("=SUM({0}{1}:{0}{2})", excelFormulaColumn, formulaStartingRowIndex + 1, reportRow));

        //            //}
        //            //wsSalaryReportTemplate.DisplayOptions.ShowFormulasInCells = false;

        //            #endregion Adding the "Totals" row for the Summary sheet

        //            wbSalaryReport.Save(filePathAndName);

        //            //Display excel document
        //            Process.Start(filePathAndName);

        //        }
        //        else
        //        {
        //            //emptyDataTableCount++;
        //            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
        //            {
        //                ShowMessageBox(new INMessageBoxWindow1(), "There is no data available for the criteria you have specified. Please change some of them and try again.", "No data available", ShowMessageType.Information);
        //            });
        //        }


        //    //if (emptyDataTableCount == _lstSelectedCampaigns.Count)
        //    //{
        //    //    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
        //    //    {
        //    //        ShowMessageBox(new INMessageBoxWindow1(), "There is no data available for the criteria you have specified. Please change some of them and try again.", "No data available", Embriant.Framework.ShowMessageType.Information);
        //    //        return;
        //    //    });
        //    ////}
        //    //else
        //    //{


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

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Get the report data - and exit the method if there is no data available in the 2nd datatable

                DataSet dsHoursReportData = Business.Insure.INReportHours(_hoursReportType, _fromDate, _toDate);
                if (dsHoursReportData.Tables[1].Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "There is no data available for given date range. Please change either the from- or to date (or both dates) and try generating the report again.", "No data available", ShowMessageType.Information);
                    });

                    return;
                }

                #endregion Get the report data - and exit the method if there is no data available in the 2nd datatable

                #region Define Excel Workbook

                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateHoursReport.xlsx");
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                string filePathAndName = String.Empty;
                if (String.IsNullOrEmpty(_liveDebugTestIndicator))
                {
                    filePathAndName = String.Format("{0}Hours Report {1} - {2} ~ {3}.xlsx",
                    GlobalSettings.UserFolder,
                    _fromDate.ToString("yyyy-MM-dd"),
                    _toDate.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
                }
                else
                {
                    filePathAndName = String.Format("{0}Hours Report ({1}) - {2} - {3} ~ {4}.xlsx",
                    GlobalSettings.UserFolder,
                    _liveDebugTestIndicator,
                    _fromDate.ToString("yyyy-MM-dd"),
                    _toDate.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
                }

                #endregion Define Excel Workbook

                #region Add the summary sheets

                InsertSummarySheets(wbTemplate, wbReport, dsHoursReportData);

                #endregion Add the summary sheets

                #region Add the individual user's hour details sheets

                InsertIndividualSheets(wbTemplate, wbReport, dsHoursReportData);

                #endregion Add the individual user's hour details sheets

                #region Saves and opens the resulting Excel workbook - if there are any pages in the workbook

                if (wbReport.Worksheets.Count > 0)
                {
                    wbReport.Save(filePathAndName);

                    //Display excel document
                    Process.Start(filePathAndName);
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "The resulting workbook does not contain any worksheets, and therefor cannot be opened. The most common cause can be traced to the fact that there is no data available for the date range that was specified.", "Zero worksheets in workbook", ShowMessageType.Information);
                    });
                }

                #endregion Saves and opens the resulting Excel workbook - if there are any pages in the workbook
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

        private void InsertSummarySheets(Workbook wbTemplate, Workbook wbReport, DataSet dsReportData)
        {
            #region Partition the given dataset for the summary sheets - and define configs

            DataTable dtSummarySheets = dsReportData.Tables[0];
            //DataTable dtSummarySheets = dsHoursReportData.Tables[0];

            string summarySheetName;
            string summarySheetTemplateName;
            byte sourceDataTableIndex;
            string summaryFilterString;
            string summaryOrderByString;
            string summarySheetHeading;
            
            byte templateDataSheetRowSpan = 0;
            byte summarySheetTemplateColumnSpan = 0;
            byte summarySheetTemplateDataRowIndex = 7;
            byte summarySheetTemplateTotalsRowIndex = 8;

            int reportRow = 0;
            int formulaStartRow = 0;

            #endregion Partition the given dataset for the summary sheets - and define configs

            foreach (DataRow drSummarySheet in dtSummarySheets.Rows)
            {

                // Set the configs for the current summary sheet
                summarySheetName = drSummarySheet["SummarySheetName"].ToString();
                summarySheetTemplateName = drSummarySheet["SummarySheetTemplateName"].ToString();
                sourceDataTableIndex = Convert.ToByte(drSummarySheet["SourceDataTableIndex"]);
                summaryFilterString = drSummarySheet["SummaryFilterString"].ToString();
                summaryOrderByString = drSummarySheet["SummaryOrderByString"].ToString();
                summarySheetHeading = drSummarySheet["SummarySheetHeading"].ToString();
                summarySheetTemplateColumnSpan = Convert.ToByte(drSummarySheet["SummarySheetTemplateColumnSpan"]);

                templateDataSheetRowSpan = 6;
                reportRow = 7;

                Worksheet wsCurrentSummarySheet;
                Worksheet wsCurrentSummarySheetTemplate = wbTemplate.Worksheets[summarySheetTemplateName];
                

                DataTable dtCurrentSummarySheetData = new DataTable();

                #region Get the data for the current summary sheet

                if (String.IsNullOrEmpty(summaryFilterString))
                {
                    dtCurrentSummarySheetData = dsReportData.Tables[sourceDataTableIndex];
                }
                else
                {
                    var filteredRows = dsReportData.Tables[sourceDataTableIndex].Select(summaryFilterString).AsEnumerable();
                    if (filteredRows.Any())
                    {
                        dtCurrentSummarySheetData = dsReportData.Tables[sourceDataTableIndex].Select(summaryFilterString, summaryOrderByString).CopyToDataTable();
                    }
                }

                #endregion Get the data for the current summary sheet

                if (dtCurrentSummarySheetData.Rows.Count > 0)
                {
                    DataTable dtSummarySheetDataTableColumnMappings = dsReportData.Tables[4];
                    DataTable dtSummarySheetTotalsAndAverageColumnMappings = dsReportData.Tables[5];

                    #region Inserting the summary page(s)

                    wsCurrentSummarySheet = wbReport.Worksheets.Add(summarySheetName);
                    Methods.CopyWorksheetOptionsFromTemplate(wsCurrentSummarySheetTemplate, wsCurrentSummarySheet, true, true, true, true, true, true, true, false, true, true, true, true, true, true, true, true, false);

                    #endregion Inserting the summary page(s)

                    #region Copy the summary template formatting

                    Methods.CopyExcelRegion(wsCurrentSummarySheetTemplate, 0, 0, templateDataSheetRowSpan, summarySheetTemplateColumnSpan, wsCurrentSummarySheet, 0, 0);

                    #endregion Copy the summary template formatting

                    #region Populating the details of the Summary sheet

                    wsCurrentSummarySheet.GetCell("A1").Value = summarySheetHeading;
                    wsCurrentSummarySheet.GetCell("A3").Value = String.Format("{0} to {1}", _fromDate.ToString("d MMMM yyyy"), _toDate.ToString("d MMMM yyyy"));
                    wsCurrentSummarySheet.GetCell("A5").Value = String.Format("Date: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    #endregion Populating the details of the Summary sheet

                    #region Add the data

                    formulaStartRow = reportRow;
                    reportRow = Methods.MapTemplatizedExcelValues(wsCurrentSummarySheetTemplate, dtCurrentSummarySheetData, dtSummarySheetDataTableColumnMappings, summarySheetTemplateDataRowIndex, 0, 0, summarySheetTemplateColumnSpan, wsCurrentSummarySheet, reportRow, 0);

                    #endregion Add the data

                    #region Add the totals / averages

                    reportRow = Methods.MapTemplatizedExcelFormulas(wsCurrentSummarySheetTemplate, dtSummarySheetTotalsAndAverageColumnMappings, summarySheetTemplateTotalsRowIndex, 0, 0, summarySheetTemplateColumnSpan, wsCurrentSummarySheet, reportRow, 0, formulaStartRow, reportRow - 1);

                    #endregion Add the totals / averages
                }
            }
        }

        private void InsertIndividualSheets(Workbook wbTemplate, Workbook wbReport, DataSet dsReportData)
        {
            #region Partition the given dataset

            DataTable dtUsers = dsReportData.Tables[3];
            DataTable dtAllHoursData = dsReportData.Tables[1];
            DataTable dtIndividualHoursSheetDataTableColumnMappings = dsReportData.Tables[6];
            DataTable dtIndividualHoursSheetTotalsAndAverageColumnMappings = dsReportData.Tables[7];

            #endregion Partition the given dataset

            #region Declarations & initializations

            int reportRow = 0;
            int formulaStartRow = 0;
            int templateDataSheetRowSpan = 6;
            byte individualHoursSheetTemplateDataRowIndex = 7;
            byte individualHoursSheetTemplateTotalsRowIndex = 8;

            #endregion Declarations & initializations

            foreach (DataRow drUser in dtUsers.Rows)
            {
                string filterString = drUser["FilterString"].ToString();
                string orderByString = drUser["OrderByString"].ToString();
                
                var filteredRows = dtAllHoursData.Select(filterString).AsEnumerable();
                if (filteredRows.Any())
                {
                    DataTable dtCurrentUserHoursData = dtAllHoursData.Select(filterString, orderByString).CopyToDataTable();

                    #region Get the individual hours sheet configuration values

                    string agentName = drUser["AgentName"].ToString();
                    string heading = drUser["Heading"].ToString();
                    string initialWorksheetName = drUser["WorksheetName"].ToString();
                    string hoursClassification = drUser["HoursClassification"].ToString();
                    string individualTemplateSheetTemplate = drUser["IndividualTemplateSheetTemplate"].ToString();
                    byte individualTemplateSheetColumnSpan = Convert.ToByte(drUser["IndividualTemplateSheetColumnSpan"]);

                    reportRow = 7;

                    #endregion Get the individual hours sheet configuration values

                    #region Inserting the current user's hours sheet

                    string worksheetName = String.Empty;

                    if (hoursClassification == null)
                    {
                        worksheetName = Methods.ParseWorksheetName(wbReport, initialWorksheetName);
                    }
                    else
                    {
                        worksheetName = Methods.ParseWorksheetName(wbReport, initialWorksheetName, " ", hoursClassification);
                    }

                    Worksheet wsCurrentUserHoursTemplate = wbTemplate.Worksheets[individualTemplateSheetTemplate];
                    Worksheet wsCurrentUserHoursSheet = wbReport.Worksheets.Add(worksheetName);
                    Methods.CopyWorksheetOptionsFromTemplate(wsCurrentUserHoursTemplate, wsCurrentUserHoursSheet, true, true, true, true, true, true, true, false, true, true, true, true, true, true, true, true, false);

                    #endregion Inserting the current user's hours sheet

                    #region Copy the summary template formatting

                    Methods.CopyExcelRegion(wsCurrentUserHoursTemplate, 0, 0, templateDataSheetRowSpan, individualTemplateSheetColumnSpan, wsCurrentUserHoursSheet, 0, 0);

                    #endregion Copy the summary template formatting

                    #region Populating the details of the Summary sheet

                    wsCurrentUserHoursSheet.GetCell("A1").Value = heading;
                    wsCurrentUserHoursSheet.GetCell("A3").Value = String.Format("{0} to {1}", _fromDate.ToString("d MMMM yyyy"), _toDate.ToString("d MMMM yyyy"));
                    wsCurrentUserHoursSheet.GetCell("A5").Value = String.Format("Date: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    #endregion Populating the details of the Summary sheet

                    #region Add the data

                    formulaStartRow = reportRow;
                    reportRow = Methods.MapTemplatizedExcelValues(wsCurrentUserHoursTemplate, dtCurrentUserHoursData, dtIndividualHoursSheetDataTableColumnMappings, individualHoursSheetTemplateDataRowIndex, 0, 0, individualTemplateSheetColumnSpan, wsCurrentUserHoursSheet, reportRow, 0);

                    #endregion Add the data

                    #region Add the totals / averages

                    reportRow = Methods.MapTemplatizedExcelFormulas(wsCurrentUserHoursTemplate, dtIndividualHoursSheetTotalsAndAverageColumnMappings, individualHoursSheetTemplateTotalsRowIndex, 0, 0, individualTemplateSheetColumnSpan, wsCurrentUserHoursSheet, reportRow, 0, formulaStartRow, reportRow - 1);

                    #endregion Add the totals / averages
                }
            }


            
        }

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;

            if (_hoursReportType == (byte)HoursReportType.AdministrativePersonnel)
            {
                btnAdminHoursReport.Content = "Admin Hours Report";
            }
            else
            {
                btnSalesConsultantHoursReport.Content = "TSR Hours Report";
            }

            EnableAllControls(true);
        }

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;

            if (_hoursReportType == (byte)HoursReportType.AdministrativePersonnel)
            {
                btnAdminHoursReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
                btnAdminHoursReport.ToolTip = btnAdminHoursReport.Content;
            }
            else
            {
                btnSalesConsultantHoursReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
                btnSalesConsultantHoursReport.ToolTip = btnSalesConsultantHoursReport.Content;
            }
        }

        #endregion Private Methods

        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void btnAdminHoursReport_Click(object sender, RoutedEventArgs e)
        {
            _hoursReportType = (byte)HoursReportType.AdministrativePersonnel;
            ExecuteReportOperations();
        }

        private void btnSalesConsultantHoursReport_Click(object sender, RoutedEventArgs e)
        {
            _hoursReportType = (byte)HoursReportType.SalesConsultants;
            ExecuteReportOperations();
        }

        private void ExecuteReportOperations()
        {
            try
            {
                if (IsAllInputParametersSpecifiedAndValid())
                {
                    //_strTodaysDate = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
                    //_strTodaysDateIncludingColons = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    EnableAllControls(false);

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

        private void EnableAllControls(bool isEnabled)
        {
            btnClose.IsEnabled = isEnabled;
            btnAdminHoursReport.IsEnabled = isEnabled;
            btnSalesConsultantHoursReport.IsEnabled = isEnabled;
            calFromDate.IsEnabled = isEnabled;
            calToDate.IsEnabled = isEnabled;
        }

        private void calFromDate_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calFromDate.SelectedDate.ToString(), out _fromDate);
        }

        private void calToDate_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calToDate.SelectedDate.ToString(), out _toDate);
        }

        #endregion Event Handlers

    }
}
