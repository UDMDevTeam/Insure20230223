using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using UDM.Insurance.Business.Mapping;
using Embriant.Framework.Configuration;
using Embriant.Framework;
using Embriant.Framework.Validation;
using INUserScheduleMapper = UDM.Insurance.Business.Mapping.INUserScheduleMapper;

namespace UDM.Insurance.Business
{
    public partial class INUserSchedule : ObjectBase<long>
    {
        #region Members
        private long? _fksystemid = null;
        private long? _fkuserid = null;
        private long? _fkinimportid = null;
        private Guid? _scheduleid = null;
        private TimeSpan? _duration = null;
        private DateTime? _start = null;
        private DateTime? _end = null;
        private string _subject = null;
        private string _description = null;
        private string _location = null;
        private string _categories = null;
        private bool? _reminderenabled = null;
        private long? _reminderinterval = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the INUserSchedule class.
        /// </summary>
        public INUserSchedule()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the INUserSchedule class.
        /// </summary>
        public INUserSchedule(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
        public long? FKSystemID
        {
            get
            {
                Fill();
                return _fksystemid;
            }
            set 
            {
                Fill();
                if (value != _fksystemid)
                {
                    _fksystemid = value;
                    _hasChanged = true;
                }
            }
        }

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

        public Guid? ScheduleID
        {
            get
            {
                Fill();
                return _scheduleid;
            }
            set 
            {
                Fill();
                if (value != _scheduleid)
                {
                    _scheduleid = value;
                    _hasChanged = true;
                }
            }
        }

        public TimeSpan? Duration
        {
            get
            {
                Fill();
                return _duration;
            }
            set 
            {
                Fill();
                if (value != _duration)
                {
                    _duration = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? Start
        {
            get
            {
                Fill();
                return _start;
            }
            set 
            {
                Fill();
                if (value != _start)
                {
                    _start = value;
                    _hasChanged = true;
                }
            }
        }

        public DateTime? End
        {
            get
            {
                Fill();
                return _end;
            }
            set 
            {
                Fill();
                if (value != _end)
                {
                    _end = value;
                    _hasChanged = true;
                }
            }
        }

        public string Subject
        {
            get
            {
                Fill();
                return _subject;
            }
            set 
            {
                Fill();
                if (value != _subject)
                {
                    _subject = value;
                    _hasChanged = true;
                }
            }
        }

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

        public string Location
        {
            get
            {
                Fill();
                return _location;
            }
            set 
            {
                Fill();
                if (value != _location)
                {
                    _location = value;
                    _hasChanged = true;
                }
            }
        }

        public string Categories
        {
            get
            {
                Fill();
                return _categories;
            }
            set 
            {
                Fill();
                if (value != _categories)
                {
                    _categories = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? ReminderEnabled
        {
            get
            {
                Fill();
                return _reminderenabled;
            }
            set 
            {
                Fill();
                if (value != _reminderenabled)
                {
                    _reminderenabled = value;
                    _hasChanged = true;
                }
            }
        }

        public long? ReminderInterval
        {
            get
            {
                Fill();
                return _reminderinterval;
            }
            set 
            {
                Fill();
                if (value != _reminderinterval)
                {
                    _reminderinterval = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an INUserSchedule object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                INUserScheduleMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an INUserSchedule object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INUserSchedule object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = INUserScheduleMapper.Save(this);
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
        /// Deletes an INUserSchedule object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INUserSchedule object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && INUserScheduleMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the INUserSchedule.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<inuserschedule>");
            xml.Append("<fksystemid>" + FKSystemID.ToString() + "</fksystemid>");
            xml.Append("<fkuserid>" + FKUserID.ToString() + "</fkuserid>");
            xml.Append("<fkinimportid>" + FKINImportID.ToString() + "</fkinimportid>");
            xml.Append("<scheduleid>" + ScheduleID.ToString() + "</scheduleid>");
            xml.Append("<duration>" + Duration.ToString() + "</duration>");
            xml.Append("<start>" + Start.Value.ToString("dd MMM yyyy HH:mm:ss") + "</start>");
            xml.Append("<end>" + End.Value.ToString("dd MMM yyyy HH:mm:ss") + "</end>");
            xml.Append("<subject>" + Subject.ToString() + "</subject>");
            xml.Append("<description>" + Description.ToString() + "</description>");
            xml.Append("<location>" + Location.ToString() + "</location>");
            xml.Append("<categories>" + Categories.ToString() + "</categories>");
            xml.Append("<reminderenabled>" + ReminderEnabled.ToString() + "</reminderenabled>");
            xml.Append("<reminderinterval>" + ReminderInterval.ToString() + "</reminderinterval>");
            xml.Append("</inuserschedule>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the INUserSchedule object from a list of parameters.
        /// </summary>
        /// <param name="fksystemid"></param>
        /// <param name="fkuserid"></param>
        /// <param name="fkinimportid"></param>
        /// <param name="scheduleid"></param>
        /// <param name="duration"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="subject"></param>
        /// <param name="description"></param>
        /// <param name="location"></param>
        /// <param name="categories"></param>
        /// <param name="reminderenabled"></param>
        /// <param name="reminderinterval"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fksystemid, long? fkuserid, long? fkinimportid, Guid? scheduleid, TimeSpan? duration, DateTime? start, DateTime? end, string subject, string description, string location, string categories, bool? reminderenabled, long? reminderinterval)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKSystemID = fksystemid;
                this.FKUserID = fkuserid;
                this.FKINImportID = fkinimportid;
                this.ScheduleID = scheduleid;
                this.Duration = duration;
                this.Start = start;
                this.End = end;
                this.Subject = subject;
                this.Description = description;
                this.Location = location;
                this.Categories = categories;
                this.ReminderEnabled = reminderenabled;
                this.ReminderInterval = reminderinterval;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) INUserSchedule's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INUserSchedule history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INUserScheduleMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an INUserSchedule object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the INUserSchedule object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return INUserScheduleMapper.UnDelete(this);
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
    /// A collection of the INUserSchedule object.
    /// </summary>
    public partial class INUserScheduleCollection : ObjectCollection<INUserSchedule>
    { 
    }
    #endregion
}
