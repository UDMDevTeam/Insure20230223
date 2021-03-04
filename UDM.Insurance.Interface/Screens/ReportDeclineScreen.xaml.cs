using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Resources;
using System.Windows.Threading;
using System.Threading;
using UDM.WPF.Library;
using Orientation = Infragistics.Documents.Excel.Orientation;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;
using Embriant.Framework;

namespace UDM.Insurance.Interface.Screens
{

    public partial class ReportDeclineScreen : INotifyPropertyChanged
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

        #endregion



        #region Properties

        private bool? _IsAllRecordsSelected = false;
        public bool? IsAllRecordsSelected
        {
            get
            {
                return _IsAllRecordsSelected;
            }
            set
            {
                _IsAllRecordsSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAllRecordsSelected"));
            }
        }

        private bool? _IsReportRunning = false;
        public bool? IsReportRunning
        {
            get
            {
                return _IsReportRunning;
            }
            set
            {
                _IsReportRunning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReportRunning"));
            }
        }

        #endregion



        #region Constants

        //private string _fontName = "Calibri";
        //private const int _fontSize = 10;
        //private const int _pointsToTwipsFactor = 20;
        //private const int _fontHeight = _fontSize * _pointsToTwipsFactor; 

        #endregion



        #region Private Members

        private List<Record> _lstSelectedBatches;
        private DataRowView _selectedCampaign;
        private DataRow _drCampaign;
        private List<DataRecord> _selectedBatches;
        private DateTime _fromDate = DateTime.Now;
        private DateTime _toDate = DateTime.Now;
        private string _campaignName = String.Empty;
        string _fkINBatchIDs = String.Empty;
        private long _fkINCampaignID;
        DataTable _dtAllBatches;
        private readonly DispatcherTimer _reportTimer = new DispatcherTimer();
        private int _seconds;

        #endregion



        #region Constructors

        public ReportDeclineScreen()
        {
            InitializeComponent();
            LoadLookups();

            _reportTimer.Tick += ReportTimer;
            _reportTimer.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion



        #region Private Methods

        //This method is derived from the LoadLookups method in ReportPODDiariesScreen //Author Nicolas Stephenson
        private void LoadLookups()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataSet dsLookups = Insure.INGetDeclineScreenLookups();
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

        private bool? AllRecordsSelected()
        {
            try
            {
                bool allSelected = (xdgBatches.Records.Select(r => (bool)((DataRecord)r).Cells["Select"].Value)).All(b => b);
                bool noneSelected = (xdgBatches.Records.Select(r => (bool)((DataRecord)r).Cells["Select"].Value)).All(b => !b);

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

        private void LoadCampaignInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign ORDER BY Code ASC");
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

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _reportTimer.Stop();
            _seconds = 0;
            IsReportRunning = false;
            btnReport.Content = "Report";

            EnableAllControls(true);
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup excel documents

                string filePathAndName = String.Format("{0}Decline Report ({1}) {2}.xlsx", GlobalSettings.UserFolder, _campaignName, DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));
                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateDeclines.xlsx");
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup excel documents

                #region Get the data

                DataSet dsDeclineReportData = Insure.INReportDeclines(_fkINCampaignID, _fkINBatchIDs, _fromDate, _toDate);

                if (dsDeclineReportData.Tables[1].Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                #endregion Get the data

                foreach (DataRow row in dsDeclineReportData.Tables[0].Rows)
                {
                    InsertIndividualDeclineReportSheet(wbTemplate, wbReport, dsDeclineReportData, row);
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

        private void InsertIndividualDeclineReportSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsDeclineReportData, DataRow drCurrentSalesConsultant)
        {
            string filterString = drCurrentSalesConsultant["FilterString"].ToString();

            var filteredRows = dsDeclineReportData.Tables[1].Select(filterString).AsEnumerable();
            if (filteredRows.Any())
            {

                #region Partition the given dataset

                string orderByString = drCurrentSalesConsultant["OrderByString"].ToString();

                DataTable dtCurrentUserActivityData = dsDeclineReportData.Tables[1].Select(filterString, orderByString).CopyToDataTable();
                DataTable dtExcelSheetDataTableColumnMappings = dsDeclineReportData.Tables[2];
                //DataTable dtExcelSheetTotalsAndAverageColumnMappings = dsUpgradeSTLReportDate.Tables[3];

                #endregion Partition the given dataset

                #region Declarations & Initializations

                int reportRow = 6;
                //int formulaStartRow = reportRow;

                byte templateDataSheetRowSpan = 5;
                byte templateColumnSpan = 10;
                byte templateRowIndex = 6;
                //byte totalsTemplateRowIndex = 14;

                string salesConsultantName = drCurrentSalesConsultant["Batch"].ToString();
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

        private void ReportTimer(object sender, EventArgs e)
        {
            _seconds++;
            btnReport.Content = TimeSpan.FromSeconds(_seconds).ToString();
            btnReport.ToolTip = btnReport.Content;
        }

        #endregion



        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    IsReportRunning = true;

            //    _selectedCampaign = cmbCampaign.SelectedItem as DataRowView;
            //    _selectedBatches = (xdgBatches.Records.Select(r => (DataRecord)r).Where(r => (bool)r.Cells["Select"].Value)).ToList();

            //    BackgroundWorker worker = new BackgroundWorker();
            //    worker.DoWork += Report;
            //    worker.RunWorkerCompleted += ReportCompleted;
            //    worker.RunWorkerAsync();

            //    _reportTimer.Start();
            //}

            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

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
                _reportTimer.Start();
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
                if (xdgBatches.DataSource != null)
                {
                    DataTable dt = ((DataView)xdgBatches.DataSource).Table;

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["Select"] = true;
                    }
                }
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
                if (xdgBatches.DataSource != null)
                {
                    DataTable dt = ((DataView)xdgBatches.DataSource).Table;

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["Select"] = false;
                    }
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            IsAllRecordsSelected = AllRecordsSelected();
        }

        private void HeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                #region Add data trigger for report button style

                {
                    Style style = new Style();

                    style.TargetType = typeof (Button);
                    style.BasedOn = (Style) FindResource("ReportButton");

                    DataTrigger trigger = new DataTrigger();
                    trigger.Value = "False";
                    trigger.Binding = new Binding {Source = sender, Path = new PropertyPath("IsChecked")};
                    Setter setter = new Setter();
                    setter.Property = IsEnabledProperty;
                    setter.Value = false;
                    trigger.Setters.Add(setter);

                    style.Triggers.Add(trigger);

                    btnReport.Style = style;
                }
                
                #endregion

                #region Bind header checkbox ischecked property to IsAllRecordsSelected

                {
                    Binding binding = new Binding { Source = this, Path = new PropertyPath("IsAllRecordsSelected") };
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    binding.Mode = BindingMode.TwoWay;
                    ((CheckBox)sender).SetBinding(ToggleButton.IsCheckedProperty, binding);
                }

                #endregion

            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void Cal1_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(Cal1.SelectedDate.ToString(), out _fromDate);
        }

        private void Cal2_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(Cal2.SelectedDate.ToString(), out _toDate);
        }

        private void EmbriantComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Methods.EmbriantComboBoxPreviewKeyDown(sender, e);
        }

        private void cmbCampaign_Loaded(object sender, RoutedEventArgs e)
        {
            cmbCampaign.Focus();
        }

        private void cmbCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //try
            //{
            //    if (cmbCampaign.SelectedIndex > -1)
            //    {
            //        SetCursor(Cursors.Wait);

            //        DataTable dt = Methods.GetTableData("SELECT ID [BatchID], UDMCode [UDMBatchCode] FROM INBatch WHERE FKINCampaignID = '" + cmbCampaign.SelectedValue + "'");
            //        DataColumn column = new DataColumn("Select", typeof(bool));
            //        column.DefaultValue = false;
            //        dt.Columns.Add(column);
            //        dt.DefaultView.Sort = "UDMBatchCode ASC";

            //        xdgBatches.DataSource = dt.DefaultView;
            //    }
            //    else
            //    {
            //        xdgBatches.DataSource = null;
            //    }

            //    IsAllRecordsSelected = false;
            //}

            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            //finally
            //{
            //    SetCursor(Cursors.Arrow);
            //}

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

                    DataView dv = dtSelectedCampaignBatches.DefaultView;
                    dv.Sort = "ID desc";
                    xdgBatches.DataSource = dv;
                }
                else
                {
                    xdgBatches.DataSource = null;
                }


            }
        }

        #endregion

    }
}
