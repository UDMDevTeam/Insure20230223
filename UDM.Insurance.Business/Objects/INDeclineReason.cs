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
    public partial class INDeclineReason : ObjectBase<long>
    {
        #region Members
        private string _code = null;
        private string _description = null;
        private string _codenumber = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INDeclineReason class.
        /// </summary>
        public INDeclineReason()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INDeclineReason class.
        /// </summary>
        public INDeclineReason(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public string Code
        {
            get
            {
                Fill();
                return _code;
            }
            set 
            {
                Fill();
                if (value != _code)
                {
                    _code = value;
                    _hasChanged = true;
                }
            }
        }

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
        /// Fills an INDeclineReason object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INDeclineReasonMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INDeclineReason object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INDeclineReason object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INDeclineReasonMapper.Save(this);
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
        /// Deletes an INDeclineReason object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INDeclineReason object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INDeclineReasonMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INDeclineReason.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<indeclinereason>");
            xml.Append("<code>" + Code.ToString() + "</code>");
            xml.Append("<description>" + Description.ToString() + "</description>");
            xml.Append("<codenumber>" + CodeNumber.ToString() + "</codenumber>");
            xml.Append("</indeclinereason>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INDeclineReason object from a list of parameters.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="description"></param>
        /// <param name="codenumber"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(string code, string description, string codenumber)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.Code = code;
                this.Description = description;
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
        /// Deletes a(n) INDeclineReason's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INDeclineReason history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INDeclineReasonMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INDeclineReason object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INDeclineReason object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INDeclineReasonMapper.UnDelete(this);
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
    /// A collection of the INDeclineReason object.
    /// </summary>
    public partial class INDeclineReasonCollection : ObjectCollection<INDeclineReason>
    { 
    }
    #endregion
}
