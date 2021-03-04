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
    public partial class INBatch : ObjectBase<long>
    {
        #region Members
        private long? _fkincampaignid = null;
        private string _code = null;
        private string _udmcode = null;
        private int? _newleads = null;
        private int? _updatedleads = null;
        private bool? _containslatentleads = null;
        private bool? _completed = null;
        private DateTime? _datereceived = null;
        private bool? _isarchived = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INBatch class.
        /// </summary>
        public INBatch()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INBatch class.
        /// </summary>
        public INBatch(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKINCampaignID
        {
            get
            {
                Fill();
                return _fkincampaignid;
            }
            set 
            {
                Fill();
                if (value != _fkincampaignid)
                {
                    _fkincampaignid = value;
                    _hasChanged = true;
                }
            }
        }

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

        public string UDMCode
        {
            get
            {
                Fill();
                return _udmcode;
            }
            set 
            {
                Fill();
                if (value != _udmcode)
                {
                    _udmcode = value;
                    _hasChanged = true;
                }
            }
        }

        public int? NewLeads
        {
            get
            {
                Fill();
                return _newleads;
            }
            set 
            {
                Fill();
                if (value != _newleads)
                {
                    _newleads = value;
                    _hasChanged = true;
                }
            }
        }

        public int? UpdatedLeads
        {
            get
            {
                Fill();
                return _updatedleads;
            }
            set 
            {
                Fill();
                if (value != _updatedleads)
                {
                    _updatedleads = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? ContainsLatentLeads
        {
            get
            {
                Fill();
                return _containslatentleads;
            }
            set 
            {
                Fill();
                if (value != _containslatentleads)
                {
                    _containslatentleads = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? Completed
        {
            get
            {
                Fill();
                return _completed;
            }
            set 
            {
                Fill();
                if (value != _completed)
                {
                    _completed = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? DateReceived
        {
            get
            {
                Fill();
                return _datereceived;
            }
            set 
            {
                Fill();
                if (value != _datereceived)
                {
                    _datereceived = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsArchived
        {
            get
            {
                Fill();
                return _isarchived;
            }
            set 
            {
                Fill();
                if (value != _isarchived)
                {
                    _isarchived = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INBatch object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INBatchMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INBatch object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INBatch object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INBatchMapper.Save(this);
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
        /// Deletes an INBatch object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INBatch object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INBatchMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INBatch.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inbatch>");
            xml.Append("<fkincampaignid>" + FKINCampaignID.ToString() + "</fkincampaignid>");
            xml.Append("<code>" + Code.ToString() + "</code>");
            xml.Append("<udmcode>" + UDMCode.ToString() + "</udmcode>");
            xml.Append("<newleads>" + NewLeads.ToString() + "</newleads>");
            xml.Append("<updatedleads>" + UpdatedLeads.ToString() + "</updatedleads>");
            xml.Append("<containslatentleads>" + ContainsLatentLeads.ToString() + "</containslatentleads>");
            xml.Append("<completed>" + Completed.ToString() + "</completed>");
            xml.Append("<datereceived>" + DateReceived.Value.ToString("dd MMM yyyy HH:mm:ss") + "</datereceived>");
            xml.Append("<isarchived>" + IsArchived.ToString() + "</isarchived>");
            xml.Append("</inbatch>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INBatch object from a list of parameters.
        /// </summary>
        /// <param name="fkincampaignid"></param>
        /// <param name="code"></param>
        /// <param name="udmcode"></param>
        /// <param name="newleads"></param>
        /// <param name="updatedleads"></param>
        /// <param name="containslatentleads"></param>
        /// <param name="completed"></param>
        /// <param name="datereceived"></param>
        /// <param name="isarchived"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkincampaignid, string code, string udmcode, int? newleads, int? updatedleads, bool? containslatentleads, bool? completed, DateTime? datereceived, bool? isarchived)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINCampaignID = fkincampaignid;
                this.Code = code;
                this.UDMCode = udmcode;
                this.NewLeads = newleads;
                this.UpdatedLeads = updatedleads;
                this.ContainsLatentLeads = containslatentleads;
                this.Completed = completed;
                this.DateReceived = datereceived;
                this.IsArchived = isarchived;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INBatch's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INBatch history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INBatchMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INBatch object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INBatch object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INBatchMapper.UnDelete(this);
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
    /// A collection of the INBatch object.
    /// </summary>
    public partial class INBatchCollection : ObjectCollection<INBatch>
    { 
    }
    #endregion
}
