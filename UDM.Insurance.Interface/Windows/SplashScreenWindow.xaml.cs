using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;

namespace UDM.Insurance.Interface
{

	public partial class SplashScreenWindow
	{

        #region Members
        private DispatcherTimer _timer;
        #endregion

        #region Constructor
        public SplashScreenWindow()
		{
            try
            {
                InitializeComponent();
                Populate();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
		}
        #endregion

        #region Methods
        private void Populate()
        {
            try
            {
                Version version = GlobalSettings.ApplicationVersion;
                txtVersionNumber.Text = "Version " + version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;

                //Timer
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromSeconds(3);
                _timer.Tick += timer_Tick;

                //Global Settings
                GlobalSettings.Initialize(ApplicationName.UDM_Insure, ApplicationType.WPF);

                //Test Database connection
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += worker_DoWork;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync();
            }
                
            finally
            {
                if (true) {}
            }
        }
        #endregion

        #region Event Handlers
        private void timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            _timer = null;

            DialogResult = true;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Test Database connection
            e.Result = Database.TestConnection(GlobalSettings.DefaultConnectionSettings);
            //e.Result = true;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if ((bool)e.Result)
                {
                    _timer.Start();
                }
                else
                {
                    txtErrorMessage.Text = "Failed to connect to the database";
                    txtErrorHeading.Visibility = Visibility.Visible;
                    txtErrorMessage.Visibility = Visibility.Visible;
                    buttonClose.Visibility = Visibility.Visible;
                }
            }

            finally
            {
                if (true) { }
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        #endregion

	}

}