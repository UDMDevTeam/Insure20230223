using Embriant.Framework;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using Infragistics.Windows.DataPresenter;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for MySuccessEditScreen.xaml
    /// </summary>
    public partial class MySuccessEditScreen
    {

        #region Variables
        List<string> UpgradeBaseList = new List<string>();
        List<string> DocumentTypeList = new List<string>();

        DataTable dtCampaigns;
        DataTable dtAgents;
        DataTable dtDocuments;
        DataTable dtAgentCalls;
        DataTable dtCampaignNotes;
        DataTable dtLanguageTypes;
        DataTable dtAgentNotes;

        DataTable dtAgentsNameList;


        private List<Record> _lstSelectedCampaigns;
        private string _fkCampaignIDs = null;
        public string _CampaignNoteIDs = "";
        public string _LanguageNoteIDs = "";
        public string _AgentNoteIDs = "";
        private byte[] fileData;
        private string _filePathAndName;
        private string _fileName;
        private string _fileNameOriginal;

        private const string ImportFilterExpression = "(*.XPS;.xls;.xlsx;.xlsm) | *.XPS;.xls;.xlsx;.xlsm";
        private const string ImportFileExtention = "(*.XPS)";

        private bool onlyUpgradeCampaigns = true;

        private long CampaignID;
        private long CampaignNotesID;
        private long LanguageTypeID;
        private long AgentNotesID;
        private string fileName;

        string VoiceCall1FilePath = "";
        string VoiceCall2FilePath = "";
        string VoiceCall3FilePath = "";

        public object[] parameters { get; private set; }

        #endregion

        public MySuccessEditScreen()
        {
            InitializeComponent();

            UpgradeBaseList.Clear();
            UpgradeBaseList.Add("Upgrade");
            UpgradeBaseList.Add("Base");
            cmbCampaignType.ItemsSource = UpgradeBaseList;
            cmbCampaignType1.ItemsSource = UpgradeBaseList;

            DocumentTypeList.Clear();
            DocumentTypeList.Add("Agent Calls");
            DocumentTypeList.Add("Agent Notes");
            DocumentTypeList.Add("Campaign Notes");
            cmbDocumentType.ItemsSource = DocumentTypeList;

            ResetScreenVB();


        }


        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OnDialogClose(_dialogResult);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }



        private void LoadDocumentInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);

                if (cmbDocumentType.SelectedValue.ToString() == "Agent Calls")
                {
                    LoadAgentCalls();
                }
                else if (cmbDocumentType.SelectedValue.ToString() == "Agent Notes")
                {
                    LoadAgentNotes();
                }
                else if (cmbDocumentType.SelectedValue.ToString() == "Campaign Notes")
                {
                    
                    LoadCampaignNotes();
                    
                }

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

        private void LoadCampaignNotes()
        {
            try
            {
                SetCursor(Cursors.Wait);

                try { dtCampaignNotes.Clear(); } catch { }

                cmbDocumentName.Visibility = Visibility.Visible;

                dtCampaignNotes = Methods.GetTableData("SELECT ID [ID], Description [Description] FROM lkpCampaignNotes");
                DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                dtCampaignNotes.Columns.Add(column);
                dtCampaignNotes.DefaultView.Sort = " [ID] ASC";

                List<string> DocumentNameList = dtCampaignNotes.AsEnumerable().Select(r => r.Field<string>("Description")).ToList();



                //List<Record> CampaignNoteIDList = dtCampaignNotes.AsEnumerable().Select(r => r.Field<Record>("ID")).ToList();

                //_CampaignNoteIDs = DocumentNameList.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                //_CampaignNoteIDs = _CampaignNoteIDs.Substring(0, _CampaignNoteIDs.Length - 1);

                //List<Record> CampaignNoteIDList = dtCampaignNotes.AsEnumerable().Select(r => r.Field<Record>("ID")).ToList();

                //_CampaignNoteIDs = CampaignNameList.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                //_CampaignNoteIDs = _CampaignNoteIDs.Substring(0, _CampaignNoteIDs.Length - 1);

                cmbDocumentName.ItemsSource = DocumentNameList;


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

        private void LoadLanguageTypes()
        {
            try
            {
                SetCursor(Cursors.Wait);

                try { dtLanguageTypes.Clear(); } catch { }

                cmbLanguageType.Visibility = Visibility.Visible;

                dtLanguageTypes = Methods.GetTableData("SELECT ID [ID], Description [Description] FROM lkpLanguage WHERE ID IN (1,2)");
                DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                dtLanguageTypes.Columns.Add(column);
                dtLanguageTypes.DefaultView.Sort = " [ID] ASC";

                List<string> LanguageNameList = dtLanguageTypes.AsEnumerable().Select(r => r.Field<string>("Description")).ToList();


                //List<Record> CampaignNoteIDList = dtCampaignNotes.AsEnumerable().Select(r => r.Field<Record>("ID")).ToList();

                //_CampaignNoteIDs = DocumentNameList.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                //_CampaignNoteIDs = _CampaignNoteIDs.Substring(0, _CampaignNoteIDs.Length - 1);

                //List<Record> CampaignNoteIDList = dtCampaignNotes.AsEnumerable().Select(r => r.Field<Record>("ID")).ToList();

                //_CampaignNoteIDs = CampaignNameList.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                //_CampaignNoteIDs = _CampaignNoteIDs.Substring(0, _CampaignNoteIDs.Length - 1);

                cmbLanguageType.ItemsSource = LanguageNameList;


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
        private void LoadAgentNotes()
        {
            try
            {
                SetCursor(Cursors.Wait);

                try { dtAgentNotes.Clear(); } catch { }

                cmbDocumentName.Visibility = Visibility.Visible;

                dtAgentNotes = Methods.GetTableData("SELECT ID [ID], Description [Description] FROM lkpAgentNotesMessages");
                DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                dtAgentNotes.Columns.Add(column);
                dtAgentNotes.DefaultView.Sort = "ID ASC";

                List<string> CampaignNameList = dtAgentNotes.AsEnumerable().Select(r => r.Field<string>("Description")).ToList();

                cmbDocumentName.ItemsSource = CampaignNameList;
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
        private void LoadAgentCalls()
        {
            try
            {
                SetCursor(Cursors.Wait);

                try { dtAgentCalls.Clear(); } catch { }

                cmbDocumentName.Visibility = Visibility.Visible;

                dtAgentCalls = Methods.GetTableData("SELECT ID [ID], Description [Description] FROM lkpAgentCalls");
                DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                dtAgentCalls.Columns.Add(column);
                dtAgentCalls.DefaultView.Sort = "ID ASC";

                List<string> CampaignNameList = dtAgentCalls.AsEnumerable().Select(r => r.Field<string>("Description")).ToList();

                cmbDocumentName.ItemsSource = CampaignNameList;
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

        private void cmbCampaignType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbCampaignType.SelectedValue.ToString() == "Base")
                {
                    onlyUpgradeCampaigns = false;
                    CommonControlData.PopulateCampaignComboBox(cmbCampaignName, onlyUpgradeCampaigns);
                }
                if (cmbCampaignType.SelectedValue.ToString() == "Upgrade")
                {
                    onlyUpgradeCampaigns = true;
                    CommonControlData.PopulateCampaignComboBox(cmbCampaignName, onlyUpgradeCampaigns);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            //LoadCampaignInfo();

        }

        private void cmbDocumentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //LoadDocumentInfo();
            try
            {

                if (cmbDocumentType.SelectedValue.ToString() == "Campaign Notes")
                {
                    CommonControlData.PopulateCampaignNotesComboBox(cmbDocumentName);
                }

                else if (cmbDocumentType.SelectedValue.ToString() == "Agent Notes")
                {
                    CommonControlData.PopulateAgentNotesComboBox(cmbDocumentName);
                }

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadDocumentFile();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void InsertCampaignDataIntoDatabase(long selectedCampaignID, long selectedCampaignNoteID, long selectedLanguageNoteID, byte[] selectedFileName)
        {

            try
            {

                StringBuilder query = new StringBuilder();

                INMySuccessCampaignDetails iNMySuccessCampaignDetails = new INMySuccessCampaignDetails();
                //iNMySuccessCampaignDetails.Fill();

                _fkCampaignIDs = selectedCampaignID.ToString();
                _CampaignNoteIDs = selectedCampaignNoteID.ToString();
                _LanguageNoteIDs = selectedLanguageNoteID.ToString(); 
                fileData = File.ReadAllBytes(selectedFileName.ToString());


                var sqlQuery = "Select[INMySuccessCampaignDetails].[ID] From INMySuccessCampaignDetails where[INMySuccessCampaignDetails].[FKCampaignID] = " + _fkCampaignIDs;
                DataTable dt = Methods.GetTableData(sqlQuery);

                //if (dt.Rows.Count == 0)
                //{
                //    ShowMessageBox(new INMessageBoxWindow1(), @"There is no record.", "Error", ShowMessageType.Error);
                //}

                long? id = dt.Rows[0]["ID"] as long?;

                GlobalSettings.ColumnIDMySuccessID = id.ToString();
                GlobalSettings.CampaignNotesID = _CampaignNoteIDs;
                GlobalSettings.LanguageNotesID = _LanguageNoteIDs; 
                //strSQL = "Select ID From INMySuccesCampaignDteails where FKCampaignID = " + _fkCampaignIDs;



                iNMySuccessCampaignDetails.ID = (long)id;
                iNMySuccessCampaignDetails.FKCampaignID = long.Parse(_fkCampaignIDs);
                iNMySuccessCampaignDetails.DocumentID = long.Parse(_CampaignNoteIDs);
                iNMySuccessCampaignDetails.LanguageID = long.Parse(_LanguageNoteIDs);
                iNMySuccessCampaignDetails.Document = fileData;

                iNMySuccessCampaignDetails.Save(_validationResult);



                ShowMessageBox(new INMessageBoxWindow1(), @"The file has been uploaded successfully!", "Success", ShowMessageType.Information);

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }


        }

        public void InsertAgentDataIntoDatabase(long selectedCampaignID, long selectedAgentNoteID, byte[] selectedFileName)
        {

            try
            {

                StringBuilder query = new StringBuilder();

                INMySuccessAgentDetails iNMySuccessAgentDetails = new INMySuccessAgentDetails();
                //iNMySuccessCampaignDetails.Fill();

                _fkCampaignIDs = selectedCampaignID.ToString();
                _AgentNoteIDs = selectedAgentNoteID.ToString();

                fileData = File.ReadAllBytes(selectedFileName.ToString());

                var sqlQuery = "Select[INMySuccessAgentsNotesDetails].[ID] From INMySuccessAgentsNotesDetails where[INMySuccessAgentsNotesDetails].[FKCampaignID] = " + _fkCampaignIDs;
                DataTable dt = Methods.GetTableData(sqlQuery);

                //if (dt.Rows.Count == 0)
                //{
                //    ShowMessageBox(new INMessageBoxWindow1(), @"There is no record.", "Error", ShowMessageType.Error);
                //}

                long? id = dt.Rows[0]["ID"] as long?;

                GlobalSettings.ColumnIDMySuccessID = id.ToString();
                GlobalSettings.AgentNotesID = _AgentNoteIDs;

                iNMySuccessAgentDetails.ID = (long)id;
                iNMySuccessAgentDetails.FKCampaignID = long.Parse(_fkCampaignIDs);
                iNMySuccessAgentDetails.DocumentID = long.Parse(_AgentNoteIDs);
                iNMySuccessAgentDetails.Document = fileData;

                iNMySuccessAgentDetails.Save(_validationResult);



                ShowMessageBox(new INMessageBoxWindow1(), @"The file has been uploaded successfully!", "Success", ShowMessageType.Information);

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }


        }

        public void LoadDocumentFile()
        {
            //OpenFileDialog fd = new OpenFileDialog();
            //fd.Title = "Select Document";
            //fd.Filter = "(*.XPS) | *.XPS";
            try
            {
                _filePathAndName = ShowOpenFileDialog(ImportFileExtention, ImportFilterExpression);

                if (_filePathAndName != string.Empty)
                {
                    //imgPhoto.Source = new BitmapImage(new Uri(fd.FileName));

                    _fileName = System.IO.Path.GetFileName(_filePathAndName);
                    string name = _fileNameOriginal = _fileName;

                    fileData = File.ReadAllBytes(_filePathAndName);

                    textBoxNotificationMessage.Text = "The file you have selected to upload is " + _fileName;
                    textBoxNotificationMessage.Visibility = Visibility.Visible;
                    //FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                    //BinaryReader br = new BinaryReader(fs);

                    //byte[] photo = br.ReadBytes((int)fs.Length);

                    //br.Close();
                    //fs.Close();


                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            //INMySuccessCampaignDetails iNMySuccessCampaignDetails = new INMySuccessCampaignDetails();

            //string FKCampaignID = GlobalSettings.CampaignID;

            //iNMySuccessCampaignDetails.FKCampaignID = long.Parse(FKCampaignID);
            //iNMySuccessCampaignDetails.Document = byte.Parse()

        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                if (cmbDocumentType.SelectedValue.ToString() == "Campaign Notes")
                {
                    InsertCampaignDataIntoDatabase(CampaignID, CampaignNotesID, LanguageTypeID, fileData);
                }
                else if (cmbDocumentType.SelectedValue.ToString() == "Agent Notes")
                {
                    InsertAgentDataIntoDatabase(CampaignID, AgentNotesID, fileData);
                }

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            //CampaignNotesID = long.Parse(GlobalSettings.CampaignNotesID);
        }

        private void cmbDocumentName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //LoadDocumentInfo();

            try
            {

                if (cmbDocumentType.SelectedValue.ToString() == "Campaign Notes")
                {
                    CampaignNotesID = long.Parse(cmbDocumentName.SelectedValue.ToString());
                    LoadLanguageTypes();
                }
                else if (cmbDocumentType.SelectedValue.ToString() == "Agent Notes")
                {
                    AgentNotesID = long.Parse(cmbDocumentName.SelectedValue.ToString());
                }

                //btnBrowse.Visibility = Visibility.Visible;
                //InsertDataIntoDatabase(selectedCampaignID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            //InsertDataIntoDatabase(selectedCampaignID); 
        }

        private void cmbCampaignName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                CampaignID = long.Parse(cmbCampaignName.SelectedValue.ToString());

                //InsertDataIntoDatabase(selectedCampaignID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void DocumentUploadBtn_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            Body2.Visibility = Visibility.Visible;
            //VoiceCallVB.Visibility = Visibility.Collapsed;
            VoiceNoteUploadVB.Visibility = Visibility.Collapsed;
        }

        private void VoiceCallUploadBtn_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            Body2.Visibility = Visibility.Collapsed;
            //VoiceCallVB.Visibility = Visibility.Visible;
            VoiceNoteUploadVB.Visibility = Visibility.Visible;
        }

        public void ResetScreenVB()
        {
            MainMenu.Visibility = Visibility.Visible;
            Body2.Visibility = Visibility.Collapsed;
            //VoiceCallVB.Visibility = Visibility.Collapsed;
            VoiceNoteUploadVB.Visibility = Visibility.Collapsed;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ResetScreenVB();
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

        private void LoadCampaignInfo()
        {
            if (cmbCampaignType1.Text == "Base")
            {
                try
                {
                    SetCursor(System.Windows.Input.Cursors.Wait);

                    try { dtCampaigns.Clear(); } catch { }

                    dtCampaigns = Methods.GetTableData("Select [C].[ID] [CampaignID], [C].[Name] [Campaign Name], [C].[Code] [CampaignCode] from INCampaign AS [C] LEFT JOIN lkpINCampaignGroup AS [CG] ON [C].[FKINCampaignGroupID] = [CG].[ID] WHERE [CG].[ID] NOT IN (1, 3, 4, 6, 24, 34, 21, 40, 22, 42, 25, 26, 39) and IsActive = 1");
                    DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                    dtCampaigns.Columns.Add(column);
                    dtCampaigns.DefaultView.Sort = "CampaignID ASC";
                    //xdgCampaigns.DataSource = dtCampaigns.DefaultView;
                    cmbCampaignName1.ItemsSource = dtCampaigns.DefaultView;
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
                    //xdgCampaigns.DataSource = dtCampaigns.DefaultView;
                    cmbCampaignName1.ItemsSource = dtCampaigns.DefaultView;
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

        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void HeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void RecordSelectorAgentCheckbox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnBack3_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Visible;
            Body2.Visibility = Visibility.Collapsed;
            //VoiceCallVB.Visibility = Visibility.Visible;
            VoiceNoteUploadVB.Visibility = Visibility.Collapsed;
        }

        private void btnToVNUpload_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            Body2.Visibility = Visibility.Collapsed;
            //VoiceCallVB.Visibility = Visibility.Collapsed;
            VoiceNoteUploadVB.Visibility = Visibility.Visible;
        }

        private void btnAgentSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbCampaignType1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbCampaignType1.SelectedValue.ToString() == "Base")
                {
                    onlyUpgradeCampaigns = false;
                    CommonControlData.PopulateCampaignComboBox(cmbCampaignName1, onlyUpgradeCampaigns);
                }
                if (cmbCampaignType1.SelectedValue.ToString() == "Upgrade")
                {
                    onlyUpgradeCampaigns = true;
                    CommonControlData.PopulateCampaignComboBox(cmbCampaignName1, onlyUpgradeCampaigns);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }



        private void cmbCampaignName1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var CampaignID = long.Parse(cmbCampaignName1.SelectedValue.ToString());

            try
            {
                SetCursor(System.Windows.Input.Cursors.Wait);
                string AgentIDString = "";

                try
                {
                    DataTable dtAgentsCampaignList = Methods.GetTableData("Select [UserID] FROM INMySuccessAgents WHERE [FKCampaignID] = " + CampaignID);
                    List<long> AgentUserIDsList = dtAgentsCampaignList.AsEnumerable().Select(r => r.Field<long>("UserID")).ToList();
                    foreach (var item in AgentUserIDsList)
                    {
                        AgentIDString = AgentIDString + item.ToString() + ",";
                    }

                }
                catch (Exception a)
                {

                }
                try
                {

                    try { AgentIDString = AgentIDString.Remove(AgentIDString.Length - 1, 1); } catch { }

                    dtAgentsNameList = Methods.GetTableData("Select [ID], [FirstName] + ' ' + [LastName] as [Name] FROM [User] where [FKUserType] = 2 and IsActive = 1 order by [Name] asc");
                    List<string> AgentNamesList = dtAgentsNameList.AsEnumerable().Select(r => r.Field<string>("Name")).ToList();

                    cmbAgentnames.ItemsSource = AgentNamesList;
                }
                catch
                {
                    cmbAgentnames.ItemsSource = null;
                }
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

        private void cmbAgentnames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }



        private void btnVoiceCallBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VoiceCall1FilePath = ShowOpenFileDialog("", "");

                if (VoiceCall1FilePath != string.Empty)
                {
                    var _fileName = System.IO.Path.GetFileName(VoiceCall1FilePath);
                    string name =  _fileName;
                    LblVoiceCall1.Content = name;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnVoiceCallBrowse2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VoiceCall2FilePath = ShowOpenFileDialog("", "");

                if (VoiceCall2FilePath != string.Empty)
                {
                    var _fileName = System.IO.Path.GetFileName(VoiceCall2FilePath);
                    string name = _fileName;
                    LblVoiceCall2.Content = name;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnVoiceCallBrowse3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VoiceCall3FilePath = ShowOpenFileDialog("", "");

                if (VoiceCall3FilePath != string.Empty)
                {
                    var _fileName = System.IO.Path.GetFileName(VoiceCall3FilePath);
                    string name = _fileName;
                    LblVoiceCall3.Content = name;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnSaveVoiceCalls_Click(object sender, RoutedEventArgs e)
        {
            string expression;
            expression = "Name = '" + cmbAgentnames.SelectedValue.ToString() + "'";
            DataRow[] foundRows;
            foundRows = dtAgentsNameList.Select(expression);
            DataTable dtCampaignsCalls = null;
            try { dtCampaignsCalls = Methods.GetTableData("Select Top 1 [ID] FROM INMySuccessAgents WHERE FKCampaignID = " + int.Parse(cmbCampaignName1.SelectedValue.ToString()) + " AND UserID = " + int.Parse(foundRows[0].ItemArray[0].ToString())); } catch { }
            List<long> AgentCallsList = dtCampaignsCalls.AsEnumerable().Select(r => r.Field<long>("ID")).ToList();



            //if (AgentCallsList.Count == 0)
            //{

            //    //INMySuccessAgents db = new INMySuccessAgents();
            //    try
            //    {

            //        db.FKCampaignID = int.Parse(cmbCampaignName1.SelectedValue.ToString());
            //        db.UserID = int.Parse(foundRows[0].ItemArray[0].ToString());
            //        db.Call1 = VoiceCall1FilePath;
            //        db.Call2 = VoiceCall2FilePath;
            //        db.Call3 = VoiceCall3FilePath;
            //        db.Save(_validationResult);

            //        ShowMessageBox(new INMessageBoxWindow1(), @"The file has been uploaded successfully!", "Success", ShowMessageType.Information);
            //        try { ResetVCScreen(); } catch { }
            //    }
            //    catch
            //    {
            //        ShowMessageBox(new INMessageBoxWindow1(), @"The file has been uploaded unsuccessfully!", "Unsuccess", ShowMessageType.Error);
            //    }
            //}
            //else
            //{
            //    INMySuccessAgents db = new INMySuccessAgents(int.Parse(AgentCallsList[0].ToString()));
            //    try
            //    {

            //        db.FKCampaignID = int.Parse(cmbCampaignName1.SelectedValue.ToString());
            //        db.UserID = int.Parse(foundRows[0].ItemArray[0].ToString());
            //        db.Call1 = VoiceCall1FilePath;
            //        db.Call2 = VoiceCall2FilePath;
            //        db.Call3 = VoiceCall3FilePath;
            //        db.Save(_validationResult);

            //        ShowMessageBox(new INMessageBoxWindow1(), @"The file has been uploaded successfully!", "Success", ShowMessageType.Information);
            //        try { ResetVCScreen(); } catch { }
            //    }
            //    catch (Exception a)
            //    {
            //        ShowMessageBox(new INMessageBoxWindow1(), @"The file has been uploaded unsuccessfully!", "Unsuccess", ShowMessageType.Error);
            //    }
            //}
        }

        public void ResetVCScreen()
        {
            cmbAgentnames.Text = "";
            LblVoiceCall1.Content = "";
            LblVoiceCall2.Content = "";
            LblVoiceCall3.Content = "";
        }

        private void cmbLanguageType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                    LanguageTypeID = long.Parse(cmbLanguageType.SelectedValue.ToString());
                
                //else if (cmbDocumentType.SelectedValue.ToString() == "Agent Notes")
                //{
                //    LanguageTypeID = long.Parse(cmbLanguageType.SelectedValue.ToString());
                //}

                btnBrowse.Visibility = Visibility.Visible;
                //InsertDataIntoDatabase(selectedCampaignID);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnCopyCampaignData_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                MySuccessCopyCampaignData mySuccessCopyCampaignData = new MySuccessCopyCampaignData();
                ShowDialog(mySuccessCopyCampaignData, new INDialogWindow(mySuccessCopyCampaignData));
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }



        }
    }
}
