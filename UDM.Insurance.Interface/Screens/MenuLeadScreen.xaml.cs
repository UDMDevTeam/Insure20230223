using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using UDM.Insurance.Business;

namespace UDM.Insurance.Interface.Screens
{
	/// <summary>
	/// Interaction logic for MenuLeadScreen.xaml
	/// </summary>
    public partial class MenuLeadScreen
	{

        #region Constructor
        public MenuLeadScreen(ScreenDirection direction)
		{
            try
            {
                InitializeComponent();
                Storyboard menuSB = (Storyboard)FindResource("MenuLeadStory");
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

                #region Determining the visitbility of the test buttons

                //switch (((User)Embriant.Framework.Configuration.GlobalSettings.ApplicationUser).FKUserType)
                //{
                //    case (int)lkpUserType.Administrator:
                //    case (int)lkpUserType.Manager:
                //        btnAllocateFromOldToNew.Visibility = Visibility.Visible;
                //        break;

                //    case (int)lkpUserType.SalesAgent:
                //    case (int)lkpUserType.DataCapturer:
                //    case (int)lkpUserType.ConfirmationAgent:
                //        btnAllocateFromOldToNew.Visibility = Visibility.Collapsed;
                //        break;

                //    default:
                //        btnAllocateFromOldToNew.Visibility = Visibility.Collapsed;
                //        break;
                //}

                //if (Embriant.Framework.Configuration.GlobalSettings.ApplicationUser

                if (Insure.INCanUserDeAllocateLeads())
                {
                    btnDeAllocateLeads.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    btnDeAllocateLeads.Visibility = System.Windows.Visibility.Collapsed;
                }

                #endregion Determining the visitbility of the test buttons

            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
		}
        #endregion

        #region Event Handlers
        
        private void btnAssign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AssignSummaryScreen assignSummaryScreen = new AssignSummaryScreen();
                OnClose(assignSummaryScreen);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrintSummaryScreen printSummaryScreen = new PrintSummaryScreen();
                OnClose(printSummaryScreen);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        //private void btnExport_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        BatchExportScreen batchExportScreen = new BatchExportScreen();
        //        ShowDialog(batchExportScreen, new INDialogWindow(batchExportScreen));
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

		private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ImportSummaryScreen importSummaryScreen = new ImportSummaryScreen();
                OnClose(importSummaryScreen);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        //private void btnAllocateFromOldToNew_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        AllocateLeadsFromOldSystemScreen allocateLeadsFromOldSystemScreen = new AllocateLeadsFromOldSystemScreen();
        //        OnClose(allocateLeadsFromOldSystemScreen);
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        private void btnDeAllocateLeads_Click(object sender, RoutedEventArgs e)
        {
            DeAllocateLeadsScreen deAllocateLeadsScreen = new DeAllocateLeadsScreen();
            OnClose(deAllocateLeadsScreen);
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MenuManagementScreen menuManagementScreen = new MenuManagementScreen(ScreenDirection.Reverse);
            OnClose(menuManagementScreen);
        }

        void menuSB_Completed(object sender, EventArgs e)
        {
            try
            {
                Keyboard.Focus(btnImport);
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MenuManagementScreen menuManagementScreen = new MenuManagementScreen(ScreenDirection.Reverse);
            OnClose(menuManagementScreen);
        }

        #endregion Event Handlers

    }
}