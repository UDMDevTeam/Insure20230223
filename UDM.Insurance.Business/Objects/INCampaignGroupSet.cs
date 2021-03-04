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
    public partial class INCampaignGroupSet : ObjectBase<long>
    {
        #region Members
        private long? _fklkpincampaigngroup = null;
        private long? _fklkpincampaigngrouptype = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INCampaignGroupSet class.
        /// </summary>
        public INCampaignGroupSet()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INCampaignGroupSet class.
        /// </summary>
        public INCampaignGroupSet(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKlkpINCampaignGroup
        {
            get
            {
                Fill();
                return _fklkpincampaigngroup;
            }
            set 
            {
                Fill();
                if (value != _fklkpincampaigngroup)
                {
                    _fklkpincampaigngroup = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKlkpINCampaignGroupType
        {
            get
            {
                Fill();
                return _fklkpincampaigngrouptype;
            }
            set 
            {
                Fill();
                if (value != _fklkpincampaigngrouptype)
                {
                    _fklkpincampaigngrouptype = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INCampaignGroupSet object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INCampaignGroupSetMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INCampaignGroupSet object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INCampaignGroupSet object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INCampaignGroupSetMapper.Save(this);
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
        /// Deletes an INCampaignGroupSet object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INCampaignGroupSet object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INCampaignGroupSetMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INCampaignGroupSet.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<incampaigngroupset>");
            xml.Append("<fklkpincampaigngroup>" + FKlkpINCampaignGroup.ToString() + "</fklkpincampaigngroup>");
            xml.Append("<fklkpincampaigngrouptype>" + FKlkpINCampaignGroupType.ToString() + "</fklkpincampaigngrouptype>");
            xml.Append("</incampaigngroupset>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INCampaignGroupSet object from a list of parameters.
        /// </summary>
        /// <param name="fklkpincampaigngroup"></param>
        /// <param name="fklkpincampaigngrouptype"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fklkpincampaigngroup, long? fklkpincampaigngrouptype)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKlkpINCampaignGroup = fklkpincampaigngroup;
                this.FKlkpINCampaignGroupType = fklkpincampaigngrouptype;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INCampaignGroupSet's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INCampaignGroupSet history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INCampaignGroupSetMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INCampaignGroupSet object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INCampaignGroupSet object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INCampaignGroupSetMapper.UnDelete(this);
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
    /// A collection of the INCampaignGroupSet object.
    /// </summary>
    public partial class INCampaignGroupSetCollection : ObjectCollection<INCampaignGroupSet>
    { 
    }
    #endregion
}
