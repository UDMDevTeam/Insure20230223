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

namespace UDM.Insurance.Interface.Screens
{

    public partial class PermissionLeadScreen
    {

        #region Variables
        private const string IDField = "ID";
        private const string DescriptionField = "Description";
        long? importID;
        bool isloaded;
        string sentTitle;
        string sentFirstname;
        string sentSurname;
        string sentCellnumber;
        string sentAltNumber;
        #endregion

        #region Constructor

        public PermissionLeadScreen(long? ImportID, string title, string firstname, string surname, string cellnumber, string altnumber)
        {
            InitializeComponent();
            LoadTitleCB();
            importID = ImportID;


            LoadLead();

        }

        private void LoadLead()
        {
            ClearScreen();


            try
            {
                string strQuery;
                DataTable dt;

                strQuery = "SELECT TOP 1 Title, Firstname, Surname, CellNumber, AltNumber FROM INPermissionLead WHERE FKImportID = " + importID;
                DataTable dtPermissionLeadDetails = Methods.GetTableData(strQuery);

                cmbTitle.Text = dtPermissionLeadDetails.Rows[0]["Title"] as string;
                medFirstName.Text = dtPermissionLeadDetails.Rows[0]["Firstname"] as string;
                medAltPhone.Text = dtPermissionLeadDetails.Rows[0]["AltNumber"] as string;
                medCellPhone.Text = dtPermissionLeadDetails.Rows[0]["CellNumber"] as string;
                medSurname.Text = dtPermissionLeadDetails.Rows[0]["Surname"] as string;
            }
            catch
            {

            }

        }
        #endregion Constructor

        private void ClearScreen()
        {
            cmbTitle.SelectedIndex = -1;
            medFirstName.Text = null;
            medSurname.Text = null;
            medCellPhone.Text = null;
            medAltPhone.Text = null;
        }

        #region Event Handlers

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        #endregion Event Handlers

        private void LoadTitleCB()
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@UserTypeID", 1);
            DataSet dsLookups = Methods.ExecuteStoredProcedure("spINGetLeadApplicationScreenLookups", parameters);

            DataTable dtTitles = dsLookups.Tables[2];
            cmbTitle.Populate(dtTitles, DescriptionField, IDField);

        }

        private void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void cmbTitle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (medCellPhone.Text == null || medCellPhone.Text == "")
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Missing fields Required.\n", "Not saved.", ShowMessageType.Information);
                }
                else
                {
                    string strQuerySavedBy;
                    strQuerySavedBy = "SELECT top 1 SavedBy FROM INPermissionLead WHERE FKImportID = " + importID;

                    DataTable dtPermissionSavedByIsloaded = Methods.GetTableData(strQuerySavedBy);

                    string strQueryDateSaved;
                    strQueryDateSaved = "SELECT top 1SavedBy FROM INPermissionLead WHERE FKImportID = " + importID;

                    DataTable dtPermissiondateSavedIsloaded = Methods.GetTableData(strQueryDateSaved);

                    INPermissionLead inpermissionlead = new INPermissionLead();
                    inpermissionlead.FKINImportID = importID;
                    try { inpermissionlead.Title = cmbTitle.Text.ToString(); } catch { inpermissionlead.Title = " "; }
                    try { inpermissionlead.Firstname = medFirstName.Text.ToString(); } catch { inpermissionlead.Firstname = " "; }
                    try { inpermissionlead.Surname = medSurname.Text.ToString(); } catch { inpermissionlead.Surname = " "; }
                    try { inpermissionlead.Cellnumber = medCellPhone.Text.ToString(); } catch { inpermissionlead.Cellnumber = " "; }
                    try { inpermissionlead.AltNumber = medAltPhone.Text.ToString(); } catch { inpermissionlead.AltNumber = " "; }
                    if (dtPermissionSavedByIsloaded.Rows.Count == 0)
                    {
                        try { inpermissionlead.SavedBy = GlobalSettings.ApplicationUser.ID.ToString(); } catch { inpermissionlead.SavedBy = " "; }
                    }
                    if(dtPermissiondateSavedIsloaded.Rows.Count == 0)
                    {
                        try { inpermissionlead.DateSaved = DateTime.Now; } catch { inpermissionlead.DateSaved = null; }
                    }
                    inpermissionlead.Save(_validationResult);

                    ShowMessageBox(new INMessageBoxWindow1(), "Lead Permission successfully saved.\n", "Save Result", ShowMessageType.Information);

                    //LeadApplicationScreen.ChangePermissionLeadChk();
                }
            }
            catch(Exception a)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Lead Permission saved unsuccessfully.\n", "Not saved Result", ShowMessageType.Information);
            }

        }
    }

}