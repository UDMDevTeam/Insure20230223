using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using Embriant.Framework;
using Embriant.Framework.Data;
using Infragistics.Windows.DataPresenter;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;


namespace UDM.Insurance.Interface.Screens
{
	public partial class AssignMiningLeadsScreen
    {
        #region Constant

        #endregion Constant

        #region Properties

        public bool IsGoldenLeads { get; set; }

        #endregion 

        #region Private Members

        private readonly long _leadBookID;
        private long _batchID;
        private DataTable _dtLeadBook;
        private DataTable _dtBatch;
        private DataTable _dtAgents;
        //private DateTime _newAllocationDate;
        //private bool _useDifferentAllocationDate = false;

        #endregion Private Members

        #region Constructors

        public AssignMiningLeadsScreen(long leadBookID)
        {
            InitializeComponent();

            _leadBookID = leadBookID;

            LoadLookupData();
        }

        #endregion Constructors

        #region Private Methods

        private void LoadLookupData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataSet ds = Insure.INGetLeadBookAssignedMiningLeadsData(_leadBookID, 1, 1);
                _dtLeadBook = ds.Tables[0];
                _dtBatch = ds.Tables[1];
                _dtAgents = ds.Tables[2];

                tbCampaign.Text = _dtBatch.Rows[0]["CampaignName"].ToString();
                tbBatch.Text = _dtBatch.Rows[0]["BatchCode"].ToString();
                tbUDMBatch.Text = _dtBatch.Rows[0]["UDMBatchCode"].ToString();
                tbLeadbook.Text = _dtLeadBook.Rows[0]["LeadBookTitle"].ToString();
                tbOriginalAgent.Text = _dtLeadBook.Rows[0]["AgentName"].ToString();
                tbStillToContact.Text = _dtLeadBook.Rows[0]["StillToContact"].ToString();
                //tbTotalAssigned.Text = _dtBatch.Rows[0]["Assigned"].ToString();
                //tbTotalUnassigned.Text = _dtBatch.Rows[0]["Unassigned"].ToString();
                ////tbTotalPrinted.Text = _dtBatch.Rows[0]["Printed"].ToString();

                //xdgAssignLeads.Tag = "Active, Employed";
                //tbAgents.Text = string.Format("{0} ({1})", xdgAssignLeads.Tag, _dtAgents.Rows.Count);

                xdgAssignLeads.DataSource = _dtAgents.DefaultView;
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

        //private void GetLeads(INImportCollection inLeads, string str, out INImportCollection foundLeads, out INImportCollection outLeads)
        //{
        //    StringBuilder strQuery;
        //    int[] arrCount = { 0, inLeads.Count, 0, 0 };
        //    foundLeads = new INImportCollection();
        //    outLeads = new INImportCollection();

        //    switch (str)
        //    {
        //        case "Re-Prime":

        //            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
        //            {
        //                lblAllocateMessage1.Text = "Processing Available Leads";
        //                lblAllocateMessage2.Text = "Re-Primed (0)";
        //                lblAllocateMessage3.Text = "";
        //                bAllocateMessage.Visibility = Visibility.Visible;
        //            }));

        //            foreach (INImport inImport in inLeads)
        //            {
        //                arrCount[0]++;
        //                strQuery = new StringBuilder();
        //                //strQuery.AppendFormat("SELECT COUNT(INImportOther.ID) [Count] FROM INImportOther WHERE FKINImportID = '{0}' AND (ReferralFrom IS NULL OR ReferralFrom = '')", inImport.ID);
        //                strQuery.AppendFormat("SELECT COUNT([INImport].[ID]) AS [Count] FROM [INImport] WHERE LTRIM(RTRIM([Testing1])) = 'Re-Prime'");

        //                if (Convert.ToInt32(Methods.GetTableData(strQuery.ToString()).Rows[0]["Count"]) == 1)
        //                {
        //                    arrCount[2]++;
        //                    foundLeads.Add(inImport);
        //                    continue;
        //                }

        //                outLeads.Add(inImport);

        //                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
        //                {
        //                    lblAllocateMessage2.Text = $"Re-Primed ({arrCount[2]})";
        //                    lblAllocateMessage3.Text = $"{arrCount[0]} / {arrCount[1]}";
        //                }));
        //            }

        //            break;

        //        case "Housewife":

        //            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
        //            {
        //                lblAllocateMessage1.Text = "Processing Available Leads";
        //                lblAllocateMessage2.Text = "Housewife (0)";
        //                lblAllocateMessage3.Text = "";
        //                bAllocateMessage.Visibility = Visibility.Visible;
        //            }));

        //            foreach (INImport inImport in inLeads)
        //            {
        //                arrCount[0]++;
        //                strQuery = new StringBuilder();
        //                strQuery.AppendFormat("SELECT COUNT(INLead.ID) [Count] FROM INLead WHERE ID = '{0}' AND Occupation LIKE 'Housewife%'", inImport.FKINLeadID);

        //                if (Convert.ToInt32(Methods.GetTableData(strQuery.ToString()).Rows[0]["Count"]) == 1)
        //                {
        //                    arrCount[2]++;
        //                    foundLeads.Add(inImport);
        //                    continue;
        //                }

        //                outLeads.Add(inImport);

        //                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
        //                {
        //                    lblAllocateMessage2.Text = $"Housewife ({arrCount[2]})";
        //                    lblAllocateMessage3.Text = $"{arrCount[0]} / {arrCount[1]}";
        //                }));
        //            }

        //            break;

        //        case "Pensioner":

        //            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
        //            {
        //                lblAllocateMessage1.Text = "Processing Available Leads";
        //                lblAllocateMessage2.Text = "Pensioner (0)";
        //                lblAllocateMessage3.Text = "";
        //                bAllocateMessage.Visibility = Visibility.Visible;
        //            }));

        //            foreach (INImport inImport in inLeads)
        //            {
        //                arrCount[0]++;
        //                strQuery = new StringBuilder();
        //                strQuery.AppendFormat("SELECT COUNT(INLead.ID) [Count] FROM INLead WHERE ID = '{0}' AND Occupation LIKE 'Pensioner%'", inImport.FKINLeadID);

        //                if (Convert.ToInt32(Methods.GetTableData(strQuery.ToString()).Rows[0]["Count"]) == 1)
        //                {
        //                    arrCount[3]++;
        //                    foundLeads.Add(inImport);
        //                    continue;
        //                }

        //                outLeads.Add(inImport);

        //                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
        //                {
        //                    lblAllocateMessage2.Text = $"Pensioner ({arrCount[3]})";
        //                    lblAllocateMessage3.Text = $"{arrCount[0]} / {arrCount[1]}";
        //                }));
        //            }

        //            break;

        //        case "IndianSurnames":

        //            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
        //            {
        //                lblAllocateMessage1.Text = "Processing Available Leads";
        //                lblAllocateMessage2.Text = "Indian Surname (0)";
        //                lblAllocateMessage3.Text = "";
        //                bAllocateMessage.Visibility = Visibility.Visible;
        //            }));

        //            DataTable dtIndianSurnames = Methods.GetTableData("SELECT Surname FROM Surnames WHERE FKSurnameType = '1'");
        //            string strSurname;

        //            foreach (INImport inImport in inLeads)
        //            {
        //                arrCount[0]++;
        //                strQuery = new StringBuilder();
        //                strQuery.AppendFormat("SELECT Surname FROM INLead WHERE ID = '{0}'", inImport.FKINLeadID);

        //                strSurname = Methods.GetTableData(strQuery.ToString()).Rows[0]["Surname"] as string;
        //                DataRow[] dr = dtIndianSurnames.Select("Surname = '" + strSurname + "'");

        //                if (dr.Length != 0)
        //                {
        //                    arrCount[3]++;
        //                    foundLeads.Add(inImport);
        //                    continue;
        //                }

        //                outLeads.Add(inImport);

        //                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
        //                {
        //                    lblAllocateMessage2.Text = $"Indian Surname ({arrCount[3]})";
        //                    lblAllocateMessage3.Text = $"{arrCount[0]} / {arrCount[1]}";
        //                }));
        //            }

        //            break;

        //        default:
        //            outLeads = null;
        //            foundLeads = null;
        //            break;
        //    }

        //    Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
        //    {
        //        bAllocateMessage.Visibility = Visibility.Collapsed;
        //        lblAllocateMessage1.Text = "";
        //        lblAllocateMessage2.Text = "";
        //        lblAllocateMessage3.Text = "";
        //    }));
        //}

        //private int ProcessLeads(INImportCollection availableLeads, int totalLeadsToAllocate, string strType, out INImportCollection leadsOfType, out INImportCollection leadsAvailable)
        //{
        //    INImportCollection leadsIn = new INImportCollection();
        //    INImportCollection leadsOut;
        //    INImportCollection leadsFound;

        //    leadsOfType = new INImportCollection();
        //    leadsAvailable = new INImportCollection();

        //    foreach (INImport lead in availableLeads)
        //    {
        //        leadsIn.Add(lead);
        //    }
            
        //    GetLeads(leadsIn, strType, out leadsFound, out leadsOut);

        //    foreach (INImport lead in leadsFound)
        //    {
        //        leadsOfType.Add(lead);
        //    }
        //    foreach (INImport lead in leadsOut)
        //    {
        //        leadsAvailable.Add(lead);
        //    }

        //    if (availableLeads.Count > 0)
        //    {
        //        decimal percTypeLeads = Math.Round((decimal)leadsFound.Count / availableLeads.Count * 100);
        //        int noOfTypeLeadsToAllocate = (int)Math.Round(totalLeadsToAllocate * percTypeLeads / 100);
        //        if (percTypeLeads > 0.000m)
        //        {
        //            if (noOfTypeLeadsToAllocate == 0) noOfTypeLeadsToAllocate = 1;
        //        }
        //        if (noOfTypeLeadsToAllocate > leadsOfType.Count)
        //        {
        //            noOfTypeLeadsToAllocate = leadsOfType.Count;
        //        }

        //        return noOfTypeLeadsToAllocate;
        //    }

        //    return 0;
        //}

        //private void RandomizeLeads(INImportCollection coll)
        //{
        //    Random random = new Random();
        //    for (int index1 = 0; index1 < coll.Count; index1++)
        //    {
        //        int index2 = random.Next(0, coll.Count);

        //        var temp = coll[index1];
        //        coll[index1] = coll[index2];
        //        coll[index2] = temp;
        //    }
        //}

        public void AssignLeads()
        {
            try
            {
                if (xdgAssignLeads.SelectedItems.Records.Count > 0 && Convert.ToInt32(_dtLeadBook.Rows[0]["StillToContact"].ToString()) > 0)
                {
                    DateTime allocationDate = DateTime.Now;
                    var record = (DataRecord)xdgAssignLeads.SelectedItems.Records[0];
                    long userID = long.Parse(record.Cells["UserID"].Value.ToString());

                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@LeadBookID", _leadBookID);
                    DataSet dsLeads = Methods.ExecuteStoredProcedure2("spINGetStillToContactLeads", parameters, IsolationLevel.Snapshot, 300);
                    DataTable dtLeads = dsLeads.Tables[0];

                    if (dtLeads.Rows.Count > 0)
                    {
                        Database.BeginTransaction(null, IsolationLevel.Snapshot);

                        foreach (DataRow row in dtLeads.Rows)
                        {
                            long importID = Convert.ToInt64(row["ImportID"]);
                            short rank = Convert.ToInt16(row["MaxRank"]);
                            rank = (short)(rank == 0 ? 2 : rank + 1);

                            INImport inImport = new INImport(importID);
                            inImport.FKUserID = userID;
                            inImport.AllocationDate = allocationDate;
                            inImport.Save(_validationResult);


                            INImportExtra iNImportExtra = INImportExtraMapper.SearchOne(inImport.ID, null);
                            if (iNImportExtra == null)
                            {
                                iNImportExtra = new INImportExtra();
                            }
                            iNImportExtra.FKINImportID = inImport.ID;
                            iNImportExtra.IsGoldenLead = IsGoldenLeads ? true : false;
                            iNImportExtra.Save(_validationResult);


                            INImportMining inImportMining = new INImportMining();
                            inImportMining.FKImportID = importID;
                            inImportMining.AllocationDate = allocationDate;
                            inImportMining.FKUserID = userID;
                            inImportMining.Rank = rank;
                            inImportMining.Save(_validationResult);
                        }

                        CommitTransaction(null);
                    }

                    LoadLookupData();
                }



                //foreach (Record rec in xdgAssignLeads.Records)
                //{
                //    var record = (DataRecord) rec;

                //    long userID = long.Parse(record.Cells["UserID"].Value.ToString());
                //    long assign = long.Parse(record.Cells["Assign"].Value.ToString());

                //    INBatch inBatch = new INBatch(_batchID);
                //    INCampaign inCampaign = new INCampaign(Convert.ToInt32(inBatch.FKINCampaignID));

                //    if (assign > 0)
                //    {
                //        //INImportCollection inImportCollection = INImportMapper.Search(null, _batchID, null);
                //        INImportCollection inImportCollection = INImportMapper.Search(null, null, _batchID, null);

                //        if (assign > inImportCollection.Count)
                //        {
                //            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                //            {
                //                INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                //                ShowMessageBox(messageWindow, inImportCollection.Count + " leads available.", "Not Enough Leads Available", ShowMessageType.Error);
                //            }));

                //            return;
                //        }

                //        if (inImportCollection.Count > 0)
                //        {
                //            //RandomizeLeads(inImportCollection);

                //            #region allocation for specific campaigns or Occupation

                //            { 

                //                #region ACCDIS
                //                //if (inCampaign.Code == "ACCDIS")
                //                //{
                //                //    INImportCollection collCampaign = new INImportCollection();
                                    
                //                //    int countMacc = Methods.GetTableData(string.Format("SELECT RefNo From INImport WHERE FKINBatchID = '{0}' AND FKUserID IS NULL AND RefNo Like 'gdna%'", _batchID)).Rows.Count;
                //                //    int countMaccMillion = Methods.GetTableData(string.Format("SELECT RefNo From INImport WHERE FKINBatchID = '{0}' AND FKUserID IS NULL AND RefNo Like 'gdnm%'", _batchID)).Rows.Count;
                //                //    int count = countMacc + countMaccMillion;

                //                //    decimal maccPerc = Math.Round((decimal)countMacc / count * 100);

                //                //    int noOfMacc = (int)Math.Round(assign * maccPerc / 100);
                //                //    if (maccPerc > 0.000m)
                //                //    {
                //                //        if (noOfMacc == 0) noOfMacc = 1;
                //                //    }
                //                //    int noOfMaccMillion = (int)(assign - noOfMacc);

                //                //    foreach (INImport inImport in inImportCollection)
                //                //    {
                //                //        if (noOfMacc > 0 && inImport.RefNo.Substring(0, 4) == "gdna")
                //                //        {
                //                //            collCampaign.Add(inImport);
                //                //            noOfMacc--;
                //                //        }
                //                //        if (noOfMaccMillion > 0 && inImport.RefNo.Substring(0, 4) == "gdnm")
                //                //        {
                //                //            collCampaign.Add(inImport);
                //                //            noOfMaccMillion--;
                //                //        }

                //                //        if (noOfMacc == 0 && noOfMaccMillion == 0) break;
                //                //    }

                //                //    inImportCollection = collCampaign;
                //                //}
                //                #endregion

                //                #region PLMMBE
                //                //else if (inCampaign.Code == "PLMMBE")
                //                //{
                //                //    INImportCollection collCampaign = new INImportCollection();

                //                //    int countMaccMillion =
                //                //        Methods.GetTableData($"SELECT RefNo From INImport WHERE FKINBatchID = '{_batchID}' AND FKUserID IS NULL AND RefNo Like 'gdnm%'")
                //                //            .Rows.Count;
                //                //    int countBlackMaccMillion =
                //                //        Methods.GetTableData($"SELECT RefNo From INImport WHERE FKINBatchID = '{_batchID}' AND FKUserID IS NULL AND RefNo Like 'gdbm%'")
                //                //            .Rows.Count;
                //                //    int countTotal = countMaccMillion + countBlackMaccMillion;

                //                //    decimal percMaccMillion = Math.Round((decimal) countMaccMillion/countTotal*100);

                //                //    int noOfMaccMillion = (int) Math.Round(assign*percMaccMillion/100);
                //                //    if (percMaccMillion > 0.000m)
                //                //    {
                //                //        if (noOfMaccMillion == 0) noOfMaccMillion = 1;
                //                //    }
                //                //    int noOfBlackMaccMillion = (int) (assign - noOfMaccMillion);

                //                //    foreach (INImport inImport in inImportCollection)
                //                //    {
                //                //        if (noOfMaccMillion > 0 && inImport.RefNo.Substring(0, 4) == "gdnm")
                //                //        {
                //                //            collCampaign.Add(inImport);
                //                //            noOfMaccMillion--;
                //                //        }
                //                //        if (noOfBlackMaccMillion > 0 && inImport.RefNo.Substring(0, 4) == "gdbm")
                //                //        {
                //                //            collCampaign.Add(inImport);
                //                //            noOfBlackMaccMillion--;
                //                //        }

                //                //        if (noOfMaccMillion == 0 && noOfBlackMaccMillion == 0) break;
                //                //    }

                //                //    inImportCollection = collCampaign;
                //                //}
                //                #endregion

                //                #region New Allocation Groupings

                //                //else
                //                {
                //                    INImportCollection leadsToAssign = new INImportCollection();
                //                    INImportCollection leadsAvailable = new INImportCollection();
                //                    INImportCollection leadsRePrime = new INImportCollection();
                //                    INImportCollection leadsHouseWives = new INImportCollection();
                //                    INImportCollection leadsPensioners = new INImportCollection();
                //                    INImportCollection leadsIndianSurames = new INImportCollection();

                //                    int noOfOther;
                //                    int noOfRePrimeLeads = 0;
                //                    int noOfHousewifeLeads = 0;
                //                    int noOfPensionerLeads = 0;
                //                    int noOfIndianSurnameLeads = 0;

                //                    //Initialize
                //                    {
                //                        foreach (INImport lead in inImportCollection)
                //                        {
                //                            leadsAvailable.Add(lead);
                //                        }
                //                    }

                //                    //Specific allocations
                //                    //if (inCampaign.Name.Contains("Cancer") && inCampaign.Name.Contains("Base"))
                //                    //{
                //                    //    noOfRePrimeLeads = ProcessLeads(leadsAvailable, (int)assign, "Re-Prime", out leadsRePrime, out leadsAvailable);
                //                    //}
                //                    //Common allocations
                //                    //{
                //                    //    noOfIndianSurnameLeads = ProcessLeads(leadsAvailable, (int)assign, "IndianSurnames", out leadsIndianSurames, out leadsAvailable);

                //                    //    noOfHousewifeLeads = ProcessLeads(leadsAvailable, (int)assign, "Housewife", out leadsHouseWives, out leadsAvailable);

                //                    //    noOfPensionerLeads = ProcessLeads(leadsAvailable, (int)assign, "Pensioner", out leadsPensioners, out leadsAvailable);
                //                    //}

                //                    //Allocate leads from different collections
                //                    {
                //                        noOfOther = Convert.ToInt32(assign - noOfRePrimeLeads - noOfHousewifeLeads - noOfPensionerLeads - noOfIndianSurnameLeads);
                //                        if (noOfOther < 0) noOfOther = 0;
                //                        if (noOfOther > leadsAvailable.Count)
                //                        {//this happens because of rounding issues
                //                            int diff = noOfOther - leadsAvailable.Count;
                //                            noOfOther = leadsAvailable.Count;

                //                            while (true)
                //                            {
                //                                if (leadsRePrime.Count > noOfRePrimeLeads)
                //                                {
                //                                    noOfRePrimeLeads++;
                //                                    diff--;
                //                                    if (diff == 0) break;
                //                                }

                //                                if (leadsHouseWives.Count > noOfHousewifeLeads)
                //                                {
                //                                    noOfHousewifeLeads++;
                //                                    diff--;
                //                                    if (diff == 0) break;
                //                                }

                //                                if (leadsPensioners.Count > noOfPensionerLeads)
                //                                {
                //                                    noOfPensionerLeads++;
                //                                    diff--;
                //                                    if (diff == 0) break;
                //                                }

                //                                if (leadsIndianSurames.Count > noOfIndianSurnameLeads)
                //                                {
                //                                    noOfIndianSurnameLeads++;
                //                                    diff--;
                //                                    if (diff == 0) break;
                //                                }
                //                            }
                                            
                //                        }

                //                        long count = 0;
                //                        while (count < assign)
                //                        {
                //                            if (noOfRePrimeLeads > 0 && count < assign)
                //                            {
                //                                leadsToAssign.Add(leadsRePrime[noOfRePrimeLeads - 1]);
                //                                leadsRePrime.RemoveAt(noOfRePrimeLeads - 1);
                //                                noOfRePrimeLeads--;
                //                                count++;
                //                            }

                //                            if (noOfHousewifeLeads > 0 && count < assign)
                //                            {
                //                                leadsToAssign.Add(leadsHouseWives[noOfHousewifeLeads - 1]);
                //                                leadsHouseWives.RemoveAt(noOfHousewifeLeads - 1);
                //                                noOfHousewifeLeads--;
                //                                count++;
                //                            }

                //                            if (noOfPensionerLeads > 0 && count < assign)
                //                            {
                //                                leadsToAssign.Add(leadsPensioners[noOfPensionerLeads - 1]);
                //                                leadsPensioners.RemoveAt(noOfPensionerLeads - 1);
                //                                noOfPensionerLeads--;
                //                                count++;
                //                            }

                //                            if (noOfIndianSurnameLeads > 0 && count < assign)
                //                            {
                //                                leadsToAssign.Add(leadsIndianSurames[noOfIndianSurnameLeads - 1]);
                //                                leadsIndianSurames.RemoveAt(noOfIndianSurnameLeads - 1);
                //                                noOfIndianSurnameLeads--;
                //                                count++;
                //                            }

                //                            if (noOfOther > 0 && count < assign)
                //                            {
                //                                leadsToAssign.Add(leadsAvailable[noOfOther - 1]);
                //                                leadsAvailable.RemoveAt(noOfOther - 1);
                //                                noOfOther--;
                //                                count++;
                //                            }
                //                        }

                //                        inImportCollection = leadsToAssign;
                //                    }

                //                }

                //                #endregion

                //            }

                //            #endregion allocation for specific campaigns or Occupation

                //            Database.BeginTransaction(null, IsolationLevel.Snapshot);

                //            int[] i = { 0 };
                //            //RandomizeLeads(inImportCollection);
                //            foreach (INImport inImport in inImportCollection)
                //            {
                //                i[0]++; 

                //                inImport.FKUserID = userID;
                //                //inImport.AllocationDate = DateTime.Now;
                //                //inImport.AllocationDate = DetermineAllocationDate(_useDifferentAllocationDate, _newAllocationDate);
                //                //inImport.BonusLead = chkBonusLeads.IsChecked;
                //                inImport.Save(_validationResult);
                                
                //                record.Cells["Assign"].Value = Convert.ToInt64(record.Cells["Assign"].Value.ToString()) - 1;
                //                record.Cells["LeadsAllocated"].Value = Convert.ToInt64(record.Cells["LeadsAllocated"].Value.ToString()) + 1;
                //                //tbTotalAssigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                //                //{
                //                //    tbTotalAssigned.Text = (Convert.ToInt64(tbTotalAssigned.Text) + 1).ToString();
                //                //}));
                //                //tbTotalUnassigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                //                //{
                //                //    tbTotalUnassigned.Text = (Convert.ToInt64(tbTotalUnassigned.Text) - 1).ToString();
                //                //}));

                //                if (i[0] == assign || Math.Abs(i[0]) == inImportCollection.Count)
                //                {
                //                    record.Cells["Assign"].Value = 0;
                //                    break;
                //                }
                //            }

                //            CommitTransaction(null);
                //        }
                //        else
                //        {
                //            record.Cells["Assign"].Value = 0;
                //        }
                //    }
                //    else if (assign < 0)
                //    {
                //        INImportCollection inImportCollection = INImportMapper.Search(userID, _batchID, false, null);

                //        if (inImportCollection.Count > 0)
                //        {
                //            Database.BeginTransaction(null, IsolationLevel.Snapshot);
                //            int[] i = {0};
                //            foreach (INImport inImport in inImportCollection)
                //            {
                //                i[0]--;
                //                inImport.FKUserID = null;
                //                inImport.AllocationDate = null;
                //                inImport.Save(_validationResult);

                //                record.Cells["Assign"].Value = Convert.ToInt64(record.Cells["Assign"].Value.ToString()) + 1;
                //                record.Cells["LeadsAllocated"].Value = Convert.ToInt64(record.Cells["LeadsAllocated"].Value.ToString()) - 1;
                //                //tbTotalAssigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                //                //{
                //                //    tbTotalAssigned.Text = (Convert.ToInt64(tbTotalAssigned.Text) - 1).ToString();
                //                //}));
                //                //tbTotalUnassigned.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                //                //{
                //                //    tbTotalUnassigned.Text = (Convert.ToInt64(tbTotalUnassigned.Text) + 1).ToString();
                //                //}));

                //                if (i[0] == assign || Math.Abs(i[0]) == inImportCollection.Count)
                //                {
                //                    record.Cells["Assign"].Value = 0;
                //                    break;
                //                }
                //            }
                //            CommitTransaction(null);
                //        }
                //        else
                //        {
                //            record.Cells["Assign"].Value = 0;
                //        }
                //    }
                //}
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

        //private DateTime DetermineAllocationDate(bool useDifferentAllocationDate, DateTime selectedDateTime)
        //{
        //    DateTime dtResult;

        //    if (!useDifferentAllocationDate)
        //    {
        //        dtResult = DateTime.Now;
        //    }
        //    else
        //    {
        //        dtResult = new DateTime(selectedDateTime.Year, selectedDateTime.Month, selectedDateTime.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
        //    }

        //    return dtResult;
        //}

        #endregion Private Methods

        #region Event Handlers

        private void btnAssign_Click(object sender, RoutedEventArgs e)
        {
            btnAssign.IsEnabled = false;
            btnClose.IsEnabled = false;
            xdgAssignLeads.IsEnabled = false;
            xdgAssignLeads.ActiveRecord = null;
            
            AssignLeads();

            btnAssign.IsEnabled = false;
            btnClose.IsEnabled = true;
            xdgAssignLeads.IsEnabled = false;

            INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
            ShowMessageBox(messageWindow, "Leads successfully assigned.", "", ShowMessageType.Information);
        }

        private void btnAgents_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataSet ds = null;
                xdgAssignLeads.DataSource = null;

                switch (xdgAssignLeads.Tag.ToString())
                {
                    case "Active, Employed":
                        ds = Insure.INGetBatchAssignedLeadsData(_batchID, 1, 0);
                        xdgAssignLeads.Tag = "Active, All";
                        goto case "Default";

                    case "Active, All":
                        ds = Insure.INGetBatchAssignedLeadsData(_batchID, 1, 1);
                        xdgAssignLeads.Tag = "Active, Employed";
                        goto case "Default";

                    //case "All":
                    //    ds = Insure.INGetBatchAssignedLeadsData(_batchID, 1, 1);
                    //    xdgAssignLeads.Tag = "Active, Employed";
                    //    goto case "Default";

                    case "Default":
                        if (ds != null)
                        {
                            _dtBatch = ds.Tables[0];
                            _dtAgents = ds.Tables[1];
                            xdgAssignLeads.DataSource = _dtAgents.DefaultView;
                            //tbAgents.Text = string.Format("{0} ({1})", xdgAssignLeads.Tag, _dtAgents.Rows.Count);
                        }
                        else
                        {
                            //tbAgents.Text = "Error!";
                        }
                        break;
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
            OnDialogClose(false);
        }

        private void xdgAssignLeads_Loaded(object sender, RoutedEventArgs e)
        {
            xdgAssignLeads.Focus();
        }

        //private void chkUseDifferentAllocationDate_Checked(object sender, RoutedEventArgs e)
        //{
        //    lblNewAllocationDate.Visibility = Visibility.Visible;
        //    dteNewAllocationDate.Visibility = Visibility.Visible;
        //}

        //private void chkUseDifferentAllocationDate_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    lblNewAllocationDate.Visibility = Visibility.Hidden;
        //    dteNewAllocationDate.Visibility = Visibility.Hidden;

        //    dteNewAllocationDate.Value = null;
        //    _newAllocationDate = DateTime.Now;
        //}

        //private void dteNewAllocationDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    DateTime selectedDateTime;

        //    if (dteNewAllocationDate.Value == null)
        //    {
        //        _newAllocationDate = DateTime.Now;
        //    }
        //    else
        //    {
        //        selectedDateTime = Convert.ToDateTime(dteNewAllocationDate.Value);
        //        _newAllocationDate = new DateTime(selectedDateTime.Year, selectedDateTime.Month, selectedDateTime.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
        //    }
        //}

        private void xdgAssignLeads_RecordActivated(object sender, Infragistics.Windows.DataPresenter.Events.RecordActivatedEventArgs e)
        {
            if (e.Record != null && Convert.ToInt32(_dtLeadBook.Rows[0]["StillToContact"].ToString()) > 0)
            {
                btnAssign.IsEnabled = ((DataRecord) e.Record).Cells["Rank"].Value == null ||
                                      ((DataRecord) e.Record).Cells["Rank"].Value == DBNull.Value;
            }
        }

        #endregion Event Handlers
        
    }
}

