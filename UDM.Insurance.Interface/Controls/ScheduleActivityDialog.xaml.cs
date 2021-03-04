
namespace UDM.Insurance.Interface.Controls
{
    public partial class ScheduleActivityDialog
    {
        public ScheduleActivityDialog()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogWindow.Close();
        }

        private void DialogWindow_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
