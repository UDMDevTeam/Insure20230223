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
    /// Interaction logic for ReportLeadStatusScreen.xaml
    /// </summary>
    public partial class ReportLeadSearchScreen
    {

        #region Constants

        //private string _fontName = "Calibri";
        //private const int _fontSize = 10;
        //private const int _pointsToTwipsFactor = 20;
        //private const int _fontHeight = _fontSize * _pointsToTwipsFactor;

        #endregion

        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        private DataRow _campaign;
        private long _campaignID;
        private RecordCollectionBase _batches;
        private List<Record> _lstSelectedBatches;
        private bool? _reportContentOnSinglePageSheet;
        //string _batchIDs;
        //private DateTime _fromDate = DateTime.Now;
        //private DateTime _toDate = DateTime.Now;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        #endregion Private Members

        #region Constructors

        public ReportLeadSearchScreen()
        {
            InitializeComponent();
            LoadCampaignInfo();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

        private void EnableAllControls(bool isEnabled)
        {
            btnReport.IsEnabled = isEnabled;
            btnClose.IsEnabled = isEnabled;
            cmbCampaign.IsEnabled = isEnabled;
            xdgBatches.IsEnabled = isEnabled;
            //xdgCampaigns.IsEnabled = true;
            //Cal1.IsEnabled = true;
            //Cal2.IsEnabled = true;
        }

        private bool? AllRecordsSelected()
        {
            try
            {
                bool allSelected = true;
                bool noneSelected = true;

                if (xdgBatches.DataSource != null)
                {
                    foreach (DataRow dr in ((DataView)xdgBatches.DataSource).Table.Rows)
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

        //private void LoadCampaignInfo()
        //{
        //    try
        //    {
        //        SetCursor(Cursors.Wait);

        //        DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign");
        //        DataColumn column = new DataColumn("Select", typeof(bool));
        //        column.DefaultValue = false;
        //        dt.Columns.Add(column);
        //        dt.DefaultView.Sort = "CampaignName ASC";
        //        xdgCampaigns.DataSource = dt.DefaultView;
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }

        //    finally
        //    {
        //        SetCursor(Cursors.Arrow);
        //    }
        //}

        private void LoadCampaignInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataTable dt = Methods.GetTableData("SELECT [ID] AS [CampaignID], [Name] AS [CampaignName], [Code] AS [CampaignCode] FROM [INCampaign] WHERE [IsActive] = 1 ORDER BY [Name] ASC");
                cmbCampaign.Populate(dt, "CampaignName", "CampaignID");
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
                    if (_xdgHeaderPrefixAreaCheckbox != null && (_xdgHeaderPrefixAreaCheckbox.IsChecked == true || _xdgHeaderPrefixAreaCheckbox.IsChecked == null))
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

        //private void SetBatchCoverSheetWSOptions(Worksheet ws)
        //{
        //    ws.DisplayOptions.ShowGridlines = false;
        //    ws.PrintOptions.PaperSize = PaperSize.A4;
        //    ws.PrintOptions.Orientation = Orientation.Portrait;
        //    ws.PrintOptions.LeftMargin = 0.6;
        //    ws.PrintOptions.TopMargin = 0.8;
        //    //ws.PrintOptions.RightMargin = 0.6;
        //    ws.PrintOptions.BottomMargin = 0.8;
        //    //ws.PrintOptions.CenterHorizontally = true;
        //    //ws.PrintOptions.ScalingType = ScalingType.UseScalingFactor;
        //    //ws.PrintOptions.ScalingFactor = 90;

        //    ws.DisplayOptions.MagnificationInPageLayoutView = 100;
        //    ws.DisplayOptions.View = WorksheetView.Normal;
        //}

        //private void SetBatchClientSummaryWSOptions(Worksheet ws)
        //{
        //    ws.DisplayOptions.ShowGridlines = false;
        //    ws.PrintOptions.PaperSize = PaperSize.A4;
        //    ws.PrintOptions.Orientation = Orientation.Portrait;
        //    ws.PrintOptions.LeftMargin = 0.6;
        //    ws.PrintOptions.TopMargin = 0.8;
        //    //ws.PrintOptions.RightMargin = 0.2;
        //    ws.PrintOptions.BottomMargin = 0.8;
        //    //ws.PrintOptions.CenterHorizontally = true;
        //    //ws.PrintOptions.ScalingType = ScalingType.UseScalingFactor;
        //    //ws.PrintOptions.ScalingFactor = 90;

        //    ws.DisplayOptions.MagnificationInPageLayoutView = 100;
        //    ws.DisplayOptions.View = WorksheetView.Normal;
        //}

        //private void SetPolicyDataWSOptions(Worksheet ws)
        //{
        //    ws.DisplayOptions.ShowGridlines = false;
        //    ws.PrintOptions.PaperSize = PaperSize.A4;
        //    ws.PrintOptions.Orientation = Orientation.Landscape;
        //    ws.PrintOptions.LeftMargin = 0.6;
        //    ws.PrintOptions.TopMargin = 0.8;
        //    //ws.PrintOptions.RightMargin = 0.6;
        //    ws.PrintOptions.BottomMargin = 0.8;
        //    //ws.PrintOptions.CenterHorizontally = true;
        //    //ws.PrintOptions.ScalingType = ScalingType.UseScalingFactor;
        //    //ws.PrintOptions.ScalingFactor = 90;

        //    ws.DisplayOptions.MagnificationInPageLayoutView = 100;
        //    ws.DisplayOptions.View = WorksheetView.Normal;
        //}

        #region Report Operations Variant 1: Each batch on a separate sheet 

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                int rowIndex;
                int worksheetCount = 0;
                long campaignID = Convert.ToInt32(_campaign.ItemArray[0]);
                string campaignName = _campaign.ItemArray[1].ToString();
                string campaignDescription = String.Format("{0} ({1})", _campaign.ItemArray[1], _campaign.ItemArray[2]);
                string reportSubtitle = String.Empty; //String.Format(@"{0} - {1} / {2}", );

                DataTable dtLeadStatusData;

                #region Setup excel documents

                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                //string filePathAndName = GlobalSettings.UserFolder + campaignName + " Lead Status Report ~ " + DateTime.Now.Millisecond + ".xlsx";
                string filePathAndName = String.Format("{0}Lead Search Report ({1}), {2}.xlsx", GlobalSettings.UserFolder, campaignName, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                Uri uri = new Uri("/Templates/ReportTemplateLeadSearch.xlsx", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    wbTemplate = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }

                Worksheet wsTemplate = wbTemplate.Worksheets["Status"];
                Worksheet wsReport;
                //Worksheet wsReport = wbReport.Worksheets.Add(campaignName);
                //worksheetCount++;

                #endregion Setup excel documents

                //foreach (DataRecord record in _campaigns)

                // Loop through each selected batch
                //foreach (DataRecord record in _batches)
                foreach (DataRecord record in _lstSelectedBatches)
                {
                    #region Get report data from database

                    long batchID = Convert.ToInt64(record.Cells["ID"].Value);
                    //string batchDescription = record.Cells["Batch"].Value.ToString();
                    string batchDescription = Methods.ParseWorksheetName(wbReport, record.Cells["Code"].Value.ToString());

                    reportSubtitle = String.Format(@"{0} - {1} / {2}", campaignName, record.Cells["UDMCode"].Value, record.Cells["Code"].Value);
                    
					SqlParameter[] parameters = new SqlParameter[2];
					parameters[0] = new SqlParameter("@CampaignID", campaignID);
                    parameters[1] = new SqlParameter("@BatchID", batchID);
                    
					DataSet dsLeadStatusData = Methods.ExecuteStoredProcedure("spINLeadSearchReport", parameters);
                    

                    //if (dsLeadStatusData.Tables.Count > 0)
                    //{
                    //    dtLeadStatusData = dsLeadStatusData.Tables[0];

                    //    if (dtLeadStatusData.Rows.Count == 0)
                    //    {
                    //        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    //        {
                    //            ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + campaignName + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
                    //        });

                    //        continue;
                    //    }
                    //}
                    //else
                    //{
                    //    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    //    {
                    //        ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + campaignName + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
                    //    });

                    //    continue;
                    //}

                    #endregion Get report data from database

                    if (dsLeadStatusData.Tables.Count < 1)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), String.Format("There is no data for the {0} campaign.", campaignName), "No Data", ShowMessageType.Information);
                        });
                    }
                    else
                    {
                        #region Report Data

                        dtLeadStatusData = dsLeadStatusData.Tables[0];
                        
                        if (dtLeadStatusData.Rows.Count > 0)
                        {
                            rowIndex = 7;

                            wsReport = wbReport.Worksheets.Add(batchDescription);
                            worksheetCount++;

                            Methods.CopyExcelRegion(wsTemplate, 0, 0, 6, 26, wsReport, 0, 0);

                            #region Adding the details 

                            //wsReport.GetCell("A3").Value = reportSubtitle;
                            //wsReport.GetCell("A4").Value = String.Format("Report Date: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            WorksheetMergedCellsRegion mergedRegion;

                            mergedRegion = wsReport.MergedCellsRegions.Add(2, 0, 2, 25);
                            mergedRegion.CellFormat.BottomBorderStyle = CellBorderLineStyle.None;
                            mergedRegion.CellFormat.LeftBorderStyle = CellBorderLineStyle.None;
                            mergedRegion.CellFormat.RightBorderStyle = CellBorderLineStyle.None;
                            mergedRegion.CellFormat.TopBorderStyle = CellBorderLineStyle.None;

                            mergedRegion.Value = reportSubtitle;

                            mergedRegion.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                            mergedRegion.CellFormat.Alignment = HorizontalCellAlignment.Center;
                            mergedRegion.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                            mergedRegion.CellFormat.WrapText = ExcelDefaultableBoolean.True;

                            //Add the report date

                            mergedRegion = wsReport.MergedCellsRegions.Add(3, 0, 3, 25);
                            mergedRegion.CellFormat.BottomBorderStyle = CellBorderLineStyle.None;
                            mergedRegion.CellFormat.LeftBorderStyle = CellBorderLineStyle.None;
                            mergedRegion.CellFormat.RightBorderStyle = CellBorderLineStyle.None;
                            mergedRegion.CellFormat.TopBorderStyle = CellBorderLineStyle.None;

                            mergedRegion.Value = String.Format("Report Date: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            mergedRegion.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                            mergedRegion.CellFormat.Alignment = HorizontalCellAlignment.Left;
                            mergedRegion.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                            mergedRegion.CellFormat.WrapText = ExcelDefaultableBoolean.True;
                           
                            #endregion Adding the details

                            foreach (DataRow dr in dtLeadStatusData.Rows)
                            {
                                Methods.CopyExcelRegion(wsTemplate, 6, 0, 1, 26, wsReport, rowIndex - 1, 0);

                                wsReport.GetCell("A" + rowIndex).Value = dr["Batch Number"].ToString();
                                wsReport.GetCell("B" + rowIndex).Value = dr["PL Reference Number"].ToString();
                                wsReport.GetCell("C" + rowIndex).Value = dr["Name"].ToString();
                                wsReport.GetCell("D" + rowIndex).Value = dr["Surname"].ToString();
                                wsReport.GetCell("E" + rowIndex).Value = dr["Language"].ToString();
                                wsReport.GetCell("F" + rowIndex).Value = dr["NumberOfTimesRemarketed"] as int?;
                                wsReport.GetCell("G" + rowIndex).Value = dr["LastDateOfRemarketing"].ToString();
                                wsReport.GetCell("H" + rowIndex).Value = dr["Option on Lead"] as int?;
                                wsReport.GetCell("I" + rowIndex).Value = dr["Sale Status"].ToString();
                                wsReport.GetCell("J" + rowIndex).Value = dr["Allocation Date"].ToString();
                                wsReport.GetCell("K" + rowIndex).Value = dr["Date of Sale"].ToString();
                                wsReport.GetCell("L" + rowIndex).Value = dr["Original Premium Sold"] as decimal?;
                                wsReport.GetCell("M" + rowIndex).Value = dr["Final Premium Sold"] as decimal?;
                                wsReport.GetCell("N" + rowIndex).Value = dr["LA2"] as int?;
                                wsReport.GetCell("O" + rowIndex).Value = dr["LA2Relationship"].ToString();
                                wsReport.GetCell("P" + rowIndex).Value = dr["Child"] as int?;
                                wsReport.GetCell("Q" + rowIndex).Value = dr["Decline Status"].ToString();
                                wsReport.GetCell("R" + rowIndex).Value = dr["Date of decline"].ToString();
                                wsReport.GetCell("S" + rowIndex).Value = dr["Decline Reason"].ToString();
                                wsReport.GetCell("T" + rowIndex).Value = dr["Cancellation Status"].ToString();
                                wsReport.GetCell("U" + rowIndex).Value = dr["Date of Cancellation"].ToString();
                                wsReport.GetCell("V" + rowIndex).Value = dr["TSR Assigned To"].ToString();
                                wsReport.GetCell("W" + rowIndex).Value = dr["TSR Sold By"].ToString();
                                wsReport.GetCell("X" + rowIndex).Value = dr["Confirmation Agent"].ToString();
                                wsReport.GetCell("Z" + rowIndex).Value = dr["Original Campaign"].ToString();
                                wsReport.GetCell("AA" + rowIndex).Value = dr["Number of Referrals Generated"].ToString();
                                rowIndex++;
                            }

                        }
                        #endregion Report Data
                    }
                }

                //Save excel document
                wbReport.Save(filePathAndName);

                //Display excel document
                Process.Start(filePathAndName);
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

            EnableAllControls(true);

            //btnReport.IsEnabled = true;
            //btnClose.IsEnabled = true;
            //IsReportRunning = false;
            //xdgCampaigns.IsEnabled = true;
            //Cal1.IsEnabled = true;
            //Cal2.IsEnabled = true;
        }

        #endregion Report Operations Variant 1: Each batch on a separate sheet

        #region Report Operations Variant 2: All batches on a single sheet

        private void ReportSingleSheet(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                int rowIndex = 0;
                int worksheetCount = 0;
                long campaignID = Convert.ToInt32(_campaign.ItemArray[0]);
                string campaignName = _campaign.ItemArray[1].ToString();
                string campaignDescription = String.Format("{0} ({1})", _campaign.ItemArray[1], _campaign.ItemArray[2]);
                string reportSubtitle = String.Empty; //String.Format(@"{0} - {1} / {2}", );

                DataTable dtLeadStatusData;

                #region Setup excel documents

                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                string filePathAndName = String.Format("{0}Lead Search Report ({1}), {2}.xlsx", GlobalSettings.UserFolder, campaignName, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                Uri uri = new Uri("/Templates/ReportTemplateLeadSearchSinglePage.xlsx", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    wbTemplate = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }

                Worksheet wsTemplate = wbTemplate.Worksheets["Status"];
                Worksheet wsReport;
                WorksheetMergedCellsRegion mergedRegion;
                //Worksheet wsReport = wbReport.Worksheets.Add(campaignName);
                //worksheetCount++;

                wsReport = wbReport.Worksheets.Add("Lead Search Report");
                worksheetCount++;

                #endregion Setup excel documents

                #region Add the report heading from the report template - including the next 4 rows

                Methods.CopyExcelRegion(wsTemplate, rowIndex, 0, 4, 24, wsReport, rowIndex, 0);

                #endregion Add the report heading from the report template - including the next 3 rows

                #region Add the report date

                rowIndex += 2;
                mergedRegion = wsReport.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 17);
                mergedRegion.CellFormat.BottomBorderStyle = CellBorderLineStyle.None;
                mergedRegion.CellFormat.LeftBorderStyle = CellBorderLineStyle.None;
                mergedRegion.CellFormat.RightBorderStyle = CellBorderLineStyle.None;
                mergedRegion.CellFormat.TopBorderStyle = CellBorderLineStyle.None;

                mergedRegion.Value = String.Format("Report Date: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                mergedRegion.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                mergedRegion.CellFormat.Alignment = HorizontalCellAlignment.Left;
                mergedRegion.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                mergedRegion.CellFormat.WrapText = ExcelDefaultableBoolean.True;

                rowIndex++ ;
                Methods.CopyExcelRegion(wsTemplate, 5, 0, 0, 24, wsReport, rowIndex, 0);
                rowIndex++;

                #endregion Add the report date


                // Loop through each selected batch
                foreach (DataRecord record in _lstSelectedBatches)
                {
                    #region Get report data from database

                    long batchID = Convert.ToInt64(record.Cells["ID"].Value);
                    string batchDescription = record.Cells["Batch"].Value.ToString();

                    reportSubtitle = String.Format(@"{0} - {1} / {2}", campaignName, record.Cells["UDMCode"].Value, record.Cells["Code"].Value);

                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@CampaignID", campaignID);
                    parameters[1] = new SqlParameter("@BatchID", batchID);

                    DataSet dsLeadStatusData = Methods.ExecuteStoredProcedure("spINLeadSearchReport", parameters);

                    #endregion Get report data from database

                    if (dsLeadStatusData.Tables.Count < 1)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), String.Format("There is no data for the {0} campaign.", campaignName), "No Data", ShowMessageType.Information);
                        });
                    }
                    else
                    {
                        #region Report Data

                        dtLeadStatusData = dsLeadStatusData.Tables[0];

                        if (dtLeadStatusData.Rows.Count > 0)
                        {
                            //rowIndex = 7;

                            //wsReport = wbReport.Worksheets.Add(batchDescription);
                            //worksheetCount++;

                            //Methods.CopyExcelRegion(wsTemplate, 0, 0, 6, 17, wsReport, 0, 0);

                            #region Adding the details

                            //wsReport.GetCell("A3").Value = reportSubtitle;
                            //wsReport.GetCell("A4").Value = String.Format("Report Date: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            #region Adding the report subtitle

                            //mergedRegion = wsReport.MergedCellsRegions.Add(rowIndex, 0, rowIndex, 16);
                            //mergedRegion.CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
                            //mergedRegion.CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                            //mergedRegion.CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                            //mergedRegion.CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;

                            //mergedRegion.Value = reportSubtitle;

                            //mergedRegion.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                            //mergedRegion.CellFormat.Alignment = HorizontalCellAlignment.Center;
                            //mergedRegion.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
                            //mergedRegion.CellFormat.WrapText = ExcelDefaultableBoolean.True;

                            //mergedRegion.CellFormat.Font.Height = 280;

                            //rowIndex += 2;

                            #endregion Adding the report subtitle

                            #region Adding the column headings from the report template

                            //Methods.CopyExcelRegion(wsTemplate, 5, 0, 0, 17, wsReport, rowIndex, 0);
                            //++rowIndex;

                            #endregion Adding the column headings from the report template

                            #endregion Adding the details

                            foreach (DataRow dr in dtLeadStatusData.Rows)
                            {
                                rowIndex++;

                                Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, 24, wsReport, rowIndex - 1, 0);

                                //wsReport.GetCell("A" + rowIndex).Value = dr["Batch Number"].ToString();
                                //wsReport.GetCell("B" + rowIndex).Value = dr["PL Reference Number"].ToString();
                                //wsReport.GetCell("C" + rowIndex).Value = dr["Name"].ToString();
                                //wsReport.GetCell("D" + rowIndex).Value = dr["Surname"].ToString();
                                //wsReport.GetCell("E" + rowIndex).Value = dr["Option on Lead"].ToString();
                                //wsReport.GetCell("F" + rowIndex).Value = dr["Sale Status"].ToString();
                                //wsReport.GetCell("G" + rowIndex).Value = dr["Allocation Date"].ToString();
                                //wsReport.GetCell("H" + rowIndex).Value = dr["Date of Sale"].ToString();
                                //wsReport.GetCell("I" + rowIndex).Value = dr["Original Premium Sold"].ToString();
                                //wsReport.GetCell("J" + rowIndex).Value = dr["Final Premium Sold"].ToString();
                                //wsReport.GetCell("K" + rowIndex).Value = dr["Decline Status"].ToString();
                                //wsReport.GetCell("L" + rowIndex).Value = dr["Date of decline"].ToString();
                                //wsReport.GetCell("M" + rowIndex).Value = dr["Decline Reason"].ToString();
                                //wsReport.GetCell("N" + rowIndex).Value = dr["Cancellation Status"].ToString();
                                //wsReport.GetCell("O" + rowIndex).Value = dr["Date of Cancellation"].ToString();
                                //wsReport.GetCell("P" + rowIndex).Value = dr["TSR Assigned To"].ToString();
                                //wsReport.GetCell("Q" + rowIndex).Value = dr["TSR Sold By"].ToString();
                                //wsReport.GetCell("R" + rowIndex).Value = dr["Confirmation Agent"].ToString();

                                wsReport.GetCell("A" + rowIndex).Value = dr["Batch Number"].ToString();
                                wsReport.GetCell("B" + rowIndex).Value = dr["PL Reference Number"].ToString();
                                wsReport.GetCell("C" + rowIndex).Value = dr["Name"].ToString();
                                wsReport.GetCell("D" + rowIndex).Value = dr["Surname"].ToString();
                                wsReport.GetCell("E" + rowIndex).Value = dr["Language"].ToString();
                                wsReport.GetCell("F" + rowIndex).Value = dr["NumberOfTimesRemarketed"] as int?;
                                wsReport.GetCell("G" + rowIndex).Value = dr["LastDateOfRemarketing"].ToString();
                                wsReport.GetCell("H" + rowIndex).Value = dr["Option on Lead"] as int?;
                                wsReport.GetCell("I" + rowIndex).Value = dr["Sale Status"].ToString();
                                wsReport.GetCell("J" + rowIndex).Value = dr["Allocation Date"].ToString();
                                wsReport.GetCell("K" + rowIndex).Value = dr["Date of Sale"].ToString();
                                wsReport.GetCell("L" + rowIndex).Value = dr["Original Premium Sold"] as decimal?;
                                wsReport.GetCell("M" + rowIndex).Value = dr["Final Premium Sold"] as decimal?;
                                wsReport.GetCell("N" + rowIndex).Value = dr["LA2"] as int?;
                                wsReport.GetCell("O" + rowIndex).Value = dr["LA2Relationship"].ToString();
                                wsReport.GetCell("P" + rowIndex).Value = dr["Child"] as int?;
                                wsReport.GetCell("Q" + rowIndex).Value = dr["Decline Status"].ToString();
                                wsReport.GetCell("R" + rowIndex).Value = dr["Date of decline"].ToString();
                                wsReport.GetCell("S" + rowIndex).Value = dr["Decline Reason"].ToString();
                                wsReport.GetCell("T" + rowIndex).Value = dr["Cancellation Status"].ToString();
                                wsReport.GetCell("U" + rowIndex).Value = dr["Date of Cancellation"].ToString();
                                wsReport.GetCell("V" + rowIndex).Value = dr["TSR Assigned To"].ToString();
                                wsReport.GetCell("W" + rowIndex).Value = dr["TSR Sold By"].ToString();
                                wsReport.GetCell("X" + rowIndex).Value = dr["Confirmation Agent"].ToString();
                                wsReport.GetCell("Y" + rowIndex).Value = dr["Number of Referrals Generated"].ToString();
                            }
                        }

                        //rowIndex++;

                        #endregion Report Data
                    }
                }

                //Save excel document
                wbReport.Save(filePathAndName);

                //Display excel document
                Process.Start(filePathAndName);
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

        private void ReportSingleSheetCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";

            EnableAllControls(true);

            //btnReport.IsEnabled = true;
            //btnClose.IsEnabled = true;
            //IsReportRunning = false;
            //xdgCampaigns.IsEnabled = true;
            //Cal1.IsEnabled = true;
            //Cal2.IsEnabled = true;
        }

        #endregion Report Operations Variant 2: All batches on a single sheet

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;
            btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnReport.ToolTip = btnReport.Content;
        }

        private bool HasAllInputParametersBeenSpecified()
        {

            if (cmbCampaign.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select a campaign.", "No campaign selected", ShowMessageType.Error);
                EnableAllControls(true);
                return false;
            }

            //_batches = xdgBatches.Records;

            var lstTemp = (from r in xdgBatches.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            _lstSelectedBatches = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Batch"].Value));
            //_batches = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

            if (_lstSelectedBatches.Count == 0)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 batch from the list.", "No batches selected", ShowMessageType.Error);
                //EnableAllControls(true);
                return false;
            }


            //_batchIDs = _batches.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");

            //if (_batchIDs.Length > 0)
            //{
            //    _batchIDs = _batchIDs.Substring(0, _batchIDs.Length - 1);
            //}
            //else
            //{
            //    _batchIDs = String.Empty;
            //}

            _reportContentOnSinglePageSheet = chkSinglePage.IsChecked;

            return true;

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
                //_campaigns = xdgCampaigns.Records;


                //btnClose.IsEnabled = false;
                //btnReport.IsEnabled = false;
                //xdgCampaigns.IsEnabled = false;
                //Cal1.IsEnabled = false;
                //Cal2.IsEnabled = false;

                EnableAllControls(false);

                if (!HasAllInputParametersBeenSpecified())
                {
                    EnableAllControls(true);
                    return;
                }

                if (_reportContentOnSinglePageSheet != null)
                {
                    if (_reportContentOnSinglePageSheet.Value == false)
                    {
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += Report;
                        worker.RunWorkerCompleted += ReportCompleted;
                        worker.RunWorkerAsync();
                        dispatcherTimer1.Start();
                    }
                    else
                    {
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += ReportSingleSheet;
                        worker.RunWorkerCompleted += ReportSingleSheetCompleted;
                        worker.RunWorkerAsync();
                        dispatcherTimer1.Start();
                    }
                }
                else
                {
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

        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;
                DataTable dt = ((DataView)xdgBatches.DataSource).Table;

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
                //DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;
                DataTable dt = ((DataView)xdgBatches.DataSource).Table;

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

		//private void Cal1_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
		//{
		//	DateTime.TryParse(Cal1.SelectedDate.ToString(), out _fromDate);
		//	EnableDisableExportButton();
		//}

        //private void Cal2_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        //{
        //    DateTime.TryParse(Cal2.SelectedDate.ToString(), out _toDate);
        //    EnableDisableExportButton();
        //}

        private void cmbCampaign_Loaded(object sender, RoutedEventArgs e)
        {
            cmbCampaign.Focus();
        }

        private void cmbCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _campaignID = Convert.ToInt64(cmbCampaign.SelectedValue);
            _campaign = (cmbCampaign.SelectedItem as DataRowView).Row;

            try
            {
                if (cmbCampaign.SelectedIndex > -1)
                {
                    SetCursor(Cursors.Wait);

                    DataTable dt = Methods.GetTableData(String.Format("SELECT [ID], [Code] + ' (' + [UDMCode] + ')' AS [Batch], [Code], [UDMCode] FROM [INBatch] WHERE [FKINCampaignID] = {0} AND [Code] NOT LIKE 'MM%' ORDER BY [Code] DESC, [UDMCode] DESC", cmbCampaign.SelectedValue));
                    DataColumn column = new DataColumn("Select", typeof(bool));
                    column.DefaultValue = false;
                    dt.Columns.Add(column);
                    //dt.DefaultView.Sort = "UDMBatchCode ASC";

                    xdgBatches.DataSource = dt.DefaultView;
                }
                else
                {
                    xdgBatches.DataSource = null;
                }

                //IsAllRecordsSelected = false;
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

        #endregion Event Handlers

    }

}
