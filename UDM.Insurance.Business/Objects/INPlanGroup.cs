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
    public partial class INPlanGroup : ObjectBase<long>
    {
        #region Members
        private string _description = null;
        private long? _fkincampaigntypeid = null;
        private long? _fkincampaigngroupid = null;
        private DateTime? _date = null;
        private decimal? _policyfee = null;
        private bool? _isactive = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INPlanGroup class.
        /// </summary>
        public INPlanGroup()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INPlanGroup class.
        /// </summary>
        public INPlanGroup(long id)
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

        public long? FKINCampaignGroupID
        {
            get
            {
                Fill();
                return _fkincampaigngroupid;
            }
            set 
            {
                Fill();
                if (value != _fkincampaigngroupid)
                {
                    _fkincampaigngroupid = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? Date
        {
            get
            {
                Fill();
                return _date;
            }
            set 
            {
                Fill();
                if (value != _date)
                {
                    _date = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? PolicyFee
        {
            get
            {
                Fill();
                return _policyfee;
            }
            set 
            {
                Fill();
                if (value != _policyfee)
                {
                    _policyfee = value;
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
        /// Fills an INPlanGroup object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INPlanGroupMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INPlanGroup object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPlanGroup object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INPlanGroupMapper.Save(this);
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
        /// Deletes an INPlanGroup object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPlanGroup object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INPlanGroupMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INPlanGroup.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inplangroup>");
            xml.Append("<description>" + Description.ToString() + "</description>");
            xml.Append("<fkincampaigntypeid>" + FKINCampaignTypeID.ToString() + "</fkincampaigntypeid>");
            xml.Append("<fkincampaigngroupid>" + FKINCampaignGroupID.ToString() + "</fkincampaigngroupid>");
            xml.Append("<date>" + Date.Value.ToString("dd MMM yyyy HH:mm:ss") + "</date>");
            xml.Append("<policyfee>" + PolicyFee.ToString() + "</policyfee>");
            xml.Append("<isactive>" + IsActive.ToString() + "</isactive>");
            xml.Append("</inplangroup>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INPlanGroup object from a list of parameters.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="fkincampaigntypeid"></param>
        /// <param name="fkincampaigngroupid"></param>
        /// <param name="date"></param>
        /// <param name="policyfee"></param>
        /// <param name="isactive"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(string description, long? fkincampaigntypeid, long? fkincampaigngroupid, DateTime? date, decimal? policyfee, bool? isactive)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.Description = description;
                this.FKINCampaignTypeID = fkincampaigntypeid;
                this.FKINCampaignGroupID = fkincampaigngroupid;
                this.Date = date;
                this.PolicyFee = policyfee;
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
        /// Deletes a(n) INPlanGroup's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPlanGroup history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INPlanGroupMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INPlanGroup object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INPlanGroup object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INPlanGroupMapper.UnDelete(this);
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
    /// A collection of the INPlanGroup object.
    /// </summary>
    public partial class INPlanGroupCollection : ObjectCollection<INPlanGroup>
    { 
    }
    #endregion
}
