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
using System.Linq;
using System.Collections.Generic;
using UDM.Insurance.Business;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportPODDiariesScreen
    {

        #region Constants

        //private string _fontName = "Calibri";
        //private const int _fontSize = 10;
        //private const int _pointsToTwipsFactor = 20;
        //private const int _fontHeight = _fontSize * _pointsToTwipsFactor;

        #endregion

        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        private DataRow _drCampaign;
        private long _fkINCampaignID;
        private string _campaignName = String.Empty;
        private RecordCollectionBase _batches;
        private List<Record> _lstSelectedBatches;
        //private bool? _reportContentOnSinglePageSheet;

        string _fkINBatchIDs = String.Empty;
        DataTable _dtAllBatches;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        #endregion Private Members

        #region Constructors

        public ReportPODDiariesScreen()
        {
            InitializeComponent();
            LoadLookups();

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

        private void LoadLookups()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataSet dsLookups = Insure.INGetPODDiariesScreenLookups();
                DataTable dtCampaigns = dsLookups.Tables[0];
                _dtAllBatches = dsLookups.Tables[1];

                cmbCampaign.Populate(dtCampaigns, "CampaignName", "ID");
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

                #region Setup excel documents

                string filePathAndName = String.Format("{0}POD Diaries Report ({1}) {2}.xlsx", GlobalSettings.UserFolder, _campaignName, DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));
                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplatePODDiaries.xlsx");
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup excel documents

                #region Get the data

                DataSet dsPODDiariesReportData = Insure.INReportPODDiaries(_fkINCampaignID, _fkINBatchIDs);

                if (dsPODDiariesReportData.Tables[1].Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                #endregion Get the data

                foreach (DataRow row in dsPODDiariesReportData.Tables[0].Rows)
                {
                    InsertIndividualPODDiariesReportSheet(wbTemplate, wbReport, dsPODDiariesReportData, row);
                }

                #region Save & Display the resulting workbook - if there is at least 1 worksheet

                if (wbReport.Worksheets.Count < 1)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", ShowMessageType.Information);
                    });
                }
                else
                {
                    //Save excel document
                    wbReport.Save(filePathAndName);

                    //Display excel document
                    Process.Start(filePathAndName);
                }

                #endregion Save & Display the resulting workbook - if there is at least 1 worksheet

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

        private void InsertIndividualPODDiariesReportSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsPODDiariesReportData, DataRow drCurrentSalesConsultant)
        {
            string filterString = drCurrentSalesConsultant["FilterString"].ToString();

            var filteredRows = dsPODDiariesReportData.Tables[1].Select(filterString).AsEnumerable();
            if (filteredRows.Any())
            {

                #region Partition the given dataset

                string orderByString = drCurrentSalesConsultant["OrderByString"].ToString();
                DataTable dtCurrentUserActivityData = dsPODDiariesReportData.Tables[1].Select(filterString, orderByString).CopyToDataTable();
                DataTable dtExcelSheetDataTableColumnMappings = dsPODDiariesReportData.Tables[2];
                //DataTable dtExcelSheetTotalsAndAverageColumnMappings = dsUpgradeSTLReportDate.Tables[3];

                #endregion Partition the given dataset

                #region Declarations & Initializations

                int reportRow = 6;
                //int formulaStartRow = reportRow;

                byte templateDataSheetRowSpan = 5;
                byte templateColumnSpan = 11;
                byte templateRowIndex = 6;
                //byte totalsTemplateRowIndex = 14;

                string salesConsultantName = drCurrentSalesConsultant["SalesConsultantName"].ToString();
                string worksheetTabName = Methods.ParseWorksheetName(wbReport, salesConsultantName); //drCurrentSalesConsultant["WorksheetTabName"].ToString();
                string campaignDataSheetTemplateName = "Report"; //selectedCampaign.Cells["CampaignCategory"].Value.ToString();

                string reportHeadingCell = "A1";        //dtReportConfigs.Rows[0]["ReportHeadingCell"].ToString();
                string reportSubHeadingCell = "A3";     //dtReportConfigs.Rows[0]["ReportSubHeadingCell"].ToString();
                string reportDateCell = "B4";           //dtReportConfigs.Rows[0]["ReportDateCell"].ToString();

                string reportTitle = drCurrentSalesConsultant["ReportTitle"].ToString();
                string reportSubTitle = drCurrentSalesConsultant["ReportSubTitle"].ToString();

                #endregion Declarations & Initializations

                #region Add the worksheet

                Worksheet wsReportTemplate = wbTemplate.Worksheets[campaignDataSheetTemplateName];
                Worksheet wsReport = wbReport.Worksheets.Add(worksheetTabName);
                Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                #endregion Add the worksheet

                #region Populating the report details

                Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
                wsReport.GetCell(reportHeadingCell).Value = reportTitle;
                wsReport.GetCell(reportSubHeadingCell).Value = reportSubTitle;
                wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                #endregion Populating the report details

                #region Add the data

                reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentUserActivityData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                #endregion Add the data

                //#region Add the totals / averages

                //reportRow = Methods.MapTemplatizedExcelFormulas(wsReportTemplate, dtExcelSheetTotalsAndAverageColumnMappings, totalsTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0, formulaStartRow, reportRow - 1);

                //#endregion Add the totals / averages
            }
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
                long campaignID = Convert.ToInt32(_drCampaign.ItemArray[0]);
                string campaignName = _drCampaign.ItemArray[1].ToString();
                string campaignDescription = String.Format("{0} ({1})", _drCampaign.ItemArray[1], _drCampaign.ItemArray[2]);
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

                rowIndex++ ;
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
            #region Ensuring that at a campaign was selected

            if (cmbCampaign.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select a campaign.", "No campaign selected", ShowMessageType.Error);
                EnableAllControls(true);
                return false;
            }
            else
            {
                _fkINCampaignID = Convert.ToInt64(cmbCampaign.SelectedValue);
            }

            #endregion Ensuring that at a campaign was selected

            #region Ensuring that at least 1 batch was selected

            var lstTemp = (from r in xdgBatches.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            _lstSelectedBatches = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Batch"].Value));

            if (_lstSelectedBatches.Count == 0)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 batch from the list.", "No batches selected", ShowMessageType.Error);
                return false;
            }
            else
            {
                _fkINBatchIDs = _lstSelectedBatches.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                _fkINBatchIDs = _fkINBatchIDs.Substring(0, _fkINBatchIDs.Length - 1);
            }

            #endregion Ensuring that at least 1 batch was selected

            // Otherwise if all is well, proceed:
            return true;

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

                EnableAllControls(false);

                if (!HasAllInputParametersBeenSpecified())
                {
                    EnableAllControls(true);
                    return;
                }

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
                //DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;
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

		//private void Cal1_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
		//{
		//	DateTime.TryParse(Cal1.SelectedDate.ToString(), out _fromDate);
		//	EnableDisableExportButton();
		//}

        //private void Cal2_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        //{
        //    DateTime.TryParse(Cal2.SelectedDate.ToString(), out _toDate);
        //    EnableDisableExportButton();
        //}

        private void cmbCampaign_Loaded(object sender, RoutedEventArgs e)
        {
            cmbCampaign.Focus();
        }

        private void cmbCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //_campaignID = Convert.ToInt64(cmbCampaign.SelectedValue);
            //_campaign = (cmbCampaign.SelectedItem as DataRowView).Row;


            if (cmbCampaign.SelectedItem != null)
            {
                _drCampaign = (cmbCampaign.SelectedItem as DataRowView).Row;
                _campaignName = _drCampaign["CampaignName"].ToString();

                string filterString = _drCampaign["BatchFilterString"].ToString();

                var filteredBatches = _dtAllBatches.Select(filterString).AsEnumerable();
                if (filteredBatches.Any())
                {
                    string orderByString = _drCampaign["BatchOrderByString"].ToString();
                    DataTable dtSelectedCampaignBatches = _dtAllBatches.Select(filterString, orderByString).CopyToDataTable();
                    xdgBatches.DataSource = dtSelectedCampaignBatches.DefaultView;
                }
                else
                {
                    xdgBatches.DataSource = null;
                }
            }
        }

        #endregion Event Handlers

    }

}
