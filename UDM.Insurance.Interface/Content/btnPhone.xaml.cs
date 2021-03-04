using System.Windows;

namespace UDM.Insurance.Interface.Content
{
    public partial class btnPhone
    {
        #region Dependency Properties

        public static readonly DependencyProperty IsPhoneActiveProperty =
        DependencyProperty.Register("IsPhoneActive", typeof(bool), typeof(btnPhone), new PropertyMetadata(false));

        public bool IsPhoneActive
        {
            get { return (bool)GetValue(IsPhoneActiveProperty); }
            set { SetValue(IsPhoneActiveProperty, value); }
        }

        #endregion

        public event RoutedEventHandler Click;
        //private LeadApplicationData LaData;

        public btnPhone()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //LaData = (LeadApplicationData)DataContext;
        }
    }
}
