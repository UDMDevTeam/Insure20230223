using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using UDM.HR.Business.Mapping;
using Embriant.Framework.Configuration;
using Embriant.Framework;
using Embriant.Framework.Validation;

namespace UDM.HR.Business
{
    public partial class HRStaff : ObjectBase<long>
    {
        #region Members
        private string _employeeno = null;
        private DateTime? _tempstartdate = null;
        private DateTime? _employmentstartdate = null;
        private DateTime? _employmentenddate = null;
        private long? _fkcompanyid = null;
        private long? _fkstafftypeid = null;
        private long? _fkworkstatusid = null;
        private long? _fkjobtitleid = null;
        private long? _fkuserid = null;
        private long? _fkrecruitmentconsultantid = null;
        private string _recruitmentsource = null;
        private long? _fktitleid = null;
        private string _initials = null;
        private string _firstname = null;
        private string _preferredname = null;
        private string _surname = null;
        private long? _fkgenderid = null;
        private long? _fkraceid = null;
        private string _idno = null;
        private DateTime? _dob = null;
        private long? _fkcitizenid = null;
        private long? _fkreligionid = null;
        private string _telhome = null;
        private string _telcell = null;
        private string _telother = null;
        private string _email = null;
        private bool? _registeredtaxpayer = null;
        private string _taxrefno = null;
        private string _irp5number = null;
        private long? _fkmedicalaidid = null;
        private bool? _criminaloffence = null;
        private bool? _employmentdismissed = null;
        private bool? _disciplinaryguilty = null;
        private bool? _workedfinservicesindustry = null;
        private DateTime? _workedfinservicesindustryfrom = null;
        private DateTime? _workedfinservicesindustryto = null;
        private DateTime? _reglevel1examdate = null;
        private string _note = null;
        private long? _fkautomationstatusid = null;
        private DateTime? _automationdate = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the HRStaff class.
        /// </summary>
        public HRStaff()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the HRStaff class.
        /// </summary>
        public HRStaff(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public string EmployeeNo
        {
            get
            {
                Fill();
                return _employeeno;
            }
            set 
            {
                Fill();
                if (value != _employeeno)
                {
                    _employeeno = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? TempStartDate
        {
            get
            {
                Fill();
                return _tempstartdate;
            }
            set 
            {
                Fill();
                if (value != _tempstartdate)
                {
                    _tempstartdate = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? EmploymentStartDate
        {
            get
            {
                Fill();
                return _employmentstartdate;
            }
            set 
            {
                Fill();
                if (value != _employmentstartdate)
                {
                    _employmentstartdate = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? EmploymentEndDate
        {
            get
            {
                Fill();
                return _employmentenddate;
            }
            set 
            {
                Fill();
                if (value != _employmentenddate)
                {
                    _employmentenddate = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKCompanyID
        {
            get
            {
                Fill();
                return _fkcompanyid;
            }
            set 
            {
                Fill();
                if (value != _fkcompanyid)
                {
                    _fkcompanyid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKStaffTypeID
        {
            get
            {
                Fill();
                return _fkstafftypeid;
            }
            set 
            {
                Fill();
                if (value != _fkstafftypeid)
                {
                    _fkstafftypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKWorkStatusID
        {
            get
            {
                Fill();
                return _fkworkstatusid;
            }
            set 
            {
                Fill();
                if (value != _fkworkstatusid)
                {
                    _fkworkstatusid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKJobTitleID
        {
            get
            {
                Fill();
                return _fkjobtitleid;
            }
            set 
            {
                Fill();
                if (value != _fkjobtitleid)
                {
                    _fkjobtitleid = value;
                    _hasChanged = true;
                }
            }
        }

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

        public long? FKRecruitmentConsultantID
        {
            get
            {
                Fill();
                return _fkrecruitmentconsultantid;
            }
            set 
            {
                Fill();
                if (value != _fkrecruitmentconsultantid)
                {
                    _fkrecruitmentconsultantid = value;
                    _hasChanged = true;
                }
            }
        }

        public string RecruitmentSource
        {
            get
            {
                Fill();
                return _recruitmentsource;
            }
            set 
            {
                Fill();
                if (value != _recruitmentsource)
                {
                    _recruitmentsource = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKTitleID
        {
            get
            {
                Fill();
                return _fktitleid;
            }
            set 
            {
                Fill();
                if (value != _fktitleid)
                {
                    _fktitleid = value;
                    _hasChanged = true;
                }
            }
        }

        public string Initials
        {
            get
            {
                Fill();
                return _initials;
            }
            set 
            {
                Fill();
                if (value != _initials)
                {
                    _initials = value;
                    _hasChanged = true;
                }
            }
        }

        public string FirstName
        {
            get
            {
                Fill();
                return _firstname;
            }
            set 
            {
                Fill();
                if (value != _firstname)
                {
                    _firstname = value;
                    _hasChanged = true;
                }
            }
        }

        public string PreferredName
        {
            get
            {
                Fill();
                return _preferredname;
            }
            set 
            {
                Fill();
                if (value != _preferredname)
                {
                    _preferredname = value;
                    _hasChanged = true;
                }
            }
        }

        public string Surname
        {
            get
            {
                Fill();
                return _surname;
            }
            set 
            {
                Fill();
                if (value != _surname)
                {
                    _surname = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKGenderID
        {
            get
            {
                Fill();
                return _fkgenderid;
            }
            set 
            {
                Fill();
                if (value != _fkgenderid)
                {
                    _fkgenderid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKRaceID
        {
            get
            {
                Fill();
                return _fkraceid;
            }
            set 
            {
                Fill();
                if (value != _fkraceid)
                {
                    _fkraceid = value;
                    _hasChanged = true;
                }
            }
        }

        public string IDNo
        {
            get
            {
                Fill();
                return _idno;
            }
            set 
            {
                Fill();
                if (value != _idno)
                {
                    _idno = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? DOB
        {
            get
            {
                Fill();
                return _dob;
            }
            set 
            {
                Fill();
                if (value != _dob)
                {
                    _dob = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKCitizenID
        {
            get
            {
                Fill();
                return _fkcitizenid;
            }
            set 
            {
                Fill();
                if (value != _fkcitizenid)
                {
                    _fkcitizenid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKReligionID
        {
            get
            {
                Fill();
                return _fkreligionid;
            }
            set 
            {
                Fill();
                if (value != _fkreligionid)
                {
                    _fkreligionid = value;
                    _hasChanged = true;
                }
            }
        }

        public string TelHome
        {
            get
            {
                Fill();
                return _telhome;
            }
            set 
            {
                Fill();
                if (value != _telhome)
                {
                    _telhome = value;
                    _hasChanged = true;
                }
            }
        }

        public string TelCell
        {
            get
            {
                Fill();
                return _telcell;
            }
            set 
            {
                Fill();
                if (value != _telcell)
                {
                    _telcell = value;
                    _hasChanged = true;
                }
            }
        }

        public string TelOther
        {
            get
            {
                Fill();
                return _telother;
            }
            set 
            {
                Fill();
                if (value != _telother)
                {
                    _telother = value;
                    _hasChanged = true;
                }
            }
        }

        public string EMail
        {
            get
            {
                Fill();
                return _email;
            }
            set 
            {
                Fill();
                if (value != _email)
                {
                    _email = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? RegisteredTaxPayer
        {
            get
            {
                Fill();
                return _registeredtaxpayer;
            }
            set 
            {
                Fill();
                if (value != _registeredtaxpayer)
                {
                    _registeredtaxpayer = value;
                    _hasChanged = true;
                }
            }
        }

        public string TaxRefNo
        {
            get
            {
                Fill();
                return _taxrefno;
            }
            set 
            {
                Fill();
                if (value != _taxrefno)
                {
                    _taxrefno = value;
                    _hasChanged = true;
                }
            }
        }

        public string IRP5Number
        {
            get
            {
                Fill();
                return _irp5number;
            }
            set 
            {
                Fill();
                if (value != _irp5number)
                {
                    _irp5number = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKMedicalAidID
        {
            get
            {
                Fill();
                return _fkmedicalaidid;
            }
            set 
            {
                Fill();
                if (value != _fkmedicalaidid)
                {
                    _fkmedicalaidid = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? CriminalOffence
        {
            get
            {
                Fill();
                return _criminaloffence;
            }
            set 
            {
                Fill();
                if (value != _criminaloffence)
                {
                    _criminaloffence = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? EmploymentDismissed
        {
            get
            {
                Fill();
                return _employmentdismissed;
            }
            set 
            {
                Fill();
                if (value != _employmentdismissed)
                {
                    _employmentdismissed = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? DisciplinaryGuilty
        {
            get
            {
                Fill();
                return _disciplinaryguilty;
            }
            set 
            {
                Fill();
                if (value != _disciplinaryguilty)
                {
                    _disciplinaryguilty = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? WorkedFinServicesIndustry
        {
            get
            {
                Fill();
                return _workedfinservicesindustry;
            }
            set 
            {
                Fill();
                if (value != _workedfinservicesindustry)
                {
                    _workedfinservicesindustry = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? WorkedFinServicesIndustryFrom
        {
            get
            {
                Fill();
                return _workedfinservicesindustryfrom;
            }
            set 
            {
                Fill();
                if (value != _workedfinservicesindustryfrom)
                {
                    _workedfinservicesindustryfrom = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? WorkedFinServicesIndustryTo
        {
            get
            {
                Fill();
                return _workedfinservicesindustryto;
            }
            set 
            {
                Fill();
                if (value != _workedfinservicesindustryto)
                {
                    _workedfinservicesindustryto = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? RegLevel1ExamDate
        {
            get
            {
                Fill();
                return _reglevel1examdate;
            }
            set 
            {
                Fill();
                if (value != _reglevel1examdate)
                {
                    _reglevel1examdate = value;
                    _hasChanged = true;
                }
            }
        }

        public string Note
        {
            get
            {
                Fill();
                return _note;
            }
            set 
            {
                Fill();
                if (value != _note)
                {
                    _note = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKAutomationStatusID
        {
            get
            {
                Fill();
                return _fkautomationstatusid;
            }
            set 
            {
                Fill();
                if (value != _fkautomationstatusid)
                {
                    _fkautomationstatusid = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? AutomationDate
        {
            get
            {
                Fill();
                return _automationdate;
            }
            set 
            {
                Fill();
                if (value != _automationdate)
                {
                    _automationdate = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an HRStaff object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                HRStaffMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an HRStaff object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the HRStaff object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = HRStaffMapper.Save(this);
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
        /// Deletes an HRStaff object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the HRStaff object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && HRStaffMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the HRStaff.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<hrstaff>");
            xml.Append("<employeeno>" + EmployeeNo.ToString() + "</employeeno>");
            xml.Append("<tempstartdate>" + TempStartDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</tempstartdate>");
            xml.Append("<employmentstartdate>" + EmploymentStartDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</employmentstartdate>");
            xml.Append("<employmentenddate>" + EmploymentEndDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</employmentenddate>");
            xml.Append("<fkcompanyid>" + FKCompanyID.ToString() + "</fkcompanyid>");
            xml.Append("<fkstafftypeid>" + FKStaffTypeID.ToString() + "</fkstafftypeid>");
            xml.Append("<fkworkstatusid>" + FKWorkStatusID.ToString() + "</fkworkstatusid>");
            xml.Append("<fkjobtitleid>" + FKJobTitleID.ToString() + "</fkjobtitleid>");
            xml.Append("<fkuserid>" + FKUserID.ToString() + "</fkuserid>");
            xml.Append("<fkrecruitmentconsultantid>" + FKRecruitmentConsultantID.ToString() + "</fkrecruitmentconsultantid>");
            xml.Append("<recruitmentsource>" + RecruitmentSource.ToString() + "</recruitmentsource>");
            xml.Append("<fktitleid>" + FKTitleID.ToString() + "</fktitleid>");
            xml.Append("<initials>" + Initials.ToString() + "</initials>");
            xml.Append("<firstname>" + FirstName.ToString() + "</firstname>");
            xml.Append("<preferredname>" + PreferredName.ToString() + "</preferredname>");
            xml.Append("<surname>" + Surname.ToString() + "</surname>");
            xml.Append("<fkgenderid>" + FKGenderID.ToString() + "</fkgenderid>");
            xml.Append("<fkraceid>" + FKRaceID.ToString() + "</fkraceid>");
            xml.Append("<idno>" + IDNo.ToString() + "</idno>");
            xml.Append("<dob>" + DOB.Value.ToString("dd MMM yyyy HH:mm:ss") + "</dob>");
            xml.Append("<fkcitizenid>" + FKCitizenID.ToString() + "</fkcitizenid>");
            xml.Append("<fkreligionid>" + FKReligionID.ToString() + "</fkreligionid>");
            xml.Append("<telhome>" + TelHome.ToString() + "</telhome>");
            xml.Append("<telcell>" + TelCell.ToString() + "</telcell>");
            xml.Append("<telother>" + TelOther.ToString() + "</telother>");
            xml.Append("<email>" + EMail.ToString() + "</email>");
            xml.Append("<registeredtaxpayer>" + RegisteredTaxPayer.ToString() + "</registeredtaxpayer>");
            xml.Append("<taxrefno>" + TaxRefNo.ToString() + "</taxrefno>");
            xml.Append("<irp5number>" + IRP5Number.ToString() + "</irp5number>");
            xml.Append("<fkmedicalaidid>" + FKMedicalAidID.ToString() + "</fkmedicalaidid>");
            xml.Append("<criminaloffence>" + CriminalOffence.ToString() + "</criminaloffence>");
            xml.Append("<employmentdismissed>" + EmploymentDismissed.ToString() + "</employmentdismissed>");
            xml.Append("<disciplinaryguilty>" + DisciplinaryGuilty.ToString() + "</disciplinaryguilty>");
            xml.Append("<workedfinservicesindustry>" + WorkedFinServicesIndustry.ToString() + "</workedfinservicesindustry>");
            xml.Append("<workedfinservicesindustryfrom>" + WorkedFinServicesIndustryFrom.Value.ToString("dd MMM yyyy HH:mm:ss") + "</workedfinservicesindustryfrom>");
            xml.Append("<workedfinservicesindustryto>" + WorkedFinServicesIndustryTo.Value.ToString("dd MMM yyyy HH:mm:ss") + "</workedfinservicesindustryto>");
            xml.Append("<reglevel1examdate>" + RegLevel1ExamDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</reglevel1examdate>");
            xml.Append("<note>" + Note.ToString() + "</note>");
            xml.Append("<fkautomationstatusid>" + FKAutomationStatusID.ToString() + "</fkautomationstatusid>");
            xml.Append("<automationdate>" + AutomationDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</automationdate>");
            xml.Append("</hrstaff>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the HRStaff object from a list of parameters.
        /// </summary>
        /// <param name="employeeno"></param>
        /// <param name="tempstartdate"></param>
        /// <param name="employmentstartdate"></param>
        /// <param name="employmentenddate"></param>
        /// <param name="fkcompanyid"></param>
        /// <param name="fkstafftypeid"></param>
        /// <param name="fkworkstatusid"></param>
        /// <param name="fkjobtitleid"></param>
        /// <param name="fkuserid"></param>
        /// <param name="fkrecruitmentconsultantid"></param>
        /// <param name="recruitmentsource"></param>
        /// <param name="fktitleid"></param>
        /// <param name="initials"></param>
        /// <param name="firstname"></param>
        /// <param name="preferredname"></param>
        /// <param name="surname"></param>
        /// <param name="fkgenderid"></param>
        /// <param name="fkraceid"></param>
        /// <param name="idno"></param>
        /// <param name="dob"></param>
        /// <param name="fkcitizenid"></param>
        /// <param name="fkreligionid"></param>
        /// <param name="telhome"></param>
        /// <param name="telcell"></param>
        /// <param name="telother"></param>
        /// <param name="email"></param>
        /// <param name="registeredtaxpayer"></param>
        /// <param name="taxrefno"></param>
        /// <param name="irp5number"></param>
        /// <param name="fkmedicalaidid"></param>
        /// <param name="criminaloffence"></param>
        /// <param name="employmentdismissed"></param>
        /// <param name="disciplinaryguilty"></param>
        /// <param name="workedfinservicesindustry"></param>
        /// <param name="workedfinservicesindustryfrom"></param>
        /// <param name="workedfinservicesindustryto"></param>
        /// <param name="reglevel1examdate"></param>
        /// <param name="note"></param>
        /// <param name="fkautomationstatusid"></param>
        /// <param name="automationdate"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(string employeeno, DateTime? tempstartdate, DateTime? employmentstartdate, DateTime? employmentenddate, long? fkcompanyid, long? fkstafftypeid, long? fkworkstatusid, long? fkjobtitleid, long? fkuserid, long? fkrecruitmentconsultantid, string recruitmentsource, long? fktitleid, string initials, string firstname, string preferredname, string surname, long? fkgenderid, long? fkraceid, string idno, DateTime? dob, long? fkcitizenid, long? fkreligionid, string telhome, string telcell, string telother, string email, bool? registeredtaxpayer, string taxrefno, string irp5number, long? fkmedicalaidid, bool? criminaloffence, bool? employmentdismissed, bool? disciplinaryguilty, bool? workedfinservicesindustry, DateTime? workedfinservicesindustryfrom, DateTime? workedfinservicesindustryto, DateTime? reglevel1examdate, string note, long? fkautomationstatusid, DateTime? automationdate)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.EmployeeNo = employeeno;
                this.TempStartDate = tempstartdate;
                this.EmploymentStartDate = employmentstartdate;
                this.EmploymentEndDate = employmentenddate;
                this.FKCompanyID = fkcompanyid;
                this.FKStaffTypeID = fkstafftypeid;
                this.FKWorkStatusID = fkworkstatusid;
                this.FKJobTitleID = fkjobtitleid;
                this.FKUserID = fkuserid;
                this.FKRecruitmentConsultantID = fkrecruitmentconsultantid;
                this.RecruitmentSource = recruitmentsource;
                this.FKTitleID = fktitleid;
                this.Initials = initials;
                this.FirstName = firstname;
                this.PreferredName = preferredname;
                this.Surname = surname;
                this.FKGenderID = fkgenderid;
                this.FKRaceID = fkraceid;
                this.IDNo = idno;
                this.DOB = dob;
                this.FKCitizenID = fkcitizenid;
                this.FKReligionID = fkreligionid;
                this.TelHome = telhome;
                this.TelCell = telcell;
                this.TelOther = telother;
                this.EMail = email;
                this.RegisteredTaxPayer = registeredtaxpayer;
                this.TaxRefNo = taxrefno;
                this.IRP5Number = irp5number;
                this.FKMedicalAidID = fkmedicalaidid;
                this.CriminalOffence = criminaloffence;
                this.EmploymentDismissed = employmentdismissed;
                this.DisciplinaryGuilty = disciplinaryguilty;
                this.WorkedFinServicesIndustry = workedfinservicesindustry;
                this.WorkedFinServicesIndustryFrom = workedfinservicesindustryfrom;
                this.WorkedFinServicesIndustryTo = workedfinservicesindustryto;
                this.RegLevel1ExamDate = reglevel1examdate;
                this.Note = note;
                this.FKAutomationStatusID = fkautomationstatusid;
                this.AutomationDate = automationdate;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) HRStaff's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the HRStaff history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return HRStaffMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an HRStaff object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the HRStaff object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return HRStaffMapper.UnDelete(this);
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
    /// A collection of the HRStaff object.
    /// </summary>
    public partial class HRStaffCollection : ObjectCollection<HRStaff>
    { 
    }
    #endregion
}
