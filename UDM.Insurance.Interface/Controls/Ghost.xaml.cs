using Embriant.WPF.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Controls
{
    public partial class Ghost : UserControl
    {
        //public event RoutedEventHandler MouseEnter;

        public Ghost()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //var parent = Methods.FindLogicalParent<BaseControl>(this);
        }

        private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var parent = Methods.FindLogicalParent<BaseControl>(this);

            if (parent != null)
            {
                var grid = (Grid)parent.FindName("LayoutRoot");

                if (grid != null)
                {
                    grid.Opacity = 0;
                }
            }
        }

        private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var parent = Methods.FindLogicalParent<BaseControl>(this);

            if (parent != null)
            {
                var grid = (Grid)parent.FindName("LayoutRoot");

                if (grid != null)
                {
                    grid.Opacity = 1;
                }
            }
        }
    }
}
