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
using Infragistics.Windows.DataPresenter.ExcelExporter;
//using Infragistics.Windows;
//using UDM.Insurance.Interface.Data;

namespace UDM.Insurance.Interface.Screens
{

    public partial class ReportConfirmationStatsScreen
    {

        #region Class AgentFilterScreen

        public class AgentFilterScreen
        {
            public string ConsultantName { get; set; }
            public string ReferenceNumber { get; set; }
            public string DateOfSale { get; set; }
            public string ConfWorkDate { get; set; }
            public string Campaign { get; set; }
            public string TSR { get; set; }
            public string Status { get; set; }
            public string ConfirmedStatus { get; set; }
            public long FKINImportID { get; set; }
        }

        #endregion Class AgentFilterScreen

        #region Private Members

        List<AgentFilterScreen> _agentFilters = new List<AgentFilterScreen>();

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        //private RecordCollectionBase _campaigns;
        private DateTime _startDate;
        private DateTime _endDate;
        long UserTypeID;
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;
        private byte _reportScope = 1;
        private bool _insertSingleSheetWithAllData = false;

        private List<Record> _lstSelectedConfirmationAgents;
        private string _fkUserIDs;

        #endregion

        #region Constructors

        public ReportConfirmationStatsScreen(long userTypeID)
        {
            InitializeComponent();
            UserTypeID = userTypeID;
            LoadAgentInfo();

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

                if (xdgAgents.DataSource != null)
                {
                    foreach (DataRow dr in ((DataView)xdgAgents.DataSource).Table.Rows)
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

        private void LoadAgentInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataTable dt = Business.Insure.INGetConfirmationStatisticsReportLookups();
                xdgAgents.DataSource = dt.DefaultView;
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
                        btnCombinedReport.IsEnabled = true;
                        btnBaseReport.IsEnabled = true;
                        btnUpgradeReport.IsEnabled = true;
                        return;
                    }
                }

                btnCombinedReport.IsEnabled = false;
                btnBaseReport.IsEnabled = false;
                btnUpgradeReport.IsEnabled = false;
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
            //btnReport.Content = "Report";

            switch (_reportScope)
            {
                case 1:
                    btnCombinedReport.Content = "Combined Report";
                    break;
                case 2:
                    btnBaseReport.Content = "Base Report";
                    break;
                case 3:
                    btnUpgradeReport.Content = "Upgrade Report";
                    break;
            }

            //btnCombinedReport.IsEnabled = true;
            //btnBaseReport.IsEnabled = true;
            //btnUpgradeReport.IsEnabled = true;
            //btnClose.IsEnabled = true;
            //xdgAgents.IsEnabled = true;
            //calStartDate.IsEnabled = true;
            //calEndDate.IsEnabled = true;

            EnableDisableControls(true);

            dgConfirmationStats.DataSource = _agentFilters;
            dgConfirmationStats.FieldLayouts[0].Fields["FKINImportID"].Visibility = Visibility.Collapsed;
        }

        //private void ReportOLD(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        SetCursor(Cursors.Wait);
                
        //        IEnumerable<DataRecord> agents = e.Argument as IEnumerable<DataRecord>;
        //        string agentIDs = string.Empty;
        //        bool first = true;
        //        if (agents != null)
        //        {
        //            foreach (DataRecord record in agents)
        //            {

        //                if ((bool)record.Cells["Select"].Value)
        //                {
        //                    long agentID = Convert.ToInt32(record.Cells["AgentID"].Value);
        //                    if (first)
        //                    {
        //                        first = false;
        //                        agentIDs = agentID.ToString();
        //                    }
        //                    else
        //                    {
        //                        agentIDs = agentIDs + "," + agentID;
        //                    }

        //                }
        //            }

        //            #region Setup excel documents

        //            Workbook wbTemplate;
        //            Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
        //            string filePathAndName = GlobalSettings.UserFolder + " Confirmation  Report ~ " + DateTime.Now.Millisecond + ".xlsx";

        //            Uri uri = new Uri("/Templates/ReportTemplateConfirmationStats.xlsx", UriKind.Relative);
        //            StreamResourceInfo info = Application.GetResourceStream(uri);
        //            if (info != null)
        //            {
        //                wbTemplate = Workbook.Load(info.Stream, true);
        //            }
        //            else
        //            {
        //                return;
        //            }

        //            Worksheet wsTemplate = wbTemplate.Worksheets["Sheet1"];
        //            Worksheet wsReport = wbReport.Worksheets.Add("Confirmation Report");

        //            wsReport.PrintOptions.PaperSize = PaperSize.A4;
        //            wsReport.PrintOptions.Orientation = Orientation.Portrait;
        //            wsReport.PrintOptions.ScalingType = ScalingType.FitToPages;

        //            #endregion Setup excel documents

        //            #region Get report data from database

        //            DataTable dtLeadAllocationData;
        //            DataTable dtAgentSummary;

        //            SqlParameter[] parameters = new SqlParameter[4];
        //            parameters[0] = new SqlParameter("@AgentIDs", agentIDs);
        //            parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
        //            parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));
        //            parameters[3] = new SqlParameter("@ReportScope", _reportScope);

        //            DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spINReportConfirmationStats", parameters);
        //            if (dsLeadAllocationData.Tables.Count > 0)
        //            {
        //                dtLeadAllocationData = dsLeadAllocationData.Tables[0];
        //                dtAgentSummary = dsLeadAllocationData.Tables[1];
        //                if (dtLeadAllocationData.Rows.Count == 0)
        //                {
        //                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
        //                    {
        //                        ShowMessageBox(new INMessageBoxWindow1(), "There is no data for the report type, the selected confirmation agent(s) and specified date range.", "No Data", ShowMessageType.Information);
        //                    });

        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
        //                {
        //                    ShowMessageBox(new INMessageBoxWindow1(), "There is no data for the report type, the selected confirmation agent(s) and specified date range.", "No Data", ShowMessageType.Information);
        //                });

        //                return;
        //            }

        //            #endregion Get report data from database

        //            Methods.CopyExcelRegion(wsTemplate, 0, 0, dtLeadAllocationData.Rows.Count + 6, 19, wsReport, 0, 0);

        //            #region report data
        //            {
        //                int rowIndex = 7;
        //                wsReport.GetCell("B3").Value = _startDate.ToShortDateString();
        //                wsReport.GetCell("B4").Value = _endDate.ToShortDateString();
        //                List<string> agentSummaryList = new List<string>();
        //                foreach (DataRow rw in dtLeadAllocationData.Rows)
        //                {
        //                    if (rw["ConfirmationConsultant"].ToString().ToLower() == "totals")
        //                    {
        //                        wsReport.GetCell("A" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        wsReport.GetCell("B" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        wsReport.GetCell("C" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        wsReport.GetCell("D" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        wsReport.GetCell("E" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        wsReport.GetCell("F" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        wsReport.GetCell("G" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        wsReport.GetCell("H" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        wsReport.GetCell("I" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        wsReport.GetCell("J" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        wsReport.GetCell("K" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        wsReport.GetCell("L" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        wsReport.GetCell("M" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        wsReport.GetCell("N" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        wsReport.GetCell("O" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        wsReport.GetCell("P" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        wsReport.GetCell("Q" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        wsReport.GetCell("R" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

        //                        // wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        // wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("A" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("A" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        // wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        //wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("B" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("B" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        //wsReport.GetCell("C" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        // wsReport.GetCell("C" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("C" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("C" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        // wsReport.GetCell("D" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        //wsReport.GetCell("D" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("D" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("D" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        // wsReport.GetCell("E" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        // wsReport.GetCell("E" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("E" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("E" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        //wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        // wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("F" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("F" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        //wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        //wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("G" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("G" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        // wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        // wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("H" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("H" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        // wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        // wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("I" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("I" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        //wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        //wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("J" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("J" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        //wsReport.GetCell("K" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        // wsReport.GetCell("K" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("K" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("K" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        //wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        //wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("L" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("L" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        // wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        //wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("M" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("M" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        //wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        // wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("N" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("N" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        //wsReport.GetCell("O" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        // wsReport.GetCell("O" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("O" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("O" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        //wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        // wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("P" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("P" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        //wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        //wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("Q" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("Q" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        //wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                        //wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("R" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                        wsReport.GetCell("R" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
        //                    }
        //                    else
        //                    {
        //                        wsReport.GetCell("A" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("A" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("A" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("A" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsReport.GetCell("B" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("B" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("B" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("B" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsReport.GetCell("C" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("C" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("C" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("C" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsReport.GetCell("D" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("D" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("D" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("D" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsReport.GetCell("E" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("E" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("E" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("E" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsReport.GetCell("F" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("F" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("F" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("F" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsReport.GetCell("G" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("G" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("G" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("G" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsReport.GetCell("H" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("H" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("H" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("H" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsReport.GetCell("I" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("I" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("I" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("I" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsReport.GetCell("J" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("J" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("J" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("J" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsReport.GetCell("K" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("K" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("K" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("K" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsReport.GetCell("L" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("L" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("L" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("L" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsReport.GetCell("M" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("M" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("M" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("M" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsReport.GetCell("N" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("N" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("N" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("N" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsReport.GetCell("O" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("O" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("O" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("O" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsReport.GetCell("P" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("P" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("P" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("P" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsReport.GetCell("Q" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("Q" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("Q" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("Q" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsReport.GetCell("R" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("R" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("R" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsReport.GetCell("R" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
        //                    }

        //                    // Methods.CopyExcelRegion(wsTemplate, rowIndex - 1, 0, 1, 19, wsReport, rowIndex - 1, 0);

        //                    wsReport.GetCell("A" + rowIndex).Value = rw["ConfirmationConsultant"].ToString();
        //                    wsReport.GetCell("B" + rowIndex).Value = rw["TotalPoliciesReceived"].ToString();
        //                    wsReport.GetCell("C" + rowIndex).Value = rw["TotalCancellations"].ToString();
        //                    wsReport.GetCell("D" + rowIndex).Value = rw["Cancellation%"] + "%";
        //                    wsReport.GetCell("E" + rowIndex).Value = rw["TotalCarriedForwards"].ToString();
        //                    wsReport.GetCell("F" + rowIndex).Value = rw["CarriedForward%"] + "%";
        //                    wsReport.GetCell("G" + rowIndex).Value = rw["PLCancellations"].ToString();
        //                    wsReport.GetCell("H" + rowIndex).Value = rw["PLCancellations%"].ToString();
        //                    wsReport.GetCell("I" + rowIndex).Value = rw["TotalPoliciesAfterFallOff"].ToString();
        //                    wsReport.GetCell("J" + rowIndex).Value = rw["TotalPoliciesConfirmed"].ToString();
        //                    wsReport.GetCell("K" + rowIndex).Value = rw["PercentageConfirmed"] + "%";
        //                    wsReport.GetCell("L" + rowIndex).Value = rw["TotalPoliciesNotConfirmed"].ToString();
        //                    wsReport.GetCell("M" + rowIndex).Value = rw["TotalPoliciesNotConfirmed%"] + "%";
        //                    wsReport.GetCell("N" + rowIndex).Value = rw["OverUnder"].ToString();
        //                    wsReport.GetCell("O" + rowIndex).Value = rw["TotalHours"].ToString();
        //                    wsReport.GetCell("P" + rowIndex).Value = rw["NumberOfConfirmsPerHour"].ToString();
        //                    wsReport.GetCell("Q" + rowIndex).Value = rw["TotalErrors"].ToString();
        //                    wsReport.GetCell("R" + rowIndex).Value = rw["Error%"].ToString();


        //                    agentSummaryList.Add(rw["ConfirmationConsultant"].ToString());

        //                    rowIndex++;

        //                }
        //                //foreach agent summary add worksheet
        //                foreach (string agentName in agentSummaryList)
        //                {
        //                    int agentRowIndex = 1;
        //                    Worksheet wsagentSummary = wbReport.Worksheets.Add(agentName);
        //                    var agentData = dtAgentSummary.AsEnumerable().Where(x => (string)x["ConsultantName"] == agentName);

        //                    wsagentSummary.GetCell("A1").Value = "Consultant Name";
        //                    wsagentSummary.GetCell("B1").Value = "Reference Number";
        //                    wsagentSummary.GetCell("C1").Value = "Date Of Sale";
        //                    wsagentSummary.GetCell("D1").Value = "Conf Work Date";
        //                    wsagentSummary.GetCell("E1").Value = "Campaign";
        //                    wsagentSummary.GetCell("F1").Value = "TSR";
        //                    wsagentSummary.GetCell("G1").Value = "Status";
        //                    wsagentSummary.GetCell("H1").Value = "Confirmed Status";
        //                    wsagentSummary.GetCell("I1").Value = "Bump-up / Reduced Premium Status";
        //                    wsagentSummary.GetCell("J1").Value = "Old Premium";
        //                    wsagentSummary.GetCell("K1").Value = "New Premium";

        //                    wsagentSummary.GetCell("A1").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                    wsagentSummary.GetCell("B1").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                    wsagentSummary.GetCell("C1").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                    wsagentSummary.GetCell("D1").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                    wsagentSummary.GetCell("E1").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                    wsagentSummary.GetCell("F1").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                    wsagentSummary.GetCell("G1").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                    wsagentSummary.GetCell("H1").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                    wsagentSummary.GetCell("I1").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                    wsagentSummary.GetCell("J1").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                    wsagentSummary.GetCell("K1").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

        //                    wsagentSummary.Columns[0].Width = 6100;
        //                    wsagentSummary.Columns[1].Width = 6100;
        //                    wsagentSummary.Columns[2].Width = 5000;
        //                    wsagentSummary.Columns[3].Width = 5000;
        //                    wsagentSummary.Columns[4].Width = 6100;
        //                    wsagentSummary.Columns[5].Width = 5000;
        //                    wsagentSummary.Columns[6].Width = 5000;
        //                    wsagentSummary.Columns[7].Width = 5000;
        //                    wsagentSummary.Columns[8].Width = 9000;
        //                    wsagentSummary.Columns[9].Width = 4000;
        //                    wsagentSummary.Columns[10].Width = 4000;

        //                    #region Header formatting
        //                    wsagentSummary.GetCell("A" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("A" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("A" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("A" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

        //                    wsagentSummary.GetCell("B" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("B" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("B" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("B" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

        //                    wsagentSummary.GetCell("C" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("C" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("C" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("C" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

        //                    wsagentSummary.GetCell("D" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("D" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("D" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("D" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

        //                    wsagentSummary.GetCell("E" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("E" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("E" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("E" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

        //                    wsagentSummary.GetCell("F" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("F" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("F" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("F" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

        //                    wsagentSummary.GetCell("G" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("G" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("G" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("G" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

        //                    wsagentSummary.GetCell("H" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("H" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("H" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("H" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

        //                    wsagentSummary.GetCell("I" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("I" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("I" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("I" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

        //                    wsagentSummary.GetCell("J" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("J" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("J" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("J" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

        //                    wsagentSummary.GetCell("K" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("K" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("K" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
        //                    wsagentSummary.GetCell("K" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

        //                    #endregion header formatting
        //                    agentRowIndex++;

        //                    foreach (var agentRow in agentData)
        //                    {
        //                        wsagentSummary.GetCell("A" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("A" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("A" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("A" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsagentSummary.GetCell("B" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("B" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("B" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("B" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsagentSummary.GetCell("C" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("C" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("C" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("C" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsagentSummary.GetCell("D" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("D" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("D" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("D" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsagentSummary.GetCell("E" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("E" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("E" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("E" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsagentSummary.GetCell("F" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("F" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("F" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("F" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsagentSummary.GetCell("G" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("G" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("G" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("G" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsagentSummary.GetCell("H" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("H" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("H" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("H" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsagentSummary.GetCell("I" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("I" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("I" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("I" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsagentSummary.GetCell("J" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("J" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("J" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("J" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsagentSummary.GetCell("K" + agentRowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("K" + agentRowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("K" + agentRowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
        //                        wsagentSummary.GetCell("K" + agentRowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

        //                        wsagentSummary.GetCell("A" + agentRowIndex).Value = agentRow["ConsultantName"].ToString();
        //                        wsagentSummary.GetCell("B" + agentRowIndex).Value = agentRow["RefNo"].ToString();
        //                        wsagentSummary.GetCell("C" + agentRowIndex).Value = agentRow["DateOfSale"].ToString();
        //                        wsagentSummary.GetCell("D" + agentRowIndex).Value = agentRow["ConfWorkDate"].ToString();
        //                        wsagentSummary.GetCell("E" + agentRowIndex).Value = agentRow["CampaignCode"].ToString();
        //                        wsagentSummary.GetCell("F" + agentRowIndex).Value = agentRow["TSR"].ToString();
        //                        wsagentSummary.GetCell("G" + agentRowIndex).Value = agentRow["Status"].ToString();
        //                        wsagentSummary.GetCell("H" + agentRowIndex).Value = agentRow["ConfirmedStatus"].ToString();
        //                        wsagentSummary.GetCell("I" + agentRowIndex).Value = agentRow["BumpUpReducedPremiumOption"].ToString();
        //                        wsagentSummary.GetCell("J" + agentRowIndex).Value = agentRow["OldPremium"].ToString();
        //                        wsagentSummary.GetCell("K" + agentRowIndex).Value = agentRow["NewPremium"].ToString();

        //                        AgentFilterScreen agentFilter = new AgentFilterScreen();
        //                        agentFilter.ConsultantName = agentRow["ConsultantName"].ToString();
        //                        agentFilter.ReferenceNumber = agentRow["RefNo"].ToString();
        //                        agentFilter.DateOfSale = agentRow["DateOfSale"].ToString();
        //                        agentFilter.ConfWorkDate = agentRow["ConfWorkDate"].ToString();
        //                        agentFilter.Campaign = agentRow["CampaignCode"].ToString();
        //                        agentFilter.TSR = agentRow["TSR"].ToString();
        //                        agentFilter.Status = agentRow["Status"].ToString();
        //                        agentFilter.ConfirmedStatus = agentRow["ConfirmedStatus"].ToString();

        //                        _agentFilters.Add(agentFilter);

        //                        agentRowIndex++;
        //                    }
        //                }
        //            }

        //            #endregion

        //            //Save excel document
        //            wbReport.Save(filePathAndName);

        //            //Display excel document
        //            Process.Start(filePathAndName);

        //        }
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

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup excel documents

                string reportScopeIdentifier = String.Empty;
                switch (_reportScope)
                {
                    case 1:
                        reportScopeIdentifier = "Combined";
                        break;
                    case 2:
                        reportScopeIdentifier = "Base";
                        break;
                    case 3:
                        reportScopeIdentifier = "Upgrade";
                        break;
                }

                string filePathAndName = String.Empty;
                if (_startDate == _endDate)
                {
                    filePathAndName = $"{GlobalSettings.UserFolder}Bump-up Statistics Report - {reportScopeIdentifier} ({_startDate.ToString("yyyy-MM-dd")}), {DateTime.Now.ToString("yyyy-MM-dd HHmmdd")}.xlsx";
                }
                else
                {
                    filePathAndName = $"{GlobalSettings.UserFolder}Bump-up Statistics Report - {reportScopeIdentifier} ({_startDate.ToString("yyyy-MM-dd")} - {_endDate.ToString("yyyy-MM-dd")}), {DateTime.Now.ToString("yyyy-MM-dd HHmmdd")}.xlsx";
                }

                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateConfirmationStatsNew.xlsx");
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup excel documents

                #region Get the data

                DataSet dsConfirmationStatsReportData = Business.Insure.INReportConfirmationStats(_fkUserIDs, _startDate, _endDate, _reportScope);

                if (dsConfirmationStatsReportData.Tables[0].Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                #endregion Get the data

                #region Insert the summary sheet

                InsertConfirmationStatsReportSummarySheet(wbTemplate, wbReport, dsConfirmationStatsReportData);

                #endregion Insert the summary sheet

                #region Insert the cumulative sheet containing all confirmation agents' data

                if (_insertSingleSheetWithAllData)
                {
                    InsertCumulativeStatsSheet(wbTemplate, wbReport, dsConfirmationStatsReportData);
                }

                #endregion Insert the cumulative sheet containing all confirmation agents' data

                #region Insert each confirmation agent's worksheet

                foreach (DataRow row in dsConfirmationStatsReportData.Tables[0].Rows)
                {
                    InsertIndividualConfirmationStatsReportSheet(wbTemplate, wbReport, dsConfirmationStatsReportData, row);
                }

                #endregion Insert each confirmation agent's worksheet

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

        private void InsertConfirmationStatsReportSummarySheet(Workbook wbTemplate, Workbook wbReport, DataSet dsConfirmationStatsReportData)
        {
            if (dsConfirmationStatsReportData.Tables[0].Rows.Count > 0)
            {
                #region Partition the given dataset

                //string orderByString = drCurrentConfirmationAgent["OrderByString"].ToString();
                DataTable dtSummarySheetData = dsConfirmationStatsReportData.Tables[0];
                DataTable dtExcelSheetDataTableColumnMappings = dsConfirmationStatsReportData.Tables[1];
                DataTable dtExcelSheetTotalsAndAverageColumnMappings = dsConfirmationStatsReportData.Tables[2];

                #endregion Partition the given dataset

                #region Declarations & Initializations

                int reportRow = 6;
                int formulaStartRow = reportRow;

                byte templateDataSheetRowSpan = 5;
                byte templateColumnSpan = 21;
                byte templateRowIndex = 6;
                byte totalsTemplateRowIndex = 7;

                string reportScopeIdentifier = String.Empty;
                switch (_reportScope)
                {
                    case 1:
                        reportScopeIdentifier = "Combined";
                        break;
                    case 2:
                        reportScopeIdentifier = "Base";
                        break;
                    case 3:
                        reportScopeIdentifier = "Upgrade";
                        break;
                }

                string reportHeadingCell = "A1";
                string reportSubHeadingCell = "A3";
                string reportDateCell = "U4";

                string reportTitle = String.Format("{0} Bump-up Statistics Report - Summary", reportScopeIdentifier);
                string reportSubTitle = dsConfirmationStatsReportData.Tables[0].Rows[0]["ReportSubTitle"].ToString();

                #endregion Declarations & Initializations

                #region Add the worksheet

                Worksheet wsReportTemplate = wbTemplate.Worksheets["Summary"];
                Worksheet wsReport = wbReport.Worksheets.Add("Summary");
                Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                wsReport.Columns[15].Hidden = true;
                wsReport.Columns[16].Hidden = true;
                wsReport.Columns[17].Hidden = true;
                wsReport.Columns[18].Hidden = true;
                wsReport.Columns[19].Hidden = true;
                wsReport.Columns[20].Hidden = true;
                wsReport.Columns[21].Hidden = true;

                #endregion Add the worksheet

                #region Populating the report details

                Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
                wsReport.GetCell(reportHeadingCell).Value = reportTitle;
                wsReport.GetCell(reportSubHeadingCell).Value = reportSubTitle;
                wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                #endregion Populating the report details

                #region Add the data

                reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtSummarySheetData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);
                
                #endregion Add the data

                #region Add the totals / averages

                reportRow = Methods.MapTemplatizedExcelFormulas(wsReportTemplate, dtExcelSheetTotalsAndAverageColumnMappings, totalsTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0, formulaStartRow, reportRow - 1);

                #endregion Add the totals / averages
            }
        }

        private void InsertCumulativeStatsSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsConfirmationStatsReportData)
        {
            if (dsConfirmationStatsReportData.Tables[3].Rows.Count > 0)
            {
                #region Partition the given dataset

                string orderByString = dsConfirmationStatsReportData.Tables[0].Rows[0]["OrderByString"].ToString();
                DataTable dtCumulativeConfirmationAgentData = dsConfirmationStatsReportData.Tables[3];
                DataTable dtExcelSheetDataTableColumnMappings = dsConfirmationStatsReportData.Tables[4];

                #endregion Partition the given dataset

                #region Declarations & Initializations

                //int reportRow = 6;

                //byte templateDataSheetRowSpan = 5;
                //byte templateColumnSpan = 17;
                //byte templateRowIndex = 6;

                byte templateDataSheetRowSpan = 4;
                byte templateColumnSpan = 18;
                byte templateRowIndex = 7;

                byte templateReportDataColumnHeadingsRowIndex = 6;
                byte templateReportDataColumnHeadingsRowSpan = 1;

                int reportRow = 5;

                string worksheetTabName = "All Confirmation Agents";
                string campaignDataSheetTemplateName = "Report";

                string reportHeadingCell = "A1";
                string reportSubHeadingCell = "A3";
                string reportDateCell = "R4";

                string nonConfirmedFilterString = "[IsConfirmed] = 0";
                string confirmedFilterString = "[IsConfirmed] = 1";

                string reportScopeIdentifier = String.Empty;
                switch (_reportScope)
                {
                    case 1:
                        reportScopeIdentifier = "Combined";
                        break;
                    case 2:
                        reportScopeIdentifier = "Base";
                        break;
                    case 3:
                        reportScopeIdentifier = "Upgrade";
                        break;
                }

                string reportTitle = String.Format("{0} Bump-up Statistics Report - All Confirmation Agents", reportScopeIdentifier);
                string reportSubTitle = dsConfirmationStatsReportData.Tables[0].Rows[0]["ReportSubTitle"].ToString();

                #endregion Declarations & Initializations

                #region Add the worksheet

                Worksheet wsReportTemplate = wbTemplate.Worksheets[campaignDataSheetTemplateName];
                Worksheet wsReport = wbReport.Worksheets.Add(worksheetTabName);
                Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                wsReport.Columns[8].Hidden = true;

                #endregion Add the worksheet

                #region Populating the report details

                Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
                wsReport.GetCell(reportHeadingCell).Value = reportTitle;
                wsReport.GetCell(reportSubHeadingCell).Value = reportSubTitle;
                wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                #endregion Populating the report details

                #region Add the lead data

                Methods.CopyExcelRegion(wsReportTemplate, templateReportDataColumnHeadingsRowIndex, 0, templateReportDataColumnHeadingsRowSpan, templateColumnSpan, wsReport, reportRow, 0);
                reportRow++;

                reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCumulativeConfirmationAgentData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);
                reportRow++;

                #endregion

                #region Add the non-confirmed lead data

                //var filteredNonConfirmedRows = dtCumulativeConfirmationAgentData.Select(nonConfirmedFilterString).AsEnumerable();
                //if (filteredNonConfirmedRows.Any())
                //{
                //    //Insert the columm headings:
                //    Methods.CopyExcelRegion(wsReportTemplate, templateReportDataColumnHeadingsRowIndex, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, reportRow, 0);
                //    wsReport.GetCell(String.Format("A{0}", reportRow + 1)).Value = "Non-Confirmed Policies";
                //    reportRow += 2;

                //    // Get the data
                //    DataTable dtCurrentConfirmationAgentNonConfirmedData = dtCumulativeConfirmationAgentData.Select(nonConfirmedFilterString, orderByString).CopyToDataTable();

                //    reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentConfirmationAgentNonConfirmedData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);
                //    reportRow++;
                //}

                #endregion Add the non-confirmed lead data

                #region Add the confirmed lead data

                //var filteredConfirmedRows = dtCumulativeConfirmationAgentData.Select(nonConfirmedFilterString).AsEnumerable();
                //if (filteredConfirmedRows.Any())
                //{
                //    //Insert the columm headings:
                //    Methods.CopyExcelRegion(wsReportTemplate, templateReportDataColumnHeadingsRowIndex, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, reportRow, 0);
                //    wsReport.GetCell(String.Format("A{0}", reportRow + 1)).Value = "Confirmed Policies";
                //    reportRow += 2;

                //    // Get the data
                //    DataTable dtCurrentConfirmationAgentConfirmedData = dtCumulativeConfirmationAgentData.Select(confirmedFilterString, orderByString).CopyToDataTable();

                //    // Insert the data
                //    reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentConfirmationAgentConfirmedData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);
                //    reportRow++;
                //}

                #endregion Add the non-confirmed lead data

                //#region Add the data

                //reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentConfirmationAgentData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                //#endregion Add the data

            }
        }

        private void InsertIndividualConfirmationStatsReportSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsConfirmationStatsReportData, DataRow drCurrentConfirmationAgent)
        {
            string filterString = drCurrentConfirmationAgent["FilterString"].ToString();

            var filteredRows = dsConfirmationStatsReportData.Tables[3].Select(filterString).AsEnumerable();
            if (filteredRows.Any())
            {
                #region Partition the given dataset

                string orderByString = drCurrentConfirmationAgent["OrderByString"].ToString();
                DataTable dtCurrentConfirmationAgentData = dsConfirmationStatsReportData.Tables[3].Select(filterString, orderByString).CopyToDataTable();
                DataTable dtExcelSheetDataTableColumnMappings = dsConfirmationStatsReportData.Tables[4];

                #endregion Partition the given dataset

                #region Declarations & Initializations

                byte templateDataSheetRowSpan = 4;
                byte templateColumnSpan = 18;
                byte templateRowIndex = 7;

                byte templateReportDataColumnHeadingsRowIndex = 6;
                byte templateReportDataColumnHeadingsRowSpan = 1;

                int reportRow = 5;

                string confirmationConsultant = drCurrentConfirmationAgent["ConfirmationConsultant"].ToString();
                string worksheetTabName = Methods.ParseWorksheetName(wbReport, confirmationConsultant);
                string campaignDataSheetTemplateName = "Report";

                string reportHeadingCell = "A1";        //dtReportConfigs.Rows[0]["ReportHeadingCell"].ToString();
                string reportSubHeadingCell = "A3";     //dtReportConfigs.Rows[0]["ReportSubHeadingCell"].ToString();
                string reportDateCell = "R4";           //dtReportConfigs.Rows[0]["ReportDateCell"].ToString();

                string reportTitle = drCurrentConfirmationAgent["ReportTitle"].ToString();
                string reportSubTitle = drCurrentConfirmationAgent["ReportSubTitle"].ToString();

                string nonConfirmedFilterString = drCurrentConfirmationAgent["NonConfirmedFilterString"].ToString();
                string confirmedFilterString = drCurrentConfirmationAgent["ConfirmedFilterString"].ToString();
                
                #endregion Declarations & Initializations

                #region Add the worksheet

                Worksheet wsReportTemplate = wbTemplate.Worksheets[campaignDataSheetTemplateName];
                Worksheet wsReport = wbReport.Worksheets.Add(worksheetTabName);
                Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                wsReport.Columns[8].Hidden = true;

                #endregion Add the worksheet

                #region Populating the report details

                Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
                wsReport.GetCell(reportHeadingCell).Value = reportTitle;
                wsReport.GetCell(reportSubHeadingCell).Value = reportSubTitle;
                wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                #endregion Populating the report details

                #region Add the lead data

                Methods.CopyExcelRegion(wsReportTemplate, templateReportDataColumnHeadingsRowIndex, 0, templateReportDataColumnHeadingsRowSpan, templateColumnSpan, wsReport, reportRow, 0);
                reportRow++;

                reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentConfirmationAgentData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);
                reportRow++;

                #endregion

                #region Add the non-confirmed lead data

                //var filteredNonConfirmedRows = dtCurrentConfirmationAgentData.Select(nonConfirmedFilterString).AsEnumerable();
                //if (filteredNonConfirmedRows.Any())
                //{
                //    //Insert the columm headings:
                //    Methods.CopyExcelRegion(wsReportTemplate, templateReportDataColumnHeadingsRowIndex, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, reportRow, 0);
                //    wsReport.GetCell(String.Format("A{0}", reportRow + 1)).Value = "Non-Confirmed Policies";
                //    reportRow += 2;

                //    // Get the data
                //    DataTable dtCurrentConfirmationAgentNonConfirmedData = dtCurrentConfirmationAgentData.Select(nonConfirmedFilterString, orderByString).CopyToDataTable();

                //    reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentConfirmationAgentNonConfirmedData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);
                //    reportRow++;
                //}

                #endregion Add the non-confirmed lead data

                #region Add the confirmed lead data

                //var filteredConfirmedRows = dtCurrentConfirmationAgentData.Select(confirmedFilterString).AsEnumerable();
                //if (filteredConfirmedRows.Any())
                //{
                //    //Insert the columm headings:
                //    Methods.CopyExcelRegion(wsReportTemplate, templateReportDataColumnHeadingsRowIndex, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, reportRow, 0);
                //    wsReport.GetCell(String.Format("A{0}", reportRow + 1)).Value = "Confirmed Policies";
                //    reportRow += 2;

                //    // Get the data
                //    DataTable dtCurrentConfirmationAgentConfirmedData = dtCurrentConfirmationAgentData.Select(confirmedFilterString, orderByString).CopyToDataTable();

                //    // Insert the data
                //    reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentConfirmationAgentConfirmedData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);
                //    reportRow++;
                //}

                #endregion Add the non-confirmed lead data

                #region Populate _agentFilters

                foreach (DataRow agentRow in dtCurrentConfirmationAgentData.Rows)
                {
                    AgentFilterScreen agentFilter = new AgentFilterScreen();
                    agentFilter.ConsultantName = agentRow["ConfirmationConsultant1"].ToString();
                    agentFilter.ReferenceNumber = agentRow["RefNo"].ToString();
                    agentFilter.DateOfSale = agentRow["DateOfSale"].ToString();
                    agentFilter.ConfWorkDate = agentRow["ConfWorkDate"].ToString();
                    agentFilter.Campaign = agentRow["CampaignCode"].ToString();
                    agentFilter.TSR = agentRow["SalesConsultant"].ToString();
                    agentFilter.Status = agentRow["LeadStatus"].ToString();
                    agentFilter.ConfirmedStatus = agentRow["ConfirmedStatus"].ToString();
                    agentFilter.FKINImportID = Convert.ToInt64(agentRow["FKINImportID"]);

                    _agentFilters.Add(agentFilter);
                }

                #endregion Populate _agentFilters
            }
        }

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;

            switch (_reportScope)
            {
                case 1:
                    btnCombinedReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
                    btnCombinedReport.ToolTip = btnCombinedReport.Content;
                    break;
                case 2:
                    btnBaseReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
                    btnBaseReport.ToolTip = btnBaseReport.Content;
                    break;
                case 3:
                    btnUpgradeReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
                    btnUpgradeReport.ToolTip = btnUpgradeReport.Content;
                    break;
            }
        }

        #endregion Private Methods

        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        public void EnableDisableControls(bool enabled)
        {
            btnClose.IsEnabled = enabled;
            btnCombinedReport.IsEnabled = enabled;
            btnBaseReport.IsEnabled = enabled;
            btnUpgradeReport.IsEnabled = enabled;
            xdgAgents.IsEnabled = enabled;
            calStartDate.IsEnabled = enabled;
            calEndDate.IsEnabled = enabled;
            btnExportToExcel.IsEnabled = enabled;
            btnLoadLeads.IsEnabled = enabled;
        }

        public void PreReportGenerationOperations(byte reportScope)
        {

            var lstTemp = (from r in xdgAgents.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            _lstSelectedConfirmationAgents = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["AgentName"].Value));

            if (_lstSelectedConfirmationAgents.Count == 0)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 confirmation agent from the list.", "No confirmation agent selected", ShowMessageType.Error);
                return;
            }
            else
            {
                _fkUserIDs = _lstSelectedConfirmationAgents.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["AgentID"].Value + ",");
                _fkUserIDs = _fkUserIDs.Substring(0, _fkUserIDs.Length - 1);
            }

            if (chkIncludeCumulativeSheet.IsChecked.HasValue)
            {
                if (chkIncludeCumulativeSheet.IsChecked.Value)
                {
                    _insertSingleSheetWithAllData = true;
                }
                else
                {
                    _insertSingleSheetWithAllData = false;
                }
            }
            else
            {
                _insertSingleSheetWithAllData = false;
            }

            EnableDisableControls(false);

            _reportScope = reportScope;
        }

        private void btnCombinedReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PreReportGenerationOperations(1);

                //IEnumerable<DataRecord> campaigns = xdgAgents.Records.Cast<DataRecord>().ToArray();

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

        private void btnBaseReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PreReportGenerationOperations(2);

                //IEnumerable<DataRecord> campaigns = xdgAgents.Records.Cast<DataRecord>().ToArray();

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
      
        private void btnUpgradeReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PreReportGenerationOperations(3);

                //IEnumerable<DataRecord> campaigns = xdgAgents.Records.Cast<DataRecord>().ToArray();

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

        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = ((DataView)xdgAgents.DataSource).Table;

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
                DataTable dt = ((DataView)xdgAgents.DataSource).Table;

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

        private void btnExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            //string filePathAndName = GlobalSettings.UserFolder + "Agent Summary  Report ~ " + DateTime.Now.Millisecond + ".xlsx";
            string filePathAndName = String.Format("{0}Agent Summary Report ~ {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmss")); //GlobalSettings.UserFolder + "Agent Summary Report ~ " + DateTime.Now.Millisecond + ".xlsx";
            DataPresenterExcelExporter exporter = new DataPresenterExcelExporter();
            exporter.Export(dgConfirmationStats, filePathAndName, WorkbookFormat.Excel2007);
            Process.Start(filePathAndName);
        }
        private void dgConfirmationStats_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRecordCellArea drca = Infragistics.Windows.Utilities.GetAncestorFromType(e.OriginalSource as DependencyObject, typeof(DataRecordCellArea), false) as DataRecordCellArea;

            if (drca != null)
            {
                if (dgConfirmationStats.ActiveRecord != null && dgConfirmationStats.ActiveRecord.FieldLayout.Description == "AgentFilterScreen")
                {
                    DataRecord record = (DataRecord)dgConfirmationStats.ActiveRecord;

                    long? fkINImportID = Int64.Parse(record.Cells["FKINImportID"].Value.ToString());

                    #region Determining whether or not the lead was allocated

                    bool hasLeadBeenAllocated = Business.Insure.HasLeadBeenAllocated(fkINImportID);

                    if (!hasLeadBeenAllocated)
                    {
                        INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                        ShowMessageBox(messageWindow, @"This lead cannot be loaded, because it has not been allocated yet. Please consult your supervisor.", "Lead not allocated", Embriant.Framework.ShowMessageType.Exclamation);
                        return;
                    }

                    #endregion Determining whether or not the lead was allocated

                    #region Checking if the lead to be loaded has a status of "DO NOT CONTACT"

                    bool clientCanBeContacted = Business.Insure.CanClientBeContacted(fkINImportID);

                    if (!clientCanBeContacted)
                    {
                        INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                        ShowMessageBox(messageWindow, @"This lead cannot be loaded in its entirety, because the client has requested not to be contacted again.", "DO NOT CONTACT CLIENT", Embriant.Framework.ShowMessageType.Exclamation);
                    }

                    #endregion Checking if the lead to be loaded has a status of "DO NOT CONTACT"

                    #region Determining whether or not the lead has a status of cancelled
                    // Please see https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/204734160/comments
                    // Please see https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/211742618/comments

                    //bool hasLeadBeenCancelled = Business.Insure.HasLeadBeenCancelled(fkINImportID);

                    //if (hasLeadBeenCancelled)
                    //{
                    //    INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                    //    ShowMessageBox(messageWindow, @"This lead cannot be loaded, because the policy has been cancelled by the client. Please consult your supervisor.", "Cancelled Policy", Embriant.Framework.ShowMessageType.Exclamation);
                    //    return;
                    //}

                    #endregion Determining whether or not the lead has a status of cancelled

                    LeadApplicationScreen leadApplicationScreen = new LeadApplicationScreen(long.Parse(record.Cells["FKINImportID"].Value.ToString()), new Data.SalesScreenGlobalData());
                    ShowDialog(leadApplicationScreen, new INDialogWindow(leadApplicationScreen));
                    //leadApplicationScreen.ShowNotes(Int64.Parse(record.Cells["ImportID"].Value.ToString()));
                }
            }
        }

        private void btnLoadLeads_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                PreReportGenerationOperations(1);

                DataSet dsConfirmationStatsReportData = Business.Insure.INReportConfirmationStats(_fkUserIDs, _startDate, _endDate, _reportScope);

                #region Populate _agentFilters

                if (dsConfirmationStatsReportData.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow agentRow in dsConfirmationStatsReportData.Tables[3].Rows)
                    {
                        AgentFilterScreen agentFilter = new AgentFilterScreen();
                        agentFilter.ConsultantName = agentRow["ConfirmationConsultant1"].ToString();
                        agentFilter.ReferenceNumber = agentRow["RefNo"].ToString();
                        agentFilter.DateOfSale = agentRow["DateOfSale"].ToString();
                        agentFilter.ConfWorkDate = agentRow["ConfWorkDate"].ToString();
                        agentFilter.Campaign = agentRow["CampaignCode"].ToString();
                        agentFilter.TSR = agentRow["SalesConsultant"].ToString();
                        agentFilter.Status = agentRow["LeadStatus"].ToString();
                        agentFilter.ConfirmedStatus = agentRow["ConfirmedStatus"].ToString();
                        agentFilter.FKINImportID = Convert.ToInt64(agentRow["FKINImportID"]);

                        _agentFilters.Add(agentFilter);
                    }

                    dgConfirmationStats.DataSource = _agentFilters;
                    dgConfirmationStats.FieldLayouts[0].Fields["FKINImportID"].Visibility = Visibility.Collapsed;
                }

                #endregion Populate _agentFilters

                EnableDisableControls(true);
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

        #endregion Event Handlers

    }
}
