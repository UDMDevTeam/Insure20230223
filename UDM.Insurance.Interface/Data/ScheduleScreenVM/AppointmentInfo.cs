using System;

namespace UDM.Insurance.Interface.Data.ScheduleScreenVM
{
    public class AppointmentInfo : BaseViewModel
    {

        #region Properties

        #region Description

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        #endregion

        #region End

        private DateTime _end;
        public DateTime End
        {
            get { return _end; }
            set
            {
                _end = value;
                OnPropertyChanged("End");
            }
        }

        #endregion

        #region EndTimeZoneId

        private string _endTimeZoneId;
        public string EndTimeZoneId
        {
            get { return _endTimeZoneId; }
            set
            {
                _endTimeZoneId = value;
                OnPropertyChanged("EndTimeZoneId");
            }
        }

        #endregion

        #region Id

        private string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        #endregion

        #region Categories

        private string _categories;
        public string Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged("Categories");
            }
        }

        #endregion

        #region IsLocked

        private bool? _isLocked = true;
        public bool? IsLocked
        {
            get { return _isLocked; }
            set
            {
                _isLocked = value;
                OnPropertyChanged("IsLocked");
            }
        }

        #endregion

        #region IsOccurrenceDeleted

        private bool? _isOccurrenceDeleted;
        public bool? IsOccurrenceDeleted
        {
            get { return _isOccurrenceDeleted; }
            set
            {
                _isOccurrenceDeleted = value;
                OnPropertyChanged("IsOccurrenceDeleted");
            }
        }

        #endregion

        #region IsTimeZoneNeutral

        private bool _isTimeZoneNeutral;
        public bool IsTimeZoneNeutral
        {
            get { return _isTimeZoneNeutral; }
            set
            {
                _isTimeZoneNeutral = value;
                OnPropertyChanged("IsTimeZoneNeutral");
            }
        }

        #endregion

        #region IsVisible

        private bool? _isVisible;
        public bool? IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        #endregion

        #region Location

        private string _location;
        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                OnPropertyChanged("Location");
            }
        }

        #endregion

        #region MaxOccurrenceDateTime

        private DateTime? _mxOccurrenceDateTime;
        public DateTime? MaxOccurrenceDateTime
        {
            get { return _mxOccurrenceDateTime; }
            set
            {
                _mxOccurrenceDateTime = value;
                OnPropertyChanged("MaxOccurrenceDateTime");
            }
        }

        #endregion

        #region OriginalOccurrenceEnd

        private DateTime _originalOccurrenceEnd; // = DateTime.Now;
        public DateTime OriginalOccurrenceEnd
        {
            get { return _originalOccurrenceEnd; }
            set
            {
                _originalOccurrenceEnd = value;
                OnPropertyChanged("OriginalOccurrenceEnd");
            }
        }

        #endregion

        #region OriginalOccurrenceStart

        private DateTime _originalOccurrenceStart; // = DateTime.Now;
        public DateTime OriginalOccurrenceStart
        {
            get { return _originalOccurrenceStart; }
            set
            {
                _originalOccurrenceStart = value;
                OnPropertyChanged("OriginalOccurrenceStart");
            }
        }

        #endregion

        #region OwningCalendarId

        private string _owningCalendarId;
        public string OwningCalendarId
        {
            get { return _owningCalendarId; }
            set
            {
                _owningCalendarId = value;
                OnPropertyChanged("OwningCalendarId");
            }
        }

        #endregion

        #region OwningResourceId

        private string _owningResourceId;
        public string OwningResourceId
        {
            get { return _owningResourceId; }
            set
            {
                _owningResourceId = value;
                OnPropertyChanged("OwningResourceId");
            }
        }

        #endregion

        #region Recurrence

        private string _recurrence;
        public string Recurrence
        {
            get { return _recurrence; }
            set
            {
                _recurrence = value;
                OnPropertyChanged("Recurrence");
            }
        }

        #endregion

        #region RecurrenceVersion

        private int? _recurrenceVersion;
        public int? RecurrenceVersion
        {
            get { return _recurrenceVersion; }
            set
            {
                _recurrenceVersion = value;
                OnPropertyChanged("RecurrenceVersion");
            }
        }

        #endregion

        #region Reminder

        private string _reminder;
        public string Reminder
        {
            get { return _reminder; }
            set
            {
                _reminder = value;
                OnPropertyChanged("Reminder");
            }
        }

        #endregion

        #region ReminderEnabled

        private bool? _reminderEnabled;
        public bool? ReminderEnabled
        {
            get { return _reminderEnabled; }
            set
            {
                _reminderEnabled = value;
                OnPropertyChanged("ReminderEnabled");
            }
        }

        #endregion

        #region ReminderInterval

        private TimeSpan _reminderInterval;
        public TimeSpan ReminderInterval
        {
            get { return _reminderInterval; }
            set
            {
                _reminderInterval = value;
                OnPropertyChanged("ReminderInterval");
            }
        }

        #endregion

        #region RootActivityId

        private string _rootActivityId;
        public string RootActivityId
        {
            get { return _rootActivityId; }
            set
            {
                _rootActivityId = value;
                OnPropertyChanged("RootActivityId");
            }
        }

        #endregion

        #region Start

        private DateTime _start; // = DateTime.Now;
        public DateTime Start
        {
            get { return _start; }
            set
            {
                _start = value;
                OnPropertyChanged("Start");
            }
        }

        #endregion

        #region StartTimeZoneId

        private string _startTimeZoneId;
        public string StartTimeZoneId
        {
            get { return _startTimeZoneId; }
            set
            {
                _startTimeZoneId = value;
                OnPropertyChanged("StartTimeZoneId");
            }
        }

        #endregion

        #region Subject

        private string _subject;
        public string Subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                OnPropertyChanged("Subject");
            }
        }

        #endregion

        #region UnmappedProperties

        private string _unmappedProperties;
        public string UnmappedProperties
        {
            get { return _unmappedProperties; }
            set
            {
                _unmappedProperties = value;
                OnPropertyChanged("UnmappedProperties");
            }
        }

        #endregion

        #region VariantProperties

        private long? _variantProperties;
        public long? VariantProperties
        {
            get { return _variantProperties; }
            set
            {
                _variantProperties = value;
                OnPropertyChanged("VariantProperties");
            }
        }

        #endregion 

        #region ImportID

        private long? _importID;
        public long? ImportID
        {
            get { return _importID; }
            set
            {
                _importID = value;
                OnPropertyChanged("ImportID");
            }
        }

        #endregion

        #endregion

    }
}
