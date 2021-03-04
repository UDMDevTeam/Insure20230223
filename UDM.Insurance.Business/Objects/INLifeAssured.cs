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
    public partial class INLifeAssured : ObjectBase<long>
    {
        #region Members
        private string _idno = null;
        private long? _fkintitleid = null;
        private string _firstname = null;
        private string _surname = null;
        private long? _fkgenderid = null;
        private DateTime? _dateofbirth = null;
        private long? _fkinrelationshipid = null;
        private long? _todeleteid = null;
        private string _telcontact = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INLifeAssured class.
        /// </summary>
        public INLifeAssured()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INLifeAssured class.
        /// </summary>
        public INLifeAssured(long id)
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

        public long? FKINRelationshipID
        {
            get
            {
                Fill();
                return _fkinrelationshipid;
            }
            set 
            {
                Fill();
                if (value != _fkinrelationshipid)
                {
                    _fkinrelationshipid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? ToDeleteID
        {
            get
            {
                Fill();
                return _todeleteid;
            }
            set 
            {
                Fill();
                if (value != _todeleteid)
                {
                    _todeleteid = value;
                    _hasChanged = true;
                }
            }
        }

        public string TelContact
        {
            get
            {
                Fill();
                return _telcontact;
            }
            set 
            {
                Fill();
                if (value != _telcontact)
                {
                    _telcontact = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INLifeAssured object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INLifeAssuredMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INLifeAssured object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLifeAssured object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INLifeAssuredMapper.Save(this);
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
        /// Deletes an INLifeAssured object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLifeAssured object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INLifeAssuredMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INLifeAssured.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inlifeassured>");
            xml.Append("<idno>" + IDNo.ToString() + "</idno>");
            xml.Append("<fkintitleid>" + FKINTitleID.ToString() + "</fkintitleid>");
            xml.Append("<firstname>" + FirstName.ToString() + "</firstname>");
            xml.Append("<surname>" + Surname.ToString() + "</surname>");
            xml.Append("<fkgenderid>" + FKGenderID.ToString() + "</fkgenderid>");
            xml.Append("<dateofbirth>" + DateOfBirth.Value.ToString("dd MMM yyyy HH:mm:ss") + "</dateofbirth>");
            xml.Append("<fkinrelationshipid>" + FKINRelationshipID.ToString() + "</fkinrelationshipid>");
            xml.Append("<todeleteid>" + ToDeleteID.ToString() + "</todeleteid>");
            xml.Append("<telcontact>" + TelContact.ToString() + "</telcontact>");
            xml.Append("</inlifeassured>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INLifeAssured object from a list of parameters.
        /// </summary>
        /// <param name="idno"></param>
        /// <param name="fkintitleid"></param>
        /// <param name="firstname"></param>
        /// <param name="surname"></param>
        /// <param name="fkgenderid"></param>
        /// <param name="dateofbirth"></param>
        /// <param name="fkinrelationshipid"></param>
        /// <param name="todeleteid"></param>
        /// <param name="telcontact"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(string idno, long? fkintitleid, string firstname, string surname, long? fkgenderid, DateTime? dateofbirth, long? fkinrelationshipid, long? todeleteid, string telcontact)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.IDNo = idno;
                this.FKINTitleID = fkintitleid;
                this.FirstName = firstname;
                this.Surname = surname;
                this.FKGenderID = fkgenderid;
                this.DateOfBirth = dateofbirth;
                this.FKINRelationshipID = fkinrelationshipid;
                this.ToDeleteID = todeleteid;
                this.TelContact = telcontact;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INLifeAssured's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLifeAssured history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INLifeAssuredMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INLifeAssured object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLifeAssured object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INLifeAssuredMapper.UnDelete(this);
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
    /// A collection of the INLifeAssured object.
    /// </summary>
    public partial class INLifeAssuredCollection : ObjectCollection<INLifeAssured>
    { 
    }
    #endregion
}
