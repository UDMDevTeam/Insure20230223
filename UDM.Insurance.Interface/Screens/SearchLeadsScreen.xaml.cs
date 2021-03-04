using Infragistics.Windows;
using Infragistics.Windows.DataPresenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Embriant.Framework.Configuration;
using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    public partial class SearchLeadsScreen
    {

        #region Private Members

        DataTable _data;
        private readonly List<BackgroundWorker> _searchWorker = new List<BackgroundWorker>();
        private readonly SalesScreenGlobalData _ssGlobalData;

        #endregion



        #region Constructors

        public SearchLeadsScreen(SalesScreenGlobalData ssGlobalData)
        {
            InitializeComponent();
            _ssGlobalData = ssGlobalData;

            SearchControl.SearchChanged += SearchControl_SearchChanged;

            #if TESTBUILD
                TestControl.Visibility = Visibility.Visible;
            #elif DEBUG
                DebugControl.Visibility = Visibility.Visible;
            #endif
        }

        #endregion



        #region Private Methods

        void searchWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            {
                SearchControl.txtSearch.IsEnabled = false;
            });

            object[] argument = (object[])e.Argument;
            try
            {
                string searchTerm = (string)argument[1];

                if (searchTerm.Trim() != string.Empty)
                {
                    if (!((BackgroundWorker)argument[0]).CancellationPending)
                    {
                        SqlParameter[] parameters = new SqlParameter[2];
                        parameters[0] = new SqlParameter("@UserID", ((Business.User)GlobalSettings.ApplicationUser).ID);
                        parameters[1] = new SqlParameter("@Term1", searchTerm);
                        _data = Methods.ExecuteStoredProcedure("spINLeadSearch2", parameters).Tables[0];
                        e.Result = new[] { argument[0], _data };
                    }
                }
                else
                {
                    e.Result = new[] { argument[0], null };
                }

                if (((BackgroundWorker)argument[0]).CancellationPending)
                {
                    e.Cancel = true;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        void searchWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                object[] result = (object[])e.Result;

                if (e.Cancelled != true)
                {
                    if (result[1] != null)
                    {
                        if (result[1].GetType() == typeof(DataTable))
                        {
                            xdgLeads.DataSource = _data.DefaultView;

                            if (_data.Rows.Count <= 5000)
                            {
                                tbTotal.Text = _data.Rows.Count.ToString();
                            }
                            else
                            {
                                tbTotal.Text = "5000+";
                            }
                        }
                    }
                    else
                    {
                        xdgLeads.DataSource = null;
                        tbTotal.Text = "0";
                    }
                }
                _searchWorker.Remove((BackgroundWorker)result[0]);
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SearchControl.txtSearch.IsEnabled = true;
                SearchControl.txtSearch.Focus();
            }
        }

        #endregion



        #region Event Handlers

        void SearchControl_SearchChanged(string searchTerm)
        {
            try
            {
                BackgroundWorker worker = new BackgroundWorker();
                _searchWorker.Add(worker);

                worker.WorkerSupportsCancellation = true;
                worker.RunWorkerCompleted += searchWorker_RunWorkerCompleted;
                worker.DoWork += searchWorker_DoWork;
                worker.RunWorkerAsync(new object[] { worker, searchTerm });
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        } 

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            SalesScreen SalesScreen = new SalesScreen();
            OnClose(SalesScreen);
        }

        private void xdgLeads_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRecordCellArea drca = Utilities.GetAncestorFromType(e.OriginalSource as DependencyObject, typeof(DataRecordCellArea), false) as DataRecordCellArea;

            if (drca != null)
            {
                if (xdgLeads.ActiveRecord != null && xdgLeads.ActiveRecord.FieldLayout.Description == "Lead")
                {
                    DataRecord record = (DataRecord) xdgLeads.ActiveRecord;

                    long? fkINImportID = Int64.Parse(record.Cells["ImportID"].Value.ToString());

                    #region Determining whether or not the lead was allocated

                    bool hasLeadBeenAllocated = Business.Insure.HasLeadBeenAllocated(fkINImportID);

                    if (!hasLeadBeenAllocated)
                    {
                        INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                        ShowMessageBox(messageWindow, @"This lead cannot be loaded, because it has not been allocated yet. Please consult your supervisor.", "Lead not allocated", Embriant.Framework.ShowMessageType.Exclamation);
                        return;
                    }

                    #endregion Determining whether or not the lead was allocated

                    #region Checking if the lead to be loaded has a status of "DO NOT CONTACT"

                    bool clientCanBeContacted = Business.Insure.CanClientBeContacted(fkINImportID);

                    if (!clientCanBeContacted)
                    {
                        INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                        ShowMessageBox(messageWindow, @"This lead cannot be loaded in its entirety, because the client has requested not to be contacted again.", "DO NOT CONTACT CLIENT", Embriant.Framework.ShowMessageType.Exclamation);
                    }

                    #endregion Checking if the lead to be loaded has a status of "DO NOT CONTACT"

                    #region Determining whether or not the lead has a status of cancelled
                    // Please see https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/204734160/comments
                    // Please see https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/211742618/comments

                    //bool hasLeadBeenCancelled = Business.Insure.HasLeadBeenCancelled(fkINImportID);

                    //if (hasLeadBeenCancelled)
                    //{
                    //    INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                    //    ShowMessageBox(messageWindow, @"This lead cannot be loaded, because the policy has been cancelled by the client. Please consult your supervisor.", "Cancelled Policy", Embriant.Framework.ShowMessageType.Exclamation);
                    //    return;
                    //}

                    #endregion Determining whether or not the lead has a status of cancelled

                    LeadApplicationScreen leadApplicationScreen = new LeadApplicationScreen(long.Parse(record.Cells["ImportID"].Value.ToString()), _ssGlobalData);
                    ShowDialog(leadApplicationScreen, new INDialogWindow(leadApplicationScreen));
                    //leadApplicationScreen.ShowNotes(Int64.Parse(record.Cells["ImportID"].Value.ToString()));
                }
            }
        }

        private void SearchControl_Loaded(object sender, RoutedEventArgs e)
        {
            SearchControl.txtSearch.Focus();
            Keyboard.Focus(SearchControl.txtSearch);
        }

        #endregion

    }
}
