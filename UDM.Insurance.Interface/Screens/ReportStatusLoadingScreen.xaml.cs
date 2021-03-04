using System.Data.SqlClient;
using System.Windows.Resources;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using Orientation = Infragistics.Documents.Excel.Orientation;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportBumpUpScreen.xaml
    /// </summary>
    public partial class ReportStatusLoadingScreen
    {

         #region Constants

        //private string _fontName = "Calibri";
        //private const int _fontSize = 10;
        //private const int _pointsToTwipsFactor = 20;
        //private const int _fontHeight = _fontSize * _pointsToTwipsFactor;

        #endregion



        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        //private RecordCollectionBase _campaigns;
        private DateTime _startDate;
        private DateTime _endDate;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        #endregion



        #region Constructors

        public ReportStatusLoadingScreen()
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

                DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign");
                DataColumn column = new DataColumn("Select", typeof (bool)) {DefaultValue = false};
                dt.Columns.Add(column);
                dt.DefaultView.Sort = "CampaignName ASC";
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
                if ((calStartDate.SelectedDate != null && (calEndDate.SelectedDate != null))) //&& (calEndDate.SelectedDate >= Cal1.SelectedDate)
                {
                    if (_xdgHeaderPrefixAreaCheckbox != null && (_xdgHeaderPrefixAreaCheckbox.IsChecked == true || _xdgHeaderPrefixAreaCheckbox.IsChecked == null))
                    {
                        btnReport.IsEnabled = true;
                        return;
                    }
                }

                btnReport.IsEnabled = false;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        //private void SetBatchCoverSheetWSOptions(Worksheet ws)
        //{
        //    ws.DisplayOptions.ShowGridlines = false;
        //    ws.PrintOptions.PaperSize = PaperSize.A4;
        //    ws.PrintOptions.Orientation = Orientation.Portrait;
        //    ws.PrintOptions.LeftMargin = 0.6;
        //    ws.PrintOptions.TopMargin = 0.8;
        //    //ws.PrintOptions.RightMargin = 0.6;
        //    ws.PrintOptions.BottomMargin = 0.8;
        //    //ws.PrintOptions.CenterHorizontally = true;
        //    //ws.PrintOptions.ScalingType = ScalingType.UseScalingFactor;
        //    //ws.PrintOptions.ScalingFactor = 90;

        //    ws.DisplayOptions.MagnificationInPageLayoutView = 100;
        //    ws.DisplayOptions.View = WorksheetView.Normal;
        //}

        //private void SetBatchClientSummaryWSOptions(Worksheet ws)
        //{
        //    ws.DisplayOptions.ShowGridlines = false;
        //    ws.PrintOptions.PaperSize = PaperSize.A4;
        //    ws.PrintOptions.Orientation = Orientation.Portrait;
        //    ws.PrintOptions.LeftMargin = 0.6;
        //    ws.PrintOptions.TopMargin = 0.8;
        //    //ws.PrintOptions.RightMargin = 0.2;
        //    ws.PrintOptions.BottomMargin = 0.8;
        //    //ws.PrintOptions.CenterHorizontally = true;
        //    //ws.PrintOptions.ScalingType = ScalingType.UseScalingFactor;
        //    //ws.PrintOptions.ScalingFactor = 90;

        //    ws.DisplayOptions.MagnificationInPageLayoutView = 100;
        //    ws.DisplayOptions.View = WorksheetView.Normal;
        //}

        //private void SetPolicyDataWSOptions(Worksheet ws)
        //{
        //    ws.DisplayOptions.ShowGridlines = false;
        //    ws.PrintOptions.PaperSize = PaperSize.A4;
        //    ws.PrintOptions.Orientation = Orientation.Landscape;
        //    ws.PrintOptions.LeftMargin = 0.6;
        //    ws.PrintOptions.TopMargin = 0.8;
        //    //ws.PrintOptions.RightMargin = 0.6;
        //    ws.PrintOptions.BottomMargin = 0.8;
        //    //ws.PrintOptions.CenterHorizontally = true;
        //    //ws.PrintOptions.ScalingType = ScalingType.UseScalingFactor;
        //    //ws.PrintOptions.ScalingFactor = 90;

        //    ws.DisplayOptions.MagnificationInPageLayoutView = 100;
        //    ws.DisplayOptions.View = WorksheetView.Normal;
        //}

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";

            btnReport.IsEnabled = true;
            btnClose.IsEnabled = true;
            xdgCampaigns.IsEnabled = true;
            calStartDate.IsEnabled = true;
            calEndDate.IsEnabled = true;
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                IEnumerable<DataRecord> campaigns = e.Argument as IEnumerable<DataRecord>;

                if (campaigns != null)
                    foreach (DataRecord record in campaigns)
                    {
                        if ((bool)record.Cells["Select"].Value)
                        {
                            long campaignID = Convert.ToInt32(record.Cells["CampaignID"].Value);
                            string campaignName = record.Cells["CampaignName"].Value.ToString();

                            # region Setup excel documents

                            Workbook wbTemplate;
                            Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                            string filePathAndName = GlobalSettings.UserFolder + campaignName + " Status Loading Report ~ " + DateTime.Now.Millisecond + ".xlsx";

                            Uri uri = new Uri("/Templates/ReportTemplateStatusLoading.xlsx", UriKind.Relative);
                            StreamResourceInfo info = Application.GetResourceStream(uri);
                            if (info != null)
                            {
                                wbTemplate = Workbook.Load(info.Stream, true);
                            }
                            else
                            {
                                return;
                            }

                            Worksheet wsTemplate = wbTemplate.Worksheets["Status loading"];
                            string worksheetName = Methods.ParseWorksheetName(wbReport, campaignName);
                            Worksheet wsReport = wbReport.Worksheets.Add(worksheetName);
                         
                            wsReport.PrintOptions.PaperSize = PaperSize.A4;
                            wsReport.PrintOptions.Orientation = Orientation.Portrait;

                            #endregion

                            #region Get report data from database

                            DataTable dtLeadAllocationData;

                            SqlParameter[] parameters = new SqlParameter[3];
                            parameters[0] = new SqlParameter("@CampaignId", campaignID+",");
                            parameters[1] = new SqlParameter("@StartDate", _startDate.ToString("yyyy-MM-dd"));
                            parameters[2] = new SqlParameter("@EndDate", _endDate.ToString("yyyy-MM-dd"));

                            DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spINReportStatusLoading", parameters);
                            if (dsLeadAllocationData.Tables.Count > 0)
                            {
                                dtLeadAllocationData = dsLeadAllocationData.Tables[0];

                                if (dtLeadAllocationData.Rows.Count == 0)
                                {
                                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + campaignName + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
                                    });

                                    continue;
                                }
                            }
                            else
                            {
                                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                                {
                                    ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + campaignName + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
                                });

                                continue;
                            }

                            #endregion

                            Methods.CopyExcelRegion(wsTemplate, 0, 0, 1, 11, wsReport, 0, 0);

                            #region report data

                            {
                                int rowIndex = 2;
                                //string previousValue = string.Empty;

                                foreach (DataRow dr in dtLeadAllocationData.Rows)
                                {
                                    Methods.CopyExcelRegion(wsTemplate, rowIndex - 1, 0, 1, 11, wsReport, rowIndex - 1, 0);

                                    string DateAccessed = dr["DateaccessedbyAlta"].ToString();
                                    if (DateAccessed != string.Empty)
                                    {
                                        DateAccessed = DateAccessed.Substring(0, 10);
                                    }
                                    wsReport.GetCell("A" + rowIndex.ToString()).Value = dr["AgentName"];
                                    wsReport.GetCell("B" + rowIndex.ToString()).Value = dr["Campaign"];
                                    wsReport.GetCell("C" + rowIndex.ToString()).Value = dr["BatchNumber"];
                                    wsReport.GetCell("D" + rowIndex.ToString()).Value = dr["RefNo"];
                                    wsReport.GetCell("E" + rowIndex.ToString()).Value = dr["NameOfLead"];
                                    wsReport.GetCell("F" + rowIndex.ToString()).Value = DateAccessed;
                                    wsReport.GetCell("G" + rowIndex.ToString()).Value = dr["OriginalStatus"];
                                    wsReport.GetCell("H" + rowIndex.ToString()).Value = dr["NewStatus"];
                                    wsReport.GetCell("I" + rowIndex.ToString()).Value = dr["AccessedBy"];
                                    rowIndex++;
                                }
                            }

                            #endregion

                            //Save excel document
                            wbReport.Save(filePathAndName);

                            //Display excel document
                            Process.Start(filePathAndName);
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

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;
            btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
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
            try
            {
                IEnumerable<DataRecord> campaigns = xdgCampaigns.Records.Cast<DataRecord>().ToArray();

                btnClose.IsEnabled = false;
                btnReport.IsEnabled = false;
                xdgCampaigns.IsEnabled = false;
                calStartDate.IsEnabled = false;
				calEndDate.IsEnabled = false;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += Report;
                worker.RunWorkerCompleted += ReportCompleted;
                worker.RunWorkerAsync(campaigns);

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
            DateTime.TryParse(calStartDate.SelectedDate.ToString(), out _startDate);
            EnableDisableExportButton();
        }

		private void Cal2_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
		{
			DateTime.TryParse(calEndDate.SelectedDate.ToString(), out _endDate);
			EnableDisableExportButton();
		}

        #endregion

    }
}
