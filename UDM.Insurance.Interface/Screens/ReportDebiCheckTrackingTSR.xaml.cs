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
using Embriant.WPF.Controls;
using System.Transactions;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportDiaryScreen.xaml
    /// </summary>
    public partial class ReportDebiCheckTrackingTSR
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

        private bool? _allRecordsChecked = false;
        public bool? AllRecordsChecked
        {
            get
            {
                return _allRecordsChecked;
            }
            set
            {
                _allRecordsChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllRecordsChecked"));
            }
        }

        #region Constants
        DataSet dsDebiCheckTrackingTSRReportData;
        private List<System.Data.DataRow> _selectedAgents;
        private List<DataRecord> _selectedTeams;

        DataTable dtSalesData = new DataTable();

        bool TeamBool = false;
        bool TrainerBool = false;
        long SelectedTeamID;
        string TeamIDs;
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

        public ReportDebiCheckTrackingTSR()
        {
            InitializeComponent();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);

            #region Report Columns
            dtSalesData.Columns.Add("Agent Name");
            dtSalesData.Columns.Add("Original Sales Sold");
            dtSalesData.Columns.Add("Less Debi-checks still outstanding");
            dtSalesData.Columns.Add("Sales on System");
            dtSalesData.Columns.Add("Accepted");
            dtSalesData.Columns.Add("Accepted %");
            dtSalesData.Columns.Add("No Reponses");
            dtSalesData.Columns.Add("No Responses %");
            dtSalesData.Columns.Add("Other Responses");
            dtSalesData.Columns.Add("Other Responses %");
            dtSalesData.Columns.Add("Original Debi-Checks Rejected");
            dtSalesData.Columns.Add("Original Debi-Checks Rejected %");
            dtSalesData.Columns.Add("Debi-Check Callback to Sale");
            dtSalesData.Columns.Add("Sale over Debi-Check %");
            dtSalesData.Columns.Add("Debi-Check to Decline");
            dtSalesData.Columns.Add("Decline over Debi-Check %");
            dtSalesData.Columns.Add("Debi-Check to CF/Cancel");
            dtSalesData.Columns.Add("Debi-Check to CF/Cancel %");
            dtSalesData.Columns.Add("Final Debi-Check Rejected");
            dtSalesData.Columns.Add("Final Debi-Check Rejected %");
            dtSalesData.Columns.Add("Current Debi-Check Call Backs");
            dtSalesData.Columns.Add("Current Debi-Check Call Backs %");
            dtSalesData.Columns.Add("Other Lead Statuses");
            dtSalesData.Columns.Add("Other Lead Statuses %");
            dtSalesData.Columns.Add("Sales where Debi-checks are N/A");
            dtSalesData.Columns.Add("Accepted % after n/a sales were removed");
            dtSalesData.Columns.Add("Supervisor Name");

            #endregion

            LoadDatagrid();
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

                if (TrainerBool == true)
                {

                    var dsAgents = Business.Insure.INGetDebiChecKTrackingTSRAgentsTeam(TeamIDs);


                    _selectedAgents = dsAgents.Tables[0].AsEnumerable().ToList();
                }
                else if (TeamBool == true)
                {

                    //_selectedTeams = (xdgAgents.Records.Select(r => (DataRecord)r).Where(r => (bool)r.Cells["IsChecked"].Value)).ToList();

                    var dsAgents = Business.Insure.INGetDebiChecKTrackingTSRAgentsTeam(TeamIDs);


                    _selectedAgents = dsAgents.Tables[0].AsEnumerable().ToList();
                }
                else
                {
                    var dsAgents = Business.Insure.INGetDebiChecKTrackingTSRAgents(_endDate, _startDate);


                    _selectedAgents = dsAgents.Tables[0].AsEnumerable().ToList();
                }



                try { dtSalesData.Clear(); } catch { };

                try { dsDebiCheckTrackingTSRReportData.Clear(); } catch { };
                if (_selectedAgents.Count > 0)
                {
                    foreach (System.Data.DataRow drAgent in _selectedAgents)
                    {
                        DataTable dtTempSalesData = null;

                        long? agentID = drAgent.ItemArray[0] as long?;
                        DataSet ds = null;
                        var transactionOptions = new TransactionOptions
                        {
                            IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                        };

                        using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                        {
                            ds = Business.Insure.INGetDebiChecKTrackingTSR(_endDate, _startDate, agentID);
                        }

                        dtTempSalesData = ds.Tables[0];
                        foreach (DataRow row in dtTempSalesData.Rows)
                        {
                            dtSalesData.Rows.Add(row.ItemArray);
                        }


                    }
                }

                #endregion Get the report data

                try
                {
                    string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


                    string filePathAndName = String.Format("{0}DebiCheck Report TSR Base {1}, {2}.xlsx", GlobalSettings.UserFolder, campaign, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
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

                    workSheet.Cells[2, 27].ColumnWidth = 20;

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
                    (workSheet.Cells[1, 26]).EntireColumn.NumberFormat = "00,00%";

                    workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 26]].Merge();

                    workSheet.Cells[totalrows, 1].Value = "Total :";

                    workSheet.Rows[2].WrapText = true;

                    Microsoft.Office.Interop.Excel.Range tRange = workSheet.UsedRange;
                    tRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    tRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                    // check file path

                    workSheet.get_Range("A2", "Y2").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "D" + totalrows.ToString()).BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "F" + totalrows.ToString()).BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "J" + totalrows.ToString()).BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "V" + totalrows.ToString()).BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "Z" + totalrows.ToString()).BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A" + totalrows.ToString(), "Z" + totalrows.ToString()).BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.Range["F3", "F" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["H3", "H" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["J3", "J" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["L3", "L" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["N3", "N" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["P3", "P" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["R3", "R" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["T3", "T" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["V3", "V" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["X3", "X" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;

                    int totalRowMinusOne = totalrows - 1;

                    workSheet.Cells[totalrows, 2].Formula = string.Format("=SUM(B1:B" + totalRowMinusOne.ToString() + ")"); //B
                    workSheet.Cells[totalrows, 3].Formula = string.Format("=SUM(C1:C" + totalRowMinusOne.ToString() + ")"); //C
                    workSheet.Cells[totalrows, 4].Formula = string.Format("=SUM(D1:D" + totalRowMinusOne.ToString() + ")"); //D
                    workSheet.Cells[totalrows, 5].Formula = string.Format("=SUM(E1:E" + totalRowMinusOne.ToString() + ")"); //E
                    workSheet.Cells[totalrows, 7].Formula = string.Format("=SUM(G1:G" + totalRowMinusOne.ToString() + ")"); //G
                    workSheet.Cells[totalrows, 9].Formula = string.Format("=SUM(I1:I" + totalRowMinusOne.ToString() + ")"); //I
                    workSheet.Cells[totalrows, 11].Formula = string.Format("=SUM(K1:K" + totalRowMinusOne.ToString() + ")"); //K
                    workSheet.Cells[totalrows, 13].Formula = string.Format("=SUM(M1:M" + totalRowMinusOne.ToString() + ")"); //M
                    workSheet.Cells[totalrows, 15].Formula = string.Format("=SUM(O1:O" + totalRowMinusOne.ToString() + ")"); //O
                    workSheet.Cells[totalrows, 17].Formula = string.Format("=SUM(Q1:Q" + totalRowMinusOne.ToString() + ")"); //Q
                    workSheet.Cells[totalrows, 19].Formula = string.Format("=SUM(S1:S" + totalRowMinusOne.ToString() + ")"); //S
                    workSheet.Cells[totalrows, 21].Formula = string.Format("=SUM(U1:U" + totalRowMinusOne.ToString() + ")"); //U
                    workSheet.Cells[totalrows, 23].Formula = string.Format("=SUM(W1:W" + totalRowMinusOne.ToString() + ")"); //W
                    workSheet.Cells[totalrows, 25].Formula = string.Format("=SUM(Y1:Y" + totalRowMinusOne.ToString() + ")"); //y
                    workSheet.Cells[totalrows, 26].Formula = string.Format("=E" + totalrows + "/(B" + totalrows + " - Y" + totalrows + ") * 100"); //Z

                    workSheet.Cells[totalrows, 6].Formula = string.Format("=E" + totalrows + "/B" + totalrows + "*100"); //F
                    workSheet.Cells[totalrows, 8].Formula = string.Format("=G" + totalrows + "/B" + totalrows + "*100"); //H
                    workSheet.Cells[totalrows, 10].Formula = string.Format("=I" + totalrows + "/B" + totalrows + "*100"); //J
                    workSheet.Cells[totalrows, 12].Formula = string.Format("=K" + totalrows + "/B" + totalrows + "*100"); //L
                    workSheet.Cells[totalrows, 14].Formula = string.Format("=M" + totalrows + "/B" + totalrows + "*100"); //N
                    workSheet.Cells[totalrows, 16].Formula = string.Format("=O" + totalrows + "/B" + totalrows + "*100"); //P
                    workSheet.Cells[totalrows, 18].Formula = string.Format("=Q" + totalrows + "/B" + totalrows + "*100"); //R
                    workSheet.Cells[totalrows, 20].Formula = string.Format("=S" + totalrows + "/B" + totalrows + "*100"); //T
                    workSheet.Cells[totalrows, 22].Formula = string.Format("=U" + totalrows + "/B" + totalrows + "*100"); //V
                    workSheet.Cells[totalrows, 24].Formula = string.Format("=W" + totalrows + "/B" + totalrows + "*100"); //X


                    for (int w = 3; w <= totalRowMinusOne; w++)
                    {
                        workSheet.Cells[w, 3].Formula = string.Format("=U" + w + "+Q" + w + "+S" + w + "+O" + w + "+W" + w);
                    }

                    for (int r = 3; r <= totalRowMinusOne; r++)
                    {
                        workSheet.Cells[r, 2].Formula = string.Format("=D" + r + "+C" + r + "");
                        workSheet.Cells[r, 26].Formula = string.Format("=E" + r + "/(B" + r + "-Y" + r + ") *100");
                    }

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

        private void ReportUpgrades(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Get the report data

                try { _selectedAgents.Clear(); } catch { }

                if (TrainerBool == true)
                {

                    var dsAgents = Business.Insure.INGetDebiChecKTrackingTSRAgentsTeam(TeamIDs);


                    _selectedAgents = dsAgents.Tables[0].AsEnumerable().ToList();
                }
                else if (TeamBool == true)
                {

                    //_selectedTeams = (xdgAgents.Records.Select(r => (DataRecord)r).Where(r => (bool)r.Cells["IsChecked"].Value)).ToList();

                    var dsAgents = Business.Insure.INGetDebiChecKTrackingTSRAgentsTeam(TeamIDs);


                    _selectedAgents = dsAgents.Tables[0].AsEnumerable().ToList();
                }
                else
                {
                    var dsAgents = Business.Insure.INGetDebiChecKTrackingTSRAgentsUpgrades(_endDate, _startDate);


                    _selectedAgents = dsAgents.Tables[0].AsEnumerable().ToList();
                }




                try { dtSalesData.Clear(); } catch { };

                try { dsDebiCheckTrackingTSRReportData.Clear(); } catch { };
                if (_selectedAgents.Count > 0)
                {
                    foreach (System.Data.DataRow drAgent in _selectedAgents)
                    {
                        DataTable dtTempSalesData = null;

                        long? agentID = drAgent.ItemArray[0] as long?;
                        DataSet ds = null;
                        var transactionOptions = new TransactionOptions
                        {
                            IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                        };

                        using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                        {
                            ds = Business.Insure.INGetDebiChecKTrackingTSRUpgrades(_endDate, _startDate, agentID);
                        }

                        dtTempSalesData = ds.Tables[0];
                        foreach (DataRow row in dtTempSalesData.Rows)
                        {
                            dtSalesData.Rows.Add(row.ItemArray);
                        }


                    }
                }

                #endregion Get the report data

                try
                {
                    string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


                    string filePathAndName = String.Format("{0}DebiCheck Report TSR Upgrades {1}, {2}.xlsx", GlobalSettings.UserFolder, campaign, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
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

                    workSheet.Cells[2, 27].ColumnWidth = 30;

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

                    (workSheet.Cells[1, 6]).EntireColumn.NumberFormat = "00,00%"; // This sets the format of the column
                    (workSheet.Cells[1, 8]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 10]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 12]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 14]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 16]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 18]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 20]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 22]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 24]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 26]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 26]).EntireColumn.NumberFormat = "00,00%";


                    workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 26]].Merge();

                    workSheet.Cells[totalrows, 1].Value = "Total :";

                    workSheet.Rows[2].WrapText = true;

                    Microsoft.Office.Interop.Excel.Range tRange = workSheet.UsedRange;
                    tRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    tRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                    // check file path

                    workSheet.get_Range("A2", "Y2").BorderAround( // these apply borders on the report
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "D" + totalrows.ToString()).BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "F" + totalrows.ToString()).BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "J" + totalrows.ToString()).BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "V" + totalrows.ToString()).BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "Z" + totalrows.ToString()).BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A" + totalrows.ToString(), "Z" + totalrows.ToString()).BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.Range["F3", "F" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["H3", "H" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["J3", "J" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["L3", "L" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["N3", "N" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["P3", "P" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["R3", "R" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["T3", "T" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["V3", "V" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["X3", "X" + totalrows.ToString()].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;

                    int totalRowMinusOne = totalrows - 1;

                    workSheet.Cells[totalrows, 2].Formula = string.Format("=SUM(B1:B" + totalRowMinusOne.ToString() + ")"); //B
                    workSheet.Cells[totalrows, 3].Formula = string.Format("=SUM(C1:C" + totalRowMinusOne.ToString() + ")"); //C
                    workSheet.Cells[totalrows, 4].Formula = string.Format("=SUM(D1:D" + totalRowMinusOne.ToString() + ")"); //D
                    workSheet.Cells[totalrows, 5].Formula = string.Format("=SUM(E1:E" + totalRowMinusOne.ToString() + ")"); //E
                    workSheet.Cells[totalrows, 7].Formula = string.Format("=SUM(G1:G" + totalRowMinusOne.ToString() + ")"); //G
                    workSheet.Cells[totalrows, 9].Formula = string.Format("=SUM(I1:I" + totalRowMinusOne.ToString() + ")"); //I
                    workSheet.Cells[totalrows, 11].Formula = string.Format("=SUM(K1:K" + totalRowMinusOne.ToString() + ")"); //K
                    workSheet.Cells[totalrows, 13].Formula = string.Format("=SUM(M1:M" + totalRowMinusOne.ToString() + ")"); //M
                    workSheet.Cells[totalrows, 15].Formula = string.Format("=SUM(O1:O" + totalRowMinusOne.ToString() + ")"); //O
                    workSheet.Cells[totalrows, 17].Formula = string.Format("=SUM(Q1:Q" + totalRowMinusOne.ToString() + ")"); //Q
                    workSheet.Cells[totalrows, 19].Formula = string.Format("=SUM(S1:S" + totalRowMinusOne.ToString() + ")"); //S
                    workSheet.Cells[totalrows, 21].Formula = string.Format("=SUM(U1:U" + totalRowMinusOne.ToString() + ")"); //U
                    workSheet.Cells[totalrows, 23].Formula = string.Format("=SUM(W1:W" + totalRowMinusOne.ToString() + ")"); //W
                    workSheet.Cells[totalrows, 25].Formula = string.Format("=SUM(Y1:Y" + totalRowMinusOne.ToString() + ")"); //y
                    workSheet.Cells[totalrows, 26].Formula = string.Format("=E" + totalrows + "/(B" + totalrows + " - Y" + totalrows + ") * 100"); //Z

                    workSheet.Cells[totalrows, 6].Formula = string.Format("=E" + totalrows + "/B" + totalrows + "*100"); //F
                    workSheet.Cells[totalrows, 8].Formula = string.Format("=G" + totalrows + "/B" + totalrows + "*100"); //H
                    workSheet.Cells[totalrows, 10].Formula = string.Format("=I" + totalrows + "/B" + totalrows + "*100"); //J
                    workSheet.Cells[totalrows, 12].Formula = string.Format("=K" + totalrows + "/B" + totalrows + "*100"); //L
                    workSheet.Cells[totalrows, 14].Formula = string.Format("=M" + totalrows + "/B" + totalrows + "*100"); //N
                    workSheet.Cells[totalrows, 16].Formula = string.Format("=O" + totalrows + "/B" + totalrows + "*100"); //P
                    workSheet.Cells[totalrows, 18].Formula = string.Format("=Q" + totalrows + "/B" + totalrows + "*100"); //R
                    workSheet.Cells[totalrows, 20].Formula = string.Format("=S" + totalrows + "/B" + totalrows + "*100"); //T
                    workSheet.Cells[totalrows, 22].Formula = string.Format("=U" + totalrows + "/B" + totalrows + "*100"); //V
                    workSheet.Cells[totalrows, 24].Formula = string.Format("=W" + totalrows + "/B" + totalrows + "*100"); //X

                    for (int w = 3; w <= totalRowMinusOne; w++)
                    {
                        workSheet.Cells[w, 3].Formula = string.Format("=U" + w + "+Q" + w + "+S" + w + "+O" + w + "+W" + w);
                    }

                    for (int r = 3; r <= totalRowMinusOne; r++)
                    {
                        workSheet.Cells[r, 2].Formula = string.Format("=D" + r + "+C" + r + "");
                        workSheet.Cells[r, 26].Formula = string.Format("=E" + r + "/(B" + r + "-Y" + r + ") *100");

                    }

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

        private void LoadDatagrid()
        {
            //SqlParameter[] parameters = new SqlParameter[1];
            //parameters[0] = new SqlParameter("@AgentMode", (int)1);
            //DataTable dt = Methods.ExecuteStoredProcedure("spGetSalesAgents4", parameters).Tables[0];

            //DataColumn column = new DataColumn("IsChecked", typeof(bool));
            //column.DefaultValue = false;
            //dt.Columns.Add(column);

            //xdgAgents.DataSource = dt.DefaultView;

            //AllRecordsChecked = false;
        }
        private bool? IsAllRecordsChecked()
        {
            try
            {
                bool allSelected = (xdgAgents.Records.Select(r => (bool)((DataRecord)r).Cells["IsChecked"].Value)).All(b => b);
                bool noneSelected = (xdgAgents.Records.Select(r => (bool)((DataRecord)r).Cells["IsChecked"].Value)).All(b => !b);

                int countSelected = (xdgAgents.Records.Select(r => (bool)((DataRecord)r).Cells["IsChecked"].Value)).Count(b => b);

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
        private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            AllRecordsChecked = IsAllRecordsChecked();
        }
        private void HeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (xdgAgents.DataSource != null)
                {
                    DataTable dt = ((DataView)xdgAgents.DataSource).Table;

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["IsChecked"] = false;
                    }
                }

                IsAllRecordsChecked();
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
                if (xdgAgents.DataSource != null)
                {
                    DataTable dt = ((DataView)xdgAgents.DataSource).Table;

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["IsChecked"] = true;
                    }
                }

                IsAllRecordsChecked();
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }
        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            SetCursor(Cursors.Wait);

            try
            {
                //_selectedAgents = (xdgAgents.Records.Select(r => (DataRecord)r).Where(r => (bool)r.Cells["IsChecked"].Value)).ToList();

                if (IsAllInputParametersSpecifiedAndValid())
                {

                    if (UpgradeCB.IsChecked == true)
                    {
                        btnClose.IsEnabled = false;
                        calStartDate.IsEnabled = false;
                        calEndDate.IsEnabled = false;

                        if (TrainerCB.IsChecked == true)
                        {
                            TrainerBool = true;
                            TeamBool = false;
                            _selectedTeams = (xdgAgents.Records.Select(r => (DataRecord)r).Where(r => (bool)r.Cells["IsChecked"].Value)).ToList();

                            try { _selectedTeams.Clear(); } catch { }
                            TeamIDs = "";
                            foreach (DataRecord drAgent in _selectedTeams)
                            {
                                string agentName = drAgent.Cells["Description"].Value as string;
                                long? agentID = drAgent.Cells["ID"].Value as long?;

                                TeamIDs = TeamIDs + " ," + agentID.ToString();
                            }

                            TeamIDs = TeamIDs.Remove(0, 2);

                        }
                        else if (TeamCB.IsChecked == true)
                        {
                            TrainerBool = false;
                            TeamBool = true;

                            try { _selectedTeams.Clear(); } catch { }
                            _selectedTeams = (xdgAgents.Records.Select(r => (DataRecord)r).Where(r => (bool)r.Cells["IsChecked"].Value)).ToList();
                            TeamIDs = "";

                            foreach (DataRecord drAgent in _selectedTeams)
                            {
                                string agentName = drAgent.Cells["Description"].Value as string;
                                long? agentID = drAgent.Cells["ID"].Value as long?;

                                TeamIDs = TeamIDs + " ," + agentID.ToString();
                            }

                            TeamIDs = TeamIDs.Remove(0, 2);

                        }
                        else
                        {
                            TrainerBool = false;
                            TeamBool = false;
                        }


                        BackgroundWorker worker = new BackgroundWorker();

                        worker.DoWork += ReportUpgrades;
                        worker.RunWorkerCompleted += ReportCompleted;
                        worker.RunWorkerAsync();
                        SetCursor(Cursors.Arrow);

                        dispatcherTimer1.Start();
                    }
                    else
                    {
                        btnClose.IsEnabled = false;
                        calStartDate.IsEnabled = false;
                        calEndDate.IsEnabled = false;

                        if (TrainerCB.IsChecked == true)
                        {
                            TrainerBool = true;
                            TeamBool = false;
                            _selectedTeams = (xdgAgents.Records.Select(r => (DataRecord)r).Where(r => (bool)r.Cells["IsChecked"].Value)).ToList();

                            try { _selectedTeams.Clear(); } catch { }
                            TeamIDs = "";
                            foreach (DataRecord drAgent in _selectedTeams)
                            {
                                string agentName = drAgent.Cells["Description"].Value as string;
                                long? agentID = drAgent.Cells["ID"].Value as long?;

                                TeamIDs = TeamIDs + " ," + agentID.ToString();
                            }

                            TeamIDs = TeamIDs.Remove(0, 2);

                        }
                        else if (TeamCB.IsChecked == true)
                        {
                            TrainerBool = false;
                            TeamBool = true;

                            try { _selectedTeams.Clear(); } catch { }
                            _selectedTeams = (xdgAgents.Records.Select(r => (DataRecord)r).Where(r => (bool)r.Cells["IsChecked"].Value)).ToList();
                            TeamIDs = "";

                            foreach (DataRecord drAgent in _selectedTeams)
                            {
                                string agentName = drAgent.Cells["Description"].Value as string;
                                long? agentID = drAgent.Cells["ID"].Value as long?;

                                TeamIDs = TeamIDs + " ," + agentID.ToString();
                            }

                            TeamIDs = TeamIDs.Remove(0, 2);

                        }
                        else
                        {
                            TrainerBool = false;
                            TeamBool = false;
                        }

                        BackgroundWorker worker = new BackgroundWorker();

                        worker.DoWork += Report;
                        worker.RunWorkerCompleted += ReportCompleted;
                        worker.RunWorkerAsync();
                        SetCursor(Cursors.Arrow);

                        dispatcherTimer1.Start();
                    }



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

        private void UpgradeCB_Checked(object sender, RoutedEventArgs e)
        {
            BaseCB.IsChecked = false;
        }

        private void BaseCB_Checked(object sender, RoutedEventArgs e)
        {
            UpgradeCB.IsChecked = false;
        }

        private void HeaderPrefixAreaCheckbox_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void RecordSelectorCheckbox_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void TrainerCB_Checked(object sender, RoutedEventArgs e)
        {

            if (TrainerCB.IsChecked == true)
            {
                TeamCB.IsChecked = false;
            }

            DataTable dt = Methods.ExecuteStoredProcedure("spGetSalesTrainingSupervisors", null).Tables[0];

            DataColumn column = new DataColumn("IsChecked", typeof(bool));
            column.DefaultValue = false;
            dt.Columns.Add(column);

            DataColumn column2 = new DataColumn("FKStaffTypeID", typeof(long));
            column2.DefaultValue = 1;
            dt.Columns.Add(column2);

            xdgAgents.DataSource = dt.DefaultView;

            AllRecordsChecked = false;

        }

        private void TeamCB_Checked(object sender, RoutedEventArgs e)
        {
            if (TeamCB.IsChecked == true)
            {
                TrainerCB.IsChecked = false;
            }

            DataTable dt = Methods.ExecuteStoredProcedure("spGetSalesSupervisors", null).Tables[0];

            DataColumn column = new DataColumn("IsChecked", typeof(bool));
            column.DefaultValue = false;
            dt.Columns.Add(column);

            DataColumn column2 = new DataColumn("FKStaffTypeID", typeof(long));
            column2.DefaultValue = 1;
            dt.Columns.Add(column2);

            xdgAgents.DataSource = dt.DefaultView;

            AllRecordsChecked = false;
        }

        private void xdgAgents_SelectedItemsChanged(object sender, Infragistics.Windows.DataPresenter.Events.SelectedItemsChangedEventArgs e)
        {

        }
    }
}
