using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Embriant.Framework;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for MoveLeadsScreen.xaml
    /// </summary>
    public partial class MoveLeadsScreen
    {
        private long _importID;
        List<string> declineReasonsWanted = new List<string>();
        private long? _destinationCampaignID = -1;
        private long? _leadStatusID;
        private int _calculatedCount;
        private bool eventViaTextBox =false;
        private bool eventViaCheckBox = false;
        DateTime now = DateTime.Now;

        public MoveLeadsScreen()
        {
            InitializeComponent(); 
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }
        private void LoadCampaigns()
        {
            try
            {
                CommonControlData.PopulateCampaignComboBox(cmbCampaigns);
            }
            catch (Exception)
            {
                
            }
        }

        private void LoadStatuses()
        {
            try
            {
                //SetCursor(Cursors.Wait);
                DataTable dt = Methods.GetTableData("select ID,Description from INLeadStatus order by ID");
                ComboBoxItem unallocated = new ComboBoxItem();
                unallocated.Content = "Un-Allocated";
                unallocated.Tag = -1;
                cmbLeadStatus.Items.Add(unallocated);
                foreach (DataRow rw in dt.Rows)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = rw["Description"].ToString();
                    item.Tag = rw["ID"].ToString();
                    cmbLeadStatus.Items.Add(item);
                }
                //cmbLeadStatus.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                //HandleException(ex);
            }

            finally
            {
                //SetCursor(Cursors.Arrow);
            }
        }

        private void LoadStatusesPrimeCampaign()
        {
            try
            {
                //SetCursor(Cursors.Wait);
                DataTable dt = Methods.GetTableData("select ID,Description from lkpPrimeLeadStatus order by ID");
                try { cmbLeadStatus.Items.Clear(); } catch { }
                ComboBoxItem unallocated = new ComboBoxItem();
                unallocated.Content = "Blank LeadStatus";
                unallocated.Tag = -1;
                cmbLeadStatus.Items.Add(unallocated);
                cmbLeadStatus.SelectedIndex = -1;
                foreach (DataRow rw in dt.Rows)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = rw["Description"].ToString();
                    item.Tag = rw["ID"].ToString();
                    cmbLeadStatus.Items.Add(item);
                }
                //cmbLeadStatus.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                //HandleException(ex);
            }

            finally
            {
                //try { SetCursor(Cursors.Arrow); } catch { }
                
            }
        }

        private void LoadDestinationCampaigns()
        {
            try
            {
                SetCursor(Cursors.Wait);
                CommonControlData.PopulateCampaignComboBox(cmbDestinationCampaigns);

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
        private void LoadBatches(long? campaignID)
        {
            try
            {
                CommonControlData.PopulateBatchComboBox(cmbBatch,campaignID);
            }
            catch (Exception)
            {
                
            }

        }

      

        private void cmbCampaigns_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCampaigns();
            lblLeadsToCopy.Text = "Number of Leads IN Batch: None";          
          
        }

        private void cmbCampaigns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtNumberSelected.Text = string.Empty;
                EnableDisableStartButton();
                long? campaignID =   long.Parse(cmbCampaigns.SelectedValue.ToString());

                if(cmbCampaigns.SelectedValue.ToString() == "431") // Cancer Referral Priming campaign
                {
                    try { cmbLeadStatus.Items.Clear(); } catch { }
                    LoadStatusesPrimeCampaign();
                }
                else
                {
                    try { cmbLeadStatus.Items.Clear(); } catch { }
                    LoadStatuses();
                }

                LoadBatches(campaignID);

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void EnableDisableStartButton()
        {
            try
            {
                if (cmbBatch.SelectedValue != null && cmbCampaigns.SelectedValue != null && cmbDestinationCampaigns.SelectedValue != null)
                {
                    buttonStart.IsEnabled = true;
                }
                else
                {
                    buttonStart.IsEnabled = false;
                }
            }
            catch (Exception)
            {
            }
        }

        private void cmbBatch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtNumberSelected.Text = string.Empty;
                //load reference numbers for batch
                EnableDisableStartButton();
                long batchID = long.Parse(cmbBatch.SelectedValue.ToString());
                long? campaignID = long.Parse(cmbCampaigns.SelectedValue.ToString());
                LoadReferences(batchID,campaignID);
                //destination batch
                INBatch batch = new INBatch(batchID);
                txtDestinationBatch.Text = batch.Code;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void LoadReferences(long? batchId,long? campaignID)
        {
            try
            {
                SetCursor(Cursors.Wait);

                if(_leadStatusID == null)
                {
                    _leadStatusID = -1;
                }
               
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@CampaignID", campaignID);
                parameters[1] = new SqlParameter("@BatchID", batchId);
                parameters[2] = new SqlParameter("@FKINLeadStatusID", _leadStatusID);

                DataTable dt = new DataTable();
                if(cmbCampaigns.SelectedValue.ToString() == "431") //Prime campaign
                {
                    dt = Methods.ExecuteStoredProcedure("spLeadMoveReferencesPrime", parameters).Tables[0];
                }
                else
                {
                    dt = Methods.ExecuteStoredProcedure("spLeadMoveReferences", parameters).Tables[0];
                }
                

                if (dt.Rows.Count > 0)
                {
                    CheckBox All = new CheckBox();
                    All.Content = "Select All";
                    All.Checked += All_Checked;
                    All.Unchecked += All_Unchecked;
                    lstReferenceNumbers.Items.Clear();
                    lstReferenceNumbers.Items.Add(All);                    
                    foreach (DataRow rw in dt.Rows)
                    {
                                CheckBox checkbox = new CheckBox();
                                checkbox.Content = rw["RefNo"].ToString();
                                checkbox.Checked += checkbox_Checked;
                                checkbox.Tag = rw["ID"];
                                lstReferenceNumbers.Items.Add(checkbox);
                    }
                    lblLeadsToCopy.Text = "Number of Leads IN Batch: " + (lstReferenceNumbers.Items.Count -1);
                    txtNumberSelected.IsEnabled = true;
                }
                else
                {
                    if (lstReferenceNumbers.Items.Count > 0)
                    {
                        lstReferenceNumbers.Items.Clear();
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

        void checkbox_Checked(object sender, RoutedEventArgs e)
        {
            int count = 0;
            eventViaCheckBox = true;
            if (eventViaTextBox == false)
            {
                foreach (CheckBox chkItem in lstReferenceNumbers.Items)
                {

                    if (chkItem.IsChecked == true)
                    {
                        count++;
                    }
                }
                txtNumberSelected.Text = count.ToString();
                _calculatedCount = count;
            }
            eventViaCheckBox = false;
        }

        void All_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox chkItem in lstReferenceNumbers.Items)
            {
                chkItem.IsChecked = false;

            }
            txtNumberSelected.Text = "0";
            _calculatedCount = 0;
        }

        void All_Checked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox chkItem in lstReferenceNumbers.Items)
            {
                chkItem.IsChecked = true;

            }
            txtNumberSelected.Text = (lstReferenceNumbers.Items.Count - 1).ToString();
            _calculatedCount = lstReferenceNumbers.Items.Count - 1;
        }
      

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void Search()
        {
            try
            {
                //filter listbox
                List<CheckBox> checkBoxList = new List<CheckBox>();
                List<CheckBox> checkBoxListFiltered = new List<CheckBox>();
                CheckBox chkAll = new CheckBox();
                bool first = true;
                foreach (CheckBox check in lstReferenceNumbers.Items)
                {
                    if (first == true)
                    {
                        chkAll = check;
                    }
                    if (check.Content.ToString().ToLower().Contains(txtSearch.Text.ToLower()))
                    {
                        check.Foreground = Brushes.DeepSkyBlue;
                        checkBoxList.Add(check);
                    }
                    first = false;
                }
                foreach (CheckBox chk in lstReferenceNumbers.Items)
                {
                    foreach (CheckBox searchedCheck in checkBoxList)
                    {
                        if (searchedCheck.Content == chk.Content)
                        {
                            //exists
                        }
                        else
                        {
                            //add it to list                            
                            checkBoxListFiltered.Add(chk);

                        }
                    }
                }
                lstReferenceNumbers.Items.Clear();
                lstReferenceNumbers.Items.Add(chkAll);
                foreach (CheckBox chk in checkBoxList)
                {
                    lstReferenceNumbers.Items.Add(chk);
                }
                foreach (CheckBox chk in checkBoxListFiltered)
                {
                    if (chk.Content != "Select All")
                    {
                        lstReferenceNumbers.Items.Add(chk);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmbDestinationCampaigns_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDestinationCampaigns(); 
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(lstReferenceNumbers.Items.Count > 1)
                {
                //copy each reference number
                bool first = true;
                int copyCount = 0;
                lstCopiedReferenceNumbers.Items.Clear();
                long destinationBatchID = 0;
                foreach (CheckBox chkRefNo in lstReferenceNumbers.Items)
                {
                    if (first != true)
                    {
                        if (chkRefNo.IsChecked == true)
                        {
                            DataTable dtLoadBatch = Methods.GetTableData("select * from INBatch where FKINCampaignID =" + _destinationCampaignID + " AND Code = '" + txtDestinationBatch.Text + "'");
                            bool batchCreated = false;
                            bool batchExists = false;
                            if (dtLoadBatch.Rows.Count > 0)
                            {
                                batchExists = true;
                                destinationBatchID = long.Parse(dtLoadBatch.Rows[0]["ID"].ToString());
                            }
                            if (dtLoadBatch.Rows.Count == 0)
                            {
                                //create a new batch
                                INBatch batch = new INBatch();
                                batch.Code = txtDestinationBatch.Text;
                                batch.UDMCode = txtDestinationBatch.Text;
                                batch.FKINCampaignID = _destinationCampaignID;
                                batch.NewLeads = 0;
                                batch.UpdatedLeads = 0;
                                batch.Save(_validationResult);
                                batchCreated = true;
                                destinationBatchID = batch.ID;

                            }

                            if (batchCreated == true || batchExists == true)
                            {
                                long destinationCampaign = long.Parse(cmbDestinationCampaigns.SelectedValue.ToString());

                                //checkbox tag is importID                        
                                _importID = long.Parse(chkRefNo.Tag.ToString());
                                INImport Currentimport = new INImport(_importID);
                                long? parentBatchID = Currentimport.FKINBatchID;
                                if (parentBatchID == destinationBatchID)
                                {
                                    parentBatchID = null;
                                }
                                Currentimport.FKINParentBatchID = parentBatchID;
                                Currentimport.FKINCampaignID = _destinationCampaignID;
                                Currentimport.FKINBatchID = destinationBatchID;
                                if(_destinationCampaignID == 431 || _destinationCampaignID == 429)
                                {
                                        Currentimport.FKUserID = null;
                                        Currentimport.IsPrinted = null;
                                        Currentimport.AllocationDate = null;
                                }
                                
                                Currentimport.Save(_validationResult);
                               
                                copyCount++;
                                CheckBox chkAdded = new CheckBox();
                                chkAdded.Tag = chkRefNo.Tag;
                                chkAdded.Content = chkRefNo.Content;
                                lstCopiedReferenceNumbers.Items.Add(chkAdded);
                            }
                        }

                    }
                    first = false;
                }
                if (copyCount > 0)
                {
                    string message = copyCount + " Leads Moved to Campaign " + cmbDestinationCampaigns.Text + " And Batch " + txtDestinationBatch.Text;
                    ShowMessageBox(new INMessageBoxWindow1(), message, "Save result", ShowMessageType.Information);
                    long? batchID = long.Parse(cmbBatch.SelectedValue.ToString());
                    long? campaignID = long.Parse(cmbCampaigns.SelectedValue.ToString());
                    LoadReferences(batchID, campaignID);
                }
                }

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmbDestinationCampaigns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                EnableDisableStartButton();
                long? destinationcampaignID = long.Parse(cmbDestinationCampaigns.SelectedValue.ToString());
                _destinationCampaignID = destinationcampaignID;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmbDestinationBatch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableDisableStartButton();
        }

        private void calFromDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                long batchID = long.Parse(cmbBatch.SelectedValue.ToString());
                long? campaignID = long.Parse(cmbCampaigns.SelectedValue.ToString());

                if (batchID != null && campaignID != null)
                {
                    LoadReferences(batchID, campaignID);
                }
            }
            catch (Exception)
            {
            }
        }

        private void calToDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                long batchID = long.Parse(cmbBatch.SelectedValue.ToString());
                long? campaignID = long.Parse(cmbCampaigns.SelectedValue.ToString());

                if (batchID != null && campaignID != null)
                {
                    LoadReferences(batchID, campaignID);
                }
            }
            catch (Exception)
            {
            }
        }

        private void cmbLeadStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtNumberSelected.Text = string.Empty;
                ComboBoxItem selItem = (ComboBoxItem)cmbLeadStatus.SelectedItem;
                _leadStatusID = long.Parse(selItem.Tag.ToString());
                if (cmbBatch.SelectedValue != null)
                {
                    long batchID = long.Parse(cmbBatch.SelectedValue.ToString());
                    long? campaignID = long.Parse(cmbCampaigns.SelectedValue.ToString());
                    LoadReferences(batchID, campaignID);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmbLeadStatus_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadStatuses();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void txtNumberSelected_TextChanged(object sender, TextChangedEventArgs e)
        {
            

            if (eventViaCheckBox == false)
            {
                eventViaTextBox = true;
                if (txtNumberSelected.Text == string.Empty)
                {
                    foreach (CheckBox chkItem in lstReferenceNumbers.Items)
                    {
                        chkItem.IsChecked = false;
                    }
                }
                else
                {
                    if (txtNumberSelected.Text.Any(char.IsLetter))
                    {
                        txtNumberSelected.Text = _calculatedCount.ToString();
                    }
                    else
                    {

                        int count = int.Parse(txtNumberSelected.Text);
                        int currentCount = 0;
                        foreach (CheckBox chkItem in lstReferenceNumbers.Items)
                        {
                            if (chkItem.IsChecked == true)
                            {
                                chkItem.IsChecked = false;
                            }
                            if (chkItem.Content != "Select All")
                            {
                                currentCount++;
                                if (currentCount <= count)
                                {
                                    chkItem.IsChecked = true;
                                }
                            }
                        }
                        _calculatedCount = count;
                    }


                }
                eventViaTextBox = false;

            }

        }
    }
}
