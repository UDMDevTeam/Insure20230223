using System.Linq;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Infragistics.Windows.Editors.Events;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using System.Collections.Generic;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportConfirmedSalesScreen
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

        #endregion INotifyPropertyChanged implementation

        #region Private Members

        private DateTime _fromDate = DateTime.Now;
        private DateTime _toDate = DateTime.Now;
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;
        private bool? _isReportRunning = false;

        private DateTime? _reportStartDate;
        private DateTime? _reportEndDate;
        private string _reportStartDateLongFormat;
        private string _reportEndDateLongFormat;

        private string _strTodaysDate;
        private string _strTodaysDateIncludingColons;

        #endregion Private Members

        #region Publicly Encapsulated Properties
        public bool? IsReportRunning
        {
            get
            {
                return _isReportRunning;
            }
            set
            {
                _isReportRunning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReportRunning"));
            }
        }

        #endregion Publicly Encapsulated Properties

        #region Constructors

        public ReportConfirmedSalesScreen()
        {
            InitializeComponent();

            //LoadCampaignClusterInfo();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

        //private DataTable LoadInternalLookupValues()
        //{
        //    //return UDM.Insurance.Business.Insure.INGetCampaignTypes(false);
        //    return UDM.Insurance.Business.Insure.INGetConfirmedSalesReportCampaignGroupings();
        //}

        private bool IsAllInputParametersSpecifiedAndValid()
        {

            #region Ensuring that the From Date was specified

            _reportStartDate = calFromDate.SelectedDate; //.Value.ToString("yyyy-MM-dd");

            if (_reportStartDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'From Date'.", @"No 'From Date' specified", ShowMessageType.Error);
                return false;
            }
            //else
            //{
            //    _reportStartDateLongFormat = calFromDate.SelectedDate.Value.ToString("dddd, dd MMMM yyyy");
            //}

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the From Date was specified

            _reportEndDate = calToDate.SelectedDate; //.Value.ToString("yyyy-MM-dd");

            if (_reportEndDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'To Date'.", @"No 'To Date' specified", ShowMessageType.Error);
                return false;
            }
            //else
            //{
            //    _reportEndDateLongFormat = calToDate.SelectedDate.Value.ToString("dddd, dd MMMM yyyy");
            //}

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the date range is valid

            if (_reportStartDate > _reportEndDate)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Invalid date range specified: The 'From Date' can not be greater than the 'To Date'.", "Invalid date range", ShowMessageType.Error);
                return false;
            }

            #endregion Ensuring that the date range is valid

            // Otherwise, if all is well, proceed:
            return true;
        }

        #region OLD

        //private void ReportOLD(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        SetCursor(Cursors.Wait);

        //        #region Setup Excel document
        //        int worksheetCount = 0;
                
        //        var wbConfirmedSalesReport = new Workbook();
        //        wbConfirmedSalesReport.SetCurrentFormat(WorkbookFormat.Excel2007);

        //        string filePathAndName = String.Format("{0}Confirmed Sales Report ~ {1}.xlsx",
        //            GlobalSettings.UserFolder,
        //            _strTodaysDate);

        //        Workbook wbTemplate;
        //        Uri uri = new Uri("/Templates/ReportTemplateConfirmedSales.xlsx", UriKind.Relative);
        //        StreamResourceInfo info = Application.GetResourceStream(uri);
        //        if (info != null)
        //        {
        //            wbTemplate = Workbook.Load(info.Stream, true);
        //        }
        //        else
        //        {
        //            return;
        //        }

        //        #endregion Setup Excel document

        //        //DataTable dtINCampaignTypes = LoadInternalLookupValues();

        //        #region Step 1: Add the summary page - only if the from- and to dates differ

        //        if (_reportStartDate.Value != _reportEndDate.Value)
        //        {
        //            AddReportPage(wbConfirmedSalesReport, wbTemplate, "Summary", "Summary", _reportStartDate.Value, _reportEndDate.Value, dtINCampaignTypes);
        //            worksheetCount++;
        //        }
                
        //        #endregion Step 1: Add the summary page

        //        #region Step 2: Add the re page

        //        for (DateTime dtCurrentDate = _reportStartDate.Value; dtCurrentDate <= _reportEndDate.Value; dtCurrentDate = dtCurrentDate.AddDays(1))
        //        {
        //            if (dtCurrentDate.DayOfWeek != DayOfWeek.Sunday)
        //            {
        //                AddReportPage(wbConfirmedSalesReport, wbTemplate, "Summary", dtCurrentDate.ToString("yyyy-MM-dd"), dtCurrentDate, dtINCampaignTypes);
        //                worksheetCount++;
        //            }
        //        }

        //        #endregion Step 2: Add the summary page

        //        #region Finally, save, and display the resulting workbook

        //        if (worksheetCount > 0)
        //        {
        //            wbConfirmedSalesReport.Save(filePathAndName);

        //            //Display excel document
        //            Process.Start(filePathAndName);

        //        }
        //        else
        //        {
        //            //emptyDataTableCount++;
        //            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
        //            {
        //                ShowMessageBox(new INMessageBoxWindow1(), "There is no data available for the criteria you have specified. Please change some of them and try again.", "No data available", ShowMessageType.Information);
        //            });
        //        }
        //        #endregion Finally, save, and display the resulting workbook
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

        //private void AddReportPage(Workbook wbResultingWorkbook, Workbook wbTemplate, string reportTemplateSheetName, string newWorksheetDescription, DateTime dtFromDate, DateTime dtToDate, DataTable dtINConfirmedSalesReportCampaignGroupings)
        //{
        //    #region Declaring & initializing variables

        //    int reportRowIndex = 7;
        //    int zeroRowsCount = 0;
        //    int campaignCount = 0;
        //    string reportPageSubHeading = String.Empty;
        //    string formulaBody = String.Empty;
        //    string campaignNotOnTargetFormula = String.Empty;
        //    string extensionCampaignFormulaBody = String.Empty;

        //    if (dtFromDate.Date != dtToDate.Date)
        //    {
        //        reportPageSubHeading = String.Format("For the period between {0} and {1}", dtFromDate.ToString("dddd, d MMMM yyyy"), dtToDate.ToString("dddd, d MMMM yyyy"));
        //        //newWorksheetDescription = "Summary";
        //    }
        //    else
        //    {
        //        reportPageSubHeading = String.Format("For {0}", dtFromDate.ToString("dddd, d MMMM yyyy"));
        //        //newWorksheetDescription = DateTime.Now.ToString("yyyy-MM-dd");
        //    }

        //    Worksheet wsNewWorksheetTemplate = wbTemplate.Worksheets[reportTemplateSheetName];
        //    Worksheet wsNewWorksheet = wbResultingWorkbook.Worksheets.Add(newWorksheetDescription);

        //    #endregion Declaring & initializing variables

        //    #region Copy the template formatting and add the details

        //    Methods.CopyExcelRegion(wsNewWorksheetTemplate, 0, 0, 4, 13, wsNewWorksheet, 0, 0);

        //    wsNewWorksheet.GetCell("A3").Value = reportPageSubHeading;
        //    wsNewWorksheet.GetCell("A5").Value = String.Format("Date generated: {0}", DateTime.Now.ToString("yyyy-MM-dd, HH:mm:ss"));

        //    if (dtFromDate.Date == dtToDate.Date)
        //    {
        //        //wsNewWorksheet.GetCell("L5").Value = String.Format("Date to be batched: {0}", UDM.Insurance.Business.Insure.GetAvailableWorkingDayFromDate(dtFromDate.AddDays(4)).ToString("yyyy-MM-dd"));
        //        wsNewWorksheet.GetCell("L5").Value = String.Format("Date to be batched: {0}", UDM.Insurance.Business.Insure.INDetermineBatchingDateFromDateOfSale(dtFromDate, 4).ToString("yyyy-MM-dd"));
        //    }
        //    else
        //    {
        //        wsNewWorksheet.GetCell("E5").Value = null;
        //    }

        //    #endregion Copy the template formatting and add the details

        //    foreach (DataRow drINConfirmedSalesReportCampaignGrouping in dtINConfirmedSalesReportCampaignGroupings.Rows)
        //    {
        //        byte campaignGroupingID = Convert.ToByte(drINConfirmedSalesReportCampaignGrouping["ID"]);
        //        string confirmedSalesReportCampaignGroup = drINConfirmedSalesReportCampaignGrouping["ConfirmedSalesReportCampaignGroup"].ToString();

        //        #region Get the data from database

        //        DataSet dsConfirmedSalesReportDataFull = Business.Insure.INGetConfirmedSalesReportData(campaignGroupingID, dtFromDate, dtToDate);

        //        DataTable dtConfirmedSalesReportData = dsConfirmedSalesReportDataFull.Tables[0];

        //        DataTable dtExcelSheetTotalsAndAverageColumnMappings = dsConfirmedSalesReportDataFull.Tables[1];



        //        #endregion Get the data from database

        //        if (dtConfirmedSalesReportData.Rows.Count > 0)
        //        {
        //            #region Loop through each campaign type, copy the formatting from the template, add the details

        //            Methods.CopyExcelRegion(wsNewWorksheetTemplate, 6, 0, 2, 13, wsNewWorksheet, reportRowIndex - 1, 0);
        //            //Methods.CopyExcelRegion(wsNewWorksheetTemplate, 6, 0, 2, 13, wsNewWorksheet, reportRowIndex, 0);

        //            //wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 1)).Value = confirmedSalesReportCampaignGroup;
        //            wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex)).Value = confirmedSalesReportCampaignGroup;

        //            reportRowIndex += 3;

        //            int formulaStartRow = reportRowIndex - 1;
        //            //int formulaStartRow = reportRowIndex;

        //            //if (campaignGroupingID != 5)
        //            //{
        //            //    campaignNotOnTargetFormula += String.Format("COUNTIF(G{0}:G{1},\"<0.8\"),", reportRowIndex, reportRowIndex + (dtConfirmedSalesReportData.Rows.Count - 1));
        //            //}

        //            switch (campaignGroupingID)
        //            {
        //                case 1:
        //                case 2:
        //                    campaignNotOnTargetFormula += String.Format("COUNTIF(X{0}:X{1},\"<0.8\"),", reportRowIndex, reportRowIndex + (dtConfirmedSalesReportData.Rows.Count - 1));
        //                    break;
        //                case 3:
        //                case 4:
        //                    campaignNotOnTargetFormula += String.Format("COUNTIF(Y{0}:Y{1},\"<0.8\"),", reportRowIndex, reportRowIndex + (dtConfirmedSalesReportData.Rows.Count - 1));
        //                    break;
        //                default:
        //                    campaignNotOnTargetFormula += String.Format("COUNTIF(G{0}:G{1},\"<0.8\"),", reportRowIndex, reportRowIndex + (dtConfirmedSalesReportData.Rows.Count - 1));
        //                    break;
        //            }

        //            foreach (DataRow drConfirmedSalesReportData in dtConfirmedSalesReportData.Rows)
        //            {
        //                Methods.CopyExcelRegion(wsNewWorksheetTemplate, 9, 0, 0, 13, wsNewWorksheet, reportRowIndex - 1, 0);
        //                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex)).Value = drConfirmedSalesReportData["CampaignName"];
        //                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = drConfirmedSalesReportData["CampaignCode"];

        //                wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).Value = drConfirmedSalesReportData["Sales"];
        //                wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).Value = drConfirmedSalesReportData["ConfirmationTarget"];
        //                wsNewWorksheet.GetCell(String.Format("E{0}", reportRowIndex)).Value = drConfirmedSalesReportData["ActualConfirmedPolicies"];

        //                wsNewWorksheet.GetCell(String.Format("F{0}", reportRowIndex)).ApplyFormula(String.Format("=D{0}-E{0}", reportRowIndex));
        //                wsNewWorksheet.GetCell(String.Format("G{0}", reportRowIndex)).ApplyFormula(String.Format("=E{0}/C{0}", reportRowIndex));

        //                wsNewWorksheet.GetCell(String.Format("H{0}", reportRowIndex)).Value = drConfirmedSalesReportData["SalesforMonth"];
        //                wsNewWorksheet.GetCell(String.Format("I{0}", reportRowIndex)).Value = drConfirmedSalesReportData["ActualConfirmedPoliciesForMonth"];
        //                wsNewWorksheet.GetCell(String.Format("J{0}", reportRowIndex)).ApplyFormula(String.Format("=I{0}/H{0}", reportRowIndex));


        //                //wsNewWorksheet.GetCell(String.Format("J{0}", reportRowIndex)).CellFormat.FormatString = "0%";


        //                //if (campaignGroupingID != 5)
        //                //{
        //                switch(campaignGroupingID)
        //                {
        //                    case 1:
        //                    case 2:
        //                        formulaBody += String.Format("X{0},", reportRowIndex);
        //                        break;
        //                    case 3:
        //                    case 4:
        //                        formulaBody += String.Format("Y{0},", reportRowIndex);
        //                        break;
        //                    default:
        //                        formulaBody += String.Format("X{0},", reportRowIndex);
        //                        break;
        //                }
        //                    //formulaBody += String.Format("X{0},", reportRowIndex);
        //                //}
        //                //else
        //                //{
        //                //    extensionCampaignFormulaBody += String.Format("X{0},", reportRowIndex);
        //                //}
        //                //campaignCount++;
        //                reportRowIndex++;
        //            }

                    

        //            reportRowIndex = Methods.MapTemplatizedExcelFormulas(wsNewWorksheetTemplate, dtExcelSheetTotalsAndAverageColumnMappings, 10, 0, 0, 14, wsNewWorksheet, reportRowIndex - 1, 0, formulaStartRow, reportRowIndex - 2);
        //            reportRowIndex++;
        //            #region Add the "Totals & averages row"

        //            //Methods.CopyExcelRegion(wsNewWorksheetTemplate, 11, 0, 0, 7, wsNewWorksheet, reportRowIndex - 1, 0);
        //            ////Methods.CopyExcelRegion(wsNewWorksheetTemplate, 11, 0, 0, 7, wsNewWorksheet, reportRowIndex, 0);

        //            #region Last minute string manipulation to prepare the formula body of the totals & averages formulae

        //            //// Remove the trailing semicolon (;)
        //            //if (formulaBody.Length > 0)
        //            //{
        //            //    if (formulaBody.EndsWith(","))
        //            //    {
        //            //        formulaBody = formulaBody.Substring(0, formulaBody.Length - 1);
        //            //    }
        //            //}

        //            #endregion Last minute string manipulation to prepare the formula body of the totals & averages formulae

        //            //if (formulaBody != String.Empty)
        //            //{
        //            //    if (zeroRowsCount == dtINConfirmedSalesReportCampaignGroupings.Rows.Count)
        //            //    {
        //            //        wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).Value = 0;
        //            //        wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).Value = 0;
        //            //        wsNewWorksheet.GetCell(String.Format("E{0}", reportRowIndex)).Value = 0;
        //            //        wsNewWorksheet.GetCell(String.Format("F{0}", reportRowIndex)).Value = 0;
        //            //        wsNewWorksheet.GetCell(String.Format("G{0}", reportRowIndex)).Value = 0;
        //            //    }

        //            //    else
        //            //    {
        //            //        string currentColumnFormula = String.Format("=COUNT({0})", formulaBody.Replace('X', 'A'));
        //            //        wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);

        //            //        currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('X', 'C'));
        //            //        wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);

        //            //        currentColumnFormula = String.Format("=AVERAGE({0})", formulaBody.Replace('X', 'D'));
        //            //        wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);

        //            //        currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('X', 'E'));
        //            //        wsNewWorksheet.GetCell(String.Format("E{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);

        //            //        currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('X', 'F'));
        //            //        wsNewWorksheet.GetCell(String.Format("F{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);

        //            //        currentColumnFormula = String.Format("=AVERAGE({0})", formulaBody.Replace('X', 'G'));
        //            //        wsNewWorksheet.GetCell(String.Format("G{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);

        //            //        currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('X', 'H'));
        //            //        wsNewWorksheet.GetCell(String.Format("H{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //            //    }
        //            //}

        //            //reportRowIndex++;

        //            #endregion Add the "Totals & averages row"                  
        //        }
        //        else
        //        {
        //            #region Otherwise, add an indication that there are no data for the campaign type

        //            Methods.CopyExcelRegion(wsNewWorksheetTemplate, 6, 0, 0, 13, wsNewWorksheet, reportRowIndex - 1, 0);
        //            //Methods.CopyExcelRegion(wsNewWorksheetTemplate, 6, 0, 0, 13, wsNewWorksheet, reportRowIndex, 0);
        //            wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex)).Value = confirmedSalesReportCampaignGroup;

        //            ++zeroRowsCount;
        //            reportRowIndex += 1;

        //            WorksheetMergedCellsRegion mergedRegion;
        //            mergedRegion = wsNewWorksheet.MergedCellsRegions.Add(reportRowIndex - 1, 0, reportRowIndex - 1, 13);
        //            //mergedRegion = wsNewWorksheet.MergedCellsRegions.Add(reportRowIndex, 0, reportRowIndex, 13);
        //            mergedRegion.CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
        //            mergedRegion.CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
        //            mergedRegion.CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
        //            mergedRegion.CellFormat.TopBorderStyle = CellBorderLineStyle.Double;
        //            mergedRegion.Value = "No data available for this campaign type.";
        //            mergedRegion.CellFormat.Alignment = HorizontalCellAlignment.Center;
        //            mergedRegion.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

        //            //if (campaignGroupingID != 5)
        //            //{
        //            switch (campaignGroupingID)
        //            {
        //                case 1:
        //                case 2:
        //                    formulaBody += String.Format("X{0},", reportRowIndex);
        //                    break;
        //                case 3:
        //                case 4:
        //                    formulaBody += String.Format("Y{0},", reportRowIndex);
        //                    break;
        //                default:
        //                    formulaBody += String.Format("X{0},", reportRowIndex);
        //                    break;
        //            }
        //            //formulaBody += String.Format("X{0},", reportRowIndex);
        //            //}
        //            //else
        //            //{
        //            //    extensionCampaignFormulaBody += String.Format("X{0},", reportRowIndex);
        //            //}
        //            reportRowIndex++;

        //            #endregion Otherwise, add an indication that there are no data for the campaign type
        //        }

        //        #endregion Loop through each campaign type, copy the formatting from the template and add the details


        //        //if (formulaStartingRow == reportRowIndex)
        //        //{
        //        //    formulaBody += String.Format("X{0};", formulaStartingRow);
        //        //}
        //        //else
        //        //{
        //        //    formulaBody += String.Format("X{0}:X{1};", formulaStartingRow, reportRowIndex);
        //        //}

        //        //formulaBody = String.Empty;
        //    }

            

        //    #region Add the summary at the bottom of the report

        //    Methods.CopyExcelRegion(wsNewWorksheetTemplate, 14, 0, 5, 4, wsNewWorksheet, reportRowIndex, 0);

        //    if (newWorksheetDescription == "Summary")
        //    {
        //        wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 1)).Value = "Grand totals for period";
        //    }
        //    else
        //    {
        //        wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 1)).Value = String.Format("Grand totals for date of sale {0}", newWorksheetDescription);
        //    }
            
        //    reportRowIndex += 2;

        //    #region Last minute string manipulation to prepare the formula body of the totals & averages formulae

        //    // Remove the trailing semicolon (;)
        //    if (formulaBody.Length > 0)
        //    {
        //        if (formulaBody.EndsWith(","))
        //        {
        //            formulaBody = formulaBody.Substring(0, formulaBody.Length - 1);
        //        }
        //    }

        //    if (campaignNotOnTargetFormula.Length > 0)
        //    {
        //        if (campaignNotOnTargetFormula.EndsWith(","))
        //        {
        //            campaignNotOnTargetFormula = campaignNotOnTargetFormula.Substring(0, campaignNotOnTargetFormula.Length - 1);
        //        }
        //    }

        //    if (extensionCampaignFormulaBody.Length > 0)
        //    {
        //        if (extensionCampaignFormulaBody.EndsWith(","))
        //        {
        //            extensionCampaignFormulaBody = extensionCampaignFormulaBody.Substring(0, extensionCampaignFormulaBody.Length - 1);
        //        }
        //    } 

        //    #endregion Last minute string manipulation to prepare the formula body of the totals & averages formulae

        //    //if (formulaBody != String.Empty)
        //    //{
        //    if (zeroRowsCount == dtINConfirmedSalesReportCampaignGroupings.Rows.Count)
        //    {
        //        for (int a = reportRowIndex; reportRowIndex < a + 5; reportRowIndex++)
        //        {
        //            wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = 0;
        //            wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).Value = 0;
        //            wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).Value = 0;
        //        }
        //        //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = 0;
        //        //reportRowIndex++;
        //        //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = 0;
        //        //reportRowIndex++;
        //        //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = 0;
        //        //reportRowIndex++;
        //        //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = 0;
        //        //reportRowIndex++;
        //        //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = 0;
        //        //reportRowIndex++;
        //    }

        //    else
        //    {
        //        Regex pattern = new Regex("[XY]");
        //        //string currentColumnFormula;
        //        int xyStatus = 0;

        //        #region complicatedCalculations to remove X or Y when calculating for base or upgrades
        //        //if (formulaBody.Contains('X') & formulaBody.Contains('Y'))
        //        //{
        //        //    //Total Campaigns
        //        //    currentColumnFormula = String.Format("=COUNTA({0})", pattern.Replace(formulaBody, "A"));
        //        //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
        //        //    currentColumnFormula = String.Format("=COUNTA({0})", currentColumnFormula.Replace('X', 'A'));
        //        //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
        //        //    currentColumnFormula = String.Format("=COUNTA({0})", currentColumnFormula.Replace('Y', 'A'));
        //        //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    reportRowIndex++;

        //        //    if (campaignNotOnTargetFormula.Contains('X') & campaignNotOnTargetFormula.Contains('Y'))
        //        //    {
        //        //        //Total Campaigns not on 80%
        //        //        //currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula);                
        //        //        currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(campaignNotOnTargetFormula, "G"));
        //        //        wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //        currentColumnFormula = campaignNotOnTargetFormula.Substring(0, campaignNotOnTargetFormula.IndexOf('Y') - 9);
        //        //        currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'G'));
        //        //        wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //        currentColumnFormula = campaignNotOnTargetFormula.Substring(campaignNotOnTargetFormula.IndexOf('Y') - 8);
        //        //        currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'G'));
        //        //        wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    }
        //        //    else if (campaignNotOnTargetFormula.Contains('X') & !(campaignNotOnTargetFormula.Contains('Y')))
        //        //    {
        //        //        //Total Campaigns not on 80%
        //        //        //currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula);                
        //        //        currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(campaignNotOnTargetFormula, "G"));
        //        //        wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //        //currentColumnFormula = campaignNotOnTargetFormula.Substring(0, campaignNotOnTargetFormula.IndexOf('Y') - 9);
        //        //        currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula.Replace('X', 'G'));
        //        //        wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //        //currentColumnFormula = campaignNotOnTargetFormula.Substring(campaignNotOnTargetFormula.IndexOf('Y') - 8);
        //        //        //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'G'));
        //        //        //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    }
        //        //    else if (!(campaignNotOnTargetFormula.Contains('X')) & campaignNotOnTargetFormula.Contains('Y'))
        //        //    {
        //        //        //Total Campaigns not on 80%
        //        //        //currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula);                
        //        //        currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(campaignNotOnTargetFormula, "G"));
        //        //        wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //        //currentColumnFormula = campaignNotOnTargetFormula.Substring(0, campaignNotOnTargetFormula.IndexOf('Y') - 9);
        //        //        //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'G'));
        //        //        //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //        //currentColumnFormula = campaignNotOnTargetFormula.Substring(campaignNotOnTargetFormula.IndexOf('Y') - 8);
        //        //        currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula.Replace('Y', 'G'));
        //        //        wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    }

        //        //    reportRowIndex++;

        //        //    //Total Policies Sold
        //        //    currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "C"));
        //        //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
        //        //    currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'C'));
        //        //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
        //        //    currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'C'));
        //        //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    reportRowIndex++;

        //        //    //Total Actual Confirmed Policies
        //        //    currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "E"));
        //        //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
        //        //    currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'E'));
        //        //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
        //        //    currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'E'));
        //        //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    reportRowIndex++;

        //        //    //Total Confirmed %
        //        //    //currentColumnFormula = String.Format("=AVERAGE({0})", formulaBody.Replace('X', 'G'));
        //        //    currentColumnFormula = String.Format("=B{0}/B{1}", reportRowIndex - 1, reportRowIndex - 2);
        //        //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    currentColumnFormula = String.Format("=C{0}/C{1}", reportRowIndex - 1, reportRowIndex - 2);
        //        //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    currentColumnFormula = String.Format("=D{0}/D{1}", reportRowIndex - 1, reportRowIndex - 2);
        //        //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //reportRowIndex++;
        //        //}
        //        //else if (formulaBody.Contains('X') & !(formulaBody.Contains('Y')))
        //        //{
        //        //    //Total Campaigns
        //        //    currentColumnFormula = String.Format("=COUNTA({0})", pattern.Replace(formulaBody, "A"));
        //        //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
        //        //    currentColumnFormula = String.Format("=COUNTA({0})", formulaBody.Replace('X', 'A'));
        //        //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
        //        //    //currentColumnFormula = String.Format("=COUNTA({0})", currentColumnFormula.Replace('Y', 'A'));
        //        //    //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    reportRowIndex++;

        //        //    //Total Campaigns not on 80%
        //        //    //currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula);                
        //        //    currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(campaignNotOnTargetFormula, "G"));
        //        //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //currentColumnFormula = campaignNotOnTargetFormula.Substring(0, campaignNotOnTargetFormula.IndexOf('Y') - 9);
        //        //    currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula.Replace('X', 'G'));
        //        //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //currentColumnFormula = campaignNotOnTargetFormula.Substring(campaignNotOnTargetFormula.IndexOf('Y') - 8);
        //        //    //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'G'));
        //        //    //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    reportRowIndex++;

        //        //    //Total Policies Sold
        //        //    currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "C"));
        //        //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
        //        //    currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('X', 'C'));
        //        //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
        //        //    //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'C'));
        //        //    //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    reportRowIndex++;

        //        //    //Total Actual Confirmed Policies
        //        //    currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "E"));
        //        //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
        //        //    currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('X', 'E'));
        //        //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
        //        //    //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'E'));
        //        //    //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    reportRowIndex++;

        //        //    //Total Confirmed %
        //        //    //currentColumnFormula = String.Format("=AVERAGE({0})", formulaBody.Replace('X', 'G'));
        //        //    currentColumnFormula = String.Format("=B{0}/B{1}", reportRowIndex - 1, reportRowIndex - 2);
        //        //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    currentColumnFormula = String.Format("=C{0}/C{1}", reportRowIndex - 1, reportRowIndex - 2);
        //        //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    currentColumnFormula = String.Format("=D{0}/D{1}", reportRowIndex - 1, reportRowIndex - 2);
        //        //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //reportRowIndex++;
        //        //}
        //        //else if (!(formulaBody.Contains('X')) & formulaBody.Contains('Y'))
        //        //{
        //        //    //Total Campaigns
        //        //    currentColumnFormula = String.Format("=COUNTA({0})", pattern.Replace(formulaBody, "A"));
        //        //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
        //        //    //currentColumnFormula = String.Format("=COUNTA({0})", currentColumnFormula.Replace('X', 'A'));
        //        //    //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
        //        //    currentColumnFormula = String.Format("=COUNTA({0})", formulaBody.Replace('Y', 'A'));
        //        //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    reportRowIndex++;

        //        //    //Total Campaigns not on 80%
        //        //    //currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula);                
        //        //    currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(campaignNotOnTargetFormula, "G"));
        //        //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //currentColumnFormula = campaignNotOnTargetFormula.Substring(0, campaignNotOnTargetFormula.IndexOf('Y') - 9);
        //        //    //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'G'));
        //        //    //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //currentColumnFormula = campaignNotOnTargetFormula.Substring(campaignNotOnTargetFormula.IndexOf('Y') - 8);
        //        //    currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula.Replace('Y', 'G'));
        //        //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    reportRowIndex++;

        //        //    //Total Policies Sold
        //        //    currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "C"));
        //        //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
        //        //    //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'C'));
        //        //    //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
        //        //    currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('Y', 'C'));
        //        //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    reportRowIndex++;

        //        //    //Total Actual Confirmed Policies
        //        //    currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "E"));
        //        //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
        //        //    //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'E'));
        //        //    //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
        //        //    currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('Y', 'E'));
        //        //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    reportRowIndex++;

        //        //    //Total Confirmed %
        //        //    //currentColumnFormula = String.Format("=AVERAGE({0})", formulaBody.Replace('X', 'G'));
        //        //    currentColumnFormula = String.Format("=B{0}/B{1}", reportRowIndex - 1, reportRowIndex - 2);
        //        //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    currentColumnFormula = String.Format("=C{0}/C{1}", reportRowIndex - 1, reportRowIndex - 2);
        //        //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    currentColumnFormula = String.Format("=D{0}/D{1}", reportRowIndex - 1, reportRowIndex - 2);
        //        //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //    //reportRowIndex++;
        //        //}
        //        //else
        //        //{
        //        //    xyStatus = 0;
        //        //}
        //        #endregion
        //        #region New Calculations
        //        //NEW CALCULATIONS
        //        //Total Campaigns
        //        string currentColumnFormula = String.Format("=COUNTA({0})", pattern.Replace(formulaBody, "A"));
        //        wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
        //        currentColumnFormula = String.Format("=COUNTA({0})", formulaBody.Replace('X', 'A'));
        //        wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
        //        currentColumnFormula = String.Format("=COUNTA({0})", formulaBody.Replace('Y', 'A'));
        //        wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        reportRowIndex++;

        //        //Total Campaigns not on 80%
        //        //currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula);                
        //        currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(campaignNotOnTargetFormula, "G"));
        //        wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = campaignNotOnTargetFormula.Substring(0, campaignNotOnTargetFormula.IndexOf('Y') - 9);
        //        currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula.Replace('X', 'G'));
        //        wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = campaignNotOnTargetFormula.Substring(campaignNotOnTargetFormula.IndexOf('Y') - 8);
        //        currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula.Replace('Y', 'G'));
        //        wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        reportRowIndex++;

        //        //Total Policies Sold
        //        currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "C"));
        //        wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
        //        currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('X', 'C'));
        //        wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
        //        currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('Y', 'C'));
        //        wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        reportRowIndex++;

        //        //Total Actual Confirmed Policies
        //        currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "E"));
        //        wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
        //        currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('X', 'E'));
        //        wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
        //        currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('Y', 'E'));
        //        wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        reportRowIndex++;

        //        //Total Confirmed %
        //        //currentColumnFormula = String.Format("=AVERAGE({0})", formulaBody.Replace('X', 'G'));
        //        currentColumnFormula = String.Format("=B{0}/B{1}", reportRowIndex - 1, reportRowIndex - 2);
        //        wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        currentColumnFormula = String.Format("=C{0}/C{1}", reportRowIndex - 1, reportRowIndex - 2);
        //        wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        currentColumnFormula = String.Format("=D{0}/D{1}", reportRowIndex - 1, reportRowIndex - 2);
        //        wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //reportRowIndex++;
        //        #endregion


        //        #region OldCalculations
        //        //OLD CALCULATIONS
        //        ////Total Campaigns
        //        //string currentColumnFormula = String.Format("=COUNTA({0})", pattern.Replace(formulaBody, "A"));
        //        //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
        //        //currentColumnFormula = String.Format("=COUNTA({0})", currentColumnFormula.Replace('X', 'A'));
        //        //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
        //        //currentColumnFormula = String.Format("=COUNTA({0})", currentColumnFormula.Replace('Y', 'A'));
        //        //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //reportRowIndex++;

        //        ////Total Campaigns not on 80%
        //        ////currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula);                
        //        //currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(campaignNotOnTargetFormula, "G"));
        //        //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = campaignNotOnTargetFormula.Substring(0, campaignNotOnTargetFormula.IndexOf('Y') - 9);
        //        //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'G'));
        //        //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = campaignNotOnTargetFormula.Substring(campaignNotOnTargetFormula.IndexOf('Y') - 8);
        //        //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'G'));
        //        //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //reportRowIndex++;

        //        ////Total Policies Sold
        //        //currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "C"));
        //        //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
        //        //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'C'));
        //        //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
        //        //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'C'));
        //        //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //reportRowIndex++;

        //        ////Total Actual Confirmed Policies
        //        //currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "E"));
        //        //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
        //        //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'E'));
        //        //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
        //        //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'E'));
        //        //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //reportRowIndex++;

        //        ////Total Confirmed %
        //        ////currentColumnFormula = String.Format("=AVERAGE({0})", formulaBody.Replace('X', 'G'));
        //        //currentColumnFormula = String.Format("=B{0}/B{1}", reportRowIndex - 1, reportRowIndex - 2);
        //        //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = String.Format("=C{0}/C{1}", reportRowIndex - 1, reportRowIndex - 2);
        //        //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        //currentColumnFormula = String.Format("=D{0}/D{1}", reportRowIndex - 1, reportRowIndex - 2);
        //        //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //        ////reportRowIndex++;
        //        #endregion

        //    }
        //    //}

        //    //if (extensionCampaignFormulaBody != String.Empty)
        //    //{
        //    //    reportRowIndex -= 4;
        //    //    if (zeroRowsCount == dtINConfirmedSalesReportCampaignGroupings.Rows.Count)
        //    //    {
        //    //        //Total Campaigns
        //    //        wsNewWorksheet.GetCell(String.Format("E{0}", reportRowIndex)).Value = 0;
        //    //        reportRowIndex++;

        //    //        //Total Policies Sold
        //    //        wsNewWorksheet.GetCell(String.Format("E{0}", reportRowIndex)).Value = 0;
        //    //        reportRowIndex++;
        //    //    }
        //    //    else
        //    //    {
        //    //        //Total Campaigns
        //    //        string currentColumnFormula = String.Format("=COUNTA({0})", extensionCampaignFormulaBody.Replace('X', 'A'));
        //    //        wsNewWorksheet.GetCell(String.Format("E{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //    //        reportRowIndex++;

        //    //        //Total Policies Sold
        //    //        currentColumnFormula = String.Format("=SUM({0})", extensionCampaignFormulaBody.Replace('X', 'C'));
        //    //        wsNewWorksheet.GetCell(String.Format("E{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
        //    //        reportRowIndex++;
        //    //    }
        //    //}
        //    #endregion Add the summary at the bottom of the report
        //}

        //private void AddReportPage(Workbook wbResultingWorkbook, Workbook wbTemplate, string reportTemplateSheetName, string newWorksheetDescription, DateTime dtDate, DataTable dtINCampaignTypes)
        //{
        //    AddReportPage(wbResultingWorkbook, wbTemplate, reportTemplateSheetName, newWorksheetDescription, dtDate, dtDate, dtINCampaignTypes);
        //}


        #endregion OLD

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup Excel document

                string filePathAndName = String.Format("{0}Platinum Estimated Sales Report ~ {1}.xlsx",
                    GlobalSettings.UserFolder,
                    _strTodaysDate);

                Workbook wbConfirmedSalesReportTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateConfirmedSales.xlsx");
                Workbook wbConfirmedSalesReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup Excel document

                #region Get the data

                DataSet dsCallMonitoringReportData = Business.Insure.INGetConfirmedSalesReportData(_fromDate, _toDate);

                if (dsCallMonitoringReportData.Tables[2].Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                #endregion Get the data

                InsertConfirmedSalesReportSheets(wbConfirmedSalesReportTemplate, wbConfirmedSalesReport, dsCallMonitoringReportData);

                #region Finally, save, and display the resulting workbook
                if (wbConfirmedSalesReport.Worksheets.Count > 0)
                {
                    wbConfirmedSalesReport.Save(filePathAndName);

                    //Display excel document
                    Process.Start(filePathAndName);
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "There is no data available for the criteria you have specified. Please change some of them and try again.", "No data available", ShowMessageType.Information);
                    });
                }
                #endregion Finally, save, and display the resulting workbook
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

        private void InsertConfirmedSalesReportSheets(Workbook wbConfirmedSalesReportTemplate, Workbook wbConfirmedSalesReport, DataSet dsConfirmedSalesReportData)
        {
            List<int> lsHiddenColumns = new List<int> {4,5,6,7,9,10};
            #region Partition the given dataset

            DataTable dtSheetDataFilters = dsConfirmedSalesReportData.Tables[0];
            DataTable dtReportData;
            DataTable dtCurrentDataSheetPartitions;
            DataTable dtCurrentDataSheetPartitionData;
            DataTable dtExcelSheetDataTableColumnMappings = dsConfirmedSalesReportData.Tables[4];
            DataTable dtExcelCellTotalsFormulasMappings = dsConfirmedSalesReportData.Tables[5];

            #endregion Partition the given dataset

            #region Declarations & Initializations

            int reportRow = 6;
            int formulaStartRow = 0;

            byte dataTableIndex = 0;
            string reportDataFilterString = String.Empty;
            string reportDataOrderByString = String.Empty;

            string templateSheetName;
            string reportSheetName;

            string reportSubtitle;
            string reportSubHeadingCell;
            string reportDateCell;

            byte templateDataSheetRowSpan;
            byte templateColumnPartitionColumnHeadingsRowSpan;
            byte templateColumnPartitionColumnHeadingsRowIndex;
            byte templateColumnSpan;
            byte templateRowIndex;
            byte totalsTemplateRowIndex; // = 10;

            byte summaryTemplateRowIndex;
            byte summaryTemplateRowSpan;
            byte summaryTemplateColumnSpan;
            string summaryHeading;

            string cumulativeFiguresColumn;
            string baseFiguresColumn;
            string upgradeFiguresColumn;

            string sumFormulaTemplate;
            string confirmedPercentageFormulaTemplate;
            short totalConfirmedPercentageNumeratorRowIndex;
            short totalConfirmedPercentageDenominatorRowIndex;

            // For each partition:
            bool isUpgradePartition;

            string campaignCountFormulaTemplate;
            string cumulativeCampaignCountFormulaBody;
            string baseCampaignCountFormulaBody;
            string upgradeCampaignCountFormulaBody;

            string campaignsNotOnTargetFormulaTemplate;
            string cumulativeCampaignsNotOnTargetFormulaBody;
            string baseCampaignsNotOnTargetFormulaBody;
            string upgradeCampaignsNotOnTargetFormulaBody;

            string policiesSoldFormulaTemplate;
            string cumulativePoliciesSoldFormulaBody;
            string basePoliciesSoldFormulaBody;
            string upgradePoliciesSoldFormulaBody;

            string policiesConfirmedFormulaTemplate;
            string cumulativePoliciesConfirmedFormulaBody;
            string basePoliciesConfirmedFormulaBody;
            string upgradePoliciesConfirmedFormulaBody;

            #endregion Declarations & Initializations

            foreach (DataRow row in dtSheetDataFilters.Rows)
            {
                dataTableIndex = Convert.ToByte(row["DataTableIndex"]);
                reportDataFilterString = row["ReportDataFilterString"].ToString();
                reportDataOrderByString = row["ReportDataOrderByString"].ToString();

                // Firstly determine if a particular sheet should be added for a particular date (regardless of the grouping):
                var filteredDataSheetRows = dsConfirmedSalesReportData.Tables[dataTableIndex].Select(reportDataFilterString, reportDataOrderByString).AsEnumerable();
                if (filteredDataSheetRows.Any())
                {

                    #region Initialize

                    dtReportData = dsConfirmedSalesReportData.Tables[dataTableIndex].Select(reportDataFilterString, reportDataOrderByString).CopyToDataTable();

                    templateSheetName = row["TemplateSheetName"].ToString();
                    reportSheetName = row["ReportSheetName"].ToString();
                    reportSubtitle = row["ReportSubtitle"].ToString();
                    reportSubHeadingCell = row["ReportSubtitleCell"].ToString();
                    reportDateCell = row["ReportDateCell"].ToString();

                    templateDataSheetRowSpan = Convert.ToByte(row["TemplateDataSheetRowSpan"]);
                    templateColumnPartitionColumnHeadingsRowSpan = Convert.ToByte(row["TemplateColumnPartitionColumnHeadingsRowSpan"]);
                    templateColumnPartitionColumnHeadingsRowIndex = Convert.ToByte(row["TemplateColumnPartitionColumnHeadingsRowIndex"]);
                    templateColumnSpan = Convert.ToByte(row["TemplateColumnSpan"]);
                    templateRowIndex = Convert.ToByte(row["TemplateRowIndex"]);
                    totalsTemplateRowIndex = Convert.ToByte(row["totalsTemplateRowIndex"]);

                    cumulativeFiguresColumn = row["CumulativeFiguresColumn"].ToString();
                    baseFiguresColumn = row["BaseFiguresColumn"].ToString();
                    upgradeFiguresColumn = row["UpgradeFiguresColumn"].ToString();

                    cumulativeCampaignCountFormulaBody = String.Empty;
                    baseCampaignCountFormulaBody = String.Empty;
                    upgradeCampaignCountFormulaBody = String.Empty;

                    cumulativeCampaignsNotOnTargetFormulaBody = String.Empty;
                    baseCampaignsNotOnTargetFormulaBody = String.Empty;
                    upgradeCampaignsNotOnTargetFormulaBody = String.Empty;

                    cumulativePoliciesSoldFormulaBody = String.Empty;
                    basePoliciesSoldFormulaBody = String.Empty;
                    upgradePoliciesSoldFormulaBody = String.Empty;

                    cumulativePoliciesConfirmedFormulaBody = String.Empty;
                    basePoliciesConfirmedFormulaBody = String.Empty;
                    upgradePoliciesConfirmedFormulaBody = String.Empty;

                    #endregion Initialize

                    #region Add the worksheet

                    Worksheet wsReportTemplate = wbConfirmedSalesReportTemplate.Worksheets[templateSheetName];
                    Worksheet wsReport = wbConfirmedSalesReport.Worksheets.Add(reportSheetName);
                    Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                    #endregion Add the worksheet

                    #region Populating the report details

                    Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
                    
                    
                    wsReport.GetCell(reportSubHeadingCell).Value = reportSubtitle;
                    wsReport.GetCell(reportDateCell).Value = String.Format("Date generated: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    foreach (int column in lsHiddenColumns)
                    {
                        wsReport.Columns[column].Hidden = true;
                    }



                    #endregion Populating the report details

                    #region Insert the partitioned data - if there are any rows available for each partition

                    // Determine if there are any rows for the given partition
                    var filteredSheetRows = dsConfirmedSalesReportData.Tables[1].Select(reportDataFilterString, reportDataOrderByString).AsEnumerable();
                    if (filteredSheetRows.Any())
                    {
                        dtCurrentDataSheetPartitions = dsConfirmedSalesReportData.Tables[1].Select(reportDataFilterString, reportDataOrderByString).CopyToDataTable();

                        foreach (DataRow drCurrentPartition in dtCurrentDataSheetPartitions.Rows)
                        {
                            #region Filter the rows according to the current partition

                            string currentPartitionDataFilterString = drCurrentPartition["FilterString"].ToString();
                            string currentPartitionDataOrderByString = drCurrentPartition["OrderByString"].ToString();
                            string currentPartitionHeading = drCurrentPartition["ConfirmedSalesReportCampaignPartitioning"].ToString();
                            
                            dtCurrentDataSheetPartitionData = dtReportData.Select(currentPartitionDataFilterString, currentPartitionDataOrderByString).CopyToDataTable();

                            #endregion Filter the rows according to the current partition

                            #region Insert the current partition's column headings

                            Methods.CopyExcelRegion(wsReportTemplate, templateColumnPartitionColumnHeadingsRowIndex, 0, templateColumnPartitionColumnHeadingsRowSpan, templateColumnSpan, wsReport, reportRow, 0);
                            wsReport.GetCell(String.Format("A{0}", reportRow + 1)).Value = currentPartitionHeading;
                            reportRow += templateColumnPartitionColumnHeadingsRowSpan;

                            #endregion Insert the current partition's column headings

                            #region Add the data

                            formulaStartRow = reportRow;
                            reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentDataSheetPartitionData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                            #endregion Add the data

                            #region Update the formulae that will be used in the summary

                            isUpgradePartition = Convert.ToBoolean(drCurrentPartition["IsUpgradePartition"]);
                            campaignCountFormulaTemplate = drCurrentPartition["CampaignCountFormulaTemplate"].ToString();
                            campaignsNotOnTargetFormulaTemplate = drCurrentPartition["CampaignsNotOnTargetFormulaTemplate"].ToString();
                            policiesSoldFormulaTemplate = drCurrentPartition["PoliciesSoldFormulaTemplate"].ToString();
                            policiesConfirmedFormulaTemplate = drCurrentPartition["PoliciesConfirmedFormulaTemplate"].ToString();

                            #region Total Campaigns Formulae

                            if (String.IsNullOrEmpty(cumulativeCampaignCountFormulaBody))
                            {
                                cumulativeCampaignCountFormulaBody += campaignCountFormulaTemplate.Replace("#FROM_ROW#", (formulaStartRow + 1).ToString()).Replace("#TO_ROW#", (reportRow).ToString());
                            }
                            else
                            {
                                cumulativeCampaignCountFormulaBody += String.Format(",{0}", campaignCountFormulaTemplate.Replace("#FROM_ROW#", (formulaStartRow + 1).ToString()).Replace("#TO_ROW#", (reportRow).ToString()));
                            }

                            if (isUpgradePartition)
                            {
                                if (String.IsNullOrEmpty(upgradeCampaignCountFormulaBody))
                                {
                                    upgradeCampaignCountFormulaBody += campaignCountFormulaTemplate.Replace("#FROM_ROW#", (formulaStartRow + 1).ToString()).Replace("#TO_ROW#", (reportRow).ToString());
                                }
                                else
                                {
                                    upgradeCampaignCountFormulaBody += String.Format(",{0}", campaignCountFormulaTemplate.Replace("#FROM_ROW#", (formulaStartRow + 1).ToString()).Replace("#TO_ROW#", (reportRow).ToString()));
                                }
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(baseCampaignCountFormulaBody))
                                {
                                    baseCampaignCountFormulaBody += campaignCountFormulaTemplate.Replace("#FROM_ROW#", (formulaStartRow + 1).ToString()).Replace("#TO_ROW#", (reportRow).ToString());
                                }
                                else
                                {
                                    baseCampaignCountFormulaBody += String.Format(",{0}", campaignCountFormulaTemplate.Replace("#FROM_ROW#", (formulaStartRow + 1).ToString()).Replace("#TO_ROW#", (reportRow).ToString()));
                                }
                            }

                            #endregion Total Campaigns Formulae

                            #region Total Campaigns not on 80% Formulae

                            if (String.IsNullOrEmpty(cumulativeCampaignsNotOnTargetFormulaBody))
                            {
                                cumulativeCampaignsNotOnTargetFormulaBody += campaignsNotOnTargetFormulaTemplate.Replace("#FROM_ROW#", (formulaStartRow + 1).ToString()).Replace("#TO_ROW#", (reportRow).ToString());
                            }
                            else
                            {
                                cumulativeCampaignsNotOnTargetFormulaBody += String.Format(",{0}", campaignsNotOnTargetFormulaTemplate.Replace("#FROM_ROW#", (formulaStartRow + 1).ToString()).Replace("#TO_ROW#", (reportRow).ToString()));
                            }

                            if (isUpgradePartition)
                            {
                                if (String.IsNullOrEmpty(upgradeCampaignsNotOnTargetFormulaBody))
                                {
                                    upgradeCampaignsNotOnTargetFormulaBody += campaignsNotOnTargetFormulaTemplate.Replace("#FROM_ROW#", (formulaStartRow + 1).ToString()).Replace("#TO_ROW#", (reportRow).ToString());
                                }
                                else
                                {
                                    upgradeCampaignsNotOnTargetFormulaBody += String.Format(",{0}", campaignsNotOnTargetFormulaTemplate.Replace("#FROM_ROW#", (formulaStartRow + 1).ToString()).Replace("#TO_ROW#", (reportRow).ToString()));
                                }
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(baseCampaignsNotOnTargetFormulaBody))
                                {
                                    baseCampaignsNotOnTargetFormulaBody += campaignsNotOnTargetFormulaTemplate.Replace("#FROM_ROW#", (formulaStartRow + 1).ToString()).Replace("#TO_ROW#", (reportRow).ToString());
                                }
                                else
                                {
                                    baseCampaignsNotOnTargetFormulaBody += String.Format(",{0}", campaignsNotOnTargetFormulaTemplate.Replace("#FROM_ROW#", (formulaStartRow + 1).ToString()).Replace("#TO_ROW#", (reportRow).ToString()));
                                }
                            }

                            #endregion Total Campaigns not on 80% Formulae

                            #region Total Policies Sold Formulae

                            if (String.IsNullOrEmpty(cumulativePoliciesSoldFormulaBody))
                            {
                                cumulativePoliciesSoldFormulaBody += policiesSoldFormulaTemplate.Replace("#ROW#", (reportRow + 1).ToString());
                            }
                            else
                            {
                                cumulativePoliciesSoldFormulaBody += String.Format(",{0}", policiesSoldFormulaTemplate.Replace("#ROW#", (reportRow + 1).ToString()));
                            }

                            if (isUpgradePartition)
                            {
                                    if (String.IsNullOrEmpty(upgradePoliciesSoldFormulaBody))
                                {
                                    upgradePoliciesSoldFormulaBody += policiesSoldFormulaTemplate.Replace("#ROW#", (reportRow + 1).ToString());
                                }
                                else
                                {
                                    upgradePoliciesSoldFormulaBody += String.Format(",{0}", policiesSoldFormulaTemplate.Replace("#ROW#", (reportRow + 1).ToString()));
                                }
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(basePoliciesSoldFormulaBody))
                                {
                                    basePoliciesSoldFormulaBody += policiesSoldFormulaTemplate.Replace("#ROW#", (reportRow + 1).ToString());
                                }
                                else
                                {
                                    basePoliciesSoldFormulaBody += String.Format(",{0}", policiesSoldFormulaTemplate.Replace("#ROW#", (reportRow + 1).ToString()));
                                }
                            }

                            #endregion Total Policies Sold Formulae

                            #region Total Actual Confirmed Policies

                            if (String.IsNullOrEmpty(cumulativePoliciesConfirmedFormulaBody))
                            {
                                cumulativePoliciesConfirmedFormulaBody += policiesConfirmedFormulaTemplate.Replace("#ROW#", (reportRow + 1).ToString());
                            }
                            else
                            {
                                cumulativePoliciesConfirmedFormulaBody += String.Format(",{0}", policiesConfirmedFormulaTemplate.Replace("#ROW#", (reportRow + 1).ToString()));
                            }

                            if (isUpgradePartition)
                            {
                                if (String.IsNullOrEmpty(upgradePoliciesConfirmedFormulaBody))
                                {
                                    upgradePoliciesConfirmedFormulaBody += policiesConfirmedFormulaTemplate.Replace("#ROW#", (reportRow + 1).ToString());
                                }
                                else
                                {
                                    upgradePoliciesConfirmedFormulaBody += String.Format(",{0}", policiesConfirmedFormulaTemplate.Replace("#ROW#", (reportRow + 1).ToString()));
                                }
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(basePoliciesConfirmedFormulaBody))
                                {
                                    basePoliciesConfirmedFormulaBody += policiesConfirmedFormulaTemplate.Replace("#ROW#", (reportRow + 1).ToString());
                                }
                                else
                                {
                                    basePoliciesConfirmedFormulaBody += String.Format(",{0}", policiesConfirmedFormulaTemplate.Replace("#ROW#", (reportRow + 1).ToString()));
                                }
                            }

                            #endregion Total Actual Confirmed Policies

                            #endregion Update the formulae that will be used in the summary

                            #region Add the totals / averages

                            reportRow = Methods.MapTemplatizedExcelFormulas(wsReportTemplate, dtExcelCellTotalsFormulasMappings, totalsTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0, formulaStartRow, reportRow - 1);

                            #endregion Add the totals / averages
                        }

                        #region Add the summary at the bottom of the report

                        #region Initiating the variables that will be used to instantiate the summary

                        summaryTemplateRowIndex = Convert.ToByte(row["SummaryTemplateRowIndex"]);
                        summaryTemplateRowSpan = Convert.ToByte(row["SummaryTemplateRowSpan"]);
                        summaryTemplateColumnSpan = Convert.ToByte(row["SummaryTemplateColumnSpan"]);
                        summaryHeading = row["SummaryHeading"].ToString();

                        sumFormulaTemplate = row["SumFormulaTemplate"].ToString();
                        confirmedPercentageFormulaTemplate = row["confirmedPercentageFormulaTemplate"].ToString();
                        totalConfirmedPercentageNumeratorRowIndex = Convert.ToInt16(row["TotalConfirmedPercentageNumeratorRowIndex"]);
                        totalConfirmedPercentageDenominatorRowIndex = Convert.ToInt16(row["TotalConfirmedPercentageDenominatorRowIndex"]);

                        #endregion Initiating the variables that will be used to instantiate the summary

                        #region Applying the formulas

                        reportRow++;

                        Methods.CopyExcelRegion(wsReportTemplate, summaryTemplateRowIndex, 0, summaryTemplateRowSpan, summaryTemplateColumnSpan, wsReport, reportRow, 0);
                        wsReport.GetCell(String.Format("A{0}", reportRow + 1)).Value = summaryHeading;
                        reportRow++;

                        #region Total Campaigns

                        if (!String.IsNullOrEmpty(cumulativeCampaignCountFormulaBody))
                        {
                            wsReport.GetCell(String.Format("{0}{1}", cumulativeFiguresColumn, reportRow + 1)).ApplyFormula(sumFormulaTemplate.Replace("#FORMULA_BODY#", cumulativeCampaignCountFormulaBody));
                        }
                        else
                        {
                            wsReport.GetCell(String.Format("{0}{1}", cumulativeFiguresColumn, reportRow + 1)).Value = 0;
                        }

                        if (!String.IsNullOrEmpty(baseCampaignCountFormulaBody))
                        {
                            wsReport.GetCell(String.Format("{0}{1}", baseFiguresColumn, reportRow + 1)).ApplyFormula(sumFormulaTemplate.Replace("#FORMULA_BODY#", baseCampaignCountFormulaBody));
                        }
                        else
                        {
                            wsReport.GetCell(String.Format("{0}{1}", baseFiguresColumn, reportRow + 1)).Value = 0;
                        }

                        if (!String.IsNullOrEmpty(upgradeCampaignCountFormulaBody))
                        {
                            wsReport.GetCell(String.Format("{0}{1}", upgradeFiguresColumn, reportRow + 1)).ApplyFormula(sumFormulaTemplate.Replace("#FORMULA_BODY#", upgradeCampaignCountFormulaBody));
                        }
                        else
                        {
                            wsReport.GetCell(String.Format("{0}{1}", upgradeFiguresColumn, reportRow + 1)).Value = 0;
                        }
                        reportRow++;

                        #endregion Total Campaigns

                        #region Total Campaigns not on 80 %

                        //if (!String.IsNullOrEmpty(cumulativeCampaignsNotOnTargetFormulaBody))
                        //{
                        //    wsReport.GetCell(String.Format("{0}{1}", cumulativeFiguresColumn, reportRow + 1)).ApplyFormula(sumFormulaTemplate.Replace("#FORMULA_BODY#", cumulativeCampaignsNotOnTargetFormulaBody));
                        //}
                        //else
                        //{
                        //    wsReport.GetCell(String.Format("{0}{1}", cumulativeFiguresColumn, reportRow + 1)).Value = 0;
                        //}

                        //if (!String.IsNullOrEmpty(baseCampaignsNotOnTargetFormulaBody))
                        //{
                        //    wsReport.GetCell(String.Format("{0}{1}", baseFiguresColumn, reportRow + 1)).ApplyFormula(sumFormulaTemplate.Replace("#FORMULA_BODY#", baseCampaignsNotOnTargetFormulaBody));
                        //}
                        //else
                        //{
                        //    wsReport.GetCell(String.Format("{0}{1}", baseFiguresColumn, reportRow + 1)).Value = 0;
                        //}

                        //if (!String.IsNullOrEmpty(upgradeCampaignsNotOnTargetFormulaBody))
                        //{
                        //    wsReport.GetCell(String.Format("{0}{1}", upgradeFiguresColumn, reportRow + 1)).ApplyFormula(sumFormulaTemplate.Replace("#FORMULA_BODY#", upgradeCampaignsNotOnTargetFormulaBody));
                        //}
                        //else
                        //{
                        //    wsReport.GetCell(String.Format("{0}{1}", upgradeFiguresColumn, reportRow + 1)).Value = 0;
                        //}
                        //reportRow++;

                        #endregion Total Campaigns not on 80 %

                        #region Total Policies Sold

                        if (!String.IsNullOrEmpty(cumulativePoliciesSoldFormulaBody))
                        {
                            wsReport.GetCell(String.Format("{0}{1}", cumulativeFiguresColumn, reportRow + 1)).ApplyFormula(sumFormulaTemplate.Replace("#FORMULA_BODY#", cumulativePoliciesSoldFormulaBody));
                        }
                        else
                        {
                            wsReport.GetCell(String.Format("{0}{1}", cumulativeFiguresColumn, reportRow + 1)).Value = 0;
                        }

                        if (!String.IsNullOrEmpty(basePoliciesSoldFormulaBody))
                        {
                            wsReport.GetCell(String.Format("{0}{1}", baseFiguresColumn, reportRow + 1)).ApplyFormula(sumFormulaTemplate.Replace("#FORMULA_BODY#", basePoliciesSoldFormulaBody));
                        }
                        else
                        {
                            wsReport.GetCell(String.Format("{0}{1}", baseFiguresColumn, reportRow + 1)).Value = 0;
                        }

                        if (!String.IsNullOrEmpty(upgradePoliciesSoldFormulaBody))
                        {
                            wsReport.GetCell(String.Format("{0}{1}", upgradeFiguresColumn, reportRow + 1)).ApplyFormula(sumFormulaTemplate.Replace("#FORMULA_BODY#", upgradePoliciesSoldFormulaBody));
                        }
                        else
                        {
                            wsReport.GetCell(String.Format("{0}{1}", upgradeFiguresColumn, reportRow + 1)).Value = 0;
                        }
                        reportRow++;

                        #endregion Total Policies Sold

                        #region Total Actual Confirmed Policies

                        //if (!String.IsNullOrEmpty(cumulativePoliciesConfirmedFormulaBody))
                        //{
                        //    wsReport.GetCell(String.Format("{0}{1}", cumulativeFiguresColumn, reportRow + 1)).ApplyFormula(sumFormulaTemplate.Replace("#FORMULA_BODY#", cumulativePoliciesConfirmedFormulaBody));
                        //}
                        //else
                        //{
                        //    wsReport.GetCell(String.Format("{0}{1}", cumulativeFiguresColumn, reportRow + 1)).Value = 0;
                        //}

                        //if (!String.IsNullOrEmpty(basePoliciesConfirmedFormulaBody))
                        //{
                        //    wsReport.GetCell(String.Format("{0}{1}", baseFiguresColumn, reportRow + 1)).ApplyFormula(sumFormulaTemplate.Replace("#FORMULA_BODY#", basePoliciesConfirmedFormulaBody));
                        //}
                        //else
                        //{
                        //    wsReport.GetCell(String.Format("{0}{1}", baseFiguresColumn, reportRow + 1)).Value = 0;
                        //}

                        //if (!String.IsNullOrEmpty(upgradePoliciesConfirmedFormulaBody))
                        //{
                        //    wsReport.GetCell(String.Format("{0}{1}", upgradeFiguresColumn, reportRow + 1)).ApplyFormula(sumFormulaTemplate.Replace("#FORMULA_BODY#", upgradePoliciesConfirmedFormulaBody));
                        //}
                        //else
                        //{
                        //    wsReport.GetCell(String.Format("{0}{1}", upgradeFiguresColumn, reportRow + 1)).Value = 0;
                        //}
                        //reportRow++;

                        #endregion Total Actual Confirmed Policies

                        #region Total Confirmed %

                        //wsReport.GetCell(String.Format("{0}{1}", cumulativeFiguresColumn, reportRow + 1)).ApplyFormula(confirmedPercentageFormulaTemplate.Replace("#COLUMN#", cumulativeFiguresColumn).Replace("#DENOM_ROW#", (reportRow + totalConfirmedPercentageDenominatorRowIndex).ToString()).Replace("#NUMER_ROW#", (reportRow + totalConfirmedPercentageNumeratorRowIndex).ToString()));
                        //wsReport.GetCell(String.Format("{0}{1}", baseFiguresColumn, reportRow + 1)).ApplyFormula(confirmedPercentageFormulaTemplate.Replace("#COLUMN#", baseFiguresColumn).Replace("#DENOM_ROW#", (reportRow + totalConfirmedPercentageDenominatorRowIndex).ToString()).Replace("#NUMER_ROW#", (reportRow + totalConfirmedPercentageNumeratorRowIndex).ToString()));
                        //wsReport.GetCell(String.Format("{0}{1}", upgradeFiguresColumn, reportRow + 1)).ApplyFormula(confirmedPercentageFormulaTemplate.Replace("#COLUMN#", upgradeFiguresColumn).Replace("#DENOM_ROW#", (reportRow + totalConfirmedPercentageDenominatorRowIndex).ToString()).Replace("#NUMER_ROW#", (reportRow + totalConfirmedPercentageNumeratorRowIndex).ToString()));

                        #endregion Total Confirmed %

                        #endregion Applying the formulas

                        #endregion Add the summary at the bottom of the report
                    }

                    #endregion Insert the partitioned data - if there are any rows available for each partition
                    
                }
                reportRow = 6;

                
            }
        }

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";

            EnableAllControls(true);
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
                    _strTodaysDate = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
                    _strTodaysDateIncludingColons = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    EnableAllControls(false);

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

        private void EnableAllControls(bool isEnabled)
        {
            btnClose.IsEnabled = isEnabled;
            btnReport.IsEnabled = isEnabled;
            calFromDate.IsEnabled = isEnabled;
            calToDate.IsEnabled = isEnabled;
        }

        private void calFromDate_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calFromDate.SelectedDate.ToString(), out _fromDate);
        }

        private void calToDate_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calToDate.SelectedDate.ToString(), out _toDate);
        }

        #endregion Event Handlers

    }
}
