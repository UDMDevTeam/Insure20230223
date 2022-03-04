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
using System.Collections.Generic;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportBumpUpStatsScreen
    {

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
        private string _selectedYear = "";
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private readonly long _mode;
        private int _timer1;
        private bool? _isReportRunning = false;

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

        public ReportBumpUpStatsScreen(long mode)
        {
            InitializeComponent();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
            this._mode = mode;



            cmbCampaign.Items.Add("2019");
            cmbCampaign.Items.Add("2020");
            cmbCampaign.Items.Add("2021");
            cmbCampaign.Items.Add("2022");

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

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);
                
                #region Setup Excel document

                string filePathAndName = String.Format("{0}Bump-Up Statistics Report, {1} - {2} ~ {3}.xlsx",
                    GlobalSettings.UserFolder,
                    _fromDate.ToString("yyyy-MM-dd"),
                    _toDate.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                Workbook wbBumpUpStatsReportTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateBumpUpStats.xlsx");
                Workbook wbBumpUpStatsReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup Excel document

                #region Get the data

                DataSet dsBumpUpStatsReportData = Business.Insure.INReportBumpUpStatistics(_fromDate, _toDate, _mode, _selectedYear);

                if (dsBumpUpStatsReportData.Tables[2].Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                #endregion Get the data

                InsertBumpUpStatsSheets(wbBumpUpStatsReportTemplate, wbBumpUpStatsReport, dsBumpUpStatsReportData);

                InsertBumpUpDataSheet(wbBumpUpStatsReportTemplate, wbBumpUpStatsReport, dsBumpUpStatsReportData);

                #region Finally, save, and display the resulting workbook

                if (wbBumpUpStatsReport.Worksheets.Count > 0)
                {
                    wbBumpUpStatsReport.Save(filePathAndName);

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

        private void InsertBumpUpDataSheet(Workbook wbBumpUpStatsReportTemplate, Workbook wbBumpUpStatsReport, DataSet dsBumpUpStatsReportData)
        {
            try
            {
                int rowIndex = 7;
                int columnSpan = 16;
                DataTable dtBumpUpDataSheet = dsBumpUpStatsReportData.Tables[6];

                DataTable dtReportConfig = dsBumpUpStatsReportData.Tables[0];
                

                Worksheet wsTemplate = wbBumpUpStatsReportTemplate.Worksheets["DataSheet"];
                Worksheet wsReport = wbBumpUpStatsReport.Worksheets.Add("BumpUpDatasheet");

                //Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsReport, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true);

                Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                Methods.CopyExcelRegion(wsTemplate, 0, 0, 6, columnSpan, wsReport, 0, 0);

                wsReport.GetCell("Subtitle").Value = dtReportConfig.Rows[0]["DataSheetSubTitle"].ToString();
                wsReport.GetCell("ReportDate1").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                wsReport.Workbook.NamedReferences.Clear();

                foreach (DataRow row in dtBumpUpDataSheet.Rows)
                {
                    Methods.CopyExcelRegion(wsTemplate, 7, 0, 0, columnSpan, wsReport, rowIndex, 0);

                    wsReport.GetCell("RefNo").Value = row["Ref No"].ToString();
                    wsReport.GetCell("Campaign").Value = row["Code"].ToString();
                    wsReport.GetCell("DateOfSale").Value = row["Date"].ToString() != "" ? DateTime.Parse(row["Date"].ToString()).ToString("yyyy-MM-dd") : "";
                    wsReport.GetCell("TSR").Value = row["TSR"].ToString();
                    wsReport.GetCell("SaleTime").Value = row["SaleTime"].ToString();
                    wsReport.GetCell("BumpUpAllocation").Value = row["ConfirmationAgent"].ToString();
                    wsReport.GetCell("DateAllocated").Value = row["DateAllocated"].ToString() != "" ? DateTime.Parse(row["DateAllocated"].ToString()).ToString("yyyy-MM-dd") : "";
                    wsReport.GetCell("DateContacted").Value = row["ContactDate"].ToString() != "" ? DateTime.Parse(row["ContactDate"].ToString()).ToString("yyyy-MM-dd") : "";
                    wsReport.GetCell("TimeContacted").Value = row["ContactTime"].ToString();
                    wsReport.GetCell("Status").Value = row["Status"].ToString();
                    wsReport.GetCell("ContactStatus").Value = row["ContactStatus"].ToString();
                    wsReport.GetCell("BumpedUp").Value = row["BumpedUp"].ToString();
                    wsReport.GetCell("OfferedBumpUp").Value = row["OfferedBumpUp"].ToString();
                    wsReport.GetCell("BumpUpDescription").Value = row["BumpUpDescription"].ToString();
                    wsReport.GetCell("BumpUpReducedPremiumStatus").Value = row["BumpUpReducedPremiumStatus"].ToString();
                    wsReport.GetCell("OldPremium").Value = row["OldPremium"].ToString();
                    wsReport.GetCell("NewPremium").Value = row["NewPremium"].ToString();

                    wsReport.Workbook.NamedReferences.Clear();
                    rowIndex++;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void InsertBumpUpStatsSheets(Workbook wbBumpUpStatsReportTemplate, Workbook wbBumpUpStatsReport, DataSet dsBumpUpStatsReportData)
        {
            #region Partition the given dataset

            DataTable dtReportSheets = dsBumpUpStatsReportData.Tables[0];
            DataTable dtReportData;
            DataTable dtExcelSheetDataTableColumnMappings;
            DataTable dtExcelCellTotalsFormulasMappings;
            DataTable dtExcelCellGrandTotalFormulasMappings;

            #endregion Partition the given dataset

            #region Declarations & Initializations

            int reportRow = 6;
            int formulaStartRow = reportRow;

            byte dataTableIndex = 0;
            byte excelDataTableColumnMappingsTableIndex = 0;
            byte excelCellTotalsFormulasMappingsTableIndex = 0;
            byte excelCellGrandTotalFormulasMappingsTableIndex = 0;

            string templateSheetName;
            string reportSheetName;

            string reportSubtitle;
            string reportSubHeadingCell;
            string reportDateCell;

            string filterString;
            string orderByString;

            byte templateDataSheetRowSpan;
            byte templateColumnSpan;
            byte templateRowIndex;
            byte totalsTemplateRowIndex;
            byte templateGrandTotalRowSpan;

            #endregion Declarations & Initializations

            foreach (DataRow row in dtReportSheets.Rows)
            {
                
                if (_mode > 0 && Convert.ToString(row["ReportSheetName"]) == "Summary")
                {
                    continue;
                }

                dataTableIndex = Convert.ToByte(row["DataTableIndex"]);
                excelDataTableColumnMappingsTableIndex = Convert.ToByte(row["ExcelDataTableColumnMappingsTableIndex"]);
                excelCellTotalsFormulasMappingsTableIndex = Convert.ToByte(row["ExcelCellTotalsFormulasMappingsTableIndex"]);
                excelCellGrandTotalFormulasMappingsTableIndex = Convert.ToByte(row["ExcelCellGrandTotalFormulasMappingsTableIndex"]);

                //dtReportData = dsBumpUpStatsReportData.Tables[dataTableIndex];
                dtExcelSheetDataTableColumnMappings = dsBumpUpStatsReportData.Tables[excelDataTableColumnMappingsTableIndex];
                dtExcelCellTotalsFormulasMappings = dsBumpUpStatsReportData.Tables[excelCellTotalsFormulasMappingsTableIndex];
                dtExcelCellGrandTotalFormulasMappings = dsBumpUpStatsReportData.Tables[excelCellGrandTotalFormulasMappingsTableIndex];

                #region Do the needed filtering where needed

                filterString = row["FilterString"].ToString();
                orderByString = row["OrderByString"].ToString();
                dtReportData = new DataTable();

                if (String.IsNullOrEmpty(filterString))
                {
                    dtReportData = dsBumpUpStatsReportData.Tables[dataTableIndex];
                }
                else
                {
                    var filteredRows = dsBumpUpStatsReportData.Tables[dataTableIndex].Select(filterString).AsEnumerable();
                    if (filteredRows.Any())
                    {
                        dtReportData = dsBumpUpStatsReportData.Tables[dataTableIndex].Select(filterString, orderByString).CopyToDataTable();
                    }
                }

                #endregion Do the needed filtering where needed

                //After the filtering was applied, are there any rows in the resulting data table?

                if (dtReportData.Rows.Count > 0)
                {
                    #region Initialize

                    //dtReportData = dsBumpUpStatsReportData.Tables[dataTableIndex];
                    //dtExcelSheetDataTableColumnMappings = dsBumpUpStatsReportData.Tables[excelDataTableColumnMappingsTableIndex];
                    //dtExcelCellTotalsFormulasMappings = dsBumpUpStatsReportData.Tables[excelCellTotalsFormulasMappingsTableIndex];
                    //dtExcelCellGrandTotalFormulasMappings = dsBumpUpStatsReportData.Tables[excelCellGrandTotalFormulasMappingsTableIndex];

                    templateSheetName = row["TemplateSheetName"].ToString();
                    reportSheetName = row["ReportSheetName"].ToString();
                    reportSubtitle = row["ReportSubtitle"].ToString();
                    reportSubHeadingCell = row["ReportSubtitleCell"].ToString();
                    reportDateCell = row["ReportDateCell"].ToString();

                    templateDataSheetRowSpan = Convert.ToByte(row["TemplateDataSheetRowSpan"]);
                    templateColumnSpan = Convert.ToByte(row["TemplateColumnSpan"]);
                    templateRowIndex = Convert.ToByte(row["TemplateRowIndex"]);
                    totalsTemplateRowIndex = Convert.ToByte(row["TemplateTotalRowIndex"]);
                    templateGrandTotalRowSpan = Convert.ToByte(row["TemplateGrandTotalRowSpan"]);

                    #endregion Initialize

                    #region Add the worksheet

                    Worksheet wsReportTemplate = wbBumpUpStatsReportTemplate.Worksheets[templateSheetName];
                    Worksheet wsReport = wbBumpUpStatsReport.Worksheets.Add(reportSheetName);
                    Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);
                    wsReport.Workbook.NamedReferences.Clear();

                    #endregion Add the worksheet

                    #region Populating the report details

                    Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
                    wsReport.Workbook.NamedReferences.Clear();
                    wsReport.GetCell(reportSubHeadingCell).Value = reportSubtitle;
                    wsReport.GetCell("A1").Value = wsReport.GetCell("A1").Value + " - " + reportSheetName;
                    wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    wsReport.Columns[1].Hidden = true;

                    #endregion Populating the report details

                    #region Add the report data

                    reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtReportData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);
                    //reportRow = 6;

                    #endregion Add the report data

                    #region Add the totals / averages
                    
                    reportRow = Methods.MapTemplatizedExcelFormulas(wsReportTemplate, dtExcelCellTotalsFormulasMappings, totalsTemplateRowIndex, 0, templateGrandTotalRowSpan, templateColumnSpan, wsReport, reportRow, 0, formulaStartRow, reportRow - 1);

                    if (String.IsNullOrEmpty(filterString))
                    {
                        DataTable dtSummaryTotals = dsBumpUpStatsReportData.Tables[7];
                        wsReport.GetCell("F" + reportRow).Value = dtSummaryTotals.Rows[0]["ContactRate"];
                    }
                    else
                    {
                        DataTable dtBUAgentTotals = dsBumpUpStatsReportData.Tables[8];
                        var filteredRows = dtBUAgentTotals.Select(filterString).AsEnumerable();
                        if (filteredRows.Any())
                        {
                            wsReport.GetCell("F" + reportRow).Value = dtBUAgentTotals.Select(filterString).FirstOrDefault()["ContactRate"];
                        }

                    }
                    //reportRow = Methods.Temp

                    #endregion Add the totals / averages

                    #region Add the grand totals

                    string grandTotalColumn = dtExcelCellGrandTotalFormulasMappings.Rows[0]["ExcelColumn"].ToString();
                    string grandTotalFormula = dtExcelCellGrandTotalFormulasMappings.Rows[0]["ValueOrFormula"].ToString().Replace("#ROW#", reportRow.ToString());

                    wsReport.GetCell(String.Format("{0}{1}", grandTotalColumn, reportRow + 1)).ApplyFormula(grandTotalFormula);

                    #endregion Add the grand totals

                    reportRow = 6;
                    formulaStartRow = reportRow;
                }
            }
        }

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";

            EnableAllControls(true);
        }

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;

            btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnReport.ToolTip = btnReport.Content;
        }

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
            btnReport.IsEnabled = isEnabled;
            calFromDate.IsEnabled = isEnabled;
            calToDate.IsEnabled = isEnabled;
        }

        #endregion Event Handlers

        private void cmbCampaign_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _selectedYear = cmbCampaign.SelectedValue.ToString();
        }
    }
}
