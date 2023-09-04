using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Embriant.Framework.Configuration;
using UDM.Insurance.Business;
using UDM.WPF.Library;
using UDM.WPF.Models;
using UDM.Insurance.Interface.MercantileService;
using Prism.Commands;
using System.Collections.Generic;

namespace UDM.Insurance.Interface.Data
{

    public class LeadApplicationData : ObservableObject
    {

        #region Constants

        public const int MaxLA = 2;
        public const int MaxChildren = 4;
        public const int MaxBeneficiaries = 6;

        public const int MaxNextOfKin = 1;

        //Mercantile variables
        private const string mercUsername = "SOAP";
        private const string mercInstCode = "UDM1";
        private const string mercPassword = "Password1";

        #endregion



        #region Members

        public static DataTable dtTitles;
        public static DataTable dtCampaignGroupSet;
        public static DataTable dtCampaignTypeSet;
        public static long? importID;


        public DateTime TodayLess1Day { get; set; }
        public DateTime TodayLess7Days { get; set; }

        public class ImportedCover
        {
            public string Name { get; set; }
            public decimal? Cover { get; set; }
            public decimal? Premium { get; set; }
        }

        public class Person : ObservableObject
        {
            private long? _titleID;
            public long? TitleID
            {
                get
                {
                    UpdateTitle(_titleID);
                    UpdateFullName();

                    return _titleID;
                }
                set
                {
                    SetProperty(ref _titleID, value, () => TitleID);
                    if (ToString().Contains("Beneficiary") || ToString().Contains("LA")) { CopyToBenefCommand.RaiseCanExecuteChanged(); }
                }
            }

            private void UpdateTitle(long? ID)
            {
                if (ID != null)
                {
                    DataRow dr = dtTitles.Select("[ID] = " + ID).FirstOrDefault();
                    if (dr != null)
                    {
                        Title = dr["Description"] as string;
                    }
                }
            }
            private string _title;
            public string Title
            {
                get { return _title; }
                set { SetProperty(ref _title, value, () => Title); }
            }

            private long? _genderID;
            public long? GenderID
            {
                get { return _genderID; }
                set
                {
                    SetProperty(ref _genderID, value, () => GenderID);
                    if (ToString().Contains("Beneficiary") || ToString().Contains("LA")) { CopyToBenefCommand.RaiseCanExecuteChanged(); }
                }
            }

            private string _initials;
            public string Initials
            {
                get { return _initials; }
                set { SetProperty(ref _initials, value, () => Initials); }
            }

            private string _name;
            public string Name
            {
                get
                {
                    UpdateFullName();

                    return _name;
                }
                set
                {
                    SetProperty(ref _name, value, () => Name);
                    if (ToString().Contains("Beneficiary") || ToString().Contains("LA")) { CopyToBenefCommand.RaiseCanExecuteChanged(); }
                }
            }

            private string _surname;
            public string Surname
            {
                get
                {
                    UpdateFullName();

                    return _surname;
                }
                set
                {
                    SetProperty(ref _surname, value, () => Surname);
                    if (ToString().Contains("Beneficiary") || ToString().Contains("LA")) { CopyToBenefCommand.RaiseCanExecuteChanged(); }
                }
            }

            private string _idNumber;
            public string IDNumber
            {
                get { return _idNumber; }
                set { SetProperty(ref _idNumber, value, () => IDNumber); }
            }

            private string _passportNumber;
            public string PassportNumber
            {
                get { return _passportNumber; }
                set { SetProperty(ref _passportNumber, value, () => PassportNumber); }
            }

            private DateTime? _dateOfBirth;
            public DateTime? DateOfBirth
            {
                get
                {
                    DateTime today = DateTime.Today;

                    if (_dateOfBirth != null)
                    {
                        DateTime bday = _dateOfBirth.Value;

                        Age = (short?)(DateTime.Today.Year - bday.Year);
                        if (bday > today.AddYears((int)-Age)) Age--;
                    }
                    else
                    {
                        Age = null;
                    }

                    return _dateOfBirth;
                }
                set
                {
                    SetProperty(ref _dateOfBirth, value, () => DateOfBirth);
                    if (ToString().Contains("Beneficiary") || ToString().Contains("LA")) { CopyToBenefCommand.RaiseCanExecuteChanged(); }
                }
            }

            private long? _relationshipID;
            public long? RelationshipID
            {
                get { return _relationshipID; }
                set
                {
                    SetProperty(ref _relationshipID, value, () => RelationshipID);
                    if (ToString().Contains("Beneficiary") || ToString().Contains("LA")) { CopyToBenefCommand.RaiseCanExecuteChanged(); }
                }
            }

            private short? _age;
            public short? Age
            {
                get { return _age; } //follows date of birth
                set { SetProperty(ref _age, value, () => Age); }
            }

            private void UpdateFullName()
            {
                string strTitle = string.Empty;
                string strSurname = string.Empty;
                string strFirstName = string.Empty;

                if (_title != null)
                {
                    strTitle = _title.Trim();
                }
                if (_name != null)
                {
                    strFirstName = _name.Split(' ')[0].Trim();
                }
                if (_surname != null)
                {
                    strSurname = _surname.Trim();
                }

                FullName = (strTitle + " " + strFirstName + " " + strSurname).Trim();
            }
            private string _fullName;
            public string FullName
            {
                get { return _fullName; }
                set { SetProperty(ref _fullName, value, () => FullName); }
            }

            private string _telContact;
            public string TelContact
            {
                get { return _telContact; }
                set
                {
                    SetProperty(ref _telContact, value, () => TelContact);
                    if (ToString().Contains("Beneficiary") || ToString().Contains("LA")) { CopyToBenefCommand.RaiseCanExecuteChanged(); }
                }
            }
        }

        public class PersonExt : Person
        {
            private string _telWork;
            public string TelWork
            {
                get { return _telWork; }
                set { SetProperty(ref _telWork, value, () => TelWork); }
            }

            private string _telHome;
            public string TelHome
            {
                get { return _telHome; }
                set { SetProperty(ref _telHome, value, () => TelHome); }
            }

            private string _telCell;
            public string TelCell
            {
                get { return _telCell; }
                set { SetProperty(ref _telCell, value, () => TelCell); }
            }

            private string _telOther;
            public string TelOther
            {
                get { return _telOther; }
                set { SetProperty(ref _telOther, value, () => TelOther); }
            }

            private string _address;
            public string Address
            {
                get { return _address; }
                set { SetProperty(ref _address, value, () => Address); }
            }

            private string _address1;
            public string Address1
            {
                get { return _address1; }
                set { SetProperty(ref _address1, value, () => Address1); }
            }

            private string _address2;
            public string Address2
            {
                get { return _address2; }
                set { SetProperty(ref _address2, value, () => Address2); }
            }

            private string _address3;
            public string Address3
            {
                get { return _address3; }
                set { SetProperty(ref _address3, value, () => Address3); }
            }

            private string _address4;
            public string Address4
            {
                get { return _address4; }
                set { SetProperty(ref _address4, value, () => Address4); }
            }

            private string _address5;
            public string Address5
            {
                get { return _address5; }
                set { SetProperty(ref _address5, value, () => Address5); }
            }

            private string _postalCode;
            public string PostalCode
            {
                get { return _postalCode; }
                set { SetProperty(ref _postalCode, value, () => PostalCode); }
            }

            private string _email;
            public string Email
            {
                get { return _email; }
                set { SetProperty(ref _email, value, () => Email); }
            }

            private string _occupation;
            public string Occupation
            {
                get { return _occupation; }
                set { SetProperty(ref _occupation, value, () => Occupation); }
            }
        }

        public class Lead : PersonExt
        {
            public long? LeadID { get; set; }

            private long? _referrorTitleID;
            public long? ReferrorTitleID
            {
                get
                {
                    UpdateReferrorTitle(_referrorTitleID);
                    UpdateReferrorDetails();

                    return _referrorTitleID;
                }
                set { SetProperty(ref _referrorTitleID, value, () => ReferrorTitleID); }
            }

            private void UpdateReferrorTitle(long? ID)
            {
                if (ID != null)
                {
                    DataRow dr = dtTitles.Select("[ID] = " + ID).FirstOrDefault();
                    if (dr != null)
                    {
                        ReferrorTitle = dr["Description"] as string;
                    }
                }
            }
            private string _referrorTitle;
            public string ReferrorTitle
            {
                get { return _referrorTitle; }
                set { SetProperty(ref _referrorTitle, value, () => ReferrorTitle); }
            }

            private string _referrorName;
            public string ReferrorName
            {
                get
                {
                    UpdateReferrorDetails();

                    return _referrorName;
                }
                set { SetProperty(ref _referrorName, value, () => ReferrorName); }
            }

            private long? _referrorRelationshipID;
            public long? ReferrorRelationshipID
            {
                get
                {
                    UpdateReferrorDetails();

                    return _referrorRelationshipID;
                }
                set { SetProperty(ref _referrorRelationshipID, value, () => ReferrorRelationshipID); }
            }

            private string _referrorContact;
            public string ReferrorContact
            {
                get
                {
                    return _referrorContact;
                }
                set { SetProperty(ref _referrorContact, value, () => ReferrorContact); }
            }

            private string _referrorType;
            public string ReferrorType
            {
                get { return _referrorType; }
                set { SetProperty(ref _referrorType, value, () => ReferrorType); }
            }

            private void UpdateReferrorDetails()
            {
                string strTitle = string.Empty;
                //string strSurname = string.Empty;
                string strName = string.Empty;

                if (_referrorTitle != null)
                {
                    strTitle = _referrorTitle.Trim();
                }
                if (_referrorName != null)
                {
                    strName = _referrorName.Trim();
                }
                //if (_surname != null)
                //{
                //    strSurname = _surname.Trim();
                //}

                ReferrorDetails = (strTitle + " " + strName).Trim(); // + " " + strSurname.Trim()
            }
            private string _referrorDetails;
            public string ReferrorDetails
            {
                get { return _referrorDetails; }
                set { SetProperty(ref _referrorDetails, value, () => ReferrorDetails); }
            }

            private bool? _isConfirmed;
            public bool? IsConfirmed
            {
                get
                {
                    return _isConfirmed;
                }
                set
                {
                    SetProperty(ref _isConfirmed, value, () => IsConfirmed);
                }
            }
            private DateTime? _stampDate;
            public DateTime? StampDate
            {
                get
                {
                    return _stampDate;
                }
                set
                {
                    SetProperty(ref _stampDate, value, () => StampDate);
                }
            }

            private bool _isLessThanHourOld;
            public bool IsLessThanHourOld
            {
                get
                {
                    return _isLessThanHourOld;
                }
                set
                {
                    SetProperty(ref _isLessThanHourOld, value, () => IsLessThanHourOld);
                }
            }
            private bool _isSaveEnabled;
            public bool IsSaveEnabled
            {
                get { return _isSaveEnabled; }
                set
                {
                    if (_isSaveEnabled != value)
                    {
                        _isSaveEnabled = value;
                        OnPropertyChanged(nameof(IsSaveEnabled));
                    }
                }
            }

            private string _toolTip;
            public string ToolTip
            {
                get { return _toolTip; }
                set
                {
                    if (_toolTip != value)
                    {
                        _toolTip = value;
                        OnPropertyChanged(nameof(ToolTip));
                    }
                }
            }
            private TimeSpan _timeLeft;
            public TimeSpan TimeLeft
            {
                get { return _timeLeft; }
                set
                {
                    if (_timeLeft != value)
                    {
                        _timeLeft = value;
                        OnPropertyChanged(nameof(TimeLeft));
                    }
                }
            }
            private string _timeLeftString;
            public string TimeLeftString
            {
                get { return _timeLeftString; }
                set
                {
                    if (_timeLeftString != value)
                    {
                        _timeLeftString = value;
                        OnPropertyChanged(nameof(TimeLeftString));
                    }
                }
            }
        }

        public class LeadHistory : Lead
        {

        }

        public class LA : Person
        {
            public long? LifeAssuredID { get; set; }
            public long? PolicyLifeAssuredID { get; set; }
        }
        public class LAHistory : LA
        {

        }

        public class Child : Person
        {
            public long? ChildID { get; set; }
            public long? PolicyChildID { get; set; }
        }
        public class ChildHistory : Child
        {

        }

        public class Beneficiary : Person
        {
            public long? BeneficiaryID { get; set; }
            public long? PolicyBeneficiaryID { get; set; }

            private Double? _percentage;
            public Double? Percentage
            {
                get { return _percentage; }
                set
                {
                    SetProperty(ref _percentage, value, () => Percentage);
                    if (ToString().Contains("Beneficiary")) { CopyToBenefCommand.RaiseCanExecuteChanged(); }
                }
            }

            private string _notes;
            public string Notes
            {
                get { return _notes; }
                set
                {
                    SetProperty(ref _notes, value, () => Notes);
                    if (ToString().Contains("Beneficiary")) { CopyToBenefCommand.RaiseCanExecuteChanged(); }
                }
            }
        }
        public class BeneficiaryHistory : Beneficiary
        {

        }

        public class NextOfKin : Person
        {
            public long? NextOfKinID { get; set; }

        }
        public class NextOfKinHistory : NextOfKin
        {

        }

        public class Application : ObservableObject
        {
            private long? _importID;
            public long? ImportID
            {
                get { return _importID; }
                set { SetProperty(ref _importID, value, () => ImportID); }
            }

            private long? _importIDNextLead;
            public long? ImportIDNextLead
            {
                get { return _importIDNextLead; }
                set { SetProperty(ref _importIDNextLead, value, () => ImportIDNextLead); }
            }

            private string _refNo;
            public string RefNo
            {
                get { return _refNo; }
                set { SetProperty(ref _refNo, value, () => RefNo); }
            }

            private long? _agentID;
            public long? AgentID
            {
                get { return _agentID; }
                set { SetProperty(ref _agentID, value, () => AgentID); }
            }

            private string _loadedRefNo;
            public string LoadedRefNo
            {
                get { return _loadedRefNo; }
                set { SetProperty(ref _loadedRefNo, value, () => LoadedRefNo); }
            }

            private bool _isLeadUpgrade;
            public bool IsLeadUpgrade
            {
                get { return _isLeadUpgrade; }
                set { SetProperty(ref _isLeadUpgrade, value, () => IsLeadUpgrade); }
            }

            private bool _isLeadLoaded;
            public bool IsLeadLoaded
            {
                get { return _isLeadLoaded; }
                set { SetProperty(ref _isLeadLoaded, value, () => IsLeadLoaded); }
            }

            private bool _isLeadSaving;
            public bool IsLeadSaving
            {
                get { return _isLeadSaving; }
                set { SetProperty(ref _isLeadSaving, value, () => IsLeadSaving); }
            }

            private bool _isConfirmed;
            public bool IsConfirmed
            {
                get { return _isConfirmed; }
                set { SetProperty(ref _isConfirmed, value, () => IsConfirmed); }
            }

            private bool _isMining;
            public bool IsMining
            {
                get { return _isMining; }
                set { SetProperty(ref _isMining, value, () => IsMining); }
            }

            private bool _isCopied;
            public bool IsCopied
            {
                get { return _isCopied; }
                set { SetProperty(ref _isCopied, value, () => IsCopied); }
            }

            private bool _isLocked;
            public bool IsLocked
            {
                get { return _isLocked; }
                set { SetProperty(ref _isLocked, value, () => IsLocked); }
            }

            private bool _canClientBeContacted;
            public bool CanClientBeContacted
            {
                get { return _canClientBeContacted; }
                set { SetProperty(ref _canClientBeContacted, value, () => CanClientBeContacted); }
            }

            private long? _campaignID;
            public long? CampaignID
            {
                get { return _campaignID; }
                set { SetProperty(ref _campaignID, value, () => CampaignID); }
            }

            private string _campaignCode;
            public string CampaignCode
            {
                get { return _campaignCode; }
                set { SetProperty(ref _campaignCode, value, () => CampaignCode); }
            }

            private lkpINCampaignGroup? _campaignGroup;
            public lkpINCampaignGroup? CampaignGroup
            {
                get
                {
                    if (_campaignGroup != null)
                    {
                        DataRow dr = dtCampaignGroupSet.Select("[FKlkpINCampaignGroup] = " + (long?)_campaignGroup).FirstOrDefault();
                        if (dr != null)
                        {
                            CampaignGroupType = (lkpINCampaignGroupType?)Convert.ToInt64(dr["FKlkpINCampaignGroupType"]);
                        }
                    }

                    return _campaignGroup;
                }
                set { SetProperty(ref _campaignGroup, value, () => CampaignGroup); }
            }

            private lkpINCampaignType? _campaignType;
            public lkpINCampaignType? CampaignType
            {
                get
                {
                    if (_campaignType != null)
                    {
                        DataRow dr = dtCampaignTypeSet.Select("[FKCampaignTypeID] = " + (long?)_campaignType).FirstOrDefault();
                        if (dr != null)
                        {
                            CampaignTypeGroup = (lkpINCampaignTypeGroup?)Convert.ToInt64(dr["FKCampaignTypeGroupID"]);
                        }
                    }

                    return _campaignType;
                }
                set { SetProperty(ref _campaignType, value, () => CampaignType); }
            }

            private lkpINCampaignGroupType? _campaignGroupType;
            public lkpINCampaignGroupType? CampaignGroupType //Follows CampaignGroup
            {
                get { return _campaignGroupType; }
                set { SetProperty(ref _campaignGroupType, value, () => CampaignGroupType); }
            }

            private lkpINCampaignTypeGroup? _campaignTypeGroup;
            public lkpINCampaignTypeGroup? CampaignTypeGroup //Follows CampaignType
            {
                get { return _campaignTypeGroup; }
                set { SetProperty(ref _campaignTypeGroup, value, () => CampaignTypeGroup); }
            }

            private string _platinumBatchCode;
            public string PlatinumBatchCode
            {
                get { return _platinumBatchCode; }
                set { SetProperty(ref _platinumBatchCode, value, () => PlatinumBatchCode); }
            }

            private string _udmBatchCode;
            public string UDMBatchCode
            {
                get { return _udmBatchCode; }
                set { SetProperty(ref _udmBatchCode, value, () => UDMBatchCode); }
            }

            private long? _declineReasonID;
            public long? DeclineReasonID
            {
                get { return _declineReasonID; }
                set { SetProperty(ref _declineReasonID, value, () => DeclineReasonID); }
            }

            private long? _cancellationReasonID;
            public long? CancellationReasonID
            {
                get { return _cancellationReasonID; }
                set { SetProperty(ref _cancellationReasonID, value, () => CancellationReasonID); }
            }

            private long? _carriedForwardReasonID;
            public long? CarriedForwardReasonID
            {
                get { return _carriedForwardReasonID; }
                set { SetProperty(ref _carriedForwardReasonID, value, () => CarriedForwardReasonID); }
            }

            private long? _fkINCallMonitoringCarriedForwardReasonID;
            public long? FKINCallMonitoringCarriedForwardReasonID
            {
                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/222494520/comments
                get { return _fkINCallMonitoringCarriedForwardReasonID; }
                set { SetProperty(ref _fkINCallMonitoringCarriedForwardReasonID, value, () => FKINCallMonitoringCarriedForwardReasonID); }
            }

            private long? _fkINCallMonitoringCancellationReasonID;
            public long? FKINCallMonitoringCancellationReasonID
            {
                get { return _fkINCallMonitoringCancellationReasonID; }
                set { SetProperty(ref _fkINCallMonitoringCancellationReasonID, value, () => FKINCallMonitoringCancellationReasonID); }
            }

            private long? _diaryReasonID;
            public long? DiaryReasonID
            {
                get { return _diaryReasonID; }
                set { SetProperty(ref _diaryReasonID, value, () => DiaryReasonID); }
            }

            private long? _leadStatus;
            public long? LeadStatus
            {
                get
                {
                    lkpLeadStatus = (lkpINLeadStatus?)_leadStatus;
                    return _leadStatus;
                }
                set { SetProperty(ref _leadStatus, value, () => LeadStatus); }
            }

            private lkpINLeadStatus? _lkpLeadStatus; //Follows LeadStatus
            public lkpINLeadStatus? lkpLeadStatus
            {
                get { return _lkpLeadStatus; }
                set { SetProperty(ref _lkpLeadStatus, value, () => lkpLeadStatus); }
            }

            private long? _loadedLeadStatus;
            public long? LoadedLeadStatus
            {
                get { return _loadedLeadStatus; }
                set { SetProperty(ref _loadedLeadStatus, value, () => LoadedLeadStatus); }
            }

            private bool _diaryStatusHandled;
            public bool DiaryStatusHandled
            {
                get { return _diaryStatusHandled; }
                set { SetProperty(ref _diaryStatusHandled, value, () => DiaryStatusHandled); }
            }

            private DateTime? _dateOfSale;
            public DateTime? DateOfSale
            {
                get { return _dateOfSale; }
                set { SetProperty(ref _dateOfSale, value, () => DateOfSale); }
            }

            private DateTime? _loadedDateOfSale;
            public DateTime? LoadedDateOfSale
            {
                get { return _loadedDateOfSale; }
                set { SetProperty(ref _loadedDateOfSale, value, () => LoadedDateOfSale); }
            }

            private DateTime? _loadedAllocationDate;
            public DateTime? LoadedAllocationDate
            {
                get { return _loadedAllocationDate; }
                set { SetProperty(ref _loadedAllocationDate, value, () => LoadedAllocationDate); OnPropertyChanged("SMSAvailableDate"); }
            }

            private DateTime _smsAvailableDate;
            public DateTime SMSAvailableDate
            {
                get { return _smsAvailableDate = (LoadedAllocationDate == null ? DateTime.MinValue : Convert.ToDateTime(LoadedAllocationDate.ToString())).AddDays(28.00d); }
            }



            private DateTime? _futureContactDate;
            public DateTime? FutureContactDate
            {
                get { return _futureContactDate; }
                set { SetProperty(ref _futureContactDate, value, () => FutureContactDate); }
            }

            //// See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/209101666/comments

            //private bool _hasNoteBeenDisplayed;
            //public bool HasNoteBeenDisplayed
            //{
            //    get { return _hasNoteBeenDisplayed; }
            //    set { SetProperty(ref _hasNoteBeenDisplayed, value, () => HasNoteBeenDisplayed); }
            //}

            //private bool _hasNotes;
            //public bool HasNotes
            //{
            //    get { return _hasNotes; }
            //    set { SetProperty(ref _hasNotes, value, () => HasNotes); }
            //}

            //private Double _beneficiariesTotalPercentage;
            //public Double BeneficiariesTotalPercentage
            //{
            //    get { return _beneficiariesTotalPercentage; }
            //    set { SetProperty(ref _beneficiariesTotalPercentage, value, () => BeneficiariesTotalPercentage); }
            //}

            private System.Windows.Visibility _notesFeatureVisibility;
            public System.Windows.Visibility NotesFeatureVisibility
            {
                get { return _notesFeatureVisibility; }
                set { SetProperty(ref _notesFeatureVisibility, value, () => NotesFeatureVisibility); }
            }

            private bool _canManageCallMonitoringDetails;

            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/220876019/comments
            public bool CanManageCallMonitoringDetails
            {
                get { return _canManageCallMonitoringDetails; }
                set { SetProperty(ref _canManageCallMonitoringDetails, value, () => CanManageCallMonitoringDetails); }
            }

            private bool _canManageQADetails;
            public bool CanManageQADetails
            {
                get { return _canManageQADetails; }
                set { SetProperty(ref _canManageQADetails, value, () => CanManageQADetails); }
            }

            //
            private bool _canChangeStatus;
            public bool CanChangeStatus
            {
                get { return _canChangeStatus; }
                set { SetProperty(ref _canChangeStatus, value, () => CanChangeStatus); }
            }

            private bool _isOverTarget;
            public bool IsOverTarget
            {
                get { return _isOverTarget; }
                set { SetProperty(ref _isOverTarget, value, () => IsOverTarget); }
            }

            private lkpSolutionConfiguration? _solutionConfiguration;
            public lkpSolutionConfiguration? SolutionConfiguration
            {
                get { return _solutionConfiguration; }
                set { SetProperty(ref _solutionConfiguration, value, () => SolutionConfiguration); }
            }

            private int? _selectedBeneficiaryIndex;
            public int? SelectedBeneficiaryIndex
            {
                get { return _selectedBeneficiaryIndex; }
                set
                {
                    SetProperty(ref _selectedBeneficiaryIndex, value, () => SelectedBeneficiaryIndex);
                    CopyToBenefCommand.RaiseCanExecuteChanged();
                }
            }

            //private string _selectedReferrorNOK = "Referror";
            //public string SelectedReferrorNOK
            //{
            //    get { return _selectedReferrorNOK; }
            //    set
            //    {
            //        SetProperty(ref _selectedReferrorNOK, value, () => SelectedReferrorNOK);
            //        ReferrorNOKCommand.RaiseCanExecuteChanged();
            //    }
            //}

            private bool _eMailRequested;
            public bool EMailRequested
            {
                get { return _eMailRequested; }
                set { SetProperty(ref _eMailRequested, value, () => EMailRequested); }
            }

            private bool _customerCareRequested;
            public bool CustomerCareRequested
            {
                get { return _customerCareRequested; }
                set { SetProperty(ref _customerCareRequested, value, () => CustomerCareRequested); }
            }

            private bool _isGoldenLead = false;
            public bool IsGoldenLead
            {
                get { return _isGoldenLead; }
                set { SetProperty(ref _isGoldenLead, value, () => IsGoldenLead); }
            }
        }
        public class ApplicationHistory : Application
        {

        }

        public class Application2 : LAInsure.Application
        {

        }

        public class Policy : ObservableObject
        {
            public long? PolicyID { get; set; }

            private short? _leadAge;
            public short? LeadAge
            {
                get { return _leadAge; }
                set { SetProperty(ref _leadAge, value, () => LeadAge); }
            }

            private DateTime? _commenceDate;
            public DateTime? CommenceDate
            {
                get { return _commenceDate; }
                set { SetProperty(ref _commenceDate, value, () => CommenceDate); }
            }

            private short? _cashBackAge;
            public short? CashBackAge
            {
                get { return _cashBackAge; }
                set { SetProperty(ref _cashBackAge, value, () => CashBackAge); }
            }

            private short? _cashBackAge2;
            public short? CashBackAge2
            {
                get { return _cashBackAge2; }
                set { SetProperty(ref _cashBackAge2, value, () => CashBackAge2); }
            }

            private string _policyNumber;
            public string PolicyNumber
            {
                get { return _policyNumber; }
                set { SetProperty(ref _policyNumber, value, () => PolicyNumber); }
            }

            private string _platinumPlan;
            public string PlatinumPlan
            {
                get { return _platinumPlan; }
                set { SetProperty(ref _platinumPlan, value, () => PlatinumPlan); }
            }

            private long? _campaignTypeID;
            public long? CampaignTypeID
            {
                get { return _campaignTypeID; }
                set { SetProperty(ref _campaignTypeID, value, () => CampaignTypeID); }
            }

            private long? _campaignGroupID;
            public long? CampaignGroupID
            {
                get { return _campaignGroupID; }
                set { SetProperty(ref _campaignGroupID, value, () => CampaignGroupID); }
            }

            private long? _planGroupID;
            public long? PlanGroupID
            {
                get { return _planGroupID; }
                set { SetProperty(ref _planGroupID, value, () => PlanGroupID); }
            }

            private long? _planID;
            public long? PlanID
            {
                get { return _planID; }
                set { SetProperty(ref _planID, value, () => PlanID); }
            }

            private long? _optionID;
            public long? OptionID
            {
                get { return _optionID; }
                set { SetProperty(ref _optionID, value, () => OptionID); }
            }

            private long? _optionFeesID;
            public long? OptionFeesID
            {
                get { return _optionFeesID; }
                set { SetProperty(ref _optionFeesID, value, () => OptionFeesID); }
            }

            private decimal? _la1Cover;
            public decimal? LA1Cover
            {
                get { return _la1Cover; }
                set { SetProperty(ref _la1Cover, value, () => LA1Cover); }
            }

            private decimal? _upgradePremium;
            public decimal? UpgradePremium
            {
                get { return _upgradePremium; }
                set { SetProperty(ref _upgradePremium, value, () => UpgradePremium); }
            }

            private decimal? _upgradeChildPremium;
            public decimal? UpgradeChildPremium
            {
                get { return _upgradeChildPremium; }
                set { SetProperty(ref _upgradeChildPremium, value, () => UpgradeChildPremium); }
            }

            private decimal? _totalPremium;
            public decimal? TotalPremium
            {
                get { return _totalPremium; }
                set { SetProperty(ref _totalPremium, value, () => TotalPremium); }
            }



            private decimal? _loadedTotalPremium;
            public decimal? LoadedTotalPremium
            {
                get { return _loadedTotalPremium; }
                set { SetProperty(ref _loadedTotalPremium, value, () => LoadedTotalPremium); }
            }

            private decimal? _totalInvoiceFee;
            public decimal? TotalInvoiceFee
            {
                get { return _totalInvoiceFee; }
                set { SetProperty(ref _totalInvoiceFee, value, () => TotalInvoiceFee); }
            }

            private decimal? _loadedTotalInvoiceFee;
            public decimal? LoadedTotalInvoiceFee
            {
                get { return _loadedTotalInvoiceFee; }
                set { SetProperty(ref _loadedTotalInvoiceFee, value, () => LoadedTotalInvoiceFee); }
            }



            private decimal? _moneyBackPayout;
            public decimal? MoneyBackPayout
            {
                get { return _moneyBackPayout; }
                set { SetProperty(ref _moneyBackPayout, value, () => MoneyBackPayout); }
            }

            private int? _moneyBackPayoutAge;
            public int? MoneyBackPayoutAge
            {
                get { return _moneyBackPayoutAge; }
                set { SetProperty(ref _moneyBackPayoutAge, value, () => MoneyBackPayoutAge); }
            }

            private bool _uDMBumpUpOption;
            public bool UDMBumpUpOption
            {
                get { return _uDMBumpUpOption; }
                set { SetProperty(ref _uDMBumpUpOption, value, () => UDMBumpUpOption); }
            }

            private decimal? _bumpUpAmount;
            public decimal? BumpUpAmount
            {
                get { return _bumpUpAmount; }
                set { SetProperty(ref _bumpUpAmount, value, () => BumpUpAmount); }
            }

            private bool _reducedPremiumOption;
            public bool ReducedPremiumOption
            {
                get { return _reducedPremiumOption; }
                set { SetProperty(ref _reducedPremiumOption, value, () => ReducedPremiumOption); }
            }

            private decimal? _reducedPremiumAmount;
            public decimal? ReducedPremiumAmount
            {
                get { return _reducedPremiumAmount; }
                set { SetProperty(ref _reducedPremiumAmount, value, () => ReducedPremiumAmount); }
            }

            private bool _isChildChecked;
            public bool IsChildChecked
            {
                get { return _isChildChecked; }
                set { SetProperty(ref _isChildChecked, value, () => IsChildChecked); }
            }

            private bool _isChildUpgradeChecked;
            public bool IsChildUpgradeChecked
            {
                get { return _isChildUpgradeChecked; }
                set { SetProperty(ref _isChildUpgradeChecked, value, () => IsChildUpgradeChecked); }
            }

            private bool _isLA2Checked;
            public bool IsLA2Checked
            {
                get { return _isLA2Checked; }
                set { SetProperty(ref _isLA2Checked, value, () => IsLA2Checked); }
            }

            private bool _isFuneralChecked;
            public bool IsFuneralChecked
            {
                get { return _isFuneralChecked; }
                set { SetProperty(ref _isFuneralChecked, value, () => IsFuneralChecked); }
            }

            private long? _platinumBumpupOptionID;
            public long? PlatinumBumpupOptionID
            {
                get { return _platinumBumpupOptionID; }
                set { SetProperty(ref _platinumBumpupOptionID, value, () => PlatinumBumpupOptionID); }
            }

            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/219813306/comments

            //private decimal? _la1FuneralCover;

            //public decimal? LA1FuneralCover
            //{
            //    get { return _la1FuneralCover; }
            //    set { SetProperty(ref _la1FuneralCover, value, () => LA1FuneralCover); }
            //}

            //private decimal? _la1AccidentalDeathCover;
            //public decimal? LA1AccidentalDeathCover
            //{
            //    get { return _la1AccidentalDeathCover; }
            //    set { SetProperty(ref _la1AccidentalDeathCover, value, () => LA1AccidentalDeathCover); }
            //}

            //private decimal? _funeralCover = 0;
            //public decimal? FuneralCover
            //{
            //    get { return _funeralCover; }
            //    set { SetProperty(ref _funeralCover, value, () => FuneralCover); }
            //}

            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/220453291/comments

            private bool _bumpUpOffered;
            public bool BumpUpOffered
            {
                get { return _bumpUpOffered; }
                set { SetProperty(ref _bumpUpOffered, value, () => BumpUpOffered); }
            }

            private bool _canOfferBumpUp;
            public bool CanOfferBumpUp
            {
                get { return _canOfferBumpUp; }
                set { SetProperty(ref _canOfferBumpUp, value, () => CanOfferBumpUp); }
            }

            private DateTime? _moneyBackDate;
            public DateTime? MoneyBackDate
            {
                get { return _moneyBackDate; }
                set { SetProperty(ref _moneyBackDate, value, () => MoneyBackDate); }
            }

            private DateTime? _moneyBackDate2;
            public DateTime? MoneyBackDate2
            {
                get { return _moneyBackDate2; }
                set { SetProperty(ref _moneyBackDate2, value, () => MoneyBackDate2); }
            }


        }
        public class PolicyHistory : Policy
        {

        }

        public class User : ObservableObject
        {
            private long? _userID;
            public long? UserID
            {
                get { return _userID; }
                set { SetProperty(ref _userID, value, () => UserID); }
            }

            private string _loginName;
            public string LoginName
            {
                get { return _loginName; }
                set { SetProperty(ref _loginName, value, () => LoginName); }
            }

            private long? _userTypeID;
            public long? UserTypeID
            {
                get { return _userTypeID; }
                set { SetProperty(ref _userTypeID, value, () => UserTypeID); }
            }

            private lkpUserType? _userType;
            public lkpUserType? UserType
            {
                get { return _userType; }
                set { SetProperty(ref _userType, value, () => UserType); }
            }

            private lkpStaffType? _staffType;
            public lkpStaffType? StaffType
            {
                get { return _staffType; }
                set { SetProperty(ref _staffType, value, () => StaffType); }
            }

            private DateTime? _tempStartDate;
            public DateTime? TempStartDate
            {
                get { return _tempStartDate; }
                set { SetProperty(ref _tempStartDate, value, () => TempStartDate); OnPropertyChanged("TempSMSAllowDate"); }
            }

            private DateTime _tempSMSAllowDate;
            public DateTime TempSMSAllowDate
            {

                //get { return _tempSMSAllowDate = TempStartDate.AddDays(); }
                //get { return _tempSMSAllowDate = (TempStartDate == null ? DateTime.MaxValue : DateTime.TryParse(TempStartDate.ToString(), out _tempSMSAllowDate) ? /*Convert.ToDateTime(TempStartDate?.ToString())).AddDays(28.00d)*/; }

                get { return _tempSMSAllowDate = DateTime.TryParse(TempStartDate.ToString(), out _tempSMSAllowDate) ? _tempSMSAllowDate.AddDays(18.00d) : DateTime.MaxValue; }
            }

        }

        public class Sale : ObservableObject
        {
            private long? _saleCallRefID;
            public long? SaleCallRefID
            {
                get { return _saleCallRefID; }
                set { SetProperty(ref _saleCallRefID, value, () => SaleCallRefID); }
            }

            private string _saleStationNo;
            public string SaleStationNo
            {
                get { return _saleStationNo; }
                set { SetProperty(ref _saleStationNo, value, () => SaleStationNo); }
            }

            private DateTime? _saleDate;
            public DateTime? SaleDate
            {
                get { return _saleDate; }
                set { SetProperty(ref _saleDate, value, () => SaleDate); }
            }

            private TimeSpan? _saleTime;
            public TimeSpan? SaleTime
            {
                get { return _saleTime; }
                set { SetProperty(ref _saleTime, value, () => SaleTime); }
            }

            private lkpTelNumberType? _saleTelNumberType;
            public lkpTelNumberType? SaleTelNumberType
            {
                get { return _saleTelNumberType; }
                set { SetProperty(ref _saleTelNumberType, value, () => SaleTelNumberType); }
            }

            private long? _bankCallRefID;
            public long? BankCallRefID
            {
                get { return _bankCallRefID; }
                set { SetProperty(ref _bankCallRefID, value, () => BankCallRefID); }
            }

            private string _bankStationNo;
            public string BankStationNo
            {
                get { return _bankStationNo; }
                set { SetProperty(ref _bankStationNo, value, () => BankStationNo); }
            }

            private DateTime? _bankDate;
            public DateTime? BankDate
            {
                get { return _bankDate; }
                set { SetProperty(ref _bankDate, value, () => BankDate); }
            }

            private TimeSpan? _bankTime;
            public TimeSpan? BankTime
            {
                get { return _bankTime; }
                set { SetProperty(ref _bankTime, value, () => BankTime); }
            }

            private lkpTelNumberType? _bankTelNumberType;
            public lkpTelNumberType? BankTelNumberType
            {
                get { return _bankTelNumberType; }
                set { SetProperty(ref _bankTelNumberType, value, () => BankTelNumberType); }
            }

            private long? _confCallRefID;
            public long? ConfCallRefID
            {
                get { return _confCallRefID; }
                set { SetProperty(ref _confCallRefID, value, () => ConfCallRefID); }

            }

            private long? _batchCallRefID;
            public long? BatchCallRefID
            {
                get { return _batchCallRefID; }
                set { SetProperty(ref _batchCallRefID, value, () => BatchCallRefID); }
            }

            private long? _fkCMCallRefUserID;
            public long? FKCMCallRefUserID
            {
                get { return _fkCMCallRefUserID; }
                set { SetProperty(ref _fkCMCallRefUserID, value, () => FKCMCallRefUserID); }
            }

            private string _confStationNo;
            public string ConfStationNo
            {
                get { return _confStationNo; }
                set { SetProperty(ref _confStationNo, value, () => ConfStationNo); }
            }

            private DateTime? _confDate;
            public DateTime? ConfDate
            {
                get { return _confDate; }
                set { SetProperty(ref _confDate, value, () => ConfDate); }
            }

            private DateTime? _confWorkDate;
            public DateTime? ConfWorkDate
            {
                get { return _confWorkDate; }
                set { SetProperty(ref _confWorkDate, value, () => ConfWorkDate); }
            }

            private TimeSpan? _confTime;
            public TimeSpan? ConfTime
            {
                get { return _confTime; }
                set { SetProperty(ref _confTime, value, () => ConfTime); }
            }

            private lkpTelNumberType? _confTelNumberType;
            public lkpTelNumberType? ConfTelNumberType
            {
                get { return _confTelNumberType; }
                set { SetProperty(ref _confTelNumberType, value, () => ConfTelNumberType); }
            }

            private long? _leadFeedbackID;
            public long? LeadFeedbackID
            {
                get { return _leadFeedbackID; }
                set { SetProperty(ref _leadFeedbackID, value, () => LeadFeedbackID); }
            }

            private string _feedback;
            public string Feedback
            {
                get { return _feedback; }
                set { SetProperty(ref _feedback, value, () => Feedback); }
            }

            private DateTime? _feedbackDate;
            public DateTime? FeedbackDate
            {
                get { return _feedbackDate; }
                set { SetProperty(ref _feedbackDate, value, () => FeedbackDate); }
            }

            private long? _confirmationFeedbackID;
            public long? ConfirmationFeedbackID
            {
                get { return _confirmationFeedbackID; }
                set { SetProperty(ref _confirmationFeedbackID, value, () => ConfirmationFeedbackID); }
            }

            private string _notes;
            public string Notes
            {
                get { return _notes; }
                set { SetProperty(ref _notes, value, () => Notes); }
            }


            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/214113774/comments
            private short _pageNumber;
            public short PageNumber
            {
                get { return _pageNumber; }
                set { SetProperty(ref _pageNumber, value, () => PageNumber); }
            }

            private bool _canChangeConfirmationDetails;
            public bool CanChangeConfirmationDetails
            {
                get { return _canChangeConfirmationDetails; }
                set { SetProperty(ref _canChangeConfirmationDetails, value, () => CanChangeConfirmationDetails); }
            }

            private bool _canChangeConfirmationCallRefUser;
            public bool CanChangeConfirmationCallRefUser
            {
                get { return _canChangeConfirmationCallRefUser; }
                set { SetProperty(ref _canChangeConfirmationCallRefUser, value, () => CanChangeConfirmationCallRefUser); }
            }

            private bool _canConfirmPolicy;
            public bool CanConfirmPolicy // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/217829500/comments
            {
                get
                {
                    return _canConfirmPolicy;
                }
                set
                {
                    SetProperty(ref _canConfirmPolicy, value, () => CanConfirmPolicy);
                }
            }

            private DateTime? _autoPopulateConfWorkDate;
            public DateTime? AutoPopulateConfWorkDate  // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/217829500/comments
            {
                get
                {
                    return _autoPopulateConfWorkDate;
                }
                set
                {
                    SetProperty(ref _autoPopulateConfWorkDate, value, () => AutoPopulateConfWorkDate);
                }
            }

            private DateTime? _autoPopulateConfDate;
            public DateTime? AutoPopulateConfDate  // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/217829500/comments
            {
                get
                {
                    return _autoPopulateConfDate;
                }
                set
                {
                    SetProperty(ref _autoPopulateConfDate, value, () => AutoPopulateConfDate);
                }
            }

            private TimeSpan? _autoPopulateConfTime;
            public TimeSpan? AutoPopulateConfTime // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/217829500/comments
            {
                get
                {
                    return _autoPopulateConfTime;
                }
                set
                {
                    SetProperty(ref _autoPopulateConfTime, value, () => AutoPopulateConfTime);
                }
            }

            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/220286902/comments
            private long? _autoPopulateConfirmationAgentFKUserID;


            public long? AutoPopulateConfirmationAgentFKUserID
            {
                get
                {
                    return _autoPopulateConfirmationAgentFKUserID;
                }

                set
                {
                    SetProperty(ref _autoPopulateConfirmationAgentFKUserID, value, () => AutoPopulateConfirmationAgentFKUserID);
                }
            }


            //private long? AutoPopulateConfirmationAgentFKUserID
        }

        public class BankDetails : PersonExt
        {
            public long? BankDetailsID { get; set; }

            private long? _paymentTypeID;
            public long? PaymentTypeID
            {
                get { return _paymentTypeID; }
                set { SetProperty(ref _paymentTypeID, value, () => PaymentTypeID); }
            }

            private lkpPaymentType? _paymentType;
            public lkpPaymentType? PaymentType
            {
                get { return _paymentType; }
                set { SetProperty(ref _paymentType, value, () => PaymentType); }
            }

            private long? _signingPowerID;
            public long? SigningPowerID
            {
                get { return _signingPowerID; }
                set { SetProperty(ref _signingPowerID, value, () => SigningPowerID); }
            }

            private string _accountHolder;
            public string AccountHolder
            {
                get { return _accountHolder; }
                set { SetProperty(ref _accountHolder, value, () => AccountHolder); }
            }

            private long? _bankID;
            public long? BankID
            {
                get { return _bankID; }
                set
                {
                    SetProperty(ref _bankID, value, () => BankID);
                }
            }

            private long? _bankBranchCodeID;
            public long? BankBranchCodeID
            {
                get { return _bankBranchCodeID; }
                set { SetProperty(ref _bankBranchCodeID, value, () => BankBranchCodeID); }
            }

            private string _accountNumber;
            public string AccountNumber
            {
                get { return _accountNumber; }
                set { SetProperty(ref _accountNumber, value, () => AccountNumber); }
            }

            private long? _accountTypeID;
            public long? AccountTypeID
            {
                get { return _accountTypeID; }
                set
                {
                    SetProperty(ref _accountTypeID, value, () => AccountTypeID);
                }
            }

            private short? _debitDay;
            public short? DebitDay
            {
                get { return _debitDay; }
                set { SetProperty(ref _debitDay, value, () => DebitDay); }
            }

            private string _bankAccountNumberPattern;
            public string BankAccountNumberPattern
            {
                get { return _bankAccountNumberPattern; }
                set { SetProperty(ref _bankAccountNumberPattern, value, () => BankAccountNumberPattern); }
            }

            private byte? _AccNumCheckStatusID = 0; //Follows LeadStatus
            public byte? AccNumCheckStatusID
            {
                get { return _AccNumCheckStatusID; }
                set { SetProperty(ref _AccNumCheckStatusID, value, () => AccNumCheckStatusID); }
            }

            private lkpINAccNumCheckStatus? _lkpAccNumCheckStatus = 0; //Follows LeadStatus
            public lkpINAccNumCheckStatus? lkpAccNumCheckStatus
            {
                get { return _lkpAccNumCheckStatus; }
                set { SetProperty(ref _lkpAccNumCheckStatus, value, () => lkpAccNumCheckStatus); }
            }

            private string _accNumCheckMsg = "";
            public string AccNumCheckMsg
            {
                get { return _accNumCheckMsg; }
                set { SetProperty(ref _accNumCheckMsg, value, () => AccNumCheckMsg); }
            }

            private string _accNumCheckMsgFull = "";
            public string AccNumCheckMsgFull
            {
                get { return _accNumCheckMsgFull; }
                set { SetProperty(ref _accNumCheckMsgFull, value, () => AccNumCheckMsgFull); }
            }
        }

        public class BankDetailsHistory : BankDetails
        {

        }

        public class ImportedPolicy : ObservableObject
        {
            public long? ImportedPolicyDataID { get; set; }

            private DateTime? _lapseDate;
            public DateTime? LapseDate
            {
                get { return _lapseDate; }
                set { SetProperty(ref _lapseDate, value, () => LapseDate); }
            }

            private DateTime? _commenceDate;
            public DateTime? CommenceDate
            {
                get { return _commenceDate; }
                set { SetProperty(ref _commenceDate, value, () => CommenceDate); }
            }

            private decimal? _contractPremium;
            public decimal? ContractPremium
            {
                get { return _contractPremium; }
                set { SetProperty(ref _contractPremium, value, () => ContractPremium); }
            }

            private DateTime? _moneyBackDate;
            public DateTime? MoneyBackDate
            {
                get { return _moneyBackDate; }
                set { SetProperty(ref _moneyBackDate, value, () => MoneyBackDate); }
            }
        }

        public class Closure : ObservableObject
        {
            private long? _closureID;
            public long? ClosureID
            {
                get { return _closureID; }
                set { SetProperty(ref _closureID, value, () => ClosureID); }
            }

            private long? _closureLanguageID;
            public long? ClosureLanguageID
            {
                get { return _closureLanguageID; }
                set { SetProperty(ref _closureLanguageID, value, () => ClosureLanguageID); }
            }

            private bool? _permissionQuestionAsked;
            public bool? PermissionQuestionAsked
            {
                get
                {
                    return _permissionQuestionAsked;
                }
                set
                {
                    SetProperty(ref _permissionQuestionAsked, value, () => PermissionQuestionAsked);
                }
            }
        }

        public class RedeemGift : ObservableObject
        {
            private long? _giftRedeemID;
            public long? GiftRedeemID
            {
                get { return _giftRedeemID; }
                set { SetProperty(ref _giftRedeemID, value, () => GiftRedeemID); }
            }

            private long? _giftRedeemStatusID;
            public long? GiftRedeemStatusID
            {
                get { return _giftRedeemStatusID; }
                set { SetProperty(ref _giftRedeemStatusID, value, () => GiftRedeemStatusID); }
            }

            private lkpINGiftRedeemStatus? _giftRedeemStatus;
            public lkpINGiftRedeemStatus? GiftRedeemStatus
            {
                get { return _giftRedeemStatus; }
                set { SetProperty(ref _giftRedeemStatus, value, () => GiftRedeemStatus); }
            }

            private long? _giftOptionID;
            public long? GiftOptionID
            {
                get { return _giftOptionID; }
                set { SetProperty(ref _giftOptionID, value, () => GiftOptionID); }
            }

            private DateTime? _dateRedeemed;
            public DateTime? DateRedeemed
            {
                get { return _dateRedeemed; }
                set { SetProperty(ref _dateRedeemed, value, () => DateRedeemed); }
            }

            private DateTime? _pODDate;
            public DateTime? PODDate
            {
                get { return _pODDate; }
                set { SetProperty(ref _pODDate, value, () => PODDate); }
            }

            private string _pODSignature;
            public string PODSignature
            {
                get { return _pODSignature; }
                set { SetProperty(ref _pODSignature, value, () => PODSignature); }
            }
        }

        public class SMSSend : ObservableObject
        {
            private string _to;
            public string to
            {
                get { return _to; }
                set { SetProperty(ref _to, value, () => to); }
            }

            private string _body;
            public string body
            {
                get { return _body; }
                set { SetProperty(ref _body, value, () => body); }
            }
        }

        public class SMS : ObservableObject
        {
            private WPF.Enumerations.Insure.lkpSMSStatusType? _smsStatusTypeID;
            public WPF.Enumerations.Insure.lkpSMSStatusType? SMSStatusTypeID
            {
                get { return _smsStatusTypeID; }
                set { SetProperty(ref _smsStatusTypeID, value, () => SMSStatusTypeID); }
            }

            private WPF.Enumerations.Insure.lkpSMSStatusSubtype? _smsStatusSubtypeID;
            public WPF.Enumerations.Insure.lkpSMSStatusSubtype? SMSStatusSubtypeID
            {
                get { return _smsStatusSubtypeID; }
                set { SetProperty(ref _smsStatusSubtypeID, value, () => SMSStatusSubtypeID); }
            }

            private DateTime? _smsSubmissionDate;
            public DateTime? SMSSubmissionDate
            {
                get { return _smsSubmissionDate; }
                set { SetProperty(ref _smsSubmissionDate, value, () => SMSSubmissionDate); OnPropertyChanged("SMSExpiryDate"); }
            }

            private string _smsToolTip;
            public string SMSToolTip
            {
                get { return _smsToolTip; }
                set { SetProperty(ref _smsToolTip, value, () => SMSToolTip); }
            }

            private bool? _isSMSSent = false;
            public bool? IsSMSSent
            {
                get { return _isSMSSent; }
                set { SetProperty(ref _isSMSSent, value, () => IsSMSSent); }
            }

            private DateTime _smsExpiryDate;
            public DateTime SMSExpiryDate
            {
                //get { return ((DateTime)SMSSubmissionDate).AddDays(28); }

                get { return _smsExpiryDate = (SMSSubmissionDate == null ? DateTime.MinValue : Convert.ToDateTime(SMSSubmissionDate.ToString())).AddDays(28.00d); }


            }

            //private int _smsCount;

            private int? _smsCount;
            public int? SMSCount
            {
                get { return _smsCount; }
                set
                {
                    SetProperty(ref _smsCount, value, () => SMSCount);
                }
            }
        }

        public class SMSResponse : ObservableObject
        {
            private string _id;
            public string id
            {
                get { return _id; }
                set { SetProperty(ref _id, value, () => id); }
            }

            private WPF.Enumerations.Insure.lkpSMSType? _type;
            public WPF.Enumerations.Insure.lkpSMSType? type
            {
                get { return _type; }
                set { SetProperty(ref _type, value, () => type); }
            }

            private string _from;
            public string from
            {
                get { return _from; }
                set { SetProperty(ref _from, value, () => from); }
            }

            private string _to;
            public string to
            {
                get { return _to; }
                set { SetProperty(ref _to, value, () => to); }
            }

            private string _body;
            public string body
            {
                get { return _body; }
                set { SetProperty(ref _body, value, () => body); }
            }

            private WPF.Enumerations.Insure.lkpSMSEncoding? _encoding;
            public WPF.Enumerations.Insure.lkpSMSEncoding? encoding
            {
                get { return _encoding; }
                set { SetProperty(ref _encoding, value, () => encoding); }
            }

            private int? _protocolId;
            public int? protocolId
            {
                get { return _protocolId; }
                set { SetProperty(ref _protocolId, value, () => protocolId); }
            }

            private int? _messageClass;
            public int? messageClass
            {
                get { return _messageClass; }
                set { SetProperty(ref _messageClass, value, () => messageClass); }
            }

            private SMSSubmission _submission;
            public SMSSubmission submission
            {
                get { return _submission; }
                set { SetProperty(ref _submission, value, () => submission); }
            }

            private SMSStatus _status;
            public SMSStatus status
            {
                get { return _status; }
                set { SetProperty(ref _status, value, () => status); }
            }

            private string _relatedSentMessageId;
            public string relatedSentMessageId
            {
                get { return _relatedSentMessageId; }
                set { SetProperty(ref _relatedSentMessageId, value, () => relatedSentMessageId); }
            }

            private string _userSuppliedId;
            public string userSuppliedId
            {
                get { return _userSuppliedId; }
                set { SetProperty(ref _userSuppliedId, value, () => userSuppliedId); }
            }

            private int? _numberOfParts;
            public int? numberOfParts
            {
                get { return _numberOfParts; }
                set { SetProperty(ref _numberOfParts, value, () => numberOfParts); }
            }

            private float? _creditCost;
            public float? creditCost
            {
                get { return _creditCost; }
                set { SetProperty(ref _creditCost, value, () => creditCost); }
            }
        }

        public class SMSSubmission : ObservableObject
        {
            private string _id;
            public string id
            {
                get { return _id; }
                set { SetProperty(ref _id, value, () => id); }
            }

            private DateTime _date;
            public DateTime date
            {
                get { return _date; }
                set { SetProperty(ref _date, value, () => date); }
            }
        }

        public class SMSStatus : ObservableObject
        {
            private string _id;
            public string id
            {
                get { return _id; }
                set { SetProperty(ref _id, value, () => id); }
            }

            private WPF.Enumerations.Insure.lkpSMSStatusType? _type;
            public WPF.Enumerations.Insure.lkpSMSStatusType? type
            {
                get { return _type; }
                set { SetProperty(ref _type, value, () => type); }
            }

            private WPF.Enumerations.Insure.lkpSMSStatusSubtype? _subtype;
            public WPF.Enumerations.Insure.lkpSMSStatusSubtype? subtype
            {
                get { return _subtype; }
                set { SetProperty(ref _subtype, value, () => subtype); }
            }
        }

        public class CTPhone : ObservableObject
        {
            private bool _isAgentLoggedIn;
            public bool IsAgentLoggedIn
            {
                get { return _isAgentLoggedIn; }
                set { SetProperty(ref _isAgentLoggedIn, value, () => IsAgentLoggedIn); }
            }

            private bool _isPhoneOffHook;
            public bool IsPhoneOffHook
            {
                get { return _isPhoneOffHook; }
                set { SetProperty(ref _isPhoneOffHook, value, () => IsPhoneOffHook); }
            }

            private CTPhoneCallStatus? _ctPhoneCallStatus;
            public CTPhoneCallStatus? CTPhoneCallStatus
            {
                get
                { return _ctPhoneCallStatus; }
                set { SetProperty(ref _ctPhoneCallStatus, value, () => CTPhoneCallStatus); }
            }

            private CTPhoneAgentStatus? _ctPhoneAgentStatus;
            public CTPhoneAgentStatus? CTPhoneAgentstatus
            {
                get
                { return _ctPhoneAgentStatus; }
                set { SetProperty(ref _ctPhoneAgentStatus, value, () => CTPhoneAgentstatus); }
            }

            private string _number;
            public string Number
            {
                get { return _number; }
                set { SetProperty(ref _number, value, () => Number); }
            }

            private string _extension;
            public string Extension
            {
                get { return _extension; }
                set { SetProperty(ref _extension, value, () => Extension); }
            }

            private string _recRef;
            public string RecRef
            {
                get { return _recRef; }
                set { SetProperty(ref _recRef, value, () => RecRef); }
            }

            private bool _isAnyPhoneActive;
            public bool IsAnyPhoneActive
            {
                get { return _isAnyPhoneActive; }
                set { SetProperty(ref _isAnyPhoneActive, value, () => IsAnyPhoneActive); }
            }

            private bool UpdateIsAnyPhoneActive()
            {
                return
                    _isWorkPhoneActive ||
                    _isHomePhoneActive ||
                    _isCellPhoneActive ||
                    _isOtherPhoneActive ||
                    _is1023PhoneActive ||
                    _is10118PhoneActive;
            }

            private bool _isWorkPhoneActive;
            public bool IsWorkPhoneActive
            {
                get
                {
                    IsAnyPhoneActive = UpdateIsAnyPhoneActive();
                    return _isWorkPhoneActive;
                }
                set { SetProperty(ref _isWorkPhoneActive, value, () => IsWorkPhoneActive); }
            }

            private bool _isHomePhoneActive;
            public bool IsHomePhoneActive
            {
                get
                {
                    IsAnyPhoneActive = UpdateIsAnyPhoneActive();
                    return _isHomePhoneActive;
                }
                set { SetProperty(ref _isHomePhoneActive, value, () => IsHomePhoneActive); }
            }

            private bool _isCellPhoneActive;
            public bool IsCellPhoneActive
            {
                get
                {
                    IsAnyPhoneActive = UpdateIsAnyPhoneActive();
                    return _isCellPhoneActive;
                }
                set { SetProperty(ref _isCellPhoneActive, value, () => IsCellPhoneActive); }
            }

            private bool _isOtherPhoneActive;
            public bool IsOtherPhoneActive
            {
                get
                {
                    IsAnyPhoneActive = UpdateIsAnyPhoneActive();
                    return _isOtherPhoneActive;
                }
                set { SetProperty(ref _isOtherPhoneActive, value, () => IsOtherPhoneActive); }
            }

            private bool _is1023PhoneActive;
            public bool Is1023PhoneActive
            {
                get
                {
                    IsAnyPhoneActive = UpdateIsAnyPhoneActive();
                    return _is1023PhoneActive;
                }
                set { SetProperty(ref _is1023PhoneActive, value, () => Is1023PhoneActive); }
            }

            private bool _is10118PhoneActive;
            public bool Is10118PhoneActive
            {
                get
                {
                    IsAnyPhoneActive = UpdateIsAnyPhoneActive();
                    return _is10118PhoneActive;
                }
                set { SetProperty(ref _is10118PhoneActive, value, () => Is10118PhoneActive); }
            }


        }

        public class INImportExtra : ObservableObject
        {
            private string _extension1;
            public string Extension1
            {
                get { return _extension1; }
                set { SetProperty(ref _extension1, value, () => Extension1); }
            }

            private string _extension2;
            public string Extension2
            {
                get { return _extension2; }
                set { SetProperty(ref _extension2, value, () => Extension2); }
            }

            private bool _notPossibleBumpUp;
            public bool NotPossibleBumpUp
            {
                get { return _notPossibleBumpUp; }
                set { SetProperty(ref _notPossibleBumpUp, value, () => NotPossibleBumpUp); }
            }


        }

        public class INImportExtraHistory : INImportExtra
        {

        }


        #endregion Members



        #region Properties

        public DataTable dtBank { get; internal set; }

        public DataTable dtFakeBankAccountNumbers { get; internal set; }

        #endregion



        #region Constructor

        public LeadApplicationData()
        {
            CopyToBenefCommand = new DelegateCommand(CopyToBenefExecute, CopyToBenefCanExecute);
            //ReferrorNOKCommand = new DelegateCommand(ReferrorNOKExecute, ReferrorNOKCanExecute); 

            TodayLess1Day = DateTime.Now.AddHours(-24);

            //TodayLess7Days = DateTime.Now.AddHours(-720); // 30 Days //DateTime.Now.AddHours(-168); //7 Days
            //TodayLess7Days = DateTime.Now.AddHours(-120); //5 Days

            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/205917879/comments
            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/216030894/comments
            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/216031385/comments

            int hoursToDeductFromToday = Insure.INCalculateConfirmationWorkWindow();
            TodayLess7Days = DateTime.Now.AddHours(hoursToDeductFromToday);

            UserData.UserID = ((Business.User)GlobalSettings.ApplicationUser).ID;
            UserData.LoginName = ((Business.User)GlobalSettings.ApplicationUser).LoginName;
            UserData.UserTypeID = ((Business.User)GlobalSettings.ApplicationUser).FKUserType;
            UserData.UserType = (lkpUserType?)((Business.User)GlobalSettings.ApplicationUser).FKUserType;

            //Preload these table to prevent slow downs elsewhere in the lead application screen
            dtTitles = Methods.GetTableData("SELECT [ID], [Description] FROM lkpINTitle");
            dtCampaignGroupSet = Methods.GetTableData("SELECT * FROM INCampaignGroupSet");
            dtCampaignTypeSet = Methods.GetTableData("SELECT * FROM INCampaignTypeSet");

            dtBank = Methods.GetTableData("SELECT * FROM lkpBank");
            dtFakeBankAccountNumbers = Methods.GetTableData("SELECT * FROM INFakeBankAccountNumbers");

            importID = AppData.ImportID;
        }

        #endregion



        #region Public Methods

        public void Clear()
        {
            AppData = new Application();
            AppData2 = new Application2();
            UserData = new User();
            PolicyData = new Policy();
            PolicyHistoryData = new PolicyHistory();
            LeadData = new Lead();
            ClosureData = new Closure();
            RedeemGiftData = new RedeemGift();
            LeadHistoryData = new LeadHistory();
            LAData = new ObservableCollection<LA>((new LA[MaxLA]).Select(c => new LA()));
            LAHistoryData = new ObservableCollection<LAHistory>((new LAHistory[MaxLA]).Select(c => new LAHistory()));
            ChildData = new ObservableCollection<Child>((new Child[MaxChildren]).Select(c => new Child()));
            ChildHistoryData = new ObservableCollection<ChildHistory>((new ChildHistory[MaxChildren]).Select(c => new ChildHistory()));
            BeneficiaryData = new ObservableCollection<Beneficiary>((new Beneficiary[MaxBeneficiaries]).Select(c => new Beneficiary()));
            BeneficiaryHistoryData = new ObservableCollection<BeneficiaryHistory>((new BeneficiaryHistory[MaxBeneficiaries]).Select(c => new BeneficiaryHistory()));
            NextOfKinData = new ObservableCollection<NextOfKin>((new NextOfKin[MaxNextOfKin]).Select(c => new NextOfKin()));
            NextOfKinHistoryData = new ObservableCollection<NextOfKinHistory>((new NextOfKinHistory[MaxNextOfKin]).Select(c => new NextOfKinHistory()));
            BankDetailsData = new BankDetails();
            BankDetailsHistoryData = new BankDetailsHistory();
            SaleData = new Sale();
            SMSData = new SMS();
            ImportedPolicyData = new ImportedPolicy();
            ImportExtraData = new INImportExtra();
            ImportExtraHistoryData = new INImportExtraHistory();
            ImportedCovers = new ObservableCollection<ImportedCover>
            {
                new ImportedCover { Name ="LA1 Cancer" },
                new ImportedCover { Name ="LA1 Disability" },
                new ImportedCover { Name ="LA1 Acc Death" },
                new ImportedCover { Name ="LA1 Funeral" },
                new ImportedCover { Name ="LA2 Cancer" },
                new ImportedCover { Name ="LA2 Disability" },
                new ImportedCover { Name ="LA2 Acc Death" },
                new ImportedCover { Name ="LA2 Funeral" },
                new ImportedCover { Name ="Kids Cancer" },
                new ImportedCover { Name ="Kids Disability" }
            };

            //Set Default Values
            TodayLess1Day = DateTime.Now.AddHours(-24);

            //See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/205917879/comments
            TodayLess7Days = DateTime.Now.AddHours(-720); // 30 Days //DateTime.Now.AddHours(-168); //7 Days

            UserData.UserID = ((Business.User)GlobalSettings.ApplicationUser).ID;
            UserData.LoginName = ((Business.User)GlobalSettings.ApplicationUser).LoginName;
            UserData.UserTypeID = ((Business.User)GlobalSettings.ApplicationUser).FKUserType;
            UserData.UserType = (lkpUserType?)((Business.User)GlobalSettings.ApplicationUser).FKUserType;

            //CTPhoneData.RecRef = string.Empty;
            //CTPhoneData.Number = string.Empty;
            //CTPhoneData.IsPhoneOffHook = false;
            //CTPhoneData.CTPhoneAgentstatus = CTPhoneAgentStatus.Ready;
            //CTPhoneData.CTPhoneCallStatus = CTPhoneCallStatus.Disconnected;
        }

        public void VerifyAccountNumber(string branchCode, string accountNum)
        {

            var serviceClient = new CdvWCFClient();
            //string response = serviceClient.CdvEFT("SOAP", "UDM1", "Password1", "223226", "03000015318");
            //string response = serviceClient.CdvEFT("UDM1", "SOAP", "Password1", "259605", "62292826917");
            cdvResponse response = serviceClient.checkCDV(mercUsername, mercInstCode, mercPassword, branchCode != null ? branchCode : "", accountNum != null ? accountNum : "");

            int length = response.cdvResult.Length;
            string passFailString = !string.IsNullOrWhiteSpace(response.cdvResult) && response.cdvResult.Length >= 2
            ? response.cdvResult.Substring(0, 2)
            : response.cdvResult;

            BankDetailsData.AccNumCheckMsg = !string.IsNullOrWhiteSpace(response.cdvResult) && response.cdvResult.Length >= 2
            ? response.cdvResult.Substring(4, response.cdvResult.Length - 4)
            : response.cdvResult;
            BankDetailsData.AccNumCheckMsgFull = response.cdvResult;
            //switch (passFailString)
            //{
            //    case "00":
            //        //pass
            //        break;
            //    case "04":
            //        //branch not found
            //        break;
            //    case "05":
            //        //CDV exception code failed
            //        break;
            //    case "07":
            //        //CDV failure
            //        break;
            //}
            if (passFailString == "00")
            {
                //Pass
                BankDetailsData.lkpAccNumCheckStatus = lkpINAccNumCheckStatus.Valid;
            }
            //else if (passFailString.Length != 2)
            //{
            //    //Error
            //}
            else
            {
                //Fail
                BankDetailsData.lkpAccNumCheckStatus = lkpINAccNumCheckStatus.Invalid;
            }

            //CdvEFTRequest request = new CdvEFTRequest("Test", "EBU1", "hello", "223226", "03000015318");

            //CdvEFTResponse response =

            //CdvEFTResponse response = CdvEFT(request);
            //MessageBox.Show(CdvEFT(request).Result);

            //Save Mercantile Verify Details
            Mercantile mercantile = new Mercantile();
            mercantile.FKSystemID = 2;
            mercantile.FKImportID = AppData.ImportID;
            mercantile.FKBankID = BankDetailsData.BankID;
            mercantile.FKBankBranchID = BankDetailsData.BankBranchCodeID;
            mercantile.AccountNumber = BankDetailsData.AccountNumber;
            mercantile.AccNumCheckStatus = Convert.ToByte(BankDetailsData.lkpAccNumCheckStatus);
            mercantile.AccNumCheckMsg = BankDetailsData.AccNumCheckMsg;
            mercantile.AccNumCheckMsgFull = BankDetailsData.AccNumCheckMsgFull;
            mercantile.Save(null);
        }

        #endregion



        #region Private Methods



        #endregion



        #region Application

        private Application _appData = new Application();
        public Application AppData
        {
            get { return _appData; }
            set { SetProperty(ref _appData, value, () => AppData); }
        }

        private Application2 _appData2 = new Application2();
        public Application2 AppData2
        {
            get { return _appData2; }
            set { SetProperty(ref _appData2, value, () => AppData2); }
        }

        #endregion



        #region Policy

        private Policy _policyData = new Policy();
        public Policy PolicyData
        {
            get { return _policyData; }
            set { SetProperty(ref _policyData, value, () => PolicyData); }
        }

        private PolicyHistory _policyHistoryData = new PolicyHistory();
        public PolicyHistory PolicyHistoryData
        {
            get { return _policyHistoryData; }
            set { SetProperty(ref _policyHistoryData, value, () => PolicyHistoryData); }
        }



        #endregion



        #region Lead

        private Lead _leadData = new Lead();
        public Lead LeadData
        {
            get { return _leadData; }
            set { SetProperty(ref _leadData, value, () => LeadData); }
        }

        private LeadHistory _leadHistoryData = new LeadHistory();
        public LeadHistory LeadHistoryData
        {
            get { return _leadHistoryData; }
            set { SetProperty(ref _leadHistoryData, value, () => LeadHistoryData); }
        }

        #endregion



        #region LA

        private ObservableCollection<LA> _laData = new ObservableCollection<LA>((new LA[MaxLA]).Select(c => new LA()));
        public ObservableCollection<LA> LAData
        {
            get { return _laData; }
            set { SetProperty(ref _laData, value, () => LAData); }
        }

        private ObservableCollection<LAHistory> _laHistoryData = new ObservableCollection<LAHistory>((new LAHistory[MaxLA]).Select(c => new LAHistory()));
        public ObservableCollection<LAHistory> LAHistoryData
        {
            get { return _laHistoryData; }
            set { SetProperty(ref _laHistoryData, value, () => LAHistoryData); }
        }

        #endregion



        #region Children

        private ObservableCollection<Child> _childData = new ObservableCollection<Child>((new Child[MaxChildren]).Select(c => new Child()));
        public ObservableCollection<Child> ChildData
        {
            get { return _childData; }
            set { SetProperty(ref _childData, value, () => ChildData); }
        }

        private ObservableCollection<ChildHistory> _childHistoryData = new ObservableCollection<ChildHistory>((new ChildHistory[MaxChildren]).Select(c => new ChildHistory()));
        public ObservableCollection<ChildHistory> ChildHistoryData
        {
            get { return _childHistoryData; }
            set { SetProperty(ref _childHistoryData, value, () => ChildHistoryData); }
        }

        #endregion



        #region Beneficiary

        private ObservableCollection<Beneficiary> _beneficiaryData = new ObservableCollection<Beneficiary>((new Beneficiary[MaxBeneficiaries]).Select(c => new Beneficiary()));
        public ObservableCollection<Beneficiary> BeneficiaryData
        {
            get { return _beneficiaryData; }
            set { SetProperty(ref _beneficiaryData, value, () => BeneficiaryData); }
        }

        private ObservableCollection<BeneficiaryHistory> _beneficiaryHistoryData = new ObservableCollection<BeneficiaryHistory>((new BeneficiaryHistory[MaxBeneficiaries]).Select(c => new BeneficiaryHistory()));
        public ObservableCollection<BeneficiaryHistory> BeneficiaryHistoryData
        {
            get { return _beneficiaryHistoryData; }
            set { SetProperty(ref _beneficiaryHistoryData, value, () => BeneficiaryHistoryData); }
        }

        #endregion



        #region NextOfKin

        private ObservableCollection<NextOfKin> _nextOfKinData = new ObservableCollection<NextOfKin>((new NextOfKin[MaxNextOfKin]).Select(c => new NextOfKin()));
        public ObservableCollection<NextOfKin> NextOfKinData
        {
            get { return _nextOfKinData; }
            set { SetProperty(ref _nextOfKinData, value, () => NextOfKinData); }
        }

        private ObservableCollection<NextOfKinHistory> _nextOfKinHistoryData = new ObservableCollection<NextOfKinHistory>((new NextOfKinHistory[MaxNextOfKin]).Select(c => new NextOfKinHistory()));
        public ObservableCollection<NextOfKinHistory> NextOfKinHistoryData
        {
            get { return _nextOfKinHistoryData; }
            set { SetProperty(ref _nextOfKinHistoryData, value, () => NextOfKinHistoryData); }
        }

        #endregion



        #region Sale

        private Sale _saleData = new Sale();
        public Sale SaleData
        {
            get { return _saleData; }
            set { SetProperty(ref _saleData, value, () => SaleData); }
        }

        #endregion



        #region Bank Details

        private BankDetails _bankDetailsData = new BankDetails();
        public BankDetails BankDetailsData
        {
            get { return _bankDetailsData; }
            set { SetProperty(ref _bankDetailsData, value, () => BankDetailsData); }
        }

        private BankDetailsHistory _bankDetailsHistoryData = new BankDetailsHistory();
        public BankDetailsHistory BankDetailsHistoryData
        {
            get { return _bankDetailsHistoryData; }
            set { SetProperty(ref _bankDetailsHistoryData, value, () => BankDetailsHistoryData); }
        }

        #endregion



        #region User

        private User _userData = new User();
        public User UserData
        {
            get { return _userData; }
            set { SetProperty(ref _userData, value, () => UserData); }
        }

        #endregion



        #region Imported Policy Data

        private ImportedPolicy _importedPolicyData = new ImportedPolicy();
        public ImportedPolicy ImportedPolicyData
        {
            get { return _importedPolicyData; }
            set { SetProperty(ref _importedPolicyData, value, () => ImportedPolicyData); }
        }

        private ObservableCollection<ImportedCover> _importedCovers = new ObservableCollection<ImportedCover>
        {
            new ImportedCover { Name ="LA1 Cancer" },
            new ImportedCover { Name ="LA1 Disability" },
            new ImportedCover { Name ="LA1 Acc Death" },
            new ImportedCover { Name ="LA1 Funeral" },
            new ImportedCover { Name ="LA2 Cancer" },
            new ImportedCover { Name ="LA2 Disability" },
            new ImportedCover { Name ="LA2 Acc Death" },
            new ImportedCover { Name ="LA2 Funeral" },
            new ImportedCover { Name ="Kids Cancer" },
            new ImportedCover { Name ="Kids Disability" }
        };
        public ObservableCollection<ImportedCover> ImportedCovers
        {
            get { return _importedCovers; }
            set { SetProperty(ref _importedCovers, value, () => ImportedCovers); }
        }

        #endregion



        #region Closure

        private Closure _closureData = new Closure();
        public Closure ClosureData
        {
            get { return _closureData; }
            set { SetProperty(ref _closureData, value, () => ClosureData); }
        }

        #endregion



        #region RedeemGiftData

        private RedeemGift _redeemGiftData = new RedeemGift();
        public RedeemGift RedeemGiftData
        {
            get { return _redeemGiftData; }
            set { SetProperty(ref _redeemGiftData, value, () => RedeemGiftData); }
        }

        #endregion



        #region CTPhone Data

        private CTPhone _ctPhoneData = new CTPhone();
        public CTPhone CTPhoneData
        {
            get { return _ctPhoneData; }
            set { SetProperty(ref _ctPhoneData, value, () => CTPhoneData); }
        }

        #endregion

        #region BulkSMS Message

        private SMSSend _smsSendData = new SMSSend();
        public SMSSend SMSSendData
        {
            get { return _smsSendData; }
            set { SetProperty(ref _smsSendData, value, () => SMSSendData); }
        }

        private SMS _smsData = new SMS();
        public SMS SMSData
        {
            get { return _smsData; }
            set { SetProperty(ref _smsData, value, () => SMSData); }
        }

        #endregion BulkSMS Message



        #region ImportExtraData

        private INImportExtra _importExtraData = new INImportExtra();
        public INImportExtra ImportExtraData
        {
            get { return _importExtraData; }
            set { SetProperty(ref _importExtraData, value, () => ImportExtraData); }
        }

        private INImportExtraHistory _importExtraHistoryData = new INImportExtraHistory();
        public INImportExtraHistory ImportExtraHistoryData
        {
            get { return _importExtraHistoryData; }
            set { SetProperty(ref _importExtraHistoryData, value, () => ImportExtraHistoryData); }
        }



        #endregion



        #region Commands

        static public DelegateCommand CopyToBenefCommand { get; private set; }

        private void CopyToBenefExecute()
        {
            int? benNumber = AppData.SelectedBeneficiaryIndex;

            if (benNumber != null && benNumber != -1)
            {
                int benIndex = Convert.ToInt32(benNumber);

                BeneficiaryData[benIndex].TitleID = LAData[1].TitleID;
                BeneficiaryData[benIndex].GenderID = LAData[1].GenderID;
                BeneficiaryData[benIndex].RelationshipID = LAData[1].RelationshipID;
                BeneficiaryData[benIndex].Name = LAData[1].Name;
                BeneficiaryData[benIndex].Surname = LAData[1].Surname;
                BeneficiaryData[benIndex].DateOfBirth = LAData[1].DateOfBirth;
                BeneficiaryData[benIndex].TelContact = LAData[1].TelContact;
            }
        }

        private bool CopyToBenefCanExecute()
        {
            int? benNumber = AppData.SelectedBeneficiaryIndex;

            if (benNumber != null && benNumber != -1)
            {
                int benIndex = Convert.ToInt32(benNumber); //Selected Beneficiary Index

                if (
                    BeneficiaryData[benIndex].TitleID == null
                    && BeneficiaryData[benIndex].GenderID == null
                    && BeneficiaryData[benIndex].RelationshipID == null
                    && string.IsNullOrWhiteSpace(BeneficiaryData[benIndex].Name)
                    && string.IsNullOrWhiteSpace(BeneficiaryData[benIndex].Surname)
                    && BeneficiaryData[benIndex].DateOfBirth == null
                    && string.IsNullOrWhiteSpace(BeneficiaryData[benIndex].TelContact)
                    && BeneficiaryData[benIndex].Percentage == null
                    && string.IsNullOrWhiteSpace(BeneficiaryData[benIndex].Notes)
                   )
                {
                    for (int index = 0; index < 6; index++)
                    {
                        if (
                            BeneficiaryData[index].TitleID == LAData[1].TitleID
                            && BeneficiaryData[index].GenderID == LAData[1].GenderID
                            && BeneficiaryData[index].RelationshipID == LAData[1].RelationshipID
                            && (BeneficiaryData[index].Name ?? "") == (LAData[1].Name ?? "")
                            && (BeneficiaryData[index].Surname ?? "") == (LAData[1].Surname ?? "")
                            && BeneficiaryData[index].DateOfBirth == LAData[1].DateOfBirth
                            && (BeneficiaryData[index].TelContact ?? "") == (LAData[1].TelContact ?? "")
                           )
                        {
                            return false;
                        }
                    }

                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }


        //static public DelegateCommand ReferrorNOKCommand { get; private set; }

        //private void ReferrorNOKExecute()
        //{
        //    AppData.SelectedReferrorNOK = AppData?.SelectedReferrorNOK == "Referror" || string.IsNullOrEmpty(AppData?.SelectedReferrorNOK) ? "NextOfKin" : "Referror";
        //}

        //private bool ReferrorNOKCanExecute()
        //{
        //    return true;
        //}

        #endregion


    }

}
