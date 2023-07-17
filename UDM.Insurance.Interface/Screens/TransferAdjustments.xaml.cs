using Embriant.Framework;
using Embriant.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class TransferAdjustments
    {

        #region Constants
        private const string IDField = "ID";
        private const string DescriptionField = "Description";

        #endregion Constants

        #region Private Members
        long loadedImportID;
        bool deleteBool = false;
        #endregion

        public TransferAdjustments()
        {
            InitializeComponent();
            LoadInitialData();
        }


        private void EmbriantComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Methods.EmbriantComboBoxPreviewKeyDown(sender, e);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            if(deleteBool == true)
            {
                //string strQuery = " ";

                //strQuery = "DELETE FROM INSalesToCallMonitoring WHERE FKImportID = " + loadedImportID;
                string DCINSalesToCallMonitoringID = Convert.ToString(Methods.GetTableData("Select ID from [INSalesToCallMonitoring] where [INSalesToCallMonitoring].[FKImportID] = " + loadedImportID).Rows[0][0]);

                //DataTable dtINImportDelete = Methods.GetTableData(strQuery);
                SalesToCallMonitoring scm = new SalesToCallMonitoring(int.Parse(DCINSalesToCallMonitoringID));
                scm.Delete(_validationResult);

                ShowMessageBox(new INMessageBoxWindow1(), "Transfer has been Removed.\nTransfer Removed.\n", "Save Result", ShowMessageType.Exclamation);
                CleaFields();
            }
            else if (DeleteReasCB.IsChecked == true)
            {
                try
                {
                    string DCINSalesToCallMonitoringID = Convert.ToString(Methods.GetTableData("Select ID from [INSalesNotTransferredDetails] where [INSalesNotTransferredDetails].[FKImportID] = " + loadedImportID).Rows[0][0]);

                    //DataTable dtINImportDelete = Methods.GetTableData(strQuery);
                    INSalesNotTransferredDetails scm = new INSalesNotTransferredDetails(int.Parse(DCINSalesToCallMonitoringID));
                    scm.Delete(_validationResult);

                    ShowMessageBox(new INMessageBoxWindow1(), "Transfer reason has been Removed.\nTransfer Reason Removed.\n", "Save Result", ShowMessageType.Exclamation);
                }
                catch
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Transfer not available.\n Not available.\n", "Save Result", ShowMessageType.Exclamation);
                }
                CleaFields();
            }
            else
            {

                try
                {
                    string DCSpecialistFKUserID = Convert.ToString(Methods.GetTableData("Select FKUserID from [lkpINCMAgentForwardedSale] where [lkpINCMAgentForwardedSale].[ID] = " + cmbDCSpecialist.SelectedValue).Rows[0][0]);
                    string DCINSalesToCallMonitoringID = Convert.ToString(Methods.GetTableData("Select ID from [INSalesToCallMonitoring] where [INSalesToCallMonitoring].[FKImportID] = " + loadedImportID).Rows[0][0]);


                    SalesToCallMonitoring scm = new SalesToCallMonitoring(int.Parse(DCINSalesToCallMonitoringID));
                    scm.FKUserID = int.Parse(DCSpecialistFKUserID);
                    scm.Save(_validationResult);


                }
                catch
                {

                }

                try
                {
                    DataTable value = Methods.GetTableData("SELECT ID FROM INSalesNotTransferredDetails WHERE FKImportID = " + loadedImportID);

                    if (value.Rows.Count == 0)
                    {
                        try
                        {
                            INSalesNotTransferredDetails details = new INSalesNotTransferredDetails();
                            details.FKImportID = loadedImportID;
                            details.FKSalesNotTransferredReason = cmbReason.SelectedValue.ToString();
                            details.FKAuthorisedUserID = long.Parse(cmbAuthorization.SelectedValue.ToString());
                            details.Save(_validationResult);
                        }
                        catch
                        {
                            INSalesNotTransferredDetails details = new INSalesNotTransferredDetails();
                            details.FKImportID = loadedImportID;
                            details.FKSalesNotTransferredReason = cmbReason.SelectedValue.ToString();
                            details.Save(_validationResult);
                        }

                    }
                    else
                    {
                        try
                        {
                            INSalesNotTransferredDetails details = new INSalesNotTransferredDetails(long.Parse(value.Rows[0][0].ToString()));
                            details.FKImportID = loadedImportID;
                            details.FKSalesNotTransferredReason = cmbReason.SelectedValue.ToString();
                            details.FKAuthorisedUserID = long.Parse(cmbAuthorization.SelectedValue.ToString());
                            details.Save(_validationResult);
                        }
                        catch
                        {
                            INSalesNotTransferredDetails details = new INSalesNotTransferredDetails(long.Parse(value.Rows[0][0].ToString()));
                            details.FKImportID = loadedImportID;
                            details.FKSalesNotTransferredReason = cmbReason.SelectedValue.ToString();
                            details.Save(_validationResult);
                        }

                    }

                }
                catch(Exception t)
                {

                }

                ShowMessageBox(new INMessageBoxWindow1(), "Transfer has been updated.\n", "Save Result", ShowMessageType.Information);

                CleaFields();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void medReference_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void CleaFields()
        {
            cmbDCSpecialist.SelectedIndex = -1;
            cmbReason.SelectedIndex = -1;
            medDOLeadStatus.Text = "";
            DeleteTranCB.IsChecked = false;
            DeleteReasCB.IsChecked = false;
        }

        private void btnGoRef_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbAuthorization.SelectedIndex = -1;
            }
            catch { }

            if (medReference.Text != "")
            {
                try
                {
                    string strQuery = " ";

                    strQuery = "SELECT ID FROM INImport WHERE ";
                    strQuery += "RefNo = '" + medReference.Text + "'";

                    DataTable dtINImport = Methods.GetTableData(strQuery);

                    if (dtINImport.Rows.Count > 1)
                    {
                        SelectLeadCampaignScreen selectLeadCampaignScreen = new SelectLeadCampaignScreen(medReference.Text);

                        //ShowOrHideFields(true);

                        if (ShowDialog(selectLeadCampaignScreen, new INDialogWindow(selectLeadCampaignScreen)) == true)
                        {
                            long importID = selectLeadCampaignScreen.ImportID;
                            loadedImportID = importID;

                            LoadData(importID);
                        }
                    }
                    else
                    {
                        long? INImportID = dtINImport.Rows[0]["ID"] as long?;
                        long inimportIDLong = long.Parse(INImportID.ToString());
                        loadedImportID = inimportIDLong;
                        LoadData(inimportIDLong);
                    }

                }
                catch
                {
                }
            }
            else
            {
            }


        }

        public void LoadData(long importid)
        {
            ClearScreen();

            #region DC Specialist
            try
            {
                string DCSpecialistFKUserID = Convert.ToString(Methods.GetTableData("Select FKUserID from [INSalestoCallMonitoring] where [INSalestoCallMonitoring].[FKImportID] = " + importid.ToString()).Rows[0][0]);
                string dcSpecialistID = Convert.ToString(Methods.GetTableData("Select ID from [lkpINCMAgentForwardedSale] where [lkpINCMAgentForwardedSale].[FKUserID] = " + DCSpecialistFKUserID).Rows[0][0]);
                cmbDCSpecialist.SelectedValue = int.Parse(dcSpecialistID);
            }
            catch
            {
                ShowMessageBox(new INMessageBoxWindow1(), "This lead was not transferred.\nLead not Transferred.\n", "Transfer Status", ShowMessageType.Exclamation);

            }

            #endregion

            #region Transfer Reason
            try
            {
                string notTransferredReasonID = Convert.ToString(Methods.GetTableData("Select FKSalesNotTransferredReason from [INSalesNotTransferredDetails] where [INSalesNotTransferredDetails].[FKImportID] = " + importid.ToString()).Rows[0][0]);
                cmbReason.SelectedValue = int.Parse(notTransferredReasonID);
                try
                {
                    if (cmbReason.SelectedValue.ToString() == "7")
                    {
                        lblAuthorised.Visibility = Visibility.Visible;
                        cmbAuthorization.Visibility = Visibility.Visible;

                        try 
                        {
                            string AuthorisedBy = Convert.ToString(Methods.GetTableData("Select FKAuthorisedUserID from [INSalesNotTransferredDetails] where [INSalesNotTransferredDetails].[FKImportID] = " + importid.ToString()).Rows[0][0]);
                            cmbReason.SelectedValue = int.Parse(notTransferredReasonID);cmbAuthorization.SelectedValue = int.Parse(AuthorisedBy);
                        }
                        catch 
                        {

                        }
                    }
                    else
                    {
                        lblAuthorised.Visibility = Visibility.Collapsed;
                        cmbAuthorization.Visibility = Visibility.Collapsed;
                    }
                }
                catch
                {

                }
            }
            catch
            {
                cmbReason.SelectedIndex = -1;

            }

            #endregion

            #region LeadStatus
            try
            {
                string LeadStatusID = Convert.ToString(Methods.GetTableData("Select FKINLeadStatusID from [INImport] where [INImport].[ID] = " + importid.ToString()).Rows[0][0]);
                string LeadStatus = Convert.ToString(Methods.GetTableData("Select Description from [INLeadStatus] where [INLeadStatus].[ID] = " + LeadStatusID).Rows[0][0]);

                medDOLeadStatus.Text = LeadStatus;
            }
            catch
            {

            }

            #endregion

            #region DebiCheck Mandates
            try
            {
                string MandateUserID2 = Convert.ToString(Methods.GetTableData("Select top 2 StampUserID from [DebiCheckSent] where [DebiCheckSent].[FKImportID] = " + importid.ToString() + " order by DebiCheckSent.[ID] desc ").Rows[0][0]);
                string MandateUserName2 = Convert.ToString(Methods.GetTableData("Select [U].[FirstName] + ' ' + [U].[LastName] from [Insure].[dbo].[User] as [U] where [U].[ID] = " + MandateUserID2 ).Rows[0][0]);
                lblTextmandate1.Text = MandateUserName2;
                try
                {
                    string MandateUserID1 = Convert.ToString(Methods.GetTableData("Select top 2 StampUserID from [DebiCheckSent] where [DebiCheckSent].[FKImportID] = " + importid.ToString() + " order by DebiCheckSent.[ID] desc ").Rows[0][1]);
                    string MandateUserName1 = Convert.ToString(Methods.GetTableData("Select [U].[FirstName] + ' ' + [U].[LastName] from [Insure].[dbo].[User] as [U] where [U].[ID] = " + MandateUserID1 ).Rows[0][0]);
                    lblTextmandate2.Text = MandateUserName1;
                }
                catch
                {
                    lblTextmandate2.Text = "";
                }

            }
            catch
            {
                lblTextmandate1.Text = " ";
                lblTextmandate2.Text = " ";
            }

            #endregion
        }

        public void LoadInitialData()
        {
            #region DebiCheck Specialist Combobox
            string strQuery;
            strQuery = "SELECT DISTINCT ID, Description FROM lkpINCMAgentForwardedSale  ";


            DataTable dtPolicyPlanGroup = Methods.GetTableData(strQuery);
            cmbDCSpecialist.Populate(dtPolicyPlanGroup, DescriptionField, IDField);

            #endregion

            #region Transfer Reason
            string strQueryTransferReason;
            strQueryTransferReason = "SELECT DISTINCT ID, Description FROM lkpSalesNotTransferredReason  ";


            DataTable dtReason = Methods.GetTableData(strQueryTransferReason);
            cmbReason.Populate(dtReason, DescriptionField, IDField);

            #endregion

            #region Campaign Combobox
            string strQueryCampaigns;
            strQueryCampaigns = "SELECT DISTINCT ID, Name as Description FROM INCampaign as [Description] where IsActive = '1' order by Name Asc ";


            DataTable dtCampaigns = Methods.GetTableData(strQueryCampaigns);
            cmbCampaigns.Populate(dtCampaigns, DescriptionField, IDField);
            #endregion

            #region Authorised Users ComboBox
            string strQueryAuth;
            strQueryAuth = "SELECT  ID, [U].[FirstName] + ' ' + [U].[LastName]  as Description FROM [Insure].[dbo].[User] as [U]  where ID IN (105, 361, 1987) ";


            DataTable dtAuth = Methods.GetTableData(strQueryAuth);
            cmbAuthorization.Populate(dtAuth, DescriptionField, IDField);
            #endregion

        }

        public void ClearScreen()
        {
            cmbDCSpecialist.SelectedIndex = -1;
            cmbReason.SelectedIndex = -1;
            medDOLeadStatus.Text = "";
            DeleteTranCB.IsChecked = false;
            lblTextmandate2.Text = " ";
            lblTextmandate1.Text = " ";
        }

        private void DeleteTranCB_Checked(object sender, RoutedEventArgs e)
        {
            deleteBool = true;
        }

        private void DeleteTranCB_Unchecked(object sender, RoutedEventArgs e)
        {
            deleteBool = false;
        }

        private void cmbCampaigns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string IsFocused;
            try
            {
                IsFocused = Convert.ToString(Methods.GetTableData("Select top 1 IsActive from [INFocusCampaigns] where [INFocusCampaigns].[FKINCampaignID] = " + cmbCampaigns.SelectedValue.ToString()).Rows[0][0]);
            }
            catch
            {
                IsFocused = "0";
            }

            if(IsFocused == "1")
            {
                IsFocusCB.IsChecked = true;
            }
            else
            {
                IsFocusCB.IsChecked = false;
            }
        }

        private void btnFocusCampaignSave_Click(object sender, RoutedEventArgs e)
        {

            string IsFocused;
            string isActive;
            try
            {
                IsFocused = Convert.ToString(Methods.GetTableData("Select top 1 ID from [INFocusCampaigns] where [INFocusCampaigns].[FKINCampaignID] = " + cmbCampaigns.SelectedValue.ToString()).Rows[0][0]);
            }
            catch
            {
                IsFocused = "";
            }

            if(IsFocusCB.IsChecked == true)
            {
                isActive = "1";
            }
            else
            {
                isActive = "0";
            }


            if (IsFocused == "")
            {
                INFocusCampaigns fc = new INFocusCampaigns();
                fc.FKINCampaignID = long.Parse(cmbCampaigns.SelectedValue.ToString());
                fc.IsActive = isActive;
                fc.Save(_validationResult);

                ShowMessageBox(new INMessageBoxWindow1(), "Saved\n", "Focus Campaign Saved !", ShowMessageType.Information);

            }
            else
            {

                long idInput = long.Parse(IsFocused);
                INFocusCampaigns fc = new INFocusCampaigns(idInput);
                fc.FKINCampaignID = long.Parse(cmbCampaigns.SelectedValue.ToString()); 
                fc.IsActive = isActive;
                fc.Save(_validationResult);

                ShowMessageBox(new INMessageBoxWindow1(), "Saved\n", "Focus Campaign Saved !", ShowMessageType.Information);

            }

        }

        private void cmbReason_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbReason.SelectedValue.ToString() == "7")
                {
                    lblAuthorised.Visibility = Visibility.Visible;
                    cmbAuthorization.Visibility = Visibility.Visible;
                }
                else
                {
                    lblAuthorised.Visibility = Visibility.Collapsed;
                    cmbAuthorization.Visibility = Visibility.Collapsed;
                }
            }
            catch 
            {

            }


        }

        private void btnChangetoDCAgent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                INImport details = new INImport(loadedImportID);
                details.FKINLeadStatusID = 24;
                details.Save(_validationResult);

                ShowMessageBox(new INMessageBoxWindow1(), "Saved\n", "Changed back to Forward to DC Agent", ShowMessageType.Information);
            }
            catch
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Not Saved\n", "Failed to save.", ShowMessageType.Exclamation);

            }



        }

        private void DeleteReasCB_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteReasCB_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
