using System.Windows;

namespace UDM.Insurance.Interface.Content
{
    public partial class btnPhone2
    {
        #region Dependency Properties

        public static readonly DependencyProperty IsPhoneActive2Property =
        DependencyProperty.Register("IsPhoneActive2", typeof(bool), typeof(btnPhone2), new PropertyMetadata(false));

        public bool IsPhoneActive2
        {
            get { return (bool)GetValue(IsPhoneActive2Property); }
            set { SetValue(IsPhoneActive2Property, value); }
        }

        #endregion

        public event RoutedEventHandler Click;
        //private LeadApplicationData LaData;

        public btnPhone2()
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
