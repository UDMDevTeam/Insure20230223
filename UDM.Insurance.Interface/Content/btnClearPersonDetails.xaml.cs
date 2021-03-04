using System.Windows;

namespace UDM.Insurance.Interface.Content
{
    public partial class btnClearPersonDetails
    {
        public event RoutedEventHandler Click;

        public btnClearPersonDetails()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}
