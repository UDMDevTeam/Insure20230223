using Embriant.WPF.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for LiveProductivity.xaml
    /// </summary>
    public partial class LiveProductivity : Window
    {
        public LiveProductivity()
        {
            InitializeComponent();
            ShowInTaskbar = false;

            //bool? dialogResult = null;

            //Window containingWindow = Window.GetWindow(this);

            //EmbriantMainWindow mainWindow = GetMainWindow();

            //if (containingWindow != null) containingWindow.IsEnabled = false;
            //dialogResult = mainWindow.ShowDialog(dialogWindow, containingWindow);
            //if (containingWindow != null) containingWindow.IsEnabled = true;

        }

        //private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    this.DragMove();
        //}

        private void xrgLiveProductivity_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }


        //private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    this.DragMove();
        //}

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
