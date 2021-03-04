using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using Embriant.Framework.Configuration;
using Embriant.WPF.Controls;
using Infragistics.Windows.Editors;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.Data;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    public partial class RedeemGiftScreen
    {

        #region Constants



        #endregion Constants

        #region Private Members

        private GiftRedeemData _screenData = new GiftRedeemData();
        public GiftRedeemData ScreenData
        {
            get { return _screenData; }
            set { _screenData = value; }
        }

        private LeadApplicationData.User _userData;
        private LeadApplicationData.Application _appData;

        #endregion Private Members

        #region Constructors

        public RedeemGiftScreen(LeadApplicationData LaData)
        {
            InitializeComponent();

            _userData = LaData.UserData;
            _appData = LaData.AppData;

            LoadLookupData(LaData.AppData.ImportID);
            LoadScreenData(LaData.AppData.ImportID);

            try
            {
                string GiftRedeemed = Convert.ToString(Methods.GetTableData("SELECT Gift FROM INImport WHERE ID = " + LaData.AppData.ImportID).Rows[0][0]);

                if (GiftRedeemed == "")
                {
                    medRedeemedGift.Text = "Gift N/A";
                }
                else
                {
                    medRedeemedGift.Text = "Gift Redeemed - " + GiftRedeemed + ".";
                }
            }
            catch
            {
                medRedeemedGift.Text = "Gift N/A";
            }
        }

        #endregion Constructors

        #region Private methods

        private void LoadLookupData(long? fkINImportID)
        {
            //string strSQL = "SELECT ID, [Description] FROM lkpINGiftRedeemStatus";
            //DataTable dtGiftRedeemStatus = Methods.GetTableData(strSQL);
            //cmbGiftStatus.Populate(dtGiftRedeemStatus, "Description", "ID");

            //strSQL = "SELECT ID, Gift FROM INGiftOption WHERE GetDate() BETWEEN ActiveStartDate AND ActiveEndDate";
            //DataTable dtGiftOption = Methods.GetTableData(strSQL);
            //cmbGiftSelection.Populate(dtGiftOption, "Gift", "ID");

            if (fkINImportID.HasValue)
            {
                DataSet dsLookups = Insure.INGetRedeemGiftScreenLookups(fkINImportID.Value);
                cmbGiftStatus.Populate(dsLookups.Tables[0], "Description", "ID");
                cmbGiftSelection.Populate(dsLookups.Tables[1], "Gift", "ID");
                cmbRedeemedBy.Populate(dsLookups.Tables[2], "User", "ID");
            }
        }

        private void LoadScreenData(long? importID)
        {
            INGiftRedeem inGiftRedeem = INGiftRedeemMapper.SearchOne(importID, null);

            if(inGiftRedeem == null)
            {
                inGiftRedeem = new INGiftRedeem();
                inGiftRedeem.FKlkpGiftRedeemStatusID = 2;
                inGiftRedeem.Save(null);
            }

            ScreenData.GiftRedeemID = inGiftRedeem.ID;
            ScreenData.ImportID = importID;
            ScreenData.GiftRedeemStatusID = inGiftRedeem.FKlkpGiftRedeemStatusID;
            ScreenData.GiftRedeemStatus = (lkpINGiftRedeemStatus?) inGiftRedeem.FKlkpGiftRedeemStatusID;
            ScreenData.GiftOptionID = inGiftRedeem.FKGiftOptionID;
            ScreenData.DateRedeemed = inGiftRedeem.RedeemedDate;
            ScreenData.PODDate = inGiftRedeem.PODDate;
            ScreenData.PODSignature = inGiftRedeem.PODSignature;
            ScreenData.IsWebRedeemed = inGiftRedeem.IsWebRedeemed;
            ScreenData.IsRedeemedGiftFieldModifiable = Insure.IsRedeemedGiftFieldsModifiable();
            ScreenData.CanEditDetails = Insure.INCanUserEditRedeemedGiftDetails(importID);

            if (inGiftRedeem.FKUserID == null)
            {
                if (_userData.UserType == lkpUserType.SalesAgent)
                {
                    ScreenData.FKUserID = _userData.UserID;
                }
                else if (_appData.AgentID != null)
                {
                    ScreenData.FKUserID = _appData.AgentID;
                }
                else
                {
                    ScreenData.FKUserID = null;
                }
            }
            else
            {
                ScreenData.FKUserID = inGiftRedeem.FKUserID;
            }
            

            //Defaults
            if (ScreenData.GiftRedeemStatusID == null)
            {
                ScreenData.GiftRedeemStatusID = 2;
            }

            if (ScreenData.DateRedeemed == null && ScreenData.GiftRedeemStatusID == (long?)lkpINGiftRedeemStatus.NotRedeemed)
            {
                ScreenData.DateRedeemed = DateTime.Now;
            }

            ScreenData.PreviousRedeemStatus = ScreenData.GiftRedeemStatusID;

            //From 2018-09-17 only the "R500 Gift Card" option should show for NR batches with leads that have not yet been redeemed.
            if (ScreenData.GiftRedeemStatusID == 2)
            {
                if (DateTime.Now >= Convert.ToDateTime("2018-09-17 00:00:00"))
                {
                    string strSQL = "SELECT ID, Gift FROM INGiftOption WHERE ID = 12";

                    DataTable dtGifts = Methods.GetTableData(strSQL);

                    cmbGiftSelection.Populate(dtGifts, "Gift", "ID");

                    ScreenData.GiftOptionID = 12;
                }
            }
            else if (ScreenData.GiftRedeemStatusID == 1 && ScreenData.DateRedeemed >= Convert.ToDateTime("2018-09-17 00:00:00"))
            {
                string strSQL = "SELECT ID, Gift FROM INGiftOption WHERE ID = 12";

                DataTable dtGifts = Methods.GetTableData(strSQL);

                cmbGiftSelection.Populate(dtGifts, "Gift", "ID");

                //ScreenData.GiftOptionID = 12;

                cmbGiftSelection.SelectedIndex = 0;
            }
        }

        //private void EnableDisableScreenControls()
        //{
        //    bool canChangeDetails = Insure.
        //}


        private void xamEditor_Select(object sender)
        {
            switch (sender.GetType().Name)
            {
                case "XamMaskedEditor":
                    var xamMEDControl = (XamMaskedEditor)sender;

                    switch (xamMEDControl.Name)
                    {
                        default:
                            xamMEDControl.SelectAll();
                            break;
                    }
                    break;

                case "XamDateTimeEditor":
                    var xamDTEControl = (XamDateTimeEditor)sender;

                    switch (xamDTEControl.Name)
                    {
                        default:
                            if (xamDTEControl.IsInEditMode)
                            {
                                xamDTEControl.SelectAll();
                            }
                            break;
                    }
                    break;
            }
        }

        private void xamEditor_GotFocus(object sender, RoutedEventArgs e)
        {
            xamEditor_Select(sender);
        }

        #endregion
        
        #region Event Handlers

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {               
            //if (previousGiftRedeemStatus != ScreenData.GiftRedeemStatusID)
            //{
            //    if (ScreenData.GiftRedeemStatusID == 1)
            //    {
            //        LaData.AppData.LeadStatus = 9;
            //    }
            //}

            Close();
        }

        private void btnRedeem_Click(object sender, RoutedEventArgs e)
        {
            //INMessageBoxWindow2 inMessageBox = new INMessageBoxWindow2(null, "Are you sure you want to save the redeem gift information?", "Redeem Gift", ShowMessageType.Exclamation);
            //var result = inMessageBox.ShowDialog();

            MessageBoxResult result = MessageBox.Show("Are you sure the redeem gift information is correct?", "Redeem Gift", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes)
            {
                if (ScreenData.GiftRedeemID != null)
                {
                    INGiftRedeem inGiftRedeem = new INGiftRedeem((long) ScreenData.GiftRedeemID);

                    inGiftRedeem.FKINImportID = ScreenData.ImportID;
                    inGiftRedeem.FKlkpGiftRedeemStatusID = ScreenData.GiftRedeemStatusID = 1;
                    inGiftRedeem.FKGiftOptionID = ScreenData.GiftOptionID;
                    inGiftRedeem.RedeemedDate = ScreenData.DateRedeemed;
                    //inGiftRedeem.PODDate = ScreenData.PODDate;
                    //inGiftRedeem.PODSignature = ScreenData.PODSignature;

                    inGiftRedeem.IsWebRedeemed = ScreenData.IsWebRedeemed;
                    inGiftRedeem.FKUserID = ScreenData.FKUserID;

                    inGiftRedeem.Save(null);
                }
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure the redeem gift information is correct?", "Confirm Details", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes)
            {
                if (ScreenData.GiftRedeemID != null)
                {
                    INGiftRedeem inGiftRedeem = new INGiftRedeem((long)ScreenData.GiftRedeemID);

                    inGiftRedeem.FKINImportID = ScreenData.ImportID;
                    inGiftRedeem.FKlkpGiftRedeemStatusID = ScreenData.GiftRedeemStatusID;
                    inGiftRedeem.FKGiftOptionID = ScreenData.GiftOptionID;
                    inGiftRedeem.RedeemedDate = ScreenData.DateRedeemed;
                    //inGiftRedeem.PODDate = ScreenData.PODDate;
                    //inGiftRedeem.PODSignature = ScreenData.PODSignature;

                    inGiftRedeem.IsWebRedeemed = ScreenData.IsWebRedeemed;
                    inGiftRedeem.FKUserID = ScreenData.FKUserID;

                    inGiftRedeem.Save(null);
                }
            }
        }

        public void EmbriantComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            EmbriantComboBox cmbControl = (EmbriantComboBox)sender;

            if (e.Key == Key.Back)
            {
                if (cmbControl.SelectedValue != null)
                {
                    cmbControl.SelectedValue = null;
                    e.Handled = true;
                }
            }
        }
        
        #endregion
        
    }
}
