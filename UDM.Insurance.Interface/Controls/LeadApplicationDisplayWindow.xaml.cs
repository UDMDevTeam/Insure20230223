using System.Windows.Input;

namespace UDM.Insurance.Interface.Controls
{
    public partial class LeadApplicationDisplayWindow
    {
        public LeadApplicationDisplayWindow()
        {
            InitializeComponent();
        }

        private void LeadApplicationDisplay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
