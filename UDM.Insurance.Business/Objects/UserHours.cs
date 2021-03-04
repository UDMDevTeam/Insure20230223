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
    public partial class UserHours : ObjectBase<long>
    {
        #region Members
        private long? _fkuserid = null;
        private long? _fkincampaignid = null;
        private DateTime? _workingdate = null;
        private TimeSpan? _morningshiftstarttime = null;
        private TimeSpan? _morningshiftendtime = null;
        private TimeSpan? _normalshiftstarttime = null;
        private TimeSpan? _normalshiftendtime = null;
        private TimeSpan? _eveningshiftstarttime = null;
        private TimeSpan? _eveningshiftendtime = null;
        private TimeSpan? _publicholidayweekendshiftstarttime = null;
        private TimeSpan? _publicholidayweekendshiftendtime = null;
        private long? _fkshifttypeid = null;
        private bool? _isredeemedhours = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the UserHours class.
        /// </summary>
        public UserHours()
        {
            SetDefaults();
        }

        /// <summary>
        /// Creates a new instance of the UserHours class.
        /// </summary>
        public UserHours(long id)
        {
            ID = id;
        }
        #endregion

        #region Properties
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

        public DateTime? WorkingDate
        {
            get
            {
                Fill();
                return _workingdate;
            }
            set 
            {
                Fill();
                if (value != _workingdate)
                {
                    _workingdate = value;
                    _hasChanged = true;
                }
            }
        }

        public TimeSpan? MorningShiftStartTime
        {
            get
            {
                Fill();
                return _morningshiftstarttime;
            }
            set 
            {
                Fill();
                if (value != _morningshiftstarttime)
                {
                    _morningshiftstarttime = value;
                    _hasChanged = true;
                }
            }
        }

        public TimeSpan? MorningShiftEndTime
        {
            get
            {
                Fill();
                return _morningshiftendtime;
            }
            set 
            {
                Fill();
                if (value != _morningshiftendtime)
                {
                    _morningshiftendtime = value;
                    _hasChanged = true;
                }
            }
        }

        public TimeSpan? NormalShiftStartTime
        {
            get
            {
                Fill();
                return _normalshiftstarttime;
            }
            set 
            {
                Fill();
                if (value != _normalshiftstarttime)
                {
                    _normalshiftstarttime = value;
                    _hasChanged = true;
                }
            }
        }

        public TimeSpan? NormalShiftEndTime
        {
            get
            {
                Fill();
                return _normalshiftendtime;
            }
            set 
            {
                Fill();
                if (value != _normalshiftendtime)
                {
                    _normalshiftendtime = value;
                    _hasChanged = true;
                }
            }
        }

        public TimeSpan? EveningShiftStartTime
        {
            get
            {
                Fill();
                return _eveningshiftstarttime;
            }
            set 
            {
                Fill();
                if (value != _eveningshiftstarttime)
                {
                    _eveningshiftstarttime = value;
                    _hasChanged = true;
                }
            }
        }

        public TimeSpan? EveningShiftEndTime
        {
            get
            {
                Fill();
                return _eveningshiftendtime;
            }
            set 
            {
                Fill();
                if (value != _eveningshiftendtime)
                {
                    _eveningshiftendtime = value;
                    _hasChanged = true;
                }
            }
        }

        public TimeSpan? PublicHolidayWeekendShiftStartTime
        {
            get
            {
                Fill();
                return _publicholidayweekendshiftstarttime;
            }
            set 
            {
                Fill();
                if (value != _publicholidayweekendshiftstarttime)
                {
                    _publicholidayweekendshiftstarttime = value;
                    _hasChanged = true;
                }
            }
        }

        public TimeSpan? PublicHolidayWeekendShiftEndTime
        {
            get
            {
                Fill();
                return _publicholidayweekendshiftendtime;
            }
            set 
            {
                Fill();
                if (value != _publicholidayweekendshiftendtime)
                {
                    _publicholidayweekendshiftendtime = value;
                    _hasChanged = true;
                }
            }
        }

        public long? FKShiftTypeID
        {
            get
            {
                Fill();
                return _fkshifttypeid;
            }
            set 
            {
                Fill();
                if (value != _fkshifttypeid)
                {
                    _fkshifttypeid = value;
                    _hasChanged = true;
                }
            }
        }

        public bool? IsRedeemedHours
        {
            get
            {
                Fill();
                return _isredeemedhours;
            }
            set 
            {
                Fill();
                if (value != _isredeemedhours)
                {
                    _isredeemedhours = value;
                    _hasChanged = true;
                }
            }
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Fills an UserHours object from the database.
        /// </summary>
        public override void Fill()
        {
            if (_isLoaded == false && _isPopulated == false && ID != 0)
            {
                _isPopulated = true;
                UserHoursMapper.Fill(this);
            }
        }

        /// <summary>
        /// Saves an UserHours object to the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the UserHours object was saved, otherwise false.</returns>
        public override bool Save(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                bool result = true;    
                this.Fill();
                if (_hasChanged == true)
                {
                    _isLoaded = UserHoursMapper.Save(this);
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
        /// Deletes an UserHours object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the UserHours object was deleted, otherwize, false.</returns>
        public override bool Delete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return BeforeDelete(validationResult) && UserHoursMapper.Delete(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Builds up a XML representation of the UserHours.
        /// </summary>
        /// <returns>XML string.</returns>
        public override string ToXML()
        {
            StringBuilder xml = new StringBuilder();
            xml.Append("<userhours>");
            xml.Append("<fkuserid>" + FKUserID.ToString() + "</fkuserid>");
            xml.Append("<fkincampaignid>" + FKINCampaignID.ToString() + "</fkincampaignid>");
            xml.Append("<workingdate>" + WorkingDate.Value.ToString("dd MMM yyyy HH:mm:ss") + "</workingdate>");
            xml.Append("<morningshiftstarttime>" + MorningShiftStartTime.ToString() + "</morningshiftstarttime>");
            xml.Append("<morningshiftendtime>" + MorningShiftEndTime.ToString() + "</morningshiftendtime>");
            xml.Append("<normalshiftstarttime>" + NormalShiftStartTime.ToString() + "</normalshiftstarttime>");
            xml.Append("<normalshiftendtime>" + NormalShiftEndTime.ToString() + "</normalshiftendtime>");
            xml.Append("<eveningshiftstarttime>" + EveningShiftStartTime.ToString() + "</eveningshiftstarttime>");
            xml.Append("<eveningshiftendtime>" + EveningShiftEndTime.ToString() + "</eveningshiftendtime>");
            xml.Append("<publicholidayweekendshiftstarttime>" + PublicHolidayWeekendShiftStartTime.ToString() + "</publicholidayweekendshiftstarttime>");
            xml.Append("<publicholidayweekendshiftendtime>" + PublicHolidayWeekendShiftEndTime.ToString() + "</publicholidayweekendshiftendtime>");
            xml.Append("<fkshifttypeid>" + FKShiftTypeID.ToString() + "</fkshifttypeid>");
            xml.Append("<isredeemedhours>" + IsRedeemedHours.ToString() + "</isredeemedhours>");
            xml.Append("</userhours>");
            return xml.ToString();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fills the UserHours object from a list of parameters.
        /// </summary>
        /// <param name="fkuserid"></param>
        /// <param name="fkincampaignid"></param>
        /// <param name="workingdate"></param>
        /// <param name="morningshiftstarttime"></param>
        /// <param name="morningshiftendtime"></param>
        /// <param name="normalshiftstarttime"></param>
        /// <param name="normalshiftendtime"></param>
        /// <param name="eveningshiftstarttime"></param>
        /// <param name="eveningshiftendtime"></param>
        /// <param name="publicholidayweekendshiftstarttime"></param>
        /// <param name="publicholidayweekendshiftendtime"></param>
        /// <param name="fkshifttypeid"></param>
        /// <param name="isredeemedhours"></param>
        /// <returns>A Validation Result object with the result of the fill opertation.</returns>
        public ValidationResult Fill(long? fkuserid, long? fkincampaignid, DateTime? workingdate, TimeSpan? morningshiftstarttime, TimeSpan? morningshiftendtime, TimeSpan? normalshiftstarttime, TimeSpan? normalshiftendtime, TimeSpan? eveningshiftstarttime, TimeSpan? eveningshiftendtime, TimeSpan? publicholidayweekendshiftstarttime, TimeSpan? publicholidayweekendshiftendtime, long? fkshifttypeid, bool? isredeemedhours)
        {
            ValidationResult result = new ValidationResult();

            try
            {
                this.FKUserID = fkuserid;
                this.FKINCampaignID = fkincampaignid;
                this.WorkingDate = workingdate;
                this.MorningShiftStartTime = morningshiftstarttime;
                this.MorningShiftEndTime = morningshiftendtime;
                this.NormalShiftStartTime = normalshiftstarttime;
                this.NormalShiftEndTime = normalshiftendtime;
                this.EveningShiftStartTime = eveningshiftstarttime;
                this.EveningShiftEndTime = eveningshiftendtime;
                this.PublicHolidayWeekendShiftStartTime = publicholidayweekendshiftstarttime;
                this.PublicHolidayWeekendShiftEndTime = publicholidayweekendshiftendtime;
                this.FKShiftTypeID = fkshifttypeid;
                this.IsRedeemedHours = isredeemedhours;
            }
            catch (Exception ex)
            {
                result.Append(ex.Message);
            }
            _isPopulated = result.Passed;
            return result;
        }

        /// <summary>
        /// Deletes a(n) UserHours's history from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the UserHours history was deleted, otherwize false.</returns>
        public bool DeleteHistory(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return UserHoursMapper.DeleteHistory(this);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Undeletes an UserHours object from the database.
        /// </summary>
        /// <param name="validationResult">Validation result to check and populate.</param>
        /// <returns>True if the UserHours object was undeleted, otherwise false.</returns>
        public bool UnDelete(ValidationResult validationResult)
        {
            if (ValidationResult.Check(ref validationResult))
            {
                return UserHoursMapper.UnDelete(this);
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
    /// A collection of the UserHours object.
    /// </summary>
    public partial class UserHoursCollection : ObjectCollection<UserHours>
    { 
    }
    #endregion
}
