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
using UDM.Insurance.Business;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportBumpUpScreen.xaml
    /// </summary>
    public partial class ReportStatusLoadingSummary
    {

        #region Constants

        //private string _fontName = "Calibri";
        //private const int _fontSize = 10;
        //private const int _pointsToTwipsFactor = 20;
        //private const int _fontHeight = _fontSize * _pointsToTwipsFactor;

        #endregion

        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        private List<Record> _lstSelectedCampaigns;
        private string _campaigns;
        private DateTime _startDate;
        private DateTime _endDate;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        #endregion

        #region Constructors

        public ReportStatusLoadingSummary()
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

        private void EnableDisableExportButton()
        {
            try
            {
                if ((calStartDate.SelectedDate != null && (calEndDate.SelectedDate != null))) //&& (calEndDate.SelectedDate >= Cal1.SelectedDate)
                {
                    if (_xdgHeaderPrefixAreaCheckbox != null && (_xdgHeaderPrefixAreaCheckbox.IsChecked == true || _xdgHeaderPrefixAreaCheckbox.IsChecked == null))
                    {
                        btnReport.IsEnabled = true;
                        return;
                    }
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
            xdgCampaigns.IsEnabled = true;
            calStartDate.IsEnabled = true;
            calEndDate.IsEnabled = true;
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Get data

                DataSet dsStatusLoadingSummaryReport = Insure.INGetStatusLoadingSummaryReportData(_campaigns, _startDate, _endDate);

                if (dsStatusLoadingSummaryReport.Tables[0].Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "There is no data for the selected campaign(s) and specified date range.", "No Data", ShowMessageType.Information);
                    });
                    return;
                }

                DataTable dtSummary = dsStatusLoadingSummaryReport.Tables[0];
                DataTable dtFiguresByStatusLoader = dsStatusLoadingSummaryReport.Tables[1];
                DataTable dtFilteringStrings = dsStatusLoadingSummaryReport.Tables[2];
                DataTable dtColumnMappings = dsStatusLoadingSummaryReport.Tables[3];
                DataTable dtFormulaMappings = dsStatusLoadingSummaryReport.Tables[4];

                #endregion Get data

                #region Setup Excel workbook

                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateStatusLoading.xlsx");
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                string filePathAndName = String.Format("{0}Status Loading Summary Report ({1} - {2}) ~ {3}.xlsx",
                    GlobalSettings.UserFolder,
                    _startDate.ToString("yyyy-MM-dd"),
                    _endDate.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));

                #endregion Setup Excel workbook

                #region Variable definitions & initializations

                int templateRowSpan = 6;
                int templateColumnSpan = 5;
                int templateDataRowIndex = 7;
                int templateTotalsRowIndex = 8;
                int reportRow = 7;
                int fromRow = 0;
                int toRow = 0;

                #endregion Variable definitions & initializations

                #region Insert the summary sheet

                Worksheet wsTemplate = wbTemplate.Worksheets["Summary"];
                Worksheet wsSummary = wbReport.Worksheets.Add("Summary");
                Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsSummary, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                #region Insert the worksheet headings

                Methods.CopyExcelRegion(wsTemplate, 0, 0, templateRowSpan, templateColumnSpan, wsSummary, 0, 0);

                //wsReport.GetCell("A1").Value = reportTitle; //String.Format("Diary Report - {0}", campaign);
                wsSummary.GetCell("A3").Value = String.Format("For all statuses loaded between {0} and {1}", _startDate.ToString("dddd, d MMMM yyyy"), _endDate.ToString("dddd, d MMMM yyyy"));
                wsSummary.GetCell("F5").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                #endregion Insert the worksheet headings

                #region Add the data

                fromRow = reportRow;
                reportRow = Methods.MapTemplatizedExcelValues(wsTemplate, dtSummary, dtColumnMappings, templateDataRowIndex, 0, 0, templateColumnSpan, wsSummary, reportRow, 0);
                toRow = reportRow - 1;

                #endregion Add the data

                #region Insert the totals

                reportRow = Methods.MapTemplatizedExcelFormulas(wsTemplate, dtFormulaMappings, templateTotalsRowIndex, 0, 0, templateColumnSpan, wsSummary, reportRow, 0, fromRow, toRow);

                #endregion Insert the totals 

                #endregion Insert the summary sheet

                #region Insert a report sheet for each status loader

                Worksheet wsReport;
                string statusLoaderName;
                string reportFilterString;

                foreach (DataRow drFilteringString in dtFilteringStrings.Rows)
                {
                    statusLoaderName = drFilteringString["StatusLoaderName"].ToString();
                    reportFilterString = drFilteringString["ReportFilterString"].ToString();
                    reportRow = 7;

                    var filteredStatusLoaderRows = dtFiguresByStatusLoader.Select(reportFilterString).AsEnumerable();
                    if (filteredStatusLoaderRows.Any())
                    {
                        #region Insert the sheet

                        wsReport = wbReport.Worksheets.Add(Methods.ParseWorksheetName(wbReport, statusLoaderName));
                        Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                        #endregion Insert the sheet

                        #region Insert the worksheet headings

                        Methods.CopyExcelRegion(wsTemplate, 0, 0, templateRowSpan, templateColumnSpan, wsReport, 0, 0);

                        wsReport.GetCell("A1").Value = String.Format("Status Loading Summary Report - {0}", statusLoaderName);
                        wsReport.GetCell("A3").Value = String.Format("For all statuses loaded between {0} and {1}", _startDate.ToString("dddd, d MMMM yyyy"), _endDate.ToString("dddd, d MMMM yyyy"));
                        wsReport.GetCell("F5").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        #endregion Insert the worksheet headings

                        #region Add the data

                        DataTable dtCurrentStatusLoaderRecords = dtFiguresByStatusLoader.Select(reportFilterString).CopyToDataTable();

                        fromRow = reportRow;
                        reportRow = Methods.MapTemplatizedExcelValues(wsTemplate, dtCurrentStatusLoaderRecords, dtColumnMappings, templateDataRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);
                        toRow = reportRow - 1;

                        #endregion Add the data

                        #region Insert the totals

                        reportRow = Methods.MapTemplatizedExcelFormulas(wsTemplate, dtFormulaMappings, templateTotalsRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0, fromRow, toRow);

                        #endregion Insert the totals 
                    }
                }

                #endregion Insert a report sheet for each status loader

                #region Save the resulting workbook

                if (wbReport.Worksheets.Count > 0)
                {
                    //Save excel document
                    wbReport.Save(filePathAndName);

                    //Open excel document
                    Process.Start(filePathAndName);
                }

                #endregion Save the resulting workbook

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


        private void ReportOLD(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                IEnumerable<DataRecord> campaigns = e.Argument as IEnumerable<DataRecord>;
                # region Setup excel documents

                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                string reportName = "Report Status Loading";

                string dateRange = (_startDate.ToString("d") + " to " + _endDate.ToString("d")).Replace("/", "-");
                //string filePathAndName = GlobalSettings.UserFolder + "Report Status Loading  Summary(" + dateRange + ") ~ " + DateTime.Now.Millisecond + ".xlsx";

                string filePathAndName = String.Format("{0}Report Status Loading Summary ({1} to {2}) ~ {3}.xlsx", 
                    GlobalSettings.UserFolder,
                    _startDate.ToString("yyyy-MM-dd"),
                    _endDate.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                Uri uri = new Uri("/Templates/ReportTemplateStatusLoadingSummary.xlsx", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    wbTemplate = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }
                Worksheet wsTemplate = wbTemplate.Worksheets[0];
                Worksheet wsReport = wbReport.Worksheets.Add(reportName);

                wsReport.Columns[0].Width = 10000;
                wsReport.Columns[1].Width = 4500;
                wsReport.Columns[2].Width = 4500;

                wsReport.PrintOptions.PaperSize = PaperSize.A4;
                wsReport.PrintOptions.Orientation = Orientation.Portrait;
                int rows = campaigns.Count();
                int rowCount = 0;
                int rowIndex = 6;
                int totals = 0;
                int totalCheckedLoadedStatus = 0;

                #endregion
                string campaignIDs = string.Empty;
               // bool isFirst = true;
                foreach (DataRecord record in campaigns)
                {
                    if ((bool)record.Cells["Select"].Value)
                    {
                        long campaignID = Convert.ToInt32(record.Cells["CampaignID"].Value);
                        //string campaignName = record.Cells["CampaignName"].Value.ToString();
                        //if (isFirst == true)
                        //{
                        //    isFirst = false;
                        //    campaignIDs = campaignID.ToString() + ",";
                        //}
                        //else
                        //{
                            campaignIDs = campaignIDs +campaignID + ","  ;
                        //}
                    }
                }
                rowCount++;

                #region Get report data from database

                DataTable dtLeadAllocationData;

                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@CampaignId", campaignIDs);
                parameters[1] = new SqlParameter("@StartDate", _startDate.ToString("yyyy-MM-dd"));
                parameters[2] = new SqlParameter("@EndDate", _endDate.ToString("yyyy-MM-dd"));

                DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spINReportStatusLoading", parameters);
                if (dsLeadAllocationData.Tables.Count > 0)
                {
                    dtLeadAllocationData = dsLeadAllocationData.Tables[0];
                    if (dtLeadAllocationData.Rows.Count == 0)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected  Campaigns and specified Date range.", "No Data", ShowMessageType.Information);
                        });
                        return;
                    }
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected Campaigns and specified Date range.", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                #endregion

                Methods.CopyExcelRegion(wsTemplate, 0, 0, 4, 11, wsReport, 0, 0);

                #region report data

                {
                    //string previousValue = string.Empty;
                    List<string> lstCampaign = new List<string>();
                    List<string> lstUniqueCampaign = new List<string>();
                    wsReport.GetCell("A1").Value = "Summary of Report Status Summary";
                    wsReport.GetCell("A1").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                    wsReport.GetCell("A2").Value = "Start Date : " + _startDate.ToString();
                    wsReport.GetCell("A2").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                    wsReport.GetCell("A3").Value = "End Date : " + _endDate.ToString();
                    wsReport.GetCell("A3").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                    wsReport.GetCell("A5").Value = "Campaign";
                    wsReport.GetCell("A5").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                    wsReport.GetCell("B5").Value = "Statuses Loaded";
                    wsReport.GetCell("B5").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                    wsReport.Workbook.Worksheets[0].Columns[0].Width = 1000;

                    wsReport.GetCell("C5").Value = "Leads Checked";
                    wsReport.GetCell("C5").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                    wsReport.Workbook.Worksheets[0].Columns[0].Width = 1000;

                    foreach (DataRow dr in dtLeadAllocationData.Rows)
                    {
                        if (!lstCampaign.Contains(dr["Campaign"].ToString()))
                        {
                            

                            DataRow[] result = dtLeadAllocationData.Select(" Campaign = '" + dr["Campaign"] + "'");
                            DataRow[] checkedLoadedStatuses = dtLeadAllocationData.Select(String.Format("Campaign = '{0}' AND IsChecked = 1", dr["Campaign"]));
                            string test = dr["Campaign"].ToString();
                            Methods.CopyExcelRegion(wsTemplate, rowIndex - 1, 0, 1, 100, wsReport, rowIndex - 1, 0);

                            wsReport.GetCell("A" + (rowIndex).ToString()).Value = dr["Campaign"].ToString();
                            wsReport.GetCell("B" + (rowIndex).ToString()).Value = result.Count().ToString();
                            wsReport.GetCell("C" + (rowIndex).ToString()).Value = checkedLoadedStatuses.Count().ToString();
                            
                            wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("C" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("C" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("C" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("C" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
                            
                            rowIndex++;
                            lstCampaign.Add(dr["Campaign"].ToString());
                            lstUniqueCampaign.Add(dr["Campaign"].ToString());
                            totals += result.Count();
                            totalCheckedLoadedStatus += checkedLoadedStatuses.Count();
                        }
                    }
                    int count = lstCampaign.Count;

                    wsReport.GetCell("A" + (rowIndex).ToString()).Value = "Totals";
                    wsReport.GetCell("A" + (rowIndex).ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                    wsReport.GetCell("B" + (rowIndex).ToString()).Value = totals.ToString();
                    wsReport.GetCell("B" + (rowIndex).ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                    wsReport.GetCell("C" + (rowIndex).ToString()).Value = totalCheckedLoadedStatus.ToString();
                    wsReport.GetCell("C" + (rowIndex).ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                    wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                    wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                    wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                    wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                    wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                    wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                    wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                    wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                    wsReport.GetCell("C" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                    wsReport.GetCell("C" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                    wsReport.GetCell("C" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                    wsReport.GetCell("C" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                    #region Additional Sheet

                    List<string> lstAgent = new List<string>();
                    rowIndex = 7;
                    string campaign = string.Empty;
                    lstCampaign = new List<string>();


                    //Loop through the entire list of records
                    //Determine How many imports where made by that agent  for that particular campaign
                    // Do the stats
                    totals = 0;
                    totalCheckedLoadedStatus = 0;

                    foreach (DataRow dr in dtLeadAllocationData.Rows)
                    {
                       

                        rowIndex = 7;
                        if (!lstAgent.Contains(dr["AccessedBy"].ToString()))
                        {
                            string agent = dr["AccessedBy"].ToString();
                            Worksheet wsAgent = null;
                            if (!lstAgent.Contains(dr["AccessedBy"].ToString()))
                            {
                                wsAgent = wbReport.Worksheets.Add(string.Join("", agent.Take(31)));
                            }
                            wsAgent.Columns[0].Width = 10000;
                            wsAgent.Columns[1].Width = 4500;
                            wsAgent.Columns[2].Width = 4500;

                            Methods.CopyExcelRegion(wsAgent, rowIndex - 1, 0, 1, 100, wsAgent, rowIndex - 1, 0);

                            wsAgent.GetCell("A1").Value = "Summary of statuses loaded - " + dr["AccessedBy"].ToString();
                            wsAgent.GetCell("A3").Value = "Start Date";
                            wsAgent.GetCell("B3").Value = _startDate.Date.ToShortDateString();

                            wsAgent.GetCell("A4").Value = "End Date";
                            wsAgent.GetCell("B4").Value = _endDate.Date.ToShortDateString();

                            wsAgent.GetCell("A6").Value = "Campaign";
                            wsAgent.GetCell("A6").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                            wsAgent.GetCell("B6").Value = "Statuses Loaded";
                            wsAgent.GetCell("B6").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                            wsAgent.GetCell("C6").Value = "Leads Checked";
                            wsAgent.GetCell("C6").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                            wsAgent.GetCell("A6").CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("A6").CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("A6").CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("A6").CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                            wsAgent.GetCell("B6").CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("B6").CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("B6").CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("B6").CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                            wsAgent.GetCell("C6").CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("C6").CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("C6").CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("C6").CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                            DataRow[] result = null;
                            DataRow[] checkedLoadedStatuses = null;

                            for (int x = 0; x < lstUniqueCampaign.Count; x++)
                            {
                                //Query Statuses for that campaign and by this campaign
                                result = dtLeadAllocationData.Select(" Campaign = '" + lstUniqueCampaign[x].ToString() + "' AND  AccessedBy = '" + dr["AccessedBy"].ToString() + "'");
                                checkedLoadedStatuses = dtLeadAllocationData.Select(String.Format("Campaign = '{0}' AND AccessedBy = '{1}' AND IsChecked = 1", lstUniqueCampaign[x].ToString(), dr["AccessedBy"].ToString()));

                                wsAgent.GetCell("A" + rowIndex.ToString()).Value = lstUniqueCampaign[x];
                                wsAgent.GetCell("B" + rowIndex.ToString()).Value = result.Count().ToString();
                                wsAgent.GetCell("C" + rowIndex.ToString()).Value = checkedLoadedStatuses.Count().ToString();

                                totals += result.Count();
                                totalCheckedLoadedStatus += checkedLoadedStatuses.Count();

                                wsAgent.GetCell("A" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                                wsAgent.GetCell("A" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                                wsAgent.GetCell("A" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                                wsAgent.GetCell("A" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                                wsAgent.GetCell("B" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                                wsAgent.GetCell("B" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                                wsAgent.GetCell("B" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                                wsAgent.GetCell("B" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                                wsAgent.GetCell("C" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                                wsAgent.GetCell("C" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                                wsAgent.GetCell("C" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                                wsAgent.GetCell("C" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                                rowIndex++;
                            }

                          
                            //Totals Section
                            wsAgent.GetCell("A" + (rowIndex).ToString()).Value = "Totals";
                            wsAgent.GetCell("A" + (rowIndex).ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                            wsAgent.GetCell("B" + (rowIndex).ToString()).Value = totals.ToString();
                            wsAgent.GetCell("B" + (rowIndex).ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                            wsAgent.GetCell("C" + (rowIndex).ToString()).Value = totalCheckedLoadedStatus.ToString();
                            wsAgent.GetCell("C" + (rowIndex).ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                            
                            
                            wsAgent.GetCell("A" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("A" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("A" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("A" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                            wsAgent.GetCell("B" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("B" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("B" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("B" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                            wsAgent.GetCell("C" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("C" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("C" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                            wsAgent.GetCell("C" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                            totals = 0;
                            totalCheckedLoadedStatus = 0;
                            lstAgent.Add(agent);
                        }
                    }

                    //Save excel document
                    wbReport.Save(filePathAndName);
                    //Display excel document
                    Process.Start(filePathAndName);








                    //for (int x = 0; x < lstUniqueCampaign.Count; x++)
                    //{
                    //    campaign = lstUniqueCampaign[x].ToString();
                    //    foreach (DataRow row in dtLeadAllocationData.Rows)
                    //    {
                    //        if (!lstAgent.Contains(row["AccessedBy"].ToString()))
                    //        {
                    //            string agent = row["AccessedBy"].ToString();
                    //            Worksheet wsAgent = null;
                    //            if (!lstAgent.Contains(row["AccessedBy"].ToString()))
                    //            {
                    //                wsAgent = wbReport.Worksheets.Add(string.Join("", agent.Take(31)));
                    //            }
                    //            Methods.CopyExcelRegion(wsAgent, rowIndex - 1, 0, 1, 100, wsAgent, rowIndex - 1, 0);
                                
                    //            //headings
                    //            wsAgent.GetCell("A5").Value = "Campaign";
                    //            wsAgent.GetCell("A5").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                    //            wsAgent.GetCell("B5").Value = "Statuses Loaded";
                    //            wsAgent.GetCell("B5").CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                    //            wsAgent.GetCell("A1").Value = "Summary of statuses loaded  -" + row["AccessedBy"].ToString();
                    //            wsAgent.GetCell("A3").Value = "Start Date";
                    //            wsAgent.GetCell("B3").Value = _startDate.ToString();

                    //            wsAgent.GetCell("A4").Value = "End Date";
                    //            wsAgent.GetCell("B4").Value = _endDate.ToString();

                    //            DataRow[] result = dtLeadAllocationData.Select(" Campaign = '" + campaign + "'");
                    //            wsAgent.GetCell("A" + rowIndex.ToString()).Value = row["Campaign"];
                    //            wsAgent.GetCell("B" + rowIndex.ToString()).Value = result.Count().ToString();

                    //            totals += result.Count();
                    //            wsAgent.GetCell("A" + (rowIndex + 1).ToString()).Value = "Totals";
                    //            wsAgent.GetCell("A" + (rowIndex + 1).ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                    //            wsAgent.GetCell("B" + (rowIndex + 1).ToString()).Value = totals.ToString();
                    //            wsAgent.GetCell("B" + (rowIndex + 1).ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                    //            rowIndex++;
                    //            lstAgent.Add(agent);
                    //            lstCampaign.Add(campaign);
                    //        }
                    //    }
                    //}



                    #endregion



                }
                #endregion

                ////Save excel document
                //wbReport.Save(filePathAndName);
                ////Display excel document
                //Process.Start(filePathAndName);
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
                //IEnumerable<DataRecord> campaigns = xdgCampaigns.Records.Cast<DataRecord>().ToArray();

                var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

                if (_lstSelectedCampaigns.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No campaign selected", Embriant.Framework.ShowMessageType.Error);
                    return;
                }
                else
                {
                    _campaigns = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                    _campaigns = _campaigns.Substring(0, _campaigns.Length - 1);
                }

                btnClose.IsEnabled = false;
                btnReport.IsEnabled = false;
                xdgCampaigns.IsEnabled = false;
                calStartDate.IsEnabled = false;
                calEndDate.IsEnabled = false;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += Report;
                worker.RunWorkerCompleted += ReportCompleted;
                worker.RunWorkerAsync(_lstSelectedCampaigns);

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
                DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;

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
