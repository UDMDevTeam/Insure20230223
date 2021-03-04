using Embriant.Framework.Configuration;
using Embriant.WPF.Controls;
using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using UDM.Insurance.Business;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportSupervisorTeam.xaml
    /// </summary>
    public partial class ReportSupervisorTeam : INotifyPropertyChanged
    {
        private List<DataRecord> _selectedAgents;
        private DateTime _fromDate = DateTime.Now;
        private DateTime _toDate = DateTime.Now;
        private readonly User _user = ((User)GlobalSettings.ApplicationUser);

        private readonly DispatcherTimer _reportTimer = new DispatcherTimer();
        private int _seconds;
        private string _agentName;
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

        private bool? _IncludeResigned = false;
        public bool? IncludeResignedTSR
        {
            get
            {
                return _IncludeResigned;
            }
            set
            {
                if (_IncludeResigned != null)
                {
                    _IncludeResigned = value;
                }
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

        #endregion

        public ReportSupervisorTeam()
        {
            InitializeComponent();
            LoadAgentInfo();


        }
        private void LoadAgentInfo()
        {
            try
            {
                Cursor = Cursors.Wait;

                if (grdAgents.IsEnabled)
                {
                    DataSet ds = Methods.ExecuteStoredProcedure("GetSupervisorsStaff", null);

                    DataTable dt = ds.Tables[0];
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

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);
                #region setup excel document

                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                string dateRange = (_fromDate.ToString("d") + " to " + _toDate.ToString("d")).Replace("/", "-");

                string filePathAndName = GlobalSettings.UserFolder + "Supervisor Report (" + dateRange + ") ~ " + DateTime.Now.Millisecond + ".xlsx";

                Uri uri = new Uri("/Templates/ReportSupervisorTeam.xlsx", UriKind.Relative);
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

                if (_selectedAgents.Count > 0)
                {
                    int supCount = 0;
                    foreach (DataRecord drAgent in _selectedAgents)
                    {
                        string agentName = drAgent.Cells["Description"].Value as string;
                        _agentName = agentName;
                        long? agentID = drAgent.Cells["ID"].Value as long?;

                        if (supCount == 0)
                        {
                            ReportBody(wbTemplate, wbReport, agentName, agentID, dateRange, true);
                        }
                        else
                        {
                            ReportBody(wbTemplate, wbReport, agentName, agentID, dateRange, false);
                        }
                        supCount++;
                    }

                    if (supCount == _selectedAgents.Count)
                    {

                    }
                }
                else
                {
                    string agentName = _user.FirstName.Trim() + " " + _user.LastName.Trim();
                    long? agentID = _user.ID;

                    ReportBody(wbTemplate, wbReport, agentName, agentID, dateRange, false);
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
        private void ReportBody(Workbook wbTemplate, Workbook wbReport, string agentName, long? agentID, string dateRange, bool addSummaryBook)
        {
            #region retrieve data from database
            string AgentIDs = string.Empty;
            bool first = true;
            List<int> tsrList = new List<int>();
            if (_selectedAgents != null)
            {
                foreach (var agent in _selectedAgents)
                {
                    if ((bool)agent.Cells["IsChecked"].Value == true)
                    {
                        if (first == true)
                        {
                            first = false;
                            tsrList.Add(Convert.ToInt32(agent.Cells["ID"].Value.ToString()));
                            AgentIDs = Convert.ToInt32(agent.Cells["ID"].Value.ToString()) + ",";
                        }
                        else
                        {
                            tsrList.Add(Convert.ToInt32(agent.Cells["ID"].Value.ToString()));
                            AgentIDs = AgentIDs + Convert.ToInt32(agent.Cells["ID"].Value.ToString()) + ",";
                        }
                    }
                }
            }

            SqlParameter[] parameters =
                {
                    new SqlParameter("@UserId", AgentIDs),
                    //new SqlParameter("@DateFrom", _fromDate),
                    new SqlParameter("@ReportDate", _toDate)
                };

            DataSet ds = new DataSet();
            DataTable dtTeams = new DataTable();

            if (IncludeResignedTSR == false)
            {
               ds =  Methods.ExecuteStoredProcedure("sp_GetSupervisors", parameters);
               dtTeams = ds.Tables[0];
            }
            else
            {
                ds = Methods.ExecuteStoredProcedure("sp_GetSupervisorsIncludeAll", parameters);
                dtTeams = ds.Tables[0];
            }

            #endregion retrieve data from database

            #region setup worksheet

            Worksheet wsSummary = null;
            Worksheet wsTemplate = wbTemplate.Worksheets["Report"];

            if (addSummaryBook == true)
            {
                wsSummary = wbReport.Worksheets.Add("Team Summary");

                wsSummary.DisplayOptions.View = WorksheetView.Normal;
                wsSummary.PrintOptions.PaperSize = PaperSize.A4;
                wsSummary.PrintOptions.Orientation = Infragistics.Documents.Excel.Orientation.Portrait;
                wsSummary.PrintOptions.LeftMargin = 0.3;
                wsSummary.PrintOptions.RightMargin = 0.3;

                wsSummary.Workbook.NamedReferences.Clear();
                Methods.CopyExcelRegion(wsTemplate, 0, 0, 3, 4, wsSummary, 0, 0);

                #region header data
                {
                    int count = 0;
                    foreach (var agents in _selectedAgents)
                    {
                        count++;
                        switch (count)
                        {
                            case 1:
                                #region Supervisor Data 1st Record
                                int rIndex = 5;
                                wsSummary.Workbook.NamedReferences.Clear();
                                int y = 0;

                                string id = agents.Cells["ID"].Value.ToString()+",";

                                SqlParameter[] prSupervisor =
                                {
                                    new SqlParameter("@UserId", AgentIDs),
                                    //new SqlParameter("@DateFrom", _fromDate),
                                    new SqlParameter("@ReportDate", _toDate)
                                };

                                DataSet dssupervisor = Methods.ExecuteStoredProcedure("sp_GetSupervisors", prSupervisor);
                                DataTable dtSupervisor = dssupervisor.Tables[0];
                                DataRow[] drResult = dtTeams.Select("FKHRSupervisorID = '" + agents.Cells["ID"].Value.ToString() + "'");

                                wsSummary.GetCell("A4").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                wsSummary.GetCell("A4").Value = agents.Cells["Description"].Value.ToString();
                                wsSummary.Columns[0].Width = 10000;
                                foreach (DataRow dr in drResult)
                                {
                                    try
                                    {
                                        //Methods.CopyExcelRegion(wsTemplate, 4, 0, 30, 30, wsSummary, 30, 0);
                                        rIndex++;
                                        if (dr[0] != null && dr[1] != null)
                                        {
                                            string TsrName = (dr[0].ToString() + "  " + dr[1].ToString());
                                            wsSummary.GetCell("A" + rIndex).Value = TsrName;
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    y++;
                                }

                                #endregion
                                break;
                            case 2:
                                #region Supervisor Data 2nd Record
                                rIndex = 5;
                                wsSummary.Workbook.NamedReferences.Clear();
                                y = 0;
                                drResult = null;
                                drResult = dtTeams.Select("FKHRSupervisorID = '" + agents.Cells["ID"].Value.ToString() + "'");
                                wsSummary.GetCell("B4").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                wsSummary.Columns[1].Width = 10000;
                                wsSummary.GetCell("B4").Value = agents.Cells["Description"].Value.ToString();

                                foreach (DataRow dr in drResult)
                                {
                                    try
                                    {
                                        //Methods.CopyExcelRegion(wsTemplate, 4, 0, 1, 5, wsSummary, rIndex, 0);
                                        rIndex++;
                                        if (dr[0] != null && dr[1] != null)
                                        {
                                            string TsrName = (dr[0].ToString() + "  " + dr[1].ToString());
                                            wsSummary.GetCell("B" + rIndex).Value = TsrName;
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    y++;
                                }
                                #endregion
                                break;
                            case 3:
                                #region Supervisor Data 3rd Record
                                rIndex = 5;
                                wsSummary.Workbook.NamedReferences.Clear();
                                y = 0;
                                drResult = null;
                                drResult = dtTeams.Select("FKHRSupervisorID = '" + agents.Cells["ID"].Value.ToString() + "'");
                                wsSummary.GetCell("C4").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                wsSummary.Columns[2].Width = 10000;
                                wsSummary.GetCell("C4").Value = agents.Cells["Description"].Value.ToString();

                                foreach (DataRow dr in drResult)
                                {
                                    try
                                    {
                                        rIndex++;
                                        if (dr[0] != null && dr[1] != null)
                                        {
                                            string TsrName = (dr[0].ToString() + "  " + dr[1].ToString());
                                            wsSummary.GetCell("C" + rIndex).Value = TsrName;
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    y++;
                                }

                                #endregion
                                break;
                            case 4:
                                #region Supervisor Data 4th Record
                                rIndex = 5;
                                wsSummary.Workbook.NamedReferences.Clear();
                                y = 0;
                                drResult = null;
                                drResult = dtTeams.Select("FKHRSupervisorID = '" + agents.Cells["ID"].Value.ToString() + "'");

                                wsSummary.GetCell("D4").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                wsSummary.Columns[3].Width = 10000;
                                wsSummary.GetCell("D4").Value = agents.Cells["Description"].Value.ToString();
                                wsSummary.Columns[3].Width = 10000;
                                foreach (DataRow dr in drResult)
                                {
                                    try
                                    {
                                        rIndex++;
                                        if (dr[0] != null && dr[1] != null)
                                        {
                                            string TsrName = (dr[0].ToString() + "  " + dr[1].ToString());
                                            wsSummary.GetCell("D" + rIndex).Value = TsrName;
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    y++;
                                }
                                #endregion
                                break;
                            case 5:
                                #region Supervisor Data 5th Record
                                rIndex = 5;
                                wsSummary.Workbook.NamedReferences.Clear();
                                y = 0;
                                drResult = null;
                                drResult = dtTeams.Select("FKHRSupervisorID = '" + agents.Cells["ID"].Value.ToString() + "'");
                                wsSummary.GetCell("E4").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                wsSummary.Columns[4].Width = 10000;
                                wsSummary.GetCell("E4").Value = agents.Cells["Description"].Value.ToString();

                                foreach (DataRow dr in drResult)
                                {
                                    try
                                    {
                                        rIndex++;
                                        if (dr[0] != null && dr[1] != null)
                                        {
                                            string TsrName = (dr[0].ToString() + "  " + dr[1].ToString());
                                            wsSummary.GetCell("E" + rIndex).Value = TsrName;
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    y++;
                                }

                                #endregion
                                break;
                            case 6:
                                #region Supervisor Data 6th Record
                                rIndex = 5;
                                wsSummary.Workbook.NamedReferences.Clear();
                                y = 0;
                                drResult = null;
                                drResult = dtTeams.Select("FKHRSupervisorID = '" + agents.Cells["ID"].Value.ToString() + "'");
                                wsSummary.GetCell("F4").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                wsSummary.Columns[5].Width = 10000;
                                wsSummary.GetCell("F4").Value = agents.Cells["Description"].Value.ToString();

                                foreach (DataRow dr in drResult)
                                {
                                    try
                                    {
                                        rIndex++;
                                        if (dr[0] != null && dr[1] != null)
                                        {
                                            string TsrName = (dr[0].ToString() + "  " + dr[1].ToString());
                                            wsSummary.GetCell("F" + rIndex).Value = TsrName;
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    y++;
                                }

                                #endregion
                                break;
                            case 7:
                                break;
                        }

                    }
                }
                addSummaryBook = false;
                #endregion
            }
            Worksheet wsReport = wbReport.Worksheets.Add(string.Join("", agentName.Take(31)));
            wsReport.DisplayOptions.View = WorksheetView.Normal;
            wsReport.PrintOptions.PaperSize = PaperSize.A4;
            wsReport.PrintOptions.Orientation = Infragistics.Documents.Excel.Orientation.Portrait;
            wsReport.PrintOptions.LeftMargin = 0.3;
            wsReport.PrintOptions.RightMargin = 0.3;

            wsReport.Workbook.NamedReferences.Clear();
            Methods.CopyExcelRegion(wsTemplate, 0, 0, 3, 4, wsReport, 0, 0);

            #endregion

            #region header data
            {
                wsReport.GetCell("A3").Value = " Start Date " + _fromDate;
                wsReport.GetCell("A4").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                wsReport.GetCell("A4").Value = " End Date " + _toDate;
                wsReport.GetCell("A6").Value = "Supervisor: " + agentName;
                wsReport.GetCell("A6").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                wsReport.GetCell("A6").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                wsReport.GetCell("A6").Value = "Agent Name";

                // wsReport.GetCell("A6").Worksheet.Workbook.Styles[0]..StyleFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.Yellow);
            }
            #endregion
            #region Team Data

            int rowIndex = 6;
            wsReport.Workbook.NamedReferences.Clear();
            int x = 0;
            DataRow[] result = dtTeams.Select("FKHRSupervisorID = '" + agentID.ToString() + "'");

            foreach (DataRow dr in result)
            {
                try
                {
                    Methods.CopyExcelRegion(wsTemplate, 4, 0, 1, 5, wsReport, rowIndex, 0);
                    rowIndex++;
                    if (dr[0] != null && dr[1] != null)
                    {
                        string TsrName = (dr[0].ToString() + "  " + dr[1].ToString());

                        wsReport.GetCell("A" + rowIndex).Value = TsrName;
                    }
                }
                catch (Exception ex)
                {

                }
                x++;
            }
            wsReport.GetCell("A" + rowIndex + 1).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            wsReport.GetCell("A" + rowIndex + 1).Value = "Total";
            wsReport.GetCell("A" + rowIndex + 2).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            wsReport.GetCell("A" + rowIndex + 2).Value = dtTeams.Rows.Count.ToString();



            #endregion
        }
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
                worker.DoWork += Report;
                worker.RunWorkerCompleted += ReportCompleted;
                worker.RunWorkerAsync();
                _reportTimer.Start();
            }
            catch (Exception ex)
            {
                HandleException(ex);
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
        private void Cal1_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            //DateTime.TryParse(Cal1.SelectedDate.ToString(), out _fromDate);
        }

        private void Cal2_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(Cal2.SelectedDate.ToString(), out _toDate);
            DateTime.TryParse(Cal2.SelectedDate.ToString(), out _fromDate);
        }
        private bool? IsAllRecordsChecked()
        {
            try
            {
                bool allSelected = (xdgAgents.Records.Select(r => (bool)((DataRecord)r).Cells["IsChecked"].Value)).All(b => b);
                bool noneSelected = (xdgAgents.Records.Select(r => (bool)((DataRecord)r).Cells["IsChecked"].Value)).All(b => !b);

                int countSelected = (xdgAgents.Records.Select(r => (bool)((DataRecord)r).Cells["IsChecked"].Value)).Count(b => b);
                lblAgents.Text = "Select Supervisor(s) " + "[" + countSelected + "]";

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

        private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void chkSelectAllCampaignTypes_Unchecked(object sender, RoutedEventArgs e)
        {
            IncludeResignedTSR = false;
        }

        private void chkSelectAllCampaignTypes_Checked(object sender, RoutedEventArgs e)
        {
            IncludeResignedTSR = true;
        }
    }
}
