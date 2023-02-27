using System.Data.SqlClient;
using System.Windows.Resources;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using Orientation = Infragistics.Documents.Excel.Orientation;
using System.Linq;
using System.Collections.Generic;
using static UDM.Insurance.Interface.PrismViews.EditClosureScreenViewModel;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportDiaryScreen.xaml
    /// </summary>
    public partial class ReportDebiCheckError
    {

        #region Constants


        #endregion Constants

        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        //private RecordCollectionBase _campaigns;
        //private System.Collections.Generic.List<Record> _campaigns;
        private List<Record> _lstSelectedCampaigns;
        private int? _campaignIDList = null;
        string IsUpgrade = "0";
        private DateTime _startDate;
        private DateTime _endDate;
        DataTable dtSalesData = new DataTable();
        DataTable dtSalesDataSummary = new DataTable();

        List<int> UserIDs = new List<int>();
        private bool _isDefault = true;
        private bool _isSummary = false;


        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        #endregion Private Members

        #region Constructors

        public ReportDebiCheckError() 
        {
            InitializeComponent();

            
            dtSalesData.Columns.Add("RefNo");
            dtSalesData.Columns.Add("Code");
            dtSalesData.Columns.Add("Mandate Status");
            dtSalesData.Columns.Add("Bank Branch");
            dtSalesData.Columns.Add("Transfer Reason");

            
            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);

            //DefaultCB.IsChecked = true;

        }

        #endregion Constructors

        #region Private Methods




        private void EnableDisableExportButton()
        {
            try
            {
                if ((calStartDate.SelectedDate != null && (calEndDate.SelectedDate != null))) //&& (calEndDate.SelectedDate >= Cal1.SelectedDate)
                {
                        btnReport.IsEnabled = true;
                        return;
                }

                btnReport.IsEnabled = false;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";

            btnReport.IsEnabled = true;
            btnClose.IsEnabled = true;
            calStartDate.IsEnabled = true;
            calEndDate.IsEnabled = true;
        }

        private bool IsAllInputParametersSpecifiedAndValid()
        {

            #region Ensuring that the From Date was specified

            if (calStartDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'From Date'.", @"No 'From Date' specified", ShowMessageType.Error);
                return false;
            }
            else
            {
                _startDate = calStartDate.SelectedDate.Value;
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the From Date was specified

            if (calEndDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'To Date'.", @"No 'To Date' specified", ShowMessageType.Error);
                return false;
            }

            else
            {
                _endDate = calEndDate.SelectedDate.Value;
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the date range is valid

            if (calStartDate.SelectedDate > calEndDate.SelectedDate.Value)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Invalid date range specified: The 'From Date' can not be greater than the 'To Date'.", "Invalid date range", ShowMessageType.Error);
                return false;
            }

            #endregion Ensuring that the date range is valid

            // Otherwise, if all is well, proceed:
            return true;
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                List<DateTime> datesSelectedList = new List<DateTime>();

                DateTime startdate = _startDate;
                datesSelectedList.Add(startdate);

                for (int x = 0; startdate < _endDate; x++)
                {
                    startdate = startdate.AddDays(1);
                    datesSelectedList.Add(startdate);
                }
                startdate = startdate.AddDays(1);
                _endDate = _endDate.AddHours(23);
                try { UserIDs.Clear(); } catch { }

                //DataSet mandateData ;
                DataTable mandateData = new DataTable();

                mandateData = Business.Insure.INDebiCheckErrorMandate(_startDate, _endDate);
                //DataTable dtAgentIDs = dsAgentIDs.Tables[0];
                                             
                try { dtSalesData.Clear(); } catch { }
                //DataTable dtSalesData = new DataTable();
                
                #endregion Get the report data

                try
                {
                    string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


                    string filePathAndName = String.Format("{0}DebiCheck Error Report{1}, {2}.xlsx", GlobalSettings.UserFolder, "", DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
                    if (dtSalesData == null || dtSalesData.Columns.Count == 0)
                        throw new Exception("ExportToExcel: Null or empty input table!\n");

                    // load excel, and create a new workbook
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Workbooks.Add();

                    // single worksheet
                    Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;

                    workSheet.Cells[1, 0 + 1] = "Debi-Check Error Mandate Report" ;
                    workSheet.Cells[1, 0 + 1].Font.Bold = true;  //_startDate.ToShortDateString()  //.ToShortDateString()
                    workSheet.Cells[2, 0 + 1] = "Date Range : " + _startDate.ToString() + " to " + _endDate.ToString();
                    workSheet.Cells[2, 0 + 1].Font.Bold = true;

                    for (var i = 0; i < dtSalesData.Columns.Count; i++)
                    {
                        workSheet.Cells[4, i + 1].Font.Bold = true;
                        workSheet.Cells[4, i + 1].ColumnWidth = 40;
                        workSheet.Cells[4, i + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        //workSheet.get_Range("A4", "J1").Font.Bold = true;
                    }

                    workSheet.Cells[4, 27].ColumnWidth = 40;

                    // column headings
                    for (var i = 0; i < dtSalesData.Columns.Count; i++)
                    {
                        workSheet.Cells[4, i + 1] = dtSalesData.Columns[i].ColumnName;
                    }

                    // rows
                    for (var i = 1; i < mandateData.Rows.Count + 1; i++)
                    {
                        // to do: format datetime values before printing
                        for (var j = 0; j < mandateData.Columns.Count; j++)
                        {
                            //if (j == 4 && (mandateData.Rows[i - 1][j] == ""))
                            //{
                            //    workSheet.Cells[i + 4, j + 1] = "NULL";
                            //}
                            //else
                            if (mandateData.Rows[i - 1][j].ToString() == "")
                            {
                                workSheet.Cells[i + 4, j + 1] = "NULL";
                            }
                            else
                            { 
                                workSheet.Cells[i + 4, j + 1] = mandateData.Rows[i - 1][j];
                            }
                        }
                    }
                    
                    int totalrows = mandateData.Rows.Count + 3; //dtSalesData                  

                    //workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 5]].Merge();

                    Microsoft.Office.Interop.Excel.Range tRange = workSheet.UsedRange;
                    tRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    tRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                    // check file path

                    workSheet.get_Range("A4", "E4").BorderAround( // these apply borders on the report
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    int totalRowMinusOne = totalrows - 1;
                   
                    excelApp.Visible = true;
                    excelApp.Workbooks.Item[1].SaveAs(filePathAndName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    //excelApp.Save(filePathAndName);
                    ////Process.Start(filePathAndName);

                    //excelApp.Workbooks.

                }
                catch (Exception ex)
                {

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


        #region Totals for Grid 1


        #endregion

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;
            btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnReport.ToolTip = btnReport.Content;
        }

        //#endregion Private Methods

        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (IsAllInputParametersSpecifiedAndValid())
                {
                    btnClose.IsEnabled = false;
                    btnReport.IsEnabled = false;
                    calStartDate.IsEnabled = false;
                    calEndDate.IsEnabled = false;

                    //Cal2.IsEnabled = false;

                    BackgroundWorker worker = new BackgroundWorker();
                    if(_isDefault == true)
                    {
                        worker.DoWork += Report;
                    }
                    else
                    {
                        //worker.DoWork += ReportSummary;
                    }
                    worker.RunWorkerCompleted += ReportCompleted;
                    worker.RunWorkerAsync();

                    dispatcherTimer1.Start();
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void Cal1_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calStartDate.SelectedDate.ToString(), out _startDate);
            EnableDisableExportButton();
        }

        private void Cal2_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calEndDate.SelectedDate.ToString(), out _endDate);
            EnableDisableExportButton();
        }

        private void UpgradeCB_Checked(object sender, RoutedEventArgs e)
        {
            //BaseCB.IsChecked = false;
            IsUpgrade = "1";
        }

        private void BaseCB_Checked(object sender, RoutedEventArgs e)
        {
            //UpgradeCB.IsChecked = false;
            IsUpgrade = "0";
        }



        #endregion

        private void DefaultCB_Checked(object sender, RoutedEventArgs e)
        {
            _isDefault = true;
            _isSummary = false;
           // SummaryCB.IsChecked = false;
        }

        private void SummaryCB_Checked(object sender, RoutedEventArgs e)
        {
            _isDefault = false;
            _isSummary = true;
            //DefaultCB.IsChecked = false;
        }
    }
}
