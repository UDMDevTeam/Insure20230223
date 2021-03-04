using System;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using Embriant.Framework.Configuration;
using System.Data;
using UDM.Insurance.Business;
using System.Windows.Threading;
using System.Threading;
using Infragistics.Windows.DataPresenter;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.Windows;
using System.Windows.Controls;
using UDM.WPF.Library;
using System.Data.SqlClient;
using Embriant.Framework;
using Embriant.Framework.Data;
using System.Windows.Data;
using System.Collections.Generic;
using Embriant.WPF.Controls;
using Infragistics.Windows.Editors;
using System.Windows.Resources;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for CaptureTSRTargetsScreen.xaml
    /// </summary>
    public partial class CaptureTSRTargetsScreen
    {
        System.Globalization.DateTimeFormatInfo dtf = new System.Globalization.DateTimeFormatInfo();
        DataSet _dsLookupData = new DataSet();
        int _selectedIndex = -1;
        long _selectedAgentID = -1;
        int _selectedMonthID = -1;       
        List<int> flagList = new List<int>();
        private int _year;
        List<MonthWeekDates> _weekDates = new List<MonthWeekDates>();
        public CaptureTSRTargetsScreen()
        {
            InitializeComponent();           
        }
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);

        }

        private void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {
            SqlParameter[] parameters =  new SqlParameter[1];
            parameters[0] = new SqlParameter("@UserID", GlobalSettings.ApplicationUser.ID);
            DataSet dsLookupData = Methods.ExecuteStoredProcedure("spCaptureTSRTargetsLookupData", parameters);
            _dsLookupData = dsLookupData;
            DataTable dtAgents = dsLookupData.Tables[0];
            DataTable dtCampaigns = dsLookupData.Tables[1];
            int[] months = {1,2,3,4,5,6,7,8,9,10,11,12 };
            cmbAgents.Populate(dtAgents, "Agent", "ID");
            
            foreach (int month in months)
            {
                ComboBoxItem cmbItem = new ComboBoxItem();
                cmbItem.Content = dtf.GetMonthName(month);
                cmbItem.Tag = month;

                cmbMonth.Items.Add(cmbItem);
            }
            _year = DateTime.Now.Year;
            cmbMonth.SelectedIndex = DateTime.Now.Month - 1;
            
            
            
        }
        public class TSRTarget: INotifyPropertyChanged
        {
            private string hours;
            private string basetarget;
            private string premiumTarget;
            private Visibility accDisTypeVisibility;
            private int? accDisTypeSelectedIndex;
            private string accDisTypeSelectedItem;
         
            public event PropertyChangedEventHandler PropertyChanged;
            public TSRTarget()
            {

            }

            public List<ComboBoxItem> cmbWeek { get; set;}
            public List<ComboBoxItem> cmbCampaign { get; set; }
            public DateTime? DateFrom { get; set; }
            public DateTime? DateTo { get; set; }
            public string Hours
            {
                get { return hours; }
                set
                {
                    hours = value;
                    OnPropertyChanged("Hours");
                }
            }           
            public string BaseTarget
            {
                get { return basetarget; }
                set
                {
                    basetarget = value;
                    OnPropertyChanged("BaseTarget");
                }
            }
            
            public string PremiumTarget
            {
                get { return premiumTarget; }
                set
                {
                    premiumTarget = value;
                    OnPropertyChanged("PremiumTarget");
                }
            }

            public long? SelectedWeekID { get; set; }            
            public long? SelectedCampaignID { get; set; }
            public long? SelectedAgentID { get; set; }
            public int? CampaignSelectedIndex { get; set; }
            public int? WeekSelectedIndex { get; set; }
            public long? TargetID { get; set; }

            public Visibility AccDisTypeVisibility
            {
                get { return accDisTypeVisibility; }
                set
                {
                    accDisTypeVisibility = value;
                    OnPropertyChanged("AccDisTypeVisibility");
                }
            }
            public int? AccDisTypeSelectedIndex
            {
                get { return accDisTypeSelectedIndex; }
                set
                {
                    accDisTypeSelectedIndex = value;
                    OnPropertyChanged("AccDisTypeSelectedIndex");
                }
            }
            public string AccDisTypeSelectedItem
            {
                get { return accDisTypeSelectedItem; }
                set
                {
                    accDisTypeSelectedItem = value;
                    OnPropertyChanged("AccDisTypeSelectedItem");
                }
            }

            // Create the OnPropertyChanged method to raise the event
            protected void OnPropertyChanged(string name)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            }
        }

        private class MonthWeekDates
        {
            public int WeekID { get; set; }
            public DateTime Date { get; set; }            
        }
        
        private void AddGridRow()
        {
            _selectedIndex = -1;
            List<ComboBoxItem> lstWeeks = new List<ComboBoxItem>();
             List<ComboBoxItem> lstCampaigns = new List<ComboBoxItem>();
            DataTable dtCampaigns = _dsLookupData.Tables[1];
            DataTable dtWeeks = _dsLookupData.Tables[2];
            foreach (DataRow camp in dtCampaigns.Rows)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = camp["Campaign"].ToString();
                item.Tag = camp["ID"].ToString();
                lstCampaigns.Add(item);
            }
            foreach (DataRow week in dtWeeks.Rows)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = week["Description"].ToString();
                item.Tag = week["ID"].ToString();
                lstWeeks.Add(item);
            }

            TSRTarget tsrTarget = new TSRTarget {AccDisTypeVisibility= Visibility.Collapsed, cmbWeek = lstWeeks, cmbCampaign = lstCampaigns, DateFrom = null, DateTo = null, Hours = "0", BaseTarget = "0", PremiumTarget = "0"};
            

            dgAgentTargets.Items.Add(tsrTarget);
            dgAgentTargets.Columns[0].Width = 100;
            dgAgentTargets.Columns[1].Width = 350;
            dgAgentTargets.Columns[2].Width = 140;
            dgAgentTargets.Columns[3].Width = 140;
            dgAgentTargets.Columns[4].Width = 65;
            dgAgentTargets.Columns[5].Width = 75;
            dgAgentTargets.Columns[6].Width = 100;
            dgAgentTargets.Columns[7].Width = 120;
            
        }

        private void dgMenuItemAddNewRow_Click(object sender, RoutedEventArgs e)
        {
            AddGridRow();
        }

        private void dgAgentTargets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                _selectedIndex = dgAgentTargets.SelectedIndex;

                if (_selectedIndex != -1)
                {
                    TSRTarget target = (TSRTarget)dgAgentTargets.Items[_selectedIndex];
                    if (target.SelectedCampaignID != null)
                    {
                        SqlParameter[] parameter = new SqlParameter[1];
                        bool isUpgradeCampaign = false;
                        parameter[0] = new SqlParameter("@CampaignID", target.SelectedCampaignID);
                        DataSet dsIsUpgrade = Methods.ExecuteStoredProcedure("spDetermineIfUpgrade", parameter);
                        if (dsIsUpgrade.Tables.Count > 0)
                        {
                            isUpgradeCampaign = true;
                        }
                        else
                        {
                            isUpgradeCampaign = false;
                        }
                        if (isUpgradeCampaign == true)
                        {
                            dgAgentTargets.Columns[6].Header = "Unit Target";
                        }
                        else
                        {
                            dgAgentTargets.Columns[6].Header = "Premium Target";
                        }
                    }
                    else
                    {
                        dgAgentTargets.Columns[6].Header = "Premium Target";
                    }
                }
        }

        private void dgMenuItemDeleteRow_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedIndex > -1)
            {
                TSRTarget target = (TSRTarget)dgAgentTargets.Items[_selectedIndex];
                if (target.TargetID != null)
                {
                    bool? result = ShowMessageBox(new INMessageBoxWindow2(), "This is a Saved Target, Would you like to remove?", "Remove Target ?", ShowMessageType.Information);

                    if (result == true)
                    {
                        SqlParameter[] parameters = new SqlParameter[1];
                        parameters[0] = new SqlParameter("@TargetID", target.TargetID);                       
                        Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spRemoveTSRTarget", parameters, 600);
                        if (flagList.Contains(_selectedIndex))
                        {
                            flagList.Remove(_selectedIndex);
                        }
                        dgAgentTargets.Items.RemoveAt(_selectedIndex);
                    }
                }
                else
                {
                    if(flagList.Contains(_selectedIndex))
                    {
                        flagList.Remove(_selectedIndex);
                    }
                    dgAgentTargets.Items.RemoveAt(_selectedIndex);
                }
                
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgAgentTargets.Items.Count > 0)
                {
                    #region checks
                    if (_selectedAgentID == -1)
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "Please Select Agent", "Cannot Save!", ShowMessageType.Exclamation);
                        return;
                    }
                    if (flagList.Count > 0)
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "Please Correct All Errors", "Cannot Save!", ShowMessageType.Exclamation);
                        return;
                    }
                    
                    foreach (TSRTarget item in dgAgentTargets.Items)
                    {
                       if((item.SelectedWeekID == null) || (item.SelectedCampaignID == null))
                       {
                           ShowMessageBox(new INMessageBoxWindow1(), "Please Enter All Relevant Data", "Cannot Save!", ShowMessageType.Exclamation);
                           return;
                       }
                        //total hours must not exceed 37
                       double currentItemHours = double.Parse(item.Hours);
                       DateTime fromDate = item.DateFrom.Value;
                       DateTime toDate = item.DateTo.Value;
                       double numberOFDays = toDate.Subtract(fromDate).TotalDays;
                       numberOFDays = numberOFDays + 1;
                       double maxHours = numberOFDays * 7.5;
                       if (toDate.DayOfWeek == DayOfWeek.Friday)
                       {
                           maxHours = maxHours - 0.5;
                       }

                       if (currentItemHours > maxHours)
                       {
                           ShowMessageBox(new INMessageBoxWindow1(), "Hours Captured Exceed Week Total of " + maxHours, "Cannot Save!", ShowMessageType.Exclamation);
                           return;
                       }
                       

                    }
                    #endregion checks
                    foreach (TSRTarget item in dgAgentTargets.Items)
                    {
                       
                        TSRTargets tsrTargets = new TSRTargets();
                        if (item.TargetID != null)
                        {
                            tsrTargets = new TSRTargets(long.Parse(item.TargetID.ToString()));
                        }
                        tsrTargets.FKINWeekID = item.SelectedWeekID;
                        tsrTargets.FKINCampaignID = item.SelectedCampaignID;
                        tsrTargets.FKAgentID = _selectedAgentID;
                        tsrTargets.DateFrom = item.DateFrom;
                        tsrTargets.DateTo = item.DateTo;
                        tsrTargets.Hours = Double.Parse(item.Hours);
                        tsrTargets.BaseTarget = Double.Parse(item.BaseTarget);
                       
                        if (tsrTargets.FKINCampaignID == 8)
                        {
                            tsrTargets.AccDisSelectedItem = item.AccDisTypeSelectedItem;
                        }
                        bool isUpgradeCampaign = false;
                        SqlParameter[] parameter = new SqlParameter[1];
                        parameter[0] = new SqlParameter("@CampaignID", item.SelectedCampaignID);
                        DataSet dsIsUpgrade = Methods.ExecuteStoredProcedure("spDetermineIfUpgrade", parameter);
                        if (dsIsUpgrade.Tables.Count > 0)
                        {
                            isUpgradeCampaign = true;
                        }
                        else
                        {
                            isUpgradeCampaign = false;
                        }
                        if (isUpgradeCampaign == false)
                        {
                            tsrTargets.PremiumTarget = Double.Parse(item.PremiumTarget);
                            tsrTargets.UnitTarget = 0;
                        }
                        else
                        {
                            tsrTargets.UnitTarget = Double.Parse(item.PremiumTarget);
                            tsrTargets.PremiumTarget = 0;
                        }
                       

                        tsrTargets.Save(_validationResult);
                        item.TargetID = tsrTargets.ID;
                    }
                    ShowMessageBox(new INMessageBoxWindow1(), "Targets Succesfully Saved ", "Save result", ShowMessageType.Information);
                    RemoveAllRows();
                    LoadSavedTargets();
                }
               
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        private void cmbGridWeek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selectedIndex != -1)
            {
                //update cuurent item
                TSRTarget target = (TSRTarget)dgAgentTargets.Items[_selectedIndex];
                EmbriantComboBox cmbWeek = sender as EmbriantComboBox;
                ComboBoxItem selectedWeek = (ComboBoxItem)cmbWeek.SelectedItem;
                target.SelectedWeekID = long.Parse(selectedWeek.Tag.ToString());
                bool ValidDate = false;
                #region validate from date
                if (target.DateFrom != null)
                {
                  foreach(MonthWeekDates weekDate in _weekDates)
                  {
                      if (weekDate.Date.Date == target.DateFrom.Value.Date)
                      {
                          if (weekDate.WeekID == target.SelectedWeekID)
                          {
                              ValidDate = true;
                          }
                      }
                  }
                  if (ValidDate == false)
                  {
                      cmbWeek.Foreground = System.Windows.Media.Brushes.Red;
                      if (!flagList.Contains(_selectedIndex))
                      {
                          flagList.Add(_selectedIndex);
                      }
                  }
                  else
                  {
                      cmbWeek.Foreground = System.Windows.Media.Brushes.Black;
                      if (flagList.Contains(_selectedIndex))
                      {
                          flagList.Remove(_selectedIndex);
                      }
                  }
                }
                #endregion Validate from Date

                ValidDate = false;
                #region validate to date
                if (target.DateTo != null)
                {
                    foreach (MonthWeekDates weekDate in _weekDates)
                    {
                        if (weekDate.Date.Date == target.DateTo.Value.Date)
                        {
                            if (weekDate.WeekID == target.SelectedWeekID)
                            {
                                ValidDate = true;
                            }
                        }
                    }
                    if (ValidDate == false)
                    {
                        cmbWeek.Foreground = System.Windows.Media.Brushes.Red;
                        if (!flagList.Contains(_selectedIndex))
                        {
                            flagList.Add(_selectedIndex);
                        }
                    }
                    else
                    {
                        cmbWeek.Foreground = System.Windows.Media.Brushes.Black;
                        if (flagList.Contains(_selectedIndex))
                        {
                            flagList.Remove(_selectedIndex);
                        }
                    }
                }

                
                #endregion Validate to Date

                
            }
        }

        private void cmbGridCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selectedIndex != -1)
            {
                TSRTarget target = (TSRTarget)dgAgentTargets.Items[_selectedIndex];
                EmbriantComboBox cmbCampaign = sender as EmbriantComboBox;
                ComboBoxItem selectedCampaign = (ComboBoxItem)cmbCampaign.SelectedItem;
                target.SelectedCampaignID = long.Parse(selectedCampaign.Tag.ToString());
                if (target.SelectedCampaignID == 8)
                {
                    target.AccDisTypeVisibility = Visibility.Visible;
                    target.AccDisTypeSelectedIndex = 0;
                }
                else
                {
                    target.AccDisTypeVisibility = Visibility.Collapsed;
                    target.AccDisTypeSelectedIndex = null;
                }
                //determine if its upgrade
                SqlParameter[] parameter = new SqlParameter[1];
                bool isUpgradeCampaign = false;
                parameter[0] = new SqlParameter("@CampaignID", target.SelectedCampaignID);
                DataSet dsIsUpgrade = Methods.ExecuteStoredProcedure("spDetermineIfUpgrade", parameter);
                if (dsIsUpgrade.Tables.Count > 0)
                {
                    isUpgradeCampaign = true;
                }
                else
                {
                    isUpgradeCampaign = false;
                }
                if (isUpgradeCampaign == true)
                {
                    dgAgentTargets.Columns[6].Header = "Unit Target";
                }
                else
                {
                    dgAgentTargets.Columns[6].Header = "Premium Target";
                }
                if (target.DateFrom != null)
                {
                    DateTime fromDate1 = DateTime.Parse(target.DateFrom.Value.ToString());
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@CampaignID", target.SelectedCampaignID);
                    parameters[1] = new SqlParameter("@FromDate", fromDate1.ToString("yyy-MM-dd"));
                    string AccDisSelectedItem = target.AccDisTypeSelectedItem;
                    if (AccDisSelectedItem == null)
                    {
                        AccDisSelectedItem = "AccDisSelectedItem";
                    }
                    parameters[2] = new SqlParameter("@AccDisSelectedItem", AccDisSelectedItem);                   
                    DataTable dtSavedTargets = Methods.ExecuteStoredProcedure("spGetSavedCampaignDefaultTargets", parameters).Tables[0];
                    if (dtSavedTargets.Rows.Count > 0)
                    {
                        //calculate base target
                        
                        DataRow row = dtSavedTargets.Rows[0];                        
                        double hours = double.Parse(target.Hours);
                        double salesPerHour = double.Parse(row["SalesPerHourTarget"].ToString());

                        double baseTarget = Math.Ceiling(hours * salesPerHour);
                        double baseTargetUnrounded = Math.Round(hours * salesPerHour,3);
                        target.BaseTarget = baseTarget.ToString();
                        //calculate premium target
                        if (isUpgradeCampaign == false)
                        {
                            double basePremiumTarget = double.Parse(row["BasePremiumTarget"].ToString());
                            double childTarget = double.Parse(row["ChildTarget"].ToString());
                            double childPremiumTarget = double.Parse(row["ChildPremiumTarget"].ToString());
                            double partnerTarget = double.Parse(row["PartnerTarget"].ToString());
                            double partnerPremiumTarget = double.Parse(row["PartnerPremiumTarget"].ToString());
                            double val1 = (baseTargetUnrounded * basePremiumTarget);
                            double val2 = (/*baseTarget*/baseTargetUnrounded * (partnerTarget / 100));
                            double val4 = val2 * (partnerPremiumTarget);
                            double val3 = /*baseTarget*/baseTargetUnrounded * (childTarget / 100);
                            double val5 = val3 * (childPremiumTarget);
                            double premiumTarget = val1 + val4 + val5;

                            target.PremiumTarget = Math.Round(premiumTarget,2).ToString();
                        }
                        else
                        {
                            //calculate unit target
                            double unitTargetVal = double.Parse(row["BaseUnitTarget"].ToString());
                            double unitTargets = (hours * salesPerHour) * unitTargetVal;
                            target.PremiumTarget = Math.Ceiling(unitTargets).ToString();
                        }
                    }
                }
            }
        }

        private void dteGridDateFrom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (_selectedIndex != -1)
            {
               
                TSRTarget target = (TSRTarget)dgAgentTargets.Items[_selectedIndex];
                XamDateTimeEditor medDateFrom = sender as XamDateTimeEditor;
                DateTime value = DateTime.Parse(medDateFrom.Value.ToString());
                if (value.Month != _selectedMonthID || value.Year != _year)
                {
                    medDateFrom.Background = System.Windows.Media.Brushes.Red;
                    if (!flagList.Contains(_selectedIndex))
                    {
                        flagList.Add(_selectedIndex);
                    }
                }
                else
                {
                    medDateFrom.Background = null;
                    if (flagList.Contains(_selectedIndex))
                    {
                        flagList.Remove(_selectedIndex);
                    }
                }
                target.DateFrom = value;

                //validate weeks and dates
                bool ValidDate = false;
                #region validate from date
                if (target.DateFrom != null)
                {
                    foreach (MonthWeekDates weekDate in _weekDates)
                    {
                        if (weekDate.Date.Date == target.DateFrom.Value.Date)
                        {
                            if (weekDate.WeekID == target.SelectedWeekID)
                            {
                                ValidDate = true;
                            }
                        }
                    }
                    if (ValidDate == false)
                    {
                        medDateFrom.Foreground = System.Windows.Media.Brushes.Red;
                        if (!flagList.Contains(_selectedIndex))
                        {
                            flagList.Add(_selectedIndex);
                        }
                    }
                    else
                    {
                        medDateFrom.Foreground = System.Windows.Media.Brushes.Black;
                        if (flagList.Contains(_selectedIndex))
                        {
                            flagList.Remove(_selectedIndex);
                        }
                    }
                }
                #endregion Validate from Date
                //update hours
                double currentItemHours = double.Parse(target.Hours);
                if (target.DateFrom != null && target.DateTo != null)
                {
                    DateTime fromDate = target.DateFrom.Value;
                    DateTime toDate = target.DateTo.Value;
                    double numberOFDays = toDate.Subtract(fromDate).TotalDays;
                    numberOFDays = numberOFDays + 1;
                    double maxHours = numberOFDays * 7.5;
                    if (toDate.DayOfWeek == DayOfWeek.Friday)
                    {
                        maxHours = maxHours - 0.5;
                    }
                    target.Hours = maxHours.ToString();
                }
               

            }
        }

        private void dteGridDateTo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (_selectedIndex != -1)
            {
                TSRTarget target = (TSRTarget)dgAgentTargets.Items[_selectedIndex];
                XamDateTimeEditor medDateTo = sender as XamDateTimeEditor;
                DateTime value = DateTime.Parse(medDateTo.Value.ToString());
                if (value.Month != _selectedMonthID || value.Year != _year)
                {
                    medDateTo.Background = System.Windows.Media.Brushes.Red;
                    if (!flagList.Contains(_selectedIndex))
                    {
                        flagList.Add(_selectedIndex);
                    }
                }
                else
                {
                    medDateTo.Background = null;
                    if (flagList.Contains(_selectedIndex))
                    {
                        flagList.Remove(_selectedIndex);
                    }
                }
                //check if selected week contains
                target.DateTo = value;

               bool ValidDate = false;
                #region validate to date
                if (target.DateTo != null)
                {
                    foreach (MonthWeekDates weekDate in _weekDates)
                    {
                        if (weekDate.Date.Date == target.DateTo.Value.Date)
                        {
                            if (weekDate.WeekID == target.SelectedWeekID)
                            {
                                ValidDate = true;
                            }
                        }
                    }
                    if (ValidDate == false)
                    {
                        medDateTo.Foreground = System.Windows.Media.Brushes.Red;
                        if (!flagList.Contains(_selectedIndex))
                        {
                            flagList.Add(_selectedIndex);
                        }
                    }
                    else
                    {
                        medDateTo.Foreground = System.Windows.Media.Brushes.Black;
                        if (flagList.Contains(_selectedIndex))
                        {
                            flagList.Remove(_selectedIndex);
                        }
                    }
                }
                
                #endregion Validate to Date

                //update hours
                double currentItemHours = double.Parse(target.Hours);
                if (target.DateFrom != null && target.DateTo != null)
                {
                    DateTime fromDate = target.DateFrom.Value;
                    DateTime toDate = target.DateTo.Value;
                    double numberOFDays = toDate.Subtract(fromDate).TotalDays;
                    numberOFDays = numberOFDays + 1;
                    double maxHours = numberOFDays * 7.5;
                    if (toDate.DayOfWeek == DayOfWeek.Friday)
                    {
                        maxHours = maxHours - 0.5;
                    }
                    target.Hours = maxHours.ToString();
                }
               // RebindGrid();

            }
        }

        private void txtHours_TextChanged(object sender, TextChangedEventArgs e)
        {
          if (_selectedIndex != -1)
            {
                TSRTarget target = (TSRTarget)dgAgentTargets.Items[_selectedIndex];
                TextBox txtHours = sender as TextBox;
              //validate if is integer
                string contentString = txtHours.Text;
                foreach (char character in contentString)
                {
                    if (char.IsLetter(character) == true)
                    {
                        txtHours.Text = "0";
                        break;
                    }
                }
                if (txtHours.Text == string.Empty)
                {
                    txtHours.Text = "0";
                }
                target.Hours = txtHours.Text;
              //check if current week already exists in tsr targets
                double weekHours = 0;
                foreach (TSRTarget tTarget in dgAgentTargets.Items)
                {
                    if (tTarget.SelectedWeekID == target.SelectedWeekID)
                    {
                        weekHours = weekHours + double.Parse(tTarget.Hours);
                    }
                }

                DateTime toDate = new DateTime();
                int dayCount = 1;
                foreach (MonthWeekDates md in _weekDates)
                {
                    if (md.Date.DayOfWeek == DayOfWeek.Friday)
                    {
                        if (md.WeekID == target.SelectedWeekID)
                        {
                            toDate = md.Date;
                            break;
                        }
                    }
                    if (dayCount == _weekDates.Count)
                    {
                        toDate = md.Date;
                    }
                    dayCount++;
                }
                DateTime fromDate = new DateTime();
                foreach (MonthWeekDates md in _weekDates)
                {
                        if (md.WeekID == target.SelectedWeekID)
                        {
                            fromDate = md.Date;
                            break;
                        }
                }

                double numberOFDays = toDate.Subtract(fromDate).TotalDays;
                numberOFDays = numberOFDays + 1;
                double maxHours = numberOFDays * 7.5;
                if (toDate.DayOfWeek == DayOfWeek.Friday)
                {
                    maxHours = maxHours - 0.5;
                }

                if (weekHours > maxHours)
                {
                    txtHours.Background = System.Windows.Media.Brushes.Red;
                    if (!flagList.Contains(_selectedIndex))
                    {
                        flagList.Add(_selectedIndex);
                    }
                }
                else
                {
                    txtHours.Background = null;
                    if (flagList.Contains(_selectedIndex))
                    {
                        flagList.Remove(_selectedIndex);
                    }
                }

                if (target.SelectedCampaignID != null)
                {
                    DateTime fromDate1 = DateTime.Parse(target.DateFrom.Value.ToString());
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@CampaignID", target.SelectedCampaignID);
                    parameters[1] = new SqlParameter("@FromDate", fromDate1.ToString("yyy-MM-dd"));
                    string AccDisSelectedItem = target.AccDisTypeSelectedItem;
                    if (AccDisSelectedItem == null)
                    {
                        AccDisSelectedItem = "AccDisSelectedItem";
                    }
                    parameters[2] = new SqlParameter("@AccDisSelectedItem", AccDisSelectedItem);
                   
                    DataTable dtSavedTargets = Methods.ExecuteStoredProcedure("spGetSavedCampaignDefaultTargets", parameters).Tables[0];
                    if (dtSavedTargets.Rows.Count > 0)
                    {
                        //determine if its upgrade
                        SqlParameter[] parameter = new SqlParameter[1];
                        bool isUpgradeCampaign = false;
                        parameter[0] = new SqlParameter("@CampaignID", target.SelectedCampaignID);
                        DataSet dsIsUpgrade = Methods.ExecuteStoredProcedure("spDetermineIfUpgrade", parameter);
                        if (dsIsUpgrade.Tables.Count > 0)
                        {
                            isUpgradeCampaign = true;
                        }
                        else
                        {
                            isUpgradeCampaign = false;
                        }

                        //calculate base target

                        DataRow row = dtSavedTargets.Rows[0];
                        double hours = double.Parse(target.Hours);
                        double salesPerHour = double.Parse(row["SalesPerHourTarget"].ToString());

                        double baseTarget = Math.Ceiling(hours * salesPerHour);
                        double baseTargetUnrounded = Math.Round(hours * salesPerHour, 3);
                        target.BaseTarget = baseTarget.ToString();
                        //calculate premium target
                        //calculate premium target
                        if (isUpgradeCampaign == false)
                        {
                            double basePremiumTarget = double.Parse(row["BasePremiumTarget"].ToString());
                            double childTarget = double.Parse(row["ChildTarget"].ToString());
                            double childPremiumTarget = double.Parse(row["ChildPremiumTarget"].ToString());
                            double partnerTarget = double.Parse(row["PartnerTarget"].ToString());
                            double partnerPremiumTarget = double.Parse(row["PartnerPremiumTarget"].ToString());
                            double val1 = (baseTargetUnrounded * basePremiumTarget);
                            double val2 = (/*baseTarget*/baseTargetUnrounded * (partnerTarget / 100));
                            double val4 = val2 * (partnerPremiumTarget);
                            double val3 = /*baseTarget*/baseTargetUnrounded * (childTarget / 100);
                            double val5 = val3 * (childPremiumTarget);
                            double premiumTarget = val1 + val4 + val5;

                            target.PremiumTarget = Math.Round(premiumTarget, 2).ToString();
                        }
                        else
                        {
                            //calculate unit target
                            double unitTargetVal = double.Parse(row["BaseUnitTarget"].ToString());
                            double unitTargets = (hours * salesPerHour) * unitTargetVal;
                            target.PremiumTarget = Math.Ceiling(unitTargets).ToString();
                        }
                    }
                }



                
            }
        }

        private void txtBaseTarget_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_selectedIndex != -1)
            {
                TSRTarget target = (TSRTarget)dgAgentTargets.Items[_selectedIndex];
                TextBox txtBaseTarget = sender as TextBox;
                string contentString = txtBaseTarget.Text;
                foreach (char character in contentString)
                {
                    if (char.IsLetter(character) == true)
                    {
                        txtBaseTarget.Text = "0";
                        break;
                    }
                }
                target.BaseTarget = txtBaseTarget.Text;
            }
        }

        private void txtPremiumTarget_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_selectedIndex != -1)
            {
                TSRTarget target = (TSRTarget)dgAgentTargets.Items[_selectedIndex];
                TextBox txtPremiumTarget = sender as TextBox;
                string contentString = txtPremiumTarget.Text;
                foreach (char character in contentString)
                {
                    if (char.IsLetter(character) == true)
                    {
                        txtPremiumTarget.Text = "0";
                        break;
                    }
                }
                target.PremiumTarget = txtPremiumTarget.Text;
            }
        }

        private void cmbAgents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedAgentID = long.Parse(cmbAgents.SelectedValue.ToString());
            RemoveAllRows();
            LoadSavedTargets();
        }

        private void cmbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //
            ComboBoxItem selectedMonth = (ComboBoxItem)cmbMonth.SelectedItem;
            _selectedMonthID = int.Parse(selectedMonth.Tag.ToString());
            //get list of weeks
            List<MonthWeekDates> weekDates = new List<MonthWeekDates>();
            //Dates (get an array of dates between from and to date)
            DateTime fromDate = new DateTime(_year, _selectedMonthID, 1);
            DateTime toDate = new DateTime(_year, _selectedMonthID, DateTime.DaysInMonth(_year, _selectedMonthID));
            int numberOFDaysBetween = int.Parse(toDate.Subtract(fromDate).TotalDays.ToString());
            int weekIndex = 1;
            for (int i = 0; i <= numberOFDaysBetween; i++)
            {
                DateTime newDate = fromDate.AddDays(i);
                if (newDate.DayOfWeek != DayOfWeek.Saturday && newDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    MonthWeekDates monthWeekDate = new MonthWeekDates();
                    monthWeekDate.WeekID = weekIndex;
                    monthWeekDate.Date = newDate;
                    weekDates.Add(monthWeekDate);
                    if (newDate.DayOfWeek == DayOfWeek.Friday)
                    {
                        weekIndex++;
                    }
                }

            }
            _weekDates = weekDates;
            RemoveAllRows();
            LoadSavedTargets();
        }
        private void LoadSavedTargets()
        {
            RemoveAllRows();
            if (_selectedAgentID != -1 && _selectedMonthID != -1)
            {
                DateTime fromDate = new DateTime(_year, _selectedMonthID, 1);
                DateTime toDate = new DateTime(_year, _selectedMonthID, DateTime.DaysInMonth(_year, _selectedMonthID));
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@AgentID", _selectedAgentID);
                parameters[1] = new SqlParameter("@DateFrom", fromDate.ToString("yyy-MM-dd"));
                parameters[2] = new SqlParameter("@DateTo", toDate.ToString("yyy-MM-dd"));
                DataTable dtTSrTargets = Methods.ExecuteStoredProcedure("spGetSavedTSRTargets", parameters).Tables[0];
                if (dtTSrTargets.Rows.Count > 0)
                {
                    RemoveAllRows();
                    foreach (DataRow rw in dtTSrTargets.Rows)
                    {
                        List<ComboBoxItem> lstWeeks = new List<ComboBoxItem>();
                        List<ComboBoxItem> lstCampaigns = new List<ComboBoxItem>();
                        DataTable dtCampaigns = _dsLookupData.Tables[1];
                        DataTable dtWeeks = _dsLookupData.Tables[2];
                        int currentIndex = 0;
                        long selectedCampaignID = -1;
                        int campaignSelectedIndex = -1;
                        long selectWeekID = -1;
                        int weekSelectedIndex = -1;
                        foreach (DataRow camp in dtCampaigns.Rows)
                        {
                            ComboBoxItem item = new ComboBoxItem();
                            item.Content = camp["Campaign"].ToString();
                            item.Tag = camp["ID"].ToString();
                            
                            if (camp["ID"].ToString() == rw["FKINCampaignID"].ToString())
                            {
                                selectedCampaignID = long.Parse(camp["ID"].ToString());
                                campaignSelectedIndex = currentIndex;
                            }
                            lstCampaigns.Add(item);

                            
                            currentIndex++;
                        }
                        currentIndex = 0;
                        foreach (DataRow week in dtWeeks.Rows)
                        {
                            ComboBoxItem item = new ComboBoxItem();
                            item.Content = week["Description"].ToString();
                            item.Tag = week["ID"].ToString();
                            if (week["ID"].ToString() == rw["FKINWeekID"].ToString())
                            {
                                selectWeekID = long.Parse(week["ID"].ToString());
                                weekSelectedIndex = currentIndex;
                            }
                            lstWeeks.Add(item);
                            currentIndex++;
                        }
                        long? targetID = long.Parse(rw["ID"].ToString());

                       
                        
                        SqlParameter[] parameter = new SqlParameter[1];
                        bool isUpgradeCampaign = false;
                        parameter[0] = new SqlParameter("@CampaignID", selectedCampaignID);
                        DataSet dsIsUpgrade = Methods.ExecuteStoredProcedure("spDetermineIfUpgrade", parameter);
                        if (dsIsUpgrade.Tables.Count > 0)
                        {
                            isUpgradeCampaign = true;
                        }
                        else
                        {
                            isUpgradeCampaign = false;
                        }
                        string premiumTarget = string.Empty;
                        if (isUpgradeCampaign == true)
                        {
                            premiumTarget = rw["UnitTarget"].ToString();
                        }
                        else
                        {
                            premiumTarget = rw["PremiumTarget"].ToString();
                        }


                        if (selectedCampaignID != 8)
                        {
                           
                            TSRTarget tsrTarget = new TSRTarget
                            {
                                cmbWeek = lstWeeks,
                                cmbCampaign = lstCampaigns,
                                DateFrom = DateTime.Parse(rw["DateFrom"].ToString()),
                                DateTo = DateTime.Parse(rw["DateTo"].ToString()),
                                Hours = rw["Hours"].ToString(),
                                BaseTarget = rw["BaseTarget"].ToString(),
                                PremiumTarget = premiumTarget
                                ,
                                SelectedCampaignID = selectedCampaignID,
                                CampaignSelectedIndex = campaignSelectedIndex,
                                SelectedWeekID = selectWeekID,
                                WeekSelectedIndex = weekSelectedIndex,
                                TargetID = targetID,
                                AccDisTypeVisibility = Visibility.Collapsed
                                
                            };
                            dgAgentTargets.Items.Add(tsrTarget);
                        }
                        else
                        {
                            int accDisSelectedIndex = -1;
                            string accDisValue = rw["AccDisSelectedItem"].ToString();
                            
                            if (accDisValue.ToLower() == "macc")
                            {
                                accDisSelectedIndex = 0;
                            }
                            if (accDisValue.ToLower() == "macc million")
                            {
                                accDisSelectedIndex = 1;
                            }
                            

                            TSRTarget tsrTarget = new TSRTarget
                            {
                                cmbWeek = lstWeeks,
                                cmbCampaign = lstCampaigns,
                                DateFrom = DateTime.Parse(rw["DateFrom"].ToString()),
                                DateTo = DateTime.Parse(rw["DateTo"].ToString()),
                                Hours = rw["Hours"].ToString(),
                                BaseTarget = rw["BaseTarget"].ToString(),
                                PremiumTarget = premiumTarget
                                ,
                                SelectedCampaignID = selectedCampaignID,
                                CampaignSelectedIndex = campaignSelectedIndex,
                                SelectedWeekID = selectWeekID,
                                WeekSelectedIndex = weekSelectedIndex,
                                TargetID = targetID,
                                AccDisTypeVisibility = Visibility.Visible,
                                AccDisTypeSelectedItem = rw["AccDisSelectedItem"].ToString(),
                                AccDisTypeSelectedIndex = accDisSelectedIndex,
                                
                            };
                            dgAgentTargets.Items.Add(tsrTarget);
                        }
                        
                        
                    }
                }
                else
                {
                    RemoveAllRows();
                    LoadDefaultEmptyRows();
                }
            }
            else
            {
                RemoveAllRows();
               // AddGridRow();
                LoadDefaultEmptyRows();
            }
        }

        private void RemoveAllRows()
        {
            if (dgAgentTargets.Items.Count > 0)
            {
                for (int i = 0; i < dgAgentTargets.Items.Count; i++)
                {
                    dgAgentTargets.Items.RemoveAt(i);
                }
            }
        }

        private void LoadDefaultEmptyRows()
        {
            _selectedIndex = -1;
            RemoveAllRows();
            List<int> addedWeekIDs = new List<int>();
            foreach (MonthWeekDates monthWeekDay in _weekDates)
            {
                if (!addedWeekIDs.Contains(monthWeekDay.WeekID))
                {
                    List<ComboBoxItem> lstWeeks = new List<ComboBoxItem>();
                    List<ComboBoxItem> lstCampaigns = new List<ComboBoxItem>();
                    DataTable dtCampaigns = _dsLookupData.Tables[1];
                    DataTable dtWeeks = _dsLookupData.Tables[2];
                    foreach (DataRow camp in dtCampaigns.Rows)
                    {
                        ComboBoxItem item = new ComboBoxItem();
                        item.Content = camp["Campaign"].ToString();
                        item.Tag = camp["ID"].ToString();
                        lstCampaigns.Add(item);
                    }
                    int currentWeekIndex = 0;
                    int selectedWeekIndex = -1;
                    foreach (DataRow week in dtWeeks.Rows)
                    {
                        ComboBoxItem item = new ComboBoxItem();
                        item.Content = week["Description"].ToString();
                        item.Tag = week["ID"].ToString();
                        if (monthWeekDay.WeekID.ToString() == week["ID"].ToString())
                        {
                            selectedWeekIndex = currentWeekIndex;
                        }
                        lstWeeks.Add(item);
                        currentWeekIndex++;
                    }
                    DateTime toDate = new DateTime();
                    int dayCount = 1;
                    foreach (MonthWeekDates md in _weekDates)
                    {
                        if (md.Date.DayOfWeek == DayOfWeek.Friday)
                        {
                            if (md.WeekID == monthWeekDay.WeekID)
                            {
                                toDate = md.Date;
                                break;
                            }
                        }
                        if (dayCount == _weekDates.Count)
                        {
                            toDate = md.Date;
                        }
                        dayCount++;
                    }

                    DateTime fromDate = monthWeekDay.Date;                   
                    double numberOFDays = toDate.Subtract(fromDate).TotalDays;
                    numberOFDays = numberOFDays + 1;
                    double maxHours = numberOFDays * 7.5;
                    if (toDate.DayOfWeek == DayOfWeek.Friday)
                    {
                        maxHours = maxHours - 0.5;
                    }
                    TSRTarget tsrTarget = new TSRTarget
                    {
                        cmbWeek = lstWeeks,
                        cmbCampaign = lstCampaigns,
                        DateFrom = monthWeekDay.Date,
                        DateTo = toDate,
                        Hours = maxHours.ToString(),
                        BaseTarget = "0",
                        PremiumTarget = "0",
                        AccDisTypeVisibility = System.Windows.Visibility.Collapsed,
                        SelectedWeekID = monthWeekDay.WeekID,
                        WeekSelectedIndex = selectedWeekIndex
                    };

                    dgAgentTargets.Items.Add(tsrTarget);
                    addedWeekIDs.Add(monthWeekDay.WeekID);
                }
            }
            dgAgentTargets.Columns[0].Width = 100;
            dgAgentTargets.Columns[1].Width = 350;
            dgAgentTargets.Columns[2].Width = 140;
            dgAgentTargets.Columns[3].Width = 140;
            dgAgentTargets.Columns[4].Width = 65;
            dgAgentTargets.Columns[5].Width = 75;
            dgAgentTargets.Columns[6].Width = 100;
            dgAgentTargets.Columns[7].Width = 120;
            
        }

        private void clYear_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            try
            {
                _year = clYear.DisplayDate.Year;

                if (_selectedMonthID != -1)
                {
                    List<MonthWeekDates> weekDates = new List<MonthWeekDates>();
                    //Dates (get an array of dates between from and to date)
                    DateTime fromDate = new DateTime(_year, _selectedMonthID, 1);
                    DateTime toDate = new DateTime(_year, _selectedMonthID, DateTime.DaysInMonth(_year, _selectedMonthID));
                    int numberOFDaysBetween = int.Parse(toDate.Subtract(fromDate).TotalDays.ToString());
                    int weekIndex = 1;
                    for (int i = 0; i <= numberOFDaysBetween; i++)
                    {
                        DateTime newDate = fromDate.AddDays(i);
                        if (newDate.DayOfWeek != DayOfWeek.Saturday && newDate.DayOfWeek != DayOfWeek.Sunday)
                        {
                            MonthWeekDates monthWeekDate = new MonthWeekDates();
                            monthWeekDate.WeekID = weekIndex;
                            monthWeekDate.Date = newDate;
                            weekDates.Add(monthWeekDate);
                            if (newDate.DayOfWeek == DayOfWeek.Friday)
                            {
                                weekIndex++;
                            }
                        }

                    }
                    _weekDates = weekDates;
                    RemoveAllRows();
                    LoadSavedTargets();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selectedIndex != -1)
            {

                TSRTarget target = (TSRTarget)dgAgentTargets.Items[_selectedIndex];
                EmbriantComboBox cmbType = sender as EmbriantComboBox;
                target.AccDisTypeSelectedIndex = cmbType.SelectedIndex;
                ComboBoxItem selectedItem = (ComboBoxItem)cmbType.SelectedItem;
                if (cmbType.SelectedItem != null)
                {
                    target.AccDisTypeSelectedItem = selectedItem.Content.ToString();
                }

                if (target.SelectedCampaignID != null)
                {
                  
                    //determine if its upgrade
                    SqlParameter[] parameter = new SqlParameter[1];
                    bool isUpgradeCampaign = false;
                    parameter[0] = new SqlParameter("@CampaignID", target.SelectedCampaignID);
                    DataSet dsIsUpgrade = Methods.ExecuteStoredProcedure("spDetermineIfUpgrade", parameter);
                    if (dsIsUpgrade.Tables.Count > 0)
                    {
                        isUpgradeCampaign = true;
                    }
                    else
                    {
                        isUpgradeCampaign = false;
                    }
                    if (isUpgradeCampaign == true)
                    {
                        dgAgentTargets.Columns[6].Header = "Unit Target";
                    }
                    else
                    {
                        dgAgentTargets.Columns[6].Header = "Premium Target";
                    }
                    if (target.DateFrom != null)
                    {
                        DateTime fromDate1 = DateTime.Parse(target.DateFrom.Value.ToString());
                        SqlParameter[] parameters = new SqlParameter[3];
                        parameters[0] = new SqlParameter("@CampaignID", target.SelectedCampaignID);
                        parameters[1] = new SqlParameter("@FromDate", fromDate1.ToString("yyy-MM-dd"));
                        parameters[2] = new SqlParameter("@AccDisSelectedItem", target.AccDisTypeSelectedItem);
                        
                        DataTable dtSavedTargets = Methods.ExecuteStoredProcedure("spGetSavedCampaignDefaultTargets", parameters).Tables[0];
                        if (dtSavedTargets.Rows.Count > 0)
                        {


                            //calculate base target

                            DataRow row = dtSavedTargets.Rows[0];
                            double hours = double.Parse(target.Hours);
                            double salesPerHour = double.Parse(row["SalesPerHourTarget"].ToString());

                            double baseTarget = Math.Ceiling(hours * salesPerHour);
                            double baseTargetUnrounded = Math.Round(hours * salesPerHour, 3);
                            target.BaseTarget = baseTarget.ToString();
                            //calculate premium target
                            if (isUpgradeCampaign == false)
                            {
                                double basePremiumTarget = double.Parse(row["BasePremiumTarget"].ToString());
                                double childTarget = double.Parse(row["ChildTarget"].ToString());
                                double childPremiumTarget = double.Parse(row["ChildPremiumTarget"].ToString());
                                double partnerTarget = double.Parse(row["PartnerTarget"].ToString());
                                double partnerPremiumTarget = double.Parse(row["PartnerPremiumTarget"].ToString());
                                double val1 = (baseTargetUnrounded * basePremiumTarget);
                                double val2 = (/*baseTarget*/baseTargetUnrounded * (partnerTarget / 100));
                                double val4 = val2 * (partnerPremiumTarget);
                                double val3 = /*baseTarget*/baseTargetUnrounded * (childTarget / 100);
                                double val5 = val3 * (childPremiumTarget);
                                double premiumTarget = val1 + val4 + val5;

                                target.PremiumTarget = Math.Round(premiumTarget, 2).ToString();
                            }
                            else
                            {
                                //calculate unit target
                                double unitTargetVal = double.Parse(row["BaseUnitTarget"].ToString());
                                double unitTargets = (hours * salesPerHour) * unitTargetVal;
                                target.PremiumTarget = Math.Ceiling(unitTargets).ToString();
                            }
                        }
                        else
                        {
                            target.PremiumTarget = "0";
                            target.BaseTarget = "0";
                        }
                    }

                }

            }
        }

        private void cmbMining_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selectedIndex != -1)
            {

                TSRTarget target = (TSRTarget)dgAgentTargets.Items[_selectedIndex];
                EmbriantComboBox cmbMining = sender as EmbriantComboBox;
               

                if (target.SelectedCampaignID != null)
                {

                    //determine if its upgrade
                    SqlParameter[] parameter = new SqlParameter[1];
                    bool isUpgradeCampaign = false;
                    parameter[0] = new SqlParameter("@CampaignID", target.SelectedCampaignID);
                    DataSet dsIsUpgrade = Methods.ExecuteStoredProcedure("spDetermineIfUpgrade", parameter);
                    if (dsIsUpgrade.Tables.Count > 0)
                    {
                        isUpgradeCampaign = true;
                    }
                    else
                    {
                        isUpgradeCampaign = false;
                    }
                    if (isUpgradeCampaign == true)
                    {
                        dgAgentTargets.Columns[6].Header = "Unit Target";
                    }
                    else
                    {
                        dgAgentTargets.Columns[6].Header = "Premium Target";
                    }
                    if (target.DateFrom != null)
                    {
                        string accDissSelectedItem = target.AccDisTypeSelectedItem;
                        if (accDissSelectedItem == null)
                        {
                            accDissSelectedItem = "AccDisSelectedItem";
                        }
                        DateTime fromDate1 = DateTime.Parse(target.DateFrom.Value.ToString());
                        SqlParameter[] parameters = new SqlParameter[3];
                        parameters[0] = new SqlParameter("@CampaignID", target.SelectedCampaignID);
                        parameters[1] = new SqlParameter("@FromDate", fromDate1.ToString("yyy-MM-dd"));
                        parameters[2] = new SqlParameter("@AccDisSelectedItem", accDissSelectedItem);
                        
                        DataTable dtSavedTargets = Methods.ExecuteStoredProcedure("spGetSavedCampaignDefaultTargets", parameters).Tables[0];
                        if (dtSavedTargets.Rows.Count > 0)
                        {


                            //calculate base target

                            DataRow row = dtSavedTargets.Rows[0];
                            double hours = double.Parse(target.Hours);
                            double salesPerHour = double.Parse(row["SalesPerHourTarget"].ToString());

                            double baseTarget = Math.Ceiling(hours * salesPerHour);
                            double baseTargetUnrounded = Math.Round(hours * salesPerHour, 3);
                            target.BaseTarget = baseTarget.ToString();
                            //calculate premium target
                            if (isUpgradeCampaign == false)
                            {
                                double basePremiumTarget = double.Parse(row["BasePremiumTarget"].ToString());
                                double childTarget = double.Parse(row["ChildTarget"].ToString());
                                double childPremiumTarget = double.Parse(row["ChildPremiumTarget"].ToString());
                                double partnerTarget = double.Parse(row["PartnerTarget"].ToString());
                                double partnerPremiumTarget = double.Parse(row["PartnerPremiumTarget"].ToString());
                                double val1 = (baseTargetUnrounded * basePremiumTarget);
                                double val2 = (/*baseTarget*/baseTargetUnrounded * (partnerTarget / 100));
                                double val4 = val2 * (partnerPremiumTarget);
                                double val3 = /*baseTarget*/baseTargetUnrounded * (childTarget / 100);
                                double val5 = val3 * (childPremiumTarget);
                                double premiumTarget = val1 + val4 + val5;

                                target.PremiumTarget = Math.Round(premiumTarget, 2).ToString();
                            }
                            else
                            {
                                //calculate unit target
                                double unitTargetVal = double.Parse(row["BaseUnitTarget"].ToString());
                                double unitTargets = (hours * salesPerHour) * unitTargetVal;
                                target.PremiumTarget = Math.Ceiling(unitTargets).ToString();
                            }
                        }
                        else
                        {
                            target.PremiumTarget = "0";
                            target.BaseTarget = "0";
                        }
                    }

                }

            }
        }

        
      
       
      

     






    }
}
