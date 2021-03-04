using System;
using System.Data;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{

    public partial class SelectCallMonitoringCancellationReasonScreen
    {

        #region Private Members

        private readonly LeadApplicationScreen _leadApplicationScreen;

        #endregion Private Members

        #region Public Members
        public long? CallMonitoringCancellationReasonID { get; set; }

        #endregion Public Members

        #region Constructor

        public SelectCallMonitoringCancellationReasonScreen(LeadApplicationScreen leadApplicationScreen)
        {
            InitializeComponent();
            LoadLookupData();
            _leadApplicationScreen = leadApplicationScreen;
        }

        #endregion Constructor


        private void LoadLookupData()
        {
            DataTable dtStatus = Methods.GetTableData("SELECT * FROM INCallMonitoringCancellationReason");
            cmbCallMonitoringCancellationReason.Populate(dtStatus, "Description", "ID");
        }

        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CallMonitoringCancellationReasonID = Convert.ToInt32(cmbCallMonitoringCancellationReason.SelectedValue);

                OnDialogClose(_dialogResult);
                _leadApplicationScreen.LaData.AppData.FKINCallMonitoringCancellationReasonID = CallMonitoringCancellationReasonID;
                _leadApplicationScreen.btnSave.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
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

        private void cmbCallMonitoringCancellationReason_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(cmbCallMonitoringCancellationReason);
        }

        private void cmbCallMonitoringCancellationReason_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;
                if (cmbCallMonitoringCancellationReason.SelectedValue != null && _leadApplicationScreen.cmbAgent.SelectedValue != null)
                {
                    btnSave.IsEnabled = true;
                }
                else
                {
                    btnSave.ToolTip = _leadApplicationScreen.btnSave.ToolTip;
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
