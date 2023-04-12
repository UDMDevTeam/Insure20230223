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
using System.Text;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportDiaryScreen.xaml
    /// </summary>
    public partial class ReportDebiCheckTracker
    {

        #region Constants
        DataSet dsDiaryReportData;
        string strQuery;
        string strQuery2;


        #endregion Constants

        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        //private RecordCollectionBase _campaigns;
        //private System.Collections.Generic.List<Record> _campaigns;
        private List<Record> _lstSelectedCampaigns;
        private int? _campaignIDList = null;

        DataTable dtSalesData = new DataTable();
        private List<System.Data.DataRow> _selectedAgents;
        private List<System.Data.DataRow> _campaignsWithNoSales;

        DataSet dsDebiCheckTrackingTSRReportData;


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

            #region Report Columns
            dtSalesData.Columns.Add("Campaign Name");
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
            //dtSalesData.Columns.Add("Supervisor Name");

            #endregion

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


                    string filePathAndName = String.Format("{0}DebiCheck Tracking Report Base ({1}), {2}.xlsx", GlobalSettings.UserFolder, campaign, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
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

                    workSheet.Cells[33, 2].Value = int.Parse(totalTable.Rows[0][0].ToString()) - 1;
                    workSheet.Cells[33, 1].Value = "Total :";

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
                    (workSheet.Cells[1, 24]).EntireColumn.NumberFormat = "00,00%";
                    (workSheet.Cells[1, 26]).EntireColumn.NumberFormat = "00,00%";


                    workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 24]].Merge();

                    workSheet.Rows[2].WrapText = true;

                    Microsoft.Office.Interop.Excel.Range tRange = workSheet.UsedRange;
                    tRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    tRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                    workSheet.get_Range("A2", "Z2").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "D32").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "F32").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "J32").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "V32").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A2", "Z32").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.get_Range("A33", "Z33").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

                    workSheet.Range["F3", "F32"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["H3", "H32"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["J3", "J32"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["L3", "L32"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["N3", "N32"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["P3", "P32"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["R3", "R32"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["T3", "T32"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["V3", "V32"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                    workSheet.Range["X3", "X32"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;


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
                    workSheet.Cells[24, 3].Formula = string.Format("=U24+Q24+S24+O24+W24");
                    workSheet.Cells[25, 3].Formula = string.Format("=U25+Q25+S25+O25+W25");
                    workSheet.Cells[26, 3].Formula = string.Format("=U26+Q26+S26+O26+W26");
                    workSheet.Cells[27, 3].Formula = string.Format("=U27+Q27+S27+O27+W27");
                    workSheet.Cells[28, 3].Formula = string.Format("=U28+Q28+S28+O28+W28");
                    workSheet.Cells[29, 3].Formula = string.Format("=U29+Q29+S29+O29+W29");
                    workSheet.Cells[30, 3].Formula = string.Format("=U30+Q30+S30+O30+W30");
                    workSheet.Cells[31, 3].Formula = string.Format("=U31+Q31+S31+O31+W31");
                    workSheet.Cells[32, 3].Formula = string.Format("=U32+Q32+S32+O32+W32");


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
                    workSheet.Cells[24, 2].Formula = string.Format("=D24+C24");
                    workSheet.Cells[25, 2].Formula = string.Format("=D25+C25");
                    workSheet.Cells[26, 2].Formula = string.Format("=D26+C26");
                    workSheet.Cells[27, 2].Formula = string.Format("=D27+C27");
                    workSheet.Cells[28, 2].Formula = string.Format("=D28+C28");
                    workSheet.Cells[29, 2].Formula = string.Format("=D29+C29");
                    workSheet.Cells[30, 2].Formula = string.Format("=D30+C30");
                    workSheet.Cells[31, 2].Formula = string.Format("=D31+C31");
                    workSheet.Cells[32, 2].Formula = string.Format("=D32+C32");


                    workSheet.Cells[33, 2].Formula = string.Format("=SUM(B1:B32)"); //B
                    workSheet.Cells[33, 3].Formula = string.Format("=SUM(C1:C32)"); //C
                    workSheet.Cells[33, 4].Formula = string.Format("=SUM(D1:D32)"); //D
                    workSheet.Cells[33, 5].Formula = string.Format("=SUM(E1:E32)"); //E
                    workSheet.Cells[33, 7].Formula = string.Format("=SUM(G1:G32)"); //G
                    workSheet.Cells[33, 9].Formula = string.Format("=SUM(I1:I32)"); //I
                    workSheet.Cells[33, 11].Formula = string.Format("=SUM(K1:K32)"); //K
                    workSheet.Cells[33, 13].Formula = string.Format("=SUM(M1:M32)"); //M
                    workSheet.Cells[33, 15].Formula = string.Format("=SUM(O1:O32)"); //O
                    workSheet.Cells[33, 17].Formula = string.Format("=SUM(Q1:Q32)"); //Q
                    workSheet.Cells[33, 19].Formula = string.Format("=SUM(S1:S32)"); //S
                    workSheet.Cells[33, 21].Formula = string.Format("=SUM(U1:U32)"); //U
                    workSheet.Cells[33, 23].Formula = string.Format("=SUM(W1:W32)"); //W
                    workSheet.Cells[33, 25].Formula = string.Format("=SUM(Y1:Y32)"); //y

                    workSheet.Cells[33, 6].Formula = string.Format("=E33/B33*100"); //F
                    workSheet.Cells[33, 8].Formula = string.Format("=G33/B33*100"); //H
                    workSheet.Cells[33, 10].Formula = string.Format("=I33/B33*100"); //J
                    workSheet.Cells[33, 12].Formula = string.Format("=K33/B33*100"); //L
                    workSheet.Cells[33, 14].Formula = string.Format("=M33/B33*100"); //N
                    workSheet.Cells[33, 16].Formula = string.Format("=O33/B33*100"); //P
                    workSheet.Cells[33, 18].Formula = string.Format("=Q33/B33*100"); //R
                    workSheet.Cells[33, 20].Formula = string.Format("=S33/B33*100"); //T
                    workSheet.Cells[33, 22].Formula = string.Format("=U33/B33*100"); //V
                    workSheet.Cells[33, 24].Formula = string.Format("=W33/B33*100"); //X
                    workSheet.Cells[33, 26].Formula = string.Format("=E33/(B33-Y33)*100"); //X


                    int totalrows = dtSalesData.Rows.Count + 3;
                    int totalRowMinusOne = totalrows - 1;

                    for (int r = 3; r <= totalRowMinusOne; r++)
                    {
                        workSheet.Cells[r, 2].Formula = string.Format("=D" + r + "+C" + r + "");
                        workSheet.Cells[r, 26].Formula = string.Format("=E" + r + "/(B" + r + "-Y" + r + ") *100");
                    }

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
        private void ReportConsolidatedUpgrades(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Get the report data

                TimeSpan ts = new TimeSpan(23, 00, 0);
                DateTime DateEnd = _endDate.Date + ts;

                TimeSpan ts1 = new TimeSpan(00, 00, 0);
                DateTime DateStart = _startDate.Date + ts1;













                try { dtSalesData.Clear(); } catch { };

                try { dsDebiCheckTrackingTSRReportData.Clear(); } catch { };
                if (_selectedAgents.Count > 0)
                {

                    string CampaignIDStringSales = "";
                    foreach (System.Data.DataRow item in _selectedAgents)
                    {
                        long? agentIDSales = item.ItemArray[0] as long?;
                        CampaignIDStringSales = CampaignIDStringSales + agentIDSales.ToString() + ",";
                    }
                    CampaignIDStringSales = CampaignIDStringSales.Remove(CampaignIDStringSales.Length - 1, 1);

                    DataTable dtTempSalesData = null;

                    DataSet ds = null;
                    var transactionOptions = new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                    };

                    using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                    {
                        ds = Business.Insure.INReportDebiChecKTrackingCampaignsUpgrades(DateEnd, _startDate, CampaignIDStringSales);
                    }

                    dtTempSalesData = ds.Tables[0];
                    foreach (DataRow row in dtTempSalesData.Rows)
                    {
                        dtSalesData.Rows.Add(row.ItemArray);
                    }




                    //foreach loop for the campiagns that dont have any sales
                    foreach (System.Data.DataRow drAgent in _campaignsWithNoSales)
                    {
                        long? campaignID = drAgent.ItemArray[0] as long?;

                        StringBuilder strQueryAccountype = new StringBuilder();
                        strQueryAccountype.Append("SELECT TOP 1 Name [Response] ");
                        strQueryAccountype.Append("FROM INCampaign ");
                        strQueryAccountype.Append("WHERE ID = " + campaignID.ToString());
                        DataTable dtBranchCode = Methods.GetTableData(strQueryAccountype.ToString());

                        string CampaignName = dtBranchCode.Rows[0]["Response"].ToString();

                        dtSalesData.Rows.Add(CampaignName, null, null, null, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    }
                }

                #endregion Get the report data

                try
                {
                    string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


                    string filePathAndName = String.Format("{0}DebiCheck Report Upgrades ({1}), {2}.xlsx", GlobalSettings.UserFolder, campaign, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
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
                    workSheet.Cells[totalrows, 26].Formula = string.Format("=E" + totalrows + "/(B" + totalrows + "-Y" + totalrows + ")*100"); //X


                    workSheet.UsedRange.Select();
                    workSheet.Sort.SortFields.Clear();
                    workSheet.Sort.SortFields.Add(workSheet.UsedRange.Columns["A"], Microsoft.Office.Interop.Excel.XlSortOn.xlSortOnValues, Microsoft.Office.Interop.Excel.XlSortOrder.xlAscending, System.Type.Missing, Microsoft.Office.Interop.Excel.XlSortDataOption.xlSortNormal);
                    var sort = workSheet.Sort;
                    sort.SetRange(workSheet.UsedRange);
                    sort.Header = Microsoft.Office.Interop.Excel.XlYesNoGuess.xlYes;
                    sort.MatchCase = false;
                    sort.Orientation = Microsoft.Office.Interop.Excel.XlSortOrientation.xlSortColumns;
                    sort.SortMethod = Microsoft.Office.Interop.Excel.XlSortMethod.xlPinYin;
                    sort.Apply();

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





                    BackgroundWorker worker = new BackgroundWorker();

                    if (BaseCB.IsChecked == true)
                    {

                        var transactionOptions = new TransactionOptions
                        {
                            IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                        };

                        using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                        {
                            dsDiaryReportData = Business.Insure.INGetDebiCheckTracking(_startDat2, _endDate2);
                        }

                        worker.DoWork += ReportConsolidated;

                    }
                    else if (AcceptedCB.IsChecked == true)
                    {

                        var transactionOptions = new TransactionOptions
                        {
                            IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                        };

                        using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                        {
                            dsDiaryReportData = Business.Insure.INGetDebiCheckTrackingAccepted(_startDat2, _endDate2);
                        }
                        worker.DoWork += ReportConsolidatedAccepted;

                    }
                    else
                    {
                        #region Campaign Selections
                        TimeSpan tsU = new TimeSpan(23, 00, 0);
                        DateTime DateEnd = _endDate.Date + tsU;

                        TimeSpan ts1U = new TimeSpan(00, 00, 0);
                        DateTime DateStart = _startDate.Date + ts1U;
                        //This gets all of the Campaign IDS that have sales attached
                        strQuery = "";
                        if (Upgrades13CB.IsChecked == true)
                        {
                            strQuery = "SELECT DISTINCT [C].[ID] FROM INCampaign as [C]";
                            strQuery += "LEFT JOIN [INImport] AS [I] ON [I].[FKINCampaignID] = [C].[ID]";
                            strQuery += "LEFT JOIN [lkpINCampaignGroup] AS LCP ON C.FKINCampaignGroupID = LCP.ID ";
                            strQuery += "where[LCP].ID IN(7, 8, 9, 14, 15, 16)";
                            strQuery += "AND [I].[FKINLeadStatusID] = 1 AND [I].DateOfSale BETWEEN '" + DateStart.ToString() + "' AND '" + DateEnd.ToString() + "'";
                        }
                        else
                        {
                            strQuery = "SELECT DISTINCT [C].[ID] FROM INCampaign as [C]";
                            strQuery += "LEFT JOIN [INImport] AS [I] ON [I].[FKINCampaignID] = [C].[ID]";
                            strQuery += "where [C].[Code] like '%u%' and [C].[Code] != 'PLMBSPOUSE' and [C].[Code] != 'PLULCBE' and [C].[Name] != 'Bump-Up Base' and[C].[Name] != 'Call Monitoring Upgrades' and [C].[Name] != 'Confirmation Upgrades' and [C].[Name] != 'PL Cancer Base Spouse'";
                            strQuery += "AND [I].[FKINLeadStatusID] = 1 AND [I].DateOfSale BETWEEN '" + DateStart.ToString() + "' AND '" + DateEnd.ToString() + "'";
                        }

                        DataTable dtAgents = Methods.GetTableData(strQuery);
                        DataSet dsAgents = new DataSet();

                        dsAgents.Tables.Add(dtAgents);

                        _selectedAgents = dsAgents.Tables[0].AsEnumerable().ToList();

                        //This adds it to a usable format for the query to get all campaign IDs that dont have sales attached
                        string CampaignIDString = "(";
                        foreach (System.Data.DataRow item in _selectedAgents)
                        {
                            long? agentID = item.ItemArray[0] as long?;
                            CampaignIDString = CampaignIDString + agentID.ToString() + ",";
                        }
                        CampaignIDString = CampaignIDString.Remove(CampaignIDString.Length - 1, 1);
                        CampaignIDString = CampaignIDString + ")";

                        // query for campiagns that dont have sales
                        string strQuery2;
                        if (Upgrades13CB.IsChecked == true)
                        {
                            strQuery2 = "SELECT DISTINCT [C].[ID] FROM INCampaign as [C]";
                            strQuery2 += "LEFT JOIN [lkpINCampaignGroup] AS LCP ON C.FKINCampaignGroupID = LCP.ID  where [LCP].ID IN (7, 8, 9, 14, 15, 16)";
                            strQuery2 += "AND [C].[IsActive] = 1 AND [C].[ID] NOT IN " + CampaignIDString;
                        }
                        else
                        {
                            strQuery2 = "SELECT DISTINCT [C].[ID] FROM INCampaign as [C]";
                            strQuery2 += "where [C].[Code] like '%u%' and [C].[Code] != 'PLMBSPOUSE' and [C].[Code] != 'PLULCBE' and [C].[Name] != 'Bump-Up Base' and[C].[Name] != 'Call Monitoring Upgrades' and [C].[Name] != 'Confirmation Upgrades' and [C].[Name] != 'PL Cancer Base Spouse'";
                            strQuery2 += "AND [C].[IsActive] = 1 AND [C].[ID] NOT IN " + CampaignIDString;
                        }

                        DataTable dtNoSaleCampaignIDs = Methods.GetTableData(strQuery2);
                        DataSet dsNoSaleCampaignIDs = new DataSet();

                        dsNoSaleCampaignIDs.Tables.Add(dtNoSaleCampaignIDs);
                        try { _campaignsWithNoSales.Clear(); } catch { }
                        _campaignsWithNoSales = dsNoSaleCampaignIDs.Tables[0].AsEnumerable().ToList();
                        #endregion

                        worker.DoWork += ReportConsolidatedUpgrades;
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

        #region CheckBoxes click events
        private void UpgradeCB_Checked(object sender, RoutedEventArgs e)
        {
            BaseCB.IsChecked = false;
            Upgrades13Lbl.Visibility = Visibility.Visible;
            Upgrades13CB.Visibility = Visibility.Visible;
        }
        private void UpgradeCB_Unchecked(object sender, RoutedEventArgs e)
        {
            Upgrades13Lbl.Visibility = Visibility.Visible;
            Upgrades13CB.Visibility = Visibility.Visible;
        }
        private void BaseCB_Checked(object sender, RoutedEventArgs e)
        {
            UpgradeCB.IsChecked = false;
        }

        private void AcceptedCB_Checked(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void ReportConsolidatedAccepted(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Get the report data
                DataTable dtSalesData;
                DataTable dtTotalSalesData;

                dtSalesData = dsDiaryReportData.Tables[0];
                dtTotalSalesData = dsDiaryReportData.Tables[2];

                #endregion Get the report data

                try
                {
                    string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


                    string filePathAndName = String.Format("{0}DebiCheck Tracking Report Accepted ({1}), {2}.xlsx", GlobalSettings.UserFolder, campaign, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
                    if (dtSalesData == null || dtSalesData.Columns.Count == 0)
                        throw new Exception("ExportToExcel: Null or empty input table!\n");

                    // load excel, and create a new workbook
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Workbooks.Add();

                    // single worksheet
                    Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;

                    workSheet.Cells[1, 0 + 1] = "Date Range : " + _startDate.ToLongDateString() + " to " + _endDate.ToLongDateString();
                    for (var i = 0; i < dtSalesData.Columns.Count; i++)
                    {

                        workSheet.Cells[2, i + 1].Font.Bold = true;

                        workSheet.Cells[2, i + 1].ColumnWidth = 30;

                        workSheet.Cells[2, i + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
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



                    //workSheet.Cells[28, 2].Value = int.Parse(totalTable.Rows[0][0].ToString());
                    //workSheet.Cells[28, 1].Value = "Base DebiCheck Percentage :";

                    //workSheet.Cells[29, 2].Value = int.Parse(totalTable.Rows[0][0].ToString());
                    //workSheet.Cells[29, 1].Value = "Upgrade DebiCheck Percentage :";

                    //workSheet.Cells[30, 2].Value = int.Parse(totalTable.Rows[0][0].ToString());
                    //workSheet.Cells[30, 1].Value = "Combined DebiCheck Percentage :";

                    //for (var i = 0; i < dtTotalSalesData.Columns.Count; i++)
                    //{

                    //    workSheet.Cells[28, i + 1].Font.Bold = true;

                    //    workSheet.Cells[2, i + 1].ColumnWidth = 30;

                    //    workSheet.Cells[2, i + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    //    //workSheet.get_Range("A4", "J1").Font.Bold = true;
                    //}


                    // column headings
                    //for (var i = 0; i < dtTotalSalesData.Columns.Count; i++)
                    //{
                    //    workSheet.Cells[28, i + 1] = dtTotalSalesData.Columns[i].ColumnName;
                    //}

                    // rows
                    for (var i = 1; i < dtTotalSalesData.Rows.Count + 1; i++)
                    {
                        // to do: format datetime values before printing
                        for (var j = 0; j < dtTotalSalesData.Columns.Count; j++)
                        {
                            workSheet.Cells[i + 27, j + 1] = dtTotalSalesData.Rows[i - 1][j];


                        }
                    }


                    (workSheet.Cells[28, 2]).EntireColumn.NumberFormat = "##%";
                    (workSheet.Cells[28, 3]).EntireColumn.NumberFormat = "##%";
                    (workSheet.Cells[28, 4]).EntireColumn.NumberFormat = "##%";
                    (workSheet.Cells[28, 5]).EntireColumn.NumberFormat = "##%";
                    (workSheet.Cells[28, 6]).EntireColumn.NumberFormat = "##%";

                    (workSheet.Cells[29, 2]).EntireColumn.NumberFormat = "##%";
                    (workSheet.Cells[29, 3]).EntireColumn.NumberFormat = "##%";
                    (workSheet.Cells[29, 4]).EntireColumn.NumberFormat = "##%";
                    (workSheet.Cells[29, 5]).EntireColumn.NumberFormat = "##%";
                    (workSheet.Cells[29, 6]).EntireColumn.NumberFormat = "##%";

                    (workSheet.Cells[30, 2]).EntireColumn.NumberFormat = "##%";
                    (workSheet.Cells[30, 3]).EntireColumn.NumberFormat = "##%";
                    (workSheet.Cells[30, 4]).EntireColumn.NumberFormat = "##%";
                    (workSheet.Cells[30, 5]).EntireColumn.NumberFormat = "##%";
                    (workSheet.Cells[30, 6]).EntireColumn.NumberFormat = "##%";



                    workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 6]].Merge();

                    workSheet.Rows[2].WrapText = true;

                    Microsoft.Office.Interop.Excel.Range tRange = workSheet.UsedRange;
                    tRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    tRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                    workSheet.get_Range("A2", "F2").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);



                    workSheet.get_Range("A2", "F27").BorderAround(
                    Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);



                    //workSheet.Range["B3", "B26"].Interior.Color = System.Drawing.Color.LightGray;
                    workSheet.Range["C3", "C26"].Interior.Color = System.Drawing.Color.LightGray;
                    workSheet.Range["D3", "D26"].Interior.Color = System.Drawing.Color.LightGray;
                    workSheet.Range["E3", "E26"].Interior.Color = System.Drawing.Color.LightGray;
                    workSheet.Range["F3", "F26"].Interior.Color = System.Drawing.Color.LightGray;

                    workSheet.Range["C27", "F27"].Interior.Color = System.Drawing.Color.LightBlue;



                    #region Page formulas


                    //workSheet.Cells[28, 2].Formula = string.Format("=ROUNDUP(AVERAGE(B1:B26),2)"); //B
                    //workSheet.Cells[28, 3].Formula = string.Format("=ROUNDUP(AVERAGE(C1:C26),2)"); //C
                    //workSheet.Cells[28, 4].Formula = string.Format("=ROUNDUP(AVERAGE(D1:D26),2)"); //D
                    //workSheet.Cells[28, 5].Formula = string.Format("=ROUNDUP(AVERAGE(E1:E26),2)"); //E
                    //workSheet.Cells[28, 6].Formula = string.Format("=ROUNDUP(AVERAGE(F1:F26),2)"); //G

                    //workSheet.Cells[29, 2].Formula = string.Format("=SUM(B27:B27)"); //B
                    //workSheet.Cells[29, 3].Formula = string.Format("=SUM(C27:C27)"); //C
                    //workSheet.Cells[29, 4].Formula = string.Format("=SUM(D27:D27)"); //D
                    //workSheet.Cells[29, 5].Formula = string.Format("=SUM(E27:E27)"); //E
                    //workSheet.Cells[29, 6].Formula = string.Format("=SUM(F27:F27)"); //G

                    //workSheet.Cells[30, 2].Formula = string.Format("=SUM(B28:B29)"); //B
                    //workSheet.Cells[30, 3].Formula = string.Format("=SUM(C28:C29)"); //C
                    //workSheet.Cells[30, 4].Formula = string.Format("=SUM(D28:D29)"); //D
                    //workSheet.Cells[30, 5].Formula = string.Format("=SUM(E28:E29)"); //E
                    //workSheet.Cells[30, 6].Formula = string.Format("=SUM(F28:F29)"); //G



                    #endregion

                    // check file path

                    excelApp.Visible = true;
                    excelApp.Workbooks.Item[1].SaveAs(filePathAndName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);


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

        private void Upgrades13CB_Checked(object sender, RoutedEventArgs e)
        {

        }


    }
}
