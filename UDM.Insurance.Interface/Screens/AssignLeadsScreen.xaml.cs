using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Embriant.Framework;
using Embriant.Framework.Data;
using Infragistics.Windows.DataPresenter;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using System.Diagnostics;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;


namespace UDM.Insurance.Interface.Screens
{
	public partial class AssignLeadsScreen
    {
        #region Constant

        #endregion Constant

        #region Properties

        public bool IsGoldenLeads { get; set; }

        #endregion

        #region Private Members

        private readonly long _batchID;
        private DataTable _dtBatch;
        private DataTable _dtAgents;
        private DateTime _newAllocationDate;
        private bool _useDifferentAllocationDate = false;

        private string _searchStr = string.Empty;
        private Stopwatch _typeStopwatch = new Stopwatch();

        #endregion Private Members

        #region Constructors

        public AssignLeadsScreen(long batchID)
        {
            InitializeComponent();

            _batchID = batchID;

            LoadLookupData();
        }

        #endregion Constructors

        #region Private Methods

        private void LoadLookupData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                //DataSet ds = Insure.INGetBatchAssignedLeadsData(_batchID, 1, 1);
                SqlParameter[] parameters = new SqlParameter[2];
				parameters[0] = new SqlParameter("@BatchID", _batchID);
                parameters[1] = new SqlParameter("@Mode", 1);
                DataSet ds = Methods.ExecuteStoredProcedure2("spINGetBatchAssignedLeadsData2", parameters, IsolationLevel.Snapshot, 10);
                _dtBatch = ds.Tables[0];
                _dtAgents = ds.Tables[1];

                tbCampaign.Text = _dtBatch.Rows[0]["CampaignName"].ToString();
                tbBatch.Text = _dtBatch.Rows[0]["BatchCode"].ToString();
                tbUDMBatch.Text = _dtBatch.Rows[0]["UDMBatchCode"].ToString();
                tbTotalAssigned.Text = _dtBatch.Rows[0]["Assigned"].ToString();
                tbTotalUnassigned.Text = _dtBatch.Rows[0]["Unassigned"].ToString();
                tbTotalPrinted.Text = _dtBatch.Rows[0]["Printed"].ToString();

                xdgAssignLeads.Tag = "Permanent";
                tbAgents.Text = string.Format("{0} ({1})", xdgAssignLeads.Tag, _dtAgents.Rows.Count);
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

        private void GetLeads(INImportCollection inLeads, string str, out INImportCollection foundLeads, out INImportCollection outLeads)
        {
            StringBuilder strQuery;
            int[] arrCount = { 0, inLeads.Count, 0, 0 };
            foundLeads = new INImportCollection();
            outLeads = new INImportCollection();
            string strSurname;

            switch (str)
            {
                case "LowerPremiumUpgrade":

                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                    {
                        lblAllocateMessage1.Text = "Processing Lower Premium Leads";
                        lblAllocateMessage2.Text = "Lower Premium (0)";
                        lblAllocateMessage3.Text = "";
                        bAllocateMessage.Visibility = Visibility.Visible;
                    }));

                    foreach (INImport inImport in inLeads)
                    {
                        arrCount[0]++;
                        strQuery = new StringBuilder();
                        strQuery.AppendFormat($"SELECT COUNT(INImportedPolicyData.ID) [Count] FROM INImportedPolicyData LEFT JOIN INImport ON INImport.FKINImportedPolicyDataID = INImportedPolicyData.ID WHERE INImport.ID = '{inImport.ID}' AND (ContractPremium < '300.00')");

                        if (Convert.ToInt32(Methods.GetTableData(strQuery.ToString()).Rows[0]["Count"]) == 1)
                        {
                            arrCount[2]++;
                            foundLeads.Add(inImport);
                            continue;
                        }

                        outLeads.Add(inImport);

                        Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                        {
                            lblAllocateMessage2.Text = $"Lower Premium ({arrCount[2]})";
                            lblAllocateMessage3.Text = $"{arrCount[0]} / {arrCount[1]}";
                        }));
                    }

                    break;

                case "Re-Marketed":

                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                    {
                        lblAllocateMessage1.Text = "Processing Available Leads";
                        lblAllocateMessage2.Text = "Re-Marketed (0)";
                        lblAllocateMessage3.Text = "";
                        bAllocateMessage.Visibility = Visibility.Visible;
                    }));

                    foreach (INImport inImport in inLeads)
                    {
                        arrCount[0]++;
                        strQuery = new StringBuilder();
                        strQuery.AppendFormat($"SELECT COUNT(INImportOther.ID) [Count] FROM INImportOther WHERE FKINImportID = '{inImport.ID}' AND (TimesRemarketed IS NOT NULL)");

                        if (Convert.ToInt32(Methods.GetTableData(strQuery.ToString()).Rows[0]["Count"]) == 1)
                        {
                            arrCount[2]++;
                            foundLeads.Add(inImport);
                            continue;
                        }

                        outLeads.Add(inImport);

                        Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                        {
                            lblAllocateMessage2.Text = $"Re-Marketed ({arrCount[2]})";
                            lblAllocateMessage3.Text = $"{arrCount[0]} / {arrCount[1]}";
                        }));
                    }

                    break;

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

                    foreach (INImport inImport in inLeads)
                    {
                        try
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
                        catch
                        {

                        }

                    }

                    break;

                case "AfricanSurnames":

                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                    {
                        lblAllocateMessage1.Text = "Processing Available Leads";
                        lblAllocateMessage2.Text = "African Surname (0)";
                        lblAllocateMessage3.Text = "";
                        bAllocateMessage.Visibility = Visibility.Visible;
                    }));

                    DataTable dtAfricanSurnames = Methods.GetTableData("SELECT Surname FROM Surnames WHERE FKSurnameType = '2'");

                    foreach (INImport inImport in inLeads)
                    {
                        arrCount[0]++;
                        strQuery = new StringBuilder();
                        strQuery.AppendFormat("SELECT Surname FROM INLead WHERE ID = '{0}'", inImport.FKINLeadID);

                        strSurname = Methods.GetTableData(strQuery.ToString()).Rows[0]["Surname"] as string;
                        DataRow[] dr = dtAfricanSurnames.Select("Surname = '" + strSurname + "'");

                        if (dr.Length != 0)
                        {
                            arrCount[3]++;
                            foundLeads.Add(inImport);
                            continue;
                        }

                        outLeads.Add(inImport);

                        Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                        {
                            lblAllocateMessage2.Text = $"African Surname ({arrCount[3]})";
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

        public void AssignLeads()
        {
            try
            {
                foreach (Record rec in xdgAssignLeads.Records)
                {
                    var record = (DataRecord) rec;

                    long userID = long.Parse(record.Cells["UserID"].Value.ToString());
                    long assign = long.Parse(record.Cells["Assign"].Value.ToString());

                    INBatch inBatch = new INBatch(_batchID);
                    INCampaign inCampaign = new INCampaign(Convert.ToInt32(inBatch.FKINCampaignID));

                    if (assign > 0)
                    {
                        //INImportCollection inImportCollection = INImportMapper.Search(null, _batchID, null);
                        INImportCollection inImportCollection = INImportMapper.Search(null, null, _batchID, null);

                        if (assign > inImportCollection.Count)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                            {
                                INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                                ShowMessageBox(messageWindow, inImportCollection.Count + " leads available.", "Not Enough Leads Available", ShowMessageType.Error);
                            }));

                            return;
                        }

                        if (inImportCollection.Count > 0)
                        {

                            #if !TRAININGBUILD

                            RandomizeLeads(inImportCollection);

                            #endif

                            #region allocation for specific campaigns or Occupation
                            #if !TRAININGBUILD

                            { 

                                #region ACCDIS
                                //if (inCampaign.Code == "ACCDIS")
                                //{
                                //    INImportCollection collCampaign = new INImportCollection();
                                    
                                //    int countMacc = Methods.GetTableData(string.Format("SELECT RefNo From INImport WHERE FKINBatchID = '{0}' AND FKUserID IS NULL AND RefNo Like 'gdna%'", _batchID)).Rows.Count;
                                //    int countMaccMillion = Methods.GetTableData(string.Format("SELECT RefNo From INImport WHERE FKINBatchID = '{0}' AND FKUserID IS NULL AND RefNo Like 'gdnm%'", _batchID)).Rows.Count;
                                //    int count = countMacc + countMaccMillion;

                                //    decimal maccPerc = Math.Round((decimal)countMacc / count * 100);

                                //    int noOfMacc = (int)Math.Round(assign * maccPerc / 100);
                                //    if (maccPerc > 0.000m)
                                //    {
                                //        if (noOfMacc == 0) noOfMacc = 1;
                                //    }
                                //    int noOfMaccMillion = (int)(assign - noOfMacc);

                                //    foreach (INImport inImport in inImportCollection)
                                //    {
                                //        if (noOfMacc > 0 && inImport.RefNo.Substring(0, 4) == "gdna")
                                //        {
                                //            collCampaign.Add(inImport);
                                //            noOfMacc--;
                                //        }
                                //        if (noOfMaccMillion > 0 && inImport.RefNo.Substring(0, 4) == "gdnm")
                                //        {
                                //            collCampaign.Add(inImport);
                                //            noOfMaccMillion--;
                                //        }

                                //        if (noOfMacc == 0 && noOfMaccMillion == 0) break;
                                //    }

                                //    inImportCollection = collCampaign;
                                //}
                                #endregion

                                #region PLMMBE
                                //else if (inCampaign.Code == "PLMMBE")
                                //{
                                //    INImportCollection collCampaign = new INImportCollection();

                                //    int countMaccMillion =
                                //        Methods.GetTableData($"SELECT RefNo From INImport WHERE FKINBatchID = '{_batchID}' AND FKUserID IS NULL AND RefNo Like 'gdnm%'")
                                //            .Rows.Count;
                                //    int countBlackMaccMillion =
                                //        Methods.GetTableData($"SELECT RefNo From INImport WHERE FKINBatchID = '{_batchID}' AND FKUserID IS NULL AND RefNo Like 'gdbm%'")
                                //            .Rows.Count;
                                //    int countTotal = countMaccMillion + countBlackMaccMillion;

                                //    decimal percMaccMillion = Math.Round((decimal) countMaccMillion/countTotal*100);

                                //    int noOfMaccMillion = (int) Math.Round(assign*percMaccMillion/100);
                                //    if (percMaccMillion > 0.000m)
                                //    {
                                //        if (noOfMaccMillion == 0) noOfMaccMillion = 1;
                                //    }
                                //    int noOfBlackMaccMillion = (int) (assign - noOfMaccMillion);

                                //    foreach (INImport inImport in inImportCollection)
                                //    {
                                //        if (noOfMaccMillion > 0 && inImport.RefNo.Substring(0, 4) == "gdnm")
                                //        {
                                //            collCampaign.Add(inImport);
                                //            noOfMaccMillion--;
                                //        }
                                //        if (noOfBlackMaccMillion > 0 && inImport.RefNo.Substring(0, 4) == "gdbm")
                                //        {
                                //            collCampaign.Add(inImport);
                                //            noOfBlackMaccMillion--;
                                //        }

                                //        if (noOfMaccMillion == 0 && noOfBlackMaccMillion == 0) break;
                                //    }

                                //    inImportCollection = collCampaign;
                                //}
                                #endregion

                                #region New Allocation Groupings

                                //else
                                {
                                    INImportCollection leadsToAssign = new INImportCollection();
                                    INImportCollection leadsAvailable = new INImportCollection();
                                    INImportCollection leadsRePrime = new INImportCollection();
                                    INImportCollection leadsHouseWives = new INImportCollection();
                                    INImportCollection leadsPensioners = new INImportCollection();
                                    INImportCollection leadsIndianSurames = new INImportCollection();
                                    INImportCollection leadsAfricanSurames = new INImportCollection();
                                    INImportCollection leadsReMarketed = new INImportCollection();
                                    INImportCollection leadsLowerPremiumUpgrade = new INImportCollection();

                                    int noOfOther;
                                    int noOfRePrimeLeads = 0;
                                    int noOfHousewifeLeads = 0;
                                    int noOfPensionerLeads = 0;
                                    int noOfIndianSurnameLeads = 0;
                                    int noOfAfricanSurnameLeads = 0;
                                    int noOfReMarketedLeads = 0;
                                    int noOfLowerPremiumUpgradeLeads = 0;

                                    //Initialize
                                    {
                                        foreach (INImport lead in inImportCollection)
                                        {
                                            leadsAvailable.Add(lead);
                                        }
                                    }

                                    //Allocate Re-Marketed first
                                    //noOfReMarketedLeads = ProcessLeads(leadsAvailable, (int)assign, "Re-Marketed", out leadsReMarketed, out leadsAvailable);

                                    //Specific allocations
                                    if (inCampaign.Name.Contains("Cancer") && inCampaign.Name.Contains("Base"))
                                    {
                                        noOfRePrimeLeads = ProcessLeads(leadsAvailable, (int)assign, "Re-Prime", out leadsRePrime, out leadsAvailable);

                                        noOfReMarketedLeads = ProcessLeads(leadsAvailable, (int)assign, "Re-Marketed", out leadsReMarketed, out leadsAvailable);

                                        noOfHousewifeLeads = ProcessLeads(leadsAvailable, (int)assign, "Housewife", out leadsHouseWives, out leadsAvailable);

                                        noOfPensionerLeads = ProcessLeads(leadsAvailable, (int)assign, "Pensioner", out leadsPensioners, out leadsAvailable);
                                    }
                                    //Macc NG Campaigns
                                    else if ((inCampaign.ID == 2 || inCampaign.ID == 17) && inBatch.Code.Contains("NG"))
                                    {
                                        noOfAfricanSurnameLeads = ProcessLeads(leadsAvailable, (int)assign, "AfricanSurnames", out leadsAfricanSurames, out leadsAvailable);

                                        noOfReMarketedLeads = ProcessLeads(leadsAvailable, (int)assign, "Re-Marketed", out leadsReMarketed, out leadsAvailable);

                                        noOfHousewifeLeads = ProcessLeads(leadsAvailable, (int)assign, "Housewife", out leadsHouseWives, out leadsAvailable);

                                        noOfPensionerLeads = ProcessLeads(leadsAvailable, (int)assign, "Pensioner", out leadsPensioners, out leadsAvailable);
                                    }
                                    //All Cancer not Base
                                    else if (inCampaign.Name.Contains("Cancer"))
                                    {
                                        if (
                                                inCampaign.Name.Contains("Upgrade") &&
                                                (
                                                    inCampaign.Name.Contains("Upgrade 4") ||
                                                    inCampaign.Name.Contains("Upgrade 5") ||
                                                    inCampaign.Name.Contains("Upgrade 6") ||
                                                    inCampaign.Name.Contains("Upgrade 7") ||
                                                    inCampaign.Name.Contains("Upgrade 8") ||
                                                    inCampaign.Name.Contains("Upgrade 9") ||
                                                    inCampaign.Name.Contains("Upgrade 10") ||
                                                    inCampaign.Name.Contains("Upgrade 11")
                                                )
                                           )
                                        {
                                            noOfLowerPremiumUpgradeLeads = ProcessLeads(leadsAvailable, (int)assign, "LowerPremiumUpgrade", out leadsLowerPremiumUpgrade, out leadsAvailable);
                                        }

                                        noOfReMarketedLeads = ProcessLeads(leadsAvailable, (int)assign, "Re-Marketed", out leadsReMarketed, out leadsAvailable);

                                        noOfHousewifeLeads = ProcessLeads(leadsAvailable, (int)assign, "Housewife", out leadsHouseWives, out leadsAvailable);

                                        noOfPensionerLeads = ProcessLeads(leadsAvailable, (int)assign, "Pensioner", out leadsPensioners, out leadsAvailable);
                                    }

                                    //Common allocations not the ones Specified Below in the if clause
                                    if(
                                        !((inCampaign.ID == 2 || inCampaign.ID == 17) && inBatch.Code.Contains("NG")) 
                                        && !(inCampaign.Name.Contains("Cancer"))
                                      )
                                    {
                                        if (
                                                inCampaign.Name.Contains("Upgrade") &&
                                                (
                                                    inCampaign.Name.Contains("Upgrade 4") ||
                                                    inCampaign.Name.Contains("Upgrade 5") ||
                                                    inCampaign.Name.Contains("Upgrade 6") ||
                                                    inCampaign.Name.Contains("Upgrade 7") ||
                                                    inCampaign.Name.Contains("Upgrade 8") ||
                                                    inCampaign.Name.Contains("Upgrade 9") ||
                                                    inCampaign.Name.Contains("Upgrade 10") ||
                                                    inCampaign.Name.Contains("Upgrade 11")
                                                )
                                           )
                                        {
                                            noOfLowerPremiumUpgradeLeads = ProcessLeads(leadsAvailable, (int)assign, "LowerPremiumUpgrade", out leadsLowerPremiumUpgrade, out leadsAvailable);
                                        }

                                        noOfIndianSurnameLeads = ProcessLeads(leadsAvailable, (int)assign, "IndianSurnames", out leadsIndianSurames, out leadsAvailable);

                                        noOfReMarketedLeads = ProcessLeads(leadsAvailable, (int)assign, "Re-Marketed", out leadsReMarketed, out leadsAvailable);

                                        noOfHousewifeLeads = ProcessLeads(leadsAvailable, (int)assign, "Housewife", out leadsHouseWives, out leadsAvailable);

                                        noOfPensionerLeads = ProcessLeads(leadsAvailable, (int)assign, "Pensioner", out leadsPensioners, out leadsAvailable);
                                    }

                                    //Allocate leads from different collections
                                    {
                                        noOfOther = Convert.ToInt32(assign - noOfRePrimeLeads - noOfHousewifeLeads - noOfPensionerLeads - noOfIndianSurnameLeads - noOfAfricanSurnameLeads - noOfReMarketedLeads - noOfLowerPremiumUpgradeLeads);
                                        if (noOfOther < 0) noOfOther = 0;
                                        if (noOfOther > leadsAvailable.Count)
                                        {//this happens because of rounding issues
                                            int diff = noOfOther - leadsAvailable.Count;
                                            noOfOther = leadsAvailable.Count;

                                            while (true)
                                            {
                                                if (leadsRePrime.Count > noOfRePrimeLeads)
                                                {
                                                    noOfRePrimeLeads++;
                                                    diff--;
                                                    if (diff == 0) break;
                                                }

                                                if (leadsHouseWives.Count > noOfHousewifeLeads)
                                                {
                                                    noOfHousewifeLeads++;
                                                    diff--;
                                                    if (diff == 0) break;
                                                }

                                                if (leadsPensioners.Count > noOfPensionerLeads)
                                                {
                                                    noOfPensionerLeads++;
                                                    diff--;
                                                    if (diff == 0) break;
                                                }

                                                if (leadsIndianSurames.Count > noOfIndianSurnameLeads)
                                                {
                                                    noOfIndianSurnameLeads++;
                                                    diff--;
                                                    if (diff == 0) break;
                                                }

                                                if (leadsAfricanSurames.Count > noOfAfricanSurnameLeads)
                                                {
                                                    noOfAfricanSurnameLeads++;
                                                    diff--;
                                                    if (diff == 0) break;
                                                }

                                                if (leadsReMarketed.Count > noOfReMarketedLeads)
                                                {
                                                    noOfReMarketedLeads++;
                                                    diff--;
                                                    if (diff == 0) break;
                                                }

                                                if (leadsLowerPremiumUpgrade.Count > noOfLowerPremiumUpgradeLeads)
                                                {
                                                    noOfLowerPremiumUpgradeLeads++;
                                                    diff--;
                                                    if (diff == 0) break;
                                                }
                                            }
                                            
                                        }

                                        long count = 0;
                                        while (count < assign)
                                        {

                                            if (noOfRePrimeLeads > 0 && count < assign)
                                            {
                                                leadsToAssign.Add(leadsRePrime[noOfRePrimeLeads - 1]);
                                                leadsRePrime.RemoveAt(noOfRePrimeLeads - 1);
                                                noOfRePrimeLeads--;
                                                count++;
                                            }

                                            if (noOfHousewifeLeads > 0 && count < assign)
                                            {
                                                leadsToAssign.Add(leadsHouseWives[noOfHousewifeLeads - 1]);
                                                leadsHouseWives.RemoveAt(noOfHousewifeLeads - 1);
                                                noOfHousewifeLeads--;
                                                count++;
                                            }

                                            if (noOfPensionerLeads > 0 && count < assign)
                                            {
                                                leadsToAssign.Add(leadsPensioners[noOfPensionerLeads - 1]);
                                                leadsPensioners.RemoveAt(noOfPensionerLeads - 1);
                                                noOfPensionerLeads--;
                                                count++;
                                            }

                                            if (noOfIndianSurnameLeads > 0 && count < assign)
                                            {
                                                leadsToAssign.Add(leadsIndianSurames[noOfIndianSurnameLeads - 1]);
                                                leadsIndianSurames.RemoveAt(noOfIndianSurnameLeads - 1);
                                                noOfIndianSurnameLeads--;
                                                count++;
                                            }

                                            if (noOfAfricanSurnameLeads > 0 && count < assign)
                                            {
                                                leadsToAssign.Add(leadsAfricanSurames[noOfAfricanSurnameLeads - 1]);
                                                leadsAfricanSurames.RemoveAt(noOfAfricanSurnameLeads - 1);
                                                noOfAfricanSurnameLeads--;
                                                count++;
                                            }

                                            if (noOfReMarketedLeads > 0 && count < assign)
                                            {
                                                leadsToAssign.Add(leadsReMarketed[noOfReMarketedLeads - 1]);
                                                leadsReMarketed.RemoveAt(noOfReMarketedLeads - 1);
                                                noOfReMarketedLeads--;
                                                count++;
                                            }

                                            if (noOfLowerPremiumUpgradeLeads > 0 && count < assign)
                                            {
                                                leadsToAssign.Add(leadsLowerPremiumUpgrade[noOfLowerPremiumUpgradeLeads - 1]);
                                                leadsLowerPremiumUpgrade.RemoveAt(noOfLowerPremiumUpgradeLeads - 1);
                                                noOfLowerPremiumUpgradeLeads--;
                                                count++;
                                            }

                                            if (noOfOther > 0 && count < assign)
                                            {
                                                leadsToAssign.Add(leadsAvailable[noOfOther - 1]);
                                                leadsAvailable.RemoveAt(noOfOther - 1);
                                                noOfOther--;
                                                count++;
                                            }
                                        }

                                        inImportCollection = leadsToAssign;
                                    }

                                }

                                #endregion

                            }

                            #endif
                            #endregion allocation for specific campaigns or Occupation

                            Database.BeginTransaction(null, IsolationLevel.Snapshot);

                            int[] i = { 0 };

                            #if !TRAININGBUILD

                            RandomizeLeads(inImportCollection);

                            #endif

                            foreach (INImport inImport in inImportCollection)
                            {
                                i[0]++; 

                                inImport.FKUserID = userID;
                                //inImport.AllocationDate = DateTime.Now;
                                inImport.AllocationDate = DetermineAllocationDate(_useDifferentAllocationDate, _newAllocationDate);
                                inImport.IsFutureAllocation = _useDifferentAllocationDate;
                                inImport.BonusLead = chkBonusLeads.IsChecked;
                                inImport.Save(_validationResult);

                                INImportExtra iNImportExtra = INImportExtraMapper.SearchOne(inImport.ID, null);
                                if (iNImportExtra == null)
                                {
                                    iNImportExtra = new INImportExtra();
                                }
                                iNImportExtra.FKINImportID = inImport.ID;
                                iNImportExtra.IsGoldenLead = IsGoldenLeads ? true : false;
                                iNImportExtra.Save(_validationResult);

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

                                if (i[0] == assign || Math.Abs(i[0]) == inImportCollection.Count)
                                {
                                    record.Cells["Assign"].Value = 0;
                                    break;
                                }
                            }

                            CommitTransaction(null);
                        }
                        else
                        {
                            record.Cells["Assign"].Value = 0;
                        }
                    }
                    else if (assign < 0)
                    {
                        INImportCollection inImportCollection = INImportMapper.Search(userID, _batchID, false, null);

                        if (inImportCollection.Count > 0)
                        {
                            Database.BeginTransaction(null, IsolationLevel.Snapshot);
                            int[] i = {0};
                            foreach (INImport inImport in inImportCollection)
                            {
                                i[0]--;
                                inImport.FKUserID = null;
                                inImport.AllocationDate = null;
                                inImport.IsFutureAllocation = null;
                                inImport.Save(_validationResult);

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

                                if (i[0] == assign || Math.Abs(i[0]) == inImportCollection.Count)
                                {
                                    record.Cells["Assign"].Value = 0;
                                    break;
                                }
                            }
                            CommitTransaction(null);
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

        private bool CanLeadsBeAllocated()
        {
            if (chkUseDifferentAllocationDate.IsChecked.HasValue)
            {
                _useDifferentAllocationDate = chkUseDifferentAllocationDate.IsChecked.Value;
            }

            if (_useDifferentAllocationDate)
            {
                if (dteNewAllocationDate.Value == null)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Because you have indicated that you want to use a different allocation date, you need to specify the new allocation date.", "Allocation Date Not Specified", ShowMessageType.Error);
                    dteNewAllocationDate.Focus();
                    return false;
                }

                _newAllocationDate = Convert.ToDateTime(dteNewAllocationDate.Value);

                if (_newAllocationDate.Date < DateTime.Now.Date)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "The new allocation date is invalid, because it is in the past.", "Invalid Allocation Date", ShowMessageType.Error);
                    dteNewAllocationDate.Focus();
                    return false;
                }
            }

            return true;
        }

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

        #endregion Private Methods

        #region Event Handlers

        private void btnAssign_Click(object sender, RoutedEventArgs e)
        {
            btnAssign.IsEnabled = false;
            btnClose.IsEnabled = false;
            btnAgents.IsEnabled = false;
            xdgAssignLeads.IsEnabled = false;
            xdgAssignLeads.ActiveRecord = null;

            if (CanLeadsBeAllocated())
            {
                AssignLeads();
            }

            btnAssign.IsEnabled = true;
            btnClose.IsEnabled = true;
            btnAgents.IsEnabled = true;
            xdgAssignLeads.IsEnabled = true;
        }

        private void btnAgents_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataSet ds = null;
                xdgAssignLeads.DataSource = null;
                SqlParameter[] parameters = new SqlParameter[2];

                switch (xdgAssignLeads.Tag.ToString())
                {
                    case "Permanent":
                        //ds = Insure.INGetBatchAssignedLeadsData(_batchID, 1, 0);
                        parameters = new SqlParameter[2];
				        parameters[0] = new SqlParameter("@BatchID", _batchID);
                        parameters[1] = new SqlParameter("@Mode", 2);
                        ds = Methods.ExecuteStoredProcedure2("spINGetBatchAssignedLeadsData2", parameters, IsolationLevel.Snapshot, 10);
                        xdgAssignLeads.Tag = "Temporary";
                        goto case "Default";

                    case "Temporary":
                        //ds = Insure.INGetBatchAssignedLeadsData(_batchID, 1, 1);
                        parameters = new SqlParameter[2];
				        parameters[0] = new SqlParameter("@BatchID", _batchID);
                        parameters[1] = new SqlParameter("@Mode", 1);
                        ds = Methods.ExecuteStoredProcedure2("spINGetBatchAssignedLeadsData2", parameters, IsolationLevel.Snapshot, 10);
                        xdgAssignLeads.Tag = "Permanent";
                        goto case "Default";

                    case "Default":
                        if (ds != null)
                        {
                            _dtBatch = ds.Tables[0];
                            _dtAgents = ds.Tables[1];
                            xdgAssignLeads.DataSource = _dtAgents.DefaultView;
                            tbAgents.Text = string.Format("{0} ({1})", xdgAssignLeads.Tag, _dtAgents.Rows.Count);
                        }
                        else
                        {
                            tbAgents.Text = "Error!";
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

        #endregion Event Handlers

        private void DoSearch(XamDataGrid grid)
        {
            try
            {
                if (grid != null && grid.Records is DataRecordCollection)
                {
                    foreach (DataRecord record in grid.Records)
                    {
                        foreach (Cell cell in record.Cells)
                        {
                            if (cell.Field.Name == "SalesAgent")
                            {
                                string cellString = cell.Value?.ToString().ToLower();

                                if (cellString.StartsWith(_searchStr.ToLower()))
                                {
                                    grid.BringRecordIntoView(record);
                                    record.IsSelected = true;
                                    record.IsActive = true;
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                
            }
        }

        private void XdgAssignLeads_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                XamDataGrid grid = sender as XamDataGrid;

                if (grid?.IsKeyboardFocusWithin == true)
                {
                    int keyValue = (int)e.Key;
                    
                    if (keyValue >= 44 && keyValue <= 69) // letters
                    {
                        _searchStr = _typeStopwatch.ElapsedMilliseconds > 1000 ? e.Key.ToString() : _searchStr + e.Key.ToString();

                        _typeStopwatch.Stop();

                        if (!string.IsNullOrEmpty(_searchStr)) DoSearch(grid);

                        _typeStopwatch.Reset();
                        _typeStopwatch.Start();
                    }
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}

