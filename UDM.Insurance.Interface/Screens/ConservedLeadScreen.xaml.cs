using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Windows.DataPresenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{

    public partial class ConservedLeadScreen
    {

        private readonly LeadApplicationScreen _LeadApplicationScreen;


        public ConservedLeadScreen()
        {
            InitializeComponent();
            LoadLookupData();
        }



        private void LoadLookupData()
        {
            //#region Load Campaign ComboBox
            //string strQueryAuth;
            //strQueryAuth = "SELECT  C.ID, C.Name  as Description FROM [INCampaign] as [C] left join INCampaignGroupSet as [CGS] on CGS.FKlkpINCampaignGroup = C.FKINCampaignGroupID where [C].[IsActive] = 1 and CGS.FKlkpINCampaignGroupType = 2 order by [C].[Name] Asc";


            //DataTable dtAuth = Methods.GetTableData(strQueryAuth);
            //CampaignsCB.Populate(dtAuth, "Description", "ID");
            //#endregion

            #region Load Batch ComboBox
            string strQueryAuth;
            strQueryAuth = "SELECT * from lkpMonths";


            DataTable dtAuth = Methods.GetTableData(strQueryAuth);
            MonthCB.Populate(dtAuth, "Description", "ID");
            #endregion
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                #region Month selection setup

                string monthNumberSelected = "";
                if(MonthCB.Text.ToString() == "Jan")
                {
                    monthNumberSelected = "01";
                }
                else if(MonthCB.Text.ToString() == "Feb")
                {
                    monthNumberSelected = "02";
                }
                else if(MonthCB.Text.ToString() == "Mar")
                {
                    monthNumberSelected = "03";
                }
                else if (MonthCB.Text.ToString() == "Apr")
                {
                    monthNumberSelected = "04";
                }
                else if (MonthCB.Text.ToString() == "May")
                {
                    monthNumberSelected = "05";
                }
                else if (MonthCB.Text.ToString() == "Jun")
                {
                    monthNumberSelected = "06";
                }
                else if (MonthCB.Text.ToString() == "Jul")
                {
                    monthNumberSelected = "07";
                }
                else if (MonthCB.Text.ToString() == "Aug")
                {
                    monthNumberSelected = "08";
                }
                else if (MonthCB.Text.ToString() == "Sept")
                {
                    monthNumberSelected = "09";
                }
                else if (MonthCB.Text.ToString() == "Oct")
                {
                    monthNumberSelected = "10";
                }
                else if (MonthCB.Text.ToString() == "Nov")
                {
                    monthNumberSelected = "11";
                }
                else if (MonthCB.Text.ToString() == "Dec")
                {
                    monthNumberSelected = "12";
                }
                else
                {
                    monthNumberSelected = "";
                }


                #endregion

                #region Date Range selection
                DateTime? StartDateRange;
                DateTime? EndDateRange;

                try
                {
                    string StartDateString = DateTime.Now.Year.ToString() + "-" + monthNumberSelected + "-" + "01" + " 00:00:00";
                    StartDateRange = DateTime.Parse(StartDateString);

                    string EndDateString = Methods.GetTableData("select EOMONTH('" + StartDateString + "')", IsolationLevel.Snapshot).Rows[0][0].ToString();
                    EndDateRange = DateTime.Parse(EndDateString);
                }
                catch (Exception y)
                {
                    StartDateRange = null;
                    EndDateRange = null;
                }


                #endregion

                #region AuthToken

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
                #endregion


                DataTable dtBatchList = Methods.GetTableData("select B.ID, C.ID as [CampaignID] from INBatch as B left join INCampaign as C on B.FKINCampaignID = C.ID left join INCampaignGroupSet as CGS on C.FKINCampaignGroupID = CGS.FKlkpINCampaignGroup where B.StampDate between '"+ StartDateRange.ToString() + "' and '" + EndDateRange + "' and B.ID not in (select CC.FKINBatchID from ConservedCampaigns as CC) and CGS.FKlkpINCampaignGroupType = 2");
                List <DataRow> BatchList = dtBatchList.AsEnumerable().ToList();

                foreach(var rowBatch in BatchList)
                {
                    string CountLeads = Methods.GetTableData("SELECT COUNT(I.ID) FROM INImport as I WHERE I.FKINCampaignID = " + rowBatch["CampaignID"].ToString() + " and I.FKINBatchID = " + rowBatch["ID"].ToString(), IsolationLevel.Snapshot).Rows[0][0].ToString();


                    try
                    {
                        string CheckIfBatchWasSaved = Methods.GetTableData("SELECT C.ID FROM ConservedCampaigns as C WHERE C.FKINCampaignID = " + rowBatch["CampaignID"].ToString() + " and C.FKINBatchID = " + rowBatch["ID"].ToString(), IsolationLevel.Snapshot).Rows[0][0].ToString();
                        ConservedCampaigns CC = new ConservedCampaigns(long.Parse(CheckIfBatchWasSaved));
                        CC.FKINCampaignID = long.Parse(rowBatch["CampaignID"].ToString());
                        CC.FKINBatchID = long.Parse(rowBatch["ID"].ToString());
                        CC.AmountOfLeads = int.Parse(CountLeads);
                        CC.Save(_validationResult);
                    }
                    catch
                    {
                        ConservedCampaigns CC = new ConservedCampaigns();
                        CC.FKINCampaignID = long.Parse(rowBatch["CampaignID"].ToString());
                        CC.FKINBatchID = long.Parse(rowBatch["ID"].ToString());
                        CC.AmountOfLeads = int.Parse(CountLeads);
                        CC.Save(_validationResult);
                    }

                    //string CountLeads = Methods.GetTableData("SELECT COUNT(I.ID) FROM INImport as I WHERE I.FKINCampaignID = " + CampaignsCB.SelectedValue + " and I.FKINBatchID = " + rowBatch["ID"].ToString(), IsolationLevel.Snapshot).Rows[0][0].ToString();
                    DataTable dtReferenceList = Methods.GetTableData("SELECT I.RefNo FROM INImport as I where I.FKINCampaignID = " + rowBatch["CampaignID"].ToString() + "and I.FKINBatchID = " + rowBatch["ID"].ToString());
                    List<DataRow> ReferenceList = dtReferenceList.AsEnumerable().ToList();

                    int x = 0;



                    foreach (var row in ReferenceList)
                    {
                        try
                        {


                            string ValidityStatus = "";
                            string submitRequest_urlLeadValidity = "https://plhqweb.platinumlife.co.za:998/api/UG/LeadValidity";
                            string ReferenceNumber = row["RefNo"].ToString();

                            using (var wb2 = new WebClient())
                            {
                                var data = new NameValueCollection();
                                data["ReferenceNumber"] = ReferenceNumber;
                                wb2.Headers.Add("Authorization", "Bearer " + token);

                                var response = wb2.UploadValues(submitRequest_urlLeadValidity, "POST", data);
                                string responseInString = Encoding.UTF8.GetString(response);
                                var customObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseInString);

                                ValidityStatus = (string)customObject["ValidityStatus"];
                                //if (LaData.AppData.RefNo == "gdna91003681533")
                                //{
                                //    ValidityStatus = "inValid";
                                //}

                            }

                            if (ValidityStatus != "Valid")
                            {
                                try
                                {
                                    string GetImportID = Methods.GetTableData("SELECT C.ID FROM INImport as C WHERE C.FKINCampaignID = " + rowBatch["CampaignID"].ToString() + " and C.FKINBatchID = " + rowBatch["ID"].ToString() + " and C.RefNo = '" + row["RefNo"].ToString() + "'", IsolationLevel.Snapshot).Rows[0][0].ToString();
                                    string CheckIfBatchWasSaved = Methods.GetTableData("SELECT C.ID FROM ConservedLeadImportTracker as C WHERE C.FKINImportID = " + GetImportID + " and C.FKINBatchID = " + rowBatch["ID"].ToString(), IsolationLevel.Snapshot).Rows[0][0].ToString();
                                    //LeadImportTracker LIT = new LeadImportTracker(long.Parse(CheckIfBatchWasSaved));
                                    //LIT.FKINImportID = long.Parse(GetImportID);
                                    //LIT.FKINBatchID = long.Parse(BatchCB.SelectedValue.ToString());
                                    //LIT.Save(_validationResult);
                                }
                                catch
                                {
                                    string GetImportID = Methods.GetTableData("SELECT C.ID FROM INImport as C WHERE C.FKINCampaignID = " + rowBatch["CampaignID"].ToString() + " and C.FKINBatchID = " + rowBatch["ID"].ToString() + " and C.RefNo = '" + row["RefNo"].ToString() + "'", IsolationLevel.Snapshot).Rows[0][0].ToString();
                                    LeadImportTracker LIT = new LeadImportTracker();
                                    LIT.FKINBatchID = long.Parse(rowBatch["ID"].ToString());
                                    LIT.FKINImportID = long.Parse(GetImportID.ToString());
                                    LIT.ConservedStatus = ValidityStatus;
                                    LIT.Save(_validationResult);
                                }
                            }
                        }
                        catch(Exception w)
                        {

                        }
                        x = x + 1;
                        //Dispatcher.BeginInvoke(DispatcherPriority.DataBind, (Action)(() => {
                        //Dispatcher.BeginInvoke(new Action(delegate
                        //{
                        //    AmountTotalsTB.Text = x.ToString();
                        //}), DispatcherPriority.ContextIdle, null);
                        //}));'


                    }
                    AmountOfLeadsTB.Text = CountLeads;
                }


            }
            catch
            {

            }

            try
            {
                //string strQueryConservedData;
                //strQueryConservedData = "SELECT  COUNT(ID) FROM [ConservedLeadImportTracker] as [CL] where [CL].FKINBatchID = " + BatchCB.SelectedValue;
                //AmountConservedTB.Text = Methods.GetTableData(strQueryConservedData, IsolationLevel.Snapshot).Rows[0][0].ToString();
            }
            catch
            {

            }

            INMessageBoxWindow1 mb = new INMessageBoxWindow1();
            ShowMessageBox(mb, "", "Done", ShowMessageType.Information);
        }

        private void CampaignsCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

            }
            catch
            {

            }

        }

        private void BatchCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string strQueryCampaignData;
                strQueryCampaignData = "SELECT  COUNT(ID) FROM [INImport] as [I] where [I].[FKINCampaignID] = " + CampaignsCB.SelectedValue + " and I.FKINBatchID = " + MonthCB.SelectedValue ;
                AmountOfLeadsTB.Text =  Methods.GetTableData(strQueryCampaignData, IsolationLevel.Snapshot).Rows[0][0].ToString();

                string strQueryConservedData;
                strQueryConservedData = "SELECT  COUNT(ID) FROM [ConservedLeadImportTracker] as [CL] where [CL].FKINBatchID = " + MonthCB.SelectedValue;
                AmountConservedTB.Text = Methods.GetTableData(strQueryConservedData, IsolationLevel.Snapshot).Rows[0][0].ToString();
            }
            catch
            {
                //AmountOfLeadsTB.Text = "";
                AmountConservedTB.Text = "0";
            }
        }
    }

}
