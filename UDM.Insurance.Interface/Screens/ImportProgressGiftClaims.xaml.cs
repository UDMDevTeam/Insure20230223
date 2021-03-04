using Embriant.Framework;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using Embriant.Framework.Library;
using Infragistics.Documents.Excel;
using Infragistics.Windows.Editors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Resources;
using System.Windows.Threading;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;


namespace UDM.Insurance.Interface.Screens
{

	public partial class ImportProgressGiftClaims
    {

        #region Constants

        private const string ImportFilterExpression = "(.xls;.xlsx;.xlsm)|*.xlsx;*.xls;*.xlsm";
        private const string ImportFileExtention = ".xls";

        #endregion



        #region Private Members
        
        private Workbook _workbook;
        private Worksheet _workSheet;
        int _rowCount;
        int _headerRow;
        private readonly DispatcherTimer _TimerOpenWorkBook = new DispatcherTimer();
        private readonly DispatcherTimer _TimerImport = new DispatcherTimer();
        private int _ImportTime;

        private bool? _fileError;

        class ImportHeaderInfo
        {
            public ImportHeaderInfo()
            {
                Index = -1;
                IgnoreIfNA = true;
            }

            public string Header { get; set; }
            public int Index { get; set; }
            public bool IgnoreIfNA { get; set; }
        }

        private readonly Dictionary<string, ImportHeaderInfo> idxFields = new Dictionary<string, ImportHeaderInfo>()
        {
            //Old Import Header Checks
            //{"RefNo", new ImportHeaderInfo { Header = "Reference number"}},
            ////{"RedeemedDate", new ImportHeaderInfo { Header = "Date voucher claimed"}},
            //{"PODDate", new ImportHeaderInfo { Header = "Date Delivered"}},
            //{"PODSignature", new ImportHeaderInfo { Header = "POD Details"}}

            //New Import Header Checks written by Nicolas Stephenson
            {"RefNo", new ImportHeaderInfo { Header = "Shipper Reference" } },
            {"PODDate", new ImportHeaderInfo { Header = "Delivered" } },
            {"PODSignature", new ImportHeaderInfo { Header = "Received By"} }
        };
        
        lkpGiftClaimsImportType _importType;

        #endregion



        #region Constructors

        public ImportProgressGiftClaims(lkpGiftClaimsImportType importType)
        {
            InitializeComponent();

            _importType = importType;
            if (_importType == lkpGiftClaimsImportType.SMS)
            {
                idxFields.Add("PODTelNumber", new ImportHeaderInfo { Header = "Receiver Suburb"});
            }

            _TimerOpenWorkBook.Tick += TimerOpenWorkBook;
            _TimerOpenWorkBook.Interval = new TimeSpan(0, 0, 0, 0, 300);

            _TimerImport.Tick += TimerImport;
            _TimerImport.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion



        #region Private Methods

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
                HandleException(ex);
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
                HandleException(ex);
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
                //First reset all controls
                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    medFile.Text = string.Empty;
                    tbTotalLeads.Text = string.Empty;
                    pbImportText.Text = string.Empty;
                    pbImportText.Foreground = Brushes.Black;
                    btnBrowse.IsEnabled = false;
                    btnImport.Content = "Import";
                });

                _workbook = null;
                _workSheet = null;
                _fileError = false;

                string filePathAndName = ShowOpenFileDialog(ImportFileExtention, ImportFilterExpression);
                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)(() => medFile.Text = filePathAndName));

                if (filePathAndName != string.Empty)
                {
                    string fileName = System.IO.Path.GetFileName(filePathAndName);

                    #region Check if filename is in correct format

                    if (fileName != null && !Regex.IsMatch(fileName, @"^UDM Female IG Claims .*.xlsx?") && _importType == lkpGiftClaimsImportType.Normal)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "The import file name is in the wrong format.\nThe file name should be in the format\n\"UDM Female IG Claims xxxxxxx.\"", "Import File Name", ShowMessageType.Error);
                            btnImport.IsEnabled = false;
                            btnBrowse.IsEnabled = true;
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
                            if (idxFields[strKey[0]].IgnoreIfNA)
                            {
                                idxFields.Remove(strKey[0]);
                                _fileError = false;
                            }
                            else
                            {
                                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                               {
                                   bool result = Convert.ToBoolean(ShowMessageBox(new INMessageBoxWindow2(), "The header field for key \"" + strKey[0] + "\" could not be found.\nSelect \"OK\" to ignore the import of this field or click \"Cancel\" to stop the import.", "Import File Alert", ShowMessageType.Information));

                                   if (result)
                                   {
                                       idxFields.Remove(strKey[0]);
                                       _fileError = false;
                                   }
                                   else
                                   {
                                       btnImport.IsEnabled = false;
                                       btnBrowse.IsEnabled = true;
                                       _fileError = true;
                                   }
                               });
                            }
                        };

                        List<string> lstHeaders = new List<string>
                        {
                            idxFields.ElementAt(0).Value.Header
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
                                    idxFields.ElementAt(index).Value.Header
                                };

                                wsCell = lstHeaders.Select(str => Methods.WorksheetFindText(_workSheet, str, _headerRow, 0, _headerRow, 1024, false, true)).FirstOrDefault(cell => cell != null);
                                if (wsCell != null)
                                {
                                    idxFields[strKey[0]].Index = wsCell.ColumnIndex;
                                }
                                else
                                {
                                    index--;
                                    headerErrorAction();
                                    if (_fileError == true) return;
                                }
                            }
                        }
                        else
                        {
                            headerErrorAction();
                            if (_fileError == true) return;
                        }
                    }

                    #endregion

                    if (fileName != null)
                    {
                        //Get the row count
                        _rowCount = _workSheet.Rows.Select(row => row).Count(row => (row.Index > _headerRow && NoNull(row.Cells[idxFields["RefNo"].Index].Value, string.Empty).ToString() != string.Empty));

                        //Display import information
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            tbTotalLeads.Text = _rowCount.ToString();
                        });
                    }
                }
                else
                {
                    _fileError = null;
                }

            }

            catch (Exception ex)
            {
                _fileError = true;
                HandleException(ex);
            }
        }

        private void OpenWorkBookComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Arrow);
                _TimerOpenWorkBook.Stop();

                if (_fileError.HasValue)
                {
                    if (!(bool)_fileError)
                    {
                        pbImportText.Foreground = Brushes.Green;
                        pbImportText.Text = pbImportText.Text.Substring(0, pbImportText.Text.Length - 1) + ">";

                        //used for initial import of batches only
                        pbImportText.Foreground = Brushes.Black;
                        pbImportText.Text = "0";
                        btnImport.IsEnabled = true;
                    }
                    else
                    {
                        pbImportText.Foreground = Brushes.Red;
                        pbImportText.Text = "Error";
                        btnBrowse.IsEnabled = true;
                        btnClose.IsEnabled = true;
                    }
                }
                else
                {
                    pbImportText.Foreground = Brushes.Black;
                    pbImportText.Text = string.Empty;
                    btnBrowse.IsEnabled = true;
                }
            }

            catch (Exception)
            {
                HandleException((Exception)e.Result);
            }
        }

        private void CancelBatchImport()
        {
            //buttonClose.IsEnabled = true;
            //btnBrowse.IsEnabled = true;

            //ShowMessageBox(new INMessageBoxWindow1(), "An error has occurred.\nThe batch import will now be cancelled.", "Batch Import Error", ShowMessageType.Error);
            
            //Database.CancelTransactions();
        }

        private bool ImportLeads()
        {
            try
            {
                pbImport.Minimum = 0;
                pbImport.Maximum = _rowCount;
                pbImport.Value = 0;
                double[] pbCounter = { 0 };

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
                                strRefNo = Methods.GetStringValue(row.Cells[idxFields["RefNo"].Index]);
                            }

                            if (!string.IsNullOrWhiteSpace(strRefNo) && strRefNo.Length > 6)
                            {
                                string strSQL = "SELECT INGiftRedeem.ID, FKlkpGiftRedeemStatusID FROM INGiftRedeem JOIN INImport ON INGiftRedeem.FKINImportID = INImport.ID WHERE INImport.RefNo = ";
                                strSQL = strSQL + "'" + strRefNo + "'";

                                DataTable dt = Methods.GetTableData(strSQL);
                                if (dt.Rows.Count == 1)
                                {
                                    long giftRedeemID = 0;
                                    long giftRedeemStatusID = 0;
                                    if (dt.Rows[0].ItemArray.Length > 0)
                                    {
                                        giftRedeemID = (long) dt.Rows[0].ItemArray[0];
                                        giftRedeemStatusID = (long)dt.Rows[0].ItemArray[1];
                                    }

                                    //Nicolas Stephenson commented the following out for the new ram import sheets
                                    //Uncomment the following if else statement to use the old import sheets from platinum
                                    //if (giftRedeemID > 0 && giftRedeemStatusID == 1)
                                    //{
                                        INGiftRedeem inGiftRedeem = new INGiftRedeem(giftRedeemID);

                                        inGiftRedeem.PODDate = Methods.GetDateValue(row.Cells[idxFields["PODDate"].Index]);

                                        if (_importType == lkpGiftClaimsImportType.Normal)
                                        {
                                            inGiftRedeem.PODSignature = Methods.GetStringValue(row.Cells[idxFields["PODSignature"].Index]);
                                        }
                                        else if (_importType == lkpGiftClaimsImportType.SMS)
                                        {
                                            inGiftRedeem.PODSignature = Methods.GetStringValue(row.Cells[idxFields["PODSignature"].Index]) +
                                                " : " + 
                                                Methods.GetStringValue(row.Cells[idxFields["PODTelNumber"].Index]);
                                        }

                                        inGiftRedeem.Save(_validationResult);
                                    //}
                                    //else
                                    //{
                                    //    pbImport.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                                    //    {
                                    //        pbImportText.Foreground = Brushes.Red;
                                    //        pbImportText.Text = "error ... gift not redeemed";
                                    //        btnBrowse.IsEnabled = true;
                                    //        btnClose.IsEnabled = true;
                                    //        btnImport.IsEnabled = false;
                                    //    }));

                                    //    _TimerImport.Stop();
                                    //    return false;
                                    //}

                                }
                                else
                                {
                                    continue;
                                    //Nicolas Stephenson commented the following out for the new Ram Import sheets.
                                    //Uncomment the following to import the old sheets from platinum.
                                    //pbImport.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                                    //{
                                    //    pbImportText.Foreground = Brushes.Red;
                                    //    pbImportText.Text = "Error ... redeemed ID not found";
                                    //    btnBrowse.IsEnabled = true;
                                    //    btnClose.IsEnabled = true;
                                    //    btnImport.IsEnabled = false;
                                    //}));

                                    //_TimerImport.Stop();
                                    //return false;
                                }

                                //update the progressbar
                                pbCounter[0] += 1;

                                pbImport.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                                {
                                    pbImport.Value = pbCounter[0];
                                    pbImportText.Text = pbCounter[0].ToString();
                                }));
                            }
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
                HandleException(ex);
                _TimerImport.Stop();
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                {
                    btnClose.IsEnabled = true;
                }));
                return false;
            }
        }

        #endregion



        #region Event Handlers

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += OpenWorkBook;
                worker.RunWorkerCompleted += OpenWorkBookComplete;
                worker.RunWorkerAsync(worker);
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //prepare for import
                btnClose.IsEnabled = false;
                btnImport.IsEnabled = false;
                btnBrowse.IsEnabled = false;

                Database.BeginTransaction(null, IsolationLevel.Snapshot);

                //import
                bool result = ImportLeads();
                if (!result) { CancelBatchImport(); return; }

                //import completed
                CommitTransaction(null);
                btnClose.IsEnabled = true;
                btnBrowse.IsEnabled = true;

                double totalLeads = double.Parse(tbTotalLeads.Text);
                //Nicolas Stephenson added the following and commented out the ShowMessageBox line right after this if else statement.
                //Ram Import Sheet (New)
                if (pbImport.Value < totalLeads)
                {
                    double importDiff = totalLeads - pbImport.Value;
                    ShowMessageBox(new INMessageBoxWindow1(), "IG gifts claims import completed.\r\n" + importDiff + " leads were not imported because they were not found on the system.", "Batch Import", ShowMessageType.Information);
                }
                else
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "IG gifts claims import completed.", "Batch Import", ShowMessageType.Information);
                }

                //Platinum Import Sheet (Old)
                //ShowMessageBox(new INMessageBoxWindow1(), "IG gifts claims import completed.", "Batch Import", ShowMessageType.Information);

            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        
        private void xamEditor_GotFocus(object sender, RoutedEventArgs e)
        {
            xamEditor_Select(sender);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
           OnDialogClose(false);
        }

        private void btnBrowse_Loaded(object sender, RoutedEventArgs e)
        {
            btnBrowse.Focus();
        }
        
        private void medFile_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox tb = FindChild<TextBox>(medFile, "PART_InputTextBox");
                tb.IsTabStop = false;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ImportProgressControl_Loaded(object sender, RoutedEventArgs e)
        {
            switch (_importType)
            {
                case lkpGiftClaimsImportType.Normal:
                    headerImportProgress.Text = "IG Gift Claims Import";
                    break;

                case lkpGiftClaimsImportType.SMS:
                    headerImportProgress.Text = "IG Gift Claims (SMS) Import";
                    break;
            }
        }

        #endregion

        
    }

}

