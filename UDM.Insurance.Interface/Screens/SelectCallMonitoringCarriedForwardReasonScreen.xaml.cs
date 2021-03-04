using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{

    public partial class SelectCallMonitoringCarriedForwardReasonScreen
    {

        #region Private Members

        private LeadApplicationScreen _leadApplicationScreen;

        #endregion Private Members

        #region Public Members
        public long? SelectCallMonitoringCarriedForwardReasonID { get; set; }

        #endregion Public Members

        #region Constructor

        public SelectCallMonitoringCarriedForwardReasonScreen(LeadApplicationScreen leadApplicationScreen)
        {
            InitializeComponent();
            LoadLookupData();
            _leadApplicationScreen = leadApplicationScreen;
        }

        #endregion Constructor


        private void LoadLookupData()
        {
            DataTable dtStatus = Business.Insure.INGetCallMonitoringCarriedForwardReasons(); //Methods.GetTableData("SELECT * FROM INCarriedForwardReason");
            cmbCallMonitoringCarriedForwardReason.Populate(dtStatus, "Description", "ID");
        }

        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectCallMonitoringCarriedForwardReasonID = Convert.ToInt32(cmbCallMonitoringCarriedForwardReason.SelectedValue);

                OnDialogClose(_dialogResult);
                _leadApplicationScreen.LaData.AppData.FKINCallMonitoringCarriedForwardReasonID = SelectCallMonitoringCarriedForwardReasonID;
                _leadApplicationScreen.btnSave.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
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

        private void cmbCallMonitoringCarriedForwardReason_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(cmbCallMonitoringCarriedForwardReason);
        }

        private void cmbCallMonitoringCarriedForwardReason_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                btnSelect.IsEnabled = false;
                if (cmbCallMonitoringCarriedForwardReason.SelectedValue != null && _leadApplicationScreen.cmbAgent.SelectedValue != null)
                {
                    btnSelect.IsEnabled = true;
                }
                else
                {
                    btnSelect.ToolTip = _leadApplicationScreen.btnSave.ToolTip;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            
        }

        #endregion Event Handlers
    }
}
