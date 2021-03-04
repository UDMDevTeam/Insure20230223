using System.Linq;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Resources;
using System.Windows.Threading;
using Infragistics.Windows.Editors.Events;
using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using UDM.Insurance.Business;
using System.Windows.Media;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UDM.Insurance.Business.Mapping;
using Infragistics.Windows.Editors;
using Infragistics.Windows.DataPresenter;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportLeadAnalysisScreen
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

        #endregion INotifyPropertyChanged implementation

        #region Private Members

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();

        private int _timer1;

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        //private DataRow _campaign;
        //private long _campaignID;

        //private Infragistics.Windows.DataPresenter.RecordCollectionBase _batches;
        //private List<Infragistics.Windows.DataPresenter.Record> _lstSelectedAgents;

        private List<Record> _lstSelectedCampaigns;
        private string _campaignIDList = String.Empty;

        private DateTime _reportStartDate = DateTime.Now;
        private DateTime _reportEndDate = DateTime.Now;

        private string _strTodaysDate;
        private string _strTodaysDateIncludingColons;

        //private LeadAnalysisReportMode _reportMode;

        //private bool _excludeLeadsWithNoFeedback;

        #endregion Private Members

        #region Constructors

        public ReportLeadAnalysisScreen()
        {
            InitializeComponent();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);

            LoadLookupData();
        }

        #endregion Constructors

        #region Private Methods

        #region Timer Methods

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;

            btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnReport.ToolTip = btnReport.Content;
        }

        #endregion Timer Methods

        #region Delegates


        private void AddLeadAnalysisReportSheetForCampaign(Workbook wbResultingWorkbook, Workbook wbTemplate, string reportTemplateSheetName, long campaignID, string campaignName, string campaignCode, DateTime fromDate, DateTime toDate)
        {

            #region Get the data for the report from the database

            DataSet dsLeadAnalysisReportData = Insure.INGetLeadAnalysisReportDataByCampaign(campaignID, fromDate, toDate);
            DataTable dtLeadAnalysisReportData = dsLeadAnalysisReportData.Tables[0];

            if (dtLeadAnalysisReportData.Rows.Count == 0)
            {
                return;
            }

            #endregion Get the data for the report from the database

            #region Declaring & initializing variables

            int leadFeedbackDetailsColumnSpan = 10;
            int leadDetailsColumnSpan = dtLeadAnalysisReportData.Columns.Count - leadFeedbackDetailsColumnSpan;

            int rowIndex = 0;

            string reportPageSubHeading = String.Empty;

            Worksheet wsNewWorksheetTemplate = wbTemplate.Worksheets[reportTemplateSheetName];
            Worksheet wsLeadAnalysisReport = wbResultingWorkbook.Worksheets.Add(campaignCode);
            WorksheetMergedCellsRegion mergedRegion;

            //Worksheet wsLeadAnalysisReport = wbResultingWorkbook.Worksheets.Add("01234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ");

            #endregion Declaring & initializing variables

            wsLeadAnalysisReport.PrintOptions.PaperSize = PaperSize.A4;
            wsLeadAnalysisReport.PrintOptions.Orientation = Infragistics.Documents.Excel.Orientation.Landscape;

            #region Step 1: Insert the report heading

            mergedRegion = wsLeadAnalysisReport.MergedCellsRegions.Add(0, 0, 0, leadDetailsColumnSpan + leadFeedbackDetailsColumnSpan - 1);
            mergedRegion.CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;
            mergedRegion.CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
            mergedRegion.CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
            mergedRegion.CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;

            mergedRegion.CellFormat.Alignment = HorizontalCellAlignment.Center;
            mergedRegion.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            mergedRegion.CellFormat.Font.Height = 360;
            mergedRegion.Value = "Lead Analysis Report";

            rowIndex += 2;

            #endregion Step 1: Insert the report heading

            #region Step 2: Add the report details - specifically which campaign this report is being generated for

            wsLeadAnalysisReport.GetCell("A3").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            wsLeadAnalysisReport.GetCell("A3").CellFormat.Font.Height = 240;
            wsLeadAnalysisReport.GetCell("A3").Value = "Campaign:";

            wsLeadAnalysisReport.GetCell("B3").Value = campaignName; // _campaign.ItemArray[3].ToString(); //_inCampaign.Name; 

            rowIndex += 2;

            #endregion Step 2: Add the report details - specifically which campaign this report is being generated for

            #region Step 3: Insert the merged cell region indicating the leads section

            mergedRegion = wsLeadAnalysisReport.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, leadDetailsColumnSpan - 1);
            mergedRegion.CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;
            mergedRegion.CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
            mergedRegion.CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
            mergedRegion.CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;

            mergedRegion.CellFormat.Alignment = HorizontalCellAlignment.Center;
            mergedRegion.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            mergedRegion.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            mergedRegion.CellFormat.Font.Height = 240;
            mergedRegion.Value = "Information As Per Original Lead";

            rowIndex += 2;

            #endregion Step 3: Insert the merged cell region indicating the leads section

            #region Step 4: Copy all the leads from the selected import file and apply formatting from the template

            InsertTemplatizedLeadsFromDataTable(rowIndex, 0, leadDetailsColumnSpan, wsNewWorksheetTemplate, wsLeadAnalysisReport, dsLeadAnalysisReportData);

            #endregion Step 4: Copy all the leads from the selected import file and apply formatting from the template

            #region Step 5: Add the Lead Feedback Details

            InsertLeadFeedbackDetails(rowIndex - 2, dtLeadAnalysisReportData.Columns.Count - leadFeedbackDetailsColumnSpan, leadFeedbackDetailsColumnSpan, wsNewWorksheetTemplate, wsLeadAnalysisReport, dsLeadAnalysisReportData.Tables[0]);

            #endregion Step 5: Add the Lead Feedback Details
        }
  
        private void AddLeadAnalysisReportSheetForBatch(Workbook wbResultingWorkbook, Workbook wbTemplate, string reportTemplateSheetName, string newWorksheetDescription, long batchID, DateTime fromDate, DateTime toDate)
        {
            
            #region Get the data for the report from the database

            DataSet dsLeadAnalysisReportData = Insure.INGetLeadAnalysisReportData(batchID, fromDate, toDate);
            DataTable dtLeadAnalysisReportData = dsLeadAnalysisReportData.Tables[0];

            if (dtLeadAnalysisReportData.Rows.Count == 0)
            {
                return;
            }

            #endregion Get the data for the report from the database

            #region Declaring & initializing variables

            int leadFeedbackDetailsColumnSpan = 10;
            int leadDetailsColumnSpan = dtLeadAnalysisReportData.Columns.Count - leadFeedbackDetailsColumnSpan;

            int rowIndex = 0;

            string reportPageSubHeading = String.Empty;

            Worksheet wsNewWorksheetTemplate = wbTemplate.Worksheets[reportTemplateSheetName];
            Worksheet wsLeadAnalysisReport = wbResultingWorkbook.Worksheets.Add(newWorksheetDescription);
            WorksheetMergedCellsRegion mergedRegion;

            //Worksheet wsLeadAnalysisReport = wbResultingWorkbook.Worksheets.Add("01234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ");

            #endregion Declaring & initializing variables

            wsLeadAnalysisReport.PrintOptions.PaperSize = PaperSize.A4;
            wsLeadAnalysisReport.PrintOptions.Orientation = Infragistics.Documents.Excel.Orientation.Landscape;

            #region Step 1: Insert the report heading

            mergedRegion = wsLeadAnalysisReport.MergedCellsRegions.Add(0, 0, 0, leadDetailsColumnSpan + leadFeedbackDetailsColumnSpan - 1);
            mergedRegion.CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;
            mergedRegion.CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
            mergedRegion.CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
            mergedRegion.CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;

            mergedRegion.CellFormat.Alignment = HorizontalCellAlignment.Center;
            mergedRegion.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            mergedRegion.CellFormat.Font.Height = 360;
            mergedRegion.Value = "Lead Analysis Report";

            rowIndex += 2;

            #endregion Step 1: Insert the report heading

            #region Step 2: Add the report details - specifically which campaign this report is being generated for

            wsLeadAnalysisReport.GetCell("A3").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            wsLeadAnalysisReport.GetCell("A3").CellFormat.Font.Height = 240;
            wsLeadAnalysisReport.GetCell("A3").Value = "Campaign:";

            //wsLeadAnalysisReport.GetCell("B3").Value = _campaign.ItemArray[3].ToString(); //_inCampaign.Name; 

            rowIndex += 2;

            #endregion Step 2: Add the report details - specifically which campaign this report is being generated for

            #region Step 3: Insert the merged cell region indicating the leads section

            mergedRegion = wsLeadAnalysisReport.MergedCellsRegions.Add(rowIndex, 0, rowIndex + 1, leadDetailsColumnSpan - 1);
            mergedRegion.CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;
            mergedRegion.CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
            mergedRegion.CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
            mergedRegion.CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;

            mergedRegion.CellFormat.Alignment = HorizontalCellAlignment.Center;
            mergedRegion.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            mergedRegion.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            mergedRegion.CellFormat.Font.Height = 240;
            mergedRegion.Value = "Information As Per Original Lead";

            rowIndex += 2;

            #endregion Step 3: Insert the merged cell region indicating the leads section

            #region Step 4: Copy all the leads from the selected import file and apply formatting from the template

            InsertTemplatizedLeadsFromDataTable(rowIndex, 0, leadDetailsColumnSpan, wsNewWorksheetTemplate, wsLeadAnalysisReport, dsLeadAnalysisReportData);

            #endregion Step 4: Copy all the leads from the selected import file and apply formatting from the template

            #region Step 5: Add the Lead Feedback Details

            InsertLeadFeedbackDetails(rowIndex - 2, dtLeadAnalysisReportData.Columns.Count - leadFeedbackDetailsColumnSpan, leadFeedbackDetailsColumnSpan, wsNewWorksheetTemplate, wsLeadAnalysisReport, dsLeadAnalysisReportData.Tables[0]);

            #endregion Step 5: Add the Lead Feedback Details

        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Defining variables

                //long campaignID = Convert.ToInt32(_campaign.ItemArray[0]);
                //string campaignName = _campaign.ItemArray[3].ToString();
                
                #endregion Defining variables

                #region Setup Excel document

                var wbLeadAnalysisReport = new Workbook();
                wbLeadAnalysisReport.SetCurrentFormat(WorkbookFormat.Excel2007);

                string filePathAndName = String.Format("{0}Lead Analysis Report ~ {1}.xlsx",
                    GlobalSettings.UserFolder,
                    DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                Workbook wbTemplate;
                Uri uri = new Uri("/Templates/ReportTemplateLeadAnalysis.xlsx", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    wbTemplate = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }

                //Worksheet wsTemplate = wbTemplate.Worksheets[0];

                #endregion Setup Excel document

                #region Report Operations

                #region Generating the report by batch
                //// Loop through each selected batch
                //foreach (Infragistics.Windows.DataPresenter.DataRecord record in _lstSelectedAgents)
                //{

                //    long batchID = Convert.ToInt64(record.Cells["ID"].Value);
                //    string batchDescription = Methods.ParseWorksheetName(wbLeadAnalysisReport, record.Cells["Code"].Value.ToString());

                //    AddLeadAnalysisReportSheetForBatch(wbLeadAnalysisReport, wbTemplate, "Lead Analysis", batchDescription, batchID, _reportStartDate, _reportEndDate);
                //    worksheetCount++;
                //}

                #endregion Generating the report by batch

                #region Generating the report by campaign
                // Loop through each selected campaign
                foreach (Infragistics.Windows.DataPresenter.DataRecord record in _lstSelectedCampaigns)
                {
                    long campaignID = Convert.ToInt64(record.Cells["CampaignID"].Value);
                    string campaignName = record.Cells["CampaignName"].Value.ToString();
                    string campaignCode = record.Cells["CampaignCode"].Value.ToString();
                    //string campaignDescription = Methods.ParseWorksheetName(wbLeadAnalysisReport, record.Cells["CampaignCode"].Value.ToString());

                    AddLeadAnalysisReportSheetForCampaign(wbLeadAnalysisReport, wbTemplate, "Lead Analysis", campaignID, campaignName, campaignCode, _reportStartDate, _reportEndDate);
                }

                #endregion Generating the report by batch

                #endregion Report Operations

                #region Finally, save, and display the resulting workbook

                if (wbLeadAnalysisReport.Worksheets.Count > 0)
                {
                    wbLeadAnalysisReport.Save(filePathAndName);

                    //Display excel document
                    Process.Start(filePathAndName);

                }
                else
                {
                    //emptyDataTableCount++;
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "There is no data available for the criteria you have specified. Please change some of them and try again.", "No data available", ShowMessageType.Information);
                    });
                }
                #endregion Finally, save, and display the resulting workbook
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
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";

            EnableAllControls(true);
        }

        #endregion Delegates

        // Will most likely not be used again
        //private void SetReportMode(LeadAnalysisReportMode reportMode)
        //{
        //    switch (reportMode)
        //    {
        //        case LeadAnalysisReportMode.Default:
        //            lblCampaigns.Visibility = Visibility.Hidden;
        //            cmbCampaign.Visibility = Visibility.Hidden;

        //            lblBatches.Visibility = Visibility.Hidden;
        //            xdgBatches.Visibility = Visibility.Hidden;

        //            vbFromDate.Visibility = Visibility.Hidden;
        //            tbFromDate.Visibility = Visibility.Hidden;
        //            //calFromDate.Visibility = Visibility.Hidden;


        //            vbToDate.Visibility = Visibility.Hidden;
        //            tbToDate.Visibility = Visibility.Hidden;
        //            //calToDate.Visibility = Visibility.Hidden;

        //            lblReportResultsToInclude.Visibility = Visibility.Hidden;
        //            cmbReportResultsToInclude.Visibility = Visibility.Hidden;

        //            btnReport.Visibility = Visibility.Hidden;

        //            break;

        //        case LeadAnalysisReportMode.ByDateRange:
        //            lblCampaigns.Visibility = Visibility.Hidden;
        //            cmbCampaign.Visibility = Visibility.Hidden;

        //            lblBatches.Visibility = Visibility.Hidden;
        //            xdgBatches.Visibility = Visibility.Hidden;

        //            tbFromDate.Visibility = Visibility.Visible;
        //            Grid.SetRow(tbFromDate, 2);
        //            Grid.SetColumn(tbFromDate, 13);

        //            vbFromDate.Visibility = Visibility.Visible;
        //            Grid.SetRow(vbFromDate, 3);
        //            Grid.SetColumn(vbFromDate, 13);

        //            tbToDate.Visibility = Visibility.Visible;
        //            Grid.SetRow(tbToDate, 2);
        //            Grid.SetColumn(tbToDate, 20);

        //            vbToDate.Visibility = Visibility.Visible;
        //            Grid.SetRow(vbToDate, 3);
        //            Grid.SetColumn(vbToDate, 20);

        //            lblReportResultsToInclude.Visibility = Visibility.Visible;
        //            Grid.SetRow(lblReportResultsToInclude, 4);
        //            Grid.SetColumn(lblReportResultsToInclude, 2);

        //            cmbReportResultsToInclude.Visibility = Visibility.Visible;
        //            Grid.SetRow(cmbReportResultsToInclude, 5);
        //            Grid.SetColumn(cmbReportResultsToInclude, 2);
        //            Grid.SetColumnSpan(cmbReportResultsToInclude, 10);

        //            btnReport.Visibility = Visibility.Visible;
        //            break;

        //        case LeadAnalysisReportMode.ByCampaignAndDateRange:
        //            lblCampaigns.Visibility = Visibility.Visible;
        //            cmbCampaign.Visibility = Visibility.Visible;

        //            lblBatches.Visibility = Visibility.Hidden;
        //            xdgBatches.Visibility = Visibility.Hidden;

        //            tbFromDate.Visibility = Visibility.Visible;
        //            Grid.SetRow(tbFromDate, 6);
        //            Grid.SetColumn(tbFromDate, 13);

        //            vbFromDate.Visibility = Visibility.Visible;
        //            Grid.SetRow(vbFromDate, 7);
        //            Grid.SetColumn(vbFromDate, 13);

        //            tbToDate.Visibility = Visibility.Visible;
        //            Grid.SetRow(tbToDate, 6);
        //            Grid.SetColumn(tbToDate, 20);

        //            vbToDate.Visibility = Visibility.Visible;
        //            Grid.SetRow(vbToDate, 7);
        //            Grid.SetColumn(vbToDate, 20);

        //            lblReportResultsToInclude.Visibility = Visibility.Visible;
        //            Grid.SetRow(lblReportResultsToInclude, 6);
        //            Grid.SetColumn(lblReportResultsToInclude, 2);

        //            cmbReportResultsToInclude.Visibility = Visibility.Visible;
        //            Grid.SetRow(cmbReportResultsToInclude, 7);
        //            Grid.SetColumn(cmbReportResultsToInclude, 2);
        //            Grid.SetColumnSpan(cmbReportResultsToInclude, 10);

        //            btnReport.Visibility = Visibility.Visible;
        //            break;

        //        case LeadAnalysisReportMode.ByCampaignAndBatches:
        //            lblCampaigns.Visibility = Visibility.Visible;
        //            cmbCampaign.Visibility = Visibility.Visible;

        //            lblBatches.Visibility = Visibility.Visible;
        //            xdgBatches.Visibility = Visibility.Visible;

        //            tbFromDate.Visibility = Visibility.Hidden;
        //            Grid.SetRow(tbFromDate, 8);
        //            Grid.SetColumn(tbFromDate, 13);

        //            vbFromDate.Visibility = Visibility.Hidden;
        //            Grid.SetRow(vbFromDate, 9);
        //            Grid.SetColumn(vbFromDate, 13);

        //            tbToDate.Visibility = Visibility.Hidden;
        //            Grid.SetRow(tbToDate, 8);
        //            Grid.SetColumn(tbToDate, 20);

        //            vbToDate.Visibility = Visibility.Hidden;
        //            Grid.SetRow(vbToDate, 9);
        //            Grid.SetColumn(vbToDate, 20);

        //            lblReportResultsToInclude.Visibility = Visibility.Visible;
        //            Grid.SetRow(lblReportResultsToInclude, 6);
        //            Grid.SetColumn(lblReportResultsToInclude, 13);

        //            cmbReportResultsToInclude.Visibility = Visibility.Visible;
        //            Grid.SetRow(cmbReportResultsToInclude, 7);
        //            Grid.SetColumn(cmbReportResultsToInclude, 13);
        //            Grid.SetColumnSpan(cmbReportResultsToInclude, 6);

        //            btnReport.Visibility = Visibility.Visible;
        //            break;

        //        default:
        //            lblCampaigns.Visibility = Visibility.Hidden;
        //            cmbCampaign.Visibility = Visibility.Hidden;

        //            lblBatches.Visibility = Visibility.Hidden;
        //            xdgBatches.Visibility = Visibility.Hidden;

        //            vbFromDate.Visibility = Visibility.Hidden;
        //            tbFromDate.Visibility = Visibility.Hidden;
        //            calFromDate.Visibility = Visibility.Hidden;


        //            vbToDate.Visibility = Visibility.Hidden;
        //            tbToDate.Visibility = Visibility.Hidden;
        //            calToDate.Visibility = Visibility.Hidden;

        //            lblReportResultsToInclude.Visibility = Visibility.Hidden;
        //            cmbReportResultsToInclude.Visibility = Visibility.Hidden;

        //            btnReport.Visibility = Visibility.Hidden;

        //            break;
        //    }
        //}

        //private void ShowOrHideControls

        private void LoadLookupData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataSet dsLeadAnalysisReportLookups = Insure.GetLeadAnalysisReportLookups();

                CommonControlData.PopulateCampaignDataGrid(xdgCampaigns, dsLeadAnalysisReportLookups.Tables[0]);

                //DataTable dtCampaigns = dsLeadAnalysisReportLookups.Tables[0];
                //cmbCampaign.Populate(dtCampaigns, "Name", "ID");

                //DataTable dtLeadAnalysisReportModes = dsLeadAnalysisReportLookups.Tables[1];
                //cmbLeadAnalysisReportMode.Populate(dtLeadAnalysisReportModes, "Description", "ID");

                //DataTable dtLeadAnalysisReportResultsToInclude = dsLeadAnalysisReportLookups.Tables[1];
                //cmbReportResultsToInclude.Populate(dtLeadAnalysisReportResultsToInclude, "Description", "ID");

                //DataTable dtCampaigns = INCampaignMapper.ListData(false, null).Tables[0];
                //dtCampaigns.DefaultView.Sort = "Name";
                //cmbCampaign.Populate(dtCampaigns, "Name", "ID");

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

            if (calFromDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'From Date'.", @"No 'From Date' specified", ShowMessageType.Error);
                return false;
            }
            else
            {
                _reportStartDate = calFromDate.SelectedDate.Value;
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the From Date was specified

            if (calToDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'To Date'.", @"No 'To Date' specified", ShowMessageType.Error);
                return false;
            }

            else
            {
                _reportEndDate = calToDate.SelectedDate.Value;
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the date range is valid

            if (_reportStartDate > _reportEndDate)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Invalid date range specified: The 'From Date' can not be greater than the 'To Date'.", "Invalid date range", ShowMessageType.Error);
                return false;
            }

            #endregion Ensuring that the date range is valid

            #region Ensuring that the user indicated which leads should be in the report

            //long? reportDataSelectionID = cmbReportResultsToInclude.SelectedValue as long?;

            ////if (cmbReportResultsToInclude.SelectedIndex == -1)
            //if (reportDataSelectionID == null)
            //{
            //    ShowMessageBox(new INMessageBoxWindow1(), "Please indicate whether or not you only want to generate this report for lead that have feedback.", "Include leads without feedback", ShowMessageType.Error);
            //    return false;
            //}
            //else
            //{
            //    // if "All leads" was selected from the drop-down
            //    if (reportDataSelectionID.Value == 1)
            //    {
            //        _excludeLeadsWithNoFeedback = false;
            //    }
            //    // if "Only leads that have feedback" was selected from the drop-down
            //    else
            //    {
            //        _excludeLeadsWithNoFeedback = true;
            //    }
            //}

            #endregion Ensuring that the user indicated which leads should be in the report

            // Otherwise, if all is well, proceed:
            return true;
        }

        private void EnableAllControls(bool isEnabled)
        {
            btnClose.IsEnabled = isEnabled;
            //cmbCampaign.IsEnabled = isEnabled;
            //xdgBatches.IsEnabled = isEnabled;
            xdgCampaigns.IsEnabled = isEnabled;
            calFromDate.IsEnabled = isEnabled;
            calToDate.IsEnabled = isEnabled;
            btnReport.IsEnabled = isEnabled;
        }

        private int AddColumnHeadingsFromDataTable(int row, int column, int columnCount, Worksheet templateWorksheet, Worksheet destinationWorksheet, DataSet dsSourceData)
        {
            int mergedCellRegionRowSpan = 0;
            //[0]	{'Black Cancer'!$M$6:$M$8}
            //[1]	{'Black Cancer'!$C$6:$C$8}
            //[2]	{'Black Cancer'!$B$6:$B$8}
            //[3]	{'Black Cancer'!$A$4:$C$5}
            //[4]	{'Black Cancer'!$H$6:$H$8}
            //[5]	{'Black Cancer'!$I$6:$I$8}
            //[6]	{'Black Cancer'!$J$6:$J$8}
            //[7]	{'Black Cancer'!$K$6:$K$8}
            //[8]	{'Black Cancer'!$L$6:$L$8}
            //[9]	{'Black Cancer'!$A$6:$A$8}
            //[10]	{'Black Cancer'!$D$6:$D$8}
            //[11]	{'Black Cancer'!$E$6:$E$8}
            //[12]	{'Black Cancer'!$F$6:$F$8}
            //[13]	{'Black Cancer'!$G$6:$G$8}
            //[14]	{'Black Cancer'!$D$4:$D$5}
            //[15]	{'Black Cancer'!$E$4:$M$5}

            WorksheetMergedCellsRegion templateMergedCellsRegion;
            WorksheetMergedCellsRegion destinationMergedCellsRegion;

            DataTable dtData = dsSourceData.Tables[0];
            DataTable dtColumnWidths = dsSourceData.Tables[1];

            int indexOfTemplateMergedCellsRegion = 0;

            for (int c = 0; c < columnCount; c++)
            {

                #region Column 1

                if (c == 0)
                {
                    indexOfTemplateMergedCellsRegion = 9;
                }

                #endregion Column 1

                #region Column 2 to (N - 1)

                else if ((c > 0) && (c < columnCount - 1))
                {
                    indexOfTemplateMergedCellsRegion = 2;
                }

                #endregion Column 2 to (N - 1)

                #region Column N

                else if (c < columnCount)
                {
                    indexOfTemplateMergedCellsRegion = 1;
                }

                #endregion Column N

                #region Add the merged cell region, apply the formatting from the template and add the column heading

                templateMergedCellsRegion = templateWorksheet.MergedCellsRegions[indexOfTemplateMergedCellsRegion];
                mergedCellRegionRowSpan = Math.Abs(templateMergedCellsRegion.LastRow - templateMergedCellsRegion.FirstRow);
                destinationMergedCellsRegion = destinationWorksheet.MergedCellsRegions.Add(
                    row,
                    c + column,
                    row + mergedCellRegionRowSpan,
                    c + column);

                Methods.ApplyMergedCellsRegionFormattingFromTemplate(templateMergedCellsRegion, destinationMergedCellsRegion);

                destinationMergedCellsRegion.Value = dtData.Columns[c].ColumnName;

                #endregion Add the merged cell region, apply the formatting from the template and add the column heading

                #region Set the column widths

                destinationWorksheet.Columns[column + c].Width = Convert.ToInt32(dtColumnWidths.Rows[0][c]) * 2;

                #endregion Set the column widths
            }

            return mergedCellRegionRowSpan;
        }

        private void AddTemplatizedValuesFromDataTable(int startRow, int startColumn, int columnCount, Worksheet templateWorksheet, Worksheet destinationWorksheet, DataTable dtSourceData)
        {
            if ((dtSourceData.Rows.Count == 0) || (columnCount == 0))
            {
                return;
            }

            WorksheetCell destinationCell;
            WorksheetCell templateCell;
            string excelCellAddress = String.Empty;

            for (int r = 0; r < dtSourceData.Rows.Count; r++)
            {
                for (int c = 0; c < columnCount; c++)
                {
                    // Define the cell to be formatted:
                    destinationCell = destinationWorksheet.Rows[startRow + r].Cells[startColumn + c];

                    #region Row 1

                    if (r == 0)
                    {
                        #region Column 1

                        if (c == 0)
                        {
                            excelCellAddress = "A9";
                            //templateCell = templateWorksheet.GetCell("A9");
                            //Methods.ApplyCellFormattingFromTemplate(templateCell, destinationCell);
                        }

                        #endregion Column 1

                        #region Column 2 to (N - 1)

                        else if ((c > 0) && (c < columnCount - 1))
                        {
                            excelCellAddress = "B9";
                        }

                        #endregion Column 2 to (N - 1)

                        #region Column N

                        else if (c < columnCount)
                        {
                            excelCellAddress = "C9";
                        }

                        #endregion Column N
                    }

                    #endregion Row 1
                        
                    #region Rows 2 to (N - 1)

                    else if ((r > 0) && (r < dtSourceData.Rows.Count - 1))
                    {
                        #region Column 1

                        if (c == 0)
                        {
                            excelCellAddress = "A10";
                        }

                        #endregion Column 1

                        #region Column 2 to (N - 1)

                        else if ((c > 0) && (c < columnCount - 1))
                        {
                            excelCellAddress = "B10";
                        }

                        #endregion Column 2 to (N - 1)

                        #region Column N

                        else if (c < columnCount)
                        {
                            excelCellAddress = "C10";
                        }

                        #endregion Column N
                    }

                    #endregion Row 2 to (N - 1)

                    #region Row N

                    else if (r < dtSourceData.Rows.Count)
                    {
                        #region Column 1

                        if (c == 0)
                        {
                            excelCellAddress = "A11";
                        }

                        #endregion Column 1

                        #region Column 2 to (N - 1)

                        else if ((c > 0) && (c < columnCount - 1))
                        {
                            excelCellAddress = "B11";
                        }

                        #endregion Column 2 to (N - 1)

                        #region Column N

                        else if (c < columnCount)
                        {
                            excelCellAddress = "C11";
                        }

                        #endregion Column N
                    }

                    #endregion Row N

                    #region Finally, apply the formatting

                    templateCell = templateWorksheet.GetCell(excelCellAddress);
                    Methods.ApplyCellFormattingFromTemplate(templateCell, destinationCell);

                    #endregion Finally, apply the formatting

                    destinationCell.Value = dtSourceData.Rows[r][c];
                }
            }
        }

        private void InsertTemplatizedLeadsFromDataTable(int row, int column, int columnCount, Worksheet templateWorksheet, Worksheet destinationWorksheet, DataSet dsSource)
        {
            int currentRow = row;

            #region Step 2: Add the column headings with their formatting

            int rowsToAdd = AddColumnHeadingsFromDataTable(row, 0, columnCount, templateWorksheet, destinationWorksheet, dsSource);
            currentRow += rowsToAdd + 1;

            #endregion Step 2: Set the formatting options for the column headings

            #region Step 3: Copy the contents

            //Methods.CopyExcelRegion(columnHeadingsWorksheet, headingsRowIndex + 1, 0, rowCount, columnCount, destinationWorksheet, currentRow, 0);

            #endregion Step 3: Copy the contents

            #region Step 4: Set the formatting options for each cell and add the corresponding value from the data table

            //AddTemplatizedValuesFromDataTable(currentRow, 0, rowCount, columnCount, templateWorksheet, destinationWorksheet, dtSource);
            DataTable dtData = dsSource.Tables[0];
            AddTemplatizedValuesFromDataTable(currentRow, 0, columnCount, templateWorksheet, destinationWorksheet, dtData);


            #endregion Step 4: Set the formatting options for each cell and add the corresponding value from the data table
        }

        private void InsertLeadFeedbackDetails(int row, int column, int templateColumnCount, Worksheet templateWorksheet, Worksheet destinationWorksheet, DataTable dtSource)
        {
            int currentRow = row;

            #region Step 1: Add the column headings

            Methods.CopyExcelRegion(templateWorksheet, 3, 3, 4, templateColumnCount, destinationWorksheet, currentRow, column);
            currentRow += 5;

            #endregion Step 1: Add the column headings

            #region Step 2: For each row, get the data from the database and insert it

            WorksheetCell cell;

            if (dtSource.Rows.Count > 0)
            {
                for (int r = 0; r < dtSource.Rows.Count; r++)
                {
                    #region Copy template formatting

                    int templateRow = 9; // default

                    #region Row 1

                    if (r == 0)
                    {
                        templateRow = 8;
                    }

                    #endregion Row 1

                    #region Rows 2 to (N - 1)

                    else if ((r > 0) && (r < dtSource.Rows.Count - 1))
                    {
                        templateRow = 9;
                    }

                    #endregion Row 2 to (N - 1)

                    #region Row N

                    else if (r < dtSource.Rows.Count)
                    {
                        templateRow = 10;
                    }

                    #endregion Row N

                    Methods.CopyExcelRegion(templateWorksheet, templateRow, 3, 0, templateColumnCount, destinationWorksheet, currentRow, column);

                    #endregion Copy template formatting

                    for (int c = column; c < dtSource.Rows[0].ItemArray.Length; c++)
                    {
                        cell = destinationWorksheet.Rows[currentRow].Cells[c];
                        cell.Value = dtSource.Rows[r].ItemArray[c];
                    }

                    //string referenceNumber = GetReferenceNumberFromOriginalImportWorksheet(_workSheet, leadRowIndex + r);

                    //if (referenceNumber != String.Empty)
                    //{
                    //    DataTable dataTable = Insure.GetLeadAnalysisDetailsForRefNo(_inBatch.ID, referenceNumber);

                    //    if (dataTable.Rows.Count > 0)
                    //    {
                    //        for (int c = 0; c < dataTable.Rows[0].ItemArray.Length; c++)
                    //        {
                    //            cell = destinationWorksheet.Rows[currentRow].Cells[column + c];
                    //            cell.Value = dataTable.Rows[0].ItemArray[c];
                    //        }
                    //    }
                    //}
                    ++currentRow;
                }
            }

            #endregion Step 2: For each row, get the data from the database and insert it

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

        private void EnableDisableExportButton()
        {
            try
            {
                if (_xdgHeaderPrefixAreaCheckbox != null && (_xdgHeaderPrefixAreaCheckbox.IsChecked == true || _xdgHeaderPrefixAreaCheckbox.IsChecked == null))
                {
                    btnReport.IsEnabled = true;
                    return;
                }
                btnReport.IsEnabled = false;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
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
                if (IsAllInputParametersSpecifiedAndValid())
                {
                    _strTodaysDate = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
                    _strTodaysDateIncludingColons = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    EnableAllControls(false);

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

        //private void cmbCampaign_Loaded(object sender, RoutedEventArgs e)
        //{
        //    cmbCampaign.Focus();
        //}

        //private void cmbCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        _campaignID = Convert.ToInt64(cmbCampaign.SelectedValue);
        //        _campaign = (cmbCampaign.SelectedItem as DataRowView).Row;

        //        try
        //        {
        //            if (cmbCampaign.SelectedIndex > -1)
        //            {
        //                SetCursor(Cursors.Wait);

        //                DataTable dt = Insure.GetLeadAnalysisReportBatchesByCampaignID(_campaignID);
        //                DataColumn column = new DataColumn("Select", typeof(bool));
        //                column.DefaultValue = false;
        //                dt.Columns.Add(column);

        //                xdgBatches.DataSource = dt.DefaultView;
        //            }
        //            else
        //            {
        //                xdgBatches.DataSource = null;
        //            }

        //            //IsAllRecordsSelected = false;
        //        }

        //        catch (Exception ex)
        //        {
        //            HandleException(ex);
        //        }

        //        finally
        //        {
        //            SetCursor(Cursors.Arrow);
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        private void EmbriantComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Methods.EmbriantComboBoxPreviewKeyDown(sender, e);
        }

        private void xamEditor_Select(object sender)
        {
            switch (sender.GetType().Name)
            {
                case "XamMaskedEditor":
                    var xamMEDControl = (XamMaskedEditor)sender;

                    switch (xamMEDControl.Name)
                    {
                        default:
                            xamMEDControl.SelectAll();
                            break;
                    }
                    break;
            }
        }

        private void xamEditor_GotFocus(object sender, RoutedEventArgs e)
        {
            xamEditor_Select(sender);
        }

        #region Datagrid-Specific

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

        #endregion Datagrid-Specific

        #region Calendar-Specific

        private void calFromDate_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calFromDate.SelectedDate.ToString(), out _reportStartDate);
        }

        private void calToDate_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calToDate.SelectedDate.ToString(), out _reportEndDate);
        }

        #endregion Calendar-Specific

        private void cmbLeadAnalysisReportMode_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion Event Handlers

        //private void cmbLeadAnalysisReportMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {

        //        SetCursor(Cursors.Wait);

        //        byte? leadAnalysisReportMode = (byte?)cmbLeadAnalysisReportMode.SelectedValue;

        //        switch (leadAnalysisReportMode)
        //        {
        //            case null:
        //                _reportMode = LeadAnalysisReportMode.Default;
        //                break;

        //            case 1:
        //                _reportMode = LeadAnalysisReportMode.ByDateRange;
        //                break;

        //            case 2:
        //                _reportMode = LeadAnalysisReportMode.ByCampaignAndDateRange;
        //                break;

        //            case 3:
        //                _reportMode = LeadAnalysisReportMode.ByCampaignAndBatches;
        //                break;

        //            default:
        //                _reportMode = LeadAnalysisReportMode.Default;
        //                break;
        //        }

        //        SetReportMode(_reportMode);

        //        //DataTable dt = Insure.GetLeadAnalysisReportBatchesByCampaignID(_campaignID);
        //        //DataColumn column = new DataColumn("Select", typeof(bool));
        //        //column.DefaultValue = false;
        //        //dt.Columns.Add(column);

        //        //xdgBatches.DataSource = dt.DefaultView;


        //        //IsAllRecordsSelected = false;
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

        private void cmbReportResultsToInclude_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void cmbReportResultsToInclude_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}
