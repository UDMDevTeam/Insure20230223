using Embriant.WPF.Controls;
using UDM.WPF.Library;
using System.Windows.Input;
using UDM.Insurance.Interface.Windows;
using UDM.Insurance.Interface.PrismInfrastructure;
using System;

namespace UDM.Insurance.Interface.Screens
{

    public partial class ReportEMailRequestsScreen : BaseControl
    {

        public ReportEMailRequestsScreen()
        {
            InitializeComponent();
        }

        private void BaseControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ReportEMailRequestsScreenViewModel vm = (ReportEMailRequestsScreenViewModel)DataContext;
            vm._ea.GetEvent<SendDialogMessageEvent>().Subscribe(DialogMessageReceived);
            vm._ea.GetEvent<UpdateUIMessageEvent>().Subscribe(UpdateUIMessageReceived);
        }

        private void DialogMessageReceived(DialogMessage dm)
        {
            ShowMessageBox(new INMessageBoxWindow1(), dm.Message, dm.Title, dm.Type);
        }

        private void UpdateUIMessageReceived(UpdateUIMessage um)
        {
            switch (um.ControlName)
            {
                case "btnReport":
                    switch (um.Property)
                    {
                        case "Content":
                            btnReport.Content = um.Value;
                            break;

                        case "ToolTip":
                            btnReport.ToolTip = um.Value;
                            break;
                    }
                    break;
            }
        }

        private void EmbriantComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Methods.EmbriantComboBoxPreviewKeyDown(sender, e);
        }
    }
}
