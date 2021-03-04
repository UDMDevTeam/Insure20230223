using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Embriant.Framework;
using Embriant.Framework.Data;
using Infragistics.Windows.DataPresenter;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;


namespace UDM.Insurance.Interface.Screens
{
	public partial class AssignPossibleBumpUpsScreen
    {
        #region Constant

        #endregion Constant

        #region Private Members

        //private readonly long _batchID;
        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        private readonly DateTime _dateOfSale;
        private readonly string _campaignGroup;
        private readonly string _campaignIDs;
        private DataTable _dtBatch;
        private DataTable _dtAgents;
        private DateTime _newAllocationDate;
        private bool _useDifferentAllocationDate = false;
        private long autoAssignCount = 0;

        #endregion Private Members

        #region Constructors

        public AssignPossibleBumpUpsScreen(string campaignGroup, DateTime dateOfSale, string campaignIDs)
        {
            InitializeComponent();

            //_batchID = batchID;
            _dateOfSale = dateOfSale;
            _campaignGroup = campaignGroup;
            _campaignIDs = campaignIDs;

            LoadLookupData();
        }

        #endregion Constructors

        #region Private Methods

        private void LoadLookupData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataSet ds = Insure.INGetDateOfSaleAssignedPossibleBumpUpsData(_dateOfSale, _campaignIDs, 1, 1);
                _dtBatch = ds.Tables[0];
                _dtAgents = ds.Tables[1];

                tbCampaign.Text = _campaignGroup;
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


        #region Not Used
        private void GetLeads(INImportCollection inLeads, string str, out INImportCollection foundLeads, out INImportCollection outLeads)
        {
            StringBuilder strQuery;
            int[] arrCount = { 0, inLeads.Count, 0, 0 };
            foundLeads = new INImportCollection();
            outLeads = new INImportCollection();

            switch (str)
            {
                case "Re-Prime":

                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                    {
                        lblAllocateMessage1.Text = "Processing Available Leads";
                        lblAllocateMessage2.Text = "Re-Primed (0)";
                        lblAllocateMessage3.Text = "";
                        bAllocateMessage.Visibility = Visibility.Visible;
                    }));

                    foreach (INImport inImport in inLeads)
                    {
                        arrCount[0]++;
                        strQuery = new StringBuilder();
                        //strQuery.AppendFormat("SELECT COUNT(INImportOther.ID) [Count] FROM INImportOther WHERE FKINImportID = '{0}' AND (ReferralFrom IS NULL OR ReferralFrom = '')", inImport.ID);
                        strQuery.AppendFormat("SELECT COUNT([INImport].[ID]) AS [Count] FROM [INImport] WHERE LTRIM(RTRIM([Testing1])) = 'Re-Prime'");

                        if (Convert.ToInt32(Methods.GetTableData(strQuery.ToString()).Rows[0]["Count"]) == 1)
                        {
                            arrCount[2]++;
                            foundLeads.Add(inImport);
                            continue;
                        }

                        outLeads.Add(inImport);

                        Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                        {
                            lblAllocateMessage2.Text = $"Re-Primed ({arrCount[2]})";
                            lblAllocateMessage3.Text = $"{arrCount[0]} / {arrCount[1]}";
                        }));
                    }

                    break;

                case "Housewife":

                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                    {
                        lblAllocateMessage1.Text = "Processing Available Leads";
                        lblAllocateMessage2.Text = "Housewife (0)";
                        lblAllocateMessage3.Text = "";
                        bAllocateMessage.Visibility = Visibility.Visible;
                    }));

                    foreach (INImport inImport in inLeads)
                    {
                        arrCount[0]++;
                        strQuery = new StringBuilder();
                        strQuery.AppendFormat("SELECT COUNT(INLead.ID) [Count] FROM INLead WHERE ID = '{0}' AND Occupation LIKE 'Housewife%'", inImport.FKINLeadID);

                        if (Convert.ToInt32(Methods.GetTableData(strQuery.ToString()).Rows[0]["Count"]) == 1)
                        {
                            arrCount[2]++;
                            foundLeads.Add(inImport);
                            continue;
                        }

                        outLeads.Add(inImport);

                        Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                        {
                            lblAllocateMessage2.Text = $"Housewife ({arrCount[2]})";
                            lblAllocateMessage3.Text = $"{arrCount[0]} / {arrCount[1]}";
                        }));
                    }

                    break;

                case "Pensioner":

                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                    {
                        lblAllocateMessage1.Text = "Processing Available Leads";
                        lblAllocateMessage2.Text = "Pensioner (0)";
                        lblAllocateMessage3.Text = "";
                        bAllocateMessage.Visibility = Visibility.Visible;
                    }));

                    foreach (INImport inImport in inLeads)
                    {
                        arrCount[0]++;
                        strQuery = new StringBuilder();
                        strQuery.AppendFormat("SELECT COUNT(INLead.ID) [Count] FROM INLead WHERE ID = '{0}' AND Occupation LIKE 'Pensioner%'", inImport.FKINLeadID);

                        if (Convert.ToInt32(Methods.GetTableData(strQuery.ToString()).Rows[0]["Count"]) == 1)
                        {
                            arrCount[3]++;
                            foundLeads.Add(inImport);
                            continue;
                        }

                        outLeads.Add(inImport);

                        Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                        {
                            lblAllocateMessage2.Text = $"Pensioner ({arrCount[3]})";
                            lblAllocateMessage3.Text = $"{arrCount[0]} / {arrCount[1]}";
                        }));
                    }

                    break;

                case "IndianSurnames":

                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                    {
                        lblAllocateMessage1.Text = "Processing Available Leads";
                        lblAllocateMessage2.Text = "Indian Surname (0)";
                        lblAllocateMessage3.Text = "";
                        bAllocateMessage.Visibility = Visibility.Visible;
                    }));

                    DataTable dtIndianSurnames = Methods.GetTableData("SELECT Surname FROM Surnames WHERE FKSurnameType = '1'");
                    string strSurname;

                    foreach (INImport inImport in inLeads)
                    {
                        arrCount[0]++;
                        strQuery = new StringBuilder();
                        strQuery.AppendFormat("SELECT Surname FROM INLead WHERE ID = '{0}'", inImport.FKINLeadID);

                        strSurname = Methods.GetTableData(strQuery.ToString()).Rows[0]["Surname"] as string;
                        DataRow[] dr = dtIndianSurnames.Select("Surname = '" + strSurname + "'");

                        if (dr.Length != 0)
                        {
                            arrCount[3]++;
                            foundLeads.Add(inImport);
                            continue;
                        }

                        outLeads.Add(inImport);

                        Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                        {
                            lblAllocateMessage2.Text = $"Indian Surname ({arrCount[3]})";
                            lblAllocateMessage3.Text = $"{arrCount[0]} / {arrCount[1]}";
                        }));
                    }

                    break;

                default:
                    outLeads = null;
                    foundLeads = null;
                    break;
            }

            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
            {
                bAllocateMessage.Visibility = Visibility.Collapsed;
                lblAllocateMessage1.Text = "";
                lblAllocateMessage2.Text = "";
                lblAllocateMessage3.Text = "";
            }));
        }

        private int ProcessLeads(INImportCollection availableLeads, int totalLeadsToAllocate, string strType, out INImportCollection leadsOfType, out INImportCollection leadsAvailable)
        {
            INImportCollection leadsIn = new INImportCollection();
            INImportCollection leadsOut;
            INImportCollection leadsFound;

            leadsOfType = new INImportCollection();
            leadsAvailable = new INImportCollection();

            foreach (INImport lead in availableLeads)
            {
                leadsIn.Add(lead);
            }

            GetLeads(leadsIn, strType, out leadsFound, out leadsOut);

            foreach (INImport lead in leadsFound)
            {
                leadsOfType.Add(lead);
            }
            foreach (INImport lead in leadsOut)
            {
                leadsAvailable.Add(lead);
            }

            if (availableLeads.Count > 0)
            {
                decimal percTypeLeads = Math.Round((decimal)leadsFound.Count / availableLeads.Count * 100);
                int noOfTypeLeadsToAllocate = (int)Math.Round(totalLeadsToAllocate * percTypeLeads / 100);
                if (percTypeLeads > 0.000m)
                {
                    if (noOfTypeLeadsToAllocate == 0) noOfTypeLeadsToAllocate = 1;
                }
                if (noOfTypeLeadsToAllocate > leadsOfType.Count)
                {
                    noOfTypeLeadsToAllocate = leadsOfType.Count;
                }

                return noOfTypeLeadsToAllocate;
            }

            return 0;
        }

        private void RandomizeLeads(INImportCollection coll)
        {
            Random random = new Random();
            for (int index1 = 0; index1 < coll.Count; index1++)
            {
                int index2 = random.Next(0, coll.Count);

                var temp = coll[index1];
                coll[index1] = coll[index2];
                coll[index2] = temp;
            }
        }
        #endregion


        public void AssignLeads()
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
                        dtUnassignedSales = Insure.INGetUnassignedPossibleBumpUpAllocationsByDateOfSale(_dateOfSale, _campaignIDs);

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

                                 Database.BeginTransaction(null, IsolationLevel.Snapshot);

                                foreach (DataRow tsr in dtTSRs.Rows)
                                {
                                    count++;
                                    //DataRow drSale = (from sale in dtUnassignedSales.AsEnumerable()
                                    //                  where sale.Field<long>("TSRAssignedTo") == long.Parse(tsr["TSRAssignedTo"].ToString())
                                    //                  orderby sale["TSRAssignedTo"] descending
                                    //                  select sale).First();

                                    DataRow drSale = dtUnassignedSales.AsEnumerable().Where(sale => long.Parse(sale["TSRAssignedTo"].ToString()) == long.Parse(tsr["TSRAssignedTo"].ToString())).FirstOrDefault();

                                    PossibleBumpUpAllocation possibleBumpUpAllocation = new PossibleBumpUpAllocation();
                                    possibleBumpUpAllocation.FKINImportID = long.Parse(drSale["ImportID"].ToString());
                                    possibleBumpUpAllocation.FKUserID = userID;
                                    possibleBumpUpAllocation.Save(_validationResult);

                                    INImport inImport = new INImport(long.Parse(drSale["ImportID"].ToString()));
                                    inImport.FKConfCallRefUserID = userID;
                                    inImport.Save(_validationResult);
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
                        
                        DataTable dtLeadsToUnallocate = Insure.GetBUAllocationsNotWorkedOn(_dateOfSale, _campaignIDs, userID);

                        if (dtLeadsToUnallocate.Rows.Count > 0)
                        {
                            //Database.BeginTransaction(null, IsolationLevel.Snapshot);
                            int[] i = { 0 };
                            foreach (DataRow possibleBumpUpAllocation in dtLeadsToUnallocate.Rows)
                            {
                                i[0]--;

                                INImport iNImport = new INImport(Convert.ToInt64(possibleBumpUpAllocation["FKINImportID"]));
                                iNImport.FKConfCallRefUserID = null;
                                iNImport.Save(_validationResult);
                                Database.ExecuteCommand("DELETE FROM PossibleBumpUpAllocation WHERE ID = " + Convert.ToInt64(possibleBumpUpAllocation["ID"]));

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

        //private bool CanLeadsBeAllocated()
        //{
        //    if (chkUseDifferentAllocationDate.IsChecked.HasValue)
        //    {
        //        _useDifferentAllocationDate = chkUseDifferentAllocationDate.IsChecked.Value;
        //    }

        //    if (_useDifferentAllocationDate)
        //    {
        //        if (dteNewAllocationDate.Value == null)
        //        {
        //            ShowMessageBox(new INMessageBoxWindow1(), "Because you have indicated that you want to use a different allocation date, you need to specify the new allocation date.", "Allocation Date Not Specified", ShowMessageType.Error);
        //            dteNewAllocationDate.Focus();
        //            return false;
        //        }

        //        _newAllocationDate = Convert.ToDateTime(dteNewAllocationDate.Value);

        //        if (_newAllocationDate.Date < DateTime.Now.Date)
        //        {
        //            ShowMessageBox(new INMessageBoxWindow1(), "The new allocation date is invalid, because it is in the past.", "Invalid Allocation Date", ShowMessageType.Error);
        //            dteNewAllocationDate.Focus();
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        private DateTime DetermineAllocationDate(bool useDifferentAllocationDate, DateTime selectedDateTime)
        {
            DateTime dtResult;

            if (!useDifferentAllocationDate)
            {
                dtResult = DateTime.Now;
            }
            else
            {
                dtResult = new DateTime(selectedDateTime.Year, selectedDateTime.Month, selectedDateTime.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            }

            return dtResult;
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
                AssignLeads();
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
                //dtAgents.Columns.Add("Index");
                //int rowCount = 0;
                //foreach (DataRow row in dtAgents.Rows)
                //{
                //    row["Index"] = rowCount++;
                //}
                //DataTable dtAgentsToBeSorted = dtAgents.AsEnumerable().Where(x => bool.Parse(x["Select"].ToString()) == true).CopyToDataTable();
                DataRow skipValIndex = dtAgents.AsEnumerable().Where(x => bool.Parse(x["Select"].ToString()) == true).OrderBy(x => x["LeadsAllocated"]).ThenBy(x => x["SalesAgent"]).FirstOrDefault();
                skipVal = int.Parse(skipValIndex["Index"].ToString());
                maxCurrentAssignValue = long.Parse(skipValIndex["LeadsAllocated"].ToString()) + long.Parse(skipValIndex["Assign"].ToString()) + 1;
                autoAssignCount = dtAgents.AsEnumerable().Sum(x => x.Field<long>("Assign"));
                //if (unassigned > 0)
                //{
                //    salesPerAgent = unassigned / (decimal)dtAgentsToBeSorted.Rows.Count;
                //}
                //autoAssignCount = 0;
                while (autoAssignCount < unassigned)
                {
                    skipValIndex = dtAgents.AsEnumerable().Where(x => bool.Parse(x["Select"].ToString()) == true).OrderBy(x => long.Parse(x["LeadsAllocated"].ToString()) + long.Parse(x["Assign"].ToString()))/*.ThenBy(x => x["Assign"])*/.ThenBy(x => x["SalesAgent"]).FirstOrDefault();

                    ((DataRecord)xdgAssignSales.Records[int.Parse(skipValIndex["Index"].ToString())]).Cells["Assign"].Value = long.Parse(((DataRecord)xdgAssignSales.Records[int.Parse(skipValIndex["Index"].ToString())]).Cells["Assign"].Value.ToString()) + 1;
                    autoAssignCount++;
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
                }

                
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        //private void btnAgents_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        SetCursor(Cursors.Wait);

        //        DataSet ds = null;
        //        xdgAssignSales.DataSource = null;

        //        switch (xdgAssignSales.Tag.ToString())
        //        {
        //            case "Active, Employed":
        //                ds = Insure.INGetDateOfSaleAssignedSalesData(_dateOfSale, _campaignGroup, 1, 0);
        //                xdgAssignSales.Tag = "Active, All";
        //                goto case "Default";

        //            case "Active, All":
        //                ds = Insure.INGetDateOfSaleAssignedSalesData(_dateOfSale, _campaignGroup, 1, 0);
        //                xdgAssignSales.Tag = "Active, Employed";
        //                goto case "Default";

        //            //case "All":
        //            //    ds = Insure.INGetBatchAssignedLeadsData(_batchID, 1, 1);
        //            //    xdgAssignSales.Tag = "Active, Employed";
        //            //    goto case "Default";

        //            case "Default":
        //                if (ds != null)
        //                {
        //                    _dtBatch = ds.Tables[0];
        //                    _dtAgents = ds.Tables[1];
        //                    xdgAssignSales.DataSource = _dtAgents.DefaultView;
        //                    tbAgents.Text = string.Format("{0} ({1})", xdgAssignSales.Tag, _dtAgents.Rows.Count);
        //                }
        //                else
        //                {
        //                    tbAgents.Text = "Error!";
        //                }
        //                break;
        //        }
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

