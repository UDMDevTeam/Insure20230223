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
    public partial class INCampaignTypeSet : ObjectBase<long>
    {
        #region Members
        private long? _fkcampaigntypeid = null;
        private long? _fkcampaigntypegroupid = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INCampaignTypeSet class.
        /// </summary>
        public INCampaignTypeSet()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INCampaignTypeSet class.
        /// </summary>
        public INCampaignTypeSet(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKCampaignTypeID
        {
            get
            {
                Fill();
                return _fkcampaigntypeid;
            }
            set 
            {
                Fill();
                if (value != _fkcampaigntypeid)
                {
                    _fkcampaigntypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKCampaignTypeGroupID
        {
            get
            {
                Fill();
                return _fkcampaigntypegroupid;
            }
            set 
            {
                Fill();
                if (value != _fkcampaigntypegroupid)
                {
                    _fkcampaigntypegroupid = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INCampaignTypeSet object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INCampaignTypeSetMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INCampaignTypeSet object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INCampaignTypeSet object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INCampaignTypeSetMapper.Save(this);
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
        /// Deletes an INCampaignTypeSet object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INCampaignTypeSet object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INCampaignTypeSetMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INCampaignTypeSet.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<incampaigntypeset>");
            xml.Append("<fkcampaigntypeid>" + FKCampaignTypeID.ToString() + "</fkcampaigntypeid>");
            xml.Append("<fkcampaigntypegroupid>" + FKCampaignTypeGroupID.ToString() + "</fkcampaigntypegroupid>");
            xml.Append("</incampaigntypeset>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INCampaignTypeSet object from a list of parameters.
        /// </summary>
        /// <param name="fkcampaigntypeid"></param>
        /// <param name="fkcampaigntypegroupid"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkcampaigntypeid, long? fkcampaigntypegroupid)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKCampaignTypeID = fkcampaigntypeid;
                this.FKCampaignTypeGroupID = fkcampaigntypegroupid;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INCampaignTypeSet's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INCampaignTypeSet history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INCampaignTypeSetMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INCampaignTypeSet object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INCampaignTypeSet object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INCampaignTypeSetMapper.UnDelete(this);
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
    /// A collection of the INCampaignTypeSet object.
    /// </summary>
    public partial class INCampaignTypeSetCollection : ObjectCollection<INCampaignTypeSet>
    { 
    }
    #endregion
}
