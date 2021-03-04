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
    public partial class INChild : ObjectBase<long>
    {
        #region Members
        private string _firstname = null;
        private DateTime? _dateofbirth = null;
        private long? _todeleteid = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INChild class.
        /// </summary>
        public INChild()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INChild class.
        /// </summary>
        public INChild(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
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
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INChild object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INChildMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INChild object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INChild object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INChildMapper.Save(this);
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
        /// Deletes an INChild object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INChild object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INChildMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INChild.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inchild>");
            xml.Append("<firstname>" + FirstName.ToString() + "</firstname>");
            xml.Append("<dateofbirth>" + DateOfBirth.Value.ToString("dd MMM yyyy HH:mm:ss") + "</dateofbirth>");
            xml.Append("<todeleteid>" + ToDeleteID.ToString() + "</todeleteid>");
            xml.Append("</inchild>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INChild object from a list of parameters.
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="dateofbirth"></param>
        /// <param name="todeleteid"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(string firstname, DateTime? dateofbirth, long? todeleteid)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FirstName = firstname;
                this.DateOfBirth = dateofbirth;
                this.ToDeleteID = todeleteid;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INChild's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INChild history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INChildMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INChild object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INChild object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INChildMapper.UnDelete(this);
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
    /// A collection of the INChild object.
    /// </summary>
    public partial class INChildCollection : ObjectCollection<INChild>
    { 
    }
    #endregion
}
