using System;
using System.Data;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{

    public partial class SelectIDNumber
    {

        public string ExtraIDNumber { get; set; }
        private readonly LeadApplicationScreen _LeadApplicationScreen;
        public SelectIDNumber(LeadApplicationScreen leadApplicationScreen, string extraIDNumber)
        {
            InitializeComponent();
            _LeadApplicationScreen = leadApplicationScreen;
            LoadLookupData();

            if(_LeadApplicationScreen.LaData.BankDetailsData.IDNumber == null)
            {
                cmbIDNumberSelection.Text = extraIDNumber;
                ExtraIDNumber = extraIDNumber;
            }
            else
            {
                cmbIDNumberSelection.Text = _LeadApplicationScreen.LaData.BankDetailsData.IDNumber;
                ExtraIDNumber = _LeadApplicationScreen.LaData.BankDetailsData.IDNumber;
            }


        }

        private void LoadLookupData()
        {
            //DataTable dtStatus = Business.Insure.INGetDeclineReasons(_LeadApplicationScreen.LaData.AppData.ImportID);  //Methods.GetTableData("SELECT * FROM INDeclineReason");
            //cmbIDNumberSelection.Populate(dtStatus, "Description", "ID");
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                ExtraIDNumber = cmbIDNumberSelection.Text;
                OnDialogClose(_dialogResult);
                _LeadApplicationScreen.IDNumberPLLKP = ExtraIDNumber;
                _LeadApplicationScreen.LaData.BankDetailsData.IDNumber = ExtraIDNumber;


                //_LeadApplicationScreen.LaData.AppData.DeclineReasonID = ExtraIDNumber;
                //_LeadApplicationScreen.btnSave.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
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
            Keyboard.Focus(cmbIDNumberSelection);
        }



    }
}
