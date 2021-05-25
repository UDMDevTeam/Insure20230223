using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UDM.Insurance.Interface.Data
{
    public static class CommonControlData
    {
        #region Populating common lookups

        #region Campaign Type

        public static void PopulateCampaignTypeComboBox(Embriant.WPF.Controls.EmbriantComboBox cmbTargetComboBox)
        {
            System.Data.DataTable dt = UDM.WPF.Library.Methods.GetTableData("SELECT [ID], [Description] FROM [lkpINCampaignType] ORDER BY [Description] ASC");
            cmbTargetComboBox.Populate(dt, "Description", "ID");
        }

        #endregion Campaign Type

        #region Campaigns

        public static void PopulateCampaignDataGrid(Infragistics.Windows.DataPresenter.XamDataGrid xdgTargetDataGrid)
        {

            System.Data.DataTable dt = UDM.WPF.Library.Methods.GetTableData("SELECT [ID] AS [CampaignID], [Name] AS [CampaignName], [Code] AS [CampaignCode] FROM [INCampaign]");
            System.Data.DataColumn column = new System.Data.DataColumn("Select", typeof(bool));
            column.DefaultValue = false;
            dt.Columns.Add(column);
            dt.DefaultView.Sort = "CampaignName ASC";
            xdgTargetDataGrid.DataSource = dt.DefaultView;
            //}

            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}

            //finally
            //{
            //    SetCursor(Cursors.Arrow);
            //}
        }

        public static void PopulateCampaignNotesComboBox(Embriant.WPF.Controls.EmbriantComboBox cmbTargetComboBox)
        {
            System.Data.DataTable dt = UDM.WPF.Library.Methods.GetTableData("SELECT ID [ID], Description [Description] FROM lkpCampaignNotes ORDER BY ID ASC");
            cmbTargetComboBox.Populate(dt, "Description", "ID");
        }
        public static void PopulateAgentNotesComboBox(Embriant.WPF.Controls.EmbriantComboBox cmbTargetComboBox)
        {
            System.Data.DataTable dt = UDM.WPF.Library.Methods.GetTableData("SELECT ID [ID], Description [Description] FROM lkpAgentNotesMessages ORDER BY ID ASC");
            cmbTargetComboBox.Populate(dt, "Description", "ID");
        }
        


        public static void PopulateCampaignDataGrid(Infragistics.Windows.DataPresenter.XamDataGrid xdgTargetDataGrid, System.Data.DataTable dataTable)
        {
            //try
            //{

            System.Data.DataColumn column = new System.Data.DataColumn("Select", typeof(bool));
            column.DefaultValue = false;
            dataTable.Columns.Add(column);

            xdgTargetDataGrid.DataSource = dataTable.DefaultView;

            //}

            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
        }

        public static void PopulateCampaignComboBox(Embriant.WPF.Controls.EmbriantComboBox cmbTargetComboBox)
        {
            System.Data.DataTable dt = UDM.WPF.Library.Methods.GetTableData("SELECT [ID] AS [CampaignID], [Name] AS [CampaignName], [Code] AS [CampaignCode] FROM [INCampaign] WHERE INCampaign.IsActive = '1' ORDER BY [Code] ASC");
            cmbTargetComboBox.Populate(dt, "CampaignName", "CampaignID");
        }

        public static void PopulateCampaignComboBox(Embriant.WPF.Controls.EmbriantComboBox cmbTargetComboBox, bool onlyUpgradeCampaigns)
        {
            System.Data.DataTable dt = UDM.Insurance.Business.Insure.INGetCampaigns(onlyUpgradeCampaigns);
            cmbTargetComboBox.Populate(dt, "CampaignName", "CampaignID");
        }

        public static void PopulateCampaignComboBox(Embriant.WPF.Controls.EmbriantComboBox cmbTargetComboBox, System.Data.DataTable dataTable)
        {
            //System.Data.DataTable dt = UDM.Insurance.Business.Insure.INGetCampaigns(onlyUpgradeCampaigns);
            cmbTargetComboBox.Populate(dataTable, "CampaignName", "CampaignID");
        }

        public static void PopulateBaseCampaignComboBox(Embriant.WPF.Controls.EmbriantComboBox cmbTargetComboBox)
        {
            System.Data.DataTable dt = UDM.WPF.Library.Methods.GetTableData("SELECT [ID] AS [CampaignID], [Name] AS [CampaignName], [Code] AS [CampaignCode] FROM [INCampaign] WHERE FKINCampaignGroupID = 1 ORDER BY [Code] ASC ");
            cmbTargetComboBox.Populate(dt, "CampaignName", "CampaignID");
        }

        public static void PopulateExtentionCampaignComboBox(Embriant.WPF.Controls.EmbriantComboBox cmbTargetComboBox)
        {
            System.Data.DataTable dt = UDM.WPF.Library.Methods.GetTableData("SELECT [ID] AS [CampaignID], [Name] AS [CampaignName], [Code] AS [CampaignCode] FROM [INCampaign] WHERE FKINCampaignGroupID = 22 ORDER BY [Code] ASC ");
            cmbTargetComboBox.Populate(dt, "CampaignName", "CampaignID");
        }

        public static void PopulateExtentionAndRenewalCampaignComboBox(Embriant.WPF.Controls.EmbriantComboBox cmbTargetComboBox)
        {
            System.Data.DataTable dt = UDM.WPF.Library.Methods.GetTableData("SELECT [ID] AS [CampaignID], [Name] AS [CampaignName], [Code] AS [CampaignCode] FROM [INCampaign] WHERE FKINCampaignGroupID IN (6, 22) ORDER BY [Code] ASC ");
            cmbTargetComboBox.Populate(dt, "CampaignName", "CampaignID");
        }

        public static void PopulateBatchComboBox(Embriant.WPF.Controls.EmbriantComboBox cmbTargetComboBox,long? campaignID)
        {
            System.Data.DataTable dt = UDM.WPF.Library.Methods.GetTableData("SELECT [ID] AS [BatchID], [Code] AS [BatchCode] FROM [INBatch] WHERE FKINCampaignID = " + campaignID + " ORDER BY [Code] ASC ");
            if (dt.Rows.Count > 0)
            {
                cmbTargetComboBox.Populate(dt, "BatchCode", "BatchID");
            }
            else
            {
                cmbTargetComboBox.Populate(new System.Data.DataTable(),string.Empty,string.Empty);
            }
           
        }
        
        

        #endregion Campaigns

        #endregion Populating common lookups

        #region Common Front-End functionality

        public static List<Infragistics.Windows.DataPresenter.Record> GetSelectedItemsFromDataGrid(Infragistics.Windows.DataPresenter.XamDataGrid xdgTargetDataGrid, string columnName)
        {
            var lstTemp = (from r in xdgTargetDataGrid.Records where (bool)((Infragistics.Windows.DataPresenter.DataRecord)r).Cells["Select"].Value select r).ToList();
            List<Infragistics.Windows.DataPresenter.Record> lstSelectedCampaigns = new List<Infragistics.Windows.DataPresenter.Record>(lstTemp.OrderBy(r => ((Infragistics.Windows.DataPresenter.DataRecord)r).Cells[columnName /*"CampaignName"*/].Value));
            return lstSelectedCampaigns;
        }

        public static System.Data.DataRow GetDataRowFromSelectedComboBoxItem(Embriant.WPF.Controls.EmbriantComboBox cmbSourceComboBox)
        {
            return (cmbSourceComboBox.SelectedItem as System.Data.DataRowView).Row;
        }

        public static long? GetObjectIDFromSelectedComboBoxItem(Embriant.WPF.Controls.EmbriantComboBox cmbSourceComboBox)
        {
            return cmbSourceComboBox.SelectedValue as long?;
        }

        #endregion Common Front-End functionality

        #region Standard Event Handlers

        //TODO: Put all standard event handlers whose implementation are the same throughout most front-ends

        #endregion Standard Event Handlers


    }
}
