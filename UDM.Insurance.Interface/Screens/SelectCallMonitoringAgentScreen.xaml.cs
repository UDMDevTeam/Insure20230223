using Embriant.Framework.Configuration;
using System;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using UDM.Insurance.Business;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{

    public partial class SelectCallMonitoringAgentScreen
    {

        public long? SelectedDeclineReasonID { get; set; }
        private readonly LeadApplicationScreen _LeadApplicationScreen;
        public SelectCallMonitoringAgentScreen(LeadApplicationScreen leadApplicationScreen)
        {
            InitializeComponent();
            _LeadApplicationScreen = leadApplicationScreen;
            LoadLookupData();

            try 
            {
                if ((lkpUserType?)((User)GlobalSettings.ApplicationUser).FKUserType == lkpUserType.SalesAgent)
                {
                    if (_LeadApplicationScreen.LaData.AppData.LeadStatus == 1)
                    {
                        DateTime? loadedDateOfSale = _LeadApplicationScreen.LaData.AppData.LoadedDateOfSale;
                        if (loadedDateOfSale < DateTime.Now.AddDays(-1))
                        {
                            btnSelect.IsEnabled = false;
                        }
                        else
                        {
                            btnSelect.IsEnabled = true;
                        }
                    }
                    else
                    {
                        btnSelect.IsEnabled = true;
                    }

                }
                else
                {
                    btnSelect.IsEnabled = true;
                }
            }
            catch
            {
                btnSelect.IsEnabled = true;
            }

        }

        private void LoadLookupData()
        {
            //This is for the rule where Upgrade leads choose a selected amount of DebiCheck Agents
            DataTable dtAgentsAvailable;
            if (_LeadApplicationScreen.LaData.AppData.IsLeadUpgrade)
            {
                if (DateTime.Now.DayOfWeek.ToString() == "Saturday")
                {
                    DataTable dtStatus = Methods.GetTableData("SELECT CASE WHEN [INCMAgentsOnline].[Online] = '1' THEN  [lkpINCMAgentForwardedSale].[Description] + ' - Available'ELSE [lkpINCMAgentForwardedSale].[Description] + ' - Unavailable' END AS [Description], [lkpINCMAgentForwardedSale].[FKUserID]  FROM [lkpINCMAgentForwardedSale] LEFT JOIN [INCMAgentsOnline] ON [lkpINCMAgentForwardedSale].[FKUserID] = [INCMAgentsOnline].[FKUserID] ORDER BY [INCMAgentsOnline].[Online] DESC");
                    cmbDeclineReason.Populate(dtStatus, "Description", "FKUserID");
                }
                else
                {

                    if (DateTime.Now.Hour <= 15)
                    {
                        dtAgentsAvailable = Business.Insure.GetAvailableDCAgents();
                        cmbDeclineReason.Populate(dtAgentsAvailable, "Description", "FKUserID");
                    }
                    else
                    {
                        DataTable dtStatus = Methods.GetTableData("SELECT CASE WHEN [INCMAgentsOnline].[Online] = '1' THEN  [lkpINCMAgentForwardedSale].[Description] + ' - Available'ELSE [lkpINCMAgentForwardedSale].[Description] + ' - Unavailable' END AS [Description], [lkpINCMAgentForwardedSale].[FKUserID]  FROM [lkpINCMAgentForwardedSale] LEFT JOIN [INCMAgentsOnline] ON [lkpINCMAgentForwardedSale].[FKUserID] = [INCMAgentsOnline].[FKUserID] ORDER BY [INCMAgentsOnline].[Online] DESC");
                        cmbDeclineReason.Populate(dtStatus, "Description", "FKUserID");
                    }
                }
            }
            else
            {
                DataTable dtStatus = Methods.GetTableData("SELECT CASE WHEN [INCMAgentsOnline].[Online] = '1' THEN  [lkpINCMAgentForwardedSale].[Description] + ' - Available'ELSE [lkpINCMAgentForwardedSale].[Description] + ' - Unavailable' END AS [Description], [lkpINCMAgentForwardedSale].[FKUserID]  FROM [lkpINCMAgentForwardedSale] LEFT JOIN [INCMAgentsOnline] ON [lkpINCMAgentForwardedSale].[FKUserID] = [INCMAgentsOnline].[FKUserID] ORDER BY [INCMAgentsOnline].[Online] DESC");
                cmbDeclineReason.Populate(dtStatus, "Description", "FKUserID");
            }




        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectedDeclineReasonID = Convert.ToInt32(cmbDeclineReason.SelectedValue);

                StringBuilder strQueryAgentOnline = new StringBuilder();
                strQueryAgentOnline.Append("SELECT TOP 1 Online [Response] ");
                strQueryAgentOnline.Append("FROM INCMAgentsOnline ");
                strQueryAgentOnline.Append("WHERE FKUserID = " + SelectedDeclineReasonID.ToString());
                DataTable dtOnline = Methods.GetTableData(strQueryAgentOnline.ToString());

                string CampaignName = dtOnline.Rows[0]["Response"].ToString();

                if (CampaignName == "1         " || CampaignName == "1")
                {
                    string ID;
                    try
                    {
                        StringBuilder strSaletoCMID = new StringBuilder();
                        strSaletoCMID.Append("SELECT TOP 1 ID [Response] ");
                        strSaletoCMID.Append("FROM INSalesToCallMonitoring ");
                        strSaletoCMID.Append("WHERE FKImportID = " + _LeadApplicationScreen.LaData.AppData.ImportID.ToString());
                        DataTable dtSAlestoCMID = Methods.GetTableData(strSaletoCMID.ToString());

                        ID = dtSAlestoCMID.Rows[0]["Response"].ToString();
                    }
                    catch
                    {
                        ID = null;
                    }

                    if (ID == null || ID == "")
                    {
                        SalesToCallMonitoring scm = new SalesToCallMonitoring();
                        scm.FKImportID = _LeadApplicationScreen.LaData.AppData.ImportID;
                        scm.FKUserID = SelectedDeclineReasonID;
                        scm.IsDisplayed = "0";

                        //_LeadApplicationScreen.btnSave.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                        _LeadApplicationScreen.ForwardToDCSave();

                        scm.Save(_validationResult);
                    }
                    else
                    {
                        SalesToCallMonitoring scm = new SalesToCallMonitoring(long.Parse(ID));
                        scm.FKUserID = SelectedDeclineReasonID;
                        scm.IsDisplayed = "0";

                        //_LeadApplicationScreen.btnSave.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                        _LeadApplicationScreen.ForwardToDCSave();

                        scm.Save(_validationResult);
                    }



                    OnDialogClose(_dialogResult);
                }
                else
                {
                    cmbDeclineReason.SelectedIndex = -1;
                    Reload();
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

        private void cmbDeclineReason_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(cmbDeclineReason);
        }

        private void cmbDeclineReason_DropDownClosed(object sender, EventArgs e)
        {
            //try
            //{
            //    btnSelect.IsEnabled = false;
            //    if (cmbDeclineReason.SelectedValue != null && _LeadApplicationScreen.cmbAgent.SelectedValue != null)
            //    {
            //        btnSelect.IsEnabled = true;
            //    }
            //    else
            //    {
            //        btnSelect.ToolTip = _LeadApplicationScreen.btnSave.ToolTip;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbDeclineReason.ItemsSource = null;
            }
            catch { }


            //This is for the rule where Upgrade leads choose a selected amount of DebiCheck Agents
            DataTable dtAgentsAvailable;
            if (_LeadApplicationScreen.LaData.AppData.IsLeadUpgrade)
            {
                if (DateTime.Now.DayOfWeek.ToString() == "Saturday")
                {
                    DataTable dtStatus = Methods.GetTableData("SELECT CASE WHEN [INCMAgentsOnline].[Online] = '1' THEN  [lkpINCMAgentForwardedSale].[Description] + ' - Available'ELSE [lkpINCMAgentForwardedSale].[Description] + ' - Unavailable' END AS [Description], [lkpINCMAgentForwardedSale].[FKUserID]  FROM [lkpINCMAgentForwardedSale] LEFT JOIN [INCMAgentsOnline] ON [lkpINCMAgentForwardedSale].[FKUserID] = [INCMAgentsOnline].[FKUserID] ORDER BY [INCMAgentsOnline].[Online] DESC");
                    cmbDeclineReason.Populate(dtStatus, "Description", "FKUserID");
                }
                else
                {

                    if (DateTime.Now.Hour <= 15)
                    {
                        dtAgentsAvailable = Business.Insure.GetAvailableDCAgents();
                        cmbDeclineReason.Populate(dtAgentsAvailable, "Description", "FKUserID");
                    }
                    else
                    {
                        DataTable dtStatus = Methods.GetTableData("SELECT CASE WHEN [INCMAgentsOnline].[Online] = '1' THEN  [lkpINCMAgentForwardedSale].[Description] + ' - Available'ELSE [lkpINCMAgentForwardedSale].[Description] + ' - Unavailable' END AS [Description], [lkpINCMAgentForwardedSale].[FKUserID]  FROM [lkpINCMAgentForwardedSale] LEFT JOIN [INCMAgentsOnline] ON [lkpINCMAgentForwardedSale].[FKUserID] = [INCMAgentsOnline].[FKUserID] ORDER BY [INCMAgentsOnline].[Online] DESC");
                        cmbDeclineReason.Populate(dtStatus, "Description", "FKUserID");
                    }
                }
            }
            else
            {
                DataTable dtStatus = Methods.GetTableData("SELECT CASE WHEN [INCMAgentsOnline].[Online] = '1' THEN  [lkpINCMAgentForwardedSale].[Description] + ' - Available'ELSE [lkpINCMAgentForwardedSale].[Description] + ' - Unavailable' END AS [Description], [lkpINCMAgentForwardedSale].[FKUserID]  FROM [lkpINCMAgentForwardedSale] LEFT JOIN [INCMAgentsOnline] ON [lkpINCMAgentForwardedSale].[FKUserID] = [INCMAgentsOnline].[FKUserID] ORDER BY [INCMAgentsOnline].[Online] DESC");
                cmbDeclineReason.Populate(dtStatus, "Description", "FKUserID");
            }
        }

        private void Reload()
        {
            try
            {
                cmbDeclineReason.ItemsSource = null;
            }
            catch { }


            //This is for the rule where Upgrade leads choose a selected amount of DebiCheck Agents
            DataTable dtAgentsAvailable;
            if (_LeadApplicationScreen.LaData.AppData.IsLeadUpgrade)
            {
                if (DateTime.Now.DayOfWeek.ToString() == "Saturday")
                {
                    DataTable dtStatus = Methods.GetTableData("SELECT CASE WHEN [INCMAgentsOnline].[Online] = '1' THEN  [lkpINCMAgentForwardedSale].[Description] + ' - Available'ELSE [lkpINCMAgentForwardedSale].[Description] + ' - Unavailable' END AS [Description], [lkpINCMAgentForwardedSale].[FKUserID]  FROM [lkpINCMAgentForwardedSale] LEFT JOIN [INCMAgentsOnline] ON [lkpINCMAgentForwardedSale].[FKUserID] = [INCMAgentsOnline].[FKUserID] ORDER BY [INCMAgentsOnline].[Online] DESC");
                    cmbDeclineReason.Populate(dtStatus, "Description", "FKUserID");
                }
                else
                {

                    if (DateTime.Now.Hour <= 15)
                    {
                        dtAgentsAvailable = Business.Insure.GetAvailableDCAgents();
                        cmbDeclineReason.Populate(dtAgentsAvailable, "Description", "FKUserID");
                    }
                    else
                    {
                        DataTable dtStatus = Methods.GetTableData("SELECT CASE WHEN [INCMAgentsOnline].[Online] = '1' THEN  [lkpINCMAgentForwardedSale].[Description] + ' - Available'ELSE [lkpINCMAgentForwardedSale].[Description] + ' - Unavailable' END AS [Description], [lkpINCMAgentForwardedSale].[FKUserID]  FROM [lkpINCMAgentForwardedSale] LEFT JOIN [INCMAgentsOnline] ON [lkpINCMAgentForwardedSale].[FKUserID] = [INCMAgentsOnline].[FKUserID] ORDER BY [INCMAgentsOnline].[Online] DESC");
                        cmbDeclineReason.Populate(dtStatus, "Description", "FKUserID");
                    }
                }
            }
            else
            {
                DataTable dtStatus = Methods.GetTableData("SELECT CASE WHEN [INCMAgentsOnline].[Online] = '1' THEN  [lkpINCMAgentForwardedSale].[Description] + ' - Available'ELSE [lkpINCMAgentForwardedSale].[Description] + ' - Unavailable' END AS [Description], [lkpINCMAgentForwardedSale].[FKUserID]  FROM [lkpINCMAgentForwardedSale] LEFT JOIN [INCMAgentsOnline] ON [lkpINCMAgentForwardedSale].[FKUserID] = [INCMAgentsOnline].[FKUserID] ORDER BY [INCMAgentsOnline].[Online] DESC");
                cmbDeclineReason.Populate(dtStatus, "Description", "FKUserID");
            }
        }

    }
}
