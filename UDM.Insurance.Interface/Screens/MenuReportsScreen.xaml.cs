using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Animation;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;
using UDM.Insurance.Interface.Views;
using Embriant.Framework.Configuration;
using UDM.WPF.Library;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Embriant.Framework;

namespace UDM.Insurance.Interface.Screens
{

    public partial class MenuReportsScreen
    {
        #region Private Members
        DataTable dtTurnoverUsers;
        #endregion Private Members

        public class ScreenData : ObservableObject
        {
            public class ToolTip : ObservableObject
            {
                private string _tip;
                public string Tip
                {
                    get { return _tip; }
                    set { SetProperty(ref _tip, value, () => Tip); }
                }
            }

            public ScreenData()
            {

            }

            private ObservableCollection<ToolTip> _toolTipData = new ObservableCollection<ToolTip>((new ToolTip[100]).Select(c => new ToolTip()));
            public ObservableCollection<ToolTip> ToolTipData
            {
                get { return _toolTipData; }
                set { SetProperty(ref _toolTipData, value, () => ToolTipData); }
            }
        }

        private ScreenData _SD = new ScreenData();
        public ScreenData SD
        {
            get { return _SD; }
            set { _SD = value; }
        }

        #region Constructor
        public MenuReportsScreen(ScreenDirection direction)
        {
            try
            {
                InitializeComponent();

#if TESTBUILD
                    TestControl.Visibility = Visibility.Visible;
#elif DEBUG
                DebugControl.Visibility = Visibility.Visible;
#elif TRAININGBUILD
                   TrainingControl.Visibility = Visibility.Visible;
#endif

                dtTurnoverUsers = Methods.ExecuteStoredProcedure("spGetTurnoverReportUsers", null).Tables[0];
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public MenuReportsScreen()
        {
        }
        #endregion Constructor

        #region Methods

        private void LoadToolTipData()
        {
            DataTable dt = Methods.GetTableData("SELECT * FROM Tooltips");

            if (dt.Rows.Count > 0)
            {
                int index = 1;
                foreach (DataRow row in dt.Rows)
                {
                    SD.ToolTipData[index].Tip = row["ToolTip"] as string;
                    index++;
                }
            }
        }

        #endregion Methods

        #region Event Handlers

        private void btnReportConfirmationFeedback_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportConfirmationFeedbackScreen reportConfirmationFeedbackScreen = new ReportConfirmationFeedbackScreen();
                ShowDialog(reportConfirmationFeedbackScreen, new INDialogWindow(reportConfirmationFeedbackScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportPremiumBreakdown_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportPremiumBreakdownScreen reportPremiumBreakdownScreen = new ReportPremiumBreakdownScreen();
                ShowDialog(reportPremiumBreakdownScreen, new INDialogWindow(reportPremiumBreakdownScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportLeadStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportLeadStatusScreen reportLeadStatusScreen = new ReportLeadStatusScreen();
                ShowDialog(reportLeadStatusScreen, new INDialogWindow(reportLeadStatusScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportLeadAllocation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportLeadAllocationScreen reportLeadAllocationScreen = new ReportLeadAllocationScreen();
                ShowDialog(reportLeadAllocationScreen, new INDialogWindow(reportLeadAllocationScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportBatchExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportBatchExportScreen reportBatchExportScreen = new ReportBatchExportScreen();
                ShowDialog(reportBatchExportScreen, new INDialogWindow(reportBatchExportScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportSalesToLead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportSalesToLeadScreen reportSalesToLeadScreen = new ReportSalesToLeadScreen();
                ShowDialog(reportSalesToLeadScreen, new INDialogWindow(reportSalesToLeadScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        //Base Conversion Report - DO NOT DELETE! 
        //private void btnReportBaseConversion_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        ReportBaseConversionScreen reportBaseConversionScreen = new ReportBaseConversionScreen();
        //        ShowDialog(reportBaseConversionScreen, new INDialogWindow(reportBaseConversionScreen));
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        private void btnReportLeadSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportLeadSearchScreen reportLeadSearchScreen = new ReportLeadSearchScreen();
                ShowDialog(reportLeadSearchScreen, new INDialogWindow(reportLeadSearchScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportBumpUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportBumpUpScreen reportBumpUpScreen = new ReportBumpUpScreen();
                ShowDialog(reportBumpUpScreen, new INDialogWindow(reportBumpUpScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportSalary_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportSalaryScreen reportSalaryScreen = new ReportSalaryScreen(SalaryReportMode.Default); //
                ShowDialog(reportSalaryScreen, new INDialogWindow(reportSalaryScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportSalaryChecking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Insure.UserHasAccessToReport(GlobalSettings.ApplicationUser.ID))
                {
                    ReportSalaryScreen reportSalaryScreen = new ReportSalaryScreen(SalaryReportMode.Checking);
                    ShowDialog(reportSalaryScreen, new INDialogWindow(reportSalaryScreen));
                }
                else
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Access denied: You do not have sufficient privileges to view this report.", "Access Denied", Embriant.Framework.ShowMessageType.Error);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportBatchStats_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportBatchStatsScreen reportBatchStatsScreen = new ReportBatchStatsScreen();
                ShowDialog(reportBatchStatsScreen, new INDialogWindow(reportBatchStatsScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MenuManagementScreen menuManagementScreen = new MenuManagementScreen(ScreenDirection.Reverse);
            OnClose(menuManagementScreen);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MenuManagementScreen menuManagementScreen = new MenuManagementScreen(ScreenDirection.Reverse);
            OnClose(menuManagementScreen);
        }

        private void btnReportPremiumBreakdownAgent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportPremiumBreakdownAgentScreen reportPremiumBreakdownAgentScreen = new ReportPremiumBreakdownAgentScreen();
                reportPremiumBreakdownAgentScreen.Show();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        //private void btnReportHours_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        ReportHoursScreen reportHoursScreen = new ReportHoursScreen();
        //        ShowDialog(reportHoursScreen, new INDialogWindow(reportHoursScreen));
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        private void btnReportReducedPremium_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportReducedPremiumScreen reportReducedPremiumScreen = new ReportReducedPremiumScreen();
                ShowDialog(reportReducedPremiumScreen, new INDialogWindow(reportReducedPremiumScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportDiary_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportDiaryScreen reportDiaryScreen = new ReportDiaryScreen();
                ShowDialog(reportDiaryScreen, new INDialogWindow(reportDiaryScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportConfirmedSales_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportConfirmedSalesScreen reportConfirmedSalesScreen = new ReportConfirmedSalesScreen();
                ShowDialog(reportConfirmedSalesScreen, new INDialogWindow(reportConfirmedSalesScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportLeadFeedback_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportLeadAnalysisScreen reportLeadAnalysisScreen = new ReportLeadAnalysisScreen();
                ShowDialog(reportLeadAnalysisScreen, new INDialogWindow(reportLeadAnalysisScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportStatusLoading_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportStatusLoadingScreen reportStatusLoadingScreen = new ReportStatusLoadingScreen();
                ShowDialog(reportStatusLoadingScreen, new INDialogWindow(reportStatusLoadingScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportBatchAnalysis_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportBatchAnalysisScreen reportBatchAnalysisScreen = new ReportBatchAnalysisScreen();
                ShowDialog(reportBatchAnalysisScreen, new INDialogWindow(reportBatchAnalysisScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportIndividualFallOff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportIndividualFallOffScreen reportIndividualFallOffScreen = new ReportIndividualFallOffScreen();
                ShowDialog(reportIndividualFallOffScreen, new INDialogWindow(reportIndividualFallOffScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportFallOff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportFallOffScreen reportFallOffScreen = new ReportFallOffScreen();
                ShowDialog(reportFallOffScreen, new INDialogWindow(reportFallOffScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportLeadActivity_Click(object sender, RoutedEventArgs e)
        {
            ReportLeadActivityScreen reportLeadActivityScreen = new ReportLeadActivityScreen();
            ShowDialog(reportLeadActivityScreen, new INDialogWindow(reportLeadActivityScreen));
        }

        private void btnReportBatch_Click(object sender, RoutedEventArgs e)
        {
            ReportBatchScreen reportBatchScreen = new ReportBatchScreen();
            ShowDialog(reportBatchScreen, new INDialogWindow(reportBatchScreen));
        }

        private void btnReportNoContact_Click(object sender, RoutedEventArgs e)
        {
            ReportNoContactScreen reportNoContactScreen = new ReportNoContactScreen();
            ShowDialog(reportNoContactScreen, new INDialogWindow(reportNoContactScreen));
        }

        private void btnReportExtensionTracking_Click(object sender, RoutedEventArgs e)
        {
            ReportExtensionTracking reportExtensionTracking = new ReportExtensionTracking();
            ShowDialog(reportExtensionTracking, new INDialogWindow(reportExtensionTracking));
        }

        private void btnReportSalesAgent_Click(object sender, RoutedEventArgs e)
        {
            ReportSalesScreen reportSalesScreen = new ReportSalesScreen();
            ShowDialog(reportSalesScreen, new INDialogWindow(reportSalesScreen));
        }

        private void btnReportConfirmationStats_Click(object sender, RoutedEventArgs e)
        {
            DataTable dtUserType = Methods.GetTableData("select FKUserType from dbo.[User] where ID = " + GlobalSettings.ApplicationUser.ID);
            long UserTypeID = long.Parse(dtUserType.Rows[0]["FKUserType"].ToString());
            ReportConfirmationStatsScreen reportConfirmationStatsScreen = new ReportConfirmationStatsScreen(UserTypeID);
            ShowDialog(reportConfirmationStatsScreen, new INDialogWindow(reportConfirmationStatsScreen));
        }

        private void btnAgentActivity_Click(object sender, RoutedEventArgs e)
        {
            AgentActivity agentScreen = new AgentActivity();
            ShowDialog(agentScreen, new INDialogWindow(agentScreen));
        }

        private void btnDailySales_Click(object sender, RoutedEventArgs e)
        {
            ReportDailySales dailySales = new ReportDailySales();
            ShowDialog(dailySales, new INDialogWindow(dailySales));
        }

        private void btnAvailableLeads_Click(object sender, RoutedEventArgs e)
        {
            ReportLeadsAvailableScreen reportLeadsAvailableScreen = new ReportLeadsAvailableScreen();
            ShowDialog(reportLeadsAvailableScreen, new INDialogWindow(reportLeadsAvailableScreen));
        }

        private void btnTargetTracker_Click(object sender, RoutedEventArgs e)
        {
            ReportTargetTrackerScreen reportTargetTrackerScreen = new ReportTargetTrackerScreen();
            ShowDialog(reportTargetTrackerScreen, new INDialogWindow(reportTargetTrackerScreen));
        }

        private void btnLoadingSummary_Click(object sender, RoutedEventArgs e)
        {
            ReportStatusLoadingSummary summary = new ReportStatusLoadingSummary();
            ShowDialog(summary, new INDialogWindow(summary));
        }

        private void btnReportSupervisor_Click(object sender, RoutedEventArgs e)
        {
            ReportSupervisorTeam team = new ReportSupervisorTeam();
            ShowDialog(team, new INDialogWindow(team));
        }

        private void btnReportBumpUpPotential_Click(object sender, RoutedEventArgs e)
        {
            ReportBumpUpPotentialScreen reportBumpUpPotentialScreen = new ReportBumpUpPotentialScreen();
            ShowDialog(reportBumpUpPotentialScreen, new INDialogWindow(reportBumpUpPotentialScreen));
        }

        private void btnTrackUpgradeBasePolicies_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportTrackUpgradeBasePolicies reportTrackUpgradeBasePolicies = new ReportTrackUpgradeBasePolicies();
                ShowDialog(reportTrackUpgradeBasePolicies, new INDialogWindow(reportTrackUpgradeBasePolicies));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportEliteConfirmationProgress_Click(object sender, RoutedEventArgs e)
        {
            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/216354979/comments
            ReportEliteConfirmationProgressScreen reportEliteConfirmationProgressScreen = new ReportEliteConfirmationProgressScreen();
            ShowDialog(reportEliteConfirmationProgressScreen, new INDialogWindow(reportEliteConfirmationProgressScreen));
        }

        private void btnReportRedeemedGifts_Click(object sender, RoutedEventArgs e)
        {
            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/216427104/comments
            //INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
            //ShowMessageBox(messageWindow, "This report is currently undergoing development and is not available as yet.", "COMING SOON!", Embriant.Framework.ShowMessageType.Exclamation);
            ReportRedeemedGiftsScreen reportRedeemedGiftsScreen = new ReportRedeemedGiftsScreen();
            ShowDialog(reportRedeemedGiftsScreen, new INDialogWindow(reportRedeemedGiftsScreen));
        }

        private void btnReportPODDiaries_Click(object sender, RoutedEventArgs e)
        {
            //See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/217293594/comments
            ReportPODDiariesScreen reportPODDiariesScreen = new ReportPODDiariesScreen();
            ShowDialog(reportPODDiariesScreen, new INDialogWindow(reportPODDiariesScreen));
        }

        private void btnReporRedeemedVsPOD_Click(object sender, RoutedEventArgs e)
        {
            ReportRedeemedVsPODScreen reportRedeemedVsPODScreen = new ReportRedeemedVsPODScreen();
            ShowDialog(reportRedeemedVsPODScreen, new INDialogWindow(reportRedeemedVsPODScreen));
        }

        private void xtcReportMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                                Keyboard.Focus(btnAgentActivity);
                            };
                            menuSB.Begin();
                        }

                        break;

                    case "tiAdministration":

                        menuSB = FindResource("MenuReportsAdministration") as Storyboard;

                        if (menuSB != null)
                        {
                            menuSB.Completed += delegate
                            {
                                Keyboard.Focus(btnAvailableLeads);
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
                                Keyboard.Focus(btnReportBatchStats);
                            };
                            menuSB.Begin();
                        }

                        break;
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadToolTipData();
        }

        private void btnDeclineReport_Click(object sender, RoutedEventArgs e)
        {
            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/218366743/comments
            ReportDeclineScreen reportDeclineScreen = new ReportDeclineScreen();
            ShowDialog(reportDeclineScreen, new INDialogWindow(reportDeclineScreen));
        }

        private void btnReportDiaryToFinal_Click(object sender, RoutedEventArgs e)
        {
            ReportDiaryToFinalScreen reportDiaryToFinalScreen = new ReportDiaryToFinalScreen();
            ShowDialog(reportDiaryToFinalScreen, new INDialogWindow(reportDiaryToFinalScreen));
        }

        private void btnConfirmedSalesByAgent_Click(object sender, RoutedEventArgs e)
        {
            ReportConfirmedSalesPerAgentScreen reportConfirmedSalesPerAgentScreen = new ReportConfirmedSalesPerAgentScreen();
            ShowDialog(reportConfirmedSalesPerAgentScreen, new INDialogWindow(reportConfirmedSalesPerAgentScreen));
        }

        private void btnInvalidAccountReport_Click(object sender, RoutedEventArgs e)
        {
            ReportInvalidAccountsScreen reportInvalidAccountsScreen = new ReportInvalidAccountsScreen();
            ShowDialog(reportInvalidAccountsScreen, new INDialogWindow(reportInvalidAccountsScreen));
        }

        private void btnReportCallMonitoringBumpUps_Click(object sender, RoutedEventArgs e)
        {
            ReportCallMonitoringBumpUpsScreen reportCallMonitoringBumpUpsScreen = new ReportCallMonitoringBumpUpsScreen();
            ShowDialog(reportCallMonitoringBumpUpsScreen, new INDialogWindow(reportCallMonitoringBumpUpsScreen));
        }

        private void btnReportCallMonitoring_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportCallMonitoringScreen reportCallMonitoringScreen = new ReportCallMonitoringScreen();
                ShowDialog(reportCallMonitoringScreen, new INDialogWindow(reportCallMonitoringScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnTopTenReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportTopTenScreen reportTopTenScreen = new ReportTopTenScreen();
                ShowDialog(reportTopTenScreen, new INDialogWindow(reportTopTenScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportCarriedForward_Click(object sender, RoutedEventArgs e)
        {
            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/222663199/comments
            try
            {
                DataTable dtUserType = Methods.GetTableData("select FKUserType from dbo.[User] where ID = " + GlobalSettings.ApplicationUser.ID);
                long UserTypeID = long.Parse(dtUserType.Rows[0]["FKUserType"].ToString());
                ReportCarriedForwardScreen reportCarriedForwardScreen = new ReportCarriedForwardScreen(UserTypeID);
                ShowDialog(reportCarriedForwardScreen, new INDialogWindow(reportCarriedForwardScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportBumpUpStats_Click(object sender, RoutedEventArgs e)
        {
            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/222821633/comments#374523641
            try
            {
                ReportBumpUpStatsScreen reportBumpUpStatsScreen = new ReportBumpUpStatsScreen(0);
                ShowDialog(reportBumpUpStatsScreen, new INDialogWindow(reportBumpUpStatsScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnTurnoverReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //IEnumerable<int> arrTurnoverUsers;

                //foreach (DataRow row in dtTurnoverUsers.Rows)
                //{

                //}
                bool contains = dtTurnoverUsers.AsEnumerable().Any(RowDefinition => (int)GlobalSettings.ApplicationUser.ID == RowDefinition.Field<long>("ID"));
                if (contains)
                {
                    ReportTurnoverScreen reportTurnoverScreen = new ReportTurnoverScreen();
                    ShowDialog(reportTurnoverScreen, new INDialogWindow(reportTurnoverScreen));
                }
                else
                {
                    INMessageBoxWindow1 messageBox = new INMessageBoxWindow1();
                    TextBlock txtDescription = LogicalTreeHelper.FindLogicalNode(messageBox, "txtDescription") as TextBlock;
                    if (txtDescription != null) txtDescription.TextAlignment = TextAlignment.Center;
                    ShowMessageBox(messageBox, "\n\nYou don't have permission to access this report.", "Access Denied", ShowMessageType.Error);
                }

            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnCallMonitoringActivity_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportCallMonitoringActivity reportCallMonitoringActivity = new ReportCallMonitoringActivity();
                ShowDialog(reportCallMonitoringActivity, new INDialogWindow(reportCallMonitoringActivity));
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnDailySalesChecking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (Insure.UserHasAccessToReport(GlobalSettings.ApplicationUser.ID))
                //{
                ReportDailySalesChecking reportDailySalesChecking = new ReportDailySalesChecking(); //SalaryReportMode.Checking
                ShowDialog(reportDailySalesChecking, new INDialogWindow(reportDailySalesChecking));
                //}
                //else
                //{
                //    ShowMessageBox(new INMessageBoxWindow1(), "Access denied: You do not have sufficient privileges to view this report.", "Access Denied", Embriant.Framework.ShowMessageType.Error);
                //}
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnPermissionQuestionReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportPermissionQuestionScreen reportPermissionQuestion = new ReportPermissionQuestionScreen();
                ShowDialog(reportPermissionQuestion, new INDialogWindow(reportPermissionQuestion));

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnSalesTrackingReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportSalesTrackingScreen reportSalesTrackingScreen = new ReportSalesTrackingScreen();
                ShowDialog(reportSalesTrackingScreen, new INDialogWindow(reportSalesTrackingScreen));

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnBaseSalesContactTrackingReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportBaseSalesContactTrackingScreen reportBaseSalesContactTrackingScreen = new ReportBaseSalesContactTrackingScreen();
                ShowDialog(reportBaseSalesContactTrackingScreen, new INDialogWindow(reportBaseSalesContactTrackingScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnConversionSalesTrackingReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportConversionSalesTracking reportConversionSalesTracking = new ReportConversionSalesTracking();
                ShowDialog(reportConversionSalesTracking, new INDialogWindow(reportConversionSalesTracking));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnUpgradeLeadPremiumReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportUpgradeLeadPremiumScreen reportUpgradeLeadPremiumScreen = new ReportUpgradeLeadPremiumScreen();
                ShowDialog(reportUpgradeLeadPremiumScreen, new INDialogWindow(reportUpgradeLeadPremiumScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void BtnDateAnalysisReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportDateAnalysisScreen reportDateAnalysisScreen = new ReportDateAnalysisScreen();
                ShowDialog(reportDateAnalysisScreen, new INDialogWindow(reportDateAnalysisScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void BtnEMailRequestsReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportEMailRequestsScreen reportEMailRequestsScreen = new ReportEMailRequestsScreen();
                ShowDialog(reportEMailRequestsScreen, new INDialogWindow(reportEMailRequestsScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void BtnCustomerCareRequestsReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportCustomerCareRequestsScreen reportCustomerCareRequestsScreen = new ReportCustomerCareRequestsScreen();
                ShowDialog(reportCustomerCareRequestsScreen, new INDialogWindow(reportCustomerCareRequestsScreen));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


        #endregion Event Handlers

        private void btnReportDeclineNoContact_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportDeclineNoContact reportDeclineNoContact = new ReportDeclineNoContact();
                OnClose(reportDeclineNoContact);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnWebReport_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportWebScreen reportweb = new ReportWebScreen();
                ShowDialog(reportweb, new INDialogWindow(reportweb));

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnQAAssessmentReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportQAWeightingsScreen reportQAWeightingsScreen = new ReportQAWeightingsScreen();
                ShowDialog(reportQAWeightingsScreen, new INDialogWindow(reportQAWeightingsScreen));

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnRemovedLeadsReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportRemoveDoNotContactLeads reportRemoveDoNotContactLeads = new ReportRemoveDoNotContactLeads();
                ShowDialog(reportRemoveDoNotContactLeads, new INDialogWindow(reportRemoveDoNotContactLeads));

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReportPermissionLead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportLeadPermission reportLeadPermission = new ReportLeadPermission();
                ShowDialog(reportLeadPermission, new INDialogWindow(reportLeadPermission));

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnDebiCheckReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportDebiCheckPL reportLeadPermission = new ReportDebiCheckPL();
                ShowDialog(reportLeadPermission, new INDialogWindow(reportLeadPermission));

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnDebiCheckTrackingReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportDebiCheckTracker reportDebiCheckTracker = new ReportDebiCheckTracker();
                ShowDialog(reportDebiCheckTracker, new INDialogWindow(reportDebiCheckTracker));

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnDebiCheckTrackingTSRReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportDebiCheckTrackingTSR reportDebiCheckTracker = new ReportDebiCheckTrackingTSR();
                ShowDialog(reportDebiCheckTracker, new INDialogWindow(reportDebiCheckTracker));

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnAcceptedDebiCheckStatusReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportDebiCheckAccepted reportDebiCheckAccepted = new ReportDebiCheckAccepted();
                ShowDialog(reportDebiCheckAccepted, new INDialogWindow(reportDebiCheckAccepted));

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnTSRDebiCHeckCallTransferReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportDebiCheckCallTransfer reportDebiCheckAccepted = new ReportDebiCheckCallTransfer();
                ShowDialog(reportDebiCheckAccepted, new INDialogWindow(reportDebiCheckAccepted));

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnTSRDebiCheckStatisticsReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportDCTransferSales reportDebiCheckAccepted = new ReportDCTransferSales();
                ShowDialog(reportDebiCheckAccepted, new INDialogWindow(reportDebiCheckAccepted));

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnDebiCheckSpecialistLogsReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReportDCSpecialistLogs reportDebiCheckAccepted = new ReportDCSpecialistLogs();
                ShowDialog(reportDebiCheckAccepted, new INDialogWindow(reportDebiCheckAccepted));

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}
