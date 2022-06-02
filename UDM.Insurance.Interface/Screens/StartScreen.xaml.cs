using System.Windows.Threading;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Embriant.Framework.Library;
using Embriant.WPF.Controls;
using System;
using System.Windows;
using System.Windows.Input;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.Windows;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{

    public partial class StartScreen
    {

        #region Private Variables

        private User _user;

        #endregion



        #region Constructor

        public StartScreen()
        {
            InitializeComponent();

#if TESTBUILD
                TestControl.Visibility = Visibility.Visible;
#elif DEBUG
            DebugControl.Visibility = Visibility.Visible;
#elif TRAININGBUILD
                TrainingControl.Visibility = Visibility.Visible;
#endif
        }

        #endregion



        #region Methods

        private bool IsActiveUser()
        {
            try
            {
                if (_user != null)
                {
                    if (_user.IsActive != null)
                    {
                        if (Convert.ToBoolean(_user.IsActive))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }

        private bool IsInsuranceUser()
        {
            try
            {
                UserSystem userSystem = UserSystemMapper.SearchOne(_user.ID, (long?)lkpSystem.Insurance, null);

                if (userSystem != null)
                {
                    if (_user.RequiresPasswordChange)
                    {
                        ResetPasswordScreen ResetScreen = new ResetPasswordScreen(_user);

                        ShowDialog(ResetScreen, new INDialogWindow(ResetScreen));

                        if (_user.Password != null) txtPassword.Password = SecurityLib.DecryptString(_user.Password);
                    }


                    return true;
                }

                return false;
            }

            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }

        private bool IsAuthenticUser()
        {
            try
            {
                string decrypt = "";
                if (_user != null && _user.Password != null)
                {
                    decrypt = SecurityLib.DecryptString(_user.Password);
                }

                if (_user != null && txtPassword.Password == decrypt && _user.RequiresPasswordChange != true)
                {

                    txtPassword.Password = null;
                    GlobalSettings.ApplicationUser = _user;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }

        private void GetSystemInfo()
        {
            try
            {
                SystemInfo systemInfo = new SystemInfo();

                systemInfo.FKSystemID = 2; //Insure
                systemInfo.SystemVersion = GlobalSettings.ApplicationVersion?.ToString();
                systemInfo.ComputerName = Environment.MachineName;
                systemInfo.UserName = Environment.UserName;
                systemInfo.UserDomainName = Environment.UserDomainName;

                systemInfo.SimpleOSName = Methods.GetSimpleOSName();

                systemInfo.OSDescription = RuntimeInformation.OSDescription;
                systemInfo.OSArchitecture = RuntimeInformation.OSArchitecture.ToString();
                systemInfo.ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString();
                systemInfo.FrameworkDescription = RuntimeInformation.FrameworkDescription;

                systemInfo.IPAddresses = Methods.GetIPAddresses();

                systemInfo.Save(_validationResult);
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        #endregion

        private void CheckUserVersion(string Username)
        {

            //try
            //{

            //    string LatestVersion;
            //    string Version;

            //    Username = ((User)GlobalSettings.ApplicationUser).LoginName;

            //    DataTable dtVersionData = User.INGetVersionInfo(Username);

            //    DataTable dtLatestVersionData = User.INGetLatestVersion();

            //    LatestVersion = dtLatestVersionData.Rows[0]["Version"].ToString();

            //    Version = dtVersionData.Rows[0]["Version"].ToString();

            //    if (LatestVersion != Version)
            //    {

            //        MessageBox.Show("This version of Insure is outdated. It will update now.", MessageBoxButton.OK.ToString());

            //        //ShowMessageBox(new INMessageBoxWindow1(), "This version of Insure is outdated. It will now update.", "Incorrect Version", ShowMessageType.Error);

            //        //Dispatcher.Invoke(DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate
            //        //{
            //        //    ShowMessageBox(new Windows.INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", Embriant.Framework.ShowMessageType.Information);
            //        //});


            //        //ShowMessageBox(new Windows.INMessageBoxWindow1(), @"This version of Insure is outdated. Please close and re-open the application.", "Incorrect Version", Embriant.Framework.ShowMessageType.Error);
            //        System.Windows.Application.Current.Shutdown();
            //        System.Windows.Forms.Application.Restart();

            //    }
            //    else
            //    {

            //    }
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}


        }


        #region Event Handlers
        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _user = UserMapper.SearchOne(null, null, null, txtUsername.Text, null, null, null, null, null);
                GlobalSettings.ApplicationUser = _user;

                string Username = ((User)GlobalSettings.ApplicationUser).LoginName;

                #region Get Message
                DataSet dsMessages = Methods.ExecuteStoredProcedure("sp_GetWellComeMessages", null);
                DataTable dtMessages = dsMessages.Tables[0];
                Random rnd = new Random();
                int messageId = rnd.Next(0, dsMessages.Tables[0].Rows.Count);
                string message = string.Empty;
                if (dtMessages.Rows[messageId]["TextMessage"] != null && dtMessages.Rows[messageId]["TextMessage"].ToString() != string.Empty && dtMessages.Rows.Count > 0)
                {
                    message = dtMessages.Rows[messageId]["TextMessage"].ToString();
                }
                #endregion


                string UserFirstName = string.Empty;
                bool userLoggedOn = false;
                bool ShowMessage = true;
                bool BirthdayMessageDisplayed = false;

                if (_user != null)
                {
                    {//User Loggin information
                        SqlParameter[] parameters =
                        {
                            new SqlParameter("@userId", _user.ID),
                        };
                        DataSet dsUsers = Methods.ExecuteStoredProcedure("sp_DetermineIfUserLoggedOn", parameters);
                        int count = dsUsers.Tables[0].Rows.Count;

                        parameters = new SqlParameter[2];
                        parameters[0] = new SqlParameter("@StaffID", (object)0);
                        parameters[1] = new SqlParameter("@UserID", _user.ID);
                        var usrFirstName = Methods.ExecuteFunction("GetUserFirstName", parameters, "Server=udmsql; Database=Blush; User ID=apollo; Password=J5TeXhzC6_8RVu9q;", IsolationLevel.ReadUncommitted);
                        if (usrFirstName != null)
                        {
                            UserFirstName = usrFirstName.ToString();
                        }

                        //DataSet dsUserInfo = Methods.ExecuteStoredProcedure("sp_GetUserName", parameters);
                        //if (dsUserInfo.Tables[0].Rows.Count > 0)
                        //{
                        //    UserFirstName = dsUserInfo.Tables[0].Rows[0]["FirstName"].ToString();
                        //}

                        if (count > 0) userLoggedOn = true;
                    }

                    if (IsActiveUser())
                    {
                        if (IsInsuranceUser())
                        {
                            bool authenticated = IsAuthenticUser();
                            if (authenticated)
                            {
                                BaseControl nextControl = null;
                                switch (((User)GlobalSettings.ApplicationUser).FKUserType)
                                {
                                    case (int)lkpUserType.SeniorAdministrator:
                                    case (int)lkpUserType.Administrator:
                                    case (int)lkpUserType.Manager:

                                        //CheckUserVersion(Username);

                                        nextControl = new MenuManagementScreen(ScreenDirection.Forward);
                                        break;
                                    case (int)lkpUserType.SalesAgent:
                                    case (int)lkpUserType.DataCapturer:

                                        //CheckUserVersion(Username);

                                        nextControl = new SalesScreen();
                                        //ShowMessage = false;
                                        break;
                                    case (int)lkpUserType.ConfirmationAgent:
                                    case (int)lkpUserType.StatusLoader:
                                    case (int)lkpUserType.CallMonitoringAgent:
                                    case (int)lkpUserType.Preserver:
                                    case (int)lkpUserType.DebiCheckAgent:
                                        //nextControl = new ConfirmScreen();

                                        //CheckUserVersion(Username);

                                        nextControl = new SalesScreen();
                                        break;

                                    default:
                                        txtPassword.Password = null;
                                        ShowMessageBox(new INMessageBoxWindow1(), "Invalid User Type for Insure System", "Login Failed", ShowMessageType.Error);
                                        txtUsername.Focus();
                                        Keyboard.Focus(txtUsername);
                                        break;
                                }

                                GetSystemInfo();

                                #region Display Welcome or Birthday Message

                                SqlParameter[] parameters = new SqlParameter[1];
                                parameters[0] = new SqlParameter("@Date", DateTime.Today);
                                var birthdayUserIDs = Methods.ExecuteFunction("GetBirthdayUserIDs", parameters, "Server=udmsql; Database=Blush; User ID=apollo; Password=J5TeXhzC6_8RVu9q;", IsolationLevel.ReadUncommitted);
                                if (birthdayUserIDs != null && birthdayUserIDs != DBNull.Value)
                                {
                                    var ids = birthdayUserIDs.ToString().Split(',').Select(long.Parse).AsEnumerable();
                                    if (ids.Contains(_user.ID))
                                    {
                                        ShowMessageBox(new INBirthDayMessage(), string.Empty, UserFirstName, ShowMessageType.Other);

                                        BirthdayMessageDisplayed = true;
                                    }
                                }

                                if (!BirthdayMessageDisplayed && ShowMessage && userLoggedOn)
                                {
                                    ShowMessageBox(new InSureWelcomeMessage(), message, "Hi " + UserFirstName, ShowMessageType.Other);
                                    Methods.ExecuteSQLNonQuery("UPDATE [User] SET LastLoggedIn = GETDATE() Where ID = '" + _user.ID + "'");
                                }

                                #endregion

                                OnClose(nextControl);
                            }
                            else
                            {
                                txtPassword.Password = null;
                                ShowMessageBox(new INMessageBoxWindow1(), "Invalid Password", "Login Failed", ShowMessageType.Error);
                                txtPassword.Focus();
                                Keyboard.Focus(txtPassword);
                            }
                        }
                        else
                        {
                            txtPassword.Password = null;
                            ShowMessageBox(new INMessageBoxWindow1(), "Invalid User for Insure System", "Login Failed", ShowMessageType.Error);
                            txtUsername.Focus();
                            Keyboard.Focus(txtUsername);
                        }
                    }
                    else
                    {
                        txtPassword.Password = null;
                        ShowMessageBox(new INMessageBoxWindow1(), "User is not Active", "Login Failed", ShowMessageType.Error);
                        txtUsername.Focus();
                        Keyboard.Focus(txtUsername);
                    }
                }
                else
                {
                    txtPassword.Password = null;
                    ShowMessageBox(new INMessageBoxWindow1(), "Invalid Username", "Login Failed", ShowMessageType.Error);
                    txtUsername.Focus();
                    Keyboard.Focus(txtUsername);
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }



        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnClose(null);
        }

        private void StartScreen_OnLoaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(500);

            timer.Tick += delegate
            {
                timer.Stop();
                txtUsername.Focus();
            };

            timer.Start();
        }

        #endregion

    }

}