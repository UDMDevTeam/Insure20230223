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
    /// Interaction logic for CaptureCampaignDefaultTargets.xaml
    /// </summary>
    public partial class CaptureCampaignTargetDefaults
    {
        System.Globalization.DateTimeFormatInfo dtf = new System.Globalization.DateTimeFormatInfo();
        DataSet _dsLookupData = new DataSet();
        int _selectedIndex = -1;
        long _selectCampaignID = -1;
        bool _isUpgradeCampaign = false;
        public CaptureCampaignTargetDefaults()
        {
            InitializeComponent();
        }
        private void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@UserID", GlobalSettings.ApplicationUser.ID);
                DataSet dsLookupData = Methods.ExecuteStoredProcedure("spCaptureTSRTargetsLookupData", parameters);
                _dsLookupData = dsLookupData;
                DataTable dtCampaigns = dsLookupData.Tables[3];
                cmbCampaigns.Populate(dtCampaigns, "Name", "ID");
                AddGridRow();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

       
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (dgCampaignTargets.Items.Count > 0)
                {
                    #region checks
                    if (_selectCampaignID == -1)
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "Please Select Campaign", "Cannot Save!", ShowMessageType.Exclamation);
                        return;
                    }
                    #endregion checks

                    #region Save objects
                    foreach (CampaignTarget target in dgCampaignTargets.Items)
                    {
                        TSRCampaignTargetDefaults tsrCampaignTargetDefaults = new TSRCampaignTargetDefaults();
                        if (target.TargetID != null)
                        {
                            tsrCampaignTargetDefaults = new TSRCampaignTargetDefaults(long.Parse(target.TargetID.ToString()));
                        }                        
                        tsrCampaignTargetDefaults.FKINCampaignClusterID = _selectCampaignID;
                        tsrCampaignTargetDefaults.SalesPerHourTarget = double.Parse(target.SalesPerHourTarget);
                        tsrCampaignTargetDefaults.BasePremiumTarget = decimal.Parse(target.BasePremiumTarget);
                        tsrCampaignTargetDefaults.PartnerPremiumTarget = decimal.Parse(target.PartnerPremiumTarget);
                        tsrCampaignTargetDefaults.ChildPremiumTarget = decimal.Parse(target.ChildPremiumTarget);
                        tsrCampaignTargetDefaults.PartnerTarget = double.Parse(target.PartnerTarget);
                        tsrCampaignTargetDefaults.ChildTarget = double.Parse(target.ChildTarget);
                        tsrCampaignTargetDefaults.DateApplicableFrom = target.DateApplicableFrom;
                        tsrCampaignTargetDefaults.BaseUnitTarget = double.Parse(target.BaseUnitTarget);                        
                        if (_selectCampaignID == 5)
                        {
                            tsrCampaignTargetDefaults.AccDisSelectedItem = target.AccDissTypeSelectedItem;
                        }
                        tsrCampaignTargetDefaults.Save(_validationResult);
                        target.TargetID = tsrCampaignTargetDefaults.ID;
                    }
                    ShowMessageBox(new INMessageBoxWindow1(), "Targets Succesfully Saved ", "Save result", ShowMessageType.Information);
                    #endregion Save Objects

                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }


        public class CampaignTarget : INotifyPropertyChanged
        {
            private string salesPerHourTarget;
            private string basePremiumTarget;
            private string partnerPremiumTarget;
            private string childPremiumTarget;
            private string partnerTarget;
            private string childTarget;
            private string baseUnitTarget;            
            private DateTime dateApplicableFrom;
            private long? targetID;
            private string accDissTypeSelectedItem;
            private int? accDissTypeSelectedIndex;


            public event PropertyChangedEventHandler PropertyChanged;
            public CampaignTarget()
            {

            }
            public string SalesPerHourTarget
            {
                get { return salesPerHourTarget; }
                set
                {
                    salesPerHourTarget = value;
                    OnPropertyChanged("SalesPerHourTarget");
                }
            }

            public string BasePremiumTarget
            {
                get { return basePremiumTarget; }
                set
                {
                    basePremiumTarget = value;
                    OnPropertyChanged("BasePremiumTarget");
                }
            }

            public string PartnerPremiumTarget
            {
                get { return partnerPremiumTarget; }
                set
                {
                    partnerPremiumTarget = value;
                    OnPropertyChanged("PartnerPremiumTarget");
                }
            }

            public string ChildPremiumTarget
            {
                get { return childPremiumTarget; }
                set
                {
                    childPremiumTarget = value;
                    OnPropertyChanged("ChildPremiumTarget");
                }
            }

            public string PartnerTarget
            {
                get { return partnerTarget; }
                set
                {
                    partnerTarget = value;
                    OnPropertyChanged("PartnerTarget");
                }
            }

            public string ChildTarget
            {
                get { return childTarget; }
                set
                {
                    childTarget = value;
                    OnPropertyChanged("ChildTarget");
                }
            }

            public DateTime DateApplicableFrom
            {
                get { return dateApplicableFrom; }
                set
                {
                    dateApplicableFrom = value;
                    OnPropertyChanged("DateApplicableFrom");
                }
            }
            public long? TargetID
            {
                get { return targetID; }
                set
                {
                    targetID = value;
                    OnPropertyChanged("TargetID");
                }
            }

            public string BaseUnitTarget
            {
                get { return baseUnitTarget; }
                set
                {
                    baseUnitTarget = value;
                    OnPropertyChanged("BaseUnitTarget");
                }
            }

            public string AccDissTypeSelectedItem
            {
                get { return accDissTypeSelectedItem; }
                set
                {
                    accDissTypeSelectedItem = value;
                    OnPropertyChanged("AccDissTypeSelectedItem");
                }
            }

            public int? AccDissTypeSelectedIndex
            {
                get { return accDissTypeSelectedIndex; }
                set
                {
                    accDissTypeSelectedIndex = value;
                    OnPropertyChanged("AccDissTypeSelectedIndex");
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

        private void dgCampaignTargets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedIndex = dgCampaignTargets.SelectedIndex;      
        }
        private void AddGridRow()
        {
            _selectedIndex = -1;

            CampaignTarget campaignTarget = new CampaignTarget { PartnerPremiumTarget = "0", PartnerTarget = "0", ChildTarget = "0", SalesPerHourTarget = "0.0", BasePremiumTarget = "0", ChildPremiumTarget = "0", DateApplicableFrom = DateTime.Now, BaseUnitTarget = "0", };
            if (_selectCampaignID == 5)
            {
                campaignTarget = new CampaignTarget { AccDissTypeSelectedIndex = 0, PartnerPremiumTarget = "0", PartnerTarget = "0", ChildTarget = "0", SalesPerHourTarget = "0.0", BasePremiumTarget = "0", ChildPremiumTarget = "0", DateApplicableFrom = DateTime.Now, BaseUnitTarget = "0", };//select macc by default
            }
            dgCampaignTargets.Items.Add(campaignTarget);
            FormatColumns();
            
        }

        private void dgMenuItemAddNewRow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddGridRow();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmbCampaigns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                RemoveAllRows();
                _selectCampaignID = long.Parse(cmbCampaigns.SelectedValue.ToString());
                //determine if its upgrade campaign
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@CampaignID", _selectCampaignID);
                DataSet dsIsUpgrade = Methods.ExecuteStoredProcedure("spDetermineIfUpgrade", parameters);
                if (dsIsUpgrade.Tables.Count > 0)
                {
                    _isUpgradeCampaign = true;
                }
                else
                {
                    _isUpgradeCampaign = false;
                }
                LoadSavedTargets();
               
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void LoadSavedTargets()
        {
            RemoveAllRows();
            if (_selectCampaignID != -1)
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@CampaignID", _selectCampaignID);
                DataTable dtSavedTargets = Methods.ExecuteStoredProcedure("spGetSavedTRSCampaignDefaultTargets", parameters).Tables[0];
                if (dtSavedTargets.Rows.Count > 0)
                {

                    foreach (DataRow row in dtSavedTargets.Rows)
                    {
                        if (_selectCampaignID != 5)
                        {
                            CampaignTarget campaignTarget = new CampaignTarget();
                            campaignTarget.TargetID = long.Parse(row["ID"].ToString());
                            campaignTarget.SalesPerHourTarget = row["SalesPerHourTarget"].ToString();
                            campaignTarget.BasePremiumTarget = Math.Round(decimal.Parse(row["BasePremiumTarget"].ToString()), 2).ToString();
                            campaignTarget.PartnerPremiumTarget = Math.Round(Decimal.Parse(row["PartnerPremiumTarget"].ToString()), 2).ToString();
                            campaignTarget.ChildPremiumTarget = Math.Round(Decimal.Parse(row["ChildPremiumTarget"].ToString()), 2).ToString();
                            campaignTarget.PartnerTarget = row["PartnerTarget"].ToString();
                            campaignTarget.ChildTarget = row["ChildTarget"].ToString();
                            campaignTarget.DateApplicableFrom = DateTime.Parse(row["DateApplicableFrom"].ToString());
                            campaignTarget.BaseUnitTarget = row["BaseUnitTarget"].ToString();
                            
                            
                            dgCampaignTargets.Items.Add(campaignTarget);
                        }
                        else
                        {
                            int accDisSelectedIndex = -1;
                            string accDisSelectedItem = row["AccDisSelectedItem"].ToString();
                            if (accDisSelectedItem.ToLower() == "macc")
                            {
                                accDisSelectedIndex = 0;
                            }
                            if (accDisSelectedItem.ToLower() == "macc million")
                            {
                                accDisSelectedIndex = 1;
                            }
                            CampaignTarget campaignTarget = new CampaignTarget();
                            campaignTarget.TargetID = long.Parse(row["ID"].ToString());
                            campaignTarget.SalesPerHourTarget = row["SalesPerHourTarget"].ToString();
                            campaignTarget.BasePremiumTarget = Math.Round(decimal.Parse(row["BasePremiumTarget"].ToString()), 2).ToString();
                            campaignTarget.PartnerPremiumTarget = Math.Round(Decimal.Parse(row["PartnerPremiumTarget"].ToString()), 2).ToString();
                            campaignTarget.ChildPremiumTarget = Math.Round(Decimal.Parse(row["ChildPremiumTarget"].ToString()), 2).ToString();
                            campaignTarget.PartnerTarget = row["PartnerTarget"].ToString();
                            campaignTarget.ChildTarget = row["ChildTarget"].ToString();
                            campaignTarget.DateApplicableFrom = DateTime.Parse(row["DateApplicableFrom"].ToString());
                            campaignTarget.BaseUnitTarget = row["BaseUnitTarget"].ToString();
                            campaignTarget.AccDissTypeSelectedItem = row["AccDisSelectedItem"].ToString();
                            campaignTarget.AccDissTypeSelectedIndex = accDisSelectedIndex;
                           
                                                   
                            dgCampaignTargets.Items.Add(campaignTarget);
                        }
                        FormatColumns();
                    }
                }
                else
                {
                    AddGridRow();
                }

            }
            else
            {
                AddGridRow();
            }
           
        }
        private void RemoveAllRows()
        {
            if (dgCampaignTargets.Items.Count > 0)
            {
                for (int i = 0; i < dgCampaignTargets.Items.Count; i++)
                {
                    dgCampaignTargets.Items.RemoveAt(i);
                }
            }            
        }

        private void txtSalesPerHourTarget_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_selectedIndex != -1)
            {
                CampaignTarget campaignTarget = (CampaignTarget)dgCampaignTargets.Items[_selectedIndex]; 
                TextBox txtSalesPerHourTarget = sender as TextBox;
                string contentString = txtSalesPerHourTarget.Text;
                foreach (char character in contentString)
                {
                    if (char.IsLetter(character) == true)
                    {
                        txtSalesPerHourTarget.Text = "0.0";
                        break;
                    }
                }
                if (txtSalesPerHourTarget.Text == string.Empty)
                {
                    txtSalesPerHourTarget.Text = "0.0";
                }
                campaignTarget.SalesPerHourTarget = txtSalesPerHourTarget.Text;
            }
        }

        private void txtBasePremiumTarget_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_selectedIndex != -1)
            {
                CampaignTarget campaignTarget = (CampaignTarget)dgCampaignTargets.Items[_selectedIndex];
                TextBox txtBasePremiumTarget = sender as TextBox;
                string contentString = txtBasePremiumTarget.Text;
                foreach (char character in contentString)
                {
                    if (char.IsLetter(character) == true)
                    {
                            txtBasePremiumTarget.Text = "0";
                            break;
                    }
                }
                if (txtBasePremiumTarget.Text == string.Empty)
                {
                    txtBasePremiumTarget.Text = "0";
                }            
               
                campaignTarget.BasePremiumTarget = txtBasePremiumTarget.Text;
            }
        }

        private void txtPartnerPremiumTarget_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_selectedIndex != -1)
            {
                CampaignTarget campaignTarget = (CampaignTarget)dgCampaignTargets.Items[_selectedIndex];
                TextBox txtPartnerPremiumTarget = sender as TextBox;
                string contentString = txtPartnerPremiumTarget.Text;
                foreach (char character in contentString)
                {
                    if (char.IsLetter(character) == true)
                    {
                        txtPartnerPremiumTarget.Text = "0";
                        break;
                    }
                }
                if (txtPartnerPremiumTarget.Text == string.Empty)
                {
                    txtPartnerPremiumTarget.Text = "0";
                }

                campaignTarget.PartnerPremiumTarget = txtPartnerPremiumTarget.Text;
            }
        }

        private void txtChildPremiumTarget_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_selectedIndex != -1)
            {
                CampaignTarget campaignTarget = (CampaignTarget)dgCampaignTargets.Items[_selectedIndex];
                TextBox txtChildPremiumTarget = sender as TextBox;
                string contentString = txtChildPremiumTarget.Text;
                foreach (char character in contentString)
                {
                    if (char.IsLetter(character) == true)
                    {
                        txtChildPremiumTarget.Text = "0";
                        break;
                    }
                }
                if (txtChildPremiumTarget.Text == string.Empty)
                {
                    txtChildPremiumTarget.Text = "0";
                }

                campaignTarget.ChildPremiumTarget = txtChildPremiumTarget.Text;
            }
        }

        private void txtPartnerTarget_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_selectedIndex != -1)
            {
                CampaignTarget campaignTarget = (CampaignTarget)dgCampaignTargets.Items[_selectedIndex];
                TextBox txtPartnerTarget = sender as TextBox;
                string contentString = txtPartnerTarget.Text;
                foreach (char character in contentString)
                {
                    if (char.IsLetter(character) == true)
                    {
                        txtPartnerTarget.Text = "0";
                        break;
                    }
                }
                if (txtPartnerTarget.Text == string.Empty)
                {
                    txtPartnerTarget.Text = "0";
                }

                campaignTarget.PartnerTarget = txtPartnerTarget.Text;
            }
        }

        private void txtChildTarget_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_selectedIndex != -1)
            {
                CampaignTarget campaignTarget = (CampaignTarget)dgCampaignTargets.Items[_selectedIndex];
                TextBox txtChildTarget = sender as TextBox;
                string contentString = txtChildTarget.Text;
                foreach (char character in contentString)
                {
                    if (char.IsLetter(character) == true)
                    {
                        txtChildTarget.Text = "0";
                        break;
                    }
                }
                if (txtChildTarget.Text == string.Empty)
                {
                    txtChildTarget.Text = "0";
                }

                campaignTarget.ChildTarget = txtChildTarget.Text;
            }
        }

        private void dteDateApplicableFrom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (_selectedIndex != -1)
            {
                CampaignTarget campaignTarget = (CampaignTarget)dgCampaignTargets.Items[_selectedIndex];
                XamDateTimeEditor dteDateApplicableFrom = sender as XamDateTimeEditor;
                campaignTarget.DateApplicableFrom = DateTime.Parse(dteDateApplicableFrom.Value.ToString());
            }
        }

        private void dgMenuItemDeleteRow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedIndex > -1)
                {
                    CampaignTarget campaigntarget = (CampaignTarget)dgCampaignTargets.Items[_selectedIndex];
                    if (campaigntarget.TargetID != null)
                    {
                        bool? result = ShowMessageBox(new INMessageBoxWindow2(), "This is a Saved Target, Would you like to remove?", "Remove Target ?", ShowMessageType.Information);

                        if (result == true)
                        {
                            SqlParameter[] parameters = new SqlParameter[1];
                            parameters[0] = new SqlParameter("@TargetID", campaigntarget.TargetID);
                            Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spRemoveTSRCampaignDefaultTarget", parameters, 600);
                           
                            dgCampaignTargets.Items.RemoveAt(_selectedIndex);
                        }
                    }
                    else
                    {
                        dgCampaignTargets.Items.RemoveAt(_selectedIndex);
                    }

                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void FormatColumns()
        {
            
            dgCampaignTargets.Columns[0].Width = 160;
            dgCampaignTargets.Columns[1].Width = 160;
            dgCampaignTargets.Columns[2].Width = 160;
            dgCampaignTargets.Columns[3].Width = 160;
            dgCampaignTargets.Columns[4].Width = 160;
            dgCampaignTargets.Columns[5].Width = 160;
            dgCampaignTargets.Columns[6].Width = 150;
            dgCampaignTargets.Columns[7].Width = 150;
            

            if (_isUpgradeCampaign == true)
            {
                dgCampaignTargets.Columns[0].Visibility = Visibility.Visible;
                dgCampaignTargets.Columns[1].Visibility = Visibility.Collapsed;
                dgCampaignTargets.Columns[2].Visibility = Visibility.Collapsed;
                dgCampaignTargets.Columns[3].Visibility = Visibility.Collapsed;
                dgCampaignTargets.Columns[4].Visibility = Visibility.Collapsed;
                dgCampaignTargets.Columns[5].Visibility = Visibility.Collapsed;
                dgCampaignTargets.Columns[6].Visibility = Visibility.Visible;
                dgCampaignTargets.Columns[7].Visibility = Visibility.Visible;
                dgCampaignTargets.Columns[8].Visibility = Visibility.Collapsed;

                dgCampaignTargets.Columns[0].Width = 369;
                dgCampaignTargets.Columns[1].Width = 160;
                dgCampaignTargets.Columns[2].Width = 160;
                dgCampaignTargets.Columns[3].Width = 160;
                dgCampaignTargets.Columns[4].Width = 160;
                dgCampaignTargets.Columns[5].Width = 160;
                dgCampaignTargets.Columns[6].Width = 369;
                dgCampaignTargets.Columns[7].Width = 369;
                
            }
            else
            {
                if (_selectCampaignID != 5)
                {
                    dgCampaignTargets.Columns[0].Visibility = Visibility.Visible;
                    dgCampaignTargets.Columns[1].Visibility = Visibility.Visible;
                    dgCampaignTargets.Columns[2].Visibility = Visibility.Visible;
                    dgCampaignTargets.Columns[3].Visibility = Visibility.Visible;
                    dgCampaignTargets.Columns[4].Visibility = Visibility.Visible;
                    dgCampaignTargets.Columns[5].Visibility = Visibility.Visible;
                    dgCampaignTargets.Columns[6].Visibility = Visibility.Collapsed;
                    dgCampaignTargets.Columns[7].Visibility = Visibility.Visible;
                    dgCampaignTargets.Columns[8].Visibility = Visibility.Collapsed;

                    dgCampaignTargets.Columns[0].Width = 160;
                    dgCampaignTargets.Columns[1].Width = 160;
                    dgCampaignTargets.Columns[2].Width = 160;
                    dgCampaignTargets.Columns[3].Width = 160;
                    dgCampaignTargets.Columns[4].Width = 160;
                    dgCampaignTargets.Columns[5].Width = 160;
                    dgCampaignTargets.Columns[6].Width = 150;
                    dgCampaignTargets.Columns[7].Width = 150;
                    
                }
                else
                {
                    dgCampaignTargets.Columns[0].Visibility = Visibility.Visible;
                    dgCampaignTargets.Columns[1].Visibility = Visibility.Visible;
                    dgCampaignTargets.Columns[2].Visibility = Visibility.Visible;
                    dgCampaignTargets.Columns[3].Visibility = Visibility.Visible;
                    dgCampaignTargets.Columns[4].Visibility = Visibility.Visible;
                    dgCampaignTargets.Columns[5].Visibility = Visibility.Visible;
                    dgCampaignTargets.Columns[6].Visibility = Visibility.Collapsed;
                    dgCampaignTargets.Columns[7].Visibility = Visibility.Visible;
                    dgCampaignTargets.Columns[8].Visibility = Visibility.Visible;

                    dgCampaignTargets.Columns[0].Width = 140;
                    dgCampaignTargets.Columns[1].Width = 140;
                    dgCampaignTargets.Columns[2].Width = 140;
                    dgCampaignTargets.Columns[3].Width = 140;
                    dgCampaignTargets.Columns[4].Width = 140;
                    dgCampaignTargets.Columns[5].Width = 140;
                    dgCampaignTargets.Columns[6].Width = 130;
                    dgCampaignTargets.Columns[7].Width = 130;
                    dgCampaignTargets.Columns[8].Width = 130;
                    
                    
                }
             
            }
        }

        private void txtBaseUnitTarget_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_selectedIndex != -1)
            {
                CampaignTarget campaignTarget = (CampaignTarget)dgCampaignTargets.Items[_selectedIndex];
                TextBox txtBaseUnitTarget = sender as TextBox;
                string contentString = txtBaseUnitTarget.Text;
                foreach (char character in contentString)
                {
                    if (char.IsLetter(character) == true)
                    {
                        txtBaseUnitTarget.Text = "0";
                        break;
                    }
                }
                if (txtBaseUnitTarget.Text == string.Empty)
                {
                    txtBaseUnitTarget.Text = "0";
                }

                campaignTarget.BaseUnitTarget = txtBaseUnitTarget.Text;
            }
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selectedIndex != -1)
            {
                CampaignTarget campaignTarget = (CampaignTarget)dgCampaignTargets.Items[_selectedIndex];
                EmbriantComboBox cmbType = sender as EmbriantComboBox;
                campaignTarget.AccDissTypeSelectedIndex = cmbType.SelectedIndex;
                ComboBoxItem selectedItem = (ComboBoxItem)cmbType.SelectedItem;
                campaignTarget.AccDissTypeSelectedItem = selectedItem.Content.ToString();
            }
        }

  

    }
}
