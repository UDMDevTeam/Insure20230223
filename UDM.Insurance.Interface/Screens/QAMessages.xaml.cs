using Embriant.Framework;
using Embriant.Framework.Configuration;
using Embriant.WPF.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Shapes;
using UDM.Insurance.Business;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for QAMessages.xaml
    /// </summary>
    public partial class QAMessages
    {
        public QAMessages()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void buttonPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool message = false;
                var openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    uploadProgressBar.Visibility = Visibility.Visible;
                    uploadProgressBar.Value = 0;


                    long User = ((User)GlobalSettings.ApplicationUser).ID;

                    #region Get Message

                    byte[] fileBytes = await Task.Run(() => File.ReadAllBytes(openFileDialog.FileName));

                    SqlParameter[] parameters =
                    {
                     new SqlParameter("@PDFDocument", fileBytes),
                     new SqlParameter("@StampDate", DateTime.Now),
                     new SqlParameter("@StampUserID", User),
                     new SqlParameter("@IsActive", false)
                    };
                    DataSet dsMessages = Methods.ExecuteStoredProcedure("sp_AddPDFTestimonial", parameters);
                    DataTable dtMessages = dsMessages.Tables[0];

                    if (dtMessages.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtMessages.Rows)
                        {
                            message = Convert.ToBoolean(row["Success"].ToString()); 
                           
                        }
                    }
                    else
                    {
                        MessageBox.Show($"An error occurred: PDf could not be saved!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        uploadProgressBar.Visibility = Visibility.Hidden;
                    }

                    for (int i = 0; i <= 100; i++)
                    {
                        uploadProgressBar.Value = i;
                        // Simulate some processing time
                        await Task.Delay(50);
                    }

                    uploadProgressBar.Visibility = Visibility.Hidden;
                    if (message)
                    {
                        MessageBoxResult result = MessageBox.Show("Upload complete! Do you want to proceed with making this the active message?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            
                            DataSet dsMessage = Methods.ExecuteStoredProcedure("sp_GetLastPDFTestimonialID", null);
                            DataTable dtMessage = dsMessages.Tables[0];
                            if (dtMessage.Rows.Count > 0)
                            {
                                DataRow row = dtMessage.Rows[0];
                                int lastId = Convert.ToInt32(row["LastID"]); 
                                string pdfName = row["PDFName"].ToString();
                                try
                                {
                                    SqlParameter[] parameter =
                                    {
                                      new SqlParameter("@PDFTestimonialID", lastId),
                                    };
                                    try
                                    {
                                        Methods.ExecuteStoredProcedure("sp_SetInactiveAndActivateSpecific", parameter);
                                        hdrMessageTypeAnswer.Text = "PDF Testimonial";
                                        hdrMessageAnswer.Text = pdfName;

                                        MessageBox.Show("PDF Testimonial now active.");
                                    }
                                    catch
                                    {
                                        MessageBox.Show("PDF Testimonial could not be made active.");
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"An error occurred: PDf could not be uploaded!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                uploadProgressBar.Visibility = Visibility.Hidden;
            }
        }
    }
}
