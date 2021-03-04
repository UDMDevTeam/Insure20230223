using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Resources;
using System.Windows.Threading;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using Orientation = Infragistics.Documents.Excel.Orientation;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportBaseConversionScreen
    {

        #region Constants

        //private string _fontName = "Calibri";
        //private const int _fontSize = 10;
        //private const int _pointsToTwipsFactor = 20;
        //private const int _fontHeight = _fontSize * _pointsToTwipsFactor;

        private const int _beneficiaryFieldCount = 10;
        private const int _maxBeneficiaryCount = 6;

        #endregion Constants

        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        private RecordCollectionBase _campaigns;
        private DateTime _fromDate = DateTime.Now;
        //private DateTime _toDate = DateTime.Now;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        private int _columnIndex = 0;

        #endregion Private Members

        #region Constructors

        public ReportBaseConversionScreen()
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
                DataColumn column = new DataColumn("Select", typeof(bool));
                column.DefaultValue = false;
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
                //if ((Cal1.SelectedDate != null)) // && (Cal2.SelectedDate != null) && (Cal2.SelectedDate >= Cal1.SelectedDate)
                //{
                    if (_xdgHeaderPrefixAreaCheckbox != null && (_xdgHeaderPrefixAreaCheckbox.IsChecked == true || _xdgHeaderPrefixAreaCheckbox.IsChecked == null))
                    {
                        btnExport.IsEnabled = true;
                        return;
                    }
                //}

                btnExport.IsEnabled = false;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void SetBatchCoverSheetWSOptions(Worksheet ws)
        {
            ws.DisplayOptions.ShowGridlines = false;
            ws.PrintOptions.PaperSize = PaperSize.A4;
            ws.PrintOptions.Orientation = Orientation.Portrait;
            ws.PrintOptions.LeftMargin = 0.6;
            ws.PrintOptions.TopMargin = 0.8;
            //ws.PrintOptions.RightMargin = 0.6;
            ws.PrintOptions.BottomMargin = 0.8;
            //ws.PrintOptions.CenterHorizontally = true;
            //ws.PrintOptions.ScalingType = ScalingType.UseScalingFactor;
            //ws.PrintOptions.ScalingFactor = 90;

            ws.DisplayOptions.MagnificationInPageLayoutView = 100;
            ws.DisplayOptions.View = WorksheetView.Normal;
        }

        private void SetBatchClientSummaryWSOptions(Worksheet ws)
        {
            ws.DisplayOptions.ShowGridlines = false;
            ws.PrintOptions.PaperSize = PaperSize.A4;
            ws.PrintOptions.Orientation = Orientation.Portrait;
            ws.PrintOptions.LeftMargin = 0.6;
            ws.PrintOptions.TopMargin = 0.8;
            //ws.PrintOptions.RightMargin = 0.2;
            ws.PrintOptions.BottomMargin = 0.8;
            //ws.PrintOptions.CenterHorizontally = true;
            //ws.PrintOptions.ScalingType = ScalingType.UseScalingFactor;
            //ws.PrintOptions.ScalingFactor = 90;

            ws.DisplayOptions.MagnificationInPageLayoutView = 100;
            ws.DisplayOptions.View = WorksheetView.Normal;
        }

        private void SetPolicyDataWSOptions(Worksheet ws)
        {
            ws.DisplayOptions.ShowGridlines = false;
            ws.PrintOptions.PaperSize = PaperSize.A4;
            ws.PrintOptions.Orientation = Orientation.Landscape;
            ws.PrintOptions.LeftMargin = 0.6;
            ws.PrintOptions.TopMargin = 0.8;
            //ws.PrintOptions.RightMargin = 0.6;
            ws.PrintOptions.BottomMargin = 0.8;
            //ws.PrintOptions.CenterHorizontally = true;
            //ws.PrintOptions.ScalingType = ScalingType.UseScalingFactor;
            //ws.PrintOptions.ScalingFactor = 90;

            ws.DisplayOptions.MagnificationInPageLayoutView = 100;
            ws.DisplayOptions.View = WorksheetView.Normal;
        }

        private void BatchExportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnExport.Content = "Export";

            btnExport.IsEnabled = true;
            btnClose.IsEnabled = true;
            xdgCampaigns.IsEnabled = true;
            //Cal1.IsEnabled = true;
            //Cal2.IsEnabled = true;
        }

        private void BatchExport(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                foreach (var campaign in _campaigns)
                {
                    var record = (DataRecord) campaign;
                    if ((bool)record.Cells["Select"].Value)
                    {
                        long campaignID = Convert.ToInt32(record.Cells["CampaignID"].Value);
                        string campaignName = record.Cells["CampaignName"].Value.ToString();

                        #region Setup excel documents

                        Workbook wbTemplate;
                        Workbook wbBatchExport = new Workbook(WorkbookFormat.Excel2007);
                        string filePathAndName = GlobalSettings.UserFolder + campaignName + " Batch Export ~ " + DateTime.Now.Millisecond + ".xlsx";

                        Uri uri = new Uri("/Templates/ReportTemplateBatchExport.xlsx", UriKind.Relative);
                        StreamResourceInfo info = Application.GetResourceStream(uri);
                        if (info != null)
                        {
                            wbTemplate = Workbook.Load(info.Stream, true);
                        }
                        else
                        {
                            return;
                        }

                        Worksheet wsBatchCoverSheetTemplate = wbTemplate.Worksheets["Batch Cover Sheet"];
                        Worksheet wsBatchClientSummaryTemplate = wbTemplate.Worksheets["Batch Client Summary"];
                        Worksheet wsPolicyDataTemplate = wbTemplate.Worksheets["Policy Data"];

                        Worksheet wsBatchCoverSheet = wbBatchExport.Worksheets.Add("Batch Cover Sheet");
                        Worksheet wsBatchClientSummary = wbBatchExport.Worksheets.Add("Batch Client Summary");
                        Worksheet wsPolicyData = wbBatchExport.Worksheets.Add("Policy Data");

                        SetBatchCoverSheetWSOptions(wsBatchCoverSheet);
                        SetBatchClientSummaryWSOptions(wsBatchClientSummary);
                        SetPolicyDataWSOptions(wsPolicyData);

                        #endregion Setup excel documents

                        #region Get batch export data from database

                        DataTable dtBatchExportData;

                        SqlParameter[] parameters = new SqlParameter[2];
                        parameters[0] = new SqlParameter("@CampaignID", campaignID);
                        parameters[1] = new SqlParameter("@DateOfSale", _fromDate.ToString("yyyy-MM-dd"));
                        //parameters[2] = new SqlParameter("@ToDate", _fromDate.ToString("yyyy-MM-dd"));

                        DataSet dsBatchExportData = Methods.ExecuteStoredProcedure("spINReportBatchExport", parameters);
                        if (dsBatchExportData.Tables.Count > 0)
                        {
                            dtBatchExportData = dsBatchExportData.Tables[0];

                            if (dtBatchExportData.Rows.Count == 0)
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

                        #endregion Get batch export data from database

                        #region Batch Cover Page

                        wsBatchCoverSheet.DisplayOptions.TabColorInfo = wsBatchCoverSheetTemplate.DisplayOptions.TabColorInfo;
                        Methods.CopyExcelRegion(wsBatchCoverSheetTemplate, 0, 0, 45, 24, wsBatchCoverSheet, 0, 0);

                        // Get the required data.
                        DataTable coverPageTable = dsBatchExportData.Tables[0];

                        if (coverPageTable != null)
                        {
                            if (coverPageTable.Rows.Count > 0)
                            {
                                DataRow coverPageRow = coverPageTable.Rows[0];

                                // Assign values to excell sheet.
                                WorksheetCell wsCoverCell = wsBatchCoverSheet.Rows[18].Cells[1];
                                wsCoverCell.Value = coverPageRow["Campaign"];

                                wsCoverCell = wsBatchCoverSheet.Rows[22].Cells[1];
                                wsCoverCell.Value = coverPageRow["CampaignCode"];

                                wsCoverCell = wsBatchCoverSheet.Rows[26].Cells[1];
                                wsCoverCell.Value = coverPageRow["BatchNumber"];

                                wsCoverCell = wsBatchCoverSheet.Rows[30].Cells[1];
                                wsCoverCell.Value = coverPageRow["DateOfSale"];

                                wsCoverCell = wsBatchCoverSheet.Rows[37].Cells[12];
                                wsCoverCell.Value = coverPageRow["TotalPolicies"];

                                wsCoverCell = wsBatchCoverSheet.Rows[39].Cells[12];
                                wsCoverCell.Value = coverPageRow["TotalUnits"];

                                wsCoverCell = wsBatchCoverSheet.Rows[41].Cells[12];
                                wsCoverCell.Value = coverPageRow["TotalPremiums"];

                                wsCoverCell = wsBatchCoverSheet.Rows[43].Cells[12];
                                wsCoverCell.Value = coverPageRow["AveragePremium"];
                            }
                        }

                        #endregion

                        #region Batch Client summary

                        wsBatchClientSummary.DisplayOptions.View = WorksheetView.PageLayout;
                        wsBatchClientSummary.DisplayOptions.TabColorInfo = wsBatchClientSummaryTemplate.DisplayOptions.TabColorInfo;

                        //Only copy until row 20. Row 21 will be repeatedly copied and inserted:
                        Methods.CopyExcelRegion(wsBatchClientSummaryTemplate, 0, 0, 19, 22, wsBatchClientSummary, 0, 0);

                        // Get the required data.                        
                        if (coverPageTable != null)
                        {
                            if (coverPageTable.Rows.Count > 0)
                            {
                                DataRow coverPageRow = coverPageTable.Rows[0];


                                // Assign values to excell sheet.
                                WorksheetCell wsCoverCell = wsBatchClientSummary.Rows[11].Cells[12];
                                wsCoverCell.Value = coverPageRow["Campaign"];

                                wsCoverCell = wsBatchClientSummary.Rows[13].Cells[12];
                                wsCoverCell.Value = coverPageRow["CampaignCode"];

                                wsCoverCell = wsBatchClientSummary.Rows[15].Cells[12];
                                wsCoverCell.Value = coverPageRow["BatchNumber"];

                                wsCoverCell = wsBatchClientSummary.Rows[17].Cells[12];
                                wsCoverCell.Value = coverPageRow["DateOfSale"];

                                // Get summary table.
                                DataTable summaryPageTable = dsBatchExportData.Tables[1];
                                if (summaryPageTable != null)
                                {
                                    if (summaryPageTable.Rows.Count > 0)
                                    {
                                        int count = 0;

                                        foreach (DataRow summaryRow in summaryPageTable.Rows)
                                        {
                                            Methods.CopyExcelRegion(wsBatchClientSummaryTemplate, 20, 2, 0, 19, wsBatchClientSummary, (20 + count), 2);

                                            // Row index
                                            wsCoverCell = wsBatchClientSummary.Rows[20 + count].Cells[2];
                                            wsCoverCell.Value = (count + 1).ToString();

                                            wsCoverCell = wsBatchClientSummary.Rows[20 + count].Cells[4];
                                            wsCoverCell.Value = summaryRow["RefNo"];

                                            wsCoverCell = wsBatchClientSummary.Rows[20 + count].Cells[10];
                                            wsCoverCell.Value = summaryRow["Surname"];

                                            wsCoverCell = wsBatchClientSummary.Rows[20 + count].Cells[16];
                                            wsCoverCell.Value = summaryRow["IDNumber"];

                                            count++;
                                        }
                                    }
                                }
                            }
                        }


                        #endregion Batch Client summary

                        #region Policy Data

                        wsPolicyData.DisplayOptions.TabColorInfo = wsPolicyDataTemplate.DisplayOptions.TabColorInfo;
                        Methods.CopyExcelRegion(wsPolicyDataTemplate, 0, 0, 3, 221, wsPolicyData, 0, 0);

                        //add table - filters
                        WorksheetRegion regionTable = new WorksheetRegion(wsPolicyData, 2, 0, 1000, 220);
                        WorksheetTable table = regionTable.FormatAsTable(true);
                        table.Name = "Batch";

                        //WorksheetTableStyle wsTableStyle = wsPolicyData.Workbook.StandardTableStyles["TableStyleLight15"].Clone("BatchExport");
                        //wsTableStyle.AreaFormats[0].BottomBorderStyle = CellBorderLineStyle.Thick;
                        //wsTableStyle.AreaFormats[0].BottomBorderColorInfo = new WorkbookColorInfo(System.Drawing.Color.Red);
                        //wsTableStyle.AreaFormats[0].TopBorderStyle = CellBorderLineStyle.Thick;
                        //wsPolicyData.Workbook.CustomTableStyles.Add(wsTableStyle);

                        //table.Style = wsPolicyData.Workbook.CustomTableStyles["BatchExport"];
                        table.Style = wsPolicyData.Workbook.StandardTableStyles["TableStyleLight15"];
                        table.DisplayBandedRows = false;

                        DataTable policyTable = dsBatchExportData.Tables[2];
                        //DataTable benificiaryTable = dsBatchExportData.Tables[3];

                        WorksheetCell wsPolicyCell;
                        if (policyTable != null)
                        {
                            if (policyTable.Rows.Count > 0)
                            {
                                int rowCount = 0;
                                foreach (DataRow polRow in policyTable.Rows)
                                {
                                    for (int b = 0; b < policyTable.Columns.Count; b++)
                                    {
                                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[b];
                                        wsPolicyCell.Value = polRow[b];

                                        ////Formatting
                                        //switch (b)
                                        //{
                                        //    case 1
                                        //        wsPolicyCell.CellFormat.FormatString = "\"$\"#,##0.00";
                                        //}
                                    }

                                    // Update the row counter:
                                    rowCount++;

                                    #region THE VERY LONG WAY!!!

                                    //#region UDM Details

                                    //// Campaign Code
                                    //WorksheetCell wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[_columnIndex];
                                    //wsPolicyCell.Value = polRow["CampaignCode"];
                                    //++_columnIndex;

                                    //// Lead Date
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[1];
                                    //wsPolicyCell.Value = polRow["LeadDate"];
                                    //++_columnIndex;

                                    //// Source
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[2];
                                    //wsPolicyCell.Value = polRow["Source"];
                                    //++_columnIndex;

                                    //// Disc Ref
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[3];
                                    //wsPolicyCell.Value = polRow["DiscRef"];

                                    //// UDM Ref
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[4];
                                    //wsPolicyCell.Value = polRow["UDMRef"];
                                    //++_columnIndex;

                                    //// Category
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[5];
                                    //wsPolicyCell.Value = polRow["Category"];
                                    //++_columnIndex;

                                    //// Referror
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[6];
                                    //wsPolicyCell.Value = polRow["Referror"];
                                    //++_columnIndex;

                                    //// Relationship
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[7];
                                    //wsPolicyCell.Value = polRow["Relationship"];
                                    //++_columnIndex;

                                    //// 1TSR
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[8];
                                    //wsPolicyCell.Value = polRow["1TSR"];
                                    //++_columnIndex;

                                    //// 2TSR
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[9];
                                    //wsPolicyCell.Value = polRow["2TSR"];
                                    //++_columnIndex;

                                    //// 2Date
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[10];
                                    //wsPolicyCell.Value = polRow["2Date"];
                                    //++_columnIndex;

                                    //// UDMNotes
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[11];
                                    //wsPolicyCell.Value = polRow["UDMNotes"];
                                    //++_columnIndex;

                                    //// Notes
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[12];
                                    //wsPolicyCell.Value = polRow["Notes"];
                                    //++_columnIndex;

                                    //// Client Ref Nr
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[13];
                                    //wsPolicyCell.Value = polRow["ClientRefNr"];
                                    //++_columnIndex;

                                    //// Policy Nr
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[14];
                                    //wsPolicyCell.Value = polRow["PolicyNr"];
                                    //++_columnIndex;

                                    //// Commence Date                                    
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[15];
                                    //wsPolicyCell.Value = polRow["CommenceDate"];
                                    //++_columnIndex;

                                    //// Alteration Date                            
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[16];
                                    //wsPolicyCell.Value = polRow["AlterationDate"];
                                    //++_columnIndex;

                                    //// Date of Sale                            
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[17];
                                    //wsPolicyCell.Value = polRow["DateOfSale"];
                                    //++_columnIndex;

                                    //// Old Product ID                            
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[18];
                                    //wsPolicyCell.Value = polRow["OldProductID"];
                                    //++_columnIndex;

                                    //// Auto Include MB                            
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[19];
                                    //wsPolicyCell.Value = polRow["AutoIncludeMB"];
                                    //++_columnIndex;

                                    //#endregion UDM Details

                                    //#region Policy Owner

                                    //// Gender                                    
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[20];
                                    //wsPolicyCell.Value = polRow["Gender"];
                                    //++_columnIndex;

                                    //// POTitle                            
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[21];
                                    //wsPolicyCell.Value = polRow["POTitle"];
                                    //++_columnIndex;

                                    //// POInitials                            
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[22];
                                    //wsPolicyCell.Value = polRow["POInitials"];
                                    //++_columnIndex;

                                    //// POFirst Name
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[23];
                                    //wsPolicyCell.Value = polRow["POFirstName"];
                                    //++_columnIndex;

                                    //// POSurname
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[24];
                                    //wsPolicyCell.Value = polRow["POSurname"];
                                    //++_columnIndex;

                                    //// POID Number
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[25];
                                    //wsPolicyCell.Value = polRow["POIDNumber"];
                                    //++_columnIndex;

                                    //// PODate of Birth
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[26];
                                    //wsPolicyCell.Value = polRow["PODateOfBirth"];
                                    //++_columnIndex;

                                    //// Age
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[27];
                                    //wsPolicyCell.Value = polRow["Age"];
                                    //++_columnIndex;

                                    //// POAddress1
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[28];
                                    //wsPolicyCell.Value = polRow["POAddress1"];
                                    //++_columnIndex;

                                    //// POAddress2
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[29];
                                    //wsPolicyCell.Value = polRow["POAddress2"];
                                    //++_columnIndex;

                                    //// POAddress3
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[30];
                                    //wsPolicyCell.Value = polRow["POAddress3"];
                                    //++_columnIndex;

                                    //// POAddress4
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[31];
                                    //wsPolicyCell.Value = polRow["POAddress4"];
                                    //++_columnIndex;

                                    //// POAreaCode
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[32];
                                    //wsPolicyCell.Value = polRow["POAreaCode"];
                                    //++_columnIndex;

                                    //// POWorkTel
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[33];
                                    //wsPolicyCell.Value = polRow["POWorkTel"];
                                    //++_columnIndex;

                                    //// POHomeTel
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[34];
                                    //wsPolicyCell.Value = polRow["POHomeTel"];
                                    //++_columnIndex;

                                    //// POCellTel
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[35];
                                    //wsPolicyCell.Value = polRow["POCellTel"];
                                    //++_columnIndex;

                                    //// POEmail
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[36];
                                    //wsPolicyCell.Value = polRow["POEmail"];
                                    //++_columnIndex;

                                    //#endregion Policy Owner
                                    
                                    //#region Premium Payer

                                    //// PPGender
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[37];
                                    //wsPolicyCell.Value = polRow["PPGender"];
                                    //++_columnIndex;

                                    //// PPTitle
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[38];
                                    //wsPolicyCell.Value = polRow["PPTitle"];
                                    //++_columnIndex;

                                    //// PPInitials
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[39];
                                    //wsPolicyCell.Value = polRow["PPInitials"];
                                    //++_columnIndex;

                                    //// PPFirstName
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[40];
                                    //wsPolicyCell.Value = polRow["PPFirstName"];
                                    //++_columnIndex;

                                    //// PPSurname
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[41];
                                    //wsPolicyCell.Value = polRow["PPSurname"];
                                    //++_columnIndex;

                                    //// PPIDNumber
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[42];
                                    //wsPolicyCell.Value = polRow["PPIDNumber"];
                                    //++_columnIndex;

                                    //// PPDateOfBirth
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[43];
                                    //wsPolicyCell.Value = polRow["PPDateOfBirth"];
                                    //++_columnIndex;

                                    //// PPWorkTel
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[44];
                                    //wsPolicyCell.Value = polRow["PPWorkTel"];
                                    //++_columnIndex;

                                    //// PPHomeTel
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[45];
                                    //wsPolicyCell.Value = polRow["PPHomeTel"];
                                    //++_columnIndex;

                                    //// PPCellTel
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[46];
                                    //wsPolicyCell.Value = polRow["PPCellTel"];
                                    //++_columnIndex;

                                    //#endregion Premium Payer

                                    //#region Banking Details

                                    //// ContractPremium
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[47];
                                    //wsPolicyCell.Value = polRow["ContractPremium"];
                                    //++_columnIndex;

                                    //// BankName
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[48];
                                    //wsPolicyCell.Value = polRow["BankName"];
                                    //++_columnIndex;

                                    //// BranchName
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[49];
                                    //wsPolicyCell.Value = polRow["BranchName"];
                                    //++_columnIndex;

                                    //// BranchCode
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[50];
                                    //wsPolicyCell.Value = polRow["BranchCode"];
                                    //++_columnIndex;

                                    //// AccountType
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[51];
                                    //wsPolicyCell.Value = polRow["AccountType"];
                                    //++_columnIndex;

                                    //// AccountNumber
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[52];
                                    //wsPolicyCell.Value = polRow["AccountNumber"];
                                    //++_columnIndex;

                                    //// DebitDay
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[53];
                                    //wsPolicyCell.Value = polRow["DebitDay"];
                                    //++_columnIndex;

                                    //#endregion Banking Details

                                    //#region Component 1: Basic Component

                                    //// CNCompID
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[54];
                                    //wsPolicyCell.Value = polRow["CNCompID"];
                                    //++_columnIndex;

                                    //// CNPrem
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[55];
                                    //wsPolicyCell.Value = polRow["CNPrem"];
                                    //++_columnIndex;

                                    //// CNCover
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[56];
                                    //wsPolicyCell.Value = polRow["CNCover"];
                                    //++_columnIndex;

                                    //// CNTerm
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[57];
                                    //wsPolicyCell.Value = polRow["CNTerm"];
                                    //++_columnIndex;

                                    //// CNUnits
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[58];
                                    //wsPolicyCell.Value = polRow["CNUnits"];
                                    //++_columnIndex;

                                    //// CNPGValue
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[59];
                                    //wsPolicyCell.Value = polRow["CNPGValue"];
                                    //++_columnIndex;

                                    //// CNCG Perc
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[60];
                                    //wsPolicyCell.Value = polRow["CNCGPerc"];
                                    //++_columnIndex;

                                    //#endregion Component 1: Basic Component

                                    //#region Component 2: Money Back Component

                                    //// MBCompID
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[61];
                                    //wsPolicyCell.Value = polRow["MBCompID"];
                                    //++_columnIndex;

                                    //// MBPrem
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[62];
                                    //wsPolicyCell.Value = polRow["MBPrem"];
                                    //++_columnIndex;

                                    //// MBCover
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[63];
                                    //wsPolicyCell.Value = polRow["MBCover"];
                                    //++_columnIndex;

                                    //// MBTerm
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[64];
                                    //wsPolicyCell.Value = polRow["MBTerm"];
                                    //++_columnIndex;

                                    //// MBUnits
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[65];
                                    //wsPolicyCell.Value = polRow["MBUnits"];
                                    //++_columnIndex;

                                    //#endregion Component 2: Money Back Component

                                    //#region Life Assured 1 Components

                                    //#region Client Details
                                    //// LA1Title
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[66];
                                    //wsPolicyCell.Value = polRow["LA1Title"];
                                    //++_columnIndex;

                                    //// LA1Initials
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[67];
                                    //wsPolicyCell.Value = polRow["LA1Initials"];
                                    //++_columnIndex;

                                    //// LA1FirstName
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[68];
                                    //wsPolicyCell.Value = polRow["LA1FirstName"];
                                    //++_columnIndex;

                                    //// LA1Surname
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[69];
                                    //wsPolicyCell.Value = polRow["LA1Surname"];
                                    //++_columnIndex;

                                    //// LA1IDNumber
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[70];
                                    //wsPolicyCell.Value = polRow["LA1IDNumber"];
                                    //++_columnIndex;

                                    //// LA1DateOfBirth
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[71];
                                    //wsPolicyCell.Value = polRow["LA1DateOfBirth"];
                                    //++_columnIndex;

                                    //// LA1Age
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[72];
                                    //wsPolicyCell.Value = polRow["LA1Age"];
                                    //++_columnIndex;

                                    //// LA1WorkTel
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[73];
                                    //wsPolicyCell.Value = polRow["LA1WorkTel"];
                                    //++_columnIndex;

                                    //// LA1HomeTel
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[74];
                                    //wsPolicyCell.Value = polRow["LA1HomeTel"];
                                    //++_columnIndex;

                                    //// LA1CellTel
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[75];
                                    //wsPolicyCell.Value = polRow["LA1CellTel"];
                                    //++_columnIndex;

                                    //// LA1Gender
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[76];
                                    //wsPolicyCell.Value = polRow["LA1Gender"];
                                    //++_columnIndex;

                                    //#endregion Client Details

                                    //#region LA1 Cancer

                                    //// LA1CompID68
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[77];
                                    //wsPolicyCell.Value = polRow["LA1CompID68"];
                                    //++_columnIndex;

                                    //// LA1Prem68
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[78];
                                    //wsPolicyCell.Value = polRow["LA1Prem68"];
                                    //++_columnIndex;

                                    //// LA1Cover68
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[79];
                                    //wsPolicyCell.Value = polRow["LA1Cover68"];
                                    //++_columnIndex;

                                    //// LA1Term68
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[80];
                                    //wsPolicyCell.Value = polRow["LA1Term68"];
                                    //++_columnIndex;

                                    //// LA1Units68
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[81];
                                    //wsPolicyCell.Value = polRow["LA1Units68"];
                                    //++_columnIndex;

                                    //#endregion LA1 Cancer

                                    //#region LA1 Death
                                    //// LA1CompID632
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[82];
                                    //wsPolicyCell.Value = polRow["LA1CompID632"];
                                    //++_columnIndex;

                                    //// LA1Prem632
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[83];
                                    //wsPolicyCell.Value = polRow["LA1Prem632"];
                                    //++_columnIndex;

                                    //// LA1Cover632
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[84];
                                    //wsPolicyCell.Value = polRow["LA1Cover632"];
                                    //++_columnIndex;

                                    //// LA1Term632
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[85];
                                    //wsPolicyCell.Value = polRow["LA1Term632"];
                                    //++_columnIndex;

                                    //// LA1Units632
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[86];
                                    //wsPolicyCell.Value = polRow["LA1Units632"];
                                    //++_columnIndex;

                                    //#endregion LA1 Death

                                    //#region LA1 Disability

                                    //// LA1CompID637
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[87];
                                    //wsPolicyCell.Value = polRow["LA1CompID637"];
                                    //++_columnIndex;

                                    //// LA1Prem637
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[88];
                                    //wsPolicyCell.Value = polRow["LA1Prem637"];
                                    //++_columnIndex;

                                    //// LA1Cover637
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[89];
                                    //wsPolicyCell.Value = polRow["LA1Cover637"];
                                    //++_columnIndex;

                                    //// LA1Term637
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[90];
                                    //wsPolicyCell.Value = polRow["LA1Term637"];
                                    //++_columnIndex;

                                    //// LA1Units637
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[91];
                                    //wsPolicyCell.Value = polRow["LA1Units637"];
                                    //++_columnIndex;

                                    //#endregion LA1 Disability

                                    //#region LA1 Funeral

                                    //// LA1CompID635
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[92];
                                    //wsPolicyCell.Value = polRow["LA1CompID635"];
                                    //++_columnIndex;

                                    //// LA1Prem635
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[93];
                                    //wsPolicyCell.Value = polRow["LA1Prem635"];
                                    //++_columnIndex;

                                    //// LA1Cover635
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[94];
                                    //wsPolicyCell.Value = polRow["LA1Cover635"];
                                    //++_columnIndex;

                                    //// LA1Term635
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[95];
                                    //wsPolicyCell.Value = polRow["LA1Term635"];
                                    //++_columnIndex;

                                    //// LA1Units635
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[96];
                                    //wsPolicyCell.Value = polRow["LA1Units635"];
                                    //++_columnIndex;

                                    //#endregion LA1 Funeral

                                    //#region Kids Component

                                    //// LA1CompID687
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[97];
                                    //wsPolicyCell.Value = polRow["LA1CompID687"];
                                    //++_columnIndex;

                                    //// LA1Prem687
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[98];
                                    //wsPolicyCell.Value = polRow["LA1Prem687"];
                                    //++_columnIndex;

                                    //// LA1Cover687
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[99];
                                    //wsPolicyCell.Value = polRow["LA1Cover687"];
                                    //++_columnIndex;

                                    //// LA1Term687
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[100];
                                    //wsPolicyCell.Value = polRow["LA1Term687"];
                                    //++_columnIndex;

                                    //// LA1Units687
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[101];
                                    //wsPolicyCell.Value = polRow["LA1Units687"];
                                    //++_columnIndex;

                                    //#endregion Kids Component

                                    //#region LA1 Future Component

                                    //// LA1CompID 
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[102];
                                    //wsPolicyCell.Value = polRow["LA1CompID"];
                                    //++_columnIndex;

                                    //// LA1Prem 
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[103];
                                    //wsPolicyCell.Value = polRow["LA1Prem"];
                                    //++_columnIndex;

                                    //// LA1Cover 
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[104];
                                    //wsPolicyCell.Value = polRow["LA1Cover"];
                                    //++_columnIndex;

                                    //// LA1Term 
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[105];
                                    //wsPolicyCell.Value = polRow["LA1Term"];
                                    //++_columnIndex;

                                    //// LA1Units 
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[106];
                                    //wsPolicyCell.Value = polRow["LA1Units"];
                                    //++_columnIndex;

                                    //#endregion LA1 Future Component

                                    //#endregion Life Assured 1 Components

                                    //#region Life Assured 2 Components

                                    //#region Client Info

                                    //// LA2Title
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[107];
                                    //wsPolicyCell.Value = polRow["LA2Title"];
                                    //++_columnIndex;

                                    //// LA2Initials
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[108];
                                    //wsPolicyCell.Value = polRow["LA2Initials"];
                                    //++_columnIndex;

                                    //// LA2FirstName
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[109];
                                    //wsPolicyCell.Value = polRow["LA2FirstName"];
                                    //++_columnIndex;

                                    //// LA2Surname
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[110];
                                    //wsPolicyCell.Value = polRow["LA2Surname"];
                                    //++_columnIndex;

                                    //// LA2IDNumber
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[111];
                                    //wsPolicyCell.Value = polRow["LA2IDNumber"];
                                    //++_columnIndex;

                                    //// LA2DateOfBirth
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[112];
                                    //wsPolicyCell.Value = polRow["LA2DateOfBirth"];
                                    //++_columnIndex;

                                    //// LA2Age
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[113];
                                    //wsPolicyCell.Value = polRow["LA2Age"];
                                    //++_columnIndex;

                                    //// LA2WorkTel
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[114];
                                    //wsPolicyCell.Value = polRow["LA2WorkTel"];
                                    //++_columnIndex;

                                    //// LA2HomeTel
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[115];
                                    //wsPolicyCell.Value = polRow["LA2HomeTel"];
                                    //++_columnIndex;

                                    //// LA2CellTel
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[116];
                                    //wsPolicyCell.Value = polRow["LA2CellTel"];
                                    //++_columnIndex;

                                    //// LA2Gender
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[117];
                                    //wsPolicyCell.Value = polRow["LA2Gender"];
                                    //++_columnIndex;

                                    //#endregion Client Info

                                    //#region LA2 Cancer

                                    //// LA2CompID68
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[118];
                                    //wsPolicyCell.Value = polRow["LA2CompID68"];
                                    //++_columnIndex;

                                    //// LA2Prem68
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[119];
                                    //wsPolicyCell.Value = polRow["LA2Prem68"];
                                    //++_columnIndex;

                                    //// LA2Cover68
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[120];
                                    //wsPolicyCell.Value = polRow["LA2Cover68"];
                                    //++_columnIndex;

                                    //// LA2Term68
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[121];
                                    //wsPolicyCell.Value = polRow["LA2Term68"];
                                    //++_columnIndex;

                                    //// LA2Units68
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[122];
                                    //wsPolicyCell.Value = polRow["LA2Units68"];
                                    //++_columnIndex;

                                    //#endregion LA2 Cancer

                                    //#region LA2 Death
                                    //// LA2CompID632
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[123];
                                    //wsPolicyCell.Value = polRow["LA2CompID632"];
                                    //++_columnIndex;

                                    //// LA2Prem632
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[124];
                                    //wsPolicyCell.Value = polRow["LA2Prem632"];
                                    //++_columnIndex;

                                    //// LA2Cover632
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[125];
                                    //wsPolicyCell.Value = polRow["LA2Cover632"];
                                    //++_columnIndex;

                                    //// LA2Term632
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[126];
                                    //wsPolicyCell.Value = polRow["LA2Term632"];
                                    //++_columnIndex;

                                    //// LA2Units632
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[127];
                                    //wsPolicyCell.Value = polRow["LA2Units632"];
                                    //++_columnIndex;

                                    //#endregion LA2 Death

                                    //#region LA2 Disability

                                    //// LA2CompID637
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[128];
                                    //wsPolicyCell.Value = polRow["LA2CompID637"];
                                    //++_columnIndex;

                                    //// LA2Prem637
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[129];
                                    //wsPolicyCell.Value = polRow["LA2Prem637"];
                                    //++_columnIndex;

                                    //// LA2Cover637
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[130];
                                    //wsPolicyCell.Value = polRow["LA2Cover637"];
                                    //++_columnIndex;

                                    //// LA2Term637
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[131];
                                    //wsPolicyCell.Value = polRow["LA2Term637"];
                                    //++_columnIndex;

                                    //// LA2Units637
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[132];
                                    //wsPolicyCell.Value = polRow["LA2Units637"];
                                    //++_columnIndex;

                                    //#endregion LA2 Disability

                                    //#region LA2 Funeral
                                    //// LA2CompID635
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[133];
                                    //wsPolicyCell.Value = polRow["LA2CompID635"];
                                    //++_columnIndex;

                                    //// LA2Prem635
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[134];
                                    //wsPolicyCell.Value = polRow["LA2Prem635"];
                                    //++_columnIndex;

                                    //// LA2Cover635
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[135];
                                    //wsPolicyCell.Value = polRow["LA2Cover635"];
                                    //++_columnIndex;

                                    //// LA2Term635
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[136];
                                    //wsPolicyCell.Value = polRow["LA2Term635"];
                                    //++_columnIndex;

                                    //// LA2Units635
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[137];
                                    //wsPolicyCell.Value = polRow["LA2Units635"];
                                    //++_columnIndex;

                                    //#endregion LA2 Funeral

                                    //#region LA2 Future Component

                                    //// LA2CompID
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[138];
                                    //wsPolicyCell.Value = polRow["LA2CompID"];
                                    //++_columnIndex;

                                    //// LA2Prem 
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[139];
                                    //wsPolicyCell.Value = polRow["LA2Prem"];
                                    //++_columnIndex;

                                    //// LA2Cover 
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[140];
                                    //wsPolicyCell.Value = polRow["LA2Cover"];
                                    //++_columnIndex;

                                    //// LA2Term 
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[141];
                                    //wsPolicyCell.Value = polRow["LA2Term"];
                                    //++_columnIndex;

                                    //// LA2Units 
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[142];
                                    //wsPolicyCell.Value = polRow["LA2Units"];
                                    //++_columnIndex;

                                    //#endregion LA2 Future Component

                                    //#endregion Life Assured 2 Components

                                    //#region Beneficiaries

                                    //#region OLD
                                    //// NOTE:
                                    //// The 4th table returned by the dataSet contains benficiary data.
                                    //// We will find all rows that has a INPolicy.ID matching polRow["PolicyID"].
                                    //// Depending on their order we will add them to ben1, ben2, ben3, ben4.

                                    ////if (benificiaryTable != null)
                                    ////{
                                    ////    if (benificiaryTable.Rows.Count > 0)
                                    ////    {
                                    ////        int benRowCount = 0;
                                    ////        foreach (DataRow benRow in benificiaryTable.Rows)
                                    ////        {
                                    ////            if (benRow["PolicyID"].ToString() == polRow["PolicyID"].ToString())
                                    ////            {
                                    ////                benRowCount++;

                                    ////                switch (benRowCount)
                                    ////                {
                                    ////                    case 1:
                                    ////                        #region BN1

                                    ////                        // BN1Title
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[143];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN1Initials
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[144];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN1FirstName
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[145];
                                    ////                        wsPolicyCell.Value = benRow["FirstName"];

                                    ////                        // BN1Surname
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[146];
                                    ////                        wsPolicyCell.Value = benRow["Surname"];

                                    ////                        // BN1IDNumber
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[147];
                                    ////                        wsPolicyCell.Value = benRow["IDNo"];

                                    ////                        // BN1DateOfBirth
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[148];
                                    ////                        wsPolicyCell.Value = benRow["DateOfBirth"];

                                    ////                        // BN1Gender
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[149];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN1Relation
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[150];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN1ClientNature
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[151];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN1Percentage
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[152];
                                    ////                        wsPolicyCell.Value = benRow["Percentage"];

                                    ////                        #endregion
                                    ////                        break;
                                    ////                    case 2:
                                    ////                        #region BN2

                                    ////                        // BN2Title
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[153];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN2Initials
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[154];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN2FirstName
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[155];
                                    ////                        wsPolicyCell.Value = benRow["FirstName"];

                                    ////                        // BN2Surname
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[156];
                                    ////                        wsPolicyCell.Value = benRow["Surname"];

                                    ////                        // BN2IDNumber
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[157];
                                    ////                        wsPolicyCell.Value = benRow["IDNo"];

                                    ////                        // BN2DateOfBirth
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[158];
                                    ////                        wsPolicyCell.Value = benRow["DateOfBirth"];

                                    ////                        // BN2Gender
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[159];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN2Relation
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[160];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN3ClientNature
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[161];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN2Percentage
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[162];
                                    ////                        wsPolicyCell.Value = benRow["Percentage"];

                                    ////                        #endregion
                                    ////                        break;
                                    ////                    case 3:
                                    ////                        #region BN3

                                    ////                        // BN3Title
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[163];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN3Initials
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[164];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN3FirstName
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[165];
                                    ////                        wsPolicyCell.Value = benRow["FirstName"];

                                    ////                        // BN3Surname
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[166];
                                    ////                        wsPolicyCell.Value = benRow["Surname"];

                                    ////                        // BN3IDNumber
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[167];
                                    ////                        wsPolicyCell.Value = benRow["IDNo"];

                                    ////                        // BN3DateOfBirth
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[168];
                                    ////                        wsPolicyCell.Value = benRow["DateOfBirth"];

                                    ////                        // BN3Gender
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[169];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN3Relation
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[170];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN3ClientNature
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[171];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN3Percentage
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[172];
                                    ////                        wsPolicyCell.Value = benRow["Percentage"];

                                    ////                        #endregion
                                    ////                        break;
                                    ////                    case 4:
                                    ////                        #region BN4

                                    ////                        // BN4Title
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[173];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN4Initials
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[174];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN4FirstName
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[175];
                                    ////                        wsPolicyCell.Value = benRow["FirstName"];

                                    ////                        // BN4Surname
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[176];
                                    ////                        wsPolicyCell.Value = benRow["Surname"];

                                    ////                        // BN4IDNumber
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[177];
                                    ////                        wsPolicyCell.Value = benRow["IDNo"];

                                    ////                        // BN4DateOfBirth
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[178];
                                    ////                        wsPolicyCell.Value = benRow["DateOfBirth"];

                                    ////                        // BN4Gender
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[179];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN4Relation
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[180];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN4ClientNature
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[181];
                                    ////                        wsPolicyCell.Value = "";

                                    ////                        // BN4Percentage
                                    ////                        wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[182];
                                    ////                        wsPolicyCell.Value = benRow["Percentage"];

                                    ////                        #endregion
                                    ////                        break;
                                                            
                                    ////                    default:
                                    ////                        // NOTE:
                                    ////                        // More than 4 beneficiaries was added
                                    ////                        // Export template only caters for 4.
                                    ////                        break;
                                    ////                }
                                    ////            }
                                    ////        }
                                    ////    }
                                    ////}

                                    //#endregion OLD

                                    //for (int a = 1; a <= _maxBeneficiaryCount; a++)
                                    //{
                                    //    wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[_columnIndex];
                                    //    wsPolicyCell.Value = polRow[String.Format("Beneficiary{0}Title", a)];
                                    //    ++_columnIndex;

                                    //    wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[_columnIndex];
                                    //    wsPolicyCell.Value = polRow[String.Format("Beneficiary{0}Initials", a)];
                                    //    ++_columnIndex;

                                    //    wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[_columnIndex];
                                    //    wsPolicyCell.Value = polRow[String.Format("Beneficiary{0}FirstName", a)];
                                    //    ++_columnIndex;

                                    //    wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[_columnIndex];
                                    //    wsPolicyCell.Value = polRow[String.Format("Beneficiary{0}Surname", a)];
                                    //    ++_columnIndex;

                                    //    wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[_columnIndex];
                                    //    wsPolicyCell.Value = polRow[String.Format("Beneficiary{0}IDNumber", a)];
                                    //    ++_columnIndex;

                                    //    wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[_columnIndex];
                                    //    wsPolicyCell.Value = polRow[String.Format("Beneficiary{0}DateOfBirth", a)];
                                    //    ++_columnIndex;

                                    //    wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[_columnIndex];
                                    //    wsPolicyCell.Value = polRow[String.Format("Beneficiary{0}Gender", a)];
                                    //    ++_columnIndex;

                                    //    wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[_columnIndex];
                                    //    wsPolicyCell.Value = polRow[String.Format("Beneficiary{0}Relationship", a)];
                                    //    ++_columnIndex;

                                    //    wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[_columnIndex];
                                    //    wsPolicyCell.Value = polRow[String.Format("Beneficiary{0}Nature", a)];
                                    //    ++_columnIndex;

                                    //    wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[_columnIndex];
                                    //    wsPolicyCell.Value = polRow[String.Format("Beneficiary{0}Percentage", a)];
                                    //    ++_columnIndex;
                                    //}

                                    //#endregion Beneficiaries

                                    //#region Kids

                                    //// Kids1DOB
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[183];
                                    //wsPolicyCell.Value = polRow["Kids1DOB"];
                                    //++_columnIndex;

                                    //// Kids2DOB
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[184];
                                    //wsPolicyCell.Value = polRow["Kids2DOB"];
                                    //++_columnIndex;

                                    //// Kids3DOB
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[185];
                                    //wsPolicyCell.Value = polRow["Kids3DOB"];
                                    //++_columnIndex;

                                    //// Kids4DOB
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[186];
                                    //wsPolicyCell.Value = polRow["Kids4DOB"];
                                    //++_columnIndex;

                                    //#endregion

                                    //#region Future Fields

                                    //// FutureField1
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[187];
                                    //wsPolicyCell.Value = polRow["FutureField1"];
                                    //++_columnIndex;

                                    //// FutureField2
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[188];
                                    //wsPolicyCell.Value = polRow["FutureField2"];
                                    //++_columnIndex;

                                    //// FutureField3
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[189];
                                    //wsPolicyCell.Value = polRow["FutureField3"];
                                    //++_columnIndex;

                                    //// FutureField4
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[190];
                                    //wsPolicyCell.Value = polRow["FutureField4"];
                                    //++_columnIndex;

                                    //// FutureField5
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[191];
                                    //wsPolicyCell.Value = polRow["FutureField5"];
                                    //++_columnIndex;

                                    //// FutureField6
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[192];
                                    //wsPolicyCell.Value = polRow["FutureField6"];
                                    //++_columnIndex;

                                    //// FutureField7
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[193];
                                    //wsPolicyCell.Value = polRow["FutureField7"];
                                    //++_columnIndex;

                                    //// FutureField8
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[194];
                                    //wsPolicyCell.Value = polRow["FutureField8"];
                                    //++_columnIndex;

                                    //// FutureField9
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[195];
                                    //wsPolicyCell.Value = polRow["FutureField9"];
                                    //++_columnIndex;

                                    //// FutureField10
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[196];
                                    //wsPolicyCell.Value = polRow["FutureField10"];
                                    //++_columnIndex;

                                    //#endregion Future Fields

                                    //#region 
                                    //// LA2Relationship
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[197];
                                    //wsPolicyCell.Value = polRow["LA2Relationship"];
                                    //++_columnIndex;

                                    //// HaloOfHope
                                    //wsPolicyCell = wsPolicyData.Rows[3 + rowCount].Cells[198];
                                    //wsPolicyCell.Value = polRow["HaloOfHope"];

                                    //#endregion

                                    //// Update the row counter:
                                    //rowCount++;

                                    ////Reset the column index to 0:
                                    //_columnIndex = 0;

                                    #endregion #region THE VERY LONG WAY!!!
                                }
                            }
                        }

                        #endregion Policy Data

                        //string batchCode = "";
                        //Worksheet ws = workBook.Worksheets.Add(campaignName);
                        //WorksheetCell wsCell = null;
                        //int exportRow = 0;


                        //Save excel document
                        wbBatchExport.SetCurrentFormat(WorkbookFormat.Excel2007);
                        wbBatchExport.Save(filePathAndName);

                        //Display excel document
                        Process.Start(filePathAndName);
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

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;
            btnExport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnExport.ToolTip = btnExport.Content;
        }

        #endregion

        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _campaigns = xdgCampaigns.Records;

                btnClose.IsEnabled = false;
                btnExport.IsEnabled = false;
                xdgCampaigns.IsEnabled = false;
                //Cal1.IsEnabled = false;
                //Cal2.IsEnabled = false;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += BatchExport;
                worker.RunWorkerCompleted += BatchExportCompleted;
                worker.RunWorkerAsync();

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

        //private void Cal1_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        //{
        //    DateTime.TryParse(Cal1.SelectedDate.ToString(), out _fromDate);
        //    EnableDisableExportButton();
        //}

        //private void Cal2_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        //{
        //    DateTime.TryParse(Cal2.SelectedDate.ToString(), out _toDate);
        //    EnableDisableExportButton();
        //}

        #endregion

    }
}
