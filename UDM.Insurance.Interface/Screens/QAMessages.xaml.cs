using Embriant.Framework;
using Embriant.Framework.Configuration;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using static System.Net.Mime.MediaTypeNames;
using MessageBox = System.Windows.MessageBox;

namespace UDM.Insurance.Interface.Screens
{

    public partial class QAMessages
    {
        DataSet _dsLookups;
        public WelcomMessage Welcome = new WelcomMessage();
        public QAMessages()
        {
            InitializeComponent();
            #region OldPage
            buttonMsg.Visibility = Visibility.Visible;
            buttonOK.Visibility = Visibility.Visible;
            hdrMessageAnswer.Visibility = Visibility.Visible;
            hdrMessage.Visibility = Visibility.Visible;
            hdrMessageTypeAnswer.Visibility = Visibility.Visible;
            hdrMessageType.Visibility = Visibility.Visible;
            hdrActive.Visibility = Visibility.Visible;
            #endregion

            #region CURRENTpAGE
            hdrMessagelbl.Visibility = Visibility.Collapsed;
            EDMessage.Text = string.Empty;
            tbMessage.Visibility = Visibility.Collapsed;
            EDMessage.Visibility = Visibility.Collapsed;
            chkActive.IsChecked = false;
            chkActive.Visibility = Visibility.Collapsed;
            buttonSave.Visibility = Visibility.Collapsed;
            #endregion


            #region CurrentPage
          
            xdgMessages.Visibility = Visibility.Collapsed;
            buttonSaveActive.Visibility = Visibility.Collapsed;
            hdrWelcome.Visibility = Visibility.Collapsed;
           
            #endregion


            DataSet dsMessages = Methods.ExecuteStoredProcedure("sp_GetActiveWelcomeRecord", null);
            DataTable dtMessages = dsMessages.Tables[0];

            if (dtMessages.Rows.Count > 0)
            {
                var row = dtMessages.Rows[0];
                hdrMessageTypeAnswer.Text = "Welcome Message";
                hdrMessageAnswer.Text = row["ActiveMessage"].ToString();
            }
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _dsLookups = Methods.ExecuteStoredProcedure("sp_QAMessageLookups", null);
                xdgMessages.ItemsSource = _dsLookups.Tables[0].DefaultView;

               

                #region CURRENTpAGE
                hdrMessagelbl.Visibility = Visibility.Collapsed;
                EDMessage.Text = string.Empty;
                tbMessage.Visibility = Visibility.Collapsed;
                EDMessage.Visibility = Visibility.Collapsed;
                chkActive.IsChecked = false;
                chkActive.Visibility = Visibility.Collapsed;
                buttonSave.Visibility = Visibility.Collapsed;
                #endregion

                #region OldPage
                buttonMsg.Visibility = Visibility.Collapsed;
                buttonOK.Visibility = Visibility.Collapsed;
                hdrMessageAnswer.Visibility = Visibility.Collapsed;
                hdrMessage.Visibility = Visibility.Collapsed;
                hdrMessageTypeAnswer.Visibility = Visibility.Collapsed;
                hdrMessageType.Visibility = Visibility.Collapsed;
                hdrActive.Visibility = Visibility.Collapsed;
                #endregion
                #region CurrentPage
              
                xdgMessages.Visibility = Visibility.Visible;
                buttonSaveActive.Visibility = Visibility.Visible;
                hdrWelcome.Visibility = Visibility.Visible;
              buttonCancel.Visibility = Visibility.Visible;
                #endregion
            }
            catch 
            {
                ShowMessageBox(new INMessageBoxWindow1(), "An error occurred: Messages could not be loaded at this time!\n", "Error", ShowMessageType.Error);
            }
        }

        //private async void buttonPDF_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        bool message = false;
        //        var openFileDialog = new Microsoft.Win32.OpenFileDialog
        //        {
        //            Filter = "PDF Files (*.pdf)|*.pdf",
        //            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        //        };

        //        if (openFileDialog.ShowDialog() == true)
        //        {
        //            uploadProgressBar.Visibility = Visibility.Visible;
        //            uploadProgressBar.Value = 0;
        //            int AddedID = 0;
        //            string Text = "";
        //            long User = ((User)GlobalSettings.ApplicationUser).ID;

                 

        //            byte[] fileBytes = await Task.Run(() => File.ReadAllBytes(openFileDialog.FileName));
        //            //string hexString = "0x" + BitConverter.ToString(fileBytes).Replace("-", string.Empty);
        //            string base64String = Convert.ToBase64String(fileBytes);
        //            SqlParameter[] parameters =
        //               {
        //             new SqlParameter("@TextMessage", base64String),
        //             new SqlParameter("@StampDate", DateTime.Now),
        //             new SqlParameter("@StampUserID", User),
        //             new SqlParameter("@IsActive", false),
        //             new SqlParameter ("@PDFName", openFileDialog.SafeFileName)
        //            };
        //            DataSet dsMessages = Methods.ExecuteStoredProcedure("sp_AddWelcomeMessage", parameters);
        //            DataTable dtMessages = dsMessages.Tables[0];

        //            if (dtMessages.Rows.Count > 0)
        //            {
        //                foreach (DataRow row in dtMessages.Rows)
        //                {
        //                    message = Convert.ToBoolean(row["Success"].ToString());
        //                    AddedID = Convert.ToInt32(row["ID"].ToString());
        //                    Text = row["PDFName"].ToString();
        //                }
        //            }
              
        //                    for (int i = 0; i <= 100; i++)
        //            {
        //                uploadProgressBar.Value = i;
        //                // Simulate some processing time
        //                await Task.Delay(50);
        //            }

        //            uploadProgressBar.Visibility = Visibility.Hidden;
        //            if (message)
        //            {
        //                MessageBoxResult result = MessageBox.Show("Upload complete! Do you want to proceed with making this the active message?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

        //                if (result == MessageBoxResult.Yes)
        //                {

                          
        //                        try
        //                        {
        //                            SqlParameter[] parameter =
        //                         {
        //                          new SqlParameter("@WelcomeMessageID", AddedID),
        //                        };

        //                            try
        //                            {
        //                                Methods.ExecuteStoredProcedure("sp_SetInactiveAndActivateSpecific", parameter);
        //                                hdrMessageTypeAnswer.Text = "PDF Testimonial";
        //                                hdrMessageAnswer.Text = Text;
        //                                MessageBox.Show("PDF Testimonial now active.");
        //                            }
        //                            catch
        //                            {
        //                                MessageBox.Show("PDF Testimonial could not be made active.");
        //                            }
        //                        }
        //                        catch
        //                        {

        //                        }
        //                    }
        //            }
        //            else
        //            {
        //                MessageBox.Show($"An error occurred: PDf could not be uploaded!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        uploadProgressBar.Visibility = Visibility.Hidden;
        //    }
        //}

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(EDMessage.Text))
                {
                    bool message = false;
                    int AddedID = 0;
                    string Text = "";
                    var IsActive = chkActive.IsChecked;
                    var Message = EDMessage.Text;
                    long User = ((User)GlobalSettings.ApplicationUser).ID;

                  
                    Welcome.TextMessage = EDMessage.Text;
                    Welcome.IsActive = false;
                    Welcome.StampDate = DateTime.Now;
                    Welcome.PDFName = "Not a Pdf";
                    Welcome.StampUserID = User;
                    SqlParameter[] parameters =
                      {
                     new SqlParameter("@TextMessage", Message),
                     new SqlParameter("@StampDate", DateTime.Now),
                     new SqlParameter("@StampUserID", User),
                     new SqlParameter("@IsActive", IsActive),
                     new SqlParameter("@PDFName", "NO")
                    };
                    bool saveResult = WelcomeMessageMapper.Save(Welcome);
                    if (saveResult)
                    {                  
                                    hdrMessageTypeAnswer.Text = "Welcome Message";
                                    hdrMessageAnswer.Text = Text;
                                  
                        ShowMessageBox(new INMessageBoxWindow1(), "Welcome message, Has been saved.\n", "Success", ShowMessageType.Information);

                        #region CURRENTpAGE
                        hdrMessagelbl.Visibility = Visibility.Collapsed;
                                    EDMessage.Text = string.Empty;
                                    tbMessage.Visibility = Visibility.Collapsed;
                                    EDMessage.Visibility = Visibility.Collapsed;
                                    chkActive.IsChecked = false;
                                    chkActive.Visibility = Visibility.Collapsed;
                                    buttonSave.Visibility = Visibility.Collapsed;
                                    #endregion

                                    #region OldPage
                                    buttonMsg.Visibility = Visibility.Visible;
                                 //   buttonPDF.Visibility = Visibility.Visible;
                                    buttonOK.Visibility = Visibility.Visible;
                                    hdrMessageAnswer.Visibility = Visibility.Visible;
                                    hdrMessage.Visibility = Visibility.Visible;
                                    hdrMessageTypeAnswer.Visibility = Visibility.Visible;
                                    hdrMessageType.Visibility = Visibility.Visible;
                                    hdrActive.Visibility = Visibility.Visible;
                                    #endregion
                               
                       
                    }
                    else
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "Save Failed, Message could not be saved.\n", "Validation", ShowMessageType.Error);
                    }
                }
                else
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Missing Data, Please make sure required fields are filled out message is missing.\n", "Validation", ShowMessageType.Information);
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Something went wrong.\n", "Error", ShowMessageType.Error);
            }
        }

        private void buttonSaveActive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (xdgMessages.SelectedIndex == -1)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select a message to make active.\n", "Error", ShowMessageType.Error);
                }
                else
                {
                   if(xdgMessages.SelectedIndex != -1)
                    {
                        DataRowView selectedRow = (DataRowView)xdgMessages.SelectedItem;
                        int selectedId = Convert.ToInt32(selectedRow["ID"]);
                        try
                        {
                            Welcome = new WelcomMessage(selectedId);
                            Welcome.TextMessage = selectedRow["Name"].ToString();
                            Welcome.ID = selectedId;
                            Welcome.IsActive = true;
                            Welcome.PDFName = "Not a Pdf";
                            WelcomeMessageMapper.UpdateIsActive(Welcome, selectedId, true);
                            MessageBox.Show($"Welcome message has been made active.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            string message = selectedRow["Name"].ToString();
                            hdrMessageTypeAnswer.Text = "Welcome Message";
                            hdrMessageAnswer.Text = message;
                            #region CurrentPage
                           
                            xdgMessages.Visibility = Visibility.Collapsed;
                            buttonSaveActive.Visibility = Visibility.Collapsed;
                            hdrWelcome.Visibility = Visibility.Collapsed;
                           buttonCancel.Visibility = Visibility.Collapsed;
                            #endregion

                            #region OldPage
                            buttonMsg.Visibility = Visibility.Visible;
                           
                            buttonOK.Visibility = Visibility.Visible;
                            hdrMessageAnswer.Visibility = Visibility.Visible;
                            hdrMessage.Visibility = Visibility.Visible;
                            hdrMessageTypeAnswer.Visibility = Visibility.Visible;
                            hdrMessageType.Visibility = Visibility.Visible;
                            hdrActive.Visibility = Visibility.Visible;
                            #endregion
                        }catch (Exception ex)
                        {

                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
            private void btnClose_Click(object sender, RoutedEventArgs e)
            {
        
                StartScreen startScreen = new StartScreen();
                OnClose(startScreen);
        
            }
        private void buttonMsg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region CURRENTpAGE
                hdrMessagelbl.Visibility = Visibility.Visible;
                EDMessage.Text = string.Empty;
                tbMessage.Visibility = Visibility.Visible;
                EDMessage.Visibility = Visibility.Visible;
                chkActive.IsChecked = false;
                chkActive.Visibility = Visibility.Visible;
                buttonSave.Visibility = Visibility.Visible;
                buttonCancel.Visibility = Visibility.Visible;   
                #endregion

                #region OldPage
                buttonMsg.Visibility = Visibility.Collapsed;
              
                buttonOK.Visibility = Visibility.Collapsed;
                hdrMessageAnswer.Visibility = Visibility.Collapsed;
                hdrMessage.Visibility = Visibility.Collapsed;
                hdrMessageTypeAnswer.Visibility = Visibility.Collapsed;
                hdrMessageType.Visibility = Visibility.Collapsed;
                hdrActive.Visibility = Visibility.Collapsed;
                #endregion
            }
            catch (Exception ex)
            {

            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            #region OldPage
            buttonMsg.Visibility = Visibility.Visible;
            buttonOK.Visibility = Visibility.Visible;
            hdrMessageAnswer.Visibility = Visibility.Visible;
            hdrMessage.Visibility = Visibility.Visible;
            hdrMessageTypeAnswer.Visibility = Visibility.Visible;
            hdrMessageType.Visibility = Visibility.Visible;
            hdrActive.Visibility = Visibility.Visible;
            #endregion

            #region CURRENTpAGE
            hdrMessagelbl.Visibility = Visibility.Collapsed;
            EDMessage.Text = string.Empty;
            tbMessage.Visibility = Visibility.Collapsed;
            EDMessage.Visibility = Visibility.Collapsed;
            chkActive.IsChecked = false;
            chkActive.Visibility = Visibility.Collapsed;
            buttonSave.Visibility = Visibility.Collapsed;
            #endregion
            buttonCancel.Visibility = Visibility.Collapsed;

            #region CurrentPage

            xdgMessages.Visibility = Visibility.Collapsed;
            buttonSaveActive.Visibility = Visibility.Collapsed;
            hdrWelcome.Visibility = Visibility.Collapsed;

            #endregion
        }
    }
}

