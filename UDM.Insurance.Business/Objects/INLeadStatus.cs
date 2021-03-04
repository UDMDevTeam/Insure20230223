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
    public partial class INLeadStatus : ObjectBase<long>
    {
        #region Members
        private string _description = null;
        private bool? _isactive = null;
        private string _codenumber = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INLeadStatus class.
        /// </summary>
        public INLeadStatus()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INLeadStatus class.
        /// </summary>
        public INLeadStatus(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public string Description
        {
            get
            {
                Fill();
                return _description;
            }
            set 
            {
                Fill();
                if (value != _description)
                {
                    _description = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsActive
        {
            get
            {
                Fill();
                return _isactive;
            }
            set 
            {
                Fill();
                if (value != _isactive)
                {
                    _isactive = value;
                    _hasChanged = true;
                }
            }
        }

        public string CodeNumber
        {
            get
            {
                Fill();
                return _codenumber;
            }
            set 
            {
                Fill();
                if (value != _codenumber)
                {
                    _codenumber = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INLeadStatus object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INLeadStatusMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INLeadStatus object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLeadStatus object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INLeadStatusMapper.Save(this);
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
        /// Deletes an INLeadStatus object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLeadStatus object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INLeadStatusMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INLeadStatus.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inleadstatus>");
            xml.Append("<description>" + Description.ToString() + "</description>");
            xml.Append("<isactive>" + IsActive.ToString() + "</isactive>");
            xml.Append("<codenumber>" + CodeNumber.ToString() + "</codenumber>");
            xml.Append("</inleadstatus>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INLeadStatus object from a list of parameters.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="isactive"></param>
        /// <param name="codenumber"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(string description, bool? isactive, string codenumber)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.Description = description;
                this.IsActive = isactive;
                this.CodeNumber = codenumber;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INLeadStatus's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLeadStatus history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INLeadStatusMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INLeadStatus object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLeadStatus object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INLeadStatusMapper.UnDelete(this);
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
    /// A collection of the INLeadStatus object.
    /// </summary>
    public partial class INLeadStatusCollection : ObjectCollection<INLeadStatus>
    { 
    }
    #endregion
}
