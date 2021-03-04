using System.ComponentModel;
using System.Windows;

namespace UDM.Insurance.Interface.Data.ScheduleScreenVM
{
    public abstract class BaseViewModel : DependencyObject, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region OnPropertyChanged

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion //OnPropertyChanged
    }
}