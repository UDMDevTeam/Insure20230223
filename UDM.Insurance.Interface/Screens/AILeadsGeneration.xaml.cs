using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Windows.DataPresenter;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Services.Description;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class AILeadsGeneration
    {

        #region Constants

        DataSet dsDebiCheckSpecialistLogsData;
        DataSet dsAutoAssignLeadsData;


        #endregion Constants

        #region Private Members
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;
        private DateTime _startDate;
        private DateTime _endDate;
        #endregion

        public AILeadsGeneration()
        {
            InitializeComponent();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);

            //LoadDatagrid();
            //LoadSummaryData();

        }

        private void LoadDatagrid()
        {
            #region Datagrid 1
            dsAutoAssignLeadsData = Business.Insure.INGetAutoAssignLeadsScreenLookups(DateTime.Parse("2023-04-01 00:00:00"), DateTime.Parse("2023-08-16 23:00:00"));

            System.Data.DataTable Dt = dsAutoAssignLeadsData.Tables[0];

            LeadsDatagrid.ItemsSource = Dt.DefaultView;
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



        private void btnOverAllLanguage_Click(object sender, RoutedEventArgs e)
        {
            #region Datagrid 1
            try { dsAutoAssignLeadsData.Clear(); } catch { }
            dsAutoAssignLeadsData = Business.Insure.INGetAutoAssignLeadsScreenLookups(DateTime.Parse("2023-04-01 00:00:00"), DateTime.Parse("2023-08-16 23:00:00"));

            System.Data.DataTable Dt = dsAutoAssignLeadsData.Tables[2];

            LeadsDatagrid.ItemsSource = Dt.DefaultView;
            #endregion
        }

        private void btnOverAllAge_Click(object sender, RoutedEventArgs e)
        {
            #region Datagrid 1
            try { dsAutoAssignLeadsData.Clear(); } catch { }
            dsAutoAssignLeadsData = Business.Insure.INGetAutoAssignLeadsScreenLookups(DateTime.Parse("2023-04-01 00:00:00"), DateTime.Parse("2023-08-16 23:00:00"));

            System.Data.DataTable Dt = dsAutoAssignLeadsData.Tables[3];

            LeadsDatagrid.ItemsSource = Dt.DefaultView;
            #endregion
        }

        private void btnAVGLanguage_Click(object sender, RoutedEventArgs e)
        {
            #region Datagrid 1
            try { dsAutoAssignLeadsData.Clear(); } catch { }
            dsAutoAssignLeadsData = Business.Insure.INGetAutoAssignLeadsScreenLookups(DateTime.Parse("2023-04-01 00:00:00"), DateTime.Parse("2023-08-16 23:00:00"));

            System.Data.DataTable Dt = dsAutoAssignLeadsData.Tables[1];

            LeadsDatagrid.ItemsSource = Dt.DefaultView;
            #endregion
        }

        private void btnAVGAge_Click(object sender, RoutedEventArgs e)
        {
            #region Datagrid 1
            try { dsAutoAssignLeadsData.Clear(); } catch { }
            dsAutoAssignLeadsData = Business.Insure.INGetAutoAssignLeadsScreenLookups(DateTime.Parse("2023-04-01 00:00:00"), DateTime.Parse("2023-08-16 23:00:00"));

            System.Data.DataTable Dt = dsAutoAssignLeadsData.Tables[0];

            LeadsDatagrid.ItemsSource = Dt.DefaultView;
            #endregion
        }

        private void btnOverallContactNumbers_Click(object sender, RoutedEventArgs e)
        {
            #region Datagrid 1
            try { dsAutoAssignLeadsData.Clear(); } catch { }
            dsAutoAssignLeadsData = Business.Insure.INGetAutoAssignLeadsScreenLookups(DateTime.Parse("2023-04-01 00:00:00"), DateTime.Parse("2023-08-16 23:00:00"));

            System.Data.DataTable Dt = dsAutoAssignLeadsData.Tables[4];

            LeadsDatagrid.ItemsSource = Dt.DefaultView;
            #endregion
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
            xdgAssignLeads.Visibility= Visibility.Visible;
            btnOverAllLanguage.Visibility = Visibility.Collapsed;
            btnAVGLanguage.Visibility = Visibility.Collapsed;
            btnOverAllAge.Visibility = Visibility.Collapsed;
            btnAVGAge.Visibility = Visibility.Collapsed;
            LeadsDatagrid.Visibility = Visibility.Collapsed;
            btnOverallContactNumbers.Visibility= Visibility.Collapsed;
            btnAverageContactNumbers.Visibility = Visibility.Collapsed;
            btnReport.Visibility = Visibility.Collapsed;
            btnStats.Visibility = Visibility.Visible;
            btnReportBreakdown.Visibility= Visibility.Collapsed;

            LoadSummaryData();

        }

        private void btnStats_Click(object sender, RoutedEventArgs e)
        {
            xdgAssignLeads.Visibility = Visibility.Collapsed;
            btnOverAllLanguage.Visibility = Visibility.Visible;
            btnAVGLanguage.Visibility = Visibility.Visible;
            btnOverAllAge.Visibility = Visibility.Visible;
            btnAVGAge.Visibility = Visibility.Visible;
            btnOverallContactNumbers.Visibility = Visibility.Visible;
            btnAverageContactNumbers.Visibility = Visibility.Visible;
            LeadsDatagrid.Visibility = Visibility.Visible;
            btnReportBreakdown.Visibility = Visibility.Visible;
            btnReport.Visibility = Visibility.Visible;
            btnStats.Visibility = Visibility.Collapsed;
        }

        private void btnReportBreakdown_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Get the report data
                System.Data.DataTable dtSalesData;
                System.Data.DataTable dtSalesData2;
                System.Data.DataTable dtSalesData3;
                System.Data.DataTable dtSalesData4;


                StringBuilder strQueryFKUserFromHistory = new StringBuilder();
                strQueryFKUserFromHistory.Append("SELECT [FKUserID] ");
                strQueryFKUserFromHistory.Append("FROM [HistoryAgeLanguage] ");
                //strQuery.Append(" ORDER BY ID ASC");    
                System.Data.DataTable dtFKUserFromHistory = Methods.GetTableData(strQueryFKUserFromHistory.ToString());

                List<long?> FKUserIDList = dtFKUserFromHistory.AsEnumerable().Select(row => row.Field<long?>("FKUserID")).ToList();
                string filePathAndName = "";
                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Workbooks.Add();
                int SheetCount = 0;
                foreach (var item in FKUserIDList)
                {
                    SheetCount++;
                    dsDebiCheckSpecialistLogsData = Business.Insure.INReportAutoAssignBreakdown(item);
                    dtSalesData = dsDebiCheckSpecialistLogsData.Tables[0];
                    dtSalesData2 = dsDebiCheckSpecialistLogsData.Tables[1];
                    dtSalesData3 = dsDebiCheckSpecialistLogsData.Tables[2];
                    dtSalesData4 = dsDebiCheckSpecialistLogsData.Tables[3];


                    try
                    {
                        string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


                        filePathAndName = String.Format("{0}Optimum Pair Breakdown, {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));


                        if (dtSalesData == null || dtSalesData.Columns.Count == 0)
                            throw new Exception("ExportToExcel: Null or empty input table!\n");

                        // load excel, and create a new workbook


                        Microsoft.Office.Interop.Excel._Worksheet workSheet;
                        // single worksheet
                        if (SheetCount == 0)
                        {
                             workSheet = excelApp.ActiveSheet;

                        }
                        else
                        {
                             workSheet = excelApp.Worksheets.Add();
                        }

                        StringBuilder strQueryFKUserName = new StringBuilder();
                        strQueryFKUserName.Append("SELECT U.FirstName + ' ' + U.LastName ");
                        strQueryFKUserName.Append("FROM [Insure].[dbo].[User] as [U] where U.ID =" + item.ToString());
                        //strQuery.Append(" ORDER BY ID ASC");    
                        System.Data.DataTable dtFKUserName = Methods.GetTableData(strQueryFKUserName.ToString());
                        workSheet.Name = dtFKUserName.Rows[0][0].ToString();


                        workSheet.Cells[1, 0 + 1] = "Breakdown";

                        int countTable2 = 0;
                        int countTable3 = 0;
                        int countTable4 = 0;

                        for (var i = 0; i < dtSalesData.Columns.Count; i++)
                        {


                            workSheet.Cells[2, i + 1].Font.Bold = true;
                            workSheet.Cells[2, i + 1].ColumnWidth = 30;
                            workSheet.Cells[2, i + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                            workSheet.Cells[2, i + 1] = dtSalesData.Columns[i].ColumnName;
                            workSheet.Cells[2, i + 1].WrapText = true;
                            workSheet.Cells[2, i + 1].VerticalAlignment = XlVAlign.xlVAlignCenter;
                            workSheet.Cells[2, i + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
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

                            countTable2++;
                            countTable3++;
                            countTable4++;
                        }

                        // Adding rows from second table
                        for (var i =  1; i < (dtSalesData2.Rows.Count + 1); i++)
                        {
                            // to do: format datetime values before printing
                            for (var j = 0; j < dtSalesData2.Columns.Count; j++)
                            {
                                workSheet.Cells[(i + 2 + countTable2), j + 1] = dtSalesData2.Rows[i - 1][j];
                            }
                            countTable3++;
                            countTable4++;
                        }

                        // Adding rows from Third table
                        for (var i = 1; i < (dtSalesData3.Rows.Count + 1); i++)
                        {
                            // to do: format datetime values before printing
                            for (var j = 0; j < dtSalesData3.Columns.Count; j++)
                            {
                                workSheet.Cells[(i + 2 + countTable3), j + 1] = dtSalesData3.Rows[i - 1][j];
                            }
                            countTable4++;
                        }

                        // Adding rows from fourth table
                        for (var i = 1; i < (dtSalesData4.Rows.Count + 1); i++)
                        {
                            // to do: format datetime values before printing
                            for (var j = 0; j < dtSalesData4.Columns.Count; j++)
                            {
                                //if()
                                //{

                                //}
                                workSheet.Cells[(i + 2 + countTable4), j + 1] = dtSalesData4.Rows[i - 1][j];
                            }
                        }


                        workSheet.Cells[7, 3].Formula = string.Format("=100-C3-C4-C5-C6");
                        (workSheet.Cells[1, 3]).EntireColumn.NumberFormat = "00,00%";

                        // check file path


                        //excelApp.Save(filePathAndName);
                        ////Process.Start(filePathAndName);

                        //excelApp.Workbooks.

                    }
                    catch (Exception ex)
                    {

                    }

                }

                excelApp.Visible = true;
                excelApp.Workbooks.Item[1].SaveAs(filePathAndName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);


                #endregion Get the report data


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
    }
}
