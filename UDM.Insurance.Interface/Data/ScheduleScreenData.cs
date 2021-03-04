using System.Collections.ObjectModel;
using UDM.Insurance.Business;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Data
{

    public class ScheduleScreenData : ObservableObject
    {

        #region Constructor

        public ScheduleScreenData()
        {
            SystemID = 2;
        }

        #endregion



        #region Public Methods

        //public void Clear()
        //{
        //    ImportID = null;
        //    RefNumber = null;
        //    SystemID = 2;
        //    UserID = null;
        //    UserName = null;
        //}

        #endregion



        #region Properties

        private long? _importID;
        public long? ImportID
        {
            get { return _importID; }
            set { SetProperty(ref _importID, value, () => ImportID); }
        }

        private string _refNumber;
        public string RefNumber
        {
            get { return _refNumber; }
            set { SetProperty(ref _refNumber, value, () => RefNumber); }
        }

        private long _systemID;
        public long SystemID
        {
            get { return _systemID; }
            set { SetProperty(ref _systemID, value, () => SystemID); }
        }

        private long? _userID;
        public long? UserID
        {
            get { return _userID; }
            set { SetProperty(ref _userID, value, () => UserID); }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value, () => UserName); }
        }

        private ObservableCollection<INUserSchedule> _inUserSchedules = new ObservableCollection<INUserSchedule>();
        public ObservableCollection<INUserSchedule> INUserSchedules
        {
            get { return _inUserSchedules; }
            set
            {
                _inUserSchedules = value;
                OnPropertyChanged("INUserSchedules");
            }
        }

        #endregion

    }
}
