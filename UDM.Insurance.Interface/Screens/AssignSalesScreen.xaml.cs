using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using Infragistics.Windows.DataPresenter;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;


namespace UDM.Insurance.Interface.Screens
{
	public partial class AssignSalesScreen
    {
        #region Constant

        #endregion Constant

        #region Private Members

        //private readonly long _batchID;
        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        private readonly DateTime _dateOfSale;
        private readonly long _campaignGroupType;
        private DataTable _dtBatch;
        private DataTable _dtAgents;
        private DateTime _newAllocationDate;
        private bool _useDifferentAllocationDate = false;
        private long autoAssignCount = 0;
        DataSet dsAssignedSalesData;

        #endregion Private Members

        #region Constructors

        public AssignSalesScreen(long campaignGroupType, DateTime dateOfSale/*long batchID*/)
        {
            InitializeComponent();

            //_batchID = batchID;
            _dateOfSale = dateOfSale;
            _campaignGroupType = campaignGroupType;

            LoadLookupData();
        }

        #endregion Constructors

        #region Private Methods

        private void LoadLookupData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                dsAssignedSalesData = Insure.INGetDateOfSaleAssignedSalesData(_dateOfSale, _campaignGroupType, 1, 1);
                _dtBatch = dsAssignedSalesData.Tables[0];
                _dtAgents = dsAssignedSalesData.Tables[1];

                tbCampaign.Text = _dtBatch.Rows[0]["CampaignName"].ToString();
                tbDateOfSale.Text = _dtBatch.Rows[0]["DateOfSale"].ToString();
                //tbUDMBatch.Text = _dtBatch.Rows[0]["UDMBatchCode"].ToString();
                tbTotalAssigned.Text = _dtBatch.Rows[0]["Assigned"].ToString();
                tbTotalUnassigned.Text = _dtBatch.Rows[0]["Unassigned"].ToString();
                //tbTotalPrinted.Text = _dtBatch.Rows[0]["Printed"].ToString();

                xdgAssignSales.Tag = "Active, Employed";
                //tbAgents.Text = string.Format("{0} ({1})", xdgAssignSales.Tag, _dtAgents.Rows.Count);
                DataTable dt = _dtAgents.DefaultView.Table;
                DataColumn selectColumn = new DataColumn("Select", typeof(bool));
                selectColumn.DefaultValue = false;
                DataColumn indexColumn = new DataColumn("Index", typeof(int));
                dt.Columns.Add(selectColumn);
                dt.Columns.Add(indexColumn);
                int rowIndex = 0;
                foreach (DataRow row in dt.Rows)
                {
                    row["Index"] = rowIndex++;
                }

                xdgAssignSales.DataSource = dt.DefaultView;
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

        private void AutoAssignSales()
        {

            try
            {

                //decimal salesPerAgent = 0;

                decimal unassigned = decimal.Parse(tbTotalUnassigned.Text);
                long maxCurrentAssignValue = 0;
                int skipVal = 0;
                if (autoAssignCount >= unassigned)
                {
                    return;
                }
                DataTable dtAgents = ((DataView)xdgAssignSales.DataSource).Table;

                #region Commented Out
                //dtAgents.Columns.Add("Index");
                //int rowCount = 0;
                //foreach (DataRow row in dtAgents.Rows)
                //{
                //    row["Index"] = rowCount++;
                //}
                //DataTable dtAgentsToBeSorted = dtAgents.AsEnumerable().Where(x => bool.Parse(x["Select"].ToString()) == true).CopyToDataTable();
                #endregion

                DataRow skipValIndex = dtAgents.AsEnumerable().Where(x => bool.Parse(x["Select"].ToString()) == true).OrderBy(x => x["LeadsAllocated"]).ThenBy(x => x["SalesAgent"]).FirstOrDefault();
                skipVal = int.Parse(skipValIndex["Index"].ToString());
                maxCurrentAssignValue = long.Parse(skipValIndex["LeadsAllocated"].ToString()) + long.Parse(skipValIndex["Assign"].ToString()) + 1;
                autoAssignCount = dtAgents.AsEnumerable().Sum(x => x.Field<long>("Assign"));

                #region Comented Out
                //if (unassigned > 0)
                //{
                //    salesPerAgent = unassigned / (decimal)dtAgentsToBeSorted.Rows.Count;
                //}
                //autoAssignCount = 0;
                #endregion

                while (autoAssignCount < unassigned)
                {
                    skipValIndex = dtAgents.AsEnumerable().Where(x => bool.Parse(x["Select"].ToString()) == true).OrderBy(x => long.Parse(x["LeadsAllocated"].ToString()) + long.Parse(x["Assign"].ToString()))/*.ThenBy(x => x["Assign"])*/.ThenBy(x => x["SalesAgent"]).FirstOrDefault();

                    ((DataRecord)xdgAssignSales.Records[int.Parse(skipValIndex["Index"].ToString())]).Cells["Assign"].Value = long.Parse(((DataRecord)xdgAssignSales.Records[int.Parse(skipValIndex["Index"].ToString())]).Cells["Assign"].Value.ToString()) + 1;
                    autoAssignCount++;

                    #region Commented Out
                    //foreach (Record rec in xdgAssignSales.Records.Skip(skipVal))
                    //{

                    //    if (autoAssignCount >= unassigned)
                    //    {
                    //        //skipVal = rec.Index;
                    //        break;
                    //    }
                    //    DataRecord record = (DataRecord)rec;
                    //    if (bool.Parse(record.Cells["Select"].Value.ToString()) == false || ((long.Parse(record.Cells["Assign"].Value.ToString()) + long.Parse(record.Cells["LeadsAllocated"].Value.ToString())) >= maxCurrentAssignValue /*&& maxCurrentAssignValue > 0*/))
                    //    {
                    //        continue;
                    //    }
                    //    else
                    //    {
                    //        record.Cells["Assign"].Value = int.Parse(record.Cells["Assign"].Value.ToString()) + 1;
                    //        maxCurrentAssignValue = long.Parse(record.Cells["Assign"].Value.ToString()) + long.Parse(record.Cells["LeadsAllocated"].Value.ToString());
                    //        autoAssignCount++;
                    //    }

                    //}
                    #endregion

                }


            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void AssignLeads()
        {
            try
            {
                DataTable dtUnassignedSales = new DataTable();

                dtUnassignedSales = Insure.INGetUnassignedCallMonitoringAllocationsByDateOfSale(_dateOfSale, _campaignGroupType);

                int counter = 0;

                DataTable dtAgents = ((DataView)xdgAssignSales.DataSource).Table;

                DataTable dtAgentsFiltered = dtAgents.AsEnumerable().Where(x => bool.Parse(x["Select"].ToString()) == true && Convert.ToInt32(x["Assign"]) > 0).CopyToDataTable();

                DataRecord record;

                bool isFinished = false;

                foreach (DataRow row in dtUnassignedSales.Rows)
                //for (int i = 0; i < dtUnassignedSales.Rows.Count; i++)
                {
                    //DataRow row = dtUnassignedSales.Rows[i];
                    //long userID = Convert.ToInt64(dtAgentsFiltered.AsEnumerable().rO(x => x["Index"]).FirstOrDefault()["FKUserID"]);

                    //DataRecord record = (DataRecord)xdgAssignSales.Records[Convert.ToInt32(drAgent.Field<long>("Index"))];
                    if (counter >= dtAgentsFiltered.Rows.Count)
                    {
                        counter = 0;
                    }

                    

                    if (dtAgentsFiltered.AsEnumerable().OrderByDescending(x => x["Assign"]).FirstOrDefault().Field<long>("Assign") <= 0)
                    {
                        break;
                    }

                    DataRow drAgent = dtAgentsFiltered.Rows[counter];

                    record = (DataRecord)xdgAssignSales.Records[Convert.ToInt32(drAgent.Field<int>("Index"))];

                    long userID = drAgent.Field<long>("UserID");
                    long assign = Convert.ToInt64(record.Cells["Assign"].Value.ToString());

                    if (assign <= 0)
                    {
                       
                        foreach (DataRow cmAgent in dtAgentsFiltered.Rows)
                        {
                            record = (DataRecord)xdgAssignSales.Records[Convert.ToInt32(cmAgent.Field<int>("Index"))];
                            if (Convert.ToInt64(record.Cells["Assign"].Value.ToString()) <= 0)
                            {
                                if (cmAgent == dtAgentsFiltered.Rows[dtAgentsFiltered.Rows.Count - 1])
                                {
                                    isFinished = true;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                                
                            }
                            else
                            {
                                counter = dtAgentsFiltered.Rows.IndexOf(cmAgent);
                                drAgent = dtAgentsFiltered.Rows[counter];
                                userID = drAgent.Field<long>("UserID");
                                assign = Convert.ToInt64(record.Cells["Assign"].Value.ToString());
                                break;
                            }
                        }
                        if (isFinished == true)
                        {
                            break;
                        }
                    }

                    //record = (DataRecord)xdgAssignSales.Records[Convert.ToInt32(drAgent.Field<int>("Index"))];

                    Database.BeginTransaction(null, IsolationLevel.Snapshot);


                    CallMonitoringAllocation callMonitoringAllocation = new CallMonitoringAllocation();
                    callMonitoringAllocation.FKINImportID = long.Parse(row["ImportID"].ToString());
                    callMonitoringAllocation.FKUserID = userID;
                    callMonitoringAllocation.Save(_validationResult);

                    record.Cells["Assign"].Value = Convert.ToInt64(record.Cells["Assign"].Value.ToString()) - 1;
                    record.Cells["LeadsAllocated"].Value = Convert.ToInt64(record.Cells["LeadsAllocated"].Value.ToString()) + 1;

                    tbTotalAssigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                    {
                        tbTotalAssigned.Text = (Convert.ToInt64(tbTotalAssigned.Text) + 1).ToString();
                    }));
                    tbTotalUnassigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                    {
                        tbTotalUnassigned.Text = (Convert.ToInt64(tbTotalUnassigned.Text) - 1).ToString();
                    }));

                    //if (count == assign /*|| Math.Abs(count) == dtUnassignedSales.Rows.Count*/)
                    //{
                    //    record.Cells["Assign"].Value = 0;
                    //    break;
                    //}
                    //dtUnassignedSales.Rows.Remove(row);

                    CommitTransaction(null);

                    counter++;


                    
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        

        public void AssignLeadsOld()
        {
            try
            {
                
                foreach (Record rec in xdgAssignSales.Records)
                {
                    var record = (DataRecord)rec;

                    DataTable dtUnassignedSales = new DataTable();

                    long userID = long.Parse(record.Cells["UserID"].Value.ToString());
                    long assign = long.Parse(record.Cells["Assign"].Value.ToString());

                    if (assign > 0)
                    {
                        dtUnassignedSales = Insure.INGetUnassignedCallMonitoringAllocationsByDateOfSale(_dateOfSale, _campaignGroupType);

                        if (assign > dtUnassignedSales.Rows.Count)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                            {
                                INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                                ShowMessageBox(messageWindow, dtUnassignedSales.Rows.Count + " leads available.", "Not Enough Leads Available", ShowMessageType.Error);
                            }));

                            return;
                        }

                        if (dtUnassignedSales.Rows.Count > 0)
                        {

                            

                            int count = 0;

                            while (dtUnassignedSales.Rows.Count > 0 && count < assign)
                            {
                                //DataTable dtTSRs = (from tsr in dtUnassignedSales.AsEnumerable()
                                //                    select tsr/*.Field<Int64>("TSRAssignedTo")*/).Distinct().CopyToDataTable();
                                DataTable dtTSRs = (dtUnassignedSales.AsEnumerable().GroupBy(x => x["TSRAssignedTo"]).Select(x => x.FirstOrDefault())).CopyToDataTable();
                                //DataTable dtTSRs = (dtUnassignedSales.AsEnumerable().GroupBy(r => new {c1 = r.Field<long>("FKStaffTypeID"), c2 = r.Field<long>("TSRAssignedTo") }).Select(r => r.FirstOrDefault())).CopyToDataTable();
                                //DataTable dtTSRs = (dtUnassignedSales.AsEnumerable().GroupBy(x => x["FKStaffTypeID"]).Select(r => r.FirstOrDefault())).CopyToDataTable();


                                Database.BeginTransaction(null, IsolationLevel.Snapshot);

                                foreach (DataRow tsr in dtTSRs.Rows)
                                {
                                    count++;
                                    //DataRow drSale = (from sale in dtUnassignedSales.AsEnumerable()
                                    //                  where sale.Field<long>("TSRAssignedTo") == long.Parse(tsr["TSRAssignedTo"].ToString())
                                    //                  orderby sale["TSRAssignedTo"] descending
                                    //                  select sale).First();

                                    DataRow drSale = dtUnassignedSales.AsEnumerable().Where(sale => long.Parse(sale["TSRAssignedTo"].ToString()) == long.Parse(tsr["TSRAssignedTo"].ToString())).FirstOrDefault();
                                    //DataRow drSale = dtUnassignedSales.AsEnumerable().Where(sale => long.Parse(sale["FKStaffTypeID"].ToString()) == long.Parse(tsr["FKStaffTypeID"].ToString())).FirstOrDefault();

                                    CallMonitoringAllocation callMonitoringAllocation = new CallMonitoringAllocation();
                                    callMonitoringAllocation.FKINImportID = long.Parse(drSale["ImportID"].ToString());
                                    callMonitoringAllocation.FKUserID = userID;
                                    callMonitoringAllocation.Save(_validationResult);
                                    //callMonitoringAllocation.FKINImportID = 
                                    //inImport.FKUserID = userID;
                                    ////inImport.AllocationDate = DateTime.Now;
                                    //inImport.AllocationDate = DetermineAllocationDate(_useDifferentAllocationDate, _newAllocationDate);
                                    //inImport.BonusLead = chkBonusLeads.IsChecked;
                                    //inImport.Save(_validationResult);

                                    record.Cells["Assign"].Value = Convert.ToInt64(record.Cells["Assign"].Value.ToString()) - 1;
                                    record.Cells["LeadsAllocated"].Value = Convert.ToInt64(record.Cells["LeadsAllocated"].Value.ToString()) + 1;
                                    tbTotalAssigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                                    {
                                        tbTotalAssigned.Text = (Convert.ToInt64(tbTotalAssigned.Text) + 1).ToString();
                                    }));
                                    tbTotalUnassigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                                    {
                                        tbTotalUnassigned.Text = (Convert.ToInt64(tbTotalUnassigned.Text) - 1).ToString();
                                    }));

                                    if (count == assign /*|| Math.Abs(count) == dtUnassignedSales.Rows.Count*/)
                                    {
                                        record.Cells["Assign"].Value = 0;
                                        break;
                                    }
                                    dtUnassignedSales.Rows.Remove(drSale);
                                }

                                CommitTransaction(null);

                            }
                            
                        }
                        else
                        {
                            record.Cells["Assign"].Value = 0;
                        }
                    }
                    else if (assign < 0)
                    {
                        //INImportCollection inImportCollection = INImportMapper.Search(userID, _batchID, false, null);

                        //if (inImportCollection.Count > 0)
                        //{
                        //    Database.BeginTransaction(null, IsolationLevel.Snapshot);
                        //    int[] i = { 0 };
                        //    foreach (INImport inImport in inImportCollection)
                        //    {
                        //        i[0]--;
                        //        inImport.FKUserID = null;
                        //        inImport.AllocationDate = null;
                        //        inImport.Save(_validationResult);

                        //        record.Cells["Assign"].Value = Convert.ToInt64(record.Cells["Assign"].Value.ToString()) + 1;
                        //        record.Cells["LeadsAllocated"].Value = Convert.ToInt64(record.Cells["LeadsAllocated"].Value.ToString()) - 1;
                        //        tbTotalAssigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                        //        {
                        //            tbTotalAssigned.Text = (Convert.ToInt64(tbTotalAssigned.Text) - 1).ToString();
                        //        }));
                        //        tbTotalUnassigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                        //        {
                        //            tbTotalUnassigned.Text = (Convert.ToInt64(tbTotalUnassigned.Text) + 1).ToString();
                        //        }));

                        //        if (i[0] == assign || Math.Abs(i[0]) == inImportCollection.Count)
                        //        {
                        //            record.Cells["Assign"].Value = 0;
                        //            break;
                        //        }
                        //    }
                        //    CommitTransaction(null);
                        //}
                        //else
                        //{
                        //    record.Cells["Assign"].Value = 0;
                        //}
                    }
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void AssignLeadsByRatio2()
        {
            try
            {
                //iterate through the call monitoring agents
                foreach (Record rec in xdgAssignSales.Records)
                {
                    var record = (DataRecord)rec;

                    DataTable dtUnassignedSales = new DataTable();

                    long userID = long.Parse(record.Cells["UserID"].Value.ToString());
                    long assign = long.Parse(record.Cells["Assign"].Value.ToString());

                    //if the amount to assign to the cm agent is more than zero then carry on otherwise go to the next cm agent
                    if (assign > 0)
                    {
                        //get the unassigned sales for the specific date of sale and campaign group type (e.g. base or upgrades)
                        dtUnassignedSales = Insure.INGetUnassignedCallMonitoringAllocationsByDateOfSale(_dateOfSale, _campaignGroupType);

                        //if the assign amount is more than how many sales are available end the process.
                        if (assign > dtUnassignedSales.Rows.Count)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                            {
                                INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                                ShowMessageBox(messageWindow, dtUnassignedSales.Rows.Count + " leads available.", "Not Enough Leads Available", ShowMessageType.Error);
                            }));

                            return;
                        }

                        
                        if (dtUnassignedSales.Rows.Count > 0)
                        {
                            //split the unassigned sales up into sales made by perms and sales made by temps
                            

                            int count = 0;

                            DataTable dtPerms = new DataTable();
                            DataTable dtTemps = new DataTable();
                            
                            if (dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 1).Count() >= 1)
                            {
                                //get the count of how many sales there are that were made by permanent agents
                                dtPerms = dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 1).CopyToDataTable();
                            }
                            if (dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 2).Count() >= 1)
                            {
                                //get the count of how many sales there are that were made by temporary agents
                                dtTemps = dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 2).CopyToDataTable();
                            }
                            
                            //calculate percentage of perms and temps out of the unassigned sales.
                            float permPercentage = dtPerms.Rows.Count / Convert.ToSingle(dtUnassignedSales.Rows.Count);
                            float tempPercentage = dtTemps.Rows.Count / Convert.ToSingle(dtUnassignedSales.Rows.Count);

                            //get a number of how many perms and temps need to be assigned.
                            float numPerms = assign * permPercentage;
                            float numTemps = assign * tempPercentage;

                            while (dtUnassignedSales.Rows.Count > 0 && count < assign)
                            {
                                //DataTable dtTSRs = (from tsr in dtUnassignedSales.AsEnumerable()
                                //                    select tsr/*.Field<Int64>("TSRAssignedTo")*/).Distinct().CopyToDataTable();



                                //DataTable dtTSRs = (dtUnassignedSales.AsEnumerable().GroupBy(x => x["TSRAssignedTo"]).Select(x => x.FirstOrDefault())).CopyToDataTable();

                                //DataTable dtTSRs = (dtUnassignedSales.AsEnumerable().GroupBy(x => x["FKStaffTypeID"]).Select(x => x.FirstOrDefault())).CopyToDataTable();
                                //DataTable dtTSRs = (dtUnassignedSales.AsEnumerable().GroupBy(r => new {c1 = r.Field<long>("FKStaffTypeID"), c2 = r.Field<long>("TSRAssignedTo") }).Select(r => r.FirstOrDefault())).CopyToDataTable();
                                //DataTable dtTSRs = (dtUnassignedSales.AsEnumerable().GroupBy(x => x["FKStaffTypeID"]).Select(r => r.FirstOrDefault())).CopyToDataTable();

                                DataTable dtPermTSRs = new DataTable();
                                if (dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 1).GroupBy(x => x["TSRAssignedTo"]).Select(x => x.FirstOrDefault()).Count() >= 1)
                                {
                                    dtPermTSRs = (dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 1).GroupBy(x => x["TSRAssignedTo"]).Select(x => x.FirstOrDefault())).CopyToDataTable();
                                }

                                DataTable dtTempTSRs = new DataTable();
                                if (dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 2).GroupBy(x => x["TSRAssignedTo"]).Select(x => x.FirstOrDefault()).Count() >= 1)
                                {
                                    dtTempTSRs = (dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 2).GroupBy(x => x["TSRAssignedTo"]).Select(x => x.FirstOrDefault())).CopyToDataTable();
                                }
                                


                                

                                //foreach (DataRow tsr in dtTSRs.Rows)
                                //{

                                
                                //long staffType = 0;
                                //DataRow drSale = dtUnassignedSales.Rows[0];
                                //DataRow drSale = (from sale in dtUnassignedSales.AsEnumerable()
                                //                  where sale.Field<long>("TSRAssignedTo") == long.Parse(tsr["TSRAssignedTo"].ToString())
                                //                  orderby sale["TSRAssignedTo"] descending
                                //                  select sale).First();
                                if (numPerms >= 0.5 && dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 1).Count() >= 1)
                                {
                                    //staffType = 1;
                                    foreach(DataRow perm in dtPermTSRs.Rows)
                                    {
                                        SaveAllocation(1, perm, userID, ref dtUnassignedSales, record);
                                        numPerms--;
                                        count++;
                                        if (numPerms < 0.5 || dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 1).Count() < 1)
                                        {
                                            break;
                                        }
                                    }                                    
                                }
                                else if (numTemps > 0.5 && dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 2).Count() >= 1)
                                {
                                    //staffType = 2;
                                    foreach (DataRow temp in dtTempTSRs.Rows)
                                    {
                                        SaveAllocation(2, temp, userID, ref dtUnassignedSales, record);
                                        numTemps--;
                                        count++;
                                        if (numTemps <= 0.5 || dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 2).Count() < 1)
                                        {
                                            break;
                                        }
                                    }
                                }

                                ////DataRow drSale = dtUnassignedSales.AsEnumerable().Where(sale => long.Parse(sale["TSRAssignedTo"].ToString()) == long.Parse(tsr["TSRAssignedTo"].ToString())).FirstOrDefault();
                                //DataRow drSale = dtUnassignedSales.AsEnumerable().Where(sale => Convert.ToInt64(sale["FKStaffTypeID"]) == staffType).FirstOrDefault();

                                //CallMonitoringAllocation callMonitoringAllocation = new CallMonitoringAllocation();
                                //callMonitoringAllocation.FKINImportID = long.Parse(drSale["ImportID"].ToString());
                                //callMonitoringAllocation.FKUserID = userID;
                                //callMonitoringAllocation.Save(_validationResult);
                                ////callMonitoringAllocation.FKINImportID = 
                                ////inImport.FKUserID = userID;
                                //////inImport.AllocationDate = DateTime.Now;
                                ////inImport.AllocationDate = DetermineAllocationDate(_useDifferentAllocationDate, _newAllocationDate);
                                ////inImport.BonusLead = chkBonusLeads.IsChecked;
                                ////inImport.Save(_validationResult);

                                //record.Cells["Assign"].Value = Convert.ToInt64(record.Cells["Assign"].Value.ToString()) - 1;
                                //record.Cells["LeadsAllocated"].Value = Convert.ToInt64(record.Cells["LeadsAllocated"].Value.ToString()) + 1;
                                //tbTotalAssigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                                //{
                                //    tbTotalAssigned.Text = (Convert.ToInt64(tbTotalAssigned.Text) + 1).ToString();
                                //}));
                                //tbTotalUnassigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                                //{
                                //    tbTotalUnassigned.Text = (Convert.ToInt64(tbTotalUnassigned.Text) - 1).ToString();
                                //}));

                                //if (count == assign /*|| Math.Abs(count) == dtUnassignedSales.Rows.Count*/)
                                //{
                                //    record.Cells["Assign"].Value = 0;
                                //    //break;
                                //}
                                //dtUnassignedSales.Rows.Remove(drSale);
                                //}

                                

                            }

                        }
                        else
                        {
                            record.Cells["Assign"].Value = 0;
                        }
                    }
                    else if (assign < 0)
                    {
                        

                        DataTable dtLeadsToUnallocate = Insure.GetCMAllocationsNotWorkedOn(_dateOfSale, _campaignGroupType, userID);

                        //CallMonitoringAllocationCollection callMonitoringAllocationCollection = new CallMonitoringAllocationCollection();

                        //callMonitoringAllocationCollection = dtLeadsToUnallocate.AsEnumerable().SelectMany<CallMonitoringAllocation>();
                        //callMonitoringAllocationCollection  = dtLeadsToUnallocate

                        //foreach (DataRow dr in dtLeadsToUnallocate.Rows)
                        //{
                        //    callMonitoringAllocationCollection.Add(CallMonitoringAllocationMapper.SearchOne(Convert.ToInt64(dr["FKINImportID"]), Convert.ToInt64(dr["FKUserID"]), null));
                        //}

                        



                        //CallMonitoringAllocation callMonitoringAllocation = new CallMonitoringAllocation();

                        if (dtLeadsToUnallocate.Rows.Count > 0)
                        {
                            //Database.BeginTransaction(null, IsolationLevel.Snapshot);
                            int[] i = { 0 };
                            foreach (DataRow callMonitoringAllocation in dtLeadsToUnallocate.Rows)
                            {
                                i[0]--;
                                //callMonitoringAllocation.Delete(_validationResult);
                                Database.ExecuteCommand("DELETE FROM CallMonitoringAllocation WHERE ID = " + Convert.ToInt64(callMonitoringAllocation["ID"]));
                                record.Cells["Assign"].Value = Convert.ToInt64(record.Cells["Assign"].Value.ToString()) + 1;
                                record.Cells["LeadsAllocated"].Value = Convert.ToInt64(record.Cells["LeadsAllocated"].Value.ToString()) - 1;
                                tbTotalAssigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                                {
                                    tbTotalAssigned.Text = (Convert.ToInt64(tbTotalAssigned.Text) - 1).ToString();
                                }));
                                tbTotalUnassigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                                {
                                    tbTotalUnassigned.Text = (Convert.ToInt64(tbTotalUnassigned.Text) + 1).ToString();
                                }));

                                if (i[0] == assign || Math.Abs(i[0]) == dtLeadsToUnallocate.Rows.Count)
                                {
                                    record.Cells["Assign"].Value = 0;
                                    break;
                                }
                            }
                            //CommitTransaction(null);
                        }
                        else
                        {
                            record.Cells["Assign"].Value = 0;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void AssignLeadsByRatio2SavedCF()
        {
            try
            {
                //iterate through the call monitoring agents
                foreach (Record rec in xdgAssignSales.Records)
                {
                    DataRecord nextRecord;
                    if (rec.Index >= (xdgAssignSales.Records.Count() - 1))
                    {
                         nextRecord = (DataRecord)xdgAssignSales.Records[0];
                    }
                    else
                    {
                        nextRecord = (DataRecord)xdgAssignSales.Records[rec.Index + 1];
                    }
                    var record = (DataRecord)rec;

                    DataTable dtUnassignedSales = new DataTable();

                    long userID = long.Parse(record.Cells["UserID"].Value.ToString());
                    long assign = long.Parse(record.Cells["Assign"].Value.ToString());

                    //if the amount to assign to the cm agent is more than zero then carry on otherwise go to the next cm agent
                    if (assign > 0)
                    {
                        //get the unassigned sales for the specific date of sale and campaign group type (e.g. base or upgrades)
                        dtUnassignedSales = Insure.INGetUnassignedCallMonitoringAllocationsByDateOfSale(_dateOfSale, _campaignGroupType);

                        //if the assign amount is more than how many sales are available end the process.
                        if (assign > dtUnassignedSales.Rows.Count)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                            {
                                INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                                ShowMessageBox(messageWindow, dtUnassignedSales.Rows.Count + " leads available.", "Not Enough Leads Available", ShowMessageType.Error);
                            }));

                            return;
                        }


                        if (dtUnassignedSales.Rows.Count > 0)
                        {
                            //split the unassigned sales up into sales made by perms and sales made by temps


                            int count = 0;

                            DataTable dtPerms = new DataTable();
                            DataTable dtTemps = new DataTable();

                            if (dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 1).Count() >= 1)
                            {
                                //get the count of how many sales there are that were made by permanent agents
                                dtPerms = dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 1).CopyToDataTable();
                            }
                            if (dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 2).Count() >= 1)
                            {
                                //get the count of how many sales there are that were made by temporary agents
                                dtTemps = dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 2).CopyToDataTable();
                            }

                            //calculate percentage of perms and temps out of the unassigned sales.
                            float permPercentage = dtPerms.Rows.Count / Convert.ToSingle(dtUnassignedSales.Rows.Count);
                            float tempPercentage = dtTemps.Rows.Count / Convert.ToSingle(dtUnassignedSales.Rows.Count);

                            //get a number of how many perms and temps need to be assigned.
                            float numPerms = assign * permPercentage;
                            float numTemps = assign * tempPercentage;

                            while ((dtUnassignedSales.Rows.Count > 0 && count < assign) && (numPerms > 0 || numTemps > 0))
                            {
                                //DataTable dtTSRs = (from tsr in dtUnassignedSales.AsEnumerable()
                                //                    select tsr/*.Field<Int64>("TSRAssignedTo")*/).Distinct().CopyToDataTable();



                                //DataTable dtTSRs = (dtUnassignedSales.AsEnumerable().GroupBy(x => x["TSRAssignedTo"]).Select(x => x.FirstOrDefault())).CopyToDataTable();

                                //DataTable dtTSRs = (dtUnassignedSales.AsEnumerable().GroupBy(x => x["FKStaffTypeID"]).Select(x => x.FirstOrDefault())).CopyToDataTable();
                                //DataTable dtTSRs = (dtUnassignedSales.AsEnumerable().GroupBy(r => new {c1 = r.Field<long>("FKStaffTypeID"), c2 = r.Field<long>("TSRAssignedTo") }).Select(r => r.FirstOrDefault())).CopyToDataTable();
                                //DataTable dtTSRs = (dtUnassignedSales.AsEnumerable().GroupBy(x => x["FKStaffTypeID"]).Select(r => r.FirstOrDefault())).CopyToDataTable();

                                DataTable dtPermTSRs = new DataTable();
                                if (dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 1).GroupBy(x => x["TSRAssignedTo"]).Select(x => x.FirstOrDefault()).Count() >= 1)
                                {
                                    dtPermTSRs = (dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 1).GroupBy(x => x["TSRAssignedTo"]).Select(x => x.FirstOrDefault())).CopyToDataTable();
                                }

                                DataTable dtTempTSRs = new DataTable();
                                if (dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 2).GroupBy(x => x["TSRAssignedTo"]).Select(x => x.FirstOrDefault()).Count() >= 1)
                                {
                                    dtTempTSRs = (dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 2).GroupBy(x => x["TSRAssignedTo"]).Select(x => x.FirstOrDefault())).CopyToDataTable();
                                }





                                //foreach (DataRow tsr in dtTSRs.Rows)
                                //{


                                //long staffType = 0;
                                //DataRow drSale = dtUnassignedSales.Rows[0];
                                //DataRow drSale = (from sale in dtUnassignedSales.AsEnumerable()
                                //                  where sale.Field<long>("TSRAssignedTo") == long.Parse(tsr["TSRAssignedTo"].ToString())
                                //                  orderby sale["TSRAssignedTo"] descending
                                //                  select sale).First();
                                if (numPerms >= 0.5 && dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 1).Count() >= 1)
                                {
                                    //staffType = 1;
                                    foreach (DataRow perm in dtPermTSRs.Rows)
                                    {
                                        
                                        bool isSaved = SaveAllocation2(1, perm, userID, ref dtUnassignedSales, record, nextRecord);
                                        if (!(isSaved))
                                        {
                                            //numTemps = numTemps + numPerms;
                                            numPerms = 0;
                                            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                                            {
                                                INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                                                ShowMessageBox(messageWindow, "You can't assign a call monitoring agent's own saved carried forward to call monitor. Please assign it to someone else.", "Saved Carried Forward Error", ShowMessageType.Error);
                                            }));
                                            break;
                                            //continue;
                                        }
                                        numPerms--;
                                        count++;
                                        if (numPerms < 0.5 || dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 1).Count() < 1)
                                        {
                                            break;
                                        }
                                    }
                                }
                                else if (numTemps > 0.5 && dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 2).Count() >= 1)
                                {
                                    //staffType = 2;
                                    foreach (DataRow temp in dtTempTSRs.Rows)
                                    {
                                        bool isSaved = SaveAllocation2(2, temp, userID, ref dtUnassignedSales, record, nextRecord);
                                        if (!(isSaved))
                                        {
                                            //numPerms = numPerms + numTemps;
                                            numTemps = 0;
                                            break;
                                            //continue;
                                        }
                                        numTemps--;
                                        count++;
                                        if (numTemps <= 0.5 || dtUnassignedSales.AsEnumerable().Where(x => Convert.ToInt32(x["FKStaffTypeID"]) == 2).Count() < 1)
                                        {
                                            break;
                                        }
                                    }
                                }

                                ////DataRow drSale = dtUnassignedSales.AsEnumerable().Where(sale => long.Parse(sale["TSRAssignedTo"].ToString()) == long.Parse(tsr["TSRAssignedTo"].ToString())).FirstOrDefault();
                                //DataRow drSale = dtUnassignedSales.AsEnumerable().Where(sale => Convert.ToInt64(sale["FKStaffTypeID"]) == staffType).FirstOrDefault();

                                //CallMonitoringAllocation callMonitoringAllocation = new CallMonitoringAllocation();
                                //callMonitoringAllocation.FKINImportID = long.Parse(drSale["ImportID"].ToString());
                                //callMonitoringAllocation.FKUserID = userID;
                                //callMonitoringAllocation.Save(_validationResult);
                                ////callMonitoringAllocation.FKINImportID = 
                                ////inImport.FKUserID = userID;
                                //////inImport.AllocationDate = DateTime.Now;
                                ////inImport.AllocationDate = DetermineAllocationDate(_useDifferentAllocationDate, _newAllocationDate);
                                ////inImport.BonusLead = chkBonusLeads.IsChecked;
                                ////inImport.Save(_validationResult);

                                //record.Cells["Assign"].Value = Convert.ToInt64(record.Cells["Assign"].Value.ToString()) - 1;
                                //record.Cells["LeadsAllocated"].Value = Convert.ToInt64(record.Cells["LeadsAllocated"].Value.ToString()) + 1;
                                //tbTotalAssigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                                //{
                                //    tbTotalAssigned.Text = (Convert.ToInt64(tbTotalAssigned.Text) + 1).ToString();
                                //}));
                                //tbTotalUnassigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                                //{
                                //    tbTotalUnassigned.Text = (Convert.ToInt64(tbTotalUnassigned.Text) - 1).ToString();
                                //}));

                                //if (count == assign /*|| Math.Abs(count) == dtUnassignedSales.Rows.Count*/)
                                //{
                                //    record.Cells["Assign"].Value = 0;
                                //    //break;
                                //}
                                //dtUnassignedSales.Rows.Remove(drSale);
                                //}



                            }
                            record.Cells["Assign"].Value = 0;
                        }
                        else
                        {
                            record.Cells["Assign"].Value = 0;
                        }
                    }
                    else if (assign < 0)
                    {


                        DataTable dtLeadsToUnallocate = Insure.GetCMAllocationsNotWorkedOn(_dateOfSale, _campaignGroupType, userID);

                        //CallMonitoringAllocationCollection callMonitoringAllocationCollection = new CallMonitoringAllocationCollection();

                        //callMonitoringAllocationCollection = dtLeadsToUnallocate.AsEnumerable().SelectMany<CallMonitoringAllocation>();
                        //callMonitoringAllocationCollection  = dtLeadsToUnallocate

                        //foreach (DataRow dr in dtLeadsToUnallocate.Rows)
                        //{
                        //    callMonitoringAllocationCollection.Add(CallMonitoringAllocationMapper.SearchOne(Convert.ToInt64(dr["FKINImportID"]), Convert.ToInt64(dr["FKUserID"]), null));
                        //}





                        //CallMonitoringAllocation callMonitoringAllocation = new CallMonitoringAllocation();

                        if (dtLeadsToUnallocate.Rows.Count > 0)
                        {
                            //Database.BeginTransaction(null, IsolationLevel.Snapshot);
                            int[] i = { 0 };
                            foreach (DataRow callMonitoringAllocation in dtLeadsToUnallocate.Rows)
                            {
                                i[0]--;
                                //callMonitoringAllocation.Delete(_validationResult);
                                CallMonitoringUnallocation callMonitoringUnallocation = new CallMonitoringUnallocation();
                                callMonitoringUnallocation.FKINImportID = callMonitoringAllocation["FKINImportID"] as long?;
                                callMonitoringUnallocation.FKUserID = callMonitoringAllocation["FKUserID"] as long?;
                                callMonitoringUnallocation.IsSavedCarriedForward = callMonitoringAllocation["IsSavedCarriedForward"] as bool?;

                               

                                callMonitoringUnallocation.ExpiryDate = callMonitoringAllocation["ExpiryDate"] as DateTime?;
                                callMonitoringUnallocation.AllocatedByUserID = callMonitoringAllocation["StampUserID"] as long?;
                                callMonitoringUnallocation.CallMonitoringAllocationDate = callMonitoringAllocation["StampDate"] as DateTime?;
                                callMonitoringUnallocation.Save(_validationResult);

                                Database.ExecuteCommand("DELETE FROM CallMonitoringAllocation WHERE ID = " + Convert.ToInt64(callMonitoringAllocation["ID"]));
                                record.Cells["Assign"].Value = Convert.ToInt64(record.Cells["Assign"].Value.ToString()) + 1;
                                record.Cells["LeadsAllocated"].Value = Convert.ToInt64(record.Cells["LeadsAllocated"].Value.ToString()) - 1;
                                tbTotalAssigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                                {
                                    tbTotalAssigned.Text = (Convert.ToInt64(tbTotalAssigned.Text) - 1).ToString();
                                }));
                                tbTotalUnassigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                                {
                                    tbTotalUnassigned.Text = (Convert.ToInt64(tbTotalUnassigned.Text) + 1).ToString();
                                }));

                                if (i[0] == assign || Math.Abs(i[0]) == dtLeadsToUnallocate.Rows.Count)
                                {
                                    record.Cells["Assign"].Value = 0;
                                    break;
                                }
                            }
                            //CommitTransaction(null);
                        }
                        else
                        {
                            record.Cells["Assign"].Value = 0;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public bool SaveAllocation2(int staffType, DataRow tsr, long userID, ref DataTable dtUnassignedSales, DataRecord record, DataRecord nextRecord)
        {
            
            Database.BeginTransaction(null, IsolationLevel.Snapshot);
            //DataRow drSale = dtUnassignedSales.AsEnumerable().Where(sale => Convert.ToInt64(sale["FKStaffTypeID"]) == staffType && Convert.ToInt64(sale["TSRAssignedTo"]) == Convert.ToInt64(tsr["TSRAssignedTo"])).FirstOrDefault();

            
            DataRow drSale;

            //check if there are any saved carried forwards from the next person in the sort.
            if (dtUnassignedSales.AsEnumerable().Where(sale => Convert.ToInt64(sale["FKStaffTypeID"]) == staffType && Convert.ToInt64(sale["TSRAssignedTo"]) == Convert.ToInt64(tsr["TSRAssignedTo"]) && sale["FirstCMUserAllocatedTo"] as long? == Convert.ToInt64(nextRecord.Cells["UserID"].Value.ToString())).Count() > 0)
            {
                drSale = dtUnassignedSales.AsEnumerable().Where(sale => Convert.ToInt64(sale["FKStaffTypeID"]) == staffType && Convert.ToInt64(sale["TSRAssignedTo"]) == Convert.ToInt64(tsr["TSRAssignedTo"]) && sale["FirstCMUserAllocatedTo"] as long? == Convert.ToInt64(nextRecord.Cells["UserID"].Value.ToString())).FirstOrDefault();
            }            
            else if (dtUnassignedSales.AsEnumerable().Where(sale => Convert.ToInt64(sale["FKStaffTypeID"]) == staffType && Convert.ToInt64(sale["TSRAssignedTo"]) == Convert.ToInt64(tsr["TSRAssignedTo"]) && sale["FirstCMUserAllocatedTo"] as long? != userID).Count() > 0)
            {
                drSale = dtUnassignedSales.AsEnumerable().Where(sale => sale["FKStaffTypeID"] as long? == staffType && sale["TSRAssignedTo"] as long? == tsr["TSRAssignedTo"] as long? && sale["FirstCMUserAllocatedTo"] as long? != userID).OrderBy(x => x["FirstCMUserAllocatedTo"] as long?).FirstOrDefault();
            }
            else
            {
                return false;
            }


            CallMonitoringAllocation callMonitoringAllocation = new CallMonitoringAllocation();
            callMonitoringAllocation.FKINImportID = long.Parse(drSale["ImportID"].ToString());
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@ImportID", callMonitoringAllocation.FKINImportID); //added this in case the person is working on a mining campaign
                                                                                            //string agentName = Methods.ExecuteFunction("fnGetUserFirstName", parameters).ToString();
            callMonitoringAllocation.ExpiryDate = Convert.ToDateTime(Methods.ExecuteFunction("fnGetCMSaleExpiryDate", parameters));
            callMonitoringAllocation.FKUserID = userID;
            if (drSale["IsRecoveredSale"] as bool? == true && drSale["FKINLeadStatusID"] as long? == 1)
            {
                callMonitoringAllocation.IsSavedCarriedForward = true;
            }
            callMonitoringAllocation.Save(_validationResult);

            record.Cells["Assign"].Value = Convert.ToInt64(record.Cells["Assign"].Value.ToString()) - 1;
            record.Cells["LeadsAllocated"].Value = Convert.ToInt64(record.Cells["LeadsAllocated"].Value.ToString()) + 1;
            tbTotalAssigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
            {
                tbTotalAssigned.Text = (Convert.ToInt64(tbTotalAssigned.Text) + 1).ToString();
            }));
            tbTotalUnassigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
            {
                tbTotalUnassigned.Text = (Convert.ToInt64(tbTotalUnassigned.Text) - 1).ToString();
            }));

            
            //if (count == assign /*|| Math.Abs(count) == dtUnassignedSales.Rows.Count*/)
            //{
            //    record.Cells["Assign"].Value = 0;
            //    //break;
            //}
            dtUnassignedSales.Rows.Remove(drSale);

            CommitTransaction(null);

            return true;
        }

        public void SaveAllocation(int staffType, DataRow tsr, long userID, ref DataTable dtUnassignedSales, DataRecord record)
        {

            Database.BeginTransaction(null, IsolationLevel.Snapshot);
            DataRow drSale = dtUnassignedSales.AsEnumerable().Where(sale => Convert.ToInt64(sale["FKStaffTypeID"]) == staffType && Convert.ToInt64(sale["TSRAssignedTo"]) == Convert.ToInt64(tsr["TSRAssignedTo"])).FirstOrDefault();
            //DataRow drSale;
            //if (dtUnassignedSales.AsEnumerable().Where(sale => Convert.ToInt64(sale["FKStaffTypeID"]) == staffType && Convert.ToInt64(sale["TSRAssignedTo"]) == Convert.ToInt64(tsr["TSRAssignedTo"]) && Convert.ToInt64(sale["FirstCallMonitoringUser"]) != userID).Count() > 0)
            //{
            //    drSale = dtUnassignedSales.AsEnumerable().Where(sale => Convert.ToInt64(sale["FKStaffTypeID"]) == staffType && Convert.ToInt64(sale["TSRAssignedTo"]) == Convert.ToInt64(tsr["TSRAssignedTo"]) && Convert.ToInt64(sale["FirstCallMonitoringUser"]) != userID).FirstOrDefault();
            //}
            //else
            //{
            //    return false;
            //}


            CallMonitoringAllocation callMonitoringAllocation = new CallMonitoringAllocation();
            callMonitoringAllocation.FKINImportID = long.Parse(drSale["ImportID"].ToString());
            callMonitoringAllocation.FKUserID = userID;
            callMonitoringAllocation.Save(_validationResult);

            record.Cells["Assign"].Value = Convert.ToInt64(record.Cells["Assign"].Value.ToString()) - 1;
            record.Cells["LeadsAllocated"].Value = Convert.ToInt64(record.Cells["LeadsAllocated"].Value.ToString()) + 1;
            tbTotalAssigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
            {
                tbTotalAssigned.Text = (Convert.ToInt64(tbTotalAssigned.Text) + 1).ToString();
            }));
            tbTotalUnassigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
            {
                tbTotalUnassigned.Text = (Convert.ToInt64(tbTotalUnassigned.Text) - 1).ToString();
            }));


            //if (count == assign /*|| Math.Abs(count) == dtUnassignedSales.Rows.Count*/)
            //{
            //    record.Cells["Assign"].Value = 0;
            //    //break;
            //}
            dtUnassignedSales.Rows.Remove(drSale);

            CommitTransaction(null);

            //return true;
        }

        //public void AssignPermLeads()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        //public void AssignTempLeads()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        public void AssignLeadsByRatio()
        {
            try
            {

                foreach (Record rec in xdgAssignSales.Records)
                {
                    var record = (DataRecord)rec;

                    DataTable dtUnassignedSales = new DataTable();

                    long userID = long.Parse(record.Cells["UserID"].Value.ToString());
                    long assign = long.Parse(record.Cells["Assign"].Value.ToString());

                    if (assign > 0)
                    {
                        dtUnassignedSales = Insure.INGetUnassignedCallMonitoringAllocationsByDateOfSale(_dateOfSale, _campaignGroupType);

                        if (assign > dtUnassignedSales.Rows.Count)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                            {
                                INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                                ShowMessageBox(messageWindow, dtUnassignedSales.Rows.Count + " leads available.", "Not Enough Leads Available", ShowMessageType.Error);
                            }));

                            return;
                        }

                        if (dtUnassignedSales.Rows.Count > 0)
                        {



                            int count = 0;

                            while (dtUnassignedSales.Rows.Count > 0 && count < assign)
                            {
                                //DataTable dtTSRs = (from tsr in dtUnassignedSales.AsEnumerable()
                                //                    select tsr/*.Field<Int64>("TSRAssignedTo")*/).Distinct().CopyToDataTable();
                                DataTable dtTSRs = (dtUnassignedSales.AsEnumerable().GroupBy(x => x["TSRAssignedTo"]).Select(x => x.FirstOrDefault())).CopyToDataTable();
                                //DataTable dtTSRs = (dtUnassignedSales.AsEnumerable().GroupBy(r => new {c1 = r.Field<long>("FKStaffTypeID"), c2 = r.Field<long>("TSRAssignedTo") }).Select(r => r.FirstOrDefault())).CopyToDataTable();
                                //DataTable dtTSRs = (dtUnassignedSales.AsEnumerable().GroupBy(x => x["FKStaffTypeID"]).Select(r => r.FirstOrDefault())).CopyToDataTable();


                                Database.BeginTransaction(null, IsolationLevel.Snapshot);

                                foreach (DataRow tsr in dtTSRs.Rows)
                                {
                                    count++;
                                    //DataRow drSale = (from sale in dtUnassignedSales.AsEnumerable()
                                    //                  where sale.Field<long>("TSRAssignedTo") == long.Parse(tsr["TSRAssignedTo"].ToString())
                                    //                  orderby sale["TSRAssignedTo"] descending
                                    //                  select sale).First();

                                    DataRow drSale = dtUnassignedSales.AsEnumerable().Where(sale => long.Parse(sale["TSRAssignedTo"].ToString()) == long.Parse(tsr["TSRAssignedTo"].ToString())).FirstOrDefault();
                                    //DataRow drSale = dtUnassignedSales.AsEnumerable().Where(sale => long.Parse(sale["FKStaffTypeID"].ToString()) == long.Parse(tsr["FKStaffTypeID"].ToString())).FirstOrDefault();

                                    CallMonitoringAllocation callMonitoringAllocation = new CallMonitoringAllocation();
                                    callMonitoringAllocation.FKINImportID = long.Parse(drSale["ImportID"].ToString());
                                    callMonitoringAllocation.FKUserID = userID;
                                    callMonitoringAllocation.Save(_validationResult);
                                    //callMonitoringAllocation.FKINImportID = 
                                    //inImport.FKUserID = userID;
                                    ////inImport.AllocationDate = DateTime.Now;
                                    //inImport.AllocationDate = DetermineAllocationDate(_useDifferentAllocationDate, _newAllocationDate);
                                    //inImport.BonusLead = chkBonusLeads.IsChecked;
                                    //inImport.Save(_validationResult);

                                    record.Cells["Assign"].Value = Convert.ToInt64(record.Cells["Assign"].Value.ToString()) - 1;
                                    record.Cells["LeadsAllocated"].Value = Convert.ToInt64(record.Cells["LeadsAllocated"].Value.ToString()) + 1;
                                    tbTotalAssigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                                    {
                                        tbTotalAssigned.Text = (Convert.ToInt64(tbTotalAssigned.Text) + 1).ToString();
                                    }));
                                    tbTotalUnassigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                                    {
                                        tbTotalUnassigned.Text = (Convert.ToInt64(tbTotalUnassigned.Text) - 1).ToString();
                                    }));

                                    if (count == assign /*|| Math.Abs(count) == dtUnassignedSales.Rows.Count*/)
                                    {
                                        record.Cells["Assign"].Value = 0;
                                        break;
                                    }
                                    dtUnassignedSales.Rows.Remove(drSale);
                                }

                                CommitTransaction(null);

                            }

                        }
                        else
                        {
                            record.Cells["Assign"].Value = 0;
                        }
                    }
                    else if (assign < 0)
                    {
                        //CallMonitoringAllocationCollection callMonitoringAllocationCollection = CallMonitoringAllocationMapper.Search(;

                        //if (inImportCollection.Count > 0)
                        //{
                        //    Database.BeginTransaction(null, IsolationLevel.Snapshot);
                        //    int[] i = { 0 };
                        //    foreach (INImport inImport in inImportCollection)
                        //    {
                        //        i[0]--;
                        //        inImport.FKUserID = null;
                        //        inImport.AllocationDate = null;
                        //        inImport.Save(_validationResult);

                        //        record.Cells["Assign"].Value = Convert.ToInt64(record.Cells["Assign"].Value.ToString()) + 1;
                        //        record.Cells["LeadsAllocated"].Value = Convert.ToInt64(record.Cells["LeadsAllocated"].Value.ToString()) - 1;
                        //        tbTotalAssigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                        //        {
                        //            tbTotalAssigned.Text = (Convert.ToInt64(tbTotalAssigned.Text) - 1).ToString();
                        //        }));
                        //        tbTotalUnassigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                        //        {
                        //            tbTotalUnassigned.Text = (Convert.ToInt64(tbTotalUnassigned.Text) + 1).ToString();
                        //        }));

                        //        if (i[0] == assign || Math.Abs(i[0]) == inImportCollection.Count)
                        //        {
                        //            record.Cells["Assign"].Value = 0;
                        //            break;
                        //        }
                        //    }
                        //    CommitTransaction(null);
                        //}
                        //else
                        //{
                        //    record.Cells["Assign"].Value = 0;
                        //}
                    }
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private bool? AllRecordsSelected()
        {
            try
            {
                bool allSelected = true;
                bool noneSelected = true;

                if (xdgAssignSales.DataSource != null)
                {
                    foreach (DataRow dr in ((DataView)xdgAssignSales.DataSource).Table.Rows)
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

        #endregion Private Methods

        #region Event Handlers

        private void btnAssign_Click(object sender, RoutedEventArgs e)
        {
            btnAssign.IsEnabled = false;
            btnClose.IsEnabled = false;
            //btnAgents.IsEnabled = false;
            xdgAssignSales.IsEnabled = false;
            xdgAssignSales.ActiveRecord = null;

            //if (CanLeadsBeAllocated())
            //{

            //AssignLeadsByRatio2(); //old sorting by ratio without saved carried forwards.
            AssignLeadsByRatio2SavedCF();
            //}

            btnAssign.IsEnabled = true;
            btnClose.IsEnabled = true;
            //btnAgents.IsEnabled = true;
            xdgAssignSales.IsEnabled = true;
        }

        private void btnAutoAssign_Click(object sender, RoutedEventArgs e)
        {
            AutoAssignSales();
        }

        

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(false);
        }

        private void chkUseDifferentAllocationDate_Checked(object sender, RoutedEventArgs e)
        {
            lblNewAllocationDate.Visibility = Visibility.Visible;
            dteNewAllocationDate.Visibility = Visibility.Visible;
        }

        private void chkUseDifferentAllocationDate_Unchecked(object sender, RoutedEventArgs e)
        {
            lblNewAllocationDate.Visibility = Visibility.Hidden;
            dteNewAllocationDate.Visibility = Visibility.Hidden;

            dteNewAllocationDate.Value = null;
            _newAllocationDate = DateTime.Now;
        }

        private void dteNewAllocationDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DateTime selectedDateTime;

            if (dteNewAllocationDate.Value == null)
            {
                _newAllocationDate = DateTime.Now;
            }
            else
            {
                selectedDateTime = Convert.ToDateTime(dteNewAllocationDate.Value);
                _newAllocationDate = new DateTime(selectedDateTime.Year, selectedDateTime.Month, selectedDateTime.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            }
        }

        private void xdgAssignSales_Loaded(object sender, RoutedEventArgs e)
        {
            xdgAssignSales.Focus();
        }

        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = ((DataView)xdgAssignSales.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = true;
                }

                //EnableDisableExportButton();
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
                DataTable dt = ((DataView)xdgAssignSales.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = false;
                }

                //EnableDisableExportButton();
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
                    //allRecordsSelected = (bool)AllRecordsSelected();
                }

                //EnableDisableExportButton();
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


        #endregion Event Handlers


    }
}

