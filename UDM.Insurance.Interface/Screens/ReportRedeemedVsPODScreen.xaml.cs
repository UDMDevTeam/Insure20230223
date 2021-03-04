using System.Windows.Resources;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
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
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Text;
using Embriant.WPF.Controls;
using Orientation = Infragistics.Documents.Excel.Orientation;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportRedeemedVsPODScreen
    {

        #region Models
        
        private class Batch : ObservableObject
        {
            private long? _batchID;
            public long? BatchID
            {
                get { return _batchID; }
                set { SetProperty(ref _batchID, value, () => BatchID); }
            }

            private string _batchCode;
            public string BatchCode
            {
                get { return _batchCode; }
                set { SetProperty(ref _batchCode, value, () => BatchCode); }
            }

            private bool _select;
            public bool Select
            {
                get { return _select; }
                set { SetProperty(ref _select, value, () => Select); }
            }
        }

        private class ReportDataModel : ObservableObject
        {

            #region Constants

            

            #endregion Constants


            #region Members

            private long? _campaignID;
            public long? CampaignID
            {
                get { return _campaignID; }
                set { SetProperty(ref _campaignID, value, () => CampaignID); }
            }

            private string _campaignName;
            public string CampaignName
            {
                get { return _campaignName; }
                set { SetProperty(ref _campaignName, value, () => CampaignName); }
            }

            private ObservableCollection<Batch> _batches;
            public ObservableCollection<Batch> Batches
            {
                get { return _batches; }
                set { SetProperty(ref _batches, value, () => Batches); }
            }

            private DateTime? _dateFrom;
            public DateTime? DateFrom
            {
                get { return _dateFrom; }
                set { SetProperty(ref _dateFrom, value, () => DateFrom); }
            }

            private DateTime? _dateTo;
            public DateTime? DateTo
            {
                get { return _dateTo; }
                set { SetProperty(ref _dateTo, value, () => DateTo); }
            }

            private bool _isReportRunning;
            public bool IsReportRunning
            {
                get { return _isReportRunning; }
                set { SetProperty(ref _isReportRunning, value, () => IsReportRunning); }
            }

            private bool? _allBatchesAreChecked;
            public bool? AllBatchesAreChecked
            {
                get
                {
                    return _allBatchesAreChecked;
                }
                set
                {
                    if (value != null && Batches != null)
                    {
                        foreach (Batch batch in Batches)
                        {
                            batch.Select = value.Value;
                        }
                    }

                    SetProperty(ref _allBatchesAreChecked, value, () => AllBatchesAreChecked);
                }
            }

            #endregion


            #region Constructor

            public ReportDataModel()
            {
                AllBatchesAreChecked = false;
            }

            #endregion


            #region Public Methods



            #endregion


            #region Private Methods



            #endregion

        }
        
        #endregion



        #region Constants

        

        #endregion



        #region Private Members

        private int _timer1;
        BackgroundWorker _worker;
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();

        private string _selectedBatchIDs;

        private ReportDataModel ReportData { get; } = new ReportDataModel();

        #endregion Private Members



        #region Constructors

        public ReportRedeemedVsPODScreen()
        {
            InitializeComponent();
            DataContext = ReportData;
            LoadLookups();

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
                
                foreach (Batch batch in ReportData.Batches)
                {
                    allSelected = allSelected && batch.Select;
                    noneSelected = noneSelected && !batch.Select;
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

        private void LoadLookups()
        {
            try
            {
                SetCursor(Cursors.Wait);

                string strCampaignIDs = Convert.ToString(Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID=1", IsolationLevel.Snapshot).Rows[0][0]);
                string strSQL = "SELECT ID, Name FROM INCampaign WHERE ID IN (" + strCampaignIDs + ") ORDER BY Name ASC";

                DataTable dtCampaigns = Methods.GetTableData(strSQL, IsolationLevel.Snapshot);
                cmbCampaign.Populate(dtCampaigns, "Name", "ID");
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
        
        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup excel documents
                
                int rowIndexReportStart = 8;
                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                string filePathAndName = String.Format("{0}Redeemed Vs POD Report ({1}) ({2:yyyy-MM-dd} - {3:yyyy-MM-dd})  ~ {4}.xlsx", GlobalSettings.UserFolder,
                                                        ReportData.CampaignName, ReportData.DateFrom, ReportData.DateTo, DateTime.Now.Millisecond);

                Uri uri = new Uri("/Templates/ReportTemplateRedeemedVsPOD.xlsx", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    wbTemplate = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }
                
                #endregion

                #region Report

                #region Summary per Batch

                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@BatchIDList", _selectedBatchIDs);
                parameters[1] = new SqlParameter("@FromDate", ReportData.DateFrom);
                parameters[2] = new SqlParameter("@ToDate", ReportData.DateTo);

                DataSet dsReportSummaryPerBatch = Methods.ExecuteStoredProcedure2("spINReportRedeemedVsPODSummaryPerBatch", parameters, IsolationLevel.Snapshot, 300);
                DataTable dtReportSummaryPerBatch = dsReportSummaryPerBatch.Tables[0];
                DataTable dtReportSummaryPerBatchTotals = dsReportSummaryPerBatch.Tables[1];

                Worksheet wsTemplateReportSummaryPerBatch = wbTemplate.Worksheets["Summary (Batch)"];
                Worksheet wsReportSummaryPerBatch;

                if (dtReportSummaryPerBatch.Rows.Count > 0)
                {
                    wsReportSummaryPerBatch = wbReport.Worksheets.Add("Summary (Batch)");
                    wsReportSummaryPerBatch.DisplayOptions.View = WorksheetView.PageLayout;
                    wsReportSummaryPerBatch.PrintOptions.PaperSize = PaperSize.A4;
                    wsReportSummaryPerBatch.PrintOptions.Orientation = Orientation.Portrait;
                    wsReportSummaryPerBatch.PrintOptions.BottomMargin = 0.4;
                    wsReportSummaryPerBatch.PrintOptions.TopMargin = 0.4;
                    wsReportSummaryPerBatch.PrintOptions.LeftMargin = 0.4;
                    wsReportSummaryPerBatch.PrintOptions.RightMargin = 0.4;
                    wsReportSummaryPerBatch.PrintOptions.ScalingFactor = 90;
                    wsReportSummaryPerBatch.DisplayOptions.MagnificationInPageLayoutView = 100;

                    Methods.CopyExcelRegion(wsTemplateReportSummaryPerBatch, 0, 0, 7, 5, wsReportSummaryPerBatch, 0, 0);

                    wsReportSummaryPerBatch.GetCell("B" + 3).Value = String.Format("{0:yyyy-MM-dd} to {1:yyyy-MM-dd}", ReportData.DateFrom, ReportData.DateTo);
                    wsReportSummaryPerBatch.GetCell("B" + 4).Value = String.Format("{0}", ReportData.CampaignName);

                    int rowIndexData = rowIndexReportStart;

                    foreach (DataRow dr in dtReportSummaryPerBatch.Rows)
                    {
                        Methods.CopyExcelRegion(wsTemplateReportSummaryPerBatch, 7, 0, 0, 5, wsReportSummaryPerBatch, rowIndexData - 1, 0);

                        wsReportSummaryPerBatch.GetCell("A" + rowIndexData).Value = dr["Batch"].ToString();
                        wsReportSummaryPerBatch.GetCell("B" + rowIndexData).Value = dr["Gifts Redeemed"] as int?;
                        wsReportSummaryPerBatch.GetCell("C" + rowIndexData).Value = dr["Gifts Delivered"] as int?;
                        wsReportSummaryPerBatch.GetCell("D" + rowIndexData).Value = dr["% Delivered"] as int?;
                        wsReportSummaryPerBatch.GetCell("E" + rowIndexData).Value = dr["Average Days to Delivery"] as double?;

                        rowIndexData++;
                    }

                    foreach (DataRow dr in dtReportSummaryPerBatchTotals.Rows)
                    {
                        Methods.CopyExcelRegion(wsTemplateReportSummaryPerBatch, 9, 0, 0, 5, wsReportSummaryPerBatch, rowIndexData - 1, 0);

                        wsReportSummaryPerBatch.GetCell("B" + rowIndexData).Value = dr["Total Gifts Redeemed"] as int?;
                        wsReportSummaryPerBatch.GetCell("C" + rowIndexData).Value = dr["Total Gifts Delivered"] as int?;
                        wsReportSummaryPerBatch.GetCell("D" + rowIndexData).Value = dr["Total % Delivered"] as int?;
                        wsReportSummaryPerBatch.GetCell("E" + rowIndexData).Value = dr["Total Average Days to Delivery"] as double?;

                        rowIndexData++;
                    }
                }

                #endregion

                #region Summary per Month

                parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@BatchIDList", _selectedBatchIDs);
                parameters[1] = new SqlParameter("@FromDate", ReportData.DateFrom);
                parameters[2] = new SqlParameter("@ToDate", ReportData.DateTo);

                DataSet dsReportSummaryPerMonth = Methods.ExecuteStoredProcedure2("spINReportRedeemedVsPODSummaryPerMonth", parameters, IsolationLevel.Snapshot, 300);
                DataTable dtReportSummaryPerMonth = dsReportSummaryPerMonth.Tables[0];
                DataTable dtReportSummaryPerMonthTotals = dsReportSummaryPerBatch.Tables[1];

                Worksheet wsTemplateReportSummaryPerMonth = wbTemplate.Worksheets["Summary (Month)"];
                Worksheet wsReportSummaryPerMonth;

                if (dtReportSummaryPerMonth.Rows.Count > 0)
                {
                    wsReportSummaryPerMonth = wbReport.Worksheets.Add("Summary (Month)");
                    wsReportSummaryPerMonth.DisplayOptions.View = WorksheetView.PageLayout;
                    wsReportSummaryPerMonth.PrintOptions.PaperSize = PaperSize.A4;
                    wsReportSummaryPerMonth.PrintOptions.Orientation = Orientation.Portrait;
                    wsReportSummaryPerMonth.PrintOptions.BottomMargin = 0.4;
                    wsReportSummaryPerMonth.PrintOptions.TopMargin = 0.4;
                    wsReportSummaryPerMonth.PrintOptions.LeftMargin = 0.4;
                    wsReportSummaryPerMonth.PrintOptions.RightMargin = 0.4;
                    wsReportSummaryPerMonth.PrintOptions.ScalingFactor = 90;
                    wsReportSummaryPerMonth.DisplayOptions.MagnificationInPageLayoutView = 100;

                    Methods.CopyExcelRegion(wsTemplateReportSummaryPerMonth, 0, 0, 7, 5, wsReportSummaryPerMonth, 0, 0);

                    wsReportSummaryPerMonth.GetCell("B" + 3).Value = String.Format("{0:yyyy-MM-dd} to {1:yyyy-MM-dd}", ReportData.DateFrom, ReportData.DateTo);
                    wsReportSummaryPerMonth.GetCell("B" + 4).Value = String.Format("{0}", ReportData.CampaignName);

                    int rowIndexData = rowIndexReportStart;

                    foreach (DataRow dr in dtReportSummaryPerMonth.Rows)
                    {
                        Methods.CopyExcelRegion(wsTemplateReportSummaryPerMonth, 7, 0, 0, 5, wsReportSummaryPerMonth, rowIndexData - 1, 0);

                        wsReportSummaryPerMonth.GetCell("A" + rowIndexData).Value = dr["Month"].ToString();
                        wsReportSummaryPerMonth.GetCell("B" + rowIndexData).Value = dr["Gifts Redeemed"] as int?;
                        wsReportSummaryPerMonth.GetCell("C" + rowIndexData).Value = dr["Gifts Delivered"] as int?;
                        wsReportSummaryPerMonth.GetCell("D" + rowIndexData).Value = dr["% Delivered"] as int?;
                        wsReportSummaryPerMonth.GetCell("E" + rowIndexData).Value = dr["Average Days to Delivery"] as double?;

                        rowIndexData++;
                    }

                    foreach (DataRow dr in dtReportSummaryPerMonthTotals.Rows)
                    {
                        Methods.CopyExcelRegion(wsTemplateReportSummaryPerMonth, 9, 0, 0, 5, wsReportSummaryPerMonth, rowIndexData - 1, 0);

                        wsReportSummaryPerMonth.GetCell("B" + rowIndexData).Value = dr["Total Gifts Redeemed"] as int?;
                        wsReportSummaryPerMonth.GetCell("C" + rowIndexData).Value = dr["Total Gifts Delivered"] as int?;
                        wsReportSummaryPerMonth.GetCell("D" + rowIndexData).Value = dr["Total % Delivered"] as int?;
                        wsReportSummaryPerMonth.GetCell("E" + rowIndexData).Value = dr["Total Average Days to Delivery"] as double?;

                        rowIndexData++;
                    }
                }

                #endregion


                #region Report

                Worksheet wsTemplateReport = wbTemplate.Worksheets["Report"];
                Worksheet wsReport;

                foreach (Batch batch in ReportData.Batches)
                {
                    if (batch.Select)
                    {
                        #region Get the data

                        parameters = new SqlParameter[3];
                        parameters[0] = new SqlParameter("@BatchID", batch.BatchID);
                        parameters[1] = new SqlParameter("@FromDate", ReportData.DateFrom);
                        parameters[2] = new SqlParameter("@ToDate", ReportData.DateTo);

                        DataSet dsReport = Methods.ExecuteStoredProcedure2("spINReportRedeemedVsPOD", parameters, IsolationLevel.Snapshot, 300);
                        DataTable dtReport = dsReport.Tables[0];

                        #endregion

                        #region Report data

                        if (dtReport.Rows.Count > 0)
                        {
                            wsReport = wbReport.Worksheets.Add(batch.BatchCode);
                            wsReport.DisplayOptions.View = WorksheetView.PageLayout;
                            wsReport.PrintOptions.PaperSize = PaperSize.A4;
                            wsReport.PrintOptions.Orientation = Orientation.Portrait;
                            wsReport.PrintOptions.BottomMargin = 0.4;
                            wsReport.PrintOptions.TopMargin = 0.4;
                            wsReport.PrintOptions.LeftMargin = 0.4;
                            wsReport.PrintOptions.RightMargin = 0.4;
                            wsReport.PrintOptions.ScalingFactor = 90;
                            wsReport.DisplayOptions.MagnificationInPageLayoutView = 100;

                            Methods.CopyExcelRegion(wsTemplateReport, 0, 0, 7, 5, wsReport, 0, 0);

                            wsReport.GetCell("B" + 3).Value = String.Format("{0:yyyy-MM-dd} to {1:yyyy-MM-dd}", ReportData.DateFrom, ReportData.DateTo);
                            wsReport.GetCell("B" + 4).Value = String.Format("{0}", ReportData.CampaignName);
                            wsReport.GetCell("B" + 5).Value = String.Format("{0}", batch.BatchCode);

                            int rowIndexData = rowIndexReportStart;

                            foreach (DataRow dr in dtReport.Rows)
                            {
                                Methods.CopyExcelRegion(wsTemplateReport, 7, 0, 0, 5, wsReport, rowIndexData - 1, 0);

                                wsReport.GetCell("A" + rowIndexData).Value = dr["RefNo"].ToString();
                                wsReport.GetCell("B" + rowIndexData).Value = dr["Sales Agent"].ToString();
                                wsReport.GetCell("C" + rowIndexData).Value = dr["Date Redeemed"].ToString();
                                wsReport.GetCell("D" + rowIndexData).Value = dr["POD Date"].ToString();
                                wsReport.GetCell("E" + rowIndexData).Value = dr["Days to Delivery"] as int?;

                                rowIndexData++;
                            }
                        }

                        #endregion
                    }
                }

                #endregion

                #endregion

                #region Save & Display the resulting workbook

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

                #endregion

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
            ReportData.IsReportRunning = false;

            btnReport.Content = new AccessText
            {
                Text = "_Report",
            };

            //btnReport.Focus();
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

        private void EmbriantComboBoxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            EmbriantComboBox cmbControl = (EmbriantComboBox)sender;

            if (e.Key == Key.Back)
            {
                if (cmbControl.SelectedValue != null)
                {
                    cmbControl.SelectedValue = null;
                    e.Handled = true;
                }
            }
        }
        
        private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportData.AllBatchesAreChecked = AllRecordsSelected();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        
        private void cmbCampaign_Loaded(object sender, RoutedEventArgs e)
        {
            cmbCampaign.Focus();
        }

        private void cmbCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCampaign.SelectedItem != null)
            {
                DataRow drCampaign = (cmbCampaign.SelectedItem as DataRowView).Row;
                ReportData.CampaignName = drCampaign["Name"].ToString();

                StringBuilder strSQL = new StringBuilder();
                strSQL.Append("SELECT ID, Code FROM INBatch ");
                strSQL.Append("WHERE FKINCampaignID = '" + ReportData.CampaignID + "' ");
                strSQL.Append("AND Code LIKE '%_NR%' ");
                strSQL.Append("ORDER BY Code DESC");

                DataTable dtBatch = Methods.GetTableData(strSQL.ToString(), IsolationLevel.Snapshot);

                DataColumn column = new DataColumn("Select", typeof(bool));
                column.DefaultValue = false;
                dtBatch.Columns.Add(column);

                ReportData.Batches = new ObservableCollection<Batch>();  
                for (int i = 0; i < dtBatch.Rows.Count; i++)  
                {  
                    Batch batch = new Batch();
                    batch.BatchID = Convert.ToInt64(dtBatch.Rows[i]["ID"]);
                    batch.BatchCode = Convert.ToString(dtBatch.Rows[i]["Code"]);
                    batch.Select = false;
                    ReportData.Batches.Add(batch);
                }
            }
        }

        private void cmdReport_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!ReportData.IsReportRunning)
            {
                if (ReportData.CampaignID != null)
                {
                    if (ReportData.AllBatchesAreChecked != false)
                    {
                        if (ReportData.DateFrom != null && ReportData.DateTo !=null)
                        {
                            if (ReportData.DateFrom <= ReportData.DateTo)
                            {
                                e.CanExecute = true;
                            }
                        }
                    }
                }
            }
        }

        private void cmdReport_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                ReportData.IsReportRunning = true;

                _selectedBatchIDs = string.Empty;
                foreach (Batch batch in ReportData.Batches)
                {
                    _selectedBatchIDs = _selectedBatchIDs + batch.BatchID + ",";
                }
                _selectedBatchIDs = _selectedBatchIDs.Trim(',');

                _worker = new BackgroundWorker();
                _worker.DoWork += Report;
                _worker.WorkerSupportsCancellation = true;
                _worker.WorkerReportsProgress = true;
                _worker.RunWorkerCompleted += ReportCompleted;

                _worker.RunWorkerAsync();

                dispatcherTimer1.Start();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmdCancel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (ReportData.IsReportRunning)
            {
                e.CanExecute = true;
            }
        }

        private void cmdCancel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _worker.CancelAsync();
            dispatcherTimer1.Stop();
        }
        
        #endregion
        
    }

}
