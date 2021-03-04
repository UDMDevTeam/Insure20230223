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
    public partial class Closure : ObjectBase<long>
    {
        #region Members
        private long? _fksystemid = null;
        private long? _fkcampaignid = null;
        private long? _fklanguageid = null;
        private byte[] _document = null;
        private bool? _isactive = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the Closure class.
        /// </summary>
        public Closure()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the Closure class.
        /// </summary>
        public Closure(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKSystemID
        {
            get
            {
                Fill();
                return _fksystemid;
            }
            set 
            {
                Fill();
                if (value != _fksystemid)
                {
                    _fksystemid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKCampaignID
        {
            get
            {
                Fill();
                return _fkcampaignid;
            }
            set 
            {
                Fill();
                if (value != _fkcampaignid)
                {
                    _fkcampaignid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKLanguageID
        {
            get
            {
                Fill();
                return _fklanguageid;
            }
            set 
            {
                Fill();
                if (value != _fklanguageid)
                {
                    _fklanguageid = value;
                    _hasChanged = true;
                }
            }
        }

        public byte[] Document
        {
            get
            {
                Fill();
                return _document;
            }
            set 
            {
                Fill();
                if (value != _document)
                {
                    _document = value;
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
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an Closure object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                ClosureMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an Closure object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the Closure object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = ClosureMapper.Save(this);
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
        /// Deletes an Closure object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the Closure object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && ClosureMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the Closure.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<closure>");
            xml.Append("<fksystemid>" + FKSystemID.ToString() + "</fksystemid>");
            xml.Append("<fkcampaignid>" + FKCampaignID.ToString() + "</fkcampaignid>");
            xml.Append("<fklanguageid>" + FKLanguageID.ToString() + "</fklanguageid>");
            xml.Append("<document>" + Document.ToString() + "</document>");
            xml.Append("<isactive>" + IsActive.ToString() + "</isactive>");
            xml.Append("</closure>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the Closure object from a list of parameters.
        /// </summary>
        /// <param name="fksystemid"></param>
        /// <param name="fkcampaignid"></param>
        /// <param name="fklanguageid"></param>
        /// <param name="document"></param>
        /// <param name="isactive"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fksystemid, long? fkcampaignid, long? fklanguageid, byte[] document, bool? isactive)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKSystemID = fksystemid;
                this.FKCampaignID = fkcampaignid;
                this.FKLanguageID = fklanguageid;
                this.Document = document;
                this.IsActive = isactive;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) Closure's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the Closure history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return ClosureMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an Closure object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the Closure object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return ClosureMapper.UnDelete(this);
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
    /// A collection of the Closure object.
    /// </summary>
    public partial class ClosureCollection : ObjectCollection<Closure>
    { 
    }
    #endregion
}
