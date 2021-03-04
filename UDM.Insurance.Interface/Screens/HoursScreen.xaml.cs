using Embriant.Framework.Configuration;
using Infragistics.Windows;
using Infragistics.Windows.DataPresenter;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{

    public partial class HoursScreen : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #region Constants



        #endregion Constants


        #region Private Members

        private User _loggedInUser;
        private long? _agentID; //= GlobalSettings.ApplicationUser.ID;

        private System.Windows.Threading.DispatcherTimer dispatcherTimer1 = new System.Windows.Threading.DispatcherTimer();
        private long _currentRecord = -1;
        public static long _employeeIndex = -1;
        private bool _captureButtonClicked = false;

        private lkpUserType? _userType;

        #endregion Private Members


        #region Publicly Encapsulated Members

        public lkpUserType? UserType
        {
            get
            {
                return _userType;
            }
            set
            {
                _userType = value;
                OnPropertyChanged("UserType");
            }
        }

        #endregion Publicly Encapsulated Members


        #region Constructors

        public HoursScreen(ScreenDirection direction)
        {
            UserType = lkpUserType.SalesAgent;
            InitializeComponent();

            LoadAgentDetails();

            //LoadSalesData();

            _loggedInUser = ((User)GlobalSettings.ApplicationUser);

            #if TESTBUILD
                            TestControl.Visibility = Visibility.Visible;
            #elif DEBUG
                        DebugControl.Visibility = Visibility.Visible;
            #endif
        }

        #endregion Constructors


        #region Private Methods

        private void LoadAgentDetails()
        {
            try
            {
                SetCursor(Cursors.Wait);

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@IncludeAllConsultantsUser", 1);
                DataTable dt = Methods.ExecuteStoredProcedure("spINGetHoursScreenUsers", parameters).Tables[0];
                cmbStaff.Populate(dt, "SalesAgentDescription", "ID");

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

        private void LoadRecentlyLoadedHoursForSelectedAgent(long? agentID)
        {
            try
            {
                SetCursor(Cursors.Wait);

                if (agentID != null)
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@UserID", _agentID);

                    DataSet dsResult = Methods.ExecuteStoredProcedure("spINGetHoursCapturedForUser", parameters);

                    if (dsResult.Tables.Count > 0)
                    {
                        xdgCapturedHours.DataSource = dsResult.Tables[0].DefaultView;
                    }
                }
                else
                {
                    xdgCapturedHours.DataSource = null;
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

        private void LoadHoursCaptureScreen(object sender)
        {
            try
            {
                var xamDataGridControl = (XamDataGrid)sender;

                if (xamDataGridControl.ActiveRecord != null)
                {
                    DataRecord currentRecord = (DataRecord)xamDataGridControl.ActiveRecord;
                    DataRow drCurrentRecord = ((DataRowView)currentRecord.DataItem).Row;

                    _currentRecord = currentRecord.Index;

                    if (_captureButtonClicked)
                    {
                        CaptureSalesAgentHoursScreen captureSalesAgentHoursScreen = new CaptureSalesAgentHoursScreen(0);

                        ShowDialog(captureSalesAgentHoursScreen, new INDialogWindow(captureSalesAgentHoursScreen));
                    }
                    else
                    {
                        CaptureSalesAgentHoursScreen captureSalesAgentHoursScreen = new CaptureSalesAgentHoursScreen(Convert.ToInt64(drCurrentRecord.ItemArray[0].ToString()));
                        ShowDialog(captureSalesAgentHoursScreen, new INDialogWindow(captureSalesAgentHoursScreen));
                    }
                }
                else
                {
                    _currentRecord = -1;

                    CaptureSalesAgentHoursScreen captureSalesAgentHoursScreen = new CaptureSalesAgentHoursScreen(0);
                    ShowDialog(captureSalesAgentHoursScreen, new INDialogWindow(captureSalesAgentHoursScreen));
                }

                //return from leave capture dialog
                _captureButtonClicked = false;

                if (cmbStaff.SelectedIndex != -1 && _employeeIndex == -1)
                {
                    _currentRecord = -1;
                    cmbStaff.SelectedIndex = -1;
                    dispatcherTimer1.Start();
                }
                else
                {
                    if (cmbStaff.SelectedIndex != _employeeIndex)
                    {
                        _currentRecord = -1;
                    }

                    cmbStaff.SelectedIndex = -1;
                    cmbStaff.SelectedIndex = (int)_employeeIndex;

                    if (cmbStaff.SelectedIndex == -1)
                    {
                        dispatcherTimer1.Start();
                    }
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void DeleteUserHoursRecord(long userHoursRecordID)
        {
            try
            {
                SetCursor(Cursors.Wait);

                UserHours userHoursRecord = new UserHours(userHoursRecordID);
                userHoursRecord.Delete(_validationResult);

                if (_validationResult.Passed)
                {
                    if (cmbStaff.SelectedIndex != -1)
                    {
                        LoadRecentlyLoadedHoursForSelectedAgent(userHoursRecordID);
                    }
                    else
                    {
                        xdgCapturedHours.DataSource = null;
                    }

                    ShowMessageBox(new INMessageBoxWindow1(), "The user hours record was successfully deleted.", "Record Deletion Successful", Embriant.Framework.ShowMessageType.Information);
                }
                else
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "The user hours record was NOT successfully deleted.", "Record Deletion Not Successful", Embriant.Framework.ShowMessageType.Error);
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

        //private void InboxLastView(XamDataGrid xamDataGrid)
        //{
        //    xamDataGrid.Records.CollapseAll(true);

        //    foreach (DataRecord recordCampaign in xamDataGrid.Records)
        //    {
        //        if (recordCampaign.Cells[1].Value.ToString().Trim() == _strCampaign) //Campaign Level
        //        {
        //            xamDataGrid.ActiveRecord = recordCampaign;
        //            xamDataGrid.ActiveRecord.IsExpanded = true;

        //            if (xamDataGrid.ActiveRecord.HasChildren)
        //            {
        //                foreach (DataRecord recordBatch in xamDataGrid.ActiveRecord.ViewableChildRecords[0].ViewableChildRecords)
        //                {
        //                    if (recordBatch.Cells[2].Value.ToString().Trim() == _strBatch)
        //                    {
        //                        xamDataGrid.ActiveRecord = recordBatch;
        //                        xamDataGrid.ActiveRecord.IsExpanded = true;

        //                        int count = recordBatch.ViewableChildRecords[0].ViewableChildRecords.Count;

        //                        if (_iLead < 11)
        //                        {
        //                            xamDataGrid.BringRecordIntoView(recordBatch.ViewableChildRecords[0].ViewableChildRecords[_iLead]);
        //                        }
        //                        else if (_iLead + 11 < count)
        //                        {
        //                            xamDataGrid.BringRecordIntoView(recordBatch.ViewableChildRecords[0].ViewableChildRecords[_iLead + 11]);
        //                        }
        //                        else if (_iLead + 12 > count)
        //                        {
        //                            xamDataGrid.BringRecordIntoView(recordBatch.ViewableChildRecords[0].ViewableChildRecords[count - 1]);
        //                        }

        //                        xamDataGrid.ActiveRecord = recordBatch.ViewableChildRecords[0].ViewableChildRecords[_iLead];
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        #endregion Private Methods


        #region Public Methods

        //public void LoadRecentlyLoadedHoursForSelectedAgent(long? agentID)
        //{
        //    try
        //    {
        //        SetCursor(Cursors.Wait);

        //        if (agentID != null)
        //        {
        //            SqlParameter[] parameters = new SqlParameter[1];
        //            parameters[0] = new SqlParameter("@UserID", _agentID);

        //            DataSet dsResult = Methods.ExecuteStoredProcedure("spINGetHoursCapturedForUser", parameters);

        //            if (dsResult.Tables.Count > 0)
        //            {
        //                xdgCapturedHours.DataSource = dsResult.Tables[0].DefaultView;
        //            }
        //        }
        //        else
        //        {
        //            xdgCapturedHours.DataSource = null;
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


        //public void RefreshRecentlyLoadedHoursForSelectedAgent()
        //{

        //}

        #endregion Public Methods


        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((_loggedInUser.FKUserType == (long)lkpUserType.Administrator) || (_loggedInUser.FKUserType == (long)lkpUserType.Manager))
                {
                    MenuToolsScreen menuToolsScreen = new MenuToolsScreen(ScreenDirection.Reverse);
                    OnClose(menuToolsScreen);
                }
                else
                {
                    StartScreen startScreen = new StartScreen();
                    OnClose(startScreen);
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            //try
            //{
            //    foreach (Window window in Application.Current.Windows)
            //    {
            //        if (window.Name == "Diary")
            //        {
            //            window.Close();
            //        }
            //    }

            //    if ((_agent.FKUserType == (long) lkpUserType.Administrator) || (_agent.FKUserType == (long) lkpUserType.Manager))
            //    {
            //        MenuToolsScreen menuToolsScreen = new MenuToolsScreen(ScreenDirection.Reverse);
            //        OnClose(menuToolsScreen);
            //    }
            //    else
            //    {
            //        StartScreen startScreen = new StartScreen();
            //        OnClose(startScreen);
            //    }
            //}

            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
        }

        private void btnCaptureHours_Click(object sender, RoutedEventArgs e)
        {
            //if (_agentID != null)
            //{
            //    if (_agentID.Value == 176)
            //    {
            //        ShowMessageBox(new INMessageBoxWindow1(), @"Hours may not be logged for the 'All Consultants' sales agent. Please select a different sales agent from the drop-down.", "Invalid sales agent selected", Embriant.Framework.ShowMessageType.Error);
            //        return;
            //    }
            //}

            _captureButtonClicked = true;
            LoadHoursCaptureScreen(xdgCapturedHours);
        }

        private void EmbriantComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cmbStaff_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _agentID = UDM.Insurance.Interface.Data.CommonControlData.GetObjectIDFromSelectedComboBoxItem(cmbStaff);
            LoadRecentlyLoadedHoursForSelectedAgent(_agentID);
        }

        #region XamDataGrid-Specific Event Handlers

        private void xdgCapturedHours_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //DataRecordCellArea drca = Utilities.GetAncestorFromType(e.OriginalSource as DependencyObject, typeof(DataRecordCellArea), false) as DataRecordCellArea;

                //if (drca != null)
                //{
                    if (xdgCapturedHours.ActiveRecord != null)
                    {
                        DataRecord record = (DataRecord)xdgCapturedHours.ActiveRecord;

                        //LeadApplicationScreen leadApplicationScreen = new LeadApplicationScreen(Int64.Parse(record.Cells["ImportID"].Value.ToString()));
                        //ShowDialog(leadApplicationScreen, new INDialogWindow(leadApplicationScreen));

                        CaptureSalesAgentHoursScreen captureSalesAgentHoursScreen = new CaptureSalesAgentHoursScreen(Int64.Parse(record.Cells["ID"].Value.ToString()));
                        ShowDialog(captureSalesAgentHoursScreen, new INDialogWindow(captureSalesAgentHoursScreen));

                        //LoadSalesData();

                        //InboxLastView((XamDataGrid)sender);
                    }
                //}
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void xdgCapturedHours_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (xdgCapturedHours.ActiveRecord != null)
                {
                    DataRecord record = (DataRecord)xdgCapturedHours.ActiveRecord;
                    long recordID = Int64.Parse(record.Cells["ID"].Value.ToString());
                    string salesAgentDescription = record.Cells["AgentName"].Value.ToString();
                    DateTime? workingDate = DateTime.Parse(record.Cells["WorkingDate"].Value.ToString());
                    string campaign = record.Cells["Campaign"].Value.ToString();

                    bool result = false;
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate
                    {
                        INMessageBoxWindow2 messageBox = new INMessageBoxWindow2();
                        messageBox.buttonOK.Content = "Yes";
                        messageBox.buttonCancel.Content = "No";
                        //string message = String.Format("The working hours of {0} were already captured for {1}. To update his/her hours, click Update, otherwise click Cancel.",
                        string message = String.Format("Are you sure you wish to delete the selected record of the working hours of {0} who was working on the {1} campaign on {2}?{3}{3}Warning: This action can not be undone.", 
                            salesAgentDescription,
                            campaign,
                            workingDate.Value.ToString("yyyy-MM-dd"),
                            Environment.NewLine);

                        var showMessageBox = ShowMessageBox(messageBox, message, "Confirm record deletion", Embriant.Framework.ShowMessageType.Exclamation);

                        result = showMessageBox != null && (bool)showMessageBox;
                    });

                    if (result)
                    {
                        DeleteUserHoursRecord(recordID);
                        //affectedRecordCount += UpdateExistingRecord(rowID);
                    }
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        #endregion XamDataGrid-Specific Event Handlers

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadRecentlyLoadedHoursForSelectedAgent(_agentID);
        }

        //private void HoursScreen_OnLoaded(object sender, RoutedEventArgs e)
        //{

        //}

        //private void xdgCapturedHours_RecordActivated(object sender, Infragistics.Windows.DataPresenter.Events.RecordActivatedEventArgs e)
        //{
        //    try
        //    {
        //        DataRecord record = (DataRecord)e.Record;

        //        switch (record.NestingDepth)
        //        {
        //            case 0:
        //                _strCampaign = record.Cells[1].Value.ToString().Trim();
        //                break;

        //            case 2:
        //                _strBatch = record.Cells[2].Value.ToString().Trim();
        //                break;

        //            case 4:
        //                //_strLead = record.Cells[3].Value.ToString().Trim();
        //                _iLead = record.Index;
        //                break;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        //private void xdgCapturedHours_RecordExpanding(object sender, Infragistics.Windows.DataPresenter.Events.RecordExpandingEventArgs e)
        //{
        //    try
        //    {
        //        DataRecord record = (DataRecord)e.Record;

        //        switch (record.NestingDepth)
        //        {
        //            case 0:
        //                _strCampaign = record.Cells[1].Value.ToString().Trim();
        //                break;

        //            case 2:
        //                _strBatch = record.Cells[2].Value.ToString().Trim();
        //                break;

        //            case 4:
        //                //_strLead = record.Cells[3].Value.ToString().Trim();
        //                _iLead = record.Index;
        //                break;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        #endregion Event Handlers
        
    }
}
