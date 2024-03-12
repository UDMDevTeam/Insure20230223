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
    public partial class ReportDiaryScreen
    {

        #region Constants

        //private string _fontName = "Calibri";
        //private const int _fontSize = 10;
        //private const int _pointsToTwipsFactor = 20;
        //private const int _fontHeight = _fontSize * _pointsToTwipsFactor;
        bool CallData;
        #endregion Constants

        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        //private RecordCollectionBase _campaigns;
        //private System.Collections.Generic.List<Record> _campaigns;
        private List<Record> _lstSelectedCampaigns;
        private string _campaignIDList = String.Empty;

        private DateTime _startDate;
        private DateTime _endDate;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        #endregion Private Members

        #region Constructors

        public ReportDiaryScreen()
        {
            InitializeComponent();
            LoadCampaignInfo();
            long UsersID = ((UDM.Insurance.Business.User)GlobalSettings.ApplicationUser).ID;
            long UserType = ((UDM.Insurance.Business.User)GlobalSettings.ApplicationUser).FKUserType.Value;
            if (UserType == 2)
            {
                xdgCampaigns.Visibility = Visibility.Collapsed;
            }
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
                long UserType = ((UDM.Insurance.Business.User)GlobalSettings.ApplicationUser).FKUserType.Value;
                if (UserType == 2)
                {
                    if ((calStartDate.SelectedDate != null && (calEndDate.SelectedDate != null))) //&& (calEndDate.SelectedDate >= Cal1.SelectedDate)
                    {
                        btnReport.IsEnabled = true;
                        return;

                    }
                }
                else
                {
                    btnReport.IsEnabled = false;

                }
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

        private bool IsAllInputParametersSpecifiedAndValid()
        {
            #region Ensuring that at least one campaign was selected

            var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));
            long UserType = ((UDM.Insurance.Business.User)GlobalSettings.ApplicationUser).FKUserType.Value;
            if (UserType != 2)
            {
                if (_lstSelectedCampaigns.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No campaigns selected", ShowMessageType.Error);
                    return false;
                }
                else
                {
                    _campaignIDList = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                    _campaignIDList = _campaignIDList.Substring(0, _campaignIDList.Length - 1);
                }
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

                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateDiary.xlsx");
                Worksheet wsTemplate = wbTemplate.Worksheets["Report"];

                #endregion Setup excel workbook

                #region Get the report data
                DataSet dsDiaryReportData;
                long UsersID = ((UDM.Insurance.Business.User)GlobalSettings.ApplicationUser).ID;
                long UserType = ((UDM.Insurance.Business.User)GlobalSettings.ApplicationUser).FKUserType.Value;
                if (CallData == true && UserType != 2)
                {
                    dsDiaryReportData = Business.Insure.INGetDiaryReportDataCalldata(_campaignIDList, _startDate, _endDate);
                }
                else if (UserType == 2)
                {
                    if (CallData == true)
                    {
                        dsDiaryReportData = Business.Insure.INGetDiaryReportDataCalldataSalesAgent("8,93,88,263,240,241,237,239,238,97,433,5,103,249,285,420,421,191,345,431,429,344,381,12,324,253,255,268,270,290,305,320,362,387,403,73,157,51,158,52,159,372,53,160,54,161,55,162,83,163,229,231,395,75,19,79,22,67,335,355,377,399,419,68,201,225,242,251,287,301,314,337,16,204,342,374,24,86,423,7,294,259,261,274,276,292,307,322,364,391,405,206,208,98,175,71,176,91,177,233,235,47,155,48,156,101,193,412,411,304,281,325,396,336,104,95,341,359,379,401,416,96,43,44,45,46,81,217,316,4,347,369,389,413,427,244,219,220,221,282,296,311,330,61,149,62,150,63,151,197,199,212,214,264,265,266,300,315,339,357,384,409,425,2,190,422,343,278,13,326,27,164,257,258,272,273,289,309,328,366,386,402,28,165,29,166,30,167,31,168,32,169,195,196,227,228,394,77,211,20,90,210,100,194,218,283,298,310,333,349,106,23,37,286,303,317,340,360,382,393,407,38,39,40,41,42,80,216,267,338,408,35,170,36,171,280,299,332,352,371,6,105,279,14,327,397,78,205,295,33,173,34,174,17,368,367,87,424,10,26,172,250,246,248,247,1,102,192,11,254,256,269,271,291,306,321,363,388,404,74,180,56,181,57,182,373,58,183,59,184,60,185,84,186,230,232,398,76,18,94,82,21,69,334,354,376,400,418,70,202,226,243,252,288,302,313,15,203,25,85,9,260,262,275,277,293,308,323,365,392,406,207,209,99,187,72,188,92,189,234,236,49,178,50,179,3,348,370,390,414,428,245,222,223,224,284,297,312,331,64,152,65,153,66,154,198,200,213,215,318,353,378,358,385,410,426,89", _startDate, _endDate, UsersID);
                    }
                    else
                    {
                        dsDiaryReportData = Business.Insure.INGetDiaryReportDataSalesAgent("8,93,88,263,240,241,237,239,238,97,433,5,103,249,285,420,421,191,345,431,429,344,381,12,324,253,255,268,270,290,305,320,362,387,403,73,157,51,158,52,159,372,53,160,54,161,55,162,83,163,229,231,395,75,19,79,22,67,335,355,377,399,419,68,201,225,242,251,287,301,314,337,16,204,342,374,24,86,423,7,294,259,261,274,276,292,307,322,364,391,405,206,208,98,175,71,176,91,177,233,235,47,155,48,156,101,193,412,411,304,281,325,396,336,104,95,341,359,379,401,416,96,43,44,45,46,81,217,316,4,347,369,389,413,427,244,219,220,221,282,296,311,330,61,149,62,150,63,151,197,199,212,214,264,265,266,300,315,339,357,384,409,425,2,190,422,343,278,13,326,27,164,257,258,272,273,289,309,328,366,386,402,28,165,29,166,30,167,31,168,32,169,195,196,227,228,394,77,211,20,90,210,100,194,218,283,298,310,333,349,106,23,37,286,303,317,340,360,382,393,407,38,39,40,41,42,80,216,267,338,408,35,170,36,171,280,299,332,352,371,6,105,279,14,327,397,78,205,295,33,173,34,174,17,368,367,87,424,10,26,172,250,246,248,247,1,102,192,11,254,256,269,271,291,306,321,363,388,404,74,180,56,181,57,182,373,58,183,59,184,60,185,84,186,230,232,398,76,18,94,82,21,69,334,354,376,400,418,70,202,226,243,252,288,302,313,15,203,25,85,9,260,262,275,277,293,308,323,365,392,406,207,209,99,187,72,188,92,189,234,236,49,178,50,179,3,348,370,390,414,428,245,222,223,224,284,297,312,331,64,152,65,153,66,154,198,200,213,215,318,353,378,358,385,410,426,89", _startDate, _endDate, UsersID);
                    }
                }
                else
                {
                    dsDiaryReportData = Business.Insure.INGetDiaryReportData(_campaignIDList, _startDate, _endDate);
                }

                DataTable dtMainReportData = dsDiaryReportData.Tables[0];
                DataTable dtCampaigns = dsDiaryReportData.Tables[1];
                DataTable dtTSRDiaries = dsDiaryReportData.Tables[2];
                DataTable dtColumnMappings = dsDiaryReportData.Tables[3];

                #endregion Get the report data

                #region Variable declarations

                string filePathAndName = String.Empty;

                string campaign = String.Empty;
                string campaignFilterString = String.Empty;
                string currentTSR = String.Empty;
                string currentTSRFilterString = String.Empty;

                byte templateColumnSpan = 11;
                byte templateRowSpan = 6;
                byte templateDataRowIndex = 7;

                int reportRow = 7;

                #endregion Variable declarations

                #region Loop through each campaign and create a new workbook for each

                foreach (DataRow drCampaign in dtCampaigns.Rows)
                {


                    campaign = drCampaign["Campaign"].ToString();
                    campaignFilterString = drCampaign["CampaignFilterString"].ToString();

                    #region Instantiate a new Excel workbook

                    Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                    filePathAndName = String.Format("{0}{1} Diary Report ~ {2}.xlsx", GlobalSettings.UserFolder, campaign, DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));
                    
                    #endregion Instantiate a new Excel workbook

                    DataTable dtCurrentCampaignTSRs = dtTSRDiaries.Select(campaignFilterString).CopyToDataTable();

                    foreach (DataRow drCurrentTSR in dtCurrentCampaignTSRs.Rows)
                    {
                        currentTSR = drCurrentTSR["AllocatedTo"].ToString();
                        currentTSRFilterString = drCurrentTSR["CampaignTSRFilterString"].ToString();

                        DataTable dtCurrentCampaignAndTSRData = dtMainReportData.Select(currentTSRFilterString).CopyToDataTable();

                        #region Add the current sales consultant's worksheet

                        Worksheet wsReport = wbReport.Worksheets.Add(Methods.ParseWorksheetName(wbReport, currentTSR));
                        Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                        #endregion Add the current sales consultant's worksheet

                        #region Insert the column headings and populate the details

                        Methods.CopyExcelRegion(wsTemplate, 0, 0, templateRowSpan, templateColumnSpan, wsReport, 0, 0);

                        wsReport.GetCell("A1").Value = String.Format("Diary Report - {0}", campaign);
                        wsReport.GetCell("A3").Value = String.Format("Leads that were saved with a Diary status for the time between {0} and {1}", _startDate.ToString("dddd, d MMMM yyyy"), _endDate.ToString("dddd, d MMMM yyyy"));
                        wsReport.GetCell("A5").Value = currentTSR;
                        //wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        #endregion Insert the column headings and populate the details

                        #region Add the data

                        Methods.MapTemplatizedExcelValues(wsTemplate, dtCurrentCampaignAndTSRData, dtColumnMappings, templateDataRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                        // Reset the row index
                        reportRow = 7;

                        #endregion Add the data
                    }


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

                #endregion Loop through each campaign and create a new workbook for each

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

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (IsAllInputParametersSpecifiedAndValid())
                {
                    long UserType = ((UDM.Insurance.Business.User)GlobalSettings.ApplicationUser).FKUserType.Value;
                    if (UserType == 2)
                    {
                    }
                    else
                    {
                        btnClose.IsEnabled = false;
                        btnReport.IsEnabled = false;
                        xdgCampaigns.IsEnabled = false;
                        calStartDate.IsEnabled = false;
                        calEndDate.IsEnabled = false;
                    }
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

        private void CallDataCB_Checked(object sender, RoutedEventArgs e)
        {
            CallData = true;
        }

        private void CallDataCB_Unchecked(object sender, RoutedEventArgs e)
        {
            CallData = false;
        }
    }
}
