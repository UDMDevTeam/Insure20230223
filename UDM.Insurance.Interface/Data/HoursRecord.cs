using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace UDM.Insurance.Interface.Data
{
    #region ObservableObjectHoursRecord Class

    public abstract class ObservableObjectHoursRecord : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propName)
        {
            var pc = PropertyChanged;
            if (pc != null) pc(this, new PropertyChangedEventArgs(propName));
        }
    }

    #endregion ObservableObjectHoursRecord Class

    public class HoursRecord : ObservableObjectHoursRecord
    {
        #region Protected Methods

        protected void SetProperty<T>(ref T field, T value, Expression<Func<T>> expr)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                var lambda = (LambdaExpression)expr;
                MemberExpression memberExpr;
                if (lambda.Body is UnaryExpression)
                {
                    var unaryExpr = (UnaryExpression)lambda.Body;
                    memberExpr = (MemberExpression)unaryExpr.Operand;
                }
                else
                {
                    memberExpr = (MemberExpression)lambda.Body;
                }
                OnPropertyChanged(memberExpr.Member.Name);
            }
        }

        #endregion Protected Methods

        #region Private Members

        private long _id;
        private long? _fkUserID;
        private long? _fkINCampaignID;

        
        private DateTime? _workingDate;
        private string _morningShiftStartTime;
        private string _morningShiftEndTime;
        private string _normalShiftStartTime;
        private string _normalShiftEndTime;
        private string _eveningShiftStartTime;
        private string _eveningShiftEndTime;
        private string _publicHolidayWeekendShiftStartTime;
        private string _publicHolidayWeekendShiftEndTime;

        #endregion Private Members

        #region Publicly Encapsulated Properties

        public long ID
        {
            get
            {
                return _id;
            }
            set
            {
                SetProperty(ref _id, value, () => ID);
            }
        }

        public long? FKUserID
        {
            get
            {
                return _fkUserID;
            }
            set
            {
                SetProperty(ref _fkUserID, value, () => FKUserID);
            }
        }

        public long? FKINCampaignID
        {
            get
            {
                return _fkINCampaignID;
            }
            set
            {
                SetProperty(ref _fkINCampaignID, value, () => FKINCampaignID);
            }
        }

        public DateTime? WorkingDate
        {
            get
            {
                return _workingDate;
            }
            set
            {
                SetProperty(ref _workingDate, value, () => WorkingDate);
            }
        }

        public string MorningShiftStartTime
        {
            get
            {
                return _morningShiftStartTime;
            }
            set
            {
                SetProperty(ref _morningShiftStartTime, value, () => MorningShiftStartTime);
            }
        }

        public string MorningShiftEndTime
        {
            get
            {
                return _morningShiftEndTime;
            }
            set
            {
                SetProperty(ref _morningShiftEndTime, value, () => MorningShiftEndTime);
            }
        }

        public string NormalShiftStartTime
        {
            get
            {
                return _normalShiftStartTime;
            }
            set
            {
                SetProperty(ref _normalShiftStartTime, value, () => NormalShiftStartTime);
            }
        }

        public string NormalShiftEndTime
        {
            get
            {
                return _normalShiftEndTime;
            }
            set
            {
                SetProperty(ref _normalShiftEndTime, value, () => NormalShiftEndTime);
            }
        }

        public string EveningShiftStartTime
        {
            get
            {
                return _eveningShiftStartTime;
            }
            set
            {
                SetProperty(ref _eveningShiftStartTime, value, () => EveningShiftStartTime);
            }
        }

        public string EveningShiftEndTime
        {
            get
            {
                return _eveningShiftEndTime;
            }
            set
            {
                SetProperty(ref _eveningShiftEndTime, value, () => EveningShiftEndTime);
            }
        }

        public string PublicHolidayWeekendShiftStartTime
        {
            get
            {
                return _publicHolidayWeekendShiftStartTime;
            }
            set
            {
                SetProperty(ref _publicHolidayWeekendShiftStartTime, value, () => PublicHolidayWeekendShiftStartTime);
            }
        }

        public string PublicHolidayWeekendShiftEndTime
        {
            get
            {
                return _publicHolidayWeekendShiftEndTime;
            }
            set
            {
                SetProperty(ref _publicHolidayWeekendShiftEndTime, value, () => PublicHolidayWeekendShiftEndTime);
            }
        }

        #endregion Publicly Encapsulated Properties
    }
}
