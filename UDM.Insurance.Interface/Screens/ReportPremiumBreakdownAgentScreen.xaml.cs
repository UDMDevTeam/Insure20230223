using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Resources;
using Embriant.Framework.Configuration;
using Embriant.WPF.Controls;
using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.Editors.Events;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportPremiumBreakdownAgentScreen : INotifyPropertyChanged
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



        #endregion



        #region Members

        private DataRowView _selectedCampaign;
        private List<DataRecord> _selectedAgents;
        private DateTime _fromDate = DateTime.Now;
        private DateTime _toDate = DateTime.Now;

        private readonly DispatcherTimer _reportTimer = new DispatcherTimer();
        private int _seconds;

        #endregion



        #region Constructors

        public ReportPremiumBreakdownAgentScreen()
        {
            InitializeComponent();

            LoadCampaignInfo();

            _reportTimer.Tick += ReportTimer;
            _reportTimer.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion



        #region Methods

        private void LoadCampaignInfo()
        {
            try
            {
                Cursor = Cursors.Wait;

                DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign ORDER BY Code ASC");
                cmbCampaign.Populate(dt, "CampaignName", "CampaignID");
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }

            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private bool? AllRecordsSelected()
        {
            try
            {
                bool allSelected = (xdgAgents.Records.Select(r => (bool)((DataRecord)r).Cells["Select"].Value)).All(b => b);
                bool noneSelected = (xdgAgents.Records.Select(r => (bool)((DataRecord)r).Cells["Select"].Value)).All(b => !b);

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
                (new BaseControl()).HandleException(ex);
                return null;
            }
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                Dispatcher.Invoke((Action)delegate
                {
                    Cursor = Cursors.Wait;
                });

                long campaignID = Convert.ToInt32(_selectedCampaign.Row["CampaignID"]);
                string campaignCode = _selectedCampaign.Row["CampaignCode"] as string;
                string campaignName = _selectedCampaign.Row["CampaignName"] as string;

                #region setup excel report document

                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                string filePathAndName = GlobalSettings.UserFolder + "Premium Breakdown Report (" + campaignCode + ") (Agents) ~ " + DateTime.Now.Millisecond + ".xlsx";

                Uri uri = new Uri("/Templates/ReportTemplatePremiumBreakdownAgent.xlsx", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    wbTemplate = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }

                Worksheet wsTemplate = wbTemplate.Worksheets["Report"];

                #endregion

                foreach(DataRecord drAgent in _selectedAgents)
                {
                    long agentID = Convert.ToInt32(drAgent.Cells["AgentID"].Value);
                    string agentName = new string((drAgent.Cells["AgentName"].Value as string + string.Empty).Take(31).ToArray());

                    #region retrieve data from database

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@CampaignID", campaignID),
                        new SqlParameter("@AgentID", agentID),
                        new SqlParameter("@FromDate", _fromDate),
                        new SqlParameter("@ToDate", _toDate)
                    };

                    DataSet ds = Methods.ExecuteStoredProcedure("spINReportPremiumBreakdownAgent", parameters);
                    DataTable dtData = ds.Tables[0];
                    DataTable dtTotals = ds.Tables[1];

                    #endregion

                    #region setup worksheet

                    Worksheet wsReport = wbReport.Worksheets.Add(agentName);

                    wsReport.DisplayOptions.View = WorksheetView.PageLayout;
                    wsReport.PrintOptions.PaperSize = PaperSize.A4;
                    wsReport.PrintOptions.Orientation = Infragistics.Documents.Excel.Orientation.Portrait;

                    Methods.CopyExcelRegion(wsTemplate, 0, 0, 8, 5, wsReport, 0, 0);

                    #endregion

                    #region header data

                    {
                        wsReport.GetCell("StartDate").Value = _fromDate.ToString("d");
                        wsReport.GetCell("EndDate").Value = _toDate.ToString("d");
                        wsReport.GetCell("Campaign").Value = campaignName;
                        wsReport.GetCell("Agent").Value = agentName; // + ((char)65279)
                        wsReport.GetCell("TotalSales").Value = dtTotals.Rows[0]["TotalSales"].ToString() + ((char)65279);
                    }

                    #endregion

                    #region report data

                    {
                        int rowIndex = 9;
                        foreach (DataRow drData in dtData.Rows)
                        {
                            Methods.CopyExcelRegion(wsTemplate, 9, 0, 1, 5, wsReport, rowIndex, 0);

                            wsReport.GetCell("OptionCode").Value = drData["OptionCode"].ToString();
                            wsReport.GetCell("CoverAmountLA1").Value = drData["LA1Cover"] as decimal?;
                            wsReport.GetCell("CoverAmountChild").Value = drData["ChildCover"] as decimal?;
                            wsReport.GetCell("Premium").Value = drData["Premium"] as decimal?;
                            wsReport.GetCell("Sales").Value = drData["Sales"] as int?;
                            wsReport.GetCell("SalesPercentage").Value = drData["Sales%"] as double?;

                            wsReport.Workbook.NamedReferences.Clear();
                            rowIndex++;
                        }
                    }

                    #endregion

                    wsReport.Workbook.NamedReferences.Clear();
                }

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
                Dispatcher.Invoke((Action)delegate
                {
                    (new BaseControl()).HandleException(ex);
                });
            }

            finally
            {
                Dispatcher.Invoke((Action) delegate
                {
                    Cursor = Cursors.Arrow;
                });
            }
        }

        private void ReportTimer(object sender, EventArgs e)
        {
            _seconds++;
            btnReport.Content = TimeSpan.FromSeconds(_seconds).ToString();
            btnReport.ToolTip = btnReport.Content;
        }

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _reportTimer.Stop();
            _seconds = 0;
            IsReportRunning = false;
            btnReport.Content = "Report";
        }

        #endregion



        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsReportRunning = true;

                _selectedCampaign = cmbCampaign.SelectedItem as DataRowView;
                _selectedAgents = (xdgAgents.Records.Select(r => (DataRecord)r).Where(r => (bool)r.Cells["Select"].Value)).ToList();

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += Report;
                worker.RunWorkerCompleted += ReportCompleted;
                worker.RunWorkerAsync();

                _reportTimer.Start();
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
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
                if (cmbCampaign.SelectedIndex > -1)
                {
                    Cursor = Cursors.Wait;

                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@CampaignID", cmbCampaign.SelectedValue)
                    };
                    DataSet ds = Methods.ExecuteStoredProcedure("spGetSalesAgentsForCampaign", parameters);

                    DataTable dt = ds.Tables[0];
                    DataColumn column = new DataColumn("Select", typeof(bool));
                    column.DefaultValue = false;
                    dt.Columns.Add(column);

                    xdgAgents.DataSource = dt.DefaultView;
                }
                else
                {
                    xdgAgents.DataSource = null;
                }

                IsAllRecordsSelected = false;
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }

            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void HeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                #region Add data trigger for report button style

                {
                    Style style = new Style();

                    style.TargetType = typeof(Button);
                    style.BasedOn = (Style)FindResource("ReportButton");

                    DataTrigger trigger = new DataTrigger();
                    trigger.Value = "False";
                    trigger.Binding = new Binding { Source = sender, Path = new PropertyPath("IsChecked") };
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
                (new BaseControl()).HandleException(ex);
            }
        }

        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (xdgAgents.DataSource != null)
                {
                    DataTable dt = ((DataView)xdgAgents.DataSource).Table;

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["Select"] = true;
                    }
                }
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        private void HeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (xdgAgents.DataSource != null)
                {
                    DataTable dt = ((DataView)xdgAgents.DataSource).Table;

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["Select"] = false;
                    }
                }
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            IsAllRecordsSelected = AllRecordsSelected();
        }

        private void Cal1_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(Cal1.SelectedDate.ToString(), out _fromDate);
        }

        private void Cal2_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(Cal2.SelectedDate.ToString(), out _toDate);
        }

        private void ctrlReportPremiumBreakdownAgentScreen_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        private void ctrlReportPremiumBreakdownAgentScreen_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        #endregion

    }
}
