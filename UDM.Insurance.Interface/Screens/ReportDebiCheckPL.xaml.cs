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
using System.Transactions;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportDiaryScreen.xaml
    /// </summary>
    public partial class ReportDebiCheckPL
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

        public ReportDebiCheckPL()
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

        private void Report(object sender, DoWorkEventArgs e)
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


                    string filePathAndName = String.Format("{0}DebiCheck Report ({1}), {2}.xlsx", GlobalSettings.UserFolder, campaign, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
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
                        workSheet.Cells[2, i + 1].ColumnWidth = 20;
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
                            workSheet.Cells[i + 2, j + 1] = dtSalesData.Rows[ i - 1 ][ j ];
                        }
                    }



                    

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


                    string filePathAndName = String.Format("{0}DebiCheck Report ({1}), {2}.xlsx", GlobalSettings.UserFolder, campaign, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
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
                        if(i == 0)
                        {
                            workSheet.Cells[2, i + 1].ColumnWidth = 100;
                        }
                        else
                        {
                            workSheet.Cells[2, i + 1].ColumnWidth = 20;
                        }

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

                    int totalrows = dtSalesData.Rows.Count + 3;

                    var totalTable = dsDiaryReportData.Tables[1];

                    workSheet.Cells[totalrows, 2].Value = int.Parse(totalTable.Rows[0][0].ToString()) - 1;
                    workSheet.Cells[totalrows, 1].Value = "Total :";



                    //workSheet.get_Range("A2", "C23").BorderAround(
                    //Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    //Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin,
                    //Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "C2").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A" + totalrows.ToString(), "C" + totalrows.ToString()).BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);


                    Microsoft.Office.Interop.Excel.Range tRange = workSheet.UsedRange;
                    tRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    tRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                    //workSheet.Range["A3", "C5"].Interior.Color = System.Drawing.Color.LightSalmon;



                    (workSheet.Cells[1, 3]).EntireColumn.NumberFormat = "00,00%";


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
                    //btnReport.IsEnabled = false;
                    calStartDate.IsEnabled = false;
                    calEndDate.IsEnabled = false;

                    //Cal2.IsEnabled = false;

                    if (CancerCB.IsChecked == true)
                    {
                        TimeSpan ts = new TimeSpan(00, 30, 0);
                        _endDate = _endDate.Date + ts;

                        TimeSpan ts1 = new TimeSpan(12, 59, 0);
                        _startDate = _startDate.Date + ts1;
                    }
                    else
                    {
                        TimeSpan ts = new TimeSpan(13, 00, 0);
                        _endDate = _endDate.Date + ts;

                        TimeSpan ts1 = new TimeSpan(23, 00, 0);
                        _startDate = _startDate.Date + ts1;
                    }

                    campaign = "";
                    if (CancerCB.IsChecked == true)
                    {
                        campaign = "07H30";
                    }
                    else if (MaccCB.IsChecked == true)
                    {
                        campaign = "13H30";
                    }

                    DataTable dtSalesData = null;
                    if(ConsolidatedStats.IsChecked == true)
                    {
                        try
                        {
                            #region Parameter workings
                            TimeSpan ts = new TimeSpan(23, 00, 0);
                            DateTime _endDate2 = DateTime.Now;
                            _endDate2 = _endDate.Date + ts;

                            TimeSpan ts1 = new TimeSpan(00, 00, 0);
                            DateTime _startDat2 = DateTime.Now;
                            _startDat2 = _startDate.Date + ts1;

                            string baseupgrade = "";
                            if(UpgradeReportCB.IsChecked == true)
                            {
                                baseupgrade = "1";
                            }
                            else
                            {
                                baseupgrade = "0";
                            }
                            #endregion

                            var transactionOptions = new TransactionOptions
                            {
                                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                            };

                            using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                            {
                                dsDiaryReportData = Business.Insure.INGetDebiCheckPLConsolidated(_startDat2, _endDate2, baseupgrade);
                            }


                        }
                        catch(Exception w)
                        {
                            //#region Parameter workings
                            //TimeSpan ts = new TimeSpan(23, 00, 0);
                            //DateTime _endDate2 = DateTime.Now;
                            //_endDate2 = _endDate.Date + ts;

                            //TimeSpan ts1 = new TimeSpan(00, 00, 0);
                            //DateTime _startDat2 = DateTime.Now;
                            //_startDat2 = _startDate.Date + ts1;

                            //string baseupgrade = "";
                            //if (UpgradeReportCB.IsChecked == true)
                            //{
                            //    baseupgrade = "1";
                            //}
                            //else
                            //{
                            //    baseupgrade = "0";
                            //}
                            //#endregion


                            //dsDiaryReportData = Business.Insure.INGetDebiCheckPLConsolidated(_startDat2, _endDate2, baseupgrade);

                        }
                    }
                    else
                    {
                        if(DailyReportCB.IsChecked == true)
                        {
                            try
                            {

                                var transactionOptions = new TransactionOptions
                                {
                                    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                                };

                                using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                                {
                                    TimeSpan ts = new TimeSpan(00, 00, 0);
                                    DateTime _endDate2 = DateTime.Now;
                                    _endDate2 = _endDate.Date + ts;

                                    TimeSpan ts1 = new TimeSpan(23, 00, 0);
                                    DateTime _startDat2 = DateTime.Now;
                                    _startDat2 = _startDate.Date + ts1;
                                    dsDiaryReportData = Business.Insure.INGetDebiCheckPL( _endDate2, _startDat2);
                                }


                            }
                            catch(Exception a)
                            {

                                TimeSpan ts = new TimeSpan(23, 00, 0);
                                DateTime _endDate2 = DateTime.Now;
                                _endDate2 = _endDate.Date + ts;

                                TimeSpan ts1 = new TimeSpan(00, 00, 0);
                                DateTime _startDat2 = DateTime.Now;
                                _startDat2 = _startDate.Date + ts1;
                                DateTime startdate = DateTime.Parse(calStartDate.SelectedDate.ToString());
                                DateTime enddate = DateTime.Parse(calEndDate.SelectedDate.ToString());
                                dsDiaryReportData = Business.Insure.INGetDebiCheckPL(_startDat2, _endDate2);

                            }
                        }
                        else
                        {
                            try
                            {
                                dsDiaryReportData = Business.Insure.INGetDebiCheckPL(_endDate, _startDate);

                            }
                            catch
                            {
                                dsDiaryReportData = Business.Insure.INGetDebiCheckPL(_endDate, _startDate);

                            }
                        }

                    }


                    BackgroundWorker worker = new BackgroundWorker();
                    if(ConsolidatedStats.IsChecked == true)
                    {
                        worker.DoWork += ReportConsolidated;
                    }
                    else
                    {
                        worker.DoWork += Report;
                    }
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

        private void CancerCB_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CancerCB_Checked(object sender, RoutedEventArgs e)
        {
            ConsolidatedStats.IsChecked = false;
            MaccCB.IsChecked = false;
            CancerBaseBool = true;
            MaccBaseBool = false;
            DailyReportCB.IsChecked = false;
        }

        private void MaccCB_Checked(object sender, RoutedEventArgs e)
        {
            CancerCB.IsChecked = false;
            ConsolidatedStats.IsChecked = false;
            CancerBaseBool = false;
            MaccBaseBool = true;
            DailyReportCB.IsChecked = false;
        }

        private void ConsolidatedStats_Checked(object sender, RoutedEventArgs e)
        {
            CancerCB.IsChecked = false;
            MaccCB.IsChecked = false;
            CancerBaseBool = false;
            MaccBaseBool = false;
            DailyReportCB.IsChecked = false;

            #region Base / upgrades Visibility
            BaseLbl.Visibility = Visibility.Visible;
            UpgradeLbl.Visibility = Visibility.Visible;
            BaseReportCB.Visibility = Visibility.Visible;
            UpgradeReportCB.Visibility = Visibility.Visible;
            BaseUpgradeBorder.Visibility = Visibility.Visible;
            #endregion
        }

        private void DailyReportCB_Checked(object sender, RoutedEventArgs e)
        {
            CancerCB.IsChecked = false;
            MaccCB.IsChecked = false;
            CancerBaseBool = false;
            MaccBaseBool = false;
            ConsolidatedStats.IsChecked = false;
        }

        private void MaccCB_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void ConsolidatedStats_Unchecked(object sender, RoutedEventArgs e)
        {
            #region Base / upgrades Visibility
            BaseLbl.Visibility = Visibility.Collapsed;
            UpgradeLbl.Visibility = Visibility.Collapsed;
            BaseReportCB.Visibility = Visibility.Collapsed;
            UpgradeReportCB.Visibility = Visibility.Collapsed;
            BaseUpgradeBorder.Visibility = Visibility.Collapsed;
            #endregion
        }

        #region Search DebiCheck Status
        private void RefrenceGoBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string refno = ReferenceNumberTB.Text;

                DataSet DebiCheckLoopup = null;
                DataTable DebiCheckLookupDT = null;


                var transactionOptions = new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                };

                using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    DebiCheckLoopup = Business.Insure.INGetDebiCheckLookupPL(refno);
                }

                DebiCheckLookupDT = DebiCheckLoopup.Tables[0];
                MandateLookupDG.ItemsSource = DebiCheckLookupDT.DefaultView;

            }
            catch (Exception q)
            {

            }
        }

        private void RefrenceGoOpenBtn_Click(object sender, RoutedEventArgs e)
        {
            RefrenceGoBtn.Visibility = Visibility;
            ReferenceNumberTB.Visibility = Visibility;
            MandateLookupDG.Visibility = Visibility;
        }

        #endregion




        private void BaseReportCB_Checked(object sender, RoutedEventArgs e)
        {
            if(BaseReportCB.IsChecked == true)
            {
                UpgradeReportCB.IsChecked = false;
            }   
        }

        private void UpgradeReportCB_Checked(object sender, RoutedEventArgs e)
        { 
            if(UpgradeReportCB.IsChecked == true)
            {
                BaseReportCB.IsChecked = false;
            }
        }
    }
}
