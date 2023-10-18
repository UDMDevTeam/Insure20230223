using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Windows.DataPresenter;
using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Services.Description;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using static UDM.Insurance.Interface.Data.LeadApplicationData;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class NoContactSMS
    {

        #region Constants

        DataSet dsDebiCheckSpecialistLogsData;
        DataSet dsAutoAssignLeadsData;
        DataSet _dsLookups;
        private System.Windows.Controls.CheckBox _xdgCampaignsHeaderPrefixAreaCheckbox;
        private System.Windows.Controls.CheckBox _xdgCampaignClusterssHeaderPrefixAreaCheckbox;
        private string _fkINCampaignFKINCampaignClusterIDs;
        private List<Record> _lstSelectedCampaigns;

        SalaryReportType _salaryReportType;
        private List<Record> _lstSelectedCampaignClusters;

        private bool _bonusSales = false;
        #region BulkSMS

        //Bulk SMS variables
        private const string bulkSMSTokenName = "UDMSoftware";
        private const string bulkSMStokenID = "5DF865E90D5D4AEE9F2A6790D02959FC-01-0";
        private const string bulkSMStokenSecret = "aQ958yyEQA*v8E1sugmfMVVg!HPRO";

        private const string bulkSMSUserName = "UDMIG";
        private const string bulkSMSPassword = "Gorilla1!";
        private const string bulkSMSURL = "https://api.bulksms.com/v1";
        private LeadApplicationData _laData = new LeadApplicationData();

        public LeadApplicationData LaData
        {
            get { return _laData; }
            set { _laData = value; }
        }
        #endregion BulkSMS

        #endregion Constants

        #region Private Members
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;
        private DateTime _startDate;
        private DateTime _endDate;
        #endregion

        public NoContactSMS()
        {
            InitializeComponent();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);

            LoadDatagrid();
            //LoadSummaryData();
            LoadLookupValues();

        }

        private void LoadDatagrid()
        {
            #region Datagrid 1
            try
            {
                dsAutoAssignLeadsData = Business.Insure.INGetNoContactBulkSMSLookups(DateTime.Parse("2023-04-01 00:00:00"), DateTime.Parse("2023-08-16 23:00:00"), "");

                System.Data.DataTable Dt = dsAutoAssignLeadsData.Tables[0];

                LeadsDatagrid.ItemsSource = Dt.DefaultView;
            } catch { }

            #endregion
        }
        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;
            btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnReport.ToolTip = btnReport.Content;
        }


        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                    btnClose.IsEnabled = false;
                    //btnReport.IsEnabled = false;


                    //Cal2.IsEnabled = false;

                    TimeSpan ts11 = new TimeSpan(23, 00, 0);
                    DateTime enddate = _endDate.Date + ts11;


                        
                    dsDebiCheckSpecialistLogsData = Business.Insure.INGetConservedLeads(_startDate, enddate);




                    BackgroundWorker worker = new BackgroundWorker();
                    
                    worker.DoWork += Report;
                    worker.RunWorkerCompleted += ReportCompleted;
                    worker.RunWorkerAsync();

                    dispatcherTimer1.Start();
                
            }

            catch (Exception ex)
            {
                //HandleException(ex);

                btnReport.IsEnabled = true;
                btnClose.IsEnabled = true;
            }
        }


        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Get the report data
                System.Data.DataTable dtSalesData;


                dtSalesData = dsDebiCheckSpecialistLogsData.Tables[0];


                #endregion Get the report data

                try
                {
                    string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    string filePathAndName = "";

                    filePathAndName = String.Format("{0}Conserved Lead Report, {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
                    

                    if (dtSalesData == null || dtSalesData.Columns.Count == 0)
                        throw new Exception("ExportToExcel: Null or empty input table!\n");

                    // load excel, and create a new workbook
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Workbooks.Add();


                    // single worksheet
                    Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;
                    

                    workSheet.Name = "Conserved Leads";
                    

                    workSheet.Cells[1, 0 + 1] = "Date Range : " + _startDate.ToShortDateString() + " to " + _endDate.ToShortDateString();
                    for (var i = 0; i < dtSalesData.Columns.Count; i++)
                    {

                        workSheet.Cells[2, i + 1].Font.Bold = true;
                        workSheet.Cells[2, i + 1].ColumnWidth = 20;
                        workSheet.Cells[2, i + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        //workSheet.get_Range("A4", "J1").Font.Bold = true;
                    }


                    // column headings
                    for (var i = 0; i < dtSalesData.Columns.Count; i++)
                    {
                        workSheet.Cells[2, i + 1] = dtSalesData.Columns[i].ColumnName;
                    }

                    // rows
                    for (var i = 1; i < dtSalesData.Rows.Count + 1; i++)
                    {
                        // to do: format datetime values before printing
                        for (var j = 0; j < dtSalesData.Columns.Count; j++)
                        {
                            workSheet.Cells[i + 2, j + 1] = dtSalesData.Rows[i - 1][j];
                        }
                    }

                    (workSheet.Cells[3, 3]).EntireColumn.NumberFormat = "MM/DD/YYYY hh:mm:ss";



                    // check file path

                    excelApp.Visible = true;
                    excelApp.Workbooks.Item[1].SaveAs(filePathAndName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    //excelApp.Save(filePathAndName);
                    ////Process.Start(filePathAndName);

                    //excelApp.Workbooks.

                }
                catch (Exception ex)
                {

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



        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }



        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";

            btnReport.IsEnabled = true;
            btnClose.IsEnabled = true;

        }

        private void LoadSummaryData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataSet ds = Insure.INGetAssignLeadsSummaryDataAutoAssign();

                DataRelation relCampaignBatch = new DataRelation("CampaignBatch", ds.Tables[0].Columns["CampaignID"], ds.Tables[1].Columns["CampaignID"]);
                ds.Relations.Add(relCampaignBatch);

                DataRelation relBatchAgent = new DataRelation("BatchAgent", ds.Tables[1].Columns["BatchID"], ds.Tables[2].Columns["BatchID"]);
                ds.Relations.Add(relBatchAgent);

                DataRelation relAgentLeadBook = new DataRelation("AgentLeadBook", new[] { ds.Tables[2].Columns["BatchID"], ds.Tables[2].Columns["UserID"] }, new[] { ds.Tables[3].Columns["BatchID"], ds.Tables[3].Columns["UserID"] }, false);
                ds.Relations.Add(relAgentLeadBook);

                DataRelation relLeadBookMiner = new DataRelation("LeadBookMiner", ds.Tables[3].Columns["LeadBookID"], ds.Tables[4].Columns["LeadBookID"]);
                ds.Relations.Add(relLeadBookMiner);

                xdgAssignLeads.DataSource = ds.Tables[0].DefaultView;
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

        private void GridLastView(XamDataGrid grid, int? indexCampaign, int? indexBatch, int? indexAgent, int? indexLeadBook)
        {
            try
            {
                if (indexCampaign != null)
                {
                    if (indexBatch != null)
                    {
                        grid.Records[(int)indexCampaign].IsExpanded = true;
                        DataRecord drBatch = (DataRecord)(((DataRecord)grid.Records[(int)indexCampaign]).ChildRecords[0].ViewableChildRecords[(int)indexBatch]);

                        if (indexAgent != null)
                        {
                            drBatch.IsExpanded = true;
                            DataRecord drAgent = (DataRecord)drBatch.ChildRecords[0].ViewableChildRecords[(int)indexAgent];

                            if (indexLeadBook != null)
                            {
                                drAgent.IsExpanded = true;
                                DataRecord drLeadBook = (DataRecord)drAgent.ChildRecords[0].ViewableChildRecords[(int)indexLeadBook];

                                drLeadBook.IsSelected = true;
                                grid.BringRecordIntoView(drLeadBook);
                                return;
                            }

                            drAgent.IsSelected = true;
                            grid.BringRecordIntoView(drAgent);
                            return;
                        }

                        drBatch.IsSelected = true;
                        grid.BringRecordIntoView(drBatch);
                        return;
                    }

                    grid.Records[(int)indexCampaign].IsSelected = true;
                    grid.BringRecordIntoView(grid.Records[(int)indexCampaign]);
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void xdgAssignLeads_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Return)
            //{
            //    var xamDataGridControl = (XamDataGrid)sender;

            //    if (xamDataGridControl.ActiveRecord != null)
            //    {
            //        DataRecord currentRecord = (DataRecord)xamDataGridControl.ActiveRecord;
            //        DataRow drCurrentRecord = ((DataRowView)currentRecord.DataItem).Row;

            //        _currentRecord = currentRecord.Index;

            //        //UserDetailsScreen userDetailsScreen = new UserDetailsScreen(Convert.ToInt64(drCurrentRecord.ItemArray[0].ToString()));
            //        //ShowDialog(userDetailsScreen, new HRDialogWindow(userDetailsScreen));

            //        //xdgUsers.DataSource = null;
            //        LoadBatchData();
            //    }
            //}
        }


        private void xdgAssignLeads_RecordActivating(object sender, Infragistics.Windows.DataPresenter.Events.RecordActivatingEventArgs e)
        {
            Record record = e.Record;
            if (record.NestingDepth >= 2)
            {
                //btnShowLeads.IsEnabled = true;
            }
            else if (record.NestingDepth < 2)
            {
                //btnShowLeads.IsEnabled = false;
            }
        }

        private void xdgAssignLeads_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var xamDataGridControl = (XamDataGrid)sender;

            if (xamDataGridControl.ActiveRecord != null)
            {
                if (((FrameworkElement)e.MouseDevice.DirectlyOver).DataContext != null)
                {
                    if ((((FrameworkElement)e.MouseDevice.DirectlyOver).DataContext).GetType().FullName == "Infragistics.Windows.DataPresenter.DataRecord")
                    {
                        DataRecord currentRecord;
                        DataRow drCurrentRecord;
                        int indexCampaign, indexBatch, indexAgent, indexLeadbook;

                        switch (xamDataGridControl.ActiveRecord.FieldLayout.Description)
                        {
                            case "Campaign":

                                break;

                            case "Batch":
                                currentRecord = (DataRecord)xamDataGridControl.ActiveRecord;
                                drCurrentRecord = ((DataRowView)currentRecord.DataItem).Row;
                                indexCampaign = currentRecord.ParentDataRecord.Index;
                                indexBatch = currentRecord.Index;

                                //if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                                //{
                                //    AssignSingleLeadScreen assignSingleLeadScreen = new AssignSingleLeadScreen(Convert.ToInt64(drCurrentRecord.ItemArray[0].ToString()));
                                //    ShowDialog(assignSingleLeadScreen, new INDialogWindow(assignSingleLeadScreen));
                                //}
                                //else
                                //{
                                    AutoAssignLeadsScreen assignLeadsScreen = new AutoAssignLeadsScreen(Convert.ToInt64(drCurrentRecord.ItemArray[0].ToString()));
                                    ShowDialog(assignLeadsScreen, new INDialogWindow(assignLeadsScreen));
                                //}

                                LoadSummaryData();
                                GridLastView(xamDataGridControl, indexCampaign, indexBatch, null, null);
                                break;

                            case "Agent":
                                //    currentRecord = (DataRecord)xamDataGridControl.ActiveRecord;
                                //    drCurrentRecord = ((DataRowView)currentRecord.DataItem).Row;
                                //    indexCampaign = currentRecord.ParentDataRecord.ParentDataRecord.Index;
                                //    indexBatch = currentRecord.ParentDataRecord.Index;
                                //    indexAgent = currentRecord.Index;

                                //    PrintLeadsScreen printLeadsScreen = new PrintLeadsScreen(Convert.ToInt64(drCurrentRecord.ItemArray[1].ToString()), Convert.ToInt64(drCurrentRecord.ItemArray[2].ToString()));
                                //    ShowDialog(printLeadsScreen, new INDialogWindow(printLeadsScreen));

                                //    LoadSummaryData();
                                //    GridLastView(xamDataGridControl, indexCampaign, indexBatch, indexAgent);
                                break;

                            case "LeadBook":
                                currentRecord = (DataRecord)xamDataGridControl.ActiveRecord;
                                drCurrentRecord = ((DataRowView)currentRecord.DataItem).Row;
                                indexCampaign = currentRecord.ParentDataRecord.ParentDataRecord.ParentDataRecord.Index;
                                indexBatch = currentRecord.ParentDataRecord.ParentDataRecord.Index;
                                indexAgent = currentRecord.ParentDataRecord.Index;
                                indexLeadbook = currentRecord.Index;

                                AssignMiningLeadsScreen assignMiningLeadsScreen = new AssignMiningLeadsScreen(Convert.ToInt64(drCurrentRecord.ItemArray[3].ToString()));
                                ShowDialog(assignMiningLeadsScreen, new INDialogWindow(assignMiningLeadsScreen));

                                LoadSummaryData();
                                GridLastView(xamDataGridControl, indexCampaign, indexBatch, indexAgent, indexLeadbook);
                                break;
                        }
                    }
                }
            }
        }

        private void btnReport_Click_1(object sender, RoutedEventArgs e)
        {
            if (GlobalSettings.ApplicationUser.ID == 1
                || GlobalSettings.ApplicationUser.ID == 427)
            {


                SMSSendData smsData = new SMSSendData();
                SM smsResponseDatas = new SM();
                System.Data.DataTable Dt;
                try
                {


                    #region Get Data from the database

                    try
                    {
                        #region Ensuring that at least one campaign was selected

                        var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                        _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

                        _fkINCampaignFKINCampaignClusterIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                        _fkINCampaignFKINCampaignClusterIDs = _fkINCampaignFKINCampaignClusterIDs.Substring(0, _fkINCampaignFKINCampaignClusterIDs.Length - 1);

                        if (_lstSelectedCampaigns.Count == 0)
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No campaigns selected", ShowMessageType.Error);
                        }
                        else
                        {
                            _fkINCampaignFKINCampaignClusterIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                            _fkINCampaignFKINCampaignClusterIDs = _fkINCampaignFKINCampaignClusterIDs.Substring(0, _fkINCampaignFKINCampaignClusterIDs.Length - 1);
                        }

                        #endregion Ensuring that at least one campaign was selected

                        dsAutoAssignLeadsData = Business.Insure.INGetNoContactBulkSMSLookups(DateTime.Parse("2023-04-01 00:00:00"), DateTime.Parse("2023-08-16 23:00:00"), _fkINCampaignFKINCampaignClusterIDs);


                        if (AllLeadsCB.IsChecked == true)
                        {
                            Dt = dsAutoAssignLeadsData.Tables[0];
                        }
                        else
                        {
                            Dt = dsAutoAssignLeadsData.Tables[1];
                        }


                    }
                    catch { Dt = null; }

                    List<long?> ImportIDs = new List<long?>();
                    foreach (DataRow row in Dt.Rows)
                    {
                        ImportIDs.Add(long.Parse(row["ID"].ToString()));
                    }

                    int Count = 0;
                    int Total = ImportIDs.Count();


                    foreach (var idimport in ImportIDs)
                    {
                        Count++;

                        Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                        {
                            //MainBorder.BorderBrush = Brushes.Green;
                            try { CounterLbl.Content = Count.ToString() + "/" + Total.ToString(); } catch { }
                        }));



                        string strQuery;
                        strQuery = "SELECT L.TelCell, L.TelHome, L.TelOther, L.TelWork FROM INImport AS I LEFT JOIN INLead AS L ON I.FKINLeadID = L.ID WHERE I.ID =  " + idimport;

                        System.Data.DataTable dtNumberDetails = Methods.GetTableData(strQuery);

                        string cell;
                        string work;
                        string home;
                        string other;



                        try { cell = dtNumberDetails.Rows[0]["TelCell"].ToString(); } catch { cell = ""; }
                        try { home = dtNumberDetails.Rows[0]["TelHome"].ToString(); } catch { home = ""; }
                        try { other = dtNumberDetails.Rows[0]["TelOther"].ToString(); } catch { other = ""; }
                        try { work = dtNumberDetails.Rows[0]["TelWork"].ToString(); } catch { work = ""; }

                        string Tel1MobileTB = cell ?? home ?? other ?? work;
                        //string Tel1MobileTB = "0847f76";
                        #endregion

                        smsData.to = Tel1MobileTB.Trim();
                        if (smsData.to.StartsWith("0"))
                        {
                            if (smsData.to.Contains("-"))
                            {
                                var removeZero = Tel1MobileTB.TrimStart('0').Replace("-", "");
                                smsData.to = "+27" + removeZero.Trim();
                            }
                            else
                            {
                                var removeZero = Tel1MobileTB.TrimStart('0');
                                smsData.to = "+27" + removeZero.Trim().Replace(" ", "");
                            }
                        }
                        else
                        {
                            if (smsData.to.Contains("-"))
                            {
                                smsData.to = "+" + Tel1MobileTB.Trim().Replace("-", "");
                            }
                            else
                            {
                                smsData.to = "+" + Tel1MobileTB.Trim().Replace(" ", "");
                            }
                        }

                        try
                        {
                            string Message = $"Believe you can and you are halfway there. \nTheodore Roosevelt.";
                            smsData.body = Message;
                            string sms = JsonConvert.SerializeObject(smsData);
                            //string sms = "{to: \"+27765389999\", body:\"Testing2\"}";
                            var request = WebRequest.Create(bulkSMSURL + "/messages");

                            request.Credentials = new NetworkCredential(bulkSMSUserName, bulkSMSPassword);
                            request.PreAuthenticate = true;
                            request.Method = "POST";
                            request.ContentType = "application/json";


                            #region WriteSMS
                            var encoding = new UnicodeEncoding();
                            var encodedData = encoding.GetBytes(sms);
                            var stream = request.GetRequestStream();
                            stream.Write(encodedData, 0, encodedData.Length);
                            stream.Close();
                            #endregion WriteSMS

                            var response = request.GetResponse();
                            var reader = new StreamReader(response.GetResponseStream());

                            #region SMS Response

                            string smsResponseJson = reader.ReadToEnd();
                            SMSTracker smsResponseData = new SMSTracker();
                            List<SMSResponse> smsResponses = JsonConvert.DeserializeObject<List<SMSResponse>>(smsResponseJson);
                            SMSResponse smsResponse = smsResponses[0];

                            smsResponseData.FKSystemID = (long?)lkpSystem.Insurance;
                            smsResponseData.FKImportID = idimport;

                            smsResponseData.SMSID = smsResponse.id;
                            smsResponseData.FKlkpSMSTypeID = (long?)smsResponse.type;
                            smsResponseData.RecipientCellNum = smsResponse.to;
                            smsResponseData.SMSBody = smsResponse.body;
                            smsResponseData.FKlkpSMSEncodingID = (long?)smsResponse.encoding;
                            smsResponseData.SubmissionID = smsResponse.submission.id;
                            smsResponseData.SubmissionDate = smsResponse.submission.date.AddHours(2); //for some reason the time given by bulk sms is 2 hours behind that's why I add 2 hours
                            smsResponseData.FKlkpSMSStatusTypeID = (long?)smsResponse.status.type;
                            smsResponseData.FKlkpSMSStatusSubtypeID = (long?)smsResponse.status.subtype;

                            smsResponseData.Save(_validationResult);


                            SMSData sMSData = new SMSData();
                            sMSData.SMSStatusTypeID = (WPF.Enumerations.Insure.lkpSMSStatusType?)smsResponseDatas.FKlkpSMSStatusTypeID;
                            sMSData.SMSStatusSubtypeID = (WPF.Enumerations.Insure.lkpSMSStatusSubtype?)smsResponseDatas.FKlkpSMSStatusSubtypeID;
                            sMSData.SMSSubmissionDate = Convert.ToDateTime(smsResponseDatas.SubmissionDate);

                            CheckSMSSent();
                        }
                        catch { }

                    }
                }

                #endregion SMS Response



                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }
        private void CheckSMSSent()
        {
            //string smsStatusTypeString = LaData.SMSData.SMSStatusTypeID == null ? "" : Enum.GetName(typeof(WPF.Enumerations.Insure.lkpSMSStatusType), LaData.SMSData.SMSStatusTypeID);
            //string smsStatusSubtypeString = LaData.SMSData.SMSStatusSubtypeID == null ? "" : Enum.GetName(typeof(WPF.Enumerations.Insure.lkpSMSStatusSubtype), LaData.SMSData.SMSStatusSubtypeID);
            //LaData.SMSData.SMSToolTip = (smsStatusSubtypeString == "" ? "SENT" : smsStatusSubtypeString) + " " + LaData.SMSData.SMSSubmissionDate;
            //LaData.SMSData.IsSMSSent = smsStatusTypeString != "FAILED" && smsStatusTypeString != "" && smsStatusTypeString != null ? true : false;
        }
        private void btnStats_Click(object sender, RoutedEventArgs e)
        {
            xdgAssignLeads.Visibility = Visibility.Collapsed;

            LeadsDatagrid.Visibility = Visibility.Visible;
            btnReport.Visibility = Visibility.Visible;
        }

        //private void btnReportBreakdown_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        SetCursor(Cursors.Wait);

        //        #region Get the report data
        //        System.Data.DataTable dtSalesData;
        //        System.Data.DataTable dtSalesData2;
        //        System.Data.DataTable dtSalesData3;
        //        System.Data.DataTable dtSalesData4;


        //        StringBuilder strQueryFKUserFromHistory = new StringBuilder();
        //        strQueryFKUserFromHistory.Append("SELECT [FKUserID] ");
        //        strQueryFKUserFromHistory.Append("FROM [HistoryAgeLanguage] ");
        //        //strQuery.Append(" ORDER BY ID ASC");    
        //        System.Data.DataTable dtFKUserFromHistory = Methods.GetTableData(strQueryFKUserFromHistory.ToString());

        //        List<long?> FKUserIDList = dtFKUserFromHistory.AsEnumerable().Select(row => row.Field<long?>("FKUserID")).ToList();
        //        string filePathAndName = "";
        //        var excelApp = new Microsoft.Office.Interop.Excel.Application();
        //        excelApp.Workbooks.Add();
        //        int SheetCount = 0;
        //        foreach (var item in FKUserIDList)
        //        {
        //            SheetCount++;
        //            dsDebiCheckSpecialistLogsData = Business.Insure.INReportAutoAssignBreakdown(item);
        //            dtSalesData = dsDebiCheckSpecialistLogsData.Tables[0];
        //            dtSalesData2 = dsDebiCheckSpecialistLogsData.Tables[1];
        //            dtSalesData3 = dsDebiCheckSpecialistLogsData.Tables[2];
        //            dtSalesData4 = dsDebiCheckSpecialistLogsData.Tables[3];


        //            try
        //            {
        //                string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


        //                filePathAndName = String.Format("{0}Optimum Pair Breakdown, {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));


        //                if (dtSalesData == null || dtSalesData.Columns.Count == 0)
        //                    throw new Exception("ExportToExcel: Null or empty input table!\n");

        //                // load excel, and create a new workbook


        //                Microsoft.Office.Interop.Excel._Worksheet workSheet;
        //                // single worksheet
        //                if (SheetCount == 0)
        //                {
        //                     workSheet = excelApp.ActiveSheet;

        //                }
        //                else
        //                {
        //                     workSheet = excelApp.Worksheets.Add();
        //                }

        //                StringBuilder strQueryFKUserName = new StringBuilder();
        //                strQueryFKUserName.Append("SELECT U.FirstName + ' ' + U.LastName ");
        //                strQueryFKUserName.Append("FROM [Insure].[dbo].[User] as [U] where U.ID =" + item.ToString());
        //                //strQuery.Append(" ORDER BY ID ASC");    
        //                System.Data.DataTable dtFKUserName = Methods.GetTableData(strQueryFKUserName.ToString());
        //                workSheet.Name = dtFKUserName.Rows[0][0].ToString();


        //                workSheet.Cells[1, 0 + 1] = "Breakdown";

        //                int countTable2 = 0;
        //                int countTable3 = 0;
        //                int countTable4 = 0;

        //                for (var i = 0; i < dtSalesData.Columns.Count; i++)
        //                {


        //                    workSheet.Cells[2, i + 1].Font.Bold = true;
        //                    workSheet.Cells[2, i + 1].ColumnWidth = 30;
        //                    workSheet.Cells[2, i + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        //                    workSheet.Cells[2, i + 1] = dtSalesData.Columns[i].ColumnName;
        //                    workSheet.Cells[2, i + 1].WrapText = true;
        //                    workSheet.Cells[2, i + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
        //                    workSheet.Cells[2, i + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
        //                    //workSheet.get_Range("A4", "J1").Font.Bold = true;
        //                }


        //                // column headings
        //                for (var i = 0; i < dtSalesData.Columns.Count; i++)
        //                {
        //                    workSheet.Cells[2, i + 1] = dtSalesData.Columns[i].ColumnName;
        //                }

        //                // rows
        //                for (var i = 1; i < dtSalesData.Rows.Count + 1; i++)
        //                {
        //                    // to do: format datetime values before printing
        //                    for (var j = 0; j < dtSalesData.Columns.Count; j++)
        //                    {
        //                        workSheet.Cells[i + 2, j + 1] = dtSalesData.Rows[i - 1][j];
        //                    }

        //                    countTable2++;
        //                    countTable3++;
        //                    countTable4++;
        //                }

        //                // Adding rows from second table
        //                for (var i =  1; i < (dtSalesData2.Rows.Count + 1); i++)
        //                {
        //                    // to do: format datetime values before printing
        //                    for (var j = 0; j < dtSalesData2.Columns.Count; j++)
        //                    {
        //                        workSheet.Cells[(i + 2 + countTable2), j + 1] = dtSalesData2.Rows[i - 1][j];
        //                    }
        //                    countTable3++;
        //                    countTable4++;
        //                }

        //                // Adding rows from Third table
        //                for (var i = 1; i < (dtSalesData3.Rows.Count + 1); i++)
        //                {
        //                    // to do: format datetime values before printing
        //                    for (var j = 0; j < dtSalesData3.Columns.Count; j++)
        //                    {
        //                        workSheet.Cells[(i + 2 + countTable3), j + 1] = dtSalesData3.Rows[i - 1][j];
        //                    }
        //                    countTable4++;
        //                }

        //                // Adding rows from fourth table
        //                for (var i = 1; i < (dtSalesData4.Rows.Count + 1); i++)
        //                {
        //                    // to do: format datetime values before printing
        //                    for (var j = 0; j < dtSalesData4.Columns.Count; j++)
        //                    {
        //                        //if()
        //                        //{

        //                        //}
        //                        workSheet.Cells[(i + 2 + countTable4), j + 1] = dtSalesData4.Rows[i - 1][j];
        //                    }
        //                }


        //                workSheet.Cells[7, 3].Formula = string.Format("=100-C3-C4-C5-C6");
        //                (workSheet.Cells[1, 3]).EntireColumn.NumberFormat = "00,00%";

        //                // check file path


        //                //excelApp.Save(filePathAndName);
        //                ////Process.Start(filePathAndName);

        //                //excelApp.Workbooks.

        //            }
        //            catch (Exception ex)
        //            {

        //            }

        //        }

        //        excelApp.Visible = true;
        //        excelApp.Workbooks.Item[1].SaveAs(filePathAndName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);


        //        #endregion Get the report data


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

        private void btnPrintLeads_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Get the report data
                System.Data.DataTable dtSalesData;
                System.Data.DataTable Dt;

                try
                {
                    #region Ensuring that at least one campaign was selected

                    var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                    _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

                    _fkINCampaignFKINCampaignClusterIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                    _fkINCampaignFKINCampaignClusterIDs = _fkINCampaignFKINCampaignClusterIDs.Substring(0, _fkINCampaignFKINCampaignClusterIDs.Length - 1);

                    if (_lstSelectedCampaigns.Count == 0)
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No campaigns selected", ShowMessageType.Error);
                    }
                    else
                    {
                        _fkINCampaignFKINCampaignClusterIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                        _fkINCampaignFKINCampaignClusterIDs = _fkINCampaignFKINCampaignClusterIDs.Substring(0, _fkINCampaignFKINCampaignClusterIDs.Length - 1);
                    }

                    #endregion Ensuring that at least one campaign was selected

                    dsAutoAssignLeadsData = Business.Insure.INGetNoContactBulkSMSLookups(DateTime.Parse("2023-04-01 00:00:00"), DateTime.Parse("2023-08-16 23:00:00"), _fkINCampaignFKINCampaignClusterIDs);
                    if (AllLeadsCB.IsChecked == true)
                    {
                        Dt = dsAutoAssignLeadsData.Tables[0];
                    }
                    else
                    {
                        Dt = dsAutoAssignLeadsData.Tables[1];
                    }
                }
                catch 
                {
                    dsAutoAssignLeadsData = Business.Insure.INGetNoContactBulkSMSLookups(DateTime.Parse("2023-04-01 00:00:00"), DateTime.Parse("2023-08-16 23:00:00"), _fkINCampaignFKINCampaignClusterIDs);
                    Dt = dsAutoAssignLeadsData.Tables[0]; 
                }

                dtSalesData = Dt;


                #endregion Get the report data

                try
                {
                    string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);



                    string filePathAndName = String.Format("{0}No Contact Report, {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
                    if (dtSalesData == null || dtSalesData.Columns.Count == 0)
                        throw new Exception("ExportToExcel: Null or empty input table!\n");

                    // load excel, and create a new workbook
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Workbooks.Add();


                    // single worksheet
                    Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;
                    workSheet.Name = "No Contact Leads";

                    workSheet.Cells[1, 0 + 1] = "202308.3";
                    for (var i = 0; i < dtSalesData.Columns.Count; i++)
                    {

                        workSheet.Cells[2, i + 1].Font.Bold = true;
                        workSheet.Cells[2, i + 1].ColumnWidth = 20;
                        workSheet.Cells[2, i + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        //workSheet.get_Range("A4", "J1").Font.Bold = true;
                    }


                    // column headings
                    for (var i = 0; i < dtSalesData.Columns.Count; i++)
                    {
                        workSheet.Cells[2, i + 1] = dtSalesData.Columns[i].ColumnName;
                    }

                    // rows
                    for (var i = 1; i < dtSalesData.Rows.Count + 1; i++)
                    {
                        // to do: format datetime values before printing
                        for (var j = 0; j < dtSalesData.Columns.Count; j++)
                        {
                            workSheet.Cells[i + 2, j + 1] = dtSalesData.Rows[i - 1][j];
                        }
                    }





                    // check file path

                    excelApp.Visible = true;
                    excelApp.Workbooks.Item[1].SaveAs(filePathAndName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    //excelApp.Save(filePathAndName);
                    ////Process.Start(filePathAndName);

                    //excelApp.Workbooks.

                }
                catch (Exception ex)
                {

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
        private void cmbSalaryReportType_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbSalaryReportType.SelectedValue != null)
            {
                //LoadLookupValues();
                //Changes... If the user selects Upgrades then disable group by textbox
                //Pheko
            }
        }

        private void CampaignsHeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _xdgCampaignsHeaderPrefixAreaCheckbox = (System.Windows.Controls.CheckBox)sender;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void cmbSalaryReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRow drSelectedSalaryReportType = (cmbSalaryReportType.SelectedItem as DataRowView).Row;
            PopulateDataGrids(drSelectedSalaryReportType);
        }

        private void CampaignsHeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Data.DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = true;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            try 
            {
                #region Ensuring that at least one campaign was selected

                var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

                _fkINCampaignFKINCampaignClusterIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                _fkINCampaignFKINCampaignClusterIDs = _fkINCampaignFKINCampaignClusterIDs.Substring(0, _fkINCampaignFKINCampaignClusterIDs.Length - 1);

                if (_lstSelectedCampaigns.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No campaigns selected", ShowMessageType.Error);
                }
                else
                {
                    _fkINCampaignFKINCampaignClusterIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                    _fkINCampaignFKINCampaignClusterIDs = _fkINCampaignFKINCampaignClusterIDs.Substring(0, _fkINCampaignFKINCampaignClusterIDs.Length - 1);
                }

                #endregion Ensuring that at least one campaign was selected

                dsAutoAssignLeadsData = Business.Insure.INGetNoContactBulkSMSLookups(DateTime.Parse("2023-04-01 00:00:00"), DateTime.Parse("2023-08-16 23:00:00"), _fkINCampaignFKINCampaignClusterIDs);

                System.Data.DataTable Dt;
                if (AllLeadsCB.IsChecked == true)
                {
                    Dt = dsAutoAssignLeadsData.Tables[0];
                }
                else
                {
                    Dt = dsAutoAssignLeadsData.Tables[1];
                }

                LeadsDatagrid.ItemsSource = Dt.DefaultView;
            }
            catch { }
        }
        private void CampaignClustersHeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Data.DataTable dt = ((DataView)xdgCampaignClusters.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = true;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void CampaignsRecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_xdgCampaignsHeaderPrefixAreaCheckbox != null)
                {
                    _xdgCampaignsHeaderPrefixAreaCheckbox.IsChecked = AllCampaignRecordsSelected();
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            try
            {
                #region Ensuring that at least one campaign was selected

                var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

                _fkINCampaignFKINCampaignClusterIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                _fkINCampaignFKINCampaignClusterIDs = _fkINCampaignFKINCampaignClusterIDs.Substring(0, _fkINCampaignFKINCampaignClusterIDs.Length - 1);

                if (_lstSelectedCampaigns.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No campaigns selected", ShowMessageType.Error);
                }
                else
                {
                    _fkINCampaignFKINCampaignClusterIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                    _fkINCampaignFKINCampaignClusterIDs = _fkINCampaignFKINCampaignClusterIDs.Substring(0, _fkINCampaignFKINCampaignClusterIDs.Length - 1);
                }

                #endregion Ensuring that at least one campaign was selected

                dsAutoAssignLeadsData = Business.Insure.INGetNoContactBulkSMSLookups(DateTime.Parse("2023-04-01 00:00:00"), DateTime.Parse("2023-08-16 23:00:00"), _fkINCampaignFKINCampaignClusterIDs);
                System.Data.DataTable Dt;
                if (AllLeadsCB.IsChecked == true)
                {
                    Dt = dsAutoAssignLeadsData.Tables[0];
                }
                else
                {
                    Dt = dsAutoAssignLeadsData.Tables[1];
                }
                LeadsDatagrid.ItemsSource = Dt.DefaultView;
            }
            catch { }
        }
        public enum SalaryReportType
        {
            Base = 1,
            Upgrade = 2
        }

        private void LoadLookupValues()
        {
            try
            {
                SetCursor(Cursors.Wait);

                _dsLookups = Insure.INGetSalaryReportScreenLookups();
                cmbSalaryReportType.Populate(_dsLookups.Tables[0], "Description", "ID");

                if (cmbSalaryReportType.Text.Trim() != "")
                {
                    switch (cmbSalaryReportType.Text)
                    {
                        case "Base, Rejuvenation, Defrost and Funeral":
                            _salaryReportType = SalaryReportType.Base;
                            tbBonusSales.Visibility = Visibility.Hidden;
                            chkBonusSales.Visibility = Visibility.Hidden;
                            chkBonusSales.IsChecked = false;
                            _bonusSales = false;
                            break;

                        case "Upgrades":
                            _salaryReportType = SalaryReportType.Upgrade;
                            tbBonusSales.Visibility = Visibility.Visible;
                            chkBonusSales.Visibility = Visibility.Visible;
                            chkBonusSales.IsChecked = false;
                            _bonusSales = false;
                            break;
                    }
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

        private void CampaignClustersHeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _xdgCampaignClusterssHeaderPrefixAreaCheckbox = (System.Windows.Controls.CheckBox)sender;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void CampaignClustersRecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_xdgCampaignClusterssHeaderPrefixAreaCheckbox != null)
                {
                    _xdgCampaignClusterssHeaderPrefixAreaCheckbox.IsChecked = AllCampaignClusterRecordsSelected();
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private bool? AllCampaignClusterRecordsSelected()
        {
            try
            {
                bool allSelected = true;
                bool noneSelected = true;

                if (xdgCampaigns.DataSource != null)
                {
                    foreach (DataRow dr in ((DataView)xdgCampaignClusters.DataSource).Table.Rows)
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
        private void CampaignClustersHeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Data.DataTable dt = ((DataView)xdgCampaignClusters.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = false;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private bool? AllCampaignRecordsSelected()
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
        private void CampaignsHeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Data.DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = false;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void PopulateDataGrids(DataRow drSelectedSalaryReportType)
        {

            string filterString = drSelectedSalaryReportType["FilterString"].ToString();
            string orderByStringCampaigns = drSelectedSalaryReportType["OrderByStringCampaigns"].ToString();
            string orderByStringCampaignClusters = drSelectedSalaryReportType["OrderByStringCampaignClusters"].ToString();

            #region Loading the campaigns

            var filteredCampaigns = _dsLookups.Tables[1].Select(filterString).AsEnumerable();
            if (filteredCampaigns.Any())
            {
                System.Data.DataTable dtCampaigns = _dsLookups.Tables[1].Select(filterString, orderByStringCampaigns).CopyToDataTable();
                xdgCampaigns.DataSource = dtCampaigns.DefaultView;
            }

            var campaignsHeaderCheckbox = Methods.FindChild<System.Windows.Controls.CheckBox>(xdgCampaigns, "CampaignsRecordSelectorCheckbox");
            if (campaignsHeaderCheckbox != null)
            {
                campaignsHeaderCheckbox.IsChecked = false;
            }

            #endregion Loading the campaigns

            #region Loading the campaign clusters

            var filteredCampaignsClusters = _dsLookups.Tables[2].Select(filterString).AsEnumerable();
            if (filteredCampaignsClusters.Any())
            {
                System.Data.DataTable dtCampaignClusters = _dsLookups.Tables[2].Select(filterString, orderByStringCampaignClusters).CopyToDataTable();
                xdgCampaignClusters.DataSource = dtCampaignClusters.DefaultView;
            }

            var campaignClustersHeaderCheckbox = Methods.FindChild<System.Windows.Controls.CheckBox>(xdgCampaignClusters, "CampaignClustersHeaderPrefixAreaCheckbox");
            if (campaignClustersHeaderCheckbox != null)
            {
                campaignClustersHeaderCheckbox.IsChecked = false;
            }

            #endregion Loading the campaign clusters  
        }

        private void AllLeadsCB_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                #region Ensuring that at least one campaign was selected

                var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

                _fkINCampaignFKINCampaignClusterIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                _fkINCampaignFKINCampaignClusterIDs = _fkINCampaignFKINCampaignClusterIDs.Substring(0, _fkINCampaignFKINCampaignClusterIDs.Length - 1);

                if (_lstSelectedCampaigns.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No campaigns selected", ShowMessageType.Error);
                }
                else
                {
                    _fkINCampaignFKINCampaignClusterIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                    _fkINCampaignFKINCampaignClusterIDs = _fkINCampaignFKINCampaignClusterIDs.Substring(0, _fkINCampaignFKINCampaignClusterIDs.Length - 1);
                }

                #endregion Ensuring that at least one campaign was selected

                dsAutoAssignLeadsData = Business.Insure.INGetNoContactBulkSMSLookups(DateTime.Parse("2023-04-01 00:00:00"), DateTime.Parse("2023-08-16 23:00:00"), _fkINCampaignFKINCampaignClusterIDs);
                System.Data.DataTable Dt;
                if (AllLeadsCB.IsChecked == true)
                {
                    Dt = dsAutoAssignLeadsData.Tables[0];
                }
                else
                {
                    Dt = dsAutoAssignLeadsData.Tables[1];
                }
                LeadsDatagrid.ItemsSource = Dt.DefaultView;
            }
            catch { }

        }

        private void AllLeadsCB_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                #region Ensuring that at least one campaign was selected

                var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

                _fkINCampaignFKINCampaignClusterIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                _fkINCampaignFKINCampaignClusterIDs = _fkINCampaignFKINCampaignClusterIDs.Substring(0, _fkINCampaignFKINCampaignClusterIDs.Length - 1);

                if (_lstSelectedCampaigns.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No campaigns selected", ShowMessageType.Error);
                }
                else
                {
                    _fkINCampaignFKINCampaignClusterIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                    _fkINCampaignFKINCampaignClusterIDs = _fkINCampaignFKINCampaignClusterIDs.Substring(0, _fkINCampaignFKINCampaignClusterIDs.Length - 1);
                }

                #endregion Ensuring that at least one campaign was selected

                dsAutoAssignLeadsData = Business.Insure.INGetNoContactBulkSMSLookups(DateTime.Parse("2023-04-01 00:00:00"), DateTime.Parse("2023-08-16 23:00:00"), _fkINCampaignFKINCampaignClusterIDs);
                System.Data.DataTable Dt;
                if (AllLeadsCB.IsChecked == true)
                {
                    Dt = dsAutoAssignLeadsData.Tables[0];
                }
                else
                {
                    Dt = dsAutoAssignLeadsData.Tables[1];
                }
                LeadsDatagrid.ItemsSource = Dt.DefaultView;
            }
            catch { }

        }
    }
    class SMSSendData
    {
        public string to { get; set; }
        public string body { get; set; }
    }
    public partial class SM
    {
        public long ID { get; set; }
        public Nullable<long> FKSystemID { get; set; }
        public Nullable<long> FKHRRecruitID { get; set; }
        public Nullable<long> FKlkpHRSMSTemplateID { get; set; }
        public string SMSID { get; set; }
        public Nullable<long> FKlkpSMSTypeID { get; set; }
        public string RecipientCellNum { get; set; }
        public string SMSBody { get; set; }
        public Nullable<long> FKlkpSMSEncodingID { get; set; }
        public string SubmissionID { get; set; }
        public Nullable<System.DateTime> SubmissionDate { get; set; }
        public Nullable<long> FKlkpSMSStatusTypeID { get; set; }
        public Nullable<long> FKlkpSMSStatusSubtypeID { get; set; }
        public Nullable<long> StampUserID { get; set; }
        public Nullable<System.DateTime> StampDate { get; set; }
    }
    public class SMSData
    {
        public WPF.Enumerations.Insure.lkpSMSStatusType? SMSStatusTypeID { get; set; }
        public WPF.Enumerations.Insure.lkpSMSStatusSubtype? SMSStatusSubtypeID { get; set; }
        public DateTime? SMSSubmissionDate { get; set; }
        public string SMSToolTip { get; set; }
        public bool? IsSMSSent { get; set; }
        public DateTime SMSExpiryDate { get; set; }

        public int? SMSCount { get; set; }
    }
}
