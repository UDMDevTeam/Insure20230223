using System.Windows.Media;

namespace UDM.Insurance.Interface.Data.ScheduleScreenVM
{
    public class ActivityCategoryInfo : BaseViewModel
    {
        #region Properties

        private string _categoryName;
        public string CategoryName
        {
            get { return _categoryName; }
            set
            {
                _categoryName = value;
                OnPropertyChanged("CategoryName");
            }
        }

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

        private Color? _color;
        public Color? Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged("Color");
            }
        }

        #endregion
    }
}
