using System.Data.SqlClient;
using System.Windows.Resources;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
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
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using Orientation = Infragistics.Documents.Excel.Orientation;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportBumpUpScreen.xaml
    /// </summary>
    public partial class ReportIndividualFallOffScreen
    {

        #region Constants

        //private string _fontName = "Calibri";
        //private const int _fontSize = 10;
        //private const int _pointsToTwipsFactor = 20;
        //private const int _fontHeight = _fontSize * _pointsToTwipsFactor;

        #endregion



        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        //private RecordCollectionBase _campaigns;
        private DateTime _startDate;
        private DateTime _endDate;
        private int _year;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        #endregion



        #region Constructors

        public ReportIndividualFallOffScreen()
        {
            InitializeComponent();
            LoadCampaignInfo();

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

                DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign");
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

      

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";

            btnReport.IsEnabled = true;
            btnClose.IsEnabled = true;
            xdgCampaigns.IsEnabled = true;
            //calStartDate.IsEnabled = true;
            //calEndDate.IsEnabled = true;
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                IEnumerable<DataRecord> campaigns = e.Argument as IEnumerable<DataRecord>;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                string fileName = string.Empty;
                if (campaigns != null)                    
                    foreach (DataRecord record in campaigns)
                    {
                        if ((bool)record.Cells["Select"].Value)
                        {
                            long campaignID = Convert.ToInt32(record.Cells["CampaignID"].Value);
                            string campaignName = record.Cells["CampaignName"].Value.ToString();
                            string campaignCode = record.Cells["CampaignCode"].Value.ToString();

                            #region Setup excel documents

                            Workbook wbTemplate;
                            //Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                            string filePathAndName = GlobalSettings.UserFolder + campaignName + " Individual Fall Off  Report ~ " + DateTime.Now.Millisecond + ".xlsx";

                            Uri uri = new Uri("/Templates/ReportTemplateIndividualFallOff.xlsx", UriKind.Relative);
                            StreamResourceInfo info = Application.GetResourceStream(uri);
                            if (info != null)
                            {
                                wbTemplate = Workbook.Load(info.Stream, true);
                            }
                            else
                            {
                                return;
                            }

                            Worksheet wsTemplate = wbTemplate.Worksheets["Campaign"];
                            

                            #endregion

                            #region Get report data from database

                            DataTable dtLeadAllocationData;

                            SqlParameter[] parameters = new SqlParameter[3];

                           DateTime fromDate = new DateTime(_year, 1, 1);
                           DateTime toDate = new DateTime(_year, 12, 31);
                            parameters[0] = new SqlParameter("@CampaignId", campaignID);
                            parameters[1] = new SqlParameter("@FromDate", fromDate.ToString("yyyy-MM-dd"));
                            parameters[2] = new SqlParameter("@ToDate", toDate.ToString("yyyy-MM-dd"));

                            DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spINReportIndividualFallOff", parameters);
                            if (dsLeadAllocationData.Tables.Count > 0)
                            {
                                dtLeadAllocationData = dsLeadAllocationData.Tables[0];

                                if (dtLeadAllocationData.Rows.Count == 0)
                                {
                                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                                    {
                                        ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + campaignName + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
                                    });

                                    continue;
                                }
                            }
                            else
                            {
                                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                                {
                                    ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + campaignName + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
                                });

                                continue;
                            }

                            #endregion

                            Worksheet wsReport = wbReport.Worksheets.Add(campaignCode);
                            wsReport.PrintOptions.PaperSize = PaperSize.A4;
                            wsReport.PrintOptions.Orientation = Orientation.Portrait;

                            Methods.CopyExcelRegion(wsTemplate, 0, 0, 1, 50, wsReport, 0, 0);

                            #region report data

                            {
                                int rowIndex = 3;
                                //string previousValue = string.Empty;
                                List<string> agentsList = new List<string>();
                                foreach (DataRow rw in dtLeadAllocationData.Rows)
                                {
                                    string AgentName = rw["AgentName"].ToString();
                                    if (!agentsList.Contains(AgentName))
                                    {
                                        agentsList.Add(AgentName);
                                    }
                                   
                                        
                                }
                                decimal totalPreConfirmationJanuary = 0;
                                decimal totalPreConfirmationFebruary = 0;
                                decimal totalPreConfirmationMarch = 0;
                                decimal totalPreConfirmationApril = 0;
                                decimal totalPreConfirmationMay = 0;
                                decimal totalPreConfirmationJune = 0;
                                decimal totalPreConfirmationJuly = 0;
                                decimal totalPreConfirmationAugust = 0;
                                decimal totalPreConfirmationSeptember = 0;
                                decimal totalPreConfirmationOctober = 0;
                                decimal totalPreConfirmationNovember = 0;
                                decimal totalPreConfirmationDecember = 0;

                                decimal totalPostConfirmationJanuary = 0;
                                decimal totalPostConfirmationFebruary = 0;
                                decimal totalPostConfirmationMarch = 0;
                                decimal totalPostConfirmationApril = 0;
                                decimal totalPostConfirmationMay = 0;
                                decimal totalPostConfirmationJune = 0;
                                decimal totalPostConfirmationJuly = 0;
                                decimal totalPostConfirmationAugust = 0;
                                decimal totalPostConfirmationSeptember = 0;
                                decimal totalPostConfirmationOctober = 0;
                                decimal totalPostConfirmationNovember = 0;
                                decimal totalPostConfirmationDecember = 0;

                                decimal totalFallOffJanuary = 0;
                                decimal totalFallOffFebruary = 0;
                                decimal totalFallOffMarch = 0;
                                decimal totalFallOffApril = 0;
                                decimal totalFallOffMay = 0;
                                decimal totalFallOffJune = 0;
                                decimal totalFallOffJuly = 0;
                                decimal totalFallOffAugust = 0;
                                decimal totalFallOffSeptember = 0;
                                decimal totalFallOffOctober = 0;
                                decimal totalFallOffNovember = 0;
                                decimal totalFallOffDecember = 0;

                                decimal totalFallOffPercentageJanuary = 0;
                                decimal totalFallOffPercentageFebruary = 0;
                                decimal totalFallOffPercentageMarch = 0;
                                decimal totalFallOffPercentageApril = 0;
                                decimal totalFallOffPercentageMay = 0;
                                decimal totalFallOffPercentageJune = 0;
                                decimal totalFallOffPercentageJuly = 0;
                                decimal totalFallOffPercentageAugust = 0;
                                decimal totalFallOffPercentageSeptember = 0;
                                decimal totalFallOffPercentageOctober = 0;
                                decimal totalFallOffPercentageNovember = 0;
                                decimal totalFallOffPercentageDecember = 0;
                                foreach (string agent in agentsList)
                                {
                                    //get falls offs for agent
                                    List<string>fallOfStatuses = new List<string>();
                                    fallOfStatuses.Add("Cancelled");
                                    fallOfStatuses.Add("Carried Forward");
                                    var rows = dtLeadAllocationData.AsEnumerable().Where(x => ((string) x["AgentName"] == agent));
                                   
                                  decimal fallOffCountJanuary = 0;
                                  decimal fallOffCountFebruary = 0;
                                  decimal fallOffCountMarch = 0;
                                  decimal fallOffCountApril = 0;
                                  decimal fallOffCountMay = 0;
                                  decimal fallOffCountJune = 0;
                                  decimal fallOffCountJuly = 0;
                                  decimal fallOffCountAugust = 0;
                                  decimal fallOffCountSeptember = 0;
                                  decimal fallOffCountOctober = 0;
                                  decimal fallOffCountNovember = 0;
                                  decimal fallOffCountDecember = 0;
                                  int totalFallOff = 0;
                                  decimal saleCountJanuary = 0;
                                  decimal saleCountFebruary = 0;
                                  decimal saleCountMarch = 0;
                                  decimal saleCountApril = 0;
                                  decimal saleCountMay = 0;
                                  decimal saleCountJune = 0;
                                  decimal saleCountJuly = 0;
                                  decimal saleCountAugust = 0;
                                  decimal saleCountSeptember = 0;
                                  decimal saleCountOctober = 0;
                                  decimal saleCountNovember = 0;
                                  decimal saleCountDecember = 0;
                                  int totalSales = 0;
                                  foreach (DataRow row in rows)
                                  {
                                      DateTime saleDate = DateTime.Parse(row["SaleDate"].ToString());
                                      if (row["Status"].ToString() == "Cancelled" || row["Status"].ToString() == "Carried Forward")  
                                      {                                        
                                          //get the month
                                          switch (saleDate.Month)
                                          {
                                              case 1:
                                                  fallOffCountJanuary++;
                                                  totalFallOff++;
                                                  break;
                                              case 2:
                                                  fallOffCountFebruary++;
                                                  totalFallOff++;
                                                  break;
                                              case 3:
                                                  fallOffCountMarch++;
                                                  totalFallOff++;
                                                  break;
                                              case 4:
                                                  fallOffCountApril++;
                                                  totalFallOff++;
                                                  break;
                                              case 5:
                                                  fallOffCountMay++;
                                                  totalFallOff++;
                                                  break;
                                              case 6:
                                                  fallOffCountJune++;
                                                  totalFallOff++;
                                                  break;
                                              case 7:
                                                  fallOffCountJuly++;
                                                  totalFallOff++;
                                                  break;
                                              case 8:
                                                  fallOffCountAugust++;
                                                  totalFallOff++;
                                                  break;
                                              case 9:
                                                  fallOffCountSeptember++;
                                                  totalFallOff++;
                                                  break;
                                              case 10:
                                                  fallOffCountOctober++;
                                                  totalFallOff++;
                                                  break;
                                              case 11:
                                                  fallOffCountNovember++;
                                                  totalFallOff++;
                                                  break;
                                              case 12:
                                                  fallOffCountDecember++;
                                                  totalFallOff++;
                                                  break;
                                          }
                                      }
                                      else if(row["Status"].ToString() == "Sale")
                                      {
                                          //for all Sales

                                          switch (saleDate.Month)
                                          {
                                              case 1:
                                                  saleCountJanuary++;
                                                  totalSales++;
                                                  break;
                                              case 2:
                                                  saleCountFebruary++;
                                                  totalSales++;
                                                  break;
                                              case 3:
                                                  saleCountMarch++;
                                                  totalSales++;
                                                  break;
                                              case 4:
                                                  saleCountApril++;
                                                  totalSales++;
                                                  break;
                                              case 5:
                                                  saleCountMay++;
                                                  totalSales++;
                                                  break;
                                              case 6:
                                                  saleCountJune++;
                                                  totalSales++;
                                                  break;
                                              case 7:
                                                  saleCountJuly++;
                                                  totalSales++;
                                                  break;
                                              case 8:
                                                  saleCountAugust++;
                                                  totalSales++;
                                                  break;
                                              case 9:
                                                  saleCountSeptember++;
                                                  totalSales++;
                                                  break;
                                              case 10:
                                                  saleCountOctober++;
                                                  totalSales++;
                                                  break;
                                              case 11:
                                                  saleCountNovember++;
                                                  totalSales++;
                                                  break;
                                              case 12:
                                                  saleCountDecember++;
                                                  totalSales++;
                                                  break;
                                          }
                                      }
                                     
                                  }
                                  wsReport.MergedCellsRegions[2].Worksheet.Rows[0].SetCellValue(1, "January " +_year);
                                  wsReport.MergedCellsRegions[2].Worksheet.Rows[0].SetCellValue(5, "February " + _year);
                                  wsReport.MergedCellsRegions[2].Worksheet.Rows[0].SetCellValue(9, "March " + _year);
                                  wsReport.MergedCellsRegions[2].Worksheet.Rows[0].SetCellValue(13, "April " + _year);
                                  wsReport.MergedCellsRegions[2].Worksheet.Rows[0].SetCellValue(17, "May " + _year);
                                  wsReport.MergedCellsRegions[2].Worksheet.Rows[0].SetCellValue(21, "June " + _year);
                                  wsReport.MergedCellsRegions[2].Worksheet.Rows[0].SetCellValue(25, "July " + _year);
                                  wsReport.MergedCellsRegions[2].Worksheet.Rows[0].SetCellValue(29, "August " + _year);
                                  wsReport.MergedCellsRegions[2].Worksheet.Rows[0].SetCellValue(33, "September " + _year);
                                  wsReport.MergedCellsRegions[2].Worksheet.Rows[0].SetCellValue(37, "October " + _year);
                                  wsReport.MergedCellsRegions[2].Worksheet.Rows[0].SetCellValue(41, "November " + _year);
                                  wsReport.MergedCellsRegions[2].Worksheet.Rows[0].SetCellValue(45, "December " + _year);
                                    Methods.CopyExcelRegion(wsTemplate, rowIndex - 1, 0, 1, 50, wsReport, rowIndex - 1, 0);
                                   
                                    wsReport.GetCell("A" + rowIndex.ToString()).Value = agent;
                               //fall off
                                    wsReport.GetCell("D" + rowIndex.ToString()).Value = fallOffCountJanuary;                                   
                                    wsReport.GetCell("H" + rowIndex.ToString()).Value = fallOffCountFebruary;
                                    wsReport.GetCell("L" + rowIndex.ToString()).Value = fallOffCountMarch;
                                    wsReport.GetCell("P" + rowIndex.ToString()).Value = fallOffCountApril;
                                    wsReport.GetCell("T" + rowIndex.ToString()).Value = fallOffCountMay;
                                    wsReport.GetCell("X" + rowIndex.ToString()).Value = fallOffCountJune;
                                    wsReport.GetCell("AB" + rowIndex.ToString()).Value = fallOffCountJuly;
                                    wsReport.GetCell("AF" + rowIndex.ToString()).Value = fallOffCountAugust;
                                    wsReport.GetCell("AJ" + rowIndex.ToString()).Value = fallOffCountSeptember;
                                    wsReport.GetCell("AN" + rowIndex.ToString()).Value = fallOffCountOctober;
                                    wsReport.GetCell("AR" + rowIndex.ToString()).Value = fallOffCountNovember;
                                    wsReport.GetCell("AV" + rowIndex.ToString()).Value = fallOffCountDecember;
                                    //fall off percentages

                                    if (saleCountJanuary > 0)
                                    {
                                        wsReport.GetCell("E" + rowIndex.ToString()).Value = Math.Round(fallOffCountJanuary / (saleCountJanuary + fallOffCountJanuary) * 100, 2) + " %";
                                        totalFallOffPercentageJanuary = totalFallOffPercentageJanuary + Math.Round(fallOffCountJanuary / saleCountJanuary * 100, 2);
                                    }
                                    if (saleCountFebruary > 0)
                                    {
                                        wsReport.GetCell("I" + rowIndex.ToString()).Value = Math.Round(fallOffCountFebruary / (saleCountFebruary + fallOffCountFebruary) * 100, 2) + " %";
                                        totalFallOffPercentageFebruary = totalFallOffPercentageFebruary + Math.Round(fallOffCountFebruary / saleCountFebruary * 100, 2);
                                    }
                                    if (saleCountMarch > 0)
                                    {
                                        wsReport.GetCell("M" + rowIndex.ToString()).Value = Math.Round(fallOffCountMarch / (saleCountMarch + fallOffCountMarch) * 100, 2) + " %";
                                        totalFallOffPercentageMarch = totalFallOffPercentageMarch + Math.Round(fallOffCountMarch / saleCountMarch * 100, 2);
                                    }
                                    if (saleCountApril > 0)
                                    {
                                        wsReport.GetCell("Q" + rowIndex.ToString()).Value = Math.Round(fallOffCountApril / (saleCountApril + fallOffCountApril) * 100, 2) + " %";
                                        totalFallOffPercentageApril = totalFallOffPercentageApril + Math.Round(fallOffCountApril / saleCountApril * 100, 2);
                                    }
                                    if (saleCountMay > 0)
                                    {
                                        wsReport.GetCell("U" + rowIndex.ToString()).Value = Math.Round(fallOffCountMay / (saleCountMay + fallOffCountMay) * 100, 2) + " %";
                                        totalFallOffPercentageMay = totalFallOffPercentageMay + Math.Round(fallOffCountMay / saleCountMay * 100, 2);
                                    }
                                    if (saleCountJune > 0)
                                    {
                                        wsReport.GetCell("Y" + rowIndex.ToString()).Value = Math.Round(fallOffCountJune / (saleCountJune + fallOffCountJune) * 100, 2) + " %";
                                        totalFallOffPercentageJune = totalFallOffPercentageJune + Math.Round(fallOffCountJune / saleCountJune * 100, 2);
                                    }
                                    if (saleCountJuly > 0)
                                    {
                                        wsReport.GetCell("AC" + rowIndex.ToString()).Value = Math.Round(fallOffCountJuly / (saleCountJuly + fallOffCountJuly) * 100, 2) + " %";
                                        totalFallOffPercentageJuly = totalFallOffPercentageJuly + Math.Round(fallOffCountJuly / saleCountJuly * 100, 2);
                                    }
                                    if (saleCountAugust > 0)
                                    {
                                        wsReport.GetCell("AG" + rowIndex.ToString()).Value = Math.Round(fallOffCountAugust / (saleCountAugust + fallOffCountAugust) * 100, 2) + " %";
                                        totalFallOffPercentageAugust = totalFallOffPercentageAugust + Math.Round(fallOffCountAugust / saleCountAugust * 100, 2);
                                    }
                                    if (saleCountSeptember > 0)
                                    {
                                        wsReport.GetCell("AK" + rowIndex.ToString()).Value = Math.Round(fallOffCountSeptember / (saleCountSeptember + fallOffCountSeptember) * 100, 2) + " %";
                                        totalFallOffPercentageSeptember = totalFallOffPercentageSeptember + Math.Round(fallOffCountSeptember / saleCountSeptember * 100, 2);
                                    }
                                    if (saleCountOctober > 0)
                                    {
                                        wsReport.GetCell("AO" + rowIndex.ToString()).Value = Math.Round(fallOffCountOctober / (saleCountOctober + fallOffCountOctober) * 100, 2) + " %";
                                        totalFallOffPercentageOctober = totalFallOffPercentageOctober + Math.Round(fallOffCountOctober / saleCountOctober * 100, 2);
                                    }
                                    if (saleCountNovember > 0)
                                    {
                                        wsReport.GetCell("AS" + rowIndex.ToString()).Value = Math.Round(fallOffCountNovember / (saleCountNovember + fallOffCountNovember) * 100, 2) + " %";
                                        totalFallOffPercentageNovember = totalFallOffPercentageNovember + Math.Round(fallOffCountNovember / saleCountNovember * 100, 2);
                                    }
                                    if (saleCountDecember > 0)
                                    {
                                        wsReport.GetCell("AW" + rowIndex.ToString()).Value = Math.Round(fallOffCountDecember / (saleCountDecember + fallOffCountDecember) * 100, 2) + " %";
                                        totalFallOffPercentageDecember = totalFallOffPercentageDecember + Math.Round(fallOffCountDecember / saleCountDecember * 100, 2);
                                    }



                                    wsReport.GetCell("B" + rowIndex.ToString()).Value = saleCountJanuary + fallOffCountJanuary;
                                        wsReport.GetCell("F" + rowIndex.ToString()).Value = saleCountFebruary + fallOffCountFebruary;
                                        wsReport.GetCell("J" + rowIndex.ToString()).Value = saleCountMarch + fallOffCountMarch;
                                        wsReport.GetCell("N" + rowIndex.ToString()).Value = saleCountApril + fallOffCountApril;
                                        wsReport.GetCell("R" + rowIndex.ToString()).Value = saleCountMay + fallOffCountMay;
                                        wsReport.GetCell("V" + rowIndex.ToString()).Value = saleCountJune+  fallOffCountJune;
                                        wsReport.GetCell("Z" + rowIndex.ToString()).Value = saleCountJuly + fallOffCountJuly;
                                        wsReport.GetCell("AD" + rowIndex.ToString()).Value = saleCountAugust + fallOffCountAugust;
                                        wsReport.GetCell("AH" + rowIndex.ToString()).Value = saleCountSeptember + fallOffCountSeptember;
                                        wsReport.GetCell("AL" + rowIndex.ToString()).Value = saleCountOctober + fallOffCountOctober ;
                                        wsReport.GetCell("AP" + rowIndex.ToString()).Value = saleCountNovember + fallOffCountNovember;
                                        wsReport.GetCell("AT" + rowIndex.ToString()).Value = saleCountDecember + fallOffCountDecember; 

                                        wsReport.GetCell("C" + rowIndex.ToString()).Value = saleCountJanuary;
                                        wsReport.GetCell("G" + rowIndex.ToString()).Value = saleCountFebruary;
                                        wsReport.GetCell("K" + rowIndex.ToString()).Value = saleCountMarch;
                                        wsReport.GetCell("O" + rowIndex.ToString()).Value = saleCountApril;
                                        wsReport.GetCell("S" + rowIndex.ToString()).Value = saleCountMay;
                                        wsReport.GetCell("W" + rowIndex.ToString()).Value = saleCountJune;
                                        wsReport.GetCell("AA" + rowIndex.ToString()).Value = saleCountJuly;
                                        //wsReport.GetCell("AD" + rowIndex.ToString()).Value = saleCountAugust - fallOffCountAugust;
                                        wsReport.GetCell("AE" + rowIndex.ToString()).Value = saleCountAugust;
                                        wsReport.GetCell("AI" + rowIndex.ToString()).Value = saleCountSeptember;
                                        wsReport.GetCell("AM" + rowIndex.ToString()).Value = saleCountOctober;
                                        wsReport.GetCell("AQ" + rowIndex.ToString()).Value = saleCountNovember;
                                        wsReport.GetCell("AU" + rowIndex.ToString()).Value = saleCountDecember;

                                        totalPreConfirmationJanuary = totalPreConfirmationJanuary + saleCountJanuary;
                                        totalPreConfirmationFebruary = totalPreConfirmationFebruary + saleCountFebruary;
                                        totalPreConfirmationMarch = totalPreConfirmationMarch + saleCountMarch;
                                        totalPreConfirmationApril = totalPreConfirmationApril + saleCountApril;
                                        totalPreConfirmationMay = totalPreConfirmationMay + saleCountMay;
                                        totalPreConfirmationJune = totalPreConfirmationJune + saleCountJune;
                                        totalPreConfirmationJuly = totalPreConfirmationJuly + saleCountJuly;
                                        totalPreConfirmationAugust = totalPreConfirmationAugust + saleCountAugust;
                                        totalPreConfirmationSeptember = totalPreConfirmationSeptember + saleCountSeptember;
                                        totalPreConfirmationOctober = totalPreConfirmationOctober + saleCountOctober;
                                        totalPreConfirmationNovember = totalPreConfirmationNovember + saleCountNovember;
                                        totalPreConfirmationDecember = totalPreConfirmationDecember + saleCountDecember;

                                        totalPostConfirmationJanuary = totalPostConfirmationJanuary + (saleCountJanuary - fallOffCountJanuary);
                                        totalPostConfirmationFebruary = totalPostConfirmationFebruary + (saleCountFebruary - fallOffCountFebruary);
                                        totalPostConfirmationMarch = totalPostConfirmationMarch + (saleCountMarch - fallOffCountMarch);
                                        totalPostConfirmationApril = totalPostConfirmationApril + (saleCountApril - fallOffCountApril);
                                        totalPostConfirmationMay = totalPostConfirmationMay + (saleCountMay - fallOffCountMay);
                                        totalPostConfirmationJune = totalPostConfirmationJune + (saleCountJune - fallOffCountJune);
                                        totalPostConfirmationJuly = totalPostConfirmationJuly + (saleCountJuly - fallOffCountJuly);
                                        totalPostConfirmationAugust = totalPostConfirmationAugust + (saleCountAugust - fallOffCountAugust);
                                        totalPostConfirmationSeptember = totalPostConfirmationSeptember + (saleCountSeptember - fallOffCountSeptember);
                                        totalPostConfirmationOctober = totalPostConfirmationOctober + (saleCountOctober - fallOffCountOctober);
                                        totalPostConfirmationNovember = totalPostConfirmationNovember + (saleCountNovember - fallOffCountNovember);
                                        totalPostConfirmationDecember = totalPostConfirmationDecember + (saleCountDecember - fallOffCountDecember);

                                        totalFallOffJanuary = totalFallOffJanuary + fallOffCountJanuary;
                                        totalFallOffFebruary = totalFallOffFebruary + fallOffCountFebruary;
                                        totalFallOffMarch = totalFallOffMarch + fallOffCountMarch;
                                        totalFallOffApril = totalFallOffApril + fallOffCountApril;
                                        totalFallOffMay = totalFallOffMay + fallOffCountMay;
                                        totalFallOffJune = totalFallOffJune + fallOffCountJune;
                                        totalFallOffJuly = totalFallOffJuly + fallOffCountJuly;
                                        totalFallOffAugust = totalFallOffAugust + fallOffCountAugust;
                                        totalFallOffSeptember = totalFallOffSeptember + fallOffCountSeptember;
                                        totalFallOffOctober = totalFallOffOctober + fallOffCountOctober;
                                        totalFallOffNovember = totalFallOffNovember + fallOffCountNovember;
                                        totalFallOffDecember = totalFallOffDecember + fallOffCountDecember;
                                    rowIndex++;
                                }
#region Totals
                                wsReport.GetCell("A" + rowIndex.ToString()).Value = "TOTALS";                                
                                wsReport.GetCell("B" + rowIndex.ToString()).Value = totalPreConfirmationJanuary;
                                wsReport.GetCell("F" + rowIndex.ToString()).Value = totalPreConfirmationFebruary;
                                wsReport.GetCell("J" + rowIndex.ToString()).Value = totalPreConfirmationMarch;
                                wsReport.GetCell("N" + rowIndex.ToString()).Value = totalPreConfirmationApril;
                                wsReport.GetCell("R" + rowIndex.ToString()).Value = totalPreConfirmationMay;
                                wsReport.GetCell("V" + rowIndex.ToString()).Value = totalPreConfirmationJune;
                                wsReport.GetCell("Z" + rowIndex.ToString()).Value = totalPreConfirmationJuly;
                                wsReport.GetCell("AD" + rowIndex.ToString()).Value = totalPreConfirmationAugust;
                                wsReport.GetCell("AH" + rowIndex.ToString()).Value = totalPreConfirmationSeptember;
                                wsReport.GetCell("AL" + rowIndex.ToString()).Value = totalPreConfirmationOctober;
                                wsReport.GetCell("AP" + rowIndex.ToString()).Value = totalPreConfirmationNovember;
                                wsReport.GetCell("AT" + rowIndex.ToString()).Value = totalPreConfirmationDecember;

                                wsReport.GetCell("C" + rowIndex.ToString()).Value = totalPostConfirmationJanuary;
                                wsReport.GetCell("G" + rowIndex.ToString()).Value = totalPostConfirmationFebruary;
                                wsReport.GetCell("K" + rowIndex.ToString()).Value = totalPostConfirmationMarch;
                                wsReport.GetCell("O" + rowIndex.ToString()).Value = totalPostConfirmationApril;
                                wsReport.GetCell("S" + rowIndex.ToString()).Value = totalPostConfirmationMay;
                                wsReport.GetCell("W" + rowIndex.ToString()).Value = totalPostConfirmationJune;
                                wsReport.GetCell("AA" + rowIndex.ToString()).Value = totalPostConfirmationJuly;
                                wsReport.GetCell("AD" + rowIndex.ToString()).Value = totalPostConfirmationAugust;
                                wsReport.GetCell("AE" + rowIndex.ToString()).Value = totalPostConfirmationSeptember;
                                wsReport.GetCell("AM" + rowIndex.ToString()).Value = totalPostConfirmationOctober;
                                wsReport.GetCell("AQ" + rowIndex.ToString()).Value = totalPostConfirmationNovember;
                                wsReport.GetCell("AU" + rowIndex.ToString()).Value = totalPostConfirmationDecember;

                                wsReport.GetCell("D" + rowIndex.ToString()).Value = totalFallOffJanuary;
                                wsReport.GetCell("H" + rowIndex.ToString()).Value = totalFallOffFebruary;
                                wsReport.GetCell("L" + rowIndex.ToString()).Value = totalFallOffMarch;
                                wsReport.GetCell("P" + rowIndex.ToString()).Value = totalFallOffApril;
                                wsReport.GetCell("T" + rowIndex.ToString()).Value = totalFallOffMay;
                                wsReport.GetCell("X" + rowIndex.ToString()).Value = totalFallOffJune;
                                wsReport.GetCell("AB" + rowIndex.ToString()).Value = totalFallOffJuly;
                                wsReport.GetCell("AF" + rowIndex.ToString()).Value = totalFallOffAugust;
                                wsReport.GetCell("AJ" + rowIndex.ToString()).Value = totalFallOffSeptember;
                                wsReport.GetCell("AN" + rowIndex.ToString()).Value = totalFallOffOctober;
                                wsReport.GetCell("AR" + rowIndex.ToString()).Value = totalFallOffNovember;
                                wsReport.GetCell("AV" + rowIndex.ToString()).Value = totalFallOffDecember;

                                if (totalPreConfirmationJanuary != 0)
                                {
                                    wsReport.GetCell("E" + rowIndex.ToString()).Value = Math.Round(totalFallOffJanuary / totalPreConfirmationJanuary * 100, 2) + " %";
                                }
                                if (totalPreConfirmationFebruary != 0)
                                {
                                    wsReport.GetCell("I" + rowIndex.ToString()).Value = Math.Round(totalFallOffFebruary / totalPreConfirmationFebruary * 100, 2) + " %";
                                }
                                if (totalPreConfirmationMarch != 0)
                                {
                                    wsReport.GetCell("M" + rowIndex.ToString()).Value = Math.Round(totalFallOffMarch / totalPreConfirmationMarch  * 100, 2) + " %";
                                }
                                if (totalPreConfirmationApril != 0)
                                {
                                    wsReport.GetCell("Q" + rowIndex.ToString()).Value = Math.Round(totalFallOffApril / totalPreConfirmationApril * 100, 2) + " %";
                                }
                                if (totalPreConfirmationMay != 0)
                                {
                                    wsReport.GetCell("U" + rowIndex.ToString()).Value = Math.Round(totalFallOffMay / totalPreConfirmationMay * 100, 2) + " %";
                                }
                                if (totalPreConfirmationJune != 0)
                                {
                                    wsReport.GetCell("Y" + rowIndex.ToString()).Value = Math.Round(totalFallOffJune / totalPreConfirmationJune * 100, 2) + " %";
                                }
                                if (totalPreConfirmationJuly != 0)
                                {
                                    wsReport.GetCell("AC" + rowIndex.ToString()).Value = Math.Round(totalFallOffJuly / totalPreConfirmationJuly * 100, 2) + " %";
                                }
                                if (totalFallOffAugust != 0)
                                {
                                    wsReport.GetCell("AG" + rowIndex.ToString()).Value = Math.Round(totalFallOffAugust / totalFallOffAugust * 100, 2) + " %";
                                }
                                if (totalPreConfirmationSeptember != 0)
                                {
                                    wsReport.GetCell("AK" + rowIndex.ToString()).Value = Math.Round(totalFallOffSeptember / totalPreConfirmationSeptember * 100, 2) + " %";
                                }
                                if (totalPreConfirmationOctober != 0)
                                {
                                    wsReport.GetCell("AO" + rowIndex.ToString()).Value = Math.Round(totalFallOffOctober / totalPreConfirmationOctober * 100, 2) + " %";
                                }
                                if (totalPreConfirmationNovember != 0)
                                {
                                    wsReport.GetCell("AS" + rowIndex.ToString()).Value = Math.Round(totalFallOffNovember / totalPreConfirmationNovember * 100, 2) + " %";
                                }
                                if (totalPreConfirmationDecember != 0)
                                {
                                    wsReport.GetCell("AW" + rowIndex.ToString()).Value = Math.Round(totalFallOffDecember / totalPreConfirmationDecember * 100, 2) + " %";
                                }
#endregion Totals

                            }

                            #endregion
                            fileName = filePathAndName;
                        }
                    }
                if (fileName != string.Empty)
                {
                    string fName = GlobalSettings.UserFolder + " Individual Fall Off  Report ~ " + DateTime.Now.Millisecond + ".xlsx";
                    wbReport.Save(fName);
                    //Display excel document                           
                    Process.Start(fName);
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

        #endregion



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
                DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = true;
                }
                foreach (var item in xdgCampaigns.SelectedItems)
                {
                }
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
                DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = false;
                }

              
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
                btnReport.IsEnabled = true;
                if (_xdgHeaderPrefixAreaCheckbox != null)
                {
                    _xdgHeaderPrefixAreaCheckbox.IsChecked = AllRecordsSelected();
                }
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
                _xdgHeaderPrefixAreaCheckbox = (CheckBox)sender;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

       

      

        #endregion

        private void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _year = clYear.DisplayDate.Year;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void clYear_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            try
            {
                _year = clYear.DisplayDate.Year;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

      

    }
}
