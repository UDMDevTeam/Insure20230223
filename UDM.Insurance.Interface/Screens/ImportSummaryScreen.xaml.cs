using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;

namespace UDM.Insurance.Interface.Screens
{

    public partial class ImportSummaryScreen
    {

        #region Private Members

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        //private long _currentRecord = -1;

        #endregion



        #region Constructors

        public ImportSummaryScreen()
        {
            InitializeComponent();

            LoadBatchData();
            
            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(200);

            #if TESTBUILD
                TestControl.Visibility = Visibility.Visible;
            #elif DEBUG
                DebugControl.Visibility = Visibility.Visible;
            #endif
        }

        #endregion



        #region Private Methods

        private void LoadBatchData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataTable dtImportSummary = Insure.INGetImportSummary().Tables[0];
                xdgImports.DataSource = dtImportSummary.DefaultView;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SetCursor(Cursors.Arrow);
            }
        }

        #endregion



        #region Event Handlers

        private void Timer1(object sender, EventArgs e)
        {
            //btnClose.Focus();
            //btnClose.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            //btnClose.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            //dispatcherTimer1.Stop();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            ImportProgressScreen importProgressScreen = new ImportProgressScreen();
            ShowDialog(importProgressScreen, new INDialogWindow(importProgressScreen));
            xdgImports.DataSource = null;
            LoadBatchData();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MenuLeadScreen menuLeadScreen = new MenuLeadScreen(ScreenDirection.Reverse);
            OnClose(menuLeadScreen);
        }

        private void xdgImports_Loaded(object sender, RoutedEventArgs e)
        {
            //dispatcherTimer1.Start();
        }

        private void xdgImports_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //var xamDataGridControl = (XamDataGrid)sender;

            //if (xamDataGridControl.ActiveRecord != null)
            //{
            //    DataRecord currentRecord = (DataRecord)xamDataGridControl.ActiveRecord;
            //    DataRow drCurrentRecord = ((DataRowView)currentRecord.DataItem).Row;

            //    _currentRecord = currentRecord.Index;

            //    //UserDetailsScreen userDetailsScreen = new UserDetailsScreen(Convert.ToInt64(drCurrentRecord.ItemArray[0].ToString()));
            //    //ShowDialog(userDetailsScreen, new HRDialogWindow(userDetailsScreen));

            //    //xdgUsers.DataSource = null;
            //    LoadBatchData();
            //}
        }

        private void xdgImports_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Return)
            //{
            //    var xamDataGridControl = (XamDataGrid)sender;

            //    if (xamDataGridControl.ActiveRecord != null)
            //    {
            //        DataRecord currentRecord = (DataRecord)xamDataGridControl.ActiveRecord;
            //        DataRow drCurrentRecord = ((DataRowView)currentRecord.DataItem).Row;

            //        _currentRecord = currentRecord.Index;

            //        //UserDetailsScreen userDetailsScreen = new UserDetailsScreen(Convert.ToInt64(drCurrentRecord.ItemArray[0].ToString()));
            //        //ShowDialog(userDetailsScreen, new HRDialogWindow(userDetailsScreen));

            //        //xdgUsers.DataSource = null;
            //        LoadBatchData();
            //    }
            //}
        }

        private void btnImport_Loaded(object sender, RoutedEventArgs e)
        {
            btnImport.Focus();
        }

        private void xdgImports_RecordsInViewChanged(object sender, Infragistics.Windows.DataPresenter.Events.RecordsInViewChangedEventArgs e)
        {
            //if (_currentRecord != -1) //Center previously displayed record in grid
            //{
            //    //foreach (DataRecord dr in xdgUsers.Records)
            //    //{
            //    //    if (dr.Index == _currentRecord)
            //    //    {
            //    //        dr.IsActive = true;
            //    //        dr.IsSelected = true;

            //    //        xdgUsers.BringRecordIntoView(dr);

            //    //        xdgUsers.ExecuteCommand(DataPresenterCommands.RecordLastDisplayed);
            //    //        for (int i = 0; i <= 10; i++)
            //    //        {
            //    //            if ((xdgUsers.ActiveRecord.Index == _currentRecord + i))
            //    //            {
            //    //                for (int j = 0; j <= 10 - i; j++)
            //    //                {
            //    //                    xdgUsers.ExecuteCommand(DataPresenterCommands.RecordNext);
            //    //                }

            //    //                dr.IsActive = true;
            //    //                dr.IsSelected = true;
            //    //                _currentRecord = -1;
            //    //                return;
            //    //            }
            //    //        }

            //    //        dr.IsActive = true;
            //    //        dr.IsSelected = true;
            //    //        _currentRecord = -1;
            //    //        return;
            //    //    }
            //    //}
            //}
        }

        #endregion

        private void btnImportSchedule_Loaded(object sender, RoutedEventArgs e)
        {
            btnImportSchedule.Focus();
        }

        private void btnImportSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ImportSchedule importSchedule = new ImportSchedule();
                ShowDialog(importSchedule, new INDialogWindow(importSchedule));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnImportIGGiftClaims_Click(object sender, RoutedEventArgs e)
        {
            ImportProgressGiftClaims importProgressGiftClaims = new ImportProgressGiftClaims(lkpGiftClaimsImportType.Normal);
            ShowDialog(importProgressGiftClaims, new INDialogWindow(importProgressGiftClaims));
        }

        private void btnImportIGGiftClaimsSMS_Click(object sender, RoutedEventArgs e)
        {
            ImportProgressGiftClaims importProgressGiftClaims = new ImportProgressGiftClaims(lkpGiftClaimsImportType.SMS);
            ShowDialog(importProgressGiftClaims, new INDialogWindow(importProgressGiftClaims));
        }

        private void btnConservedLeads_Loaded(object sender, RoutedEventArgs e)
        {
            ConservedLeadScreen CLS = new ConservedLeadScreen();
            ShowDialog(CLS, new INDialogWindow(CLS));
        }
    }
}
