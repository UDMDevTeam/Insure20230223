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
    public partial class INImport : ObjectBase<long>
    {
        #region Members
        private long? _fkuserid = null;
        private long? _fkincampaignid = null;
        private long? _fkinbatchid = null;
        private long? _fkinleadstatusid = null;
        private long? _fkindeclinereasonid = null;
        private long? _fkinpolicyid = null;
        private long? _fkinleadid = null;
        private string _refno = null;
        private string _referrorpolicyid = null;
        private long? _fkinreferrortitleid = null;
        private string _referror = null;
        private long? _fkinreferrorrelationshipid = null;
        private string _referrorcontact = null;
        private string _gift = null;
        private DateTime? _platinumcontactdate = null;
        private TimeSpan? _platinumcontacttime = null;
        private string _canceroption = null;
        private short? _platinumage = null;
        private DateTime? _allocationdate = null;
        private Byte? _isprinted = null;
        private DateTime? _dateofsale = null;
        private string _bankcallref = null;
        private long? _fkbankcallrefuserid = null;
        private string _bankstationno = null;
        private DateTime? _bankdate = null;
        private TimeSpan? _banktime = null;
        private long? _fkbanktelnumbertypeid = null;
        private string _salecallref = null;
        private long? _fksalecallrefuserid = null;
        private string _salestationno = null;
        private DateTime? _saledate = null;
        private TimeSpan? _saletime = null;
        private long? _fksaletelnumbertypeid = null;
        private string _confcallref = null;
        private long? _fkconfcallrefuserid = null;
        private string _confstationno = null;
        private DateTime? _confdate = null;
        private TimeSpan? _conftime = null;
        private long? _fkconftelnumbertypeid = null;
        private bool? _isconfirmed = null;
        private string _notes = null;
        private DateTime? _importdate = null;
        private long? _fkclosureid = null;
        private string _feedback = null;
        private DateTime? _feedbackdate = null;
        private DateTime? _futurecontactdate = null;
        private long? _fkinimportedpolicydataid = null;
        private string _testing1 = null;
        private long? _fkinleadfeedbackid = null;
        private long? _fkincancellationreasonid = null;
        private bool? _iscopied = null;
        private long? _fkinconfirmationfeedbackid = null;
        private long? _fkinparentbatchid = null;
        private bool? _bonuslead = null;
        private long? _fkbatchcallrefuserid = null;
        private bool? _ismining = null;
        private DateTime? _confworkdate = null;
        private bool? _ischecked = null;
        private DateTime? _checkeddate = null;
        private DateTime? _datebatched = null;
        private bool? _iscopyduplicate = null;
        private long? _fkindiaryreasonid = null;
        private long? _fkincarriedforwardreasonid = null;
        private long? _fkincallmonitoringcarriedforwardreasonid = null;
        private bool? _permissionquestionasked = null;
        private long? _fkincallmonitoringcancellationreasonid = null;
        private bool? _isfutureallocation = null;
        private DateTime? _moneybackdate = null;
        private DateTime? _conversionmbdate = null;
        private bool? _obtainedreferrals = null;
        private string _emailstatus = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INImport class.
        /// </summary>
        public INImport()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INImport class.
        /// </summary>
        public INImport(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKUserID
        {
            get
            {
                Fill();
                return _fkuserid;
            }
            set 
            {
                Fill();
                if (value != _fkuserid)
                {
                    _fkuserid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINCampaignID
        {
            get
            {
                Fill();
                return _fkincampaignid;
            }
            set 
            {
                Fill();
                if (value != _fkincampaignid)
                {
                    _fkincampaignid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINBatchID
        {
            get
            {
                Fill();
                return _fkinbatchid;
            }
            set 
            {
                Fill();
                if (value != _fkinbatchid)
                {
                    _fkinbatchid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINLeadStatusID
        {
            get
            {
                Fill();
                return _fkinleadstatusid;
            }
            set 
            {
                Fill();
                if (value != _fkinleadstatusid)
                {
                    _fkinleadstatusid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINDeclineReasonID
        {
            get
            {
                Fill();
                return _fkindeclinereasonid;
            }
            set 
            {
                Fill();
                if (value != _fkindeclinereasonid)
                {
                    _fkindeclinereasonid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINPolicyID
        {
            get
            {
                Fill();
                return _fkinpolicyid;
            }
            set 
            {
                Fill();
                if (value != _fkinpolicyid)
                {
                    _fkinpolicyid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINLeadID
        {
            get
            {
                Fill();
                return _fkinleadid;
            }
            set 
            {
                Fill();
                if (value != _fkinleadid)
                {
                    _fkinleadid = value;
                    _hasChanged = true;
                }
            }
        }

        public string RefNo
        {
            get
            {
                Fill();
                return _refno;
            }
            set 
            {
                Fill();
                if (value != _refno)
                {
                    _refno = value;
                    _hasChanged = true;
                }
            }
        }

        public string ReferrorPolicyID
        {
            get
            {
                Fill();
                return _referrorpolicyid;
            }
            set 
            {
                Fill();
                if (value != _referrorpolicyid)
                {
                    _referrorpolicyid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINReferrorTitleID
        {
            get
            {
                Fill();
                return _fkinreferrortitleid;
            }
            set 
            {
                Fill();
                if (value != _fkinreferrortitleid)
                {
                    _fkinreferrortitleid = value;
                    _hasChanged = true;
                }
            }
        }

        public string Referror
        {
            get
            {
                Fill();
                return _referror;
            }
            set 
            {
                Fill();
                if (value != _referror)
                {
                    _referror = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINReferrorRelationshipID
        {
            get
            {
                Fill();
                return _fkinreferrorrelationshipid;
            }
            set 
            {
                Fill();
                if (value != _fkinreferrorrelationshipid)
                {
                    _fkinreferrorrelationshipid = value;
                    _hasChanged = true;
                }
            }
        }

        public string ReferrorContact
        {
            get
            {
                Fill();
                return _referrorcontact;
            }
            set 
            {
                Fill();
                if (value != _referrorcontact)
                {
                    _referrorcontact = value;
                    _hasChanged = true;
                }
            }
        }

        public string Gift
        {
            get
            {
                Fill();
                return _gift;
            }
            set 
            {
                Fill();
                if (value != _gift)
                {
                    _gift = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? PlatinumContactDate
        {
            get
            {
                Fill();
                return _platinumcontactdate;
            }
            set 
            {
                Fill();
                if (value != _platinumcontactdate)
                {
                    _platinumcontactdate = value;
                    _hasChanged = true;
                }
            }
        }

        public TimeSpan? PlatinumContactTime
        {
            get
            {
                Fill();
                return _platinumcontacttime;
            }
            set 
            {
                Fill();
                if (value != _platinumcontacttime)
                {
                    _platinumcontacttime = value;
                    _hasChanged = true;
                }
            }
        }

        public string CancerOption
        {
            get
            {
                Fill();
                return _canceroption;
            }
            set 
            {
                Fill();
                if (value != _canceroption)
                {
                    _canceroption = value;
                    _hasChanged = true;
                }
            }
        }

        public short? PlatinumAge
        {
            get
            {
                Fill();
                return _platinumage;
            }
            set 
            {
                Fill();
                if (value != _platinumage)
                {
                    _platinumage = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? AllocationDate
        {
            get
            {
                Fill();
                return _allocationdate;
            }
            set 
            {
                Fill();
                if (value != _allocationdate)
                {
                    _allocationdate = value;
                    _hasChanged = true;
                }
            }
        }

        public Byte? IsPrinted
        {
            get
            {
                Fill();
                return _isprinted;
            }
            set 
            {
                Fill();
                if (value != _isprinted)
                {
                    _isprinted = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? DateOfSale
        {
            get
            {
                Fill();
                return _dateofsale;
            }
            set 
            {
                Fill();
                if (value != _dateofsale)
                {
                    _dateofsale = value;
                    _hasChanged = true;
                }
            }
        }

        public string BankCallRef
        {
            get
            {
                Fill();
                return _bankcallref;
            }
            set 
            {
                Fill();
                if (value != _bankcallref)
                {
                    _bankcallref = value;
                    _hasChanged = true;
                }
            }
        }


        public long? FKBankCallRefUserID
        {
            get
            {
                Fill();
                return _fkbankcallrefuserid;
            }
            set 
            {
                Fill();
                if (value != _fkbankcallrefuserid)
                {
                    _fkbankcallrefuserid = value;
                    _hasChanged = true;
                }
            }
        }

        public string BankStationNo
        {
            get
            {
                Fill();
                return _bankstationno;
            }
            set 
            {
                Fill();
                if (value != _bankstationno)
                {
                    _bankstationno = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? BankDate
        {
            get
            {
                Fill();
                return _bankdate;
            }
            set 
            {
                Fill();
                if (value != _bankdate)
                {
                    _bankdate = value;
                    _hasChanged = true;
                }
            }
        }

        public TimeSpan? BankTime
        {
            get
            {
                Fill();
                return _banktime;
            }
            set 
            {
                Fill();
                if (value != _banktime)
                {
                    _banktime = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKBankTelNumberTypeID
        {
            get
            {
                Fill();
                return _fkbanktelnumbertypeid;
            }
            set 
            {
                Fill();
                if (value != _fkbanktelnumbertypeid)
                {
                    _fkbanktelnumbertypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public string SaleCallRef
        {
            get
            {
                Fill();
                return _salecallref;
            }
            set 
            {
                Fill();
                if (value != _salecallref)
                {
                    _salecallref = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKSaleCallRefUserID
        {
            get
            {
                Fill();
                return _fksalecallrefuserid;
            }
            set 
            {
                Fill();
                if (value != _fksalecallrefuserid)
                {
                    _fksalecallrefuserid = value;
                    _hasChanged = true;
                }
            }
        }

        public string SaleStationNo
        {
            get
            {
                Fill();
                return _salestationno;
            }
            set 
            {
                Fill();
                if (value != _salestationno)
                {
                    _salestationno = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? SaleDate
        {
            get
            {
                Fill();
                return _saledate;
            }
            set 
            {
                Fill();
                if (value != _saledate)
                {
                    _saledate = value;
                    _hasChanged = true;
                }
            }
        }

        public TimeSpan? SaleTime
        {
            get
            {
                Fill();
                return _saletime;
            }
            set 
            {
                Fill();
                if (value != _saletime)
                {
                    _saletime = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKSaleTelNumberTypeID
        {
            get
            {
                Fill();
                return _fksaletelnumbertypeid;
            }
            set 
            {
                Fill();
                if (value != _fksaletelnumbertypeid)
                {
                    _fksaletelnumbertypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public string ConfCallRef
        {
            get
            {
                Fill();
                return _confcallref;
            }
            set 
            {
                Fill();
                if (value != _confcallref)
                {
                    _confcallref = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKConfCallRefUserID
        {
            get
            {
                Fill();
                return _fkconfcallrefuserid;
            }
            set 
            {
                Fill();
                if (value != _fkconfcallrefuserid)
                {
                    _fkconfcallrefuserid = value;
                    _hasChanged = true;
                }
            }
        }

        public string ConfStationNo
        {
            get
            {
                Fill();
                return _confstationno;
            }
            set 
            {
                Fill();
                if (value != _confstationno)
                {
                    _confstationno = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? ConfDate
        {
            get
            {
                Fill();
                return _confdate;
            }
            set 
            {
                Fill();
                if (value != _confdate)
                {
                    _confdate = value;
                    _hasChanged = true;
                }
            }
        }

        public TimeSpan? ConfTime
        {
            get
            {
                Fill();
                return _conftime;
            }
            set 
            {
                Fill();
                if (value != _conftime)
                {
                    _conftime = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKConfTelNumberTypeID
        {
            get
            {
                Fill();
                return _fkconftelnumbertypeid;
            }
            set 
            {
                Fill();
                if (value != _fkconftelnumbertypeid)
                {
                    _fkconftelnumbertypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsConfirmed
        {
            get
            {
                Fill();
                return _isconfirmed;
            }
            set 
            {
                Fill();
                if (value != _isconfirmed)
                {
                    _isconfirmed = value;
                    _hasChanged = true;
                }
            }
        }

        public string Notes
        {
            get
            {
                Fill();
                return _notes;
            }
            set 
            {
                Fill();
                if (value != _notes)
                {
                    _notes = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? ImportDate
        {
            get
            {
                Fill();
                return _importdate;
            }
            set 
            {
                Fill();
                if (value != _importdate)
                {
                    _importdate = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKClosureID
        {
            get
            {
                Fill();
                return _fkclosureid;
            }
            set 
            {
                Fill();
                if (value != _fkclosureid)
                {
                    _fkclosureid = value;
                    _hasChanged = true;
                }
            }
        }

        public string Feedback
        {
            get
            {
                Fill();
                return _feedback;
            }
            set 
            {
                Fill();
                if (value != _feedback)
                {
                    _feedback = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? FeedbackDate
        {
            get
            {
                Fill();
                return _feedbackdate;
            }
            set 
            {
                Fill();
                if (value != _feedbackdate)
                {
                    _feedbackdate = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? FutureContactDate
        {
            get
            {
                Fill();
                return _futurecontactdate;
            }
            set 
            {
                Fill();
                if (value != _futurecontactdate)
                {
                    _futurecontactdate = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINImportedPolicyDataID
        {
            get
            {
                Fill();
                return _fkinimportedpolicydataid;
            }
            set 
            {
                Fill();
                if (value != _fkinimportedpolicydataid)
                {
                    _fkinimportedpolicydataid = value;
                    _hasChanged = true;
                }
            }
        }

        public string Testing1
        {
            get
            {
                Fill();
                return _testing1;
            }
            set 
            {
                Fill();
                if (value != _testing1)
                {
                    _testing1 = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINLeadFeedbackID
        {
            get
            {
                Fill();
                return _fkinleadfeedbackid;
            }
            set 
            {
                Fill();
                if (value != _fkinleadfeedbackid)
                {
                    _fkinleadfeedbackid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINCancellationReasonID
        {
            get
            {
                Fill();
                return _fkincancellationreasonid;
            }
            set 
            {
                Fill();
                if (value != _fkincancellationreasonid)
                {
                    _fkincancellationreasonid = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsCopied
        {
            get
            {
                Fill();
                return _iscopied;
            }
            set 
            {
                Fill();
                if (value != _iscopied)
                {
                    _iscopied = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINConfirmationFeedbackID
        {
            get
            {
                Fill();
                return _fkinconfirmationfeedbackid;
            }
            set 
            {
                Fill();
                if (value != _fkinconfirmationfeedbackid)
                {
                    _fkinconfirmationfeedbackid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINParentBatchID
        {
            get
            {
                Fill();
                return _fkinparentbatchid;
            }
            set 
            {
                Fill();
                if (value != _fkinparentbatchid)
                {
                    _fkinparentbatchid = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? BonusLead
        {
            get
            {
                Fill();
                return _bonuslead;
            }
            set 
            {
                Fill();
                if (value != _bonuslead)
                {
                    _bonuslead = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKBatchCallRefUserID
        {
            get
            {
                Fill();
                return _fkbatchcallrefuserid;
            }
            set 
            {
                Fill();
                if (value != _fkbatchcallrefuserid)
                {
                    _fkbatchcallrefuserid = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsMining
        {
            get
            {
                Fill();
                return _ismining;
            }
            set 
            {
                Fill();
                if (value != _ismining)
                {
                    _ismining = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? ConfWorkDate
        {
            get
            {
                Fill();
                return _confworkdate;
            }
            set 
            {
                Fill();
                if (value != _confworkdate)
                {
                    _confworkdate = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsChecked
        {
            get
            {
                Fill();
                return _ischecked;
            }
            set 
            {
                Fill();
                if (value != _ischecked)
                {
                    _ischecked = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? CheckedDate
        {
            get
            {
                Fill();
                return _checkeddate;
            }
            set 
            {
                Fill();
                if (value != _checkeddate)
                {
                    _checkeddate = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? DateBatched
        {
            get
            {
                Fill();
                return _datebatched;
            }
            set 
            {
                Fill();
                if (value != _datebatched)
                {
                    _datebatched = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsCopyDuplicate
        {
            get
            {
                Fill();
                return _iscopyduplicate;
            }
            set 
            {
                Fill();
                if (value != _iscopyduplicate)
                {
                    _iscopyduplicate = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINDiaryReasonID
        {
            get
            {
                Fill();
                return _fkindiaryreasonid;
            }
            set 
            {
                Fill();
                if (value != _fkindiaryreasonid)
                {
                    _fkindiaryreasonid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINCarriedForwardReasonID
        {
            get
            {
                Fill();
                return _fkincarriedforwardreasonid;
            }
            set 
            {
                Fill();
                if (value != _fkincarriedforwardreasonid)
                {
                    _fkincarriedforwardreasonid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINCallMonitoringCarriedForwardReasonID
        {
            get
            {
                Fill();
                return _fkincallmonitoringcarriedforwardreasonid;
            }
            set 
            {
                Fill();
                if (value != _fkincallmonitoringcarriedforwardreasonid)
                {
                    _fkincallmonitoringcarriedforwardreasonid = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? PermissionQuestionAsked
        {
            get
            {
                Fill();
                return _permissionquestionasked;
            }
            set 
            {
                Fill();
                if (value != _permissionquestionasked)
                {
                    _permissionquestionasked = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINCallMonitoringCancellationReasonID
        {
            get
            {
                Fill();
                return _fkincallmonitoringcancellationreasonid;
            }
            set 
            {
                Fill();
                if (value != _fkincallmonitoringcancellationreasonid)
                {
                    _fkincallmonitoringcancellationreasonid = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsFutureAllocation
        {
            get
            {
                Fill();
                return _isfutureallocation;
            }
            set 
            {
                Fill();
                if (value != _isfutureallocation)
                {
                    _isfutureallocation = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? MoneyBackDate
        {
            get
            {
                Fill();
                return _moneybackdate;
            }
            set
            {
                Fill();
                if(value != _moneybackdate)
                {
                    _moneybackdate = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? ConversionMBDate
        {
            get
            {
                Fill();
                return _conversionmbdate;
            }
            set
            {
                Fill();
                if (value != _conversionmbdate)
                {
                    _conversionmbdate = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? ObtainedReferrals
        {
            get
            {
                Fill();
                return _obtainedreferrals;
            }
            set
            {
                Fill();
                if (value != _obtainedreferrals)
                {
                    _obtainedreferrals = value;
                    _hasChanged = true;
                }
            }
        }

        public string EmailStatus
        {
            get
            {
                Fill();
                return _emailstatus;
            }
            set
            {
                Fill();
                if (value != _emailstatus)
                {
                    _emailstatus = value;
                    _hasChanged = true;
                }
            }
        }

        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INImport object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INImportMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INImport object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImport object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INImportMapper.Save(this);
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
        /// Deletes an INImport object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImport object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INImportMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INImport.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inimport>");
            xml.Append("<fkuserid>" + FKUserID.ToString() + "</fkuserid>");
            xml.Append("<fkincampaignid>" + FKINCampaignID.ToString() + "</fkincampaignid>");
            xml.Append("<fkinbatchid>" + FKINBatchID.ToString() + "</fkinbatchid>");
            xml.Append("<fkinleadstatusid>" + FKINLeadStatusID.ToString() + "</fkinleadstatusid>");
            xml.Append("<fkindeclinereasonid>" + FKINDeclineReasonID.ToString() + "</fkindeclinereasonid>");
            xml.Append("<fkinpolicyid>" + FKINPolicyID.ToString() + "</fkinpolicyid>");
            xml.Append("<fkinleadid>" + FKINLeadID.ToString() + "</fkinleadid>");
            xml.Append("<refno>" + RefNo.ToString() + "</refno>");
            xml.Append("<referrorpolicyid>" + ReferrorPolicyID.ToString() + "</referrorpolicyid>");
            xml.Append("<fkinreferrortitleid>" + FKINReferrorTitleID.ToString() + "</fkinreferrortitleid>");
            xml.Append("<referror>" + Referror.ToString() + "</referror>");
            xml.Append("<fkinreferrorrelationshipid>" + FKINReferrorRelationshipID.ToString() + "</fkinreferrorrelationshipid>");
            xml.Append("<referrorcontact>" + ReferrorContact.ToString() + "</referrorcontact>");
            xml.Append("<gift>" + Gift.ToString() + "</gift>");
            xml.Append("<platinumcontactdate>" + PlatinumContactDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</platinumcontactdate>");
            xml.Append("<platinumcontacttime>" + PlatinumContactTime.ToString() + "</platinumcontacttime>");
            xml.Append("<canceroption>" + CancerOption.ToString() + "</canceroption>");
            xml.Append("<platinumage>" + PlatinumAge.ToString() + "</platinumage>");
            xml.Append("<allocationdate>" + AllocationDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</allocationdate>");
            xml.Append("<isprinted>" + IsPrinted.ToString() + "</isprinted>");
            xml.Append("<dateofsale>" + DateOfSale.Value.ToString("dd MMM yyyy HH:mm:ss") + "</dateofsale>");
            xml.Append("<bankcallref>" + BankCallRef.ToString() + "</bankcallref>");
            xml.Append("<fkbankcallrefuserid>" + FKBankCallRefUserID.ToString() + "</fkbankcallrefuserid>");
            xml.Append("<bankstationno>" + BankStationNo.ToString() + "</bankstationno>");
            xml.Append("<bankdate>" + BankDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</bankdate>");
            xml.Append("<banktime>" + BankTime.ToString() + "</banktime>");
            xml.Append("<fkbanktelnumbertypeid>" + FKBankTelNumberTypeID.ToString() + "</fkbanktelnumbertypeid>");
            xml.Append("<salecallref>" + SaleCallRef.ToString() + "</salecallref>");
            xml.Append("<fksalecallrefuserid>" + FKSaleCallRefUserID.ToString() + "</fksalecallrefuserid>");
            xml.Append("<salestationno>" + SaleStationNo.ToString() + "</salestationno>");
            xml.Append("<saledate>" + SaleDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</saledate>");
            xml.Append("<saletime>" + SaleTime.ToString() + "</saletime>");
            xml.Append("<fksaletelnumbertypeid>" + FKSaleTelNumberTypeID.ToString() + "</fksaletelnumbertypeid>");
            xml.Append("<confcallref>" + ConfCallRef.ToString() + "</confcallref>");
            xml.Append("<fkconfcallrefuserid>" + FKConfCallRefUserID.ToString() + "</fkconfcallrefuserid>");
            xml.Append("<confstationno>" + ConfStationNo.ToString() + "</confstationno>");
            xml.Append("<confdate>" + ConfDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</confdate>");
            xml.Append("<conftime>" + ConfTime.ToString() + "</conftime>");
            xml.Append("<fkconftelnumbertypeid>" + FKConfTelNumberTypeID.ToString() + "</fkconftelnumbertypeid>");
            xml.Append("<isconfirmed>" + IsConfirmed.ToString() + "</isconfirmed>");
            xml.Append("<notes>" + Notes.ToString() + "</notes>");
            xml.Append("<importdate>" + ImportDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</importdate>");
            xml.Append("<fkclosureid>" + FKClosureID.ToString() + "</fkclosureid>");
            xml.Append("<feedback>" + Feedback.ToString() + "</feedback>");
            xml.Append("<feedbackdate>" + FeedbackDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</feedbackdate>");
            xml.Append("<futurecontactdate>" + FutureContactDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</futurecontactdate>");
            xml.Append("<fkinimportedpolicydataid>" + FKINImportedPolicyDataID.ToString() + "</fkinimportedpolicydataid>");
            xml.Append("<testing1>" + Testing1.ToString() + "</testing1>");
            xml.Append("<fkinleadfeedbackid>" + FKINLeadFeedbackID.ToString() + "</fkinleadfeedbackid>");
            xml.Append("<fkincancellationreasonid>" + FKINCancellationReasonID.ToString() + "</fkincancellationreasonid>");
            xml.Append("<iscopied>" + IsCopied.ToString() + "</iscopied>");
            xml.Append("<fkinconfirmationfeedbackid>" + FKINConfirmationFeedbackID.ToString() + "</fkinconfirmationfeedbackid>");
            xml.Append("<fkinparentbatchid>" + FKINParentBatchID.ToString() + "</fkinparentbatchid>");
            xml.Append("<bonuslead>" + BonusLead.ToString() + "</bonuslead>");
            xml.Append("<fkbatchcallrefuserid>" + FKBatchCallRefUserID.ToString() + "</fkbatchcallrefuserid>");
            xml.Append("<ismining>" + IsMining.ToString() + "</ismining>");
            xml.Append("<confworkdate>" + ConfWorkDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</confworkdate>");
            xml.Append("<ischecked>" + IsChecked.ToString() + "</ischecked>");
            xml.Append("<checkeddate>" + CheckedDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</checkeddate>");
            xml.Append("<datebatched>" + DateBatched.Value.ToString("dd MMM yyyy HH:mm:ss") + "</datebatched>");
            xml.Append("<iscopyduplicate>" + IsCopyDuplicate.ToString() + "</iscopyduplicate>");
            xml.Append("<fkindiaryreasonid>" + FKINDiaryReasonID.ToString() + "</fkindiaryreasonid>");
            xml.Append("<fkincarriedforwardreasonid>" + FKINCarriedForwardReasonID.ToString() + "</fkincarriedforwardreasonid>");
            xml.Append("<fkincallmonitoringcarriedforwardreasonid>" + FKINCallMonitoringCarriedForwardReasonID.ToString() + "</fkincallmonitoringcarriedforwardreasonid>");
            xml.Append("<permissionquestionasked>" + PermissionQuestionAsked.ToString() + "</permissionquestionasked>");
            xml.Append("<fkincallmonitoringcancellationreasonid>" + FKINCallMonitoringCancellationReasonID.ToString() + "</fkincallmonitoringcancellationreasonid>");
            xml.Append("<isfutureallocation>" + IsFutureAllocation.ToString() + "</isfutureallocation>");
            xml.Append("<moneybackdate>" + MoneyBackDate.ToString() + "</moneybackdate>");
            xml.Append("<conversionmbdate>" + ConversionMBDate.ToString() + "</conversionmbdate>");
            xml.Append("<obtainedreferrals>" + ObtainedReferrals.ToString() + "</obtainedreferrals>");
            xml.Append("<emailstatus>" + EmailStatus.ToString() + "</emailstatus>");
            xml.Append("</inimport>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INImport object from a list of parameters.
        /// </summary>
        /// <param name="fkuserid"></param>
        /// <param name="fkincampaignid"></param>
        /// <param name="fkinbatchid"></param>
        /// <param name="fkinleadstatusid"></param>
        /// <param name="fkindeclinereasonid"></param>
        /// <param name="fkinpolicyid"></param>
        /// <param name="fkinleadid"></param>
        /// <param name="refno"></param>
        /// <param name="referrorpolicyid"></param>
        /// <param name="fkinreferrortitleid"></param>
        /// <param name="referror"></param>
        /// <param name="fkinreferrorrelationshipid"></param>
        /// <param name="referrorcontact"></param>
        /// <param name="gift"></param>
        /// <param name="platinumcontactdate"></param>
        /// <param name="platinumcontacttime"></param>
        /// <param name="canceroption"></param>
        /// <param name="platinumage"></param>
        /// <param name="allocationdate"></param>
        /// <param name="isprinted"></param>
        /// <param name="dateofsale"></param>
        /// <param name="bankcallref"></param>
        /// <param name="fkbankcallrefuserid"></param>
        /// <param name="bankstationno"></param>
        /// <param name="bankdate"></param>
        /// <param name="banktime"></param>
        /// <param name="fkbanktelnumbertypeid"></param>
        /// <param name="salecallref"></param>
        /// <param name="fksalecallrefuserid"></param>
        /// <param name="salestationno"></param>
        /// <param name="saledate"></param>
        /// <param name="saletime"></param>
        /// <param name="fksaletelnumbertypeid"></param>
        /// <param name="confcallref"></param>
        /// <param name="fkconfcallrefuserid"></param>
        /// <param name="confstationno"></param>
        /// <param name="confdate"></param>
        /// <param name="conftime"></param>
        /// <param name="fkconftelnumbertypeid"></param>
        /// <param name="isconfirmed"></param>
        /// <param name="notes"></param>
        /// <param name="importdate"></param>
        /// <param name="fkclosureid"></param>
        /// <param name="feedback"></param>
        /// <param name="feedbackdate"></param>
        /// <param name="futurecontactdate"></param>
        /// <param name="fkinimportedpolicydataid"></param>
        /// <param name="testing1"></param>
        /// <param name="fkinleadfeedbackid"></param>
        /// <param name="fkincancellationreasonid"></param>
        /// <param name="iscopied"></param>
        /// <param name="fkinconfirmationfeedbackid"></param>
        /// <param name="fkinparentbatchid"></param>
        /// <param name="bonuslead"></param>
        /// <param name="fkbatchcallrefuserid"></param>
        /// <param name="ismining"></param>
        /// <param name="confworkdate"></param>
        /// <param name="ischecked"></param>
        /// <param name="checkeddate"></param>
        /// <param name="datebatched"></param>
        /// <param name="iscopyduplicate"></param>
        /// <param name="fkindiaryreasonid"></param>
        /// <param name="fkincarriedforwardreasonid"></param>
        /// <param name="fkincallmonitoringcarriedforwardreasonid"></param>
        /// <param name="permissionquestionasked"></param>
        /// <param name="fkincallmonitoringcancellationreasonid"></param>
        /// <param name="isfutureallocation"></param>
        /// <param name="obtainedreferrals"></param>
        /// <param name="emailstatus"></param>

        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkuserid, long? fkincampaignid, long? fkinbatchid, long? fkinleadstatusid, long? fkindeclinereasonid, long? fkinpolicyid, long? fkinleadid, string refno, string referrorpolicyid, long? fkinreferrortitleid, string referror, long? fkinreferrorrelationshipid, string referrorcontact, string gift, DateTime? platinumcontactdate, TimeSpan? platinumcontacttime, string canceroption, short? platinumage, DateTime? allocationdate, Byte? isprinted, DateTime? dateofsale, string bankcallref, long? fkbankcallrefuserid, string bankstationno, DateTime? bankdate, TimeSpan? banktime, long? fkbanktelnumbertypeid, string salecallref, long? fksalecallrefuserid, string salestationno, DateTime? saledate, TimeSpan? saletime, long? fksaletelnumbertypeid, string confcallref, long? fkconfcallrefuserid, string confstationno, DateTime? confdate, TimeSpan? conftime, long? fkconftelnumbertypeid, bool? isconfirmed, string notes, DateTime? importdate, long? fkclosureid, string feedback, DateTime? feedbackdate, DateTime? futurecontactdate, long? fkinimportedpolicydataid, string testing1, long? fkinleadfeedbackid, long? fkincancellationreasonid, bool? iscopied, long? fkinconfirmationfeedbackid, long? fkinparentbatchid, bool? bonuslead, long? fkbatchcallrefuserid, bool? ismining, DateTime? confworkdate, bool? ischecked, DateTime? checkeddate, DateTime? datebatched, bool? iscopyduplicate, long? fkindiaryreasonid, long? fkincarriedforwardreasonid, long? fkincallmonitoringcarriedforwardreasonid, bool? permissionquestionasked, long? fkincallmonitoringcancellationreasonid, bool? isfutureallocation, DateTime? moneybackdate, DateTime? conversionmbdate, bool? obtainedreferrals, string emailstatus)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKUserID = fkuserid;
                this.FKINCampaignID = fkincampaignid;
                this.FKINBatchID = fkinbatchid;
                this.FKINLeadStatusID = fkinleadstatusid;
                this.FKINDeclineReasonID = fkindeclinereasonid;
                this.FKINPolicyID = fkinpolicyid;
                this.FKINLeadID = fkinleadid;
                this.RefNo = refno;
                this.ReferrorPolicyID = referrorpolicyid;
                this.FKINReferrorTitleID = fkinreferrortitleid;
                this.Referror = referror;
                this.FKINReferrorRelationshipID = fkinreferrorrelationshipid;
                this.ReferrorContact = referrorcontact;
                this.Gift = gift;
                this.PlatinumContactDate = platinumcontactdate;
                this.PlatinumContactTime = platinumcontacttime;
                this.CancerOption = canceroption;
                this.PlatinumAge = platinumage;
                this.AllocationDate = allocationdate;
                this.IsPrinted = isprinted;
                this.DateOfSale = dateofsale;
                this.BankCallRef = bankcallref;
                this.FKBankCallRefUserID = fkbankcallrefuserid;
                this.BankStationNo = bankstationno;
                this.BankDate = bankdate;
                this.BankTime = banktime;
                this.FKBankTelNumberTypeID = fkbanktelnumbertypeid;
                this.SaleCallRef = salecallref;
                this.FKSaleCallRefUserID = fksalecallrefuserid;
                this.SaleStationNo = salestationno;
                this.SaleDate = saledate;
                this.SaleTime = saletime;
                this.FKSaleTelNumberTypeID = fksaletelnumbertypeid;
                this.ConfCallRef = confcallref;
                this.FKConfCallRefUserID = fkconfcallrefuserid;
                this.ConfStationNo = confstationno;
                this.ConfDate = confdate;
                this.ConfTime = conftime;
                this.FKConfTelNumberTypeID = fkconftelnumbertypeid;
                this.IsConfirmed = isconfirmed;
                this.Notes = notes;
                this.ImportDate = importdate;
                this.FKClosureID = fkclosureid;
                this.Feedback = feedback;
                this.FeedbackDate = feedbackdate;
                this.FutureContactDate = futurecontactdate;
                this.FKINImportedPolicyDataID = fkinimportedpolicydataid;
                this.Testing1 = testing1;
                this.FKINLeadFeedbackID = fkinleadfeedbackid;
                this.FKINCancellationReasonID = fkincancellationreasonid;
                this.IsCopied = iscopied;
                this.FKINConfirmationFeedbackID = fkinconfirmationfeedbackid;
                this.FKINParentBatchID = fkinparentbatchid;
                this.BonusLead = bonuslead;
                this.FKBatchCallRefUserID = fkbatchcallrefuserid;
                this.IsMining = ismining;
                this.ConfWorkDate = confworkdate;
                this.IsChecked = ischecked;
                this.CheckedDate = checkeddate;
                this.DateBatched = datebatched;
                this.IsCopyDuplicate = iscopyduplicate;
                this.FKINDiaryReasonID = fkindiaryreasonid;
                this.FKINCarriedForwardReasonID = fkincarriedforwardreasonid;
                this.FKINCallMonitoringCarriedForwardReasonID = fkincallmonitoringcarriedforwardreasonid;
                this.PermissionQuestionAsked = permissionquestionasked;
                this.FKINCallMonitoringCancellationReasonID = fkincallmonitoringcancellationreasonid;
                this.IsFutureAllocation = isfutureallocation;
                this.MoneyBackDate = moneybackdate;
                this.ConversionMBDate = conversionmbdate;
                this.ObtainedReferrals = obtainedreferrals;
                this.EmailStatus = emailstatus;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INImport's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImport history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INImport object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImport object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportMapper.UnDelete(this);
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
    /// A collection of the INImport object.
    /// </summary>
    public partial class INImportCollection : ObjectCollection<INImport>
    { 
    }
    #endregion
}
