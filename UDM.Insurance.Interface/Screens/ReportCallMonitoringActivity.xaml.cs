using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
using UDM.Insurance.Interface.Data;

namespace UDM.Insurance.Interface.Screens
{

    public partial class ReportCallMonitoringActivity : INotifyPropertyChanged
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

        private string _fkUserIDs = String.Empty;

        #endregion Private Members

        #region Constructors

        public ReportCallMonitoringActivity()
        {

            InitializeComponent();
            LoadAgentInfo();

            _reportTimer.Tick += ReportTimer;
            _reportTimer.Interval = new TimeSpan(0, 0, 1);
            btnReport.IsEnabled = true;
        }

        #endregion

        #region Private Methods

        private void LoadAgentInfo()
        {
            try
            {
                Cursor = Cursors.Wait;

                if (grdAgents.IsEnabled)
                {
                    //DataSet ds = Methods.ExecuteStoredProcedure("spGetAllAgents", null);

                    StringBuilder strQuery = new StringBuilder();
                    strQuery.Append("SELECT ID, Blush.dbo.GetUserName(ID) [Description] FROM [User] WHERE FKUserType = '8' ORDER BY [Description]");

                    DataTable dt = Methods.GetTableData(strQuery.ToString());
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

        private bool IsAllInputParametersSpecifiedAndValid()
        {
            #region Ensuring that at least 1 user was selected

            //var lstTemp = (from r in xdgAgents.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            //_lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));
            _selectedAgents = (xdgAgents.Records.Select(r => (DataRecord)r).Where(r => (bool)r.Cells["IsChecked"].Value)).ToList();

            if (_selectedAgents.Count == 0)
            {
                ShowMessageBox(new Windows.INMessageBoxWindow1(), "Please select at least 1 sales agent from the list.", "No sales agent selected", Embriant.Framework.ShowMessageType.Error);
                return false;
            }
            else
            {
                _fkUserIDs = _selectedAgents.Cast<DataRecord>().Where(record => (bool)record.Cells["IsChecked"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                _fkUserIDs = _fkUserIDs.Substring(0, _fkUserIDs.Length - 1);
            }

            #endregion Ensuring that at least 1 campaign was selected

            #region Ensuring that the From Date was specified

            if (Cal1.SelectedDate == null)
            {
                ShowMessageBox(new Windows.INMessageBoxWindow1(), @"Please specify the 'From Date'.", @"No 'From Date' specified", Embriant.Framework.ShowMessageType.Error);
                return false;
            }
            else
            {
                _fromDate = Cal1.SelectedDate.Value;
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the To Date was specified

            if (Cal2.SelectedDate == null)
            {
                ShowMessageBox(new Windows.INMessageBoxWindow1(), @"Please specify the 'To Date'.", @"No 'To Date' specified", Embriant.Framework.ShowMessageType.Error);
                return false;
            }

            else
            {
                _toDate = Cal2.SelectedDate.Value;
            }

            #endregion Ensuring that the To Date was specified

            #region Ensuring that the date range is valid

            if (_fromDate > _toDate)
            {
                ShowMessageBox(new Windows.INMessageBoxWindow1(), @"Invalid date range specified: The 'From Date' can not be greater than the 'To Date'.", "Invalid date range", Embriant.Framework.ShowMessageType.Error);
                return false;
            }


            #endregion Ensuring that the date range is valid

            // Otherwise if all is well, proceed:
            return true;
        }

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _reportTimer.Stop();
            _seconds = 0;
            IsReportRunning = false;
            btnReport.Content = "Report";
            lblCurrentAgent.Text = String.Empty;
        }

        private void ReportBody(Workbook wbTemplate, Workbook wbReport, string agentName, long? agentID, string dateRange)
        {
            #region retrieve data from database

            SqlParameter[] parameters =
                {
                    new SqlParameter("@FKAgentID", agentID),
                    new SqlParameter("@FromDate", _fromDate),
                    new SqlParameter("@ToDate", _toDate)
                };

            DataSet ds = Methods.ExecuteStoredProcedure("spINReportCallMonitoringActivity", parameters);
            DataTable dtActivity = ds.Tables[0];

            #endregion

            #region setup worksheet

            //WorksheetCell wsCell;
            Worksheet wsTemplate = wbTemplate.Worksheets["Report"];
            Worksheet wsReport = wbReport.Worksheets.Add(string.Join("", agentName.Take(31)));

            wsReport.DisplayOptions.View = WorksheetView.Normal;

            wsReport.PrintOptions.PaperSize = PaperSize.A4;
            wsReport.PrintOptions.Orientation = Orientation.Portrait;
            wsReport.PrintOptions.LeftMargin = 0.3;
            wsReport.PrintOptions.RightMargin = 0.3;

            Methods.CopyExcelRegion(wsTemplate, 0, 0, 6, 32, wsReport, 0, 0);

            #endregion

            #region header data

            {
                wsReport.GetCell("A1").Value = agentName;
                wsReport.GetCell("A3").Value = "Call Monitoring Activity : " + dateRange;
            }

            #endregion

            #region report data
            {
                int rowReport = 6;
                wsReport.Workbook.NamedReferences.Clear();

                for (int rowIndex = 0; rowIndex < dtActivity.Rows.Count; rowIndex++)
                {
                    Methods.CopyExcelRegion(wsTemplate, 6, 0, 1, 32, wsReport, rowIndex + rowReport, 0);

                    wsReport.GetCell("CampaignCode").Value = dtActivity.Rows[rowIndex]["CampaignCode"].ToString();
                    wsReport.GetCell("BatchCode").Value = dtActivity.Rows[rowIndex]["BatchCode"].ToString();
                    wsReport.GetCell("RefNo").Value = dtActivity.Rows[rowIndex]["RefNo"].ToString();

                    wsReport.GetCell("WasMoneyBackVsVoucherBenefitsExplainedCorrectly").Value = dtActivity.Rows[rowIndex]["WasMoneyBackVsVoucherBenefitsExplainedCorrectly"].ToString();
                    wsReport.GetCell("IsBankingDetailsCapturedCorrectly").Value = dtActivity.Rows[rowIndex]["IsBankingDetailsCapturedCorrectly"].ToString();
                    wsReport.GetCell("WasAccountVerified").Value = dtActivity.Rows[rowIndex]["WasAccountVerified"].ToString();
                    wsReport.GetCell("WasDebitDateConfirmed").Value = dtActivity.Rows[rowIndex]["WasDebitDateConfirmed"].ToString();
                    wsReport.GetCell("IsAccountInClientsName").Value = dtActivity.Rows[rowIndex]["IsAccountInClientsName"].ToString();
                    wsReport.GetCell("DoesClientHaveSigningPower").Value = dtActivity.Rows[rowIndex]["DoesClientHaveSigningPower"].ToString();

                    wsReport.GetCell("WasCorrectClosureQuestionAsked").Value = dtActivity.Rows[rowIndex]["WasCorrectClosureQuestionAsked"].ToString();
                    wsReport.GetCell("WasResponseClearAndPositive").Value = dtActivity.Rows[rowIndex]["WasResponseClearAndPositive"].ToString();
                    wsReport.GetCell("WasUDMAndPLMentionedAsFSPs").Value = dtActivity.Rows[rowIndex]["WasUDMAndPLMentionedAsFSPs"].ToString();
                    wsReport.GetCell("WasDebitAmountMentionedCorrectly").Value = dtActivity.Rows[rowIndex]["WasDebitAmountMentionedCorrectly"].ToString();
                    wsReport.GetCell("WasFirstDebitDateExplainedCorrectly").Value = dtActivity.Rows[rowIndex]["WasFirstDebitDateExplainedCorrectly"].ToString();
                    wsReport.GetCell("WasCorrectCoverCommencementDateMentioned").Value = dtActivity.Rows[rowIndex]["WasCorrectCoverCommencementDateMentioned"].ToString();
                    wsReport.GetCell("WasNonPaymentProcedureExplained").Value = dtActivity.Rows[rowIndex]["WasNonPaymentProcedureExplained"].ToString();
                    wsReport.GetCell("WasAnnualIncreaseExplained").Value = dtActivity.Rows[rowIndex]["WasAnnualIncreaseExplained"].ToString();
                    wsReport.GetCell("ExclusionsExplained").Value = dtActivity.Rows[rowIndex]["ExclusionsExplained"].ToString();

                    wsReport.GetCell("WasCorrectQuestionAskedBumpUpClosure").Value = dtActivity.Rows[rowIndex]["WasCorrectQuestionAskedBumpUpClosure"].ToString();
                    wsReport.GetCell("WasResponseClearAndPositiveBumpUpClosure").Value = dtActivity.Rows[rowIndex]["WasResponseClearAndPositiveBumpUpClosure"].ToString();
                    wsReport.GetCell("WasUDMAndPLMentionedAsFSPsBumpUpClosure").Value = dtActivity.Rows[rowIndex]["WasUDMAndPLMentionedAsFSPsBumpUpClosure"].ToString();
                    wsReport.GetCell("WasDebitAmountMentionedCorrectlyBumpUpClosure").Value = dtActivity.Rows[rowIndex]["WasDebitAmountMentionedCorrectlyBumpUpClosure"].ToString();
                    wsReport.GetCell("WasFirstDebitDateExplainedCorrectlyBumpUpClosure").Value = dtActivity.Rows[rowIndex]["WasFirstDebitDateExplainedCorrectlyBumpUpClosure"].ToString();
                    wsReport.GetCell("WasCorrectCoverCommencementDateMentionedBumpUpClosure").Value = dtActivity.Rows[rowIndex]["WasCorrectCoverCommencementDateMentionedBumpUpClosure"].ToString();
                    wsReport.GetCell("WasNonPaymentProcedureExplainedBumpUpClosure").Value = dtActivity.Rows[rowIndex]["WasNonPaymentProcedureExplainedBumpUpClosure"].ToString();
                    wsReport.GetCell("WasAnnualIncreaseExplainedBumpUpClosure").Value = dtActivity.Rows[rowIndex]["WasAnnualIncreaseExplainedBumpUpClosure"].ToString();
                    wsReport.GetCell("ExclusionsExplainedBumpUpClosure").Value = dtActivity.Rows[rowIndex]["ExclusionsExplainedBumpUpClosure"].ToString();

                    wsReport.GetCell("WasCallEvaluatedBySecondaryUser").Value = dtActivity.Rows[rowIndex]["WasCallEvaluatedBySecondaryUser"].ToString();
                    wsReport.GetCell("IsRecoveredSale").Value = dtActivity.Rows[rowIndex]["IsRecoveredSale"].ToString();
                    wsReport.GetCell("Comments").Value = dtActivity.Rows[rowIndex]["Comments"].ToString();

                    wsReport.GetCell("StampUser").Value = dtActivity.Rows[rowIndex]["StampUser"].ToString();
                    wsReport.GetCell("StampDate").Value = dtActivity.Rows[rowIndex]["StampDate"].ToString();

                    wsReport.Workbook.NamedReferences.Clear();
                }

            }
            #endregion
        }

        public List<ActivityDetail> SortReferenceStatuses(string referenceId, DataTable dtAgent, int statusId)
        {
            string description = string.Empty;
            string statusID = string.Empty;
            List<ActivityDetail> lstAgent = new List<ActivityDetail>();
            
            if (dtAgent != null)
            {
                //Get All Records Containing a Reference
                var results = (from row in dtAgent.AsEnumerable()
                               where row.Field<string>("refNo") == referenceId
                               select row);
                               
                               //.OrderBy(x => x.Field<DateTime>("datetime"));

                //Do we have more than one reference number
                if (results.Count() > 1)
                {
                    try
                    {
                        foreach (DataRow row in results)
                        {
                            if (row["StatusID"].ToString().Length > 0)
                            {
                                ActivityDetail agentObj = new ActivityDetail();
                                agentObj.ActivityDate = DateTime.Parse(row["datetime"].ToString());
                                agentObj.ReferenceNumber = row["refNo"].ToString();
                                agentObj.Status = int.Parse(row["StatusID"].ToString());
                                agentObj.Description = GetStatusDescription(agentObj.Status);
                                lstAgent.Add(agentObj); 
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                    for (int x = 0; x < lstAgent.Count; x++)
                    {
                        //Ensure it doesnt go out of index
                        int test1 = lstAgent.Count;
                        int test2 = lstAgent.Count -1;
                        if (x < lstAgent.Count - 1)
                        {
                            ActivityDetail detail = lstAgent[x];
                            //Check current record and next record
                            if (detail.Status == lstAgent[x + 1].Status)
                            {
                                lstAgent[x].FinalDescription = "No Change";
                                lstAgent[x + 1].FinalDescription = GetStatusDescription(detail.Status);
                            }
                            else
                            {
                                if (lstAgent.Count > 1)
                                {
                                    lstAgent[x].FinalDescription = GetStatusDescription(lstAgent[x].Status);
                                    lstAgent[x + 1].FinalDescription = GetStatusDescription(lstAgent[x+1].Status);
                                }
                                else
                                {
                                    lstAgent[x].FinalDescription = GetStatusDescription(lstAgent[x].Status);
                                }
                            }
                        }
                        else
                        {
                            string test = string.Empty;
                        }
                    }
                }
                else
                {
                    if (results.Count() > 0)
                    {
                        foreach (var item in results)
                        {
                            //only one record found
                            ActivityDetail agentObj = new ActivityDetail();
                            agentObj.ActivityDate = DateTime.Parse(item["datetime"].ToString());
                            agentObj.ReferenceNumber = item["refNo"].ToString();
                            agentObj.Status = int.Parse(item["StatusID"].ToString());
                            agentObj.Description = GetStatusDescription(agentObj.Status);
                            description = GetStatusDescription(statusId);
                            agentObj.FinalDescription = description;
                            lstAgent.Add(agentObj);
                        }
                    }
                }
            }
            return lstAgent;
        }
        private string GetLatestStatusDescription(string referenceId, DataTable dtAgent, int statusId)
        {
            string description = string.Empty;
            string statusID = string.Empty;
            List<AgentActivity> lstAgent = new List<AgentActivity>();

            try
            {
                if (dtAgent != null)
                {
                    //Get All Records Containing a Reference
                    var results = (from row in dtAgent.AsEnumerable()
                                   where row.Field<string>("refNo") == referenceId
                                   select row).OrderBy(x => x.Field<DateTime>("datetime"));

                    //Do we have more than one reference number
                    if (results.Count() > 1)
                    {
                        foreach (DataRow row in results)
                        {
                            
                        }
                    }
                    else
                    {
                        description = GetStatusDescription(statusId);
                    }


                    if (results != null)
                    {
                        if (results.Count() == 1)
                        {
                            description = "No Change";
                        }
                        else
                        {
                            foreach (DataRow row in results)
                            {
                                statusID = row["StatusID"].ToString();
                            }

                            //The status has changed. Get the latest status
                            DateTime oldest = results.Max(x => x.Field<DateTime>("datetime"));
                            //results = results.Where(x => x.Field<DateTime>("datetime") == oldest);

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                HandleException(ex);
            }

            return description;
        }
        public string GetStatusDescription(int statusID)
        {
            string status = string.Empty;
            //Extract the description
            try
            {
                SqlParameter[] parameters =
                        {
                            new SqlParameter("@StatusID", statusID)
                            
                        };
                DataSet ds = Methods.ExecuteStoredProcedure("sp_statusDescription", parameters);
                DataTable dtStatuses = ds.Tables[0];
                status = dtStatuses.Rows[0]["Description"].ToString();
                ds.Dispose();
                dtStatuses.Dispose();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return status;
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

                string filePathAndName = GlobalSettings.UserFolder + "Call Monitoring Activity Report (" + dateRange + ") ~ " + DateTime.Now.Millisecond + ".xlsx";

                Uri uri = new Uri("/Templates/ReportTemplateCallMonitoringActivity.xlsx", UriKind.Relative);
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
                    foreach (DataRecord drAgent in _selectedAgents)
                    {
                        string agentName = drAgent.Cells["Description"].Value as string;
                        _agentName = agentName;

                        long? agentID = drAgent.Cells["ID"].Value as long?;
                        ReportBody(wbTemplate, wbReport, agentName, agentID, dateRange);
                    }
                }
                else
                {
                    string agentName = _user.FirstName.Trim() + " " + _user.LastName.Trim();
                    long? agentID = _user.ID;

                    ReportBody(wbTemplate, wbReport, agentName, agentID, dateRange);
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

        private void ReportOld(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup excel documents

                string filePathAndName = String.Format("{0}Call Monitoring Activity Report {1:yyyy-MM-dd HHmmdd}.xlsx", GlobalSettings.UserFolder, DateTime.Now);

                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateCallMonitoringActivity.xlsx");
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup excel documents

                #region Get the data

                DataSet dsReport = Insure.INReportCallMonitoringActivity(_fkUserIDs, _fromDate, _toDate);

                if (dsReport.Tables.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate
                    {
                        ShowMessageBox(new Windows.INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", Embriant.Framework.ShowMessageType.Information);
                    });

                    return;
                }

                #endregion Get the data

                //foreach (DataRow row in dsReport.Tables[0].Rows)
                //{
                //    InsertIndividualActivityReportSheet(wbTemplate, wbReport, dsUpgradeSTLReportDate, row);
                //}

                #region Save & Display the resulting workbook - if there is at least 1 worksheet

                if (wbReport.Worksheets.Count < 1)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate
                    {
                        ShowMessageBox(new Windows.INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", Embriant.Framework.ShowMessageType.Information);
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

        private void InsertIndividualActivityReportSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsUpgradeSTLReportDate, DataRow drCurrentSalesConsultant)
        {
            string filterString = drCurrentSalesConsultant["FilterString"].ToString();

            var filteredRows = dsUpgradeSTLReportDate.Tables[1].Select(filterString).AsEnumerable();
            if (filteredRows.Any())
            {

                #region Partition the given dataset

                string orderByString = drCurrentSalesConsultant["OrderByString"].ToString();
                DataTable dtCurrentUserActivityData = dsUpgradeSTLReportDate.Tables[1].Select(filterString, orderByString).CopyToDataTable();
                DataTable dtExcelSheetDataTableColumnMappings = dsUpgradeSTLReportDate.Tables[2];
                //DataTable dtExcelSheetTotalsAndAverageColumnMappings = dsUpgradeSTLReportDate.Tables[3];

                #endregion Partition the given dataset

                #region Declarations & Initializations

                int reportRow = 7;
                //int formulaStartRow = reportRow;

                byte templateDataSheetRowSpan = 6;
                byte templateColumnSpan = 2;
                byte templateRowIndex = 7;
                //byte totalsTemplateRowIndex = 14;

                string salesConsultantName = drCurrentSalesConsultant["SalesConsultantName"].ToString();
                string worksheetTabName = Methods.ParseWorksheetName(wbReport, salesConsultantName); //drCurrentSalesConsultant["WorksheetTabName"].ToString();
                string campaignDataSheetTemplateName = "Report"; //selectedCampaign.Cells["CampaignCategory"].Value.ToString();

                string reportHeadingCell = "A1";        //dtReportConfigs.Rows[0]["ReportHeadingCell"].ToString();
                string reportSubHeadingCell = "A3";     //dtReportConfigs.Rows[0]["ReportSubHeadingCell"].ToString();
                string reportDateCell = "C5";           //dtReportConfigs.Rows[0]["ReportDateCell"].ToString();

                string reportTitle = drCurrentSalesConsultant["ReportTitle"].ToString();
                string reportSubTitle = drCurrentSalesConsultant["ReportSubTitle"].ToString();

                #endregion Declarations & Initializations

                #region Add the worksheet

                Worksheet wsReportTemplate = wbTemplate.Worksheets[campaignDataSheetTemplateName];
                Worksheet wsReport = wbReport.Worksheets.Add(worksheetTabName);
                Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                #endregion Add the worksheet

                #region Populating the report details

                Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
                wsReport.GetCell(reportHeadingCell).Value = reportTitle;
                wsReport.GetCell(reportSubHeadingCell).Value = reportSubTitle;
                wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                #endregion Populating the report details

                #region Add the data

                reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentUserActivityData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                #endregion Add the data

                //#region Add the totals / averages

                //reportRow = Methods.MapTemplatizedExcelFormulas(wsReportTemplate, dtExcelSheetTotalsAndAverageColumnMappings, totalsTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0, formulaStartRow, reportRow - 1);

                //#endregion Add the totals / averages
            }
        }

        private void InsertActivityReportSheet()
        {
            



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
                if (IsAllInputParametersSpecifiedAndValid())
                {
                    IsReportRunning = true;
                    //_selectedAgents = (xdgAgents.Records.Select(r => (DataRecord)r).Where(r => (bool)r.Cells["IsChecked"].Value)).ToList();

                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += Report;
                    worker.RunWorkerCompleted += ReportCompleted;
                    worker.RunWorkerAsync();

                    _reportTimer.Start();
                }
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

    }
}
