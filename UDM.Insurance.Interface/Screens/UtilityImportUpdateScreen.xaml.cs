using Embriant.Framework;
using Embriant.Framework.Data;
using Embriant.WPF.Controls;
using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.Editors;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using ValidationResult = Embriant.Framework.Validation.ValidationResult;

namespace UDM.Insurance.Interface.Screens
{
    public partial class UtilityImportUpdateScreen : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion



        #region Properties

        private bool? _IsAllRecordsSelected = false;
        public bool? IsAllRecordsSelected
        {
            get
            {
                return _IsAllRecordsSelected;
            }
            set
            {
                _IsAllRecordsSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAllRecordsSelected"));
            }
        }

        private bool? _IsImportRunning = false;
        public bool? IsImportRunning
        {
            get
            {
                return _IsImportRunning;
            }
            set
            {
                _IsImportRunning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsImportRunning"));
            }
        }

        #endregion



        #region Constants

        private const string ImportFilterExpression = "(.xls;.xlsx;.xlsm)|*.xlsx;*.xls;*.xlsm";
        private const string ImportFileExtention = ".xls";

        #endregion



        #region Members

        private string strSelectedTable;
        private List<DataRecord> _selectedFields;

        private readonly DispatcherTimer _TimerImport = new DispatcherTimer();
        private int _ImportTime;

        private readonly DispatcherTimer _TimerOpenWorkBook = new DispatcherTimer();
        private bool _fileError;

        private Workbook _workbook;
        private Worksheet _workSheet;
        public static INCampaign _inCampaign;
        public static INBatch _InBatch;
        private string _batchCode;
        private string _batchCode2;
        private string _campaignName; 
        private string _campaignCode;

        int _rowCount;
        int _headerRow;

        class ImportHeaderInfo
        {
            public ImportHeaderInfo()
            {
                Index = -1;
            }

            public string Header { get; set; }
            public string HeaderAlt1 { get; set; }
            public string HeaderAlt2 { get; set; }
            public string HeaderAlt3 { get; set; }
            public int Index { get; set; }
        }

        private Dictionary<string, ImportHeaderInfo> idxFields;

        private readonly ValidationResult _validationResult = new ValidationResult();

        #endregion



        #region Constructors

        public UtilityImportUpdateScreen()
        {
            InitializeComponent();

            LoadTables();

            _TimerOpenWorkBook.Tick += TimerOpenWorkBook;
            _TimerOpenWorkBook.Interval = new TimeSpan(0, 0, 0, 0, 300);

            _TimerImport.Tick += TimerImport;
            _TimerImport.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion



        #region Methods

        private void TimerOpenWorkBook(object sender, EventArgs e)
        {
            try
            {
                string str = pbImportText.Text;
                int strLength = str.Length;
                char chr = ' ';

                switch (str[strLength - 1])
                {
                    case '-':
                        chr = '\x00F7';
                        break;

                    case '\x00F7':
                        chr = '-';
                        break;
                }

                str = str.Substring(0, strLength - 1) + chr;
                pbImportText.Text = str;
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        private void TimerImport(object sender, EventArgs e)
        {
            try
            {
                _ImportTime++;
                btnImport.Content = TimeSpan.FromSeconds(_ImportTime).ToString();
                //btnImport.ToolTip = btnImport.Content;
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        private void LoadTables()
        {
            try
            {
                Cursor = Cursors.Wait;

                DataTable dt = Methods.GetTableData("SELECT ROW_NUMBER() OVER (ORDER BY Name) [Rank], Name FROM sys.Tables ORDER BY Name");

                cmbTables.Populate(dt, "Name", "Rank");
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }

            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void LoadImportHeaders()
        {
            try
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart) delegate
                {
                    Cursor = Cursors.Wait;

                    _selectedFields = (xdgFields.Records.Select(r => (DataRecord)r).Where(r => (bool)r.Cells["Select"].Value)).ToList();
                    string strSelectedFields = "";
                    foreach (DataRecord dr in _selectedFields)
                    {
                        strSelectedFields = strSelectedFields + "'" + dr.Cells["FieldName"].Value + "',";
                    }
                    strSelectedFields = strSelectedFields.Substring(0, strSelectedFields.Length - 1) + "";

                    idxFields = new Dictionary<string, ImportHeaderInfo>();
                    DataTable dt = Methods.GetTableData("SELECT * FROM INImportHeader WHERE (TableName = '" + strSelectedTable + "' AND Name IN (" + strSelectedFields + ")) OR Name = 'RefNo'");

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            idxFields.Add(row["Name"].ToString(), new ImportHeaderInfo { Header = row["Header"].ToString(), HeaderAlt1 = row["HeaderAlt1"].ToString(), HeaderAlt2 = row["HeaderAlt2"].ToString(), HeaderAlt3 = row["HeaderAlt3"].ToString() });
                        }
                    }
                });
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }

            finally
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart) delegate
                {
                    Cursor = Cursors.Arrow; 
                });
            }
        }

        private bool? AllRecordsSelected()
        {
            try
            {
                bool allSelected = (xdgFields.Records.Select(r => (bool)((DataRecord)r).Cells["Select"].Value)).All(b => b);
                bool noneSelected = (xdgFields.Records.Select(r => (bool)((DataRecord)r).Cells["Select"].Value)).All(b => !b);

                if (allSelected)
                {
                    return true;
                }
                if (noneSelected)
                {
                    return false;
                }

                return null;
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
                return null;
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

        public void OpenWorkBook(object sender, DoWorkEventArgs e)
        {
            try
            {
                LoadImportHeaders();

                //First reset all controls
                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    medFile.Text = string.Empty;
                    tbCampaignType.Text = string.Empty;
                    tbCampaignGroup.Text = string.Empty;
                    tbBatch.Text = string.Empty;
                    tbUDMBatch.Text = string.Empty;
                    tbTotalLeads.Text = string.Empty;
                    tbCampaign.Text = string.Empty;
                    pbImportText.Text = string.Empty;
                    pbImportText.Foreground = Brushes.Black;
                });

                _workbook = null;
                _workSheet = null;
                _inCampaign = null;

                //Open file
                string filePathAndName = string.Empty;
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.DefaultExt = ImportFileExtention;
                dlg.Filter = ImportFilterExpression;

                bool? result = dlg.ShowDialog();
                if (result == true)
                {
                    filePathAndName = dlg.FileName;
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart) (() => medFile.Text = filePathAndName));
                }

                if (!string.IsNullOrWhiteSpace(filePathAndName))
                {
                    string fileName = System.IO.Path.GetFileName(filePathAndName);

                    #region Check if filename is in correct format

                    if (fileName != null && !Regex.IsMatch(fileName, @"^Batch [a-zA-Z0-9]+ \(.+\) [a-zA-Z0-9]+[ ]*.xlsx?"))
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            INMessageBoxWindow1 messageBox = new INMessageBoxWindow1();
                            messageBox.Message = "The import file name is in the wrong format.\nThe file name should be in the format\n\"Batch xxx (campaign code) xxx.\"";
                            messageBox.Header = "Import File Name";
                            messageBox.MessageType = ShowMessageType.Error;
                            messageBox.Owner = this;
                            messageBox.ShowDialog();
                        });

                        _fileError = true;
                        return;
                    }

                    #endregion

                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)(() => pbImportText.Text = "reading file ­­­­­" + '-'));
                    _TimerOpenWorkBook.Start();
                    _workbook = Workbook.Load(filePathAndName);

                    _workSheet = _workbook.Worksheets[0];
                    _rowCount = 0;

                    #region Check if all columns are available and determine column indexes

                    {
                        string[] strKey = { idxFields.ElementAt(0).Key };

                        Action headerErrorAction = () =>
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                            {
                                INMessageBoxWindow1 messageBox = new INMessageBoxWindow1();
                                messageBox.Message = "The header field for key \"" + strKey[0] + "\" could not be found.";
                                messageBox.Header = "Import File Alert";
                                messageBox.MessageType = ShowMessageType.Error;
                                messageBox.Owner = this;
                                messageBox.ShowDialog();
                            });

                            _fileError = true;
                        };

                        List<string> lstHeaders = new List<string>
                            {
                                idxFields.ElementAt(0).Value.Header, 
                                idxFields.ElementAt(0).Value.HeaderAlt1,
                                idxFields.ElementAt(0).Value.HeaderAlt2,
                                idxFields.ElementAt(0).Value.HeaderAlt3
                            };

                        WorksheetCell wsCell = lstHeaders.Select(str => Methods.WorksheetFindText(_workSheet, str, 0, 0, 1024, 1024, false, true)).FirstOrDefault(cell => cell != null);
                        if (wsCell != null)
                        {
                            _headerRow = wsCell.RowIndex;

                            for (int index = 0; index < idxFields.Count; index++)
                            {
                                strKey[0] = idxFields.ElementAt(index).Key;

                                lstHeaders = new List<string>
                                    {
                                        idxFields.ElementAt(index).Value.Header, 
                                        idxFields.ElementAt(index).Value.HeaderAlt1,
                                        idxFields.ElementAt(index).Value.HeaderAlt2,
                                        idxFields.ElementAt(index).Value.HeaderAlt3
                                    };

                                wsCell = lstHeaders.Select(str => Methods.WorksheetFindText(_workSheet, str, _headerRow, 0, _headerRow, 1024, false, true)).FirstOrDefault(cell => cell != null);
                                if (wsCell != null)
                                {
                                    idxFields[strKey[0]].Index = wsCell.ColumnIndex;
                                }
                                else
                                {
                                    headerErrorAction();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            headerErrorAction();
                        }
                    }

                    #endregion

                    if (fileName != null)
                    {
                        //get batch code and filename
                        fileName = fileName.Replace("Batch", string.Empty);
                        fileName = fileName.Replace("_", " ");
                        fileName = fileName.Replace("  ", " ");
                        fileName = fileName.Trim();

                        _batchCode = fileName.Substring(0, fileName.IndexOf('(')).Trim();
                        _batchCode2 = fileName.Substring(fileName.IndexOf(") ", StringComparison.Ordinal) + 2, fileName.IndexOf('.') - (fileName.IndexOf(") ", StringComparison.Ordinal) + 2)).Trim();
                        _campaignName = fileName.Substring(fileName.IndexOf('(') + 1, fileName.IndexOf(')') - fileName.IndexOf('(') - 1); 
                        _campaignCode = fileName.Substring(fileName.IndexOf('(') + 1, fileName.IndexOf(')') - fileName.IndexOf('(') - 1).Replace(" ", string.Empty).ToUpper();
                        _inCampaign = INCampaignMapper.SearchOne(null, null, null, _campaignCode, null, null, null, null, null, null, null, null);
                        if (_inCampaign != null)
                        {
                            _InBatch = INBatchMapper.SearchOne(_inCampaign.ID, _batchCode, null, null, null, null, null,null,null,null);

                            if (_InBatch == null)
                            {
                                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart) delegate
                                {
                                    INMessageBoxWindow1 messageBox = new INMessageBoxWindow1();
                                    messageBox.Message = "The batch " + _batchCode + " does not exist.";
                                    messageBox.Header = "Import File Alert";
                                    messageBox.MessageType = ShowMessageType.Error;
                                    messageBox.Owner = this;
                                    messageBox.ShowDialog();
                                    _fileError = true;
                                });
                                return;
                            }
                        }
                        else
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart) delegate
                            {
                                INMessageBoxWindow1 messageBox = new INMessageBoxWindow1();
                                messageBox.Message = "The campaign " + _campaignCode + " does not exist.";
                                messageBox.Header = "Import File Alert";
                                messageBox.MessageType = ShowMessageType.Error;
                                messageBox.Owner = this;
                                messageBox.ShowDialog();
                                _fileError = true;
                            });
                            return;
                        }
                        
                        //Display import information
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            //Get the row count
                            _rowCount = _workSheet.Rows.Select(row => row).Count(row => (row.Index > _headerRow && Methods.NoNull(row.Cells[idxFields["RefNo"].Index].Value, string.Empty).ToString() != string.Empty));

                            tbCampaign.Text = _campaignName;
                            if (_inCampaign.FKINCampaignTypeID != null) tbCampaignType.Text = ((lkpINCampaignType)_inCampaign.FKINCampaignTypeID).ToString();
                            if (_inCampaign.FKINCampaignGroupID != null) tbCampaignGroup.Text = ((lkpINCampaignGroup)_inCampaign.FKINCampaignGroupID).ToString();
                            tbBatch.Text = _batchCode;
                            tbUDMBatch.Text = _batchCode2;
                            tbTotalLeads.Text = _rowCount.ToString();
                            _fileError = false;
                        });
                    }
                }
                else
                {
                    _fileError = true;
                }
            }

            catch (Exception ex)
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    (new BaseControl()).HandleException(ex);
                });
            }
        }

        private void OpenWorkBookComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                Cursor = Cursors.Arrow;
                _TimerOpenWorkBook.Stop();

                if (_fileError)
                {
                    pbImportText.Foreground = Brushes.Red;
                    pbImportText.Text = "Error";
                }
                else
                {
                    pbImportText.Foreground = Brushes.Black;
                    pbImportText.Text = "Ready";
                }
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        private Double? GetFloatValue(WorksheetCell cell)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim();
                if (!string.IsNullOrWhiteSpace(str))
                {
                    return Convert.ToDouble(str);
                }
            }

            return null;
        }

        private Decimal? GetDecimalValue(WorksheetCell cell)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim();
                if (!string.IsNullOrWhiteSpace(str))
                {
                    return Convert.ToDecimal(str);
                }
            }

            return null;
        }

        private Int32? GetIntegerValue(WorksheetCell cell)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim();
                Int32 value;

                if (Int32.TryParse(str, out value))
                {
                    return Convert.ToInt32(str);
                }
            }   

            return null;
        }

        private long? GetLongValue(WorksheetCell cell)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim();
                Int64 value;

                if (Int64.TryParse(str, out value))
                {
                    return Convert.ToInt64(str);
                }
            }

            return null;
        }

        private string GetStringValue(WorksheetCell cell)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim();
                if (!string.IsNullOrWhiteSpace(str))
                {
                    return str;
                }
            }

            return null;
        }

        private string GetEMailAddress(WorksheetCell cell)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim().Replace(" ", "");
                if (!string.IsNullOrWhiteSpace(str))
                {
                    if (Methods.IsValidEmail(str))
                    {
                        return str;
                    }
                }
            }

            return null;
        }

        private DateTime? GetDateValue(WorksheetCell cell)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim();
                str = Methods.ExcelFieldToDate(str);
                if (!string.IsNullOrWhiteSpace(str))
                {
                    return Convert.ToDateTime(str);
                }
            }

            return null;
        }

        private TimeSpan? GetTimeValue(WorksheetCell cell)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim();
                if (!string.IsNullOrWhiteSpace(str))
                {
                    return TimeSpan.Parse(str);
                }
            }

            return null;
        }

        private long? GetLookupID(WorksheetCell cell, string lookup)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim();
                string strLower = str.ToLower();
                DataTable dt;

                switch (str)
                {
                    case "afr":
                        str = "Afrikaans";
                        break;

                    case "eng":
                        str = "English";
                        break;

                    case "m":
                        str = "Male";
                        break;

                    case "f":
                        str = "Female";
                        break;
                }

                if (!string.IsNullOrWhiteSpace(str))
                {
                    if (lookup == "BankBranch")
                    {
                        dt = Methods.GetTableData("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED SELECT ID FROM " + lookup + " WHERE [Code] = '" + str + "'");

                        if (dt.Rows.Count > 0)
                        {
                            return Convert.ToInt64(dt.Rows[0]["ID"].ToString());
                        }

                        return null;
                    }

                    dt = Methods.GetTableData("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED SELECT ID FROM " + lookup + " WHERE [Description] = '" + str + "'");

                    if (dt.Rows.Count > 0)
                    {
                        return Convert.ToInt64(dt.Rows[0]["ID"].ToString());
                    }



                    //if (lookup == "lkpINTitle" || lookup == "lkpINRelationship")
                    //{
                    //    Lookup newLookup = new Lookup(lookup);
                    //    newLookup.Description = str;
                    //    newLookup.Save(_validationResult);

                    //    return newLookup.ID;
                    //}

                    //search for keywords in lookup fields to reduce number of names for the same thing
                    string strQuery;
                    lkpKeywordType keywordType = new lkpKeywordType();

                    switch (lookup)
                    {
                        case "lkpBank":
                            keywordType = lkpKeywordType.Bank;
                            break;

                        case "lkpAccountType":
                            keywordType = lkpKeywordType.AccountType;
                            break;

                        case "lkpPaymentMethod":
                            keywordType = lkpKeywordType.PaymentMethod;
                            break;
                    }

                    if (keywordType != 0)
                    {
                        strQuery = "USE Blush SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED ";
                        strQuery += "SELECT * FROM Keyword WHERE FKKeywordTypeID = '" + (long)keywordType + "'";

                        dt = Methods.GetTableData(strQuery);

                        foreach (DataRow dr in dt.Rows)
                        {
                            string strTerm = (string)dr["Term"];
                            strTerm = strTerm.ToLower();

                            if (strLower.Contains(strTerm))
                            {
                                return Convert.ToInt64(dr["LookupID"].ToString());
                            }
                        }
                    }
                }
            }

            return null;
        }

        private void CancelBatchImport()
        {
            INMessageBoxWindow1 messageBox = new INMessageBoxWindow1();
            messageBox.Message = "An error has occurred.\nThe batch import will now be cancelled.";
            messageBox.Header = "Batch Import Error";
            messageBox.MessageType = ShowMessageType.Error;
            messageBox.Owner = this;
            messageBox.ShowDialog();
            Database.CancelTransactions();
            _TimerImport.Stop();
            IsImportRunning = false;
        }

        private bool ImportLeads()
        {
            try
            {
                pbImport.Minimum = 0;
                pbImport.Maximum = _rowCount;
                pbImport.Value = 0;
                double[] pbCounter = {0};

                if (_workSheet != null)
                {
                    _TimerImport.Start();

                    foreach (WorksheetRow row in _workSheet.Rows)
                    {
                        if (row.Index > _headerRow)
                        {
                            string strRefNo = null;
                            if (row.Cells[idxFields["RefNo"].Index].Value != null)
                            {
                                strRefNo = GetStringValue(row.Cells[idxFields["RefNo"].Index]);
                            }

                            if (!string.IsNullOrWhiteSpace(strRefNo) && strRefNo.Length > 8)
                            {

                                #region ******* do import update work *******
                                
                                string strQuery = "SELECT ID [ImportID], FKINImportedPolicyDataID [ImportedPolicyDataID] FROM INImport WHERE RefNo = '" + strRefNo + "' AND FKINBatchID = '" + _InBatch.ID + "'";
                                DataTable dt = Methods.GetTableData(strQuery);
                                if (dt.Rows.Count == 1)
                                {
                                    long importID = (long)dt.Rows[0]["ImportID"];
                                    INImport import = new INImport(importID);
                                    long? importedPolicyDataID = dt.Rows[0]["ImportedPolicyDataID"] as long?;

                                    switch (strSelectedTable)
                                    {
                                        case "INImportedPolicyData":

                                            INImportedPolicyData importedPolicyData = importedPolicyDataID != null ? new INImportedPolicyData((long)importedPolicyDataID) : new INImportedPolicyData();
                                            List<WorksheetCell> wsCells = _selectedFields.Select(dr => row.Cells[idxFields[dr.Cells["FieldName"].Value.ToString()].Index]).ToList();

                                            //Check if there are any cell entries for these details
                                            IEnumerable<WorksheetCell> wsCellsWithEntries = wsCells.Select(cell => cell).Where(cell => cell.Value != null).Where(cell => !string.IsNullOrWhiteSpace(cell.Value.ToString()));

                                            if (wsCellsWithEntries.Any())
                                            {
                                                foreach (DataRecord dr in _selectedFields)
                                                {
                                                    string fieldName = dr.Cells["FieldName"].Value.ToString();

                                                    switch (fieldName)
                                                    {
                                                        case "CommenceDate":
                                                            importedPolicyData.CommenceDate = GetDateValue(row.Cells[idxFields["CommenceDate"].Index]);
                                                            break;

                                                        case "AppSignDate":
                                                            importedPolicyData.AppSignDate = GetDateValue(row.Cells[idxFields["AppSignDate"].Index]);
                                                            break;

                                                        case "ContractPremium":
                                                            importedPolicyData.ContractPremium = GetDecimalValue(row.Cells[idxFields["ContractPremium"].Index]);
                                                            break;

                                                        case "ContractTerm":
                                                            importedPolicyData.ContractTerm = GetIntegerValue(row.Cells[idxFields["ContractTerm"].Index]);
                                                            break;

                                                        case "LapseDate":
                                                            importedPolicyData.LapseDate = GetDateValue(row.Cells[idxFields["LapseDate"].Index]);
                                                            break;

                                                        case "LA1CancerCover":
                                                            importedPolicyData.LA1CancerCover = GetDecimalValue(row.Cells[idxFields["LA1CancerCover"].Index]);
                                                            break;

                                                        case "LA1CancerPremium":
                                                            importedPolicyData.LA1CancerPremium = GetDecimalValue(row.Cells[idxFields["LA1CancerPremium"].Index]);
                                                            break;

                                                        case "LA1AccidentalDeathCover":
                                                            importedPolicyData.LA1AccidentalDeathCover = GetDecimalValue(row.Cells[idxFields["LA1AccidentalDeathCover"].Index]);
                                                            break;

                                                        case "LA1AccidentalDeathPremium":
                                                            importedPolicyData.LA1AccidentalDeathPremium = GetDecimalValue(row.Cells[idxFields["LA1AccidentalDeathPremium"].Index]);
                                                            break;

                                                        case "LA1DisabilityCover":
                                                            importedPolicyData.LA1DisabilityCover = GetDecimalValue(row.Cells[idxFields["LA1DisabilityCover"].Index]);
                                                            break;

                                                        case "LA1DisabilityPremium":
                                                            importedPolicyData.LA1DisabilityPremium = GetDecimalValue(row.Cells[idxFields["LA1DisabilityPremium"].Index]);
                                                            break;

                                                        case "LA1FuneralCover":
                                                            importedPolicyData.LA1FuneralCover = GetDecimalValue(row.Cells[idxFields["LA1FuneralCover"].Index]);
                                                            break;

                                                        case "LA1FuneralPremium":
                                                            importedPolicyData.LA1FuneralPremium = GetDecimalValue(row.Cells[idxFields["LA1FuneralPremium"].Index]);
                                                            break;

                                                        case "LA2CancerCover":
                                                            importedPolicyData.LA2CancerCover = GetDecimalValue(row.Cells[idxFields["LA2CancerCover"].Index]);
                                                            break;

                                                        case "LA2CancerPremium":
                                                            importedPolicyData.LA2CancerPremium = GetDecimalValue(row.Cells[idxFields["LA2CancerPremium"].Index]);
                                                            break;

                                                        case "LA2AccidentalDeathCover":
                                                            importedPolicyData.LA2AccidentalDeathCover = GetDecimalValue(row.Cells[idxFields["LA2AccidentalDeathCover"].Index]);
                                                            break;

                                                        case "LA2AccidentalDeathPremium":
                                                            importedPolicyData.LA2AccidentalDeathPremium = GetDecimalValue(row.Cells[idxFields["LA2AccidentalDeathPremium"].Index]);
                                                            break;

                                                        case "LA2DisabilityCover":
                                                            importedPolicyData.LA2DisabilityCover = GetDecimalValue(row.Cells[idxFields["LA2DisabilityCover"].Index]);
                                                            break;

                                                        case "LA2DisabilityPremium":
                                                            importedPolicyData.LA2DisabilityPremium = GetDecimalValue(row.Cells[idxFields["LA2DisabilityPremium"].Index]);
                                                            break;

                                                        case "LA2FuneralCover":
                                                            importedPolicyData.LA2FuneralCover = GetDecimalValue(row.Cells[idxFields["LA2FuneralCover"].Index]);
                                                            break;

                                                        case "LA2FuneralPremium":
                                                            importedPolicyData.LA2FuneralPremium = GetDecimalValue(row.Cells[idxFields["LA2FuneralPremium"].Index]);
                                                            break;

                                                        case "KidsCancerCover":
                                                            importedPolicyData.KidsCancerCover = GetDecimalValue(row.Cells[idxFields["KidsCancerCover"].Index]);
                                                            break;

                                                        case "KidsCancerPremium":
                                                            importedPolicyData.KidsCancerPremium = GetDecimalValue(row.Cells[idxFields["KidsCancerPremium"].Index]);
                                                            break;

                                                        case "KidsDisabilityCover":
                                                            importedPolicyData.KidsDisabilityCover = GetDecimalValue(row.Cells[idxFields["KidsDisabilityCover"].Index]);
                                                            break;

                                                        case "KidsDisabilityPremium":
                                                            importedPolicyData.KidsDisabilityPremium = GetDecimalValue(row.Cells[idxFields["KidsDisabilityPremium"].Index]);
                                                            break;

                                                        case "MoneyBackPremium":
                                                            importedPolicyData.MoneyBackPremium = GetDecimalValue(row.Cells[idxFields["MoneyBackPremium"].Index]);
                                                            break;

                                                        case "MoneyBackTerm":
                                                            importedPolicyData.MoneyBackTerm = Convert.ToInt16(GetIntegerValue(row.Cells[idxFields["MoneyBackTerm"].Index]));
                                                            break;

                                                    }
                                                }
                                            }

                                            importedPolicyData.Save(_validationResult);
                                            import.FKINImportedPolicyDataID = importedPolicyData.ID;
                                            import.Save(_validationResult);
                                            break;
                                    }
                                }

                                #endregion
                                
                            }

                            #region update the progressbar

                            pbCounter[0] += 1;

                            pbImport.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                            {
                                pbImport.Value = pbCounter[0];
                                pbImportText.Text = pbCounter[0].ToString();
                            }));

                            #endregion
                        }
                    }

                    _TimerImport.Stop();
                    return true;
                }

                //import not successfull
                _TimerImport.Stop();
                return false;
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
                _TimerImport.Stop();
                return false;
            }
        }

        #endregion



        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IsImportRunning = true;
                Database.BeginTransaction(null, IsolationLevel.Snapshot);






                //import
                bool result = ImportLeads();
                if (!result) { CancelBatchImport(); return; }

                Database.CompleteTransaction(_validationResult);
                IsImportRunning = false;

                INMessageBoxWindow1 messageBox = new INMessageBoxWindow1();
                messageBox.Message = "Batch import completed.";
                messageBox.Header = "Batch Import"; 
                messageBox.MessageType = ShowMessageType.Information;
                messageBox.Owner = this;
                messageBox.ShowDialog();
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        private void EmbriantComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Methods.EmbriantComboBoxPreviewKeyDown(sender, e);
        }

        private void cmbTables_Loaded(object sender, RoutedEventArgs e)
        {
            cmbTables.Focus();
        }

        private void cmbTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbTables.SelectedIndex > -1)
                {
                    Cursor = Cursors.Wait;

                    strSelectedTable = ((DataRowView)cmbTables.SelectedItem).Row["Name"].ToString();

                    DataTable dt = Methods.GetTableData(string.Format("SELECT ID [INImportHeaderID], Name [FieldName], Header [ImportSheetHeader] FROM INImportHeader WHERE TableName = '{0}' ORDER BY ID ASC", strSelectedTable));

                    DataColumn column = new DataColumn("Select", typeof(bool));
                    column.DefaultValue = false;
                    dt.Columns.Add(column);

                    xdgFields.DataSource = dt.DefaultView;
                }
                else
                {
                    xdgFields.DataSource = null;
                }

                IsAllRecordsSelected = false;
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }

            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void HeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                #region Add data trigger for report button style

                {
                    Style style = new Style();

                    style.TargetType = typeof(Button);
                    style.BasedOn = (Style)FindResource("ImportButton");

                    DataTrigger trigger = new DataTrigger();
                    trigger.Value = "False";
                    trigger.Binding = new Binding { Source = sender, Path = new PropertyPath("IsChecked") };
                    Setter setter = new Setter();
                    setter.Property = IsEnabledProperty;
                    setter.Value = false;
                    trigger.Setters.Add(setter);

                    style.Triggers.Add(trigger);

                    btnImport.Style = style;
                }

                #endregion

                #region Bind header checkbox ischecked property to IsAllRecordsSelected

                {
                    Binding binding = new Binding { Source = this, Path = new PropertyPath("IsAllRecordsSelected") };
                    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    binding.Mode = BindingMode.TwoWay;
                    ((CheckBox)sender).SetBinding(ToggleButton.IsCheckedProperty, binding);
                }

                #endregion

            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (xdgFields.DataSource != null)
                {
                    DataTable dt = ((DataView)xdgFields.DataSource).Table;

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["Select"] = true;
                    }
                }
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        private void HeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (xdgFields.DataSource != null)
                {
                    DataTable dt = ((DataView)xdgFields.DataSource).Table;

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["Select"] = false;
                    }
                }
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            IsAllRecordsSelected = AllRecordsSelected();
        }

        private void ctrlUtilityImportUpdateScreen_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        private void ctrlUtilityImportUpdateScreen_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void xamEditor_GotFocus(object sender, RoutedEventArgs e)
        {
            xamEditor_Select(sender);
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pbImport.Value = 0;
                btnImport.Content = "Update";

                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += OpenWorkBook;
                worker.RunWorkerCompleted += OpenWorkBookComplete;
                worker.RunWorkerAsync(worker);
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        private void medFile_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox tb = Methods.FindChild<TextBox>(medFile, "PART_InputTextBox");
                tb.IsTabStop = false;
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        #endregion
        
    }
}
