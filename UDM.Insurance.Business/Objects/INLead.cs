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
    public partial class INLead : ObjectBase<long>
    {
        #region Members
        private string _idno = null;
        private string _passportno = null;
        private long? _fkintitleid = null;
        private string _initials = null;
        private string _firstname = null;
        private string _surname = null;
        private long? _fklanguageid = null;
        private long? _fkgenderid = null;
        private DateTime? _dateofbirth = null;
        private string _yearofbirth = null;
        private string _telwork = null;
        private string _telhome = null;
        private string _telcell = null;
        private string _telother = null;
        private string _address = null;
        private string _address1 = null;
        private string _address2 = null;
        private string _address3 = null;
        private string _address4 = null;
        private string _address5 = null;
        private string _postalcode = null;
        private string _email = null;
        private string _occupation = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INLead class.
        /// </summary>
        public INLead()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INLead class.
        /// </summary>
        public INLead(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
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

        public string PassportNo
        {
            get
            {
                Fill();
                return _passportno;
            }
            set 
            {
                Fill();
                if (value != _passportno)
                {
                    _passportno = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINTitleID
        {
            get
            {
                Fill();
                return _fkintitleid;
            }
            set 
            {
                Fill();
                if (value != _fkintitleid)
                {
                    _fkintitleid = value;
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

        public long? FKLanguageID
        {
            get
            {
                Fill();
                return _fklanguageid;
            }
            set 
            {
                Fill();
                if (value != _fklanguageid)
                {
                    _fklanguageid = value;
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

        public DateTime? DateOfBirth
        {
            get
            {
                Fill();
                return _dateofbirth;
            }
            set 
            {
                Fill();
                if (value != _dateofbirth)
                {
                    _dateofbirth = value;
                    _hasChanged = true;
                }
            }
        }

        public string YearOfBirth
        {
            get
            {
                Fill();
                return _yearofbirth;
            }
            set 
            {
                Fill();
                if (value != _yearofbirth)
                {
                    _yearofbirth = value;
                    _hasChanged = true;
                }
            }
        }

        public string TelWork
        {
            get
            {
                Fill();
                return _telwork;
            }
            set 
            {
                Fill();
                if (value != _telwork)
                {
                    _telwork = value;
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

        public string Address
        {
            get
            {
                Fill();
                return _address;
            }
            set 
            {
                Fill();
                if (value != _address)
                {
                    _address = value;
                    _hasChanged = true;
                }
            }
        }

        public string Address1
        {
            get
            {
                Fill();
                return _address1;
            }
            set 
            {
                Fill();
                if (value != _address1)
                {
                    _address1 = value;
                    _hasChanged = true;
                }
            }
        }

        public string Address2
        {
            get
            {
                Fill();
                return _address2;
            }
            set 
            {
                Fill();
                if (value != _address2)
                {
                    _address2 = value;
                    _hasChanged = true;
                }
            }
        }

        public string Address3
        {
            get
            {
                Fill();
                return _address3;
            }
            set 
            {
                Fill();
                if (value != _address3)
                {
                    _address3 = value;
                    _hasChanged = true;
                }
            }
        }

        public string Address4
        {
            get
            {
                Fill();
                return _address4;
            }
            set 
            {
                Fill();
                if (value != _address4)
                {
                    _address4 = value;
                    _hasChanged = true;
                }
            }
        }

        public string Address5
        {
            get
            {
                Fill();
                return _address5;
            }
            set 
            {
                Fill();
                if (value != _address5)
                {
                    _address5 = value;
                    _hasChanged = true;
                }
            }
        }

        public string PostalCode
        {
            get
            {
                Fill();
                return _postalcode;
            }
            set 
            {
                Fill();
                if (value != _postalcode)
                {
                    _postalcode = value;
                    _hasChanged = true;
                }
            }
        }

        public string Email
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

        public string Occupation
        {
            get
            {
                Fill();
                return _occupation;
            }
            set 
            {
                Fill();
                if (value != _occupation)
                {
                    _occupation = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INLead object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INLeadMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INLead object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLead object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INLeadMapper.Save(this);
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
        /// Deletes an INLead object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLead object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INLeadMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INLead.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inlead>");
            xml.Append("<idno>" + IDNo.ToString() + "</idno>");
            xml.Append("<passportno>" + PassportNo.ToString() + "</passportno>");
            xml.Append("<fkintitleid>" + FKINTitleID.ToString() + "</fkintitleid>");
            xml.Append("<initials>" + Initials.ToString() + "</initials>");
            xml.Append("<firstname>" + FirstName.ToString() + "</firstname>");
            xml.Append("<surname>" + Surname.ToString() + "</surname>");
            xml.Append("<fklanguageid>" + FKLanguageID.ToString() + "</fklanguageid>");
            xml.Append("<fkgenderid>" + FKGenderID.ToString() + "</fkgenderid>");
            xml.Append("<dateofbirth>" + DateOfBirth.Value.ToString("dd MMM yyyy HH:mm:ss") + "</dateofbirth>");
            xml.Append("<yearofbirth>" + YearOfBirth.ToString() + "</yearofbirth>");
            xml.Append("<telwork>" + TelWork.ToString() + "</telwork>");
            xml.Append("<telhome>" + TelHome.ToString() + "</telhome>");
            xml.Append("<telcell>" + TelCell.ToString() + "</telcell>");
            xml.Append("<telother>" + TelOther.ToString() + "</telother>");
            xml.Append("<address>" + Address.ToString() + "</address>");
            xml.Append("<address1>" + Address1.ToString() + "</address1>");
            xml.Append("<address2>" + Address2.ToString() + "</address2>");
            xml.Append("<address3>" + Address3.ToString() + "</address3>");
            xml.Append("<address4>" + Address4.ToString() + "</address4>");
            xml.Append("<address5>" + Address5.ToString() + "</address5>");
            xml.Append("<postalcode>" + PostalCode.ToString() + "</postalcode>");
            xml.Append("<email>" + Email.ToString() + "</email>");
            xml.Append("<occupation>" + Occupation.ToString() + "</occupation>");
            xml.Append("</inlead>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INLead object from a list of parameters.
        /// </summary>
        /// <param name="idno"></param>
        /// <param name="passportno"></param>
        /// <param name="fkintitleid"></param>
        /// <param name="initials"></param>
        /// <param name="firstname"></param>
        /// <param name="surname"></param>
        /// <param name="fklanguageid"></param>
        /// <param name="fkgenderid"></param>
        /// <param name="dateofbirth"></param>
        /// <param name="yearofbirth"></param>
        /// <param name="telwork"></param>
        /// <param name="telhome"></param>
        /// <param name="telcell"></param>
        /// <param name="telother"></param>
        /// <param name="address"></param>
        /// <param name="address1"></param>
        /// <param name="address2"></param>
        /// <param name="address3"></param>
        /// <param name="address4"></param>
        /// <param name="address5"></param>
        /// <param name="postalcode"></param>
        /// <param name="email"></param>
        /// <param name="occupation"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(string idno, string passportno, long? fkintitleid, string initials, string firstname, string surname, long? fklanguageid, long? fkgenderid, DateTime? dateofbirth, string yearofbirth, string telwork, string telhome, string telcell, string telother, string address, string address1, string address2, string address3, string address4, string address5, string postalcode, string email, string occupation)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.IDNo = idno;
                this.PassportNo = passportno;
                this.FKINTitleID = fkintitleid;
                this.Initials = initials;
                this.FirstName = firstname;
                this.Surname = surname;
                this.FKLanguageID = fklanguageid;
                this.FKGenderID = fkgenderid;
                this.DateOfBirth = dateofbirth;
                this.YearOfBirth = yearofbirth;
                this.TelWork = telwork;
                this.TelHome = telhome;
                this.TelCell = telcell;
                this.TelOther = telother;
                this.Address = address;
                this.Address1 = address1;
                this.Address2 = address2;
                this.Address3 = address3;
                this.Address4 = address4;
                this.Address5 = address5;
                this.PostalCode = postalcode;
                this.Email = email;
                this.Occupation = occupation;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INLead's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLead history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INLeadMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INLead object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLead object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INLeadMapper.UnDelete(this);
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
    /// A collection of the INLead object.
    /// </summary>
    public partial class INLeadCollection : ObjectCollection<INLead>
    { 
    }
    #endregion
}
