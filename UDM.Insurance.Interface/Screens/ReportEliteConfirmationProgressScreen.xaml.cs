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
using UDM.Insurance.Business;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportDiaryScreen.xaml
    /// </summary>
    public partial class ReportEliteConfirmationProgressScreen
    {

        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        //private RecordCollectionBase _campaigns;
        private System.Collections.Generic.List<Record> _campaigns;
        private DateTime _startDate;
        private DateTime _endDate;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        //private DataSet _ds;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/216354979/comments for original request
        /// </summary>
        public ReportEliteConfirmationProgressScreen()
        {
            InitializeComponent();
            //LoadCampaignInfo();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

        //private bool? AllRecordsSelected()
        //{
        //    try
        //    {
        //        bool allSelected = true;
        //        bool noneSelected = true;

        //        if (xdgCampaigns.DataSource != null)
        //        {
        //            foreach (DataRow dr in ((DataView)xdgCampaigns.DataSource).Table.Rows)
        //            {
        //                allSelected = allSelected && (bool)dr["Select"];
        //                noneSelected = noneSelected && !(bool)dr["Select"];
        //            }
        //        }

        //        if (allSelected)
        //        {
        //            return true;
        //        }
        //        if (noneSelected)
        //        {
        //            return false;
        //        }

        //        return null;
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //        return null;
        //    }
        //}

        //private void LoadCampaignInfo()
        //{
        //    try
        //    {
        //        SetCursor(Cursors.Wait);

        //        DataTable dtLookups = Insure.INGetBumpUpPotentialReportLookupsAndConfigs().Tables[0];
        //        xdgCampaigns.DataSource = dtLookups.DefaultView; //_ds.Tables[0].DefaultView;
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }

        //    finally
        //    {
        //        SetCursor(Cursors.Arrow);
        //    }
        //}

        private void EnableDisableExportButton()
        {
            try
            {
                if ((calStartDate.SelectedDate != null && (calEndDate.SelectedDate != null))) //&& (calEndDate.SelectedDate >= Cal1.SelectedDate)
                {
                    //if (_xdgHeaderPrefixAreaCheckbox != null && (_xdgHeaderPrefixAreaCheckbox.IsChecked == true || _xdgHeaderPrefixAreaCheckbox.IsChecked == null))
                    //{
                        btnReport.IsEnabled = true;
                        return;
                    //}
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
            //xdgCampaigns.IsEnabled = true;
            calStartDate.IsEnabled = true;
            calEndDate.IsEnabled = true;
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup excel documents

                string filePathAndName = String.Format("{0}Immediate Sort Report ~ {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));

                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateEliteConfirmationProgress.xlsx");
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup excel documents

                DataSet dsEliteConfirmationProgressReport = Insure.INGetEliteConfirmationProgressReport(_startDate, _endDate);

                foreach(DataRow row in dsEliteConfirmationProgressReport.Tables[0].Rows)
                {
                    InsertEliteConfirmationProgressReportDataSheet(wbTemplate, wbReport, dsEliteConfirmationProgressReport, row);
                }

                #region TODO: Adapt to get campaign IDs from XamDatagrid when the requirement becomes applicable

                //foreach (DataRecord record in _campaigns)
                //{
                //    if ((bool)record.Cells["Select"].Value)
                //    {
                //        #region Get report data from database

                //        long campaignID = Convert.ToInt32(record.Cells["CampaignID"].Value);

                //        #endregion Get report data from database

                //        #region Add the summary sheet

                //        //summarySheetRowIndex = InsertAndUpdateSummarySheet(wbTemplate, wbReport, dsBumpUpPotentialReportData, summarySheetRowIndex);
                //        ////++summarySheetRowIndex;

                //        #endregion Add the summary sheet

                //        #region Add the data sheet for the current sheet

                //        InsertBumpUpPotentialReportDataSheet(wbTemplate, wbReport, dsBumpUpPotentialReportData, record);

                //        #endregion Add the data sheet for the current sheet
                //    }
                //}

                #endregion TODO: Adapt to get campaign IDs from XamDatagrid when the requirement becomes applicable

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


        private void InsertEliteConfirmationProgressReportDataSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsEliteConfirmationProgressReport, DataRow drCurrentCampaign /*DataRecord selectedCampaign*/)
        {

            string filterString = drCurrentCampaign["FilterString"].ToString();

            var filteredRows = dsEliteConfirmationProgressReport.Tables[1].Select(filterString).AsEnumerable();
            if (filteredRows.Any())
            {

                #region Partition the given dataset

                string orderByString = drCurrentCampaign["OrderByString"].ToString();
                DataTable dtCurrentCampaignData = dsEliteConfirmationProgressReport.Tables[1].Select(filterString, orderByString).CopyToDataTable();
                DataTable dtExcelSheetDataTableColumnMappings = dsEliteConfirmationProgressReport.Tables[2];

                #endregion Partition the given dataset

                #region Declarations & Initializations

                int reportRow = 7;

                byte templateDataSheetRowSpan = 6;
                byte templateColumnSpan = 10;
                byte templateRowIndex = 7;

                string reportTitle = drCurrentCampaign["ReportTitle"].ToString();
                string worksheetTabName = drCurrentCampaign["WorksheetTabName"].ToString();
                string campaignDataSheetTemplateName = "Base"; //selectedCampaign.Cells["CampaignCategory"].Value.ToString();

                string reportHeadingCell = "A1";        //dtReportConfigs.Rows[0]["ReportHeadingCell"].ToString();
                string reportSubHeadingCell = "A3";     //dtReportConfigs.Rows[0]["ReportSubHeadingCell"].ToString();
                string reportDateCell = "K5";           //dtReportConfigs.Rows[0]["ReportDateCell"].ToString();

                #endregion Declarations & Initializations

                #region Add the worksheet

                Worksheet wsReportTemplate = wbTemplate.Worksheets[campaignDataSheetTemplateName];
                Worksheet wsReport = wbReport.Worksheets.Add(worksheetTabName);
                Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                #endregion Add the worksheet

                #region Populating the report details

                Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
                wsReport.GetCell(reportHeadingCell).Value = reportTitle;

                if (_startDate == _endDate)
                {
                    wsReport.GetCell(reportSubHeadingCell).Value = String.Format(@"For all sales achieved on {0}", _startDate.ToString("dddd, d MMMM yyyy"));
                }
                else
                {
                    wsReport.GetCell(reportSubHeadingCell).Value = String.Format(@"For all sales achieved between {0} and {1}", _startDate.ToString("dddd, d MMMM yyyy"), _endDate.ToString("dddd, d MMMM yyyy"));
                }

                wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                #endregion Populating the report details

                #region Add the data

                reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentCampaignData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                #endregion Add the data
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
                //var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                //_campaigns = new System.Collections.Generic.List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

                btnClose.IsEnabled = false;
                btnReport.IsEnabled = false;
                //xdgCampaigns.IsEnabled = false;
                calStartDate.IsEnabled = false;
				calEndDate.IsEnabled = false;

                //Cal2.IsEnabled = false;

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

        #endregion

    }
}
