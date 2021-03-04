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
    public partial class INBumpUpOption : ObjectBase<long>
    {
        #region Members
        private string _description = null;
        private string _importcode = null;
        private long? _fkincampaigntypeid = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INBumpUpOption class.
        /// </summary>
        public INBumpUpOption()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INBumpUpOption class.
        /// </summary>
        public INBumpUpOption(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public string Description
        {
            get
            {
                Fill();
                return _description;
            }
            set 
            {
                Fill();
                if (value != _description)
                {
                    _description = value;
                    _hasChanged = true;
                }
            }
        }

        public string ImportCode
        {
            get
            {
                Fill();
                return _importcode;
            }
            set 
            {
                Fill();
                if (value != _importcode)
                {
                    _importcode = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINCampaignTypeID
        {
            get
            {
                Fill();
                return _fkincampaigntypeid;
            }
            set 
            {
                Fill();
                if (value != _fkincampaigntypeid)
                {
                    _fkincampaigntypeid = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INBumpUpOption object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INBumpUpOptionMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INBumpUpOption object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INBumpUpOption object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INBumpUpOptionMapper.Save(this);
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
        /// Deletes an INBumpUpOption object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INBumpUpOption object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INBumpUpOptionMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INBumpUpOption.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inbumpupoption>");
            xml.Append("<description>" + Description.ToString() + "</description>");
            xml.Append("<importcode>" + ImportCode.ToString() + "</importcode>");
            xml.Append("<fkincampaigntypeid>" + FKINCampaignTypeID.ToString() + "</fkincampaigntypeid>");
            xml.Append("</inbumpupoption>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INBumpUpOption object from a list of parameters.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="importcode"></param>
        /// <param name="fkincampaigntypeid"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(string description, string importcode, long? fkincampaigntypeid)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.Description = description;
                this.ImportCode = importcode;
                this.FKINCampaignTypeID = fkincampaigntypeid;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INBumpUpOption's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INBumpUpOption history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INBumpUpOptionMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INBumpUpOption object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INBumpUpOption object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INBumpUpOptionMapper.UnDelete(this);
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
    /// A collection of the INBumpUpOption object.
    /// </summary>
    public partial class INBumpUpOptionCollection : ObjectCollection<INBumpUpOption>
    { 
    }
    #endregion
}
