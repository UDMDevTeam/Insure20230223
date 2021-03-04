using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using UDM.Insurance.Business;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{

    public partial class SaleDetailNotesScreen
    {

        private CallMonitoringDetailsScreen _CallMonitoringDetailsScreen;
        public SaleDetailNotesScreen(CallMonitoringDetailsScreen callMonitoringDetailsScreen)
        {
            InitializeComponent();
            
            _CallMonitoringDetailsScreen = callMonitoringDetailsScreen;
            tbxNotes.Text = callMonitoringDetailsScreen.SaleDetailNotes;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //OnDialogClose(_dialogResult);
        }

    }
}
