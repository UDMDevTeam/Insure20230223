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
    public partial class INImportSchedules : ObjectBase<long>
    {
        #region Members
        private long? _fkincampaignid = null;
        private string _batchname = null;
        private string _udmcode = null;
        private byte[] _importfile = null;
        private DateTime? _scheduledate = null;
        private TimeSpan? _scheduletime = null;
        private bool? _hasrun = null;
        private int? _numberofleads = null;
        private int? _importatempts = null;
        private DateTime? _datereceived = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INImportSchedules class.
        /// </summary>
        public INImportSchedules()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INImportSchedules class.
        /// </summary>
        public INImportSchedules(long id)
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

        public string BatchName
        {
            get
            {
                Fill();
                return _batchname;
            }
            set 
            {
                Fill();
                if (value != _batchname)
                {
                    _batchname = value;
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

        public byte[] ImportFile
        {
            get
            {
                Fill();
                return _importfile;
            }
            set 
            {
                Fill();
                if (value != _importfile)
                {
                    _importfile = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? ScheduleDate
        {
            get
            {
                Fill();
                return _scheduledate;
            }
            set 
            {
                Fill();
                if (value != _scheduledate)
                {
                    _scheduledate = value;
                    _hasChanged = true;
                }
            }
        }

        public TimeSpan? ScheduleTime
        {
            get
            {
                Fill();
                return _scheduletime;
            }
            set 
            {
                Fill();
                if (value != _scheduletime)
                {
                    _scheduletime = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? HasRun
        {
            get
            {
                Fill();
                return _hasrun;
            }
            set 
            {
                Fill();
                if (value != _hasrun)
                {
                    _hasrun = value;
                    _hasChanged = true;
                }
            }
        }

        public int? NumberOfLeads
        {
            get
            {
                Fill();
                return _numberofleads;
            }
            set 
            {
                Fill();
                if (value != _numberofleads)
                {
                    _numberofleads = value;
                    _hasChanged = true;
                }
            }
        }

        public int? ImportAtempts
        {
            get
            {
                Fill();
                return _importatempts;
            }
            set 
            {
                Fill();
                if (value != _importatempts)
                {
                    _importatempts = value;
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
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INImportSchedules object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INImportSchedulesMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INImportSchedules object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportSchedules object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INImportSchedulesMapper.Save(this);
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
        /// Deletes an INImportSchedules object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportSchedules object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INImportSchedulesMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INImportSchedules.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inimportschedules>");
            xml.Append("<fkincampaignid>" + FKINCampaignID.ToString() + "</fkincampaignid>");
            xml.Append("<batchname>" + BatchName.ToString() + "</batchname>");
            xml.Append("<udmcode>" + UDMCode.ToString() + "</udmcode>");
            xml.Append("<importfile>" + ImportFile.ToString() + "</importfile>");
            xml.Append("<scheduledate>" + ScheduleDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</scheduledate>");
            xml.Append("<scheduletime>" + ScheduleTime.ToString() + "</scheduletime>");
            xml.Append("<hasrun>" + HasRun.ToString() + "</hasrun>");
            xml.Append("<numberofleads>" + NumberOfLeads.ToString() + "</numberofleads>");
            xml.Append("<importatempts>" + ImportAtempts.ToString() + "</importatempts>");
            xml.Append("<datereceived>" + DateReceived.Value.ToString("dd MMM yyyy HH:mm:ss") + "</datereceived>");
            xml.Append("</inimportschedules>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INImportSchedules object from a list of parameters.
        /// </summary>
        /// <param name="fkincampaignid"></param>
        /// <param name="batchname"></param>
        /// <param name="udmcode"></param>
        /// <param name="importfile"></param>
        /// <param name="scheduledate"></param>
        /// <param name="scheduletime"></param>
        /// <param name="hasrun"></param>
        /// <param name="numberofleads"></param>
        /// <param name="importatempts"></param>
        /// <param name="datereceived"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkincampaignid, string batchname, string udmcode, byte[] importfile, DateTime? scheduledate, TimeSpan? scheduletime, bool? hasrun, int? numberofleads, int? importatempts, DateTime? datereceived)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKINCampaignID = fkincampaignid;
                this.BatchName = batchname;
                this.UDMCode = udmcode;
                this.ImportFile = importfile;
                this.ScheduleDate = scheduledate;
                this.ScheduleTime = scheduletime;
                this.HasRun = hasrun;
                this.NumberOfLeads = numberofleads;
                this.ImportAtempts = importatempts;
                this.DateReceived = datereceived;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INImportSchedules's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportSchedules history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportSchedulesMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INImportSchedules object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INImportSchedules object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INImportSchedulesMapper.UnDelete(this);
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
    /// A collection of the INImportSchedules object.
    /// </summary>
    public partial class INImportSchedulesCollection : ObjectCollection<INImportSchedules>
    { 
    }
    #endregion
}
