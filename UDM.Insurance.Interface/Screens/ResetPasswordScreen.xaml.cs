using System.Windows;
using System.Windows.Input;
using Embriant.Framework.Configuration;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;
using Embriant.Framework.Library;
using Embriant.Framework;
using Embriant.Framework.Data;
using System.Data;

namespace UDM.Insurance.Interface.Screens
{

	public partial class ResetPasswordScreen
	{
	    readonly User _user;

        public ResetPasswordScreen(User user)
        {
            _user = user;
            InitializeComponent();
            Keyboard.Focus(passwordboxNewPassword);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(false);
        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            if (_user != null && passwordboxNewPassword.Password == passwordboxConfirmPassword.Password && passwordboxNewPassword.Password.Length > 6)
            {
                string _encrypt = SecurityLib.EncryptString(passwordboxNewPassword.Password);
                _user.Password = _encrypt;
                _user.RequiresPasswordChange = false;
                _user.Save(_validationResult);
                object userID = Database.GetParameter("@UserID", _user.ID);
                object password = Database.GetParameter("@Password", _encrypt);
                object[] paramArray = new[] { userID, password };
                Database.ExecuteDataSet(null, CommandType.StoredProcedure, "SpUpdateSystemsPasswords", paramArray, 600);
                
                GlobalSettings.ApplicationUser = _user;
                OnDialogClose(true);
            }
            else if (passwordboxNewPassword.Password != passwordboxConfirmPassword.Password)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Passwords do not match", "Password change failed", ShowMessageType.Error);
            }
            else if (passwordboxNewPassword.Password.Length <= 6)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "The Password should be longer than six characters.", "Password to short", ShowMessageType.Error);
            }
            else
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Error", "Password change failed", ShowMessageType.Error);
                OnDialogClose(false);
            }
        }

        private void passwordboxNewPassword_Loaded(object sender, RoutedEventArgs e)
        {
            passwordboxNewPassword.Focus();
            Keyboard.Focus(passwordboxNewPassword);
        }
	}
}

