using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using Embriant.Framework;
using Embriant.Framework.Data;
using Embriant.Framework.Mapping;
using Infragistics.Windows.Editors;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;


namespace UDM.Insurance.Interface.Screens
{

    /// <summary>
    /// Interaction logic for ImportAddCampaignScreen.xaml
	/// </summary>
	public partial class ImportAddCampaignScreen
    {

        #region Constant

        #endregion



        #region Private Members
        //private INCampaign _campaign;
        #endregion

        

        #region Constructors

        public ImportAddCampaignScreen(string campaignName, string campaignCode)
        {
            InitializeComponent();

            LoadLookupData();

            medCampaignCode.Text = campaignCode;
            medCampaignName.Text = campaignName;
        }

        #endregion



        #region Private Methods

        private void LoadLookupData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataTable dtCampaignType = LookupMapper.ListData(Lookups.lkpINCampaignType.ToString()).Tables[0];
                dtCampaignType.DefaultView.Sort = "Description";
                cmbCampaignType.Populate(dtCampaignType, "Description", "ID");

                DataTable dtCampaignGroup = LookupMapper.ListData(Lookups.lkpINCampaignGroup.ToString()).Tables[0];
                dtCampaignGroup.DefaultView.Sort = "Description";
                cmbCampaignGroup.Populate(dtCampaignGroup, "Description", "ID");
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

        private bool IsValidData()
        {
            try
            {
                if (string.IsNullOrEmpty(medCampaignCode.Text))
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "campaign code required", "Invalid Data", ShowMessageType.Error);
                    medCampaignCode.Focus();
                    return false;
                }
                else
                {
                    var campaign = INCampaignMapper.SearchOne(null, null, null, medCampaignCode.Text, null, null, null, null, null, null, null, null);

                    if (campaign != null)
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "campaign code exists", "Invalid Data", ShowMessageType.Error);
                        medCampaignCode.Focus();

                        return false;
                    }
                }

                if (string.IsNullOrEmpty(medCampaignName.Text))
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "campaign name required", "Invalid Data", ShowMessageType.Error);
                    medCampaignName.Focus();
                    return false;
                }
                else
                {
                    var campaign = INCampaignMapper.SearchOne(null, null, medCampaignName.Text, null, null, null, null, null, null, null, null, null);

                    if (campaign != null)
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "campaign name exists", "Invalid Data", ShowMessageType.Error);
                        medCampaignName.Focus();

                        return false;
                    }
                }

                if (cmbCampaignType.SelectedIndex == -1)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "campaign type required", "Invalid Data", ShowMessageType.Error);
                    cmbCampaignType.Focus();
                    return false;
                }

                if (cmbCampaignGroup.SelectedIndex == -1)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "campaign group required", "Invalid Data", ShowMessageType.Error);
                    cmbCampaignGroup.Focus();
                    return false;
                }

                if (string.IsNullOrEmpty(medConversion2.Text))
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "conversion 2 % required", "Invalid Data", ShowMessageType.Error);
                    medConversion2.Focus();
                    return false;   
                }

                if (string.IsNullOrEmpty(medConversion4.Text))
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "conversion 4 % required", "Invalid Data", ShowMessageType.Error);
                    medConversion4.Focus();
                    return false;
                }

                if (string.IsNullOrEmpty(medConversion6.Text))
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "conversion 6 % required", "Invalid Data", ShowMessageType.Error);
                    medConversion6.Focus();
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }

        private void Save()
        {
            try
            {
                SetCursor(Cursors.Wait);

                Database.BeginTransaction(null, IsolationLevel.Snapshot);

                //campaign data
                ImportProgressScreen._inCampaign = new INCampaign();

                ImportProgressScreen._inCampaign.Code = medCampaignCode.Text;
                ImportProgressScreen._inCampaign.Name = medCampaignName.Text;
                ImportProgressScreen._inCampaign.FKINCampaignGroupID = (long)cmbCampaignGroup.SelectedValue;
                ImportProgressScreen._inCampaign.FKINCampaignTypeID = (long)cmbCampaignType.SelectedValue;                
                ImportProgressScreen._inCampaign.Conversion2 = float.Parse(medConversion2.Text.Replace("%", string.Empty));
                ImportProgressScreen._inCampaign.Conversion4 = float.Parse(medConversion4.Text.Replace("%", string.Empty));
                ImportProgressScreen._inCampaign.Conversion6 = float.Parse(medConversion6.Text.Replace("%", string.Empty));

                // TODO:
                // Add conversion values here, modify sql/procedure.

                ImportProgressScreen._inCampaign.Save(_validationResult);

                if (CommitTransaction(""))
                {
                    btnSave.IsEnabled = false;
                    btnSave.Content = "Done";
                    ShowMessageBox(new INMessageBoxWindow1(), "campaign successfully saved", "Save result", ShowMessageType.Information);
                    OnDialogClose(false);
                }
                else
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "error saving campaign", "Save result", ShowMessageType.Error);
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

        private void xamEditor_Select(object sender)
        {
            switch (sender.GetType().Name)
            {
                case "XamMaskedEditor":
                    var xamMEDControl = (XamMaskedEditor)sender;

                    switch (xamMEDControl.Name)
                    {
                        default:
                            xamMEDControl.SelectAll();
                            break;
                    }
                    break;
            }
        }

        #endregion



        #region Event Handlers

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidData())
            {
                Save();
            }
        }

        private void EmbriantComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Methods.EmbriantComboBoxPreviewKeyDown(sender, e);
        }

        private void xamEditor_GotFocus(object sender, RoutedEventArgs e)
        {
            xamEditor_Select(sender);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(false);
        }

        private void medCampaignCode_Loaded(object sender, RoutedEventArgs e)
        {
            medCampaignCode.Focus();
        }

        #endregion

        


	}
}

