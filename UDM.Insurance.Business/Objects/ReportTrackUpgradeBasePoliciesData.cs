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
    public partial class ReportTrackUpgradeBasePoliciesData : ObjectBase<long>
    {
        #region Members
        private long? _year = null;
        private string _month = null;
        private long? _leadsreceived = null;
        private long? _shemaccleads = null;
        private decimal? _targetpercentage = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the ReportTrackUpgradeBasePoliciesData class.
        /// </summary>
        public ReportTrackUpgradeBasePoliciesData()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the ReportTrackUpgradeBasePoliciesData class.
        /// </summary>
        public ReportTrackUpgradeBasePoliciesData(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? Year
        {
            get
            {
                Fill();
                return _year;
            }
            set 
            {
                Fill();
                if (value != _year)
                {
                    _year = value;
                    _hasChanged = true;
                }
            }
        }

        public string Month
        {
            get
            {
                Fill();
                return _month;
            }
            set 
            {
                Fill();
                if (value != _month)
                {
                    _month = value;
                    _hasChanged = true;
                }
            }
        }

        public long? LeadsReceived
        {
            get
            {
                Fill();
                return _leadsreceived;
            }
            set 
            {
                Fill();
                if (value != _leadsreceived)
                {
                    _leadsreceived = value;
                    _hasChanged = true;
                }
            }
        }

        public long? SheMaccLeads
        {
            get
            {
                Fill();
                return _shemaccleads;
            }
            set 
            {
                Fill();
                if (value != _shemaccleads)
                {
                    _shemaccleads = value;
                    _hasChanged = true;
                }
            }
        }

        public decimal? TargetPercentage
        {
            get
            {
                Fill();
                return _targetpercentage;
            }
            set 
            {
                Fill();
                if (value != _targetpercentage)
                {
                    _targetpercentage = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an ReportTrackUpgradeBasePoliciesData object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                ReportTrackUpgradeBasePoliciesDataMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an ReportTrackUpgradeBasePoliciesData object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the ReportTrackUpgradeBasePoliciesData object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = ReportTrackUpgradeBasePoliciesDataMapper.Save(this);
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
        /// Deletes an ReportTrackUpgradeBasePoliciesData object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the ReportTrackUpgradeBasePoliciesData object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && ReportTrackUpgradeBasePoliciesDataMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the ReportTrackUpgradeBasePoliciesData.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<reporttrackupgradebasepoliciesdata>");
            xml.Append("<year>" + Year.ToString() + "</year>");
            xml.Append("<month>" + Month.ToString() + "</month>");
            xml.Append("<leadsreceived>" + LeadsReceived.ToString() + "</leadsreceived>");
            xml.Append("<shemaccleads>" + SheMaccLeads.ToString() + "</shemaccleads>");
            xml.Append("<targetpercentage>" + TargetPercentage.ToString() + "</targetpercentage>");
            xml.Append("</reporttrackupgradebasepoliciesdata>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the ReportTrackUpgradeBasePoliciesData object from a list of parameters.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="leadsreceived"></param>
        /// <param name="shemaccleads"></param>
        /// <param name="targetpercentage"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? year, string month, long? leadsreceived, long? shemaccleads, decimal? targetpercentage)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.Year = year;
                this.Month = month;
                this.LeadsReceived = leadsreceived;
                this.SheMaccLeads = shemaccleads;
                this.TargetPercentage = targetpercentage;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) ReportTrackUpgradeBasePoliciesData's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the ReportTrackUpgradeBasePoliciesData history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return ReportTrackUpgradeBasePoliciesDataMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an ReportTrackUpgradeBasePoliciesData object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the ReportTrackUpgradeBasePoliciesData object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return ReportTrackUpgradeBasePoliciesDataMapper.UnDelete(this);
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
    /// A collection of the ReportTrackUpgradeBasePoliciesData object.
    /// </summary>
    public partial class ReportTrackUpgradeBasePoliciesDataCollection : ObjectCollection<ReportTrackUpgradeBasePoliciesData>
    { 
    }
    #endregion
}
