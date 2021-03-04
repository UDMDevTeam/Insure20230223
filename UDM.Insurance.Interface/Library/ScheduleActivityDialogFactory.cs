using System.Windows.Media;
using Infragistics.Controls.Schedules;
using Infragistics.Controls.Schedules.Primitives;
using System;
using System.Windows;
using Infragistics.Windows.Controls;
using UDM.Insurance.Interface.Controls;

namespace UDM.Insurance.Interface.Library
{
    public class ScheduleActivityDialogFactory : ScheduleDialogFactoryBase
    {
        public override ActivityTypes SupportedActivityDialogTypes
        {
            get { return ActivityTypes.Appointment; }
        }

        public override FrameworkElement CreateActivityDialog(FrameworkElement container, XamScheduleDataManager dataManager, ActivityBase activity, bool allowModifications, bool allowRemove)
        {
            FrameworkElement fe;
            ActivityDialogRibbonLite ribbonLite;

            ResourceDictionary res = (ResourceDictionary)Application.LoadComponent(new Uri("../Styles/INScheduleDialogStyles.xaml", UriKind.Relative));

            ScheduleActivityDialog dialog = new ScheduleActivityDialog();
            dialog.IsVisibleChanged += Dialog_IsVisibleChanged;
            
            switch (activity.ActivityType)
            {
                case ActivityType.Appointment:
                {
                    fe = new AppointmentDialogCore(container, dataManager, activity as Appointment);
                    fe.Style = (Style)res["StyleAppointment"];
                    ribbonLite = new ActivityDialogRibbonLite(dataManager.ColorSchemeResolved, true, true, true);
                    dialog.LayoutRoot.Children.Add(fe);
                    break;
                }

                case ActivityType.Journal:
                {
                    fe = new JournalDialogCore(container, dataManager, activity as Journal);
                    ribbonLite = new ActivityDialogRibbonLite(dataManager.ColorSchemeResolved, true, false, false);
                    break;
                }

                case ActivityType.Task:
                {
                    fe = new TaskDialogCore(container, dataManager, activity as Task);
                    ribbonLite = new ActivityDialogRibbonLite(dataManager.ColorSchemeResolved, true, false, false);
                    break;
                }

                default:
                {
                    // Return null for unsupported activity types.
                    return null;
                }
            }
            
            ribbonLite.Style = (Style)res["StyleActivityDialogRibbon"];
            ribbonLite.DataContext = fe;
            ((ActivityDialogCore)fe).NavigationControlSiteContent = ribbonLite;

            return dialog;
        }

        void Dialog_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ScheduleActivityDialog dialog = (ScheduleActivityDialog)sender;

            if (dialog.IsVisible)
            {
                dialog.Width = SystemParameters.PrimaryScreenWidth * 0.4;
                dialog.Height = dialog.Width * 0.8;
                dialog.ResizeMode = ResizeMode.NoResize;
                dialog.Background = Brushes.Transparent;
                dialog.BorderThickness = new Thickness(0);
            }

        }
    }
}