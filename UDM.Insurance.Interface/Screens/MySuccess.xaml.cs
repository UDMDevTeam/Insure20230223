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
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Media;
using Embriant.WPF.Windows;
using System.Diagnostics;
using System.Windows.Navigation;

namespace UDM.Insurance.Interface.Screens
{

    public partial class MySuccess
    {

        #region Variables
        List<string> UpgradeBaseList = new List<string>();

        private TimeSpan TotalTime;

        DataTable dtCampaigns;
        DataTable dtCampaignNotes;
        DataTable dtCampaignsCalls;
        DataTable dtAgentNotes; 

        DataTable dtAgents;
        private DataTable dtAgentCallsDG;

        private List<Record> _lstSelectedCampaigns;
        private List<Record> _lstSelectedCampaignNotes;
        private string _fkCampaignIDs = "";
        public string _CampaignNoteIDs = "";
        public string _AgentNoteIDs = "";


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
            UpgradeBaseList.Add("Base");
            UpgradeBaseList.Add("Upgrade");         
            cmbBaseUpgrade.ItemsSource = UpgradeBaseList;

            timeSlider.AddHandler(MouseLeftButtonUpEvent,
                      new MouseButtonEventHandler(timeSlider_MouseLeftButtonUp),
                      true);
        }

        #endregion Constructor

        #region Private Methods


        #endregion Private Methods

        #region Event Handlers

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
            if(MediaplayerVB.Visibility == Visibility.Visible)
            {
                MySuccess mySuccess = new MySuccess();
                mySuccess.Body.Visibility = Visibility.Collapsed;
                mySuccess.Body2.Visibility = Visibility.Visible;
                mySuccess.LoadAgentCalls();
                mySuccess.LoadAgentNotesDG();
                mySuccess.LoadCampaignNotesDG();
                ShowDialog(mySuccess, new INDialogWindow(mySuccess));
            }

        }

        #endregion Event Handlers

        private void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void cmbBaseUpgrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                LoadCampaignInfo();

            }
            catch
            {

            }
        }

        #region Load Datagrids

        public void LoadAgentCalls()
        {
            try
            {
                SetCursor(System.Windows.Input.Cursors.Wait);

                try { dtCampaignsCalls.Clear(); } catch { }

                dtCampaignsCalls = Methods.GetTableData("SELECT ID [ID], Description [Description] FROM lkpAgentCalls");
                DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                dtCampaignsCalls.Columns.Add(column);
                dtCampaignsCalls.DefaultView.Sort = "ID ASC";
                xdgAgentCalls.DataSource = dtCampaignsCalls.DefaultView;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SetCursor(System.Windows.Input.Cursors.Arrow);
            }
        }

        public void LoadAgentNotesDG()
        {
            try
            {
                SetCursor(System.Windows.Input.Cursors.Wait);

                try { dtAgentNotes.Clear(); } catch { }

                dtAgentNotes = Methods.GetTableData("SELECT ID [ID], Description [Description] FROM lkpAgentNotesMessages");
                DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                dtAgentNotes.Columns.Add(column);
                dtAgentNotes.DefaultView.Sort = "ID ASC";
                xdgAgentNotes.DataSource = dtAgentNotes.DefaultView;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SetCursor(System.Windows.Input.Cursors.Arrow);
            }
        }

        private void LoadCampaignInfo()
        {
            if(cmbBaseUpgrade.Text == "Base")
            {
                try
                {
                    SetCursor(System.Windows.Input.Cursors.Wait);

                    try { dtCampaigns.Clear(); } catch { }

                    dtCampaigns = Methods.GetTableData("Select [C].[ID] [CampaignID], [C].[Name] [Campaign Name], [C].[Code] [CampaignCode] from INCampaign AS [C] LEFT JOIN lkpINCampaignGroup AS [CG] ON [C].[FKINCampaignGroupID] = [CG].[ID] WHERE [CG].[ID] NOT IN (1, 3, 4, 6, 24, 34, 21, 40, 22, 42, 25, 26, 39)");
                    DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                    dtCampaigns.Columns.Add(column);
                    dtCampaigns.DefaultView.Sort = "CampaignID ASC";
                    xdgCampaigns.DataSource = dtCampaigns.DefaultView;
                }

                catch (Exception ex)
                {
                    HandleException(ex);
                }

                finally
                {
                    SetCursor(System.Windows.Input.Cursors.Arrow);
                }
            }
            else
            {
                try
                {
                    SetCursor(System.Windows.Input.Cursors.Wait);

                    try { dtCampaigns.Clear(); } catch { }

                    dtCampaigns = Methods.GetTableData("Select [C].[ID] [CampaignID], [C].[Name] [Campaign Name], [C].[Code] [CampaignCode] from INCampaign AS [C] LEFT JOIN lkpINCampaignGroup AS [CG] ON [C].[FKINCampaignGroupID] = [CG].[ID] WHERE CG.ID IN (1, 3, 4, 6, 24, 34, 21, 40, 22, 42, 25, 26, 39)");
                    DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                    dtCampaigns.Columns.Add(column);
                    dtCampaigns.DefaultView.Sort = "CampaignID ASC";
                    xdgCampaigns.DataSource = dtCampaigns.DefaultView;
                }

                catch (Exception ex)
                {
                    HandleException(ex);
                }

                finally
                {
                    SetCursor(System.Windows.Input.Cursors.Arrow);
                }
            }

        }

        public void LoadAgentDG()
        {
            try
            {
                SetCursor(System.Windows.Input.Cursors.Wait);

                try { dtAgents.Clear(); } catch { }

                var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();


                _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Campaign Name"].Value));

                //if (_lstSelectedCampaigns.Count == 0)
                //{
                //    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No Campaigns Selected", ShowMessageType.Error);
                //    return;
                //}
                //else
                //{

                    _fkCampaignIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                    _fkCampaignIDs = _fkCampaignIDs.Substring(0, _fkCampaignIDs.Length - 1);

                GlobalSettings.MySuccessCampaignID = _fkCampaignIDs;
                //}

                dtAgents = Methods.GetTableData(
                    "SELECT [INMySuccessAgents].[UserID] AS [AgentID], (SELECT [User].[FirstName] + ' ' + [User].[LastName] FROM [User] WHERE [User].[ID] = [INMySuccessAgents].[UserID] ) AS [AgentName] " +
                    "FROM [INMySuccessAgents]" +
                    "WHERE [INMySuccessAgents].[FKCampaignID] in (" + _fkCampaignIDs + ")");
                DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                dtAgents.Columns.Add(column);
                dtAgents.DefaultView.Sort = "[AgentName] ASC";
                xdgAgents.DataSource = dtAgents.DefaultView;

                GlobalSettings.CampaignID = _fkCampaignIDs;

            }

            catch (Exception ex)
            {
            }

            finally
            {
                SetCursor(System.Windows.Input.Cursors.Arrow);
            }
        }


        #endregion

        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var listTemp = (from r in xdgCampaignNotes.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                var _listSelectedAgentNotes = new List<Record>(listTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));
                var _CampaignNoteIDs = _listSelectedAgentNotes.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                _CampaignNoteIDs = _CampaignNoteIDs.Substring(0, _CampaignNoteIDs.Length - 1);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void HeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadAgentDG();
            }
            catch (Exception ex)
            {
            }
        }

        #region Navigation


        private void btnNext_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            McMediaElement.Stop();

            Body.Visibility = Visibility.Collapsed;
            Body2.Visibility = Visibility.Visible;
            MediaplayerVB.Visibility = Visibility.Collapsed;
        }
        #endregion

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

        #region Variables
        private List<Record> _lstSelectedCampaignNote;
        #endregion
        #region Load Datagrid
        public void LoadCampaignNotesDG()
        {
            try
            {

                
                SetCursor(System.Windows.Input.Cursors.Wait);
                try { dtCampaignNotes.Clear(); } catch { }



                xdgCampaignNotes.DataSourceResetBehavior = DataSourceResetBehavior.DiscardExistingRecords;
                xdgCampaignNotes.DataItems.Clear();
                xdgCampaignNotes.DataSource = null;


                var lstTemp = (from r in xdgCampaignNotes.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                    
                dtCampaignNotes = Methods.GetTableData("SELECT ID [ID], Description [Description] FROM lkpCampaignNotes");
                DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                dtCampaignNotes.Columns.Add(column);


                dtCampaignNotes.DefaultView.Sort = "ID ASC";

                //for (int rowIndex = 0; rowIndex < dtCampaignNotes.Rows.Count; rowIndex++)
                //{
                //    dtCampaignNotes.Rows[rowIndex][0] = false;
                //}
                xdgCampaignNotes.DataSource = dtCampaignNotes.DefaultView;



                DataTable dt = ((DataView)xdgCampaignNotes.DataSource).Table;
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = true;
                }

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = false;
                }

                xdgCampaignNotes.DataSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                SetCursor(System.Windows.Input.Cursors.Arrow);
            }
        }

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
            //try
            //{


            //        var lstTemp = (from r in xdgCampaignNotes.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();

            //    if (lstTemp.Count == 0)
            //    {  
                    
            //    }
            //        _lstSelectedCampaignNote = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

            //        _CampaignNoteIDs = _lstSelectedCampaignNote.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
            //        _CampaignNoteIDs = _CampaignNoteIDs.Substring(0, _CampaignNoteIDs.Length - 1);

            //        var listTemp = (from r in xdgAgentNotes.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            //        var _listSelectedAgentNotes = new List<Record>(listTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

            //        //var _NotesDescription = _listSelectedAgentNotes.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
            //        //_NotesDescription = _NotesDescription.Substring(0, _NotesDescription.Length - 1);

            //        MySuccessCampaignNotes mySuccessCampaignNotes = new MySuccessCampaignNotes(_CampaignNoteIDs);
            //        ShowDialog(mySuccessCampaignNotes, new INDialogWindow(mySuccessCampaignNotes));

            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
        }
        #endregion
        #endregion

        #region MediaPlayer
        //private void btnNext3_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        OpenFileDialog dlg = new OpenFileDialog();
        //        dlg.InitialDirectory = "c:\\";
        //        dlg.Filter = "Media files (*.wmv)|*.wmv|All Files (*.*)|*.*";
        //        dlg.RestoreDirectory = true;

        //        if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            string selectedFileName = dlg.FileName;
        //            FileNameLabel.Content = selectedFileName;
        //            McMediaElement.Source = new Uri("\\\\192.168.2.141\\Devs\\Angelo_CancerElite_FINObj.wav");
        //            McMediaElement.Play();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}






        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }


        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            McMediaElement.Play();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            McMediaElement.Pause();

        }


        private void McMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {

        }

        private void McMediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        private void RecordSelectorAgentCheckbox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RecordSelectorAgentCheckbox_Click_1(object sender, RoutedEventArgs e)
        {
            var listTemp = (from r in xdgAgents.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();



            string AgentID = listTemp.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["AgentID"].Value + ",");
            AgentID = AgentID.Substring(0, AgentID.Length - 1);

            GlobalSettings.MySuccessAgentID = AgentID;

            if (listTemp.Count == 0)
            {

            }
            else
            {
                LoadAgentNotesDG();
                LoadAgentCalls();
                LoadCampaignNotesDG();


                Body.Visibility = Visibility.Collapsed;
                Body2.Visibility = Visibility.Visible;
                MediaplayerVB.Visibility = Visibility.Collapsed;
            }
        }

        private void RecordSelectorAgentNotesCheckbox_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                var listTemp = (from r in xdgAgentNotes.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                var _listSelectedAgentNotes = new List<Record>(listTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

                var _AgentNotesID = _listSelectedAgentNotes.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                _AgentNotesID = _AgentNotesID.Substring(0, _AgentNotesID.Length - 1);

                MySuccessAgentNotes mySuccessAgentNotes = new MySuccessAgentNotes(_AgentNotesID);
                ShowDialog(mySuccessAgentNotes, new INDialogWindow(mySuccessAgentNotes));
                    
            }

            catch (Exception ex)
            {
            }

            //    }
            //    else if (_NotesDescription == "2")
            //    {
            //        try
            //        {
            //            MySuccessCampaignNotes mySuccess = new MySuccessCampaignNotes(_fkCampaignIDs);
            //            ShowDialog(mySuccess, new INDialogWindow(mySuccess));
            //        }

            //        catch (Exception ex)
            //        {
            //            HandleException(ex);
            //        }
            //    }
            //    else if (_NotesDescription == "3")
            //    {
            //        try
            //        {
            //            MySuccessCampaignNotes mySuccess = new MySuccessCampaignNotes(_fkCampaignIDs);
            //            ShowDialog(mySuccess, new INDialogWindow(mySuccess));
            //        }

            //        catch (Exception ex)
            //        {
            //            HandleException(ex);
            //        }
            //    }
            //    else
            //    {

            //    }
            //}
            //catch
            //{

            //}

        }

        private void RecordSelectorAgentCallsCheckbox_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var listTemp = (from r in xdgAgentCalls.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                var _listSelectedAgentCalls = new List<Record>(listTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

                var _CallsDescription = _listSelectedAgentCalls.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                _CallsDescription = _CallsDescription.Substring(0, _CallsDescription.Length - 1);

                List<Record> listAgents = new List<Record>();
                List<Record> _listSelectedAgent = new List<Record>();
                List<Record> listCampaign = new List<Record>();
                List<Record> _listSelectedCampaign = new List<Record>();
                string _AgentSelected;
                string _CampaignSelected;
                try
                {
                    listAgents = (from r in xdgAgents.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                    _listSelectedAgent = new List<Record>(listAgents.OrderBy(r => ((DataRecord)r).Cells["AgentID"].Value));

                    _AgentSelected = _listSelectedAgent.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["AgentID"].Value + ",");
                    _AgentSelected = _AgentSelected.Substring(0, _AgentSelected.Length - 1);

                    listCampaign = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                    _listSelectedCampaign = new List<Record>(listCampaign.OrderBy(r => ((DataRecord)r).Cells["CampaignID"].Value));

                    _CampaignSelected = _listSelectedCampaign.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                    _CampaignSelected = _CampaignSelected.Substring(0, _CampaignSelected.Length - 1);
                }
                catch
                {
                    _AgentSelected = GlobalSettings.MySuccessAgentID;
                    _CampaignSelected = GlobalSettings.MySuccessCampaignID;
                }




                

                if (_CallsDescription == "1")
                {
                    Body.Visibility = Visibility.Collapsed;
                    Body2.Visibility = Visibility.Collapsed;
                    MediaplayerVB.Visibility = Visibility.Visible;

                    string FileNameFromDB = Convert.ToString(Methods.GetTableData("SELECT Call1 FROM INMySuccessAgents WHERE FKCampaignID =" + "'" + _CampaignSelected + "'" + " AND UserID =" + _AgentSelected).Rows[0][0]);

                    McMediaElement.Source = new Uri(FileNameFromDB);
                    McMediaElement.Play();

                    TimerLbl.Content = McMediaElement.Position.ToString();
                }
                else if (_CallsDescription == "2")
                {
                    Body.Visibility = Visibility.Collapsed;
                    Body2.Visibility = Visibility.Collapsed;
                    MediaplayerVB.Visibility = Visibility.Visible;

                    string FileNameFromDB = Convert.ToString(Methods.GetTableData("SELECT Call2 FROM INMySuccessAgents WHERE FKCampaignID =" + "'" + _CampaignSelected + "'" + " AND UserID =" + _AgentSelected).Rows[0][0]);

                    McMediaElement.Source = new Uri(FileNameFromDB);
                    McMediaElement.Play();

                    TimerLbl.Content = McMediaElement.Position.ToString();

                }
                else if (_CallsDescription == "3")
                {
                    Body.Visibility = Visibility.Collapsed;
                    Body2.Visibility = Visibility.Collapsed;
                    MediaplayerVB.Visibility = Visibility.Visible;

                    string FileNameFromDB = Convert.ToString(Methods.GetTableData("SELECT Call3 FROM INMySuccessAgents WHERE FKCampaignID =" + "'" + _CampaignSelected + "'" + " AND UserID =" + _AgentSelected).Rows[0][0]);

                    McMediaElement.Source = new Uri(FileNameFromDB);
                    McMediaElement.Play();

                    TimerLbl.Content = McMediaElement.Position.ToString();

                }
                else
                {

                }
            }
            catch
            {

            }

        }

        private void RecordSelectorCampaignNotesCheckbox_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                var lstTemp = (from r in xdgCampaignNotes.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();

                _lstSelectedCampaignNote = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

                //if (_lstSelectedCampaignNote.Count == 0 || _lstSelectedCampaignNote.Count >= 2)
                //{
                //    ShowMessageBox(new INMessageBoxWindow1(), "Please Select Only One(1) Campaign Note From The List.", "Campaign Notes Selected", ShowMessageType.Error);
                //    return;
                //}

                //else
                //{

                    _CampaignNoteIDs = _lstSelectedCampaignNote.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                    _CampaignNoteIDs = _CampaignNoteIDs.Substring(0, _CampaignNoteIDs.Length - 1);

                    var listTemp = (from r in xdgAgentNotes.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                    var _listSelectedAgentNotes = new List<Record>(listTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

                //var _NotesDescription = _listSelectedAgentNotes.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                //_NotesDescription = _NotesDescription.Substring(0, _NotesDescription.Length - 1);



                MySuccessCampaignNotes mySuccessCampaignNotes = new MySuccessCampaignNotes(_CampaignNoteIDs);
                ShowDialog(mySuccessCampaignNotes, new INDialogWindow(mySuccessCampaignNotes));

                    

                //}
            }

            catch (Exception ex)
            {
            }
        }
        private void btnFastForward_Click(object sender, RoutedEventArgs e)
        {
            var item1 = McMediaElement.Position;
            TimeSpan Time = new TimeSpan(0, 0, 10);
            McMediaElement.Position = item1.Add(Time);
        }
        private void btnRewind_Click(object sender, RoutedEventArgs e)
        {
            var item1 = McMediaElement.Position;
            TimeSpan Time = new TimeSpan(0, 0, 10);
            McMediaElement.Position = item1.Subtract(Time);
        }

        private void McMediaElement_MediaOpened_1(object sender, RoutedEventArgs e)
        {
            TotalTime = McMediaElement.NaturalDuration.TimeSpan;
            timeSlider.Maximum = 1;
            // Create a timer that will update the counters and the time slider
            DispatcherTimer timerVideoTime = new DispatcherTimer();
            timerVideoTime.Interval = TimeSpan.FromSeconds(1);
            timerVideoTime.Tick += new EventHandler(timer_Tick);
            timerVideoTime.Start();

        }

        private void timeSlider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (TotalTime.TotalSeconds > 0)
            {
                McMediaElement.Position = TimeSpan.FromSeconds(timeSlider.Value * TotalTime.TotalSeconds);
            }

        }

        void timer_Tick(object sender, EventArgs e)
        {

            // Check if the movie finished calculate it's total time
            if (McMediaElement.NaturalDuration.TimeSpan.TotalSeconds > 0)
            {
                if (TotalTime.TotalSeconds > 0)
                {
                    // Updating time slider
                    timeSlider.Value = McMediaElement.Position.TotalSeconds /
                                       TotalTime.TotalSeconds;
                }
            }

            TimerLbl.Content = McMediaElement.Position.ToString();

        }
    }

}