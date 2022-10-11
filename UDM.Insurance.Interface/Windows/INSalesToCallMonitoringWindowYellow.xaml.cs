using System.Windows;
using Embriant.Framework;
using Embriant.WPF.Controls;

namespace UDM.Insurance.Interface.Windows
{
    public partial class INSalesToCallMonitoringWindowYellow
    {

        #region Constructor
        public INSalesToCallMonitoringWindowYellow()
        {
            InitializeComponent();
        }

        public INSalesToCallMonitoringWindowYellow(BaseControl baseControl): base(baseControl)
        {
            InitializeComponent();
        }

        public INSalesToCallMonitoringWindowYellow(BaseControl baseControl, string message, string headerText, ShowMessageType messageType): base(baseControl, message, headerText, messageType)
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        private void INMessageBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                txtHeading.Text = Header;
                txtDescription.Text = Message;
                switch (MessageType)
                {
                    case ShowMessageType.Error:
                        imageError.Visibility = Visibility.Visible;
                        break;
                    case ShowMessageType.Exclamation:
                        imageExclamation.Visibility = Visibility.Visible;
                        break;
                    case ShowMessageType.Information:
                        imageInformation.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;


        }
        #endregion

    }
}
