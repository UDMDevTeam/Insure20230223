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
    public partial class INLeadBookImport : ObjectBase<long>
    {
        #region Members
        private long? _fkinleadbookid = null;
        private long? _fkinimportid = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INLeadBookImport class.
        /// </summary>
        public INLeadBookImport()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INLeadBookImport class.
        /// </summary>
        public INLeadBookImport(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKINLeadBookID
        {
            get
            {
                Fill();
                return _fkinleadbookid;
            }
            set 
            {
                Fill();
                if (value != _fkinleadbookid)
                {
                    _fkinleadbookid = value;
                    _hasChanged = true;
                }
            }
        }

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
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INLeadBookImport object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INLeadBookImportMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INLeadBookImport object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLeadBookImport object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INLeadBookImportMapper.Save(this);
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
        /// Deletes an INLeadBookImport object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLeadBookImport object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INLeadBookImportMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INLeadBookImport.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inleadbookimport>");
            xml.Append("<fkinleadbookid>" + FKINLeadBookID.ToString() + "</fkinleadbookid>");
            xml.Append("<fkinimportid>" + FKINImportID.ToString() + "</fkinimportid>");
            xml.Append("</inleadbookimport>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INLeadBookImport object from a list of parameters.
        /// </summary>
        /// <param name="fkinleadbookid"></param>
        /// <param name="fkinimportid"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinleadbookid, long? fkinimportid)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINLeadBookID = fkinleadbookid;
                this.FKINImportID = fkinimportid;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INLeadBookImport's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLeadBookImport history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INLeadBookImportMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INLeadBookImport object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INLeadBookImport object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INLeadBookImportMapper.UnDelete(this);
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
    /// A collection of the INLeadBookImport object.
    /// </summary>
    public partial class INLeadBookImportCollection : ObjectCollection<INLeadBookImport>
    { 
    }
    #endregion
}
