using Embriant.Framework;
using Embriant.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
    public partial class ReportDCAnalysis
    {

        #region Constants

        DataSet dsDebiCheckSpecialistLogsData;

        #endregion Constants

        #region Private Members
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;
        private DateTime _startDate;
        private DateTime _endDate;

        string BaseUpgradeBool = "1";
        #endregion

        public ReportDCAnalysis()
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
                        dsDebiCheckSpecialistLogsData = Business.Insure.INGetDCAnalysis(_startDate, enddate, BaseUpgradeBool);
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


                dtSalesData = dsDebiCheckSpecialistLogsData.Tables[0];

                int countForNonRedeemed = 0;

                #endregion Get the report data

                try
                {
                    string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    string filePathAndName = "";

                    filePathAndName = String.Format("{0}DC Analysis Report, {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
                    

                    if (dtSalesData == null || dtSalesData.Columns.Count == 0)
                        throw new Exception("ExportToExcel: Null or empty input table!\n");

                    // load excel, and create a new workbook
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Workbooks.Add();


                    // single worksheet
                    Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;
                    

                    workSheet.Name = "DC Analysis";
                    

                    workSheet.Cells[1, 0 + 1] = "Date Range : " + _startDate.ToShortDateString() + " to " + _endDate.ToShortDateString();
                    workSheet.Cells[1, 0 + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    workSheet.Cells[2, 0 + 2] = "Accepted rates";
                    workSheet.Cells[2, 0 + 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    workSheet.Cells[2, 0 + 5] = "Transfer Stats";
                    workSheet.Cells[2, 0 + 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    workSheet.Cells[2, 0 + 8] = "Over Transferred";
                    workSheet.Cells[2, 0 + 8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;




                    for (var i = 0; i < dtSalesData.Columns.Count; i++)
                    {

                        workSheet.Cells[3, i + 1].Font.Bold = true;
                        workSheet.Cells[3, i + 1].ColumnWidth = 20;
                        workSheet.Cells[3, i + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        //workSheet.get_Range("A4", "J1").Font.Bold = true;
                    }


                    // column headings
                    for (var i = 0; i < dtSalesData.Columns.Count; i++)
                    {
                        workSheet.Cells[3, i + 1] = dtSalesData.Columns[i].ColumnName;
                    }

                    // rows
                    for (var i = 1; i < dtSalesData.Rows.Count + 1; i++)
                    {
                        // to do: format datetime values before printing
                        for (var j = 0; j < dtSalesData.Columns.Count; j++)
                        {
                            workSheet.Cells[i + 3, j + 1] = dtSalesData.Rows[i - 1][j];
                        }

                        countForNonRedeemed = countForNonRedeemed + 1;
                    }

                    //(workSheet.Cells[3, 3]).EntireColumn.NumberFormat = "MM/DD/YYYY hh:mm:ss";

                    #region Totals
                    workSheet.Cells[countForNonRedeemed + 4, 2].Formula = string.Format("=SUM(B4:B" + (countForNonRedeemed + 3).ToString() + ")"); //B
                    workSheet.Cells[countForNonRedeemed + 4, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[countForNonRedeemed + 4, 3].Formula = string.Format("=SUM(C4:C" + (countForNonRedeemed + 3).ToString() + ")"); //C
                    workSheet.Cells[countForNonRedeemed + 4, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[countForNonRedeemed + 4, 5].Formula = string.Format("=SUM(E4:E" + (countForNonRedeemed + 3).ToString() + ")"); //E
                    workSheet.Cells[countForNonRedeemed + 4, 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[countForNonRedeemed + 4, 6].Formula = string.Format("=SUM(F4:F" + (countForNonRedeemed + 3).ToString() + ")"); //F
                    workSheet.Cells[countForNonRedeemed + 4, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[countForNonRedeemed + 4, 8].Formula = string.Format("=SUM(H4:H" + (countForNonRedeemed + 3).ToString() + ")"); //H
                    workSheet.Cells[countForNonRedeemed + 4, 8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[countForNonRedeemed + 4, 9].Formula = string.Format("=SUM(I4:I" + (countForNonRedeemed + 3).ToString() + ")"); //I
                    workSheet.Cells[countForNonRedeemed + 4, 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[countForNonRedeemed + 4, 11].Formula = string.Format("=SUM(K4:K" + (countForNonRedeemed + 3).ToString() + ")"); //K
                    workSheet.Cells[countForNonRedeemed + 4, 11].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[countForNonRedeemed + 4, 12].Formula = string.Format("=SUM(L4:L" + (countForNonRedeemed + 3).ToString() + ")"); //L
                    workSheet.Cells[countForNonRedeemed + 4, 12].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[countForNonRedeemed + 4, 15].Formula = string.Format("=SUM(O4:O" + (countForNonRedeemed + 3).ToString() + ")"); //O
                    workSheet.Cells[countForNonRedeemed + 4, 15].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                    workSheet.Cells[countForNonRedeemed + 4, 4].Formula = string.Format("=C" + (countForNonRedeemed + 4).ToString() + "/B" + (countForNonRedeemed + 4).ToString()); //D
                    workSheet.Cells[countForNonRedeemed + 4, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[countForNonRedeemed + 4, 7].Formula = string.Format("=F" + (countForNonRedeemed + 4).ToString() + "/E" + (countForNonRedeemed + 4).ToString()); //G
                    workSheet.Cells[countForNonRedeemed + 4, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    workSheet.Cells[countForNonRedeemed + 4, 10].Formula = string.Format("=I" + (countForNonRedeemed + 4).ToString() + "/H" + (countForNonRedeemed + 4).ToString()); //J
                    workSheet.Cells[countForNonRedeemed + 4, 10].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    #endregion

                    #region Column formulas
                    for (int w = 4; w <= countForNonRedeemed + 4; w++)
                    {
                        workSheet.Cells[w, 13].Formula = string.Format("=L" + w + "/C" + w);
                        workSheet.Cells[w, 14].Formula = string.Format("=L" + w + "/K" + w);
                    }
                    #endregion

                    #region worksheet formatting
                    //background shades
                    workSheet.Range["B3", "D3"].Interior.Color = System.Drawing.Color.LightGreen;
                    workSheet.Range["E3", "G3"].Interior.Color = System.Drawing.Color.LightPink;
                    workSheet.Range["H3", "J3"].Interior.Color = System.Drawing.Color.LightYellow;
                    workSheet.Range["K3", "O3"].Interior.Color = System.Drawing.Color.LightBlue;

                    workSheet.Range["B2", "B2"].Interior.Color = System.Drawing.Color.LightGreen;
                    workSheet.Range["E2", "E2"].Interior.Color = System.Drawing.Color.LightPink;
                    workSheet.Range["H2", "H2"].Interior.Color = System.Drawing.Color.LightYellow;


                    //Row Formats
                    (workSheet.Cells[1, 4]).EntireColumn.NumberFormat = "##.##%";
                    (workSheet.Cells[1, 7]).EntireColumn.NumberFormat = "##.##%";
                    (workSheet.Cells[1, 10]).EntireColumn.NumberFormat = "##.##%";
                    (workSheet.Cells[1, 13]).EntireColumn.NumberFormat = "##.##%";
                    (workSheet.Cells[1, 14]).EntireColumn.NumberFormat = "##.##%";

                    //row height
                    (workSheet.Rows[3]).EntireRow.RowHeight = 40;

                    //grid lines
                    Microsoft.Office.Interop.Excel.Range tRange = workSheet.UsedRange;
                    tRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    tRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                    //merge columns 
                    workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 15]].Merge();
                    workSheet.Range[workSheet.Cells[2, 2], workSheet.Cells[2, 4]].Merge();
                    workSheet.Range[workSheet.Cells[2, 5], workSheet.Cells[2, 7]].Merge();
                    workSheet.Range[workSheet.Cells[2, 8], workSheet.Cells[2, 10]].Merge();
                    workSheet.Range[workSheet.Cells[2, 11], workSheet.Cells[2, 15]].Merge();


                    #endregion



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

        private void UpgradeCB_Checked(object sender, RoutedEventArgs e)
        {
            BaseCB.IsChecked = false;
            BaseUpgradeBool = "1";
        }

        private void BaseCB_Checked(object sender, RoutedEventArgs e)
        {
            UpgradeCB.IsChecked = false;
            BaseUpgradeBool = "0";
        }


    }
}
