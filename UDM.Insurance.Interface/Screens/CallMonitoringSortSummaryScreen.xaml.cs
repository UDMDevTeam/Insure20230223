using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using Infragistics.Windows.DataPresenter;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;

namespace UDM.Insurance.Interface.Screens
{
    public partial class CallMonitoringSortSummaryScreen
    {

        #region Private Members



        #endregion


        #region Constructors

        public CallMonitoringSortSummaryScreen()
        {
            InitializeComponent();

            LoadSummaryData();

#if TESTBUILD
                TestControl.Visibility = Visibility.Visible;
#elif DEBUG
                DebugControl.Visibility = Visibility.Visible;
#endif
        }

        #endregion Constructors


        #region Private Methods

        private void LoadSummaryData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataSet ds = Insure.INGetCallMonitoringSortSummaryData();

                DataRelation relCampaignBatch = new DataRelation("Batch", ds.Tables[0].Columns["CampaignGroupType"], ds.Tables[1].Columns["CampaignGroupType"]);
                ds.Relations.Add(relCampaignBatch);

                DataColumn[] parentColumns = new DataColumn[] { ds.Tables[1].Columns["DateOfSale"], ds.Tables[1].Columns["CampaignGroupType"] };
                DataColumn[] childColumns = new DataColumn[] { ds.Tables[2].Columns["DateOfSale"], ds.Tables[2].Columns["CampaignGroupType"] };

                //DataRelation relBatchAgent = new DataRelation("BatchAgent", ds.Tables[1].Columns["DateOfSale"], ds.Tables[2].Columns["DateOfSale"]);
                DataRelation relBatchAgent = new DataRelation("BatchAgent", parentColumns, childColumns, false);
                ds.Relations.Add(relBatchAgent);

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

        private void GridLastView(XamDataGrid grid, int? indexCampaign, int? indexBatch, int? indexAgent)
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
            MenuToolsScreen menuToolScreen = new MenuToolsScreen(ScreenDirection.Reverse);
            OnClose(menuToolScreen);
        }

        //private void btnShowLeads_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void xdgAssignLeads_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
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
                            int indexCampaign, indexBatch; //, indexAgent

                            switch (xamDataGridControl.ActiveRecord.FieldLayout.Description)
                            {
                                case "Campaign":

                                    break;

                                case "Batch":
                                    currentRecord = (DataRecord)xamDataGridControl.ActiveRecord;
                                    drCurrentRecord = ((DataRowView)currentRecord.DataItem).Row;
                                    lkpINCampaignGroupType campaignGroupTypeEnum = (lkpINCampaignGroupType)Enum.Parse(typeof(lkpINCampaignGroupType), drCurrentRecord.ItemArray[4].ToString());
                                    long campaignGroupType = Convert.ToInt64(campaignGroupTypeEnum);
                                    indexCampaign = currentRecord.ParentDataRecord.Index;
                                    indexBatch = currentRecord.Index;

                                    //if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                                    //{
                                    //    AssignSingleLeadScreen assignSingleLeadScreen = new AssignSingleLeadScreen(Convert.ToInt64(drCurrentRecord.ItemArray[0].ToString()));
                                    //    ShowDialog(assignSingleLeadScreen, new INDialogWindow(assignSingleLeadScreen));
                                    //}
                                    //else
                                    //{
                                    AssignSalesScreen assignSalesScreen = new AssignSalesScreen(campaignGroupType, DateTime.Parse(drCurrentRecord.ItemArray[0].ToString())/*Convert.ToInt64(drCurrentRecord.ItemArray[0].ToString())*/);
                                    ShowDialog(assignSalesScreen, new INDialogWindow(assignSalesScreen));
                                    //}

                                    LoadSummaryData();
                                    GridLastView(xamDataGridControl, indexCampaign, indexBatch, null);
                                    break;

                                    //case "Agent":
                                    //    currentRecord = (DataRecord)xamDataGridControl.ActiveRecord;
                                    //    drCurrentRecord = ((DataRowView)currentRecord.DataItem).Row;
                                    //    indexCampaign = currentRecord.ParentDataRecord.ParentDataRecord.Index;
                                    //    indexBatch = currentRecord.ParentDataRecord.Index;
                                    //    indexAgent = currentRecord.Index;

                                    //    PrintLeadsScreen printLeadsScreen = new PrintLeadsScreen(Convert.ToInt64(drCurrentRecord.ItemArray[1].ToString()), Convert.ToInt64(drCurrentRecord.ItemArray[2].ToString()));
                                    //    ShowDialog(printLeadsScreen, new INDialogWindow(printLeadsScreen));

                                    //    LoadSummaryData();
                                    //    GridLastView(xamDataGridControl, indexCampaign, indexBatch, indexAgent);
                                    //    break;
                            }
                        }
                    }
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

        #endregion Event Handlers

    }
}
