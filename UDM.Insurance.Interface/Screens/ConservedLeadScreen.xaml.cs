using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Windows.DataPresenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
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
            #region Load Campaign ComboBox
            string strQueryAuth;
            strQueryAuth = "SELECT  C.ID, C.Name  as Description FROM [INCampaign] as [C] left join INCampaignGroupSet as [CGS] on CGS.FKlkpINCampaignGroup = C.FKINCampaignGroupID where [C].[IsActive] = 1 and CGS.FKlkpINCampaignGroupType = 2 order by [C].[Name] Asc";


            DataTable dtAuth = Methods.GetTableData(strQueryAuth);
            CampaignsCB.Populate(dtAuth, "Description", "ID");
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
                #region AuthToken

                string auth_url = "http://plhqweb.platinumlife.co.za:999/Token";
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

                string CountLeads = Methods.GetTableData("SELECT COUNT(I.ID) FROM INImport as I WHERE I.FKINCampaignID = " + CampaignsCB.SelectedValue + " and I.FKINBatchID = " + BatchCB.SelectedValue, IsolationLevel.Snapshot).Rows[0][0].ToString();

                DataTable dtReferenceList = Methods.GetTableData("SELECT I.RefNo FROM INImport as I where I.FKINCampaignID = " + CampaignsCB.SelectedValue + "and I.FKINBatchID = " + BatchCB.SelectedValue);
                List<DataRow> ReferenceList = dtReferenceList.AsEnumerable().ToList();

                //List<long> CMAgentListLong = new List<long>();
                //try { CMAgentListLong.Clear(); } catch { }

                try
                {
                    string CheckIfBatchWasSaved = Methods.GetTableData("SELECT C.ID FROM ConservedCampaigns as C WHERE C.FKINCampaignID = " + CampaignsCB.SelectedValue + " and C.FKINBatchID = " + BatchCB.SelectedValue, IsolationLevel.Snapshot).Rows[0][0].ToString();
                    ConservedCampaigns CC = new ConservedCampaigns(long.Parse(CheckIfBatchWasSaved));
                    CC.FKINCampaignID = long.Parse(CampaignsCB.SelectedValue.ToString());
                    CC.FKINBatchID = long.Parse(BatchCB.SelectedValue.ToString());
                    CC.AmountOfLeads = int.Parse(CountLeads);
                    CC.Save(_validationResult);
                }
                catch
                {
                    ConservedCampaigns CC = new ConservedCampaigns();
                    CC.FKINCampaignID = long.Parse(CampaignsCB.SelectedValue.ToString());
                    CC.FKINBatchID = long.Parse(BatchCB.SelectedValue.ToString());
                    CC.AmountOfLeads = int.Parse(CountLeads);
                    CC.Save(_validationResult);
                }


                int x = 0;
                foreach (var row in ReferenceList)
                {
                    try
                    {
                        string ValidityStatus = "";
                        string submitRequest_urlLeadValidity = "http://plhqweb.platinumlife.co.za:999/api/UG/LeadValidity";
                        using (var wb = new WebClient())
                        {
                            string referenceNumber = row["RefNo"].ToString();

                            var data = new NameValueCollection();
                            data["ReferenceNumber"] = referenceNumber;
                            wb.Headers.Add("Authorization", "Bearer " + token);

                            var response = wb.UploadValues(submitRequest_urlLeadValidity, "POST", data);
                            string responseInString = Encoding.UTF8.GetString(response);
                            var customObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseInString);

                            ValidityStatus = (string)customObject["ValidityStatus"];
                        }

                        if(ValidityStatus != "Valid")
                        {
                            try
                            {
                                string GetImportID = Methods.GetTableData("SELECT C.ID FROM INImport as C WHERE C.FKINCampaignID = " + CampaignsCB.SelectedValue + " and C.FKINBatchID = " + BatchCB.SelectedValue + " and C.RefNo = '" + row["RefNo"].ToString() + "'", IsolationLevel.Snapshot).Rows[0][0].ToString();
                                string CheckIfBatchWasSaved = Methods.GetTableData("SELECT C.ID FROM ConservedLeadImportTracker as C WHERE C.FKINImportID = " + GetImportID + " and C.FKINBatchID = " + BatchCB.SelectedValue, IsolationLevel.Snapshot).Rows[0][0].ToString();
                                //LeadImportTracker LIT = new LeadImportTracker(long.Parse(CheckIfBatchWasSaved));
                                //LIT.FKINImportID = long.Parse(GetImportID);
                                //LIT.FKINBatchID = long.Parse(BatchCB.SelectedValue.ToString());
                                //LIT.Save(_validationResult);
                            }
                            catch
                            {
                                string GetImportID = Methods.GetTableData("SELECT C.ID FROM INImport as C WHERE C.FKINCampaignID = " + CampaignsCB.SelectedValue + " and C.FKINBatchID = " + BatchCB.SelectedValue + " and C.RefNo = '" + row["RefNo"].ToString() + "'", IsolationLevel.Snapshot).Rows[0][0].ToString();
                                LeadImportTracker LIT = new LeadImportTracker();
                                LIT.FKINBatchID = long.Parse(BatchCB.SelectedValue.ToString());
                                LIT.FKINImportID = long.Parse(GetImportID.ToString());
                                LIT.Save(_validationResult);
                            }
                        }
                    }
                    catch
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
            catch
            {

            }

            try
            {
                string strQueryConservedData;
                strQueryConservedData = "SELECT  COUNT(ID) FROM [ConservedLeadImportTracker] as [CL] where [CL].FKINBatchID = " + BatchCB.SelectedValue;
                AmountConservedTB.Text = Methods.GetTableData(strQueryConservedData, IsolationLevel.Snapshot).Rows[0][0].ToString();
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
                #region Load Batch ComboBox
                string strQueryAuth;
                strQueryAuth = "SELECT  ID, Code  as Description FROM [INBatch] as [B] where [B].[FKINCampaignID] = " + CampaignsCB.SelectedValue + " order by [B].[ID] Desc";


                DataTable dtAuth = Methods.GetTableData(strQueryAuth);
                BatchCB.Populate(dtAuth, "Description", "ID");
                #endregion
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
                strQueryCampaignData = "SELECT  COUNT(ID) FROM [INImport] as [I] where [I].[FKINCampaignID] = " + CampaignsCB.SelectedValue + " and I.FKINBatchID = " + BatchCB.SelectedValue ;
                AmountOfLeadsTB.Text =  Methods.GetTableData(strQueryCampaignData, IsolationLevel.Snapshot).Rows[0][0].ToString();

                string strQueryConservedData;
                strQueryConservedData = "SELECT  COUNT(ID) FROM [ConservedLeadImportTracker] as [CL] where [CL].FKINBatchID = " + BatchCB.SelectedValue;
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
