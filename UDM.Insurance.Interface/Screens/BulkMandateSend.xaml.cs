using Embriant.Framework;
using Embriant.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class BulkMandateSend
    {

        #region Constants

        DataSet dsDebiCheckAcceptedReportData;

        DataSet dsBulkNoesponseData;

        #endregion Constants

        #region Private Members
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;
        private DateTime _startDate;
        private DateTime _endDate;
        #endregion

        public BulkMandateSend()
        {
            InitializeComponent();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }
        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;
            btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnReport.ToolTip = btnReport.Content;
        }

        private bool IsAllInputParametersSpecifiedAndValid()
        {

            #region Ensuring that the From Date was specified

            if (calStartDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'From Date'.", @"No 'From Date' specified", ShowMessageType.Error);
                return false;
            }
            else
            {
                _startDate = calStartDate.SelectedDate.Value;
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the From Date was specified

            if (calEndDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'To Date'.", @"No 'To Date' specified", ShowMessageType.Error);
                return false;
            }

            else
            {
                _endDate = calEndDate.SelectedDate.Value;
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the date range is valid

            if (calStartDate.SelectedDate > calEndDate.SelectedDate.Value)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Invalid date range specified: The 'From Date' can not be greater than the 'To Date'.", "Invalid date range", ShowMessageType.Error);
                return false;
            }

            #endregion Ensuring that the date range is valid

            // Otherwise, if all is well, proceed:
            return true;
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<long> BulkImportIDList = new List<long>();
                string token = "";

                foreach (DataRow row in dsBulkNoesponseData.Tables[0].Rows)
                {
                    BulkImportIDList.Add(long.Parse(row.ItemArray.GetValue(0).ToString()));
                }

                int x = 0;

                foreach(var item in BulkImportIDList)
                {
                    #region Get Token
                    try
                    {


                        string auth_url = "https://plhqweb.platinumlife.co.za:8082/Token";
                        string Username = "udm@platinumlife.co.za";
                        string Password = "P@ssword1";


                        using (var wb = new MyWebClient(180000))
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
                    }
                    catch
                    {

                    }

                    #endregion

                    #region GetData
                    string responsesPolicyID ;
                    string responsesLeadID ;
                    string responsesReferenceNumber ;
                    string responsesCampaignID ;
                    string responsesDateOfSale;

                    bool ifResales = false;
                    try
                    {
                        StringBuilder strQueryPolicyID = new StringBuilder();
                        strQueryPolicyID.Append("SELECT TOP 1 FKINPolicyID [Response], FKINLeadID [LeadID], RefNo [ReferenceNumber],  FKINCampaignID [CampaignID], DateOfSale [DateOfSale]");
                        strQueryPolicyID.Append("FROM INImport ");
                        strQueryPolicyID.Append("WHERE ID = " + item);
                        DataTable dtPolicyID = Methods.GetTableData(strQueryPolicyID.ToString());

                         responsesPolicyID = dtPolicyID.Rows[0]["Response"].ToString();
                         responsesLeadID = dtPolicyID.Rows[0]["LeadID"].ToString();
                         responsesReferenceNumber = dtPolicyID.Rows[0]["ReferenceNumber"].ToString();
                         responsesCampaignID = dtPolicyID.Rows[0]["CampaignID"].ToString();
                        responsesDateOfSale = dtPolicyID.Rows[0]["DateOfSale"].ToString();

                        if (int.Parse(responsesCampaignID) == 7
                            || int.Parse(responsesCampaignID) == 9
                            || int.Parse(responsesCampaignID) == 10
                            || int.Parse(responsesCampaignID) == 294
                            || int.Parse(responsesCampaignID) == 295
                            || int.Parse(responsesCampaignID) == 24
                            || int.Parse(responsesCampaignID) == 25
                            || int.Parse(responsesCampaignID) == 11
                            || int.Parse(responsesCampaignID) == 12
                            || int.Parse(responsesCampaignID) == 13
                            || int.Parse(responsesCampaignID) == 14
                            || int.Parse(responsesCampaignID) == 85
                            || int.Parse(responsesCampaignID) == 86
                            || int.Parse(responsesCampaignID) == 87
                            || int.Parse(responsesCampaignID) == 281
                            || int.Parse(responsesCampaignID) == 324
                            || int.Parse(responsesCampaignID) == 325
                            || int.Parse(responsesCampaignID) == 326
                            || int.Parse(responsesCampaignID) == 327
                            || int.Parse(responsesCampaignID) == 4)
                        {
                            ifResales = true;
                        }
                        else
                        {
                            ifResales = false;
                        }
                    }
                    catch(Exception q)
                    {
                        responsesPolicyID = "";
                        responsesLeadID = "";
                        responsesReferenceNumber = "";
                        responsesCampaignID = "";
                        responsesDateOfSale = "";
                    }


                    
                    //-------------------------------------
                    string responsesBankDetailsID;
                    string responsesTotalPremium;
                    string responsePlatinumPolicyID;
                    try
                    {
                        StringBuilder strQueryBankDetailsID = new StringBuilder();
                        strQueryBankDetailsID.Append("SELECT TOP 1 FKINBankDetailsID [Response], TotalPremium [TotalPremium], PolicyID [PolicyID]");
                        strQueryBankDetailsID.Append("FROM INPolicy ");
                        strQueryBankDetailsID.Append("WHERE ID = " + responsesPolicyID);
                        DataTable dtBankDetailsID = Methods.GetTableData(strQueryBankDetailsID.ToString());

                         responsesBankDetailsID = dtBankDetailsID.Rows[0]["Response"].ToString();
                         responsesTotalPremium = dtBankDetailsID.Rows[0]["TotalPremium"].ToString();
                        responsePlatinumPolicyID = dtBankDetailsID.Rows[0]["PolicyID"].ToString();

                    }
                    catch (Exception w)
                    {
                        responsesBankDetailsID = "";
                        responsesTotalPremium = "";
                        responsePlatinumPolicyID = "";
                    }


                    //----------------------------------------
                    string responsesAccountHolder;
                    string responsesTellCell;
                    string responsesBranchID;
                    string responsesAccountNumber;
                    string responsesAccountTypeID;
                    string responsesDebitDay;
                    string responsesBankDetailsIDNumber ;


                    try
                    {
                        StringBuilder strQueryBankDetailsData = new StringBuilder();
                        strQueryBankDetailsData.Append("SELECT TOP 1 AccountHolder [Response], AHTelCell [TellCell], FKBankBranchID [BranchID], AccountNo [AccountNumber],  FKAccountTypeID [AccountTypeID], DebitDay [DebitDay], AHIDNo [IDNumber] ");
                        strQueryBankDetailsData.Append("FROM INBankDetails ");
                        strQueryBankDetailsData.Append("WHERE ID = " + responsesBankDetailsID);
                        DataTable dtBankDetailsData = Methods.GetTableData(strQueryBankDetailsData.ToString());

                        responsesAccountHolder = dtBankDetailsData.Rows[0]["Response"].ToString();
                        responsesTellCell = dtBankDetailsData.Rows[0]["TellCell"].ToString();
                        responsesBranchID = dtBankDetailsData.Rows[0]["BranchID"].ToString();
                        responsesAccountNumber = dtBankDetailsData.Rows[0]["AccountNumber"].ToString();
                        responsesAccountTypeID = dtBankDetailsData.Rows[0]["AccountTypeID"].ToString();
                        responsesDebitDay = dtBankDetailsData.Rows[0]["DebitDay"].ToString();
                        responsesBankDetailsIDNumber = dtBankDetailsData.Rows[0]["IDNumber"].ToString();

                    }
                    catch (Exception r)
                    {
                        responsesAccountHolder = "";
                        responsesTellCell = "";
                        responsesBranchID = "";
                        responsesAccountNumber = "";
                        responsesAccountTypeID = "";
                        responsesDebitDay = "";
                        responsesBankDetailsIDNumber = "";
                    }


                    //-------------------------------------------
                    string responsesEmail;
                    string responsesLeadFirstName ;
                    string responsesLeadSurname;
                    string responsesLeadTel;
                    string responsesLeadCel;
                    string responsesLeadWork;
                    string responsesLeadPassportNumber;



                    try
                    {
                        StringBuilder strQueryLeadData = new StringBuilder();
                        strQueryLeadData.Append("SELECT TOP 1 Email [Email], FirstName [FirstName], Surname [Surname], TelWork [TelWork], TelHome [TelHome], TelCell [TelCell], PassportNo [PassportNumber] ");
                        strQueryLeadData.Append("FROM INLead ");
                        strQueryLeadData.Append("WHERE ID = " + responsesLeadID);
                        DataTable dtLeadData = Methods.GetTableData(strQueryLeadData.ToString());

                        responsesEmail = dtLeadData.Rows[0]["Email"].ToString();
                        responsesLeadFirstName = dtLeadData.Rows[0]["FirstName"].ToString();
                        responsesLeadSurname = dtLeadData.Rows[0]["Surname"].ToString();
                        responsesLeadTel = dtLeadData.Rows[0]["TelHome"].ToString();
                        responsesLeadCel = dtLeadData.Rows[0]["TelCell"].ToString();
                        responsesLeadWork = dtLeadData.Rows[0]["TelWork"].ToString();
                        responsesLeadPassportNumber = dtLeadData.Rows[0]["PassportNumber"].ToString();

                    }
                    catch (Exception t)
                    {
                        responsesEmail = "";
                        responsesLeadFirstName = "";
                        responsesLeadSurname = "";
                        responsesLeadTel = "";
                        responsesLeadCel = "";
                        responsesLeadWork = "";
                        responsesLeadPassportNumber = "";
                    }

                    //--------------------------------------
                    string responsesBankBranchCode;
                    if(ifResales == true)
                    {
                        responsesBankBranchCode = "";
                    }
                    else
                    {
                        try
                        {
                            StringBuilder strQueryBranchCode = new StringBuilder();
                            strQueryBranchCode.Append("SELECT TOP 1 Code [Code] ");
                            strQueryBranchCode.Append("FROM BankBranch ");
                            strQueryBranchCode.Append("WHERE ID = " + responsesBranchID);
                            DataTable dtBranchCode = Methods.GetTableData(strQueryBranchCode.ToString());

                            responsesBankBranchCode = dtBranchCode.Rows[0]["Code"].ToString();
                        }
                        catch (Exception i)
                        {
                            responsesBankBranchCode = "";
                        }
                    }


                    //---------------------------------------------
                    string responsesCampaignCode;

                    try
                    {
                        StringBuilder strQueryCampaignCode = new StringBuilder();
                        strQueryCampaignCode.Append("SELECT TOP 1 Code [Code] ");
                        strQueryCampaignCode.Append("FROM INCampaign ");
                        strQueryCampaignCode.Append("WHERE ID = " + responsesCampaignID);
                        DataTable dtCampaignCode = Methods.GetTableData(strQueryCampaignCode.ToString());

                        responsesCampaignCode = dtCampaignCode.Rows[0]["Code"].ToString();
                    }
                    catch(Exception y)
                    {
                        responsesCampaignCode = "";
                    }

                    //-----------------------------------------
                    string responsesAccountType;
                    if(ifResales == true)
                    {
                        responsesAccountType = "";
                    }
                    else
                    {
                        try
                        {
                            StringBuilder strQueryAccountype = new StringBuilder();
                            strQueryAccountype.Append("SELECT TOP 1 Description [Response] ");
                            strQueryAccountype.Append("FROM lkpAccountType ");
                            strQueryAccountype.Append("WHERE ID = " + responsesAccountTypeID);
                            strQueryAccountype.Append(" ORDER BY ID DESC");
                            DataTable dtAccountType = Methods.GetTableData(strQueryAccountype.ToString());

                            responsesAccountType = dtAccountType.Rows[0]["Response"].ToString();
                        }
                        catch (Exception u)
                        {
                            responsesAccountType = "";
                        }
                    }


                    string AccountTypeNumber = "";
                    if (responsesAccountType == "Current")
                    {
                        AccountTypeNumber = "1";
                    }
                    else if (responsesAccountType == "Savings")
                    {
                        AccountTypeNumber = "2";
                    }
                    else if (responsesAccountType == "Transmission")
                    {
                        AccountTypeNumber = "3";
                    }
                    #endregion

                    string submitMandate_url = "https://plhqweb.platinumlife.co.za:8082/api/Mandate/submitMandateRequest";
                    using (var wb = new MyWebClient(180000))
                    {
                        var data = new NameValueCollection();


                        if (int.Parse(responsesCampaignID) == 7
                            || int.Parse(responsesCampaignID) == 9
                            || int.Parse(responsesCampaignID) == 10
                            || int.Parse(responsesCampaignID) == 294
                            || int.Parse(responsesCampaignID) == 295
                            || int.Parse(responsesCampaignID) == 24
                            || int.Parse(responsesCampaignID) == 25
                            || int.Parse(responsesCampaignID) == 11
                            || int.Parse(responsesCampaignID) == 12
                            || int.Parse(responsesCampaignID) == 13
                            || int.Parse(responsesCampaignID) == 14
                            || int.Parse(responsesCampaignID) == 85
                            || int.Parse(responsesCampaignID) == 86
                            || int.Parse(responsesCampaignID) == 87
                            || int.Parse(responsesCampaignID) == 281
                            || int.Parse(responsesCampaignID) == 324
                            || int.Parse(responsesCampaignID) == 325
                            || int.Parse(responsesCampaignID) == 326
                            || int.Parse(responsesCampaignID) == 327
                            || int.Parse(responsesCampaignID) == 4)
                        {
                            #region Reset PL Lookup Variables
                            string PolicyNumberPLLKP = "";
                            string ReferenceNumberPLLKP = "";
                            string ResponseDetailPLLKP = "";
                            string BankPLLKP = "";
                            string AccountNumberPLLKP = "";
                            string AccountHolderPLLKP = "";
                            string BranchCodePLLKP = "";
                            string AccountTypePLLKP = "";
                            string DebitDayPLLKP = "";
                            string IDNumberPLLKP = "";
                            string PassportNumberPLLKP = "";
                            #endregion
                            try
                            {


                                string auth_url = "https://plhqweb.platinumlife.co.za:998/Token";
                                string Username = "udm@platinumlife.co.za";
                                string Password = "P@ssword1";
                                string token2 = "";



                                #region SumbitClientDetailRequest
                                string submitRequest_urlID = "https://plhqweb.platinumlife.co.za:998/api/CD/PL_Request";
                                using (var wb2 = new WebClient())
                                {
                                    var data2 = new NameValueCollection();
                                    data2["Organization"] = "PL"; //THG //TBF //IG //IPS
                                    data2["PolicyNumber"] = responsePlatinumPolicyID;
                                    data2["ReferenceNumber"] = responsesReferenceNumber;
                                    data2["PassportNumber"] = "";
                                    data2["FirstName"] = responsesLeadFirstName;
                                    data2["LastName"] = responsesLeadSurname;
                                    data2["MobileNumber"] = responsesLeadCel;
                                    data2["HomeNumber"] = responsesLeadTel;
                                    data2["WorkNumber"] = responsesLeadWork;
                                    data2["EmailAddress"] = responsesEmail;

                                    wb2.Headers.Add("Authorization", "Bearer " + token);

                                    var response2 = wb2.UploadValues(submitRequest_urlID, "POST", data2);
                                    string responseInString2 = Encoding.UTF8.GetString(response2);
                                    var customObject2 = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseInString2);


                                    string ReferenceNumber = (string)customObject2["ReferenceNumber"];
                                    string ResponseDetail = (string)customObject2["ResponseDetail"]; // WILL RETURN "OK" OR "No Data Found"
                                    string IDNumber = (string)customObject2["IDNumber"];
                                    string PassportNumber = (string)customObject2["PassportNumber"];
                                    IDNumberPLLKP = IDNumber;
                                    PassportNumberPLLKP = PassportNumber;

                                }
                                #endregion


                                #region SumbitBankingDetailRequest
                                string submitRequest_url = "https://plhqweb.platinumlife.co.za:998/api/BD/PL_Request";
                                using (var wb3 = new WebClient())
                                {
                                    var data3 = new NameValueCollection();
                                    data3["Organization"] = "PL"; //THG //TBF //IG //IPS
                                    data3["PolicyNumber"] = responsePlatinumPolicyID;
                                    data3["IDNumber"] = IDNumberPLLKP;
                                    data3["ReferenceNumber"] = responsesReferenceNumber;
                                    data3["PassportNumber"] = PassportNumberPLLKP;
                                    data3["FirstName"] = responsesLeadFirstName;
                                    data3["LastName"] = responsesLeadSurname;
                                    data3["MobileNumber"] = responsesLeadCel;
                                    data3["HomeNumber"] = responsesLeadTel;
                                    data3["WorkNumber"] = responsesLeadWork;
                                    data3["EmailAddress"] = responsesEmail;

                                    wb3.Headers.Add("Authorization", "Bearer " + token);

                                    var response3 = wb3.UploadValues(submitRequest_url, "POST", data3);
                                    string responseInString3 = Encoding.UTF8.GetString(response3);
                                    var customObject3 = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseInString3);

                                    PolicyNumberPLLKP = (string)customObject3["PolicyNumber"];
                                    ReferenceNumberPLLKP = (string)customObject3["ReferenceNumber"];
                                    ResponseDetailPLLKP = (string)customObject3["ResponseDetail"]; // WILL RETURN "OK" OR "No Data Found"
                                    BankPLLKP = (string)customObject3["Bank"];
                                    AccountNumberPLLKP = (string)customObject3["AccountNumber"];
                                    AccountHolderPLLKP = (string)customObject3["AccountHolder"];
                                    BranchCodePLLKP = (string)customObject3["BranchCode"];
                                    AccountTypePLLKP = (string)customObject3["AccountType"];
                                    DebitDayPLLKP = (string)customObject3["DebitDay"];
                                    string policyHolderBool = (string)customObject3["PolicyOwner"];

                                    if (AccountTypePLLKP == "Current")
                                    {
                                        AccountTypeNumber = "1";
                                    }
                                    else if (AccountTypePLLKP == "Savings")
                                    {
                                        AccountTypeNumber = "2";
                                    }
                                    else if (AccountTypePLLKP == "Transmission")
                                    {
                                        AccountTypeNumber = "3";
                                    }

                                }
                                #endregion
                            }
                            catch (Exception r)
                            {

                            }
                            string IDNumberDebiCheck = "1";
                            if(PassportNumberPLLKP == "")
                            {
                                IDNumberDebiCheck = "1";
                            }
                            else
                            {
                                IDNumberDebiCheck = "2";
                            }

                            DateTime CommencementDateEdited;
                            DateTime CommencementDateDebiCheck;
                            DateTime datevariable = DateTime.Parse(responsesDateOfSale);
                            CommencementDateDebiCheck = datevariable.AddMonths(2);
                            CommencementDateEdited = new DateTime(CommencementDateDebiCheck.Year, CommencementDateDebiCheck.Month, int.Parse(responsesDebitDay));



                            try { data["ClientName"] = responsesAccountHolder; } catch { data["ClientName"] = ""; }
                            try { data["CellNumber"] = responsesTellCell; } catch { data["CellNumber"] = ""; }
                            try { data["Email"] = responsesEmail; } catch { data["Email"] = ""; }
                            try { data["DocumentTypeID"] = IDNumberDebiCheck; } catch { data["DocumentTypeID"] = ""; }
                            if (IDNumberDebiCheck == "2")
                            {
                                try { data["IdentificationNumber"] = PassportNumberPLLKP; } catch { data["IdentificationNumber"] = ""; }
                            }
                            else if (IDNumberDebiCheck == "1")
                            {
                                try { data["IdentificationNumber"] = IDNumberPLLKP; } catch { data["IdentificationNumber"] = ""; }
                            }
                            try { data["ReferenceNumber"] = responsesReferenceNumber; } catch { data["ReferenceNumber"] = ""; }
                            try { data["BranchCode"] = BranchCodePLLKP; } catch { data["BranchCode"] = ""; }
                            try { data["AccountNumber"] = AccountNumberPLLKP; } catch { data["AccountNumber"] = ""; }
                            try { data["InstallmentAmount"] = responsesTotalPremium; } catch { data["InstallmentAmount"] = ""; }
                            try { data["MaxInstallmentAmount"] = (decimal.Parse(responsesTotalPremium) * 12).ToString(); } catch { data["MaxInstallmentAmount"] = ""; }

                            try { data["FirstCollectionDate"] = CommencementDateEdited.ToString(); } catch { data["FirstCollectionDate"] = ""; }
                            try { data["AccountTypeID"] = AccountTypeNumber; } catch { data["AccountTypeID"] = "1"; }
                            try { data["CustomField1"] = responsesCampaignCode; } catch { data["CustomField1"] = " "; }
                            try { data["CustomField2"] = "b"; } catch { data["CustomField2"] = " "; }

                        }
                        else
                        {
                            string IDNumberDebiCheck = "1";
                            if (responsesLeadPassportNumber == "")
                            {
                                IDNumberDebiCheck = "1";
                            }
                            else
                            {
                                IDNumberDebiCheck = "2";
                            }

                            DateTime CommencementDateEdited;
                            DateTime CommencementDateDebiCheck;
                            DateTime datevariable = DateTime.Parse(responsesDateOfSale);
                            CommencementDateDebiCheck = datevariable.AddMonths(2);
                            CommencementDateEdited = new DateTime(CommencementDateDebiCheck.Year, CommencementDateDebiCheck.Month, int.Parse(responsesDebitDay));


                            try { data["ClientName"] = responsesAccountHolder; } catch { data["ClientName"] = ""; }
                            try { data["CellNumber"] = responsesTellCell; } catch { data["CellNumber"] = ""; }
                            try { data["Email"] = responsesEmail; } catch { data["Email"] = ""; }
                            try { data["DocumentTypeID"] = IDNumberDebiCheck; } catch { data["DocumentTypeID"] = ""; }
                            if (IDNumberDebiCheck == "2")
                            {
                                try { data["IdentificationNumber"] = responsesLeadPassportNumber; } catch { data["IdentificationNumber"] = ""; }
                            }
                            else if (IDNumberDebiCheck == "1")
                            {
                                try { data["IdentificationNumber"] = responsesBankDetailsIDNumber; } catch { data["IdentificationNumber"] = ""; }
                            }
                            try { data["ReferenceNumber"] = responsesReferenceNumber; } catch { data["ReferenceNumber"] = ""; }
                            try { data["BranchCode"] = responsesBankBranchCode; } catch { data["BranchCode"] = ""; }
                            try { data["AccountNumber"] = responsesAccountNumber; } catch { data["AccountNumber"] = ""; }
                            try { data["InstallmentAmount"] = responsesTotalPremium; } catch { data["InstallmentAmount"] = ""; }
                            try { data["MaxInstallmentAmount"] = (decimal.Parse(responsesTotalPremium) * 12).ToString(); } catch { data["MaxInstallmentAmount"] = ""; }

                            try { data["FirstCollectionDate"] = CommencementDateEdited.ToString(); } catch { data["FirstCollectionDate"] = ""; }
                            try { data["AccountTypeID"] = AccountTypeNumber; } catch { data["AccountTypeID"] = "1"; }
                            try { data["CustomField1"] = responsesCampaignCode; } catch { data["CustomField1"] = " "; }
                            try { data["CustomField2"] = "b"; } catch { data["CustomField2"] = " "; }

                        }


                        wb.Headers.Add("Authorization", "Bearer " + token);

                        var response = wb.UploadValues(submitMandate_url, "POST", data);
                        string responseInString;

                        try
                        {
                            responseInString = Encoding.UTF8.GetString(response);
                        }
                        catch
                        {
                            responseInString = null;
                        }

                        var customObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseInString);
                    }

                    #region Progress Bar
                    x++;


                    double calc = (x / double.Parse(BulkImportIDList.Count().ToString())) * 100;
                    //ProgressBarBulk.Value = calc;

                    ProgressBarBulk.Dispatcher.Invoke(() => ProgressBarBulk.Value = calc, DispatcherPriority.Background);

                    #endregion
                }






                //BackgroundWorker worker = new BackgroundWorker();

                //    worker.DoWork += Report;
                //    worker.RunWorkerCompleted += ReportCompleted;
                //    worker.RunWorkerAsync();

                //    dispatcherTimer1.Start();

            }

            catch (Exception ex)
            {
                //HandleException(ex);

                btnReport.IsEnabled = true;
                btnClose.IsEnabled = true;
                calStartDate.IsEnabled = true;
                calEndDate.IsEnabled = true;
            }
        }





        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void EnableDisableExportButton()
        {
            try
            {
                if ((calStartDate.SelectedDate != null && (calEndDate.SelectedDate != null))) //&& (calEndDate.SelectedDate >= Cal1.SelectedDate)
                {
                    btnReport.IsEnabled = true;
                    return;
                }

                //btnReport.IsEnabled = false;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";

            btnReport.IsEnabled = true;
            btnClose.IsEnabled = true;
            calStartDate.IsEnabled = true;
            calEndDate.IsEnabled = true;
        }

        private void calEndDate_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calEndDate.SelectedDate.ToString(), out _endDate);
            EnableDisableExportButton();
        }

        private void calStartDate_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calStartDate.SelectedDate.ToString(), out _startDate);
            EnableDisableExportButton();
        }

        private void UpgradeCB_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void BaseCB_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btnGenerateData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TimeSpan ts = new TimeSpan(23, 00, 0);
                _endDate = _endDate.Date + ts;


                try { dsBulkNoesponseData.Clear();} catch { }
                try { DataGridBulk.ItemsSource = null; } catch { }

                var transactionOptions = new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                };

                using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    dsBulkNoesponseData = Business.Insure.INGetDebiCheckBulkNoResponses(_startDate, _endDate);
                }


                DataGridBulk.ItemsSource = new DataView(dsBulkNoesponseData.Tables[0]);

                int BulkMandateSendTotal = dsBulkNoesponseData.Tables[0].Rows.Count;
                TotalResponsesLbl.Content = BulkMandateSendTotal.ToString();

                btnReport.Visibility = Visibility.Visible;
                btnGenerateData.Visibility = Visibility.Hidden;
            }
            catch(Exception u)
            {

            }
            finally
            {

            }




        }
    }
}
