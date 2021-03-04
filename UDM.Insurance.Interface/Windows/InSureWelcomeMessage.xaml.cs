using System.Windows;
using Embriant.Framework;
using Embriant.WPF.Controls;

namespace UDM.Insurance.Interface.Windows
{
    /// <summary>
    /// Interaction logic for InSureWelcomeMessage.xaml
    /// </summary>
    public partial class InSureWelcomeMessage
    {
         #region Constructor
        public InSureWelcomeMessage()
        {
            InitializeComponent();
            
        }
        public InSureWelcomeMessage(BaseControl baseControl): base(baseControl)
        {
            InitializeComponent();
        }

        public InSureWelcomeMessage(BaseControl baseControl, string message, string headerText, ShowMessageType messageType)
            : base(baseControl, message, headerText, messageType)
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers
        /// <summary>
        /// Initializes all the controls with data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void INMessageBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
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

            //catch (Exception ex)
            //{
            //    //TODO Implement
            //}

            finally
            {
                if (true) { }
            }
        }

        /// <summary>
        /// Sets the DialogResult and closes the dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            DialogResult = true;
                //return(true);
            //}
            //catch (Exception ex)
            //{
            //    DialogResult = false;
            //    //HandleException(ex);
            //}
            //finally
            //{
                
            //}
        }
        #endregion

        private void buttonOK_Loaded(object sender, RoutedEventArgs e)
        {
            buttonOK.Focus();
        }
    }
}
