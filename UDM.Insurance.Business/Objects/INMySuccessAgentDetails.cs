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
    public partial class INMySuccessAgentDetails : ObjectBase<long>
    {
        #region Members
        private long? _id = null;
        private long? _fkcampaignid = null;
        private long? _documentid = null;
        private byte[] _document = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the Closure class.
        /// </summary>
        public INMySuccessAgentDetails()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the Closure class.
        /// </summary>
        public INMySuccessAgentDetails(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties

        //public new long? ID
        //{
        //    get
        //    {
        //        Fill();
        //        return _id;
        //    }
        //    set
        //    {
        //        Fill();
        //        if (value != _id)
        //        {
        //            _id = value;
        //            _hasChanged = true;
        //        }
        //    }
        //}

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

        public long? DocumentID
        {
            get
            {
                Fill();
                return _documentid;
            }
            set
            {
                Fill();
                if (value != _documentid)
                {
                    _documentid = value;
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
                INMySuccessAgentDetailsMapper.Fill(this);
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
                    _isLoaded = INMySuccessAgentDetailsMapper.Save(this);
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
                return BeforeDelete(validationResult) && INMySuccessAgentDetailsMapper.Delete(this);
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
            xml.Append("<inmysuccesscampaigndetails>");
            xml.Append("<fkcampaignid>" + FKCampaignID.ToString() + "</fkcampaignid>");
            xml.Append("<documentid>" + DocumentID.ToString() + "</documentid>");
            xml.Append("<document>" + Document.ToString() + "</document>");
            xml.Append("</inmysuccessagentdetails>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the Closure object from a list of parameters.
        /// </summary>
        /// <param name="fkcampaignid"></param>
        /// <param name="documentid"></param>
        /// <param name="document"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkcampaignid, long? _documentid, byte[] document)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKCampaignID = fkcampaignid;
                this.DocumentID = _documentid;
                this.Document = document;
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
                return INMySuccessAgentDetailsMapper.DeleteHistory(this);
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
                return INMySuccessAgentDetailsMapper.UnDelete(this);
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
    public partial class INMySuccessAgentDetailsCollection : ObjectCollection<INMySuccessAgentDetails>
    {
    }
    #endregion
}
