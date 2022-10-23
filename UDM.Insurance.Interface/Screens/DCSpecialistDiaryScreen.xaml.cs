using Embriant.Framework.Configuration;
using Infragistics.Windows.DataPresenter;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using UDM.Insurance.Business;
using System.Text.RegularExpressions;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{

    public partial class DCSpecialistDiaryScreen
    {

        public DateTime? SelectedDeclineReasonID { get; set; }
        private readonly LeadApplicationScreen _LeadApplicationScreen;
        DataTable dtSalesData = new DataTable();


        public DCSpecialistDiaryScreen(LeadApplicationScreen leadApplicationScreen)
        {
            InitializeComponent();
            _LeadApplicationScreen = leadApplicationScreen;
            LoadLookupData();

        }




        private void LoadLookupData()
        {



        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedDeclineReasonID = DateTime.Parse(calToDate.SelectedDate.ToString());

                //StringBuilder strQueryAgentOnline = new StringBuilder();
                //strQueryAgentOnline.Append("SELECT TOP 1 Online [Response] ");
                //strQueryAgentOnline.Append("FROM INCMAgentsOnline ");
                //strQueryAgentOnline.Append("WHERE FKUserID = " + SelectedDeclineReasonID.ToString());
                //DataTable dtOnline = Methods.GetTableData(strQueryAgentOnline.ToString());

                //string CampaignName = dtOnline.Rows[0]["Response"].ToString();

                //if (CampaignName == "1         " || CampaignName == "1")
                //{
                    string ID;
                    try
                    {
                        StringBuilder strSaletoCMID = new StringBuilder();
                        strSaletoCMID.Append("SELECT TOP 1 ID [Response] ");
                        strSaletoCMID.Append("FROM INDCSpecialistDiary ");
                        strSaletoCMID.Append("WHERE FKINImportID = " + _LeadApplicationScreen.LaData.AppData.ImportID.ToString());
                        DataTable dtSAlestoCMID = Methods.GetTableData(strSaletoCMID.ToString());

                        ID = dtSAlestoCMID.Rows[0]["Response"].ToString();
                    }
                    catch
                    {
                        ID = null;
                    }

                TimeSpan? selectedTime = null;
        
                try 
                {
                    selectedTime = TimeSpan.Parse(HourTB.Text + ":" + MinutesTB.Text + ":00");
                }
                catch 
                {
                    return;
                }


                    if (ID == null || ID == "")
                    {
                        INDCSpecialistDiary scm = new INDCSpecialistDiary();
                        scm.FKINImportID = _LeadApplicationScreen.LaData.AppData.ImportID;
                        scm.FKUserID = GlobalSettings.ApplicationUser.ID;
                        scm.StartDate = DateTime.Now;
                        scm.EndDate = SelectedDeclineReasonID;
                        scm.Time = selectedTime;


                        scm.Save(_validationResult);
                    }
                    else
                    {
                        INDCSpecialistDiary scm = new INDCSpecialistDiary(long.Parse(ID));
                        scm.FKINImportID = _LeadApplicationScreen.LaData.AppData.ImportID;
                        scm.FKUserID = GlobalSettings.ApplicationUser.ID;
                        scm.StartDate = DateTime.Now;
                        scm.EndDate = SelectedDeclineReasonID;
                        scm.Time = selectedTime;

                        scm.Save(_validationResult);
                    }



                    OnDialogClose(_dialogResult);
                //}
                //else
                //{
                //    cmbDeclineReason.SelectedIndex = -1;
                //}



            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void HourTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int hour = int.Parse(HourTB.Text);
                if (hour >= 25)
                {
                    HourTB.Clear();
                }
            }
            catch
            {

            }
        }

        private void MinutesTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int minute = int.Parse(MinutesTB.Text);
                if (minute >= 61)
                {
                    HourTB.Clear();
                }
            }
            catch
            {

            }
        }
    }
}
