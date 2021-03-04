using System;
using System.Data;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using UDM.Insurance.Interface.Data;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{

    public partial class SelectDiaryReasonScreen
    {

        public long? SelectedDiaryReasonID { get; set; }

        private readonly SalesScreenGlobalData _ssGlobalData;

        public SelectDiaryReasonScreen(SalesScreenGlobalData ssGlobalData)
        {
            InitializeComponent();
            _ssGlobalData = ssGlobalData;

            LoadLookupData();
        }

        private void LoadLookupData()
        {
            DataTable dtStatus = Methods.GetTableData("SELECT * FROM INDiaryReason ORDER BY [Description] ASC");
            cmbDiaryReason.Populate(dtStatus, "Description", "ID");
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!_ssGlobalData.LeadApplicationScreen.medDOAccountNumber.IsValueValid)
                {
                    _ssGlobalData.LeadApplicationScreen.LaData.BankDetailsData.AccountNumber = string.Empty;
                }
                SelectedDiaryReasonID = Convert.ToInt32(cmbDiaryReason.SelectedValue);
                OnDialogClose(_dialogResult);

                //long? userID = _LeadApplicationScreen.LaData.UserData.UserID;
                long? importID = _ssGlobalData.LeadApplicationScreen.LaData.AppData.ImportID;
                string refNo = _ssGlobalData.LeadApplicationScreen.LaData.AppData.RefNo;

                _ssGlobalData.LeadApplicationScreen.LaData.AppData.DiaryReasonID = SelectedDiaryReasonID;
                _ssGlobalData.LeadApplicationScreen.btnSave.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));


                if (Convert.ToBoolean(chkScheduleDiary.IsChecked))
                {
                    {
                        _ssGlobalData.ScheduleScreen.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            _ssGlobalData.ScheduleScreen._ssData.ImportID = importID;
                            _ssGlobalData.ScheduleScreen._ssData.RefNumber = refNo;
                            _ssGlobalData.ScheduleScreen.WindowState = WindowState.Normal;
                            _ssGlobalData.ScheduleScreen.Show();
                            _ssGlobalData.ScheduleScreen.Activate();
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void EmbriantComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Methods.EmbriantComboBoxPreviewKeyDown(sender, e);
        }

        private void cmbDiaryReason_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(cmbDiaryReason);
        }

        private void cmbDiaryReason_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;
                if (cmbDiaryReason.SelectedValue != null && _ssGlobalData.LeadApplicationScreen.cmbAgent.SelectedValue != null)
                {
                    btnSave.IsEnabled = true;
                }
                else
                {
                    btnSave.ToolTip = _ssGlobalData.LeadApplicationScreen.btnSave.ToolTip;
                }
                
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void tbScheduleDiary_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            chkScheduleDiary.IsChecked = !Convert.ToBoolean(chkScheduleDiary.IsChecked);
        }
    }
}
