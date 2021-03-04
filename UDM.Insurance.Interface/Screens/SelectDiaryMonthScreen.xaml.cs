using System;
using System.Windows;
using System.Windows.Input;

namespace UDM.Insurance.Interface.Screens
{

    public partial class SelectDiaryMonthScreen
    {

        public DateTime? SelectedDate { get; set; }

        public SelectDiaryMonthScreen()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (Cal1.SelectedDate != null)
            {
                SelectedDate = Cal1.SelectedDate;
            }

            OnDialogClose(_dialogResult);
        }

        private void Cal1_Loaded(object sender, RoutedEventArgs e)
        {
            Cal1.SelectedDate = SelectedDate;
            Keyboard.Focus(Cal1);
        }

        private void Cal1_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            if (Cal1.SelectedDate != null)
            {
                SelectedDate = Cal1.SelectedDate;
            }
        }

    }
}
