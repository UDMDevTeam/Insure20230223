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
    public partial class TSRCampaignTargetDefaults : ObjectBase<long>
    {
        #region Members
        private long? _fkincampaignid = null;
        private Double? _salesperhourtarget = null;
        private decimal? _basepremiumtarget = null;
        private decimal? _partnerpremiumtarget = null;
        private decimal? _childpremiumtarget = null;
        private Double? _partnertarget = null;
        private Double? _childtarget = null;
        private DateTime? _dateapplicablefrom = null;
        private Double? _baseunittarget = null;
        private string _accdisselecteditem = null;
        private long? _fkincampaignclusterid = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the TSRCampaignTargetDefaults class.
        /// </summary>
        public TSRCampaignTargetDefaults()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the TSRCampaignTargetDefaults class.
        /// </summary>
        public TSRCampaignTargetDefaults(long id)
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

        public Double? SalesPerHourTarget
        {
            get
            {
                Fill();
                return _salesperhourtarget;
            }
            set 
            {
                Fill();
                if (value != _salesperhourtarget)
                {
                    _salesperhourtarget = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? BasePremiumTarget
        {
            get
            {
                Fill();
                return _basepremiumtarget;
            }
            set 
            {
                Fill();
                if (value != _basepremiumtarget)
                {
                    _basepremiumtarget = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? PartnerPremiumTarget
        {
            get
            {
                Fill();
                return _partnerpremiumtarget;
            }
            set 
            {
                Fill();
                if (value != _partnerpremiumtarget)
                {
                    _partnerpremiumtarget = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? ChildPremiumTarget
        {
            get
            {
                Fill();
                return _childpremiumtarget;
            }
            set 
            {
                Fill();
                if (value != _childpremiumtarget)
                {
                    _childpremiumtarget = value;
                    _hasChanged = true;
                }
            }
        }

        public Double? PartnerTarget
        {
            get
            {
                Fill();
                return _partnertarget;
            }
            set 
            {
                Fill();
                if (value != _partnertarget)
                {
                    _partnertarget = value;
                    _hasChanged = true;
                }
            }
        }

        public Double? ChildTarget
        {
            get
            {
                Fill();
                return _childtarget;
            }
            set 
            {
                Fill();
                if (value != _childtarget)
                {
                    _childtarget = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? DateApplicableFrom
        {
            get
            {
                Fill();
                return _dateapplicablefrom;
            }
            set 
            {
                Fill();
                if (value != _dateapplicablefrom)
                {
                    _dateapplicablefrom = value;
                    _hasChanged = true;
                }
            }
        }

        public Double? BaseUnitTarget
        {
            get
            {
                Fill();
                return _baseunittarget;
            }
            set 
            {
                Fill();
                if (value != _baseunittarget)
                {
                    _baseunittarget = value;
                    _hasChanged = true;
                }
            }
        }

        public string AccDisSelectedItem
        {
            get
            {
                Fill();
                return _accdisselecteditem;
            }
            set 
            {
                Fill();
                if (value != _accdisselecteditem)
                {
                    _accdisselecteditem = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINCampaignClusterID
        {
            get
            {
                Fill();
                return _fkincampaignclusterid;
            }
            set 
            {
                Fill();
                if (value != _fkincampaignclusterid)
                {
                    _fkincampaignclusterid = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an TSRCampaignTargetDefaults object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                TSRCampaignTargetDefaultsMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an TSRCampaignTargetDefaults object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the TSRCampaignTargetDefaults object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = TSRCampaignTargetDefaultsMapper.Save(this);
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
        /// Deletes an TSRCampaignTargetDefaults object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the TSRCampaignTargetDefaults object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && TSRCampaignTargetDefaultsMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the TSRCampaignTargetDefaults.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<tsrcampaigntargetdefaults>");
            xml.Append("<fkincampaignid>" + FKINCampaignID.ToString() + "</fkincampaignid>");
            xml.Append("<salesperhourtarget>" + SalesPerHourTarget.ToString() + "</salesperhourtarget>");
            xml.Append("<basepremiumtarget>" + BasePremiumTarget.ToString() + "</basepremiumtarget>");
            xml.Append("<partnerpremiumtarget>" + PartnerPremiumTarget.ToString() + "</partnerpremiumtarget>");
            xml.Append("<childpremiumtarget>" + ChildPremiumTarget.ToString() + "</childpremiumtarget>");
            xml.Append("<partnertarget>" + PartnerTarget.ToString() + "</partnertarget>");
            xml.Append("<childtarget>" + ChildTarget.ToString() + "</childtarget>");
            xml.Append("<dateapplicablefrom>" + DateApplicableFrom.Value.ToString("dd MMM yyyy HH:mm:ss") + "</dateapplicablefrom>");
            xml.Append("<baseunittarget>" + BaseUnitTarget.ToString() + "</baseunittarget>");
            xml.Append("<accdisselecteditem>" + AccDisSelectedItem.ToString() + "</accdisselecteditem>");
            xml.Append("<fkincampaignclusterid>" + FKINCampaignClusterID.ToString() + "</fkincampaignclusterid>");
            xml.Append("</tsrcampaigntargetdefaults>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the TSRCampaignTargetDefaults object from a list of parameters.
        /// </summary>
        /// <param name="fkincampaignid"></param>
        /// <param name="salesperhourtarget"></param>
        /// <param name="basepremiumtarget"></param>
        /// <param name="partnerpremiumtarget"></param>
        /// <param name="childpremiumtarget"></param>
        /// <param name="partnertarget"></param>
        /// <param name="childtarget"></param>
        /// <param name="dateapplicablefrom"></param>
        /// <param name="baseunittarget"></param>
        /// <param name="accdisselecteditem"></param>
        /// <param name="fkincampaignclusterid"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkincampaignid, Double? salesperhourtarget, decimal? basepremiumtarget, decimal? partnerpremiumtarget, decimal? childpremiumtarget, Double? partnertarget, Double? childtarget, DateTime? dateapplicablefrom, Double? baseunittarget, string accdisselecteditem, long? fkincampaignclusterid)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINCampaignID = fkincampaignid;
                this.SalesPerHourTarget = salesperhourtarget;
                this.BasePremiumTarget = basepremiumtarget;
                this.PartnerPremiumTarget = partnerpremiumtarget;
                this.ChildPremiumTarget = childpremiumtarget;
                this.PartnerTarget = partnertarget;
                this.ChildTarget = childtarget;
                this.DateApplicableFrom = dateapplicablefrom;
                this.BaseUnitTarget = baseunittarget;
                this.AccDisSelectedItem = accdisselecteditem;
                this.FKINCampaignClusterID = fkincampaignclusterid;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) TSRCampaignTargetDefaults's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the TSRCampaignTargetDefaults history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return TSRCampaignTargetDefaultsMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an TSRCampaignTargetDefaults object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the TSRCampaignTargetDefaults object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return TSRCampaignTargetDefaultsMapper.UnDelete(this);
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
    /// A collection of the TSRCampaignTargetDefaults object.
    /// </summary>
    public partial class TSRCampaignTargetDefaultsCollection : ObjectCollection<TSRCampaignTargetDefaults>
    { 
    }
    #endregion
}
