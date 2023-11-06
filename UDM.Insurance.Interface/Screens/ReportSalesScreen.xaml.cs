using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Resources;
using System.Windows.Threading;
using Embriant.Framework.Configuration;
using Embriant.WPF.Controls;
using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.Editors.Events;
using UDM.Insurance.Business;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{

    public partial class ReportSalesScreen : INotifyPropertyChanged
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

        #endregion



        #region Properties

        private bool? _allRecordsChecked = false;
        public bool? AllRecordsChecked
        {
            get
            {
                return _allRecordsChecked;
            }
            set
            {
                _allRecordsChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllRecordsChecked"));
            }
        }

        private bool? _IsReportRunning = false;
        public bool? IsReportRunning
        {
            get
            {
                return _IsReportRunning;
            }
            set
            {
                _IsReportRunning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReportRunning"));
            }
        }

        private ReportSalesScreenData _screenData = new ReportSalesScreenData();
        public ReportSalesScreenData ScreenData
        {
            get { return _screenData; }
            set { _screenData = value; }
        }

        private lkpAgentMode? _agentMode = lkpAgentMode.Both;
        public lkpAgentMode? AgentMode
        {
            get { return _agentMode; }
            set { _agentMode = value; }
        }

        private lkpINCampTSRReportMode? _reportMode = lkpINCampTSRReportMode.ByTSR;
        public lkpINCampTSRReportMode? ReportMode
        {
            get { return _reportMode; }
            set { _reportMode = value; }
        }

        #endregion



        #region Constants

        //private string _fontName = "Calibri";
        //private const int _fontSize = 10;
        //private const int _pointsToTwipsFactor = 20;
        //private const int _fontHeight = _fontSize * _pointsToTwipsFactor; 

        #endregion



        #region Private Members

        //private DataRowView _selectedCampaign;
        private List<DataRecord> _selectedAgents;
        private DateTime _fromDate = DateTime.Now;
        private DateTime _toDate = DateTime.Now;
        private readonly User _user = ((User)GlobalSettings.ApplicationUser);

        private readonly DispatcherTimer _reportTimer = new DispatcherTimer();
        private int _seconds;
        private string _agentName;

        #endregion



        #region Constructors

        public ReportSalesScreen()
        {
            InitializeComponent();
            LoadAgentInfo();

            lblFoundation.Visibility = Visibility.Collapsed;
            lblPrePerm.Visibility = Visibility.Collapsed;

            chkFoundation.Visibility = Visibility.Collapsed;
            chkPrePerm.Visibility = Visibility.Collapsed;

            _reportTimer.Tick += ReportTimer;
            _reportTimer.Interval = new TimeSpan(0, 0, 1);

            //PropertyChanged += ReportSalesScreen_PropertyChanged;

            if((lkpUserType?)((Business.User)GlobalSettings.ApplicationUser).FKUserType == lkpUserType.SalesAgent)
            {
                DebiCheckCB.Visibility = Visibility.Collapsed;
            }
            else
            {
                DebiCheckCB.Visibility = Visibility.Visible;
            }
        }

        #endregion



        #region Private Methods

        private void LoadAgentInfo()
        {
            try
            {
                Cursor = Cursors.Wait;


                if (chkFoundation.IsChecked == true) 
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    if (AgentMode != null) parameters[0] = new SqlParameter("@AgentMode", (int)AgentMode);
                    DataTable dt = Methods.ExecuteStoredProcedure("spGetSalesAgents4Foundation", parameters).Tables[0];

                    DataColumn column = new DataColumn("IsChecked", typeof(bool));
                    column.DefaultValue = false;
                    dt.Columns.Add(column);

                    xdgAgents.DataSource = dt.DefaultView;

                    AllRecordsChecked = false;
                }

                else if (grdAgents.IsEnabled)
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    if (AgentMode != null) parameters[0] = new SqlParameter("@AgentMode", (int)AgentMode);
                    DataTable dt = Methods.ExecuteStoredProcedure("spGetSalesAgents4", parameters).Tables[0];

                    DataColumn column = new DataColumn("IsChecked", typeof(bool));
                    column.DefaultValue = false;
                    dt.Columns.Add(column);

                    xdgAgents.DataSource = dt.DefaultView;
                }
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }

            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private bool? IsAllRecordsChecked()
        {
            try
            {
                bool allSelected = (xdgAgents.Records.Select(r => (bool)((DataRecord)r).Cells["IsChecked"].Value)).All(b => b);
                bool noneSelected = (xdgAgents.Records.Select(r => (bool)((DataRecord)r).Cells["IsChecked"].Value)).All(b => !b);

                int countSelected = (xdgAgents.Records.Select(r => (bool)((DataRecord)r).Cells["IsChecked"].Value)).Count(b => b);
                lblAgents.Text = "Select Agent(s) " + "[" + countSelected + "]";
                lblTeams.Text = "Select Team(s) " + "[" + countSelected + "]";

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
                (new BaseControl()).HandleException(ex);
                return null;
            }
        }

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _reportTimer.Stop();
            _seconds = 0;
            IsReportRunning = false;
            btnReport.Content = "Report";
            lblCurrentAgent.Text = String.Empty;
        }

        private void AddOvertimeSheet(Workbook wbTemplate, Workbook wbReport, string agentName, long? agentID, DateTime fromDate, DateTime toDate)
        {
            #region First, get the data from the database

            DataTable dtOvertime = Insure.INGetSalesReportOvertimeData(agentID, fromDate, toDate);

            #endregion First, get the data from the database

            if (dtOvertime.Rows.Count > 0)
            {

                #region Declarations

                int reportRowIndex = 6;

                #endregion Declarations

                #region Add the new worksheet

                string newWorksheetDescription = Methods.ParseWorksheetName(wbReport, agentName, " ", "");
                Worksheet wsNewWorksheetTemplate = wbTemplate.Worksheets["Overtime"];
                //Worksheet wsNewWorksheet = wbReport.Worksheets.Add(newWorksheetDescription);
                Worksheet wsNewWorksheet;
                try
                {
                    wsNewWorksheet = wbReport.Worksheets.Add(newWorksheetDescription);
                    //wsNewWorksheet = wbReport.Worksheets.Add(string.Join("", agentName.Take(31) + "Overtime"));
                }
                catch
                {
                    wsNewWorksheet = wbReport.Worksheets.Add(newWorksheetDescription + "2");
                    //wsNewWorksheet = wbReport.Worksheets.Add(string.Join("", agentName + "a".Take(31) + "Overtime"));
                }

                #endregion Add the new worksheet

                #region Copy the template formatting and add the details

                Methods.CopyExcelRegion(wsNewWorksheetTemplate, 0, 0, 4, 1, wsNewWorksheet, 0, 0);

                if (fromDate.Date == toDate.Date)
                {
                    wsNewWorksheet.GetCell("A3").Value = String.Format("Date: {0}", fromDate.ToString("yyyy-MM-dd"));
                }
                else
                {
                    wsNewWorksheet.GetCell("A3").Value = String.Format("{0} - {1}", fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"));
                }

                #endregion Copy the template formatting and add the details

                #region Add each row

                foreach (DataRow drOvertimeData in dtOvertime.Rows)
                {
                    Methods.CopyExcelRegion(wsNewWorksheetTemplate, 5, 0, 0, 1, wsNewWorksheet, reportRowIndex - 1, 0);
                    wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex)).Value = drOvertimeData["WorkingDate"];
                    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = drOvertimeData["OvertimeHoursWorked"];

                    reportRowIndex++;
                }

                #endregion Add each row

                #region Add the total

                Methods.CopyExcelRegion(wsNewWorksheetTemplate, 6, 0, 0, 1, wsNewWorksheet, reportRowIndex - 1, 0);
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(String.Format("=SUM(B6:B{0})", reportRowIndex - 1));

                #endregion Add the total
            }
        }

        private void AddreferralsSheet(Workbook wbTemplate, Workbook wbReport, string agentName, long? agentID, DateTime fromDate, DateTime toDate)
        {
            #region First, get the data from the database

            SqlParameter[] parameters =
               {
                    new SqlParameter("@AgentID", agentID),
                    new SqlParameter("@FromDate", _fromDate),
                    new SqlParameter("@ToDate", _toDate)
                };

            DataSet ds = null;

            ds = Methods.ExecuteStoredProcedureSaleReport("spINReportSales", parameters);


            DataTable dtReferences = ds.Tables[25];
            DataTable dtTotalSales = ds.Tables[26];
            DataTable dtSalesLessApplicable = ds.Tables[27];
            DataTable dtTransfers = ds.Tables[28];
            DataTable dtNonTransfers = ds.Tables[29];
            DataTable dtLessForwardToDCAgent = ds.Tables[30];
            DataTable dtCarriedForwards = ds.Tables[31];
            DataTable dtCancellations = ds.Tables[32];
            DataTable dtRejectedDebiCheckCallBacks = ds.Tables[33];
            DataTable dtRejecteddebicheckFinal = ds.Tables[34];
            DataTable dtReferrals = ds.Tables[35];

            #endregion First, get the data from the database

            if (dtReferences.Rows.Count > 0)
            {

                #region Declarations

                int reportRowIndex = 6;

                #endregion Declarations

                #region Add the new worksheet

                string newWorksheetDescription = Methods.ParseWorksheetName(wbReport, agentName, " ", "Referrals");
                Worksheet wsNewWorksheetTemplate = wbTemplate.Worksheets["Referrals"];
                //Worksheet wsNewWorksheet = wbReport.Worksheets.Add(newWorksheetDescription);
                Worksheet wsNewWorksheet;
                try
                {
                    wsNewWorksheet = wbReport.Worksheets.Add(newWorksheetDescription);
                }
                catch
                {
                    wsNewWorksheet = wbReport.Worksheets.Add(newWorksheetDescription + "2");
                }

                #endregion Add the new worksheet

                #region Copy the template formatting and add the details

                //Methods.CopyExcelRegion(wsNewWorksheetTemplate, 0, 0, 4, 6, wsNewWorksheet, 0, 0);

                if (fromDate.Date == toDate.Date)
                {
                    wsNewWorksheet.GetCell("A3").Value = String.Format("Date: {0}", fromDate.ToString("yyyy-MM-dd"));
                }
                else
                {
                    wsNewWorksheet.GetCell("A3").Value = String.Format("{0} - {1}", fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"));
                }

                #endregion Copy the template formatting and add the details

                #region Add each row

                foreach (DataRow drOvertimeData in dtReferrals.Rows)
                {
                    Methods.CopyExcelRegion(wsNewWorksheetTemplate, 5, 0, 0, 6, wsNewWorksheet, reportRowIndex - 1, 0);
                    wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex)).Value = drOvertimeData["RefNo"];
                    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = drOvertimeData["DateCaptured"];
                    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).Value = drOvertimeData["ReferralCount"];

                    reportRowIndex++;
                }

                #region add Headers

                #endregion
                wsNewWorksheet.GetCell(String.Format("A{0}", 5)).Value = "RefNo";
                wsNewWorksheet.GetCell(String.Format("B{0}", 5)).Value = "Date Captured";
                wsNewWorksheet.GetCell(String.Format("C{0}", 5)).Value = "Referral Count";

                wsNewWorksheet.Rows[4].Cells[0].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                wsNewWorksheet.Rows[4].Cells[1].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                wsNewWorksheet.Rows[4].Cells[2].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                //wsNewWorksheet.Rows[4].Cells[3].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                //wsNewWorksheet.Rows[4].Cells[4].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                //wsNewWorksheet.Rows[4].Cells[5].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                //wsNewWorksheet.Rows[4].Cells[6].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);





                #endregion Add each row

                wsNewWorksheet.Columns[0].Width = 7000;
                wsNewWorksheet.Columns[1].Width = 7000;
                wsNewWorksheet.Columns[2].Width = 7000;
                //wsNewWorksheet.Columns[3].Width = 7000;


                #region Add the total



                #endregion Add the total
            }
        }


        private void AddFowardToDCAgentSheet(Workbook wbTemplate, Workbook wbReport, string agentName, long? agentID, DateTime fromDate, DateTime toDate)
        {
            #region First, get the data from the database

             SqlParameter[] parameters =
                {
                    new SqlParameter("@AgentID", agentID),
                    new SqlParameter("@FromDate", _fromDate),
                    new SqlParameter("@ToDate", _toDate)
                };

            DataSet ds = null;

            ds = Methods.ExecuteStoredProcedureSaleReport("spINReportSales", parameters);


            DataTable dtReferences = ds.Tables[25];
            DataTable dtTotalSales = ds.Tables[26];
            DataTable dtSalesLessApplicable = ds.Tables[27];
            DataTable dtTransfers = ds.Tables[28];
            DataTable dtNonTransfers = ds.Tables[29];
            DataTable dtLessForwardToDCAgent = ds.Tables[30];
            DataTable dtCarriedForwards = ds.Tables[31];
            DataTable dtCancellations = ds.Tables[32];
            DataTable dtRejectedDebiCheckCallBacks = ds.Tables[33];
            DataTable dtRejecteddebicheckFinal = ds.Tables[34];

            #endregion First, get the data from the database

            if (dtReferences.Rows.Count > 0)
            {

                #region Declarations

                int reportRowIndex = 6;

                #endregion Declarations

                #region Add the new worksheet

                string newWorksheetDescription = Methods.ParseWorksheetName(wbReport, agentName, " ", "TransferStats");
                Worksheet wsNewWorksheetTemplate = wbTemplate.Worksheets["ForwardToDCAgent"];
                //Worksheet wsNewWorksheet = wbReport.Worksheets.Add(newWorksheetDescription);
                Worksheet wsNewWorksheet;
                try
                {
                    wsNewWorksheet = wbReport.Worksheets.Add(newWorksheetDescription);
                }
                catch
                {
                    wsNewWorksheet = wbReport.Worksheets.Add(newWorksheetDescription + "2");
                }

                #endregion Add the new worksheet

                #region Copy the template formatting and add the details

                //Methods.CopyExcelRegion(wsNewWorksheetTemplate, 0, 0, 4, 6, wsNewWorksheet, 0, 0);

                if (fromDate.Date == toDate.Date)
                {
                    wsNewWorksheet.GetCell("A3").Value = String.Format("Date: {0}", fromDate.ToString("yyyy-MM-dd"));
                }
                else
                {
                    wsNewWorksheet.GetCell("A3").Value = String.Format("{0} - {1}", fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"));
                }

                #endregion Copy the template formatting and add the details

                #region Add each row

                foreach (DataRow drOvertimeData in dtReferences.Rows)
                {
                    Methods.CopyExcelRegion(wsNewWorksheetTemplate, 5, 0, 0, 6, wsNewWorksheet, reportRowIndex - 1, 0);
                    wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex)).Value = drOvertimeData["DateOfSale"];
                    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = drOvertimeData["RefNo"];
                    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).Value = drOvertimeData["Code"];
                    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).Value = drOvertimeData["FirstName"];
                    wsNewWorksheet.GetCell(String.Format("E{0}", reportRowIndex)).Value = drOvertimeData["Transferred"];
                    wsNewWorksheet.GetCell(String.Format("F{0}", reportRowIndex)).Value = drOvertimeData["LeadStatus"];
                    wsNewWorksheet.GetCell(String.Format("G{0}", reportRowIndex)).Value = drOvertimeData["Description"];

                    reportRowIndex++;
                }

                #region add Headers

                #endregion
                wsNewWorksheet.GetCell(String.Format("A{0}", 5)).Value = "Date";
                wsNewWorksheet.GetCell(String.Format("B{0}", 5)).Value = "Reference Number";
                wsNewWorksheet.GetCell(String.Format("C{0}", 5)).Value = "Campaign Code";
                wsNewWorksheet.GetCell(String.Format("D{0}", 5)).Value = "Client";
                wsNewWorksheet.GetCell(String.Format("E{0}", 5)).Value = "Transferred";
                wsNewWorksheet.GetCell(String.Format("F{0}", 5)).Value = "Lead Status";
                wsNewWorksheet.GetCell(String.Format("G{0}", 5)).Value = "Reason";

                wsNewWorksheet.Rows[4].Cells[0].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                wsNewWorksheet.Rows[4].Cells[1].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                wsNewWorksheet.Rows[4].Cells[2].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                wsNewWorksheet.Rows[4].Cells[3].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                wsNewWorksheet.Rows[4].Cells[4].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                wsNewWorksheet.Rows[4].Cells[5].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                wsNewWorksheet.Rows[4].Cells[6].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);





                #endregion Add each row

                wsNewWorksheet.Columns[0].Width = 7000;
                wsNewWorksheet.Columns[1].Width = 7000;
                wsNewWorksheet.Columns[2].Width = 7000;
                wsNewWorksheet.Columns[3].Width = 7000;
                wsNewWorksheet.Columns[4].Width = 7000;
                wsNewWorksheet.Columns[5].Width = 7000;
                wsNewWorksheet.Columns[6].Width = 7000;

                #region Add the total

                //Methods.CopyExcelRegion(wsNewWorksheetTemplate, 6, 0, 0, 1, wsNewWorksheet, reportRowIndex - 1, 0);
                //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(String.Format("=SUM(B6:B{0})", reportRowIndex - 1));

                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 2)).Value = "Total Sales";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 2)).Value = dtTotalSales.Rows[0][0].ToString();
                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 3)).Value = "Less Not Applicable Sales";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 3)).Value = dtSalesLessApplicable.Rows[0][0].ToString();
                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 4)).Value = "TotalSales";
                wsNewWorksheet.Rows[reportRowIndex + 3].Cells[0].CellFormat.Font.Bold = Infragistics.Documents.Excel.ExcelDefaultableBoolean.True;

                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 6)).Value = "Transferred";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 6)).Value = dtTransfers.Rows[0][0].ToString();          
                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 7)).Value = "Not Transferred";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 7)).Value = dtNonTransfers.Rows[0][0].ToString();

                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 9)).Value = "Outstanding Sales";
                wsNewWorksheet.Rows[reportRowIndex + 8].Cells[0].CellFormat.Font.Bold = Infragistics.Documents.Excel.ExcelDefaultableBoolean.True;
                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 10)).Value = "Less Forward To DC Agent";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 10)).Value = dtLessForwardToDCAgent.Rows[0][0].ToString();              
                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 11)).Value = "Carried Forwards";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 11)).Value = dtCarriedForwards.Rows[0][0].ToString();
                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 12)).Value = "Cancellations";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 12)).Value = dtCancellations.Rows[0][0].ToString();
                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 13)).Value = "Debi-Check Rejected - Call Back";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 13)).Value = dtRejectedDebiCheckCallBacks.Rows[0][0];
                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 14)).Value = "Debi-Check Rejected - Final";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 14)).Value = dtRejecteddebicheckFinal.Rows[0][0].ToString();

                for(int x = 1; x <= 2 ; x++)
                {
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[0].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.DarkGray), null, FillPatternStyle.Solid);
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[1].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[1].CellFormat.Alignment = (HorizontalCellAlignment)HorizontalAlignment.Center;
                }
                wsNewWorksheet.Rows[reportRowIndex + 3].Cells[0].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.DarkGray), null, FillPatternStyle.Solid);

                for (int x = 5; x <= 6; x++)
                {
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[0].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.DarkGray), null, FillPatternStyle.Solid);
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[1].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[1].CellFormat.Alignment = (HorizontalCellAlignment)HorizontalAlignment.Center;

                }

                for (int x = 9; x <= 13; x++)
                {
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[0].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.DarkGray), null, FillPatternStyle.Solid);
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[1].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[1].CellFormat.Alignment = (HorizontalCellAlignment)HorizontalAlignment.Center;

                }
                #endregion Add the total
            }
        }

        private void AddFowardToDCAgentSheetDC(Workbook wbTemplate, Workbook wbReport, string agentName, long? agentID, DateTime fromDate, DateTime toDate)
        {
            #region First, get the data from the database

            SqlParameter[] parameters =
                {
                    new SqlParameter("@AgentID", agentID),
                    new SqlParameter("@FromDate", _fromDate),
                    new SqlParameter("@ToDate", _toDate)
                };

            DataSet ds = null;

            ds = Methods.ExecuteStoredProcedureSaleReport("spINReportSalesDC", parameters);

            DataTable dtReferences = ds.Tables[25];
            DataTable dtTotalSales = ds.Tables[26];
            DataTable dtSalesLessApplicable = ds.Tables[27];
            DataTable dtTransfers = ds.Tables[28];
            DataTable dtNonTransfers = ds.Tables[29];
            DataTable dtLessForwardToDCAgent = ds.Tables[30];
            DataTable dtCarriedForwards = ds.Tables[31];
            DataTable dtCancellations = ds.Tables[32];
            DataTable dtRejectedDebiCheckCallBacks = ds.Tables[33];
            DataTable dtRejecteddebicheckFinal = ds.Tables[34];
            DataTable dtTransferAccepted = ds.Tables[35];
            DataTable dtNoTransferAccepted = ds.Tables[36];


            #endregion First, get the data from the database

            if (dtReferences.Rows.Count > 0)
            {

                #region Declarations

                int reportRowIndex = 6;

                #endregion Declarations

                #region Add the new worksheet

                string newWorksheetDescription = Methods.ParseWorksheetName(wbReport, agentName, " ", "TransferStats");
                Worksheet wsNewWorksheetTemplate = wbTemplate.Worksheets["ForwardToDCAgent"];
                //Worksheet wsNewWorksheet = wbReport.Worksheets.Add(newWorksheetDescription);
                Worksheet wsNewWorksheet;
                try
                {
                    wsNewWorksheet = wbReport.Worksheets.Add(newWorksheetDescription);
                }
                catch
                {
                    wsNewWorksheet = wbReport.Worksheets.Add(newWorksheetDescription + "2");
                }

                #endregion Add the new worksheet

                #region Copy the template formatting and add the details

                Methods.CopyExcelRegion(wsNewWorksheetTemplate, 0, 0, 4, 7, wsNewWorksheet, 0, 0);

                if (fromDate.Date == toDate.Date)
                {
                    wsNewWorksheet.GetCell("A3").Value = String.Format("Date: {0}", fromDate.ToString("yyyy-MM-dd"));
                }
                else
                {
                    wsNewWorksheet.GetCell("A3").Value = String.Format("{0} - {1}", fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"));
                }

                #endregion Copy the template formatting and add the details

                #region Add each row

                foreach (DataRow drOvertimeData in dtReferences.Rows)
                {
                    Methods.CopyExcelRegion(wsNewWorksheetTemplate, 5, 0, 0, 8, wsNewWorksheet, reportRowIndex - 1, 0);
                    wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex)).Value = drOvertimeData["DateOfSale"];
                    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = drOvertimeData["RefNo"];
                    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).Value = drOvertimeData["Code"];
                    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).Value = drOvertimeData["FirstName"];
                    wsNewWorksheet.GetCell(String.Format("E{0}", reportRowIndex)).Value = drOvertimeData["Transferred"];
                    wsNewWorksheet.GetCell(String.Format("F{0}", reportRowIndex)).Value = drOvertimeData["LeadStatus"];
                    wsNewWorksheet.GetCell(String.Format("G{0}", reportRowIndex)).Value = drOvertimeData["Description"];
                    wsNewWorksheet.GetCell(String.Format("H{0}", reportRowIndex)).Value = drOvertimeData["MandateStatus"];

                    reportRowIndex++;
                }

                #region Headers
                wsNewWorksheet.GetCell(String.Format("A{0}", 5)).Value = "Date";
                wsNewWorksheet.GetCell(String.Format("B{0}", 5)).Value = "Reference Number";
                wsNewWorksheet.GetCell(String.Format("C{0}", 5)).Value = "Campaign Code";
                wsNewWorksheet.GetCell(String.Format("D{0}", 5)).Value = "Client";
                wsNewWorksheet.GetCell(String.Format("E{0}", 5)).Value = "Transferred";
                wsNewWorksheet.GetCell(String.Format("F{0}", 5)).Value = "Lead Status";
                wsNewWorksheet.GetCell(String.Format("G{0}", 5)).Value = "Reason";
                wsNewWorksheet.GetCell(String.Format("H{0}", 5)).Value = "Mandate Status";

                wsNewWorksheet.Rows[4].Cells[0].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                wsNewWorksheet.Rows[4].Cells[1].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                wsNewWorksheet.Rows[4].Cells[2].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                wsNewWorksheet.Rows[4].Cells[3].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                wsNewWorksheet.Rows[4].Cells[4].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                wsNewWorksheet.Rows[4].Cells[5].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                wsNewWorksheet.Rows[4].Cells[6].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                wsNewWorksheet.Rows[4].Cells[7].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);


                #endregion

                #endregion Add each row

                wsNewWorksheet.Columns[0].Width = 7000;
                wsNewWorksheet.Columns[1].Width = 7000;
                wsNewWorksheet.Columns[2].Width = 7000;
                wsNewWorksheet.Columns[3].Width = 7000;
                wsNewWorksheet.Columns[4].Width = 7000;
                wsNewWorksheet.Columns[5].Width = 7000;
                wsNewWorksheet.Columns[6].Width = 7000;
                wsNewWorksheet.Columns[7].Width = 7000;


                #region Add the total

                //Methods.CopyExcelRegion(wsNewWorksheetTemplate, 6, 0, 0, 1, wsNewWorksheet, reportRowIndex - 1, 0);
                //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(String.Format("=SUM(B6:B{0})", reportRowIndex - 1));

                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 2)).Value = "Total Sales";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 2)).Value = dtTotalSales.Rows[0][0].ToString();
                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 3)).Value = "Less Not Applicable Sales";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 3)).Value = dtSalesLessApplicable.Rows[0][0].ToString();
                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 4)).Value = "Total Sales";
                wsNewWorksheet.Rows[reportRowIndex + 3].Cells[0].CellFormat.Font.Bold = Infragistics.Documents.Excel.ExcelDefaultableBoolean.True;

                wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex + 5)).Value = "Accepted %";
                wsNewWorksheet.Rows[reportRowIndex + 4].Cells[2].CellFormat.Font.Bold = Infragistics.Documents.Excel.ExcelDefaultableBoolean.True;

                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 6)).Value = "Transferred";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 6)).Value = dtTransfers.Rows[0][0].ToString();
                wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex + 6)).Value = dtTransferAccepted.Rows[0][0].ToString() + "%";

                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 7)).Value = "Not Transferred";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 7)).Value = dtNonTransfers.Rows[0][0].ToString();
                wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex + 7)).Value = dtNoTransferAccepted.Rows[0][0].ToString() + "%";


                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 9)).Value = "Outstanding Sales";
                wsNewWorksheet.Rows[reportRowIndex + 8].Cells[0].CellFormat.Font.Bold = Infragistics.Documents.Excel.ExcelDefaultableBoolean.True;
                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 10)).Value = "Less Forward To DC Agent";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 10)).Value = dtLessForwardToDCAgent.Rows[0][0].ToString();
                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 11)).Value = "Carried Forwards";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 11)).Value = dtCarriedForwards.Rows[0][0].ToString();
                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 12)).Value = "Cancellations";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 12)).Value = dtCancellations.Rows[0][0].ToString();
                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 13)).Value = "Debi-Check Rejected - Call Back";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 13)).Value = dtRejectedDebiCheckCallBacks.Rows[0][0];
                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 14)).Value = "Debi-Check Rejected - Final";
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex + 14)).Value = dtRejecteddebicheckFinal.Rows[0][0].ToString();

                for (int x = 1; x <= 2; x++)
                {
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[0].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.DarkGray), null, FillPatternStyle.Solid);
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[1].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[1].CellFormat.Alignment = (HorizontalCellAlignment)HorizontalAlignment.Center;
                }
                wsNewWorksheet.Rows[reportRowIndex + 3].Cells[0].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.DarkGray), null, FillPatternStyle.Solid);
                //wsNewWorksheet.Rows[reportRowIndex + 4].Cells[2].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.DarkGray), null, FillPatternStyle.Solid);


                for (int x = 5; x <= 6; x++)
                {
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[0].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.DarkGray), null, FillPatternStyle.Solid);
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[1].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[1].CellFormat.Alignment = (HorizontalCellAlignment)HorizontalAlignment.Center;
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[2].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);


                }

                for (int x = 9; x <= 13; x++)
                {
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[0].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.DarkGray), null, FillPatternStyle.Solid);
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[1].CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(Color.LightGray), null, FillPatternStyle.Solid);
                    wsNewWorksheet.Rows[reportRowIndex + x].Cells[1].CellFormat.Alignment = (HorizontalCellAlignment)HorizontalAlignment.Center;

                }
                #endregion Add the total
            }
        }

        private void AddRedeemedGiftsSheet(Workbook wbTemplate, Workbook wbReport, string agentName, long? agentID, DateTime fromDate, DateTime toDate)
        {
            #region First, get the data from the database - and exit method if there are no campaigns

            DataSet dsRedeemedGifts = Insure.INGetSalesReportRedeemedGiftsData(agentID, fromDate, toDate);
            if (dsRedeemedGifts.Tables[0].Rows.Count == 0)
            {
                return;
            }

            DataTable dtCampaigns = dsRedeemedGifts.Tables[0];
            DataTable dtMainData = dsRedeemedGifts.Tables[1];
            DataTable dtDataSheetColumnCellMappings = dsRedeemedGifts.Tables[2];
            DataTable dtDataSheetTotalFormulasCellMappings = dsRedeemedGifts.Tables[3];

            #endregion First, get the data from the database - and exit method if there are no campaigns

            #region Declarations

            byte templateColumnSpan = 7;
            byte sheetHeadingRowSpan = 5;
            byte campaignSubHeadingTemplateRowIndex = 6;
            byte columnHeadingsTemplateRowIndex = 7;
            byte dataRowTemplateRowIndex = 8;
            byte totalsTemplateRowIndex = 11;
            byte noDataRowTemplateRowIndex = 9;
            byte grandTotalsTemplateRowIndex = 13;

            int reportRowIndex = 0;
            int formulaStartRow = 0;

            string grandTotalFormulaTemplate = String.Empty; //"SUM(###COLUMN##";

            #endregion Declarations

            #region Add the new worksheet

            string newWorksheetDescription = Methods.ParseWorksheetName(wbReport, String.Format("{0} Redeemed Gifts", agentName));
            Worksheet wsNewWorksheetTemplate = wbTemplate.Worksheets["RedeemedGifts"];


            //Worksheet wsNewWorksheet = wbReport.Worksheets.Add(newWorksheetDescription);

            Worksheet wsNewWorksheet;

            try
            {
                wsNewWorksheet = wbReport.Worksheets.Add(newWorksheetDescription);
            }
            catch
            {
                wsNewWorksheet = wbReport.Worksheets.Add(newWorksheetDescription + "2");
            }


            Methods.CopyWorksheetOptionsFromTemplate(wsNewWorksheetTemplate, wsNewWorksheet, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);
            wsNewWorksheet.Workbook.NamedReferences.Clear();

            #endregion Add the new worksheet

            #region Copy the template formatting and add the details

            Methods.CopyExcelRegion(wsNewWorksheetTemplate, 0, 0, sheetHeadingRowSpan, templateColumnSpan, wsNewWorksheet, reportRowIndex, 0);

            if (fromDate.Date == toDate.Date)
            {
                wsNewWorksheet.GetCell("A3").Value = String.Format("Date: {0}", fromDate.ToString("yyyy-MM-dd"));
            }
            else
            {
                wsNewWorksheet.GetCell("A3").Value = String.Format("{0} - {1}", fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"));
            }

            wsNewWorksheet.GetCell("A5").Value = agentName;




            reportRowIndex = 6;

            #endregion Copy the template formatting and add the details

            foreach (DataRow drCurrentCampaign in dtCampaigns.Rows)
            {
                wsNewWorksheet.Workbook.NamedReferences.Clear();

                string filterString = drCurrentCampaign["FilterString"].ToString();
                string orderByString = drCurrentCampaign["OrderByString"].ToString();

                var filteredRows = dtMainData.Select(filterString, orderByString).AsEnumerable();
                if (filteredRows.Any())
                {
                    DataTable dtCurrentCampaignData = dtMainData.Select(filterString, orderByString).CopyToDataTable();
                    string campaignCode = drCurrentCampaign["CampaignCode"].ToString();
                    string gift1 = drCurrentCampaign["Gift1"].ToString();
                    string gift2 = drCurrentCampaign["Gift2"].ToString();
                    string gift3 = drCurrentCampaign["Gift3"].ToString();
                    string gift1ColumnHeadingCell = drCurrentCampaign["Gift1ColumnHeadingCell"].ToString();
                    string gift2ColumnHeadingCell = drCurrentCampaign["Gift2ColumnHeadingCell"].ToString();
                    string gift3ColumnHeadingCell = drCurrentCampaign["Gift3ColumnHeadingCell"].ToString();

                    // Add the sub-heading containing the campaign code:
                    wsNewWorksheet.Workbook.NamedReferences.Clear();
                    Methods.CopyExcelRegion(wsNewWorksheetTemplate, campaignSubHeadingTemplateRowIndex, 0, 0, templateColumnSpan, wsNewWorksheet, reportRowIndex, 0);
                    wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 1)).Value = campaignCode;
                    reportRowIndex++;

                    // Insert the column headings:
                    wsNewWorksheet.Workbook.NamedReferences.Clear();
                    Methods.CopyExcelRegion(wsNewWorksheetTemplate, columnHeadingsTemplateRowIndex, 0, 0, templateColumnSpan, wsNewWorksheet, reportRowIndex, 0);
                    wsNewWorksheet.GetCell(gift1ColumnHeadingCell).Value = gift1;
                    wsNewWorksheet.GetCell(gift2ColumnHeadingCell).Value = gift2;
                    wsNewWorksheet.GetCell(gift3ColumnHeadingCell).Value = gift3;

                    reportRowIndex++;

                    #region Add each row

                    formulaStartRow = reportRowIndex;
                    reportRowIndex = Methods.MapTemplatizedExcelValues(wsNewWorksheetTemplate, dtCurrentCampaignData, dtDataSheetColumnCellMappings, dataRowTemplateRowIndex, 0, 0, templateColumnSpan, wsNewWorksheet, reportRowIndex, 0);
                    reportRowIndex++;

                    #endregion Add each row

                    #region Insert the totals

                    reportRowIndex = Methods.MapTemplatizedExcelFormulas(wsNewWorksheetTemplate, dtDataSheetTotalFormulasCellMappings, totalsTemplateRowIndex, 0, 0, templateColumnSpan, wsNewWorksheet, reportRowIndex, 0, formulaStartRow, reportRowIndex - 1);

                    if (grandTotalFormulaTemplate.Trim().Length > 0)
                    {
                        grandTotalFormulaTemplate = String.Format("{0}:#COLUMN#{1}", grandTotalFormulaTemplate, reportRowIndex + 1);
                    }
                    else
                    {
                        grandTotalFormulaTemplate = String.Format("#COLUMN#{0}", reportRowIndex + 1);
                    }

                    reportRowIndex++;

                    #endregion Insert the totals
                }
                else
                {
                    Methods.CopyExcelRegion(wsNewWorksheetTemplate, totalsTemplateRowIndex, 0, 0, templateColumnSpan, wsNewWorksheet, reportRowIndex, 0);
                    reportRowIndex++;
                }

                //wsNewWorksheet.Workbook.NamedReferences.Clear();
            }

            #region Insert the grand total

            if (dtCampaigns.Rows.Count > 1)
            {
                Methods.CopyExcelRegion(wsNewWorksheetTemplate, grandTotalsTemplateRowIndex, 0, 0, templateColumnSpan, wsNewWorksheet, reportRowIndex, 0);
                if (dtDataSheetTotalFormulasCellMappings.Rows.Count > 0)
                {
                    #region Loop through each row in the formulas data table, use the first column (labeled "ExcelColumn") and use its value to apply the grand totals

                    foreach (DataRow drColumnRowMapping in dtDataSheetTotalFormulasCellMappings.Rows)
                    {
                        string excelColumn = drColumnRowMapping["ExcelColumn"].ToString();
                        string grandTotalFormula = string.Format("=SUM({0})", grandTotalFormulaTemplate);
                        grandTotalFormula = grandTotalFormula.Replace("#COLUMN#", excelColumn);
                        wsNewWorksheet.GetCell(String.Format("{0}{1}", excelColumn, reportRowIndex + 1)).ApplyFormula(grandTotalFormula);
                    }

                    #endregion Loop through each row in the formulas data table, use the first column (labeled "ExcelColumn") and use its value to apply the grand totals
                }
            }

            #endregion Insert the grand total
        }

        private void ReportBody(Workbook wbTemplate, Workbook wbReport, string agentName, long? agentID, string dateRange)
        {
            #region Retrieve data from database

            SqlParameter[] parameters =
                {
                    new SqlParameter("@AgentID", agentID),
                    new SqlParameter("@FromDate", _fromDate),
                    new SqlParameter("@ToDate", _toDate)
                };

            DataSet ds = null;


            ds = Methods.ExecuteStoredProcedureSaleReport("spINReportSales", parameters);



            DataTable dtCampaigns = ds.Tables[0];
            DataTable dtCampaignsCF = ds.Tables[21];
            DataTable dtSales = ds.Tables[1];
            DataTable dtSalesCF = ds.Tables[22];
            DataTable dtCampaignTotals = ds.Tables[2];
            DataTable dtCampaignTotalsCF = ds.Tables[23];
            DataTable dtGrandTotals = ds.Tables[3];
            DataTable dtGrandTotalsCF = ds.Tables[24];

            //bool isUpgradeCampaign = Insure.IsUpgradeCampaign(drCampaign["CampaignID"] as long?);


            #endregion retrieve data from database

            #region setup worksheet

            //WorksheetCell wsCell;
            Worksheet wsTemplate = wbTemplate.Worksheets["Report"];
            //Worksheet wsReport = wbReport.Worksheets.Add(string.Join("", agentName.Take(31)));

            Worksheet wsReport;

            try
            {
                wsReport = wbReport.Worksheets.Add(string.Join("", agentName.Take(31)));
            }
            catch
            {
                wsReport = wbReport.Worksheets.Add(string.Join("", agentName + "a".Take(31)));
            }

            wsReport.DisplayOptions.View = WorksheetView.Normal;

            wsReport.PrintOptions.PaperSize = PaperSize.A4;
            wsReport.PrintOptions.Orientation = Orientation.Portrait;
            wsReport.PrintOptions.LeftMargin = 0.3;
            wsReport.PrintOptions.RightMargin = 0.3;

            wsReport.Workbook.NamedReferences.Clear();
            Methods.CopyExcelRegion(wsTemplate, 0, 0, 3, 4, wsReport, 0, 0);

            #endregion

            #region header data

            {
                wsReport.GetCell("AgentName").Value = agentName;
                wsReport.GetCell("SalesDates").Value = "Sales: " + dateRange;

                //wsReport.GetCell("Batches").Value = batchCodes + ((char)65279);
            }

            #endregion

            #region report data

            {
                #region Sales

                int rowIndex = 4;
                wsReport.Workbook.NamedReferences.Clear();

                foreach (DataRow drCampaign in dtCampaigns.Rows)
                {
                    string filterString = drCampaign["FilterString"].ToString();
                    string orderByString = drCampaign["OrderByString"].ToString();
                    string campaignTotalsFilteringString = drCampaign["CampaignTotalsFilteringString"].ToString();

                    // Determine if there are any rows for the given partition
                    var filteredSheetRows = dtSales.Select(filterString).AsEnumerable();
                    if (filteredSheetRows.Any())
                    {
                        DataTable dtCampaignSales = dtSales.Select(filterString, orderByString).CopyToDataTable();

                        Methods.CopyExcelRegion(wsTemplate, 4, 0, 1, 11, wsReport, rowIndex, 0);

                        rowIndex++;

                        wsReport.GetCell("Campaign").Value = drCampaign["CampaignCode"] as string;

                        //DataTable dtCampaignSales = dtSales.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'").CopyToDataTable();

                        //string filterString = drCampaign["FilterString"].ToString();
                        //string orderByString = drCampaign["OrderByString"].ToString();
                        //string campaignTotalsFilteringString = drCampaign["CampaignTotalsFilteringString"].ToString();
                        //DataTable dtCampaignSales = dtSales.Select(filterString, orderByString).CopyToDataTable();

                        foreach (DataRow drSale in dtCampaignSales.Rows)
                        {
                            rowIndex++;
                            Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, 11, wsReport, rowIndex, 0);

                            bool isGoldenLead = false;
                            CellFillPattern cellFill = null;

                            if ((drSale["RefNo"] as string).Contains("Golden"))
                            {
                                isGoldenLead = true;
                                cellFill = new CellFillPattern(new WorkbookColorInfo(Color.Gold), null, FillPatternStyle.Solid);
                            }

                            wsReport.GetCell("DateOfSale").Value = ((DateTime)drSale["DateOfSale"]).ToString("d");
                            if (isGoldenLead) { wsReport.GetCell("DateOfSale").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("Batch").Value = drSale["Batch"] as string;
                            if (isGoldenLead) { wsReport.GetCell("Batch").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("RefNo").Value = drSale["RefNo"] as string;
                            if (isGoldenLead) { wsReport.GetCell("RefNo").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("Client").Value = drSale["LeadName"] as string;
                            if (isGoldenLead) { wsReport.GetCell("Client").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("Premium").Value = drSale["Premium"] as decimal?;
                            if (isGoldenLead)
                            {
                                wsReport.GetCell("Premium").CellFormat.Fill = cellFill;
                                wsReport.GetCell("Premium").CellFormat.Font.ColorInfo = new WorkbookColorInfo(Color.Red);
                                //wsReport.GetCell("Premium").Value = "[" + wsReport.GetCell("Premium").Value + "]";
                            }
                            wsReport.GetCell("Units").Value = drSale["Units"] as decimal?;
                            if (isGoldenLead) { wsReport.GetCell("Units").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("SavedStatus").Value = drSale["SavedStatus"] as string;
                            if (isGoldenLead) { wsReport.GetCell("SavedStatus").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("SavedStatusOriginalDOS").Value = drSale["SavedStatusOriginalDOS"] as string;
                            if (isGoldenLead) { wsReport.GetCell("SavedStatusOriginalDOS").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("ReferralResult").Value = drSale["ReferralResult"] as string;
                            if (isGoldenLead) { wsReport.GetCell("ReferralResult").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("MandateStatus").Value = drSale["MandateStatus"] as string;
                            if (isGoldenLead) { wsReport.GetCell("MandateStatus").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("Incentive").Value = drSale["Incentive"] as int?;
                            if (isGoldenLead) { wsReport.GetCell("Incentive").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("DCSpecialist").Value = drSale["DCSpecialist"] as string;
                            if (isGoldenLead) { wsReport.GetCell("DCSpecialist").CellFormat.Fill = cellFill; }


                            wsReport.Workbook.NamedReferences.Clear();
                        }

                        {
                            rowIndex++;
                            Methods.CopyExcelRegion(wsTemplate, 7, 0, 0, 11, wsReport, rowIndex, 0);

                            //DataRow dr = dtCampaignTotals.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'")[0];
                            DataRow dr = dtCampaignTotals.Select(campaignTotalsFilteringString).CopyToDataTable().Rows[0];
                            wsReport.GetCell("TotalSales").Value = dr["TotalSales"] as int?;
                            wsReport.GetCell("TotalPremium").Value = dr["TotalPremium"] as decimal?;
                            wsReport.GetCell("TotalUnits").Value = dr["TotalUnits"] as decimal?;
                            wsReport.GetCell("TotalReferrals").Value = dr["TotalReferrals"] as int?;
                            wsReport.GetCell("TotalIncentive").Value = dr["TotalIncentive"] as int?;
                            wsReport.Workbook.NamedReferences.Clear();
                        }

                        wsReport.Workbook.NamedReferences.Clear();
                        rowIndex += 2;
                    }
                }

                {
                    Methods.CopyExcelRegion(wsTemplate, 9, 0, 0, 11, wsReport, rowIndex, 0);

                    DataRow dr = dtGrandTotals.Rows[0];
                    wsReport.GetCell("GrandTotalSales").Value = dr["GrandTotalSales"] as int?;
                    wsReport.GetCell("GrandTotalPremium").Value = dr["GrandTotalPremium"] as decimal?;
                    wsReport.GetCell("GrandTotalUnits").Value = dr["GrandTotalUnits"] as decimal?;
                    wsReport.GetCell("GrandTotalReferrals").Value = dr["GrandTotalReferrals"] as int?;
                    wsReport.GetCell("GrandTotalIncentive").Value = dr["GrandTotalIncentive"] as int?;

                    wsReport.Workbook.NamedReferences.Clear();
                }

                #endregion Sales

                #region Cancellations
                DataTable dtcancellationCampaigns = ds.Tables[4];
                DataTable dtCancellations = ds.Tables[5];
                DataTable dtCancellationCampaignTotals = ds.Tables[6];
                DataTable dtCancellationGrandTotals = ds.Tables[7];
                //wsReport = wbReport.Worksheets.Add(string.Join("",("Cancellations-"+ agentName).Take(31)));

                try
                {
                    wsReport = wbReport.Worksheets.Add(string.Join("", ("Cancellations-" + agentName).Take(31)));
                }
                catch
                {
                    wsReport = wbReport.Worksheets.Add(string.Join("", ("Cancellations " + agentName + "a").Take(31)));
                }

                rowIndex = 4;
                wsReport.DisplayOptions.View = WorksheetView.Normal;

                wsReport.PrintOptions.PaperSize = PaperSize.A4;
                wsReport.PrintOptions.Orientation = Orientation.Portrait;
                wsReport.PrintOptions.LeftMargin = 0.3;
                wsReport.PrintOptions.RightMargin = 0.3;

                wsReport.Workbook.NamedReferences.Clear();
                Methods.CopyExcelRegion(wsTemplate, 0, 0, 3, 5, wsReport, 0, 0);

                #region Cancellation header data

                {
                    wsReport.GetCell("AgentName").Value = agentName;
                    wsReport.GetCell("SalesDates").Value = "Cancellations: " + dateRange;

                    //wsReport.GetCell("Batches").Value = batchCodes + ((char)65279);
                }
                #endregion Cancellation header data

                foreach (DataRow drCampaign in dtcancellationCampaigns.Rows)
                {
                    Methods.CopyExcelRegion(wsTemplate, 4, 0, 1, 13, wsReport, rowIndex, 0);
                    rowIndex++;

                    wsReport.GetCell("Campaign").Value = drCampaign["CampaignCode"] as string;

                    DataTable dtCampaignCancellations = dtCancellations.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'").CopyToDataTable();
                    foreach (DataRow drSale in dtCampaignCancellations.Rows)
                    {
                        rowIndex++;
                        Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, 13, wsReport, rowIndex, 0);

                        bool isGoldenLead = false;
                        CellFillPattern cellFill = null;

                        if ((drSale["RefNo"] as string).Contains("Golden"))
                        {
                            isGoldenLead = true;
                            cellFill = new CellFillPattern(new WorkbookColorInfo(Color.Gold), null, FillPatternStyle.Solid);
                        }

                        wsReport.GetCell("DateOfSale").Value = ((DateTime)drSale["DateOfSale"]).ToString("d");
                        if (isGoldenLead) { wsReport.GetCell("DateOfSale").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("Batch").Value = drSale["Batch"] as string;
                        if (isGoldenLead) { wsReport.GetCell("Batch").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("RefNo").Value = drSale["RefNo"] as string;
                        if (isGoldenLead) { wsReport.GetCell("RefNo").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("Client").Value = drSale["LeadName"] as string;
                        if (isGoldenLead) { wsReport.GetCell("Client").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("Premium").Value = drSale["Premium"] as decimal?;
                        if (isGoldenLead) { wsReport.GetCell("Premium").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("Units").Value = drSale["Units"] as decimal?;
                        if (isGoldenLead) { wsReport.GetCell("Units").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("Reason").Value = drSale["Reason"] as string;
                        if (isGoldenLead) { wsReport.GetCell("Reason").CellFormat.Fill = cellFill; }
                        wsReport.Workbook.NamedReferences.Clear();
                    }

                    {
                        rowIndex++;
                        Methods.CopyExcelRegion(wsTemplate, 7, 0, 0, 9, wsReport, rowIndex, 0);

                        DataRow dr = dtCancellationCampaignTotals.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'")[0];
                        wsReport.GetCell("TotalSales").Value = dr["TotalCancellations"] as int?;
                        wsReport.GetCell("TotalPremium").Value = dr["TotalPremium"] as decimal?;
                        wsReport.GetCell("TotalUnits").Value = dr["TotalUnits"] as decimal?;

                        wsReport.Workbook.NamedReferences.Clear();
                    }

                    wsReport.Workbook.NamedReferences.Clear();
                    rowIndex += 2;
                }

                {
                    Methods.CopyExcelRegion(wsTemplate, 9, 0, 0, 9, wsReport, rowIndex, 0);

                    DataRow dr = dtCancellationGrandTotals.Rows[0];
                    wsReport.GetCell("GrandTotalSales").Value = dr["GrandTotalCancellations"] as int?;
                    wsReport.GetCell("GrandTotalPremium").Value = dr["GrandTotalPremium"] as decimal?;
                    wsReport.GetCell("GrandTotalUnits").Value = dr["GrandTotalUnits"] as decimal?;

                    wsReport.Workbook.NamedReferences.Clear();
                }
                #endregion Cancellations

                #region DebiCheck Call Backs

                DataTable dtDebiCheckCallBacksCampaigns = ds.Tables[17];
                DataTable dtDebiCheckCallBacks = ds.Tables[18];
                DataTable dtDebiCheckCallBacksCampaignTotals = ds.Tables[19];
                DataTable dtDebiCheckcallBackGrandTotals = ds.Tables[20];
                //wsReport = wbReport.Worksheets.Add(string.Join("", ("DC Call Backs -" + agentName).Take(31)));

                try
                {
                    wsReport = wbReport.Worksheets.Add(string.Join("", ("DC Call Backs -" + agentName).Take(31)));
                }
                catch
                {
                    wsReport = wbReport.Worksheets.Add(string.Join("", ("DC Call Backs   " + agentName + "a").Take(31)));
                }

                rowIndex = 4;
                wsReport.DisplayOptions.View = WorksheetView.Normal;

                wsReport.PrintOptions.PaperSize = PaperSize.A4;
                wsReport.PrintOptions.Orientation = Orientation.Portrait;
                wsReport.PrintOptions.LeftMargin = 0.3;
                wsReport.PrintOptions.RightMargin = 0.3;

                wsReport.DisplayOptions.TabColorInfo = new WorkbookColorInfo(Color.Orange);


                wsReport.Workbook.NamedReferences.Clear();
                Methods.CopyExcelRegion(wsTemplate, 0, 0, 3, 4, wsReport, 0, 0);

                #region DebiCheck Call Backs header data

                {
                    wsReport.GetCell("AgentName").Value = agentName;
                    wsReport.GetCell("SalesDates").Value = "Debi Check Call Backs: " + dateRange;

                    //wsReport.GetCell("Batches").Value = batchCodes + ((char)65279);
                }
                #endregion DebiCheck Call Backs header data

                foreach (DataRow drCampaign in dtDebiCheckCallBacksCampaigns.Rows)
                {
                    Methods.CopyExcelRegion(wsTemplate, 4, 0, 1, 13, wsReport, rowIndex, 0);
                    rowIndex++;

                    wsReport.GetCell("Campaign").Value = drCampaign["CampaignCode"] as string;

                    DataTable dtCampaignDebiCheckCallBacks = dtDebiCheckCallBacks.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'").CopyToDataTable();
                    foreach (DataRow drSale in dtCampaignDebiCheckCallBacks.Rows)
                    {
                        rowIndex++;
                        Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, 13, wsReport, rowIndex, 0);

                        bool isGoldenLead = false;
                        CellFillPattern cellFill = null;

                        if ((drSale["RefNo"] as string).Contains("Golden"))
                        {
                            isGoldenLead = true;
                            cellFill = new CellFillPattern(new WorkbookColorInfo(Color.Gold), null, FillPatternStyle.Solid);
                        }

                        wsReport.GetCell("DateOfSale").Value = ((DateTime)drSale["DateOfSale"]).ToString("d");
                        if (isGoldenLead) { wsReport.GetCell("DateOfSale").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("Batch").Value = drSale["Batch"] as string;
                        if (isGoldenLead) { wsReport.GetCell("Batch").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("RefNo").Value = drSale["RefNo"] as string;
                        if (isGoldenLead) { wsReport.GetCell("RefNo").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("Client").Value = drSale["LeadName"] as string;
                        if (isGoldenLead) { wsReport.GetCell("Client").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("Premium").Value = drSale["Premium"] as decimal?;
                        if (isGoldenLead) { wsReport.GetCell("Premium").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("Units").Value = drSale["Units"] as decimal?;
                        if (isGoldenLead) { wsReport.GetCell("Units").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("Reason").Value = drSale["Reason"] as string;
                        if (isGoldenLead) { wsReport.GetCell("Reason").CellFormat.Fill = cellFill; }
                        wsReport.Workbook.NamedReferences.Clear();
                    }

                    {
                        rowIndex++;
                        Methods.CopyExcelRegion(wsTemplate, 7, 0, 0, 9, wsReport, rowIndex, 0);

                        DataRow dr = dtDebiCheckCallBacksCampaignTotals.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'")[0];
                        wsReport.GetCell("TotalSales").Value = dr["TotalDebiCheckCallBacks"] as int?;
                        wsReport.GetCell("TotalPremium").Value = dr["TotalPremium"] as decimal?;
                        wsReport.GetCell("TotalUnits").Value = dr["TotalUnits"] as decimal?;

                        wsReport.Workbook.NamedReferences.Clear();
                    }

                    wsReport.Workbook.NamedReferences.Clear();
                    rowIndex += 2;
                }

                {
                    Methods.CopyExcelRegion(wsTemplate, 9, 0, 0, 9, wsReport, rowIndex, 0);

                    DataRow dr = dtDebiCheckcallBackGrandTotals.Rows[0];
                    wsReport.GetCell("GrandTotalSales").Value = dr["GrandTotalDebiCheckCallBacks"] as int?;
                    wsReport.GetCell("GrandTotalPremium").Value = dr["GrandTotalPremium"] as decimal?;
                    wsReport.GetCell("GrandTotalUnits").Value = dr["GrandTotalUnits"] as decimal?;

                    wsReport.Workbook.NamedReferences.Clear();
                }
                #endregion DebiCheck Call Backs

                #region Reduced Premiums
                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/210674629/comments

                wsReport.Workbook.NamedReferences.Clear();

                DataTable dtReducedPremiumCampaigns = ds.Tables[13];
                DataTable dtReducedPremiums = ds.Tables[14];
                DataTable dtReducedPremiumCampaignTotals = ds.Tables[15];
                DataTable dtReducedPremiumGrandTotals = ds.Tables[16];
                //wsReport = wbReport.Worksheets.Add(string.Join("", ("Reduced Premiums - " + agentName).Take(31)));
                string newReducedPremiumWorksheetDescription = Methods.ParseWorksheetName(wbReport, String.Format("{0} Reduced Premiums", agentName));
                //wsReport = wbReport.Worksheets.Add(newReducedPremiumWorksheetDescription);

                try
                {
                    wsReport = wbReport.Worksheets.Add(newReducedPremiumWorksheetDescription);
                }
                catch
                {
                    wsReport = wbReport.Worksheets.Add(newReducedPremiumWorksheetDescription + "2");
                }

                rowIndex = 4;
                wsReport.DisplayOptions.View = WorksheetView.Normal;

                wsReport.PrintOptions.PaperSize = PaperSize.A4;
                wsReport.PrintOptions.Orientation = Orientation.Portrait;
                wsReport.PrintOptions.LeftMargin = 0.3;
                wsReport.PrintOptions.RightMargin = 0.3;

                //wsReport.Workbook.NamedReferences.Clear();
                Methods.CopyExcelRegion(wsTemplate, 0, 0, 3, 5, wsReport, 0, 0);

                #region Reduced Premium header data

                {
                    wsReport.GetCell("AgentName").Value = agentName;
                    wsReport.GetCell("SalesDates").Value = "Reduced Premiums: " + dateRange;
                    //wsReport.GetCell("Batches").Value = batchCodes + ((char)65279);
                }
                #endregion Reduced Premium header data

                foreach (DataRow drCampaign in dtReducedPremiumCampaigns.Rows)
                {
                    Methods.CopyExcelRegion(wsTemplate, 4, 11, 1, 8, wsReport, rowIndex, 0);
                    rowIndex++;

                    wsReport.GetCell("RPCampaign").Value = drCampaign["CampaignCode"] as string;

                    DataTable dtCampaignReducedPremiums = dtReducedPremiums.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'").CopyToDataTable();
                    foreach (DataRow drSale in dtCampaignReducedPremiums.Rows)
                    {
                        rowIndex++;
                        Methods.CopyExcelRegion(wsTemplate, 6, 11, 0, 11, wsReport, rowIndex, 0);

                        bool isGoldenLead = false;
                        CellFillPattern cellFill = null;

                        if ((drSale["RefNo"] as string).Contains("Golden"))
                        {
                            isGoldenLead = true;
                            cellFill = new CellFillPattern(new WorkbookColorInfo(Color.Gold), null, FillPatternStyle.Solid);
                        }

                        wsReport.GetCell("RPDateOfSale").Value = ((DateTime)drSale["DateOfSale"]).ToString("d");
                        if (isGoldenLead) { wsReport.GetCell("DateOfSale").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("RPBatch").Value = drSale["Batch"] as string;
                        if (isGoldenLead) { wsReport.GetCell("RPBatch").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("RPRefNo").Value = drSale["RefNo"] as string;
                        if (isGoldenLead) { wsReport.GetCell("RPRefNo").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("RPClient").Value = drSale["LeadName"] as string;
                        if (isGoldenLead) { wsReport.GetCell("RPClient").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("RPOriginalPremium").Value = drSale["OriginalPremium"] as decimal?;
                        if (isGoldenLead) { wsReport.GetCell("RPOriginalPremium").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("RPReducedPremium").Value = drSale["ReducedPremium"] as decimal?;
                        if (isGoldenLead) { wsReport.GetCell("RPReducedPremium").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("RPFallOffPremium").Value = drSale["FallOffPremium"] as decimal?;
                        if (isGoldenLead) { wsReport.GetCell("RPFallOffPremium").CellFormat.Fill = cellFill; }
                        wsReport.GetCell("RPUnits").Value = drSale["ReducedUnits"] as decimal?;
                        if (isGoldenLead) { wsReport.GetCell("RPUnits").CellFormat.Fill = cellFill; }

                        wsReport.Workbook.NamedReferences.Clear();
                    }

                    {
                        rowIndex++;
                        Methods.CopyExcelRegion(wsTemplate, 7, 11, 0, 11, wsReport, rowIndex, 0);

                        DataRow dr = dtReducedPremiumCampaignTotals.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'")[0];
                        wsReport.GetCell("RPTotalSales").Value = dr["ReducedPremiumCount"] as int?;
                        wsReport.GetCell("RPTotalOriginalPremium").Value = dr["TotalOriginalPremium"] as decimal?;
                        wsReport.GetCell("RPTotalReducedPremium").Value = dr["TotalReducedPremium"] as decimal?;
                        wsReport.GetCell("RPTotalFallOffPremium").Value = dr["TotalFallOffPremium"] as decimal?;
                        wsReport.GetCell("RPTotalUnits").Value = dr["TotalReducedUnits"] as decimal?;

                        wsReport.Workbook.NamedReferences.Clear();
                    }

                    wsReport.Workbook.NamedReferences.Clear();
                    rowIndex += 2;
                }

                {
                    Methods.CopyExcelRegion(wsTemplate, 9, 13, 0, 8, wsReport, rowIndex, 1);

                    DataRow dr = dtReducedPremiumGrandTotals.Rows[0];
                    wsReport.GetCell("RPGrandTotalSales").Value = dr["GrandTotalReducedPremiumCount"] as int?;
                    wsReport.GetCell("RPGrandTotalOriginalPremium").Value = dr["GrandTotalOriginalPremium"] as decimal?;
                    wsReport.GetCell("RPGrandTotalReducedPremium").Value = dr["GrandTotalReducedPremium"] as decimal?;
                    wsReport.GetCell("RPGrandTotalFallOffPremium").Value = dr["GrandTotalFallOffPremium"] as decimal?;
                    wsReport.GetCell("RPGrandTotalUnits").Value = dr["GrandTotalReducedUnits"] as decimal?;

                    wsReport.Workbook.NamedReferences.Clear();
                }

                #endregion Reduced Premiums

                #region Carried Forwards

                #region New Carried Forwards

                if (1 == 1)
                {
                    string strAgentName = string.Format("{0}...Carried Forwards", new string(agentName.Take(20).ToArray()));
                    if (strAgentName.Length > 31)
                    {
                        strAgentName = (new string(strAgentName.Take(31).ToArray()));
                        strAgentName = strAgentName.Remove(strAgentName.Length - 3, 3) + "...";
                    }
                    //wsReport = wbReport.Worksheets.Add(strAgentName);

                    try
                    {
                        wsReport = wbReport.Worksheets.Add(strAgentName);
                    }
                    catch
                    {
                        wsReport = wbReport.Worksheets.Add(strAgentName + "2");
                    }

                    wsReport.DisplayOptions.View = WorksheetView.Normal;

                    wsReport.PrintOptions.PaperSize = PaperSize.A4;
                    wsReport.PrintOptions.Orientation = Orientation.Portrait;
                    wsReport.PrintOptions.LeftMargin = 0.3;
                    wsReport.PrintOptions.RightMargin = 0.3;

                    wsTemplate = wbTemplate.Worksheets["CarriedForwards"];

                    wsReport.Workbook.NamedReferences.Clear();
                    Methods.CopyExcelRegion(wsTemplate, 0, 0, 3, 4, wsReport, 0, 1);

                    #region Carried Forward Header Data

                    {
                        wsReport.GetCell("AgentName").Value = agentName;
                        wsReport.GetCell("SalesDates").Value = "Carried Forwards: " + dateRange;
                    }

                    #endregion Carried Forward Header Data

                    rowIndex = 4;
                    wsReport.Workbook.NamedReferences.Clear();

                    foreach (DataRow drCampaign in dtCampaignsCF.Rows)
                    {
                        string filterString = drCampaign["FilterString"].ToString();
                        string orderByString = drCampaign["OrderByString"].ToString();
                        string campaignTotalsFilteringString = drCampaign["CampaignTotalsFilteringString"].ToString();

                        // Determine if there are any rows for the given partition
                        var filteredSheetRows = dtSalesCF.Select(filterString).AsEnumerable();
                        if (filteredSheetRows.Any())
                        {
                            DataTable dtCampaignSalesCF = dtSalesCF.Select(filterString, orderByString).CopyToDataTable();

                            Methods.CopyExcelRegion(wsTemplate, 4, 1, 1, 7, wsReport, rowIndex, 1);

                            rowIndex++;

                            wsReport.GetCell("CampaignCF2").Value = drCampaign["CampaignCode"] as string;

                            //DataTable dtCampaignSales = dtSales.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'").CopyToDataTable();

                            //string filterString = drCampaign["FilterString"].ToString();
                            //string orderByString = drCampaign["OrderByString"].ToString();
                            //string campaignTotalsFilteringString = drCampaign["CampaignTotalsFilteringString"].ToString();
                            //DataTable dtCampaignSales = dtSales.Select(filterString, orderByString).CopyToDataTable();

                            foreach (DataRow drSale in dtCampaignSalesCF.Rows)
                            {
                                rowIndex++;
                                Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, 7, wsReport, rowIndex, 0);

                                bool isGoldenLead = false;
                                CellFillPattern cellFill = null;

                                if ((drSale["RefNo"] as string).Contains("Golden"))
                                {
                                    isGoldenLead = true;
                                    cellFill = new CellFillPattern(new WorkbookColorInfo(Color.Gold), null, FillPatternStyle.Solid);
                                }

                                wsReport.GetCell("DateOfSaleCF2").Value = ((DateTime)drSale["DateOfSale"]).ToString("d");
                                if (isGoldenLead) { wsReport.GetCell("DateOfSaleCF2").CellFormat.Fill = cellFill; }

                                //wsReport.GetCell("BatchCF2").Value = drSale["Batch"] as string;
                                //if (isGoldenLead) { wsReport.GetCell("BatchCF2").CellFormat.Fill = cellFill; }

                                //wsReport.GetCell("RefNoCF2").Value = drSale["RefNo"] as string;
                                //if (isGoldenLead) { wsReport.GetCell("RefNoCF2").CellFormat.Fill = cellFill; }

                                //wsReport.GetCell("ClientCF2").Value = drSale["LeadName"] as string;
                                //if (isGoldenLead) { wsReport.GetCell("ClientCF2").CellFormat.Fill = cellFill; }

                                wsReport.GetCell("PremiumCF2").Value = drSale["Premium"] as decimal?;
                                if (isGoldenLead)
                                {
                                    wsReport.GetCell("PremiumCF2").CellFormat.Fill = cellFill;
                                    wsReport.GetCell("PremiumCF2").CellFormat.Font.ColorInfo = new WorkbookColorInfo(Color.Red);
                                }

                                wsReport.GetCell("UnitsCF2").Value = drSale["Units"] as decimal?;
                                if (isGoldenLead) { wsReport.GetCell("UnitsCF2").CellFormat.Fill = cellFill; }

                                wsReport.GetCell("CarriedForwardReasonCF2").Value = drSale["SavedStatus"] as string;
                                if (isGoldenLead) { wsReport.GetCell("CarriedForwardReasonCF2").CellFormat.Fill = cellFill; }

                                //wsReport.GetCell("SavedStatusOriginalDOS").Value = drSale["SavedStatusOriginalDOS"] as string;
                                //if (isGoldenLead) { wsReport.GetCell("SavedStatusOriginalDOS").CellFormat.Fill = cellFill; }

                                wsReport.Workbook.NamedReferences.Clear();
                            }

                            {
                                rowIndex++;
                                Methods.CopyExcelRegion(wsTemplate, 7, 0, 0, 6, wsReport, rowIndex, 0);

                                //DataRow dr = dtCampaignTotals.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'")[0];
                                DataRow dr = dtCampaignTotalsCF.Select(campaignTotalsFilteringString).CopyToDataTable().Rows[0];
                                wsReport.GetCell("TotalSalesCF2").Value = dr["TotalSales"] as int?;
                                wsReport.GetCell("TotalPremiumCF2").Value = dr["TotalPremium"] as decimal?;
                                wsReport.GetCell("TotalUnitsCF2").Value = dr["TotalUnits"] as decimal?;

                                wsReport.Workbook.NamedReferences.Clear();
                            }

                            wsReport.Workbook.NamedReferences.Clear();
                            rowIndex += 2;
                        }
                    }

                    {
                        Methods.CopyExcelRegion(wsTemplate, 9, 0, 0, 6, wsReport, rowIndex, 0);

                        DataRow dr = dtGrandTotalsCF.Rows[0];
                        wsReport.GetCell("GrandTotalSalesCF2").Value = dr["GrandTotalSales"] as int?;
                        wsReport.GetCell("GrandTotalPremiumCF2").Value = dr["GrandTotalPremium"] as decimal?;
                        wsReport.GetCell("GrandTotalUnitsCF2").Value = dr["GrandTotalUnits"] as decimal?;

                        wsReport.Workbook.NamedReferences.Clear();
                    }
                }

                #endregion  New Carried Forwards

                #region Old Carried Forwards

                if (0 == 1)
                {
                    DataTable dtIncludeCarriedForwardBreakdown = ds.Tables[8];
                    DataTable dtCarriedForwardCampaigns = ds.Tables[9];
                    DataTable dtCarriedForwards = ds.Tables[10];
                    DataTable dtCarriedForwardCampaignTotals = ds.Tables[11];
                    DataTable dtCarriedForwardGrandTotals = ds.Tables[12];
                    //wsReport = wbReport.Worksheets.Add(string.Join("", ("Carried Forwards - " + agentName).Take(31)));
                    string newCarriedForwardsWorksheetDescription = Methods.ParseWorksheetName(wbReport, String.Format("{0} Carried Forwards", agentName));
                    //wsReport = wbReport.Worksheets.Add(newCarriedForwardsWorksheetDescription);

                    try
                    {
                        wsReport = wbReport.Worksheets.Add(newCarriedForwardsWorksheetDescription);
                    }
                    catch
                    {
                        wsReport = wbReport.Worksheets.Add(newCarriedForwardsWorksheetDescription + "a");
                    }

                    rowIndex = 4;
                    wsReport.DisplayOptions.View = WorksheetView.Normal;

                    wsReport.PrintOptions.PaperSize = PaperSize.A4;
                    wsReport.PrintOptions.Orientation = Orientation.Portrait;
                    wsReport.PrintOptions.LeftMargin = 0.3;
                    wsReport.PrintOptions.RightMargin = 0.3;

                    wsReport.Workbook.NamedReferences.Clear();
                    Methods.CopyExcelRegion(wsTemplate, 0, 0, 3, 5, wsReport, 0, 0);

                    #region Carried Forward Header Data

                    {
                        wsReport.GetCell("AgentName").Value = agentName;
                        wsReport.GetCell("SalesDates").Value = "Carried Forwards: " + dateRange;

                        //wsReport.GetCell("Batches").Value = batchCodes + ((char)65279);
                    }
                    #endregion Carried Forward Header Data

                    #region Determining if a detailed breakdown of the carried forwards should be included

                    bool includeCarriedForwardBreakdown = false;

                    if (dtIncludeCarriedForwardBreakdown.Rows.Count > 0)
                    {
                        includeCarriedForwardBreakdown = Convert.ToBoolean(dtIncludeCarriedForwardBreakdown.Rows[0]["IncludeCarriedForwardBreakdown"]);
                    }

                    #endregion Determining if a detailed breakdown of the carried forwards should be included

                    if (includeCarriedForwardBreakdown)
                    {
                        foreach (DataRow drCampaign in dtCarriedForwardCampaigns.Rows)
                        {
                            Methods.CopyExcelRegion(wsTemplate, 4, 0, 1, 5, wsReport, rowIndex, 0);
                            rowIndex++;

                            wsReport.GetCell("Campaign").Value = drCampaign["CampaignCode"] as string;

                            DataTable dtCampaignCancellations = dtCarriedForwards.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'").CopyToDataTable();
                            foreach (DataRow drSale in dtCarriedForwards.Rows)
                            {
                                rowIndex++;
                                Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, 5, wsReport, rowIndex, 0);

                                bool isGoldenLead = false;
                                CellFillPattern cellFill = null;

                                if ((drSale["RefNo"] as string).Contains("Golden"))
                                {
                                    isGoldenLead = true;
                                    cellFill = new CellFillPattern(new WorkbookColorInfo(Color.Gold), null, FillPatternStyle.Solid);
                                }

                                wsReport.GetCell("DateOfSale").Value = ((DateTime)drSale["DateOfSale"]).ToString("d");
                                if (isGoldenLead) { wsReport.GetCell("DateOfSale").CellFormat.Fill = cellFill; }
                                wsReport.GetCell("Batch").Value = drSale["Batch"] as string;
                                if (isGoldenLead) { wsReport.GetCell("Batch").CellFormat.Fill = cellFill; }
                                wsReport.GetCell("RefNo").Value = drSale["RefNo"] as string;
                                if (isGoldenLead) { wsReport.GetCell("RefNo").CellFormat.Fill = cellFill; }
                                wsReport.GetCell("Client").Value = drSale["LeadName"] as string;
                                if (isGoldenLead) { wsReport.GetCell("Client").CellFormat.Fill = cellFill; }
                                wsReport.GetCell("Premium").Value = drSale["Premium"] as decimal?;
                                if (isGoldenLead) { wsReport.GetCell("Premium").CellFormat.Fill = cellFill; }
                                wsReport.GetCell("Units").Value = drSale["Units"] as decimal?;
                                if (isGoldenLead) { wsReport.GetCell("Units").CellFormat.Fill = cellFill; }

                                wsReport.Workbook.NamedReferences.Clear();
                            }

                            {
                                rowIndex++;
                                Methods.CopyExcelRegion(wsTemplate, 7, 0, 0, 5, wsReport, rowIndex, 0);

                                DataRow dr = dtCarriedForwardCampaignTotals.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'")[0];
                                wsReport.GetCell("TotalSales").Value = dr["TotalCarriedForwards"] as int?;
                                wsReport.GetCell("TotalPremium").Value = dr["TotalPremium"] as decimal?;
                                wsReport.GetCell("TotalUnits").Value = dr["TotalUnits"] as decimal?;

                                wsReport.Workbook.NamedReferences.Clear();
                            }

                            wsReport.Workbook.NamedReferences.Clear();
                            rowIndex += 2;
                        }

                        {
                            Methods.CopyExcelRegion(wsTemplate, 9, 0, 0, 5, wsReport, rowIndex, 0);

                            DataRow dr = dtCarriedForwardGrandTotals.Rows[0];
                            wsReport.GetCell("GrandTotalSales").Value = dr["GrandTotalCarriedForwards"] as int?;
                            wsReport.GetCell("GrandTotalPremium").Value = dr["GrandTotalPremium"] as decimal?;
                            wsReport.GetCell("GrandTotalUnits").Value = dr["GrandTotalUnits"] as decimal?;

                            wsReport.Workbook.NamedReferences.Clear();
                        }
                    }
                    else
                    {

                        DataRow drCarriedForwardGrandTotal = dtCarriedForwardGrandTotals.Rows[0];

                        Methods.CopyExcelRegion(wsTemplate, 12, 0, 2, 4, wsReport, rowIndex, 0);

                        wsReport.GetCell("GrandTotalCarriedForwardsBase").Value = drCarriedForwardGrandTotal["GrandTotalCarriedForwardsBase"] as int?;
                        wsReport.GetCell("GrandTotalCarriedForwardPremium").Value = Methods.ForceCurrencyFormatting(drCarriedForwardGrandTotal["GrandTotalCarriedForwardPremium"], false);
                        wsReport.GetCell("GrandTotalCarriedForwardUpgrades").Value = drCarriedForwardGrandTotal["GrandTotalCarriedForwardUpgrades"] as int?;
                        wsReport.GetCell("GrandTotalCarriedForwardUnits").Value = drCarriedForwardGrandTotal["GrandTotalCarriedForwardUnits"] as decimal?;

                        wsReport.Workbook.NamedReferences.Clear();
                    }
                }

                #endregion  Old Carried Forwards

                #endregion Carried Forwards



                //wsReport.Rows[rowIndex].Cells[5].CellFormat.SetFormatting(wsTemplate.Rows[5].Cells[5].CellFormat) ;//= "# ##0.00;[Red]# ##0.00";
            }

            #endregion

        }

        private void ReportBodyDC(Workbook wbTemplate, Workbook wbReport, string agentName, long? agentID, string dateRange)
        {

            try
            {

                #region Retrieve data from database

                SqlParameter[] parameters =
                    {
                    new SqlParameter("@AgentID", agentID),
                    new SqlParameter("@FromDate", _fromDate),
                    new SqlParameter("@ToDate", _toDate)
                };

                DataSet ds = null;


                ds = Methods.ExecuteStoredProcedureSaleReport("spINReportSalesDC", parameters);



                DataTable dtCampaigns = ds.Tables[0];
                DataTable dtCampaignsCF = ds.Tables[21];
                DataTable dtSales = ds.Tables[1];
                DataTable dtSalesCF = ds.Tables[22];
                DataTable dtCampaignTotals = ds.Tables[2];
                DataTable dtCampaignTotalsCF = ds.Tables[23];
                DataTable dtGrandTotals = ds.Tables[3];
                DataTable dtGrandTotalsCF = ds.Tables[24];

                //bool isUpgradeCampaign = Insure.IsUpgradeCampaign(drCampaign["CampaignID"] as long?);


                #endregion retrieve data from database

                #region setup worksheet

                //WorksheetCell wsCell;
                Worksheet wsTemplate = wbTemplate.Worksheets["Report"];
                //Worksheet wsReport = wbReport.Worksheets.Add(string.Join("", agentName.Take(31)));

                Worksheet wsReport;

                try
                {
                    wsReport = wbReport.Worksheets.Add(string.Join("", agentName.Take(31)));
                }
                catch
                {
                    wsReport = wbReport.Worksheets.Add(string.Join("", agentName + "a".Take(31)));
                }

                wsReport.DisplayOptions.View = WorksheetView.Normal;

                wsReport.PrintOptions.PaperSize = PaperSize.A4;
                wsReport.PrintOptions.Orientation = Orientation.Portrait;
                wsReport.PrintOptions.LeftMargin = 0.3;
                wsReport.PrintOptions.RightMargin = 0.3;

                wsReport.Workbook.NamedReferences.Clear();
                Methods.CopyExcelRegion(wsTemplate, 0, 0, 3, 4, wsReport, 0, 0);

                #endregion

                #region header data

                {
                    wsReport.GetCell("AgentName").Value = agentName;
                    wsReport.GetCell("SalesDates").Value = "Sales: " + dateRange;

                    //wsReport.GetCell("Batches").Value = batchCodes + ((char)65279);
                }

                #endregion

                #region report data

                {
                    #region Sales

                    int rowIndex = 4;
                    wsReport.Workbook.NamedReferences.Clear();

                    foreach (DataRow drCampaign in dtCampaigns.Rows)
                    {
                        string filterString = drCampaign["FilterString"].ToString();
                        string orderByString = drCampaign["OrderByString"].ToString();
                        string campaignTotalsFilteringString = drCampaign["CampaignTotalsFilteringString"].ToString();

                        // Determine if there are any rows for the given partition
                        var filteredSheetRows = dtSales.Select(filterString).AsEnumerable();
                        if (filteredSheetRows.Any())
                        {
                            DataTable dtCampaignSales = dtSales.Select(filterString, orderByString).CopyToDataTable();

                            Methods.CopyExcelRegion(wsTemplate, 4, 0, 1, 11, wsReport, rowIndex, 0);

                            rowIndex++;

                            wsReport.GetCell("Campaign").Value = drCampaign["CampaignCode"] as string;

                            //DataTable dtCampaignSales = dtSales.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'").CopyToDataTable();

                            //string filterString = drCampaign["FilterString"].ToString();
                            //string orderByString = drCampaign["OrderByString"].ToString();
                            //string campaignTotalsFilteringString = drCampaign["CampaignTotalsFilteringString"].ToString();
                            //DataTable dtCampaignSales = dtSales.Select(filterString, orderByString).CopyToDataTable();

                            foreach (DataRow drSale in dtCampaignSales.Rows)
                            {
                                rowIndex++;
                                Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, 11, wsReport, rowIndex, 0);

                                bool isGoldenLead = false;
                                CellFillPattern cellFill = null;

                                if ((drSale["RefNo"] as string).Contains("Golden"))
                                {
                                    isGoldenLead = true;
                                    cellFill = new CellFillPattern(new WorkbookColorInfo(Color.Gold), null, FillPatternStyle.Solid);
                                }

                                wsReport.GetCell("DateOfSale").Value = ((DateTime)drSale["DateOfSale"]).ToString("d");
                                if (isGoldenLead) { wsReport.GetCell("DateOfSale").CellFormat.Fill = cellFill; }
                                wsReport.GetCell("Batch").Value = drSale["Batch"] as string;
                                if (isGoldenLead) { wsReport.GetCell("Batch").CellFormat.Fill = cellFill; }
                                wsReport.GetCell("RefNo").Value = drSale["RefNo"] as string;
                                if (isGoldenLead) { wsReport.GetCell("RefNo").CellFormat.Fill = cellFill; }
                                wsReport.GetCell("Client").Value = drSale["LeadName"] as string;
                                if (isGoldenLead) { wsReport.GetCell("Client").CellFormat.Fill = cellFill; }
                                wsReport.GetCell("Premium").Value = drSale["Premium"] as decimal?;
                                if (isGoldenLead)
                                {
                                    wsReport.GetCell("Premium").CellFormat.Fill = cellFill;
                                    wsReport.GetCell("Premium").CellFormat.Font.ColorInfo = new WorkbookColorInfo(Color.Red);
                                    //wsReport.GetCell("Premium").Value = "[" + wsReport.GetCell("Premium").Value + "]";
                                }
                                wsReport.GetCell("Units").Value = drSale["Units"] as decimal?;
                                if (isGoldenLead) { wsReport.GetCell("Units").CellFormat.Fill = cellFill; }
                                wsReport.GetCell("SavedStatus").Value = drSale["SavedStatus"] as string;
                                if (isGoldenLead) { wsReport.GetCell("SavedStatus").CellFormat.Fill = cellFill; }
                                wsReport.GetCell("SavedStatusOriginalDOS").Value = drSale["SavedStatusOriginalDOS"] as string;
                                if (isGoldenLead) { wsReport.GetCell("SavedStatusOriginalDOS").CellFormat.Fill = cellFill; }
                                wsReport.GetCell("ReferralResult").Value = drSale["ReferralResult"] as string;
                                if (isGoldenLead) { wsReport.GetCell("ReferralResult").CellFormat.Fill = cellFill; }
                                wsReport.GetCell("MandateStatus").Value = drSale["MandateStatus"] as string;
                                if (isGoldenLead) { wsReport.GetCell("MandateStatus").CellFormat.Fill = cellFill; }
                                wsReport.GetCell("Incentive").Value = drSale["Incentive"] as int?;
                                if (isGoldenLead) { wsReport.GetCell("Incentive").CellFormat.Fill = cellFill; }
                                wsReport.GetCell("DCSpecialist").Value = drSale["DCSpecialist"] as string;
                                if (isGoldenLead) { wsReport.GetCell("DCSpecialist").CellFormat.Fill = cellFill; }


                                wsReport.Workbook.NamedReferences.Clear();
                            }

                            {
                                rowIndex++;
                                Methods.CopyExcelRegion(wsTemplate, 7, 0, 0, 11, wsReport, rowIndex, 0);

                                //DataRow dr = dtCampaignTotals.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'")[0];
                                DataRow dr = dtCampaignTotals.Select(campaignTotalsFilteringString).CopyToDataTable().Rows[0];
                                wsReport.GetCell("TotalSales").Value = dr["TotalSales"] as int?;
                                wsReport.GetCell("TotalPremium").Value = dr["TotalPremium"] as decimal?;
                                wsReport.GetCell("TotalUnits").Value = dr["TotalUnits"] as decimal?;
                                wsReport.GetCell("TotalReferrals").Value = dr["TotalReferrals"] as int?;
                                wsReport.GetCell("TotalIncentive").Value = dr["TotalIncentive"] as int?;
                                wsReport.Workbook.NamedReferences.Clear();
                            }

                            wsReport.Workbook.NamedReferences.Clear();
                            rowIndex += 2;
                        }
                    }

                    {
                        Methods.CopyExcelRegion(wsTemplate, 9, 0, 0, 12, wsReport, rowIndex, 0);

                        DataRow dr = dtGrandTotals.Rows[0];
                        wsReport.GetCell("GrandTotalSales").Value = dr["GrandTotalSales"] as int?;
                        wsReport.GetCell("GrandTotalPremium").Value = dr["GrandTotalPremium"] as decimal?;
                        wsReport.GetCell("GrandTotalUnits").Value = dr["GrandTotalUnits"] as decimal?;
                        wsReport.GetCell("GrandTotalReferrals").Value = dr["GrandTotalReferrals"] as int?;
                        wsReport.GetCell("GrandTotalIncentive").Value = dr["GrandTotalIncentive"] as int?;

                        wsReport.Workbook.NamedReferences.Clear();
                    }

                    #endregion Sales

                    #region Cancellations
                    DataTable dtcancellationCampaigns = ds.Tables[4];
                    DataTable dtCancellations = ds.Tables[5];
                    DataTable dtCancellationCampaignTotals = ds.Tables[6];
                    DataTable dtCancellationGrandTotals = ds.Tables[7];
                    //wsReport = wbReport.Worksheets.Add(string.Join("", ("Cancellations-" + agentName).Take(31)));

                    try
                    {
                        wsReport = wbReport.Worksheets.Add(string.Join("", ("Cancellations-" + agentName).Take(31)));
                    }
                    catch
                    {
                        wsReport = wbReport.Worksheets.Add(string.Join("", ("Cancellations    " + agentName + "a").Take(31)));
                    }

                    rowIndex = 4;
                    wsReport.DisplayOptions.View = WorksheetView.Normal;

                    wsReport.PrintOptions.PaperSize = PaperSize.A4;
                    wsReport.PrintOptions.Orientation = Orientation.Portrait;
                    wsReport.PrintOptions.LeftMargin = 0.3;
                    wsReport.PrintOptions.RightMargin = 0.3;

                    wsReport.Workbook.NamedReferences.Clear();
                    Methods.CopyExcelRegion(wsTemplate, 0, 0, 3, 5, wsReport, 0, 0);

                    #region Cancellation header data

                    {
                        wsReport.GetCell("AgentName").Value = agentName;
                        wsReport.GetCell("SalesDates").Value = "Cancellations- " + dateRange;

                        //wsReport.GetCell("Batches").Value = batchCodes + ((char)65279);
                    }
                    #endregion Cancellation header data

                    foreach (DataRow drCampaign in dtcancellationCampaigns.Rows)
                    {
                        Methods.CopyExcelRegion(wsTemplate, 4, 0, 1, 13, wsReport, rowIndex, 0);
                        rowIndex++;

                        wsReport.GetCell("Campaign").Value = drCampaign["CampaignCode"] as string;

                        DataTable dtCampaignCancellations = dtCancellations.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'").CopyToDataTable();
                        foreach (DataRow drSale in dtCampaignCancellations.Rows)
                        {
                            rowIndex++;
                            Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, 13, wsReport, rowIndex, 0);

                            bool isGoldenLead = false;
                            CellFillPattern cellFill = null;

                            if ((drSale["RefNo"] as string).Contains("Golden"))
                            {
                                isGoldenLead = true;
                                cellFill = new CellFillPattern(new WorkbookColorInfo(Color.Gold), null, FillPatternStyle.Solid);
                            }

                            wsReport.GetCell("DateOfSale").Value = ((DateTime)drSale["DateOfSale"]).ToString("d");
                            if (isGoldenLead) { wsReport.GetCell("DateOfSale").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("Batch").Value = drSale["Batch"] as string;
                            if (isGoldenLead) { wsReport.GetCell("Batch").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("RefNo").Value = drSale["RefNo"] as string;
                            if (isGoldenLead) { wsReport.GetCell("RefNo").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("Client").Value = drSale["LeadName"] as string;
                            if (isGoldenLead) { wsReport.GetCell("Client").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("Premium").Value = drSale["Premium"] as decimal?;
                            if (isGoldenLead) { wsReport.GetCell("Premium").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("Units").Value = drSale["Units"] as decimal?;
                            if (isGoldenLead) { wsReport.GetCell("Units").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("Reason").Value = drSale["Reason"] as string;
                            if (isGoldenLead) { wsReport.GetCell("Reason").CellFormat.Fill = cellFill; }

                            wsReport.Workbook.NamedReferences.Clear();
                        }

                        {
                            rowIndex++;
                            Methods.CopyExcelRegion(wsTemplate, 7, 0, 0, 9, wsReport, rowIndex, 0);

                            DataRow dr = dtCancellationCampaignTotals.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'")[0];
                            wsReport.GetCell("TotalSales").Value = dr["TotalCancellations"] as int?;
                            wsReport.GetCell("TotalPremium").Value = dr["TotalPremium"] as decimal?;
                            wsReport.GetCell("TotalUnits").Value = dr["TotalUnits"] as decimal?;

                            wsReport.Workbook.NamedReferences.Clear();
                        }

                        wsReport.Workbook.NamedReferences.Clear();
                        rowIndex += 2;
                    }

                    {
                        Methods.CopyExcelRegion(wsTemplate, 9, 0, 0, 9, wsReport, rowIndex, 0);

                        DataRow dr = dtCancellationGrandTotals.Rows[0];
                        wsReport.GetCell("GrandTotalSales").Value = dr["GrandTotalCancellations"] as int?;
                        wsReport.GetCell("GrandTotalPremium").Value = dr["GrandTotalPremium"] as decimal?;
                        wsReport.GetCell("GrandTotalUnits").Value = dr["GrandTotalUnits"] as decimal?;

                        wsReport.Workbook.NamedReferences.Clear();
                    }
                    #endregion Cancellations

                    #region DebiCheck Call Backs

                    DataTable dtDebiCheckCallBacksCampaigns = ds.Tables[17];
                    DataTable dtDebiCheckCallBacks = ds.Tables[18];
                    DataTable dtDebiCheckCallBacksCampaignTotals = ds.Tables[19];
                    DataTable dtDebiCheckcallBackGrandTotals = ds.Tables[20];
                    //wsReport = wbReport.Worksheets.Add(string.Join("", ("DC Call Backs -" + agentName).Take(31)));

                    try
                    {
                        wsReport = wbReport.Worksheets.Add(string.Join("", ("DC Call Backs -" + agentName).Take(31)));
                    }
                    catch
                    {
                        wsReport = wbReport.Worksheets.Add(string.Join("", ("DC Call Backs   " + agentName + "a").Take(31)));
                    }

                    rowIndex = 4;
                    wsReport.DisplayOptions.View = WorksheetView.Normal;

                    wsReport.PrintOptions.PaperSize = PaperSize.A4;
                    wsReport.PrintOptions.Orientation = Orientation.Portrait;
                    wsReport.PrintOptions.LeftMargin = 0.3;
                    wsReport.PrintOptions.RightMargin = 0.3;

                    wsReport.DisplayOptions.TabColorInfo = new WorkbookColorInfo(Color.Orange);


                    wsReport.Workbook.NamedReferences.Clear();
                    Methods.CopyExcelRegion(wsTemplate, 0, 0, 3, 4, wsReport, 0, 0);

                    #region DebiCheck Call Backs header data

                    {
                        wsReport.GetCell("AgentName").Value = agentName;
                        wsReport.GetCell("SalesDates").Value = "Debi Check Call Backs: " + dateRange;

                        //wsReport.GetCell("Batches").Value = batchCodes + ((char)65279);
                    }
                    #endregion DebiCheck Call Backs header data

                    foreach (DataRow drCampaign in dtDebiCheckCallBacksCampaigns.Rows)
                    {
                        Methods.CopyExcelRegion(wsTemplate, 4, 0, 1, 13, wsReport, rowIndex, 0);
                        rowIndex++;

                        wsReport.GetCell("Campaign").Value = drCampaign["CampaignCode"] as string;

                        DataTable dtCampaignDebiCheckCallBacks = dtDebiCheckCallBacks.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'").CopyToDataTable();
                        foreach (DataRow drSale in dtCampaignDebiCheckCallBacks.Rows)
                        {
                            rowIndex++;
                            Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, 13, wsReport, rowIndex, 0);

                            bool isGoldenLead = false;
                            CellFillPattern cellFill = null;

                            if ((drSale["RefNo"] as string).Contains("Golden"))
                            {
                                isGoldenLead = true;
                                cellFill = new CellFillPattern(new WorkbookColorInfo(Color.Gold), null, FillPatternStyle.Solid);
                            }

                            wsReport.GetCell("DateOfSale").Value = ((DateTime)drSale["DateOfSale"]).ToString("d");
                            if (isGoldenLead) { wsReport.GetCell("DateOfSale").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("Batch").Value = drSale["Batch"] as string;
                            if (isGoldenLead) { wsReport.GetCell("Batch").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("RefNo").Value = drSale["RefNo"] as string;
                            if (isGoldenLead) { wsReport.GetCell("RefNo").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("Client").Value = drSale["LeadName"] as string;
                            if (isGoldenLead) { wsReport.GetCell("Client").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("Premium").Value = drSale["Premium"] as decimal?;
                            if (isGoldenLead) { wsReport.GetCell("Premium").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("Units").Value = drSale["Units"] as decimal?;
                            if (isGoldenLead) { wsReport.GetCell("Units").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("Reason").Value = drSale["Reason"] as string;
                            if (isGoldenLead) { wsReport.GetCell("Reason").CellFormat.Fill = cellFill; }
                            wsReport.Workbook.NamedReferences.Clear();
                        }

                        {
                            rowIndex++;
                            Methods.CopyExcelRegion(wsTemplate, 7, 0, 0, 9, wsReport, rowIndex, 0);

                            DataRow dr = dtDebiCheckCallBacksCampaignTotals.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'")[0];
                            wsReport.GetCell("TotalSales").Value = dr["TotalDebiCheckCallBacks"] as int?;
                            wsReport.GetCell("TotalPremium").Value = dr["TotalPremium"] as decimal?;
                            wsReport.GetCell("TotalUnits").Value = dr["TotalUnits"] as decimal?;

                            wsReport.Workbook.NamedReferences.Clear();
                        }

                        wsReport.Workbook.NamedReferences.Clear();
                        rowIndex += 2;
                    }

                    {
                        Methods.CopyExcelRegion(wsTemplate, 9, 0, 0, 9, wsReport, rowIndex, 0);

                        DataRow dr = dtDebiCheckcallBackGrandTotals.Rows[0];
                        wsReport.GetCell("GrandTotalSales").Value = dr["GrandTotalDebiCheckCallBacks"] as int?;
                        wsReport.GetCell("GrandTotalPremium").Value = dr["GrandTotalPremium"] as decimal?;
                        wsReport.GetCell("GrandTotalUnits").Value = dr["GrandTotalUnits"] as decimal?;

                        wsReport.Workbook.NamedReferences.Clear();
                    }
                    #endregion DebiCheck Call Backs

                    #region Reduced Premiums
                    // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/210674629/comments

                    wsReport.Workbook.NamedReferences.Clear();

                    DataTable dtReducedPremiumCampaigns = ds.Tables[13];
                    DataTable dtReducedPremiums = ds.Tables[14];
                    DataTable dtReducedPremiumCampaignTotals = ds.Tables[15];
                    DataTable dtReducedPremiumGrandTotals = ds.Tables[16];
                    //wsReport = wbReport.Worksheets.Add(string.Join("", ("Reduced Premiums - " + agentName).Take(31)));
                    string newReducedPremiumWorksheetDescription = Methods.ParseWorksheetName(wbReport, String.Format("{0} Reduced Premiums", agentName));
                    //wsReport = wbReport.Worksheets.Add(newReducedPremiumWorksheetDescription);

                    try
                    {
                        wsReport = wbReport.Worksheets.Add(newReducedPremiumWorksheetDescription);
                    }
                    catch
                    {
                        wsReport = wbReport.Worksheets.Add(newReducedPremiumWorksheetDescription + "2");
                    }

                    rowIndex = 4;
                    wsReport.DisplayOptions.View = WorksheetView.Normal;

                    wsReport.PrintOptions.PaperSize = PaperSize.A4;
                    wsReport.PrintOptions.Orientation = Orientation.Portrait;
                    wsReport.PrintOptions.LeftMargin = 0.3;
                    wsReport.PrintOptions.RightMargin = 0.3;

                    //wsReport.Workbook.NamedReferences.Clear();
                    Methods.CopyExcelRegion(wsTemplate, 0, 0, 3, 5, wsReport, 0, 0);

                    #region Reduced Premium header data

                    {
                        wsReport.GetCell("AgentName").Value = agentName;
                        wsReport.GetCell("SalesDates").Value = "Reduced Premiums: " + dateRange;
                        //wsReport.GetCell("Batches").Value = batchCodes + ((char)65279);
                    }
                    #endregion Reduced Premium header data

                    foreach (DataRow drCampaign in dtReducedPremiumCampaigns.Rows)
                    {
                        Methods.CopyExcelRegion(wsTemplate, 4, 11, 1, 8, wsReport, rowIndex, 0);
                        rowIndex++;

                        wsReport.GetCell("RPCampaign").Value = drCampaign["CampaignCode"] as string;

                        DataTable dtCampaignReducedPremiums = dtReducedPremiums.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'").CopyToDataTable();
                        foreach (DataRow drSale in dtCampaignReducedPremiums.Rows)
                        {
                            rowIndex++;
                            Methods.CopyExcelRegion(wsTemplate, 6, 11, 0, 11, wsReport, rowIndex, 0);

                            bool isGoldenLead = false;
                            CellFillPattern cellFill = null;

                            if ((drSale["RefNo"] as string).Contains("Golden"))
                            {
                                isGoldenLead = true;
                                cellFill = new CellFillPattern(new WorkbookColorInfo(Color.Gold), null, FillPatternStyle.Solid);
                            }

                            wsReport.GetCell("RPDateOfSale").Value = ((DateTime)drSale["DateOfSale"]).ToString("d");
                            if (isGoldenLead) { wsReport.GetCell("DateOfSale").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("RPBatch").Value = drSale["Batch"] as string;
                            if (isGoldenLead) { wsReport.GetCell("RPBatch").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("RPRefNo").Value = drSale["RefNo"] as string;
                            if (isGoldenLead) { wsReport.GetCell("RPRefNo").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("RPClient").Value = drSale["LeadName"] as string;
                            if (isGoldenLead) { wsReport.GetCell("RPClient").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("RPOriginalPremium").Value = drSale["OriginalPremium"] as decimal?;
                            if (isGoldenLead) { wsReport.GetCell("RPOriginalPremium").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("RPReducedPremium").Value = drSale["ReducedPremium"] as decimal?;
                            if (isGoldenLead) { wsReport.GetCell("RPReducedPremium").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("RPFallOffPremium").Value = drSale["FallOffPremium"] as decimal?;
                            if (isGoldenLead) { wsReport.GetCell("RPFallOffPremium").CellFormat.Fill = cellFill; }
                            wsReport.GetCell("RPUnits").Value = drSale["ReducedUnits"] as decimal?;
                            if (isGoldenLead) { wsReport.GetCell("RPUnits").CellFormat.Fill = cellFill; }

                            wsReport.Workbook.NamedReferences.Clear();
                        }

                        {
                            rowIndex++;
                            Methods.CopyExcelRegion(wsTemplate, 7, 11, 0, 11, wsReport, rowIndex, 0);

                            DataRow dr = dtReducedPremiumCampaignTotals.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'")[0];
                            wsReport.GetCell("RPTotalSales").Value = dr["ReducedPremiumCount"] as int?;
                            wsReport.GetCell("RPTotalOriginalPremium").Value = dr["TotalOriginalPremium"] as decimal?;
                            wsReport.GetCell("RPTotalReducedPremium").Value = dr["TotalReducedPremium"] as decimal?;
                            wsReport.GetCell("RPTotalFallOffPremium").Value = dr["TotalFallOffPremium"] as decimal?;
                            wsReport.GetCell("RPTotalUnits").Value = dr["TotalReducedUnits"] as decimal?;

                            wsReport.Workbook.NamedReferences.Clear();
                        }

                        wsReport.Workbook.NamedReferences.Clear();
                        rowIndex += 2;
                    }

                    {
                        Methods.CopyExcelRegion(wsTemplate, 9, 13, 0, 8, wsReport, rowIndex, 1);

                        DataRow dr = dtReducedPremiumGrandTotals.Rows[0];
                        wsReport.GetCell("RPGrandTotalSales").Value = dr["GrandTotalReducedPremiumCount"] as int?;
                        wsReport.GetCell("RPGrandTotalOriginalPremium").Value = dr["GrandTotalOriginalPremium"] as decimal?;
                        wsReport.GetCell("RPGrandTotalReducedPremium").Value = dr["GrandTotalReducedPremium"] as decimal?;
                        wsReport.GetCell("RPGrandTotalFallOffPremium").Value = dr["GrandTotalFallOffPremium"] as decimal?;
                        wsReport.GetCell("RPGrandTotalUnits").Value = dr["GrandTotalReducedUnits"] as decimal?;

                        wsReport.Workbook.NamedReferences.Clear();
                    }

                    #endregion Reduced Premiums

                    #region Carried Forwards

                    #region New Carried Forwards

                    if (1 == 1)
                    {
                        string strAgentName = string.Format("{0}...Carried Forwards", new string(agentName.Take(20).ToArray()));
                        if (strAgentName.Length > 31)
                        {
                            strAgentName = (new string(strAgentName.Take(31).ToArray()));
                            strAgentName = strAgentName.Remove(strAgentName.Length - 3, 3) + "...";
                        }
                        //wsReport = wbReport.Worksheets.Add(strAgentName);

                        try
                        {
                            wsReport = wbReport.Worksheets.Add(strAgentName);
                        }
                        catch
                        {
                            wsReport = wbReport.Worksheets.Add(strAgentName + "2");
                        }

                        wsReport.DisplayOptions.View = WorksheetView.Normal;

                        wsReport.PrintOptions.PaperSize = PaperSize.A4;
                        wsReport.PrintOptions.Orientation = Orientation.Portrait;
                        wsReport.PrintOptions.LeftMargin = 0.3;
                        wsReport.PrintOptions.RightMargin = 0.3;

                        wsTemplate = wbTemplate.Worksheets["CarriedForwards"];

                        wsReport.Workbook.NamedReferences.Clear();
                        Methods.CopyExcelRegion(wsTemplate, 0, 0, 3, 4, wsReport, 0, 1);

                        #region Carried Forward Header Data

                        {
                            wsReport.GetCell("AgentName").Value = agentName;
                            wsReport.GetCell("SalesDates").Value = "Carried Forwards: " + dateRange;
                        }

                        #endregion Carried Forward Header Data

                        rowIndex = 4;
                        wsReport.Workbook.NamedReferences.Clear();

                        foreach (DataRow drCampaign in dtCampaignsCF.Rows)
                        {
                            string filterString = drCampaign["FilterString"].ToString();
                            string orderByString = drCampaign["OrderByString"].ToString();
                            string campaignTotalsFilteringString = drCampaign["CampaignTotalsFilteringString"].ToString();

                            // Determine if there are any rows for the given partition
                            var filteredSheetRows = dtSalesCF.Select(filterString).AsEnumerable();
                            if (filteredSheetRows.Any())
                            {
                                DataTable dtCampaignSalesCF = dtSalesCF.Select(filterString, orderByString).CopyToDataTable();

                                Methods.CopyExcelRegion(wsTemplate, 4, 1, 1, 7, wsReport, rowIndex, 1);

                                rowIndex++;

                                wsReport.GetCell("CampaignCF2").Value = drCampaign["CampaignCode"] as string;

                                //DataTable dtCampaignSales = dtSales.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'").CopyToDataTable();

                                //string filterString = drCampaign["FilterString"].ToString();
                                //string orderByString = drCampaign["OrderByString"].ToString();
                                //string campaignTotalsFilteringString = drCampaign["CampaignTotalsFilteringString"].ToString();
                                //DataTable dtCampaignSales = dtSales.Select(filterString, orderByString).CopyToDataTable();

                                foreach (DataRow drSale in dtCampaignSalesCF.Rows)
                                {
                                    rowIndex++;
                                    Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, 7, wsReport, rowIndex, 0);

                                    bool isGoldenLead = false;
                                    CellFillPattern cellFill = null;

                                    if ((drSale["RefNo"] as string).Contains("Golden"))
                                    {
                                        isGoldenLead = true;
                                        cellFill = new CellFillPattern(new WorkbookColorInfo(Color.Gold), null, FillPatternStyle.Solid);
                                    }

                                    wsReport.GetCell("DateOfSaleCF2").Value = ((DateTime)drSale["DateOfSale"]).ToString("d");
                                    if (isGoldenLead) { wsReport.GetCell("DateOfSaleCF2").CellFormat.Fill = cellFill; }

                                    //wsReport.GetCell("BatchCF2").Value = drSale["Batch"] as string;
                                    //if (isGoldenLead) { wsReport.GetCell("BatchCF2").CellFormat.Fill = cellFill; }

                                    //wsReport.GetCell("RefNoCF2").Value = drSale["RefNo"] as string;
                                    //if (isGoldenLead) { wsReport.GetCell("RefNoCF2").CellFormat.Fill = cellFill; }

                                    //wsReport.GetCell("ClientCF2").Value = drSale["LeadName"] as string;
                                    //if (isGoldenLead) { wsReport.GetCell("ClientCF2").CellFormat.Fill = cellFill; }

                                    wsReport.GetCell("PremiumCF2").Value = drSale["Premium"] as decimal?;
                                    if (isGoldenLead)
                                    {
                                        wsReport.GetCell("PremiumCF2").CellFormat.Fill = cellFill;
                                        wsReport.GetCell("PremiumCF2").CellFormat.Font.ColorInfo = new WorkbookColorInfo(Color.Red);
                                    }

                                    wsReport.GetCell("UnitsCF2").Value = drSale["Units"] as decimal?;
                                    if (isGoldenLead) { wsReport.GetCell("UnitsCF2").CellFormat.Fill = cellFill; }

                                    wsReport.GetCell("CarriedForwardReasonCF2").Value = drSale["SavedStatus"] as string;
                                    if (isGoldenLead) { wsReport.GetCell("CarriedForwardReasonCF2").CellFormat.Fill = cellFill; }

                                    //wsReport.GetCell("SavedStatusOriginalDOS").Value = drSale["SavedStatusOriginalDOS"] as string;
                                    //if (isGoldenLead) { wsReport.GetCell("SavedStatusOriginalDOS").CellFormat.Fill = cellFill; }

                                    wsReport.Workbook.NamedReferences.Clear();
                                }

                                {
                                    rowIndex++;
                                    Methods.CopyExcelRegion(wsTemplate, 7, 0, 0, 6, wsReport, rowIndex, 0);

                                    //DataRow dr = dtCampaignTotals.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'")[0];
                                    DataRow dr = dtCampaignTotalsCF.Select(campaignTotalsFilteringString).CopyToDataTable().Rows[0];
                                    wsReport.GetCell("TotalSalesCF2").Value = dr["TotalSales"] as int?;
                                    wsReport.GetCell("TotalPremiumCF2").Value = dr["TotalPremium"] as decimal?;
                                    wsReport.GetCell("TotalUnitsCF2").Value = dr["TotalUnits"] as decimal?;

                                    wsReport.Workbook.NamedReferences.Clear();
                                }

                                wsReport.Workbook.NamedReferences.Clear();
                                rowIndex += 2;
                            }
                        }

                        {
                            Methods.CopyExcelRegion(wsTemplate, 9, 0, 0, 6, wsReport, rowIndex, 0);

                            DataRow dr = dtGrandTotalsCF.Rows[0];
                            wsReport.GetCell("GrandTotalSalesCF2").Value = dr["GrandTotalSales"] as int?;
                            wsReport.GetCell("GrandTotalPremiumCF2").Value = dr["GrandTotalPremium"] as decimal?;
                            wsReport.GetCell("GrandTotalUnitsCF2").Value = dr["GrandTotalUnits"] as decimal?;

                            wsReport.Workbook.NamedReferences.Clear();
                        }
                    }

                    #endregion  New Carried Forwards

                    #region Old Carried Forwards

                    if (0 == 1)
                    {
                        DataTable dtIncludeCarriedForwardBreakdown = ds.Tables[8];
                        DataTable dtCarriedForwardCampaigns = ds.Tables[9];
                        DataTable dtCarriedForwards = ds.Tables[10];
                        DataTable dtCarriedForwardCampaignTotals = ds.Tables[11];
                        DataTable dtCarriedForwardGrandTotals = ds.Tables[12];
                        //wsReport = wbReport.Worksheets.Add(string.Join("", ("Carried Forwards - " + agentName).Take(31)));
                        string newCarriedForwardsWorksheetDescription = Methods.ParseWorksheetName(wbReport, String.Format("{0} Carried Forwards", agentName));

                        try
                        {
                            wsReport = wbReport.Worksheets.Add(newCarriedForwardsWorksheetDescription);
                        }
                        catch
                        {
                            wsReport = wbReport.Worksheets.Add(newCarriedForwardsWorksheetDescription + "2");
                        }

                        rowIndex = 4;
                        wsReport.DisplayOptions.View = WorksheetView.Normal;

                        wsReport.PrintOptions.PaperSize = PaperSize.A4;
                        wsReport.PrintOptions.Orientation = Orientation.Portrait;
                        wsReport.PrintOptions.LeftMargin = 0.3;
                        wsReport.PrintOptions.RightMargin = 0.3;

                        wsReport.Workbook.NamedReferences.Clear();
                        Methods.CopyExcelRegion(wsTemplate, 0, 0, 3, 5, wsReport, 0, 0);

                        #region Carried Forward Header Data

                        {
                            wsReport.GetCell("AgentName").Value = agentName;
                            wsReport.GetCell("SalesDates").Value = "Carried Forwards: " + dateRange;

                            //wsReport.GetCell("Batches").Value = batchCodes + ((char)65279);
                        }
                        #endregion Carried Forward Header Data

                        #region Determining if a detailed breakdown of the carried forwards should be included

                        bool includeCarriedForwardBreakdown = false;

                        if (dtIncludeCarriedForwardBreakdown.Rows.Count > 0)
                        {
                            includeCarriedForwardBreakdown = Convert.ToBoolean(dtIncludeCarriedForwardBreakdown.Rows[0]["IncludeCarriedForwardBreakdown"]);
                        }

                        #endregion Determining if a detailed breakdown of the carried forwards should be included

                        if (includeCarriedForwardBreakdown)
                        {
                            foreach (DataRow drCampaign in dtCarriedForwardCampaigns.Rows)
                            {
                                Methods.CopyExcelRegion(wsTemplate, 4, 0, 1, 5, wsReport, rowIndex, 0);
                                rowIndex++;

                                wsReport.GetCell("Campaign").Value = drCampaign["CampaignCode"] as string;

                                DataTable dtCampaignCancellations = dtCarriedForwards.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'").CopyToDataTable();
                                foreach (DataRow drSale in dtCarriedForwards.Rows)
                                {
                                    rowIndex++;
                                    Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, 5, wsReport, rowIndex, 0);

                                    bool isGoldenLead = false;
                                    CellFillPattern cellFill = null;

                                    if ((drSale["RefNo"] as string).Contains("Golden"))
                                    {
                                        isGoldenLead = true;
                                        cellFill = new CellFillPattern(new WorkbookColorInfo(Color.Gold), null, FillPatternStyle.Solid);
                                    }

                                    wsReport.GetCell("DateOfSale").Value = ((DateTime)drSale["DateOfSale"]).ToString("d");
                                    if (isGoldenLead) { wsReport.GetCell("DateOfSale").CellFormat.Fill = cellFill; }
                                    wsReport.GetCell("Batch").Value = drSale["Batch"] as string;
                                    if (isGoldenLead) { wsReport.GetCell("Batch").CellFormat.Fill = cellFill; }
                                    wsReport.GetCell("RefNo").Value = drSale["RefNo"] as string;
                                    if (isGoldenLead) { wsReport.GetCell("RefNo").CellFormat.Fill = cellFill; }
                                    wsReport.GetCell("Client").Value = drSale["LeadName"] as string;
                                    if (isGoldenLead) { wsReport.GetCell("Client").CellFormat.Fill = cellFill; }
                                    wsReport.GetCell("Premium").Value = drSale["Premium"] as decimal?;
                                    if (isGoldenLead) { wsReport.GetCell("Premium").CellFormat.Fill = cellFill; }
                                    wsReport.GetCell("Units").Value = drSale["Units"] as decimal?;
                                    if (isGoldenLead) { wsReport.GetCell("Units").CellFormat.Fill = cellFill; }

                                    wsReport.Workbook.NamedReferences.Clear();
                                }

                                {
                                    rowIndex++;
                                    Methods.CopyExcelRegion(wsTemplate, 7, 0, 0, 5, wsReport, rowIndex, 0);

                                    DataRow dr = dtCarriedForwardCampaignTotals.Select("CampaignID = '" + (drCampaign["CampaignID"] as long?) + "'")[0];
                                    wsReport.GetCell("TotalSales").Value = dr["TotalCarriedForwards"] as int?;
                                    wsReport.GetCell("TotalPremium").Value = dr["TotalPremium"] as decimal?;
                                    wsReport.GetCell("TotalUnits").Value = dr["TotalUnits"] as decimal?;

                                    wsReport.Workbook.NamedReferences.Clear();
                                }

                                wsReport.Workbook.NamedReferences.Clear();
                                rowIndex += 2;
                            }

                            {
                                Methods.CopyExcelRegion(wsTemplate, 9, 0, 0, 5, wsReport, rowIndex, 0);

                                DataRow dr = dtCarriedForwardGrandTotals.Rows[0];
                                wsReport.GetCell("GrandTotalSales").Value = dr["GrandTotalCarriedForwards"] as int?;
                                wsReport.GetCell("GrandTotalPremium").Value = dr["GrandTotalPremium"] as decimal?;
                                wsReport.GetCell("GrandTotalUnits").Value = dr["GrandTotalUnits"] as decimal?;

                                wsReport.Workbook.NamedReferences.Clear();
                            }
                        }
                        else
                        {

                            DataRow drCarriedForwardGrandTotal = dtCarriedForwardGrandTotals.Rows[0];

                            Methods.CopyExcelRegion(wsTemplate, 12, 0, 2, 4, wsReport, rowIndex, 0);

                            wsReport.GetCell("GrandTotalCarriedForwardsBase").Value = drCarriedForwardGrandTotal["GrandTotalCarriedForwardsBase"] as int?;
                            wsReport.GetCell("GrandTotalCarriedForwardPremium").Value = Methods.ForceCurrencyFormatting(drCarriedForwardGrandTotal["GrandTotalCarriedForwardPremium"], false);
                            wsReport.GetCell("GrandTotalCarriedForwardUpgrades").Value = drCarriedForwardGrandTotal["GrandTotalCarriedForwardUpgrades"] as int?;
                            wsReport.GetCell("GrandTotalCarriedForwardUnits").Value = drCarriedForwardGrandTotal["GrandTotalCarriedForwardUnits"] as decimal?;

                            wsReport.Workbook.NamedReferences.Clear();
                        }
                    }

                    #endregion  Old Carried Forwards

                    #endregion Carried Forwards



                    //wsReport.Rows[rowIndex].Cells[5].CellFormat.SetFormatting(wsTemplate.Rows[5].Cells[5].CellFormat) ;//= "# ##0.00;[Red]# ##0.00";
                }

                #endregion
            }
            catch (Exception ex) 
            {
                HandleException(ex); 
            }
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region setup excel document

                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                string dateRange = (_fromDate.ToString("d") + " to " + _toDate.ToString("d")).Replace("/", "-");

                //string filePathAndName = GlobalSettings.UserFolder + "Sales Report (" + dateRange + ") ~ " + DateTime.Now.Millisecond + ".xlsx";


                string filePathAndName = "Sales Report (" + dateRange + ") ~ " + DateTime.Now.Millisecond + ".xlsx";

                Uri uri = new Uri("/Templates/ReportTemplateSales.xlsx", UriKind.Relative);

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

                if (ReportMode == lkpINCampTSRReportMode.ByTSR)
                {
                    if (_selectedAgents.Count > 0)
                    {
                        foreach (DataRecord drAgent in _selectedAgents)
                        {
                            string agentName = drAgent.Cells["Description"].Value as string;
                            _agentName = agentName;
                            long? agentID = drAgent.Cells["ID"].Value as long?;

                            ReportBody(wbTemplate, wbReport, agentName, agentID, dateRange);
                            AddOvertimeSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                            AddRedeemedGiftsSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                            AddFowardToDCAgentSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                            AddreferralsSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                        }
                    }
                    else
                    {
                        string agentName = _user.FirstName.Trim() + " " + _user.LastName.Trim();
                        long? agentID = _user.ID;

                        ReportBody(wbTemplate, wbReport, agentName, agentID, dateRange);
                        AddOvertimeSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                        AddRedeemedGiftsSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                        AddFowardToDCAgentSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                        AddreferralsSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);

                    }
                }
                else if (ReportMode == lkpINCampTSRReportMode.ByQA)
                {
                    if (_selectedAgents.Count > 0)
                    {
                        foreach (DataRecord drQA in _selectedAgents)
                        {
                            SqlParameter[] parameters = new SqlParameter[4];
                            parameters[0] = new SqlParameter("@SupervisorUserID", drQA.Cells["ID"].Value as long?);
                            parameters[1] = new SqlParameter("@AgentMode", (int)AgentMode);
                            parameters[2] = new SqlParameter("@FromDate", _fromDate);
                            parameters[3] = new SqlParameter("@ToDate", _toDate);

                            DataTable dt = Methods.ExecuteStoredProcedure("spGetAgentsForSupervisor", parameters).Tables[0];

                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow drAgent in dt.Rows)
                                {
                                    long? agentID = drAgent.ItemArray[0] as long?;
                                    string agentName = drAgent.ItemArray[1] as string;
                                    _agentName = agentName;

                                    ReportBody(wbTemplate, wbReport, agentName, agentID, dateRange);
                                    AddOvertimeSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                                    AddRedeemedGiftsSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                                    AddFowardToDCAgentSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                                    AddreferralsSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);

                                }
                            }
                            else
                            {
                                Dispatcher.Invoke(DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate
                                {
                                    ShowMessageBox(new Windows.INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", Embriant.Framework.ShowMessageType.Information);
                                });

                                return;
                            }
                        }
                    }
                }
                else if (ReportMode == lkpINCampTSRReportMode.TrainingSupervisor)
                {
                    if (_selectedAgents.Count > 0)
                    {
                        foreach (DataRecord drQA in _selectedAgents)
                        {
                            SqlParameter[] parameters = new SqlParameter[4];
                            parameters[0] = new SqlParameter("@SupervisorUserID", drQA.Cells["ID"].Value as long?);
                            parameters[1] = new SqlParameter("@AgentMode", (int)AgentMode);
                            parameters[2] = new SqlParameter("@FromDate", _fromDate);
                            parameters[3] = new SqlParameter("@ToDate", _toDate);

                            DataTable dt = Methods.ExecuteStoredProcedure("spGetAgentsForTrainingSupervisor", parameters).Tables[0];

                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow drAgent in dt.Rows)
                                {
                                    long? agentID = drAgent.ItemArray[0] as long?;
                                    string agentName = drAgent.ItemArray[1] as string;
                                    _agentName = agentName;

                                    ReportBody(wbTemplate, wbReport, agentName, agentID, dateRange);
                                    AddOvertimeSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                                    AddRedeemedGiftsSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);

                                }
                            }
                            else
                            {
                                Dispatcher.Invoke(DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate
                                {
                                    ShowMessageBox(new Windows.INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", Embriant.Framework.ShowMessageType.Information);
                                });

                                return;
                            }
                        }
                    }
                }
                else
                {
                    return;
                }


                #region save and display excel document

                //Save excel document
                wbReport.SetCurrentFormat(WorkbookFormat.Excel2007);
                wbReport.Save(filePathAndName);

                //Display excel document
                Process.Start(filePathAndName);

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

        private void ReportDC(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region setup excel document

                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                string dateRange = (_fromDate.ToString("d") + " to " + _toDate.ToString("d")).Replace("/", "-");

                //string filePathAndName = GlobalSettings.UserFolder + "Sales Report (" + dateRange + ") ~ " + DateTime.Now.Millisecond + ".xlsx";


                string filePathAndName = "Sales Report (" + dateRange + ") ~ " + DateTime.Now.Millisecond + ".xlsx";

                Uri uri = new Uri("/Templates/ReportTemplateSales.xlsx", UriKind.Relative);

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

                if (ReportMode == lkpINCampTSRReportMode.ByTSR)
                {
                    if (_selectedAgents.Count > 0)
                    {
                        foreach (DataRecord drAgent in _selectedAgents)
                        {
                            string agentName = drAgent.Cells["Description"].Value as string;
                            _agentName = agentName;
                            long? agentID = drAgent.Cells["ID"].Value as long?;

                            ReportBodyDC(wbTemplate, wbReport, agentName, agentID, dateRange);
                            AddOvertimeSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                            AddRedeemedGiftsSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                            AddFowardToDCAgentSheetDC(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                        }
                    }
                    else
                    {
                        string agentName = _user.FirstName.Trim() + " " + _user.LastName.Trim();
                        long? agentID = _user.ID;

                        ReportBodyDC(wbTemplate, wbReport, agentName, agentID, dateRange);
                        AddOvertimeSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                        AddRedeemedGiftsSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                        AddFowardToDCAgentSheetDC(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);

                    }
                }
                else if (ReportMode == lkpINCampTSRReportMode.ByQA)
                {
                    if (_selectedAgents.Count > 0)
                    {
                        foreach (DataRecord drQA in _selectedAgents)
                        {
                            SqlParameter[] parameters = new SqlParameter[4];
                            parameters[0] = new SqlParameter("@SupervisorUserID", drQA.Cells["ID"].Value as long?);
                            parameters[1] = new SqlParameter("@AgentMode", (int)AgentMode);
                            parameters[2] = new SqlParameter("@FromDate", _fromDate);
                            parameters[3] = new SqlParameter("@ToDate", _toDate);

                            DataTable dt = Methods.ExecuteStoredProcedure("spGetAgentsForSupervisor", parameters).Tables[0];

                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow drAgent in dt.Rows)
                                {
                                    long? agentID = drAgent.ItemArray[0] as long?;
                                    string agentName = drAgent.ItemArray[1] as string;
                                    _agentName = agentName;

                                    ReportBodyDC(wbTemplate, wbReport, agentName, agentID, dateRange);
                                    AddOvertimeSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                                    AddRedeemedGiftsSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                                    AddFowardToDCAgentSheetDC(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);

                                }
                            }
                            else
                            {
                                Dispatcher.Invoke(DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate
                                {
                                    ShowMessageBox(new Windows.INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", Embriant.Framework.ShowMessageType.Information);
                                });

                                return;
                            }
                        }
                    }
                }
                else if (ReportMode == lkpINCampTSRReportMode.TrainingSupervisor)
                {
                    if (_selectedAgents.Count > 0)
                    {
                        foreach (DataRecord drQA in _selectedAgents)
                        {
                            SqlParameter[] parameters = new SqlParameter[4];
                            parameters[0] = new SqlParameter("@SupervisorUserID", drQA.Cells["ID"].Value as long?);
                            parameters[1] = new SqlParameter("@AgentMode", (int)AgentMode);
                            parameters[2] = new SqlParameter("@FromDate", _fromDate);
                            parameters[3] = new SqlParameter("@ToDate", _toDate);

                            DataTable dt = Methods.ExecuteStoredProcedure("spGetAgentsForTrainingSupervisor", parameters).Tables[0];

                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow drAgent in dt.Rows)
                                {
                                    long? agentID = drAgent.ItemArray[0] as long?;
                                    string agentName = drAgent.ItemArray[1] as string;
                                    _agentName = agentName;

                                    ReportBodyDC(wbTemplate, wbReport, agentName, agentID, dateRange);
                                    AddOvertimeSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                                    AddRedeemedGiftsSheet(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);
                                    AddFowardToDCAgentSheetDC(wbTemplate, wbReport, agentName, agentID, _fromDate, _toDate);

                                }
                            }
                            else
                            {
                                Dispatcher.Invoke(DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate
                                {
                                    ShowMessageBox(new Windows.INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", Embriant.Framework.ShowMessageType.Information);
                                });

                                return;
                            }
                        }
                    }
                }
                else
                {
                    return;
                }


                #region save and display excel document

                //Save excel document
                wbReport.SetCurrentFormat(WorkbookFormat.Excel2007);
                wbReport.Save(filePathAndName);

                //Display excel document
                Process.Start(filePathAndName);

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

        private void ReportTimer(object sender, EventArgs e)
        {
            _seconds++;
            btnReport.Content = TimeSpan.FromSeconds(_seconds).ToString();
            btnReport.ToolTip = btnReport.Content;
            lblCurrentAgent.Text = _agentName;
        }

        #endregion



        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsReportRunning = true;
                _selectedAgents = (xdgAgents.Records.Select(r => (DataRecord)r).Where(r => (bool)r.Cells["IsChecked"].Value)).ToList();

                BackgroundWorker worker = new BackgroundWorker();
                if (DebiCheckCB.IsChecked == true)
                {
                    worker.DoWork += ReportDC;

                }
                else
                {
                    worker.DoWork += Report;
                }
                worker.RunWorkerCompleted += ReportCompleted;
                worker.RunWorkerAsync();

                _reportTimer.Start();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void Cal1_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(Cal1.SelectedDate.ToString(), out _fromDate);
        }

        private void Cal2_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(Cal2.SelectedDate.ToString(), out _toDate);
        }

        private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            AllRecordsChecked = IsAllRecordsChecked();
        }

        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (xdgAgents.DataSource != null)
                {
                    DataTable dt = ((DataView)xdgAgents.DataSource).Table;

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["IsChecked"] = true;
                    }
                }

                IsAllRecordsChecked();
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        private void HeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (xdgAgents.DataSource != null)
                {
                    DataTable dt = ((DataView)xdgAgents.DataSource).Table;

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["IsChecked"] = false;
                    }
                }

                IsAllRecordsChecked();
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

            if (Convert.ToBoolean(grdAgents?.IsEnabled))
            {


                if (ReportMode == lkpINCampTSRReportMode.ByTSR)
                {

                    if (AgentMode == lkpAgentMode.Temporary)
                    {

                        lblFoundation.Visibility = Visibility.Visible;
                        lblPrePerm.Visibility = Visibility.Visible;

                        chkFoundation.Visibility = Visibility.Visible;
                        chkPrePerm.Visibility = Visibility.Visible;

                        if (chkFoundation.IsChecked == true)
                        {
                           
                            SqlParameter[] parameters = new SqlParameter[1];
                            if (AgentMode != null) parameters[0] = new SqlParameter("@AgentMode", (int)AgentMode);
                            DataTable dt = Methods.ExecuteStoredProcedure("spGetSalesAgents4Foundation", parameters).Tables[0];

                            DataColumn column = new DataColumn("IsChecked", typeof(bool));
                            column.DefaultValue = false;
                            dt.Columns.Add(column);

                            xdgAgents.DataSource = dt.DefaultView;

                            AllRecordsChecked = false;
                        }
                        else if (chkPrePerm.IsChecked == true)
                        {
                            DataTable dt = Methods.ExecuteStoredProcedure("spGetSalesTrainingSupervisorsPrePerm", null).Tables[0];

                            DataColumn column = new DataColumn("IsChecked", typeof(bool));
                            column.DefaultValue = false;
                            dt.Columns.Add(column);

                            DataColumn column2 = new DataColumn("FKStaffTypeID", typeof(long));
                            column2.DefaultValue = 1;
                            dt.Columns.Add(column2);

                            xdgAgents.DataSource = dt.DefaultView;

                            AllRecordsChecked = false;
                        }
                        else 
                        {
                           
                            SqlParameter[] parameters = new SqlParameter[1];
                            if (AgentMode != null) parameters[0] = new SqlParameter("@AgentMode", (int)AgentMode);
                            DataTable dt = Methods.ExecuteStoredProcedure("spGetSalesAgents4", parameters).Tables[0];

                            DataColumn column = new DataColumn("IsChecked", typeof(bool));
                            column.DefaultValue = false;
                            dt.Columns.Add(column);

                            xdgAgents.DataSource = dt.DefaultView;

                            AllRecordsChecked = false;
                        }

                    }
                    else
                    {

                        SqlParameter[] parameters = new SqlParameter[1];
                        if (AgentMode != null) parameters[0] = new SqlParameter("@AgentMode", (int)AgentMode);
                        DataTable dt = Methods.ExecuteStoredProcedure("spGetSalesAgents4", parameters).Tables[0];

                        DataColumn column = new DataColumn("IsChecked", typeof(bool));
                        column.DefaultValue = false;
                        dt.Columns.Add(column);

                        xdgAgents.DataSource = dt.DefaultView;

                        AllRecordsChecked = false;
                    }




                }
                else if (ReportMode == lkpINCampTSRReportMode.ByQA)
                {

                    lblFoundation.Visibility = Visibility.Collapsed;
                    lblPrePerm.Visibility = Visibility.Collapsed;

                    chkFoundation.Visibility = Visibility.Collapsed;
                    chkPrePerm.Visibility = Visibility.Collapsed;


                    DataTable dt = Methods.ExecuteStoredProcedure("spGetSalesSupervisors", null).Tables[0];

                    DataColumn column = new DataColumn("IsChecked", typeof(bool));
                    column.DefaultValue = false;
                    dt.Columns.Add(column);

                    DataColumn column2 = new DataColumn("FKStaffTypeID", typeof(long));
                    column2.DefaultValue = 1;
                    dt.Columns.Add(column2);

                    xdgAgents.DataSource = dt.DefaultView;

                    AllRecordsChecked = false;
                }
                else if (ReportMode == lkpINCampTSRReportMode.TrainingSupervisor)
                {

                    lblFoundation.Visibility = Visibility.Visible;
                    lblPrePerm.Visibility = Visibility.Visible;

                    chkFoundation.Visibility = Visibility.Visible;
                    chkPrePerm.Visibility = Visibility.Visible;

                    if (chkPrePerm.IsChecked == true)
                    {
                        DataTable dt = Methods.ExecuteStoredProcedure("spGetSalesTrainingSupervisorsPrePerm", null).Tables[0];

                        DataColumn column = new DataColumn("IsChecked", typeof(bool));
                        column.DefaultValue = false;
                        dt.Columns.Add(column);

                        DataColumn column2 = new DataColumn("FKStaffTypeID", typeof(long));
                        column2.DefaultValue = 1;
                        dt.Columns.Add(column2);

                        xdgAgents.DataSource = dt.DefaultView;

                        AllRecordsChecked = false;
                    }
                    else 
                    {
                        DataTable dt = Methods.ExecuteStoredProcedure("spGetSalesTrainingSupervisors", null).Tables[0];

                        DataColumn column = new DataColumn("IsChecked", typeof(bool));
                        column.DefaultValue = false;
                        dt.Columns.Add(column);

                        DataColumn column2 = new DataColumn("FKStaffTypeID", typeof(long));
                        column2.DefaultValue = 1;
                        dt.Columns.Add(column2);

                        xdgAgents.DataSource = dt.DefaultView;

                        AllRecordsChecked = false;
                    }

                   
                }
            }
        }

        #endregion



        public class ReportSalesScreenData : ObservableObject
        {

            #region Members

            public class User : ObservableObject
            {
                private long? _userID;
                public long? UserID
                {
                    get { return _userID; }
                    set { SetProperty(ref _userID, value, () => UserID); }
                }

                private long? _userTypeID;
                public long? UserTypeID
                {
                    get { return _userTypeID; }
                    set { SetProperty(ref _userTypeID, value, () => UserTypeID); }
                }

                private lkpUserType? _userType;
                public lkpUserType? UserType
                {
                    get { return _userType; }
                    set { SetProperty(ref _userType, value, () => UserType); }
                }

                private string _name;
                public string Name
                {
                    get { return _name; }
                    set { SetProperty(ref _name, value, () => Name); }
                }

                private string _surname;
                public string Surname
                {
                    get { return _surname; }
                    set { SetProperty(ref _surname, value, () => Surname); }
                }

                private string _fullName;
                public string FullName
                {
                    get { return _fullName; }
                    set { SetProperty(ref _fullName, value, () => FullName); }
                }
            }

            #endregion



            #region Constructor

            public ReportSalesScreenData()
            {
                UserData.UserID = ((Business.User)GlobalSettings.ApplicationUser).ID;
                UserData.UserTypeID = ((Business.User)GlobalSettings.ApplicationUser).FKUserType;
                UserData.UserType = (lkpUserType?)((Business.User)GlobalSettings.ApplicationUser).FKUserType;
                UserData.Name = ((Business.User)GlobalSettings.ApplicationUser).FirstName.Trim();
                UserData.Surname = ((Business.User)GlobalSettings.ApplicationUser).LastName.Trim();
                UserData.FullName = UserData.Name + " " + UserData.Surname;
            }

            #endregion



            #region Public Methods

            public void Clear()
            {

            }

            #endregion



            #region User

            private User _userData = new User();
            public User UserData
            {
                get { return _userData; }
                set { SetProperty(ref _userData, value, () => UserData); }
            }

            #endregion

        }

        private void chkPrePerm_Checked(object sender, RoutedEventArgs e)
        {
            LoadAgentInfo();
        }

        private void chkFoundation_Checked(object sender, RoutedEventArgs e)
        {
            LoadAgentInfo();
        }
    }

}
