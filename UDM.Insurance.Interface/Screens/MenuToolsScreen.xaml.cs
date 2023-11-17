using Embriant.Framework;
using Embriant.Framework.Configuration;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Unity.Ioc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.PrismViews;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using Unity;

namespace UDM.Insurance.Interface.Screens
{
    public partial class MenuToolsScreen
    {
        private readonly SalesScreenGlobalData _ssGlobalData = new SalesScreenGlobalData();

        #region Constructor

        public MenuToolsScreen(ScreenDirection direction)
        {
            try
            {
                DataContext = this;

                EditClosureCommand = new DelegateCommand(EditClosureCommandExecute, EditClosureCommandCanExecute);

                InitializeComponent();
                //I commented the following out because whenever you closed one of the dialog windows the user would be returned to the first tab.
                //var menuSB = (Storyboard)FindResource("MenuToolsStory");
                //menuSB.Completed += menuSB_Completed;

                //menuSB.Begin();
                //if (direction == ScreenDirection.Reverse)
                //{
                //    menuSB.Pause();
                //    menuSB.SeekAlignedToLastTick(new TimeSpan(0, 0, 0, 0, 350));
                //    menuSB.Resume();
                //}

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

        private void btnSales_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SalesScreen salesScreen = new SalesScreen();
                OnClose(salesScreen);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void btnPrime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrimeLeadScreen PrimeScreen = new PrimeLeadScreen(null, _ssGlobalData,false);
                OnClose(PrimeScreen);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            var menuManagementScreen = new MenuManagementScreen(ScreenDirection.Reverse);
            OnClose(menuManagementScreen);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MenuManagementScreen menuManagementScreen = new MenuManagementScreen(ScreenDirection.Reverse);
            OnClose(menuManagementScreen);
        }

        private void btnCaptureHours_Click(object sender, RoutedEventArgs e)
        {
            HoursScreen hoursScreen = new HoursScreen(ScreenDirection.Reverse);
            OnClose(hoursScreen);
        }

        private void btnUtilityImportUpdate_Click(object sender, RoutedEventArgs e)
        {
            UtilityImportUpdateScreen utilityImportUpdateScreen = new UtilityImportUpdateScreen();
            utilityImportUpdateScreen.Show();
        }

        private void btnCallMonitoringSort_Click(object sender, RoutedEventArgs e)
        {
            CallMonitoringSortSummaryScreen callMonitoringSortSummaryScreen = new CallMonitoringSortSummaryScreen();
            OnClose(callMonitoringSortSummaryScreen);
        }

        private void btnBumpUpSort_Click(object sender, RoutedEventArgs e)
        {
            BumpUpSortSummaryScreen bumpUpSortSummaryScreen = new BumpUpSortSummaryScreen();
            OnClose(bumpUpSortSummaryScreen);
        }

        void menuSB_Completed(object sender, EventArgs e)
        {
            try
            {
                Keyboard.Focus(btnSales);
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                btnUtilityImportUpdate.Visibility = ((User)GlobalSettings.ApplicationUser).LoginName == "Admin" ? Visibility.Visible : Visibility.Hidden;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnCampaignLeadCopy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CampaignsLeadCopyScreen leadCopyScreen = new CampaignsLeadCopyScreen();
                ShowDialog(leadCopyScreen, new INDialogWindow(leadCopyScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }



        public DelegateCommand EditClosureCommand { get; private set; }
        private bool EditClosureCommandCanExecute()
        {
            List<long> _lstUserIDsWithAccess;

            string strResult = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 12", IsolationLevel.Snapshot).Rows[0][0].ToString();
            _lstUserIDsWithAccess = strResult.Split(',').Select(long.Parse).ToList();

            return _lstUserIDsWithAccess != null && _lstUserIDsWithAccess.Contains(GlobalSettings.ApplicationUser.ID) ? true : false;
        }
        private void EditClosureCommandExecute()
        {
            try
            {
                UnityContainerExtension container = (UnityContainerExtension)Application.Current.Resources["IoC"];
                IRegionManager regionManager = RegionManager.GetRegionManager(Application.Current.MainWindow);

                EditClosureScreen editClosureScreen = container.Resolve<EditClosureScreen>();
                editClosureScreen.Tag = "Active";

                IRegionManager scopedRegionManager = regionManager.CreateRegionManager();
                RegionManager.SetRegionManager(editClosureScreen, scopedRegionManager);

                ShowDialog(editClosureScreen, new INDialogWindow(editClosureScreen));
                editClosureScreen.Tag = null;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }



        private void btnMoveLeads_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MoveLeadsScreen moveLeadsScreen = new MoveLeadsScreen();
                ShowDialog(moveLeadsScreen, new INDialogWindow(moveLeadsScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnTsrTargets_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CaptureTSRTargetsScreen captureTsrTargetsScreen = new CaptureTSRTargetsScreen();
                ShowDialog(captureTsrTargetsScreen, new INDialogWindow(captureTsrTargetsScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


        private void btnCampaignTargets_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                CaptureCampaignTargetDefaults captureCampaignTargetDefaults = new CaptureCampaignTargetDefaults();
                ShowDialog(captureCampaignTargetDefaults, new INDialogWindow(captureCampaignTargetDefaults));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnSingleLeadDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //string userName = GlobalSettings.ApplicationUserName;
                long userID = ((User)GlobalSettings.ApplicationUser).ID;
                //long[] permittedUsers = { 1,2792,3662,427,1348,382,139 };
                //int pos = Array.IndexOf(permittedUsers, userID);

                List<long> permittedUsers;

                string strResult = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 17", IsolationLevel.Snapshot).Rows[0][0].ToString();
                permittedUsers = strResult.Split(',').Select(long.Parse).ToList();

                if (permittedUsers.Contains(userID))
                {
                    DeleteSingleLeadScreen deleteSingleLeadScreen = new DeleteSingleLeadScreen();
                    ShowDialog(deleteSingleLeadScreen, new INDialogWindow(deleteSingleLeadScreen));
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"You do not have permission to access this function!", "Access Denied", ShowMessageType.Error);
                    });
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void xtcReportMenu_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TabItem selectedTab = e.AddedItems[0] as TabItem;
            Storyboard menuSB;

            if (selectedTab != null)
            {
                switch (selectedTab.Name)
                {
                    case "tiSales":

                        menuSB = FindResource("MenuReportsSales") as Storyboard;

                        if (menuSB != null)
                        {
                            menuSB.Completed += delegate
                            {
                                Keyboard.Focus(btnSales);
                            };
                            menuSB.Begin();
                        }

                        break;

                    case "tiProcessing":

                        menuSB = FindResource("MenuReportsProcessing") as Storyboard;

                        if (menuSB != null)
                        {
                            menuSB.Completed += delegate
                            {
                                Keyboard.Focus(btnCaptureHours);
                            };
                            menuSB.Begin();
                        }

                        break;
                }
            }
        }
        private void btnEditMySuccess_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                MySuccessEditScreen mySuccessEditScreen = new MySuccessEditScreen();
                ShowDialog(mySuccessEditScreen, new INDialogWindow(mySuccessEditScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnDebiCheckStatusLoader_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                DebiCheckStatusLoader mySuccessEditScreen = new DebiCheckStatusLoader();
                ShowDialog(mySuccessEditScreen, new INDialogWindow(mySuccessEditScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnDebiCheckConfiguration_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalSettings.ApplicationUser.ID == 105 || GlobalSettings.ApplicationUser.ID == 1)
            {
                try
                {
                    DebiCheckConfigurationPage mySuccessEditScreen = new DebiCheckConfigurationPage();
                    ShowDialog(mySuccessEditScreen, new INDialogWindow(mySuccessEditScreen));
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
            else
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    ShowMessageBox(new INMessageBoxWindow1(), @"You do not have permission to access this function!", "Access Denied", ShowMessageType.Error);
                });
            }

        }
        #endregion

        private void btnDebiCheckBulkSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BulkMandateSend mySuccessEditScreen = new BulkMandateSend();
                ShowDialog(mySuccessEditScreen, new INDialogWindow(mySuccessEditScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnTransferAdjustments_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TransferAdjustments mySuccessEditScreen = new TransferAdjustments();
                ShowDialog(mySuccessEditScreen, new INDialogWindow(mySuccessEditScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnAutoAssignLeads_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AILeadsGeneration mySuccessEditScreen = new AILeadsGeneration();
                ShowDialog(mySuccessEditScreen, new INDialogWindow(mySuccessEditScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnNoContactSMS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NoContactSMS mySuccessEditScreen = new NoContactSMS();
                ShowDialog(mySuccessEditScreen, new INDialogWindow(mySuccessEditScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }

}
