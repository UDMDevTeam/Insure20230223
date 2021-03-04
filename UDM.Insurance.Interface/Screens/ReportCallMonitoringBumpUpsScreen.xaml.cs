//using System.Linq;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
using System;
using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Windows;
//using System.Windows.Controls;
using System.Windows.Input;
//using System.Windows.Resources;
using System.Windows.Threading;
//using Infragistics.Windows.Editors.Events;
//using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
//using System.Text.RegularExpressions;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportCallMonitoringBumpUpsScreen
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

        private DateTime _fromDate;
        private DateTime _toDate;
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;
        private bool? _isReportRunning = false;

        //private DateTime _reportStartDate;
        //private DateTime _reportEndDate;
        //private string _reportStartDateLongFormat;
        //private string _reportEndDateLongFormat;

        //private string _strTodaysDate;
        //private string _strTodaysDateIncludingColons;

        #endregion Private Members

        #region Publicly Encapsulated Properties
        public bool? IsReportRunning
        {
            get
            {
                return _isReportRunning;
            }
            set
            {
                _isReportRunning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReportRunning"));
            }
        }

        #endregion Publicly Encapsulated Properties

        #region Constructors

        public ReportCallMonitoringBumpUpsScreen()
        {
            InitializeComponent();

            //LoadCampaignClusterInfo();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

        //private DataTable LoadInternalLookupValues()
        //{
        //    //return UDM.Insurance.Business.Insure.INGetCampaignTypes(false);
        //    return UDM.Insurance.Business.Insure.INGetConfirmedSalesReportCampaignGroupings();
        //}

        private bool IsAllInputParametersSpecifiedAndValid()
        {

            #region Ensuring that the From Date was specified

            if (calFromDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'From Date'.", @"No 'From Date' specified", ShowMessageType.Error);
                return false;
            }
            else
            {
                _fromDate = calFromDate.SelectedDate.Value;
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
                _toDate = calToDate.SelectedDate.Value;
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the date range is valid

            if (_fromDate > _toDate)
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

                #region Setup excel documents

                string filePathAndName = String.Format("{0}Call Monitoring Bump-Up Report, {1} - {2} ~ {3}.xlsx",
                    GlobalSettings.UserFolder,
                    _fromDate.ToString("yyyy-MM-dd HHmmdd"),
                    _toDate.ToString("yyyy-MM-dd HHmmdd"),
                    DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));
                Workbook wbConfirmedSalesReportTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateCallMonitoringBumpUps.xlsx");
                Workbook wbConfirmedSalesReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup excel documents

                #region Get the data

                DataSet dsCallMonitoringBumpUpReportData = Business.Insure.INReportCallMonitoringBumpUps(_fromDate, _toDate);

                if (dsCallMonitoringBumpUpReportData.Tables[0].Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                #endregion Get the data

                #region Insert the report sheet

                InsertCallMonitoringBumpUpReportSheet(wbConfirmedSalesReportTemplate, wbConfirmedSalesReport, dsCallMonitoringBumpUpReportData);

                #endregion Insert the report sheet

                #region Insert the report sheet

                InsertCallMonitoringBumpUpReportIndividualLeadsSheet(wbConfirmedSalesReportTemplate, wbConfirmedSalesReport, dsCallMonitoringBumpUpReportData);

                #endregion Insert the report sheet

                #region Finally, save and display the resulting workbook

                if (wbConfirmedSalesReport.Worksheets.Count > 0)
                {
                    wbConfirmedSalesReport.Save(filePathAndName);

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

                #endregion Finally, save and display the resulting workbook
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

        private void InsertCallMonitoringBumpUpReportSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsReportData)
        {
            // Refer to POD Diaries Report Data

            #region Partition the given dataset

            //string orderByString = drCurrentSalesConsultant["OrderByString"].ToString();
            DataTable dtReportData = dsReportData.Tables[0];
            DataTable dtExcelSheetDataTableColumnMappings = dsReportData.Tables[1];
            DataTable dtExcelSheetTotalsAndAverageColumnMappings = dsReportData.Tables[2];

            #endregion Partition the given dataset

            #region Declarations & Initializations

            int reportRow = 7;
            int formulaStartRow = reportRow;

            byte templateDataSheetRowSpan = 6;
            byte templateColumnSpan = 7;
            byte templateRowIndex = 7;
            byte totalsTemplateRowIndex = 8;

            string campaignDataSheetTemplateName = "CallMonitoringBumpUpReport"; 
            string reportSubHeadingCell = "A3";
            string reportDateCell = "G5";
            string reportSubTitle;

            if (_fromDate == _toDate)
            {
                reportSubTitle = String.Format("For {0}", _fromDate.ToString("yyyy-MM-dd"));
            }
            else
            {
                reportSubTitle = String.Format("For the period between {0} and {1}",
                    _fromDate.ToString("yyyy-MM-dd"),
                    _toDate.ToString("yyyy-MM-dd"));
            }

            #endregion Declarations & Initializations

            #region Add the worksheet

            Worksheet wsReportTemplate = wbTemplate.Worksheets[campaignDataSheetTemplateName];
            Worksheet wsReport = wbReport.Worksheets.Add("Report");
            Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

            #endregion Add the worksheet

            #region Populating the report details

            Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
            wsReport.GetCell(reportSubHeadingCell).Value = reportSubTitle;
            wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            #endregion Populating the report details

            #region Add the data

            reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtReportData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

            #endregion Add the data

            #region Add the totals / averages

            reportRow = Methods.MapTemplatizedExcelFormulas(wsReportTemplate, dtExcelSheetTotalsAndAverageColumnMappings, totalsTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0, formulaStartRow, reportRow - 1);

            #endregion Add the totals / averages

        }

        private void InsertCallMonitoringBumpUpReportIndividualLeadsSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsReportData)
        {
            // Refer to POD Diaries Report Data

            #region Partition the given dataset

            DataTable dtReportData = dsReportData.Tables[3];
            DataTable dtExcelSheetDataTableColumnMappings = dsReportData.Tables[4];
            
            #endregion Partition the given dataset

            #region Declarations & Initializations

            int reportRow = 3;
            //int formulaStartRow = reportRow;

            byte templateDataSheetRowSpan = 2;
            byte templateColumnSpan = 10;
            byte templateRowIndex = 3;

            string individualLeadsSheetTemplateName = "Datasheet";

            #endregion Declarations & Initializations

            #region Add the worksheet

            Worksheet wsReportTemplate = wbTemplate.Worksheets[individualLeadsSheetTemplateName];
            Worksheet wsReport = wbReport.Worksheets.Add(individualLeadsSheetTemplateName);
            Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

            #endregion Add the worksheet

            #region Populating the report details

            Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);

            #endregion Populating the report details

            #region Add the data

            reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtReportData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

            #endregion Add the data
        }


        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";

            EnableAllControls(true);
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

        private void EnableAllControls(bool isEnabled)
        {
            btnClose.IsEnabled = isEnabled;
            btnReport.IsEnabled = isEnabled;
            calFromDate.IsEnabled = isEnabled;
            calToDate.IsEnabled = isEnabled;
        }

        #endregion Event Handlers
    }
}
