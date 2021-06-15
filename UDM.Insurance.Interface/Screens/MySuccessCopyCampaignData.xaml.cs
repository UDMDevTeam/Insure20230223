using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.Events;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for MySuccessCopyCampaignData.xaml
    /// </summary>
    public partial class MySuccessCopyCampaignData
    {

        private List<Record> _lstSelectedCampaigns;

        DataTable dtDocumentData;
        DataTable dtDestinationData;
        private string _fkCampaignIDs = "";


        public MySuccessCopyCampaignData()
        {
            InitializeComponent();

            LoadDocumentType();

        }

        private void LoadDocumentType()
        {
            try
            {

                CommonControlData.PopulateDocumentTypeComboBox(cmbDocumentType);
                CommonControlData.PopulateDocumentTypeComboBox(cmbDestinationDocumentType);

            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void LoadLookups()
        {

            bool onlyUpgradeCampaigns; 


            if (chkCampaignBase.IsChecked == true)
            {
                onlyUpgradeCampaigns = false;

                object param1 = Database.GetParameter("@OnlyUpgradeCampaigns", onlyUpgradeCampaigns);
                object[] paramArray = new[] { param1 };
                DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetCampaigns", paramArray).Tables[0];

                //DataColumn column = new DataColumn("Select", typeof(bool));
                //column.DefaultValue = false;
                //dt.Columns.Add(column);

                xdgSourceCampaigns.DataSource = dt.DefaultView;

            }

            if (chkCampaignUpgrade.IsChecked == true)
            {
                onlyUpgradeCampaigns = true;
                object param1 = Database.GetParameter("@OnlyUpgradeCampaigns", onlyUpgradeCampaigns);
                object[] paramArray = new[] { param1 };
                DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetCampaigns", paramArray).Tables[0];

                //DataColumn column = new DataColumn("Select", typeof(bool));
                //column.DefaultValue = false;
                //dt.Columns.Add(column);

                xdgSourceCampaigns.DataSource = dt.DefaultView;
            }

            if (chkCampaignResales.IsChecked == true)
            {
                onlyUpgradeCampaigns = false;
                object param1 = Database.GetParameter("@OnlyUpgradeCampaigns", onlyUpgradeCampaigns);
                object[] paramArray = new[] { param1 };
                DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetResalesCampaigns", paramArray).Tables[0];

                //DataColumn column = new DataColumn("Select", typeof(bool));
                //column.DefaultValue = false;
                //dt.Columns.Add(column);
                //dt.DefaultView.Sort = "UDMBatchCode ASC";

                xdgSourceCampaigns.DataSource = dt.DefaultView;
            }

        }

        private void LoadDestinationLookups()
        {

            bool onlyUpgradeCampaigns;


            if (chkDestinationCampaignBase.IsChecked == true)
            {
                onlyUpgradeCampaigns = false;

                object param1 = Database.GetParameter("@OnlyUpgradeCampaigns", onlyUpgradeCampaigns);
                object[] paramArray = new[] { param1 };
                DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetCampaigns", paramArray).Tables[0];

                //DataColumn column = new DataColumn("Select", typeof(bool));
                //column.DefaultValue = false;
                //dt.Columns.Add(column);

                xdgDestinationCampaigns.DataSource = dt.DefaultView;

            }

            if (chkDestinationCampaignUpgrade.IsChecked == true)
            {
                onlyUpgradeCampaigns = true;

                object param1 = Database.GetParameter("@OnlyUpgradeCampaigns", onlyUpgradeCampaigns);
                object[] paramArray = new[] { param1 };
                DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetCampaigns", paramArray).Tables[0];

                //DataColumn column = new DataColumn("Select", typeof(bool));
                //column.DefaultValue = false;
                //dt.Columns.Add(column);

                xdgDestinationCampaigns.DataSource = dt.DefaultView;
            }

            if (chkDestinationCampaignResales.IsChecked == true)
            {
                onlyUpgradeCampaigns = false;
                object param1 = Database.GetParameter("@OnlyUpgradeCampaigns", onlyUpgradeCampaigns);
                object[] paramArray = new[] { param1 };
                DataTable dt = Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spINGetResalesCampaigns", paramArray).Tables[0];

                //DataColumn column = new DataColumn("Select", typeof(bool));
                //column.DefaultValue = false;
                //dt.Columns.Add(column);
                //dt.DefaultView.Sort = "UDMBatchCode ASC";

                xdgDestinationCampaigns.DataSource = dt.DefaultView;
            }

        }

        private void LoadSourceDocumentData() 
        {
            try
            {

                try { dtDocumentData.Clear(); } catch { }

                var lstTemp = (from r in xdgSourceCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();


                _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

                //if (_lstSelectedCampaigns.Count == 0)
                //{
                //    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No Campaigns Selected", ShowMessageType.Error);
                //    return;
                //}
                //else
                //{

                _fkCampaignIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                _fkCampaignIDs = _fkCampaignIDs.Substring(0, _fkCampaignIDs.Length - 1);

                //GlobalSettings.MySuccessCampaignID = _fkCampaignIDs;
                //}
                if (cmbDocumentType.SelectedValue == null)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select a document type. ", "No Document Type Selected", ShowMessageType.Error);

                }
                else if (cmbDocumentType.SelectedValue.ToString() == "1")
                {
                    dtDocumentData = Methods.GetTableData(

                    "SELECT [INMySuccessCampaignDetails].[FKCampaignID] AS [CampaignID], [INMySuccessCampaignDetails].[ScriptEng] FROM [INMySuccessCampaignDetails]" +
                    "WHERE [INMySuccessCampaignDetails].[FKCampaignID] in (" + _fkCampaignIDs + ")");
                }
                else if (cmbDocumentType.SelectedItem.ToString() == "2")
                {
                    dtDocumentData = Methods.GetTableData(

                    "SELECT [INMySuccessCampaignDetails].[FKCampaignID] AS [CampaignID], [INMySuccessCampaignDetails].[ClosureEng] FROM [INMySuccessCampaignDetails]" +
                    "WHERE [INMySuccessCampaignDetails].[FKCampaignID] in (" + _fkCampaignIDs + ")");
                }
                else if (cmbDocumentType.SelectedItem.ToString() == "3")
                {
                    dtDocumentData = Methods.GetTableData(

                    "SELECT [INMySuccessCampaignDetails].[FKCampaignID] AS [CampaignID], [INMySuccessCampaignDetails].[Options] FROM [INMySuccessCampaignDetails]" +
                    "WHERE [INMySuccessCampaignDetails].[FKCampaignID] in (" + _fkCampaignIDs + ")");
                }
                else if (cmbDocumentType.SelectedItem.ToString() == "4")
                {
                    dtDocumentData = Methods.GetTableData(

                    "SELECT [INMySuccessCampaignDetails].[FKCampaignID] AS [CampaignID], [INMySuccessCampaignDetails].[IncentiveStructure] FROM [INMySuccessCampaignDetails]" +
                    "WHERE [INMySuccessCampaignDetails].[FKCampaignID] in (" + _fkCampaignIDs + ")");
                }
                else if (cmbDocumentType.SelectedItem.ToString() == "5")
                {
                    dtDocumentData = Methods.GetTableData(

                    "SELECT [INMySuccessCampaignDetails].[FKCampaignID] AS [CampaignID], [INMySuccessCampaignDetails].[Objectionhandling] FROM [INMySuccessCampaignDetails]" +
                    "WHERE [INMySuccessCampaignDetails].[FKCampaignID] in (" + _fkCampaignIDs + ")");
                }
                else if (cmbDocumentType.SelectedItem.ToString() == "6")
                {
                    dtDocumentData = Methods.GetTableData(

                    "SELECT [INMySuccessCampaignDetails].[FKCampaignID] AS [CampaignID], [INMySuccessCampaignDetails].[NeedCreation] FROM [INMySuccessCampaignDetails]" +
                    "WHERE [INMySuccessCampaignDetails].[FKCampaignID] in (" + _fkCampaignIDs + ")");
                }
                



            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void LoadDestinationDocumentData()
        {
            try
            {

                try { dtDocumentData.Clear(); } catch { }

                var lstTemp = (from r in xdgDestinationCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();


                _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

                //if (_lstSelectedCampaigns.Count == 0)
                //{
                //    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No Campaigns Selected", ShowMessageType.Error);
                //    return;
                //}
                //else
                //{

                _fkCampaignIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                _fkCampaignIDs = _fkCampaignIDs.Substring(0, _fkCampaignIDs.Length - 1);

                //GlobalSettings.MySuccessCampaignID = _fkCampaignIDs;

                //}
                if (cmbDestinationDocumentType.SelectedValue == null)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select a document type. ", "No Document Type Selected", ShowMessageType.Error);
                }
                else if (cmbDestinationDocumentType.SelectedValue.ToString() == "1")
                {
                    dtDestinationData = Methods.GetTableData(

                    "SELECT [INMySuccessCampaignDetails].[FKCampaignID] AS [CampaignID], [INMySuccessCampaignDetails].[ScriptEng] FROM [INMySuccessCampaignDetails]" +
                    "WHERE [INMySuccessCampaignDetails].[FKCampaignID] in (" + _fkCampaignIDs + ")");
                }
                else if (cmbDestinationDocumentType.SelectedValue.ToString() == "2")
                {
                    dtDestinationData = Methods.GetTableData(

                    "SELECT [INMySuccessCampaignDetails].[FKCampaignID] AS [CampaignID], [INMySuccessCampaignDetails].[ClosureEng] FROM [INMySuccessCampaignDetails]" +
                    "WHERE [INMySuccessCampaignDetails].[FKCampaignID] in (" + _fkCampaignIDs + ")");
                }
                else if (cmbDestinationDocumentType.SelectedValue.ToString() == "3")
                {
                    dtDestinationData = Methods.GetTableData(

                    "SELECT [INMySuccessCampaignDetails].[FKCampaignID] AS [CampaignID], [INMySuccessCampaignDetails].[Options] FROM [INMySuccessCampaignDetails]" +
                    "WHERE [INMySuccessCampaignDetails].[FKCampaignID] in (" + _fkCampaignIDs + ")");
                }
                else if (cmbDestinationDocumentType.SelectedValue.ToString() == "4")
                {
                    dtDestinationData = Methods.GetTableData(

                    "SELECT [INMySuccessCampaignDetails].[FKCampaignID] AS [CampaignID], [INMySuccessCampaignDetails].[IncentiveStructure] FROM [INMySuccessCampaignDetails]" +
                    "WHERE [INMySuccessCampaignDetails].[FKCampaignID] in (" + _fkCampaignIDs + ")");
                }
                else if (cmbDestinationDocumentType.SelectedValue.ToString() == "5")
                {
                    dtDestinationData = Methods.GetTableData(

                    "SELECT [INMySuccessCampaignDetails].[FKCampaignID] AS [CampaignID], [INMySuccessCampaignDetails].[Objectionhandling] FROM [INMySuccessCampaignDetails]" +
                    "WHERE [INMySuccessCampaignDetails].[FKCampaignID] in (" + _fkCampaignIDs + ")");
                }
                else if (cmbDestinationDocumentType.SelectedValue.ToString() == "6")
                {
                    dtDestinationData = Methods.GetTableData(

                    "SELECT [INMySuccessCampaignDetails].[FKCampaignID] AS [CampaignID], [INMySuccessCampaignDetails].[NeedCreation] FROM [INMySuccessCampaignDetails]" +
                    "WHERE [INMySuccessCampaignDetails].[FKCampaignID] in (" + _fkCampaignIDs + ")");
                }
               

                //DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                //dtDocumentData.Columns.Add(column);
                //dtDocumentData.DefaultView.Sort = "[AgentName] ASC";



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

                //string CampaignID = cmbCampaigns.SelectedValue.ToString();
                //string SelectedCampaignName = Convert.ToString(Methods.GetTableData("SELECT Name FROM INCampaign WHERE ID = " + CampaignID).Rows[0][0]);

                //if (SelectedCampaignName.ToString() != null)
                //{
                //    cmbCampaigns.SelectedValue = SelectedCampaignName;
                //}
                //else
                //{
                //    //cmbDestinationCampaigns.SelectedValue = null;
                //}
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void chkCampaignUpgrade_Checked(object sender, RoutedEventArgs e)
        {
            LoadLookups(); 
        }

        private void chkCampaignUpgrade_Unchecked(object sender, RoutedEventArgs e)
        {
            
        }

        private void chkDestinationCampaignUpgrade_Checked(object sender, RoutedEventArgs e)
        {
            LoadDestinationLookups();
        }

        private void chkDestinationCampaignUpgrade_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chkDestinationCampaignResales_Checked(object sender, RoutedEventArgs e)
        {
            LoadDestinationLookups();
        }

        private void chkDestinationCampaignResales_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chkDestinationCampaignBase_Checked(object sender, RoutedEventArgs e)
        {
            LoadDestinationLookups();
        }

        private void chkDestinationCampaignBase_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chkCampaignBase_Checked(object sender, RoutedEventArgs e)
        {
            LoadLookups();
        }

        private void chkCampaignBase_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chkCampaignResales_Checked(object sender, RoutedEventArgs e)
        {
            LoadLookups();
        }

        private void chkCampaignResales_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void SourceCampaignHeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void DestinationCampaignHeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void SourceCampaignRecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                LoadSourceDocumentData();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        private void DestinationCampaignRecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadDestinationDocumentData();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                foreach (DataRow dr in dtDocumentData.Rows) 
                {

                    foreach (DataRow dataRow in dtDestinationData.Rows)
                    {
                        if (dtDocumentData.Rows.Count > 1)
                        {
                            dtDestinationData.Rows.Add(dr.ItemArray);
                        }
                        else
                        {
                            if (dtDocumentData.Rows.Count == 0)
                            {
                                MessageBox.Show("Please load data for this campaign");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
