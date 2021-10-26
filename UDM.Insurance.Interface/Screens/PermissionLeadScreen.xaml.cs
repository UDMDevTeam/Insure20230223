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
        bool CellNumBool = false;
        bool AddNumBool = false;

        string CellnumberKeypad = "";
        string AddnumberKeypad = "";

        #region Variables
        private const string IDField = "ID";
        private const string DescriptionField = "Description";
        long? importID;

        #endregion

        #region Constructor

        public PermissionLeadScreen(long? ImportID, string title, string firstname, string surname, string cellnumber, string altnumber)
        {
            InitializeComponent();
            LoadTitleCB();
            CheckDateSavedVisibility();
            importID = ImportID;
            LoadLead();

        }

        private void CheckDateSavedVisibility()
        {
            try
            {
                if (GlobalSettings.ApplicationUser.ID == 72 || GlobalSettings.ApplicationUser.ID == 174)
                {
                    DateSavedDP.Visibility = Visibility.Visible;
                    lblDatePicker.Visibility = Visibility.Visible;


                }
                else
                {
                    DateSavedDP.Visibility = Visibility.Collapsed;
                    lblDatePicker.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {

            }

        }

        private void LoadLead()
        {
            ClearScreen();


            try
            {
                string strQuery;
                DataTable dt;

                strQuery = "SELECT TOP 1 Title, Firstname, Surname, CellNumber, AltNumber, DateSaved, DateOfBirth, Occupation FROM INPermissionLead WHERE FKImportID = " + importID;
                DataTable dtPermissionLeadDetails = Methods.GetTableData(strQuery);

                cmbTitle.Text = dtPermissionLeadDetails.Rows[0]["Title"] as string;
                medFirstName.Text = dtPermissionLeadDetails.Rows[0]["Firstname"] as string;
                medAltPhone.Text = dtPermissionLeadDetails.Rows[0]["AltNumber"] as string;
                medCellPhone.Text = dtPermissionLeadDetails.Rows[0]["CellNumber"] as string;
                medSurname.Text = dtPermissionLeadDetails.Rows[0]["Surname"] as string;
                try { DateSavedDP.SelectedDate = dtPermissionLeadDetails.Rows[0]["DateSaved"] as DateTime?; } catch { }
                try { DateOfBirthDP.SelectedDate = dtPermissionLeadDetails.Rows[0]["DateOfBirth"] as DateTime?; } catch { }
                medOccupation.Text = dtPermissionLeadDetails.Rows[0]["Occupation"] as string;

            }
            catch
            {

            }

        }
        #endregion Constructor

        private void ClearScreen()
        {
            try { cmbTitle.SelectedIndex = -1; } catch { }
            try { medFirstName.Text = null; } catch { }
            try { medSurname.Text = null; } catch { }
            try { medCellPhone.Text = null; } catch { }
            try { medAltPhone.Text = null; } catch { }
            try { DateSavedDP.SelectedDate = null; } catch { }
            try { DateOfBirthDP.SelectedDate = null; } catch { }
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
                if (medCellPhone.Text == null || medCellPhone.Text == "" || DateOfBirthDP.SelectedDate == null)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Missing fields Required.\n", "Not saved.", ShowMessageType.Information);
                }
                else
                {
                    INNextOfKin nok = new INNextOfKin();
                    long? ID = null;
                    DataTable dtIsloaded;
                    long IDReal;
                    try
                    {
                        nok.FKINImportID = importID;
                        nok.FirstName = medFirstName.Text;
                        nok.Surname = medSurname.Text;
                        nok.FKINRelationshipID = long.Parse(getNOKRelationship.Text);
                        nok.TelContact = medCellPhone.Text;
                        nok.Save(_validationResult);

                        string strQueryIsLoaded;
                        strQueryIsLoaded = "SELECT top 1 ID FROM INPermissionLead WHERE FKImportID = " + importID;
                        dtIsloaded = Methods.GetTableData(strQueryIsLoaded);
                        ID = dtIsloaded.Rows[0]["ID"] as long?;
                        IDReal = long.Parse(ID.ToString());
                    }
                    catch
                    {
                        IDReal = 0;
                        dtIsloaded = null;
                    }


                    string strQuerySavedBy;
                    strQuerySavedBy = "SELECT top 1 SavedBy FROM INPermissionLead WHERE FKImportID = " + importID;
                    DataTable dtPermissionSavedByIsloaded = Methods.GetTableData(strQuerySavedBy);

                    string strQueryDateSaved;
                    strQueryDateSaved = "SELECT top 1 SavedBy FROM INPermissionLead WHERE FKImportID = " + importID;
                    DataTable dtPermissiondateSavedIsloaded = Methods.GetTableData(strQueryDateSaved);

                    if(dtIsloaded == null)
                    {
                        INPermissionLead inpermissionlead = new INPermissionLead();
                        try { inpermissionlead.FKINImportID = importID; } catch { }
                        try { inpermissionlead.Title = cmbTitle.Text.ToString(); } catch { }
                        try { inpermissionlead.Firstname = medFirstName.Text.ToString(); } catch { }
                        try { inpermissionlead.Surname = medSurname.Text.ToString(); } catch {  }
                        try { inpermissionlead.Cellnumber = medCellPhone.Text.ToString(); } catch { }
                        try { inpermissionlead.AltNumber = medAltPhone.Text.ToString(); } catch {  }
                        try { inpermissionlead.DateOfBirth = DateOfBirthDP.SelectedDate; } catch {  }

                        if (dtPermissionSavedByIsloaded.Rows.Count == 0)
                        {
                            try { inpermissionlead.SavedBy = GlobalSettings.ApplicationUser.ID.ToString(); } catch { }
                        }
                        if (GlobalSettings.ApplicationUser.ID == 72 || GlobalSettings.ApplicationUser.ID == 174) //this is for Kashmira to edit the Date the reference was saved for report purposes.
                        {
                            try { inpermissionlead.DateSaved = DateSavedDP.SelectedDate; } catch { }

                        }
                        else
                        {
                            if (dtPermissiondateSavedIsloaded.Rows.Count == 0)
                            {
                                try { inpermissionlead.DateSaved = DateTime.Now; } catch { }
                            }
                        }
                        try { inpermissionlead.Occupation = medOccupation.Text.ToString(); } catch {  }
                        inpermissionlead.Save(_validationResult);

                        ShowMessageBox(new INMessageBoxWindow1(), "Lead Permission successfully saved.\n", "Save Result", ShowMessageType.Information);
                    }
                    else
                    {
                        INPermissionLead inpermissionlead = new INPermissionLead(IDReal);
                        try { inpermissionlead.FKINImportID = importID; } catch { }
                        try { inpermissionlead.Title = cmbTitle.Text.ToString(); } catch { }
                        try { inpermissionlead.Firstname = medFirstName.Text.ToString(); } catch { }
                        try { inpermissionlead.Surname = medSurname.Text.ToString(); } catch { }
                        try { inpermissionlead.Cellnumber = medCellPhone.Text.ToString(); } catch {}
                        try { inpermissionlead.AltNumber = medAltPhone.Text.ToString(); } catch { }
                        try { inpermissionlead.DateOfBirth = DateOfBirthDP.SelectedDate; } catch {}

                        if (dtPermissionSavedByIsloaded.Rows.Count == 0)
                        {
                            try { inpermissionlead.SavedBy = GlobalSettings.ApplicationUser.ID.ToString(); } catch {}
                        }
                        if (GlobalSettings.ApplicationUser.ID == 72 || GlobalSettings.ApplicationUser.ID == 174) //this is for Kashmira to edit the Date the reference was saved for report purposes.
                        {
                            try { inpermissionlead.DateSaved = DateSavedDP.SelectedDate; } catch { }

                        }
                        else
                        {
                            if (dtPermissiondateSavedIsloaded.Rows.Count == 0)
                            {
                                try { inpermissionlead.DateSaved = DateTime.Now; } catch {  }
                            }
                        }
                        try { inpermissionlead.Occupation = medOccupation.Text.ToString(); } catch {  }
                        inpermissionlead.Save(_validationResult);

                        ShowMessageBox(new INMessageBoxWindow1(), "Lead Permission successfully saved.\n", "Save Result", ShowMessageType.Information);
                        }
                }
            }
            catch (Exception a)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Lead Permission saved unsuccessfully.\n", "Not saved Result", ShowMessageType.Information);
            }

        }

        #region Keypad buttons Methods
        private void dteDateOfBirth_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void dteDateOfBirth_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void dteDateOfBirth_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

        }

        private void Cancelbtn_Click(object sender, RoutedEventArgs e)
        {
            if (CellNumBool == true)
            {
                CellnumberKeypad = "";
                medCellPhone.Text = CellnumberKeypad;
            }
            if (AddNumBool == true)
            {
                AddnumberKeypad = "";
                medAltPhone.Text = AddnumberKeypad;
            }
        }

        private void Enterbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CellNumBool == true)
                {
                    CellnumberKeypad = CellnumberKeypad.Remove((CellnumberKeypad.Count() - 1), 1);
                    medCellPhone.Text = CellnumberKeypad;
                }
                if (AddNumBool == true)
                {
                    AddnumberKeypad = AddnumberKeypad.Remove((AddnumberKeypad.Count() - 1), 1);
                    medAltPhone.Text = AddnumberKeypad;
                }
            }
            catch
            {

            }
        }

        private void Zerobtn_Click(object sender, RoutedEventArgs e)
        {
            if (CellNumBool == true)
            {
                CellnumberKeypad = CellnumberKeypad + '0';
                medCellPhone.Text = CellnumberKeypad;
            }
            if (AddNumBool == true)
            {
                AddnumberKeypad = AddnumberKeypad + '0';
                medAltPhone.Text = AddnumberKeypad;
            }
        }

        private void Ninebtn_Click(object sender, RoutedEventArgs e)
        {
            if (CellNumBool == true)
            {
                CellnumberKeypad = CellnumberKeypad + '9';
                medCellPhone.Text = CellnumberKeypad;
            }
            if (AddNumBool == true)
            {
                AddnumberKeypad = AddnumberKeypad + '9';
                medAltPhone.Text = AddnumberKeypad;
            }
        }

        private void Eightbtn_Click(object sender, RoutedEventArgs e)
        {
            if (CellNumBool == true)
            {
                CellnumberKeypad = CellnumberKeypad + '8';
                medCellPhone.Text = CellnumberKeypad;
            }
            if (AddNumBool == true)
            {
                AddnumberKeypad = AddnumberKeypad + '8';
                medAltPhone.Text = AddnumberKeypad;
            }
        }

        private void Sevenbtn_Click(object sender, RoutedEventArgs e)
        {
            if (CellNumBool == true)
            {
                CellnumberKeypad = CellnumberKeypad + '7';
                medCellPhone.Text = CellnumberKeypad;
            }
            if (AddNumBool == true)
            {
                AddnumberKeypad = AddnumberKeypad + '7';
                medAltPhone.Text = AddnumberKeypad;
            }
        }

        private void Sixbtn_Click(object sender, RoutedEventArgs e)
        {
            if (CellNumBool == true)
            {
                CellnumberKeypad = CellnumberKeypad + '6';
                medCellPhone.Text = CellnumberKeypad;
            }
            if (AddNumBool == true)
            {
                AddnumberKeypad = AddnumberKeypad + '6';
                medAltPhone.Text = AddnumberKeypad;
            }
        }

        private void Fivebtn_Click(object sender, RoutedEventArgs e)
        {
            if (CellNumBool == true)
            {
                CellnumberKeypad = CellnumberKeypad + '5';
                medCellPhone.Text = CellnumberKeypad;
            }
            if (AddNumBool == true)
            {
                AddnumberKeypad = AddnumberKeypad + '5';
                medAltPhone.Text = AddnumberKeypad;
            }
        }

        private void Fourbtn_Click(object sender, RoutedEventArgs e)
        {
            if (CellNumBool == true)
            {
                CellnumberKeypad = CellnumberKeypad + '4';
                medCellPhone.Text = CellnumberKeypad;
            }
            if (AddNumBool == true)
            {
                AddnumberKeypad = AddnumberKeypad + '4';
                medAltPhone.Text = AddnumberKeypad;
            }
        }

        private void Threebtn_Click(object sender, RoutedEventArgs e)
        {
            if (CellNumBool == true)
            {
                CellnumberKeypad = CellnumberKeypad + '3';
                medCellPhone.Text = CellnumberKeypad;
            }
            if (AddNumBool == true)
            {
                AddnumberKeypad = AddnumberKeypad + '3';
                medAltPhone.Text = AddnumberKeypad;
            }
        }

        private void Twobtn_Click(object sender, RoutedEventArgs e)
        {
            if (CellNumBool == true)
            {
                CellnumberKeypad = CellnumberKeypad + '2';
                medCellPhone.Text = CellnumberKeypad;
            }
            if (AddNumBool == true)
            {
                AddnumberKeypad = AddnumberKeypad + '2';
                medAltPhone.Text = AddnumberKeypad;
            }
        }

        private void Onebtn_Click(object sender, RoutedEventArgs e)
        {
            if (CellNumBool == true)
            {
                CellnumberKeypad = CellnumberKeypad + '1';
                medCellPhone.Text = CellnumberKeypad;
            }
            if (AddNumBool == true)
            {
                AddnumberKeypad = AddnumberKeypad + '1';
                medAltPhone.Text = AddnumberKeypad;
            }
        }
        #endregion

        private void medCellPhone_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OpenKeypad();
            CellNumBool = true;
            AddNumBool = false;
        }

        private void medAltPhone_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            OpenKeypad();
            CellNumBool = false;
            AddNumBool = true;
        }

        private void OpenKeypad()
        {
            KeypadBorder.Visibility = Visibility.Visible;
            Onebtn.Visibility = Visibility.Visible;
            Twobtn.Visibility = Visibility.Visible;
            Threebtn.Visibility = Visibility.Visible;
            Fourbtn.Visibility = Visibility.Visible;
            Fivebtn.Visibility = Visibility.Visible;
            Sixbtn.Visibility = Visibility.Visible;
            Sevenbtn.Visibility = Visibility.Visible;
            Eightbtn.Visibility = Visibility.Visible;
            Ninebtn.Visibility = Visibility.Visible;
            Zerobtn.Visibility = Visibility.Visible;
            Cancelbtn.Visibility = Visibility.Visible;
            Enterbtn.Visibility = Visibility.Visible;
            CloseKeypadbtn.Visibility = Visibility.Visible;
        }

        private void CloseKeypadbtn_Click(object sender, RoutedEventArgs e)
        {
            KeypadBorder.Visibility = Visibility.Collapsed;
            Onebtn.Visibility = Visibility.Collapsed;
            Twobtn.Visibility = Visibility.Collapsed;
            Threebtn.Visibility = Visibility.Collapsed;
            Fourbtn.Visibility = Visibility.Collapsed;
            Fivebtn.Visibility = Visibility.Collapsed;
            Sixbtn.Visibility = Visibility.Collapsed;
            Sevenbtn.Visibility = Visibility.Collapsed;
            Eightbtn.Visibility = Visibility.Collapsed;
            Ninebtn.Visibility = Visibility.Collapsed;
            Zerobtn.Visibility = Visibility.Collapsed;
            Cancelbtn.Visibility = Visibility.Collapsed;
            Enterbtn.Visibility = Visibility.Collapsed;
            CloseKeypadbtn.Visibility = Visibility.Collapsed;
        }
    }

}