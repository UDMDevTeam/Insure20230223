
namespace UDM.Insurance.Interface.Data.ScheduleScreenVM
{
    public class ResourceInfo : BaseViewModel
    {
        #region Properties

        #region DaySettingsOverrides

        private string _daySettingsOverrides;
        public string DaySettingsOverrides
        {
            get { return _daySettingsOverrides; }
            set
            {
                _daySettingsOverrides = value;
                OnPropertyChanged("DaySettingsOverrides");
            }
        }

        #endregion

        #region DaysOfWeek

        private string _daysOfWeek;
        public string DaysOfWeek
        {
            get { return _daysOfWeek; }
            set
            {
                _daysOfWeek = value;
                OnPropertyChanged("DaysOfWeek");
            }
        }

        #endregion

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

        #region EmailAddress

        private string _emailAddress;
        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                _emailAddress = value;
                OnPropertyChanged("EmailAddress");
            }
        }

        #endregion

        #region FirstDayOfWeek

        private byte? _firstDayOfWeek;
        public byte? FirstDayOfWeek
        {
            get { return _firstDayOfWeek; }
            set
            {
                _firstDayOfWeek = value;
                OnPropertyChanged("FirstDayOfWeek");
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

        #region IsLocked

        private string _isLocked;
        public string IsLocked
        {
            get { return _isLocked; }
            set
            {
                _isLocked = value;
                OnPropertyChanged("IsLocked");
            }
        }

        #endregion

        #region IsVisible

        private string _isVisible;
        public string IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        #endregion

        #region Name

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        #endregion

        #region PrimaryCalendarId

        private string _primaryCalendarId;
        public string PrimaryCalendarId
        {
            get { return _primaryCalendarId; }
            set
            {
                _primaryCalendarId = value;
                OnPropertyChanged("PrimaryCalendarId");
            }
        }

        #endregion

        #region PrimaryTimeZoneId

        private string _primaryTimeZoneId;
        public string PrimaryTimeZoneId
        {
            get { return _primaryTimeZoneId; }
            set
            {
                _primaryTimeZoneId = value;
                OnPropertyChanged("PrimaryTimeZoneId");
            }
        }

        #endregion 

        #endregion
    }
}