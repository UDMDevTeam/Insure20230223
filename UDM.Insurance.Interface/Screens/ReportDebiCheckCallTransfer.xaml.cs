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
    public partial class ReportDebiCheckCallTransfer
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
        List<int> UserIDs = new List<int>();


        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        #endregion Private Members

        #region Constructors

        public ReportDebiCheckCallTransfer()
        {
            InitializeComponent();

            dtSalesData.Columns.Add("TSR Name");
            dtSalesData.Columns.Add("Employment Status");
            dtSalesData.Columns.Add("Supervisor");
            dtSalesData.Columns.Add("Total Sales");
            dtSalesData.Columns.Add("Less Sales where Debi-checks are N/A");
            dtSalesData.Columns.Add("Less DC Agent not available - valid");
            dtSalesData.Columns.Add("Sales to Be Transferred");
            dtSalesData.Columns.Add("Actual Sales Transferred");
            dtSalesData.Columns.Add("% Transferred");
            dtSalesData.Columns.Add("Total Calls Not Transferred");
            dtSalesData.Columns.Add("%  Total calls not transferred");
            dtSalesData.Columns.Add("Debi-Check agent not available");
            dtSalesData.Columns.Add("Overtime Sale");
            dtSalesData.Columns.Add("Difficult client");
            dtSalesData.Columns.Add("Remote shift");
            dtSalesData.Columns.Add("Actual Transferred");
            dtSalesData.Columns.Add("Debi-Check agent not available - Invalid");
            dtSalesData.Columns.Add("Total Sales Hidden");

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

                try { UserIDs.Clear(); } catch { }

                DataSet dsAgentIDs ;

                dsAgentIDs = Business.Insure.INGetReportCallTransferAgents(_startDate, _endDate, IsUpgrade);
                DataTable dtAgentIDs = dsAgentIDs.Tables[0];

                foreach (DataRow item in dtAgentIDs.Rows)
                {
                    UserIDs.Add(int.Parse(item[0].ToString()));
                }

                //#region Added additional upgrades
                //UserIDs.Add(40539);
                //UserIDs.Add(40530);
                //#endregion

                DataSet dsDiaryReportData;
                try { dtSalesData.Clear(); } catch { }
                //DataTable dtSalesData = new DataTable();

                foreach (var item in UserIDs)
                {
                    #region Get the report data
                    try
                    {
                        TimeSpan ts11 = new TimeSpan(23, 00, 0);
                        DateTime enddate = _endDate.Date + ts11;

                        dsDiaryReportData = Business.Insure.INGetReportCallTransfer(item, _startDate, enddate, IsUpgrade);

                        DataTable dtSalesRow = dsDiaryReportData.Tables[0];

                        foreach (DataRow items in dtSalesRow.Rows)
                        {
                            dtSalesData.Rows.Add(items.ItemArray);
                        }
                    }
                    catch
                    {

                    }


                }

                #endregion Get the report data

                int countForNonRedeemed = 0;


                try
                {
                        string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                        string filePathAndName = String.Format("TSRDebiCheckCallTransferStats" + DateTime.Now.ToString() + ".xlsx", UserFolder + " ", " Combined ", DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));

                        if (dtSalesData == null || dtSalesData.Columns.Count == 0)
                            throw new Exception("ExportToExcel: Null or empty input table!\n");

                        // load excel, and create a new workbook
                        var excelApp = new Microsoft.Office.Interop.Excel.Application();
                        excelApp.Workbooks.Add();
                        

                        // single worksheet
                        Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;
                        workSheet.Name = "Summary Page";
                        //    workSheet.Cells[1, 0 + 1] =  "Date Range : " + _endDate.ToShortDateString() + " to " + _startDate.ToShortDateString();
                        for (var i = 0; i < dtSalesData.Columns.Count; i++)
                        {

                            workSheet.Cells[2, i + 1].Font.Bold = true;
                            if (i == 0)
                            {
                                workSheet.Cells[2, i + 1].ColumnWidth = 30;
                            }
                            else
                            {
                                workSheet.Cells[2, i + 1].ColumnWidth = 15;
                            }
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

                        countForNonRedeemed = countForNonRedeemed + 1;

                        }




                    workSheet.Range["A2", "B2"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["C2", "Q2"].Interior.Color = System.Drawing.Color.LightBlue;

                    #region Totals for Grid 1
                    for (int w = 3; w <= countForNonRedeemed + 3; w++)
                    {
                        workSheet.Cells[w, 7].Formula = string.Format("=R" + w + "-E" + w);
                    }

                    workSheet.Cells[countForNonRedeemed + 3, 4].Formula = string.Format("=SUM(D3:D" + (countForNonRedeemed + 2).ToString() + ")"); //E
                    workSheet.Cells[countForNonRedeemed + 3, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Cells[countForNonRedeemed + 3, 5].Formula = string.Format("=SUM(E3:E" + (countForNonRedeemed + 2).ToString() + ")"); //E
                    workSheet.Cells[countForNonRedeemed + 3, 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Cells[countForNonRedeemed + 3, 6].Formula = string.Format("=SUM(F3:F" + (countForNonRedeemed + 2).ToString() + ")"); //F
                    workSheet.Cells[countForNonRedeemed + 3, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Cells[countForNonRedeemed + 3, 7].Formula = string.Format("=SUM(G3:G" + (countForNonRedeemed + 2).ToString() + ")"); //H
                    workSheet.Cells[countForNonRedeemed + 3, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Cells[countForNonRedeemed + 3, 8].Formula = string.Format("=SUM(H3:H" + (countForNonRedeemed + 2).ToString() + ")"); //J
                    workSheet.Cells[countForNonRedeemed + 3, 8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Cells[countForNonRedeemed + 3, 10].Formula = string.Format("=SUM(J3:J" + (countForNonRedeemed + 2).ToString() + ")"); //K
                    workSheet.Cells[countForNonRedeemed + 3, 10].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Cells[countForNonRedeemed + 3, 12].Formula = string.Format("=SUM(L3:L" + (countForNonRedeemed + 2).ToString() + ")"); //L
                    workSheet.Cells[countForNonRedeemed + 3, 12].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Cells[countForNonRedeemed + 3, 13].Formula = string.Format("=SUM(M3:M" + (countForNonRedeemed + 2).ToString() + ")"); //M
                    workSheet.Cells[countForNonRedeemed + 3, 13].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Cells[countForNonRedeemed + 3, 14].Formula = string.Format("=SUM(N3:N" + (countForNonRedeemed + 2).ToString() + ")"); //M
                    workSheet.Cells[countForNonRedeemed + 3, 14].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Cells[countForNonRedeemed + 3, 15].Formula = string.Format("=SUM(O3:O" + (countForNonRedeemed + 2).ToString() + ")"); //D
                    workSheet.Cells[countForNonRedeemed + 3, 15].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Cells[countForNonRedeemed + 3, 16].Formula = string.Format("=SUM(P3:P" + (countForNonRedeemed + 2).ToString() + ")"); //L
                    workSheet.Cells[countForNonRedeemed + 3, 16].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Cells[countForNonRedeemed + 3, 17].Formula = string.Format("=SUM(Q3:Q" + (countForNonRedeemed + 2).ToString() + ")"); //L
                    workSheet.Cells[countForNonRedeemed + 3, 17].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    workSheet.Cells[countForNonRedeemed + 3, 9].Formula = string.Format("=H" + (countForNonRedeemed + 3).ToString() + "/G" + (countForNonRedeemed + 3).ToString()); //M
                    workSheet.Cells[countForNonRedeemed + 3, 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Cells[countForNonRedeemed + 3, 11].Formula = string.Format("=J" + (countForNonRedeemed + 3).ToString() + "/G" + (countForNonRedeemed + 3).ToString()); //M
                    workSheet.Cells[countForNonRedeemed + 3, 11].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;


                    #endregion

                    workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 17]].Merge();
                    workSheet.Cells[1,  1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;


                    (workSheet.Cells[1, 9]).EntireColumn.NumberFormat = "##%";
                    (workSheet.Cells[1, 11]).EntireColumn.NumberFormat = "##%";
                    (workSheet.Cells[1, 15]).EntireColumn.Hidden = true;
                    (workSheet.Cells[1, 14]).EntireColumn.Hidden = true;
                    (workSheet.Cells[1, 18]).EntireColumn.Hidden = true;


                    (workSheet.Rows[2]).EntireRow.RowHeight = 40;
                    workSheet.Rows[2].WrapText = true;

                    Microsoft.Office.Interop.Excel.Range tRange = workSheet.UsedRange;
                    tRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    tRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                    AddSummaryPages(excelApp);

                    // check file path
                    foreach (var items in datesSelectedList)
                    {
                        AddPages(excelApp, items);
                    }




                        excelApp.Visible = true;
                        excelApp.Workbooks.Item[1].SaveAs(filePathAndName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

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

        private void AddSummaryPages(Microsoft.Office.Interop.Excel.Application excelApp)
        {
            //this is the All Data Sheet

            #region Get the report data
            DataTable dtSalesData3 = new DataTable();

            dtSalesData3.Columns.Add("TSRName");
            dtSalesData3.Columns.Add("RefNo");
            dtSalesData3.Columns.Add("Description");
            dtSalesData3.Columns.Add("Date");


            try { dtSalesData3.Clear(); } catch { }

            foreach (var item in UserIDs)
            {
                try
                {
                    TimeSpan ts1 = new TimeSpan(23, 00, 0);
                    DateTime enddate = _endDate.Date + ts1;

                    DataSet dsDiaryReportData = new DataSet();
                    dsDiaryReportData = Business.Insure.INGetReportCallTransfer(item, _startDate, enddate, IsUpgrade);

                    DataTable dtSalesRow = dsDiaryReportData.Tables[1];
                    foreach (DataRow items in dtSalesRow.Rows)
                    {
                        dtSalesData3.Rows.Add(items.ItemArray);
                    }
                }
                catch
                {

                }


            }


            #endregion Get the report data
            TimeSpan ts11 = new TimeSpan(23, 00, 0);

            DateTime enddate1 = _endDate.Date + ts11;

            int countForNonRedeemed = 0;
            Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.Worksheets.Add();
            workSheet.Name = "Data Sheet";
            workSheet.Cells[1, 0 + 1] = _endDate.ToString("yyyy/MM/dd");
            for (var i = 0; i < dtSalesData3.Columns.Count; i++)
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
            for (var i = 0; i < dtSalesData3.Columns.Count; i++)
            {

                workSheet.Cells[2, i + 1] = dtSalesData3.Columns[i].ColumnName;
            }

            // rows
            for (var i = 1; i < dtSalesData3.Rows.Count + 1; i++)
            {
                // to do: format datetime values before printing
                for (var j = 0; j < dtSalesData3.Columns.Count; j++)
                {
                    workSheet.Cells[i + 2, j + 1] = dtSalesData3.Rows[i - 1][j];
                    workSheet.Cells[i + 2, j + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                }

                countForNonRedeemed = countForNonRedeemed + 1;
            }

            #region Totals for Grid 1
            //workSheet.Cells[countForNonRedeemed + 3, 3].Formula = string.Format("=SUM(C3:C" + (countForNonRedeemed + 2).ToString() + ")"); //D
            //workSheet.Cells[countForNonRedeemed + 3, 4].Formula = string.Format("=SUM(D3:D" + (countForNonRedeemed + 2).ToString() + ")"); //E
            //workSheet.Cells[countForNonRedeemed + 3, 5].Formula = string.Format("=SUM(E3:E" + (countForNonRedeemed + 2).ToString() + ")"); //E
            //workSheet.Cells[countForNonRedeemed + 3, 6].Formula = string.Format("=SUM(F3:F" + (countForNonRedeemed + 2).ToString() + ")"); //F

            workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 4]].Merge();
            workSheet.Cells[1, 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            #endregion

            countForNonRedeemed = countForNonRedeemed + 3;
            int CountSecondGridTotals = countForNonRedeemed;


            Microsoft.Office.Interop.Excel.Range tRange = workSheet.UsedRange;
            tRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            tRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

            //workSheet.Range["A2", "B2"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
            //workSheet.Range["C2", "G2"].Interior.Color = System.Drawing.Color.LightBlue;


            //(workSheet.Cells[1, 7]).EntireColumn.NumberFormat = "##%";
            //(workSheet.Rows[2]).EntireRow.RowHeight = 30;
            //workSheet.Rows[2].WrapText = true;

            //(workSheet.Cells[1, 8]).EntireColumn.NumberFormat = "##%";
            //(workSheet.Cells[1, 9]).EntireColumn.NumberFormat = "##%";
            //(workSheet.Cells[1, 12]).EntireColumn.NumberFormat = "##%";
            //(workSheet.Cells[1, 14]).EntireColumn.NumberFormat = "##%";
            //(workSheet.Cells[1, 10]).EntireColumn.NumberFormat = "#";

        }


        private void AddPages(Microsoft.Office.Interop.Excel.Application excelApp, DateTime dateselected)
        {

            #region Get the report data
            DataTable dtSalesData2 = new DataTable();

            dtSalesData2.Columns.Add("TSR Name");
            dtSalesData2.Columns.Add("Employment Status");
            dtSalesData2.Columns.Add("Supervisor");
            dtSalesData2.Columns.Add("Total Sales");
            dtSalesData2.Columns.Add("Less Sales where Debi-checks are N/A");
            dtSalesData2.Columns.Add("Less DC Agent not available - valid");
            dtSalesData2.Columns.Add("Sales to Be Transferred");
            dtSalesData2.Columns.Add("Actual Sales Transferred");
            dtSalesData2.Columns.Add("% Transferred");
            dtSalesData2.Columns.Add("Total Calls Not Transferred");
            dtSalesData2.Columns.Add("%  Total calls not transferred");
            dtSalesData2.Columns.Add("Debi-Check agent not available");
            dtSalesData2.Columns.Add("Overtime Sale");
            dtSalesData2.Columns.Add("Difficult client");
            dtSalesData2.Columns.Add("Remote shift");
            dtSalesData2.Columns.Add("Actual Transferred");
            dtSalesData2.Columns.Add("Debi-Check agent not available - Invalid");
            dtSalesData2.Columns.Add("Total Sales Hidden");


            try { dtSalesData2.Clear(); } catch { }

            foreach (var item in UserIDs)
            {
                try
                {
                    TimeSpan ts1 = new TimeSpan(23, 00, 0);
                    DateTime enddate = dateselected.Date + ts1;

                    DataSet dsDiaryReportData = new DataSet();
                    dsDiaryReportData = Business.Insure.INGetReportCallTransfer(item, dateselected, enddate, IsUpgrade);

                    DataTable dtSalesRow = dsDiaryReportData.Tables[0];
                    foreach (DataRow items in dtSalesRow.Rows)
                    {
                        dtSalesData2.Rows.Add(items.ItemArray);
                    }
                }
                catch
                {

                }


            }


            #endregion Get the report data
            TimeSpan ts11 = new TimeSpan(23, 00, 0);

            DateTime enddate1 = dateselected.Date + ts11;

            int countForNonRedeemed = 0;
            Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.Worksheets.Add();
            workSheet.Name = dateselected.ToString("yyyy-MM-dd");
            workSheet.Cells[1, 0 + 1] = dateselected.ToString("yyyy/MM/dd");
            for (var i = 0; i < dtSalesData2.Columns.Count; i++)
            {

                workSheet.Cells[2, i + 1].Font.Bold = true;
                if (i == 0)
                {
                    workSheet.Cells[2, i + 1].ColumnWidth = 30;
                }
                else
                {
                    workSheet.Cells[2, i + 1].ColumnWidth = 15;
                }

                workSheet.Cells[2, i + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                //workSheet.get_Range("A4", "J1").Font.Bold = true;
            }


            // column headings
            for (var i = 0; i < dtSalesData2.Columns.Count; i++)
            {

                workSheet.Cells[2, i + 1] = dtSalesData2.Columns[i].ColumnName;
            }

            // rows
            for (var i = 1; i < dtSalesData2.Rows.Count + 1; i++)
            {
                // to do: format datetime values before printing
                for (var j = 0; j < dtSalesData2.Columns.Count; j++)
                {
                    workSheet.Cells[i + 2, j + 1] = dtSalesData2.Rows[i - 1][j];
                    workSheet.Cells[i + 2, j + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                }

                countForNonRedeemed = countForNonRedeemed + 1;
            }

            #region Totals for Grid 1
            for (int w = 3; w <= countForNonRedeemed + 3; w++)
            {
                workSheet.Cells[w, 7].Formula = string.Format("=R" + w + "-E" + w);
            }

            workSheet.Cells[countForNonRedeemed + 3, 4].Formula = string.Format("=SUM(D3:D" + (countForNonRedeemed + 2).ToString() + ")"); //E
            workSheet.Cells[countForNonRedeemed + 3, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[countForNonRedeemed + 3, 5].Formula = string.Format("=SUM(E3:E" + (countForNonRedeemed + 2).ToString() + ")"); //E
            workSheet.Cells[countForNonRedeemed + 3, 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[countForNonRedeemed + 3, 6].Formula = string.Format("=SUM(F3:F" + (countForNonRedeemed + 2).ToString() + ")"); //F
            workSheet.Cells[countForNonRedeemed + 3, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[countForNonRedeemed + 3, 7].Formula = string.Format("=SUM(G3:G" + (countForNonRedeemed + 2).ToString() + ")"); //H
            workSheet.Cells[countForNonRedeemed + 3, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[countForNonRedeemed + 3, 8].Formula = string.Format("=SUM(H3:H" + (countForNonRedeemed + 2).ToString() + ")"); //J
            workSheet.Cells[countForNonRedeemed + 3, 8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[countForNonRedeemed + 3, 10].Formula = string.Format("=SUM(J3:J" + (countForNonRedeemed + 2).ToString() + ")"); //K
            workSheet.Cells[countForNonRedeemed + 3, 10].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[countForNonRedeemed + 3, 12].Formula = string.Format("=SUM(L3:L" + (countForNonRedeemed + 2).ToString() + ")"); //L
            workSheet.Cells[countForNonRedeemed + 3, 12].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[countForNonRedeemed + 3, 13].Formula = string.Format("=SUM(M3:M" + (countForNonRedeemed + 2).ToString() + ")"); //M
            workSheet.Cells[countForNonRedeemed + 3, 13].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[countForNonRedeemed + 3, 14].Formula = string.Format("=SUM(N3:N" + (countForNonRedeemed + 2).ToString() + ")"); //M
            workSheet.Cells[countForNonRedeemed + 3, 14].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[countForNonRedeemed + 3, 15].Formula = string.Format("=SUM(O3:O" + (countForNonRedeemed + 2).ToString() + ")"); //D
            workSheet.Cells[countForNonRedeemed + 3, 15].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[countForNonRedeemed + 3, 16].Formula = string.Format("=SUM(P3:P" + (countForNonRedeemed + 2).ToString() + ")"); //L
            workSheet.Cells[countForNonRedeemed + 3, 16].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[countForNonRedeemed + 3, 17].Formula = string.Format("=SUM(Q3:Q" + (countForNonRedeemed + 2).ToString() + ")"); //L
            workSheet.Cells[countForNonRedeemed + 3, 17].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            workSheet.Cells[countForNonRedeemed + 3, 9].Formula = string.Format("=H" + (countForNonRedeemed + 3).ToString() + "/G" + (countForNonRedeemed + 3).ToString()); //M
            workSheet.Cells[countForNonRedeemed + 3, 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[countForNonRedeemed + 3, 11].Formula = string.Format("=J" + (countForNonRedeemed + 3).ToString() + "/G" + (countForNonRedeemed + 3).ToString()); //M
            workSheet.Cells[countForNonRedeemed + 3, 11].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            #endregion

            countForNonRedeemed = countForNonRedeemed + 3;
            int CountSecondGridTotals = countForNonRedeemed;

            workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 17]].Merge();
            workSheet.Cells[1, 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;


            //workSheet.get_Range("A2", "C2").BorderAround(
            //Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
            //Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin,
            //Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

            //workSheet.get_Range("A24", "C24").BorderAround(
            //Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
            //Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin,
            //Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);


            Microsoft.Office.Interop.Excel.Range tRange = workSheet.UsedRange;
            tRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            tRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

            workSheet.Range["A2", "B2"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
            workSheet.Range["C2", "Q2"].Interior.Color = System.Drawing.Color.LightBlue;


            (workSheet.Cells[1, 9]).EntireColumn.NumberFormat = "##%";
            (workSheet.Cells[1, 11]).EntireColumn.NumberFormat = "##%";
            (workSheet.Cells[1, 15]).EntireColumn.Hidden = true;
            (workSheet.Cells[1, 14]).EntireColumn.Hidden = true;
            (workSheet.Cells[1, 18]).EntireColumn.Hidden = true;

            (workSheet.Rows[2]).EntireRow.RowHeight = 40;
            workSheet.Rows[2].WrapText = true;

            //(workSheet.Cells[1, 8]).EntireColumn.NumberFormat = "##%";
            //(workSheet.Cells[1, 9]).EntireColumn.NumberFormat = "##%";
            //(workSheet.Cells[1, 12]).EntireColumn.NumberFormat = "##%";
            //(workSheet.Cells[1, 14]).EntireColumn.NumberFormat = "##%";
            //(workSheet.Cells[1, 10]).EntireColumn.NumberFormat = "#";

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
                    btnReport.IsEnabled = false;
                    calStartDate.IsEnabled = false;
                    calEndDate.IsEnabled = false;

                    //Cal2.IsEnabled = false;

                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += Report;
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
            BaseCB.IsChecked = false;
            IsUpgrade = "1";
        }

        private void BaseCB_Checked(object sender, RoutedEventArgs e)
        {
            UpgradeCB.IsChecked = false;
            IsUpgrade = "0";
        }


        #endregion


    }
}
