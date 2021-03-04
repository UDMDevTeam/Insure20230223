using System;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using Infragistics.Windows.DataPresenter;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;
using System.Linq;
using UDM.WPF.Library;
using System.Collections.Generic;

namespace UDM.Insurance.Interface.Screens
{
    public partial class BumpUpSortSummaryScreen
    {

        #region Private Members

        BackgroundWorker worker;

        DataTable dtBUAgents = new DataTable();
        DataSet dsBUSummary = new DataSet();
        DataTable dtBUSummary = new DataTable();
        #endregion


        #region Constructors

        public BumpUpSortSummaryScreen()
        {
            InitializeComponent();

            //Task<DataSet> t = new Task<DataSet>(new Action(LoadSummaryData));
            worker = new BackgroundWorker();
            worker.DoWork += LoadSummaryData;
            worker.RunWorkerCompleted += LoadDataCompleted;
            worker.RunWorkerAsync();
            
            //LoadSummaryData();

            #if TESTBUILD
            TestControl.Visibility = Visibility.Visible;
            #elif DEBUG
                DebugControl.Visibility = Visibility.Visible;
            #endif
        }

        #endregion Constructors


        #region Private Methods

        //public async Task DoWork()
        //{
        //    DataSet ds = await Task.FromResult<DataSet>(LoadSummaryData());
        //}

        private void LoadSummaryData(object sender, DoWorkEventArgs e)
        {
            //DataSet dsBUSummary = new DataSet();
            try
            {
                SetCursor(Cursors.Wait);

                //Task t =  new Task
                //DataSet ds = Insure.INGetCallMonitoringSortSummaryData();
                dsBUSummary = Insure.INGetBumpUpSortSummaryData();

                DataRelation relCampaignBatch = new DataRelation("Batch", dsBUSummary.Tables[0].Columns["CampaignGroup"], dsBUSummary.Tables[1].Columns["CampaignGroup"]);
                dsBUSummary.Relations.Add(relCampaignBatch);

                DataColumn[] parentColumns = new DataColumn[] { dsBUSummary.Tables[1].Columns["DateOfSale"], dsBUSummary.Tables[1].Columns["CampaignGroup"] };
                DataColumn[] childColumns = new DataColumn[] { dsBUSummary.Tables[2].Columns["DateOfSale"], dsBUSummary.Tables[2].Columns["CampaignGroup"]/*, ds.Tables[2].Columns["UserID"]*/ };
                DataColumn[] agentColumns = new DataColumn[] { dsBUSummary.Tables[2].Columns["DateOfSale"], dsBUSummary.Tables[2].Columns["CampaignGroup"], dsBUSummary.Tables[2].Columns["UserID"] };
                DataColumn[] unassignedLeadColumns = new DataColumn[] { dsBUSummary.Tables[3].Columns["DateOfSale"], dsBUSummary.Tables[3].Columns["CampaignGroup"] };
                DataColumn[] assignedLeadColumns = new DataColumn[] { dsBUSummary.Tables[4].Columns["DateOfSale"], dsBUSummary.Tables[4].Columns["CampaignGroup"], dsBUSummary.Tables[4].Columns["UserID"] };

                //DataRelation relBatchAgent = new DataRelation("BatchAgent", ds.Tables[1].Columns["DateOfSale"], ds.Tables[2].Columns["DateOfSale"]);
                DataRelation relBatchAgent = new DataRelation("Bump-Up Agents", parentColumns, childColumns, false);
                dsBUSummary.Relations.Add(relBatchAgent);               

                DataRelation relBatchUnassignedLeads = new DataRelation("Unassigned Leads", parentColumns, unassignedLeadColumns, false);
                dsBUSummary.Relations.Add(relBatchUnassignedLeads);

                DataRelation relBatchAssignedLeads = new DataRelation("Assigned Leads", agentColumns, assignedLeadColumns, false);
                dsBUSummary.Relations.Add(relBatchAssignedLeads);

                dtBUAgents = Methods.GetTableData("SELECT Blush.dbo.GetFullUserName(ID) [BumpUpAgent], [ID] [UserID] FROM [User] WHERE FKUserType = 3 AND IsActive = 1");
                
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SetCursor(Cursors.Arrow);
                
            }
            e.Result = dsBUSummary;
            //return ds;
        }

        private void LoadDataCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //DataSet ds = new DataSet();
            dsBUSummary = (DataSet)e.Result;
            dtBUSummary = dsBUSummary.Tables[0];
            xdgAssignLeads.DataSource = dtBUSummary.DefaultView;
            xdgAssignLeads.AutoFit = true;
        }

        private void GridLastView(XamDataGrid grid, int? indexCampaign, int? indexBatch, int? indexAgent)
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
                                    string campaignGroup = drCurrentRecord.ItemArray[4].ToString();
                                    string campaignIDs = drCurrentRecord.ItemArray[5].ToString();
                                    indexCampaign = currentRecord.ParentDataRecord.Index;
                                    indexBatch = currentRecord.Index;

                                    //if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                                    //{
                                    //    AssignSingleLeadScreen assignSingleLeadScreen = new AssignSingleLeadScreen(Convert.ToInt64(drCurrentRecord.ItemArray[0].ToString()));
                                    //    ShowDialog(assignSingleLeadScreen, new INDialogWindow(assignSingleLeadScreen));
                                    //}
                                    //else
                                    //{
                                    AssignPossibleBumpUpsScreen assignPossibleBumpUpsScreen = new AssignPossibleBumpUpsScreen(campaignGroup, DateTime.Parse(drCurrentRecord.ItemArray[0].ToString()), campaignIDs/*Convert.ToInt64(drCurrentRecord.ItemArray[0].ToString())*/);
                                        ShowDialog(assignPossibleBumpUpsScreen, new INDialogWindow(assignPossibleBumpUpsScreen));
                                    //}

                                    //LoadSummaryData();
                                    if (!worker.IsBusy)
                                    {
                                        worker.RunWorkerAsync();
                                    }
                                    
                                    GridLastView(xamDataGridControl, indexCampaign, indexBatch, null);
                                    break;

                                //case "Agent":
                                //    //currentRecord = (DataRecord)xamDataGridControl.ActiveRecord;
                                //    //drCurrentRecord = ((DataRowView)currentRecord.DataItem).Row;
                                //    //indexCampaign = currentRecord.ParentDataRecord.ParentDataRecord.Index;
                                //    //indexBatch = currentRecord.ParentDataRecord.Index;
                                //    //indexAgent = currentRecord.Index;

                                //    //PrintLeadsScreen printLeadsScreen = new PrintLeadsScreen(Convert.ToInt64(drCurrentRecord.ItemArray[1].ToString()), Convert.ToInt64(drCurrentRecord.ItemArray[2].ToString()));
                                //    //ShowDialog(printLeadsScreen, new INDialogWindow(printLeadsScreen));

                                //    //LoadSummaryData();
                                //    //GridLastView(xamDataGridControl, indexCampaign, indexBatch, indexAgent);
                                //    break;
                                //case "AssignedLeads":
                                //    break;
                                //case "UnassignedLeads":
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

        private void ContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (((MenuItem)sender).Header.ToString())
                {
                    //case "Assign":
                    //    if (xdgAssignLeads.SelectedItems.Records.Count == 1)
                    //    {
                    //        //DeleteLeaveRecord(Convert.ToInt64(((DataRowView)((DataRecord)((FrameworkElement)sender).DataContext).DataItem).Row.ItemArray[0]));
                    //        //currentRecord = (DataRecord)xamDataGridControl.ActiveRecord;
                    //        //drCurrentRecord = ((DataRowView)currentRecord.DataItem).Row;
                    //        //string campaignGroup = drCurrentRecord.ItemArray[4].ToString();
                    //        long importID = Convert.ToInt64(((DataRecord)xdgAssignLeads.ActiveRecord).Cells["ImportID"].Value);
                    //        long userID = Convert.ToInt64(((DataRecord)xdgAssignLeads.ActiveRecord).Cells["UserID"].Value);
                    //        AssignPossibleBumpUp(importID, userID);
                    //    }
                    //    break;
                    case "Unassign":
                        if (xdgAssignLeads.SelectedItems.Records.Count == 1)
                        {
                            //DeleteLeaveRecord(Convert.ToInt64(((DataRowView)((DataRecord)((FrameworkElement)sender).DataContext).DataItem).Row.ItemArray[0]));
                            //long importID = Convert.ToInt64(((DataRowView)((DataRecord)((FrameworkElement)e.Source).DataContext).DataItem).Row["ImportID"]);
                            long importID = Convert.ToInt64(((DataRecord)xdgAssignLeads.ActiveRecord).Cells["ImportID"].Value);                            
                            long possibleBumpUpAllocationID = Convert.ToInt64(((DataRecord)xdgAssignLeads.ActiveRecord).Cells["PossibleBumpUpAllocationID"].Value);
                            UnassignPossibleBumpUp(importID, possibleBumpUpAllocationID);
                        }
                        break;
                    default:
                        if (xdgAssignLeads.SelectedItems.Records.Count == 1)
                        {
                            //DeleteLeaveRecord(Convert.ToInt64(((DataRowView)((DataRecord)((FrameworkElement)sender).DataContext).DataItem).Row.ItemArray[0]));
                            //currentRecord = (DataRecord)xamDataGridControl.ActiveRecord;
                            //drCurrentRecord = ((DataRowView)currentRecord.DataItem).Row;
                            //string campaignGroup = drCurrentRecord.ItemArray[4].ToString();
                            long importID = Convert.ToInt64(((DataRecord)xdgAssignLeads.ActiveRecord).Cells["ImportID"].Value);
                            //long userID = Convert.ToInt64(((DataRecord)xdgAssignLeads.ActiveRecord).Cells["UserID"].Value);
                            long userID = Convert.ToInt64(((MenuItem)sender).Tag);
                            AssignPossibleBumpUp(importID, userID, ((MenuItem)sender).Header.ToString().Replace("Assign To ", ""));
                        }
                        break;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void AssignPossibleBumpUp(long importID, long userID, string bumpUpAgent)
        {
            try
            {
                Database.BeginTransaction(null, IsolationLevel.Snapshot);

                PossibleBumpUpAllocation possibleBumpUpAllocation = new PossibleBumpUpAllocation();
                possibleBumpUpAllocation.FKINImportID = importID;
                possibleBumpUpAllocation.FKUserID = userID;
                possibleBumpUpAllocation.Save(_validationResult);

                INImport inImport = new INImport(importID);
                inImport.FKConfCallRefUserID = userID;
                inImport.Save(_validationResult);

                CommitTransaction(null);

                #region Adjust total values in parent records.
                //Adjust total values in parent records.
                //int assignedBUA = Convert.ToInt32(((DataRecord)xdgAssignLeads.ActiveRecord).ParentDataRecord.Cells["AssignedPossibleBumpUps"].Value);

                //foreach( ExpandableFieldRecord dr in ((DataRecord)xdgAssignLeads.ActiveRecord).ParentDataRecord.ChildRecords)
                //{
                //    //((DataRowView)((DataRecord)xdgAssignLeads.ActiveRecord).ParentDataRecord.DataItem);

                //    //((DataRecord)xdgAssignLeads.ActiveRecord).ParentDataRecord.Cells["AssignedPossibleBumpUps"].Value = assignedBUA - 1;
                //    Console.WriteLine(dr.Field.Name);
                //    if (dr.Field.Name == "Bump-Up Agents")
                //    {
                //        DataRecord recordToUpdate = (DataRecord)dr.ChildRecords.Where(x => Convert.ToInt64(((DataRecord)x).Cells["UserID"].Value) == userID).FirstOrDefault();
                //        int assignedBUA = Convert.ToInt32(recordToUpdate.Cells["AssignedPossibleBumpUps"].Value);
                //        recordToUpdate.Cells["AssignedPossibleBumpUps"].Value = assignedBUA + 1;
                //    }

                //}

                int assignedDOS = Convert.ToInt32(((DataRecord)xdgAssignLeads.ActiveRecord).ParentDataRecord.Cells["AllocatedPossibleBumpUps"].Value);
                ((DataRecord)xdgAssignLeads.ActiveRecord).ParentDataRecord.Cells["AllocatedPossibleBumpUps"].Value = assignedDOS + 1;

                int unassignedDOS = Convert.ToInt32(((DataRecord)xdgAssignLeads.ActiveRecord).ParentDataRecord.Cells["UnAllocatedPossibleBumpUps"].Value);
                ((DataRecord)xdgAssignLeads.ActiveRecord).ParentDataRecord.Cells["UnAllocatedPossibleBumpUps"].Value = unassignedDOS - 1;
                #endregion Adjust total values in parent records.

                #region Move Lead from Unassigned to Assigned table
                //dsBUSummary.Tables[1].Rows[dsBUSummary.Tables[1].AsEnumerable().Where(x => x[""] == )]
                DataRecord drActiveLead = (DataRecord)xdgAssignLeads.ActiveRecord;
                DataRow drToAssign = dsBUSummary.Tables[3].Select("CampaignGroup='" + drActiveLead.Cells["CampaignGroup"].Value.ToString() + "' AND DateOfSale='" + drActiveLead.Cells["DateOfSale"].Value.ToString() + "'"/* + "' AND UserID=" + Convert.ToInt64(drActiveLead.Cells["UserID"].Value) + " AND PossibleBumpUpAllocationID=" + Convert.ToInt64(drActiveLead.Cells["PossibleBumpUpAllocationID"].Value)*/).FirstOrDefault();

                DataRow drToUpdate = dsBUSummary.Tables[2].Select("CampaignGroup='" + drActiveLead.Cells["CampaignGroup"].Value.ToString() + "' AND DateOfSale='" + drActiveLead.Cells["DateOfSale"].Value.ToString() + "' AND UserID=" + userID).FirstOrDefault();

                if (drToUpdate == null)
                {
                    DataRow drBatchRow = drToAssign.GetParentRow("Unassigned Leads");
                    //table.Rows.Add(57, "Koko", "Shar Pei", DateTime.Now);
                    drToUpdate = dsBUSummary.Tables[2].Rows.Add(drBatchRow["DateOfSale"], userID, bumpUpAgent, 0, drBatchRow["CampaignGroup"], drBatchRow["CampaignIDs"]);
                }
                int assignedBUA = Convert.ToInt32(drToUpdate["AssignedPossibleBumpUps"]);
                drToUpdate["AssignedPossibleBumpUps"] = assignedBUA + 1;
                /******************************************************************************************************/
                //drToAssign["PossibleBumpUpAllocationID"] = DBNull.Value;
                drToAssign["PossibleBumpUpAllocationID"] = possibleBumpUpAllocation.ID;
                drToAssign["UserID"] = userID;
                drToAssign["BumpUpAgent"] = bumpUpAgent;
                drToAssign["AssignedStatus"] = "Assigned";
                //dsBUSummary.Tables[3].Rows.Add(drToUnassign);
                dsBUSummary.Tables[4].ImportRow(drToAssign);
                dsBUSummary.Tables[3].Rows[dsBUSummary.Tables[3].Rows.IndexOf(drToAssign)].Delete();

                //DataRecord drBUAgent = ((DataRecord)xdgAssignLeads.ActiveRecord.ParentDataRecord);
                
                //dsSummary.TablesdsBUSummary.Tables[2].Rows.IndexOf(drToUpdate);
                //drToUpdate.Delete();
               
                #endregion Move Lead from Unassigned to Assigned table

                
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            
        }

        private void UnassignPossibleBumpUp(long importID, long possibleBumpUpAllocationID)
        {
            try
            {
                Database.BeginTransaction(null, IsolationLevel.Snapshot);

                INImport iNImport = new INImport(importID);
                iNImport.FKConfCallRefUserID = null;
                iNImport.Save(_validationResult);
                Database.ExecuteCommand("DELETE FROM PossibleBumpUpAllocation WHERE ID = " + possibleBumpUpAllocationID);

                CommitTransaction(null);

                #region Adjust total values in parent records.
                //Adjust total values in parent records.
                int assignedBUA = Convert.ToInt32(((DataRecord)xdgAssignLeads.ActiveRecord).ParentDataRecord.Cells["AssignedPossibleBumpUps"].Value);
                ((DataRecord)xdgAssignLeads.ActiveRecord).ParentDataRecord.Cells["AssignedPossibleBumpUps"].Value = assignedBUA - 1;

                int assignedDOS = Convert.ToInt32(((DataRecord)xdgAssignLeads.ActiveRecord).ParentDataRecord.ParentDataRecord.Cells["AllocatedPossibleBumpUps"].Value);
                ((DataRecord)xdgAssignLeads.ActiveRecord).ParentDataRecord.ParentDataRecord.Cells["AllocatedPossibleBumpUps"].Value = assignedDOS - 1;

                int unassignedDOS = Convert.ToInt32(((DataRecord)xdgAssignLeads.ActiveRecord).ParentDataRecord.ParentDataRecord.Cells["UnAllocatedPossibleBumpUps"].Value);
                ((DataRecord)xdgAssignLeads.ActiveRecord).ParentDataRecord.ParentDataRecord.Cells["UnAllocatedPossibleBumpUps"].Value = unassignedDOS + 1;

                //xdgAssignLeads.ExecuteCommand(DataPresenterCommands.DeleteSelectedDataRecords);


                #endregion Adjust total values in parent records.

                #region Move Lead from Assigned to Unassigned table
                //dsBUSummary.Tables[1].Rows[dsBUSummary.Tables[1].AsEnumerable().Where(x => x[""] == )]
                DataRecord drActiveLead = (DataRecord)xdgAssignLeads.ActiveRecord;
                DataRow drToUnassign = dsBUSummary.Tables[4].Select("CampaignGroup='" + drActiveLead.Cells["CampaignGroup"].Value.ToString() + "' AND DateOfSale='" + drActiveLead.Cells["DateOfSale"].Value.ToString() + "' AND UserID=" + Convert.ToInt64(drActiveLead.Cells["UserID"].Value) + " AND PossibleBumpUpAllocationID=" + Convert.ToInt64(drActiveLead.Cells["PossibleBumpUpAllocationID"].Value)).FirstOrDefault();
                drToUnassign["PossibleBumpUpAllocationID"] = DBNull.Value;
                drToUnassign["UserID"] = DBNull.Value;
                drToUnassign["BumpUpAgent"] = DBNull.Value;
                drToUnassign["AssignedStatus"] = "Unassigned";
                //dsBUSummary.Tables[3].Rows.Add(drToUnassign);
                dsBUSummary.Tables[3].ImportRow(drToUnassign);
                dsBUSummary.Tables[4].Rows[dsBUSummary.Tables[4].Rows.IndexOf(drToUnassign)].Delete();
                //DataRow drToUpdate = dsBUSummary.Tables[2].Select("CampaignGroup='" + drBUAgent.Cells["CampaignGroup"].Value.ToString() + "' AND DateOfSale='" + drBUAgent.Cells["DateOfSale"].Value.ToString() + "' AND UserID=" + Convert.ToInt64(drBUAgent.Cells["UserID"].Value)).FirstOrDefault();
                //dsBUSummary.Tables[]
                //dsSummary.TablesdsBUSummary.Tables[2].Rows.IndexOf(drToUpdate);
                //drToUpdate.Delete();
                //int assignedBUA = Convert.ToInt32(drToUpdate["AssignedPossibleBumpUps"]);
                //drToUpdate["AssignedPossibleBumpUps"] = assignedBUA - 1;
                #endregion Move Lead from Assigned to Unassigned table

                

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        private void XdgAssignLeads_ContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        //private void XdgAssignLeads_ContextMenuOpening(object sender, Infragistics.Controls.Menus.OpeningEventArgs e, System.Windows.Controls.ContextMenuEventArgs e2)
        {
            try
            {
                List<long> lstUserIDsWithAcccessToSingleAllocate;
                string setting = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 21", IsolationLevel.Snapshot).Rows[0][0].ToString();
                lstUserIDsWithAcccessToSingleAllocate = setting.Split(',').Select(long.Parse).ToList();
                var dataContext = ((FrameworkElement)e.OriginalSource).DataContext.GetType() == typeof(DataRecord) ? ((FrameworkElement)e.OriginalSource).DataContext : null;

                //check if Item4 is already there, this will probably run more than once
                

                if (dataContext != null && (((DataRecord)dataContext).FieldLayout.Key.ToString() == "AssignedLeads" || ((DataRecord)dataContext).FieldLayout.Key.ToString() == "UnassignedLeads") && ((DataRecord)dataContext).IsActive && dataContext.GetType().Name == "DataRecord" && lstUserIDsWithAcccessToSingleAllocate.Contains(GlobalSettings.ApplicationUser.ID))
                {

                    e.Handled = true; //need to suppress empty menu
                    //FrameworkElement fe = e.Source as FrameworkElement;
                    //fe.ContextMenu = BuildMenu();
                    //FlagForCustomContextMenu = true;
                    //fe.ContextMenu.IsOpen = true;

                    FrameworkElement fe = e.Source as FrameworkElement;
                    ContextMenu cm = fe.ContextMenu;

                    //foreach (MenuItem mi in cm.Items)
                    //{
                    //    if ((String)mi.Header == "Unassign")
                    //    {
                    //        //e.Handled = false;
                    //        cm.IsOpen = true;
                    //        return;
                    //    }

                    //}

                    //MenuItem miUnassign = new MenuItem();
                    //miUnassign.Header = "Unassign";
                    //fe.ContextMenu.Items.Add(miUnassign);
                    //cm.Items[0] = new MenuItem();
                    //((MenuItem)cm.Items[0]).Header = "Assign";

                    //foreach (MenuItem mi in cm.Items)
                    //{
                    int menuItemsCount = fe.ContextMenu.Items.Count;
                    if (((DataRecord)dataContext).FieldLayout.Key.ToString() == "AssignedLeads")
                    {
                        if ((String)((MenuItem)cm.Items[0]).Header == "Unassign")
                        {
                            cm.IsOpen = true;
                            return;
                        }
                        else
                        {

                            for (int i = 0; i < menuItemsCount; i++)
                            {
                                fe.ContextMenu.Items.RemoveAt(0);
                            }

                            //foreach (MenuItem item in fe.ContextMenu.Items)
                            //{
                            //    fe.ContextMenu.Items.RemoveAt(0);
                            //}
                            
                            MenuItem miUnassign = new MenuItem();
                            miUnassign.Header = "Unassign";
                            miUnassign.Click += ContextMenuItem_Click;
                            fe.ContextMenu.Items.Add(miUnassign);
                            
                            //((MenuItem)cm.Items[0]).Header = "Assign";
                        }
                    }
                    else if ((((DataRecord)dataContext).FieldLayout.Key.ToString() == "UnassignedLeads"))
                    {
                        //if ((String)((MenuItem)cm.Items[0]).Header == "Assign")
                        //{
                        //    cm.IsOpen = true;
                        //    return;
                        //}
                        //else
                        //{
                        for (int x = 0; x < menuItemsCount; x++)
                        {
                            fe.ContextMenu.Items.RemoveAt(0);
                        }
                        //foreach (MenuItem item in fe.ContextMenu.Items)
                        //{
                        //    fe.ContextMenu.Items.RemoveAt(0);
                        //}
                        int counter = 0;
                        foreach (DataRow dr in dtBUAgents.Rows)
                        {
                            //MenuItem miAssign = new MenuItem().Header = dr["BumpUpAgent"];

                            //miAssign.Header = "Assign";
                            //miAssign.Click += ContextMenuItem_Click;
                            //fe.ContextMenu.Items.Add(miAssign);
                            fe.ContextMenu.Items.Add(new MenuItem());
                            ((MenuItem)fe.ContextMenu.Items[counter]).Header = "Assign To " + dr["BumpUpAgent"].ToString();
                            ((MenuItem)fe.ContextMenu.Items[counter]).Tag = Convert.ToInt64(dr["UserID"]);
                            ((MenuItem)fe.ContextMenu.Items[counter]).Click += ContextMenuItem_Click;
                            counter++;
                        }
                            
                            
                            //((MenuItem)cm.Items[0]).Header = "Unassign";
                        //}
                    }
                    //}
                    //if (cm != null)
                    //{
                    //    //foreach (MenuItem mi in cm.Items)
                    //    //{
                    //    if (((DataRecord)dataContext).FieldLayout.Key.ToString() == "AssignedLeads")
                    //    {
                    //        if ((String)((MenuItem)cm.Items[0]).Header == "Assign")
                    //        {
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            //MenuItem miAssign = new MenuItem();
                    //            //miAssign.Header = "Assign";
                    //            //fe.ContextMenu.Items.Add(miAssign);
                    //            ((MenuItem)cm.Items[0]).Header = "Assign";
                    //        }
                    //    }
                    //    else if ((((DataRecord)dataContext).FieldLayout.Key.ToString() == "UnassignedLeads"))
                    //    {
                    //        if ((String)((MenuItem)cm.Items[0]).Header == "Unassign")
                    //        {
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            ((MenuItem)cm.Items[0]).Header = "Unassign";
                    //        }
                    //    }



                    //    //}

                    //e.Handled = false;
                    cm.IsOpen = true;
                }
                else
                {
                    e.Handled = true;
                }
                //if (dataContext != null && ((DataRecord)dataContext).FieldLayout.Key.ToString() != "AssignedLeads" && ((DataRecord)dataContext).FieldLayout.Key.ToString() != "UnassignedLeads")
                //{
                //    if ((dataContext.GetType().Name == "DataRecord")/* && !((DataRecord)dataContext).IsActive*/)
                //    {

                //        //e.Cancel = true;
                //        e.Handled = true;
                //    }
                //}
                //else
                //{
                //    if (dataContext != null && !((DataRecord)dataContext).IsActive)
                //    {
                //        e.Handled = true;
                //    }

                //}
                
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void XdgAssignLeads_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //dispatcherTimer1.Start();

                //add context menu to data records
                Style styleDataRecordCellArea = new Style(typeof(DataRecordCellArea));
                styleDataRecordCellArea.BasedOn = xdgAssignLeads.FieldLayoutSettings.DataRecordCellAreaStyle;
                //if (!((((User)GlobalSettings.ApplicationUser).FKUserType == (long)lkpUserType.SalesAgent) ||
                //    (((User)GlobalSettings.ApplicationUser).FKUserType == (long)lkpUserType.ConfirmationAgent)))
                //{

                styleDataRecordCellArea.Setters.Add(new Setter { Property = ContextMenuProperty, Value = xdgAssignLeads.FindResource("RecordContextMenu") });

                //}

                xdgAssignLeads.FieldLayoutSettings.DataRecordCellAreaStyle = styleDataRecordCellArea;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        #endregion Event Handlers


    }
}
