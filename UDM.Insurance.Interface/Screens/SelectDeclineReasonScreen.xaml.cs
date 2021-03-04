using System;
using System.Data;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{

    public partial class SelectDeclineReasonScreen
    {

        public long? SelectedDeclineReasonID { get; set; }
        private readonly LeadApplicationScreen _LeadApplicationScreen;
        public SelectDeclineReasonScreen(LeadApplicationScreen leadApplicationScreen)
        {
            InitializeComponent();
            _LeadApplicationScreen = leadApplicationScreen;
            LoadLookupData();
        }

        private void LoadLookupData()
        {
            DataTable dtStatus = Business.Insure.INGetDeclineReasons(_LeadApplicationScreen.LaData.AppData.ImportID);  //Methods.GetTableData("SELECT * FROM INDeclineReason");
            cmbDeclineReason.Populate(dtStatus, "Description", "ID");
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                

                SelectedDeclineReasonID = Convert.ToInt32(cmbDeclineReason.SelectedValue);
                OnDialogClose(_dialogResult);
                _LeadApplicationScreen._declineReasonID = SelectedDeclineReasonID;

                //date of decline
                DateTime declineDate = DateTime.Now;
                _LeadApplicationScreen.dteDateOfSale.Value = declineDate.Date;

                //_LeadApplicationScreen.dteSaleDate.Value = saleDate.Date;
                //string time = saleDate.TimeOfDay.Hours + ":" + saleDate.TimeOfDay.Minutes;
                //_LeadApplicationScreen.dteSaleTime.Value = time;

                _LeadApplicationScreen.LaData.AppData.DeclineReasonID = SelectedDeclineReasonID;
                _LeadApplicationScreen.btnSave.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void EmbriantComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Methods.EmbriantComboBoxPreviewKeyDown(sender, e);
        }

        private void cmbDeclineReason_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(cmbDeclineReason);
        }

        private void cmbDeclineReason_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                btnSelect.IsEnabled = false;
                if (cmbDeclineReason.SelectedValue != null && _LeadApplicationScreen.cmbAgent.SelectedValue != null)
                {
                    btnSelect.IsEnabled = true;
                }
                else
                {
                    btnSelect.ToolTip = _LeadApplicationScreen.btnSave.ToolTip;
                }
                
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

    }
}
