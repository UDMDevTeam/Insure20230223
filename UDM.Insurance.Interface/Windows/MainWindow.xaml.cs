using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Threading;
using Embriant.Framework;
using Embriant.Framework.Exceptions;
using UDM.Insurance.Interface.Screens;
using UDM.WPF.Windows;
using Prism.Regions;

namespace UDM.Insurance.Interface.Windows
{
    public partial class MainWindow : IExceptionHandler
    {
        protected bool _errorDialogVisible;
        VersionWindow versionWindow;

        #region Constructors

        public MainWindow() //IRegionManager regionManager
        {
            InitializeComponent();

            //regionManager.RegisterViewWithRegion("ContentRegion", typeof(EditClosureScreen));

            Width = SystemParameters.WorkArea.Width;
            Height = SystemParameters.WorkArea.Height;
            Left = 0;
            Top = 0;
            WindowState = WindowState.Normal;

            //InstallCTPhoneOCX();
        }

        //private void InstallCTPhoneOCX()
        //{
        //    var assembly = Assembly.GetExecutingAssembly();
        //    var resourceName = "UDM.Insurance.Interface.Resources.Other.ctphoneax.msi";
        //    var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //    var fileName = "ctphoneax.msi";
        //    var pathInstalled = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        //    var ocxFileName = "CTPhone_ActiveXControl.ocx";

        //    if (!File.Exists(pathInstalled + "\\" + ocxFileName))
        //    {
        //        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        //        {
        //            byte[] byteData = StreamToBytes(stream);
        //            File.WriteAllBytes(path + "\\" + fileName, byteData);

        //            Process process = new Process();
        //            //process.StartInfo.WorkingDirectory = path;
        //            process.StartInfo.FileName = "msiexec.exe";
        //            process.StartInfo.Arguments = string.Format(" /i \"{0}\" ALLUSERS=1", path + "\\" + fileName);
        //            //process.StartInfo.UseShellExecute = true;
        //            //process.StartInfo.CreateNoWindow = false;
        //            process.Start();
        //            process.WaitForExit();
        //        }
        //    }
        //}

        //static byte[] StreamToBytes(Stream input)
        //{
        //    int capacity = input.CanSeek ? (int)input.Length : 0; //Bitwise operator - If can seek, Capacity becomes Length, else becomes 0.
        //    using (MemoryStream output = new MemoryStream(capacity)) //Using the MemoryStream output, with the given capacity.
        //    {
        //        int readLength;
        //        byte[] buffer = new byte[capacity/*4096*/];  //An array of bytes
        //        do
        //        {
        //            readLength = input.Read(buffer, 0, buffer.Length);   //Read the memory data, into the buffer
        //            output.Write(buffer, 0, readLength); //Write the buffer to the output MemoryStream incrementally.
        //        }
        //        while (readLength != 0); //Do all this while the readLength is not 0
        //        return output.ToArray();  //When finished, return the finished MemoryStream object as an array.
        //    }
        //}

        #endregion

        #region Event Handlers

        private new void Window_Initialized(object sender, EventArgs e)
        {
            try
            {
                //var test = Title;

                _currentScreen = new StartScreen();
                _mainGrid = MainGrid;
                base.Window_Initialized(sender, e);
                ExceptionHandler.AddCustomHandler(this);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public AfterHandleAction HandleException(Exception ex, ExceptionData data)
        {
            if (!_errorDialogVisible)
            {
                _errorDialogVisible = true;

                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += ShowErrorMessage;
                worker.RunWorkerCompleted += ShowErrorMessageComplete;
                worker.RunWorkerAsync(data);

                _errorDialogVisible = false;
            }
            return AfterHandleAction.ContinueHandling;
        }

        public void ShowErrorMessage(object sender, DoWorkEventArgs e)
        {
            ExceptionData exceptionData = (ExceptionData) e.Argument;

            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart) (() => ShowMessageBox(new INMessageBoxWindow1(), exceptionData.ErrorMessage, "An error has occurred", ShowMessageType.Error)));
        }

        private void ShowErrorMessageComplete(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void EmbriantMainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (_currentScreen.Name == "LeadApplicationForm")
            //{
            //    ((LeadApplicationFormScreen)_currentScreen).CatchKey(e);
            //}
        }

        private void EmbriantMainWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)) && Keyboard.IsKeyDown(Key.V))
            {
                if (versionWindow == null || !versionWindow.IsLoaded)
                {
                    versionWindow = new VersionWindow();
                    versionWindow.Owner = this;
                    versionWindow.ShowDialog();
                }
                else
                {
                    versionWindow.Activate();
                }
            }
            //else
            //{
            //    Shell shell = new Shell();
            //    shell.Show();
            //}
        }

        #endregion
        
    }
}