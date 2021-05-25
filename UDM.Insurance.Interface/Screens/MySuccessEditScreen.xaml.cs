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
        DataTable dtDocuments;
        DataTable dtAgentCalls;
        DataTable dtCampaignNotes;
        DataTable dtAgentNotes;


        private List<Record> _lstSelectedCampaigns;
        private string _fkCampaignIDs = null;
        public string _CampaignNoteIDs = "";
        public string _AgentNoteIDs = "";
        private byte[] fileData;
        private string _filePathAndName;
        private string _fileName;
        private string _fileNameOriginal;

        private const string ImportFilterExpression = "(*.XPS) | *.XPS";
        private const string ImportFileExtention = "(*.XPS)";

        private bool onlyUpgradeCampaigns = true;

        private long CampaignID;
        private long CampaignNotesID;
        private long AgentNotesID;
        private string fileName; 

        public object[] parameters { get; private set; }

        #endregion

        public MySuccessEditScreen()
        {
            InitializeComponent();

            UpgradeBaseList.Clear();
            UpgradeBaseList.Add("Upgrade");
            UpgradeBaseList.Add("Base");
            cmbCampaignType.ItemsSource = UpgradeBaseList;

            DocumentTypeList.Clear();
            DocumentTypeList.Add("Agent Calls");
            DocumentTypeList.Add("Agent Notes");
            DocumentTypeList.Add("Campaign Notes");
            cmbDocumentType.ItemsSource = DocumentTypeList;

        }


        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            //var MySuccessEditScreen = new MySuccessEditScreen(); 
            //OnClose(MySuccessEditScreen);

            //ShowMessageBox(new INMessageBoxWindow1(), @"The file has been uploaded successfully!", "Success", ShowMessageType.Information);
            try
            {
                OnDialogClose(_dialogResult);
            }
            catch (Exception ex) 
            {
                HandleException(ex); 
            }
        }

        private void LoadCampaignInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);

                //try { dtCampaigns.Clear(); } catch { }

                //cmbCampaignName.Visibility = Visibility.Visible;

                //dtCampaigns = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign");
                //DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                //dtCampaigns.Columns.Add(column);
                //dtCampaigns.DefaultView.Sort = "CampaignID ASC";

                //List<Int64> CampaignIDList = dtCampaigns.AsEnumerable().Select(r => r.Field<Int64>("CampaignID")).ToList();
                //List<string> CampaignNameList = dtCampaigns.AsEnumerable().Select(r => r.Field<string>("CampaignName")).ToList();

                CommonControlData.PopulateCampaignComboBox(cmbCampaignName);

                //cmbCampaignName.ItemsSource = CampaignNameList;



                //var CampaignID = cmbCampaignName.SelectedValue.ToString();
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

        public void InsertCampaignDataIntoDatabase(long selectedCampaignID, long selectedCampaignNoteID, byte[] selectedFileName) 
        {

            try
            {

                StringBuilder query = new StringBuilder();

                INMySuccessCampaignDetails iNMySuccessCampaignDetails = new INMySuccessCampaignDetails();
                //iNMySuccessCampaignDetails.Fill();

                _fkCampaignIDs = selectedCampaignID.ToString();
                _CampaignNoteIDs = selectedCampaignNoteID.ToString(); 
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

                //strSQL = "Select ID From INMySuccesCampaignDteails where FKCampaignID = " + _fkCampaignIDs;

                    

                    iNMySuccessCampaignDetails.ID = (long)id;
                    iNMySuccessCampaignDetails.FKCampaignID = long.Parse(_fkCampaignIDs);
                    iNMySuccessCampaignDetails.DocumentID = long.Parse(_CampaignNoteIDs);
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
                    InsertCampaignDataIntoDatabase(CampaignID, CampaignNotesID, fileData);
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
                }
                else if (cmbDocumentType.SelectedValue.ToString() == "Agent Notes")
                {
                    AgentNotesID = long.Parse(cmbDocumentName.SelectedValue.ToString());
                }

                btnBrowse.Visibility = Visibility.Visible;
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
    }
}
