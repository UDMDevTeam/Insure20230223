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
    public partial class INQADetailsAnswerType : ObjectBase<long>
    {
        #region Members
        private string _description = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INQADetailsAnswerType class.
        /// </summary>
        public INQADetailsAnswerType()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INQADetailsAnswerType class.
        /// </summary>
        public INQADetailsAnswerType(long id)
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
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INQADetailsAnswerType object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INQADetailsAnswerTypeMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INQADetailsAnswerType object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsAnswerType object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INQADetailsAnswerTypeMapper.Save(this);
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
        /// Deletes an INQADetailsAnswerType object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsAnswerType object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INQADetailsAnswerTypeMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INQADetailsAnswerType.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inqadetailsanswertype>");
            xml.Append("<description>" + Description.ToString() + "</description>");
            xml.Append("</inqadetailsanswertype>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INQADetailsAnswerType object from a list of parameters.
        /// </summary>
        /// <param name="description"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(string description)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.Description = description;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INQADetailsAnswerType's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsAnswerType history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INQADetailsAnswerTypeMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INQADetailsAnswerType object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsAnswerType object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INQADetailsAnswerTypeMapper.UnDelete(this);
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
    /// A collection of the INQADetailsAnswerType object.
    /// </summary>
    public partial class INQADetailsAnswerTypeCollection : ObjectCollection<INQADetailsAnswerType>
    { 
    }
    #endregion
}
