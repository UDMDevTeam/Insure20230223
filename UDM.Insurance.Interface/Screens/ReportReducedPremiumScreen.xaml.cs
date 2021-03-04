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
using Orientation = Infragistics.Documents.Excel.Orientation;
using System.Linq;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportReducedPremiumScreen.xaml
    /// </summary>
    public partial class ReportReducedPremiumScreen
    {

        #region Constants

        //private string _fontName = "Calibri";
        //private const int _fontSize = 10;
        //private const int _pointsToTwipsFactor = 20;
        //private const int _fontHeight = _fontSize * _pointsToTwipsFactor;

        #endregion Constants

        #region Private Members

        private System.Collections.Generic.List<Record> _lstSelectedCampaigns;
        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        private RecordCollectionBase _campaigns;
        private DateTime _startDate;
        private DateTime _endDate;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        #endregion Private Members

        #region Constructors

        public ReportReducedPremiumScreen()
        {
            InitializeComponent();
            LoadCampaignInfo();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

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
                DataColumn column = new DataColumn("Select", typeof(bool));
                column.DefaultValue = false;
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

                #region Setup excel documents

                int cumulativeReportRow = 8;
                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                //string filePathAndName = GlobalSettings.UserFolder + campaignName + " Bump Up Report ~ " + DateTime.Now.Millisecond + ".xlsx";
                string filePathAndName = String.Format("{0}Reduced Premium Report ~ {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));

                Uri uri = new Uri("/Templates/ReportTemplateReducedPremium.xlsx", UriKind.Relative);
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

                Worksheet wsCumulative = wbReport.Worksheets.Add("Cumulative");

                //Worksheet wsBatchCoverSheetTemplate = wbTemplate.Worksheets["Batch Cover Sheet"];
                //Worksheet wsBatchClientSummaryTemplate = wbTemplate.Worksheets["Batch Client Summary"];
                //Worksheet wsPolicyDataTemplate = wbTemplate.Worksheets["Policy Data"];

                //Worksheet wsBatchCoverSheet = wbBatchExport.Worksheets.Add("Batch Cover Sheet");
                //Worksheet wsBatchClientSummary = wbBatchExport.Worksheets.Add("Batch Client Summary");
                //Worksheet wsPolicyData = wbBatchExport.Worksheets.Add("Policy Data");

                //SetBatchCoverSheetWSOptions(wsBatchCoverSheet);
                //SetBatchClientSummaryWSOptions(wsBatchClientSummary);
                //SetPolicyDataWSOptions(wsPolicyData);

                #endregion Setup excel documents

                #region Populating the report details for the cumulative sheet

                Methods.CopyExcelRegion(wsTemplate, 0, 0, 6, 11, wsCumulative, 0, 0);

                wsCumulative.GetCell("A1").Value = String.Format("Reduced Premium Report - Cumulative");
                wsCumulative.GetCell("A3").Value = String.Format("From {0} to {1}", _startDate.ToString("dddd, dd MMMM yyyy"), _endDate.ToString("dddd, dd MMMM yyyy"));
                wsCumulative.GetCell("A5").Value = String.Format("Date Generated: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                #endregion Populating the report details for the cumulative sheet

                foreach (DataRecord record in _lstSelectedCampaigns)
                {
                    #region Get report data from database

                    DataTable dtReducedPremiumReport;

                    //_startDate = calStartDate.SelectedDate;
                    //_endDate = calEndDate.SelectedDate;

                    long campaignID = Convert.ToInt32(record.Cells["CampaignID"].Value);
                    string campaignName = record.Cells["CampaignName"].Value.ToString();
                    string campaignCode = record.Cells["CampaignCode"].Value.ToString();

                    SqlParameter[] parameters = new SqlParameter[3];
					parameters[0] = new SqlParameter("@CampaignId", campaignID);
					parameters[1] = new SqlParameter("@StartDate", _startDate.ToString("yyyy-MM-dd"));
					parameters[2] = new SqlParameter("@EndDate", _endDate.ToString("yyyy-MM-dd"));

                    DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportReducedPremium", parameters);
                    dtReducedPremiumReport = dsReducedPremiumReport.Tables[0];

                    //                  if (dsReducedPremiumReport.Tables.Count > 0)
                    //{
                    //                      dtReducedPremiumReport = dsReducedPremiumReport.Tables[0];

                    //                      if (dtReducedPremiumReport.Rows.Count == 0)
                    //	{
                    //		Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    //		{
                    //			ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + campaignName + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
                    //		});

                    //		continue;
                    //	}
                    //}
                    //else
                    //{
                    //	Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    //	{
                    //		ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + campaignName + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
                    //	});

                    //	continue;
                    //                  }

                    #endregion Get report data from database

                    if (dtReducedPremiumReport.Rows.Count > 0)
                    {
                        Worksheet wsReport = wbReport.Worksheets.Add(campaignCode);

                        //wsReport.DisplayOptions.View = WorksheetView.PageLayout;
                        wsReport.PrintOptions.PaperSize = PaperSize.A4;
                        wsReport.PrintOptions.Orientation = Orientation.Portrait;

                        #region Populating the report details

                        Methods.CopyExcelRegion(wsTemplate, 0, 0, 6, 11, wsReport, 0, 0);

                        wsReport.GetCell("A1").Value = String.Format("Reduced Premium Report - {0}", campaignName);
                        wsReport.GetCell("A3").Value = String.Format("From {0} to {1}", _startDate.ToString("dddd, dd MMMM yyyy"), _endDate.ToString("dddd, dd MMMM yyyy"));
                        wsReport.GetCell("A5").Value = String.Format("Date Generated: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        #endregion Populating the report details

                        #region Report data

                        int rowIndex = 8;
                        //string previousValue = string.Empty;

                        foreach (DataRow dr in dtReducedPremiumReport.Rows)
                        {
                            #region Each campaign's worksheet

                            Methods.CopyExcelRegion(wsTemplate, 7, 0, 1, 11, wsReport, rowIndex - 1, 0);

                            wsReport.GetCell("A" + rowIndex.ToString()).Value = dr["Date"];
                            wsReport.GetCell("B" + rowIndex.ToString()).Value = dr["Confirmed"];
                            wsReport.GetCell("C" + rowIndex.ToString()).Value = dr["Ref No"];
                            wsReport.GetCell("D" + rowIndex.ToString()).Value = dr["Week"];
                            wsReport.GetCell("E" + rowIndex.ToString()).Value = dr["Original Policy"];
                            wsReport.GetCell("F" + rowIndex.ToString()).Value = dr["Sale Premium"];
                            wsReport.GetCell("G" + rowIndex.ToString()).Value = dr["New Policy"];
                            wsReport.GetCell("H" + rowIndex.ToString()).Value = dr["Reduced Premium"];
                            wsReport.GetCell("I" + rowIndex.ToString()).Value = dr["Premium Difference"];
                            wsReport.GetCell("J" + rowIndex.ToString()).Value = dr["TSR"];
                            wsReport.GetCell("K" + rowIndex.ToString()).Value = dr["Code"];

                            rowIndex++;

                            #endregion Each campaign's worksheet

                            #region Cumulative worksheet

                            Methods.CopyExcelRegion(wsTemplate, 7, 0, 1, 11, wsCumulative, cumulativeReportRow - 1, 0);

                            wsCumulative.GetCell("A" + cumulativeReportRow.ToString()).Value = dr["Date"];
                            wsCumulative.GetCell("B" + cumulativeReportRow.ToString()).Value = dr["Confirmed"];
                            wsCumulative.GetCell("C" + cumulativeReportRow.ToString()).Value = dr["Ref No"];
                            wsCumulative.GetCell("D" + cumulativeReportRow.ToString()).Value = dr["Week"];
                            wsCumulative.GetCell("E" + cumulativeReportRow.ToString()).Value = dr["Original Policy"];
                            wsCumulative.GetCell("F" + cumulativeReportRow.ToString()).Value = dr["Sale Premium"];
                            wsCumulative.GetCell("G" + cumulativeReportRow.ToString()).Value = dr["New Policy"];
                            wsCumulative.GetCell("H" + cumulativeReportRow.ToString()).Value = dr["Reduced Premium"];
                            wsCumulative.GetCell("I" + cumulativeReportRow.ToString()).Value = dr["Premium Difference"];
                            wsCumulative.GetCell("J" + cumulativeReportRow.ToString()).Value = dr["TSR"];
                            wsCumulative.GetCell("K" + cumulativeReportRow.ToString()).Value = dr["Code"];

                            cumulativeReportRow++;

                            #endregion Cumulative worksheet

                        }

                        #endregion Report data
                    }

                }

                if (wbReport.Worksheets.Count > 0)
                {
                    //Save excel document
                    wbReport.Save(filePathAndName);

                    //Display excel document
                    Process.Start(filePathAndName);
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

        #endregion Private Methods

        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private bool IsAllInputParametersSpecifiedAndValid()
        {
            #region Ensuring that at least one campaign was selected

            var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            _lstSelectedCampaigns = new System.Collections.Generic.List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

            if (_lstSelectedCampaigns.Count == 0)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No batches selected", ShowMessageType.Error);
                return false;
            }


            #endregion Ensuring that at least one campaign was selected

            #region Ensuring that the From Date was specified

            if (calStartDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'From Date'.", @"No 'From Date' specified", ShowMessageType.Error);
                return false;
            }
            else
            {
                _startDate = calStartDate.SelectedDate.Value;
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the From Date was specified

            if (calEndDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'To Date'.", @"No 'To Date' specified", ShowMessageType.Error);
                return false;
            }

            else
            {
                _endDate = calEndDate.SelectedDate.Value;
            }

            #endregion Ensuring that the From Date was specified

            return true;

        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //_campaigns = xdgCampaigns.Records;

                if (IsAllInputParametersSpecifiedAndValid())
                {
                    btnClose.IsEnabled = false;
                    btnReport.IsEnabled = false;
                    xdgCampaigns.IsEnabled = false;
                    calStartDate.IsEnabled = false;
                    calEndDate.IsEnabled = false;

                    //Cal2.IsEnabled = false;

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
