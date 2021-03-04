using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using UDM.Insurance.Business;

namespace UDM.Insurance.Interface.Screens
{
	/// <summary>
	/// Interaction logic for MenuManagementScreen.xaml
	/// </summary>
    public partial class MenuManagementScreen
	{

        #region Constructor
        public MenuManagementScreen(ScreenDirection direction)
		{
            try
            {
                InitializeComponent();
                var menuSB = (Storyboard)FindResource("MenuManagementStory");
                menuSB.Completed += menuSB_Completed;

                menuSB.Begin();
                if (direction == ScreenDirection.Reverse)
                {
                    menuSB.Pause();
                    menuSB.SeekAlignedToLastTick(new TimeSpan(0, 0, 0, 0, 350));
                    menuSB.Resume();
                }

                #if TESTBUILD
                    TestControl.Visibility = Visibility.Visible;
                #elif DEBUG
                    DebugControl.Visibility = Visibility.Visible;
                #elif TRAININGBUILD
                    TrainingControl.Visibility = Visibility.Visible;
                #endif
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
		}
        #endregion

        #region Event Handlers
        
        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuReportsScreen menuReportsScreen = new MenuReportsScreen(ScreenDirection.Forward);
                OnClose(menuReportsScreen);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

		private void btnTools_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuToolsScreen = new MenuToolsScreen(ScreenDirection.Forward);
                OnClose(menuToolsScreen);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

		private void btnLeads_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuLeadScreen = new MenuLeadScreen(ScreenDirection.Forward);
                OnClose(menuLeadScreen);
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            StartScreen startScreen = new StartScreen();
            OnClose(startScreen);
        }

        void menuSB_Completed(object sender, EventArgs e)
        {
            try
            {
                Keyboard.Focus(btnLeads);
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        #endregion

    }
}