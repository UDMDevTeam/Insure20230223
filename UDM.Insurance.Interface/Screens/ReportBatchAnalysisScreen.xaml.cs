using System.Data.SqlClient;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Windows.DataPresenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Infragistics.Documents.Excel;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using Prism.Regions;
using Workbook = Microsoft.Office.Interop.Excel.Workbook;
using Microsoft.Office.Interop.Word;
using System.Linq.Expressions;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportBatchAnalysisScreen
    {

        #region Constants

        //private string _fontName = "Calibri";
        //private const int _fontSize = 10;
        //private const int _pointsToTwipsFactor = 20;
        //private const int _fontHeight = _fontSize * _pointsToTwipsFactor;

        #endregion

        #region Private Members

        private System.Windows.Controls.CheckBox _xdgHeaderPrefixAreaCheckbox;
        //private RecordCollectionBase _campaigns;
        private DateTime _startDate;
        private DateTime _endDate;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        private byte _reportTypeID = 0;

        private List<Record> _lstSelectedFKINCampaignIDs;
        private string _campaignIDs = String.Empty;

        #endregion

        #region Constructors

        public ReportBatchAnalysisScreen()
        {
            InitializeComponent();
            LoadCampaignInfo();
            LoadLookups();
            cmbReportType.SelectedIndex = 0;
            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion

        #region Private Methods

        private bool? AllRecordsSelected()
        {
            try
            {
                bool allSelected = true;
                bool noneSelected = true;

                if (xdgCampaigns.DataSource != null)
                {
                    foreach (DataRow dr in ((DataView)xdgCampaigns.DataSource).Table.Rows)
                    {
                        allSelected = allSelected && (bool)dr["Select"];
                        noneSelected = noneSelected && !(bool)dr["Select"];
                    }
                }

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
                HandleException(ex);
                return null;
            }
        }

        private void LoadCampaignInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);

                System.Data.DataTable dt = Methods.GetTableData("SELECT ID AS [CampaignID], Name AS [CampaignName], Code AS [CampaignCode] FROM INCampaign");
                DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                dt.Columns.Add(column);
                dt.DefaultView.Sort = "CampaignName ASC";
                xdgCampaigns.DataSource = dt.DefaultView;




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

        private void EnableDisableExportButton()
        {
            try
            {
                if (calStartDate.SelectedDate != null && (calEndDate.SelectedDate != null)) //&& (calEndDate.SelectedDate >= Cal1.SelectedDate)
                {
                    if (_xdgHeaderPrefixAreaCheckbox != null && (_xdgHeaderPrefixAreaCheckbox.IsChecked == true || _xdgHeaderPrefixAreaCheckbox.IsChecked == null))
                    {
                        btnReport.IsEnabled = true;
                        return;
                    }
                }

                btnReport.IsEnabled = true;
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
            xdgCampaigns.IsEnabled = true;
            calStartDate.IsEnabled = true;
            calEndDate.IsEnabled = true;
        }

        #region ReportOLD(object sender, DoWorkEventArgs e)

        private void SeparateExcelFiles(IEnumerable<DataRecord> campaigns)
        {
            try

            {

                SetCursor(Cursors.Wait);

                Excel.Application xlApp = new Excel.Application();

                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                Excel.Range xlRange;

                object misValue = System.Reflection.Missing.Value;

                int? reportRowTotal = 0;

                xlWorkBook = xlApp.Workbooks.Add(misValue);

                int rowIndex = 0;
                int formulaFromRow = 0;
                int formulaToRow = 0;
                int tableFirstRow = 0;
                int reportColumnCount = 67;//66//57//39
                int totalSelectedCampaigns = 0;

                if (campaigns != null)
                {
                    var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                    _lstSelectedFKINCampaignIDs = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignID"].Value));

                    if (_lstSelectedFKINCampaignIDs.Count > 0)
                    {
                        _campaignIDs = _lstSelectedFKINCampaignIDs.Cast<DataRecord>().Where(r => (bool)r.Cells["Select"].Value).Aggregate(String.Empty, (current, r) => current + r.Cells["CampaignID"].Value + ",");
                        _campaignIDs = _campaignIDs.Substring(0, _campaignIDs.Length - 1);

                       
                    }

                       

                    rowIndex = 8;
                    formulaFromRow = 8;
                    formulaToRow = 0;
                    tableFirstRow = 6;

                        foreach (DataRecord record in _lstSelectedFKINCampaignIDs)
                        {

                        //if ((bool)record.Cells["Select"].Value)
                        //{
                        //long campaignID = Convert.ToInt32(record.Cells["CampaignID"].Value);

                        System.Data.DataTable dtLeadAllocationData =  null;
                        System.Data.DataTable dtTotals = null;

                        string campaignID = record.Cells["CampaignID"].Value.ToString();

                        SqlParameter[] parameters = new SqlParameter[3];
                        parameters[0] = new SqlParameter("@CampaignIDs", campaignID);
                        parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
                        parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

                        DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spINReportIncomingBatchAnalysis", parameters);



                            dtLeadAllocationData = dsLeadAllocationData.Tables[0];
                            dtTotals = dsLeadAllocationData.Tables[1];

                            string campaignName = record.Cells["CampaignName"].Value.ToString();
                            string campaignCode = record.Cells["CampaignCode"].Value.ToString();


                            //if (_lstSelectedFKINCampaignIDs.Count <= 1)
                            //{

                            //}
                            //if(_lstSelectedFKINCampaignIDs.Count < 2)
                            //{
                            //    rowIndex++;
                            //}

                            #region Setup excel documents

                            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                            string CampaignInfo = "All Campaign Information";

                            xlWorkSheet.Name = CampaignInfo;

                            #endregion Setup excel documents

                            #region Get report data from database



                            //if (dsLeadAllocationData != null)
                            //{
                            //    if (dsLeadAllocationData.Tables[0].Rows.Count > 0)
                            //    {
                            //        dtLeadAllocationData = dsLeadAllocationData.Tables[0].AsEnumerable().OrderByDescending(x => x["ImportDate"]).CopyToDataTable();

                            //    }
                            //    else
                            //    {
                            //        dtLeadAllocationData = dsLeadAllocationData.Tables[0];
                            //    }

                            //    dtTotals = dsLeadAllocationData.Tables[1];

                            //    if (dtLeadAllocationData.Rows.Count == 0)
                            //    {
                            //        if (campaigns.Count() == 1)
                            //        {
                            //            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                            //            {
                            //                ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the Campaign and specified Date range.", "No Data", ShowMessageType.Information);
                            //            });

                            //            continue;
                            //        }
                            //    }
                            //}
                            //else
                            //{

                            //    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                            //    {
                            //        ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the Campaign and specified Date range.", "No Data", ShowMessageType.Information);
                            //    });

                            //    continue;

                            //}

                           

                            #endregion Get report data from database


                            //foreach (DataRecord data in campaigns)
                            //{
                            //    if ((bool)data.Cells["Select"].Value)
                            //    {
                            #region report data

                            //{

                            #region Adding the report heading

                            Excel.Range h1 = xlWorkSheet.Cells[1, 2];
                            Excel.Range h2 = xlWorkSheet.Cells[1, reportColumnCount];
                            Excel.Range headingRange = (Excel.Range)xlWorkSheet.get_Range(h1, h2);

                            headingRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            headingRange.Font.Bold = true;
                            headingRange.Font.Size = 16;
                            headingRange.Borders.Weight = Excel.XlBorderWeight.xlThick;
                            headingRange.Merge(true);
                            xlWorkSheet.Cells[1, 2] = string.Format("Batch Analysis Report - {0}", CampaignInfo);

                            #endregion Adding the report heading

                            #region Adding the report subheading

                            Excel.Range sh1 = xlWorkSheet.Cells[3, 1];
                            Excel.Range sh2 = xlWorkSheet.Cells[3, reportColumnCount];
                            Excel.Range subheadingRange = (Excel.Range)xlWorkSheet.get_Range(sh1, sh2);

                            subheadingRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            subheadingRange.Font.Bold = true;
                            //subheadingRange.Font.Size = 11;
                            //headingRange.Borders.Weight = Excel.XlBorderWeight.xlThick;
                            subheadingRange.Merge(true);
                            xlWorkSheet.Cells[3, 1] = string.Format("For the period between {0} and {1}", _startDate.ToString("d MMMM yyyy"), _endDate.ToString("d MMMM yyyy"));

                            #endregion Adding the report subheading

                            #region Adding the report date

                            Excel.Range d1 = xlWorkSheet.Cells[4, 63];
                            Excel.Range d2 = xlWorkSheet.Cells[4, 64];
                            Excel.Range dateLabelCellRange = (Excel.Range)xlWorkSheet.get_Range(d1, d2);
                            dateLabelCellRange.Merge(true);
                            dateLabelCellRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                            dateLabelCellRange.Font.Bold = true;

                            Excel.Range d3 = xlWorkSheet.Cells[4, 65];
                            Excel.Range d4 = xlWorkSheet.Cells[4, 66];
                            Excel.Range dateCellRange = (Excel.Range)xlWorkSheet.get_Range(d3, d4);
                            dateCellRange.Merge(true);

                            xlWorkSheet.Cells[4, 63] = "Date Generated:";
                            xlWorkSheet.Cells[4, 65] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            #endregion Adding the report date

                            #region Partitioning the columns

                            //headers
                            xlWorkSheet.Cells[tableFirstRow, 5] = "Contact Numbers";
                            xlWorkSheet.Cells[tableFirstRow, 14] = "ID Numbers vs Date of Birth";
                            xlWorkSheet.Cells[tableFirstRow, 30] = "Pensioners & Housewives";
                            xlWorkSheet.Cells[tableFirstRow, 34] = "Re-Primed Leads";
                            xlWorkSheet.Cells[tableFirstRow, 37] = "Indian Leads";
                            xlWorkSheet.Cells[tableFirstRow, 40] = "African Leads";
                            xlWorkSheet.Cells[tableFirstRow, 43] = "Re-Marketed Leads";
                            xlWorkSheet.Cells[tableFirstRow, 46] = "Number of Times Re-Marketed to";

                            //new headers
                            xlWorkSheet.Cells[tableFirstRow, 58] = "Next of Kin Contact Details";
                            xlWorkSheet.Cells[tableFirstRow, 61] = "Beneficiary Contact Details";
                            xlWorkSheet.Cells[tableFirstRow, 64] = "LA2 Contact Details";

                            Excel.Range c1 = xlWorkSheet.Cells[tableFirstRow, 5];
                            Excel.Range c2 = xlWorkSheet.Cells[tableFirstRow, 13];
                            Excel.Range c3 = xlWorkSheet.Cells[tableFirstRow, 14];
                            Excel.Range c4 = xlWorkSheet.Cells[tableFirstRow, 19];

                            Excel.Range c5 = xlWorkSheet.Cells[tableFirstRow, 30];
                            Excel.Range c6 = xlWorkSheet.Cells[tableFirstRow, 33];

                            Excel.Range c7 = xlWorkSheet.Cells[tableFirstRow, 34];
                            Excel.Range c8 = xlWorkSheet.Cells[tableFirstRow, 36];

                            Excel.Range c9 = xlWorkSheet.Cells[tableFirstRow, 37];
                            Excel.Range c10 = xlWorkSheet.Cells[tableFirstRow, 39];

                            Excel.Range c11 = xlWorkSheet.Cells[tableFirstRow, 40];
                            Excel.Range c12 = xlWorkSheet.Cells[tableFirstRow, 42];

                            Excel.Range c13 = xlWorkSheet.Cells[tableFirstRow, 43];
                            Excel.Range c14 = xlWorkSheet.Cells[tableFirstRow, 45];

                            Excel.Range c15 = xlWorkSheet.Cells[tableFirstRow, 46];
                            Excel.Range c16 = xlWorkSheet.Cells[tableFirstRow, 57];
                            //****************************************************/
                            Excel.Range c17 = xlWorkSheet.Cells[tableFirstRow, 58];
                            Excel.Range c18 = xlWorkSheet.Cells[tableFirstRow, 60];

                            Excel.Range c19 = xlWorkSheet.Cells[tableFirstRow, 61];
                            Excel.Range c20 = xlWorkSheet.Cells[tableFirstRow, 63];

                            Excel.Range c21 = xlWorkSheet.Cells[tableFirstRow, 64];
                            Excel.Range c22 = xlWorkSheet.Cells[tableFirstRow, 67];

                            Excel.Range range = (Excel.Range)xlWorkSheet.get_Range(c1, c2);
                            Excel.Range range2 = (Excel.Range)xlWorkSheet.get_Range(c3, c4);
                            Excel.Range range3 = (Excel.Range)xlWorkSheet.get_Range(c5, c6);
                            Excel.Range range4 = (Excel.Range)xlWorkSheet.get_Range(c7, c8);
                            Excel.Range range5 = (Excel.Range)xlWorkSheet.get_Range(c9, c10);
                            Excel.Range range6 = (Excel.Range)xlWorkSheet.get_Range(c11, c12);
                            Excel.Range range7 = (Excel.Range)xlWorkSheet.get_Range(c13, c14);
                            Excel.Range range8 = (Excel.Range)xlWorkSheet.get_Range(c15, c16);

                            Excel.Range range9 = (Excel.Range)xlWorkSheet.get_Range(c17, c18);
                            Excel.Range range10 = (Excel.Range)xlWorkSheet.get_Range(c19, c20);
                            Excel.Range range11 = (Excel.Range)xlWorkSheet.get_Range(c21, c22);

                            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range.Font.Bold = true;
                            //range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                            range.Merge(true);

                            range2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range2.Font.Bold = true;
                            //range2.Borders.Weight = Excel.XlBorderWeight.xlThin;
                            range2.Merge(true);

                            range3.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range3.Font.Bold = true;
                            range3.Merge(true);

                            range4.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range4.Font.Bold = true;
                            range4.Merge(true);

                            range5.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range5.Font.Bold = true;
                            range5.Merge(true);

                            range6.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range6.Font.Bold = true;
                            range6.Merge(true);

                            range7.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range7.Font.Bold = true;
                            range7.Merge(true);

                            range8.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range8.Font.Bold = true;
                            range8.Merge(true);
                            //************************************************************/

                            range9.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range9.Font.Bold = true;
                            range9.Merge(true);

                            range10.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range10.Font.Bold = true;
                            range10.Merge(true);

                            range11.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range11.Font.Bold = true;
                            range11.Merge(true);

                            #endregion Partitioning the columns

                            #region Adding the column headings

                            for (int i = 1; i <= reportColumnCount; i++)
                            {
                                if (i == 1)
                                {
                                    xlWorkSheet.Cells[tableFirstRow + 1, i].Interior.ColorIndex = 2;
                                }
                                if (i >= 2 && i <= 5)
                                {
                                    xlWorkSheet.Cells[tableFirstRow + 1, i].Interior.ColorIndex = 35;
                                    xlWorkSheet.Cells[tableFirstRow, i].Interior.ColorIndex = 35;
                                }
                                if (i >= 5 && i <= 13)
                                {
                                    xlWorkSheet.Cells[tableFirstRow + 1, i].Interior.ColorIndex = 34;
                                    xlWorkSheet.Cells[tableFirstRow, i].Interior.ColorIndex = 34;
                                }
                                if (i >= 14 && i <= 19)
                                {
                                    xlWorkSheet.Cells[tableFirstRow + 1, i].Interior.ColorIndex = 36;
                                    xlWorkSheet.Cells[tableFirstRow, i].Interior.ColorIndex = 36;
                                }
                                if (i >= 20 && i <= 25)
                                {
                                    xlWorkSheet.Cells[tableFirstRow + 1, i].Interior.ColorIndex = 45;
                                    xlWorkSheet.Cells[tableFirstRow, i].Interior.ColorIndex = 45;
                                }
                                if (i == 26)
                                {
                                    xlWorkSheet.Cells[tableFirstRow + 1, i].Interior.ColorIndex = 33;
                                    xlWorkSheet.Cells[tableFirstRow, i].Interior.ColorIndex = 33;
                                }
                                if (i >= 27 && i <= 28)
                                {
                                    xlWorkSheet.Cells[tableFirstRow + 1, i].Interior.ColorIndex = 39;
                                    xlWorkSheet.Cells[tableFirstRow, i].Interior.ColorIndex = 39;
                                }
                                if (i >= 30 && i <= 33)
                                {
                                    xlWorkSheet.Cells[tableFirstRow + 1, i].Interior.ColorIndex = 40;
                                    xlWorkSheet.Cells[tableFirstRow, i].Interior.ColorIndex = 40;
                                }

                                if (i >= 34 && i <= 36)
                                {
                                    xlWorkSheet.Cells[tableFirstRow + 1, i].Interior.ColorIndex = 35; //light green
                                    xlWorkSheet.Cells[tableFirstRow, i].Interior.ColorIndex = 35;
                                }

                                if (i >= 37 && i <= 39)
                                {
                                    xlWorkSheet.Cells[tableFirstRow + 1, i].Interior.ColorIndex = 34; //cyan
                                    xlWorkSheet.Cells[tableFirstRow, i].Interior.ColorIndex = 34;
                                }

                                if (i >= 40 && i <= 42)
                                {
                                    xlWorkSheet.Cells[tableFirstRow + 1, i].Interior.ColorIndex = 15;
                                    xlWorkSheet.Cells[tableFirstRow, i].Interior.ColorIndex = 15;
                                }

                                if (i >= 43 && i <= 45)
                                {
                                    xlWorkSheet.Cells[tableFirstRow + 1, i].Interior.ColorIndex = 24; //light purple
                                    xlWorkSheet.Cells[tableFirstRow, i].Interior.ColorIndex = 24;
                                }

                                if (i >= 46 && i <= 57)
                                {
                                    xlWorkSheet.Cells[tableFirstRow + 1, i].Interior.ColorIndex = 19;
                                    xlWorkSheet.Cells[tableFirstRow, i].Interior.ColorIndex = 19;
                                }
                                //*********************************************************/
                                if (i >= 58 && i <= 60)
                                {
                                    xlWorkSheet.Cells[tableFirstRow + 1, i].Interior.ColorIndex = 35;
                                    xlWorkSheet.Cells[tableFirstRow, i].Interior.ColorIndex = 35;
                                }

                                if (i >= 61 && i <= 63)
                                {
                                    xlWorkSheet.Cells[tableFirstRow + 1, i].Interior.ColorIndex = 34;
                                    xlWorkSheet.Cells[tableFirstRow, i].Interior.ColorIndex = 34;
                                }

                                if (i >= 64 && i <= 66)
                                {
                                    xlWorkSheet.Cells[tableFirstRow + 1, i].Interior.ColorIndex = 24;
                                    xlWorkSheet.Cells[tableFirstRow, i].Interior.ColorIndex = 24;
                                }
                                if (i == 67)
                                {
                                    xlWorkSheet.Cells[tableFirstRow + 1, i].Interior.ColorIndex = 24;
                                    xlWorkSheet.Cells[tableFirstRow, i].Interior.ColorIndex = 24;
                                }


                                //xlWorkSheet.Cells[tableFirstRow + 1, i].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                            }

                            xlWorkSheet.Cells[tableFirstRow + 1, 1] = "Campaign Code";
                            xlWorkSheet.Cells[tableFirstRow + 1, 2] = "PL Disc Number";
                            xlWorkSheet.Cells[tableFirstRow + 1, 3] = "UDM Batch Number";
                            xlWorkSheet.Cells[tableFirstRow + 1, 4] = "Import Date";
                            xlWorkSheet.Cells[tableFirstRow + 1, 5] = "# Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 6] = "1 X Tel No.";
                            xlWorkSheet.Cells[tableFirstRow + 1, 7] = "%";
                            xlWorkSheet.Cells[tableFirstRow + 1, 8] = "% STL";
                            xlWorkSheet.Cells[tableFirstRow + 1, 9] = "2 X Tel No.";
                            xlWorkSheet.Cells[tableFirstRow + 1, 10] = "%";
                            xlWorkSheet.Cells[tableFirstRow + 1, 11] = "% STL";
                            xlWorkSheet.Cells[tableFirstRow + 1, 12] = "3 X Tel No.";
                            xlWorkSheet.Cells[tableFirstRow + 1, 13] = "%";
                            xlWorkSheet.Cells[tableFirstRow + 1, 14] = "% STL";

                            xlWorkSheet.Cells[tableFirstRow + 1, 15] = "ID Numbers";
                            xlWorkSheet.Cells[tableFirstRow + 1, 16] = "%";
                            xlWorkSheet.Cells[tableFirstRow + 1, 17] = "% STL";

                            xlWorkSheet.Cells[tableFirstRow + 1, 18] = "No ID #";
                            xlWorkSheet.Cells[tableFirstRow + 1, 19] = "%";
                            xlWorkSheet.Cells[tableFirstRow + 1, 20] = "% STL";

                            xlWorkSheet.Cells[tableFirstRow + 1, 21] = "ID No's Given by 1x Tel No. Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 22] = "% of ID No's Given by 1 X Tel No. Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 23] = "% STL";
                            xlWorkSheet.Cells[tableFirstRow + 1, 24] = "ID No's Given by 2x and 3x Tel No. Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 25] = "% of ID No's Given by 2x and 3x Tel No. Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 26] = "% STL";
                            xlWorkSheet.Cells[tableFirstRow + 1, 27] = "% Email Addresses";
                            xlWorkSheet.Cells[tableFirstRow + 1, 28] = "Days from commencement of free cover - when client receives the gift - to UDM receiving the leads"; //"Average Age of the Leads: From Date of Delivery of Gift Pack and Start of Free Cover (Days)";
                            xlWorkSheet.Cells[tableFirstRow + 1, 29] = "Average days since the generation of the lead";
                            xlWorkSheet.Cells[tableFirstRow + 1, 30] = "AVG Age of Clients";

                            xlWorkSheet.Cells[tableFirstRow + 1, 31] = "Pensioners";
                            xlWorkSheet.Cells[tableFirstRow + 1, 32] = "% Pensioners";
                            xlWorkSheet.Cells[tableFirstRow + 1, 33] = "Housewives";
                            xlWorkSheet.Cells[tableFirstRow + 1, 34] = "% Housewives";

                            xlWorkSheet.Cells[tableFirstRow + 1, 35] = "Number of Re-Primed Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 36] = "% Number of Re-Primed Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 37] = "% STL";

                            xlWorkSheet.Cells[tableFirstRow + 1, 38] = "Number of Indian Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 39] = "% of Indian Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 40] = "% STL";

                            xlWorkSheet.Cells[tableFirstRow + 1, 41] = "Number of African Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 42] = "% of African Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 43] = "% STL";

                            xlWorkSheet.Cells[tableFirstRow + 1, 44] = "Number of Re-Marketed Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 45] = "% of Re-Marketed Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 46] = "% STL";

                            xlWorkSheet.Cells[tableFirstRow + 1, 47] = "1";
                            xlWorkSheet.Cells[tableFirstRow + 1, 48] = "% of Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 49] = "% STL";

                            xlWorkSheet.Cells[tableFirstRow + 1, 50] = "2";
                            xlWorkSheet.Cells[tableFirstRow + 1, 51] = "% of Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 52] = "% STL";

                            xlWorkSheet.Cells[tableFirstRow + 1, 53] = "3";
                            xlWorkSheet.Cells[tableFirstRow + 1, 54] = "% of Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 55] = "% STL";

                            xlWorkSheet.Cells[tableFirstRow + 1, 56] = "4+";
                            xlWorkSheet.Cells[tableFirstRow + 1, 57] = "% of Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 58] = "% STL";

                            //********************************************/
                            xlWorkSheet.Cells[tableFirstRow + 1, 59] = "Number of Leads with Contact Details";
                            xlWorkSheet.Cells[tableFirstRow + 1, 60] = "% of Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 61] = "% STL";

                            xlWorkSheet.Cells[tableFirstRow + 1, 62] = "Number of Leads with Contact Details";
                            xlWorkSheet.Cells[tableFirstRow + 1, 63] = "% of Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 64] = "% STL";

                            xlWorkSheet.Cells[tableFirstRow + 1, 65] = "Number of Leads with Contact Details";
                            xlWorkSheet.Cells[tableFirstRow + 1, 66] = "% of Leads";
                            xlWorkSheet.Cells[tableFirstRow + 1, 67] = "% STL";

                            //xlWorkSheet.Columns[1].ColumnWidth = 20;
                            //xlWorkSheet.Columns[2].ColumnWidth = 20;
                            //xlWorkSheet.Cells[2, 1].Font.Bold = true;
                            //xlWorkSheet.Cells[2, 2].Font.Bold = true;

                            for (int i = 1; i <= reportColumnCount; i++)//formatting of all columns
                            {
                                #region Setting the column widths
                                //if (i <= 2)
                                //{
                                //    xlWorkSheet.Columns[i].ColumnWidth = 10;
                                //}
                                //else if (i == 27 || i == 28)
                                //{
                                //    xlWorkSheet.Columns[i].ColumnWidth = 15;
                                //}
                                //else if (i >= 30 || i <= 30)
                                //{
                                //    xlWorkSheet.Columns[i].ColumnWidth = 14;
                                //}
                                //else
                                //{
                                //    xlWorkSheet.Columns[i].ColumnWidth = 7.43;                                        
                                //}

                                switch (i)
                                {
                                    case 1:
                                        xlWorkSheet.Columns[i].ColumnWidth = 17;
                                        break;

                                    case 2:
                                    case 14:
                                    case 34:
                                    case 37:
                                        xlWorkSheet.Columns[i].ColumnWidth = 8.29;
                                        break;

                                    case 3:
                                    case 32:
                                    case 33:
                                        xlWorkSheet.Columns[i].ColumnWidth = 11;
                                        break;

                                    case 4:
                                    case 6:
                                    case 7:
                                    case 9:
                                    case 10:
                                    case 12:
                                    case 13:
                                    case 15:
                                    case 16:
                                    case 18:
                                    case 19:
                                    case 22:
                                    case 25:
                                    case 35:
                                    case 36:
                                    case 38:
                                    case 39:
                                        xlWorkSheet.Columns[i].ColumnWidth = 11.00;
                                        break;

                                    case 5:
                                    case 8:
                                    case 11:
                                    case 30:
                                    case 31:
                                    case 43:
                                    case 44:
                                    case 45:
                                        xlWorkSheet.Columns[i].ColumnWidth = 10.14;
                                        break;

                                    case 17:
                                    case 27:
                                        xlWorkSheet.Columns[i].ColumnWidth = 14;
                                        break;

                                    case 20:
                                    case 21:
                                    case 28:
                                        xlWorkSheet.Columns[i].ColumnWidth = 12.43;
                                        break;

                                    case 24:
                                        xlWorkSheet.Columns[i].ColumnWidth = 12.86;
                                        break;

                                    case 26:
                                    case 29:
                                        xlWorkSheet.Columns[i].ColumnWidth = 12.43;
                                        break;
                                }

                                #endregion Setting the column widths

                                xlWorkSheet.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                xlWorkSheet.Columns[i].WrapText = true;
                                xlWorkSheet.Cells[tableFirstRow + 1, i].Font.Bold = false;
                                xlWorkSheet.Cells[tableFirstRow, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                                xlWorkSheet.Cells[tableFirstRow + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;

                            }

                            #endregion Adding the column headings



                            List<string> batchCodes = new List<string>();
                            foreach (DataRow rw in dtLeadAllocationData.Rows)
                            {
                                if (!batchCodes.Contains(rw["BatchNumber"].ToString()))
                                {
                                    batchCodes.Add(rw["BatchNumber"].ToString());
                                }
                            }

                            #region Totals variable definitions & Initializations

                            decimal totalLeads = 0;
                            decimal totalTelNo1 = 0;
                            decimal totalTelNo2 = 0;
                            decimal totalTelNo3 = 0;
                            decimal totalNoIDs = 0;
                            decimal TotalIDs = 0;
                            decimal totalIDnumbersByTel1 = 0;
                            decimal totalIDnumbersByTel2And3 = 0;
                            decimal totalTelNo1Percent = 0;
                            decimal totalTelNo2Percent = 0;
                            decimal totalTelNo3Percent = 0;
                            decimal totaltelno1xStl = 0;
                            decimal totaltelno2xStl = 0;
                            decimal totaltelno3xStl = 0;
                            decimal totaltelno1xStlPercent = 0;
                            decimal totaltelno2xStlPercent = 0;
                            decimal totaltelno3xStlPercent = 0;

                            decimal totalIDsPercent = 0;
                            decimal totalDobPercent = 0;
                            decimal totalIdsGivenByTel1Percent = 0;
                            decimal totalIdsGivenByTel2And3Percent = 0;
                            decimal totalsalesWithIDNumber = 0;
                            decimal totaliDNosGivenByTel1StlPercent = 0;
                            decimal totaliDNosGivenByTel2and3StlPercent = 0;
                            decimal totalsalesWithoutIDNumber = 0;

                            decimal totalEmailAddresses = 0;
                            decimal totalEmailAddressPercent = 0;

                            //double totalAverageLeads = 0;
                            //decimal totalAverageClients = 0;

                            double totalSalesWithIDNumberPercentage = 0;
                            double totalSalesWithoutIDNumberPercentage = 0;
                            decimal totalAverageDaysBetweenLeadCreationAndImport = 0;

                            double totalPensioners = 0;
                            double totalHousewives = 0;

                            double totalRePrimedLeads = 0;
                            double totalRePrimedLeadsSTL = 0;

                            double totalIndianLeads = 0;
                            double totalIndianLeadsSTL = 0;

                            decimal totalAfricanLeads = 0;
                            decimal totalAfricanLeadsSTL = 0;

                            decimal totalReMarketedLeads = 0;
                            decimal totalReMarketedLeadsSTL = 0;

                            decimal totalReMarketedLeads1 = 0;
                            decimal totalReMarketedLeadsSTL1 = 0;

                            decimal totalReMarketedLeads2 = 0;
                            decimal totalReMarketedLeadsSTL2 = 0;

                            decimal totalReMarketedLeads3 = 0;
                            decimal totalReMarketedLeadsSTL3 = 0;

                            decimal totalReMarketedLeads4 = 0;
                            decimal totalReMarketedLeadsSTL4 = 0;

                            decimal totalNOKContactDetailsLeads = 0;
                            decimal totalNOKContactDetailsSTL = 0;

                            decimal totalBenContactDetailsLeads = 0;
                            decimal totalBenContactDetailsSTL = 0;

                            decimal totalLA2ContactDetailsLeads = 0;
                            decimal totalLA2ContactDetailsSTL = 0;

                            decimal GrandTotalOfClientsAges = 0;
                            decimal TotalCountOfClientsWithAges = 0;

                            decimal GrandTotalOfDaysBetweenLeadCreationAndImport = 0;
                            decimal TotalCountOfDaysBetweenLeadCreationAndImport = 0;

                            decimal GrandTotalOfLeadsAges = 0;
                            decimal TotalCountOfLeadsWithAges = 0;

                            decimal dResult = 0;

                            #endregion Totals variable definitions & Initializations


                            foreach (string code in batchCodes)
                            {


                                #region Variable declarations & initializations

                                string BatchNumber = code;
                                string udmCode = string.Empty;
                                string dateReceived = string.Empty;

                                //int noPlatinumContactDate = 0;

                                decimal telNo1x = 0;
                                decimal telno1xStl = 0;
                                decimal telno1xStlPercent = 0;
                                decimal telno2xStl = 0;
                                decimal telno2xStlPercent = 0;
                                decimal telno3xStl = 0;
                                decimal telno3xStlPercent = 0;

                                decimal telNo1xPercent = 0;
                                decimal telNo2x = 0;
                                decimal telNo2xPercent = 0;
                                decimal telNo3x = 0;
                                decimal telNo3xPercent = 0;
                                decimal idnumbers = 0;
                                decimal idnumbersPercent = 0;
                                decimal noIDs = 0;
                                decimal NoIDsPercent = 0;
                                decimal emailAddressCount = 0;
                                int iDNosGivenByTel1 = 0;
                                decimal iDNosGivenByTel1Percent = 0;

                                decimal iDNosGivenByTel1Stl = 0;
                                decimal iDNosGivenByTel1StlPercent = 0;

                                int iDNosGivenByTel2and3 = 0;
                                decimal iDNosGivenByTel2and3Percent = 0;

                                decimal iDNosGivenByTel2and3Stl = 0;
                                decimal iDNosGivenByTel2and3StlPercent = 0;

                                decimal emailAddressesPercent = 0;

                                decimal AverageAgeOfLeads = 0;
                                decimal TotalOfLeadsAges = 0;
                                decimal CountOfLeadsWithAges = 0;
                                decimal noLeadAge = 0;

                                decimal AverageDaysBetweenLeadCreationAndImport = 0;
                                decimal TotalOfDaysBetweenLeadCreationAndImport = 0;
                                decimal CountOfDaysBetweenLeadCreationAndImport = 0;
                                decimal NoDaysBetweenLeadCreationAndImport = 0;

                                decimal AverageAgeOfClients = 0;
                                decimal TotalOfClientsAges = 0;
                                decimal CountOfClientsWithAges = 0;
                                decimal noDob = 0;

                                int leadcount = Convert.ToInt32(dtTotals.Select("BatchNumber = '" + code + "'")[0]["LeadCount"]);
                                //dtLeadAllocationData.AsEnumerable().Select(r => r.Field<long>("ID")).Distinct().ToArray().Count()

                                int salesWithIDNumber = 0;
                                decimal salesWithIDNumberPercentage = 0;

                                int salesWithoutIDNumber = 0;
                                decimal salesWithoutIDNumberPercentage = 0;

                                decimal pensioners = 0;
                                decimal pensionersPercentage = 0;
                                decimal housewives = 0;
                                decimal housewivesPercentage = 0;

                                decimal reprimedLeads = 0;
                                decimal reprimedLeadsPercentage = 0;
                                decimal reprimedLeadsSTL = 0;
                                decimal reprimedLeadsSTLPercentage = 0;

                                decimal indianLeads = 0;
                                decimal indianLeadsPercentage = 0;
                                decimal indianLeadsSTL = 0;
                                decimal indianLeadsSTLPercentage = 0;

                                decimal africanLeads = 0;
                                decimal africanLeadsPercentage = 0;
                                decimal africanLeadsSTL = 0;
                                decimal africanLeadsSTLPercentage = 0;

                                decimal reMarketedLeads = 0;
                                decimal reMarketedLeadsPercentage = 0;
                                decimal reMarketedLeadsSTL = 0;
                                decimal reMarketedLeadsSTLPercentage = 0;

                                decimal reMarketedLeads1 = 0;
                                decimal reMarketedLeadsPercentage1 = 0;
                                decimal reMarketedLeadsSTL1 = 0;
                                decimal reMarketedLeadsSTLPercentage1 = 0;

                                decimal reMarketedLeads2 = 0;
                                decimal reMarketedLeadsPercentage2 = 0;
                                decimal reMarketedLeadsSTL2 = 0;
                                decimal reMarketedLeadsSTLPercentage2 = 0;

                                decimal reMarketedLeads3 = 0;
                                decimal reMarketedLeadsPercentage3 = 0;
                                decimal reMarketedLeadsSTL3 = 0;
                                decimal reMarketedLeadsSTLPercentage3 = 0;

                                decimal reMarketedLeads4 = 0;
                                decimal reMarketedLeadsPercentage4 = 0;
                                decimal reMarketedLeadsSTL4 = 0;
                                decimal reMarketedLeadsSTLPercentage4 = 0;

                                decimal NOKContactDetailsLeads = 0;
                                decimal NOKContactDetailsLeadsPercentage = 0;
                                decimal NOKContactDetailsSTL = 0;
                                decimal NOKContactDetailsSTLPercentage = 0;

                                decimal BenContactDetailsLeads = 0;
                                decimal BenContactDetailsLeadsPercentage = 0;
                                decimal BenContactDetailsSTL = 0;
                                decimal BenContactDetailsSTLPercentage = 0;

                                decimal LA2ContactDetailsLeads = 0;
                                decimal LA2ContactDetailsLeadsPercentage = 0;
                                decimal LA2ContactDetailsSTL = 0;
                                decimal LA2ContactDetailsSTLPercentage = 0;

                                #endregion Variable declarations & initializations

                                #region Loop through each row in the data table to get the totals



                                foreach (DataRow row in dtLeadAllocationData.Rows)
                                {
                                    int telCount = 0;
                                    double age = 0;
                                    double age2 = 0;

                                    if (row["BatchNumber"].ToString() == code)
                                    {
                                        udmCode = row["UDMCode"].ToString();
                                        string importDate = row["ImportDate"].ToString();
                                        if (importDate != string.Empty)
                                        {
                                            dateReceived = importDate.Substring(0, 10);
                                        }

                                        //leadcount++;

                                        #region telephone numbers
                                        string telwork = row["TelWork"].ToString();
                                        string telHome = row["TelHome"].ToString();
                                        string telCell = row["TelCell"].ToString();
                                        string leadStatusID = row["FKINLeadStatusID"].ToString();
                                        if (telwork != string.Empty)
                                        {
                                            telCount++;
                                        }
                                        if (telHome != string.Empty)
                                        {
                                            telCount++;
                                        }
                                        if (telCell != string.Empty)
                                        {
                                            telCount++;
                                        }

                                        if (telCount == 1)
                                        {
                                            telNo1x++;
                                            //determine if sale
                                            if (leadStatusID == "1")//sale
                                            {
                                                telno1xStl++;
                                            }
                                        }
                                        if (telCount == 2)
                                        {
                                            telNo2x++;
                                            //determine if sale
                                            if (leadStatusID == "1")//sale
                                            {
                                                telno2xStl++;
                                            }
                                        }
                                        if (telCount > 2)
                                        {
                                            telNo3x++;
                                            //determine if sale
                                            if (leadStatusID == "1")//sale
                                            {
                                                telno3xStl++;
                                            }
                                        }
                                        #endregion telephone numbers

                                        #region id numbers and DateOfBirth

                                        string idnumber = row["IDNo"].ToString();
                                        string dateOfBirth = row["DateOfBirth"].ToString();
                                        if (idnumber != string.Empty)
                                        {
                                            idnumbers++;
                                            if (telCount == 1)
                                            {
                                                iDNosGivenByTel1++;
                                                //determine if sale
                                                if (leadStatusID == "1")//sale
                                                {
                                                    iDNosGivenByTel1Stl++;
                                                    salesWithIDNumber++;
                                                }
                                            }
                                            if (telCount >= 2)
                                            {
                                                iDNosGivenByTel2and3++;
                                                //determine if sale
                                                if (leadStatusID == "1")//sale
                                                {
                                                    iDNosGivenByTel2and3Stl++;
                                                    salesWithIDNumber++;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            noIDs++;
                                        }

                                        if (dateOfBirth != string.Empty)
                                        {

                                            //workout age                                                
                                            int year = DateTime.Parse(dateOfBirth).Year;
                                            int month = DateTime.Parse(dateOfBirth).Month;
                                            int day = DateTime.Parse(dateOfBirth).Day;
                                            DateTime dob = new DateTime(year, month, day);
                                            DateTime now = DateTime.Now;
                                            //now = now.AddYears(-year);
                                            //now = now.AddMonths(-month);
                                            //now.AddDays(-day);    
                                            //  now = //now.Subtract(dob.Date

                                            TimeSpan span = now.Subtract(dob);
                                            //age = //(span.TotalDays/ 365.25).ToString()
                                            age = span.TotalDays / 365.25;


                                            TotalOfClientsAges += (decimal)age;
                                            CountOfClientsWithAges++;
                                        }
                                        else
                                        {
                                            noDob++;
                                        }

                                        #endregion id numbers and DateOfBirth

                                        #region email addresses
                                        string emailAddress = row["Email"].ToString();
                                        if (emailAddress != string.Empty)
                                        {
                                            emailAddressCount++;
                                        }
                                        #endregion email addresses

                                        #region Lead Age

                                        //string platinumContactDate = row["PlatinumContactDate"].ToString();
                                        //if (platinumContactDate != string.Empty)
                                        //{
                                        //    int year = DateTime.Parse(platinumContactDate).Year;
                                        //    int month = DateTime.Parse(platinumContactDate).Month;
                                        //    int day = DateTime.Parse(platinumContactDate).Day;
                                        //    DateTime platDate = new DateTime(year, month, day);
                                        //    DateTime impDate = DateTime.Parse(importDate);
                                        //    TimeSpan span2 = impDate.Subtract(platDate);
                                        //    age2 = span2.TotalDays;// / 365.25;
                                        //    totalLeadAges = totalLeadAges + age2;
                                        //}
                                        //else
                                        //{
                                        //    noPlatinumContactDate++;
                                        //}

                                        if (row["LeadAge"] != DBNull.Value)
                                        {
                                            int leadAge = Convert.ToInt32(row["LeadAge"]);
                                            TotalOfLeadsAges += leadAge;
                                            CountOfLeadsWithAges++;
                                        }
                                        else
                                        {
                                            noLeadAge++;
                                        }

                                        #endregion Lead Age

                                        #region ID Number STLs

                                        if (leadStatusID == "1")
                                        {
                                            if (idnumber == string.Empty)
                                            {
                                                salesWithoutIDNumber++;
                                            }
                                        }

                                        #endregion ID Number STLs

                                        #region Days Between Lead Creation And Import

                                        if (row["DaysBetweenLeadCreationAndImport"] != DBNull.Value)
                                        {
                                            int daysBetweenLeadCreationAndImport = Convert.ToInt32(row["DaysBetweenLeadCreationAndImport"]);
                                            TotalOfDaysBetweenLeadCreationAndImport += daysBetweenLeadCreationAndImport;
                                            CountOfDaysBetweenLeadCreationAndImport++;
                                        }
                                        else
                                        {
                                            NoDaysBetweenLeadCreationAndImport++;
                                        }

                                        #endregion Days Between Lead Creation And Import

                                        #region Pensioners & Housewives

                                        if (row["Occupation"].ToString().Trim() == "Pensioner")
                                        {
                                            ++pensioners;
                                        }
                                        else if (row["Occupation"].ToString().Trim() == "Housewife")
                                        {
                                            ++housewives;
                                        }

                                        #endregion Pensioners & Housewives

                                        #region Re-Primed Leads

                                        //if (!string.IsNullOrEmpty(row["ReferralFrom"].ToString().Trim()))
                                        if (Convert.ToBoolean(row["IsRePrimed"])) // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/215482028/comments
                                        {
                                            ++reprimedLeads;

                                            if (leadStatusID == "1")
                                            {
                                                ++reprimedLeadsSTL;
                                            }
                                        }

                                        #endregion Re-Primed Leads

                                        #region Indian Leads
                                        // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/214952271/comments#355827524

                                        bool isIndian = Convert.ToBoolean(row["IsIndian"]);

                                        if (isIndian)
                                        {
                                            indianLeads++;

                                            if (leadStatusID == "1")
                                            {
                                                indianLeadsSTL++;
                                            }
                                        }

                                        #endregion Indian Leads

                                        #region African Leads

                                        bool isAfrican = Convert.ToBoolean(row["IsAfrican"]);

                                        if (isAfrican)
                                        {
                                            africanLeads++;

                                            if (leadStatusID == "1")
                                            {
                                                africanLeadsSTL++;
                                            }
                                        }

                                        #endregion African Leads

                                        #region ReMarketed Leads

                                        bool isReMarketed = Convert.ToBoolean(row["IsReMarketed"]);
                                        if (isReMarketed)
                                        {
                                            reMarketedLeads++;

                                            if (leadStatusID == "1")
                                            {
                                                reMarketedLeadsSTL++;
                                            }
                                        }

                                        bool isReMarketed1 = Convert.ToBoolean(row["IsReMarketed1"]);
                                        if (isReMarketed1)
                                        {
                                            reMarketedLeads1++;

                                            if (leadStatusID == "1")
                                            {
                                                reMarketedLeadsSTL1++;
                                            }
                                        }

                                        bool isReMarketed2 = Convert.ToBoolean(row["IsReMarketed2"]);
                                        if (isReMarketed2)
                                        {
                                            reMarketedLeads2++;

                                            if (leadStatusID == "1")
                                            {
                                                reMarketedLeadsSTL2++;
                                            }
                                        }

                                        bool isReMarketed3 = Convert.ToBoolean(row["IsReMarketed3"]);
                                        if (isReMarketed3)
                                        {
                                            reMarketedLeads3++;

                                            if (leadStatusID == "1")
                                            {
                                                reMarketedLeadsSTL3++;
                                            }
                                        }

                                        bool isReMarketed4 = Convert.ToBoolean(row["IsReMarketed4"]);
                                        if (isReMarketed4)
                                        {
                                            reMarketedLeads4++;

                                            if (leadStatusID == "1")
                                            {
                                                reMarketedLeadsSTL4++;
                                            }
                                        }

                                        #endregion ReMarketed Leads

                                        #region NOK Contact Detail Leads

                                        bool hasNokContactDetails = Convert.ToBoolean(row["NOKHasContactDetails"]);
                                        if (hasNokContactDetails)
                                        {
                                            NOKContactDetailsLeads++;

                                            if (leadStatusID == "1")
                                            {
                                                NOKContactDetailsSTL++;
                                            }
                                        }

                                        #endregion NOK Contact Detail Leads

                                        #region Ben Contact Detail Leads

                                        bool hasBenContactDetails = Convert.ToBoolean(row["BenHasContactDetails"]);
                                        if (hasBenContactDetails)
                                        {
                                            BenContactDetailsLeads++;

                                            if (leadStatusID == "1")
                                            {
                                                BenContactDetailsSTL++;
                                            }
                                        }

                                        #endregion Ben Contact Detail Leads

                                        #region LA2 Contact Detail Leads

                                        bool hasLA2ContactDetails = Convert.ToBoolean(row["LAHasContactDetails"]);
                                        if (hasLA2ContactDetails)
                                        {
                                            LA2ContactDetailsLeads++;

                                            if (leadStatusID == "1")
                                            {
                                                LA2ContactDetailsSTL++;
                                            }
                                        }

                                        #endregion LA2 Contact Detail Leads


                                    }
                                }

                                #endregion Loop through each row in the data table to get the totals

                                if (CountOfClientsWithAges > 0)
                                {
                                    AverageAgeOfClients = Math.Round(TotalOfClientsAges / CountOfClientsWithAges, 2);
                                }

                                //if ((leadcount - noPlatinumContactDate) > 0)
                                //{
                                //    averageAgeOfLeads = Math.Round(totalLeadAges / leadcount - noPlatinumContactDate, 0);
                                //}

                                if (CountOfLeadsWithAges > 0)
                                {
                                    AverageAgeOfLeads = Math.Round(TotalOfLeadsAges / CountOfLeadsWithAges, 0);
                                    AverageDaysBetweenLeadCreationAndImport = (decimal)Math.Round(TotalOfDaysBetweenLeadCreationAndImport / (leadcount - NoDaysBetweenLeadCreationAndImport), 0);
                                }

                                if (CountOfClientsWithAges > 0)
                                {
                                    AverageAgeOfClients = Math.Round(TotalOfClientsAges / CountOfClientsWithAges, 2);
                                }

                                decimal totalcontactNumber = telNo1x + telNo2x + telNo3x;
                                decimal totalIDNumbers = iDNosGivenByTel1 + iDNosGivenByTel2and3;
                                if (totalcontactNumber > 0)
                                {
                                    telNo1xPercent = Math.Round(telNo1x / totalcontactNumber * 100, 2);
                                    telNo2xPercent = Math.Round(telNo2x / totalcontactNumber * 100, 2);
                                    telNo3xPercent = Math.Round(telNo3x / totalcontactNumber * 100, 2);
                                }
                                if (telNo1x > 0)
                                {
                                    telno1xStlPercent = Math.Round(telno1xStl / telNo1x * 100, 2);
                                }
                                if (telNo2x > 0)
                                {
                                    telno2xStlPercent = Math.Round(telno2xStl / telNo2x * 100, 2);
                                }
                                if (telNo3x > 0)
                                {
                                    telno3xStlPercent = Math.Round(telno3xStl / telNo3x * 100, 2);
                                }
                                if (leadcount > 0)
                                {
                                    idnumbersPercent = Math.Round(idnumbers / leadcount * 100, 2);
                                    NoIDsPercent = Math.Round(noIDs / leadcount * 100, 2);
                                    emailAddressesPercent = Math.Round(emailAddressCount / leadcount * 100, 2);
                                }
                                if (totalIDNumbers > 0)
                                {
                                    iDNosGivenByTel1Percent = Math.Round(iDNosGivenByTel1 / totalIDNumbers * 100, 2);
                                    iDNosGivenByTel2and3Percent = Math.Round(iDNosGivenByTel2and3 / totalIDNumbers * 100, 2);
                                    salesWithIDNumberPercentage = Math.Round(salesWithIDNumber / totalIDNumbers * 100, 2);
                                    salesWithoutIDNumberPercentage = Math.Round(salesWithoutIDNumber / totalIDNumbers * 100, 2);
                                }
                                if (iDNosGivenByTel1 > 0)
                                {
                                    iDNosGivenByTel1StlPercent = Math.Round(iDNosGivenByTel1Stl / iDNosGivenByTel1 * 100, 2);
                                }
                                if (iDNosGivenByTel2and3 > 0)
                                {
                                    iDNosGivenByTel2and3StlPercent = Math.Round(iDNosGivenByTel2and3Stl / iDNosGivenByTel2and3 * 100, 2);
                                }

                                #region Pensioner & Housewife totals & percentages

                                if (leadcount > 0)
                                {
                                    pensionersPercentage = Math.Round(pensioners / leadcount * 100, 2);
                                    housewivesPercentage = Math.Round(housewives / leadcount * 100, 2);
                                }

                                #endregion Pensioner & Housewife totals & percentages

                                #region Re-Primed Leads Totals & Percentages

                                if (leadcount > 0)
                                {
                                    reprimedLeadsPercentage = Math.Round(reprimedLeads / leadcount * 100, 2);

                                    if (reprimedLeads > 0)
                                    {
                                        reprimedLeadsSTLPercentage = Math.Round(reprimedLeadsSTL / reprimedLeads * 100, 2);
                                    }
                                    else
                                    {
                                        reprimedLeadsSTLPercentage = 0.00m;
                                    }
                                }

                                #endregion Re-Primed Leads Totals & Percentages

                                #region Indian Leads Percentages

                                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/214952271/comments#355827524

                                if (leadcount > 0)
                                {
                                    indianLeadsPercentage = Math.Round(indianLeads / leadcount * 100, 2);

                                    if (indianLeads > 0)
                                    {
                                        indianLeadsSTLPercentage = Math.Round(indianLeadsSTL / indianLeads * 100, 2);
                                    }
                                    else
                                    {
                                        indianLeadsSTLPercentage = 0.00m;
                                    }
                                }

                                #endregion Indian Leads Percentages

                                #region African Leads Percentages

                                if (leadcount > 0)
                                {
                                    africanLeadsPercentage = leadcount > 0 ? africanLeads / leadcount : 0;
                                    africanLeadsSTLPercentage = africanLeads > 0 ? africanLeadsSTL / africanLeads : 0;
                                }

                                #endregion African Leads Percentages

                                #region ReMarketed Leads Percentages

                                reMarketedLeadsPercentage = leadcount > 0 ? reMarketedLeads / leadcount : 0;
                                reMarketedLeadsSTLPercentage = reMarketedLeads > 0 ? reMarketedLeadsSTL / reMarketedLeads : 0;

                                reMarketedLeadsPercentage1 = reMarketedLeads > 0 ? reMarketedLeads1 / reMarketedLeads : 0;
                                reMarketedLeadsSTLPercentage1 = reMarketedLeads1 > 0 ? reMarketedLeadsSTL1 / reMarketedLeads1 : 0;

                                reMarketedLeadsPercentage2 = reMarketedLeads > 0 ? reMarketedLeads2 / reMarketedLeads : 0;
                                reMarketedLeadsSTLPercentage2 = reMarketedLeads2 > 0 ? reMarketedLeadsSTL2 / reMarketedLeads2 : 0;

                                reMarketedLeadsPercentage3 = reMarketedLeads > 0 ? reMarketedLeads3 / reMarketedLeads : 0;
                                reMarketedLeadsSTLPercentage3 = reMarketedLeads3 > 0 ? reMarketedLeadsSTL3 / reMarketedLeads3 : 0;

                                reMarketedLeadsPercentage4 = reMarketedLeads > 0 ? reMarketedLeads4 / reMarketedLeads : 0;
                                reMarketedLeadsSTLPercentage4 = reMarketedLeads4 > 0 ? reMarketedLeadsSTL4 / reMarketedLeads4 : 0;

                                #endregion ReMarketed Leads Percentages

                                #region NOK Lead Percentages

                                NOKContactDetailsLeadsPercentage = leadcount > 0 ? NOKContactDetailsLeads / leadcount : 0;
                                NOKContactDetailsSTLPercentage = NOKContactDetailsLeads > 0 ? NOKContactDetailsSTL / NOKContactDetailsLeads : 0;

                                #endregion NOK Lead Percentages

                                #region Ben Lead Percentages

                                BenContactDetailsLeadsPercentage = leadcount > 0 ? BenContactDetailsLeads / leadcount : 0;
                                BenContactDetailsSTLPercentage = BenContactDetailsLeads > 0 ? BenContactDetailsSTL / BenContactDetailsLeads : 0;

                                #endregion Ben Lead Percentages

                                #region LA2 Lead Percentages

                                LA2ContactDetailsLeadsPercentage = leadcount > 0 ? LA2ContactDetailsLeads / leadcount : 0;
                                LA2ContactDetailsSTLPercentage = LA2ContactDetailsLeads > 0 ? LA2ContactDetailsSTL / LA2ContactDetailsLeads : 0;

                                #endregion LA2 Lead Percentages


                                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/210685249/comments

                                #region Formatting the body of the report

                                for (int i = 1; i <= reportColumnCount; i++)
                                {
                                    if (i == 1)
                                    {
                                        xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 2;
                                    }
                                    if (i >= 2 && i <= 5)
                                    {
                                        xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 35;
                                    }
                                    if (i >= 5 && i <= 13)
                                    {
                                        xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 34;
                                    }

                                    if (i >= 14 && i <= 19)
                                    {
                                        xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 36;
                                    }

                                    if (i >= 20 && i <= 25)
                                    {
                                        xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 45;
                                    }

                                    if (i == 26)
                                    {
                                        xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 33;
                                    }

                                    if (i >= 27 && i <= 28)
                                    {
                                        xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 39;
                                    }

                                    if (i >= 30 && i <= 33)
                                    {
                                        xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 40;
                                    }

                                    if (i >= 34 && i <= 36)
                                    {
                                        xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 35;
                                        //xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 35;
                                    }

                                    if (i >= 37 && i <= 39)
                                    {
                                        xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 34;
                                        //xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 34;
                                    }

                                    if (i >= 40 && i <= 42)
                                    {
                                        xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 15;
                                    }

                                    if (i >= 43 && i <= 45)
                                    {
                                        xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 24;
                                    }

                                    if (i >= 46 && i <= 57)
                                    {
                                        xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 19;
                                    }
                                    //*****************************************************/
                                    if (i >= 58 && i <= 60)
                                    {
                                        xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 35;
                                    }

                                    if (i >= 61 && i <= 63)
                                    {
                                        xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 34;
                                    }

                                    if (i >= 64 && i <= 67)
                                    {
                                        xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 24;
                                    }

                                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                }

                                #endregion Formatting the body of the report

                                // foreach (DataRecord dataRecord in campaigns)
                                //{
                                //if ((bool)dataRecord.Cells["Select"].Value)
                                //{

                                string campaignCode01 = record.Cells["CampaignCode"].Value.ToString();

                                xlWorkSheet.Cells[rowIndex, 1] = campaignCode01;
                                xlWorkSheet.Cells[rowIndex, 2] = BatchNumber;
                                xlWorkSheet.Cells[rowIndex, 3] = udmCode;
                                xlWorkSheet.Cells[rowIndex, 4] = dateReceived;
                                xlWorkSheet.Cells[rowIndex, 5] = leadcount;
                                xlWorkSheet.Cells[rowIndex, 6] = telNo1x;
                                xlWorkSheet.Cells[rowIndex, 7] = telNo1xPercent + " %";
                                xlWorkSheet.Cells[rowIndex, 8] = telno1xStlPercent + " %";
                                xlWorkSheet.Cells[rowIndex, 9] = telNo2x;
                                xlWorkSheet.Cells[rowIndex, 10] = telNo2xPercent + " %";
                                xlWorkSheet.Cells[rowIndex, 11] = telno2xStlPercent + " %";
                                xlWorkSheet.Cells[rowIndex, 12] = telNo3x;
                                xlWorkSheet.Cells[rowIndex, 13] = telNo3xPercent + " %";
                                xlWorkSheet.Cells[rowIndex, 14] = telno3xStlPercent + " %";

                                xlWorkSheet.Cells[rowIndex, 15] = idnumbers;
                                xlWorkSheet.Cells[rowIndex, 16] = idnumbersPercent + " %";
                                xlWorkSheet.Cells[rowIndex, 17] = salesWithIDNumberPercentage + " %";

                                xlWorkSheet.Cells[rowIndex, 18] = noIDs;
                                xlWorkSheet.Cells[rowIndex, 19] = NoIDsPercent + " %";
                                xlWorkSheet.Cells[rowIndex, 20] = salesWithoutIDNumberPercentage + " %";

                                xlWorkSheet.Cells[rowIndex, 21] = iDNosGivenByTel1;
                                xlWorkSheet.Cells[rowIndex, 22] = iDNosGivenByTel1Percent + " %";
                                xlWorkSheet.Cells[rowIndex, 23] = iDNosGivenByTel1StlPercent + " %";
                                xlWorkSheet.Cells[rowIndex, 24] = iDNosGivenByTel2and3;
                                xlWorkSheet.Cells[rowIndex, 25] = iDNosGivenByTel2and3Percent + " %";
                                xlWorkSheet.Cells[rowIndex, 26] = iDNosGivenByTel2and3StlPercent + " %";
                                xlWorkSheet.Cells[rowIndex, 27] = emailAddressesPercent + " %";

                                xlWorkSheet.Cells[rowIndex, 28] = AverageAgeOfLeads;
                                xlRange = xlWorkSheet.Cells[rowIndex, 28];
                                xlRange.NumberFormat = "0";

                                xlWorkSheet.Cells[rowIndex, 29] = AverageDaysBetweenLeadCreationAndImport;
                                xlRange = xlWorkSheet.Cells[rowIndex, 29];
                                xlRange.NumberFormat = "0";

                                xlWorkSheet.Cells[rowIndex, 30] = AverageAgeOfClients;
                                xlRange = xlWorkSheet.Cells[rowIndex, 30];
                                xlRange.NumberFormat = "0.00";

                                xlWorkSheet.Cells[rowIndex, 31] = pensioners;
                                xlWorkSheet.Cells[rowIndex, 32] = pensionersPercentage + " %";
                                xlWorkSheet.Cells[rowIndex, 33] = housewives;
                                xlWorkSheet.Cells[rowIndex, 34] = housewivesPercentage + " %";

                                xlWorkSheet.Cells[rowIndex, 35] = reprimedLeads;
                                xlWorkSheet.Cells[rowIndex, 36] = reprimedLeadsPercentage + " %";
                                xlWorkSheet.Cells[rowIndex, 37] = reprimedLeadsSTLPercentage + " %";

                                xlWorkSheet.Cells[rowIndex, 38] = indianLeads;
                                xlWorkSheet.Cells[rowIndex, 39] = indianLeadsPercentage + " %";
                                xlWorkSheet.Cells[rowIndex, 40] = indianLeadsSTLPercentage + " %";

                                xlWorkSheet.Cells[rowIndex, 41] = africanLeads;
                                xlWorkSheet.Cells[rowIndex, 42] = Math.Round(africanLeadsPercentage, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 42];
                                xlRange.NumberFormat = africanLeadsPercentage == 1m ? "0%" : "0.00%";
                                xlWorkSheet.Cells[rowIndex, 43] = Math.Round(africanLeadsSTLPercentage, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 43];
                                xlRange.NumberFormat = africanLeadsSTLPercentage == 1m ? "0%" : "0.00%";

                                xlWorkSheet.Cells[rowIndex, 44] = reMarketedLeads;
                                xlWorkSheet.Cells[rowIndex, 45] = Math.Round(reMarketedLeadsPercentage, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 45];
                                xlRange.NumberFormat = reMarketedLeadsPercentage == 1m ? "0%" : "0.00%";
                                xlWorkSheet.Cells[rowIndex, 46] = Math.Round(reMarketedLeadsSTLPercentage, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 46];
                                xlRange.NumberFormat = reMarketedLeadsSTLPercentage == 1m ? "0%" : "0.00%";

                                xlWorkSheet.Cells[rowIndex, 47] = reMarketedLeads1;
                                xlWorkSheet.Cells[rowIndex, 48] = Math.Round(reMarketedLeadsPercentage1, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 48];
                                xlRange.NumberFormat = reMarketedLeadsPercentage1 == 1m ? "0%" : "0.00%";
                                xlWorkSheet.Cells[rowIndex, 49] = Math.Round(reMarketedLeadsSTLPercentage1, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 49];
                                xlRange.NumberFormat = reMarketedLeadsSTLPercentage1 == 1m ? "0%" : "0.00%";

                                xlWorkSheet.Cells[rowIndex, 50] = reMarketedLeads2;
                                xlWorkSheet.Cells[rowIndex, 51] = Math.Round(reMarketedLeadsPercentage2, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 51];
                                xlRange.NumberFormat = reMarketedLeadsPercentage2 == 1m ? "0%" : "0.00%";
                                xlWorkSheet.Cells[rowIndex, 52] = Math.Round(reMarketedLeadsSTLPercentage2, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 52];
                                xlRange.NumberFormat = reMarketedLeadsSTLPercentage2 == 1m ? "0%" : "0.00%";

                                xlWorkSheet.Cells[rowIndex, 53] = reMarketedLeads3;
                                xlWorkSheet.Cells[rowIndex, 54] = Math.Round(reMarketedLeadsPercentage3, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 54];
                                xlRange.NumberFormat = reMarketedLeadsPercentage3 == 1m ? "0%" : "0.00%";
                                xlWorkSheet.Cells[rowIndex, 55] = Math.Round(reMarketedLeadsSTLPercentage3, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 55];
                                xlRange.NumberFormat = reMarketedLeadsSTLPercentage3 == 1m ? "0%" : "0.00%";

                                xlWorkSheet.Cells[rowIndex, 56] = reMarketedLeads4;
                                xlWorkSheet.Cells[rowIndex, 57] = Math.Round(reMarketedLeadsPercentage4, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 57];
                                xlRange.NumberFormat = reMarketedLeadsPercentage4 == 1m ? "0%" : "0.00%";
                                xlWorkSheet.Cells[rowIndex, 58] = Math.Round(reMarketedLeadsSTLPercentage4, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 58];
                                xlRange.NumberFormat = reMarketedLeadsSTLPercentage4 == 1m ? "0%" : "0.00%";

                                xlWorkSheet.Cells[rowIndex, 59] = NOKContactDetailsLeads;
                                xlWorkSheet.Cells[rowIndex, 60] = Math.Round(NOKContactDetailsLeadsPercentage, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 60];
                                xlRange.NumberFormat = NOKContactDetailsLeadsPercentage == 1m ? "0%" : "0.00%";
                                xlWorkSheet.Cells[rowIndex, 61] = Math.Round(NOKContactDetailsSTLPercentage, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 61];
                                xlRange.NumberFormat = NOKContactDetailsSTLPercentage == 1m ? "0%" : "0.00%";

                                xlWorkSheet.Cells[rowIndex, 62] = BenContactDetailsLeads;
                                xlWorkSheet.Cells[rowIndex, 63] = Math.Round(BenContactDetailsLeadsPercentage, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 63];
                                xlRange.NumberFormat = BenContactDetailsLeadsPercentage == 1m ? "0%" : "0.00%";
                                xlWorkSheet.Cells[rowIndex, 64] = Math.Round(BenContactDetailsSTLPercentage, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 64];
                                xlRange.NumberFormat = BenContactDetailsSTLPercentage == 1m ? "0%" : "0.00%";

                                xlWorkSheet.Cells[rowIndex, 65] = LA2ContactDetailsLeads;
                                xlWorkSheet.Cells[rowIndex, 66] = Math.Round(LA2ContactDetailsLeadsPercentage, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 66];
                                xlRange.NumberFormat = LA2ContactDetailsLeadsPercentage == 1m ? "0%" : "0.00%";
                                xlWorkSheet.Cells[rowIndex, 67] = Math.Round(LA2ContactDetailsSTLPercentage, 4);
                                xlRange = xlWorkSheet.Cells[rowIndex, 67];
                                xlRange.NumberFormat = LA2ContactDetailsSTLPercentage == 1m ? "0%" : "0.00%";


                                rowIndex++;

                                //}


                                //}



                                #region Updating the totals

                                totalLeads = totalLeads + leadcount;
                                totalTelNo1 = totalTelNo1 + telNo1x;
                                totalTelNo2 = totalTelNo2 + telNo2x;
                                totalTelNo3 = totalTelNo3 + telNo3x;
                                TotalIDs = TotalIDs + idnumbers;
                                totalNoIDs = totalNoIDs + noIDs;
                                totalIDnumbersByTel1 = totalIDnumbersByTel1 + iDNosGivenByTel1;
                                totalIDnumbersByTel2And3 = totalIDnumbersByTel2And3 + iDNosGivenByTel2and3;
                                totalTelNo1Percent = totalTelNo1Percent + telNo1xPercent;
                                totalTelNo2Percent = totalTelNo2Percent + telNo2xPercent;
                                totalTelNo3Percent = totalTelNo3Percent + telNo3xPercent;
                                totaltelno1xStl += telno1xStl;
                                totaltelno2xStl += telno2xStl;
                                totaltelno3xStl += telno3xStl;
                                totaltelno1xStlPercent = totaltelno1xStlPercent + telno1xStlPercent;
                                totaltelno2xStlPercent = totaltelno2xStlPercent + telno2xStlPercent;
                                totaltelno3xStlPercent = totaltelno3xStlPercent + telno3xStlPercent;
                                totalIDsPercent = totalIDsPercent + idnumbersPercent;
                                totalDobPercent = totalDobPercent + NoIDsPercent;
                                totalIdsGivenByTel1Percent = totalIdsGivenByTel1Percent + iDNosGivenByTel1Percent;
                                totalIdsGivenByTel2And3Percent = totalIdsGivenByTel2And3Percent + iDNosGivenByTel2and3Percent;
                                totaliDNosGivenByTel1StlPercent = totaliDNosGivenByTel1StlPercent + iDNosGivenByTel1StlPercent;
                                totaliDNosGivenByTel2and3StlPercent = totaliDNosGivenByTel2and3StlPercent + iDNosGivenByTel2and3StlPercent;

                                totalsalesWithIDNumber += salesWithIDNumber;
                                totalsalesWithoutIDNumber += salesWithoutIDNumber;

                                //totalEmailAddressPercent = totalEmailAddressPercent + emailAddressesPercent;
                                //totalAverageLeads = Math.Round(totalAverageLeads + AverageAgeOfLeads, 0);
                                //totalAverageClients = Math.Round(totalAverageClients + AverageAgeOfClients, 2);

                                totalSalesWithIDNumberPercentage = totalSalesWithIDNumberPercentage + (double)salesWithIDNumberPercentage;
                                totalSalesWithoutIDNumberPercentage = totalSalesWithoutIDNumberPercentage + (double)salesWithoutIDNumberPercentage;
                                totalAverageDaysBetweenLeadCreationAndImport = totalAverageDaysBetweenLeadCreationAndImport + AverageDaysBetweenLeadCreationAndImport;

                                totalPensioners += (double)pensioners;
                                totalHousewives += (double)housewives;

                                totalRePrimedLeads += (double)reprimedLeads;
                                totalRePrimedLeadsSTL += (double)reprimedLeadsSTL;

                                totalIndianLeads += (double)indianLeads;
                                totalIndianLeadsSTL += (double)indianLeadsSTL;

                                totalAfricanLeads += africanLeads;
                                totalAfricanLeadsSTL += africanLeadsSTL;

                                totalReMarketedLeads += reMarketedLeads;
                                totalReMarketedLeadsSTL += reMarketedLeadsSTL;

                                totalReMarketedLeads1 += reMarketedLeads1;
                                totalReMarketedLeadsSTL1 += reMarketedLeadsSTL1;

                                totalReMarketedLeads2 += reMarketedLeads2;
                                totalReMarketedLeadsSTL2 += reMarketedLeadsSTL2;

                                totalReMarketedLeads3 += reMarketedLeads3;
                                totalReMarketedLeadsSTL3 += reMarketedLeadsSTL3;

                                totalReMarketedLeads4 += reMarketedLeads4;
                                totalReMarketedLeadsSTL4 += reMarketedLeadsSTL4;

                                totalNOKContactDetailsLeads += NOKContactDetailsLeads;
                                totalNOKContactDetailsSTL += NOKContactDetailsSTL;

                                totalBenContactDetailsLeads += BenContactDetailsLeads;
                                totalBenContactDetailsSTL += BenContactDetailsSTL;

                                totalLA2ContactDetailsLeads += LA2ContactDetailsLeads;
                                totalLA2ContactDetailsSTL += LA2ContactDetailsSTL;

                                totalEmailAddresses += emailAddressCount;

                                GrandTotalOfClientsAges += TotalOfClientsAges;
                                TotalCountOfClientsWithAges += CountOfClientsWithAges;

                                GrandTotalOfDaysBetweenLeadCreationAndImport += TotalOfDaysBetweenLeadCreationAndImport;
                                TotalCountOfDaysBetweenLeadCreationAndImport += CountOfDaysBetweenLeadCreationAndImport;

                                GrandTotalOfLeadsAges += TotalOfLeadsAges;
                                TotalCountOfLeadsWithAges += CountOfLeadsWithAges;

                                #endregion Updating the totals


                                #region Totals

                                //xlWorkSheet.Cells[rowIndex, 1].Font.Bold = true;
                                ////////////////////////////////////////////commented from here////////////////////////////
                                //for (int i = 1; i <= reportColumnCount; i++) // this will aply it form col 1 to 20
                                //{
                                //    xlWorkSheet.Cells[rowIndex, i].Font.Bold = true;
                                //    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                                //}

                                //xlWorkSheet.Cells[rowIndex, 1] = string.Empty;
                                //xlWorkSheet.Cells[rowIndex, 2] = string.Empty;
                                //xlWorkSheet.Cells[rowIndex, 3] = string.Empty;
                                //xlWorkSheet.Cells[rowIndex, 4] = string.Empty;
                                //xlWorkSheet.Cells[rowIndex, 5] = totalLeads;
                                //xlWorkSheet.Cells[rowIndex, 6] = totalTelNo1;

                                //xlWorkSheet.Cells[rowIndex, 9] = totalTelNo2;
                                //xlWorkSheet.Cells[rowIndex, 12] = totalTelNo3;
                                //xlWorkSheet.Cells[rowIndex, 15] = TotalIDs;
                                //xlWorkSheet.Cells[rowIndex, 18] = totalNoIDs;
                                //xlWorkSheet.Cells[rowIndex, 21] = totalIDnumbersByTel1;
                                //xlWorkSheet.Cells[rowIndex, 23] = totalIDnumbersByTel2And3;

                                //xlWorkSheet.Cells[rowIndex, 31] = totalPensioners;
                                //xlWorkSheet.Cells[rowIndex, 33] = totalHousewives;

                                //xlWorkSheet.Cells[rowIndex, 35] = totalRePrimedLeads;
                                //xlWorkSheet.Cells[rowIndex, 38] = totalIndianLeads;
                                //xlWorkSheet.Cells[rowIndex, 41] = totalAfricanLeads;

                                //xlWorkSheet.Cells[rowIndex, 44] = totalReMarketedLeads;
                                //xlWorkSheet.Cells[rowIndex, 47] = totalReMarketedLeads1;
                                //xlWorkSheet.Cells[rowIndex, 50] = totalReMarketedLeads2;
                                //xlWorkSheet.Cells[rowIndex, 53] = totalReMarketedLeads3;
                                //xlWorkSheet.Cells[rowIndex, 56] = totalReMarketedLeads4;

                                //xlWorkSheet.Cells[rowIndex, 59] = totalNOKContactDetailsLeads;

                                //xlWorkSheet.Cells[rowIndex, 62] = totalBenContactDetailsLeads;

                                //xlWorkSheet.Cells[rowIndex, 65] = totalLA2ContactDetailsLeads;

                                //if (batchCodes.Count > 0)
                                //{
                                //    xlWorkSheet.Cells[rowIndex, 22] = Math.Round(totalSalesWithoutIDNumberPercentage / batchCodes.Count, 2) + " %"; //"";

                                //    xlWorkSheet.Cells[rowIndex, 23] = Math.Round(totalIdsGivenByTel1Percent / batchCodes.Count, 2) + " %";
                                //    xlWorkSheet.Cells[rowIndex, 24] = Math.Round(totaliDNosGivenByTel1StlPercent / batchCodes.Count, 2) + " %";
                                //    xlWorkSheet.Cells[rowIndex, 25] = Math.Round(totalIdsGivenByTel2And3Percent / batchCodes.Count, 2) + " %";
                                //    xlWorkSheet.Cells[rowIndex, 26] = Math.Round(totaliDNosGivenByTel2and3StlPercent / batchCodes.Count, 2) + " %";

                                //    if (totalLeads > 0)
                                //    {
                                //        xlWorkSheet.Cells[rowIndex, 7] = Math.Round(totalTelNo1 / totalLeads * 100, 2) + " %";
                                //        if (totalTelNo1 > 0)
                                //        {
                                //            xlWorkSheet.Cells[rowIndex, 8] = Math.Round(totaltelno1xStl / totalTelNo1 * 100, 2) + " %";
                                //        }

                                //        xlWorkSheet.Cells[rowIndex, 10] = Math.Round(totalTelNo2 / totalLeads * 100, 2) + " %";
                                //        if (totalTelNo1 > 0)
                                //        {
                                //            xlWorkSheet.Cells[rowIndex, 11] = Math.Round(totaltelno2xStl / totalTelNo2 * 100, 2) + " %";
                                //        }

                                //        xlWorkSheet.Cells[rowIndex, 13] = Math.Round(totalTelNo3 / totalLeads * 100, 2) + " %";
                                //        if (totalTelNo1 > 0)
                                //        {
                                //            xlWorkSheet.Cells[rowIndex, 14] = Math.Round(totaltelno3xStl / totalTelNo3 * 100, 2) + " %";
                                //        }

                                //        xlWorkSheet.Cells[rowIndex, 16] = Math.Round(TotalIDs / totalLeads * 100, 2) + " %";
                                //        if (TotalIDs > 0)
                                //        {
                                //            xlWorkSheet.Cells[rowIndex, 17] = Math.Round(totalsalesWithIDNumber / TotalIDs * 100, 2) + " %";
                                //        }

                                //        xlWorkSheet.Cells[rowIndex, 19] = Math.Round(totalNoIDs / totalLeads * 100, 2) + " %";
                                //        if (totalNoIDs > 0)
                                //        {
                                //            xlWorkSheet.Cells[rowIndex, 20] = Math.Round(totalsalesWithoutIDNumber / totalNoIDs * 100, 2) + " %";
                                //        }

                                //        xlWorkSheet.Cells[rowIndex, 27] = Math.Round(totalEmailAddresses / totalLeads * 100, 2) + " %";

                                //        if (TotalCountOfLeadsWithAges > 0)
                                //        {
                                //            xlWorkSheet.Cells[rowIndex, 28] = Math.Round(GrandTotalOfLeadsAges / TotalCountOfLeadsWithAges, 2);
                                //        }
                                //        xlRange = xlWorkSheet.Cells[rowIndex, 28];
                                //        xlRange.NumberFormat = "0.00";

                                //        if (TotalCountOfDaysBetweenLeadCreationAndImport > 0)
                                //        {
                                //            xlWorkSheet.Cells[rowIndex, 29] = Math.Round(GrandTotalOfDaysBetweenLeadCreationAndImport / TotalCountOfDaysBetweenLeadCreationAndImport, 2);
                                //        }
                                //        xlRange = xlWorkSheet.Cells[rowIndex, 29];
                                //        xlRange.NumberFormat = "0.00";


                                //        if (TotalCountOfClientsWithAges > 0)
                                //        {
                                //            xlWorkSheet.Cells[rowIndex, 30] = Math.Round(GrandTotalOfClientsAges / TotalCountOfClientsWithAges, 2);
                                //        }
                                //        xlRange = xlWorkSheet.Cells[rowIndex, 30];
                                //        xlRange.NumberFormat = "0.00";

                                //        #region Pensioners & Housewives

                                //        xlWorkSheet.Cells[rowIndex, 32] = Math.Round(totalPensioners / (double)totalLeads * 100, 2) + " %";
                                //        xlWorkSheet.Cells[rowIndex, 34] = Math.Round(totalHousewives / (double)totalLeads * 100, 2) + " %";

                                //        #endregion Pensioners & Housewives

                                //        #region Re-Primed Leads

                                //        xlWorkSheet.Cells[rowIndex, 36] = Math.Round(totalRePrimedLeads / (double)totalLeads * 100, 2) + " %";
                                //        if (totalRePrimedLeads > 0)
                                //        {
                                //            xlWorkSheet.Cells[rowIndex, 36] = Math.Round(totalRePrimedLeadsSTL / (double)totalRePrimedLeads * 100, 2) + " %";
                                //        }
                                //        else
                                //        {
                                //            xlWorkSheet.Cells[rowIndex, 37] = "0 %";
                                //        }

                                //        #endregion Re-Primed Leads

                                //        #region Indian Leads

                                //        xlWorkSheet.Cells[rowIndex, 39] = Math.Round(totalIndianLeads / (double)totalLeads * 100, 2) + " %";

                                //        if (totalIndianLeads > 0)
                                //        {
                                //            xlWorkSheet.Cells[rowIndex, 40] = Math.Round(totalIndianLeadsSTL / totalIndianLeads * 100, 2) + " %";
                                //        }
                                //        else
                                //        {
                                //            xlWorkSheet.Cells[rowIndex, 40] = "0 %";
                                //        }

                                //        #endregion Indian Leads

                                //        #region African Leads

                                //        dResult = totalLeads > 0 ? Math.Round(totalAfricanLeads / totalLeads, 4) : 0;
                                //        xlWorkSheet.Cells[rowIndex, 42] = dResult;
                                //        xlRange = xlWorkSheet.Cells[rowIndex, 42];
                                //        xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                                //        dResult = totalAfricanLeads > 0 ? Math.Round(totalAfricanLeadsSTL / totalAfricanLeads, 4) : 0;
                                //        xlWorkSheet.Cells[rowIndex, 43] = dResult;
                                //        xlRange = xlWorkSheet.Cells[rowIndex, 43];
                                //        xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                                //        #endregion African Leads

                                //        #region ReMarketed Leads

                                //        {
                                //            dResult = totalLeads > 0 ? Math.Round(totalReMarketedLeads / totalLeads, 4) : 0;
                                //            xlWorkSheet.Cells[rowIndex, 45] = dResult;
                                //            xlRange = xlWorkSheet.Cells[rowIndex, 45];
                                //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                                //            dResult = totalReMarketedLeads > 0 ? Math.Round(totalReMarketedLeadsSTL / totalReMarketedLeads, 4) : 0;
                                //            xlWorkSheet.Cells[rowIndex, 46] = dResult;
                                //            xlRange = xlWorkSheet.Cells[rowIndex, 46];
                                //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
                                //        }

                                //        {
                                //            dResult = totalReMarketedLeads > 0 ? Math.Round(totalReMarketedLeads1 / totalReMarketedLeads, 4) : 0;
                                //            xlWorkSheet.Cells[rowIndex, 48] = dResult;
                                //            xlRange = xlWorkSheet.Cells[rowIndex, 48];
                                //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                                //            dResult = totalReMarketedLeads1 > 0 ? Math.Round(totalReMarketedLeadsSTL1 / totalReMarketedLeads1, 4) : 0;
                                //            xlWorkSheet.Cells[rowIndex, 49] = dResult;
                                //            xlRange = xlWorkSheet.Cells[rowIndex, 49];
                                //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
                                //        }

                                //        {
                                //            dResult = totalReMarketedLeads > 0 ? Math.Round(totalReMarketedLeads2 / totalReMarketedLeads, 4) : 0;
                                //            xlWorkSheet.Cells[rowIndex, 51] = dResult;
                                //            xlRange = xlWorkSheet.Cells[rowIndex, 51];
                                //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                                //            dResult = totalReMarketedLeads2 > 0 ? Math.Round(totalReMarketedLeadsSTL2 / totalReMarketedLeads2, 4) : 0;
                                //            xlWorkSheet.Cells[rowIndex, 52] = dResult;
                                //            xlRange = xlWorkSheet.Cells[rowIndex, 52];
                                //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
                                //        }

                                //        {
                                //            dResult = totalReMarketedLeads > 0 ? Math.Round(totalReMarketedLeads3 / totalReMarketedLeads, 4) : 0;
                                //            xlWorkSheet.Cells[rowIndex, 54] = dResult;
                                //            xlRange = xlWorkSheet.Cells[rowIndex, 54];
                                //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                                //            dResult = totalReMarketedLeads3 > 0 ? Math.Round(totalReMarketedLeadsSTL3 / totalReMarketedLeads3, 4) : 0;
                                //            xlWorkSheet.Cells[rowIndex, 55] = dResult;
                                //            xlRange = xlWorkSheet.Cells[rowIndex, 55];
                                //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
                                //        }

                                //        {
                                //            dResult = totalReMarketedLeads > 0 ? Math.Round(totalReMarketedLeads4 / totalReMarketedLeads, 4) : 0;
                                //            xlWorkSheet.Cells[rowIndex, 57] = dResult;
                                //            xlRange = xlWorkSheet.Cells[rowIndex, 57];
                                //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                                //            dResult = totalReMarketedLeads4 > 0 ? Math.Round(totalReMarketedLeadsSTL4 / totalReMarketedLeads4, 4) : 0;
                                //            xlWorkSheet.Cells[rowIndex, 58] = dResult;
                                //            xlRange = xlWorkSheet.Cells[rowIndex, 58];
                                //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
                                //        }

                                //        #endregion ReMarketed Leads

                                //        #region NOK Contact Detail Leads
                                //        {
                                //            dResult = totalLeads > 0 ? Math.Round(totalNOKContactDetailsLeads / totalLeads, 4) : 0;
                                //            xlWorkSheet.Cells[rowIndex, 60] = dResult;
                                //            xlRange = xlWorkSheet.Cells[rowIndex, 60];
                                //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                                //            dResult = totalNOKContactDetailsLeads > 0 ? Math.Round(totalNOKContactDetailsSTL / totalNOKContactDetailsLeads, 4) : 0;
                                //            xlWorkSheet.Cells[rowIndex, 61] = dResult;
                                //            xlRange = xlWorkSheet.Cells[rowIndex, 61];
                                //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
                                //        }
                                //        #endregion NOK Contact Detail Leads

                                //        #region Ben Contact Detail Leads
                                //        {
                                //            dResult = totalLeads > 0 ? Math.Round(totalBenContactDetailsLeads / totalLeads, 4) : 0;
                                //            xlWorkSheet.Cells[rowIndex, 63] = dResult;
                                //            xlRange = xlWorkSheet.Cells[rowIndex, 63];
                                //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                                //            dResult = totalBenContactDetailsLeads > 0 ? Math.Round(totalBenContactDetailsSTL / totalBenContactDetailsLeads, 4) : 0;
                                //            xlWorkSheet.Cells[rowIndex, 64] = dResult;
                                //            xlRange = xlWorkSheet.Cells[rowIndex, 64];
                                //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
                                //        }
                                //        #endregion Ben Contact Detail Leads

                                //        #region LA2 Contact Detail Leads
                                //        {
                                //            dResult = totalLeads > 0 ? Math.Round(totalLA2ContactDetailsLeads / totalLeads, 4) : 0;
                                //            xlWorkSheet.Cells[rowIndex, 66] = dResult;
                                //            xlRange = xlWorkSheet.Cells[rowIndex, 66];
                                //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                                //            dResult = totalLA2ContactDetailsLeads > 0 ? Math.Round(totalLA2ContactDetailsSTL / totalLA2ContactDetailsLeads, 4) : 0;
                                //            xlWorkSheet.Cells[rowIndex, 67] = dResult;
                                //            xlRange = xlWorkSheet.Cells[rowIndex, 67];
                                //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
                                //        }
                                //        #endregion LA2 Contact Detail Leads
                                //    }
                                //else
                                //{
                                //    xlWorkSheet.Cells[rowIndex, 35] = "0 %";
                                //    //xlWorkSheet.Cells[rowIndex, 36] = "0 %";
                                //}
                                ////////////////////////////////////
                                //}
                                //else
                                //{
                                //    //xlWorkSheet.Cells[rowIndex, 6] = "0 %";
                                //    //xlWorkSheet.Cells[rowIndex, 7] = "0 %";
                                //    //xlWorkSheet.Cells[rowIndex, 9] = "0 %";
                                //    //xlWorkSheet.Cells[rowIndex, 10] = "0 %";
                                //    //xlWorkSheet.Cells[rowIndex, 12] = "0 %";
                                //    //xlWorkSheet.Cells[rowIndex, 13] = "0 %";
                                //    //xlWorkSheet.Cells[rowIndex, 15] = "0 %";
                                //    //xlWorkSheet.Cells[rowIndex, 17] = "0 %";
                                //    //xlWorkSheet.Cells[rowIndex, 19] = "0 %";
                                //    //xlWorkSheet.Cells[rowIndex, 20] = "0 %";
                                //    //xlWorkSheet.Cells[rowIndex, 22] = "0 %";
                                //    //xlWorkSheet.Cells[rowIndex, 23] = "0 %";
                                //    //xlWorkSheet.Cells[rowIndex, 24] = "0 %";
                                //    //xlWorkSheet.Cells[rowIndex, 25] = "0 %";
                                //    //xlWorkSheet.Cells[rowIndex, 26] = "0 %";

                                //    xlWorkSheet.Cells[rowIndex, 6] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 7] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 9] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 10] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 12] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 13] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 15] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 16] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 18] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 19] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 21] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 22] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 24] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 25] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 26] = "0%";
                                //    //xlWorkSheet.Cells[rowIndex, 27] = "0%";
                                //    //xlWorkSheet.Cells[rowIndex, 28] = "0";
                                //    //xlWorkSheet.Cells[rowIndex, 29] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 31] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 33] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 35] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 36] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 38] = "0%";
                                //    xlWorkSheet.Cells[rowIndex, 39] = "0%";
                                //}
                                /////////////////////////////////////////////////commented till here//////////////////////////////

                                //add summary

                                //formulaToRow = rowIndex - 1;

                                //////////////////////////////////This was also commented
                                //rowIndex++;
                                //rowIndex++;
                                ///////////////////////////////////Till Here
                                //rowIndex++; rowIndex++; rowIndex++;
                                //Excel.Range desc1 = xlWorkSheet.Cells[rowIndex, 1];
                                //Excel.Range desc2 = xlWorkSheet.Cells[rowIndex, 6];

                                //Excel.Range descrange = (Excel.Range)xlWorkSheet.get_Range(desc1, desc2);
                                //descrange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                //descrange.Font.Bold = true;
                                //descrange.Merge(true);
                                //descrange.Borders.Weight = Excel.XlBorderWeight.xlMedium;
                                //xlWorkSheet.Cells[rowIndex, 1] = "Column Descriptions";
                                //xlWorkSheet.Cells[rowIndex, 1].Font.Bold = true;
                                //rowIndex++;
                                //Excel.Range descsub1 = xlWorkSheet.Cells[rowIndex, 1];
                                //Excel.Range descsub2 = xlWorkSheet.Cells[rowIndex, 2];
                                //Excel.Range descSubrange = (Excel.Range)xlWorkSheet.get_Range(descsub1, descsub2);
                                //descSubrange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                //descSubrange.Font.Bold = true;
                                //descSubrange.Merge(true);
                                //descSubrange.Borders.Weight = Excel.XlBorderWeight.xlMedium;
                                //xlWorkSheet.Cells[rowIndex, 1] = "Column Name";

                                //Excel.Range descsubb1 = xlWorkSheet.Cells[rowIndex, 3];
                                //Excel.Range descsubb2 = xlWorkSheet.Cells[rowIndex, 6];
                                //Excel.Range descSubbrange = (Excel.Range)xlWorkSheet.get_Range(descsubb1, descsubb2);
                                //descSubbrange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                //descSubbrange.Font.Bold = true;
                                //descSubbrange.Merge(true);
                                //descSubbrange.Borders.Weight = Excel.XlBorderWeight.xlMedium;
                                //xlWorkSheet.Cells[rowIndex, 3] = "Description";
                                //rowIndex++;

                                //Excel.Range descval1 = xlWorkSheet.Cells[rowIndex, 1];
                                //Excel.Range descval2 = xlWorkSheet.Cells[rowIndex, 2];
                                //Excel.Range descval1range = (Excel.Range)xlWorkSheet.get_Range(descval1, descval2);
                                //descval1range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                                //descval1range.Merge(true);
                                //descval1range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                                //xlWorkSheet.Cells[rowIndex, 1] = "Average age of leads (Days)";

                                //Excel.Range desval3 = xlWorkSheet.Cells[rowIndex, 3];
                                //Excel.Range desval4 = xlWorkSheet.Cells[rowIndex, 6];
                                //Excel.Range descval2range = (Excel.Range)xlWorkSheet.get_Range(desval3, desval4);
                                //descval2range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                                //descval2range.Merge(true);
                                //descval2range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                                //xlWorkSheet.Cells[rowIndex, 3] = "Last PL contact to UDM import";

                                #endregion Totals

                                #region Hiding the unused columns

                                // See

                                var hiddenRange = xlWorkSheet.Range[xlWorkSheet.Cells[1, 20], xlWorkSheet.Cells[1, 25]];
                                hiddenRange.EntireColumn.Hidden = true;

                                #endregion Hiding the unused columns

                            }

                            /////////////Totals per campaign /////////////////////////////////////

                            //    for (int i = 1; i <= reportColumnCount; i++) // this will aply it form col 1 to 20
                            //    {
                            //        xlWorkSheet.Cells[rowIndex, i].Font.Bold = false;
                            //        xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            //    }

                            //rowIndex++;
                            //rowIndex++;





                            if (_lstSelectedFKINCampaignIDs.Count >= 1)
                            {
                                //rowIndex++;
                                totalSelectedCampaigns++;
                            }

                            //int totalSelectedCampaigns = _lstSelectedFKINCampaignIDs.Count; 

                            rowIndex--;

                            if (totalSelectedCampaigns >= _lstSelectedFKINCampaignIDs.Count)
                            {
                                if (reportRowTotal == 0)
                                {
                                    reportRowTotal = rowIndex;
                                }



                                for (int i = 1; i <= reportColumnCount; i++) // this will aply it form col 1 to 20
                                {
                                    xlWorkSheet.Cells[reportRowTotal + 1, i].Font.Bold = true;
                                    xlWorkSheet.Cells[reportRowTotal + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                                }

                                /////////////////////////////////////Final Single Total/////////////////////////////////////////////

                                //xlWorkSheet.Cells[reportRowTotal + 1, 5].Font.Bold = false;
                                //xlWorkSheet.Cells[reportRowTotal + 1, 5].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                                xlWorkSheet.Cells[reportRowTotal + 1, 5] = "=SUM(E8:E" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 6] = "=SUM(F8:F" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 7] = "=AVERAGE(G8:G" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 8] = "=AVERAGE(H8:H" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 9] = "=SUM(I8:I" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 10] = "=AVERAGE(J8:J" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 11] = "=AVERAGE(K8:K" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 12] = "=SUM(L8:L" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 13] = "=AVERAGE(M8:M" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 14] = "=AVERAGE(N8:N" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 15] = "=SUM(O8:O" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 16] = "=AVERAGE(P8:P" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 17] = "=AVERAGE(Q8:Q" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 18] = "=SUM(R8:R" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 19] = "=AVERAGE(S8:S" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 20] = "=SUM(T8:T" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 21] = "=SUM(U8:U" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 22] = "=SUM(V8:V" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 23] = "=SUM(W8:W" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 24] = "=SUM(X8:X" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 25] = "=SUM(Y8:Y" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 26] = "=AVERAGE(Z8:Z" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 27] = "=AVERAGE(AA8:AA" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 28] = "=SUM(AB8:AB" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 29] = "=SUM(AC8:AC" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 30] = "=AVERAGE(AD8:AD" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 31] = "=SUM(AE8:AE" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 32] = "=AVERAGE(AF8:AF" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 33] = "=SUM(AG8:AG" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 34] = "=AVERAGE(AH8:AH" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 35] = "=SUM(AI8:AI" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 36] = "=AVERAGE(AJ8:AJ" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 37] = "=AVERAGE(AK8:AK" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 38] = "=SUM(AL8:AL" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 39] = "=AVERAGE(AM8:AM" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 40] = "=AVERAGE(AN8:AN" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 41] = "=SUM(AO8:AO" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 42] = "=AVERAGE(AP8:AP" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 43] = "=AVERAGE(AQ8:AQ" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 44] = "=SUM(AR8:AR" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 45] = "=AVERAGE(AS8:AS" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 46] = "=AVERAGE(AT8:AT" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 47] = "=SUM(AU8:AU" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 48] = "=AVERAGE(AV8:AV" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 49] = "=AVERAGE(AW8:AW" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 50] = "=SUM(AX8:AX" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 51] = "=AVERAGE(AY8:AY" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 52] = "=AVERAGE(AZ8:AZ" + reportRowTotal.ToString() + ")";

                                xlWorkSheet.Cells[reportRowTotal + 1, 53] = "=SUM(BA8:BA" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 54] = "=AVERAGE(BB8:BB" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 55] = "=AVERAGE(BC8:BC" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 56] = "=SUM(BD8:BD" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 57] = "=AVERAGE(BE8:BE" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 58] = "=AVERAGE(BF8:BF" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 59] = "=SUM(BG8:BG" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 60] = "=AVERAGE(BH8:BH" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 61] = "=AVERAGE(BI8:BI" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 62] = "=SUM(BJ8:BJ" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 63] = "=AVERAGE(BK8:BK" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 64] = "=AVERAGE(BL8:BL" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 65] = "=SUM(BM8:BM" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 66] = "=AVERAGE(BN8:BN" + reportRowTotal.ToString() + ")";
                                xlWorkSheet.Cells[reportRowTotal + 1, 67] = "=AVERAGE(BO8:BO" + reportRowTotal.ToString() + ")";
                            }
                            //////////////////////////////////////////End of Single Total////////////////////////////////////////////

                            //int totalRows = 1;
                            //reportRowTotal--;

                            //if ((bool)record.Cells["Select"].Value)
                            //{

                            //    for (int j = 0; j < totalRows; j++)
                            //    {


                            //        for (int i = 1; i <= reportColumnCount; i++) // this will aply it form col 1 to 20
                            //        {
                            //            xlWorkSheet.Cells[reportRowTotal + 1, i].Font.Bold = true;
                            //            xlWorkSheet.Cells[reportRowTotal + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            //        }
                            //    }
                            //}


                            //xlWorkSheet.Cells[reportColumnCount, 3] = "=SUM(C2:C" + reportColumnCount + ")";
                            //xlWorkSheet.Cells[reportColumnCount, 4] = "=SUM(D2:D" + reportColumnCount + ")";

                            //    xlWorkSheet.Cells[rowIndex, 1] = string.Empty;
                            //    xlWorkSheet.Cells[rowIndex, 2] = string.Empty;
                            //    xlWorkSheet.Cells[rowIndex, 3] = string.Empty;
                            //    xlWorkSheet.Cells[rowIndex, 4] = string.Empty;
                            //    xlWorkSheet.Cells[rowIndex, 5] = totalLeads;
                            //    xlWorkSheet.Cells[rowIndex, 6] = totalTelNo1;

                            //    xlWorkSheet.Cells[rowIndex, 9] = totalTelNo2;
                            //    xlWorkSheet.Cells[rowIndex, 12] = totalTelNo3;
                            //    xlWorkSheet.Cells[rowIndex, 15] = TotalIDs;
                            //    xlWorkSheet.Cells[rowIndex, 18] = totalNoIDs;
                            //    xlWorkSheet.Cells[rowIndex, 21] = totalIDnumbersByTel1;
                            //    xlWorkSheet.Cells[rowIndex, 23] = totalIDnumbersByTel2And3;

                            //    xlWorkSheet.Cells[rowIndex, 31] = totalPensioners;
                            //    xlWorkSheet.Cells[rowIndex, 33] = totalHousewives;

                            //    xlWorkSheet.Cells[rowIndex, 35] = totalRePrimedLeads;
                            //    xlWorkSheet.Cells[rowIndex, 38] = totalIndianLeads;
                            //    xlWorkSheet.Cells[rowIndex, 41] = totalAfricanLeads;

                            //    xlWorkSheet.Cells[rowIndex, 44] = totalReMarketedLeads;
                            //    xlWorkSheet.Cells[rowIndex, 47] = totalReMarketedLeads1;
                            //    xlWorkSheet.Cells[rowIndex, 50] = totalReMarketedLeads2;
                            //    xlWorkSheet.Cells[rowIndex, 53] = totalReMarketedLeads3;
                            //    xlWorkSheet.Cells[rowIndex, 56] = totalReMarketedLeads4;

                            //    xlWorkSheet.Cells[rowIndex, 59] = totalNOKContactDetailsLeads;

                            //    xlWorkSheet.Cells[rowIndex, 62] = totalBenContactDetailsLeads;

                            //    xlWorkSheet.Cells[rowIndex, 65] = totalLA2ContactDetailsLeads;

                            //if (batchCodes.Count > 0)
                            //{
                            //    xlWorkSheet.Cells[rowIndex, 22] = Math.Round(totalSalesWithoutIDNumberPercentage / batchCodes.Count, 2) + " %"; //"";

                            //    xlWorkSheet.Cells[rowIndex, 23] = Math.Round(totalIdsGivenByTel1Percent / batchCodes.Count, 2) + " %";
                            //    xlWorkSheet.Cells[rowIndex, 24] = Math.Round(totaliDNosGivenByTel1StlPercent / batchCodes.Count, 2) + " %";
                            //    xlWorkSheet.Cells[rowIndex, 25] = Math.Round(totalIdsGivenByTel2And3Percent / batchCodes.Count, 2) + " %";
                            //    xlWorkSheet.Cells[rowIndex, 26] = Math.Round(totaliDNosGivenByTel2and3StlPercent / batchCodes.Count, 2) + " %";

                            //    if (totalLeads > 0)
                            //    {
                            //        xlWorkSheet.Cells[rowIndex, 7] = Math.Round(totalTelNo1 / totalLeads * 100, 2) + " %";
                            //        if (totalTelNo1 > 0)
                            //        {
                            //            xlWorkSheet.Cells[rowIndex, 8] = Math.Round(totaltelno1xStl / totalTelNo1 * 100, 2) + " %";
                            //        }

                            //        xlWorkSheet.Cells[rowIndex, 10] = Math.Round(totalTelNo2 / totalLeads * 100, 2) + " %";
                            //        if (totalTelNo1 > 0)
                            //        {
                            //            xlWorkSheet.Cells[rowIndex, 11] = Math.Round(totaltelno2xStl / totalTelNo2 * 100, 2) + " %";
                            //        }

                            //        xlWorkSheet.Cells[rowIndex, 13] = Math.Round(totalTelNo3 / totalLeads * 100, 2) + " %";
                            //        if (totalTelNo1 > 0)
                            //        {
                            //            xlWorkSheet.Cells[rowIndex, 14] = Math.Round(totaltelno3xStl / totalTelNo3 * 100, 2) + " %";
                            //        }

                            //        xlWorkSheet.Cells[rowIndex, 16] = Math.Round(TotalIDs / totalLeads * 100, 2) + " %";
                            //        if (TotalIDs > 0)
                            //        {
                            //            xlWorkSheet.Cells[rowIndex, 17] = Math.Round(totalsalesWithIDNumber / TotalIDs * 100, 2) + " %";
                            //        }

                            //        xlWorkSheet.Cells[rowIndex, 19] = Math.Round(totalNoIDs / totalLeads * 100, 2) + " %";
                            //        if (totalNoIDs > 0)
                            //        {
                            //            xlWorkSheet.Cells[rowIndex, 20] = Math.Round(totalsalesWithoutIDNumber / totalNoIDs * 100, 2) + " %";
                            //        }

                            //        xlWorkSheet.Cells[rowIndex, 27] = Math.Round(totalEmailAddresses / totalLeads * 100, 2) + " %";

                            //        if (TotalCountOfLeadsWithAges > 0)
                            //        {
                            //            xlWorkSheet.Cells[rowIndex, 28] = Math.Round(GrandTotalOfLeadsAges / TotalCountOfLeadsWithAges, 2);
                            //        }
                            //        xlRange = xlWorkSheet.Cells[rowIndex, 28];
                            //        xlRange.NumberFormat = "0.00";

                            //        if (TotalCountOfDaysBetweenLeadCreationAndImport > 0)
                            //        {
                            //            xlWorkSheet.Cells[rowIndex, 29] = Math.Round(GrandTotalOfDaysBetweenLeadCreationAndImport / TotalCountOfDaysBetweenLeadCreationAndImport, 2);
                            //        }
                            //        xlRange = xlWorkSheet.Cells[rowIndex, 29];
                            //        xlRange.NumberFormat = "0.00";


                            //        if (TotalCountOfClientsWithAges > 0)
                            //        {
                            //            xlWorkSheet.Cells[rowIndex, 30] = Math.Round(GrandTotalOfClientsAges / TotalCountOfClientsWithAges, 2);
                            //        }
                            //        xlRange = xlWorkSheet.Cells[rowIndex, 30];
                            //        xlRange.NumberFormat = "0.00";

                            //        #region Pensioners & Housewives

                            //        xlWorkSheet.Cells[rowIndex, 32] = Math.Round(totalPensioners / (double)totalLeads * 100, 2) + " %";
                            //        xlWorkSheet.Cells[rowIndex, 34] = Math.Round(totalHousewives / (double)totalLeads * 100, 2) + " %";

                            //        #endregion Pensioners & Housewives

                            //        #region Re-Primed Leads

                            //        xlWorkSheet.Cells[rowIndex, 36] = Math.Round(totalRePrimedLeads / (double)totalLeads * 100, 2) + " %";
                            //        if (totalRePrimedLeads > 0)
                            //        {
                            //            xlWorkSheet.Cells[rowIndex, 36] = Math.Round(totalRePrimedLeadsSTL / (double)totalRePrimedLeads * 100, 2) + " %";
                            //        }
                            //        else
                            //        {
                            //            xlWorkSheet.Cells[rowIndex, 37] = "0 %";
                            //        }

                            //        #endregion Re-Primed Leads

                            //        #region Indian Leads

                            //        xlWorkSheet.Cells[rowIndex, 39] = Math.Round(totalIndianLeads / (double)totalLeads * 100, 2) + " %";

                            //        if (totalIndianLeads > 0)
                            //        {
                            //            xlWorkSheet.Cells[rowIndex, 40] = Math.Round(totalIndianLeadsSTL / totalIndianLeads * 100, 2) + " %";
                            //        }
                            //        else
                            //        {
                            //            xlWorkSheet.Cells[rowIndex, 40] = "0 %";
                            //        }

                            //        #endregion Indian Leads

                            //        #region African Leads

                            //        dResult = totalLeads > 0 ? Math.Round(totalAfricanLeads / totalLeads, 4) : 0;
                            //        xlWorkSheet.Cells[rowIndex, 42] = dResult;
                            //        xlRange = xlWorkSheet.Cells[rowIndex, 42];
                            //        xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                            //        dResult = totalAfricanLeads > 0 ? Math.Round(totalAfricanLeadsSTL / totalAfricanLeads, 4) : 0;
                            //        xlWorkSheet.Cells[rowIndex, 43] = dResult;
                            //        xlRange = xlWorkSheet.Cells[rowIndex, 43];
                            //        xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                            //        #endregion African Leads

                            //        #region ReMarketed Leads

                            //        {
                            //            dResult = totalLeads > 0 ? Math.Round(totalReMarketedLeads / totalLeads, 4) : 0;
                            //            xlWorkSheet.Cells[rowIndex, 45] = dResult;
                            //            xlRange = xlWorkSheet.Cells[rowIndex, 45];
                            //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                            //            dResult = totalReMarketedLeads > 0 ? Math.Round(totalReMarketedLeadsSTL / totalReMarketedLeads, 4) : 0;
                            //            xlWorkSheet.Cells[rowIndex, 46] = dResult;
                            //            xlRange = xlWorkSheet.Cells[rowIndex, 46];
                            //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
                            //        }

                            //        {
                            //            dResult = totalReMarketedLeads > 0 ? Math.Round(totalReMarketedLeads1 / totalReMarketedLeads, 4) : 0;
                            //            xlWorkSheet.Cells[rowIndex, 48] = dResult;
                            //            xlRange = xlWorkSheet.Cells[rowIndex, 48];
                            //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                            //            dResult = totalReMarketedLeads1 > 0 ? Math.Round(totalReMarketedLeadsSTL1 / totalReMarketedLeads1, 4) : 0;
                            //            xlWorkSheet.Cells[rowIndex, 49] = dResult;
                            //            xlRange = xlWorkSheet.Cells[rowIndex, 49];
                            //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
                            //        }

                            //        {
                            //            dResult = totalReMarketedLeads > 0 ? Math.Round(totalReMarketedLeads2 / totalReMarketedLeads, 4) : 0;
                            //            xlWorkSheet.Cells[rowIndex, 51] = dResult;
                            //            xlRange = xlWorkSheet.Cells[rowIndex, 51];
                            //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                            //            dResult = totalReMarketedLeads2 > 0 ? Math.Round(totalReMarketedLeadsSTL2 / totalReMarketedLeads2, 4) : 0;
                            //            xlWorkSheet.Cells[rowIndex, 52] = dResult;
                            //            xlRange = xlWorkSheet.Cells[rowIndex, 52];
                            //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
                            //        }

                            //        {
                            //            dResult = totalReMarketedLeads > 0 ? Math.Round(totalReMarketedLeads3 / totalReMarketedLeads, 4) : 0;
                            //            xlWorkSheet.Cells[rowIndex, 54] = dResult;
                            //            xlRange = xlWorkSheet.Cells[rowIndex, 54];
                            //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                            //            dResult = totalReMarketedLeads3 > 0 ? Math.Round(totalReMarketedLeadsSTL3 / totalReMarketedLeads3, 4) : 0;
                            //            xlWorkSheet.Cells[rowIndex, 55] = dResult;
                            //            xlRange = xlWorkSheet.Cells[rowIndex, 55];
                            //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
                            //        }

                            //        {
                            //            dResult = totalReMarketedLeads > 0 ? Math.Round(totalReMarketedLeads4 / totalReMarketedLeads, 4) : 0;
                            //            xlWorkSheet.Cells[rowIndex, 57] = dResult;
                            //            xlRange = xlWorkSheet.Cells[rowIndex, 57];
                            //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                            //            dResult = totalReMarketedLeads4 > 0 ? Math.Round(totalReMarketedLeadsSTL4 / totalReMarketedLeads4, 4) : 0;
                            //            xlWorkSheet.Cells[rowIndex, 58] = dResult;
                            //            xlRange = xlWorkSheet.Cells[rowIndex, 58];
                            //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
                            //        }

                            //        #endregion ReMarketed Leads

                            //        #region NOK Contact Detail Leads
                            //        {
                            //            dResult = totalLeads > 0 ? Math.Round(totalNOKContactDetailsLeads / totalLeads, 4) : 0;
                            //            xlWorkSheet.Cells[rowIndex, 60] = dResult;
                            //            xlRange = xlWorkSheet.Cells[rowIndex, 60];
                            //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                            //            dResult = totalNOKContactDetailsLeads > 0 ? Math.Round(totalNOKContactDetailsSTL / totalNOKContactDetailsLeads, 4) : 0;
                            //            xlWorkSheet.Cells[rowIndex, 61] = dResult;
                            //            xlRange = xlWorkSheet.Cells[rowIndex, 61];
                            //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
                            //        }
                            //        #endregion NOK Contact Detail Leads

                            //        #region Ben Contact Detail Leads
                            //        {
                            //            dResult = totalLeads > 0 ? Math.Round(totalBenContactDetailsLeads / totalLeads, 4) : 0;
                            //            xlWorkSheet.Cells[rowIndex, 63] = dResult;
                            //            xlRange = xlWorkSheet.Cells[rowIndex, 63];
                            //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                            //            dResult = totalBenContactDetailsLeads > 0 ? Math.Round(totalBenContactDetailsSTL / totalBenContactDetailsLeads, 4) : 0;
                            //            xlWorkSheet.Cells[rowIndex, 64] = dResult;
                            //            xlRange = xlWorkSheet.Cells[rowIndex, 64];
                            //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
                            //        }
                            //        #endregion Ben Contact Detail Leads

                            //        #region LA2 Contact Detail Leads
                            //        {
                            //            dResult = totalLeads > 0 ? Math.Round(totalLA2ContactDetailsLeads / totalLeads, 4) : 0;
                            //            xlWorkSheet.Cells[rowIndex, 66] = dResult;
                            //            xlRange = xlWorkSheet.Cells[rowIndex, 66];
                            //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

                            //            dResult = totalLA2ContactDetailsLeads > 0 ? Math.Round(totalLA2ContactDetailsSTL / totalLA2ContactDetailsLeads, 4) : 0;
                            //            xlWorkSheet.Cells[rowIndex, 67] = dResult;
                            //            xlRange = xlWorkSheet.Cells[rowIndex, 67];
                            //            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
                            //        }
                            //        #endregion LA2 Contact Detail Leads
                            //    }
                            //    else
                            //    {
                            //        xlWorkSheet.Cells[rowIndex, 35] = "0 %";
                            //        //xlWorkSheet.Cells[rowIndex, 36] = "0 %";
                            //    }
                            //    //////////////////////////////////
                            //}
                            //else
                            //{
                            //    //xlWorkSheet.Cells[rowIndex, 6] = "0 %";
                            //    //xlWorkSheet.Cells[rowIndex, 7] = "0 %";
                            //    //xlWorkSheet.Cells[rowIndex, 9] = "0 %";
                            //    //xlWorkSheet.Cells[rowIndex, 10] = "0 %";
                            //    //xlWorkSheet.Cells[rowIndex, 12] = "0 %";
                            //    //xlWorkSheet.Cells[rowIndex, 13] = "0 %";
                            //    //xlWorkSheet.Cells[rowIndex, 15] = "0 %";
                            //    //xlWorkSheet.Cells[rowIndex, 17] = "0 %";
                            //    //xlWorkSheet.Cells[rowIndex, 19] = "0 %";
                            //    //xlWorkSheet.Cells[rowIndex, 20] = "0 %";
                            //    //xlWorkSheet.Cells[rowIndex, 22] = "0 %";
                            //    //xlWorkSheet.Cells[rowIndex, 23] = "0 %";
                            //    //xlWorkSheet.Cells[rowIndex, 24] = "0 %";
                            //    //xlWorkSheet.Cells[rowIndex, 25] = "0 %";
                            //    //xlWorkSheet.Cells[rowIndex, 26] = "0 %";

                            //    xlWorkSheet.Cells[rowIndex, 6] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 7] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 9] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 10] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 12] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 13] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 15] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 16] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 18] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 19] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 21] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 22] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 24] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 25] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 26] = "0%";
                            //    //xlWorkSheet.Cells[rowIndex, 27] = "0%";
                            //    //xlWorkSheet.Cells[rowIndex, 28] = "0";
                            //    //xlWorkSheet.Cells[rowIndex, 29] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 31] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 33] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 35] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 36] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 38] = "0%";
                            //    xlWorkSheet.Cells[rowIndex, 39] = "0%";
                            //}
                            //    ///////////////////////End of Total per campaign///////////////////////////////////////
                            //}


                            #endregion

                            //}

                        }

                    }

                        #region Display Excel Document
                        //string filePathAndName = GlobalSettings.UserFolder + campaignName + " Incoming Batch Analysis  Report ~ " + DateTime.Now.Millisecond + ".xls";
                        string filePathAndName = string.Format("{0}{1} Incoming Batch Analysis Report {2}.xls", GlobalSettings.UserFolder, null, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                        //Save excel document
                        //  wbReport.Save(filePathAndName);
                        xlWorkBook.SaveAs(filePathAndName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                        xlWorkBook.Close(true, misValue, misValue);

                        xlApp.Quit();

                        //Display excel document
                        Process.Start(filePathAndName);

                        #endregion


                    }

                
            




            catch (Exception ex)
            {
                HandleException(ex);
                MessageBox.Show(ex.ToString());
            }

            finally
            {
                SetCursor(Cursors.Arrow);
            }
        }

        //private void AddWorksheetsToWorkbook(IEnumerable<DataRecord> campaigns)
        //{


        //        SetCursor(Cursors.Wait);

        //        Excel.Application xlApp = new Excel.Application();

        //        Excel.Workbook xlWorkBook;
        //        var xlWorkSheet = new Excel.Worksheet[300];
        //        Excel.Range xlRange;

        //        //int s = 0;
        //        int rowIndex = 0;
        //        int formulaFromRow = 0;
        //        int formulaToRow = 0;
        //        int tableFirstRow = 0;
        //        int reportColumnCount = 67; //66//57//39

        //        if (campaigns != null)
        //        {


        //                foreach (DataRecord record in campaigns)
        //                {



        //                    if ((bool)record.Cells["Select"].Value)
        //                    {

        //                        //long campaignID = Convert.ToInt32(record.Cells["CampaignID"].Value);
        //                        //string campaignName = record.Cells["CampaignName"].Value.ToString();
        //                        //string campaignCode = record.Cells["CampaignCode"].Value.ToString();


        //                        //rowIndex = 8;
        //                        //formulaFromRow = 8;
        //                        //formulaToRow = 0;
        //                        //tableFirstRow = 6;

        //                        #region Setup excel documents


        //                        object misValue = System.Reflection.Missing.Value;
        //                        xlWorkBook = xlApp.Workbooks.Add(misValue);



        //                        var newWorksheet = new Excel.Worksheet[300];


        //                        int l = 0;

        //                        int s = 0;

        //                        foreach (DataRecord dr in campaigns)
        //                        {


        //                            if ((bool)dr.Cells["Select"].Value)
        //                            {

        //                                newWorksheet[0] = (Excel.Worksheet)xlWorkBook.Worksheets.Add();


        //                                newWorksheet[0].Name = "Sheet" + l + "{0}";

        //                                l++;
        //                            }


        //                        }

        //                //s++;

        //                //xlWorkSheet[t] = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(++s);



        //                #endregion Setup excel documents


        //                        int t = 1;

        //                        //t++;


        //                        foreach (DataRecord dr in campaigns)
        //                        {


        //                            if ((bool)dr.Cells["Select"].Value)
        //                            {




        //                                long campaignID = Convert.ToInt32(dr.Cells["CampaignID"].Value);
        //                                string campaignName = dr.Cells["CampaignName"].Value.ToString();
        //                                string codeCampaign = dr.Cells["CampaignCode"].Value.ToString();

        //                                rowIndex = 8;
        //                                formulaFromRow = 8;
        //                                formulaToRow = 0;
        //                                tableFirstRow = 6;



        //                            xlWorkSheet[t] = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(t);

        //                            #region Get report data from database

        //                            System.Data.DataTable dtLeadAllocationData;
        //                            System.Data.DataTable dtTotals;

        //                            SqlParameter[] parameters = new SqlParameter[3];
        //                            parameters[0] = new SqlParameter("@CampaignID", campaignID);
        //                            parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
        //                            parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

        //                            DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spINReportIncomingBatchAnalysis", parameters);

        //                            if (dsLeadAllocationData.Tables.Count > 0)
        //                            {
        //                                if (dsLeadAllocationData.Tables[0].Rows.Count > 0)
        //                                {

        //                                    dtLeadAllocationData = dsLeadAllocationData.Tables[0].AsEnumerable().OrderByDescending(x => x["ImportDate"]).CopyToDataTable();

        //                                }
        //                                else
        //                                {
        //                                    dtLeadAllocationData = dsLeadAllocationData.Tables[0];
        //                                }

        //                                dtTotals = dsLeadAllocationData.Tables[1];

        //                                if (dtLeadAllocationData.Rows.Count == 0)
        //                                {
        //                                    if (campaigns.Count() == 1)
        //                                    {
        //                                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
        //                                        {
        //                                            ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + codeCampaign + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
        //                                        });

        //                                        continue;
        //                                    }
        //                                }
        //                            }

        //                            else
        //                            {

        //                                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
        //                                {
        //                                    ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + codeCampaign + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
        //                                });

        //                                continue;

        //                            }


        //                            #endregion Get report data from database


        //                            #region report data



        //                            {


        //                                #region Adding the report heading

        //                                Excel.Range h1 = xlWorkSheet[t].Cells[1, 2];
        //                                Excel.Range h2 = xlWorkSheet[t].Cells[1, reportColumnCount];
        //                                Excel.Range headingRange = (Excel.Range)xlWorkSheet[t].get_Range(h1, h2);

        //                                headingRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                headingRange.Font.Bold = true;
        //                                headingRange.Font.Size = 16;
        //                                headingRange.Borders.Weight = Excel.XlBorderWeight.xlThick;
        //                                headingRange.Merge(true);
        //                                xlWorkSheet[t].Cells[1, 2] = string.Format("Batch Analysis Report");

        //                                #endregion Adding the report heading

        //                                #region Adding the report subheading

        //                                Excel.Range sh1 = xlWorkSheet[t].Cells[3, 1];
        //                                Excel.Range sh2 = xlWorkSheet[t].Cells[3, reportColumnCount];
        //                                Excel.Range subheadingRange = (Excel.Range)xlWorkSheet[t].get_Range(sh1, sh2);

        //                                subheadingRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                subheadingRange.Font.Bold = true;
        //                                //subheadingRange.Font.Size = 11;
        //                                //headingRange.Borders.Weight = Excel.XlBorderWeight.xlThick;
        //                                subheadingRange.Merge(true);
        //                                xlWorkSheet[t].Cells[3, 1] = string.Format("For the period between {0} and {1}", _startDate.ToString("d MMMM yyyy"), _endDate.ToString("d MMMM yyyy"));

        //                                #endregion Adding the report subheading

        //                                #region Adding the report date

        //                                Excel.Range d1 = xlWorkSheet[t].Cells[4, 63];
        //                                Excel.Range d2 = xlWorkSheet[t].Cells[4, 64];
        //                                Excel.Range dateLabelCellRange = (Excel.Range)xlWorkSheet[t].get_Range(d1, d2);
        //                                dateLabelCellRange.Merge(true);
        //                                dateLabelCellRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
        //                                dateLabelCellRange.Font.Bold = true;

        //                                Excel.Range d3 = xlWorkSheet[t].Cells[4, 65];
        //                                Excel.Range d4 = xlWorkSheet[t].Cells[4, 66];
        //                                Excel.Range dateCellRange = (Excel.Range)xlWorkSheet[t].get_Range(d3, d4);
        //                                dateCellRange.Merge(true);

        //                                xlWorkSheet[t].Cells[4, 63] = "Date Generated:";
        //                                xlWorkSheet[t].Cells[4, 65] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //                                #endregion Adding the report date

        //                                #region Partitioning the columns

        //                                //headers
        //                                xlWorkSheet[t].Cells[tableFirstRow, 5] = "Contact Numbers";
        //                                xlWorkSheet[t].Cells[tableFirstRow, 14] = "ID Numbers vs Date of Birth";
        //                                xlWorkSheet[t].Cells[tableFirstRow, 30] = "Pensioners & Housewives";
        //                                xlWorkSheet[t].Cells[tableFirstRow, 34] = "Re-Primed Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow, 37] = "Indian Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow, 40] = "African Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow, 43] = "Re-Marketed Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow, 46] = "Number of Times Re-Marketed to";

        //                                //new headers
        //                                xlWorkSheet[t].Cells[tableFirstRow, 58] = "Next of Kin Contact Details";
        //                                xlWorkSheet[t].Cells[tableFirstRow, 61] = "Beneficiary Contact Details";
        //                                xlWorkSheet[t].Cells[tableFirstRow, 64] = "LA2 Contact Details";

        //                                Excel.Range c1 = xlWorkSheet[t].Cells[tableFirstRow, 5];
        //                                Excel.Range c2 = xlWorkSheet[t].Cells[tableFirstRow, 13];
        //                                Excel.Range c3 = xlWorkSheet[t].Cells[tableFirstRow, 14];
        //                                Excel.Range c4 = xlWorkSheet[t].Cells[tableFirstRow, 19];

        //                                Excel.Range c5 = xlWorkSheet[t].Cells[tableFirstRow, 30];
        //                                Excel.Range c6 = xlWorkSheet[t].Cells[tableFirstRow, 33];

        //                                Excel.Range c7 = xlWorkSheet[t].Cells[tableFirstRow, 34];
        //                                Excel.Range c8 = xlWorkSheet[t].Cells[tableFirstRow, 36];

        //                                Excel.Range c9 = xlWorkSheet[t].Cells[tableFirstRow, 37];
        //                                Excel.Range c10 = xlWorkSheet[t].Cells[tableFirstRow, 39];

        //                                Excel.Range c11 = xlWorkSheet[t].Cells[tableFirstRow, 40];
        //                                Excel.Range c12 = xlWorkSheet[t].Cells[tableFirstRow, 42];

        //                                Excel.Range c13 = xlWorkSheet[t].Cells[tableFirstRow, 43];
        //                                Excel.Range c14 = xlWorkSheet[t].Cells[tableFirstRow, 45];

        //                                Excel.Range c15 = xlWorkSheet[t].Cells[tableFirstRow, 46];
        //                                Excel.Range c16 = xlWorkSheet[t].Cells[tableFirstRow, 57];
        //                                //****************************************************/
        //                                Excel.Range c17 = xlWorkSheet[t].Cells[tableFirstRow, 58];
        //                                Excel.Range c18 = xlWorkSheet[t].Cells[tableFirstRow, 60];

        //                                Excel.Range c19 = xlWorkSheet[t].Cells[tableFirstRow, 61];
        //                                Excel.Range c20 = xlWorkSheet[t].Cells[tableFirstRow, 63];

        //                                Excel.Range c21 = xlWorkSheet[t].Cells[tableFirstRow, 64];
        //                                Excel.Range c22 = xlWorkSheet[t].Cells[tableFirstRow, 66];

        //                                Excel.Range range = (Excel.Range)xlWorkSheet[t].get_Range(c1, c2);
        //                                Excel.Range range2 = (Excel.Range)xlWorkSheet[t].get_Range(c3, c4);
        //                                Excel.Range range3 = (Excel.Range)xlWorkSheet[t].get_Range(c5, c6);
        //                                Excel.Range range4 = (Excel.Range)xlWorkSheet[t].get_Range(c7, c8);
        //                                Excel.Range range5 = (Excel.Range)xlWorkSheet[t].get_Range(c9, c10);
        //                                Excel.Range range6 = (Excel.Range)xlWorkSheet[t].get_Range(c11, c12);
        //                                Excel.Range range7 = (Excel.Range)xlWorkSheet[t].get_Range(c13, c14);
        //                                Excel.Range range8 = (Excel.Range)xlWorkSheet[t].get_Range(c15, c16);

        //                                Excel.Range range9 = (Excel.Range)xlWorkSheet[t].get_Range(c17, c18);
        //                                Excel.Range range10 = (Excel.Range)xlWorkSheet[t].get_Range(c19, c20);
        //                                Excel.Range range11 = (Excel.Range)xlWorkSheet[t].get_Range(c21, c22);

        //                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                range.Font.Bold = true;
        //                                //range.Borders.Weight = Excel.XlBorderWeight.xlThin;
        //                                range.Merge(true);

        //                                range2.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                range2.Font.Bold = true;
        //                                //range2.Borders.Weight = Excel.XlBorderWeight.xlThin;
        //                                range2.Merge(true);

        //                                range3.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                range3.Font.Bold = true;
        //                                range3.Merge(true);

        //                                range4.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                range4.Font.Bold = true;
        //                                range4.Merge(true);

        //                                range5.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                range5.Font.Bold = true;
        //                                range5.Merge(true);

        //                                range6.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                range6.Font.Bold = true;
        //                                range6.Merge(true);

        //                                range7.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                range7.Font.Bold = true;
        //                                range7.Merge(true);

        //                                range8.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                range8.Font.Bold = true;
        //                                range8.Merge(true);
        //                                //************************************************************/

        //                                range9.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                range9.Font.Bold = true;
        //                                range9.Merge(true);

        //                                range10.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                range10.Font.Bold = true;
        //                                range10.Merge(true);

        //                                range11.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                range11.Font.Bold = true;
        //                                range11.Merge(true);

        //                                #endregion Partitioning the columns

        //                                #region Adding the column headings

        //                                for (int i = 1; i <= reportColumnCount; i++)
        //                                {
        //                                    if (i == 1)
        //                                    {
        //                                        xlWorkSheet[t].Cells[tableFirstRow + 1, i].Interior.ColorIndex = 2;
        //                                    }
        //                                    if (i >= 2 && i <= 5)
        //                                    {
        //                                        xlWorkSheet[t].Cells[tableFirstRow + 1, i].Interior.ColorIndex = 35;
        //                                        xlWorkSheet[t].Cells[tableFirstRow, i].Interior.ColorIndex = 35;
        //                                    }
        //                                    if (i >= 5 && i <= 13)
        //                                    {
        //                                        xlWorkSheet[t].Cells[tableFirstRow + 1, i].Interior.ColorIndex = 34;
        //                                        xlWorkSheet[t].Cells[tableFirstRow, i].Interior.ColorIndex = 34;
        //                                    }
        //                                    if (i >= 14 && i <= 19)
        //                                    {
        //                                        xlWorkSheet[t].Cells[tableFirstRow + 1, i].Interior.ColorIndex = 36;
        //                                        xlWorkSheet[t].Cells[tableFirstRow, i].Interior.ColorIndex = 36;
        //                                    }
        //                                    if (i >= 20 && i <= 25)
        //                                    {
        //                                        xlWorkSheet[t].Cells[tableFirstRow + 1, i].Interior.ColorIndex = 45;
        //                                        xlWorkSheet[t].Cells[tableFirstRow, i].Interior.ColorIndex = 45;
        //                                    }
        //                                    if (i == 26)
        //                                    {
        //                                        xlWorkSheet[t].Cells[tableFirstRow + 1, i].Interior.ColorIndex = 33;
        //                                        xlWorkSheet[t].Cells[tableFirstRow, i].Interior.ColorIndex = 33;
        //                                    }
        //                                    if (i >= 27 && i <= 28)
        //                                    {
        //                                        xlWorkSheet[t].Cells[tableFirstRow + 1, i].Interior.ColorIndex = 39;
        //                                        xlWorkSheet[t].Cells[tableFirstRow, i].Interior.ColorIndex = 39;
        //                                    }
        //                                    if (i >= 30 && i <= 33)
        //                                    {
        //                                        xlWorkSheet[t].Cells[tableFirstRow + 1, i].Interior.ColorIndex = 40;
        //                                        xlWorkSheet[t].Cells[tableFirstRow, i].Interior.ColorIndex = 40;
        //                                    }

        //                                    if (i >= 34 && i <= 36)
        //                                    {
        //                                        xlWorkSheet[t].Cells[tableFirstRow + 1, i].Interior.ColorIndex = 35; //light green
        //                                        xlWorkSheet[t].Cells[tableFirstRow, i].Interior.ColorIndex = 35;
        //                                    }

        //                                    if (i >= 37 && i <= 39)
        //                                    {
        //                                        xlWorkSheet[t].Cells[tableFirstRow + 1, i].Interior.ColorIndex = 34; //cyan
        //                                        xlWorkSheet[t].Cells[tableFirstRow, i].Interior.ColorIndex = 34;
        //                                    }

        //                                    if (i >= 40 && i <= 42)
        //                                    {
        //                                        xlWorkSheet[t].Cells[tableFirstRow + 1, i].Interior.ColorIndex = 15;
        //                                        xlWorkSheet[t].Cells[tableFirstRow, i].Interior.ColorIndex = 15;
        //                                    }

        //                                    if (i >= 43 && i <= 45)
        //                                    {
        //                                        xlWorkSheet[t].Cells[tableFirstRow + 1, i].Interior.ColorIndex = 24; //light purple
        //                                        xlWorkSheet[t].Cells[tableFirstRow, i].Interior.ColorIndex = 24;
        //                                    }

        //                                    if (i >= 46 && i <= 57)
        //                                    {
        //                                        xlWorkSheet[t].Cells[tableFirstRow + 1, i].Interior.ColorIndex = 19;
        //                                        xlWorkSheet[t].Cells[tableFirstRow, i].Interior.ColorIndex = 19;
        //                                    }
        //                                    //*********************************************************/
        //                                    if (i >= 58 && i <= 60)
        //                                    {
        //                                        xlWorkSheet[t].Cells[tableFirstRow + 1, i].Interior.ColorIndex = 35;
        //                                        xlWorkSheet[t].Cells[tableFirstRow, i].Interior.ColorIndex = 35;
        //                                    }

        //                                    if (i >= 61 && i <= 63)
        //                                    {
        //                                        xlWorkSheet[t].Cells[tableFirstRow + 1, i].Interior.ColorIndex = 34;
        //                                        xlWorkSheet[t].Cells[tableFirstRow, i].Interior.ColorIndex = 34;
        //                                    }

        //                                    if (i >= 64 && i <= 66)
        //                                    {
        //                                        xlWorkSheet[t].Cells[tableFirstRow + 1, i].Interior.ColorIndex = 24;
        //                                        xlWorkSheet[t].Cells[tableFirstRow, i].Interior.ColorIndex = 24;
        //                                    }

        //                                    xlWorkSheet[t].Cells[tableFirstRow + 1, i].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        //                                }

        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 1] = "Campaign";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 2] = "PL Disc Number";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 3] = "UDM Batch Number";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 4] = "Import Date";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 5] = "# Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 6] = "1 X Tel No.";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 7] = "%";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 8] = "% STL";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 9] = "2 X Tel No.";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 10] = "%";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 11] = "% STL";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 12] = "3 X Tel No.";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 13] = "%";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 14] = "% STL";

        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 15] = "ID Numbers";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 16] = "%";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 17] = "% STL";

        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 18] = "No ID #";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 19] = "%";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 20] = "% STL";

        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 21] = "ID No's Given by 1x Tel No. Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 22] = "% of ID No's Given by 1 X Tel No. Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 23] = "% STL";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 24] = "ID No's Given by 2x and 3x Tel No. Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 25] = "% of ID No's Given by 2x and 3x Tel No. Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 26] = "% STL";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 27] = "% Email Addresses";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 28] = "Days from commencement of free cover - when client receives the gift - to UDM receiving the leads"; //"Average Age of the Leads: From Date of Delivery of Gift Pack and Start of Free Cover (Days)";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 29] = "Average days since the generation of the lead";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 30] = "AVG Age of Clients";

        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 31] = "Pensioners";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 32] = "% Pensioners";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 33] = "Housewives";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 34] = "% Housewives";

        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 35] = "Number of Re-Primed Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 36] = "% Number of Re-Primed Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 37] = "% STL";

        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 38] = "Number of Indian Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 39] = "% of Indian Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 40] = "% STL";

        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 41] = "Number of African Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 42] = "% of African Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 43] = "% STL";

        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 44] = "Number of Re-Marketed Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 45] = "% of Re-Marketed Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 46] = "% STL";

        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 47] = "1";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 48] = "% of Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 49] = "% STL";

        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 50] = "2";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 51] = "% of Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 52] = "% STL";

        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 53] = "3";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 54] = "% of Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 55] = "% STL";

        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 56] = "4+";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 57] = "% of Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 58] = "% STL";

        //                                //********************************************/
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 59] = "Number of Leads with Contact Details";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 60] = "% of Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 61] = "% STL";

        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 62] = "Number of Leads with Contact Details";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 63] = "% of Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 64] = "% STL";

        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 65] = "Number of Leads with Contact Details";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 66] = "% of Leads";
        //                                xlWorkSheet[t].Cells[tableFirstRow + 1, 67] = "% STL";

        //                                //xlWorkSheet.Columns[1].ColumnWidth = 20;
        //                                //xlWorkSheet.Columns[2].ColumnWidth = 20;
        //                                //xlWorkSheet.Cells[2, 1].Font.Bold = true;
        //                                //xlWorkSheet.Cells[2, 2].Font.Bold = true;

        //                                for (int i = 1; i <= reportColumnCount; i++)//formatting of all columns
        //                                {
        //                                    #region Setting the column widths
        //                                    //if (i <= 2)
        //                                    //{
        //                                    //    xlWorkSheet.Columns[i].ColumnWidth = 10;
        //                                    //}
        //                                    //else if (i == 27 || i == 28)
        //                                    //{
        //                                    //    xlWorkSheet.Columns[i].ColumnWidth = 15;
        //                                    //}
        //                                    //else if (i >= 30 || i <= 30)
        //                                    //{
        //                                    //    xlWorkSheet.Columns[i].ColumnWidth = 14;
        //                                    //}
        //                                    //else
        //                                    //{
        //                                    //    xlWorkSheet.Columns[i].ColumnWidth = 7.43;                                        
        //                                    //}

        //                                    switch (i)
        //                                    {
        //                                        case 1:
        //                                            xlWorkSheet[t].Columns[i].ColumnWidth = 17;
        //                                            break;

        //                                        case 2:
        //                                        case 14:
        //                                        case 34:
        //                                        case 37:
        //                                            xlWorkSheet[t].Columns[i].ColumnWidth = 8.29;
        //                                            break;

        //                                        case 3:
        //                                        case 32:
        //                                        case 33:
        //                                            xlWorkSheet[t].Columns[i].ColumnWidth = 11;
        //                                            break;

        //                                        case 4:
        //                                        case 6:
        //                                        case 7:
        //                                        case 9:
        //                                        case 10:
        //                                        case 12:
        //                                        case 13:
        //                                        case 15:
        //                                        case 16:
        //                                        case 18:
        //                                        case 19:
        //                                        case 22:
        //                                        case 25:
        //                                        case 35:
        //                                        case 36:
        //                                        case 38:
        //                                        case 39:
        //                                            xlWorkSheet[t].Columns[i].ColumnWidth = 7.43;
        //                                            break;

        //                                        case 5:
        //                                        case 8:
        //                                        case 11:
        //                                        case 30:
        //                                        case 31:
        //                                        case 43:
        //                                        case 44:
        //                                        case 45:
        //                                            xlWorkSheet[t].Columns[i].ColumnWidth = 10.14;
        //                                            break;

        //                                        case 17:
        //                                        case 27:
        //                                            xlWorkSheet[t].Columns[i].ColumnWidth = 14;
        //                                            break;

        //                                        case 20:
        //                                        case 21:
        //                                        case 28:
        //                                            xlWorkSheet[t].Columns[i].ColumnWidth = 12.43;
        //                                            break;

        //                                        case 24:
        //                                            xlWorkSheet[t].Columns[i].ColumnWidth = 12.86;
        //                                            break;

        //                                        case 26:
        //                                        case 29:
        //                                            xlWorkSheet[t].Columns[i].ColumnWidth = 12.43;
        //                                            break;
        //                                    }

        //                                    #endregion Setting the column widths

        //                                    xlWorkSheet[t].Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                    xlWorkSheet[t].Columns[i].WrapText = true;
        //                                    xlWorkSheet[t].Cells[tableFirstRow + 1, i].Font.Bold = true;
        //                                    xlWorkSheet[t].Cells[tableFirstRow, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
        //                                    xlWorkSheet[t].Cells[tableFirstRow + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
        //                                }

        //                                #endregion Adding the column headings

        //                                List<string> batchCodes = new List<string>();
        //                                foreach (DataRow rw in dtLeadAllocationData.Rows)
        //                                {

        //                                    if (!batchCodes.Contains(rw["BatchNumber"].ToString()))
        //                                    {
        //                                        batchCodes.Add(rw["BatchNumber"].ToString());
        //                                    }
        //                                }

        //                                #region Totals variable definitions & Initializations



        //                                decimal totalLeads = 0;
        //                                decimal totalTelNo1 = 0;
        //                                decimal totalTelNo2 = 0;
        //                                decimal totalTelNo3 = 0;
        //                                decimal totalNoIDs = 0;
        //                                decimal TotalIDs = 0;
        //                                decimal totalIDnumbersByTel1 = 0;
        //                                decimal totalIDnumbersByTel2And3 = 0;
        //                                decimal totalTelNo1Percent = 0;
        //                                decimal totalTelNo2Percent = 0;
        //                                decimal totalTelNo3Percent = 0;
        //                                decimal totaltelno1xStl = 0;
        //                                decimal totaltelno2xStl = 0;
        //                                decimal totaltelno3xStl = 0;
        //                                decimal totaltelno1xStlPercent = 0;
        //                                decimal totaltelno2xStlPercent = 0;
        //                                decimal totaltelno3xStlPercent = 0;

        //                                decimal totalIDsPercent = 0;
        //                                decimal totalDobPercent = 0;
        //                                decimal totalIdsGivenByTel1Percent = 0;
        //                                decimal totalIdsGivenByTel2And3Percent = 0;
        //                                decimal totalsalesWithIDNumber = 0;
        //                                decimal totaliDNosGivenByTel1StlPercent = 0;
        //                                decimal totaliDNosGivenByTel2and3StlPercent = 0;
        //                                decimal totalsalesWithoutIDNumber = 0;

        //                                decimal totalEmailAddresses = 0;
        //                                decimal totalEmailAddressPercent = 0;

        //                                //double totalAverageLeads = 0;
        //                                //decimal totalAverageClients = 0;

        //                                double totalSalesWithIDNumberPercentage = 0;
        //                                double totalSalesWithoutIDNumberPercentage = 0;
        //                                decimal totalAverageDaysBetweenLeadCreationAndImport = 0;

        //                                double totalPensioners = 0;
        //                                double totalHousewives = 0;

        //                                double totalRePrimedLeads = 0;
        //                                double totalRePrimedLeadsSTL = 0;

        //                                double totalIndianLeads = 0;
        //                                double totalIndianLeadsSTL = 0;

        //                                decimal totalAfricanLeads = 0;
        //                                decimal totalAfricanLeadsSTL = 0;

        //                                decimal totalReMarketedLeads = 0;
        //                                decimal totalReMarketedLeadsSTL = 0;

        //                                decimal totalReMarketedLeads1 = 0;
        //                                decimal totalReMarketedLeadsSTL1 = 0;

        //                                decimal totalReMarketedLeads2 = 0;
        //                                decimal totalReMarketedLeadsSTL2 = 0;

        //                                decimal totalReMarketedLeads3 = 0;
        //                                decimal totalReMarketedLeadsSTL3 = 0;

        //                                decimal totalReMarketedLeads4 = 0;
        //                                decimal totalReMarketedLeadsSTL4 = 0;

        //                                decimal totalNOKContactDetailsLeads = 0;
        //                                decimal totalNOKContactDetailsSTL = 0;

        //                                decimal totalBenContactDetailsLeads = 0;
        //                                decimal totalBenContactDetailsSTL = 0;

        //                                decimal totalLA2ContactDetailsLeads = 0;
        //                                decimal totalLA2ContactDetailsSTL = 0;

        //                                decimal GrandTotalOfClientsAges = 0;
        //                                decimal TotalCountOfClientsWithAges = 0;

        //                                decimal GrandTotalOfDaysBetweenLeadCreationAndImport = 0;
        //                                decimal TotalCountOfDaysBetweenLeadCreationAndImport = 0;

        //                                decimal GrandTotalOfLeadsAges = 0;
        //                                decimal TotalCountOfLeadsWithAges = 0;

        //                                decimal dResult = 0;

        //                                #endregion Totals variable definitions & Initializations

        //                                foreach (string code in batchCodes)
        //                                {
        //                                    #region Variable declarations & initializations

        //                                    string campaignCode = dr.Cells["CampaignCode"].Value.ToString();


        //                                    string BatchNumber = code;
        //                                    string udmCode = string.Empty;
        //                                    string dateReceived = string.Empty;

        //                                    //int noPlatinumContactDate = 0;

        //                                    decimal telNo1x = 0;
        //                                    decimal telno1xStl = 0;
        //                                    decimal telno1xStlPercent = 0;
        //                                    decimal telno2xStl = 0;
        //                                    decimal telno2xStlPercent = 0;
        //                                    decimal telno3xStl = 0;
        //                                    decimal telno3xStlPercent = 0;

        //                                    decimal telNo1xPercent = 0;
        //                                    decimal telNo2x = 0;
        //                                    decimal telNo2xPercent = 0;
        //                                    decimal telNo3x = 0;
        //                                    decimal telNo3xPercent = 0;
        //                                    decimal idnumbers = 0;
        //                                    decimal idnumbersPercent = 0;
        //                                    decimal noIDs = 0;
        //                                    decimal NoIDsPercent = 0;
        //                                    decimal emailAddressCount = 0;
        //                                    int iDNosGivenByTel1 = 0;
        //                                    decimal iDNosGivenByTel1Percent = 0;

        //                                    decimal iDNosGivenByTel1Stl = 0;
        //                                    decimal iDNosGivenByTel1StlPercent = 0;

        //                                    int iDNosGivenByTel2and3 = 0;
        //                                    decimal iDNosGivenByTel2and3Percent = 0;

        //                                    decimal iDNosGivenByTel2and3Stl = 0;
        //                                    decimal iDNosGivenByTel2and3StlPercent = 0;

        //                                    decimal emailAddressesPercent = 0;

        //                                    decimal AverageAgeOfLeads = 0;
        //                                    decimal TotalOfLeadsAges = 0;
        //                                    decimal CountOfLeadsWithAges = 0;
        //                                    decimal noLeadAge = 0;

        //                                    decimal AverageDaysBetweenLeadCreationAndImport = 0;
        //                                    decimal TotalOfDaysBetweenLeadCreationAndImport = 0;
        //                                    decimal CountOfDaysBetweenLeadCreationAndImport = 0;
        //                                    decimal NoDaysBetweenLeadCreationAndImport = 0;

        //                                    decimal AverageAgeOfClients = 0;
        //                                    decimal TotalOfClientsAges = 0;
        //                                    decimal CountOfClientsWithAges = 0;
        //                                    decimal noDob = 0;

        //                                    int leadcount = Convert.ToInt32(dtTotals.Select("BatchNumber = '" + code + "'")[0]["LeadCount"]);
        //                                    //dtLeadAllocationData.AsEnumerable().Select(r => r.Field<long>("ID")).Distinct().ToArray().Count()

        //                                    int salesWithIDNumber = 0;
        //                                    decimal salesWithIDNumberPercentage = 0;

        //                                    int salesWithoutIDNumber = 0;
        //                                    decimal salesWithoutIDNumberPercentage = 0;

        //                                    decimal pensioners = 0;
        //                                    decimal pensionersPercentage = 0;
        //                                    decimal housewives = 0;
        //                                    decimal housewivesPercentage = 0;

        //                                    decimal reprimedLeads = 0;
        //                                    decimal reprimedLeadsPercentage = 0;
        //                                    decimal reprimedLeadsSTL = 0;
        //                                    decimal reprimedLeadsSTLPercentage = 0;

        //                                    decimal indianLeads = 0;
        //                                    decimal indianLeadsPercentage = 0;
        //                                    decimal indianLeadsSTL = 0;
        //                                    decimal indianLeadsSTLPercentage = 0;

        //                                    decimal africanLeads = 0;
        //                                    decimal africanLeadsPercentage = 0;
        //                                    decimal africanLeadsSTL = 0;
        //                                    decimal africanLeadsSTLPercentage = 0;

        //                                    decimal reMarketedLeads = 0;
        //                                    decimal reMarketedLeadsPercentage = 0;
        //                                    decimal reMarketedLeadsSTL = 0;
        //                                    decimal reMarketedLeadsSTLPercentage = 0;

        //                                    decimal reMarketedLeads1 = 0;
        //                                    decimal reMarketedLeadsPercentage1 = 0;
        //                                    decimal reMarketedLeadsSTL1 = 0;
        //                                    decimal reMarketedLeadsSTLPercentage1 = 0;

        //                                    decimal reMarketedLeads2 = 0;
        //                                    decimal reMarketedLeadsPercentage2 = 0;
        //                                    decimal reMarketedLeadsSTL2 = 0;
        //                                    decimal reMarketedLeadsSTLPercentage2 = 0;

        //                                    decimal reMarketedLeads3 = 0;
        //                                    decimal reMarketedLeadsPercentage3 = 0;
        //                                    decimal reMarketedLeadsSTL3 = 0;
        //                                    decimal reMarketedLeadsSTLPercentage3 = 0;

        //                                    decimal reMarketedLeads4 = 0;
        //                                    decimal reMarketedLeadsPercentage4 = 0;
        //                                    decimal reMarketedLeadsSTL4 = 0;
        //                                    decimal reMarketedLeadsSTLPercentage4 = 0;

        //                                    decimal NOKContactDetailsLeads = 0;
        //                                    decimal NOKContactDetailsLeadsPercentage = 0;
        //                                    decimal NOKContactDetailsSTL = 0;
        //                                    decimal NOKContactDetailsSTLPercentage = 0;

        //                                    decimal BenContactDetailsLeads = 0;
        //                                    decimal BenContactDetailsLeadsPercentage = 0;
        //                                    decimal BenContactDetailsSTL = 0;
        //                                    decimal BenContactDetailsSTLPercentage = 0;

        //                                    decimal LA2ContactDetailsLeads = 0;
        //                                    decimal LA2ContactDetailsLeadsPercentage = 0;
        //                                    decimal LA2ContactDetailsSTL = 0;
        //                                    decimal LA2ContactDetailsSTLPercentage = 0;

        //                                    #endregion Variable declarations & initializations

        //                                    #region Loop through each row in the data table to get the totals

        //                                    foreach (DataRow row in dtLeadAllocationData.Rows)
        //                                    {
        //                                        int telCount = 0;
        //                                        double age = 0;
        //                                        double age2 = 0;
        //                                        if (row["BatchNumber"].ToString() == code)
        //                                        {
        //                                            udmCode = row["UDMCode"].ToString();
        //                                            string importDate = row["ImportDate"].ToString();
        //                                            if (importDate != string.Empty)
        //                                            {
        //                                                dateReceived = importDate.Substring(0, 10);
        //                                            }

        //                                            //leadcount++;

        //                                            #region telephone numbers
        //                                            string telwork = row["TelWork"].ToString();
        //                                            string telHome = row["TelHome"].ToString();
        //                                            string telCell = row["TelCell"].ToString();
        //                                            string leadStatusID = row["FKINLeadStatusID"].ToString();
        //                                            if (telwork != string.Empty)
        //                                            {
        //                                                telCount++;
        //                                            }
        //                                            if (telHome != string.Empty)
        //                                            {
        //                                                telCount++;
        //                                            }
        //                                            if (telCell != string.Empty)
        //                                            {
        //                                                telCount++;
        //                                            }

        //                                            if (telCount == 1)
        //                                            {
        //                                                telNo1x++;
        //                                                //determine if sale
        //                                                if (leadStatusID == "1")//sale
        //                                                {
        //                                                    telno1xStl++;
        //                                                }
        //                                            }
        //                                            if (telCount == 2)
        //                                            {
        //                                                telNo2x++;
        //                                                //determine if sale
        //                                                if (leadStatusID == "1")//sale
        //                                                {
        //                                                    telno2xStl++;
        //                                                }
        //                                            }
        //                                            if (telCount > 2)
        //                                            {
        //                                                telNo3x++;
        //                                                //determine if sale
        //                                                if (leadStatusID == "1")//sale
        //                                                {
        //                                                    telno3xStl++;
        //                                                }
        //                                            }
        //                                            #endregion telephone numbers

        //                                            #region id numbers and DateOfBirth

        //                                            string idnumber = row["IDNo"].ToString();
        //                                            string dateOfBirth = row["DateOfBirth"].ToString();
        //                                            if (idnumber != string.Empty)
        //                                            {
        //                                                idnumbers++;
        //                                                if (telCount == 1)
        //                                                {
        //                                                    iDNosGivenByTel1++;
        //                                                    //determine if sale
        //                                                    if (leadStatusID == "1")//sale
        //                                                    {
        //                                                        iDNosGivenByTel1Stl++;
        //                                                        salesWithIDNumber++;
        //                                                    }
        //                                                }
        //                                                if (telCount >= 2)
        //                                                {
        //                                                    iDNosGivenByTel2and3++;
        //                                                    //determine if sale
        //                                                    if (leadStatusID == "1")//sale
        //                                                    {
        //                                                        iDNosGivenByTel2and3Stl++;
        //                                                        salesWithIDNumber++;
        //                                                    }
        //                                                }
        //                                            }
        //                                            else
        //                                            {
        //                                                noIDs++;
        //                                            }

        //                                            if (dateOfBirth != string.Empty)
        //                                            {

        //                                                //workout age                                                
        //                                                int year = DateTime.Parse(dateOfBirth).Year;
        //                                                int month = DateTime.Parse(dateOfBirth).Month;
        //                                                int day = DateTime.Parse(dateOfBirth).Day;
        //                                                DateTime dob = new DateTime(year, month, day);
        //                                                DateTime now = DateTime.Now;
        //                                                //now = now.AddYears(-year);
        //                                                //now = now.AddMonths(-month);
        //                                                //now.AddDays(-day);    
        //                                                //  now = //now.Subtract(dob.Date

        //                                                TimeSpan span = now.Subtract(dob);
        //                                                //age = //(span.TotalDays/ 365.25).ToString()
        //                                                age = span.TotalDays / 365.25;


        //                                                TotalOfClientsAges += (decimal)age;
        //                                                CountOfClientsWithAges++;
        //                                            }
        //                                            else
        //                                            {
        //                                                noDob++;
        //                                            }

        //                                            #endregion id numbers and DateOfBirth

        //                                            #region email addresses
        //                                            string emailAddress = row["Email"].ToString();
        //                                            if (emailAddress != string.Empty)
        //                                            {
        //                                                emailAddressCount++;
        //                                            }
        //                                            #endregion email addresses

        //                                            #region Lead Age

        //                                            //string platinumContactDate = row["PlatinumContactDate"].ToString();
        //                                            //if (platinumContactDate != string.Empty)
        //                                            //{
        //                                            //    int year = DateTime.Parse(platinumContactDate).Year;
        //                                            //    int month = DateTime.Parse(platinumContactDate).Month;
        //                                            //    int day = DateTime.Parse(platinumContactDate).Day;
        //                                            //    DateTime platDate = new DateTime(year, month, day);
        //                                            //    DateTime impDate = DateTime.Parse(importDate);
        //                                            //    TimeSpan span2 = impDate.Subtract(platDate);
        //                                            //    age2 = span2.TotalDays;// / 365.25;
        //                                            //    totalLeadAges = totalLeadAges + age2;
        //                                            //}
        //                                            //else
        //                                            //{
        //                                            //    noPlatinumContactDate++;
        //                                            //}

        //                                            if (row["LeadAge"] != DBNull.Value)
        //                                            {
        //                                                int leadAge = Convert.ToInt32(row["LeadAge"]);
        //                                                TotalOfLeadsAges += leadAge;
        //                                                CountOfLeadsWithAges++;
        //                                            }
        //                                            else
        //                                            {
        //                                                noLeadAge++;
        //                                            }

        //                                            #endregion Lead Age

        //                                            #region ID Number STLs

        //                                            if (leadStatusID == "1")
        //                                            {
        //                                                if (idnumber == string.Empty)
        //                                                {
        //                                                    salesWithoutIDNumber++;
        //                                                }
        //                                            }

        //                                            #endregion ID Number STLs

        //                                            #region Days Between Lead Creation And Import

        //                                            if (row["DaysBetweenLeadCreationAndImport"] != DBNull.Value)
        //                                            {
        //                                                int daysBetweenLeadCreationAndImport = Convert.ToInt32(row["DaysBetweenLeadCreationAndImport"]);
        //                                                TotalOfDaysBetweenLeadCreationAndImport += daysBetweenLeadCreationAndImport;
        //                                                CountOfDaysBetweenLeadCreationAndImport++;
        //                                            }
        //                                            else
        //                                            {
        //                                                NoDaysBetweenLeadCreationAndImport++;
        //                                            }

        //                                            #endregion Days Between Lead Creation And Import

        //                                            #region Pensioners & Housewives

        //                                            if (row["Occupation"].ToString().Trim() == "Pensioner")
        //                                            {
        //                                                ++pensioners;
        //                                            }
        //                                            else if (row["Occupation"].ToString().Trim() == "Housewife")
        //                                            {
        //                                                ++housewives;
        //                                            }

        //                                            #endregion Pensioners & Housewives

        //                                            #region Re-Primed Leads

        //                                            //if (!string.IsNullOrEmpty(row["ReferralFrom"].ToString().Trim()))
        //                                            if (Convert.ToBoolean(row["IsRePrimed"])) // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/215482028/comments
        //                                            {
        //                                                ++reprimedLeads;

        //                                                if (leadStatusID == "1")
        //                                                {
        //                                                    ++reprimedLeadsSTL;
        //                                                }
        //                                            }

        //                                            #endregion Re-Primed Leads

        //                                            #region Indian Leads
        //                                            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/214952271/comments#355827524

        //                                            bool isIndian = Convert.ToBoolean(row["IsIndian"]);

        //                                            if (isIndian)
        //                                            {
        //                                                indianLeads++;

        //                                                if (leadStatusID == "1")
        //                                                {
        //                                                    indianLeadsSTL++;
        //                                                }
        //                                            }

        //                                            #endregion Indian Leads

        //                                            #region African Leads

        //                                            bool isAfrican = Convert.ToBoolean(row["IsAfrican"]);

        //                                            if (isAfrican)
        //                                            {
        //                                                africanLeads++;

        //                                                if (leadStatusID == "1")
        //                                                {
        //                                                    africanLeadsSTL++;
        //                                                }
        //                                            }

        //                                            #endregion African Leads

        //                                            #region ReMarketed Leads

        //                                            bool isReMarketed = Convert.ToBoolean(row["IsReMarketed"]);
        //                                            if (isReMarketed)
        //                                            {
        //                                                reMarketedLeads++;

        //                                                if (leadStatusID == "1")
        //                                                {
        //                                                    reMarketedLeadsSTL++;
        //                                                }
        //                                            }

        //                                            bool isReMarketed1 = Convert.ToBoolean(row["IsReMarketed1"]);
        //                                            if (isReMarketed1)
        //                                            {
        //                                                reMarketedLeads1++;

        //                                                if (leadStatusID == "1")
        //                                                {
        //                                                    reMarketedLeadsSTL1++;
        //                                                }
        //                                            }

        //                                            bool isReMarketed2 = Convert.ToBoolean(row["IsReMarketed2"]);
        //                                            if (isReMarketed2)
        //                                            {
        //                                                reMarketedLeads2++;

        //                                                if (leadStatusID == "1")
        //                                                {
        //                                                    reMarketedLeadsSTL2++;
        //                                                }
        //                                            }

        //                                            bool isReMarketed3 = Convert.ToBoolean(row["IsReMarketed3"]);
        //                                            if (isReMarketed3)
        //                                            {
        //                                                reMarketedLeads3++;

        //                                                if (leadStatusID == "1")
        //                                                {
        //                                                    reMarketedLeadsSTL3++;
        //                                                }
        //                                            }

        //                                            bool isReMarketed4 = Convert.ToBoolean(row["IsReMarketed4"]);
        //                                            if (isReMarketed4)
        //                                            {
        //                                                reMarketedLeads4++;

        //                                                if (leadStatusID == "1")
        //                                                {
        //                                                    reMarketedLeadsSTL4++;
        //                                                }
        //                                            }

        //                                            #endregion ReMarketed Leads

        //                                            #region NOK Contact Detail Leads

        //                                            bool hasNokContactDetails = Convert.ToBoolean(row["NOKHasContactDetails"]);
        //                                            if (hasNokContactDetails)
        //                                            {
        //                                                NOKContactDetailsLeads++;

        //                                                if (leadStatusID == "1")
        //                                                {
        //                                                    NOKContactDetailsSTL++;
        //                                                }
        //                                            }

        //                                            #endregion NOK Contact Detail Leads

        //                                            #region Ben Contact Detail Leads

        //                                            bool hasBenContactDetails = Convert.ToBoolean(row["BenHasContactDetails"]);
        //                                            if (hasBenContactDetails)
        //                                            {
        //                                                BenContactDetailsLeads++;

        //                                                if (leadStatusID == "1")
        //                                                {
        //                                                    BenContactDetailsSTL++;
        //                                                }
        //                                            }

        //                                            #endregion Ben Contact Detail Leads

        //                                            #region LA2 Contact Detail Leads

        //                                            bool hasLA2ContactDetails = Convert.ToBoolean(row["LAHasContactDetails"]);
        //                                            if (hasLA2ContactDetails)
        //                                            {
        //                                                LA2ContactDetailsLeads++;

        //                                                if (leadStatusID == "1")
        //                                                {
        //                                                    LA2ContactDetailsSTL++;
        //                                                }
        //                                            }

        //                                            #endregion LA2 Contact Detail Leads


        //                                        }
        //                                    }

        //                                    #endregion Loop through each row in the data table to get the totals

        //                                    if (CountOfClientsWithAges > 0)
        //                                    {
        //                                        AverageAgeOfClients = Math.Round(TotalOfClientsAges / CountOfClientsWithAges, 2);
        //                                    }

        //                                    //if ((leadcount - noPlatinumContactDate) > 0)
        //                                    //{
        //                                    //    averageAgeOfLeads = Math.Round(totalLeadAges / leadcount - noPlatinumContactDate, 0);
        //                                    //}

        //                                    if (CountOfLeadsWithAges > 0)
        //                                    {
        //                                        AverageAgeOfLeads = Math.Round(TotalOfLeadsAges / CountOfLeadsWithAges, 0);
        //                                        AverageDaysBetweenLeadCreationAndImport = (decimal)Math.Round(TotalOfDaysBetweenLeadCreationAndImport / (leadcount - NoDaysBetweenLeadCreationAndImport), 0);
        //                                    }

        //                                    if (CountOfClientsWithAges > 0)
        //                                    {
        //                                        AverageAgeOfClients = Math.Round(TotalOfClientsAges / CountOfClientsWithAges, 2);
        //                                    }

        //                                    decimal totalcontactNumber = telNo1x + telNo2x + telNo3x;
        //                                    decimal totalIDNumbers = iDNosGivenByTel1 + iDNosGivenByTel2and3;
        //                                    if (totalcontactNumber > 0)
        //                                    {
        //                                        telNo1xPercent = Math.Round(telNo1x / totalcontactNumber * 100, 2);
        //                                        telNo2xPercent = Math.Round(telNo2x / totalcontactNumber * 100, 2);
        //                                        telNo3xPercent = Math.Round(telNo3x / totalcontactNumber * 100, 2);
        //                                    }
        //                                    if (telNo1x > 0)
        //                                    {
        //                                        telno1xStlPercent = Math.Round(telno1xStl / telNo1x * 100, 2);
        //                                    }
        //                                    if (telNo2x > 0)
        //                                    {
        //                                        telno2xStlPercent = Math.Round(telno2xStl / telNo2x * 100, 2);
        //                                    }
        //                                    if (telNo3x > 0)
        //                                    {
        //                                        telno3xStlPercent = Math.Round(telno3xStl / telNo3x * 100, 2);
        //                                    }
        //                                    if (leadcount > 0)
        //                                    {
        //                                        idnumbersPercent = Math.Round(idnumbers / leadcount * 100, 2);
        //                                        NoIDsPercent = Math.Round(noIDs / leadcount * 100, 2);
        //                                        emailAddressesPercent = Math.Round(emailAddressCount / leadcount * 100, 2);
        //                                    }
        //                                    if (totalIDNumbers > 0)
        //                                    {
        //                                        iDNosGivenByTel1Percent = Math.Round(iDNosGivenByTel1 / totalIDNumbers * 100, 2);
        //                                        iDNosGivenByTel2and3Percent = Math.Round(iDNosGivenByTel2and3 / totalIDNumbers * 100, 2);
        //                                        salesWithIDNumberPercentage = Math.Round(salesWithIDNumber / totalIDNumbers * 100, 2);
        //                                        salesWithoutIDNumberPercentage = Math.Round(salesWithoutIDNumber / totalIDNumbers * 100, 2);
        //                                    }
        //                                    if (iDNosGivenByTel1 > 0)
        //                                    {
        //                                        iDNosGivenByTel1StlPercent = Math.Round(iDNosGivenByTel1Stl / iDNosGivenByTel1 * 100, 2);
        //                                    }
        //                                    if (iDNosGivenByTel2and3 > 0)
        //                                    {
        //                                        iDNosGivenByTel2and3StlPercent = Math.Round(iDNosGivenByTel2and3Stl / iDNosGivenByTel2and3 * 100, 2);
        //                                    }

        //                                    #region Pensioner & Housewife totals & percentages

        //                                    if (leadcount > 0)
        //                                    {
        //                                        pensionersPercentage = Math.Round(pensioners / leadcount * 100, 2);
        //                                        housewivesPercentage = Math.Round(housewives / leadcount * 100, 2);
        //                                    }

        //                                    #endregion Pensioner & Housewife totals & percentages

        //                                    #region Re-Primed Leads Totals & Percentages

        //                                    if (leadcount > 0)
        //                                    {
        //                                        reprimedLeadsPercentage = Math.Round(reprimedLeads / leadcount * 100, 2);

        //                                        if (reprimedLeads > 0)
        //                                        {
        //                                            reprimedLeadsSTLPercentage = Math.Round(reprimedLeadsSTL / reprimedLeads * 100, 2);
        //                                        }
        //                                        else
        //                                        {
        //                                            reprimedLeadsSTLPercentage = 0.00m;
        //                                        }
        //                                    }

        //                                    #endregion Re-Primed Leads Totals & Percentages

        //                                    #region Indian Leads Percentages

        //                                    // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/214952271/comments#355827524

        //                                    if (leadcount > 0)
        //                                    {
        //                                        indianLeadsPercentage = Math.Round(indianLeads / leadcount * 100, 2);

        //                                        if (indianLeads > 0)
        //                                        {
        //                                            indianLeadsSTLPercentage = Math.Round(indianLeadsSTL / indianLeads * 100, 2);
        //                                        }
        //                                        else
        //                                        {
        //                                            indianLeadsSTLPercentage = 0.00m;
        //                                        }
        //                                    }

        //                                    #endregion Indian Leads Percentages

        //                                    #region African Leads Percentages

        //                                    if (leadcount > 0)
        //                                    {
        //                                        africanLeadsPercentage = leadcount > 0 ? africanLeads / leadcount : 0;
        //                                        africanLeadsSTLPercentage = africanLeads > 0 ? africanLeadsSTL / africanLeads : 0;
        //                                    }

        //                                    #endregion African Leads Percentages

        //                                    #region ReMarketed Leads Percentages

        //                                    reMarketedLeadsPercentage = leadcount > 0 ? reMarketedLeads / leadcount : 0;
        //                                    reMarketedLeadsSTLPercentage = reMarketedLeads > 0 ? reMarketedLeadsSTL / reMarketedLeads : 0;

        //                                    reMarketedLeadsPercentage1 = reMarketedLeads > 0 ? reMarketedLeads1 / reMarketedLeads : 0;
        //                                    reMarketedLeadsSTLPercentage1 = reMarketedLeads1 > 0 ? reMarketedLeadsSTL1 / reMarketedLeads1 : 0;

        //                                    reMarketedLeadsPercentage2 = reMarketedLeads > 0 ? reMarketedLeads2 / reMarketedLeads : 0;
        //                                    reMarketedLeadsSTLPercentage2 = reMarketedLeads2 > 0 ? reMarketedLeadsSTL2 / reMarketedLeads2 : 0;

        //                                    reMarketedLeadsPercentage3 = reMarketedLeads > 0 ? reMarketedLeads3 / reMarketedLeads : 0;
        //                                    reMarketedLeadsSTLPercentage3 = reMarketedLeads3 > 0 ? reMarketedLeadsSTL3 / reMarketedLeads3 : 0;

        //                                    reMarketedLeadsPercentage4 = reMarketedLeads > 0 ? reMarketedLeads4 / reMarketedLeads : 0;
        //                                    reMarketedLeadsSTLPercentage4 = reMarketedLeads4 > 0 ? reMarketedLeadsSTL4 / reMarketedLeads4 : 0;

        //                                    #endregion ReMarketed Leads Percentages

        //                                    #region NOK Lead Percentages

        //                                    NOKContactDetailsLeadsPercentage = leadcount > 0 ? NOKContactDetailsLeads / leadcount : 0;
        //                                    NOKContactDetailsSTLPercentage = NOKContactDetailsLeads > 0 ? NOKContactDetailsSTL / NOKContactDetailsLeads : 0;

        //                                    #endregion NOK Lead Percentages

        //                                    #region Ben Lead Percentages

        //                                    BenContactDetailsLeadsPercentage = leadcount > 0 ? BenContactDetailsLeads / leadcount : 0;
        //                                    BenContactDetailsSTLPercentage = BenContactDetailsLeads > 0 ? BenContactDetailsSTL / BenContactDetailsLeads : 0;

        //                                    #endregion Ben Lead Percentages

        //                                    #region LA2 Lead Percentages

        //                                    LA2ContactDetailsLeadsPercentage = leadcount > 0 ? LA2ContactDetailsLeads / leadcount : 0;
        //                                    LA2ContactDetailsSTLPercentage = LA2ContactDetailsLeads > 0 ? LA2ContactDetailsSTL / LA2ContactDetailsLeads : 0;

        //                                    #endregion LA2 Lead Percentages


        //                                    // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/210685249/comments

        //                                    #region Formatting the body of the report

        //                                    for (int i = 1; i <= reportColumnCount; i++)
        //                                    {
        //                                        if (i == 1)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, i].Interior.ColorIndex = 2;
        //                                        }
        //                                        if (i >= 2 && i <= 5)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, i].Interior.ColorIndex = 35;
        //                                        }
        //                                        if (i >= 5 && i <= 13)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, i].Interior.ColorIndex = 34;
        //                                        }

        //                                        if (i >= 14 && i <= 19)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, i].Interior.ColorIndex = 36;
        //                                        }

        //                                        if (i >= 20 && i <= 25)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, i].Interior.ColorIndex = 45;
        //                                        }

        //                                        if (i == 26)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, i].Interior.ColorIndex = 33;
        //                                        }

        //                                        if (i >= 27 && i <= 28)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, i].Interior.ColorIndex = 39;
        //                                        }

        //                                        if (i >= 30 && i <= 33)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, i].Interior.ColorIndex = 40;
        //                                        }

        //                                        if (i >= 34 && i <= 36)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, i].Interior.ColorIndex = 35;
        //                                            //xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 35;
        //                                        }

        //                                        if (i >= 37 && i <= 39)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, i].Interior.ColorIndex = 34;
        //                                            //xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 34;
        //                                        }

        //                                        if (i >= 40 && i <= 42)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, i].Interior.ColorIndex = 15;
        //                                        }

        //                                        if (i >= 43 && i <= 45)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, i].Interior.ColorIndex = 24;
        //                                        }

        //                                        if (i >= 46 && i <= 57)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, i].Interior.ColorIndex = 19;
        //                                        }
        //                                        //*****************************************************/
        //                                        if (i >= 58 && i <= 60)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, i].Interior.ColorIndex = 35;
        //                                        }

        //                                        if (i >= 61 && i <= 63)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, i].Interior.ColorIndex = 34;
        //                                        }

        //                                        if (i >= 64 && i <= 66)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, i].Interior.ColorIndex = 24;
        //                                        }

        //                                        xlWorkSheet[t].Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlThin;
        //                                    }

        //                                    #endregion Formatting the body of the report



        //                                    xlWorkSheet[t].Cells[rowIndex, 1] = campaignCode;
        //                                    xlWorkSheet[t].Cells[rowIndex, 2] = BatchNumber;
        //                                    xlWorkSheet[t].Cells[rowIndex, 3] = udmCode;
        //                                    xlWorkSheet[t].Cells[rowIndex, 4] = dateReceived;
        //                                    xlWorkSheet[t].Cells[rowIndex, 5] = leadcount;
        //                                    xlWorkSheet[t].Cells[rowIndex, 6] = telNo1x;
        //                                    xlWorkSheet[t].Cells[rowIndex, 7] = telNo1xPercent + " %";
        //                                    xlWorkSheet[t].Cells[rowIndex, 8] = telno1xStlPercent + " %";
        //                                    xlWorkSheet[t].Cells[rowIndex, 9] = telNo2x;
        //                                    xlWorkSheet[t].Cells[rowIndex, 10] = telNo2xPercent + " %";
        //                                    xlWorkSheet[t].Cells[rowIndex, 11] = telno2xStlPercent + " %";
        //                                    xlWorkSheet[t].Cells[rowIndex, 12] = telNo3x;
        //                                    xlWorkSheet[t].Cells[rowIndex, 13] = telNo3xPercent + " %";
        //                                    xlWorkSheet[t].Cells[rowIndex, 14] = telno3xStlPercent + " %";

        //                                    xlWorkSheet[t].Cells[rowIndex, 15] = idnumbers;
        //                                    xlWorkSheet[t].Cells[rowIndex, 16] = idnumbersPercent + " %";
        //                                    xlWorkSheet[t].Cells[rowIndex, 17] = salesWithIDNumberPercentage + " %";

        //                                    xlWorkSheet[t].Cells[rowIndex, 18] = noIDs;
        //                                    xlWorkSheet[t].Cells[rowIndex, 19] = NoIDsPercent + " %";
        //                                    xlWorkSheet[t].Cells[rowIndex, 20] = salesWithoutIDNumberPercentage + " %";

        //                                    xlWorkSheet[t].Cells[rowIndex, 21] = iDNosGivenByTel1;
        //                                    xlWorkSheet[t].Cells[rowIndex, 22] = iDNosGivenByTel1Percent + " %";
        //                                    xlWorkSheet[t].Cells[rowIndex, 23] = iDNosGivenByTel1StlPercent + " %";
        //                                    xlWorkSheet[t].Cells[rowIndex, 24] = iDNosGivenByTel2and3;
        //                                    xlWorkSheet[t].Cells[rowIndex, 25] = iDNosGivenByTel2and3Percent + " %";
        //                                    xlWorkSheet[t].Cells[rowIndex, 26] = iDNosGivenByTel2and3StlPercent + " %";
        //                                    xlWorkSheet[t].Cells[rowIndex, 27] = emailAddressesPercent + " %";

        //                                    xlWorkSheet[t].Cells[rowIndex, 28] = AverageAgeOfLeads;
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 28];
        //                                    xlRange.NumberFormat = "0";

        //                                    xlWorkSheet[t].Cells[rowIndex, 29] = AverageDaysBetweenLeadCreationAndImport;
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 29];
        //                                    xlRange.NumberFormat = "0";

        //                                    xlWorkSheet[t].Cells[rowIndex, 30] = AverageAgeOfClients;
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 30];
        //                                    xlRange.NumberFormat = "0.00";

        //                                    xlWorkSheet[t].Cells[rowIndex, 31] = pensioners;
        //                                    xlWorkSheet[t].Cells[rowIndex, 32] = pensionersPercentage + " %";
        //                                    xlWorkSheet[t].Cells[rowIndex, 33] = housewives;
        //                                    xlWorkSheet[t].Cells[rowIndex, 34] = housewivesPercentage + " %";

        //                                    xlWorkSheet[t].Cells[rowIndex, 35] = reprimedLeads;
        //                                    xlWorkSheet[t].Cells[rowIndex, 36] = reprimedLeadsPercentage + " %";
        //                                    xlWorkSheet[t].Cells[rowIndex, 37] = reprimedLeadsSTLPercentage + " %";

        //                                    xlWorkSheet[t].Cells[rowIndex, 38] = indianLeads;
        //                                    xlWorkSheet[t].Cells[rowIndex, 39] = indianLeadsPercentage + " %";
        //                                    xlWorkSheet[t].Cells[rowIndex, 40] = indianLeadsSTLPercentage + " %";

        //                                    xlWorkSheet[t].Cells[rowIndex, 41] = africanLeads;
        //                                    xlWorkSheet[t].Cells[rowIndex, 42] = Math.Round(africanLeadsPercentage, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 42];
        //                                    xlRange.NumberFormat = africanLeadsPercentage == 1m ? "0%" : "0.00%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 43] = Math.Round(africanLeadsSTLPercentage, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 43];
        //                                    xlRange.NumberFormat = africanLeadsSTLPercentage == 1m ? "0%" : "0.00%";

        //                                    xlWorkSheet[t].Cells[rowIndex, 44] = reMarketedLeads;
        //                                    xlWorkSheet[t].Cells[rowIndex, 45] = Math.Round(reMarketedLeadsPercentage, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 45];
        //                                    xlRange.NumberFormat = reMarketedLeadsPercentage == 1m ? "0%" : "0.00%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 46] = Math.Round(reMarketedLeadsSTLPercentage, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 46];
        //                                    xlRange.NumberFormat = reMarketedLeadsSTLPercentage == 1m ? "0%" : "0.00%";

        //                                    xlWorkSheet[t].Cells[rowIndex, 47] = reMarketedLeads1;
        //                                    xlWorkSheet[t].Cells[rowIndex, 48] = Math.Round(reMarketedLeadsPercentage1, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 48];
        //                                    xlRange.NumberFormat = reMarketedLeadsPercentage1 == 1m ? "0%" : "0.00%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 49] = Math.Round(reMarketedLeadsSTLPercentage1, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 49];
        //                                    xlRange.NumberFormat = reMarketedLeadsSTLPercentage1 == 1m ? "0%" : "0.00%";

        //                                    xlWorkSheet[t].Cells[rowIndex, 50] = reMarketedLeads2;
        //                                    xlWorkSheet[t].Cells[rowIndex, 51] = Math.Round(reMarketedLeadsPercentage2, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 51];
        //                                    xlRange.NumberFormat = reMarketedLeadsPercentage2 == 1m ? "0%" : "0.00%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 52] = Math.Round(reMarketedLeadsSTLPercentage2, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 52];
        //                                    xlRange.NumberFormat = reMarketedLeadsSTLPercentage2 == 1m ? "0%" : "0.00%";

        //                                    xlWorkSheet[t].Cells[rowIndex, 53] = reMarketedLeads3;
        //                                    xlWorkSheet[t].Cells[rowIndex, 54] = Math.Round(reMarketedLeadsPercentage3, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 54];
        //                                    xlRange.NumberFormat = reMarketedLeadsPercentage3 == 1m ? "0%" : "0.00%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 55] = Math.Round(reMarketedLeadsSTLPercentage3, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 55];
        //                                    xlRange.NumberFormat = reMarketedLeadsSTLPercentage3 == 1m ? "0%" : "0.00%";

        //                                    xlWorkSheet[t].Cells[rowIndex, 56] = reMarketedLeads4;
        //                                    xlWorkSheet[t].Cells[rowIndex, 57] = Math.Round(reMarketedLeadsPercentage4, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 57];
        //                                    xlRange.NumberFormat = reMarketedLeadsPercentage4 == 1m ? "0%" : "0.00%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 58] = Math.Round(reMarketedLeadsSTLPercentage4, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 58];
        //                                    xlRange.NumberFormat = reMarketedLeadsSTLPercentage4 == 1m ? "0%" : "0.00%";

        //                                    xlWorkSheet[t].Cells[rowIndex, 59] = NOKContactDetailsLeads;
        //                                    xlWorkSheet[t].Cells[rowIndex, 60] = Math.Round(NOKContactDetailsLeadsPercentage, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 60];
        //                                    xlRange.NumberFormat = NOKContactDetailsLeadsPercentage == 1m ? "0%" : "0.00%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 61] = Math.Round(NOKContactDetailsSTLPercentage, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 61];
        //                                    xlRange.NumberFormat = NOKContactDetailsSTLPercentage == 1m ? "0%" : "0.00%";

        //                                    xlWorkSheet[t].Cells[rowIndex, 62] = BenContactDetailsLeads;
        //                                    xlWorkSheet[t].Cells[rowIndex, 63] = Math.Round(BenContactDetailsLeadsPercentage, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 63];
        //                                    xlRange.NumberFormat = BenContactDetailsLeadsPercentage == 1m ? "0%" : "0.00%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 64] = Math.Round(BenContactDetailsSTLPercentage, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 64];
        //                                    xlRange.NumberFormat = BenContactDetailsSTLPercentage == 1m ? "0%" : "0.00%";

        //                                    xlWorkSheet[t].Cells[rowIndex, 65] = LA2ContactDetailsLeads;
        //                                    xlWorkSheet[t].Cells[rowIndex, 66] = Math.Round(LA2ContactDetailsLeadsPercentage, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 66];
        //                                    xlRange.NumberFormat = LA2ContactDetailsLeadsPercentage == 1m ? "0%" : "0.00%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 67] = Math.Round(LA2ContactDetailsSTLPercentage, 4);
        //                                    xlRange = xlWorkSheet[t].Cells[rowIndex, 67];
        //                                    xlRange.NumberFormat = LA2ContactDetailsSTLPercentage == 1m ? "0%" : "0.00%";



        //                                    rowIndex++;

        //                                    #region Updating the totals

        //                                    totalLeads = totalLeads + leadcount;
        //                                    totalTelNo1 = totalTelNo1 + telNo1x;
        //                                    totalTelNo2 = totalTelNo2 + telNo2x;
        //                                    totalTelNo3 = totalTelNo3 + telNo3x;
        //                                    TotalIDs = TotalIDs + idnumbers;
        //                                    totalNoIDs = totalNoIDs + noIDs;
        //                                    totalIDnumbersByTel1 = totalIDnumbersByTel1 + iDNosGivenByTel1;
        //                                    totalIDnumbersByTel2And3 = totalIDnumbersByTel2And3 + iDNosGivenByTel2and3;
        //                                    //totalTelNo1Percent = totalTelNo1Percent + telNo1xPercent;
        //                                    //totalTelNo2Percent = totalTelNo2Percent + telNo2xPercent;
        //                                    //totalTelNo3Percent = totalTelNo3Percent + telNo3xPercent;
        //                                    totaltelno1xStl += telno1xStl;
        //                                    totaltelno2xStl += telno2xStl;
        //                                    totaltelno3xStl += telno3xStl;
        //                                    totaltelno1xStlPercent = totaltelno1xStlPercent + telno1xStlPercent;
        //                                    totaltelno2xStlPercent = totaltelno2xStlPercent + telno2xStlPercent;
        //                                    totaltelno3xStlPercent = totaltelno3xStlPercent + telno3xStlPercent;
        //                                    totalIDsPercent = totalIDsPercent + idnumbersPercent;
        //                                    totalDobPercent = totalDobPercent + NoIDsPercent;
        //                                    totalIdsGivenByTel1Percent = totalIdsGivenByTel1Percent + iDNosGivenByTel1Percent;
        //                                    totalIdsGivenByTel2And3Percent = totalIdsGivenByTel2And3Percent + iDNosGivenByTel2and3Percent;
        //                                    totaliDNosGivenByTel1StlPercent = totaliDNosGivenByTel1StlPercent + iDNosGivenByTel1StlPercent;
        //                                    totaliDNosGivenByTel2and3StlPercent = totaliDNosGivenByTel2and3StlPercent + iDNosGivenByTel2and3StlPercent;

        //                                    totalsalesWithIDNumber += salesWithIDNumber;
        //                                    totalsalesWithoutIDNumber += salesWithoutIDNumber;

        //                                    totalEmailAddressPercent = totalEmailAddressPercent + emailAddressesPercent;
        //                                    //totalAverageLeads = Math.Round(totalAverageLeads + AverageAgeOfLeads, 0);
        //                                    //totalAverageClients = Math.Round(totalAverageClients + AverageAgeOfClients, 2);

        //                                    totalSalesWithIDNumberPercentage = totalSalesWithIDNumberPercentage + (double)salesWithIDNumberPercentage;
        //                                    totalSalesWithoutIDNumberPercentage = totalSalesWithoutIDNumberPercentage + (double)salesWithoutIDNumberPercentage;
        //                                    totalAverageDaysBetweenLeadCreationAndImport = totalAverageDaysBetweenLeadCreationAndImport + AverageDaysBetweenLeadCreationAndImport;

        //                                    totalPensioners += (double)pensioners;
        //                                    totalHousewives += (double)housewives;

        //                                    totalRePrimedLeads += (double)reprimedLeads;
        //                                    totalRePrimedLeadsSTL += (double)reprimedLeadsSTL;

        //                                    totalIndianLeads += (double)indianLeads;
        //                                    totalIndianLeadsSTL += (double)indianLeadsSTL;

        //                                    totalAfricanLeads += africanLeads;
        //                                    totalAfricanLeadsSTL += africanLeadsSTL;

        //                                    totalReMarketedLeads += reMarketedLeads;
        //                                    totalReMarketedLeadsSTL += reMarketedLeadsSTL;

        //                                    totalReMarketedLeads1 += reMarketedLeads1;
        //                                    totalReMarketedLeadsSTL1 += reMarketedLeadsSTL1;

        //                                    totalReMarketedLeads2 += reMarketedLeads2;
        //                                    totalReMarketedLeadsSTL2 += reMarketedLeadsSTL2;

        //                                    totalReMarketedLeads3 += reMarketedLeads3;
        //                                    totalReMarketedLeadsSTL3 += reMarketedLeadsSTL3;

        //                                    totalReMarketedLeads4 += reMarketedLeads4;
        //                                    totalReMarketedLeadsSTL4 += reMarketedLeadsSTL4;

        //                                    totalNOKContactDetailsLeads += NOKContactDetailsLeads;
        //                                    totalNOKContactDetailsSTL += NOKContactDetailsSTL;

        //                                    totalBenContactDetailsLeads += BenContactDetailsLeads;
        //                                    totalBenContactDetailsSTL += BenContactDetailsSTL;

        //                                    totalLA2ContactDetailsLeads += LA2ContactDetailsLeads;
        //                                    totalLA2ContactDetailsSTL += LA2ContactDetailsSTL;

        //                                    totalEmailAddresses += emailAddressCount;

        //                                    GrandTotalOfClientsAges += TotalOfClientsAges;
        //                                    TotalCountOfClientsWithAges += CountOfClientsWithAges;

        //                                    GrandTotalOfDaysBetweenLeadCreationAndImport += TotalOfDaysBetweenLeadCreationAndImport;
        //                                    TotalCountOfDaysBetweenLeadCreationAndImport += CountOfDaysBetweenLeadCreationAndImport;

        //                                    GrandTotalOfLeadsAges += TotalOfLeadsAges;
        //                                    TotalCountOfLeadsWithAges += CountOfLeadsWithAges;

        //                                    #endregion Updating the totals

        //                                }

        //                                #region Totals




        //                                xlWorkSheet[t].Cells[rowIndex, 1].Font.Bold = true;


        //                                for (int i = 1; i <= reportColumnCount; i++) // this will aply it form col 1 to 20
        //                                {

        //                                    //rowIndex = 8;

        //                                    xlWorkSheet[t].Cells[rowIndex, i].Font.Bold = true;
        //                                    xlWorkSheet[t].Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
        //                                }

        //                                //rowIndex = 8;

        //                                xlWorkSheet[t].Cells[rowIndex, 1] = dr.Cells["CampaignCode"].Value.ToString().ToString();
        //                                xlWorkSheet[t].Cells[rowIndex, 2] = string.Empty;
        //                                xlWorkSheet[t].Cells[rowIndex, 3] = string.Empty;
        //                                xlWorkSheet[t].Cells[rowIndex, 4] = string.Empty;
        //                                xlWorkSheet[t].Cells[rowIndex, 5] = totalLeads;
        //                                xlWorkSheet[t].Cells[rowIndex, 6] = totalTelNo1;

        //                                xlWorkSheet[t].Cells[rowIndex, 9] = totalTelNo2;
        //                                xlWorkSheet[t].Cells[rowIndex, 12] = totalTelNo3;
        //                                xlWorkSheet[t].Cells[rowIndex, 15] = TotalIDs;
        //                                xlWorkSheet[t].Cells[rowIndex, 18] = totalNoIDs;
        //                                xlWorkSheet[t].Cells[rowIndex, 21] = totalIDnumbersByTel1;
        //                                xlWorkSheet[t].Cells[rowIndex, 23] = totalIDnumbersByTel2And3;

        //                                xlWorkSheet[t].Cells[rowIndex, 31] = totalPensioners;
        //                                xlWorkSheet[t].Cells[rowIndex, 33] = totalHousewives;

        //                                xlWorkSheet[t].Cells[rowIndex, 35] = totalRePrimedLeads;
        //                                xlWorkSheet[t].Cells[rowIndex, 38] = totalIndianLeads;
        //                                xlWorkSheet[t].Cells[rowIndex, 41] = totalAfricanLeads;

        //                                xlWorkSheet[t].Cells[rowIndex, 44] = totalReMarketedLeads;
        //                                xlWorkSheet[t].Cells[rowIndex, 47] = totalReMarketedLeads1;
        //                                xlWorkSheet[t].Cells[rowIndex, 50] = totalReMarketedLeads2;
        //                                xlWorkSheet[t].Cells[rowIndex, 53] = totalReMarketedLeads3;
        //                                xlWorkSheet[t].Cells[rowIndex, 56] = totalReMarketedLeads4;

        //                                xlWorkSheet[t].Cells[rowIndex, 59] = totalNOKContactDetailsLeads;

        //                                xlWorkSheet[t].Cells[rowIndex, 62] = totalBenContactDetailsLeads;

        //                                xlWorkSheet[t].Cells[rowIndex, 65] = totalLA2ContactDetailsLeads;

        //                                if (batchCodes.Count > 0)
        //                                {
        //                                    xlWorkSheet[t].Cells[rowIndex, 22] = Math.Round(totalSalesWithoutIDNumberPercentage / batchCodes.Count, 2) + " %"; //"";

        //                                    xlWorkSheet[t].Cells[rowIndex, 23] = Math.Round(totalIdsGivenByTel1Percent / batchCodes.Count, 2) + " %";
        //                                    xlWorkSheet[t].Cells[rowIndex, 24] = Math.Round(totaliDNosGivenByTel1StlPercent / batchCodes.Count, 2) + " %";
        //                                    xlWorkSheet[t].Cells[rowIndex, 25] = Math.Round(totalIdsGivenByTel2And3Percent / batchCodes.Count, 2) + " %";
        //                                    xlWorkSheet[t].Cells[rowIndex, 26] = Math.Round(totaliDNosGivenByTel2and3StlPercent / batchCodes.Count, 2) + " %";

        //                                    if (totalLeads > 0)
        //                                    {
        //                                        xlWorkSheet[t].Cells[rowIndex, 7] = Math.Round(totalTelNo1 / totalLeads * 100, 2) + " %";
        //                                        if (totalTelNo1 > 0)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, 8] = Math.Round(totaltelno1xStl / totalTelNo1 * 100, 2) + " %";
        //                                        }

        //                                        xlWorkSheet[t].Cells[rowIndex, 10] = Math.Round(totalTelNo2 / totalLeads * 100, 2) + " %";
        //                                        if (totalTelNo1 > 0)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, 11] = Math.Round(totaltelno2xStl / totalTelNo2 * 100, 2) + " %";
        //                                        }

        //                                        xlWorkSheet[t].Cells[rowIndex, 13] = Math.Round(totalTelNo3 / totalLeads * 100, 2) + " %";
        //                                        if (totalTelNo1 > 0)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, 14] = Math.Round(totaltelno3xStl / totalTelNo3 * 100, 2) + " %";
        //                                        }

        //                                        xlWorkSheet[t].Cells[rowIndex, 16] = Math.Round(TotalIDs / totalLeads * 100, 2) + " %";
        //                                        if (TotalIDs > 0)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, 17] = Math.Round(totalsalesWithIDNumber / TotalIDs * 100, 2) + " %";
        //                                        }

        //                                        xlWorkSheet[t].Cells[rowIndex, 19] = Math.Round(totalNoIDs / totalLeads * 100, 2) + " %";
        //                                        if (totalNoIDs > 0)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, 20] = Math.Round(totalsalesWithoutIDNumber / totalNoIDs * 100, 2) + " %";
        //                                        }

        //                                        xlWorkSheet[t].Cells[rowIndex, 27] = Math.Round(totalEmailAddresses / totalLeads * 100, 2) + " %";

        //                                        if (TotalCountOfLeadsWithAges > 0)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, 28] = Math.Round(GrandTotalOfLeadsAges / TotalCountOfLeadsWithAges, 2);
        //                                        }
        //                                        xlRange = xlWorkSheet[t].Cells[rowIndex, 28];
        //                                        xlRange.NumberFormat = "0.00";

        //                                        if (TotalCountOfDaysBetweenLeadCreationAndImport > 0)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, 29] = Math.Round(GrandTotalOfDaysBetweenLeadCreationAndImport / TotalCountOfDaysBetweenLeadCreationAndImport, 2);
        //                                        }
        //                                        xlRange = xlWorkSheet[t].Cells[rowIndex, 29];
        //                                        xlRange.NumberFormat = "0.00";


        //                                        if (TotalCountOfClientsWithAges > 0)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, 30] = Math.Round(GrandTotalOfClientsAges / TotalCountOfClientsWithAges, 2);
        //                                        }
        //                                        xlRange = xlWorkSheet[t].Cells[rowIndex, 30];
        //                                        xlRange.NumberFormat = "0.00";

        //                                        #region Pensioners & Housewives

        //                                        xlWorkSheet[t].Cells[rowIndex, 32] = Math.Round(totalPensioners / (double)totalLeads * 100, 2) + " %";
        //                                        xlWorkSheet[t].Cells[rowIndex, 34] = Math.Round(totalHousewives / (double)totalLeads * 100, 2) + " %";

        //                                        #endregion Pensioners & Housewives

        //                                        #region Re-Primed Leads

        //                                        xlWorkSheet[t].Cells[rowIndex, 36] = Math.Round(totalRePrimedLeads / (double)totalLeads * 100, 2) + " %";
        //                                        if (totalRePrimedLeads > 0)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, 36] = Math.Round(totalRePrimedLeadsSTL / (double)totalRePrimedLeads * 100, 2) + " %";
        //                                        }
        //                                        else
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, 37] = "0 %";
        //                                        }

        //                                        #endregion Re-Primed Leads

        //                                        #region Indian Leads

        //                                        xlWorkSheet[t].Cells[rowIndex, 39] = Math.Round(totalIndianLeads / (double)totalLeads * 100, 2) + " %";

        //                                        if (totalIndianLeads > 0)
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, 40] = Math.Round(totalIndianLeadsSTL / totalIndianLeads * 100, 2) + " %";
        //                                        }
        //                                        else
        //                                        {
        //                                            xlWorkSheet[t].Cells[rowIndex, 40] = "0 %";
        //                                        }

        //                                        #endregion Indian Leads

        //                                        #region African Leads

        //                                        dResult = totalLeads > 0 ? Math.Round(totalAfricanLeads / totalLeads, 4) : 0;
        //                                        xlWorkSheet[t].Cells[rowIndex, 42] = dResult;
        //                                        xlRange = xlWorkSheet[t].Cells[rowIndex, 42];
        //                                        xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

        //                                        dResult = totalAfricanLeads > 0 ? Math.Round(totalAfricanLeadsSTL / totalAfricanLeads, 4) : 0;
        //                                        xlWorkSheet[t].Cells[rowIndex, 43] = dResult;
        //                                        xlRange = xlWorkSheet[t].Cells[rowIndex, 43];
        //                                        xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

        //                                        #endregion African Leads

        //                                        #region ReMarketed Leads

        //                                        {
        //                                            dResult = totalLeads > 0 ? Math.Round(totalReMarketedLeads / totalLeads, 4) : 0;
        //                                            xlWorkSheet[t].Cells[rowIndex, 45] = dResult;
        //                                            xlRange = xlWorkSheet[t].Cells[rowIndex, 45];
        //                                            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

        //                                            dResult = totalReMarketedLeads > 0 ? Math.Round(totalReMarketedLeadsSTL / totalReMarketedLeads, 4) : 0;
        //                                            xlWorkSheet[t].Cells[rowIndex, 46] = dResult;
        //                                            xlRange = xlWorkSheet[t].Cells[rowIndex, 46];
        //                                            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
        //                                        }

        //                                        {
        //                                            dResult = totalReMarketedLeads > 0 ? Math.Round(totalReMarketedLeads1 / totalReMarketedLeads, 4) : 0;
        //                                            xlWorkSheet[t].Cells[rowIndex, 48] = dResult;
        //                                            xlRange = xlWorkSheet[t].Cells[rowIndex, 48];
        //                                            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

        //                                            dResult = totalReMarketedLeads1 > 0 ? Math.Round(totalReMarketedLeadsSTL1 / totalReMarketedLeads1, 4) : 0;
        //                                            xlWorkSheet[t].Cells[rowIndex, 49] = dResult;
        //                                            xlRange = xlWorkSheet[t].Cells[rowIndex, 49];
        //                                            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
        //                                        }

        //                                        {
        //                                            dResult = totalReMarketedLeads > 0 ? Math.Round(totalReMarketedLeads2 / totalReMarketedLeads, 4) : 0;
        //                                            xlWorkSheet[t].Cells[rowIndex, 51] = dResult;
        //                                            xlRange = xlWorkSheet[t].Cells[rowIndex, 51];
        //                                            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

        //                                            dResult = totalReMarketedLeads2 > 0 ? Math.Round(totalReMarketedLeadsSTL2 / totalReMarketedLeads2, 4) : 0;
        //                                            xlWorkSheet[t].Cells[rowIndex, 52] = dResult;
        //                                            xlRange = xlWorkSheet[t].Cells[rowIndex, 52];
        //                                            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
        //                                        }

        //                                        {
        //                                            dResult = totalReMarketedLeads > 0 ? Math.Round(totalReMarketedLeads3 / totalReMarketedLeads, 4) : 0;
        //                                            xlWorkSheet[t].Cells[rowIndex, 54] = dResult;
        //                                            xlRange = xlWorkSheet[t].Cells[rowIndex, 54];
        //                                            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

        //                                            dResult = totalReMarketedLeads3 > 0 ? Math.Round(totalReMarketedLeadsSTL3 / totalReMarketedLeads3, 4) : 0;
        //                                            xlWorkSheet[t].Cells[rowIndex, 55] = dResult;
        //                                            xlRange = xlWorkSheet[t].Cells[rowIndex, 55];
        //                                            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
        //                                        }

        //                                        {
        //                                            dResult = totalReMarketedLeads > 0 ? Math.Round(totalReMarketedLeads4 / totalReMarketedLeads, 4) : 0;
        //                                            xlWorkSheet[t].Cells[rowIndex, 57] = dResult;
        //                                            xlRange = xlWorkSheet[t].Cells[rowIndex, 57];
        //                                            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

        //                                            dResult = totalReMarketedLeads4 > 0 ? Math.Round(totalReMarketedLeadsSTL4 / totalReMarketedLeads4, 4) : 0;
        //                                            xlWorkSheet[t].Cells[rowIndex, 58] = dResult;
        //                                            xlRange = xlWorkSheet[t].Cells[rowIndex, 58];
        //                                            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
        //                                        }

        //                                        #endregion ReMarketed Leads

        //                                        #region NOK Contact Detail Leads
        //                                        {
        //                                            dResult = totalLeads > 0 ? Math.Round(totalNOKContactDetailsLeads / totalLeads, 4) : 0;
        //                                            xlWorkSheet[t].Cells[rowIndex, 60] = dResult;
        //                                            xlRange = xlWorkSheet[t].Cells[rowIndex, 60];
        //                                            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

        //                                            dResult = totalNOKContactDetailsLeads > 0 ? Math.Round(totalNOKContactDetailsSTL / totalNOKContactDetailsLeads, 4) : 0;
        //                                            xlWorkSheet[t].Cells[rowIndex, 61] = dResult;
        //                                            xlRange = xlWorkSheet[t].Cells[rowIndex, 61];
        //                                            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
        //                                        }
        //                                        #endregion NOK Contact Detail Leads

        //                                        #region Ben Contact Detail Leads
        //                                        {
        //                                            dResult = totalLeads > 0 ? Math.Round(totalBenContactDetailsLeads / totalLeads, 4) : 0;
        //                                            xlWorkSheet[t].Cells[rowIndex, 63] = dResult;
        //                                            xlRange = xlWorkSheet[t].Cells[rowIndex, 63];
        //                                            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

        //                                            dResult = totalBenContactDetailsLeads > 0 ? Math.Round(totalBenContactDetailsSTL / totalBenContactDetailsLeads, 4) : 0;
        //                                            xlWorkSheet[t].Cells[rowIndex, 64] = dResult;
        //                                            xlRange = xlWorkSheet[t].Cells[rowIndex, 64];
        //                                            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
        //                                        }
        //                                        #endregion Ben Contact Detail Leads

        //                                        #region LA2 Contact Detail Leads
        //                                        {
        //                                            dResult = totalLeads > 0 ? Math.Round(totalLA2ContactDetailsLeads / totalLeads, 4) : 0;
        //                                            xlWorkSheet[t].Cells[rowIndex, 66] = dResult;
        //                                            xlRange = xlWorkSheet[t].Cells[rowIndex, 66];
        //                                            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";

        //                                            dResult = totalLA2ContactDetailsLeads > 0 ? Math.Round(totalLA2ContactDetailsSTL / totalLA2ContactDetailsLeads, 4) : 0;
        //                                            xlWorkSheet[t].Cells[rowIndex, 67] = dResult;
        //                                            xlRange = xlWorkSheet[t].Cells[rowIndex, 67];
        //                                            xlRange.NumberFormat = dResult == 1m ? "0%" : "0.00%";
        //                                        }
        //                                        #endregion LA2 Contact Detail Leads
        //                                    }
        //                                    else
        //                                    {
        //                                        xlWorkSheet[t].Cells[rowIndex, 35] = "0 %";
        //                                        //xlWorkSheet.Cells[rowIndex, 36] = "0 %";
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    //xlWorkSheet.Cells[rowIndex, 6] = "0 %";
        //                                    //xlWorkSheet.Cells[rowIndex, 7] = "0 %";
        //                                    //xlWorkSheet.Cells[rowIndex, 9] = "0 %";
        //                                    //xlWorkSheet.Cells[rowIndex, 10] = "0 %";
        //                                    //xlWorkSheet.Cells[rowIndex, 12] = "0 %";
        //                                    //xlWorkSheet.Cells[rowIndex, 13] = "0 %";
        //                                    //xlWorkSheet.Cells[rowIndex, 15] = "0 %";
        //                                    //xlWorkSheet.Cells[rowIndex, 17] = "0 %";
        //                                    //xlWorkSheet.Cells[rowIndex, 19] = "0 %";
        //                                    //xlWorkSheet.Cells[rowIndex, 20] = "0 %";
        //                                    //xlWorkSheet.Cells[rowIndex, 22] = "0 %";
        //                                    //xlWorkSheet.Cells[rowIndex, 23] = "0 %";
        //                                    //xlWorkSheet.Cells[rowIndex, 24] = "0 %";
        //                                    //xlWorkSheet.Cells[rowIndex, 25] = "0 %";
        //                                    //xlWorkSheet.Cells[rowIndex, 26] = "0 %";

        //                                    xlWorkSheet[t].Cells[rowIndex, 6] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 7] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 9] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 10] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 12] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 13] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 15] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 16] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 18] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 19] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 21] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 22] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 24] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 25] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 26] = "0%";
        //                                    //xlWorkSheet.Cells[rowIndex, 27] = "0%";
        //                                    //xlWorkSheet.Cells[rowIndex, 28] = "0";
        //                                    //xlWorkSheet.Cells[rowIndex, 29] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 31] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 33] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 35] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 36] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 38] = "0%";
        //                                    xlWorkSheet[t].Cells[rowIndex, 39] = "0%";
        //                                }



        //                                //add summary
        //                                formulaToRow = rowIndex - 1;

        //                                rowIndex = 20;

        //                                //rowIndex++; rowIndex++; rowIndex++;
        //                                Excel.Range desc1 = xlWorkSheet[t].Cells[rowIndex, 1];
        //                                Excel.Range desc2 = xlWorkSheet[t].Cells[rowIndex, 6];

        //                                Excel.Range descrange = (Excel.Range)xlWorkSheet[t].get_Range(desc1, desc2);
        //                                descrange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                descrange.Font.Bold = true;
        //                                descrange.Merge(true);
        //                                descrange.Borders.Weight = Excel.XlBorderWeight.xlMedium;
        //                                xlWorkSheet[t].Cells[rowIndex, 1] = "Column Descriptions";
        //                                xlWorkSheet[t].Cells[rowIndex, 1].Font.Bold = true;

        //                                rowIndex = 21;

        //                                Excel.Range descsub1 = xlWorkSheet[t].Cells[rowIndex, 1];
        //                                Excel.Range descsub2 = xlWorkSheet[t].Cells[rowIndex, 2];
        //                                Excel.Range descSubrange = (Excel.Range)xlWorkSheet[t].get_Range(descsub1, descsub2);
        //                                descSubrange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                descSubrange.Font.Bold = true;
        //                                descSubrange.Merge(true);
        //                                descSubrange.Borders.Weight = Excel.XlBorderWeight.xlMedium;
        //                                xlWorkSheet[t].Cells[rowIndex, 1] = "Column Name";

        //                                Excel.Range descsubb1 = xlWorkSheet[t].Cells[rowIndex, 3];
        //                                Excel.Range descsubb2 = xlWorkSheet[t].Cells[rowIndex, 6];
        //                                Excel.Range descSubbrange = (Excel.Range)xlWorkSheet[t].get_Range(descsubb1, descsubb2);
        //                                descSubbrange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                descSubbrange.Font.Bold = true;
        //                                descSubbrange.Merge(true);
        //                                descSubbrange.Borders.Weight = Excel.XlBorderWeight.xlMedium;
        //                                xlWorkSheet[t].Cells[rowIndex, 3] = "Description";

        //                                //rowIndex++;

        //                                rowIndex = 22;

        //                                Excel.Range descval1 = xlWorkSheet[t].Cells[rowIndex, 1];
        //                                Excel.Range descval2 = xlWorkSheet[t].Cells[rowIndex, 2];
        //                                Excel.Range descval1range = (Excel.Range)xlWorkSheet[t].get_Range(descval1, descval2);
        //                                descval1range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

        //                                descval1range.Merge(true);
        //                                descval1range.Borders.Weight = Excel.XlBorderWeight.xlThin;
        //                                xlWorkSheet[t].Cells[rowIndex, 1] = "Average age of leads (Days)";

        //                                Excel.Range desval3 = xlWorkSheet[t].Cells[rowIndex, 3];
        //                                Excel.Range desval4 = xlWorkSheet[t].Cells[rowIndex, 6];
        //                                Excel.Range descval2range = (Excel.Range)xlWorkSheet[t].get_Range(desval3, desval4);
        //                                descval2range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

        //                                descval2range.Merge(true);
        //                                descval2range.Borders.Weight = Excel.XlBorderWeight.xlThin;
        //                                xlWorkSheet[t].Cells[rowIndex, 3] = "Last PL contact to UDM import";

        //                                #endregion Totals

        //                                #region Hiding the unused columns

        //                                // See

        //                                var hiddenRange = xlWorkSheet[t].Range[xlWorkSheet[t].Cells[1, 20], xlWorkSheet[t].Cells[1, 25]];
        //                                hiddenRange.EntireColumn.Hidden = true;

        //                                #endregion Hiding the unused columns

        //                                #region Charts

        //                                #region Percentage Tel No

        //                                Excel.Worksheet xlWorkSheetChartTelNo;

        //                                xlWorkSheetChartTelNo = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(t);
        //                            xlWorkSheetChartTelNo.Name = codeCampaign;

        //                                #region Adding the report heading

        //                                Excel.Range chartSheetHeading1 = xlWorkSheetChartTelNo.Cells[1, 1];
        //                                Excel.Range chartSheetHeading2 = xlWorkSheetChartTelNo.Cells[1, 14];
        //                                Excel.Range chartSheetHeadingRange = (Excel.Range)xlWorkSheetChartTelNo.get_Range(chartSheetHeading1, chartSheetHeading2);

        //                                chartSheetHeadingRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                chartSheetHeadingRange.Font.Bold = true;
        //                                chartSheetHeadingRange.Font.Size = 16;
        //                                chartSheetHeadingRange.Borders.Weight = Excel.XlBorderWeight.xlThick;
        //                                chartSheetHeadingRange.Merge(true);
        //                                xlWorkSheetChartTelNo.Cells[1, 1] = string.Format("Batch Analysis Report - {0}", codeCampaign);

        //                                #endregion Adding the report heading

        //                                #region Adding the report subheading

        //                                Excel.Range chartSheetSubHeading1 = xlWorkSheetChartTelNo.Cells[3, 1];
        //                                Excel.Range chartSheetSubHeading2 = xlWorkSheetChartTelNo.Cells[3, 14];
        //                                Excel.Range chartSheetSubHeadingRange = (Excel.Range)xlWorkSheetChartTelNo.get_Range(chartSheetSubHeading1, chartSheetSubHeading2);
        //                                chartSheetSubHeadingRange.Merge(true);
        //                                chartSheetSubHeadingRange.Font.Bold = true;
        //                                chartSheetSubHeadingRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

        //                                xlWorkSheetChartTelNo.Cells[3, 1] = string.Format("For the period between {0} and {1}", _startDate.ToString("d MMMM yyyy"), _endDate.ToString("d MMMM yyyy"));

        //                                #endregion Adding the report subheading

        //                                #region Adding the report date

        //                                Excel.Range chartSheetDate1 = xlWorkSheetChartTelNo.Cells[5, 1];
        //                                Excel.Range chartSheetDate2 = xlWorkSheetChartTelNo.Cells[5, 14];
        //                                Excel.Range chartSheetDateCellRange = (Excel.Range)xlWorkSheetChartTelNo.get_Range(chartSheetDate1, chartSheetDate2);
        //                                chartSheetDateCellRange.Font.Bold = true;
        //                                chartSheetDateCellRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
        //                                chartSheetDateCellRange.Merge(true);

        //                                xlWorkSheetChartTelNo.Cells[5, 1] = string.Format("Date Generated: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        //                                //xlWorkSheetChartTelNo.Cells[4, 27] = "Date Generated:";
        //                                //xlWorkSheetChartTelNo.Cells[4, 28] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //                                #endregion Adding the report date

        //                                Excel.ChartObjects xlCharts = (Excel.ChartObjects)xlWorkSheetChartTelNo.ChartObjects(Type.Missing);
        //                                Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(1, 600, 670, 400);
        //                                Excel.Chart chartPage = myChart.Chart;
        //                                chartPage.HasTitle = true;
        //                                chartPage.ChartTitle.Text = "% Telephone Numbers";

        //                                var yAxis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
        //                                yAxis.HasTitle = true;
        //                                yAxis.AxisTitle.Text = "Percentage";

        //                                Excel.SeriesCollection oSeriesCollection = (Excel.SeriesCollection)myChart.Chart.SeriesCollection(misValue);
        //                                Excel.Series series1 = oSeriesCollection.NewSeries();
        //                                Excel.Series series2 = oSeriesCollection.NewSeries();
        //                                Excel.Series series3 = oSeriesCollection.NewSeries();

        //                                Excel.Range series1_range = xlWorkSheet[t].get_Range("f" + formulaFromRow, "f" + formulaToRow);
        //                                Excel.Range series2_range = xlWorkSheet[t].get_Range("i" + formulaFromRow, "i" + formulaToRow);
        //                                Excel.Range series3_range = xlWorkSheet[t].get_Range("l" + formulaFromRow, "l" + formulaToRow);

        //                                series1.Values = series1_range;
        //                                series2.Values = series2_range;
        //                                series3.Values = series3_range;

        //                                series1.Name = "1 x Tel No. %";
        //                                series2.Name = "2 x Tel No. %";
        //                                series3.Name = "3 x Tel No. %";

        //                                series1.XValues = xlWorkSheet[t].get_Range("a" + formulaFromRow, "a" + formulaToRow);
        //                                chartPage.ChartType = Excel.XlChartType.xlLine;

        //                                #endregion Percentage Tel No

        //                                #region Percentage ID & DOB

        //                                Excel.Worksheet xlWorkSheetChartIDDob;
        //                                xlWorkSheetChartIDDob = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(t);
        //                                xlWorkSheetChartIDDob.Name = codeCampaign;

        //                                #region Adding the report heading

        //                            chartSheetHeading1 = xlWorkSheetChartIDDob.Cells[1, 1];
        //                                chartSheetHeading2 = xlWorkSheetChartIDDob.Cells[1, 14];
        //                                chartSheetHeadingRange = (Excel.Range)xlWorkSheetChartIDDob.get_Range(chartSheetHeading1, chartSheetHeading2);

        //                                chartSheetHeadingRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                chartSheetHeadingRange.Font.Bold = true;
        //                                chartSheetHeadingRange.Font.Size = 16;
        //                                chartSheetHeadingRange.Borders.Weight = Excel.XlBorderWeight.xlThick;
        //                                chartSheetHeadingRange.Merge(true);
        //                                xlWorkSheetChartIDDob.Cells[1, 1] = string.Format("Batch Analysis Report - {0}", codeCampaign);

        //                                #endregion Adding the report heading

        //                                #region Adding the report subheading

        //                                chartSheetSubHeading1 = xlWorkSheetChartIDDob.Cells[3, 1];
        //                                chartSheetSubHeading2 = xlWorkSheetChartIDDob.Cells[3, 14];
        //                                chartSheetSubHeadingRange = (Excel.Range)xlWorkSheetChartIDDob.get_Range(chartSheetSubHeading1, chartSheetSubHeading2);
        //                                chartSheetSubHeadingRange.Merge(true);
        //                                chartSheetSubHeadingRange.Font.Bold = true;
        //                                chartSheetSubHeadingRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

        //                                xlWorkSheetChartIDDob.Cells[3, 1] = string.Format("For the period between {0} and {1}", _startDate.ToString("d MMMM yyyy"), _endDate.ToString("d MMMM yyyy"));

        //                                #endregion Adding the report subheading

        //                                #region Adding the report date

        //                                chartSheetDate1 = xlWorkSheetChartIDDob.Cells[5, 1];
        //                                chartSheetDate2 = xlWorkSheetChartIDDob.Cells[5, 14];
        //                                chartSheetDateCellRange = (Excel.Range)xlWorkSheetChartIDDob.get_Range(chartSheetDate1, chartSheetDate2);
        //                                chartSheetDateCellRange.Font.Bold = true;
        //                                chartSheetDateCellRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
        //                                chartSheetDateCellRange.Merge(true);

        //                                xlWorkSheetChartIDDob.Cells[5, 1] = string.Format("Date Generated: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        //                                #endregion Adding the report date

        //                                Excel.ChartObjects xlChartsIDDob = (Excel.ChartObjects)xlWorkSheetChartIDDob.ChartObjects(Type.Missing);
        //                                Excel.ChartObject myChartIDDOB = (Excel.ChartObject)xlChartsIDDob.Add(680, 600, 670, 400);
        //                                Excel.Chart chartPageIDDOB = myChartIDDOB.Chart;
        //                                chartPageIDDOB.HasTitle = true;
        //                                chartPageIDDOB.ChartTitle.Text = "% ID Numbers & Date Of Births";

        //                                var yAxisIDDOB = (Excel.Axis)chartPageIDDOB.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
        //                                yAxisIDDOB.HasTitle = true;
        //                                yAxisIDDOB.AxisTitle.Text = "Percentage";

        //                                Excel.SeriesCollection oSeriesCollectionIDDOB = (Excel.SeriesCollection)myChartIDDOB.Chart.SeriesCollection(misValue);
        //                                Excel.Series series1IDDOB = oSeriesCollectionIDDOB.NewSeries();
        //                                Excel.Series series2IDDOB = oSeriesCollectionIDDOB.NewSeries();


        //                                Excel.Range series1IDDOB_range = xlWorkSheet[t].get_Range("o" + formulaFromRow, "o" + formulaToRow);
        //                                Excel.Range series2IDDOB_range = xlWorkSheet[t].get_Range("r" + formulaFromRow, "r" + formulaToRow);

        //                                series1IDDOB.Values = series1IDDOB_range;
        //                                series2IDDOB.Values = series2IDDOB_range;


        //                                series1IDDOB.Name = "ID Numbers %";
        //                                series2IDDOB.Name = "Date Of Birth %";

        //                                series1IDDOB.XValues = xlWorkSheet[t].get_Range("a" + formulaFromRow, "a" + (rowIndex - 1));
        //                                chartPageIDDOB.ChartType = Excel.XlChartType.xlLine;
        //                                #endregion Percentage ID & DOB

        //                                #region Percentage IDs to 1x2x3x

        //                                Excel.Worksheet xlWorkSheetChartIDsto1x2x3x;
        //                                xlWorkSheetChartIDsto1x2x3x = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(t);
        //                                xlWorkSheetChartIDsto1x2x3x.Name = codeCampaign;

        //                                #region Adding the report heading

        //                                chartSheetHeading1 = xlWorkSheetChartIDsto1x2x3x.Cells[1, 1];
        //                                chartSheetHeading2 = xlWorkSheetChartIDsto1x2x3x.Cells[1, 14];
        //                                chartSheetHeadingRange = (Excel.Range)xlWorkSheetChartIDsto1x2x3x.get_Range(chartSheetHeading1, chartSheetHeading2);

        //                                chartSheetHeadingRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                chartSheetHeadingRange.Font.Bold = true;
        //                                chartSheetHeadingRange.Font.Size = 16;
        //                                chartSheetHeadingRange.Borders.Weight = Excel.XlBorderWeight.xlThick;
        //                                chartSheetHeadingRange.Merge(true);
        //                                xlWorkSheetChartIDsto1x2x3x.Cells[1, 1] = string.Format("Batch Analysis Report - {0}", codeCampaign);

        //                                #endregion Adding the report heading

        //                                #region Adding the report subheading

        //                                chartSheetSubHeading1 = xlWorkSheetChartIDsto1x2x3x.Cells[3, 1];
        //                                chartSheetSubHeading2 = xlWorkSheetChartIDsto1x2x3x.Cells[3, 14];
        //                                chartSheetSubHeadingRange = (Excel.Range)xlWorkSheetChartIDsto1x2x3x.get_Range(chartSheetSubHeading1, chartSheetSubHeading2);
        //                                chartSheetSubHeadingRange.Merge(true);
        //                                chartSheetSubHeadingRange.Font.Bold = true;
        //                                chartSheetSubHeadingRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

        //                                xlWorkSheetChartIDsto1x2x3x.Cells[3, 1] = string.Format("For the period between {0} and {1}", _startDate.ToString("d MMMM yyyy"), _endDate.ToString("d MMMM yyyy"));

        //                                #endregion Adding the report subheading

        //                                #region Adding the report date

        //                                chartSheetDate1 = xlWorkSheetChartIDsto1x2x3x.Cells[5, 1];
        //                                chartSheetDate2 = xlWorkSheetChartIDsto1x2x3x.Cells[5, 14];
        //                                chartSheetDateCellRange = (Excel.Range)xlWorkSheetChartIDsto1x2x3x.get_Range(chartSheetDate1, chartSheetDate2);
        //                                chartSheetDateCellRange.Font.Bold = true;
        //                                chartSheetDateCellRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
        //                                chartSheetDateCellRange.Merge(true);

        //                                xlWorkSheetChartIDsto1x2x3x.Cells[5, 1] = string.Format("Date Generated: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        //                                #endregion Adding the report date

        //                                Excel.ChartObjects xlChartsIDsto1x2x3x = (Excel.ChartObjects)xlWorkSheetChartIDsto1x2x3x.ChartObjects(Type.Missing);
        //                                Excel.ChartObject myChartIDsto1x2x3x = (Excel.ChartObject)xlChartsIDsto1x2x3x.Add(1360, 600, 670, 400);
        //                                Excel.Chart chartPageIDsto1x2x3x = myChartIDsto1x2x3x.Chart;
        //                                chartPageIDsto1x2x3x.HasTitle = true;
        //                                chartPageIDsto1x2x3x.ChartTitle.Text = "% of ID numbers given to 1x and 2x/3x telephone numbers";

        //                                var yAxisIDsto1x2x3x = (Excel.Axis)chartPageIDsto1x2x3x.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
        //                                yAxisIDsto1x2x3x.HasTitle = true;
        //                                yAxisIDsto1x2x3x.AxisTitle.Text = "Percentage";

        //                                Excel.SeriesCollection oSeriesCollectionIDsto1x2x3x = (Excel.SeriesCollection)myChartIDsto1x2x3x.Chart.SeriesCollection(misValue);
        //                                Excel.Series series1IDsto1x2x3x = oSeriesCollectionIDsto1x2x3x.NewSeries();
        //                                Excel.Series series2IDsto1x2x3x = oSeriesCollectionIDsto1x2x3x.NewSeries();


        //                                Excel.Range series1IDsto1x2x3x_range = xlWorkSheet[t].get_Range("u" + formulaFromRow, "u" + formulaToRow);
        //                                Excel.Range series2IDsto1x2x3x_range = xlWorkSheet[t].get_Range("x" + formulaFromRow, "x" + formulaToRow);


        //                                series1IDsto1x2x3x.Values = series1IDsto1x2x3x_range;
        //                                series2IDsto1x2x3x.Values = series2IDsto1x2x3x_range;


        //                                series1IDsto1x2x3x.Name = "ID's to 1x Tel No. Leads";
        //                                series2IDsto1x2x3x.Name = "ID's to 2x/3x Tel No. Leads";


        //                                series1IDsto1x2x3x.XValues = xlWorkSheet[t].get_Range("a" + formulaFromRow, "a" + formulaToRow);
        //                                chartPageIDsto1x2x3x.ChartType = Excel.XlChartType.xlLine;
        //                                #endregion Percentage IDs to 1x2x3x

        //                                #region Percentage Email Addresses

        //                                Excel.Worksheet xlWorkSheetChartEmailAddressPercent;
        //                                xlWorkSheetChartEmailAddressPercent = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(t);
        //                                xlWorkSheetChartEmailAddressPercent.Name = codeCampaign;

        //                                #region Adding the report heading

        //                                chartSheetHeading1 = xlWorkSheetChartEmailAddressPercent.Cells[1, 1];
        //                                chartSheetHeading2 = xlWorkSheetChartEmailAddressPercent.Cells[1, 14];
        //                                chartSheetHeadingRange = (Excel.Range)xlWorkSheetChartEmailAddressPercent.get_Range(chartSheetHeading1, chartSheetHeading2);

        //                                chartSheetHeadingRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                chartSheetHeadingRange.Font.Bold = true;
        //                                chartSheetHeadingRange.Font.Size = 16;
        //                                chartSheetHeadingRange.Borders.Weight = Excel.XlBorderWeight.xlThick;
        //                                chartSheetHeadingRange.Merge(true);
        //                                xlWorkSheetChartEmailAddressPercent.Cells[1, 1] = string.Format("Batch Analysis Report - {0}", codeCampaign);

        //                                #endregion Adding the report heading

        //                                #region Adding the report subheading

        //                                chartSheetSubHeading1 = xlWorkSheetChartEmailAddressPercent.Cells[3, 1];
        //                                chartSheetSubHeading2 = xlWorkSheetChartEmailAddressPercent.Cells[3, 14];
        //                                chartSheetSubHeadingRange = (Excel.Range)xlWorkSheetChartEmailAddressPercent.get_Range(chartSheetSubHeading1, chartSheetSubHeading2);
        //                                chartSheetSubHeadingRange.Merge(true);
        //                                chartSheetSubHeadingRange.Font.Bold = true;
        //                                chartSheetSubHeadingRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

        //                                xlWorkSheetChartEmailAddressPercent.Cells[3, 1] = string.Format("For the period between {0} and {1}", _startDate.ToString("d MMMM yyyy"), _endDate.ToString("d MMMM yyyy"));

        //                                #endregion Adding the report subheading

        //                                #region Adding the report date

        //                                chartSheetDate1 = xlWorkSheetChartEmailAddressPercent.Cells[5, 1];
        //                                chartSheetDate2 = xlWorkSheetChartEmailAddressPercent.Cells[5, 14];
        //                                chartSheetDateCellRange = (Excel.Range)xlWorkSheetChartEmailAddressPercent.get_Range(chartSheetDate1, chartSheetDate2);
        //                                chartSheetDateCellRange.Font.Bold = true;
        //                                chartSheetDateCellRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
        //                                chartSheetDateCellRange.Merge(true);

        //                                xlWorkSheetChartEmailAddressPercent.Cells[5, 1] = string.Format("Date Generated: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        //                                #endregion Adding the report date

        //                                Excel.ChartObjects xlChartsEmailAddressPercent = (Excel.ChartObjects)xlWorkSheetChartEmailAddressPercent.ChartObjects(Type.Missing);
        //                                Excel.ChartObject myChartEmailAddressPercent = (Excel.ChartObject)xlChartsEmailAddressPercent.Add(2040, 600, 670, 400);
        //                                Excel.Chart chartPageEmailAddressPercent = myChartEmailAddressPercent.Chart;
        //                                chartPageEmailAddressPercent.HasTitle = true;
        //                                chartPageEmailAddressPercent.ChartTitle.Text = "% Email Addresses";

        //                                var yAxisEmailAddressPercent = (Excel.Axis)chartPageEmailAddressPercent.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
        //                                yAxisEmailAddressPercent.HasTitle = true;
        //                                yAxisEmailAddressPercent.AxisTitle.Text = "Percentage";

        //                                Excel.SeriesCollection oSeriesCollectionEmailAddressPercent = (Excel.SeriesCollection)myChartEmailAddressPercent.Chart.SeriesCollection(misValue);
        //                                Excel.Series series1EmailAddressPercent = oSeriesCollectionEmailAddressPercent.NewSeries();

        //                                Excel.Range series1EmailAddressPercent_range = xlWorkSheet[t].get_Range("z" + formulaFromRow, "z" + formulaToRow);
        //                                series1EmailAddressPercent.Values = series1EmailAddressPercent_range;
        //                                series1EmailAddressPercent.Name = "% Email Addresses";

        //                                series1EmailAddressPercent.XValues = xlWorkSheet[t].get_Range("a" + formulaFromRow, "a" + formulaToRow);
        //                                chartPageEmailAddressPercent.ChartType = Excel.XlChartType.xlLine;
        //                                #endregion Percentage Email Addresses

        //                                #region Average Age Of Leads

        //                                Excel.Worksheet xlWorkSheetChartAverageAgeOfLeads;
        //                                xlWorkSheetChartAverageAgeOfLeads = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(t);
        //                                xlWorkSheetChartAverageAgeOfLeads.Name = codeCampaign;

        //                                #region Adding the report heading

        //                                chartSheetHeading1 = xlWorkSheetChartAverageAgeOfLeads.Cells[1, 1];
        //                                chartSheetHeading2 = xlWorkSheetChartAverageAgeOfLeads.Cells[1, 14];
        //                                chartSheetHeadingRange = (Excel.Range)xlWorkSheetChartAverageAgeOfLeads.get_Range(chartSheetHeading1, chartSheetHeading2);

        //                                chartSheetHeadingRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                chartSheetHeadingRange.Font.Bold = true;
        //                                chartSheetHeadingRange.Font.Size = 16;
        //                                chartSheetHeadingRange.Borders.Weight = Excel.XlBorderWeight.xlThick;
        //                                chartSheetHeadingRange.Merge(true);
        //                                xlWorkSheetChartAverageAgeOfLeads.Cells[1, 1] = string.Format("Batch Analysis Report - {0}", codeCampaign);

        //                                #endregion Adding the report heading

        //                                #region Adding the report subheading

        //                                chartSheetSubHeading1 = xlWorkSheetChartAverageAgeOfLeads.Cells[3, 1];
        //                                chartSheetSubHeading2 = xlWorkSheetChartAverageAgeOfLeads.Cells[3, 14];
        //                                chartSheetSubHeadingRange = (Excel.Range)xlWorkSheetChartAverageAgeOfLeads.get_Range(chartSheetSubHeading1, chartSheetSubHeading2);
        //                                chartSheetSubHeadingRange.Merge(true);
        //                                chartSheetSubHeadingRange.Font.Bold = true;
        //                                chartSheetSubHeadingRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

        //                                xlWorkSheetChartAverageAgeOfLeads.Cells[3, 1] = string.Format("For the period between {0} and {1}", _startDate.ToString("d MMMM yyyy"), _endDate.ToString("d MMMM yyyy"));

        //                                #endregion Adding the report subheading

        //                                #region Adding the report date

        //                                chartSheetDate1 = xlWorkSheetChartAverageAgeOfLeads.Cells[5, 1];
        //                                chartSheetDate2 = xlWorkSheetChartAverageAgeOfLeads.Cells[5, 14];
        //                                chartSheetDateCellRange = (Excel.Range)xlWorkSheetChartAverageAgeOfLeads.get_Range(chartSheetDate1, chartSheetDate2);
        //                                chartSheetDateCellRange.Font.Bold = true;
        //                                chartSheetDateCellRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
        //                                chartSheetDateCellRange.Merge(true);

        //                                xlWorkSheetChartAverageAgeOfLeads.Cells[5, 1] = string.Format("Date Generated: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        //                                #endregion Adding the report date

        //                                Excel.ChartObjects xlChartsAverageAgeOfLeads = (Excel.ChartObjects)xlWorkSheetChartAverageAgeOfLeads.ChartObjects(Type.Missing);
        //                                Excel.ChartObject myChartAverageAgeOfLeads = (Excel.ChartObject)xlChartsAverageAgeOfLeads.Add(2720, 600, 670, 400);
        //                                Excel.Chart chartPageAverageAgeOfLeads = myChartAverageAgeOfLeads.Chart;
        //                                chartPageAverageAgeOfLeads.HasTitle = true;
        //                                chartPageAverageAgeOfLeads.ChartTitle.Text = "Average Age of the Leads(Days)";

        //                                var yAxisAverageAgeOfLeads = (Excel.Axis)chartPageAverageAgeOfLeads.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
        //                                yAxisAverageAgeOfLeads.HasTitle = true;
        //                                yAxisAverageAgeOfLeads.AxisTitle.Text = "Age";

        //                                Excel.SeriesCollection oSeriesCollectionAverageAgeOfLeads = (Excel.SeriesCollection)myChartAverageAgeOfLeads.Chart.SeriesCollection(misValue);
        //                                Excel.Series series1AverageAgeOfLeads = oSeriesCollectionAverageAgeOfLeads.NewSeries();

        //                                Excel.Range series1AverageAgeOfLeads_range = xlWorkSheet[t].get_Range("aa" + formulaFromRow, "aa" + formulaToRow);
        //                                series1AverageAgeOfLeads.Values = series1AverageAgeOfLeads_range;
        //                                series1AverageAgeOfLeads.Name = "Average Age of the Leads";

        //                                series1AverageAgeOfLeads.XValues = xlWorkSheet[t].get_Range("a" + formulaFromRow, "a" + formulaToRow);
        //                                chartPageAverageAgeOfLeads.ChartType = Excel.XlChartType.xlLine;

        //                                #endregion Average Age of Leads

        //                                #region Average Days Since Lead Generation 

        //                                Excel.Worksheet xlWorkSheetChartAvgDaysSinceLeadGeneration;
        //                                xlWorkSheetChartAvgDaysSinceLeadGeneration = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(t);
        //                                xlWorkSheetChartAvgDaysSinceLeadGeneration.Name = codeCampaign;

        //                                #region Adding the report heading

        //                                chartSheetHeading1 = xlWorkSheetChartAvgDaysSinceLeadGeneration.Cells[1, 1];
        //                                chartSheetHeading2 = xlWorkSheetChartAvgDaysSinceLeadGeneration.Cells[1, 14];
        //                                chartSheetHeadingRange = (Excel.Range)xlWorkSheetChartAvgDaysSinceLeadGeneration.get_Range(chartSheetHeading1, chartSheetHeading2);

        //                                chartSheetHeadingRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //                                chartSheetHeadingRange.Font.Bold = true;
        //                                chartSheetHeadingRange.Font.Size = 16;
        //                                chartSheetHeadingRange.Borders.Weight = Excel.XlBorderWeight.xlThick;
        //                                chartSheetHeadingRange.Merge(true);
        //                                xlWorkSheetChartAvgDaysSinceLeadGeneration.Cells[1, 1] = string.Format("Batch Analysis Report - {0}", codeCampaign);

        //                                #endregion Adding the report heading

        //                                #region Adding the report subheading

        //                                chartSheetSubHeading1 = xlWorkSheetChartAvgDaysSinceLeadGeneration.Cells[3, 1];
        //                                chartSheetSubHeading2 = xlWorkSheetChartAvgDaysSinceLeadGeneration.Cells[3, 14];
        //                                chartSheetSubHeadingRange = (Excel.Range)xlWorkSheetChartAvgDaysSinceLeadGeneration.get_Range(chartSheetSubHeading1, chartSheetSubHeading2);
        //                                chartSheetSubHeadingRange.Merge(true);
        //                                chartSheetSubHeadingRange.Font.Bold = true;
        //                                chartSheetSubHeadingRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

        //                                xlWorkSheetChartAvgDaysSinceLeadGeneration.Cells[3, 1] = string.Format("For the period between {0} and {1}", _startDate.ToString("d MMMM yyyy"), _endDate.ToString("d MMMM yyyy"));

        //                                #endregion Adding the report subheading

        //                                #region Adding the report date

        //                                chartSheetDate1 = xlWorkSheetChartAvgDaysSinceLeadGeneration.Cells[5, 1];
        //                                chartSheetDate2 = xlWorkSheetChartAvgDaysSinceLeadGeneration.Cells[5, 14];
        //                                chartSheetDateCellRange = (Excel.Range)xlWorkSheetChartAvgDaysSinceLeadGeneration.get_Range(chartSheetDate1, chartSheetDate2);
        //                                chartSheetDateCellRange.Font.Bold = true;
        //                                chartSheetDateCellRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
        //                                chartSheetDateCellRange.Merge(true);

        //                                xlWorkSheetChartAvgDaysSinceLeadGeneration.Cells[5, 1] = string.Format("Date Generated: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        //                                #endregion Adding the report date

        //                                Excel.ChartObjects xlChartsAvgDaysSinceLeadGeneration = (Excel.ChartObjects)xlWorkSheetChartAvgDaysSinceLeadGeneration.ChartObjects(Type.Missing);
        //                                Excel.ChartObject myChartAvgDaysSinceLeadGeneration = (Excel.ChartObject)xlChartsAvgDaysSinceLeadGeneration.Add(3400, 600, 670, 400);
        //                                Excel.Chart chartPageAvgDaysSinceLeadGeneration = myChartAvgDaysSinceLeadGeneration.Chart;
        //                                chartPageAvgDaysSinceLeadGeneration.HasTitle = true;
        //                                chartPageAvgDaysSinceLeadGeneration.ChartTitle.Text = "Avg days since lead generation";

        //                                var yAxisAvgDaysSinceLeadGeneration = (Excel.Axis)chartPageAvgDaysSinceLeadGeneration.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
        //                                yAxisAvgDaysSinceLeadGeneration.HasTitle = true;
        //                                yAxisAvgDaysSinceLeadGeneration.AxisTitle.Text = "Days";

        //                                Excel.SeriesCollection oSeriesCollectionAvgDaysSinceLeadGeneration = (Excel.SeriesCollection)myChartAvgDaysSinceLeadGeneration.Chart.SeriesCollection(misValue);
        //                                Excel.Series series1AvgDaysSinceLeadGeneration = oSeriesCollectionAvgDaysSinceLeadGeneration.NewSeries();

        //                                Excel.Range series1AvgDaysSinceLeadGeneration_range = xlWorkSheet[t].get_Range("AB" + formulaFromRow, "AB" + formulaToRow);
        //                                series1AvgDaysSinceLeadGeneration.Values = series1AvgDaysSinceLeadGeneration_range;
        //                                series1AvgDaysSinceLeadGeneration.Name = "Avg days since lead generation";

        //                                series1AvgDaysSinceLeadGeneration.XValues = xlWorkSheet[t].get_Range("A" + formulaFromRow, "A" + formulaToRow);
        //                                chartPageAvgDaysSinceLeadGeneration.ChartType = Excel.XlChartType.xlLine;

        //                                #endregion Average Days Since Lead Generation

        //                                #endregion Charts



        //                            }



        //                            #endregion



        //                            xlWorkSheet[t].Name = codeCampaign;

        //                             t++;



        //                        }


        //                        }


        //                #region Display Excel Document

        //                //GlobalSettings.UserFolder, campaignName,

        //                //string filePathAndName = GlobalSettings.UserFolder + campaignName + " Incoming Batch Analysis  Report ~ " + DateTime.Now.Millisecond + ".xls";
        //                string filePathAndName = string.Format("{0} Incoming Batch Analysis Report ~ {1}.xls", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));


        //                //Save excel document
        //                //  wbReport.Save(filePathAndName);
        //                xlWorkBook.SaveAs(filePathAndName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

        //                xlWorkBook.Close(true, misValue, misValue);

        //                xlApp.Quit();

        //                xlWorkBook = null;

        //                //Display excel document+
        //                Process.Start(filePathAndName);




        //                #endregion

        //            }




        //        }



        //    }







        //}
        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {

                this.Dispatcher.Invoke(() =>
                {
                    SetCursor(Cursors.Wait);

                    IEnumerable<DataRecord> campaigns = e.Argument as IEnumerable<DataRecord>;

                    //Excel.Application xlApp = new Excel.Application();

                    //Excel.Workbook Workbook;

                    //int worksheetCount = 0;

                    //Setup excel document


                    //if (MergeReportCheckBox.IsChecked == true)
                    //{
                    //    if (campaigns != null)
                    //    {
                    //        foreach (DataRecord drWorksheets in campaigns)
                    //        {


                    //            //long campaignID = Convert.ToInt32(drWorksheets.Cells["CampaignID"].Value);
                    //            //string campaignName = drWorksheets.Cells["CampaignName"].Value.ToString();
                    //            //string campaignCode = drWorksheets.Cells["CampaignCode"].Value.ToString();

                    //            AddWorksheetsToWorkbook(campaigns);


                    //        }


                    //    }
                    //}


                    //if (MergeReportCheckBox.IsChecked == false)
                    //{
                    SeparateExcelFiles(campaigns);
                    //}




                });



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

        #endregion ReportOLD(object sender, DoWorkEventArgs e)

        #endregion


        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;
            btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnReport.ToolTip = btnReport.Content;
        }





        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IEnumerable<DataRecord> campaigns = xdgCampaigns.Records.Cast<DataRecord>().ToArray();

                btnClose.IsEnabled = false;
                btnReport.IsEnabled = false;
                xdgCampaigns.IsEnabled = false;
                calStartDate.IsEnabled = false;
                calEndDate.IsEnabled = false;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += Report;
                worker.RunWorkerCompleted += ReportCompleted;
                worker.RunWorkerAsync(campaigns);

                dispatcherTimer1.Start();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Data.DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;
                btnReport.IsEnabled = true;
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = true;
                }

                EnableDisableExportButton();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Data.DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;
                btnReport.IsEnabled = true;
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = false;
                }

                EnableDisableExportButton();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_xdgHeaderPrefixAreaCheckbox != null)
                {
                    _xdgHeaderPrefixAreaCheckbox.IsChecked = AllRecordsSelected();
                }

                EnableDisableExportButton();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _xdgHeaderPrefixAreaCheckbox = (System.Windows.Controls.CheckBox)sender;
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

        private void MergeReportCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }


        private void MergeReportCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void MergeReportCheckBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void LoadLookups()
        {
            System.Data.DataTable dtBatchReportTypes = Business.Insure.INGetBatchAnalysisReportTypes();
            cmbReportType.Populate(dtBatchReportTypes, "ReportType", "ID");

        }

        private void SetFormMode(byte selectedReportType)
        {

            try
            {

                System.Data.DataTable dtReportTypeCampaigns = Business.Insure.INGetBatchAnalysisReportCampaignsOrCampaignTypesByReportType(selectedReportType);

                xdgCampaigns.DataSource = dtReportTypeCampaigns.DefaultView;

                _reportTypeID = selectedReportType;

                switch (selectedReportType)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        {
                            xdgCampaigns.IsEnabled = true;

                            //lblSalesConversion.Visibility = Visibility.Visible;
                            //chkSalesConversion.Visibility = Visibility.Visible;

                            //lblContactsConversion.Visibility = Visibility.Visible;
                            //chkContactsConversion.Visibility = Visibility.Visible;

                            //lblCal1.Visibility = Visibility.Visible;
                            //calStartDate.Visibility = Visibility.Visible;
                            //calEndDate.Visibility = Visibility.Visible;
                        }

                        break;

                    //case 2:
                    //    {
                    //        xdgCampaigns.IsEnabled = true;
                    //        //HeaderPrefixAreaCheckbox_Checked(xdgCampaigns, new RoutedEventArgs());


                    //        //lblSalesConversion.Visibility = Visibility.Collapsed;
                    //        //chkSalesConversion.Visibility = Visibility.Collapsed;
                    //        //lblContactsConversion.Visibility = Visibility.Collapsed;
                    //        //chkContactsConversion.Visibility = Visibility.Collapsed;
                    //        //RData.SalesConversionPerBatch = false;
                    //        //RData.ContactsConversionPerBatch = false;

                    //        //lblCal1.Visibility = Visibility.Collapsed;
                    //        //calStartDate.Visibility = Visibility.Collapsed;
                    //        //calEndDate.Visibility = Visibility.Collapsed;

                    //        //Have to set the display mode for the calendar like this because there is a bug in the windows calendar
                    //        //that displays the calendar funny when using a datacontext and setting the calendar mode to year before
                    //        //setting the data context.

                    //        //System.Action act = delegate ()
                    //        //{
                    //        //    //_calendar.SelectedDate = ((ViewModel)DataContext).Display;
                    //        //    clYear.DisplayMode = CalendarMode.Year;
                    //        //    //_calendar.SelectedDate = null;
                    //        //};
                    //        //Dispatcher.BeginInvoke(act, DispatcherPriority.ApplicationIdle);

                    //    }

                    //    break;

                    //case 3:
                    //    {
                    //        xdgCampaigns.IsEnabled = true;


                    //        //lblSalesConversion.Visibility = Visibility.Collapsed;
                    //        //chkSalesConversion.Visibility = Visibility.Collapsed;
                    //        //lblContactsConversion.Visibility = Visibility.Collapsed;
                    //        //chkContactsConversion.Visibility = Visibility.Collapsed;
                    //        //RData.SalesConversionPerBatch = false;
                    //        //RData.ContactsConversionPerBatch = false;


                    //        //lblCal1.Visibility = Visibility.Collapsed;
                    //        //calStartDate.Visibility = Visibility.Collapsed;
                    //        //calEndDate.Visibility = Visibility.Collapsed;
                    //    }

                    //    break;
                }

                //xdgCampaigns.DataSource = null;
            }
            catch (Exception ex) 
            {
                HandleException(ex);
            }

        }

        #endregion

        private void cmbReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbReportType.SelectedValue != null)
                {
                    SetFormMode(Convert.ToByte(cmbReportType.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}
