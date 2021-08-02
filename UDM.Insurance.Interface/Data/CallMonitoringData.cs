using System;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Data
{
    public class CallMonitoringData : ObservableObject
    {

        #region Private Members

        private LeadApplicationData _leadApplicationScreenData;
        private long? _id;
        private long? _fKINImportID;
        private string _refNo;
        private string _hasPolicyBeenBumpedUp;
        private string _includesLA2Cover;
        private bool _canModifyBumpUpCallDetails;
        private bool? _wasClearYesGivenInSalesQuestion;
        private bool? _isBankingDetailsCapturedCorrectly;
        private bool? _wasAccountVerified;
        private bool? _wasDebitDateConfirmed;
        private bool? _isAccountInClientsName;
        private bool? _doesClientHaveSigningPower;
        private bool? _wasCorrectClosureQuestionAsked;
        private bool? _wasResponseClearAndPositive;
        private bool? _wasUDMAndPLMentionedAsFSPs;
        private bool? _wasDebitAmountMentionedCorrectly;
        private bool? _wasFirstDebitDateExplainedCorrectly;
        private bool? _wasCorrectCoverCommencementDateMentioned;
        private bool? _wasNonPaymentProcedureExplained;
        private bool? _wasAnnualIncreaseExplained;
        private bool? _wasCorrectQuestionAskedBumpUpClosure;
        private bool? _wasResponseClearAndPositiveBumpUpClosure;
        private bool? _wasUDMAndPLMentionedAsFSPsBumpUpClosure;
        private bool? _wasDebitAmountMentionedCorrectlyBumpUpClosure;
        private bool? _wasFirstDebitDateExplainedCorrectlyBumpUpClosure;
        private bool? _wasCorrectCoverCommencementDateMentionedBumpUpClosure;
        private bool? _wasNonPaymentProcedureExplainedBumpUpClosure;
        private bool? _wasAnnualIncreaseExplainedBumpUpClosure;
        private bool? _exclusionsExplained;
        private bool? _exclusionsExplainedBumpUpClosure;
        private bool? _isRecoveredSale;
        private bool? _isCallMonitored;
        private bool? _wasPermissionQuestionAsked;
        private bool? _wasNextOfKinQuestionAsked;
        private bool? _wasDebiCheckProcessExplainedCorrectly;

        private long? _fKINCallMonitoringOutcomeID;
        private string _comments;
        private long? _fKCallMonitoringUserID;
        private bool? _wasCallEvaluatedBySecondaryUser;
        private long? _fKSecondaryCallMonitoringUserID;
        private long? _fKTertiaryCallMonitoringUserID;
        private long? _tsrBUSavedCarriedForwardAssignedByUserID;
        private bool? _isTSRBUSavedCarriedForward;
        private DateTime? _tSRBUSavedCarriedForwardDate;
        private long? _fKINCallAssessmentOutcomeID;
        private decimal _totalCost;
        private DateTime? _callMonitoredDate;
        private string _nextOfKinDetails;

        public string RefNoCampaignGroupType;
        public DateTime? StartTimeOverAssessment;
        public DateTime? EndTimeOverAssessment;
        public DateTime? StartTimeOverAssessorOutcome;
        public DateTime? EndTimeOverAssessorOutcome;
        public DateTime? StartTimeCFOverAssessment;
        public DateTime? EndTimeCFOverAssessment;




        #endregion Private Members

        #region Publicly-Exposed Properties

        public LeadApplicationData LeadApplicationScreenData
        {
            get
            {
                return _leadApplicationScreenData;
            }
            set
            {
                SetProperty(ref _leadApplicationScreenData, value, () => LeadApplicationScreenData);
            }
        }

        public long? ID
        {
            get
            {
                return _id;
            }
            set
            {
                SetProperty(ref _id, value, () => ID);
            }
        }

        public long? FKINImportID
        {
            get
            {
                return _fKINImportID;
            }
            set
            {
                SetProperty(ref _fKINImportID, value, () => FKINImportID);
            }
        }

        public string RefNo
        {
            get
            {
                return _refNo;
            }
            set
            {
                SetProperty(ref _refNo, value, () => RefNo);
            }
        }

        public string HasPolicyBeenBumpedUp
        {
            get
            {
                return _hasPolicyBeenBumpedUp;
            }
            set
            {
                SetProperty(ref _hasPolicyBeenBumpedUp, value, () => HasPolicyBeenBumpedUp);
            }
        }

        public string IncludesLA2Cover
        {
            get
            {
                return _includesLA2Cover;
            }
            set
            {
                SetProperty(ref _includesLA2Cover, value, () => IncludesLA2Cover);
            }
        }

        //public bool? WasMoneyBackVsVoucherBenefitsExplainedCorrectly
        //{
        //    get
        //    {
        //        return _wasMoneyBackVsVoucherBenefitsExplainedCorrectly;
        //    }
        //    set
        //    {
        //        SetProperty(ref _wasMoneyBackVsVoucherBenefitsExplainedCorrectly, value, () => WasMoneyBackVsVoucherBenefitsExplainedCorrectly);
        //    }
        //}

        public bool? WasClearYesGivenInSalesQuestion
        {
            get
            {
                return _wasClearYesGivenInSalesQuestion;
            }
            set
            {
                SetProperty(ref _wasClearYesGivenInSalesQuestion, value, () => WasClearYesGivenInSalesQuestion);
            }
        }

        public bool CanModifyBumpUpCallDetails
        {
            get
            {
                return _canModifyBumpUpCallDetails;
            }
            set
            {
                SetProperty(ref _canModifyBumpUpCallDetails, value, () => CanModifyBumpUpCallDetails);
            }
        }

        public bool? IsBankingDetailsCapturedCorrectly
        {
            get
            {
                return _isBankingDetailsCapturedCorrectly;
            }
            set
            {
                SetProperty(ref _isBankingDetailsCapturedCorrectly, value, () => IsBankingDetailsCapturedCorrectly);
            }
        }

        public bool? WasAccountVerified
        {
            get
            {
                return _wasAccountVerified;
            }
            set
            {
                SetProperty(ref _wasAccountVerified, value, () => WasAccountVerified);
            }
        }

        public bool? WasDebitDateConfirmed
        {
            get
            {
                return _wasDebitDateConfirmed;
            }
            set
            {
                SetProperty(ref _wasDebitDateConfirmed, value, () => WasDebitDateConfirmed);
            }
        }

        public bool? IsAccountInClientsName
        {
            get
            {
                return _isAccountInClientsName;
            }
            set
            {
                SetProperty(ref _isAccountInClientsName, value, () => IsAccountInClientsName);
            }
        }

        public bool? DoesClientHaveSigningPower
        {
            get
            {
                return _doesClientHaveSigningPower;
            }
            set
            {
                SetProperty(ref _doesClientHaveSigningPower, value, () => DoesClientHaveSigningPower);
            }
        }

        public bool? WasDebiCheckProcessExplainedCorrectly
        {
            get
            {
                return _wasDebiCheckProcessExplainedCorrectly;
            }
            set
            {
                SetProperty(ref _wasDebiCheckProcessExplainedCorrectly, value, () => WasDebiCheckProcessExplainedCorrectly);
            }
        }

        public bool? WasCorrectClosureQuestionAsked
        {
            get
            {
                return _wasCorrectClosureQuestionAsked;
            }
            set
            {
                SetProperty(ref _wasCorrectClosureQuestionAsked, value, () => WasCorrectClosureQuestionAsked);
            }
        }

        public bool? WasResponseClearAndPositive
        {
            get
            {
                return _wasResponseClearAndPositive;
            }
            set
            {
                SetProperty(ref _wasResponseClearAndPositive, value, () => WasResponseClearAndPositive);
            }
        }

        public bool? WasUDMAndPLMentionedAsFSPs
        {
            get
            {
                return _wasUDMAndPLMentionedAsFSPs;
            }
            set
            {
                SetProperty(ref _wasUDMAndPLMentionedAsFSPs, value, () => WasUDMAndPLMentionedAsFSPs);
            }
        }

        public bool? WasDebitAmountMentionedCorrectly
        {
            get
            {
                return _wasDebitAmountMentionedCorrectly;
            }
            set
            {
                SetProperty(ref _wasDebitAmountMentionedCorrectly, value, () => WasDebitAmountMentionedCorrectly);
            }
        }

        public bool? WasFirstDebitDateExplainedCorrectly
        {
            get
            {
                return _wasFirstDebitDateExplainedCorrectly;
            }
            set
            {
                SetProperty(ref _wasFirstDebitDateExplainedCorrectly, value, () => WasFirstDebitDateExplainedCorrectly);
            }
        }

        public bool? WasCorrectCoverCommencementDateMentioned
        {
            get
            {
                return _wasCorrectCoverCommencementDateMentioned;
            }
            set
            {
                SetProperty(ref _wasCorrectCoverCommencementDateMentioned, value, () => WasCorrectCoverCommencementDateMentioned);
            }
        }

        public bool? WasNonPaymentProcedureExplained
        {
            get
            {
                return _wasNonPaymentProcedureExplained;
            }
            set
            {
                SetProperty(ref _wasNonPaymentProcedureExplained, value, () => WasNonPaymentProcedureExplained);
            }
        }

        public bool? WasAnnualIncreaseExplained
        {
            get
            {
                return _wasAnnualIncreaseExplained;
            }
            set
            {
                SetProperty(ref _wasAnnualIncreaseExplained, value, () => WasAnnualIncreaseExplained);
            }
        }

        public bool? WasCorrectQuestionAskedBumpUpClosure
        {
            get
            {
                return _wasCorrectQuestionAskedBumpUpClosure;
            }
            set
            {
                SetProperty(ref _wasCorrectQuestionAskedBumpUpClosure, value, () => WasCorrectQuestionAskedBumpUpClosure);
            }
        }

        public bool? WasResponseClearAndPositiveBumpUpClosure
        {
            get
            {
                return _wasResponseClearAndPositiveBumpUpClosure;
            }
            set
            {
                SetProperty(ref _wasResponseClearAndPositiveBumpUpClosure, value, () => WasResponseClearAndPositiveBumpUpClosure);
            }
        }

        public bool? WasUDMAndPLMentionedAsFSPsBumpUpClosure
        {
            get
            {
                return _wasUDMAndPLMentionedAsFSPsBumpUpClosure;
            }
            set
            {
                SetProperty(ref _wasUDMAndPLMentionedAsFSPsBumpUpClosure, value, () => WasUDMAndPLMentionedAsFSPsBumpUpClosure);
            }
        }

        public bool? WasDebitAmountMentionedCorrectlyBumpUpClosure
        {
            get
            {
                return _wasDebitAmountMentionedCorrectlyBumpUpClosure;
            }
            set
            {
                SetProperty(ref _wasDebitAmountMentionedCorrectlyBumpUpClosure, value, () => WasDebitAmountMentionedCorrectlyBumpUpClosure);
            }
        }

        public bool? WasFirstDebitDateExplainedCorrectlyBumpUpClosure
        {
            get
            {
                return _wasFirstDebitDateExplainedCorrectlyBumpUpClosure;
            }
            set
            {
                SetProperty(ref _wasFirstDebitDateExplainedCorrectlyBumpUpClosure, value, () => WasFirstDebitDateExplainedCorrectlyBumpUpClosure);
            }
        }

        public bool? WasCorrectCoverCommencementDateMentionedBumpUpClosure
        {
            get
            {
                return _wasCorrectCoverCommencementDateMentionedBumpUpClosure;
            }
            set
            {
                SetProperty(ref _wasCorrectCoverCommencementDateMentionedBumpUpClosure, value, () => WasCorrectCoverCommencementDateMentionedBumpUpClosure);
            }
        }

        public bool? WasNonPaymentProcedureExplainedBumpUpClosure
        {
            get
            {
                return _wasNonPaymentProcedureExplainedBumpUpClosure;
            }
            set
            {
                SetProperty(ref _wasNonPaymentProcedureExplainedBumpUpClosure, value, () => WasNonPaymentProcedureExplainedBumpUpClosure);
            }
        }

        public bool? WasAnnualIncreaseExplainedBumpUpClosure
        {
            get
            {
                return _wasAnnualIncreaseExplainedBumpUpClosure;
            }
            set
            {
                SetProperty(ref _wasAnnualIncreaseExplainedBumpUpClosure, value, () => WasAnnualIncreaseExplainedBumpUpClosure);
            }
        }

        public bool? WasNextOfKinQuestionAsked
        {
            get
            {
                return _wasNextOfKinQuestionAsked;
            }
            set
            {
                SetProperty(ref _wasNextOfKinQuestionAsked, value, () => WasNextOfKinQuestionAsked);
            }
        }

        public bool? WasPermissionQuestionAsked
        {
            get
            {
                return _wasPermissionQuestionAsked;
            }
            set
            {
                SetProperty(ref _wasPermissionQuestionAsked, value, () => WasPermissionQuestionAsked);
            }
        }

        public long? FKINCallMonitoringOutcomeID
        {
            get
            {
                return _fKINCallMonitoringOutcomeID;
            }
            set
            {
                SetProperty(ref _fKINCallMonitoringOutcomeID, value, () => FKINCallMonitoringOutcomeID);
            }
        }

        public string Comments
        {
            get
            {
                return _comments;
            }
            set
            {
                SetProperty(ref _comments, value, () => Comments);
            }
        }

        public long? FKCallMonitoringUserID
        {
            get
            {
                return _fKCallMonitoringUserID;
            }
            set
            {
                SetProperty(ref _fKCallMonitoringUserID, value, () => FKCallMonitoringUserID);
            }
        }

        public bool? WasCallEvaluatedBySecondaryUser
        {
            get
            {
                return _wasCallEvaluatedBySecondaryUser;
            }
            set
            {
                SetProperty(ref _wasCallEvaluatedBySecondaryUser, value, () => WasCallEvaluatedBySecondaryUser);
            }
        }


        public bool? IsRecoveredSale
        {
            get
            {
                return _isRecoveredSale;
            }
            set
            {
                SetProperty(ref _isRecoveredSale, value, () => IsRecoveredSale);
            }
        }

        public bool? IsCallMonitored
        {
            get
            {
                return _isCallMonitored;
            }
            set
            {
                SetProperty(ref _isCallMonitored, value, () => IsCallMonitored);
            }
        }

        public long? FKSecondaryCallMonitoringUserID
        {
            get
            {
                return _fKSecondaryCallMonitoringUserID;
            }
            set
            {
                SetProperty(ref _fKSecondaryCallMonitoringUserID, value, () => FKSecondaryCallMonitoringUserID);
            }
        }

        public long? FKTertiaryCallMonitoringUserID
        {
            get
            {
                return _fKTertiaryCallMonitoringUserID;
            }
            set
            {
                SetProperty(ref _fKTertiaryCallMonitoringUserID, value, () => FKTertiaryCallMonitoringUserID);
            }
        }

        public long? TSRBUSavedCarriedForwardAssignedByUserID
        {
            get
            {
                return _tsrBUSavedCarriedForwardAssignedByUserID;
            }
            set
            {
                SetProperty(ref _tsrBUSavedCarriedForwardAssignedByUserID, value, () => TSRBUSavedCarriedForwardAssignedByUserID);
            }
        }

        public bool? IsTSRBUSavedCarriedForward
        {
            get
            {
                return _isTSRBUSavedCarriedForward;
            }
            set
            {
                SetProperty(ref _isTSRBUSavedCarriedForward, value, () => IsTSRBUSavedCarriedForward);
            }
        }

        public DateTime? TSRBUSavedCarriedForwardDate
        {
            get
            {
                return _tSRBUSavedCarriedForwardDate;
            }
            set
            {
                SetProperty(ref _tSRBUSavedCarriedForwardDate, value, () => TSRBUSavedCarriedForwardDate);
            }
        }

        public DateTime? CallMonitoredDate
        {
            get
            {
                return _callMonitoredDate;
            }
            set
            {
                SetProperty(ref _callMonitoredDate, value, () => CallMonitoredDate);
            }
        }

        public long? FKINCallAssessmentOutcomeID
        {
            get
            {
                return _fKINCallAssessmentOutcomeID;
            }
            set
            {
                SetProperty(ref _fKINCallAssessmentOutcomeID, value, () => FKINCallAssessmentOutcomeID);
            }
        }

        public decimal TotalCost
        {
            get
            {
                return _totalCost;
            }
            set
            {
                SetProperty(ref _totalCost, value, () => TotalCost);
            }
        }

        public string NextOfKinDetails
        {
            get
            {
                return _nextOfKinDetails;
            }
            set
            {
                SetProperty(ref _nextOfKinDetails, value, () => NextOfKinDetails);
            }
        }

        public bool? ExclusionsExplained
        {
            get
            {
                return _exclusionsExplained;
            }
            set
            {
                SetProperty(ref _exclusionsExplained, value, () => ExclusionsExplained);
            }
        }

        public bool? ExclusionsExplainedBumpUpClosure
        {
            get
            {
                return _exclusionsExplainedBumpUpClosure;
            }
            set
            {
                SetProperty(ref _exclusionsExplainedBumpUpClosure, value, () => ExclusionsExplainedBumpUpClosure);
            }
        }

        #endregion Publicly-Exposed Properties

        #region Constructor

        public CallMonitoringData()
        {
            
        }

        #endregion Constructor

        #region Public Methods

        public void Clear()
        {

        }

        #endregion Public Methods

        #region Private Methods

        #endregion Private Methods

    }
}
