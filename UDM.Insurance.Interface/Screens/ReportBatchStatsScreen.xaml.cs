using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
using Infragistics.Windows.Editors.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Resources;
using System.Windows.Threading;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using Orientation = Infragistics.Documents.Excel.Orientation;

namespace UDM.Insurance.Interface.Screens
{

    public partial class ReportBatchStatsScreen
    {

        #region BatchStatsReportType Enumumerator

        public enum BatchStatsReportType
        {
            Base = 1,
            Upgrade = 2
        }

        #endregion BatchStatsReportType Enumumerator

        #region Constants

        //private string _fontName = "Calibri";
        //private const int _fontSize = 10;
        //private const int _pointsToTwipsFactor = 20;
        //private const int _fontHeight = _fontSize * _pointsToTwipsFactor;

        #endregion

        #region Private Members

        //private CheckBox _xdgHeaderPrefixAreaCheckbox;
        //private RecordCollectionBase _campaigns;
        private long _campaignID;
        private DateTime _startDate;
        private DateTime _endDate;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        private BatchStatsReportType _batchStatsReportType;
        private bool _isUpgradeReport;

        #endregion Private Members

        #region Constructors

        public ReportBatchStatsScreen()
        {
            InitializeComponent();

            //LoadCampaignInfo();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

        private void LoadLookupValues()
        {
            try
            {
                SetCursor(Cursors.Wait);

                if (cmbBatchStatsReportType.Text.Trim() != "")
                {
                    switch (cmbBatchStatsReportType.Text)
                    {
                        case "Base, Rejuvenation, Defrost and Funeral":
                            _batchStatsReportType = BatchStatsReportType.Base;
                            CommonControlData.PopulateCampaignComboBox(cmbCampaign, false);
                            _isUpgradeReport = false;
                            break;

                        case "Upgrades":
                            _batchStatsReportType = BatchStatsReportType.Upgrade;
                            CommonControlData.PopulateCampaignComboBox(cmbCampaign, true);
                            _isUpgradeReport = true;
                            break;
                    }
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
        
        private bool IsAllInputParametersSpecifiedAndValid()
        {
            #region Ensuring that the batch and stats report type was selected

            if (cmbBatchStatsReportType.Text.Trim() == String.Empty)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'Batch and Stats Report Type'.", @"'Batch and Stats Report Type' not specified", ShowMessageType.Error);
                return false;
            }

            #endregion Ensuring that the batch and stats report type was selected

            #region Ensuring that a campaing was selected

            if (cmbCampaign.Text.Trim() == String.Empty)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please select a campaign.", @"No campaign select", ShowMessageType.Error);
                return false;
            }

            #endregion Ensuring that a campaing was selected

            #region Ensuring that the Start Date was specified

            if (calStartDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'Start Date'.", @"No 'Start Date' specified", ShowMessageType.Error);
                return false;
            }
            else
            {
                _startDate = calStartDate.SelectedDate.Value;
            }

            #endregion Ensuring that the Start Date was specified

            #region Ensuring that the End Date was specified

            if (calEndDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'End Date'.", @"No 'End Date' specified", ShowMessageType.Error);
                return false;
            }
            else
            {
                _endDate = calEndDate.SelectedDate.Value;
            }

            #endregion Ensuring that the End Date was specified

            #region Ensuring that the date range is valid

            if (_startDate > _endDate)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Invalid date range specified: The 'Start Date' can not be greater than the 'End Date'.", "Invalid date range", ShowMessageType.Error);
                return false;
            }

            #endregion Ensuring that the date range is valid

            // Otherwise if all is well, proceed:
            return true;
        }

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";

            //btnReport.IsEnabled = true;
            //btnClose.IsEnabled = true;
            //calStartDate.IsEnabled = true;

            EnableAllControls(true);
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup excel documents

                INCampaign campaign = new INCampaign(_campaignID);
                string strCampaignCode = campaign.Code;

                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                string filePathAndName = String.Format("{0}Batch and Stats Check Report ~ {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                Uri uri = new Uri("/Templates/ReportTemplateBatchStats.xlsx", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    wbTemplate = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }

                #region Determining which template sheets to use in report:

                Worksheet wsTemplateDaily = null;
                Worksheet wsTemplateMonthly = null;

                if (_isUpgradeReport)
                {
                    wsTemplateDaily = wbTemplate.Worksheets["Daily Campaign Totals Upgrades"];
                    wsTemplateMonthly = wbTemplate.Worksheets["Monthly Campaign Totals Upgrade"];
                }
                else
                {
                    wsTemplateDaily = wbTemplate.Worksheets["Daily Campaign Totals"];
                    wsTemplateMonthly = wbTemplate.Worksheets["Monthly Campaign Totals"];
                }

                #endregion Determining which template sheets to use in report:

                #endregion Setup excel documents

                #region Report Daily Sheets

                int days = (_endDate - _startDate).Days + 1;
                List<DateTime> range = Enumerable.Range(0, days).Select(i => _startDate.AddDays(i)).ToList();

                foreach (DateTime date in range)
                {
                    Worksheet wsReportDaily = null;
                    int rowIndex = 4;

                    DataTable dtBatchStatsDailyData = Insure.INReportBatchStatsDailyTSR(date, date, _campaignID);

                    if (dtBatchStatsDailyData.Rows.Count > 0)
                    {
                        string strDate = date.ToShortDateString().Replace('/','-');
                        wsReportDaily = wbReport.Worksheets.Add(strDate);

                        wsReportDaily.PrintOptions.PaperSize = PaperSize.A4;
                        wsReportDaily.PrintOptions.Orientation = Orientation.Portrait;

                        Methods.CopyExcelRegion(wsTemplateDaily, 0, 0, 1, 3, wsReportDaily, 0, 0);
                        Methods.CopyExcelRegion(wsTemplateDaily, 1, 0, 1, 3, wsReportDaily, 1, 0);
                        Methods.CopyExcelRegion(wsTemplateDaily, 2, 0, 1, 3, wsReportDaily, 2, 0);

                        WorksheetCell wsCell;

                        wsCell = wsReportDaily.Rows[0].Cells[0];
                        wsCell.Value = String.Format("Daily Totals ({1}) {0}", strDate, strCampaignCode);
                        wsCell.CellFormat.Alignment = HorizontalCellAlignment.Center;
                        
                        //string previousValue = string.Empty;

                        foreach (DataRow dr in dtBatchStatsDailyData.Rows)
                        {
                            Methods.CopyExcelRegion(wsTemplateDaily, 3, 0, 1, 3, wsReportDaily, rowIndex + 1, 0);

                            wsCell = wsReportDaily.Rows[rowIndex].Cells[0];
                            wsCell.Value = dr["TSR"];
                            wsCell.CellFormat.Alignment = HorizontalCellAlignment.Left;

                            wsCell = wsReportDaily.Rows[rowIndex].Cells[1];
                            wsCell.Value = dr["Total Policies"];
                      
                            wsCell.CellFormat.Alignment = HorizontalCellAlignment.Center;

                            if (_isUpgradeReport)
                            {
                                wsCell = wsReportDaily.Rows[rowIndex].Cells[2];
                                wsCell.Value = dr["Total Units"];
                                wsCell.CellFormat.Alignment = HorizontalCellAlignment.Right;
                                wsCell.CellFormat.FormatString = "#,##0.00";
                            }
                            else
                            {
                                wsCell = wsReportDaily.Rows[rowIndex].Cells[2];
                                wsCell.Value = dr["Total Premium"];
                                wsCell.CellFormat.Alignment = HorizontalCellAlignment.Right;
                                wsCell.CellFormat.FormatString = "#,##0.00";
                            }

                            rowIndex++;
                        }
                        string excelFormulaColumn = ExcelUtils.GetExcelColumnNameFromColumnIndex(2);
                        wsReportDaily.GetCell("B" + (rowIndex + 2)).ApplyFormula(String.Format("=SUM({0}{1}:{0}{2})", excelFormulaColumn, 5, rowIndex));
                        excelFormulaColumn = ExcelUtils.GetExcelColumnNameFromColumnIndex(3);
                        wsReportDaily.GetCell("C" + (rowIndex + 2)).ApplyFormula(String.Format("=SUM({0}{1}:{0}{2})", excelFormulaColumn, 5, rowIndex));
                    }
                }

                #endregion Report Daily Sheets

                #region Report Daily Sheets Summary

                {
                    Worksheet wsReportDailySummary = wbReport.Worksheets.Add("Summary");
                    wsReportDailySummary.MoveToIndex(0);

                    wsReportDailySummary.PrintOptions.PaperSize = PaperSize.A4;
                    wsReportDailySummary.PrintOptions.Orientation = Orientation.Portrait;

                    wbReport.WindowOptions.SelectedWorksheet = wsReportDailySummary;

                    //DataTable dtBatchStatsDailySummaryData = null;

                    ////Get the data for the Daily Campaign Totals worksheet
                    //SqlParameter[] parameters = new SqlParameter[3];
                    //parameters[0] = new SqlParameter("@StartDate", _startDate.ToShortDateString().Replace('/', '-'));
                    //parameters[1] = new SqlParameter("@EndDate", _endDate.ToShortDateString().Replace('/', '-'));
                    //parameters[2] = new SqlParameter("@CampaignID", _campaignID);
                    //DataSet dsBatchStatsDailySummaryData = Methods.ExecuteStoredProcedure("spINReportBatchStatsDailyTSR", parameters);

                    //if (dsBatchStatsDailySummaryData.Tables.Count > 0)
                    //{
                    //    dtBatchStatsDailySummaryData = dsBatchStatsDailySummaryData.Tables[0];
                    //}

                    DataTable dtBatchStatsDailySummaryData = Insure.INReportBatchStatsDailyTSR(_startDate, _endDate, _campaignID);

                    Methods.CopyExcelRegion(wsTemplateDaily, 0, 0, 1, 3, wsReportDailySummary, 0, 0);
                    Methods.CopyExcelRegion(wsTemplateDaily, 1, 0, 1, 3, wsReportDailySummary, 1, 0);
                    Methods.CopyExcelRegion(wsTemplateDaily, 2, 0, 1, 3, wsReportDailySummary, 2, 0);

                    WorksheetCell wsCell;

                    wsCell = wsReportDailySummary.Rows[0].Cells[0];
                    wsCell.Value = String.Format("Daily Totals ({2}) {0} to {1}", _startDate.ToShortDateString().Replace('/', '-'), _endDate.ToShortDateString().Replace('/', '-'), strCampaignCode);
                    wsCell.CellFormat.Alignment = HorizontalCellAlignment.Center;

                    {
                        int rowIndex = 4;

                        if (dtBatchStatsDailySummaryData != null)
                        {
                            foreach (DataRow dr in dtBatchStatsDailySummaryData.Rows)
                            {
                                Methods.CopyExcelRegion(wsTemplateDaily, 3, 0, 1, 3, wsReportDailySummary, rowIndex + 1, 0);

                                wsCell = wsReportDailySummary.Rows[rowIndex].Cells[0];
                                wsCell.Value = dr["TSR"];
                                wsCell.CellFormat.Alignment = HorizontalCellAlignment.Left;

                                wsCell = wsReportDailySummary.Rows[rowIndex].Cells[1];
                                wsCell.Value = dr["Total Policies"];
                                wsCell.CellFormat.Alignment = HorizontalCellAlignment.Center;

                                if (_isUpgradeReport)
                                {
                                    wsCell = wsReportDailySummary.Rows[rowIndex].Cells[2];
                                    wsCell.Value = dr["Total Units"];
                                    wsCell.CellFormat.Alignment = HorizontalCellAlignment.Right;
                                    wsCell.CellFormat.FormatString = "#,##0.00";
                                }
                                else
                                {
                                    wsCell = wsReportDailySummary.Rows[rowIndex].Cells[2];
                                    wsCell.Value = dr["Total Premium"];
                                    wsCell.CellFormat.Alignment = HorizontalCellAlignment.Right;
                                    wsCell.CellFormat.FormatString = "#,##0.00";
                                }

                                rowIndex++;
                            }
                        }

                        string excelFormulaColumn = ExcelUtils.GetExcelColumnNameFromColumnIndex(2);
                        wsReportDailySummary.GetCell("B" + (rowIndex + 2)).ApplyFormula(String.Format("=SUM({0}{1}:{0}{2})", excelFormulaColumn, 5, rowIndex));
                        excelFormulaColumn = ExcelUtils.GetExcelColumnNameFromColumnIndex(3);
                        wsReportDailySummary.GetCell("C" + (rowIndex + 2)).ApplyFormula(String.Format("=SUM({0}{1}:{0}{2})", excelFormulaColumn, 5, rowIndex));
                    }
                }

                #endregion Report Daily Sheets Summary

                #region Monthly Sales Data

                {
                    Worksheet wsReportMonthly = wbReport.Worksheets.Add("All Campaign Totals");
                    wsReportMonthly.MoveToIndex(1);
                    wsReportMonthly.PrintOptions.PaperSize = PaperSize.A4;
                    wsReportMonthly.PrintOptions.Orientation = Orientation.Portrait;

                    DataTable dtBatchStatsMonthlyData = Insure.INReportBatchStatsMonthlyCampaigns(_startDate, _endDate, _isUpgradeReport);

                    Methods.CopyExcelRegion(wsTemplateMonthly, 0, 0, 1, 3, wsReportMonthly, 0, 0);
                    Methods.CopyExcelRegion(wsTemplateMonthly, 1, 0, 1, 3, wsReportMonthly, 1, 0);
                    Methods.CopyExcelRegion(wsTemplateMonthly, 2, 0, 1, 3, wsReportMonthly, 2, 0);

                    WorksheetCell wsCell;

                    wsCell = wsReportMonthly.Rows[0].Cells[0];
                    wsCell.Value = String.Format("All Campaign Totals: {0} to {1}", _startDate.ToShortDateString().Replace('/', '-'), _endDate.ToShortDateString().Replace('/', '-'));
                    wsCell.CellFormat.Alignment = HorizontalCellAlignment.Center;

                    {
                        int rowIndex = 4;

                        if (dtBatchStatsMonthlyData != null)
                        {
                            foreach (DataRow dr in dtBatchStatsMonthlyData.Rows)
                            {
                                Methods.CopyExcelRegion(wsTemplateMonthly, 3, 0, 1, 3, wsReportMonthly, rowIndex + 1, 0);

                                wsCell = wsReportMonthly.Rows[rowIndex].Cells[0];
                                wsCell.Value = dr["Campaign"];
                                wsCell.CellFormat.Alignment = HorizontalCellAlignment.Left;

                                wsCell = wsReportMonthly.Rows[rowIndex].Cells[1];
                                wsCell.Value = dr["Total Policies"];
                                wsCell.CellFormat.Alignment = HorizontalCellAlignment.Center;

                                if (_isUpgradeReport)
                                {
                                    wsCell = wsReportMonthly.Rows[rowIndex].Cells[2];
                                    wsCell.Value = dr["Total Units"];
                                    wsCell.CellFormat.Alignment = HorizontalCellAlignment.Right;
                                    wsCell.CellFormat.FormatString = "#,##0.00";
                                }
                                else
                                {
                                    wsCell = wsReportMonthly.Rows[rowIndex].Cells[2];
                                    wsCell.Value = dr["Total Premium"];
                                    wsCell.CellFormat.Alignment = HorizontalCellAlignment.Right;
                                    wsCell.CellFormat.FormatString = "#,##0.00";
                                }
                                rowIndex++;
                            }
                        }

                        string excelFormulaColumn = ExcelUtils.GetExcelColumnNameFromColumnIndex(2);
                        wsReportMonthly.GetCell("B" + (rowIndex + 2)).ApplyFormula(String.Format("=SUM({0}{1}:{0}{2})", excelFormulaColumn, 5, rowIndex));
                        excelFormulaColumn = ExcelUtils.GetExcelColumnNameFromColumnIndex(3);
                        wsReportMonthly.GetCell("C" + (rowIndex + 2)).ApplyFormula(String.Format("=SUM({0}{1}:{0}{2})", excelFormulaColumn, 5, rowIndex));
                    }
                }

                #endregion Monthly Sales Data


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

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;
            btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnReport.ToolTip = btnReport.Content;
        }

        private void EnableAllControls(bool isEnabled)
        {
            //btnReport.IsEnabled = isEnabled;
            //btnClose.IsEnabled = isEnabled;
            //cmbCampaign.IsEnabled = isEnabled;
            //xdgBatches.IsEnabled = isEnabled;

            btnReport.IsEnabled = isEnabled;
            calStartDate.IsEnabled = isEnabled;
            cmbBatchStatsReportType.IsEnabled = isEnabled;
            cmbCampaign.IsEnabled = isEnabled;
            btnClose.IsEnabled = isEnabled;
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
        
        private void Cal1_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calStartDate.SelectedDate.ToString(), out _startDate);
            //EnableDisableReportButton();
        }

        private void Cal2_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calEndDate.SelectedDate.ToString(), out _endDate);
            //EnableDisableReportButton();
        }

        private void cmbCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var objectIdFromSelectedComboBoxItem = CommonControlData.GetObjectIDFromSelectedComboBoxItem(cmbCampaign);
            if (objectIdFromSelectedComboBoxItem != null)
            {
                _campaignID = objectIdFromSelectedComboBoxItem.Value;
            }
            //EnableDisableReportButton();
        }

        private void cmbBatchStatsReportType_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbBatchStatsReportType.SelectedValue != null)
            {
                LoadLookupValues();
            }
        }

        #endregion Event Handlers
    }

}
