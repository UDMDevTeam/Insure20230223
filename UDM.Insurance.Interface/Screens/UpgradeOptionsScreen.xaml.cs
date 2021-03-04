using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using Embriant.WPF.Controls;
using Infragistics.Windows.Editors;
using Infragistics.Windows.Editors.Events;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using System.Text;
using AxCTPhone_ActiveXControl;
using Infragistics.Controls.Menus;
using UDM.Insurance.Interface.Content;
using UDM.Insurance.Interface.Models;
using PlacementMode = System.Windows.Controls.Primitives.PlacementMode;
using Newtonsoft.Json;
using System.Net;
using System.Windows.Markup;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for UpgradeOptionsScreen.xaml
    /// </summary>
    public partial class UpgradeOptionsScreen
    {
        public UpgradeOptionsScreen()
        {
            InitializeComponent();

            //LoadCampaignID(long ? importID);

            loadCampaignInfo();
            SetPolicyDefaults();
            loadCard();

        }

        private void LoadCampaignID(long? importID) 
        {

            parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@ImportID", importID);
            parameters[1] = new SqlParameter("@LoggedInUserID", GlobalSettings.ApplicationUser.ID);
            DataSet ds = Methods.ExecuteStoredProcedure("spINGetLeadByImportID", parameters);

            DataTable dtSale = ds.Tables[5];
            
            LaData.AppData.CampaignID = dtSale.Rows[0]["CampaignID"] as long?;
        }

        #region Private Members

        DataTable dtCover;

        private LeadApplicationData _laData = new LeadApplicationData();
        private SqlParameter[] parameters;

        public LeadApplicationData LaData
        {
            get { return _laData; }
            set { _laData = value; }
        }

        private string DisplayCurrencyFormat(string str)
        {
            string strDisplay = string.Empty;
            string[] words = str.Split(' ');

            foreach (string word in words)
            {
                string strValue = word.Replace(",", "");
                decimal value;

                if (decimal.TryParse(strValue, out value))
                {
                    var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                    nfi.CurrencyDecimalSeparator = ".";
                    nfi.NumberGroupSeparator = ",";

                    strValue = "R " + value.ToString("#,#.00", nfi);
                }

                strDisplay = strDisplay + " " + strValue;
            }

            return strDisplay;
        }

        private void SetPolicyDefaults()
        {
            
            if (LaData.AppData.ImportID == null) return;
            if (LaData.AppData.LeadStatus != null) return;

            if (LaData.AppData.IsLeadUpgrade)
            {
                {//Select Highest Default Option
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@CampaignID", LaData.AppData.CampaignID);
                    parameters[1] = new SqlParameter("@PlanID", LaData.PolicyData.PlanID);
                    DataSet dsLookups = Methods.ExecuteStoredProcedure("spINGetPolicyDefaultOption", parameters);

                    DataTable dt = dsLookups.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        LaData.PolicyData.OptionID = dt.Rows[0]["HigherOptionID"] as long?;
                        return;
                    }
                }
            }

            if (LaData.AppData.IsLeadUpgrade) return;
            if (LaData.AppData.CampaignGroup == lkpINCampaignGroup.Extension) return;

            if ((LaData.AppData.CampaignType == lkpINCampaignType.CancerFuneral || LaData.AppData.CampaignType == lkpINCampaignType.CancerFuneral99) && (LaData.PolicyData.PlanID != null)) //LaData.AppData.CampaignType == lkpINCampaignType.MaccFuneral
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@CampaignTypeID", (long?)LaData.AppData.CampaignType);
                parameters[1] = new SqlParameter("@CampaignGroupID", (long?)LaData.AppData.CampaignGroup);
                parameters[2] = new SqlParameter("@PlanID", LaData.PolicyData.PlanID);
                DataSet dsLookups = Methods.ExecuteStoredProcedure("spINGetPolicyDefaultCover", parameters);

                if (dsLookups.Tables.Count > 0)
                {
                    DataTable dt = dsLookups.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        LaData.PolicyData.LA1Cover = dt.Rows[0]["LA1Cover"] as decimal?;
                        LaData.PolicyData.IsChildChecked = true;
                        
                        LaData.PolicyData.IsFuneralChecked = true;
                        
                    }
                }
            }

            if (LaData.AppData.CampaignType == lkpINCampaignType.FemaleDis || LaData.AppData.CampaignType == lkpINCampaignType.IGFemaleDisability)
            {
               
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@CampaignTypeID", (long?)LaData.AppData.CampaignType);
                parameters[1] = new SqlParameter("@CampaignGroupID", (long?)LaData.AppData.CampaignGroup);
                parameters[2] = new SqlParameter("@PlanID", LaData.PolicyData.PlanID);
                DataSet dsLookups = Methods.ExecuteStoredProcedure("spINGetPolicyDefaultCover", parameters);

                if (dsLookups.Tables.Count > 0)
                {
                    DataTable dt = dsLookups.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        LaData.PolicyData.LA1Cover = dt.Rows[0]["LA1Cover"] as decimal?;
                    }
                }

            }
        }


        #endregion

        private void loadCard()
        {

            List<string> CountCoverList = new List<string>();
            CountCoverList.Clear();

            foreach (var item in dtCover.AsEnumerable())
            {
                DataRow drv = (DataRow)item;
                String valueOfItem = drv["Description"].ToString();
                CountCoverList.Add(valueOfItem);
            }

            int countOptions = CountCoverList.Count();

            ActivateCards(countOptions, CountCoverList);
        }

        private void ActivateCards(int CountOptions, List<string> CountCoverList)
        {
            //CardLayoutPage.Visibility = Visibility.Visible;

            ////CardLayoutPage.AddHandler(Border.MouseLeftButtonDownEvent, new RoutedEventHandler(btnCard1_Click), true);
            ////CardLayoutPage.AddHandler(Border.MouseLeftButtonDownEvent, new RoutedEventHandler(Card2_Click), true);

            //Border border = new Border();

            //border.AddHandler(Border.MouseLeftButtonDownEvent, new RoutedEventHandler(btnCard1_Click), true);

            if (CountOptions == 1)
            {
                btnCard1.IsEnabled = true;
                btnCard1.Content = CountCoverList[0];
                Card2.IsEnabled = false;
                Card2.Visibility = Visibility.Hidden;
                Card3.IsEnabled = false;
                Card3.Visibility = Visibility.Hidden;
                Card4.IsEnabled = false;
                Card4.Visibility = Visibility.Hidden;
                Card5.IsEnabled = false;
                Card5.Visibility = Visibility.Hidden;
                Card6.IsEnabled = false;
                Card6.Visibility = Visibility.Hidden;
                Card7.IsEnabled = false;
                Card7.Visibility = Visibility.Hidden;
                Card8.IsEnabled = false;
                Card8.Visibility = Visibility.Hidden;
                Card9.IsEnabled = false;
                Card9.Visibility = Visibility.Hidden;
                Card10.IsEnabled = false;
                Card10.Visibility = Visibility.Hidden;
                Card11.IsEnabled = false;
                Card11.Visibility = Visibility.Hidden;
                Card12.IsEnabled = false;
                Card12.Visibility = Visibility.Hidden;
            }
            else if (CountOptions == 2)
            {
                btnCard1.IsEnabled = true;
                btnCard1.Background = Brushes.Gray;
                btnCard1.BorderBrush = Brushes.DeepSkyBlue;
                btnCard1.Content = CountCoverList[0];
                Card2.IsEnabled = true;
                Card2.Background = Brushes.Gray;
                Card2.BorderBrush = Brushes.DeepSkyBlue;
                Card2.Content = CountCoverList[1];
                Card3.IsEnabled = false;
                Card3.Visibility = Visibility.Hidden;
                Card4.IsEnabled = false;
                Card4.Visibility = Visibility.Hidden;
                Card5.IsEnabled = false;
                Card5.Visibility = Visibility.Hidden;
                Card6.IsEnabled = false;
                Card6.Visibility = Visibility.Hidden;
                Card7.IsEnabled = false;
                Card7.Visibility = Visibility.Hidden;
                Card8.IsEnabled = false;
                Card8.Visibility = Visibility.Hidden;
                Card9.IsEnabled = false;
                Card9.Visibility = Visibility.Hidden;
                Card10.IsEnabled = false;
                Card10.Visibility = Visibility.Hidden;
                Card11.IsEnabled = false;
                Card11.Visibility = Visibility.Hidden;
                Card12.IsEnabled = false;
                Card12.Visibility = Visibility.Hidden;
            }
            else if (CountOptions == 3)
            {
                btnCard1.IsEnabled = true;
                btnCard1.Background = Brushes.Gray;
                btnCard1.BorderBrush = Brushes.DeepSkyBlue;
                btnCard1.Content = CountCoverList[0];
                Card2.IsEnabled = true;
                Card2.Background = Brushes.Gray;
                Card2.BorderBrush = Brushes.DeepSkyBlue;
                Card2.Content = CountCoverList[1];
                Card3.IsEnabled = true;
                Card3.Background = Brushes.Gray;
                Card3.BorderBrush = Brushes.DeepSkyBlue;
                Card3.Content = CountCoverList[2];
                Card4.IsEnabled = false;
                Card4.Visibility = Visibility.Hidden;
                Card5.IsEnabled = false;
                Card5.Visibility = Visibility.Hidden;
                Card6.IsEnabled = false;
                Card6.Visibility = Visibility.Hidden;
                Card7.IsEnabled = false;
                Card7.Visibility = Visibility.Hidden;
                Card8.IsEnabled = false;
                Card8.Visibility = Visibility.Hidden;
                Card9.IsEnabled = false;
                Card9.Visibility = Visibility.Hidden;
                Card10.IsEnabled = false;
                Card10.Visibility = Visibility.Hidden;
                Card11.IsEnabled = false;
                Card11.Visibility = Visibility.Hidden;
                Card12.IsEnabled = false;
                Card12.Visibility = Visibility.Hidden;
            }
            else if (CountOptions == 4)
            {
                btnCard1.IsEnabled = true;
                btnCard1.Background = Brushes.Gray;
                btnCard1.BorderBrush = Brushes.DeepSkyBlue;
                btnCard1.Content = CountCoverList[0];
                Card2.IsEnabled = true;
                Card2.Background = Brushes.Gray;
                Card2.BorderBrush = Brushes.DeepSkyBlue;
                Card2.Content = CountCoverList[1];
                Card3.IsEnabled = true;
                Card3.Background = Brushes.Gray;
                Card3.BorderBrush = Brushes.DeepSkyBlue;
                Card3.Content = CountCoverList[2];
                Card4.IsEnabled = true;
                Card4.Background = Brushes.Gray;
                Card4.BorderBrush = Brushes.DeepSkyBlue;
                Card4.Content = CountCoverList[3];
                Card5.IsEnabled = false;
                Card5.Visibility = Visibility.Hidden;
                Card6.IsEnabled = false;
                Card6.Visibility = Visibility.Hidden;
                Card7.IsEnabled = false;
                Card7.Visibility = Visibility.Hidden;
                Card8.IsEnabled = false;
                Card8.Visibility = Visibility.Hidden;
                Card9.IsEnabled = false;
                Card9.Visibility = Visibility.Hidden;
                Card10.IsEnabled = false;
                Card10.Visibility = Visibility.Hidden;
                Card11.IsEnabled = false;
                Card11.Visibility = Visibility.Hidden;
                Card12.IsEnabled = false;
                Card12.Visibility = Visibility.Hidden;
            }
            else if (CountOptions == 5)
            {
                btnCard1.IsEnabled = true;
                btnCard1.Background = Brushes.Gray;
                btnCard1.BorderBrush = Brushes.DeepSkyBlue;
                btnCard1.Content = CountCoverList[0];
                Card2.IsEnabled = true;
                Card2.Background = Brushes.Gray;
                Card2.BorderBrush = Brushes.DeepSkyBlue;
                Card2.Content = CountCoverList[1];
                Card3.IsEnabled = true;
                Card3.Background = Brushes.Gray;
                Card3.BorderBrush = Brushes.DeepSkyBlue;
                Card3.Content = CountCoverList[2];
                Card4.IsEnabled = true;
                Card4.Background = Brushes.Gray;
                Card4.BorderBrush = Brushes.DeepSkyBlue;
                Card4.Content = CountCoverList[3];
                Card5.IsEnabled = true;
                Card5.Background = Brushes.Gray;
                Card5.BorderBrush = Brushes.DeepSkyBlue;
                Card5.Content = CountCoverList[4];
                Card6.IsEnabled = false;
                Card6.Visibility = Visibility.Hidden;
                Card7.IsEnabled = false;
                Card7.Visibility = Visibility.Hidden;
                Card8.IsEnabled = false;
                Card8.Visibility = Visibility.Hidden;
                Card9.IsEnabled = false;
                Card9.Visibility = Visibility.Hidden;
                Card10.IsEnabled = false;
                Card10.Visibility = Visibility.Hidden;
                Card11.IsEnabled = false;
                Card11.Visibility = Visibility.Hidden;
                Card12.IsEnabled = false;
                Card12.Visibility = Visibility.Hidden;
            }
            else if (CountOptions == 6)
            {



                btnCard1.IsEnabled = true;
                btnCard1.Content = CountCoverList[0];
                Card2.IsEnabled = true;
                Card2.Content = CountCoverList[1];
                Card3.IsEnabled = true;
                Card3.Content = CountCoverList[2];
                Card4.IsEnabled = true;
                Card4.Content = CountCoverList[3];
                Card5.IsEnabled = true;
                Card5.Content = CountCoverList[4];
                Card6.IsEnabled = true;
                Card6.Content = CountCoverList[5];
                Card7.IsEnabled = false;
                Card7.Visibility = Visibility.Hidden;
                Card8.IsEnabled = false;
                Card8.Visibility = Visibility.Hidden;
                Card9.IsEnabled = false;
                Card9.Visibility = Visibility.Hidden;
                Card10.IsEnabled = false;
                Card10.Visibility = Visibility.Hidden;
                Card11.IsEnabled = false;
                Card11.Visibility = Visibility.Hidden;
                Card12.IsEnabled = false;
                Card12.Visibility = Visibility.Hidden;


            }
            else if (CountOptions == 7)
            {
                btnCard1.IsEnabled = true;
                btnCard1.Content = CountCoverList[0];
                Card2.IsEnabled = true;
                Card2.Content = CountCoverList[1];
                Card3.IsEnabled = true;
                Card3.Content = CountCoverList[2];
                Card4.IsEnabled = true;
                Card4.Content = CountCoverList[3];
                Card5.IsEnabled = true;
                Card5.Content = CountCoverList[4];
                Card6.IsEnabled = true;
                Card6.Content = CountCoverList[5];
                Card7.IsEnabled = true;
                Card7.Content = CountCoverList[6];
                Card8.IsEnabled = false;
                Card8.Visibility = Visibility.Hidden;
                Card9.IsEnabled = false;
                Card9.Visibility = Visibility.Hidden;
                Card10.IsEnabled = false;
                Card10.Visibility = Visibility.Hidden;
                Card11.IsEnabled = false;
                Card11.Visibility = Visibility.Hidden;
                Card12.IsEnabled = false;
                Card12.Visibility = Visibility.Hidden;
            }
            else if (CountOptions == 8)
            {
                btnCard1.IsEnabled = true;
                btnCard1.Content = CountCoverList[5];
                Card2.IsEnabled = true;
                Card2.Content = CountCoverList[5];
                Card3.IsEnabled = true;
                Card3.Content = CountCoverList[5];
                Card4.IsEnabled = true;
                Card4.Content = CountCoverList[5];
                Card5.IsEnabled = true;
                Card5.Content = CountCoverList[5];
                Card6.IsEnabled = true;
                Card6.Content = CountCoverList[5];
                Card7.IsEnabled = true;
                Card7.Content = CountCoverList[5];
                Card8.IsEnabled = true;
                Card8.Content = CountCoverList[5];
                Card9.IsEnabled = false;
                Card9.Visibility = Visibility.Hidden;
                Card10.IsEnabled = false;
                Card10.Visibility = Visibility.Hidden;
                Card11.IsEnabled = false;
                Card11.Visibility = Visibility.Hidden;
                Card12.IsEnabled = false;
                Card12.Visibility = Visibility.Hidden;
            }
            else if (CountOptions == 9)
            {
                btnCard1.IsEnabled = true;
                btnCard1.Content = CountCoverList[0];
                Card2.IsEnabled = true;
                Card2.Content = CountCoverList[1];
                Card3.IsEnabled = true;
                Card3.Content = CountCoverList[2];
                Card4.IsEnabled = true;
                Card4.Content = CountCoverList[3];
                Card5.IsEnabled = true;
                Card5.Content = CountCoverList[4];
                Card6.IsEnabled = true;
                Card6.Content = CountCoverList[5];
                Card7.IsEnabled = true;
                Card7.Content = CountCoverList[6];
                Card8.IsEnabled = true;
                Card8.Content = CountCoverList[7];
                Card9.IsEnabled = true;
                Card9.Content = CountCoverList[8];
                Card10.IsEnabled = false;
                Card10.Visibility = Visibility.Hidden;
                Card11.IsEnabled = false;
                Card11.Visibility = Visibility.Hidden;
                Card12.IsEnabled = false;
                Card12.Visibility = Visibility.Hidden;
            }
            else if (CountOptions == 10)
            {
                btnCard1.IsEnabled = true;
                btnCard1.Content = CountCoverList[0];
                Card2.IsEnabled = true;
                Card2.Content = CountCoverList[1];
                Card3.IsEnabled = true;
                Card3.Content = CountCoverList[2];
                Card4.IsEnabled = true;
                Card4.Content = CountCoverList[3];
                Card5.IsEnabled = true;
                Card5.Content = CountCoverList[4];
                Card6.IsEnabled = true;
                Card6.Content = CountCoverList[5];
                Card7.IsEnabled = true;
                Card7.Content = CountCoverList[6];
                Card8.IsEnabled = true;
                Card8.Content = CountCoverList[7];
                Card9.IsEnabled = true;
                Card9.Content = CountCoverList[8];
                Card10.IsEnabled = true;
                Card10.Content = CountCoverList[9];
                Card11.IsEnabled = false;
                Card11.Visibility = Visibility.Hidden;
                Card12.IsEnabled = false;
                Card12.Visibility = Visibility.Hidden;
            }
            else if (CountOptions == 11)
            {
                btnCard1.IsEnabled = true;
                btnCard1.Content = CountCoverList[0];
                Card2.IsEnabled = true;
                Card2.Content = CountCoverList[1];
                Card3.IsEnabled = true;
                Card3.Content = CountCoverList[2];
                Card4.IsEnabled = true;
                Card4.Content = CountCoverList[3];
                Card5.IsEnabled = true;
                Card5.Content = CountCoverList[4];
                Card6.IsEnabled = true;
                Card6.Content = CountCoverList[5];
                Card7.IsEnabled = true;
                Card7.Content = CountCoverList[6];
                Card8.IsEnabled = true;
                Card8.Content = CountCoverList[7];
                Card9.IsEnabled = true;
                Card9.Content = CountCoverList[8];
                Card10.IsEnabled = true;
                Card10.Content = CountCoverList[9];
                Card11.IsEnabled = true;
                Card11.Content = CountCoverList[10];
                Card12.IsEnabled = false;
                Card12.Visibility = Visibility.Hidden;
            }
            else if (CountOptions == 12)
            {
                btnCard1.IsEnabled = true;
                btnCard1.Content = CountCoverList[0];
                Card2.IsEnabled = true;
                Card2.Content = CountCoverList[1];
                Card3.IsEnabled = true;
                Card3.Content = CountCoverList[2];
                Card4.IsEnabled = true;
                Card4.Content = CountCoverList[3];
                Card5.IsEnabled = true;
                Card5.Content = CountCoverList[4];
                Card6.IsEnabled = true;
                Card6.Content = CountCoverList[5];
                Card7.IsEnabled = true;
                Card7.Content = CountCoverList[6];
                Card8.IsEnabled = true;
                Card8.Content = CountCoverList[7];
                Card9.IsEnabled = true;
                Card9.Content = CountCoverList[8];
                Card10.IsEnabled = true;
                Card10.Content = CountCoverList[9];
                Card11.IsEnabled = true;
                Card11.Content = CountCoverList[10];
                Card12.IsEnabled = true;
                Card12.Content = CountCoverList[11];
            }
        }

        private void loadCampaignInfo() 
        {
            if (LaData.PolicyData.PlanID != null)
            {
                decimal? selectedLA1Cover = LaData.PolicyData.LA1Cover;
                long? selectedUpgradeCover = LaData.PolicyData.OptionID;

                SqlParameter[] parameters = new SqlParameter[5];//5
                parameters[0] = new SqlParameter("@CampaignID", LaData.AppData.CampaignID);
                parameters[1] = new SqlParameter("@PlanID", LaData.PolicyData.PlanID);
                parameters[2] = new SqlParameter("@UserID", LaData.UserData.UserID);
                parameters[3] = new SqlParameter("@OptionID", LaData.PolicyData.OptionID);

                if (LaData.PolicyData.OptionID == null)
                {
                    parameters[3].Value = DBNull.Value;
                }

                

                DataSet dsLookups = Methods.ExecuteStoredProcedure("_spGetPolicyPlanCovers", parameters);
                dtCover = dsLookups.Tables[0];

                foreach (DataRow row in dtCover.Rows)
                {
                    if (row != null && row["Description"] != null && row["Description"] != DBNull.Value)
                    {
                        string str = row["Description"].ToString();
                        row["Description"] = DisplayCurrencyFormat(str);
                    }
                }

                //if (LaData.AppData.IsLeadUpgrade)
                //{
                //    LeadApplicationScreen leadApplicationScreen = new LeadApplicationScreen(); 

                //    cmbUpgradeCover.Populate(dtCover, "Description", "Value");
                //    LaData.PolicyData.OptionID = selectedUpgradeCover;
                //}
                //else
                //{
                //    cmbLA1Cover.Populate(dtCover, "Description", "Value");
                //    LaData.PolicyData.LA1Cover = selectedLA1Cover;
                //}

                
            }

        }

        private void ReturnToPage2()
        {
            //Page1.Visibility = Visibility.Hidden;
            //Page2.Visibility = Visibility.Visible;
            //Page3.Visibility = Visibility.Hidden;
            //Page4.Visibility = Visibility.Hidden;
            //Page5.Visibility = Visibility.Hidden;



            //LeadApplicationScreen leadApplicationScreen = new LeadApplicationScreen();
            UpgradeOptionsScreen upgradeOptionsScreen = new UpgradeOptionsScreen();
            //ShowDialog(leadApplicationScreen, new INDialogWindow(leadApplicationScreen));

            //leadApplicationScreen.Page2.Visibility = Visibility.Visible;

            CardLayoutPage.IsEnabled = false;
            CardLayoutPage.Visibility = Visibility.Hidden;


            //leadApplicationScreen.Page1.Visibility = Visibility.Hidden;
            //leadApplicationScreen.Page2.Visibility = Visibility.Visible;
            //leadApplicationScreen.Page3.Visibility = Visibility.Hidden;
            //leadApplicationScreen.Page4.Visibility = Visibility.Hidden;
            //leadApplicationScreen.Page5.Visibility = Visibility.Hidden;

            //leadApplicationScreen.CardLayoutPage.IsEnabled = false;
            //leadApplicationScreen.CardLayoutPage.Visibility = Visibility.Hidden;

            

            //leadApplicationScreen.Page1.Visibility = Visibility.Hidden;
            //leadApplicationScreen.Page2.Visibility = Visibility.Visible;
            //leadApplicationScreen.Page3.Visibility = Visibility.Hidden;
            //leadApplicationScreen.Page4.Visibility = Visibility.Hidden;
            //leadApplicationScreen.Page5.Visibility = Visibility.Hidden;


        }

        private void btnCard1_Click(object sender, RoutedEventArgs e)
        {

            ReturnToPage2();

        }
    }
}
