using System.Linq;
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
    public partial class ReportTopTenScreen
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

        private DateTime _reportDate;
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

        public ReportTopTenScreen()
        {
            InitializeComponent();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                
                SetCursor(Cursors.Wait);

                #region Setup excel documents

                string filePathAndName = String.Format("{0}Top Ten Report, {1} ~ {2}.xlsx",
                    GlobalSettings.UserFolder,
                    _reportDate.ToString("yyyy-MM-dd HHmmdd"),
                    DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));
                Workbook wbTopTenReportTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateTopTen.xlsx");
                Workbook wbTopTenReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup excel documents

                #region Get the data                

                DataSet dsTopTenReportData = Business.Insure.INReportTopTen(_reportDate);

                if (dsTopTenReportData.Tables[0].Rows.Count == 0 && dsTopTenReportData.Tables[1].Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                #endregion Get the data

                #region Insert the various report sheets

                InsertTopTenReportSheets(wbTopTenReportTemplate, wbTopTenReport, dsTopTenReportData, dsTopTenReportData.Tables[0], "Base Agents", String.Format("For Agents on Base Campaigns for the {0}", _reportDate.ToShortDateString()));

                InsertTopTenReportSheets(wbTopTenReportTemplate, wbTopTenReport, dsTopTenReportData, dsTopTenReportData.Tables[1], "Upgrade Agents", String.Format("For Agents on Upgrade Campaigns for the {0}", _reportDate.ToShortDateString()));

                #endregion Insert the various report sheets

                #region Finally, save and display the resulting workbook

                if (wbTopTenReport.Worksheets.Count > 0)
                {
                    wbTopTenReport.Save(filePathAndName);

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

        private void InsertTopTenReportSheets(Workbook wbTemplate, Workbook wbReport, DataSet dsReportData, DataTable dtAgentData, string sheetName, string reportSubTitle)
        {
            // Refer to POD Diaries Report Data

            #region Partition the given dataset

            //string orderByString = drCurrentSalesConsultant["OrderByString"].ToString();
            //DataTable dtReportDataFiltersAndConfigs = dsReportData.Tables[0];

            //DataTable dtExcelSheetDataTableColumnMappings = dsReportData.Tables[2];

            DataTable dtBaseAgentsTotalSales = dsReportData.Tables[0];

            DataTable dtUpgradeAgentsTotalSales = dsReportData.Tables[1];

            DataTable dtExcelSheetDataTableColumnMappings = dsReportData.Tables[2];

            #endregion Partition the given dataset

            #region Declarations & Initializations

            int reportRow = 7;
            int formulaStartRow = reportRow;

            //string sheetName = "Base Agents";

            //string reportSubtitle = "For Agents on Base Campaigns for the {0}";

            string templateSheetName = sheetName;
            string reportSubHeadingCell = "A3";
            string reportDateCell = "B5";

            byte templateDataSheetRowSpan = 6;
            byte templateColumnSpan = 5;
            byte templateRowIndex = 7;

            #region Add the worksheet

            Worksheet wsReportTemplate = wbTemplate.Worksheets[templateSheetName];
            Worksheet wsReport = wbReport.Worksheets.Add(sheetName);
            Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

            #endregion Add the worksheet

            #region Populating the report details

            Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
            wsReport.GetCell(reportSubHeadingCell).Value = reportSubTitle;
            wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            #endregion Populating the report details

            #endregion Declarations & Initializations

            //foreach (DataRow row in dtBaseAgentsTotalSales.Rows)
            //{             

                

                #region Add the data
                reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtAgentData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                #endregion Add the data
            //}
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
                    EnableAllControls(false);
                    _reportDate = calReportDate.SelectedDate.Value;
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

        private void EnableAllControls(bool isEnabled)
        {
            btnClose.IsEnabled = isEnabled;
            btnReport.IsEnabled = isEnabled;
            calReportDate.IsEnabled = isEnabled;
        }

        #endregion Event Handlers

        private void calReportDate_Loaded(object sender, RoutedEventArgs e)
        {
            calReportDate.SelectedDate = DateTime.Today.AddDays(-1);
        }
    }
}
