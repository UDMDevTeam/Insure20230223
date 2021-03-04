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
    public partial class TSRTargets : ObjectBase<long>
    {
        #region Members
        private long? _fkinweekid = null;
        private long? _fkincampaignid = null;
        private long? _fkagentid = null;
        private DateTime? _datefrom = null;
        private DateTime? _dateto = null;
        private Double? _hours = null;
        private Double? _basetarget = null;
        private Double? _premiumtarget = null;
        private string _accdisselecteditem = null;
        private Double? _unittarget = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the TSRTargets class.
        /// </summary>
        public TSRTargets()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the TSRTargets class.
        /// </summary>
        public TSRTargets(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKINWeekID
        {
            get
            {
                Fill();
                return _fkinweekid;
            }
            set 
            {
                Fill();
                if (value != _fkinweekid)
                {
                    _fkinweekid = value;
                    _hasChanged = true;
                }
            }
        }

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

        public long? FKAgentID
        {
            get
            {
                Fill();
                return _fkagentid;
            }
            set 
            {
                Fill();
                if (value != _fkagentid)
                {
                    _fkagentid = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? DateFrom
        {
            get
            {
                Fill();
                return _datefrom;
            }
            set 
            {
                Fill();
                if (value != _datefrom)
                {
                    _datefrom = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? DateTo
        {
            get
            {
                Fill();
                return _dateto;
            }
            set 
            {
                Fill();
                if (value != _dateto)
                {
                    _dateto = value;
                    _hasChanged = true;
                }
            }
        }

        public Double? Hours
        {
            get
            {
                Fill();
                return _hours;
            }
            set 
            {
                Fill();
                if (value != _hours)
                {
                    _hours = value;
                    _hasChanged = true;
                }
            }
        }

        public Double? BaseTarget
        {
            get
            {
                Fill();
                return _basetarget;
            }
            set 
            {
                Fill();
                if (value != _basetarget)
                {
                    _basetarget = value;
                    _hasChanged = true;
                }
            }
        }

        public Double? PremiumTarget
        {
            get
            {
                Fill();
                return _premiumtarget;
            }
            set 
            {
                Fill();
                if (value != _premiumtarget)
                {
                    _premiumtarget = value;
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

        public Double? UnitTarget
        {
            get
            {
                Fill();
                return _unittarget;
            }
            set 
            {
                Fill();
                if (value != _unittarget)
                {
                    _unittarget = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an TSRTargets object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                TSRTargetsMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an TSRTargets object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the TSRTargets object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = TSRTargetsMapper.Save(this);
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
        /// Deletes an TSRTargets object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the TSRTargets object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && TSRTargetsMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the TSRTargets.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<tsrtargets>");
            xml.Append("<fkinweekid>" + FKINWeekID.ToString() + "</fkinweekid>");
            xml.Append("<fkincampaignid>" + FKINCampaignID.ToString() + "</fkincampaignid>");
            xml.Append("<fkagentid>" + FKAgentID.ToString() + "</fkagentid>");
            xml.Append("<datefrom>" + DateFrom.Value.ToString("dd MMM yyyy HH:mm:ss") + "</datefrom>");
            xml.Append("<dateto>" + DateTo.Value.ToString("dd MMM yyyy HH:mm:ss") + "</dateto>");
            xml.Append("<hours>" + Hours.ToString() + "</hours>");
            xml.Append("<basetarget>" + BaseTarget.ToString() + "</basetarget>");
            xml.Append("<premiumtarget>" + PremiumTarget.ToString() + "</premiumtarget>");
            xml.Append("<accdisselecteditem>" + AccDisSelectedItem.ToString() + "</accdisselecteditem>");
            xml.Append("<unittarget>" + UnitTarget.ToString() + "</unittarget>");
            xml.Append("</tsrtargets>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the TSRTargets object from a list of parameters.
        /// </summary>
        /// <param name="fkinweekid"></param>
        /// <param name="fkincampaignid"></param>
        /// <param name="fkagentid"></param>
        /// <param name="datefrom"></param>
        /// <param name="dateto"></param>
        /// <param name="hours"></param>
        /// <param name="basetarget"></param>
        /// <param name="premiumtarget"></param>
        /// <param name="accdisselecteditem"></param>
        /// <param name="unittarget"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkinweekid, long? fkincampaignid, long? fkagentid, DateTime? datefrom, DateTime? dateto, Double? hours, Double? basetarget, Double? premiumtarget, string accdisselecteditem, Double? unittarget)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINWeekID = fkinweekid;
                this.FKINCampaignID = fkincampaignid;
                this.FKAgentID = fkagentid;
                this.DateFrom = datefrom;
                this.DateTo = dateto;
                this.Hours = hours;
                this.BaseTarget = basetarget;
                this.PremiumTarget = premiumtarget;
                this.AccDisSelectedItem = accdisselecteditem;
                this.UnitTarget = unittarget;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) TSRTargets's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the TSRTargets history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return TSRTargetsMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an TSRTargets object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the TSRTargets object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return TSRTargetsMapper.UnDelete(this);
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
    /// A collection of the TSRTargets object.
    /// </summary>
    public partial class TSRTargetsCollection : ObjectCollection<TSRTargets>
    { 
    }
    #endregion
}
