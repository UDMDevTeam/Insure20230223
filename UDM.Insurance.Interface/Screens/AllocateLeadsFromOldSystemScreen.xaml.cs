using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using Infragistics.Windows.DataPresenter;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using System.Data.SqlClient;
using System.Windows.Controls;
using Embriant.Framework;
using System.ComponentModel;
using Infragistics.Documents.Excel;
using Embriant.Framework.Configuration;
using System.Windows.Resources;
using System.Windows.Threading;
using System.Threading;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for AssignSummaryScreen.xaml
    /// </summary>
    public partial class AllocateLeadsFromOldSystemScreen
    {

        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        #region Old System Variables

        private DataRow _oldCampaign;
        private int _oldCampaignID;
        private string _oldCampaignTableName;
        private int _oldBatchID;
        private int _oldAllocationWeek;
        private int _oldEmployeeID;
        private DateTime _oldAllocationDateTime;

        #endregion Old System Variables

        #region New System Variables

        private DataRow _newCampaign;
        private long _newCampaignID;
        private string _newCampaignDescription;

        private DataRow _newBatch;
        private long _newBatchID;
        private string _newBatchDescription;

        private DataRow _newSalesAgent;
        private long _newSalesAgentID;
        private string _newSalesAgentDescription;

        private bool _isPrinted;

        #endregion New System Variables

        #endregion Private Members

        #region Constructors

        public AllocateLeadsFromOldSystemScreen()
        {
            InitializeComponent();

            LoadOldCampaigns();
            LoadNewCampaigns();
            LoadNewSalesAgents();

            #if TESTBUILD
                        TestControl.Visibility = Visibility.Visible;
            #else
                        TestControl.Visibility = Visibility.Collapsed;
            #endif
        }

        #endregion

        #region Private Methods

        #region Populating the controls that contain data from the old system 

        private void LoadOldCampaigns()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataTable dt = Methods.ExecuteStoredProcedure("spTempGetUDMAdminCampaigns", null, false).Tables[0];
                cmbOldCampaign.Populate(dt, "CampaignDescription", "CampaignID");
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

        private void LoadOldCampaignBatches(int oldCampaignID)
        {
            try
            {
                SetCursor(Cursors.Wait);

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@OldCampaignID", oldCampaignID);

                DataTable dt = Methods.ExecuteStoredProcedure("spTempGetUDMAdminBatches", parameters, false).Tables[0];

                cmbOldBatch.ItemsSource = null;
                cmbOldBatch.Populate(dt, "BatchDescription", "BatchID");
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

        private void LoadAllocationWeeks(int oldCampaignID, int oldBatchID)
        {
            try
            {
                SetCursor(Cursors.Wait);


                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@OldCampaignID", oldCampaignID);
                parameters[1] = new SqlParameter("@OldBatchID", oldBatchID);

                DataTable dt = Methods.ExecuteStoredProcedure("spTempGetUDMAllocationDates", parameters, false).Tables[0];
                cmbAllocationWeek.Populate(dt, "Week", "Week");
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

        private void LoadOldLeadAllocatedEmployees(int oldCampaignID, int oldBatchID, int allocationWeek)
        {
            try
            {
                SetCursor(Cursors.Wait);

                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@OldCampaignID", oldCampaignID);
                parameters[1] = new SqlParameter("@OldBatchID", oldBatchID);
                parameters[2] = new SqlParameter("@AllocationWeek", allocationWeek);

                DataTable dt = Methods.ExecuteStoredProcedure("spTempGetUDMLeadAllocatedEmployees", parameters, false).Tables[0];
                cmbOldSalesAgent.Populate(dt, "EmployeeDescription", "EmployeeID");
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

        private void LoadOldAllocationDates(int oldCampaignID, string oldCampaignTableName, int oldBatchID, int allocationWeek, int oldEmployeeID)
        {
            try
            {
                SetCursor(Cursors.Wait);

                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@OldCampaignID", oldCampaignID);
                parameters[1] = new SqlParameter("@OldCampaignTable", oldCampaignTableName);
                parameters[2] = new SqlParameter("@OldBatchID", oldBatchID);
                parameters[3] = new SqlParameter("@AllocationWeek", allocationWeek);
                parameters[4] = new SqlParameter("@OldEmployeeID", oldEmployeeID);

                DataTable dt = Methods.ExecuteStoredProcedure("spTempGetUDMEmployeeLeadAllocations", parameters, false).Tables[0];

                DataColumn column = new DataColumn("Select", typeof(bool));
                column.DefaultValue = false;
                dt.Columns.Add(column);
                //dt.DefaultView.Sort = "CampaignName ASC";
                //xdgEmployeeLeadAssignments.DataSource = dt.DefaultView;

                cmbOldAllocationDate.Populate(dt, "AssignDate", "AssignDate");
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

        #endregion Populating the controls that contain data from the old system

        #region Populating the controls that contain data from Insure

        private void LoadNewCampaigns()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataTable dt = Methods.ExecuteStoredProcedure("spTempGetInsureCampaigns", null, false).Tables[0];
                cmbNewCampaign.Populate(dt, "CampaignDescription", "ID");
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

        private void LoadNewCampaignBatches(long newCampaignID)
        {
            try
            {
                SetCursor(Cursors.Wait);

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@FKINCampaignID", newCampaignID);

                DataTable dt = Methods.ExecuteStoredProcedure("spTempGetInsureBatches", parameters, false).Tables[0];

                cmbNewBatch.ItemsSource = null;
                cmbNewBatch.Populate(dt, "BatchDescription", "ID");
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

        private void LoadNewSalesAgents()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataTable dt = Methods.ExecuteStoredProcedure("spTempGetInsureSalesAgents", null, false).Tables[0];
                cmbNewSalesAgent.Populate(dt, "SalesAgentDescription", "ID");
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

        #endregion Populating the controls that contain data from Insure

        //private bool? AllRecordsSelected()
        //{
        //    try
        //    {
        //        bool allSelected = true;
        //        bool noneSelected = true;

        //        if (xdgEmployeeLeadAssignments.DataSource != null)
        //        {
        //            foreach (DataRow dr in ((DataView)xdgEmployeeLeadAssignments.DataSource).Table.Rows)
        //            {
        //                allSelected = allSelected && (bool)dr["Select"];
        //                noneSelected = noneSelected && !(bool)dr["Select"];
        //            }
        //        }

        //        if (allSelected)
        //        {
        //            return true;
        //        }
        //        if (noneSelected)
        //        {
        //            return false;
        //        }

        //        return null;
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //        return null;
        //    }
        //}

        private bool HasAllInputParametersBeenSpecified()
        {
            #region UDM Admin parameters

            #region Old Campaign

            if (cmbOldCampaign.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select an old campaign.", "No old campaign selected", ShowMessageType.Error);
                EnableAllControls(true);
                return false;
            }

            #endregion Old Campaign

            #region Old Batch

            if (cmbOldBatch.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select an old batch.", "No old batch selected", ShowMessageType.Error);
                EnableAllControls(true);
                return false;
            }

            #endregion Old Batch

            #region Allocation Week

            if (cmbAllocationWeek.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select an allocation week.", "No allocation week selected", ShowMessageType.Error);
                EnableAllControls(true);
                return false;
            }

            #endregion Allocation Week

            #region Old Sales Agent

            if (cmbOldSalesAgent.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select an old sales agent.", "No old sales agent selected", ShowMessageType.Error);
                EnableAllControls(true);
                return false;
            }

            #endregion Old Sales Agent

            #region Old Allocation Date

            if (cmbOldAllocationDate.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select an old allocation date.", "No old allocation date selected", ShowMessageType.Error);
                EnableAllControls(true);
                return false;
            }

            #endregion Old Allocation Date

            #endregion UDM Admin parameters

            #region New System parameters

            #region New Campaign

            if (cmbNewCampaign.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select a new campaign.", "No new campaign selected", ShowMessageType.Error);
                EnableAllControls(true);
                return false;
            }

            #endregion New Campaign

            #region New Batch

            if (cmbNewBatch.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select a new batch.", "No new batch selected", ShowMessageType.Error);
                EnableAllControls(true);
                return false;
            }

            #endregion New Batch

            #region New Sales Agent

            if (cmbNewSalesAgent.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select a new sales agent.", "No new new sales agent selected", ShowMessageType.Error);
                EnableAllControls(true);
                return false;
            }

            #endregion New Sales Agent

            #region Is Printed

            if (chkFlagAsPrinted.IsChecked == null)
            {
                _isPrinted = false;
            }
            else
            {
                _isPrinted = chkFlagAsPrinted.IsChecked.Value;
            }

            #endregion Is Printed

            #endregion New System parameters

            return true;
        }

        private void EnableAllControls(bool isEnabled)
        {

            btnClose.IsEnabled = isEnabled;
            cmbOldCampaign.IsEnabled = isEnabled;
            cmbOldBatch.IsEnabled = isEnabled;
            cmbAllocationWeek.IsEnabled = isEnabled;
            cmbOldSalesAgent.IsEnabled = isEnabled;
            cmbOldAllocationDate.IsEnabled = isEnabled;

            cmbNewCampaign.IsEnabled = isEnabled;
            cmbNewBatch.IsEnabled = isEnabled;
            cmbNewSalesAgent.IsEnabled = isEnabled;

            btnAssignLeads.IsEnabled = isEnabled;

        }

        private void AssignLeads(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region First do the lead assignments

                int affectedRecordCount = 0;
                SqlParameter[] parameters = new SqlParameter[9];

                parameters[0] = new SqlParameter("@OldSystemUserID", _oldEmployeeID);
                parameters[1] = new SqlParameter("@OldSystemCampaignTableName", _oldCampaignTableName);
                parameters[2] = new SqlParameter("@OldSystemCampaignID", _oldCampaignID);
                parameters[3] = new SqlParameter("@OldSystemBatchID", _oldBatchID);
                parameters[4] = new SqlParameter("@AssignDate", _oldAllocationDateTime);

                parameters[5] = new SqlParameter("@NewSystemUserID", _newSalesAgentID);
                parameters[6] = new SqlParameter("@NewSystemCampaignID", _newCampaignID);
                parameters[7] = new SqlParameter("@NewSystemBatchID", _newBatchID);
                parameters[8] = new SqlParameter("@IsPrinted", _isPrinted);

                /*DataTable dt =*/
                affectedRecordCount = Methods.ExecuteUpdateStoredProcedure("spAssignLeadsFromOldToNew", parameters, false);

                //parameters = new SqlParameter[3];
                //parameters[0] = new SqlParameter("@AssignDate", _oldAllocationDateTime);
                //parameters[1] = new SqlParameter("@NewSystemUserID", _newSalesAgentID);
                //parameters[2] = new SqlParameter("@NewSystemBatchID", _newBatchID);

                //DataTable dt = Methods.ExecuteStoredProcedure("spTempGetAllocatedLeads", parameters, false).Tables[0];

                if (affectedRecordCount > 0) //dt.Rows.Count > 0)
                {
                    //LogOldSystemNewSystemLeadAssignment(affectedRecordCount);

                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), String.Format("{0} leads were successfully allocated to {1} from {2} of the {3} campaign.",
                            affectedRecordCount, // ), "Lead Allocations Successful", ShowMessageType.Information);
                            _newSalesAgentDescription,
                            _newBatchDescription,
                            _newCampaignDescription), "Lead Allocations Successful", ShowMessageType.Information);
                    });
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), String.Format("The lead allocations to {0} from batch {1} failed. Please verify the the selected values are correct and that correcponding leads were imported.",
                            _newSalesAgentDescription,
                            _newBatchDescription), "Lead Allocations Failed", ShowMessageType.Error);
                    });
                    return;
                }

                #endregion First do the lead assignments

            }  // This one will be removed when the report is added.

            #region Then, generate a report on the lead assignments

            //    int rowIndex;
            //    int worksheetCount = 0;
            //    //long campaignID = Convert.ToInt32(_campaign.ItemArray[0]);
            //    //string campaignName = _campaign.ItemArray[1].ToString();
            //    //string campaignDescription = String.Format("{0} ({1})", _campaign.ItemArray[1].ToString(), _campaign.ItemArray[2].ToString());
            //    //DataTable dtLeadStatusData;

            //    #region Setup excel documents

            //    Workbook wbTemplate;
            //    Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

            //    string filePathAndName = String.Format("{0}Lead Status Report ({1}), {2}.xlsx", GlobalSettings.UserFolder, campaignName, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

            //    Uri uri = new Uri("/Templates/ReportTemplateLeadStatus.xlsx", UriKind.Relative);
            //    StreamResourceInfo info = Application.GetResourceStream(uri);
            //    if (info != null)
            //    {
            //        wbTemplate = Workbook.Load(info.Stream, true);
            //    }
            //    else
            //    {
            //        return;
            //    }

            //    Worksheet wsTemplate = wbTemplate.Worksheets["Status"];
            //    Worksheet wsReport;
            //    //Worksheet wsReport = wbReport.Worksheets.Add(campaignName);
            //    //worksheetCount++;

            //    #endregion Setup excel documents

            //    foreach (DataRecord record in _lstSelectedAgents)
            //    {
            //        #region Get report data from database

            //        long batchID = Convert.ToInt64(record.Cells["ID"].Value);
            //        string batchDescription = record.Cells["Batch"].Value.ToString();

            //        SqlParameter[] parameters = new SqlParameter[2];
            //        parameters[0] = new SqlParameter("@CampaignID", campaignID);
            //        parameters[1] = new SqlParameter("@BatchID", batchID);

            //        DataSet dsLeadStatusData = Methods.ExecuteStoredProcedure("spINLeadStatusReport", parameters);


            //        #endregion Get report data from database

            //        if (dsLeadStatusData.Tables.Count > 0)
            //        {
            //            #region Report Data

            //            dtLeadStatusData = dsLeadStatusData.Tables[0];

            //            if (dtLeadStatusData.Rows.Count > 0)
            //            {
            //                rowIndex = 7;

            //                wsReport = wbReport.Worksheets.Add(batchDescription);
            //                worksheetCount++;

            //                #region Adding the details

            //                wsReport.GetCell("B3").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //                wsReport.GetCell("B4").Value = campaignDescription;

            //                #endregion Adding the details

            //                Methods.CopyExcelRegion(wsTemplate, 0, 0, 6, 16, wsReport, 0, 0);

            //                foreach (DataRow dr in dtLeadStatusData.Rows)
            //                {
            //                    Methods.CopyExcelRegion(wsTemplate, 6, 0, 1, 16, wsReport, rowIndex - 1, 0);

            //                    wsReport.GetCell("A" + rowIndex.ToString()).Value = dr["Batch Number"].ToString();
            //                    wsReport.GetCell("B" + rowIndex.ToString()).Value = dr["PL Reference Number"].ToString();
            //                    wsReport.GetCell("C" + rowIndex.ToString()).Value = dr["Name"].ToString();
            //                    wsReport.GetCell("D" + rowIndex.ToString()).Value = dr["Surname"].ToString();
            //                    wsReport.GetCell("E" + rowIndex.ToString()).Value = dr["Sale Status"].ToString();
            //                    wsReport.GetCell("F" + rowIndex.ToString()).Value = dr["Date of Sale"].ToString();
            //                    wsReport.GetCell("G" + rowIndex.ToString()).Value = dr["Original Premium Sold"].ToString();
            //                    wsReport.GetCell("H" + rowIndex.ToString()).Value = dr["Final Premium Sold"].ToString();
            //                    wsReport.GetCell("I" + rowIndex.ToString()).Value = dr["Decline Status"].ToString();
            //                    wsReport.GetCell("J" + rowIndex.ToString()).Value = dr["Date of decline"].ToString();
            //                    wsReport.GetCell("K" + rowIndex.ToString()).Value = dr["Decline Reason"].ToString();
            //                    wsReport.GetCell("L" + rowIndex.ToString()).Value = dr["Cancellation Status"].ToString();
            //                    wsReport.GetCell("M" + rowIndex.ToString()).Value = dr["Date of Cancellation"].ToString();
            //                    wsReport.GetCell("N" + rowIndex.ToString()).Value = dr["TSR Assigned To"].ToString();
            //                    wsReport.GetCell("O" + rowIndex.ToString()).Value = dr["TSR Sold By"].ToString();
            //                    wsReport.GetCell("P" + rowIndex.ToString()).Value = dr["Confirmation Agent"].ToString();

            //                    rowIndex++;
            //                }

            //            }
            //            #endregion Report Data

            //            //Save excel document
            //            wbReport.Save(filePathAndName);

            //            //Display excel document
            //            Process.Start(filePathAndName);

            //        }
            //        else
            //        {
            //            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            //            {
            //                ShowMessageBox(new INMessageBoxWindow1(), String.Format("There is no data for the {0} campaign.", campaignName), "No Data", ShowMessageType.Information);
            //            });

            //            continue;
            //        }
            //    }
            //}

            #endregion Then, generate a report on the lead assignments

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SetCursor(Cursors.Arrow);
            }

                
        }

        private void LogOldSystemNewSystemLeadAssignment(int leadAllocatedCount)
        {
            //try
            //{
                SqlParameter[] parameters = new SqlParameter[8];

                parameters[0] = new SqlParameter("@OldSystemEmployeeID", _oldEmployeeID);
                parameters[1] = new SqlParameter("@OldSystemCampaignID", _oldCampaignID);
                parameters[2] = new SqlParameter("@OldSystemBatchID", _oldBatchID);
                parameters[3] = new SqlParameter("@OldSystemAssignDate", _oldAllocationDateTime);
                parameters[4] = new SqlParameter("@NewSystemFKUserID", _newSalesAgentID);
                parameters[5] = new SqlParameter("@NewSystemCampaignID", _newCampaignID);
                parameters[6] = new SqlParameter("@NewSystemBatchID", _newBatchID);
                parameters[7] = new SqlParameter("@AllocatedLeadCount", leadAllocatedCount);

                Methods.ExecuteUpdateStoredProcedure("spTempRecordOldSystemNewSystemLeadAllocations", parameters, false);
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

        }

        private void AssignmentsCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnAssignLeads.Content = "AssignLeads";

            EnableAllControls(true);
        }

        #endregion Private Methods

        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //MenuToolsScreen menuToolsScreen = new MenuToolsScreen(ScreenDirection.Reverse);
            //OnClose(menuToolsScreen);
            MenuLeadScreen menuLeadScreen = new MenuLeadScreen(ScreenDirection.Reverse);
            OnClose(menuLeadScreen);
        }

        #region EmbriantComboBox SelectionChanged Events

        private void cmbOldCampaign_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbOldCampaign.SelectedIndex > -1)
                {
                    SetCursor(Cursors.Wait);

                    _oldCampaignID = Convert.ToInt32(cmbOldCampaign.SelectedValue);
                    _oldCampaign = (cmbOldCampaign.SelectedItem as DataRowView).Row;
                    _oldCampaignTableName = _oldCampaign.ItemArray[4].ToString();

                    //cmbOldBatch.SelectedValue = null;
                    //cmbOldBatch.DataContext = null;
                    LoadOldCampaignBatches(_oldCampaignID);
                    //LoadAllocationWeeks(_oldCampaignID);

                    //_campaign = (cmbOldCampaign.SelectedItem as DataRowView).Row;
                    
                }
                else
                {
                    
                    cmbOldBatch.SelectedValue = null;
                    cmbOldBatch.DataContext = null;

                    cmbAllocationWeek.DataContext = null;
                    _oldCampaignTableName = String.Empty;
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

        private void cmbOldBatch_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _oldBatchID = Convert.ToInt32(cmbOldBatch.SelectedValue);
            //_campaign = (cmbOldBatch.SelectedItem as DataRowView).Row;

            try
            {
                if (cmbOldBatch.SelectedIndex > -1)
                {
                    SetCursor(Cursors.Wait);

                    LoadAllocationWeeks(_oldCampaignID, _oldBatchID);
                }
                else
                {
                    cmbAllocationWeek.DataContext = null;
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

        private void cmbAllocationWeek_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _oldAllocationWeek = Convert.ToInt32(cmbAllocationWeek.SelectedValue);

            try
            {
                if ((cmbOldCampaign.SelectedIndex > -1) && (cmbOldBatch.SelectedIndex > -1)  && (cmbAllocationWeek.SelectedIndex > -1))
                {
                    SetCursor(Cursors.Wait);
                    LoadOldLeadAllocatedEmployees(_oldCampaignID, _oldBatchID, _oldAllocationWeek);
                }
                else
                {
                    cmbOldSalesAgent.DataContext = null;

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

        private void cmbOldSalesAgent_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _oldEmployeeID = Convert.ToInt32(cmbOldSalesAgent.SelectedValue);

            try
            {
                if ((cmbOldCampaign.SelectedIndex > -1) && 
                    (cmbAllocationWeek.SelectedIndex > -1) &&
                    (cmbOldBatch.SelectedIndex > -1) &&
                    (_oldCampaignTableName != String.Empty) &&
                    (cmbOldSalesAgent.SelectedIndex > -1))
                {
                    SetCursor(Cursors.Wait);
                    //LoadOldLeadAllocatedEmployees(_oldCampaignID, _oldAllocationWeek);
                    LoadOldAllocationDates(_oldCampaignID, _oldCampaignTableName, _oldBatchID, _oldAllocationWeek, _oldEmployeeID);
                }
                else
                {
                    //cmbOldSalesAgent.DataContext = null;
                    cmbOldAllocationDate.DataContext = null;
                    //xdgEmployeeLeadAssignments.DataSource = null;

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

        private void cmbOldAllocationDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _oldAllocationDateTime = Convert.ToDateTime(cmbOldAllocationDate.SelectedValue);
        }

        #endregion EmbriantComboBox SelectionChanged Events:

        #region XamDataGrid Events:

        //private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        //DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;
        //        DataTable dt = ((DataView)xdgEmployeeLeadAssignments.DataSource).Table;

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            dr["Select"] = true;
        //        }

        //        //EnableDisableExportButton();
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        //private void HeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        //DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;
        //        DataTable dt = ((DataView)xdgEmployeeLeadAssignments.DataSource).Table;

        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            dr["Select"] = false;
        //        }

        //        //EnableDisableExportButton();
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        //private void HeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        _xdgHeaderPrefixAreaCheckbox = (CheckBox)sender;
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        //private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (_xdgHeaderPrefixAreaCheckbox != null)
        //        {
        //            _xdgHeaderPrefixAreaCheckbox.IsChecked = AllRecordsSelected();
        //        }

        //        //EnableDisableExportButton();
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        #endregion XamDataGrid Events:

        private void cmbNewCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbNewCampaign.SelectedIndex > -1)
                {
                    SetCursor(Cursors.Wait);

                    _newCampaignID = Convert.ToInt64(cmbNewCampaign.SelectedValue);
                    _newCampaign = (cmbNewCampaign.SelectedItem as DataRowView).Row;
                    _newCampaignDescription = _newCampaign.ItemArray[3].ToString();
                    
                    LoadNewCampaignBatches(_newCampaignID);

                }
                else
                {
                    cmbNewBatch.SelectedValue = null;
                    cmbNewBatch.DataContext = null;
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

        private void cmbNewBatch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbNewBatch.SelectedIndex > -1)
                {
                    SetCursor(Cursors.Wait);

                    _newBatchID = Convert.ToInt64(cmbNewBatch.SelectedValue);
                    _newBatch = (cmbNewBatch.SelectedItem as DataRowView).Row;
                    _newBatchDescription = _newBatch.ItemArray[3].ToString();
                }
                //else
                //{
                //    cmbNewBatch.SelectedValue = null;
                //    cmbNewBatch.DataContext = null;
                //}
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

        private void cmbNewSalesAgent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbNewSalesAgent.SelectedIndex > -1)
                {
                    SetCursor(Cursors.Wait);

                    _newSalesAgentID = Convert.ToInt64(cmbNewSalesAgent.SelectedValue);
                    _newSalesAgent = (cmbNewSalesAgent.SelectedItem as DataRowView).Row;
                    _newSalesAgentDescription = _newSalesAgent.ItemArray[1].ToString();
                }
                //else
                //{
                //    cmbNewSalesAgent.SelectedValue = null;
                //    cmbNewSalesAgent.DataContext = null;
                //}
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

        private void btnAssignLeads_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EnableAllControls(false);

                if (!HasAllInputParametersBeenSpecified())
                {
                    EnableAllControls(true);
                    return;
                }

                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += AssignLeads;
                worker.RunWorkerCompleted += AssignmentsCompleted;
                worker.RunWorkerAsync();

                dispatcherTimer1.Start();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        //private void xdgAssignLeads_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    var xamDataGridControl = (XamDataGrid)sender;

        //    if (xamDataGridControl.ActiveRecord != null)
        //    {
        //        if (((FrameworkElement)e.MouseDevice.DirectlyOver).DataContext != null)
        //        {
        //            if ((((FrameworkElement)e.MouseDevice.DirectlyOver).DataContext).GetType().FullName == "Infragistics.Windows.DataPresenter.DataRecord")
        //            {
        //                DataRecord currentRecord;
        //                DataRow drCurrentRecord;
        //                int indexCampaign, indexBatch, indexAgent;

        //                switch (xamDataGridControl.ActiveRecord.FieldLayout.Description)
        //                {
        //                    case "Campaign":

        //                        break;

        //                    case "Batch":
        //                        currentRecord = (DataRecord)xamDataGridControl.ActiveRecord;
        //                        drCurrentRecord = ((DataRowView)currentRecord.DataItem).Row;
        //                        indexCampaign = currentRecord.ParentDataRecord.Index;
        //                        indexBatch = currentRecord.Index;

        //                        AssignLeadsScreen assignLeadsScreen = new AssignLeadsScreen(Convert.ToInt64(drCurrentRecord.ItemArray[0].ToString()));
        //                        ShowDialog(assignLeadsScreen, new INDialogWindow(assignLeadsScreen));

        //                        LoadSummaryData();
        //                        GridLastView(xamDataGridControl, indexCampaign, indexBatch, null);
        //                        break;

        //                    //case "Agent":
        //                    //    currentRecord = (DataRecord)xamDataGridControl.ActiveRecord;
        //                    //    drCurrentRecord = ((DataRowView)currentRecord.DataItem).Row;
        //                    //    indexCampaign = currentRecord.ParentDataRecord.ParentDataRecord.Index;
        //                    //    indexBatch = currentRecord.ParentDataRecord.Index;
        //                    //    indexAgent = currentRecord.Index;

        //                    //    PrintLeadsScreen printLeadsScreen = new PrintLeadsScreen(Convert.ToInt64(drCurrentRecord.ItemArray[1].ToString()), Convert.ToInt64(drCurrentRecord.ItemArray[2].ToString()));
        //                    //    ShowDialog(printLeadsScreen, new INDialogWindow(printLeadsScreen));

        //                    //    LoadSummaryData();
        //                    //    GridLastView(xamDataGridControl, indexCampaign, indexBatch, indexAgent);
        //                    //    break;
        //                }
        //            }
        //        }
        //    }
        //}

        //private void xdgAssignLeads_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    //if (e.Key == Key.Return)
        //    //{
        //    //    var xamDataGridControl = (XamDataGrid)sender;

        //    //    if (xamDataGridControl.ActiveRecord != null)
        //    //    {
        //    //        DataRecord currentRecord = (DataRecord)xamDataGridControl.ActiveRecord;
        //    //        DataRow drCurrentRecord = ((DataRowView)currentRecord.DataItem).Row;

        //    //        _currentRecord = currentRecord.Index;

        //    //        //UserDetailsScreen userDetailsScreen = new UserDetailsScreen(Convert.ToInt64(drCurrentRecord.ItemArray[0].ToString()));
        //    //        //ShowDialog(userDetailsScreen, new HRDialogWindow(userDetailsScreen));

        //    //        //xdgUsers.DataSource = null;
        //    //        LoadBatchData();
        //    //    }
        //    //}
        //}

        #endregion

    }
}
