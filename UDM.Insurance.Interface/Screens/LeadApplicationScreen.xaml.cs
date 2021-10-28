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
using System.Windows.Media.Imaging;
using System.Collections.Specialized;
using System.Transactions;
//using static UDM.WPF.Enumerations.Insure;

namespace UDM.Insurance.Interface.Screens
{
    public partial class LeadApplicationScreen
    {

        #region Unmanaged Methods

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        bool DebiCheckSentTwice;

        #endregion

        #region Constants

        //Mercantile variables
        private const string mercUsername = "SOAP";
        private const string mercInstCode = "UDM1";
        private const string mercPassword = "Password1";

        private const string debiCheckURL = "http://plhqweb.platinumlife.co.za:8081/";

        string PolicyNumberPLLKP = "";
        string ReferenceNumberPLLKP = "";
        string ResponseDetailPLLKP = ""; // WILL RETURN "OK" OR "No Data Found"
        string BankPLLKP = "";
        string AccountNumberPLLKP = "";
        string AccountHolderPLLKP = "";
        string BranchCodePLLKP = "";
        string AccountTypePLLKP = "";
        string DebitDayPLLKP = "";

        #region BulkSMS

        //Bulk SMS variables
        private const string bulkSMSTokenName = "UDMSoftware";
        private const string bulkSMStokenID = "5DF865E90D5D4AEE9F2A6790D02959FC-01-0";
        private const string bulkSMStokenSecret = "aQ958yyEQA*v8E1sugmfMVVg!HPRO";

        private const string bulkSMSUserName = "UDMIG";
        private const string bulkSMSPassword = "Gorilla1!";
        private const string bulkSMSURL = "https://api.bulksms.com/v1";

        #endregion BulkSMS


        private int BumpUpSelected = 0;

        private const string IDField = "ID";
        private const string DescriptionField = "Description";
        private readonly Enum[] _upgrades =
        {
            lkpINCampaignGroup.Upgrade1,
            lkpINCampaignGroup.Upgrade2,
            lkpINCampaignGroup.Upgrade3,
            lkpINCampaignGroup.Upgrade4,
            lkpINCampaignGroup.Upgrade5,
            lkpINCampaignGroup.Upgrade6,
            lkpINCampaignGroup.Upgrade7,
            lkpINCampaignGroup.Upgrade8,
            lkpINCampaignGroup.Upgrade9,
            lkpINCampaignGroup.Upgrade10,
            lkpINCampaignGroup.Upgrade11,
            lkpINCampaignGroup.Upgrade12,
            lkpINCampaignGroup.Upgrade13,
            lkpINCampaignGroup.DoubleUpgrade1,
            lkpINCampaignGroup.DoubleUpgrade2,
            lkpINCampaignGroup.DoubleUpgrade3,
            lkpINCampaignGroup.DoubleUpgrade4,
            lkpINCampaignGroup.DoubleUpgrade5,
            lkpINCampaignGroup.DoubleUpgrade6,
            lkpINCampaignGroup.DoubleUpgrade7,
            lkpINCampaignGroup.DoubleUpgrade8,
            lkpINCampaignGroup.DoubleUpgrade9,
            lkpINCampaignGroup.DoubleUpgrade10,
            lkpINCampaignGroup.DoubleUpgrade11,
            lkpINCampaignGroup.DoubleUpgrade12,
            lkpINCampaignGroup.DoubleUpgrade13,
            lkpINCampaignGroup.DoubleUpgrade14

        };
        //lkpINCampaignType.BlackMaccMillion
        readonly IEnumerable<lkpINCampaignType?> campaignTypesCancer = new lkpINCampaignType?[] { lkpINCampaignType.Cancer, lkpINCampaignType.CancerFuneral, lkpINCampaignType.IGCancer, lkpINCampaignType.TermCancer, };
        readonly IEnumerable<lkpINCampaignType?> campaignTypesMacc = new lkpINCampaignType?[] { lkpINCampaignType.Macc, lkpINCampaignType.MaccFuneral, lkpINCampaignType.MaccMillion, lkpINCampaignType.BlackMacc, lkpINCampaignType.FemaleDis, lkpINCampaignType.AccDis, lkpINCampaignType.IGFemaleDisability, lkpINCampaignType.BlackMaccMillion };
        readonly IEnumerable<lkpINCampaignType?> campaignTypesMaccNotAccDis = new lkpINCampaignType?[] { lkpINCampaignType.Macc, lkpINCampaignType.MaccFuneral, lkpINCampaignType.MaccMillion, lkpINCampaignType.BlackMacc, lkpINCampaignType.BlackMaccMillion, lkpINCampaignType.FemaleDis, lkpINCampaignType.IGFemaleDisability };

        #endregion

        #region CardOptionID Variables
        int? card1ID = null;
        int? card2ID = null;
        int? card3ID = null;
        int? card4ID = null;
        int? card5ID = null;
        int? card6ID = null;
        int? card7ID = null;
        int? card8ID = null;
        int? card9ID = null;
        int? card10ID = null;
        int? card11ID = null;
        int? card12ID = null;
        int? card13ID = null;
        int? card14ID = null;
        int? card15ID = null;
        int? card16ID = null;


        List<Uri> ImageURLString = new List<Uri>();
        long? DefaultOptionIDCards = null;
        #endregion

        #region Private Members

        private string _strXpsDoc;
        private XpsDocument _xpsDocument;
        private Key _popupKey = Key.Escape;
        private readonly IEnumerable<Border> _pages;
        public long? _declineReasonID = -1;
        public long? _cancellationReasonID = -1;
        string _la1AccDeathCover = string.Empty;
        string _la2AccDeathCover = string.Empty;
        string _la1Cover = string.Empty;
        string _la2Cover = string.Empty;
        string _childCover = string.Empty;
        string _totalLa1Cover = string.Empty;
        string _totalLa2Cover = string.Empty;
        string _totalLa1AccDeathCover = string.Empty;
        string _totalLa2AccDeathCover = string.Empty;
        private readonly SalesScreenGlobalData _ssGlobalData;
        private LeadApplicationData _laData = new LeadApplicationData();

        private int _la1FuneralCover = 0;
        private int _la1AccidentalDeathCover = 0;
        private int _funeralCover;
        INImportCallMonitoring inImportCallMonitoring;

        DataTable dtCover;


        string permissionTitle = null;
        string permissionFirstname = null;
        string permissionSurname = null;
        string permissionCellNumber = null;
        string permissionAltNumber = null;
        public LeadApplicationData LaData
        {
            get { return _laData; }
            set { _laData = value; }
        }

        private bool _flagLeadIsBusyLoading;
        //private bool _isNotesFeatureAvailable = false;

        private Point _posMouseVB;
        private Point _posVB;

        //private LiveProductivity _liveProductivity;

        private bool _hasNoteBeenDisplayed;

        private DependencyObject _focusScope;
        private IInputElement _focusedElement;

        //CTPhone
        private System.Windows.Forms.Integration.WindowsFormsHost _hostCTPhone;
        private AxCTPhone_ActiveX _axCtPhone;
        private readonly DispatcherTimer _callDuration = new DispatcherTimer();
        private int _secCallduration;

        //StopWatch
        private readonly Stopwatch _swNextLead = new Stopwatch();
        private readonly Stopwatch _swPrevLead = new Stopwatch();

        //Colors
        private SolidColorBrush defaultBorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF808080"));

        #endregion Private Members

        #region Constructors

        public LeadApplicationScreen(long? importID, SalesScreenGlobalData ssGlobalData)
        {
            Methods.SetNumberFormat();
            //CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture; //for testing

            InitializeComponent();
            _ssGlobalData = ssGlobalData;
            _ssGlobalData.LeadApplicationScreen = this;

            if (GlobalSettings.ApplicationUser.ID == 1 || GlobalSettings.ApplicationUser.ID == 199 || GlobalSettings.ApplicationUser.ID == 72)
            {
                btnOverrideBumpUp.Visibility = Visibility.Visible;
            }



            Page1.Visibility = Visibility.Visible;
            Page2.Visibility = Visibility.Collapsed;
            Page3.Visibility = Visibility.Collapsed;
            Page4.Visibility = Visibility.Collapsed;
            Page5.Visibility = Visibility.Collapsed;
            ClosurePage.Visibility = Visibility.Hidden; //Otherwise scrollviewer on dvclosure cannot be found (when collapsed)

            Beneficiary1.Visibility = Visibility.Visible;
            Beneficiary2.Visibility = Visibility.Collapsed;
            Beneficiary3.Visibility = Visibility.Collapsed;
            Beneficiary4.Visibility = Visibility.Collapsed;
            Beneficiary5.Visibility = Visibility.Collapsed;
            Beneficiary6.Visibility = Visibility.Collapsed;

            DOAccountHolderDetail.Visibility = Visibility.Collapsed;
            DOAccountDetail.Visibility = Visibility.Collapsed;
            BankingDetails.Visibility = Visibility.Collapsed;

            CheckIfTemp();

            _pages = new[] { Page1, Page2, Page3, Page4, Page5, ClosurePage };

            LoadLookupData();
            UnLoadUpgradeLead(); //necessary for correct initial layout

            //_hasNoteBeenDisplayed = false;

            //InitializeCTPhone();
            _callDuration.Tick += CTPhone_CallDuration;
            _callDuration.Interval = new TimeSpan(0, 0, 1);

            if (importID != null)
            {

                LaData.AppData.CanClientBeContacted = Insure.CanClientBeContacted(importID.Value);
                ShowOrHideFields(LaData.AppData.CanClientBeContacted);

                //_isNotesFeatureAvailable = Insure.IsNotesFeatureAvailable(importID.Value);
                //if (_isNotesFeatureAvailable)
                //{
                //    LaData.AppData.NotesFeatureVisibility = Visibility.Visible;
                //}
                //else
                //{
                //    LaData.AppData.NotesFeatureVisibility = Visibility.Collapsed;
                //}

                LoadLead(importID);


                //ShowNotes(importID.Value);
            }
            //CanSave.IsChecked = true;

            //DisplayLiveProductivity();

#if RELEASEBUILD
            LaData.AppData.SolutionConfiguration = lkpSolutionConfiguration.Release;
#elif DEBUG
                LaData.AppData.SolutionConfiguration = lkpSolutionConfiguration.Debug;
#elif TESTBUILD
                LaData.AppData.SolutionConfiguration = lkpSolutionConfiguration.Test;
#elif TRAININGBUILD
                LaData.AppData.SolutionConfiguration = lkpSolutionConfiguration.Training;
#endif
        }

        public LeadApplicationScreen(long? importID, SalesScreenGlobalData ssGlobalData, bool openCMScreen) : this(importID, ssGlobalData)
        {
            if (openCMScreen)
            {
                CallMonitoringDetailsScreen callMonitoringDetailsScreen = new CallMonitoringDetailsScreen(LaData);
                IsEnabled = false;
                callMonitoringDetailsScreen.ShowDialog();
                IsEnabled = true;
            }

        }

        private void CheckIfTemp()
        {
            #region Check if Temp

            if (GlobalSettings.ApplicationUser.ID != 0)
            {

                var valueStaffType = Methods.GetTableData("SELECT TOP 1 * FROM Blush.dbo.HRStaff AS HRS WHERE FKUserID = " + GlobalSettings.ApplicationUser.ID)?.AsEnumerable().Select(x => x["FKStaffTypeID"]).FirstOrDefault();

                if (valueStaffType != DBNull.Value)
                {
                    LaData.UserData.StaffType = (lkpStaffType?)(long?)valueStaffType;
                }


                var valueTempStartDate = Methods.GetTableData("SELECT TOP 1 * FROM Blush.dbo.HRStaff AS HRS WHERE FKUserID = " + GlobalSettings.ApplicationUser.ID)?.AsEnumerable().Select(x => x["TempStartDate"]).FirstOrDefault();

                if (valueTempStartDate != DBNull.Value)
                {
                    LaData.UserData.TempStartDate = Convert.ToDateTime(valueTempStartDate);//TempStartDate needs to be set before staff type
                }

                if (LaData.UserData.StaffType != null)
                {
                    if (LaData.UserData.StaffType == lkpStaffType.Temporary)
                    {
                        CopyToBenef.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    CopyToBenef.Visibility = Visibility.Collapsed;
                }
            }


            #endregion Check if Temp
        }

        //private void DisplayLiveProductivity()
        //{
        //    // See https://udmint.basecamphq.com/projects/10204838-udm-hr/todo_items/203770077/comments
        //    _liveProductivity = new LiveProductivity();
        //    _liveProductivity.Show();
        //}

        private void DisplayRemainingSales(long? importID)
        {
            bool show = importID.HasValue;
            string message = string.Empty; //Insure.INDetermineRemainingSales( 
            string contactMessage = string.Empty;
            decimal targetPercentage;
            decimal contactPercentage;
            DataSet dsMessages = new DataSet();
            //Run runContact = new Run();

            if (show)
            {
                dsMessages = Insure.INDetermineRemainingSales(importID.Value);
                message = dsMessages.Tables[0].Rows[0][0].ToString();
                contactMessage = dsMessages.Tables[1].Rows[0]["ContactRate"].ToString();
                //LaData.AppData.TargetPercentage = (decimal)dsMessages.Tables[1].Rows[0]["TargetPercentage"];
                //LaData.AppData.ContactPercentage = (decimal)dsMessages.Tables[1].Rows[0]["ContactPercentage"];
                LaData.AppData.IsOverTarget = Boolean.Parse(dsMessages.Tables[1].Rows[0]["IsOverTarget"].ToString());
                spMessage.Visibility = Visibility.Visible;
                lblSalesRemaining.Text = message;
                //lblSalesRemaining.Inlines.Clear();
                //lblSalesRemaining.Inlines.Add(message);
                lblContactMessage.Text = contactMessage;
                //if (contactPercentage >= targetPercentage)
                //{
                //    lblContactMessage.Foreground = new SolidColorBrush(Colors.LimeGreen);
                //}
                //else
                //{
                //    lblContactMessage.Foreground = new SolidColorBrush(Colors.Firebrick);
                //}

                #region Animations For Contact Percentage

                RepeatBehavior rb = new RepeatBehavior(10);

                var fade = new DoubleAnimation()
                {
                    From = 18,
                    To = 20,
                    Duration = TimeSpan.FromMilliseconds(250),
                    AutoReverse = true,
                    RepeatBehavior = rb
                };

                Storyboard.SetTarget(fade, lblContactMessage);
                Storyboard.SetTargetProperty(fade, new PropertyPath(TextBlock.FontSizeProperty));

                var sb = new Storyboard();
                sb.Children.Add(fade);

                sb.Begin();

                #endregion Animations For Contact Percentage
                //lblSalesRemaining.Inlines.Add(runContact);

            }
            else
            {
                spMessage.Visibility = Visibility.Hidden;
                lblSalesRemaining.Text = string.Empty;
            }
        }

        #endregion

        #region Publicly Exposed Methods

        public void NotifyUserIfClientCannotBeContacted(bool canClientBeContacted)
        {
            if (!canClientBeContacted)
            {
                INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                ShowMessageBox(messageWindow, @"This lead cannot be loaded in its entirety, because the client has requested not to be contacted again.", "DO NOT CONTACT CLIENT", ShowMessageType.Exclamation);
            }
        }

        #endregion Publicly Exposed Methods

        #region Private Methods

        //private bool CanClientBeContacted(long importID)
        //{
        //    bool result = false;

        //    SqlParameter[] parameters = new SqlParameter[1];
        //    parameters[0] = new SqlParameter("@ImportID", importID);
        //    DataTable dt = Methods.ExecuteStoredProcedure("spINDetermineLeadContactability", parameters).Tables[0];

        //    result = (bool)dt.Rows[0][0];

        //    return result;
        //}

        private string UppercaseFirst(string s)
        {

            char[] a = new char[] { ' ' };
            char[] seperator = new char[] { ' ' };
            int arrayCount = 0;

            if (!string.IsNullOrEmpty(s))
            {
                string[] words = s.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
                if (words.Count() > 0 && !string.IsNullOrEmpty(s))
                {
                    //Loop through all the words
                    foreach (string word in words)
                    {
                        //Get the current word
                        a = word.ToCharArray();
                        //Set the first character to uppercase
                        a[0] = char.ToUpper(a[0]);
                        for (int x = 1; x < a.Length; x++)
                        {
                            a[x] = char.ToLower(a[x]);
                        }
                        words[arrayCount] = new string(a);
                        arrayCount++;
                    }
                }
                return ConvertStringArrayToString(words).Trim();
            }
            return s;
        }

        static string ConvertStringArrayToString(string[] array)
        {
            // Concatenate all the elements into a StringBuilder.
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
                builder.Append(" ");
            }
            return builder.ToString();
        }

        private long? GetBaseOptionID(long? PlanID, decimal? LA1Cover, bool? IsLA2Checked, decimal? LA2Cover)
        {
            LA1Cover = LA1Cover ?? 0.00m;
            LA2Cover = LA2Cover ?? 0.00m;

            if (PlanID != null)
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@PlanID", PlanID);
                parameters[1] = new SqlParameter("@LA1Cover", LA1Cover);
                parameters[2] = new SqlParameter("@IsLA2Checked", IsLA2Checked);
                parameters[3] = new SqlParameter("@LA2Cover", LA2Cover);
                object obj = Methods.ExecuteFunction("fnINGetOptionID1", parameters);

                return obj as long?;
            }

            return null;
        }

        private bool LogInfo()
        {
            //check if optionID is actually correct and consistent (e.g 1N / 1M issue)
            if (!LaData.AppData.IsLeadUpgrade)
            {
                long? optionID = GetBaseOptionID(LaData.PolicyData.PlanID, LaData.PolicyData.LA1Cover, LaData.PolicyData.IsLA2Checked, null);

                if (optionID != null)
                {
                    if (LaData.PolicyData.OptionID != optionID)
                    {
                        InfoLog1 log = new InfoLog1();
                        log.FKImportID = LaData.AppData.ImportID;
                        log.FKPlanID = LaData.PolicyData.PlanID;
                        log.LA1Cover = LaData.PolicyData.LA1Cover;
                        log.IsLA2Checked = LaData.PolicyData.IsLA2Checked;
                        log.FKOptionID1 = LaData.PolicyData.OptionID;
                        log.FKOptionID2 = optionID;
                        log.Save(_validationResult);

                        return true;
                    }
                }
            }

            return false;
        }

        private void LoadLead(long? importID)
        {
            try
            {
                try
                {
                    chkMoveToLeadPermissions.IsChecked = false;

                }
                catch
                {

                }

                DebiCheckSentTwice = false;
                Mandate1TB.Text = " ";
                Mandate2TB.Text = " ";

                #region DebiCheck Button Enabled
                #endregion

                SetCursor(Cursors.Wait);
                CheckIfTemp();
                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/211742618/comments
                UpdateLeadStatuses(importID);
                UpdateLeadApplicationScreenLookups(importID);
                //LoadLookupData();

                try { LaData.AppData.ImportID = importID; } catch { }


                if (importID == null) return;

                bool isNotesFeatureAvailable = Insure.IsNotesFeatureAvailable(importID.Value);
                if (isNotesFeatureAvailable)
                {
                    LaData.AppData.NotesFeatureVisibility = Visibility.Visible;
                }
                else
                {
                    LaData.AppData.NotesFeatureVisibility = Visibility.Collapsed;
                }

                _flagLeadIsBusyLoading = true;
                _la1AccDeathCover = string.Empty;
                _la2AccDeathCover = string.Empty;
                _childCover = string.Empty;
                _la1Cover = string.Empty;
                _la2Cover = string.Empty;
                _totalLa1Cover = string.Empty;
                _totalLa2Cover = string.Empty;
                _totalLa1AccDeathCover = string.Empty;
                _totalLa2AccDeathCover = string.Empty;

                #region Get the complete lead from the database

                #region Refresh agent list to include agents used for this import id

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ImportID", importID);
                DataSet dsAgentLookups = Methods.ExecuteStoredProcedure("spINGetLeadApplicationScreenAgents", parameters);

                DataTable dtSalesAgents = dsAgentLookups.Tables[0];
                cmbAgent.Populate(dtSalesAgents, "Name", IDField);
                cmbSaleCallRef.Populate(dtSalesAgents, "Name", IDField);
                cmbBankCallRef.Populate(dtSalesAgents, "Name", IDField);

                #endregion

                parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ImportID", importID);
                parameters[1] = new SqlParameter("@LoggedInUserID", GlobalSettings.ApplicationUser.ID);
                DataSet ds = Methods.ExecuteStoredProcedure("spINGetLeadByImportID", parameters);

                DataTable dtLead = ds.Tables[0];
                DataTable dtPolicy = ds.Tables[1];
                DataTable dtBanking = ds.Tables[2];
                DataTable dtLA = ds.Tables[3];
                DataTable dtBeneficiary = ds.Tables[4];
                DataTable dtSale = ds.Tables[5];
                DataTable dtChild = ds.Tables[6];
                DataTable dtImportedPolicyData = ds.Tables[7];
                DataTable dtNextOfKin = ds.Tables[8];
                DataTable dtSMS = ds.Tables[9];

                #endregion Get the complete lead from the database



                #region Get the lead History from the database

                parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ImportID", importID);
                DataSet dsHst = Methods.ExecuteStoredProcedure("spINGetLeadHstByImportID", parameters);

                DataTable dtLeadHst = dsHst.Tables[0];
                DataTable dtPolicyHst = dsHst.Tables[1];
                DataTable dtLAHst = dsHst.Tables[2];
                DataTable dtBeneficiaryHst = dsHst.Tables[3];
                DataTable dtChildHst = dsHst.Tables[4];
                DataTable dtNextOfKinHst = dsHst.Tables[5];

                #endregion Get the lead History from the database

                #region (General) Application Data
                LaData.AppData.AgentID = dtLead.Rows[0]["AgentID"] as long?;
                LaData.AppData.RefNo = LaData.AppData.LoadedRefNo = dtLead.Rows[0]["RefNo"] as string;
                LaData.AppData.DateOfSale = LaData.AppData.LoadedDateOfSale = dtLead.Rows[0]["DateOfSale"] as DateTime?;
                LaData.AppData.LeadStatus = dtLead.Rows[0]["StatusID"] as long?;
                LaData.AppData.lkpLeadStatus = (lkpINLeadStatus?)LaData.AppData.LeadStatus;
                LaData.AppData.LoadedLeadStatus = dtLead.Rows[0]["StatusID"] as long?;
                LaData.AppData.DeclineReasonID = dtLead.Rows[0]["DeclineReasonID"] as long?;
                LaData.AppData.CancellationReasonID = dtLead.Rows[0]["CancellationReasonID"] as long?; // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/204782186/comments

                LaData.AppData.CarriedForwardReasonID = dtLead.Rows[0]["CarriedForwardReasonID"] as long?; // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/220642271/comments

                LaData.AppData.FKINCallMonitoringCarriedForwardReasonID = dtLead.Rows[0]["FKINCallMonitoringCarriedForwardReasonID"] as long?; // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/222494520/comments

                LaData.AppData.FKINCallMonitoringCancellationReasonID = dtLead.Rows[0]["FKINCallMonitoringCancellationReasonID"] as long?;

                LaData.AppData.DiaryReasonID = dtLead.Rows[0]["DiaryReasonID"] as long?;
                LaData.AppData.FutureContactDate = dtLead.Rows[0]["FutureContactDate"] as DateTime?;
                LaData.AppData.LoadedAllocationDate = dtLead.Rows[0]["AllocationDate"] as DateTime?;

                LaData.AppData.PlatinumBatchCode = dtSale.Rows[0]["BatchCode"] as string;
                LaData.AppData.UDMBatchCode = dtSale.Rows[0]["UDMBatchCode"] as string;
                LaData.AppData.CampaignCode = dtSale.Rows[0]["CampaignCode"] as string;
                LaData.AppData.CampaignID = dtSale.Rows[0]["CampaignID"] as long?;
                LaData.AppData.IsConfirmed = Convert.ToBoolean(dtSale.Rows[0]["IsConfirmed"] as bool?);

                if (LaData.AppData.CampaignID == 344)
                {
                    lblCancerQuestionOne.Visibility = Visibility.Visible;
                    lblCancerQuestionTwo.Visibility = Visibility.Visible;
                    chkQuestionOne.Visibility = Visibility.Visible;
                    chkQuestionTwo.Visibility = Visibility.Visible;
                }
                else
                {
                    lblCancerQuestionOne.Visibility = Visibility.Collapsed;
                    lblCancerQuestionTwo.Visibility = Visibility.Collapsed;
                    chkQuestionOne.Visibility = Visibility.Collapsed;
                    chkQuestionTwo.Visibility = Visibility.Collapsed;
                }



                try
                {
                    StringBuilder strQueryCancerQuestion = new StringBuilder();
                    strQueryCancerQuestion.Append("SELECT [QuestionOne], [QuestionTwo] ");
                    strQueryCancerQuestion.Append("FROM [CancerQuestion] ");
                    strQueryCancerQuestion.Append("WHERE FKINImportID = " + importID);
                    //strQuery.Append(" ORDER BY ID ASC");    
                    DataTable dtCancerQuestion = Methods.GetTableData(strQueryCancerQuestion.ToString());

                    if (dtCancerQuestion.Rows.Count > 0 && LaData.AppData.CampaignID == 344)
                    //if (dtCancerQuestion.Rows.Count > 0)
                    {
                        bool? questionOne = (bool)dtCancerQuestion.Rows[0]["QuestionOne"];
                        bool? questionTwo = (bool)dtCancerQuestion.Rows[0]["QuestionTwo"];
                        if (questionOne == true && questionTwo == false)
                        {
                            chkQuestionOne.IsChecked = true;
                            chkQuestionTwo.IsChecked = false;
                        }
                        else if (questionOne == false && questionTwo == true)
                        {
                            chkQuestionOne.IsChecked = false;
                            chkQuestionTwo.IsChecked = true;
                        }
                        else
                        {
                            INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                            ShowMessageBox(messageWindow, "This client has already been saved.", "Record already exists", ShowMessageType.Exclamation);
                        }
                    }
                }
                catch (Exception ex)
                {
                }


                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/219494758/comments
                if (dtLeadHst.Rows.Count > 0)
                {
                    LaData.LeadHistoryData.IsConfirmed = dtLeadHst.Rows[0]["IsConfirmed"] as bool?;
                }

                LaData.AppData.IsMining = Convert.ToBoolean(dtSale.Rows[0]["IsMining"] as bool?);
                LaData.AppData.IsCopied = Convert.ToBoolean(dtLead.Rows[0]["IsCopied"] as bool?);

                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/220876019/comments
                LaData.AppData.CanManageCallMonitoringDetails = Convert.ToBoolean(dtLead.Rows[0]["CanManageCallMonitoringDetails"]);

                LaData.AppData.CanManageQADetails = Convert.ToBoolean(dtLead.Rows[0]["CanManageQADetails"]);

                LaData.AppData.CanChangeStatus = Convert.ToBoolean(dtLead.Rows[0]["CanChangeStatus"]);


                #region Show the remaining sales for a particular agent and batch

                DisplayRemainingSales(importID);

                #endregion Show the remaining sales for a particular agent and batch

                if (LaData.AppData.LeadStatus != null)
                {
                    if ((lkpINLeadStatus)LaData.AppData.LeadStatus == lkpINLeadStatus.Declined)//brigette request 2015/07/08
                    {
                        lblDateOfSale.Text = "Date Of Decline";
                        lblSaleDate.Text = "Decline Date / Time";
                    }
                }

                cmbStatus_ToolTip(importID);



                #endregion

                #region (Page 1) Lead Data

                if (dtLead.Rows.Count > 0)
                {
                    LaData.LeadData.LeadID = dtLead.Rows[0]["LeadID"] as long?;

                    LaData.LeadData.TitleID = LaData.LeadHistoryData.TitleID = dtLead.Rows[0]["TitleID"] as long?;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.TitleID = dtLeadHst.Rows[0]["FKINTitleID"] as long?;

                    LaData.LeadData.Name = LaData.LeadHistoryData.Name = dtLead.Rows[0]["FirstName"] as string;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.Name = dtLeadHst.Rows[0]["FirstName"] as string;

                    LaData.LeadData.Initials = LaData.LeadHistoryData.Initials = dtLead.Rows[0]["Initials"] as string;
                    LaData.LeadData.Initials = Methods.ExtractInitialsFromFirstName(LaData.LeadData.Name);
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.Initials = dtLeadHst.Rows[0]["Initials"] as string;

                    LaData.LeadData.Surname = LaData.LeadHistoryData.Surname = dtLead.Rows[0]["Surname"] as string;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.Surname = dtLeadHst.Rows[0]["Surname"] as string;

                    LaData.LeadData.DateOfBirth = LaData.LeadHistoryData.DateOfBirth = dtLead.Rows[0]["DateOfBirth"] as DateTime?;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.DateOfBirth = dtLeadHst.Rows[0]["DateOfBirth"] as DateTime?;

                    LaData.LeadData.IDNumber = LaData.LeadHistoryData.IDNumber = dtLead.Rows[0]["IDNo"] as string;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.IDNumber = dtLeadHst.Rows[0]["IDNo"] as string;

                    LaData.LeadData.PassportNumber = LaData.LeadHistoryData.PassportNumber = dtLead.Rows[0]["PassportNo"] as string;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.PassportNumber = dtLeadHst.Rows[0]["PassportNo"] as string;

                    LaData.LeadData.GenderID = LaData.LeadHistoryData.GenderID = dtLead.Rows[0]["GenderID"] as long?;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.GenderID = dtLeadHst.Rows[0]["FKGenderID"] as long?;

                    LaData.LeadData.TelWork = LaData.LeadHistoryData.TelWork = dtLead.Rows[0]["TelWork"] as string;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.TelWork = dtLeadHst.Rows[0]["TelWork"] as string;
                    SetDialHoursGraph(stpWorkPhone, LaData.LeadData.TelWork);

                    LaData.LeadData.TelHome = LaData.LeadHistoryData.TelHome = dtLead.Rows[0]["TelHome"] as string;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.TelHome = dtLeadHst.Rows[0]["TelHome"] as string;
                    SetDialHoursGraph(stpHomePhone, LaData.LeadData.TelHome);

                    LaData.LeadData.TelCell = LaData.LeadHistoryData.TelCell = dtLead.Rows[0]["TelCell"] as string;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.TelCell = dtLeadHst.Rows[0]["TelCell"] as string;
                    SetDialHoursGraph(stpCellPhone, LaData.LeadData.TelCell);

                    LaData.LeadData.TelOther = LaData.LeadHistoryData.TelOther = dtLead.Rows[0]["TelOther"] as string;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.TelOther = dtLeadHst.Rows[0]["TelOther"] as string;
                    SetDialHoursGraph(stpOtherPhone, LaData.LeadData.TelOther);

                    LaData.LeadData.Address1 = LaData.LeadHistoryData.Address1 = dtLead.Rows[0]["Address1"] as string;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.Address1 = dtLeadHst.Rows[0]["Address1"] as string;

                    LaData.LeadData.Address2 = LaData.LeadHistoryData.Address2 = dtLead.Rows[0]["Address2"] as string;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.Address2 = dtLeadHst.Rows[0]["Address2"] as string;

                    LaData.LeadData.Address3 = LaData.LeadHistoryData.Address3 = dtLead.Rows[0]["Address3"] as string;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.Address3 = dtLeadHst.Rows[0]["Address3"] as string;

                    LaData.LeadData.Address4 = LaData.LeadHistoryData.Address4 = dtLead.Rows[0]["Address4"] as string;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.Address4 = dtLeadHst.Rows[0]["Address4"] as string;

                    LaData.LeadData.Address5 = LaData.LeadHistoryData.Address5 = dtLead.Rows[0]["Address5"] as string;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.Address5 = dtLeadHst.Rows[0]["Address5"] as string;

                    LaData.LeadData.PostalCode = LaData.LeadHistoryData.PostalCode = dtLead.Rows[0]["PostalCode"] as string;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.PostalCode = dtLeadHst.Rows[0]["PostalCode"] as string;

                    #region Address Mappings - removed, as requested by Brigette, 2015-03-24
                    ////to date only Address fields 1 to 4 are supplied in the Platinum import sheets

                    //medAddress1.Value = dtLead.Rows[0]["Address1"];

                    //if(!string.IsNullOrWhiteSpace(dtLead.Rows[0]["Address5"] as string))
                    //{
                    //    ecbTown.Text = dtLead.Rows[0]["Address5"] as string;

                    //    {
                    //        string strSuburb = dtLead.Rows[0]["Address4"] as string;

                    //        if (!string.IsNullOrWhiteSpace(strSuburb))
                    //        {
                    //            parameters = new SqlParameter[1];
                    //            parameters[0] = new SqlParameter("@Keyword", strSuburb.Substring(0, 1));

                    //            ds = Methods.ExecuteStoredProcedure("Blush.dbo.spGetPostalCodeList", parameters);
                    //            DataTable dt = ds.Tables[0];

                    //            ecbSuburb.Populate(dt, "Suburb", IDField);
                    //            ecbSuburb.Text = strSuburb;
                    //        }
                    //    }

                    //    if (!string.IsNullOrWhiteSpace(dtLead.Rows[0]["Address3"] as string))
                    //    {
                    //        if (!string.IsNullOrWhiteSpace(dtLead.Rows[0]["Address2"] as string))
                    //        {
                    //            medAddress2.Value = dtLead.Rows[0]["Address2"] as string + ", " + dtLead.Rows[0]["Address3"];
                    //        }
                    //        else
                    //        {
                    //            medAddress2.Value = dtLead.Rows[0]["Address3"] as string;
                    //        }
                    //    }
                    //    else if (!string.IsNullOrWhiteSpace(dtLead.Rows[0]["Address2"] as string))
                    //    {
                    //        medAddress2.Value = dtLead.Rows[0]["Address2"] as string;
                    //    }
                    //}
                    //else if (!string.IsNullOrWhiteSpace(dtLead.Rows[0]["Address4"] as string))
                    //{
                    //    ecbTown.Text = dtLead.Rows[0]["Address4"] as string;

                    //    {
                    //        string strSuburb = dtLead.Rows[0]["Address3"] as string;

                    //        if (!string.IsNullOrWhiteSpace(strSuburb))
                    //        {
                    //            parameters = new SqlParameter[1];
                    //            parameters[0] = new SqlParameter("@Keyword", strSuburb.Substring(0, 1));

                    //            ds = Methods.ExecuteStoredProcedure("Blush.dbo.spGetPostalCodeList", parameters);
                    //            DataTable dt = ds.Tables[0];

                    //            ecbSuburb.Populate(dt, "Suburb", IDField);
                    //            ecbSuburb.Text = strSuburb;
                    //        }
                    //    }

                    //    medAddress2.Value = dtLead.Rows[0]["Address2"] as string;
                    //}
                    //else if (!string.IsNullOrWhiteSpace(dtLead.Rows[0]["Address3"] as string))
                    //{
                    //    ecbTown.Text = dtLead.Rows[0]["Address3"] as string;

                    //    {
                    //        string strSuburb = dtLead.Rows[0]["Address2"] as string;

                    //        if (!string.IsNullOrWhiteSpace(strSuburb))
                    //        {
                    //            parameters = new SqlParameter[1];
                    //            parameters[0] = new SqlParameter("@Keyword", strSuburb.Substring(0, 1));

                    //            ds = Methods.ExecuteStoredProcedure("Blush.dbo.spGetPostalCodeList", parameters);
                    //            DataTable dt = ds.Tables[0];

                    //            ecbSuburb.Populate(dt, "Suburb", IDField);
                    //            ecbSuburb.Text = strSuburb;
                    //        }
                    //    }
                    //}
                    //else if (!string.IsNullOrWhiteSpace(dtLead.Rows[0]["Address2"] as string))
                    //{
                    //    ecbTown.Text = dtLead.Rows[0]["Address2"] as string;
                    //}

                    //medPostalCode.Value = dtLead.Rows[0]["PostalCode"];
                    #endregion Address Mappings - removed, as requested by Brigette, 2015-03-24

                    #region Address mappings

                    //List<string> addressLines = new List<string>(5);

                    //if (!string.IsNullOrWhiteSpace(dtLead.Rows[0]["Address1"] as string))
                    //{
                    //    addressLines.Add(dtLead.Rows[0]["Address1"] as string);
                    //}

                    //if (!string.IsNullOrWhiteSpace(dtLead.Rows[0]["Address2"] as string))
                    //{
                    //    addressLines.Add(dtLead.Rows[0]["Address2"] as string);
                    //}

                    //if (!string.IsNullOrWhiteSpace(dtLead.Rows[0]["Address3"] as string))
                    //{
                    //    addressLines.Add(dtLead.Rows[0]["Address3"] as string);
                    //}

                    //if (!string.IsNullOrWhiteSpace(dtLead.Rows[0]["Address4"] as string))
                    //{
                    //    addressLines.Add(dtLead.Rows[0]["Address4"] as string);
                    //}

                    //if (!string.IsNullOrWhiteSpace(dtLead.Rows[0]["Address5"] as string))
                    //{
                    //    addressLines.Add(dtLead.Rows[0]["Address5"] as string);
                    //}

                    //int nullAddressLinesToAdd = addressLines.Capacity - addressLines.Count();

                    //for (int a = 0; a < nullAddressLinesToAdd; a++)
                    //{
                    //    addressLines.Add(null);
                    //}

                    ////medAddress1.Value = addressLines[0];
                    //medAddress2.Value = addressLines[1];
                    //medAddress3.Value = addressLines[2];
                    //medSuburb.Value = addressLines[3];
                    //medTown.Value = addressLines[4];

                    //medPostalCode.Value = dtLead.Rows[0]["PostalCode"];

                    #endregion Address mappings

                    LaData.LeadData.ReferrorTitleID = dtLead.Rows[0]["ReferrorTitleID"] as long?;

                    LaData.LeadData.ReferrorName = dtLead.Rows[0]["ReferrorName"] as string;

                    LaData.LeadData.ReferrorRelationshipID = dtLead.Rows[0]["ReferrorRelationshipID"] as long?;

                    //LaData.LeadData.ReferrorType = dtLead.Rows[0]["ReferrorType"] as string;

                    LaData.LeadData.Email = LaData.LeadHistoryData.Email = dtLead.Rows[0]["EMail"] as string;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.Email = dtLeadHst.Rows[0]["EMail"] as string;


                }

                #endregion

                #region (Page 2) Policy Data

                if (dtImportedPolicyData.Rows.Count > 0)
                {
                    LaData.ImportedPolicyData.LapseDate = dtImportedPolicyData.Rows[0]["LapseDate"] as DateTime?;
                    LaData.ImportedPolicyData.CommenceDate = dtImportedPolicyData.Rows[0]["CommenceDate"] as DateTime?;
                    LaData.ImportedPolicyData.MoneyBackDate = dtImportedPolicyData.Rows[0]["MoneyBackDate"] as DateTime?;
                }

                if (dtPolicy.Rows.Count > 0)
                {
                    string strQuery;
                    DataTable dt;

                    LaData.PolicyData.PolicyID = dtPolicy.Rows[0]["PolicyID"] as long?;

                    LaData.PolicyData.CampaignTypeID = dtPolicy.Rows[0]["CampaignTypeID"] as long?;

                    LaData.AppData.CampaignType = (lkpINCampaignType?)LaData.PolicyData.CampaignTypeID;
                    LaData.PolicyData.CampaignGroupID = dtPolicy.Rows[0]["CampaignGroupID"] as long?;
                    LaData.AppData.CampaignGroup = (lkpINCampaignGroup?)LaData.PolicyData.CampaignGroupID;

                    LaData.PolicyData.CommenceDate = LaData.PolicyHistoryData.CommenceDate = dtPolicy.Rows[0]["CommenceDate"] as DateTime?;

                    try { LaData.PolicyData.MoneyBackDate = LaData.PolicyHistoryData.MoneyBackDate = dtImportedPolicyData.Rows[0]["MoneyBackDate"] as DateTime?; } catch { }

                    try { LaData.PolicyData.MoneyBackDate2 = LaData.PolicyHistoryData.MoneyBackDate = dtImportedPolicyData.Rows[0]["ConversionMBDate"] as DateTime?; } catch { }

                    parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@ImportID", importID);
                    object obj = Methods.ExecuteFunction("GetOriginalCommenceDate", parameters);

                    DateTime? originalCommenceDate = null;
                    if (obj != DBNull.Value) { originalCommenceDate = Convert.ToDateTime(obj); }
                    if (originalCommenceDate != null)
                    {
                        dteCommenceDate.ToolTip = "Original Commence Date: " + new string(originalCommenceDate.ToString().Take(10).ToArray());
                    }
                    else
                    {
                        dteCommenceDate.ToolTip = null;
                    }

                    if (!Convert.IsDBNull(dtSale.Rows[0]["CampaignGroupID"]) &&
                        (LaData.AppData.LeadStatus == null || LaData.AppData.LeadStatus != 1) &&
                         (Convert.ToInt64(dtSale.Rows[0]["CampaignGroupID"]) == (long)lkpINCampaignGroup.ReDefrost ||
                          Convert.ToInt64(dtSale.Rows[0]["CampaignGroupID"]) == (long)lkpINCampaignGroup.Defrosted ||
                          Convert.ToInt64(dtSale.Rows[0]["CampaignGroupID"]) == (long)lkpINCampaignGroup.Rejuvenation ||
                          Convert.ToInt64(dtSale.Rows[0]["CampaignGroupID"]) == (long)lkpINCampaignGroup.DefrostR99 ||
                          Convert.ToInt64(dtSale.Rows[0]["CampaignGroupID"]) == (long)lkpINCampaignGroup.Lite ||
                          Convert.ToInt64(dtSale.Rows[0]["CampaignGroupID"]) == (long)lkpINCampaignGroup.SpouseLite ||
                          Convert.ToInt64(dtSale.Rows[0]["CampaignGroupID"]) == (long)lkpINCampaignGroup.Reactivation))
                    {
                        //CommenceDate is set to null here because it should be like a normal base campaign in which the commencedate is null to start with
                        //even though a commencedate is sent through by Platinum for these campaigns it should not be used
                        LaData.PolicyData.CommenceDate = null;
                    }
                    if ((LaData.AppData.LeadStatus == null || LaData.AppData.LeadStatus != 1) && !_upgrades.Contains(LaData.AppData.CampaignGroup))
                    {
                        //Sets commencedate if it is null here
                        LaData.PolicyData.CommenceDate = LaData.PolicyData.CommenceDate ?? new DateTime(DateTime.Today.AddMonths(2).Year, DateTime.Today.AddMonths(2).Month, 1);
                    }

                    LaData.PolicyData.PolicyNumber = dtPolicy.Rows[0]["PlatinumPolicyID"] as string;
                    LaData.PolicyData.LeadAge = dtPolicy.Rows[0]["LeadAge"] as short?;

                    //Calculate CachBack age
                    if (_upgrades.Contains(LaData.AppData.CampaignGroup))
                    {

                        if (originalCommenceDate != null && LaData.LeadData.DateOfBirth != null && LaData.PolicyData.MoneyBackDate2 != null)
                        {
                            parameters = new SqlParameter[3];
                            parameters[0] = new SqlParameter("@DOB", LaData.LeadData.DateOfBirth);
                            parameters[1] = new SqlParameter("@CommenceDate", originalCommenceDate);
                            parameters[2] = new SqlParameter("@ConversionMBDate", LaData.PolicyData.MoneyBackDate2);
                            //LaData.PolicyData.CashBackAge = Convert.ToInt16(Methods.ExecuteFunction("GetCashBackAge2", parameters));
                            LaData.PolicyData.CashBackAge2 = Convert.ToInt16(Methods.ExecuteFunction("GetUpgradeCashBackAge2", parameters));

                            parameters = new SqlParameter[2];
                            parameters[0] = new SqlParameter("@DOB", LaData.LeadData.DateOfBirth);
                            parameters[1] = new SqlParameter("@CommenceDate", originalCommenceDate);
                            LaData.PolicyData.CashBackAge = Convert.ToInt16(Methods.ExecuteFunction("GetCashBackAge2", parameters));

                        }
                        else if (originalCommenceDate != null && LaData.LeadData.DateOfBirth != null && LaData.PolicyData.MoneyBackDate2 == null)
                        {
                            parameters = new SqlParameter[2];
                            parameters[0] = new SqlParameter("@DOB", LaData.LeadData.DateOfBirth);
                            parameters[1] = new SqlParameter("@CommenceDate", originalCommenceDate);
                            LaData.PolicyData.CashBackAge = Convert.ToInt16(Methods.ExecuteFunction("GetCashBackAge2", parameters));

                        }
                    }
                    else if (LaData.PolicyData.CommenceDate != null && LaData.LeadData.DateOfBirth != null)
                    {


                        parameters = new SqlParameter[2];
                        parameters[0] = new SqlParameter("@DOB", LaData.LeadData.DateOfBirth);
                        parameters[1] = new SqlParameter("@CommenceDate", LaData.PolicyData.CommenceDate);
                        LaData.PolicyData.CashBackAge = Convert.ToInt16(Methods.ExecuteFunction("GetCashBackAge2", parameters));


                    }



                    //upgrade campaigns will alow saving to occur when ID flags as per Brigette 2015/07/02
                    if (_upgrades.Contains(LaData.AppData.CampaignGroup))
                    {
                        IsUpgradeCampaign.IsChecked = true;
                    }

                    LaData.PolicyData.PlatinumPlan = dtPolicy.Rows[0]["CancerOption"] as string;
                    if (campaignTypesMaccNotAccDis.Contains(LaData.AppData.CampaignType))
                    {
                        if (!(LaData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                            &&
                            (LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8)
                            ))
                        {
                            LaData.PolicyData.PlatinumPlan = "1";
                        }
                    }
                    else if (LaData.AppData.CampaignType == lkpINCampaignType.AccDis)
                    {
                        //this should be either 1 or 2 as imported!
                        //check ref number for macc and million lead
                        string checkRef = LaData.AppData.RefNo;
                        //gdna - macc
                        //gdnm - million
                        var refNumberCode = checkRef.Substring(0, 4);
                        if (refNumberCode.ToLower() == "gdna")//macc
                        {
                            LaData.PolicyData.PlatinumPlan = "2";
                        }
                        if (refNumberCode.ToLower() == "gdnm")//million
                        {
                            LaData.PolicyData.PlatinumPlan = "1";
                        }
                    }
                    else if (LaData.AppData.CampaignType == lkpINCampaignType.Macc)
                    {
                        LaData.PolicyData.PlatinumPlan = "2";
                    }
                    else if (LaData.AppData.CampaignType == lkpINCampaignType.MaccMillion)
                    {
                        LaData.PolicyData.PlatinumPlan = "1";
                    }


                    LaData.AppData.IsLeadUpgrade = _upgrades.Contains(LaData.AppData.CampaignGroup);

                    // Additional checking for the Do Not Contact Client status
                    if (LaData.AppData.IsLeadUpgrade)
                    {
                        LoadUpgradeLead();

                        //if ((lkpINLeadStatus?)LaData.AppData.LeadStatus == lkpINLeadStatus.DoNotContactClient)
                        //{
                        //    if ((lkpUserType?)((User)GlobalSettings.ApplicationUser).FKUserType != lkpUserType.SalesAgent)
                        //    {
                        //        LoadUpgradeLead();
                        //    }
                        //}
                        //else
                        //{
                        //    LoadUpgradeLead();
                        //}
                    }


                    LaData.PolicyData.PlanID = dtPolicy.Rows[0]["PolicyPlanID"] as long?;
                    LaData.PolicyData.OptionID = dtPolicy.Rows[0]["OptionID"] as long?;
                    LaData.PolicyData.LA1Cover = dtPolicy.Rows[0]["LA1Cover"] as decimal?;

                    LaData.PolicyData.OptionFeesID = dtPolicy.Rows[0]["OptionFeesID"] as long?;

                    //CanSave.IsChecked = true;
                    //if (LaData.AppData.CampaignGroup == lkpINCampaignGroup.Base)
                    //{
                    //    //check base campaign for ref
                    //    DataTable baseData = Methods.GetTableData("select * FROM dbo.INCampaign INNER JOIN dbo.lkpINCampaignGroup ON dbo.INCampaign.FKINCampaignGroupID = dbo.lkpINCampaignGroup.ID " +
                    //        "INNER JOIN dbo.INImport ON dbo.INCampaign.ID = dbo.INImport.FKINCampaignID WHERE (dbo.INImport.RefNo = '" + LaData.AppData.RefNo + "') AND (dbo.INCampaign.FKINCampaignGroupID = 22) ");
                    //    if (baseData.Rows.Count > 0)
                    //    {
                    //        if (LaData.UserData.UserType != lkpUserType.Administrator && LaData.UserData.UserType != lkpUserType.Manager && LaData.UserData.UserType != lkpUserType.SeniorAdministrator)
                    //        {
                    //            CanSave.IsChecked = false;
                    //        }
                    //        else
                    //        {
                    //            CanSave.IsChecked = true;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    CanSave.IsChecked = true;
                    //}

                    if (LaData.AppData.CampaignGroup == lkpINCampaignGroup.Extension)
                    {
                        LoadExtensionLead();
                        //if ((lkpINLeadStatus?)LaData.AppData.LeadStatus == lkpINLeadStatus.DoNotContactClient)
                        //{
                        //    if ((lkpUserType?)((User)GlobalSettings.ApplicationUser).FKUserType != lkpUserType.SalesAgent)
                        //    {
                        //        LoadExtensionLead();
                        //    }
                        //}
                        //else
                        //{
                        //    LoadExtensionLead();
                        //}
                    }

                    #region Load Pricing Groups

                    {
                        LaData.PolicyData.PlanGroupID = dtPolicy.Rows[0]["PolicyPlanGroupID"] as long?;

                        strQuery = "SELECT DISTINCT ID, Description FROM INPlanGroup WHERE ";
                        strQuery += "FKINCampaignTypeID = '" + LaData.PolicyData.CampaignTypeID;
                        strQuery += "' AND FKINCampaignGroupID = '" + LaData.PolicyData.CampaignGroupID;
                        strQuery += "' AND IsActive = '1' ";
                        strQuery += " OR INPlanGroup.ID = '" + LaData.PolicyData.PlanGroupID;
                        strQuery += "' ORDER BY ID DESC";


                        DataTable dtPolicyPlanGroup = Methods.GetTableData(strQuery);
                        cmbPolicyPlanGroup.Populate(dtPolicyPlanGroup, DescriptionField, IDField);

                        if (LaData.PolicyData.PlanGroupID == null)
                        {
                            if (dtPolicyPlanGroup.Rows.Count > 0)
                            {
                                LaData.PolicyData.PlanGroupID = dtPolicyPlanGroup.Rows[0]["ID"] as long?;
                            }
                        }
                    }

                    #endregion

                    LaData.PolicyData.IsChildChecked = Convert.ToBoolean(dtPolicy.Rows[0]["OptionChild"] as bool?);
                    LaData.PolicyData.IsChildUpgradeChecked = Convert.ToBoolean(dtPolicy.Rows[0]["OptionChild"] as bool?);

                    LaData.PolicyData.IsLA2Checked = Convert.ToBoolean(dtPolicy.Rows[0]["OptionLA2"] as bool?);
                    //Check LA2 settings for loaded option
                    if (!LaData.AppData.IsLeadUpgrade && LaData.PolicyData.PlanID != null && LaData.PolicyData.LA1Cover > 0.00m)
                    {
                        strQuery = "SELECT LA2Cover FROM INOption WHERE FKINPlanID = '" + LaData.PolicyData.PlanID + "' AND LA1Cover = '" + LaData.PolicyData.LA1Cover + "' AND IsActive = '1'" + "AND LA2Cover != 0";
                        dt = Methods.GetTableData(strQuery);
                        if (dt.Rows.Count == 0)
                        {
                            LaData.PolicyData.IsLA2Checked = false;
                            chkLA2.IsEnabled = false;
                        }

                        strQuery = "SELECT LA2Cover FROM INOption WHERE FKINPlanID = '" + LaData.PolicyData.PlanID + "' AND LA1Cover = '" + LaData.PolicyData.LA1Cover + "' AND IsActive = '1'" + "AND LA2Cover = 0";
                        dt = Methods.GetTableData(strQuery);
                        if (dt.Rows.Count == 0)
                        {
                            LaData.PolicyData.IsLA2Checked = true;
                            chkLA2.IsEnabled = false;
                        }
                    }

                    LaData.PolicyData.IsFuneralChecked = Convert.ToBoolean(dtPolicy.Rows[0]["OptionFuneral"] as bool?);

                    LaData.PolicyData.LoadedTotalPremium = dtPolicy.Rows[0]["TotalPremium"] as decimal?;

                    LaData.PolicyData.LoadedTotalInvoiceFee = dtPolicy.Rows[0]["TotalInvoiceFee"] as decimal?;


                    #region Bump-Ups and Reduced Premiums

                    {
                        bool udmBumpUpOption = Convert.ToBoolean(dtPolicy.Rows[0]["UDMBumpUpOption"] as bool?);
                        bool reducedPremiumOption = Convert.ToBoolean(dtPolicy.Rows[0]["ReducedPremiumOption"] as bool?);

                        if (udmBumpUpOption)
                        {
                            decimal? bumpUpAmount = dtPolicy.Rows[0]["BumpUpAmount"] as decimal?;

                            if (bumpUpAmount != null && bumpUpAmount > 0)
                            {
                                LaData.PolicyData.BumpUpAmount = bumpUpAmount;
                                LaData.PolicyData.UDMBumpUpOption = udmBumpUpOption;
                                xamCEBumpUpAmount.Visibility = Visibility.Visible;
                            }
                        }
                        else if (reducedPremiumOption)
                        {
                            decimal? reducedPremiumAmount = dtPolicy.Rows[0]["ReducedPremiumAmount"] as decimal?;

                            if (reducedPremiumAmount != null && reducedPremiumAmount > 0)
                            {
                                LaData.PolicyData.ReducedPremiumAmount = reducedPremiumAmount;
                                LaData.PolicyData.ReducedPremiumOption = reducedPremiumOption;
                                xamCEReducedPremiumAmount.Visibility = Visibility.Visible;
                            }
                        }
                    }

                    LaData.PolicyData.BumpUpOffered = Convert.ToBoolean(dtPolicy.Rows[0]["BumpUpOffered"]);
                    LaData.PolicyData.CanOfferBumpUp = Convert.ToBoolean(dtPolicy.Rows[0]["CanOfferBumpUp"]);

                    // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/220765495/comments
                    if (LaData.PolicyData.CanOfferBumpUp)
                    {
                        if (LaData.AppData.IsLeadUpgrade)
                        {
                            lblHasBumpUpBeenOfferedUpgrades.Visibility = Visibility.Visible;
                            chkHasBumpUpBeenOfferedUpgrades.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            lblHasBumpUpBeenOfferedBase.Visibility = Visibility.Visible;
                            chkHasBumpUpBeenOfferedBase.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        if (LaData.AppData.IsLeadUpgrade)
                        {
                            lblHasBumpUpBeenOfferedUpgrades.Visibility = Visibility.Collapsed;
                            chkHasBumpUpBeenOfferedUpgrades.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            lblHasBumpUpBeenOfferedBase.Visibility = Visibility.Collapsed;
                            chkHasBumpUpBeenOfferedBase.Visibility = Visibility.Collapsed;
                        }
                    }

                    #endregion Bump-Ups and Reduced Premiums

                }

                #endregion

                #region (Page 3) Bank Data

                if (dtBanking.Rows.Count > 0)
                {
                    LaData.BankDetailsData.BankDetailsID = dtBanking.Rows[0]["BankDetailsID"] as long?;

                    if (LaData.BankDetailsData.BankDetailsID > 0)
                    {
                        LaData.BankDetailsData.PaymentTypeID = dtBanking.Rows[0]["PaymentMethodID"] as long? ?? (long?)lkpPaymentType.DebitOrder;

                        LaData.BankDetailsData.TitleID = dtBanking.Rows[0]["AHTitleID"] as long?;
                        LaData.BankDetailsData.Initials = dtBanking.Rows[0]["AHInitials"] as string;
                        LaData.BankDetailsData.Name = dtBanking.Rows[0]["AHFirstName"] as string;
                        LaData.BankDetailsData.Surname = dtBanking.Rows[0]["AHSurname"] as string;
                        LaData.BankDetailsData.IDNumber = dtBanking.Rows[0]["AHIDNo"] as string;
                        LaData.BankDetailsData.TelHome = dtBanking.Rows[0]["HomePhone"] as string;
                        LaData.BankDetailsData.TelCell = dtBanking.Rows[0]["CellPhone"] as string;
                        LaData.BankDetailsData.TelWork = dtBanking.Rows[0]["WorkPhone"] as string;

                        LaData.BankDetailsData.SigningPowerID = dtBanking.Rows[0]["SigningPowerID"] as long?;

                        LaData.BankDetailsData.AccountHolder = dtBanking.Rows[0]["AccountHolder"] as string;
                        LaData.BankDetailsData.BankID = dtBanking.Rows[0]["BankID"] as long?;
                        LaData.BankDetailsData.BankBranchCodeID = dtBanking.Rows[0]["BankBranchID"] as long?;
                        LaData.BankDetailsData.AccountNumber = dtBanking.Rows[0]["AccountNo"] as string;
                        LaData.BankDetailsData.AccountTypeID = dtBanking.Rows[0]["AccountTypeID"] as long?;
                        LaData.BankDetailsData.DebitDay = dtBanking.Rows[0]["DebitDay"] as short?;

                        LaData.BankDetailsData.AccNumCheckStatusID = dtBanking.Rows[0]["AccNumCheckStatus"] as byte? ?? (byte?)lkpINAccNumCheckStatus.UnChecked;
                        LaData.BankDetailsData.lkpAccNumCheckStatus = (lkpINAccNumCheckStatus?)LaData.BankDetailsData.AccNumCheckStatusID;

                        LaData.BankDetailsData.AccNumCheckMsg = dtBanking.Rows[0]["AccNumCheckMsg"] as string;
                        LaData.BankDetailsData.AccNumCheckMsgFull = dtBanking.Rows[0]["AccNumCheckMsgFull"] as string;
                    }
                    else
                    {
                        LaData.BankDetailsData.PaymentTypeID = (long?)lkpPaymentType.DebitOrder;
                    }
                }

                #endregion

                #region (Page 4) LA, Children and Beneficiaries

                #region LA

                {
                    if (dtLA.Rows.Count > 0)
                    {
                        for (int i = 0; i < LeadApplicationData.MaxLA; i++)
                        {
                            DataTable dt = null;
                            DataTable dtHst = null;
                            DataRow[] dr = dtLA.Select("LifeAssuredRank = " + (i + 1));
                            DataRow[] drHst = dtLAHst.Select("LARank = " + (i + 1));
                            if (dr.Length > 0) { dt = dr.CopyToDataTable(); }
                            if (drHst.Length > 0) { dtHst = drHst.CopyToDataTable(); }

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                LeadApplicationData.LA LAData = LaData.LAData[i];
                                LeadApplicationData.LA LAHistoryData = LaData.LAHistoryData[i];

                                LAData.LifeAssuredID = LAHistoryData.LifeAssuredID = dt.Rows[0]["LifeAssuredID"] as long?;
                                LAData.PolicyLifeAssuredID = LAHistoryData.PolicyLifeAssuredID = dt.Rows[0]["PolicyLifeAssuredID"] as long?;

                                LAData.TitleID = LAHistoryData.TitleID = dt.Rows[0]["LATitleID"] as long?;
                                if (dtHst != null && dtHst.Rows.Count > 0) LAHistoryData.TitleID = dtHst.Rows[0]["FKINTitleID"] as long?;
                                if (LaData.AppData.IsLeadUpgrade && i == 1 && LAHistoryData.TitleID != null)
                                    cmbLA2TitleUpg.IsEnabled = false;

                                LAData.GenderID = LAHistoryData.GenderID = dt.Rows[0]["LAGenderID"] as long?;
                                if (dtHst != null && dtHst.Rows.Count > 0) LAHistoryData.GenderID = dtHst.Rows[0]["FKGenderID"] as long?;
                                if (LaData.AppData.IsLeadUpgrade && i == 1 && LAHistoryData.GenderID != null)
                                    cmbLA2GenderUpg.IsEnabled = false;

                                LAData.Name = LAHistoryData.Name = dt.Rows[0]["LAFirstName"] as string;
                                if (dtHst != null && dtHst.Rows.Count > 0) LAHistoryData.Name = dtHst.Rows[0]["FirstName"] as string;
                                if (LaData.AppData.IsLeadUpgrade && i == 1 && !string.IsNullOrWhiteSpace(LAHistoryData.Name))
                                    medLA2NameUpg.IsEnabled = false;

                                LAData.Surname = LAHistoryData.Surname = dt.Rows[0]["LASurname"] as string;
                                if (dtHst != null && dtHst.Rows.Count > 0) LAHistoryData.Surname = dtHst.Rows[0]["Surname"] as string;
                                if (LaData.AppData.IsLeadUpgrade && i == 1 && !string.IsNullOrWhiteSpace(LAHistoryData.Surname))
                                    medLA2SurnameUpg.IsEnabled = false;

                                LAData.DateOfBirth = LAHistoryData.DateOfBirth = dt.Rows[0]["LADateOfBirth"] as DateTime?;
                                if (dtHst != null && dtHst.Rows.Count > 0) LAHistoryData.DateOfBirth = dtHst.Rows[0]["DateOfBirth"] as DateTime?;
                                if (LaData.AppData.IsLeadUpgrade && i == 1 && LAHistoryData.DateOfBirth != null)
                                    dteLA2DateOfBirthUpg.IsEnabled = false;

                                LAData.IDNumber = LAHistoryData.IDNumber = dt.Rows[0]["LAIDNo"] as string;
                                if (dtHst != null && dtHst.Rows.Count > 0) LAHistoryData.IDNumber = dtHst.Rows[0]["IDNo"] as string;

                                LAData.RelationshipID = LAHistoryData.RelationshipID = dt.Rows[0]["LARelationshipID"] as long?;
                                if (dtHst != null && dtHst.Rows.Count > 0) LAHistoryData.RelationshipID = dtHst.Rows[0]["FKINRelationshipID"] as long?;
                                if (LaData.AppData.IsLeadUpgrade && i == 1 && LAHistoryData.RelationshipID != null)
                                    cmbLA2RelationshipUpg.IsEnabled = false;

                                LAData.TelContact = LAHistoryData.TelContact = dt.Rows[0]["LATelContact"] as string;
                                if (dtHst != null && dtHst.Rows.Count > 0) LAHistoryData.TelContact = dtHst.Rows[0]["TelContact"] as string;
                                //if (LaData.AppData.IsLeadUpgrade && i == 1 && !string.IsNullOrWhiteSpace(LAHistoryData.TelContact))
                                //    medLA2ContactPhoneUpg.IsEnabled = false;

                            }
                        }
                    }

                    cmbLifeAssured.SelectedIndex = 1;
                }

                #endregion

                #region Beneficiaries

                {
                    for (int i = 0; i < LeadApplicationData.MaxBeneficiaries; i++)
                    {
                        DataTable dt = null;
                        DataTable dtHst = null;
                        DataRow[] dr = dtBeneficiary.Select("BeneficiaryRank = " + (i + 1));
                        DataRow[] drHst = dtBeneficiaryHst.Select("BeneficiaryRank = " + (i + 1));
                        if (dr.Length > 0) { dt = dr.CopyToDataTable(); }
                        if (drHst.Length > 0) { dtHst = drHst.CopyToDataTable(); }

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            LeadApplicationData.Beneficiary BeneficiaryData = LaData.BeneficiaryData[i];
                            LeadApplicationData.Beneficiary BeneficiaryHistoryData = LaData.BeneficiaryHistoryData[i];

                            BeneficiaryData.BeneficiaryID = BeneficiaryHistoryData.BeneficiaryID = dt.Rows[0]["BeneficiaryID"] as long?;
                            BeneficiaryData.PolicyBeneficiaryID = BeneficiaryHistoryData.PolicyBeneficiaryID = dt.Rows[0]["PolicyBeneficiaryID"] as long?;

                            BeneficiaryData.TitleID = BeneficiaryHistoryData.TitleID = dt.Rows[0]["BeneficiaryTitleID"] as long?;
                            if (dtHst != null && dtHst.Rows.Count > 0) BeneficiaryHistoryData.TitleID = dtHst.Rows[0]["FKINTitleID"] as long?;

                            BeneficiaryData.GenderID = BeneficiaryHistoryData.GenderID = dt.Rows[0]["BeneficiaryGenderID"] as long?;
                            if (dtHst != null && dtHst.Rows.Count > 0) BeneficiaryHistoryData.GenderID = dtHst.Rows[0]["FKGenderID"] as long?;

                            BeneficiaryData.Name = BeneficiaryHistoryData.Name = dt.Rows[0]["BeneficiaryFirstName"] as string;
                            if (dtHst != null && dtHst.Rows.Count > 0) BeneficiaryHistoryData.Name = dtHst.Rows[0]["FirstName"] as string;

                            BeneficiaryData.Surname = BeneficiaryHistoryData.Surname = dt.Rows[0]["BeneficiarySurname"] as string;
                            if (dtHst != null && dtHst.Rows.Count > 0) BeneficiaryHistoryData.Surname = dtHst.Rows[0]["Surname"] as string;

                            BeneficiaryData.DateOfBirth = BeneficiaryHistoryData.DateOfBirth = dt.Rows[0]["BeneficiaryDateOfBirth"] as DateTime?;
                            if (dtHst != null && dtHst.Rows.Count > 0) BeneficiaryHistoryData.DateOfBirth = dtHst.Rows[0]["DateOfBirth"] as DateTime?;

                            BeneficiaryData.RelationshipID = BeneficiaryHistoryData.RelationshipID = dt.Rows[0]["BeneficiaryRelationshipID"] as long?;
                            if (dtHst != null && dtHst.Rows.Count > 0) BeneficiaryHistoryData.RelationshipID = dtHst.Rows[0]["FKINRelationshipID"] as long?;

                            BeneficiaryData.Percentage = BeneficiaryHistoryData.Percentage = dt.Rows[0]["BeneficiaryPercentage"] as double?;
                            if (dtHst != null && dtHst.Rows.Count > 0) BeneficiaryHistoryData.Percentage = dtHst.Rows[0]["BeneficiaryPercentage"] as double?;

                            BeneficiaryData.Notes = BeneficiaryHistoryData.Notes = dt.Rows[0]["BeneficiaryNotes"] as string;

                            BeneficiaryData.TelContact = BeneficiaryHistoryData.TelContact = dt.Rows[0]["BeneficiaryTelContact"] as string;
                            if (dtHst != null && dtHst.Rows.Count > 0) BeneficiaryHistoryData.TelContact = dtHst.Rows[0]["TelContact"] as string;

                        }
                    }

                    LaData.AppData.SelectedBeneficiaryIndex = 0;
                }

                #endregion

                #region NextOfKin

                {
                    if (dtNextOfKin.Rows.Count > 0)
                    {
                        for (int i = 0; i < LeadApplicationData.MaxNextOfKin; i++)
                        {
                            DataTable dt = null;
                            DataTable dtHst = null;
                            DataRow[] dr = dtNextOfKin.Select("NOKID IS NOT NULL");
                            DataRow[] drHst = dtNextOfKinHst.Select("ID IS NOT NULL");
                            if (dr.Length > 0) { dt = dr.CopyToDataTable(); }
                            if (drHst.Length > 0) { dtHst = drHst.CopyToDataTable(); }

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                LeadApplicationData.NextOfKin NextOfKinData = LaData.NextOfKinData[i];
                                LeadApplicationData.NextOfKinHistory NextOfKinHistoryData = LaData.NextOfKinHistoryData[i];

                                NextOfKinData.NextOfKinID = NextOfKinHistoryData.NextOfKinID = dt.Rows[0]["NOKID"] as long?;

                                NextOfKinData.Name = NextOfKinHistoryData.Name = dt.Rows[0]["NOKFirstName"] as string;
                                if (dtHst != null && dtHst.Rows.Count > 0) NextOfKinHistoryData.Name = dtHst.Rows[0]["FirstName"] as string;

                                NextOfKinData.Surname = NextOfKinHistoryData.Surname = dt.Rows[0]["NOKSurname"] as string;
                                if (dtHst != null && dtHst.Rows.Count > 0) NextOfKinHistoryData.Surname = dtHst.Rows[0]["Surname"] as string;

                                NextOfKinData.RelationshipID = NextOfKinHistoryData.RelationshipID = dt.Rows[0]["NOKRelationshipID"] as long?;
                                if (dtHst != null && dtHst.Rows.Count > 0) NextOfKinHistoryData.RelationshipID = dtHst.Rows[0]["FKINRelationshipID"] as long?;

                                NextOfKinData.TelContact = NextOfKinHistoryData.TelContact = dt.Rows[0]["NOKTelContact"] as string;
                                if (dtHst != null && dtHst.Rows.Count > 0) NextOfKinHistoryData.TelContact = dtHst.Rows[0]["TelContact"] as string;
                            }
                        }
                    }
                }

                #endregion

                #region Children

                {
                    if (dtChild.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtChild.Rows.Count; i++)
                        {
                            LeadApplicationData.Child ChildData = LaData.ChildData[i];
                            LeadApplicationData.Child ChildHistoryData = LaData.ChildHistoryData[i];

                            ChildData.ChildID = ChildHistoryData.ChildID = dtChild.Rows[i]["ChildID"] as long?;
                            ChildData.PolicyChildID = ChildHistoryData.PolicyChildID = dtChild.Rows[i]["PolicyChildID"] as long?;
                            ChildData.Name = ChildHistoryData.Name = dtChild.Rows[i]["ChildFirstName"] as string;
                            ChildData.DateOfBirth = ChildHistoryData.DateOfBirth = dtChild.Rows[i]["ChildDateOfBirth"] as DateTime?;
                        }
                    }
                }

                #endregion

                #endregion LA, children and beneficiary details (Page 4)

                #region (Page 5) Sale Data

                if (dtSale.Rows.Count > 0)
                {
                    LaData.SaleData.SaleCallRefID = dtLead.Rows[0]["AgentID"] as long?;
                    LaData.SaleData.SaleStationNo = dtSale.Rows[0]["SaleStationNo"] as string;
                    LaData.SaleData.SaleDate = dtSale.Rows[0]["SaleDate"] as DateTime?;
                    LaData.SaleData.SaleTime = dtSale.Rows[0]["SaleTime"] as TimeSpan?;
                    LaData.SaleData.SaleTelNumberType = (lkpTelNumberType?)(dtSale.Rows[0]["SaleTelNumberTypeID"] as long?);

                    LaData.SaleData.BankCallRefID = dtSale.Rows[0]["BankCallRefUserID"] as long?;
                    LaData.SaleData.BankStationNo = dtSale.Rows[0]["BankStationNo"] as string;
                    LaData.SaleData.BankDate = dtSale.Rows[0]["BankDate"] as DateTime?;
                    LaData.SaleData.BankTime = dtSale.Rows[0]["BankTime"] as TimeSpan?;
                    LaData.SaleData.BankTelNumberType = (lkpTelNumberType?)(dtSale.Rows[0]["BankTelNumberTypeID"] as long?);

                    LaData.SaleData.ConfCallRefID = dtSale.Rows[0]["ConfCallRefUserID"] as long?;
                    LaData.SaleData.BatchCallRefID = dtSale.Rows[0]["BatchCallRefUserID"] as long?;

                    LaData.SaleData.ConfStationNo = dtSale.Rows[0]["ConfStationNo"] as string;
                    LaData.SaleData.ConfDate = dtSale.Rows[0]["ConfDate"] as DateTime?;
                    LaData.SaleData.ConfTime = dtSale.Rows[0]["ConfTime"] as TimeSpan?;
                    LaData.SaleData.ConfWorkDate = dtSale.Rows[0]["ConfWorkDate"] as DateTime?;
                    LaData.SaleData.ConfTelNumberType = (lkpTelNumberType?)(dtSale.Rows[0]["ConfTelNumberTypeID"] as long?);

                    LaData.SaleData.LeadFeedbackID = dtSale.Rows[0]["LeadFeedbackID"] as long?;
                    LaData.SaleData.Feedback = dtSale.Rows[0]["Feedback"] as string;
                    LaData.SaleData.FeedbackDate = dtSale.Rows[0]["FeedbackDate"] as DateTime?;

                    LaData.SaleData.ConfirmationFeedbackID = dtSale.Rows[0]["ConfirmationFeedbackID"] as long?;
                    LaData.SaleData.FKCMCallRefUserID = dtSale.Rows[0]["FKCMCallRefUserID"] as long?;
                    LaData.AppData.EMailRequested = Convert.ToBoolean(dtSale.Rows[0]["EMailRequested"]);
                    LaData.AppData.CustomerCareRequested = Convert.ToBoolean(dtSale.Rows[0]["CustomerCareRequested"]);
                    LaData.AppData.IsGoldenLead = Convert.ToBoolean(dtSale.Rows[0]["IsGoldenLead"]);
                    LaData.SaleData.Notes = dtSale.Rows[0]["Notes"] as string;

                    if (dtSale.Rows[0]["PageNumber"] != DBNull.Value)
                    {
                        LaData.SaleData.PageNumber = Convert.ToInt16(dtSale.Rows[0]["PageNumber"]); // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/214113774/comments
                    }

                    LaData.SaleData.CanChangeConfirmationDetails = Convert.ToBoolean(dtSale.Rows[0]["CanChangeConfirmationDetails"]);
                    LaData.SaleData.CanChangeConfirmationCallRefUser = Convert.ToBoolean(dtSale.Rows[0]["CanChangeConfirmationCallRefUser"]);

                    LaData.SaleData.CanConfirmPolicy = Convert.ToBoolean(dtSale.Rows[0]["CanConfirmPolicy"]);
                    LaData.SaleData.AutoPopulateConfWorkDate = dtSale.Rows[0]["AutoPopulateConfWorkDate"] as DateTime?;
                    LaData.SaleData.AutoPopulateConfDate = dtSale.Rows[0]["AutoPopulateConfDate"] as DateTime?;
                    LaData.SaleData.AutoPopulateConfTime = dtSale.Rows[0]["AutoPopulateConfTime"] as TimeSpan?;
                    LaData.SaleData.AutoPopulateConfirmationAgentFKUserID = dtSale.Rows[0]["AutoPopulateConfirmationAgentFKUserID"] as long?;
                    //LaData.SaleData.AutoPopulateConfirmationAgentFKUserID = dtSale.Rows[0]["AutoPopulateConfirmationAgentFKUserID"] != DBNull.Value ? Convert.ToInt64(dtSale.Rows[0]["AutoPopulateConfirmationAgentFKUserID"]) : (long?)null;

                    LaData.ClosureData.PermissionQuestionAsked = Convert.ToBoolean(dtSale.Rows[0]["PermissionQuestionAsked"] as bool?);
                }

                #endregion (Page 5) Sale Data

                #region Closure

                LaData.ClosureData.ClosureID = dtSale.Rows[0]["ClosureID"] as long?;
                if (LaData.ClosureData.ClosureID != null)
                {
                    chkClosureAccept.IsEnabled = true;
                    chkClosureAccept.IsChecked = true;
                    LoadClosureDocument();
                }

                #endregion

                #region Imported Policy Data

                {
                    if (dtImportedPolicyData.Rows.Count > 0)
                    {
                        LaData.ImportedPolicyData.ContractPremium = dtImportedPolicyData.Rows[0]["ContractPremium"] as decimal?;
                        LaData.ImportedCovers[0].Cover = dtImportedPolicyData.Rows[0]["LA1CancerCover"] as decimal?;
                        LaData.ImportedCovers[0].Premium = dtImportedPolicyData.Rows[0]["LA1CancerPremium"] as decimal?;
                        LaData.ImportedCovers[1].Cover = dtImportedPolicyData.Rows[0]["LA1DisabilityCover"] as decimal?;
                        LaData.ImportedCovers[1].Premium = dtImportedPolicyData.Rows[0]["LA1DisabilityPremium"] as decimal?;
                        LaData.ImportedCovers[2].Cover = dtImportedPolicyData.Rows[0]["LA1AccidentalDeathCover"] as decimal?;
                        LaData.ImportedCovers[2].Premium = dtImportedPolicyData.Rows[0]["LA1AccidentalDeathPremium"] as decimal?;
                        LaData.ImportedCovers[3].Cover = dtImportedPolicyData.Rows[0]["LA1FuneralCover"] as decimal?;
                        LaData.ImportedCovers[3].Premium = dtImportedPolicyData.Rows[0]["LA1FuneralPremium"] as decimal?;
                        LaData.ImportedCovers[4].Cover = dtImportedPolicyData.Rows[0]["LA2CancerCover"] as decimal?;
                        LaData.ImportedCovers[4].Premium = dtImportedPolicyData.Rows[0]["LA2CancerPremium"] as decimal?;
                        LaData.ImportedCovers[5].Cover = dtImportedPolicyData.Rows[0]["LA2DisabilityCover"] as decimal?;
                        LaData.ImportedCovers[5].Premium = dtImportedPolicyData.Rows[0]["LA2DisabilityPremium"] as decimal?;
                        LaData.ImportedCovers[6].Cover = dtImportedPolicyData.Rows[0]["LA2AccidentalDeathCover"] as decimal?;
                        LaData.ImportedCovers[6].Premium = dtImportedPolicyData.Rows[0]["LA2AccidentalDeathPremium"] as decimal?;
                        LaData.ImportedCovers[7].Cover = dtImportedPolicyData.Rows[0]["LA2FuneralCover"] as decimal?;
                        LaData.ImportedCovers[7].Premium = dtImportedPolicyData.Rows[0]["LA2FuneralPremium"] as decimal?;
                        LaData.ImportedCovers[8].Cover = dtImportedPolicyData.Rows[0]["KidsCancerCover"] as decimal?;
                        LaData.ImportedCovers[8].Premium = dtImportedPolicyData.Rows[0]["KidsCancerPremium"] as decimal?;
                        LaData.ImportedCovers[9].Cover = dtImportedPolicyData.Rows[0]["KidsDisabilityCover"] as decimal?;
                        LaData.ImportedCovers[9].Premium = dtImportedPolicyData.Rows[0]["KidsDisabilityPremium"] as decimal?;

                        if (LaData.AppData.IsLeadUpgrade)
                        {
                            PopulateImportedPolicyDataUpgrade();
                        }
                        //contract preium to display as per Brigette request for re-activation campaigns 2015/07/03
                        if (LaData.AppData.CampaignGroup == lkpINCampaignGroup.Reactivation)
                        {
                            lblcnPrem.Visibility = Visibility.Visible;
                            medCnPrem.Visibility = Visibility.Visible;
                        }
                    }
                }

                #endregion

                #region Platinum Bumpup

                {
                    var sqlQuery = "Select ID, Description From INBumpUpOption Where FKINCampaignTypeID = '" + LaData.PolicyData.CampaignTypeID + "'";
                    DataTable dt = Methods.GetTableData(sqlQuery);
                    DataView dv = dt.DefaultView;
                    dv.Sort = "Description ASC";

                    //cmbPlatinumBumpUp.Populate(dv.ToTable(), DescriptionField, IDField);
                    LaData.PolicyData.PlatinumBumpupOptionID = dtPolicy.Rows[0]["BumpUpOptionID"] as long?;
                }

                #endregion

                #region Beneficiary Grid

                {
                    if ((LaData.AppData.CampaignType == lkpINCampaignType.CancerFuneral
                         || LaData.AppData.CampaignType == lkpINCampaignType.MaccFuneral
                         || LaData.AppData.CampaignType == lkpINCampaignType.CancerFuneral99
                         || LaData.AppData.CampaignType == lkpINCampaignType.FemaleDis
                         || LaData.AppData.CampaignType == lkpINCampaignType.BlackMacc
                         || LaData.AppData.CampaignType == lkpINCampaignType.IGFemaleDisability
                         || LaData.UserData.UserType != lkpUserType.SalesAgent)
                         && (!LaData.AppData.IsLeadUpgrade))
                    {
                        grdBeneficiaries.Visibility = Visibility.Visible;
                        grdBenficiariesPercentageTotalBase.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        grdBeneficiaries.Visibility = Visibility.Hidden;
                        grdBenficiariesPercentageTotalBase.Visibility = Visibility.Hidden;
                    }
                }

                #endregion

                #region Redeemed Gift Information

                if (LaData.AppData.PlatinumBatchCode != null)
                {
                    if (LaData.AppData.PlatinumBatchCode.Contains("_"))
                    {
                        string batchCodeModifier = LaData.AppData.PlatinumBatchCode.Substring(LaData.AppData.PlatinumBatchCode.IndexOf("_", StringComparison.Ordinal)).ToUpper();

                        if (GlobalConstants.BatchCodes.RedeemGift.Contains(batchCodeModifier))
                        {
                            INGiftRedeem inGiftRedeem = INGiftRedeemMapper.SearchOne(importID, null);

                            if (inGiftRedeem != null)
                            {
                                LaData.RedeemGiftData.GiftRedeemID = inGiftRedeem.ID;
                                LaData.RedeemGiftData.GiftRedeemStatusID = inGiftRedeem.FKlkpGiftRedeemStatusID;
                                LaData.RedeemGiftData.GiftRedeemStatus = (lkpINGiftRedeemStatus?)inGiftRedeem.FKlkpGiftRedeemStatusID;
                                LaData.RedeemGiftData.GiftOptionID = inGiftRedeem.FKGiftOptionID;
                                LaData.RedeemGiftData.DateRedeemed = inGiftRedeem.RedeemedDate;
                                LaData.RedeemGiftData.PODDate = inGiftRedeem.PODDate;
                                LaData.RedeemGiftData.PODSignature = inGiftRedeem.PODSignature;
                            }
                        }
                    }
                }

                #endregion

                #region ImportExtra

                INImportExtra inImportExtra = INImportExtraMapper.SearchOne(importID, null, null, null, null, null, null, null, null);
                if (inImportExtra != null)
                {
                    LaData.ImportExtraData.Extension1 = !string.IsNullOrWhiteSpace(inImportExtra.Extension1) ? inImportExtra.Extension1 : GetUserExtension(LaData.AppData.AgentID);
                    LaData.ImportExtraData.NotPossibleBumpUp = string.IsNullOrWhiteSpace(inImportExtra.NotPossibleBumpUp.ToString()) ? false : (bool)inImportExtra.NotPossibleBumpUp;
                }
                else
                {
                    LaData.ImportExtraData.Extension1 = GetUserExtension(LaData.AppData.AgentID);
                }

                #endregion

                #region SMS Data
                if (dtSMS.Rows.Count > 0)
                {
                    LaData.SMSData.SMSStatusTypeID = (WPF.Enumerations.Insure.lkpSMSStatusType?)(dtSMS.Rows[0]["SMSStatusTypeID"] as long?);
                    LaData.SMSData.SMSStatusSubtypeID = (WPF.Enumerations.Insure.lkpSMSStatusSubtype?)(dtSMS.Rows[0]["SMSStatusSubtypeID"] as long?);
                    LaData.SMSData.SMSSubmissionDate = Convert.ToDateTime(dtSMS.Rows[0]["SMSSubmissionDate"].ToString());//DateTime.ParseExact(dtSMS.Rows[0]["SMSSubmissionDate"].ToString(), "yyyy-MM-ddThh:mm:ssZ", new CultureInfo("en-ZA"));
                    LaData.SMSData.SMSCount = Convert.ToInt32(dtSMS.Rows[0]["SMSCount"].ToString());

                    CheckSMSSent();
                }

                #endregion SMS Data

                #region Import Call Monitoring

                inImportCallMonitoring = INImportCallMonitoringMapper.SearchOne(importID, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

                #endregion

                LaData.AppData.ImportID = importID;

                #region CardLayout Start Display
                string displayAmount;

                try { displayAmount = Convert.ToString(Methods.GetTableData("Select [INOption].[Display] from [INOption] where [INOption].[ID] = " + LaData.PolicyData.OptionID).Rows[0][0]); } catch (Exception a) { displayAmount = "Please select an option"; }

                UpgradeBtnOptionSelection.Content = displayAmount;

                #endregion

                if (!LaData.AppData.IsLeadUpgrade) CalculateCost(false);
                LaData.AppData.IsLeadLoaded = true;



                #region lead Permission Navigation
                if (LaData.AppData.CampaignID == 102 ||
                    LaData.AppData.CampaignID == 2 ||
                    LaData.AppData.CampaignID == 103 ||
                    LaData.AppData.CampaignID == 250 ||
                    LaData.AppData.CampaignID == 6 ||
                    LaData.AppData.CampaignID == 105)
                {
                    btnPermissionLead.Visibility = Visibility.Visible;
                    if (LaData.AppData.CampaignID == 6 || LaData.AppData.CampaignID == 105)
                    {

                    }
                    else
                    {
                        chkLeadPermission.Visibility = Visibility.Visible;
                        LblLeadPermission.Visibility = Visibility.Visible;
                    }


                    try
                    {
                        string strQuery;
                        strQuery = "SELECT ID FROM INPermissionLead WHERE FKImportID = " + LaData.AppData.ImportID;

                        DataTable dtPermissionLeadIsloaded = Methods.GetTableData(strQuery);
                        if (dtPermissionLeadIsloaded.Rows.Count == 0)
                        {
                            chkLeadPermission.IsChecked = false;
                        }
                        else
                        {
                            chkLeadPermission.IsChecked = true;
                        }
                    }
                    catch
                    {

                    }

                }
                else
                {
                    btnPermissionLead.Visibility = Visibility.Collapsed;
                    chkLeadPermission.Visibility = Visibility.Collapsed;
                    LblLeadPermission.Visibility = Visibility.Collapsed;
                }
                #endregion
                //ShowNotes(importID.Value);

                if (LaData.AppData.IsLeadUpgrade)
                {
                    btnSave.SetValue(Grid.RowProperty, 16);
                }
                else
                {
                    btnSave.SetValue(Grid.RowProperty, 15);
                }

                #region DebiCheck Workings

                try
                {
                    StringBuilder strQuery = new StringBuilder();
                    strQuery.Append("SELECT TOP 1 SMSBody [Response] ");
                    strQuery.Append("FROM DebiCheckSent ");
                    strQuery.Append("WHERE FKImportID = " + LaData.AppData.ImportID);
                    strQuery.Append(" ORDER BY ID DESC");
                    DataTable dt = Methods.GetTableData(strQuery.ToString());

                    string responses = dt.Rows[0]["Response"].ToString();
                    //string responses = "2";

                    if (responses.Contains("1"))
                    {
                        DebiCheckBorder.BorderBrush = Brushes.Green;
                        btnDebiCheck.ToolTip = "Record Created";

                        btnDebiCheck.IsEnabled = false;

                    }
                    else if (responses.Contains("2"))
                    {
                        DebiCheckBorder.BorderBrush = Brushes.Green;
                        btnDebiCheck.ToolTip = "Record Submitted";

                        btnDebiCheck.IsEnabled = false;

                    }
                    else if (responses.Contains("3"))
                    {
                        DebiCheckBorder.BorderBrush = Brushes.Green;
                        btnDebiCheck.ToolTip = "Results Received but no Further Details";

                        btnDebiCheck.IsEnabled = false;

                    }
                    else if (responses.Contains("4"))
                    {
                        DebiCheckBorder.BorderBrush = Brushes.Red;
                        btnDebiCheck.ToolTip = "Bank Returned Authentication Failure";
                        btnDebiCheck.IsEnabled = true;

                    }
                    else if (responses.Contains("5"))
                    {
                        DebiCheckBorder.BorderBrush = Brushes.Red;
                        btnDebiCheck.ToolTip = "Bank Returned Error With File Submitted";
                        btnDebiCheck.IsEnabled = true;

                    }
                    else if (responses.Contains("6"))
                    {
                        DebiCheckBorder.BorderBrush = Brushes.Green;
                        btnDebiCheck.ToolTip = "Mandate Approved";

                        btnDebiCheck.IsEnabled = false;

                    }
                    else if (responses.Contains("7"))
                    {
                        DebiCheckBorder.BorderBrush = Brushes.Red;
                        btnDebiCheck.ToolTip = "Client Rejected";

                        btnDebiCheck.IsEnabled = false;
                    }
                    else if (responses.Contains("8"))
                    {
                        DebiCheckBorder.BorderBrush = Brushes.Green;
                        btnDebiCheck.ToolTip = "No Response From Client Sent Mandate";

                        btnDebiCheck.IsEnabled = false;
                    }
                    else if (responses.Contains("9"))
                    {
                        DebiCheckBorder.BorderBrush = Brushes.Yellow;
                        btnDebiCheck.ToolTip = "Timeout on Submission. Please Re-Submit";
                        btnDebiCheck.IsEnabled = true;

                    }
                    else if (responses.Contains("10"))
                    {
                        DebiCheckBorder.BorderBrush = Brushes.Green;
                        btnDebiCheck.ToolTip = "File delivered to XCOM for processing";

                        btnDebiCheck.IsEnabled = false;
                    }
                    else if (responses.Contains(""))
                    {
                        DebiCheckBorder.BorderBrush = Brushes.Green;
                        btnDebiCheck.ToolTip = "Sent";

                        btnDebiCheck.IsEnabled = false;
                    }
                    else
                    {
                        DebiCheckBorder.BorderBrush = Brushes.White;
                        btnDebiCheck.IsEnabled = true;
                    }
                }
                catch
                {
                    DebiCheckBorder.BorderBrush = Brushes.White;
                    btnDebiCheck.IsEnabled = true;
                }

                if (LaData.AppData.CampaignGroupType == lkpINCampaignGroupType.Base)
                {
                    btnDebiCheck.SetValue(Grid.RowProperty, 16);
                    DebiCheckBorder.SetValue(Grid.RowProperty, 16);

                    if (LaData.AppData.CampaignID == 7
                            || LaData.AppData.CampaignID == 9
                            || LaData.AppData.CampaignID == 10
                            || LaData.AppData.CampaignID == 294
                            || LaData.AppData.CampaignID == 295
                            || LaData.AppData.CampaignID == 24
                            || LaData.AppData.CampaignID == 25
                            || LaData.AppData.CampaignID == 11
                            || LaData.AppData.CampaignID == 12
                            || LaData.AppData.CampaignID == 13
                            || LaData.AppData.CampaignID == 14
                            || LaData.AppData.CampaignID == 85
                            || LaData.AppData.CampaignID == 86
                            || LaData.AppData.CampaignID == 87
                            || LaData.AppData.CampaignID == 281
                            || LaData.AppData.CampaignID == 324
                            || LaData.AppData.CampaignID == 325
                            || LaData.AppData.CampaignID == 326
                            || LaData.AppData.CampaignID == 327
                            || LaData.AppData.CampaignID == 264
                            || LaData.AppData.CampaignID == 4)
                    {
                        GetBankingDetailLookup();
                    }
                }
                else
                {
                    btnDebiCheck.SetValue(Grid.RowProperty, 17);
                    DebiCheckBorder.SetValue(Grid.RowProperty, 17);
                    GetBankingDetailLookup();

                }

                #endregion



            }

            catch (Exception ex)
            {
                HandleException(ex);
                LaData.AppData.ImportID = null;
                LaData.AppData.IsLeadLoaded = false;
                _flagLeadIsBusyLoading = false;
            }

            finally
            {
                SetCursor(Cursors.Arrow);
                if (LaData.AppData.IsLeadUpgrade) CalculateCostUpgrade(false);
                medReference.Focus();
                //_flagLeadIsBusyLoading = false;

                SetPolicyDefaults();

                #region Goto Page2 for Sales Agents (Base)

                if (!LaData.AppData.IsLeadUpgrade &&
                    LaData.AppData.CampaignGroup != lkpINCampaignGroup.Extension &&
                    LaData.UserData.UserType == lkpUserType.SalesAgent)
                {
                    if (IsVisible)
                    {
                        if (Page1.IsVisible)
                        {
                            btnNext_Click(null, null);
                        }
                    }
                    else
                    {
                        btnNext.IsVisibleChanged += delegate
                        {
                            if (btnNext.IsVisible)
                            {
                                btnNext_Click(null, null);
                            }
                        };
                    }
                }

                #endregion

                #region Find next and previous lead in leadbook if available

                if (LaData.AppData.ImportID != null)
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@ImportID", LaData.AppData.ImportID);

                    DataSet ds = Methods.ExecuteStoredProcedure("spINGetNextAvailableLead", parameters);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            LaData.AppData2.ImportIDNextLead = ds.Tables[0].Rows[0]["FKINImportID"] as long?;
                        }
                    }

                    ds = Methods.ExecuteStoredProcedure("spINGetPrevAvailableLead", parameters);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            LaData.AppData2.ImportIDPrevLead = ds.Tables[0].Rows[0]["FKINImportID"] as long?;
                        }
                    }
                }

                #endregion

                //if ((LaData.AppData.IsLeadLoaded) && (LaData.AppData.LoadedRefNo != null))
                //{
                //    ShowNotes(importID.Value);
                //}

                #region Change application screen border colour

                MainBorder.BorderBrush = (Brush)FindResource("BrandedBrushIN");

                if (LaData.AppData.PlatinumBatchCode != null)
                {
                    if (LaData.AppData.PlatinumBatchCode.Contains("_"))
                    {
                        string batchCodeModifier = LaData.AppData.PlatinumBatchCode.Substring(LaData.AppData.PlatinumBatchCode.IndexOf("_", StringComparison.Ordinal)).ToUpper();

                        if (GlobalConstants.BatchCodes.GiftNotRedeemed.Contains(batchCodeModifier))
                        {
                            if (LaData.RedeemGiftData.GiftRedeemStatus == lkpINGiftRedeemStatus.Redeemed)
                            {
                                MainBorder.BorderBrush = Brushes.Yellow;
                            }
                        }
                    }
                }

                //if (GlobalSettings.ApplicationUser.)
                DateTime tempStartDate = Convert.ToDateTime(Methods.GetTableData("SELECT TOP 1 TempStartDate FROM Blush.dbo.HRStaff WHERE FKUserID = " + GlobalSettings.ApplicationUser.ID).AsEnumerable().Select(x => x["TempStartDate"]).FirstOrDefault());

                if (tempStartDate.AddDays(14) >= DateTime.Today)
                {
                    if (LaData.AppData.LeadStatus == (long)lkpINLeadStatus.Accepted)
                    {
                        MainBorder.BorderBrush = Brushes.Orange;
                    }
                }

                #endregion

                cmbStatus.ToolTip = GetSavedLeadStatusToolTip(LaData.AppData.ImportID);



                _flagLeadIsBusyLoading = false;
            }
        }

        private void SetDialHoursGraph(StackPanel stackPanel, string phoneNumber)
        {
            //Clear Segments First
            {
                int index = 0;
                foreach (object o in LogicalTreeHelper.GetChildren(stackPanel))
                {
                    if (o is Border)
                    {
                        SolidColorBrush borderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
                        ((Border)o).Background = borderBrush;
                        index++;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PhoneNumber", phoneNumber);
                DataSet ds = Methods.ExecuteStoredProcedure("Voice.dbo.spGetDialHours", parameters);
                DataTable dt = ds.Tables[0];

                int index = 0;
                foreach (object o in LogicalTreeHelper.GetChildren(stackPanel))
                {
                    if (o is Border)
                    {
                        if (!string.IsNullOrWhiteSpace(dt.Rows[index]["Result"] as string))
                        {
                            string strColor = dt.Rows[index]["Result"] as string;
                            SolidColorBrush borderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(strColor));
                            ((Border)o).Background = borderBrush;
                        }

                        index++;
                    }
                }
            }
        }

        private void CheckSMSSent()
        {
            string smsStatusTypeString = LaData.SMSData.SMSStatusTypeID == null ? "" : Enum.GetName(typeof(WPF.Enumerations.Insure.lkpSMSStatusType), LaData.SMSData.SMSStatusTypeID);
            string smsStatusSubtypeString = LaData.SMSData.SMSStatusSubtypeID == null ? "" : Enum.GetName(typeof(WPF.Enumerations.Insure.lkpSMSStatusSubtype), LaData.SMSData.SMSStatusSubtypeID);

            LaData.SMSData.SMSToolTip = (smsStatusSubtypeString == "" ? "SENT" : smsStatusSubtypeString) + " " + LaData.SMSData.SMSSubmissionDate;

            LaData.SMSData.IsSMSSent = smsStatusTypeString != "FAILED" && smsStatusTypeString != "" && smsStatusTypeString != null ? true : false;
        }

        private string GetSavedLeadStatusToolTip(long? importID)
        {
            string strToolTip = null;

            if (!string.IsNullOrEmpty(Convert.ToString(cmbStatus.ToolTip)))
            {
                strToolTip = Convert.ToString(cmbStatus.ToolTip);
            }

            if (importID != null && LaData.AppData.lkpLeadStatus == lkpINLeadStatus.Accepted)
            {
                SqlParameter[] parameters = new SqlParameter[2];
                bool isSavedLeadStatusCF;
                bool isSavedLeadStatusCMCF;
                bool isSavedLeadStatusCan;
                bool isSavedLeadStatusCMCan;
                string strOriginalDOS;

                strToolTip = strToolTip + Environment.NewLine;

                parameters[0] = new SqlParameter("@ImportID", LaData.AppData.ImportID);

                parameters[1] = new SqlParameter("@INLeadStatusID", 8);
                isSavedLeadStatusCF = Convert.ToBoolean(Methods.ExecuteFunction("IsSavedLeadStatus", parameters));

                parameters[1] = new SqlParameter("@INLeadStatusID", 17);
                isSavedLeadStatusCMCF = Convert.ToBoolean(Methods.ExecuteFunction("IsSavedLeadStatus", parameters));

                parameters[1] = new SqlParameter("@INLeadStatusID", 7);
                isSavedLeadStatusCan = Convert.ToBoolean(Methods.ExecuteFunction("IsSavedLeadStatus", parameters));

                parameters[1] = new SqlParameter("@INLeadStatusID", 16);
                isSavedLeadStatusCMCan = Convert.ToBoolean(Methods.ExecuteFunction("IsSavedLeadStatus", parameters));

                if (isSavedLeadStatusCF)
                {
                    parameters[1] = new SqlParameter("@INLeadStatusID", 8);
                    strOriginalDOS = Convert.ToDateTime(Methods.ExecuteFunction("GetSavedLeadStatusOriginalDOS", parameters)).ToShortDateString();

                    strToolTip = strToolTip + "Saved Carried Forward: " + strOriginalDOS;
                }
                else if (isSavedLeadStatusCMCF)
                {
                    parameters[1] = new SqlParameter("@INLeadStatusID", 17);
                    strOriginalDOS = Convert.ToDateTime(Methods.ExecuteFunction("GetSavedLeadStatusOriginalDOS", parameters)).ToShortDateString();

                    strToolTip = strToolTip + "Saved CM Carried Forward: " + strOriginalDOS;
                }
                else if (isSavedLeadStatusCan)
                {
                    parameters[1] = new SqlParameter("@INLeadStatusID", 7);
                    strOriginalDOS = Convert.ToDateTime(Methods.ExecuteFunction("GetSavedLeadStatusOriginalDOS", parameters)).ToShortDateString();

                    strToolTip = strToolTip + "Saved Cancelled: " + strOriginalDOS;
                }
                else if (isSavedLeadStatusCMCan)
                {
                    parameters[1] = new SqlParameter("@INLeadStatusID", 16);
                    strOriginalDOS = Convert.ToDateTime(Methods.ExecuteFunction("GetSavedLeadStatusOriginalDOS", parameters)).ToShortDateString();

                    strToolTip = strToolTip + "Saved CM Cancelled: " + strOriginalDOS;
                }
            }

            return strToolTip;
        }

        private string GetUserExtension(long? userID)
        {
            string result = null;

            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT TOP 1 Extension ");
            strSQL.Append("FROM Blush.dbo.lkpHRExtension ");
            strSQL.Append("JOIN Blush.dbo.HRStaffExtension ON Blush.dbo.HRStaffExtension.FKHRExtensionID = Blush.dbo.lkpHRExtension.ID ");
            strSQL.Append("JOIN Blush.dbo.HRStaff ON Blush.dbo.HRStaff.ID = Blush.dbo.HRStaffExtension.FKHRStaffID ");
            strSQL.Append("WHERE Blush.dbo.HRStaff.FKUserID = '" + userID + "' ");
            strSQL.Append("ORDER BY DateEffective DESC");

            DataTable dtExtension = Methods.GetTableData(strSQL.ToString());

            if (dtExtension.Rows.Count > 0)
            {
                result = dtExtension.Rows[0]["Extension"] as string;
            }

            return result;
        }

        private void UpdateLeadStatuses(long? importID)
        {
            DataTable dtLeadStatuses = Insure.AddMissingLeadStatuses(importID);

            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/223229893/comments
            //bool isCancellation = Insure.HasLeadBeenCancelled(importID);
            //if (isCancellation)
            //{
            //    //cmbStatus.IsEnabled = false;
            //    LaData.AppData.CanChangeStatus = false;
            //}
            //else
            //{
            //    //cmbStatus.IsEnabled = true;
            //    LaData.AppData.CanChangeStatus = true;
            //}

            cmbStatus.Populate(dtLeadStatuses, DescriptionField, IDField);

        }

        private void UpdateLeadApplicationScreenLookups(long? importID)
        {
            DataSet dsUpdatedLeadApplicationScreenLookups = Insure.UpdateLeadApplicationScreenLookups(importID);
            DataTable dtLA2Relationships = dsUpdatedLeadApplicationScreenLookups.Tables[0];

            cmbLA2Relationship.Populate(dtLA2Relationships, DescriptionField, IDField);
        }

        private void ShowNotes(long fkINImportID)
        {
            //if (_isNotesFeatureAvailable)
            //{
            #region Display the Notes screen

            if (!_hasNoteBeenDisplayed)
            {
                NoteScreen noteScreen = new NoteScreen(fkINImportID /*, NoteScreen.NoteScreenMode.ReadOnly*/);
                if (noteScreen.HasNotes)
                {
                    ShowDialog(this, new INDialogWindow(noteScreen));
                }

                //#region Get the notes data

                //DataTable dtNotes = Insure.GetLeadNotes(fkINImportID);

                //#endregion Get the notes data

                //if (dtNotes.Rows.Count > 0)
                //{
                //    NoteScreen noteScreen = new NoteScreen(fkINImportID, NoteScreen.NoteScreenMode.ReadOnly);
                //    noteScreen.LoadNote(dtNotes.Rows[0]);
                //    ShowDialog(this, new INDialogWindow(noteScreen));
                //    _hasNoteBeenDisplayed = true;
                //}
            }

            #endregion Display the Notes screen
            //}
        }

        private void SetPolicyDefaults()
        {
            if (LaData.AppData.CampaignType == lkpINCampaignType.FemaleDis || LaData.AppData.CampaignType == lkpINCampaignType.IGFemaleDisability)
            {
                chkFuneral.IsEnabled = false;
            }

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
                        DefaultOptionIDCards = LaData.PolicyData.OptionID;

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
                        chkChild_Click(null, null);
                        LaData.PolicyData.IsFuneralChecked = true;
                        chkFuneral_Click(null, null);
                    }
                }
            }

            if (LaData.AppData.CampaignType == lkpINCampaignType.FemaleDis || LaData.AppData.CampaignType == lkpINCampaignType.IGFemaleDisability)
            {
                chkFuneral.IsEnabled = false;

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

        private void LoadUpgradeLead()
        {
            LayoutRoot.Children.Remove(Page1);
            Page1.Visibility = Visibility.Visible;
            BorderPage1.Child = Page1;

            LayoutRoot.Children.Remove(Page2);
            Page2.Visibility = Visibility.Visible;
            BorderPage2.Child = Page2;

            Page3.Visibility = Visibility.Collapsed;

            LayoutRoot.Children.Remove(Page4);
            Page4.Visibility = Visibility.Visible;
            BorderPage3.Child = Page4;

            LayoutRoot.Children.Remove(Page5);
            Page5.Visibility = Visibility.Visible;
            BorderPage4.Child = Page5;

            #region show/hide relevant fields

            grdPolicyBase.Visibility = Visibility.Collapsed;
            //grdReferror.Visibility = Visibility.Collapsed;
            grdChildren.Visibility = Visibility.Collapsed;
            grdFeedback.Visibility = Visibility.Collapsed;

            grdPolicyBase.Visibility = Visibility.Collapsed;
            grdPolicyUpgrade.Visibility = Visibility.Visible;

            PolicyDetailsBase.Visibility = Visibility.Collapsed;
            PolicyDetailsUpgrade.Visibility = Visibility.Visible;

            lbllapseDate.Visibility = Visibility.Collapsed;
            dteLapseDate.Visibility = Visibility.Collapsed;

            //lblLA2Title.Visibility = Visibility.Collapsed;
            //cmbLA2Title.Visibility = Visibility.Collapsed;
            //lblLA2Gender.Visibility = Visibility.Collapsed;
            //cmbLA2Gender.Visibility = Visibility.Collapsed;
            //lblLA2IDNumber.Visibility = Visibility.Collapsed;
            //medLA2IDNumber.Visibility = Visibility.Collapsed;

            grdBeneficiaries.Visibility = Visibility.Collapsed;
            grdBenficiariesPercentageTotalBase.Visibility = Visibility.Collapsed;
            grdBeneficiariesUpg.Visibility = Visibility.Visible;
            LA2.Visibility = Visibility.Collapsed;
            LA2Upg.Visibility = Visibility.Visible;

            lblCommenceDateUpgrade.Visibility = Visibility.Visible;
            dteCommenceDateUpgrade.Visibility = Visibility.Visible;

            #endregion

            lblPage.Text = "(Upgrade)";
            UpgradePage.Visibility = Visibility.Visible;
        }

        private void LoadExtensionLead()
        {
            Page1.Visibility = Visibility.Visible;
            LayoutRoot.Children.Remove(Page2);
            LayoutRoot.Children.Remove(Page3);
            LayoutRoot.Children.Remove(Page4);
            Page5.Visibility = Visibility.Collapsed;
            //lblClosure.Visibility = Visibility.Collapsed;
            //chkClosure.Visibility = Visibility.Collapsed;
            IsExtension.IsChecked = true;
            lblPage.Text = "(Extension)";

        }

        private void UnLoadUpgradeLead()
        {
            try
            {
                if (LaData.AppData.IsLeadUpgrade)
                {
                    BorderPage1.Child = null;
                    Page1.Visibility = Visibility.Visible;
                    if (Page1.Parent == null) LayoutRoot.Children.Add(Page1);

                    BorderPage2.Child = null;
                    Page2.Visibility = Visibility.Collapsed;
                    if (Page2.Parent == null) LayoutRoot.Children.Add(Page2);

                    Page3.Visibility = Visibility.Collapsed;

                    BorderPage3.Child = null;
                    Page4.Visibility = Visibility.Collapsed;
                    if (Page4.Parent == null) LayoutRoot.Children.Add(Page4);

                    BorderPage4.Child = null;
                    Page5.Visibility = Visibility.Collapsed;
                    if (Page5.Parent == null) LayoutRoot.Children.Add(Page5);
                }

                #region show/hide relevant fields

                grdPolicyBase.Visibility = Visibility.Visible;
                //grdReferror.Visibility = Visibility.Visible;
                grdChildren.Visibility = Visibility.Visible;
                grdFeedback.Visibility = Visibility.Visible;

                grdPolicyBase.Visibility = Visibility.Visible;
                grdPolicyUpgrade.Visibility = Visibility.Collapsed;

                PolicyDetailsBase.Visibility = Visibility.Visible;
                PolicyDetailsUpgrade.Visibility = Visibility.Collapsed;

                lbllapseDate.Visibility = Visibility.Visible;
                dteLapseDate.Visibility = Visibility.Visible;

                //lblLA2Title.Visibility = Visibility.Visible;
                //cmbLA2Title.Visibility = Visibility.Visible;
                //lblLA2Gender.Visibility = Visibility.Visible;
                //cmbLA2Gender.Visibility = Visibility.Visible;
                //lblLA2IDNumber.Visibility = Visibility.Visible;
                //medLA2IDNumber.Visibility = Visibility.Visible;

                grdBeneficiaries.Visibility = Visibility.Visible;
                grdBenficiariesPercentageTotalBase.Visibility = Visibility.Visible;
                grdBeneficiariesUpg.Visibility = Visibility.Collapsed;
                LA2.Visibility = Visibility.Visible;
                LA2Upg.Visibility = Visibility.Collapsed;

                lblCommenceDateUpgrade.Visibility = Visibility.Collapsed;
                dteCommenceDateUpgrade.Visibility = Visibility.Collapsed;

                #endregion

                lblPage.Text = "(Lead)";
                UpgradePage.Visibility = Visibility.Collapsed;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void Save()
        {
            try
            {
                SetCursor(Cursors.Wait);

                if (LogInfo())
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Inconsistent policy options found\nPlease correct and try saving again\nLead not saved\n", "Save Result", ShowMessageType.Exclamation);
                    return;
                }

                LaData.AppData.IsLeadSaving = true;



                Database.BeginTransaction(null, System.Data.IsolationLevel.Snapshot);

                #region Import
                if (LaData.AppData.ImportID.HasValue)
                {
                    INImport import = new INImport((long)LaData.AppData.ImportID);
                    import.FKINLeadStatusID = LaData.AppData.LeadStatus;

                    import.FKUserID = LaData.AppData.AgentID;
                    import.DateOfSale = LaData.AppData.DateOfSale;

                    #region Sale Data (page5)

                    import.FKBankCallRefUserID = LaData.SaleData.BankCallRefID;
                    import.BankStationNo = LaData.SaleData.BankStationNo;
                    import.BankDate = LaData.SaleData.BankDate;
                    import.BankTime = LaData.SaleData.BankTime;
                    import.FKBankTelNumberTypeID = (long?)LaData.SaleData.BankTelNumberType;

                    import.FKSaleCallRefUserID = LaData.SaleData.SaleCallRefID;
                    import.SaleStationNo = medSaleStationNo.Value as string;
                    import.SaleDate = dteSaleDate.DateValue;
                    import.SaleTime = (dteSaleTime.DateValue == null) ? (TimeSpan?)null : (DateTime.Parse(dteSaleTime.DateValue.ToString())).TimeOfDay;
                    import.FKSaleTelNumberTypeID = (long?)LaData.SaleData.SaleTelNumberType;

                    import.FKConfCallRefUserID = (long?)cmbConfCallRef.SelectedValue;
                    import.FKBatchCallRefUserID = (long?)cmbBatchCallRef.SelectedValue;
                    import.ConfStationNo = medConfStationNo.Value as string;
                    import.ConfDate = dteConfDate.DateValue;
                    import.ConfWorkDate = dteConfWorkDate.DateValue;
                    import.ConfTime = (dteConfTime.DateValue == null) ? (TimeSpan?)null : (DateTime.Parse(dteConfTime.DateValue.ToString())).TimeOfDay;
                    import.FKConfTelNumberTypeID = (long?)LaData.SaleData.ConfTelNumberType;

                    import.Notes = LaData.SaleData.Notes;

                    import.Feedback = LaData.SaleData.Feedback;
                    import.FeedbackDate = LaData.SaleData.FeedbackDate;
                    import.FKINLeadFeedbackID = LaData.SaleData.LeadFeedbackID;

                    import.FKINConfirmationFeedbackID = LaData.SaleData.ConfirmationFeedbackID;

                    #endregion

                    import.FKClosureID = Convert.ToBoolean(chkClosure.IsChecked) ? LaData.ClosureData.ClosureID : null;
                    import.IsConfirmed = LaData.AppData.IsConfirmed;
                    import.IsMining = LaData.AppData.IsMining;
                    //Change: Pheko
                    //if (_declineReasonID == -1)
                    //{
                    //    _declineReasonID = LaData.AppData.DeclineReasonID;
                    //}
                    //if (_cancellationReasonID == -1)
                    //{
                    //    _cancellationReasonID = LaData.AppData.CancellationReasonID;
                    //}

                    import.FKINDeclineReasonID = LaData.AppData.DeclineReasonID;
                    import.FKINCancellationReasonID = LaData.AppData.CancellationReasonID;
                    import.FKINCarriedForwardReasonID = LaData.AppData.CarriedForwardReasonID;
                    import.FKINCallMonitoringCarriedForwardReasonID = LaData.AppData.FKINCallMonitoringCarriedForwardReasonID; // https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/222494520/comments
                    import.FKINCallMonitoringCancellationReasonID = LaData.AppData.FKINCallMonitoringCancellationReasonID;
                    import.FKINDiaryReasonID = LaData.AppData.DiaryReasonID;
                    import.FutureContactDate = LaData.AppData.FutureContactDate;
                    import.CancerOption = LaData.PolicyData.PlatinumPlan;
                    import.PermissionQuestionAsked = LaData.ClosureData.PermissionQuestionAsked;

                    import.Save(_validationResult);

                }
                else
                {
                    throw new Exception("importID Error");
                }
                #endregion

                #region Permission Lead
                try
                {
                    if (chkLeadPermission.IsChecked == true)
                    {
                        string strQuery;
                        strQuery = "SELECT ID FROM INPermissionLead WHERE FKImportID = " + LaData.AppData.ImportID;

                        DataTable dtPermissionLeadIsloaded = Methods.GetTableData(strQuery);
                        if (dtPermissionLeadIsloaded.Rows.Count == 0)
                        {
                            GlobalSettings.IsLoadedPermission = 0;
                        }
                        else
                        {
                            GlobalSettings.IsLoadedPermission = 1;

                        }

                        if (medLA2ContactPhone.Text == null || medLA2ContactPhone.Text == "")
                        {

                        }
                        else
                        {

                            long? ID = null;
                            DataTable dtIsloaded;
                            long IDReal;
                            try
                            {
                                string strQueryIsLoaded;
                                strQueryIsLoaded = "SELECT top 1 ID FROM INPermissionLead WHERE FKImportID = " + LaData.AppData.ImportID; ;
                                dtIsloaded = Methods.GetTableData(strQueryIsLoaded);
                                ID = dtIsloaded.Rows[0]["ID"] as long?;
                                IDReal = long.Parse(ID.ToString());
                            }
                            catch
                            {
                                IDReal = 0;
                                dtIsloaded = null;
                            }

                            string strQuerySavedBy;
                            strQuerySavedBy = "SELECT top 1 SavedBy FROM INPermissionLead WHERE FKImportID = " + LaData.AppData.ImportID;
                            DataTable dtPermissionSavedByIsloaded = Methods.GetTableData(strQuerySavedBy);

                            string strQueryDateSaved;
                            strQueryDateSaved = "SELECT top 1 SavedBy FROM INPermissionLead WHERE FKImportID = " + LaData.AppData.ImportID;
                            DataTable dtPermissiondateSavedIsloaded = Methods.GetTableData(strQueryDateSaved);

                            if (dtIsloaded == null)
                            {
                                string dateString = "";
                                DateTime resultingDate;
                                try
                                {
                                    dateString = dteLA2DateOfBirth.Text;
                                    var fr = new System.Globalization.CultureInfo("fr-FR");
                                    resultingDate = DateTime.ParseExact(dateString, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                                }
                                catch
                                {
                                    resultingDate = DateTime.Parse("0000/00/00");
                                }

                                INPermissionLead inpermissionlead = new INPermissionLead();
                                try { inpermissionlead.FKINImportID = LaData.AppData.ImportID; } catch { }
                                try { inpermissionlead.Title = cmbLA2Title.Text.ToString(); } catch { }
                                try { inpermissionlead.Firstname = medLA2Name.Text.ToString(); } catch { }
                                try { inpermissionlead.Surname = medLA2Surname.Text.ToString(); } catch { }
                                try { inpermissionlead.Cellnumber = medLA2ContactPhone.Text.ToString(); } catch { }
                                try { inpermissionlead.AltNumber = medAltContactPhone.Text.ToString(); } catch { }

                                try { inpermissionlead.DateOfBirth = resultingDate; } catch { }

                                if (dtPermissionSavedByIsloaded.Rows.Count == 0)
                                {
                                    try { inpermissionlead.SavedBy = GlobalSettings.ApplicationUser.ID.ToString(); } catch { }
                                }
                                if (dtPermissiondateSavedIsloaded.Rows.Count == 0)
                                {
                                    try { inpermissionlead.DateSaved = DateTime.Now; } catch { }
                                }

                                inpermissionlead.Save(_validationResult);

                                ShowMessageBox(new INMessageBoxWindow1(), "Lead Permission successfully saved.\n", "Save Result", ShowMessageType.Information);
                            }
                            else
                            {
                                string dateString = "";
                                DateTime resultingDate;
                                try
                                {
                                    dateString = dteLA2DateOfBirth.Text;
                                    var fr = new System.Globalization.CultureInfo("fr-FR");
                                    resultingDate = DateTime.ParseExact(dateString, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                                }
                                catch
                                {
                                    resultingDate = DateTime.Parse("0000/00/00");
                                }


                                INPermissionLead inpermissionlead = new INPermissionLead(IDReal);
                                try { inpermissionlead.FKINImportID = LaData.AppData.ImportID; } catch { }
                                try { inpermissionlead.Title = cmbLA2Title.Text.ToString(); } catch { }
                                try { inpermissionlead.Firstname = medLA2Name.Text.ToString(); } catch { }
                                try { inpermissionlead.Surname = medLA2Surname.Text.ToString(); } catch { }
                                try { inpermissionlead.Cellnumber = medLA2ContactPhone.Text.ToString(); } catch { }
                                try { inpermissionlead.AltNumber = medAltContactPhone.Text.ToString(); } catch { }


                                try { inpermissionlead.DateOfBirth = resultingDate; } catch { }

                                if (dtPermissionSavedByIsloaded.Rows.Count == 0)
                                {
                                    try { inpermissionlead.SavedBy = GlobalSettings.ApplicationUser.ID.ToString(); } catch { }
                                }

                                if (dtPermissiondateSavedIsloaded.Rows.Count == 0)
                                {
                                    try { inpermissionlead.DateSaved = DateTime.Now; } catch { }
                                }

                                inpermissionlead.Save(_validationResult);

                                //ShowMessageBox(new INMessageBoxWindow1(), "Lead Permission successfully saved.\n", "Save Result", ShowMessageType.Information);
                            }


                        }
                    }
                }
                catch
                {

                }


                #endregion

                #region Lead

                if (LaData.LeadData.LeadID.HasValue)
                {
                    INLead lead = new INLead((long)LaData.LeadData.LeadID);

                    lead.FKINTitleID = LaData.LeadData.TitleID;
                    lead.Initials = LaData.LeadData.Initials;
                    lead.FirstName = UppercaseFirst(LaData.LeadData.Name);
                    lead.Surname = UppercaseFirst(LaData.LeadData.Surname);
                    lead.IDNo = UppercaseFirst(LaData.LeadData.IDNumber);
                    lead.PassportNo = UppercaseFirst(LaData.LeadData.PassportNumber);
                    lead.DateOfBirth = LaData.LeadData.DateOfBirth;
                    lead.FKGenderID = LaData.LeadData.GenderID;
                    lead.TelHome = LaData.LeadData.TelHome;
                    lead.TelCell = LaData.LeadData.TelCell;
                    lead.TelWork = LaData.LeadData.TelWork;
                    lead.TelOther = LaData.LeadData.TelOther;
                    lead.Address1 = UppercaseFirst(LaData.LeadData.Address1);
                    lead.Address2 = UppercaseFirst(LaData.LeadData.Address2);
                    lead.Address3 = UppercaseFirst(LaData.LeadData.Address3);
                    lead.Address4 = UppercaseFirst(LaData.LeadData.Address4);
                    lead.Address5 = UppercaseFirst(LaData.LeadData.Address5);
                    lead.PostalCode = LaData.LeadData.PostalCode;
                    lead.Email = LaData.LeadData.Email;
                    lead.Save(_validationResult);
                }
                else
                {
                    throw new Exception("leadID Error");
                }

                #endregion

                #region ImportExtra

                INImportExtra inImportExtra = INImportExtraMapper.SearchOne(LaData.AppData.ImportID, null, null, null, null, null, null, null, null) ?? new INImportExtra();

                inImportExtra.FKINImportID = LaData.AppData.ImportID;
                inImportExtra.Extension1 = GetUserExtension(LaData.AppData.AgentID);
                inImportExtra.NotPossibleBumpUp = LaData.ImportExtraData.NotPossibleBumpUp;
                inImportExtra.FKCMCallRefUserID = LaData.SaleData.FKCMCallRefUserID;
                inImportExtra.EMailRequested = LaData.AppData.EMailRequested;
                inImportExtra.CustomerCareRequested = LaData.AppData.CustomerCareRequested;
                inImportExtra.IsGoldenLead = LaData.AppData.IsGoldenLead;
                inImportExtra.Save(_validationResult);

                #endregion

                if (LaData.AppData.CampaignGroup != lkpINCampaignGroup.Extension)//no need for extension
                {
                    #region Bank Details

                    {
                        INBankDetails inBankDetails = (LaData.BankDetailsData.BankDetailsID == null) ? new INBankDetails() : new INBankDetails((long)LaData.BankDetailsData.BankDetailsID);

                        if (LaData.BankDetailsData.PaymentTypeID >= 0)
                        {
                            switch (LaData.BankDetailsData.PaymentType)
                            {
                                case lkpPaymentType.DebitOrder:
                                    inBankDetails.FKPaymentMethodID = LaData.BankDetailsData.PaymentTypeID;

                                    inBankDetails.FKAHTitleID = LaData.BankDetailsData.TitleID;
                                    inBankDetails.AHInitials = LaData.BankDetailsData.Initials;
                                    inBankDetails.AHFirstName = UppercaseFirst(LaData.BankDetailsData.Name);
                                    inBankDetails.AHSurname = UppercaseFirst(LaData.BankDetailsData.Surname);
                                    inBankDetails.AHIDNo = UppercaseFirst(LaData.BankDetailsData.IDNumber);
                                    inBankDetails.AHTelHome = LaData.BankDetailsData.TelHome;
                                    inBankDetails.AHTelCell = LaData.BankDetailsData.TelCell;
                                    inBankDetails.AHTelWork = LaData.BankDetailsData.TelWork;

                                    inBankDetails.FKSigningPowerID = LaData.BankDetailsData.SigningPowerID;

                                    inBankDetails.AccountHolder = UppercaseFirst(LaData.BankDetailsData.AccountHolder);
                                    inBankDetails.FKBankID = LaData.BankDetailsData.BankID;
                                    inBankDetails.FKBankBranchID = LaData.BankDetailsData.BankBranchCodeID;
                                    inBankDetails.AccountNo = LaData.BankDetailsData.AccountNumber;
                                    inBankDetails.FKAccountTypeID = LaData.BankDetailsData.AccountTypeID;
                                    inBankDetails.DebitDay = LaData.BankDetailsData.DebitDay;
                                    inBankDetails.AccNumCheckStatus = Convert.ToByte(LaData.BankDetailsData.lkpAccNumCheckStatus);
                                    inBankDetails.AccNumCheckMsg = LaData.BankDetailsData.AccNumCheckMsg;
                                    inBankDetails.AccNumCheckMsgFull = LaData.BankDetailsData.AccNumCheckMsgFull;
                                    break;
                            }
                            inBankDetails.Save(_validationResult);
                            LaData.BankDetailsData.BankDetailsID = inBankDetails.ID;
                        }
                    }
                    #endregion
                }

                if (LaData.AppData.CampaignGroup != lkpINCampaignGroup.Extension)//no need for extension
                {
                    #region Policy
                    if (LaData.PolicyData.PolicyID.HasValue)
                    {
                        INPolicy policy = new INPolicy(Convert.ToInt32(LaData.PolicyData.PolicyID));

                        policy.CommenceDate = LaData.PolicyData.CommenceDate;
                        policy.FKINBankDetailsID = LaData.BankDetailsData.BankDetailsID;
                        policy.FKINOptionID = LaData.PolicyData.OptionID;
                        policy.FKINOptionFeesID = LaData.PolicyData.OptionFeesID;
                        if (inImportCallMonitoring != null && policy.UDMBumpUpOption == false && LaData.PolicyData.UDMBumpUpOption == true)
                        {
                            inImportCallMonitoring.IsCallMonitored = false;
                        }
                        policy.UDMBumpUpOption = LaData.PolicyData.UDMBumpUpOption;

                        policy.BumpUpAmount = LaData.PolicyData.UDMBumpUpOption ? LaData.PolicyData.BumpUpAmount : null;
                        policy.ReducedPremiumOption = LaData.PolicyData.ReducedPremiumOption;
                        policy.ReducedPremiumAmount = LaData.PolicyData.ReducedPremiumOption ? LaData.PolicyData.ReducedPremiumAmount : null;

                        //policy.FKINBumpUpOptionID = cmbPlatinumBumpUp.SelectedValue as long?;

                        policy.OptionLA2 = LaData.PolicyData.IsLA2Checked;
                        policy.OptionFuneral = LaData.PolicyData.IsFuneralChecked;
                        policy.TotalPremium = LaData.AppData.IsLeadUpgrade ? LaData.PolicyData.UpgradePremium : LaData.PolicyData.TotalPremium;
                        policy.TotalInvoiceFee = LaData.PolicyData.TotalInvoiceFee;
                        policy.FKINOptionFeesID = LaData.PolicyData.OptionFeesID;
                        policy.OptionChild = LaData.AppData.IsLeadUpgrade ? LaData.PolicyData.IsChildUpgradeChecked : LaData.PolicyData.IsChildChecked;

                        policy.BumpUpOffered = LaData.PolicyData.BumpUpOffered;
                        //LaData.PolicyData.BumpUpOffered = Convert.ToBoolean(dtPolicy.Rows[0]["BumpUpOffered"]);
                        //LaData.PolicyData.CanOfferBumpUp = Convert.ToBoolean(dtPolicy.Rows[0]["CanOfferBumpUp"]);

                        policy.Save(_validationResult);
                    }
                    else
                    {
                        throw new Exception("policyID Error");
                    }
                    #endregion
                }

                if (LaData.AppData.CampaignGroup != lkpINCampaignGroup.Extension)//no need for extension
                {
                    #region LA

                    {
                        for (int i = 0; i < LeadApplicationData.MaxLA; i++)
                        {
                            LeadApplicationData.LA LAData = LaData.LAData[i];

                            if (HasEntriesLA(LAData))
                            {
                                INLifeAssured inLifeAssured = (LAData.LifeAssuredID == null) ? new INLifeAssured() : new INLifeAssured((long)LAData.LifeAssuredID);

                                inLifeAssured.FKINTitleID = LAData.TitleID;
                                inLifeAssured.FKGenderID = LAData.GenderID;
                                inLifeAssured.FirstName = UppercaseFirst(LAData.Name);
                                inLifeAssured.Surname = UppercaseFirst(LAData.Surname);
                                inLifeAssured.IDNo = LAData.IDNumber;
                                inLifeAssured.DateOfBirth = LAData.DateOfBirth;
                                inLifeAssured.FKINRelationshipID = LAData.RelationshipID;
                                inLifeAssured.TelContact = LAData.TelContact;
                                inLifeAssured.Save(_validationResult);

                                if (LAData.LifeAssuredID == null) //only if new LA create blank history record
                                {
                                    string strQuery = "INSERT INTO zHstINLifeAssured (ID) VALUES ('" + inLifeAssured.ID + "')";
                                    Methods.ExecuteSQLNonQuery(strQuery);
                                }

                                LAData.LifeAssuredID = inLifeAssured.ID;

                                INPolicyLifeAssured inPolicyLifeAssured = (LAData.PolicyLifeAssuredID == null) ? new INPolicyLifeAssured() : new INPolicyLifeAssured((long)LAData.PolicyLifeAssuredID);
                                inPolicyLifeAssured.FKINPolicyID = LaData.PolicyData.PolicyID;
                                inPolicyLifeAssured.FKINLifeAssuredID = LAData.LifeAssuredID;
                                inPolicyLifeAssured.LifeAssuredRank = i + 1;
                                inPolicyLifeAssured.Save(_validationResult);

                                if (LAData.PolicyLifeAssuredID == null) //only if new LA create blank history record
                                {
                                    string strQuery = "INSERT INTO zHstINPolicyLifeAssured (ID, FKINPolicyID, FKINLifeAssuredID, LifeAssuredRank) VALUES (";
                                    strQuery += "'" + inPolicyLifeAssured.ID + "',";
                                    strQuery += "'" + LaData.PolicyData.PolicyID + "',";
                                    strQuery += "'" + LAData.LifeAssuredID + "',";
                                    strQuery += "'" + inPolicyLifeAssured.LifeAssuredRank + "')";
                                    Methods.ExecuteSQLNonQuery(strQuery);
                                }

                                LAData.PolicyLifeAssuredID = inPolicyLifeAssured.ID;
                            }
                            else
                            {
                                if (LAData.LifeAssuredID != null)
                                {
                                    INPolicyLifeAssuredCollection inPolicyLifeAssuredCollection = INPolicyLifeAssuredMapper.Search(LaData.PolicyData.PolicyID, LAData.LifeAssuredID, i + 1, null);

                                    foreach (INPolicyLifeAssured inPolicyLifeAssured in inPolicyLifeAssuredCollection)
                                    {
                                        inPolicyLifeAssured.Delete(_validationResult);
                                    }

                                    INLifeAssured inLifeAssured = new INLifeAssured((long)LAData.LifeAssuredID);
                                    inLifeAssured.Delete(_validationResult);
                                }
                            }
                        }
                    }

                    #endregion
                }

                if (LaData.AppData.CampaignGroup != lkpINCampaignGroup.Extension)//no need for extension
                {
                    #region Beneficiaries

                    {
                        for (int i = 0; i < LeadApplicationData.MaxBeneficiaries; i++)
                        {
                            LeadApplicationData.Beneficiary BeneficiaryData = LaData.BeneficiaryData[i];
                            if (HasEntriesBeneficiary(BeneficiaryData))
                            {
                                INBeneficiary inBeneficiary = (BeneficiaryData.BeneficiaryID == null) ? new INBeneficiary() : new INBeneficiary((long)BeneficiaryData.BeneficiaryID);

                                inBeneficiary.FKINTitleID = BeneficiaryData.TitleID;
                                inBeneficiary.FKGenderID = BeneficiaryData.GenderID;
                                inBeneficiary.FirstName = UppercaseFirst(BeneficiaryData.Name);
                                inBeneficiary.Surname = UppercaseFirst(BeneficiaryData.Surname);
                                inBeneficiary.DateOfBirth = BeneficiaryData.DateOfBirth;
                                inBeneficiary.FKINRelationshipID = BeneficiaryData.RelationshipID;
                                inBeneficiary.Notes = UppercaseFirst(BeneficiaryData.Notes);
                                inBeneficiary.TelContact = BeneficiaryData.TelContact;
                                inBeneficiary.Save(_validationResult);

                                if (BeneficiaryData.BeneficiaryID == null) //only if new beneficiary create blank history record
                                {
                                    string strQuery = "INSERT INTO zHstINBeneficiary (ID) VALUES ('" + inBeneficiary.ID + "')";
                                    Methods.ExecuteSQLNonQuery(strQuery);
                                }

                                BeneficiaryData.BeneficiaryID = inBeneficiary.ID;

                                INPolicyBeneficiary inPolicyBeneficiary = (BeneficiaryData.PolicyBeneficiaryID == null) ? new INPolicyBeneficiary() : new INPolicyBeneficiary((long)BeneficiaryData.PolicyBeneficiaryID);
                                inPolicyBeneficiary.FKINPolicyID = LaData.PolicyData.PolicyID;
                                inPolicyBeneficiary.FKINBeneficiaryID = BeneficiaryData.BeneficiaryID;
                                inPolicyBeneficiary.BeneficiaryRank = i + 1;
                                inPolicyBeneficiary.BeneficiaryPercentage = BeneficiaryData.Percentage;
                                inPolicyBeneficiary.Save(_validationResult);

                                if (BeneficiaryData.PolicyBeneficiaryID == null) //only if new beneficiary create blank history record
                                {
                                    string strQuery = "INSERT INTO zHstINPolicyBeneficiary (ID, FKINPolicyID, FKINBeneficiaryID, BeneficiaryRank) VALUES (";
                                    strQuery += "'" + inPolicyBeneficiary.ID + "',";
                                    strQuery += "'" + LaData.PolicyData.PolicyID + "',";
                                    strQuery += "'" + BeneficiaryData.BeneficiaryID + "',";
                                    strQuery += "'" + inPolicyBeneficiary.BeneficiaryRank + "')";
                                    Methods.ExecuteSQLNonQuery(strQuery);
                                }

                                BeneficiaryData.PolicyBeneficiaryID = inPolicyBeneficiary.ID;
                            }
                            else
                            {
                                if (BeneficiaryData.BeneficiaryID != null)
                                {
                                    INPolicyBeneficiaryCollection inPolicyBeneficiaryCollection = INPolicyBeneficiaryMapper.Search(LaData.PolicyData.PolicyID, BeneficiaryData.BeneficiaryID, i + 1, null, null);

                                    foreach (INPolicyBeneficiary inPolicyBeneficiary in inPolicyBeneficiaryCollection)
                                    {
                                        inPolicyBeneficiary.Delete(_validationResult);
                                    }

                                    INBeneficiary inBeneficiary = new INBeneficiary((long)BeneficiaryData.BeneficiaryID);
                                    inBeneficiary.Delete(_validationResult);
                                }
                            }
                        }
                    }

                    #endregion
                }

                if (LaData.AppData.CampaignGroup != lkpINCampaignGroup.Extension)//no need for extension
                {
                    #region Children

                    {
                        for (int i = 0; i < LeadApplicationData.MaxChildren; i++)
                        {
                            LeadApplicationData.Child ChildData = LaData.ChildData[i];

                            if (HasEntriesChild(ChildData))
                            {
                                INChild inChild = (ChildData.ChildID == null) ? new INChild() : new INChild((long)ChildData.ChildID);
                                inChild.DateOfBirth = ChildData.DateOfBirth;
                                inChild.Save(_validationResult);

                                if (ChildData.ChildID == null) //only if new child create blank history record
                                {
                                    string strQuery = "INSERT INTO zHstINChild (ID) VALUES ('" + inChild.ID + "')";
                                    Methods.ExecuteSQLNonQuery(strQuery);
                                }

                                ChildData.ChildID = inChild.ID;

                                INPolicyChild inPolicyChild = (ChildData.PolicyChildID == null) ? new INPolicyChild() : new INPolicyChild((long)ChildData.PolicyChildID);
                                inPolicyChild.FKINPolicyID = LaData.PolicyData.PolicyID;
                                inPolicyChild.FKINChildID = inChild.ID;
                                inPolicyChild.ChildRank = i + 1;
                                inPolicyChild.Save(_validationResult);

                                if (ChildData.PolicyChildID == null) //only if new child create blank history record
                                {
                                    string strQuery = "INSERT INTO zHstINPolicyChild (ID, FKINPolicyID, FKINChildID, ChildRank) VALUES (";
                                    strQuery += "'" + inPolicyChild.ID + "',";
                                    strQuery += "'" + LaData.PolicyData.PolicyID + "',";
                                    strQuery += "'" + ChildData.ChildID + "',";
                                    strQuery += "'" + inPolicyChild.ChildRank + "')";
                                    Methods.ExecuteSQLNonQuery(strQuery);
                                }
                                ChildData.PolicyChildID = inPolicyChild.ID;
                            }
                            else
                            {
                                if (ChildData.PolicyChildID != null)
                                {
                                    INPolicyChild inPolicyChild = new INPolicyChild((long)ChildData.PolicyChildID);
                                    inPolicyChild.Delete(_validationResult);
                                }
                                if (ChildData.ChildID != null)
                                {
                                    INChild inChild = new INChild((long)ChildData.ChildID);
                                    inChild.Delete(_validationResult);
                                }
                            }
                        }
                    }

                    #endregion
                }

                #region NextOfKin

                {
                    for (int i = 0; i < LeadApplicationData.MaxNextOfKin; i++)
                    {
                        LeadApplicationData.NextOfKin NextOfKinData = LaData.NextOfKinData[i];

                        INNextOfKin inNextOfKin = (NextOfKinData.NextOfKinID == null) ? new INNextOfKin() : new INNextOfKin((long)NextOfKinData.NextOfKinID);

                        inNextOfKin.FKINImportID = LaData.AppData.ImportID;
                        inNextOfKin.FirstName = UppercaseFirst(NextOfKinData.Name);
                        inNextOfKin.Surname = UppercaseFirst(NextOfKinData.Surname);
                        inNextOfKin.FKINRelationshipID = NextOfKinData.RelationshipID;
                        inNextOfKin.TelContact = NextOfKinData.TelContact;
                        inNextOfKin.Save(_validationResult);

                        if (NextOfKinData.NextOfKinID == null) //only if new NextOfKin create blank history record
                        {
                            string strQuery = "INSERT INTO zHstINNextOfKin (ID) VALUES ('" + inNextOfKin.ID + "')";
                            Methods.ExecuteSQLNonQuery(strQuery);
                        }

                        NextOfKinData.NextOfKinID = inNextOfKin.ID;
                    }
                }

                #endregion



                #region Call Monitoring Details
                if (inImportCallMonitoring != null)
                {
                    //This was commented out because Brigette and Kashmira agreed that the expiry date should remain the same with "Call Search In Process" call monitoring outcomes 
                    //as the the carried forward should be resolved on the same day.
                    //if (inImportCallMonitoring.FKINCallMonitoringOutcomeID == 3 && (inImportCallMonitoring.IsRecoveredSale == false || inImportCallMonitoring.IsRecoveredSale == null) && (lkpUserType?)((User)GlobalSettings.ApplicationUser).FKUserType != lkpUserType.SalesAgent && (lkpINLeadStatus?)LaData.AppData.LeadStatus == lkpINLeadStatus.Accepted)
                    //{
                    //    DataTable dtCallMonitoringAllocations = Methods.GetTableData("SELECT * FROM CallMonitoringAllocation WHERE FKINImportID = " + LaData.AppData.ImportID);
                    //    //CallMonitoringAllocationCollection callMonitoringAllocationCollection = CallMonitoringAllocationMapper.Search(LaData.AppData.ImportID, null, null, null, null);
                    //    foreach (DataRow dr in dtCallMonitoringAllocations.Rows)
                    //    {
                    //        //CallMonitoringAllocation cma = new CallMonitoringAllocation(Convert.ToInt64(dr["ID"]));
                    //        SqlParameter[] parameters = new SqlParameter[1];
                    //        //parameters[0] = new SqlParameter("@ImportID", LaData.AppData.ImportID);
                    //        parameters[0] = new SqlParameter("@DateOfSale", LaData.AppData.DateOfSale);
                    //        DateTime ExpiryDate = Convert.ToDateTime(Methods.ExecuteFunction("fnGetCMSaleExpiryDateDOS", parameters));
                    //        if (ExpiryDate != null)
                    //        {
                    //            Methods.ExecuteSQLNonQuery("UPDATE CallMonitoringAllocation SET ExpiryDate = '" + ExpiryDate + "' WHERE ID = " + Convert.ToInt64(dr["ID"]));
                    //        }

                    //        //cma.Save(_validationResult);
                    //    }

                    //}

                    inImportCallMonitoring.Save(_validationResult);
                }



                #endregion


                CommitTransaction("");



                switch (LaData.AppData.LeadStatus)
                {
                    case 1:
                        if (LaData.UserData.UserTypeID == 2)
                        {
                            //CommitTransaction("");

                            INMessageBoxWindow1 inMessageBoxWindow = new INMessageBoxWindow1();

                            SolidColorBrush brush = new SolidColorBrush((Color)FindResource("BrandedColourIN"));
                            inMessageBoxWindow.txtDescription.Foreground = brush;

                            ColorAnimation anima = new ColorAnimation((Color)FindResource("BrandedColourIN"), Colors.White, new Duration(TimeSpan.FromMilliseconds(300)));
                            anima.AutoReverse = true;
                            anima.RepeatBehavior = RepeatBehavior.Forever;
                            brush.BeginAnimation(SolidColorBrush.ColorProperty, anima);

                            DoubleAnimation siz = new DoubleAnimation(18, 24, new Duration(TimeSpan.FromSeconds(2)));
                            siz.AutoReverse = true;
                            siz.RepeatBehavior = RepeatBehavior.Forever;
                            inMessageBoxWindow.txtDescription.BeginAnimation(FontSizeProperty, siz);

                            //inMessageBoxWindow.txtDescription.FontSize = 24;
                            inMessageBoxWindow.txtDescription.FontFamily = new FontFamily("Arial");
                            ShowMessageBox(inMessageBoxWindow, "Great job! This is a completed sale.\n", "Save Result", ShowMessageType.Information);
                        }
                        else
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "Lead successfully saved.\n", "Save Result", ShowMessageType.Information);
                        }
                        break;

                    default:
                        CommitTransaction("");
                        ShowMessageBox(new INMessageBoxWindow1(), "Lead successfully saved.\n", "Save Result", ShowMessageType.Information);
                        break;
                }

                ClearApplicationScreen();
                medReference.Focus();
            }

            catch (Exception ex)
            {
                Database.CancelTransactions();
                HandleException(ex);

                ClearApplicationScreen();
                medReference.Focus();
            }

            finally
            {
                //LogInfo();
                //if (LaData.CTPhoneData.IsAgentLoggedIn && LaData.CTPhoneData.RecRef != string.Empty)
                //{
                //    _axCtPhone.UpdateRecording2(LaData.CTPhoneData.RecRef, LaData.AppData.CampaignCode, LaData.AppData.PlatinumBatchCode, LaData.AppData.LeadStatus, LaData.AppData.DeclineReasonID, LaData.AppData.DiaryReasonID, LaData.AppData.ImportID, string.Empty);
                //    _axCtPhone.RescheduleDialerLeadExUCID("19000", null, LaData.AppData.RefNo, null, null, 100, LaData.AppData.LeadStatus, null, null);
                //}

                SetCursor(Cursors.Arrow);
            }
        }

        private void ShowToolTip(ToolTip toolTip)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Tick += delegate
            {
                toolTip.IsOpen = false;
                timer.Stop();
            };

            toolTip.Opened += delegate
            {
                toolTip.StaysOpen = false;
            };

            toolTip.Closed += delegate
            {
                toolTip.IsOpen = false;
                timer.Stop();
                toolTip.Content = null;
                toolTip.StaysOpen = true;
            };

            toolTip.IsOpen = true;
            timer.Start();
        }

        private bool IsValidData()
        {
            try
            {
                //validation: also done in xaml save button style
                if (LaData.AppData.LeadStatus == 1)
                {
                    #region Beneficiaries Percentages Total

                    if (!Convert.ToBoolean(lblBeneficiaryPercentageTotalUpg.Tag) || !Convert.ToBoolean(lblBeneficiaryPercentageTotalBase.Tag))
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "The beneficiary percentages do not add up to 100%.\n\nLead not saved.", "Beneficiary Percentages", ShowMessageType.Error);
                        return false;
                    }

                    #endregion Beneficiaries Percentages Total

                    #region Rule: Any option that includes Funeral cover () LA1 Funeral cover or LA1 Accidental Death cover (Upgrades) must have a beneficiary percentage of 100%

                    decimal beneficiaryTotalPercentage = 0;

                    if (LaData.AppData.IsLeadUpgrade)
                    {
                        if ((_la1AccidentalDeathCover + _la1FuneralCover) > 0)
                        {
                            //if (!Convert.ToBoolean(lblBeneficiaryPercentageTotalUpg.Tag) || !Convert.ToBoolean(lblBeneficiaryPercentageTotalBase.Tag))

                            beneficiaryTotalPercentage = Convert.ToDecimal(lblBeneficiaryPercentageTotalUpg.Text);
                            if (beneficiaryTotalPercentage != 100m)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "The lead cannot be saved, because a Life Assured 1 Cover option was selected that includes LA1 Funeral and/or LA1 Accidental Death cover, but the beneficiary percentages do not add up to 100%.", "Beneficiary Percentages", ShowMessageType.Error);
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (_funeralCover > 0)
                        {

                            //if (!Convert.ToBoolean(lblBeneficiaryPercentageTotalUpg.Tag) || !Convert.ToBoolean(lblBeneficiaryPercentageTotalBase.Tag))
                            beneficiaryTotalPercentage = Convert.ToDecimal(lblBeneficiaryPercentageTotalBase.Text);
                            if (beneficiaryTotalPercentage != 100m)
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "The lead cannot be saved, because a Life Assured 1 Cover option was selected that includes Funeral cover, but the beneficiary percentages do not add up to 100%.", "Beneficiary Percentages", ShowMessageType.Error);
                                return false;
                            }
                        }

                    }

                    #endregion Rule: Any option that includes Funeral cover () LA1 Funeral cover or LA1 Accidental Death cover (Upgrades) must have a beneficiary percentage of 100%
                }


                #region Lead Feedback

                {
                    if (cmbLeadFeedback.SelectedIndex == -1)
                    {
                        if (!LaData.AppData.IsLeadUpgrade)
                        {
                            if (LaData.AppData.CampaignGroup != lkpINCampaignGroup.Extension)
                            {
                                bool? result = ShowMessageBox(new INMessageBoxWindow2(), "Would you like to provide Lead Feedback?", "Lead Feedback", ShowMessageType.Information);

                                if (Convert.ToBoolean(result))
                                {
                                    GotoPage(Page5);
                                    cmbLeadFeedback.Focus();

                                    ToolTip cmbLeadFeedbackToolTip = (ToolTip)cmbLeadFeedback.ToolTip;

                                    cmbLeadFeedbackToolTip.Content = "lead feedback required";
                                    cmbLeadFeedbackToolTip.PlacementTarget = cmbLeadFeedback;
                                    cmbLeadFeedbackToolTip.Placement = PlacementMode.Right;
                                    cmbLeadFeedbackToolTip.VerticalOffset = -80;
                                    cmbLeadFeedbackToolTip.HorizontalOffset = -120;

                                    ShowToolTip(cmbLeadFeedbackToolTip);
                                    return false;
                                }

                                LaData.SaleData.LeadFeedbackID = (long)lkpLeadFeedback.NoFeedback;
                            }
                        }
                    }
                }

                #endregion

                #region Confirmation Feedback

                if (LaData.SaleData.ConfirmationFeedbackID == null)
                {
                    if (LaData.UserData.UserType == lkpUserType.ConfirmationAgent)
                    {
                        if (LaData.AppData.CampaignGroup != lkpINCampaignGroup.Extension)
                        {
                            bool? result = ShowMessageBox(new INMessageBoxWindow2(), "Would you like to provide Confirmation Feedback?", "Confirmation Feedback", ShowMessageType.Information);

                            if (Convert.ToBoolean(result))
                            {
                                GotoPage(Page5);
                                cmbConfirmationFeedback.Focus();

                                ToolTip cmbConfirmationFeedbackToolTip = (ToolTip)cmbConfirmationFeedback.ToolTip;

                                cmbConfirmationFeedbackToolTip.Content = "confirmation feedback required";
                                cmbConfirmationFeedbackToolTip.PlacementTarget = cmbConfirmationFeedback;
                                cmbConfirmationFeedbackToolTip.Placement = PlacementMode.Right;
                                cmbConfirmationFeedbackToolTip.VerticalOffset = -80;
                                cmbConfirmationFeedbackToolTip.HorizontalOffset = -120;

                                ShowToolTip(cmbConfirmationFeedbackToolTip);
                                return false;
                            }

                            LaData.SaleData.ConfirmationFeedbackID = (long)lkpConfirmationFeedback.NoFeedback;
                        }
                    }
                }

                #endregion

                return true;
            }

            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }

        private void GotoPage(Border page)
        {
            Border visiblePage = _pages.First(b => b.IsVisible.Equals(true));

            if (!Equals(page, visiblePage))
            {
                visiblePage.Visibility = Visibility.Collapsed;
                page.Visibility = Visibility.Visible;

                switch (page.Name)
                {
                    case "Page1":
                        lblPage.Text = "(Lead)";
                        break;

                    case "Page2":
                        lblPage.Text = "(Policy)";
                        break;

                    case "Page3":
                        lblPage.Text = "(Banking)";
                        break;

                    case "Page4":
                        lblPage.Text = "(LA + Beneficiary)";
                        break;

                    case "Page5":
                        lblPage.Text = "(Sale)";
                        break;

                    case "ClosurePage":
                        lblPage.Text = "(Closure)";
                        break;
                }

                medReference.Focus();
            }
        }

        private void ClearApplicationScreen()
        {
            try
            {
                SetCursor(Cursors.Wait);
                LaData.AppData.IsLeadLoaded = false;
                CloseScriptWindows();

                string strRefNo = LaData.AppData.RefNo;
                long? importIDNextLead = LaData.AppData2.ImportIDNextLead;
                long? ImportIDPrevLead = LaData.AppData2.ImportIDPrevLead;

                UnLoadUpgradeLead();

                LaData.Clear();

                ResetDisplayedCosts();
                ClearClosureDocument();

                cmbLA1Cover.ItemsSource = null;
                cmbUpgradeCover.ItemsSource = null;
                cmbPolicyPlanGroup.ItemsSource = null;
                LaData.LeadData.DateOfBirth = null;

                LaData.AppData.RefNo = strRefNo;
                LaData.AppData2.ImportIDNextLead = importIDNextLead;
                LaData.AppData2.ImportIDPrevLead = ImportIDPrevLead;

                medLA2NameUpg.IsEnabled = true;
                medLA2SurnameUpg.IsEnabled = true;
                dteLA2DateOfBirthUpg.IsEnabled = true;
                cmbLA2TitleUpg.IsEnabled = true;
                cmbLA2GenderUpg.IsEnabled = true;
                cmbLA2RelationshipUpg.IsEnabled = true;
                //medLA2ContactPhoneUpg.IsEnabled = true;

                //if ((LaData.AppData.LoadedRefNo != null) && (LaData.AppData.IsLeadLoaded))
                //{
                _hasNoteBeenDisplayed = false;
                //}

                chkLA2.IsEnabled = true;
                chkChild.IsEnabled = true;
                chkFuneral.IsEnabled = false;

                dteCommenceDate.ToolTip = null;

                _la1AccidentalDeathCover = 0;
                _funeralCover = 0;
                _la1FuneralCover = 0;
                chkShowAllOptions.IsChecked = false;

                grdBeneficiaries.Visibility = Visibility.Collapsed;
                grdBenficiariesPercentageTotalBase.Visibility = Visibility.Collapsed;

                SetDialHoursGraph(stpWorkPhone, string.Empty);
                SetDialHoursGraph(stpHomePhone, string.Empty);
                SetDialHoursGraph(stpCellPhone, string.Empty);
                SetDialHoursGraph(stpOtherPhone, string.Empty);

                MainBorder.BorderBrush = (Brush)FindResource("BrandedBrushIN");
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SetCursor(Cursors.Arrow);
            }
        }

        private void LoadLookupData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@UserTypeID", LaData.UserData.UserTypeID);
                DataSet dsLookups = Methods.ExecuteStoredProcedure("spINGetLeadApplicationScreenLookups", parameters);



                DataTable dtStatus = dsLookups.Tables[0];


                cmbStatus.Populate(dtStatus, DescriptionField, IDField);

                parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ImportID", (object)0);
                DataSet dsAgentLookups = Methods.ExecuteStoredProcedure("spINGetLeadApplicationScreenAgents", parameters);

                DataTable dtSalesAgents = dsAgentLookups.Tables[0];
                cmbAgent.Populate(dtSalesAgents, "Name", IDField);
                cmbSaleCallRef.Populate(dtSalesAgents, "Name", IDField);
                cmbBankCallRef.Populate(dtSalesAgents, "Name", IDField);

                DataTable dtTitles = dsLookups.Tables[2];
                cmbTitle.Populate(dtTitles, DescriptionField, IDField);
                cmbDOTitle.Populate(dtTitles, DescriptionField, IDField);
                cmbLA1Title.Populate(dtTitles, DescriptionField, IDField);

                cmbLA2Title.Populate(dtTitles, DescriptionField, IDField);
                cmbLA2TitleUpg.Populate(dtTitles, DescriptionField, IDField);

                cmbReferrorTitle.Populate(dtTitles, DescriptionField, IDField);

                cmbBeneficiaryTitle1.Populate(dtTitles, DescriptionField, IDField);
                cmbBeneficiaryTitle1Upg.Populate(dtTitles, DescriptionField, IDField);
                cmbBeneficiaryTitle2.Populate(dtTitles, DescriptionField, IDField);
                cmbBeneficiaryTitle2Upg.Populate(dtTitles, DescriptionField, IDField);
                cmbBeneficiaryTitle3.Populate(dtTitles, DescriptionField, IDField);
                cmbBeneficiaryTitle3Upg.Populate(dtTitles, DescriptionField, IDField);
                cmbBeneficiaryTitle4.Populate(dtTitles, DescriptionField, IDField);
                cmbBeneficiaryTitle4Upg.Populate(dtTitles, DescriptionField, IDField);
                cmbBeneficiaryTitle5.Populate(dtTitles, DescriptionField, IDField);
                cmbBeneficiaryTitle5Upg.Populate(dtTitles, DescriptionField, IDField);
                cmbBeneficiaryTitle6.Populate(dtTitles, DescriptionField, IDField);
                cmbBeneficiaryTitle6Upg.Populate(dtTitles, DescriptionField, IDField);

                DataTable dtPaymentType = dsLookups.Tables[3];
                cmbPaymentType.Populate(dtPaymentType, DescriptionField, IDField);
                //cmbPaymentType.SelectedValue = (long?)lkpPaymentType.DebitOrder;

                DataTable dtBank = dsLookups.Tables[4];
                cmbDOBank.Populate(dtBank, DescriptionField, IDField);

                DataTable dtAccountType = dsLookups.Tables[5];
                cmbDOAccountType.Populate(dtAccountType, DescriptionField, IDField);

                DataTable dtLARelationship = dsLookups.Tables[16];
                cmbLA1Relationship.Populate(dtLARelationship, DescriptionField, IDField);
                cmbLA2Relationship.Populate(dtLARelationship, DescriptionField, IDField);
                cmbLA2RelationshipUpg.Populate(dtLARelationship, DescriptionField, IDField);
                cmbNOKRelationship.Populate(dtLARelationship, DescriptionField, IDField);
                cmbNOKRelationship2.Populate(dtLARelationship, DescriptionField, IDField);
                cmbNOKRelationshipUpgrade.Populate(dtLARelationship, DescriptionField, IDField);

                DataTable dtBeneficiaryRelationship = dsLookups.Tables[17];
                cmbBeneficiaryRelationship1.Populate(dtBeneficiaryRelationship, DescriptionField, IDField);
                cmbBeneficiaryRelationship1Upg.Populate(dtBeneficiaryRelationship, DescriptionField, IDField);
                cmbBeneficiaryRelationship2.Populate(dtBeneficiaryRelationship, DescriptionField, IDField);
                cmbBeneficiaryRelationship2Upg.Populate(dtBeneficiaryRelationship, DescriptionField, IDField);
                cmbBeneficiaryRelationship3.Populate(dtBeneficiaryRelationship, DescriptionField, IDField);
                cmbBeneficiaryRelationship3Upg.Populate(dtBeneficiaryRelationship, DescriptionField, IDField);
                cmbBeneficiaryRelationship4.Populate(dtBeneficiaryRelationship, DescriptionField, IDField);
                cmbBeneficiaryRelationship4Upg.Populate(dtBeneficiaryRelationship, DescriptionField, IDField);
                cmbBeneficiaryRelationship5.Populate(dtBeneficiaryRelationship, DescriptionField, IDField);
                cmbBeneficiaryRelationship5Upg.Populate(dtBeneficiaryRelationship, DescriptionField, IDField);
                cmbBeneficiaryRelationship6.Populate(dtBeneficiaryRelationship, DescriptionField, IDField);
                cmbBeneficiaryRelationship6Upg.Populate(dtBeneficiaryRelationship, DescriptionField, IDField);

                DataTable dtRelationship = dsLookups.Tables[6];
                cmbReferrorRelationship.Populate(dtRelationship, DescriptionField, IDField);

                DataTable dtGender = dsLookups.Tables[7];
                cmbLA1Gender.Populate(dtGender, DescriptionField, IDField);
                cmbLA2Gender.Populate(dtGender, DescriptionField, IDField);
                cmbLA2GenderUpg.Populate(dtGender, DescriptionField, IDField);
                cmbGender.Populate(dtGender, DescriptionField, IDField);
                cmbBeneficiary1Gender.Populate(dtGender, DescriptionField, IDField);
                cmbBeneficiaryGender1Upg.Populate(dtGender, DescriptionField, IDField);
                cmbBeneficiary2Gender.Populate(dtGender, DescriptionField, IDField);
                cmbBeneficiaryGender2Upg.Populate(dtGender, DescriptionField, IDField);
                cmbBeneficiary3Gender.Populate(dtGender, DescriptionField, IDField);
                cmbBeneficiaryGender3Upg.Populate(dtGender, DescriptionField, IDField);
                cmbBeneficiary4Gender.Populate(dtGender, DescriptionField, IDField);
                cmbBeneficiaryGender4Upg.Populate(dtGender, DescriptionField, IDField);
                cmbBeneficiary5Gender.Populate(dtGender, DescriptionField, IDField);
                cmbBeneficiaryGender5Upg.Populate(dtGender, DescriptionField, IDField);
                cmbBeneficiary6Gender.Populate(dtGender, DescriptionField, IDField);
                cmbBeneficiaryGender6Upg.Populate(dtGender, DescriptionField, IDField);

                DataTable dtPolicyType = dsLookups.Tables[8];
                cmbPolicyType.Populate(dtPolicyType, DescriptionField, IDField);

                //DataTable dtTownCity = dsLookups.Tables[9];
                //ecbTown.Populate(dtTownCity, DescriptionField, IDField);

                // Signing Power
                DataTable dtSigningPower = dsLookups.Tables[10];
                cmbDOSigningPower.Populate(dtSigningPower, DescriptionField, IDField);

                DataTable dtPolicyGroup = dsLookups.Tables[12];
                cmbPolicyGroup.Populate(dtPolicyGroup, DescriptionField, IDField);

                //DataTable dtPolicyPlanGroup = dsLookups.Tables[13];
                //cmbPolicyPlanGroup.Populate(dtPolicyPlanGroup, DescriptionField, IDField);

                DataTable dtConfirmationAgents = dsLookups.Tables[14];
                cmbConfCallRef.Populate(dtConfirmationAgents, "Name", IDField);
                cmbBatchCallRef.Populate(dtConfirmationAgents, "Name", IDField);

                //DataTable dtDataCapturers = dsLookups.Tables[15];

                DataTable dtLeadFeedback = dsLookups.Tables[18];
                cmbLeadFeedback.Populate(dtLeadFeedback, DescriptionField, IDField);

                DataTable dtConfirmationFeedback = dsLookups.Tables[19];
                cmbConfirmationFeedback.Populate(dtConfirmationFeedback, DescriptionField, IDField);

                DataTable dtCMCallRefUsers = dsLookups.Tables[20];
                cmbCMCallRef.Populate(dtCMCallRefUsers, "Name", IDField);

                // Ensure a payment type is selected, this will show the account details.
                //if (cmbPaymentType.Items.Count > 0)
                //{
                //    cmbPaymentType.SelectedIndex = 0;
                //}
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SetCursor(Cursors.Arrow);
            }
        }

        private void xamEditor_Select(object sender)
        {
            switch (sender.GetType().Name)
            {
                case "XamMaskedEditor":
                    var xamMEDControl = (XamMaskedEditor)sender;

                    switch (xamMEDControl.Name)
                    {
                        default:
                            xamMEDControl.SelectAll();
                            break;
                    }
                    break;

                case "XamDateTimeEditor":
                    var xamDTEControl = (XamDateTimeEditor)sender;

                    switch (xamDTEControl.Name)
                    {
                        default:
                            if (xamDTEControl.IsInEditMode)
                            {
                                xamDTEControl.SelectAll();
                            }
                            break;
                    }
                    break;
            }
        }

        //private void xamEditor_SetMask(object sender)
        //{
        //    switch (sender.GetType().Name)
        //    {
        //        case "XamMaskedEditor":
        //            var xamMEDControl = (XamMaskedEditor)sender;

        //            switch (xamMEDControl.Name)
        //            {
        //                case "medHomePhone":
        //                case "medCellPhone":
        //                case "medWorkPhone":
        //                case "medOtherPhone":
        //                case "medDOHomePhone":
        //                case "medDOCellPhone":
        //                case "medDOWorkPhone":
        //                    xamMEDControl.Mask = (string.IsNullOrEmpty(xamMEDControl.Text)) ? "##########" : "###-###-####";
        //                    break;

        //                default:
        //                    IEnumerable<string> strCompare = new[] { new string(xamMEDControl.Name.Take(26).ToArray()) };
        //                    if (strCompare.Contains("medBeneficiaryContactPhone"))
        //                    {
        //                        xamMEDControl.Mask = (string.IsNullOrEmpty(xamMEDControl.Text)) ? "##########" : "###-###-####";
        //                    }
        //                    break;
        //            }

        //            break;
        //    }
        //}

        //private long? DeterminePolicyPlanID()
        //{
        //    //DataTable dt = Methods.GetTableData("SELECT ID FROM INPlan WHERE FKINPolicyTypeID = '" + _policyTypeID + "' AND '" + _leadAge + "' BETWEEN AgeMin AND AgeMax AND IsActive = '1'");
        //    //DataTable dt = Methods.GetTableData("SELECT ID FROM INPlan WHERE FKINPolicyTypeID = '" + _policyTypeID + "' AND PlanCode = 'Option " + _platinumPlan + "'");

        //    //if (dt.Rows.Count > 0)
        //    //{
        //    //    return (long?)dt.Rows[0]["ID"];
        //    //}

        //    return null;
        //}

        //private long? DetermineMoneyBackID()
        //{
        //    try
        //    {
        //        DataTable dt = Methods.GetTableData("SELECT ID FROM INMoneyBack WHERE FKINOptionID = '" + _laData.OptionID + "' AND IsActive ='1' AND '" + _leadAge + "' BETWEEN AgeMin AND AgeMax");

        //        if (dt.Rows.Count == 1)
        //        {
        //            return (long?)dt.Rows[0]["ID"];
        //        }

        //        return null;
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //        return null;
        //    }
        //}

        private decimal? GetMoneyBackPayout(int policyYear, int birthYear, decimal? premium)
        {
            try
            {
                string strQuery = string.Format("SELECT (dbo.INGetMoneyBackPayout('{0}', '{1}', '{2}'))", policyYear, birthYear, premium);

                DataTable dt = Methods.GetTableData(strQuery);

                return dt.Rows[0][0] as decimal?;
            }

            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        private int? GetMoneyBackPayoutAge(DateTime? birthDate, DateTime? commenceDate)
        {
            try
            {
                if (birthDate != null)
                {
                    int age = DateTime.Now.Year - ((DateTime)birthDate).Year - (DateTime.Now.DayOfYear < ((DateTime)birthDate).DayOfYear ? 1 : 0);

                    if (age >= 18 && age < 45)
                    {
                        return 65;
                    }
                    //if (age == 45)
                    //{
                    //    if (commenceDate != null && (new DateTime(DateTime.Now.Year, ((DateTime)birthDate).Month, ((DateTime)birthDate).Day) < new DateTime(DateTime.Now.Year, ((DateTime)commenceDate).Month, ((DateTime)commenceDate).Day)))
                    //    {
                    //        return 66;
                    //    }

                    //    return 65;
                    //}
                    if (age >= 45 && age <= 80)
                    {
                        return age + 20;
                    }

                    return age;
                }

                return null;
            }

            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        private void ResetDisplayedCosts()
        {
            xamCELA1Cost.Value = 0.00m;
            xamCELA1Cover.Value = 0.00m;
            xamCELA2Cost.Value = 0.00m;
            xamCELA2Cover.Value = 0.00m;
            xamCEChildCost.Value = 0.00m;
            xamCEChildCover.Value = 0.00m;
            xamCEFuneralCostLA1.Value = 0.00m;
            xamCEFuneralCoverLA1.Value = 0.00m;
            xamCEFuneralCostLA2.Value = 0.00m;
            xamCEFuneralCoverLA2.Value = 0.00m;
            xamCELA1LA2TotalCover.Value = 0.00m;
            xamCEIGLA1FreeCover.Value = 0.00m;
            xamCE50PercViolenceBenefit.Value = 0.00m;

            lbl50PercViolenceBenefit.Visibility = Visibility.Collapsed;
            xamCE50PercViolenceBenefit.Visibility = Visibility.Collapsed;
            lblIGLA1FreeCover.Visibility = Visibility.Collapsed;
            xamCEIGLA1FreeCover.Visibility = Visibility.Collapsed;
            //xamCEFuneralCost.Visibility = Visibility.Collapsed;
            //xamCEFuneralCover.Visibility = Visibility.Collapsed;

            grdBaseLine3.Visibility = Visibility.Collapsed;
            grdBaseLine4.Visibility = Visibility.Collapsed;

            lblLA1CostCover.Text = "Life Assured 1";
            lblLA2CostCover.Text = "Life Assured 2";
            lblLA1CostCoverOther.Text = "Life Assured 1";
            lblLA2CostCoverOther.Text = "Life Assured 2";
            lblChildCostCover.Text = "Child";

            lblFuneralCostCoverLA1.Visibility = Visibility.Collapsed;
            xamCEFuneralCostLA1.Visibility = Visibility.Collapsed;
            xamCEFuneralCoverLA1.Visibility = Visibility.Collapsed;

            lblFuneralCostCoverLA2.Visibility = Visibility.Collapsed;
            xamCEFuneralCostLA2.Visibility = Visibility.Collapsed;
            xamCEFuneralCoverLA2.Visibility = Visibility.Collapsed;
        }

        //check bumpup only
        private void CalculateCost(bool checkBumpup)
        {
            try
            {
                if (LaData.AppData.ImportID == null) return;

                if (LaData.PolicyData.OptionID != null)
                {
                    DataTable dtOption = Methods.GetTableData("SELECT * FROM INOption WHERE ID = '" + LaData.PolicyData.OptionID + "'");
                    DataTable dtOptionExtra = Methods.GetTableData("SELECT * FROM INOptionExtra WHERE FKOptionID = '" + LaData.PolicyData.OptionID + "'");

                    LaData.PolicyData.TotalPremium = 0;

                    if (dtOption.Rows.Count == 1)
                    {
                        decimal LA1Cost = Convert.ToDecimal(NoNull(dtOption.Rows[0]["LA1Cost"], 0));
                        xamCELA1Cost.Value = LA1Cost;
                        decimal LA1Cover = Convert.ToDecimal(NoNull(dtOption.Rows[0]["LA1Cover"], 0));
                        xamCELA1Cover.Value = LA1Cover;
                        LaData.PolicyData.TotalPremium = LaData.PolicyData.TotalPremium + LA1Cost;

                        decimal LA2Cover = 0.00m;
                        if (LaData.PolicyData.IsLA2Checked)
                        {
                            decimal LA2Cost = Convert.ToDecimal(NoNull(dtOption.Rows[0]["LA2Cost"], 0));
                            xamCELA2Cost.Value = LA2Cost;
                            LA2Cover = Convert.ToDecimal(NoNull(dtOption.Rows[0]["LA2Cover"], 0));
                            xamCELA2Cover.Value = LA2Cover;
                            LaData.PolicyData.TotalPremium = LaData.PolicyData.TotalPremium + LA2Cost;
                        }

                        decimal LA1LA2TotalCover = LA1Cover + LA2Cover;


                        decimal ChildCost = 0.00m;
                        decimal ChildCover = 0.00m;
                        if (LaData.PolicyData.IsChildChecked)
                        {
                            if (LaData.AppData.CampaignType == lkpINCampaignType.FemaleDisCancer)
                            {
                                string optionCode = dtOption.Rows[0]["OptionCode"] as string;
                                string[] disabilityCodes = { "AAA", "BBB", "CCC", "DDD", "EEE", "aaa", "bbb", "ccc", "ddd", "eee" };
                                if (disabilityCodes.Contains(optionCode.Substring(optionCode.Length - 3)))
                                {
                                    lblChildCostCover.Text = "Child (Disability)";
                                }
                                else
                                {
                                    lblChildCostCover.Text = "Child (Cancer)";
                                }
                            }
                            else
                            {
                                lblChildCostCover.Text = "Child";
                            }

                            ChildCost = Convert.ToDecimal(NoNull(dtOption.Rows[0]["ChildCost"], 0));
                            xamCEChildCost.Value = ChildCost;
                            ChildCover = Convert.ToDecimal(NoNull(dtOption.Rows[0]["ChildCover"], 0));
                            xamCEChildCover.Value = ChildCover;
                            LaData.PolicyData.TotalPremium = LaData.PolicyData.TotalPremium + ChildCost;
                        }
                        else
                        {
                            lblChildCostCover.Text = "Child";
                            xamCEChildCost.Value = 0.00m;
                            xamCEChildCover.Value = 0.00m;
                        }

                        decimal FuneralCostLA1 = 0.00m;
                        decimal FuneralCoverLA1 = 0.00m;
                        decimal FuneralCostLA2 = 0.00m;
                        decimal FuneralCoverLA2 = 0.00m;
                        if (LaData.PolicyData.IsFuneralChecked)
                        {
                            FuneralCostLA1 = LaData.AppData.CampaignType == lkpINCampaignType.FemaleDisCancer
                                ? Convert.ToDecimal(NoNull(dtOption.Rows[0]["LA1FuneralCost"], 0))
                                : Convert.ToDecimal(NoNull(dtOption.Rows[0]["FuneralCost"], 0));
                            xamCEFuneralCostLA1.Value = FuneralCostLA1;

                            FuneralCoverLA1 = Convert.ToDecimal(NoNull(dtOption.Rows[0]["FuneralCover"], 0));
                            FuneralCoverLA1 = LaData.AppData.CampaignType == lkpINCampaignType.FemaleDisCancer
                                ? Convert.ToDecimal(NoNull(dtOption.Rows[0]["LA1FuneralCover"], 0))
                                : Convert.ToDecimal(NoNull(dtOption.Rows[0]["FuneralCover"], 0));
                            _funeralCover = Convert.ToInt32(NoNull(dtOption.Rows[0]["FuneralCover"], 0));
                            xamCEFuneralCoverLA1.Value = FuneralCoverLA1;

                            LaData.PolicyData.TotalPremium = LaData.PolicyData.TotalPremium + FuneralCostLA1;

                            FuneralCostLA2 = Convert.ToDecimal(NoNull(dtOption.Rows[0]["LA2FuneralCost"], 0));
                            xamCEFuneralCostLA2.Value = FuneralCostLA2;
                            FuneralCoverLA2 = Convert.ToDecimal(NoNull(dtOption.Rows[0]["LA2FuneralCover"], 0));
                            _funeralCover = _funeralCover + Convert.ToInt32(NoNull(dtOption.Rows[0]["LA2FuneralCover"], 0));

                            xamCEFuneralCoverLA2.Value = FuneralCoverLA2;
                            LaData.PolicyData.TotalPremium = LaData.PolicyData.TotalPremium + FuneralCostLA2;
                        }
                        else
                        {
                            _funeralCover = 0;
                            xamCEFuneralCostLA1.Value = 0.00m;
                            xamCEFuneralCoverLA1.Value = 0.00m;
                            xamCEFuneralCostLA2.Value = 0.00m;
                            xamCEFuneralCoverLA2.Value = 0.00m;
                        }

                        //Extra Options Added from 2020-03-01
                        decimal LA1CostOther = 0.00m;
                        decimal LA1CoverOther = 0.00m;
                        decimal LA2CostOther = 0.00m;
                        decimal LA2CoverOther = 0.00m;

                        if (dtOptionExtra?.Rows.Count == 1)
                        {
                            if (LaData.AppData.CampaignType == lkpINCampaignType.FemaleDisCancer)
                            {
                                LA1CostOther = Convert.ToDecimal(NoNull(dtOptionExtra.Rows[0]["LA1CancerCost"], 0));
                                xamCELA1CostOther.Value = LA1CostOther;
                                LA1CoverOther = Convert.ToDecimal(NoNull(dtOptionExtra.Rows[0]["LA1CancerCover"], 0));
                                xamCELA1CoverOther.Value = LA1CoverOther;

                                LA2CostOther = Convert.ToDecimal(NoNull(dtOptionExtra.Rows[0]["LA2CancerCost"], 0));
                                xamCELA2CostOther.Value = LA2CostOther;
                                LA2CoverOther = Convert.ToDecimal(NoNull(dtOptionExtra.Rows[0]["LA2CancerCover"], 0));
                                xamCELA2CoverOther.Value = LA2CoverOther;
                            }
                            else
                            {

                            }
                        }

                        xamCETotalPremium.Value = LaData.PolicyData.TotalPremium;

                        SqlParameter[] parameters = new SqlParameter[5];
                        parameters[0] = new SqlParameter("@ImportID", LaData.AppData.ImportID);
                        parameters[1] = new SqlParameter("@NewOptionID", LaData.PolicyData.OptionID);
                        parameters[2] = new SqlParameter("@NewOptionLA2", LaData.PolicyData.IsLA2Checked);
                        parameters[3] = new SqlParameter("@NewOptionChild", LaData.PolicyData.IsChildChecked);
                        parameters[4] = new SqlParameter("@NewPremium", LaData.PolicyData.TotalPremium);
                        LaData.PolicyData.TotalInvoiceFee = Convert.ToDecimal(Methods.ExecuteFunction("fnGetTotalFeeByOptions", parameters));

                        #region calculate moneyback payout

                        if (dteDateOfBirth.Value != DBNull.Value && dteDateOfBirth.DateValue != null && dteDateOfBirth.IsValueValid)
                        {
                            int birthYear = ((DateTime)dteDateOfBirth.DateValue).Year;
                            int policyYear;

                            if (dteDateOfSale.DateValue != null && dteDateOfSale.IsValueValid)
                            {
                                policyYear = ((DateTime)dteDateOfSale.DateValue).Year;
                            }
                            else
                            {
                                policyYear = DateTime.Now.Year;
                            }

                            LaData.PolicyData.MoneyBackPayout = GetMoneyBackPayout(policyYear, birthYear, Convert.ToDecimal(LaData.PolicyData.TotalPremium));
                            LaData.PolicyData.MoneyBackPayoutAge = GetMoneyBackPayoutAge(LaData.LeadData.DateOfBirth, LaData.PolicyData.CommenceDate);
                        }
                        else
                        {
                            LaData.PolicyData.MoneyBackPayout = 0.00m;
                            LaData.PolicyData.MoneyBackPayoutAge = null;
                        }

                        #endregion

                        //calculate 50%violence benefit for macc campaigns
                        switch (LaData.AppData.CampaignType)
                        {
                            case lkpINCampaignType.Macc:
                            case lkpINCampaignType.MaccMillion:
                            case lkpINCampaignType.AccDis:
                                xamCE50PercViolenceBenefit.Value = LA1Cover + (LA1Cover * 50 / 100);
                                xamCE50PercViolenceBenefit.Visibility = Visibility.Visible;
                                lbl50PercViolenceBenefit.Visibility = Visibility.Visible;
                                break;

                            default:
                                xamCE50PercViolenceBenefit.Value = 0.00m;
                                xamCE50PercViolenceBenefit.Visibility = Visibility.Collapsed;
                                lbl50PercViolenceBenefit.Visibility = Visibility.Collapsed;
                                break;
                        }

                        //calculate IG cancer free cover
                        switch (LaData.AppData.CampaignID)
                        {
                            case 3:
                            case 4:
                            case 334:
                            case 264:

                                DataTable dt = Methods.GetTableData("SELECT IGFreeCover FROM INPlan WHERE ID = '" + LaData.PolicyData.PlanID + "'");
                                xamCEIGLA1FreeCover.Value = dt.Rows.Count > 0 ? Convert.ToDecimal(dt.Rows[0]["IGFreeCover"]) : 0.00m;

                                if (cmbLA1Cover.SelectedIndex == 0) //only show free cover if it is the higher option
                                {
                                    xamCEIGLA1FreeCover.Visibility = Visibility.Visible;
                                    lblIGLA1FreeCover.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    xamCEIGLA1FreeCover.Value = 0.00m;
                                    xamCEIGLA1FreeCover.Visibility = Visibility.Collapsed;
                                    lblIGLA1FreeCover.Visibility = Visibility.Collapsed;
                                }
                                break;

                            default:
                                xamCEIGLA1FreeCover.Value = 0.00m;
                                xamCEIGLA1FreeCover.Visibility = Visibility.Collapsed;
                                lblIGLA1FreeCover.Visibility = Visibility.Collapsed;
                                break;
                        }

                        //Show or Hide Base Cover Lines
                        grdBaseLine1.Visibility = LA1Cover > 1 ? Visibility.Visible : Visibility.Collapsed;
                        grdBaseLine2.Visibility = LA2Cover > 1 ? Visibility.Visible : Visibility.Collapsed;
                        grdBaseLine3.Visibility = LA1CoverOther > 1 ? Visibility.Visible : Visibility.Collapsed;
                        grdBaseLine4.Visibility = LA2CoverOther > 1 ? Visibility.Visible : Visibility.Collapsed;
                        grdBaseLine5.Visibility = ChildCover > 1 ? Visibility.Visible : Visibility.Collapsed;

                        if (LaData.AppData.CampaignType == lkpINCampaignType.FemaleDisCancer || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion || LaData.AppData.CampaignType == lkpINCampaignType.BlackMacc || LaData.AppData.CampaignType == lkpINCampaignType.FemaleDis)
                        {
                            lblLA1CostCover.Text = "Life Assured 1 (Disability)";
                            lblLA2CostCover.Text = "Life Assured 2 (Disability)";
                            lblLA1CostCoverOther.Text = "Life Assured 1 (Cancer)";
                            lblLA2CostCoverOther.Text = "Life Assured 2 (Cancer)";

                            LaData.PolicyData.TotalPremium = LaData.PolicyData.TotalPremium + LA1CostOther + LA2CostOther;

                            LA1LA2TotalCover = LA1LA2TotalCover + LA1CoverOther + LA2CoverOther; // + FuneralCoverLA1 + FuneralCoverLA2
                        }
                        else
                        {
                            lblLA1CostCover.Text = "Life Assured 1";
                            lblLA2CostCover.Text = "Life Assured 2";
                            lblLA1CostCoverOther.Text = "Life Assured 1";
                            lblLA2CostCoverOther.Text = "Life Assured 2";
                        }

                        //Funeral hide/show
                        if (FuneralCoverLA1 > 1)
                        {
                            lblFuneralCostCoverLA1.Visibility = Visibility.Visible;
                            xamCEFuneralCostLA1.Visibility = Visibility.Visible;
                            xamCEFuneralCoverLA1.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            lblFuneralCostCoverLA1.Visibility = Visibility.Collapsed;
                            xamCEFuneralCostLA1.Visibility = Visibility.Collapsed;
                            xamCEFuneralCoverLA1.Visibility = Visibility.Collapsed;
                        }

                        if (FuneralCoverLA2 > 1)
                        {
                            lblFuneralCostCoverLA2.Visibility = Visibility.Visible;
                            xamCEFuneralCostLA2.Visibility = Visibility.Visible;
                            xamCEFuneralCoverLA2.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            lblFuneralCostCoverLA2.Visibility = Visibility.Collapsed;
                            xamCEFuneralCostLA2.Visibility = Visibility.Collapsed;
                            xamCEFuneralCoverLA2.Visibility = Visibility.Collapsed;
                        }

                        xamCELA1LA2TotalCover.Value = LA1LA2TotalCover;
                    }
                }
                else
                {
                    ResetDisplayedCosts();
                }

                if (LaData.AppData.IsLeadLoaded && checkBumpup) CalculateBumpUpOrReducedPremium();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        private FrameworkElement CreateControl(Type type, string strStyle, Grid grid, int row, int column, int columnSpan)
        {
            FrameworkElement control = (FrameworkElement)Activator.CreateInstance(type);
            Style style = new Style(type);
            style.BasedOn = (Style)FindResource(strStyle);
            control.Style = style;
            grid.Children.Add(control);
            control.SetValue(Grid.RowProperty, row);
            control.SetValue(Grid.ColumnProperty, column);
            control.SetValue(Grid.ColumnSpanProperty, columnSpan);

            return control;
        }

        private void CalculateCostUpgrade(bool checkBumpUp)
        {
            try
            {
                if (LaData.AppData.IsLeadSaving || !LaData.AppData.IsLeadLoaded) return;

                #region Initialize

                for (int row = 1; row <= 6; row++)
                {
                    for (int column = 0; column <= 10; column++)
                    {
                        UIElement test = grdPolicyDetailsUpgrade.Children.Cast<UIElement>().FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
                        grdPolicyDetailsUpgrade.Children.Remove(test);
                    }
                }

                LaData.PolicyData.UpgradePremium = null;
                LaData.PolicyData.TotalPremium = null;

                #endregion

                if (LaData.PolicyData.OptionID != null)
                {
                    DataTable dtOption = Methods.GetTableData("SELECT * FROM INOption WHERE ID = '" + LaData.PolicyData.OptionID + "'");
                    //DataTable dtOptionFees = Methods.GetTableData("SELECT * FROM INOptionFees WHERE FKINOptionID = '" + LaData.PolicyData.Option)

                    LaData.PolicyData.TotalPremium = 0;
                    int gridRow = 1;
                    FrameworkElement control;

                    if (grdPolicyDetailsUpgradeA.RowDefinitions.Count > 1)
                    {
                        grdPolicyDetailsUpgradeA.RowDefinitions.RemoveRange(1, grdPolicyDetailsUpgradeA.RowDefinitions.Count - 1);
                    }
                    if (grdPolicyDetailsUpgradeA.Children.Count > 2)
                    {
                        grdPolicyDetailsUpgradeA.Children.RemoveRange(2, grdPolicyDetailsUpgradeA.Children.Count - 2);
                    }

                    #region LA1 Cover

                    if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA1Cover"], 0)) != 0)
                    {
                        grdPolicyDetailsUpgradeA.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(21) }); // continue with the scrollviewer when needed
                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgradeA, gridRow, 0, 6);
                        if ((LaData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                                                    &&
                                                    (LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                                                    ||
                                                    LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                                                    ||
                                                    LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                                                    ||
                                                    LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8
                                                    ||
                                                    (LaData.AppData.CampaignType == lkpINCampaignType.Cancer
                                                    &&
                                                    LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade9)

                                                    )))

                        {
                            ((TextBlock)control).Text = "LA1 Cancer";
                        }
                        else if (campaignTypesCancer.Contains(LaData.AppData.CampaignType) || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade5 || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade6)
                        {
                            ((TextBlock)control).Text = "LA1 Cancer";
                        }
                        else if (campaignTypesMacc.Contains(LaData.AppData.CampaignType))
                        {
                            ((TextBlock)control).Text = "LA1 Disability";
                        }
                        else
                        {
                            ((TextBlock)control).Text = "LA1";
                        }

                        //if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA1Cost"], 0)) != 0)
                        //{
                        //    control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 4, 3);
                        //    ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA1Cost"]).ToString();
                        //}

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgradeA, gridRow, 7, 4);
                        ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA1Cover"]).ToString(CultureInfo.CurrentCulture);

                        _la1Cover = Convert.ToDecimal(dtOption.Rows[0]["LA1Cover"]).ToString(CultureInfo.CurrentCulture);
                        gridRow++;

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);
                        if ((LaData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                            &&
                            (LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8
                            ||
                            LaData.AppData.CampaignType == lkpINCampaignType.Cancer
                            &&
                            (LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade9

                            )
                            )
                            ))

                        {
                            ((TextBlock)control).Text = "Total LA1 Cancer";
                        }
                        else if (campaignTypesCancer.Contains(LaData.AppData.CampaignType) || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade5 || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade6)
                        {
                            ((TextBlock)control).Text = "Total LA1 Cancer";
                        }
                        else if (campaignTypesMacc.Contains(LaData.AppData.CampaignType))
                        {
                            ((TextBlock)control).Text = "Total LA1 Disability";
                        }
                        else
                        {
                            ((TextBlock)control).Text = "Total LA1";
                        }
                        //else
                        //{
                        //    ((TextBlock)control).Text = campaignTypesCancer.Contains(LaData.AppData.CampaignType) ? "Total LA1 Cancer" : "Total LA1";
                        //    ((TextBlock)control).Text = campaignTypesMacc.Contains(LaData.AppData.CampaignType) ? "Total LA1 Acc Disability" : "Total LA1";
                        //}

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        decimal total = 0.00m;
                        if ((LaData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                                                    &&
                                                    (LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                                                    ||
                                                    LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                                                    ||
                                                    LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                                                    ||
                                                    LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8
                                                    ||
                                                    (LaData.AppData.CampaignType == lkpINCampaignType.Cancer
                                                    &&
                                                    (LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade9
                                                    )
                                                    ))))

                        {
                            total = Convert.ToDecimal(dtOption.Rows[0]["LA1Cover"]) + Convert.ToDecimal(LaData.ImportedCovers[0].Cover);
                        }
                        else if (campaignTypesCancer.Contains(LaData.AppData.CampaignType) || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade5 || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade6)
                        {
                            total = Convert.ToDecimal(dtOption.Rows[0]["LA1Cover"]) + Convert.ToDecimal(LaData.ImportedCovers[0].Cover);
                        }
                        else if (campaignTypesMacc.Contains(LaData.AppData.CampaignType))
                        {
                            total = Convert.ToDecimal(dtOption.Rows[0]["LA1Cover"]) + Convert.ToDecimal(LaData.ImportedCovers[1].Cover);
                        }
                        ((XamCurrencyEditor)control).Text = total.ToString(CultureInfo.CurrentCulture);
                        _totalLa1Cover = total.ToString(CultureInfo.CurrentCulture);
                        gridRow++;
                    }
                    else
                    {
                        _la1Cover = "0.0";
                        _totalLa1Cover = "0.0";
                    }

                    #endregion

                    #region LA2 Cover

                    if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA2Cover"], 0)) != 0)
                    {
                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 4);
                        if ((LaData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                            &&
                            (LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8
                            ||
                            LaData.AppData.CampaignType == lkpINCampaignType.Cancer
                            &&
                            (LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade9

                            )
                            )))
                        {
                            ((TextBlock)control).Text = "LA2 Cancer";
                        }


                        else if (campaignTypesCancer.Contains(LaData.AppData.CampaignType) || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade5 || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade6)
                        {
                            ((TextBlock)control).Text = "LA2 Cancer";
                        }
                        else if (campaignTypesMacc.Contains(LaData.AppData.CampaignType))
                        {
                            ((TextBlock)control).Text = "LA2 Disability";
                        }
                        else
                        {
                            ((TextBlock)control).Text = "LA2";
                        }
                        //else
                        //{
                        //    ((TextBlock)control).Text = campaignTypesCancer.Contains(LaData.AppData.CampaignType) ? "LA2 Cancer" : "LA2";
                        //    ((TextBlock)control).Text = campaignTypesMacc.Contains(LaData.AppData.CampaignType) ? "LA2 Acc Disability" : "LA2";
                        //}

                        //if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA2Cost"], 0)) != 0)
                        //{
                        //    control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 4, 3);
                        //    ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA2Cost"]).ToString();
                        //}

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA2Cover"]).ToString(CultureInfo.CurrentCulture);

                        _la2Cover = Convert.ToDecimal(dtOption.Rows[0]["LA2Cover"]).ToString(CultureInfo.CurrentCulture);
                        gridRow++;

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);
                        if ((LaData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                            &&
                            (LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8
                            ||
                            LaData.AppData.CampaignType == lkpINCampaignType.Cancer
                            &&
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade9

                       )
                       ))
                        {
                            ((TextBlock)control).Text = "Total LA2 Cancer";
                        }
                        else if (campaignTypesCancer.Contains(LaData.AppData.CampaignType) || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade5 || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade6)
                        {
                            ((TextBlock)control).Text = "Total LA2 Cancer";
                        }
                        else if (campaignTypesMacc.Contains(LaData.AppData.CampaignType))
                        {
                            ((TextBlock)control).Text = "Total LA2 Disability";
                        }
                        else
                        {
                            ((TextBlock)control).Text = "Total LA2";
                        }
                        //else
                        //{
                        //    ((TextBlock)control).Text = campaignTypesCancer.Contains(LaData.AppData.CampaignType) ? "Total LA2 Cancer" : "Total LA2";
                        //    ((TextBlock)control).Text = campaignTypesMacc.Contains(LaData.AppData.CampaignType) ? "Total LA2 Acc Disability" : "Total LA2";
                        //}

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        decimal total = 0.00m;
                        if ((LaData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                            &&
                            (LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8
                            ||
                            LaData.AppData.CampaignType == lkpINCampaignType.Cancer
                            &&
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade9

                            )
                            ))
                        {
                            total = Convert.ToDecimal(dtOption.Rows[0]["LA2Cover"]) + Convert.ToDecimal(LaData.ImportedCovers[4].Cover);
                        }
                        else if (campaignTypesCancer.Contains(LaData.AppData.CampaignType) || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade5 || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade6)
                        {
                            total = Convert.ToDecimal(dtOption.Rows[0]["LA2Cover"]) + Convert.ToDecimal(LaData.ImportedCovers[4].Cover);
                        }
                        else if (campaignTypesMacc.Contains(LaData.AppData.CampaignType))
                        {
                            total = Convert.ToDecimal(dtOption.Rows[0]["LA2Cover"]) + Convert.ToDecimal(LaData.ImportedCovers[5].Cover);
                        }
                        ((XamCurrencyEditor)control).Text = total.ToString(CultureInfo.CurrentCulture);
                        _totalLa2Cover = total.ToString(CultureInfo.CurrentCulture);
                        gridRow++;
                    }
                    else
                    {
                        _la2Cover = "0.0";
                        _totalLa2Cover = "0.0";
                    }

                    #endregion

                    #region LA1 Death Cover

                    if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA1AccidentalDeathCover"], 0)) != 0)
                    {
                        _la1AccidentalDeathCover = Convert.ToInt32(NoNull(dtOption.Rows[0]["LA1AccidentalDeathCover"], 0));

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 4);
                        ((TextBlock)control).Text = "LA1 Acc Death";

                        //if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA1AccidentalDeathCost"], 0)) != 0)
                        //{
                        //    control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 4, 3);
                        //    ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA1AccidentalDeathCost"]).ToString();
                        //}

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA1AccidentalDeathCover"]).ToString(CultureInfo.CurrentCulture);
                        _la1AccDeathCover = Convert.ToDecimal(dtOption.Rows[0]["LA1AccidentalDeathCover"]).ToString(CultureInfo.CurrentCulture);
                        gridRow++;

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);
                        ((TextBlock)control).Text = "Total LA1 Acc Death";

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        var total = Convert.ToDecimal(dtOption.Rows[0]["LA1AccidentalDeathCover"]) + Convert.ToDecimal(LaData.ImportedCovers[2].Cover);
                        ((XamCurrencyEditor)control).Text = total.ToString(CultureInfo.CurrentCulture);
                        _totalLa1AccDeathCover = total.ToString(CultureInfo.CurrentCulture);
                        gridRow++;
                    }
                    else
                    {
                        _la1AccidentalDeathCover = 0;
                        _la1AccDeathCover = "0.0";
                        _totalLa1AccDeathCover = "0.0";
                    }

                    #endregion

                    #region LA2 Death Cover

                    if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA2AccidentalDeathCover"], 0)) != 0)
                    {
                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 4);
                        ((TextBlock)control).Text = "LA2 Acc Death";

                        //if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA2AccidentalDeathCost"], 0)) != 0)
                        //{
                        //    control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 4, 3);
                        //    ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA2AccidentalDeathCost"]).ToString();
                        //}

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA2AccidentalDeathCover"]).ToString(CultureInfo.CurrentCulture);
                        _la2AccDeathCover = Convert.ToDecimal(dtOption.Rows[0]["LA2AccidentalDeathCover"]).ToString(CultureInfo.CurrentCulture);
                        gridRow++;

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);
                        ((TextBlock)control).Text = "Total LA2 Acc Death";

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        var total = Convert.ToDecimal(dtOption.Rows[0]["LA2AccidentalDeathCover"]) + Convert.ToDecimal(LaData.ImportedCovers[6].Cover);
                        ((XamCurrencyEditor)control).Text = total.ToString(CultureInfo.CurrentCulture);
                        _totalLa2AccDeathCover = total.ToString(CultureInfo.CurrentCulture);
                        gridRow++;
                    }
                    else
                    {
                        _la2AccDeathCover = "0.0";
                        _totalLa2AccDeathCover = "0.0";
                    }

                    #endregion

                    #region Funeral Cover
                    if (Convert.ToInt32(NoNull(dtOption.Rows[0]["FuneralCover"], 0)) != 0)
                    {
                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 4);
                        ((TextBlock)control).Text = "Funeral";

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["FuneralCover"]).ToString(CultureInfo.CurrentCulture);

                        gridRow++;

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);
                        ((TextBlock)control).Text = "Total Funeral";

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        var total = Convert.ToDecimal(dtOption.Rows[0]["FuneralCover"]) + Convert.ToDecimal(LaData.ImportedCovers[3].Cover);
                        ((XamCurrencyEditor)control).Text = total.ToString(CultureInfo.CurrentCulture);

                        gridRow++;
                    }
                    #endregion Funeral Cover

                    #region LA1 Funeral cover

                    if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA1FuneralCover"], 0)) != 0)
                    {
                        _la1FuneralCover = Convert.ToInt32(NoNull(dtOption.Rows[0]["LA1FuneralCover"], 0));

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 4);
                        ((TextBlock)control).Text = "LA1 Funeral";

                        //if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA1AccidentalDeathCost"], 0)) != 0)
                        //{
                        //    control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 4, 3);
                        //    ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA1AccidentalDeathCost"]).ToString();
                        //}

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA1FuneralCover"]).ToString(CultureInfo.CurrentCulture);

                        gridRow++;

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);
                        ((TextBlock)control).Text = "Total LA1 Funeral";

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        var total = Convert.ToDecimal(dtOption.Rows[0]["LA1FuneralCover"]) + Convert.ToDecimal(LaData.ImportedCovers[3].Cover);
                        ((XamCurrencyEditor)control).Text = total.ToString(CultureInfo.CurrentCulture);

                        gridRow++;
                    }
                    else
                    {
                        _la1FuneralCover = 0;
                    }

                    #endregion

                    #region LA2 Funeral cover

                    if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA2FuneralCover"], 0)) != 0)
                    {
                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 4);
                        ((TextBlock)control).Text = "LA2 Funeral";

                        //if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA1AccidentalDeathCost"], 0)) != 0)
                        //{
                        //    control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 4, 3);
                        //    ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA1AccidentalDeathCost"]).ToString();
                        //}

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["LA2FuneralCover"]).ToString(CultureInfo.CurrentCulture);

                        gridRow++;

                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);
                        ((TextBlock)control).Text = "Total LA2 Funeral";

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                        var total = Convert.ToDecimal(dtOption.Rows[0]["LA2FuneralCover"]) + Convert.ToDecimal(LaData.ImportedCovers[3].Cover);
                        ((XamCurrencyEditor)control).Text = total.ToString(CultureInfo.CurrentCulture);

                        gridRow++;
                    }

                    #endregion

                    #region Child Cover

                    if (LaData.PolicyData.IsChildUpgradeChecked)
                    {
                        if (Convert.ToInt32(NoNull(dtOption.Rows[0]["ChildCover"], 0)) != 0)
                        {
                            control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);

                            if ((LaData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                            &&
                            (LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8)
                            ))
                            {
                                ((TextBlock)control).Text = "Child Cancer";
                            }
                            else if (campaignTypesCancer.Contains(LaData.AppData.CampaignType) || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade5 || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade6)
                            {
                                ((TextBlock)control).Text = "Child Cancer";
                            }
                            else if (campaignTypesMacc.Contains(LaData.AppData.CampaignType))
                            {
                                ((TextBlock)control).Text = "Child Disability";
                            }
                            else
                            {
                                ((TextBlock)control).Text = "Child";
                            }

                            //if (Convert.ToInt32(NoNull(dtOption.Rows[0]["ChildCost"], 0)) != 0)
                            //{
                            //    control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 4, 3);
                            //    ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["ChildCost"]).ToString();
                            //}

                            control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                            ((XamCurrencyEditor)control).Text = Convert.ToDecimal(dtOption.Rows[0]["ChildCover"]).ToString(CultureInfo.CurrentCulture);

                            //Child Premium
                            LaData.PolicyData.UpgradeChildPremium = Convert.ToDecimal(dtOption.Rows[0]["ChildCost"]);


                            _childCover = Convert.ToDecimal(dtOption.Rows[0]["ChildCover"]).ToString(CultureInfo.CurrentCulture);
                            gridRow++;

                            control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 0, 6);
                            //((TextBlock)control).Text = "Total Child";
                            if ((LaData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                            &&
                            (LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                            ||
                            LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8)
                            ))
                            {
                                ((TextBlock)control).Text = "Total Child Cancer";
                            }
                            else if (campaignTypesCancer.Contains(LaData.AppData.CampaignType) || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade5 || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade6)
                            {
                                ((TextBlock)control).Text = "Total Child Cancer";
                            }
                            else if (campaignTypesMacc.Contains(LaData.AppData.CampaignType))
                            {
                                ((TextBlock)control).Text = "Total Child Disability";
                            }
                            else
                            {
                                ((TextBlock)control).Text = "Total Child";
                            }

                            control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 7, 4);
                            decimal total = 0.00m;

                            if (campaignTypesCancer.Contains(LaData.AppData.CampaignType))
                            {
                                total = Convert.ToDecimal(dtOption.Rows[0]["ChildCover"]) + Convert.ToDecimal(LaData.ImportedCovers[8].Cover);
                            }
                            else if (campaignTypesMacc.Contains(LaData.AppData.CampaignType))
                            {
                                if ((LaData.AppData.CampaignType == lkpINCampaignType.MaccMillion
                                    &&
                                    (LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade5
                                    ||
                                    LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade6
                                    ||
                                    LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade7
                                    ||
                                    LaData.AppData.CampaignGroup == lkpINCampaignGroup.DoubleUpgrade8)
                                    ))
                                {
                                    total = Convert.ToDecimal(dtOption.Rows[0]["ChildCover"]) + Convert.ToDecimal(LaData.ImportedCovers[8].Cover);
                                }
                                else
                                {
                                    total = Convert.ToDecimal(dtOption.Rows[0]["ChildCover"]) + Convert.ToDecimal(LaData.ImportedCovers[9].Cover);
                                }
                            }

                            ((XamCurrencyEditor)control).Text = total.ToString(CultureInfo.CurrentCulture);

                        }
                        else
                        {
                            LaData.PolicyData.IsChildUpgradeChecked = false;
                            _childCover = "0.0";
                        }
                    }
                    else
                    {
                        _childCover = "0.0";
                    }

                    #endregion

                    #region Total Upgrade Premium

                    LaData.PolicyData.UpgradePremium = LaData.PolicyData.IsChildUpgradeChecked ? dtOption.Rows[0]["TotalPremium2"] as decimal? : dtOption.Rows[0]["TotalPremium1"] as decimal?;

                    SqlParameter[] parameters = new SqlParameter[5];
                    parameters[0] = new SqlParameter("@ImportID", LaData.AppData.ImportID);
                    parameters[1] = new SqlParameter("@NewOptionID", LaData.PolicyData.OptionID);
                    parameters[2] = new SqlParameter("@NewOptionLA2", LaData.PolicyData.IsLA2Checked);
                    parameters[3] = new SqlParameter("@NewOptionChild", LaData.PolicyData.IsChildChecked);
                    parameters[4] = new SqlParameter("@NewPremium", LaData.PolicyData.UpgradePremium);
                    LaData.PolicyData.TotalInvoiceFee = Convert.ToDecimal(Methods.ExecuteFunction("fnGetTotalFeeByOptions", parameters));

                    #endregion

                    #region Total Premium

                    LaData.PolicyData.TotalPremium = LaData.ImportedPolicyData.ContractPremium + LaData.PolicyData.UpgradePremium;

                    #endregion

                    #region Moneyback Payout

                    //if (dteDateOfBirth.Value != DBNull.Value && dteDateOfBirth.DateValue != null && dteDateOfBirth.IsValueValid)
                    //{
                    //    int birthYear = ((DateTime)dteDateOfBirth.DateValue).Year;
                    //    int policyYear;

                    //    if (dteDateOfSale.DateValue != null && dteDateOfSale.IsValueValid)
                    //    {
                    //        policyYear = ((DateTime)dteDateOfSale.DateValue).Year;
                    //    }
                    //    else
                    //    {
                    //        policyYear = DateTime.Now.Year;
                    //    }

                    //    xamCEMoneyBackUpg.Value = GetMoneyBackPayout(policyYear, birthYear, LaData.PolicyData.TotalPremium);
                    //}
                    //else
                    //{
                    //    xamCEMoneyBackUpg.Value = 0.00m;
                    //}

                    #endregion

                }

                if ((!_flagLeadIsBusyLoading) && checkBumpUp)
                {
                    CalculateBumpUpOrReducedPremium();
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void CalculateBumpUpOrReducedPremium()
        {
            try
            {
                if (LaData.AppData.IsLeadLoaded && !LaData.AppData.IsLeadSaving && !PopupChkLA2.IsOpen && LaData.AppData.LeadStatus == 1 && LaData.PolicyData.LoadedTotalPremium != null)
                {
                    if (LaData.UserData.UserTypeID != Convert.ToInt32(lkpUserType.DataCapturer) && LaData.UserData.UserTypeID != Convert.ToInt32(lkpUserType.SalesAgent))
                    //if (LaData.UserData.UserTypeID == Convert.ToInt32(lkpUserType.ConfirmationAgent))
                    {
                        decimal Premium1 = Convert.ToDecimal(LaData.PolicyData.LoadedTotalPremium);
                        decimal InvoiceFee1 = Convert.ToDecimal(LaData.PolicyData.LoadedTotalInvoiceFee);

                        decimal Premium2 = 0;
                        decimal InvoiceFee2 = 0;
                        if (LaData.AppData.IsLeadUpgrade)
                        {
                            if (xamCEUpgradePremiumUpg.Text != string.Empty)
                            {
                                if (xamCEUpgradePremiumUpg.Text != null)
                                {
                                    Premium2 = Convert.ToDecimal(Regex.Match(xamCEUpgradePremiumUpg.Text, @"\d*[\.,]\d*").Value);
                                    InvoiceFee2 = Convert.ToDecimal(LaData.PolicyData.TotalInvoiceFee);
                                }
                            }
                        }
                        else
                        {
                            if (xamCETotalPremium.Text != string.Empty)
                            {
                                Premium2 = Convert.ToDecimal(Regex.Match(xamCETotalPremium.Text, @"\d*[\.,]\d*").Value);
                                InvoiceFee2 = Convert.ToDecimal(LaData.PolicyData.TotalInvoiceFee);
                            }
                        }

                        if ((LaData.PolicyData.UDMBumpUpOption && (Premium2 <= Premium1)) || (LaData.PolicyData.UDMBumpUpOption && (Premium2 != Premium1 - LaData.PolicyData.BumpUpAmount)))
                        {
                            LaData.PolicyData.BumpUpAmount = null;
                            LaData.PolicyData.UDMBumpUpOption = false;
                        }
                        else if ((LaData.PolicyData.ReducedPremiumOption && (Premium2 >= Premium1)) || (LaData.PolicyData.ReducedPremiumOption && (Premium2 != Premium1 - LaData.PolicyData.ReducedPremiumAmount)))
                        {
                            LaData.PolicyData.ReducedPremiumAmount = null;
                            LaData.PolicyData.ReducedPremiumOption = false;
                        }


                        if (Premium1 > 0 && Premium2 > 0 && InvoiceFee1 > 0 && InvoiceFee2 > 0)//if (InvoiceFee1 > 0 && InvoiceFee2 > 0)
                        {
                            SqlParameter[] parameters = new SqlParameter[1];
                            parameters[0] = new SqlParameter("@ImportID", LaData.AppData.ImportID);
                            //this gets what the total invoice fee was when the sales agent saved it so that the bumpup agents cannot say that it was a bumpup by
                            //saving on a lower premium and then going back and saving it on a higher premium but still a lower premium than what the sales agent saved it on.
                            decimal historicTotalInvoiceFee = Convert.ToDecimal(Methods.ExecuteFunction("fnGetLatestInvoiceFeeSavedBySalesAgent", parameters));
                            if ((InvoiceFee2 > InvoiceFee1 && InvoiceFee2 > historicTotalInvoiceFee) || (Premium2 > Premium1 && LaData.AppData.IsLeadUpgrade))//bumpup
                            {

                                //if (Fee2 > Fee1)
                                INMessageBoxWindow2 messageBox = new INMessageBoxWindow2();
                                messageBox.buttonOK.Content = "Yes";
                                messageBox.buttonCancel.Content = "No";

                                var showMessageBox = ShowMessageBox(messageBox, "Is this a Bump Up?", "Bump Up", ShowMessageType.Information);
                                bool result = showMessageBox != null && (bool)showMessageBox;

                                if (result)
                                {

                                    LaData.PolicyData.BumpUpAmount = Premium2 - Premium1;
                                    LaData.PolicyData.UDMBumpUpOption = true;

                                    LaData.PolicyData.ReducedPremiumAmount = null;
                                    LaData.PolicyData.ReducedPremiumOption = false;
                                    if ((lkpUserType?)((User)GlobalSettings.ApplicationUser).FKUserType == lkpUserType.CallMonitoringAgent)
                                    {
                                        LaData.SaleData.FKCMCallRefUserID = GlobalSettings.ApplicationUser.ID;
                                    }
                                    BumpUpSelected = 1;

                                    //Comment this out after testing and before publishing again.
                                    //if (inImportCallMonitoring != null)
                                    //{
                                    //    inImportCallMonitoring.IsCallMonitored = false;
                                    //}
                                }
                            }
                            else if (Premium2 < Premium1) //reduced premium
                            {
                                INMessageBoxWindow2 messageBox = new INMessageBoxWindow2();
                                messageBox.buttonOK.Content = "Yes";
                                messageBox.buttonCancel.Content = "No";

                                var showMessageBox = ShowMessageBox(messageBox, "Is this a Reduced Premium?", "Reduced Premium", ShowMessageType.Information);
                                bool result = showMessageBox != null && (bool)showMessageBox;

                                if (result)
                                {
                                    LaData.PolicyData.ReducedPremiumAmount = Premium1 - Premium2;
                                    LaData.PolicyData.ReducedPremiumOption = true;

                                    LaData.PolicyData.BumpUpAmount = null;
                                    LaData.PolicyData.UDMBumpUpOption = false;

                                    if ((lkpUserType?)((User)GlobalSettings.ApplicationUser).FKUserType == lkpUserType.CallMonitoringAgent)
                                    {
                                        LaData.SaleData.FKCMCallRefUserID = GlobalSettings.ApplicationUser.ID;
                                    }
                                }
                            }
                            else
                            {
                                if ((Premium2 > Premium1 && InvoiceFee2 <= InvoiceFee1) || InvoiceFee2 <= historicTotalInvoiceFee)
                                {
                                    INMessageBoxWindow1 mb = new INMessageBoxWindow1();
                                    var showMessageBox = ShowMessageBox(mb, "Please note this is not a bump up due to the fee not increasing.", "Not a Bump Up", ShowMessageType.Information);
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void PopulateImportedPolicyDataUpgrade()
        {
            try
            {
                int gridRow = 1;
                int maxRow = 5;

                if (LaData.AppData.CampaignCode == "PLDMM6U" || LaData.AppData.CampaignCode == "PLDMM5U" || LaData.AppData.CampaignCode == "PLDMM7U")
                {
                    maxRow = 8;
                }
                FrameworkElement control;

                #region Initialize

                for (int row = 1; row <= maxRow; row++)
                {
                    for (int column = 13; column <= 23; column++)
                    {
                        UIElement test = grdPolicyDetailsUpgrade.Children.Cast<UIElement>().FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
                        grdPolicyDetailsUpgrade.Children.Remove(test);
                    }
                }

                #endregion

                foreach (var importedCover in LaData.ImportedCovers)
                {
                    if (importedCover.Cover != null && importedCover.Premium != null)
                    {
                        control = CreateControl(typeof(TextBlock), "INLabelText2", grdPolicyDetailsUpgrade, gridRow, 13, 4);
                        ((TextBlock)control).Text = importedCover.Name;

                        //control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 17, 3);
                        //((XamCurrencyEditor)control).Text = importedCover.Premium.ToString();

                        control = CreateControl(typeof(XamCurrencyEditor), "INXamCurrencyEditorLabelStyle1", grdPolicyDetailsUpgrade, gridRow, 20, 4);
                        ((XamCurrencyEditor)control).Text = importedCover.Cover.ToString();

                        if (gridRow < maxRow) { gridRow++; } else { break; }
                    }
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private bool HasEntriesLA(LeadApplicationData.LA LAData)
        {
            if (LAData.TitleID != null) { return true; }
            if (LAData.GenderID != null) { return true; }
            if (LAData.Name != null) { return true; }
            if (LAData.Surname != null) { return true; }
            if (LAData.IDNumber != null) { return true; }
            if (LAData.DateOfBirth != null) { return true; }
            if (LAData.RelationshipID != null) { return true; }

            return false;
        }

        private bool HasEntriesBeneficiary(LeadApplicationData.Beneficiary BeneficiaryData)
        {
            if (BeneficiaryData.TitleID != null) { return true; }
            if (BeneficiaryData.GenderID != null) { return true; }
            if (BeneficiaryData.Name != null) { return true; }
            if (BeneficiaryData.Surname != null) { return true; }
            if (BeneficiaryData.DateOfBirth != null) { return true; }
            if (BeneficiaryData.RelationshipID != null) { return true; }
            if (BeneficiaryData.Percentage != null) { return true; }
            if (BeneficiaryData.Notes != null) { return true; }

            return false;
        }

        private bool HasEntriesChild(LeadApplicationData.Child ChildData)
        {
            return ChildData.DateOfBirth != null;
        }

        private DateTime? IDNumberToDOB(string strInput)
        {
            try
            {
                //Calculate date of birth from the first 6 characters of the IDNumber
                if (!string.IsNullOrWhiteSpace(strInput) && strInput.Length >= 6)
                {
                    strInput = strInput.Substring(0, 6);
                    string strYY = strInput.Substring(0, 2);
                    string strMM = strInput.Substring(2, 2);
                    string strDD = strInput.Substring(4, 2);

                    //Build output date string
                    string strOutput = int.Parse(strYY) > 10 ? "19" + strYY : "20" + strYY;
                    strOutput = strOutput + "/" + strMM + "/" + strDD;

                    DateTime date;
                    bool result = DateTime.TryParse(strOutput, out date);



                    if (result)
                    {
                        return date;
                    }




                }

                return null;
            }

            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }

        private string CapitalizeWords(string editorName, string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            string str = text.Trim().ToLower();
            string final = string.Empty;

            switch (editorName)
            {
                case "medName":
                case "medSurname":
                case "medAddress1":
                case "medAddress2":
                case "medAddress3":
                case "medDOInitials":
                case "medDOName":
                case "medDOSurname":
                case "medDOAccountHolder":
                case "medLA2Name":
                case "medLA2Surname":
                case "medLA2NameUpg":
                case "medLA2SurnameUpg":
                case "medBeneficiaryName1":
                case "medBeneficiarySurname1":
                case "medBeneficiaryName2":
                case "medBeneficiarySurname2":
                case "medBeneficiaryName3":
                case "medBeneficiarySurname3":
                case "medBeneficiaryName4":
                case "medBeneficiarySurname4":
                case "medBeneficiaryName5":
                case "medBeneficiarySurname5":
                case "medBeneficiaryName6":
                case "medBeneficiarySurname6":
                case "medBeneficiaryName1Upg":
                case "medBeneficiarySurname1Upg":
                case "medBeneficiaryName2Upg":
                case "medBeneficiarySurname2Upg":
                case "medBeneficiaryName3Upg":
                case "medBeneficiarySurname3Upg":
                case "medBeneficiaryName4Upg":
                case "medBeneficiarySurname4Upg":
                case "medBeneficiaryName5Upg":
                case "medBeneficiarySurname5Upg":
                case "medBeneficiaryName6Upg":
                case "medBeneficiarySurname6Upg":
                case "medNOKName":
                case "medNOKSurname":
                case "medNOKNameUpgrade":
                case "medNOKSurnameUpgrade":
                    if (str.Length > 0)
                    {
                        // See https://stackoverflow.com/questions/32923297/how-to-uppercase-a-letter-in-the-middle-of-a-string
                        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                        final = ti.ToTitleCase(str);

                        final = final.Replace(" Van ", " van ");
                        final = final.Replace("Van ", "van ");
                        final = final.Replace(" Der ", " der ");
                        final = final.Replace(" Den ", " den ");
                        final = final.Replace(" De ", " de ");

                        //string[] parts = str.Split(' ', '-');
                        //for (int i = 0; i < parts.Length; i++)
                        //{
                        //    if (i + 1 == parts.Length)
                        //    {
                        //        if (SurnameCapitalization(parts[i]))
                        //            final += parts[i];
                        //        else
                        //            final += parts[i][0].ToString().ToUpper() + parts[i].Substring(1, parts[i].Length - 1);
                        //    }
                        //    else
                        //    {
                        //        if (SurnameCapitalization(parts[i]))
                        //        {
                        //            final += parts[i] + " ";
                        //        }
                        //        else
                        //        {
                        //            if (parts[i] == "")
                        //            {
                        //                final += parts[i];
                        //            }
                        //            else
                        //            {
                        //                final += parts[i][0].ToString().ToUpper() + parts[i].Substring(1, parts[i].Length - 1) + " ";
                        //            }
                        //        }
                        //    }
                        //}
                    }

                    // Extra filtering on address (Special word transforms)
                    switch (editorName)
                    {
                        case "medAddress1":
                        case "medAddress2":
                        case "medAddress3":
                            final = final.Replace(" Corner Of ", " Cnr ");
                            final = final.Replace("Corner Of ", "Cnr ");
                            final = final.Replace(" Corner Of", " Cnr");
                            final = final.Replace(" And ", " & ");
                            //final = final.Replace("And ", "& ");
                            //final = final.Replace(" And", " &");
                            final = final.Replace(" Building ", " Bldg ");
                            final = final.Replace("Building ", "Bldg ");
                            final = final.Replace(" Building", " Bldg");
                            final = final.Replace(" Attention ", " Att ");
                            final = final.Replace("Attention ", "Att ");
                            final = final.Replace(" Attention", " Att");
                            final = final.Replace(" Department ", " Dept ");
                            final = final.Replace("Department ", "Dept ");
                            final = final.Replace(" Department", " Dept");
                            final = final.Replace("Po Box", "PO Box");
                            break;
                    }

                    return final.Trim();
            }

            return text.Trim();
        }

        private bool SurnameCapitalization(string part)
        {
            if (part == "van" || part == "der")
                return true;

            return false;
        }

        //private string GetInitials(string names, string separator)
        //{
        //    // Extract the first character out of each block of non-whitespace
        //    Regex extractInitials = new Regex(@"\s*([^\s])[^\s]*\s*");
        //    return extractInitials.Replace(names, "$1" + separator).ToUpper();
        //}

        public void LoadClosureDocument()
        {
            DataTable dt;
            string strSQL = null;

            if (LaData.ClosureData.ClosureID != null)
            {
                strSQL = "SELECT * FROM Closure WHERE ID = '" + LaData.ClosureData.ClosureID + "'";
            }
            else
            {
                if (LaData.AppData.CampaignID != null)
                {
                    LaData.ClosureData.ClosureLanguageID = LaData.ClosureData.ClosureLanguageID ?? (long)lkpLanguage.English;

                    strSQL = "SELECT * FROM Closure WHERE FKSystemID = '2'";
                    strSQL = strSQL + " AND FKCampaignID = '" + LaData.AppData.CampaignID + "'";
                    strSQL = strSQL + " AND FKLanguageID = '" + LaData.ClosureData.ClosureLanguageID + "'";
                    strSQL = strSQL + " AND IsActive = '1'";
                }
            }

            if (strSQL != null)
            {
                dt = Methods.GetTableData(strSQL);

                if (dt != null && dt.Rows.Count == 1)
                {
                    LaData.ClosureData.ClosureID = dt.Rows[0]["ID"] as long?;
                    LaData.ClosureData.ClosureLanguageID = dt.Rows[0]["FKLanguageID"] as long?;

                    byte[] data = dt.Rows[0]["Document"] as byte[];

                    if (data != null)
                    {
                        if (LaData.AppData.CampaignType == lkpINCampaignType.Macc ||
                            LaData.AppData.CampaignType == lkpINCampaignType.Cancer ||
                            LaData.AppData.CampaignType == lkpINCampaignType.CancerFuneral ||
                            LaData.AppData.CampaignType == lkpINCampaignType.AccDis ||
                            LaData.AppData.CampaignType == lkpINCampaignType.IGCancer ||
                            LaData.AppData.CampaignType == lkpINCampaignType.FemaleDis ||
                            LaData.AppData.CampaignType == lkpINCampaignType.IGFemaleDisability ||
                            LaData.AppData.CampaignType == lkpINCampaignType.MaccMillion ||
                            LaData.AppData.CampaignType == lkpINCampaignType.BlackMacc ||
                            LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion ||
                            LaData.AppData.CampaignType == lkpINCampaignType.TermCancer ||
                            LaData.AppData.CampaignType == lkpINCampaignType.MaccCancer ||
                            LaData.AppData.CampaignType == lkpINCampaignType.MaccMillionCancer)
                        {
                            _strXpsDoc = GlobalSettings.UserFolder + "temp ~ " + Guid.NewGuid() + ".rtf";
                            FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                            objFileStream.Write(data, 0, data.Length);
                            objFileStream.Close();

                            objFileStream.Close();
                            objFileStream.Dispose();
                            //edit rtf document

                            string[] content = File.ReadAllLines(_strXpsDoc);

                            string[] newData = new string[content.Length];
                            int index = 0;
                            foreach (string c in content)
                            {
                                string c2 = c;
                                bool formattingapplied = false;
                                string dat = string.Empty;

                                #region Substituting the Clients surname tag

                                if (c.Contains("[Clients surname]"))
                                {

                                    dat = c2.Replace("[Clients surname]", cmbTitle.Text.Trim() + " " + medSurname.Text.Trim());
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the Clients surname tag

                                #region Substituting the Clients Name tag

                                if (c.Contains("[Clients name]"))
                                {

                                    dat = c2.Replace("[Clients name]", medName.Text.Trim());
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the Clients Name tag

                                #region Substituting the (mm) tag

                                //if (c.Contains("(mm)")||c.Contains("(mm"))
                                //{
                                //    if (LaData.PolicyData.CommenceDate.HasValue)
                                //    {
                                //        string month = String.Empty;

                                //        if (LaData.ClosureData.ClosureLanguageID == (long)lkpLanguage.Afrikaans)
                                //        {
                                //            switch (LaData.PolicyData.CommenceDate.Value.Month)
                                //            {
                                //                case 1:
                                //                    month = "Januarie";
                                //                    break;
                                //                case 2:
                                //                    month = "Februarie";
                                //                    break;
                                //                case 3:
                                //                    month = "Maart";
                                //                    break;
                                //                case 4:
                                //                    month = "April";
                                //                    break;
                                //                case 5:
                                //                    month = "Mei";
                                //                    break;
                                //                case 6:
                                //                    month = "Junie";
                                //                    break;
                                //                case 7:
                                //                    month = "Julie";
                                //                    break;
                                //                case 8:
                                //                    month = "Augustus";
                                //                    break;
                                //                case 9:
                                //                    month = "September";
                                //                    break;
                                //                case 10:
                                //                    month = "Oktober";
                                //                    break;
                                //                case 11:
                                //                    month = "November";
                                //                    break;
                                //                case 12:
                                //                    month = "Desember";
                                //                    break;
                                //                default:
                                //                    month = "(mm)";
                                //                    break;
                                //            }
                                //        }
                                //        else
                                //        {
                                //            switch (LaData.PolicyData.CommenceDate.Value.Month)
                                //            {
                                //                case 1:
                                //                    month = "January";
                                //                    break;
                                //                case 2:
                                //                    month = "February";
                                //                    break;
                                //                case 3:
                                //                    month = "March";
                                //                    break;
                                //                case 4:
                                //                    month = "April";
                                //                    break;
                                //                case 5:
                                //                    month = "May";
                                //                    break;
                                //                case 6:
                                //                    month = "June";
                                //                    break;
                                //                case 7:
                                //                    month = "July";
                                //                    break;
                                //                case 8:
                                //                    month = "August";
                                //                    break;
                                //                case 9:
                                //                    month = "September";
                                //                    break;
                                //                case 10:
                                //                    month = "October";
                                //                    break;
                                //                case 11:
                                //                    month = "November";
                                //                    break;
                                //                case 12:
                                //                    month = "December";
                                //                    break;
                                //                default:
                                //                    month = "(mm)";
                                //                    break;
                                //            }
                                //        }

                                //        if (c.Contains("(mm)"))
                                //        {
                                //            dat = c2.Replace("(mm)", month);
                                //        }
                                //        else if (c.Contains("(mm"))
                                //        {
                                //            dat = c2.Replace("(mm", month);
                                //        }

                                //        c2 = dat;
                                //        formattingapplied = true;
                                //    }
                                //}

                                #endregion Substituting the LA1FuneralCover tag

                                #region Substituting TotalPremium

                                if (c.Contains("[TotalPremium]"))
                                {
                                    if (LaData.AppData.IsLeadLoaded)
                                    {
                                        //string totalPremium = LaData.AppData.IsLeadUpgrade ? Convert.ToString(xamCETotalPremiumUpg.Value) : Convert.ToString(xamCETotalPremium.Value);

                                        //dat = c2.Replace("[TotalPremium]", Methods.CurrencyToWords(totalPremium, LaData.ClosureData.ClosureLanguageID));
                                        //c2 = dat;

                                        //formattingapplied = true;

                                        if (LaData.AppData.IsLeadUpgrade)
                                        {
                                            string totalPremium = "0.00";
                                            if (xamCETotalPremiumUpg.Value != null)
                                            {
                                                totalPremium = xamCETotalPremiumUpg.Value.ToString();
                                            }

                                            dat = c2.Replace("[TotalPremium]", string.Format("{0:c}", totalPremium));
                                            c2 = dat;
                                            formattingapplied = true;
                                        }
                                        else
                                        {
                                            dat = c2.Replace("[TotalPremium]", string.Format("{0:c}", xamCETotalPremium.Value));
                                            c2 = dat;
                                            formattingapplied = true;
                                        }
                                    }
                                }

                                #endregion Substituting TotalPremium

                                #region Substituting LA1Cover

                                if (c.Contains("[LA1Cover]"))
                                {
                                    string currencyValue = LaData.AppData.IsLeadUpgrade ? Convert.ToString(_la1Cover) : Convert.ToString(xamCELA1Cover.Value);

                                    dat = c2.Replace("[LA1Cover]", Methods.CurrencyToWords(currencyValue, LaData.ClosureData.ClosureLanguageID, true));
                                    c2 = dat;
                                    formattingapplied = true;

                                    //if (LaData.AppData.IsLeadUpgrade)
                                    //{
                                    //    dat = c2.Replace("[LA1Cover]", string.Format("{0:c}",_la1Cover));
                                    //    c2 = dat;
                                    //    formattingapplied = true;
                                    //}
                                    //else
                                    //{
                                    //    dat = c2.Replace("[LA1Cover]", string.Format("{0:c}",xamCELA1Cover.Value));
                                    //    c2 = dat;
                                    //    formattingapplied = true;
                                    //}

                                }

                                #endregion Substituting LA1Cover

                                #region Substituting the TotalLA1Cover tag

                                if (c.Contains("[TotalLA1Cover]"))
                                {
                                    dat = c2.Replace("[TotalLA1Cover]", Methods.CurrencyToWords(_totalLa1Cover, LaData.ClosureData.ClosureLanguageID, true));
                                    //dat = c2.Replace("[TotalLA1Cover]", string.Format("{0:c}", _totalLa1Cover));
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the TotalLA1Cover tage

                                #region Substituting the LA2Cover tag

                                if (c.Contains("[LA2Cover]"))
                                {
                                    string currencyValue = LaData.AppData.IsLeadUpgrade ? Convert.ToString(_la2Cover) : Convert.ToString(xamCELA2Cover.Value);

                                    dat = c2.Replace("[LA2Cover]", Methods.CurrencyToWords(currencyValue, LaData.ClosureData.ClosureLanguageID, true));
                                    c2 = dat;
                                    formattingapplied = true;

                                    //if (LaData.AppData.IsLeadUpgrade)
                                    //{
                                    //    dat = c2.Replace("[LA2Cover]", string.Format("{0:c}", _la2Cover));
                                    //    c2 = dat;
                                    //    formattingapplied = true;
                                    //}
                                    //else
                                    //{
                                    //    dat = c2.Replace("[LA2Cover]", string.Format("{0:c}", xamCELA2Cover.Value));
                                    //    c2 = dat;
                                    //    formattingapplied = true;
                                    //}
                                }

                                #endregion Substituting the LA2Cover tag

                                #region Substituting the TotalLA2Cover tag

                                if (c.Contains("[TotalLA2Cover]"))
                                {
                                    //dat = c2.Replace("[TotalLA2Cover]", string.Format("{0:c}", _totalLa2Cover));
                                    dat = c2.Replace("[TotalLA2Cover]", Methods.CurrencyToWords(_totalLa2Cover, LaData.ClosureData.ClosureLanguageID, true));
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the TotalLA2Cover tag

                                #region Substituting the ChildCover tag

                                if (c.Contains("[ChildCover]"))
                                {
                                    string currencyValue = LaData.AppData.IsLeadUpgrade ? Convert.ToString(_childCover) : Convert.ToString(xamCEChildCover.Value);

                                    dat = c2.Replace("[ChildCover]", Methods.CurrencyToWords(currencyValue, LaData.ClosureData.ClosureLanguageID, true));
                                    c2 = dat;
                                    formattingapplied = true;

                                    //if (LaData.AppData.IsLeadUpgrade)
                                    //{
                                    //    dat = c2.Replace("[ChildCover]", string.Format("{0:c}", _childCover));
                                    //    c2 = dat;
                                    //    formattingapplied = true;
                                    //}
                                    //else
                                    //{
                                    //    dat = c2.Replace("[ChildCover]", string.Format("{0:c}", xamCEChildCover.Value));
                                    //    c2 = dat;
                                    //    formattingapplied = true;
                                    //}
                                }

                                #endregion Substituting the ChildCover tag

                                #region Substituting the LA1Death tag

                                if (c.Contains("[LA1Death]"))
                                {
                                    dat = c2.Replace("[LA1Death]", Methods.CurrencyToWords(_la1AccDeathCover, LaData.ClosureData.ClosureLanguageID, true));
                                    c2 = dat;
                                    formattingapplied = true;

                                    //string deathCover = string.Empty;
                                    //if (_la1AccDeathCover == "0.0")
                                    //{
                                    //    //check if la2 death cover has value
                                    //    if (_la2AccDeathCover != "0.0")
                                    //    {
                                    //        if (LaData.AppData.CampaignType == lkpINCampaignType.Cancer)
                                    //        {
                                    //            //Nicolas Stephenson commented the following out because Brigette said that only the 
                                    //            //current cover must be read to the client in the closure. See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/228287307/comments
                                    //            //dat = c2.Replace("[LA1Death]", _la2AccDeathCover);
                                    //            //c2 = dat;
                                    //            //formattingapplied = true;

                                    //            dat = c2.Replace("[LA1Death]", string.Format("{0:c}", _la1AccDeathCover));
                                    //            c2 = dat;
                                    //            formattingapplied = true;
                                    //        }
                                    //        else
                                    //        {
                                    //            dat = c2.Replace("[LA1Death]", string.Format("{0:c}", _la1AccDeathCover));
                                    //            c2 = dat;
                                    //            formattingapplied = true;
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        dat = c2.Replace("[LA1Death]", string.Format("{0:c}", _la1AccDeathCover));
                                    //        c2 = dat;
                                    //        formattingapplied = true;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    dat = c2.Replace("[LA1Death]", string.Format("{0:c}", _la1AccDeathCover));
                                    //    c2 = dat;
                                    //    formattingapplied = true;
                                    //}
                                }

                                #endregion Substituting the LA1Death tag

                                #region Substituting the TotalLA1Death tag

                                if (c.Contains("[TotalLA1Death]"))
                                {
                                    dat = c2.Replace("[TotalLA1Death]", Methods.CurrencyToWords(_totalLa1AccDeathCover, LaData.ClosureData.ClosureLanguageID, true));
                                    c2 = dat;
                                    formattingapplied = true;

                                    //dat = c2.Replace("[TotalLA1Death]", string.Format("{0:c}", _totalLa1AccDeathCover));
                                    //c2 = dat;
                                    //formattingapplied = true;
                                }

                                #endregion Substituting the TotalLA1Death tag

                                #region Substituting the TotalLA2Death tag

                                if (c.Contains("[TotalLA2Death]"))
                                {
                                    dat = c2.Replace("[TotalLA2Death]", Methods.CurrencyToWords(_totalLa2AccDeathCover, LaData.ClosureData.ClosureLanguageID, true));
                                    c2 = dat;
                                    formattingapplied = true;

                                    //dat = c2.Replace("[TotalLA2Death]", string.Format("{0:c}", _totalLa2AccDeathCover));
                                    //c2 = dat;
                                    //formattingapplied = true;
                                }

                                #endregion Substituting the TotalLA2Death tag

                                #region Substituting the LA2Death tag

                                if (c.Contains("[LA2Death]"))
                                {
                                    dat = c2.Replace("[LA2Death]", Methods.CurrencyToWords(_la2AccDeathCover, LaData.ClosureData.ClosureLanguageID, true));
                                    c2 = dat;
                                    formattingapplied = true;

                                    //if (_la2AccDeathCover == "0.0")
                                    //{
                                    //    if (_la1AccDeathCover != "0.0")
                                    //    {
                                    //        if (LaData.AppData.CampaignType == lkpINCampaignType.Cancer)
                                    //        {
                                    //            dat = c2.Replace("[LA1Death]", string.Format("{0:c}", _la1AccDeathCover));
                                    //            c2 = dat;
                                    //            formattingapplied = true;

                                    //            dat = c2.Replace("[LA2Death]", string.Format("{0:c}", _la2AccDeathCover));
                                    //            c2 = dat;
                                    //            formattingapplied = true;
                                    //        }
                                    //        else
                                    //        {
                                    //            dat = c2.Replace("[LA2Death]", string.Format("{0:c}", _la2AccDeathCover));
                                    //            c2 = dat;
                                    //            formattingapplied = true;
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        dat = c2.Replace("[LA2Death]", string.Format("{0:c}", _la2AccDeathCover));
                                    //        c2 = dat;
                                    //        formattingapplied = true;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    dat = c2.Replace("[LA2Death]", string.Format("{0:c}", _la2AccDeathCover));
                                    //    c2 = dat;
                                    //    formattingapplied = true;
                                    //}
                                }

                                #endregion Substituting the LA2Death tag

                                #region Substituting the (yyyy/mm/dd) tag

                                if (c.Contains("(yyyy/mm/dd)"))
                                {
                                    DateTime policyDeductDate;
                                    string policyDeductDateS;
                                    string debitDay = medDODebitDay.Text;
                                    if (debitDay != string.Empty)
                                    {
                                        string commenceDate = LaData.PolicyData.CommenceDate.Value.ToString();
                                        if (commenceDate != string.Empty)
                                        {
                                            DateTime comDate = DateTime.Parse(commenceDate);

                                            //policyDeductDate = new DateTime(comDate.Year, comDate.Month, int.Parse(debitDay));
                                            policyDeductDateS = comDate.Year + "/" + comDate.Month + "/" + debitDay;
                                            //dat = c2.Replace("yyyy/mm/dd", policyDeductDate.ToShortDateString());
                                            dat = c2.Replace("yyyy/mm/dd", policyDeductDateS);
                                            c2 = dat;
                                            formattingapplied = true;
                                        }
                                    }
                                }

                                #endregion Substituting the (yyyy/mm/dd) tag

                                #region Substituting the NameSurname tag

                                if (c.Contains("[NameSurname]"))
                                {

                                    dat = c2.Replace("[NameSurname]", medName.Text.Trim() + " " + medSurname.Text.Trim());
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the NameSurname tag

                                #region Substituting the IdNumber tag

                                if (c.Contains("[IdNumber]"))
                                {

                                    dat = c2.Replace("[IdNumber]", medIDNumber.Text.Trim());
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the IdNumber tag

                                #region Substituting the DateOfBirth tag

                                if (c.Contains("[DateOfBirth]"))
                                {
                                    string dateOfBirth = dteDateOfBirth.Value.ToString();
                                    if (dateOfBirth != string.Empty)
                                    {
                                        dat = c2.Replace("[DateOfBirth]", DateTime.Parse(dateOfBirth).ToShortDateString());
                                        c2 = dat;
                                        formattingapplied = true;
                                    }
                                }

                                #endregion Substituting the DateOfBirth tag

                                #region Substituting the WorkPhone tag

                                if (c.Contains("[WorkPhone]"))
                                {

                                    dat = c2.Replace("[WorkPhone]", medWorkPhone.Text.Trim());
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the WorkPhone tag

                                #region Substituting the HomePhone tag

                                if (c.Contains("[HomePhone]"))
                                {

                                    dat = c2.Replace("[HomePhone]", medHomePhone.Text.Trim());
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the HomePhone tag

                                #region Substituting the CellPhone tag

                                if (c.Contains("[CellPhone]"))
                                {

                                    dat = c2.Replace("[CellPhone]", medCellPhone.Text.Trim());
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the CellPhone tag

                                #region Substituting the EmailAddress tag

                                if (c.Contains("[EmailAddress]"))
                                {

                                    dat = c2.Replace("[EmailAddress]", medEMail.Text.Trim());
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the EmailAddress tag

                                #region Substituting the Address123 tag

                                if (c.Contains("[Address123]"))
                                {

                                    dat = c2.Replace("[Address123]", medAddress1.Text.Trim() + ", " + medAddress2.Text.Trim() + ", " + medAddress3.Text.Trim());
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the Address123 tag

                                #region Substituting the Address45PostalCode tag

                                if (c.Contains("[Address45PostalCode]"))
                                {

                                    dat = c2.Replace("[Address45PostalCode]", medSuburb.Text.Trim() + ", " + medTown.Text.Trim() + " " + medPostalCode.Text.Trim());
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the Address45PostalCode tag

                                #region Substituting the TSR tag

                                if (c.Contains("[TSR]"))
                                {

                                    dat = c2.Replace("[TSR]", cmbAgent.Text.Trim());
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the TSR tag

                                #region Substituting the BenName tag

                                if (c.Contains("[BenName]"))
                                {

                                    dat = c2.Replace("[BenName]", medBeneficiaryName1Upg.Text.Trim() + " " + medBeneficiarySurname1Upg.Text.Trim());
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the BenName tag

                                #region Substituting the BenDateOfBirth tag

                                if (c.Contains("[BenDateOfBirth]"))
                                {
                                    string bendateOfBirth = dteBeneficiaryDateOfBirth1Upg.Value.ToString();
                                    if (bendateOfBirth != string.Empty)
                                    {
                                        dat = c2.Replace("[BenDateOfBirth]", DateTime.Parse(bendateOfBirth).ToShortDateString());
                                        c2 = dat;
                                        formattingapplied = true;
                                    }
                                }

                                #endregion Substituting the BenDateOfBirth tag

                                #region Substituting the BenRelation tag

                                if (c.Contains("[BenRelation]"))
                                {
                                    dat = c2.Replace("[BenRelation]", cmbBeneficiaryRelationship1Upg.Text);
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the BenRelation tag

                                #region Substituting the ChildCost tag

                                if (c.Contains("[ChildCost]"))
                                {
                                    string childCost = Methods.GetTableData("select ChildCost from INOption where ID = " + LaData.PolicyData.OptionID).Rows[0]["ChildCost"].ToString();
                                    dat = c2.Replace("[ChildCost]", string.Format("{0:c}", childCost));
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the ChildCost tag

                                #region Substituting the CurrentLA1Cover tag

                                if (c.Contains("[CurrentLA1Cover]"))
                                {
                                    string cover = string.Format("{0:c}", LaData.ImportedCovers[0].Cover);
                                    if (cover == string.Empty)
                                    {
                                        cover = "0.0";
                                    }

                                    dat = c2.Replace("[CurrentLA1Cover]", Methods.CurrencyToWords(string.Format("{0:c}", cover), LaData.ClosureData.ClosureLanguageID, true));
                                    c2 = dat;
                                    formattingapplied = true;

                                    //string cover = string.Format("{0:c}",LaData.ImportedCovers[0].Cover);
                                    //if (cover == string.Empty)
                                    //{
                                    //    cover = "0.0";
                                    //}
                                    //dat = c2.Replace("[CurrentLA1Cover]", string.Format("{0:c}", cover));
                                    //c2 = dat;
                                    //formattingapplied = true;
                                }

                                #endregion Substituting the CurrentLA1Cover tag

                                #region Substituting the CurrentLA2Cover tag

                                if (c.Contains("[CurrentLA2Cover]"))
                                {
                                    string cover = string.Format("{0:c}", LaData.ImportedCovers[4].Cover);
                                    if (cover == string.Empty)
                                    {
                                        cover = "0.0";
                                    }

                                    try //DMM5U solution
                                    {
                                        dat = c2.Replace("[CurrentLA2Cover]", Methods.CurrencyToWords(string.Format("{0:c}", cover), LaData.ClosureData.ClosureLanguageID, true));

                                    }
                                    catch
                                    {

                                    }
                                    c2 = dat;
                                    formattingapplied = true;

                                    //string cover = LaData.ImportedCovers[4].Cover.ToString();
                                    //if(cover == string.Empty)
                                    //{
                                    //    cover = "0.0";
                                    //}
                                    //dat = c2.Replace("[CurrentLA2Cover]", string.Format("{0:c}", cover));
                                    //c2 = dat;
                                    //formattingapplied = true;
                                }

                                #endregion Substituting the CurrentLA2Cover tag

                                #region Substituting the CurrentLA1Death tag

                                if (c.Contains("[CurrentLA1Death]"))
                                {
                                    string cover = string.Format("{0:c}", LaData.ImportedCovers[2].Cover);
                                    if (cover == string.Empty)
                                    {
                                        cover = "0.0";
                                    }

                                    dat = c2.Replace("[CurrentLA1Death]", Methods.CurrencyToWords(string.Format("{0:c}", cover), LaData.ClosureData.ClosureLanguageID, true));
                                    c2 = dat;
                                    formattingapplied = true;

                                    //string cover = string.Format("{0:c}",LaData.ImportedCovers[2].Cover);
                                    //if (cover == string.Empty)
                                    //{
                                    //    cover = "0.0";
                                    //}
                                    //dat = c2.Replace("[CurrentLA1Death]", string.Format("{0:c}", cover));
                                    //c2 = dat;
                                    //formattingapplied = true;
                                }

                                #endregion Substituting the CurrentLA1Death tag

                                #region Substituting the LA1FuneralCover tag

                                if (c.Contains("[LA1FuneralCover]"))
                                {
                                    string la1FuneralCover = "0.00";

                                    if (LaData.PolicyData.OptionID != null)
                                    {
                                        DataTable dtOption = Methods.GetTableData(String.Format("SELECT * FROM [INOption] WHERE [ID] = {0}", LaData.PolicyData.OptionID));
                                        if (dtOption.Rows.Count > 0)
                                        {
                                            if (Convert.ToInt32(NoNull(dtOption.Rows[0]["LA1FuneralCover"], 0)) != 0)
                                            {
                                                la1FuneralCover = Convert.ToDecimal(dtOption.Rows[0]["LA1FuneralCover"]).ToString(CultureInfo.CurrentCulture);
                                            }
                                        }

                                        dat = c2.Replace("[LA1FuneralCover]", Methods.CurrencyToWords(string.Format("{0:c}", la1FuneralCover), LaData.ClosureData.ClosureLanguageID, true));
                                        c2 = dat;
                                        formattingapplied = true;

                                        //dat = c2.Replace("[LA1FuneralCover]", string.Format("{0:c}", la1FuneralCover));
                                        //c2 = dat;
                                        //formattingapplied = true;
                                    }
                                }

                                #endregion Substituting the LA1FuneralCover tag

                                #region Substituting the FuneralCover tag

                                if (c.Contains("[FuneralCover]"))
                                {
                                    string funeralCover = "0.00";
                                    if (!LaData.AppData.IsLeadUpgrade)
                                    {
                                        if (xamCEFuneralCoverLA1.Value != null)
                                        {
                                            funeralCover = xamCEFuneralCoverLA1.Value.ToString();
                                        }
                                    }

                                    //dat = c2.Replace("[FuneralCover]", string.Format("{0:c}", funeralCover));
                                    dat = c2.Replace("[FuneralCover]", Methods.CurrencyToWords(string.Format("{0:c}", funeralCover), LaData.ClosureData.ClosureLanguageID, true));
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the FuneralCover tag


                                #region Substituting the Clients surname tag

                                if (c.Contains("[MoneyBackAge]"))
                                {

                                    dat = c2.Replace("[MoneyBackAge]", LaData.PolicyData.CashBackAge.ToString());
                                    c2 = dat;
                                    formattingapplied = true;
                                }

                                #endregion Substituting the Clients surname tag


                                if (formattingapplied == false)
                                {
                                    dat = c;
                                }

                                newData[index] = dat;
                                index++;
                            }
                            string formattedFile = GlobalSettings.UserFolder + "tempFormatted ~ " + Guid.NewGuid() + ".rtf";
                            File.WriteAllLines(formattedFile, newData);



                            FlowDocument document = new FlowDocument();

                            //Read the file stream to a Byte array 'data'
                            TextRange txtRange;
                            byte[] fileData = File.ReadAllBytes(formattedFile);
                            using (MemoryStream stream = new MemoryStream(fileData))
                            {
                                // create a TextRange around the entire document

                                txtRange = new TextRange(document.ContentStart, document.ContentEnd);

                                txtRange.Load(stream, DataFormats.Rtf);

                            }

                            //Change the font-size of the first occurence of the word thereafter to font size 20. See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/227973609/comments
                            FindTextInFlowDocument("thereafter", document, 20, ref txtRange);
                            string commenceMonth = GetCommenceMonthForClosure();
                            ReplaceTextInFlowDocument("(mm)", document, commenceMonth, ref txtRange);


                            document.ColumnWidth = 1300;


                            MemoryStream xpsDoc = FlowDocumentToXPS(document, 1300, 1000);
                            byte[] bytes = xpsDoc.ToArray();
                            string convXps = GlobalSettings.UserFolder + "tempFormatted ~ " + Guid.NewGuid() + ".xps";
                            File.WriteAllBytes(convXps, bytes);

                            _xpsDocument = new XpsDocument(convXps, FileAccess.Read);
                            dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();

                            File.Delete(_strXpsDoc);
                            File.Delete(formattedFile);


                        }
                        else
                        {

                            _strXpsDoc = GlobalSettings.UserFolder + "temp ~ " + Guid.NewGuid() + ".xps";
                            FileStream objFileStream = new FileStream(_strXpsDoc, FileMode.Create, FileAccess.Write, FileShare.None);
                            objFileStream.Write(data, 0, data.Length);
                            objFileStream.Close();

                            _xpsDocument = new XpsDocument(_strXpsDoc, FileAccess.Read);
                            dvClosure.Document = _xpsDocument.GetFixedDocumentSequence();

                            objFileStream.Close();
                            objFileStream.Dispose();

                        }
                    }
                }
                else
                {
                    LaData.ClosureData.ClosureID = null;
                    chkClosureAccept.IsChecked = false;
                }
            }
        }

        public void FindTextInFlowDocument(string searchString, FlowDocument searchDocument, double fontSize, ref TextRange textRange)
        {
            TextPointer text = searchDocument.ContentStart;
            TextPointer position = searchDocument.ContentStart;
            TextPointer nextPointer = searchDocument.ContentStart;
            textRange = new TextRange(position, nextPointer);


            for (position = searchDocument.ContentStart;
            position != null && position.CompareTo(searchDocument.ContentEnd) <= 0;
            position = position.GetNextContextPosition(LogicalDirection.Forward))
            {
                if (position.CompareTo(searchDocument.ContentEnd) == 0)
                {
                    break;
                }

                String textRun = position.GetTextInRun(LogicalDirection.Forward);
                StringComparison stringComparison = StringComparison.CurrentCulture;
                Int32 indexInRun = textRun.IndexOf(searchString, stringComparison);

                if (indexInRun >= 0)
                {
                    position = position.GetPositionAtOffset(indexInRun);
                    if (position != null)
                    {
                        nextPointer = position.GetPositionAtOffset(searchString.Length);

                        textRange = new TextRange(position, nextPointer);
                        textRange.ApplyPropertyValue(TextElement.FontSizeProperty, fontSize);
                        break;
                    }
                }
            }
        }

        public void ReplaceTextInFlowDocument(string searchString, FlowDocument searchDocument, string replacementString, ref TextRange textRange)
        {
            TextPointer text = searchDocument.ContentStart;
            TextPointer position = searchDocument.ContentStart;
            TextPointer nextPointer = searchDocument.ContentStart;
            textRange = new TextRange(position, nextPointer);


            for (position = searchDocument.ContentStart;
            position != null && position.CompareTo(searchDocument.ContentEnd) <= 0;
            position = position.GetNextContextPosition(LogicalDirection.Forward))
            {
                if (position.CompareTo(searchDocument.ContentEnd) == 0)
                {
                    break;
                }

                String textRun = position.GetTextInRun(LogicalDirection.Forward);
                StringComparison stringComparison = StringComparison.CurrentCulture;
                Int32 indexInRun = textRun.IndexOf(searchString, stringComparison);

                if (indexInRun >= 0)
                {
                    position = position.GetPositionAtOffset(indexInRun);
                    if (position != null)
                    {
                        nextPointer = position.GetPositionAtOffset(searchString.Length);

                        textRange = new TextRange(position, nextPointer);
                        textRange.Text = replacementString;
                        //textRange.ApplyPropertyValue(TextElement.FontSizeProperty, fontSize);
                        //break;
                    }
                }
            }
        }

        public string GetCommenceMonthForClosure()
        {
            string month = String.Empty;
            try
            {

                #region Substituting the (mm) tag
                if (LaData.PolicyData.CommenceDate.HasValue)
                {


                    if (LaData.ClosureData.ClosureLanguageID == (long)lkpLanguage.Afrikaans)
                    {
                        switch (LaData.PolicyData.CommenceDate.Value.Month)
                        {
                            case 1:
                                month = "Januarie";
                                break;
                            case 2:
                                month = "Februarie";
                                break;
                            case 3:
                                month = "Maart";
                                break;
                            case 4:
                                month = "April";
                                break;
                            case 5:
                                month = "Mei";
                                break;
                            case 6:
                                month = "Junie";
                                break;
                            case 7:
                                month = "Julie";
                                break;
                            case 8:
                                month = "Augustus";
                                break;
                            case 9:
                                month = "September";
                                break;
                            case 10:
                                month = "Oktober";
                                break;
                            case 11:
                                month = "November";
                                break;
                            case 12:
                                month = "Desember";
                                break;
                            default:
                                month = "(mm)";
                                break;
                        }
                    }
                    else
                    {
                        switch (LaData.PolicyData.CommenceDate.Value.Month)
                        {
                            case 1:
                                month = "January";
                                break;
                            case 2:
                                month = "February";
                                break;
                            case 3:
                                month = "March";
                                break;
                            case 4:
                                month = "April";
                                break;
                            case 5:
                                month = "May";
                                break;
                            case 6:
                                month = "June";
                                break;
                            case 7:
                                month = "July";
                                break;
                            case 8:
                                month = "August";
                                break;
                            case 9:
                                month = "September";
                                break;
                            case 10:
                                month = "October";
                                break;
                            case 11:
                                month = "November";
                                break;
                            case 12:
                                month = "December";
                                break;
                            default:
                                month = "(mm)";
                                break;
                        }
                    }
                }
                #endregion 
            }


            catch (Exception ex)
            {
                HandleException(ex);
            }
            return month;
        }

        public static MemoryStream FlowDocumentToXPS(FlowDocument flowDocument, int width, int height)
        {

            MemoryStream stream = new MemoryStream();
            using (Package package = Package.Open(stream, FileMode.Create, FileAccess.ReadWrite))
            {
                using (XpsDocument xpsDoc = new XpsDocument(package, CompressionOption.Maximum))
                {
                    XpsSerializationManager rsm = new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);
                    DocumentPaginator paginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator;
                    //paginator.PageSize = new System.Windows.Size(width, height);

                    rsm.SaveAsXaml(paginator);
                    rsm.Commit();
                }
            }
            stream.Position = 0;
            Console.WriteLine(stream.Length);
            Console.WriteLine(stream.Position);
            return stream;
        }
        public void ClearClosureDocument()
        {
            dvClosure.Document = null;
            chkClosureAccept.IsChecked = false;
            chkClosureAccept.IsEnabled = false;
            //chkClosureAfrikaans.IsChecked = false;
            LaData.ClosureData.ClosureLanguageID = null;

            if (_strXpsDoc != null)
            {
                _xpsDocument.Close();

                File.Delete(_strXpsDoc);
                _strXpsDoc = null;
            }
        }

        private void ShowIDNumberField()
        {
            medIDNumber.Visibility = Visibility.Visible;
            medPassportNumber.Visibility = Visibility.Hidden;
            lblID.TextDecorations = TextDecorations.Underline;
            lblPassport.TextDecorations = null;
            medIDNumber.Tag = null;
        }

        private void FocusIDNumberField()
        {
            medIDNumber.Visibility = Visibility.Visible;
            medPassportNumber.Visibility = Visibility.Hidden;
            lblID.TextDecorations = TextDecorations.Underline;
            lblPassport.TextDecorations = null;
            medIDNumber.Focus();
            medIDNumber.Tag = null;
        }

        private void FocusPassportNumberField()
        {
            medIDNumber.Visibility = Visibility.Hidden;
            medPassportNumber.Visibility = Visibility.Visible;
            lblID.TextDecorations = null;
            lblPassport.TextDecorations = TextDecorations.Underline;
            medPassportNumber.Focus();
            medPassportNumber.Tag = null;
        }

        private void cmbStatus_ToolTip(long? importID)
        {
            switch (LaData.AppData.LeadStatus)
            {
                case 2:
                    DataTable dt = Methods.GetTableData("SELECT Description FROM INDeclineReason WHERE ID = '" + LaData.AppData.DeclineReasonID + "'");
                    cmbStatus.ToolTip = dt.Rows.Count > 0 ? dt.Rows[0]["Description"] : null;
                    break;

                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/204782186/comments
                case 7:
                    if (LaData.AppData.CancellationReasonID.HasValue)
                    {
                        DataTable dtCancellationReasons = Methods.GetTableData(string.Format("SELECT [Description] FROM [INCancellationReason] WHERE [ID] = {0}", LaData.AppData.CancellationReasonID));
                        cmbStatus.ToolTip = dtCancellationReasons.Rows.Count > 0 ? dtCancellationReasons.Rows[0]["Description"] : null;
                    }
                    break;

                case 8:
                    if (LaData.AppData.CarriedForwardReasonID.HasValue)
                    {
                        DataTable dtCarriedForwardReasons = Methods.GetTableData(string.Format("SELECT [Description] FROM [INCarriedForwardReason] WHERE [ID] = {0}", LaData.AppData.CarriedForwardReasonID));
                        cmbStatus.ToolTip = dtCarriedForwardReasons.Rows.Count > 0 ? dtCarriedForwardReasons.Rows[0]["Description"] : null;
                    }
                    break;

                case 9:
                    if (LaData.AppData.DiaryReasonID.HasValue)
                    {
                        try
                        {
                            DataTable dtDiaryReasons = Methods.GetTableData(string.Format("SELECT [Description] FROM [INDiaryReason] WHERE [ID] = {0}", LaData.AppData.DiaryReasonID));

                            SqlParameter[] parameters = new SqlParameter[2];
                            parameters[0] = new SqlParameter("@ImportID", importID);
                            parameters[1] = new SqlParameter("@LoggedInUserID", GlobalSettings.ApplicationUser.ID);
                            DataSet dsLookups = Methods.ExecuteStoredProcedure("spINGetLeadByImportID", parameters);
                            string StartDate = dsLookups.Tables[1].Rows[0]["Start"].ToString();

                            cmbStatus.ToolTip = dtDiaryReasons.Rows.Count > 0 ? dtDiaryReasons.Rows[0]["Description"] + " --- " + StartDate : null;
                        }
                        catch
                        {

                        }
                    }
                    break;


                case 10:
                    if (LaData.AppData.FutureContactDate != null)
                    {
                        cmbStatus.ToolTip = LaData.AppData.FutureContactDate.Value.Month + "/" + LaData.AppData.FutureContactDate.Value.Year;
                    }
                    break;

                case 16:
                    if (LaData.AppData.FKINCallMonitoringCancellationReasonID.HasValue)
                    {
                        DataTable dtToolTip = Methods.GetTableData(string.Format("SELECT [Description] FROM [INCallMonitoringCancellationReason] WHERE [ID] = {0}", LaData.AppData.FKINCallMonitoringCancellationReasonID));
                        cmbStatus.ToolTip = dtToolTip.Rows.Count > 0 ? dtToolTip.Rows[0]["Description"] : null;
                    }
                    break;

                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/222494520/comments
                case 17:
                    if (LaData.AppData.FKINCallMonitoringCarriedForwardReasonID.HasValue)
                    {
                        DataTable dtCallMonitoringCancellationReasons = Methods.GetTableData(string.Format("SELECT [Description] FROM [INCallMonitoringCarriedForwardReason] WHERE [ID] = {0}", LaData.AppData.FKINCallMonitoringCarriedForwardReasonID));
                        cmbStatus.ToolTip = dtCallMonitoringCancellationReasons.Rows.Count > 0 ? dtCallMonitoringCancellationReasons.Rows[0]["Description"] : null;
                    }
                    break;



                default:
                    cmbStatus.ToolTip = null;
                    break;
            }
        }

        private long? GetOptionID()
        {
            if (!LaData.AppData.IsLeadLoaded || LaData.AppData.IsLeadSaving) return LaData.PolicyData.OptionID;
            if (LaData.PolicyData.LA1Cover == null)
            {
                chkFuneral.IsChecked = false;
                chkFuneral.IsEnabled = true;
                chkChild.IsChecked = false;
                chkChild.IsEnabled = true;
                chkLA2.IsChecked = false;
                chkLA2.IsEnabled = true;
                return null;
            }

            {
                //enabled untill possibly proved otherwise down below
                chkLA2.IsEnabled = true;
            }


            long? optionID = null;
            decimal? LA1Cover;
            decimal? LA2Cover = 0.00m;
            string strQuery;
            DataTable dt;

            LA1Cover = LaData.PolicyData.LA1Cover;

            EmbriantComboBox cmb = Methods.FindChild<EmbriantComboBox>(PopupChkLA2.Child, "");
            if (cmb != null)
            {
                LA2Cover = Convert.ToDecimal(cmb.SelectedValue);
            }

            //disable LA2 checkbox if LA2Cover is 0 for the current option
            {
                if (LA1Cover > 0)
                {
                    strQuery = "SELECT LA2Cover FROM INOption WHERE FKINPlanID = '" + LaData.PolicyData.PlanID + "' AND LA1Cover = '" + LA1Cover + "' AND IsActive = '1'" + "AND LA2Cover != 0";
                    dt = Methods.GetTableData(strQuery);
                    if (dt.Rows.Count == 0)
                    {
                        LaData.PolicyData.IsLA2Checked = false;
                        chkLA2.IsEnabled = false;
                    }

                    strQuery = "SELECT LA2Cover FROM INOption WHERE FKINPlanID = '" + LaData.PolicyData.PlanID + "' AND LA1Cover = '" + LA1Cover + "' AND IsActive = '1'" + "AND LA2Cover = 0";
                    dt = Methods.GetTableData(strQuery);
                    if (dt.Rows.Count == 0)
                    {
                        LaData.PolicyData.IsLA2Checked = true;
                        chkLA2.IsEnabled = false;
                    }

                    if (LaData.AppData.CampaignType == lkpINCampaignType.FemaleDisCancer || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion)
                    {
                        strQuery = "SELECT FuneralCover FROM INOption WHERE FKINPlanID = '" + LaData.PolicyData.PlanID + "' AND LA1Cover = '" + LA1Cover + "' AND IsActive = '1'" + "AND (LA1FuneralCover != 0 OR LA2FuneralCover != 0)";
                        dt = Methods.GetTableData(strQuery);
                        if (dt.Rows.Count == 0)
                        {
                            LaData.PolicyData.IsFuneralChecked = false;
                            chkFuneral.IsEnabled = false;
                        }
                        else
                        {
                            LaData.PolicyData.IsFuneralChecked = true;
                            chkFuneral.IsEnabled = false;
                        }
                    }
                    else
                    {
                        strQuery = "SELECT FuneralCover FROM INOption WHERE FKINPlanID = '" + LaData.PolicyData.PlanID + "' AND LA1Cover = '" + LA1Cover + "' AND IsActive = '1'" + "AND FuneralCover != 0";
                        dt = Methods.GetTableData(strQuery);
                        if (dt.Rows.Count == 0)
                        {
                            LaData.PolicyData.IsFuneralChecked = false;
                            chkFuneral.IsEnabled = false;
                        }
                        else
                        {
                            if (LaData.AppData.CampaignType == lkpINCampaignType.FemaleDis ||
                                LaData.AppData.CampaignType == lkpINCampaignType.IGFemaleDisability ||
                                LaData.AppData.CampaignType == lkpINCampaignType.FemaleDisCancer ||
                                LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion ||
                                LaData.AppData.CampaignType == lkpINCampaignType.BlackMacc)
                            {
                                LaData.PolicyData.IsFuneralChecked = true;
                                chkFuneral.IsEnabled = false;
                            }
                            else
                            {
                                chkFuneral.IsEnabled = true;
                            }
                        }
                    }

                }
            }

            if (LaData.PolicyData.IsLA2Checked)
            {
                strQuery = "SELECT LA2Cover FROM INOption WHERE FKINPlanID = '" + LaData.PolicyData.PlanID + "' AND LA1Cover = '" + LA1Cover + "' AND IsActive = '1'" + "AND LA2Cover != 0";
                dt = Methods.GetTableData(strQuery);

                if (dt.Rows.Count == 1)
                {
                    LA2Cover = Convert.ToDecimal(dt.Rows[0].ItemArray[0]);
                }
                else if (dt.Rows.Count > 1)
                {
                    if (LA2Cover == 0)
                    {
                        PopupChkLA2.IsOpen = true;
                    }
                }
            }

            strQuery = "SELECT ID FROM INOption WHERE FKINPlanID = '" + LaData.PolicyData.PlanID + "' AND LA1Cover = '" + LA1Cover + "' AND LA2Cover = '" + LA2Cover + "' AND IsActive = '1'";
            dt = Methods.GetTableData(strQuery);
            if (dt.Rows.Count == 1)
            {
                optionID = dt.Rows[0].ItemArray[0] as long?;
            }
            else if (dt.Rows.Count == 0)
            {
                strQuery = "SELECT ID, LA2Cover FROM INOption WHERE FKINPlanID = '" + LaData.PolicyData.PlanID + "' AND LA1Cover = '" + LA1Cover + "' AND IsActive = '1'";
                dt = Methods.GetTableData(strQuery);
                if (dt.Rows.Count > 0)
                {
                    optionID = dt.Rows[0].ItemArray[0] as long?;
                    LA2Cover = Convert.ToDecimal(dt.Rows[0].ItemArray[1]);
                }

                if (LA2Cover > 0)
                {
                    LaData.PolicyData.IsLA2Checked = true;
                }
            }

            cmbOptionCode.SelectedValue = optionID;
            return optionID;
        }


        private long? GetGenderIDFromTitle(long? titleID)
        {
            long? resultingGenderID = null;

            #region Get the data from database

            if (titleID.HasValue)
            {

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@TitleID", titleID.Value);

                DataTable dtResultingGenderID = Methods.ExecuteStoredProcedure("spINGetTitleGender", parameters).Tables[0];

                if (dtResultingGenderID.Rows.Count > 0)
                {
                    resultingGenderID = dtResultingGenderID.Rows[0][0] as long?;
                }
            }

            #endregion Get the data from database

            return resultingGenderID;
        }

        public void ShowOrHideFields(bool show)
        {
            Visibility visibility;

            if (show)
            {
                #region Enable all controls

                visibility = Visibility.Visible;

                //cmbStatus.Style = FindResource("StyleCMBValidate") as Style;
                LaData.AppData.CanChangeStatus = true; //cmbStatus.IsEnabled = true;
                cmbAgent.Style = FindResource("StyleCMBValidate") as Style;
                dteDateOfSale.Style = FindResource("INDTE1Validate") as Style;
                cmbTitle.Style = FindResource("StyleCMBValueChanged") as Style;
                medInitials.Style = FindResource("StyleXMEValueChanged") as Style;
                medName.Style = FindResource("StyleXMEValueChangedAndUpgDisabled") as Style;
                medSurname.Style = FindResource("StyleXMEValueChangedAndUpgDisabled") as Style;

                #endregion Enable all controls
            }
            else
            {
                #region Disable all controls

                visibility = Visibility.Hidden;

                //cmbStatus.Style = FindResource("cmbINDisabled") as Style;
                LaData.AppData.CanChangeStatus = false; //cmbStatus.IsEnabled = false;
                cmbAgent.Style = FindResource("cmbINDisabled") as Style;
                dteDateOfSale.Style = FindResource("INXamDateTimeEditorStyle3") as Style;
                cmbTitle.Style = FindResource("cmbINDisabled") as Style;
                medInitials.Style = FindResource("medINDisabled") as Style;
                medName.Style = FindResource("medINDisabled") as Style;
                medSurname.Style = FindResource("medINDisabled") as Style;

                #endregion Disable all controls
            }

            btnPrevious.Visibility = visibility;
            btnNext.Visibility = visibility;

            #region Page 1 Controls

            lblID.Visibility = visibility;
            lblSeperator.Visibility = visibility;
            lblPassport.Visibility = visibility;
            lblNumber.Visibility = visibility;

            medIDNumber.Visibility = visibility;
            medPassportNumber.Visibility = visibility;

            lblDateOfBirth.Visibility = visibility;
            dteDateOfBirth.Visibility = visibility;

            lblGender.Visibility = visibility;
            cmbGender.Visibility = visibility;

            lblWorkPhone.Visibility = visibility;
            medWorkPhone.Visibility = visibility;

            lblHomePhone.Visibility = visibility;
            medHomePhone.Visibility = visibility;

            lblCellPhone.Visibility = visibility;
            medCellPhone.Visibility = visibility;

            lblOtherPhone.Visibility = visibility;
            medOtherPhone.Visibility = visibility;

            lbllapseDate.Visibility = visibility;
            dteLapseDate.Visibility = visibility;

            lblAddress.Visibility = visibility;
            medAddress1.Visibility = visibility;
            medAddress2.Visibility = visibility;
            medAddress3.Visibility = visibility;
            medSuburb.Visibility = visibility;
            medTown.Visibility = visibility;

            lblPostalCode.Visibility = visibility;
            medPostalCode.Visibility = visibility;
            //btnChangeAddress.Visibility = visibility;

            lblReferrorTitle.Visibility = visibility;
            cmbReferrorTitle.Visibility = visibility;
            lblReferrorName.Visibility = visibility;
            medReferrorName.Visibility = visibility;

            lblReferrorRelationship.Visibility = visibility;
            cmbReferrorRelationship.Visibility = visibility;

            lblEMail.Visibility = visibility;
            medEMail.Visibility = visibility;

            tbConfirm.Visibility = visibility;
            chkConfirm.Visibility = visibility;

            //This has been commented out by Nicolas. See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/227995056/comments
            //lblClosure.Visibility = visibility;
            //chkClosure.Visibility = visibility;

            #endregion Page 1 Controls

            btnSave.Visibility = visibility;

            if (show)
            {
                FocusIDNumberField();
            }
        }



        private void Viewbox_Zoom(object sender)
        {
            Viewbox viewbox = (Viewbox)sender;
            const double zoomWidth = 1;
            const double zoomHeight = 1;

            if (viewbox != null && Convert.ToInt32(viewbox.Tag.ToString()) != 1)
            {
                vb1.Tag = 0; vb2.Tag = 0; vb3.Tag = 0; vb4.Tag = 0;

                switch (viewbox.Name)
                {
                    case "vb1":
                        UpgradeLayout.ColumnDefinitions[0].Width = new GridLength(zoomWidth, GridUnitType.Star);
                        UpgradeLayout.ColumnDefinitions[2].Width = new GridLength(1 - zoomWidth, GridUnitType.Star);
                        UpgradeLayout.RowDefinitions[0].Height = new GridLength(zoomHeight, GridUnitType.Star);
                        UpgradeLayout.RowDefinitions[2].Height = new GridLength(1 - zoomHeight, GridUnitType.Star);
                        vb1.Tag = 1;
                        break;

                    case "vb2":
                        UpgradeLayout.ColumnDefinitions[0].Width = new GridLength(1 - zoomWidth, GridUnitType.Star);
                        UpgradeLayout.ColumnDefinitions[2].Width = new GridLength(zoomWidth, GridUnitType.Star);
                        UpgradeLayout.RowDefinitions[0].Height = new GridLength(zoomHeight, GridUnitType.Star);
                        UpgradeLayout.RowDefinitions[2].Height = new GridLength(1 - zoomHeight, GridUnitType.Star);
                        vb2.Tag = 1;
                        break;

                    case "vb3":
                        UpgradeLayout.ColumnDefinitions[0].Width = new GridLength(zoomWidth, GridUnitType.Star);
                        UpgradeLayout.ColumnDefinitions[2].Width = new GridLength(1 - zoomWidth, GridUnitType.Star);
                        UpgradeLayout.RowDefinitions[0].Height = new GridLength(1 - zoomHeight, GridUnitType.Star);
                        UpgradeLayout.RowDefinitions[2].Height = new GridLength(zoomHeight, GridUnitType.Star);
                        vb3.Tag = 1;
                        break;

                    case "vb4":
                        UpgradeLayout.ColumnDefinitions[0].Width = new GridLength(1 - zoomWidth, GridUnitType.Star);
                        UpgradeLayout.ColumnDefinitions[2].Width = new GridLength(zoomWidth, GridUnitType.Star);
                        UpgradeLayout.RowDefinitions[0].Height = new GridLength(1 - zoomHeight, GridUnitType.Star);
                        UpgradeLayout.RowDefinitions[2].Height = new GridLength(zoomHeight, GridUnitType.Star);
                        vb4.Tag = 1;
                        break;
                }
            }
        }

        private void Viewbox_Normal()
        {
            if (!GridSplitterH.IsMouseOver && !GridSplitterV.IsMouseOver)
            {
                const double zoomWidth = 0.5;
                const double zoomHeight = 0.5;

                UpgradeLayout.ColumnDefinitions[0].Width = new GridLength(zoomWidth, GridUnitType.Star);
                UpgradeLayout.ColumnDefinitions[2].Width = new GridLength(1 - zoomWidth, GridUnitType.Star);
                UpgradeLayout.RowDefinitions[0].Height = new GridLength(zoomHeight, GridUnitType.Star);
                UpgradeLayout.RowDefinitions[2].Height = new GridLength(1 - zoomHeight, GridUnitType.Star);

                vb1.Tag = vb2.Tag = vb3.Tag = vb4.Tag = 0;
            }
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

        private void CloseScriptWindows()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType().Name == "ScriptScreen")
                {
                    window.Close();
                    return;
                }
            }
        }

        private void CheckIfMoneyBuilderAccount()
        {
            INMessageBoxWindow2 messageBox = new INMessageBoxWindow2();
            messageBox.buttonOK.Content = "Yes";
            messageBox.buttonCancel.Content = "No";
            if (LaData.UserData.StaffType == lkpStaffType.Permanent)
            {
                messageBox.BGRectangle.Opacity = 0.45;
            }

            var showMessageBox = ShowMessageBox(messageBox, "Is this a Money Builder Account?", "Money Builder", ShowMessageType.Information);
            bool result = showMessageBox != null && (bool)showMessageBox;

            if (result)
            {


                INMessageBoxWindow1 moneyBuilderMessage = new INMessageBoxWindow1();

                if (LaData.UserData.StaffType == lkpStaffType.Permanent)
                {
                    moneyBuilderMessage.BGRectangle.Opacity = 0.45;
                }

                ShowMessageBox(moneyBuilderMessage, "You cannot use a Money Builder Account!", "Money Builder", ShowMessageType.Exclamation);

                LaData.BankDetailsData.AccountNumber = "";
                medDOAccountNumber.Focus();
            }
        }

        #endregion Private Methods

        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.Title == "Redeem Gift")
                {
                    window.Close();
                }
            }

            //CloseCTPhone();
            CloseScriptWindows();
            OnDialogClose(_dialogResult);
        }

        private void xamEditor_Loaded(object sender, RoutedEventArgs e)
        {
            //xamEditor_SetMask(sender);
        }

        public void CancerOptionSpouseWorking(string strInput)
        {

            if (!string.IsNullOrWhiteSpace(strInput) && strInput.Length >= 6)
            {
                strInput = strInput.Substring(0, 6);
                string strYY = strInput.Substring(0, 2);
                string strMM = strInput.Substring(2, 2);
                string strDD = strInput.Substring(4, 2);

                //Build output date string
                string strOutput = int.Parse(strYY) > 10 ? "19" + strYY : "20" + strYY;
                strOutput = strOutput + "/" + strMM + "/" + strDD;

                DateTime date;
                bool result = DateTime.TryParse(strOutput, out date);


                #region PLCBSpouse Campaign Auto CancerOption Update

                if (LaData.AppData.CampaignID == 344 || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade5 || LaData.AppData.CampaignType == lkpINCampaignType.BlackMaccMillion && LaData.AppData.CampaignGroup == lkpINCampaignGroup.Upgrade6)
                {
                    decimal? selectedLA1Cover = LaData.PolicyData.LA1Cover;
                    int age = DateTime.Now.Year - date.Year;
                    int optionID = 0;

                    string strQuery1 = "SELECT DISTINCT CancerOption FROM INImport ";
                    strQuery1 += "WHERE ID = '" + LaData.AppData.ImportID + "'";

                    DataTable dtCancerOption = Methods.GetTableData(strQuery1);
                    long? Canceroption;
                    try
                    {
                        Canceroption = long.Parse(dtCancerOption.Rows[0]["CancerOption"].ToString());
                    }
                    catch
                    {
                        Canceroption = null;
                    }

                    if (Canceroption != null)
                    {

                    }
                    else
                    {
                        if (age >= 18 && age <= 34)
                        {
                            optionID = 1;
                        }
                        else if (age >= 35 && age <= 39)
                        {
                            optionID = 2;
                        }
                        else if (age >= 40 && age <= 44)
                        {
                            optionID = 3;
                        }
                        else if (age >= 45 && age <= 49)
                        {
                            optionID = 4;
                        }
                        else if (age >= 50 && age <= 54)
                        {
                            optionID = 5;
                        }
                        else if (age >= 55 && age <= 150)
                        {
                            optionID = 6;
                        }

                        if (LaData.PolicyData.PlanGroupID != null)
                        {
                            string strQuery = "SELECT DISTINCT ID, PlanCode [Description] FROM INPlan ";
                            strQuery += "WHERE FKINPlanGroupID = '" + LaData.PolicyData.PlanGroupID + "'";

                            DataTable dtPolicyPlan = Methods.GetTableData(strQuery);
                            cmbPolicyPlan.Populate(dtPolicyPlan, DescriptionField, IDField);

                            LaData.PolicyData.PlanID = ((DataView)cmbPolicyPlan.ItemsSource).Table.AsEnumerable().Where(r => r["Description"] as string == "Option " + optionID).Select(r => r["ID"] as long?).FirstOrDefault();
                        }
                        else
                        {
                            LaData.PolicyData.PlanID = null;
                            cmbPolicyPlan.ItemsSource = null;
                            LaData.PolicyData.OptionID = null;
                            cmbOptionCode.ItemsSource = null;
                            LaData.PolicyData.LA1Cover = null;
                            cmbLA1Cover.ItemsSource = null;
                        }
                    }


                }

                #endregion





            }

        }

        private void xamEditor_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            //xamEditor_SetMask(sender);

            var xme = sender as XamMaskedEditor;

            if (xme != null && xme.Value != null)
            {
                switch (xme.Name)
                {
                    case "medIDNumber":
                        LaData.LeadData.DateOfBirth = IDNumberToDOB(xme.Text);
                        CancerOptionSpouseWorking(xme.Text);
                        break;

                    case "medLA2IDNumber":
                        if (!LaData.AppData.IsLeadUpgrade) LaData.LAData[1].DateOfBirth = IDNumberToDOB(xme.Text);
                        break;

                        //case "medBeneficiaryIDNumber1":
                        //    if (!LaData.AppData.IsLeadUpgrade) LaData.BeneficiaryData[0].DateOfBirth = IDNumberToDOB(xme.Text);
                        //    break;

                        //case "medBeneficiaryIDNumber2":
                        //    if (!LaData.AppData.IsLeadUpgrade) LaData.BeneficiaryData[1].DateOfBirth = IDNumberToDOB(xme.Text);
                        //    break;

                        //case "medBeneficiaryIDNumber3":
                        //    if (!LaData.AppData.IsLeadUpgrade) LaData.BeneficiaryData[2].DateOfBirth = IDNumberToDOB(xme.Text);
                        //    break;

                        //case "medBeneficiaryIDNumber4":
                        //    if (!LaData.AppData.IsLeadUpgrade) LaData.BeneficiaryData[3].DateOfBirth = IDNumberToDOB(xme.Text);
                        //    break;

                        //case "medBeneficiaryIDNumber5":
                        //    if (!LaData.AppData.IsLeadUpgrade) LaData.BeneficiaryData[4].DateOfBirth = IDNumberToDOB(xme.Text);
                        //    break;

                        //case "medBeneficiaryIDNumber6":
                        //    if (!LaData.AppData.IsLeadUpgrade) LaData.BeneficiaryData[5].DateOfBirth = IDNumberToDOB(xme.Text);
                        //    break;
                }
            }
        }

        private void xamEditor_GotFocus(object sender, RoutedEventArgs e)
        {
            //xamEditor_SetMask(sender);
            xamEditor_Select(sender);

            switch (sender.GetType().Name)
            {
                case "XamDateTimeEditor":
                    var xamDTEControl = (XamDateTimeEditor)sender;

                    switch (xamDTEControl.Name)
                    {
                        case "dteDateOfBirth":
                            if (medPassportNumber.Tag as string == "Focus")
                            {
                                FocusPassportNumberField();
                            }
                            if (medIDNumber.Tag as string == "Show")
                            {
                                ShowIDNumberField();
                            }
                            break;
                    }
                    break;

                case "XamMaskedEditor":
                    var xamXMEControl = (XamMaskedEditor)sender;

                    switch (xamXMEControl.Name)
                    {
                        case "medReference":
                            if (LaData.AppData.IsLeadUpgrade)
                            {
                                lblPage.Text = "(Upgrade)";
                            }
                            break;
                    }
                    break;
            }
        }

        private void xamEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            //xamEditor_SetMask(sender);
            xamEditor_Select(sender);

            switch (sender.GetType().Name)
            {
                case "XamMaskedEditor":
                    var xamMEDControl = (XamMaskedEditor)sender;

                    switch (xamMEDControl.Name)
                    {
                        case "medName":
                            //if (string.IsNullOrWhiteSpace(LaData.LeadData.Initials)) 
                            LaData.LeadData.Initials = Methods.ExtractInitialsFromFirstName(LaData.LeadData.Name);
                            break;

                        case "medDOName":
                            //if (string.IsNullOrWhiteSpace(LaData.BankDetailsData.Initials)) 
                            LaData.BankDetailsData.Initials = Methods.ExtractInitialsFromFirstName(LaData.BankDetailsData.Name);
                            break;

                        case "medDOSurname":
                            //if (string.IsNullOrWhiteSpace(LaData.BankDetailsData.AccountHolder)) 
                            LaData.BankDetailsData.AccountHolder = (LaData.BankDetailsData.Initials + " " + LaData.BankDetailsData.Surname).Trim();
                            break;

                        case "medIDNumber":
                            if (xamMEDControl.Tag != null)
                            {
                                xamMEDControl.Text = xamMEDControl.Tag as string;
                            }
                            xamMEDControl.Tag = null;

                            xamEditor_TextChanged(sender, null);
                            break;

                        case "medPassportNumber":
                        case "medLA1IDNumber":
                        case "medLA2IDNumber":
                        case "medDOIDNumber":
                        case "medBeneficiaryIDNumber1":
                        case "medBeneficiaryIDNumber2":
                        case "medBeneficiaryIDNumber3":
                        case "medBeneficiaryIDNumber4":
                        case "medBeneficiaryIDNumber5":
                        case "medBeneficiaryIDNumber6":
                            if (xamMEDControl.Tag != null)
                            {
                                xamMEDControl.Text = xamMEDControl.Tag as string;
                            }
                            xamMEDControl.Tag = null;

                            xamEditor_TextChanged(sender, null);
                            break;

                        case "medWorkPhone":
                        case "medHomePhone":
                        case "medCellPhone":
                        case "medOtherPhone":
                        case "medDOHomePhone":
                        case "medDOCellPhone":
                        case "medDOWorkPhone":
                            if (xamMEDControl.Tag != null)
                            {
                                xamMEDControl.Text = xamMEDControl.Tag as string;
                            }
                            xamMEDControl.Tag = null;
                            break;

                        case "medEMail":
                            if (xamMEDControl.Tag != null)
                            {
                                xamMEDControl.Text = xamMEDControl.Tag as string;
                            }
                            xamMEDControl.Tag = null;
                            break;

                            //default:
                            //    xamMEDControl.Text = CapatalizeWords(xamMEDControl.Name, xamMEDControl.Text);
                            //    break;
                    }

                    xamMEDControl.Text = CapitalizeWords(xamMEDControl.Name, xamMEDControl.Text);
                    break;
            }
        }

        private void xamEditor_EditModeValidationError(object sender, EditModeValidationErrorEventArgs e)
        {
            switch (sender.GetType().Name)
            {
                case "XamMaskedEditor":
                    var xamMEDControl = (XamMaskedEditor)sender;

                    switch (xamMEDControl.Name)
                    {
                        case "medIDNumber":
                            xamMEDControl.Tag = xamMEDControl.Text;
                            xamMEDControl.Text = "0000000000000";
                            return;

                        case "medPassportNumber":
                        case "medLA1IDNumber":
                        case "medLA2IDNumber":
                        case "medDOIDNumber":
                        case "medBeneficiaryIDNumber1":
                        case "medBeneficiaryIDNumber2":
                        case "medBeneficiaryIDNumber3":
                        case "medBeneficiaryIDNumber4":
                        case "medBeneficiaryIDNumber5":
                        case "medBeneficiaryIDNumber6":
                            xamMEDControl.Tag = xamMEDControl.Text;
                            xamMEDControl.Text = "0000000000000";
                            return;

                        case "medWorkPhone":
                        case "medHomePhone":
                        case "medCellPhone":
                        case "medOtherPhone":
                        case "medDOHomePhone":
                        case "medDOCellPhone":
                        case "medDOWorkPhone":
                            xamMEDControl.Tag = xamMEDControl.Text;
                            xamMEDControl.Text = "0000000000";
                            return;

                        case "medEMail":
                            xamMEDControl.Tag = xamMEDControl.Text;
                            xamMEDControl.Text = string.Empty;
                            return;
                    }

                    //xamMEDControl.Focus();
                    //Keyboard.Focus(xamMEDControl);
                    //xamMEDControl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    break;

                case "XamDateTimeEditor":
                    var xamDTEControl = (XamDateTimeEditor)sender;

                    switch (xamDTEControl.Name)
                    {
                        default:
                            ShowMessageBox(new INMessageBoxWindow1(), "Date is not valid.", "Invalid Data", ShowMessageType.Error);
                            break;
                    }

                    //xamDTEControl.Focus();
                    //Keyboard.Focus(xamDTEControl);
                    //xamDTEControl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    break;
            }
        }

        private void medReference_Loaded(object sender, RoutedEventArgs e)
        {
            Methods.FindChild<TextBox>(medReference, "PART_InputTextBox").Focus();
        }

        private void medReference_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        if (!string.IsNullOrWhiteSpace(LaData.AppData.RefNo) && (LaData.AppData.RefNo != LaData.AppData.LoadedRefNo))
                        {
                            ClearApplicationScreen();

                            DataTable dt = Methods.GetTableData("SELECT ID FROM INImport WHERE RefNo = '" + LaData.AppData.RefNo + "'");

                            switch (dt.Rows.Count)
                            {
                                case 0:
                                    ShowOrHideFields(true);
                                    ShowMessageBox(new INMessageBoxWindow1(), LaData.AppData.RefNo + " is not a valid reference number.", "Invalid Reference Number.", ShowMessageType.Exclamation);
                                    medReference.Focus();
                                    break;

                                case 1:
                                    long? fkINImportID = Convert.ToInt64(dt.Rows[0]["ID"].ToString());

                                    #region Determining whether or not the lead was allocated

                                    bool hasLeadBeenAllocated = Insure.HasLeadBeenAllocated(fkINImportID);

                                    if (!hasLeadBeenAllocated)
                                    {
                                        ShowOrHideFields(true);
                                        INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                                        ShowMessageBox(messageWindow, @"This lead cannot be loaded, because it has not been allocated yet. Please consult your supervisor.", "Lead not allocated", ShowMessageType.Exclamation);
                                        break;
                                    }

                                    #endregion Determining whether or not the lead was allocated

                                    bool isNotesFeatureAvailable = Insure.IsNotesFeatureAvailable(fkINImportID);

                                    #region Determining whether or not the lead has a status of cancelled
                                    // Please see https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/204734160/comments
                                    // Please see https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/211742618/comments

                                    //bool hasLeadBeenCancelled = Insure.HasLeadBeenCancelled(fkINImportID);

                                    //if (hasLeadBeenCancelled)
                                    //{
                                    //    ShowOrHideFields(true);
                                    //    INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                                    //    ShowMessageBox(messageWindow, @"This lead cannot be loaded, because the policy has been cancelled by the client. Please consult your supervisor.", "Cancelled Policy", ShowMessageType.Exclamation);
                                    //    break;
                                    //}

                                    #endregion Determining whether or not the lead has a status of cancelled

                                    LaData.AppData.CanClientBeContacted = Insure.CanClientBeContacted(fkINImportID); //Insure.CanClientBeContacted(Convert.ToInt64(dt.Rows[0]["ID"].ToString()));
                                    NotifyUserIfClientCannotBeContacted(LaData.AppData.CanClientBeContacted);
                                    ShowOrHideFields(LaData.AppData.CanClientBeContacted);

                                    LoadLead(Convert.ToInt64(dt.Rows[0]["ID"].ToString()));

                                    if (isNotesFeatureAvailable)
                                    {
                                        ShowNotes(fkINImportID.Value);
                                    }

                                    break;

                                default:
                                    SelectLeadCampaignScreen selectLeadCampaignScreen = new SelectLeadCampaignScreen(LaData.AppData.RefNo);

                                    ShowOrHideFields(true);

                                    if (ShowDialog(selectLeadCampaignScreen, new INDialogWindow(selectLeadCampaignScreen)) == true)
                                    {
                                        long importID = selectLeadCampaignScreen.ImportID;
                                        hasLeadBeenAllocated = Business.Insure.HasLeadBeenAllocated(importID);
                                        //hasLeadBeenCancelled = Business.Insure.HasLeadBeenCancelled(importID);
                                        bool canClientBeContacted = Business.Insure.CanClientBeContacted(importID);
                                        isNotesFeatureAvailable = Insure.IsNotesFeatureAvailable(importID);

                                        #region Checking if the lead to be loaded has a status of "DO NOT CONTACT"

                                        if (!canClientBeContacted)
                                        {
                                            UDM.Insurance.Interface.Windows.INMessageBoxWindow1 messageWindow = new UDM.Insurance.Interface.Windows.INMessageBoxWindow1();
                                            ShowMessageBox(messageWindow, @"This lead cannot be loaded, because the client has requested not to be contacted again.", "DO NOT CONTACT CLIENT", Embriant.Framework.ShowMessageType.Exclamation);
                                            break;
                                            //OnDialogClose(false);
                                        }

                                        #endregion Checking if the lead to be loaded has a status of "DO NOT CONTACT"

                                        #region Determining whether or not the lead was allocated

                                        if (!hasLeadBeenAllocated)
                                        {
                                            UDM.Insurance.Interface.Windows.INMessageBoxWindow1 messageWindow = new UDM.Insurance.Interface.Windows.INMessageBoxWindow1();
                                            ShowMessageBox(messageWindow, @"This lead cannot be loaded, because it has not been allocated yet. Please consult your supervisor.", "Lead not allocated", Embriant.Framework.ShowMessageType.Exclamation);
                                            //OnDialogClose(false);
                                            break;
                                        }

                                        #endregion Determining whether or not the lead was allocated

                                        #region Determining whether or not the lead has a status of cancelled
                                        // Please see https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/204734160/comments
                                        // Please see https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/211742618/comments

                                        //else if (hasLeadBeenCancelled)
                                        //{
                                        //    UDM.Insurance.Interface.Windows.INMessageBoxWindow1 messageWindow = new UDM.Insurance.Interface.Windows.INMessageBoxWindow1();
                                        //    ShowMessageBox(messageWindow, @"This lead cannot be loaded, because the policy has been cancelled by the client. Please consult your supervisor.", "Cancelled Policy", Embriant.Framework.ShowMessageType.Exclamation);
                                        //    //OnDialogClose(false);
                                        //    break;
                                        //}

                                        #endregion Determining whether or not the lead has a status of cancelled


                                        //LoadLead(selectLeadCampaignScreen.ImportID);
                                        LoadLead(importID);

                                        if (isNotesFeatureAvailable)
                                        {
                                            ShowNotes(importID);
                                        }
                                    }

                                    break;
                            }
                        }
                        break;

                    case Key.Insert:
                        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                        {
                            switch (sender.GetType().Name)
                            {
                                case "XamMaskedEditor":
                                    ((XamMaskedEditor)sender).Paste();
                                    e.Handled = true;
                                    break;
                            }
                        }
                        break;

                    case Key.Tab:
                        if (LaData.AppData.IsLeadUpgrade)
                        {
                            cmbAgent.Focus();
                            e.Handled = true;
                        }
                        else
                        {
                            Border visiblePage = _pages.First(b => b.IsVisible.Equals(true));

                            switch (visiblePage.Name)
                            {
                                case "Page1":
                                    cmbAgent.Focus();
                                    e.Handled = true;
                                    break;
                            }
                        }
                        break;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void EmbriantComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Methods.EmbriantComboBoxPreviewKeyDown(sender, e);

            EmbriantComboBox cmb = sender as EmbriantComboBox;
            if (cmb != null)
            {
                if (e.Key == Key.Back)
                {
                    switch (cmb.Name)
                    {
                        case "cmbLA1Cover":
                            LaData.PolicyData.TotalPremium = null;
                            LaData.PolicyData.MoneyBackPayout = null;
                            LaData.PolicyData.MoneyBackPayoutAge = null;
                            break;
                    }
                }
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (ClosurePage.IsVisible)
            {
                ClosurePage.Visibility = Visibility.Hidden;
                ((Border)ClosurePage.Tag).Visibility = Visibility.Visible;
                ClosurePage.Tag = null;
                lblPage.Text = lblPage.Tag.ToString();
                lblPage.Tag = null;
                return;
            }

            if (LaData.AppData.IsLeadUpgrade)
            {
                if (vb1.Tag as int? == 1)
                {
                    Viewbox_Zoom(vb2);
                    Keyboard.Focus(cmbPolicyPlanGroup);
                    lblPage.Text = "(Policy)";
                }
                else if (vb2.Tag as int? == 1)
                {
                    Viewbox_Zoom(vb3);
                    Keyboard.Focus(cmbLA2Title);
                    lblPage.Text = "(LA + Beneficiary)";
                }
                else if (vb3.Tag as int? == 1)
                {
                    Viewbox_Zoom(vb4);
                    Keyboard.Focus(medSaleStationNo);
                    lblPage.Text = "(Sale)";
                }
                else if (vb4.Tag as int? == 1)
                {
                    Viewbox_Zoom(vb1);
                    Keyboard.Focus(cmbAgent);
                    lblPage.Text = "(Lead)";
                }
            }
            else
            {
                if (LaData.AppData.CampaignGroup == lkpINCampaignGroup.Extension)
                {
                    if (Page1.IsVisible)
                    {
                        Page1.Visibility = Visibility.Collapsed;
                        Page5.Visibility = Visibility.Visible;
                        lblMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        chkMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        lblPage.Text = "(Lead)";
                    }
                    else if (Page5.IsVisible)
                    {
                        Page5.Visibility = Visibility.Collapsed;
                        Page1.Visibility = Visibility.Visible;
                        lblMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        chkMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                    }

                }
                else
                {

                    if (Page1.IsVisible)
                    {
                        Page1.Visibility = Visibility.Collapsed;
                        Page2.Visibility = Visibility.Visible;
                        lblMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        chkMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        lblPage.Text = "(Policy)";
                    }
                    else if (Page2.IsVisible)
                    {
                        Page2.Visibility = Visibility.Collapsed;
                        Page3.Visibility = Visibility.Visible;
                        if (LaData.AppData.CampaignID == 102 ||
                            LaData.AppData.CampaignID == 2 ||
                            LaData.AppData.CampaignID == 103 ||
                            LaData.AppData.CampaignID == 250 ||
                            LaData.AppData.CampaignID == 6 ||
                            LaData.AppData.CampaignID == 105)
                        {
                            lblMoveToLeadPermissions.Visibility = Visibility.Visible;
                            chkMoveToLeadPermissions.Visibility = Visibility.Visible;
                        }

                        lblPage.Text = "(Banking)";
                    }
                    else if (Page3.IsVisible)
                    {
                        Page3.Visibility = Visibility.Collapsed;
                        Page4.Visibility = Visibility.Visible;
                        lblMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        chkMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        lblPage.Text = "(LA + Beneficiary)";
                    }
                    else if (Page4.IsVisible)
                    {
                        Page4.Visibility = Visibility.Collapsed;
                        Page5.Visibility = Visibility.Visible;
                        lblMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        chkMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        lblPage.Text = "(Sale)";
                    }
                    else if (Page5.IsVisible)
                    {
                        Page5.Visibility = Visibility.Collapsed;
                        Page1.Visibility = Visibility.Visible;
                        lblMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        chkMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        lblPage.Text = "(Lead)";
                    }

                    if (Page3.IsVisible)
                    {
                        // Carry over value from lead details to account details.
                        if (string.IsNullOrEmpty(medDOInitials.Text))
                            medDOInitials.Text = medInitials.Text;
                        if (string.IsNullOrEmpty(medDOName.Text))
                            medDOName.Text = medName.Text;
                        if (string.IsNullOrEmpty(medDOSurname.Text))
                            medDOSurname.Text = medSurname.Text;
                        if (string.IsNullOrEmpty(medDOIDNumber.Text))
                            medDOIDNumber.Text = medIDNumber.Text;
                        if (string.IsNullOrEmpty(medDOWorkPhone.Text))
                            medDOWorkPhone.Text = medWorkPhone.Text;
                        if (string.IsNullOrEmpty(medDOHomePhone.Text))
                            medDOHomePhone.Text = medHomePhone.Text;
                        if (string.IsNullOrEmpty(medDOCellPhone.Text))
                            medDOCellPhone.Text = medCellPhone.Text;
                    }
                }

                medReference.Focus();
            }
        }


        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (ClosurePage.IsVisible)
            {
                ClosurePage.Visibility = Visibility.Hidden;
                ((Border)ClosurePage.Tag).Visibility = Visibility.Visible;
                ClosurePage.Tag = null;
                lblPage.Text = lblPage.Tag.ToString();
                lblPage.Tag = null;
                return;
            }

            if (LaData.AppData.IsLeadUpgrade)
            {
                if (vb1.Tag as int? == 1)
                {
                    Viewbox_Zoom(vb4);
                    Keyboard.Focus(medSaleStationNo);
                    lblPage.Text = "(Sale)";
                }
                else if (vb2.Tag as int? == 1)
                {
                    Viewbox_Zoom(vb1);
                    Keyboard.Focus(cmbAgent);
                    lblPage.Text = "(Lead)";
                }
                else if (vb3.Tag as int? == 1)
                {
                    Viewbox_Zoom(vb2);
                    Keyboard.Focus(cmbPolicyPlanGroup);
                    lblPage.Text = "(Policy)";
                }
                else if (vb4.Tag as int? == 1)
                {
                    Viewbox_Zoom(vb3);
                    Keyboard.Focus(cmbLA2Title);
                    lblPage.Text = "(LA + Beneficiary)";
                }
            }
            else
            {
                if (LaData.AppData.CampaignGroup == lkpINCampaignGroup.Extension)
                {
                    if (Page1.IsVisible)
                    {
                        Page1.Visibility = Visibility.Collapsed;
                        Page5.Visibility = Visibility.Visible;
                        lblMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        chkMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        lblPage.Text = "(Lead)";
                    }
                    else if (Page5.IsVisible)
                    {
                        Page5.Visibility = Visibility.Collapsed;
                        Page1.Visibility = Visibility.Visible;
                        lblMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        chkMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                    }

                }
                else
                {
                    if (Page1.IsVisible)
                    {
                        Page1.Visibility = Visibility.Collapsed;
                        Page5.Visibility = Visibility.Visible;
                        lblMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        chkMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        lblPage.Text = "(Sale)";
                    }
                    else if (Page2.IsVisible)
                    {
                        Page2.Visibility = Visibility.Collapsed;
                        Page1.Visibility = Visibility.Visible;
                        lblMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        chkMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        lblPage.Text = "(Lead)";
                    }
                    else if (Page3.IsVisible)
                    {
                        Page3.Visibility = Visibility.Collapsed;
                        Page2.Visibility = Visibility.Visible;
                        lblMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        chkMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        lblPage.Text = "(Policy)";
                    }
                    else if (Page4.IsVisible)
                    {
                        Page4.Visibility = Visibility.Collapsed;
                        Page3.Visibility = Visibility.Visible;
                        if (LaData.AppData.CampaignID == 102 ||
                            LaData.AppData.CampaignID == 2 ||
                            LaData.AppData.CampaignID == 103 ||
                            LaData.AppData.CampaignID == 250 ||
                            LaData.AppData.CampaignID == 6 ||
                            LaData.AppData.CampaignID == 105)
                        {
                            lblMoveToLeadPermissions.Visibility = Visibility.Visible;
                            chkMoveToLeadPermissions.Visibility = Visibility.Visible;
                        }

                        lblPage.Text = "(Banking)";
                    }
                    else if (Page5.IsVisible)
                    {
                        Page5.Visibility = Visibility.Collapsed;
                        Page4.Visibility = Visibility.Visible;
                        lblMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        chkMoveToLeadPermissions.Visibility = Visibility.Collapsed;
                        lblPage.Text = "(LA + Beneficiary)";
                    }

                    if (Page3.IsVisible)
                    {
                        // Carry over value from lead details to account details.
                        if (string.IsNullOrEmpty(medDOInitials.Text))
                            medDOInitials.Text = medInitials.Text;
                        if (string.IsNullOrEmpty(medDOName.Text))
                            medDOName.Text = medName.Text;
                        if (string.IsNullOrEmpty(medDOSurname.Text))
                            medDOSurname.Text = medSurname.Text;
                        if (string.IsNullOrEmpty(medDOIDNumber.Text))
                            medDOIDNumber.Text = medIDNumber.Text;
                        if (string.IsNullOrEmpty(medDOWorkPhone.Text))
                            medDOWorkPhone.Text = medWorkPhone.Text;
                        if (string.IsNullOrEmpty(medDOHomePhone.Text))
                            medDOHomePhone.Text = medHomePhone.Text;
                        if (string.IsNullOrEmpty(medDOCellPhone.Text))
                            medDOCellPhone.Text = medCellPhone.Text;
                    }
                }

                medReference.Focus();
            }


        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region DebiCheck Workings
                if (LaData.BankDetailsData.BankID == 266 || LaData.BankDetailsData.BankID == 245 || LaData.BankDetailsData.BankID == 267 || GlobalSettings.ApplicationUser.ID == 199)
                {

                }
                else
                {
                    if (cmbStatus.Text == "Sale")
                    {
                        if (LaData.AppData.CampaignID == 7
                            || LaData.AppData.CampaignID == 9
                            || LaData.AppData.CampaignID == 10
                            || LaData.AppData.CampaignID == 294
                            || LaData.AppData.CampaignID == 295
                            || LaData.AppData.CampaignID == 24
                            || LaData.AppData.CampaignID == 25
                            || LaData.AppData.CampaignID == 11
                            || LaData.AppData.CampaignID == 12
                            || LaData.AppData.CampaignID == 13
                            || LaData.AppData.CampaignID == 14
                            || LaData.AppData.CampaignID == 85
                            || LaData.AppData.CampaignID == 86
                            || LaData.AppData.CampaignID == 87
                            || LaData.AppData.CampaignID == 281
                            || LaData.AppData.CampaignID == 324
                            || LaData.AppData.CampaignID == 325
                            || LaData.AppData.CampaignID == 326
                            || LaData.AppData.CampaignID == 327
                            || LaData.AppData.CampaignID == 264
                            || LaData.AppData.CampaignID == 4)
                        {
                            DateTime ImportDate;

                            try
                            {
                                StringBuilder strQueryImportDate = new StringBuilder();
                                strQueryImportDate.Append("SELECT TOP 1 ImportDate [Response] ");
                                strQueryImportDate.Append("FROM INImport ");
                                strQueryImportDate.Append("WHERE ID = " + LaData.AppData.ImportID);
                                strQueryImportDate.Append(" ORDER BY ID DESC");
                                DataTable dtImportDate = Methods.GetTableData(strQueryImportDate.ToString());

                                ImportDate = DateTime.Parse(dtImportDate.Rows[0]["Response"].ToString());

                            }
                            catch
                            {
                                ImportDate = DateTime.Now;
                            }




                            if (ImportDate > DateTime.Now.AddMonths(-3))
                            {
                                try
                                {
                                    StringBuilder strQueryDerbiCheckCheckSave = new StringBuilder();
                                    strQueryDerbiCheckCheckSave.Append("SELECT TOP 1 SMSBody [Response] ");
                                    strQueryDerbiCheckCheckSave.Append("FROM DebiCheckSent ");
                                    strQueryDerbiCheckCheckSave.Append("WHERE FKImportID = " + LaData.AppData.ImportID);
                                    strQueryDerbiCheckCheckSave.Append(" ORDER BY ID DESC");
                                    DataTable dt = Methods.GetTableData(strQueryDerbiCheckCheckSave.ToString());
                                    string responses = dt.Rows[0]["Response"].ToString();


                                }
                                catch
                                {
                                    INMessageBoxWindow2 messageBox = new INMessageBoxWindow2();
                                    messageBox.buttonOK.Content = "Yes";
                                    var showMessageBox = ShowMessageBox(messageBox, "Please send a Debi-Check.", "Debi-Check", ShowMessageType.Information);
                                    bool result = showMessageBox != null && (bool)showMessageBox;
                                    return;
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            if (LaData.AppData.CampaignGroupType == lkpINCampaignGroupType.Base)
                            {
                                try
                                {
                                    StringBuilder strQueryDerbiCheckCheckSave = new StringBuilder();
                                    strQueryDerbiCheckCheckSave.Append("SELECT TOP 1 SMSBody [Response] ");
                                    strQueryDerbiCheckCheckSave.Append("FROM DebiCheckSent ");
                                    strQueryDerbiCheckCheckSave.Append("WHERE FKImportID = " + LaData.AppData.ImportID);
                                    strQueryDerbiCheckCheckSave.Append(" ORDER BY ID DESC");
                                    DataTable dt = Methods.GetTableData(strQueryDerbiCheckCheckSave.ToString());
                                    string responses = dt.Rows[0]["Response"].ToString();
                                }
                                catch
                                {
                                    INMessageBoxWindow2 messageBox = new INMessageBoxWindow2();
                                    messageBox.buttonOK.Content = "Yes";
                                    var showMessageBox = ShowMessageBox(messageBox, "Please send a Debi-Check.", "Debi-Check", ShowMessageType.Information);
                                    bool result = showMessageBox != null && (bool)showMessageBox;
                                    return;
                                }
                            }
                            else if (LaData.AppData.CampaignGroupType == lkpINCampaignGroupType.Upgrade)
                            {
                                try
                                {
                                    StringBuilder strQueryDerbiCheckCheckSave = new StringBuilder();
                                    strQueryDerbiCheckCheckSave.Append("SELECT TOP 1 SMSBody [Response] ");
                                    strQueryDerbiCheckCheckSave.Append("FROM DebiCheckSent ");
                                    strQueryDerbiCheckCheckSave.Append("WHERE FKImportID = " + LaData.AppData.ImportID);
                                    strQueryDerbiCheckCheckSave.Append(" ORDER BY ID DESC");
                                    DataTable dt = Methods.GetTableData(strQueryDerbiCheckCheckSave.ToString());
                                    string responses = dt.Rows[0]["Response"].ToString();
                                }
                                catch
                                {
                                    INMessageBoxWindow2 messageBox = new INMessageBoxWindow2();
                                    messageBox.buttonOK.Content = "Yes";
                                    var showMessageBox = ShowMessageBox(messageBox, "Please send a Debi-Check.", "Debi-Check", ShowMessageType.Information);
                                    bool result = showMessageBox != null && (bool)showMessageBox;
                                    return;
                                }
                            }
                        }
                        #endregion




                        if (chkLeadPermission.Visibility == Visibility.Visible)
                        {
                            if (chkLeadPermission.IsChecked == false)
                            {
                                string strQuery;
                                strQuery = "SELECT ID FROM INPermissionLead WHERE FKImportID = " + LaData.AppData.ImportID;

                                DataTable dtPolicyPlanGroup = Methods.GetTableData(strQuery);
                                if (dtPolicyPlanGroup.Rows.Count == 0)
                                {
                                    INMessageBoxWindow2 messageBox = new INMessageBoxWindow2();
                                    messageBox.buttonOK.Content = "Yes";
                                    messageBox.buttonCancel.Content = "No";

                                    var showMessageBox = ShowMessageBox(messageBox, "Add Permission Lead?", "Permission Lead", ShowMessageType.Information);
                                    bool result = showMessageBox != null && (bool)showMessageBox;

                                    if (result == true)
                                    {
                                        PermissionLeadScreen mySuccess = new PermissionLeadScreen(LaData.AppData.ImportID, cmbLA2Title.Text, medLA2Name.Text, medLA2Surname.Text, medLA2ContactPhone.Text, medAltContactPhone.Text);
                                        ShowDialog(mySuccess, new INDialogWindow(mySuccess));

                                        return;
                                    }
                                }
                                else
                                {
                                }
                            }
                        }

                    }

                }
            }
            catch
            {

            }



            if ((lkpUserType?)((User)GlobalSettings.ApplicationUser).FKUserType == lkpUserType.ConfirmationAgent && (lkpINLeadStatus?)LaData.AppData.LeadStatus == lkpINLeadStatus.Accepted && LaData.AppData.IsConfirmed == false)
            {
                //INMessageBoxWindow2 messageWindow = new INMessageBoxWindow2();
                //ShowMessageBox(messageWindow, )
                if (LaData.SaleData.ConfCallRefID == null)
                {
                    LaData.SaleData.ConfCallRefID = LaData.SaleData.AutoPopulateConfirmationAgentFKUserID;
                }
                //else if (LaData.SaleData.ConfCallRefID != null)
                //{
                //    LaData.SaleData.BatchCallRefID = LaData.SaleData.AutoPopulateConfirmationAgentFKUserID;
                //}
            }

#if !TRAININGBUILD
            if (LaData.UserData.UserType == lkpUserType.SalesAgent)
            {
                if (!LaData.AppData.IsLeadUpgrade &&
                    LaData.AppData.LeadStatus == 1 &&
                    LaData.AppData.CampaignGroup != lkpINCampaignGroup.Defrosted &&
                    LaData.AppData.CampaignGroup != lkpINCampaignGroup.Reactivation &&
                    LaData.AppData.CampaignGroup != lkpINCampaignGroup.ReDefrost &&
                    LaData.AppData.CampaignGroup != lkpINCampaignGroup.Rejuvenation &&
                    LaData.AppData.CampaignGroup != lkpINCampaignGroup.Resurrection &&
                    LaData.AppData.CampaignGroup != lkpINCampaignGroup.DefrostR99 &&
                    LaData.AppData.CampaignGroup != lkpINCampaignGroup.Lite &&
                    LaData.AppData.CampaignGroup != lkpINCampaignGroup.SpouseLite &&
                    LaData.AppData.CampaignType != lkpINCampaignType.IGFemaleDisability &&
                    LaData.AppData.CampaignType != lkpINCampaignType.IGCancer &&
                    (LaData.BankDetailsData.lkpAccNumCheckStatus == lkpINAccNumCheckStatus.Invalid ||
                    LaData.BankDetailsData.lkpAccNumCheckStatus == lkpINAccNumCheckStatus.UnChecked))
                {
                    INMessageBoxWindow1 mbw = new INMessageBoxWindow1();
                    string message;
                    if (LaData.BankDetailsData.lkpAccNumCheckStatus == lkpINAccNumCheckStatus.Invalid)
                    {
                        message = "The Bank Details Account Number is Invalid.";
                    }
                    else
                    {
                        message = "The Bank Details Account Number has not been Verified.";
                    }
                    ShowMessageBox(mbw, message, "Lead not Saved", ShowMessageType.Error);
                    return;
                }

                LaData.AppData.AgentID = LaData.UserData.UserID;
            }
#endif

            if (IsValidData())
            {
                if ((lkpINLeadStatus?)LaData.AppData.LeadStatus == lkpINLeadStatus.DoNotContactClient)
                {
                    string message;

                    if ((lkpUserType?)((User)GlobalSettings.ApplicationUser).FKUserType == lkpUserType.SalesAgent)
                    {
                        message = "WARNING: If you save this lead with this status selected, no further changes will be possible. Are you sure you want to continue?";
                    }
                    else
                    {
                        message = "WARNING: If you save this lead with this status selected, no further changes will be possible by the sales agent to whom this lead is allocated. Are you sure you want to continue?";
                    }

                    INMessageBoxWindow2 messageWindow = new INMessageBoxWindow2();
                    messageWindow.buttonOK.Content = "Yes";
                    messageWindow.buttonCancel.Content = "Cancel";

                    bool? confirmSave = ShowMessageBox(messageWindow, message, "Confirm Lead Status", ShowMessageType.Exclamation);

                    if (confirmSave.HasValue)
                    {
                        if (confirmSave.Value)
                        {
                            Save();
                        }
                    }
                }
                else
                {
                    Save();
                }
            }

            BumpUpSelected = 0;
        }

        private void cmbDOBank_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int selectedIndex = cmbDOBank.SelectedIndex;
                long? selectedValue = (selectedIndex >= 0) ? (long?)((DataRowView)cmbDOBank.Items[selectedIndex]).Row.ItemArray[0] : null;

                DataTable dtBankBranch = Methods.GetTableData("SELECT ID, Name, Code FROM BankBranch WHERE FKBank = '" + selectedValue + "'");

                if (dtBankBranch.Rows.Count > 0)
                {
                    cmbDOBankBranchCode.Populate(dtBankBranch, "Code", IDField);
                    cmbDOBankBranchCode.SelectedIndex = 0;
                }

                DataTable dtBankAccountNoSpec = Methods.GetTableData("SELECT Pattern FROM BankAccountNoSpec WHERE FKBankID = '" + selectedValue + "'");

                if (dtBankAccountNoSpec.Rows.Count > 0)
                {
                    LaData.BankDetailsData.BankAccountNumberPattern = dtBankAccountNoSpec.Rows[0]["Pattern"] as string;
                    medDOAccountNumber.ValueConstraint.RegexPattern = LaData.BankDetailsData.BankAccountNumberPattern;
                }
                else
                {
                    LaData.BankDetailsData.BankAccountNumberPattern = "NULL";
                    medDOAccountNumber.ValueConstraint.RegexPattern = LaData.BankDetailsData.BankAccountNumberPattern;
                }


                //if (!(cmbDOBank.SelectedValue == null) && !(cmbDOAccountType.SelectedValue == null))
                //{
                //    if (int.Parse(cmbDOBank.SelectedValue.ToString()) == 240 && int.Parse(cmbDOAccountType.SelectedValue.ToString()) == 3)
                //    {
                //        CheckIfMoneyBuilderAccount();
                //    }
                //}

            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


        //private void cmbDOBankBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    cmbDOBankBranchCode.SelectedIndex = cmbDOBankBranch.SelectedIndex;
        //}

        //private void cmbDOBankBranchCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    cmbDOBankBranch.SelectedIndex = cmbDOBankBranchCode.SelectedIndex;
        //}

        private void cmbBeneficiary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!LaData.AppData.IsLeadUpgrade)
            {
                var oldBeneficiary = Page4.FindName("Beneficiary" + cmbBeneficiary.SelectionBoxItem);
                if (oldBeneficiary != null)
                {
                    ((Border)oldBeneficiary).Visibility = Visibility.Collapsed;
                }

                var newBeneficiary = Page4.FindName("Beneficiary" + (cmbBeneficiary.SelectedIndex + 1));
                if (newBeneficiary != null)
                {
                    ((Border)newBeneficiary).Visibility = Visibility.Visible;
                }
            }
        }

        private void chkChild_Click(object sender, RoutedEventArgs e)
        {
            if (LaData.AppData.IsLeadUpgrade)
            {
                if (cmbUpgradeCover.SelectedValue == null)
                {
                    LaData.PolicyData.IsChildUpgradeChecked = false;
                }

                CalculateCostUpgrade(true);
            }
            else
            {
                if (LaData.PolicyData.LA1Cover == null)
                {
                    LaData.PolicyData.IsChildChecked = false;
                }

                CalculateCost(true);
            }
        }

        private void cmbPolicyPlanGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                if (LaData.PolicyData.PlanGroupID != null)
                {
                    string strQuery = "SELECT DISTINCT ID, PlanCode [Description] FROM INPlan ";
                    strQuery += "WHERE FKINPlanGroupID = '" + LaData.PolicyData.PlanGroupID + "'";

                    DataTable dtPolicyPlan = Methods.GetTableData(strQuery);
                    cmbPolicyPlan.Populate(dtPolicyPlan, DescriptionField, IDField);

                    LaData.PolicyData.PlanID = ((DataView)cmbPolicyPlan.ItemsSource).Table.AsEnumerable().Where(r => r["Description"] as string == "Option " + LaData.PolicyData.PlatinumPlan).Select(r => r["ID"] as long?).FirstOrDefault();
                }
                else
                {
                    LaData.PolicyData.PlanID = null;
                    cmbPolicyPlan.ItemsSource = null;
                    LaData.PolicyData.OptionID = null;
                    cmbOptionCode.ItemsSource = null;
                    LaData.PolicyData.LA1Cover = null;
                    cmbLA1Cover.ItemsSource = null;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmbPolicyPlan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (LaData.PolicyData.PlanID != null)
                {
                    decimal? selectedLA1Cover = LaData.PolicyData.LA1Cover;
                    long? selectedUpgradeCover = LaData.PolicyData.OptionID;

                    DataTable dtOptionCode = Methods.GetTableData("SELECT DISTINCT ID, OptionCode FROM INOption WHERE FKINPlanID = '" + LaData.PolicyData.PlanID + "' AND IsActive = '1'");
                    cmbOptionCode.Populate(dtOptionCode, "OptionCode", IDField);

                    //LaData.PolicyData.OptionID = GetOptionID();

                    SqlParameter[] parameters = new SqlParameter[5];//5
                    parameters[0] = new SqlParameter("@CampaignID", LaData.AppData.CampaignID);
                    parameters[1] = new SqlParameter("@PlanID", LaData.PolicyData.PlanID);
                    parameters[2] = new SqlParameter("@UserID", LaData.UserData.UserID);
                    parameters[3] = new SqlParameter("@OptionID", LaData.PolicyData.OptionID);

                    if (LaData.PolicyData.OptionID == null)
                    {
                        parameters[3].Value = DBNull.Value;
                    }

                    if ((LaData.AppData.IsLeadUpgrade ||
                        LaData.AppData.CampaignCode == "PLFDB" ||
                        LaData.AppData.CampaignCode == "PLFDMIN" ||
                        LaData.AppData.CampaignCode == "PLFDBPE" ||
                        LaData.AppData.CampaignCode == "PLULFDB" ||
                        LaData.AppData.CampaignCode == "PLULFDMIN" ||
                        LaData.AppData.CampaignCode == "PLULFDBPE") &&
                        LaData.AppData.LeadStatus == null &&
                        !Convert.ToBoolean(chkShowAllOptions.IsChecked))
                    {
                        parameters[4] = new SqlParameter("@HigherOptionMode", 1);
                        chkShowAllOptions.IsChecked = false;
                    }
                    else
                    {
                        parameters[4] = new SqlParameter("@HigherOptionMode", -1);
                        chkShowAllOptions.Tag = 1;
                        chkShowAllOptions.IsChecked = true;
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

                    if (LaData.AppData.IsLeadUpgrade)
                    {
                        cmbUpgradeCover.Populate(dtCover, "Description", "Value");
                        LaData.PolicyData.OptionID = selectedUpgradeCover;
                    }
                    else
                    {
                        cmbLA1Cover.Populate(dtCover, "Description", "Value");
                        LaData.PolicyData.LA1Cover = selectedLA1Cover;
                    }

                    //LaData.PolicyData.OptionID = null;
                    //LaData.PolicyData.OptionID = selectedOptionID;
                    //BindingExpression binding = cmbLA1Cover.GetBindingExpression(EmbriantComboBox.SelectedValueProperty);
                    //binding.UpdateSource();

                    //if (LaData.AppData.IsLeadUpgrade)
                    //{
                    //    if (LaData.UserData.UserType == lkpUserType.ConfirmationAgent || LaData.UserData.UserType == lkpUserType.Manager)
                    //    {
                    //        //DataTable dtCover = Methods.GetTableData("SELECT ID, Display FROM INOption WHERE FKINPlanID = '" + LaData.PolicyData.PlanID + "' AND IsActive = '1' ORDER BY ID ASC");

                    //        foreach (DataRow row in dtCover.Rows)
                    //        {
                    //            string str = row["Display"] as string;
                    //            row["Display"] = DisplayCurrencyFormat(str);
                    //        }

                    //        cmbUpgradeCover.Populate(dtCover, "Display", IDField);
                    //    }
                    //    else
                    //    {
                    //        //option rr shoul not appear
                    //        //DataTable dtCover = Methods.GetTableData("SELECT ID, Display FROM INOption WHERE FKINPlanID = '" + LaData.PolicyData.PlanID + "' AND IsActive = '1' and OptionCode NOT like '%RRR%' ORDER BY ID ASC ");

                    //        foreach (DataRow row in dtCover.Rows)
                    //        {
                    //            string str = row["Display"] as string;
                    //            row["Display"] = DisplayCurrencyFormat(str);
                    //        }

                    //        cmbUpgradeCover.Populate(dtCover, "Display", IDField);
                    //    }
                    //}
                    //else
                    //{
                    //    //DataTable dtLA1Cover = Methods.GetTableData("SELECT DISTINCT LA1Cover FROM INOption WHERE FKINPlanID = '" + LaData.PolicyData.PlanID + "' AND IsActive = '1' ORDER BY LA1Cover DESC");

                    //    dtCover.Columns.Add("Description", typeof(string));
                    //    foreach (DataRow row in dtCover.Rows)
                    //    {
                    //        if (row != null && row["LA1Cover"] != null && row["LA1Cover"] != DBNull.Value)
                    //        {
                    //            string str = row["LA1Cover"].ToString();
                    //            row["Description"] = DisplayCurrencyFormat(str);
                    //        }
                    //    }

                    //    cmbLA1Cover.Populate(dtCover, "Description", "LA1Cover");
                    //}
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmbCover_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LaData.AppData.IsLeadLoaded)
            {
                if (!LaData.AppData.IsLeadUpgrade)
                {
                    LaData.PolicyData.OptionID = GetOptionID();

                    if (LaData.PolicyData.LA1Cover == null)
                    {
                        LaData.PolicyData.IsLA2Checked = false;
                        LaData.PolicyData.IsChildChecked = false;
                    }
                }

                if (LaData.AppData.IsLeadUpgrade) CalculateCostUpgrade(true); else CalculateCost(true);
            }
        }

        private void ButtonUpgradeCalculations(int? optionID)
        {

            LaData.PolicyData.OptionID = optionID;
            if (LaData.PolicyData.LA1Cover == null)
            {
                LaData.PolicyData.IsLA2Checked = false;
                LaData.PolicyData.IsChildChecked = false;
            }
            if (LaData.AppData.IsLeadUpgrade) CalculateCostUpgrade(true); else CalculateCost(true);


        }

        private void cmbPaymentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                LaData.BankDetailsData.PaymentType = LaData.BankDetailsData.PaymentTypeID != null ? (lkpPaymentType?)LaData.BankDetailsData.PaymentTypeID : null;

                if (LaData.BankDetailsData.PaymentType != null)
                {
                    switch (LaData.BankDetailsData.PaymentType)
                    {
                        case lkpPaymentType.DebitOrder:
                            DOAccountHolderDetail.Visibility = Visibility.Visible;
                            DOAccountDetail.Visibility = Visibility.Visible;
                            BankingDetails.Visibility = Visibility.Visible;
                            break;
                    }
                }
                else
                {
                    DOAccountHolderDetail.Visibility = Visibility.Collapsed;
                    DOAccountDetail.Visibility = Visibility.Collapsed;
                    BankingDetails.Visibility = Visibility.Collapsed;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        #region Removed postal code checking - as requested by Brigette, 2015-03-24

        //private void tb_CommandExecuted(object sender, RoutedEventArgs e)
        //{
        //    TextBox tb = sender as TextBox;

        //    var executedRoutedEventArgs = e as ExecutedRoutedEventArgs;
        //    if (e != null && (e.Handled && executedRoutedEventArgs != null && executedRoutedEventArgs.Command == ApplicationCommands.Paste))
        //    {
        //        if (tb != null && (tb.Tag as string) == "ecbSuburb")
        //        {
        //            string strPaste = Clipboard.GetText();

        //            tb.Text = "";

        //            SqlParameter[] parameters = new SqlParameter[1];
        //            parameters[0] = new SqlParameter("@Keyword", strPaste.Substring(0, 1));

        //            DataSet ds = Methods.ExecuteStoredProcedure("Blush.dbo.spGetPostalCodeList", parameters);
        //            DataTable dt = ds.Tables[0];

        //            ecbSuburb.Populate(dt, "Suburb", IDField);

        //            TextCompositionEventArgs args =
        //                new TextCompositionEventArgs(InputManager.Current.PrimaryKeyboardDevice,
        //                    new TextComposition(InputManager.Current, tb, strPaste));
        //            args.RoutedEvent = TextCompositionManager.PreviewTextInputEvent;

        //            InputManager.Current.ProcessInput(args);
        //        }
        //    }
        //}

        //private void ecb_Loaded(object sender, RoutedEventArgs e)
        //{
        //    EmbriantComboBox ecb = sender as EmbriantComboBox;
        //    TextBox tb = FindChild<TextBox>(ecb, "PART_EditableTextBox");

        //    if (ecb != null) tb.Tag = ecb.Name;
        //    tb.AddHandler(CommandManager.ExecutedEvent, new RoutedEventHandler(tb_CommandExecuted), true);
        //}

        //private void ecb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    EmbriantComboBox ecb = sender as EmbriantComboBox;

        //    if (ecb != null)
        //    {
        //        if (e != null)
        //        {
        //            if (ecb.Name == "ecbSuburb" && ((TextBox)e.OriginalSource).Text == ((TextBox)e.OriginalSource).SelectedText)
        //            {
        //                SqlParameter[] parameters = new SqlParameter[1];
        //                parameters[0] = new SqlParameter("@Keyword", e.Text);

        //                DataSet ds = Methods.ExecuteStoredProcedure("Blush.dbo.spGetPostalCodeList", parameters);
        //                DataTable dt = ds.Tables[0];

        //                ecb.Populate(dt, "Suburb", IDField);
        //            }
        //        }

        //    }
        //}

        //private void ecb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    EmbriantComboBox ecb = sender as EmbriantComboBox;

        //    if (ecb != null && _laData.IsLeadLoaded)
        //    {
        //        if (ecb.Name == "ecbSuburb")
        //        {
        //            ecbTown.Text = string.Empty;
        //            medPostalCode.Text = string.Empty;

        //            if (ecb.SelectedItem != null)
        //            {
        //                DataRowView drv = (DataRowView)ecb.SelectedItem;

        //                ecbTown.Text = drv["Town"].ToString();
        //                medPostalCode.Text = drv["StreetCode"].ToString();
        //            }
        //        }
        //        else if (ecb.Name == "ecbTown")
        //        {
        //            if (ecb.SelectedItem != null)
        //            {
        //                if (ecbSuburb.SelectedItem == null)
        //                {
        //                    string sqlQuery;
        //                    DataTable dt;
        //                    DataRowView drv = (DataRowView)ecb.SelectedItem;
        //                    string strTown = drv["Description"].ToString().Replace("'", "''");

        //                    sqlQuery = "Select (CASE StreetCode WHEN NULL THEN BoxCode ELSE StreetCode END) [StreetCode] From Blush.dbo.PostalCode Where Suburb = '" + strTown + "' and City = '" + strTown + "'";
        //                    dt = Methods.GetTableData(sqlQuery);

        //                    if (dt.Rows.Count > 0)
        //                    {
        //                        medPostalCode.Text = dt.Rows[0]["StreetCode"].ToString();
        //                        return;
        //                    }

        //                    sqlQuery = "Select (CASE StreetCode WHEN NULL THEN BoxCode ELSE StreetCode END) [StreetCode] From Blush.dbo.PostalCode Where Suburb Is Null and City = '" + strTown + "'";
        //                    dt = Methods.GetTableData(sqlQuery);

        //                    if (dt.Rows.Count > 0)
        //                    {
        //                        medPostalCode.Text = dt.Rows[0]["StreetCode"].ToString();
        //                        return;
        //                    }

        //                    sqlQuery = "Select (CASE StreetCode WHEN NULL THEN BoxCode ELSE StreetCode END) [StreetCode] From Blush.dbo.PostalCode Where City = '" + strTown + "'";
        //                    dt = Methods.GetTableData(sqlQuery);

        //                    if (dt.Rows.Count > 0)
        //                    {
        //                        medPostalCode.Text = dt.Rows[0]["StreetCode"].ToString();
        //                    }
        //                }
        //                else
        //                {
        //                    //string sqlQuery;
        //                    //DataTable dt;
        //                    DataRowView drvSuburb = (DataRowView)ecbSuburb.SelectedItem;
        //                    DataRowView drvTown = (DataRowView)ecbTown.SelectedItem;
        //                    string strSuburb = drvSuburb["Suburb"].ToString().Replace("'", "''");
        //                    string strTown = drvTown["Description"].ToString().Replace("'", "''");

        //                    if (strSuburb == strTown)
        //                    {
        //                        //ecbSuburb.SelectedItem = null;
        //                        //ecbSuburb.Text = null;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                medPostalCode.Text = string.Empty;
        //            }
        //        }
        //    }
        //}

        //private void ecb_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    EmbriantComboBox cmbControl = (EmbriantComboBox)sender;

        //    if (e.Key == Key.Back)
        //    {
        //        if (cmbControl.SelectedIndex == -1)
        //        {
        //            cmbControl.Text = string.Empty;
        //        }
        //        else
        //        {
        //            cmbControl.SelectedIndex = -1;
        //        }

        //        e.Handled = true;
        //    }
        //}

        #endregion Removed postal code checking - as requested by Brigette, 2015-03-24

        private void dteDateOfBirth_Loaded(object sender, RoutedEventArgs e)
        {
            //dteDateOfBirth.Value = IDNumberToDOB(medIDNumber.Text);




        }

        private void dteLA2DateOfBirth_Loaded(object sender, RoutedEventArgs e)
        {
            if (!LaData.AppData.IsLeadUpgrade)
            {
                dteLA2DateOfBirth.Value = IDNumberToDOB(medLA2IDNumber.Text);
            }
        }

        //private void dteBeneficiaryDateOfBirth_Loaded(object sender, RoutedEventArgs e)
        //{
        //    switch (cmbBeneficiary.SelectionBoxItem as string)
        //    {
        //        case "1":
        //            dteBeneficiaryDateOfBirth1.Value = IDNumberToDOB(medBeneficiaryIDNumber1.Text);
        //            break;

        //        case "2":
        //            dteBeneficiaryDateOfBirth2.Value = IDNumberToDOB(medBeneficiaryIDNumber2.Text);
        //            break;

        //        case "3":
        //            dteBeneficiaryDateOfBirth3.Value = IDNumberToDOB(medBeneficiaryIDNumber3.Text);
        //            break;

        //        case "4":
        //            dteBeneficiaryDateOfBirth4.Value = IDNumberToDOB(medBeneficiaryIDNumber4.Text);
        //            break;

        //        case "5":
        //            dteBeneficiaryDateOfBirth5.Value = IDNumberToDOB(medBeneficiaryIDNumber5.Text);
        //            break;

        //        case "6":
        //            dteBeneficiaryDateOfBirth6.Value = IDNumberToDOB(medBeneficiaryIDNumber6.Text);
        //            break;
        //    }
        //}

        private void cmbDOSigningPower_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbDOSigningPower.SelectedItem != null && ((DataRowView)cmbDOSigningPower.SelectedItem).Row.ItemArray[1].ToString() == "Policy Owner")
            //{
            //    // Pull through to account details.
            //    if (medDOName.Text != null) medDOAccountHolder.Text = GetInitials(medDOName.Text, "") + " " + medDOSurname.Text;
            //}
            ////else
            ////{
            ////    medDOAccountHolder.Text = string.Empty;
            ////}
            ///
            if (cmbDOSigningPower.SelectedItem != null)
            {
                SigningPower();
            }

        }

        private void cmbTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            // Update the title field in banking details
            //if (this.cmbTitle.SelectedItem != null)
            //    this.cmbDOTitle.SelectedIndex = this.cmbTitle.SelectedIndex;
        }

        private void MedDOAccountNumber_OnEditModeValidationError(object sender, EditModeValidationErrorEventArgs e)
        {
            if (Convert.ToBoolean(medDOAccountNumber.IsEnabled))
            {
                if (Keyboard.IsKeyDown(Key.Tab))
                {
                    //UIElement nextControl = (UIElement)PredictFocus(FocusNavigationDirection.Up);

                    //medDOAccountNumber.IsEnabled = false;
                    Keyboard.ClearFocus();
                    cmbDOAccountType.Focus();
                    e.Handled = true;
                }
                else if (!medDOAccountNumber.IsMouseCaptureWithin)
                {
                    //medDOAccountNumber.IsEnabled = false;
                    Keyboard.ClearFocus();
                    UIElement parentControl = Methods.FindParent<XamMaskedEditor>((DependencyObject)(Mouse.DirectlyOver)) ?? (UIElement)Methods.FindParent<EmbriantComboBox>((DependencyObject)(Mouse.DirectlyOver));
                    if (parentControl != null) parentControl.Focus();
                    e.Handled = true;
                }
            }
            else
            {
                medDOAccountNumber.IsEnabled = true;
                e.Handled = true;
            }
        }

        private void cmbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LaData.AppData.IsLeadLoaded) //LaData.AppData.ImportID != null && e.AddedItems.Count > 0
            {
                //lkpINLeadStatus? leadStatus = (lkpINLeadStatus?)(((DataRowView)e.AddedItems[0]).Row["ID"] as long?);
                //MessageBox.Show(LaData.AppData.LeadStatus.ToString());
                switch (LaData.AppData.lkpLeadStatus)
                {
                    #region Sales

                    case lkpINLeadStatus.Accepted:
                        //auto populate date and time  
                        DateTime saleDate = DateTime.Now;
                        dteSaleDate.Value = saleDate;

                        lblDateOfSale.Text = "Date Of Sale";
                        lblSaleDate.Text = "Sale Date / Time";
                        string time = saleDate.TimeOfDay.Hours + ":" + saleDate.TimeOfDay.Minutes;
                        dteSaleTime.Value = time;

                        chkClosureChecked();
                        break;

                    #endregion Sales

                    #region Cancellations

                    case lkpINLeadStatus.Cancelled:
                        //see if this can be selected
                        if (LaData.UserData.UserType == lkpUserType.SalesAgent)
                        {
                            if (LaData.AppData.LoadedLeadStatus == (long?)lkpINLeadStatus.Accepted)
                            {
                                if (LaData.AppData.LoadedDateOfSale < DateTime.Now.AddHours(-24))
                                {
                                    break;
                                }
                            }
                        }
                        SelectCancellationReasonScreen selectCancelllationReasonScreen = new SelectCancellationReasonScreen(this);
                        selectCancelllationReasonScreen.SelectedCancellationReasonID = LaData.AppData.CancellationReasonID;
                        ShowDialog(selectCancelllationReasonScreen, new INDialogWindow(selectCancelllationReasonScreen));
                        LaData.AppData.CancellationReasonID = selectCancelllationReasonScreen.SelectedCancellationReasonID;

                        if (LaData.AppData.CancellationReasonID == null)
                        {
                            cmbStatus.SelectedIndex = -1;
                        }

                        chkUDMBumpUp.IsChecked = false;
                        chkReducedPremium.IsChecked = false;

                        Methods.FindChild<TextBox>(medReference, "PART_InputTextBox").Focus();

                        cmbStatus_ToolTip(null);

                        break;

                    #endregion Cancellations

                    #region Carried Forwards

                    case lkpINLeadStatus.CarriedForward:
                        //see if this can be selected
                        if (LaData.UserData.UserType == lkpUserType.SalesAgent)
                        {
                            if (LaData.AppData.LoadedLeadStatus == (long?)lkpINLeadStatus.Accepted)
                            {
                                if (LaData.AppData.LoadedDateOfSale < DateTime.Now.AddHours(-24))
                                {
                                    break;
                                }
                            }
                        }
                        SelectCarriedForwardReasonScreen selectCarriedForwardReasonScreen = new SelectCarriedForwardReasonScreen(this);
                        selectCarriedForwardReasonScreen.SelectedCarriedForwardReasonID = LaData.AppData.CarriedForwardReasonID;
                        ShowDialog(selectCarriedForwardReasonScreen, new INDialogWindow(selectCarriedForwardReasonScreen));
                        LaData.AppData.CarriedForwardReasonID = selectCarriedForwardReasonScreen.SelectedCarriedForwardReasonID;

                        if (LaData.AppData.CarriedForwardReasonID == null)
                        {
                            cmbStatus.SelectedIndex = -1;
                        }

                        //chkUDMBumpUp.IsChecked = false;
                        //chkReducedPremium.IsChecked = false;

                        Methods.FindChild<TextBox>(medReference, "PART_InputTextBox").Focus();

                        cmbStatus_ToolTip(null);

                        break;

                    #endregion Carried Forwards

                    #region Declines

                    case lkpINLeadStatus.Declined:
                        //see if this can be selected
                        if (LaData.UserData.UserType == lkpUserType.SalesAgent)
                        {
                            if (LaData.AppData.LoadedLeadStatus == (long?)lkpINLeadStatus.Accepted)
                            {
                                if (LaData.AppData.LoadedDateOfSale < DateTime.Now.AddHours(-24))
                                {
                                    break;
                                }
                            }
                        }

                        LaData.AppData.DeclineReasonID = null;
                        SelectDeclineReasonScreen selectDeclineReasonScreen = new SelectDeclineReasonScreen(this);
                        selectDeclineReasonScreen.SelectedDeclineReasonID = LaData.AppData.DeclineReasonID;
                        ShowDialog(selectDeclineReasonScreen, new INDialogWindow(selectDeclineReasonScreen));

                        LaData.AppData.DeclineReasonID = selectDeclineReasonScreen.SelectedDeclineReasonID;

                        if (LaData.AppData.DeclineReasonID == null)
                        {
                            cmbStatus.SelectedIndex = -1;
                        }

                        Methods.FindChild<TextBox>(medReference, "PART_InputTextBox").Focus();
                        break;

                    #endregion Declines

                    #region Diaries

                    case lkpINLeadStatus.Diary:
                        //see if this can be selected
                        if (LaData.UserData.UserType == lkpUserType.SalesAgent)
                        {
                            if (LaData.AppData.LoadedLeadStatus == (long?)lkpINLeadStatus.Accepted)
                            {
                                if (LaData.AppData.LoadedDateOfSale < DateTime.Now.AddHours(-24))
                                {
                                    break;
                                }
                            }
                        }

                        LaData.AppData.DiaryReasonID = null;
                        SelectDiaryReasonScreen selectDiaryReasonScreen = new SelectDiaryReasonScreen(_ssGlobalData);
                        selectDiaryReasonScreen.SelectedDiaryReasonID = LaData.AppData.DiaryReasonID;
                        ShowDialog(selectDiaryReasonScreen, new INDialogWindow(selectDiaryReasonScreen));

                        LaData.AppData.DiaryReasonID = selectDiaryReasonScreen.SelectedDiaryReasonID;

                        if (LaData.AppData.DiaryReasonID == null)
                        {
                            //cmbStatus.ResetText();
                            //cmbStatus.SelectedIndex = -1;
                            //LaData.AppData.LeadStatus = -1;
                            LaData.AppData.LeadStatus = null;
                        }

                        Methods.FindChild<TextBox>(medReference, "PART_InputTextBox").Focus();

                        LaData.AppData.DiaryStatusHandled = true;
                        break;

                    #endregion Diaries

                    #region Diaries ( > 7 Weeks)

                    case lkpINLeadStatus.DiaryGT7Weeks:
                        //see if this can be selected
                        if (LaData.UserData.UserType == lkpUserType.SalesAgent)
                        {
                            if (LaData.AppData.LoadedLeadStatus == (long?)lkpINLeadStatus.Accepted)
                            {
                                if (LaData.AppData.LoadedDateOfSale < DateTime.Now.AddHours(-24))
                                {
                                    break;
                                }
                            }
                        }

                        LaData.AppData.FutureContactDate = null;

                        SelectDiaryMonthScreen selectDiaryMonthScreen = new SelectDiaryMonthScreen();
                        selectDiaryMonthScreen.SelectedDate = LaData.AppData.FutureContactDate;
                        ShowDialog(selectDiaryMonthScreen, new INDialogWindow(selectDiaryMonthScreen));

                        cmbStatus.Focus();
                        LaData.AppData.FutureContactDate = selectDiaryMonthScreen.SelectedDate;

                        if (LaData.AppData.FutureContactDate == null)
                        {
                            cmbStatus.SelectedIndex = -1;
                        }
                        break;

                    #endregion Diaries ( > 7 Weeks)

                    #region Call Monitoring Cancellation

                    case lkpINLeadStatus.CallMonitoringCancellation:
                        //see if this can be selected
                        if (LaData.UserData.UserType == lkpUserType.SalesAgent)
                        {
                            if (LaData.AppData.LoadedLeadStatus == (long?)lkpINLeadStatus.Accepted)
                            {
                                if (LaData.AppData.LoadedDateOfSale < DateTime.Now.AddHours(-24))
                                {
                                    break;
                                }
                            }
                        }
                        SelectCallMonitoringCancellationReasonScreen selectCallMonitoringCancellationReasonScreen = new SelectCallMonitoringCancellationReasonScreen(this);
                        selectCallMonitoringCancellationReasonScreen.CallMonitoringCancellationReasonID = LaData.AppData.FKINCallMonitoringCancellationReasonID;
                        ShowDialog(selectCallMonitoringCancellationReasonScreen, new INDialogWindow(selectCallMonitoringCancellationReasonScreen));
                        LaData.AppData.FKINCallMonitoringCancellationReasonID = selectCallMonitoringCancellationReasonScreen.CallMonitoringCancellationReasonID;

                        if (LaData.AppData.FKINCallMonitoringCancellationReasonID == null)
                        {
                            cmbStatus.SelectedIndex = -1;
                        }

                        //chkUDMBumpUp.IsChecked = false;
                        //chkReducedPremium.IsChecked = false;

                        Methods.FindChild<TextBox>(medReference, "PART_InputTextBox").Focus();

                        cmbStatus_ToolTip(null);

                        break;

                    #endregion Call Monitoring Cancellation

                    #region Call Monitoring Carried Forward

                    case lkpINLeadStatus.CallMonitoringCarriedForward:
                        //see if this can be selected
                        if (LaData.UserData.UserType == lkpUserType.SalesAgent)
                        {
                            if (LaData.AppData.LoadedLeadStatus == (long?)lkpINLeadStatus.Accepted)
                            {
                                if (LaData.AppData.LoadedDateOfSale < DateTime.Now.AddHours(-24))
                                {
                                    break;
                                }
                            }
                        }
                        SelectCallMonitoringCarriedForwardReasonScreen selectCallMonitoringCarriedForwardReasonScreen = new SelectCallMonitoringCarriedForwardReasonScreen(this);
                        selectCallMonitoringCarriedForwardReasonScreen.SelectCallMonitoringCarriedForwardReasonID = LaData.AppData.FKINCallMonitoringCarriedForwardReasonID;
                        ShowDialog(selectCallMonitoringCarriedForwardReasonScreen, new INDialogWindow(selectCallMonitoringCarriedForwardReasonScreen));
                        LaData.AppData.FKINCallMonitoringCarriedForwardReasonID = selectCallMonitoringCarriedForwardReasonScreen.SelectCallMonitoringCarriedForwardReasonID;

                        if (LaData.AppData.FKINCallMonitoringCarriedForwardReasonID == null)
                        {
                            cmbStatus.SelectedIndex = -1;
                        }

                        //chkUDMBumpUp.IsChecked = false;
                        //chkReducedPremium.IsChecked = false;

                        Methods.FindChild<TextBox>(medReference, "PART_InputTextBox").Focus();

                        cmbStatus_ToolTip(null);

                        break;

                    #endregion Call Monitoring Carried Forward

                    default:
                        LaData.PolicyData.CommenceDate = null;
                        break;
                }

                cmbStatus_ToolTip(null);
                LaData.AppData.DiaryStatusHandled = false;
            }
        }

        private void cmbStatus_DropDownClosed(object sender, EventArgs e)
        {
            if (LaData.AppData.ImportID != null && cmbStatus.SelectedValue != null && !LaData.AppData.DiaryStatusHandled)
            {
                if ((lkpINLeadStatus)((long?)cmbStatus.SelectedValue) == lkpINLeadStatus.Diary)
                {
                    //foreach (Window window in Application.Current.Windows)
                    //{
                    //    if (window.Name == "Diary")
                    //    {
                    //        window.Close();
                    //    }
                    //}

                    //{
                    //    INUserScheduleCollection schedules = INUserScheduleMapper.Search(2, LaData.UserData.UserID, LaData.AppData.ImportID, null);

                    //    foreach (INUserSchedule schedule in schedules)
                    //    {
                    //        schedule.Delete(_validationResult);
                    //    }

                    //    ScheduleScreen scheduleScreen = new ScheduleScreen();
                    //    scheduleScreen._ssData.ImportID = LaData.AppData.ImportID;
                    //    scheduleScreen._ssData.RefNumber = LaData.AppData.RefNo;
                    //    scheduleScreen.Show();
                    //}

                    //LaData.AppData.DiaryReasonID = null;
                    //SelectDiaryReasonScreen selectDiaryReasonScreen = new SelectDiaryReasonScreen(this);
                    //selectDiaryReasonScreen.SelectedDiaryReasonID = LaData.AppData.DiaryReasonID;
                    //ShowDialog(selectDiaryReasonScreen, new INDialogWindow(selectDiaryReasonScreen));

                    //LaData.AppData.DiaryReasonID = selectDiaryReasonScreen.SelectedDiaryReasonID;

                    //if (LaData.AppData.DiaryReasonID == null)
                    //{
                    //    cmbStatus.SelectedIndex = -1;
                    //}

                    //Methods.FindChild<TextBox>(medReference, "PART_InputTextBox").Focus();



                    //LaData.AppData.DiaryStatusHandled = true;
                }
            }
        }

        #region Closure Events

        private void chkClosure_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            chkClosureChecked();
        }

        private void chkClosureChecked()
        {
            if (!LaData.AppData.IsLeadUpgrade)
            {
                if (Page1.IsVisible)
                {
                    Page1.Visibility = Visibility.Collapsed;
                    ClosurePage.Tag = Page1;
                }
                else if (Page2.IsVisible)
                {
                    Page2.Visibility = Visibility.Collapsed;
                    ClosurePage.Tag = Page2;
                }
                else if (Page3.IsVisible)
                {
                    Page3.Visibility = Visibility.Collapsed;
                    ClosurePage.Tag = Page3;
                }
                else if (Page4.IsVisible)
                {
                    Page4.Visibility = Visibility.Collapsed;
                    ClosurePage.Tag = Page4;
                }
                else if (Page5.IsVisible)
                {
                    Page5.Visibility = Visibility.Collapsed;
                    ClosurePage.Tag = Page5;
                }
            }
            else
            {
                if (UpgradePage.IsVisible)
                {
                    UpgradePage.Visibility = Visibility.Collapsed;
                    ClosurePage.Tag = UpgradePage;
                }
            }

            lblPage.Tag = lblPage.Text;
            lblPage.Text = "(Closure)";
            ClosurePage.Visibility = Visibility.Visible;

            //if (dvClosure.PageCount == 0)
            //{
            //    LoadClosureDocument();
            //}  
            //LaData.ClosureData.ClosureLanguageID = null;
            LoadClosureDocument();
            dvClosure.Focus();
        }

        private void chkClosure_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)e.NewValue)
            {
                chkClosureAccept.IsChecked = false;
                chkClosureAccept.IsEnabled = false;
                ClearClosureDocument();

                if (ClosurePage.Visibility == Visibility.Visible)
                {
                    btnNext_Click(null, null);
                }
            }
        }

        private void dvClosure_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight)
            {
                if (chkClosureAccept != null)
                {
                    chkClosureAccept.IsEnabled = true;
                }
            }
        }

        private void dvClosure_LayoutUpdated(object sender, EventArgs e)
        {
            //ClosureControl should be Hidden not Collapsed
            var sv = Methods.FindChild<ScrollViewer>(dvClosure, null);

            if (sv != null)
            {
                sv.ScrollChanged += dvClosure_OnScrollChanged;
            }
        }

        private void chkClosureAccept_Unchecked(object sender, RoutedEventArgs e)
        {
            chkClosureAccept.IsEnabled = false;
        }

        private void chkClosureAfrikaans_Checked(object sender, RoutedEventArgs e)
        {
            if (LaData.AppData.IsLeadLoaded)
            {
                LaData.ClosureData.ClosureID = null;
                LaData.ClosureData.ClosureLanguageID = (long)lkpLanguage.Afrikaans;
                chkClosureAccept.IsChecked = false;
                LoadClosureDocument();
            }
        }

        private void chkClosureAfrikaans_Unchecked(object sender, RoutedEventArgs e)
        {
            if (LaData.AppData.IsLeadLoaded)
            {
                LaData.ClosureData.ClosureID = null;
                LaData.ClosureData.ClosureLanguageID = (long)lkpLanguage.English;
                chkClosureAccept.IsChecked = false;
                LoadClosureDocument();
            }
        }

        #endregion

        private void dteDateOfBirth_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Tab) && !dteDateOfBirth.IsInEditMode)
            {
                if (medIDNumber.IsVisible)
                {
                    medPassportNumber.Tag = "Focus";
                }
                if (medPassportNumber.IsVisible)
                {
                    medIDNumber.Tag = "Show";
                }
            }
        }

        private void lblID_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FocusIDNumberField();
        }

        private void lblPassport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FocusPassportNumberField();
        }

        private void medDOInitials_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            if (LaData.AppData.IsLeadLoaded)
            {
                if (medDOInitials.Text != null)
                {
                    medDOInitials.Text = medDOInitials.Text.ToUpper();
                    if (string.IsNullOrWhiteSpace(medDOInitials.Text))
                    {
                        medDOAccountHolder.Text = medDOSurname.Text;
                    }
                    else
                    {
                        medDOAccountHolder.Text = medDOInitials.Text + " " + medDOSurname.Text;
                    }
                }
            }
        }

        private void medDOSurname_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            if (LaData.AppData.IsLeadLoaded)
            {
                medDOAccountHolder.Text = medDOInitials.Text + " " + medDOSurname.Text;
            }
        }

        private void cmbLifeAssured_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!LaData.AppData.IsLeadUpgrade)
            {
                var oldLifeAssured = Page4.FindName("LA" + cmbLifeAssured.SelectionBoxItem);
                if (oldLifeAssured != null)
                {
                    ((Border)oldLifeAssured).Visibility = Visibility.Collapsed;
                }

                var newLifeAssured = Page4.FindName("LA" + (cmbLifeAssured.SelectedIndex + 1));
                if (newLifeAssured != null)
                {
                    ((Border)newLifeAssured).Visibility = Visibility.Visible;
                }
            }
        }

        private void chkLA2_Checked(object sender, RoutedEventArgs e)
        {
            if (LaData.PolicyData.LA1Cover == null)
            {
                LaData.PolicyData.IsLA2Checked = false;
                e.Handled = true;
                return;
            }

            EmbriantComboBox cmb = Methods.FindChild<EmbriantComboBox>(PopupChkLA2.Child, "");
            if (cmb != null) cmb.SelectedIndex = -1;
            LaData.PolicyData.OptionID = GetOptionID();
            CalculateCost(true);
        }

        private void chkLA2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (LaData.PolicyData.LA1Cover == null)
            {
                LaData.PolicyData.IsLA2Checked = false;
                e.Handled = true;
                return;
            }

            {
                xamCELA2Cost.Value = 0.00m;
                xamCELA2Cover.Value = 0.00m;
            }

            EmbriantComboBox cmb = Methods.FindChild<EmbriantComboBox>(PopupChkLA2.Child, "");
            if (cmb != null) cmb.SelectedIndex = -1;
            LaData.PolicyData.OptionID = GetOptionID();
            CalculateCost(true);
        }

        private void dteDateOfSale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (dteDateOfSale.IsValueValid && LaData.AppData.IsLeadLoaded)
            {
                if (LaData.AppData.lkpLeadStatus == lkpINLeadStatus.Accepted)
                {
                    LaData.SaleData.SaleDate = LaData.AppData.DateOfSale;
                    LaData.SaleData.ConfWorkDate = LaData.SaleData.AutoPopulateConfWorkDate;
                }
                //if (LaData.AppData.lkpLeadStatus == lkpINLeadStatus.Accepted || LaData.AppData.lkpLeadStatus == null)
                //{
                if (LaData.AppData.DateOfSale != null)
                {
                    DateTime date = (DateTime)LaData.AppData.DateOfSale;

                    if (date.Year == 2015 && date.Month == 11 && (date.Day >= 20 && date.Day <= 30))
                    {
                        LaData.PolicyData.CommenceDate = new DateTime(2016, 02, 01);
                    }
                    else if (date.Year == 2016 && date.Month == 11 && (date.Day >= 21 && date.Day <= 30))
                    {
                        LaData.PolicyData.CommenceDate = new DateTime(2017, 02, 01);
                    }
                    else if (date.Year == 2017 && date.Month == 11 && (date.Day >= 23 && date.Day <= 30))
                    {
                        LaData.PolicyData.CommenceDate = new DateTime(2018, 02, 01);
                    }
                    else if (date.Year == 2018 && date.Month == 11 && (date.Day >= 26 && date.Day <= 30))
                    {
                        LaData.PolicyData.CommenceDate = new DateTime(2019, 02, 01);
                    }
                    else if (date.Year == 2019 && date.Month == 11 && (date.Day >= 21 && date.Day <= 30))
                    {
                        LaData.PolicyData.CommenceDate = new DateTime(2020, 02, 01);
                    }
                    else if (date.Year == 2020 && date.Month == 11 && (date.Day >= 23 && date.Day <= 30))
                    {
                        LaData.PolicyData.CommenceDate = new DateTime(2021, 02, 01);
                    }
                    else
                    {
                        LaData.PolicyData.CommenceDate = new DateTime(date.AddMonths(2).Year, date.AddMonths(2).Month, 1);
                    }
                }
                else
                {
                    LaData.PolicyData.CommenceDate = null;
                }
                //}

                if (LaData.AppData.IsLeadUpgrade) CalculateCostUpgrade(false); else CalculateCost(false);
            }
        }

        private void dteSaleDate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (dteSaleDate.IsValueValid && LaData.AppData.IsLeadLoaded)
            {
                dteDateOfSale.Value = dteSaleDate.Value;
                if (LaData.AppData.lkpLeadStatus == lkpINLeadStatus.Accepted) LaData.AppData.DateOfSale = LaData.SaleData.SaleDate;
            }
        }

        private void cmbAgent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LaData.SaleData.SaleCallRefID = LaData.AppData.AgentID;
            LaData.ImportExtraData.Extension1 = GetUserExtension(LaData.AppData.AgentID);
        }

        private void medInitials_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            if (medInitials.Text != null) medInitials.Text = medInitials.Text.ToUpper();
        }

        private void tbxFeedback_TextChanged(object sender, TextChangedEventArgs e)
        {
            LaData.SaleData.FeedbackDate = DateTime.Now;
        }

        private void chkUDMBumpUp_Unchecked(object sender, RoutedEventArgs e)
        {
            xamCEBumpUpAmount.Visibility = Visibility.Hidden;
        }

        private void chkUDMBumpUp_Checked(object sender, RoutedEventArgs e)
        {
            if (xamCEBumpUpAmount.Value != null && (decimal)xamCEBumpUpAmount.Value > 0)
            {
                xamCEBumpUpAmount.Visibility = Visibility.Visible;
            }
            else
            {
                chkUDMBumpUp.IsChecked = false;
            }
        }

        private void chkReducedPremium_Unchecked(object sender, RoutedEventArgs e)
        {
            xamCEReducedPremiumAmount.Visibility = Visibility.Hidden;


        }

        private void chkReducedPremium_Checked(object sender, RoutedEventArgs e)
        {
            if (xamCEReducedPremiumAmount.Value != null && (decimal)xamCEReducedPremiumAmount.Value > 0)
            {
                xamCEReducedPremiumAmount.Visibility = Visibility.Visible;
            }
            else
            {
                chkReducedPremium.IsChecked = false;
            }
        }

        private void cmbStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void CtrlLeadApplicationScreen_Loaded(object sender, RoutedEventArgs e)
        {
            //CTPhone
            //grdCTPhone.Children.Add(_hostCTPhone);
            //OpenCTPhone();

            if (!LaData.AppData.IsLeadUpgrade && (LaData.AppData.CampaignType == lkpINCampaignType.CancerFuneral ||
                                                  LaData.AppData.CampaignType == lkpINCampaignType.MaccFuneral ||
                                                  LaData.AppData.CampaignType == lkpINCampaignType.CancerFuneral99 ||
                                                  LaData.AppData.CampaignType == lkpINCampaignType.FemaleDis ||
                                                  LaData.AppData.CampaignType == lkpINCampaignType.IGFemaleDisability ||
                                                  LaData.AppData.CampaignType == lkpINCampaignType.BlackMacc ||
                                                  LaData.UserData.UserType != lkpUserType.SalesAgent))
            {
                grdBeneficiaries.Visibility = Visibility.Visible;
                grdBenficiariesPercentageTotalBase.Visibility = Visibility.Visible;
            }
            else
            {
                grdBeneficiaries.Visibility = Visibility.Hidden;
                grdBenficiariesPercentageTotalBase.Visibility = Visibility.Hidden;
            }

            // See https://udmint.basecamphq.com/projects/10204838-udm-hr/todo_items/203770077/comments
            //DisplayLiveProductivity();

            //THANK YOU http://geekswithblogs.net/ilich/archive/2012/10/16/running-code-when-windows-rendering-is-completed.aspx !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (LaData.AppData.ImportID.HasValue)
            {
                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/211823326/comments
                bool isNotesFeatureAvailable = Insure.IsNotesFeatureAvailable(LaData.AppData.ImportID);

                if (isNotesFeatureAvailable)
                {
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        ShowNotes(LaData.AppData.ImportID.Value);
                    }), DispatcherPriority.ContextIdle, null);
                }
            }
        }

        private void chkFuneral_Click(object sender, RoutedEventArgs e)
        {
            if (LaData.PolicyData.LA1Cover == null)
            {
                LaData.PolicyData.IsFuneralChecked = false;
            }

            CalculateCost(true);
        }

        private void TitleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (sender.GetType().Name)
            {
                case "EmbriantComboBox":
                    var comboBoxControl = (EmbriantComboBox)sender;

                    switch (comboBoxControl.Name)
                    {
                        case "cmbTitle":
                            {
                                if (LaData.LeadData.TitleID != null && LaData.BankDetailsData.TitleID == null)
                                {
                                    LaData.BankDetailsData.TitleID = LaData.LeadData.TitleID;
                                }

                                if (LaData.LeadData.TitleID != null)
                                {
                                    if (LaData.LeadData.GenderID == null)
                                    {
                                        LaData.LeadData.GenderID = GetGenderIDFromTitle(LaData.LeadData.TitleID);
                                    }
                                }
                            }
                            break;

                        case "cmbLA2Title":
                            {
                                if (LaData.LAData[1].TitleID != null)
                                {
                                    if (LaData.LAData[1].GenderID == null)
                                    {
                                        LaData.LAData[1].GenderID = GetGenderIDFromTitle(LaData.LAData[1].TitleID);
                                    }
                                }
                            }
                            break;

                        #region Beneficiary Titles

                        case "cmbBeneficiaryTitle1":
                            {
                                if (LaData.BeneficiaryData[0].TitleID != null)
                                {
                                    if (LaData.BeneficiaryData[0].GenderID == null)
                                    {
                                        LaData.BeneficiaryData[0].GenderID = GetGenderIDFromTitle(LaData.BeneficiaryData[0].TitleID);
                                    }
                                }
                            }
                            break;

                        case "cmbBeneficiaryTitle2":
                            {
                                if (LaData.BeneficiaryData[1].TitleID != null)
                                {
                                    if (LaData.BeneficiaryData[1].GenderID == null)
                                    {
                                        LaData.BeneficiaryData[1].GenderID = GetGenderIDFromTitle(LaData.BeneficiaryData[1].TitleID);
                                    }
                                }
                            }
                            break;

                        case "cmbBeneficiaryTitle3":
                            {
                                if (LaData.BeneficiaryData[2].TitleID != null)
                                {
                                    if (LaData.BeneficiaryData[2].GenderID == null)
                                    {
                                        LaData.BeneficiaryData[2].GenderID = GetGenderIDFromTitle(LaData.BeneficiaryData[2].TitleID);
                                    }
                                }
                            }
                            break;

                        case "cmbBeneficiaryTitle4":
                            {
                                if (LaData.BeneficiaryData[3].TitleID != null)
                                {
                                    if (LaData.BeneficiaryData[3].GenderID == null)
                                    {
                                        LaData.BeneficiaryData[3].GenderID = GetGenderIDFromTitle(LaData.BeneficiaryData[3].TitleID);
                                    }
                                }
                            }
                            break;

                        case "cmbBeneficiaryTitle5":
                            {
                                if (LaData.BeneficiaryData[4].TitleID != null)
                                {
                                    if (LaData.BeneficiaryData[4].GenderID == null)
                                    {
                                        LaData.BeneficiaryData[4].GenderID = GetGenderIDFromTitle(LaData.BeneficiaryData[4].TitleID);
                                    }
                                }
                            }
                            break;

                        case "cmbBeneficiaryTitle6":
                            {
                                if (LaData.BeneficiaryData[5].TitleID != null)
                                {
                                    if (LaData.BeneficiaryData[5].GenderID == null)
                                    {
                                        LaData.BeneficiaryData[5].GenderID = GetGenderIDFromTitle(LaData.BeneficiaryData[5].TitleID);
                                    }
                                }
                            }
                            break;

                            #endregion Beneficiary Titles
                    }

                    break;
            }
        }

        //private void btnChangeAddress_Click(object sender, RoutedEventArgs e)
        //{
        //    //EnableAddressControls(true);
        //}

        #region Viewbox-Specific Event Handlers

        private void Viewbox_MouseEnter(object sender, MouseEventArgs e)
        {
            //if (!GridSplitterH.IsMouseOver && !GridSplitterV.IsMouseOver)
            //{
            //    Viewbox_Big(sender);
            //}
        }

        private void Viewbox_MouseLeave(object sender, MouseEventArgs e)
        {
            //if (!((Viewbox)sender).IsKeyboardFocusWithin && !GridSplitterH.IsMouseOver && !GridSplitterV.IsMouseOver)
            //{
            //    Viewbox_Normal(sender);
            //}
        }

        private void Viewbox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender != null)
            {
                Viewbox vb = (Viewbox)sender;

                if (vb.IsMouseOver)
                {
                    _posMouseVB = Mouse.GetPosition(vb);
                }
            }

            Viewbox_Zoom(sender);
        }

        private void Viewbox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!Equals(e.NewFocus, btnNext) && !Equals(e.NewFocus, btnPrevious))
            {
                Viewbox_Normal();
            }
        }

        private void Viewbox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Viewbox viewbox = (Viewbox)sender;

            if (viewbox != null && Convert.ToInt32(viewbox.Tag.ToString()) != 1)
            {
                switch (viewbox.Name)
                {
                    case "vb1":
                        cmbAgent.Focus();
                        break;

                    case "vb2":
                        cmbPolicyPlanGroup.Focus();
                        break;

                    case "vb3":
                        if (cmbLA2TitleUpg.IsEnabled) { cmbLA2TitleUpg.Focus(); }
                        else if (cmbBeneficiaryTitle1Upg.IsEnabled) { cmbBeneficiaryTitle1Upg.Focus(); }
                        break;

                    case "vb4":
                        medSaleStationNo.Focus();
                        break;
                }
            }

            e.Handled = true;
        }

        private void Viewbox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender != null)
            {
                Viewbox vb = (Viewbox)sender;

                if (vb.IsMouseOver)
                {
                    _posVB = vb.PointToScreen(new Point(0d, 0d));

                    double newX = _posMouseVB.X * (e.NewSize.Width / e.PreviousSize.Width);
                    double newY = _posMouseVB.Y * (e.NewSize.Height / e.PreviousSize.Height);

                    SetCursorPos((int)(_posVB.X + newX), (int)(_posVB.Y + newY));
                }

                if (vb.IsKeyboardFocusWithin)
                {
                    switch (vb.Name)
                    {
                        case "vb1":
                            lblPage.Text = "(Lead)";
                            break;

                        case "vb2":
                            lblPage.Text = "(Policy)";
                            break;

                        case "vb3":
                            lblPage.Text = "(LA + Beneficiary)";
                            break;

                        case "vb4":
                            lblPage.Text = "(Sale)";
                            break;

                        default:
                            lblPage.Text = "(Upgrade)";
                            break;
                    }
                }
            }
        }

        #endregion Viewbox-Specific Event Handlers

        private void EmbriantComboBox_DropDownOpened(object sender, EventArgs e)
        {
            EmbriantComboBox ecb = (EmbriantComboBox)sender;
            Viewbox vb = Methods.FindLogicalParent<Viewbox>(ecb);

            if (LaData.AppData.IsLeadUpgrade)
            {
                if (vb != null && (new[] { "vb1", "vb2", "vb3", "vb4" }).Contains(vb.Name) && vb.Tag as int? != 1)
                {
                    ecb.IsDropDownOpen = false;

                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = new TimeSpan(100);
                    timer.Tick += delegate
                    {
                        ecb.IsDropDownOpen = true;
                        timer.Stop();
                    };
                    timer.Start();
                }
            }
        }

        private void CtrlLeadApplicationScreen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (LaData.AppData.IsLeadUpgrade)
            {
                medReference.Focus();
                lblPage.Text = "(Upgrade)";
            }
        }

        private void CtrlLeadApplicationScreen_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.PageUp)
            {
                btnNext_Click(btnNext, null);
            }
            else if (e.Key == Key.PageDown)
            {
                btnPrevious_Click(btnPrevious, null);
            }
        }

        private void btnNextLead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _swNextLead.Reset();
                _swNextLead.Start();

                if ((LaData.AppData2.ImportIDNextLead != null) && (LaData.AppData2.ImportIDNextLead != LaData.AppData.ImportID))
                {
                    long? importId = LaData.AppData2.ImportIDNextLead;

                    ClearApplicationScreen();
                    LoadLead(importId);

                    // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/211823326/comments
                    bool isNotesFeatureAvailable = Insure.IsNotesFeatureAvailable(importId);
                    if (isNotesFeatureAvailable)
                    {
                        ShowNotes(importId.Value);
                    }
                }

                _swNextLead.Stop();
                btnNextLead.ToolTip = Math.Round(_swNextLead.Elapsed.TotalMilliseconds / 1000, 3) + " s";
                _swNextLead.Reset();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        private void btnPrevLead_Click(object sender, RoutedEventArgs e)
        {
            _swPrevLead.Reset();
            _swPrevLead.Start();

            if ((LaData.AppData2.ImportIDPrevLead != null) && (LaData.AppData2.ImportIDPrevLead != LaData.AppData.ImportID))
            {
                long? importId = LaData.AppData2.ImportIDPrevLead;

                ClearApplicationScreen();
                LoadLead(importId);

                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/211823326/comments
                bool isNotesFeatureAvailable = Insure.IsNotesFeatureAvailable(importId);
                if (isNotesFeatureAvailable)
                {
                    ShowNotes(importId.Value);
                }
            }

            _swPrevLead.Stop();
            btnPrevLead.ToolTip = Math.Round(_swPrevLead.Elapsed.TotalMilliseconds / 1000, 3) + " s";
            _swPrevLead.Reset();
        }

        //private void btnNotes_Click(object sender, RoutedEventArgs e)
        //{

        //    if (LaData.AppData.IsLeadLoaded)
        //    {
        //        if (LaData.AppData.ImportID.HasValue)
        //        {
        //            DataTable dtNotes = Insure.GetLeadNotes(LaData.AppData.ImportID.Value);
        //            NoteScreen noteScreen = new NoteScreen(LaData.AppData.ImportID.Value, NoteScreen.NoteScreenMode.ReadOnly);

        //            if (dtNotes.Rows.Count > 0)
        //            {
        //                noteScreen.LoadNote(dtNotes.Rows[0]);
        //            }
        //            else
        //            {
        //                noteScreen.SetFormMode(NoteScreen.NoteScreenMode.New);
        //            }

        //            ShowDialog(noteScreen, new INDialogWindow(noteScreen));
        //        }
        //    }
        //}

        private void btnNotes_Click(object sender, RoutedEventArgs e)
        {

            if (LaData.AppData.IsLeadLoaded)
            {
                if (LaData.AppData.ImportID.HasValue)
                {
                    //DataTable dtNotes = Insure.GetLeadNotes(LaData.AppData.ImportID.Value);
                    NoteScreen noteScreen = new NoteScreen(LaData.AppData.ImportID /*, NoteScreen.NoteScreenMode.ReadOnly*/);
                    ShowDialog(noteScreen, new INDialogWindow(noteScreen));
                }
            }
        }

        private void BtnClearUpgBeneficiary_Click(object sender, RoutedEventArgs e)
        {
            btnClearPersonDetails button = sender as btnClearPersonDetails;

            if (button != null)
            {
                int btnIndex = Convert.ToInt32(button.Name.LastOrDefault().ToString());
                if (btnIndex > 0)
                {
                    if (LaData.BeneficiaryData[btnIndex - 1].BeneficiaryID != null)
                    {
                        long? idBeneficiary = LaData.BeneficiaryData[btnIndex - 1].BeneficiaryID;
                        LaData.BeneficiaryData[btnIndex - 1] = new LeadApplicationData.Beneficiary();
                        LaData.BeneficiaryData[btnIndex - 1].BeneficiaryID = idBeneficiary;
                    }
                }
            }
        }

        private void cmbStatus_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (LaData.AppData.ImportID != null && LaData.AppData.LeadStatus != null && !LaData.AppData.DiaryStatusHandled)
            {
                //see if this can be selected
                if (LaData.UserData.UserType == lkpUserType.SalesAgent)
                {
                    if (LaData.AppData.LoadedLeadStatus == (long?)lkpINLeadStatus.Accepted)
                    {
                        if (LaData.AppData.LoadedDateOfSale < DateTime.Now.AddHours(-24))
                        {
                            return;
                        }
                    }
                }

                if ((lkpINLeadStatus)(LaData.AppData.LeadStatus) == lkpINLeadStatus.Diary)
                {
                    LaData.AppData.DiaryReasonID = null;
                    SelectDiaryReasonScreen selectDiaryReasonScreen = new SelectDiaryReasonScreen(_ssGlobalData);
                    selectDiaryReasonScreen.SelectedDiaryReasonID = LaData.AppData.DiaryReasonID;
                    ShowDialog(selectDiaryReasonScreen, new INDialogWindow(selectDiaryReasonScreen));

                    LaData.AppData.DiaryReasonID = selectDiaryReasonScreen.SelectedDiaryReasonID;

                    if (LaData.AppData.DiaryReasonID == null)
                    {
                        //LaData.AppData.LeadStatus = -1;
                        LaData.AppData.LeadStatus = null;
                    }

                    Methods.FindChild<TextBox>(medReference, "PART_InputTextBox").Focus();
                }
            }
        }

        private void CtrlLeadApplicationScreen_Close(BaseControl nextScreen)
        {
            _ssGlobalData.LeadApplicationScreen = null;
        }

        private void XamContextMenu_ItemClicked(object sender, ItemClickedEventArgs e)
        {

        }

        private void XamMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                XamMenuItem xamMenuItem = sender as XamMenuItem;

                if (xamMenuItem != null && xamMenuItem.Header != null)
                {
                    switch (xamMenuItem.Header.ToString())
                    {
                        #region Web Gift Status

                        case "Web Gift Status":
                            if (LaData.AppData.PlatinumBatchCode != null)
                            {
                                if (LaData.AppData.PlatinumBatchCode.Contains("_"))
                                {
                                    string batchCodeModifier = LaData.AppData.PlatinumBatchCode.Substring(LaData.AppData.PlatinumBatchCode.IndexOf("_", StringComparison.Ordinal)).ToUpper();

                                    if (GlobalConstants.BatchCodes.RedeemGift.Contains(batchCodeModifier))
                                    {
                                        foreach (Window window in Application.Current.Windows)
                                        {
                                            if (window.Title == "Redeem Gift")
                                            {
                                                window.WindowState = WindowState.Normal;
                                                return;
                                            }
                                        }

                                        //IInputElement focusedElement = Keyboard.FocusedElement;//;FocusManager.GetFocusedElement(this);
                                        bool redeemedNow = false;
                                        RedeemGiftScreen redeemGiftScreen = new RedeemGiftScreen(LaData);
                                        redeemGiftScreen.Title = "Redeem Gift";
                                        redeemGiftScreen.hdrRedeemGiftScreen.Text = "Redeem Gift";
                                        //redeemGiftScreen._userData = LaData.UserData;
                                        //redeemGiftScreen._appData = LaData.AppData;
                                        IsEnabled = false;
                                        redeemGiftScreen.ShowDialog();
                                        IsEnabled = true;

                                        FocusManager.SetFocusedElement(_focusScope, _focusedElement);
                                        Keyboard.Focus(_focusedElement);
                                        //Keyboard.Focus(medReference);

                                        //MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));

                                        if (redeemGiftScreen.ScreenData.PreviousRedeemStatus != redeemGiftScreen.ScreenData.GiftRedeemStatusID)
                                        {
                                            if (redeemGiftScreen.ScreenData.GiftRedeemStatusID == 1)
                                            {
                                                //LaData.AppData.LeadStatus = 9;
                                                //cmbStatus.SelectedValuePath = 9;
                                                //cmbStatus.SelectedIndex = 1;

                                                //LaData.AppData.lkpLeadStatus = lkpINLeadStatus.Diary;
                                            }

                                        }
                                        //insert code here
                                    }
                                }
                            }
                            break;

                        #endregion Web Gift Status

                        #region Objection Information

                        case "Objection Information":
                            foreach (Window window in Application.Current.Windows)
                            {
                                if (window.Title == "Objection Information")
                                {
                                    window.WindowState = WindowState.Normal;
                                    return;
                                }
                            }

                            ScriptScreen scriptScreenObjection = new ScriptScreen();
                            scriptScreenObjection.Title = "Objection Information";
                            scriptScreenObjection.hdrScriptScreen.Text = "Objection Information";
                            scriptScreenObjection.ScriptType = lkpScriptType.ObjectionInformation;
                            scriptScreenObjection.LaData = LaData;
                            scriptScreenObjection.Show();
                            break;

                        #endregion Objection Information

                        #region Claim Information

                        case "Claim Information":
                            foreach (Window window in Application.Current.Windows)
                            {
                                if (window.Title == "Claim Information")
                                {
                                    window.WindowState = WindowState.Normal;
                                    return;
                                }
                            }

                            ScriptScreen scriptScreenClaim = new ScriptScreen();
                            scriptScreenClaim.Title = "Claim Information";
                            scriptScreenClaim.hdrScriptScreen.Text = "Claim Information";
                            scriptScreenClaim.ScriptType = lkpScriptType.ClaimInformation;
                            scriptScreenClaim.LaData = LaData;
                            scriptScreenClaim.Show();
                            break;

                        #endregion Claim Information

                        #region Exclusion Information

                        case "Exclusion Information":
                            foreach (Window window in Application.Current.Windows)
                            {
                                if (window.Title == "Exclusion Information")
                                {
                                    window.WindowState = WindowState.Normal;
                                    return;
                                }
                            }

                            ScriptScreen scriptScreenExclusion = new ScriptScreen();
                            scriptScreenExclusion.Title = "Exclusion Information";
                            scriptScreenExclusion.hdrScriptScreen.Text = "Exclusion Information";
                            scriptScreenExclusion.ScriptType = lkpScriptType.ExclusionInformation;
                            scriptScreenExclusion.LaData = LaData;
                            scriptScreenExclusion.Show();
                            break;

                        #endregion Exclusion Information

                        #region Debi-check Statuses    

                        case "Debi-check Statuses":
                            foreach (Window window in Application.Current.Windows)
                            {
                                if (window.Title == "Debi-check Statuses")
                                {
                                    window.WindowState = WindowState.Normal;
                                    return;
                                }
                            }
                            ScriptScreen scriptScreenDebiCheck = new ScriptScreen();
                            scriptScreenDebiCheck.chkAfrikaans.Visibility = Visibility.Hidden;
                            scriptScreenDebiCheck.lblAfrikaans.Visibility = Visibility.Hidden;
                            scriptScreenDebiCheck.Title = "Debi-check Statuses";
                            scriptScreenDebiCheck.hdrScriptScreen.Text = "Debi-check Statuses";
                            scriptScreenDebiCheck.ScriptType = lkpScriptType.DebicheckInformation;
                            scriptScreenDebiCheck.LaData = LaData;
                            scriptScreenDebiCheck.Show();
                            break;

                        #endregion Debi-check Statuses


                        #region RAM Service Test

                        case "RAM Service Test":

                            //TrackingWSSoapClient trackingWsSoapClient = new TrackingWSSoapClient();

                            //string result = trackingWsSoapClient.HelloWorld();
                            //INMessageBoxWindow1 mb = new INMessageBoxWindow1();
                            //ShowMessageBox(mb, result, "RAM Soap Service Call (Test1)", ShowMessageType.Exclamation);

                            //result = trackingWsSoapClient.HelloWorld2(DateTime.Parse("2017-01-01 14:14:14"));
                            //mb = new INMessageBoxWindow1();
                            //ShowMessageBox(mb, result, "RAM Soap Service Call (Test2)", ShowMessageType.Exclamation);

                            //result = trackingWsSoapClient.Logon("user", "password");
                            //mb = new INMessageBoxWindow1();
                            //ShowMessageBox(mb, result, "RAM Soap Service Call (Logon)", ShowMessageType.Exclamation);

                            //bool result2 = trackingWsSoapClient.UserSessionValid("userid", "logonToken");
                            //mb = new INMessageBoxWindow1();
                            //ShowMessageBox(mb, result2.ToString(), "RAM Soap Service Call (UserSessionValid)", ShowMessageType.Exclamation);

                            break;

                        #endregion RAM Service Test

                        #region Call Monitoring Details

                        case "Call Monitoring Details":
                            CallMonitoringDetailsScreen callMonitoringDetailsScreen = new CallMonitoringDetailsScreen(LaData);
                            //IsEnabled = false;-- interferes with the closure checkbox
                            callMonitoringDetailsScreen.ShowDialog();
                            //IsEnabled = true;

                            break;

                        #endregion Call Monitoring Details

                        #region Lead Contact Details

                        case "Lead Contact Details":
                            foreach (Window window in Application.Current.Windows)
                            {
                                if (window.Title == "Lead Contact Details")
                                {
                                    window.WindowState = WindowState.Normal;
                                    return;
                                }
                            }

                            LeadContactDetailsScreen leadContactDetailsScreen = new LeadContactDetailsScreen();
                            leadContactDetailsScreen.LaData = LaData;
                            leadContactDetailsScreen.Show();
                            break;

                        #endregion

                        #region QA Details

                        case "QA Details":

                            QADetailsScreen qaDetailsScreen = new QADetailsScreen((long)LaData.AppData.ImportID);
                            ShowDialog(qaDetailsScreen, new INDialogWindow(qaDetailsScreen));

                            break;

                            #endregion
                    }
                }

            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void XamMenuItem_GotFocus(object sender, RoutedEventArgs e)
        {


            //e.Handled = true;
        }

        private void XamMenuItem_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void XamMenuItem_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            _focusScope = FocusManager.GetFocusScope(this);
            _focusedElement = FocusManager.GetFocusedElement(_focusScope);
        }

        private void chkConfirm_Checked(object sender, RoutedEventArgs e)
        {
            LaData.SaleData.ConfDate = LaData.SaleData.AutoPopulateConfDate;
            LaData.SaleData.ConfTime = LaData.SaleData.AutoPopulateConfTime;
            if (LaData.SaleData.ConfCallRefID == null)
            {
                LaData.SaleData.ConfCallRefID = LaData.SaleData.AutoPopulateConfirmationAgentFKUserID;
            }
            else if (LaData.SaleData.ConfCallRefID != null && LaData.SaleData.ConfCallRefID != LaData.SaleData.AutoPopulateConfirmationAgentFKUserID)
            {
                LaData.SaleData.BatchCallRefID = LaData.SaleData.AutoPopulateConfirmationAgentFKUserID;
            }

        }

        private void medDOAccountNumber_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            XamMaskedEditor xme = sender as XamMaskedEditor;

            if (LaData.AppData.IsLeadLoaded)
            {
                if (xme != null && xme.Text != null)
                {
                    LaData.BankDetailsData.lkpAccNumCheckStatus = lkpINAccNumCheckStatus.UnChecked;
                    if (xme.Text.Length > 6 && LaData.BankDetailsData.BankID != null)
                    {
                        if (LaData.dtFakeBankAccountNumbers.Select("FKBankID = '" + LaData.BankDetailsData.BankID + "' AND AccNo = '" + xme.Text + "'").Any())
                        {
                            INMessageBoxWindow1 mb = new INMessageBoxWindow1();
                            string strMessage = LaData.dtBank.Select("ID = '" + LaData.BankDetailsData.BankID + "'").CopyToDataTable().Rows[0]["Description"] + "\n";
                            strMessage += "'" + xme.Text + "' is a fraudulent account number!\nPlease try another account number ...";
                            ShowMessageBox(mb, strMessage, "Alert", ShowMessageType.Exclamation);
                            //xme.Text = string.Empty;
                            LaData.BankDetailsData.AccountNumber = string.Empty;
                            xme.Focus();
                        }
                    }

                    //if (txtIsMoneyBuilder.Tag != null)
                    //{
                    //    if (txtIsMoneyBuilder.Tag.ToString() == "True")
                    //    {
                    //        LaData.BankDetailsData.AccountNumber = string.Empty;
                    //        xme.Focus();
                    //    }
                    //}                    
                }
            }
        }

        private void cmbDOBankBranchCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EmbriantComboBox xme = sender as EmbriantComboBox;
            if (LaData.AppData.IsLeadLoaded)
            {
                if (xme != null && xme.Text != null)
                {
                    LaData.BankDetailsData.lkpAccNumCheckStatus = lkpINAccNumCheckStatus.UnChecked;
                }
            }
        }

        private void cmbDOAccountType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (!(cmbDOBank.SelectedValue == null) && !(cmbDOAccountType.SelectedValue == null))
            //if (!(LaData.BankDetailsData.BankID == 240) && !(cmbDOAccountType.SelectedValue == null))
            //{
            if (LaData.BankDetailsData.BankID == 240 && LaData.BankDetailsData.AccountTypeID == 3 /*&& medDOAccountNumber.IsValueValid*/ && LaData.BankDetailsData.lkpAccNumCheckStatus != lkpINAccNumCheckStatus.UnChecked)
            {
                CheckIfMoneyBuilderAccount();
            }
            //}
        }

        private void txtIsMoneyBuilder_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Boolean isMoneyBuilder = false;

            //if (txtIsMoneyBuilder.Text == "True")
            //{
            //    if (LaData.AppData.IsLeadLoaded)
            //    {
            //        CheckIfMoneyBuilderAccount();
            //    }


            //    //if (isMoneyBuilder)
            //    //{
            //    //    txtIsMoneyBuilder.Tag = true;
            //    //    LaData.BankDetailsData.AccountNumber = string.Empty;
            //    //    medDOAccountNumber.GetBindingExpression(XamMaskedEditor.TextProperty).UpdateSource();
            //    //}
            //    //else
            //    //{
            //    //    txtIsMoneyBuilder.Tag = false;
            //    //}
            //}            
        }

        private void medAddressFields_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            if (LaData.AppData.IsLeadLoaded &&
                LaData.AppData.IsLeadUpgrade &&
                LaData.UserData.UserType == lkpUserType.SalesAgent)
            {
                if (Convert.ToString((object)LaData.LeadData.Address1).ToLower() == Convert.ToString((object)LaData.LeadHistoryData.Address1).ToLower() &&
                    Convert.ToString((object)LaData.LeadData.Address2).ToLower() == Convert.ToString((object)LaData.LeadHistoryData.Address2).ToLower() &&
                    Convert.ToString((object)LaData.LeadData.Address3).ToLower() == Convert.ToString((object)LaData.LeadHistoryData.Address3).ToLower() &&
                    Convert.ToString((object)LaData.LeadData.Address4).ToLower() == Convert.ToString((object)LaData.LeadHistoryData.Address4).ToLower() &&
                    Convert.ToString((object)LaData.LeadData.Address5).ToLower() == Convert.ToString((object)LaData.LeadHistoryData.Address5).ToLower() &&
                    LaData.LeadData.PostalCode != LaData.LeadHistoryData.PostalCode)
                {
                    IInputElement focusedControl = Keyboard.FocusedElement;
                    LaData.LeadData.PostalCode = LaData.LeadHistoryData.PostalCode;
                    INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                    ShowMessageBox(messageWindow, @"The postal code cannot be changed without changing any of the other address fields.", "Postal Code", ShowMessageType.Error);
                    Keyboard.Focus(focusedControl);
                }
            }
        }

        private void chkShowAllOptions_Checked(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(chkShowAllOptions.Tag) == 1)
            {
                chkShowAllOptions.Tag = null;
                return;
            }
            cmbPolicyPlan_SelectionChanged(null, null);
        }

        private void chkShowAllOptions_Unchecked(object sender, RoutedEventArgs e)
        {
            chkShowAllOptions.Tag = null;
            cmbPolicyPlan_SelectionChanged(null, null);
        }

        #endregion Event Handlers

        #region Popups

        #region Popup Methods



        #endregion

        #region Popup Events

        private void Popup_Loaded(object sender, RoutedEventArgs e)
        {
            Popup popup = (Popup)sender;
            ContentControl cc = (ContentControl)popup.Child;
            cc.ApplyTemplate();
        }

        private void PopupContentControl_Loaded(object sender, RoutedEventArgs e)
        {
            ContentControl cc = (ContentControl)sender;
            DataTemplate dt = cc.ContentTemplate;

            ContentPresenter cp = null;
            if (VisualTreeHelper.GetChildrenCount(cc) > 0)
            {
                cp = VisualTreeHelper.GetChild(cc, 0) as ContentPresenter;
                if (cp != null) cp.ApplyTemplate();
            }

            if (dt != null && cp != null)
            {
                Button btnClosePopup = (Button)dt.FindName("btnClosePopup", cp);
                btnClosePopup.Click += PopupCloseButton_Click;
            }
        }

        private void PopupCloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                Popup popup = (Popup)(Methods.FindParent<ContentControl>((Button)sender)).Parent;

                if (popup != null) popup.IsOpen = false;
            }
        }

        private void Popup_Opened(object sender, EventArgs e)
        {
            if (sender != null)
            {
                Popup popup = (Popup)sender;

                EmbriantComboBox cmb = Methods.FindChild<EmbriantComboBox>(popup.Child, "");
                if (cmb != null)
                {
                    cmb.Focus();
                }

                switch (popup.Name)
                {
                    case "PopupChkLA2":
                        if (cmb != null)
                        {
                            decimal? LA1Cover = LaData.PolicyData.LA1Cover;
                            string strQuery = "SELECT LA2Cover [Description] FROM INOption WHERE FKINPlanID = '" + LaData.PolicyData.PlanID + "' AND LA1Cover = '" + LA1Cover + "' AND IsActive = '1' ";
                            strQuery = strQuery + "AND LA2Cover != 0";

                            DataTable dt = Methods.GetTableData(strQuery);
                            cmb.Populate(dt, DescriptionField, null);
                            cmb.SelectedValuePath = DescriptionField;
                        }
                        break;
                }
            }
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            if (sender != null)
            {
                Popup popup = (Popup)sender;

                EmbriantComboBox cmb = Methods.FindChild<EmbriantComboBox>(popup.Child, "");

                switch (popup.Name)
                {
                    case "PopupChkLA2":
                        if (cmb != null)
                        {
                            if (cmb.SelectedIndex == -1)
                            {
                                LaData.PolicyData.IsLA2Checked = false;
                            }
                            else
                            {
                                LaData.PolicyData.OptionID = GetOptionID();
                                CalculateCost(true);
                            }
                        }

                        switch (_popupKey)
                        {
                            case Key.Tab:
                                chkChild.Focus();
                                break;

                            default:
                                chkLA2.Focus();
                                break;
                        }

                        break;
                }

            }
        }

        private void Popup_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender != null)
            {
                Popup popup = (Popup)sender;

                switch (e.Key)
                {
                    case Key.Tab:
                        _popupKey = Key.Tab;
                        popup.IsOpen = false;
                        break;

                    case Key.Escape:
                        EmbriantComboBox cmb = Methods.FindChild<EmbriantComboBox>(PopupChkLA2.Child, "");
                        if (cmb != null) cmb.SelectedIndex = -1;
                        _popupKey = Key.Escape;
                        popup.IsOpen = false;
                        break;

                    case Key.Enter:
                        _popupKey = Key.Enter;
                        popup.IsOpen = false;
                        break;
                }
            }
        }





        #endregion

        #endregion Popups





        #region Debug

        //void handler(object sender, RoutedEventArgs e)
        //{
        //    if (e.OriginalSource.ToString().Contains("System.Windows.Controls.Viewbox"))
        //    {
        //        if (e.RoutedEvent.ToString() != "CommandManager.PreviewCanExecute" &&
        //            e.RoutedEvent.ToString() != "CommandManager.CanExecute" && 
        //            e.RoutedEvent.ToString() != "InputManager.InputReport" && 
        //            e.RoutedEvent.ToString() != "InputManager.PreviewInputReport")
        //        Console.WriteLine(e.OriginalSource + "=>" + e.RoutedEvent);
        //    }
        //}

        #endregion

        #region Commands

        private void SelectHamburgerMenuCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SelectHamburgerMenuCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            contextMenuManager.ContextMenu.IsOpen = true;
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //hoping this will replace that massive set of triggers in xaml someday 01-08-2017 (PM)
            //these rules below are additional to the triggers for the save button in xaml

            //btnSave.Content = "Save";
            LaData.AppData.CanChangeStatus = true;//cmbStatus.IsEnabled = true;//Nicolas changed this in order to preserve the binding on cmbStatus.IsEnabled
            LaData.AppData.IsLocked = false;

            if (LaData.UserData.UserType == lkpUserType.SalesAgent && LaData.AppData.IsCopied)
            {
                //Image img = new Image();
                //img.Source = new BitmapImage(new Uri(@"../Resources/lock.png", UriKind.Relative));
                //img.Height = 25;

                //btnSave.Content = img;
                //btnSave.ToolTip = "Lead is Locked";
                LaData.AppData.CanChangeStatus = false;//cmbStatus.IsEnabled = false;//Nicolas changed this in order to preserve the binding on cmbStatus.IsEnabled
                LaData.AppData.IsLocked = true;
                e.CanExecute = false;
                return;
            }

            if (LaData.UserData.UserType == lkpUserType.SalesAgent
                &&
                (LaData.AppData.lkpLeadStatus == lkpINLeadStatus.CallMonitoringCarriedForward
                || LaData.AppData.lkpLeadStatus == lkpINLeadStatus.CallMonitoringCancellation
                || LaData.AppData.lkpLeadStatus == lkpINLeadStatus.Cancelled
                || LaData.AppData.lkpLeadStatus == lkpINLeadStatus.CarriedForward
                ))
            {
                LaData.AppData.CanChangeStatus = false;//cmbStatus.IsEnabled = false;//Nicolas changed this in order to preserve the binding on cmbStatus.IsEnabled
            }

            if (LaData.AppData.IsLeadUpgrade)
            {
                if (LaData.UserData.UserType == lkpUserType.SalesAgent)
                {
                    if (LaData?.AppData?.UDMBatchCode != null)
                    {
                        if (Regex.IsMatch(LaData.AppData.UDMBatchCode, @"^(200\d|20[0-9]\d)(0[1-9]|1[0-2]).*$"))
                        {
                            string strBatchCodeDatePart = LaData.AppData.UDMBatchCode.Substring(0, 6);
                            DateTime batchDate = DateTime.ParseExact(strBatchCodeDatePart + "01", "yyyyMMdd", CultureInfo.InvariantCulture);
                            DateTime batchEndDate = batchDate.AddMonths(3);

                            if (DateTime.Now.Date >= batchEndDate.Date)
                            {
                                //Image img = new Image();
                                //img.Source = new BitmapImage(new Uri(@"../Resources/lock.png", UriKind.Relative));
                                //img.Height = 25;

                                //btnSave.Content = img;
                                //btnSave.ToolTip = "Lead is Locked";
                                LaData.AppData.CanChangeStatus = false;//cmbStatus.IsEnabled = false;//Nicolas changed this in order to preserve the binding on cmbStatus.IsEnabled
                                LaData.AppData.IsLocked = true;
                                e.CanExecute = false;
                                return;
                            }
                        }
                    }
                }
            }
            else if (!LaData.AppData.IsLeadUpgrade)
            {
                try
                {
                    if (LaData.UserData.UserType == lkpUserType.SalesAgent)
                    {
                        string IsCompleted = Convert.ToString(Methods.GetTableData("SELECT Completed FROM INBatch WHERE Code =" + "'" + LaData.AppData.PlatinumBatchCode + "'" + " AND FKINCampaignID =" + LaData.AppData.CampaignID).Rows[0][0]);


                        if (IsCompleted == "True")
                        {
                            LaData.AppData.CanChangeStatus = false;//cmbStatus.IsEnabled = false;//Nicolas changed this in order to preserve the binding on cmbStatus.IsEnabled
                            LaData.AppData.IsLocked = true;

                            e.CanExecute = false;
                            return;
                        }
                    }
                }
                catch
                {

                }
            }


            bool upgLa2DetailsRequired = false;
            bool upgLa2DetailsComplete = false;

            bool upgBenDetailsRequired = false;
            bool[] upgBenDetailsComplete = { false, false, false, false, false, false };
            bool allUpgBenDetailsComplete = true;
            bool upgTotalBeneficiaryPercentage100 = false;

            bool baseBenDetailsRequired = false;
            bool[] baseBenDetailsComplete = { false, false, false, false, false, false };
            bool allBaseBenDetailsComplete = true;
            bool baseTotalBeneficiaryPercentage100 = false;

            #region Determine Booleans First

            switch (LaData.UserData.UserType)
            {
                case lkpUserType.SalesAgent:
                case lkpUserType.Administrator:
                case lkpUserType.ConfirmationAgent:

                    switch (LaData.AppData.lkpLeadStatus)
                    {
                        case lkpINLeadStatus.Accepted:

                            if (LaData.AppData.IsLeadUpgrade)
                            {
                                if (cmbUpgradeCover.Text.Contains("LA2"))
                                {
                                    upgLa2DetailsRequired = true;
                                }

                                if (cmbUpgradeCover.Text.Contains("Death"))
                                {
                                    upgBenDetailsRequired = true;
                                }

                                if (cmbUpgradeCover.Text.Contains("Funeral"))
                                {
                                    upgBenDetailsRequired = true;
                                }
                            }
                            else
                            {
                                if (chkFuneral.IsChecked == true)
                                {
                                    baseBenDetailsRequired = true;
                                }
                            }

                            break;
                    }

                    break;
            }

            if (LaData.AppData.IsLeadUpgrade)
            {
                if (upgLa2DetailsRequired)
                {
                    upgLa2DetailsComplete = LaData.LAData[1].TitleID != null &&
                                            LaData.LAData[1].GenderID != null &&
                                            !string.IsNullOrWhiteSpace(LaData.LAData[1].Name) &&
                                            !string.IsNullOrWhiteSpace(LaData.LAData[1].Surname) &&
                                            LaData.LAData[1].DateOfBirth != null &&
                                            LaData.LAData[1].RelationshipID != null;

                }

                if (upgBenDetailsRequired)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (i == 0)
                        {
                            upgBenDetailsComplete[i] = LaData.BeneficiaryData[i].TitleID != null &&
                                                       LaData.BeneficiaryData[i].GenderID != null &&
                                                       !string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Name) &&
                                                       !string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Surname) &&
                                                       LaData.BeneficiaryData[i].DateOfBirth != null &&
                                                       LaData.BeneficiaryData[i].RelationshipID != null //&&
                                                                                                        //(!string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].TelContact) || 
                                                                                                        //(string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].TelContact) && LaData.BeneficiaryData[i].DateOfBirth != null && Methods.CalculateAge((DateTime)LaData.BeneficiaryData[i].DateOfBirth)[0] < 18))
                                                        ||
                            (!string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Name) && ((LaData.BeneficiaryData[i].Name).ToLower() == "estate" || (LaData.BeneficiaryData[i].Name).ToLower() == "trust"));
                        }
                        else
                        {
                            upgBenDetailsComplete[i] = (
                                                            LaData.BeneficiaryData[i].TitleID != null &&
                                                            LaData.BeneficiaryData[i].GenderID != null &&
                                                            !string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Name) &&
                                                            !string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Surname) &&
                                                            LaData.BeneficiaryData[i].DateOfBirth != null &&
                                                            LaData.BeneficiaryData[i].RelationshipID != null //&&
                                                                                                             //(!string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].TelContact) || 
                                                                                                             //(string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].TelContact) && LaData.BeneficiaryData[i].DateOfBirth != null && Methods.CalculateAge((DateTime)LaData.BeneficiaryData[i].DateOfBirth)[0] < 18))
                                                        )
                                                        ||
                                                        !(
                                                            LaData.BeneficiaryData[i].TitleID != null ||
                                                            LaData.BeneficiaryData[i].GenderID != null ||
                                                            !string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Name) ||
                                                            !string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Surname) ||
                                                            LaData.BeneficiaryData[i].DateOfBirth != null ||
                                                            LaData.BeneficiaryData[i].RelationshipID != null //||
                                                                                                             //(!string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].TelContact) || 
                                                                                                             //(string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].TelContact) && LaData.BeneficiaryData[i].DateOfBirth != null && Methods.CalculateAge((DateTime)LaData.BeneficiaryData[i].DateOfBirth)[0] < 18))
                                                          );
                        }

                        allUpgBenDetailsComplete = allUpgBenDetailsComplete && upgBenDetailsComplete[i];
                    }

                    int iResult;
                    int.TryParse(lblBeneficiaryPercentageTotalUpg.Text, out iResult);
                    if (iResult != 0)
                    {
                        upgTotalBeneficiaryPercentage100 = iResult == 100;
                    }
                    else
                    {
                        upgTotalBeneficiaryPercentage100 = false;
                    }
                }
            }
            else
            {
                if (baseBenDetailsRequired)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (i == 0)
                        {
                            baseBenDetailsComplete[i] = LaData.BeneficiaryData[i].TitleID != null &&
                                                       LaData.BeneficiaryData[i].GenderID != null &&
                                                       !string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Name) &&
                                                       !string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Surname) &&
                                                       LaData.BeneficiaryData[i].DateOfBirth != null &&
                                                       LaData.BeneficiaryData[i].RelationshipID != null //&&
                                                                                                        //(!string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].TelContact) ||
                                                                                                        //(string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].TelContact) && LaData.BeneficiaryData[i].DateOfBirth != null && Methods.CalculateAge((DateTime)LaData.BeneficiaryData[i].DateOfBirth)[0] < 18))
                                                        ||
                            (!string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Name) && ((LaData.BeneficiaryData[i].Name).ToLower() == "estate" || (LaData.BeneficiaryData[i].Name).ToLower() == "trust"));
                        }
                        else
                        {
                            baseBenDetailsComplete[i] = (
                                                         LaData.BeneficiaryData[i].TitleID != null &&
                                                         LaData.BeneficiaryData[i].GenderID != null &&
                                                         !string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Name) &&
                                                         !string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Surname) &&
                                                         LaData.BeneficiaryData[i].DateOfBirth != null &&
                                                         LaData.BeneficiaryData[i].RelationshipID != null //&&
                                                                                                          //(!string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].TelContact) ||
                                                                                                          //(string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].TelContact) && LaData.BeneficiaryData[i].DateOfBirth != null && Methods.CalculateAge((DateTime)LaData.BeneficiaryData[i].DateOfBirth)[0] < 18))
                                                        )
                                                        ||
                                                        !(
                                                          LaData.BeneficiaryData[i].TitleID != null ||
                                                          LaData.BeneficiaryData[i].GenderID != null ||
                                                          !string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Name) ||
                                                          !string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Surname) ||
                                                          LaData.BeneficiaryData[i].DateOfBirth != null ||
                                                          LaData.BeneficiaryData[i].RelationshipID != null //&&
                                                                                                           //(!string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].TelContact) ||
                                                                                                           //(string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].TelContact) && LaData.BeneficiaryData[i].DateOfBirth != null && Methods.CalculateAge((DateTime)LaData.BeneficiaryData[i].DateOfBirth)[0] < 18))
                                                         );
                        }

                        allBaseBenDetailsComplete = allBaseBenDetailsComplete && baseBenDetailsComplete[i];
                    }

                    int iResult;
                    int.TryParse(lblBeneficiaryPercentageTotalBase.Text, out iResult);
                    if (iResult != 0)
                    {
                        baseTotalBeneficiaryPercentage100 = iResult == 100;
                    }
                    else
                    {
                        baseTotalBeneficiaryPercentage100 = false;
                    }
                }
            }

            #endregion

            #region Mark/UnMark Incomplete Fields and Set ToolTip

            if (LaData.AppData.IsLeadUpgrade)
            {
                //Upgrade LA2 Details
                if (upgLa2DetailsRequired && !upgLa2DetailsComplete)
                {
                    cmbLA2TitleUpg.BorderBrush = LaData.LAData[1].TitleID == null ? Brushes.Red : defaultBorderBrush;
                    cmbLA2GenderUpg.BorderBrush = LaData.LAData[1].GenderID == null ? Brushes.Red : defaultBorderBrush;
                    medLA2NameUpg.BorderBrush = string.IsNullOrWhiteSpace(LaData.LAData[1].Name) ? Brushes.Red : defaultBorderBrush;
                    medLA2SurnameUpg.BorderBrush = string.IsNullOrWhiteSpace(LaData.LAData[1].Surname) ? Brushes.Red : defaultBorderBrush;
                    Border mainBorder = Methods.FindChild<Border>(dteLA2DateOfBirthUpg, "MainBorder");
                    if (mainBorder != null)
                    {
                        mainBorder.BorderBrush = LaData.LAData[1].DateOfBirth == null ? Brushes.Red : defaultBorderBrush;
                    }
                    cmbLA2RelationshipUpg.BorderBrush = LaData.LAData[1].RelationshipID == null ? Brushes.Red : defaultBorderBrush;

                    grdSave.ToolTip = "LA2 Information Incomplete";
                    e.CanExecute = false;
                    return;
                }
                else
                {
                    cmbLA2TitleUpg.BorderBrush = defaultBorderBrush;
                    cmbLA2GenderUpg.BorderBrush = defaultBorderBrush;
                    medLA2NameUpg.BorderBrush = defaultBorderBrush;
                    medLA2SurnameUpg.BorderBrush = defaultBorderBrush;
                    Border mainBorder = Methods.FindChild<Border>(dteLA2DateOfBirthUpg, "MainBorder");
                    if (mainBorder != null)
                    {
                        mainBorder.BorderBrush = defaultBorderBrush;
                    }
                    cmbLA2RelationshipUpg.BorderBrush = defaultBorderBrush;

                    grdSave.ToolTip = null;
                    e.CanExecute = true;
                }

                //Upgrade Beneficiary Details
                if (upgBenDetailsRequired && !allUpgBenDetailsComplete)
                {
                    string strName;
                    EmbriantComboBox cmb;
                    XamMaskedEditor med;
                    XamDateTimeEditor dte;
                    Border mainBorder;

                    for (int i = 0; i < 6; i++)
                    {
                        if (!upgBenDetailsComplete[i])
                        {
                            strName = "cmbBeneficiaryTitle" + (i + 1) + "Upg";
                            cmb = (EmbriantComboBox)(FindName(strName));
                            cmb.BorderBrush = LaData.BeneficiaryData[i].TitleID == null ? Brushes.Red : defaultBorderBrush;

                            strName = "cmbBeneficiaryGender" + (i + 1) + "Upg";
                            cmb = (EmbriantComboBox)(FindName(strName));
                            cmb.BorderBrush = LaData.BeneficiaryData[i].GenderID == null ? Brushes.Red : defaultBorderBrush;

                            strName = "medBeneficiaryName" + (i + 1) + "Upg";
                            med = (XamMaskedEditor)(FindName(strName));
                            med.BorderBrush = string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Name) ? Brushes.Red : defaultBorderBrush;

                            strName = "medBeneficiarySurname" + (i + 1) + "Upg";
                            med = (XamMaskedEditor)(FindName(strName));
                            med.BorderBrush = string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Surname) ? Brushes.Red : defaultBorderBrush;

                            strName = "dteBeneficiaryDateOfBirth" + (i + 1) + "Upg";
                            dte = (XamDateTimeEditor)(FindName(strName));
                            mainBorder = Methods.FindChild<Border>(dte, "MainBorder");
                            if (mainBorder != null)
                            {
                                mainBorder.BorderBrush = LaData.BeneficiaryData[i].DateOfBirth == null ? Brushes.Red : defaultBorderBrush;
                            }

                            strName = "cmbBeneficiaryRelationship" + (i + 1) + "Upg";
                            cmb = (EmbriantComboBox)(FindName(strName));
                            cmb.BorderBrush = LaData.BeneficiaryData[i].RelationshipID == null ? Brushes.Red : defaultBorderBrush;
                        }
                        else
                        {
                            strName = "cmbBeneficiaryTitle" + (i + 1) + "Upg";
                            cmb = (EmbriantComboBox)(FindName(strName));
                            cmb.BorderBrush = defaultBorderBrush;

                            strName = "cmbBeneficiaryGender" + (i + 1) + "Upg";
                            cmb = (EmbriantComboBox)(FindName(strName));
                            cmb.BorderBrush = defaultBorderBrush;

                            strName = "medBeneficiaryName" + (i + 1) + "Upg";
                            med = (XamMaskedEditor)(FindName(strName));
                            med.BorderBrush = defaultBorderBrush;

                            strName = "medBeneficiarySurname" + (i + 1) + "Upg";
                            med = (XamMaskedEditor)(FindName(strName));
                            med.BorderBrush = defaultBorderBrush;

                            strName = "dteBeneficiaryDateOfBirth" + (i + 1) + "Upg";
                            dte = (XamDateTimeEditor)(FindName(strName));
                            mainBorder = Methods.FindChild<Border>(dte, "MainBorder");
                            if (mainBorder != null)
                            {
                                mainBorder.BorderBrush = defaultBorderBrush;
                            }

                            strName = "cmbBeneficiaryRelationship" + (i + 1) + "Upg";
                            cmb = (EmbriantComboBox)(FindName(strName));
                            cmb.BorderBrush = defaultBorderBrush;
                        }
                    }

                    grdSave.ToolTip = "Beneficiary Information Incomplete";
                    e.CanExecute = false;
                    return;
                }
                else
                {
                    string strName;
                    EmbriantComboBox cmb;
                    XamMaskedEditor med;
                    XamDateTimeEditor dte;
                    Border mainBorder;

                    for (int i = 0; i < 6; i++)
                    {
                        strName = "cmbBeneficiaryTitle" + (i + 1) + "Upg";
                        cmb = (EmbriantComboBox)(FindName(strName));
                        cmb.BorderBrush = defaultBorderBrush;

                        strName = "cmbBeneficiaryGender" + (i + 1) + "Upg";
                        cmb = (EmbriantComboBox)(FindName(strName));
                        cmb.BorderBrush = defaultBorderBrush;

                        strName = "medBeneficiaryName" + (i + 1) + "Upg";
                        med = (XamMaskedEditor)(FindName(strName));
                        med.BorderBrush = defaultBorderBrush;

                        strName = "medBeneficiarySurname" + (i + 1) + "Upg";
                        med = (XamMaskedEditor)(FindName(strName));
                        med.BorderBrush = defaultBorderBrush;

                        strName = "dteBeneficiaryDateOfBirth" + (i + 1) + "Upg";
                        dte = (XamDateTimeEditor)(FindName(strName));
                        mainBorder = Methods.FindChild<Border>(dte, "MainBorder");
                        if (mainBorder != null)
                        {
                            mainBorder.BorderBrush = defaultBorderBrush;
                        }

                        strName = "cmbBeneficiaryRelationship" + (i + 1) + "Upg";
                        cmb = (EmbriantComboBox)(FindName(strName));
                        cmb.BorderBrush = defaultBorderBrush;
                    }

                    grdSave.ToolTip = null;
                    e.CanExecute = true;
                }

                if (upgBenDetailsRequired && !upgTotalBeneficiaryPercentage100)
                {
                    grdSave.ToolTip = "Sum of Beneficiary Percentages Not Equal to 100%";
                    e.CanExecute = false;
                    return;
                }
            }
            else
            {
                //Base Beneficiary Details
                if (baseBenDetailsRequired && !allBaseBenDetailsComplete)
                {
                    string strName;
                    EmbriantComboBox cmb;
                    XamMaskedEditor med;
                    XamDateTimeEditor dte;
                    Border mainBorder;

                    for (int i = 0; i < 6; i++)
                    {
                        if (!baseBenDetailsComplete[i])
                        {
                            strName = "cmbBeneficiaryTitle" + (i + 1);
                            cmb = (EmbriantComboBox)(FindName(strName));
                            cmb.BorderBrush = LaData.BeneficiaryData[i].TitleID == null ? Brushes.Red : defaultBorderBrush;

                            strName = "cmbBeneficiary" + (i + 1) + "Gender";
                            cmb = (EmbriantComboBox)(FindName(strName));
                            cmb.BorderBrush = LaData.BeneficiaryData[i].GenderID == null ? Brushes.Red : defaultBorderBrush;

                            strName = "medBeneficiaryName" + (i + 1);
                            med = (XamMaskedEditor)(FindName(strName));
                            med.BorderBrush = string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Name) ? Brushes.Red : defaultBorderBrush;

                            strName = "medBeneficiarySurname" + (i + 1);
                            med = (XamMaskedEditor)(FindName(strName));
                            med.BorderBrush = string.IsNullOrWhiteSpace(LaData.BeneficiaryData[i].Surname) ? Brushes.Red : defaultBorderBrush;

                            strName = "dteBeneficiaryDateOfBirth" + (i + 1);
                            dte = (XamDateTimeEditor)(FindName(strName));
                            mainBorder = Methods.FindChild<Border>(dte, "MainBorder");
                            if (mainBorder != null)
                            {
                                mainBorder.BorderBrush = LaData.BeneficiaryData[i].DateOfBirth == null ? Brushes.Red : defaultBorderBrush;
                            }

                            strName = "cmbBeneficiaryRelationship" + (i + 1);
                            cmb = (EmbriantComboBox)(FindName(strName));
                            cmb.BorderBrush = LaData.BeneficiaryData[i].RelationshipID == null ? Brushes.Red : defaultBorderBrush;
                        }
                        else
                        {
                            strName = "cmbBeneficiaryTitle" + (i + 1);
                            cmb = (EmbriantComboBox)(FindName(strName));
                            cmb.BorderBrush = defaultBorderBrush;

                            strName = "cmbBeneficiary" + (i + 1) + "Gender";
                            cmb = (EmbriantComboBox)(FindName(strName));
                            cmb.BorderBrush = defaultBorderBrush;

                            strName = "medBeneficiaryName" + (i + 1);
                            med = (XamMaskedEditor)(FindName(strName));
                            med.BorderBrush = defaultBorderBrush;

                            strName = "medBeneficiarySurname" + (i + 1);
                            med = (XamMaskedEditor)(FindName(strName));
                            med.BorderBrush = defaultBorderBrush;

                            strName = "dteBeneficiaryDateOfBirth" + (i + 1);
                            dte = (XamDateTimeEditor)(FindName(strName));
                            mainBorder = Methods.FindChild<Border>(dte, "MainBorder");
                            if (mainBorder != null)
                            {
                                mainBorder.BorderBrush = defaultBorderBrush;
                            }

                            strName = "cmbBeneficiaryRelationship" + (i + 1);
                            cmb = (EmbriantComboBox)(FindName(strName));
                            cmb.BorderBrush = defaultBorderBrush;
                        }
                    }

                    grdSave.ToolTip = "Beneficiary Information Incomplete";
                    e.CanExecute = false;
                    return;
                }
                else
                {
                    string strName;
                    EmbriantComboBox cmb;
                    XamMaskedEditor med;
                    XamDateTimeEditor dte;
                    Border mainBorder;

                    for (int i = 0; i < 6; i++)
                    {
                        strName = "cmbBeneficiaryTitle" + (i + 1);
                        cmb = (EmbriantComboBox)(FindName(strName));
                        cmb.BorderBrush = defaultBorderBrush;

                        strName = "cmbBeneficiary" + (i + 1) + "Gender";
                        cmb = (EmbriantComboBox)(FindName(strName));
                        cmb.BorderBrush = defaultBorderBrush;

                        strName = "medBeneficiaryName" + (i + 1);
                        med = (XamMaskedEditor)(FindName(strName));
                        med.BorderBrush = defaultBorderBrush;

                        strName = "medBeneficiarySurname" + (i + 1);
                        med = (XamMaskedEditor)(FindName(strName));
                        med.BorderBrush = defaultBorderBrush;

                        strName = "dteBeneficiaryDateOfBirth" + (i + 1);
                        dte = (XamDateTimeEditor)(FindName(strName));
                        mainBorder = Methods.FindChild<Border>(dte, "MainBorder");
                        if (mainBorder != null)
                        {
                            mainBorder.BorderBrush = defaultBorderBrush;
                        }

                        strName = "cmbBeneficiaryRelationship" + (i + 1);
                        cmb = (EmbriantComboBox)(FindName(strName));
                        cmb.BorderBrush = defaultBorderBrush;
                    }

                    grdSave.ToolTip = null;
                    e.CanExecute = true;
                }

                if (baseBenDetailsRequired && !baseTotalBeneficiaryPercentage100)
                {
                    grdSave.ToolTip = "Sum of Beneficiary Percentages Not Equal to 100%";
                    e.CanExecute = false;
                    return;
                }
            }

            #endregion

        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        #endregion

        #region CTPhone

        private void InitializeCTPhone()
        {
            _hostCTPhone = new System.Windows.Forms.Integration.WindowsFormsHost();
            _axCtPhone = new AxCTPhone_ActiveX();
            _hostCTPhone.Child = _axCtPhone;

            _axCtPhone.OnMonitoringStarted += AxCtPhoneOnMonitoringStarted;
            _axCtPhone.OnMonitoringStopped += AxCtPhoneOnMonitoringStopped;
            _axCtPhone.OnAgentLogin += AxCtPhoneOnAgentLogin;
            _axCtPhone.OnAgentLogout += AxCtPhoneOnAgentLogout;
            _axCtPhone.OnAgentNotReadyWithReason += AxCtPhoneOnAgentNotReadyWithReason;

            _axCtPhone.OnOffhook += AxCtPhoneOnOffhook;
            _axCtPhone.OnRecordingStarted += AxCtPhoneOnRecordingStarted;
            _axCtPhone.OnRecordingStopped += AxCtPhoneOnRecordingStopped;
            _axCtPhone.OnTpDisconnected += AxCtPhoneOnTpDisconnected;
            _axCtPhone.OnTpAnswered += AxCtPhoneOnTpAnswered;
            _axCtPhone.OnInboundCall += AxCtPhoneOnInboundCall;

            _axCtPhone.OnAgentReady += AxCtPhoneOnAgentReady;
            _axCtPhone.OnAgentBusy += AxCtPhoneOnAgentBusy;
            _axCtPhone.OnAgentAfterCallWork += AxCtPhoneOnAgentAfterCallWork;

            _axCtPhone.OnOpAnswered += AxCtPhoneOnOpAnswered;
            _axCtPhone.OnOpDisconnected += AxCtPhoneOnOpDisconnected;

            _axCtPhone.OnDestRinging += AxCtPhoneOnDestRinging;
            _axCtPhone.OnDestBusy += AxCtPhoneOnDestBusy;
            _axCtPhone.OnDestInvalid += AxCtPhoneOnDestInvalid;
        }

        private void AxCtPhoneOnDestInvalid(object sender, ICTPhone_ActiveXEvents_OnDestInvalidEvent ictPhoneActiveXEventsOnDestInvalidEvent)
        {
            LaData.CTPhoneData.CTPhoneCallStatus = CTPhoneCallStatus.Invalid;
        }

        private void AxCtPhoneOnDestBusy(object sender, ICTPhone_ActiveXEvents_OnDestBusyEvent ictPhoneActiveXEventsOnDestBusyEvent)
        {
            LaData.CTPhoneData.CTPhoneCallStatus = CTPhoneCallStatus.Busy;
        }

        private void AxCtPhoneOnDestRinging(object sender, ICTPhone_ActiveXEvents_OnDestRingingEvent ictPhoneActiveXEventsOnDestRingingEvent)
        {
            LaData.CTPhoneData.CTPhoneCallStatus = CTPhoneCallStatus.Ringing;
        }

        private void AxCtPhoneOnOpDisconnected(object sender, ICTPhone_ActiveXEvents_OnOpDisconnectedEvent ictPhoneActiveXEventsOnOpDisconnectedEvent)
        {
            LaData.CTPhoneData.IsPhoneOffHook = false;

            LaData.CTPhoneData.IsWorkPhoneActive = false;
            LaData.CTPhoneData.IsHomePhoneActive = false;
            LaData.CTPhoneData.IsCellPhoneActive = false;
            LaData.CTPhoneData.IsOtherPhoneActive = false;
            LaData.CTPhoneData.Is1023PhoneActive = false;
            LaData.CTPhoneData.Is10118PhoneActive = false;

            LaData.CTPhoneData.CTPhoneCallStatus = CTPhoneCallStatus.Disconnected;
            LaData.CTPhoneData.CTPhoneAgentstatus = CTPhoneAgentStatus.Ready;

            _callDuration.Stop();
        }

        private void AxCtPhoneOnOpAnswered(object sender, ICTPhone_ActiveXEvents_OnOpAnsweredEvent ictPhoneActiveXEventsOnOpAnsweredEvent)
        {
            LaData.CTPhoneData.CTPhoneCallStatus = CTPhoneCallStatus.Answered;
            _secCallduration = 0;
            _callDuration.Start();
        }

        private void AxCtPhoneOnRecordingStarted(object sender, ICTPhone_ActiveXEvents_OnRecordingStartedEvent ictPhoneActiveXEventsOnRecordingStartedEvent)
        {
            using (InsureEntities db = new InsureEntities(""))
            {
                Models.CallData callData = new Models.CallData();

                callData.FKImportID = LaData.AppData.ImportID;
                callData.Number = LaData.CTPhoneData.Number;
                callData.Extension = LaData.CTPhoneData.Extension;
                callData.RecRef = LaData.CTPhoneData.RecRef = ictPhoneActiveXEventsOnRecordingStartedEvent.recRef;
                callData.StampUserID = LaData.UserData.UserID;
                callData.StampDate = DateTime.Now;

                db.CallDatas.Add(callData);
                db.SaveChanges();
            }
        }

        private void AxCtPhoneOnRecordingStopped(object sender, ICTPhone_ActiveXEvents_OnRecordingStoppedEvent ictPhoneActiveXEventsOnRecordingStoppedEvent)
        {
            _axCtPhone.UpdateRecording(ictPhoneActiveXEventsOnRecordingStoppedEvent.recRef, LaData.AppData.RefNo, LaData.LeadData.IDNumber, LaData.LeadData.Initials, LaData.LeadData.Surname);
        }

        private void AxCtPhoneOnInboundCall(object sender, ICTPhone_ActiveXEvents_OnInboundCallEvent ictPhoneActiveXEventsOnInboundCallEvent)
        {

        }

        private void AxCtPhoneOnTpAnswered(object sender, ICTPhone_ActiveXEvents_OnTpAnsweredEvent ictPhoneActiveXEventsOnTpAnsweredEvent)
        {

        }

        private void AxCtPhoneOnTpDisconnected(object sender, ICTPhone_ActiveXEvents_OnTpDisconnectedEvent ictPhoneActiveXEventsOnTpDisconnectedEvent)
        {
            if (LaData.CTPhoneData.CTPhoneCallStatus == CTPhoneCallStatus.Held)
            {
                LaData.CTPhoneData.CTPhoneCallStatus = CTPhoneCallStatus.Answered;
            }
            else
            {
                LaData.CTPhoneData.IsPhoneOffHook = false;

                LaData.CTPhoneData.IsWorkPhoneActive = false;
                LaData.CTPhoneData.IsHomePhoneActive = false;
                LaData.CTPhoneData.IsCellPhoneActive = false;
                LaData.CTPhoneData.IsOtherPhoneActive = false;
                LaData.CTPhoneData.Is1023PhoneActive = false;
                LaData.CTPhoneData.Is10118PhoneActive = false;

                LaData.CTPhoneData.CTPhoneCallStatus = CTPhoneCallStatus.Disconnected;
                LaData.CTPhoneData.CTPhoneAgentstatus = CTPhoneAgentStatus.Ready;

                _callDuration.Stop();
            }
        }

        private void AxCtPhoneOnOffhook(object sender, ICTPhone_ActiveXEvents_OnOffhookEvent ictPhoneActiveXEventsOnOffhookEvent)
        {
            LaData.CTPhoneData.IsPhoneOffHook = true;
        }

        private void AxCtPhoneOnAgentAfterCallWork(object sender, ICTPhone_ActiveXEvents_OnAgentAfterCallWorkEvent ictPhoneActiveXEventsOnAgentAfterCallWorkEvent)
        {

        }

        private void AxCtPhoneOnAgentBusy(object sender, ICTPhone_ActiveXEvents_OnAgentBusyEvent ictPhoneActiveXEventsOnAgentBusyEvent)
        {
            LaData.CTPhoneData.CTPhoneAgentstatus = CTPhoneAgentStatus.Busy;
        }

        private void AxCtPhoneOnAgentReady(object sender, ICTPhone_ActiveXEvents_OnAgentReadyEvent ictPhoneActiveXEventsOnAgentReadyEvent)
        {
            LaData.CTPhoneData.CTPhoneAgentstatus = CTPhoneAgentStatus.Ready;
        }

        private void AxCtPhoneOnAgentNotReadyWithReason(object sender, ICTPhone_ActiveXEvents_OnAgentNotReadyWithReasonEvent ictPhoneActiveXEventsOnAgentNotReadyWithReasonEvent)
        {
            LaData.CTPhoneData.CTPhoneAgentstatus = CTPhoneAgentStatus.NotReady;
        }

        private void AxCtPhoneOnAgentLogout(object sender, ICTPhone_ActiveXEvents_OnAgentLogoutEvent ictPhoneActiveXEventsOnAgentLogoutEvent)
        {
            LaData.CTPhoneData.IsAgentLoggedIn = false;
            _callDuration.Stop();
        }

        private void AxCtPhoneOnAgentLogin(object sender, ICTPhone_ActiveXEvents_OnAgentLoginEvent ictPhoneActiveXEventsOnAgentLoginEvent)
        {
            LaData.CTPhoneData.IsAgentLoggedIn = true;
        }

        private void AxCtPhoneOnMonitoringStopped(object sender, EventArgs eventArgs)
        {
            LaData.CTPhoneData.IsAgentLoggedIn = false;
        }

        private void AxCtPhoneOnMonitoringStarted(object sender, EventArgs eventArgs)
        {
            using (InsureEntities db = new InsureEntities(""))
            {
                var ctp =
                (
                    from ctphone in db.CTPhones
                    where ctphone.LoginName.ToLower() == LaData.UserData.LoginName.ToLower()
                    select ctphone
                ).FirstOrDefault();

                if (ctp != null)
                {
                    _axCtPhone.SetAgentStatusLogin(ctp.LoginName, ctp.Password, ctp.Group);
                    _axCtPhone.SetAgentStatusReady();
                }
            }
        }

        private void OpenCTPhone()
        {
            long? userID = LaData.UserData.UserID;
            LaData.CTPhoneData.Extension = string.Empty;

            //using (BlushEntities db = new BlushEntities(""))
            using (InsureEntities db = new InsureEntities(""))
            {
                //var result =
                //(
                //    from hrstaff in db.HRStaffs
                //    join hrstaffextensions in db.HRStaffExtensions on hrstaff.ID equals hrstaffextensions.FKHRStaffID
                //    join lkphrextension in db.lkpHRExtensions on hrstaffextensions.FKHRExtensionID equals lkphrextension.ID
                //    where hrstaff.FKUserID == userID
                //    select lkphrextension.Extension
                //)

                var extension =
                    (
                        from ctphone in db.CTPhones
                        where ctphone.LoginName.ToLower() == LaData.UserData.LoginName.ToLower()
                              && ctphone.IsEnabled == true
                        select ctphone.Extension
                    ).FirstOrDefault();

                if (extension != null)
                {
                    if (!string.IsNullOrWhiteSpace(extension) && extension.Length == 4)
                    {
                        LaData.CTPhoneData.Extension = extension;
                        _axCtPhone.InitDNModeSIP("192.168.0.232", extension, false, "192.168.0.232");
                        //_axCtPhone.InitDNMode("127.0.0.1", extension, false);

                        grdPhoneIcon.ToolTip = LaData.CTPhoneData.Extension;
                    }
                }
                else
                {
                    grdPhoneIcon.ToolTip = "Not Connected!";
                }
            }



            //(from HRStaffs in db.HRStaffs
            // from HRStaffExtensions in db.HRStaffExtensions
            // where
            //   (String)HRStaffs.FKUserID == "506"
            // select new
            // {
            //     HRStaffExtensions.HRStaff1.Extension
            // }).Take(1)

            //StringBuilder strQuery = new StringBuilder();
            //strQuery.Append("USE Blush ");
            //strQuery.Append("SELECT TOP 1 lkpHRExtension.Extension ");
            //strQuery.Append("FROM HRStaff ");
            //strQuery.Append("JOIN HRStaffExtension ON HRStaffExtension.FKHRStaffID = HRStaff.ID ");
            //strQuery.Append("JOIN lkpHRExtension ON lkpHRExtension.ID = HRStaffExtension.FKHRExtensionID ");
            //strQuery.Append("WHERE HRStaff.FKUserID = '" + userID + "'");

            //DataTable dtResult = Methods.GetTableData(strQuery.ToString());
            //if (dtResult.Rows.Count == 1)
            //{
            //    string userExtension = dtResult.Rows[0][0] as string;

            //    if (!string.IsNullOrWhiteSpace(userExtension) && userExtension.Length == 4)
            //    {
            //        _axCtPhone.InitDNMode("192.168.1.198", userExtension, false);
            //    }
            //}
        }

        private void CloseCTPhone()
        {
            _axCtPhone.SetAgentStatusNotReady();
            _axCtPhone.SetAgentStatusLogout();
            _axCtPhone.CloseDNMode();
        }

        private void btnPhone_Click(object sender, RoutedEventArgs e)
        {
            btnPhone phone = sender as btnPhone;
            LaData.CTPhoneData.Number = string.Empty;

            if (phone != null && !LaData.CTPhoneData.IsPhoneOffHook)
            {
                switch (phone.Name)
                {
                    case "btnWorkPhone":
                        LaData.CTPhoneData.Number = medWorkPhone.Text;
                        LaData.CTPhoneData.IsWorkPhoneActive = true;
                        break;

                    case "btnHomePhone":
                        LaData.CTPhoneData.Number = medHomePhone.Text;
                        LaData.CTPhoneData.IsHomePhoneActive = true;
                        break;

                    case "btnCellPhone":
                        LaData.CTPhoneData.Number = medCellPhone.Text;
                        LaData.CTPhoneData.IsCellPhoneActive = true;
                        break;

                    case "btnOtherPhone":
                        LaData.CTPhoneData.Number = medOtherPhone.Text;
                        LaData.CTPhoneData.IsOtherPhoneActive = true;
                        break;
                }

                _axCtPhone.MakeCall(LaData.CTPhoneData.Number);
            }
            else
            {
                _axCtPhone.HangupCall(0);

                if (phone != null)
                {
                    switch (phone.Name)
                    {
                        case "btnWorkPhone":
                            LaData.CTPhoneData.IsWorkPhoneActive = false;
                            break;

                        case "btnHomePhone":
                            LaData.CTPhoneData.IsHomePhoneActive = false;
                            break;

                        case "btnCellPhone":
                            LaData.CTPhoneData.IsCellPhoneActive = false;
                            break;

                        case "btnOtherPhone":
                            LaData.CTPhoneData.IsOtherPhoneActive = false;
                            break;
                    }
                }

                LaData.CTPhoneData.Number = string.Empty;
            }
        }

        private void XamMenuItemPhone_Click(object sender, EventArgs e)
        {
            try
            {
                XamMenuItem xamMenuItem = sender as XamMenuItem;

                if (xamMenuItem?.Header != null)
                {
                    switch (xamMenuItem.Header.ToString())
                    {
                        case "Hold Call":
                            if (LaData.CTPhoneData.IsAgentLoggedIn)
                            {
                                _axCtPhone.HoldCall();
                                LaData.CTPhoneData.CTPhoneCallStatus = CTPhoneCallStatus.Held;
                            }
                            break;

                        case "Retrieve Call":
                            if (LaData.CTPhoneData.IsAgentLoggedIn)
                            {
                                _axCtPhone.RetrieveHeld();
                            }
                            break;

                        case "Hangup Call":
                            if (LaData.CTPhoneData.IsAgentLoggedIn)
                            {
                                if (LaData.CTPhoneData.CTPhoneCallStatus == CTPhoneCallStatus.Held)
                                {
                                    _axCtPhone.RetrieveHeld();
                                    LaData.CTPhoneData.CTPhoneCallStatus = CTPhoneCallStatus.Answered;
                                }
                                _axCtPhone.HangupCall(0);
                                LaData.CTPhoneData.Number = string.Empty;
                            }
                            break;

                        case "1023":
                            LaData.CTPhoneData.Number = string.Empty;

                            if (!LaData.CTPhoneData.IsPhoneOffHook)
                            {
                                LaData.CTPhoneData.Number = "1023";
                                LaData.CTPhoneData.Is1023PhoneActive = true;
                                _axCtPhone.MakeCall(LaData.CTPhoneData.Number);
                            }
                            else
                            {
                                _axCtPhone.HangupCall(0);
                                LaData.CTPhoneData.Is1023PhoneActive = false;
                                LaData.CTPhoneData.Number = string.Empty;
                            }
                            break;

                        case "10118":
                            LaData.CTPhoneData.Number = string.Empty;

                            if (!LaData.CTPhoneData.IsPhoneOffHook)
                            {
                                LaData.CTPhoneData.Number = "10118";
                                LaData.CTPhoneData.Is10118PhoneActive = true;
                                _axCtPhone.MakeCall(LaData.CTPhoneData.Number);
                            }
                            else
                            {
                                _axCtPhone.HangupCall(0);
                                LaData.CTPhoneData.Is10118PhoneActive = false;
                                LaData.CTPhoneData.Number = string.Empty;
                            }
                            break;

                    }
                }

            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void KPBorder_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void KPMenuEventSetter_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void KPButton_PreviewMouseUp(object sender, RoutedEventArgs e)
        {
            if (sender != null && LaData.CTPhoneData.IsAgentLoggedIn && LaData.CTPhoneData.CTPhoneCallStatus == CTPhoneCallStatus.Answered)
            {
                Button btn = (Button)sender;
                _axCtPhone.SendDTMF(btn.Content);
            }
        }

        private void CTPhone_CallDuration(object sender, EventArgs e)
        {
            _secCallduration++;
            Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() => {
                tbCallDuration.Text = TimeSpan.FromSeconds(_secCallduration).ToString(); ;
            }));
        }

        #endregion

        #region Mercantile

        private void btnVerifyAccNum_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LaData.VerifyAccountNumber(cmbDOBankBranchCode.Text, LaData.BankDetailsData.AccountNumber);

                //INMessageBoxWindow1 
                INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                //messageWindow.Opacity = 0.5;
                //messageWindow.txtDescription.Opacity = 1;
                if (LaData.UserData.StaffType == lkpStaffType.Permanent)
                {
                    messageWindow.BGRectangle.Opacity = 0.45;
                }

                //messageWindow.WindowStartupLocation = WindowStartupLocation.Manual;
                //messageWindow.Left = 500;


                ShowMessageBox(messageWindow, "Remember to ask, \"Is the account in the client's name?\"", "Account In Client's Name?", ShowMessageType.Information);
                //if (LaData.AppData.BankingDetails.BankID == 240)
                if (LaData.BankDetailsData.BankID == 240 && LaData.BankDetailsData.AccountTypeID == 3)
                {
                    CheckIfMoneyBuilderAccount();
                }

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        private void scvBasePolicyDetails_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            try
            {
                scvBasePolicyDetails.Margin = scvBasePolicyDetails.ComputedVerticalScrollBarVisibility == Visibility.Visible
                    ? new Thickness(0, 0, -20, 0)
                    : new Thickness(0, 0, 0, 0);
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


        //public void VerifyAccountNumber(string branchCode, string accountNum)
        //{
        //    try
        //    {

        //        var serviceClient = new CdvWCFClient();
        //        //string response = serviceClient.CdvEFT("SOAP", "UDM1", "Password1", "223226", "03000015318");
        //        //string response = serviceClient.CdvEFT("UDM1", "SOAP", "Password1", "259605", "62292826917");
        //        cdvResponse response = serviceClient.checkCDV(mercUsername, mercInstCode, mercPassword, branchCode != null ? branchCode : "", accountNum != null ? accountNum : "");
        //        string passFailString = !string.IsNullOrWhiteSpace(response.cdvResult) && response.cdvResult.Length >= 2
        //        ? response.cdvResult.Substring(0, 2)
        //        : response.cdvResult;

        //        //switch (passFailString)
        //        //{
        //        //    case "00":
        //        //        //pass
        //        //        break;
        //        //    case "04":
        //        //        //branch not found
        //        //        break;
        //        //    case "05":
        //        //        //CDV exception code failed
        //        //        break;
        //        //    case "07":
        //        //        //CDV failure
        //        //        break;
        //        //}
        //        if (passFailString == "00")
        //        {
        //            //Pass
        //            LaData.BankDetailsData.lkpAccNumCheckStatus = lkpINAccNumCheckStatus.Valid;
        //        }
        //        //else if (passFailString.Length != 2)
        //        //{
        //        //    //Error
        //        //}
        //        else
        //        {
        //            //Fail
        //            LaData.BankDetailsData.lkpAccNumCheckStatus = lkpINAccNumCheckStatus.Invalid;
        //        }

        //        //CdvEFTRequest request = new CdvEFTRequest("Test", "EBU1", "hello", "223226", "03000015318");

        //        //CdvEFTResponse response =

        //        //CdvEFTResponse response = CdvEFT(request);
        //        //MessageBox.Show(CdvEFT(request).Result);
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }

        //}

        #endregion

        #region BulkSMS

        private void btnSendSMS_Click(object sender, RoutedEventArgs e)
        {
            SMS smsResponseData = new SMS();
            try
            {
                if (LaData.LeadData.TelCell == ""/*medCellPhone.Text == ""*/)
                {
                    INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                    ShowMessageBox(messageWindow, @"Please fill in the client's cell phone number.", "Cell Phone Number Blank", ShowMessageType.Exclamation);
                }
                else if (LaData.LeadData.Title == ""/*cmbTitle.Text == ""*/)
                {
                    INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                    ShowMessageBox(messageWindow, @"Please fill in the client's title.", "Title Blank", ShowMessageType.Exclamation);
                }
                else if (LaData.LeadData.Surname == ""/*medSurname.Text == ""*/)
                {
                    INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                    ShowMessageBox(messageWindow, @"Please fill in the client's surname.", "Surname Blank", ShowMessageType.Exclamation);
                }
                //else if (cmbAgent.Text == "" || cmbAgent.SelectedValue == null)
                //{
                //    INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                //    ShowMessageBox(messageWindow, @"Please choose an agent from the drop down list.", "Agent Field Blank", ShowMessageType.Exclamation);
                //    //comment out after testing
                //}
                else if (GlobalSettings.ApplicationUser.ID == 0)
                {
                    INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                    ShowMessageBox(messageWindow, @"The logged in user does not have a user ID.", "Blank User ID", ShowMessageType.Exclamation);
                    //comment out after testing
                }
                else if (LaData.AppData.RefNo == ""/*medReference.Text == ""*/)
                {
                    INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                    ShowMessageBox(messageWindow, @"You cannot send an sms with a blank reference number.", "Reference Number Blank", ShowMessageType.Exclamation);
                }
                else
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    //parameters[0] = new SqlParameter("@UserID", cmbAgent.SelectedValue);
                    parameters[0] = new SqlParameter("@UserID", GlobalSettings.ApplicationUser.ID); //added this in case the person is working on a mining campaign
                    //string agentName = Methods.ExecuteFunction("fnGetUserFirstName", parameters).ToString();
                    string agentName = Methods.ExecuteFunction("fnGetUserName", parameters).ToString();
                    //LaData.SMSSendData.to = medCellPhone.Text.Trim();//LaData.LeadData.TelCell;
                    LaData.SMSSendData.to = LaData.LeadData.TelCell.Trim();
                    LaData.SMSSendData.to = "+27" + LaData.SMSSendData.to.Substring(1);

                    //string[] excludedCampaigns = { "PE" };
                    //string[] excludedCampaignNames = { "Revival" };

                    string defaultMessage = $"Hi {LaData.LeadData.Title?.Trim()} {LaData.LeadData.Surname?.Trim()}, Thanks for speaking to our Sales Executive, {agentName}.\nYour ref no is: {LaData.AppData.RefNo?.Trim()}.\nThe Platinum Life Team.\n(This is a no reply message)"; ;

                    if (LaData.AppData.CampaignGroupType == lkpINCampaignGroupType.Base)
                    {
                        if (!LaData.AppData.CampaignCode.EndsWith("PE") && !LaData.AppData.CampaignCode.EndsWith("R")
                           && !(LaData.AppData.CampaignGroup == lkpINCampaignGroup.Defrosted)
                           && !(LaData.AppData.CampaignGroup == lkpINCampaignGroup.ReDefrost)
                           && !(LaData.AppData.CampaignGroup == lkpINCampaignGroup.Resurrection)
                           && !(LaData.AppData.CampaignGroup == lkpINCampaignGroup.Rejuvenation)
                           && !(LaData.AppData.CampaignGroup == lkpINCampaignGroup.Reactivation)
                           && !(LaData.AppData.CampaignGroup == lkpINCampaignGroup.DefrostR99)
                           && !(LaData.AppData.CampaignGroup == lkpINCampaignGroup.Lite)
                           && !(LaData.AppData.CampaignGroup == lkpINCampaignGroup.SpouseLite)

                           )

                        {
                            if (LaData.AppData.CampaignType == lkpINCampaignType.Macc || LaData.AppData.CampaignType == lkpINCampaignType.MaccMillion)
                            {
                                LaData.SMSSendData.body = $"Hi {LaData.LeadData.Title?.Trim()} {LaData.LeadData.Surname?.Trim()}, Thanks for speaking to our Sales Executive, {agentName}.\nWe have activated your R50000 free cover.\nYour ref no is: {LaData.AppData.RefNo?.Trim()}.\nThe Platinum Life Team.\n(This is a no reply message)";
                            }
                            else if (LaData.AppData.CampaignType == lkpINCampaignType.Cancer)
                            {
                                LaData.SMSSendData.body = $"Hi {LaData.LeadData.Title?.Trim()} {LaData.LeadData.Surname?.Trim()}, Thanks for speaking to our Sales Executive, {agentName}.\nWe have activated your R20000 free cover.\nYour ref no is: {LaData.AppData.RefNo?.Trim()}.\nThe Platinum Life Team.\n(This is a no reply message)";
                            }
                            else if (LaData.AppData.CampaignType == lkpINCampaignType.FemaleDis)
                            {
                                LaData.SMSSendData.body = $"Hi {LaData.LeadData.Title?.Trim()} {LaData.LeadData.Surname?.Trim()}, Thanks for speaking to our Sales Executive, {agentName}.\nWe have activated your R20000 free cancer cover and R20000 free accidental cover.\nYour ref no is: {LaData.AppData.RefNo?.Trim()}.\nThe Platinum Life Team.\n(This is a no reply message)";
                            }
                            else
                            {
                                LaData.SMSSendData.body = defaultMessage;
                            }
                        }
                        else
                        {
                            LaData.SMSSendData.body = defaultMessage;
                        }
                    }
                    if (LaData.AppData.CampaignGroupType == lkpINCampaignGroupType.Upgrade)
                    {
                        LaData.SMSSendData.body = defaultMessage;

                    }


                    //if (!excludedCampaigns.Contains(LaData.AppData.CampaignCode))
                    //{
                    //    LaData.SMSSendData.body = $"Hi {LaData.LeadData.Title?.Trim()} {LaData.LeadData.Surname?.Trim()}, Thanks for speaking to our Sales Executive, {agentName}.\nWe have activated your free cover.\nYour ref no is: {LaData.AppData.RefNo?.Trim()}.\nThe Platinum Life Team.\n(This is a no reply message)";
                    //}
                    //else
                    //{
                    //    LaData.SMSSendData.body = $"Hi {LaData.LeadData.Title?.Trim()} {LaData.LeadData.Surname?.Trim()}, Thanks for speaking to our Sales Executive, {agentName}.\nYour ref no is: {LaData.AppData.RefNo?.Trim()}.\nThe Platinum Life Team.\n(This is a no reply message)";
                    //}


                    //LaData.SMSSendData.body = "Testing2";
                    string sms = JsonConvert.SerializeObject(LaData.SMSSendData);
                    //string sms = "{to: \"+27765389999\", body:\"Testing2\"}";
                    var request = WebRequest.Create(bulkSMSURL + "/messages");

                    request.Credentials = new NetworkCredential(bulkSMSUserName, bulkSMSPassword);
                    request.PreAuthenticate = true;
                    request.Method = "POST";
                    request.ContentType = "application/json";

                    #region WriteSMS
                    var encoding = new UnicodeEncoding();
                    var encodedData = encoding.GetBytes(sms);
                    var stream = request.GetRequestStream();
                    stream.Write(encodedData, 0, encodedData.Length);
                    stream.Close();
                    #endregion WriteSMS


                    var response = request.GetResponse();

                    var reader = new StreamReader(response.GetResponseStream());
                    //Console.WriteLine(reader.ReadToEnd());

                    #region SMS Response

                    string smsResponseJson = reader.ReadToEnd();

                    List<LeadApplicationData.SMSResponse> smsResponses = JsonConvert.DeserializeObject<List<LeadApplicationData.SMSResponse>>(smsResponseJson);
                    LeadApplicationData.SMSResponse smsResponse = smsResponses[0];


                    smsResponseData.FKSystemID = (long?)lkpSystem.Insurance;
                    smsResponseData.FKImportID = LaData.AppData.ImportID;

                    smsResponseData.SMSID = smsResponse.id;
                    smsResponseData.FKlkpSMSTypeID = (long?)smsResponse.type;
                    smsResponseData.RecipientCellNum = smsResponse.to;
                    smsResponseData.SMSBody = smsResponse.body;
                    smsResponseData.FKlkpSMSEncodingID = (long?)smsResponse.encoding;
                    smsResponseData.SubmissionID = smsResponse.submission.id;
                    smsResponseData.SubmissionDate = smsResponse.submission.date.AddHours(2); //for some reason the time given by bulk sms is 2 hours behind that's why I add 2 hours
                    smsResponseData.FKlkpSMSStatusTypeID = (long?)smsResponse.status.type;
                    smsResponseData.FKlkpSMSStatusSubtypeID = (long?)smsResponse.status.subtype;

                    smsResponseData.Save(_validationResult);

                    LaData.SMSData.SMSStatusTypeID = (WPF.Enumerations.Insure.lkpSMSStatusType?)smsResponseData.FKlkpSMSStatusTypeID;
                    LaData.SMSData.SMSStatusSubtypeID = (WPF.Enumerations.Insure.lkpSMSStatusSubtype?)smsResponseData.FKlkpSMSStatusSubtypeID;
                    LaData.SMSData.SMSSubmissionDate = Convert.ToDateTime(smsResponseData.SubmissionDate);//DateTime.ParseExact(dtSMS.Rows[0]["SMSSubmissionDate"].ToString(), "yyyy-MM-ddThh:mm:ssZ", new CultureInfo("en-ZA"));

                    CheckSMSSent();


                    #endregion SMS Response



                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }



        #endregion BulkSMS

        #region CardLayout

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Page2.Visibility = Visibility.Hidden;

            //CardLayoutPage.IsEnabled = true;
            CardLayoutPage.Visibility = Visibility.Visible;
            loadCard();
        }

        private void ReturnToPage2()
        {
            Page2.Visibility = Visibility.Visible;


            CardLayoutPage.Visibility = Visibility.Hidden;
        }

        private void loadCard()
        {
            List<string> CountCoverList = new List<string>();
            CountCoverList.Clear();

            List<int> CountIDList = new List<int>();
            CountIDList.Clear();

            foreach (var item in dtCover.AsEnumerable())
            {
                DataRow drv = (DataRow)item;
                String valueOfItem = drv["Description"].ToString();
                int valueOfID = int.Parse(drv["Value"].ToString());
                CountCoverList.Add(valueOfItem);
                CountIDList.Add(valueOfID);
            }

            int countOptions = CountCoverList.Count();

            ActivateCards(countOptions, CountCoverList, CountIDList);
        }

        private void CorrectImageBtn(List<string> CountCoverList, int CountOptions)
        {
            ImageURLString.Clear();
            int x = 0;
            foreach (var item in CountCoverList)
            {
                if (item.Contains("Cancer"))
                {
                    ImageURLString.Add(new Uri(@"../Resources/CancerRibbon.png", UriKind.Relative));
                }
                else if (item.Contains("Acc Death"))
                {
                    ImageURLString.Add(new Uri(@"../Resources/carAccident.png", UriKind.Relative));
                }
                else if (item.Contains("Acc Dis"))
                {
                    ImageURLString.Add(new Uri(@"../Resources/wheelchair.png", UriKind.Relative));
                }
                else if (item.Contains("Acc Funeral"))
                {
                    ImageURLString.Add(new Uri(@"../Resources/Death.png", UriKind.Relative));
                }
                else
                {
                    ImageURLString.Add(new Uri(@"../Resources/CancerRibbon.png", UriKind.Relative));
                }
                x++;
            }
        }

        private void ActivateCards(int CountOptions, List<string> CountCoverList, List<int> CountIDList)
        {
            CorrectImageBtn(CountCoverList, CountOptions);

            if (CountOptions == 1)
            {
                Card1.Visibility = Visibility.Visible;
                Card1TB.Text = CountCoverList[0];
                card1ID = CountIDList[0];
                CardImage1.Source = new BitmapImage(ImageURLString[0]);
                if (DefaultOptionIDCards == card1ID)
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card2.Visibility = Visibility.Hidden;
                Card3.Visibility = Visibility.Hidden;
                Card4.Visibility = Visibility.Hidden;
                Card5.Visibility = Visibility.Hidden;
                Card6.Visibility = Visibility.Hidden;
                Card7.Visibility = Visibility.Hidden;
                Card8.Visibility = Visibility.Hidden;
                Card9.Visibility = Visibility.Hidden;
                Card10.Visibility = Visibility.Hidden;
                Card11.Visibility = Visibility.Hidden;
                Card12.Visibility = Visibility.Hidden;
                Card13.Visibility = Visibility.Hidden;
                Card14.Visibility = Visibility.Hidden;
                Card15.Visibility = Visibility.Hidden;
                Card16.Visibility = Visibility.Hidden;


            }
            else if (CountOptions == 2)
            {
                Card1.Visibility = Visibility.Visible;
                Card1TB.Text = CountCoverList[0];
                card1ID = CountIDList[0];
                CardImage1.Source = new BitmapImage(ImageURLString[0]);
                if (DefaultOptionIDCards == card1ID)
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card2.Visibility = Visibility.Visible;
                Card2TB.Text = CountCoverList[1];
                card2ID = CountIDList[1];
                CardImage2.Source = new BitmapImage(ImageURLString[1]);
                if (DefaultOptionIDCards == card2ID)
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card3.Visibility = Visibility.Hidden;
                Card4.Visibility = Visibility.Hidden;
                Card5.Visibility = Visibility.Hidden;
                Card6.Visibility = Visibility.Hidden;
                Card7.Visibility = Visibility.Hidden;
                Card8.Visibility = Visibility.Hidden;
                Card9.Visibility = Visibility.Hidden;
                Card10.Visibility = Visibility.Hidden;
                Card11.Visibility = Visibility.Hidden;
                Card12.Visibility = Visibility.Hidden;
                Card13.Visibility = Visibility.Hidden;
                Card14.Visibility = Visibility.Hidden;
                Card15.Visibility = Visibility.Hidden;
                Card16.Visibility = Visibility.Hidden;

            }
            else if (CountOptions == 3)
            {
                Card1.Visibility = Visibility.Visible;
                Card1TB.Text = CountCoverList[0];
                card1ID = CountIDList[0];
                CardImage1.Source = new BitmapImage(ImageURLString[0]);
                if (DefaultOptionIDCards == card1ID)
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card2.Visibility = Visibility.Visible;
                Card2TB.Text = CountCoverList[1];
                card2ID = CountIDList[1];
                CardImage2.Source = new BitmapImage(ImageURLString[1]);
                if (DefaultOptionIDCards == card2ID)
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card3.Visibility = Visibility.Visible;
                Card3TB.Text = CountCoverList[2];
                card3ID = CountIDList[2];
                CardImage3.Source = new BitmapImage(ImageURLString[2]);
                if (DefaultOptionIDCards == card3ID)
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card4.Visibility = Visibility.Hidden;
                Card5.Visibility = Visibility.Hidden;
                Card6.Visibility = Visibility.Hidden;
                Card7.Visibility = Visibility.Hidden;
                Card8.Visibility = Visibility.Hidden;
                Card9.Visibility = Visibility.Hidden;
                Card10.Visibility = Visibility.Hidden;
                Card11.Visibility = Visibility.Hidden;
                Card12.Visibility = Visibility.Hidden;
                Card13.Visibility = Visibility.Hidden;
                Card14.Visibility = Visibility.Hidden;
                Card15.Visibility = Visibility.Hidden;
                Card16.Visibility = Visibility.Hidden;

            }
            else if (CountOptions == 4)
            {
                Card1.Visibility = Visibility.Visible;
                Card1TB.Text = CountCoverList[0];
                card1ID = CountIDList[0];
                CardImage1.Source = new BitmapImage(ImageURLString[0]);
                if (DefaultOptionIDCards == card1ID)
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card2.Visibility = Visibility.Visible;
                Card2TB.Text = CountCoverList[1];
                card2ID = CountIDList[1];
                CardImage2.Source = new BitmapImage(ImageURLString[1]);
                if (DefaultOptionIDCards == card2ID)
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card3.Visibility = Visibility.Visible;
                Card3TB.Text = CountCoverList[2];
                card3ID = CountIDList[2];
                CardImage3.Source = new BitmapImage(ImageURLString[2]);
                if (DefaultOptionIDCards == card3ID)
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card4.Visibility = Visibility.Visible;
                Card4TB.Text = CountCoverList[3];
                card4ID = CountIDList[3];
                CardImage4.Source = new BitmapImage(ImageURLString[3]);
                if (DefaultOptionIDCards == card4ID)
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card5.Visibility = Visibility.Hidden;
                Card6.Visibility = Visibility.Hidden;
                Card7.Visibility = Visibility.Hidden;
                Card8.Visibility = Visibility.Hidden;
                Card9.Visibility = Visibility.Hidden;
                Card10.Visibility = Visibility.Hidden;
                Card11.Visibility = Visibility.Hidden;
                Card12.Visibility = Visibility.Hidden;
                Card13.Visibility = Visibility.Hidden;
                Card14.Visibility = Visibility.Hidden;
                Card15.Visibility = Visibility.Hidden;
                Card16.Visibility = Visibility.Hidden;

            }
            else if (CountOptions == 5)
            {
                Card1.Visibility = Visibility.Visible;
                Card1TB.Text = CountCoverList[0];
                card1ID = CountIDList[0];
                CardImage1.Source = new BitmapImage(ImageURLString[0]);
                if (DefaultOptionIDCards == card1ID)
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card2.Visibility = Visibility.Visible;
                Card2TB.Text = CountCoverList[1];
                card2ID = CountIDList[1];
                CardImage2.Source = new BitmapImage(ImageURLString[1]);
                if (DefaultOptionIDCards == card2ID)
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card3.Visibility = Visibility.Visible;
                Card3TB.Text = CountCoverList[2];
                card3ID = CountIDList[2];
                CardImage3.Source = new BitmapImage(ImageURLString[2]);
                if (DefaultOptionIDCards == card3ID)
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card4.Visibility = Visibility.Visible;
                Card4TB.Text = CountCoverList[3];
                card4ID = CountIDList[3];
                CardImage4.Source = new BitmapImage(ImageURLString[3]);
                if (DefaultOptionIDCards == card4ID)
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card5.Visibility = Visibility.Visible;
                Card5TB.Text = CountCoverList[4];
                card5ID = CountIDList[4];
                CardImage5.Source = new BitmapImage(ImageURLString[4]);
                if (DefaultOptionIDCards == card5ID)
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card6.Visibility = Visibility.Hidden;
                Card7.Visibility = Visibility.Hidden;
                Card8.Visibility = Visibility.Hidden;
                Card9.Visibility = Visibility.Hidden;
                Card10.Visibility = Visibility.Hidden;
                Card11.Visibility = Visibility.Hidden;
                Card12.Visibility = Visibility.Hidden;
                Card13.Visibility = Visibility.Hidden;
                Card14.Visibility = Visibility.Hidden;
                Card15.Visibility = Visibility.Hidden;
                Card16.Visibility = Visibility.Hidden;

            }
            else if (CountOptions == 6)
            {
                Card1.Visibility = Visibility.Visible;
                Card1TB.Text = CountCoverList[0];
                card1ID = CountIDList[0];
                CardImage1.Source = new BitmapImage(ImageURLString[0]);
                if (DefaultOptionIDCards == card1ID)
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card2.Visibility = Visibility.Visible;
                Card2TB.Text = CountCoverList[1];
                card2ID = CountIDList[1];
                CardImage2.Source = new BitmapImage(ImageURLString[1]);
                if (DefaultOptionIDCards == card2ID)
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card3.Visibility = Visibility.Visible;
                Card3TB.Text = CountCoverList[2];
                card3ID = CountIDList[2];
                CardImage3.Source = new BitmapImage(ImageURLString[2]);
                if (DefaultOptionIDCards == card3ID)
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card4.Visibility = Visibility.Visible;
                Card4TB.Text = CountCoverList[3];
                card4ID = CountIDList[3];
                CardImage4.Source = new BitmapImage(ImageURLString[3]);
                if (DefaultOptionIDCards == card4ID)
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card5.Visibility = Visibility.Visible;
                Card5TB.Text = CountCoverList[4];
                card5ID = CountIDList[4];
                CardImage5.Source = new BitmapImage(ImageURLString[4]);
                if (DefaultOptionIDCards == card5ID)
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card6.Visibility = Visibility.Visible;
                Card6TB.Text = CountCoverList[5];
                card6ID = CountIDList[5];
                CardImage6.Source = new BitmapImage(ImageURLString[5]);
                if (DefaultOptionIDCards == card6ID)
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card7.Visibility = Visibility.Hidden;
                Card8.Visibility = Visibility.Hidden;
                Card9.Visibility = Visibility.Hidden;
                Card10.Visibility = Visibility.Hidden;
                Card11.Visibility = Visibility.Hidden;
                Card12.Visibility = Visibility.Hidden;
                Card13.Visibility = Visibility.Hidden;
                Card14.Visibility = Visibility.Hidden;
                Card15.Visibility = Visibility.Hidden;
                Card16.Visibility = Visibility.Hidden;

            }
            else if (CountOptions == 7)
            {
                Card1.Visibility = Visibility.Visible;
                Card1TB.Text = CountCoverList[0];
                card1ID = CountIDList[0];
                CardImage1.Source = new BitmapImage(ImageURLString[0]);
                if (DefaultOptionIDCards == card1ID)
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card2.Visibility = Visibility.Visible;
                Card2TB.Text = CountCoverList[1];
                card2ID = CountIDList[1];
                CardImage2.Source = new BitmapImage(ImageURLString[1]);
                if (DefaultOptionIDCards == card2ID)
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card3.Visibility = Visibility.Visible;
                Card3TB.Text = CountCoverList[2];
                card3ID = CountIDList[2];
                CardImage3.Source = new BitmapImage(ImageURLString[2]);
                if (DefaultOptionIDCards == card3ID)
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card4.Visibility = Visibility.Visible;
                Card4TB.Text = CountCoverList[3];
                card4ID = CountIDList[3];
                CardImage4.Source = new BitmapImage(ImageURLString[3]);
                if (DefaultOptionIDCards == card4ID)
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card5.Visibility = Visibility.Visible;
                Card5TB.Text = CountCoverList[4];
                card5ID = CountIDList[4];
                CardImage5.Source = new BitmapImage(ImageURLString[4]);
                if (DefaultOptionIDCards == card5ID)
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card6.Visibility = Visibility.Visible;
                Card6TB.Text = CountCoverList[5];
                card6ID = CountIDList[5];
                CardImage6.Source = new BitmapImage(ImageURLString[5]);
                if (DefaultOptionIDCards == card6ID)
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card7.Visibility = Visibility.Visible;
                Card7TB.Text = CountCoverList[6];
                card7ID = CountIDList[6];
                CardImage7.Source = new BitmapImage(ImageURLString[6]);
                if (DefaultOptionIDCards == card7ID)
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card8.Visibility = Visibility.Hidden;
                Card9.Visibility = Visibility.Hidden;
                Card10.Visibility = Visibility.Hidden;
                Card11.Visibility = Visibility.Hidden;
                Card12.Visibility = Visibility.Hidden;
                Card13.Visibility = Visibility.Hidden;
                Card14.Visibility = Visibility.Hidden;
                Card15.Visibility = Visibility.Hidden;
                Card16.Visibility = Visibility.Hidden;

            }
            else if (CountOptions == 8)
            {
                Card1.Visibility = Visibility.Visible;
                Card1TB.Text = CountCoverList[0];
                card1ID = CountIDList[0];
                CardImage1.Source = new BitmapImage(ImageURLString[0]);
                if (DefaultOptionIDCards == card1ID)
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card2.Visibility = Visibility.Visible;
                Card2TB.Text = CountCoverList[1];
                card2ID = CountIDList[1];
                CardImage2.Source = new BitmapImage(ImageURLString[1]);
                if (DefaultOptionIDCards == card2ID)
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card3.Visibility = Visibility.Visible;
                Card3TB.Text = CountCoverList[2];
                card3ID = CountIDList[2];
                CardImage3.Source = new BitmapImage(ImageURLString[2]);
                if (DefaultOptionIDCards == card3ID)
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card4.Visibility = Visibility.Visible;
                Card4TB.Text = CountCoverList[3];
                card4ID = CountIDList[3];
                CardImage4.Source = new BitmapImage(ImageURLString[3]);
                if (DefaultOptionIDCards == card4ID)
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card5.Visibility = Visibility.Visible;
                Card5TB.Text = CountCoverList[4];
                card5ID = CountIDList[4];
                CardImage5.Source = new BitmapImage(ImageURLString[4]);
                if (DefaultOptionIDCards == card5ID)
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card6.Visibility = Visibility.Visible;
                Card6TB.Text = CountCoverList[5];
                card6ID = CountIDList[5];
                CardImage6.Source = new BitmapImage(ImageURLString[5]);
                if (DefaultOptionIDCards == card6ID)
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card7.Visibility = Visibility.Visible;
                Card7TB.Text = CountCoverList[6];
                card7ID = CountIDList[6];
                CardImage7.Source = new BitmapImage(ImageURLString[6]);
                if (DefaultOptionIDCards == card7ID)
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card8.Visibility = Visibility.Visible;
                Card8TB.Text = CountCoverList[7];
                card8ID = CountIDList[7];
                CardImage8.Source = new BitmapImage(ImageURLString[7]);
                if (DefaultOptionIDCards == card8ID)
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card9.Visibility = Visibility.Hidden;
                Card10.Visibility = Visibility.Hidden;
                Card11.Visibility = Visibility.Hidden;
                Card12.Visibility = Visibility.Hidden;
                Card13.Visibility = Visibility.Hidden;
                Card14.Visibility = Visibility.Hidden;
                Card15.Visibility = Visibility.Hidden;
                Card16.Visibility = Visibility.Hidden;

            }
            else if (CountOptions == 9)
            {
                Card1.Visibility = Visibility.Visible;
                Card1TB.Text = CountCoverList[0];
                card1ID = CountIDList[0];
                CardImage1.Source = new BitmapImage(ImageURLString[0]);
                if (DefaultOptionIDCards == card1ID)
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card2.Visibility = Visibility.Visible;
                Card2TB.Text = CountCoverList[1];
                card2ID = CountIDList[1];
                CardImage2.Source = new BitmapImage(ImageURLString[1]);
                if (DefaultOptionIDCards == card2ID)
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card3.Visibility = Visibility.Visible;
                Card3TB.Text = CountCoverList[2];
                card3ID = CountIDList[2];
                CardImage3.Source = new BitmapImage(ImageURLString[2]);
                if (DefaultOptionIDCards == card3ID)
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card4.Visibility = Visibility.Visible;
                Card4TB.Text = CountCoverList[3];
                card4ID = CountIDList[3];
                CardImage4.Source = new BitmapImage(ImageURLString[3]);
                if (DefaultOptionIDCards == card4ID)
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card5.Visibility = Visibility.Visible;
                Card5TB.Text = CountCoverList[4];
                card5ID = CountIDList[4];
                CardImage5.Source = new BitmapImage(ImageURLString[4]);
                if (DefaultOptionIDCards == card5ID)
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card6.Visibility = Visibility.Visible;
                Card6TB.Text = CountCoverList[5];
                card6ID = CountIDList[5];
                CardImage6.Source = new BitmapImage(ImageURLString[5]);
                if (DefaultOptionIDCards == card6ID)
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card7.Visibility = Visibility.Visible;
                Card7TB.Text = CountCoverList[6];
                card7ID = CountIDList[6];
                CardImage7.Source = new BitmapImage(ImageURLString[6]);
                if (DefaultOptionIDCards == card7ID)
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card8.Visibility = Visibility.Visible;
                Card8TB.Text = CountCoverList[7];
                card8ID = CountIDList[7];
                CardImage8.Source = new BitmapImage(ImageURLString[7]);
                if (DefaultOptionIDCards == card8ID)
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card9.Visibility = Visibility.Visible;
                Card9TB.Text = CountCoverList[8];
                card9ID = CountIDList[8];
                CardImage9.Source = new BitmapImage(ImageURLString[8]);
                if (DefaultOptionIDCards == card9ID)
                {
                    Card9.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card9.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card10.Visibility = Visibility.Hidden;
                Card11.Visibility = Visibility.Hidden;
                Card12.Visibility = Visibility.Hidden;
                Card13.Visibility = Visibility.Hidden;
                Card14.Visibility = Visibility.Hidden;
                Card15.Visibility = Visibility.Hidden;
                Card16.Visibility = Visibility.Hidden;

            }
            else if (CountOptions == 10)
            {
                Card1.Visibility = Visibility.Visible;
                Card1TB.Text = CountCoverList[0];
                card1ID = CountIDList[0];
                CardImage1.Source = new BitmapImage(ImageURLString[0]);
                if (DefaultOptionIDCards == card1ID)
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card2.Visibility = Visibility.Visible;
                Card2TB.Text = CountCoverList[1];
                card2ID = CountIDList[1];
                CardImage2.Source = new BitmapImage(ImageURLString[1]);
                if (DefaultOptionIDCards == card2ID)
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card3.Visibility = Visibility.Visible;
                Card3TB.Text = CountCoverList[2];
                card3ID = CountIDList[2];
                CardImage3.Source = new BitmapImage(ImageURLString[2]);
                if (DefaultOptionIDCards == card3ID)
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card4.Visibility = Visibility.Visible;
                Card4TB.Text = CountCoverList[3];
                card4ID = CountIDList[3];
                CardImage4.Source = new BitmapImage(ImageURLString[3]);
                if (DefaultOptionIDCards == card4ID)
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card5.Visibility = Visibility.Visible;
                Card5TB.Text = CountCoverList[4];
                card5ID = CountIDList[4];
                CardImage5.Source = new BitmapImage(ImageURLString[4]);
                if (DefaultOptionIDCards == card5ID)
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card6.Visibility = Visibility.Visible;
                Card6TB.Text = CountCoverList[5];
                card6ID = CountIDList[5];
                CardImage6.Source = new BitmapImage(ImageURLString[5]);
                if (DefaultOptionIDCards == card6ID)
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card7.Visibility = Visibility.Visible;
                Card7TB.Text = CountCoverList[6];
                card7ID = CountIDList[6];
                CardImage7.Source = new BitmapImage(ImageURLString[6]);
                if (DefaultOptionIDCards == card7ID)
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card8.Visibility = Visibility.Visible;
                Card8TB.Text = CountCoverList[7];
                card8ID = CountIDList[7];
                CardImage8.Source = new BitmapImage(ImageURLString[7]);
                if (DefaultOptionIDCards == card8ID)
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card9.Visibility = Visibility.Visible;
                Card9TB.Text = CountCoverList[8];
                card9ID = CountIDList[8];
                CardImage9.Source = new BitmapImage(ImageURLString[8]);
                if (DefaultOptionIDCards == card9ID)
                {
                    Card9.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card9.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card10.Visibility = Visibility.Visible;
                Card10TB.Text = CountCoverList[9];
                card10ID = CountIDList[9];
                CardImage10.Source = new BitmapImage(ImageURLString[9]);
                if (DefaultOptionIDCards == card10ID)
                {
                    Card10.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card10.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card11.Visibility = Visibility.Hidden;
                Card12.Visibility = Visibility.Hidden;
                Card13.Visibility = Visibility.Hidden;
                Card14.Visibility = Visibility.Hidden;
                Card15.Visibility = Visibility.Hidden;
                Card16.Visibility = Visibility.Hidden;

            }
            else if (CountOptions == 11)
            {
                Card1.Visibility = Visibility.Visible;
                Card1TB.Text = CountCoverList[0];
                card1ID = CountIDList[0];
                CardImage1.Source = new BitmapImage(ImageURLString[0]);
                if (DefaultOptionIDCards == card1ID)
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card2.Visibility = Visibility.Visible;
                Card2TB.Text = CountCoverList[1];
                card2ID = CountIDList[1];
                CardImage2.Source = new BitmapImage(ImageURLString[1]);
                if (DefaultOptionIDCards == card2ID)
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card3.Visibility = Visibility.Visible;
                Card3TB.Text = CountCoverList[2];
                card3ID = CountIDList[2];
                CardImage3.Source = new BitmapImage(ImageURLString[2]);
                if (DefaultOptionIDCards == card3ID)
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card4.Visibility = Visibility.Visible;
                Card4TB.Text = CountCoverList[3];
                card4ID = CountIDList[3];
                CardImage4.Source = new BitmapImage(ImageURLString[3]);
                if (DefaultOptionIDCards == card4ID)
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card5.Visibility = Visibility.Visible;
                Card5TB.Text = CountCoverList[4];
                card5ID = CountIDList[4];
                CardImage5.Source = new BitmapImage(ImageURLString[4]);
                if (DefaultOptionIDCards == card5ID)
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card6.Visibility = Visibility.Visible;
                Card6TB.Text = CountCoverList[5];
                card6ID = CountIDList[5];
                CardImage6.Source = new BitmapImage(ImageURLString[5]);
                if (DefaultOptionIDCards == card6ID)
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card7.Visibility = Visibility.Visible;
                Card7TB.Text = CountCoverList[6];
                card7ID = CountIDList[6];
                CardImage7.Source = new BitmapImage(ImageURLString[6]);
                if (DefaultOptionIDCards == card7ID)
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card8.Visibility = Visibility.Visible;
                Card8TB.Text = CountCoverList[7];
                card8ID = CountIDList[7];
                CardImage8.Source = new BitmapImage(ImageURLString[7]);
                if (DefaultOptionIDCards == card8ID)
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card9.Visibility = Visibility.Visible;
                Card9TB.Text = CountCoverList[8];
                card9ID = CountIDList[8];
                CardImage9.Source = new BitmapImage(ImageURLString[8]);
                if (DefaultOptionIDCards == card9ID)
                {
                    Card9.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card9.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card10.Visibility = Visibility.Visible;
                Card10TB.Text = CountCoverList[9];
                card10ID = CountIDList[9];
                CardImage10.Source = new BitmapImage(ImageURLString[9]);
                if (DefaultOptionIDCards == card10ID)
                {
                    Card10.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card10.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card11.Visibility = Visibility.Visible;
                Card11TB.Text = CountCoverList[10];
                card11ID = CountIDList[10];
                CardImage1.Source = new BitmapImage(ImageURLString[10]);
                if (DefaultOptionIDCards == card11ID)
                {
                    Card11.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card11.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card12.Visibility = Visibility.Hidden;
                Card13.Visibility = Visibility.Hidden;
                Card14.Visibility = Visibility.Hidden;
                Card15.Visibility = Visibility.Hidden;
                Card16.Visibility = Visibility.Hidden;

            }
            else if (CountOptions == 12)
            {
                Card1.Visibility = Visibility.Visible;
                Card1TB.Text = CountCoverList[0];
                card1ID = CountIDList[0];
                CardImage1.Source = new BitmapImage(ImageURLString[0]);
                if (DefaultOptionIDCards == card1ID)
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card2.Visibility = Visibility.Visible;
                Card2TB.Text = CountCoverList[1];
                card2ID = CountIDList[1];
                CardImage2.Source = new BitmapImage(ImageURLString[1]);
                if (DefaultOptionIDCards == card2ID)
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card3.Visibility = Visibility.Visible;
                Card3TB.Text = CountCoverList[2];
                card3ID = CountIDList[2];
                CardImage3.Source = new BitmapImage(ImageURLString[2]);
                if (DefaultOptionIDCards == card3ID)
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card4.Visibility = Visibility.Visible;
                Card4TB.Text = CountCoverList[3];
                card4ID = CountIDList[3];
                CardImage4.Source = new BitmapImage(ImageURLString[3]);
                if (DefaultOptionIDCards == card4ID)
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card5.Visibility = Visibility.Visible;
                Card5TB.Text = CountCoverList[4];
                card5ID = CountIDList[4];
                CardImage5.Source = new BitmapImage(ImageURLString[4]);
                if (DefaultOptionIDCards == card5ID)
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card6.Visibility = Visibility.Visible;
                Card6TB.Text = CountCoverList[5];
                card6ID = CountIDList[5];
                CardImage6.Source = new BitmapImage(ImageURLString[5]);
                if (DefaultOptionIDCards == card6ID)
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card7.Visibility = Visibility.Visible;
                Card7TB.Text = CountCoverList[6];
                card7ID = CountIDList[6];
                CardImage7.Source = new BitmapImage(ImageURLString[6]);
                if (DefaultOptionIDCards == card7ID)
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card8.Visibility = Visibility.Visible;
                Card8TB.Text = CountCoverList[7];
                card8ID = CountIDList[7];
                CardImage8.Source = new BitmapImage(ImageURLString[7]);
                if (DefaultOptionIDCards == card8ID)
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card9.Visibility = Visibility.Visible;
                Card9TB.Text = CountCoverList[8];
                card9ID = CountIDList[8];
                CardImage9.Source = new BitmapImage(ImageURLString[8]);
                if (DefaultOptionIDCards == card9ID)
                {
                    Card9.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card9.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card10.Visibility = Visibility.Visible;
                Card10TB.Text = CountCoverList[9];
                card10ID = CountIDList[9];
                CardImage10.Source = new BitmapImage(ImageURLString[9]);
                if (DefaultOptionIDCards == card10ID)
                {
                    Card10.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card10.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card11TB.Text = CountCoverList[10];
                card11ID = CountIDList[10];
                Card11.Visibility = Visibility.Visible;
                CardImage11.Source = new BitmapImage(ImageURLString[10]);
                if (DefaultOptionIDCards == card11ID)
                {
                    Card11.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card11.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card12.Visibility = Visibility.Visible;
                Card12TB.Text = CountCoverList[11];
                card12ID = CountIDList[11];
                CardImage12.Source = new BitmapImage(ImageURLString[11]);
                if (DefaultOptionIDCards == card12ID)
                {
                    Card12.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card12.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card13.Visibility = Visibility.Hidden;
                Card14.Visibility = Visibility.Hidden;
                Card15.Visibility = Visibility.Hidden;
                Card16.Visibility = Visibility.Hidden;

            }
            else if (CountOptions == 13)
            {
                Card1.Visibility = Visibility.Visible;
                Card1TB.Text = CountCoverList[0];
                card1ID = CountIDList[0];
                CardImage1.Source = new BitmapImage(ImageURLString[0]);
                if (DefaultOptionIDCards == card1ID)
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card2.Visibility = Visibility.Visible;
                Card2TB.Text = CountCoverList[1];
                card2ID = CountIDList[1];
                CardImage2.Source = new BitmapImage(ImageURLString[1]);
                if (DefaultOptionIDCards == card2ID)
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card3.Visibility = Visibility.Visible;
                Card3TB.Text = CountCoverList[2];
                card3ID = CountIDList[2];
                CardImage3.Source = new BitmapImage(ImageURLString[2]);
                if (DefaultOptionIDCards == card3ID)
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card4.Visibility = Visibility.Visible;
                Card4TB.Text = CountCoverList[3];
                card4ID = CountIDList[3];
                CardImage4.Source = new BitmapImage(ImageURLString[3]);
                if (DefaultOptionIDCards == card4ID)
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card5.Visibility = Visibility.Visible;
                Card5TB.Text = CountCoverList[4];
                card5ID = CountIDList[4];
                CardImage5.Source = new BitmapImage(ImageURLString[4]);
                if (DefaultOptionIDCards == card5ID)
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card6.Visibility = Visibility.Visible;
                Card6TB.Text = CountCoverList[5];
                card6ID = CountIDList[5];
                CardImage6.Source = new BitmapImage(ImageURLString[5]);
                if (DefaultOptionIDCards == card6ID)
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card7.Visibility = Visibility.Visible;
                Card7TB.Text = CountCoverList[6];
                card7ID = CountIDList[6];
                CardImage7.Source = new BitmapImage(ImageURLString[6]);
                if (DefaultOptionIDCards == card7ID)
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card8.Visibility = Visibility.Visible;
                Card8TB.Text = CountCoverList[7];
                card8ID = CountIDList[7];
                CardImage8.Source = new BitmapImage(ImageURLString[7]);
                if (DefaultOptionIDCards == card8ID)
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card9.Visibility = Visibility.Visible;
                Card9TB.Text = CountCoverList[8];
                card9ID = CountIDList[8];
                CardImage9.Source = new BitmapImage(ImageURLString[8]);
                if (DefaultOptionIDCards == card9ID)
                {
                    Card9.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card9.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card10.Visibility = Visibility.Visible;
                Card10TB.Text = CountCoverList[9];
                card10ID = CountIDList[9];
                CardImage10.Source = new BitmapImage(ImageURLString[9]);
                if (DefaultOptionIDCards == card10ID)
                {
                    Card10.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card10.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card11TB.Text = CountCoverList[10];
                card11ID = CountIDList[10];
                Card11.Visibility = Visibility.Visible;
                CardImage11.Source = new BitmapImage(ImageURLString[10]);
                if (DefaultOptionIDCards == card11ID)
                {
                    Card11.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card11.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card12.Visibility = Visibility.Visible;
                Card12TB.Text = CountCoverList[11];
                card12ID = CountIDList[11];
                CardImage12.Source = new BitmapImage(ImageURLString[11]);
                if (DefaultOptionIDCards == card12ID)
                {
                    Card12.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card12.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card13.Visibility = Visibility.Visible;
                Card13TB.Text = CountCoverList[12];
                card13ID = CountIDList[12];
                CardImage13.Source = new BitmapImage(ImageURLString[12]);
                if (DefaultOptionIDCards == card13ID)
                {
                    Card13.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card13.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card14.Visibility = Visibility.Hidden;
                Card15.Visibility = Visibility.Hidden;
                Card16.Visibility = Visibility.Hidden;

            }
            else if (CountOptions == 14)
            {
                Card1.Visibility = Visibility.Visible;
                Card1TB.Text = CountCoverList[0];
                card1ID = CountIDList[0];
                CardImage1.Source = new BitmapImage(ImageURLString[0]);
                if (DefaultOptionIDCards == card1ID)
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card2.Visibility = Visibility.Visible;
                Card2TB.Text = CountCoverList[1];
                card2ID = CountIDList[1];
                CardImage2.Source = new BitmapImage(ImageURLString[1]);
                if (DefaultOptionIDCards == card2ID)
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card3.Visibility = Visibility.Visible;
                Card3TB.Text = CountCoverList[2];
                card3ID = CountIDList[2];
                CardImage3.Source = new BitmapImage(ImageURLString[2]);
                if (DefaultOptionIDCards == card3ID)
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card4.Visibility = Visibility.Visible;
                Card4TB.Text = CountCoverList[3];
                card4ID = CountIDList[3];
                CardImage4.Source = new BitmapImage(ImageURLString[3]);
                if (DefaultOptionIDCards == card4ID)
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card5.Visibility = Visibility.Visible;
                Card5TB.Text = CountCoverList[4];
                card5ID = CountIDList[4];
                CardImage5.Source = new BitmapImage(ImageURLString[4]);
                if (DefaultOptionIDCards == card5ID)
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card6.Visibility = Visibility.Visible;
                Card6TB.Text = CountCoverList[5];
                card6ID = CountIDList[5];
                CardImage6.Source = new BitmapImage(ImageURLString[5]);
                if (DefaultOptionIDCards == card6ID)
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card7.Visibility = Visibility.Visible;
                Card7TB.Text = CountCoverList[6];
                card7ID = CountIDList[6];
                CardImage7.Source = new BitmapImage(ImageURLString[6]);
                if (DefaultOptionIDCards == card7ID)
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card8.Visibility = Visibility.Visible;
                Card8TB.Text = CountCoverList[7];
                card8ID = CountIDList[7];
                CardImage8.Source = new BitmapImage(ImageURLString[7]);
                if (DefaultOptionIDCards == card8ID)
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card9.Visibility = Visibility.Visible;
                Card9TB.Text = CountCoverList[8];
                card9ID = CountIDList[8];
                CardImage9.Source = new BitmapImage(ImageURLString[8]);
                if (DefaultOptionIDCards == card9ID)
                {
                    Card9.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card9.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card10.Visibility = Visibility.Visible;
                Card10TB.Text = CountCoverList[9];
                card10ID = CountIDList[9];
                CardImage10.Source = new BitmapImage(ImageURLString[9]);
                if (DefaultOptionIDCards == card10ID)
                {
                    Card10.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card10.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card11TB.Text = CountCoverList[10];
                card11ID = CountIDList[10];
                Card11.Visibility = Visibility.Visible;
                CardImage11.Source = new BitmapImage(ImageURLString[10]);
                if (DefaultOptionIDCards == card11ID)
                {
                    Card11.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card11.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card12.Visibility = Visibility.Visible;
                Card12TB.Text = CountCoverList[11];
                card12ID = CountIDList[11];
                CardImage12.Source = new BitmapImage(ImageURLString[11]);
                if (DefaultOptionIDCards == card12ID)
                {
                    Card12.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card12.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card13.Visibility = Visibility.Visible;
                Card13TB.Text = CountCoverList[12];
                card13ID = CountIDList[12];
                CardImage13.Source = new BitmapImage(ImageURLString[12]);
                if (DefaultOptionIDCards == card13ID)
                {
                    Card13.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card13.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card14.Visibility = Visibility.Visible;
                Card14TB.Text = CountCoverList[13];
                card14ID = CountIDList[13];
                CardImage14.Source = new BitmapImage(ImageURLString[13]);
                if (DefaultOptionIDCards == card14ID)
                {
                    Card14.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card14.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card15.Visibility = Visibility.Hidden;
                Card16.Visibility = Visibility.Hidden;

            }
            else if (CountOptions == 15)
            {
                Card1.Visibility = Visibility.Visible;
                Card1TB.Text = CountCoverList[0];
                card1ID = CountIDList[0];
                CardImage1.Source = new BitmapImage(ImageURLString[0]);
                if (DefaultOptionIDCards == card1ID)
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card2.Visibility = Visibility.Visible;
                Card2TB.Text = CountCoverList[1];
                card2ID = CountIDList[1];
                CardImage2.Source = new BitmapImage(ImageURLString[1]);
                if (DefaultOptionIDCards == card2ID)
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card3.Visibility = Visibility.Visible;
                Card3TB.Text = CountCoverList[2];
                card3ID = CountIDList[2];
                CardImage3.Source = new BitmapImage(ImageURLString[2]);
                if (DefaultOptionIDCards == card3ID)
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card4.Visibility = Visibility.Visible;
                Card4TB.Text = CountCoverList[3];
                card4ID = CountIDList[3];
                CardImage4.Source = new BitmapImage(ImageURLString[3]);
                if (DefaultOptionIDCards == card4ID)
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card5.Visibility = Visibility.Visible;
                Card5TB.Text = CountCoverList[4];
                card5ID = CountIDList[4];
                CardImage5.Source = new BitmapImage(ImageURLString[4]);
                if (DefaultOptionIDCards == card5ID)
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card6.Visibility = Visibility.Visible;
                Card6TB.Text = CountCoverList[5];
                card6ID = CountIDList[5];
                CardImage6.Source = new BitmapImage(ImageURLString[5]);
                if (DefaultOptionIDCards == card6ID)
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card7.Visibility = Visibility.Visible;
                Card7TB.Text = CountCoverList[6];
                card7ID = CountIDList[6];
                CardImage7.Source = new BitmapImage(ImageURLString[6]);
                if (DefaultOptionIDCards == card7ID)
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card8.Visibility = Visibility.Visible;
                Card8TB.Text = CountCoverList[7];
                card8ID = CountIDList[7];
                CardImage8.Source = new BitmapImage(ImageURLString[7]);
                if (DefaultOptionIDCards == card8ID)
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card9.Visibility = Visibility.Visible;
                Card9TB.Text = CountCoverList[8];
                card9ID = CountIDList[8];
                CardImage9.Source = new BitmapImage(ImageURLString[8]);
                if (DefaultOptionIDCards == card9ID)
                {
                    Card9.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card9.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card10.Visibility = Visibility.Visible;
                Card10TB.Text = CountCoverList[9];
                card10ID = CountIDList[9];
                CardImage10.Source = new BitmapImage(ImageURLString[9]);
                if (DefaultOptionIDCards == card10ID)
                {
                    Card10.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card10.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card11TB.Text = CountCoverList[10];
                card11ID = CountIDList[10];
                Card11.Visibility = Visibility.Visible;
                CardImage11.Source = new BitmapImage(ImageURLString[10]);
                if (DefaultOptionIDCards == card11ID)
                {
                    Card11.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card11.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card12.Visibility = Visibility.Visible;
                Card12TB.Text = CountCoverList[11];
                card12ID = CountIDList[11];
                CardImage12.Source = new BitmapImage(ImageURLString[11]);
                if (DefaultOptionIDCards == card12ID)
                {
                    Card12.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card12.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card13.Visibility = Visibility.Visible;
                Card13TB.Text = CountCoverList[12];
                card13ID = CountIDList[12];
                CardImage13.Source = new BitmapImage(ImageURLString[12]);
                if (DefaultOptionIDCards == card13ID)
                {
                    Card13.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card13.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card14.Visibility = Visibility.Visible;
                Card14TB.Text = CountCoverList[13];
                card14ID = CountIDList[13];
                CardImage14.Source = new BitmapImage(ImageURLString[13]);
                if (DefaultOptionIDCards == card14ID)
                {
                    Card14.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card14.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card15.Visibility = Visibility.Visible;
                Card15TB.Text = CountCoverList[14];
                card15ID = CountIDList[14];
                CardImage15.Source = new BitmapImage(ImageURLString[14]);
                if (DefaultOptionIDCards == card15ID)
                {
                    Card15.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card15.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card16.Visibility = Visibility.Hidden;

            }
            else if (CountOptions == 16)
            {
                Card1.Visibility = Visibility.Visible;
                Card1TB.Text = CountCoverList[0];
                card1ID = CountIDList[0];
                CardImage1.Source = new BitmapImage(ImageURLString[0]);
                if (DefaultOptionIDCards == card1ID)
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card1.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card2.Visibility = Visibility.Visible;
                Card2TB.Text = CountCoverList[1];
                card2ID = CountIDList[1];
                CardImage2.Source = new BitmapImage(ImageURLString[1]);
                if (DefaultOptionIDCards == card2ID)
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card2.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card3.Visibility = Visibility.Visible;
                Card3TB.Text = CountCoverList[2];
                card3ID = CountIDList[2];
                CardImage3.Source = new BitmapImage(ImageURLString[2]);
                if (DefaultOptionIDCards == card3ID)
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card3.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card4.Visibility = Visibility.Visible;
                Card4TB.Text = CountCoverList[3];
                card4ID = CountIDList[3];
                CardImage4.Source = new BitmapImage(ImageURLString[3]);
                if (DefaultOptionIDCards == card4ID)
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card4.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card5.Visibility = Visibility.Visible;
                Card5TB.Text = CountCoverList[4];
                card5ID = CountIDList[4];
                CardImage5.Source = new BitmapImage(ImageURLString[4]);
                if (DefaultOptionIDCards == card5ID)
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card5.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card6.Visibility = Visibility.Visible;
                Card6TB.Text = CountCoverList[5];
                card6ID = CountIDList[5];
                CardImage6.Source = new BitmapImage(ImageURLString[5]);
                if (DefaultOptionIDCards == card6ID)
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card6.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card7.Visibility = Visibility.Visible;
                Card7TB.Text = CountCoverList[6];
                card7ID = CountIDList[6];
                CardImage7.Source = new BitmapImage(ImageURLString[6]);
                if (DefaultOptionIDCards == card7ID)
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card7.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card8.Visibility = Visibility.Visible;
                Card8TB.Text = CountCoverList[7];
                card8ID = CountIDList[7];
                CardImage8.Source = new BitmapImage(ImageURLString[7]);
                if (DefaultOptionIDCards == card8ID)
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card8.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card9.Visibility = Visibility.Visible;
                Card9TB.Text = CountCoverList[8];
                card9ID = CountIDList[8];
                CardImage9.Source = new BitmapImage(ImageURLString[8]);
                if (DefaultOptionIDCards == card9ID)
                {
                    Card9.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card9.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card10.Visibility = Visibility.Visible;
                Card10TB.Text = CountCoverList[9];
                card10ID = CountIDList[9];
                CardImage10.Source = new BitmapImage(ImageURLString[9]);
                if (DefaultOptionIDCards == card10ID)
                {
                    Card10.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card10.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card11TB.Text = CountCoverList[10];
                card11ID = CountIDList[10];
                Card11.Visibility = Visibility.Visible;
                CardImage11.Source = new BitmapImage(ImageURLString[10]);
                if (DefaultOptionIDCards == card11ID)
                {
                    Card11.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card11.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card12.Visibility = Visibility.Visible;
                Card12TB.Text = CountCoverList[11];
                card12ID = CountIDList[11];
                CardImage12.Source = new BitmapImage(ImageURLString[11]);
                if (DefaultOptionIDCards == card12ID)
                {
                    Card12.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card12.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card13.Visibility = Visibility.Visible;
                Card13TB.Text = CountCoverList[12];
                card13ID = CountIDList[12];
                CardImage13.Source = new BitmapImage(ImageURLString[12]);
                if (DefaultOptionIDCards == card13ID)
                {
                    Card13.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card13.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card14.Visibility = Visibility.Visible;
                Card14TB.Text = CountCoverList[13];
                card14ID = CountIDList[13];
                CardImage14.Source = new BitmapImage(ImageURLString[13]);
                if (DefaultOptionIDCards == card14ID)
                {
                    Card14.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card14.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card15.Visibility = Visibility.Visible;
                Card15TB.Text = CountCoverList[14];
                card15ID = CountIDList[14];
                CardImage15.Source = new BitmapImage(ImageURLString[14]);
                if (DefaultOptionIDCards == card15ID)
                {
                    Card15.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card15.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }

                Card16.Visibility = Visibility.Visible;
                Card16TB.Text = CountCoverList[15];
                card16ID = CountIDList[15];
                CardImage16.Source = new BitmapImage(ImageURLString[15]);
                if (DefaultOptionIDCards == card16ID)
                {
                    Card16.BorderBrush = System.Windows.Media.Brushes.BlueViolet;
                }
                else
                {
                    Card16.BorderBrush = System.Windows.Media.Brushes.DarkCyan;
                }
            }
        }

        private void Card1_Click(object sender, RoutedEventArgs e)
        {
            ReturnToPage2();
            //cmbUpgradeCover.SelectedValue = card1ID;
            ButtonUpgradeCalculations(card1ID);
            UpgradeBtnOptionSelection.Content = Card1TB.Text;
        }

        private void Card2_Click(object sender, RoutedEventArgs e)
        {
            ReturnToPage2();
            ButtonUpgradeCalculations(card2ID);
            UpgradeBtnOptionSelection.Content = Card2TB.Text;
        }

        private void Card3_Click(object sender, RoutedEventArgs e)
        {
            ReturnToPage2();
            ButtonUpgradeCalculations(card3ID);
            UpgradeBtnOptionSelection.Content = Card3TB.Text;
        }

        private void Card4_Click(object sender, RoutedEventArgs e)
        {
            ReturnToPage2();
            ButtonUpgradeCalculations(card4ID);
            UpgradeBtnOptionSelection.Content = Card4TB.Text;
        }

        private void Card5_Click(object sender, RoutedEventArgs e)
        {
            ReturnToPage2();
            ButtonUpgradeCalculations(card5ID);
            UpgradeBtnOptionSelection.Content = Card5TB.Text;
        }

        private void Card6_Click(object sender, RoutedEventArgs e)
        {
            ReturnToPage2();
            ButtonUpgradeCalculations(card6ID);
            UpgradeBtnOptionSelection.Content = Card6TB.Text;
        }

        private void Card7_Click(object sender, RoutedEventArgs e)
        {
            ReturnToPage2();
            ButtonUpgradeCalculations(card7ID);
            UpgradeBtnOptionSelection.Content = Card7TB.Text;
        }

        private void Card8_Click(object sender, RoutedEventArgs e)
        {
            ReturnToPage2();
            ButtonUpgradeCalculations(card8ID);
            UpgradeBtnOptionSelection.Content = Card8TB.Text;
        }

        private void Card9_Click(object sender, RoutedEventArgs e)
        {
            ReturnToPage2();
            ButtonUpgradeCalculations(card9ID);
            UpgradeBtnOptionSelection.Content = Card9TB.Text;
        }

        private void Card10_Click(object sender, RoutedEventArgs e)
        {
            ReturnToPage2();
            ButtonUpgradeCalculations(card10ID);
            UpgradeBtnOptionSelection.Content = Card10TB.Text;
        }

        private void Card11_Click(object sender, RoutedEventArgs e)
        {
            ReturnToPage2();
            ButtonUpgradeCalculations(card11ID);
            UpgradeBtnOptionSelection.Content = Card11TB.Text;
        }

        private void Card12_Click(object sender, RoutedEventArgs e)
        {
            ReturnToPage2();
            ButtonUpgradeCalculations(card12ID);
            UpgradeBtnOptionSelection.Content = Card12TB.Text;
        }
        private void Card13_Click(object sender, RoutedEventArgs e)
        {
            ReturnToPage2();
            ButtonUpgradeCalculations(card13ID);
            UpgradeBtnOptionSelection.Content = Card13TB.Text;
        }

        private void Card15_Click(object sender, RoutedEventArgs e)
        {
            ReturnToPage2();
            ButtonUpgradeCalculations(card15ID);
            UpgradeBtnOptionSelection.Content = Card15TB.Text;
        }

        private void Card14_Click(object sender, RoutedEventArgs e)
        {
            ReturnToPage2();
            ButtonUpgradeCalculations(card14ID);
            UpgradeBtnOptionSelection.Content = Card14TB.Text;
        }

        private void Card16_Click(object sender, RoutedEventArgs e)
        {
            ReturnToPage2();
            ButtonUpgradeCalculations(card16ID);
            UpgradeBtnOptionSelection.Content = Card16TB.Text;
        }

        #endregion
        private void btnOverrideBumpUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal Premium1 = Convert.ToDecimal(LaData.PolicyData.LoadedTotalPremium);
                decimal InvoiceFee1 = Convert.ToDecimal(LaData.PolicyData.LoadedTotalInvoiceFee);
                decimal Premium2 = Convert.ToDecimal(Regex.Match(xamCETotalPremium.Text, @"\d*[\.,]\d*").Value);
                decimal InvoiceFee2 = Convert.ToDecimal(LaData.PolicyData.TotalInvoiceFee);

                //if (Premium1 > 0 && Premium2 > 0 && InvoiceFee1 > 0 && InvoiceFee2 > 0)//if (InvoiceFee1 > 0 && InvoiceFee2 > 0)
                //{
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ImportID", LaData.AppData.ImportID);
                //this gets what the total invoice fee was when the sales agent saved it so that the bumpup agents cannot say that it was a bumpup by
                //saving on a lower premium and then going back and saving it on a higher premium but still a lower premium than what the sales agent saved it on.
                decimal historicTotalInvoiceFee = Convert.ToDecimal(Methods.ExecuteFunction("fnGetLatestInvoiceFeeSavedBySalesAgent", parameters));

                decimal NewInvoiceFee = Convert.ToDecimal(Methods.ExecuteFunction("fnGetMostRecentInvoiceFeeSavedByBumpUpAgent", parameters));

                if ((InvoiceFee2 >= InvoiceFee1 && InvoiceFee2 <= historicTotalInvoiceFee) || (Premium2 > Premium1 && LaData.AppData.IsLeadUpgrade))//bumpup
                {

                    //if (Fee2 > Fee1)

                    INMessageBoxWindow2 messageBox = new INMessageBoxWindow2();
                    messageBox.buttonOK.Content = "Yes";
                    messageBox.buttonCancel.Content = "No";

                    var showMessageBox = ShowMessageBox(messageBox, "Is this a Bump Up?", "Bump Up", ShowMessageType.Information);
                    bool result = showMessageBox != null && (bool)showMessageBox;

                    if (result)
                    {

                        LaData.PolicyData.BumpUpAmount = Premium2 - Premium1;
                        LaData.PolicyData.UDMBumpUpOption = true;



                        LaData.PolicyData.ReducedPremiumAmount = null;
                        LaData.PolicyData.ReducedPremiumOption = false;
                        if ((lkpUserType?)((User)GlobalSettings.ApplicationUser).FKUserType == lkpUserType.CallMonitoringAgent)
                        {
                            LaData.SaleData.FKCMCallRefUserID = GlobalSettings.ApplicationUser.ID;
                        }


                        //Comment this out after testing and before publishing again.
                        //if (inImportCallMonitoring != null)
                        //{
                        //    inImportCallMonitoring.IsCallMonitored = false;
                        //}
                    }

                }

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }


        }

        #region Permission Lead
        private void btnPermissionLead_Click(object sender, RoutedEventArgs e)
        {

            if ((bool)chkLeadPermission.IsChecked)
            {
                try
                {
                    string strQuery;
                    strQuery = "SELECT ID FROM INPermissionLead WHERE FKImportID = " + LaData.AppData.ImportID;

                    DataTable dtPolicyPlanGroup = Methods.GetTableData(strQuery);
                    if (dtPolicyPlanGroup.Rows.Count == 0)
                    {
                        GlobalSettings.IsLoadedPermission = 0;
                    }
                    else
                    {
                        GlobalSettings.IsLoadedPermission = 1;
                    }

                    PermissionLeadScreen mySuccess = new PermissionLeadScreen(LaData.AppData.ImportID, cmbLA2Title.Text, medLA2Name.Text, medLA2Surname.Text, medLA2ContactPhone.Text, medAltContactPhone.Text);
                    ShowDialog(mySuccess, new INDialogWindow(mySuccess));



                }
                catch
                {
                    PermissionLeadScreen mySuccess = new PermissionLeadScreen(LaData.AppData.ImportID, cmbLA2Title.Text, medLA2Name.Text, medLA2Surname.Text, medLA2ContactPhone.Text, medAltContactPhone.Text);
                    ShowDialog(mySuccess, new INDialogWindow(mySuccess));
                }
            }
            else
            {
                try
                {
                    string strQuery;
                    strQuery = "SELECT ID FROM INPermissionLead WHERE FKImportID = " + LaData.AppData.ImportID;

                    DataTable dtPolicyPlanGroup = Methods.GetTableData(strQuery);
                    if (dtPolicyPlanGroup.Rows.Count == 0)
                    {
                        GlobalSettings.IsLoadedPermission = 0;
                    }
                    else
                    {
                        GlobalSettings.IsLoadedPermission = 1;

                    }
                    PermissionLeadScreen mySuccess = new PermissionLeadScreen(LaData.AppData.ImportID, null, null, null, null, null);
                    ShowDialog(mySuccess, new INDialogWindow(mySuccess));



                }
                catch
                {
                    PermissionLeadScreen mySuccess = new PermissionLeadScreen(LaData.AppData.ImportID, null, null, null, null, null);
                    ShowDialog(mySuccess, new INDialogWindow(mySuccess));
                }
            }


        }

        private void chkLeadPermission_Checked_1(object sender, RoutedEventArgs e)
        {
            lblAltContactPhone.Visibility = Visibility.Visible;
            medAltContactPhone.Visibility = Visibility.Visible;

            try
            {
                //INPermissionLead inpermissionlead = new INPermissionLead();
                //inpermissionlead.FKINImportID = LaData.AppData.ImportID;
                //try { inpermissionlead.Title = cmbLA2Title.Text.ToString(); } catch { inpermissionlead.Title = " "; }
                //try { inpermissionlead.Firstname = medLA2Name.Text.ToString(); } catch { inpermissionlead.Firstname = " "; } // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/222495313/comments
                //try { inpermissionlead.Surname = medLA2Surname.Text.ToString(); } catch { inpermissionlead.Surname = " "; }
                //try { inpermissionlead.Cellnumber = medLA2ContactPhone.Text.ToString(); } catch { inpermissionlead.Cellnumber = " "; }
                //try { inpermissionlead.AltNumber = medAltContactPhone.Text.ToString(); } catch { inpermissionlead.AltNumber = " "; }


                //inpermissionlead.Save(_validationResult);
            }
            catch
            {

            }
        }

        private void chkLeadPermission_Unchecked(object sender, RoutedEventArgs e)
        {
            lblAltContactPhone.Visibility = Visibility.Collapsed;
            medAltContactPhone.Visibility = Visibility.Collapsed;
        }

        private void chkLeadPermission_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (chkLeadPermission.IsEnabled == false)
            {
                chkLeadPermission.IsEnabled = true;
            }
        }
        #endregion


        private void btnDebiCheck_Click(object sender, RoutedEventArgs e)
        {
            #region Constants
            //string documentTypeDebiCheckSent = LaData.BankDetailsData.typ
            string branchCodeID = LaData.BankDetailsData.BankBranchCodeID.ToString();
            string accounTypeID = LaData.BankDetailsData.AccountTypeID.ToString();

            string MandateRequestID;
            string MandateStatusCode;
            string ClientResponseStatus;
            string ClientsBankResponseStatusCode;
            string SubmittingBankResponseStatusCode;

            string responsesBranchCode = "";
            string responsesAccountTypeDebiCheck = "";

            DateTime CommencementDateDebiCheck = DateTime.Now;
            DateTime CommencementDateEdited;

            string IDNumberDebiCheck = "1";
            #endregion

            #region variable workings
            // this is for the branch code
            try
            {
                StringBuilder strQueryBranchCode = new StringBuilder();
                strQueryBranchCode.Append("SELECT TOP 1 Code [Response] ");
                strQueryBranchCode.Append("FROM BankBranch ");
                strQueryBranchCode.Append("WHERE ID = " + branchCodeID);
                strQueryBranchCode.Append(" ORDER BY ID DESC");
                DataTable dtBranchCode = Methods.GetTableData(strQueryBranchCode.ToString());

                responsesBranchCode = dtBranchCode.Rows[0]["Response"].ToString();

            }
            catch
            {
                responsesBranchCode = "";
            }

            //this is for the account type
            try
            {
                StringBuilder strQueryAccountype = new StringBuilder();
                strQueryAccountype.Append("SELECT TOP 1 Description [Response] ");
                strQueryAccountype.Append("FROM lkpAccountType ");
                strQueryAccountype.Append("WHERE ID = " + accounTypeID);
                strQueryAccountype.Append(" ORDER BY ID DESC");
                DataTable dtBranchCode = Methods.GetTableData(strQueryAccountype.ToString());

                string responsesAccountType = dtBranchCode.Rows[0]["Response"].ToString();

                if (responsesAccountType == "Current")
                {
                    responsesAccountTypeDebiCheck = "1";
                }
                else if (responsesAccountType == "Savings")
                {
                    responsesAccountTypeDebiCheck = "2";
                }
                else if (responsesAccountType == "Transmission")
                {
                    responsesAccountTypeDebiCheck = "3";
                }
            }
            catch
            {

            }

            //this is for the commencement date
            try
            {
                DateTime datevariable = DateTime.Parse(LaData.AppData.DateOfSale.ToString());
                CommencementDateDebiCheck = datevariable.AddMonths(2);
                CommencementDateEdited = new DateTime(CommencementDateDebiCheck.Year, CommencementDateDebiCheck.Month, int.Parse(LaData.BankDetailsData.DebitDay.ToString()));
            }
            catch
            {
                CommencementDateEdited = DateTime.Now;
            }

            //this is for thw ID Number
            try
            {
                string IDNumber = LaData.BankDetailsData.IDNumber;
                int result = IDNumber.Count();
                if (result == 13)
                {
                    IDNumberDebiCheck = "1";
                }
                else
                {
                    IDNumberDebiCheck = "2";
                }
            }
            catch
            {
                IDNumberDebiCheck = "1";
            }

            //btnDebiCheck.IsEnabled = false;
            string token = "";

            #endregion

            #region Get Token
            try
            {


                string auth_url = "http://plhqweb.platinumlife.co.za:8081/Token";
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

            #region Mandate Request
            try
            {
                string submitMandate_url = "http://plhqweb.platinumlife.co.za:8081/api/Mandate/submitMandateRequest";
                using (var wb = new MyWebClient(180000))
                {
                    var data = new NameValueCollection();

                    if (LaData.AppData.CampaignGroupType == lkpINCampaignGroupType.Upgrade)
                    {

                        string AccountTypeNumber = " ";
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

                        try
                        {
                            DateTime datevariable = DateTime.Parse(LaData.AppData.DateOfSale.ToString());
                            CommencementDateDebiCheck = datevariable.AddMonths(2);
                            CommencementDateEdited = new DateTime(CommencementDateDebiCheck.Year, CommencementDateDebiCheck.Month, int.Parse(DebitDayPLLKP));
                        }
                        catch
                        {
                            CommencementDateEdited = DateTime.Now;
                        }

                        try { data["ClientName"] = LaData.LeadData.FullName; } catch { data["ClientName"] = ""; }
                        try { data["CellNumber"] = LaData.LeadData.TelCell; } catch { data["CellNumber"] = ""; }
                        try { data["Email"] = LaData.LeadData.Email; } catch { data["Email"] = ""; }
                        try { data["DocumentTypeID"] = IDNumberDebiCheck; } catch { data["DocumentTypeID"] = ""; }
                        if (IDNumberDebiCheck == "2")
                        {
                            try { data["IdentificationNumber"] = LaData.LeadData.PassportNumber; } catch { data["IdentificationNumber"] = ""; }
                        }
                        else if (IDNumberDebiCheck == "1")
                        {
                            try { data["IdentificationNumber"] = LaData.LeadData.IDNumber; } catch { data["IdentificationNumber"] = ""; }
                        }
                        try { data["ReferenceNumber"] = LaData.AppData.RefNo; } catch { data["ReferenceNumber"] = ""; }
                        try { data["BranchCode"] = BranchCodePLLKP; } catch { data["BranchCode"] = ""; }
                        try { data["AccountNumber"] = AccountNumberPLLKP; } catch { data["AccountNumber"] = ""; }
                        try { data["InstallmentAmount"] = LaData.PolicyData.TotalPremium.ToString(); } catch { data["InstallmentAmount"] = ""; }
                        try { data["MaxInstallmentAmount"] = ""; } catch { data["MaxInstallmentAmount"] = ""; }
                        try { data["FirstCollectionDate"] = CommencementDateEdited.ToString(); } catch { data["FirstCollectionDate"] = ""; }
                        try { data["AccountTypeID"] = AccountTypeNumber; } catch { data["AccountTypeID"] = "1"; }
                        try { data["CustomField1"] = LaData.AppData.CampaignCode; } catch { data["CustomField1"] = " "; }



                    }
                    else if (LaData.AppData.CampaignID == 7
                            || LaData.AppData.CampaignID == 9
                            || LaData.AppData.CampaignID == 10
                            || LaData.AppData.CampaignID == 294
                            || LaData.AppData.CampaignID == 295
                            || LaData.AppData.CampaignID == 24
                            || LaData.AppData.CampaignID == 25
                            || LaData.AppData.CampaignID == 11
                            || LaData.AppData.CampaignID == 12
                            || LaData.AppData.CampaignID == 13
                            || LaData.AppData.CampaignID == 14
                            || LaData.AppData.CampaignID == 85
                            || LaData.AppData.CampaignID == 86
                            || LaData.AppData.CampaignID == 87
                            || LaData.AppData.CampaignID == 281
                            || LaData.AppData.CampaignID == 324
                            || LaData.AppData.CampaignID == 325
                            || LaData.AppData.CampaignID == 326
                            || LaData.AppData.CampaignID == 327
                            || LaData.AppData.CampaignID == 264
                            || LaData.AppData.CampaignID == 4)
                    {
                        string AccountTypeNumber = " ";
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
                        else
                        {
                            try
                            {
                                StringBuilder strQueryAccountype = new StringBuilder();
                                strQueryAccountype.Append("SELECT TOP 1 Description [Response] ");
                                strQueryAccountype.Append("FROM lkpAccountType ");
                                strQueryAccountype.Append("WHERE ID = " + accounTypeID);
                                strQueryAccountype.Append(" ORDER BY ID DESC");
                                DataTable dtBranchCode = Methods.GetTableData(strQueryAccountype.ToString());

                                string responsesAccountType = dtBranchCode.Rows[0]["Response"].ToString();

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
                            }
                            catch
                            {

                            }
                        }





                        try
                        {
                            DateTime datevariable = DateTime.Parse(LaData.AppData.DateOfSale.ToString());
                            CommencementDateDebiCheck = datevariable.AddMonths(2);
                            CommencementDateEdited = new DateTime(CommencementDateDebiCheck.Year, CommencementDateDebiCheck.Month, int.Parse(DebitDayPLLKP));
                        }
                        catch
                        {
                            DateTime datevariable = DateTime.Parse(LaData.AppData.DateOfSale.ToString());
                            CommencementDateDebiCheck = datevariable.AddMonths(2);
                            CommencementDateEdited = new DateTime(CommencementDateDebiCheck.Year, CommencementDateDebiCheck.Month, int.Parse(LaData.BankDetailsData.DebitDay.ToString()));
                        }

                        try { data["ClientName"] = LaData.LeadData.FullName; } catch { data["ClientName"] = ""; }
                        try { data["CellNumber"] = LaData.LeadData.TelCell; } catch { data["CellNumber"] = ""; }
                        try { data["Email"] = LaData.LeadData.Email; } catch { data["Email"] = ""; }
                        try { data["DocumentTypeID"] = IDNumberDebiCheck; } catch { data["DocumentTypeID"] = ""; }
                        if (IDNumberDebiCheck == "2")
                        {
                            try { data["IdentificationNumber"] = LaData.LeadData.PassportNumber; } catch { data["IdentificationNumber"] = ""; }
                        }
                        else if (IDNumberDebiCheck == "1")
                        {
                            try { data["IdentificationNumber"] = LaData.LeadData.IDNumber; } catch { data["IdentificationNumber"] = ""; }
                        }
                        try { data["ReferenceNumber"] = LaData.AppData.RefNo; } catch { data["ReferenceNumber"] = ""; }

                        if (medDOAccountNumber.Text != "")
                        {
                            try { data["BranchCode"] = responsesBranchCode; } catch { data["BranchCode"] = ""; }
                        }
                        else
                        {
                            try { data["BranchCode"] = BranchCodePLLKP; } catch { data["BranchCode"] = ""; }
                        }

                        if (medDOAccountNumber.Text != "")
                        {
                            try { data["AccountNumber"] = medDOAccountNumber.Text; } catch { data["AccountNumber"] = ""; }
                        }
                        else
                        {
                            try { data["AccountNumber"] = AccountNumberPLLKP; } catch { data["AccountNumber"] = ""; }
                        }

                        try { data["InstallmentAmount"] = LaData.PolicyData.TotalPremium.ToString(); } catch { data["InstallmentAmount"] = ""; }
                        try { data["MaxInstallmentAmount"] = ""; } catch { data["MaxInstallmentAmount"] = ""; }
                        try { data["FirstCollectionDate"] = CommencementDateEdited.ToString(); } catch { data["FirstCollectionDate"] = ""; }
                        try { data["AccountTypeID"] = AccountTypeNumber; } catch { data["AccountTypeID"] = "1"; }
                        try { data["CustomField1"] = LaData.AppData.CampaignCode; } catch { data["CustomField1"] = " "; }
                    }
                    else
                    {
                        try { data["ClientName"] = LaData.BankDetailsData.FullName; } catch { data["ClientName"] = ""; }
                        try { data["CellNumber"] = LaData.BankDetailsData.TelCell; } catch { data["CellNumber"] = ""; }
                        try { data["Email"] = LaData.BankDetailsData.Email; } catch { data["Email"] = ""; }
                        try { data["DocumentTypeID"] = IDNumberDebiCheck; } catch { data["DocumentTypeID"] = ""; }
                        if (IDNumberDebiCheck == "2")
                        {
                            try { data["IdentificationNumber"] = LaData.LeadData.PassportNumber; } catch { data["IdentificationNumber"] = ""; }
                        }
                        else if (IDNumberDebiCheck == "1")
                        {
                            try { data["IdentificationNumber"] = LaData.BankDetailsData.IDNumber; } catch { data["IdentificationNumber"] = ""; }
                        }
                        try { data["ReferenceNumber"] = LaData.AppData.RefNo; } catch { data["ReferenceNumber"] = ""; }
                        try { data["BranchCode"] = responsesBranchCode; } catch { data["BranchCode"] = ""; }
                        try { data["AccountNumber"] = LaData.BankDetailsData.AccountNumber; } catch { data["AccountNumber"] = ""; }
                        try { data["InstallmentAmount"] = LaData.PolicyData.TotalPremium.ToString(); } catch { data["InstallmentAmount"] = ""; }
                        try { data["MaxInstallmentAmount"] = ""; } catch { data["MaxInstallmentAmount"] = ""; }
                        try { data["FirstCollectionDate"] = CommencementDateEdited.ToString(); } catch { data["FirstCollectionDate"] = ""; }
                        try { data["AccountTypeID"] = responsesAccountTypeDebiCheck; } catch { data["AccountTypeID"] = "1"; }
                        try { data["CustomField1"] = LaData.AppData.CampaignCode; } catch { data["CustomField1"] = " "; }

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

                    MandateRequestID = (string)customObject["MandateRequestID"];
                    MandateStatusCode = (string)customObject["MandateStatusCode"];
                    ClientResponseStatus = (string)customObject["ClientResponseStatus"];
                    ClientsBankResponseStatusCode = (string)customObject["ClientsBankResponseStatusCode"];
                    SubmittingBankResponseStatusCode = (string)customObject["SubmittingBankResponseStatusCode"];
                }
            }
            catch (Exception q)
            {
                MandateStatusCode = null;
                ClientResponseStatus = null;
                ClientsBankResponseStatusCode = null;
            }


            #endregion

            #region Save Debi Chek on our side
            try
            {

                DebiCheckSent DebiCheckSentData = new DebiCheckSent();

                DebiCheckSentData.FKSystemID = (long?)lkpSystem.Insurance;
                DebiCheckSentData.FKImportID = LaData.AppData.ImportID;
                DebiCheckSentData.SMSID = ClientResponseStatus;
                DebiCheckSentData.FKlkpSMSTypeID = 2;
                DebiCheckSentData.RecipientCellNum = LaData.BankDetailsData.TelCell;
                DebiCheckSentData.SMSBody = MandateStatusCode;
                DebiCheckSentData.FKlkpSMSEncodingID = 2;
                DebiCheckSentData.SubmissionID = ClientsBankResponseStatusCode;
                DebiCheckSentData.FKlkpSMSStatusSubtypeID = 1;
                DebiCheckSentData.FKlkpSMSStatusSubtypeID = 1;
                DebiCheckSentData.SubmissionDate = DateTime.Now;
                DebiCheckSentData.FKlkpSMSStatusTypeID = 1;
                DebiCheckSentData.FKlkpSMSStatusSubtypeID = 1;

                //DebiCheckSentData.FKSystemID = (long?)lkpSystem.Insurance;
                //DebiCheckSentData.FKImportID = LaData.AppData.ImportID;
                //DebiCheckSentData.SMSID = "";
                //DebiCheckSentData.FKlkpSMSTypeID = 2;
                //DebiCheckSentData.RecipientCellNum = LaData.BankDetailsData.TelCell;
                //DebiCheckSentData.SMSBody = "6";
                //DebiCheckSentData.FKlkpSMSEncodingID = 2;
                //DebiCheckSentData.SubmissionID = "";
                //DebiCheckSentData.FKlkpSMSStatusSubtypeID = 1;
                //DebiCheckSentData.FKlkpSMSStatusSubtypeID = 1;
                //DebiCheckSentData.SubmissionDate = DateTime.Now;
                //DebiCheckSentData.FKlkpSMSStatusTypeID = 1;
                //DebiCheckSentData.FKlkpSMSStatusSubtypeID = 1;


                DebiCheckSentData.Save(_validationResult);


            }
            catch
            {

            }

            #endregion

            #region Button Feedback
            try
            {
                StringBuilder strQuery = new StringBuilder();
                strQuery.Append("SELECT TOP 1 SMSBody [Response] ");
                strQuery.Append("FROM DebiCheckSent ");
                strQuery.Append("WHERE FKImportID = " + LaData.AppData.ImportID);
                strQuery.Append(" ORDER BY ID DESC");
                DataTable dt = Methods.GetTableData(strQuery.ToString());

                string responses = dt.Rows[0]["Response"].ToString();
                //string responses = "2";

                if (responses.Contains("1"))
                {
                    DebiCheckBorder.BorderBrush = Brushes.Green;
                    btnDebiCheck.ToolTip = "Record Created";
                    if (DebiCheckSentTwice == true)
                    {
                        btnDebiCheck.IsEnabled = false;
                    }

                }
                else if (responses.Contains("2"))
                {
                    DebiCheckBorder.BorderBrush = Brushes.Green;
                    btnDebiCheck.ToolTip = "Record Submitted";
                    if (DebiCheckSentTwice == true)
                    {
                        btnDebiCheck.IsEnabled = false;
                    }

                }
                else if (responses.Contains("3"))
                {
                    DebiCheckBorder.BorderBrush = Brushes.Green;
                    btnDebiCheck.ToolTip = "Results Received but no Further Details";
                    if (DebiCheckSentTwice == true)
                    {
                        btnDebiCheck.IsEnabled = false;
                    }

                }
                else if (responses.Contains("4"))
                {
                    DebiCheckBorder.BorderBrush = Brushes.Red;
                    btnDebiCheck.ToolTip = "Bank Returned Authentication Failure";
                    btnDebiCheck.IsEnabled = true;

                }
                else if (responses.Contains("5"))
                {
                    DebiCheckBorder.BorderBrush = Brushes.Red;
                    btnDebiCheck.ToolTip = "Bank Returned Error With File Submitted";
                    btnDebiCheck.IsEnabled = true;

                }
                else if (responses.Contains("6"))
                {
                    DebiCheckBorder.BorderBrush = Brushes.Green;
                    btnDebiCheck.ToolTip = "Mandate Approved";
                    if (DebiCheckSentTwice == true)
                    {
                        btnDebiCheck.IsEnabled = false;
                    }

                }
                else if (responses.Contains("7"))
                {
                    DebiCheckBorder.BorderBrush = Brushes.Red;
                    btnDebiCheck.ToolTip = "Client Rejected";
                    if (DebiCheckSentTwice == true)
                    {
                        btnDebiCheck.IsEnabled = false;
                    }
                }
                else if (responses.Contains("8"))
                {
                    DebiCheckBorder.BorderBrush = Brushes.Green;
                    btnDebiCheck.ToolTip = "No Response From Client Sent Mandate";
                    if (DebiCheckSentTwice == true)
                    {
                        btnDebiCheck.IsEnabled = false;
                    }
                }
                else if (responses.Contains("9"))
                {
                    DebiCheckBorder.BorderBrush = Brushes.Yellow;
                    btnDebiCheck.ToolTip = "Timeout on Submission. Please Re-Submit";
                    btnDebiCheck.IsEnabled = true;

                }
                else if (responses.Contains("10"))
                {
                    DebiCheckBorder.BorderBrush = Brushes.Green;
                    btnDebiCheck.ToolTip = "File delivered to XCOM for processing";
                    if (DebiCheckSentTwice == true)
                    {
                        btnDebiCheck.IsEnabled = false;
                    }
                }
                else if (responses.Contains(""))
                {
                    DebiCheckBorder.BorderBrush = Brushes.Green;
                    btnDebiCheck.ToolTip = "Sent";
                    if (DebiCheckSentTwice == true)
                    {
                        btnDebiCheck.IsEnabled = false;
                    }
                }
                else
                {
                    DebiCheckBorder.BorderBrush = Brushes.White;
                    btnDebiCheck.IsEnabled = true;
                }

                DebiCheckSentTwice = true;
                GetMandateInfo();
            }
            catch
            {

            }
            #endregion
        }

        private void GetBankingDetailLookup()
        {
            #region AuthToken
            try
            {
                PolicyNumberPLLKP = "";
                ReferenceNumberPLLKP = "";
                ResponseDetailPLLKP = "";
                BankPLLKP = "";
                AccountNumberPLLKP = "";
                AccountHolderPLLKP = "";
                BranchCodePLLKP = "";
                AccountTypePLLKP = "";
                DebitDayPLLKP = "";

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

                #region SumbitBankingDetailRequest
                string submitRequest_url = "http://plhqweb.platinumlife.co.za:999/api/BD/PL_Request";
                using (var wb = new WebClient())
                {
                    var data = new NameValueCollection();
                    data["Organization"] = "PL"; //THG //TBF //IG //IPS
                    data["PolicyNumber"] = LaData.PolicyData.PolicyNumber;
                    data["IDNumber"] = LaData.LeadData.IDNumber;
                    data["ReferenceNumber"] = LaData.AppData.RefNo;
                    data["PassportNumber"] = "";
                    data["FirstName"] = LaData.LeadData.Name;
                    data["LastName"] = LaData.LeadData.Surname;
                    data["MobileNumber"] = LaData.LeadData.TelCell;
                    data["HomeNumber"] = LaData.LeadData.TelHome;
                    data["WorkNumber"] = LaData.LeadData.TelWork;
                    data["EmailAddress"] = LaData.LeadData.Email;

                    wb.Headers.Add("Authorization", "Bearer " + token);

                    var response = wb.UploadValues(submitRequest_url, "POST", data);
                    string responseInString = Encoding.UTF8.GetString(response);
                    var customObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseInString);

                    PolicyNumberPLLKP = (string)customObject["PolicyNumber"];
                    ReferenceNumberPLLKP = (string)customObject["ReferenceNumber"];
                    ResponseDetailPLLKP = (string)customObject["ResponseDetail"]; // WILL RETURN "OK" OR "No Data Found"
                    BankPLLKP = (string)customObject["Bank"];
                    AccountNumberPLLKP = (string)customObject["AccountNumber"];
                    AccountHolderPLLKP = (string)customObject["AccountHolder"];
                    BranchCodePLLKP = (string)customObject["BranchCode"];
                    AccountTypePLLKP = (string)customObject["AccountType"];
                    DebitDayPLLKP = (string)customObject["DebitDay"];

                    if (BankPLLKP == null || BankPLLKP == "")
                    {
                        GotBankingDetailsPL.Background = System.Windows.Media.Brushes.Red;
                        GotBankingDetailsPLLBL2.Text = "Adjust contact details and try again";
                    }
                    else
                    {
                        GotBankingDetailsPL.Background = System.Windows.Media.Brushes.Green;
                        GotBankingDetailsPLLBL2.Text = " ";
                    }

                }
            }
            catch (Exception r)
            {

            }

            #endregion


        }

        public void IsDebiCheckValidForResales()
        {
            //btnDebiCheck.Visibility = Visibility.Collapsed;
            //DebiCheckBorder.Visibility = Visibility.Collapsed;
            //if (medDOAccountNumber.Text == "" || medDOAccountNumber.Text == null)
            //{

            //}
            //else
            //{
            btnDebiCheck.Visibility = Visibility.Visible;
            DebiCheckBorder.Visibility = Visibility.Visible;
            //DebiCheckBorder.BorderBrush = Brushes.White;
            btnDebiCheck.IsEnabled = true;
            //}
        }

        public void GetMandateInfo()
        {

            //StringBuilder strQuery = new StringBuilder();
            //strQuery.Append("SELECT TOP 1 [MandateRequestStatus] [Response] , [CreatedDate]  ");
            //strQuery.Append("FROM [41.170.75.25].[MR_DC].[PLUDM].[MandateRequestsView] ");
            //strQuery.Append("WHERE [ReferenceNumber] COLLATE Latin1_General_CI_AS = " + LaData.AppData.RefNo);
            //strQuery.Append(" ORDER BY [CreatedDate] DESC");
            //DataTable dt = Methods.GetTableData(strQuery.ToString());
            DataSet dsDiaryReportData = null;

            try
            {

                var transactionOptions = new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                };

                using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    dsDiaryReportData = Business.Insure.INGetMandateInfo(LaData.AppData.RefNo);
                }
            }
            catch
            {

            }


            try
            {
                DataTable dt = dsDiaryReportData.Tables[0];
                try
                {
                    string responses = dt.Rows[0]["Response"].ToString();
                    string datetime = dt.Rows[0]["CreatedDate"].ToString();


                    Mandate1TB.Text = responses + " " + datetime;


                }
                catch
                {

                }


                try
                {
                    string responses2 = dt.Rows[1]["Response"].ToString();
                    string datetime = dt.Rows[1]["CreatedDate"].ToString();

                    Mandate2TB.Text = responses2 + " " + datetime;
                }
                catch
                {

                }
            }
            catch
            {

            }



        }

        public void IsDebiCheckValid()
        {
            if (cmbStatus.SelectedValue.ToString() == "1")
            {
                if (LaData.BankDetailsData.Name != null)
                {
                    if (LaData.BankDetailsData.TelCell != null)
                    {

                        if (LaData.LeadData.IDNumber != null)
                        {
                            if (LaData.BankDetailsData.AccountNumber != null)
                            {
                                if (LaData.BankDetailsData.DebitDay != null)
                                {
                                    btnDebiCheck.Opacity = 1;
                                    btnDebiCheck.ToolTip = null;
                                }
                                else
                                {

                                    btnDebiCheck.Opacity = 0.5;
                                    btnDebiCheck.ToolTip = "Fill In Debit Day.";
                                }
                            }
                            else
                            {

                                btnDebiCheck.Opacity = 0.5;

                                btnDebiCheck.ToolTip = "Fill In Account Number.";
                            }
                        }
                        else
                        {

                            btnDebiCheck.Opacity = 0.5;
                            btnDebiCheck.ToolTip = "Fill in ID Number.";
                        }
                    }
                    else
                    {
                        btnDebiCheck.Opacity = 0.5;
                        btnDebiCheck.ToolTip = "Fill in Cell Number";
                    }
                }
                else
                {
                    btnDebiCheck.Opacity = 0.5;
                    btnDebiCheck.ToolTip = "Fill in Name.";
                }
            }
            else
            {

                btnDebiCheck.Opacity = 0.5;
                btnDebiCheck.ToolTip = "Lead Status is not a sale";
            }
        }
        private void ClosurePage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //DebiCheck Check is only for Base campaigns

            try
            {
                if (LaData.AppData.CampaignGroupType == lkpINCampaignGroupType.Base)
                {

                    //these banks dont accept DebiCheck Mandates Yet
                    if (LaData.BankDetailsData.BankID == 266 || LaData.BankDetailsData.BankID == 245 || LaData.BankDetailsData.BankID == 267)
                    {
                        btnDebiCheck.Visibility = Visibility.Collapsed;
                        DebiCheckBorder.Visibility = Visibility.Collapsed;
                        Mandate1Lbl1.Visibility = Visibility.Collapsed;
                        Mandate1TB.Visibility = Visibility.Collapsed;
                        Mandate2Lbl1.Visibility = Visibility.Collapsed;
                        Mandate2TB.Visibility = Visibility.Collapsed;
                        GotBankingDetailsPL.Visibility = Visibility.Collapsed;
                        GotBankingDetailsPLLBL.Visibility = Visibility.Collapsed;
                        GotBankingDetailsPLBTN.Visibility = Visibility.Collapsed;
                        GotBankingDetailsPLLBL2.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        if (ClosurePage.Visibility == Visibility.Visible)
                        {
                            btnDebiCheck.Visibility = Visibility.Visible;
                            DebiCheckBorder.Visibility = Visibility.Visible;
                            Mandate1Lbl1.Visibility = Visibility.Visible;
                            Mandate1TB.Visibility = Visibility.Visible;
                            Mandate2Lbl1.Visibility = Visibility.Visible;
                            Mandate2TB.Visibility = Visibility.Visible;
                            GotBankingDetailsPL.Visibility = Visibility.Collapsed;
                            GotBankingDetailsPLLBL.Visibility = Visibility.Collapsed;
                            GotBankingDetailsPLBTN.Visibility = Visibility.Collapsed;
                            GotBankingDetailsPLLBL2.Visibility = Visibility.Collapsed;

                            IsDebiCheckValid();

                            //this is for the resales campaign rule
                            if (LaData.AppData.CampaignID == 7
                                || LaData.AppData.CampaignID == 9
                                || LaData.AppData.CampaignID == 10
                                || LaData.AppData.CampaignID == 294
                                || LaData.AppData.CampaignID == 295
                                || LaData.AppData.CampaignID == 24
                                || LaData.AppData.CampaignID == 25
                                || LaData.AppData.CampaignID == 11
                                || LaData.AppData.CampaignID == 12
                                || LaData.AppData.CampaignID == 13
                                || LaData.AppData.CampaignID == 14
                                || LaData.AppData.CampaignID == 85
                                || LaData.AppData.CampaignID == 86
                                || LaData.AppData.CampaignID == 87
                                || LaData.AppData.CampaignID == 281
                                || LaData.AppData.CampaignID == 324
                                || LaData.AppData.CampaignID == 325
                                || LaData.AppData.CampaignID == 326
                                || LaData.AppData.CampaignID == 327
                                || LaData.AppData.CampaignID == 264
                                || LaData.AppData.CampaignID == 4)
                            {
                                IsDebiCheckValidForResales();

                                GotBankingDetailsPL.Visibility = Visibility.Visible;
                                GotBankingDetailsPLLBL.Visibility = Visibility.Visible;
                                GotBankingDetailsPLBTN.Visibility = Visibility.Visible;
                                GotBankingDetailsPLLBL2.Visibility = Visibility.Visible;

                                //btnDebiCheck.Visibility = Visibility.Collapsed;
                                //DebiCheckBorder.Visibility = Visibility.Collapsed;
                                //Mandate1Lbl1.Visibility = Visibility.Collapsed;
                                //Mandate1TB.Visibility = Visibility.Collapsed;
                                //Mandate2Lbl1.Visibility = Visibility.Collapsed;
                                //Mandate2TB.Visibility = Visibility.Collapsed;

                                //GotBankingDetailsPL.Visibility = Visibility.Collapsed;
                                //GotBankingDetailsPLLBL.Visibility = Visibility.Collapsed;
                                //GotBankingDetailsPLBTN.Visibility = Visibility.Collapsed;
                                //GotBankingDetailsPLLBL2.Visibility = Visibility.Collapsed;
                            }

                            //this enables the button depending on the Leadstatus the lead is saved as
                            try
                            {
                                StringBuilder strQuery = new StringBuilder();
                                strQuery.Append("SELECT TOP 1 FKINLeadStatusID [Response] ");
                                strQuery.Append("FROM INImport ");
                                strQuery.Append("WHERE ID = " + LaData.AppData.ImportID);
                                strQuery.Append(" ORDER BY ID DESC");
                                DataTable dt = Methods.GetTableData(strQuery.ToString());

                                string response = dt.Rows[0]["Response"].ToString();

                                if (response.Contains("19") || response.Contains("23"))
                                {
                                    DebiCheckBorder.BorderBrush = Brushes.White;
                                    btnDebiCheck.IsEnabled = true;
                                }
                                if (response.Contains("21"))
                                {
                                    DebiCheckBorder.BorderBrush = Brushes.White;
                                    btnDebiCheck.IsEnabled = true;
                                }
                            }
                            catch
                            {

                            }
                            //this is for when there is a bump up
                            try
                            {
                                if (chkUDMBumpUp.IsChecked == true)
                                {
                                    btnDebiCheck.IsEnabled = true;
                                }
                            }
                            catch
                            {

                            }

                        }
                        else
                        {
                            btnDebiCheck.Visibility = Visibility.Collapsed;
                            DebiCheckBorder.Visibility = Visibility.Collapsed;
                            Mandate1Lbl1.Visibility = Visibility.Collapsed;
                            Mandate1TB.Visibility = Visibility.Collapsed;
                            Mandate2Lbl1.Visibility = Visibility.Collapsed;
                            Mandate2TB.Visibility = Visibility.Collapsed;
                            GotBankingDetailsPL.Visibility = Visibility.Collapsed;
                            GotBankingDetailsPLLBL.Visibility = Visibility.Collapsed;
                            GotBankingDetailsPLBTN.Visibility = Visibility.Collapsed;
                            GotBankingDetailsPLLBL2.Visibility = Visibility.Collapsed;

                        }
                    }


                }
                else if (LaData.AppData.CampaignGroupType == lkpINCampaignGroupType.Upgrade)
                {
                    //this enables the button depending on the Leadstatus the lead is saved as
                    try
                    {
                        if (ClosurePage.Visibility == Visibility.Visible)
                        {
                            btnDebiCheck.Visibility = Visibility.Visible;
                            DebiCheckBorder.Visibility = Visibility.Visible;
                            Mandate1Lbl1.Visibility = Visibility.Visible;
                            Mandate1TB.Visibility = Visibility.Visible;
                            Mandate2Lbl1.Visibility = Visibility.Visible;
                            Mandate2TB.Visibility = Visibility.Visible;
                            GotBankingDetailsPL.Visibility = Visibility.Visible;
                            GotBankingDetailsPLLBL.Visibility = Visibility.Visible;
                            GotBankingDetailsPLBTN.Visibility = Visibility.Visible;
                            GotBankingDetailsPLLBL2.Visibility = Visibility.Visible;

                            //IsDebiCheckValid();

                            //this enables the button depending on the Leadstatus the lead is saved as
                            try
                            {
                                StringBuilder strQuery = new StringBuilder();
                                strQuery.Append("SELECT TOP 1 FKINLeadStatusID [Response] ");
                                strQuery.Append("FROM INImport ");
                                strQuery.Append("WHERE ID = " + LaData.AppData.ImportID);
                                strQuery.Append(" ORDER BY ID DESC");
                                DataTable dt = Methods.GetTableData(strQuery.ToString());

                                string response = dt.Rows[0]["Response"].ToString();

                                if (response.Contains("19"))
                                {
                                    DebiCheckBorder.BorderBrush = Brushes.White;
                                    btnDebiCheck.IsEnabled = true;
                                }
                                if (response.Contains("21"))
                                {
                                    DebiCheckBorder.BorderBrush = Brushes.White;
                                    btnDebiCheck.IsEnabled = true;
                                }
                                if (response.Contains("23"))
                                {
                                    DebiCheckBorder.BorderBrush = Brushes.White;
                                    btnDebiCheck.IsEnabled = true;
                                }
                            }
                            catch
                            {

                            }
                            //this is for when there is a bump up
                            try
                            {
                                if (chkUDMBumpUp.IsChecked == true)
                                {
                                    btnDebiCheck.IsEnabled = true;
                                }
                            }
                            catch
                            {

                            }

                        }
                        else
                        {
                            btnDebiCheck.Visibility = Visibility.Collapsed;
                            DebiCheckBorder.Visibility = Visibility.Collapsed;
                            Mandate1Lbl1.Visibility = Visibility.Collapsed;
                            Mandate1TB.Visibility = Visibility.Collapsed;
                            Mandate2Lbl1.Visibility = Visibility.Collapsed;
                            Mandate2TB.Visibility = Visibility.Collapsed;
                            GotBankingDetailsPL.Visibility = Visibility.Collapsed;
                            GotBankingDetailsPLLBL.Visibility = Visibility.Collapsed;
                            GotBankingDetailsPLBTN.Visibility = Visibility.Collapsed;
                            GotBankingDetailsPLLBL2.Visibility = Visibility.Collapsed;
                        }

                    }
                    catch
                    {

                    }
                    //this is for when there is a bump up
                    try
                    {
                        if (chkUDMBumpUp.IsChecked == true)
                        {
                            btnDebiCheck.IsEnabled = true;
                        }
                    }
                    catch
                    {

                    }
                }

                else
                {
                    btnDebiCheck.Visibility = Visibility.Collapsed;
                    DebiCheckBorder.Visibility = Visibility.Collapsed;
                    Mandate1Lbl1.Visibility = Visibility.Collapsed;
                    Mandate1TB.Visibility = Visibility.Collapsed;
                    Mandate2Lbl1.Visibility = Visibility.Collapsed;
                    Mandate2TB.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                DebiCheckBorder.BorderBrush = Brushes.White;

            }
            //thia is for Janelle Naidoo and Gizelle Frazer & Nthabiseng Dhlomo to try boost the Accepted rates
            if (GlobalSettings.ApplicationUser.ID == 2767 || GlobalSettings.ApplicationUser.ID == 6181 || GlobalSettings.ApplicationUser.ID == 2810 || GlobalSettings.ApplicationUser.ID == 1)
            {
                btnDebiCheck.IsEnabled = true;
            }


            try { GetMandateInfo(); } catch (Exception y) { GetMandateInfo(); }

        }
        private void btnAddContacts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LeadContactTracingDetailsScreen leadContactTracingDetailsScreen = new LeadContactTracingDetailsScreen();
                leadContactTracingDetailsScreen.LaData = LaData;
                leadContactTracingDetailsScreen.Show();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void GotBankingDetailsPLBTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetBankingDetailLookup();
            }
            catch
            {

            }
        }
        private void chkQuestionOne_Checked(object sender, RoutedEventArgs e)
        {
            if (chkQuestionOne.IsChecked == true)
            {
                chkQuestionTwo.IsChecked = false;
                cmbStatus.SelectedValue = (long)2; // Declined - Pre existing condition  
                //SelectDeclineReasonScreen selectDeclineReasonScreen = new SelectDeclineReasonScreen(this);  
                ////selectDeclineReasonScreen.SelectedDeclineReasonID = 26; 
                //ShowDialog(selectDeclineReasonScreen, new INDialogWindow(selectDeclineReasonScreen));     
            }
            else
            {
                chkQuestionOne.IsChecked = false;
                cmbStatus.SelectedValue = (long)-1;
            }
        }


        private void chkQuestionTwo_Checked(object sender, RoutedEventArgs e)
        {
            if (chkQuestionTwo.IsChecked == true)
            {
                chkQuestionOne.IsChecked = false;
                cmbStatus.SelectedValue = (long)2; // Declined - Pre existing condition  
                //SelectDeclineReasonScreen selectDeclineReasonScreen = new SelectDeclineReasonScreen(this);  
                ////selectDeclineReasonScreen.SelectedDeclineReasonID = 26; 
                //ShowDialog(selectDeclineReasonScreen, new INDialogWindow(selectDeclineReasonScreen));     
            }
            else
            {
                chkQuestionTwo.IsChecked = false;
                cmbStatus.SelectedValue = (long)-1;
            }
        }

        private void chkQuestionOne_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chkQuestionTwo_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chkMoveToLeadPermissions_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkMoveToLeadPermissions.IsChecked == true)
                {
                    var name = medNOKName.Text;
                    var surname = medNOKSurname.Text;
                    //string relationship = cmbNOKRelationship.SelectedValue.ToString();
                    var contact = medNOKContactPhone.Text;


                    PermissionLeadScreen permissionLeadScreen = new PermissionLeadScreen(LaData.AppData.ImportID, null, name, surname, contact, null);
                    permissionLeadScreen.medFirstName.Text = name;
                    permissionLeadScreen.medSurname.Text = surname;
                    permissionLeadScreen.medCellPhone.Text = contact;

                    ShowDialog(permissionLeadScreen, new INDialogWindow(permissionLeadScreen));




                }
                else
                {

                }

            }
            catch (Exception ex)
            {

            }

        }

        #region Signing Power

        private void SigningPower()
        {
            try
            {
                String valueSelected = cmbDOSigningPower.SelectedValue.ToString();
                if (valueSelected != null && (valueSelected == "2" || valueSelected == "3"))
                {
                    INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                    ShowMessageBox(messageWindow, "Please call the Account holder to authorize the Debi-check mandate", "Reminder - DebiCheck", ShowMessageType.Exclamation);
                }

            }
            catch (Exception ex)
            {

            }

        }

        #endregion


    }

    public class MyWebClient : WebClient
    {
        //time in milliseconds
        private int timeout;
        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        }

        public MyWebClient()
        {
            this.timeout = 180000;
        }

        public MyWebClient(int timeout)
        {
            this.timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var result = base.GetWebRequest(address);
            result.Timeout = this.timeout;
            return result;
        }
    }

}
