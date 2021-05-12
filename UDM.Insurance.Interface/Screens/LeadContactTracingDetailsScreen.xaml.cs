using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Embriant.WPF.Controls;
using UDM.Insurance.Interface.Data;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    public partial class LeadContactTracingDetailsScreen
    {
        #region Constants



        #endregion


        #region Dependency Properties

        //public long? ScriptID
        //{
        //    get { return (long?)GetValue(ScriptIDProperty); }
        //    set { SetValue(ScriptIDProperty, value); }
        //}
        //public static readonly DependencyProperty ScriptIDProperty = DependencyProperty.Register("ScriptID", typeof(long?), typeof(ScriptScreen), new UIPropertyMetadata(null));

        //public lkpScriptType? ScriptType
        //{
        //    get { return (lkpScriptType?)GetValue(ScriptTypeProperty); }
        //    set { SetValue(ScriptTypeProperty, value); }
        //}
        //public static readonly DependencyProperty ScriptTypeProperty = DependencyProperty.Register("ScriptType", typeof(lkpScriptType?), typeof(ScriptScreen), new UIPropertyMetadata(null));

        //public long? ScriptLanguageID
        //{
        //    get { return (long?)GetValue(ScriptLanguageIDProperty); }
        //    set { SetValue(ScriptLanguageIDProperty, value); }
        //}
        //public static readonly DependencyProperty ScriptLanguageIDProperty = DependencyProperty.Register("ScriptLanguageID", typeof(long?), typeof(ScriptScreen), new UIPropertyMetadata(null));

        #endregion


        #region Members

        public LeadApplicationData LaData;

        DispatcherTimer timer = new DispatcherTimer();

        #endregion


        #region Constructors

        public LeadContactTracingDetailsScreen()
        {
            InitializeComponent();

            timer.Tick += TimerTick;
            timer.Interval = TimeSpan.FromSeconds(1);
        }

        #endregion


        #region Private methods

        private void TimerTick(object sender, EventArgs e)
        {
            pbStatus.Value++;
        }


        private void DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ImportID", LaData.AppData.ImportID);

                DataTable dtContactNumbers = Methods.ExecuteStoredProcedure2("spINGetLeadContactTracingNumbers", parameters, IsolationLevel.Snapshot, 300).Tables[0];

                e.Result = dtContactNumbers;
            }

            catch (Exception ex)
            {
                string message = ex.Message + "\n\n" + ex.InnerException?.Message;

                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(this, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });

                e.Cancel = true;
            }
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                DataTable dtContactNumbers = (DataTable)e?.Result;
                xdgContactTracingDetails.DataSource = dtContactNumbers.AsEnumerable();


                grdProgress.Visibility = Visibility.Collapsed;
                timer.Stop();
            }

            catch (Exception ex)
            {
                string message = ex.Message + "\n\n" + ex.InnerException?.Message;

                //Dispatcher.Invoke(() =>
                //{
                MessageBox.Show(this, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                bdrDataGrid.Background = Brushes.Red;
                //});
            }

            finally
            {
                timer.Stop();
                grdProgress.Visibility = Visibility.Collapsed;
            }
        }

        #endregion


        #region Event Handlers

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.MainWindow.Opacity = 1;
            //Application.Current.MainWindow.ShowInTaskbar = true;

            Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LaData?.AppData?.ImportID != null)
                {
                    BackgroundWorker bgw = new BackgroundWorker();

                    bgw.WorkerReportsProgress = true;
                    bgw.WorkerSupportsCancellation = true;
                    bgw.DoWork += DoWork;
                    bgw.RunWorkerCompleted += RunWorkerCompleted;

                    grdProgress.Visibility = Visibility.Visible;
                    bgw.RunWorkerAsync();

                    timer.Start();
                }
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }



        #endregion


    }
}
