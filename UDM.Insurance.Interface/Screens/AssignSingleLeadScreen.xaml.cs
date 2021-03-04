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

namespace UDM.Insurance.Interface.Screens
{
    public partial class AssignSingleLeadScreen : INotifyPropertyChanged
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
                set
                {
                    SetProperty(ref _isFilteredOut, value, () => IsFilteredOut);
                    if (_isFilteredOut)
                    {
                        IsChecked = false;
                    }
                }
            }

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

        public class DataViewModel : INotifyPropertyChanged
        {
            public DataViewModel(ObservableCollection<Lead> leads)
            {
                Leads = leads;

                foreach (Lead lead in Leads)
                {
                    lead.PropertyChanged += delegate
                    {
                        OnPropertyChanged("AllLeadsAreChecked");
                    };
                }
            }

            private ObservableCollection<Lead> _leads;
            public ObservableCollection<Lead> Leads
            {
                get
                {
                    TotalChecked = 0;
                    foreach (Lead lead in _leads)
                    {
                        if (lead.IsChecked)
                        {
                            TotalChecked++;
                        }
                    }

                    return _leads;
                }
                private set
                {
                    _leads = value;
                    OnPropertyChanged("Leads");
                }
            }

            public bool? AllLeadsAreChecked
            {
                get
                {
                    bool? value = null;
                    for (int i = 0; i < Leads.Count; ++i)
                    {
                        if (i == 0)
                        {
                            value = Leads[0].IsChecked;
                        }
                        else if (value != Leads[i].IsChecked)
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

                    foreach (Lead member in Leads)
                    {
                        if (!member.IsFilteredOut)
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
                    _totalChecked = value;
                    OnPropertyChanged("TotalChecked");
                }
            }

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            void OnPropertyChanged(string prop)
            {
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
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

        private readonly long _batchID;
        private DataTable _dtBatch;
        private DataTable _dtLeads;
        private DataTable _dtAgents;
        private ObservableCollection<Lead> _leads;
        private DataViewModel _dvm;
        private BackgroundWorker _worker;

        #endregion



        #region Constructors

        public AssignSingleLeadScreen(long batchID)
        {
            InitializeComponent();

            _batchID = batchID;

            LoadLookupData();
        }

        #endregion



        #region Private Methods

        private void LoadLookupData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("BatchID", _batchID);

                DataSet ds = Methods.ExecuteStoredProcedure("spINGetBatchAssignSingleLeadsData", parameters);
                _dtBatch = ds.Tables[0];
                _dtLeads = ds.Tables[1];
                _dtAgents = ds.Tables[2];

                _leads = new ObservableCollection<Lead>();
                foreach (DataRow row in _dtLeads.AsEnumerable())
                {
                    _leads.Add(new Lead
                    {
                        ImportID = row["ImportID"] as long?,
                        RefNumber = row["RefNumber"] as string,
                        IDNumber = row["IDNumber"] as string,
                        FirstName = row["FirstName"] as string,
                        Surname = row["Surname"] as string,
                    });
                }

                DataContext = _dvm = new DataViewModel(_leads);

                tbCampaign.Text = _dtBatch.Rows[0]["CampaignName"].ToString();
                tbBatch.Text = _dtBatch.Rows[0]["BatchCode"].ToString();
                tbUDMBatch.Text = _dtBatch.Rows[0]["UDMBatchCode"].ToString();

                Batch2.Assigned = _dtBatch.Rows[0]["Assigned"] as int?;
                Batch2.UnAssigned = _dtBatch.Rows[0]["Unassigned"] as int?;

                tbTotalPrinted.Text = _dtBatch.Rows[0]["Printed"].ToString();

                cmbAgent.Populate(_dtAgents, "SalesAgent", "UserID");
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

        private void AllocateCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_dvm != null)
            {
                bool test1 = _dvm.TotalChecked > 0;

                bool test2 = cmbAgent.SelectedValue != null;

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
                acAllocate.Text = "_Cancel";
                AllocateLeads();
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
                    long? userID = E.Argument as long?;
                    //long? totalLeads = _leads.Count(lead => lead.IsChecked);
                    int count = 0;

                    Database.BeginTransaction(null, IsolationLevel.Snapshot);

                    foreach (Lead lead in _leads.Where(lead => lead.IsChecked))
                    {
                        count++;

                        if (lead.ImportID != null)
                        {
                            INImport import = new INImport((long)lead.ImportID);

                            import.FKUserID = userID;
                            import.AllocationDate = DateTime.Now;

                            import.Save(_validationResult);
                        }

                        //Thread.Sleep(1000);

                        if (_worker.CancellationPending)
                        {
                            Database.CancelTransactions();
                            E.Cancel = true;
                            return;
                        }

                        //int progressPercentage = Convert.ToInt32(((double)count / totalLeads) * 100);
                        BackgroundWorker backgroundWorker = Sender as BackgroundWorker;
                        if (backgroundWorker != null) backgroundWorker.ReportProgress(count);
                    }

                    CommitTransaction(null);
                }

                catch (Exception ex)
                {
                    Methods.HandleException(ex, this);
                }
            };

            _worker.ProgressChanged += (Sender, E) =>
            {
                //pbAllocate.Value = E.ProgressPercentage;
                //btnAllocate.Content = E.ProgressPercentage;

                Batch2.Assigned++;
                Batch2.UnAssigned--;
            };

            _worker.RunWorkerCompleted += (Sender, E) =>
            {
                LoadLookupData();
                acAllocate.Text = "_Allocate";
                IsAllocating = false;
                xdgAssignLeads.Focus();
            };

            _worker.RunWorkerAsync(cmbAgent.SelectedValue);
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
            XamDataGrid xamDataGrid = sender as XamDataGrid;
            if (xamDataGrid != null)
            {
                List<Record> records = xamDataGrid.Records.ToList();
                foreach (Record record in records)
                {
                    if (record != null)
                    {
                        DataRecord dataRecord = (DataRecord) record;
                        if (dataRecord.IsDataRecord)
                        {
                            ((Lead) dataRecord.DataItem).IsFilteredOut = Convert.ToBoolean(dataRecord.IsFilteredOut);
                        }
                    }
                }
            }
        }

        private void cmbAgent_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Methods.EmbriantComboBoxPreviewKeyDown(sender, e);
        }

        private void xdgAssignLeads_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fe = e.MouseDevice.DirectlyOver as FrameworkElement;
            if (fe != null && fe.GetType().Name == "XamDataGrid")
            {
                if (xdgAssignLeads != null && xdgAssignLeads.ActiveRecord != null)
                {
                    RecordPresenter rp = RecordPresenter.FromRecord(xdgAssignLeads.ActiveRecord);

                    if (rp != null)
                    {
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
            if (e.Key == Key.Space)
            {
                FrameworkElement fe = e.OriginalSource as FrameworkElement;
                if (fe != null && fe.GetType().Name == "DataRecordCellArea")
                {
                    if (xdgAssignLeads != null && xdgAssignLeads.ActiveRecord != null)
                    {
                        RecordPresenter rp = RecordPresenter.FromRecord(xdgAssignLeads.ActiveRecord);

                        if (rp != null)
                        {
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
        }

        #endregion

    }

}

