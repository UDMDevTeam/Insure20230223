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
using UDM.WPF.Library;
using Orientation = Infragistics.Documents.Excel.Orientation;

namespace UDM.Insurance.Interface.Screens
{

    public partial class ReportPremiumBreakdownScreen : INotifyPropertyChanged
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

        #region Publicly Exposed Properties

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

        #endregion  Publicly Exposed Properties

        #region Private Members

        private DataRowView _selectedCampaign;
        private List<DataRecord> _selectedBatches;
        private DateTime _fromDate = DateTime.Now;
        private DateTime _toDate = DateTime.Now;
        private DataTable _dtAllBatches;

        private readonly DispatcherTimer _reportTimer = new DispatcherTimer();
        private int _seconds;

        private byte _staffType;

        #endregion Private Members

        #region Constructors

        public ReportPremiumBreakdownScreen()
        {
            InitializeComponent();
            LoadCampaignInfo();

            _reportTimer.Tick += ReportTimer;
            _reportTimer.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

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

        private void LoadCampaignInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);

                //DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign ORDER BY Code ASC");
                DataSet dsPremiumBreakdownScreenLookups = Business.Insure.INGetPremiumBreakdownReportScreenLookups();
                DataTable dtCampaigns = dsPremiumBreakdownScreenLookups.Tables[0];
                _dtAllBatches = dsPremiumBreakdownScreenLookups.Tables[1];
                DataTable dtStaffTypes = dsPremiumBreakdownScreenLookups.Tables[2];

                cmbCampaign.Populate(dtCampaigns, "CampaignName", "CampaignID");
                cmbStaffType.Populate(dtStaffTypes, "Description", "ID");
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
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Retrieve data from database

                string batchIDs = _selectedBatches.Aggregate(String.Empty, (current, record) => current + record.Cells["BatchID"].Value + ",");
                batchIDs = batchIDs.Substring(0, batchIDs.Length - 1);

                string batchCodes = _selectedBatches.Aggregate(String.Empty, (current, record) => current + record.Cells["UDMBatchCode"].Value + ",");
                batchCodes = batchCodes.Substring(0, batchCodes.Length - 1);

                DataSet ds = Business.Insure.INReportPremiumBreakdown(batchIDs, _fromDate, _toDate, _staffType); //Methods.ExecuteStoredProcedure("spINReportPremiumBreakdown", parameters);
                DataTable dtData = ds.Tables[0];
                DataTable dtTotals = ds.Tables[1];

                #endregion Retrieve data from database

                #region Setup excel report document

                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplatePremiumBreakdown.xlsx");
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                string filePathAndName = String.Format("{0}{1} Premium Breakdown Report ~ {2}.xlsx", GlobalSettings.UserFolder, _selectedCampaign.Row["CampaignCode"], DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));

                Worksheet wsTemplate = wbTemplate.Worksheets["Report"];
                Worksheet wsReport = wbReport.Worksheets.Add(_selectedCampaign.Row["CampaignCode"].ToString());

                wsReport.DisplayOptions.View = WorksheetView.PageLayout;
                wsReport.PrintOptions.PaperSize = PaperSize.A4;
                wsReport.PrintOptions.Orientation = Orientation.Portrait;

                Methods.CopyExcelRegion(wsTemplate, 0, 0, 8, 5, wsReport, 0, 0);

                #endregion Setup excel report document

                #region Header data

                {
                    wsReport.GetCell("StartDate").Value = _fromDate.ToString("d");
                    wsReport.GetCell("EndDate").Value = _toDate.ToString("d");
                    wsReport.GetCell("Campaign").Value = _selectedCampaign.Row["CampaignCode"];
                    wsReport.GetCell("Batches").Value = batchCodes + ((char)65279);
                    wsReport.GetCell("TotalSales").Value = dtTotals.Rows[0]["TotalSales"].ToString() + ((char)65279);
                }

                #endregion Header data

                #region report data

                {
                    int rowIndex = 9;
                    foreach (DataRow dr in dtData.Rows)
                    {
                        Methods.CopyExcelRegion(wsTemplate, 9, 0, 1, 5, wsReport, rowIndex, 0);

                        wsReport.GetCell("OptionCode").Value = dr["OptionCode"].ToString();
                        wsReport.GetCell("CoverAmountLA1").Value = dr["LA1Cover"] as decimal?;
                        wsReport.GetCell("CoverAmountChild").Value = dr["ChildCover"] as decimal?;
                        wsReport.GetCell("Premium").Value = dr["TotalPremium"] as decimal?;
                        wsReport.GetCell("Sales").Value = dr["Sales"] as int?;
                        wsReport.GetCell("SalesPercentage").Value = dr["Sales%"] as double?;

                        wsReport.Workbook.NamedReferences.Clear();
                        rowIndex++;
                    }
                }

                #endregion

                #region save and display excel document

                //Save excel document
                wbReport.SetCurrentFormat(WorkbookFormat.Excel2007);
                wbReport.Save(filePathAndName);

                //Display excel document
                Process.Start(filePathAndName);

                #endregion
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

        private void ReportTimer(object sender, EventArgs e)
        {
            _seconds++;
            btnReport.Content = TimeSpan.FromSeconds(_seconds).ToString();
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
                IsReportRunning = true;

                _selectedCampaign = cmbCampaign.SelectedItem as DataRowView;
                _selectedBatches = (xdgBatches.Records.Select(r => (DataRecord)r).Where(r => (bool)r.Cells["Select"].Value)).ToList();

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
            try
            {
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

                if (cmbCampaign.SelectedItem != null)
                {
                    DataRow drCampaign = (cmbCampaign.SelectedItem as DataRowView).Row;
                    string filterString = drCampaign["BatchFilterString"].ToString();

                    var filteredBatches = _dtAllBatches.Select(filterString).AsEnumerable();
                    if (filteredBatches.Any())
                    {
                        string orderByString = drCampaign["BatchOrderByString"].ToString();
                        DataTable dtSelectedCampaignBatches = _dtAllBatches.Select(filterString, orderByString).CopyToDataTable();
                        xdgBatches.DataSource = dtSelectedCampaignBatches.DefaultView;
                    }
                    else
                    {
                        xdgBatches.DataSource = null;
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

        #endregion

        private void cmbStaffType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _staffType = Convert.ToByte(cmbStaffType.SelectedValue);
        }
    }
}
