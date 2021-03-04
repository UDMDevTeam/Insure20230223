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
    public partial class INImportLatentLeadReason : ObjectBase<long>
    {
        #region Members
        private long? _fkinimportid = null;
        private long? _fkinlatentleadreasonid1 = null;
        private long? _fkinlatentleadreasonid2 = null;
        private long? _fkinlatentleadreasonid3 = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INImportLatentLeadReason class.
        /// </summary>
        public INImportLatentLeadReason()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INImportLatentLeadReason class.
        /// </summary>
        public INImportLatentLeadReason(long id)
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

        public long? FKINLatentLeadReasonID1
        {
            get
            {
                Fill();
                return _fkinlatentleadreasonid1;
            }
            set 
            {
                Fill();
                if (value != _fkinlatentleadreasonid1)
                {
                    _fkinlatentleadreasonid1 = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINLatentLeadReasonID2
        {
            get
            {
                Fill();
                return _fkinlatentleadreasonid2;
            }
            set 
            {
                Fill();
                if (value != _fkinlatentleadreasonid2)
                {
                    _fkinlatentleadreasonid2 = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINLatentLeadReasonID3
        {
            get
            {
                Fill();
                return _fkinlatentleadreasonid3;
            }
            set 
            {
                Fill();
                if (value != _fkinlatentleadreasonid3)
                {
                    _fkinlatentleadreasonid3 = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INImportLatentLeadReason object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INImportLatentLeadReasonMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INImportLatentLeadReason object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportLatentLeadReason object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INImportLatentLeadReasonMapper.Save(this);
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
        /// Deletes an INImportLatentLeadReason object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportLatentLeadReason object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INImportLatentLeadReasonMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INImportLatentLeadReason.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inimportlatentleadreason>");
            xml.Append("<fkinimportid>" + FKINImportID.ToString() + "</fkinimportid>");
            xml.Append("<fkinlatentleadreasonid1>" + FKINLatentLeadReasonID1.ToString() + "</fkinlatentleadreasonid1>");
            xml.Append("<fkinlatentleadreasonid2>" + FKINLatentLeadReasonID2.ToString() + "</fkinlatentleadreasonid2>");
            xml.Append("<fkinlatentleadreasonid3>" + FKINLatentLeadReasonID3.ToString() + "</fkinlatentleadreasonid3>");
            xml.Append("</inimportlatentleadreason>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INImportLatentLeadReason object from a list of parameters.
        /// </summary>
        /// <param name="fkinimportid"></param>
        /// <param name="fkinlatentleadreasonid1"></param>
        /// <param name="fkinlatentleadreasonid2"></param>
        /// <param name="fkinlatentleadreasonid3"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinimportid, long? fkinlatentleadreasonid1, long? fkinlatentleadreasonid2, long? fkinlatentleadreasonid3)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINImportID = fkinimportid;
                this.FKINLatentLeadReasonID1 = fkinlatentleadreasonid1;
                this.FKINLatentLeadReasonID2 = fkinlatentleadreasonid2;
                this.FKINLatentLeadReasonID3 = fkinlatentleadreasonid3;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INImportLatentLeadReason's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportLatentLeadReason history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportLatentLeadReasonMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INImportLatentLeadReason object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportLatentLeadReason object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportLatentLeadReasonMapper.UnDelete(this);
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
    /// A collection of the INImportLatentLeadReason object.
    /// </summary>
    public partial class INImportLatentLeadReasonCollection : ObjectCollection<INImportLatentLeadReason>
    { 
    }
    #endregion
}
