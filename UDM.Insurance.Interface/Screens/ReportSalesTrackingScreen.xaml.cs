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
using System.Collections.Generic;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportDiaryScreen.xaml
    /// </summary>
    public partial class ReportSalesTrackingScreen
    {

        #region Constants

        #endregion Constants

        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        //private RecordCollectionBase _campaigns;
        //private System.Collections.Generic.List<Record> _campaigns;
        private List<Record> _lstSelectedCampaigns;
        //private string _campaignIDList = String.Empty;
        private long _campaignID;

        private DateTime _startDate;
        private DateTime _endDate;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        #endregion Private Members

        #region Constructors

        public ReportSalesTrackingScreen()
        {
            InitializeComponent();
            LoadCampaignInfo();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

        private void LoadCampaignInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);

                //DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign WHERE [ID] IN (101, 102, 103, 104, 105, 193, 203, 204, 205)");
                DataTable dt = Business.Insure.INGetSalesTrackingReportLookups();
                //DataColumn column = new DataColumn("Select", typeof(bool));
                //column.DefaultValue = false;
                //dt.Columns.Add(column);
                //dt.DefaultView.Sort = "CampaignName ASC";
                //xdgCampaigns.DataSource = dt.DefaultView;
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

        private void EnableDisableExportButton()
        {
            try
            {
                if ((calStartDate.SelectedDate != null && (calEndDate.SelectedDate != null))) //&& (calEndDate.SelectedDate >= Cal1.SelectedDate)
                {
                    if (cmbCampaign.SelectedIndex != -1)
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
            cmbCampaign.IsEnabled = true;
            calStartDate.IsEnabled = true;
            calEndDate.IsEnabled = true;
        }

        private bool IsAllInputParametersSpecifiedAndValid()
        {
            #region Ensuring that at least one campaign was selected

            //var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            //_lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

            //if (_lstSelectedCampaigns.Count == 0)
            //{
            //    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No campaigns selected", ShowMessageType.Error);
            //    return false;
            //}
            //else
            //{
            //    _campaignIDList = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
            //    _campaignIDList = _campaignIDList.Substring(0, _campaignIDList.Length - 1);
            //}

            if (cmbCampaign.SelectedIndex == -1)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select a campaign from the drop down list.", "No campaign selected", ShowMessageType.Error);
                return false;
            }
            else
            {
                _campaignID = Convert.ToInt64(cmbCampaign.SelectedValue);
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

            #region Ensuring that the date range is valid

            if (calStartDate.SelectedDate > calEndDate.SelectedDate.Value)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Invalid date range specified: The 'From Date' can not be greater than the 'To Date'.", "Invalid date range", ShowMessageType.Error);
                return false;
            }

            #endregion Ensuring that the date range is valid

            // Otherwise, if all is well, proceed:
            return true;
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup excel workbook

                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateSalesTracking.xlsx");
                string filePathAndName = String.Empty;

                #region Instantiate a new Excel workbook

                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                filePathAndName = String.Format("{0} Sales Tracking Report ~ {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));

                #endregion Instantiate a new Excel workbook

                #endregion Setup excel workbook

                DataSet dsSalesTrackingReportData = Business.Insure.INGetSalesTrackingReportData(_campaignID, _startDate, _endDate);


                InsertSalesSheet(wbTemplate, wbReport, dsSalesTrackingReportData);
                InsertContactsSheet(wbTemplate, wbReport, dsSalesTrackingReportData);




                #region Save and open the resulting workbook

                if (wbReport.Worksheets.Count > 0)
                    {
                        //Save excel document
                        wbReport.Save(filePathAndName);

                        //Display excel document
                        Process.Start(filePathAndName);
                    }
                #endregion Save and open the resulting workbook

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

        public void InsertSalesSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsSalesTrackingReportData)
        {
            try
            {
                #region Get the report data

                Worksheet wsTemplate = wbTemplate.Worksheets["Sales"];

                DataTable dtMainReportData = dsSalesTrackingReportData.Tables[0];
                DataTable dtColumnMappings = dsSalesTrackingReportData.Tables[1];
                DataTable dtTotalsReportData = dsSalesTrackingReportData.Tables[2];
                DataTable dtTotalsColumnMappings = dsSalesTrackingReportData.Tables[3];
                DataTable dtReportConfigs = dsSalesTrackingReportData.Tables[4];

                #endregion Get the report data

                #region Variable declarations



                string campaign = String.Empty;
                string campaignFilterString = String.Empty;
                string currentTSR = String.Empty;
                string currentTSRFilterString = String.Empty;

                byte templateColumnSpan = Convert.ToByte(dtReportConfigs.Rows[0]["TemplateSalesColumnSpan"]);
                byte templateRowSpan = Convert.ToByte(dtReportConfigs.Rows[0]["TemplateSalesRowSpan"]);
                byte templateDataRowIndex = Convert.ToByte(dtReportConfigs.Rows[0]["TemplateSalesDataRowIndex"]);
                byte templateTotalsRowIndex = Convert.ToByte(dtReportConfigs.Rows[0]["TemplateSalesTotalsRowIndex"]);

                int reportRow = 6;

                #endregion Variable declarations


                #region Add the current sales consultant's worksheet

                Worksheet wsReport = wbReport.Worksheets.Add(Methods.ParseWorksheetName(wbReport, "Upgrade Sales Tracking Report"));
                Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                #endregion Add the current sales consultant's worksheet

                #region Insert the column headings and populate the details

                Methods.CopyExcelRegion(wsTemplate, 0, 0, templateRowSpan, templateColumnSpan, wsReport, 0, 0);

                wsReport.GetCell("A3").Value = dtReportConfigs.Rows[0]["SubTitle"];

                wsReport.GetCell("A5").Value = dtReportConfigs.Rows[0]["MonthYearRange"];

                #endregion Insert the column headings and populate the details

                #region Add the data

                reportRow = Methods.MapTemplatizedExcelValues(wsTemplate, dtMainReportData, dtColumnMappings, templateDataRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                //Totals

                Methods.MapTemplatizedExcelValues(wsTemplate, dtTotalsReportData, dtTotalsColumnMappings, templateTotalsRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);


                #endregion Add the data
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void InsertContactsSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsSalesTrackingReportData)
        {
            try
            {
                #region Get the report data

                Worksheet wsTemplate = wbTemplate.Worksheets["Contacts"];

                DataTable dtReportConfigs = dsSalesTrackingReportData.Tables[4];
                DataTable dtMainReportData = dsSalesTrackingReportData.Tables[5];
                DataTable dtColumnMappings = dsSalesTrackingReportData.Tables[6];
                DataTable dtTotalsReportData = dsSalesTrackingReportData.Tables[7];
                DataTable dtTotalsColumnMappings = dsSalesTrackingReportData.Tables[8];
                

                #endregion Get the report data

                #region Variable declarations

                string campaign = String.Empty;
                string campaignFilterString = String.Empty;
                string currentTSR = String.Empty;
                string currentTSRFilterString = String.Empty;

                byte templateColumnSpan = Convert.ToByte(dtReportConfigs.Rows[0]["TemplateContactsColumnSpan"]);
                byte templateRowSpan = Convert.ToByte(dtReportConfigs.Rows[0]["TemplateContactsRowSpan"]);
                byte templateDataRowIndex = Convert.ToByte(dtReportConfigs.Rows[0]["TemplateContactsDataRowIndex"]);
                byte templateTotalsRowIndex = Convert.ToByte(dtReportConfigs.Rows[0]["TemplateContactsTotalsRowIndex"]);

                int reportRow = 6;

                #endregion Variable declarations

                #region Add the current sales consultant's worksheet

                Worksheet wsReport = wbReport.Worksheets.Add(Methods.ParseWorksheetName(wbReport, "Upgrade Contact Tracking Report"));
                Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                #endregion Add the current sales consultant's worksheet

                #region Insert the column headings and populate the details

                Methods.CopyExcelRegion(wsTemplate, 0, 0, templateRowSpan, templateColumnSpan, wsReport, 0, 0);

                wsReport.GetCell("A3").Value = dtReportConfigs.Rows[0]["SubTitle"];

                wsReport.GetCell("A5").Value = dtReportConfigs.Rows[0]["MonthYearRange"];

                #endregion Insert the column headings and populate the details

                #region Add the data

                reportRow = Methods.MapTemplatizedExcelValues(wsTemplate, dtMainReportData, dtColumnMappings, templateDataRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                //Totals

                Methods.MapTemplatizedExcelValues(wsTemplate, dtTotalsReportData, dtTotalsColumnMappings, templateTotalsRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);


                #endregion Add the data
            }
            catch (Exception ex)
            {
                HandleException(ex);
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

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //_campaigns = xdgCampaigns.Records;

                //var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                //_campaigns = new System.Collections.Generic.List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

                if (IsAllInputParametersSpecifiedAndValid())
                {
                    btnClose.IsEnabled = false;
                    btnReport.IsEnabled = false;
                    cmbCampaign.IsEnabled = false;
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

        //private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            dr["Select"] = true;
        //        }

        //        EnableDisableExportButton();
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        //private void HeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            dr["Select"] = false;
        //        }

        //        EnableDisableExportButton();
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        //private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (_xdgHeaderPrefixAreaCheckbox != null)
        //        {
        //            _xdgHeaderPrefixAreaCheckbox.IsChecked = AllRecordsSelected();
        //        }

        //        EnableDisableExportButton();
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        //private void HeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        _xdgHeaderPrefixAreaCheckbox = (CheckBox)sender;
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

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

        private void cmbCampaign_Loaded(object sender, RoutedEventArgs e)
        {
            cmbCampaign.Focus();
        }

        #endregion

        private void cmbCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCampaign.SelectedIndex != -1)
            {
                btnReport.IsEnabled = true;
            }
        }
    }
}
