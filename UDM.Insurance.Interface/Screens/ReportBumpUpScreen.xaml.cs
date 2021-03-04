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
    public partial class ReportBumpUpScreen
    {

        #region Constants

        //private string _fontName = "Calibri";
        //private const int _fontSize = 10;
        //private const int _pointsToTwipsFactor = 20;
        //private const int _fontHeight = _fontSize * _pointsToTwipsFactor;

        #endregion

        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;

        private List<Record> _lstSelectedCampaigns;
        private string _campaignIDList = String.Empty;

        private DateTime _startDate;
        private DateTime _endDate;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        #endregion

        #region Constructors

        public ReportBumpUpScreen()
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

        //private void EnableDisableExportButton()
        //{
        //    try
        //    {
        //        if ((calStartDate.SelectedDate != null && (calEndDate.SelectedDate != null))) //&& (calEndDate.SelectedDate >= Cal1.SelectedDate)
        //        {
        //            if (_xdgHeaderPrefixAreaCheckbox != null && (_xdgHeaderPrefixAreaCheckbox.IsChecked == true || _xdgHeaderPrefixAreaCheckbox.IsChecked == null))
        //            {
        //                btnReport.IsEnabled = true;
        //                return;
        //            }
        //        }

        //        btnReport.IsEnabled = false;
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        private bool IsAllInputParametersSpecifiedAndValid()
        {


            #region Ensuring that at least one campaign was selected

            var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

            //_campaignIDList = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
            //_campaignIDList = _campaignIDList.Substring(0, _campaignIDList.Length - 1);

            if (_lstSelectedCampaigns.Count == 0)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No batches selected", ShowMessageType.Error);
                return false;
            }
            else
            {
                _campaignIDList = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                _campaignIDList = _campaignIDList.Substring(0, _campaignIDList.Length - 1);
            }

            #endregion Ensuring that at least one campaign was selected

            #region Ensuring that at least 1 batch is selected - Not relevant for this scenario

            //var lstTemp = (from r in xdgBatches.Records where (bool)((Infragistics.Windows.DataPresenter.DataRecord)r).Cells["Select"].Value select r).ToList();
            //_lstSelectedAgents = new List<Infragistics.Windows.DataPresenter.Record>(lstTemp.OrderBy(r => ((Infragistics.Windows.DataPresenter.DataRecord)r).Cells["Code"].Value));

            //if (_lstSelectedAgents.Count == 0)
            //{
            //    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 batch from the list.", "No batches selected", ShowMessageType.Error);
            //    return false;
            //}

            #endregion Ensuring that at least 1 batch is selected - Not relevant for this scenario

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

        private void AddBumpUpReportSheet(Workbook wbResultingWorkbook, Workbook wbTemplate, string reportTemplateSheetName, string newWorksheetDescription, DataTable dataTable/*long batchID, DateTime fromDate, DateTime toDate*/)
        {
            if (dataTable.Rows.Count > 0)
            {

                Worksheet wsTemplate = wbTemplate.Worksheets[reportTemplateSheetName];
                Worksheet wsReport = wbResultingWorkbook.Worksheets.Add(newWorksheetDescription);

                wsReport.PrintOptions.PaperSize = PaperSize.A4;
                wsReport.PrintOptions.Orientation = Orientation.Portrait;

                #region Get report data from database

                //DataTable dtLeadAllocationData;

                //SqlParameter[] parameters = new SqlParameter[3];
                //parameters[0] = new SqlParameter("@CampaignId", campaignID);
                //parameters[1] = new SqlParameter("@StartDate", _startDate.ToString("yyyy-MM-dd"));
                //parameters[2] = new SqlParameter("@EndDate", _endDate.ToString("yyyy-MM-dd"));

                //DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spINReportBumpUp", parameters);

                //if (dsLeadAllocationData.Tables.Count > 0)
                //{
                //    dtLeadAllocationData = dsLeadAllocationData.Tables[0];

                //    if (dtLeadAllocationData.Rows.Count == 0)
                //    {
                //        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                //        {
                //            ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + campaignName + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
                //        });

                //        continue;
                //    }
                //}
                //else
                //{
                //    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                //    {
                //        ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + campaignName + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
                //    });

                //    continue;
                //}

                #endregion Get report data from database

                Methods.CopyExcelRegion(wsTemplate, 0, 0, 1, 11, wsReport, 0, 0);

                #region Report Data

                {
                    int rowIndex = 2;
                    //string previousValue = string.Empty;

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        Methods.CopyExcelRegion(wsTemplate, rowIndex - 1, 0, 1, 11, wsReport, rowIndex - 1, 0);

                        wsReport.GetCell("A" + rowIndex).Value = dr["Date"];
                        wsReport.GetCell("B" + rowIndex).Value = dr["Confirmed"];
                        wsReport.GetCell("C" + rowIndex).Value = dr["Ref No"];
                        wsReport.GetCell("D" + rowIndex).Value = dr["Week"];
                        wsReport.GetCell("E" + rowIndex).Value = dr["Original Policy"];
                        wsReport.GetCell("F" + rowIndex).Value = dr["Sale Premium"];
                        wsReport.GetCell("G" + rowIndex).Value = dr["New Policy"];
                        wsReport.GetCell("H" + rowIndex).Value = dr["Bumped Up Premium"];
                        wsReport.GetCell("I" + rowIndex).Value = dr["Premium Difference"];
                        wsReport.GetCell("J" + rowIndex).Value = dr["TSR"];
                        wsReport.GetCell("K" + rowIndex).Value = dr["Code"];
                        wsReport.GetCell("L" + rowIndex).Value = dr["Bump Up Description"];

                        rowIndex++;
                    }
                }

                #endregion Report Data
            }
        }

        //private void AddBumpUpReportSheetSummary(Workbook wbResultingWorkbook, Workbook wbTemplate, string reportTemplateSheetName, string newWorksheetDescription, DataTable dataTable/*long batchID, DateTime fromDate, DateTime toDate*/)
        //{
        //    if (dataTable.Rows.Count > 0)
        //    {

        //        Worksheet wsTemplate = wbTemplate.Worksheets[reportTemplateSheetName];
        //        Worksheet wsReport = wbResultingWorkbook.Worksheets.Add(newWorksheetDescription);

        //        wsReport.PrintOptions.PaperSize = PaperSize.A4;
        //        wsReport.PrintOptions.Orientation = Orientation.Portrait;

        //        #region Get report data from database

        //        //DataTable dtLeadAllocationData;

        //        //SqlParameter[] parameters = new SqlParameter[3];
        //        //parameters[0] = new SqlParameter("@CampaignId", campaignID);
        //        //parameters[1] = new SqlParameter("@StartDate", _startDate.ToString("yyyy-MM-dd"));
        //        //parameters[2] = new SqlParameter("@EndDate", _endDate.ToString("yyyy-MM-dd"));

        //        //DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spINReportBumpUp", parameters);

        //        //if (dsLeadAllocationData.Tables.Count > 0)
        //        //{
        //        //    dtLeadAllocationData = dsLeadAllocationData.Tables[0];

        //        //    if (dtLeadAllocationData.Rows.Count == 0)
        //        //    {
        //        //        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
        //        //        {
        //        //            ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + campaignName + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
        //        //        });

        //        //        continue;
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
        //        //    {
        //        //        ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + campaignName + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
        //        //    });

        //        //    continue;
        //        //}

        //        #endregion Get report data from database

        //        Methods.CopyExcelRegion(wsTemplate, 0, 0, 1, 11, wsReport, 0, 0);

        //        #region Report Data

        //        {
        //            int rowIndex = 2;
        //            //string previousValue = string.Empty;

        //            foreach (DataRow dr in dataTable.Rows)
        //            {
        //                Methods.CopyExcelRegion(wsTemplate, rowIndex - 1, 0, 1, 11, wsReport, rowIndex - 1, 0);

        //                wsReport.GetCell("A" + rowIndex).Value = dr["Date"];
        //                wsReport.GetCell("B" + rowIndex).Value = dr["Confirmed"];
        //                wsReport.GetCell("C" + rowIndex).Value = dr["Ref No"];
        //                wsReport.GetCell("D" + rowIndex).Value = dr["Week"];
        //                wsReport.GetCell("E" + rowIndex).Value = dr["Original Policy"];
        //                wsReport.GetCell("F" + rowIndex).Value = dr["Sale Premium"];
        //                wsReport.GetCell("G" + rowIndex).Value = dr["New Policy"];
        //                wsReport.GetCell("H" + rowIndex).Value = dr["Bumped Up Premium"];
        //                wsReport.GetCell("I" + rowIndex).Value = dr["Premium Difference"];
        //                wsReport.GetCell("J" + rowIndex).Value = dr["TSR"];
        //                wsReport.GetCell("K" + rowIndex).Value = dr["Code"];
        //                //wsReport.GetCell("L" + rowIndex).Value = dr["Bump Up Description"];

        //                rowIndex++;
        //            }
        //        }

        //        #endregion Report Data
        //    }
        //}

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup excel documents

                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                //string filePathAndName = GlobalSettings.UserFolder + campaignName + " Bump Up Report ~ " + DateTime.Now.Millisecond + ".xlsx";
                string filePathAndName = String.Format("{0}Bump Up Report ~ {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                Uri uri = new Uri("/Templates/ReportTemplateBumpUp.xlsx", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    wbTemplate = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }


                #endregion Setup excel documents

                #region Add the summary sheet

                DataTable dtBumpUpReportSummaryData = UDM.Insurance.Business.Insure.INGetBumpUpReportSummaryData(_campaignIDList, _startDate, _endDate);
                AddBumpUpReportSheet(wbReport, wbTemplate, "Report", "Summary", dtBumpUpReportSummaryData);

                #endregion Add the summary sheet

                foreach (DataRecord record in _lstSelectedCampaigns)
                {
                    if ((bool)record.Cells["Select"].Value)
                    {
                        long campaignID = Convert.ToInt32(record.Cells["CampaignID"].Value);
                        //string campaignName = record.Cells["CampaignName"].Value.ToString();
                        string campaignCode = record.Cells["CampaignCode"].Value.ToString();
                      
                        #region Add a new sheet for each campaign

                        DataTable dtBumpUpReportData = UDM.Insurance.Business.Insure.INGetBumpUpReportData(campaignID, _startDate, _endDate);
                        AddBumpUpReportSheet(wbReport, wbTemplate, "Report", campaignCode, dtBumpUpReportData);

                        #endregion Add a new sheet for each campaign

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
                //IEnumerable<DataRecord> campaigns = xdgCampaigns.Records.Cast<DataRecord>().ToArray();

                if (IsAllInputParametersSpecifiedAndValid())
                {

                    btnClose.IsEnabled = false;
                    btnReport.IsEnabled = false;
                    xdgCampaigns.IsEnabled = false;
                    calStartDate.IsEnabled = false;
                    calEndDate.IsEnabled = false;

                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += Report;
                    worker.RunWorkerCompleted += ReportCompleted;
                    //worker.RunWorkerAsync(campaigns);
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

                //EnableDisableExportButton();
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

                //EnableDisableExportButton();
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

                //EnableDisableExportButton();
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
            //EnableDisableExportButton();
        }

		private void Cal2_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
		{
			DateTime.TryParse(calEndDate.SelectedDate.ToString(), out _endDate);
			//EnableDisableExportButton();
		}

        #endregion

    }
}
