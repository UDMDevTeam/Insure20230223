using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{

    public partial class SelectCancellationReasonScreen
    {

        public long? SelectedCancellationReasonID { get; set; }
        private LeadApplicationScreen _LeadApplicationScreen;
        public SelectCancellationReasonScreen(LeadApplicationScreen leadApplicationScreen)
        {
            InitializeComponent();
            LoadLookupData();
            _LeadApplicationScreen = leadApplicationScreen;
        }

        private void LoadLookupData()
        {
            DataTable dtStatus = Methods.GetTableData("SELECT * FROM INCancellationReason");
            cmbCancellationReason.Populate(dtStatus, "Description", "ID");
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedCancellationReasonID = Convert.ToInt32(cmbCancellationReason.SelectedValue);
                //DateTime saleDate = DateTime.Now;
                //_LeadApplicationScreen.dteDateOfSale.Value = saleDate.Date;
                //_LeadApplicationScreen.dteSaleDate.Value = saleDate;
               
                //string time = saleDate.TimeOfDay.Hours + ":" + saleDate.TimeOfDay.Minutes;
                //_LeadApplicationScreen.dteSaleTime.Value = time;

                OnDialogClose(_dialogResult);
                _LeadApplicationScreen.LaData.AppData.CancellationReasonID = SelectedCancellationReasonID;
                _LeadApplicationScreen.btnSave.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
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

        private void cmbCancellationReason_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(cmbCancellationReason);
        }

        private void cmbCancellationReason_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                btnSelect.IsEnabled = false;
                if (cmbCancellationReason.SelectedValue != null && _LeadApplicationScreen.cmbAgent.SelectedValue != null)
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
