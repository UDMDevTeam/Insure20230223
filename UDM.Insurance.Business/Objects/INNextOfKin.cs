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
    public partial class INNextOfKin : ObjectBase<long>
    {
        #region Members
        private long? _fkinimportid = null;
        private long? _fkinrelationshipid = null;
        private string _firstname = null;
        private string _surname = null;
        private string _telcontact = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INNextOfKin class.
        /// </summary>
        public INNextOfKin()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INNextOfKin class.
        /// </summary>
        public INNextOfKin(long id)
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
        /// Fills an INNextOfKin object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INNextOfKinMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INNextOfKin object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INNextOfKin object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INNextOfKinMapper.Save(this);
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
        /// Deletes an INNextOfKin object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INNextOfKin object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INNextOfKinMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INNextOfKin.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<innextofkin>");
            xml.Append("<fkinimportid>" + FKINImportID.ToString() + "</fkinimportid>");
            xml.Append("<fkinrelationshipid>" + FKINRelationshipID.ToString() + "</fkinrelationshipid>");
            xml.Append("<firstname>" + FirstName.ToString() + "</firstname>");
            xml.Append("<surname>" + Surname.ToString() + "</surname>");
            xml.Append("<telcontact>" + TelContact.ToString() + "</telcontact>");
            xml.Append("</innextofkin>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INNextOfKin object from a list of parameters.
        /// </summary>
        /// <param name="fkinimportid"></param>
        /// <param name="fkinrelationshipid"></param>
        /// <param name="firstname"></param>
        /// <param name="surname"></param>
        /// <param name="telcontact"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinimportid, long? fkinrelationshipid, string firstname, string surname, string telcontact)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINImportID = fkinimportid;
                this.FKINRelationshipID = fkinrelationshipid;
                this.FirstName = firstname;
                this.Surname = surname;
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
        /// Deletes a(n) INNextOfKin's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INNextOfKin history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INNextOfKinMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INNextOfKin object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INNextOfKin object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INNextOfKinMapper.UnDelete(this);
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
    /// A collection of the INNextOfKin object.
    /// </summary>
    public partial class INNextOfKinCollection : ObjectCollection<INNextOfKin>
    { 
    }
    #endregion
}
