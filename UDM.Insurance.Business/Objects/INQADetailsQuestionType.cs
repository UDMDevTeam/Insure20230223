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
    public partial class INQADetailsQuestionType : ObjectBase<long>
    {
        #region Members
        private string _description = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INQADetailsQuestionType class.
        /// </summary>
        public INQADetailsQuestionType()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INQADetailsQuestionType class.
        /// </summary>
        public INQADetailsQuestionType(long id)
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
        /// Fills an INQADetailsQuestionType object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INQADetailsQuestionTypeMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INQADetailsQuestionType object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsQuestionType object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INQADetailsQuestionTypeMapper.Save(this);
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
        /// Deletes an INQADetailsQuestionType object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsQuestionType object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INQADetailsQuestionTypeMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INQADetailsQuestionType.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inqadetailsquestiontype>");
            xml.Append("<description>" + Description.ToString() + "</description>");
            xml.Append("</inqadetailsquestiontype>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INQADetailsQuestionType object from a list of parameters.
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
        /// Deletes a(n) INQADetailsQuestionType's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsQuestionType history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INQADetailsQuestionTypeMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INQADetailsQuestionType object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsQuestionType object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INQADetailsQuestionTypeMapper.UnDelete(this);
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
    /// A collection of the INQADetailsQuestionType object.
    /// </summary>
    public partial class INQADetailsQuestionTypeCollection : ObjectCollection<INQADetailsQuestionType>
    { 
    }
    #endregion
}
