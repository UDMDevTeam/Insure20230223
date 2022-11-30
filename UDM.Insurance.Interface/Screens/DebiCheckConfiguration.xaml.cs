using System.Data.SqlClient;
using System.Windows.Resources;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using Orientation = Infragistics.Documents.Excel.Orientation;
using System.Linq;
using System.Collections.Generic;
using System.Transactions;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using UDM.Insurance.Business;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportDiaryScreen.xaml
    /// </summary>
    public partial class DebiCheckConfigurationPage
    {

        #region Constants


        #endregion Constants

        #region Private Members



        #endregion Private Members

        #region Constructors

        public DebiCheckConfigurationPage()
        {
            InitializeComponent();

            try
            {
                int DebiCheckConfigBool;

                StringBuilder strQueryDebiCheckConfig = new StringBuilder();
                strQueryDebiCheckConfig.Append("SELECT TOP 1 DebiCheckPower [Response] ");
                strQueryDebiCheckConfig.Append("FROM DebiCheckConfiguration ");
                DataTable dtDebiCheckConfig = Methods.GetTableData(strQueryDebiCheckConfig.ToString());

                DebiCheckConfigBool = int.Parse(dtDebiCheckConfig.Rows[0]["Response"].ToString());

                if (DebiCheckConfigBool == 1)
                {
                    DebiCheckToggleBtn.IsChecked = true;
                }
                else
                {
                    DebiCheckToggleBtn.IsChecked = false;
                }
            }
            catch(Exception w)
            {

            }

        }

        #endregion Constructors

        #region Private Methods
        private bool CheckAPIConnection()
        {
            try
            {
                string auth_url = "https://plhqweb.platinumlife.co.za:998/Token";
                string Username = "udm@platinumlife.co.za";
                string Password = "P@ssword1";
                string token = "";

                using (var wb = new WebClient())
                {
                    var data = new NameValueCollection();
                    data["username"] = Username;
                    data["password"] = Password;
                    data["grant_type"] = "password";

                    var response = wb.UploadValues(auth_url, "POST", data);
                    string responseInString = Encoding.UTF8.GetString(response);
                    var customObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseInString);
                    token = (string)customObject["access_token"];
                }

                if(token != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        private bool CheckBankingAPIConnection()
        {
            try
            {
                string auth_url = "https://plhqweb.platinumlife.co.za:998/Token";
                string Username = "udm@platinumlife.co.za";
                string Password = "P@ssword1";
                string token = "";

                using (var wb = new WebClient())
                {
                    var data = new NameValueCollection();
                    data["username"] = Username;
                    data["password"] = Password;
                    data["grant_type"] = "password";

                    var response = wb.UploadValues(auth_url, "POST", data);
                    string responseInString = Encoding.UTF8.GetString(response);
                    var customObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseInString);
                    token = (string)customObject["access_token"];
                }

                if (token != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }
        #endregion Private Methods

        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void DebiCheckToggleBtn_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DebiCheckToggleBtn.IsChecked == true)
                {
                    DebiCheckConfiguration dcc = new DebiCheckConfiguration(1);
                    dcc.DebiCheckPower = 1;
                    dcc.Save(_validationResult);
                }
            }
            catch(Exception q)
            {

            }

            
        }

        private void DebiCheckToggleBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DebiCheckToggleBtn.IsChecked == false)
                {
                    DebiCheckConfiguration dcc = new DebiCheckConfiguration(1);
                    dcc.DebiCheckPower = 0;
                    dcc.Save(_validationResult);
                }
            }
            catch(Exception r)
            {

            }
        }



        private void APIConnectionBtn_Click(object sender, RoutedEventArgs e)
        {
            bool APIConnectionTest = CheckAPIConnection();

            if (APIConnectionTest == true)
            {
                APIConnectionLbl.Background = System.Windows.Media.Brushes.Green;
            }
            else
            {
                APIConnectionLbl.Background = System.Windows.Media.Brushes.Red;
            }
        }




        #endregion

        private void PLServerConnectionBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                DataSet dsDiaryReportData;

                var transactionOptions = new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                };

                using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    dsDiaryReportData = Business.Insure.INGetPLServerConnectionTest("");
                }

                PLServerConnectionLbl.Background = System.Windows.Media.Brushes.Green;
            }
            catch(Exception y)
            {
                PLServerConnectionLbl.Background = System.Windows.Media.Brushes.Red;
            }
        }

        private void BankingAPIConnectionBtn_Click(object sender, RoutedEventArgs e)
        {
            bool APIConnectionTest = CheckBankingAPIConnection();

            if (APIConnectionTest == true)
            {
                BankingConnectionLbl.Background = System.Windows.Media.Brushes.Green;
            }
            else
            {
                BankingConnectionLbl.Background = System.Windows.Media.Brushes.Red;
            }
            
        }
    }
}
