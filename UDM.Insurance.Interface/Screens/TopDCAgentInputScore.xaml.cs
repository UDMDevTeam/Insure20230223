using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static UDM.Insurance.Interface.Data.LeadApplicationData;
using UDM.WPF.Library;
using Embriant.Framework.Configuration;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Business.Objects;
using UDM.Insurance.Business;

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
                bool saveResult = INTopDCAgentsMapper.Save(agent);
                if (!saveResult)
                {
                    Console.WriteLine($"Failed to save Base Agent Score.");
                }
            }

            // Save Upgrade Agents
            foreach (var agent in TopUpgradeAgents)
            {
                bool saveResult = INTopDCAgentsMapper.Save(agent);
                if (!saveResult)
                {
                    Console.WriteLine($"Failed to save Upgrade Agent Score.");
                }
            }
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
                SaveMethod();
            }
            catch { 
            
            }
        }
    }
}
