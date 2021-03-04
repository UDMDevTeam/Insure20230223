using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using Infragistics.Windows.DataPresenter;
using UDM.WPF.Library;


namespace UDM.Insurance.Interface.Screens
{

	public partial class SelectLeadCampaignScreen
    {

        #region Constant

        #endregion



        #region Private Members
        private readonly string _refNo;
        private long _importID;
        #endregion



        #region Properties
        public long ImportID
        {
            get
            {
                return _importID;
            }
        }
        #endregion

        

        #region Constructors

        public SelectLeadCampaignScreen(string refNo)
        {
            InitializeComponent();
            _refNo = refNo;
            LoadCampaignData();
        }

        #endregion



        #region Private Methods

        private void LoadCampaignData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@RefNo", _refNo);
                DataSet ds = Methods.ExecuteStoredProcedure("spINGetCampaignsByRefNo", parameters);

                xdgSelectLeadCampaign.DataSource = ds.Tables[0].DefaultView;
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

        #endregion



        #region Event Handlers

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (xdgSelectLeadCampaign.SelectedItems.Records.Count > 0)
            {
                _importID = Convert.ToInt64(((DataRecord)xdgSelectLeadCampaign.SelectedItems.Records[0]).Cells[0].Value);

                //bool hasLeadBeenAllocated = Business.Insure.HasLeadBeenAllocated(_importID);
                //bool hasLeadBeenCancelled = Business.Insure.HasLeadBeenCancelled(_importID);
                //bool canClientBeContacted = Business.Insure.CanClientBeContacted(_importID);

                //#region Checking if the lead to be loaded has a status of "DO NOT CONTACT"

                //if (!canClientBeContacted)
                //{
                //    UDM.Insurance.Interface.Windows.INMessageBoxWindow1 messageWindow = new UDM.Insurance.Interface.Windows.INMessageBoxWindow1();
                //    ShowMessageBox(messageWindow, @"This lead cannot be loaded, because the client has requested not to be contacted again.", "DO NOT CONTACT CLIENT", Embriant.Framework.ShowMessageType.Exclamation);
                //    OnDialogClose(false);
                //}

                //#endregion Checking if the lead to be loaded has a status of "DO NOT CONTACT"

                //#region Determining whether or not the lead was allocated

                //if (!hasLeadBeenAllocated)
                //{
                //    UDM.Insurance.Interface.Windows.INMessageBoxWindow1 messageWindow = new UDM.Insurance.Interface.Windows.INMessageBoxWindow1();
                //    ShowMessageBox(messageWindow, @"This lead cannot be loaded, because it has not been allocated yet. Please consult your supervisor.", "Lead not allocated", Embriant.Framework.ShowMessageType.Exclamation);
                //    OnDialogClose(false);
                //}

                //#endregion Determining whether or not the lead was allocated

                //#region Determining whether or not the lead has a status of cancelled
                //// Please see https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/204734160/comments

                //else if (hasLeadBeenCancelled)
                //{
                //    UDM.Insurance.Interface.Windows.INMessageBoxWindow1 messageWindow = new UDM.Insurance.Interface.Windows.INMessageBoxWindow1();
                //    ShowMessageBox(messageWindow, @"This lead cannot be loaded, because the policy has been cancelled by the client. Please consult your supervisor.", "Cancelled Policy", Embriant.Framework.ShowMessageType.Exclamation);
                //    OnDialogClose(false);
                //}

                //#endregion Determining whether or not the lead has a status of cancelled

                //else
                //{
                //    OnDialogClose(true);
                //}
                OnDialogClose(true);
            }
            else
            {
                OnDialogClose(false);
            }
        }

        private void SelectLeadCampaignScreen_OnLoaded(object sender, RoutedEventArgs e)
	    {
            xdgSelectLeadCampaign.Focus();
	    }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(false);
        }


        #endregion

        private void XdgSelectLeadCampaign_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}

