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
    public partial class ConservedCampaigns : ObjectBase<long>
    {
        #region Members
        private long? _fkincampaignid = null;
        private long? _fkinbatchid = null;
        private int? _amountofleads = null;
        private int? _amountconserved = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the CallData class.
        /// </summary>
        public ConservedCampaigns()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the CallData class.
        /// </summary>
        public ConservedCampaigns(long id)
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

        public long? FKINBatchID
        {
            get
            {
                Fill();
                return _fkinbatchid;
            }
            set 
            {
                Fill();
                if (value != _fkinbatchid)
                {
                    _fkinbatchid = value;
                    _hasChanged = true;
                }
            }
        }

        public int? AmountOfLeads
        {
            get
            {
                Fill();
                return _amountofleads;
            }
            set 
            {
                Fill();
                if (value != _amountofleads)
                {
                    _amountofleads = value;
                    _hasChanged = true;
                }
            }
        }

        public int? AmountConserved
        {
            get
            {
                Fill();
                return _amountconserved;
            }
            set 
            {
                Fill();
                if (value != _amountconserved)
                {
                    _amountconserved = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an CallData object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                ConservedCampaignsMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an CallData object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CallData object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = ConservedCampaignsMapper.Save(this);
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
        /// Deletes an CallData object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CallData object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && ConservedCampaignsMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the CallData.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<ConservedCampaigns>");
            xml.Append("<fkincampaignid>" + FKINCampaignID.ToString() + "</fkincampaignid>");
            xml.Append("<fkinbatchid>" + FKINBatchID.ToString() + "</fkinbatchid>");
            xml.Append("<amountofleads>" + AmountOfLeads.ToString() + "</amountofleads>");
            xml.Append("<amountConserved>" + AmountConserved.ToString() + "</amountconserved>");
            xml.Append("</ConservedCampaigns>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the CallData object from a list of parameters.
        /// </summary>
        /// <param name="fkimportid"></param>
        /// <param name="number"></param>
        /// <param name="extension"></param>
        /// <param name="recref"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkincampaignid, long? fkinbatchid, int? amountofleads, int? amountconserved)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINCampaignID = fkincampaignid;
                this.FKINBatchID = fkinbatchid;
                this.AmountOfLeads = amountofleads;
                this.AmountConserved = amountconserved;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) CallData's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CallData history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return ConservedCampaignsMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an CallData object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the CallData object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return ConservedCampaignsMapper.UnDelete(this);
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
    /// A collection of the CallData object.
    /// </summary>
    public partial class ConservedCampaignsCollection : ObjectCollection<ConservedCampaigns>
    { 
    }
    #endregion
}
