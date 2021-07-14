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

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportDiaryScreen.xaml
    /// </summary>
    public partial class ReportDebiCheckTracker
    {

        #region Constants
        DataSet dsDiaryReportData;


        #endregion Constants

        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        //private RecordCollectionBase _campaigns;
        //private System.Collections.Generic.List<Record> _campaigns;
        private List<Record> _lstSelectedCampaigns;
        private int? _campaignIDList = null;

        private DateTime _startDate;
        private DateTime _endDate;

        private bool CancerBaseBool;
        private bool MaccBaseBool;
        string campaign = "";


        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        #endregion Private Members

        #region Constructors

        public ReportDebiCheckTracker()
        {
            InitializeComponent();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
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

        private void ReportConsolidated(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Get the report data
                DataTable dtSalesData;

                dtSalesData = dsDiaryReportData.Tables[0];


                #endregion Get the report data

                try
                {
                    string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


                    string filePathAndName = String.Format("{0}DebiCheck Tracking Report ({1}), {2}.xlsx", GlobalSettings.UserFolder, campaign, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
                    if (dtSalesData == null || dtSalesData.Columns.Count == 0)
                        throw new Exception("ExportToExcel: Null or empty input table!\n");

                    // load excel, and create a new workbook
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Workbooks.Add();

                    // single worksheet
                    Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;

                    workSheet.Cells[1, 0 + 1] = "Date Range : " + _endDate.ToShortDateString() + " to " + _startDate.ToShortDateString();
                    for (var i = 0; i < dtSalesData.Columns.Count; i++)
                    {

                        workSheet.Cells[2, i + 1].Font.Bold = true;

                            workSheet.Cells[2, i + 1].ColumnWidth = 10;

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
                        }
                    }

                    var totalTable = dsDiaryReportData.Tables[1];

                    workSheet.Cells[24, 2].Value = int.Parse(totalTable.Rows[0][0].ToString()) - 1;
                    workSheet.Cells[24, 1].Value = "Total :";

                    (workSheet.Cells[1, 6]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 8]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 10]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 12]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 14]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 16]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 18]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 20]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 22]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 24]).EntireColumn.NumberFormat = "00,00%";

                    workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 24]].Merge();

                    workSheet.Rows[2].WrapText = true;

                    Microsoft.Office.Interop.Excel.Range tRange = workSheet.UsedRange;
                    tRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    tRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                    workSheet.get_Range("A2", "X2").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "D24").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "F24").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "J24").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "V24").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "X24").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A24", "X24").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.Range["F3", "F24"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["H3", "H24"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["J3", "J24"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["L3", "L24"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["N3", "N24"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["P3", "P24"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["R3", "R24"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["T3", "T24"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["V3", "V24"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["X3", "X24"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;


                    #region Page formulas

                    workSheet.Cells[3, 3].Formula = string.Format("=U3+Q3+S3+O3+W3");
                    workSheet.Cells[4, 3].Formula = string.Format("=U4+Q4+S4+O4+W4");
                    workSheet.Cells[5, 3].Formula = string.Format("=U5+Q5+S5+O5+W5");
                    workSheet.Cells[6, 3].Formula = string.Format("=U6+Q6+S6+O6+W6");
                    workSheet.Cells[7, 3].Formula = string.Format("=U7+Q7+S7+O7+W7");
                    workSheet.Cells[8, 3].Formula = string.Format("=U8+Q8+S8+O8+W8");
                    workSheet.Cells[9, 3].Formula = string.Format("=U9+Q9+S9+O9+W9");
                    workSheet.Cells[10, 3].Formula = string.Format("=U10+Q10+S10+O10+W10");
                    workSheet.Cells[11, 3].Formula = string.Format("=U11+Q11+S11+O11+W11");
                    workSheet.Cells[12, 3].Formula = string.Format("=U12+Q12+S12+O12+W12");
                    workSheet.Cells[13, 3].Formula = string.Format("=U13+Q13+S13+O13+W13");
                    workSheet.Cells[14, 3].Formula = string.Format("=U14+Q14+S14+O14+W14");
                    workSheet.Cells[15, 3].Formula = string.Format("=U15+Q15+S15+O15+W15");
                    workSheet.Cells[16, 3].Formula = string.Format("=U16+Q16+S16+O16+W16");
                    workSheet.Cells[17, 3].Formula = string.Format("=U17+Q17+S17+O17+W17");
                    workSheet.Cells[18, 3].Formula = string.Format("=U18+Q18+S18+O18+W18");
                    workSheet.Cells[19, 3].Formula = string.Format("=U19+Q19+S19+O19+W19");
                    workSheet.Cells[20, 3].Formula = string.Format("=U20+Q20+S20+O20+W20");
                    workSheet.Cells[21, 3].Formula = string.Format("=U21+Q21+S21+O21+W21");
                    workSheet.Cells[22, 3].Formula = string.Format("=U22+Q22+S22+O22+W22");
                    workSheet.Cells[23, 3].Formula = string.Format("=U23+Q23+S23+O23+W23");

                    workSheet.Cells[3, 2].Formula = string.Format("=D3+C3");
                    workSheet.Cells[4, 2].Formula = string.Format("=D4+C4");
                    workSheet.Cells[5, 2].Formula = string.Format("=D5+C5");
                    workSheet.Cells[6, 2].Formula = string.Format("=D6+C6");
                    workSheet.Cells[7, 2].Formula = string.Format("=D7+C7");
                    workSheet.Cells[8, 2].Formula = string.Format("=D8+C8");
                    workSheet.Cells[9, 2].Formula = string.Format("=D9+C9");
                    workSheet.Cells[10, 2].Formula = string.Format("=D10+C10");
                    workSheet.Cells[11, 2].Formula = string.Format("=D11+C11");
                    workSheet.Cells[12, 2].Formula = string.Format("=D12+C12");
                    workSheet.Cells[13, 2].Formula = string.Format("=D13+C13");
                    workSheet.Cells[14, 2].Formula = string.Format("=D14+C14");
                    workSheet.Cells[15, 2].Formula = string.Format("=D15+C15");
                    workSheet.Cells[16, 2].Formula = string.Format("=D16+C16");
                    workSheet.Cells[17, 2].Formula = string.Format("=D17+C17");
                    workSheet.Cells[18, 2].Formula = string.Format("=D18+C18");
                    workSheet.Cells[19, 2].Formula = string.Format("=D19+C19");
                    workSheet.Cells[20, 2].Formula = string.Format("=D20+C20");
                    workSheet.Cells[21, 2].Formula = string.Format("=D21+C21");
                    workSheet.Cells[22, 2].Formula = string.Format("=D22+C22");
                    workSheet.Cells[23, 2].Formula = string.Format("=D23+C23");

                    workSheet.Cells[24, 2].Formula = string.Format("=SUM(B1:B23)"); //B
                    workSheet.Cells[24, 3].Formula = string.Format("=SUM(C1:C23)"); //C
                    workSheet.Cells[24, 4].Formula = string.Format("=SUM(D1:D23)"); //D
                    workSheet.Cells[24, 5].Formula = string.Format("=SUM(E1:E23)"); //E
                    workSheet.Cells[24, 7].Formula = string.Format("=SUM(G1:G23)"); //G
                    workSheet.Cells[24, 9].Formula = string.Format("=SUM(I1:I23)"); //I
                    workSheet.Cells[24, 11].Formula = string.Format("=SUM(K1:K23)"); //K
                    workSheet.Cells[24, 13].Formula = string.Format("=SUM(M1:M23)"); //M
                    workSheet.Cells[24, 15].Formula = string.Format("=SUM(O1:O23)"); //O
                    workSheet.Cells[24, 17].Formula = string.Format("=SUM(Q1:Q23)"); //Q
                    workSheet.Cells[24, 19].Formula = string.Format("=SUM(S1:S23)"); //S
                    workSheet.Cells[24, 21].Formula = string.Format("=SUM(U1:U23)"); //U
                    workSheet.Cells[24, 23].Formula = string.Format("=SUM(W1:W23)"); //W

                    workSheet.Cells[24, 6].Formula = string.Format("=E24/B24*100"); //F
                    workSheet.Cells[24, 8].Formula = string.Format("=G24/B24*100"); //H
                    workSheet.Cells[24, 10].Formula = string.Format("=I24/B24*100"); //J
                    workSheet.Cells[24, 12].Formula = string.Format("=K24/B24*100"); //L
                    workSheet.Cells[24, 14].Formula = string.Format("=M24/B24*100"); //N
                    workSheet.Cells[24, 16].Formula = string.Format("=O24/B24*100"); //P
                    workSheet.Cells[24, 18].Formula = string.Format("=Q24/B24*100"); //R
                    workSheet.Cells[24, 20].Formula = string.Format("=S24/B24*100"); //T
                    workSheet.Cells[24, 22].Formula = string.Format("=U24/B24*100"); //V
                    workSheet.Cells[24, 24].Formula = string.Format("=W24/B24*100"); //X

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


        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;
            btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnReport.ToolTip = btnReport.Content;
        }

        #endregion Private Methods

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
                    calStartDate.IsEnabled = false;
                    calEndDate.IsEnabled = false;


                    DataTable dtSalesData = null;

                    TimeSpan ts = new TimeSpan(23, 00, 0);
                    DateTime _endDate2 = DateTime.Now;
                    _endDate2 = _endDate.Date + ts;

                    TimeSpan ts1 = new TimeSpan(00, 00, 0);
                    DateTime _startDat2 = DateTime.Now;
                   _startDat2 = _startDate.Date + ts1;

                    dsDiaryReportData = Business.Insure.INGetDebiCheckTracking(_startDat2, _endDate2);


                    BackgroundWorker worker = new BackgroundWorker();

                    worker.DoWork += ReportConsolidated;

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




        private void HeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _xdgHeaderPrefixAreaCheckbox = (CheckBox)sender;
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

        #endregion






    }
}
