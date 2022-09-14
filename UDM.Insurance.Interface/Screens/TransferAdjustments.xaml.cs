﻿using Embriant.Framework;
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
                        INSalesNotTransferredDetails details = new INSalesNotTransferredDetails();
                        details.FKImportID = loadedImportID;
                        details.FKSalesNotTransferredReason = cmbReason.SelectedValue.ToString();
                        details.Save(_validationResult);
                    }
                    else
                    {
                        INSalesNotTransferredDetails details = new INSalesNotTransferredDetails(long.Parse(value.Rows[0][0].ToString()));
                        details.FKSalesNotTransferredReason = cmbReason.SelectedValue.ToString();
                        details.Save(_validationResult);
                    }
                }
                catch
                {

                }

                ShowMessageBox(new INMessageBoxWindow1(), "Transfer has been updated.\nTransfer updated.\n", "Save Result", ShowMessageType.Exclamation);


            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void medReference_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnGoRef_Click(object sender, RoutedEventArgs e)
        {
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
    }
}