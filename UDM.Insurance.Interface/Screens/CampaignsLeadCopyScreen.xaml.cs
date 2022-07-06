using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Embriant.Framework;
using Embriant.Framework.Data;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.Events;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    public partial class CampaignsLeadCopyScreen : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion

        #region Classes

        public class SourceLead : ObservableObject
        {
            bool _isChecked;
            public bool IsChecked
            {
                get { return _isChecked; }
                set { SetProperty(ref _isChecked, value, () => IsChecked); }
            }

            bool _isFilteredOut;
            public bool IsFilteredOut
            {
                get { return _isFilteredOut; }
                set { SetProperty(ref _isFilteredOut, value, () => IsFilteredOut); }
            }

            private long? _importID;
            public long? ImportID
            {
                get { return _importID; }
                set { SetProperty(ref _importID, value, () => ImportID); }
            }

            private string _canceroption;
            public string CancerOption
            {
                get { return _canceroption; }
                set { SetProperty(ref _canceroption, value, () => CancerOption); }
            }

            private string _refNumber;
            public string RefNumber
            {
                get { return _refNumber; }
                set { SetProperty(ref _refNumber, value, () => RefNumber); }
            }

            private string _leadStatus;
            public string LeadStatus
            {
                get { return _leadStatus; }
                set { SetProperty(ref _leadStatus, value, () => LeadStatus); }
            }

            private long? _fkINDeclineReasonID;

            public long? FKINDeclineReasonID
            {
                get
                {
                    return _fkINDeclineReasonID;
                }
                set
                {
                    SetProperty(ref _fkINDeclineReasonID, value, () => FKINDeclineReasonID);
                }
            }

            private string _declineReason;
            public string DeclineReason
            {
                get { return _declineReason; }
                set { SetProperty(ref _declineReason, value, () => DeclineReason); }
            }

            private long? _fkINImportLatentLeadReasonID;
            public long? FKINImportLatentLeadReasonID
            {
                get
                {
                    return _fkINImportLatentLeadReasonID;
                }
                set
                {
                    SetProperty(ref _fkINImportLatentLeadReasonID, value, () => FKINImportLatentLeadReasonID);
                }
            }
        }

        public class DestinationLead : ObservableObject
        {
            private long? _importID;
            public long? ImportID
            {
                get { return _importID; }
                set { SetProperty(ref _importID, value, () => ImportID); }
            }

            private string _refNumber;
            public string RefNumber
            {
                get { return _refNumber; }
                set { SetProperty(ref _refNumber, value, () => RefNumber); }
            }
        }

        public class Leads : ObservableObject
        {
            public Leads(ObservableCollection<SourceLead> sourceLeads, ObservableCollection<DestinationLead> destinationLeads)
            {
                SourceLeads = sourceLeads;
                DestinationLeads = destinationLeads;

                foreach (SourceLead lead in SourceLeads)
                {
                    lead.PropertyChanged += LeadOnPropertyChanged;
                }

                AllLeadsAreChecked = false;
            }

            private void LeadOnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e != null && sender != null)
                {
                    var sourceLead = (SourceLead)sender;

                    switch (e.PropertyName)
                    {
                        case "IsChecked":
                            if (sourceLead.IsChecked)
                            {
                                TotalChecked++;
                            }
                            else
                            {
                                TotalChecked--;
                            }

                            if (TotalChecked == 0)
                            {
                                AllLeadsAreChecked = false;
                            }
                            else if (TotalChecked == SourceLeads.Count || TotalChecked == TotalFilteredInLeads)
                            {
                                AllLeadsAreChecked = true;
                            }
                            else
                            {
                                AllLeadsAreChecked = null;
                            }

                            TotalLeadsAvailable = TotalFilteredInLeads - TotalChecked;
                            break;

                        case "IsFilteredOut":
                            if (sourceLead.IsFilteredOut)
                            {
                                sourceLead.IsChecked = false;
                            }
                            break;
                    }
                }
            }

            private ObservableCollection<SourceLead> _sourceLeads;
            public ObservableCollection<SourceLead> SourceLeads
            {
                get { return _sourceLeads; }
                set { SetProperty(ref _sourceLeads, value, () => SourceLeads); }
                //private set
                //{
                //    _sourceLeads = value;
                //    OnPropertyChanged("SourceLeads");
                //}
            }

            private ObservableCollection<DestinationLead> _destinationLeads;
            public ObservableCollection<DestinationLead> DestinationLeads
            {
                get { return _destinationLeads; }
                set { SetProperty(ref _destinationLeads, value, () => DestinationLeads); }
                //private set
                //{
                //    _destinationLeads = value;
                //    OnPropertyChanged("DestinationLeads");
                //}
            }

            private bool? _allLeadsAreChecked;
            public bool? AllLeadsAreChecked
            {
                get { return _allLeadsAreChecked; }
                set
                {
                    _allLeadsAreChecked = value;

                    if (value == null)
                    {
                        //SetProperty(ref _allLeadsAreChecked, value, () => AllLeadsAreChecked);
                        OnPropertyChanged("AllLeadsAreChecked");
                        return;
                    }

                    foreach (SourceLead member in SourceLeads)
                    {
                        if (!member.IsFilteredOut)
                            member.IsChecked = value.Value;
                    }

                    //SetProperty(ref _allLeadsAreChecked, value, () => AllLeadsAreChecked);
                    OnPropertyChanged("AllLeadsAreChecked");
                }
            }

            int _totalFilteredInLeads;
            public int TotalFilteredInLeads
            {
                get { return _totalFilteredInLeads; }
                set { SetProperty(ref _totalFilteredInLeads, value, () => TotalFilteredInLeads); }
                //set
                //{
                //    _totalFilteredInLeads = value;
                //    OnPropertyChanged("TotalFilteredInLeads");
                //}
            }

            int _totalLeadsAvailable;
            public int TotalLeadsAvailable
            {
                get { return _totalLeadsAvailable; }
                set { SetProperty(ref _totalLeadsAvailable, value, () => TotalLeadsAvailable); }
                //set
                //{
                //    _totalLeadsAvailable = value;
                //    OnPropertyChanged("TotalLeadsAvailable");
                //}
            }

            int _totalChecked;
            public int TotalChecked
            {
                get { return _totalChecked; }
                set { SetProperty(ref _totalChecked, value, () => TotalChecked); }
                //set
                //{
                //    _totalChecked = value;
                //    OnPropertyChanged("TotalChecked");
                //}
            }

            int _totalLeadsCopied;
            public int TotalLeadsCopied
            {
                get { return _totalLeadsCopied; }
                set { SetProperty(ref _totalLeadsCopied, value, () => TotalLeadsCopied); }
                //set
                //{
                //    _totalLeadsCopied = value;
                //    OnPropertyChanged("TotalLeadsCopied");
                //}
            }

            //#region INotifyPropertyChanged Members

            //public event PropertyChangedEventHandler PropertyChanged;

            //void OnPropertyChanged(string prop)
            //{
            //    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
            //}

            //#endregion
        }

        public class ScreenData : ObservableObject
        {
            Leads _leads;
            public Leads Leads
            {
                get { return _leads; }
                set { SetProperty(ref _leads, value, () => Leads); }
            }

            bool _isCopying;
            public bool IsCopying
            {
                get { return _isCopying; }
                set { SetProperty(ref _isCopying, value, () => IsCopying); }
            }

            long? _sourceCampaignID;
            public long? SourceCampaignID
            {
                get { return _sourceCampaignID; }
                set { SetProperty(ref _sourceCampaignID, value, () => SourceCampaignID); }
            }

            long? _destinationCampaignID;
            public long? DestinationCampaignID
            {
                get { return _destinationCampaignID; }
                set { SetProperty(ref _destinationCampaignID, value, () => DestinationCampaignID); }
            }

            long? _sourceBatchID;
            public long? SourceBatchID
            {
                get { return _sourceBatchID; }
                set { SetProperty(ref _sourceBatchID, value, () => SourceBatchID); }
            }

            long? _destinationBatchID;
            public long? DestinationBatchID
            {
                get { return _destinationBatchID; }
                set { SetProperty(ref _destinationBatchID, value, () => DestinationBatchID); }
            }

            string _destinationBatchCode;
            public string DestinationBatchCode
            {
                get { return _destinationBatchCode; }
                set { SetProperty(ref _destinationBatchCode, value, () => DestinationBatchCode); }
            }
        }

        #endregion

        #region Properties

        private ScreenData _scrData = new ScreenData();
        public ScreenData ScrData
        {
            get { return _scrData; }
            set
            {
                _scrData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ScrData"));
            }
        }

        #endregion

        #region Members
        
        private Collection<SourceLead> _leadsToCopy;
        private Collection<SourceLead> _leadsToCopyFinal;
        private Collection<SourceLead> _leadsToCopyDuplicates;
        private BackgroundWorker _worker;

        #endregion
    
        #region Constructors

        public CampaignsLeadCopyScreen()
        {
            InitializeComponent();
            LoadLookups();

            ScrData.Leads = new Leads(new ObservableCollection<SourceLead>(), new ObservableCollection<DestinationLead>());
        }

        #endregion Constructors
        
        #region Methods

        private void ResetScreen()
        {
            ScrData = new ScreenData();
            ScrData.Leads = new Leads(new ObservableCollection<SourceLead>(), new ObservableCollection<DestinationLead>());

            cmbCampaigns.Focus();
            xdgSourceLeads.FieldLayouts[0].RecordFilters.Clear();
            cbAllowRecordFiltering.IsChecked = false;
        }

        private void LoadLookups()
        {
            DataSet dsLoadLookups = Insure.INGetRenewalLeadCopyScreenLookups();
            cmbCampaigns.Populate(dsLoadLookups.Tables[0], "CampaignName", "CampaignID");
            cmbDestinationCampaigns.Populate(dsLoadLookups.Tables[1], "CampaignName", "CampaignID");
        }
        
        private void LoadBatches()
        {
            try
            {
                if (ScrData.SourceCampaignID != null)
                {
                    CommonControlData.PopulateBatchComboBox(cmbBatch, ScrData.SourceCampaignID);
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        
        private void LoadSourceLeads(long? campaignID, long? batchId)
        {
            try
            {
                SetCursor(Cursors.Wait);

                ScrData.Leads = new Leads(new ObservableCollection<SourceLead>(), new ObservableCollection<DestinationLead>());

                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CampaignID", campaignID);
                parameters[1] = new SqlParameter("@BatchID", batchId);
                DataTable dtSourceLeads = Methods.ExecuteStoredProcedure("_spGetLeadsToCopy", parameters).Tables[0];

                if (dtSourceLeads.Rows.Count > 0)
                {
                    ObservableCollection<SourceLead> sourceLeads = new ObservableCollection<SourceLead>();
                    ObservableCollection<DestinationLead> destinationLeads = new ObservableCollection<DestinationLead>();

                    foreach (DataRow row in dtSourceLeads.AsEnumerable())
                    {
                        sourceLeads.Add(new SourceLead
                        {
                            ImportID = row["ImportID"] as long?,
                            RefNumber = row["RefNo"] as string,
                            LeadStatus = row["LeadStatus"] as string,
                            FKINDeclineReasonID = row["FKINDeclineReasonID"] as long ?,
                            DeclineReason = row["DeclineReason"] as string,
                            FKINImportLatentLeadReasonID = row["FKINImportLatentLeadReasonID"] as long?,
                            CancerOption = row["CancerOption"] as string
                        });
                    }

                    ScrData.Leads = new Leads(sourceLeads, destinationLeads);
                    cbAllowRecordFiltering.IsChecked = true;
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

        private void CopyLeadsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                if (ScrData != null && ScrData.Leads != null)
                {
                    bool test1 = ScrData.Leads.TotalChecked > 0;

                    bool test2 = cmbDestinationCampaigns.SelectedValue != null;

                    bool test3 = ScrData.IsCopying;

                    e.CanExecute = (test1 && test2) || test3;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void CopyLeadsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ScrData.IsCopying)
            {
                switch (atCopy.Text)
                {
                    case "_Cancel":
                        _worker.CancelAsync();
                        cbAllowRecordFiltering.IsChecked = true;
                        ScrData.IsCopying = false;
                        atCopy.Text = "_Copy";
                        break;

                    case "_Continue":
                        ScrData.IsCopying = false;
                        atCopy.Text = "_Copy";
                        ResetScreen();
                        break;
                }
            }
            else
            {
                ScrData.Leads.TotalLeadsCopied = 0;
                cbAllowRecordFiltering.IsChecked = false;
                ScrData.IsCopying = true;
                atCopy.Text = "_Cancel";
                CopyLeads();
            }
        }

        private void DuplicateFinder()
        {
            DataTable dtUpgradeCampaigns = Methods.GetTableData("SELECT C.ID AS CampaignID FROM INCampaign AS C " +
                                                                            "LEFT JOIN INCampaignGroupSet AS CGS ON C.FKINCampaignGroupID = CGS.FKlkpINCampaignGroup " +
                                                                            "WHERE CGS.FKlkpINCampaignGroupType = 2");

            string strQuery;
            int dupCheckMonth = -4;
            _leadsToCopyFinal = new Collection<SourceLead>();
            _leadsToCopyDuplicates = new Collection<SourceLead>();

            strQuery = "SELECT DISTINCT RefNo FROM INImport ";
            strQuery += "JOIN INLead ON INImport.FKINLeadID = INLead.ID ";
            strQuery += "JOIN INCampaign ON INImport.FKINCampaignID = INCampaign.ID ";
            strQuery += "JOIN lkpINCampaignType ON INCampaign.FKINCampaignTypeID = lkpINCampaignType.ID ";
            strQuery += "JOIN lkpINCampaignGroup ON INCampaign.FKINCampaignGroupID = lkpINCampaignGroup.ID WHERE ";

            long campaignID = Convert.ToInt64(ScrData.DestinationCampaignID);
            INCampaign inCampaign = new INCampaign(campaignID);

            #region build query - campaign type and campaign group
            {
                IEnumerable<lkpINCampaignType> campaignTypes;
                IEnumerable<lkpINCampaignGroup> campaignGroups;

                if (inCampaign.FKINCampaignTypeID != null && inCampaign.FKINCampaignGroupID != null)
                {
                    lkpINCampaignType campaignType = (lkpINCampaignType)inCampaign.FKINCampaignTypeID;
                    lkpINCampaignGroup campaignGroup = (lkpINCampaignGroup)inCampaign.FKINCampaignGroupID;

                    while (true)
                    {
                        //Macc
                        campaignTypes = new[]
                        {
                                lkpINCampaignType.Macc, lkpINCampaignType.MaccMillion, lkpINCampaignType.BlackMaccMillion, lkpINCampaignType.AccDis, lkpINCampaignType.MaccFuneral,
                                lkpINCampaignType.BlackMacc, lkpINCampaignType.FemaleDis, lkpINCampaignType.IGFemaleDisability
                        };
                        campaignGroups = new[]
                        {
                                lkpINCampaignGroup.Base, lkpINCampaignGroup.Renewals, lkpINCampaignGroup.Starter, lkpINCampaignGroup.Defrosted,
                                lkpINCampaignGroup.Rejuvenation, lkpINCampaignGroup.Reactivation, lkpINCampaignGroup.Extension, lkpINCampaignGroup.ReDefrost,
                                lkpINCampaignGroup.Resurrection
                        };
                        if (campaignTypes.Contains(campaignType) && campaignGroups.Contains(campaignGroup))
                        {
                            strQuery += "lkpINCampaignType.ID IN (" + string.Join(",", campaignTypes.Cast<int>().ToArray()) + ") AND ";
                            strQuery += "lkpINCampaignGroup.ID IN (" + string.Join(",", campaignGroups.Cast<int>().ToArray()) + ") AND ";
                            break;
                        }

                        //Cancer 
                        campaignTypes = new[]
                        {
                                lkpINCampaignType.Cancer, lkpINCampaignType.CancerFuneral, lkpINCampaignType.CancerFuneral99,  lkpINCampaignType.IGCancer, lkpINCampaignType.TermCancer,
                                lkpINCampaignType.MaccCancer, lkpINCampaignType.MaccMillionCancer
                        };
                        campaignGroups = new[]
                        {
                                lkpINCampaignGroup.Base, lkpINCampaignGroup.Renewals, lkpINCampaignGroup.Starter, lkpINCampaignGroup.Defrosted,
                                lkpINCampaignGroup.Rejuvenation, lkpINCampaignGroup.Reactivation, lkpINCampaignGroup.Extension, lkpINCampaignGroup.ReDefrost,
                                lkpINCampaignGroup.Resurrection
                        };
                        if (campaignTypes.Contains(campaignType) && campaignGroups.Contains(campaignGroup))
                        {
                            strQuery += "lkpINCampaignType.ID IN (" + string.Join(",", campaignTypes.Cast<int>().ToArray()) + ") AND ";
                            strQuery += "lkpINCampaignGroup.ID IN (" + string.Join(",", campaignGroups.Cast<int>().ToArray()) + ") AND ";
                            break;
                        }

                        //Macc upgrades
                        campaignTypes = new[]
                        {
                                lkpINCampaignType.Macc, lkpINCampaignType.MaccMillion, lkpINCampaignType.BlackMaccMillion, lkpINCampaignType.AccDis, lkpINCampaignType.MaccFuneral,
                                lkpINCampaignType.BlackMacc, lkpINCampaignType.FemaleDis, lkpINCampaignType.IGFemaleDisability
                            };
                        campaignGroups = new[]
                        {
                                lkpINCampaignGroup.Upgrade, lkpINCampaignGroup.Upgrade1, lkpINCampaignGroup.Upgrade2, lkpINCampaignGroup.Upgrade3, lkpINCampaignGroup.Upgrade4,
                                lkpINCampaignGroup.Upgrade5, lkpINCampaignGroup.Upgrade6, lkpINCampaignGroup.Upgrade7, lkpINCampaignGroup.Upgrade8, lkpINCampaignGroup.Upgrade9,
                                lkpINCampaignGroup.Upgrade10, lkpINCampaignGroup.Upgrade11, lkpINCampaignGroup.Upgrade12, lkpINCampaignGroup.Upgrade13, lkpINCampaignGroup.Upgrade14, lkpINCampaignGroup.Upgrade15,

                                lkpINCampaignGroup.DoubleUpgrade1, lkpINCampaignGroup.DoubleUpgrade2,
                                lkpINCampaignGroup.DoubleUpgrade3, lkpINCampaignGroup.DoubleUpgrade4, lkpINCampaignGroup.DoubleUpgrade5, lkpINCampaignGroup.DoubleUpgrade6,
                                lkpINCampaignGroup.DoubleUpgrade7, lkpINCampaignGroup.DoubleUpgrade8, lkpINCampaignGroup.DoubleUpgrade9, lkpINCampaignGroup.DoubleUpgrade10,
                                lkpINCampaignGroup.DoubleUpgrade11, lkpINCampaignGroup.DoubleUpgrade12, lkpINCampaignGroup.DoubleUpgrade13, lkpINCampaignGroup.DoubleUpgrade14,
                                lkpINCampaignGroup.DoubleUpgrade15, lkpINCampaignGroup.DefrostR99, lkpINCampaignGroup.Lite, lkpINCampaignGroup.R99Upgrade, lkpINCampaignGroup.R99, lkpINCampaignGroup.Tier3
                            };
                        if (campaignTypes.Contains(campaignType) && campaignGroups.Contains(campaignGroup))
                        {
                            strQuery += "lkpINCampaignType.ID IN (" + string.Join(",", campaignTypes.Cast<int>().ToArray()) + ") AND ";
                            strQuery += "lkpINCampaignGroup.ID IN (" + string.Join(",", campaignGroups.Cast<int>().ToArray()) + ") AND ";
                            break;
                        }

                        //Cancer upgrades
                        campaignTypes = new[]
                        {
                                lkpINCampaignType.Cancer, lkpINCampaignType.CancerFuneral, lkpINCampaignType.CancerFuneral99, lkpINCampaignType.IGCancer, lkpINCampaignType.TermCancer
                        };
                        campaignGroups = new[]
                        {
                                lkpINCampaignGroup.Upgrade, lkpINCampaignGroup.Upgrade1, lkpINCampaignGroup.Upgrade2, lkpINCampaignGroup.Upgrade3, lkpINCampaignGroup.Upgrade4,
                                lkpINCampaignGroup.Upgrade5, lkpINCampaignGroup.Upgrade6, lkpINCampaignGroup.Upgrade7, lkpINCampaignGroup.Upgrade8,  lkpINCampaignGroup.Upgrade9,
                                lkpINCampaignGroup.Upgrade10, lkpINCampaignGroup.Upgrade11, lkpINCampaignGroup.Upgrade12, lkpINCampaignGroup.Upgrade13, lkpINCampaignGroup.Upgrade14, lkpINCampaignGroup.Upgrade15,
                                lkpINCampaignGroup.DoubleUpgrade1, lkpINCampaignGroup.DoubleUpgrade2,
                                lkpINCampaignGroup.DoubleUpgrade3, lkpINCampaignGroup.DoubleUpgrade4, lkpINCampaignGroup.DoubleUpgrade5, lkpINCampaignGroup.DoubleUpgrade6,
                                lkpINCampaignGroup.DoubleUpgrade7, lkpINCampaignGroup.DoubleUpgrade8, lkpINCampaignGroup.DoubleUpgrade9, lkpINCampaignGroup.DoubleUpgrade10,
                                lkpINCampaignGroup.DoubleUpgrade11, lkpINCampaignGroup.DoubleUpgrade12, lkpINCampaignGroup.DoubleUpgrade13, lkpINCampaignGroup.DoubleUpgrade14,
                                lkpINCampaignGroup.DoubleUpgrade15, lkpINCampaignGroup.DefrostR99, lkpINCampaignGroup.Lite, lkpINCampaignGroup.R99Upgrade, lkpINCampaignGroup.R99, lkpINCampaignGroup.Tier3
                            };
                        if (campaignTypes.Contains(campaignType) && campaignGroups.Contains(campaignGroup))
                        {
                            strQuery += "lkpINCampaignType.ID IN (" + string.Join(",", campaignTypes.Cast<int>().ToArray()) + ") AND ";
                            strQuery += "lkpINCampaignGroup.ID IN (" + string.Join(",", campaignGroups.Cast<int>().ToArray()) + ") AND ";
                            break;
                        }

                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "The campaign type or campaign group\n is not registered for copy.", "Batch not Duplicate Checked", ShowMessageType.Error);
                        });

                        return;
                    }
                }
            }

            #endregion

            foreach (SourceLead lead in _leadsToCopy)
            {
                string strSQL;
                DataTable dt;

                long importID = Convert.ToInt64(lead.ImportID);
                INImport inImport = new INImport(importID);

                long leadID = Convert.ToInt64(inImport.FKINLeadID);
                INLead inLead = new INLead(leadID);

                if (!(dtUpgradeCampaigns.AsEnumerable().Any(row => Convert.ToInt64(ScrData.DestinationCampaignID) == row.Field<Int64>("CampaignID"))))
                {

                #region check for duplicate RefNo in this campaign and Last ImportDate < x months

                {
                    string strRefNo = Insure.INGetRenewalReferenceNumber((long)ScrData.SourceCampaignID, (long)ScrData.DestinationCampaignID, lead.RefNumber);

                    if (!string.IsNullOrWhiteSpace(strRefNo))
                    {
                        strSQL = strQuery + "RefNo = '" + strRefNo + "' and ImportDate >= DateAdd(MM, " + dupCheckMonth + ", GetDate())";
                        dt = Methods.GetTableData(strSQL);

                        if (dt.Rows.Count > 0)
                        {
                            _leadsToCopyDuplicates.Add(lead);
                            inImport.IsCopyDuplicate = true;
                            inImport.Save(_validationResult);
                            continue;
                        }
                    }
                }

                #endregion

                #region check for duplicate ID numbers in leads and last importdate < x months

                {
                    string strIDNumber = inLead.IDNo;

                    if (!string.IsNullOrWhiteSpace(strIDNumber) && !strIDNumber.Contains("0000000"))
                    {
                        strSQL = strQuery + "INLead.IDNo = '" + strIDNumber + "' AND ImportDate >= DateAdd(MM, " + dupCheckMonth + ", GetDate())";
                        dt = Methods.GetTableData(strSQL);

                        if (dt.Rows.Count > 0)
                        {
                            _leadsToCopyDuplicates.Add(lead);
                            inImport.IsCopyDuplicate = true;
                            inImport.Save(_validationResult);
                            continue;
                        }
                    }
                }

                #endregion

                #region check for first name, last name, dob and last importdate < x months

                {
                    string strLeadFirstname = inLead.FirstName;
                    string strLeadSurname = inLead.Surname;
                    string strLeadDateOfBirth = null;
                    if (inLead.DateOfBirth != null)
                    {
                        strLeadDateOfBirth = inLead.DateOfBirth.ToString();
                    }

                    if (!string.IsNullOrWhiteSpace(strLeadFirstname) && !string.IsNullOrWhiteSpace(strLeadSurname) && !string.IsNullOrWhiteSpace(strLeadDateOfBirth))
                    {
                        strSQL = strQuery + "INLead.FirstName = '" + strLeadFirstname.Replace("'", "''") + "' AND INLead.Surname = '" + strLeadSurname.Replace("'", "''") + "' AND INLead.DateOfBirth = '" + strLeadDateOfBirth + "' AND ImportDate >= DateAdd(MM, " + dupCheckMonth + ", GetDate())";
                        dt = Methods.GetTableData(strSQL);

                        if (dt.Rows.Count > 0)
                        {
                            _leadsToCopyDuplicates.Add(lead);
                            inImport.IsCopyDuplicate = true;
                            inImport.Save(_validationResult);
                            continue;
                        }
                    }
                }

                #endregion

                #region check for first name, last name, dob and tel cell < x months

                {
                    string strLeadFirstname = inLead.FirstName;
                    string strLeadSurname = inLead.Surname;
                    string strTelCell = inLead.TelCell;

                    if (!string.IsNullOrWhiteSpace(strLeadFirstname) && !string.IsNullOrWhiteSpace(strLeadSurname) && !string.IsNullOrWhiteSpace(strTelCell))
                    {
                        strSQL = strQuery + "INLead.FirstName = '" + strLeadFirstname.Replace("'", "''") + "' AND INLead.Surname = '" + strLeadSurname.Replace("'", "''") + "' AND INLead.TelCell = '" + strTelCell + "' AND ImportDate >= DateAdd(MM, " + dupCheckMonth + ", GetDate())";
                        dt = Methods.GetTableData(strSQL);

                        if (dt.Rows.Count > 0)
                        {
                            _leadsToCopyDuplicates.Add(lead);
                            inImport.IsCopyDuplicate = true;
                            inImport.Save(_validationResult);
                            continue;
                        }
                    }
                }

                #endregion

                }


                _leadsToCopyFinal.Add(lead);

            }

            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            {
                ShowMessageBox(new INMessageBoxWindow1(), _leadsToCopyDuplicates.Count + " Duplicates Found", "Duplicates", ShowMessageType.Information);
            });
        }

        private void CopyLeads()
        {
            try
            {
                _worker = new BackgroundWorker();
                _worker.WorkerSupportsCancellation = true;
                _worker.WorkerReportsProgress = true;

                _worker.DoWork += (sender, e) =>
                {
                    try
                    {
                        Database.BeginTransaction(null, IsolationLevel.Snapshot);

                        _leadsToCopy = new Collection<SourceLead>();
                        foreach (SourceLead lead in ScrData.Leads.SourceLeads.Where(lead => lead.IsChecked))
                        {
                            _leadsToCopy.Add(lead);
                        }

                        //check for duplicates in _leadsToCopy
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            atCopy.Text = "Duplicate Check";
                            btnCopy.IsEnabled = false;
                        });

                        DataTable dtUpgradeCampaigns = Methods.GetTableData("SELECT C.ID AS CampaignID FROM INCampaign AS C " +
                                                                            "LEFT JOIN INCampaignGroupSet AS CGS ON C.FKINCampaignGroupID = CGS.FKlkpINCampaignGroup " +
                                                                            "WHERE CGS.FKlkpINCampaignGroupType = 2");

                        //if (!(dtUpgradeCampaigns.AsEnumerable().Any(row => Convert.ToInt64(ScrData.DestinationCampaignID) == row.Field<Int64>("CampaignID"))))
                        //{
                        DuplicateFinder();
                        //}
                        
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            atCopy.Text = "_Cancel";
                            btnCopy.IsEnabled = true;
                        });

                        DataTable dtBatch = Methods.GetTableData("SELECT * FROM [INBatch] WHERE [FKINCampaignID] = " + ScrData.DestinationCampaignID + " AND Code = '" + ScrData.DestinationBatchCode + "'");

                        bool batchCreated = false;
                        bool batchExists = false;

                        if (dtBatch.Rows.Count > 0)
                        {
                            batchExists = true;
                            ScrData.DestinationBatchID = long.Parse(dtBatch.Rows[0]["ID"].ToString());
                        }
                        if (dtBatch.Rows.Count == 0)
                        {
                            //create new batch
                            INBatch batch = new INBatch();
                            batch.Code = ScrData.DestinationBatchCode;
                            batch.UDMCode = ScrData.DestinationBatchCode;
                            batch.FKINCampaignID = ScrData.DestinationCampaignID;
                            batch.NewLeads = 0;
                            batch.UpdatedLeads = 0;
                            batch.Save(_validationResult);
                            batchCreated = true;
                            ScrData.DestinationBatchID = batch.ID;
                        }

                        if (batchExists || batchCreated)
                        {
                            int counter = 1;
                            foreach (SourceLead lead in _leadsToCopyFinal)
                            {
                                if (lead.ImportID != null && ScrData.SourceCampaignID != null && ScrData.DestinationCampaignID != null)
                                {

                                    #region Copy INImport

                                    INImport srcImport = new INImport((long)lead.ImportID);
                                    INImport desImport = new INImport();

                                    Methods.CopyObject(srcImport, desImport);

                                    desImport.FKUserID = null;
                                    desImport.FKINCampaignID = ScrData.DestinationCampaignID;
                                    desImport.FKINBatchID = ScrData.DestinationBatchID;
                                    desImport.FKINLeadStatusID = null;
                                    desImport.FKINDeclineReasonID = lead.FKINDeclineReasonID; // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/222129304/comments
                                    
                                    if (!(dtUpgradeCampaigns.AsEnumerable().Any(row => Convert.ToInt64(ScrData.DestinationCampaignID) == row.Field<Int64>("CampaignID"))))
                                    {
                                        desImport.RefNo = Insure.INGetRenewalReferenceNumber((long)ScrData.SourceCampaignID, (long)ScrData.DestinationCampaignID, srcImport.RefNo);
                                    }

                                    else
                                    {
                                        desImport.RefNo = srcImport.RefNo;
                                    }

                                    desImport.AllocationDate = null;
                                    desImport.IsPrinted = null;
                                    desImport.DateOfSale = null;

                                    desImport.BankCallRef = null;
                                    desImport.FKBankCallRefUserID = null;
                                    desImport.BankStationNo = null;
                                    desImport.BankDate = null;
                                    desImport.BankTime = null;
                                    desImport.FKBankTelNumberTypeID = null;
                                    desImport.SaleCallRef = null;
                                    desImport.FKSaleCallRefUserID = null;
                                    desImport.SaleStationNo = null;
                                    desImport.SaleDate = null;
                                    desImport.SaleTime = null;
                                    desImport.FKSaleTelNumberTypeID = null;
                                    desImport.ConfCallRef = null;
                                    desImport.FKConfCallRefUserID = null;
                                    desImport.ConfStationNo = null;
                                    desImport.ConfDate = null;
                                    desImport.ConfTime = null;
                                    desImport.FKConfTelNumberTypeID = null;

                                    desImport.IsConfirmed = null;
                                    desImport.Notes = null;
                                    desImport.FKClosureID = null;
                                    desImport.Feedback = null;
                                    desImport.FeedbackDate = null;
                                    desImport.FKINLeadFeedbackID = null;
                                    desImport.FKINCancellationReasonID = null;
                                    desImport.IsCopied = null;

                                    desImport.FKINConfirmationFeedbackID = null;
                                    desImport.FKINParentBatchID = null;
                                    desImport.BonusLead = false;
                                    desImport.FKBatchCallRefUserID = null;
                                    desImport.IsMining = null;
                                    desImport.ConfWorkDate = null;
                                    desImport.IsChecked = null;
                                    desImport.CheckedDate = null;
                                    desImport.DateBatched = null;

                                    desImport.Save(_validationResult);

                                    #endregion Copy INImport

                                    #region Copy INImportOther

                                    {
                                        List<Tuple<string, string, object>> parameters = new List<Tuple<string, string, object>>();
                                        parameters.Add(new Tuple<string, string, object>("", "FKINImportID", lead.ImportID));
                                        List<long?> objIDs = Methods.GetObjectIDs("INImportOther", parameters);

                                        if (objIDs != null)
                                        {
                                            foreach (long? id in objIDs)
                                            {
                                                if (id != null)
                                                {
                                                    INImportOther srcObject = new INImportOther((long)id);
                                                    INImportOther desObject = new INImportOther();

                                                    Methods.CopyObject(srcObject, desObject);

                                                    desObject.FKINImportID = desImport.ID;
                                                    desObject.FKINBatchID = desImport.FKINBatchID;
                                                    desObject.RefNo = desImport.RefNo;

                                                    desObject.Save(_validationResult);
                                                }
                                            }
                                        }
                                    }

                                    #endregion Copy INImportOther

                                    #region Copy INGiftRedeem Details

                                    if (ScrData.DestinationBatchCode.Contains("_R") || ScrData.DestinationBatchCode.Contains("_NR"))
                                    {
                                        INGiftRedeem srcINGiftRedeem = INGiftRedeemMapper.SearchOne(srcImport.ID, null, null, null, null, null, null, null, null);
                                        INGiftRedeem desINGiftRedeem = new INGiftRedeem();

                                        if (srcINGiftRedeem != null) 
                                        {  
                                            desINGiftRedeem.FKINImportID = desImport.ID;
                                            desINGiftRedeem.FKlkpGiftRedeemStatusID = srcINGiftRedeem.FKlkpGiftRedeemStatusID;
                                            desINGiftRedeem.FKGiftOptionID = srcINGiftRedeem.FKGiftOptionID;
                                            desINGiftRedeem.RedeemedDate = srcINGiftRedeem.RedeemedDate;
                                            desINGiftRedeem.PODDate = srcINGiftRedeem.PODDate;
                                            desINGiftRedeem.PODSignature = srcINGiftRedeem.PODSignature;
                                            desINGiftRedeem.IsWebRedeemed = srcINGiftRedeem.IsWebRedeemed;
                                            desINGiftRedeem.FKUserID = srcINGiftRedeem.FKUserID;
                                        }
                                        else
                                        {
                                            desINGiftRedeem.FKINImportID = desImport.ID;
                                            desINGiftRedeem.FKlkpGiftRedeemStatusID = ScrData.DestinationBatchCode.Contains("_R") ? 1 : 2;
                                        }

                                        counter++;
                                        desINGiftRedeem.Save(_validationResult);
                                    }

                                    #endregion

                                    #region Copy INImportLatentLeadReason

                                    // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/222129304/comments
                                    //INImportLatentLeadReason

                                    if (lead.FKINImportLatentLeadReasonID != null)
                                    {
                                        INImportLatentLeadReason srcINImportLatentLeadReason = new INImportLatentLeadReason((long)lead.FKINImportLatentLeadReasonID);
                                        INImportLatentLeadReason desINImportLatentLeadReason = new INImportLatentLeadReason();

                                        desINImportLatentLeadReason.FKINImportID = desImport.ID;
                                        desINImportLatentLeadReason.FKINLatentLeadReasonID1 = srcINImportLatentLeadReason.FKINLatentLeadReasonID1;
                                        desINImportLatentLeadReason.FKINLatentLeadReasonID2 = srcINImportLatentLeadReason.FKINLatentLeadReasonID2;
                                        desINImportLatentLeadReason.FKINLatentLeadReasonID3 = srcINImportLatentLeadReason.FKINLatentLeadReasonID3;

                                        desINImportLatentLeadReason.Save(_validationResult);
                                    }

                                    #endregion Copy INImportLatentLeadReason

                                    #region Copy INNextOfKin

                                    {
                                        List<Tuple<string, string, object>> parameters = new List<Tuple<string, string, object>>();
                                        parameters.Add(new Tuple<string, string, object>("", "FKINImportID", lead.ImportID));
                                        List<long?> objIDs = Methods.GetObjectIDs("INNextOfKin", parameters);

                                        if (objIDs != null)
                                        {
                                            foreach (long? id in objIDs)
                                            {
                                                if (id != null)
                                                {
                                                    INNextOfKin srcObject = new INNextOfKin((long)id);
                                                    INNextOfKin desObject = new INNextOfKin();

                                                    Methods.CopyObject(srcObject, desObject);

                                                    desObject.FKINImportID = desImport.ID;
                                                    desObject.FKINRelationshipID = srcObject.FKINRelationshipID;
                                                    desObject.FirstName = srcObject.FirstName;
                                                    desObject.Surname = srcObject.Surname;
                                                    desObject.TelContact = srcObject.TelContact;


                                                    desObject.Save(_validationResult);
                                                }
                                            }
                                        }
                                    }



                                    #endregion Copy INNextOfKin

                                    #region Copy INImportContactTracing 

                                    {
                                        List<Tuple<string, string, object>> parameters = new List<Tuple<string, string, object>>();
                                        parameters.Add(new Tuple<string, string, object>("", "FKINImportID", lead.ImportID));
                                        List<long?> objIDs = Methods.GetObjectIDs("INImportContactTracing", parameters);

                                        if (objIDs != null)
                                        {
                                            foreach (long? id in objIDs)
                                            {
                                                if (id != null)
                                                {
                                                    INImportContactTracing srcObject = new INImportContactTracing((long)id);
                                                    INImportContactTracing desObject = new INImportContactTracing();

                                                    Methods.CopyObject(srcObject, desObject);

                                                    desObject.FKINImportID = desImport.ID;
                                                    desObject.ContactTraceOne = srcObject.ContactTraceOne;
                                                    desObject.ContactTraceTwo = srcObject.ContactTraceTwo;
                                                    desObject.ContactTraceThree = srcObject.ContactTraceThree;
                                                    desObject.ContactTraceFour = srcObject.ContactTraceFour;
                                                    desObject.ContactTraceFive = srcObject.ContactTraceFive;
                                                    desObject.ContactTraceSix = srcObject.ContactTraceSix;

                                                    desObject.Save(_validationResult);
                                                }
                                            }
                                        }
                                    }



                                    #endregion Copy INNextOfKin

                                    #region clear OptionID

                                    if (desImport.FKINPolicyID != null)
                                    {
                                        try
                                        {
                                            long policyID = Convert.ToInt64(desImport.FKINPolicyID);

                                            if (policyID > 0)
                                            {
                                                INPolicy inPolicy = new INPolicy(policyID);
                                                if (inPolicy.FKINOptionID != null)
                                                {
                                                    inPolicy.FKINOptionID = null;
                                                    inPolicy.Save(_validationResult);
                                                }
                                            }
                                        }
                                        catch
                                        {

                                        }

                                    }

                                    #endregion

                                    //this lead has been copied, flag and lock source lead for sales agents
                                    srcImport.IsCopied = true;
                                    srcImport.Save(_validationResult);

                                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                                    {
                                        ScrData.Leads.DestinationLeads.Add(new DestinationLead
                                        {
                                            ImportID = lead.ImportID,
                                            RefNumber = lead.RefNumber
                                        });

                                        ScrData.Leads.SourceLeads.Remove(lead);
                                    });

                                    ScrData.Leads.TotalLeadsCopied++;
                                }

                                if (_worker.CancellationPending)
                                {
                                    Database.CancelTransactions();
                                    e.Cancel = true;
                                    return;
                                }
                            }

                            CommitTransaction(null);
                        }
                        else
                        {
                            Database.CancelTransactions();
                            ShowMessageBox(new INMessageBoxWindow1(), "An error occurred in copying the leads.", "Copy Error", ShowMessageType.Error);
                        }
                    }

                    catch (Exception ex)
                    {
                        Database.CancelTransactions();
                        Methods.HandleException(ex, this);
                    }
                };

                _worker.ProgressChanged += (Sender, E) =>
                {
                    
                };

                _worker.RunWorkerCompleted += (Sender, E) =>
                {
                    atCopy.Text = "_Continue";
                };

                _worker.RunWorkerAsync();

            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        #endregion Methods
        
        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }
        
        private void cmbCampaigns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                LoadBatches();

                string CampaignID = cmbCampaigns.SelectedValue.ToString();
                string SelectedCampaignName = Convert.ToString(Methods.GetTableData("SELECT Name FROM INCampaign WHERE ID = " + CampaignID).Rows[0][0]);

                if (SelectedCampaignName.ToString().Contains("Upgrade"))
                {
                    cmbDestinationCampaigns.SelectedValue = cmbCampaigns.SelectedValue;
                }
                else
                {
                    //cmbDestinationCampaigns.SelectedValue = null;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmbBatch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ScrData.SourceBatchID != null)
                {
                    LoadSourceLeads(ScrData.SourceCampaignID, ScrData.SourceBatchID);

                    INBatch batch = new INBatch((long) ScrData.SourceBatchID);
                    ScrData.DestinationBatchCode = batch.Code;
                }
                else
                {
                    ScrData.Leads = new Leads(new ObservableCollection<SourceLead>(), new ObservableCollection<DestinationLead>());
                    ScrData.DestinationCampaignID = null;
                    ScrData.DestinationBatchCode = null;
                }

                ScrData.Leads.TotalFilteredInLeads = xdgSourceLeads.RecordManager.GetFilteredInDataRecords().Count();
                ScrData.Leads.TotalLeadsAvailable = ScrData.Leads.TotalFilteredInLeads - ScrData.Leads.TotalChecked;
                ScrData.Leads.TotalLeadsCopied = 0;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        
        private void xdgSourceLeads_RecordFilterChanged(object sender, RecordFilterChangedEventArgs e)
        {
            XamDataGrid xamDataGrid = sender as XamDataGrid;
            if (xamDataGrid != null)
            {
                List<Record> records = xamDataGrid.Records.ToList();
                foreach (Record record in records)
                {
                    if (record != null)
                    {
                        DataRecord dataRecord = (DataRecord)record;
                        if (dataRecord.IsDataRecord)
                        {
                            ((SourceLead)dataRecord.DataItem).IsFilteredOut = Convert.ToBoolean(dataRecord.IsFilteredOut);
                        }
                    }
                }
            }

            ScrData.Leads.TotalFilteredInLeads = xdgSourceLeads.RecordManager.GetFilteredInDataRecords().Count();
            ScrData.Leads.TotalLeadsAvailable = ScrData.Leads.TotalFilteredInLeads - ScrData.Leads.TotalChecked;
        }

        #endregion

    }

}
