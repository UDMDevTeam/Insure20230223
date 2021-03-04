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
    public partial class INQADetailsINImport : ObjectBase<long>
    {
        #region Members
        private long? _fkimportid = null;
        private long? _fkassessorid = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INQADetailsINImport class.
        /// </summary>
        public INQADetailsINImport()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INQADetailsINImport class.
        /// </summary>
        public INQADetailsINImport(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKImportID
        {
            get
            {
                Fill();
                return _fkimportid;
            }
            set 
            {
                Fill();
                if (value != _fkimportid)
                {
                    _fkimportid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKAssessorID
        {
            get
            {
                Fill();
                return _fkassessorid;
            }
            set 
            {
                Fill();
                if (value != _fkassessorid)
                {
                    _fkassessorid = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INQADetailsINImport object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INQADetailsINImportMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INQADetailsINImport object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsINImport object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INQADetailsINImportMapper.Save(this);
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
        /// Deletes an INQADetailsINImport object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsINImport object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INQADetailsINImportMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INQADetailsINImport.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inqadetailsinimport>");
            xml.Append("<fkimportid>" + FKImportID.ToString() + "</fkimportid>");
            xml.Append("<fkassessorid>" + FKAssessorID.ToString() + "</fkassessorid>");
            xml.Append("</inqadetailsinimport>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INQADetailsINImport object from a list of parameters.
        /// </summary>
        /// <param name="fkimportid"></param>
        /// <param name="fkassessorid"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkimportid, long? fkassessorid)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKImportID = fkimportid;
                this.FKAssessorID = fkassessorid;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INQADetailsINImport's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsINImport history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INQADetailsINImportMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INQADetailsINImport object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INQADetailsINImport object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INQADetailsINImportMapper.UnDelete(this);
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
    /// A collection of the INQADetailsINImport object.
    /// </summary>
    public partial class INQADetailsINImportCollection : ObjectCollection<INQADetailsINImport>
    { 
    }
    #endregion
}
