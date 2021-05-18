using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using UDM.Insurance.Business.Mapping;
using Embriant.Framework.Configuration;
using Embriant.Framework;
using Embriant.Framework.Validation;

namespace UDM.Insurance.Business
{
    public partial class INImportCallMonitoring : ObjectBase<long>
    {
        #region Members
        private long? _fkinimportid = null;
        private bool? _isbankingdetailscapturedcorrectly = null;
        private bool? _wasaccountverified = null;
        private bool? _wasdebitdateconfirmed = null;
        private bool? _isaccountinclientsname = null;
        private bool? _doesclienthavesigningpower = null;
        private bool? _wascorrectclosurequestionasked = null;
        private bool? _wasresponseclearandpositive = null;
        private bool? _wasudmandplmentionedasfsps = null;
        private bool? _wasdebitamountmentionedcorrectly = null;
        private bool? _wasfirstdebitdateexplainedcorrectly = null;
        private bool? _wascorrectcovercommencementdatementioned = null;
        private bool? _wasnonpaymentprocedureexplained = null;
        private bool? _wasannualincreaseexplained = null;
        private bool? _wascorrectquestionaskedbumpupclosure = null;
        private bool? _wasresponseclearandpositivebumpupclosure = null;
        private bool? _wasudmandplmentionedasfspsbumpupclosure = null;
        private bool? _wasdebitamountmentionedcorrectlybumpupclosure = null;
        private bool? _wasfirstdebitdateexplainedcorrectlybumpupclosure = null;
        private bool? _wascorrectcovercommencementdatementionedbumpupclosure = null;
        private bool? _wasnonpaymentprocedureexplainedbumpupclosure = null;
        private bool? _wasannualincreaseexplainedbumpupclosure = null;
        private long? _fkincallmonitoringoutcomeid = null;
        private string _comments = null;
        private long? _fkcallmonitoringuserid = null;
        private bool? _wascallevaluatedbysecondaryuser = null;
        private long? _fksecondarycallmonitoringuserid = null;
        private long? _fkincallassessmentoutcomeid = null;
        private bool? _exclusionsexplained = null;
        private bool? _exclusionsexplainedbumpupclosure = null;
        private bool? _isrecoveredsale = null;
        private bool? _wasmoneybackvsvoucherbenefitsexplainedcorrectly = null;
        private bool? _iscallmonitored = null;
        private bool? _wasclearyesgiveninsalesquestion = null;
        private bool? _waspermissionquestionasked = null;
        private bool? _wasnextofkinquestionasked = null;
        private long? _fktertiarycallmonitoringuserid = null;
        private bool? _istsrbusavedcarriedforward = null;
        private DateTime? _tsrbusavedcarriedforwarddate = null;
        private long? _tsrbusavedcarriedforwardassignedbyuserid = null;
        private DateTime? _callmonitoreddate = null;
        private bool? _wasdebicheckprocessexplainedcorrectly = null; 
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INImportCallMonitoring class.
        /// </summary>
        public INImportCallMonitoring()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INImportCallMonitoring class.
        /// </summary>
        public INImportCallMonitoring(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKINImportID
        {
            get
            {
                Fill();
                return _fkinimportid;
            }
            set 
            {
                Fill();
                if (value != _fkinimportid)
                {
                    _fkinimportid = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsBankingDetailsCapturedCorrectly
        {
            get
            {
                Fill();
                return _isbankingdetailscapturedcorrectly;
            }
            set 
            {
                Fill();
                if (value != _isbankingdetailscapturedcorrectly)
                {
                    _isbankingdetailscapturedcorrectly = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasAccountVerified
        {
            get
            {
                Fill();
                return _wasaccountverified;
            }
            set 
            {
                Fill();
                if (value != _wasaccountverified)
                {
                    _wasaccountverified = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasDebitDateConfirmed
        {
            get
            {
                Fill();
                return _wasdebitdateconfirmed;
            }
            set 
            {
                Fill();
                if (value != _wasdebitdateconfirmed)
                {
                    _wasdebitdateconfirmed = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsAccountInClientsName
        {
            get
            {
                Fill();
                return _isaccountinclientsname;
            }
            set 
            {
                Fill();
                if (value != _isaccountinclientsname)
                {
                    _isaccountinclientsname = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? DoesClientHaveSigningPower
        {
            get
            {
                Fill();
                return _doesclienthavesigningpower;
            }
            set 
            {
                Fill();
                if (value != _doesclienthavesigningpower)
                {
                    _doesclienthavesigningpower = value;
                    _hasChanged = true;
                }
            }
        }


        public bool? WasDebiCheckProcessExplainedCorrectly
        {
            get
            {
                Fill();
                return _wasdebicheckprocessexplainedcorrectly;
            }
            set
            {
                Fill();
                if (value != _wasdebicheckprocessexplainedcorrectly)
                {
                    _wasdebicheckprocessexplainedcorrectly = value;
                    _hasChanged = true;
                }
            }
        }


        


        public bool? WasCorrectClosureQuestionAsked
        {
            get
            {
                Fill();
                return _wascorrectclosurequestionasked;
            }
            set 
            {
                Fill();
                if (value != _wascorrectclosurequestionasked)
                {
                    _wascorrectclosurequestionasked = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasResponseClearAndPositive
        {
            get
            {
                Fill();
                return _wasresponseclearandpositive;
            }
            set 
            {
                Fill();
                if (value != _wasresponseclearandpositive)
                {
                    _wasresponseclearandpositive = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasUDMAndPLMentionedAsFSPs
        {
            get
            {
                Fill();
                return _wasudmandplmentionedasfsps;
            }
            set 
            {
                Fill();
                if (value != _wasudmandplmentionedasfsps)
                {
                    _wasudmandplmentionedasfsps = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasDebitAmountMentionedCorrectly
        {
            get
            {
                Fill();
                return _wasdebitamountmentionedcorrectly;
            }
            set 
            {
                Fill();
                if (value != _wasdebitamountmentionedcorrectly)
                {
                    _wasdebitamountmentionedcorrectly = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasFirstDebitDateExplainedCorrectly
        {
            get
            {
                Fill();
                return _wasfirstdebitdateexplainedcorrectly;
            }
            set 
            {
                Fill();
                if (value != _wasfirstdebitdateexplainedcorrectly)
                {
                    _wasfirstdebitdateexplainedcorrectly = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasCorrectCoverCommencementDateMentioned
        {
            get
            {
                Fill();
                return _wascorrectcovercommencementdatementioned;
            }
            set 
            {
                Fill();
                if (value != _wascorrectcovercommencementdatementioned)
                {
                    _wascorrectcovercommencementdatementioned = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasNonPaymentProcedureExplained
        {
            get
            {
                Fill();
                return _wasnonpaymentprocedureexplained;
            }
            set 
            {
                Fill();
                if (value != _wasnonpaymentprocedureexplained)
                {
                    _wasnonpaymentprocedureexplained = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasAnnualIncreaseExplained
        {
            get
            {
                Fill();
                return _wasannualincreaseexplained;
            }
            set 
            {
                Fill();
                if (value != _wasannualincreaseexplained)
                {
                    _wasannualincreaseexplained = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasCorrectQuestionAskedBumpUpClosure
        {
            get
            {
                Fill();
                return _wascorrectquestionaskedbumpupclosure;
            }
            set 
            {
                Fill();
                if (value != _wascorrectquestionaskedbumpupclosure)
                {
                    _wascorrectquestionaskedbumpupclosure = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasResponseClearAndPositiveBumpUpClosure
        {
            get
            {
                Fill();
                return _wasresponseclearandpositivebumpupclosure;
            }
            set 
            {
                Fill();
                if (value != _wasresponseclearandpositivebumpupclosure)
                {
                    _wasresponseclearandpositivebumpupclosure = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasUDMAndPLMentionedAsFSPsBumpUpClosure
        {
            get
            {
                Fill();
                return _wasudmandplmentionedasfspsbumpupclosure;
            }
            set 
            {
                Fill();
                if (value != _wasudmandplmentionedasfspsbumpupclosure)
                {
                    _wasudmandplmentionedasfspsbumpupclosure = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasDebitAmountMentionedCorrectlyBumpUpClosure
        {
            get
            {
                Fill();
                return _wasdebitamountmentionedcorrectlybumpupclosure;
            }
            set 
            {
                Fill();
                if (value != _wasdebitamountmentionedcorrectlybumpupclosure)
                {
                    _wasdebitamountmentionedcorrectlybumpupclosure = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasFirstDebitDateExplainedCorrectlyBumpUpClosure
        {
            get
            {
                Fill();
                return _wasfirstdebitdateexplainedcorrectlybumpupclosure;
            }
            set 
            {
                Fill();
                if (value != _wasfirstdebitdateexplainedcorrectlybumpupclosure)
                {
                    _wasfirstdebitdateexplainedcorrectlybumpupclosure = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasCorrectCoverCommencementDateMentionedBumpUpClosure
        {
            get
            {
                Fill();
                return _wascorrectcovercommencementdatementionedbumpupclosure;
            }
            set 
            {
                Fill();
                if (value != _wascorrectcovercommencementdatementionedbumpupclosure)
                {
                    _wascorrectcovercommencementdatementionedbumpupclosure = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasNonPaymentProcedureExplainedBumpUpClosure
        {
            get
            {
                Fill();
                return _wasnonpaymentprocedureexplainedbumpupclosure;
            }
            set 
            {
                Fill();
                if (value != _wasnonpaymentprocedureexplainedbumpupclosure)
                {
                    _wasnonpaymentprocedureexplainedbumpupclosure = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasAnnualIncreaseExplainedBumpUpClosure
        {
            get
            {
                Fill();
                return _wasannualincreaseexplainedbumpupclosure;
            }
            set 
            {
                Fill();
                if (value != _wasannualincreaseexplainedbumpupclosure)
                {
                    _wasannualincreaseexplainedbumpupclosure = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINCallMonitoringOutcomeID
        {
            get
            {
                Fill();
                return _fkincallmonitoringoutcomeid;
            }
            set 
            {
                Fill();
                if (value != _fkincallmonitoringoutcomeid)
                {
                    _fkincallmonitoringoutcomeid = value;
                    _hasChanged = true;
                }
            }
        }

        public string Comments
        {
            get
            {
                Fill();
                return _comments;
            }
            set 
            {
                Fill();
                if (value != _comments)
                {
                    _comments = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKCallMonitoringUserID
        {
            get
            {
                Fill();
                return _fkcallmonitoringuserid;
            }
            set 
            {
                Fill();
                if (value != _fkcallmonitoringuserid)
                {
                    _fkcallmonitoringuserid = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasCallEvaluatedBySecondaryUser
        {
            get
            {
                Fill();
                return _wascallevaluatedbysecondaryuser;
            }
            set 
            {
                Fill();
                if (value != _wascallevaluatedbysecondaryuser)
                {
                    _wascallevaluatedbysecondaryuser = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKSecondaryCallMonitoringUserID
        {
            get
            {
                Fill();
                return _fksecondarycallmonitoringuserid;
            }
            set 
            {
                Fill();
                if (value != _fksecondarycallmonitoringuserid)
                {
                    _fksecondarycallmonitoringuserid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINCallAssessmentOutcomeID
        {
            get
            {
                Fill();
                return _fkincallassessmentoutcomeid;
            }
            set 
            {
                Fill();
                if (value != _fkincallassessmentoutcomeid)
                {
                    _fkincallassessmentoutcomeid = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? ExclusionsExplained
        {
            get
            {
                Fill();
                return _exclusionsexplained;
            }
            set 
            {
                Fill();
                if (value != _exclusionsexplained)
                {
                    _exclusionsexplained = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? ExclusionsExplainedBumpUpClosure
        {
            get
            {
                Fill();
                return _exclusionsexplainedbumpupclosure;
            }
            set 
            {
                Fill();
                if (value != _exclusionsexplainedbumpupclosure)
                {
                    _exclusionsexplainedbumpupclosure = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsRecoveredSale
        {
            get
            {
                Fill();
                return _isrecoveredsale;
            }
            set 
            {
                Fill();
                if (value != _isrecoveredsale)
                {
                    _isrecoveredsale = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasMoneyBackVsVoucherBenefitsExplainedCorrectly
        {
            get
            {
                Fill();
                return _wasmoneybackvsvoucherbenefitsexplainedcorrectly;
            }
            set 
            {
                Fill();
                if (value != _wasmoneybackvsvoucherbenefitsexplainedcorrectly)
                {
                    _wasmoneybackvsvoucherbenefitsexplainedcorrectly = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsCallMonitored
        {
            get
            {
                Fill();
                return _iscallmonitored;
            }
            set 
            {
                Fill();
                if (value != _iscallmonitored)
                {
                    _iscallmonitored = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasClearYesGivenInSalesQuestion
        {
            get
            {
                Fill();
                return _wasclearyesgiveninsalesquestion;
            }
            set 
            {
                Fill();
                if (value != _wasclearyesgiveninsalesquestion)
                {
                    _wasclearyesgiveninsalesquestion = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasPermissionQuestionAsked
        {
            get
            {
                Fill();
                return _waspermissionquestionasked;
            }
            set 
            {
                Fill();
                if (value != _waspermissionquestionasked)
                {
                    _waspermissionquestionasked = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WasNextOfKinQuestionAsked
        {
            get
            {
                Fill();
                return _wasnextofkinquestionasked;
            }
            set 
            {
                Fill();
                if (value != _wasnextofkinquestionasked)
                {
                    _wasnextofkinquestionasked = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKTertiaryCallMonitoringUserID
        {
            get
            {
                Fill();
                return _fktertiarycallmonitoringuserid;
            }
            set 
            {
                Fill();
                if (value != _fktertiarycallmonitoringuserid)
                {
                    _fktertiarycallmonitoringuserid = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsTSRBUSavedCarriedForward
        {
            get
            {
                Fill();
                return _istsrbusavedcarriedforward;
            }
            set 
            {
                Fill();
                if (value != _istsrbusavedcarriedforward)
                {
                    _istsrbusavedcarriedforward = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? TSRBUSavedCarriedForwardDate
        {
            get
            {
                Fill();
                return _tsrbusavedcarriedforwarddate;
            }
            set 
            {
                Fill();
                if (value != _tsrbusavedcarriedforwarddate)
                {
                    _tsrbusavedcarriedforwarddate = value;
                    _hasChanged = true;
                }
            }
        }

        public long? TSRBUSavedCarriedForwardAssignedByUserID
        {
            get
            {
                Fill();
                return _tsrbusavedcarriedforwardassignedbyuserid;
            }
            set 
            {
                Fill();
                if (value != _tsrbusavedcarriedforwardassignedbyuserid)
                {
                    _tsrbusavedcarriedforwardassignedbyuserid = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? CallMonitoredDate
        {
            get
            {
                Fill();
                return _callmonitoreddate;
            }
            set 
            {
                Fill();
                if (value != _callmonitoreddate)
                {
                    _callmonitoreddate = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INImportCallMonitoring object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INImportCallMonitoringMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INImportCallMonitoring object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportCallMonitoring object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INImportCallMonitoringMapper.Save(this);
                    result &= _isLoaded;
                }
                _hasChanged = !result;
                return result && AfterSave(validationResult);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes an INImportCallMonitoring object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportCallMonitoring object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INImportCallMonitoringMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INImportCallMonitoring.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inimportcallmonitoring>");
            xml.Append("<fkinimportid>" + FKINImportID.ToString() + "</fkinimportid>");
            xml.Append("<isbankingdetailscapturedcorrectly>" + IsBankingDetailsCapturedCorrectly.ToString() + "</isbankingdetailscapturedcorrectly>");
            xml.Append("<wasaccountverified>" + WasAccountVerified.ToString() + "</wasaccountverified>");
            xml.Append("<wasdebitdateconfirmed>" + WasDebitDateConfirmed.ToString() + "</wasdebitdateconfirmed>");
            xml.Append("<isaccountinclientsname>" + IsAccountInClientsName.ToString() + "</isaccountinclientsname>");
            xml.Append("<doesclienthavesigningpower>" + DoesClientHaveSigningPower.ToString() + "</doesclienthavesigningpower>");
            xml.Append("<wascorrectclosurequestionasked>" + WasCorrectClosureQuestionAsked.ToString() + "</wascorrectclosurequestionasked>");
            xml.Append("<wasresponseclearandpositive>" + WasResponseClearAndPositive.ToString() + "</wasresponseclearandpositive>");
            xml.Append("<wasudmandplmentionedasfsps>" + WasUDMAndPLMentionedAsFSPs.ToString() + "</wasudmandplmentionedasfsps>");
            xml.Append("<wasdebitamountmentionedcorrectly>" + WasDebitAmountMentionedCorrectly.ToString() + "</wasdebitamountmentionedcorrectly>");
            xml.Append("<wasfirstdebitdateexplainedcorrectly>" + WasFirstDebitDateExplainedCorrectly.ToString() + "</wasfirstdebitdateexplainedcorrectly>");
            xml.Append("<wascorrectcovercommencementdatementioned>" + WasCorrectCoverCommencementDateMentioned.ToString() + "</wascorrectcovercommencementdatementioned>");
            xml.Append("<wasnonpaymentprocedureexplained>" + WasNonPaymentProcedureExplained.ToString() + "</wasnonpaymentprocedureexplained>");
            xml.Append("<wasannualincreaseexplained>" + WasAnnualIncreaseExplained.ToString() + "</wasannualincreaseexplained>");
            xml.Append("<wascorrectquestionaskedbumpupclosure>" + WasCorrectQuestionAskedBumpUpClosure.ToString() + "</wascorrectquestionaskedbumpupclosure>");
            xml.Append("<wasresponseclearandpositivebumpupclosure>" + WasResponseClearAndPositiveBumpUpClosure.ToString() + "</wasresponseclearandpositivebumpupclosure>");
            xml.Append("<wasudmandplmentionedasfspsbumpupclosure>" + WasUDMAndPLMentionedAsFSPsBumpUpClosure.ToString() + "</wasudmandplmentionedasfspsbumpupclosure>");
            xml.Append("<wasdebitamountmentionedcorrectlybumpupclosure>" + WasDebitAmountMentionedCorrectlyBumpUpClosure.ToString() + "</wasdebitamountmentionedcorrectlybumpupclosure>");
            xml.Append("<wasfirstdebitdateexplainedcorrectlybumpupclosure>" + WasFirstDebitDateExplainedCorrectlyBumpUpClosure.ToString() + "</wasfirstdebitdateexplainedcorrectlybumpupclosure>");
            xml.Append("<wascorrectcovercommencementdatementionedbumpupclosure>" + WasCorrectCoverCommencementDateMentionedBumpUpClosure.ToString() + "</wascorrectcovercommencementdatementionedbumpupclosure>");
            xml.Append("<wasnonpaymentprocedureexplainedbumpupclosure>" + WasNonPaymentProcedureExplainedBumpUpClosure.ToString() + "</wasnonpaymentprocedureexplainedbumpupclosure>");
            xml.Append("<wasannualincreaseexplainedbumpupclosure>" + WasAnnualIncreaseExplainedBumpUpClosure.ToString() + "</wasannualincreaseexplainedbumpupclosure>");
            xml.Append("<fkincallmonitoringoutcomeid>" + FKINCallMonitoringOutcomeID.ToString() + "</fkincallmonitoringoutcomeid>");
            xml.Append("<comments>" + Comments.ToString() + "</comments>");
            xml.Append("<fkcallmonitoringuserid>" + FKCallMonitoringUserID.ToString() + "</fkcallmonitoringuserid>");
            xml.Append("<wascallevaluatedbysecondaryuser>" + WasCallEvaluatedBySecondaryUser.ToString() + "</wascallevaluatedbysecondaryuser>");
            xml.Append("<fksecondarycallmonitoringuserid>" + FKSecondaryCallMonitoringUserID.ToString() + "</fksecondarycallmonitoringuserid>");
            xml.Append("<fkincallassessmentoutcomeid>" + FKINCallAssessmentOutcomeID.ToString() + "</fkincallassessmentoutcomeid>");
            xml.Append("<exclusionsexplained>" + ExclusionsExplained.ToString() + "</exclusionsexplained>");
            xml.Append("<exclusionsexplainedbumpupclosure>" + ExclusionsExplainedBumpUpClosure.ToString() + "</exclusionsexplainedbumpupclosure>");
            xml.Append("<isrecoveredsale>" + IsRecoveredSale.ToString() + "</isrecoveredsale>");
            xml.Append("<wasmoneybackvsvoucherbenefitsexplainedcorrectly>" + WasMoneyBackVsVoucherBenefitsExplainedCorrectly.ToString() + "</wasmoneybackvsvoucherbenefitsexplainedcorrectly>");
            xml.Append("<iscallmonitored>" + IsCallMonitored.ToString() + "</iscallmonitored>");
            xml.Append("<wasclearyesgiveninsalesquestion>" + WasClearYesGivenInSalesQuestion.ToString() + "</wasclearyesgiveninsalesquestion>");
            xml.Append("<waspermissionquestionasked>" + WasPermissionQuestionAsked.ToString() + "</waspermissionquestionasked>");
            xml.Append("<wasnextofkinquestionasked>" + WasNextOfKinQuestionAsked.ToString() + "</wasnextofkinquestionasked>");
            xml.Append("<fktertiarycallmonitoringuserid>" + FKTertiaryCallMonitoringUserID.ToString() + "</fktertiarycallmonitoringuserid>");
            xml.Append("<istsrbusavedcarriedforward>" + IsTSRBUSavedCarriedForward.ToString() + "</istsrbusavedcarriedforward>");
            xml.Append("<tsrbusavedcarriedforwarddate>" + TSRBUSavedCarriedForwardDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</tsrbusavedcarriedforwarddate>");
            xml.Append("<tsrbusavedcarriedforwardassignedbyuserid>" + TSRBUSavedCarriedForwardAssignedByUserID.ToString() + "</tsrbusavedcarriedforwardassignedbyuserid>");
            xml.Append("<callmonitoreddate>" + CallMonitoredDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</callmonitoreddate>");
            xml.Append("</inimportcallmonitoring>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INImportCallMonitoring object from a list of parameters.
        /// </summary>
        /// <param name="fkinimportid"></param>
        /// <param name="isbankingdetailscapturedcorrectly"></param>
        /// <param name="wasaccountverified"></param>
        /// <param name="wasdebitdateconfirmed"></param>
        /// <param name="isaccountinclientsname"></param>
        /// <param name="doesclienthavesigningpower"></param>
        /// <param name="wascorrectclosurequestionasked"></param>
        /// <param name="wasresponseclearandpositive"></param>
        /// <param name="wasudmandplmentionedasfsps"></param>
        /// <param name="wasdebitamountmentionedcorrectly"></param>
        /// <param name="wasfirstdebitdateexplainedcorrectly"></param>
        /// <param name="wascorrectcovercommencementdatementioned"></param>
        /// <param name="wasnonpaymentprocedureexplained"></param>
        /// <param name="wasannualincreaseexplained"></param>
        /// <param name="wascorrectquestionaskedbumpupclosure"></param>
        /// <param name="wasresponseclearandpositivebumpupclosure"></param>
        /// <param name="wasudmandplmentionedasfspsbumpupclosure"></param>
        /// <param name="wasdebitamountmentionedcorrectlybumpupclosure"></param>
        /// <param name="wasfirstdebitdateexplainedcorrectlybumpupclosure"></param>
        /// <param name="wascorrectcovercommencementdatementionedbumpupclosure"></param>
        /// <param name="wasnonpaymentprocedureexplainedbumpupclosure"></param>
        /// <param name="wasannualincreaseexplainedbumpupclosure"></param>
        /// <param name="fkincallmonitoringoutcomeid"></param>
        /// <param name="comments"></param>
        /// <param name="fkcallmonitoringuserid"></param>
        /// <param name="wascallevaluatedbysecondaryuser"></param>
        /// <param name="fksecondarycallmonitoringuserid"></param>
        /// <param name="fkincallassessmentoutcomeid"></param>
        /// <param name="exclusionsexplained"></param>
        /// <param name="exclusionsexplainedbumpupclosure"></param>
        /// <param name="isrecoveredsale"></param>
        /// <param name="wasmoneybackvsvoucherbenefitsexplainedcorrectly"></param>
        /// <param name="iscallmonitored"></param>
        /// <param name="wasclearyesgiveninsalesquestion"></param>
        /// <param name="waspermissionquestionasked"></param>
        /// <param name="wasnextofkinquestionasked"></param>
        /// <param name="fktertiarycallmonitoringuserid"></param>
        /// <param name="istsrbusavedcarriedforward"></param>
        /// <param name="tsrbusavedcarriedforwarddate"></param>
        /// <param name="tsrbusavedcarriedforwardassignedbyuserid"></param>
        /// <param name="callmonitoreddate"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinimportid, bool? isbankingdetailscapturedcorrectly, bool? wasaccountverified, bool? wasdebitdateconfirmed, bool? isaccountinclientsname, bool? doesclienthavesigningpower, bool? wascorrectclosurequestionasked, bool? wasresponseclearandpositive, bool? wasudmandplmentionedasfsps, bool? wasdebitamountmentionedcorrectly, bool? wasfirstdebitdateexplainedcorrectly, bool? wascorrectcovercommencementdatementioned, bool? wasnonpaymentprocedureexplained, bool? wasannualincreaseexplained, bool? wascorrectquestionaskedbumpupclosure, bool? wasresponseclearandpositivebumpupclosure, bool? wasudmandplmentionedasfspsbumpupclosure, bool? wasdebitamountmentionedcorrectlybumpupclosure, bool? wasfirstdebitdateexplainedcorrectlybumpupclosure, bool? wascorrectcovercommencementdatementionedbumpupclosure, bool? wasnonpaymentprocedureexplainedbumpupclosure, bool? wasannualincreaseexplainedbumpupclosure, long? fkincallmonitoringoutcomeid, string comments, long? fkcallmonitoringuserid, bool? wascallevaluatedbysecondaryuser, long? fksecondarycallmonitoringuserid, long? fkincallassessmentoutcomeid, bool? exclusionsexplained, bool? exclusionsexplainedbumpupclosure, bool? isrecoveredsale, bool? wasmoneybackvsvoucherbenefitsexplainedcorrectly, bool? iscallmonitored, bool? wasclearyesgiveninsalesquestion, bool? waspermissionquestionasked, bool? wasnextofkinquestionasked, long? fktertiarycallmonitoringuserid, bool? istsrbusavedcarriedforward, DateTime? tsrbusavedcarriedforwarddate, long? tsrbusavedcarriedforwardassignedbyuserid, DateTime? callmonitoreddate)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINImportID = fkinimportid;
                this.IsBankingDetailsCapturedCorrectly = isbankingdetailscapturedcorrectly;
                this.WasAccountVerified = wasaccountverified;
                this.WasDebitDateConfirmed = wasdebitdateconfirmed;
                this.IsAccountInClientsName = isaccountinclientsname;
                this.DoesClientHaveSigningPower = doesclienthavesigningpower;
                this.WasCorrectClosureQuestionAsked = wascorrectclosurequestionasked;
                this.WasResponseClearAndPositive = wasresponseclearandpositive;
                this.WasUDMAndPLMentionedAsFSPs = wasudmandplmentionedasfsps;
                this.WasDebitAmountMentionedCorrectly = wasdebitamountmentionedcorrectly;
                this.WasFirstDebitDateExplainedCorrectly = wasfirstdebitdateexplainedcorrectly;
                this.WasCorrectCoverCommencementDateMentioned = wascorrectcovercommencementdatementioned;
                this.WasNonPaymentProcedureExplained = wasnonpaymentprocedureexplained;
                this.WasAnnualIncreaseExplained = wasannualincreaseexplained;
                this.WasCorrectQuestionAskedBumpUpClosure = wascorrectquestionaskedbumpupclosure;
                this.WasResponseClearAndPositiveBumpUpClosure = wasresponseclearandpositivebumpupclosure;
                this.WasUDMAndPLMentionedAsFSPsBumpUpClosure = wasudmandplmentionedasfspsbumpupclosure;
                this.WasDebitAmountMentionedCorrectlyBumpUpClosure = wasdebitamountmentionedcorrectlybumpupclosure;
                this.WasFirstDebitDateExplainedCorrectlyBumpUpClosure = wasfirstdebitdateexplainedcorrectlybumpupclosure;
                this.WasCorrectCoverCommencementDateMentionedBumpUpClosure = wascorrectcovercommencementdatementionedbumpupclosure;
                this.WasNonPaymentProcedureExplainedBumpUpClosure = wasnonpaymentprocedureexplainedbumpupclosure;
                this.WasAnnualIncreaseExplainedBumpUpClosure = wasannualincreaseexplainedbumpupclosure;
                this.FKINCallMonitoringOutcomeID = fkincallmonitoringoutcomeid;
                this.Comments = comments;
                this.FKCallMonitoringUserID = fkcallmonitoringuserid;
                this.WasCallEvaluatedBySecondaryUser = wascallevaluatedbysecondaryuser;
                this.FKSecondaryCallMonitoringUserID = fksecondarycallmonitoringuserid;
                this.FKINCallAssessmentOutcomeID = fkincallassessmentoutcomeid;
                this.ExclusionsExplained = exclusionsexplained;
                this.ExclusionsExplainedBumpUpClosure = exclusionsexplainedbumpupclosure;
                this.IsRecoveredSale = isrecoveredsale;
                this.WasMoneyBackVsVoucherBenefitsExplainedCorrectly = wasmoneybackvsvoucherbenefitsexplainedcorrectly;
                this.IsCallMonitored = iscallmonitored;
                this.WasClearYesGivenInSalesQuestion = wasclearyesgiveninsalesquestion;
                this.WasPermissionQuestionAsked = waspermissionquestionasked;
                this.WasNextOfKinQuestionAsked = wasnextofkinquestionasked;
                this.FKTertiaryCallMonitoringUserID = fktertiarycallmonitoringuserid;
                this.IsTSRBUSavedCarriedForward = istsrbusavedcarriedforward;
                this.TSRBUSavedCarriedForwardDate = tsrbusavedcarriedforwarddate;
                this.TSRBUSavedCarriedForwardAssignedByUserID = tsrbusavedcarriedforwardassignedbyuserid;
                this.CallMonitoredDate = callmonitoreddate;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INImportCallMonitoring's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportCallMonitoring history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportCallMonitoringMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INImportCallMonitoring object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportCallMonitoring object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportCallMonitoringMapper.UnDelete(this);
            }
            else
            {
                return false;
            }
        }
        #endregion
    }

    #region Collection
    /// <summary>
    /// A collection of the INImportCallMonitoring object.
    /// </summary>
    public partial class INImportCallMonitoringCollection : ObjectCollection<INImportCallMonitoring>
    { 
    }
    #endregion
}
