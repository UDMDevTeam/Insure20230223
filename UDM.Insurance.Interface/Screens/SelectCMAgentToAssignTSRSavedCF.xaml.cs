using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using UDM.Insurance.Business;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{

    public partial class SelectCMAgentToAssignTSRSavedCF
    {

        public long? SelectedCMAgentUserID { get; set; }
        private CallMonitoringDetailsScreen _CallMonitoringDetailsScreen;
        public SelectCMAgentToAssignTSRSavedCF(CallMonitoringDetailsScreen callMonitoringDetailsScreen)
        {
            InitializeComponent();
            
            _CallMonitoringDetailsScreen = callMonitoringDetailsScreen;
            LoadLookupData();
        }

        private void LoadLookupData()
        {
            DataTable dtCMAgent = Methods.GetTableData("SELECT [ID], [Blush].[dbo].[GetFullUserName]([ID]) [Description] FROM [User] WHERE FKUserType = 8");
            cmbCMAgent.Populate(dtCMAgent, "Description", "ID");
            long? importID = _CallMonitoringDetailsScreen.ScreenData.FKINImportID;
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@ImportID", importID);
            DataTable dtCallMonitoringAllocation = Methods.ExecuteStoredProcedure("spINGetCMAllocationsByImportID", parameters).Tables[0];
            if (dtCallMonitoringAllocation.AsEnumerable().Where(x => x["IsSavedCarriedForward"] as bool? == true).Count() == 0)
            {
                cmbCMAgent.SelectedValue = Convert.ToInt64(dtCallMonitoringAllocation.AsEnumerable().Select(x => x["FKUserID"]).FirstOrDefault());
            }


        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //OnDialogClose(_dialogResult);
        }

        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedCMAgentUserID = Convert.ToInt32(cmbCMAgent.SelectedValue);
                long? importID = _CallMonitoringDetailsScreen.ScreenData.FKINImportID;
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ImportID", importID);
                DataTable dtCallMonitoringAllocation = Methods.ExecuteStoredProcedure("spINGetCMAllocationsByImportID", parameters).Tables[0];
                long CMAID = 0;

                if (dtCallMonitoringAllocation.AsEnumerable().Where(x => x["IsSavedCarriedForward"] as bool? == true).Count() == 0)
                {
                    CMAID = Convert.ToInt64(dtCallMonitoringAllocation.AsEnumerable().Select(x => x["FKUserID"]).FirstOrDefault());
                }    
                else
                {
                    //CMAID = Convert.ToInt64(dtCallMonitoringAllocation.AsEnumerable().Where(x => x["IsSavedCarriedForward"] as bool? == true).Select(x => x["ID"]).FirstOrDefault());
                    string cmAgent = dtCallMonitoringAllocation.AsEnumerable().Where(x => x["IsSavedCarriedForward"] as bool? == true).Select(x => x["CMAgent"]).FirstOrDefault().ToString();
                    MessageBox.Show("Failed to save call monitoring allocation. This call has already been assigned to " + cmAgent + " for over assessment.", "Assigned for Over Assessment");
                    return;
                }

                if (SelectedCMAgentUserID != CMAID)
                {
                    CallMonitoringAllocation callMonitoringAllocation = new CallMonitoringAllocation();
                    callMonitoringAllocation.FKINImportID = importID;
                    callMonitoringAllocation.FKUserID = SelectedCMAgentUserID;
                    callMonitoringAllocation.IsSavedCarriedForward = true;
                    callMonitoringAllocation.Save(null);
                }

                

                //DateTime saleDate = DateTime.Now;
                //_LeadApplicationScreen.dteDateOfSale.Value = saleDate.Date;
                //_LeadApplicationScreen.dteSaleDate.Value = saleDate;

                //string time = saleDate.TimeOfDay.Hours + ":" + saleDate.TimeOfDay.Minutes;
                //_LeadApplicationScreen.dteSaleTime.Value = time;

                //OnDialogClose(_dialogResult);
                //_LeadApplicationScreen.LaData.AppData.CarriedForwardReasonID = SelectedCarriedForwardReasonID;

                _CallMonitoringDetailsScreen.btnSave.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                DialogResult = true;
            }
            catch (Exception ex)
            {
                //HandleException(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void EmbriantComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Methods.EmbriantComboBoxPreviewKeyDown(sender, e);
        }

        private void cmbCancellationReason_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(cmbCMAgent);
        }

        private void cmbCancellationReason_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                btnSelect.IsEnabled = false;
                if (cmbCMAgent.SelectedValue != null)
                {
                    btnSelect.IsEnabled = true;
                }
                else
                {
                    btnSelect.ToolTip = _CallMonitoringDetailsScreen.btnSave.ToolTip;
                }
            }
            catch (Exception ex)
            {
                //HandleException(ex);
                MessageBox.Show(ex.Message);
            }
            
        }

    }
}
