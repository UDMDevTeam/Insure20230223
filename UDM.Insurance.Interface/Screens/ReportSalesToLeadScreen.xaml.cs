using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Resources;
using System.Windows.Threading;
using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportSalesToLeadScreen.xaml
    /// </summary>
    public partial class ReportSalesToLeadScreen
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
        private List<Record> _lstSelectedCampaigns;

        //private DateTime _fromDate = DateTime.Now;
        //private DateTime _toDate = DateTime.Now;
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;
        private bool? _isReportRunning = false;
        bool _isUpgradeSTLReport = false;

        private DateTime _reportStartDate;
        private DateTime _reportEndDate;

        private DataTable _dtIndividualLeads;
        private DataTable _dtConversionSummarySheetData;
        private DataTable _dtAllCampaigns;
        private DataTable _dtSTLOptions;
        private DataTable _dtSTLConversionPercentageOption;


        private bool _hasSummaryPageConversionTargetsBeenUpdated;

        private byte _stlOption;
        private byte _stlOptionFromCmb;
        private byte _stlConversionPercentageOption;
        private byte _staffType;

        private bool level2Checked = false;

        string _fkINCampaignIDs = String.Empty;

        private int _conversionSummarySheetRowIndex = 0;

        private string _liveDebugTestIndicator = String.Empty;

        private ReportData _rData = new ReportData();

        public ReportData RData
        {
            get
            {
                return _rData;
            }
            set
            {
                _rData = value;
            }
        }

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

        public ReportSalesToLeadScreen()
        {
            InitializeComponent();
            LoadLookupData();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);

            #if DEBUG
            _liveDebugTestIndicator = "DEBUG";
            #elif TESTBUILD
            _liveDebugTestIndicator = "TEST";
            #else
            _liveDebugTestIndicator = String.Empty;
            #endif
        }

        #endregion Constructors

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

        private void LoadLookupData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataSet dsDropDownItemLists = Business.Insure.INGetSTLLookups();

                _dtAllCampaigns = dsDropDownItemLists.Tables[0];

                DataTable dtSTLReportStaffTypes = dsDropDownItemLists.Tables[1];
                cmbStaffType.Populate(dtSTLReportStaffTypes, "Description", "ID");

                DataTable dtBaseOrUpgradeCampaigns = dsDropDownItemLists.Tables[2];
                cmbBaseOrUpgrade.Populate(dtBaseOrUpgradeCampaigns, "Description", "ID");

                _dtSTLOptions = dsDropDownItemLists.Tables[3];
                //cmbSTLOption.Populate(dtSTLOptions, "Description", "ID");

                _dtSTLConversionPercentageOption = dsDropDownItemLists.Tables[4];
                //cmbSTLConversionPercentageOption.Populate(dtSTLConversionPercentageOption, "Description", "ID");
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

            #region Finally, if there were any campaigns from which no leads were allocated, display a message

            //if (_noDataCampaigns.Count > 0)
            //{
            //    System.Text.StringBuilder campaignList = new System.Text.StringBuilder();
            //    string message = String.Empty;

            //    foreach (string currentCampaign in _noDataCampaigns)
            //    {
            //        campaignList.AppendLine(String.Format("* {0}", currentCampaign));
            //    }

            //    message = String.Format("The STL report could not be generated for the following {0} campaigns, because there were no leads allocated:{1}{2}",
            //        _noDataCampaigns.Count,
            //        Environment.NewLine,
            //        campaignList.ToString());

            //    ShowMessageBox(new INMessageBoxWindow1(), message, "Skipped Campaigns", ShowMessageType.Information);

            //}

            #endregion Finally, if there were any campaigns from which no leads were allocated, display a message
        }

        private bool IsAllInputParametersSpecifiedAndValid()
        {

            #region Ensuring that the user indicated whether the user wants to generate a STL report for the base or upgrade campaigns

            if (cmbBaseOrUpgrade.SelectedItem == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please indicate if you are generating the STL report for base or upgrade campaigns.", "Base or Upgrade", Embriant.Framework.ShowMessageType.Error);
                return false;
            }
            else
            {
                _isUpgradeSTLReport = Convert.ToBoolean((cmbBaseOrUpgrade.SelectedItem as DataRowView).Row["IsUpgrade"]);
            }

            #endregion Ensuring that the user indicated whether the user wants to generate a STL report for the base or upgrade campaigns

            #region Ensuring that at least 1 campaign was selected

            var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

            if (_lstSelectedCampaigns.Count == 0)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No campaign selected", Embriant.Framework.ShowMessageType.Error);
                return false;
            }
            else
            {
                _fkINCampaignIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                _fkINCampaignIDs = _fkINCampaignIDs.Substring(0, _fkINCampaignIDs.Length - 1);
            }

            #endregion Ensuring that at least 1 campaign was selected

            #region Ensuring that the From Date was specified

            if (calFromDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'From Date'.", @"No 'From Date' specified", ShowMessageType.Error);
                return false;
            }
            else
            {
                _reportStartDate = calFromDate.SelectedDate.Value;
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the To Date was specified

            if (calToDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'To Date'.", @"No 'To Date' specified", Embriant.Framework.ShowMessageType.Error);
                return false;
            }

            else
            {
                _reportEndDate = calToDate.SelectedDate.Value;
            }

            #endregion Ensuring that the To Date was specified

            #region Ensuring that the date range is valid

            if (_reportStartDate > _reportEndDate)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Invalid date range specified: The 'From Date' can not be greater than the 'To Date'.", "Invalid date range", Embriant.Framework.ShowMessageType.Error);
                return false;
            }

            //else
            //{
            //    _dateRanges = new List<Tuple<DateTime, DateTime, byte>>();
            //    _dateRanges = DetermineDateRanges(_week134CutOverDate, _reportStartDate, _reportEndDate);
            //}


            #endregion Ensuring that the date range is valid

            #region Ensuring that the STL Option was selected - Base campaigns only

            //if (!_isUpgradeSTLReport)
            //{
            if (cmbSTLOption.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select the STL Option.", " STL Option not selected", ShowMessageType.Error);
                return false;
            }
            else
            {
                _stlOptionFromCmb = Convert.ToByte(cmbSTLOption.SelectedValue);
                if (Enumerable.Range(10,3).Contains(Convert.ToByte(cmbSTLOption.SelectedValue)))
                {
                    _stlOption = (byte)(Convert.ToByte(cmbSTLOption.SelectedValue) - (byte)5);
                    
                }
                else
                {
                    _stlOption = Convert.ToByte(cmbSTLOption.SelectedValue);
                    
                }
                    
            }
            //}

            #endregion Ensuring that the STL Option was selected - Base campaigns only

            #region Ensuring that the STL Conversion Percentage Option was selected - Base campaigns only

            //if (!_isUpgradeSTLReport)
            //{
                if (cmbSTLConversionPercentageOption.SelectedValue == null)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select which conversion percentages should be in effect for the report.", " Conversion percentages not selected", ShowMessageType.Error);
                    return false;
                }
                else
                {
                    _stlConversionPercentageOption = Convert.ToByte(cmbSTLConversionPercentageOption.SelectedValue);
                }
            //}

            #endregion Ensuring that the STL Conversion Percentage Option was selected - Base campaigns only

            #region Ensuring that the Staff Type was selected

            if (cmbStaffType.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please select the Staff Type.", @"Staff Type not selected", Embriant.Framework.ShowMessageType.Error);
                return false;
            }
            else
            {
                _staffType = Convert.ToByte(cmbStaffType.SelectedValue);
            }

            #endregion Ensuring that the Staff Type was selected

            // Otherwise if all is well, proceed:
            return true;
        }

        #region OLD
        
        //private void AddSummarySheet(Workbook wbResultingWorkbook, Worksheet wsTemplate, string campaignName, DateTime fromDate, DateTime toDate, DataTable dtSalesConsultants, int stlOption /*, int stlReportType*/)
        //{
        //    #region Declarations

        //    int reportRow = 10;
        //    int reportTemplateRowIndex = 10;
        //    int templateColumnSpan = 0;

        //    switch (stlOption)
        //    {
        //        case 1:
        //        case 2:
        //            templateColumnSpan = 71;
        //            break;

        //        case 3:
        //        case 4:
        //            templateColumnSpan = 84;
        //            break;
        //    }

        //    string averageConversionFormula = String.Empty;

        //    Worksheet wsReportSummarySheet;

        //    #endregion Declarations

        //    #region Add the new worksheet

        //    string newWorksheetDescription = Methods.ParseWorksheetName(wbResultingWorkbook, "Conversion Summary");

        //    wsReportSummarySheet = wbResultingWorkbook.Worksheets.Add(newWorksheetDescription);
        //    Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsReportSummarySheet, true, true, true, true, true, true, true, false, true, true, true, true, true, true, true, true, false);

        //    wsReportSummarySheet.PrintOptions.ScalingType = ScalingType.FitToPages;
        //    wsReportSummarySheet.PrintOptions.MaxPagesHorizontally = 1;

        //    #endregion Add the new worksheet

        //    #region Step 1: Copy a region from the template that consists of the headings and the column headings

        //    Methods.CopyExcelRegion(wsTemplate, 0, 0, 9, templateColumnSpan, wsReportSummarySheet, 0, 0);

        //    //byte firstSTLWeekNumber = 0;
        //    //byte secondSTLWeekNumber = 0;
        //    //byte thirdSTLWeekNumber = 0;

        //    switch (stlOption)
        //    {
        //        case 1:
        //            wsReportSummarySheet.GetCell("AI9").Value = "Week 2";
        //            wsReportSummarySheet.GetCell("AM9").Value = "Week 4";
        //            wsReportSummarySheet.GetCell("AQ9").Value = "Week 6";
        //            break;

        //        case 2:
        //            wsReportSummarySheet.GetCell("AI9").Value = "Week 1";
        //            wsReportSummarySheet.GetCell("AM9").Value = "Week 3";
        //            wsReportSummarySheet.GetCell("AQ9").Value = "Week 4";
        //            break;
        //    }

        //    #endregion Step 1: Copy a region from the template that consists of the headings and the column headings

        //    #region Step 2: Populate the report details and append the correct conversion target percentages underneath the week 2, 4 and 6 columns

        //    wsReportSummarySheet.GetCell("A7").Value = String.Format("{0} conversion summary for the period between {1} and {2} ",
        //        campaignName,
        //        fromDate.ToString("d MMMM yyyy"),
        //        toDate.ToString("d MMMM yyyy"));

        //    #endregion Step 2: Populate the report details and append the correct conversion target percentages underneath the week 2, 4 and 6 columns

        //    #region Step 3: For each entry in dtSalesConsultants, add a templatized row

        //    for (int a = 0; a < dtSalesConsultants.Rows.Count; a++)
        //    {

        //        Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, templateColumnSpan, wsReportSummarySheet, reportRow, 0);
        //        reportRow++;
        //    }

        //    #endregion Step 3: For each entry in dtSalesConsultants, add a templatized row

        //    #region Add the totals & averages row

        //    reportTemplateRowIndex = 11;
        //    Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, templateColumnSpan, wsReportSummarySheet, reportRow, 0);

        //    wsReportSummarySheet.GetCell(String.Format("P{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(P11:P{0})", reportRow));
        //    wsReportSummarySheet.GetCell(String.Format("S{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(S11:S{0})", reportRow));
        //    wsReportSummarySheet.GetCell(String.Format("V{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(V11:V{0})", reportRow));
        //    wsReportSummarySheet.GetCell(String.Format("Z{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(Z11:Z{0})", reportRow));
        //    wsReportSummarySheet.GetCell(String.Format("AD{0}", reportRow + 1)).ApplyFormula(String.Format("=V{0}/P{0}", reportRow + 1));

        //    switch (stlOption)
        //    {
        //        case 1:
        //        case 2:
        //            wsReportSummarySheet.GetCell(String.Format("AI{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(AI11:AI{0})", reportRow));
        //            wsReportSummarySheet.GetCell(String.Format("AM{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(AM11:AM{0})", reportRow));
        //            wsReportSummarySheet.GetCell(String.Format("AQ{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(AQ11:AQ{0})", reportRow));
        //            wsReportSummarySheet.GetCell(String.Format("AU{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(AU11:AU{0})", reportRow));
        //            wsReportSummarySheet.GetCell(String.Format("AY{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(AY11:AY{0})", reportRow));
        //            wsReportSummarySheet.GetCell(String.Format("BC{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(BC11:BC{0})", reportRow)); // Total Contacts
        //            wsReportSummarySheet.GetCell(String.Format("BG{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(BG11:BG{0})", reportRow)); // % Contact To Lead
        //            wsReportSummarySheet.GetCell(String.Format("BL{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(BL11:BN{0})", reportRow)); // Contacts Remaining
        //            wsReportSummarySheet.GetCell(String.Format("BQ{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(BQ11:BQ{0})", reportRow)); // Fall Offs

        //            break;

        //        case 3:
        //        case 4:

        //            wsReportSummarySheet.GetCell(String.Format("AI{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(AI11:AI{0})", reportRow)); // Week 1 Sales
        //            wsReportSummarySheet.GetCell(String.Format("AM{0}", reportRow + 1)).ApplyFormula(Methods.GenerateExcelFractionFormulaWithZeroNumeratorChecking("AI", 11, reportRow, reportRow + 1, "P")); // Week 1 %

        //            wsReportSummarySheet.GetCell(String.Format("AQ{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(AQ11:AQ{0})", reportRow)); // Week 4 Sales
        //            wsReportSummarySheet.GetCell(String.Format("AU{0}", reportRow + 1)).ApplyFormula(Methods.GenerateExcelFractionFormulaWithZeroNumeratorChecking("AQ", 11, reportRow, reportRow + 1, "P")); // Week 4 %

        //            wsReportSummarySheet.GetCell(String.Format("AY{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(AY11:AY{0})", reportRow)); // Week 5 Sales
        //            wsReportSummarySheet.GetCell(String.Format("BC{0}", reportRow + 1)).ApplyFormula(Methods.GenerateExcelFractionFormulaWithZeroNumeratorChecking("AY", 11, reportRow, reportRow + 1, "P")); // Week 5 %

        //            wsReportSummarySheet.GetCell(String.Format("BG{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(BG11:BG{0})", reportRow)); // Declines
        //            wsReportSummarySheet.GetCell(String.Format("BK{0}", reportRow + 1)).ApplyFormula(Methods.GenerateExcelFractionFormulaWithZeroNumeratorChecking("BG", 11, reportRow, reportRow + 1, "P")); // Decline %

        //            wsReportSummarySheet.GetCell(String.Format("BO{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(BO11:BO{0})", reportRow)); // Total Contacts
        //            wsReportSummarySheet.GetCell(String.Format("BS{0}", reportRow + 1)).ApplyFormula(Methods.GenerateExcelFractionFormulaWithZeroNumeratorChecking("BO", 11, reportRow, reportRow + 1, "P")); // % Contact To Lead

        //            wsReportSummarySheet.GetCell(String.Format("BX{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(BX11:BX{0})", reportRow)); // Contacts Remaining
        //            wsReportSummarySheet.GetCell(String.Format("CC{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(CC11:CC{0})", reportRow)); // Fall Offs

        //            break;
        //    }

        //    #endregion Add the totals & averages row
        //}

        //private void AddIndividualSalesConsultantSTLReportToWorkbook(Workbook wbResultingWorkbook, Worksheet wsTemplate, string salesAgentName, long campaignID, long fkUserID, DateTime fromDate, DateTime toDate, int reportSummaryRow)
        //{
        //    #region Get the report data from the database

        //    //DataSet dsSTLReportData = UDM.Insurance.Business.Insure.INGetSTLDataByUserCampaignAndDateRange(campaignID, fkUserID, fromDate, toDate);
        //    DataSet dsSTLReportData = UDM.Insurance.Business.Insure.INReportSTL(campaignID, fkUserID, fromDate, toDate, _stlOption);
        //    DataTable dtCampaignDetails = dsSTLReportData.Tables[0];
        //    DataTable dtUserDetails = dsSTLReportData.Tables[1];
        //    DataTable dtMainSTLReportData = dsSTLReportData.Tables[2];
        //    DataTable dtIndividualLeadData = dsSTLReportData.Tables[3];

        //    #endregion Get the report data from the database

        //    if (dtMainSTLReportData.Rows.Count == 0)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        #region Declarations

        //        int reportRow = 13;
        //        int reportTemplateRowIndex = 13;
        //        int bonusSectionFormulaFirstRowIndex = reportRow;

        //        //int reportSummaryRow = 10;
        //        int reportSummaryTemplateRowIndex = 10;
                
        //        string averageConversionFormula = String.Empty;

        //        Worksheet wsNewReportSheet;

        //        #endregion Declarations

        //        #region Add the new worksheet

        //        string newWorksheetDescription = Methods.ParseWorksheetName(wbResultingWorkbook, salesAgentName).Replace("'", "");

        //        wsNewReportSheet = wbResultingWorkbook.Worksheets.Add(newWorksheetDescription);
        //        //Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsNewReportSheet);
        //        Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsNewReportSheet, true, true, true, true, true, false, false, false, true, true, true, true, true, true, true, true, false);

        //        wsNewReportSheet.PrintOptions.ScalingType = ScalingType.FitToPages;
        //        wsNewReportSheet.PrintOptions.MaxPagesHorizontally = 1;

        //        #endregion Add the new worksheet

        //        #region Step 1: Copy a region from the template that consists of the headings and the column headings

        //        Methods.CopyExcelRegion(wsTemplate, 0, 0, 12, 76, wsNewReportSheet, 0, 0);

        //        #endregion Step 1: Copy a region from the template that consists of the headings and the column headings

        //        #region Step 2: Populate the report details and append the correct conversion target percentages underneath the week 2, 4 and 6 columns

        //        wsNewReportSheet.GetCell("A6").Value = String.Format("{0} ({1})", dtCampaignDetails.Rows[0]["CampaignName"].ToString(), dtCampaignDetails.Rows[0]["CampaignCode"].ToString());
        //        wsNewReportSheet.GetCell("A8").Value = String.Format("From {0} to {1}", fromDate.ToString("dddd, d MMMM yyyy"), toDate.ToString("dddd, d MMMM yyyy"));
        //        wsNewReportSheet.GetCell("G10").Value = String.Format("{0} ({1})", dtUserDetails.Rows[0]["FullName"].ToString(), dtUserDetails.Rows[0]["EmployeeNo"].ToString());
        //        wsNewReportSheet.GetCell("BP10").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //        wsNewReportSheet.GetCell("AN13").Value = dtMainSTLReportData.Rows[0]["Week2STLTargetPercentage"];
        //        wsNewReportSheet.GetCell("AT13").Value = dtMainSTLReportData.Rows[0]["Week4STLTargetPercentage"];
        //        wsNewReportSheet.GetCell("AZ13").Value = dtMainSTLReportData.Rows[0]["Week6STLTargetPercentage"];

        //        #region Update the summary page's conversion target percentages for weeks 2, 4 and 6 if not yet updated

        //        if (!_hasSummaryPageConversionTargetsBeenUpdated)
        //        {
        //            _hasSummaryPageConversionTargetsBeenUpdated = true;

        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell("AI10").Value = dtMainSTLReportData.Rows[0]["Week2STLTargetPercentage"];
        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell("AM10").Value = dtMainSTLReportData.Rows[0]["Week4STLTargetPercentage"];
        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell("AQ10").Value = dtMainSTLReportData.Rows[0]["Week6STLTargetPercentage"];
        //        }

        //        #endregion Update the summary page's conversion target percentages for weeks 2, 4 and 6 if not yet updated

        //        #endregion Step 2: Populate the report details and append the correct conversion target percentages underneath the week 2, 4 and 6 columns

        //        #region Step 3: Populate the main data section of the report

        //        for (int i = 0; i < dtMainSTLReportData.Rows.Count; i++)
        //        {
        //            #region Step 3.1. Copy the template formatting for the data row

        //            Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, 76, wsNewReportSheet, reportRow, 0);

        //            #endregion Step 3.1. Copy the template formatting for the data row

        //            #region Step 3.2. Add the values

        //            wsNewReportSheet.GetCell(String.Format("A{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["DateAllocated"];
        //            wsNewReportSheet.GetCell(String.Format("F{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["DayOfTheWeek"];
        //            wsNewReportSheet.GetCell(String.Format("K{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["DateToCommenceSelling"];
        //            wsNewReportSheet.GetCell(String.Format("P{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["TargetDate"];
        //            wsNewReportSheet.GetCell(String.Format("U{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Code"];
        //            wsNewReportSheet.GetCell(String.Format("AA{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["TotalLeadsInBatch"];
        //            wsNewReportSheet.GetCell(String.Format("AD{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["STLTargetSales"];
        //            wsNewReportSheet.GetCell(String.Format("AG{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["ActualSales"];
        //            wsNewReportSheet.GetCell(String.Format("AJ{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["RemainingSalesSTL"];
        //            wsNewReportSheet.GetCell(String.Format("AN{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Week2ConversionPercentage"];
        //            wsNewReportSheet.GetCell(String.Format("AQ{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Week3ConversionPercentage"];
        //            wsNewReportSheet.GetCell(String.Format("AT{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Week4ConversionPercentage"];
        //            wsNewReportSheet.GetCell(String.Format("AW{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Week5ConversionPercentage"];
        //            wsNewReportSheet.GetCell(String.Format("AZ{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Week6ConversionPercentage"];
        //            wsNewReportSheet.GetCell(String.Format("BC{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Declines"];
        //            wsNewReportSheet.GetCell(String.Format("BF{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["TotalContacts"];
        //            wsNewReportSheet.GetCell(String.Format("BJ{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["ContactsToLeadPercentage"];
        //            wsNewReportSheet.GetCell(String.Format("BN{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["ContactsRemaining"];
        //            wsNewReportSheet.GetCell(String.Format("BR{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["FallOffs"];
        //            wsNewReportSheet.GetCell(String.Format("BU{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["ExtraLeadsSTL"];

        //            ++reportRow;

        //            #endregion Step 3.2. Add the values
        //        }

        //        #region Step 3.3: Add the totals & averages row with the relevant formulae

        //        #region Copy the template formatting for the data row

        //        reportTemplateRowIndex = 14;
        //        Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 1, 76, wsNewReportSheet, reportRow, 0);

        //        #endregion Copy the template formatting for the data row

        //        #region Apply the appropriate Excel formulae

        //        // Row 1
        //        wsNewReportSheet.GetCell(String.Format("AA{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(AA14:AA{0})", reportRow));
        //        wsNewReportSheet.GetCell(String.Format("AD{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(AD14:AD{0})", reportRow));
        //        wsNewReportSheet.GetCell(String.Format("AG{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(AG14:AG{0})", reportRow));
        //        wsNewReportSheet.GetCell(String.Format("AJ{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(AJ14:AJ{0})", reportRow));
        //        //wsNewReportSheet.GetCell(String.Format("AN{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(AN14:AN{0})", reportRow));
        //        //wsNewReportSheet.GetCell(String.Format("AQ{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(AQ14:AQ{0})", reportRow));
        //        //wsNewReportSheet.GetCell(String.Format("AT{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(AT14:AT{0})", reportRow));
        //        //wsNewReportSheet.GetCell(String.Format("AW{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(AW14:AW{0})", reportRow));

        //        wsNewReportSheet.GetCell(String.Format("AN{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelAverageFormula("AN", 14, reportRow));
        //        wsNewReportSheet.GetCell(String.Format("AQ{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelAverageFormula("AQ", 14, reportRow));
        //        wsNewReportSheet.GetCell(String.Format("AT{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelAverageFormula("AT", 14, reportRow));
        //        wsNewReportSheet.GetCell(String.Format("AW{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelAverageFormula("AW", 14, reportRow));

        //        //wsNewReportSheet.GetCell(String.Format("AZ{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(AZ14:AZ{0})", reportRow));
        //        wsNewReportSheet.GetCell(String.Format("AZ{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelAverageFormula("AZ", 14, reportRow));
        //        wsNewReportSheet.GetCell(String.Format("BC{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(BC14:BC{0})", reportRow));
        //        wsNewReportSheet.GetCell(String.Format("BF{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(BF14:BF{0})", reportRow));
        //        //wsNewReportSheet.GetCell(String.Format("BJ{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(BJ14:BJ{0})", reportRow));
        //        wsNewReportSheet.GetCell(String.Format("BJ{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelAverageFormula("BJ", 14, reportRow));
        //        wsNewReportSheet.GetCell(String.Format("BN{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(BN14:BN{0})", reportRow));
        //        wsNewReportSheet.GetCell(String.Format("BR{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(BR14:BR{0})", reportRow));
        //        wsNewReportSheet.GetCell(String.Format("BU{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(BU14:BU{0})", reportRow));

        //        //averageConversionFormula = String.Format("AI{0}{1}AO{0}{1}AU{0}", reportRow + 1, ";");
        //        //averageConversionFormula = "= (" + String.Format("AI{0}", reportRow + 1) + " + " + String.Format("AO{0}", reportRow + 1) + " + " + String.Format("AU{0}", reportRow + 1) + ") / 3";
        //        averageConversionFormula = String.Format("=AG{0}/AA{0}", reportRow + 1);

        //        ++reportRow;

        //        // Row 2
        //        //wsNewReportSheet.GetCell(String.Format("AA{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(AA14:AA{0})", reportRow - 1));
        //        //wsNewReportSheet.GetCell(String.Format("AD{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(AD14:AD{0})", reportRow - 1));
        //        wsNewReportSheet.GetCell(String.Format("AG{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(AG14:AG{0})", reportRow - 1));
        //        //wsNewReportSheet.GetCell(String.Format("AJ{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(AJ14:AJ{0})", reportRow - 1));
        //        //wsNewReportSheet.GetCell(String.Format("BC{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(BC14:BC{0})", reportRow - 1));
        //        //wsNewReportSheet.GetCell(String.Format("BF{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(BF14:BF{0})", reportRow - 1));
        //        //wsNewReportSheet.GetCell(String.Format("BJ{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(BJ14:BJ{0})", reportRow - 1));
        //        //wsNewReportSheet.GetCell(String.Format("BN{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(BN14:BN{0})", reportRow - 1));
        //        //wsNewReportSheet.GetCell(String.Format("BR{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(BR14:BR{0})", reportRow - 1));
        //        //wsNewReportSheet.GetCell(String.Format("BU{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(BU14:BU{0})", reportRow - 1));

        //        #endregion Apply the appropriate Excel formulae

        //        #endregion Step 3.3: Add the totals & averages row with the relevant formulae

        //        //reportRow += 2;

        //        #endregion Step 3: Populate the main data section of the report

        //        #region Step 7: Update the summary sheet

        //        //#region Step 3.1. Copy the template formatting for the data row

        //        //Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, 76, wsNewReportSheet, reportRow, 0);

        //        //#endregion Step 3.1. Copy the template formatting for the data row

        //        #region Add appropriate cross-sheet formulae

        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("A{0}", reportSummaryRow + 1)).Value = salesAgentName;
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("P{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AA", reportRow)); // Total Leads
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("S{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AD", reportRow)); // Target Sales

        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("V{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AG", reportRow)); // Sales To Date
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("Z{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AJ", reportRow)); // RemainingSalesSTL

        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AD{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("=V{0}/P{0}", reportSummaryRow + 1)); // Average On All Batches
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AI{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AN", reportRow)); // Week2ConversionPercentage
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AM{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AT", reportRow)); // Week4ConversionPercentage
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AQ{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AZ", reportRow)); // Week6ConversionPercentage

        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AU{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "BC", reportRow)); // Declines
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AY{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("=AU{0}/P{0}", reportSummaryRow + 1)); // Decline %

        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("BC{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "BF", reportRow)); // TotalContacts
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("BG{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "BJ", reportRow)); // ContactsToLeadPercentage
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("BL{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "BN", reportRow)); // ContactsRemaining
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("BQ{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "BR", reportRow)); // FallOffs

        //        //wsNewReportSheet.GetCell(String.Format("AD{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["STLTargetSales"];
        //        //wsNewReportSheet.GetCell(String.Format("AG{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["ActualSales"];
        //        //wsNewReportSheet.GetCell(String.Format("AJ{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["RemainingSalesSTL"];
        //        //wsNewReportSheet.GetCell(String.Format("AN{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Week2ConversionPercentage"];
        //        //wsNewReportSheet.GetCell(String.Format("AT{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Week4ConversionPercentage"];
        //        //wsNewReportSheet.GetCell(String.Format("AZ{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Week6ConversionPercentage"];
        //        //wsNewReportSheet.GetCell(String.Format("BC{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Declines"];
        //        //wsNewReportSheet.GetCell(String.Format("BF{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["TotalContacts"];
        //        //wsNewReportSheet.GetCell(String.Format("BJ{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["ContactsToLeadPercentage"];
        //        //wsNewReportSheet.GetCell(String.Format("BN{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["ContactsRemaining"];
        //        //wsNewReportSheet.GetCell(String.Format("BR{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["FallOffs"];

        //        ++reportSummaryRow;

        //        #endregion Add appropriate cross-sheet formulae

        //        #endregion Step 7: Update the summary sheet

        //        #region Step 4: Populate the bonus section

        //        if (!UDM.Insurance.Business.Insure.IsUpgradeCampaign(campaignID))
        //        {
        //            #region Step 4.1. Copy a region from the template worksheet that consists of the "Bonus" worksheet cell and the column headings of the bonus section

        //            reportTemplateRowIndex = 17;
        //            reportRow += 2;
        //            Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 2, 49, wsNewReportSheet, reportRow - 1, 0);
        //            bonusSectionFormulaFirstRowIndex = reportRow;
        //            reportRow += 3;
        //            reportTemplateRowIndex = 20;

        //            #endregion Step 4.1. Copy a region from the template worksheet that consists of the "Bonus" worksheet cell and the column headings of the bonus section

        //            for (int i = 0; i < dtMainSTLReportData.Rows.Count; i++)
        //            {
        //                #region Step 4.1. Copy the template formatting for the data row

        //                Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, 49, wsNewReportSheet, reportRow - 1, 0);

        //                #endregion Step 4.1. Copy the template formatting for the data row

        //                #region Step 4.2. Add the values

        //                wsNewReportSheet.GetCell(String.Format("A{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Code"];
        //                wsNewReportSheet.GetCell(String.Format("G{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["ActualSales"];
        //                wsNewReportSheet.GetCell(String.Format("J{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["BonusPotential"];
        //                wsNewReportSheet.GetCell(String.Format("N{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["TotalLeadsInBatch"];
        //                wsNewReportSheet.GetCell(String.Format("Q{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week2Sales"];
        //                wsNewReportSheet.GetCell(String.Format("T{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week2ConversionPercentage"];
        //                wsNewReportSheet.GetCell(String.Format("X{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["QualifiesForWeek2ConversionBonusYesNoSTL"];
        //                wsNewReportSheet.GetCell(String.Format("Z{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week4Sales"];
        //                wsNewReportSheet.GetCell(String.Format("AC{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week4ConversionPercentage"];
        //                wsNewReportSheet.GetCell(String.Format("AG{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["QualifiesForWeek4ConversionBonusYesNoSTL"];
        //                wsNewReportSheet.GetCell(String.Format("AI{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week6Sales"];
        //                wsNewReportSheet.GetCell(String.Format("AL{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week6ConversionPercentage"];
        //                wsNewReportSheet.GetCell(String.Format("AP{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["QualifiesForWeek6ConversionBonusYesNoSTL"];
        //                wsNewReportSheet.GetCell(String.Format("AR{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["TotalBonusQualifiedSTL"];

        //                ++reportRow;

        //                #endregion Step 4.2. Add the values
        //            }

        //            #region Step 4.3: Add the totals & averages row with the relevant formulae

        //            reportTemplateRowIndex = 21;

        //            #region Copy the template formatting for the data row

        //            Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, 49, wsNewReportSheet, reportRow - 1, 0);

        //            #endregion Copy the template formatting for the data row

        //            #region Apply the appropriate Excel formulae

        //            wsNewReportSheet.GetCell(String.Format("G{0}", reportRow)).ApplyFormula(String.Format("=SUM(G{0}:G{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));
        //            wsNewReportSheet.GetCell(String.Format("J{0}", reportRow)).ApplyFormula(String.Format("=SUM(J{0}:J{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));
        //            wsNewReportSheet.GetCell(String.Format("N{0}", reportRow)).ApplyFormula(String.Format("=SUM(N{0}:N{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));
        //            wsNewReportSheet.GetCell(String.Format("Q{0}", reportRow)).ApplyFormula(String.Format("=SUM(Q{0}:Q{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));
        //            wsNewReportSheet.GetCell(String.Format("T{0}", reportRow)).ApplyFormula(String.Format("=AVERAGE(T{0}:T{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));

        //            wsNewReportSheet.GetCell(String.Format("Z{0}", reportRow)).ApplyFormula(String.Format("=SUM(Z{0}:Z{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));
        //            wsNewReportSheet.GetCell(String.Format("AC{0}", reportRow)).ApplyFormula(String.Format("=AVERAGE(AC{0}:AC{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));

        //            wsNewReportSheet.GetCell(String.Format("AI{0}", reportRow)).ApplyFormula(String.Format("=SUM(AI{0}:AI{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));
        //            wsNewReportSheet.GetCell(String.Format("AL{0}", reportRow)).ApplyFormula(String.Format("=AVERAGE(AL{0}:AL{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));

        //            wsNewReportSheet.GetCell(String.Format("AR{0}", reportRow)).ApplyFormula(String.Format("=SUM(AR{0}:AR{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));

        //            reportRow += 2;

        //            #endregion Apply the appropriate Excel formulae

        //            #endregion Step 4.3: Add the totals & averages row with the relevant formulae
        //        }
        //        else
        //        {
        //            reportRow += 3;
        //        }

        //        #endregion Step 4: Populate the bonus section

        //        #region Step 5: Indicate the average conversion

        //        #region Copy the template formatting for the data row

        //        reportTemplateRowIndex = 23;
        //        Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, 76, wsNewReportSheet, reportRow - 1, 0);

        //        #endregion Copy the template formatting for the data row

        //        #region Apply for formula

        //        //wsNewReportSheet.GetCell(String.Format("BL{0}", reportRow)).ApplyFormula(String.Format("=AVERAGE({0})", averageConversionFormula));
        //        wsNewReportSheet.GetCell(String.Format("BS{0}", reportRow)).ApplyFormula(averageConversionFormula);

        //        #endregion Apply for formula

        //        #endregion Step 5: Indicate the average conversion

        //        #region Step 6: Update the globally accessible data table with the current TSR's individual leads

        //        _dtIndividualLeads.Merge(dtIndividualLeadData);

        //        #endregion Step 6: Update the globally accessible data table with the current TSR's individual leads

                
        //    }
        //}

        //private void AddIndividualLeadDetailsSheetToWorkbook(Workbook wbResultingWorkbook, Worksheet wsTemplate, DataTable dataTable, DateTime fromDate, DateTime toDate)
        //{
        //    if (dataTable.Rows.Count > 0)
        //    {
        //        #region Declarations

        //        int reportRow = 3;
        //        int reportTemplateRowIndex = 3;

        //        #endregion Declarations

        //        #region Add the sheet to the workbook

        //        Worksheet wsLeadDetails = wbResultingWorkbook.Worksheets.Add("All Lead Details");
        //        //Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsLeadDetails);
        //        Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsLeadDetails, true, true, true, true, true, true, true, false, true, true, true, true, true, true, true, true, false);

        //        wsLeadDetails.PrintOptions.ScalingType = ScalingType.FitToPages;
        //        wsLeadDetails.PrintOptions.MaxPagesHorizontally = 1;

        //        #endregion Add the sheet to the workbook

        //        #region Step 1: Copy a region from the template that consists of the headings and the column headings

        //        Methods.CopyExcelRegion(wsTemplate, 0, 0, 2, 19, wsLeadDetails, 0, 0);

        //        wsLeadDetails.GetCell("A1").Value = String.Format("Details of all leads assigned between {0} and {1}", fromDate.ToString("dddd, dd MMMM yyyy"), toDate.ToString("dddd, dddd, dd MMMM yyyy"));

        //        //if (stlReportType == 2)
        //        //{
        //        //    wsLeadDetails.GetCell("C3").Value = "Week 1 Target %";
        //        //    wsLeadDetails.GetCell("D3").Value = "Week 3 Target %";
        //        //    wsLeadDetails.GetCell("E3").Value = "Week 4 Target %";
        //        //}

        //        byte firstSTLWeekNumber = 0;
        //        byte secondSTLWeekNumber = 0;
        //        byte thirdSTLWeekNumber = 0;

        //        switch (_stlOption)
        //        {
        //            case 1:
        //                firstSTLWeekNumber = 2;
        //                secondSTLWeekNumber = 4;
        //                thirdSTLWeekNumber = 6;
        //                break;

        //            case 2:
        //                firstSTLWeekNumber = 1;
        //                secondSTLWeekNumber = 3;
        //                thirdSTLWeekNumber = 4;
        //                break;

        //            case 3:
        //                firstSTLWeekNumber = 1;
        //                secondSTLWeekNumber = 2;
        //                thirdSTLWeekNumber = 5;
        //                break;
        //        }

        //        wsLeadDetails.GetCell("C3").Value = String.Format("Week {0} Target %", firstSTLWeekNumber);
        //        wsLeadDetails.GetCell("D3").Value = String.Format("Week {0} Target %", secondSTLWeekNumber);
        //        wsLeadDetails.GetCell("E3").Value = String.Format("Week {0} Target %", thirdSTLWeekNumber);

        //        #endregion Step 1: Copy a region from the template that consists of the headings and the column headings

        //        #region Step 2: Copy row formatting from the template

        //        Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, 19, wsLeadDetails, reportRow, 0);

        //        #endregion Step 2: Copy row formatting from the template

        //        #region Step 3: Populate the main data section of the report

        //        for (int i = 0; i < dataTable.Rows.Count; i++)
        //        {
        //            #region Step 3.1. Copy the template formatting for the data row

        //            Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, 19, wsLeadDetails, reportRow, 0);

        //            #endregion Step 3.1. Copy the template formatting for the data row

        //            #region Step 3.2. Add the values

        //            wsLeadDetails.GetCell(String.Format("A{0}", reportRow + 1)).Value = dataTable.Rows[i]["CampaignName"];
        //            wsLeadDetails.GetCell(String.Format("B{0}", reportRow + 1)).Value = dataTable.Rows[i]["CampaignCode"];
        //            wsLeadDetails.GetCell(String.Format("C{0}", reportRow + 1)).Value = dataTable.Rows[i]["Week2STLTargetPercentage"];
        //            wsLeadDetails.GetCell(String.Format("D{0}", reportRow + 1)).Value = dataTable.Rows[i]["Week4STLTargetPercentage"];
        //            wsLeadDetails.GetCell(String.Format("E{0}", reportRow + 1)).Value = dataTable.Rows[i]["Week6STLTargetPercentage"];
        //            wsLeadDetails.GetCell(String.Format("F{0}", reportRow + 1)).Value = dataTable.Rows[i]["ComissionPerLead"];
        //            wsLeadDetails.GetCell(String.Format("G{0}", reportRow + 1)).Value = dataTable.Rows[i]["BatchCode"];
        //            wsLeadDetails.GetCell(String.Format("H{0}", reportRow + 1)).Value = dataTable.Rows[i]["UDMBatchCode"];
        //            wsLeadDetails.GetCell(String.Format("I{0}", reportRow + 1)).Value = dataTable.Rows[i]["ReferenceNumber"];
        //            wsLeadDetails.GetCell(String.Format("J{0}", reportRow + 1)).Value = dataTable.Rows[i]["SalesConsultantName"];
        //            wsLeadDetails.GetCell(String.Format("K{0}", reportRow + 1)).Value = dataTable.Rows[i]["AllocationDate"];
        //            wsLeadDetails.GetCell(String.Format("L{0}", reportRow + 1)).Value = dataTable.Rows[i]["SalesStartDate"];
        //            wsLeadDetails.GetCell(String.Format("M{0}", reportRow + 1)).Value = dataTable.Rows[i]["LeadStatusDescription"];
        //            wsLeadDetails.GetCell(String.Format("N{0}", reportRow + 1)).Value = dataTable.Rows[i]["LeadStatusClassification"];
        //            wsLeadDetails.GetCell(String.Format("O{0}", reportRow + 1)).Value = dataTable.Rows[i]["DateOfSale"];
        //            wsLeadDetails.GetCell(String.Format("P{0}", reportRow + 1)).Value = dataTable.Rows[i]["SaleDayOfTheWeek"];
        //            wsLeadDetails.GetCell(String.Format("Q{0}", reportRow + 1)).Value = dataTable.Rows[i]["SaleCallRef"];
        //            wsLeadDetails.GetCell(String.Format("R{0}", reportRow + 1)).Value = dataTable.Rows[i]["WeekNumber"];

        //            ++reportRow;

        //            #endregion Step 3.2. Add the values
        //        }

        //        #endregion Step 3: Populate the main data section of the report
        //    }
        //}

        //#region Additional Methods - To accomodate for the new STL Structure effective between 2015-10-19 and 2015-11-15

        //private void AddIndividualSalesConsultantSTLReportToWorkbook20151019(Workbook wbResultingWorkbook, Worksheet wsTemplate, string salesAgentName, long campaignID, long fkUserID, DateTime fromDate, DateTime toDate, int reportSummaryRow)
        //{
        //    #region Get the report data from the database

        //    DataSet dsSTLReportData = UDM.Insurance.Business.Insure.INReportSTL(campaignID, fkUserID, fromDate, toDate, _stlOption);
        //    DataTable dtCampaignDetails = dsSTLReportData.Tables[0];
        //    DataTable dtUserDetails = dsSTLReportData.Tables[1];
        //    DataTable dtMainSTLReportData = dsSTLReportData.Tables[2];
        //    DataTable dtIndividualLeadData = dsSTLReportData.Tables[3];

        //    #endregion Get the report data from the database

        //    if (dtMainSTLReportData.Rows.Count == 0)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        #region Declarations

        //        int reportRow = 13;
        //        int reportTemplateColumnSpan = 73;
        //        int reportTemplateRowIndex = 13;
        //        int bonusSectionFormulaFirstRowIndex = reportRow;

        //        //int reportSummaryRow = 10;
        //        int reportSummaryTemplateRowIndex = 10;

        //        string averageConversionFormula = String.Empty;

        //        Worksheet wsNewReportSheet;

        //        #endregion Declarations

        //        #region Add the new worksheet

        //        string newWorksheetDescription = Methods.ParseWorksheetName(wbResultingWorkbook, salesAgentName).Replace("'", "");

        //        wsNewReportSheet = wbResultingWorkbook.Worksheets.Add(newWorksheetDescription);
        //        Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsNewReportSheet, true, true, true, true, true, false, false, false, true, true, true, true, true, true, true, true, false);

        //        wsNewReportSheet.PrintOptions.ScalingType = ScalingType.FitToPages;
        //        wsNewReportSheet.PrintOptions.MaxPagesHorizontally = 1;

        //        #endregion Add the new worksheet

        //        #region Step 1: Copy a region from the template that consists of the headings and the column headings

        //        Methods.CopyExcelRegion(wsTemplate, 0, 0, 12, reportTemplateColumnSpan, wsNewReportSheet, 0, 0);

        //        #endregion Step 1: Copy a region from the template that consists of the headings and the column headings

        //        #region Step 2: Populate the report details and append the correct conversion target percentages underneath the week 2, 4 and 6 columns

        //        wsNewReportSheet.GetCell("A6").Value = String.Format("{0} ({1})", dtCampaignDetails.Rows[0]["CampaignName"].ToString(), dtCampaignDetails.Rows[0]["CampaignCode"].ToString());
        //        wsNewReportSheet.GetCell("A8").Value = String.Format("From {0} to {1}", fromDate.ToString("dddd, d MMMM yyyy"), toDate.ToString("dddd, d MMMM yyyy"));
        //        wsNewReportSheet.GetCell("G10").Value = String.Format("{0} ({1})", dtUserDetails.Rows[0]["FullName"].ToString(), dtUserDetails.Rows[0]["EmployeeNo"].ToString());
        //        wsNewReportSheet.GetCell("BL10").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //        wsNewReportSheet.GetCell("AN13").Value = dtMainSTLReportData.Rows[0]["Week2STLTargetPercentage"];
        //        wsNewReportSheet.GetCell("AT13").Value = dtMainSTLReportData.Rows[0]["Week4STLTargetPercentage"];
        //        wsNewReportSheet.GetCell("AW13").Value = dtMainSTLReportData.Rows[0]["Week6STLTargetPercentage"];

        //        #region Update the summary page's conversion target percentages for weeks 2, 4 and 6 if not yet updated

        //        if (!_hasSummaryPageConversionTargetsBeenUpdated)
        //        {
        //            _hasSummaryPageConversionTargetsBeenUpdated = true;

        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell("AI10").Value = dtMainSTLReportData.Rows[0]["Week2STLTargetPercentage"];
        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell("AM10").Value = dtMainSTLReportData.Rows[0]["Week4STLTargetPercentage"];
        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell("AQ10").Value = dtMainSTLReportData.Rows[0]["Week6STLTargetPercentage"];
        //        }

        //        #endregion Update the summary page's conversion target percentages for weeks 2, 4 and 6 if not yet updated

        //        #endregion Step 2: Populate the report details and append the correct conversion target percentages underneath the week 2, 4 and 6 columns

        //        #region Step 3: Populate the main data section of the report

        //        for (int i = 0; i < dtMainSTLReportData.Rows.Count; i++)
        //        {
        //            #region Step 3.1. Copy the template formatting for the data row

        //            Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, reportTemplateColumnSpan, wsNewReportSheet, reportRow, 0);

        //            #endregion Step 3.1. Copy the template formatting for the data row

        //            #region Step 3.2. Add the values

        //            wsNewReportSheet.GetCell(String.Format("A{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["DateAllocated"];
        //            wsNewReportSheet.GetCell(String.Format("F{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["DayOfTheWeek"];
        //            wsNewReportSheet.GetCell(String.Format("K{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["DateToCommenceSelling"];
        //            wsNewReportSheet.GetCell(String.Format("P{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["TargetDate"];
        //            wsNewReportSheet.GetCell(String.Format("U{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Code"];
        //            wsNewReportSheet.GetCell(String.Format("AA{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["TotalLeadsInBatch"];
        //            wsNewReportSheet.GetCell(String.Format("AD{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["STLTargetSales"];
        //            wsNewReportSheet.GetCell(String.Format("AG{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["ActualSales"];
        //            wsNewReportSheet.GetCell(String.Format("AJ{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["RemainingSalesSTL"];
        //            wsNewReportSheet.GetCell(String.Format("AN{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Week1ConversionPercentage"];
        //            wsNewReportSheet.GetCell(String.Format("AQ{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Week2ConversionPercentage"];
        //            wsNewReportSheet.GetCell(String.Format("AT{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Week3ConversionPercentage"];
        //            wsNewReportSheet.GetCell(String.Format("AW{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Week4ConversionPercentage"];
        //            wsNewReportSheet.GetCell(String.Format("AZ{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Declines"];
        //            wsNewReportSheet.GetCell(String.Format("BC{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["TotalContacts"];
        //            wsNewReportSheet.GetCell(String.Format("BG{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["ContactsToLeadPercentage"];
        //            wsNewReportSheet.GetCell(String.Format("BK{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["ContactsRemaining"];
        //            wsNewReportSheet.GetCell(String.Format("BO{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["FallOffs"];
        //            wsNewReportSheet.GetCell(String.Format("BR{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["ExtraLeadsSTL"];

        //            ++reportRow;

        //            #endregion Step 3.2. Add the values
        //        }

        //        #region Step 3.3: Add the totals & averages row with the relevant formulae

        //        #region Copy the template formatting for the data row

        //        reportTemplateRowIndex = 14;
        //        Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 1, reportTemplateColumnSpan, wsNewReportSheet, reportRow, 0);

        //        #endregion Copy the template formatting for the data row

        //        #region Apply the appropriate Excel formulae

        //        // Row 1
        //        wsNewReportSheet.GetCell(String.Format("AA{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(AA14:AA{0})", reportRow));
        //        wsNewReportSheet.GetCell(String.Format("AD{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(AD14:AD{0})", reportRow));
        //        wsNewReportSheet.GetCell(String.Format("AG{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(AG14:AG{0})", reportRow));
        //        wsNewReportSheet.GetCell(String.Format("AJ{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(AJ14:AJ{0})", reportRow));

        //        wsNewReportSheet.GetCell(String.Format("AN{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelAverageFormula("AN", 14, reportRow));
        //        wsNewReportSheet.GetCell(String.Format("AQ{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelAverageFormula("AQ", 14, reportRow));
        //        wsNewReportSheet.GetCell(String.Format("AT{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelAverageFormula("AT", 14, reportRow));
        //        wsNewReportSheet.GetCell(String.Format("AW{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelAverageFormula("AW", 14, reportRow));

        //        wsNewReportSheet.GetCell(String.Format("AZ{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(AZ14:AZ{0})", reportRow));
        //        wsNewReportSheet.GetCell(String.Format("BC{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(BC14:BC{0})", reportRow));

        //        wsNewReportSheet.GetCell(String.Format("BG{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelAverageFormula("BG", 14, reportRow));
        //        wsNewReportSheet.GetCell(String.Format("BK{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(BK14:BK{0})", reportRow));
        //        wsNewReportSheet.GetCell(String.Format("BO{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(BO14:BO{0})", reportRow));
        //        wsNewReportSheet.GetCell(String.Format("BR{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM(BR14:BR{0})", reportRow));

        //        averageConversionFormula = String.Format("=AG{0}/AA{0}", reportRow + 1);

        //        ++reportRow;

        //        // Row 2
        //        wsNewReportSheet.GetCell(String.Format("AG{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(AG14:AG{0})", reportRow - 1));

        //        #endregion Apply the appropriate Excel formulae

        //        #endregion Step 3.3: Add the totals & averages row with the relevant formulae

        //        #endregion Step 3: Populate the main data section of the report

        //        #region Step 7: Update the summary sheet

        //        //#region Step 3.1. Copy the template formatting for the data row

        //        //Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, 76, wsNewReportSheet, reportRow, 0);

        //        //#endregion Step 3.1. Copy the template formatting for the data row

        //        #region Add appropriate cross-sheet formulae

        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("A{0}", reportSummaryRow + 1)).Value = salesAgentName;
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("P{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AA", reportRow)); // Total Leads
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("S{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AD", reportRow)); // Target Sales

        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("V{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AG", reportRow)); // Sales To Date
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("Z{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AJ", reportRow)); // RemainingSalesSTL

        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AD{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("=V{0}/P{0}", reportSummaryRow + 1)); // Average On All Batches
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AI{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AN", reportRow)); // Week2ConversionPercentage
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AM{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AQ", reportRow)); // Week4ConversionPercentage
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AM{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AT", reportRow)); // Week4ConversionPercentage
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AQ{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AW", reportRow)); // Week6ConversionPercentage

        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AU{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AZ", reportRow)); // Declines
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AY{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("=AU{0}/P{0}", reportSummaryRow + 1)); // Decline %

        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("BC{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "BC", reportRow)); // TotalContacts
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("BG{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "BG", reportRow)); // ContactsToLeadPercentage
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("BL{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "BK", reportRow)); // ContactsRemaining
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("BQ{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "BO", reportRow)); // FallOffs

        //        ++reportSummaryRow;

        //        #endregion Add appropriate cross-sheet formulae

        //        #endregion Step 7: Update the summary sheet

        //        #region Step 4: Populate the bonus section

        //        if (!UDM.Insurance.Business.Insure.IsUpgradeCampaign(campaignID))
        //        {
        //            #region Step 4.1. Copy a region from the template worksheet that consists of the "Bonus" worksheet cell and the column headings of the bonus section

        //            reportTemplateColumnSpan = 49;
        //            reportTemplateRowIndex = 17;
        //            reportRow += 2;
        //            Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 2, reportTemplateColumnSpan, wsNewReportSheet, reportRow - 1, 0);
        //            bonusSectionFormulaFirstRowIndex = reportRow;
        //            reportRow += 3;
        //            reportTemplateRowIndex = 20;

        //            #endregion Step 4.1. Copy a region from the template worksheet that consists of the "Bonus" worksheet cell and the column headings of the bonus section

        //            for (int i = 0; i < dtMainSTLReportData.Rows.Count; i++)
        //            {
        //                #region Step 4.1. Copy the template formatting for the data row

        //                Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, 49, wsNewReportSheet, reportRow - 1, 0);

        //                #endregion Step 4.1. Copy the template formatting for the data row

        //                #region Step 4.2. Add the values

        //                wsNewReportSheet.GetCell(String.Format("A{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Code"];
        //                wsNewReportSheet.GetCell(String.Format("G{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["ActualSales"];
        //                wsNewReportSheet.GetCell(String.Format("J{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["BonusPotential"];
        //                wsNewReportSheet.GetCell(String.Format("N{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["TotalLeadsInBatch"];
        //                wsNewReportSheet.GetCell(String.Format("Q{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week1Sales"];
        //                wsNewReportSheet.GetCell(String.Format("T{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week1ConversionPercentage"];
        //                wsNewReportSheet.GetCell(String.Format("X{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["QualifiesForWeek1ConversionBonusYesNoSTL"];
        //                wsNewReportSheet.GetCell(String.Format("Z{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week3Sales"];
        //                wsNewReportSheet.GetCell(String.Format("AC{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week3ConversionPercentage"];
        //                wsNewReportSheet.GetCell(String.Format("AG{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["QualifiesForWeek3ConversionBonusYesNoSTL"];
        //                wsNewReportSheet.GetCell(String.Format("AI{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week4Sales"];
        //                wsNewReportSheet.GetCell(String.Format("AL{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week4ConversionPercentage"];
        //                wsNewReportSheet.GetCell(String.Format("AP{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["QualifiesForWeek4ConversionBonusYesNoSTL"];
        //                wsNewReportSheet.GetCell(String.Format("AR{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["TotalBonusQualifiedSTL"];

        //                ++reportRow;

        //                #endregion Step 4.2. Add the values
        //            }

        //            #region Step 4.3: Add the totals & averages row with the relevant formulae

        //            reportTemplateRowIndex = 21;

        //            #region Copy the template formatting for the data row

        //            Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, reportTemplateColumnSpan, wsNewReportSheet, reportRow - 1, 0);

        //            #endregion Copy the template formatting for the data row

        //            #region Apply the appropriate Excel formulae

        //            wsNewReportSheet.GetCell(String.Format("G{0}", reportRow)).ApplyFormula(String.Format("=SUM(G{0}:G{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));
        //            wsNewReportSheet.GetCell(String.Format("J{0}", reportRow)).ApplyFormula(String.Format("=SUM(J{0}:J{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));
        //            wsNewReportSheet.GetCell(String.Format("N{0}", reportRow)).ApplyFormula(String.Format("=SUM(N{0}:N{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));
        //            wsNewReportSheet.GetCell(String.Format("Q{0}", reportRow)).ApplyFormula(String.Format("=SUM(Q{0}:Q{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));
        //            wsNewReportSheet.GetCell(String.Format("T{0}", reportRow)).ApplyFormula(String.Format("=AVERAGE(T{0}:T{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));

        //            wsNewReportSheet.GetCell(String.Format("Z{0}", reportRow)).ApplyFormula(String.Format("=SUM(Z{0}:Z{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));
        //            wsNewReportSheet.GetCell(String.Format("AC{0}", reportRow)).ApplyFormula(String.Format("=AVERAGE(AC{0}:AC{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));

        //            wsNewReportSheet.GetCell(String.Format("AI{0}", reportRow)).ApplyFormula(String.Format("=SUM(AI{0}:AI{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));
        //            wsNewReportSheet.GetCell(String.Format("AL{0}", reportRow)).ApplyFormula(String.Format("=AVERAGE(AL{0}:AL{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));

        //            wsNewReportSheet.GetCell(String.Format("AR{0}", reportRow)).ApplyFormula(String.Format("=SUM(AR{0}:AR{1})", bonusSectionFormulaFirstRowIndex, reportRow - 1));

        //            reportRow += 2;

        //            #endregion Apply the appropriate Excel formulae

        //            #endregion Step 4.3: Add the totals & averages row with the relevant formulae

        //            reportTemplateColumnSpan = 70;
        //        }
        //        else
        //        {
        //            reportRow += 3;
        //        }

        //        #endregion Step 4: Populate the bonus section

        //        #region Step 5: Indicate the average conversion

        //        #region Copy the template formatting for the data row

        //        reportTemplateRowIndex = 23;
        //        Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, reportTemplateColumnSpan, wsNewReportSheet, reportRow - 1, 0);

        //        #endregion Copy the template formatting for the data row

        //        #region Apply for formula

        //        wsNewReportSheet.GetCell(String.Format("BK{0}", reportRow)).ApplyFormula(averageConversionFormula);

        //        #endregion Apply for formula

        //        #endregion Step 5: Indicate the average conversion

        //        #region Step 6: Update the globally accessible data table with the current TSR's individual leads

        //        _dtIndividualLeads.Merge(dtIndividualLeadData);

        //        #endregion Step 6: Update the globally accessible data table with the current TSR's individual leads

        //    }
        //}

        //#endregion Additional Methods - To accomodate for the new STL Structure effective between 2015-10-19 and 2015-11-15

        //#region Additional Methods - To accomodate for the new STL Structure effective on 2015-11-16

        //private void AddIndividualSalesConsultantSTLReportToWorkbook20151116(Workbook wbResultingWorkbook, Worksheet wsTemplate, string salesAgentName, long campaignID, long fkUserID, DateTime fromDate, DateTime toDate, int reportSummaryRow)
        //{
        //    #region Get the report data from the database

        //    //DataSet dsSTLReportData = UDM.Insurance.Business.Insure.INGetSTLDataByUserCampaignAndDateRange20151116(campaignID, fkUserID, fromDate, toDate);
        //    DataSet dsSTLReportData = UDM.Insurance.Business.Insure.INReportSTL(campaignID, fkUserID, fromDate, toDate, _stlOption);
        //    DataTable dtCampaignDetails = dsSTLReportData.Tables[0];
        //    DataTable dtUserDetails = dsSTLReportData.Tables[1];
        //    DataTable dtMainSTLReportData = dsSTLReportData.Tables[2];
        //    DataTable dtIndividualLeadData = dsSTLReportData.Tables[3];
        //    DataTable dtWeeklyBonusPartitions = dsSTLReportData.Tables[4];

        //    #endregion Get the report data from the database

        //    if (dtMainSTLReportData.Rows.Count == 0)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        #region Declarations

        //        int reportRow = 13;
        //        int reportTemplateRowIndex = 13;
        //        int mainSectionTotalsRowIndex = 0;
        //        int bonusSectionFormulaFirstRowIndex = reportRow;

        //        //int reportSummaryRow = 10;
        //        int reportSummaryTemplateRowIndex = 10;

        //        string averageConversionFormula = String.Empty;

        //        Worksheet wsNewReportSheet;

        //        #endregion Declarations

        //        #region Add the new worksheet

        //        string newWorksheetDescription = Methods.ParseWorksheetName(wbResultingWorkbook, salesAgentName).Replace("'", "");

        //        wsNewReportSheet = wbResultingWorkbook.Worksheets.Add(newWorksheetDescription);
        //        Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsNewReportSheet, true, true, true, true, true, false, false, false, true, true, true, true, true, true, true, true, false);

        //        wsNewReportSheet.PrintOptions.ScalingType = ScalingType.FitToPages;
        //        wsNewReportSheet.PrintOptions.MaxPagesHorizontally = 1;

        //        #endregion Add the new worksheet

        //        #region Step 1: Copy a region from the template that consists of the headings and the column headings

        //        Methods.CopyExcelRegion(wsTemplate, 0, 0, 12, 76, wsNewReportSheet, 0, 0);

        //        #endregion Step 1: Copy a region from the template that consists of the headings and the column headings

        //        #region Step 2: Populate the report details and append the correct conversion target percentages underneath the week 2, 4 and 6 columns

        //        wsNewReportSheet.GetCell("A6").Value = String.Format("{0} ({1})", dtCampaignDetails.Rows[0]["CampaignName"].ToString(), dtCampaignDetails.Rows[0]["CampaignCode"].ToString());
        //        wsNewReportSheet.GetCell("A8").Value = String.Format("From {0} to {1}", fromDate.ToString("dddd, d MMMM yyyy"), toDate.ToString("dddd, d MMMM yyyy"));
        //        wsNewReportSheet.GetCell("G10").Value = String.Format("{0} ({1})", dtUserDetails.Rows[0]["FullName"].ToString(), dtUserDetails.Rows[0]["EmployeeNo"].ToString());
        //        wsNewReportSheet.GetCell("BP10").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //        wsNewReportSheet.GetCell("AN13").Value = dtMainSTLReportData.Rows[0]["Week2STLTargetPercentage"];
        //        wsNewReportSheet.GetCell("AQ13").Value = dtMainSTLReportData.Rows[0]["Week4STLTargetPercentage"];
        //        wsNewReportSheet.GetCell("AZ13").Value = dtMainSTLReportData.Rows[0]["Week6STLTargetPercentage"];

        //        #region Update the summary page's conversion target percentages for weeks 2, 4 and 6 if not yet updated

        //        if (!_hasSummaryPageConversionTargetsBeenUpdated)
        //        {
        //            _hasSummaryPageConversionTargetsBeenUpdated = true;

        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell("AM10").Value = dtMainSTLReportData.Rows[0]["Week2STLTargetPercentage"];
        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell("AU10").Value = dtMainSTLReportData.Rows[0]["Week4STLTargetPercentage"];
        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell("BC10").Value = dtMainSTLReportData.Rows[0]["Week6STLTargetPercentage"];
        //        }

        //        #endregion Update the summary page's conversion target percentages for weeks 2, 4 and 6 if not yet updated

        //        #endregion Step 2: Populate the report details and append the correct conversion target percentages underneath the week 2, 4 and 6 columns

        //        #region Step 3: Populate the main data section of the report

        //        for (int i = 0; i < dtMainSTLReportData.Rows.Count; i++)
        //        {
        //            #region Step 3.1. Copy the template formatting for the data row

        //            Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, 76, wsNewReportSheet, reportRow, 0);

        //            #endregion Step 3.1. Copy the template formatting for the data row

        //            #region Step 3.2. Add the values

        //            wsNewReportSheet.GetCell(String.Format("A{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["DateAllocated"];
        //            wsNewReportSheet.GetCell(String.Format("F{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["DayOfTheWeek"];
        //            wsNewReportSheet.GetCell(String.Format("K{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["DateToCommenceSelling"];
        //            wsNewReportSheet.GetCell(String.Format("P{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["TargetDate"];
        //            wsNewReportSheet.GetCell(String.Format("U{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Code"];
        //            wsNewReportSheet.GetCell(String.Format("AA{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["TotalLeadsInBatch"];
        //            wsNewReportSheet.GetCell(String.Format("AD{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["STLTargetSales"];
        //            wsNewReportSheet.GetCell(String.Format("AG{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["ActualSales"];
        //            wsNewReportSheet.GetCell(String.Format("AJ{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["RemainingSalesSTL"];
        //            wsNewReportSheet.GetCell(String.Format("AN{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Week1ConversionPercentage"];
        //            wsNewReportSheet.GetCell(String.Format("AQ{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Week2ConversionPercentage"];
        //            wsNewReportSheet.GetCell(String.Format("AT{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Week3ConversionPercentage"];
        //            wsNewReportSheet.GetCell(String.Format("AW{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Week4ConversionPercentage"];
        //            wsNewReportSheet.GetCell(String.Format("AZ{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Week5ConversionPercentage"];
        //            wsNewReportSheet.GetCell(String.Format("BC{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["Declines"];
        //            wsNewReportSheet.GetCell(String.Format("BF{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["TotalContacts"];
        //            wsNewReportSheet.GetCell(String.Format("BJ{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["ContactsToLeadPercentage"];
        //            wsNewReportSheet.GetCell(String.Format("BN{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["ContactsRemaining"];
        //            wsNewReportSheet.GetCell(String.Format("BR{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["FallOffs"];
        //            wsNewReportSheet.GetCell(String.Format("BU{0}", reportRow + 1)).Value = dtMainSTLReportData.Rows[i]["ExtraLeadsSTL"];

        //            ++reportRow;

        //            #endregion Step 3.2. Add the values
        //        }

        //        #region Step 3.3: Add the totals & averages row with the relevant formulae

        //        #region Copy the template formatting for the data row

        //        reportTemplateRowIndex = 14;
        //        Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 1, 76, wsNewReportSheet, reportRow, 0);

        //        #endregion Copy the template formatting for the data row

        //        #region Apply the appropriate Excel formulae

        //        // Row 1

        //        wsNewReportSheet.GetCell(String.Format("AA{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("AA", 14, reportRow, "SUM"));
        //        wsNewReportSheet.GetCell(String.Format("AD{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("AD", 14, reportRow, "SUM"));
        //        wsNewReportSheet.GetCell(String.Format("AG{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("AG", 14, reportRow, "SUM"));
        //        wsNewReportSheet.GetCell(String.Format("AJ{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("AJ", 14, reportRow, "SUM"));

        //        //wsNewReportSheet.GetCell(String.Format("AN{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelAverageFormula("AN", 14, reportRow));
        //        //wsNewReportSheet.GetCell(String.Format("AQ{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelAverageFormula("AQ", 14, reportRow));
        //        wsNewReportSheet.GetCell(String.Format("AT{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelAverageFormula("AT", 14, reportRow));
        //        wsNewReportSheet.GetCell(String.Format("AW{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelAverageFormula("AW", 14, reportRow));
        //        //wsNewReportSheet.GetCell(String.Format("AZ{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelAverageFormula("AZ", 14, reportRow));

        //        wsNewReportSheet.GetCell(String.Format("BC{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("BC", 14, reportRow, "SUM"));
        //        wsNewReportSheet.GetCell(String.Format("BF{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("BF", 14, reportRow, "SUM"));

        //        wsNewReportSheet.GetCell(String.Format("BJ{0}", reportRow + 1)).ApplyFormula(Methods.GenerateExcelFractionFormulaWithZeroNumeratorChecking("BF", 14, reportRow, reportRow + 1, "AA"));

        //        wsNewReportSheet.GetCell(String.Format("BN{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("BN", 14, reportRow, "SUM"));
        //        wsNewReportSheet.GetCell(String.Format("BR{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("BR", 14, reportRow, "SUM"));
        //        wsNewReportSheet.GetCell(String.Format("BU{0}", reportRow + 1)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("BU", 14, reportRow, "SUM"));

        //        averageConversionFormula = String.Format("=AG{0}/AA{0}", reportRow + 1);

        //        mainSectionTotalsRowIndex = reportRow;
        //        ++reportRow;

        //        // Row 2
        //        wsNewReportSheet.GetCell(String.Format("AG{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(AG14:AG{0})", reportRow - 1));

        //        #endregion Apply the appropriate Excel formulae

        //        #endregion Step 3.3: Add the totals & averages row with the relevant formulae

        //        //reportRow += 2;

        //        #endregion Step 3: Populate the main data section of the report

        //        #region Step 7: Update the summary sheet

        //        #region Add appropriate cross-sheet formulae

        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("A{0}", reportSummaryRow + 1)).Value = salesAgentName;
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("P{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AA", reportRow)); // Total Leads
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("S{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AD", reportRow)); // Target Sales

        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("V{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AG", reportRow)); // Sales To Date
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("Z{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AJ", reportRow)); // RemainingSalesSTL

        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AD{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("=V{0}/P{0}", reportSummaryRow + 1)); // Average On All Batches
                
        //        //wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AM{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AN", reportRow)); // Week1ConversionPercentage
        //        //wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AU{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AQ", reportRow)); // Week2ConversionPercentage
        //        //wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("BC{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AZ", reportRow)); // Week5ConversionPercentage

        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("BG{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "BC", reportRow)); // Declines
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("BK{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("=BG{0}/P{0}", reportSummaryRow + 1)); // Decline %

        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("BO{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "BF", reportRow)); // TotalContacts
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("BS{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "BJ", reportRow)); // ContactsToLeadPercentage
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("BX{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "BN", reportRow)); // ContactsRemaining
        //        wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("CC{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "BR", reportRow)); // FallOffs

        //        //++reportSummaryRow; // Not YET!

        //        #endregion Add appropriate cross-sheet formulae

        //        #endregion Step 7: Update the summary sheet

        //        #region Step 4: Populate the bonus section

        //        int? totalSalesWeek1 = null;
        //        int? totalSalesWeek4 = null;
        //        int? totalSalesWeek5 = null;

        //        if (!UDM.Insurance.Business.Insure.IsUpgradeCampaign(campaignID))
        //        {
        //            #region Step 4.1. Copy a region from the template worksheet that consists of the "Bonus" worksheet cell and the column headings of the bonus section

        //            reportTemplateRowIndex = 17;
        //            reportRow += 2;
        //            Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 2, 59, wsNewReportSheet, reportRow - 1, 0);
                    
        //            reportRow += 3;
        //            bonusSectionFormulaFirstRowIndex = reportRow;
        //            reportTemplateRowIndex = 20;

        //            #endregion Step 4.1. Copy a region from the template worksheet that consists of the "Bonus" worksheet cell and the column headings of the bonus section

        //            #region Update the column headings of AB, AO and BB

        //            wsNewReportSheet.GetCell(String.Format("AB{0}", reportRow - 2)).Value = dtWeeklyBonusPartitions.Rows[0]["FirstWeekBonusSplitColumnHeading"];
        //            wsNewReportSheet.GetCell(String.Format("AO{0}", reportRow - 2)).Value = dtWeeklyBonusPartitions.Rows[0]["SecondWeekBonusSplitColumnHeading"];
        //            wsNewReportSheet.GetCell(String.Format("BB{0}", reportRow - 2)).Value = dtWeeklyBonusPartitions.Rows[0]["ThirdWeekBonusSplitColumnHeading"];

        //            #endregion Update the column headings of AB, AO and BB

        //            for (int i = 0; i < dtMainSTLReportData.Rows.Count; i++)
        //            {
        //                #region Step 4.1.1. Copy the template formatting for the data row

        //                Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, 59, wsNewReportSheet, reportRow - 1, 0);

        //                #endregion Step 4.1.1. Copy the template formatting for the data row

        //                #region Step 4.1.2. Add the values

        //                wsNewReportSheet.GetCell(String.Format("A{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Code"];
        //                wsNewReportSheet.GetCell(String.Format("G{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["ActualSales"];
        //                wsNewReportSheet.GetCell(String.Format("J{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["BonusPotential"];
        //                wsNewReportSheet.GetCell(String.Format("N{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["TotalLeadsInBatch"];

        //                wsNewReportSheet.GetCell(String.Format("Q{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week1BonusPotential"];
        //                wsNewReportSheet.GetCell(String.Format("Q{0}", reportRow)).CellFormat.FormatString = wsNewReportSheet.GetCell(String.Format("J{0}", reportRow)).CellFormat.FormatString;
        //                wsNewReportSheet.GetCell(String.Format("U{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week1Sales"];
        //                wsNewReportSheet.GetCell(String.Format("X{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week1ConversionPercentage"];
        //                wsNewReportSheet.GetCell(String.Format("AB{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["QualifiesForWeek1ConversionBonusYesNoSTL"];

        //                wsNewReportSheet.GetCell(String.Format("AD{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week2BonusPotential"];
        //                wsNewReportSheet.GetCell(String.Format("AD{0}", reportRow)).CellFormat.FormatString = wsNewReportSheet.GetCell(String.Format("J{0}", reportRow)).CellFormat.FormatString; //"\"R\"#,##0.00";
        //                wsNewReportSheet.GetCell(String.Format("AH{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week2Sales"];
        //                wsNewReportSheet.GetCell(String.Format("AK{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week2ConversionPercentage"];
        //                wsNewReportSheet.GetCell(String.Format("AO{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["QualifiesForWeek2ConversionBonusYesNoSTL"];

        //                wsNewReportSheet.GetCell(String.Format("AQ{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week5BonusPotential"];
        //                wsNewReportSheet.GetCell(String.Format("AQ{0}", reportRow)).CellFormat.FormatString = wsNewReportSheet.GetCell(String.Format("J{0}", reportRow)).CellFormat.FormatString; // "\"R\"#,##0.00";
        //                wsNewReportSheet.GetCell(String.Format("AU{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week5Sales"];
        //                wsNewReportSheet.GetCell(String.Format("AX{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["Week5ConversionPercentage"];
        //                wsNewReportSheet.GetCell(String.Format("BB{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["QualifiesForWeek5ConversionBonusYesNoSTL"];

        //                wsNewReportSheet.GetCell(String.Format("BD{0}", reportRow)).Value = dtMainSTLReportData.Rows[i]["TotalBonusQualifiedSTL"];

        //                ++reportRow;

        //                #endregion Step 4.1.2. Add the values
        //            }

        //            #region Step 4.3: Add the totals & averages row with the relevant formulae

        //            #region Copy the template formatting for the data row

        //            reportTemplateRowIndex = 21;
        //            Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, 59, wsNewReportSheet, reportRow - 1, 0);

        //            #endregion Copy the template formatting for the data row

        //            #region Apply the appropriate Excel formulae

        //            wsNewReportSheet.GetCell(String.Format("G{0}", reportRow)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("G", bonusSectionFormulaFirstRowIndex, reportRow - 1, "SUM"));
        //            wsNewReportSheet.GetCell(String.Format("J{0}", reportRow)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("J", bonusSectionFormulaFirstRowIndex, reportRow - 1, "SUM"));
        //            wsNewReportSheet.GetCell(String.Format("N{0}", reportRow)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("N", bonusSectionFormulaFirstRowIndex, reportRow - 1, "SUM"));

        //            wsNewReportSheet.GetCell(String.Format("Q{0}", reportRow)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("Q", bonusSectionFormulaFirstRowIndex, reportRow - 1, "SUM")); // Week 1 Bonus
        //            wsNewReportSheet.GetCell(String.Format("Q{0}", reportRow)).CellFormat.FormatString = wsNewReportSheet.GetCell(String.Format("J{0}", reportRow)).CellFormat.FormatString;
        //            wsNewReportSheet.GetCell(String.Format("U{0}", reportRow)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("U", bonusSectionFormulaFirstRowIndex, reportRow - 1, "SUM")); // Week 1 Sales
        //            wsNewReportSheet.GetCell(String.Format("X{0}", reportRow)).ApplyFormula(Methods.GenerateExcelFractionFormulaWithZeroNumeratorChecking("U", bonusSectionFormulaFirstRowIndex, reportRow - 1, reportRow, "N")); //Week 1 %

        //            wsNewReportSheet.GetCell(String.Format("AD{0}", reportRow)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("AD", bonusSectionFormulaFirstRowIndex, reportRow - 1, "SUM")); // Week 2 Bonus
        //            wsNewReportSheet.GetCell(String.Format("AD{0}", reportRow)).CellFormat.FormatString = wsNewReportSheet.GetCell(String.Format("J{0}", reportRow)).CellFormat.FormatString;
        //            wsNewReportSheet.GetCell(String.Format("AH{0}", reportRow)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("AH", bonusSectionFormulaFirstRowIndex, reportRow - 1, "SUM")); // Week 2 Sales
        //            wsNewReportSheet.GetCell(String.Format("AK{0}", reportRow)).ApplyFormula(Methods.GenerateExcelFractionFormulaWithZeroNumeratorChecking("AH", bonusSectionFormulaFirstRowIndex, reportRow - 1, reportRow, "N")); // Week 2 %

        //            wsNewReportSheet.GetCell(String.Format("AQ{0}", reportRow)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("AQ", bonusSectionFormulaFirstRowIndex, reportRow - 1, "SUM")); // Week 5 Bonus
        //            wsNewReportSheet.GetCell(String.Format("AQ{0}", reportRow)).CellFormat.FormatString = wsNewReportSheet.GetCell(String.Format("J{0}", reportRow)).CellFormat.FormatString;
        //            wsNewReportSheet.GetCell(String.Format("AU{0}", reportRow)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("AU", bonusSectionFormulaFirstRowIndex, reportRow - 1, "SUM")); // Week 5 Sales
        //            wsNewReportSheet.GetCell(String.Format("AX{0}", reportRow)).ApplyFormula(Methods.GenerateExcelFractionFormulaWithZeroNumeratorChecking("AU", bonusSectionFormulaFirstRowIndex, reportRow - 1, reportRow, "N")); // Week 5 %

        //            wsNewReportSheet.GetCell(String.Format("BD{0}", reportRow)).ApplyFormula(Methods.GenerateBlankCheckingExcelFormula("BD", bonusSectionFormulaFirstRowIndex, reportRow - 1, "SUM")); //Total Bonus Qualified				


        //            //reportRow += 2;

        //            // Set the % cells of the main data section to have the same values as those of the bonus section: 

        //            wsNewReportSheet.GetCell(String.Format("AN{0}", mainSectionTotalsRowIndex + 1)).ApplyFormula(String.Format("=X{0}", reportRow));
        //            wsNewReportSheet.GetCell(String.Format("AQ{0}", mainSectionTotalsRowIndex + 1)).ApplyFormula(String.Format("=AK{0}", reportRow));
        //            //wsNewReportSheet.GetCell(String.Format("AT{0}", mainSectionTotalsRowIndex + 1)).ApplyFormula(String.Format("=X{0}", reportRow));
        //            //wsNewReportSheet.GetCell(String.Format("AW{0}", mainSectionTotalsRowIndex + 1)).ApplyFormula(String.Format("=X{0}", reportRow));
        //            wsNewReportSheet.GetCell(String.Format("AZ{0}", mainSectionTotalsRowIndex + 1)).ApplyFormula(String.Format("=AX{0}", reportRow));

        //            #endregion Apply the appropriate Excel formulae

        //            #endregion Step 4.3: Add the totals & averages row with the relevant formulae

        //            #region Step 4.4. Update the Conversion Summary Page

        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AI{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "U", reportRow));  // TotalWeek1Sales
        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AQ{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AH", reportRow));  // TotalWeek2Sales
        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AY{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AU", reportRow)); // TotalWeek5Sales

        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AM{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "X", reportRow)); // Week1ConversionPercentage
        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AU{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AK", reportRow)); // Week2ConversionPercentage
        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("BC{0}", reportSummaryRow + 1)).ApplyFormula(String.Format("='{0}'!{1}{2}", newWorksheetDescription, "AX", reportRow)); // Week5ConversionPercentage

        //            ++reportSummaryRow;

        //            reportRow += 2;

        //            #endregion Step 4.4. Update the Conversion Summary Page
        //        }
        //        else
        //        {
        //            #region Step 4.4. Update the Conversion Summary Page (Upgrades)

        //            // Because upgrade campaigns do not have bonuses, we don't have a totals row from which we can get the total sales for weeks 1, 4 and 5, and therefor we can't update the
        //            //Conversion Summary sheet using a formula that contains a reference
        //            for (int i = 0; i < dtMainSTLReportData.Rows.Count; i++)
        //            {
        //                totalSalesWeek1 += Convert.ToInt32(dtMainSTLReportData.Rows[i]["Week1Sales"]);
        //                totalSalesWeek4 += Convert.ToInt32(dtMainSTLReportData.Rows[i]["Week2Sales"]);
        //                totalSalesWeek5 += Convert.ToInt32(dtMainSTLReportData.Rows[i]["Week5Sales"]);
        //            }

        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AI{0}", reportSummaryRow + 1)).Value = totalSalesWeek1; // Week1Sales
        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AQ{0}", reportSummaryRow + 1)).Value = totalSalesWeek4; // Week4Sales
        //            wbResultingWorkbook.Worksheets["Conversion Summary"].GetCell(String.Format("AY{0}", reportSummaryRow + 1)).Value = totalSalesWeek5; // Week5Sales

        //            ++reportSummaryRow;

        //            #endregion Step 4.4. Update the Conversion Summary Page (Upgrades)

        //            reportRow += 3;
        //        }

        //        #endregion Step 4: Populate the bonus section

        //        #region Step 5: Indicate the average conversion

        //        #region Copy the template formatting for the data row

        //        reportTemplateRowIndex = 23;
        //        Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, 76, wsNewReportSheet, reportRow - 1, 0);

        //        #endregion Copy the template formatting for the data row

        //        #region Apply for formula

        //        //wsNewReportSheet.GetCell(String.Format("BL{0}", reportRow)).ApplyFormula(String.Format("=AVERAGE({0})", averageConversionFormula));
        //        wsNewReportSheet.GetCell(String.Format("BS{0}", reportRow)).ApplyFormula(averageConversionFormula);

        //        #endregion Apply for formula

        //        #endregion Step 5: Indicate the average conversion

        //        #region Step 6: Update the globally accessible data table with the current TSR's individual leads

        //        _dtIndividualLeads.Merge(dtIndividualLeadData);

        //        #endregion Step 6: Update the globally accessible data table with the current TSR's individual leads
        //    }
        //}

        //#endregion Additional Methods - To accomodate for the new STL Structure effective on 2015-11-16

        //private void STLReportOLD(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        SetCursor(Cursors.Wait);

        //        #region Declarations

        //        int emptyDataTableCount = 0;
        //        int reportSummaryRow; //= 10;

        //        //_noDataCampaigns = new List<string>();

        //        #endregion Declarations

        //        //foreach (Tuple<DateTime, DateTime, byte> dateRange in _dateRanges)
        //        //{
        //        //    #region Getting the date range(s) from _dateRanges and determine which kind of STL report to generate

        //        //    DateTime newFromDate = dateRange.Item1;
        //        //    DateTime newToDate = dateRange.Item2;
        //        //    byte stlReportType = dateRange.Item3;

        //        //    #endregion Getting the date range(s) from _dateRanges and determine which kind of STL report to generate

        //            #region Loop through each of the selected campaigns and generate the report

        //            foreach (DataRecord drCampaign in _lstSelectedCampaigns)
        //            {
        //                #region Setup Excel document
        //                reportSummaryRow = 10;
        //                _hasSummaryPageConversionTargetsBeenUpdated = false;

        //                var wbSTLReport = new Workbook();
        //                wbSTLReport.SetCurrentFormat(WorkbookFormat.Excel2007);

        //                string campaignCode = drCampaign.Cells["CampaignCode"].Value.ToString();
        //                string campaignName = drCampaign.Cells["CampaignName"].Value.ToString();

        //                string filePathAndName = String.Format("{0}Sales To Lead Report ({1}) {2} - {3} ~ {4}.xlsx",
        //                    GlobalSettings.UserFolder,
        //                    campaignCode,
        //                    /*newFromDate.ToString("yyyy-MM-dd"),*/ _reportStartDate.ToString("yyyy-MM-dd"),
        //                    /*newToDate.ToString("yyyy-MM-dd"), */ _reportEndDate.ToString("yyyy-MM-dd"),
        //                    DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

        //                Workbook wbTemplate;
        //                Uri uri = new Uri("/Templates/ReportTemplateSTLAndBCP.xlsx", UriKind.Relative);
        //                StreamResourceInfo info = Application.GetResourceStream(uri);
        //                if (info != null)
        //                {
        //                    wbTemplate = Workbook.Load(info.Stream, true);
        //                }
        //                else
        //                {
        //                    return;
        //                }

        //                string individualSTLReportTemplateSheetName = String.Empty;
        //                string conversionSummaryTemplateSheetName = String.Empty;

        //                switch (_stlOption)
        //                {
        //                    case 1:
        //                        individualSTLReportTemplateSheetName = "STL";
        //                        conversionSummaryTemplateSheetName = "Conversion Summary";
        //                        break;

        //                    case 2:
        //                        individualSTLReportTemplateSheetName = "STL20151019";
        //                        conversionSummaryTemplateSheetName = "Conversion Summary";
        //                        break;

        //                    case 3:
        //                    case 4:
        //                        individualSTLReportTemplateSheetName = "STL20151116";
        //                        conversionSummaryTemplateSheetName = "ConversionSummary20151116";
        //                        break;
        //                }


        //                //if (_stlOption == 1)
        //                //{
        //                //    individualSTLReportTemplateSheetName = "STL";
        //                //}
        //                //else
        //                //{
        //                //    individualSTLReportTemplateSheetName = "STL20151019";
        //                //}

        //                Worksheet wsSummarySheetTemplate = wbTemplate.Worksheets[conversionSummaryTemplateSheetName];
        //                Worksheet wsSTLDefaultSheetTemplate = wbTemplate.Worksheets[individualSTLReportTemplateSheetName];
        //                Worksheet wsLeadDetailsTemplate = wbTemplate.Worksheets["Details"];

        //                #endregion Setup Excel document

        //                #region Determine which TSRs were working on the current campaign within the specified date range

        //                long campaignID = Convert.ToInt64(drCampaign.Cells["CampaignID"].Value);

        //                DataSet dsCampaignSalesConsultants = UDM.Insurance.Business.Insure.INGetUsersByCampaignAndDateRange(campaignID, _reportStartDate, _reportEndDate, _staffType);
        //                DataTable dtCampaignSalesConsultants = dsCampaignSalesConsultants.Tables[0];


        //                //DataTable dtCampaignSalesConsultants = UDM.Insurance.Business.Insure.INGetUsersByCampaignAndDateRange(campaignID, newFromDate, newToDate);

        //                #endregion Determine which TSRs were working on the current campaign within the specified date range

        //                if (dtCampaignSalesConsultants.Rows.Count > 0)
        //                {

        //                    #region Add the summary sheet

        //                    //switch (_stlOption)
        //                    //{
        //                    //    case 1:
        //                    //    case 2:
        //                    //        AddSummarySheet(wbSTLReport, wsSummarySheetTemplate, campaignName, _reportStartDate, _reportEndDate, dtCampaignSalesConsultants);
        //                    //        break;

        //                    //    case 3:
        //                    //        AddSummarySheet(wbSTLReport, wsSummarySheetTemplate, campaignName, _reportStartDate, _reportEndDate, dtCampaignSalesConsultants);
        //                    //        break;
        //                    //}

        //                    AddSummarySheet(wbSTLReport, wsSummarySheetTemplate, campaignName, _reportStartDate, _reportEndDate, dtCampaignSalesConsultants, _stlOption);

        //                    #endregion Add the summary sheet

        //                    #region For each of the sales agents, add a separate sheet containing his / her STL Report

        //                    _dtIndividualLeads = new DataTable();

        //                    foreach (DataRow row in dtCampaignSalesConsultants.Rows)
        //                    {
        //                        long fkUserID = Convert.ToInt64(row["FKUserID"]);
        //                        string agentName = row["FullName"].ToString();

        //                        //AddIndividualSalesConsultantSTLReportToWorkbook(wbSTLReport, wsSTLDefaultSheetTemplate, agentName, campaignID, fkUserID, _reportStartDate, _reportEndDate, reportSummaryRow);

        //                        switch (_stlOption)
        //                        {
        //                            case 1:
        //                                AddIndividualSalesConsultantSTLReportToWorkbook(wbSTLReport, wsSTLDefaultSheetTemplate, agentName, campaignID, fkUserID, _reportStartDate, _reportEndDate, reportSummaryRow);
        //                                break;

        //                            case 2:
        //                                AddIndividualSalesConsultantSTLReportToWorkbook20151019(wbSTLReport, wsSTLDefaultSheetTemplate, agentName, campaignID, fkUserID, _reportStartDate, _reportEndDate, reportSummaryRow);
        //                                break;

        //                            case 3:
        //                            case 4:
        //                                AddIndividualSalesConsultantSTLReportToWorkbook20151116(wbSTLReport, wsSTLDefaultSheetTemplate, agentName, campaignID, fkUserID, _reportStartDate, _reportEndDate, reportSummaryRow);
        //                                break;
        //                        }

        //                        //if (_stlOption == 1)
        //                        //{
        //                        //    //AddIndividualSalesConsultantSTLReportToWorkbook(wbSTLReport, wsSTLDefaultSheetTemplate, agentName, campaignID, fkUserID, newFromDate, newToDate, reportSummaryRow);
        //                        //    AddIndividualSalesConsultantSTLReportToWorkbook(wbSTLReport, wsSTLDefaultSheetTemplate, agentName, campaignID, fkUserID, _reportStartDate, _reportEndDate, reportSummaryRow);
        //                        //}
        //                        //else
        //                        //{
        //                        //    //AddIndividualSalesConsultantSTLReportToWorkbook20151019(wbSTLReport, wsSTLDefaultSheetTemplate, agentName, campaignID, fkUserID, newFromDate, newToDate, reportSummaryRow);
        //                        //    AddIndividualSalesConsultantSTLReportToWorkbook20151019(wbSTLReport, wsSTLDefaultSheetTemplate, agentName, campaignID, fkUserID, _reportStartDate, _reportEndDate, reportSummaryRow);
        //                        //}

        //                        reportSummaryRow++;
        //                    }

        //                    #endregion For each of the sales agents, add a separate sheet containing his / her STL Report

        //                    #region Add an additional sheet containing every lead that comprises the report figures - for diagnostic purposes

        //                    //AddIndividualLeadDetailsSheetToWorkbook(wbSTLReport, wsLeadDetailsTemplate, _dtIndividualLeads, _reportStartDate, _reportEndDate);
        //                    //AddIndividualLeadDetailsSheetToWorkbook(wbSTLReport, wsLeadDetailsTemplate, _dtIndividualLeads, newFromDate, newToDate, stlReportType);
        //                    AddIndividualLeadDetailsSheetToWorkbook(wbSTLReport, wsLeadDetailsTemplate, _dtIndividualLeads, _reportStartDate, _reportEndDate);

        //                    #endregion Add an additional sheet containing every lead that comprises the report figures - for diagnostic purposes

        //                    #region Save and open the resulting workbook - if at least one sales agent has data available

        //                    if (emptyDataTableCount == _lstSelectedCampaigns.Count)
        //                    {
        //                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
        //                        {

        //                            ShowMessageBox(new INMessageBoxWindow1(), "There is no data available for the criteria you have specified. Please change some of them and try again.", "No data available", Embriant.Framework.ShowMessageType.Error);
        //                            return;
        //                        });
        //                    }
        //                    else
        //                    {
        //                        wbSTLReport.Save(filePathAndName);

        //                        //Display excel document
        //                        Process.Start(filePathAndName);
        //                    }

        //                    #endregion Save and open the resulting workbook - if at least one sales agent has data available
        //                }
        //                else
        //                {
        //                    //_noDataCampaigns.Add(campaignName);

        //                    if (dsCampaignSalesConsultants.Tables[1].Rows.Count > 0)
        //                    {
        //                        string message = dsCampaignSalesConsultants.Tables[1].Rows[0]["ZeroTSRMessage"].ToString();

        //                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
        //                        {

        //                            ShowMessageBox(new INMessageBoxWindow1(), message, "Zero lead assignees", Embriant.Framework.ShowMessageType.Error);
        //                            return;
        //                        });
        //                    }
        //                }
        //            }

        //            #endregion Loop through each of the selected campaigns and generate the report
        //        }
        //    //}

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }

        //    finally
        //    {
        //        SetCursor(Cursors.Arrow);
        //    }
        //}

        #endregion OLD

        private void STLReport(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataSet dsSTLReportDataMultipleCampaigns = new DataSet();

                #region Get the report data - and exit the method if there is no data available
                //if (_isUpgradeSTLReport)
                //{
                //    dsSTLReportDataMultipleCampaigns = Business.Insure.INReportSTLMultipleCampaignsUpgrades(_fkINCampaignIDs, _reportStartDate, _reportEndDate, _staffType, _stlOption, _stlConversionPercentageOption);
                //}
                 if (_stlOptionFromCmb>=10)
                {
                    dsSTLReportDataMultipleCampaigns = Business.Insure.INReportSTLMultipleCampaignsJuly2017Structure(_fkINCampaignIDs, _reportStartDate, _reportEndDate, _staffType, _stlOption, _stlConversionPercentageOption);
                }
                else if(level2Checked == true)
                {
                    dsSTLReportDataMultipleCampaigns = Business.Insure.INReportSTLMultipleCampaignsLevel2(_fkINCampaignIDs, _reportStartDate, _reportEndDate, _staffType, _stlOption, _stlConversionPercentageOption, Convert.ToByte(RData.STLBatchType ?? Business.lkpINSTLBatchType.Combined));
                }
                else
                {
                    //This is the latest stored procedure for the STL report.
                    dsSTLReportDataMultipleCampaigns = Business.Insure.INReportSTLMultipleCampaigns(_fkINCampaignIDs, _reportStartDate, _reportEndDate, _staffType, _stlOption, _stlConversionPercentageOption, Convert.ToByte(RData.STLBatchType ?? Business.lkpINSTLBatchType.Combined));
                }
                
                DataTable dtCampaigns = dsSTLReportDataMultipleCampaigns.Tables[0];

                foreach (DataRow drCampaign in dtCampaigns.Rows)
                {
                    // The 4th data table in the dataset is the one most likely to have data in it. Check that table to see if there is at least 1 row. 
                    //But first, we need to filter out only the current campaign's data:
                    string filterString = drCampaign["FilterString"].ToString();

                    var filteredRows = dsSTLReportDataMultipleCampaigns.Tables[3].Select(filterString).AsEnumerable();
                    if (filteredRows.Any())
                    {
                        BuildSTLReportForCurrentCampaign(dsSTLReportDataMultipleCampaigns, drCampaign);
                    }
                }

                #endregion Get the report data - and exit the method if there is no data available

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

        //private void STLReportUpgradesNew(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        SetCursor(Cursors.Wait);

        //        DataSet dsSTLReportDataMultipleCampaigns = new DataSet();

        //        #region Get the report data - and exit the method if there is no data available
        //        dsSTLReportDataMultipleCampaigns = Business.Insure.INReportSTLMultipleCampaignsUpgrades(_fkINCampaignIDs, _reportStartDate, _reportEndDate, _staffType, _stlOption, _stlConversionPercentageOption);

        //        DataTable dtCampaigns = dsSTLReportDataMultipleCampaigns.Tables[0];

        //        //foreach (DataRow drCampaign in dtCampaigns.Rows)
        //        //{
        //        //    // The 4th data table in the dataset is the one most likely to have data in it. Check that table to see if there is at least 1 row. 
        //        //    //But first, we need to filter out only the current campaign's data:
        //        //    string filterString = drCampaign["FilterString"].ToString();

        //        //    var filteredRows = dsSTLReportDataMultipleCampaigns.Tables[3].Select(filterString).AsEnumerable();
        //        //    if (filteredRows.Any())
        //        //    {
        //                BuildSTLReportForCurrentCampaignUpgrades(dsSTLReportDataMultipleCampaigns/*, drCampaign*/);
        //        //    }
        //        //}

        //        #endregion Get the report data - and exit the method if there is no data available

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

        #region Build the STL report, one campaign at a time

        private void BuildSTLReportForCurrentCampaign(DataSet dsSTLReportData, DataRow drCampaign)
        {
            // We assume that the dataset we're working with contains data, because we have already tested for this in STLReport(object sender, DoWorkEventArgs e)

            #region Partition the given dataset

            //DataTable dtSalesConsultants = dsSTLReportData.Tables[1];
            DataTable dtConversionSummarySheetExcelCellDataTableColumnMappings = dsSTLReportData.Tables[4];
            DataTable dtConversionSummarySheetExcelCellTotalsFormulasMappings = dsSTLReportData.Tables[5];

            #endregion Partition the given dataset

            #region Definitions & Initializations

            // Reset this value for each campaign
            _conversionSummarySheetRowIndex = Convert.ToInt32(drCampaign["ConversionSummaryTemplateDataRowIndex"]);

            // This
            string currentTSRWorsksheetName = String.Empty;
            int mainDataSectionTotalsRowIndex = 0;

            #endregion Definitions & Initializations

            #region Setup Excel document

            string campaignCode = drCampaign["CampaignCode"].ToString();

            string filePathAndName = String.Empty;
            if (String.IsNullOrEmpty(_liveDebugTestIndicator))
            {
                filePathAndName = String.Format("{0}STL Report ({1}) {2} - {3} ~ {4}.xlsx",
                GlobalSettings.UserFolder,
                campaignCode,
                _reportStartDate.ToString("yyyy-MM-dd"),
                _reportEndDate.ToString("yyyy-MM-dd"),
                DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
            }
            else
            {
                filePathAndName = String.Format("{0}STL Report ({1}) - ({2}) - {3} - {4} ~ {5}.xlsx",
                GlobalSettings.UserFolder,
                campaignCode,
                _liveDebugTestIndicator,
                _reportStartDate.ToString("yyyy-MM-dd"),
                _reportEndDate.ToString("yyyy-MM-dd"),
                DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
            }

            Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateSTLAndBCP.xlsx");
            Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

            #endregion Setup Excel document

            #region Insert the conversion cover sheet

            Worksheet wsConversionSummaryTemplate;
            Worksheet wsConversionSummary;
            InsertConversionCoverSheet(wbTemplate, wbReport, out wsConversionSummaryTemplate, out wsConversionSummary, drCampaign);

            #endregion Insert the conversion cover sheet

            #region Insert a worksheet that contains the current sales consultant's STL Report

             string currentCampaignSalesAgentsFilterString = drCampaign["FilterString"].ToString();
            var filteredRows = dsSTLReportData.Tables[1].Select(currentCampaignSalesAgentsFilterString).AsEnumerable();
            if (filteredRows.Any())
            {
                DataTable dtSalesConsultants = dsSTLReportData.Tables[1].Select(currentCampaignSalesAgentsFilterString).CopyToDataTable();

                foreach (DataRow row in dtSalesConsultants.Rows)
                {
                    #region Insert a separate STL report worksheet for each sales consultant

                    InsertSTLReportForCurrentConsultant(wbTemplate, wbReport, dsSTLReportData, drCampaign, row, out currentTSRWorsksheetName, out mainDataSectionTotalsRowIndex);

                    #endregion Insert a separate STL report worksheet for each sales consultant

                    #region Update the conversion summary sheet

                    _conversionSummarySheetRowIndex = UpdateConversionSummarySheet(wsConversionSummaryTemplate, wsConversionSummary, dtConversionSummarySheetExcelCellDataTableColumnMappings, drCampaign, row, currentTSRWorsksheetName, mainDataSectionTotalsRowIndex, _conversionSummarySheetRowIndex);

                    #endregion Update the conversion summary sheet
                }

                #endregion Insert a worksheet that contains the current sales consultant's STL Report

                #region Insert a worksheet that contains the details of each individual lead that comprises the figures on the report

                InsertDetailedLeadDetailsSheet(wbTemplate, wbReport, dsSTLReportData, drCampaign);

                #endregion Insert a worksheet that contains the details of each individual lead that comprises the figures on the report

                #region Lastly, update the totals and averages on the conversion cover sheet by adding the appropriate formulae

                InsertConversionSummaryTotals(wsConversionSummaryTemplate, wsConversionSummary, dtConversionSummarySheetExcelCellTotalsFormulasMappings, drCampaign, _conversionSummarySheetRowIndex);
                _conversionSummarySheetRowIndex = 0;

                #endregion Lastly, update the totals and averages on the conversion cover sheet by adding the appropriate formulae

                #region Saves and opens the resulting Excel workbook - if there are any pages in the workbook

                if (wbReport.Worksheets.Count > 0)
                {
                    wbReport.Save(filePathAndName);

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

                #endregion Saves and opens the resulting Excel workbook - if there are any pages in the workbook
            }
        }

        //private void BuildSTLReportForCurrentCampaignUpgrades(DataSet dsSTLReportData/*, DataRow drCampaign*/)
        //{
        //    // We assume that the dataset we're working with contains data, because we have already tested for this in STLReport(object sender, DoWorkEventArgs e)

        //    #region Partition the given dataset

        //    //DataTable dtSalesConsultants = dsSTLReportData.Tables[1];
        //    DataTable dtConversionSummarySheetExcelCellDataTableColumnMappings = dsSTLReportData.Tables[4];
        //    DataTable dtConversionSummarySheetExcelCellTotalsFormulasMappings = dsSTLReportData.Tables[5];

        //    #endregion Partition the given dataset

        //    #region Definitions & Initializations

        //    //// Reset this value for each campaign
        //    //_conversionSummarySheetRowIndex = Convert.ToInt32(drCampaign["ConversionSummaryTemplateDataRowIndex"]);

        //    // This
        //    string currentTSRWorsksheetName = String.Empty;
        //    int mainDataSectionTotalsRowIndex = 0;

        //    #endregion Definitions & Initializations

        //    #region Setup Excel document

        //    //string campaignCode = drCampaign["CampaignCode"].ToString();

        //    string filePathAndName = String.Empty;
        //    if (String.IsNullOrEmpty(_liveDebugTestIndicator))
        //    {
        //        filePathAndName = String.Format("{0}STL Report {1} - {2} ~ {3}.xlsx",
        //        GlobalSettings.UserFolder,
        //        //campaignCode,
        //        _reportStartDate.ToString("yyyy-MM-dd"),
        //        _reportEndDate.ToString("yyyy-MM-dd"),
        //        DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
        //    }
        //    else
        //    {
        //        filePathAndName = String.Format("{0}STL Report - ({1}) - {2} - {3} ~ {4}.xlsx",
        //        GlobalSettings.UserFolder,
        //        //campaignCode,
        //        _liveDebugTestIndicator,
        //        _reportStartDate.ToString("yyyy-MM-dd"),
        //        _reportEndDate.ToString("yyyy-MM-dd"),
        //        DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
        //    }

        //    Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateSTLAndBCP.xlsx");
        //    Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

        //    #endregion Setup Excel document

        //    #region Insert the conversion cover sheet

        //    Worksheet wsConversionSummaryTemplate;
        //    Worksheet wsConversionSummary;
        //    //InsertConversionCoverSheet(wbTemplate, wbReport, out wsConversionSummaryTemplate, out wsConversionSummary, drCampaign);

        //    #endregion Insert the conversion cover sheet

        //    #region Insert a worksheet that contains the current sales consultant's STL Report

        //    string currentCampaignSalesAgentsFilterString = drCampaign["FilterString"].ToString();
        //    var filteredRows = dsSTLReportData.Tables[1].Select(currentCampaignSalesAgentsFilterString).AsEnumerable();
        //    if (filteredRows.Any())
        //    {
        //        DataTable dtSalesConsultants = dsSTLReportData.Tables[1].Select(currentCampaignSalesAgentsFilterString).CopyToDataTable();

        //        foreach (DataRow row in dtSalesConsultants.Rows)
        //        {
        //            #region Insert a separate STL report worksheet for each sales consultant

        //            InsertSTLReportForCurrentConsultant(wbTemplate, wbReport, dsSTLReportData, drCampaign, row, out currentTSRWorsksheetName, out mainDataSectionTotalsRowIndex);

        //            #endregion Insert a separate STL report worksheet for each sales consultant

        //            //#region Update the conversion summary sheet

        //            //_conversionSummarySheetRowIndex = UpdateConversionSummarySheet(wsConversionSummaryTemplate, wsConversionSummary, dtConversionSummarySheetExcelCellDataTableColumnMappings, drCampaign, row, currentTSRWorsksheetName, mainDataSectionTotalsRowIndex, _conversionSummarySheetRowIndex);

        //            //#endregion Update the conversion summary sheet
        //        }

        //        #endregion Insert a worksheet that contains the current sales consultant's STL Report

        //        //#region Insert a worksheet that contains the details of each individual lead that comprises the figures on the report

        //        //InsertDetailedLeadDetailsSheet(wbTemplate, wbReport, dsSTLReportData, drCampaign);

        //        //#endregion Insert a worksheet that contains the details of each individual lead that comprises the figures on the report

        //        //#region Lastly, update the totals and averages on the conversion cover sheet by adding the appropriate formulae

        //        //InsertConversionSummaryTotals(wsConversionSummaryTemplate, wsConversionSummary, dtConversionSummarySheetExcelCellTotalsFormulasMappings, drCampaign, _conversionSummarySheetRowIndex);
        //        //_conversionSummarySheetRowIndex = 0;

        //        //#endregion Lastly, update the totals and averages on the conversion cover sheet by adding the appropriate formulae

        //        #region Saves and opens the resulting Excel workbook - if there are any pages in the workbook

        //        if (wbReport.Worksheets.Count > 0)
        //        {
        //            wbReport.Save(filePathAndName);

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

        //        #endregion Saves and opens the resulting Excel workbook - if there are any pages in the workbook
        //    }
        //}

        private void InsertConversionCoverSheet(Workbook wbTemplate, Workbook wbReport, out Worksheet wsConversionSummaryTemplate, out Worksheet wsConversionSummary, DataRow drCurrentCampaign)
        
        
        {
            #region Get the conversion summary sheet configuration values

            string conversionSummaryTemplateSheetName = drCurrentCampaign["ConversionSummaryTemplateSheetName"].ToString();
            string conversionSummaryNewSheetName = drCurrentCampaign["ConversionSummaryNewSheetName"].ToString();
            byte conversionSummaryTemplateColumnSpan = Convert.ToByte(drCurrentCampaign["ConversionSummaryTemplateColumnSpan"]);
            byte conversionSummaryTemplateRowSpan = Convert.ToByte(drCurrentCampaign["ConversionSummaryTemplateRowSpan"]);
            //byte conversionSummaryTemplateDataRowIndex = Convert.ToByte(drCurrentCampaign["ConversionSummaryTemplateDataRowIndex"]);
            //byte conversionSummaryTemplateTotalsRowIndex = Convert.ToByte(drCurrentCampaign["ConversionSummaryTemplateTotalsRowIndex"]);
            string conversionSummarySheetHeadingCell = drCurrentCampaign["ConversionSummarySheetHeadingCell"].ToString();
            string conversionSummarySheetHeading = drCurrentCampaign["ConversionSummarySheetHeading"].ToString();

            string conversionSummarySheetFirstSTLTargetPercentageCell = drCurrentCampaign["ConversionSummarySheetFirstSTLTargetPercentageCell"].ToString();
            string conversionSummarySheetSecondSTLTargetPercentageCell = drCurrentCampaign["ConversionSummarySheetSecondSTLTargetPercentageCell"].ToString();
            string conversionSummarySheetThirdSTLTargetPercentageCell = drCurrentCampaign["ConversionSummarySheetThirdSTLTargetPercentageCell"].ToString();
            string conversionSummarySheetFourthSTLTargetPercentageCell = drCurrentCampaign["ConversionSummarySheetFourthSTLTargetPercentageCell"].ToString();

            string conversionSummarySheetThirdWeekSalesColumnHeadingCell = drCurrentCampaign["ConversionSummarySheetThirdWeekSalesColumnHeadingCell"].ToString();
            string conversionSummarySheetThirdWeekSalesColumnHeading = drCurrentCampaign["ConversionSummarySheetThirdWeekSalesColumnHeading"].ToString();
            string conversionSummarySheetThirdWeekSalesPercentageColumnHeadingCell = drCurrentCampaign["ConversionSummarySheetThirdWeekSalesPercentageColumnHeadingCell"].ToString();
            string conversionSummarySheetThirdWeekSalesPercentageColumnHeading = drCurrentCampaign["ConversionSummarySheetThirdWeekSalesPercentageColumnHeading"].ToString();


            // STL Percentages and cell addresses for the target percentages of the 3 weeks of the MAIN DATA Section:
            decimal firstSTLTargetPercentage = Convert.ToDecimal(drCurrentCampaign["FirstSTLTargetPercentage"]);
            decimal secondSTLTargetPercentage = Convert.ToDecimal(drCurrentCampaign["SecondSTLTargetPercentage"]);
            decimal thirdSTLTargetPercentage = Convert.ToDecimal(drCurrentCampaign["ThirdSTLTargetPercentage"]);
            decimal fourthSTLTargetPercentage = Convert.ToDecimal(drCurrentCampaign["FourthSTLTargetPercentage"]);

            #endregion Get the conversion summary sheet configuration values

            #region Adding the conversion cover sheet

            wsConversionSummaryTemplate = wbTemplate.Worksheets[conversionSummaryTemplateSheetName];
            wsConversionSummary = wbReport.Worksheets.Add(Methods.ParseWorksheetName(wbReport, conversionSummaryNewSheetName));
            Methods.CopyWorksheetOptionsFromTemplate(wsConversionSummaryTemplate, wsConversionSummary, true, true, true, true, true, false, false, false, true, true, true, true, true, true, true, true, false);

            wsConversionSummary.PrintOptions.ScalingType = ScalingType.FitToPages;
            wsConversionSummary.PrintOptions.MaxPagesHorizontally = 1;

            wsConversionSummary.Columns[34].Hidden = true;
            wsConversionSummary.Columns[35].Hidden = true;
            wsConversionSummary.Columns[36].Hidden = true;
            wsConversionSummary.Columns[37].Hidden = true;

            wsConversionSummary.Columns[42].Hidden = true;
            wsConversionSummary.Columns[43].Hidden = true;
            wsConversionSummary.Columns[44].Hidden = true;
            wsConversionSummary.Columns[45].Hidden = true;

            wsConversionSummary.Columns[50].Hidden = true;
            wsConversionSummary.Columns[51].Hidden = true;
            wsConversionSummary.Columns[52].Hidden = true;
            wsConversionSummary.Columns[53].Hidden = true;

            #endregion Adding the conversion cover sheet

            #region Populating the report details

            Methods.CopyExcelRegion(wsConversionSummaryTemplate, 0, 0, conversionSummaryTemplateRowSpan, conversionSummaryTemplateColumnSpan, wsConversionSummary, 0, 0);

            wsConversionSummary.GetCell(conversionSummarySheetHeadingCell).Value = conversionSummarySheetHeading;

            // Replacing the column headings of the 3rd week, depending on which STL option was selected
            wsConversionSummary.GetCell(conversionSummarySheetThirdWeekSalesColumnHeadingCell).Value = conversionSummarySheetThirdWeekSalesColumnHeading;
            wsConversionSummary.GetCell(conversionSummarySheetThirdWeekSalesPercentageColumnHeadingCell).Value = conversionSummarySheetThirdWeekSalesPercentageColumnHeading;

            // Adding the target percentages for the 3 different weeks:
            if (_stlOption == 4 || _stlOptionFromCmb >= 10 || _stlOption == 5)
            {
                wsConversionSummary.GetCell(conversionSummarySheetFirstSTLTargetPercentageCell).Value = firstSTLTargetPercentage;
            }            
            wsConversionSummary.GetCell(conversionSummarySheetSecondSTLTargetPercentageCell).Value = secondSTLTargetPercentage;
            wsConversionSummary.GetCell(conversionSummarySheetThirdSTLTargetPercentageCell).Value = thirdSTLTargetPercentage;
            wsConversionSummary.GetCell(conversionSummarySheetFourthSTLTargetPercentageCell).Value = fourthSTLTargetPercentage;

            #endregion Populating the report details

            //return wsConversionSummary;
        }

        private void InsertSTLReportForCurrentConsultant(Workbook wbTemplate, Workbook wbReport, DataSet dsSTLReportData, DataRow drCurrentCampaign, DataRow drCurrentSalesConsultant, out string worksheetName, out int mainDataSectionTotalsRowIndex)
        {
            #region Initiating the out variables, as this cannot be done in the body of the if-statement

            worksheetName = String.Empty;
            mainDataSectionTotalsRowIndex = 0;

            #endregion Initiating the out variables, as this cannot be done in the body of the if-statement

            #region Determining the data table that contains the current sheet's data - and exit this method if there are 0 rows

            string filterString = drCurrentSalesConsultant["FilterString"].ToString();
            string orderByString = drCurrentSalesConsultant["OrderByString"].ToString();

            var filteredRows = dsSTLReportData.Tables[2].Select(filterString, orderByString).AsEnumerable();
            if (filteredRows.Any())
            {
                #region Partition the given dataset

                DataTable dtCurrentSalesConsultantSTLReportData = dsSTLReportData.Tables[2].Select(filterString, orderByString).CopyToDataTable();

                DataTable dtCurrentSalesConsultantDataSectionExcelCellDataTableColumnMappings = dsSTLReportData.Tables[6];
                DataTable dtCurrentSalesConsultantDataSectionExcelCellTotalsFormulasMappings = dsSTLReportData.Tables[7];

                DataTable dtCurrentSalesConsultantBonusSectionExcelCellDataTableColumnMappings = dsSTLReportData.Tables[8];
                DataTable dtCurrentSalesConsultantBonusSectionExcelCellTotalsFormulasMappings = dsSTLReportData.Tables[9];

                #endregion Partition the given dataset

                #region Get the report configuration values

                string salesConsultantName = drCurrentSalesConsultant["SalesConsultantName"].ToString();
                string initialworksheetName = drCurrentSalesConsultant["WorksheetName"].ToString();

                string batchClassification = drCurrentSalesConsultant["BatchClassification"] != DBNull.Value ? drCurrentSalesConsultant["BatchClassification"].ToString() : null;

                string reportTemplateSheetName = drCurrentSalesConsultant["ReportTemplateSheetName"].ToString();
                byte reportTemplateColumnSpan = Convert.ToByte(drCurrentSalesConsultant["ReportTemplateColumnSpan"]);
                byte reportTemplateRowSpan = Convert.ToByte(drCurrentSalesConsultant["ReportTemplateRowSpan"]);
                byte reportTemplateDataRowIndex = Convert.ToByte(drCurrentSalesConsultant["ReportTemplateDataRowIndex"]);
                byte reportTemplateTotalsRowIndex = Convert.ToByte(drCurrentSalesConsultant["ReportTemplateTotalsRowIndex"]);
                byte reportTemplateTotalsRowSpan = Convert.ToByte(drCurrentSalesConsultant["ReportTemplateTotalsRowSpan"]);

                string reportHeadingCell = drCurrentSalesConsultant["ReportHeadingCell"].ToString();
                string reportHeading = drCurrentSalesConsultant["ReportHeading"].ToString();
                string reportSubHeadingCell = drCurrentSalesConsultant["ReportSubHeadingCell"].ToString();
                string reportSubHeading = drCurrentSalesConsultant["ReportSubHeading"].ToString();
                string salesConsultantNameCell = drCurrentSalesConsultant["SalesConsultantNameCell"].ToString();
                string reportDateCell = drCurrentSalesConsultant["ReportDateCell"].ToString();

                // STL Percentages and cell addresses for the target percentages of the 3 weeks of the MAIN DATA Section:
                
                
                decimal firstSTLTargetPercentage = Convert.ToDecimal(drCurrentCampaign["FirstSTLTargetPercentage"]);
                string firstSTLTargetPercentageCell = drCurrentCampaign["FirstSTLTargetPercentageCell"].ToString();
                decimal secondSTLTargetPercentage = Convert.ToDecimal(drCurrentCampaign["SecondSTLTargetPercentage"]);
                string secondSTLTargetPercentageCell = drCurrentCampaign["SecondSTLTargetPercentageCell"].ToString();
                decimal thirdSTLTargetPercentage = Convert.ToDecimal(drCurrentCampaign["ThirdSTLTargetPercentage"]);
                string thirdSTLTargetPercentageCell = drCurrentCampaign["ThirdSTLTargetPercentageCell"].ToString();
                decimal fourthSTLTargetPercentage = Convert.ToDecimal(drCurrentCampaign["FourthSTLTargetPercentage"]);
                string fourthSTLTargetPercentageCell = drCurrentCampaign["FourthSTLTargetPercentageCell"].ToString();

                // Column headings and initial cell addresses of the 3 weeks of the BONUS Section:
                string firstWeekBonusSplitColumnHeading = drCurrentCampaign["FirstWeekBonusSplitColumnHeading"].ToString();
                string firstWeekBonusSplitColumnHeadingCell = drCurrentCampaign["FirstWeekBonusSplitColumnHeadingCell"].ToString();
                string secondWeekBonusSplitColumnHeading = drCurrentCampaign["SecondWeekBonusSplitColumnHeading"].ToString();
                string secondWeekBonusSplitColumnHeadingCell = drCurrentCampaign["SecondWeekBonusSplitColumnHeadingCell"].ToString();
                string thirdWeekBonusSplitColumnHeading = drCurrentCampaign["ThirdWeekBonusSplitColumnHeading"].ToString();
                string thirdWeekBonusSplitColumnHeadingCell = drCurrentCampaign["ThirdWeekBonusSplitColumnHeadingCell"].ToString();

                //Bonus section configs
                byte reportTemplateBonusFirstRowIndex = Convert.ToByte(drCurrentSalesConsultant["ReportTemplateBonusFirstRowIndex"]);
                byte reportTemplateBonusRowSpan = Convert.ToByte(drCurrentSalesConsultant["ReportTemplateBonusRowSpan"]);
                byte reportTemplateBonusDataRowIndex = Convert.ToByte(drCurrentSalesConsultant["ReportTemplateBonusDataRowIndex"]);
                byte reportTemplateBonusTotalsRowIndex = Convert.ToByte(drCurrentSalesConsultant["ReportTemplateBonusTotalsRowIndex"]);

                //Average conversion
                byte averageConversionTemplateRowIndex = Convert.ToByte(drCurrentSalesConsultant["AverageConversionTemplateRowIndex"]);
                string averageConversionFormula = drCurrentSalesConsultant["AverageConversionFormula"].ToString();
                string averageConversionFormulaColumn = drCurrentSalesConsultant["AverageConversionFormulaColumn"].ToString();

                //Conversion summary sheet
                string conversionSummaryNewSheetName = drCurrentCampaign["ConversionSummaryNewSheetName"].ToString();
                string columnsToBeHidden = drCurrentCampaign["ColumnsToBeHidden"].ToString();

                #endregion Get the report configuration values

                #region Declarations & initializations

                int reportRow = reportTemplateDataRowIndex;
                int formulaStartRow = reportRow;
                int averageConversionFormulaRowIndex = 0;
                int bonusSectionFormulaFirstRowIndex = 0;

                #endregion Declarations & initializations

                #region Adding a new sheet for the current sales consultant

                Worksheet wsTemplate = wbTemplate.Worksheets[reportTemplateSheetName];

                if (batchClassification == null)
                {
                    worksheetName = Methods.ParseWorksheetName(wbReport, initialworksheetName);
                }
                else
                {
                    worksheetName = Methods.ParseWorksheetName(wbReport, initialworksheetName, " ", batchClassification);
                }

                Worksheet wsReport = wbReport.Worksheets.Add(worksheetName);
                Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsReport, true, true, true, true, true, false, false, false, true, true, true, true, true, true, true, true, false);

                wsReport.PrintOptions.ScalingType = ScalingType.FitToPages;
                wsReport.PrintOptions.MaxPagesHorizontally = 1;

                //wsReport.Columns[39].Hidden = true;
                //wsReport.Columns[40].Hidden = true;
                //wsReport.Columns[41].Hidden = true;

                //wsReport.Columns[45].Hidden = true;
                //wsReport.Columns[46].Hidden = true;
                //wsReport.Columns[47].Hidden = true;

                //wsReport.Columns[51].Hidden = true;
                //wsReport.Columns[52].Hidden = true;
                //wsReport.Columns[53].Hidden = true;

                //wsReport.Columns[57].Hidden = true;
                //wsReport.Columns[58].Hidden = true;
                //wsReport.Columns[59].Hidden = true;

                //wsReport.Columns[63].Hidden = true;
                //wsReport.Columns[64].Hidden = true;
                //wsReport.Columns[65].Hidden = true;

                #endregion Adding a new sheet for the current sales consultant

                #region Hiding all the unnecessary columns

                string[] columns = columnsToBeHidden.Split(new char[] {','});

                foreach (string currentColumn in columns)
                {
                    byte currentColumnIndex = Convert.ToByte(currentColumn);
                    wsReport.Columns[currentColumnIndex].Hidden = true;
                }

                #endregion Hiding all the unnecessary columns

                #region Populating the report details

                Methods.CopyExcelRegion(wsTemplate, 0, 0, reportTemplateRowSpan, reportTemplateColumnSpan, wsReport, 0, 0);

                wsReport.GetCell(reportHeadingCell).Value = reportHeading;
                wsReport.GetCell(reportSubHeadingCell).Value = reportSubHeading;
                wsReport.GetCell(salesConsultantNameCell).Value = salesConsultantName;
                wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // Adding the target percentages for the 3 different weeks:

                if (_stlOption == 4 || _stlOptionFromCmb >= 10 || _stlOption == 5)
                {
                    wsReport.GetCell(firstSTLTargetPercentageCell).Value = firstSTLTargetPercentage;
                }                
                wsReport.GetCell(secondSTLTargetPercentageCell).Value = secondSTLTargetPercentage;
                wsReport.GetCell(thirdSTLTargetPercentageCell).Value = thirdSTLTargetPercentage;
                wsReport.GetCell(fourthSTLTargetPercentageCell).Value = fourthSTLTargetPercentage;


                #endregion Populating the report details

                #region Add the data of the MAIN DATA SECTION

                reportRow = Methods.MapTemplatizedExcelValues(wsTemplate, dtCurrentSalesConsultantSTLReportData, dtCurrentSalesConsultantDataSectionExcelCellDataTableColumnMappings, reportTemplateDataRowIndex, 0, 0, reportTemplateColumnSpan, wsReport, reportRow, 0);
                mainDataSectionTotalsRowIndex = reportRow;

                #endregion Add the data of the MAIN DATA SECTION

                #region Add the totals / averages of the MAIN DATA SECTION

                reportRow = Methods.MapTemplatizedExcelFormulas(wsTemplate, dtCurrentSalesConsultantDataSectionExcelCellTotalsFormulasMappings, reportTemplateTotalsRowIndex, 0, reportTemplateTotalsRowSpan, reportTemplateColumnSpan, wsReport, reportRow, 0, formulaStartRow, reportRow - 1);
                mainDataSectionTotalsRowIndex = reportRow;

                // Add the average to cell AG just beneath the total
                wsReport.GetCell(String.Format("AG{0}", reportRow + 1)).ApplyFormula(String.Format("=AVERAGE(AG{0}:AG{1})", formulaStartRow + 1, reportRow));

                // Update averageConversionFormulaRowIndex
                averageConversionFormulaRowIndex = reportRow;

                #endregion Add the totals / averages of the MAIN DATA SECTION

                #region Add the data of the BONUS SECTION
                reportRow += 2;

                if (!(drCurrentCampaign["CampaignName"].ToString().CaseInsensitiveContains("Previously Excluded")))
                {                    
                    Methods.CopyExcelRegion(wsTemplate, reportTemplateBonusFirstRowIndex, 0, reportTemplateBonusRowSpan, reportTemplateColumnSpan, wsReport, reportRow, 0);

                    reportRow += 2;
                    bonusSectionFormulaFirstRowIndex = reportRow;

                    wsReport.GetCell(firstWeekBonusSplitColumnHeadingCell.Replace("#ROW#", reportRow.ToString())).Value = firstWeekBonusSplitColumnHeading;
                    wsReport.GetCell(secondWeekBonusSplitColumnHeadingCell.Replace("#ROW#", reportRow.ToString())).Value = secondWeekBonusSplitColumnHeading;
                    wsReport.GetCell(thirdWeekBonusSplitColumnHeadingCell.Replace("#ROW#", reportRow.ToString())).Value = thirdWeekBonusSplitColumnHeading;

                    reportRow++;

                    reportRow = Methods.MapTemplatizedExcelValues(wsTemplate, dtCurrentSalesConsultantSTLReportData, dtCurrentSalesConsultantBonusSectionExcelCellDataTableColumnMappings, reportTemplateBonusDataRowIndex, 0, 0, reportTemplateColumnSpan, wsReport, reportRow, 0);
                }

                //Methods.CopyExcelRegion(wsTemplate, reportTemplateBonusFirstRowIndex, 0, reportTemplateBonusRowSpan, reportTemplateColumnSpan, wsReport, reportRow, 0);                

                #endregion Add the data of the BONUS SECTION

                #region Add the totals / averages of the MAIN DATA SECTION

                if (!(drCurrentCampaign["CampaignName"].ToString().CaseInsensitiveContains("Previously Excluded")))
                {
                    reportRow = Methods.MapTemplatizedExcelFormulas(wsTemplate, dtCurrentSalesConsultantBonusSectionExcelCellTotalsFormulasMappings, reportTemplateBonusTotalsRowIndex, 0, 0, reportTemplateColumnSpan, wsReport, reportRow, 0, bonusSectionFormulaFirstRowIndex, reportRow - 1);
                }

                #endregion Add the totals / averages of the MAIN DATA SECTION

                #region Add the AVERAGE CONVERSION SECTION

                ++reportRow;
                //averageConversionFormulaRowIndex
                Methods.CopyExcelRegion(wsTemplate, averageConversionTemplateRowIndex, 0, 0, reportTemplateColumnSpan, wsReport, reportRow, 0);

                wsReport.GetCell(String.Format("{0}{1}", averageConversionFormulaColumn, reportRow + 1)).ApplyFormula(averageConversionFormula.Replace("#ROW#", averageConversionFormulaRowIndex.ToString()));

                #endregion Add the AVERAGE CONVERSION SECTION
            }

            #endregion Determining the data table that contains the current sheet's data - and exit this method if there are 0 rows
        }

        private int UpdateConversionSummarySheet(Worksheet wsTemplate, Worksheet wsReport, DataTable dtConversionSummarySheetExcelCellDataTableColumnMappings, DataRow drCurrentCampaign, DataRow drCurrentSalesConsultant, string worksheetName, int referencedReportRow, int reportRow)
        {
            if (!String.IsNullOrEmpty(worksheetName))
            {
                #region Get the conversion summary sheet configuration values

                byte conversionSummaryTemplateColumnSpan = Convert.ToByte(drCurrentCampaign["ConversionSummaryTemplateColumnSpan"]);
                byte conversionSummaryTemplateDataRowIndex = Convert.ToByte(drCurrentCampaign["ConversionSummaryTemplateDataRowIndex"]);

                #endregion Get the conversion summary sheet configuration values

                #region Add the cross-worksheet cell references

                reportRow = Methods.MapTemplatizedCrossSheetExcelCellReferences(wsTemplate, dtConversionSummarySheetExcelCellDataTableColumnMappings, conversionSummaryTemplateDataRowIndex, 0, 0, conversionSummaryTemplateColumnSpan, wsReport, worksheetName, referencedReportRow - 1, reportRow, 0);

                #endregion Add the cross-worksheet cell references
            }
            return reportRow;
        }

        private void InsertDetailedLeadDetailsSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsSTLReportData, DataRow drCurrentCampaign)
        {
            string filterString = drCurrentCampaign["FilterString"].ToString();
            string orderByString = drCurrentCampaign["OrderByString"].ToString();

            #region Partition the given dataset

            DataTable dtCurrentCampaignLeadDetailsSheet = dsSTLReportData.Tables[3].Select(filterString, orderByString).CopyToDataTable();
            DataTable dtCurrentCampaignLeadDetailsSheetExcelCellDataTableColumnMappings = dsSTLReportData.Tables[10];

            #endregion Partition the given dataset

            #region Get the lead details sheet configuration values

            bool insertLeadDetailsSheet  = Convert.ToBoolean(drCurrentCampaign["InsertLeadDetailsSheet"]);
            string leadDetailsTemplateSheetName = drCurrentCampaign["LeadDetailsTemplateSheetName"].ToString();
            byte leadDetailsTemplateColumnSpan = Convert.ToByte(drCurrentCampaign["LeadDetailsTemplateColumnSpan"]);
            byte leadDetailsTemplateRowSpan  = Convert.ToByte(drCurrentCampaign["LeadDetailsTemplateRowSpan"]);
            byte leadDetailsTemplateDataRowIndex = Convert.ToByte(drCurrentCampaign["LeadDetailsTemplateDataRowIndex"]);
            string leadDetailsSheetHeading = drCurrentCampaign["LeadDetailsSheetHeading"].ToString();

            string firstWeekSTLTargetPercentageColumnHeadingCell = drCurrentCampaign["FirstWeekSTLTargetPercentageColumnHeadingCell"].ToString();
            string firstWeekSTLTargetPercentageColumnHeading = drCurrentCampaign["FirstWeekSTLTargetPercentageColumnHeading"].ToString();
            string secondWeekSTLTargetPercentageColumnHeadingCell = drCurrentCampaign["SecondWeekSTLTargetPercentageColumnHeadingCell"].ToString();
            string secondWeekSTLTargetPercentageColumnHeading = drCurrentCampaign["SecondWeekSTLTargetPercentageColumnHeading"].ToString();
            string thirdWeekSTLTargetPercentageColumnHeadingCell = drCurrentCampaign["ThirdWeekSTLTargetPercentageColumnHeadingCell"].ToString();
            string thirdWeekSTLTargetPercentageColumnHeading = drCurrentCampaign["ThirdWeekSTLTargetPercentageColumnHeading"].ToString();

            #endregion Get the lead details sheet configuration values

            #region Declarations

            int reportRow = 3;

            #endregion Declarations

            #region Add the sheet to the workbook

            Worksheet wsTemplate = wbTemplate.Worksheets[leadDetailsTemplateSheetName];
            Worksheet wsLeadDetails = wbReport.Worksheets.Add("All Lead Details");

            Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsLeadDetails, true, true, true, true, true, true, true, false, true, true, true, true, true, true, true, true, false);

            wsLeadDetails.PrintOptions.ScalingType = ScalingType.FitToPages;
            wsLeadDetails.PrintOptions.MaxPagesHorizontally = 1;

            #endregion Add the sheet to the workbook

            #region Populating the lead details sheet details

            Methods.CopyExcelRegion(wsTemplate, 0, 0, leadDetailsTemplateRowSpan, leadDetailsTemplateColumnSpan, wsLeadDetails, 0, 0);

            wsLeadDetails.GetCell("A1").Value = leadDetailsSheetHeading;

            wsLeadDetails.GetCell(firstWeekSTLTargetPercentageColumnHeadingCell).Value = firstWeekSTLTargetPercentageColumnHeading;
            wsLeadDetails.GetCell(secondWeekSTLTargetPercentageColumnHeadingCell).Value = secondWeekSTLTargetPercentageColumnHeading;
            wsLeadDetails.GetCell(thirdWeekSTLTargetPercentageColumnHeadingCell).Value = thirdWeekSTLTargetPercentageColumnHeading;

            #endregion Populating the lead details sheet details

            #region Add the data of the MAIN DATA SECTION

            reportRow = Methods.MapTemplatizedExcelValues(wsTemplate, dtCurrentCampaignLeadDetailsSheet, dtCurrentCampaignLeadDetailsSheetExcelCellDataTableColumnMappings, leadDetailsTemplateDataRowIndex, 0, 0, leadDetailsTemplateColumnSpan, wsLeadDetails, reportRow, 0);

            #endregion Add the data of the MAIN DATA SECTION

        }

        private void InsertConversionSummaryTotals(Worksheet wsTemplate, Worksheet wsReport, DataTable dtConversionSummarySheetExcelCellTotalsFormulasMappings, DataRow drCurrentCampaign, int reportRow)
        {
            #region Get the conversion summary sheet configuration values

            byte conversionSummaryTemplateColumnSpan = Convert.ToByte(drCurrentCampaign["ConversionSummaryTemplateColumnSpan"]);
            byte conversionSummaryTemplateTotalsRowIndex = Convert.ToByte(drCurrentCampaign["ConversionSummaryTemplateTotalsRowIndex"]);

            #endregion Get the conversion summary sheet configuration values

            #region Declarations & initializations

            byte formulaStartRow = 10;

            #endregion Declarations & initializations

            #region Add the totals / averages

            reportRow = Methods.MapTemplatizedExcelFormulas(wsTemplate, dtConversionSummarySheetExcelCellTotalsFormulasMappings, conversionSummaryTemplateTotalsRowIndex, 0, 0, conversionSummaryTemplateColumnSpan, wsReport, reportRow, 0, formulaStartRow, reportRow - 1);

            #endregion Add the totals / averages
        }

        #endregion Build the STL report, one campaign at a time

        #region STL Report - Upgrades

        private void STLReportUpgrades(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup excel documents

                string filePathAndName = String.Format("{0}STL Report - Upgrades ~ {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));

                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateSTLAndBCP.xlsx");
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup excel documents

                #region Get the data

                DataSet dsUpgradeSTLReportDate = new DataSet();

                if (_stlOption == 8)
                {
                    dsUpgradeSTLReportDate = Business.Insure.INReportSTLUpgrades(_fkINCampaignIDs, _reportStartDate, _reportEndDate, _staffType);
                }
                else if (_stlOption == 9)
                {
                    dsUpgradeSTLReportDate = Business.Insure.INReportSTLUpgrades2(_fkINCampaignIDs, _reportStartDate, _reportEndDate, _staffType, _stlOption, _stlConversionPercentageOption);
                }
                else if (_stlOption == 13)
                {
                    dsUpgradeSTLReportDate = Business.Insure.INReportSTLUpgrades20180621(_fkINCampaignIDs, _reportStartDate, _reportEndDate, _staffType, _stlOption, _stlConversionPercentageOption);
                }
                else if (_stlOption == 14)
                {
                    dsUpgradeSTLReportDate = Business.Insure.INReportSTLUpgrades20180723(_fkINCampaignIDs, _reportStartDate, _reportEndDate, _staffType, _stlOption, _stlConversionPercentageOption, Convert.ToByte(RData.STLBatchType ?? Business.lkpINSTLBatchType.Combined));
                }
                

                if (dsUpgradeSTLReportDate.Tables[1].Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                #endregion Get the data

                foreach (DataRow row in dsUpgradeSTLReportDate.Tables[0].Rows)
                {
                    if (_stlOption == 8)
                    {
                        InsertIndividualUpgradeSTLReportDataSheet(wbTemplate, wbReport, dsUpgradeSTLReportDate, row);
                    }
                    else if (_stlOption == 9)
                    {
                        InsertIndividualUpgradeSTLReportDataSheet2(wbTemplate, wbReport, dsUpgradeSTLReportDate, row);
                    }                    
                    else if (_stlOption == 13)
                    {
                        InsertIndividualUpgradeSTLReportDataSheet20180621(wbTemplate, wbReport, dsUpgradeSTLReportDate, row);
                    }
                    else if (_stlOption == 14)
                    {
                        InsertIndividualUpgradeSTLReportDataSheet20180723(wbTemplate, wbReport, dsUpgradeSTLReportDate, row);
                    }
                }

                #region TODO: Adapt to get campaign IDs from XamDatagrid when the requirement becomes applicable

                //foreach (DataRecord record in _campaigns)
                //{
                //    if ((bool)record.Cells["Select"].Value)
                //    {
                //        #region Get report data from database

                //        long campaignID = Convert.ToInt32(record.Cells["CampaignID"].Value);

                //        #endregion Get report data from database

                //        #region Add the summary sheet

                //        //summarySheetRowIndex = InsertAndUpdateSummarySheet(wbTemplate, wbReport, dsBumpUpPotentialReportData, summarySheetRowIndex);
                //        ////++summarySheetRowIndex;

                //        #endregion Add the summary sheet

                //        #region Add the data sheet for the current sheet

                //        InsertBumpUpPotentialReportDataSheet(wbTemplate, wbReport, dsBumpUpPotentialReportData, record);

                //        #endregion Add the data sheet for the current sheet
                //    }
                //}

                #endregion TODO: Adapt to get campaign IDs from XamDatagrid when the requirement becomes applicable

                #region Save & Display the resulting workbook - if there is at least 1 worksheet

                if (wbReport.Worksheets.Count < 1)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", ShowMessageType.Information);
                    });
                }
                else
                {
                    //Save excel document
                    wbReport.Save(filePathAndName);

                    //Display excel document
                    Process.Start(filePathAndName);
                }

                #endregion Save & Display the resulting workbook - if there is at least 1 worksheet
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

        private void InsertIndividualUpgradeSTLReportDataSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsUpgradeSTLReportDate, DataRow drCurrentSalesConsultant)
        {
            string filterString = drCurrentSalesConsultant["FilterString"].ToString();

            var filteredRows = dsUpgradeSTLReportDate.Tables[1].Select(filterString).AsEnumerable();
            if (filteredRows.Any())
            {

                #region Partition the given dataset

                string orderByString = drCurrentSalesConsultant["OrderByString"].ToString();
                DataTable dtCurrentCampaignData = dsUpgradeSTLReportDate.Tables[1].Select(filterString, orderByString).CopyToDataTable();
                DataTable dtExcelSheetDataTableColumnMappings = dsUpgradeSTLReportDate.Tables[2];
                DataTable dtExcelSheetTotalsAndAverageColumnMappings = dsUpgradeSTLReportDate.Tables[3];

                #endregion Partition the given dataset

                #region Declarations & Initializations

                int reportRow = 13;
                int formulaStartRow = reportRow;

                byte templateDataSheetRowSpan = 12;
                byte templateColumnSpan = 62;
                byte templateRowIndex = 13;
                byte totalsTemplateRowIndex = 14;

                string salesConsultantName = drCurrentSalesConsultant["SalesConsultantName"].ToString();
                string worksheetTabName = Methods.ParseWorksheetName(wbReport, salesConsultantName); //drCurrentSalesConsultant["WorksheetTabName"].ToString();
                string campaignDataSheetTemplateName = "Upgrades"; //selectedCampaign.Cells["CampaignCategory"].Value.ToString();

                string reportHeadingCell = "A6";        //dtReportConfigs.Rows[0]["ReportHeadingCell"].ToString();
                string reportSubHeadingCell = "A8";     //dtReportConfigs.Rows[0]["ReportSubHeadingCell"].ToString();
                string reportDateCell = "BC10";           //dtReportConfigs.Rows[0]["ReportDateCell"].ToString();

                #endregion Declarations & Initializations

                #region Add the worksheet

                Worksheet wsReportTemplate = wbTemplate.Worksheets[campaignDataSheetTemplateName];
                Worksheet wsReport = wbReport.Worksheets.Add(worksheetTabName);
                Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                #endregion Add the worksheet

                #region Populating the report details

                Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
                wsReport.GetCell(reportHeadingCell).Value = salesConsultantName;

                if (_reportStartDate == _reportEndDate)
                {
                    wsReport.GetCell(reportSubHeadingCell).Value = String.Format(@"For {0}", _reportStartDate.ToString("dddd, d MMMM yyyy"));
                }
                else
                {
                    wsReport.GetCell(reportSubHeadingCell).Value = String.Format(@"For the period between {0} and {1}", _reportStartDate.ToString("dddd, d MMMM yyyy"), _reportEndDate.ToString("dddd, d MMMM yyyy"));
                }

                wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                #endregion Populating the report details

                #region Add the data

                reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentCampaignData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                #endregion Add the data

                #region Add the totals / averages

                reportRow = Methods.MapTemplatizedExcelFormulas(wsReportTemplate, dtExcelSheetTotalsAndAverageColumnMappings, totalsTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0, formulaStartRow, reportRow - 1);

                #endregion Add the totals / averages
            }
        }

        private void InsertIndividualUpgradeSTLReportDataSheet2(Workbook wbTemplate, Workbook wbReport, DataSet dsUpgradeSTLReportDate, DataRow drCurrentSalesConsultant)
        {
            string filterString = drCurrentSalesConsultant["FilterString"].ToString();

            var filteredRows = dsUpgradeSTLReportDate.Tables[1].Select(filterString).AsEnumerable();
            if (filteredRows.Any())
            {

                #region Partition the given dataset

                string orderByString = drCurrentSalesConsultant["OrderByString"].ToString();

                byte averageConversionTemplateRowIndex = Convert.ToByte(drCurrentSalesConsultant["AverageConversionTemplateRowIndex"]);
                string averageConversionFormula = drCurrentSalesConsultant["AverageConversionFormula"].ToString();
                string averageConversionFormulaColumn = drCurrentSalesConsultant["AverageConversionFormulaColumn"].ToString();

                DataTable dtCurrentCampaignData = dsUpgradeSTLReportDate.Tables[1].Select(filterString, orderByString).CopyToDataTable();
                DataTable dtCurrentCampaignTotals = dsUpgradeSTLReportDate.Tables[4].Select(filterString).CopyToDataTable();
                DataTable dtExcelSheetDataTableColumnMappings = dsUpgradeSTLReportDate.Tables[2];
                DataTable dtExcelSheetTotalsAndAverageColumnMappings = dsUpgradeSTLReportDate.Tables[3];

                DataTable dtExcelSheetBonusDataTableColumnMappings = dsUpgradeSTLReportDate.Tables[5];
                DataTable dtExcelSheetBonusTotalsAndAverageColumnMappings = dsUpgradeSTLReportDate.Tables[6];


                #endregion Partition the given dataset

                #region Declarations & Initializations

                int reportRow = 12;
                int formulaStartRow = reportRow;

                byte templateDataSheetRowSpan = 11;                
                byte templateColumnSpan = 87;
                byte templateRowIndex = 12;
                byte totalsTemplateRowIndex = 13;

                byte templateBonusSheetRowSpan = 2;
                byte totalsBonusTemplateRowIndex = 20;
                byte templateBonusRowIndex = 19;

                string salesConsultantName = drCurrentSalesConsultant["SalesConsultantName"].ToString();
                string worksheetTabName = Methods.ParseWorksheetName(wbReport, salesConsultantName); //drCurrentSalesConsultant["WorksheetTabName"].ToString();
                string campaignDataSheetTemplateName = "STL20180301_2_Weeks(Upgrades)"; //selectedCampaign.Cells["CampaignCategory"].Value.ToString();

                string reportHeadingCell = "L9";        //dtReportConfigs.Rows[0]["ReportHeadingCell"].ToString();
                string reportSubHeadingCell = "F7";     //dtReportConfigs.Rows[0]["ReportSubHeadingCell"].ToString();
                string reportDateCell = "BX9";           //dtReportConfigs.Rows[0]["ReportDateCell"].ToString();

                #endregion Declarations & Initializations

                #region Add the worksheet

                Worksheet wsReportTemplate = wbTemplate.Worksheets[campaignDataSheetTemplateName];
                Worksheet wsReport = wbReport.Worksheets.Add(worksheetTabName);
                Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                #endregion Add the worksheet

                #region Populating the report details

                Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
                wsReport.GetCell(reportHeadingCell).Value = salesConsultantName;

                if (_reportStartDate == _reportEndDate)
                {
                    wsReport.GetCell(reportSubHeadingCell).Value = String.Format(@"For {0}", _reportStartDate.ToString("dddd, d MMMM yyyy"));
                }
                else
                {
                    wsReport.GetCell(reportSubHeadingCell).Value = String.Format(@"For the period between {0} and {1}", _reportStartDate.ToString("dddd, d MMMM yyyy"), _reportEndDate.ToString("dddd, d MMMM yyyy"));
                }

                wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                #endregion Populating the report details

                #region Add the data

                reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentCampaignData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                #endregion Add the data

                #region Add the totals / averages

                //reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentCampaignTotals, dtExcelSheetTotalsAndAverageColumnMappings, totalsTemplateRowIndex, 0, 2, templateColumnSpan, wsReport, reportRow, 0);

                reportRow = Methods.MapTemplatizedExcelFormulas(wsReportTemplate, dtExcelSheetTotalsAndAverageColumnMappings, totalsTemplateRowIndex, 0, 2, templateColumnSpan, wsReport, reportRow, 0, formulaStartRow, reportRow - 1);

                int averageConversionFormulaRowIndex = reportRow;

                #endregion Add the totals / averages

                #region Copy Bonus Template

                reportRow = reportRow + 1;

                Methods.CopyExcelRegion(wsReportTemplate, 16, 0, templateBonusSheetRowSpan, templateColumnSpan, wsReport, reportRow, 0);

                #endregion Copy Bonus Template

                #region Add Bonus data

                reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentCampaignData, dtExcelSheetBonusDataTableColumnMappings, templateBonusRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow + 3, 0);

                #endregion Add Bonus data

                #region Add Bonus Totals

                reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentCampaignTotals, dtExcelSheetBonusTotalsAndAverageColumnMappings, totalsBonusTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                #endregion Add Bonus Totals

                #region Average Conversion Percentage
                
                Methods.CopyExcelRegion(wsReportTemplate, averageConversionTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                wsReport.GetCell(String.Format("{0}{1}", averageConversionFormulaColumn, reportRow + 1)).ApplyFormula(averageConversionFormula.Replace("#ROW#", averageConversionFormulaRowIndex.ToString()));

                #endregion Average Conversion Percentage

            }
        }

        private void InsertIndividualUpgradeSTLReportDataSheet20180621(Workbook wbTemplate, Workbook wbReport, DataSet dsUpgradeSTLReportDate, DataRow drCurrentSalesConsultant)
        {
            string filterString = drCurrentSalesConsultant["FilterString"].ToString();

            var filteredRows = dsUpgradeSTLReportDate.Tables[1].Select(filterString).AsEnumerable();
            if (filteredRows.Any())
            {

                #region Partition the given dataset

                string orderByString = drCurrentSalesConsultant["OrderByString"].ToString();

                byte averageConversionTargetTemplateRowIndex = Convert.ToByte(drCurrentSalesConsultant["AverageConversionTargetTemplateRowIndex"]);
                string averageConversionTargetFormula = drCurrentSalesConsultant["AverageConversionTargetFormula"].ToString();
                string averageConversionTargetFormulaColumn = drCurrentSalesConsultant["AverageConversionTargetFormulaColumn"].ToString();

                byte averageConversionTemplateRowIndex = Convert.ToByte(drCurrentSalesConsultant["AverageConversionTemplateRowIndex"]);
                string averageConversionFormula = drCurrentSalesConsultant["AverageConversionFormula"].ToString();
                string averageConversionFormulaColumn = drCurrentSalesConsultant["AverageConversionFormulaColumn"].ToString();

                byte averageContactTargetTemplateRowIndex = Convert.ToByte(drCurrentSalesConsultant["AverageContactTargetTemplateRowIndex"]);
                decimal averageContactTargetFormula = decimal.Parse(drCurrentSalesConsultant["AverageContactTargetFormula"].ToString());
                string averageContactTargetFormulaColumn = drCurrentSalesConsultant["AverageContactTargetFormulaColumn"].ToString();

                byte averageContactRateTemplateRowIndex = Convert.ToByte(drCurrentSalesConsultant["AverageContactRateTemplateRowIndex"]);
                string averageContactRateFormula = drCurrentSalesConsultant["AverageContactRateFormula"].ToString();
                string averageContactRateFormulaColumn = drCurrentSalesConsultant["AverageContactRateFormulaColumn"].ToString();

                DataTable dtCurrentCampaignData = dsUpgradeSTLReportDate.Tables[1].Select(filterString, orderByString).CopyToDataTable();
                DataTable dtCurrentCampaignTotals = dsUpgradeSTLReportDate.Tables[4].Select(filterString).CopyToDataTable();
                DataTable dtExcelSheetDataTableColumnMappings = dsUpgradeSTLReportDate.Tables[2];
                DataTable dtExcelSheetTotalsAndAverageColumnMappings = dsUpgradeSTLReportDate.Tables[3];

                DataTable dtExcelSheetBonusDataTableColumnMappings = dsUpgradeSTLReportDate.Tables[5];
                DataTable dtExcelSheetBonusTotalsAndAverageColumnMappings = dsUpgradeSTLReportDate.Tables[6];


                #endregion Partition the given dataset

                #region Declarations & Initializations

                int reportRow = 13;
                int formulaStartRow = reportRow;

                byte templateDataSheetRowSpan = 12;
                byte templateColumnSpan = 130;
                byte templateRowIndex = 13;
                byte totalsTemplateRowIndex = 14;

                byte templateBonusSheetRowSpan = 2;
                byte totalsBonusTemplateRowIndex = 21;
                byte templateBonusRowIndex = 20;

                string salesConsultantName = drCurrentSalesConsultant["SalesConsultantName"].ToString();
                string worksheetTabName = Methods.ParseWorksheetName(wbReport, salesConsultantName + " - Month " + drCurrentSalesConsultant["BatchMonth"].ToString()); //drCurrentSalesConsultant["WorksheetTabName"].ToString();
                string campaignDataSheetTemplateName = "STL20180621_4_Weeks(Upgrades)"; //selectedCampaign.Cells["CampaignCategory"].Value.ToString();

                string reportHeadingCell = "L9";        //dtReportConfigs.Rows[0]["ReportHeadingCell"].ToString();
                string reportSubHeadingCell = "F7";     //dtReportConfigs.Rows[0]["ReportSubHeadingCell"].ToString();
                string reportDateCell = "DH9";           //dtReportConfigs.Rows[0]["ReportDateCell"].ToString();

                #endregion Declarations & Initializations

                #region Add the worksheet

                Worksheet wsReportTemplate = wbTemplate.Worksheets[campaignDataSheetTemplateName];
                Worksheet wsReport = wbReport.Worksheets.Add(worksheetTabName);
                Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                #endregion Add the worksheet

                #region Populating the report details

                Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
                wsReport.GetCell(reportHeadingCell).Value = salesConsultantName;

                if (_reportStartDate == _reportEndDate)
                {
                    wsReport.GetCell(reportSubHeadingCell).Value = String.Format(@"For {0}", _reportStartDate.ToString("dddd, d MMMM yyyy"));
                }
                else
                {
                    wsReport.GetCell(reportSubHeadingCell).Value = String.Format(@"For the period between {0} and {1}", _reportStartDate.ToString("dddd, d MMMM yyyy"), _reportEndDate.ToString("dddd, d MMMM yyyy"));
                }

                wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                #endregion Populating the report details

                #region Add the data

                reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentCampaignData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                #endregion Add the data

                #region Add the totals / averages

                //reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentCampaignTotals, dtExcelSheetTotalsAndAverageColumnMappings, totalsTemplateRowIndex, 0, 2, templateColumnSpan, wsReport, reportRow, 0);

                reportRow = Methods.MapTemplatizedExcelFormulas(wsReportTemplate, dtExcelSheetTotalsAndAverageColumnMappings, totalsTemplateRowIndex, 0, 2, templateColumnSpan, wsReport, reportRow, 0, formulaStartRow, reportRow - 1);

                int averageConversionFormulaRowIndex = reportRow;

                #endregion Add the totals / averages

                #region Copy Bonus Template

                reportRow = reportRow + 1;

                Methods.CopyExcelRegion(wsReportTemplate, 17, 0, templateBonusSheetRowSpan, templateColumnSpan, wsReport, reportRow, 0);

                #endregion Copy Bonus Template

                if (int.Parse(drCurrentSalesConsultant["BatchMonth"].ToString()) == 1)
                {
                    #region Add Bonus data

                    reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentCampaignData, dtExcelSheetBonusDataTableColumnMappings, templateBonusRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow + 3, 0);

                    #endregion Add Bonus data

                    #region Add Bonus Totals

                    reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentCampaignTotals, dtExcelSheetBonusTotalsAndAverageColumnMappings, totalsBonusTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                    #endregion Add Bonus Totals
                }


                #region Average Conversion Percentage
                //Conversion Target
                Methods.CopyExcelRegion(wsReportTemplate, averageConversionTargetTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                wsReport.GetCell(String.Format("{0}{1}", averageConversionTargetFormulaColumn, reportRow + 1)).ApplyFormula(averageConversionTargetFormula.Replace("#ROW#", averageConversionFormulaRowIndex.ToString()));

                //Conversion Average
                Methods.CopyExcelRegion(wsReportTemplate, averageConversionTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow + 1, 0);

                wsReport.GetCell(String.Format("{0}{1}", averageConversionFormulaColumn, reportRow + 2)).ApplyFormula(averageConversionFormula.Replace("#ROW#", (averageConversionFormulaRowIndex).ToString()));

                //Contact Target
                Methods.CopyExcelRegion(wsReportTemplate, averageContactTargetTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow + 3, 0);

                wsReport.GetCell(String.Format("{0}{1}", averageContactTargetFormulaColumn, reportRow + 4)).Value = averageContactTargetFormula;//.ApplyFormula(averageContactTargetFormula.Replace("#ROW#", (averageConversionFormulaRowIndex + 3).ToString()));


                //Contact Rate
                Methods.CopyExcelRegion(wsReportTemplate, averageContactRateTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow + 4, 0);

                wsReport.GetCell(String.Format("{0}{1}", averageContactRateFormulaColumn, reportRow + 5)).ApplyFormula(averageContactRateFormula.Replace("#ROW#", (averageConversionFormulaRowIndex).ToString()));

                #endregion Average Conversion Percentage

            }
        }

        #region Monthly Upgrade STL

        private void InsertIndividualUpgradeSTLReportDataSheet20180723(Workbook wbTemplate, Workbook wbReport, DataSet dsUpgradeSTLReportDate, DataRow drCurrentSalesConsultant)
        {
            string filterString = drCurrentSalesConsultant["FilterString"].ToString();

            var filteredRows = dsUpgradeSTLReportDate.Tables[1].Select(filterString).AsEnumerable();
            if (filteredRows.Any())
            {

                #region Partition the given dataset

                string orderByString = drCurrentSalesConsultant["OrderByString"].ToString();

                byte averageConversionTargetTemplateRowIndex = Convert.ToByte(drCurrentSalesConsultant["AverageConversionTargetTemplateRowIndex"]);
                string averageConversionTargetFormula = drCurrentSalesConsultant["AverageConversionTargetFormula"].ToString();
                string averageConversionTargetFormulaColumn = drCurrentSalesConsultant["AverageConversionTargetFormulaColumn"].ToString();

                byte averageConversionTemplateRowIndex = Convert.ToByte(drCurrentSalesConsultant["AverageConversionTemplateRowIndex"]);
                string averageConversionFormula = drCurrentSalesConsultant["AverageConversionFormula"].ToString();
                string averageConversionFormulaColumn = drCurrentSalesConsultant["AverageConversionFormulaColumn"].ToString();

                byte averageContactTargetTemplateRowIndex = Convert.ToByte(drCurrentSalesConsultant["AverageContactTargetTemplateRowIndex"]);
                decimal averageContactTargetFormula = decimal.Parse(drCurrentSalesConsultant["AverageContactTargetFormula"].ToString());
                string averageContactTargetFormulaColumn = drCurrentSalesConsultant["AverageContactTargetFormulaColumn"].ToString();

                byte averageContactRateTemplateRowIndex = Convert.ToByte(drCurrentSalesConsultant["AverageContactRateTemplateRowIndex"]);
                string averageContactRateFormula = drCurrentSalesConsultant["AverageContactRateFormula"].ToString();
                string averageContactRateFormulaColumn = drCurrentSalesConsultant["AverageContactRateFormulaColumn"].ToString();

                DataTable dtCurrentCampaignData = dsUpgradeSTLReportDate.Tables[1].Select(filterString, orderByString).CopyToDataTable();
                DataTable dtCurrentCampaignTotals = dsUpgradeSTLReportDate.Tables[4].Select(filterString).CopyToDataTable();
                DataTable dtExcelSheetDataTableColumnMappings = dsUpgradeSTLReportDate.Tables[2];
                DataTable dtExcelSheetTotalsAndAverageColumnMappings = dsUpgradeSTLReportDate.Tables[3];

                DataTable dtExcelSheetBonusDataTableColumnMappings = dsUpgradeSTLReportDate.Tables[5];
                DataTable dtExcelSheetBonusTotalsAndAverageColumnMappings = dsUpgradeSTLReportDate.Tables[6];

                DataTable dtTemplateSettings = dsUpgradeSTLReportDate.Tables[7];


                #endregion Partition the given dataset

                #region Declarations & Initializations

                int reportRow = 13;
                int formulaStartRow = reportRow;

                byte templateDataSheetRowSpan = byte.Parse(dtTemplateSettings.Rows[0]["TemplateDataSheetRowSpan"].ToString());
                byte templateColumnSpan = byte.Parse(dtTemplateSettings.Rows[0]["TemplateColumnSpan"].ToString());
                byte templateRowIndex = byte.Parse(dtTemplateSettings.Rows[0]["TemplateRowIndex"].ToString());
                byte totalsTemplateRowIndex = byte.Parse(dtTemplateSettings.Rows[0]["TotalsTemplateRowIndex"].ToString());

                byte templateBonusSheetRowSpan = byte.Parse(dtTemplateSettings.Rows[0]["TemplateBonusSheetRowSpan"].ToString());
                byte totalsBonusTemplateRowIndex = byte.Parse(dtTemplateSettings.Rows[0]["TotalsBonusTemplateRowIndex"].ToString());
                byte templateBonusRowIndex = byte.Parse(dtTemplateSettings.Rows[0]["TemplateBonusRowIndex"].ToString());

                string salesConsultantName = drCurrentSalesConsultant["SalesConsultantName"].ToString();
                string worksheetTabName = Methods.ParseWorksheetName(wbReport, salesConsultantName + " - Month " + drCurrentSalesConsultant["BatchMonth"].ToString()); //drCurrentSalesConsultant["WorksheetTabName"].ToString();
                string campaignDataSheetTemplateName = dtTemplateSettings.Rows[0]["CampaignDataSheetTemplateName"].ToString(); //selectedCampaign.Cells["CampaignCategory"].Value.ToString();

                string reportHeadingCell = dtTemplateSettings.Rows[0]["ReportHeadingCell"].ToString();        //dtReportConfigs.Rows[0]["ReportHeadingCell"].ToString();
                string reportSubHeadingCell = dtTemplateSettings.Rows[0]["ReportSubHeadingCell"].ToString();     //dtReportConfigs.Rows[0]["ReportSubHeadingCell"].ToString();
                string reportDateCell = dtTemplateSettings.Rows[0]["ReportDateCell"].ToString();           //dtReportConfigs.Rows[0]["ReportDateCell"].ToString();

                #endregion Declarations & Initializations

                #region Add the worksheet

                Worksheet wsReportTemplate = wbTemplate.Worksheets[campaignDataSheetTemplateName];
                Worksheet wsReport = wbReport.Worksheets.Add(worksheetTabName);
                Methods.CopyWorksheetOptionsFromTemplate(wsReportTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                #endregion Add the worksheet

                #region Hiding all the unnecessary columns

                string columnsToBeHidden = dtTemplateSettings.Rows[0]["ColumnsToBeHidden"].ToString();

                string[] hiddenColumns = columnsToBeHidden.Split(new char[] {','});

                foreach (string currentColumn in hiddenColumns)
                {
                    byte currentColumnIndex = Convert.ToByte(currentColumn);
                    wsReport.Columns[currentColumnIndex].Hidden = true;
                }

                #endregion Hiding all the unnecessary columns

                #region Populating the report details

                Methods.CopyExcelRegion(wsReportTemplate, 0, 0, templateDataSheetRowSpan, templateColumnSpan, wsReport, 0, 0);
                wsReport.GetCell(reportHeadingCell).Value = salesConsultantName;

                if (_reportStartDate == _reportEndDate)
                {
                    wsReport.GetCell(reportSubHeadingCell).Value = String.Format(@"For {0}", _reportStartDate.ToString("dddd, d MMMM yyyy"));
                }
                else
                {
                    wsReport.GetCell(reportSubHeadingCell).Value = String.Format(@"For the period between {0} and {1}", _reportStartDate.ToString("dddd, d MMMM yyyy"), _reportEndDate.ToString("dddd, d MMMM yyyy"));
                }

                wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                #endregion Populating the report details

                #region Add the data

                reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentCampaignData, dtExcelSheetDataTableColumnMappings, templateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                #endregion Add the data

                #region Add the totals / averages

                //reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentCampaignTotals, dtExcelSheetTotalsAndAverageColumnMappings, totalsTemplateRowIndex, 0, 2, templateColumnSpan, wsReport, reportRow, 0);

                reportRow = Methods.MapTemplatizedExcelFormulas(wsReportTemplate, dtExcelSheetTotalsAndAverageColumnMappings, totalsTemplateRowIndex, 0, 2, templateColumnSpan, wsReport, reportRow, 0, formulaStartRow, reportRow - 1);

                int averageConversionFormulaRowIndex = reportRow;

                #endregion Add the totals / averages

                #region Copy Bonus Template

                reportRow = reportRow + 1;

                Methods.CopyExcelRegion(wsReportTemplate, 17, 0, templateBonusSheetRowSpan, templateColumnSpan, wsReport, reportRow, 0);

                #endregion Copy Bonus Template

                if (int.Parse(drCurrentSalesConsultant["BatchMonth"].ToString()) == 1)
                {
                    #region Add Bonus data

                    reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentCampaignData, dtExcelSheetBonusDataTableColumnMappings, templateBonusRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow + 3, 0);

                    #endregion Add Bonus data

                    #region Add Bonus Totals

                    reportRow = Methods.MapTemplatizedExcelValues(wsReportTemplate, dtCurrentCampaignTotals, dtExcelSheetBonusTotalsAndAverageColumnMappings, totalsBonusTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                    #endregion Add Bonus Totals
                }


                #region Average Conversion Percentage
                //Conversion Target
                Methods.CopyExcelRegion(wsReportTemplate, averageConversionTargetTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                wsReport.GetCell(String.Format("{0}{1}", averageConversionTargetFormulaColumn, reportRow + 1)).ApplyFormula(averageConversionTargetFormula.Replace("#ROW#", averageConversionFormulaRowIndex.ToString()));

                //Conversion Average
                Methods.CopyExcelRegion(wsReportTemplate, averageConversionTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow + 1, 0);

                wsReport.GetCell(String.Format("{0}{1}", averageConversionFormulaColumn, reportRow + 2)).ApplyFormula(averageConversionFormula.Replace("#ROW#", (averageConversionFormulaRowIndex).ToString()));

                //Contact Target
                Methods.CopyExcelRegion(wsReportTemplate, averageContactTargetTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow + 3, 0);

                wsReport.GetCell(String.Format("{0}{1}", averageContactTargetFormulaColumn, reportRow + 4)).Value = averageContactTargetFormula;//.ApplyFormula(averageContactTargetFormula.Replace("#ROW#", (averageConversionFormulaRowIndex + 3).ToString()));


                //Contact Rate
                Methods.CopyExcelRegion(wsReportTemplate, averageContactRateTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow + 4, 0);

                wsReport.GetCell(String.Format("{0}{1}", averageContactRateFormulaColumn, reportRow + 5)).ApplyFormula(averageContactRateFormula.Replace("#ROW#", (averageConversionFormulaRowIndex).ToString()));

                #endregion Average Conversion Percentage

            }
        }

        #endregion Monthly Upgrade STL



        #endregion STL Report - Upgrades

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

                    EnableAllControls(false);

                    BackgroundWorker worker = new BackgroundWorker();
                    if (_isUpgradeSTLReport)
                    {
                        worker.DoWork += STLReportUpgrades;
                    }
                    //else if (_isUpgradeSTLReport && _stlOption == 9)
                    //{
                    //    worker.DoWork += STLReportUpgradesNew;
                    //}
                    else
                    {
                        worker.DoWork += STLReport;
                    }

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
            xdgCampaigns.IsEnabled = isEnabled;
            calFromDate.IsEnabled = isEnabled;
            calToDate.IsEnabled = isEnabled;
            //chkListAllAgents.IsEnabled = isEnabled;
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
                DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;

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

        private void calFromDate_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calFromDate.SelectedDate.ToString(), out _reportStartDate);
            //EnableDisableExportButton();
        }

        private void calToDate_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calToDate.SelectedDate.ToString(), out _reportEndDate);
            //EnableDisableExportButton();
        }

        //private void cmbSTLOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    _stlOption = Convert.ToInt32(cmbSTLOption.SelectedValue);
        //}


        private void cmbBaseOrUpgrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBaseOrUpgrade.SelectedValue != null)
            {
                bool isUpgradeSTLReport = Convert.ToBoolean((cmbBaseOrUpgrade.SelectedItem as DataRowView).Row["IsUpgrade"]);

                //if (isUpgradeSTLReport)
                //{
                //    lblSTLOption.Visibility = Visibility.Hidden;
                //    cmbSTLOption.Visibility = Visibility.Hidden;

                //    lblSTLConversionPercentageOption.Visibility = Visibility.Hidden;
                //    cmbSTLConversionPercentageOption.Visibility = Visibility.Hidden;
                //}
                //else
                //{
                //    lblSTLOption.Visibility = Visibility.Visible;
                //    cmbSTLOption.Visibility = Visibility.Visible;
                //    lblSTLConversionPercentageOption.Visibility = Visibility.Visible;
                //    cmbSTLConversionPercentageOption.Visibility = Visibility.Visible;
                //}

                //DataTable dtCampaigns = UDM.Insurance.Business.Insure.INGetCampaigns(isUpgradeSTLReport);

                string campaignFilterString = (cmbBaseOrUpgrade.SelectedItem as DataRowView).Row["CampaignFilterString"].ToString();
                string campaignOrderByString = (cmbBaseOrUpgrade.SelectedItem as DataRowView).Row["CampaignOrderByString"].ToString();
                DataTable dtCampaigns = _dtAllCampaigns.Select(campaignFilterString, campaignOrderByString).CopyToDataTable();
                DataTable dtSTLOptions = _dtSTLOptions.Select(campaignFilterString, "[ID]").CopyToDataTable();
                DataTable dtSTLConversionPercentageOption = _dtSTLConversionPercentageOption.Select(campaignFilterString, "[ID]").CopyToDataTable();

                cmbSTLOption.Populate(dtSTLOptions, "Description", "ID");

                cmbSTLConversionPercentageOption.Populate(dtSTLConversionPercentageOption, "Description", "ID");

                xdgCampaigns.DataSource = dtCampaigns.DefaultView;
            }
        }


        #endregion Event Handlers

        private void cmbSTLOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            byte salesToLeadsOption = Convert.ToByte(cmbSTLOption.SelectedValue);
        }

        private void cmbStaffType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            byte staffType = Convert.ToByte(cmbStaffType.SelectedValue);
        }

        private void radLevel2_Checked(object sender, RoutedEventArgs e)
        {
            if (radLevel2.IsChecked == true)
            {
                level2Checked = true;
            }
            else
            {
                level2Checked = false;
            }
        }
    }
}
