using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Embriant.Framework.Configuration;
using Infragistics.Windows.DataPresenter;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using System.Collections.Generic;
using System.Windows.Resources;
using UDM.WPF.Library;
using System.Windows.Controls;
using UDM.Insurance.Interface.Windows;
using System.Data.SqlClient;
using System.Threading;
using Embriant.Framework;
using Embriant.WPF.Controls;
using UDM.Insurance.Interface.Views;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.ObjectModel;
using System.Globalization;

namespace UDM.Insurance.Interface.ViewModels
{
    public class ReportDailySalesViewModel : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Private Members

        private DataView _agents;
        private DateTime? _startDate;
        private DateTime? toDate;
        private bool headerPrefixCheckBoxChecked;
        private bool tempStaffCheckBoxChecked;
        private ICommand generateReportCommand;
        private ICommand cancelReportCommand;
        private ICommand _loadPermAgentsCommand;
        private ICommand _loadTempAgentsCommand;
        private BaseControl reportDailySalesControl;
        private int? reportMode;
        private int progressBarValue;
        private int progressBarValueCampaigns;

        private string progressText;
        private string progressTextCampaigns;
        BackgroundWorker worker = new BackgroundWorker();
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;
        private string _reportContent;
        private string reportToolTip;
        private bool closeEnabled = true;

        DataTable _dtSalesAgents;

        #endregion Private Members

        #region Constructors

        public ReportDailySalesViewModel()
        {
            LoadSalesAgentInfo();
            GenerateReportCommand = new RelayCommand(GenerateReport, EnableDisableReportButton);
            CancelReportCommand = new RelayCommand(CancelReport, EnableDisableCancelReportButton);
            LoadPermAgentsCommand = new RelayCommand(LoadPermAgents);
            LoadTempAgentsCommand = new RelayCommand(LoadTempAgents);

            ReportContent = "Generate Report";
            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
            ProgressBarValue = 0;
        }

        private void LoadTempAgents(object obj)
        {
            var filteredDivisionRows = _dtSalesAgents.Select("[FKStaffTypeID] = 2", "[Description] ASC").AsEnumerable();
            if (filteredDivisionRows.Any())
            {
                DataTable dtPermSalesConsultants = _dtSalesAgents.Select("[FKStaffTypeID] = 2", "[Description] ASC").CopyToDataTable();
                Agents = dtPermSalesConsultants.DefaultView;
            }
            else
            {
                string supervisorName = Insure.GetLoggedInUserNameAndSurname();
                BaseControl currentBaseControl = LoadbaseControl();

                currentBaseControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    currentBaseControl.ShowMessageBox(new INMessageBoxWindow1(), String.Format("{0}, there are currently no temporary agents in your team.", supervisorName), "No Data", ShowMessageType.Information);
                });

                Agents = new DataView();
            }
        }

        private void LoadPermAgents(object obj)
        {
            var filteredDivisionRows = _dtSalesAgents.Select("[FKStaffTypeID] = 1", "[Description] ASC").AsEnumerable();
            if (filteredDivisionRows.Any())
            {
                DataTable dtPermSalesConsultants = _dtSalesAgents.Select("[FKStaffTypeID] = 1", "[Description] ASC").CopyToDataTable();
                Agents = dtPermSalesConsultants.DefaultView;
            }
            else
            {
                string supervisorName = Insure.GetLoggedInUserNameAndSurname();
                BaseControl currentBaseControl = LoadbaseControl();

                reportDailySalesControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    reportDailySalesControl.ShowMessageBox(new INMessageBoxWindow1(), String.Format("{0}, there are currently no permanent agents in your team.", supervisorName), "No Data", ShowMessageType.Information);
                });

                Agents = new DataView();
            }
        }

        #endregion Constructors

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;
            ReportContent = TimeSpan.FromSeconds(_timer1).ToString();
            ReportToolTip = ReportContent;
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #region Publicly-Exposed Properties

        public string ReportContent
        {
            get { return _reportContent; }
            set
            {
                _reportContent = value;
                OnPropertyChanged("ReportContent");
            }
        }

        public string ReportToolTip
        {
            get { return reportToolTip; }
            set
            {
                reportToolTip = value;
                OnPropertyChanged("ReportToolTip");
            }
        }

        public ICommand GenerateReportCommand
        {
            get { return generateReportCommand; }
            set
            {
                generateReportCommand = value;
            }
        }

        public ICommand CancelReportCommand
        {
            get { return cancelReportCommand; }
            set
            {
                cancelReportCommand = value;
            }
        }

        public ICommand LoadPermAgentsCommand
        {
            get
            {
                return _loadPermAgentsCommand;
            }
            set
            {
                _loadPermAgentsCommand = value;
            }
        }

        public ICommand LoadTempAgentsCommand
        {
            get
            {
                return _loadTempAgentsCommand;
            }

            set
            {
                _loadTempAgentsCommand = value;
            }
        }



        public DataView Agents
        {
            get { return _agents; }
            set
            {
                _agents = value;
                OnPropertyChanged("Agents");
            }
        }

        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged("StartDate");

            }
        }

        public int? ReportMode
        {
            get { return reportMode; }
            set
            {
                reportMode = value;
                OnPropertyChanged("ReportMode");

            }
        }

        public DateTime? ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged("ToDate");
            }
        }

        public int ProgressBarValue
        {
            get { return progressBarValue; }
            set
            {
                progressBarValue = value;
                OnPropertyChanged("ProgressBarValue");
            }
        }

        public int ProgressBarValueCampaigns
        {
            get { return progressBarValueCampaigns; }
            set
            {
                progressBarValueCampaigns = value;
                OnPropertyChanged("ProgressBarValueCampaigns");
            }
        }

        public string ProgressText
        {
            get { return progressText; }
            set
            {
                progressText = value;
                OnPropertyChanged("ProgressText");
            }
        }

        public string ProgressTextCampaigns
        {
            get { return progressTextCampaigns; }
            set
            {
                progressTextCampaigns = value;
                OnPropertyChanged("ProgressTextCampaigns");
            }
        }

        public bool HeaderPrefixCheckBoxChecked
        {
            get { return headerPrefixCheckBoxChecked; }
            set
            {
                headerPrefixCheckBoxChecked = value;
                OnPropertyChanged("HeaderPrefixCheckBoxChecked");
                if (value == true)
                {
                    HeaderPrefixAreaChecked();
                }
                else
                {
                    HeaderPrefixAreaUnChecked();
                }
            }
        }

        public bool TempStaffCheckBoxChecked
        {
            get { return tempStaffCheckBoxChecked; }
            set
            {
                tempStaffCheckBoxChecked = value;
                OnPropertyChanged("TempStaffCheckBoxChecked");
                if (value == true)
                {
                    HeaderPrefixAreaChecked();
                }
                else
                {
                    HeaderPrefixAreaUnChecked();
                }
            }
        }

        #endregion Publicly-Exposed Properties

        #region Private Methods

        private void LoadSalesAgentInfo()
        {
            //SqlParameter[] parameters = new SqlParameter[1];
            //parameters[0] = new SqlParameter("@UserID", GlobalSettings.ApplicationUser.ID);
            //DataSet ds = Methods.ExecuteStoredProcedure("spGetSupervisorAgents", parameters);
            //DataTable dt = ds.Tables[0];
            //DataColumn column = new DataColumn("IsChecked", typeof(bool));
            //column.DefaultValue = false;
            //dt.Columns.Add(column);

            _dtSalesAgents = Insure.INGetDailySalesReportLookups().Tables[0];
            Agents = _dtSalesAgents.DefaultView;
        }

        private void GenerateReport(object obj)
        {
            try
            {
                //load basecontrol
                int count = Agents.Table.AsEnumerable().Where(x => (bool)x["IsChecked"] == true).Count();
                ReportDailySales dailysSales = (ReportDailySales)LoadbaseControl();
                List<object> arguments = new List<object>();
                if (dailysSales.chkTempSales.IsChecked == true)
                {
                    reportDailySalesControl = LoadbaseControl();
                    CloseEnabled = false;
                    ProgressText = "Fetching Data....";
                    worker = new BackgroundWorker();
                    worker.DoWork += Report;
                    worker.ProgressChanged += worker_ProgressChanged;
                    worker.WorkerReportsProgress = true;
                    worker.WorkerSupportsCancellation = true;
                    worker.RunWorkerCompleted += ReportCompleted;

                    DataSet ds = Methods.ExecuteStoredProcedure("sp_GetTempStaff", null);
                    DataTable dt = ds.Tables[0];
                    DataColumn column = new DataColumn("IsChecked", typeof(bool));
                    column.DefaultValue = true;
                    dt.Columns.Add(column);
                    DataView AgentsList = dt.DefaultView;

                    arguments.Add(AgentsList);
                    arguments.Add(reportDailySalesControl);
                    arguments.Add(ReportMode);
                    worker.RunWorkerAsync(arguments);
                    dispatcherTimer1.Start();

                }

                if (dailysSales.chkPermSales.IsChecked == true)
                {
                    reportDailySalesControl = LoadbaseControl();
                    CloseEnabled = false;
                    ProgressText = "Fetching Data....";
                    worker = new BackgroundWorker();
                    worker.DoWork += Report;
                    worker.ProgressChanged += worker_ProgressChanged;
                    worker.WorkerReportsProgress = true;
                    worker.WorkerSupportsCancellation = true;
                    worker.RunWorkerCompleted += ReportCompleted;

                    DataSet ds = Methods.ExecuteStoredProcedure("sp_GetPermStaff", null);
                    DataTable dt = ds.Tables[0];
                    DataColumn column = new DataColumn("IsChecked", typeof(bool));
                    column.DefaultValue = true;
                    dt.Columns.Add(column);
                    DataView AgentsList = dt.DefaultView;

                    arguments.Add(AgentsList);
                    arguments.Add(reportDailySalesControl);
                    arguments.Add(ReportMode);
                    worker.RunWorkerAsync(arguments);
                    dispatcherTimer1.Start();
                }

                //AllAgents
                if (dailysSales.chkPermSales.IsChecked == false && dailysSales.chkTempSales.IsChecked == false)
                {
                    reportDailySalesControl = LoadbaseControl();
                    CloseEnabled = false;
                    ProgressText = "Fetching Data....";
                    worker = new BackgroundWorker();
                    worker.DoWork += Report;
                    worker.ProgressChanged += worker_ProgressChanged;
                    worker.WorkerReportsProgress = true;
                    worker.WorkerSupportsCancellation = true;
                    worker.RunWorkerCompleted += ReportCompleted;
                    arguments.Add(Agents);
                    arguments.Add(reportDailySalesControl);
                    arguments.Add(ReportMode);
                    worker.RunWorkerAsync(arguments);
                    dispatcherTimer1.Start();
                }
            }
            catch (Exception ex)
            {
                reportDailySalesControl.HandleException(ex);
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Dictionary<object, int> returnObj = e.UserState as Dictionary<object, int>;
            foreach (var retobj in returnObj)//1 is first progress bar
            {
                if (retobj.Value == 1)
                {
                    Dictionary<string, long> campaignTypes = new Dictionary<string, long>();
                    foreach (KeyValuePair<object, int> keyva in returnObj)
                    {
                        campaignTypes = (Dictionary<string, long>)keyva.Key;
                        break;//there will always be 1
                    }

                    int progressVal = e.ProgressPercentage;
                    ProgressText = "Compiling Worksheet Campaign Type " + e.ProgressPercentage + " of " + campaignTypes.Count;
                    Double value = Double.Parse(progressVal.ToString()) / Double.Parse(campaignTypes.Count.ToString());

                    ProgressBarValue = int.Parse(Math.Floor(value * 100).ToString());
                }
                else // 2nd progress bar
                {
                    Dictionary<string, long> campaigns = new Dictionary<string, long>();
                    foreach (KeyValuePair<object, int> keyva in returnObj)
                    {
                        campaigns = (Dictionary<string, long>)keyva.Key;
                        break;//there will always be 1
                    }
                    int progressVal = e.ProgressPercentage;
                    ProgressTextCampaigns = "Campaign " + e.ProgressPercentage + " of " + campaigns.Count;
                    Double value = Double.Parse(progressVal.ToString()) / Double.Parse(campaigns.Count.ToString());
                    ProgressBarValueCampaigns = int.Parse(Math.Floor(value * 100).ToString());

                }
            }

        }

        private void CancelReport(object obj)
        {
            try
            {
                //load basecontrol
                reportDailySalesControl = LoadbaseControl();
                worker.CancelAsync();
                ReportDailySales reportDailySales = new ReportDailySales();
                reportDailySalesControl.ShowDialog(reportDailySales, new INDialogWindow(reportDailySales));
                WindowCollection windows = Application.Current.Windows;
                foreach (Window w in windows)
                {
                    if (w.Title == "INDialogWindow")
                    {
                        w.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                reportDailySalesControl.HandleException(ex);
            }
        }

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            ReportContent = "Generate Report";
            ProgressBarValue = 0;
            ProgressBarValueCampaigns = 0;
            ProgressText = string.Empty;
            ProgressTextCampaigns = string.Empty;
            CloseEnabled = true;

            //ProgressListVisible = Visibility.Collapsed;
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            List<object> arguments = e.Argument as List<object>;
            BaseControl reportDailySalesControl = arguments[1] as BaseControl;
            DataView agentsList = arguments[0] as DataView;
            int? selectedReportMode = arguments[2] as int?;//report mode is indexed based

            if (selectedReportMode == 0)
            {
                CancerBaseReport(reportDailySalesControl, agentsList, selectedReportMode, sender);
            }
            if (selectedReportMode == 1)
            {
                int count = agentsList.Count;
                MaccBaseReport(reportDailySalesControl, agentsList, selectedReportMode, sender);
            }
            if (selectedReportMode == 2)
            {
                UpgradeReport(reportDailySalesControl, agentsList, selectedReportMode, sender);
            }
        }

        #endregion Private Methods

        public DataView GetPermAgents()
        {
            DataSet ds = Methods.ExecuteStoredProcedure("sp_GetPermStaff", null);
            DataTable dt = ds.Tables[0];
            DataColumn column = new DataColumn("IsChecked", typeof(bool));
            column.DefaultValue = false;
            dt.Columns.Add(column);
            DataView Agents = dt.DefaultView;
            return Agents;
        }
        private void ReportTemps(object sender, DoWorkEventArgs e)
        {
            List<object> arguments = e.Argument as List<object>;
            BaseControl reportDailySalesControl = arguments[1] as BaseControl;
            DataView agentsList = arguments[0] as DataView;
            int? selectedReportMode = arguments[2] as int?;//report mode is indexed based

            if (selectedReportMode == 0)
            {
                CancerBaseReport(reportDailySalesControl, agentsList, selectedReportMode, sender);
            }
            if (selectedReportMode == 1)
            {
                MaccBaseReport(reportDailySalesControl, agentsList, selectedReportMode, sender);
            }
            if (selectedReportMode == 2)
            {
                UpgradeReport(reportDailySalesControl, agentsList, selectedReportMode, sender);
            }
        }
        private void CancerBaseReport(BaseControl reportDailySalesControl, DataView agentsList, int? selectedReportMode, object sender)
        {
            try
            {
                reportDailySalesControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate { reportDailySalesControl.Cursor = Cursors.Wait; });

                DataTable dtLeadAllocationData;
                DataTable dtAgentHours;
                DataTable dtTsrTargets;
                DataTable dtSuperVisor;
                string AgentIDs = string.Empty;
                bool first = true;
                if (agentsList != null)
                {
                    foreach (DataRowView agent in agentsList)
                    {
                        if (first == true)
                        {
                            first = false;
                            if (agent["ID"] != null)
                            {
                                AgentIDs = Convert.ToInt32(agent["ID"].ToString()) + ",";
                            }
                        }
                        else
                        {
                            if (agent["ID"] != null)
                            {
                                AgentIDs = AgentIDs + Convert.ToInt32(agent["ID"].ToString()) + ",";
                            }
                        }
                    }
                }

                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@AgentIDs", AgentIDs);
                parameters[1] = new SqlParameter("@FromDate", StartDate.Value.ToString("yyy-MM-dd"));
                parameters[2] = new SqlParameter("@ToDate", ToDate.Value.ToString("yyy-MM-dd"));
                parameters[3] = new SqlParameter("@ReportMode", selectedReportMode);

                DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spInReportDailySales", parameters, false);
                if (dsLeadAllocationData.Tables.Count > 0)
                {
                    dtLeadAllocationData = dsLeadAllocationData.Tables[0];
                    dtAgentHours = dsLeadAllocationData.Tables[1];
                    dtTsrTargets = dsLeadAllocationData.Tables[2];
                    dtSuperVisor = dsLeadAllocationData.Tables[3];
                    if (dtLeadAllocationData.Rows.Count == 0)
                    {
                        reportDailySalesControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            reportDailySalesControl.ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected Agent(s)", "No Data", ShowMessageType.Information);
                        });

                        return;
                    }
                }
                else
                {
                    reportDailySalesControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        reportDailySalesControl.ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected Agent(s) ", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                //load campaignTypes (we already know this will return only upgrade campaign groups therefore we only need to determine the type)
                Dictionary<string, long> campaigntypes = new Dictionary<string, long>();
                foreach (DataRow rw in dtLeadAllocationData.Rows)
                {
                    if (!campaigntypes.ContainsValue(int.Parse(rw["CampaignTypeID"].ToString())))
                    {
                        campaigntypes.Add(rw["CampaignType"].ToString(), int.Parse(rw["CampaignTypeID"].ToString()));

                    }
                }
                //re-iterate by campaign type


                #region report Data
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                xlApp = new Excel.Application();
                object misValue = System.Reflection.Missing.Value;
                xlWorkBook = xlApp.Workbooks.Add(misValue);

                string filePathAndName = GlobalSettings.UserFolder + " Daily Sales  Report ~ " + Guid.NewGuid() + ".xls";


                int campaignTypeRow = 0;
                foreach (KeyValuePair<string, long> campaignType in campaigntypes)
                {
                    campaignTypeRow++;
                    Dictionary<object, int> returnObject = new Dictionary<object, int>();
                    returnObject.Add(campaigntypes, 1);//the 1 indicates that its for the first ptogress bar
                    (sender as BackgroundWorker).ReportProgress(campaignTypeRow, returnObject);

                    Excel.Worksheet xlWorkSheet;
                    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Add();  //.Worksheets.get_Item(1);
                    xlWorkSheet.Name = campaignType.Key + " Base";

                    #region Headings
                    System.Globalization.DateTimeFormatInfo dtf = new System.Globalization.DateTimeFormatInfo();
                    xlWorkSheet.Cells[1, 1] = dtf.GetMonthName(StartDate.Value.Month) + " " + StartDate.Value.Year;
                    Excel.Range c1 = xlWorkSheet.Cells[1, 1];
                    Excel.Range c2 = xlWorkSheet.Cells[1, 2];
                    Excel.Range range = (Excel.Range)xlWorkSheet.get_Range(c1, c2);
                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    range.Font.Bold = true;
                    range.Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    range.Merge(true);

                    //Dates (get an array of dates between from and to date)

                    int numberOFDaysBetween = int.Parse(ToDate.Value.Subtract(StartDate.Value).TotalDays.ToString());
                    List<DateTime> dates = new List<DateTime>();
                    for (int i = 0; i <= numberOFDaysBetween; i++)
                    {
                        DateTime newDate = StartDate.Value.AddDays(i);
                        if (newDate.DayOfWeek != DayOfWeek.Sunday)
                        {
                            dates.Add(newDate);
                        }
                    }

                    int DateColIndex = 3;
                    xlWorkSheet.Cells[2, 2] = campaignType.Key + " Base"; xlWorkSheet.Cells[2, 2].Font.Bold = true; xlWorkSheet.Cells[2, 2].ColumnWidth = 20; xlWorkSheet.Cells[2, 2].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    xlWorkSheet.Cells[3, 1] = "SUP"; xlWorkSheet.Cells[3, 1].ColumnWidth = 5; xlWorkSheet.Cells[3, 1].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    xlWorkSheet.Cells[3, 2] = "Sales Consultant"; xlWorkSheet.Cells[3, 2].ColumnWidth = 20; xlWorkSheet.Cells[3, 2].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    xlWorkSheet.Cells[4, 1].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, 2].Borders.Weight = Excel.XlBorderWeight.xlThin;



                    foreach (DateTime date in dates)
                    {
                        int weekindex = GetWeekOfMonth(date);

                        xlWorkSheet.Cells[2, DateColIndex] = dtf.GetDayName(date.DayOfWeek);
                        xlWorkSheet.Cells[3, DateColIndex] = date.Day + "-" + dtf.GetAbbreviatedMonthName(date.Month);

                        Excel.Range f1 = xlWorkSheet.Cells[2, DateColIndex];
                        Excel.Range f2 = xlWorkSheet.Cells[3, DateColIndex + 4];
                        Excel.Range range4 = (Excel.Range)xlWorkSheet.get_Range(f1, f2);
                        range4.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        //range4.Font.Bold = true;
                        range4.Borders.Weight = Excel.XlBorderWeight.xlThin;
                        range4.Merge(true);

                        xlWorkSheet.Cells[4, DateColIndex] = "Hours"; xlWorkSheet.Cells[4, DateColIndex].Borders.Weight = Excel.XlBorderWeight.xlThin;
                        xlWorkSheet.Cells[4, DateColIndex + 1] = "Premium"; xlWorkSheet.Cells[4, DateColIndex + 1].Borders.Weight = Excel.XlBorderWeight.xlThin;
                        xlWorkSheet.Cells[4, DateColIndex + 2] = "Base"; xlWorkSheet.Cells[4, DateColIndex + 2].Borders.Weight = Excel.XlBorderWeight.xlThin;
                        xlWorkSheet.Cells[4, DateColIndex + 3] = "Partners"; xlWorkSheet.Cells[4, DateColIndex + 3].Borders.Weight = Excel.XlBorderWeight.xlThin;
                        xlWorkSheet.Cells[4, DateColIndex + 4] = "Children"; xlWorkSheet.Cells[4, DateColIndex + 4].Borders.Weight = Excel.XlBorderWeight.xlThin;



                        DateColIndex++; DateColIndex++; DateColIndex++; DateColIndex++; DateColIndex++;
                        if (date.DayOfWeek == DayOfWeek.Saturday || date == dates[dates.Count - 1])
                        {

                            int interiorColorIndex = 34;//interior color index for this grouping
                            //week target********************************************************************************************
                            xlWorkSheet.Cells[2, DateColIndex] = "WEEK TARGET";
                            xlWorkSheet.Cells[3, DateColIndex] = "Week " + weekindex; xlWorkSheet.Cells[3, DateColIndex].Interior.ColorIndex = interiorColorIndex;
                            Excel.Range d1 = xlWorkSheet.Cells[2, DateColIndex];
                            Excel.Range d2 = xlWorkSheet.Cells[2, DateColIndex + 4];
                            Excel.Range range2 = (Excel.Range)xlWorkSheet.get_Range(d1, d2);  //range2.Interior.ColorIndex = interiorColorIndex;
                            range2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range2.Font.Bold = true;
                            range2.Borders.Weight = Excel.XlBorderWeight.xlThin;
                            range2.Merge(true);


                            Excel.Range e1 = xlWorkSheet.Cells[3, DateColIndex];
                            Excel.Range e2 = xlWorkSheet.Cells[2, DateColIndex + 4];
                            Excel.Range range3 = (Excel.Range)xlWorkSheet.get_Range(e1, e2); range3.Interior.ColorIndex = interiorColorIndex;
                            range3.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range3.Font.Bold = true;
                            range3.Borders.Weight = Excel.XlBorderWeight.xlThin;
                            range3.Merge(true);

                            xlWorkSheet.Cells[4, DateColIndex] = "Hours"; xlWorkSheet.Cells[4, DateColIndex].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex].Interior.ColorIndex = interiorColorIndex; xlWorkSheet.Cells[4, DateColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 1] = "Premium"; xlWorkSheet.Cells[4, DateColIndex + 1].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 1].Interior.ColorIndex = interiorColorIndex; xlWorkSheet.Cells[4, DateColIndex + 1].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 2] = "Base"; xlWorkSheet.Cells[4, DateColIndex + 2].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 2].Interior.ColorIndex = interiorColorIndex; xlWorkSheet.Cells[4, DateColIndex + 2].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 3] = "Partners"; xlWorkSheet.Cells[4, DateColIndex + 3].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 3].Interior.ColorIndex = interiorColorIndex; xlWorkSheet.Cells[4, DateColIndex + 3].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 4] = "Children"; xlWorkSheet.Cells[4, DateColIndex + 4].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 4].Interior.ColorIndex = interiorColorIndex; xlWorkSheet.Cells[4, DateColIndex + 4].Font.Bold = true;
                            DateColIndex++; DateColIndex++; DateColIndex++; DateColIndex++; DateColIndex++;
                            //week target*********************************************************************************************

                            //week actual******************************************************************************************
                            int interiorColorIndex2 = 19;
                            xlWorkSheet.Cells[2, DateColIndex] = "WEEK ACTUAL";
                            xlWorkSheet.Cells[3, DateColIndex] = "Week " + weekindex;
                            Excel.Range g1 = xlWorkSheet.Cells[2, DateColIndex];
                            Excel.Range g2 = xlWorkSheet.Cells[3, DateColIndex + 5];
                            Excel.Range range5 = (Excel.Range)xlWorkSheet.get_Range(g1, g2); range5.Interior.ColorIndex = interiorColorIndex2;
                            range5.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range5.Font.Bold = true;
                            range5.Borders.Weight = Excel.XlBorderWeight.xlThin;
                            range5.Merge(true);

                            Excel.Range h1 = xlWorkSheet.Cells[3, DateColIndex];
                            Excel.Range h2 = xlWorkSheet.Cells[2, DateColIndex + 4];
                            Excel.Range range6 = (Excel.Range)xlWorkSheet.get_Range(e1, e2); //range6.Interior.ColorIndex = interiorColorIndex2;
                            range6.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range6.Font.Bold = true;
                            range6.Borders.Weight = Excel.XlBorderWeight.xlThin;
                            range6.Merge(true);

                            xlWorkSheet.Cells[4, DateColIndex] = "Hours"; xlWorkSheet.Cells[4, DateColIndex].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex].Interior.ColorIndex = interiorColorIndex2; xlWorkSheet.Cells[4, DateColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 1] = "Premium"; xlWorkSheet.Cells[4, DateColIndex + 1].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 1].Interior.ColorIndex = interiorColorIndex2; xlWorkSheet.Cells[4, DateColIndex + 1].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 2] = "Base"; xlWorkSheet.Cells[4, DateColIndex + 2].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 2].Interior.ColorIndex = interiorColorIndex2; xlWorkSheet.Cells[4, DateColIndex + 2].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 3] = "Partners"; xlWorkSheet.Cells[4, DateColIndex + 3].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 3].Interior.ColorIndex = interiorColorIndex2; xlWorkSheet.Cells[4, DateColIndex + 3].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 4] = "Children"; xlWorkSheet.Cells[4, DateColIndex + 4].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 4].Interior.ColorIndex = interiorColorIndex2; xlWorkSheet.Cells[4, DateColIndex + 4].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 5] = "S/H"; xlWorkSheet.Cells[4, DateColIndex + 5].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 5].Interior.ColorIndex = interiorColorIndex2; xlWorkSheet.Cells[4, DateColIndex + 5].Font.Bold = true;
                            DateColIndex++; DateColIndex++; DateColIndex++; DateColIndex++; DateColIndex++; DateColIndex++;
                            //Week actual******************************************************************************************

                        }

                    }

                    #endregion Headings


                    //re-iterate by campaign codes **get current  campaign codes for campaign type
                    var dtCampaignsForType = dtLeadAllocationData.AsEnumerable().Where(x => (long)x["CampaignTypeID"] == campaignType.Value);
                    Dictionary<string, long> campaigns = new Dictionary<string, long>();

                    foreach (DataRow rw in dtCampaignsForType)
                    {
                        if (!campaigns.ContainsValue(long.Parse(rw["CampaignID"].ToString())))
                        {
                            campaigns.Add(rw["CampaignCode"].ToString(), long.Parse(rw["CampaignID"].ToString()));
                        }
                    }
                    //***************************////////////////////*****************
                    int rowIndex = 5;
                    int campaignRow = 0;
                    foreach (KeyValuePair<string, long> campaign in campaigns)
                    {
                        campaignRow++;
                        Dictionary<object, int> returnObjectCampaign = new Dictionary<object, int>();
                        returnObjectCampaign.Add(campaigns, 2);//the 2 indicates that its for the second ptogress bar
                        (sender as BackgroundWorker).ReportProgress(campaignRow, returnObjectCampaign);

                        List<TotalObject> HoursDayTotals = new List<TotalObject>();
                        List<TotalObject> PremiumDayTotals = new List<TotalObject>();
                        List<TotalObject> BaseDayTotals = new List<TotalObject>();
                        List<TotalObject> PartnerDayTotals = new List<TotalObject>();
                        List<TotalObject> ChildrenDayTotals = new List<TotalObject>();

                        //week target
                        List<TotalObject> HoursWeekTotals = new List<TotalObject>();
                        List<TotalObject> PremiumWeekTotals = new List<TotalObject>();
                        List<TotalObject> BaseWeekTotals = new List<TotalObject>();
                        List<TotalObject> PartnerWeekTotals = new List<TotalObject>();
                        List<TotalObject> ChildrenWeekTotals = new List<TotalObject>();
                        //week actual
                        List<TotalObject> HoursWeekActualTotals = new List<TotalObject>();
                        List<TotalObject> PremiumWeekActualTotals = new List<TotalObject>();
                        List<TotalObject> BaseWeekActualTotals = new List<TotalObject>();
                        List<TotalObject> PartnerWeekActualTotals = new List<TotalObject>();
                        List<TotalObject> ChildrenWeekActualTotals = new List<TotalObject>();

                        xlWorkSheet.Cells[rowIndex, 1] = campaign.Key; xlWorkSheet.Cells[rowIndex, 1].Font.Bold = true;
                        for (int i = 1; i <= DateColIndex - 1; i++)//Blue formatting
                        {
                            xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 37;
                            xlWorkSheet.Cells[rowIndex, i].Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlMedium;
                            xlWorkSheet.Cells[rowIndex, i].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;
                        }
                        rowIndex++;
                        var campaignData = dtCampaignsForType.AsEnumerable().Where(x => (long)x["CampaignID"] == campaign.Value);
                        Dictionary<string, long> agentsForCampaign = new Dictionary<string, long>();

                        foreach (DataRow rw in campaignData)
                        {

                            if (!agentsForCampaign.ContainsValue(long.Parse(rw["FKUserID"].ToString())))
                            {
                                agentsForCampaign.Add(rw["Agent"].ToString(), long.Parse(rw["FKUserID"].ToString()));
                                var supervisor = dtSuperVisor.AsEnumerable().Where(x => (long)x["FKUserID"] == long.Parse(rw["FKUserID"].ToString()));
                                string superVisorName = string.Empty;
                                bool firstTime = true;
                                foreach (DataRow sup in supervisor)
                                {
                                    if (firstTime == true)
                                    {
                                        firstTime = false;
                                        superVisorName = sup["SuperVisorCode"].ToString();
                                    }
                                    else
                                    {
                                        superVisorName = superVisorName + "," + sup["SuperVisorCode"].ToString();

                                    }

                                }
                                xlWorkSheet.Cells[rowIndex, 2] = rw["Agent"].ToString();
                                xlWorkSheet.Cells[rowIndex, 1] = superVisorName; xlWorkSheet.Cells[rowIndex, 1].Font.Bold = true;
                                int resultColumn = 3;
                                //now we want to re-iterate this based on column first


                                double totalPremiumForWeek = 0;
                                double totalBaseForWeek = 0;
                                int totalChildrenForWeek = 0;
                                int totalPartnerForWeek = 0;
                                double totalHoursForWeek = 0;
                                foreach (DateTime resultDate in dates)
                                {
                                    //determine which week in the month the date belongs to 

                                    int currentWeek = GetWeekOfMonth(resultDate);

                                    bool skippedRows = false;
                                    var resultsForAgent = campaignData.AsEnumerable().Where(x => (long)x["FKUserID"] == (long)rw["FKUserID"]);
                                    var resultsForAgentDate = resultsForAgent.AsEnumerable().Where(x => DateTime.Parse(x["DateOfSale"].ToString()).ToString("yyy-MM-dd") == resultDate.ToString("yyy-MM-dd"));
                                    if (resultsForAgentDate.Count() > 0)
                                    {
                                        //count figures for date
                                        double totalPremiumForday = 0;
                                        int childCountForDay = 0;
                                        int partnerCountForDay = 0;
                                        int baseCount = 0;
                                        double agentHoursForDay = 0;
                                        foreach (DataRow rwResult in resultsForAgentDate)
                                        {
                                            string dateOfSale = rwResult["DateOfSale"].ToString();
                                            DateTime DateOfSale = DateTime.Parse(dateOfSale);

                                            totalPremiumForday = totalPremiumForday + Double.Parse(rwResult["Premium"].ToString());

                                            if ((bool)rwResult["OptionChild"] == true)
                                            {
                                                childCountForDay++;
                                            }
                                            if ((bool)rwResult["OptionLA2"] == true)
                                            {
                                                partnerCountForDay++;
                                            }
                                            baseCount++;
                                        }
                                        //calculate hours for date
                                        var agentHours = dtAgentHours.AsEnumerable().Where(x => (long)x["FKUserID"] == (long)rw["FKUserID"] && DateTime.Parse(x["WorkingDate"].ToString()).ToString("yyy-MM-dd") == resultDate.ToString("yyy-MM-dd") && (long)x["FKINCampaignID"] == campaign.Value);

                                        if (agentHours.Count() > 0)
                                        {
                                            foreach (DataRow rwAgentHour in agentHours)
                                            {
                                                //calculate morning shift total hours
                                                if (rwAgentHour["MorningShiftStartTime"].ToString() != string.Empty && rwAgentHour["MorningShiftEndTime"].ToString() != string.Empty)
                                                {
                                                    TimeSpan morningShiftStart = (TimeSpan)rwAgentHour["MorningShiftStartTime"];
                                                    TimeSpan morningShiftEnd = (TimeSpan)rwAgentHour["MorningShiftEndTime"];
                                                    agentHoursForDay = agentHoursForDay + morningShiftEnd.Subtract(morningShiftStart).TotalHours;
                                                }
                                                //calculate normal shift total hours
                                                if (rwAgentHour["NormalShiftStartTime"].ToString() != string.Empty && rwAgentHour["NormalShiftEndTime"].ToString() != string.Empty)
                                                {
                                                    TimeSpan normalShiftStart = (TimeSpan)rwAgentHour["NormalShiftStartTime"];
                                                    TimeSpan normalShiftEnd = (TimeSpan)rwAgentHour["NormalShiftEndTime"];
                                                    agentHoursForDay = agentHoursForDay + normalShiftEnd.Subtract(normalShiftStart).TotalHours;
                                                }
                                                //calculate evening shift total hours
                                                if (rwAgentHour["EveningShiftStartTime"].ToString() != string.Empty && rwAgentHour["EveningShiftEndTime"].ToString() != string.Empty)
                                                {
                                                    TimeSpan eveningShiftStart = (TimeSpan)rwAgentHour["EveningShiftStartTime"];
                                                    TimeSpan eveningShiftEnd = (TimeSpan)rwAgentHour["EveningShiftEndTime"];
                                                    agentHoursForDay = agentHoursForDay + eveningShiftEnd.Subtract(eveningShiftStart).TotalHours;
                                                }
                                                //calculate weekend shift total hours
                                                if (rwAgentHour["PublicHolidayWeekendShiftStartTime"].ToString() != string.Empty && rwAgentHour["PublicHolidayWeekendShiftEndTime"].ToString() != string.Empty)
                                                {
                                                    TimeSpan weekendShiftStart = (TimeSpan)rwAgentHour["PublicHolidayWeekendShiftStartTime"];
                                                    TimeSpan weekendShiftEnd = (TimeSpan)rwAgentHour["PublicHolidayWeekendShiftEndTime"];
                                                    agentHoursForDay = agentHoursForDay + weekendShiftEnd.Subtract(weekendShiftStart).TotalHours;
                                                }
                                            }
                                        }
                                        //hours for date//
                                        xlWorkSheet.Cells[rowIndex, resultColumn] = Math.Round(agentHoursForDay, 2); xlWorkSheet.Cells[rowIndex, resultColumn].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                        totalHoursForWeek = totalHoursForWeek + agentHoursForDay;
                                        HoursDayTotals.Add(new TotalObject(resultDate, agentHoursForDay, campaign.Key));
                                        resultColumn++;
                                        //////////
                                        //units for date//
                                        xlWorkSheet.Cells[rowIndex, resultColumn] = totalPremiumForday; xlWorkSheet.Cells[rowIndex, resultColumn].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                        totalPremiumForWeek = totalPremiumForWeek + totalPremiumForday;

                                        PremiumDayTotals.Add(new TotalObject(resultDate, totalPremiumForday, campaign.Key));
                                        resultColumn++;
                                        ////
                                        ////base for date
                                        xlWorkSheet.Cells[rowIndex, resultColumn] = baseCount; xlWorkSheet.Cells[rowIndex, resultColumn].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                        totalBaseForWeek = totalBaseForWeek + baseCount;
                                        BaseDayTotals.Add(new TotalObject(resultDate, baseCount, campaign.Key));
                                        resultColumn++;
                                        ////////
                                        ////partners for date
                                        xlWorkSheet.Cells[rowIndex, resultColumn] = partnerCountForDay; xlWorkSheet.Cells[rowIndex, resultColumn].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                        totalPartnerForWeek = totalPartnerForWeek + partnerCountForDay;
                                        PartnerDayTotals.Add(new TotalObject(resultDate, partnerCountForDay, campaign.Key));
                                        resultColumn++;
                                        ////////
                                        ////children for date
                                        xlWorkSheet.Cells[rowIndex, resultColumn] = childCountForDay; xlWorkSheet.Cells[rowIndex, resultColumn].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                        totalChildrenForWeek = totalChildrenForWeek + childCountForDay;
                                        ChildrenDayTotals.Add(new TotalObject(resultDate, childCountForDay, campaign.Key));
                                        resultColumn++;
                                        //////


                                        if (resultDate.DayOfWeek == DayOfWeek.Saturday || resultDate == dates[dates.Count - 1])
                                        {
                                            if (skippedRows == false)
                                            {

                                                double hoursTarget = 0;
                                                double premiumTarget = 0;
                                                double basetarget = 0;
                                                double childrentarget = 0;

                                                //week target
                                                //hours 
                                                var currentWeekData = dtTsrTargets.AsEnumerable().Where(x => (long)x["FKAgentID"] == (long)rw["FKUserID"] && (long)x["FKINCampaignID"] == campaign.Value && (long)x["FKINWeekID"] == (long)currentWeek);
                                                if (currentWeekData.Count() > 0)
                                                {
                                                    foreach (DataRow targetRow in currentWeekData)
                                                    {
                                                        hoursTarget = hoursTarget + double.Parse(targetRow["Hours"].ToString());
                                                        premiumTarget = premiumTarget + double.Parse(targetRow["PremiumTarget"].ToString());
                                                        basetarget = basetarget + double.Parse(targetRow["BaseTarget"].ToString());
                                                    }
                                                }
                                                xlWorkSheet.Cells[rowIndex, resultColumn] = hoursTarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                                HoursWeekTotals.Add(new TotalObject(resultDate, hoursTarget, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = premiumTarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                                PremiumWeekTotals.Add(new TotalObject(resultDate, premiumTarget, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = basetarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                                BaseWeekTotals.Add(new TotalObject(resultDate, basetarget, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = "0"; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;//partner target                                               
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = childrentarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                                ChildrenWeekTotals.Add(new TotalObject(resultDate, childrentarget, campaign.Key));
                                                resultColumn++;
                                                //week actual

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = Math.Round(totalHoursForWeek, 2); xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                                HoursWeekActualTotals.Add(new TotalObject(currentWeek, totalHoursForWeek, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = totalPremiumForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                                PremiumWeekActualTotals.Add(new TotalObject(currentWeek, totalPremiumForWeek, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = totalBaseForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                                BaseWeekActualTotals.Add(new TotalObject(currentWeek, totalBaseForWeek, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = totalPartnerForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;//partners actual                                               
                                                PartnerWeekActualTotals.Add(new TotalObject(currentWeek, totalPartnerForWeek, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = totalChildrenForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                                ChildrenWeekActualTotals.Add(new TotalObject(currentWeek, totalChildrenForWeek, campaign.Key));
                                                resultColumn++;

                                                double shActual = 0;
                                                if (totalHoursForWeek > 0 && totalBaseForWeek > 0)
                                                {
                                                    shActual = Math.Round((totalBaseForWeek / totalHoursForWeek), 2);
                                                }
                                                xlWorkSheet.Cells[rowIndex, resultColumn] = shActual; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;//  S/H actual

                                                resultColumn++;
                                                skippedRows = true;
                                                totalPremiumForWeek = 0;
                                                totalBaseForWeek = 0;
                                                totalPartnerForWeek = 0;
                                                totalChildrenForWeek = 0;
                                                totalHoursForWeek = 0;
                                            }
                                        }

                                    }
                                    else
                                    {

                                        resultColumn++;
                                        resultColumn++;
                                        resultColumn++;
                                        resultColumn++;
                                        resultColumn++;
                                    }

                                    if (resultDate.DayOfWeek == DayOfWeek.Saturday || resultDate == dates[dates.Count - 1])
                                    {
                                        if (skippedRows == false)
                                        {

                                            double hoursTarget = 0;
                                            double premiumTarget = 0;
                                            double basetarget = 0;
                                            double childrentarget = 0;

                                            //week target
                                            //hours 
                                            var currentWeekData = dtTsrTargets.AsEnumerable().Where(x => (long)x["FKAgentID"] == (long)rw["FKUserID"] && (long)x["FKINCampaignID"] == campaign.Value && (long)x["FKINWeekID"] == (long)currentWeek);
                                            if (currentWeekData.Count() > 0)
                                            {
                                                foreach (DataRow targetRow in currentWeekData)
                                                {
                                                    hoursTarget = hoursTarget + double.Parse(targetRow["Hours"].ToString());
                                                    premiumTarget = premiumTarget + double.Parse(targetRow["PremiumTarget"].ToString());
                                                    basetarget = basetarget + double.Parse(targetRow["BaseTarget"].ToString());
                                                }
                                            }
                                            xlWorkSheet.Cells[rowIndex, resultColumn] = hoursTarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                            HoursWeekTotals.Add(new TotalObject(resultDate, hoursTarget, campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = premiumTarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                            PremiumWeekTotals.Add(new TotalObject(resultDate, premiumTarget, campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = basetarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                            BaseWeekTotals.Add(new TotalObject(resultDate, basetarget, campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = "0"; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;//partner target                                               
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = childrentarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                            ChildrenWeekTotals.Add(new TotalObject(resultDate, childrentarget, campaign.Key));
                                            resultColumn++;
                                            //week actual

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = Math.Round(totalHoursForWeek, 2); xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                            HoursWeekActualTotals.Add(new TotalObject(currentWeek, Math.Round(totalHoursForWeek, 2), campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = totalPremiumForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                            PremiumWeekActualTotals.Add(new TotalObject(currentWeek, totalPremiumForWeek, campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = totalBaseForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                            BaseWeekActualTotals.Add(new TotalObject(currentWeek, totalBaseForWeek, campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = totalPartnerForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;//total partner for week                                           
                                            PartnerWeekActualTotals.Add(new TotalObject(currentWeek, totalPartnerForWeek, campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = totalChildrenForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                            ChildrenWeekActualTotals.Add(new TotalObject(currentWeek, totalChildrenForWeek, campaign.Key));
                                            resultColumn++;

                                            double shActual = 0;
                                            if (totalHoursForWeek > 0 && totalBaseForWeek > 0)
                                            {
                                                shActual = Math.Round((totalBaseForWeek / totalHoursForWeek), 2);
                                            }
                                            xlWorkSheet.Cells[rowIndex, resultColumn] = shActual; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19; // S/H actual                                           
                                            resultColumn++;

                                            skippedRows = true;
                                            totalPremiumForWeek = 0;
                                            totalBaseForWeek = 0;
                                            totalPartnerForWeek = 0;
                                            totalChildrenForWeek = 0;
                                            totalHoursForWeek = 0;
                                        }
                                    }

                                }
                                //cell borders
                                for (int i = 1; i <= resultColumn - 1; i++)
                                {
                                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                }

                                rowIndex++;
                            }

                        }

                        //totals here///////////////////
                        xlWorkSheet.Cells[rowIndex, 2] = "TOTALS"; xlWorkSheet.Cells[rowIndex, 2].Font.Bold = true;
                        xlWorkSheet.Cells[rowIndex, 2].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                        int totalColIndex = 3;

                        foreach (DateTime totalDate in dates)
                        {
                            int currentWeekIndex = GetWeekOfMonth(totalDate);
                            var totalHours = (from h in HoursDayTotals where DateTime.Parse(h.Description.ToString()).Date == totalDate.Date select h);
                            var totalPremiums = (from u in PremiumDayTotals where DateTime.Parse(u.Description.ToString()).Date == totalDate.Date select u);
                            var totalBase = (from b in BaseDayTotals where DateTime.Parse(b.Description.ToString()).Date == totalDate.Date select b);
                            var totalPartner = (from p in PartnerDayTotals where DateTime.Parse(p.Description.ToString()).Date == totalDate.Date select p);
                            var totalChildren = (from c in ChildrenDayTotals where DateTime.Parse(c.Description.ToString()).Date == totalDate.Date select c);

                            double totalHoursForDay = 0;
                            double totalPremiumForDay = 0;
                            int totalBaseForDay = 0;
                            int totalPartnerForDay = 0;
                            int totalChildrenForDay = 0;
                            foreach (TotalObject unit in totalPremiums)
                            {
                                totalPremiumForDay = totalPremiumForDay + double.Parse(unit.Value.ToString());
                            }
                            foreach (TotalObject baseTarg in totalBase)
                            {
                                totalBaseForDay = totalBaseForDay + int.Parse(baseTarg.Value.ToString());
                            }
                            foreach (TotalObject partner in totalPartner)
                            {
                                totalPartnerForDay = totalPartnerForDay + int.Parse(partner.Value.ToString());
                            }
                            foreach (TotalObject children in totalChildren)
                            {
                                totalChildrenForDay = totalChildrenForDay + int.Parse(children.Value.ToString());
                            }
                            foreach (TotalObject hour in totalHours)
                            {
                                totalHoursForDay = totalHoursForDay + double.Parse(hour.Value.ToString());
                            }
                            xlWorkSheet.Cells[rowIndex, totalColIndex] = Math.Round(totalHoursForDay, 2); xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            totalColIndex++;


                            xlWorkSheet.Cells[rowIndex, totalColIndex] = totalPremiumForDay; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            totalColIndex++;

                            xlWorkSheet.Cells[rowIndex, totalColIndex] = totalBaseForDay; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            totalColIndex++;

                            xlWorkSheet.Cells[rowIndex, totalColIndex] = totalPartnerForDay; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;//total partner for day
                            xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            totalColIndex++;

                            xlWorkSheet.Cells[rowIndex, totalColIndex] = totalChildrenForDay; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            totalColIndex++;


                            if (totalDate.Date.DayOfWeek == DayOfWeek.Saturday || totalDate == dates[dates.Count - 1])
                            {

                                var totalHoursWeek = (from h in HoursWeekTotals where DateTime.Parse(h.Description.ToString()).Date == totalDate.Date select h);
                                var totalPremiumWeek = (from u in PremiumWeekTotals where DateTime.Parse(u.Description.ToString()).Date == totalDate.Date select u);
                                var totalBaseWeek = (from b in BaseWeekTotals where DateTime.Parse(b.Description.ToString()).Date == totalDate.Date select b);
                                var totalPartnerWeek = (from p in PartnerWeekTotals where DateTime.Parse(p.Description.ToString()).Date == totalDate.Date select p);
                                var totalChildrenWeek = (from c in ChildrenWeekTotals where DateTime.Parse(c.Description.ToString()).Date == totalDate.Date select c);

                                double totalHoursForWeek = 0;
                                double totalUnitsForWeek = 0;
                                int totalBaseForWeek = 0;
                                int totalPartnerForWeek = 0;
                                int totalChildrenForWeek = 0;
                                foreach (TotalObject unit in totalPremiumWeek)
                                {
                                    totalUnitsForWeek = totalUnitsForWeek + double.Parse(unit.Value.ToString());
                                }
                                foreach (TotalObject baseTarg in totalBaseWeek)
                                {
                                    totalBaseForWeek = totalBaseForWeek + int.Parse(baseTarg.Value.ToString());
                                }
                                foreach (TotalObject partner in totalPartnerWeek)
                                {
                                    totalPartnerForWeek = totalPartnerForWeek + int.Parse(partner.Value.ToString());
                                }
                                foreach (TotalObject children in totalChildrenWeek)
                                {
                                    totalChildrenForWeek = totalChildrenForWeek + int.Parse(children.Value.ToString());
                                }
                                foreach (TotalObject hour in totalHoursWeek)
                                {
                                    totalHoursForWeek = totalHoursForWeek + double.Parse(hour.Value.ToString());
                                }

                                //week target
                                xlWorkSheet.Cells[rowIndex, totalColIndex] = Math.Round(totalHoursForWeek, 2); xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 34;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalUnitsForWeek; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 34;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalBaseForWeek; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 34;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalPartnerForWeek; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;//total partner for week
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 34;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalChildrenForWeek; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 34;
                                totalColIndex++;

                                //week actual
                                var totalHoursWeekActual = (from h in HoursWeekActualTotals where int.Parse(h.Description.ToString()) == currentWeekIndex select h);
                                var totalPremiumWeekActual = (from u in PremiumWeekActualTotals where int.Parse(u.Description.ToString()) == currentWeekIndex select u);
                                var totalBaseWeekActual = (from b in BaseWeekActualTotals where int.Parse(b.Description.ToString()) == currentWeekIndex select b);
                                var totalPartnerWeekActual = (from p in PartnerWeekActualTotals where int.Parse(p.Description.ToString()) == currentWeekIndex select p);
                                var totalChildrenWeekActual = (from c in ChildrenWeekActualTotals where int.Parse(c.Description.ToString()) == currentWeekIndex select c);

                                double totalHoursForWeekActual = 0;
                                double totalUnitsForWeekActual = 0;
                                int totalBaseForWeekActual = 0;
                                int totalPartnerForWeekActual = 0;
                                int totalChildrenForWeekActual = 0;
                                foreach (TotalObject unit in totalPremiumWeekActual)
                                {
                                    totalUnitsForWeekActual = totalUnitsForWeekActual + double.Parse(unit.Value.ToString());
                                }
                                foreach (TotalObject baseTarg in totalBaseWeekActual)
                                {
                                    totalBaseForWeekActual = totalBaseForWeekActual + int.Parse(baseTarg.Value.ToString());
                                }
                                foreach (TotalObject partner in totalPartnerWeekActual)
                                {
                                    totalPartnerForWeekActual = totalPartnerForWeekActual + int.Parse(partner.Value.ToString());
                                }
                                foreach (TotalObject children in totalChildrenWeekActual)
                                {
                                    totalChildrenForWeekActual = totalChildrenForWeekActual + int.Parse(children.Value.ToString());
                                }
                                foreach (TotalObject hour in totalHoursWeekActual)
                                {
                                    totalHoursForWeekActual = totalHoursForWeekActual + double.Parse(hour.Value.ToString());
                                }

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = Math.Round(totalHoursForWeekActual, 2); xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 19;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalUnitsForWeekActual; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 19;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalBaseForWeekActual; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 19;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalPartnerForWeekActual; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 19;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalChildrenForWeekActual; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 19;
                                totalColIndex++;

                                double shActualTotal = 0;
                                if (totalHoursForWeekActual > 0 && totalBaseForWeekActual > 0)
                                {
                                    shActualTotal = Math.Round((totalBaseForWeekActual / totalHoursForWeekActual), 2);
                                }

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = shActualTotal; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 19;// S/H actual
                                totalColIndex++;
                            }
                        }
                        rowIndex++;


                    }


                }
                xlWorkBook.SaveAs(filePathAndName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                xlWorkBook.Close(true, misValue, misValue);

                xlApp.Quit();

                //Display excel document
                Process.Start(filePathAndName);
                #endregion report Data

            }
            catch (Exception ex)
            {
                reportDailySalesControl.HandleException(ex);
            }
            finally
            {
                reportDailySalesControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate { reportDailySalesControl.Cursor = Cursors.Arrow; });
            }
        }

        private void MaccBaseReport(BaseControl reportDailySalesControl, DataView agentsList, int? selectedReportMode, object sender)
        {
            try
            {
                reportDailySalesControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate { reportDailySalesControl.Cursor = Cursors.Wait; });

                DataTable dtLeadAllocationData;
                DataTable dtAgentHours;
                DataTable dtTsrTargets;
                DataTable dtSuperVisor;
                string AgentIDs = string.Empty;
                bool first = true;
                if (agentsList != null)
                {
                    foreach (DataRowView agent in agentsList)
                    {
                        if (first == true)
                        {
                            first = false;
                            if (agent["ID"] != null)
                            {
                                AgentIDs = Convert.ToInt32(agent["ID"].ToString()) + ",";
                            }
                        }
                        else
                        {
                            if (agent["ID"] != null)
                            {
                                AgentIDs = AgentIDs + Convert.ToInt32(agent["ID"].ToString()) + ",";
                            }
                        }
                    }
                }

                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@AgentIDs", AgentIDs);
                parameters[1] = new SqlParameter("@FromDate", StartDate.Value.ToString("yyy-MM-dd"));
                parameters[2] = new SqlParameter("@ToDate", ToDate.Value.ToString("yyy-MM-dd"));
                parameters[3] = new SqlParameter("@ReportMode", selectedReportMode);

                DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spInReportDailySales", parameters, false);
                if (dsLeadAllocationData.Tables.Count > 0)
                {
                    dtLeadAllocationData = dsLeadAllocationData.Tables[0];
                    dtAgentHours = dsLeadAllocationData.Tables[1];
                    dtTsrTargets = dsLeadAllocationData.Tables[2];
                    dtSuperVisor = dsLeadAllocationData.Tables[3];
                    if (dtLeadAllocationData.Rows.Count == 0)
                    {
                        reportDailySalesControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            reportDailySalesControl.ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected Agent(s)", "No Data", ShowMessageType.Information);
                        });

                        return;
                    }
                }
                else
                {
                    reportDailySalesControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        reportDailySalesControl.ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected Agent(s) ", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                //load campaignTypes (we already know this will return only upgrade campaign groups therefore we only need to determine the type)
                Dictionary<string, long> campaigntypes = new Dictionary<string, long>();
                foreach (DataRow rw in dtLeadAllocationData.Rows)
                {
                    if (!campaigntypes.ContainsValue(int.Parse(rw["CampaignTypeID"].ToString())))
                    {
                        campaigntypes.Add(rw["CampaignType"].ToString(), int.Parse(rw["CampaignTypeID"].ToString()));

                    }
                }
                //re-iterate by campaign type


                #region report Data
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                xlApp = new Excel.Application();
                object misValue = System.Reflection.Missing.Value;
                xlWorkBook = xlApp.Workbooks.Add(misValue);

                string filePathAndName = GlobalSettings.UserFolder + " Daily Sales  Report ~ " + Guid.NewGuid() + ".xls";


                int campaignTypeRow = 0;
                foreach (KeyValuePair<string, long> campaignType in campaigntypes)
                {
                    campaignTypeRow++;
                    Dictionary<object, int> returnObject = new Dictionary<object, int>();
                    returnObject.Add(campaigntypes, 1);//the 1 indicates that its for the first ptogress bar
                    (sender as BackgroundWorker).ReportProgress(campaignTypeRow, returnObject);

                    Excel.Worksheet xlWorkSheet;
                    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Add();  //.Worksheets.get_Item(1);
                    xlWorkSheet.Name = campaignType.Key + " Base";

                    #region Headings
                    System.Globalization.DateTimeFormatInfo dtf = new System.Globalization.DateTimeFormatInfo();
                    xlWorkSheet.Cells[1, 1] = dtf.GetMonthName(StartDate.Value.Month) + " " + StartDate.Value.Year;
                    Excel.Range c1 = xlWorkSheet.Cells[1, 1];
                    Excel.Range c2 = xlWorkSheet.Cells[1, 2];
                    Excel.Range range = (Excel.Range)xlWorkSheet.get_Range(c1, c2);
                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    range.Font.Bold = true;
                    range.Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    range.Merge(true);

                    //Dates (get an array of dates between from and to date)

                    int numberOFDaysBetween = int.Parse(ToDate.Value.Subtract(StartDate.Value).TotalDays.ToString());
                    List<DateTime> dates = new List<DateTime>();
                    for (int i = 0; i <= numberOFDaysBetween; i++)
                    {
                        DateTime newDate = StartDate.Value.AddDays(i);
                        if (newDate.DayOfWeek != DayOfWeek.Sunday)
                        {
                            dates.Add(newDate);
                        }
                    }

                    int DateColIndex = 3;
                    xlWorkSheet.Cells[2, 2] = campaignType.Key + " Base"; xlWorkSheet.Cells[2, 2].Font.Bold = true; xlWorkSheet.Cells[2, 2].ColumnWidth = 20; xlWorkSheet.Cells[2, 2].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    xlWorkSheet.Cells[3, 1] = "SUP"; xlWorkSheet.Cells[3, 1].ColumnWidth = 5; xlWorkSheet.Cells[3, 1].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    xlWorkSheet.Cells[3, 2] = "Sales Consultant"; xlWorkSheet.Cells[3, 2].ColumnWidth = 20; xlWorkSheet.Cells[3, 2].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    xlWorkSheet.Cells[4, 1].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, 2].Borders.Weight = Excel.XlBorderWeight.xlThin;

                    foreach (DateTime date in dates)
                    {
                        int weekindex = GetWeekOfMonth(date);

                        xlWorkSheet.Cells[2, DateColIndex] = dtf.GetDayName(date.DayOfWeek);
                        xlWorkSheet.Cells[3, DateColIndex] = date.Day + "-" + dtf.GetAbbreviatedMonthName(date.Month);

                        Excel.Range f1 = xlWorkSheet.Cells[2, DateColIndex];
                        Excel.Range f2 = xlWorkSheet.Cells[3, DateColIndex + 3];
                        Excel.Range range4 = (Excel.Range)xlWorkSheet.get_Range(f1, f2);
                        range4.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        //range4.Font.Bold = true;
                        range4.Borders.Weight = Excel.XlBorderWeight.xlThin;
                        range4.Merge(true);

                        xlWorkSheet.Cells[4, DateColIndex] = "Hours"; xlWorkSheet.Cells[4, DateColIndex].Borders.Weight = Excel.XlBorderWeight.xlThin;
                        xlWorkSheet.Cells[4, DateColIndex + 1] = "Premium"; xlWorkSheet.Cells[4, DateColIndex + 1].Borders.Weight = Excel.XlBorderWeight.xlThin;
                        xlWorkSheet.Cells[4, DateColIndex + 2] = "Base"; xlWorkSheet.Cells[4, DateColIndex + 2].Borders.Weight = Excel.XlBorderWeight.xlThin;
                        xlWorkSheet.Cells[4, DateColIndex + 3] = "Children"; xlWorkSheet.Cells[4, DateColIndex + 3].Borders.Weight = Excel.XlBorderWeight.xlThin;

                        DateColIndex++; DateColIndex++; DateColIndex++; DateColIndex++;
                        if (date.DayOfWeek == DayOfWeek.Saturday || date == dates[dates.Count - 1])
                        {

                            int interiorColorIndex = 34;//interior color index for this grouping
                            //week target********************************************************************************************
                            xlWorkSheet.Cells[2, DateColIndex] = "WEEK TARGET";
                            xlWorkSheet.Cells[3, DateColIndex] = "Week " + weekindex; xlWorkSheet.Cells[3, DateColIndex].Interior.ColorIndex = interiorColorIndex;
                            Excel.Range d1 = xlWorkSheet.Cells[2, DateColIndex];
                            Excel.Range d2 = xlWorkSheet.Cells[2, DateColIndex + 3];
                            Excel.Range range2 = (Excel.Range)xlWorkSheet.get_Range(d1, d2);  //range2.Interior.ColorIndex = interiorColorIndex;
                            range2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range2.Font.Bold = true;
                            range2.Borders.Weight = Excel.XlBorderWeight.xlThin;
                            range2.Merge(true);


                            Excel.Range e1 = xlWorkSheet.Cells[3, DateColIndex];
                            Excel.Range e2 = xlWorkSheet.Cells[2, DateColIndex + 3];
                            Excel.Range range3 = (Excel.Range)xlWorkSheet.get_Range(e1, e2); range3.Interior.ColorIndex = interiorColorIndex;
                            range3.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range3.Font.Bold = true;
                            range3.Borders.Weight = Excel.XlBorderWeight.xlThin;
                            range3.Merge(true);

                            xlWorkSheet.Cells[4, DateColIndex] = "Hours"; xlWorkSheet.Cells[4, DateColIndex].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex].Interior.ColorIndex = interiorColorIndex; xlWorkSheet.Cells[4, DateColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 1] = "Premium"; xlWorkSheet.Cells[4, DateColIndex + 1].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 1].Interior.ColorIndex = interiorColorIndex; xlWorkSheet.Cells[4, DateColIndex + 1].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 2] = "Base"; xlWorkSheet.Cells[4, DateColIndex + 2].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 2].Interior.ColorIndex = interiorColorIndex; xlWorkSheet.Cells[4, DateColIndex + 2].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 3] = "Children"; xlWorkSheet.Cells[4, DateColIndex + 3].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 3].Interior.ColorIndex = interiorColorIndex; xlWorkSheet.Cells[4, DateColIndex + 3].Font.Bold = true;

                            DateColIndex++; DateColIndex++; DateColIndex++; DateColIndex++;
                            //week target*********************************************************************************************

                            //week actual******************************************************************************************
                            int interiorColorIndex2 = 19;
                            xlWorkSheet.Cells[2, DateColIndex] = "WEEK ACTUAL";
                            xlWorkSheet.Cells[3, DateColIndex] = "Week " + weekindex;
                            Excel.Range g1 = xlWorkSheet.Cells[2, DateColIndex];
                            Excel.Range g2 = xlWorkSheet.Cells[3, DateColIndex + 4];
                            Excel.Range range5 = (Excel.Range)xlWorkSheet.get_Range(g1, g2); range5.Interior.ColorIndex = interiorColorIndex2;
                            range5.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range5.Font.Bold = true;
                            range5.Borders.Weight = Excel.XlBorderWeight.xlThin;
                            range5.Merge(true);

                            Excel.Range h1 = xlWorkSheet.Cells[3, DateColIndex];
                            Excel.Range h2 = xlWorkSheet.Cells[2, DateColIndex + 4];
                            Excel.Range range6 = (Excel.Range)xlWorkSheet.get_Range(e1, e2); //range6.Interior.ColorIndex = interiorColorIndex2;
                            range6.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range6.Font.Bold = true;
                            range6.Borders.Weight = Excel.XlBorderWeight.xlThin;
                            range6.Merge(true);

                            xlWorkSheet.Cells[4, DateColIndex] = "Hours"; xlWorkSheet.Cells[4, DateColIndex].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex].Interior.ColorIndex = interiorColorIndex2; xlWorkSheet.Cells[4, DateColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 1] = "Premium"; xlWorkSheet.Cells[4, DateColIndex + 1].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 1].Interior.ColorIndex = interiorColorIndex2; xlWorkSheet.Cells[4, DateColIndex + 1].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 2] = "Base"; xlWorkSheet.Cells[4, DateColIndex + 2].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 2].Interior.ColorIndex = interiorColorIndex2; xlWorkSheet.Cells[4, DateColIndex + 2].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 3] = "Children"; xlWorkSheet.Cells[4, DateColIndex + 3].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 3].Interior.ColorIndex = interiorColorIndex2; xlWorkSheet.Cells[4, DateColIndex + 3].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 4] = "S/H"; xlWorkSheet.Cells[4, DateColIndex + 4].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 4].Interior.ColorIndex = interiorColorIndex2; xlWorkSheet.Cells[4, DateColIndex + 4].Font.Bold = true;

                            DateColIndex++; DateColIndex++; DateColIndex++; DateColIndex++; DateColIndex++;
                            //Week actual******************************************************************************************

                        }

                    }

                    #endregion Headings


                    //re-iterate by campaign codes **get current  campaign codes for campaign type
                    var dtCampaignsForType = dtLeadAllocationData.AsEnumerable().Where(x => (long)x["CampaignTypeID"] == campaignType.Value);
                    Dictionary<string, long> campaigns = new Dictionary<string, long>();

                    foreach (DataRow rw in dtCampaignsForType)
                    {
                        if (!campaigns.ContainsValue(long.Parse(rw["CampaignID"].ToString())))
                        {
                            campaigns.Add(rw["CampaignCode"].ToString(), long.Parse(rw["CampaignID"].ToString()));
                        }
                    }
                    //***************************////////////////////*****************
                    int rowIndex = 5;
                    int campaignRow = 0;
                    foreach (KeyValuePair<string, long> campaign in campaigns)
                    {
                        campaignRow++;
                        Dictionary<object, int> returnObjectCampaign = new Dictionary<object, int>();
                        returnObjectCampaign.Add(campaigns, 2);//the 2 indicates that its for the second ptogress bar
                        (sender as BackgroundWorker).ReportProgress(campaignRow, returnObjectCampaign);

                        List<TotalObject> HoursDayTotals = new List<TotalObject>();
                        List<TotalObject> PremiumDayTotals = new List<TotalObject>();
                        List<TotalObject> BaseDayTotals = new List<TotalObject>();
                        List<TotalObject> ChildrenDayTotals = new List<TotalObject>();

                        //week target
                        List<TotalObject> HoursWeekTotals = new List<TotalObject>();
                        List<TotalObject> PremiumWeekTotals = new List<TotalObject>();
                        List<TotalObject> BaseWeekTotals = new List<TotalObject>();
                        List<TotalObject> ChildrenWeekTotals = new List<TotalObject>();
                        //week actual
                        List<TotalObject> HoursWeekActualTotals = new List<TotalObject>();
                        List<TotalObject> PremiumWeekActualTotals = new List<TotalObject>();
                        List<TotalObject> BaseWeekActualTotals = new List<TotalObject>();
                        List<TotalObject> ChildrenWeekActualTotals = new List<TotalObject>();

                        xlWorkSheet.Cells[rowIndex, 1] = campaign.Key; xlWorkSheet.Cells[rowIndex, 1].Font.Bold = true;
                        for (int i = 1; i <= DateColIndex - 1; i++)//Orange formatting
                        {
                            xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 46;
                            xlWorkSheet.Cells[rowIndex, i].Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlMedium;
                            xlWorkSheet.Cells[rowIndex, i].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;
                        }
                        rowIndex++;
                        var campaignData = dtCampaignsForType.AsEnumerable().Where(x => (long)x["CampaignID"] == campaign.Value);
                        Dictionary<string, long> agentsForCampaign = new Dictionary<string, long>();

                        foreach (DataRow rw in campaignData)
                        {

                            if (!agentsForCampaign.ContainsValue(long.Parse(rw["FKUserID"].ToString())))
                            {
                                agentsForCampaign.Add(rw["Agent"].ToString(), long.Parse(rw["FKUserID"].ToString()));

                                var supervisor = dtSuperVisor.AsEnumerable().Where(x => (long)x["FKUserID"] == long.Parse(rw["FKUserID"].ToString()));
                                string superVisorName = string.Empty;
                                bool firstTime = true;
                                foreach (DataRow sup in supervisor)
                                {
                                    if (firstTime == true)
                                    {
                                        firstTime = false;
                                        superVisorName = sup["SuperVisorCode"].ToString();
                                    }
                                    else
                                    {
                                        superVisorName = superVisorName + "," + sup["SuperVisorCode"].ToString();
                                    }

                                }
                                xlWorkSheet.Cells[rowIndex, 2] = rw["Agent"].ToString();
                                xlWorkSheet.Cells[rowIndex, 1] = superVisorName; xlWorkSheet.Cells[rowIndex, 1].Font.Bold = true;
                                int resultColumn = 3;
                                //now we want to re-iterate this based on column first


                                double totalPremiumForWeek = 0;
                                double totalBaseForWeek = 0;
                                int totalChildrenForWeek = 0;
                                double totalHoursForWeek = 0;
                                foreach (DateTime resultDate in dates)
                                {
                                    int currentWeek = GetWeekOfMonth(resultDate);

                                    bool skippedRows = false;
                                    var resultsForAgent = campaignData.AsEnumerable().Where(x => (long)x["FKUserID"] == (long)rw["FKUserID"]);
                                    var resultsForAgentDate = resultsForAgent.AsEnumerable().Where(x => DateTime.Parse(x["DateOfSale"].ToString()).ToString("yyy-MM-dd") == resultDate.ToString("yyy-MM-dd"));
                                    if (resultsForAgentDate.Count() > 0)
                                    {
                                        //count figures for date
                                        double totalPremiumForday = 0;
                                        int childCountForDay = 0;
                                        int partnerCountForDay = 0;
                                        int baseCount = 0;
                                        double agentHoursForDay = 0;
                                        foreach (DataRow rwResult in resultsForAgentDate)
                                        {
                                            string dateOfSale = rwResult["DateOfSale"].ToString();
                                            DateTime DateOfSale = DateTime.Parse(dateOfSale);

                                            totalPremiumForday = totalPremiumForday + Double.Parse(rwResult["Premium"].ToString());

                                            if ((bool)rwResult["OptionChild"] == true)
                                            {
                                                childCountForDay++;
                                            }
                                            if ((bool)rwResult["OptionLA2"] == true)
                                            {
                                                partnerCountForDay++;
                                            }
                                            baseCount++;
                                        }
                                        //calculate hours for date
                                        var agentHours = dtAgentHours.AsEnumerable().Where(x => (long)x["FKUserID"] == (long)rw["FKUserID"] && DateTime.Parse(x["WorkingDate"].ToString()).ToString("yyy-MM-dd") == resultDate.ToString("yyy-MM-dd") && (long)x["FKINCampaignID"] == campaign.Value);

                                        if (agentHours.Count() > 0)
                                        {
                                            foreach (DataRow rwAgentHour in agentHours)
                                            {
                                                //calculate morning shift total hours
                                                if (rwAgentHour["MorningShiftStartTime"].ToString() != string.Empty && rwAgentHour["MorningShiftEndTime"].ToString() != string.Empty)
                                                {
                                                    TimeSpan morningShiftStart = (TimeSpan)rwAgentHour["MorningShiftStartTime"];
                                                    TimeSpan morningShiftEnd = (TimeSpan)rwAgentHour["MorningShiftEndTime"];
                                                    agentHoursForDay = agentHoursForDay + morningShiftEnd.Subtract(morningShiftStart).TotalHours;
                                                }
                                                //calculate normal shift total hours
                                                if (rwAgentHour["NormalShiftStartTime"].ToString() != string.Empty && rwAgentHour["NormalShiftEndTime"].ToString() != string.Empty)
                                                {
                                                    TimeSpan normalShiftStart = (TimeSpan)rwAgentHour["NormalShiftStartTime"];
                                                    TimeSpan normalShiftEnd = (TimeSpan)rwAgentHour["NormalShiftEndTime"];
                                                    agentHoursForDay = agentHoursForDay + normalShiftEnd.Subtract(normalShiftStart).TotalHours;
                                                }
                                                //calculate evening shift total hours
                                                if (rwAgentHour["EveningShiftStartTime"].ToString() != string.Empty && rwAgentHour["EveningShiftEndTime"].ToString() != string.Empty)
                                                {
                                                    TimeSpan eveningShiftStart = (TimeSpan)rwAgentHour["EveningShiftStartTime"];
                                                    TimeSpan eveningShiftEnd = (TimeSpan)rwAgentHour["EveningShiftEndTime"];
                                                    agentHoursForDay = agentHoursForDay + eveningShiftEnd.Subtract(eveningShiftStart).TotalHours;
                                                }
                                                //calculate weekend shift total hours
                                                if (rwAgentHour["PublicHolidayWeekendShiftStartTime"].ToString() != string.Empty && rwAgentHour["PublicHolidayWeekendShiftEndTime"].ToString() != string.Empty)
                                                {
                                                    TimeSpan weekendShiftStart = (TimeSpan)rwAgentHour["PublicHolidayWeekendShiftStartTime"];
                                                    TimeSpan weekendShiftEnd = (TimeSpan)rwAgentHour["PublicHolidayWeekendShiftEndTime"];
                                                    agentHoursForDay = agentHoursForDay + weekendShiftEnd.Subtract(weekendShiftStart).TotalHours;
                                                }
                                            }
                                        }
                                        //hours for date//
                                        xlWorkSheet.Cells[rowIndex, resultColumn] = Math.Round(agentHoursForDay, 2); xlWorkSheet.Cells[rowIndex, resultColumn].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                        totalHoursForWeek = totalHoursForWeek + agentHoursForDay;
                                        HoursDayTotals.Add(new TotalObject(resultDate, agentHoursForDay, campaign.Key));
                                        resultColumn++;
                                        //////////
                                        //units for date//
                                        xlWorkSheet.Cells[rowIndex, resultColumn] = totalPremiumForday; xlWorkSheet.Cells[rowIndex, resultColumn].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                        totalPremiumForWeek = totalPremiumForWeek + totalPremiumForday;

                                        PremiumDayTotals.Add(new TotalObject(resultDate, totalPremiumForday, campaign.Key));
                                        resultColumn++;
                                        ////
                                        ////base for date
                                        xlWorkSheet.Cells[rowIndex, resultColumn] = baseCount; xlWorkSheet.Cells[rowIndex, resultColumn].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                        totalBaseForWeek = totalBaseForWeek + baseCount;
                                        BaseDayTotals.Add(new TotalObject(resultDate, baseCount, campaign.Key));
                                        resultColumn++;
                                        ////////

                                        ////children for date
                                        xlWorkSheet.Cells[rowIndex, resultColumn] = childCountForDay; xlWorkSheet.Cells[rowIndex, resultColumn].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                        totalChildrenForWeek = totalChildrenForWeek + childCountForDay;
                                        ChildrenDayTotals.Add(new TotalObject(resultDate, childCountForDay, campaign.Key));
                                        resultColumn++;
                                        //////


                                        if (resultDate.DayOfWeek == DayOfWeek.Saturday || resultDate == dates[dates.Count - 1])
                                        {
                                            if (skippedRows == false)
                                            {

                                                double hoursTarget = 0;
                                                double premiumTarget = 0;
                                                double basetarget = 0;
                                                double childrentarget = 0;

                                                //week target
                                                //hours 
                                                var currentWeekData = dtTsrTargets.AsEnumerable().Where(x => (long)x["FKAgentID"] == (long)rw["FKUserID"] && (long)x["FKINCampaignID"] == campaign.Value && (long)x["FKINWeekID"] == (long)currentWeek);
                                                if (currentWeekData.Count() > 0)
                                                {
                                                    foreach (DataRow targetRow in currentWeekData)
                                                    {
                                                        hoursTarget = hoursTarget + double.Parse(targetRow["Hours"].ToString());
                                                        premiumTarget = premiumTarget + double.Parse(targetRow["PremiumTarget"].ToString());
                                                        basetarget = basetarget + double.Parse(targetRow["BaseTarget"].ToString());
                                                    }
                                                }
                                                xlWorkSheet.Cells[rowIndex, resultColumn] = hoursTarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                                HoursWeekTotals.Add(new TotalObject(resultDate, hoursTarget, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = premiumTarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                                PremiumWeekTotals.Add(new TotalObject(resultDate, premiumTarget, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = basetarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                                BaseWeekTotals.Add(new TotalObject(resultDate, basetarget, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = childrentarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                                ChildrenWeekTotals.Add(new TotalObject(resultDate, childrentarget, campaign.Key));
                                                resultColumn++;
                                                //week actual

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = Math.Round(totalHoursForWeek, 2); xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                                HoursWeekActualTotals.Add(new TotalObject(currentWeek, totalHoursForWeek, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = totalPremiumForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                                PremiumWeekActualTotals.Add(new TotalObject(currentWeek, totalPremiumForWeek, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = totalBaseForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                                BaseWeekActualTotals.Add(new TotalObject(currentWeek, totalBaseForWeek, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = totalChildrenForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                                ChildrenWeekActualTotals.Add(new TotalObject(currentWeek, totalChildrenForWeek, campaign.Key));
                                                resultColumn++;

                                                double shActual = 0;
                                                if (totalHoursForWeek > 0 && totalBaseForWeek > 0)
                                                {
                                                    shActual = Math.Round((totalBaseForWeek / totalHoursForWeek), 2);
                                                }
                                                xlWorkSheet.Cells[rowIndex, resultColumn] = shActual; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;//  S/H actual

                                                resultColumn++;
                                                skippedRows = true;
                                                totalPremiumForWeek = 0;
                                                totalBaseForWeek = 0;
                                                totalChildrenForWeek = 0;
                                                totalHoursForWeek = 0;
                                            }
                                        }

                                    }
                                    else
                                    {

                                        resultColumn++;
                                        resultColumn++;
                                        resultColumn++;
                                        resultColumn++;
                                    }

                                    if (resultDate.DayOfWeek == DayOfWeek.Saturday || resultDate == dates[dates.Count - 1])
                                    {
                                        if (skippedRows == false)
                                        {

                                            double hoursTarget = 0;
                                            double premiumTarget = 0;
                                            double basetarget = 0;
                                            double childrentarget = 0;

                                            //week target
                                            //hours 
                                            var currentWeekData = dtTsrTargets.AsEnumerable().Where(x => (long)x["FKAgentID"] == (long)rw["FKUserID"] && (long)x["FKINCampaignID"] == campaign.Value && (long)x["FKINWeekID"] == (long)currentWeek);
                                            if (currentWeekData.Count() > 0)
                                            {
                                                foreach (DataRow targetRow in currentWeekData)
                                                {
                                                    hoursTarget = hoursTarget + double.Parse(targetRow["Hours"].ToString());
                                                    premiumTarget = premiumTarget + double.Parse(targetRow["PremiumTarget"].ToString());
                                                    basetarget = basetarget + double.Parse(targetRow["BaseTarget"].ToString());
                                                }
                                            }
                                            xlWorkSheet.Cells[rowIndex, resultColumn] = hoursTarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                            HoursWeekTotals.Add(new TotalObject(resultDate, hoursTarget, campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = premiumTarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                            PremiumWeekTotals.Add(new TotalObject(resultDate, premiumTarget, campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = basetarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                            BaseWeekTotals.Add(new TotalObject(resultDate, basetarget, campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = childrentarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                            ChildrenWeekTotals.Add(new TotalObject(resultDate, childrentarget, campaign.Key));
                                            resultColumn++;
                                            //week actual

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = Math.Round(totalHoursForWeek, 2); xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                            HoursWeekActualTotals.Add(new TotalObject(currentWeek, Math.Round(totalHoursForWeek, 2), campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = totalPremiumForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                            PremiumWeekActualTotals.Add(new TotalObject(currentWeek, totalPremiumForWeek, campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = totalBaseForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                            BaseWeekActualTotals.Add(new TotalObject(currentWeek, totalBaseForWeek, campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = totalChildrenForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                            ChildrenWeekActualTotals.Add(new TotalObject(currentWeek, totalChildrenForWeek, campaign.Key));
                                            resultColumn++;

                                            double shActual = 0;
                                            if (totalHoursForWeek > 0 && totalBaseForWeek > 0)
                                            {
                                                shActual = Math.Round((totalBaseForWeek / totalHoursForWeek), 2);
                                            }
                                            xlWorkSheet.Cells[rowIndex, resultColumn] = shActual; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19; // S/H actual                                           
                                            resultColumn++;

                                            skippedRows = true;
                                            totalPremiumForWeek = 0;
                                            totalBaseForWeek = 0;
                                            totalChildrenForWeek = 0;
                                            totalHoursForWeek = 0;
                                        }
                                    }

                                }
                                //cell borders
                                for (int i = 1; i <= resultColumn - 1; i++)
                                {
                                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                }

                                rowIndex++;
                            }

                        }

                        //totals here///////////////////
                        xlWorkSheet.Cells[rowIndex, 2] = "TOTALS"; xlWorkSheet.Cells[rowIndex, 2].Font.Bold = true;
                        xlWorkSheet.Cells[rowIndex, 2].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                        int totalColIndex = 3;

                        foreach (DateTime totalDate in dates)
                        {
                            int currentWeekIndex = GetWeekOfMonth(totalDate);
                            var totalHours = (from h in HoursDayTotals where DateTime.Parse(h.Description.ToString()).Date == totalDate.Date select h);
                            var totalPremiums = (from u in PremiumDayTotals where DateTime.Parse(u.Description.ToString()).Date == totalDate.Date select u);
                            var totalBase = (from b in BaseDayTotals where DateTime.Parse(b.Description.ToString()).Date == totalDate.Date select b);
                            var totalChildren = (from c in ChildrenDayTotals where DateTime.Parse(c.Description.ToString()).Date == totalDate.Date select c);

                            double totalHoursForDay = 0;
                            double totalPremiumForDay = 0;
                            int totalBaseForDay = 0;

                            int totalChildrenForDay = 0;
                            foreach (TotalObject unit in totalPremiums)
                            {
                                totalPremiumForDay = totalPremiumForDay + double.Parse(unit.Value.ToString());
                            }
                            foreach (TotalObject baseTarg in totalBase)
                            {
                                totalBaseForDay = totalBaseForDay + int.Parse(baseTarg.Value.ToString());
                            }
                            foreach (TotalObject children in totalChildren)
                            {
                                totalChildrenForDay = totalChildrenForDay + int.Parse(children.Value.ToString());
                            }
                            foreach (TotalObject hour in totalHours)
                            {
                                totalHoursForDay = totalHoursForDay + double.Parse(hour.Value.ToString());
                            }
                            xlWorkSheet.Cells[rowIndex, totalColIndex] = Math.Round(totalHoursForDay, 2); xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            totalColIndex++;


                            xlWorkSheet.Cells[rowIndex, totalColIndex] = totalPremiumForDay; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            totalColIndex++;

                            xlWorkSheet.Cells[rowIndex, totalColIndex] = totalBaseForDay; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            totalColIndex++;

                            xlWorkSheet.Cells[rowIndex, totalColIndex] = totalChildrenForDay; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            totalColIndex++;


                            if (totalDate.Date.DayOfWeek == DayOfWeek.Saturday || totalDate == dates[dates.Count - 1])
                            {

                                var totalHoursWeek = (from h in HoursWeekTotals where DateTime.Parse(h.Description.ToString()).Date == totalDate.Date select h);
                                var totalPremiumWeek = (from u in PremiumWeekTotals where DateTime.Parse(u.Description.ToString()).Date == totalDate.Date select u);
                                var totalBaseWeek = (from b in BaseWeekTotals where DateTime.Parse(b.Description.ToString()).Date == totalDate.Date select b);
                                var totalChildrenWeek = (from c in ChildrenWeekTotals where DateTime.Parse(c.Description.ToString()).Date == totalDate.Date select c);

                                double totalHoursForWeek = 0;
                                double totalUnitsForWeek = 0;
                                int totalBaseForWeek = 0;
                                int totalChildrenForWeek = 0;
                                foreach (TotalObject unit in totalPremiumWeek)
                                {
                                    totalUnitsForWeek = totalUnitsForWeek + double.Parse(unit.Value.ToString());
                                }
                                foreach (TotalObject baseTarg in totalBaseWeek)
                                {
                                    totalBaseForWeek = totalBaseForWeek + int.Parse(baseTarg.Value.ToString());
                                }
                                foreach (TotalObject children in totalChildrenWeek)
                                {
                                    totalChildrenForWeek = totalChildrenForWeek + int.Parse(children.Value.ToString());
                                }
                                foreach (TotalObject hour in totalHoursWeek)
                                {
                                    totalHoursForWeek = totalHoursForWeek + double.Parse(hour.Value.ToString());
                                }

                                //week target
                                xlWorkSheet.Cells[rowIndex, totalColIndex] = Math.Round(totalHoursForWeek, 2); xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 34;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalUnitsForWeek; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 34;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalBaseForWeek; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 34;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalChildrenForWeek; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 34;
                                totalColIndex++;

                                //week actual
                                var totalHoursWeekActual = (from h in HoursWeekActualTotals where int.Parse(h.Description.ToString()) == currentWeekIndex select h);
                                var totalPremiumWeekActual = (from u in PremiumWeekActualTotals where int.Parse(u.Description.ToString()) == currentWeekIndex select u);
                                var totalBaseWeekActual = (from b in BaseWeekActualTotals where int.Parse(b.Description.ToString()) == currentWeekIndex select b);
                                var totalChildrenWeekActual = (from c in ChildrenWeekActualTotals where int.Parse(c.Description.ToString()) == currentWeekIndex select c);

                                double totalHoursForWeekActual = 0;
                                double totalUnitsForWeekActual = 0;
                                int totalBaseForWeekActual = 0;
                                int totalChildrenForWeekActual = 0;
                                foreach (TotalObject unit in totalPremiumWeekActual)
                                {
                                    totalUnitsForWeekActual = totalUnitsForWeekActual + double.Parse(unit.Value.ToString());
                                }
                                foreach (TotalObject baseTarg in totalBaseWeekActual)
                                {
                                    totalBaseForWeekActual = totalBaseForWeekActual + int.Parse(baseTarg.Value.ToString());
                                }
                                foreach (TotalObject children in totalChildrenWeekActual)
                                {
                                    totalChildrenForWeekActual = totalChildrenForWeekActual + int.Parse(children.Value.ToString());
                                }
                                foreach (TotalObject hour in totalHoursWeekActual)
                                {
                                    totalHoursForWeekActual = totalHoursForWeekActual + double.Parse(hour.Value.ToString());
                                }

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = Math.Round(totalHoursForWeekActual, 2); xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 19;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalUnitsForWeekActual; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 19;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalBaseForWeekActual; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 19;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalChildrenForWeekActual; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 19;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = "0"; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 19;// S/H actual
                                totalColIndex++;
                            }
                        }
                        rowIndex++;


                    }


                }
                xlWorkBook.SaveAs(filePathAndName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                xlWorkBook.Close(true, misValue, misValue);

                xlApp.Quit();

                //Display excel document
                Process.Start(filePathAndName);
                #endregion report Data




            }
            catch (Exception ex)
            {
                reportDailySalesControl.HandleException(ex);
            }
            finally
            {
                reportDailySalesControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate { reportDailySalesControl.Cursor = Cursors.Arrow; });
            }
        }
        private void UpgradeReport(BaseControl reportDailySalesControl, DataView agentsList, int? selectedReportMode, object sender)
        {
            try
            {
                reportDailySalesControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate { reportDailySalesControl.Cursor = Cursors.Wait; });

                DataTable dtLeadAllocationData;
                DataTable dtAgentHours;
                DataTable dtTsrTargets;
                DataTable dtSuperVisor;
                string AgentIDs = string.Empty;
                bool first = true;
                if (agentsList != null)
                {
                    foreach (DataRowView agent in agentsList)
                    {
                        if (first == true)
                        {
                            first = false;
                            AgentIDs = Convert.ToInt32(agent["ID"].ToString()) + ",";
                        }
                        else
                        {
                            if (agent["ID"] != null)
                            {
                                AgentIDs = AgentIDs + Convert.ToInt32(agent["ID"].ToString()) + ",";
                            }
                        }
                    }
                }

                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@AgentIDs", AgentIDs);
                parameters[1] = new SqlParameter("@FromDate", StartDate.Value.ToString("yyy-MM-dd"));
                parameters[2] = new SqlParameter("@ToDate", ToDate.Value.ToString("yyy-MM-dd"));
                parameters[3] = new SqlParameter("@ReportMode", selectedReportMode);

                DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spInReportDailySales", parameters, false);
                if (dsLeadAllocationData.Tables.Count > 0)
                {
                    dtLeadAllocationData = dsLeadAllocationData.Tables[0];
                    dtAgentHours = dsLeadAllocationData.Tables[1];
                    dtTsrTargets = dsLeadAllocationData.Tables[2];
                    dtSuperVisor = dsLeadAllocationData.Tables[3];
                    if (dtLeadAllocationData.Rows.Count == 0)
                    {
                        reportDailySalesControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            reportDailySalesControl.ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected Agent(s)", "No Data", ShowMessageType.Information);
                        });

                        return;
                    }
                }
                else
                {
                    reportDailySalesControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        reportDailySalesControl.ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected Agent(s) ", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                //load campaignTypes (we already know this will return only upgrade campaign groups therefore we only need to determine the type)
                Dictionary<string, long> campaigntypes = new Dictionary<string, long>();
                foreach (DataRow rw in dtLeadAllocationData.Rows)
                {
                    if (!campaigntypes.ContainsValue(int.Parse(rw["CampaignTypeID"].ToString())))
                    {
                        campaigntypes.Add(rw["CampaignType"].ToString(), int.Parse(rw["CampaignTypeID"].ToString()));

                    }
                }
                //re-iterate by campaign type


                #region report Data
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                xlApp = new Excel.Application();
                object misValue = System.Reflection.Missing.Value;
                xlWorkBook = xlApp.Workbooks.Add(misValue);

                string filePathAndName = GlobalSettings.UserFolder + " Daily Sales  Report ~ " + Guid.NewGuid() + ".xls";


                int campaignTypeRow = 0;
                foreach (KeyValuePair<string, long> campaignType in campaigntypes)
                {
                    campaignTypeRow++;
                    Dictionary<object, int> returnObject = new Dictionary<object, int>();
                    returnObject.Add(campaigntypes, 1);//the 1 indicates that its for the first ptogress bar
                    (sender as BackgroundWorker).ReportProgress(campaignTypeRow, returnObject);

                    Excel.Worksheet xlWorkSheet;
                    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Add();  //.Worksheets.get_Item(1);
                    xlWorkSheet.Name = campaignType.Key + " Upgrades";

                    #region Headings
                    System.Globalization.DateTimeFormatInfo dtf = new System.Globalization.DateTimeFormatInfo();
                    xlWorkSheet.Cells[1, 1] = dtf.GetMonthName(StartDate.Value.Month) + " " + StartDate.Value.Year;
                    Excel.Range c1 = xlWorkSheet.Cells[1, 1];
                    Excel.Range c2 = xlWorkSheet.Cells[1, 2];
                    Excel.Range range = (Excel.Range)xlWorkSheet.get_Range(c1, c2);
                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    range.Font.Bold = true;
                    range.Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    range.Merge(true);

                    //Dates (get an array of dates between from and to date)

                    int numberOFDaysBetween = int.Parse(ToDate.Value.Subtract(StartDate.Value).TotalDays.ToString());
                    List<DateTime> dates = new List<DateTime>();
                    for (int i = 0; i <= numberOFDaysBetween; i++)
                    {
                        DateTime newDate = StartDate.Value.AddDays(i);
                        if (newDate.DayOfWeek != DayOfWeek.Sunday)
                        {
                            dates.Add(newDate);
                        }
                    }

                    int DateColIndex = 3;
                    xlWorkSheet.Cells[2, 2] = campaignType.Key + " Upgrades"; xlWorkSheet.Cells[2, 2].Font.Bold = true; xlWorkSheet.Cells[2, 2].ColumnWidth = 20; xlWorkSheet.Cells[2, 2].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    xlWorkSheet.Cells[3, 1] = "SUP"; xlWorkSheet.Cells[3, 1].ColumnWidth = 5; xlWorkSheet.Cells[3, 1].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    xlWorkSheet.Cells[3, 2] = "Sales Consultant"; xlWorkSheet.Cells[3, 2].ColumnWidth = 20; xlWorkSheet.Cells[3, 2].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    xlWorkSheet.Cells[4, 1].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, 2].Borders.Weight = Excel.XlBorderWeight.xlThin;



                    foreach (DateTime date in dates)
                    {
                        int weekindex = GetWeekOfMonth(date);

                        xlWorkSheet.Cells[2, DateColIndex] = dtf.GetDayName(date.DayOfWeek);
                        xlWorkSheet.Cells[3, DateColIndex] = date.Day + "-" + dtf.GetAbbreviatedMonthName(date.Month);

                        Excel.Range f1 = xlWorkSheet.Cells[2, DateColIndex];
                        Excel.Range f2 = xlWorkSheet.Cells[3, DateColIndex + 3];
                        Excel.Range range4 = (Excel.Range)xlWorkSheet.get_Range(f1, f2);
                        range4.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        //range4.Font.Bold = true;
                        range4.Borders.Weight = Excel.XlBorderWeight.xlThin;
                        range4.Merge(true);

                        xlWorkSheet.Cells[4, DateColIndex] = "Hours"; xlWorkSheet.Cells[4, DateColIndex].Borders.Weight = Excel.XlBorderWeight.xlThin;
                        xlWorkSheet.Cells[4, DateColIndex + 1] = "Units"; xlWorkSheet.Cells[4, DateColIndex + 1].Borders.Weight = Excel.XlBorderWeight.xlThin;
                        xlWorkSheet.Cells[4, DateColIndex + 2] = "Base"; xlWorkSheet.Cells[4, DateColIndex + 2].Borders.Weight = Excel.XlBorderWeight.xlThin;
                        xlWorkSheet.Cells[4, DateColIndex + 3] = "Children"; xlWorkSheet.Cells[4, DateColIndex + 3].Borders.Weight = Excel.XlBorderWeight.xlThin;



                        DateColIndex++; DateColIndex++; DateColIndex++; DateColIndex++;
                        if (date.DayOfWeek == DayOfWeek.Saturday || date == dates[dates.Count - 1])
                        {

                            int interiorColorIndex = 34;//interior color index for this grouping
                            //week target********************************************************************************************
                            xlWorkSheet.Cells[2, DateColIndex] = "WEEK TARGET";
                            xlWorkSheet.Cells[3, DateColIndex] = "Week " + weekindex; xlWorkSheet.Cells[3, DateColIndex].Interior.ColorIndex = interiorColorIndex;
                            Excel.Range d1 = xlWorkSheet.Cells[2, DateColIndex];
                            Excel.Range d2 = xlWorkSheet.Cells[2, DateColIndex + 3];
                            Excel.Range range2 = (Excel.Range)xlWorkSheet.get_Range(d1, d2);  //range2.Interior.ColorIndex = interiorColorIndex;
                            range2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range2.Font.Bold = true;
                            range2.Borders.Weight = Excel.XlBorderWeight.xlThin;
                            range2.Merge(true);


                            Excel.Range e1 = xlWorkSheet.Cells[3, DateColIndex];
                            Excel.Range e2 = xlWorkSheet.Cells[2, DateColIndex + 3];
                            Excel.Range range3 = (Excel.Range)xlWorkSheet.get_Range(e1, e2); range3.Interior.ColorIndex = interiorColorIndex;
                            range3.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range3.Font.Bold = true;
                            range3.Borders.Weight = Excel.XlBorderWeight.xlThin;
                            range3.Merge(true);

                            xlWorkSheet.Cells[4, DateColIndex] = "Hours"; xlWorkSheet.Cells[4, DateColIndex].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex].Interior.ColorIndex = interiorColorIndex; xlWorkSheet.Cells[4, DateColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 1] = "Units"; xlWorkSheet.Cells[4, DateColIndex + 1].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 1].Interior.ColorIndex = interiorColorIndex; xlWorkSheet.Cells[4, DateColIndex + 1].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 2] = "Base"; xlWorkSheet.Cells[4, DateColIndex + 2].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 2].Interior.ColorIndex = interiorColorIndex; xlWorkSheet.Cells[4, DateColIndex + 2].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 3] = "Children"; xlWorkSheet.Cells[4, DateColIndex + 3].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 3].Interior.ColorIndex = interiorColorIndex; xlWorkSheet.Cells[4, DateColIndex + 3].Font.Bold = true;
                            DateColIndex++; DateColIndex++; DateColIndex++; DateColIndex++;
                            //week target*********************************************************************************************

                            //week actual******************************************************************************************
                            int interiorColorIndex2 = 19;
                            xlWorkSheet.Cells[2, DateColIndex] = "WEEK ACTUAL";
                            xlWorkSheet.Cells[3, DateColIndex] = "Week " + weekindex;
                            Excel.Range g1 = xlWorkSheet.Cells[2, DateColIndex];
                            Excel.Range g2 = xlWorkSheet.Cells[3, DateColIndex + 3];
                            Excel.Range range5 = (Excel.Range)xlWorkSheet.get_Range(g1, g2); range5.Interior.ColorIndex = interiorColorIndex2;
                            range5.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range5.Font.Bold = true;
                            range5.Borders.Weight = Excel.XlBorderWeight.xlThin;
                            range5.Merge(true);

                            Excel.Range h1 = xlWorkSheet.Cells[3, DateColIndex];
                            Excel.Range h2 = xlWorkSheet.Cells[2, DateColIndex + 3];
                            Excel.Range range6 = (Excel.Range)xlWorkSheet.get_Range(e1, e2); //range6.Interior.ColorIndex = interiorColorIndex2;
                            range6.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range6.Font.Bold = true;
                            range6.Borders.Weight = Excel.XlBorderWeight.xlThin;
                            range6.Merge(true);

                            xlWorkSheet.Cells[4, DateColIndex] = "Hours"; xlWorkSheet.Cells[4, DateColIndex].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex].Interior.ColorIndex = interiorColorIndex2; xlWorkSheet.Cells[4, DateColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 1] = "Units"; xlWorkSheet.Cells[4, DateColIndex + 1].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 1].Interior.ColorIndex = interiorColorIndex2; xlWorkSheet.Cells[4, DateColIndex + 1].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 2] = "Base"; xlWorkSheet.Cells[4, DateColIndex + 2].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 2].Interior.ColorIndex = interiorColorIndex2; xlWorkSheet.Cells[4, DateColIndex + 2].Font.Bold = true;
                            xlWorkSheet.Cells[4, DateColIndex + 3] = "Children"; xlWorkSheet.Cells[4, DateColIndex + 3].Borders.Weight = Excel.XlBorderWeight.xlThin; xlWorkSheet.Cells[4, DateColIndex + 3].Interior.ColorIndex = interiorColorIndex2; xlWorkSheet.Cells[4, DateColIndex + 3].Font.Bold = true;
                            DateColIndex++; DateColIndex++; DateColIndex++; DateColIndex++;
                            //Week actual******************************************************************************************

                        }

                    }

                    #endregion Headings


                    //re-iterate by campaign codes **get current  campaign codes for campaign type
                    var dtCampaignsForType = dtLeadAllocationData.AsEnumerable().Where(x => (long)x["CampaignTypeID"] == campaignType.Value);
                    Dictionary<string, long> campaigns = new Dictionary<string, long>();

                    foreach (DataRow rw in dtCampaignsForType)
                    {
                        if (!campaigns.ContainsValue(long.Parse(rw["CampaignID"].ToString())))
                        {
                            campaigns.Add(rw["CampaignCode"].ToString(), long.Parse(rw["CampaignID"].ToString()));
                        }
                    }
                    //***************************////////////////////*****************
                    int rowIndex = 5;
                    int campaignRow = 0;
                    foreach (KeyValuePair<string, long> campaign in campaigns)
                    {
                        campaignRow++;
                        Dictionary<object, int> returnObjectCampaign = new Dictionary<object, int>();
                        returnObjectCampaign.Add(campaigns, 2);//the 2 indicates that its for the second ptogress bar
                        (sender as BackgroundWorker).ReportProgress(campaignRow, returnObjectCampaign);

                        List<TotalObject> HoursDayTotals = new List<TotalObject>();
                        List<TotalObject> UnitDayTotals = new List<TotalObject>();
                        List<TotalObject> BaseDayTotals = new List<TotalObject>();
                        List<TotalObject> ChildrenDayTotals = new List<TotalObject>();

                        //week target
                        List<TotalObject> HoursWeekTotals = new List<TotalObject>();
                        List<TotalObject> UnitWeekTotals = new List<TotalObject>();
                        List<TotalObject> BaseWeekTotals = new List<TotalObject>();
                        List<TotalObject> ChildrenWeekTotals = new List<TotalObject>();
                        //week actual
                        List<TotalObject> HoursWeekActualTotals = new List<TotalObject>();
                        List<TotalObject> UnitWeekActualTotals = new List<TotalObject>();
                        List<TotalObject> BaseWeekActualTotals = new List<TotalObject>();
                        List<TotalObject> ChildrenWeekActualTotals = new List<TotalObject>();

                        xlWorkSheet.Cells[rowIndex, 1] = campaign.Key; xlWorkSheet.Cells[rowIndex, 1].Font.Bold = true;
                        for (int i = 1; i <= DateColIndex - 1; i++)//yellow formatting
                        {
                            xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 27;
                            xlWorkSheet.Cells[rowIndex, i].Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlMedium;
                            xlWorkSheet.Cells[rowIndex, i].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;
                        }
                        rowIndex++;
                        var campaignData = dtCampaignsForType.AsEnumerable().Where(x => (long)x["CampaignID"] == campaign.Value);
                        Dictionary<string, long> agentsForCampaign = new Dictionary<string, long>();

                        foreach (DataRow rw in campaignData)
                        {

                            if (!agentsForCampaign.ContainsValue(long.Parse(rw["FKUserID"].ToString())))
                            {
                                agentsForCampaign.Add(rw["Agent"].ToString(), long.Parse(rw["FKUserID"].ToString()));

                                var supervisor = dtSuperVisor.AsEnumerable().Where(x => (long)x["FKUserID"] == long.Parse(rw["FKUserID"].ToString()));
                                string superVisorName = string.Empty;
                                bool firstTime = true;
                                foreach (DataRow sup in supervisor)
                                {
                                    if (firstTime == true)
                                    {
                                        firstTime = false;
                                        superVisorName = sup["SuperVisorCode"].ToString();
                                    }
                                    else
                                    {
                                        superVisorName = superVisorName + "," + sup["SuperVisorCode"].ToString();
                                    }

                                }
                                xlWorkSheet.Cells[rowIndex, 2] = rw["Agent"].ToString();
                                xlWorkSheet.Cells[rowIndex, 1] = superVisorName; xlWorkSheet.Cells[rowIndex, 1].Font.Bold = true;
                                int resultColumn = 3;
                                //now we want to re-iterate this based on column first


                                double totalUnitsForWeek = 0;
                                double totalBaseForWeek = 0;
                                int totalChildrenForWeek = 0;
                                double totalHoursForWeek = 0;
                                foreach (DateTime resultDate in dates)
                                {
                                    int currentWeek = GetWeekOfMonth(resultDate);

                                    bool skippedRows = false;
                                    var resultsForAgent = campaignData.AsEnumerable().Where(x => (long)x["FKUserID"] == (long)rw["FKUserID"]);
                                    var resultsForAgentDate = resultsForAgent.AsEnumerable().Where(x => DateTime.Parse(x["DateOfSale"].ToString()).ToString("yyy-MM-dd") == resultDate.ToString("yyy-MM-dd"));
                                    if (resultsForAgentDate.Count() > 0)
                                    {
                                        //count figures for date
                                        double totalUnitsForday = 0;
                                        int childCountForDay = 0;
                                        int baseCount = 0;
                                        double agentHoursForDay = 0;
                                        foreach (DataRow rwResult in resultsForAgentDate)
                                        {
                                            string dateOfSale = rwResult["DateOfSale"].ToString();
                                            DateTime DateOfSale = DateTime.Parse(dateOfSale);

                                            totalUnitsForday = totalUnitsForday + Double.Parse(rwResult["Units"].ToString());

                                            if ((bool)rwResult["OptionChild"] == true)
                                            {
                                                childCountForDay++;
                                            }
                                            baseCount++;
                                        }
                                        //calculate hours for date
                                        var agentHours = dtAgentHours.AsEnumerable().Where(x => (long)x["FKUserID"] == (long)rw["FKUserID"] && DateTime.Parse(x["WorkingDate"].ToString()).ToString("yyy-MM-dd") == resultDate.ToString("yyy-MM-dd") && (long)x["FKINCampaignID"] == campaign.Value);

                                        if (agentHours.Count() > 0)
                                        {
                                            foreach (DataRow rwAgentHour in agentHours)
                                            {
                                                //calculate morning shift total hours
                                                if (rwAgentHour["MorningShiftStartTime"].ToString() != string.Empty && rwAgentHour["MorningShiftEndTime"].ToString() != string.Empty)
                                                {
                                                    TimeSpan morningShiftStart = (TimeSpan)rwAgentHour["MorningShiftStartTime"];
                                                    TimeSpan morningShiftEnd = (TimeSpan)rwAgentHour["MorningShiftEndTime"];
                                                    agentHoursForDay = agentHoursForDay + morningShiftEnd.Subtract(morningShiftStart).TotalHours;
                                                }
                                                //calculate normal shift total hours
                                                if (rwAgentHour["NormalShiftStartTime"].ToString() != string.Empty && rwAgentHour["NormalShiftEndTime"].ToString() != string.Empty)
                                                {
                                                    TimeSpan normalShiftStart = (TimeSpan)rwAgentHour["NormalShiftStartTime"];
                                                    TimeSpan normalShiftEnd = (TimeSpan)rwAgentHour["NormalShiftEndTime"];
                                                    agentHoursForDay = agentHoursForDay + normalShiftEnd.Subtract(normalShiftStart).TotalHours;
                                                }
                                                //calculate evening shift total hours
                                                if (rwAgentHour["EveningShiftStartTime"].ToString() != string.Empty && rwAgentHour["EveningShiftEndTime"].ToString() != string.Empty)
                                                {
                                                    TimeSpan eveningShiftStart = (TimeSpan)rwAgentHour["EveningShiftStartTime"];
                                                    TimeSpan eveningShiftEnd = (TimeSpan)rwAgentHour["EveningShiftEndTime"];
                                                    agentHoursForDay = agentHoursForDay + eveningShiftEnd.Subtract(eveningShiftStart).TotalHours;
                                                }
                                                //calculate weekend shift total hours
                                                if (rwAgentHour["PublicHolidayWeekendShiftStartTime"].ToString() != string.Empty && rwAgentHour["PublicHolidayWeekendShiftEndTime"].ToString() != string.Empty)
                                                {
                                                    TimeSpan weekendShiftStart = (TimeSpan)rwAgentHour["PublicHolidayWeekendShiftStartTime"];
                                                    TimeSpan weekendShiftEnd = (TimeSpan)rwAgentHour["PublicHolidayWeekendShiftEndTime"];
                                                    agentHoursForDay = agentHoursForDay + weekendShiftEnd.Subtract(weekendShiftStart).TotalHours;
                                                }
                                            }
                                        }
                                        //hours for date//
                                        xlWorkSheet.Cells[rowIndex, resultColumn] = Math.Round(agentHoursForDay, 2); xlWorkSheet.Cells[rowIndex, resultColumn].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                        totalHoursForWeek = totalHoursForWeek + agentHoursForDay;
                                        HoursDayTotals.Add(new TotalObject(resultDate, agentHoursForDay, campaign.Key));
                                        resultColumn++;
                                        //////////
                                        //units for date//
                                        xlWorkSheet.Cells[rowIndex, resultColumn] = totalUnitsForday; xlWorkSheet.Cells[rowIndex, resultColumn].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                        totalUnitsForWeek = totalUnitsForWeek + totalUnitsForday;

                                        UnitDayTotals.Add(new TotalObject(resultDate, totalUnitsForday, campaign.Key));
                                        resultColumn++;
                                        ////
                                        ////base for date
                                        xlWorkSheet.Cells[rowIndex, resultColumn] = baseCount; xlWorkSheet.Cells[rowIndex, resultColumn].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                        totalBaseForWeek = totalBaseForWeek + baseCount;
                                        BaseDayTotals.Add(new TotalObject(resultDate, baseCount, campaign.Key));
                                        resultColumn++;
                                        ////////
                                        ////children for date
                                        xlWorkSheet.Cells[rowIndex, resultColumn] = childCountForDay; xlWorkSheet.Cells[rowIndex, resultColumn].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                        totalChildrenForWeek = totalChildrenForWeek + childCountForDay;
                                        ChildrenDayTotals.Add(new TotalObject(resultDate, childCountForDay, campaign.Key));
                                        resultColumn++;
                                        //////


                                        if (resultDate.DayOfWeek == DayOfWeek.Saturday || resultDate == dates[dates.Count - 1])
                                        {
                                            if (skippedRows == false)
                                            {

                                                double hoursTarget = 0;
                                                double unitsTarget = 0;
                                                double basetarget = 0;
                                                double childrentarget = 0;
                                                //week target
                                                //hours 
                                                var currentWeekData = dtTsrTargets.AsEnumerable().Where(x => (long)x["FKAgentID"] == (long)rw["FKUserID"] && (long)x["FKINCampaignID"] == campaign.Value && (long)x["FKINWeekID"] == (long)currentWeek);
                                                if (currentWeekData.Count() > 0)
                                                {
                                                    foreach (DataRow targetRow in currentWeekData)
                                                    {
                                                        hoursTarget = hoursTarget + double.Parse(targetRow["Hours"].ToString());
                                                        unitsTarget = unitsTarget + double.Parse(targetRow["UnitTarget"].ToString());
                                                        basetarget = basetarget + double.Parse(targetRow["BaseTarget"].ToString());
                                                    }
                                                }
                                                xlWorkSheet.Cells[rowIndex, resultColumn] = hoursTarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                                HoursWeekTotals.Add(new TotalObject(resultDate, hoursTarget, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = unitsTarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                                UnitWeekTotals.Add(new TotalObject(resultDate, unitsTarget, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = basetarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                                BaseWeekTotals.Add(new TotalObject(resultDate, basetarget, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = childrentarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                                ChildrenWeekTotals.Add(new TotalObject(resultDate, childrentarget, campaign.Key));
                                                resultColumn++;
                                                //week actual

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = Math.Round(totalHoursForWeek, 2); xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                                HoursWeekActualTotals.Add(new TotalObject(currentWeek, totalHoursForWeek, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = totalUnitsForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                                UnitWeekActualTotals.Add(new TotalObject(currentWeek, totalUnitsForWeek, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = totalBaseForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                                BaseWeekActualTotals.Add(new TotalObject(currentWeek, totalBaseForWeek, campaign.Key));
                                                resultColumn++;

                                                xlWorkSheet.Cells[rowIndex, resultColumn] = totalChildrenForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                                ChildrenWeekActualTotals.Add(new TotalObject(currentWeek, totalChildrenForWeek, campaign.Key));
                                                resultColumn++;
                                                skippedRows = true;
                                                totalUnitsForWeek = 0;
                                                totalBaseForWeek = 0;
                                                totalChildrenForWeek = 0;
                                                totalHoursForWeek = 0;
                                            }
                                        }

                                    }
                                    else
                                    {

                                        resultColumn++;
                                        resultColumn++;
                                        resultColumn++;
                                        resultColumn++;
                                    }

                                    if (resultDate.DayOfWeek == DayOfWeek.Saturday || resultDate == dates[dates.Count - 1])
                                    {
                                        if (skippedRows == false)
                                        {

                                            double hoursTarget = 0;
                                            double unitsTarget = 0;
                                            double basetarget = 0;
                                            double childrentarget = 0;
                                            //week target
                                            //hours 
                                            var currentWeekData = dtTsrTargets.AsEnumerable().Where(x => (long)x["FKAgentID"] == (long)rw["FKUserID"] && (long)x["FKINCampaignID"] == campaign.Value && (long)x["FKINWeekID"] == (long)currentWeek);
                                            if (currentWeekData.Count() > 0)
                                            {
                                                foreach (DataRow targetRow in currentWeekData)
                                                {
                                                    hoursTarget = hoursTarget + double.Parse(targetRow["Hours"].ToString());
                                                    unitsTarget = unitsTarget + double.Parse(targetRow["UnitTarget"].ToString());
                                                    basetarget = basetarget + double.Parse(targetRow["BaseTarget"].ToString());
                                                }
                                            }
                                            xlWorkSheet.Cells[rowIndex, resultColumn] = hoursTarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                            HoursWeekTotals.Add(new TotalObject(resultDate, hoursTarget, campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = unitsTarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                            UnitWeekTotals.Add(new TotalObject(resultDate, unitsTarget, campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = basetarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                            BaseWeekTotals.Add(new TotalObject(resultDate, basetarget, campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = childrentarget; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 34;
                                            ChildrenWeekTotals.Add(new TotalObject(resultDate, childrentarget, campaign.Key));
                                            resultColumn++;
                                            //week actual

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = Math.Round(totalHoursForWeek, 2); xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                            HoursWeekActualTotals.Add(new TotalObject(currentWeek, Math.Round(totalHoursForWeek, 2), campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = totalUnitsForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                            UnitWeekActualTotals.Add(new TotalObject(currentWeek, totalUnitsForWeek, campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = totalBaseForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                            BaseWeekActualTotals.Add(new TotalObject(currentWeek, totalBaseForWeek, campaign.Key));
                                            resultColumn++;

                                            xlWorkSheet.Cells[rowIndex, resultColumn] = totalChildrenForWeek; xlWorkSheet.Cells[rowIndex, resultColumn].Interior.ColorIndex = 19;
                                            ChildrenWeekActualTotals.Add(new TotalObject(currentWeek, totalChildrenForWeek, campaign.Key));
                                            resultColumn++;
                                            skippedRows = true;
                                            totalUnitsForWeek = 0;
                                            totalBaseForWeek = 0;
                                            totalChildrenForWeek = 0;
                                            totalHoursForWeek = 0;
                                        }
                                    }

                                }
                                //cell borders
                                for (int i = 1; i <= resultColumn - 1; i++)
                                {
                                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                }

                                rowIndex++;
                            }

                        }

                        //totals here///////////////////
                        xlWorkSheet.Cells[rowIndex, 2] = "TOTALS"; xlWorkSheet.Cells[rowIndex, 2].Font.Bold = true;
                        xlWorkSheet.Cells[rowIndex, 2].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                        int totalColIndex = 3;

                        foreach (DateTime totalDate in dates)
                        {
                            int currentWeekIndex = GetWeekOfMonth(totalDate);

                            var totalHours = (from h in HoursDayTotals where DateTime.Parse(h.Description.ToString()).Date == totalDate.Date select h);
                            var totalUnits = (from u in UnitDayTotals where DateTime.Parse(u.Description.ToString()).Date == totalDate.Date select u);
                            var totalBase = (from b in BaseDayTotals where DateTime.Parse(b.Description.ToString()).Date == totalDate.Date select b);
                            var totalChildren = (from c in ChildrenDayTotals where DateTime.Parse(c.Description.ToString()).Date == totalDate.Date select c);

                            double totalHoursForDay = 0;
                            double totalUnitsForDay = 0;
                            int totalBaseForDay = 0;
                            int totalChildrenForDay = 0;
                            foreach (TotalObject unit in totalUnits)
                            {
                                totalUnitsForDay = totalUnitsForDay + double.Parse(unit.Value.ToString());
                            }
                            foreach (TotalObject baseTarg in totalBase)
                            {
                                totalBaseForDay = totalBaseForDay + int.Parse(baseTarg.Value.ToString());
                            }
                            foreach (TotalObject children in totalChildren)
                            {
                                totalChildrenForDay = totalChildrenForDay + int.Parse(children.Value.ToString());
                            }
                            foreach (TotalObject hour in totalHours)
                            {
                                totalHoursForDay = totalHoursForDay + double.Parse(hour.Value.ToString());
                            }
                            xlWorkSheet.Cells[rowIndex, totalColIndex] = Math.Round(totalHoursForDay, 2); xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            totalColIndex++;


                            xlWorkSheet.Cells[rowIndex, totalColIndex] = totalUnitsForDay; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            totalColIndex++;

                            xlWorkSheet.Cells[rowIndex, totalColIndex] = totalBaseForDay; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            totalColIndex++;

                            xlWorkSheet.Cells[rowIndex, totalColIndex] = totalChildrenForDay; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                            xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            totalColIndex++;


                            if (totalDate.Date.DayOfWeek == DayOfWeek.Saturday || totalDate == dates[dates.Count - 1])
                            {

                                var totalHoursWeek = (from h in HoursWeekTotals where DateTime.Parse(h.Description.ToString()).Date == totalDate.Date select h);
                                var totalUnitsWeek = (from u in UnitWeekTotals where DateTime.Parse(u.Description.ToString()).Date == totalDate.Date select u);
                                var totalBaseWeek = (from b in BaseWeekTotals where DateTime.Parse(b.Description.ToString()).Date == totalDate.Date select b);
                                var totalChildrenWeek = (from c in ChildrenWeekTotals where DateTime.Parse(c.Description.ToString()).Date == totalDate.Date select c);

                                double totalHoursForWeek = 0;
                                double totalUnitsForWeek = 0;
                                int totalBaseForWeek = 0;
                                int totalChildrenForWeek = 0;
                                foreach (TotalObject unit in totalUnitsWeek)
                                {
                                    totalUnitsForWeek = totalUnitsForWeek + double.Parse(unit.Value.ToString());
                                }
                                foreach (TotalObject baseTarg in totalBaseWeek)
                                {
                                    totalBaseForWeek = totalBaseForWeek + int.Parse(baseTarg.Value.ToString());
                                }
                                foreach (TotalObject children in totalChildrenWeek)
                                {
                                    totalChildrenForWeek = totalChildrenForWeek + int.Parse(children.Value.ToString());
                                }
                                foreach (TotalObject hour in totalHoursWeek)
                                {
                                    totalHoursForWeek = totalHoursForWeek + double.Parse(hour.Value.ToString());
                                }

                                //week target
                                xlWorkSheet.Cells[rowIndex, totalColIndex] = Math.Round(totalHoursForWeek, 2); xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 34;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalUnitsForWeek; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 34;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalBaseForWeek; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 34;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalChildrenForWeek; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 34;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                                totalColIndex++;

                                //week actual
                                var totalHoursWeekActual = (from h in HoursWeekActualTotals where int.Parse(h.Description.ToString()) == currentWeekIndex select h);
                                var totalUnitsWeekActual = (from u in UnitWeekActualTotals where int.Parse(u.Description.ToString()) == currentWeekIndex select u);
                                var totalBaseWeekActual = (from b in BaseWeekActualTotals where int.Parse(b.Description.ToString()) == currentWeekIndex select b);
                                var totalChildrenWeekActual = (from c in ChildrenWeekActualTotals where int.Parse(c.Description.ToString()) == currentWeekIndex select c);

                                double totalHoursForWeekActual = 0;
                                double totalUnitsForWeekActual = 0;
                                int totalBaseForWeekActual = 0;
                                int totalChildrenForWeekActual = 0;
                                foreach (TotalObject unit in totalUnitsWeekActual)
                                {
                                    totalUnitsForWeekActual = totalUnitsForWeekActual + double.Parse(unit.Value.ToString());
                                }
                                foreach (TotalObject baseTarg in totalBaseWeekActual)
                                {
                                    totalBaseForWeekActual = totalBaseForWeekActual + int.Parse(baseTarg.Value.ToString());
                                }
                                foreach (TotalObject children in totalChildrenWeekActual)
                                {
                                    totalChildrenForWeekActual = totalChildrenForWeekActual + int.Parse(children.Value.ToString());
                                }
                                foreach (TotalObject hour in totalHoursWeekActual)
                                {
                                    totalHoursForWeekActual = totalHoursForWeekActual + double.Parse(hour.Value.ToString());
                                }

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = Math.Round(totalHoursForWeekActual, 2); xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 19;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalUnitsForWeekActual; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 19;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalBaseForWeekActual; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 19;
                                totalColIndex++;

                                xlWorkSheet.Cells[rowIndex, totalColIndex] = totalChildrenForWeekActual; xlWorkSheet.Cells[rowIndex, totalColIndex].Font.Bold = true;
                                xlWorkSheet.Cells[rowIndex, totalColIndex].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[rowIndex, totalColIndex].Interior.ColorIndex = 19;
                                totalColIndex++;
                            }
                        }
                        rowIndex++;


                    }


                }
                xlWorkBook.SaveAs(filePathAndName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                xlWorkBook.Close(true, misValue, misValue);

                xlApp.Quit();

                //Display excel document
                Process.Start(filePathAndName);
                #endregion report Data




            }
            catch (Exception ex)
            {
                reportDailySalesControl.HandleException(ex);
            }
            finally
            {
                reportDailySalesControl.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate { reportDailySalesControl.Cursor = Cursors.Arrow; });
            }
        }

        public static int GetWeekOfMonth(DateTime date)
        {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);

            return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f)/* + 1*/;
        }

        public class TotalObject
        {
            public TotalObject(object description, object value, string campaignCode)
            {
                Description = description;
                Value = value;
                CampaignCode = campaignCode;
            }
            public object Description { get; set; }
            public object Value { get; set; }
            public string CampaignCode { get; set; } // makes for easy reference
        }

        public bool CloseEnabled
        {
            get { return closeEnabled; }
            set
            {
                closeEnabled = value;
                OnPropertyChanged("CloseEnabled");
            }
        }

        private bool EnableDisableReportButton(object obj)
        {
            bool agentSelected = false;
            foreach (DataRowView rv in Agents)
            {
                agentSelected = true;
                if ((bool)rv["IsChecked"] == true)
                {
                    agentSelected = true;
                }
            }
            if (_startDate != null && ToDate != null && agentSelected == true && ReportMode != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool EnableDisableCancelReportButton(object obj)
        {
            if (CloseEnabled == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void HeaderPrefixAreaChecked()
        {
            ReportDailySales dailysSales = (ReportDailySales)LoadbaseControl();
            DataTable dt = ((DataView)dailysSales.xdgAgents.DataSource).Table;

            foreach (DataRow dr in dt.Rows)
            {
                dr["IsChecked"] = true;
            }
        }
        public void TempStaffAreaChecked()
        {
            ReportDailySales dailysSales = (ReportDailySales)LoadbaseControl();
            DataTable dt = ((DataView)dailysSales.xdgAgents.DataSource).Table;

            foreach (DataRow dr in dt.Rows)
            {
                dr["IsChecked"] = true;
            }
        }
        public void HeaderPrefixAreaUnChecked()
        {
            ReportDailySales dailysSales = (ReportDailySales)LoadbaseControl();
            DataTable dt = ((DataView)dailysSales.xdgAgents.DataSource).Table;
            foreach (DataRow dr in dt.Rows)
            {
                dr["IsChecked"] = false;
            }
        }
        private BaseControl LoadbaseControl()
        {
            BaseControl baseControl = new BaseControl();
            WindowCollection windows = Application.Current.Windows;
            foreach (Window w in windows)
            {
                if (w.Title == "INDialogWindow")
                {
                    INDialogWindow dlgWindow = (INDialogWindow)w;
                    Grid contentGrid = (Grid)dlgWindow.Content;
                    baseControl = (BaseControl)contentGrid.Children[0];
                    break;
                }
            }
            return baseControl;
        }

    }
}
