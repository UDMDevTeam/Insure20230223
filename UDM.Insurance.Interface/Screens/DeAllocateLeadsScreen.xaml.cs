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
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for DeAllocateLeadsScreen.xaml
    /// </summary>
    public partial class DeAllocateLeadsScreen
    {

        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        private DataRow _campaign;
        private long _campaignID;
        private string _campaignDescription;

        private DataRow _batch;
        private long _batchID;
        private string _batchDescription;

        private DataRow _salesConsultant;
        private long _salesConsultantID;
        private string _salesConsultantDescription;

        private DateTime _allocationDate;

        private bool _canBeDeAllocated = false;
        private string _messageBoxText;
        private string _messageBoxHeaderText;

        private bool _commit = false;
        private int _countSelected;

        private List<Record> _lstSelectedINImportIDs;
        private string _inImportIDs;

        private long _fkINLeadDeAllocationReasonID;
        private long _fkRequestorUserID;

        private string _note;

        #endregion Private Members

        #region Excel Worksheet-Related Properties

        private readonly System.Drawing.Color _redForegroundColour = System.Drawing.Color.FromArgb(156, 0, 6);
        private readonly System.Drawing.Color _redBackgroundColour = System.Drawing.Color.FromArgb(255, 199, 206);

        #endregion Excel Worksheet-Related Properties

        #region Constructors

        public DeAllocateLeadsScreen()
        {
            InitializeComponent();

            LoadLookups();

            //if (!IsLoggedInAsDeveloper())
            if (! Insure.INIsAuthorizedToDeAllocateLeads())
            {
                tbRequestor.Visibility = Visibility.Collapsed;
                cmbRequestor.Visibility = Visibility.Collapsed;
            }
            else
            {
                tbRequestor.Visibility = Visibility.Visible;
                cmbRequestor.Visibility = Visibility.Visible;
            }

#if TESTBUILD
                        TestControl.Visibility = Visibility.Visible;
#else
            TestControl.Visibility = Visibility.Collapsed;
#endif
        }

        #endregion

        #region Private Methods

        #region Populating the controls that contain data from Insure

        private void LoadLookups()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataSet ds = Insure.INGetLeadDeallocationScreenLookups();

                DataTable dtCampaigns = ds.Tables[0];
                cmbCampaign.Populate(dtCampaigns, "Name", "ID");

                DataTable dtRequestors = ds.Tables[1];
                cmbRequestor.Populate(dtRequestors, "User", "ID");

                DataTable dtDeAllocationReasons = ds.Tables[2];
                cmbDeAllocationReasons.Populate(dtDeAllocationReasons, "Description", "ID");
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

        private void LoadCampaignBatches(long campaignID)
        {
            try
            {
                SetCursor(Cursors.Wait);

                //SqlParameter[] parameters = new SqlParameter[1];
                //parameters[0] = new SqlParameter("@FKINCampaignID", newCampaignID);

                //DataTable dt = Methods.ExecuteStoredProcedure("spTempGetInsureBatches", parameters, false).Tables[0];
                DataTable dt = Insure.INGetBatchesByCampaignID(campaignID);

                cmbBatch.ItemsSource = null;
                cmbBatch.Populate(dt, "Code", "ID");
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

        private void LoadSalesAgents(long batchID)
        {
            try
            {
                SetCursor(Cursors.Wait);

                //DataTable dt = Methods.ExecuteStoredProcedure("spTempGetInsureSalesAgents", null, false).Tables[0];
                //cmbSalesAgent.Populate(dt, "SalesAgentDescription", "ID");
                DataTable dt = Insure.INGetBatchAssignees(batchID);
                cmbSalesConsultant.Populate(dt, "SalesConsultant", "FKUserID");
                
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

        private void LoadAllocationDates(long batchID, long userID)
        {
            try
            {
                SetCursor(Cursors.Wait);

                //DataTable dt = Methods.ExecuteStoredProcedure("spTempGetInsureSalesAgents", null, false).Tables[0];
                //cmbSalesAgent.Populate(dt, "SalesAgentDescription", "ID");
                DataTable dt = Insure.INGetAllocationDatesByBatchIDAndUserID(batchID, userID);
                cmbAllocationDates.Populate(dt, "AllocationDate", "AllocationDate");
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

        private void LoadLeadsToBeDeallocated(long batchID, long userID, DateTime allocationDate)
        {
            DataSet ds = Insure.INGetDeAllocatableLeadDetails(batchID, userID, allocationDate);
            DataTable dt = ds.Tables[0];
            xdgLeadsToBeDeAllocated.DataSource = dt.DefaultView;

            int totalLeads = 0;
            int cleanLeads = 0;
            int usedLeads = 0;
            string status;
            string statusForegroundColour;

            // Display the details:
            DataTable dtDetails = ds.Tables[1];
            if (dtDetails.Rows.Count > 0)
            {
                totalLeads = Convert.ToInt32(dtDetails.Rows[0]["TotalLeads"]);
                cleanLeads = Convert.ToInt32(dtDetails.Rows[0]["CleanLeads"]);
                usedLeads = Convert.ToInt32(dtDetails.Rows[0]["UsedLeads"]);
                _canBeDeAllocated = Convert.ToBoolean(dtDetails.Rows[0]["CanBeDeAllocated"]);
                status = dtDetails.Rows[0]["Status"].ToString();
                statusForegroundColour = dtDetails.Rows[0]["StatusForegroundColour"].ToString();

                tbSummary.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString(statusForegroundColour));
                tbSummary.Text = String.Format("{0} total lead(s). {1} clean lead(s). {2} used lead(s).", totalLeads, cleanLeads, usedLeads);

                tbStatus.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString(statusForegroundColour));
                tbStatus.Text = status;

                _messageBoxText = dtDetails.Rows[0]["MessageBoxText"].ToString();
                _messageBoxHeaderText = dtDetails.Rows[0]["MessageBoxHeaderText"].ToString();

                if (cleanLeads == 0)
                {
                    //ShowMessageBox(new INMessageBoxWindow1(), _messageBoxText, _messageBoxHeaderText, ShowMessageType.Exclamation);
                    btnDeAllocateLeads.IsEnabled = false;
                }
                else
                {
                    btnDeAllocateLeads.IsEnabled = true;
                }
            }

            UpdateSelectedLeadCount();
        }

        private void UpdateSelectedLeadCount()
        {
            _countSelected = (xdgLeadsToBeDeAllocated.Records.Select(r => (bool)((DataRecord)r).Cells["Select"].Value)).Count(b => b);
            tbSelectedLeadCount.Text = String.Format("{0} selected leads", _countSelected);
        }

        #endregion Populating the controls that contain data from Insure

        private bool HasAllInputParametersBeenSpecified()
        {

            #region Campaign

            if (cmbCampaign.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select a new campaign.", "No campaign selected", ShowMessageType.Error);
                EnableAllControls(true);
                return false;
            }

            #endregion Campaign

            #region Batch

            if (cmbBatch.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select a batch.", "No batch selected", ShowMessageType.Error);
                EnableAllControls(true);
                return false;
            }

            #endregion Batch

            #region Sales Agent

            if (cmbSalesConsultant.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select a sales consultant.", "No sales consultant selected", ShowMessageType.Error);
                EnableAllControls(true);
                return false;
            }

            #endregion Sales Agent

            #region Allocation Date

            if (cmbAllocationDates.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select an allocation date.", "No allocation date selected", ShowMessageType.Error);
                EnableAllControls(true);
                return false;
            }

            #endregion Allocation Date

            var lstTemp = (from r in xdgLeadsToBeDeAllocated.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            _lstSelectedINImportIDs = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["INImportID"].Value));

            if (_lstSelectedINImportIDs.Count == 0)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please 1 lead to be deallocated.", "No lead selected", Embriant.Framework.ShowMessageType.Error);
                return false;
            }
            else
            {
                _inImportIDs = _lstSelectedINImportIDs.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["INImportID"].Value + ",");
                _inImportIDs = _inImportIDs.Substring(0, _inImportIDs.Length - 1);
            }

            #region Ensuring that the de-allocation reason has been specified

            if (cmbDeAllocationReasons.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please specify the reason for leads being de-allocated.", "No reason selected", ShowMessageType.Error);
                EnableAllControls(true);
                return false;
            }
            else
            {
                _fkINLeadDeAllocationReasonID = Convert.ToInt64(cmbDeAllocationReasons.SelectedValue);

            }

            #endregion Ensuring that the de-allocation reason has been specified

            #region (Admin Account Only) Ensuring that the requestor has been specified

            //if (IsLoggedInAsDeveloper())
            if (!Insure.INIsAuthorizedToDeAllocateLeads())
            {
                if (cmbRequestor.SelectedValue == null)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please indicate who requested the leads to be de-allocated.", "No requestor selected", ShowMessageType.Error);
                    EnableAllControls(true);
                    return false;
                }
                else
                {
                    _fkRequestorUserID = Convert.ToInt64(cmbRequestor.SelectedValue);
                }
            }
            else
            {
                _fkRequestorUserID = GlobalSettings.ApplicationUser.ID;
            }

            #endregion (Admin Account Only) Ensuring that the requestor has been specified

            #region Note

            //if (txtNotes.Text.Trim().Length > 0)
            //{
                _note = txtNotes.Text.Trim();
            //}
            //else
            //{
            //    _note = null;
            //}

            #endregion Note

            #region Can the leads be de-allocated?

            if (!_canBeDeAllocated)
            {

            }

            #endregion Can the leads be de-allocated?

            #region Commit the transaction?

            if (!chkTestDeAllocation.IsChecked.HasValue)
            {
                _commit = false;
            }
            else
            {
                _commit = !chkTestDeAllocation.IsChecked.Value;
            }

            #endregion Commit the transaction?

            return true;
        }

        private void EnableAllControls(bool isEnabled)
        {
            btnClose.IsEnabled = isEnabled;

            cmbCampaign.IsEnabled = isEnabled;
            cmbBatch.IsEnabled = isEnabled;
            cmbSalesConsultant.IsEnabled = isEnabled;
            cmbAllocationDates.IsEnabled = isEnabled;
            xdgLeadsToBeDeAllocated.IsEnabled = isEnabled;
            chkTestDeAllocation.IsEnabled = isEnabled;

            btnDeAllocateLeads.IsEnabled = isEnabled;
        }

        private void ResetControls(bool resetCampaignCombobox, bool resetBatchCombobox, bool resetSalesConsultantCombobox, bool resetAllocationDatesCombobox, bool resetLeadsDataGrid, bool resetSummaryTextBlock, bool resetStatusTextBlock, bool resetReasonCombobox, bool resetRequestorCombobox, bool resetNotesTextBox)
        {
            //DeAllocateLeadsScreen deAllocateLeadsScreen = new DeAllocateLeadsScreen();
            //OnClose(deAllocateLeadsScreen);

            if (resetCampaignCombobox)
            {
                cmbCampaign.SelectedValue = null;
                //cmbCampaign.SelectedIndex = -1;
            }

            if (resetBatchCombobox)
            {
                cmbBatch.SelectedValue = null;
                cmbBatch.ItemsSource = null;
            }

            if (resetSalesConsultantCombobox)
            {
                cmbSalesConsultant.SelectedValue = null;
                cmbSalesConsultant.ItemsSource = null;
            }

            if (resetAllocationDatesCombobox)
            {
                cmbAllocationDates.SelectedValue = null;
                cmbAllocationDates.ItemsSource = null;
            }

            if (resetLeadsDataGrid)
            {
                xdgLeadsToBeDeAllocated.DataSource = null;
            }

            if (resetSummaryTextBlock)
            {
                tbSummary.Text = String.Empty;
            }

            if (resetStatusTextBlock)
            {
                tbStatus.Text = String.Empty;
            }

            if (resetReasonCombobox)
            {
                cmbDeAllocationReasons.SelectedValue = null;
            }

            if (resetRequestorCombobox)
            {
                cmbRequestor.SelectedValue = null;
            }

            if (resetNotesTextBox)
            {
                txtNotes.Text = String.Empty;
            }

            chkTestDeAllocation.IsChecked = true;
            _countSelected = 0;
        }

        private void DeAllocateLeads(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region First do the lead de-allocations

                DataSet dsLeadDeAllocations = Insure.INDeAllocateLeads(_batchID, _salesConsultantID, _allocationDate, _inImportIDs, _fkINLeadDeAllocationReasonID, _fkRequestorUserID, _note, _commit);


                //if (affectedRecordCount > 0) //dt.Rows.Count > 0)
                //{
                //    //LogOldSystemNewSystemLeadAssignment(affectedRecordCount);

                //    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                //    {
                //        ShowMessageBox(new INMessageBoxWindow1(), String.Format("{0} leads were successfully allocated to {1} from {2} of the {3} campaign.",
                //            affectedRecordCount, // ), "Lead Allocations Successful", ShowMessageType.Information);
                //            _salesConsultantDescription,
                //            _batchDescription,
                //            _campaignDescription), "Lead Allocations Successful", ShowMessageType.Information);
                //    });
                //}
                //else
                //{
                //    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                //    {
                //        ShowMessageBox(new INMessageBoxWindow1(), String.Format("The lead allocations to {0} from batch {1} failed. Please verify the the selected values are correct and that correcponding leads were imported.",
                //            _salesConsultantDescription,
                //            _batchDescription), "Lead Allocations Failed", ShowMessageType.Error);
                //    });
                //    return;
                //}

                

                #endregion First do the lead de-allocations

                #region Then, generate a report on the lead assignments

                bool successful = GenerateLeadDeAllocationReport(dsLeadDeAllocations, _commit);

                if (_commit)
                {
                    if (successful)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "The leads were successfully de-allocated", "Lead De-Allocations Successful", ShowMessageType.Information);

                            DeAllocateLeadsScreen deAllocateLeadsScreen = new DeAllocateLeadsScreen();
                            OnClose(deAllocateLeadsScreen);
                        });
                    }
                    else
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "The lead de-allocations were unsuccessful", "Lead De-Allocations Failed", ShowMessageType.Error);
                        });
                    }
                }
                else
                {
                    if (successful)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "Testing of the lead de-allocations were successfully completed. Please consult the generated Excel workbook. The actual de-allocation can now be performed", "Testing Successful", ShowMessageType.Information);
                            chkTestDeAllocation.IsChecked = false;
                        });
                    }
                    else
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "Testing of the lead de-allocations was unsuccessful", "Testing Failed", ShowMessageType.Error);
                        });
                    }
                }

                #endregion Then, generate a report on the lead assignments

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

        private bool GenerateLeadDeAllocationReport(DataSet dsReportData, bool commit)
        {
            #region Don't generate the report of there are no data

            if (dsReportData == null)
            {
                return false;
            }

            if (dsReportData.Tables.Count < 10)
            {
                return false;
            }

            if (dsReportData.Tables[6].Rows.Count == 0)
            {
                return false;
            }

            #endregion Don't generate the report of there are no data

            #region Partition the given DataSet into various DataTables

            DataTable dtSalesConsultantDetails = dsReportData.Tables[0];
            DataTable dtLeadbookDetails = dsReportData.Tables[1];
            DataTable dtCampaignDetails = dsReportData.Tables[2];
            DataTable dtBatchDetails = dsReportData.Tables[3];
            DataTable dtRequestorDetails = dsReportData.Tables[4];
            DataTable dtDeAllocationReasonDetails = dsReportData.Tables[5];
            DataTable dtOriginalLeadsDetails = dsReportData.Tables[6];
            DataTable dtAffectedRecordsDetails = dsReportData.Tables[7];
            DataTable dtDeallocatedLeads = dsReportData.Tables[8];
            DataTable dtRemainingLeads = dsReportData.Tables[9];

            #endregion Partition the given DataSet into various DataTables

            #region Declarations and initializations

            int reportRow = 3;
            int reportTemplateRowIndex = 17;
            int reportTemplateColumnSpan = 80;
            DateTime reportDate = DateTime.Now;
            string reportHeading = String.Empty; //"Lead De-Allocations";
            string currentSectionHeadingText = String.Empty;
            string currentSectionDataText = null;

            if (_commit)
            {
                reportHeading = "Lead De-Allocations";
            }
            else
            {
                reportHeading = "Lead De-Allocations (Testing)";
            }
            
            #endregion Declarations and initializations

            #region Setup Excel document

            Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateLeadDeAllocations.xlsx");
            Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
            string filePathAndName = String.Format("{0}Insure {1} {2}.xlsx", GlobalSettings.UserFolder, reportHeading, reportDate.ToString("yyyy-MM-dd HHmmss"));

            string sheetName = String.Empty;

            if (dtSalesConsultantDetails.Rows.Count == 0)
            {
                sheetName = reportDate.ToString("yyyy-MM-dd HHmmss");
            }
            else
            {
                sheetName = Methods.ParseWorksheetName(wbReport, dtSalesConsultantDetails.Rows[0]["User"].ToString());
            }

            Worksheet wsLeadDeAllocationReportTemplate = wbTemplate.Worksheets["Template"];
            Worksheet wsNewReportSheet = wbReport.Worksheets.Add(sheetName);
            Methods.CopyWorksheetOptionsFromTemplate(wsLeadDeAllocationReportTemplate, wsNewReportSheet, true, true, true, true, true, true, true, false, true, true, true, true, true, true, true, true, false);

            #endregion Setup Excel document

            #region Step 1: Copy a region from the template that consists of the headings

            Methods.CopyExcelRegion(wsLeadDeAllocationReportTemplate, 0, 0, 12, reportTemplateColumnSpan, wsNewReportSheet, 0, 0);

            #endregion Step 1: Copy a region from the template that consists of the headings

            #region Step 2: Populate the report details

            wsNewReportSheet.GetCell(String.Format("A1")).Value = reportHeading;

            wsNewReportSheet.GetCell(String.Format("I{0}", reportRow + 1)).Value = dtSalesConsultantDetails.Rows[0]["UserID"];
            wsNewReportSheet.GetCell(String.Format("BO{0}", reportRow + 1)).Value = dtBatchDetails.Rows[0]["BatchID"];

            reportRow += 2;
            wsNewReportSheet.GetCell(String.Format("I{0}", reportRow + 1)).Value = dtSalesConsultantDetails.Rows[0]["User"];
            wsNewReportSheet.GetCell(String.Format("BO{0}", reportRow + 1)).Value = dtBatchDetails.Rows[0]["Batch"];

            reportRow += 2;
            wsNewReportSheet.GetCell(String.Format("I{0}", reportRow + 1)).Value = dtLeadbookDetails.Rows[0]["LeadBookID"];
            wsNewReportSheet.GetCell(String.Format("BO{0}", reportRow + 1)).Value = dtCampaignDetails.Rows[0]["CampaignID"];

            reportRow += 2;
            wsNewReportSheet.GetCell(String.Format("I{0}", reportRow + 1)).Value = dtLeadbookDetails.Rows[0]["LeadBook"];
            wsNewReportSheet.GetCell(String.Format("BO{0}", reportRow + 1)).Value = dtCampaignDetails.Rows[0]["Campaign"];

            reportRow += 2;
            wsNewReportSheet.GetCell(String.Format("I{0}", reportRow + 1)).Value = reportDate.ToString("yyyy-MM-dd HH:mm:ss");
            wsNewReportSheet.GetCell(String.Format("BO{0}", reportRow + 1)).Value = dtRequestorDetails.Rows[0]["Requestor"];

            //wsNewReportSheet.GetCell("A8").Value = String.Format("From {0} to {1}", fromDate.ToString("dddd, d MMMM yyyy"), toDate.ToString("dddd, d MMMM yyyy"));
            //wsNewReportSheet.GetCell("G10").Value = String.Format("{0} ({1})", dtUserDetails.Rows[0]["FullName"].ToString(), dtUserDetails.Rows[0]["EmployeeNo"].ToString());
            //wsNewReportSheet.GetCell("BP10").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            #endregion Step 2: Populate the report details

            #region Step 3: Show the original batch

            currentSectionHeadingText = "Original Batch";
            reportRow += 2;
            reportRow = InsertDetailsSection(wsLeadDeAllocationReportTemplate, wsNewReportSheet, dtOriginalLeadsDetails, reportRow, currentSectionHeadingText, null, true);

            #endregion Step 3: Show the original batch

            #region Step 4: Show the deallocated leads

            reportRow++;
            currentSectionHeadingText = "De-Allocated Leads";
            if (dtDeallocatedLeads.Rows.Count > 0)
            {
                if (dtOriginalLeadsDetails.Rows.Count == dtDeallocatedLeads.Rows.Count)
                {
                    currentSectionDataText = "All of the leads allocated to the sales consultant were de-allocated.";
                }
                else
                {
                    currentSectionDataText = null;
                }
            }
            else
            {
                currentSectionDataText = "None of the leads that were allocated to the sales consultant were de-allocated.";
            }

            reportRow = InsertDetailsSection(wsLeadDeAllocationReportTemplate, wsNewReportSheet, dtDeallocatedLeads, reportRow, currentSectionHeadingText, currentSectionDataText, false);

            #endregion Step 4: Show the deallocated leads

            #region Step 5: Show the residual leads

            reportRow++;
            currentSectionHeadingText = "Remaining Leads";

            if (dtRemainingLeads.Rows.Count > 0)
            {
                if (dtOriginalLeadsDetails.Rows.Count == dtDeallocatedLeads.Rows.Count)
                {
                    currentSectionDataText = "None of the leads that were allocated to the sales consultant were de-allocated.";
                }
                else
                {
                    currentSectionDataText = null;
                }
            }
            else
            {
                currentSectionDataText = "None of the leads that were originally allocated to the sales consultant remain.";
            }

            reportRow = InsertDetailsSection(wsLeadDeAllocationReportTemplate, wsNewReportSheet, dtRemainingLeads, reportRow, currentSectionHeadingText, currentSectionDataText, true);

            #endregion Step 5: Show the residual leads

            #region Step 6: Show the summary

            reportRow += 2;
            reportTemplateRowIndex = 20;

            Methods.CopyExcelRegion(wsLeadDeAllocationReportTemplate, reportTemplateRowIndex, 0, 3, reportTemplateColumnSpan, wsNewReportSheet, reportRow, 0);
            reportRow += 3;

            wsNewReportSheet.GetCell(String.Format("A{0}", reportRow + 1)).Value = dtOriginalLeadsDetails.Rows.Count;
            wsNewReportSheet.GetCell(String.Format("E{0}", reportRow + 1)).Value = dtDeallocatedLeads.Rows.Count;
            wsNewReportSheet.GetCell(String.Format("J{0}", reportRow + 1)).Value = dtRemainingLeads.Rows.Count;

            #endregion Step 6: Show the summary

            #region Step 7: De-Allocation Reason

            reportTemplateRowIndex = 25;
            reportRow += 3;

            Methods.CopyExcelRegion(wsLeadDeAllocationReportTemplate, reportTemplateRowIndex, 0, 3, reportTemplateColumnSpan, wsNewReportSheet, reportRow, 0);
            wsNewReportSheet.GetCell(String.Format("E{0}", reportRow + 1)).Value = dtDeAllocationReasonDetails.Rows[0]["Reason"];
            wsNewReportSheet.GetCell(String.Format("E{0}", reportRow + 3)).Value = _note;

            #endregion Step 7: De-Allocation Reason

            #region Step 8: Save the workbook

            if (wbReport.Worksheets.Count > 0)
            {
                //Save excel document
                wbReport.Save(filePathAndName);

                //Display excel document
                System.Diagnostics.Process.Start(filePathAndName);
            }

            #endregion Step 8: Save the workbook

            return true;
        }

        private int InsertDetailsSection(Worksheet wsTemplate, Worksheet wsNewReportSheet, DataTable dataTable, int reportRow, string headingText, string noDataText, bool indicateLeadsWithStatuses)
        {
            #region Declarations & initializations

            int reportTemplateRowIndex = 13;
            int reportTemplateColumnSpan = 80;

            #endregion Declarations & initializations

            Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 3, reportTemplateColumnSpan, wsNewReportSheet, reportRow, 0);
            wsNewReportSheet.GetCell(String.Format("A{0}", reportRow + 1)).Value = headingText;

            if (dataTable.Rows.Count > 0)
            {
                // The reason for this is to avoid duplicating the same information - for example if the number of rows in 2 of the data tables is the same
                if (noDataText != null)
                {
                    reportRow += 4;
                    reportTemplateRowIndex += 5;

                    Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, reportTemplateColumnSpan, wsNewReportSheet, reportRow, 0);
                    wsNewReportSheet.GetCell(String.Format("A{0}", reportRow + 1)).Value = noDataText;

                    reportRow++;
                }
                else
                {
                    #region Display data

                    reportRow += 4;
                    reportTemplateRowIndex += 4;

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        #region Step 3.1. Copy the template formatting for the data row

                        Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, reportTemplateColumnSpan, wsNewReportSheet, reportRow, 0);

                        #endregion Step 3.1. Copy the template formatting for the data row

                        #region Step 3.2. Add the values

                        wsNewReportSheet.GetCell(String.Format("A{0}", reportRow + 1)).Value = dataTable.Rows[i]["Count"];
                        wsNewReportSheet.GetCell(String.Format("D{0}", reportRow + 1)).Value = dataTable.Rows[i]["LeadBookEntryID"];
                        wsNewReportSheet.GetCell(String.Format("I{0}", reportRow + 1)).Value = dataTable.Rows[i]["INImportID"];
                        wsNewReportSheet.GetCell(String.Format("M{0}", reportRow + 1)).Value = dataTable.Rows[i]["RefNo"];
                        wsNewReportSheet.GetCell(String.Format("T{0}", reportRow + 1)).Value = dataTable.Rows[i]["AllocationDate"].ToString();
                        wsNewReportSheet.GetCell(String.Format("AC{0}", reportRow + 1)).Value = dataTable.Rows[i]["IsPrintedYesNo"];
                        wsNewReportSheet.GetCell(String.Format("AG{0}", reportRow + 1)).Value = dataTable.Rows[i]["LeadStatus"];
                        wsNewReportSheet.GetCell(String.Format("AR{0}", reportRow + 1)).Value = dataTable.Rows[i]["IDNumber"];
                        wsNewReportSheet.GetCell(String.Format("AY{0}", reportRow + 1)).Value = dataTable.Rows[i]["ClientName"];
                        wsNewReportSheet.GetCell(String.Format("BK{0}", reportRow + 1)).Value = dataTable.Rows[i]["ClientSurname"];

                        if (indicateLeadsWithStatuses)
                        {
                            if (dataTable.Rows[i]["LeadStatus"] != DBNull.Value)
                            {
                                wsNewReportSheet.GetCell(String.Format("A{0}", reportRow + 1)).CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(_redBackgroundColour), null, FillPatternStyle.Solid);
                                wsNewReportSheet.GetCell(String.Format("A{0}", reportRow + 1)).CellFormat.Font.ColorInfo = new WorkbookColorInfo(_redForegroundColour);

                                wsNewReportSheet.GetCell(String.Format("D{0}", reportRow + 1)).CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(_redBackgroundColour), null, FillPatternStyle.Solid);
                                wsNewReportSheet.GetCell(String.Format("D{0}", reportRow + 1)).CellFormat.Font.ColorInfo = new WorkbookColorInfo(_redForegroundColour);

                                wsNewReportSheet.GetCell(String.Format("I{0}", reportRow + 1)).CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(_redBackgroundColour), null, FillPatternStyle.Solid);
                                wsNewReportSheet.GetCell(String.Format("I{0}", reportRow + 1)).CellFormat.Font.ColorInfo = new WorkbookColorInfo(_redForegroundColour);

                                wsNewReportSheet.GetCell(String.Format("M{0}", reportRow + 1)).CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(_redBackgroundColour), null, FillPatternStyle.Solid);
                                wsNewReportSheet.GetCell(String.Format("M{0}", reportRow + 1)).CellFormat.Font.ColorInfo = new WorkbookColorInfo(_redForegroundColour);

                                wsNewReportSheet.GetCell(String.Format("T{0}", reportRow + 1)).CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(_redBackgroundColour), null, FillPatternStyle.Solid);
                                wsNewReportSheet.GetCell(String.Format("T{0}", reportRow + 1)).CellFormat.Font.ColorInfo = new WorkbookColorInfo(_redForegroundColour);

                                wsNewReportSheet.GetCell(String.Format("AC{0}", reportRow + 1)).CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(_redBackgroundColour), null, FillPatternStyle.Solid);
                                wsNewReportSheet.GetCell(String.Format("AC{0}", reportRow + 1)).CellFormat.Font.ColorInfo = new WorkbookColorInfo(_redForegroundColour);

                                wsNewReportSheet.GetCell(String.Format("AG{0}", reportRow + 1)).CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(_redBackgroundColour), null, FillPatternStyle.Solid);
                                wsNewReportSheet.GetCell(String.Format("AG{0}", reportRow + 1)).CellFormat.Font.ColorInfo = new WorkbookColorInfo(_redForegroundColour);

                                wsNewReportSheet.GetCell(String.Format("AR{0}", reportRow + 1)).CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(_redBackgroundColour), null, FillPatternStyle.Solid);
                                wsNewReportSheet.GetCell(String.Format("AR{0}", reportRow + 1)).CellFormat.Font.ColorInfo = new WorkbookColorInfo(_redForegroundColour);

                                wsNewReportSheet.GetCell(String.Format("AY{0}", reportRow + 1)).CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(_redBackgroundColour), null, FillPatternStyle.Solid);
                                wsNewReportSheet.GetCell(String.Format("AY{0}", reportRow + 1)).CellFormat.Font.ColorInfo = new WorkbookColorInfo(_redForegroundColour);

                                wsNewReportSheet.GetCell(String.Format("BK{0}", reportRow + 1)).CellFormat.Fill = new CellFillPattern(new WorkbookColorInfo(_redBackgroundColour), null, FillPatternStyle.Solid);
                                wsNewReportSheet.GetCell(String.Format("BK{0}", reportRow + 1)).CellFormat.Font.ColorInfo = new WorkbookColorInfo(_redForegroundColour);
                            }
                        }

                        ++reportRow;

                        #endregion Step 3.2. Add the values
                    }

                    #endregion Display data
                }
            }
            else
            {
                if (noDataText != null)
                {
                    reportRow += 4;
                    reportTemplateRowIndex += 5;

                    Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, reportTemplateColumnSpan, wsNewReportSheet, reportRow, 0);
                    wsNewReportSheet.GetCell(String.Format("A{0}", reportRow + 1)).Value = noDataText;
                }
            }

            return reportRow;
        }

        private void DeAllocateLeadsCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnDeAllocateLeads.Content = "De-Allocate Leads";

            EnableAllControls(true);

            //ResetControls(true, true, true, true, true, true, true, true, true, true);

            
        }

        private bool? AllRecordsSelected()
        {
            try
            {
                bool allSelected = true;
                bool noneSelected = true;

                if (xdgLeadsToBeDeAllocated.DataSource != null)
                {
                    foreach (DataRow dr in ((DataView)xdgLeadsToBeDeAllocated.DataSource).Table.Rows)
                    {
                        allSelected = allSelected && (bool)dr["Select"];
                        noneSelected = noneSelected && !(bool)dr["Select"];
                    }
                }

                if (allSelected)
                {
                    return true;
                }
                if (noneSelected)
                {
                    return false;
                }

                UpdateSelectedLeadCount();

                return null;
            }

            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;
                DataTable dt = ((DataView)xdgLeadsToBeDeAllocated.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToBoolean(dr["CanBeDeAllocated"]))
                    {
                        dr["Select"] = true;
                    }
                }

                //EnableDisableExportButton();
                UpdateSelectedLeadCount();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                //DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;
                DataTable dt = ((DataView)xdgLeadsToBeDeAllocated.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = false;
                }

                //EnableDisableExportButton();
                UpdateSelectedLeadCount();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_xdgHeaderPrefixAreaCheckbox != null)
                {
                    _xdgHeaderPrefixAreaCheckbox.IsChecked = AllRecordsSelected();
                }

                //EnableDisableExportButton();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _xdgHeaderPrefixAreaCheckbox = (CheckBox)sender;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        //private bool IsLoggedInAsDeveloper()
        //{
        //    return (GlobalSettings.ApplicationUser.ID == 1);
        //}

        #endregion Private Methods

        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MenuLeadScreen menuLeadScreen = new MenuLeadScreen(ScreenDirection.Reverse);
            OnClose(menuLeadScreen);
        }

        #region EmbriantComboBox SelectionChanged Events

        private void cmbCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbCampaign.SelectedValue != null)
                {
                    SetCursor(Cursors.Wait);

                    _campaignID = Convert.ToInt64(cmbCampaign.SelectedValue);
                    _campaign = (cmbCampaign.SelectedItem as DataRowView).Row;
                    _campaignDescription = _campaign.ItemArray[3].ToString();

                    LoadCampaignBatches(_campaignID);

                }
                else
                {
                    ResetControls(false, true, true, true, true, true, true, true, true, false);
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

        private void cmbBatch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbBatch.SelectedValue != null)
                {
                    SetCursor(Cursors.Wait);

                    _batchID = Convert.ToInt64(cmbBatch.SelectedValue);
                    _batch = (cmbBatch.SelectedItem as DataRowView).Row;
                    _batchDescription = _batch.ItemArray[3].ToString();

                    LoadSalesAgents(_batchID);
                }
                else
                {
                    ResetControls(false, false, true, true, true, true, true, true, true, false);
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

        private void cmbSalesAgent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbSalesConsultant.SelectedIndex > -1)
                {
                    SetCursor(Cursors.Wait);

                    _salesConsultantID = Convert.ToInt64(cmbSalesConsultant.SelectedValue);
                    _salesConsultant = (cmbSalesConsultant.SelectedItem as DataRowView).Row;
                    _salesConsultantDescription = _salesConsultant.ItemArray[1].ToString();

                    LoadAllocationDates(_batchID, _salesConsultantID);

                }
                else
                {
                    ResetControls(false, false, false, true, true, true, true, true, true, false);
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

        private void cmbAllocationDates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbAllocationDates.SelectedValue != null)
                {
                    SetCursor(Cursors.Wait);

                    _allocationDate = Convert.ToDateTime(cmbAllocationDates.SelectedValue);
                    LoadLeadsToBeDeallocated(_batchID, _salesConsultantID, _allocationDate);
                }
                else
                {
                    ResetControls(false, false, false, false, true, true, true, true, true, false);
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

        #endregion EmbriantComboBox SelectionChanged Events:

        #region XamDataGrid Events:
        
        #endregion XamDataGrid Events:


        private void btnDeAllocate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool? result;

                EnableAllControls(false);

                if (!HasAllInputParametersBeenSpecified())
                {
                    EnableAllControls(true);
                    return;
                }

                if (!_canBeDeAllocated)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), _messageBoxText, _messageBoxHeaderText, ShowMessageType.Error);
                    return;
                }
                else
                {
                    INMessageBoxWindow2 messageBox = new INMessageBoxWindow2();
                    messageBox.buttonOK.Content = "Yes";
                    messageBox.buttonCancel.Content = "No";
                    result = ShowMessageBox(messageBox, _messageBoxText, _messageBoxHeaderText, ShowMessageType.Exclamation);

                    if ((bool)result)
                    {
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += DeAllocateLeads;
                        worker.RunWorkerCompleted += DeAllocateLeadsCompleted;
                        worker.RunWorkerAsync();

                        dispatcherTimer1.Start();
                    }
                    else
                    {
                        EnableAllControls(true);
                        return;
                    }
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmbDeAllocationReasons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbRequestor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void HeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        //{

        //}

        //private void HeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        //{

        //}

        //private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        //{

        //}

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
