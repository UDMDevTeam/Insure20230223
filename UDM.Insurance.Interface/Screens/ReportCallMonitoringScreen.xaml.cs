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
using UDM.Insurance.Interface.Data;
using System.Collections.Generic;
using Infragistics.Windows.DataPresenter;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using Workbook = Infragistics.Documents.Excel.Workbook;
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
        private System.Data.DataTable dtCallMonitoringQueryLookups;
        private byte _staffType;

        //private DateTime _reportStartDate;
        //private DateTime _reportEndDate;
        //private string _reportStartDateLongFormat;
        //private string _reportEndDateLongFormat;

        //private string _strTodaysDate;
        //private string _strTodaysDateIncludingColons;

        private System.Windows.Controls.CheckBox _xdgHeaderPrefixAreaCheckbox;
        private List<Record> _campaigns;
        private DateTime _startDate = DateTime.Now;
        private DateTime _endDate = DateTime.Now;

        //private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        //private int _timer1;

        //private byte _staffType;

        private bool _includeAdmin;

        private System.Data.DataTable _dtAllCampaigns;

        private System.Data.DataTable dtStaffType;

        private System.Data.DataTable _dtAllAgents;

        private System.Data.DataTable _dtAllQAs;

        private System.Data.DataTable _dtAllSalesCoach;


        private DataSet dsCallMonitoringLookups;

        private DataSet dsTurnoverLookupsSalesCoaches;


        private bool isLookupsLoaded = false;

        private ReportData _rData = new ReportData();

        bool allRecordsSelected = false;

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

        private List<Record> _lstSelectedCampaigns;
        private string _fkCampaignIDs = "";

        private List<Record> _lstSelectedAgents;
        private string _fkUserIDs = "";

        private List<Record> _lstSelectedQAs;
        private string _fkQAIDs = "";

        private List<Record> _lstSelectedSalesCoaches;
        private string _fkSalesCoachesIDs = "";


        //Workbook wbTemplate;
        //Workbook wbReport;
        DataSet dsReportData;

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

            if (GlobalSettings.ApplicationUser.ID == 1 || GlobalSettings.ApplicationUser.ID == 72 || GlobalSettings.ApplicationUser.ID == 199 || GlobalSettings.ApplicationUser.ID == 394 || GlobalSettings.ApplicationUser.ID == 3388)
            {
                CallMonReportCB.Visibility = Visibility.Visible;
                tbOldReportType.Visibility = Visibility.Visible;
            }
            else
            {
                CallMonReportCB.Visibility = Visibility.Collapsed;
                tbOldReportType.Visibility = Visibility.Collapsed;
            }

            LoadCallMonitoringQueryLookups();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);

            isLookupsLoaded = true;
        }

        #endregion Constructors

        #region Private Methods

        private void LoadCallMonitoringQueryLookups()
        {

            dsCallMonitoringLookups = Insure.INGetReportCallMonitoringQueryScreenLookups();

            _dtAllCampaigns = dsCallMonitoringLookups.Tables[0];
            _dtAllAgents = dsCallMonitoringLookups.Tables[3];
            _dtAllQAs = dsCallMonitoringLookups.Tables[4];

            LoadStaffTypes();

        }

        private void LoadAgentInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);

                RData.CallMonitoringQueryMode = lkpINCampTSRReportMode.ByTSR;

                if (RData.CallMonitoringQueryMode == lkpINCampTSRReportMode.ByTSR)
                {
                    DataSet ds = Insure.INGetTurnoverAgents(RData.CallMonitoringQueryCompanyMode, _staffType, RData.IncludeAdmin);

                    System.Data.DataTable dt = ds.Tables[0];
                    DataColumn column = new DataColumn("Select", typeof(bool));
                    column.DefaultValue = false;
                    dt.Columns.Add(column);

                    xdgCampaigns.DataSource = dt.DefaultView;
                }
                //DataSet ds = Methods.ExecuteStoredProcedure("spGetSalesAgents2", null);

                //DataTable dt = ds.Tables[0];
                //DataColumn column = new DataColumn("IsChecked", typeof(bool));
                //column.DefaultValue = false;
                //dt.Columns.Add(column);

                //xdgCampaigns.DataSource = dt.DefaultView;
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

        private void LoadQAInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);

                RData.CallMonitoringQueryMode = lkpINCampTSRReportMode.ByQA;

                if (RData.CallMonitoringQueryMode == lkpINCampTSRReportMode.ByQA)
                {
                    System.Data.DataTable dtQAs = _dtAllQAs.Select().CopyToDataTable();

                    xdgCampaigns.DataSource = dtQAs.DefaultView;
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

        private void LoadSalesCoachInfo()
        {
            try
            {
                RData.CallMonitoringQueryMode = lkpINCampTSRReportMode.SalesCoaches;

                if (RData.CallMonitoringQueryMode == lkpINCampTSRReportMode.SalesCoaches)
                {

                    dsTurnoverLookupsSalesCoaches = Insure.INGetTurnoverScreenLookupsSalesCoaches();
                    //_dtAllSalesCoach.Clear();
                    _dtAllSalesCoach = dsTurnoverLookupsSalesCoaches.Tables[0];

                    xdgCampaigns.DataSource = _dtAllSalesCoach.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void LoadStaffTypes()
        {

            try
            {


                if (radInsurance.IsChecked == true)
                {
                    RData.CallMonitoringQueryCompanyMode = lkpINTurnoverCompanyMode.Insurance;
                }

                else if (radIG.IsChecked == true)
                {
                    RData.CallMonitoringQueryCompanyMode = lkpINTurnoverCompanyMode.IG;
                }

                else
                {
                    RData.CallMonitoringQueryCompanyMode = lkpINTurnoverCompanyMode.Both;
                }


                dtStaffType = dsCallMonitoringLookups.Tables[1].Select("[Company] = " + (int)RData.CallMonitoringQueryCompanyMode).CopyToDataTable();

                DataView staffView = new DataView(dtStaffType, "", "[ID]", DataViewRowState.OriginalRows);
                System.Data.DataTable dtStaffCmb = staffView.ToTable(false, "ID", "Description");
                cmbStaffType.Populate(dtStaffCmb, "Description", "ID");
                cmbStaffType.SelectedIndex = 2;

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

            var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();

            if (RData.CallMonitoringQueryMode == lkpINCampTSRReportMode.ByTSR)
            {
                _lstSelectedAgents = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

                if (_lstSelectedAgents.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 agent from the list.", "No agents selected", ShowMessageType.Error);
                    return false;
                }
                else if (_lstSelectedAgents.Count > 1)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select only least 1 agent from the list.", "Multiple agent selection", ShowMessageType.Error);
                    return false;
                }
                else if (_lstSelectedAgents.Count == 1)
                {
                    _fkUserIDs = _lstSelectedAgents.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                    _fkUserIDs = _fkUserIDs.Substring(0, _fkUserIDs.Length - 1);
                }
            }
            else if (RData.CallMonitoringQueryMode == lkpINCampTSRReportMode.ByQA)
            {
                _lstSelectedQAs = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

                if (_lstSelectedQAs.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 Supervisor/QA from the list.", "No Team selected", ShowMessageType.Error);
                    return false;
                }
                else
                {
                    _fkQAIDs = _lstSelectedQAs.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                    _fkQAIDs = _fkQAIDs.Substring(0, _fkQAIDs.Length - 1);
                }
            }
            else if (RData.CallMonitoringQueryMode == lkpINCampTSRReportMode.SalesCoaches)
            {
                _lstSelectedSalesCoaches = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

                if (_lstSelectedSalesCoaches.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 Sales Coach from the list.", "No Team selected", ShowMessageType.Error);
                    return false;
                }
                else
                {
                    _fkSalesCoachesIDs = _lstSelectedSalesCoaches.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                    _fkSalesCoachesIDs = _fkSalesCoachesIDs.Substring(0, _fkSalesCoachesIDs.Length - 1);
                }
            }


            // Otherwise, if all is well, proceed:
            return true;


        }

        private bool IsAllInputParametersSpecifiedAndValidOld()
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

                string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


                //string filePathAndName = String.Format("{0}DebiCheck Report Upgrades ({1}), {2}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));


                //string filePathAndName = String.Format("Call Monitoring Query Report, {0} - {1} ~ {2}.xlsx",
                //    GlobalSettings.UserFolder,
                //    _fromDate.ToString("yyyy-MM-dd HHmmdd"),
                //    _toDate.ToString("yyyy-MM-dd HHmmdd"),
                //    DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));

                //Infragistics.Documents.Excel.Workbook wbConfirmedSalesReportTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateCallMonitoring.xlsx");
                //Infragistics.Documents.Excel.Workbook wbConfirmedSalesReport = new Workbook(WorkbookFormat.Excel2007);
                //Infragistics.Documents.Excel.Workbook wbTemplate = new Workbook(WorkbookFormat.Excel2007);
                //Infragistics.Documents.Excel.Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);


                #endregion Setup excel documents

                #region Get the data

                DataSet dsCallMonitoringReportData = Business.Insure.INReportCallMonitoring(_fromDate, _toDate, _staffType, _fkUserIDs, _fkQAIDs, _fkSalesCoachesIDs);

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

                //InsertCallMonitoringReportSheets(wbConfirmedSalesReportTemplate, wbConfirmedSalesReport, dsCallMonitoringReportData);

                Excel.Application xlApp = new Excel.Application();

                xlApp.Workbooks.Add();

                //Worksheet wsReport = xlApp.ActiveSheet;

                //wbConfirmedSalesReport 

                //Excel._Workbook workBook = xlApp.ActiveWorkbook;

                System.Data.DataTable dtReportDataFiltersAndConfigs = dsCallMonitoringReportData.Tables[0];

                System.Data.DataTable dtExcelSheetDataTableColumnMappings = dsCallMonitoringReportData.Tables[2];

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

                int sheetNameCount = dtReportDataFiltersAndConfigs.AsEnumerable().Where(x => x["SheetName"].ToString() != "Y").ToList().Count;

                int rowIndex = 0;

                System.Data.DataTable dtReportData;

                dtReportData = dsCallMonitoringReportData.Tables[1];

                dtReportData.Columns.Remove("FKINImportID");

                for (int k = 1; k <= sheetNameCount; k++)
                {

                    //filterString = row["FilterString"].ToString();
                    //orderByString = row["OrderByString"].ToString();

                    //if (!String.IsNullOrEmpty(filterString))
                    //{
                    //    var filteredRows = dsCallMonitoringReportData.Tables[1].Select(filterString, orderByString).AsEnumerable();
                    //    if (!filteredRows.Any())
                    //    {
                    //        continue;
                    //    }
                    //    else
                    //    {
                    //        dtReportData = dsCallMonitoringReportData.Tables[1].Select(filterString, orderByString).CopyToDataTable();
                    //    }
                    //}
                    //else
                    //{
                    //    dtReportData = dsCallMonitoringReportData.Tables[1];
                    //}

                    //dtReportData = dsCallMonitoringReportData.Tables[1].Select(filterString, orderByString).CopyToDataTable();



                    //#region Get the configs for the report sheet


                    sheetName = dtReportDataFiltersAndConfigs.Rows[rowIndex]["SheetName"].ToString();

                    //sheetName = row["SheetName"].ToString();
                    //reportSubtitle = row["ReportSubtitle"].ToString();

                    //templateSheetName = row["TemplateSheetName"].ToString();
                    //reportSubHeadingCell = row["ReportSubHeadingCell"].ToString();
                    //reportDateCell = row["ReportDateCell"].ToString();

                    rowIndex++;

                    //templateDataSheetRowSpan = Convert.ToByte(row["TemplateDataSheetRowSpan"]);
                    //templateColumnSpan = Convert.ToByte(row["TemplateColumnSpan"]);
                    //templateRowIndex = Convert.ToByte(row["TemplateRowIndex"]);

                    #endregion Get the configs for the report sheet

                    #region Add the worksheet

                    Excel._Worksheet workSheet = xlApp.Worksheets.Add();

                    workSheet.Columns.AutoFit();

                    workSheet.Cells[1, 1] = "Date Generated: " + _fromDate.ToShortDateString() + " " + _toDate.ToShortDateString();
                    workSheet.Cells[1, 1].Font.Bold = true;



                    // column headings
                    for (var i = 0; i < dtReportData.Columns.Count; i++)
                    {

                        workSheet.Columns.AutoFit();

                        workSheet.Cells[4, i + 1] = dtReportData.Columns[i].ColumnName;
                        workSheet.Cells[4, i + 1].Font.Bold = true;
                        workSheet.Cells[4, i + 1].Interior.Color = System.Drawing.Color.LightGray;

                        Microsoft.Office.Interop.Excel.Range range = workSheet.UsedRange;
                        Microsoft.Office.Interop.Excel.Range cell = range.Cells[4, i + 1];
                        Microsoft.Office.Interop.Excel.Borders border = cell.Borders;
                        border[XlBordersIndex.xlEdgeLeft].LineStyle =
                            Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        border[XlBordersIndex.xlEdgeTop].LineStyle =
                            Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        border[XlBordersIndex.xlEdgeBottom].LineStyle =
                            Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        border[XlBordersIndex.xlEdgeRight].LineStyle =
                            Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    }

                    // rows
                    for (var i = 0; i < dtReportData.Rows.Count; i++)
                    {
                        // to do: format datetime values before printing
                        for (var j = 0; j < dtReportData.Columns.Count; j++)
                        {
                            workSheet.Columns.AutoFit();

                            workSheet.Cells[i + 5, j + 1] = dtReportData.Rows[i][j];

                            Microsoft.Office.Interop.Excel.Range range = workSheet.UsedRange;
                            Microsoft.Office.Interop.Excel.Range cell = range.Cells[i + 5, j + 1];
                            Microsoft.Office.Interop.Excel.Borders border = cell.Borders;
                            border[XlBordersIndex.xlEdgeLeft].LineStyle =
                                Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                            border[XlBordersIndex.xlEdgeTop].LineStyle =
                                Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                            border[XlBordersIndex.xlEdgeBottom].LineStyle =
                                Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                            border[XlBordersIndex.xlEdgeRight].LineStyle =
                                Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        }
                    }

                    //int sheetNameCount = row.dtReportDataFiltersAndConfigs.Columns.Count;

                    //int WorksheetNameCount = dtReportDataFiltersAndConfigs.AsEnumerable().Where(x => x["SheetName"].ToString() != "Y").ToList().Count;


                    for (int i = 1; i <= sheetNameCount; i++)
                    {
                        workSheet.Name = sheetName;

                        if (workSheet.Name.Contains("Sheet1"))
                        {
                            //workSheet.Delete();
                            xlApp.Worksheets.Delete();
                        }
                    }

                    #endregion Add the data




                }



                xlApp.Visible = true;

                //InsertCallMonitoringReportSheets(wbConfirmedSalesReportTemplate, wbConfirmedSalesReport, dsCallMonitoringReportData);

                //#endregion Insert the various report sheets

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


            //string orderByString = drCurrentSalesConsultant["OrderByString"].ToString();
            System.Data.DataTable dtReportDataFiltersAndConfigs = dsReportData.Tables[0];

            System.Data.DataTable dtExcelSheetDataTableColumnMappings = dsReportData.Tables[2];


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

                System.Data.DataTable dtReportData;

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

                Infragistics.Documents.Excel.Worksheet wsReportTemplate = wbTemplate.Worksheets[templateSheetName];
                Infragistics.Documents.Excel.Worksheet wsReport = wbReport.Worksheets.Add(sheetName);
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


        private void ReportOld(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup excel documents

                string filePathAndName = String.Format("Call Monitoring Query Report, {0} - {1} ~ {2}.xlsx",
                    //GlobalSettings.UserFolder,
                    _fromDate.ToString("yyyy-MM-dd HHmmdd"),
                    _toDate.ToString("yyyy-MM-dd HHmmdd"),
                    DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));
                Infragistics.Documents.Excel.Workbook wbConfirmedSalesReportTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateCallMonitoring.xlsx");
                Infragistics.Documents.Excel.Workbook wbConfirmedSalesReport = new Infragistics.Documents.Excel.Workbook(WorkbookFormat.Excel2007);

                #endregion Setup excel documents

                #region Get the data

                DataSet dsCallMonitoringReportData = Business.Insure.INReportCallMonitoringOldVersion(_fromDate, _toDate, _staffType);

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

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";

            EnableAllControls(true);
        }

        public void PreReportGenerationOperations(/*byte reportScope*/)
        {

            var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();

            if (RData.CallMonitoringQueryMode == lkpINCampTSRReportMode.ByTSR)
            {
                _lstSelectedAgents = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

                if (_lstSelectedAgents.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 agent from the list.", "No agents selected", ShowMessageType.Error);
                    return;
                }
                else if (_lstSelectedAgents.Count > 1)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select only least 1 agent from the list.", "Multiple agent selection", ShowMessageType.Error);
                    return;
                }
                else if (_lstSelectedAgents.Count == 1)
                {
                    _fkUserIDs = _lstSelectedAgents.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                    _fkUserIDs = _fkUserIDs.Substring(0, _fkUserIDs.Length - 1);
                }
            }
            else if (RData.CallMonitoringQueryMode == lkpINCampTSRReportMode.ByQA)
            {
                _lstSelectedQAs = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

                if (_lstSelectedQAs.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 Supervisor/QA from the list.", "No Team selected", ShowMessageType.Error);
                    return;
                }
                else if (_lstSelectedQAs.Count > 1)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 Supervisor/QA from the list.", "No Team selected", ShowMessageType.Error);
                    return;
                }
                else if (_lstSelectedQAs.Count == 1)
                {
                    _fkQAIDs = _lstSelectedQAs.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                    _fkQAIDs = _fkQAIDs.Substring(0, _fkQAIDs.Length - 1);
                }
            }
            else if (RData.CallMonitoringQueryMode == lkpINCampTSRReportMode.SalesCoaches)
            {
                _lstSelectedSalesCoaches = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

                if (_lstSelectedSalesCoaches.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 Sales Coach from the list.", "No Team selected", ShowMessageType.Error);
                    return;
                }
                else if (_lstSelectedSalesCoaches.Count > 1)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 Sales Coach from the list.", "No Team selected", ShowMessageType.Error);
                    return;
                }
                else if (_lstSelectedSalesCoaches.Count == 1)
                {
                    _fkSalesCoachesIDs = _lstSelectedSalesCoaches.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                    _fkSalesCoachesIDs = _fkSalesCoachesIDs.Substring(0, _fkSalesCoachesIDs.Length - 1);
                }
            }


            //if (chkIncludeCumulativeSheet.IsChecked.HasValue)
            //{
            //    if (chkIncludeCumulativeSheet.IsChecked.Value)
            //    {
            //        _insertSingleSheetWithAllData = true;
            //    }
            //    else
            //    {
            //        _insertSingleSheetWithAllData = false;
            //    }
            //}
            //else
            //{
            //    _insertSingleSheetWithAllData = false;
            //}

            //EnableDisableControls(false);

            //_reportScope = reportScope;
        }

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;

            btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnReport.ToolTip = btnReport.Content;
        }

        //#endregion Private Methods

        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (CallMonReportCB.IsChecked == true)
                {

                    EnableAllControls(true);

                    if (IsAllInputParametersSpecifiedAndValidOld())
                    {
                        //PreReportGenerationOperations();
                        //btnClose.IsEnabled = false;
                        //btnReport.IsEnabled = false;
                        //xdgCampaigns.IsEnabled = false;
                        //calFromDate.IsEnabled = false;
                        //calToDate.IsEnabled = false;

                        //allRecordsSelected = AllRecordsSelected() ?? false;

                        EnableAllControls(true);

                        BackgroundWorker worker = new BackgroundWorker();

                        worker.DoWork += ReportOld;
                        worker.RunWorkerCompleted += ReportCompleted;
                        worker.RunWorkerAsync();

                        dispatcherTimer1.Start();
                    }
                }
                else if (CallMonReportCB.IsChecked == false)
                {
                    if (IsAllInputParametersSpecifiedAndValid())
                    {

                        PreReportGenerationOperations();
                        btnClose.IsEnabled = false;
                        btnReport.IsEnabled = false;
                        xdgCampaigns.IsEnabled = false;
                        calFromDate.IsEnabled = false;
                        calToDate.IsEnabled = false;

                        allRecordsSelected = AllRecordsSelected() ?? false;

                        BackgroundWorker worker = new BackgroundWorker();

                        //if (CallMonReportCB.IsChecked == true)
                        //{

                        //    worker.DoWork += ReportOld;
                        //    worker.RunWorkerCompleted += ReportCompleted;
                        //    worker.RunWorkerAsync();

                        //    dispatcherTimer1.Start();
                        //}
                        //else
                        //{
                        worker.DoWork += Report;
                        worker.RunWorkerCompleted += ReportCompleted;
                        worker.RunWorkerAsync();

                        dispatcherTimer1.Start();
                        //}


                    }
                }
                //if (IsAllInputParametersSpecifiedAndValid())
                //{
                //    EnableAllControls(false);

                //    BackgroundWorker worker = new BackgroundWorker();
                //    worker.DoWork += Report;
                //    worker.RunWorkerCompleted += ReportCompleted;
                //    worker.RunWorkerAsync();

                //    dispatcherTimer1.Start();
                //}
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
            xdgCampaigns.IsEnabled = isEnabled;
            calToDate.IsEnabled = isEnabled;
        }


        #endregion Event Handlers

        private void radCompanyType_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isLookupsLoaded)
                {
                    LoadStaffTypes();
                    LoadAgentInfo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void radBySalesCoach_Checked(object sender, RoutedEventArgs e)
        {

            try
            {
                _fkUserIDs = "";
                _fkQAIDs = "";
                _fkCampaignIDs = "";
                LoadSalesCoachInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void radByQA_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                _fkUserIDs = "";
                _fkCampaignIDs = "";
                LoadQAInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Data.DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;

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
                System.Data.DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;

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
                    //allRecordsSelected = (bool)AllRecordsSelected();
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
                _xdgHeaderPrefixAreaCheckbox = (System.Windows.Controls.CheckBox)sender;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void EnableDisableExportButton()
        {
            try
            {

                if (CallMonReportCB.IsChecked == true)
                {

                    btnReport.IsEnabled = true;

                }
                else
                {
                    if ((calFromDate.SelectedDate != null && (calToDate.SelectedDate != null))) //&& (calEndDate.SelectedDate >= Cal1.SelectedDate)
                    {
                        if (_xdgHeaderPrefixAreaCheckbox != null && (_xdgHeaderPrefixAreaCheckbox.IsChecked == true || _xdgHeaderPrefixAreaCheckbox.IsChecked == null))
                        {
                            btnReport.IsEnabled = true;
                            return;
                        }
                    }
                }

                btnReport.IsEnabled = false;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void xdgCampaigns_FieldLayoutInitialized(object sender, Infragistics.Windows.DataPresenter.Events.FieldLayoutInitializedEventArgs e)
        {

            if (RData.CallMonitoringQueryMode == lkpINCampTSRReportMode.ByTSR)
            {
                Field fieldID = e.FieldLayout.Fields["ID"];
                fieldID.Visibility = Visibility.Collapsed;
                Field fieldDescription = e.FieldLayout.Fields["Description"];
                fieldDescription.Visibility = Visibility.Visible;
                fieldDescription.Width = new FieldLength(290);
                fieldDescription.Label = "Agent Name";
                Field fieldSelect = e.FieldLayout.Fields["Select"];
                fieldSelect.Visibility = Visibility.Collapsed;
            }
            else if (RData.CallMonitoringQueryMode == lkpINCampTSRReportMode.ByQA)
            {
                Field fieldID = e.FieldLayout.Fields["ID"];
                fieldID.Visibility = Visibility.Collapsed;
                Field fieldDescription = e.FieldLayout.Fields["Description"];
                fieldDescription.Visibility = Visibility.Visible;
                fieldDescription.Width = new FieldLength(290);
                fieldDescription.Label = "Team Name";
                Field fieldSelect = e.FieldLayout.Fields["Select"];
                fieldSelect.Visibility = Visibility.Collapsed;
            }
            else if (RData.CallMonitoringQueryMode == lkpINCampTSRReportMode.SalesCoaches)
            {
                Field fieldID = e.FieldLayout.Fields["ID"];
                fieldID.Visibility = Visibility.Collapsed;
                Field fieldDescription = e.FieldLayout.Fields["Description"];
                fieldDescription.Visibility = Visibility.Visible;
                fieldDescription.Width = new FieldLength(290);
                fieldDescription.Label = "Sales Coach";
                Field fieldSelect = e.FieldLayout.Fields["Select"];
                fieldSelect.Visibility = Visibility.Collapsed;
            }

        }

        private void radByTSR_Checked(object sender, RoutedEventArgs e)
        {
            if (isLookupsLoaded == true)
            {
                _fkCampaignIDs = "";
                _fkQAIDs = "";
                LoadAgentInfo();
            }

        }

        private void cmbStaffType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cmbStaffType.SelectedIndex != -1)
            {
                _staffType = Convert.ToByte(cmbStaffType.SelectedIndex);
                LoadAgentInfo();
            }
        }

        private void CallMonReportCB_Checked(object sender, RoutedEventArgs e)
        {


            xdgCampaigns.Visibility = Visibility.Collapsed;

            radByQA.Visibility = Visibility.Collapsed;
            radBySalesCoach.Visibility = Visibility.Collapsed;
            radByTSR.Visibility = Visibility.Collapsed;

            tbCompanyType.Visibility = Visibility.Collapsed;
            tbStaffType.Visibility = Visibility.Collapsed;

            radIG.Visibility = Visibility.Collapsed;
            radInsurance.Visibility = Visibility.Collapsed;
            radBoth.Visibility = Visibility.Collapsed;

            EnableDisableExportButton();

            EnableAllControls(true);

        }
    }
}
