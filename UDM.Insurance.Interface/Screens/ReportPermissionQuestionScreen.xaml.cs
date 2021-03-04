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
using System.IO;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportPermissionQuestionScreen
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
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
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

        public ReportPermissionQuestionScreen()
        {
            InitializeComponent();

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

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup Excel document

                string filePathAndName = String.Format("{0}Permission Question Report, {1} - {2} ~ {3}.xlsx",
                   GlobalSettings.UserFolder,
                   _fromDate.ToString("yyyy-MM-dd"),
                   _toDate.ToString("yyyy-MM-dd"),
                   DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                Workbook wbPermissionQuestionReportTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplatePermissionQuestion.xlsx");
                Workbook wbPermissionQuestionReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup Excel document

                #region GetSales

                DataSet dsPermissionQuestion = Business.Insure.INReportPermissionQuestion(_fromDate, _toDate);

                //DataTable dtPermissionQuestionData = dsPermissionQuestion.Tables[0];
                //DataTable dtExcelMappings = dsPermissionQuestion.Tables[1];

                if (dsPermissionQuestion.Tables[1].Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                #endregion GetSales

                //#region Export To CSV
                //var lines = new List<string>();

                //string[] columnNames = dtPermissionQuestionData.Columns.Cast<DataColumn>().
                //                                  Select(column => column.ColumnName).
                //                                  ToArray();

                //var header = string.Join(";", columnNames);
                //lines.Add(header);

                //var valueLines = dtPermissionQuestionData.AsEnumerable()
                //                   .Select(row => string.Join(";", row.ItemArray));
                //lines.AddRange(valueLines);

                //string filePathAndName = String.Format("{0}Permission Question Report, {1} - {2} ~ {3}.csv",
                //   GlobalSettings.UserFolder,
                //   _fromDate.ToString("yyyy-MM-dd"),
                //   _toDate.ToString("yyyy-MM-dd"),
                //   DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                //File.WriteAllLines(filePathAndName, lines);

                //Process.Start(filePathAndName);
                //#endregion

                //foreach ()
                InsertPermissionQuestionSheets(wbPermissionQuestionReportTemplate, wbPermissionQuestionReport, dsPermissionQuestion);


                #region ExportToExcel
                //Workbook wb = new Workbook();
                //wb.Worksheets.Add("PL Daily Sales");
                //string dateString = "";
                //string wbFilePath = "";



                //string filePathAndName = String.Format("{0}Permission Question Report, {1} - {2} ~ {3}.xlsx",
                //   GlobalSettings.UserFolder,
                //   _fromDate.ToString("yyyy-MM-dd"),
                //   _toDate.ToString("yyyy-MM-dd"),
                //   DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                //wb.Save(filePathAndName);

                ////Display excel document
                //Process.Start(filePathAndName);

                if (wbPermissionQuestionReport.Worksheets.Count > 0)
                {
                    wbPermissionQuestionReport.Save(filePathAndName);

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
                #endregion ExportToExcel

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

        private void InsertPermissionQuestionSheets(Workbook wbPermissionQuestionReportTemplate, Workbook wbPermissionQuestionReport, DataSet dsPermissionQuestionReportData)
        {
            #region Partition the given dataset

            DataTable dtReportSheets = dsPermissionQuestionReportData.Tables[0];
            DataTable dtReportData;
            DataTable dtExcelSheetDataTableColumnMappings;

            #endregion Partition the given dataset

            #region Declarations & Initializations

            int reportRow = 7;
            //int formulaStartRow = reportRow;

            byte dataTableIndex = 0;
            byte excelDataTableColumnMappingsTableIndex = 0;

            string templateSheetName;
            string reportSheetName;

            string reportTitle;
            string reportSubtitle;
            string reportSubHeadingCell;
            string reportDateCell;

            string filterString;
            //string orderByString;

            byte templateDataSheetRowSpan;
            byte templateColumnSpan;
            byte templateRowIndex;

            #endregion Declarations & Initializations

            foreach (DataRow row in dtReportSheets.Rows)
            {
                reportRow = 7;
                dataTableIndex = Convert.ToByte(row["DataTableIndex"]);
                excelDataTableColumnMappingsTableIndex = Convert.ToByte(row["ExcelDataTableColumnMappingsTableIndex"]);

                //dtReportData = dsPermissionQuestionReportData.Tables[dataTableIndex];
                dtExcelSheetDataTableColumnMappings = dsPermissionQuestionReportData.Tables[excelDataTableColumnMappingsTableIndex];

                string columnNameFirst = dtExcelSheetDataTableColumnMappings.Rows[0][0].ToString();
                string columnNameLast = dtExcelSheetDataTableColumnMappings.Rows[dtExcelSheetDataTableColumnMappings.Rows.Count - 1][0].ToString();

                #region Do the needed filtering where needed

                filterString = row["FilterString"].ToString();
                //orderByString = row["OrderByString"].ToString();
                dtReportData = new DataTable();

                if (String.IsNullOrEmpty(filterString))
                {
                    dtReportData = dsPermissionQuestionReportData.Tables[dataTableIndex];
                }
                else
                {
                    var filteredRows = dsPermissionQuestionReportData.Tables[dataTableIndex].Select(filterString).AsEnumerable();
                    if (filteredRows.Any())
                    {
                        dtReportData = dsPermissionQuestionReportData.Tables[dataTableIndex].Select(filterString).CopyToDataTable();
                    }
                }

                #endregion Do the needed filtering where needed

                //After the filtering was applied, are there any rows in the resulting data table?

                if (dtReportData.Rows.Count > 0)
                {
                    #region Initialize

                    //dtReportData = dsPermissionQuestionReportData.Tables[dataTableIndex];
                    //dtExcelSheetDataTableColumnMappings = dsPermissionQuestionReportData.Tables[excelDataTableColumnMappingsTableIndex];
                    //dtExcelCellTotalsFormulasMappings = dsPermissionQuestionReportData.Tables[excelCellTotalsFormulasMappingsTableIndex];
                    //dtExcelCellGrandTotalFormulasMappings = dsPermissionQuestionReportData.Tables[excelCellGrandTotalFormulasMappingsTableIndex];

                    templateSheetName = row["TemplateSheetName"].ToString();
                    reportSheetName = row["ReportSheetName"].ToString();
                    reportTitle = "Permission Question Report for " + row["ReportSheetName"].ToString() + " Campaigns";
                    reportSubtitle = row["ReportSubtitle"].ToString();
                    reportSubHeadingCell = row["ReportSubtitleCell"].ToString();
                    reportDateCell = row["ReportDateCell"].ToString();

                    templateDataSheetRowSpan = Convert.ToByte(row["TemplateDataSheetRowSpan"]);
                    templateColumnSpan = Convert.ToByte(row["TemplateColumnSpan"]);
                    templateRowIndex = Convert.ToByte(row["TemplateRowIndex"]);

                    #endregion Initialize

                    #region Add the worksheet

                    Worksheet wsReportTemplate = wbPermissionQuestionReportTemplate.Worksheets[templateSheetName];
                    Worksheet wsReport = wbPermissionQuestionReport.Worksheets.Add(reportSheetName);
                    
                    Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                    #endregion Add the worksheet

                    #region Populating the report details

                    Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
                    wsReport.GetCell("A1").Value = reportTitle;
                    wsReport.GetCell(reportSubHeadingCell).Value = reportSubtitle;
                    wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    #endregion Populating the report details

                    #region Add the report data

                    reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtReportData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);
                    //reportRow = 6;
                    //wsReport(reportSheetName).Columns(columnNameFirst + ":" + columnNameLast).AutoFit();
                    //wsReport.Columns.AutoFit();
                    //for (int x = 0; x < templateColumnSpan; x++)
                    //{
                    //    //wsReport.Columns[x].CellFormat.ShrinkToFit = ExcelDefaultableBoolean.True;
                    //    wsReport.Columns[x].CellFormat.ShrinkToFit = ExcelDefaultableBoolean.Default;
                    //}
                

                    #endregion Add the report data

                    //#region Add the totals / averages

                    //reportRow = Methods.MapTemplatizedExcelFormulas(wsReportTemplate, dtExcelCellTotalsFormulasMappings, totalsTemplateRowIndex, 0, templateGrandTotalRowSpan, templateColumnSpan, wsReport, reportRow, 0, formulaStartRow, reportRow - 1);

                    //#endregion Add the totals / averages

                    //#region Add the grand totals

                    //string grandTotalColumn = dtExcelCellGrandTotalFormulasMappings.Rows[0]["ExcelColumn"].ToString();
                    //string grandTotalFormula = dtExcelCellGrandTotalFormulasMappings.Rows[0]["ValueOrFormula"].ToString().Replace("#ROW#", reportRow.ToString());

                    //wsReport.GetCell(String.Format("{0}{1}", grandTotalColumn, reportRow + 1)).ApplyFormula(grandTotalFormula);

                    //#endregion Add the grand totals

                    //reportRow = 6;
                    //formulaStartRow = reportRow;
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

    }
}
