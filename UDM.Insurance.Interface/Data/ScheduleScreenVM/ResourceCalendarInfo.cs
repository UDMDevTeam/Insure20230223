
namespace UDM.Insurance.Interface.Data.ScheduleScreenVM
{
    public class ResourceCalendarInfo : BaseViewModel
    {
        #region Properties

        #region BaseColor

        private int? _baseColor;
        public int? BaseColor
        {
            get { return _baseColor; }
            set
            {
                _baseColor = value;
                OnPropertyChanged("BaseColor");
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

        #region UnmappedProperties

        public string UnmappedProperties { get; set; }

        #endregion

        #endregion
    }
}