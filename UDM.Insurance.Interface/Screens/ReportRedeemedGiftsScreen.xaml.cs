using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using System.Linq;
using System.Threading;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportRedeemedGiftsScreen
    {

        #region Constants


        #endregion Constants

        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        private DateTime _date = DateTime.Now;
        private List<Record> _lstSelectedFKINCampaignIDs;
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;
        private string _campaignIDs = String.Empty;

        #endregion Private Members

        #region Constructors

        public ReportRedeemedGiftsScreen()
        {
            InitializeComponent();
            LoadCampaignInfo();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion

        #region Private Methods

        private bool? AllRecordsSelected()
        {
            try
            {
                bool allSelected = true;
                bool noneSelected = true;

                if (xdgCampaigns.DataSource != null)
                {
                    foreach (DataRow dr in ((DataView)xdgCampaigns.DataSource).Table.Rows)
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

        private void LoadCampaignInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);

                //DataTable dt = Methods.GetTableData("SELECT [ID] AS [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign");

                //DataColumn column = new DataColumn("Select", typeof(bool));
                //column.DefaultValue = false;
                //dt.Columns.Add(column);
                //dt.DefaultView.Sort = "CampaignName ASC";

                DataTable dt = Business.Insure.INGetRedeemedGiftsExportLookups();
                xdgCampaigns.DataSource = dt.DefaultView;
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
                if ((Cal1.SelectedDate != null)) // && (Cal2.SelectedDate != null) && (Cal2.SelectedDate >= Cal1.SelectedDate)
                {
                    if (_xdgHeaderPrefixAreaCheckbox != null && (_xdgHeaderPrefixAreaCheckbox.IsChecked == true || _xdgHeaderPrefixAreaCheckbox.IsChecked == null))
                    {
                        btnExport.IsEnabled = true;
                        return;
                    }
                }

                btnExport.IsEnabled = false;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void EnableAllControls(bool isEnabled)
        {
            btnExport.IsEnabled = isEnabled;
            btnClose.IsEnabled = isEnabled;
            xdgCampaigns.IsEnabled = isEnabled;
            Cal1.IsEnabled = isEnabled;
            //medPlatinumBatchCode.IsEnabled = isEnabled;
        }

        private bool IsAllInputsValidAndComplete()
        {
            #region Verifying that at least 1 campaign was selected

            var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            _lstSelectedFKINCampaignIDs = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

            if (_lstSelectedFKINCampaignIDs.Count == 0)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No batches selected", ShowMessageType.Error);
                return false;
            }
            else
            {
                _campaignIDs = _lstSelectedFKINCampaignIDs.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                _campaignIDs = _campaignIDs.Substring(0, _campaignIDs.Length - 1);
            }

            #endregion Verifying that at least 1 campaign was selected

            #region Checking the selected date

            if (Cal1.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please specify a date.", "No date selected", ShowMessageType.Error);
                return false;
            }
            else
            {
                _date = Cal1.SelectedDate.Value;
            }

            #endregion Checking the selected date

            #region Validating the Platinum Batch Code field

            //if (medPlatinumBatchCode.Text.Trim() != String.Empty)
            //{
            //    if ((medPlatinumBatchCode.Text.Trim().Contains(@"\")) ||
            //        (medPlatinumBatchCode.Text.Trim().Contains(@"/")) ||
            //        (medPlatinumBatchCode.Text.Trim().Contains(@":")) ||
            //        (medPlatinumBatchCode.Text.Trim().Contains(@"*")) ||
            //        (medPlatinumBatchCode.Text.Trim().Contains(@"?")) ||
            //        (medPlatinumBatchCode.Text.Trim().Contains("\"")) ||
            //        (medPlatinumBatchCode.Text.Trim().Contains(@"<")) ||
            //        (medPlatinumBatchCode.Text.Trim().Contains(@">")) ||
            //        (medPlatinumBatchCode.Text.Trim().Contains(@"|")))
            //    {
            //        ShowMessageBox(new INMessageBoxWindow1(), @"The Platinum Life Batch Code field may not contain any of the following characters: \ / : * ? "" < > |", "Invalid Platinum Life Batch Code", ShowMessageType.Error);
            //        return false;
            //    }
            //    else
            //    {
            //        _platinumBatchCode = medPlatinumBatchCode.Text.Trim();
            //    }
            //}
            //else
            //{
            //    ShowMessageBox(new INMessageBoxWindow1(), "Please specify the Platinum Life Batch Code.", "No Platinum Life Batch Code", ShowMessageType.Error);
            //    return false;
            //}

            #endregion Validating the Platinum Batch Code field

            return true;
        }

        private void RedeemedGiftsExportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnExport.Content = "Export";

            EnableAllControls(true);
        }

        private void RedeemedGiftsExport(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup excel documents

                string filePathAndName = String.Format("{0}Redeemed Gifts Export Sheet ~ {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));

                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateRedeemedGifts.xlsx");
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup excel documents

                #region Get the data

                DataSet dsRedeemedGiftsExportData = Business.Insure.INGetRedeemedGiftsExportData(_campaignIDs, _date);

                #endregion Get the data

                #region For each of the campaigns in the first data table, add a worksheet to the workbook - if there is any data

                foreach (DataRow row in dsRedeemedGiftsExportData.Tables[0].Rows)
                {
                    InsertRedeemedGiftsExportDataSheet(wbTemplate, wbReport, dsRedeemedGiftsExportData, row);
                }

                #endregion For each of the campaigns in the first data table, add a worksheet to the workbook - if there is any data

                #region Save the workbook - if it has at aleast 1 worksheet

                if (wbReport.Worksheets.Count > 0)
                {
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
                        System.Diagnostics.Process.Start(filePathAndName);
                    }
                }

                #endregion Save the workbook - if it has at aleast 1 worksheet
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

        private void InsertRedeemedGiftsExportDataSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsRedeemedGiftsExportData, DataRow drCurrentCampaign)
        {
            string filterString = drCurrentCampaign["FilterString"].ToString();

            var filteredRows = dsRedeemedGiftsExportData.Tables[1].Select(filterString).AsEnumerable();
            if (filteredRows.Any())
            {

                #region Partition the given dataset

                string orderByString = drCurrentCampaign["OrderByString"].ToString();
                DataTable dtCurrentCampaignData = dsRedeemedGiftsExportData.Tables[1].Select(filterString, orderByString).CopyToDataTable();
                DataTable dtExcelSheetDataTableColumnMappings = dsRedeemedGiftsExportData.Tables[2];

                #endregion Partition the given dataset

                #region Declarations & Initializations

                int reportRow = 1;

                byte templateDataSheetRowSpan = 0;
                byte templateColumnSpan = 10;
                byte templateRowIndex = 1;

                //string reportTitle = drCurrentCampaign["ReportTitle"].ToString();
                string worksheetTabName = drCurrentCampaign["WorksheetTabName"].ToString();
                string campaignDataSheetTemplateName = "Export"; //selectedCampaign.Cells["CampaignCategory"].Value.ToString();

                //string reportHeadingCell = "A1";        //dtReportConfigs.Rows[0]["ReportHeadingCell"].ToString();
                //string reportSubHeadingCell = "A3";     //dtReportConfigs.Rows[0]["ReportSubHeadingCell"].ToString();
                //string reportDateCell = "G5";           //dtReportConfigs.Rows[0]["ReportDateCell"].ToString();

                #endregion Declarations & Initializations

                #region Add the worksheet

                Worksheet wsReportTemplate = wbTemplate.Worksheets[campaignDataSheetTemplateName];
                Worksheet wsReport = wbReport.Worksheets.Add(worksheetTabName);
                Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                #endregion Add the worksheet

                #region Inserting the column headings

                Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);

                #endregion Inserting the column headings

                #region Add the data

                reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentCampaignData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                #endregion Add the data
            }
        }

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;
            btnExport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnExport.ToolTip = btnExport.Content;
        }

        #endregion

        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsAllInputsValidAndComplete())
                {
                    EnableAllControls(false);

                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += RedeemedGiftsExport;
                    worker.RunWorkerCompleted += RedeemedGiftsExportCompleted;
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
                DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;

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
                DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;

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
            //DateTime.TryParse(Cal1.SelectedDate.ToString(), out _fromDate);
            EnableDisableExportButton();
        }

        #endregion Event Handlers

    }
}
