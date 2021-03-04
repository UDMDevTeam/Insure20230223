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
    public partial class ReportFallOffScreen
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
      

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        #endregion



        #region Constructors

        public ReportFallOffScreen()
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
                string campaignIDs = string.Empty;
                bool isFirst = true;
                if (campaigns != null)
                {
                    foreach (DataRecord record in campaigns)
                    {
                        if ((bool)record.Cells["Select"].Value)
                        {
                            long campaignID = Convert.ToInt32(record.Cells["CampaignID"].Value);
                            if (isFirst == true)
                            {
                                isFirst = false;
                                campaignIDs = campaignID.ToString();                         
                            }
                            else
                            {
                                campaignIDs = campaignIDs + "," + campaignID;
                            }
                        }
                    }

                    //setup excel documents
                    if (campaignIDs != string.Empty)
                    {
                        #region Setting the Excel workbook

                        Workbook wbTemplate;
                        
                        string filePathAndName = GlobalSettings.UserFolder +  " Campaign Fall Off  Report ~ " + DateTime.Now.Millisecond + ".xlsx";
                        Uri uri = new Uri("/Templates/ReportTemplateFallOffReport.xlsx", UriKind.Relative);
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
                        Worksheet wsCancellationsDatasheetTemplate = wbTemplate.Worksheets["CancellationsDatasheet"];
                        Worksheet wsCarriedForwardDatasheetTemplate = wbTemplate.Worksheets["CarriedForwardDatasheet"];

                        #endregion Setting the Excel workbook

                        #region Getting the report data
                        //report data
                        DataTable dtLeadAllocationData;
                        SqlParameter[] parameters = new SqlParameter[3];

                        DateTime fromDate = _startDate;

                        DateTime toDate = _endDate;
                        if(!campaignIDs.Contains(","))
                        {
                            campaignIDs = campaignIDs + ",";
                        }

                        parameters[0] = new SqlParameter("@CampaignId", campaignIDs);
                        parameters[1] = new SqlParameter("@FromDate", fromDate.ToString("yyyy-MM-dd"));
                        parameters[2] = new SqlParameter("@ToDate", toDate.ToString("yyyy-MM-dd"));
                        DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spINReportFallOff", parameters);

                        if (dsLeadAllocationData.Tables.Count > 0)
                        {
                            dtLeadAllocationData = dsLeadAllocationData.Tables[0];

                            if (dtLeadAllocationData.Rows.Count == 0)
                            {
                                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                                {
                                    ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected Campaigns and specified Date range.", "No Data", ShowMessageType.Information);
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

                        #endregion Getting the report data

                        #region Main Report

                        Worksheet wsReport = wbReport.Worksheets.Add("Fall Off Report");
                        wsReport.PrintOptions.PaperSize = PaperSize.A4;
                        wsReport.PrintOptions.Orientation = Orientation.Portrait;
                        wsReport.PrintOptions.ScalingType = ScalingType.FitToPages;
                        Methods.CopyExcelRegion(wsTemplate, 0, 0, 1, 23, wsReport, 2, 0);

                        int rowFirstData = 5;
                        int rowIndex = rowFirstData;
                        List<string> campaignsList = new List<string>();
                        foreach (DataRow rw in dtLeadAllocationData.Rows)
                        {
                            string campaign = rw["CampaignName"].ToString();
                            if (!campaignsList.Contains(campaign))
                            {
                                campaignsList.Add(campaign);
                            }
                        }

                        System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                        wsReport.GetCell("A" + (rowIndex - 2)).Value = "Campaign Fall Of Report ";
                        decimal totalSales = 0;
                        decimal confirmedTotal = 0;
                        decimal confirmationCancelledTotal = 0;
                        decimal carriedForwardTotal = 0;
                        decimal cancelledByPlatinumTotal = 0;
                           
                        decimal cancelationClientHasPolicy = 0;
                        decimal carriedForwardToCancellation = 0;
                         
                        decimal numberOfCombinedClientHasPolicy = 0;
                        decimal numberOfCombinedcarriedForward = 0;
                        decimal numberOfCombinedQueriedCancelled = 0;
                        decimal totalCancellations = 0;

                        decimal totalCallMonitoringCancellations = 0;
                        decimal callMonitoringCarriedForwardTotal = 0;

                        string columnsToBeHidden = "3";

                        wsReport.GetCell("A" + 1).Value = "Campaign Fall Of Report - " + _startDate.ToShortDateString() + " to "+ _endDate.ToShortDateString();
                        wsReport.GetCell("A" + 2).Value = "Compiled By " + Methods.GetTableData("select FirstName + ' ' + LastName as CompiledBy from dbo.[User] where ID = " + GlobalSettings.ApplicationUser.ID).Rows[0]["CompiledBy"];

                        #region Get the individual numbers for each campaign

                        foreach (string campaign in campaignsList)
                        {
                            Methods.CopyExcelRegion(wsTemplate, 2, 0, 1, 23, wsReport, rowIndex - 1, 0);
                            //get falls offs for agent
                            List<string> fallOfStatuses = new List<string>();
                            fallOfStatuses.Add("Cancelled");
                            fallOfStatuses.Add("Carried Forward");
                            var rows = dtLeadAllocationData.AsEnumerable().Where(x => ((string)x["CampaignName"] == campaign));                             
                            int numberOfSales = 0;
                            int numberOfSalesOriginal = 0;
                            decimal numberOfCancelled = 0;
                            decimal numberOfConfirmed = 0;
                            decimal queriedCancelled = 0;
                            decimal numberOfCarriedForward = 0;
                            decimal numberCancelledByPlatinum = 0;
                            decimal numberOfCombineCancellations = 0;

                            decimal numberOfCallMonitoringCancellations = 0;
                            decimal numberOfCallMonitoringCarriedForwards = 0;

                            //numberOfCombinedClientHasPolicy = 0;
                            //numberOfCombinedcarriedForward = 0; 

                            foreach (DataRow rw in rows)
                            {
                                #region Sales

                                if ((string)rw["Status"] == "Sale") //|| ((string)rw["Status"] == "Carried Forward"))
                                {
                                    numberOfSales++;
                                    totalSales++;
                                    string isConfirmed = rw["IsConfirmed"].ToString();
                                    if (isConfirmed.ToLower() == "true")
                                    {
                                        numberOfConfirmed++;
                                        confirmedTotal++;
                                    }
                                }

                                #endregion Sales

                                #region Cancellations

                                //if (((string)rw["Status"] == "Cancelled") || ((string)rw["Status"] == "Call Monitoring Cancellation")) // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/220382093/comments
                                if ((string)rw["Status"] == "Cancelled" || Convert.ToInt32(rw["StatusID"]) == 13)
                                {
                                    //// See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/213218545/comments
                                    //numberOfSales++;
                                    //totalSales++;

                                    string cancellationReasonID = rw["FKINCancellationReasonID"].ToString();
                                    if (cancellationReasonID == "7")//cancelled with platinum life
                                    {
                                        numberCancelledByPlatinum++;
                                        cancelledByPlatinumTotal++;
                                    }
                                    if (cancellationReasonID != "7")
                                    {
                                        numberOfCancelled++;
                                        confirmationCancelledTotal++;
                                    }
                                }

                                #endregion Cancellations

                                #region Carried Forwards

                                //if (((string)rw["Status"] == "Carried Forward") || ((string)rw["Status"] == "Call Monitoring Carried Forward")) // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/220382093/comments
                                if ((string)rw["Status"] == "Carried Forward")
                                {
                                    //// See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/213218545/comments
                                    //numberOfSales++;
                                    //totalSales++;

                                    numberOfCarriedForward++;
                                    carriedForwardTotal++;
                                }

                                #endregion Carried Forwards

                                #region Call-Monitoring Cancellations

                                //See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/222343878/comments

                                if ((string)rw["Status"] == "Call Monitoring Cancellation") // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/220382093/comments
                                {
                                    numberOfCallMonitoringCancellations++;
                                    totalCallMonitoringCancellations++;
                                }

                                #endregion Call-Monitoring Cancellations

                                #region Call-Monitoring Carried Forwards

                                //See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/222343878/comments

                                if ((string)rw["Status"] == "Call Monitoring Carried Forward")
                                {
                                    numberOfCallMonitoringCarriedForwards++;
                                    callMonitoringCarriedForwardTotal++;
                                }

                                #endregion Call-Monitoring Carried Forwards

                                #region Cancellation Reason

                                string CancelationId = rw["FKINCancellationReasonID"].ToString();
                                if (CancelationId == "8")
                                {
                                    cancelationClientHasPolicy++;
                                    numberOfCombinedClientHasPolicy++;
                                }
                                CancelationId = rw["FKINCancellationReasonID"].ToString();
                                if (CancelationId == "9" || Convert.ToInt32(rw["StatusID"]) == 13)
                                {
                                    carriedForwardToCancellation++;
                                    numberOfCombinedcarriedForward++;
                                }
                                if (CancelationId == "10")
                                {
                                    queriedCancelled++;
                                    numberOfCombinedQueriedCancelled++;
                                }

                                #endregion Cancellation Reason
                            }
                            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/208066432/comments
                            numberOfCancelled = numberOfCancelled - cancelationClientHasPolicy - carriedForwardToCancellation + queriedCancelled;

                            #region Formating

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

                            wsReport.GetCell("D" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("D" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("D" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("D" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("E" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("E" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("E" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("E" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("K" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("K" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("K" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("K" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("O" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("O" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("O" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("O" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("S" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("S" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("S" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("S" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("T" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("T" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("T" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("T" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("U" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("U" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("U" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("U" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("V" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("V" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("V" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("V" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            wsReport.GetCell("W" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("W" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("W" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                            wsReport.GetCell("W" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                            #endregion Formating

                            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/213218545/comments
                            numberOfSalesOriginal = numberOfSales + int.Parse(numberOfCancelled.ToString()) + int.Parse(numberOfCarriedForward.ToString()) 
                                                    + int.Parse(numberOfCallMonitoringCancellations.ToString()) + int.Parse(numberOfCallMonitoringCarriedForwards.ToString()) 
                                                    + int.Parse(numberCancelledByPlatinum.ToString()) + int.Parse(cancelationClientHasPolicy.ToString()) + int.Parse(carriedForwardToCancellation.ToString());
                            //numberOfSalesOriginal = numberOfSales + int.Parse(numberCancelledByPlatinum.ToString()) + int.Parse(numberOfCancelled.ToString());

                            wsReport.GetCell("A" + rowIndex).Value = campaign;
                            wsReport.GetCell("B" + rowIndex).Value = numberOfSalesOriginal; 
                            wsReport.GetCell("C" + rowIndex).Value = numberOfSales;

                            #region Confirmed (Columns D & E)
                            wsReport.GetCell("D" + rowIndex).Value = numberOfConfirmed;
                            if (numberOfSales > 0)
                            {
                                wsReport.GetCell("E" + rowIndex).ApplyFormula("=D" + rowIndex + "/C" + rowIndex);
                            }
                            else
                            {
                                wsReport.GetCell("E" + rowIndex).Value = 0;
                            }

                            #endregion Confirmed (Columns D & E)

                            #region Cancellations (Columns F & G)

                            wsReport.GetCell("F" + rowIndex).Value = numberOfCancelled;
                            if (numberOfSalesOriginal > 0)
                            {
                                //wsReport.GetCell("G" + rowIndex).Value = Math.Round(numberOfCancelled / numberOfSalesOriginal * 100, 2) + " %";
                                wsReport.GetCell("G" + rowIndex).ApplyFormula("=F" + rowIndex + "/B" + rowIndex);
                            }
                            else
                            {
                                wsReport.GetCell("G" + rowIndex).Value = 0;
                            }

                            #endregion Cancellations (Columns F & G)

                            #region Carried Forwards (Columns H & I)

                            wsReport.GetCell("H" + rowIndex).Value = numberOfCarriedForward;
                            if (numberOfSalesOriginal > 0)
                            {
                                //wsReport.GetCell("I" + rowIndex).Value = Math.Round(numberOfCarriedForward / numberOfSalesOriginal * 100, 2) + " %";
                                wsReport.GetCell("I" + rowIndex).ApplyFormula("=H" + rowIndex + "/B" + rowIndex);
                            }
                            else
                            {
                                wsReport.GetCell("I" + rowIndex).Value = 0;
                            }

                            #endregion Carried Forwards (Columns H & I)

                            #region Platinum Cancellations (Columns J & K)

                            wsReport.GetCell("J" + rowIndex).Value = numberCancelledByPlatinum;
                            if (numberOfSalesOriginal > 0)
                            {
                                //wsReport.GetCell("K" + rowIndex).Value = Math.Round(numberCancelledByPlatinum / numberOfSalesOriginal * 100, 2) + " %";
                                wsReport.GetCell("K" + rowIndex).ApplyFormula("=J" + rowIndex + "/B" + rowIndex);
                            }
                            else
                            {
                                wsReport.GetCell("K" + rowIndex).Value = 0;
                            }

                            #endregion Platinum Cancellations (Columns J & K)

                            #region Cancellation - Client already is a policy holder (Columns L & M)

                            // Cancellation - Client has Policy
                            wsReport.GetCell("L" + rowIndex).Value = cancelationClientHasPolicy;
                            if (numberOfSalesOriginal > 0)
                            {
                                //wsReport.GetCell("M" + rowIndex).Value = Math.Round(cancelationClientHasPolicy / numberOfSalesOriginal * 100, 2) + " %";
                                wsReport.GetCell("M" + rowIndex).ApplyFormula("=L" + rowIndex + "/B" + rowIndex);
                            }
                            else
                            {
                                wsReport.GetCell("M" + rowIndex).Value = 0;
                            }

                            #endregion Cancellation - Client already is a policy holder (Columns L & M)

                            #region Carried forward to Cancellation (Columns N & O)

                            //Carried forward to Cancellation
                            wsReport.GetCell("N" + rowIndex).Value = carriedForwardToCancellation;
                            if (numberOfSalesOriginal > 0)
                            {
                                //wsReport.GetCell("O" + rowIndex).Value = Math.Round(carriedForwardToCancellation / numberOfSalesOriginal * 100, 2) + " %";
                                wsReport.GetCell("O" + rowIndex).ApplyFormula("=N" + rowIndex + "/B" + rowIndex);
                            }
                            else
                            {
                                wsReport.GetCell("O" + rowIndex).Value = 0;
                            }

                            #endregion Carried forward to Cancellation (Columns N & O)

                            #region Call-Monitoring Cancellations (Columns P & Q)

                            wsReport.GetCell("P" + rowIndex).Value = numberOfCallMonitoringCancellations;
                            if (numberOfSalesOriginal > 0)
                            {
                                //wsReport.GetCell("Q" + rowIndex).Value = Math.Round(numberOfCallMonitoringCancellations / numberOfSalesOriginal * 100, 2) + " %";
                                wsReport.GetCell("Q" + rowIndex).ApplyFormula("=P" + rowIndex + "/B" + rowIndex);
                            }
                            else
                            {
                                wsReport.GetCell("Q" + rowIndex).Value = 0;
                            }

                            #endregion Call-Monitoring Cancellations (Columns P & Q)

                            #region Call-Monitoring Carried Forwards (Columns R & S)

                            wsReport.GetCell("R" + rowIndex).Value = numberOfCallMonitoringCarriedForwards;
                            if (numberOfSalesOriginal > 0)
                            {
                                //wsReport.GetCell("S" + rowIndex).Value = Math.Round(numberOfCallMonitoringCarriedForwards / numberOfSalesOriginal * 100, 2) + " %";
                                wsReport.GetCell("S" + rowIndex).ApplyFormula("=R" + rowIndex + "/B" + rowIndex);
                            }
                            else
                            {
                                wsReport.GetCell("S" + rowIndex).Value = 0;
                            }

                            #endregion Call-Monitoring Carried Forwards (Columns R & S)

                            #region Combined Carried Forwards (Columns T & U)

                            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/222495090/comments

                            //wsReport.GetCell("T" + rowIndex).Value = numberOfCarriedForward /* Column H */ + numberOfCallMonitoringCarriedForwards /* Column R */;
                            wsReport.GetCell("T" + rowIndex).ApplyFormula("=H" + rowIndex + "+R" + rowIndex);
                            if (numberOfSalesOriginal > 0)
                            {
                                //wsReport.GetCell("U" + rowIndex).Value = Math.Round((numberOfCarriedForward + numberOfCallMonitoringCarriedForwards) / numberOfSalesOriginal * 100, 2) + " %";
                                wsReport.GetCell("U" + rowIndex).ApplyFormula("=T" + rowIndex + "/B" + rowIndex);
                            }
                            else
                            {
                                wsReport.GetCell("U" + rowIndex).Value = 0;
                            }

                            #endregion Combined Carried Forwards (Columns T & U)

                            #region Combined Cancellations (Columns V & W)

                            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/208066432/comments
                            //numberOfCombineCancellations = numberCancelledByPlatinum + numberOfCancelled + cancelationClientHasPolicy + carriedForwardToCancellation /*+ queriedCancelled*/;
                            //wsReport.GetCell("V" + rowIndex).Value = numberOfCombineCancellations + numberOfCallMonitoringCancellations; // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/222872768/comments
                            wsReport.GetCell("V" + rowIndex).ApplyFormula("=F" + rowIndex + "+J" + rowIndex + "+L" + rowIndex + "+N" + rowIndex + "+P" + rowIndex);
                            if (numberOfSalesOriginal > 0)
                            {
                                //wsReport.GetCell("W" + rowIndex.ToString()).Value = Math.Round((numberOfCombineCancellations + numberOfCallMonitoringCancellations) / numberOfSalesOriginal * 100, 2) + " %";
                                wsReport.GetCell("W" + rowIndex).ApplyFormula("=V" + rowIndex + "/B" + rowIndex);
                            }
                            else
                            {
                                wsReport.GetCell("W" + rowIndex).Value = 0;
                            }

                            #endregion Combined Cancellations (Columns T & U)

                            rowIndex++;
                            carriedForwardToCancellation = 0;
                            cancelationClientHasPolicy = 0;
                        }

                        #endregion Get the individual numbers for each campaign

                        #region Totals

                        Methods.CopyExcelRegion(wsTemplate, 2, 0, 1, 23, wsReport, rowIndex - 1, 0);

                        // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/208066432/comments
                        confirmationCancelledTotal = confirmationCancelledTotal - numberOfCombinedClientHasPolicy - numberOfCombinedcarriedForward + numberOfCombinedQueriedCancelled;

                        #region Formatting

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

                        wsReport.GetCell("D" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("D" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("D" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("D" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("E" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("E" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("E" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("E" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("K" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("K" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("K" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("K" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("O" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("O" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("O" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("O" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("S" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("S" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("S" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("S" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("T" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("T" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("T" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("T" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("U" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("U" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("U" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("U" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("V" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("V" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("V" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("V" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("W" + rowIndex.ToString()).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("W" + rowIndex.ToString()).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("W" + rowIndex.ToString()).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                        wsReport.GetCell("W" + rowIndex.ToString()).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;

                        wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        wsReport.GetCell("A" + rowIndex.ToString()).Value = "Totals";
                        wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                        #endregion Formatting

                        // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/213218545/comments
                        //decimal totalOriginalSales = (totalSales + confirmationCancelledTotal + cancelledByPlatinumTotal);
                        decimal totalOriginalSales = totalSales + confirmationCancelledTotal + carriedForwardTotal 
                                                     + totalCallMonitoringCancellations + callMonitoringCarriedForwardTotal
                                                     + cancelledByPlatinumTotal + numberOfCombinedClientHasPolicy + numberOfCombinedcarriedForward;

                        //wsReport.GetCell("B" + rowIndex).Value = totalOriginalSales;
                        wsReport.GetCell("B" + rowIndex).ApplyFormula("=SUM(B" + rowFirstData + ":B" + (rowIndex - 1) + ")");
                        //wsReport.GetCell("C" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        //wsReport.GetCell("C" + rowIndex).Value = totalSales;
                        wsReport.GetCell("C" + rowIndex).ApplyFormula("=SUM(C" + rowFirstData + ":C" + (rowIndex - 1) + ")");

                        #region Total Confirmations (Columns D & E)

                        //wsReport.GetCell("D" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        //wsReport.GetCell("D" + rowIndex).Value = confirmedTotal;
                        wsReport.GetCell("D" + rowIndex).ApplyFormula("=SUM(D" + rowFirstData + ":D" + (rowIndex - 1) + ")");

                        //percentage confirmed
                        //wsReport.GetCell("E" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        if (totalSales > 0)
                        {
                            //wsReport.GetCell("E" + rowIndex).Value = Math.Round(confirmedTotal / totalSales * 100, 2) + " %";
                            wsReport.GetCell("E" + rowIndex).ApplyFormula("=D" + rowIndex + "/C" + rowIndex);
                        }
                        else
                        {
                            wsReport.GetCell("E" + rowIndex).Value = 0;
                        }

                        #endregion Total Confirmations (Columns D & E)

                        #region Total Cancellations (Columns F & G)

                        //wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        //wsReport.GetCell("F" + rowIndex).Value = confirmationCancelledTotal;
                        wsReport.GetCell("F" + rowIndex).ApplyFormula("=SUM(F" + rowFirstData + ":F" + (rowIndex - 1) + ")");

                        //percentage total cancelled by confirmation
                        //wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        if (totalOriginalSales > 0)
                        {
                            //wsReport.GetCell("G" + rowIndex.ToString()).Value = Math.Round(confirmationCancelledTotal / totalOriginalSales * 100, 2) + " %";
                            wsReport.GetCell("G" + rowIndex).ApplyFormula("=F" + rowIndex + "/B" + rowIndex);
                        }
                        else
                        {
                            wsReport.GetCell("G" + rowIndex).Value = 0;
                        }

                        #endregion Total Cancellations (Columns F & G)

                        #region Total Carried Forwards (Columns H & I)

                        //wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        //wsReport.GetCell("H" + rowIndex.ToString()).Value = carriedForwardTotal;
                        wsReport.GetCell("H" + rowIndex).ApplyFormula("=SUM(H" + rowFirstData + ":H" + (rowIndex - 1) + ")");
                        //percentage total carried forward
                        //wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        if (totalOriginalSales > 0)
                        {
                            //wsReport.GetCell("I" + rowIndex.ToString()).Value = Math.Round(carriedForwardTotal / totalOriginalSales * 100, 2) + " %";
                            wsReport.GetCell("I" + rowIndex).ApplyFormula("=H" + rowIndex + "/B" + rowIndex);
                        }
                        else
                        {
                            wsReport.GetCell("I" + rowIndex).Value = 0;
                        }

                        #endregion Total Carried Forwards (Columns H & I)

                        #region Total Platinum Cancellations (Columns J & K)

                        //wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        //wsReport.GetCell("J" + rowIndex.ToString()).Value = cancelledByPlatinumTotal;
                        wsReport.GetCell("J" + rowIndex).ApplyFormula("=SUM(J" + rowFirstData + ":J" + (rowIndex - 1) + ")");
                        //////percentage cancelled by platinum
                        wsReport.GetCell("K" + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        if (totalOriginalSales > 0 )
                        {
                            //wsReport.GetCell("K" + rowIndex).Value = Math.Round(cancelledByPlatinumTotal / totalOriginalSales * 100, 2) + " %";
                            wsReport.GetCell("K" + rowIndex).ApplyFormula("=J" + rowIndex + "/B" + rowIndex);
                        }
                        else
                        {
                            wsReport.GetCell("K" + rowIndex).Value = 0;
                        }

                        /*
                            decimal numberOfCombinedClientHasPolicy = 0;
                            decimal numberOfCombinedcarriedForward = 0; 
                        */

                        #endregion Total Platinum Cancellations (Columns J & K)

                        #region Total Cancellations - Client already is a policy holder (Columns L & M)

                        ////Cancelled Already
                        //wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        //wsReport.GetCell("L" + rowIndex.ToString()).Value = numberOfCombinedClientHasPolicy.ToString(); 
                        wsReport.GetCell("L" + rowIndex).ApplyFormula("=SUM(L" + rowFirstData + ":L" + (rowIndex - 1) + ")");
                        //percentage already has a policy
                        //wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                        if (totalOriginalSales > 0)
                        {
                            //wsReport.GetCell("M" + rowIndex.ToString()).Value = Math.Round(numberOfCombinedClientHasPolicy / totalOriginalSales * 100, 2) + " %";
                            wsReport.GetCell("M" + rowIndex).ApplyFormula("=L" + rowIndex + "/B" + rowIndex);
                        }
                        else
                        {
                            wsReport.GetCell("M" + rowIndex).Value = 0;
                        }

                        #endregion Total Cancellations - Client already is a policy holder (Columns L & M)

                        #region Total Carried forward to Cancellation (Columns N & O)

                        ////Carried Forward Cancelled
                        //wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        //wsReport.GetCell("N" + rowIndex.ToString()).Value = numberOfCombinedcarriedForward.ToString();
                        wsReport.GetCell("N" + rowIndex).ApplyFormula("=SUM(N" + rowFirstData + ":N" + (rowIndex - 1) + ")");
                        //percentage already has a policy
                        //wsReport.GetCell("O" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        if (totalOriginalSales > 0)
                        {
                            //wsReport.GetCell("O" + rowIndex.ToString()).Value = Math.Round(numberOfCombinedcarriedForward / totalOriginalSales * 100, 2) + " %";
                            wsReport.GetCell("O" + rowIndex).ApplyFormula("=N" + rowIndex + "/B" + rowIndex);
                        }
                        else
                        {
                            wsReport.GetCell("O" + rowIndex).Value = 0;
                        }

                        #endregion Total Carried forward to Cancellation (Columns N & O)

                        ////queried -cancelled
                        //wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        //wsReport.GetCell("P" + rowIndex.ToString()).Value = numberOfCombinedQueriedCancelled.ToString();
                        //wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        //if (totalSales > 0)
                        //{
                        //    wsReport.GetCell("Q" + rowIndex.ToString()).Value = Math.Round(numberOfCombinedQueriedCancelled / totalOriginalSales * 100, 2) + " %";
                        //}
                        //else
                        //{
                        //    wsReport.GetCell("Q" + rowIndex.ToString()).Value = 0 + " %";
                        //}
                        //////

                        #region Total Call-Monitoring Cancellations (Columns P & Q)

                        //wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        //wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.FormatString = "0";
                        //wsReport.GetCell("P" + rowIndex.ToString()).Value = totalCallMonitoringCancellations;
                        //wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        wsReport.GetCell("P" + rowIndex).ApplyFormula("=SUM(P" + rowFirstData + ":P" + (rowIndex - 1) + ")");
                        if (totalOriginalSales > 0)
                        {
                            //wsReport.GetCell("Q" + rowIndex.ToString()).Value = Math.Round(totalCallMonitoringCancellations / totalOriginalSales * 100, 2) + " %";
                            wsReport.GetCell("Q" + rowIndex).ApplyFormula("=P" + rowIndex + "/B" + rowIndex);
                        }
                        else
                        {
                            wsReport.GetCell("Q" + rowIndex).Value = 0;
                        }

                        #endregion Total Call-Monitoring Cancellations (Columns P & Q)

                        #region Total Call-Monitoring Carried Forwards (Columns R & S)

                        //wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        //wsReport.GetCell("R" + rowIndex.ToString()).Value = callMonitoringCarriedForwardTotal;
                        //wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.FormatString = "0";
                        //wsReport.GetCell("S" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        wsReport.GetCell("R" + rowIndex).ApplyFormula("=SUM(R" + rowFirstData + ":R" + (rowIndex - 1) + ")");
                        if (totalOriginalSales > 0)
                        {
                            //wsReport.GetCell("S" + rowIndex.ToString()).Value = Math.Round(callMonitoringCarriedForwardTotal / totalOriginalSales * 100, 2) + " %";
                            wsReport.GetCell("S" + rowIndex).ApplyFormula("=R" + rowIndex + "/B" + rowIndex);
                        }
                        else
                        {
                            wsReport.GetCell("S" + rowIndex).Value = 0;
                        }

                        #endregion Total Call-Monitoring Carried Forwards (Columns R & S)

                        #region Total Combined Carried Forwards (Columns T & U)

                        // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/222495090/comments

                        //wsReport.GetCell("T" + rowIndex.ToString()).Value = carriedForwardTotal /* Column H */ + callMonitoringCarriedForwardTotal /* Column R */;
                        wsReport.GetCell("T" + rowIndex).ApplyFormula("=SUM(T" + rowFirstData + ":T" + (rowIndex - 1) + ")");
                        if (totalOriginalSales > 0)
                        {
                            //wsReport.GetCell("U" + rowIndex.ToString()).Value = Math.Round((carriedForwardTotal + callMonitoringCarriedForwardTotal) / totalSales * 100, 2) + " %";
                            wsReport.GetCell("U" + rowIndex).ApplyFormula("=T" + rowIndex + "/B" + rowIndex);
                        }
                        else
                        {
                            wsReport.GetCell("U" + rowIndex).Value = 0;
                        }

                        #endregion Total Combined Carried Forwards (Columns T & U)

                        #region Total Cancellations (Columns V & W)

                        //totalCancellations = confirmationCancelledTotal + cancelledByPlatinumTotal + numberOfCombinedClientHasPolicy + numberOfCombinedcarriedForward + totalCallMonitoringCancellations/*+ numberOfCombinedQueriedCancelled*/;
                        //wsReport.GetCell("V" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        //wsReport.GetCell("V" + rowIndex.ToString()).Value = totalCancellations;
                        //////percentage combined cancellations
                        //wsReport.GetCell("W" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                        wsReport.GetCell("V" + rowIndex).ApplyFormula("=SUM(V" + rowFirstData + ":V" + (rowIndex - 1) + ")");
                        if (totalOriginalSales > 0)
                        {
                            //wsReport.GetCell("W" + rowIndex.ToString()).Value = Math.Round(totalCancellations / totalOriginalSales * 100, 2) + " %";
                            wsReport.GetCell("W" + rowIndex).ApplyFormula("=V" + rowIndex + "/B" + rowIndex);
                        }
                        else
                        {
                            wsReport.GetCell("W" + rowIndex).Value = 0;
                        }
                        //////////////

                        #endregion Total Cancellations (Columns V & W)

                        #endregion Totals

                        #endregion Main Report

                        #region Cancellations Datasheet

                        InsertCancellationDataSheet(wbReport, wsCancellationsDatasheetTemplate, campaignIDs, fromDate, toDate, columnsToBeHidden);

                        #endregion Cancellations Datasheet

                        #region Carried Forward Datasheet

                        InsertCarriedForwardsDataSheet(wbReport, wsCarriedForwardDatasheetTemplate, campaignIDs, fromDate, toDate, columnsToBeHidden);

                        #endregion Carried Forward Datasheet

                        #region Save and open the resulting workbook

                        //string fName = GlobalSettings.UserFolder + " Campaign Fall Off  Report ~ " + DateTime.Now.Millisecond + ".xlsx";
                        wbReport.Save(filePathAndName);

                        //Display excel document                           
                        Process.Start(filePathAndName);

                        #endregion Save and open the resulting workbook
                    }
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

        private void InsertCancellationDataSheet(Workbook wbResultingWorkbook, Worksheet wsTemplate, string campaignIDs, DateTime fromDate, DateTime toDate, string columnsToBeHidden)
        {
            #region Firstly, check if there is any data to be displayed

            DataSet dsFallOffReportDatasheetData = UDM.Insurance.Business.Insure.INGetFallOffReportDatasheetData(campaignIDs, fromDate, toDate);

            if ((dsFallOffReportDatasheetData == null) || (dsFallOffReportDatasheetData.Tables.Count == 0))
            {
                return;
            }

            if (dsFallOffReportDatasheetData.Tables[0].Rows.Count == 0)
            {
                return;
            }

            #endregion Firstly, check if there is any data to be displayed

            DataTable dtCancellationsDatasheetData = dsFallOffReportDatasheetData.Tables[0];

            #region Declarations

            int reportRow = 3;
            int reportTemplateRowIndex = 3;
            int reportTemplateColumnSpan = 9;

            #endregion Declarations

            #region Add the new worksheet

            Worksheet wsNewReportSheet = wbResultingWorkbook.Worksheets.Add("Cancellations Datasheet");
            Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsNewReportSheet, true, true, true, true, true, false, false, false, true, true, true, true, true, true, true, true, false);

            //wsNewReportSheet.PrintOptions.ScalingType = ScalingType.FitToPages;
            //wsNewReportSheet.PrintOptions.MaxPagesHorizontally = 1;

            #endregion Add the new worksheet

            #region Step 1: Copy a region from the template that consists of the headings and the column headings

            Methods.CopyExcelRegion(wsTemplate, 0, 0, 2, reportTemplateColumnSpan, wsNewReportSheet, 0, 0);

            #endregion Step 1: Copy a region from the template that consists of the headings and the column headings

            for (int i = 0; i < dtCancellationsDatasheetData.Rows.Count; i++)
            {
                #region Step 2.1. Copy the template formatting for the data row

                Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, reportTemplateColumnSpan, wsNewReportSheet, reportRow, 0);

                #endregion Step 2.1. Copy the template formatting for the data row

                #region Step 2.2. Add the values

                wsNewReportSheet.GetCell(String.Format("A{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["RefNo"];
                wsNewReportSheet.GetCell(String.Format("B{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["CampaignName"];
                wsNewReportSheet.GetCell(String.Format("C{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["DateOfSale"];
                wsNewReportSheet.GetCell(String.Format("D{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["DateOfWork"];
                wsNewReportSheet.GetCell(String.Format("E{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["DateOfCancellation"];
                wsNewReportSheet.GetCell(String.Format("F{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["TypeOfCancellation"];
                wsNewReportSheet.GetCell(String.Format("G{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["CancellationReason"];
                wsNewReportSheet.GetCell(String.Format("H{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["TSR"];
                wsNewReportSheet.GetCell(String.Format("I{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["CallMonitoringAgent"];
                wsNewReportSheet.GetCell(String.Format("J{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["ConfirmationAgent"];

                ++reportRow;

                #endregion Step 2.2. Add the values
            }

            #region Hiding all the unnecessary columns

            if (!String.IsNullOrEmpty(columnsToBeHidden))
            {
                string[] columns = columnsToBeHidden.Split(new char[] { ',' });

                foreach (string currentColumn in columns)
                {
                    byte currentColumnIndex = Convert.ToByte(currentColumn);
                    wsNewReportSheet.Columns[currentColumnIndex].Hidden = true;
                }
            }
            #endregion
        }

        private void InsertCarriedForwardsDataSheet(Workbook wbResultingWorkbook, Worksheet wsTemplate, string campaignIDs, DateTime fromDate, DateTime toDate, string columnsToBeHidden)
        {
            #region Firstly, check if there is any data to be displayed

            DataSet dsFallOffReportDatasheetData = UDM.Insurance.Business.Insure.INGetFallOffReportDatasheetData(campaignIDs, fromDate, toDate);

            if ((dsFallOffReportDatasheetData == null) || (dsFallOffReportDatasheetData.Tables.Count == 0))
            {
                return;
            }

            if (dsFallOffReportDatasheetData.Tables[1].Rows.Count == 0)
            {
                return;
            }

            #endregion Firstly, check if there is any data to be displayed

            DataTable dtCarriedForwardsDatasheetData = dsFallOffReportDatasheetData.Tables[1];

            #region Declarations

            int reportRow = 3;
            int reportTemplateRowIndex = 3;
            int reportTemplateColumnSpan = 10;

            #endregion Declarations

            #region Add the new worksheet

            Worksheet wsNewReportSheet = wbResultingWorkbook.Worksheets.Add("Carried Forwards Datasheet");
            Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsNewReportSheet, true, true, true, true, true, false, false, false, true, true, true, true, true, true, true, true, false);

            //wsNewReportSheet.PrintOptions.ScalingType = ScalingType.FitToPages;
            //wsNewReportSheet.PrintOptions.MaxPagesHorizontally = 1;

            #endregion Add the new worksheet

            #region Step 1: Copy a region from the template that consists of the headings and the column headings

            Methods.CopyExcelRegion(wsTemplate, 0, 0, 2, reportTemplateColumnSpan, wsNewReportSheet, 0, 0);

            #endregion Step 1: Copy a region from the template that consists of the headings and the column headings

            for (int i = 0; i < dtCarriedForwardsDatasheetData.Rows.Count; i++)
            {
                #region Step 2.1. Copy the template formatting for the data row

                Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, reportTemplateColumnSpan, wsNewReportSheet, reportRow, 0);

                #endregion Step 2.1. Copy the template formatting for the data row

                #region Step 2.2. Add the values

                wsNewReportSheet.GetCell(String.Format("A{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["RefNo"];
                wsNewReportSheet.GetCell(String.Format("B{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["CampaignName"];
                wsNewReportSheet.GetCell(String.Format("C{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["DateOfSale"];
                wsNewReportSheet.GetCell(String.Format("D{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["DateOfWork"];
                wsNewReportSheet.GetCell(String.Format("E{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["DateOfCarriedForward"];
                wsNewReportSheet.GetCell(String.Format("F{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["TypeOfCarriedForward"];
                wsNewReportSheet.GetCell(String.Format("G{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["CarriedForwardReason"];
                wsNewReportSheet.GetCell(String.Format("H{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["TSR"];
                wsNewReportSheet.GetCell(String.Format("I{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["CallMonitoringAgent"];
                wsNewReportSheet.GetCell(String.Format("J{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["ConfirmationAgent"];
                wsNewReportSheet.GetCell(String.Format("K{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["BatchNumber"];


                ++reportRow;

                #endregion Step 2.2. Add the values
            }

            #region Hiding all the unnecessary columns

            if (!String.IsNullOrEmpty(columnsToBeHidden))
            {
                string[] columns = columnsToBeHidden.Split(new char[] { ',' });

                foreach (string currentColumn in columns)
                {
                    byte currentColumnIndex = Convert.ToByte(currentColumn);
                    wsNewReportSheet.Columns[currentColumnIndex].Hidden = true;
                }
            }
            #endregion
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
                EnableDisableReportButton();
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
                btnReport.IsEnabled = false;
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
                EnableDisableReportButton();
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


        private void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
            
                
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

       

      


        #endregion

        private void calEndDate_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            _endDate = DateTime.Parse(calEndDate.SelectedDate.Value.ToString());
            EnableDisableReportButton();
        }

        private void calStartDate_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            _startDate = DateTime.Parse(calStartDate.SelectedDate.Value.ToString());
            EnableDisableReportButton();
        }

        private void EnableDisableReportButton()
        {
            if ((calStartDate.SelectedDate != null) && (calEndDate.SelectedDate != null))
            {
                btnReport.IsEnabled = true;
            }
            else
            {
                btnReport.IsEnabled = false;
            }
        }

    }
}
