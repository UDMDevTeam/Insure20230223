using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using Embriant.WPF.Controls;
using Infragistics.Windows.Editors;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.Data;
using UDM.WPF.Library;
using System.Linq;
using UDM.WPF.Classes;
using Embriant.Framework.Configuration;
using System.ComponentModel;
using System.Collections.Generic;

namespace UDM.Insurance.Interface.Screens
{
    public class StandardNote
    {
        public string Title { get; set; }
        public bool IsSelected { get; set; }
    }


    public class ContactNumber
    {
        public string ID {get; set; }
        public string TelNumber { get; set; }
    }


    public partial class CallMonitoringDetailsScreen
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region Constants



        #endregion

        #region Private Members

        private CallMonitoringData _screenData = new CallMonitoringData();
        private CallMonitoringData _loadedScreenData = new CallMonitoringData();
        private DataTable _dtAllBankBranches;
        private DataTable _dtAllBankAccountNumberPatterns;

        private ObservableNodeList itemSourceStandardNotes = new ObservableNodeList();

        private long userID;

        private lkpUserType? _userType;
        public lkpUserType? UserType
        {
            get { return _userType; }
            set { _userType = value; OnPropertyChanged("UserType"); }
        }

        private string _saleDetailNotes;
        public string SaleDetailNotes
        {
            get { return _saleDetailNotes; }
            set { _saleDetailNotes = value; OnPropertyChanged("SaleDetailNotes"); }
        }


        //private LeadApplicationData _leadApplicationScreenData;

        #endregion Private Members

        #region Publicly-Exposed Properties

        public CallMonitoringData ScreenData
        {
            get { return _screenData; }
            set { _screenData = value; }
        }

        public CallMonitoringData LoadedScreenData
        {
            get { return _loadedScreenData; }
            set { _loadedScreenData = value; }
        }

        //public LeadApplicationData LeadApplicationScreenData
        //{
        //    get { return _leadApplicationScreenData; }
        //    set { _leadApplicationScreenData = value; }
        //}

        #endregion Publicly-Exposed Properties

        #region Constructors

        public CallMonitoringDetailsScreen(LeadApplicationData leadApplicationData)
        {
            InitializeComponent();

            //_leadApplicationScreenData = leadApplicationData;

            userID = ((User)GlobalSettings.ApplicationUser).ID;
            UserType = (lkpUserType?)((User)GlobalSettings.ApplicationUser).FKUserType;

            LoadLookupData();
            LoadScreenData(leadApplicationData);
            SaleDetailNotes = leadApplicationData.SaleData.Notes;

            if(userID == 2857 || userID == 2810 || userID == 394)
            {
                chkTSRBUSavedCF.IsEnabled = true;
            }



            //var dataSource = new List<StandardNote>();
            //dataSource.Add(new StandardNote() { Title = "123", IsSelected = false });
            //dataSource.Add(new StandardNote() { Title = "456", IsSelected = false });
            //dataSource.Add(new StandardNote() { Title = "789", IsSelected = false });

            //cmbStandardNotes.ItemsSource = dataSource;

        }

        #endregion Constructors

        #region Private Methods

        private void LoadLookupData()
        {

            DataSet dsCallMonitoringScreenLookups = Insure.INGetCallMonitoringScreenLookups();
            cmbCallMonitoringOutcomeID.Populate(dsCallMonitoringScreenLookups.Tables[0], "Description", "ID");
            cmbCallMonitoringUser.Populate(dsCallMonitoringScreenLookups.Tables[1], "CallMonitoringUser", "ID");
            cmbCallAssessmentOutcome.Populate(dsCallMonitoringScreenLookups.Tables[2], "Description", "ID");
            cmbSecondaryCallMonitoringUser.Populate(dsCallMonitoringScreenLookups.Tables[3], "CallOverAssessor", "ID");
            cmbBank.Populate(dsCallMonitoringScreenLookups.Tables[4], "Description", "ID");
            cmbAccountType.Populate(dsCallMonitoringScreenLookups.Tables[5], "Description", "ID");
            _dtAllBankBranches = dsCallMonitoringScreenLookups.Tables[6];
            _dtAllBankAccountNumberPatterns = dsCallMonitoringScreenLookups.Tables[7];
            cmbTertiaryCallMonitoringUser.Populate(dsCallMonitoringScreenLookups.Tables[8], "CallOverAssessor", "ID");
            SetStandardNotes(null);





            //ObservableNodeList itemSource = new ObservableNodeList();

            //itemSource.Add(new Node("123") { IsSelected = false });
            //itemSource.Add(new Node("456") { IsSelected = false });
            //itemSource.Add(new Node("789") { IsSelected = false });







            //if (fkINImportID.HasValue)
            //{
            //    DataSet dsLookups = Insure.INGetRedeemGiftScreenLookups(fkINImportID.Value);
            //    cmbGiftStatus.Populate(dsLookups.Tables[0], "Description", "ID");
            //    cmbGiftSelection.Populate(dsLookups.Tables[1], "Gift", "ID");
            //}
        }

        private void SetStandardNotes(long? importID)
        {
            if (importID == null)
            {
                DataTable dtStandardNotes = Insure.INGetCallMonitoringStandardNotes();

                foreach (DataRow note in dtStandardNotes.Rows)
                {
                    itemSourceStandardNotes.Add(new Node(note["Description"].ToString()) { IsSelected = false, ID = Convert.ToInt64(note["ID"]) });
                }

            }
            else if (importID != null)
            {
                DataTable dtSelectedNotes = Insure.GetCMSelectedNotes(importID);
                if (dtSelectedNotes.Rows.Count > 0)
                {
                    foreach (DataRow note in dtSelectedNotes.Rows)
                    {
                        
                        if (note["ID"].ToString() != "")
                        {
                            int index = (itemSourceStandardNotes.IndexOf(itemSourceStandardNotes.Where(n => Convert.ToInt64(n.ID) == Convert.ToInt64(note["ID"])).FirstOrDefault<Node>()));
                            //itemSourceStandardNotes.Where(n => Convert.ToInt64(n.ID) == Convert.ToInt64(note["ID"])).FirstOrDefault<Node>();

                            //itemSourceStandardNotes[].IsSelected = true;
                            itemSourceStandardNotes[index].IsSelected = true;
                        }
                        //itemSourceStandardNotes.AsEnumerable().Where(n => n.ID == Convert.ToInt64(note["ID"]));
                    //itemSourceStandardNotes.AsEnumerable().Select(n => n.ID).Where(n => n.ID == Convert.ToInt64(note["ID"]))
                        
                        //((Node)itemSourceStandardNotes.AsEnumerable().Where(n => n.ID == Convert.ToInt64(note["ID"]))).IsSelected = Convert.ToBoolean(note["IsSelected"]);
                        //itemSourceStandardNotes[index].IsSelected = Convert.ToBoolean(note["IsSelected"]);
                        
                    }
                }

            }

            cmbStandardNotes.ItemsSource = itemSourceStandardNotes;


        }

        private void LoadScreenData(LeadApplicationData leadApplicationData)
        {

            #region Lead Contact Numbers

            List<ContactNumber> lstContactNumbers = new List<ContactNumber>();
            if (leadApplicationData?.LeadData?.TelCell?.Length == 10)
            {
                lstContactNumbers.Add(new ContactNumber { ID = "Cell", TelNumber = "Cell: " + Convert.ToInt64(leadApplicationData.LeadData.TelCell).ToString("0#########") });
            }
            if (leadApplicationData?.LeadData?.TelWork?.Length == 10)
            {
                lstContactNumbers.Add(new ContactNumber { ID = "Work", TelNumber = "Work: " + Convert.ToInt64(leadApplicationData.LeadData.TelWork).ToString("0#########") });
            }
            if (leadApplicationData?.LeadData?.TelHome?.Length == 10)
            {
                lstContactNumbers.Add(new ContactNumber { ID = "Home", TelNumber = "Home: " + Convert.ToInt64(leadApplicationData.LeadData.TelHome).ToString("0#########") });
            }
            if (leadApplicationData?.LeadData?.TelOther?.Length == 10)
            {
                lstContactNumbers.Add(new ContactNumber { ID = "Other", TelNumber = "Other: " + Convert.ToInt64(leadApplicationData.LeadData.TelOther).ToString("0#########") });
            }
            
            cmbContactNumbers.ItemsSource = lstContactNumbers;
            cmbContactNumbers.DisplayMemberPath = "TelNumber";
            cmbContactNumbers.SelectedValuePath = "ID";

            #endregion

            SetStandardNotes(leadApplicationData.AppData.ImportID);
            ScreenData.LeadApplicationScreenData = leadApplicationData;
            //SetBankBranch(leadApplicationData.BankDetailsData.BankID);
            ScreenData.TotalCost = CalculateTotalCost(ScreenData.LeadApplicationScreenData.PolicyData.OptionID);

            ScreenData.FKINImportID = leadApplicationData.AppData.ImportID;
            ScreenData.RefNo = leadApplicationData.AppData.RefNo;
            ScreenData.CanModifyBumpUpCallDetails = leadApplicationData.PolicyData.UDMBumpUpOption;
            if (leadApplicationData.NextOfKinData[0] != null)
            {
                string relationship = "";
                ScreenData.NextOfKinDetails = leadApplicationData.NextOfKinData[0].FullName;
                if (leadApplicationData.NextOfKinData[0].RelationshipID != null && leadApplicationData.NextOfKinData[0].RelationshipID != 0)
                {
                    relationship = Methods.GetTableData("SELECT Description FROM lkpINRelationship WHERE ID = " + leadApplicationData.NextOfKinData[0].RelationshipID).Rows[0]["Description"].ToString();
                    ScreenData.NextOfKinDetails += "(" + relationship + ")";
                }
                if (leadApplicationData.NextOfKinData[0].TelContact != "" && leadApplicationData.NextOfKinData[0].TelContact != null)
                {
                    ScreenData.NextOfKinDetails += " TelContact: " + leadApplicationData.NextOfKinData[0].TelContact;
                }
                //ScreenData.NextOfKinDetails = leadApplicationData.NextOfKinData[0].FullName + "(" + relationship + ") TelContact: " + leadApplicationData.NextOfKinData[0].TelContact;
            }
            


            if (leadApplicationData.PolicyData.UDMBumpUpOption)
            {
                ScreenData.HasPolicyBeenBumpedUp = "Yes";
            }
            else
            {
                ScreenData.HasPolicyBeenBumpedUp = "No";
            }

            if (leadApplicationData.PolicyData.IsLA2Checked)
            {
                ScreenData.IncludesLA2Cover = "Yes";
            }
            else
            {
                ScreenData.IncludesLA2Cover = "No";
            }

            INImportCallMonitoring inImportCallMonitoring = new INImportCallMonitoring();

            if (leadApplicationData.AppData.ImportID != null)
            {
                inImportCallMonitoring = INImportCallMonitoringMapper.SearchOne(leadApplicationData.AppData.ImportID, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            }            


            if (inImportCallMonitoring != null)
            {
                ScreenData.ID = inImportCallMonitoring.ID;
                ScreenData.WasClearYesGivenInSalesQuestion = inImportCallMonitoring.WasClearYesGivenInSalesQuestion;
                ScreenData.IsBankingDetailsCapturedCorrectly = inImportCallMonitoring.IsBankingDetailsCapturedCorrectly;
                ScreenData.WasAccountVerified = inImportCallMonitoring.WasAccountVerified;
                ScreenData.WasDebitDateConfirmed = inImportCallMonitoring.WasDebitDateConfirmed;
                ScreenData.IsAccountInClientsName = inImportCallMonitoring.IsAccountInClientsName;
                ScreenData.DoesClientHaveSigningPower = inImportCallMonitoring.DoesClientHaveSigningPower;
                ScreenData.WasDebiCheckProcessExplainedCorrectly = inImportCallMonitoring.WasDebiCheckProcessExplainedCorrectly;
                ScreenData.WasCorrectClosureQuestionAsked = inImportCallMonitoring.WasCorrectClosureQuestionAsked;
                ScreenData.WasResponseClearAndPositive = inImportCallMonitoring.WasResponseClearAndPositive;
                ScreenData.WasUDMAndPLMentionedAsFSPs = inImportCallMonitoring.WasUDMAndPLMentionedAsFSPs;
                ScreenData.WasDebitAmountMentionedCorrectly = inImportCallMonitoring.WasDebitAmountMentionedCorrectly;
                ScreenData.WasFirstDebitDateExplainedCorrectly = inImportCallMonitoring.WasFirstDebitDateExplainedCorrectly;
                ScreenData.WasCorrectCoverCommencementDateMentioned = inImportCallMonitoring.WasCorrectCoverCommencementDateMentioned;
                ScreenData.WasNonPaymentProcedureExplained = inImportCallMonitoring.WasNonPaymentProcedureExplained;
                ScreenData.WasAnnualIncreaseExplained = inImportCallMonitoring.WasAnnualIncreaseExplained;
                ScreenData.WasCorrectQuestionAskedBumpUpClosure = inImportCallMonitoring.WasCorrectQuestionAskedBumpUpClosure;
                ScreenData.WasResponseClearAndPositiveBumpUpClosure = inImportCallMonitoring.WasResponseClearAndPositiveBumpUpClosure;
                ScreenData.WasUDMAndPLMentionedAsFSPsBumpUpClosure = inImportCallMonitoring.WasUDMAndPLMentionedAsFSPsBumpUpClosure;
                ScreenData.WasDebitAmountMentionedCorrectlyBumpUpClosure = inImportCallMonitoring.WasDebitAmountMentionedCorrectlyBumpUpClosure;
                ScreenData.WasFirstDebitDateExplainedCorrectlyBumpUpClosure = inImportCallMonitoring.WasFirstDebitDateExplainedCorrectlyBumpUpClosure;
                ScreenData.WasCorrectCoverCommencementDateMentionedBumpUpClosure = inImportCallMonitoring.WasCorrectCoverCommencementDateMentionedBumpUpClosure;
                ScreenData.WasNonPaymentProcedureExplainedBumpUpClosure = inImportCallMonitoring.WasNonPaymentProcedureExplainedBumpUpClosure;
                ScreenData.WasAnnualIncreaseExplainedBumpUpClosure = inImportCallMonitoring.WasAnnualIncreaseExplainedBumpUpClosure;
                ScreenData.FKINCallMonitoringOutcomeID = inImportCallMonitoring.FKINCallMonitoringOutcomeID;
                ScreenData.Comments = inImportCallMonitoring.Comments;
                ScreenData.FKCallMonitoringUserID = inImportCallMonitoring.FKCallMonitoringUserID;
                ScreenData.WasCallEvaluatedBySecondaryUser = inImportCallMonitoring.WasCallEvaluatedBySecondaryUser;
                ScreenData.FKSecondaryCallMonitoringUserID = inImportCallMonitoring.FKSecondaryCallMonitoringUserID;
                ScreenData.FKINCallAssessmentOutcomeID = inImportCallMonitoring.FKINCallAssessmentOutcomeID;
                ScreenData.IsRecoveredSale = inImportCallMonitoring.IsRecoveredSale;
                ScreenData.IsCallMonitored = inImportCallMonitoring.IsCallMonitored;
                ScreenData.WasPermissionQuestionAsked = inImportCallMonitoring.WasPermissionQuestionAsked;
                ScreenData.WasNextOfKinQuestionAsked = inImportCallMonitoring.WasNextOfKinQuestionAsked;
                ScreenData.FKTertiaryCallMonitoringUserID = inImportCallMonitoring.FKTertiaryCallMonitoringUserID;
                ScreenData.IsTSRBUSavedCarriedForward = inImportCallMonitoring.IsTSRBUSavedCarriedForward;
                ScreenData.TSRBUSavedCarriedForwardDate = inImportCallMonitoring.TSRBUSavedCarriedForwardDate;
                ScreenData.TSRBUSavedCarriedForwardAssignedByUserID = inImportCallMonitoring.TSRBUSavedCarriedForwardAssignedByUserID;
                ScreenData.CallMonitoredDate = inImportCallMonitoring.CallMonitoredDate;



                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/221494754/comments
                ScreenData.ExclusionsExplained = inImportCallMonitoring.ExclusionsExplained;
                ScreenData.ExclusionsExplainedBumpUpClosure = inImportCallMonitoring.ExclusionsExplainedBumpUpClosure;
            }
            else
            {
                // Create a new INImportCallMonitoring record:
                inImportCallMonitoring = new INImportCallMonitoring();
                ApplyDefaultSettings();
                inImportCallMonitoring.FKINImportID = leadApplicationData.AppData.ImportID;
                inImportCallMonitoring.Save(null);

                ScreenData.ID = inImportCallMonitoring.ID;
            }

            #region Create a secondary screen data object to keep track of the details as they are loaded

            RefreshLoadedScreenFields();

            #endregion Create a secondary screen data object to keep track of the details as they are loaded

        }

        private void SetBankBranch(long? fkBankID)
        {
            if (fkBankID != null)
            {

                #region Set the drop-down items of cmbBankBranch

                var filteredBankBranches = _dtAllBankBranches.Select(String.Format("[FKBankID] = {0}", fkBankID.Value), "[Name]").AsEnumerable();
                if (filteredBankBranches.Any())
                {
                    DataTable dtSelectedBankBranches = _dtAllBankBranches.Select(String.Format("[FKBankID] = {0}", fkBankID.Value), "[Name]").CopyToDataTable();
                    cmbBankBranch.Populate(dtSelectedBankBranches, "Code", "ID");
                    //cmbBankBranch.SelectedIndex = 0;
                }

                #endregion Set the drop-down items of cmbBankBranch

                var filteredBankAccountNumberPatterns = _dtAllBankAccountNumberPatterns.Select(String.Format("[FKBankID] = {0}", fkBankID.Value)).AsEnumerable();
                if (filteredBankAccountNumberPatterns.Any())
                {
                    DataTable dtSelectedBankAccountNumberPatterns = _dtAllBankAccountNumberPatterns.Select(String.Format("[FKBankID] = {0}", fkBankID.Value)).CopyToDataTable();

                    string pattern = dtSelectedBankAccountNumberPatterns.Rows[0]["Pattern"] as string;
                    ScreenData.LeadApplicationScreenData.BankDetailsData.BankAccountNumberPattern = pattern;
                    medAccountNumber.ValueConstraint.RegexPattern = pattern;
                }
                else
                {
                    ScreenData.LeadApplicationScreenData.BankDetailsData.BankAccountNumberPattern = "NULL";
                    medAccountNumber.ValueConstraint.RegexPattern = "NULL";
                }
            }
        }

        private decimal CalculateTotalCost(long? fkINOption)
        {
            decimal totalCost = 0.00m;

            if (fkINOption != null)
            {
                DataTable dtINOption = Insure.INGetOptionDetailsFromID(fkINOption.Value);
                if (dtINOption.Rows.Count > 0)
                {
                    decimal la1Cost = dtINOption.Rows[0]["LA1Cost"] != DBNull.Value ? Convert.ToDecimal(dtINOption.Rows[0]["LA1Cost"]) : 0.00m;
                    decimal la2Cost = dtINOption.Rows[0]["LA2Cost"] != DBNull.Value ? Convert.ToDecimal(dtINOption.Rows[0]["LA2Cost"]) : 0.00m;
                    decimal childCost = dtINOption.Rows[0]["ChildCost"] != DBNull.Value ? Convert.ToDecimal(dtINOption.Rows[0]["ChildCost"]) : 0.00m;
                    decimal funeralCost = dtINOption.Rows[0]["FuneralCost"] != DBNull.Value ? Convert.ToDecimal(dtINOption.Rows[0]["FuneralCost"]) : 0.00m;

                    totalCost = la1Cost;

                    if (ScreenData.LeadApplicationScreenData.PolicyData.IsLA2Checked)
                    {
                        totalCost += la2Cost;
                    }
                    if (ScreenData.LeadApplicationScreenData.PolicyData.IsChildChecked)
                    {
                        totalCost += childCost;
                    }
                    if (ScreenData.LeadApplicationScreenData.PolicyData.IsFuneralChecked)
                    {
                        totalCost += funeralCost;
                    }
                }
            }

            return totalCost;
        }

        private void ApplyDefaultSettings()
        {
            DataTable dtCallMonitoringScreenDefaults = Insure.INGetCallMonitoringScreenDefaults();

            if (dtCallMonitoringScreenDefaults.Rows.Count > 0)
            {
                ScreenData.WasClearYesGivenInSalesQuestion = dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasClearYesGivenInSalesQuestion"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasClearYesGivenInSalesQuestion"]) : (bool?)null;
                ScreenData.IsBankingDetailsCapturedCorrectly = dtCallMonitoringScreenDefaults.Rows[0]["DefaultIsBankingDetailsCapturedCorrectly"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultIsBankingDetailsCapturedCorrectly"]) : (bool?)null;
                ScreenData.WasAccountVerified = dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasAccountVerified"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasAccountVerified"]) : (bool?)null;
                ScreenData.WasDebitDateConfirmed = dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasDebitDateConfirmed"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasDebitDateConfirmed"]) : (bool?)null;
                ScreenData.IsAccountInClientsName = dtCallMonitoringScreenDefaults.Rows[0]["DefaultIsAccountInClientsName"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultIsAccountInClientsName"]) : (bool?)null;
                ScreenData.DoesClientHaveSigningPower = dtCallMonitoringScreenDefaults.Rows[0]["DefaultDoesClientHaveSigningPower"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultDoesClientHaveSigningPower"]) : (bool?)null;
                ScreenData.WasDebiCheckProcessExplainedCorrectly = dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasDebiCheckProcessExplainedCorrectly"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasDebiCheckProcessExplainedCorrectly"]) : (bool?)null;

                
                ScreenData.WasCorrectClosureQuestionAsked = dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasCorrectClosureQuestionAsked"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasCorrectClosureQuestionAsked"]) : (bool?)null;
                ScreenData.WasResponseClearAndPositive = dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasResponseClearAndPositive"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasResponseClearAndPositive"]) : (bool?)null;
                ScreenData.WasUDMAndPLMentionedAsFSPs = dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasUDMAndPLMentionedAsFSPs"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasUDMAndPLMentionedAsFSPs"]) : (bool?)null;
                ScreenData.WasDebitAmountMentionedCorrectly = dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasDebitAmountMentionedCorrectly"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasDebitAmountMentionedCorrectly"]) : (bool?)null;
                ScreenData.WasFirstDebitDateExplainedCorrectly = dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasFirstDebitDateExplainedCorrectly"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasFirstDebitDateExplainedCorrectly"]) : (bool?)null;
                ScreenData.WasCorrectCoverCommencementDateMentioned = dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasCorrectCoverCommencementDateMentioned"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasCorrectCoverCommencementDateMentioned"]) : (bool?)null;
                ScreenData.WasNonPaymentProcedureExplained = dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasNonPaymentProcedureExplained"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasNonPaymentProcedureExplained"]) : (bool?)null;
                ScreenData.WasAnnualIncreaseExplained = dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasAnnualIncreaseExplained"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasAnnualIncreaseExplained"]) : (bool?)null;
                ScreenData.WasCallEvaluatedBySecondaryUser = dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasCallEvaluatedBySecondaryUser"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultWasCallEvaluatedBySecondaryUser"]) : (bool?)null;
                ScreenData.IsRecoveredSale = dtCallMonitoringScreenDefaults.Rows[0]["DefaultIsRecoveredSale"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultIsRecoveredSale"]) : (bool?)null;
                //ScreenData.IsRecoveredSale = dtCallMonitoringScreenDefaults.Rows[0]["DefaultIsRecoveredSale"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultIsRecoveredSale"]) : (bool?)null;

                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/221494754/comments
                ScreenData.ExclusionsExplained = dtCallMonitoringScreenDefaults.Rows[0]["DefaultExclusionsExplained"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultExclusionsExplained"]) : (bool?)null;
                //ScreenData.ExclusionsExplainedBumpUpClosure = dtCallMonitoringScreenDefaults.Rows[0]["DefaultExclusionsExplainedBumpUpClosure"] != DBNull.Value ? Convert.ToBoolean(dtCallMonitoringScreenDefaults.Rows[0]["DefaultExclusionsExplainedBumpUpClosure"]) : (bool?)null;
            }
        }

        private void RefreshLoadedScreenFields()
        {
            LoadedScreenData.WasClearYesGivenInSalesQuestion = ScreenData.WasClearYesGivenInSalesQuestion;
            LoadedScreenData.IsBankingDetailsCapturedCorrectly = ScreenData.IsBankingDetailsCapturedCorrectly;
            LoadedScreenData.WasAccountVerified = ScreenData.WasAccountVerified;
            LoadedScreenData.WasDebitDateConfirmed = ScreenData.WasDebitDateConfirmed;
            LoadedScreenData.IsAccountInClientsName = ScreenData.IsAccountInClientsName;
            LoadedScreenData.DoesClientHaveSigningPower = ScreenData.DoesClientHaveSigningPower;
            LoadedScreenData.WasDebiCheckProcessExplainedCorrectly = ScreenData.WasDebiCheckProcessExplainedCorrectly;
            LoadedScreenData.WasCorrectClosureQuestionAsked = ScreenData.WasCorrectClosureQuestionAsked;
            LoadedScreenData.WasResponseClearAndPositive = ScreenData.WasResponseClearAndPositive;
            LoadedScreenData.WasUDMAndPLMentionedAsFSPs = ScreenData.WasUDMAndPLMentionedAsFSPs;
            LoadedScreenData.WasDebitAmountMentionedCorrectly = ScreenData.WasDebitAmountMentionedCorrectly;
            LoadedScreenData.WasFirstDebitDateExplainedCorrectly = ScreenData.WasFirstDebitDateExplainedCorrectly;
            LoadedScreenData.WasCorrectCoverCommencementDateMentioned = ScreenData.WasCorrectCoverCommencementDateMentioned;
            LoadedScreenData.WasNonPaymentProcedureExplained = ScreenData.WasNonPaymentProcedureExplained;
            LoadedScreenData.WasAnnualIncreaseExplained = ScreenData.WasAnnualIncreaseExplained;
            LoadedScreenData.WasCorrectQuestionAskedBumpUpClosure = ScreenData.WasCorrectQuestionAskedBumpUpClosure;
            LoadedScreenData.WasResponseClearAndPositiveBumpUpClosure = ScreenData.WasResponseClearAndPositiveBumpUpClosure;
            LoadedScreenData.WasUDMAndPLMentionedAsFSPsBumpUpClosure = ScreenData.WasUDMAndPLMentionedAsFSPsBumpUpClosure;
            LoadedScreenData.WasDebitAmountMentionedCorrectlyBumpUpClosure = ScreenData.WasDebitAmountMentionedCorrectlyBumpUpClosure;
            LoadedScreenData.WasFirstDebitDateExplainedCorrectlyBumpUpClosure = ScreenData.WasFirstDebitDateExplainedCorrectlyBumpUpClosure;
            LoadedScreenData.WasCorrectCoverCommencementDateMentionedBumpUpClosure = ScreenData.WasCorrectCoverCommencementDateMentionedBumpUpClosure;
            LoadedScreenData.WasNonPaymentProcedureExplainedBumpUpClosure = ScreenData.WasNonPaymentProcedureExplainedBumpUpClosure;
            LoadedScreenData.WasAnnualIncreaseExplainedBumpUpClosure = ScreenData.WasAnnualIncreaseExplainedBumpUpClosure;
            LoadedScreenData.FKINCallMonitoringOutcomeID = ScreenData.FKINCallMonitoringOutcomeID;
            LoadedScreenData.Comments = ScreenData.Comments;
            LoadedScreenData.FKCallMonitoringUserID = ScreenData.FKCallMonitoringUserID;
            LoadedScreenData.WasCallEvaluatedBySecondaryUser = ScreenData.WasCallEvaluatedBySecondaryUser;
            LoadedScreenData.FKSecondaryCallMonitoringUserID = ScreenData.FKSecondaryCallMonitoringUserID;
            LoadedScreenData.FKINCallAssessmentOutcomeID = ScreenData.FKINCallAssessmentOutcomeID;
            LoadedScreenData.IsRecoveredSale = ScreenData.IsRecoveredSale;
            LoadedScreenData.IsCallMonitored = ScreenData.IsCallMonitored;
            LoadedScreenData.WasPermissionQuestionAsked = ScreenData.WasPermissionQuestionAsked;
            LoadedScreenData.WasNextOfKinQuestionAsked = ScreenData.WasNextOfKinQuestionAsked;
            LoadedScreenData.FKTertiaryCallMonitoringUserID = ScreenData.FKTertiaryCallMonitoringUserID;
            LoadedScreenData.IsTSRBUSavedCarriedForward = ScreenData.IsTSRBUSavedCarriedForward;
            LoadedScreenData.TSRBUSavedCarriedForwardDate = ScreenData.TSRBUSavedCarriedForwardDate;
            LoadedScreenData.TSRBUSavedCarriedForwardAssignedByUserID = ScreenData.TSRBUSavedCarriedForwardAssignedByUserID;
            LoadedScreenData.CallMonitoredDate = ScreenData.CallMonitoredDate;
            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/221494754/comments
            LoadedScreenData.ExclusionsExplained = ScreenData.ExclusionsExplained;
            LoadedScreenData.ExclusionsExplainedBumpUpClosure = ScreenData.ExclusionsExplainedBumpUpClosure;
        }

        private bool HasDetailsBeenModified()
        {
            return
                (LoadedScreenData.WasClearYesGivenInSalesQuestion != ScreenData.WasClearYesGivenInSalesQuestion) || 
                (LoadedScreenData.IsBankingDetailsCapturedCorrectly != ScreenData.IsBankingDetailsCapturedCorrectly) ||
                (LoadedScreenData.WasAccountVerified != ScreenData.WasAccountVerified) ||
                (LoadedScreenData.WasDebitDateConfirmed != ScreenData.WasDebitDateConfirmed) ||
                (LoadedScreenData.IsAccountInClientsName != ScreenData.IsAccountInClientsName) ||
                (LoadedScreenData.DoesClientHaveSigningPower != ScreenData.DoesClientHaveSigningPower) ||
                (LoadedScreenData.WasDebiCheckProcessExplainedCorrectly != ScreenData.WasDebiCheckProcessExplainedCorrectly) ||
                (LoadedScreenData.WasCorrectClosureQuestionAsked != ScreenData.WasCorrectClosureQuestionAsked) ||
                (LoadedScreenData.WasResponseClearAndPositive != ScreenData.WasResponseClearAndPositive) ||
                (LoadedScreenData.WasUDMAndPLMentionedAsFSPs != ScreenData.WasUDMAndPLMentionedAsFSPs) ||
                (LoadedScreenData.WasDebitAmountMentionedCorrectly != ScreenData.WasDebitAmountMentionedCorrectly) ||
                (LoadedScreenData.WasFirstDebitDateExplainedCorrectly != ScreenData.WasFirstDebitDateExplainedCorrectly) ||
                (LoadedScreenData.WasCorrectCoverCommencementDateMentioned != ScreenData.WasCorrectCoverCommencementDateMentioned) ||
                (LoadedScreenData.WasNonPaymentProcedureExplained != ScreenData.WasNonPaymentProcedureExplained) ||
                (LoadedScreenData.WasAnnualIncreaseExplained != ScreenData.WasAnnualIncreaseExplained) ||
                (LoadedScreenData.WasCorrectQuestionAskedBumpUpClosure != ScreenData.WasCorrectQuestionAskedBumpUpClosure) ||
                (LoadedScreenData.WasResponseClearAndPositiveBumpUpClosure != ScreenData.WasResponseClearAndPositiveBumpUpClosure) ||
                (LoadedScreenData.WasUDMAndPLMentionedAsFSPsBumpUpClosure != ScreenData.WasUDMAndPLMentionedAsFSPsBumpUpClosure) ||
                (LoadedScreenData.WasDebitAmountMentionedCorrectlyBumpUpClosure != ScreenData.WasDebitAmountMentionedCorrectlyBumpUpClosure) ||
                (LoadedScreenData.WasFirstDebitDateExplainedCorrectlyBumpUpClosure != ScreenData.WasFirstDebitDateExplainedCorrectlyBumpUpClosure) ||
                (LoadedScreenData.WasCorrectCoverCommencementDateMentionedBumpUpClosure != ScreenData.WasCorrectCoverCommencementDateMentionedBumpUpClosure) ||
                (LoadedScreenData.WasNonPaymentProcedureExplainedBumpUpClosure != ScreenData.WasNonPaymentProcedureExplainedBumpUpClosure) ||
                (LoadedScreenData.WasAnnualIncreaseExplainedBumpUpClosure != ScreenData.WasAnnualIncreaseExplainedBumpUpClosure) ||
                (LoadedScreenData.FKINCallMonitoringOutcomeID != ScreenData.FKINCallMonitoringOutcomeID) ||
                (LoadedScreenData.Comments != ScreenData.Comments) ||
                (LoadedScreenData.FKCallMonitoringUserID != ScreenData.FKCallMonitoringUserID) ||
                (LoadedScreenData.WasCallEvaluatedBySecondaryUser != ScreenData.WasCallEvaluatedBySecondaryUser) ||
                (LoadedScreenData.FKSecondaryCallMonitoringUserID != ScreenData.FKSecondaryCallMonitoringUserID) ||
                (LoadedScreenData.FKINCallAssessmentOutcomeID != ScreenData.FKINCallAssessmentOutcomeID) ||
                (LoadedScreenData.IsRecoveredSale != ScreenData.IsRecoveredSale) ||
                (LoadedScreenData.IsCallMonitored != ScreenData.IsCallMonitored) ||
                (LoadedScreenData.WasPermissionQuestionAsked != ScreenData.WasPermissionQuestionAsked) ||
                (LoadedScreenData.WasNextOfKinQuestionAsked != ScreenData.WasNextOfKinQuestionAsked) ||                
                (LoadedScreenData.FKTertiaryCallMonitoringUserID != ScreenData.FKTertiaryCallMonitoringUserID) ||
                (LoadedScreenData.IsTSRBUSavedCarriedForward != ScreenData.IsTSRBUSavedCarriedForward) ||
                (LoadedScreenData.TSRBUSavedCarriedForwardDate != ScreenData.TSRBUSavedCarriedForwardDate) ||
                (LoadedScreenData.TSRBUSavedCarriedForwardAssignedByUserID != ScreenData.TSRBUSavedCarriedForwardAssignedByUserID) ||
                (LoadedScreenData.CallMonitoredDate != ScreenData.CallMonitoredDate) ||
                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/221494754/comments
                (LoadedScreenData.ExclusionsExplained != ScreenData.ExclusionsExplained) ||
                (LoadedScreenData.ExclusionsExplainedBumpUpClosure != ScreenData.ExclusionsExplainedBumpUpClosure);
        }

        private bool AreAllRequiredFieldsCompletedAndValid()
        {
            if (Convert.ToBoolean(ScreenData.WasCallEvaluatedBySecondaryUser))
            {
                if (ScreenData.FKINCallAssessmentOutcomeID == null)
                {
                    MessageBox.Show("Given that the call is indicated as being over-assessed, please indicate the outcome of the over-assessment.", "Outcome of Over Assessment Not Specified", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if (ScreenData.FKSecondaryCallMonitoringUserID == null)
                {
                    MessageBox.Show("Given that the call is indicated as being over-assessed, please indicate who conducted the over-assessment.", "Over Assessor Not Specified", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }

        private void Save()
        {
            try
            {
                Methods.ExecuteSQLNonQuery("delete from ImportCMStandardNotes where FKImportCallMonitoringID = " + (long)ScreenData.ID);

                foreach (Node note in (ObservableNodeList)cmbStandardNotes.ItemsSource)
                {
                    if (note.IsSelected == true)
                    {
                        //Methods.ExecuteSQLNonQuery("insert into ImportCMStandardNotes (FKCallMonitoringStandardNotesID, FKImportCallMonitoringID) values(" + note.ID + "," + (long)ScreenData.ID +")");
                        ImportCMStandardNotes importCMStandardNotes = new ImportCMStandardNotes();
                        importCMStandardNotes.FKCallMonitoringStandardNotesID = note.ID;
                        importCMStandardNotes.FKImportCallMonitoringID = (long)ScreenData.ID;

                        importCMStandardNotes.Save(null);
                    }
                    else
                    {
                        continue;
                    }
                }

                //((ObservableNodeList)cmbStandardNotes.ItemsSource);


                INImportCallMonitoring inImportCallMonitoring = new INImportCallMonitoring((long)ScreenData.ID);
                inImportCallMonitoring.WasClearYesGivenInSalesQuestion = ScreenData.WasClearYesGivenInSalesQuestion; // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/222495313/comments
                inImportCallMonitoring.IsBankingDetailsCapturedCorrectly = ScreenData.IsBankingDetailsCapturedCorrectly;
                inImportCallMonitoring.WasAccountVerified = ScreenData.WasAccountVerified;
                inImportCallMonitoring.WasDebitDateConfirmed = ScreenData.WasDebitDateConfirmed;
                inImportCallMonitoring.IsAccountInClientsName = ScreenData.IsAccountInClientsName;
                inImportCallMonitoring.DoesClientHaveSigningPower = ScreenData.DoesClientHaveSigningPower;
                inImportCallMonitoring.WasDebiCheckProcessExplainedCorrectly = ScreenData.WasDebiCheckProcessExplainedCorrectly;
                inImportCallMonitoring.WasCorrectClosureQuestionAsked = ScreenData.WasCorrectClosureQuestionAsked;
                inImportCallMonitoring.WasResponseClearAndPositive = ScreenData.WasResponseClearAndPositive;
                inImportCallMonitoring.WasUDMAndPLMentionedAsFSPs = ScreenData.WasUDMAndPLMentionedAsFSPs;
                inImportCallMonitoring.WasDebitAmountMentionedCorrectly = ScreenData.WasDebitAmountMentionedCorrectly;
                inImportCallMonitoring.WasFirstDebitDateExplainedCorrectly = ScreenData.WasFirstDebitDateExplainedCorrectly;
                inImportCallMonitoring.WasCorrectCoverCommencementDateMentioned = ScreenData.WasCorrectCoverCommencementDateMentioned;
                inImportCallMonitoring.WasNonPaymentProcedureExplained = ScreenData.WasNonPaymentProcedureExplained;
                inImportCallMonitoring.WasAnnualIncreaseExplained = ScreenData.WasAnnualIncreaseExplained;
                inImportCallMonitoring.WasCorrectQuestionAskedBumpUpClosure = ScreenData.WasCorrectQuestionAskedBumpUpClosure;
                inImportCallMonitoring.WasResponseClearAndPositiveBumpUpClosure = ScreenData.WasResponseClearAndPositiveBumpUpClosure;
                inImportCallMonitoring.WasUDMAndPLMentionedAsFSPsBumpUpClosure = ScreenData.WasUDMAndPLMentionedAsFSPsBumpUpClosure;
                inImportCallMonitoring.WasDebitAmountMentionedCorrectlyBumpUpClosure = ScreenData.WasDebitAmountMentionedCorrectlyBumpUpClosure;
                inImportCallMonitoring.WasFirstDebitDateExplainedCorrectlyBumpUpClosure = ScreenData.WasFirstDebitDateExplainedCorrectlyBumpUpClosure;
                inImportCallMonitoring.WasCorrectCoverCommencementDateMentionedBumpUpClosure = ScreenData.WasCorrectCoverCommencementDateMentionedBumpUpClosure;
                inImportCallMonitoring.WasNonPaymentProcedureExplainedBumpUpClosure = ScreenData.WasNonPaymentProcedureExplainedBumpUpClosure;
                inImportCallMonitoring.WasAnnualIncreaseExplainedBumpUpClosure = ScreenData.WasAnnualIncreaseExplainedBumpUpClosure;
                inImportCallMonitoring.FKINCallMonitoringOutcomeID = ScreenData.FKINCallMonitoringOutcomeID;
                inImportCallMonitoring.Comments = ScreenData.Comments;
                inImportCallMonitoring.FKCallMonitoringUserID = ScreenData.FKCallMonitoringUserID;
                inImportCallMonitoring.WasCallEvaluatedBySecondaryUser = ScreenData.WasCallEvaluatedBySecondaryUser;
                inImportCallMonitoring.FKSecondaryCallMonitoringUserID = ScreenData.FKSecondaryCallMonitoringUserID;
                inImportCallMonitoring.FKINCallAssessmentOutcomeID = ScreenData.FKINCallAssessmentOutcomeID;
                inImportCallMonitoring.IsRecoveredSale = ScreenData.IsRecoveredSale;
                inImportCallMonitoring.WasPermissionQuestionAsked = ScreenData.WasPermissionQuestionAsked;
                inImportCallMonitoring.WasNextOfKinQuestionAsked = ScreenData.WasNextOfKinQuestionAsked;
                inImportCallMonitoring.FKTertiaryCallMonitoringUserID = ScreenData.FKTertiaryCallMonitoringUserID;

                inImportCallMonitoring.IsTSRBUSavedCarriedForward = ScreenData.IsTSRBUSavedCarriedForward;
                if ((LoadedScreenData.IsTSRBUSavedCarriedForward == null || LoadedScreenData.IsTSRBUSavedCarriedForward == false) && ScreenData.IsTSRBUSavedCarriedForward == true)
                {
                    ScreenData.TSRBUSavedCarriedForwardDate = DateTime.Now;
                    ScreenData.TSRBUSavedCarriedForwardAssignedByUserID = GlobalSettings.ApplicationUser.ID;                    
                }
                inImportCallMonitoring.TSRBUSavedCarriedForwardDate = ScreenData.TSRBUSavedCarriedForwardDate;
                inImportCallMonitoring.TSRBUSavedCarriedForwardAssignedByUserID = ScreenData.TSRBUSavedCarriedForwardAssignedByUserID;
                

                inImportCallMonitoring.IsCallMonitored = ScreenData.IsCallMonitored;

                if (inImportCallMonitoring.IsCallMonitored == true && inImportCallMonitoring.CallMonitoredDate == null)
                {
                    ScreenData.CallMonitoredDate = DateTime.Now;
                    //inImportCallMonitoring.CallMonitoredDate = DateTime.Now;
                }
                inImportCallMonitoring.CallMonitoredDate = ScreenData.CallMonitoredDate;

                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/221494754/comments
                inImportCallMonitoring.ExclusionsExplained = ScreenData.ExclusionsExplained;
                inImportCallMonitoring.ExclusionsExplainedBumpUpClosure = ScreenData.ExclusionsExplainedBumpUpClosure;

                inImportCallMonitoring.Save(null);

                MessageBox.Show("The call monitoring details have been saved successfully.", "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                RefreshLoadedScreenFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        //private void xamEditor_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    xamEditor_Select(sender);
        //}

        #endregion  Private Methods

        #region Event Handlers

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        
        private void btbClose_Click(object sender, RoutedEventArgs e)
        {      
            if (HasDetailsBeenModified())
            {
                MessageBoxResult result = MessageBox.Show("Would you save the changes to these call monitoring details?", "Save Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    if (AreAllRequiredFieldsCompletedAndValid())
                    {
                        Save();
                        
                        Close();
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                    Close();
                }
                else
                {
                    return;
                }
            }
            else
            {
                Close();
            } 
        }

        public void EmbriantComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            EmbriantComboBox cmbControl = (EmbriantComboBox)sender;

            if (e.Key == Key.Back)
            {
                if (cmbControl.SelectedValue != null)
                {
                    cmbControl.SelectedValue = null;
                    e.Handled = true;
                }
            }
        }

        #endregion

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (AreAllRequiredFieldsCompletedAndValid())
            {
                Save();
            }
        }

        private void chkWasCallEvaluatedBySecondaryUser_Checked(object sender, RoutedEventArgs e)
        {
            cmbCallAssessmentOutcome.IsEnabled = true;
            cmbSecondaryCallMonitoringUser.IsEnabled = true;
        }

        private void chkWasCallEvaluatedBySecondaryUser_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbCallAssessmentOutcome.IsEnabled = false;
            cmbSecondaryCallMonitoringUser.IsEnabled = false;
        }

        private void cmbBank_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            //SetBankBranch((long?)cmbBank.SelectedValue);
            SetBankBranch(ScreenData.LeadApplicationScreenData.BankDetailsData.BankID);
            cmbBankBranch.SelectedIndex = 0;

            //if (cmbBank.SelectedValue != null)
            //{
            //    long fkBankID = Convert.ToInt64(cmbBank.SelectedValue);

            //    #region Set the drop-down items of cmbBankBranch

            //    var filteredBankBranches = _dtAllBankBranches.Select(String.Format("[FKBankID] = {0}", fkBankID), "[Name]").AsEnumerable();
            //    if (filteredBankBranches.Any())
            //    {
            //        DataTable dtSelectedBankBranches = _dtAllBankBranches.Select(String.Format("[FKBankID] = {0}", fkBankID), "[Name]").CopyToDataTable();
            //        cmbBankBranch.Populate(dtSelectedBankBranches, "Code", "ID");
            //        cmbBankBranch.SelectedIndex = 0;
            //    }

            //    #endregion Set the drop-down items of cmbBankBranch

            //    var filteredBankAccountNumberPatterns = _dtAllBankAccountNumberPatterns.Select(String.Format("[FKBankID] = {0}", fkBankID)).AsEnumerable();
            //    if (filteredBankAccountNumberPatterns.Any())
            //    {
            //        DataTable dtSelectedBankAccountNumberPatterns = _dtAllBankAccountNumberPatterns.Select(String.Format("[FKBankID] = {0}", fkBankID)).CopyToDataTable();

            //        string pattern = dtSelectedBankAccountNumberPatterns.Rows[0]["Pattern"] as string;
            //        ScreenData.LeadApplicationScreenData.BankDetailsData.BankAccountNumberPattern = pattern;
            //        medAccountNumber.ValueConstraint.RegexPattern = pattern;
            //    }
            //    else
            //    {
            //        ScreenData.LeadApplicationScreenData.BankDetailsData.BankAccountNumberPattern = "NULL";
            //        medAccountNumber.ValueConstraint.RegexPattern = "NULL";
            //    }
            //}
        }

        private void btnVerifyAccNum_Click(object sender, RoutedEventArgs e)
        {
            ScreenData.LeadApplicationScreenData.VerifyAccountNumber(cmbBankBranch.Text, ScreenData.LeadApplicationScreenData.BankDetailsData.AccountNumber);
        }


        private void medDOAccountNumber_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {

        }

        private void xamEditor_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void medAccountNumber_OnEditModeValidationError(object sender, Infragistics.Windows.Editors.Events.EditModeValidationErrorEventArgs e)
        {

        }

        private void medAccountNumber_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {

        }

        private void chkTSRBUSavedCF_Click(object sender, RoutedEventArgs e)
        {
            if (chkTSRBUSavedCF.IsChecked == true)
            {
                SelectCMAgentToAssignTSRSavedCF selectCMAgentToAssignTSRSavedCF = new SelectCMAgentToAssignTSRSavedCF(this);
                //new INDialogWindow(selectCMAgentToAssignTSRSavedCF).ShowDialog();
                bool? dialogResult = selectCMAgentToAssignTSRSavedCF.ShowDialog();
                if (!((bool)dialogResult))
                {
                    chkTSRBUSavedCF.IsChecked = false;
                }
            }
        }

        private void btnNotes_Click(object sender, RoutedEventArgs e)
        {
            SaleDetailNotesScreen noteScreen = new SaleDetailNotesScreen(this);
            noteScreen.ShowDialog();
        }
    }

}
