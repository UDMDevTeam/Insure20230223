using Embriant.Framework;
using Infragistics.Windows;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.Editors;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{

    public partial class NoteScreen
    {

        #region Enumerations

        //public enum NoteScreenMode
        //{
        //    New,
        //    ReadOnly
        //}

        #endregion Enumerations

        #region Constants

        private const byte _notesToLoad = 3;

        #endregion Constants

        #region Private Fields

        private bool _isForward;
        private NoteData _noteData = new NoteData();
        //private NoteScreenMode _mode;
        private string _loadedNote;
        private bool _hasNotes = false;

        #endregion Private Fields

        #region Publicly-Exposed Properties
        public NoteData NoteData
        {
            get
            {
                return _noteData;
            }

            set
            {
                _noteData = value;
            }
        }

        public bool HasNotes
        {
            get
            {
                return _hasNotes;
            }

            set
            {
                _hasNotes = value;
            }
        }

        #endregion Publicly-Exposed Properties

        #region Constructor

        public NoteScreen(long? fkINImportID /*, NoteScreenMode mode*/)
        {
            InitializeComponent();
            LoadLookupData();
            //_mode = mode;

            LoadNotes(fkINImportID.Value);

            //if (fkINImportID.HasValue)
            //{
            //    //LoadMostRecentNotes(fkINImportID.Value);
            //    //NoteData.FKINImportID = fkINImportID.Value;
            //    LoadNotes(fkINImportID.Value);
            //}

            //SetFormMode(mode);
        }

        #endregion Constructor

        private void LoadLookupData()
        {
            DataTable dtSalesConsultants = Insure.INGetUsersAndAddMissingUser(((User)Embriant.Framework.Configuration.GlobalSettings.ApplicationUser).ID.ToString()); //Methods.GetTableData("SELECT * FROM INCancellationReason");
            //cmbLoggedInUser.Populate(dtSalesConsultants, "Description", "ID");
        }


        private void LoadNotes(long? fkINImportID)
        {
            if (!fkINImportID.HasValue)
            {
                return;
            }
            else
            {
                //DataTable dtMostRecentNote = Insure.INGetLatestNote(fkINImportID.Value);
                //GetModelFromDataTable(fkINImportID.Value, dtMostRecentNote);
                DataTable dtNotes = Insure.INGetLeadNotes(fkINImportID.Value);
                xdcNotes.DataSource = dtNotes.DefaultView;
                _hasNotes = (dtNotes.Rows.Count > 1);
            }
        }

        #region Obsolete

        //public void SetFormMode(NoteScreenMode mode)
        //{

        //    //if (mode == NoteScreenMode.New)
        //    //{
        //    //    //cmbLoggedInUser.SelectedValue = ((User)Embriant.Framework.Configuration.GlobalSettings.ApplicationUser).ID;
        //    //    //dteNoteDate.Value = DateTime.Now;
        //    //    //tbxNotes.Text = String.Empty;
        //    //    tbxNotes.IsReadOnly = false;
        //    //    btnSave.IsEnabled = true;
        //    //    btnNew.IsEnabled = false;
        //    //}
        //    //else
        //    //{
        //    //    tbxNotes.IsReadOnly = true;
        //    //    btnSave.IsEnabled = false;
        //    //    btnNew.IsEnabled = true;
        //    //}
        //}


        //private void LoadMostRecentNote(long? fkINImportID)
        //{
        //    if (!fkINImportID.HasValue)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        DataTable dtMostRecentNote = Insure.INGetLatestNote(fkINImportID.Value);
        //        GetModelFromDataTable(fkINImportID.Value, dtMostRecentNote);
        //    }
        //}

        //private void LoadNextNote()
        //{
        //    DataTable dtNextNote = Insure.INGetLeadNoteByImportIDAndSequence(NoteData.FKINImportID, NoteData.NextSequence);
        //    GetModelFromDataTable(NoteData.FKINImportID, dtNextNote);
        //}

        //private void LoadPreviousNote()
        //{
        //    DataTable dtNextNote = Insure.INGetLeadNoteByImportIDAndSequence(NoteData.FKINImportID, NoteData.PreviousSequence);
        //    GetModelFromDataTable(NoteData.FKINImportID, dtNextNote);
        //}

        //private void LoadMostRecentNotes(long? fkINImportID, byte notesToLoad)
        //{
        //    if (!fkINImportID.HasValue)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        DataTable dtMostRecentNote = Insure.INGetLatestNote(fkINImportID.Value);
        //        GetModelFromDataTable(fkINImportID.Value, dtMostRecentNote);
        //    }
        //}

        //private void LoadNextNote()
        //{
        //    DataTable dtNextNote = Insure.INGetLeadNoteByImportIDAndSequence(NoteData.FKINImportID, NoteData.NextSequence);
        //    GetModelFromDataTable(NoteData.FKINImportID, dtNextNote);
        //}

        //private void LoadPreviousNote()
        //{
        //    DataTable dtNextNote = Insure.INGetLeadNoteByImportIDAndSequence(NoteData.FKINImportID, NoteData.PreviousSequence);
        //    GetModelFromDataTable(NoteData.FKINImportID, dtNextNote);
        //}


        //private void GetNoteModelsFromDataTable(long fkINImportID, DataTable dataTable)
        //{
        //    //NoteData model = new NoteData();
        //    //DataTable dtMostRecentNote = Insure.INGetLatestNote(fkINImportID);

        //    if (dataTable.Rows.Count > 0)
        //    {
        //        NoteData.FKINImportID = Convert.ToInt64(dataTable.Rows[0]["FKINImportID"]);
        //        NoteData.FKUserID = Convert.ToInt64(dataTable.Rows[0]["FKUserID"]);
        //        NoteData.NoteDate = Convert.ToDateTime(dataTable.Rows[0]["NoteDate"]);
        //        NoteData.Note = dataTable.Rows[0]["Note"].ToString();
        //        NoteData.NoteCount = Convert.ToInt32(dataTable.Rows[0]["NoteCount"]);
        //        NoteData.Sequence = Convert.ToInt32(dataTable.Rows[0]["Sequence"]);

        //        NoteData.NextSequence = Convert.ToInt32(dataTable.Rows[0]["NextSequence"]);
        //        NoteData.PreviousSequence = Convert.ToInt32(dataTable.Rows[0]["PreviousSequence"]);

        //        NoteData.MinSequence = Convert.ToInt32(dataTable.Rows[0]["MinSequence"]);
        //        NoteData.MaxSequence = Convert.ToInt32(dataTable.Rows[0]["MaxSequence"]);

        //        NoteData.NavigatorText = dataTable.Rows[0]["NavigatorText"].ToString();
        //        NoteData.HasNotes = (NoteData.NoteCount > 0);

        //    }
        //    else
        //    {
        //        CreateDefaultNoteModel();
        //    }

        //    //return model;
        //}

        #endregion Obsolete


        private void GetNoteModelFromDataRecord(DataRecord record)
        {
            long? id = record.Cells["ID"].Value as long?;
            NoteData.FKINImportID = Convert.ToInt64(record.Cells["FKINImportID"].Value);
            NoteData.FKUserID = Convert.ToInt64(record.Cells["FKUserID"].Value);
            NoteData.NoteDate = Convert.ToDateTime(record.Cells["NoteDate"].Value);
            NoteData.Note = record.Cells["Note"].Value.ToString();
            NoteData.Sequence = Convert.ToInt32(record.Cells["Sequence"].Value);
            NoteData.CanBeModified = Convert.ToBoolean(record.Cells["CanBeModified"].Value);
        }

        //private void CreateDefaultNoteModel()
        //{
        //    //NoteData.FKINImportID = fkINImportID;
        //    NoteData.FKUserID = ((User)Embriant.Framework.Configuration.GlobalSettings.ApplicationUser).ID;
        //    NoteData.NoteDate = DateTime.Now;
        //    NoteData.Note = null;
        //    NoteData.NoteCount = 0;
        //    NoteData.Sequence = 0;
        //    NoteData.NextSequence = 0;
        //    NoteData.PreviousSequence = 0;

        //    NoteData.MinSequence = 0;
        //    NoteData.MaxSequence = 0;

        //    NoteData.NavigatorText = "0 of 0";
        //    NoteData.HasNotes = (NoteData.NoteCount > 0);
        //}


        //private void SaveNote()
        //{
        //    try
        //    {
        //        INImportNote note = new INImportNote();
        //        note.FKUserID = NoteData.FKUserID;
        //        note.FKINImportID = NoteData.FKINImportID;
        //        note.NoteDate = NoteData.NoteDate;
        //        note.Note = NoteData.Note;
        //        note.Sequence = NoteData.MaxSequence + 1;
        //        note.Save(_validationResult);

        //        OnDialogClose(_dialogResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        #region Event Handlers

        #region ComboBox - Specific Event Handlers

        private void EmbriantComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Methods.EmbriantComboBoxPreviewKeyDown(sender, e);
        }

        private void cmbLoggedInUser_Loaded(object sender, RoutedEventArgs e)
        {
            //Keyboard.Focus(cmbLoggedInUser);
        }

        private void cmbLoggedInUser_DropDownClosed(object sender, EventArgs e)
        {
            //try
            //{
            //    btnSave.IsEnabled = false;
            //    if (cmbLoggedInUser.SelectedValue != null && _leadApplicationScreen.cmbAgent.SelectedValue != null)
            //    {
            //        btnSave.IsEnabled = true;
            //    }
            //    else
            //    {
            //        btnSave.ToolTip = _leadApplicationScreen.btnSave.ToolTip;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            
        }

        #endregion ComboBox - Specific Event Handlers


        //private void btnNew_Click(object sender, RoutedEventArgs e)
        //{
        //    SetFormMode(NoteScreenMode.New);

        //    NoteData.FKUserID = ((User)Embriant.Framework.Configuration.GlobalSettings.ApplicationUser).ID;
        //    NoteData.NoteDate = DateTime.Now;
        //    NoteData.Note = null;

        //    btnPreviousNote.IsEnabled = false;
        //    btnNextNote.IsEnabled = false;

        //    if (NoteData.NoteCount == 0)
        //    {
        //        NoteData.NoteCount = 1;
        //        NoteData.Sequence = 1;
        //        NoteData.NextSequence = 1;
        //        NoteData.PreviousSequence = 1;
        //    }

        //    else
        //    {
        //        NoteData.NoteCount++;
        //        NoteData.PreviousSequence = NoteData.MaxSequence;
        //        NoteData.NextSequence = NoteData.MinSequence;
        //        NoteData.Sequence = NoteData.MaxSequence + 1;
        //    }
        //    NoteData.NavigatorText = String.Format("{0} of {0}", NoteData.NoteCount);
        //}


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //SaveNote();

            try
            {
                //CalculateCaptured();
                DataRecord currentRecord = (DataRecord)xdcNotes.ActiveRecord;
                INImportNote note;

                long? id = currentRecord.Cells["ID"].Value as long?;
                string currentNote = currentRecord.Cells["Note"].Value.ToString();

                if (NoteData.Note.Trim() != currentNote)
                {
                    if (currentNote.Trim() == String.Empty)
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"Saving can not continue, because blank notes are not allowed. The change to the note has been reversed.", "Blank Note", ShowMessageType.Information);

                        XamDataCards xamDataCards = sender as XamDataCards;
                        XamTextEditor activeTextEditor = null;

                        if (xamDataCards != null)
                        {
                            if (xamDataCards.ActiveCell != null)
                            {
                                activeTextEditor = Utilities.GetDescendantFromType(CellValuePresenter.FromCell(xamDataCards.ActiveRecord.DataPresenter.ActiveCell), typeof(XamTextEditor), true) as XamTextEditor;
                                activeTextEditor.Value = NoteData.Note.Trim();
                            }
                        }
                        //e.Handled = true;
                        return;
                    }

                    if (id.HasValue)
                    {
                        note = new INImportNote(id.Value);
                    }
                    else
                    {
                        note = new INImportNote();
                    }

                    note.FKUserID = NoteData.FKUserID;
                    note.FKINImportID = NoteData.FKINImportID;
                    note.NoteDate = NoteData.NoteDate;
                    note.Note = currentNote;
                    note.Sequence = NoteData.Sequence;
                    note.Save(_validationResult);

                    if (_validationResult.Passed)
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"The note has been saved successfully.", "Note Saved", ShowMessageType.Information);
                        OnDialogClose(_dialogResult);
                    }
                    else
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"An error has occurred while trying to save the note.", "Note Not Saved", ShowMessageType.Error);
                    }
                }
                //e.Handled = true;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void btnPreviousNote_Click(object sender, RoutedEventArgs e)
        {
        //    LoadPreviousNote();
        }

        private void btnNextNote_Click(object sender, RoutedEventArgs e)
        {
        //    LoadNextNote();
        }

        #endregion Event Handlers

        private void xdcNotes_CellActivated(object sender, Infragistics.Windows.DataPresenter.Events.CellActivatedEventArgs e)
        {
            try
            {
                //CalculateCaptured();
                DataRecord currentRecord = (DataRecord)xdcNotes.ActiveRecord;
                DataRow drCurrentRecord = ((DataRowView)currentRecord.DataItem).Row;

                GetNoteModelFromDataRecord(currentRecord);

                //bool canModifyNote = Convert.ToBoolean(currentRecord.Cells["CanBeModified"].Value);

                #region Set the value of _isForward

                if (currentRecord.Index == (xdcNotes.Records.Count - 1))
                {
                    _isForward = false;
                }
                else if (currentRecord.Index == 0)
                {
                    _isForward = true;
                }

                #endregion Set the value of _isForward

                if (e.Cell.Field.Name == "Note")
                {
                    e.Cell.Field.Settings.AllowEdit = NoteData.CanBeModified; //canModifyNote;
                    e.Handled = true;
                }

                if (e.Cell.Field.Settings.AllowEdit == false)
                {
                    xdcNotes.ExecuteCommand(_isForward ? DataPresenterCommands.CellBelow : DataPresenterCommands.CellAbove);
                    e.Handled = true;
                }

                btnSave.IsEnabled = NoteData.CanBeModified;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void xdcNotes_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            //if (e.Key == Key.Enter || e.Key == Key.Return)
            //{

            //    if (xdcNotes.ActiveCell != null && xdcNotes.ActiveCell.IsInEditMode)
            //    {
            //        CellValuePresenter cvp = CellValuePresenter.FromCell(xdcNotes.ActiveCell);

            //        bool ignore = false;

            //        if (cvp.Editor is XamTextEditor)
            //        {
            //            ignore = true;
            //        }

            //        if (!ignore)
            //        {
            //            ((XamDataCards)sender).ExecuteCommand(DataPresenterCommands.EndEditModeAndAcceptChanges);
            //            e.Handled = true;
            //        }
            //    }
            //}

            if (e.Key == Key.Tab)
            {
                //xdcNotes.ExecuteCommand(DataPresenterCommands.EndEditModeAndAcceptChanges);

                if (xdcNotes.ActiveCell != null && xdcNotes.ActiveCell.IsInEditMode)
                {
                    CellValuePresenter cvp = CellValuePresenter.FromCell(xdcNotes.ActiveCell);
                    XamTextEditor xte = cvp.Editor as XamTextEditor;

                    if (xte.Text.Length > 0)
                    {
                        //xdcNotes.ExecuteCommand(DataPresenterCommands.EndEditModeAndAcceptChanges);
                        //xdcNotes.ExecuteCommand(DataPresenterCommands.CellBelow);
                        xdcNotes.ExecuteCommand(_isForward ? DataPresenterCommands.CellNextByTab : DataPresenterCommands.CellPreviousByTab);
                        xdcNotes.ExecuteCommand(DataPresenterCommands.StartEditMode);
                        //e.Handled = true;
                    }
                }
                btnSave.IsEnabled = false;
            }

            /*
            try
            {
                //CalculateCaptured();
                XamDataCards xamDataCards = sender as XamDataCards;
                XamTextEditor activeTextEditor = null;
                //int inputLength = 0;

                if (xamDataCards != null)
                {
                    if (xamDataCards.ActiveCell != null)
                    {
                        #region Get active texteditor

                        switch (xamDataCards.ActiveCell.Field.Name)
                        {
                            case "Note":
                                activeTextEditor = Utilities.GetDescendantFromType(CellValuePresenter.FromCell(xamDataCards.ActiveRecord.DataPresenter.ActiveCell), typeof(XamTextEditor), true) as XamTextEditor;
                                //inputLength = NoNull(xamDataCards.ActiveCell.Value.ToString(), String.Empty).ToString().Length;
                                break;
                        }

                        #endregion Get active texteditor

                        #region Navigate with the arrow keys

                        switch (e.Key)
                        {
                            case Key.Right:
                            case Key.Down:
                            case Key.Tab:
                                _isForward = true;
                                xamDataCards.ExecuteCommand(DataPresenterCommands.CellBelow);
                                xamDataCards.ExecuteCommand(DataPresenterCommands.StartEditMode);
                                break;

                            case Key.Left:
                            case Key.Up:
                                _isForward = false;
                                xamDataCards.ExecuteCommand(DataPresenterCommands.CellAbove);
                                xamDataCards.ExecuteCommand(DataPresenterCommands.StartEditMode);
                                break;

                            default:
                                if (activeTextEditor != null)
                                {
                                    //if (e.Key == Key.Enter)
                                    //{
                                    //    activeTextEditor.Value = activeTextEditor.Value + Environment.NewLine;
                                    //}
                                    //else if ((e.Key >= Key.A) && (e.Key <= Key.Z))
                                    //{
                                    //    if (Keyboard.IsKeyToggled(Key.CapsLock))
                                    //    {
                                    //        activeTextEditor.Value = activeTextEditor.Value + e.Key.ToString();
                                    //    }
                                    //    else
                                    //    {
                                    //        activeTextEditor.Value = activeTextEditor.Value + e.Key.ToString().ToLower();
                                    //    }
                                    //}
                                    //else if (((e.Key >= Key.NumPad0) && (e.Key <= Key.NumPad9)) ||
                                    //    ((e.Key >= Key.D0 && e.Key <= Key.D9)))
                                    //{
                                    //    activeTextEditor.Value = activeTextEditor.Value + e.Key.ToString();
                                    //}

                                    if (e.Key == Key.Back)
                                    {
                                        if (activeTextEditor.Value.ToString().Length > 0)
                                        {
                                            activeTextEditor.Value = activeTextEditor.Value.ToString().Substring(0, activeTextEditor.Value.ToString().Length - 1);
                                        }
                                    }

                                    else
                                    {
                                        activeTextEditor.Value = activeTextEditor.Value + Methods.GetStringFromKeyStroke(e.Key);
                                    }

                                    activeTextEditor.SelectionStart = activeTextEditor.Value.ToString().Length;
                                }

                                break;
                        }

                        #endregion Navigate with the arrow keys

                        //xdcNotes_RecordDeactivating(null, null);

                    }
                    else
                    {
                        return;
                    }
                }

                e.Handled = true;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
            */

        }

        private void xdcNotes_RecordDeactivating(object sender, Infragistics.Windows.DataPresenter.Events.RecordDeactivatingEventArgs e)
        {

        }

        private void xdcNotes_CellDeactivating(object sender, Infragistics.Windows.DataPresenter.Events.CellDeactivatingEventArgs e)
        {
            /*
            try
            {
                //CalculateCaptured();
                DataRecord currentRecord = (DataRecord)xdcNotes.ActiveRecord;
                INImportNote note;

                long? id = currentRecord.Cells["ID"].Value as long?;
                string currentNote = currentRecord.Cells["Note"].Value.ToString();

                if (NoteData.Note.Trim() != currentNote)
                {
                    if (currentNote.Trim() == String.Empty)
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"Saving can not continue, because blank notes are not allowed. The change to the note has been reversed.", "Blank Note", ShowMessageType.Information);

                        XamDataCards xamDataCards = sender as XamDataCards;
                        XamTextEditor activeTextEditor = null;

                        if (xamDataCards != null)
                        {
                            if (xamDataCards.ActiveCell != null)
                            {
                                activeTextEditor = Utilities.GetDescendantFromType(CellValuePresenter.FromCell(xamDataCards.ActiveRecord.DataPresenter.ActiveCell), typeof(XamTextEditor), true) as XamTextEditor;
                                activeTextEditor.Value = NoteData.Note.Trim();
                            }
                        }
                        e.Handled = true;
                        return;
                    }

                    if (id.HasValue)
                    {
                        note = new INImportNote(id.Value);
                    }
                    else
                    {
                        note = new INImportNote();
                    }

                    note.FKUserID = NoteData.FKUserID;
                    note.FKINImportID = NoteData.FKINImportID;
                    note.NoteDate = NoteData.NoteDate;
                    note.Note = currentNote;
                    note.Sequence = NoteData.Sequence;
                    note.Save(_validationResult);
                }
                e.Handled = true;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
            */
        }
    }
}
