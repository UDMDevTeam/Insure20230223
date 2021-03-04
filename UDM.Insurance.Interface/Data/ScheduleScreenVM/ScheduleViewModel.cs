using Infragistics.Controls.Schedules;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Xml.Serialization;

namespace UDM.Insurance.Interface.Data.ScheduleScreenVM
{
    public class ScheduleViewModel : BaseViewModel
    {

        #region Construtors

        public ScheduleViewModel()
        {
        }

        public ScheduleViewModel(string currentUserId, string currentUserName, string currentUserCalendarId)
        {
            Resources.Add(new ResourceInfo
            {
                Id = currentUserId,
                Name = currentUserName
            });

            ResourceCalendars.Add(new ResourceCalendarInfo
            {
                Id = currentUserCalendarId,
                OwningResourceId = currentUserId
            });

            

            //ActivityCategory actCat1 = new ActivityCategory
            //{
            //    CategoryName = "Category1",
            //    Description = "First type of Activity",
            //    Color = Colors.Red
            //};

            //ActivityCategories.Add(actCat1);

            ActivityCategories.Add(new ActivityCategoryInfo
            {
                CategoryName = "Business",
                Description = "Business",
                Color = Colors.GreenYellow
            });

            CurrentResourceCalendar = ResourceCalendars[0];
        }

        #endregion

        #region Properties

        #region Appointments

        private ObservableCollection<AppointmentInfo> _appointments = new ObservableCollection<AppointmentInfo>();
        public ObservableCollection<AppointmentInfo> Appointments
        {
            get { return _appointments; }
            set
            {
                _appointments = value;
                OnPropertyChanged("Appointments");
            }
        }

        #endregion

        #region CalendarDisplayMode

        private CalendarDisplayMode _calendarDisplayMode = CalendarDisplayMode.Separate;
        public CalendarDisplayMode CalendarDisplayMode
        {
            get { return _calendarDisplayMode; }
            set
            {
                _calendarDisplayMode = value;
                OnPropertyChanged("CalendarDisplayMode ");
            }
        }

        #endregion

        #region CurrentResourceCalendar

        private ResourceCalendarInfo _currentResourceCalendar = new ResourceCalendarInfo();
        [XmlIgnore]
        public ResourceCalendarInfo CurrentResourceCalendar
        {
            get { return _currentResourceCalendar; }
            set
            {
                _currentResourceCalendar = value;
                OnPropertyChanged("CurrentResourceCalendar");
            }
        }

        #endregion

        #region Resources

        private ObservableCollection<ResourceInfo> _resources = new ObservableCollection<ResourceInfo>();
        public ObservableCollection<ResourceInfo> Resources
        {
            get { return _resources; }
            set
            {
                _resources = value;
                OnPropertyChanged("Resources");
            }
        }

        #endregion

        #region ResourceCalendars

        private ObservableCollection<ResourceCalendarInfo> _resourceCalendars = new ObservableCollection<ResourceCalendarInfo>();
        public ObservableCollection<ResourceCalendarInfo> ResourceCalendars
        {
            get { return _resourceCalendars; }
            set
            {
                _resourceCalendars = value;
                OnPropertyChanged("ResourceCalendars");
            }
        }

        #endregion 

        #region ActivityCategories

        //private ActivityCategoryCollection _activityCategories = new ActivityCategoryCollection();
        //public ActivityCategoryCollection ActivityCategories
        //{
        //    get { return _activityCategories; }
        //    set
        //    {
        //        _activityCategories = value;
        //        OnPropertyChanged("ActivityCategory");
        //    }
        //}

        private ObservableCollection<ActivityCategoryInfo> _activityCategories = new ObservableCollection<ActivityCategoryInfo>();
        public ObservableCollection<ActivityCategoryInfo> ActivityCategories
        {
            get { return _activityCategories; }
            set
            {
                _activityCategories = value;
                OnPropertyChanged("ActivityCategory");
            }
        }

        #endregion 

        #endregion

    }
}