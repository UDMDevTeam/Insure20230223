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
using System.Collections.Generic;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportDiaryScreen.xaml
    /// </summary>
    public partial class ReportBumpUpPotentialScreen
    {

        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        //private RecordCollectionBase _campaigns;
        private List<Record> _campaigns;
        private string _campaignIDList = String.Empty;
        private DateTime _startDate;
        private DateTime _endDate;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        //private DataSet _ds;

        #endregion Private Members

        #region Constructors

        public ReportBumpUpPotentialScreen()
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

                DataTable dtLookups = Insure.INGetBumpUpPotentialReportLookupsAndConfigs().Tables[0];
                //_ds = Insure.INGetBumpUpPotentialReportLookupsAndConfigs();
                xdgCampaigns.DataSource = dtLookups.DefaultView; //_ds.Tables[0].DefaultView;
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

        private void InsertSummarySheet()
        {

        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup excel documents

                string filePathAndName = String.Format("{0}Bump-Up Potential ~ {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));

                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateBumpUpPotential.xlsx");
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup excel documents

                int summarySheetStartRowIndex = 8;
                int summarySheetRowIndex = summarySheetStartRowIndex;

                DataSet dsBumpUpPotentialReportData = new DataSet();

                DataSet dsBumpUpPotentialReportCampaignGroups = Insure.INGetBumpUpPotentialReportCampaignGroups(_campaignIDList);
                //DataSet dsCampaignGroups = new DataSet();
                //dsCampaignGroups.Tables.Add(dsBumpUpPotentialReportCampaignGroups.Tables[0]);
                //dsCampaignGroups.Tables.Add(dsBumpUpPotentialReportCampaignGroups.Tables[1]);
                //DataTable dtCampaignGroups = dsBumpUpPotentialReportCampaignGroups

                foreach (DataTable group in dsBumpUpPotentialReportCampaignGroups.Tables)
                {
                    if (group.Rows.Count <= 0)
                    {
                        continue;
                    }
                    DataRow groupRow = (DataRow)group.Rows[0];
                    if (!(groupRow.Table.Columns.Contains("GroupName")))
                    {
                        break;
                    }

                    #region InsertandUpdateSummarySheet

                    #region Partition the given dataset

                    //DataTable dtSummarySheetReportRow = dsBumpUpPotentialReportData.Tables[1];
                    DataTable dtDataSheetColumnCellMappings = dsBumpUpPotentialReportCampaignGroups.Tables[2];
                    DataTable dtSummarySheetColumnCellMappings = dsBumpUpPotentialReportCampaignGroups.Tables[3];
                    DataTable dtSummarySheetFormulaCellMappings = dsBumpUpPotentialReportCampaignGroups.Tables[4];
                    DataTable dtReportConfigs = dsBumpUpPotentialReportCampaignGroups.Tables[5];

                    #endregion Partition the given dataset

                    #region Get the report configs

                    //string summarySheetHeadingCell = dtReportConfigs.Rows[0]["SummarySheetHeadingCell"].ToString();
                    string summarySheetSubHeadingCell = dtReportConfigs.Rows[0]["SummarySheetSubHeadingCell"].ToString();
                    string summarySheetDateCell = dtReportConfigs.Rows[0]["SummarySheetDateCell"].ToString();

                    #endregion Get the report configs

                    #region Declarations & Initializations

                    byte templateSummaryDataSheetRowSpan = 8;
                    byte templateSummaryColumnSpan = 11;
                    byte templateSummaryRowIndex = 8;

                    #endregion Declarations & Initializations

                    #region Add the worksheet, if it does not exist yet

                    Worksheet wsSummaryTemplate = wbTemplate.Worksheets["Summary"];
                    Worksheet wsSummary;

                    if (!Methods.WorksheetExistsInWorkbook(wbReport, "Summary"))
                    {
                        wsSummary = wbReport.Worksheets.Add("Summary");
                        Methods.CopyWorksheetOptionsFromTemplate(wsSummaryTemplate, wsSummary, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                        #region Populating the report details

                        Methods.CopyExcelRegion(wsSummaryTemplate, 0, 0, templateSummaryDataSheetRowSpan, templateSummaryColumnSpan, wsSummary, 0, 0);

                        if (_startDate == _endDate)
                        {
                            wsSummary.GetCell(summarySheetSubHeadingCell).Value = String.Format(@"For all sales achieved on {0}", _startDate.ToString("dddd, d MMMM yyyy"));
                        }
                        else
                        {
                            wsSummary.GetCell(summarySheetSubHeadingCell).Value = String.Format(@"For all sales achieved between {0} and {1}", _startDate.ToString("dddd, d MMMM yyyy"), _endDate.ToString("dddd, d MMMM yyyy"));
                        }

                        wsSummary.GetCell(summarySheetDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        #endregion Populating the report details
                    }
                    else
                    {
                        wsSummary = wbReport.Worksheets["Summary"];
                    }

                    #endregion Add the worksheet, if it does not exist yet
                    #endregion InsertandUpdateSummarySheet


                    #region InsertBumpUpPotentialReportDataSheet

                    #region Declarations & Initializations

                    int reportRow = 9;

                    byte templateDataSheetRowSpan = 8;
                    byte templateColumnSpan = 20;
                    byte templateRowIndex = 9;

                    #endregion Declarations & Initializations

                    #region Get the report configs
                    string campaignName = groupRow["GroupName"].ToString();
                    //string campaignCode = groupRow["CampaignCode"].ToString();
                    string campaignDataSheetTemplateName = groupRow["GroupName"].ToString();
                    //string summarySheetTemplateName = selectedCampaign.Cells["SummarySheetTemplateName"].Value.ToString();

                    string reportHeadingCell = dtReportConfigs.Rows[0]["ReportHeadingCell"].ToString();
                    string reportSubHeadingCell = dtReportConfigs.Rows[0]["ReportSubHeadingCell"].ToString();
                    string reportDateCell = dtReportConfigs.Rows[0]["ReportDateCell"].ToString();

                    #endregion Get the report configs

                    #region Add the worksheet

                    //Worksheet wsReportTemplate = wbTemplate.Worksheets[campaignDataSheetTemplateName];
                    Worksheet wsReportTemplate = wbTemplate.Worksheets["Base"];
                    Worksheet wsReport = wbReport.Worksheets.Add(campaignDataSheetTemplateName);
                    Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                    #endregion Add the worksheet

                    #region Populating the report details

                    Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
                    wsReport.GetCell(reportHeadingCell).Value = String.Format("Bump-Up Potential Report - {0}", campaignName);

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

                    

                    #endregion InsertBumpUpPotentialReportDataSheet

                    foreach (DataRow record in group.Rows)
                    {
                            #region Get report data from database

                            

                            long campaignID = int.Parse(record["CampaignID"].ToString());
                            dsBumpUpPotentialReportData = Insure.INGetBumpUpPotentialReportData(campaignID, _startDate, _endDate);

                            #endregion Get report data from database

                            #region Add the summary sheet



                            #region Add the data
                            DataTable dtSummarySheetReportRow = dsBumpUpPotentialReportData.Tables[1];
                            summarySheetRowIndex = Methods.MapTemplatizedExcelValues(wsSummaryTemplate, dtSummarySheetReportRow, dtSummarySheetFormulaCellMappings, templateSummaryRowIndex, 0, 0, templateSummaryColumnSpan, wsSummary, summarySheetRowIndex, 0);

                            #endregion Add the data

                            //summarySheetRowIndex = InsertAndUpdateSummarySheet(wbTemplate, wbReport, dsBumpUpPotentialReportData, summarySheetRowIndex);
                            //++summarySheetRowIndex;

                            #endregion Add the summary sheet

                            #region Add the data sheet for the current sheet

                            #region Add the data
                            DataTable dtReportData = dsBumpUpPotentialReportData.Tables[0];
                            reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtReportData, dtDataSheetColumnCellMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                            #endregion Add the data

                            //InsertBumpUpPotentialReportDataSheet(wbTemplate, wbReport, dsBumpUpPotentialReportData, record);

                            #endregion Add the data sheet for the current sheet
                    }
                }
                

                //this foreach must change into 
                

                if (dsBumpUpPotentialReportData.Tables.Count > 0)
                {
                    AddSummarySheetTotals(wbTemplate, wbReport, dsBumpUpPotentialReportData, summarySheetRowIndex);
                }

                #region Save & Display the resulting workbook - if there are more than 1 worksheets

                if (wbReport.Worksheets.Count < 1)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"There is no data to export for the selected campaign(s) and/or specified date range.", "No Data", ShowMessageType.Information);
                    });
                }
                else
                {
                    //Save excel document
                    wbReport.Save(filePathAndName);

                    //Display excel document
                    Process.Start(filePathAndName);
                }

                #endregion Save & Display the resulting workbook - if there are more than 1 worksheets
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

        //private int InsertAndUpdateSummarySheet(Workbook wbTemplate, Workbook wbReport, DataSet dsBumpUpPotentialReportData, int reportRow)
        //{
        //    #region Exit from method if the 2nd table in the dataset does not contain any rows

        //    if (dsBumpUpPotentialReportData.Tables[1].Rows.Count == 0)
        //    {
        //        return reportRow;
        //    }

        //    #endregion Exit from method if the 2nd table in the dataset does not contain any rows

        //    #region Partition the given dataset

        //    DataTable dtSummarySheetReportRow = dsBumpUpPotentialReportData.Tables[1];
        //    DataTable dtSummarySheetColumnCellMappings = dsBumpUpPotentialReportData.Tables[4];
        //    DataTable dtReportConfigs = dsBumpUpPotentialReportData.Tables[5];

        //    #endregion Partition the given dataset

        //    #region Get the report configs

        //    //string summarySheetHeadingCell = dtReportConfigs.Rows[0]["SummarySheetHeadingCell"].ToString();
        //    string summarySheetSubHeadingCell = dtReportConfigs.Rows[0]["SummarySheetSubHeadingCell"].ToString();
        //    string summarySheetDateCell = dtReportConfigs.Rows[0]["SummarySheetDateCell"].ToString();

        //    #endregion Get the report configs

        //    #region Declarations & Initializations

        //    byte templateDataSheetRowSpan = 8;
        //    byte templateColumnSpan = 11;
        //    byte templateRowIndex = 8;

        //    #endregion Declarations & Initializations

        //    #region Add the worksheet, if it does not exist yet

        //    Worksheet wsSummaryTemplate = wbTemplate.Worksheets["Summary"];
        //    Worksheet wsSummary;

        //    if (! Methods.WorksheetExistsInWorkbook(wbReport, "Summary"))
        //    {
        //        wsSummary = wbReport.Worksheets.Add("Summary");
        //        Methods.CopyWorksheetOptionsFromTemplate(wsSummaryTemplate, wsSummary, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

        //        #region Populating the report details

        //        Methods.CopyExcelRegion(wsSummaryTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsSummary, 0, 0);

        //        if (_startDate == _endDate)
        //        {
        //            wsSummary.GetCell(summarySheetSubHeadingCell).Value = String.Format(@"For all sales achieved on {0}", _startDate.ToString("dddd, d MMMM yyyy"));
        //        }
        //        else
        //        {
        //            wsSummary.GetCell(summarySheetSubHeadingCell).Value = String.Format(@"For all sales achieved between {0} and {1}", _startDate.ToString("dddd, d MMMM yyyy"), _endDate.ToString("dddd, d MMMM yyyy"));
        //        }

        //        wsSummary.GetCell(summarySheetDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //        #endregion Populating the report details
        //    }
        //    else
        //    {
        //        wsSummary = wbReport.Worksheets["Summary"];
        //    }

        //    #endregion Add the worksheet, if it does not exist yet

        //    #region Add the data

        //    reportRow = Methods.MapTemplatizedExcelValues(wsSummaryTemplate, dtSummarySheetReportRow, dtSummarySheetColumnCellMappings, templateRowIndex, 0, 0, templateColumnSpan, wsSummary, reportRow, 0);

        //    #endregion Add the data

        //    return reportRow;
        //}

        private void InsertBumpUpPotentialReportDataSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsBumpUpPotentialReportData, DataRow selectedCampaign)
        {
            #region Exit from method if the 1st table in the dataset does not contain any rows

            if (dsBumpUpPotentialReportData.Tables[0].Rows.Count == 0)
            {
                return;
            }

            #endregion Exit from method if the 1st table in the dataset does not contain any rows

            #region Partition the given dataset

            DataTable dtReportData = dsBumpUpPotentialReportData.Tables[0];
            DataTable dtSummarySheetReportRow = dsBumpUpPotentialReportData.Tables[1];
            DataTable dtDataSheetColumnCellMappings = dsBumpUpPotentialReportData.Tables[2];
            DataTable dtSummarySheetColumnCellMappings = dsBumpUpPotentialReportData.Tables[3];
            DataTable dtSummarySheetFormulaCellMappings = dsBumpUpPotentialReportData.Tables[4];
            DataTable dtReportConfigs = dsBumpUpPotentialReportData.Tables[5];

            #endregion Partition the given dataset

            #region Declarations & Initializations

            int reportRow = 9;

            byte templateDataSheetRowSpan = 8;
            byte templateColumnSpan = 20;
            byte templateRowIndex = 9;

            #endregion Declarations & Initializations

            #region Get the report configs

            string campaignName = selectedCampaign["CampaignName"].ToString();
            string campaignCode = selectedCampaign["CampaignCode"].ToString();
            string campaignDataSheetTemplateName = selectedCampaign["GroupName"].ToString();
            //string summarySheetTemplateName = selectedCampaign.Cells["SummarySheetTemplateName"].Value.ToString();

            string reportHeadingCell = dtReportConfigs.Rows[0]["ReportHeadingCell"].ToString();
            string reportSubHeadingCell = dtReportConfigs.Rows[0]["ReportSubHeadingCell"].ToString();
            string reportDateCell = dtReportConfigs.Rows[0]["ReportDateCell"].ToString();

            #endregion Get the report configs

            #region Add the worksheet

            //Worksheet wsReportTemplate = wbTemplate.Worksheets[campaignDataSheetTemplateName];
            Worksheet wsReportTemplate = wbTemplate.Worksheets["Base"];
            Worksheet wsReport = wbReport.Worksheets.Add(campaignDataSheetTemplateName);
            Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

            #endregion Add the worksheet

            #region Populating the report details

            Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
            wsReport.GetCell(reportHeadingCell).Value = String.Format("Bump-Up Potential Report - {0}", campaignName);
 
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

            reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtReportData, dtDataSheetColumnCellMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

            #endregion Add the data
        }

        private void AddSummarySheetTotals(Workbook wbTemplate, Workbook wbReport, DataSet dsBumpUpPotentialReportLookups, int reportRow)
        {
            Worksheet wsSummaryTemplate = wbTemplate.Worksheets["Summary"];
            Worksheet wsSummary;

            if (Methods.WorksheetExistsInWorkbook(wbReport, "Summary"))
            {
                wsSummary = wbReport.Worksheets["Summary"];

                byte formulaStartRow = 8;
                byte totalsTemplateRowIndex = 9;
                byte templateColumnSpan = 11;

                DataTable dtExcelFormulaMappings = dsBumpUpPotentialReportLookups.Tables[3];

                #region Totals & Averages

                reportRow = Methods.MapTemplatizedExcelFormulas(wsSummaryTemplate, dtExcelFormulaMappings, totalsTemplateRowIndex, 0, 0, templateColumnSpan, wsSummary, reportRow, 0, formulaStartRow, reportRow - 1);

                #endregion Totals & Averages
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

        private bool IsAllInputParametersSpecifiedAndValid()
        {
            #region Ensuring that at least one campaign was selected

            var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            _campaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

            if (_campaigns.Count == 0)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No campaigns selected", ShowMessageType.Error);
                return false;
            }
            else
            {
                _campaignIDList = _campaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                _campaignIDList = _campaignIDList.Substring(0, _campaignIDList.Length - 1);
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
