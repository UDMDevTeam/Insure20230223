using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Embriant.Framework.Data;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.Events;
using UDM.Insurance.Business;
using UDM.WPF.Library;
using System.Threading;
using System.Windows.Threading;
using UDM.Insurance.Interface.Windows;
using Embriant.Framework;
using System.Diagnostics;
using Embriant.Framework.Configuration;

namespace UDM.Insurance.Interface.Screens
{
    public partial class DeleteSingleLeadScreen : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }

        #endregion



        #region Classes

        public class Batch : ObservableObject
        {
            int? _assigned;
            public int? Assigned
            {
                get { return _assigned; }
                set { SetProperty(ref _assigned, value, () => Assigned); }
            }

            int? _unAssigned;
            public int? UnAssigned
            {
                get { return _unAssigned; }
                set { SetProperty(ref _unAssigned, value, () => UnAssigned); }
            }
        }

        public class Lead : ObservableObject
        {
            //bool _isChecked;
            //public bool IsChecked
            //{
            //    get { return _isChecked; }
            //    set { SetProperty(ref _isChecked, value, () => IsChecked); }
            //}

            //bool _isFilteredOut;
            //public bool IsFilteredOut
            //{
            //    get { return _isFilteredOut; }
            //    set
            //    {
            //        SetProperty(ref _isFilteredOut, value, () => IsFilteredOut);
            //        if (_isFilteredOut)
            //        {
            //            IsChecked = false;
            //        }
            //    }
            //}

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

            //private long? _leadID;
            //public long? LeadID
            //{
            //    get { return _leadID; }
            //    set { SetProperty(ref _leadID, value, () => LeadID); }
            //}
            private string _campaignCode;
            public string CampaignCode
            {
                get { return _campaignCode; }
                set { SetProperty(ref _campaignCode, value, () => CampaignCode); }
            }

            private string _batchCode;
            public string BatchCode
            {
                get { return _batchCode; }
                set { SetProperty(ref _batchCode, value, () => BatchCode); }
            }

            private string _tsrAssignedTo;
            public string TSRAssignedTo
            {
                get { return _tsrAssignedTo; }
                set { SetProperty(ref _tsrAssignedTo, value, () => TSRAssignedTo); }
            }

            private string _iDNumber;
            public string IDNumber
            {
                get { return _iDNumber; }
                set { SetProperty(ref _iDNumber, value, () => IDNumber); }
            }

            private string _firstName;
            public string FirstName
            {
                get { return _firstName; }
                set { SetProperty(ref _firstName, value, () => FirstName); }
            }

            private string _surname;
            public string Surname
            {
                get { return _surname; }
                set { SetProperty(ref _surname, value, () => Surname); }
            }
        }

        public class LeadViewModel : INotifyPropertyChanged
        {
            public readonly Lead _lead;
            //private DataViewModel _dvm;
            bool _isChecked;

            public LeadViewModel(Lead lead/*, DataViewModel dvm, int totalChecked*/)
            {
                _lead = lead;
                
                /*_dvm = dvm;
                _totalChecked = totalChecked;*/
            }

            public bool IsChecked
            {
                get { return _isChecked; }
                set
                {
                    if (value == _isChecked)
                        return;

                    _isChecked = value;

                    //_dvm.TotalChecked =
                    this.OnPropertyChanged("IsChecked");
                }
            }
            public long? ImportID { get { return _lead.ImportID; } }
            public string RefNumber { get { return _lead.RefNumber; } }
            public string CampaignCode { get { return _lead.CampaignCode; } }
            public string BatchCode
            {
                get { return _lead.BatchCode; }
            }
            public string TSRAssignedTo
            {
                get { return _lead.TSRAssignedTo; }
                
            }

            public string IDNumber
            {
                get { return _lead.IDNumber; }
            }

            public string FirstName
            {
                get { return _lead.FirstName; }
            }

            public string Surname
            {
                get { return _lead.Surname; }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            void OnPropertyChanged(string prop)
            {
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }

        }

        public class DataViewModel : INotifyPropertyChanged
        {
            //public DataViewModel(ObservableCollection<Lead> leads/*, ObservableCollection<Lead> filteredLeads*/)
            public DataViewModel(List<LeadViewModel> leads/*, ObservableCollection<Lead> filteredLeads*/)
            {
                Leads = leads;

                foreach (LeadViewModel lead in Leads)
                {
                    lead.PropertyChanged += (sender, e) =>//delegate
                    {
                        if (e.PropertyName == "IsChecked")
                        OnPropertyChanged("AllLeadsAreChecked");
                    };
                }
            }

            public List<LeadViewModel> Leads { get; private set; }

            //private List<LeadViewModel> propLeads;
            //public List<LeadViewModel> Leads
            //{
            //    get
            //    {
            //        TotalChecked = 0;
            //        TotalChecked = propLeads.Where(x => x.IsChecked == true).Count();
            //        return propLeads;
            //    }
            //    private set
            //    {
            //        propLeads = value;
            //        OnPropertyChanged("Leads");
            //    }
            //}

            //private ObservableCollection<Lead> _leads;
            //public ObservableCollection<Lead> Leads
            //{
            //    get
            //    {
            //        //TotalChecked = 0;
            //        //foreach (Lead lead in _leads)
            //        //{
            //        //    if (lead.IsChecked)
            //        //    {
            //        //        TotalChecked++;
            //        //    }
            //        //}
            //        TotalChecked = 0;
            //        TotalChecked = _leads.Where(x => x.IsChecked == true).Count();
            //        //TotalChecked = _leads.Where(x => x.IsChecked == true && x.IsFilteredOut == false).Count();

            //        return _leads;
            //    }
            //    private set
            //    {
            //        _leads = value;
            //        OnPropertyChanged("Leads");
            //    }
            //}



            public bool? AllLeadsAreChecked
            {
                get
                {
                    bool? value = null;
                    for (int i = 0; i < this.Leads.Count; ++i)
                    {
                        if (i == 0)
                        {
                            value = this.Leads[0].IsChecked;
                        }
                        else if (value != this.Leads[i].IsChecked)
                        {
                            value = null;
                            break;
                        }
                    }

                    return value;
                }
                set
                {
                    if (value == null)
                        return;

                    foreach (LeadViewModel member in this.Leads)
                    {
                        //if (!member.IsFilteredOut)
                            member.IsChecked = value.Value;
                    }
                }
            }

            int _totalChecked;
            public int TotalChecked
            {
                get { return _totalChecked; }
                set
                {
                    //TotalChecked = value;
                    _totalChecked = value;
                    OnPropertyChanged("TotalChecked");
                }
            }

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            void OnPropertyChanged(string prop)
            {
                if (this.PropertyChanged != null) this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }

            #endregion
        }

        //public static class CustomCommands
        //{
        //    public static readonly RoutedUICommand Allocate = new RoutedUICommand
        //    (
        //        "Allocate",
        //        "Allocate",
        //        typeof(CustomCommands)
        //    //new InputGestureCollection()
        //    //{
        //    //    new KeyGesture(Key.A, ModifierKeys.Alt)
        //    //}
        //    );

        //    //Define more commands here, just like the one above
        //}

        #endregion



        #region Constant

        #endregion



        #region Properties

        bool _isDeleting;
        public bool IsDeleting
        {
            get { return _isDeleting; }
            set
            {
                _isDeleting = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleting"));
            }

        }

        bool _isAllocating;
        public bool IsAllocating
        {
            get { return _isAllocating; }
            set
            {
                _isAllocating = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAllocating"));
            }
        }

        Batch _batch2 = new Batch();
        public Batch Batch2
        {
            get { return _batch2; }
            set
            {
                _batch2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Batch2"));
            }
        }

        #endregion



        #region Private Members

        private long _batchID;
        private long _campaignID;
        private DataTable _dtBatch;
        private DataTable _dtLeads;
        private DataTable _dtAgents;
        //private ObservableCollection<Lead> _leads;
        private List<LeadViewModel> _leads;
        private DataViewModel _dvm;
        private BackgroundWorker _worker;

        #endregion



        #region Constructors

        public DeleteSingleLeadScreen()
        {
            InitializeComponent();

            //LoadLookupData();
            LoadCampaignInfo();
        }

        #endregion



        #region Private Methods

        private void LoadLeads()
        {
            try
            {
                if (cmbBatch.SelectedIndex > -1)
                {

                    SetCursor(Cursors.Wait);
                    Cursor = Cursors.Wait;

                    //DataTable dt = Methods.GetTableData("SELECT ReferenceNumber [RefNo], CampaignCode [CampaignCode] FROM INImport WHERE FKINBatchCode = '" + cmbBatch.SelectedValue + "'");
                    //DataColumn column = new DataColumn("Select", typeof(bool));
                    //column.DefaultValue = false;
                    //dt.Columns.Add(column);
                    //dt.DefaultView.Sort = "UDMBatchCode ASC";

                    //xdgAssignLeads.DataSource = dt.DefaultView;

                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("CampaignID", (long)cmbCampaign.SelectedValue);
                    parameters[1] = new SqlParameter("BatchID", (long)cmbBatch.SelectedValue);

                    DataSet ds = Methods.ExecuteStoredProcedure("spINGetDeleteSingleLeadsData", parameters);
                    _dtLeads = ds.Tables[0];
                    tbBatch.Text = ((DataRowView)cmbBatch.Items[cmbBatch.SelectedIndex]).Row.ItemArray[2].ToString();
                    //_dtBatch = ds.Tables[0];
                    //_dtAgents = ds.Tables[2];

                    _leads = new List<LeadViewModel>();
                    //foreach (DataRow row in _dtLeads.AsEnumerable())
                    //{
                    //    _leads
                    //        (new Lead
                    //        {
                    //            ImportID = row["ImportID"] as long?,
                    //            RefNumber = row["ReferenceNumber"] as string,
                    //            CampaignCode = row["CampaignCode"] as string,
                    //            BatchCode = row["BatchCode"] as string,
                    //            TSRAssignedTo = row["TSRAssignedTo"] as string,
                    //            IDNumber = row["IDNumber"] as string,
                    //            FirstName = row["FirstName"] as string,
                    //            Surname = row["Surname"] as string,
                    //        });
                    //}

                    foreach (DataRow row in _dtLeads.AsEnumerable())
                    {
                        Lead l = new Lead
                        {
                            ImportID = row["ImportID"] as long?,
                            RefNumber = row["ReferenceNumber"] as string,
                            CampaignCode = row["CampaignCode"] as string,
                            BatchCode = row["BatchCode"] as string,
                            TSRAssignedTo = row["TSRAssignedTo"] as string,
                            IDNumber = row["IDNumber"] as string,
                            FirstName = row["FirstName"] as string,
                            Surname = row["Surname"] as string,
                        };
                        _leads.Add(new LeadViewModel(l));

                    }

                    DataContext = _dvm = new DataViewModel(_leads);
                    foreach (LeadViewModel lvm in _dvm.Leads)
                    {
                        lvm.PropertyChanged += LeadViewModel_OnPropertyChanged;
                    }
                       ((DataViewModel)DataContext).TotalChecked = xdgAssignLeads.RecordManager.GetFilteredInDataRecords().Where(x => ((LeadViewModel)x.DataItem).IsChecked).Count();

                    //tbCampaign.Text = _dtBatch.Rows[0]["CampaignName"].ToString();


                    //tbUDMBatch.Text = _dtBatch.Rows[0]["UDMBatchCode"].ToString();

                    //Batch2.Assigned = _dtBatch.Rows[0]["Assigned"] as int?;
                    //Batch2.UnAssigned = _dtBatch.Rows[0]["Unassigned"] as int?;

                    //tbTotalPrinted.Text = _dtBatch.Rows[0]["Printed"].ToString();
                }
                //else
                //{
                //    xdgAssignLeads.DataSource = null;
                //}

                //IsAllRecordsSelected = false;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SetCursor(Cursors.Arrow);
                //Cursor = Cursors.Arrow;
            }
        }

        //private void LoadLookupData()
        //{
        //    try
        //    {
        //        SetCursor(Cursors.Wait);

        //        SqlParameter[] parameters = new SqlParameter[1];
        //        parameters[0] = new SqlParameter("BatchID", _batchID);

        //        //DataSet ds = Methods.ExecuteStoredProcedure("spINGetBatchAssignSingleLeadsData", parameters);
        //        DataSet ds = Methods.ExecuteStoredProcedure("spINGetDeleteSingleLeadScreenLookups", null);
        //        _dtBatch = ds.Tables[0];
        //        _dtLeads = ds.Tables[1];
        //        _dtAgents = ds.Tables[2];

        //        _leads = new ObservableCollection<Lead>();
        //        foreach (DataRow row in _dtLeads.AsEnumerable())
        //        {
        //            _leads.Add(new Lead
        //            {
        //                ImportID = row["ImportID"] as long?,
        //                RefNumber = row["RefNumber"] as string,
        //                IDNumber = row["IDNumber"] as string,
        //                FirstName = row["FirstName"] as string,
        //                Surname = row["Surname"] as string,
        //            });
        //        }

        //        DataContext = _dvm = new DataViewModel(_leads);

        //        tbCampaign.Text = _dtBatch.Rows[0]["CampaignName"].ToString();
        //        tbBatch.Text = _dtBatch.Rows[0]["BatchCode"].ToString();
        //        tbUDMBatch.Text = _dtBatch.Rows[0]["UDMBatchCode"].ToString();

        //        Batch2.Assigned = _dtBatch.Rows[0]["Assigned"] as int?;
        //        Batch2.UnAssigned = _dtBatch.Rows[0]["Unassigned"] as int?;

        //        tbTotalPrinted.Text = _dtBatch.Rows[0]["Printed"].ToString();

        //        //cmbAgent.Populate(_dtAgents, "SalesAgent", "UserID");
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

        private void AllocateCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_dvm != null)
            {
                bool test1 = _dvm.TotalChecked > 0;

                bool test2 = cmbBatch.SelectedValue != null;

                bool test3 = IsAllocating;

                e.CanExecute = (test1 && test2) || test3;
            }
        }

        private void AllocateCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsAllocating)
            {
                _worker.CancelAsync();
            }
            else
            {
                acDelete.Text = "_Cancel";
                AllocateLeads();
            }
        }

        private void LoadCampaignInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);

                //DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign WHERE [Code] LIKE '%REN%' ORDER BY Code ASC");
                DataTable dt = Insure.INGetDeleteSingleLeadScreenLookups();
                cmbCampaign.Populate(dt, "CampaignName", "CampaignID");
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

        #endregion



        #region Event Handlers
        private void AllocateLeads()
        {
            IsAllocating = true;
            
            _worker = new BackgroundWorker();
            _worker.WorkerSupportsCancellation = true;
            _worker.WorkerReportsProgress = true;

            _worker.DoWork += (Sender, E) =>
            {
                try
                {
                    long? batchID = E.Argument as long?;
                    //long? totalLeads = _leads.Count(lead => lead.IsChecked);
                    int count = 0;
                    List<DataRecord> filteredLeads = xdgAssignLeads.RecordManager.GetFilteredInDataRecords().ToList();
                    string[] checkedLeads = new string[filteredLeads.Count(lead => ((LeadViewModel)lead.DataItem).IsChecked)];
                    // string[] checkedLeads = new string[_leads.Count(lead => lead.IsChecked && !lead.IsFilteredOut)];
                    //Database.BeginTransaction(null, IsolationLevel.Snapshot);
                    foreach (LeadViewModel lvm in _leads.Where(lead => lead.IsChecked))
                    {
                        Lead lead = lvm._lead;
                        checkedLeads[count] = lead.ImportID.ToString();
                        count++;                            
                    }
                    //string[] checkedLeads = _leads.Where(lead => lead.IsChecked);
                    string importIDs = string.Join(",", checkedLeads);
                    DataSet dsDeleteSingleLead = Insure.spDeleteSingleLead(importIDs, GlobalSettings.ApplicationUser.ID);

                    if (dsDeleteSingleLead.Tables[1].Rows.Count == 0)
                    {
                        DataTable tbl = dsDeleteSingleLead.Tables[3];
                        DataRow row1 = tbl.Rows[0];

                        if (tbl.AsEnumerable().Any(row => row.Field<String>("RefNo").Contains("del")))
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), @"These leads have already been deleted.", "Previously Deleted", ShowMessageType.Exclamation);
                            });
                            return;
                        }
                        else
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), @"These leads do not exist in the database.", "No Data", ShowMessageType.Error);
                            });
                            return;
                        }
                        

                        
                    }
                    else
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), dsDeleteSingleLead.Tables[0].Rows.Count + @" Leads successfully deleted.", "Leads Deleted", ShowMessageType.Information);
                        });
                        return;
                    }
                    //foreach (Lead lead in _leads.Where(lead => lead.IsChecked))
                    //{
                    //    count++;

                    //    if (lead.ImportID != null)
                    //    {

                    //        //INImport import = new INImport((long)lead.ImportID);

                    //        //import.FKINBatchID = batchID;
                    //        //import.AllocationDate = DateTime.Now;

                    //        //import.Save(_validationResult);
                    //    }

                    //Thread.Sleep(1000);

                    if (_worker.CancellationPending)
                        {
                            //Database.CancelTransactions();
                            E.Cancel = true;
                            return;
                        }

                        //int progressPercentage = Convert.ToInt32(((double)count / totalLeads) * 100);
                        //BackgroundWorker backgroundWorker = Sender as BackgroundWorker;
                        //if (backgroundWorker != null) backgroundWorker.ReportProgress(count);
                    //}

                    //CommitTransaction(null);
                }

                catch (Exception ex)
                {
                    Methods.HandleException(ex, this);
                }
            };

            //_worker.ProgressChanged += (Sender, E) =>
            //{
            //    //pbAllocate.Value = E.ProgressPercentage;
            //    //btnAllocate.Content = E.ProgressPercentage;

            //    Batch2.Assigned++;
            //    Batch2.UnAssigned--;
            //};

            _worker.RunWorkerCompleted += (Sender, E) =>
            {
                //LoadLookupData();
                acDelete.Text = "_Delete";
                IsAllocating = false;
                xdgAssignLeads.Focus();
            };

            _worker.RunWorkerAsync();
        }
        
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(false);
        }

        private void xdgAssignLeads_Loaded(object sender, RoutedEventArgs e)
        {
            xdgAssignLeads.Focus();
        }

        private void xdgAssignLeads_RecordFilterChanged(object sender, RecordFilterChangedEventArgs e)
        {
            //ObservableCollection<Lead> filteredLeads = new ObservableCollection<Lead>(xdgAssignLeads.RecordManager.GetFilteredInDataRecords().Select(x => new Lead
            //{
            //    ImportID = ((Lead)x.DataItem).ImportID,
            //    RefNumber = ((Lead)x.DataItem).RefNumber,
            //    CampaignCode = ((Lead)x.DataItem).CampaignCode,
            //    BatchCode = ((Lead)x.DataItem).BatchCode,
            //    TSRAssignedTo = ((Lead)x.DataItem).TSRAssignedTo,
            //    IDNumber = ((Lead)x.DataItem).IDNumber,
            //    FirstName = ((Lead)x.DataItem).FirstName,
            //    Surname = ((Lead)x.DataItem).Surname,
            //}));
            //DataContext = _dvm = new DataViewModel(filteredLeads);
            //_dvm.TotalChecked = _leads.Where(x => x.IsChecked == true && x.IsFilteredOut == false).Count();
            
            //XamDataGrid xamDataGrid = sender as XamDataGrid;
            //if (xamDataGrid != null)
            //{
            //    //List<Record> records = xamDataGrid.Records.ToList();
            //    List<Record> records = xamDataGrid.RecordManager.GetFilteredInDataRecords().ToList();

            //    //Was busy trying to make this process faster currently it takes about 19 or 20 seconds
            //    //for (int x = 0; x < records.Count; x++)
            //    //{
            //    //    if (records[x] != null)
            //    //    {
            //    //        DataRecord dataRecord = (DataRecord)records[x];
            //    //        if (dataRecord.IsDataRecord)
            //    //        {
            //    //            ((Lead)dataRecord.DataItem).IsFilteredOut = Convert.ToBoolean(dataRecord.IsFilteredOut);

            //    //            //IEnumerable<Lead> filteredLeadss = _leads.Where(lead => !lead.IsFilteredOut);
            //    //            //(DataRow)_leads.Where(lead => !lead.IsFilteredOut);
            //    //            //_leads.Clear();
            //    //            //foreach (DataRow lead in (DataRow)filteredLeadss.AsEnumerable().GetEnumerator())
            //    //            //{
            //    //            //    _leads.Add(new Lead
            //    //            //    {
            //    //            //        ImportID = lead["ImportID"] as long?,
            //    //            //        RefNumber = lead["ReferenceNumber"] as string,
            //    //            //        CampaignCode = lead["CampaignCode"] as string,
            //    //            //        BatchCode = lead["BatchCode"] as string,
            //    //            //        TSRAssignedTo = lead["TSRAssignedTo"] as string,
            //    //            //        IDNumber = lead["IDNumber"] as string,
            //    //            //        FirstName = lead["FirstName"] as string,
            //    //            //        Surname = lead["Surname"] as string,
            //    //            //    });
            //    //            //}
            //    //            //ObservableCollection<Lead> filteredLeads =  _leads.Select(lead => !lead.IsFilteredOut);


            //    //        }
            //    //    }
            //    //}
            //    //need to make this process faster
            //    foreach (Record record in records)
            //    {
            //        if (record != null)
            //        {
            //            DataRecord dataRecord = (DataRecord)record;
            //            if (dataRecord.IsDataRecord)
            //            {
            //                ((Lead)dataRecord.DataItem).IsFilteredOut = Convert.ToBoolean(dataRecord.IsFilteredOut);


            //            }
            //        }
            //    }
            //}
        }

        private void cmbAgent_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Methods.EmbriantComboBoxPreviewKeyDown(sender, e);
        }

        private void xdgAssignLeads_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //DataContext = _dvm = new DataViewModel(_leads);
            
            _dvm.TotalChecked = xdgAssignLeads.RecordManager.GetFilteredInDataRecords().Where(x => ((LeadViewModel)x.DataItem).IsChecked).Count();
            //((DataViewModel)DataContext).TotalChecked = xdgAssignLeads.RecordManager.GetFilteredInDataRecords().Where(x => ((LeadViewModel)x.DataItem).IsChecked).Count();
            FrameworkElement fe = e.MouseDevice.DirectlyOver as FrameworkElement;
            if (fe != null && fe.GetType().Name == "XamDataGrid")
            {
                if (xdgAssignLeads != null && xdgAssignLeads.ActiveRecord != null)
                {
                    RecordPresenter rp = RecordPresenter.FromRecord(xdgAssignLeads.ActiveRecord);

                    if (rp != null)
                    {
                        //(LeadViewModel)rp.Record
                        RecordSelector rs = Methods.FindVisualChildren<RecordSelector>(rp).FirstOrDefault();

                        if (rs != null)
                        {
                            CheckBox cb = Methods.FindVisualChildren<CheckBox>(rs).FirstOrDefault();
                            if (cb != null)
                            {
                                if (cb.IsChecked == null)
                                {
                                    cb.IsChecked = false;
                                }

                                cb.IsChecked = !cb.IsChecked;
                            }
                        }
                    }
                }
            }
            
        }
        
        private void xdgAssignLeads_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Space)
            //{
            //    FrameworkElement fe = e.OriginalSource as FrameworkElement;
            //    if (fe != null && fe.GetType().Name == "DataRecordCellArea")
            //    {
            //        if (xdgAssignLeads != null && xdgAssignLeads.ActiveRecord != null)
            //        {
            //            RecordPresenter rp = RecordPresenter.FromRecord(xdgAssignLeads.ActiveRecord);

            //            if (rp != null)
            //            {
            //                RecordSelector rs = Methods.FindVisualChildren<RecordSelector>(rp).FirstOrDefault();

            //                if (rs != null)
            //                {
            //                    CheckBox cb = Methods.FindVisualChildren<CheckBox>(rs).FirstOrDefault();

            //                    if (cb != null)
            //                    {
            //                        if (cb.IsChecked == null)
            //                        {
            //                            cb.IsChecked = false;
            //                        }

            //                        cb.IsChecked = !cb.IsChecked;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

        private void cmbCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbCampaign.SelectedIndex > -1)
                {
                    SetCursor(Cursors.Wait);

                    //DataTable dt = Methods.GetTableData("SELECT ID [BatchID], UDMCode [UDMBatchCode] FROM INBatch WHERE FKINCampaignID = '" + cmbCampaign.SelectedValue + "' ORDER BY [UDMBatchCode] DESC");
                    DataTable dt = Insure.INGetBatchesByCampaignID(Convert.ToInt64(cmbCampaign.SelectedValue));

                    //_campaignID = (long)cmbCampaign.SelectedValue;
                    //DataColumn column = new DataColumn("Select", typeof(bool));
                    //column.DefaultValue = false;
                    //dt.Columns.Add(column);
                    //dt.DefaultView.Sort = "UDMBatchCode ASC";
                    cmbBatch.Populate(dt, "Code", "ID");
                    tbCampaign.Text = ((DataRowView)cmbCampaign.Items[cmbCampaign.SelectedIndex]).Row.ItemArray[1].ToString();
                    //tbCampaign.Text = cmbCampaign.Items[cmbCampaign.SelectedIndex]
                    //xdgBatches.DataSource = dt.DefaultView;
                }
                else
                {
                    //xdgBatches.DataSource = null;
                }

                //IsAllRecordsSelected = false;
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

        private void cmbBatch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbBatch.SelectedIndex > -1)
                {

                    SetCursor(Cursors.Wait);
                    Cursor = Cursors.Wait;
                    
                    //DataTable dt = Methods.GetTableData("SELECT ReferenceNumber [RefNo], CampaignCode [CampaignCode] FROM INImport WHERE FKINBatchCode = '" + cmbBatch.SelectedValue + "'");
                    //DataColumn column = new DataColumn("Select", typeof(bool));
                    //column.DefaultValue = false;
                    //dt.Columns.Add(column);
                    //dt.DefaultView.Sort = "UDMBatchCode ASC";

                    //xdgAssignLeads.DataSource = dt.DefaultView;

                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("CampaignID", (long)cmbCampaign.SelectedValue);
                    parameters[1] = new SqlParameter("BatchID", (long)cmbBatch.SelectedValue);

                    DataSet ds = Methods.ExecuteStoredProcedure("spINGetDeleteSingleLeadsData", parameters);
                    _dtLeads = ds.Tables[0];
                    tbBatch.Text = ((DataRowView)cmbBatch.Items[cmbBatch.SelectedIndex]).Row.ItemArray[2].ToString();
                    //_dtBatch = ds.Tables[0];
                    //_dtAgents = ds.Tables[2];

                    _leads = new List<LeadViewModel>();
                    //foreach (DataRow row in _dtLeads.AsEnumerable())
                    //{
                    //    _leads
                    //        (new Lead
                    //        {
                    //            ImportID = row["ImportID"] as long?,
                    //            RefNumber = row["ReferenceNumber"] as string,
                    //            CampaignCode = row["CampaignCode"] as string,
                    //            BatchCode = row["BatchCode"] as string,
                    //            TSRAssignedTo = row["TSRAssignedTo"] as string,
                    //            IDNumber = row["IDNumber"] as string,
                    //            FirstName = row["FirstName"] as string,
                    //            Surname = row["Surname"] as string,
                    //        });
                    //}

                    foreach (DataRow row in _dtLeads.AsEnumerable())
                    {
                        Lead l = new Lead
                        {
                            ImportID = row["ImportID"] as long?,
                            RefNumber = row["ReferenceNumber"] as string,
                            CampaignCode = row["CampaignCode"] as string,
                            BatchCode = row["BatchCode"] as string,
                            TSRAssignedTo = row["TSRAssignedTo"] as string,
                            IDNumber = row["IDNumber"] as string,
                            FirstName = row["FirstName"] as string,
                            Surname = row["Surname"] as string,
                        };
                        _leads.Add(new LeadViewModel(l));
                        
                    }

                    DataContext = _dvm = new DataViewModel(_leads);
                    foreach(LeadViewModel lvm in _dvm.Leads)
                    {
                        lvm.PropertyChanged += LeadViewModel_OnPropertyChanged;
                    }
                    ((DataViewModel)DataContext).TotalChecked = xdgAssignLeads.RecordManager.GetFilteredInDataRecords().Where(x => ((LeadViewModel)x.DataItem).IsChecked).Count();

                    //tbCampaign.Text = _dtBatch.Rows[0]["CampaignName"].ToString();


                    //tbUDMBatch.Text = _dtBatch.Rows[0]["UDMBatchCode"].ToString();

                    //Batch2.Assigned = _dtBatch.Rows[0]["Assigned"] as int?;
                    //Batch2.UnAssigned = _dtBatch.Rows[0]["Unassigned"] as int?;

                    //tbTotalPrinted.Text = _dtBatch.Rows[0]["Printed"].ToString();
                }
                //else
                //{
                //    xdgAssignLeads.DataSource = null;
                //}

                //IsAllRecordsSelected = false;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SetCursor(Cursors.Arrow);
                //Cursor = Cursors.Arrow;
            }
        }

        #endregion

        private void cmbBatch_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Methods.EmbriantComboBoxPreviewKeyDown(sender, e);
        }

        void LeadViewModel_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ((DataViewModel)DataContext).TotalChecked = xdgAssignLeads.RecordManager.GetFilteredInDataRecords().Where(x => ((LeadViewModel)x.DataItem).IsChecked).Count();
        }
    }

}

