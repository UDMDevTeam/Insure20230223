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
using UDM.Insurance.Business;
using System.Transactions;
//using System.Text.RegularExpressions;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportCallMonitoringScreen
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
        private DataTable dtCallMonitoringQueryLookups;
        private byte _staffType;

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

        public ReportCallMonitoringScreen()
        {
            InitializeComponent();

            LoadCallMonitoringQueryLookups();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

        private void LoadCallMonitoringQueryLookups()
        {
            dtCallMonitoringQueryLookups = Insure.INGetReportCallMonitoringQueryScreenLookups();

            LoadStaffTypes();
        }

        private void LoadStaffTypes()
        {
            try
            {

                //DataView staffView = new DataView(dtCallMonitoringQueryLookups, "", "[ID]", DataViewRowState.OriginalRows);
                //DataTable dtStaffCmb = staffView.ToTable(false, "ID", "Description");

                cmbStaffType.Populate(dtCallMonitoringQueryLookups, "Description", "ID");
                cmbStaffType.SelectedIndex = 2;

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

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

                string filePathAndName = String.Format("{0}Call Monitoring Query Report, {1} - {2} ~ {3}.xlsx",
                    GlobalSettings.UserFolder,
                    _fromDate.ToString("yyyy-MM-dd HHmmdd"),
                    _toDate.ToString("yyyy-MM-dd HHmmdd"),
                    DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));
                Workbook wbConfirmedSalesReportTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateCallMonitoring.xlsx");
                Workbook wbConfirmedSalesReport = new Workbook(WorkbookFormat.Excel2007);



                #endregion Setup excel documents

                #region Get the data
                try
                {
                    DataSet dsCallMonitoringReportData = null;

                    var transactionOptions = new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                    };

                    using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                    {
                        dsCallMonitoringReportData = Business.Insure.INReportCallMonitoring(_fromDate, _toDate, _staffType);
                    }



                    if (dsCallMonitoringReportData.Tables[1].Rows.Count == 0)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", ShowMessageType.Information);
                        });
                        return;

                    }


                    #endregion Get the data

                    #region Insert the various report sheets

                    InsertCallMonitoringReportSheets(wbConfirmedSalesReportTemplate, wbConfirmedSalesReport, dsCallMonitoringReportData);
                }
                catch (Exception ex)
                {
                    return;
                    //  HandleException(ex);
                }

                #endregion Insert the various report sheets

                #region Finally, save and display the resulting workbook

                if (wbConfirmedSalesReport.Worksheets.Count > 0)
                {
                    wbConfirmedSalesReport.Save(filePathAndName);

                    //Display excel document
                    Process.Start(filePathAndName);

                }
                else
                {

                    //return;
                    ////ex.Message = Error filling data set with data adapter
                    ////InnerException = {"Remote access is not supported for transaction isolation level \"SNAPSHOT\"."}

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

        private void InsertCallMonitoringReportSheets(Workbook wbTemplate, Workbook wbReport, DataSet dsReportData)
        {
            // Refer to POD Diaries Report Data

            #region Partition the given dataset

            //string orderByString = drCurrentSalesConsultant["OrderByString"].ToString();
            DataTable dtReportDataFiltersAndConfigs = dsReportData.Tables[0];

            DataTable dtExcelSheetDataTableColumnMappings = dsReportData.Tables[2];

            #endregion Partition the given dataset

            #region Declarations & Initializations

            int reportRow = 8;
            int formulaStartRow = reportRow;

            string sheetName;
            string filterString;
            string orderByString;
            string reportSubtitle;

            string templateSheetName;
            string reportSubHeadingCell;
            string reportDateCell;

            byte templateDataSheetRowSpan;
            byte templateColumnSpan;
            byte templateRowIndex;

            #endregion Declarations & Initializations

            foreach (DataRow row in dtReportDataFiltersAndConfigs.Rows)
            {

                DataTable dtReportData;

                filterString = row["FilterString"].ToString();
                orderByString = row["OrderByString"].ToString();

                if (!String.IsNullOrEmpty(filterString))
                {
                    var filteredRows = dsReportData.Tables[1].Select(filterString, orderByString).AsEnumerable();
                    if (!filteredRows.Any())
                    {
                        continue;
                    }
                    else
                    {
                        dtReportData = dsReportData.Tables[1].Select(filterString, orderByString).CopyToDataTable();
                    }
                }
                else
                {
                    dtReportData = dsReportData.Tables[1];
                }

                #region Get the configs for the report sheet

                sheetName = row["SheetName"].ToString();
                reportSubtitle = row["ReportSubtitle"].ToString();

                templateSheetName = row["TemplateSheetName"].ToString();
                reportSubHeadingCell = row["ReportSubHeadingCell"].ToString();
                reportDateCell = row["ReportDateCell"].ToString();

                templateDataSheetRowSpan = Convert.ToByte(row["TemplateDataSheetRowSpan"]);
                templateColumnSpan = Convert.ToByte(row["TemplateColumnSpan"]);
                templateRowIndex = Convert.ToByte(row["TemplateRowIndex"]);

                #endregion Get the configs for the report sheet

                #region Add the worksheet

                Worksheet wsReportTemplate = wbTemplate.Worksheets[templateSheetName];
                Worksheet wsReport = wbReport.Worksheets.Add(sheetName);
                Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                #endregion Add the worksheet

                #region Populating the report details

                Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
                wsReport.GetCell(reportSubHeadingCell).Value = reportSubtitle;
                wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                #endregion Populating the report details

                #region Add the data
                reportRow = 8;
                reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtReportData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                #endregion Add the data
            }
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

        private void cmbStaffType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cmbStaffType.SelectedIndex != -1)
            {
                _staffType = Convert.ToByte(cmbStaffType.SelectedValue);
            }
        }

        #endregion Event Handlers

    }
}
