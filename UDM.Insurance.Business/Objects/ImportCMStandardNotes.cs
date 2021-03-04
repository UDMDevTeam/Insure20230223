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
    public partial class ImportCMStandardNotes : ObjectBase<long>
    {
        #region Members
        private long? _fkcallmonitoringstandardnotesid = null;
        private long? _fkimportcallmonitoringid = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the ImportCMStandardNotes class.
        /// </summary>
        public ImportCMStandardNotes()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the ImportCMStandardNotes class.
        /// </summary>
        public ImportCMStandardNotes(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKCallMonitoringStandardNotesID
        {
            get
            {
                Fill();
                return _fkcallmonitoringstandardnotesid;
            }
            set 
            {
                Fill();
                if (value != _fkcallmonitoringstandardnotesid)
                {
                    _fkcallmonitoringstandardnotesid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKImportCallMonitoringID
        {
            get
            {
                Fill();
                return _fkimportcallmonitoringid;
            }
            set 
            {
                Fill();
                if (value != _fkimportcallmonitoringid)
                {
                    _fkimportcallmonitoringid = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an ImportCMStandardNotes object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                ImportCMStandardNotesMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an ImportCMStandardNotes object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the ImportCMStandardNotes object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = ImportCMStandardNotesMapper.Save(this);
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
        /// Deletes an ImportCMStandardNotes object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the ImportCMStandardNotes object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && ImportCMStandardNotesMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the ImportCMStandardNotes.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<importcmstandardnotes>");
            xml.Append("<fkcallmonitoringstandardnotesid>" + FKCallMonitoringStandardNotesID.ToString() + "</fkcallmonitoringstandardnotesid>");
            xml.Append("<fkimportcallmonitoringid>" + FKImportCallMonitoringID.ToString() + "</fkimportcallmonitoringid>");
            xml.Append("</importcmstandardnotes>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the ImportCMStandardNotes object from a list of parameters.
        /// </summary>
        /// <param name="fkcallmonitoringstandardnotesid"></param>
        /// <param name="fkimportcallmonitoringid"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkcallmonitoringstandardnotesid, long? fkimportcallmonitoringid)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKCallMonitoringStandardNotesID = fkcallmonitoringstandardnotesid;
                this.FKImportCallMonitoringID = fkimportcallmonitoringid;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) ImportCMStandardNotes's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the ImportCMStandardNotes history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return ImportCMStandardNotesMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an ImportCMStandardNotes object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the ImportCMStandardNotes object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return ImportCMStandardNotesMapper.UnDelete(this);
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
    /// A collection of the ImportCMStandardNotes object.
    /// </summary>
    public partial class ImportCMStandardNotesCollection : ObjectCollection<ImportCMStandardNotes>
    { 
    }
    #endregion
}
