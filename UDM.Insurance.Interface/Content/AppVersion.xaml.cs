using Embriant.Framework.Configuration;
using System.Windows;

namespace UDM.Insurance.Interface.Content
{

    public partial class AppVersion
    {
        public AppVersion()
        {
            InitializeComponent();
        }

        private void txtVersionNumber_Loaded(object sender, RoutedEventArgs e)
        {
            txtVersionNumber.Text = "Version " + GlobalSettings.ApplicationVersion;
        }
    }

}
