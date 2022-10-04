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
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{

    public partial class SelectCallMonitoringAgentScreen
    {

        public long? SelectedDeclineReasonID { get; set; }
        private readonly LeadApplicationScreen _LeadApplicationScreen;
        DataTable dtSalesData = new DataTable();
        ObservableCollection<DataGridViewAgents> agentCollection = new ObservableCollection<DataGridViewAgents>();


        public SelectCallMonitoringAgentScreen(LeadApplicationScreen leadApplicationScreen)
        {
            InitializeComponent();
            _LeadApplicationScreen = leadApplicationScreen;
            LoadLookupData();

            #region Datagrid
            dtSalesData.Columns.Add("Description");
            dtSalesData.Columns.Add("FKUserID");
            this.SelectAgentDG.DataContext = dtSalesData;
            #endregion


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

            try
            {
                RebindData();
                SetTimer();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
            }

        }
        private void SetTimer()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 02);
            dispatcherTimer.Start();
        }
        protected void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            RebindData();
        }
        private void RebindData()
        {
            DataTable dtAgentsOnline = Methods.GetTableData("SELECT CASE WHEN [INCMAgentsOnline].[Online] = '1' THEN  [lkpINCMAgentForwardedSale].[Description] + ' - Available'ELSE [lkpINCMAgentForwardedSale].[Description] + ' - Unavailable' END AS [Description], [lkpINCMAgentForwardedSale].[FKUserID]  FROM [lkpINCMAgentForwardedSale] LEFT JOIN [INCMAgentsOnline] ON [lkpINCMAgentForwardedSale].[FKUserID] = [INCMAgentsOnline].[FKUserID] WHERE [INCMAgentsOnline].[Online] = 1 ORDER BY [INCMAgentsOnline].[Online] DESC ");


            //try { dtSalesData.Clear(); } catch { }
            //foreach (DataRow row in dtAgentsOnline.Rows)
            //{
            //    dtSalesData.Rows.Add(row.ItemArray);
            //}
            //SelectAgentDG.ItemsSource = dtSalesData.AsDataView();

            try { agentCollection.Clear(); } catch { }

            for (int x = 0; x < dtAgentsOnline.Rows.Count; x++)
            {
                agentCollection.Add(new DataGridViewAgents { Name = dtAgentsOnline.Rows[x][0].ToString(), FKUserID = dtAgentsOnline.Rows[x][1].ToString() });

            }
            ListCollectionView collectionView = new ListCollectionView(agentCollection);
            SelectAgentDG.DataSource = collectionView;
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

        private void EmbriantComboBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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

        private void SelectAgentDG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            string selectedUserid = "";
            try
            {
                selectedUserid = Convert.ToInt64(((DataRecord)SelectAgentDG.SelectedItems.Records[0]).Cells[1].Value).ToString();
            }
            catch
            {
                return;
            }

            #region Randomized Messages
            Random rnd = new Random();
            int month = rnd.Next(1, 8);



            #endregion

            #region Call Transfer %


            SqlParameter[] parameters =
            {
                    new SqlParameter("@FKUserID", GlobalSettings.ApplicationUser.ID),
                    new SqlParameter("@IsUpgrade", _LeadApplicationScreen.LaData.AppData.IsLeadUpgrade)
            };

            DataSet ds = null;


            ds = Methods.ExecuteStoredProcedureSaleReport("spGetDCCallTransferStatsFKUserID", parameters);
            DataTable firstTable = ds.Tables[0];
            string Percentage;

            try
            {
                decimal cellValue = decimal.Parse(firstTable.Rows[0][0].ToString()) * 100;
                decimal roundedCellValue = Math.Round(cellValue);
                int cellValueInt = decimal.ToInt32(roundedCellValue);
                Percentage = cellValueInt.ToString();
            }
            catch
            {
                Percentage = "0";

            }


            #endregion

            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
            {
                DebiCheckAgentLbl.Visibility = Visibility.Visible;
                TransferPercentageLbl.Visibility = Visibility.Visible;
                SelectAgentDG.Visibility = Visibility.Collapsed;
                headingSelectDeclineReason.Text = "Cheers to you for a job well done!";
                TransferPercentageTxtB.Text = "Your call transfer % is currently " + Percentage + "%";


                string DCAgentName = "To " + ((DataRecord)SelectAgentDG.SelectedItems.Records[0]).Cells[0].Value.ToString();
                string updatedDCName = DCAgentName.Replace("- Available", "");
                DebiCheckAgentLbl.Text = updatedDCName;

                MailImage.Visibility = Visibility.Visible;
                //btnClose.Visibility = Visibility.Collapsed;

                #region Animations
                SolidColorBrush brush = new SolidColorBrush((Color)FindResource("BrandedColourIN"));
                headingSelectDeclineReason.Foreground = brush;

                ColorAnimation anima = new ColorAnimation((Color)FindResource("BrandedColourIN"), Colors.White, new Duration(TimeSpan.FromMilliseconds(300)));
                anima.AutoReverse = true;
                anima.RepeatBehavior = RepeatBehavior.Forever;
                brush.BeginAnimation(SolidColorBrush.ColorProperty, anima);

                DoubleAnimation siz = new DoubleAnimation(18, 24, new Duration(TimeSpan.FromSeconds(2)));
                siz.AutoReverse = true;
                siz.RepeatBehavior = RepeatBehavior.Forever;
                MailImage.BeginAnimation(FontSizeProperty, siz);

                //inMessageBoxWindow.txtDescription.FontSize = 24;
                headingSelectDeclineReason.FontFamily = new FontFamily("Arial");
                #endregion

            });



            try
            {

                //if (SelectAgentDG.SelectedItems.Count == 0)
                //{
                //    return;
                //}

                //foreach (var data in SelectAgentDG.SelectedItems)
                //{
                //    DataGridViewAgents mydata = data as DataGridViewAgents;
                //    selectedUserid = mydata.FKUserID;

                //}


                SelectedDeclineReasonID = Convert.ToInt32(selectedUserid);


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


                    //TransferScreen ms = new TransferScreen();
                    //ShowDialog(ms, new INDialogWindow(ms));
                    //OnDialogClose(_dialogResult);

                    Task.Delay(2000).ContinueWith(_ =>
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            btnFinish.Visibility = Visibility.Visible;
                        });
                    });



                }
                else
                {
                    try
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            headingSelectDeclineReason.Text = "Oops, Agent just went offline.";

                            btnFinish.Visibility = Visibility.Visible;
                            btnFinish.Content = "Please retry";
                        });
                    }
                    catch
                    {

                    }

                }



            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
        }



        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }
    }
    public class DataGridViewAgents
    {
        public string Name { get; set; }
        public string FKUserID { get; set; }

        public bool Selected { get; set; }


    }
}
