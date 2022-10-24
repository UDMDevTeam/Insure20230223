using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Windows;
using Infragistics.Windows.Controls;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.Events;
using Infragistics.Windows.Editors;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using System.Data.SqlClient;
using System.Threading;
using UDM.Insurance.Interface.Data;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using System.Collections.Generic;
using System.Net;
using System.Collections.Specialized;
using System.Text;

//using Infragistics.Controls.Editors;

namespace UDM.Insurance.Interface.Screens
{

    public partial class SalesScreen : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        #region Constants
        public DispatcherTimer timer = new DispatcherTimer();
        public long? FKImportID = null;
        public long? ImportID = null;
        public string CallBackChecked = null;
        string CMToSReferenceNumber;
        private LeadApplicationData _laData = new LeadApplicationData();
        #endregion

        protected Embriant.Framework.Validation.ValidationResult _validationResult = new Embriant.Framework.Validation.ValidationResult();


        #region Private Members

        private readonly long _agentID = GlobalSettings.ApplicationUser.ID;
        private User _agent;

        private lkpUserType? _userType;
        public lkpUserType? UserType
        {
            get { return _userType; }
            set { _userType = value; OnPropertyChanged("UserType"); }
        }

        private int? _jobTitle;

        public int? JobTitle
        {
            get { return _jobTitle; }
            set { _jobTitle = value; OnPropertyChanged("JobTitle"); }
        }

        public LeadApplicationData LaData
        {
            get { return _laData; }
            set { _laData = value; }
        }

        private readonly SalesScreenGlobalData _ssGlobalData = new SalesScreenGlobalData();
        List<long> CMAgentListLong = new List<long>();

        string Username = ((User)GlobalSettings.ApplicationUser).LoginName;


        #endregion

        #region Constructors

        public SalesScreen()
        {

            Style styleAllocation = new Style(typeof(XamDateTimeEditor));
            styleAllocation.Setters.Add(new Setter(XamDateTimeEditor.MaskProperty, "yyyy-mm-dd hh:mm:ss"));
            Resources.Add(typeof(XamDateTimeEditor), styleAllocation);

            UserType = lkpUserType.SalesAgent;

            //JobTitle = 21;



            //CheckUserVersion(Username);

            InitializeComponent();

            _ssGlobalData.SalesScreen = this;

            LoadAgentDetails();

            LoadSalesData();

            LoadTimer();

            btnMySuccess.ToolTip = "My Success is Yours.";

            #region Makes a button visible just for Samantha Holder
            if (_agentID == 3388 || _agentID == 2857 || _agentID == 394)
            {
                btnCallMonQuery.Visibility = Visibility.Visible;
                btnCallMonTracking.Visibility = Visibility.Visible;
            }
            else
            {
                btnCallMonQuery.Visibility = Visibility.Collapsed;
                btnCallMonTracking.Visibility = Visibility.Collapsed;
            }
            #endregion

            #region Call Monitoring From Sales Pop up background thread

            DataTable dtAgentList = Methods.GetTableData("SELECT FKUserID FROM INCMAgentsOnline");
            List<DataRow> CMAgentList = dtAgentList.AsEnumerable().ToList();

            try { CMAgentListLong.Clear(); } catch { }


            foreach (var row in CMAgentList)
            {
                long userIDrow = long.Parse(row["FKUserID"].ToString());
                CMAgentListLong.Add(userIDrow);
            }

            if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += worker_DoWork;

                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync();

                SetAgentOnline();
            }


            #endregion
            if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
            {
                btnDCSpecialistDiaries.Visibility = Visibility.Visible;
            }
            else
            {
                btnDCSpecialistDiaries.Visibility = Visibility.Collapsed;
            }


#if TESTBUILD
            TestControl.Visibility = Visibility.Visible;
#elif DEBUG
            DebugControl.Visibility = Visibility.Visible;
#elif TRAININGBUILD
                TrainingControl.Visibility = Visibility.Visible;
#endif
        }

        #endregion


        #region Private Methods

        private void LoadSalesData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Restart The Counter
                try
                {
                    if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
                    {
                        //SetAgentOnline();
                        //timer.Start();
                    }
                }
                catch
                {

                }

                #endregion

                LoadSalesGrid();
                xdgSales.DataSource = null;

                if (UserType == lkpUserType.CallMonitoringAgent || UserType == lkpUserType.Preserver)
                {
                    DataSet ds = Insure.INGetSalesAssignedToCMAgent(_agentID);

                    DataRelation relCampaignBatch = new DataRelation("CampaignBatch", ds.Tables[0].Columns["CampaignGroupType"], ds.Tables[1].Columns["CampaignGroupType"]);
                    ds.Relations.Add(relCampaignBatch);

                    DataColumn[] parentColumns = new DataColumn[] { ds.Tables[1].Columns["DateOfSale"], ds.Tables[1].Columns["CampaignGroupType"] };
                    DataColumn[] childColumns = new DataColumn[] { ds.Tables[2].Columns["DateOfSale"], ds.Tables[2].Columns["CampaignGroupType"] };

                    DataRelation relBatchAgent = new DataRelation("BatchLead", parentColumns, childColumns);
                    ds.Relations.Add(relBatchAgent);

                    xdgSales.DataSource = ds.Tables[0].DefaultView;

                }
                else if (UserType == lkpUserType.ConfirmationAgent)
                {
                    DataSet ds = Insure.INGetPossibleBumpUpsAssignedToBUAgent(_agentID);

                    DataRelation relCampaignBatch = new DataRelation("CampaignBatch", ds.Tables[0].Columns["CampaignGroup"], ds.Tables[1].Columns["CampaignGroup"]);
                    ds.Relations.Add(relCampaignBatch);

                    DataColumn[] parentColumns = new DataColumn[] { ds.Tables[1].Columns["DateOfSale"], ds.Tables[1].Columns["CampaignGroup"] };
                    DataColumn[] childColumns = new DataColumn[] { ds.Tables[2].Columns["DateOfSale"], ds.Tables[2].Columns["CampaignGroup"] };

                    DataRelation relBatchAgent = new DataRelation("BatchLead", parentColumns, childColumns);
                    ds.Relations.Add(relBatchAgent);

                    xdgSales.DataSource = ds.Tables[0].DefaultView;
                    //foreach (Field field in xdgSales.FieldLayouts[2].Fields)
                    //{
                    //    string fieldName = field.Name;
                    //    if (fieldName == "CallBackDate")
                    //    {

                    //        xdgSales.FieldLayouts[2].Fields[fieldName].Settings.AllowEdit = true;
                    //    }
                    //    else
                    //    {
                    //        continue;
                    //    }
                    //}
                }
                else if (UserType == lkpUserType.DebiCheckAgent)
                {

                    DataSet ds = Insure.INGetSalesAssignedToDCAgent(_agentID);

                    DataRelation relCampaignBatch = new DataRelation("CampaignBatch", ds.Tables[0].Columns["CampaignGroupType"], ds.Tables[1].Columns["CampaignGroupType"]);
                    ds.Relations.Add(relCampaignBatch);

                    DataColumn[] parentColumns = new DataColumn[] { ds.Tables[1].Columns["DateOfSale"], ds.Tables[1].Columns["CampaignGroupType"] };
                    DataColumn[] childColumns = new DataColumn[] { ds.Tables[2].Columns["DateOfSale"], ds.Tables[2].Columns["CampaignGroupType"] };

                    DataRelation relBatchAgent = new DataRelation("BatchLead", parentColumns, childColumns);
                    ds.Relations.Add(relBatchAgent);

                    xdgSales.DataSource = ds.Tables[0].DefaultView;

                }
                else
                {
#if TRAININGBUILD
                        DataSet ds = Insure.INGetLeadsAssignedToUserTraining(_agentID);
#else
                    DataSet ds = Insure.INGetLeadsAssignedToUser(_agentID);
#endif

                    DataRelation relCampaignBatch = new DataRelation("CampaignBatch", ds.Tables[0].Columns["CampaignID"], ds.Tables[1].Columns["CampaignID"]);
                    ds.Relations.Add(relCampaignBatch);

                    DataRelation relBatchAgent = new DataRelation("BatchLead", ds.Tables[1].Columns["LeadBookID"], ds.Tables[2].Columns["LeadBookID"]);
                    ds.Relations.Add(relBatchAgent);

                    //ds.Tables[2].Columns.Add("CallBack", Type.GetType("System.Boolean"));

                    xdgSales.DataSource = ds.Tables[0].DefaultView;



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

        private void LoadSalesCallBack()
        {

            try
            {
                SetCursor(Cursors.Wait);

                #region Restart The Counter
                try
                {
                    if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
                    {
                        //SetAgentOnline();
                        //timer.Start();
                    }
                }
                catch
                {

                }

                #endregion

                LoadSalesGrid();
                xdgSales.DataSource = null;

                //if (UserType == lkpUserType.CallMonitoringAgent || UserType == lkpUserType.Preserver)
                //{
                //    DataSet ds = Insure.INGetSalesAssignedToCMAgent(_agentID);

                //    DataRelation relCampaignBatch = new DataRelation("CampaignBatch", ds.Tables[0].Columns["CampaignGroupType"], ds.Tables[1].Columns["CampaignGroupType"]);
                //    ds.Relations.Add(relCampaignBatch);

                //    DataColumn[] parentColumns = new DataColumn[] { ds.Tables[1].Columns["DateOfSale"], ds.Tables[1].Columns["CampaignGroupType"] };
                //    DataColumn[] childColumns = new DataColumn[] { ds.Tables[2].Columns["DateOfSale"], ds.Tables[2].Columns["CampaignGroupType"] };

                //    DataRelation relBatchAgent = new DataRelation("BatchLead", parentColumns, childColumns);
                //    ds.Relations.Add(relBatchAgent);

                //    xdgSales.DataSource = ds.Tables[0].DefaultView;

                //}
                //else if (UserType == lkpUserType.ConfirmationAgent)
                //{
                //    DataSet ds = Insure.INGetPossibleBumpUpsAssignedToBUAgent(_agentID);

                //    DataRelation relCampaignBatch = new DataRelation("CampaignBatch", ds.Tables[0].Columns["CampaignGroup"], ds.Tables[1].Columns["CampaignGroup"]);
                //    ds.Relations.Add(relCampaignBatch);

                //    DataColumn[] parentColumns = new DataColumn[] { ds.Tables[1].Columns["DateOfSale"], ds.Tables[1].Columns["CampaignGroup"] };
                //    DataColumn[] childColumns = new DataColumn[] { ds.Tables[2].Columns["DateOfSale"], ds.Tables[2].Columns["CampaignGroup"] };

                //    DataRelation relBatchAgent = new DataRelation("BatchLead", parentColumns, childColumns);
                //    ds.Relations.Add(relBatchAgent);

                //    xdgSales.DataSource = ds.Tables[0].DefaultView;
                //    //foreach (Field field in xdgSales.FieldLayouts[2].Fields)
                //    //{
                //    //    string fieldName = field.Name;
                //    //    if (fieldName == "CallBackDate")
                //    //    {

                //    //        xdgSales.FieldLayouts[2].Fields[fieldName].Settings.AllowEdit = true;
                //    //    }
                //    //    else
                //    //    {
                //    //        continue;
                //    //    }
                //    //}
                //}
                //else if (UserType == lkpUserType.DebiCheckAgent)
                //{

                //    DataSet ds = Insure.INGetSalesAssignedToDCAgent(_agentID);

                //    DataRelation relCampaignBatch = new DataRelation("CampaignBatch", ds.Tables[0].Columns["CampaignGroupType"], ds.Tables[1].Columns["CampaignGroupType"]);
                //    ds.Relations.Add(relCampaignBatch);

                //    DataColumn[] parentColumns = new DataColumn[] { ds.Tables[1].Columns["DateOfSale"], ds.Tables[1].Columns["CampaignGroupType"] };
                //    DataColumn[] childColumns = new DataColumn[] { ds.Tables[2].Columns["DateOfSale"], ds.Tables[2].Columns["CampaignGroupType"] };

                //    DataRelation relBatchAgent = new DataRelation("BatchLead", parentColumns, childColumns);
                //    ds.Relations.Add(relBatchAgent);

                //    xdgSales.DataSource = ds.Tables[0].DefaultView;

                //}
                if (UserType == lkpUserType.SalesAgent)
                {
#if TRAININGBUILD
                        DataSet ds = Insure.INGetLeadsAssignedToUserTraining(_agentID);
#else
                    DataSet ds = Insure.INGetLeadsAssignedToUser(_agentID);
#endif

                    DataRelation relCampaignBatch = new DataRelation("CampaignBatch", ds.Tables[0].Columns["CampaignID"], ds.Tables[1].Columns["CampaignID"]);
                    ds.Relations.Add(relCampaignBatch);

                    DataRelation relBatchAgent = new DataRelation("BatchLead", ds.Tables[1].Columns["LeadBookID"], ds.Tables[2].Columns["LeadBookID"]);
                    ds.Relations.Add(relBatchAgent);

                    //ds.Tables[2].Columns.Add("CallBack", Type.GetType("System.Boolean"));

                    xdgSales.DataSource = ds.Tables[0].DefaultView;

                    foreach (var r in xdgSales.Records)
                    {
                        r.IsExpanded = true;
                        foreach (var c in r.ViewableChildRecords)
                        {
                            c.IsExpanded = true;
                            foreach (var c1 in c.ViewableChildRecords)
                            {
                                c1.IsExpanded = true;
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
                SetCursor(Cursors.Arrow);
            }


        }

        private void LoadAgentDetails()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataTable dt = Methods.GetTableData("SELECT * FROM [User] WHERE ID = " + _agentID);
                DataTable dtJobTitle = Methods.GetTableData("SELECT FKJobTitleID FROM [Blush].[dbo].[HRStaff] WHERE FKUserID = " + _agentID);
                if (dtJobTitle.Rows.Count > 0 && dtJobTitle.Rows[0]["FKJobTitleID"] != DBNull.Value)
                {
                    JobTitle = Convert.ToInt32(dtJobTitle.Rows[0]["FKJobTitleID"]);

                    _agent = Methods.ToCollection<User>(dt)[0];

                    lblAgent.Text = _agent.FirstName.Trim() + " " + _agent.LastName.Trim();
                    UserType = (lkpUserType?)((User)GlobalSettings.ApplicationUser).FKUserType;
                }

                //xrgLiveProductivity.G
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

        private void LoadTimer()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += AutoRefreshTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 30, 0);
            if (UserType == lkpUserType.CallMonitoringAgent || UserType == lkpUserType.ConfirmationAgent || UserType == lkpUserType.Preserver || UserType == lkpUserType.DebiCheckAgent)
            {
                dispatcherTimer.Start();
            }

        }

        private void ShowLeadApplicationScreen(long? ImportID)
        {
            try
            {

                //CheckUserVersion(Username); 

                DataRecord record = (DataRecord)xdgSales.ActiveRecord;
                DataRecord drBatch = null;
                DataRecord drCampaign = null;
                RecordFilter filterLead = null;
                int? rIndex = null;

                if (record != null && record.FieldLayout.Description == "Lead")
                {
                    rIndex = record.Index;
                    drBatch = record.ParentDataRecord;
                    drCampaign = drBatch.ParentDataRecord;

                    if (record.ParentRecord.ViewableChildRecords[0].RecordType == RecordType.FilterRecord)
                    {
                        FilterRecord filterRecord = (FilterRecord)record.ParentRecord.ViewableChildRecords[0];
                        filterLead = filterRecord.HasActiveFilters ? filterRecord.Cells["Lead"].RecordFilter : null;
                    }
                }

                #region Checking if the lead to be loaded has a status of "DO NOT CONTACT"

                bool clientCanBeContacted = Insure.CanClientBeContacted(ImportID);
                if (!clientCanBeContacted)
                {
                    INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                    ShowMessageBox(messageWindow, @"This lead cannot be loaded in its entirety, because the client has requested not to be contacted again.", "DO NOT CONTACT CLIENT", ShowMessageType.Exclamation);
                }

                #endregion Checking if the lead to be loaded has a status of "DO NOT CONTACT"

                #region Determining whether or not the lead has a status of cancelled
                // Please see https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/204734160/comments
                // Please see https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/211742618/comments

                //bool hasLeadBeenCancelled = Insure.HasLeadBeenCancelled(ImportID);

                //if (hasLeadBeenCancelled)
                //{
                //    INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                //    ShowMessageBox(messageWindow, @"This lead cannot be loaded, because the policy has been cancelled by the client. Please consult your supervisor.", "Cancelled Policy", ShowMessageType.Exclamation);
                //    return;
                //}

                #endregion Determining whether or not the lead has a status of cancelled


                if (UserType == lkpUserType.CallMonitoringAgent || UserType == lkpUserType.Preserver)
                {
                    if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
                    {
                        SetAgentOffline();
                        timer.Stop();
                    }


                    LeadApplicationScreen leadApplicationScreen = new LeadApplicationScreen(ImportID, _ssGlobalData, true);
                    ShowDialog(leadApplicationScreen, new INDialogWindow(leadApplicationScreen));





                }
                else
                {
                    if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
                    {
                        SetAgentOffline();
                        timer.Stop();
                    }

                    LeadApplicationScreen leadApplicationScreen = new LeadApplicationScreen(ImportID, _ssGlobalData);
                    ShowDialog(leadApplicationScreen, new INDialogWindow(leadApplicationScreen));
                }


                LoadSalesData();

                InboxLastView(xdgSales, drCampaign, drBatch, filterLead, rIndex);
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void InboxLastView(XamDataGrid xamDataGrid, DataRecord drOriginalCampaign, DataRecord drOriginalBatch, RecordFilter rfLeadFilter, int? RecordIndex)
        {
            try
            {
                xamDataGrid.Records.CollapseAll(true);

                foreach (Record rCampaign in xamDataGrid.Records)
                {
                    DataRecord drCampaign = rCampaign as DataRecord;
                    if (drCampaign != null && drOriginalCampaign != null)
                    {
                        string drCampaignStr = drCampaign.ToString().Split(':')[1].Trim();
                        string drOriginalCampaignStr = drOriginalCampaign.ToString().Split(':')[1].Trim();

                        if (drCampaignStr == drOriginalCampaignStr)
                        {
                            drCampaign.IsActive = true;
                            drCampaign.IsExpanded = true;

                            if (drCampaign.HasChildren)
                            {
                                foreach (Record rBatch in drCampaign.ViewableChildRecords[0].ViewableChildRecords)
                                {
                                    DataRecord drBatch = rBatch as DataRecord;
                                    if (drBatch != null && drOriginalBatch != null)
                                    {
                                        string drBatchStr = drBatch.ToString();//.Split(':')[1].Trim();
                                        string drOriginalBatchStr = drOriginalBatch.ToString();//.Split(':')[1].Trim();

                                        if (drBatchStr == drOriginalBatchStr)
                                        {
                                            drBatch.IsActive = true;
                                            drBatch.IsExpanded = true;

                                            if (drBatch.HasChildren)
                                            {
                                                var record = xamDataGrid.ActiveRecord.ViewableChildRecords[0].ViewableChildRecords[0];
                                                //                    if (record.RecordType == RecordType.FilterRecord)
                                                //                    {
                                                //                        FilterRecord filterRecord = (FilterRecord)record;
                                                //                        if (rfLeadFilter != null)
                                                //                        {
                                                //                            ComparisonOperator condOperator = ((ComparisonCondition)rfLeadFilter.Conditions[0]).Operator;
                                                //                            var condValue = ((ComparisonCondition)rfLeadFilter.Conditions[0]).Value;
                                                //                            filterRecord.Cells["Lead"].RecordFilter.Conditions.Add(new ComparisonCondition(condOperator, condValue));
                                                //                        }
                                                //                    }

                                                int count = drBatch.ViewableChildRecords[0].ViewableChildRecords.Count;

                                                if (RecordIndex != null && RecordIndex >= 0 && RecordIndex <= count - 1)
                                                {
                                                    if (RecordIndex < 11)
                                                    {
                                                        xamDataGrid.BringRecordIntoView(drBatch.ViewableChildRecords[0].ViewableChildRecords[(int)RecordIndex]);
                                                    }
                                                    else if (RecordIndex + 11 < count)
                                                    {
                                                        xamDataGrid.BringRecordIntoView(drBatch.ViewableChildRecords[0].ViewableChildRecords[(int)RecordIndex + 11]);
                                                    }
                                                    else if (RecordIndex + 12 > count)
                                                    {
                                                        xamDataGrid.BringRecordIntoView(drBatch.ViewableChildRecords[0].ViewableChildRecords[count - 1]);
                                                    }

                                                    xamDataGrid.ActiveRecord = ((ExpandableFieldRecord)drBatch.ViewableChildRecords[0]).ChildRecords[(int)RecordIndex];
                                                }

                                                xamDataGrid.Focus();
                                            }
                                        }
                                    }
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
        }

        private void LoadSalesGrid()
        {
            try
            {
                if (UserType == lkpUserType.CallMonitoringAgent || UserType == lkpUserType.Preserver)
                {
                    #region Campaign

                    FieldLayout flCampaign = new FieldLayout();
                    flCampaign.Key = "Campaign";

                    //Field fieldCampaignID = new Field("CampaignID");
                    //fieldCampaignID.Visibility = Visibility.Collapsed;

                    Field fieldCampaignName = new Field("CampaignName");
                    fieldCampaignName.Visibility = Visibility.Visible;
                    fieldCampaignName.Width = new FieldLength(300);
                    fieldCampaignName.Label = "Campaign";

                    Field fieldCampaignCode = new Field("CampaignCode");
                    fieldCampaignCode.Visibility = Visibility.Collapsed;

                    Field fieldCampaignGroupType = new Field("CampaignGroupType");
                    fieldCampaignGroupType.Visibility = Visibility.Collapsed;

                    //Field fieldCampaignGroup = new Field("CampaignGroup");
                    //fieldCampaignGroup.Visibility = Visibility.Collapsed;

                    //flCampaign.Fields.Add(fieldCampaignID);

                    flCampaign.Fields.Add(fieldCampaignName);

                    flCampaign.Fields.Add(fieldCampaignCode);

                    flCampaign.Fields.Add(fieldCampaignGroupType);

                    //flCampaign.Fields.Add(fieldCampaignGroup);

                    xdgSales.FieldLayouts.Add(flCampaign);

                    #endregion Campaign

                    #region Batch(DateOfSale)
                    FieldLayout flBatch = new FieldLayout();
                    flBatch.Key = "Batch";
                    flBatch.ParentFieldLayoutKey = "Campaign";

                    Field fieldBatchDateOfSale = new Field("DateOfSale");
                    fieldBatchDateOfSale.Visibility = Visibility.Visible;
                    fieldBatchDateOfSale.Width = new FieldLength(110);
                    fieldBatchDateOfSale.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldBatchDateOfSale.Label = "Date Of Sale";

                    Field fieldBatchCampaignGroupType = new Field("CampaignGroupType");
                    fieldBatchCampaignGroupType.Visibility = Visibility.Collapsed;


                    //Field fieldBatchID = new Field("BatchID");
                    //fieldBatchID.Visibility = Visibility.Collapsed;

                    //Field fieldBatchCampaignID = new Field("CampaignID");
                    //fieldBatchCampaignID.Visibility = Visibility.Collapsed;

                    //Field fieldCampaignGroupID = new Field("CampaignGroupID");
                    //fieldCampaignGroupID.Visibility = Visibility.Collapsed;

                    //Field fieldLeadBookID = new Field("LeadBookID");
                    //fieldLeadBookID.Visibility = Visibility.Collapsed;

                    //Field fieldBatchCode = new Field("BatchCode");
                    //fieldBatchCode.Visibility = Visibility.Visible;
                    //fieldBatchCode.Width = new FieldLength(160);
                    //fieldBatchCode.Label = "Batch";

                    //Field fieldAllocationDate = new Field("AllocationDate");
                    //fieldAllocationDate.Visibility = Visibility.Visible;
                    //fieldAllocationDate.Width = new FieldLength(150);
                    //fieldAllocationDate.Label = "Allocation Date";

                    //Field fieldExpireDate = new Field("ExpireDate");
                    //fieldExpireDate.Visibility = Visibility.Visible;
                    //fieldExpireDate.Width = new FieldLength(150);
                    //fieldExpireDate.Label = "Expire Date";

                    //flBatch.Fields.Add(fieldBatchID);

                    //flBatch.Fields.Add(fieldBatchCampaignID);

                    //flBatch.Fields.Add(fieldCampaignGroupID);

                    //flBatch.Fields.Add(fieldLeadBookID);

                    //flBatch.Fields.Add(fieldBatchCode);

                    //flBatch.Fields.Add(fieldAllocationDate);

                    //flBatch.Fields.Add(fieldExpireDate);

                    flBatch.Fields.Add(fieldBatchDateOfSale);

                    flBatch.Fields.Add(fieldBatchCampaignGroupType);

                    xdgSales.FieldLayouts.Add(flBatch);
                    #endregion Batch

                    #region Lead
                    FieldLayout flLead = new FieldLayout();
                    flLead.Key = "Lead";
                    flLead.ParentFieldLayoutKey = "Batch";

                    Field fieldLeadDateOfSale = new Field("DateOfSale");
                    fieldLeadDateOfSale.Visibility = Visibility.Visible;
                    fieldLeadDateOfSale.Width = new FieldLength(110);
                    fieldLeadDateOfSale.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldLeadDateOfSale.Label = "Date Of Sale";

                    Field fieldActualDateOfSale = new Field("ActualDateOfSale");
                    fieldActualDateOfSale.Visibility = Visibility.Visible;
                    fieldActualDateOfSale.Width = new FieldLength(110);
                    fieldActualDateOfSale.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldActualDateOfSale.Label = "Actual Date Of Sale";

                    Field fieldAllocationDateTime = new Field("AllocationDateTime");
                    //fieldAllocationDateTime.Settings.EditorType = typeof(XamDateTimeEditor);
                    //fieldAllocationDateTime.Settings.EditAsType = typeof(DateTime);
                    //fieldAllocationDateTime.Settings.EditorStyle = styleAllocation;
                    fieldAllocationDateTime.Visibility = Visibility.Visible;
                    fieldAllocationDateTime.Width = new FieldLength(180);
                    fieldAllocationDateTime.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldAllocationDateTime.Label = "Date and Time of Allocation";

                    Field fieldExpiryDate = new Field("ExpiryDate");
                    fieldExpiryDate.Visibility = Visibility.Visible;
                    fieldExpiryDate.Width = new FieldLength(110);
                    fieldExpiryDate.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldExpiryDate.Label = "Expiry Date";

                    Field fieldIsTSRBUSavedCarriedForward = new Field("IsTSRBUSavedCarriedForward");
                    fieldIsTSRBUSavedCarriedForward.Visibility = Visibility.Visible;
                    fieldIsTSRBUSavedCarriedForward.Width = new FieldLength(80);
                    fieldIsTSRBUSavedCarriedForward.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldIsTSRBUSavedCarriedForward.Label = "TSR/BU Saved Carried Forward";

                    Field fieldRefNo = new Field("RefNo");
                    fieldRefNo.Visibility = Visibility.Visible;
                    fieldRefNo.Width = new FieldLength(165);
                    fieldRefNo.Label = "Reference Number";

                    Field fieldTSR = new Field("TSR");
                    fieldTSR.Visibility = Visibility.Visible;
                    fieldTSR.Width = new FieldLength(250);
                    fieldTSR.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldTSR.Label = "TSR Name";

                    Field fieldLeadStatus = new Field("LeadStatus");
                    fieldLeadStatus.Visibility = Visibility.Visible;
                    fieldLeadStatus.Width = new FieldLength(275);
                    fieldLeadStatus.Label = "Lead Status";

                    Field fieldCallMonitoringStatus = new Field("CallMonitoringStatus");
                    fieldCallMonitoringStatus.Visibility = Visibility.Visible;
                    fieldCallMonitoringStatus.Width = new FieldLength(104);
                    fieldCallMonitoringStatus.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldCallMonitoringStatus.Label = "Call Monitoring Status";

                    Field fieldIsBumpedUp = new Field("IsBumpedUp");
                    fieldIsBumpedUp.Visibility = Visibility.Visible;
                    fieldIsBumpedUp.Width = new FieldLength(84);
                    fieldIsBumpedUp.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldIsBumpedUp.Label = "Bumped Up";

                    Field fieldLeadCampaignGroupType = new Field("CampaignGroupType");
                    fieldLeadCampaignGroupType.Visibility = Visibility.Collapsed;

                    Field fieldIsOverAssessment = new Field("IsOverAssessment");
                    fieldIsOverAssessment.Visibility = Visibility.Collapsed;
                    //fieldIsOverAssessment.Width = new FieldLength(110);
                    //fieldIsOverAssessment.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    //fieldIsOverAssessment.Label = "Is Over Assessment";

                    Field fieldImportID = new Field("ImportID");
                    fieldImportID.Visibility = Visibility.Collapsed;

                    flLead.Fields.Add(fieldLeadDateOfSale);

                    flLead.Fields.Add(fieldActualDateOfSale);

                    flLead.Fields.Add(fieldAllocationDateTime);

                    flLead.Fields.Add(fieldIsTSRBUSavedCarriedForward);

                    flLead.Fields.Add(fieldRefNo);

                    flLead.Fields.Add(fieldTSR);

                    flLead.Fields.Add(fieldLeadStatus);

                    flLead.Fields.Add(fieldCallMonitoringStatus);

                    flLead.Fields.Add(fieldIsBumpedUp);

                    flLead.Fields.Add(fieldExpiryDate);

                    flLead.Fields.Add(fieldLeadCampaignGroupType);

                    flLead.Fields.Add(fieldIsOverAssessment);

                    flLead.Fields.Add(fieldImportID);


                    xdgSales.FieldLayouts.Add(flLead);


                    #endregion Lead


                }
                else if (UserType == lkpUserType.ConfirmationAgent)
                {
                    #region Campaign

                    FieldLayout flCampaign = new FieldLayout();
                    flCampaign.Key = "Campaign";

                    Field fieldCampaignName = new Field("CampaignGroup");
                    fieldCampaignName.Visibility = Visibility.Visible;
                    fieldCampaignName.Width = new FieldLength(300);
                    fieldCampaignName.Label = "CampaignGroup";

                    flCampaign.Fields.Add(fieldCampaignName);

                    xdgSales.FieldLayouts.Add(flCampaign);

                    #endregion Campaign

                    #region Batch(DateOfSale)
                    FieldLayout flBatch = new FieldLayout();
                    flBatch.Key = "Batch";
                    flBatch.ParentFieldLayoutKey = "Campaign";

                    Field fieldBatchDateOfSale = new Field("DateOfSale");
                    fieldBatchDateOfSale.Visibility = Visibility.Visible;
                    fieldBatchDateOfSale.Width = new FieldLength(300);
                    fieldBatchDateOfSale.Label = "Date Of Sale";

                    Field fieldBatchCampaignGroupType = new Field("CampaignGroup");
                    fieldBatchCampaignGroupType.Visibility = Visibility.Collapsed;

                    flBatch.Fields.Add(fieldBatchDateOfSale);

                    flBatch.Fields.Add(fieldBatchCampaignGroupType);

                    xdgSales.FieldLayouts.Add(flBatch);
                    #endregion Batch

                    #region Lead
                    FieldLayout flLead = new FieldLayout();
                    flLead.Key = "Lead";
                    flLead.ParentFieldLayoutKey = "Batch";

                    Field fieldLeadDateOfSale = new Field("DateOfSale");
                    fieldLeadDateOfSale.Visibility = Visibility.Visible;
                    fieldLeadDateOfSale.Width = new FieldLength(200);
                    fieldLeadDateOfSale.Label = "Date Of Sale";

                    Field fieldAllocationDateTime = new Field("AllocationDateTime");

                    fieldAllocationDateTime.Visibility = Visibility.Visible;
                    fieldAllocationDateTime.Width = new FieldLength(235);
                    fieldAllocationDateTime.Label = "Date and Time of Allocation";

                    Field fieldExpiryDate = new Field("ExpiryDate");
                    fieldExpiryDate.Visibility = Visibility.Visible;
                    fieldExpiryDate.Width = new FieldLength(107);
                    fieldExpiryDate.Label = "Expiry Date";

                    Field fieldRefNo = new Field("RefNo");
                    fieldRefNo.Visibility = Visibility.Visible;
                    fieldRefNo.Width = new FieldLength(195);
                    fieldRefNo.Label = "Reference Number";

                    Field fieldTSR = new Field("TSR");
                    fieldTSR.Visibility = Visibility.Visible;
                    fieldTSR.Width = new FieldLength(350);
                    fieldTSR.Label = "TSR Name";

                    Field fieldLeadStatus = new Field("LeadStatus");
                    fieldLeadStatus.Visibility = Visibility.Visible;
                    fieldLeadStatus.Width = new FieldLength(300);
                    fieldLeadStatus.Label = "Lead Status";

                    Field fieldLeadStatusID = new Field("LeadStatusID");
                    fieldLeadStatusID.Visibility = Visibility.Hidden;
                    fieldLeadStatusID.Label = "Lead Status ID";

                    Field fieldCallMonitoringStatus = new Field("BumpUpStatus");
                    fieldCallMonitoringStatus.Visibility = Visibility.Visible;
                    fieldCallMonitoringStatus.Width = new FieldLength(300);
                    fieldCallMonitoringStatus.Label = "Bump-Up Status";

                    Field fieldPremium = new Field("Premium");
                    fieldPremium.Visibility = Visibility.Visible;
                    fieldPremium.Width = new FieldLength(300);
                    fieldPremium.Label = "Premium";

                    Field fieldBumpUpOffered = new Field("BumpUpOffered");
                    fieldBumpUpOffered.Visibility = Visibility.Visible;
                    fieldBumpUpOffered.Width = new FieldLength(200);
                    fieldBumpUpOffered.Label = "Bump-Up Offered";


                    Field fieldCallBackDate = new Field("CallBackDate");
                    fieldCallBackDate.Visibility = Visibility.Visible;
                    fieldCallBackDate.Width = new FieldLength(300);
                    fieldCallBackDate.Label = "Diary Date";
                    fieldCallBackDate.Settings.AllowEdit = true;



                    XamDateTimeEditor editor = new XamDateTimeEditor();
                    editor.DropDownButtonDisplayMode = DropDownButtonDisplayMode.Always;

                    //List<string> Statuses = new List<string>();
                    //Statuses.Add("Call Back");

                    //XamComboEditor cmb = new XamComboEditor();
                    //cmb.ItemsSource = Statuses;


                    //editor.PromptChar = ' ';

                    //XamDateTimeInput inputEditor = new XamDateTimeInput();
                    //inputEditor.SpinButtonDisplayMode = Infragistics.Controls.Editors.SpinButtonDisplayMode.MouseOver;

                    //Calendar cal = new Calendar();

                    //ControlTemplate controlTemplate = new ControlTemplate(typeof(CellValuePresenter));



                    //Style dateTimeStyle = new Style(typeof(CellValuePresenter));

                    //dateTimeStyle.Setters.Add(new Setter(CellValuePresenter.TemplateProperty, controlTemplate));

                    //CellValuePresenter style = new CellValuePresenter();
                    //style.
                    Style style = new Style(typeof(XamDateTimeEditor));

                    //Style style = new Style(typeof(XamDateTimeInput));

                    style.Setters.Add(new Setter(XamDateTimeEditor.MaskProperty, "yyyy-mm-dd hh:mm"));
                    //style.Setters.Add(new Setter(XamDateTimeInput.MaskProperty, "yyyy-mm-dd hh:mm:ss"));
                    //style.Setters.Add(new Setter(XamDateTimeInput.SpinButtonDisplayModeProperty, Infragistics.Controls.Editors.SpinButtonDisplayMode.MouseOver));
                    //style.Setters.Add(new Setter(XamDateTimeInput.DropDownButtonDisplayModeProperty, Infragistics.Controls.Editors.DropDownButtonDisplayMode.MouseOver));

                    style.Setters.Add(new Setter(XamDateTimeEditor.DropDownButtonDisplayModeProperty, DropDownButtonDisplayMode.Always));
                    style.Setters.Add(new Setter(XamDateTimeEditor.PromptCharProperty, ' '));




                    //Resources.Add(typeof(XamDateTimeEditor), style);


                    //fieldCallBackDate.Settings.CellValuePresenterStyle = StaticResource.callBackDateStyle;
                    fieldCallBackDate.Settings.EditorType = typeof(XamDateTimeEditor);
                    fieldCallBackDate.Settings.EditAsType = typeof(DateTime);
                    fieldCallBackDate.Settings.EditorStyle = style;
                    //fieldCallBackDate.Settings.EditorStyle = style;

                    Field fieldLeadCampaignGroupType = new Field("CampaignGroup");
                    fieldLeadCampaignGroupType.Visibility = Visibility.Collapsed;

                    Field fieldPossibleBumpUpID = new Field("PossibleBumpUpID");
                    fieldPossibleBumpUpID.Visibility = Visibility.Collapsed;

                    Field fieldImportID = new Field("ImportID");
                    fieldImportID.Visibility = Visibility.Collapsed;

                    flLead.Fields.Add(fieldLeadDateOfSale);

                    flLead.Fields.Add(fieldAllocationDateTime);

                    flLead.Fields.Add(fieldExpiryDate);

                    flLead.Fields.Add(fieldRefNo);

                    flLead.Fields.Add(fieldTSR);

                    flLead.Fields.Add(fieldLeadStatus);

                    flLead.Fields.Add(fieldCallMonitoringStatus);

                    flLead.Fields.Add(fieldBumpUpOffered);

                    flLead.Fields.Add(fieldCallBackDate);

                    flLead.Fields.Add(fieldPremium);

                    flLead.Fields.Add(fieldLeadCampaignGroupType);

                    flLead.Fields.Add(fieldPossibleBumpUpID);

                    flLead.Fields.Add(fieldImportID);

                    xdgSales.FieldLayouts.Add(flLead);
                    #endregion Lead

                    //foreach (FieldLayout fl in xdgSales.FieldLayouts)
                    //{
                    //    foreach (Field field in fl.Fields)
                    //    {
                    //        if (field.Name == "CallBackDate")
                    //        {
                    //            continue;
                    //        }
                    //        else
                    //        {
                    //            field.Settings.AllowEdit = false;
                    //        }                            
                    //    }
                    //}
                }
                else if (UserType == lkpUserType.DebiCheckAgent)
                {
                    #region Campaign

                    FieldLayout flCampaign = new FieldLayout();
                    flCampaign.Key = "Campaign";

                    //Field fieldCampaignID = new Field("CampaignID");
                    //fieldCampaignID.Visibility = Visibility.Collapsed;

                    Field fieldCampaignName = new Field("CampaignName");
                    fieldCampaignName.Visibility = Visibility.Visible;
                    fieldCampaignName.Width = new FieldLength(300);
                    fieldCampaignName.Label = "Campaign";

                    Field fieldCampaignCode = new Field("CampaignCode");
                    fieldCampaignCode.Visibility = Visibility.Collapsed;

                    Field fieldCampaignGroupType = new Field("CampaignGroupType");
                    fieldCampaignGroupType.Visibility = Visibility.Collapsed;

                    //Field fieldCampaignGroup = new Field("CampaignGroup");
                    //fieldCampaignGroup.Visibility = Visibility.Collapsed;

                    //flCampaign.Fields.Add(fieldCampaignID);

                    flCampaign.Fields.Add(fieldCampaignName);

                    flCampaign.Fields.Add(fieldCampaignCode);

                    flCampaign.Fields.Add(fieldCampaignGroupType);

                    //flCampaign.Fields.Add(fieldCampaignGroup);

                    xdgSales.FieldLayouts.Add(flCampaign);

                    #endregion Campaign

                    #region Batch(DateOfSale)
                    FieldLayout flBatch = new FieldLayout();
                    flBatch.Key = "Batch";
                    flBatch.ParentFieldLayoutKey = "Campaign";

                    Field fieldBatchDateOfSale = new Field("DateOfSale");
                    fieldBatchDateOfSale.Visibility = Visibility.Visible;
                    fieldBatchDateOfSale.Width = new FieldLength(200);
                    fieldBatchDateOfSale.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldBatchDateOfSale.Label = "Date Of Allocation";

                    Field fieldBatchCampaignGroupType = new Field("CampaignGroupType");
                    fieldBatchCampaignGroupType.Visibility = Visibility.Collapsed;


                    //Field fieldBatchID = new Field("BatchID");
                    //fieldBatchID.Visibility = Visibility.Collapsed;

                    //Field fieldBatchCampaignID = new Field("CampaignID");
                    //fieldBatchCampaignID.Visibility = Visibility.Collapsed;

                    //Field fieldCampaignGroupID = new Field("CampaignGroupID");
                    //fieldCampaignGroupID.Visibility = Visibility.Collapsed;

                    //Field fieldLeadBookID = new Field("LeadBookID");
                    //fieldLeadBookID.Visibility = Visibility.Collapsed;

                    //Field fieldBatchCode = new Field("BatchCode");
                    //fieldBatchCode.Visibility = Visibility.Visible;
                    //fieldBatchCode.Width = new FieldLength(160);
                    //fieldBatchCode.Label = "Batch";

                    //Field fieldAllocationDate = new Field("AllocationDate");
                    //fieldAllocationDate.Visibility = Visibility.Visible;
                    //fieldAllocationDate.Width = new FieldLength(150);
                    //fieldAllocationDate.Label = "Allocation Date";

                    //Field fieldExpireDate = new Field("ExpireDate");
                    //fieldExpireDate.Visibility = Visibility.Visible;
                    //fieldExpireDate.Width = new FieldLength(150);
                    //fieldExpireDate.Label = "Expire Date";

                    //flBatch.Fields.Add(fieldBatchID);

                    //flBatch.Fields.Add(fieldBatchCampaignID);

                    //flBatch.Fields.Add(fieldCampaignGroupID);

                    //flBatch.Fields.Add(fieldLeadBookID);

                    //flBatch.Fields.Add(fieldBatchCode);

                    //flBatch.Fields.Add(fieldAllocationDate);

                    //flBatch.Fields.Add(fieldExpireDate);

                    flBatch.Fields.Add(fieldBatchDateOfSale);

                    flBatch.Fields.Add(fieldBatchCampaignGroupType);

                    xdgSales.FieldLayouts.Add(flBatch);
                    #endregion Batch

                    #region Lead
                    FieldLayout flLead = new FieldLayout();
                    flLead.Key = "Lead";
                    flLead.ParentFieldLayoutKey = "Batch";

                    Field fieldLeadDateOfSale = new Field("DateOfSale");
                    fieldLeadDateOfSale.Visibility = Visibility.Visible;
                    fieldLeadDateOfSale.Width = new FieldLength(110);
                    fieldLeadDateOfSale.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldLeadDateOfSale.Label = "Date Of Allocation";

                    Field fieldActualDateOfSale = new Field("ActualDateOfSale");
                    fieldActualDateOfSale.Visibility = Visibility.Visible;
                    fieldActualDateOfSale.Width = new FieldLength(110);
                    fieldActualDateOfSale.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldActualDateOfSale.Label = "Actual Date Of Sale";

                    Field fieldAllocationDateTime = new Field("AllocationDateTime");
                    //fieldAllocationDateTime.Settings.EditorType = typeof(XamDateTimeEditor);
                    //fieldAllocationDateTime.Settings.EditAsType = typeof(DateTime);
                    //fieldAllocationDateTime.Settings.EditorStyle = styleAllocation;
                    fieldAllocationDateTime.Visibility = Visibility.Collapsed;
                    fieldAllocationDateTime.Width = new FieldLength(180);
                    fieldAllocationDateTime.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldAllocationDateTime.Label = "Date and Time of Allocation";

                    Field fieldExpiryDate = new Field("ExpiryDate");
                    fieldExpiryDate.Visibility = Visibility.Visible;
                    fieldExpiryDate.Width = new FieldLength(110);
                    fieldExpiryDate.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldExpiryDate.Label = "Expiry Date";

                    Field fieldIsTSRBUSavedCarriedForward = new Field("IsTSRBUSavedCarriedForward");
                    fieldIsTSRBUSavedCarriedForward.Visibility = Visibility.Visible;
                    fieldIsTSRBUSavedCarriedForward.Width = new FieldLength(80);
                    fieldIsTSRBUSavedCarriedForward.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldIsTSRBUSavedCarriedForward.Label = "TSR/BU Saved Carried Forward";

                    Field fieldRefNo = new Field("RefNo");
                    fieldRefNo.Visibility = Visibility.Visible;
                    fieldRefNo.Width = new FieldLength(165);
                    fieldRefNo.Label = "Reference Number";

                    Field fieldTSR = new Field("TSR");
                    fieldTSR.Visibility = Visibility.Visible;
                    fieldTSR.Width = new FieldLength(250);
                    fieldTSR.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldTSR.Label = "TSR Name";

                    Field fieldLeadStatus = new Field("LeadStatus");
                    fieldLeadStatus.Visibility = Visibility.Visible;
                    fieldLeadStatus.Width = new FieldLength(275);
                    fieldLeadStatus.Label = "Lead Status";

                    Field fieldCallMonitoringStatus = new Field("CallMonitoringStatus");
                    fieldCallMonitoringStatus.Visibility = Visibility.Visible;
                    fieldCallMonitoringStatus.Width = new FieldLength(104);
                    fieldCallMonitoringStatus.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldCallMonitoringStatus.Label = "Call Monitoring Status";

                    Field fieldIsBumpedUp = new Field("IsBumpedUp");
                    fieldIsBumpedUp.Visibility = Visibility.Visible;
                    fieldIsBumpedUp.Width = new FieldLength(84);
                    fieldIsBumpedUp.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    fieldIsBumpedUp.Label = "Bumped Up";

                    Field fieldLeadCampaignGroupType = new Field("CampaignGroupType");
                    fieldLeadCampaignGroupType.Visibility = Visibility.Collapsed;

                    Field fieldIsOverAssessment = new Field("IsOverAssessment");
                    fieldIsOverAssessment.Visibility = Visibility.Collapsed;
                    //fieldIsOverAssessment.Width = new FieldLength(110);
                    //fieldIsOverAssessment.Settings.LabelTextWrapping = TextWrapping.Wrap;
                    //fieldIsOverAssessment.Label = "Is Over Assessment";

                    Field fieldImportID = new Field("ImportID");
                    fieldImportID.Visibility = Visibility.Collapsed;

                    flLead.Fields.Add(fieldLeadDateOfSale);

                    flLead.Fields.Add(fieldActualDateOfSale);

                    flLead.Fields.Add(fieldAllocationDateTime);

                    flLead.Fields.Add(fieldIsTSRBUSavedCarriedForward);

                    flLead.Fields.Add(fieldRefNo);

                    flLead.Fields.Add(fieldTSR);

                    flLead.Fields.Add(fieldLeadStatus);

                    flLead.Fields.Add(fieldCallMonitoringStatus);

                    flLead.Fields.Add(fieldIsBumpedUp);

                    flLead.Fields.Add(fieldExpiryDate);

                    flLead.Fields.Add(fieldLeadCampaignGroupType);

                    flLead.Fields.Add(fieldIsOverAssessment);

                    flLead.Fields.Add(fieldImportID);


                    xdgSales.FieldLayouts.Add(flLead);


                    #endregion Lead


                }
                else
                {

                    #region Campaign

                    FieldLayout flCampaign = new FieldLayout();
                    flCampaign.Key = "Campaign";

                    Field fieldCampaignID = new Field("CampaignID");
                    fieldCampaignID.Visibility = Visibility.Collapsed;

                    Field fieldCampaignName = new Field("CampaignName");
                    fieldCampaignName.Visibility = Visibility.Visible;
                    fieldCampaignName.Width = new FieldLength(300);
                    fieldCampaignName.Label = "Campaign";

                    Field fieldCampaignCode = new Field("CampaignCode");
                    fieldCampaignCode.Visibility = Visibility.Collapsed;

                    Field fieldCampaignType = new Field("CampaignType");
                    fieldCampaignType.Visibility = Visibility.Collapsed;

                    Field fieldCampaignGroup = new Field("CampaignGroup");
                    fieldCampaignGroup.Visibility = Visibility.Collapsed;

                    flCampaign.Fields.Add(fieldCampaignID);

                    flCampaign.Fields.Add(fieldCampaignName);

                    flCampaign.Fields.Add(fieldCampaignCode);

                    flCampaign.Fields.Add(fieldCampaignType);

                    flCampaign.Fields.Add(fieldCampaignGroup);

                    xdgSales.FieldLayouts.Add(flCampaign);

                    #endregion Campaign

                    #region Batch
                    FieldLayout flBatch = new FieldLayout();
                    flBatch.Key = "Batch";
                    flBatch.ParentFieldLayoutKey = "Campaign";

                    Field fieldBatchID = new Field("BatchID");
                    fieldBatchID.Visibility = Visibility.Collapsed;

                    Field fieldBatchCampaignID = new Field("CampaignID");
                    fieldBatchCampaignID.Visibility = Visibility.Collapsed;

                    Field fieldCampaignGroupID = new Field("CampaignGroupID");
                    fieldCampaignGroupID.Visibility = Visibility.Collapsed;

                    Field fieldLeadBookID = new Field("LeadBookID");
                    fieldLeadBookID.Visibility = Visibility.Collapsed;

                    Field fieldBatchCode = new Field("BatchCode");
                    fieldBatchCode.Visibility = Visibility.Visible;
                    fieldBatchCode.Width = new FieldLength(160);
                    fieldBatchCode.Label = "Batch";

                    Field fieldAllocationDate = new Field("AllocationDate");
                    fieldAllocationDate.Visibility = Visibility.Visible;
                    fieldAllocationDate.Width = new FieldLength(150);
                    fieldAllocationDate.Label = "Allocation Date";

                    Field fieldExpireDate = new Field("ExpireDate");
                    fieldExpireDate.Visibility = Visibility.Visible;
                    fieldExpireDate.Width = new FieldLength(150);
                    fieldExpireDate.Label = "Expire Date";

                    flBatch.Fields.Add(fieldBatchID);

                    flBatch.Fields.Add(fieldBatchCampaignID);

                    flBatch.Fields.Add(fieldCampaignGroupID);

                    flBatch.Fields.Add(fieldLeadBookID);

                    flBatch.Fields.Add(fieldBatchCode);

                    flBatch.Fields.Add(fieldAllocationDate);

                    flBatch.Fields.Add(fieldExpireDate);

                    xdgSales.FieldLayouts.Add(flBatch);
                    #endregion Batch

                    #region Lead
                    FieldLayout flLead = new FieldLayout();
                    flLead.Key = "Lead";
                    flLead.ParentFieldLayoutKey = "Batch";

                    Field fieldLeadBatchID = new Field("BatchID");
                    fieldLeadBatchID.Visibility = Visibility.Collapsed;

                    Field fieldImportID = new Field("ImportID");
                    fieldImportID.Visibility = Visibility.Collapsed;

                    Field fieldLeadLeadBookID = new Field("LeadBookID");
                    fieldLeadLeadBookID.Visibility = Visibility.Collapsed;

                    Field fieldLead = new Field("Lead");
                    fieldLead.Settings.AllowRecordFiltering = true;
                    fieldLead.Settings.FilterOperatorDefaultValue = ComparisonOperator.StartsWith;
                    fieldLead.Settings.FilterOperatorDropDownItems = ComparisonOperatorFlags.None;
                    fieldLead.Visibility = Visibility.Visible;
                    fieldLead.Width = new FieldLength(260);
                    fieldLead.Label = "Lead";

                    Field fieldRefNo = new Field("RefNo");
                    fieldRefNo.Visibility = Visibility.Visible;
                    fieldRefNo.Width = new FieldLength(180);
                    fieldRefNo.Label = "Reference Number";

                    Field fieldReferrer = new Field("Referrer");
                    fieldReferrer.Visibility = Visibility.Visible;
                    fieldReferrer.Width = new FieldLength(160);
                    fieldReferrer.Label = "Referrer";

                    Field fieldStatus = new Field("Status");
                    fieldStatus.Visibility = Visibility.Visible;
                    fieldStatus.Width = new FieldLength(160);
                    fieldStatus.Label = "Status";

                    Field fieldDiaryReason = new Field("DiaryReason");
                    fieldDiaryReason.Visibility = Visibility.Visible;
                    fieldDiaryReason.Width = new FieldLength(160);
                    fieldDiaryReason.Label = "Diary Reason";

                    //XamCheckEditor cmb = new XamCheckEditor();
                    //cmb.IsChecked = false;
                    //cmb.Visibility = Visibility.Visible;
                    //cmb.MouseDown += Cmb_MouseDown;

                    Field fieldCallBack = new Field("CallBack");
                    fieldCallBack.Visibility = Visibility.Visible;
                    fieldCallBack.Width = new FieldLength(160);
                    fieldCallBack.Label = "Call Back?";
                    fieldCallBack.Settings.EditorType = typeof(XamCheckEditor);

                    //XamCheckEditor cmb = new XamCheckEditor();
                    //cmb.IsChecked = false;
                    //cmb.Visibility = Visibility.Visible;
                    //cmb.MouseDown += Cmb_MouseDown;

                    //List<string> Statuses = new List<string>();
                    //Statuses.Add("Call Back");



                    //cmb.IsChecked = false;
                    //cmb.IsReadOnly = false;
                    //cmb.MouseDown += Cmb_MouseDown;

                    //Field fieldLeadBookRank = new Field("LeadBookRank");
                    //fieldLeadBookRank.Visibility = Visibility.Collapsed;
                    //fieldLeadBookRank.Width = new FieldLength(160);
                    //fieldLeadBookRank.Label = "LeadBookRank";




                    //fieldCallBack.Settings.EditorType = typeof(XamCheckEditor);

                    flLead.Fields.Add(fieldLeadBatchID);

                    flLead.Fields.Add(fieldImportID);

                    flLead.Fields.Add(fieldLeadLeadBookID);

                    flLead.Fields.Add(fieldLead);

                    flLead.Fields.Add(fieldRefNo);

                    flLead.Fields.Add(fieldReferrer);

                    flLead.Fields.Add(fieldStatus);

                    flLead.Fields.Add(fieldDiaryReason);

                    //flLead.Fields.Add(fieldLeadBookRank);

                    flLead.Fields.Add(fieldCallBack);

                    xdgSales.FieldLayouts.Add(flLead);
                    #endregion Lead

                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void CheckUserVersion(string Username)
        {

            try
            {

                string LatestVersion;
                string Version;

                Username = ((User)GlobalSettings.ApplicationUser).LoginName;

                DataTable dtVersionData = User.INGetVersionInfo(Username);

                DataTable dtLatestVersionData = User.INGetLatestVersion();

                LatestVersion = dtLatestVersionData.Rows[0]["Version"].ToString();

                Version = dtVersionData.Rows[0]["Version"].ToString();

                if (LatestVersion != Version)
                {

                    MessageBox.Show("This version of Insure is outdated. It will update now.", MessageBoxButton.OK.ToString());

                    System.Windows.Application.Current.Shutdown();
                    System.Windows.Forms.Application.Restart();

                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }


        }


        #endregion


        #region Event Handlers

        private void AutoRefreshTimer_Tick(object sender, EventArgs e)
        {
            LoadAgentDetails();
            LoadSalesData();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _ssGlobalData.ScheduleScreen.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    _ssGlobalData.ScheduleScreen.Close();
                });

                if ((_agent?.FKUserType == (long)lkpUserType.Administrator) || (_agent?.FKUserType == (long)lkpUserType.Manager))
                {
                    if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
                    {
                        SetAgentOffline();
                        timer.Stop();
                    }

                    MenuManagementScreen menuToolsScreen = new MenuManagementScreen(ScreenDirection.Reverse);
                    OnClose(menuToolsScreen);
                }
                else
                {
                    if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
                    {
                        SetAgentOffline();
                        timer.Stop();
                    }

                    StartScreen startScreen = new StartScreen();
                    OnClose(startScreen);


                }
            }
            catch (Exception ex)
            {
                if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
                {
                    SetAgentOffline();
                    timer.Stop();
                }
                HandleException(ex);
            }
        }

        private void btnApplication_Click(object sender, RoutedEventArgs e)
        {
            ShowLeadApplicationScreen(null);
        }

        private void xdgSales_KeyUp(object sender, KeyEventArgs e)
        {
            DataRecord record = (DataRecord)xdgSales.ActiveRecord;
            if (record != null && record.RecordType == RecordType.DataRecord && record.FieldLayout.Description == "Lead")
            {
                if ((e.Key >= Key.A) && (e.Key <= Key.Z))
                {
                    foreach (Record rLead in record.ParentRecord.ViewableChildRecords)
                    {
                        if (rLead.RecordType == RecordType.DataRecord)
                        {
                            DataRecord drLead = rLead as DataRecord;

                            if (drLead != null && drLead.Cells[2].Value != null)
                            {
                                if (drLead.Cells[2].Value.ToString()[0].ToString().ToUpper() == e.Key.ToString())
                                {
                                    int count = record.ParentRecord.ViewableChildRecords.Count;
                                    xdgSales.ActiveRecord = drLead;
                                    var recordIndex = xdgSales.ActiveRecord.Index;

                                    if (recordIndex >= 0)
                                    {
                                        if (recordIndex < 11)
                                        {
                                            xdgSales.BringRecordIntoView(record.ParentRecord.ViewableChildRecords[recordIndex]);
                                        }
                                        else if (recordIndex + 11 < count)
                                        {
                                            xdgSales.BringRecordIntoView(record.ParentRecord.ViewableChildRecords[recordIndex + 11]);
                                        }
                                        else if (recordIndex + 12 > count)
                                        {
                                            xdgSales.BringRecordIntoView(record.ParentRecord.ViewableChildRecords[count - 1]);
                                        }
                                    }
                                    //xdgSales.BringRecordIntoView(drLead);


                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void xdgSales_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataRecordCellArea drca = Utilities.GetAncestorFromType(e.OriginalSource as DependencyObject, typeof(DataRecordCellArea), false) as DataRecordCellArea;

                if (drca != null)
                {
                    if (xdgSales.ActiveRecord != null && xdgSales.ActiveRecord.RecordType == RecordType.DataRecord && xdgSales.ActiveRecord.FieldLayout.Description == "Lead")
                    {
                        if (UserType == lkpUserType.CallMonitoringAgent || UserType == lkpUserType.Preserver)
                        {
                            DataTable dtCMA = Methods.GetTableData("SELECT * FROM CallMonitoringAllocation AS CMA LEFT JOIN [User] AS U ON CMA.FKUserID = U.ID WHERE FKINImportID = " + Int64.Parse(((DataRecord)xdgSales.ActiveRecord).Cells["ImportID"].Value.ToString()));
                            if (dtCMA.AsEnumerable().Where(x => x["IsSavedCarriedForward"] as bool? == true && x["FKUserID"] as long? != _agentID).Count() > 0)
                            {
                                string cmAgentFirstName = dtCMA.AsEnumerable().Where(x => x["IsSavedCarriedForward"] as bool? == true).Select(x => x["FirstName"]).FirstOrDefault().ToString();
                                string cmAgentLastName = dtCMA.AsEnumerable().Where(x => x["IsSavedCarriedForward"] as bool? == true).Select(x => x["LastName"]).FirstOrDefault().ToString();
                                INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                                ShowMessageBox(messageWindow, "This saved carried forward has been allocated to " + cmAgentFirstName + " " + cmAgentLastName + " to be call monitored." +
                                    " Therefore it has been locked.", "Lead Locked", ShowMessageType.Error);
                            }
                            else
                            {
                                if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
                                {
                                    SetAgentOffline();
                                    timer.Stop();
                                }

                                string LeadStatusPulled = "";
                                string ImportIDString = ((DataRecord)xdgSales.ActiveRecord).Cells["ImportID"].Value.ToString();
                                try
                                {
                                    StringBuilder strQuery = new StringBuilder();
                                    strQuery.Append("SELECT FKINLeadStatusID [Code] ");
                                    strQuery.Append("FROM INImport ");
                                    strQuery.Append($"WHERE ID = " + ImportIDString);

                                    DataTable dt = Methods.GetTableData(strQuery.ToString());

                                    LeadStatusPulled = dt.Rows[0]["Code"].ToString();
                                }
                                catch { }


                                //if(LeadStatusPulled == "1")
                                //{
                                //    ShowLeadApplicationScreen(Int64.Parse(((DataRecord)xdgSales.ActiveRecord).Cells["ImportID"].Value.ToString()));
                                //}
                                //else
                                //{
                                //    if (CheckLeadValidity(((DataRecord)xdgSales.ActiveRecord).Cells["ImportID"].Value.ToString()))
                                //    {
                                //        ShowLeadApplicationScreen(Int64.Parse(((DataRecord)xdgSales.ActiveRecord).Cells["ImportID"].Value.ToString()));
                                //    }
                                //    else
                                //    {
                                //        Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() => {
                                //            //    MainBorder.BorderBrush = Brushes.LightBlue;
                                //            INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                                //            ShowMessageBox(messageWindow, "Do not contact !", "Platinum Conserved Lead.", ShowMessageType.Exclamation);
                                //        }));
                                //    }
                                //}

                                ShowLeadApplicationScreen(Int64.Parse(((DataRecord)xdgSales.ActiveRecord).Cells["ImportID"].Value.ToString()));

                            }
                        }
                        else
                        {

                            string LeadStatusPulled = "";
                            string ImportIDString = ((DataRecord)xdgSales.ActiveRecord).Cells["ImportID"].Value.ToString();
                            try
                            {
                                StringBuilder strQuery = new StringBuilder();
                                strQuery.Append("SELECT FKINLeadStatusID [Code] ");
                                strQuery.Append("FROM INImport ");
                                strQuery.Append($"WHERE ID = " + ImportIDString);

                                DataTable dt = Methods.GetTableData(strQuery.ToString());

                                LeadStatusPulled = dt.Rows[0]["Code"].ToString();
                            }
                            catch { }


                            //if (LeadStatusPulled == "1")
                            //{
                            //    ShowLeadApplicationScreen(Int64.Parse(((DataRecord)xdgSales.ActiveRecord).Cells["ImportID"].Value.ToString()));
                            //}
                            //else
                            //{
                            //    if (CheckLeadValidity(((DataRecord)xdgSales.ActiveRecord).Cells["ImportID"].Value.ToString()))
                            //    {
                            //        ShowLeadApplicationScreen(Int64.Parse(((DataRecord)xdgSales.ActiveRecord).Cells["ImportID"].Value.ToString()));
                            //    }
                            //    else
                            //    {
                            //        Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() => {
                            //            //    MainBorder.BorderBrush = Brushes.LightBlue;
                            //            INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                            //            ShowMessageBox(messageWindow, "Do not contact !", "Platinum Conserved Lead.", ShowMessageType.Exclamation);
                            //        }));
                            //    }
                            //}

                            ShowLeadApplicationScreen(Int64.Parse(((DataRecord)xdgSales.ActiveRecord).Cells["ImportID"].Value.ToString()));

                        }

                    }
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public  bool CheckLeadValidity(string importid)
        {
            string ReferenceNumber = "";
            try
            {
                StringBuilder strQuery = new StringBuilder();
                strQuery.Append("SELECT RefNo [Code] ");
                strQuery.Append("FROM INImport ");
                strQuery.Append($"WHERE ID = " + importid);

                DataTable dt = Methods.GetTableData(strQuery.ToString());

                ReferenceNumber = dt.Rows[0]["Code"].ToString();
            }
            catch { }

            string DCPower = "";
            try
            {
                StringBuilder strQueryDCPower = new StringBuilder();
                strQueryDCPower.Append("SELECT DebiCheckPower [Code] ");
                strQueryDCPower.Append("FROM DebiCheckConfiguration ");

                DataTable dt = Methods.GetTableData(strQueryDCPower.ToString());

                DCPower = dt.Rows[0]["Code"].ToString();
            }
            catch { }

            string BaseOrUpgrade = "";
            try
            {
                StringBuilder strQueryBaseOrUpg = new StringBuilder();
                strQueryBaseOrUpg.Append("SELECT CGS.FKlkpINCampaignGroupType [Code] FROM INImport AS I LEFT JOIN INCampaign AS C ON I.FKINCampaignID = C.ID LEFT JOIN INCampaignGroupSet AS CGS ON C.FKINCampaignGroupID = CGS.FKlkpINCampaignGroup WHERE I.ID = " + importid);

                DataTable dt = Methods.GetTableData(strQueryBaseOrUpg.ToString());

                BaseOrUpgrade = dt.Rows[0]["Code"].ToString();
            }
            catch { }

            

            if (BaseOrUpgrade == "2")
            {
                if (DCPower == "1")
                {
                    #region AuthToken

                    string auth_url = "http://plhqweb.platinumlife.co.za:999/Token";
                    string Username = "udm@platinumlife.co.za";
                    string Password = "P@ssword1";
                    string token = "";

                    using (var wb = new WebClient())
                    {
                        var data = new NameValueCollection();
                        data["username"] = Username;
                        data["password"] = Password;
                        data["grant_type"] = "password";

                        var response = wb.UploadValues(auth_url, "POST", data);
                        string responseInString = Encoding.UTF8.GetString(response);
                        var customObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseInString);
                        token = (string)customObject["access_token"];
                    }
                    #endregion

                    string ValidityStatus = "";
                    string submitRequest_urlLeadValidity = "http://plhqweb.platinumlife.co.za:999/api/UG/LeadValidity";
                    using (var wb = new WebClient())
                    {
                        var data = new NameValueCollection();
                        data["ReferenceNumber"] = ReferenceNumber;
                        wb.Headers.Add("Authorization", "Bearer " + token);

                        var response = wb.UploadValues(submitRequest_urlLeadValidity, "POST", data);
                        string responseInString = Encoding.UTF8.GetString(response);
                        var customObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseInString);

                        ValidityStatus = (string)customObject["ValidityStatus"];
                        //if (ReferenceNumber == "gdna91003681533")
                        //{
                        //    ValidityStatus = "Invalid";
                        //}
                    }

                    if (ValidityStatus != "Valid")
                    {
                        long InimportLong = long.Parse(importid);

                        INImport inimport = new INImport(InimportLong);
                        inimport.FKINLeadStatusID = 26;
                        inimport.Save(_validationResult);

                        LoadAgentDetails();

                        LoadSalesData();

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }



        }

        private void xdgSales_RecordFilterChanging(object sender, RecordFilterChangingEventArgs e)
        {
            IInputElement focusedElement = Keyboard.FocusedElement;

            XamComboEditor xce = Utilities.GetAncestorFromType((DependencyObject)focusedElement, typeof(XamComboEditor), true) as XamComboEditor;

            ScrollViewer sv = Utilities.GetDescendantFromType(xdgSales, typeof(ScrollViewer), true) as ScrollViewer;
            if (sv != null) sv.ScrollToVerticalOffset(0);

            DispatcherTimer timer1 = new DispatcherTimer();
            timer1.Interval = new TimeSpan(100);
            timer1.Tick += delegate
            {
                if (xce != null && !xce.IsInEditMode)
                {
                    xce.IsInEditMode = true;
                }

                timer1.Stop();
            };
            timer1.Start();
        }

        //private void xdgSales_RecordActivated(object sender, Infragistics.Windows.DataPresenter.Events.RecordActivatedEventArgs e)
        //{
        //    try
        //    {
        //        DataRecord record = (DataRecord)e.Record;

        //        if (record.RecordType == RecordType.DataRecord)
        //        {
        //            switch (record.NestingDepth)
        //            {
        //                case 0:
        //                    _strCampaign = record.Cells[1].Value.ToString().Trim();
        //                    break;

        //                case 2:
        //                    _strBatch = record.Cells[2].Value.ToString().Trim();
        //                    break;

        //                case 4:
        //                    //_strLead = record.Cells[3].Value.ToString().Trim();
        //                    _iLead = record.Index;
        //                    break;
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        //private void xdgSales_RecordExpanding(object sender, Infragistics.Windows.DataPresenter.Events.RecordExpandingEventArgs e)
        //{
        //    try
        //    {
        //        DataRecord record = (DataRecord)e.Record;

        //        if (record.RecordType == RecordType.DataRecord)
        //        {
        //            switch (record.NestingDepth)
        //            {
        //                case 0:
        //                    _strCampaign = record.Cells[1].Value.ToString().Trim();
        //                    break;

        //                case 2:
        //                    _strBatch = record.Cells[2].Value.ToString().Trim();
        //                    break;

        //                case 4:
        //                    //_strLead = record.Cells[3].Value.ToString().Trim();
        //                    _iLead = record.Index;
        //                    break;
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        private void btnStatusLoading_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
                //{
                //    SetAgentOffline();
                //    timer.Stop();
                //}

                StatusLoadingScreen StatusLoadingScreen = new StatusLoadingScreen();
                ShowDialog(StatusLoadingScreen, new INDialogWindow(StatusLoadingScreen));
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnLeadSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
                {
                    SetAgentOffline();
                    timer.Stop();
                }
                SearchLeadsScreen SearchLeadScreen = new SearchLeadsScreen(_ssGlobalData);
                OnClose(SearchLeadScreen);
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnSchedule_Click(object sender, RoutedEventArgs e)
        {
            _ssGlobalData.ScheduleScreen.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
            {
                _ssGlobalData.ScheduleScreen.WindowState = WindowState.Normal;
                _ssGlobalData.ScheduleScreen.Show();
                _ssGlobalData.ScheduleScreen.Activate();
            });
        }

        private void SalesScreen_OnLoaded(object sender, RoutedEventArgs e)
        {
            #region Get Message
            DataSet dsMessages = Methods.ExecuteStoredProcedure("sp_GetWellComeMessages", null);
            DataTable dtMessages = dsMessages.Tables[0];
            Random rnd = new Random();
            int messageId = rnd.Next(0, dsMessages.Tables[0].Rows.Count);
            string message = string.Empty;
            if (dtMessages.Rows[messageId]["TextMessage"] != null && dtMessages.Rows[messageId]["TextMessage"].ToString() != string.Empty && dtMessages.Rows.Count > 0)
            {
                message = dtMessages.Rows[messageId]["TextMessage"].ToString();
            }
            # endregion

            #region User Loggin information
            SqlParameter[] parameters =
                {
                    new SqlParameter("@userId", GlobalSettings.ApplicationUser.ID),
                };

            DataSet dsUsers = Methods.ExecuteStoredProcedure("sp_DetermineIfUserLoggedOn", parameters);

            int count = dsUsers.Tables[0].Rows.Count;
            string UserFirstName = string.Empty;
            string userId = GlobalSettings.ApplicationUser.ID.ToString();

            if (UserType.Value == lkpUserType.ConfirmationAgent)
            {
                btnSalesReport.Visibility = Visibility.Visible;
                btnConfirmationStats.Visibility = Visibility.Visible;
                btnCallMonitoringTrackingReport.Visibility = Visibility.Collapsed;
                btnBumpUpStatsReport.Visibility = Visibility.Visible;
                btnStatusLoading.Visibility = Visibility.Collapsed;
            }
            else if (UserType.Value == lkpUserType.CallMonitoringAgent || UserType.Value == lkpUserType.Preserver)
            {
                btnSalesReport.Visibility = Visibility.Collapsed;
                btnConfirmationStats.Visibility = Visibility.Collapsed;
                btnCallMonitoringTrackingReport.Visibility = Visibility.Visible;
                btnBumpUpStatsReport.Visibility = Visibility.Visible;
            }
            else if (UserType.Value == lkpUserType.DebiCheckAgent)
            {
                btnSalesReport.Visibility = Visibility.Collapsed;
                btnConfirmationStats.Visibility = Visibility.Collapsed;
                btnCallMonitoringTrackingReport.Visibility = Visibility.Collapsed;
                btnBumpUpStatsReport.Visibility = Visibility.Collapsed;
                btnSchedule.Visibility = Visibility.Collapsed;
                btnCaptureHours.Visibility = Visibility.Visible;
                btnStatusLoading.Visibility = Visibility.Collapsed;
                btnTransferSalesReport.Visibility = Visibility.Visible;
            }

            DataSet dsUserInfo = Methods.ExecuteStoredProcedure("sp_GetUserName", parameters);
            if (dsUserInfo.Tables[0].Rows.Count > 0)
            {
                UserFirstName = dsUserInfo.Tables[0].Rows[0]["FirstName"].ToString();
            }
            bool userLoggedOn = false;
            if (count > 0)
                userLoggedOn = true;

            # endregion

            if (userLoggedOn == false)
            {
                //ShowMessageBox(new InSureWelcomeMessage(), message, "Hi " + UserFirstName, ShowMessageType.Other);
                //Methods.ExecuteStoredProcedure("sp_UpdateLastLoggin", parameters);
                Methods.ExecuteSQLNonQuery("UPDATE [User] SET LastLoggedIn = GETDATE() Where ID = '" + userId + "'");
            }

            {
                // Create a thread
                Thread scheduleScreenThread = new Thread(() =>
                {
                    // Create and show the Window
                    ScheduleScreen scheduleScreen = new ScheduleScreen(_ssGlobalData);

                    //var width = scheduleScreen.Width;
                    //var height = scheduleScreen.Height;

                    //scheduleScreen.Width = 0;
                    //scheduleScreen.Height = 0;

                    scheduleScreen.Show();
                    scheduleScreen.Activate();
                    scheduleScreen.Visibility = Visibility.Hidden;

                    //scheduleScreen.Width = width;
                    //scheduleScreen.Height = height;

                    //var left = scheduleScreen.Left;
                    //var top = scheduleScreen.Top;

                    //scheduleScreen.Left = left - width / 2;
                    //scheduleScreen.Top = top - height / 2;

                    scheduleScreen.Closed += (sender2, e2) =>
                    {
                        scheduleScreen.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
                    };

                    // Start the Dispatcher Processing
                    Dispatcher.Run();
                });
                // Set the apartment state
                scheduleScreenThread.SetApartmentState(ApartmentState.STA);
                // Make the thread a background thread
                scheduleScreenThread.IsBackground = true;
                // Start the thread
                scheduleScreenThread.Start();

                //ScheduleScreen scheduleScreen = new ScheduleScreen();


                //scheduleScreen.Show();
                //scheduleScreen.Visibility = Visibility.Hidden;

                //scheduleScreen.Width = width;
                //scheduleScreen.Height = height;

                //var left = scheduleScreen.Left;
                //var top = scheduleScreen.Top;

                //scheduleScreen.Left = left - width / 2;
                //scheduleScreen.Top = top - height / 2;


            }
        }

        #region Call Monitoring Sales PopUps Settings
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Tick += saleTick_Tick;
            timer.Start();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void saleTick_Tick(object sender, EventArgs e)
        {
            try
            {
                timer.Stop();
                FKImportID = null;
                long? FKUserID = GlobalSettings.ApplicationUser.ID;
                DataTable dtStatus = Methods.GetTableData("SELECT Top 1 FKImportID, ID FROM INSalesToCallMonitoring WHERE FKUserID = " + FKUserID + " AND IsDisplayed = 0 and FKImportID is not null");
                FKImportID = long.Parse(dtStatus.Rows[0]["FKImportID"].ToString());
                long PopUpID = long.Parse(dtStatus.Rows[0]["ID"].ToString());

                DataTable dtReferenceNumber = Methods.GetTableData("SELECT Top 1 RefNo FROM INImport WHERE ID = " + FKImportID);
                CMToSReferenceNumber = dtReferenceNumber.Rows[0]["RefNo"].ToString();

                if (FKImportID == null)
                {

                }
                else
                {
                    try
                    {
                        string TSRName;
                        string Extension;
                        string Extension2;

                        SalesToCallMonitoring stc = new SalesToCallMonitoring(PopUpID);
                        stc.IsDisplayed = "1";
                        stc.Save(_validationResult);

                        if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
                        {
                            timer.Stop();
                            SetAgentOffline();
                        }

                        try
                        {

                            DataTable dtTSRName = Methods.GetTableData("SELECT [U].[FirstName] + ' ' + [U].[LastName] AS [RefNo]  FROM [Insure].[dbo].[User] AS [U] WHERE [ID] = (SELECT FKUserID FROM INImport WHERE ID = " + FKImportID + ")");
                            TSRName = dtTSRName.Rows[0]["RefNo"].ToString();
                            
                        }
                        catch
                        {
                            TSRName = "Sales Agent";
                        }

                        try
                        {
                            DataTable dtExtensions = Methods.GetTableData("SELECT [E].[Extension] FROM [Blush].[dbo].[lkpHRExtension] AS [E] WHERE [E].[ID] IN (SELECT [HE].[FKHRExtensionID] FROM [Blush].[dbo].[HRStaffExtension] AS [HE] WHERE [HE].[FKHRStaffID] = (SELECT  [H].[ID] FROM [Blush].[dbo].[HRStaff] AS [H] WHERE [FKUserID] = (SELECT [FKUserID] FROM [INImport] WHERE [ID] = " + FKImportID + ")))");
                            Extension = dtExtensions.Rows[0]["Extension"].ToString();
                            DataTable dtExtensions2 = Methods.GetTableData("SELECT [E].[Extension] FROM [Blush].[dbo].[lkpHRExtension] AS [E] WHERE [E].[ID] IN (SELECT [HE].[FKHRExtensionID] FROM [Blush].[dbo].[HRStaffSecondExtension] AS [HE] WHERE [HE].[FKHRStaffID] = (SELECT  [H].[ID] FROM [Blush].[dbo].[HRStaff] AS [H] WHERE [FKUserID] = (SELECT [FKUserID] FROM [INImport] WHERE [ID] = " + FKImportID + ")))");

                            try
                            {
                                Extension2 = dtExtensions.Rows[1]["Extension"].ToString();
                            }
                            catch
                            {
                                Extension2 = "";
                            }
                        }
                        catch
                        {
                            Extension = "";
                            Extension2 = ""; 
                        }

                        string IsFocused;
                        try
                        {
                            string CampaignID = Convert.ToString(Methods.GetTableData("Select [I].[FKINCampaignID] from [INImport] as [I] where [I].[ID] = " + FKImportID).Rows[0][0]);
                            IsFocused = Convert.ToString(Methods.GetTableData("Select top 1 IsActive from [INFocusCampaigns] where [INFocusCampaigns].[FKINCampaignID] = " + CampaignID).Rows[0][0]);
                        }
                        catch
                        {
                            IsFocused = "0";
                        }

                        if(IsFocused == "1")
                        {
                            ShowMessageBox(new Windows.INSalesToCallMonitoringWindowYellow(), CMToSReferenceNumber + " - " + TSRName + " ( " + Extension + " " + Extension2 + ")", "Incoming Sale to complete!", Embriant.Framework.ShowMessageType.Information);
                            LeadApplicationScreen las = new LeadApplicationScreen(FKImportID, _ssGlobalData);
                            ShowDialog(las, new INDialogWindow(las));
                        }
                        else
                        {
                            ShowMessageBox(new Windows.INSalesToCallMonitoringWindow(), CMToSReferenceNumber + " - " + TSRName + " ( " + Extension + " " + Extension2 + ")", "Incoming Sale to complete!", Embriant.Framework.ShowMessageType.Information);
                            LeadApplicationScreen las = new LeadApplicationScreen(FKImportID, _ssGlobalData);
                            ShowDialog(las, new INDialogWindow(las));
                        }




                    }
                    catch (Exception g)
                    {
                        timer.Interval = new TimeSpan(0, 0, 5);
                        timer.Start();
                        SetAgentOnline();
                    }
                }

                //timer.Interval = new TimeSpan(0, 0, 10);
                //timer.Start();
                //SetAgentOnline();
            }
            catch
            {
                timer.Interval = new TimeSpan(0, 0, 5);
                timer.Start();
                SetAgentOnline();
            }
        }

        public void SetAgentOnline()
        {
            try
            {
                DataTable dtAgentOnlineID = Methods.GetTableData("SELECT ID FROM INCMAgentsOnline WHERE FKUserID = " + GlobalSettings.ApplicationUser.ID);
                long AgentOnlineID = long.Parse(dtAgentOnlineID.Rows[0]["ID"].ToString());

                INCMAgentsOnline cmo = new INCMAgentsOnline(AgentOnlineID);
                cmo.Online = "1";
                cmo.Save(_validationResult);
            }
            catch
            {

            }

        }

        public void SetAgentOffline()
        {
            try
            {
                DataTable dtAgentOnlineID = Methods.GetTableData("SELECT ID FROM INCMAgentsOnline WHERE FKUserID = " + GlobalSettings.ApplicationUser.ID);
                long AgentOnlineID = long.Parse(dtAgentOnlineID.Rows[0]["ID"].ToString());

                INCMAgentsOnline cmo = new INCMAgentsOnline(AgentOnlineID);
                cmo.Online = "0";
                cmo.Save(_validationResult);
            }
            catch (Exception W)
            {

            }

        }
        #endregion

        private void btnCaptureHours_Click(object sender, RoutedEventArgs e)
        {
            //if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
            //{
            //    SetAgentOffline();
            //    timer.Stop();
            //}

            CaptureSalesAgentHoursScreen captureSalesAgentHoursScreen = new CaptureSalesAgentHoursScreen(0);
            ShowDialog(captureSalesAgentHoursScreen, new INDialogWindow(captureSalesAgentHoursScreen));
        }

        private void btnSalesReport_Click(object sender, RoutedEventArgs e)
        {
            if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
            {
                SetAgentOffline();
                timer.Stop();
            }

            ReportSalesScreen reportSalesScreen = new ReportSalesScreen();
            ShowDialog(reportSalesScreen, new INDialogWindow(reportSalesScreen));
        }

        private void btnConfirmationStats_Click(object sender, RoutedEventArgs e)
        {
            if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
            {
                SetAgentOffline();
                timer.Stop();
            }

            DataTable dtUserType = Methods.GetTableData("select FKUserType from dbo.[User] where ID = " + GlobalSettings.ApplicationUser.ID);
            long UserTypeID = long.Parse(dtUserType.Rows[0]["FKUserType"].ToString());
            ReportConfirmationStatsScreen reportConfirmationStats = new ReportConfirmationStatsScreen(UserTypeID);
            ShowDialog(reportConfirmationStats, new INDialogWindow(reportConfirmationStats));
        }

        private void btnCallMonitoringTrackingReport_Click(object sender, RoutedEventArgs e)
        {
            //if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
            //{
            //    SetAgentOffline();
            //    timer.Stop();
            //}

            DataTable dtUserType = Methods.GetTableData("select FKUserType from dbo.[User] where ID = " + GlobalSettings.ApplicationUser.ID);
            long UserTypeID = long.Parse(dtUserType.Rows[0]["FKUserType"].ToString());
            ReportCarriedForwardScreen reportCarriedForwardScreen = new ReportCarriedForwardScreen(UserTypeID);
            ShowDialog(reportCarriedForwardScreen, new INDialogWindow(reportCarriedForwardScreen));
        }

        private void btnBumpUpStatsReport_Click(object sender, RoutedEventArgs e)
        {
            //if (CMAgentListLong.Contains(GlobalSettings.ApplicationUser.ID))
            //{
            //    SetAgentOffline();
            //    timer.Stop();
            //}

            ReportBumpUpStatsScreen reportBumpUpStatsScreen = new ReportBumpUpStatsScreen(GlobalSettings.ApplicationUser.ID);
            ShowDialog(reportBumpUpStatsScreen, new INDialogWindow(reportBumpUpStatsScreen));
        }

        #endregion

        private void xdgSales_FieldLayoutInitialized(object sender, FieldLayoutInitializedEventArgs e)
        {
            //if (UserType == lkpUserType.ConfirmationAgent)
            //{
            //    foreach (Field field in xdgSales.FieldLayouts[2].Fields)
            //    {
            //        string fieldName = field.Name;
            //        if (fieldName == "CallBackDate")
            //        {

            //            xdgSales.FieldLayouts[2].Fields[fieldName].Settings.AllowEdit = true;
            //        }
            //        else
            //        {
            //            continue;
            //        }
            //    }
            //}


        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                xdgSales.Visibility = Visibility.Visible;
                xdgSalesDCDiary.Visibility = Visibility.Collapsed;
            }
            catch { }

            LoadAgentDetails();

            LoadSalesData();
        }

        private void xdgSales_EditModeStarted(object sender, EditModeStartedEventArgs e)
        {
            if (e.Cell.Field.Name == "CallBackDate")
            {
                XamDataGrid grid = sender as XamDataGrid;
                XamDateTimeEditor activeTextEditor = null;
                activeTextEditor = (XamDateTimeEditor)CellValuePresenter.FromCell(grid.ActiveCell).Editor;
                if (activeTextEditor.Value == null)
                {
                    activeTextEditor.Value = DateTime.Now;
                }

            }
        }

        private void xdgSales_RecordDeactivating(object sender, RecordDeactivatingEventArgs e)
        {
            try
            {
                if (UserType == lkpUserType.ConfirmationAgent)
                {


                    long _possibleBumpUpID;
                    DataRecord currentRecord = (DataRecord)xdgSales.ActiveRecord;

                    if (currentRecord.Cells.Count >= 13)
                    {

                        _possibleBumpUpID = Convert.ToInt64(currentRecord.Cells["PossibleBumpUpID"].Value);
                        PossibleBumpUpAllocation possibleBumpUpAllocation = new PossibleBumpUpAllocation(_possibleBumpUpID);

                        //Get lead status ID
                        //string strLeadStatus = currentRecord.Cells["LeadStatus"].Value as string;
                        //long? idLeadStatus = ((from r in _dtLeadStatus.AsEnumerable() where r.Field<string>("Description").Equals(strLeadStatus) select r.Field<long?>("ID"))).FirstOrDefault();
                        //possibleBumpUpAllocation.CallBackDate = DateTime.Parse(currentRecord.Cells["CallBackDate"].Value.ToString());
                        DateTime? callBackDate = currentRecord.Cells["CallBackDate"].Value.ToString() != "" ? Convert.ToDateTime(currentRecord.Cells["CallBackDate"].Value.ToString()) : (DateTime?)null;
                        possibleBumpUpAllocation.CallBackDate = callBackDate;
                        possibleBumpUpAllocation.Save(_validationResult);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void xdgSales_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            XamDataGrid grid = sender as XamDataGrid;
            XamDateTimeEditor activeTextEditor = null;
            //int inputLength = 0;

            if (grid != null)
            {
                if (grid.ActiveCell != null)
                {
                    #region Get active texteditor
                    switch (grid.ActiveCell.Field.Name)
                    {
                        case "CallBackDate":
                            //activeTextEditor = Utilities.GetDescendantFromType(CellValuePresenter.FromCell(grid.ActiveRecord.DataPresenter.ActiveCell), typeof(XamTextEditor), true) as XamTextEditor;
                            activeTextEditor = (XamDateTimeEditor)CellValuePresenter.FromCell(grid.ActiveCell).Editor;//Utilities.GetDescendantFromType(CellValuePresenter.FromCell(grid.ActiveRecord.DataPresenter.ActiveCell), typeof(XamTextEditor), true) as XamTextEditor;

                            //inputLength = NoNull(grid.ActiveCell.Value.ToString(), String.Empty).ToString().Length;
                            break;
                    }
                    #endregion

                    #region Clear the contents of the text field
                    //I disabled this feature because it made it difficult to edit the date time string
                    //if (e.Key == Key.Back)
                    //{// && (string)activeTextEditor.Value != "Accepted" && (string)activeTextEditor.Value != "Carried Forward"
                    //    if (activeTextEditor != null)
                    //    {
                    //        activeTextEditor.Value = null;
                    //    }
                    //}
                    #endregion

                    #region Save active record

                    if (e.Key == Key.Enter)
                    {
                        if (grid != null && grid.ActiveRecord != null)
                        {
                            if (grid.ActiveCell != null)
                            {
                                switch (grid.ActiveCell.Field.Name)
                                {
                                    case "CallBackDate":
                                        activeTextEditor = (XamDateTimeEditor)CellValuePresenter.FromCell(grid.ActiveCell).Editor;//Utilities.GetDescendantFromType(CellValuePresenter.FromCell(grid.ActiveRecord.DataPresenter.ActiveCell), typeof(XamTextEditor), true) as XamTextEditor;
                                        if (activeTextEditor != null) activeTextEditor.EndEditMode(true, true);
                                        break;
                                }
                            }

                            xdgSales_RecordDeactivating(null, null);
                        }
                    }

                    #endregion
                }
                else
                {
                    return;
                }
            }

        }

        private void xdgSales_RecordExpanded(object sender, RecordExpandedEventArgs e)
        {
            if (UserType == lkpUserType.CallMonitoringAgent || UserType == lkpUserType.Preserver)
            {
                if (e.Record != null)
                {
                    switch (e.Record.FieldLayout.Description)
                    {
                        case "Campaign":
                            break;
                        case "Batch":
                            foreach (Record record in (e.Record as DataRecord).ChildRecords[0].ChildRecords)
                            {
                                DataRecord dr = (DataRecord)record;
                                if (dr.Cells["IsOverAssessment"].Value as int? == 1)
                                {
                                    if (record != null)
                                    {
                                        record.Tag = Brushes.Yellow;
                                    }

                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (UserType == lkpUserType.ConfirmationAgent)
            {
                if (e.Record != null)
                {
                    switch (e.Record.FieldLayout.Description)
                    {
                        case "Campaign":
                            break;
                        case "Batch":
                            foreach (Record record in (e.Record as DataRecord).ChildRecords[0].ChildRecords)
                            {
                                DataRecord dr = (DataRecord)record;
                                if (dr.Cells["LeadStatusID"].Value as long? == (long)lkpINLeadStatus.CallMonitoringCarriedForward)
                                {
                                    if (record != null)
                                    {
                                        record.Tag = Brushes.Orange;
                                    }

                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void btnCallMonQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportCallMonitoringScreen reportCallMonitoringScreen = new ReportCallMonitoringScreen();
                ShowDialog(reportCallMonitoringScreen, new INDialogWindow(reportCallMonitoringScreen));
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnCallMonTracking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportCarriedForwardScreen reportCarriedForwardScreen = new ReportCarriedForwardScreen(1);
                ShowDialog(reportCarriedForwardScreen, new INDialogWindow(reportCarriedForwardScreen));
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnMySuccess_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MySuccess mySuccess = new MySuccess();
                ShowDialog(mySuccess, new INDialogWindow(mySuccess));
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnTransferSalesReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportDCTransferSales reportDCTransferSales = new ReportDCTransferSales();
                ShowDialog(reportDCTransferSales, new INDialogWindow(reportDCTransferSales));
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


        private void XamCheckEditor_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            string LeadStatus;
            long? DiaryReason;


            LaData.AppData.ImportID = Int64.Parse(((DataRecord)xdgSales.ActiveRecord).Cells["ImportID"].Value.ToString());

            LaData.AppData.DiaryReasonID = 1;

            LaData.AppData.LeadStatus = 9;


            INImport import = new INImport((long)LaData.AppData.ImportID);

            import.FKINLeadStatusID = LaData.AppData.LeadStatus;

            import.FKINDiaryReasonID = LaData.AppData.DiaryReasonID;

            import.Save(_validationResult);

            //MessageBox.Show("The Diary Reason and Lead Status have been updated.");

            LoadSalesCallBack();



        }

        private void btnDCSpecialistDiaries_Click(object sender, RoutedEventArgs e)
        {

            try 
            {
                #region Campaign


                try
                {
                    xdgSales.Visibility = Visibility.Collapsed;
                    xdgSalesDCDiary.Visibility = Visibility.Visible;
                }
                catch { }

                FieldLayout flCampaign = new FieldLayout();
                flCampaign.Key = "Campaign";

                //Field fieldCampaignID = new Field("CampaignID");
                //fieldCampaignID.Visibility = Visibility.Collapsed;

                Field fieldCampaignName = new Field("CampaignName");
                fieldCampaignName.Visibility = Visibility.Visible;
                fieldCampaignName.Width = new FieldLength(300);
                fieldCampaignName.Label = "Campaign";

                Field fieldCampaignCode = new Field("CampaignCode");
                fieldCampaignCode.Visibility = Visibility.Collapsed;

                Field fieldCampaignGroupType = new Field("CampaignGroupType");
                fieldCampaignGroupType.Visibility = Visibility.Collapsed;

                //Field fieldCampaignGroup = new Field("CampaignGroup");
                //fieldCampaignGroup.Visibility = Visibility.Collapsed;

                //flCampaign.Fields.Add(fieldCampaignID);

                flCampaign.Fields.Add(fieldCampaignName);

                flCampaign.Fields.Add(fieldCampaignCode);

                flCampaign.Fields.Add(fieldCampaignGroupType);

                //flCampaign.Fields.Add(fieldCampaignGroup);

                xdgSalesDCDiary.FieldLayouts.Add(flCampaign);

                #endregion Campaign

                #region Batch(DateOfSale)
                FieldLayout flBatch = new FieldLayout();
                flBatch.Key = "Batch";
                flBatch.ParentFieldLayoutKey = "Campaign";

                Field fieldBatchDateOfSale = new Field("DateOfSale");
                fieldBatchDateOfSale.Visibility = Visibility.Visible;
                fieldBatchDateOfSale.Width = new FieldLength(200);
                fieldBatchDateOfSale.Settings.LabelTextWrapping = TextWrapping.Wrap;
                fieldBatchDateOfSale.Label = "Date Of Diary";

                Field fieldBatchCampaignGroupType = new Field("CampaignGroupType");
                fieldBatchCampaignGroupType.Visibility = Visibility.Collapsed;


                //Field fieldBatchID = new Field("BatchID");
                //fieldBatchID.Visibility = Visibility.Collapsed;

                //Field fieldBatchCampaignID = new Field("CampaignID");
                //fieldBatchCampaignID.Visibility = Visibility.Collapsed;

                //Field fieldCampaignGroupID = new Field("CampaignGroupID");
                //fieldCampaignGroupID.Visibility = Visibility.Collapsed;

                //Field fieldLeadBookID = new Field("LeadBookID");
                //fieldLeadBookID.Visibility = Visibility.Collapsed;

                //Field fieldBatchCode = new Field("BatchCode");
                //fieldBatchCode.Visibility = Visibility.Visible;
                //fieldBatchCode.Width = new FieldLength(160);
                //fieldBatchCode.Label = "Batch";

                //Field fieldAllocationDate = new Field("AllocationDate");
                //fieldAllocationDate.Visibility = Visibility.Visible;
                //fieldAllocationDate.Width = new FieldLength(150);
                //fieldAllocationDate.Label = "Allocation Date";

                //Field fieldExpireDate = new Field("ExpireDate");
                //fieldExpireDate.Visibility = Visibility.Visible;
                //fieldExpireDate.Width = new FieldLength(150);
                //fieldExpireDate.Label = "Expire Date";

                //flBatch.Fields.Add(fieldBatchID);

                //flBatch.Fields.Add(fieldBatchCampaignID);

                //flBatch.Fields.Add(fieldCampaignGroupID);

                //flBatch.Fields.Add(fieldLeadBookID);

                //flBatch.Fields.Add(fieldBatchCode);

                //flBatch.Fields.Add(fieldAllocationDate);

                //flBatch.Fields.Add(fieldExpireDate);

                flBatch.Fields.Add(fieldBatchDateOfSale);

                flBatch.Fields.Add(fieldBatchCampaignGroupType);

                xdgSalesDCDiary.FieldLayouts.Add(flBatch);
                #endregion Batch

                #region Lead
                FieldLayout flLead = new FieldLayout();
                flLead.Key = "Lead";
                flLead.ParentFieldLayoutKey = "Batch";

                Field fieldLeadDateOfSale = new Field("DateOfSale");
                fieldLeadDateOfSale.Visibility = Visibility.Visible;
                fieldLeadDateOfSale.Width = new FieldLength(110);
                fieldLeadDateOfSale.Settings.LabelTextWrapping = TextWrapping.Wrap;
                fieldLeadDateOfSale.Label = "Call Back Date";

                Field fieldActualDateOfSale = new Field("ActualDateOfSale");
                fieldActualDateOfSale.Visibility = Visibility.Collapsed;
                fieldActualDateOfSale.Width = new FieldLength(110);
                fieldActualDateOfSale.Settings.LabelTextWrapping = TextWrapping.Wrap;
                fieldActualDateOfSale.Label = "Call Back Date";

                Field fieldAllocationDateTime = new Field("AllocationDateTime");
                //fieldAllocationDateTime.Settings.EditorType = typeof(XamDateTimeEditor);
                //fieldAllocationDateTime.Settings.EditAsType = typeof(DateTime);
                //fieldAllocationDateTime.Settings.EditorStyle = styleAllocation;
                fieldAllocationDateTime.Visibility = Visibility.Collapsed;
                fieldAllocationDateTime.Width = new FieldLength(180);
                fieldAllocationDateTime.Settings.LabelTextWrapping = TextWrapping.Wrap;
                fieldAllocationDateTime.Label = "Date and Time of Allocation";

                Field fieldExpiryDate = new Field("ExpiryDate");
                fieldExpiryDate.Visibility = Visibility.Collapsed;
                fieldExpiryDate.Width = new FieldLength(110);
                fieldExpiryDate.Settings.LabelTextWrapping = TextWrapping.Wrap;
                fieldExpiryDate.Label = "Call Back Date";

                Field fieldIsTSRBUSavedCarriedForward = new Field("IsTSRBUSavedCarriedForward");
                fieldIsTSRBUSavedCarriedForward.Visibility = Visibility.Visible;
                fieldIsTSRBUSavedCarriedForward.Width = new FieldLength(80);
                fieldIsTSRBUSavedCarriedForward.Settings.LabelTextWrapping = TextWrapping.Wrap;
                fieldIsTSRBUSavedCarriedForward.Label = "Call Back Time";

                Field fieldRefNo = new Field("RefNo");
                fieldRefNo.Visibility = Visibility.Visible;
                fieldRefNo.Width = new FieldLength(165);
                fieldRefNo.Label = "Reference Number";

                Field fieldTSR = new Field("TSR");
                fieldTSR.Visibility = Visibility.Visible;
                fieldTSR.Width = new FieldLength(250);
                fieldTSR.Settings.LabelTextWrapping = TextWrapping.Wrap;
                fieldTSR.Label = "TSR Name";

                Field fieldLeadStatus = new Field("LeadStatus");
                fieldLeadStatus.Visibility = Visibility.Visible;
                fieldLeadStatus.Width = new FieldLength(275);
                fieldLeadStatus.Label = "Lead Status";

                Field fieldCallMonitoringStatus = new Field("CallMonitoringStatus");
                fieldCallMonitoringStatus.Visibility = Visibility.Collapsed;
                fieldCallMonitoringStatus.Width = new FieldLength(104);
                fieldCallMonitoringStatus.Settings.LabelTextWrapping = TextWrapping.Wrap;
                fieldCallMonitoringStatus.Label = "Call Monitoring Status";

                Field fieldIsBumpedUp = new Field("IsBumpedUp");
                fieldIsBumpedUp.Visibility = Visibility.Collapsed;
                fieldIsBumpedUp.Width = new FieldLength(84);
                fieldIsBumpedUp.Settings.LabelTextWrapping = TextWrapping.Wrap;
                fieldIsBumpedUp.Label = "Bumped Up";

                Field fieldLeadCampaignGroupType = new Field("CampaignGroupType");
                fieldLeadCampaignGroupType.Visibility = Visibility.Collapsed;

                Field fieldIsOverAssessment = new Field("IsOverAssessment");
                fieldIsOverAssessment.Visibility = Visibility.Collapsed;
                //fieldIsOverAssessment.Width = new FieldLength(110);
                //fieldIsOverAssessment.Settings.LabelTextWrapping = TextWrapping.Wrap;
                //fieldIsOverAssessment.Label = "Is Over Assessment";

                Field fieldImportID = new Field("ImportID");
                fieldImportID.Visibility = Visibility.Collapsed;

                flLead.Fields.Add(fieldLeadDateOfSale);

                flLead.Fields.Add(fieldActualDateOfSale);

                flLead.Fields.Add(fieldAllocationDateTime);

                flLead.Fields.Add(fieldIsTSRBUSavedCarriedForward);

                flLead.Fields.Add(fieldRefNo);

                flLead.Fields.Add(fieldTSR);

                flLead.Fields.Add(fieldLeadStatus);

                flLead.Fields.Add(fieldCallMonitoringStatus);

                flLead.Fields.Add(fieldIsBumpedUp);

                flLead.Fields.Add(fieldExpiryDate);

                flLead.Fields.Add(fieldLeadCampaignGroupType);

                flLead.Fields.Add(fieldIsOverAssessment);

                flLead.Fields.Add(fieldImportID);


                xdgSalesDCDiary.FieldLayouts.Add(flLead);


                #endregion Lead
            }
            catch
            {

            }

            try
            {
                DataSet ds = Insure.INGetDiariesAssignedToDCAgent(_agentID);

                DataRelation relCampaignBatch = new DataRelation("CampaignBatch", ds.Tables[0].Columns["CampaignGroupType"], ds.Tables[1].Columns["CampaignGroupType"]);
                ds.Relations.Add(relCampaignBatch);

                DataColumn[] parentColumns = new DataColumn[] { ds.Tables[1].Columns["DateOfSale"], ds.Tables[1].Columns["CampaignGroupType"] };
                DataColumn[] childColumns = new DataColumn[] { ds.Tables[2].Columns["DateOfSale"], ds.Tables[2].Columns["CampaignGroupType"] };

                DataRelation relBatchAgent = new DataRelation("BatchLead", parentColumns, childColumns);
                ds.Relations.Add(relBatchAgent);

                xdgSalesDCDiary.DataSource = ds.Tables[0].DefaultView;
            }
            catch
            {

            }
        }
    }

}
