using System;
using System.Windows;
using System.Windows.Input;
using Embriant.Framework.Configuration;
using System.Data;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;
using System.Windows.Controls;
using UDM.WPF.Library;
using System.Data.SqlClient;
using Embriant.Framework;
using Embriant.Framework.Data;
using System.Collections.Generic;
using Infragistics.Windows.DataPresenter;
using System.Linq;

namespace UDM.Insurance.Interface.Screens
{

    public partial class MySuccess
    {

        #region Variables
        List<string> UpgradeBaseList = new List<string>();

        DataTable dtCampaigns;
        DataTable dtAgents;
        private DataTable dtAgentCallsDG;


        private List<Record> _lstSelectedCampaigns;
        private string _fkCampaignIDs = "";

        #endregion

        #region Constructor

        public MySuccess()
        {
            InitializeComponent();

            #region Default Page Layouts
            Body.Visibility = Visibility.Visible;
            Body2.Visibility = Visibility.Collapsed;
            #endregion

            UpgradeBaseList.Clear();
            UpgradeBaseList.Add("Upgrade");
            UpgradeBaseList.Add("Base");
            cmbBaseUpgrade.ItemsSource = UpgradeBaseList;
        }

        #endregion Constructor



        #region Private Methods


        #endregion Private Methods

        #region Event Handlers

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void LoadAgentCallsDG()
        {
            try { dtAgentCallsDG.Clear(); } catch { }
            dtAgentCallsDG.Rows.Add("1", "Call 1 - Sale", false);
            dtAgentCallsDG.Rows.Add("2", "Call 2 - Objection", false);
            dtAgentCallsDG.Rows.Add("3", "Call 3 - Intro objection turned into Sale", false);

            xdgAgentCalls.DataSource = dtAgentCallsDG.DefaultView;
        }

        #endregion Event Handlers

        private void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void cmbBaseUpgrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCampaignInfo();
        }

        #region Load Datagrids
        private void LoadCampaignInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);

                try { dtCampaigns.Clear(); } catch { }

                dtCampaigns = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign");
                DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                dtCampaigns.Columns.Add(column);
                dtCampaigns.DefaultView.Sort = "CampaignName ASC";
                xdgCampaigns.DataSource = dtCampaigns.DefaultView;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SetCursor(Cursors.Arrow);
            }
        }

        private void LoadAgentDG()
        {
            try
            {
                SetCursor(Cursors.Wait);

                try { dtAgents.Clear(); } catch { }

                var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();


                _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

                if (_lstSelectedCampaigns.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No campaigns selected", ShowMessageType.Error);
                    return;
                }
                else
                {
                    _fkCampaignIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                    _fkCampaignIDs = _fkCampaignIDs.Substring(0, _fkCampaignIDs.Length - 1);
                }

                dtAgents = Methods.GetTableData(
                    "SELECT [INMySuccessAgents].[UserID] AS [AgentID], (SELECT [User].[FirstName] + ' ' + [User].[LastName] FROM [User] WHERE [User].[ID] = [INMySuccessAgents].[UserID] ) AS [AgentName] " +
                    "FROM [INMySuccessAgents]" +
                    "WHERE [INMySuccessAgents].[FKCampaignID] in (" + _fkCampaignIDs + ")");
                DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                dtAgents.Columns.Add(column);
                dtAgents.DefaultView.Sort = "[AgentName] ASC";
                xdgAgents.DataSource = dtAgents.DefaultView;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SetCursor(Cursors.Arrow);
            }
        }

        #endregion

        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void HeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void HeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            LoadAgentDG();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            LoadAgentCallsDG();

            Body.Visibility = Visibility.Collapsed;
            Body2.Visibility = Visibility.Visible;
        }

        private void HeaderPrefixAreaAgentCallsCheckbox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void HeaderPrefixAreaAgentCallsCheckbox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RecordSelectorAgentCallsCheckbox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HeaderPrefixAreaAgentNotesCheckbox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void HeaderPrefixAreaAgentNotesCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {

        }



        private void HeaderPrefixAreaAgentNotesCheckbox_Checked(object sender, RoutedEventArgs e)
        {

        }




        #region Danes Work
        private void HeaderPrefixAreaCampaignNotesCheckbox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void HeaderPrefixAreaCampaignNotesCheckbox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void HeaderPrefixAreaCampaignNotesCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void RecordSelectorAgentNotesCheckbox_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion


    }
}