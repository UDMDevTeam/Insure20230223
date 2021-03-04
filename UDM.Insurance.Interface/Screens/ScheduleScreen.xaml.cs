using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using Embriant.Framework.Configuration;
using Infragistics.Controls.Schedules;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.Data.ScheduleScreenVM;
using UDM.Insurance.Interface.Windows;
using ValidationResult = Embriant.Framework.Validation.ValidationResult;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ScheduleScreen
    {

        #region Members

        private readonly ValidationResult _validationResult = null;
        private readonly ScheduleViewModel _scheduleViewModel;
        public readonly ScheduleScreenData _ssData = new ScheduleScreenData();
        private readonly DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private readonly SalesScreenGlobalData _ssGlobalData;

        #endregion



        #region Constructors

        public ScheduleScreen(SalesScreenGlobalData ssGlobalData)
        {
            try
            {
                InitializeComponent();
                _ssGlobalData = ssGlobalData;
                _ssGlobalData.ScheduleScreen = this;

                var scheme2010 = new Office2010ColorScheme();
                scheme2010.OfficeColorScheme = OfficeColorScheme.Silver;
                XamScheduleDataManager.ColorScheme = scheme2010;

                User user = new User(GlobalSettings.ApplicationUser.ID);
                _ssData.UserID = user.ID;
                _ssData.UserName = user.FirstName.Trim() + " " + user .LastName.Trim();

                INUserScheduleCollection inUserScheduleCollection = INUserScheduleMapper.Search(_ssData.SystemID, _ssData.UserID, null, null);
                foreach (INUserSchedule userSchedule in inUserScheduleCollection)
                {
                    if (userSchedule.Start <= DateTime.Now.AddDays(-7)) 
                    {
                        userSchedule.Delete(_validationResult); //Delete older than 7 days schedule entries
                    }
                    else
                    {
                        _ssData.INUserSchedules.Add(userSchedule);
                    }
                }

                DataContext = _ssData;
                _scheduleViewModel = new ScheduleViewModel(_ssData.UserID.ToString(), _ssData.UserName, "currentUserCalendar");

                //Load Existing Appointments (Schedules)
                foreach (INUserSchedule userSchedule in _ssData.INUserSchedules)
                {
                    _scheduleViewModel.Appointments.Add(new AppointmentInfo
                    {
                        Id = userSchedule.ScheduleID.ToString(),
                        OwningCalendarId = "currentUserCalendar",
                        OwningResourceId = _ssData.UserID.ToString(),
                        Start = Convert.ToDateTime(userSchedule.Start),
                        End = Convert.ToDateTime(userSchedule.End),
                        Subject = userSchedule.Subject,
                        Description = userSchedule.Description,
                        Location = userSchedule.Location,
                        Categories = userSchedule.Categories,
                        ReminderEnabled = userSchedule.ReminderEnabled,
                        ReminderInterval = TimeSpan.FromSeconds(Convert.ToDouble(userSchedule.ReminderInterval)),
                        StartTimeZoneId = "South Africa Standard Time",
                        EndTimeZoneId = "South Africa Standard Time",
                        ImportID = userSchedule.FKINImportID,
                        //IsTimeZoneNeutral = true,
                    });
                }

                dispatcherTimer.Tick += dispatcherTimer_Tick;
                dispatcherTimer.Interval = TimeSpan.FromMilliseconds(200);
            }

            catch (Exception ex)
            {
                //HandleException(ex);
            }
        }

        #endregion



        #region Private Methods

        private void SaveActivity(ActivityAddedEventArgs e)
        {
            try
            {
                Guid guid;
                INUserSchedule userSchedule = new INUserSchedule();

                userSchedule.FKSystemID = _ssData.SystemID;
                userSchedule.FKUserID = _ssData.UserID;
                userSchedule.FKINImportID = ((AppointmentInfo)e.Activity.DataItem).ImportID;
                if (Guid.TryParse(e.Activity.Id, out guid)) { userSchedule.ScheduleID = guid; }
                userSchedule.Duration = e.Activity.Duration;
                userSchedule.Start = e.Activity.Start;
                userSchedule.End = e.Activity.End;
                userSchedule.Subject = e.Activity.Subject;
                userSchedule.Description = e.Activity.Description;
                userSchedule.Location = ((AppointmentInfo)e.Activity.DataItem).Location;
                userSchedule.Categories = e.Activity.Categories;
                userSchedule.ReminderEnabled = e.Activity.ReminderEnabled;
                userSchedule.ReminderInterval = Convert.ToInt64(e.Activity.ReminderInterval.TotalSeconds);

                userSchedule.Save(_validationResult);
                _ssData.INUserSchedules.Add(userSchedule);
            }

            catch (Exception ex)
            {
                //HandleException(ex);
            }
        }

        private void RemoveActivity(ActivityRemovedEventArgs e)
        {
            Guid guid;
            if (Guid.TryParse(e.Activity.Id, out guid))
            {
                INUserSchedule userSchedule = _ssData.INUserSchedules.Select(s => s).FirstOrDefault(s => s.ScheduleID == guid);
                _ssData.INUserSchedules.Remove(userSchedule);
                if (userSchedule != null) userSchedule.Delete(_validationResult);
            }
        }

        private void UpdateActivity(ActivityChangedEventArgs e)
        {
            try
            {
                Guid guid;

                if (Guid.TryParse(e.Activity.Id, out guid))
                {
                    INUserSchedule userSchedule = _ssData.INUserSchedules.Select(s => s).FirstOrDefault(s => s.ScheduleID == guid);

                    if (userSchedule != null)
                    {
                        _ssData.INUserSchedules.Remove(userSchedule);

                        userSchedule.Duration = e.Activity.Duration;
                        userSchedule.Start = e.Activity.Start;
                        userSchedule.End = e.Activity.End;
                        userSchedule.Subject = e.Activity.Subject;
                        userSchedule.Description = e.Activity.Description;
                        userSchedule.Location = ((AppointmentInfo)e.Activity.DataItem).Location;
                        userSchedule.Categories = e.Activity.Categories;
                        userSchedule.ReminderEnabled = e.Activity.ReminderEnabled;
                        userSchedule.ReminderInterval = Convert.ToInt64(e.Activity.ReminderInterval.TotalSeconds);
                        userSchedule.Save(_validationResult);

                        _ssData.INUserSchedules.Add(userSchedule);
                    }
                }
            }

            catch (Exception ex)
            {
                //HandleException(ex);
            }
        }

        #endregion



        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            _ssData.RefNumber = null;
            _ssData.ImportID = null;
            Hide();
            //if (_ssGlobalData.LeadApplicationScreen != null)
            //{
            //    _ssGlobalData.LeadApplicationScreen.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart) delegate
            //    {
                    
            //        _ssGlobalData.LeadApplicationScreen.medReference.Focus();
            //        Keyboard.Focus(_ssGlobalData.LeadApplicationScreen.medReference);
            //        //_ssGlobalData.LeadApplicationScreen.medReference.RaiseEvent(new RoutedEventArgs(MouseDoubleClickEvent));
            //        //_ssGlobalData.LeadApplicationScreen.medReference.SelectAll();

            //    });
            //}
        }

        private void outlookView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (((XamOutlookCalendarView)sender).SelectedActivities.Count == 1)
            {
                if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
                {
                    Guid guid;
                    if (Guid.TryParse(((XamOutlookCalendarView)sender).SelectedActivities[0].Id, out guid))
                    {
                        INUserSchedule userSchedule = _ssData.INUserSchedules.Select(s => s).FirstOrDefault(s => s.ScheduleID == guid);
                        if (userSchedule != null)
                        {
                            long? importID = userSchedule.FKINImportID;

                            if (importID != null && importID > 0)
                            {
                                Hide();

                                _ssGlobalData.SalesScreen.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                                {
                                    LeadApplicationScreen leadApplicationScreen = new LeadApplicationScreen(importID, _ssGlobalData);
                                    _ssGlobalData.SalesScreen.ShowDialog(leadApplicationScreen, new INDialogWindow(leadApplicationScreen));
                                });
                            }
                        }
                    }
                }
            }
        }

        private void XamScheduleDataManager_ActivityAdded(object sender, ActivityAddedEventArgs e)
        {
            if (e.Activity.DataItem != null) ((AppointmentInfo) e.Activity.DataItem).ImportID = _ssData.ImportID;
            _ssData.ImportID = null;
            SaveActivity(e);
        }

        private void XamScheduleDataManager_ActivityChanged(object sender, ActivityChangedEventArgs e)
        {
            UpdateActivity(e);
        }

        private void XamScheduleDataManager_ActivityRemoved(object sender, ActivityRemovedEventArgs e)
        {
            RemoveActivity(e);
        }

        private void XamScheduleDataManager_ActivityDialogDisplaying(object sender, ActivityDialogDisplayingEventArgs e)
        {
            if (!string.IsNullOrEmpty(_ssData.RefNumber))
            {
                e.Activity.Subject = _ssData.RefNumber;
                _ssData.RefNumber = null;
                e.Activity.Categories = "Business";
            }
        }

        private void XamScheduleDataConnector_Loaded(object sender, RoutedEventArgs e)
        {
            XamScheduleDataConnector.DataContext = _scheduleViewModel;
        }

        private void outlookView_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            outlookView.BringIntoView(DateTime.Now + new TimeSpan(1,0,0));
        }

        private void WindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        #endregion

    }
}
