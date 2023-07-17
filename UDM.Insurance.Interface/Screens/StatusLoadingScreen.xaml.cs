using System.Linq;
using Embriant.Framework.Configuration;
using Infragistics.Windows;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.Editors;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    public partial class StatusLoadingScreen
    {

        #region Members

        private bool isForward = true;
        private readonly DataTable _dtLeadStatus;
        private readonly DataTable _dtDeclineReason;
        private readonly DataTable _dtLatentLeadReason;
        private long _userID;
        private long _leadBookID;

        #endregion Members

        #region Constructors

        public StatusLoadingScreen()
        {
            InitializeComponent();

            //LoadLookups();

            DataSet dsStatusLoadingScreenLookups = Insure.INGetStatusLoadingScreenLookups();
            DataTable dtAgents = dsStatusLoadingScreenLookups.Tables[0];
            _dtLeadStatus = dsStatusLoadingScreenLookups.Tables[1];
            _dtDeclineReason = dsStatusLoadingScreenLookups.Tables[2];
            _dtLatentLeadReason = dsStatusLoadingScreenLookups.Tables[3];

            cmbAgent.Populate(dtAgents, "Description", "ID");
        }

        #endregion Constructors


        #region Methods

        //private void LoadLookups()
        //{
        //    //DataSet dsStatusLoadingScreenLookups = Insure.INGetStatusLoadingScreenLookups();
        //    //DataTable dtAgents = dsStatusLoadingScreenLookups.Tables[0];
        //    //_dtLeadStatus = dsStatusLoadingScreenLookups.Tables[1];
        //    //_dtDeclineReason = dsStatusLoadingScreenLookups.Tables[2];
        //    //_dtLatentLeadReason = dsStatusLoadingScreenLookups.Tables[3];

        //    //cmbAgent.Populate(dtAgents, "Description", "ID");

        //    //_dtLeadStatus = INLeadStatusMapper.ListData(false, null).Tables[0];
        //    //_dtDeclineReason = INDeclineReasonMapper.ListData(false, null).Tables[0];

        //    //DataTable dtAgents = Methods.ExecuteStoredProcedure("spGetSalesAgents", null).Tables[0];
        //    //cmbAgent.Populate(dtAgents, "Description", "ID");
        //}

        private void PopulateLeadGrid(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                xdgLeads.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart) delegate
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@LeadBookID", _leadBookID);
                    xdgLeads.DataSource = Methods.ExecuteStoredProcedure("spGetLeadsForStatusLoading", parameters).Tables[0].DefaultView;
                    CalculateCaptured();
                });
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void PopulateLeadGridCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Arrow);
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

        private void cmbAgent_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                CalculateCaptured();
                _userID = Convert.ToInt64(cmbAgent.SelectedValue);

                //DataTable dtLeadBook = INLeadBookMapper.SearchData(_userID, null, null, null).Tables[0];
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@FKUserID", _userID);
                DataTable dtLeadBook = Methods.ExecuteStoredProcedure("spINGetStatusLoadingLeadBook26Weeks", parameters).Tables[0];

                dtLeadBook.DefaultView.Sort = "Description DESC"; //Sort by Title
                cmbLeadBook.Populate(dtLeadBook, "Description", "ID");
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmbLeadBook_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                CalculateCaptured();
                _leadBookID = Convert.ToInt64(cmbLeadBook.SelectedValue);

                BackgroundWorker inboxWorker = new BackgroundWorker();
                inboxWorker.DoWork += PopulateLeadGrid;
                inboxWorker.RunWorkerCompleted += PopulateLeadGridCompleted;
                inboxWorker.RunWorkerAsync(GlobalSettings.ApplicationUser.ID);
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        #region XamDataGrid - Related

        private void xdgLeads_Loaded(object sender, RoutedEventArgs e)
        {
            CalculateCaptured();
            cmbAgent.Focus();
        }

        private void xdgLeads_CellActivated(object sender, Infragistics.Windows.DataPresenter.Events.CellActivatedEventArgs e)
        {
            try
            {
                CalculateCaptured();
                DataRecord currentRecord = (DataRecord) xdgLeads.ActiveRecord;
                DataRow drCurrentRecord = ((DataRowView) currentRecord.DataItem).Row;

                string strLeadStatus = currentRecord.Cells["LeadStatus"].Value as string;
                long? idLeadStatus = ((from r in _dtLeadStatus.AsEnumerable() where r.Field<string>("Description").Equals(strLeadStatus) select r.Field<long?>("ID"))).FirstOrDefault();

                if (currentRecord.Index == (xdgLeads.Records.Count - 1))
                {
                    isForward = false;
                }
                else if (currentRecord.Index == 0)
                {
                    isForward = true;
                }

                if (e.Cell.Field.Settings.AllowEdit == false)
                {
                    xdgLeads.ExecuteCommand(isForward ? DataPresenterCommands.CellNextByTab : DataPresenterCommands.CellPreviousByTab);
                    e.Handled = true;
                }
                else if (e.Cell.Field.Name == "DeclineReasonCode")
                {
                    if (idLeadStatus != 2)
                    {
                        xdgLeads.ExecuteCommand(isForward ? DataPresenterCommands.CellNextByTab : DataPresenterCommands.CellPreviousByTab);
                        e.Handled = true;
                    }
                }
                else if (e.Cell.Field.Name == "LeadStatus")
                {
                    if (idLeadStatus == 1)
                    {
                        e.Cell.EndEditMode();
                        xdgLeads.ExecuteCommand(isForward ? DataPresenterCommands.CellBelow : DataPresenterCommands.CellAbove);
                        e.Handled = true;
                    }
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void CalculateCaptured()
        {
            int capturedCount = 0;
            if (xdgLeads.DataSource != null)
            {
                foreach (DataRowView r in xdgLeads.DataSource)
                {

                    if (r.Row["IsChecked"].ToString() == "1")
                    {
                        capturedCount++;
                    }
                }
            }
            lblTotalCaptures.Text = "Total Captured:" + capturedCount;
            lblTotalCapturedToday.Text = "Total Captured Today:" + Methods.GetTableData("SELECT * FROM dbo.INImport INNER JOIN dbo.INLeadBookImport ON dbo.INImport.ID = dbo.INLeadBookImport.FKINImportID where dbo.INLeadBookImport.FKINLeadBookID = "+_leadBookID + " AND dbo.INImport.CheckedDate >='"+DateTime.Now.ToShortDateString()+"'").Rows.Count;
        }

        private void xdgLeads_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                CalculateCaptured();
                XamDataGrid grid = sender as XamDataGrid;
                XamTextEditor activeTextEditor = null;
                int inputLength = 0;

                if (grid != null)
                {
                    if (grid.ActiveCell != null)
                    {
                        #region Get active texteditor

                        switch (grid.ActiveCell.Field.Name)
                        {
                            case "LeadStatus":
                            case "DeclineReasonCode":
                            case "LatentLeadReason1":
                            case "LatentLeadReason2":
                            case "LatentLeadReason3":
                                activeTextEditor = Utilities.GetDescendantFromType(CellValuePresenter.FromCell(grid.ActiveRecord.DataPresenter.ActiveCell), typeof (XamTextEditor), true) as XamTextEditor;
                                inputLength = NoNull(grid.ActiveCell.Value.ToString(), String.Empty).ToString().Length;
                                break;
                        }

                        #endregion Get active texteditor

                        #region Navigate with the arrow keys

                        if (e.Key == Key.Right || e.Key == Key.Left || e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Tab)
                        {
                            if (inputLength != 1)
                            {
                                grid.ExecuteCommand(DataPresenterCommands.EndEditModeAndAcceptChanges);

                                if (e.Key == Key.Right || e.Key == Key.Tab)
                                {
                                    isForward = true;
                                    grid.ExecuteCommand(DataPresenterCommands.CellRight);
                                }

                                if (e.Key == Key.Left)
                                {
                                    isForward = false;
                                    grid.ExecuteCommand(DataPresenterCommands.CellLeft);
                                }

                                if (e.Key == Key.Down)
                                {
                                    isForward = true;
                                    grid.ExecuteCommand(DataPresenterCommands.CellBelow);
                                }

                                if (e.Key == Key.Up)
                                {
                                    isForward = false;
                                    grid.ExecuteCommand(DataPresenterCommands.CellAbove);
                                }

                                grid.ExecuteCommand(DataPresenterCommands.StartEditMode);
                            }
                        }

                        #endregion
                        
                        #region Translate the typed code

                        if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9))
                        {
                            if (activeTextEditor != null)
                            {
                                if (activeTextEditor.Value != null && activeTextEditor.Value.ToString().Length == 1)
                                {
                                    activeTextEditor.Value = activeTextEditor.Value + e.Key.ToString().Substring(e.Key.ToString().Length - 1, 1);
                                    switch (grid.ActiveCell.Field.Name)
                                    {
                                        case "LeadStatus":
                                            DataRow[] drLeadStatus = _dtLeadStatus.Select("CodeNumber = " + activeTextEditor.Value);
                                            activeTextEditor.Value = drLeadStatus.Length > 0 ? drLeadStatus[0].ItemArray[1] : null;
                                            break;

                                        case "DeclineReasonCode":
                                            DataRow[] drDeclineReasonCode = _dtDeclineReason.Select("CodeNumber = " + activeTextEditor.Value);
                                            activeTextEditor.Value = drDeclineReasonCode.Length > 0 ? drDeclineReasonCode[0].ItemArray[1] : null;
                                            break;

                                        case "LatentLeadReason1":
                                        case "LatentLeadReason2":
                                        case "LatentLeadReason3":
                                            DataRow[] drLatentLeadReasonCode = _dtLatentLeadReason.Select("[CodeNumber] = " + activeTextEditor.Value);
                                            activeTextEditor.Value = drLatentLeadReasonCode.Length > 0 ? drLatentLeadReasonCode[0].ItemArray[2] : null;
                                            break;
                                    }
                                    if (activeTextEditor.Value != null) activeTextEditor.SelectionStart = activeTextEditor.Value.ToString().Length;
                                }
                                else
                                {
                                    activeTextEditor.Value = e.Key.ToString().Substring(e.Key.ToString().Length - 1, 1);
                                    if (activeTextEditor.Value != null) activeTextEditor.SelectionStart = 1;
                                }
                            }
                        }

                        #endregion

                        #region Clear the contents of the text field

                        if (e.Key == Key.Back)
                        {// && (string)activeTextEditor.Value != "Accepted" && (string)activeTextEditor.Value != "Carried Forward"
                            if (activeTextEditor != null)
                            {
                                activeTextEditor.Value = String.Empty;
                            }
                        }

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
                                        case "LeadStatus":
                                        case "DeclineReasonCode":
                                        case "LatentLeadReason1":
                                        case "LatentLeadReason2":
                                        case "LatentLeadReason3":
                                            activeTextEditor = Utilities.GetDescendantFromType(CellValuePresenter.FromCell(grid.ActiveRecord.DataPresenter.ActiveCell), typeof (XamTextEditor), true) as XamTextEditor;
                                            if (activeTextEditor != null) activeTextEditor.EndEditMode(true, true);
                                            break;
                                    }
                                }

                                xdgLeads_RecordDeactivating(null, null);
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        return;
                    }
                }

                e.Handled = true;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void xdgLeads_RecordDeactivating(object sender, Infragistics.Windows.DataPresenter.Events.RecordDeactivatingEventArgs e)
        {
            try
            {
                CalculateCaptured();
                long _importID;
                DataRecord currentRecord = (DataRecord) xdgLeads.ActiveRecord;

                _importID = Convert.ToInt64(currentRecord.Cells["ImportID"].Value);
                INImport inImport = new INImport(_importID);

                //Get lead status ID
                string strLeadStatus = currentRecord.Cells["LeadStatus"].Value as string;
                long? idLeadStatus = ((from r in _dtLeadStatus.AsEnumerable() where r.Field<string>("Description").Equals(strLeadStatus) select r.Field<long?>("ID"))).FirstOrDefault();
                inImport.FKINLeadStatusID = idLeadStatus;
                
                //IsChekced
                bool isChecked = Convert.ToBoolean(currentRecord.Cells["IsChecked"].Value);// as bool?;
                inImport.IsChecked = isChecked;
                inImport.CheckedDate = inImport.IsChecked == true ? (DateTime?) DateTime.Now.Date : null;

                if (idLeadStatus == null) { currentRecord.Cells["LeadStatus"].Value = null; }

                //Clear Decline Reason Code if not LeadStatus Declined
                if ((lkpINLeadStatus?) idLeadStatus != lkpINLeadStatus.Declined)
                {
                    currentRecord.Cells["DeclineReasonCode"].Value = null;
                }

                //Get decline reason code ID
                string strDeclineReason = currentRecord.Cells["DeclineReasonCode"].Value as string;
                long? idDeclineReason = ((from r in _dtDeclineReason.AsEnumerable() where r.Field<string>("Code").Equals(strDeclineReason) select r.Field<long?>("ID"))).FirstOrDefault();
                inImport.FKINDeclineReasonID = idDeclineReason;
                if (idDeclineReason == null) { currentRecord.Cells["DeclineReasonCode"].Value = null; }

                //Set diary reason ID to default '1' which is call back
                if (idLeadStatus == 9)
                {
                    inImport.FKINDiaryReasonID = 1;
                }

                inImport.Save(_validationResult);

                #region Save the 3 reasons

                string latentLeadReason1Code = currentRecord.Cells["LatentLeadReason1"].Value as string;
                string latentLeadReason2Code = currentRecord.Cells["LatentLeadReason2"].Value as string;
                string latentLeadReason3Code = currentRecord.Cells["LatentLeadReason3"].Value as string;

                // At least 1 latent lead reason must be specified in order for saving to happen:
                if (!((latentLeadReason1Code == null) || (latentLeadReason2Code == null) || (latentLeadReason3Code == null)))
                {
                    long? fkINImportLatentLeadReasonID = currentRecord.Cells["FKINImportLatentLeadReasonID"].Value as long?;
                    long? fkINImportLatentLeadReasonID1 = ((from r in _dtLatentLeadReason.AsEnumerable() where r.Field<string>("Code").Equals(latentLeadReason1Code) select r.Field<long?>("ID"))).FirstOrDefault();
                    long? fkINImportLatentLeadReasonID2 = ((from r in _dtLatentLeadReason.AsEnumerable() where r.Field<string>("Code").Equals(latentLeadReason2Code) select r.Field<long?>("ID"))).FirstOrDefault();
                    long? fkINImportLatentLeadReasonID3 = ((from r in _dtLatentLeadReason.AsEnumerable() where r.Field<string>("Code").Equals(latentLeadReason3Code) select r.Field<long?>("ID"))).FirstOrDefault();

                    INImportLatentLeadReason inImportLatentLeadReason = fkINImportLatentLeadReasonID != null ? new INImportLatentLeadReason((long)fkINImportLatentLeadReasonID) : new INImportLatentLeadReason();
                    inImportLatentLeadReason.FKINImportID = _importID;
                    inImportLatentLeadReason.FKINLatentLeadReasonID1 = fkINImportLatentLeadReasonID1;
                    inImportLatentLeadReason.FKINLatentLeadReasonID2 = fkINImportLatentLeadReasonID2;
                    inImportLatentLeadReason.FKINLatentLeadReasonID3 = fkINImportLatentLeadReasonID3;

                    inImportLatentLeadReason.Save(_validationResult);
                }

                #endregion Save the 3 reasons

            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        #endregion XamDataGrid - Related

        private void Field_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        #endregion Event Handlers

    }
}