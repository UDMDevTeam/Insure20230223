using System.Linq;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
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
using Infragistics.Windows.Editors.Events;
using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using System.Text.RegularExpressions;
using Infragistics.Windows.DataPresenter;
using System.Collections.Generic;
using UDM.Insurance.Business;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportConfirmedSalesPerAgentScreen
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


        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        private DateTime _fromDate = DateTime.Now;
        private DateTime _toDate = DateTime.Now;
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;
        private bool? _isReportRunning = false;
        private List<Record> _lstSelectedAgents;
        string _agentIDs = String.Empty;
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

        public ReportConfirmedSalesPerAgentScreen()
        {
            InitializeComponent();

            //LoadCampaignClusterInfo();
            LoadAgents();
            

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

        private void LoadAgents()
        {
            DataTable dtAgents = Insure.INGetConfirmationAgents();

            xdgAgents.DataSource = dtAgents.DefaultView;
        }

        private DataTable LoadInternalLookupValues()
        {
            //return UDM.Insurance.Business.Insure.INGetCampaignTypes(false);
            return UDM.Insurance.Business.Insure.INGetConfirmedSalesReportCampaignGroupings();
        }

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

            #region Ensuring that the To Date was specified

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

            #endregion Ensuring that the To Date was specified

            #region Ensuring that the date range is valid

            if (_reportStartDate > _reportEndDate)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Invalid date range specified: The 'From Date' can not be greater than the 'To Date'.", "Invalid date range", ShowMessageType.Error);
                return false;
            }

            #endregion Ensuring that the date range is valid

            #region Ensuring that at least 1 batch was selected

            var lstTemp = (from r in xdgAgents.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            _lstSelectedAgents = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

            if (_lstSelectedAgents.Count == 0)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 agent from the list.", "No agents selected", ShowMessageType.Error);
                return false;
            }
            else
            {
                _agentIDs = _lstSelectedAgents.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                
                _agentIDs = _agentIDs.Substring(0, _agentIDs.Length - 1);
            }

            #endregion Ensuring that at least 1 batch was selected

            // Otherwise, if all is well, proceed:
            return true;
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup Excel document
                int worksheetCount = 0;
                
                var wbConfirmedSalesReport = new Workbook();
                wbConfirmedSalesReport.SetCurrentFormat(WorkbookFormat.Excel2007);

                string filePathAndName = String.Format("{0}Confirmed Sales Report Per Agent ~ {1}.xlsx",
                    GlobalSettings.UserFolder,
                    _strTodaysDate);

                Workbook wbTemplate;
                Uri uri = new Uri("/Templates/ReportTemplateConfirmedSalesPerAgent.xlsx", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    wbTemplate = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }

                #endregion Setup Excel document

                DataTable dtINCampaignTypes = LoadInternalLookupValues();

                #region Step 1: Add the summary page - only if the from- and to dates differ

                if (_lstSelectedAgents.Count > 1)
                {
                    AddReportPage(wbConfirmedSalesReport, wbTemplate, "Summary", "Summary", _reportStartDate.Value, _reportEndDate.Value, dtINCampaignTypes, _agentIDs);
                    worksheetCount++;
                }
                
                #endregion Step 1: Add the summary page

                #region Step 2: Add the re page

                //for (DateTime dtCurrentDate = _reportStartDate.Value; dtCurrentDate <= _reportEndDate.Value; dtCurrentDate = dtCurrentDate.AddDays(1))
                //{
                for (int agentCount = 0; agentCount < _lstSelectedAgents.Count; agentCount++)
                {
                    //if (dtCurrentDate.DayOfWeek != DayOfWeek.Sunday)
                    //{
                        AddReportPage(wbConfirmedSalesReport, wbTemplate, "Summary", ((DataRecord)_lstSelectedAgents[agentCount]).Cells["Description"].Value.ToString(), _reportStartDate.Value, _reportEndDate.Value, dtINCampaignTypes, ((DataRecord)_lstSelectedAgents[agentCount]).Cells["ID"].Value.ToString());
                        worksheetCount++;
                    //}
                }
                    
                //}

                #endregion Step 2: Add the summary page

                #region Finally, save, and display the resulting workbook

                if (worksheetCount > 0)
                {
                    wbConfirmedSalesReport.Save(filePathAndName);

                    //Display excel document
                    Process.Start(filePathAndName);

                }
                else
                {
                    //emptyDataTableCount++;
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

        private void AddReportPage(Workbook wbResultingWorkbook, Workbook wbTemplate, string reportTemplateSheetName, string newWorksheetDescription, DateTime dtFromDate, DateTime dtToDate, DataTable dtINConfirmedSalesReportCampaignGroupings, string fkAgentIDs)
        {
            #region Declaring & initializing variables

            int reportRowIndex = 7;
            int zeroRowsCount = 0;
            int campaignCount = 0;
            string reportPageSubHeading = String.Empty;
            string formulaBody = String.Empty;
            string campaignNotOnTargetFormula = String.Empty;
            string extensionCampaignFormulaBody = String.Empty;

            if (dtFromDate.Date != dtToDate.Date)
            {
                reportPageSubHeading = String.Format("For the period between {0} and {1}", dtFromDate.ToString("dddd, d MMMM yyyy"), dtToDate.ToString("dddd, d MMMM yyyy"));
                //newWorksheetDescription = "Summary";
            }
            else
            {
                reportPageSubHeading = String.Format("For {0}", dtFromDate.ToString("dddd, d MMMM yyyy"));
                //newWorksheetDescription = DateTime.Now.ToString("yyyy-MM-dd");
            }

            Worksheet wsNewWorksheetTemplate = wbTemplate.Worksheets[reportTemplateSheetName];
            Worksheet wsNewWorksheet = wbResultingWorkbook.Worksheets.Add(newWorksheetDescription);

            #endregion Declaring & initializing variables

            #region Copy the template formatting and add the details

            Methods.CopyExcelRegion(wsNewWorksheetTemplate, 0, 0, 4, 13, wsNewWorksheet, 0, 0);

            wsNewWorksheet.GetCell("A3").Value = reportPageSubHeading;
            wsNewWorksheet.GetCell("A5").Value = String.Format("Date generated: {0}", DateTime.Now.ToString("yyyy-MM-dd, HH:mm:ss"));

            if (dtFromDate.Date == dtToDate.Date)
            {
                //wsNewWorksheet.GetCell("L5").Value = String.Format("Date to be batched: {0}", UDM.Insurance.Business.Insure.GetAvailableWorkingDayFromDate(dtFromDate.AddDays(4)).ToString("yyyy-MM-dd"));
                wsNewWorksheet.GetCell("L5").Value = String.Format("Date to be batched: {0}", UDM.Insurance.Business.Insure.INDetermineBatchingDateFromDateOfSale(dtFromDate, 4).ToString("yyyy-MM-dd"));
            }
            else
            {
                wsNewWorksheet.GetCell("E5").Value = null;
            }

            #endregion Copy the template formatting and add the details

            DataSet dsConfirmedSalesPerAgentReportDataFull = Insure.INReportConfirmedSalesPerAgent(fkAgentIDs, dtFromDate, dtToDate);

            DataTable dtConfirmedSalesReportData = dsConfirmedSalesPerAgentReportDataFull.Tables[0];

            DataTable dtExcelSheetTotalsAndAverageColumnMappings = dsConfirmedSalesPerAgentReportDataFull.Tables[1];

            foreach (DataRow drINConfirmedSalesReportCampaignGrouping in dtINConfirmedSalesReportCampaignGroupings.Rows)
            {
                byte campaignGroupingID = Convert.ToByte(drINConfirmedSalesReportCampaignGrouping["ID"]);
                string confirmedSalesReportCampaignGroup = drINConfirmedSalesReportCampaignGrouping["ConfirmedSalesReportCampaignGroup"].ToString();

                #region Get the data from database

                //DataSet dsConfirmedSalesReportDataFull = Business.Insure.INGetConfirmedSalesReportData(campaignGroupingID, dtFromDate, dtToDate);





                #endregion Get the data from database

                DataRow[] dr = dtConfirmedSalesReportData.Select("CampaignGroupID = " + drINConfirmedSalesReportCampaignGrouping["ID"].ToString());

                //if (dtConfirmedSalesReportData.Rows.Count > 0)
                if (dr.Count() > 0)
                {
                    #region Loop through each campaign type, copy the formatting from the template, add the details

                    Methods.CopyExcelRegion(wsNewWorksheetTemplate, 6, 0, 2, 13, wsNewWorksheet, reportRowIndex - 1, 0);
                    //Methods.CopyExcelRegion(wsNewWorksheetTemplate, 6, 0, 2, 13, wsNewWorksheet, reportRowIndex, 0);

                    //wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 1)).Value = confirmedSalesReportCampaignGroup;
                    wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex)).Value = confirmedSalesReportCampaignGroup;

                    reportRowIndex += 3;

                    int formulaStartRow = reportRowIndex - 1;
                    //int formulaStartRow = reportRowIndex;

                    //if (campaignGroupingID != 5)
                    //{
                    //    campaignNotOnTargetFormula += String.Format("COUNTIF(G{0}:G{1},\"<0.8\"),", reportRowIndex, reportRowIndex + (dtConfirmedSalesReportData.Rows.Count - 1));
                    //}

                    switch (campaignGroupingID)
                    {
                        case 1:
                        case 2:
                            campaignNotOnTargetFormula += String.Format("COUNTIF(X{0}:X{1},\"<0.8\"),", reportRowIndex, reportRowIndex + (dr.Count() - 1));
                            break;
                        case 3:
                        case 4:
                            campaignNotOnTargetFormula += String.Format("COUNTIF(Y{0}:Y{1},\"<0.8\"),", reportRowIndex, reportRowIndex + (dr.Count() - 1));
                            break;
                        default:
                            campaignNotOnTargetFormula += String.Format("COUNTIF(G{0}:G{1},\"<0.8\"),", reportRowIndex, reportRowIndex + (dr.Count() - 1));
                            break;
                    }



                    //foreach (DataRow drConfirmedSalesReportData in dtConfirmedSalesReportData.Rows)
                    foreach (DataRow drConfirmedSalesReportData in dr)
                    {
                        Methods.CopyExcelRegion(wsNewWorksheetTemplate, 9, 0, 0, 13, wsNewWorksheet, reportRowIndex - 1, 0);
                        wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex)).Value = drConfirmedSalesReportData["CampaignName"];
                        wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = drConfirmedSalesReportData["CampaignCode"];

                        wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).Value = drConfirmedSalesReportData["Sales"];
                        wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).Value = drConfirmedSalesReportData["ConfirmationTarget"];
                        wsNewWorksheet.GetCell(String.Format("E{0}", reportRowIndex)).Value = drConfirmedSalesReportData["ActualConfirmedPolicies"];

                        wsNewWorksheet.GetCell(String.Format("F{0}", reportRowIndex)).ApplyFormula(String.Format("=D{0}-E{0}", reportRowIndex));
                        wsNewWorksheet.GetCell(String.Format("G{0}", reportRowIndex)).ApplyFormula(String.Format("=E{0}/C{0}", reportRowIndex));

                        wsNewWorksheet.GetCell(String.Format("H{0}", reportRowIndex)).Value = drConfirmedSalesReportData["SalesforMonth"];
                        wsNewWorksheet.GetCell(String.Format("I{0}", reportRowIndex)).Value = drConfirmedSalesReportData["ActualConfirmedPoliciesForMonth"];
                        wsNewWorksheet.GetCell(String.Format("J{0}", reportRowIndex)).ApplyFormula(String.Format("=I{0}/H{0}", reportRowIndex));


                        //wsNewWorksheet.GetCell(String.Format("J{0}", reportRowIndex)).CellFormat.FormatString = "0%";


                        //if (campaignGroupingID != 5)
                        //{
                        switch(campaignGroupingID)
                        {
                            case 1:
                            case 2:
                                formulaBody += String.Format("X{0},", reportRowIndex);
                                break;
                            case 3:
                            case 4:
                                formulaBody += String.Format("Y{0},", reportRowIndex);
                                break;
                            default:
                                formulaBody += String.Format("X{0},", reportRowIndex);
                                break;
                        }
                            //formulaBody += String.Format("X{0},", reportRowIndex);
                        //}
                        //else
                        //{
                        //    extensionCampaignFormulaBody += String.Format("X{0},", reportRowIndex);
                        //}
                        //campaignCount++;
                        reportRowIndex++;
                    }

                    

                    reportRowIndex = Methods.MapTemplatizedExcelFormulas(wsNewWorksheetTemplate, dtExcelSheetTotalsAndAverageColumnMappings, 10, 0, 0, 14, wsNewWorksheet, reportRowIndex - 1, 0, formulaStartRow, reportRowIndex - 2);
                    reportRowIndex++;
                    #region Add the "Totals & averages row"

                    //Methods.CopyExcelRegion(wsNewWorksheetTemplate, 11, 0, 0, 7, wsNewWorksheet, reportRowIndex - 1, 0);
                    ////Methods.CopyExcelRegion(wsNewWorksheetTemplate, 11, 0, 0, 7, wsNewWorksheet, reportRowIndex, 0);

                    #region Last minute string manipulation to prepare the formula body of the totals & averages formulae

                    //// Remove the trailing semicolon (;)
                    //if (formulaBody.Length > 0)
                    //{
                    //    if (formulaBody.EndsWith(","))
                    //    {
                    //        formulaBody = formulaBody.Substring(0, formulaBody.Length - 1);
                    //    }
                    //}

                    #endregion Last minute string manipulation to prepare the formula body of the totals & averages formulae

                    //if (formulaBody != String.Empty)
                    //{
                    //    if (zeroRowsCount == dtINConfirmedSalesReportCampaignGroupings.Rows.Count)
                    //    {
                    //        wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).Value = 0;
                    //        wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).Value = 0;
                    //        wsNewWorksheet.GetCell(String.Format("E{0}", reportRowIndex)).Value = 0;
                    //        wsNewWorksheet.GetCell(String.Format("F{0}", reportRowIndex)).Value = 0;
                    //        wsNewWorksheet.GetCell(String.Format("G{0}", reportRowIndex)).Value = 0;
                    //    }

                    //    else
                    //    {
                    //        string currentColumnFormula = String.Format("=COUNT({0})", formulaBody.Replace('X', 'A'));
                    //        wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);

                    //        currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('X', 'C'));
                    //        wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);

                    //        currentColumnFormula = String.Format("=AVERAGE({0})", formulaBody.Replace('X', 'D'));
                    //        wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);

                    //        currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('X', 'E'));
                    //        wsNewWorksheet.GetCell(String.Format("E{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);

                    //        currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('X', 'F'));
                    //        wsNewWorksheet.GetCell(String.Format("F{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);

                    //        currentColumnFormula = String.Format("=AVERAGE({0})", formulaBody.Replace('X', 'G'));
                    //        wsNewWorksheet.GetCell(String.Format("G{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);

                    //        currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('X', 'H'));
                    //        wsNewWorksheet.GetCell(String.Format("H{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                    //    }
                    //}

                    //reportRowIndex++;

                    #endregion Add the "Totals & averages row"                  
                }
                else
                {
                    #region Otherwise, add an indication that there are no data for the campaign type

                    Methods.CopyExcelRegion(wsNewWorksheetTemplate, 6, 0, 0, 13, wsNewWorksheet, reportRowIndex - 1, 0);
                    //Methods.CopyExcelRegion(wsNewWorksheetTemplate, 6, 0, 0, 13, wsNewWorksheet, reportRowIndex, 0);
                    wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex)).Value = confirmedSalesReportCampaignGroup;

                    ++zeroRowsCount;
                    reportRowIndex += 1;

                    WorksheetMergedCellsRegion mergedRegion;
                    mergedRegion = wsNewWorksheet.MergedCellsRegions.Add(reportRowIndex - 1, 0, reportRowIndex - 1, 13);
                    //mergedRegion = wsNewWorksheet.MergedCellsRegions.Add(reportRowIndex, 0, reportRowIndex, 13);
                    mergedRegion.CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
                    mergedRegion.CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                    mergedRegion.CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                    mergedRegion.CellFormat.TopBorderStyle = CellBorderLineStyle.Double;
                    mergedRegion.Value = "No data available for this campaign type.";
                    mergedRegion.CellFormat.Alignment = HorizontalCellAlignment.Center;
                    mergedRegion.CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                    //if (campaignGroupingID != 5)
                    //{
                    switch (campaignGroupingID)
                    {
                        case 1:
                        case 2:
                            formulaBody += String.Format("X{0},", reportRowIndex);
                            break;
                        case 3:
                        case 4:
                            formulaBody += String.Format("Y{0},", reportRowIndex);
                            break;
                        default:
                            formulaBody += String.Format("X{0},", reportRowIndex);
                            break;
                    }
                    //formulaBody += String.Format("X{0},", reportRowIndex);
                    //}
                    //else
                    //{
                    //    extensionCampaignFormulaBody += String.Format("X{0},", reportRowIndex);
                    //}
                    reportRowIndex++;

                    #endregion Otherwise, add an indication that there are no data for the campaign type
                }

                #endregion Loop through each campaign type, copy the formatting from the template and add the details


                //if (formulaStartingRow == reportRowIndex)
                //{
                //    formulaBody += String.Format("X{0};", formulaStartingRow);
                //}
                //else
                //{
                //    formulaBody += String.Format("X{0}:X{1};", formulaStartingRow, reportRowIndex);
                //}

                //formulaBody = String.Empty;
            }

            

            #region Add the summary at the bottom of the report

            Methods.CopyExcelRegion(wsNewWorksheetTemplate, 14, 0, 5, 4, wsNewWorksheet, reportRowIndex, 0);

            if (newWorksheetDescription == "Summary")
            {
                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 1)).Value = "Grand totals for period";
            }
            else
            {
                wsNewWorksheet.GetCell(String.Format("A{0}", reportRowIndex + 1)).Value = String.Format("Grand totals for agent {0}", newWorksheetDescription);
            }
            
            reportRowIndex += 2;

            #region Last minute string manipulation to prepare the formula body of the totals & averages formulae

            // Remove the trailing semicolon (;)
            if (formulaBody.Length > 0)
            {
                if (formulaBody.EndsWith(","))
                {
                    formulaBody = formulaBody.Substring(0, formulaBody.Length - 1);
                }
            }

            if (campaignNotOnTargetFormula.Length > 0)
            {
                if (campaignNotOnTargetFormula.EndsWith(","))
                {
                    campaignNotOnTargetFormula = campaignNotOnTargetFormula.Substring(0, campaignNotOnTargetFormula.Length - 1);
                }
            }

            if (extensionCampaignFormulaBody.Length > 0)
            {
                if (extensionCampaignFormulaBody.EndsWith(","))
                {
                    extensionCampaignFormulaBody = extensionCampaignFormulaBody.Substring(0, extensionCampaignFormulaBody.Length - 1);
                }
            } 

            #endregion Last minute string manipulation to prepare the formula body of the totals & averages formulae

            //if (formulaBody != String.Empty)
            //{
            if (zeroRowsCount == dtINConfirmedSalesReportCampaignGroupings.Rows.Count)
            {
                for (int a = reportRowIndex; reportRowIndex < a + 5; reportRowIndex++)
                {
                    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = 0;
                    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).Value = 0;
                    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).Value = 0;
                }
                //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = 0;
                //reportRowIndex++;
                //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = 0;
                //reportRowIndex++;
                //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = 0;
                //reportRowIndex++;
                //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = 0;
                //reportRowIndex++;
                //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).Value = 0;
                //reportRowIndex++;
            }

            else
            {
                Regex pattern = new Regex("[XY]");
                //string currentColumnFormula;
                int xyStatus = 0;

                #region complicatedCalculations to remove X or Y when calculating for base or upgrades
                //if (formulaBody.Contains('X') & formulaBody.Contains('Y'))
                //{
                //    //Total Campaigns
                //    currentColumnFormula = String.Format("=COUNTA({0})", pattern.Replace(formulaBody, "A"));
                //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
                //    currentColumnFormula = String.Format("=COUNTA({0})", currentColumnFormula.Replace('X', 'A'));
                //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
                //    currentColumnFormula = String.Format("=COUNTA({0})", currentColumnFormula.Replace('Y', 'A'));
                //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    reportRowIndex++;

                //    if (campaignNotOnTargetFormula.Contains('X') & campaignNotOnTargetFormula.Contains('Y'))
                //    {
                //        //Total Campaigns not on 80%
                //        //currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula);                
                //        currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(campaignNotOnTargetFormula, "G"));
                //        wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //        currentColumnFormula = campaignNotOnTargetFormula.Substring(0, campaignNotOnTargetFormula.IndexOf('Y') - 9);
                //        currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'G'));
                //        wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //        currentColumnFormula = campaignNotOnTargetFormula.Substring(campaignNotOnTargetFormula.IndexOf('Y') - 8);
                //        currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'G'));
                //        wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    }
                //    else if (campaignNotOnTargetFormula.Contains('X') & !(campaignNotOnTargetFormula.Contains('Y')))
                //    {
                //        //Total Campaigns not on 80%
                //        //currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula);                
                //        currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(campaignNotOnTargetFormula, "G"));
                //        wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //        //currentColumnFormula = campaignNotOnTargetFormula.Substring(0, campaignNotOnTargetFormula.IndexOf('Y') - 9);
                //        currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula.Replace('X', 'G'));
                //        wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //        //currentColumnFormula = campaignNotOnTargetFormula.Substring(campaignNotOnTargetFormula.IndexOf('Y') - 8);
                //        //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'G'));
                //        //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    }
                //    else if (!(campaignNotOnTargetFormula.Contains('X')) & campaignNotOnTargetFormula.Contains('Y'))
                //    {
                //        //Total Campaigns not on 80%
                //        //currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula);                
                //        currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(campaignNotOnTargetFormula, "G"));
                //        wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //        //currentColumnFormula = campaignNotOnTargetFormula.Substring(0, campaignNotOnTargetFormula.IndexOf('Y') - 9);
                //        //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'G'));
                //        //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //        //currentColumnFormula = campaignNotOnTargetFormula.Substring(campaignNotOnTargetFormula.IndexOf('Y') - 8);
                //        currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula.Replace('Y', 'G'));
                //        wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    }

                //    reportRowIndex++;

                //    //Total Policies Sold
                //    currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "C"));
                //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
                //    currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'C'));
                //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
                //    currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'C'));
                //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    reportRowIndex++;

                //    //Total Actual Confirmed Policies
                //    currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "E"));
                //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
                //    currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'E'));
                //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
                //    currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'E'));
                //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    reportRowIndex++;

                //    //Total Confirmed %
                //    //currentColumnFormula = String.Format("=AVERAGE({0})", formulaBody.Replace('X', 'G'));
                //    currentColumnFormula = String.Format("=B{0}/B{1}", reportRowIndex - 1, reportRowIndex - 2);
                //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    currentColumnFormula = String.Format("=C{0}/C{1}", reportRowIndex - 1, reportRowIndex - 2);
                //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    currentColumnFormula = String.Format("=D{0}/D{1}", reportRowIndex - 1, reportRowIndex - 2);
                //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //reportRowIndex++;
                //}
                //else if (formulaBody.Contains('X') & !(formulaBody.Contains('Y')))
                //{
                //    //Total Campaigns
                //    currentColumnFormula = String.Format("=COUNTA({0})", pattern.Replace(formulaBody, "A"));
                //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
                //    currentColumnFormula = String.Format("=COUNTA({0})", formulaBody.Replace('X', 'A'));
                //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
                //    //currentColumnFormula = String.Format("=COUNTA({0})", currentColumnFormula.Replace('Y', 'A'));
                //    //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    reportRowIndex++;

                //    //Total Campaigns not on 80%
                //    //currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula);                
                //    currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(campaignNotOnTargetFormula, "G"));
                //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //currentColumnFormula = campaignNotOnTargetFormula.Substring(0, campaignNotOnTargetFormula.IndexOf('Y') - 9);
                //    currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula.Replace('X', 'G'));
                //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //currentColumnFormula = campaignNotOnTargetFormula.Substring(campaignNotOnTargetFormula.IndexOf('Y') - 8);
                //    //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'G'));
                //    //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    reportRowIndex++;

                //    //Total Policies Sold
                //    currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "C"));
                //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
                //    currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('X', 'C'));
                //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
                //    //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'C'));
                //    //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    reportRowIndex++;

                //    //Total Actual Confirmed Policies
                //    currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "E"));
                //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
                //    currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('X', 'E'));
                //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
                //    //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'E'));
                //    //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    reportRowIndex++;

                //    //Total Confirmed %
                //    //currentColumnFormula = String.Format("=AVERAGE({0})", formulaBody.Replace('X', 'G'));
                //    currentColumnFormula = String.Format("=B{0}/B{1}", reportRowIndex - 1, reportRowIndex - 2);
                //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    currentColumnFormula = String.Format("=C{0}/C{1}", reportRowIndex - 1, reportRowIndex - 2);
                //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    currentColumnFormula = String.Format("=D{0}/D{1}", reportRowIndex - 1, reportRowIndex - 2);
                //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //reportRowIndex++;
                //}
                //else if (!(formulaBody.Contains('X')) & formulaBody.Contains('Y'))
                //{
                //    //Total Campaigns
                //    currentColumnFormula = String.Format("=COUNTA({0})", pattern.Replace(formulaBody, "A"));
                //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
                //    //currentColumnFormula = String.Format("=COUNTA({0})", currentColumnFormula.Replace('X', 'A'));
                //    //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
                //    currentColumnFormula = String.Format("=COUNTA({0})", formulaBody.Replace('Y', 'A'));
                //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    reportRowIndex++;

                //    //Total Campaigns not on 80%
                //    //currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula);                
                //    currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(campaignNotOnTargetFormula, "G"));
                //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //currentColumnFormula = campaignNotOnTargetFormula.Substring(0, campaignNotOnTargetFormula.IndexOf('Y') - 9);
                //    //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'G'));
                //    //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //currentColumnFormula = campaignNotOnTargetFormula.Substring(campaignNotOnTargetFormula.IndexOf('Y') - 8);
                //    currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula.Replace('Y', 'G'));
                //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    reportRowIndex++;

                //    //Total Policies Sold
                //    currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "C"));
                //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
                //    //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'C'));
                //    //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
                //    currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('Y', 'C'));
                //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    reportRowIndex++;

                //    //Total Actual Confirmed Policies
                //    currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "E"));
                //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
                //    //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'E'));
                //    //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
                //    currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('Y', 'E'));
                //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    reportRowIndex++;

                //    //Total Confirmed %
                //    //currentColumnFormula = String.Format("=AVERAGE({0})", formulaBody.Replace('X', 'G'));
                //    currentColumnFormula = String.Format("=B{0}/B{1}", reportRowIndex - 1, reportRowIndex - 2);
                //    wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    currentColumnFormula = String.Format("=C{0}/C{1}", reportRowIndex - 1, reportRowIndex - 2);
                //    wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    currentColumnFormula = String.Format("=D{0}/D{1}", reportRowIndex - 1, reportRowIndex - 2);
                //    wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //    //reportRowIndex++;
                //}
                //else
                //{
                //    xyStatus = 0;
                //}
                #endregion
                #region New Calculations
                //NEW CALCULATIONS
                //Total Campaigns
                string currentColumnFormula = String.Format("=COUNTA({0})", pattern.Replace(formulaBody, "A"));
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
                currentColumnFormula = String.Format("=COUNTA({0})", formulaBody.Replace('X', 'A'));
                wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
                currentColumnFormula = String.Format("=COUNTA({0})", formulaBody.Replace('Y', 'A'));
                wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                reportRowIndex++;

                //Total Campaigns not on 80%
                //currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula);                
                currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(campaignNotOnTargetFormula, "G"));
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = campaignNotOnTargetFormula.Substring(0, campaignNotOnTargetFormula.IndexOf('Y') - 9);
                currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula.Replace('X', 'G'));
                wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = campaignNotOnTargetFormula.Substring(campaignNotOnTargetFormula.IndexOf('Y') - 8);
                currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula.Replace('Y', 'G'));
                wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                reportRowIndex++;

                //Total Policies Sold
                currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "C"));
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
                currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('X', 'C'));
                wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
                currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('Y', 'C'));
                wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                reportRowIndex++;

                //Total Actual Confirmed Policies
                currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "E"));
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
                currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('X', 'E'));
                wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
                currentColumnFormula = String.Format("=SUM({0})", formulaBody.Replace('Y', 'E'));
                wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                reportRowIndex++;

                //Total Confirmed %
                //currentColumnFormula = String.Format("=AVERAGE({0})", formulaBody.Replace('X', 'G'));
                currentColumnFormula = String.Format("=B{0}/B{1}", reportRowIndex - 1, reportRowIndex - 2);
                wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                currentColumnFormula = String.Format("=C{0}/C{1}", reportRowIndex - 1, reportRowIndex - 2);
                wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                currentColumnFormula = String.Format("=D{0}/D{1}", reportRowIndex - 1, reportRowIndex - 2);
                wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //reportRowIndex++;
                #endregion


                #region OldCalculations
                //OLD CALCULATIONS
                ////Total Campaigns
                //string currentColumnFormula = String.Format("=COUNTA({0})", pattern.Replace(formulaBody, "A"));
                //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
                //currentColumnFormula = String.Format("=COUNTA({0})", currentColumnFormula.Replace('X', 'A'));
                //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
                //currentColumnFormula = String.Format("=COUNTA({0})", currentColumnFormula.Replace('Y', 'A'));
                //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //reportRowIndex++;

                ////Total Campaigns not on 80%
                ////currentColumnFormula = String.Format("=SUM({0})", campaignNotOnTargetFormula);                
                //currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(campaignNotOnTargetFormula, "G"));
                //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = campaignNotOnTargetFormula.Substring(0, campaignNotOnTargetFormula.IndexOf('Y') - 9);
                //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'G'));
                //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = campaignNotOnTargetFormula.Substring(campaignNotOnTargetFormula.IndexOf('Y') - 8);
                //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'G'));
                //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //reportRowIndex++;

                ////Total Policies Sold
                //currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "C"));
                //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
                //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'C'));
                //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
                //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'C'));
                //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //reportRowIndex++;

                ////Total Actual Confirmed Policies
                //currentColumnFormula = String.Format("=SUM({0})", pattern.Replace(formulaBody, "E"));
                //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = formulaBody.Substring(0, formulaBody.IndexOf('Y') - 1);
                //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('X', 'E'));
                //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = formulaBody.Substring(formulaBody.IndexOf('Y'));
                //currentColumnFormula = String.Format("=SUM({0})", currentColumnFormula.Replace('Y', 'E'));
                //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //reportRowIndex++;

                ////Total Confirmed %
                ////currentColumnFormula = String.Format("=AVERAGE({0})", formulaBody.Replace('X', 'G'));
                //currentColumnFormula = String.Format("=B{0}/B{1}", reportRowIndex - 1, reportRowIndex - 2);
                //wsNewWorksheet.GetCell(String.Format("B{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = String.Format("=C{0}/C{1}", reportRowIndex - 1, reportRowIndex - 2);
                //wsNewWorksheet.GetCell(String.Format("C{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                //currentColumnFormula = String.Format("=D{0}/D{1}", reportRowIndex - 1, reportRowIndex - 2);
                //wsNewWorksheet.GetCell(String.Format("D{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
                ////reportRowIndex++;
                #endregion

            }
            //}

            //if (extensionCampaignFormulaBody != String.Empty)
            //{
            //    reportRowIndex -= 4;
            //    if (zeroRowsCount == dtINConfirmedSalesReportCampaignGroupings.Rows.Count)
            //    {
            //        //Total Campaigns
            //        wsNewWorksheet.GetCell(String.Format("E{0}", reportRowIndex)).Value = 0;
            //        reportRowIndex++;

            //        //Total Policies Sold
            //        wsNewWorksheet.GetCell(String.Format("E{0}", reportRowIndex)).Value = 0;
            //        reportRowIndex++;
            //    }
            //    else
            //    {
            //        //Total Campaigns
            //        string currentColumnFormula = String.Format("=COUNTA({0})", extensionCampaignFormulaBody.Replace('X', 'A'));
            //        wsNewWorksheet.GetCell(String.Format("E{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
            //        reportRowIndex++;

            //        //Total Policies Sold
            //        currentColumnFormula = String.Format("=SUM({0})", extensionCampaignFormulaBody.Replace('X', 'C'));
            //        wsNewWorksheet.GetCell(String.Format("E{0}", reportRowIndex)).ApplyFormula(currentColumnFormula);
            //        reportRowIndex++;
            //    }
            //}
            #endregion Add the summary at the bottom of the report
        }

        //private void AddReportPage(Workbook wbResultingWorkbook, Workbook wbTemplate, string reportTemplateSheetName, string newWorksheetDescription, DateTime dtDate, DataTable dtINCampaignTypes)
        //{
        //    AddReportPage(wbResultingWorkbook, wbTemplate, reportTemplateSheetName, newWorksheetDescription, dtDate, dtDate, dtINCampaignTypes);
        //}

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

        private bool? AllRecordsSelected()
        {
            try
            {
                bool allSelected = true;
                bool noneSelected = true;

                if (xdgAgents.DataSource != null)
                {
                    foreach (DataRow dr in ((DataView)xdgAgents.DataSource).Table.Rows)
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

        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = ((DataView)xdgAgents.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = true;
                }

                //EnableDisableExportButton();
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
                DataTable dt = ((DataView)xdgAgents.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = false;
                }

                //EnableDisableExportButton();
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

                //EnableDisableExportButton();
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

        #endregion Event Handlers


    }
}
