using Embriant.Framework;
using System;
using System.Windows;
using UDM.Insurance.Interface.Windows;
using System.Data;
using UDM.Insurance.Business;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for QAAssesmentAddEditQuestions.xaml
    /// </summary>
    public partial class QAAssesmentAddEditQuestions 
    {
        DataSet dsQALookups = new DataSet();
        DataTable dtQAQuestions = new DataTable();
        DataTable dtQAType = new DataTable();
        DataTable dtQASubCat = new DataTable();
        private const string IDField = "ID";
        private const string DescriptionField = "Description";
        private const string CatField = "SubCategory";
        public QAAssesmentAddEditQuestions()
        {
            InitializeComponent();
            dsQALookups = Insure.QAAssessmentQuestionsLookups();
            dtQAQuestions = dsQALookups.Tables[0];
            dtQAType = dsQALookups.Tables[1];
            dtQASubCat = dsQALookups.Tables[2];
            cmbTypes.Populate(dtQAType, DescriptionField, IDField);
            cmbCat.Populate(dtQASubCat, CatField, IDField);
            dataGrid.ItemsSource = dtQAQuestions.DefaultView;
        }

        private void btnAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddAndEditVisibility("Add");
            }
            catch(Exception ex)
            {
                
            }
        }

        private void btnEditQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                AddAndEditVisibility("Edit");
            }
            else
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select an Item before continuing to edit.", "Validation", ShowMessageType.Error);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(edQuestionInput.Text) && cmbCat.SelectedIndex != -1 && cmbTypes.SelectedIndex !=-1)
            {
                // Code for saving will go above these two things
                ShowMessageBox(new INMessageBoxWindow1(), "This item has been succesfully saved!", "Success", ShowMessageType.Information);
                ClearFields();
                AddAndEditVisibility("GoBack");
            }
            else
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please make sure to complete the entire form", "Validation", ShowMessageType.Error);
            }
        }

        private void btnAddAnother_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearFields();
            }catch(Exception ex)
            {

            }
        }

        #region Visibility and clearing methods

        private void ClearFields()
        {
            try
            {
                cmbTypes.SelectedIndex = -1;
                cmbCat.SelectedIndex = -1;
                edQuestionInput.Text = string.Empty;
                dataGrid.SelectedItem = null;
            }catch (Exception ex)
            {

            }
        }

        private void AddAndEditVisibility(string Type)
        {
            try
            {
                dataGrid.Visibility = Visibility.Collapsed;
                btnAddQuestion.Visibility = Visibility.Collapsed;
                btnEditQuestion.Visibility = Visibility.Collapsed;
                if (Type.Contains("Add"))
                {
                    hdrQuestion.Visibility = Visibility.Visible;
                    edQuestionInput.Visibility = Visibility.Visible;
                    hdrQuestionType.Visibility = Visibility.Visible;
                    cmbTypes.Visibility = Visibility.Visible;
                    hdrCategory.Visibility = Visibility.Visible;
                    cmbCat.Visibility = Visibility.Visible;
                    btnSave.Visibility = Visibility.Visible;
                    btnAddAnother.Visibility = Visibility.Visible;
                }
                else if (Type.Contains("Edit"))
                {
                    hdrQuestion.Visibility = Visibility.Visible;
                    edQuestionInput.Visibility = Visibility.Visible;
                    hdrQuestionType.Visibility = Visibility.Visible;
                    cmbTypes.Visibility = Visibility.Visible;
                    hdrCategory.Visibility = Visibility.Visible;
                    cmbCat.Visibility = Visibility.Visible;
                    btnSave.Visibility = Visibility.Visible;
                }
                else
                {
                    hdrQuestion.Visibility = Visibility.Collapsed;
                    edQuestionInput.Visibility = Visibility.Collapsed;
                    hdrQuestionType.Visibility = Visibility.Collapsed;
                    cmbTypes.Visibility = Visibility.Collapsed;
                    hdrCategory.Visibility = Visibility.Collapsed;
                    cmbCat.Visibility = Visibility.Collapsed;
                    btnSave.Visibility = Visibility.Collapsed;
                    btnAddAnother.Visibility = Visibility.Collapsed;

                    dataGrid.Visibility = Visibility.Visible;
                    btnAddQuestion.Visibility = Visibility.Visible;
                    btnEditQuestion.Visibility = Visibility.Visible;
                }
            }catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
