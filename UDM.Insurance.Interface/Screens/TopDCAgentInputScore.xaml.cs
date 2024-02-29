using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using UDM.WPF.Library;
using Embriant.Framework.Configuration;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Business.Objects;
using UDM.Insurance.Business;
using Embriant.Framework;
using UDM.Insurance.Interface.Windows;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for TopDCAgentInputScore.xaml
    /// </summary>
    public partial class TopDCAgentInputScore 
    {
        private const string IDField = "ID";
        private const string DescriptionField = "Name";
        public TopDCAgentInputScore()
        {
            InitializeComponent();
            LookUps();
        }

        private void LookUps()
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[0];
                DataSet dsLookups = Methods.ExecuteStoredProcedure("GetActiveDebiCheckAgents", parameters);
                DataTable dtStatus = dsLookups.Tables[0];
                cmbAgent.Populate(dtStatus, DescriptionField, IDField);
                cmbAgent2.Populate(dtStatus, DescriptionField, IDField);
                cmbAgent3.Populate(dtStatus, DescriptionField, IDField);
                cmbAgentUpgrades.Populate(dtStatus, DescriptionField, IDField);
                cmbAgent2Upgrade.Populate(dtStatus, DescriptionField, IDField);
                cmbAgent3Upgrade.Populate(dtStatus, DescriptionField, IDField);
            }catch (Exception ex)
            {

            }
        }
        private void SaveMethod()
        {
           long StampUser =  GlobalSettings.ApplicationUser.ID;
           DateTime stampDate = DateTime.Now.Date.AddDays(-1);
            bool saveResult = false;
            bool saveResultUpgrades = false;
            #region Base
            List<INTopDCAgents> TopBaseAgents = new List<INTopDCAgents>
            {
                new INTopDCAgents { StampUserID = StampUser, CampaignType = 1, StampDate = stampDate, FKUserID = Convert.ToInt32(cmbAgent.SelectedValue.ToString()), AcceptedRates = Convert.ToDecimal(edRate1.Text) },
                new INTopDCAgents{ StampUserID = StampUser, CampaignType=1, StampDate = stampDate, FKUserID = Convert.ToInt32(cmbAgent2.SelectedValue.ToString()), AcceptedRates = Convert.ToDecimal(edRate2.Text) },
                new INTopDCAgents{ StampUserID = StampUser, CampaignType=1, StampDate = stampDate, FKUserID = Convert.ToInt32(cmbAgent3.SelectedValue.ToString()), AcceptedRates = Convert.ToDecimal(edRate3.Text) },
            };

            #endregion
            #region Upgrades
            List<INTopDCAgents> TopUpgradeAgents = new List<INTopDCAgents>
            {
                new INTopDCAgents { StampUserID = StampUser, CampaignType = 2, StampDate = stampDate, FKUserID = Convert.ToInt32(cmbAgentUpgrades.SelectedValue.ToString()), AcceptedRates = Convert.ToDecimal(edRate1Upgrade.Text) },
                new INTopDCAgents{ StampUserID = StampUser, CampaignType=2, StampDate = stampDate, FKUserID = Convert.ToInt32(cmbAgent2Upgrade.SelectedValue.ToString()), AcceptedRates = Convert.ToDecimal(edRate2Upgrade.Text) },
                new INTopDCAgents{ StampUserID = StampUser, CampaignType=2, StampDate = stampDate, FKUserID = Convert.ToInt32(cmbAgent3Upgrade.SelectedValue.ToString()), AcceptedRates = Convert.ToDecimal(edRate3Upgrade.Text) },
            };
            #endregion
            // Save Base Agents
            foreach (var agent in TopBaseAgents)
            {
                 saveResult = INTopDCAgentsMapper.Save(agent);
                if (!saveResult)
                {
                    Console.WriteLine($"Failed to save Base Agent Score.");
                }
            }

            // Save Upgrade Agents
            foreach (var agent in TopUpgradeAgents)
            {
                saveResultUpgrades = INTopDCAgentsMapper.Save(agent);
                if (!saveResult)
                {
                    Console.WriteLine($"Failed to save Upgrade Agent Score.");
                }
            }
           
            ShowMessageBox(new INMessageBoxWindow1(), "Top DC stats, Has been saved.\n", "Success", ShowMessageType.Information);
         
        }
        private class TOPDCAgents
        {
            public int FKUserID { get; set; }
            public long StampUserID { get; set; }
            public int CampaignTypeSet { get; set; }

            public decimal AcceptedRates { get; set; }
            public DateTime StampDate { get; set; }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            var menuManagementScreen = new MenuManagementScreen(ScreenDirection.Reverse);
            OnClose(menuManagementScreen);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbAgent.SelectedIndex != -1 && cmbAgentUpgrades.SelectedIndex != -1)
                {

                    if (edRate1.Text != string.Empty || edRate1.Text != "" && edRate1Upgrade.Text != string.Empty || edRate1Upgrade.Text != "")
                    {
                        SaveMethod();
                    }
                    else
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "Please add Accepted rates for atleast one Agent for Base and Upgrades.\n", "Validation", ShowMessageType.Error);
                    }
                }
                else
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select atleast one Agent for Base and Upgrades.\n", "Validation", ShowMessageType.Error);
                }
            }
            catch { 
            
            }
        }
    }
}
