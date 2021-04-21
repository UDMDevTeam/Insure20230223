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

        DataTable dtCampaigns;
        DataTable dtCampaignNotes;
        DataTable dtCampaignsCalls;

        DataTable dtAgents;
        private DataTable dtAgentCallsDG;

        private List<Record> _lstSelectedCampaigns;
        private string _fkCampaignIDs = "";
        public string _CampaignNoteIDs = "";

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

        private void LoadAgentCalls()
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

        private void LoadAgentNotesDG()
        {
            try
            {
                SetCursor(System.Windows.Input.Cursors.Wait);

                try { dtCampaignNotes.Clear(); } catch { }

                dtCampaignNotes = Methods.GetTableData("SELECT ID [ID], Description [Description] FROM lkpAgentNotesMessages");
                DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                dtCampaignNotes.Columns.Add(column);
                dtCampaignNotes.DefaultView.Sort = "ID ASC";
                xdgAgentNotes.DataSource = dtCampaignNotes.DefaultView;
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
                    SetCursor(System.Windows.Input.Cursors.Arrow);
                }
            }
            else
            {
                try
                {
                    SetCursor(System.Windows.Input.Cursors.Wait);

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
                    SetCursor(System.Windows.Input.Cursors.Arrow);
                }
            }

        }

        private void LoadAgentDG()
        {
            try
            {
                SetCursor(System.Windows.Input.Cursors.Wait);

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

                GlobalSettings.CampaignID = _fkCampaignIDs;

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
                HandleException(ex);
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
                dtCampaignNotes = Methods.GetTableData("SELECT ID [ID], Description [Description] FROM lkpCampaignNotes");
                DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                dtCampaignNotes.Columns.Add(column);
                dtCampaignNotes.DefaultView.Sort = "Description ASC";
                xdgCampaignNotes.DataSource = dtCampaignNotes.DefaultView;
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
            try
            {
                var lstTemp = (from r in xdgCampaignNotes.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();

                _lstSelectedCampaignNote = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

                _CampaignNoteIDs = _lstSelectedCampaignNote.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                _CampaignNoteIDs = _CampaignNoteIDs.Substring(0, _CampaignNoteIDs.Length - 1);

                var listTemp = (from r in xdgAgentNotes.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                var _listSelectedAgentNotes = new List<Record>(listTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

                //var _NotesDescription = _listSelectedAgentNotes.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                //_NotesDescription = _NotesDescription.Substring(0, _NotesDescription.Length - 1);

                MySuccessCampaignNotes mySuccessCampaignNotes = new MySuccessCampaignNotes(_CampaignNoteIDs);
                ShowDialog(mySuccessCampaignNotes, new INDialogWindow(mySuccessCampaignNotes));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        #endregion
        #endregion

        #region MediaPlayer
        private void btnNext3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.InitialDirectory = "c:\\";
                dlg.Filter = "Media files (*.wmv)|*.wmv|All Files (*.*)|*.*";
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedFileName = dlg.FileName;
                    FileNameLabel.Content = selectedFileName;
                    McMediaElement.Source = new Uri(selectedFileName);
                    McMediaElement.Play();
                }
            }
            catch (Exception ex)
            {

            }
        }






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

                var _NotesDescription = _listSelectedAgentNotes.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                _NotesDescription = _NotesDescription.Substring(0, _NotesDescription.Length - 1);


                if (_NotesDescription == "1")
                {
                    try
                    {
                        MySuccessCampaignNotes mySuccess = new MySuccessCampaignNotes(_fkCampaignIDs);
                        ShowDialog(mySuccess, new INDialogWindow(mySuccess));
                    }

                    catch (Exception ex)
                    {
                        HandleException(ex);
                    }
                }
                else if (_NotesDescription == "2")
                {
                    try
                    {
                        MySuccessCampaignNotes mySuccess = new MySuccessCampaignNotes(_fkCampaignIDs);
                        ShowDialog(mySuccess, new INDialogWindow(mySuccess));
                    }

                    catch (Exception ex)
                    {
                        HandleException(ex);
                    }
                }
                else if (_NotesDescription == "3")
                {
                    try
                    {
                        MySuccessCampaignNotes mySuccess = new MySuccessCampaignNotes(_fkCampaignIDs);
                        ShowDialog(mySuccess, new INDialogWindow(mySuccess));
                    }

                    catch (Exception ex)
                    {
                        HandleException(ex);
                    }
                }
                else
                {

                }
            }
            catch
            {

            }

        }

        private void RecordSelectorAgentCallsCheckbox_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var listTemp = (from r in xdgAgentCalls.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                var _listSelectedAgentCalls = new List<Record>(listTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

                var _CallsDescription = _listSelectedAgentCalls.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                _CallsDescription = _CallsDescription.Substring(0, _CallsDescription.Length - 1);

                if (_CallsDescription == "1")
                {
                    Body.Visibility = Visibility.Collapsed;
                    Body2.Visibility = Visibility.Collapsed;
                    MediaplayerVB.Visibility = Visibility.Visible;
                }
                else if (_CallsDescription == "2")
                {
                    Body.Visibility = Visibility.Collapsed;
                    Body2.Visibility = Visibility.Collapsed;
                    MediaplayerVB.Visibility = Visibility.Visible;
                }
                else if (_CallsDescription == "3")
                {
                    Body.Visibility = Visibility.Collapsed;
                    Body2.Visibility = Visibility.Collapsed;
                    MediaplayerVB.Visibility = Visibility.Visible;
                }
                else
                {

                }
            }
            catch
            {

            }

        }
    }

}