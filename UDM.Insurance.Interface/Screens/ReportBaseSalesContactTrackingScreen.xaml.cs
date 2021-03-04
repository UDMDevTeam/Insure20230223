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
using UDM.Insurance.Interface.Data;

namespace UDM.Insurance.Interface.Screens
{

    public partial class ReportBaseSalesContactTrackingScreen
    {

        #region Constants

        #endregion Constants

        #region Private Members
        private string _campaignIDs = String.Empty;
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

        private ReportData _rData = new ReportData();
        public ReportData RData
        {
            get
            {
                return _rData;
            }
            set
            {
                _rData = value;
            }
        }

        #endregion Private Members

        #region Constructors

        public ReportBaseSalesContactTrackingScreen()
        {
            InitializeComponent();
            LoadLookups();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

        private void LoadLookups()
        {
            DataTable dtBatchReportTypes = Business.Insure.INGetBatchReportTypes();
            cmbReportType.Populate(dtBatchReportTypes, "ReportType", "ID");
        }

        private void EnableDisableExportButton()
        {
            try
            {
                if ((calStartDate.SelectedDate != null && (calEndDate.SelectedDate != null))) //&& (calEndDate.SelectedDate >= Cal1.SelectedDate)
                {
                    var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                    _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignType"].Value));
                    //lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignType"]).ToList<Record>();

                    if (_lstSelectedCampaigns.Count != 0)
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
            cmbReportType.IsEnabled = true;
            xdgCampaigns.IsEnabled = true;
            calStartDate.IsEnabled = true;
            calEndDate.IsEnabled = true;
        }

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
        

        private bool IsAllInputParametersSpecifiedAndValid()
        {
            #region Ensuring that at least one campaign was selected

            var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignType"].Value));

            if (_lstSelectedCampaigns.Count == 0)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No campaigns selected", ShowMessageType.Error);
                return false;
            }
            else
            {
                //_campaignIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                _campaignIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignTypeID"].Value + ",");
                _campaignIDs = _campaignIDs.Substring(0, _campaignIDs.Length - 1);
            }

            //if (cmbCampaign.SelectedIndex == -1)
            //{
            //    ShowMessageBox(new INMessageBoxWindow1(), "Please select a campaign from the drop down list.", "No campaign selected", ShowMessageType.Error);
            //    return false;
            //}
            //else
            //{
            //    _campaignID = Convert.ToInt64(cmbCampaign.SelectedValue);
            //}

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

                bool isCampaignTypeIDs = false;                

                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    isCampaignTypeIDs = Convert.ToByte(cmbReportType.SelectedValue) == 1 ? true : false;
                });



                #region Setup excel workbook

                //This is referencing the batch report template because the report used to be on the batch report.
                //I didn't have time to put it in a separate sheet.
                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateBatchReport.xlsx");
                string filePathAndName = String.Empty;

                DataSet dsBaseSalesTrackingReportData = Business.Insure.INGetConversionReportData(_campaignIDs, true, false, _startDate, _endDate, (int)RData.Weeks, isCampaignTypeIDs);

                DataSet dsBaseContactTrackingReportData = Business.Insure.INGetConversionReportData(_campaignIDs, false, true, _startDate, _endDate, (int)RData.Weeks, isCampaignTypeIDs);

                #region Instantiate a new Excel workbook

                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                filePathAndName = String.Format("{0} Base Sales and Contact Tracking Report ~ {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));

                #endregion Instantiate a new Excel workbook

                #endregion Setup excel workbook

                //DataSet dsSalesTrackingReportData = Business.Insure.INGetSalesTrackingReportData(_campaignID, _startDate, _endDate);


                InsertSalesContactSheet(wbTemplate, wbReport, dsBaseSalesTrackingReportData, "Sales");
                InsertSalesContactSheet(wbTemplate, wbReport, dsBaseContactTrackingReportData, "Contacts");
                //InsertContactsSheet(wbTemplate, wbReport, dsBaseContactTrackingReportData);




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

        public void InsertSalesContactSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsConversionReportData, string worksheetName)
        {
            try
            {
                #region Get the report data
                
                //if (RData.Weeks == Business.lkpBaseSalesContactsWeeks.Weeks13)
                //{
                //    wsTemplate = wbTemplate.Worksheets["SalesAndContactsConversion"];
                //}
                //else if (RData.Weeks == Business.lkpBaseSalesContactsWeeks.Weeks24)
                //{
                //    wsTemplate = wbTemplate.Worksheets["SalesAndContactsConversion(24W)"];
                //}
                

                DataTable dtReportConfigs = dsConversionReportData.Tables[0];
                DataTable dtColumnMappings = dsConversionReportData.Tables[3];
                DataTable dtTotalsColumnMappings = dsConversionReportData.Tables[4];
                DataTable dtTemplateSettings = dsConversionReportData.Tables[5];

                Worksheet wsTemplate = wbTemplate.Worksheets[dtTemplateSettings.Rows[0]["TemplateSheet"].ToString()];

                #endregion Get the report data

                #region Variable declarations

                string filePathAndName = String.Empty;

                string campaign = String.Empty;
                string campaignFilterString = String.Empty;
                string currentTSR = String.Empty;
                string currentTSRFilterString = String.Empty;

                byte templateColumnSpan = byte.Parse(dtTemplateSettings.Rows[0]["TemplateColumnSpan"].ToString());
                byte templateRowSpan = byte.Parse(dtTemplateSettings.Rows[0]["TemplateRowSpan"].ToString());
                byte templateDataRowIndex = byte.Parse(dtTemplateSettings.Rows[0]["TemplateDataRowIndex"].ToString());

                #endregion Variable declarations

                foreach (DataRow row in dtReportConfigs.Rows)
                {


                    DataTable dtMainReportData = dsConversionReportData.Tables[1].Select(row["FilterString"].ToString()).CopyToDataTable();/*.OrderBy(row["OrderByString"].ToString());*/
                    DataTable dtTotalsReportData = dsConversionReportData.Tables[2].Select(row["FilterString"].ToString()).CopyToDataTable(); ;
                    #region Loop through each campaign and create a new workbook for each                   
                    int reportRow = byte.Parse(dtTemplateSettings.Rows[0]["ReportRow"].ToString());

                    #region Add the current sales consultant's worksheet

                    Worksheet wsReport = wbReport.Worksheets.Add(Methods.ParseWorksheetName(wbReport, row["CampaignCode"].ToString() + "(" + worksheetName + ")"));
                    Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                    #endregion Add the current sales consultant's worksheet

                    #region Insert the column headings and populate the details

                    Methods.CopyExcelRegion(wsTemplate, 0, 0, templateRowSpan, templateColumnSpan, wsReport, 0, 0);

                    wsReport.GetCell(dtTemplateSettings.Rows[0]["TitleCell"].ToString()).Value = row["CampaignName"];

                    wsReport.GetCell(dtTemplateSettings.Rows[0]["SubTitle1Cell"].ToString()).Value = row["SubTitle1"];

                    wsReport.GetCell(dtTemplateSettings.Rows[0]["SubTitle2Cell"].ToString()).Value = row["SubTitle2"];

                    //wsReport.GetCell("A5").Value = dtReportConfigs.Rows[0]["MonthYearRange"];

                    #endregion Insert the column headings and populate the details

                    #region Add the data

                    reportRow = Methods.MapTemplatizedExcelValues(wsTemplate, dtMainReportData, dtColumnMappings, templateDataRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                    //Totals

                    Methods.MapTemplatizedExcelValues(wsTemplate, dtTotalsReportData, dtTotalsColumnMappings, templateDataRowIndex + 1, 0, 0, templateColumnSpan, wsReport, reportRow, 0);


                    #endregion Add the data




                    #endregion Loop through each campaign and create a new workbook for each

                }
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
                    cmbReportType.IsEnabled = false;
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

        private void cmbReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //ComboBoxItem selectedItem = (ComboBoxItem)cmbReportType.SelectedItem;
                //_reportTypeID = int.Parse(selectedItem.Tag.ToString());
                //LoadCampaignInfo(_reportTypeID);
                if (cmbReportType.SelectedValue != null)
                {
                    DataTable dtReportTypeCampaigns = Business.Insure.INGetBaseSalesContactReportCampaignsOrCampaignTypesByReportType(Convert.ToByte(cmbReportType.SelectedValue));

                    xdgCampaigns.DataSource = dtReportTypeCampaigns.DefaultView;
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
                btnReport.IsEnabled = true;
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = true;
                }
                foreach (var item in xdgCampaigns.SelectedItems)
                {
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
                DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;
                btnReport.IsEnabled = false;
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = false;
                }
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
                btnReport.IsEnabled = true;
                if (_xdgHeaderPrefixAreaCheckbox != null)
                {
                    _xdgHeaderPrefixAreaCheckbox.IsChecked = AllRecordsSelected();
                }
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

        #endregion Event Handlers
    }
}
