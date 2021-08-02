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
    public partial class INImportCallMonitoringStats : ObjectBase<long>
    {
        #region Members
        private long? _id = null;
        private long? _fkuserid = null;
        private long? _fkinimportid = null;
        private string _fkINlkpCampaignGroupType = null;
        private DateTime? _startTimeOverAssessment = null;
        private DateTime? _endTimeOverAssessment = null;
        private DateTime? _startTimeOverAssessorOutcome = null;
        private DateTime? _endTimeOverAssessorOutcome = null;
        private DateTime? _startTimeCFOverAssessment = null;
        private DateTime? _endTimeCFOverAssessment = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the Closure class.
        /// </summary>
        public INImportCallMonitoringStats()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the Closure class.
        /// </summary>
        public INImportCallMonitoringStats(long id)
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

        public long? FKUserID
        {
            get
            {
                Fill();
                return _fkuserid;
            }
            set
            {
                Fill();
                if (value != _fkuserid)
                {
                    _fkuserid = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKINImportID
        {
            get
            {
                Fill();
                return _fkinimportid;
            }
            set
            {
                Fill();
                if (value != _fkinimportid)
                {
                    _fkinimportid = value;
                    _hasChanged = true;
                }
            }
        }

        public string FKINlkpCampaignGroupType
        {
            get
            {
                Fill();
                return _fkINlkpCampaignGroupType;
            }
            set
            {
                Fill();
                if (value != _fkINlkpCampaignGroupType)
                {
                    _fkINlkpCampaignGroupType = value;
                    _hasChanged = true;
                }
            }
        }


        public DateTime? StartTimeOverAssessment
        {
            get
            {
                Fill();
                return _startTimeOverAssessment;
            }
            set
            {
                Fill();
                if (value != _startTimeOverAssessment)
                {
                    _startTimeOverAssessment = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? EndTimeOverAssessment
        {
            get
            {
                Fill();
                return _endTimeOverAssessment;
            }
            set
            {
                Fill();
                if (value != _endTimeOverAssessment)
                {
                    _endTimeOverAssessment = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? StartTimeOverAssessorOutcome
        {
            get
            {
                Fill();
                return _startTimeOverAssessorOutcome;
            }
            set
            {
                Fill();
                if (value != _startTimeOverAssessorOutcome)
                {
                    _startTimeOverAssessorOutcome = value;
                    _hasChanged = true;
                }
            }
        }
        public DateTime? EndTimeOverAssessorOutcome
        {
            get
            {
                Fill();
                return _endTimeOverAssessorOutcome;
            }
            set
            {
                Fill();
                if (value != _endTimeOverAssessorOutcome)
                {
                    _endTimeOverAssessorOutcome = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? StartTimeCFOverAssessment
        {
            get
            {
                Fill();
                return _startTimeCFOverAssessment;
            }
            set
            {
                Fill();
                if (value != _startTimeCFOverAssessment)
                {
                    _startTimeCFOverAssessment = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? EndTimeCFOverAssessment
        {
            get
            {
                Fill();
                return _endTimeCFOverAssessment;
            }
            set
            {
                Fill();
                if (value != _endTimeCFOverAssessment)
                {
                    _endTimeCFOverAssessment = value;
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
                INCallMonitoringStatsMapper.Fill(this);
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
                    _isLoaded = INCallMonitoringStatsMapper.Save(this);
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
                return BeforeDelete(validationResult) && INCallMonitoringStatsMapper.Delete(this);
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
            xml.Append("<incallmonitoringstats>");
            xml.Append("<fkinimportid>" + FKINImportID.ToString() + "</fkinimportid>");
            xml.Append("<fkinlkpcampaigngrouptype>" + FKINlkpCampaignGroupType.ToString() + "</fkinlkpcampaigngrouptype>");
            xml.Append("<starttimeoverassessment>" + StartTimeOverAssessment.ToString() + "</starttimeoverassessment>");
            xml.Append("<endtimeoverassessment>" + EndTimeOverAssessment.ToString() + "</endtimeoverassessment>");
            xml.Append("<starttimeoverassessoroutcome>" + StartTimeOverAssessorOutcome.ToString() + "</starttimeoverassessoroutcome>");
            xml.Append("<endtimeoverassessoroutcome>" + EndTimeOverAssessorOutcome.ToString() + "</endtimeoverassessoroutcome>");
            xml.Append("<starttimecfoverassessment>" + StartTimeCFOverAssessment.ToString() + "</starttimecfoverassessment>");
            xml.Append("<endtimecfoverassessment>" + EndTimeCFOverAssessment.ToString() + "</endtimecfoverassessment>");
            xml.Append("</incallmonitoringstats>");
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
        public ValidationResult Fill(long? fkinimportid, string fklkpincampaigngrouptype, DateTime? starttimeoverassessment, DateTime? endtimeoverassessment, DateTime? starttimeoverassessoroutcome, DateTime? endtimeoverassessoroutcome, DateTime? starttimecfoverassessment, DateTime? endtimecfoverassessment)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINImportID = fkinimportid;
                this.FKINlkpCampaignGroupType = fklkpincampaigngrouptype;
                this.StartTimeOverAssessment = starttimeoverassessment;
                this.EndTimeOverAssessment = endtimeoverassessment;
                this.StartTimeOverAssessorOutcome = starttimeoverassessoroutcome;
                this.EndTimeOverAssessorOutcome = endtimeoverassessoroutcome;
                this.StartTimeCFOverAssessment = starttimecfoverassessment;
                this.EndTimeCFOverAssessment = endtimecfoverassessment;

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
                return INCallMonitoringStatsMapper.DeleteHistory(this);
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
                return INCallMonitoringStatsMapper.UnDelete(this);
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
    public partial class INCallMonitoringStatsCollection : ObjectCollection<INImportCallMonitoringStats>
    {
    }
    #endregion
}
