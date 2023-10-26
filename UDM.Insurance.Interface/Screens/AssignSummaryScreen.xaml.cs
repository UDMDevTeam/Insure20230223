using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Infragistics.Windows.DataPresenter;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;

namespace UDM.Insurance.Interface.Screens
{
    public partial class AssignSummaryScreen
    {

        #region Private Members

        private Stopwatch _swAssignSummaryScreenStartupTime = new Stopwatch();

        #endregion


        #region Constructors

        public AssignSummaryScreen()
        {
            InitializeComponent();

            _swAssignSummaryScreenStartupTime.Reset();
            _swAssignSummaryScreenStartupTime.Start();

            LoadSummaryData();

            #if TESTBUILD
                TestControl.Visibility = Visibility.Visible;
            #elif DEBUG
                DebugControl.Visibility = Visibility.Visible;
            #elif TRAININGBUILD
                TrainingControl.Visibility = Visibility.Visible;
            #endif

            _swAssignSummaryScreenStartupTime.Stop();
            ScreenTitle.ToolTip = Math.Round(_swAssignSummaryScreenStartupTime.Elapsed.TotalMilliseconds / 1000, 3) + " s";
            _swAssignSummaryScreenStartupTime.Reset();
        }

        #endregion Constructors


        #region Private Methods

        private void LoadSummaryData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataSet ds = Insure.INGetAssignLeadsSummaryData();

                DataRelation relCampaignBatch = new DataRelation("CampaignBatch", ds.Tables[0].Columns["CampaignID"], ds.Tables[1].Columns["CampaignID"]);
                ds.Relations.Add(relCampaignBatch);

                DataRelation relBatchAgent = new DataRelation("BatchAgent", ds.Tables[1].Columns["BatchID"], ds.Tables[2].Columns["BatchID"]);
                ds.Relations.Add(relBatchAgent);

                DataRelation relAgentLeadBook = new DataRelation("AgentLeadBook", new[] { ds.Tables[2].Columns["BatchID"], ds.Tables[2].Columns["UserID"] }, new[] { ds.Tables[3].Columns["BatchID"], ds.Tables[3].Columns["UserID"] }, false);
                ds.Relations.Add(relAgentLeadBook);

                DataRelation relLeadBookMiner = new DataRelation("LeadBookMiner", ds.Tables[3].Columns["LeadBookID"], ds.Tables[4].Columns["LeadBookID"]);
                ds.Relations.Add(relLeadBookMiner);
                DataView parentView = new DataView(ds.Tables[0]);

               
                List<DataRow> rowsToRemove = new List<DataRow>();

               
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow[] childRows = row.GetChildRows("CampaignBatch");
                    if (childRows.Length == 0)
                    {
                        rowsToRemove.Add(row);
                    }
                }

                // Remove the rows that have no children
                foreach (DataRow row in rowsToRemove)
                {
                    ds.Tables[0].Rows.Remove(row);
                }

             
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
                        grid.Records[(int) indexCampaign].IsExpanded = true;
                        DataRecord drBatch = (DataRecord)(((DataRecord)grid.Records[(int)indexCampaign]).ChildRecords[0].ViewableChildRecords[(int)indexBatch]);

                        if (indexAgent != null)
                        {
                            drBatch.IsExpanded = true;
                            DataRecord drAgent = (DataRecord)drBatch.ChildRecords[0].ViewableChildRecords[(int)indexAgent];

                            if (indexLeadBook !=null)
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

        #endregion Private Methods


        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MenuLeadScreen menuLeadScreen = new MenuLeadScreen(ScreenDirection.Reverse);
            OnClose(menuLeadScreen);
        }

        //private void btnShowLeads_Click(object sender, RoutedEventArgs e)
        //{

        //}

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

                                if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                                {
                                    AssignSingleLeadScreen assignSingleLeadScreen = new AssignSingleLeadScreen(Convert.ToInt64(drCurrentRecord.ItemArray[0].ToString()));
                                    ShowDialog(assignSingleLeadScreen, new INDialogWindow(assignSingleLeadScreen));
                                }
                                else
                                {
                                    AssignLeadsScreen assignLeadsScreen = new AssignLeadsScreen(Convert.ToInt64(drCurrentRecord.ItemArray[0].ToString()));
                                    ShowDialog(assignLeadsScreen, new INDialogWindow(assignLeadsScreen));
                                }

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

        #endregion Event Handlers

    }
}
