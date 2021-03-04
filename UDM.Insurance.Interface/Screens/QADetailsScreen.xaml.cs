using Embriant.WPF.Controls;
using UDM.WPF.Library;
using System.Windows.Input;
using UDM.Insurance.Interface.Windows;
using UDM.Insurance.Interface.PrismInfrastructure;
using System.ComponentModel;

namespace UDM.Insurance.Interface.Screens
{

    public partial class QADetailsScreen : BaseControl
    {
        public static int SelectedQAManual;
        public static bool OverrideBool;
        public static bool BorderlineBool;


        public QADetailsScreen(long importID)
        {
            InitializeComponent();

            QADetailsScreenViewModel vm = new QADetailsScreenViewModel(importID);
            DataContext = vm;

        }

        private void BaseControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            QADetailsScreenViewModel vm = (QADetailsScreenViewModel)DataContext;
            vm._ea.GetEvent<SendDialogMessageEvent>().Subscribe(DialogMessageReceived);
            //vm._ea.GetEvent<UpdateUIMessageEvent>().Subscribe(UpdateUIMessageReceived);
        }

        private void DialogMessageReceived(DialogMessage dm)
        {
            ShowMessageBox(new INMessageBoxWindow1(), dm.Message, dm.Title, dm.Type);
        }

        private void UpdateUIMessageReceived(UpdateUIMessage um)
        {
            //switch (um.ControlName)
            //{
            //    case "btnReport":
            //        switch (um.Property)
            //        {
            //            case "Content":
            //                btnReport.Content = um.Value;
            //                break;

            //            case "ToolTip":
            //                btnReport.ToolTip = um.Value;
            //                break;
            //        }
            //        break;
            //}
        }

        private void EmbriantComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Methods.EmbriantComboBoxPreviewKeyDown(sender, e);
        }

        private void cmbQAID_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                SelectedQAManual = int.Parse(cmbQAID.SelectedValue.ToString());
            }
            catch { }
        }



        public void btnResult_Click_1()
        {
            lblResult.Text = QADetailsScreenViewModel.OverallPercentage.ToString();
        }

        private void BorderCheckbox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if(BorderCheckbox.IsChecked == true)
            {
                BorderlineBool = true;
            }
            else
            {
                BorderlineBool = false;
            }
        }

        private void OverridenCheckbox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OverridenCheckbox.IsChecked == true)
            {
                OverrideBool = true;
            }
            else
            {
                OverrideBool = false;
            }
        }
    }
}
