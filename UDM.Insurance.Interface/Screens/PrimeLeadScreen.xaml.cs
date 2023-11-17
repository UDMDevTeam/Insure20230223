using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Data;
using UDM.WPF.Library;
using static UDM.Insurance.Interface.Data.LeadApplicationData;
using Embriant.Framework.Configuration;
using Embriant.Framework;
using UDM.Insurance.Interface.Windows;
using UDM.Insurance.Business.Mapping;
using Infragistics.Windows.Editors;
using Infragistics.Windows.Editors.Events;
using System.Globalization;
using Embriant.WPF.Controls;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using UDM.Insurance.Business.Objects;
using UDM.Insurance.Interface.TempClass;
using Embriant.Framework.Data;
using Infragistics.Controls.Menus;
using System.Text.RegularExpressions;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for PrimeLeadScreen.xaml
    /// </summary>
    public partial class PrimeLeadScreen
    {
        #region BulkSMS

        //Bulk SMS variables
        private const string bulkSMSTokenName = "UDMSoftware";
        private const string bulkSMStokenID = "5DF865E90D5D4AEE9F2A6790D02959FC-01-0";
        private const string bulkSMStokenSecret = "aQ958yyEQA*v8E1sugmfMVVg!HPRO";

        private const string bulkSMSUserName = "UDMIG";
        private const string bulkSMSPassword = "Gorilla1!";
        private const string bulkSMSURL = "https://api.bulksms.com/v1";

        #endregion BulkSMS
        #region Private Members
        private string _strXpsDoc;
        private XpsDocument _xpsDocument;
        private Key _popupKey = Key.Escape;
        private readonly IEnumerable<Border> _pages;
        private readonly SalesScreenGlobalData _ssGlobalData;
        private LeadApplicationData _laData = new LeadApplicationData();

        bool LeadLoadingBool = false;
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
        private Point _posMouseVB;
        private Point _posVB;
        //private LiveProductivity _liveProductivity;
        private bool _hasNoteBeenDisplayed;
        private DependencyObject _focusScope;
        private IInputElement _focusedElement;
        //StopWatch
        private readonly Stopwatch _swNextLead = new Stopwatch();
        private readonly Stopwatch _swPrevLead = new Stopwatch();
        //Colors
        private SolidColorBrush defaultBorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF808080"));
        #endregion Private Members
        private const string IDField = "ID";
        private const string DescriptionField = "Description";
        public PrimeLeadScreen(long? importID, SalesScreenGlobalData ssGlobalData)
        {
            Methods.SetNumberFormat();
            InitializeComponent();
            _ssGlobalData = ssGlobalData;
            _ssGlobalData.PrimeLeadScreen = this;

            LoadLookupData();
            if (importID != null)
            {
                LaData.AppData.CanClientBeContacted = Insure.CanClientBeContacted(importID.Value);
                ShowOrHideFields(LaData.AppData.CanClientBeContacted);
                LoadLead(importID);
            }

        }
        public PrimeLeadScreen(long? importID, SalesScreenGlobalData ssGlobalData, bool openCMScreen) : this(importID, ssGlobalData)
        {
            if (openCMScreen)
            {
                CallMonitoringDetailsScreen callMonitoringDetailsScreen = new CallMonitoringDetailsScreen(LaData);
                IsEnabled = false;
                callMonitoringDetailsScreen.ShowDialog();
                IsEnabled = true;
            }
        }
        private void CloseScriptWindows()
        {
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                if (window.GetType().Name == "ScriptScreen")
                {
                    window.Close();
                    return;
                }
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

                LaData.Clear();
                LaData.LeadData.DateOfBirth = null;
                LaData.AppData.RefNo = strRefNo;
                LaData.AppData2.ImportIDNextLead = importIDNextLead;
                LaData.AppData2.ImportIDPrevLead = ImportIDPrevLead;

                _hasNoteBeenDisplayed = false;
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
        private void CheckSMSSent()
        {
            string smsStatusTypeString = LaData.SMSData.SMSStatusTypeID == null ? "" : Enum.GetName(typeof(WPF.Enumerations.Insure.lkpSMSStatusType), LaData.SMSData.SMSStatusTypeID);
            string smsStatusSubtypeString = LaData.SMSData.SMSStatusSubtypeID == null ? "" : Enum.GetName(typeof(WPF.Enumerations.Insure.lkpSMSStatusSubtype), LaData.SMSData.SMSStatusSubtypeID);
            LaData.SMSData.SMSToolTip = (smsStatusSubtypeString == "" ? "SENT" : smsStatusSubtypeString) + " " + LaData.SMSData.SMSSubmissionDate;
            LaData.SMSData.IsSMSSent = smsStatusTypeString != "FAILED" && smsStatusTypeString != "" && smsStatusTypeString != null ? true : false;
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
                        //CopyToBenef.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    // CopyToBenef.Visibility = Visibility.Collapsed;
                }
            }


            #endregion Check if Temp
        }
        private void UpdateLeadStatuses(long? importID)
        {
         
        }
        private void UpdateLeadApplicationScreenLookups(long? importID)
        {
            DataSet dsUpdatedLeadApplicationScreenLookups = Insure.UpdateLeadApplicationScreenLookups(importID);
            DataTable dtLA2Relationships = dsUpdatedLeadApplicationScreenLookups.Tables[0];
            // cmbLA2Relationship.Populate(dtLA2Relationships, DescriptionField, IDField);
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
        private void LoadLead(long? importID)
        {
            #region Defaults
            try
            {
                ClearApplicationScreen();
            }
            catch
            {

            }
            #endregion
            try
            {

                LeadLoadingBool = true;

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

                #region Get the complete lead from the database

                #region Refresh agent list to include agents used for this import id
                DataTable dtSMSVoucher = new DataTable();
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ImportID", importID);
                DataSet dsAgentLookups = Methods.ExecuteStoredProcedure("spINGetLeadApplicationScreenAgents", parameters);
                DataTable dtSalesAgents = dsAgentLookups.Tables[0];
                cmbAgent.Populate(dtSalesAgents, "Name", IDField);
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
                LaData.LeadData.StampDate = dtLead.Rows[0]["StampDate"] as DateTime?;
                // CheckTSANonEdit();

                #region Cancer Questions
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
                    DataTable dtCancerQuestion = Methods.GetTableData(strQueryCancerQuestion.ToString());
                    if (dtCancerQuestion.Rows.Count > 0 && LaData.AppData.CampaignID == 344)
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

                    btnSave12.IsEnabled = true;
                }
                catch (Exception ex)
                {
                }
                #endregion

                #region Hardbound Leads
                try
                {
                    if (dtLead.Rows[0]["EmailStatus"] as string == "HARD_BOUNCE")
                    {
                        lblHardBound.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        lblHardBound.Visibility = Visibility.Collapsed;
                    }
                }
                catch { lblHardBound.Visibility = Visibility.Collapsed; }
                #endregion
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


                #endregion Show the remaining sales for a particular agent and batch
                if (LaData.AppData.LeadStatus != null)
                {
                    if ((lkpINLeadStatus)LaData.AppData.LeadStatus == lkpINLeadStatus.Declined)//brigette request 2015/07/08
                    {
                        lblDateOfSale.Text = "Date Of Decline";
                        // lblSaleDate.Text = "Decline Date / Time";
                    }
                }
                cmbStatus_ToolTip(importID);


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

                    #endregion Address Mappings - removed, as requested by Brigette, 2015-03-24

                    #region Address mappings

                    #endregion Address mappings

                    LaData.LeadData.ReferrorTitleID = dtLead.Rows[0]["ReferrorTitleID"] as long?;
                    LaData.LeadData.ReferrorName = dtLead.Rows[0]["ReferrorName"] as string;
                    LaData.LeadData.ReferrorRelationshipID = dtLead.Rows[0]["ReferrorRelationshipID"] as long?;

                    try { LaData.LeadData.ReferrorContact = dtLead.Rows[0]["ReferrorContact"] as string; } catch { }
                    //LaData.LeadData.ReferrorType = dtLead.Rows[0]["ReferrorType"] as string;
                    LaData.LeadData.Email = LaData.LeadHistoryData.Email = dtLead.Rows[0]["EMail"] as string;
                    if (dtLeadHst.Rows.Count > 0) LaData.LeadHistoryData.Email = dtLeadHst.Rows[0]["EMail"] as string;
                }
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
                #region ImportExtra

                Business.INImportExtra inImportExtra = INImportExtraMapper.SearchOne(importID, null, null, null, null, null, null, null, null);
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
                LaData.AppData.ImportID = importID;
                StringBuilder strQueryPrimeLeadExsists = new StringBuilder();
                strQueryPrimeLeadExsists.Append("SELECT [DateOfStatus], [FKlkpPrimedLeadStatusID] ");
                strQueryPrimeLeadExsists.Append("FROM [INPrimedLeads] ");
                strQueryPrimeLeadExsists.AppendFormat("WHERE [FKINImportID] = '{0}'", LaData.AppData.ImportID);
                DataTable dtPrime = Methods.GetTableData(strQueryPrimeLeadExsists.ToString());
                if (dtPrime.Rows.Count > 0)
                {
                    dteDateOfSale.Text = dtPrime.Rows[0]["DateOfStatus"].ToString();
                   cmbStatus.SelectedValue = dtPrime.Rows[0]["FKlkpPrimedLeadStatusID"] as long?;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                LaData.AppData.ImportID = null;
                LaData.AppData.IsLeadLoaded = false;
                _flagLeadIsBusyLoading = false;
                LeadLoadingBool = false;
            }
            finally
            {
                try
                {
                    SetCursor(Cursors.Arrow);
                }
                catch { }
                medReference.Focus();
                //_flagLeadIsBusyLoading = false;
                SetPolicyDefaults();
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
            }
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
            #endregion Page 1 Controls
            btnSave12.Visibility = visibility;
            if (show)
            {
                FocusIDNumberField();
            }
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
        private void FocusPassportNumberField()
        {
            medIDNumber.Visibility = Visibility.Hidden;
            medPassportNumber.Visibility = Visibility.Visible;
            lblID.TextDecorations = null;
            lblPassport.TextDecorations = TextDecorations.Underline;
            medPassportNumber.Focus();
            medPassportNumber.Tag = null;
        }
        private void ShowIDNumberField()
        {
            medIDNumber.Visibility = Visibility.Visible;
            medPassportNumber.Visibility = Visibility.Hidden;
            lblID.TextDecorations = TextDecorations.Underline;
            lblPassport.TextDecorations = null;
            medIDNumber.Tag = null;
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
                               // lblPage.Text = "(Upgrade)";
                            }
                            break;
                    }
                    break;
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
                        // CancerOptionSpouseWorking(xme.Text);
                        break;
                    case "medLA2IDNumber":
                        if (!LaData.AppData.IsLeadUpgrade) LaData.LAData[1].DateOfBirth = IDNumberToDOB(xme.Text);
                        break;

                }
            }
        }
        private void cmbStatus_DropDownClosed(object sender, EventArgs e)
        {
            if (LaData.AppData.ImportID != null && cmbStatus.SelectedValue != null && !LaData.AppData.DiaryStatusHandled)
            {
                if ((lkpINLeadStatus)((long?)cmbStatus.SelectedValue) == lkpINLeadStatus.Diary)
                {

                }
            }
        }
        private void cmbStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void cmbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LaData.AppData.IsLeadLoaded) //LaData.AppData.ImportID != null && e.AddedItems.Count > 0
            {
                var Result = cmbStatus.SelectedItem;
                switch (LaData.AppData.lkpLeadStatus)
                {
                    #region Sales

                    case lkpINLeadStatus.Accepted:
                        //auto populate date and time  
                        DateTime saleDate = DateTime.Now;
                        lblDateOfSale.Text = "Date Of Sale";
                        string time = saleDate.TimeOfDay.Hours + ":" + saleDate.TimeOfDay.Minutes;

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
                        //SelectCancellationReasonScreen selectCancelllationReasonScreen = new SelectCancellationReasonScreen(this);
                        //selectCancelllationReasonScreen.SelectedCancellationReasonID = LaData.AppData.CancellationReasonID;
                        //ShowDialog(selectCancelllationReasonScreen, new INDialogWindow(selectCancelllationReasonScreen));
                        //LaData.AppData.CancellationReasonID = selectCancelllationReasonScreen.SelectedCancellationReasonID;

                        //if (LaData.AppData.CancellationReasonID == null)
                        //{
                        //    cmbStatus.SelectedIndex = -1;
                        //}



                        Methods.FindChild<TextBox>(medReference, "PART_InputTextBox").Focus();

                        cmbStatus_ToolTip(null);
                        //cmbSalesNotTransferredReasons.Visibility = Visibility.Collapsed;
                        //lblSalesNotTransferredReasons.Visibility = Visibility.Collapsed;
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
                        //SelectCarriedForwardReasonScreen selectCarriedForwardReasonScreen = new SelectCarriedForwardReasonScreen(this);
                        //selectCarriedForwardReasonScreen.SelectedCarriedForwardReasonID = LaData.AppData.CarriedForwardReasonID;
                        //ShowDialog(selectCarriedForwardReasonScreen, new INDialogWindow(selectCarriedForwardReasonScreen));
                        //LaData.AppData.CarriedForwardReasonID = selectCarriedForwardReasonScreen.SelectedCarriedForwardReasonID;

                        //if (LaData.AppData.CarriedForwardReasonID == null)
                        //{
                        //    cmbStatus.SelectedIndex = -1;
                        //}

                        ////chkUDMBumpUp.IsChecked = false;
                        ////chkReducedPremium.IsChecked = false;

                        Methods.FindChild<TextBox>(medReference, "PART_InputTextBox").Focus();

                        cmbStatus_ToolTip(null);

                        //cmbSalesNotTransferredReasons.Visibility = Visibility.Collapsed;
                        //lblSalesNotTransferredReasons.Visibility = Visibility.Collapsed;
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

                        //LaData.AppData.DeclineReasonID = null;
                        //SelectDeclineReasonScreen selectDeclineReasonScreen = new SelectDeclineReasonScreen(this);
                        //selectDeclineReasonScreen.SelectedDeclineReasonID = LaData.AppData.DeclineReasonID;
                        //ShowDialog(selectDeclineReasonScreen, new INDialogWindow(selectDeclineReasonScreen));

                        //LaData.AppData.DeclineReasonID = selectDeclineReasonScreen.SelectedDeclineReasonID;

                        //if (LaData.AppData.DeclineReasonID == null)
                        //{
                        //    cmbStatus.SelectedIndex = -1;
                        //}

                        Methods.FindChild<TextBox>(medReference, "PART_InputTextBox").Focus();

                        //cmbSalesNotTransferredReasons.Visibility = Visibility.Collapsed;
                        //lblSalesNotTransferredReasons.Visibility = Visibility.Collapsed;
                        break;

                    #endregion Declines

                    #region Forward To Call Monitoring Agent
                    case lkpINLeadStatus.ForwardToCMAgent:
                        //see if this can be selected

                        cmbStatus.SelectedIndex = -1;
                        break;

                    #endregion

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

                        //cmbSalesNotTransferredReasons.Visibility = Visibility.Collapsed;
                        //lblSalesNotTransferredReasons.Visibility = Visibility.Collapsed;
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
                                    //cmbSalesNotTransferredReasons.Visibility = Visibility.Collapsed;
                                    //lblSalesNotTransferredReasons.Visibility = Visibility.Collapsed;
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
                        //cmbSalesNotTransferredReasons.Visibility = Visibility.Collapsed;
                        //lblSalesNotTransferredReasons.Visibility = Visibility.Collapsed;
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
                        //SelectCallMonitoringCancellationReasonScreen selectCallMonitoringCancellationReasonScreen = new SelectCallMonitoringCancellationReasonScreen(this);
                        //selectCallMonitoringCancellationReasonScreen.CallMonitoringCancellationReasonID = LaData.AppData.FKINCallMonitoringCancellationReasonID;
                        //ShowDialog(selectCallMonitoringCancellationReasonScreen, new INDialogWindow(selectCallMonitoringCancellationReasonScreen));
                        //LaData.AppData.FKINCallMonitoringCancellationReasonID = selectCallMonitoringCancellationReasonScreen.CallMonitoringCancellationReasonID;

                        //if (LaData.AppData.FKINCallMonitoringCancellationReasonID == null)
                        //{
                        //    cmbStatus.SelectedIndex = -1;
                        //}

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
                        //SelectCallMonitoringCarriedForwardReasonScreen selectCallMonitoringCarriedForwardReasonScreen = new SelectCallMonitoringCarriedForwardReasonScreen(this);
                        //selectCallMonitoringCarriedForwardReasonScreen.SelectCallMonitoringCarriedForwardReasonID = LaData.AppData.FKINCallMonitoringCarriedForwardReasonID;
                        //ShowDialog(selectCallMonitoringCarriedForwardReasonScreen, new INDialogWindow(selectCallMonitoringCarriedForwardReasonScreen));
                        //LaData.AppData.FKINCallMonitoringCarriedForwardReasonID = selectCallMonitoringCarriedForwardReasonScreen.SelectCallMonitoringCarriedForwardReasonID;

                        if (LaData.AppData.FKINCallMonitoringCarriedForwardReasonID == null)
                        {
                            cmbStatus.SelectedIndex = -1;
                        }

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
        private bool IsValidData()
        {
            try
            {
                //validation: also done in xaml save button style
                if (LaData.AppData.LeadStatus == 1)
                {
                    #region Beneficiaries Percentages Total


                    #endregion Beneficiaries Percentages Total

                    #region Rule: Any option that includes Funeral cover () LA1 Funeral cover or LA1 Accidental Death cover (Upgrades) must have a beneficiary percentage of 100%
                    decimal beneficiaryTotalPercentage = 0;

                    #endregion Rule: Any option that includes Funeral cover () LA1 Funeral cover or LA1 Accidental Death cover (Upgrades) must have a beneficiary percentage of 100%
                }
                #region Lead Feedback
                {

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
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            #region INPermission Pop up message

            if ((lkpUserType?)((Business.User)GlobalSettings.ApplicationUser).FKUserType == lkpUserType.ConfirmationAgent && (lkpINLeadStatus?)LaData.AppData.LeadStatus == lkpINLeadStatus.Accepted && LaData.AppData.IsConfirmed == false)
            {
                if (LaData.SaleData.ConfCallRefID == null)
                {
                    LaData.SaleData.ConfCallRefID = LaData.SaleData.AutoPopulateConfirmationAgentFKUserID;
                }

            }
#if !TRAININGBUILD
            if (LaData.UserData.UserType == lkpUserType.SalesAgent)
            {

                LaData.AppData.AgentID = LaData.UserData.UserID;
            }
#endif
            if (LaData.UserData.UserType == lkpUserType.SalesAgent)
            {
                if (LaData.AppData.IsLeadUpgrade == true)
                {
                }
                else
                {
                    if (LaData.AppData.LeadStatus == 1 || LaData.AppData.LeadStatus == 24)
                    {
                        if (LaData.BankDetailsData.SigningPowerID == null)
                        {
                            INMessageBoxWindow1 mbw = new INMessageBoxWindow1();
                            string message = "Signing Power not selected.";
                            ShowMessageBox(mbw, message, "Lead not Saved", ShowMessageType.Error);
                            return;
                        }
                    }

                }
            }
            if (IsValidData())
            {
                if ((lkpINLeadStatus?)LaData.AppData.LeadStatus == lkpINLeadStatus.DoNotContactClient)
                {
                    string message;
                    if ((lkpUserType?)((Business.User)GlobalSettings.ApplicationUser).FKUserType == lkpUserType.SalesAgent)
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

        }
        private void Save()
        {
            try
            {
                SetCursor(Cursors.Wait);
                if (string.IsNullOrEmpty(medName.Text) || string.IsNullOrEmpty(medSurname.Text) || cmbGender.SelectedIndex == -1 || string.IsNullOrEmpty(medCellPhone.Text))
                {
                    //Add Popup 
                    INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                    ShowMessageBox(messageWindow, "Please fill in the required information.", "Fill In Information", ShowMessageType.Exclamation);
                }
                else
                {
                    //do the save 
                    StringBuilder strQueryPrimeLeadExsists = new StringBuilder();
                    strQueryPrimeLeadExsists.Append("SELECT * ");
                    strQueryPrimeLeadExsists.Append("FROM [INPrimedLeads] ");
                    strQueryPrimeLeadExsists.AppendFormat("WHERE [FKINImportID] = '{0}'", LaData.AppData.ImportID);
                    DataTable dtPrime = Methods.GetTableData(strQueryPrimeLeadExsists.ToString());
                    LaData.AppData.AgentID = LaData.UserData.UserID;
                    if (dtPrime.Rows.Count <= 0)
                    {
                        if (LaData.AppData.ImportID.HasValue)
                        {
                            
                            INImport import = new INImport((long)LaData.AppData.ImportID);

                            import.FKUserID = LaData.AppData.AgentID;
                            import.StampDate = DateTime.Now;
                            import.Save(_validationResult);
                        }

                        if (LaData.LeadData.LeadID.HasValue)
                        {
                            INLead lead = new INLead((long)LaData.LeadData.LeadID);
                            var selectedTitleRow = cmbTitle.SelectedItem as DataRowView;
                            var selectedGenderRow = cmbGender.SelectedItem as DataRowView;
                            lead.FKINTitleID = (long)selectedTitleRow[0];
                            lead.Initials = medInitials.Text;
                            lead.FirstName = UppercaseFirst(medName.Text);
                            lead.Surname = UppercaseFirst(medSurname.Text);
                            lead.IDNo = UppercaseFirst(medIDNumber.Text);
                            lead.PassportNo = UppercaseFirst(medPassportNumber.Text);
                            lead.DateOfBirth = LaData.LeadData.DateOfBirth;
                            lead.FKGenderID = (long)selectedGenderRow[0];
                            lead.TelHome = medHomePhone.Text;
                            lead.TelCell = LaData.LeadData.TelCell;
                            lead.TelWork = medWorkPhone.Text;
                            lead.TelOther = medOtherPhone.Text;
                            lead.Address1 = UppercaseFirst(medAddress1.Text);
                            lead.Address2 = UppercaseFirst(medAddress2.Text);
                            lead.Address3 = UppercaseFirst(medAddress3.Text);
                            lead.Address4 = UppercaseFirst(medSuburb.Text);
                            lead.Address5 = UppercaseFirst(medTown.Text);
                            lead.PostalCode = medPostalCode.Text;
                            lead.Email = medEMail.Text;
                            lead.Save(_validationResult);
                        }
                        else
                        {
                            throw new Exception("leadID Error");
                        }
                        try
                        {
                            var selectedStatusRow = cmbStatus.SelectedItem as DataRowView;

                            StringBuilder strInsertPrimeLead = new StringBuilder();
                            strInsertPrimeLead.Append("INSERT INTO [INPrimedLeads] ");
                            strInsertPrimeLead.Append("([FKlkpPrimedLeadStatusID], [DateOfStatus], [FKINImportID],[StampUserID]) ");
                            strInsertPrimeLead.AppendFormat("VALUES ('{0}', '{1}', '{2}', '{3}')",
                                                            (long)selectedStatusRow[0],
                                                            DateTime.Now,
                                                            LaData.AppData.ImportID,
                                                            GlobalSettings.ApplicationUser.ID);

                            Methods.ExecuteSQLNonQuery(strInsertPrimeLead.ToString());
                            INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                            ShowMessageBox(messageWindow, "Saving this lead has been successful!", "Save Successful", ShowMessageType.Information);

                            ClearApplicationScreen();
                        } catch { }
                    }
                    else
                    {
                        if (LaData.AppData.ImportID.HasValue)
                        {
                            INImport import = new INImport((long)LaData.AppData.ImportID);
                            LaData.AppData.AgentID = LaData.UserData.UserID;
                            import.FKUserID = LaData.AppData.AgentID;
                            import.Save(_validationResult);
                        }

                        if (LaData.LeadData.LeadID.HasValue)
                        {
                            INLead lead = new INLead((long)LaData.LeadData.LeadID);
                            var selectedTitleRow = cmbTitle.SelectedItem as DataRowView;
                            var selectedGenderRow = cmbGender.SelectedItem as DataRowView;
                            lead.FKINTitleID = (long)selectedTitleRow[0];
                            lead.Initials = medInitials.Text;
                            lead.FirstName = UppercaseFirst(medName.Text);
                            lead.Surname = UppercaseFirst(medSurname.Text);
                            lead.IDNo = UppercaseFirst(medIDNumber.Text);
                            lead.PassportNo = UppercaseFirst(medPassportNumber.Text);
                            lead.DateOfBirth = LaData.LeadData.DateOfBirth;
                            lead.FKGenderID = (long)selectedGenderRow[0];
                            lead.TelHome = medHomePhone.Text;
                            lead.TelCell = LaData.LeadData.TelCell;
                            lead.TelWork = medWorkPhone.Text;
                            lead.TelOther = medOtherPhone.Text;
                            lead.Address1 = UppercaseFirst(medAddress1.Text);
                            lead.Address2 = UppercaseFirst(medAddress2.Text);
                            lead.Address3 = UppercaseFirst(medAddress3.Text);
                            lead.Address4 = UppercaseFirst(medSuburb.Text);
                            lead.Address5 = UppercaseFirst(medTown.Text);
                            lead.PostalCode = medPostalCode.Text;
                            lead.Email = medEMail.Text;
                            lead.Save(_validationResult);
                        }
                        else
                        {
                            throw new Exception("leadID Error");
                        }
                        if(cmbStatus.SelectedIndex != -1) { 
                        try
                        {
                                var selectedStatusRow = cmbStatus.SelectedItem as DataRowView;
                                if (dtPrime.Rows[0]["FKlkpPrimedLeadStatusID"] as long? != (long)selectedStatusRow[0])
                                {
                                    //Add History:
                                    StringBuilder strInsertPrimeLeadHist = new StringBuilder();
                                    strInsertPrimeLeadHist.Append("INSERT INTO [zHstPrimedLeads] ");
                                    strInsertPrimeLeadHist.Append("([FKINImportID],[FKlkpPrimedLeadStatusID], [DateOfStatus], [StampUserID]) ");
                                    strInsertPrimeLeadHist.AppendFormat("VALUES ('{0}', '{1}', '{2}','{3}')", dtPrime.Rows[0]["FKINImportID"], dtPrime.Rows[0]["FKlkpPrimedLeadStatusID"], dtPrime.Rows[0]["DateOfStatus"], dtPrime.Rows[0]["StampUserID"]);
                                    Methods.ExecuteSQLNonQuery(strInsertPrimeLeadHist.ToString());



                                    StringBuilder strUpdatePrimeLead = new StringBuilder();
                                    strUpdatePrimeLead.Append("UPDATE [INPrimedLeads] ");
                                    strUpdatePrimeLead.Append("SET ");
                                    strUpdatePrimeLead.AppendFormat("[FKlkpPrimedLeadStatusID] = '{0}', ", (long)selectedStatusRow[0]);
                                    strUpdatePrimeLead.AppendFormat("[DateOfStatus] = '{0}', ", DateTime.Now);
                                    strUpdatePrimeLead.AppendFormat("[StampUserID] = '{0}' ", GlobalSettings.ApplicationUser.ID);
                                    strUpdatePrimeLead.AppendFormat("WHERE [FKINImportID] = '{0}'", LaData.AppData.ImportID);
                                    Methods.ExecuteSQLNonQuery(strUpdatePrimeLead.ToString());
                                }
                                else
                                {
                                    //Add History:
                                    StringBuilder strInsertPrimeLeadHist = new StringBuilder();
                                    strInsertPrimeLeadHist.Append("INSERT INTO [zHstPrimedLeads] ");
                                    strInsertPrimeLeadHist.Append("([FKINImportID],[FKlkpPrimedLeadStatusID], [DateOfStatus], [StampUserID]) ");
                                    strInsertPrimeLeadHist.AppendFormat("VALUES ('{0}', '{1}', '{2}','{3}')", dtPrime.Rows[0]["FKINImportID"], dtPrime.Rows[0]["FKlkpPrimedLeadStatusID"], dtPrime.Rows[0]["DateOfStatus"], dtPrime.Rows[0]["StampUserID"]);
                                    Methods.ExecuteSQLNonQuery(strInsertPrimeLeadHist.ToString());



                                    StringBuilder strUpdatePrimeLead = new StringBuilder();
                                    strUpdatePrimeLead.Append("UPDATE [INPrimedLeads] ");
                                    strUpdatePrimeLead.Append("SET ");
                                    strUpdatePrimeLead.AppendFormat("[FKlkpPrimedLeadStatusID] = '{0}', ", (long)selectedStatusRow[0]);
                                    strUpdatePrimeLead.AppendFormat("[StampUserID] = '{0}' ", GlobalSettings.ApplicationUser.ID);
                                    strUpdatePrimeLead.AppendFormat("WHERE [FKINImportID] = '{0}'", LaData.AppData.ImportID);
                                    Methods.ExecuteSQLNonQuery(strUpdatePrimeLead.ToString());
                                }
                            INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                            ShowMessageBox(messageWindow, "Saving this lead has been successful!", "Save Successful", ShowMessageType.Information);

                            ClearApplicationScreen();

                        } catch (Exception ex)
                        {

                        }
                        }
                        else
                        {
                            INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                            ShowMessageBox(messageWindow, "Saving this lead has been successful!", "Save Successful", ShowMessageType.Information);

                        }
                    }
                }
            } catch (Exception ex)
            {
                Database.CancelTransactions();
                HandleException(ex);
                medReference.Focus();
            }
            finally
            {
                SetCursor(Cursors.Arrow);
            }
        }
        public static string UppercaseFirst(string s)
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

                            #endregion Beneficiary Titles
                    }
                    break;
            }
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
        private void IDConfirmedCB_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LeadLoadingBool == false)
                {
                    string IsSavedString;
                    try
                    {
                        DataTable dtIsSaved = Methods.GetTableData(string.Format("SELECT [ID] FROM [INIDConfirmed] WHERE [FKImportID] = {0}", LaData.AppData.ImportID));
                        IsSavedString = dtIsSaved.Rows[0]["ID"].ToString();
                    }
                    catch
                    {
                        IsSavedString = null;
                    }
                    if (IsSavedString == null || IsSavedString == "")
                    {
                        INIDConfirmed con = new INIDConfirmed();
                        con.FKImportID = LaData.AppData.ImportID;
                        con.Confirmed = "1";
                        con.Save(_validationResult);
                    }
                    else
                    {
                        INIDConfirmed con = new INIDConfirmed(long.Parse(IsSavedString));
                        con.FKImportID = LaData.AppData.ImportID;
                        con.Confirmed = "1";
                        con.Save(_validationResult);
                    }
                }
            }
            catch
            {

            }
        }
        private void IDConfirmedCB_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LeadLoadingBool == false)
                {
                    string IsSavedString;
                    try
                    {
                        DataTable dtIsSaved = Methods.GetTableData(string.Format("SELECT [ID] FROM [INIDConfirmed] WHERE [FKImportID] = {0}", LaData.AppData.ImportID));
                        IsSavedString = dtIsSaved.Rows[0]["ID"].ToString();
                    }
                    catch
                    {
                        IsSavedString = null;
                    }
                    if (IsSavedString == null || IsSavedString == "")
                    {
                        INIDConfirmed con = new INIDConfirmed();
                        con.FKImportID = LaData.AppData.ImportID;
                        con.Confirmed = "0";
                        con.Save(_validationResult);
                    }
                    else
                    {
                        INIDConfirmed con = new INIDConfirmed(long.Parse(IsSavedString));
                        con.FKImportID = LaData.AppData.ImportID;
                        con.Confirmed = "0";
                        con.Save(_validationResult);
                    }
                }
            }
            catch
            {

            }
        }
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
        private void dteDateOfBirth_Loaded(object sender, RoutedEventArgs e)
        {
            //dteDateOfBirth.Value = IDNumberToDOB(medIDNumber.Text);
        }
        private void medInitials_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            if (medInitials.Text != null) medInitials.Text = medInitials.Text.ToUpper();
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
                    else if (date.Year == 2021 && date.Month == 11 && (date.Day >= 24 && date.Day <= 30))
                    {
                        LaData.PolicyData.CommenceDate = new DateTime(2022, 02, 01);
                    }
                    else if (date.Year == 2022 && date.Month == 11 && (date.Day >= 20 && date.Day <= 30))
                    {
                        LaData.PolicyData.CommenceDate = new DateTime(2023, 02, 01);
                    }
                    else if (date.Year == 2023 && date.Month == 11 && (date.Day >= 20 && date.Day <= 30))
                    {
                        LaData.PolicyData.CommenceDate = new DateTime(2024, 02, 01);
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
                //  if (LaData.AppData.IsLeadUpgrade) CalculateCostUpgrade(false); else CalculateCost(false);
            }
        }
        private void cmbAgent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LaData.SaleData.SaleCallRefID = LaData.AppData.AgentID;
            LaData.ImportExtraData.Extension1 = GetUserExtension(LaData.AppData.AgentID);
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
                            //xamMEDControl.Text = "0000000000000";
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
        public void NotifyUserIfClientCannotBeContacted(bool canClientBeContacted)
        {
            if (!canClientBeContacted)
            {
                INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
                ShowMessageBox(messageWindow, @"This lead cannot be loaded in its entirety, because the client has requested not to be contacted again.", "DO NOT CONTACT CLIENT", ShowMessageType.Exclamation);
            }
        }
        private void ShowNotes(long fkINImportID)
        {
            #region Display the Notes screen

            if (!_hasNoteBeenDisplayed)
            {
                NoteScreen noteScreen = new NoteScreen(fkINImportID /*, NoteScreen.NoteScreenMode.ReadOnly*/);
                if (noteScreen.HasNotes)
                {
                    ShowDialog(this, new INDialogWindow(noteScreen));
                }
            }

            #endregion Display the Notes screen
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
                                            break;
                                        }

                                        #endregion Determining whether or not the lead was allocated

                                        #region Determining whether or not the lead has a status of cancelled
                                        // Please see https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/204734160/comments
                                        // Please see https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/211742618/comments

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

        private void medReference_Loaded(object sender, RoutedEventArgs e)
        {
            Methods.FindChild<TextBox>(medReference, "PART_InputTextBox").Focus();
        }
        private void LoadLookupData()
        {
            try
            {
                try
                {
                    SetCursor(Cursors.Wait);
                }
                catch
                {
                }
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@UserTypeID", LaData.UserData.UserTypeID);
                DataSet dsLookups = Methods.ExecuteStoredProcedure("spINGetLeadApplicationScreenLookupsPrime", parameters);
                DataTable dtStatus = dsLookups.Tables[0];
                cmbStatus.Populate(dtStatus, DescriptionField, IDField);
                parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ImportID", (object)0);
                DataSet dsAgentLookups = Methods.ExecuteStoredProcedure("spINGetLeadApplicationScreenAgents", parameters);
                DataTable dtSalesAgents = dsAgentLookups.Tables[0];
                cmbAgent.Populate(dtSalesAgents, "Name", IDField);
                DataTable dtTitles = dsLookups.Tables[2];
                cmbTitle.Populate(dtTitles, DescriptionField, IDField);

                cmbReferrorTitle.Populate(dtTitles, DescriptionField, IDField);
                DataTable dtGender = dsLookups.Tables[7];
                cmbGender.Populate(dtGender, DescriptionField, IDField);
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

        private void XamMenuItem_Click()
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
                                        foreach (Window window in System.Windows.Application.Current.Windows)
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
                            foreach (Window window in System.Windows.Application.Current.Windows)
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
                            foreach (Window window in System.Windows.Application.Current.Windows)
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
                            foreach (Window window in System.Windows.Application.Current.Windows)
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
                            foreach (Window window in System.Windows.Application.Current.Windows)
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

                        #region Debi-check Overtime    

                        case "Debi-check Agent Overtime":
                            foreach (Window window in System.Windows.Application.Current.Windows)
                            {
                                if (window.Title == "Debi-check Agent Overtime")
                                {
                                    window.WindowState = WindowState.Normal;
                                    return;
                                }
                            }
                            ScriptScreen scriptScreenDebiCheckOvertime = new ScriptScreen();
                            scriptScreenDebiCheckOvertime.chkAfrikaans.Visibility = Visibility.Hidden;
                            scriptScreenDebiCheckOvertime.lblAfrikaans.Visibility = Visibility.Hidden;
                            scriptScreenDebiCheckOvertime.Title = "Debi-check Agent Overtime";
                            scriptScreenDebiCheckOvertime.hdrScriptScreen.Text = "Debi-check Agent Overtime";
                            scriptScreenDebiCheckOvertime.ScriptType = lkpScriptType.DebiCheckOvertime;
                            scriptScreenDebiCheckOvertime.LaData = LaData;
                            scriptScreenDebiCheckOvertime.Show();
                            break;

                        #endregion Debi-check Overtime

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
                            foreach (Window window in System.Windows.Application.Current.Windows)
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
        private void CtrlLeadApplicationScreen_Close(BaseControl nextScreen)
        {
            _ssGlobalData.PrimeLeadScreen = null;
        }
        private void btnPrevLead_Click(object sender, RoutedEventArgs e)
        {
            _swPrevLead.Reset();
            _swPrevLead.Start();
            // Clear the dictionary
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
        private void chkQuestionTwo_Checked(object sender, RoutedEventArgs e)
        {
            if (chkQuestionTwo.IsChecked == true)
            {
                chkQuestionOne.IsChecked = false;
                cmbStatus.SelectedValue = (long)2; // Declined - Pre existing condition  
              
            }
            else
            {
                chkQuestionTwo.IsChecked = false;
                cmbStatus.SelectedValue = (long)-1;
            }
        }
        private void btnPermissionLead_Click(object sender, RoutedEventArgs e)
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
        private void CtrlLeadApplicationScreen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (LaData.AppData.IsLeadUpgrade)
            {
                medReference.Focus();
                //lblPage.Text = "(Upgrade)";
            }
        }
        private void CtrlLeadApplicationScreen_Loaded(object sender, RoutedEventArgs e)
        {

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
        private void CtrlLeadApplicationScreen_KeyUp(object sender, KeyEventArgs e)
        {
          
        }
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
        private void chkQuestionOne_Unchecked(object sender, RoutedEventArgs e)
        {

        }
        private void chkQuestionTwo_Unchecked(object sender, RoutedEventArgs e)
        {

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
        private void lblID_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FocusIDNumberField();
        }
        private void lblPassport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FocusPassportNumberField();
        }
        private void cmbTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            // Update the title field in banking details
            //if (this.cmbTitle.SelectedItem != null)
            //    this.cmbDOTitle.SelectedIndex = this.cmbTitle.SelectedIndex;
        }
        private void SelectIDBtn_Click(object sender, RoutedEventArgs e)
        {
           // SelectIDNumber selectidnumber = new SelectIDNumber(this, IDNumberPO);
          //  ShowDialog(selectidnumber, new INDialogWindow(selectidnumber));
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
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            var menuToolsScreen = new MenuToolsScreen(ScreenDirection.Reverse);
            OnClose(menuToolsScreen);
        }

        private void xamEditor_Loaded(object sender, RoutedEventArgs e)
        {

        }
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
            
           
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void XamContextMenu_ItemClicked(object sender, ItemClickedEventArgs e)
        {

        }
    }
}
#endregion