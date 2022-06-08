using Embriant.Framework;
using Embriant.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UDM.Insurance.Interface.Windows;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class ReportResalesAcquiredAccountDetails
    {

        #region Constants

        DataSet dsResalesAcquiredAccountReportData;

        #endregion Constants

        #region Private Members
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;
        private DateTime _startDate;
        private DateTime _endDate;
        DataTable dtDataSheet;
        DataTable dtAllDataSheet;

        #endregion

        public ReportResalesAcquiredAccountDetails()
        {
            InitializeComponent();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }
        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;
            btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnReport.ToolTip = btnReport.Content;
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

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (IsAllInputParametersSpecifiedAndValid())
                {
                    btnClose.IsEnabled = false;
                    //btnReport.IsEnabled = false;
                    calStartDate.IsEnabled = false;
                    calEndDate.IsEnabled = false;

                    //Cal2.IsEnabled = false;

                    TimeSpan ts11 = new TimeSpan(23, 00, 0);
                    DateTime enddate = _endDate.Date + ts11;

                    var transactionOptions = new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                    };

                    using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                    {
                        dsResalesAcquiredAccountReportData = Business.Insure.INGetResalesAqcuiredAccountDetails(_startDate, enddate);
                    }

                    BackgroundWorker worker = new BackgroundWorker();
                    
                    worker.DoWork += Report;
                    worker.RunWorkerCompleted += ReportCompleted;
                    worker.RunWorkerAsync();

                    dispatcherTimer1.Start();
                }
            }

            catch (Exception ex)
            {
                //HandleException(ex);

                btnReport.IsEnabled = true;
                btnClose.IsEnabled = true;
                calStartDate.IsEnabled = true;
                calEndDate.IsEnabled = true;
            }
        }


        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Get the report data
                DataTable dtSalesData;
                try { dtDataSheet.Clear(); } catch { }
                try { dtAllDataSheet.Clear(); } catch { }

                dtSalesData = dsResalesAcquiredAccountReportData.Tables[0];
                dtDataSheet = dsResalesAcquiredAccountReportData.Tables[1];
                dtAllDataSheet = dsResalesAcquiredAccountReportData.Tables[2];
                #endregion Get the report data

                try
                {
                    string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);



                    string filePathAndName = String.Format("{0}Resales Account Aqcuired Details Report, {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
                    if (dtSalesData == null || dtSalesData.Columns.Count == 0)
                        throw new Exception("ExportToExcel: Null or empty input table!\n");

                    // load excel, and create a new workbook
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Workbooks.Add();


                    // single worksheet
                    Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;
                    workSheet.Name = "PL Acquired Details"; 

                    workSheet.Cells[1, 0 + 1] = "Date Range : " + _startDate.ToShortDateString() + " to " + _endDate.ToShortDateString();
                    for (var i = 0; i < dtSalesData.Columns.Count; i++)
                    {

                        workSheet.Cells[2, i + 1].Font.Bold = true;
                        workSheet.Cells[2, i + 1].ColumnWidth = 30;
                        workSheet.Cells[2, i + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        //workSheet.get_Range("A4", "J1").Font.Bold = true;
                    }


                    // column headings
                    for (var i = 0; i < dtSalesData.Columns.Count; i++)
                    {
                        workSheet.Cells[2, i + 1] = dtSalesData.Columns[i].ColumnName;
                    }

                    // rows
                    for (var i = 1; i < dtSalesData.Rows.Count + 1; i++)
                    {
                        // to do: format datetime values before printing
                        for (var j = 0; j < dtSalesData.Columns.Count; j++)
                        {
                            workSheet.Cells[i + 2, j + 1] = dtSalesData.Rows[i - 1][j];
                            workSheet.Cells[i + 2, j + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                        }
                    }



                    AddDataSheet(excelApp);
                    AddAllDataSheet(excelApp);

                    // check file path
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

        private void AddDataSheet(Microsoft.Office.Interop.Excel.Application excelApp)
        {
            //this is the All Data Sheet

            #region Get the report data


            #endregion Get the report data



            int countForNonRedeemed = 0;
            Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.Worksheets.Add();
            workSheet.Name = "UDM Acquired Details";
            workSheet.Cells[1, 0 + 1] = "Date Range : " + _startDate.ToShortDateString() + " to " + _endDate.ToShortDateString();
            for (var i = 0; i < dtDataSheet.Columns.Count; i++)
            {

                workSheet.Cells[2, i + 1].Font.Bold = true;
                if (i == 0)
                {
                    workSheet.Cells[2, i + 1].ColumnWidth = 30;
                }
                else
                {
                    workSheet.Cells[2, i + 1].ColumnWidth = 30;
                }

                workSheet.Cells[2, i + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                //workSheet.get_Range("A4", "J1").Font.Bold = true;
            }


            // column headings
            for (var i = 0; i < dtDataSheet.Columns.Count; i++)
            {

                workSheet.Cells[2, i + 1] = dtDataSheet.Columns[i].ColumnName;
            }

            // rows
            for (var i = 1; i < dtDataSheet.Rows.Count + 1; i++)
            {
                // to do: format datetime values before printing
                for (var j = 0; j < dtDataSheet.Columns.Count; j++)
                {
                    workSheet.Cells[i + 2, j + 1] = dtDataSheet.Rows[i - 1][j];
                    workSheet.Cells[i + 2, j + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                }

                countForNonRedeemed = countForNonRedeemed + 1;
            }



            #region Totals for Grid 1

            //workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 7]].Merge();
            workSheet.Cells[1, 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            #endregion

            countForNonRedeemed = countForNonRedeemed + 3;
            int CountSecondGridTotals = countForNonRedeemed;
        }

        private void AddAllDataSheet(Microsoft.Office.Interop.Excel.Application excelApp)
        {
            //this is the All Data Sheet

            #region Get the report data


            #endregion Get the report data



            int countForNonRedeemed = 0;
            Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.Worksheets.Add();
            workSheet.Name = "Combined Data";
            workSheet.Cells[1, 0 + 1] = "Date Range : " + _startDate.ToShortDateString() + " to " + _endDate.ToShortDateString();
            for (var i = 0; i < dtAllDataSheet.Columns.Count; i++)
            {

                workSheet.Cells[2, i + 1].Font.Bold = true;
                if (i == 0)
                {
                    workSheet.Cells[2, i + 1].ColumnWidth = 30;
                }
                else
                {
                    workSheet.Cells[2, i + 1].ColumnWidth = 30;
                }

                workSheet.Cells[2, i + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                //workSheet.get_Range("A4", "J1").Font.Bold = true;
            }


            // column headings
            for (var i = 0; i < dtAllDataSheet.Columns.Count; i++)
            {

                workSheet.Cells[2, i + 1] = dtAllDataSheet.Columns[i].ColumnName;
            }

            // rows
            for (var i = 1; i < dtAllDataSheet.Rows.Count + 1; i++)
            {
                // to do: format datetime values before printing
                for (var j = 0; j < dtAllDataSheet.Columns.Count; j++)
                {
                    workSheet.Cells[i + 2, j + 1] = dtAllDataSheet.Rows[i - 1][j];
                    workSheet.Cells[i + 2, j + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                }

                countForNonRedeemed = countForNonRedeemed + 1;
            }



            #region Totals for Grid 1

            //workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 7]].Merge();
            workSheet.Cells[1, 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            #endregion

            countForNonRedeemed = countForNonRedeemed + 3;
            int CountSecondGridTotals = countForNonRedeemed;

        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void EnableDisableExportButton()
        {
            try
            {
                if ((calStartDate.SelectedDate != null && (calEndDate.SelectedDate != null))) //&& (calEndDate.SelectedDate >= Cal1.SelectedDate)
                {
                    btnReport.IsEnabled = true;
                    return;
                }

                //btnReport.IsEnabled = false;
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

        private void calEndDate_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calEndDate.SelectedDate.ToString(), out _endDate);
            EnableDisableExportButton();
        }

        private void calStartDate_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calStartDate.SelectedDate.ToString(), out _startDate);
            EnableDisableExportButton();
        }
    }
}
