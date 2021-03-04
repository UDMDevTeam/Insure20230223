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
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.Editors.Events;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using UDM.Insurance.Interface.Data;

namespace UDM.Insurance.Interface.Screens
{

    public partial class ReportTurnoverScreen
    {

        #region Constants

        //private string _fontName = "Calibri";
        //private const int _fontSize = 10;
        //private const int _pointsToTwipsFactor = 20;
        //private const int _fontHeight = _fontSize * _pointsToTwipsFactor;

        #endregion



        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        private List<Record> _campaigns;
        private DateTime _startDate = DateTime.Now;
        private DateTime _endDate = DateTime.Now;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        private byte _staffType;

        private bool _includeAdmin;

        private DataTable _dtAllCampaigns;

        private DataTable dtStaffType;

        private DataTable _dtAllAgents;

        private DataTable _dtAllQAs;

        private DataTable _dtAllSalesCoach;


        private DataSet dsTurnoverLookups;

        private DataSet dsTurnoverLookupsSalesCoaches;


        private bool isLookupsLoaded = false;

        private ReportData _rData = new ReportData();

        bool allRecordsSelected = false;

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

        private List<Record> _lstSelectedCampaigns;
        private string _fkCampaignIDs = "";

        private List<Record> _lstSelectedAgents;
        private string _fkUserIDs = "";

        private List<Record> _lstSelectedQAs;
        private string _fkQAIDs = "";

        private List<Record> _lstSelectedSalesCoachess;
        private string _fkSalesCoachesIDs = "";
        #endregion



        #region Constructors

        public ReportTurnoverScreen()
        {
            //LoadTurnoverLookups();
            InitializeComponent();
            //LoadCampaignInfo();
            LoadTurnoverLookups();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
            isLookupsLoaded = true;
        }

        #endregion



        #region Private Methods

        private void LoadTurnoverLookups()
        {
            dsTurnoverLookups = Insure.INGetTurnoverScreenLookups();
            _dtAllCampaigns = dsTurnoverLookups.Tables[0];
            _dtAllAgents = dsTurnoverLookups.Tables[3];
            _dtAllQAs = dsTurnoverLookups.Tables[4];

            LoadStaffTypes();
            
            cmbCampaignType.Populate(dsTurnoverLookups.Tables[2], "Description", "ID");
            cmbCampaignType.SelectedIndex = 2;

            //cmbCampaigns.Populate(dsLoadLookups.Tables[0], "CampaignName", "CampaignID");
            //cmbDestinationCampaigns.Populate(dsLoadLookups.Tables[1], "CampaignName", "CampaignID");

            LoadCampaignInfo();

            //string campaignFilterString = (cmbCampaignType.SelectedItem as DataRowView).Row["CampaignFilterString"].ToString();
            //string campaignOrderByString = (cmbCampaignType.SelectedItem as DataRowView).Row["CampaignOrderByString"].ToString();
            //DataTable dtCampaigns = _dtAllCampaigns.Select(campaignFilterString, campaignOrderByString).CopyToDataTable();

            //xdgCampaigns.DataSource = dtCampaigns.DefaultView;
        }

        private void LoadStaffTypes()
        {
            try
            {
                dtStaffType = dsTurnoverLookups.Tables[1].Select("[Company] = " + (int)RData.TurnoverCompanyMode).CopyToDataTable();

                DataView staffView = new DataView(dtStaffType, "", "[ID]", DataViewRowState.OriginalRows);
                DataTable dtStaffCmb = staffView.ToTable(false, "ID", "Description");
                cmbStaffType.Populate(dtStaffCmb, "Description", "ID");
                cmbStaffType.SelectedIndex = 2;

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

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
                //SetCursor(Cursors.Wait);
                if (RData.TurnoverReportMode == lkpINCampTSRReportMode.ByCampaign)
                {
                    //DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign");
                    //DataColumn column = new DataColumn("Select", typeof(bool));
                    //column.DefaultValue = false;
                    //dt.Columns.Add(column);
                    //dt.DefaultView.Sort = "CampaignName ASC";
                    //xdgCampaigns.DataSource = dt.DefaultView;

                    string campaignFilterString = (cmbCampaignType.SelectedItem as DataRowView).Row["CampaignFilterString"].ToString();
                    string campaignOrderByString = (cmbCampaignType.SelectedItem as DataRowView).Row["CampaignOrderByString"].ToString();
                    DataTable dtCampaigns = _dtAllCampaigns.Select(campaignFilterString, campaignOrderByString).CopyToDataTable();

                    xdgCampaigns.DataSource = dtCampaigns.DefaultView;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                //SetCursor(Cursors.Arrow);
            }
        }

        private void LoadAgentInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);
                if (RData.TurnoverReportMode == lkpINCampTSRReportMode.ByTSR)
                {
                    DataSet ds = Insure.INGetTurnoverAgents(RData.TurnoverCompanyMode, _staffType, RData.IncludeAdmin);

                    DataTable dt = ds.Tables[0];
                    DataColumn column = new DataColumn("Select", typeof(bool));
                    column.DefaultValue = false;
                    dt.Columns.Add(column);

                    xdgCampaigns.DataSource = dt.DefaultView;
                }
                //DataSet ds = Methods.ExecuteStoredProcedure("spGetSalesAgents2", null);

                //DataTable dt = ds.Tables[0];
                //DataColumn column = new DataColumn("IsChecked", typeof(bool));
                //column.DefaultValue = false;
                //dt.Columns.Add(column);

                //xdgCampaigns.DataSource = dt.DefaultView;
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

        private void LoadQAInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);
                if (RData.TurnoverReportMode == lkpINCampTSRReportMode.ByQA)
                {


                    //DataSet ds = Insure.INGetTurnoverAgents(RData.TurnoverCompanyMode, _staffType);

                    //DataTable dt = ds.Tables[0];
                    //DataColumn column = new DataColumn("Select", typeof(bool));
                    //column.DefaultValue = false;
                    //dt.Columns.Add(column);

                    //xdgCampaigns.DataSource = dt.DefaultView;

                    //string campaignFilterString = (cmbCampaignType.SelectedItem as DataRowView).Row["CampaignFilterString"].ToString();
                    //string campaignOrderByString = (cmbCampaignType.SelectedItem as DataRowView).Row["CampaignOrderByString"].ToString();
                    DataTable dtQAs = _dtAllQAs.Select().CopyToDataTable();

                    xdgCampaigns.DataSource = dtQAs.DefaultView;
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

        public void PreReportGenerationOperations(/*byte reportScope*/)
        {

            var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            if (RData.TurnoverReportMode == lkpINCampTSRReportMode.ByCampaign)
            {
                _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

                if (_lstSelectedCampaigns.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No campaigns selected", ShowMessageType.Error);
                    return;
                }
                else
                {
                    _fkCampaignIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                    _fkCampaignIDs = _fkCampaignIDs.Substring(0, _fkCampaignIDs.Length - 1);
                }
            }
            else if (RData.TurnoverReportMode == lkpINCampTSRReportMode.ByTSR)
            {
                _lstSelectedAgents = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

                if (_lstSelectedAgents.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 agent from the list.", "No agents selected", ShowMessageType.Error);
                    return;
                }
                else
                {
                    _fkUserIDs = _lstSelectedAgents.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                    _fkUserIDs = _fkUserIDs.Substring(0, _fkUserIDs.Length - 1);
                }
            }
            else if (RData.TurnoverReportMode == lkpINCampTSRReportMode.ByQA)
            {
                _lstSelectedQAs = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

                if (_lstSelectedQAs.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 Supervisor/QA from the list.", "No Team selected", ShowMessageType.Error);
                    return;
                }
                else
                {
                    _fkQAIDs = _lstSelectedQAs.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                    _fkQAIDs = _fkQAIDs.Substring(0, _fkQAIDs.Length - 1);
                }
            }
            else if (RData.TurnoverReportMode == lkpINCampTSRReportMode.SalesCoaches)
            {
                _lstSelectedSalesCoachess = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

                if (_lstSelectedSalesCoachess.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 Sales Coach from the list.", "No Team selected", ShowMessageType.Error);
                    return;
                }
                else
                {
                    _fkQAIDs = _lstSelectedSalesCoachess.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                    _fkQAIDs = _fkQAIDs.Substring(0, _fkQAIDs.Length - 1);
                }
            }


            //if (chkIncludeCumulativeSheet.IsChecked.HasValue)
            //{
            //    if (chkIncludeCumulativeSheet.IsChecked.Value)
            //    {
            //        _insertSingleSheetWithAllData = true;
            //    }
            //    else
            //    {
            //        _insertSingleSheetWithAllData = false;
            //    }
            //}
            //else
            //{
            //    _insertSingleSheetWithAllData = false;
            //}

            //EnableDisableControls(false);

            //_reportScope = reportScope;
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                int summarySheetRowIndex = 6;
                int summarySheetNewRowIndex = 0;
                int allAllocationsSheetRowIndex = 7;
                int allAllocationsTotalsFromRow = 7;
                int allAllocationsSheetTotalsRow = 8;
                int allAllocationsSheetNewRowIndex = 0;

                //#region Setup excel documents

                
                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateTurnover(2).xlsx");
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                string filePathAndName = String.Format("{0}Turnover Report ({1} - {2}) ~ {3}.xlsx",
                    GlobalSettings.UserFolder,
                    _startDate.ToString("yyyy-MM-dd"),
                    _endDate.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                bool allSalesAgents = false;
                //bool allCampaigns = false;

                //allSalesAgents = (bool)AllRecordsSelected();

                if (allRecordsSelected && (RData.TurnoverReportMode == lkpINCampTSRReportMode.ByTSR || RData.TurnoverReportMode == lkpINCampTSRReportMode.ByQA || RData.TurnoverReportMode == lkpINCampTSRReportMode.SalesCoaches))
                {
                    allSalesAgents = true;
                }
                else
                {
                    allSalesAgents = false;
                }

                //if (allRecordsSelected && RData.TurnoverReportMode == lkpINCampTSRReportMode.ByCampaign)
                //{
                //    allCampaigns = true;
                //}
                //else
                //{
                //    allCampaigns = false;
                //}


                DataSet dsTurnoverReport = Insure.INReportTurnover(_fkCampaignIDs, _fkUserIDs, _startDate, _endDate, RData.IncludeBumpups, RData.TurnoverCompanyMode, _staffType, allSalesAgents, _fkQAIDs);

                if (dsTurnoverReport.Tables[2].Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "There is no data available for the criteria you have specified. Please change some of them and try again.", "No data available", ShowMessageType.Information);
                    });

                    return;
                }

                DataRow drReportConfigs = dsTurnoverReport.Tables[0].Rows[0]; // Specify it only once here

                foreach (DataRow row in dsTurnoverReport.Tables[1].Rows)
                {
                    AddReportPage(wbTemplate, wbReport, dsTurnoverReport, row/*, drReportConfigs*/);
                }

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

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SetCursor(Cursors.Arrow);
            }
        }

        private void AddReportPage(Workbook wbTemplate, Workbook wbReport, DataSet dsTurnoverReportData, DataRow drCurrentCampaign/*, DataRow drReportConfigs*/)
        {
            try
            {


                #region Determining the data table that contains the current sheet's data - and exit this method if there are 0 rows

                string filterString = drCurrentCampaign["FilterString"].ToString();
                byte sourceDataTableIndex = Convert.ToByte(drCurrentCampaign["SourceDataTableIndex"]);
                byte sourceTotalsTableIndex = Convert.ToByte(drCurrentCampaign["SourceTotalsTableIndex"]);
                string pageTitle = drCurrentCampaign["PageTitle"].ToString();
                string campaignName = drCurrentCampaign["Campaign"].ToString();

                string campaignNameTitleColumn = "D";



                DataTable dtCurrentCampaignTurnoverReportData;

                DataTable dtCurrentCampaignTurnoverReportTotals;


                DataTable dtExcelSheetDataTableColumnMappings;

                DataTable dtTotalsMappings;

                //These 2 datatables ("dtExcelSheetDataTableColumnMappingsUpgrade" and "dtTotalsMappingsUpgrade") are for upgrades when the byTSR report mode is selected 
                //otherwise the above 2 datatables ("dtExcelSheetDataTableColumnMappings" and "dtTotalsMappings") are used and just have the upgrade mappings inserted into them below
                DataTable dtExcelSheetDataTableColumnMappingsUpgrade;

                DataTable dtTotalsMappingsUpgrade;

                dtExcelSheetDataTableColumnMappingsUpgrade = dsTurnoverReportData.Tables[10];

                dtTotalsMappingsUpgrade = dsTurnoverReportData.Tables[11];

                DataTable dtReportConfigs = dsTurnoverReportData.Tables[0];





                DataRow drReportConfigs;

                DataRow drReportConfigsUpgrade;

                drReportConfigsUpgrade = (from rowU in dtReportConfigs.AsEnumerable()
                                          where rowU.Field<string>("ReportTemplateSheetName").Trim().CaseInsensitiveContains("Upgrade")
                                          select rowU).FirstOrDefault();

                string reportTemplateSheetNameUpgrade = drReportConfigsUpgrade["ReportTemplateSheetName"].ToString();
                byte reportTemplateRowSpanUpgrade = Convert.ToByte(drReportConfigsUpgrade["ReportTemplateRowSpan"]);
                byte reportTemplateColumnSpanUpgrade = Convert.ToByte(drReportConfigsUpgrade["ReportTemplateColumnSpan"]);
                byte reportTemplateDataRowIndexUpgrade = Convert.ToByte(drReportConfigsUpgrade["ReportTemplateDataRowIndex"]);
                byte reportTemplateTotalsRowIndexUpgrade = Convert.ToByte(drReportConfigsUpgrade["ReportTemplateTotalsRowIndex"]);

                int reportRow = 0;//Convert.ToInt32(drReportConfigs["ReportTemplateDataRowIndex"]);
                int formulaStartRow = reportRow;

                //If the filter string is empty it means it is for the summary page
                //if (String.IsNullOrEmpty(filterString))
                //If the data table index is equal to 6 it means it is for the summary page
                if (sourceDataTableIndex == 6)
                {
                    dtCurrentCampaignTurnoverReportData = dsTurnoverReportData.Tables[sourceDataTableIndex];

                    dtCurrentCampaignTurnoverReportTotals = dsTurnoverReportData.Tables[sourceTotalsTableIndex];

                    dtExcelSheetDataTableColumnMappings = dsTurnoverReportData.Tables[7];

                    dtTotalsMappings = dsTurnoverReportData.Tables[9];

                    drReportConfigs = (from row in dtReportConfigs.AsEnumerable()
                                       where row.Field<string>("ReportTemplateSheetName").Trim() == "Summary"//.Trim().CaseInsensitiveContains("Summary")
                                       select row).FirstOrDefault();
                    reportRow = 7;


                }
                //If the data table index is equal to 12 it means it is for the TSR Summary page on the by QA turnover
                else if (sourceDataTableIndex == 12)
                {
                    dtCurrentCampaignTurnoverReportData = dsTurnoverReportData.Tables[sourceDataTableIndex];

                    dtCurrentCampaignTurnoverReportTotals = dsTurnoverReportData.Tables[sourceTotalsTableIndex];

                    dtExcelSheetDataTableColumnMappings = dsTurnoverReportData.Tables[14];

                    dtTotalsMappings = dsTurnoverReportData.Tables[15];

                    drReportConfigs = (from row in dtReportConfigs.AsEnumerable()
                                       where row.Field<string>("ReportTemplateSheetName").Trim() == "TSRSummary"//.Trim().CaseInsensitiveContains("Summary")
                                       select row).FirstOrDefault();
                    reportRow = 7;
                }
                //If the filter string is not empty it means it is not for the summary page
                else
                {
                    reportRow = 8;
                    var filteredRows = dsTurnoverReportData.Tables[sourceDataTableIndex].Select(filterString).AsEnumerable();
                    //if (!filteredRows.Any())
                    //{
                    //    return;
                    //}

                    var rowCurrentCampaignTurnoverReportData = dsTurnoverReportData.Tables[sourceDataTableIndex].Select(filterString);

                    dtCurrentCampaignTurnoverReportData = new DataTable();

                    if (rowCurrentCampaignTurnoverReportData.Any())
                    {
                        dtCurrentCampaignTurnoverReportData = rowCurrentCampaignTurnoverReportData.CopyToDataTable();
                    }
                    //dtCurrentCampaignTurnoverReportData = dsTurnoverReportData.Tables[sourceDataTableIndex].Select(filterString).CopyToDataTable();

                    var rowCurrentCampaignTurnoverReportTotals = dsTurnoverReportData.Tables[sourceTotalsTableIndex].Select(filterString);

                    dtCurrentCampaignTurnoverReportTotals = new DataTable();

                    if (rowCurrentCampaignTurnoverReportTotals.Any())
                    {
                        dtCurrentCampaignTurnoverReportTotals = rowCurrentCampaignTurnoverReportTotals.CopyToDataTable();
                    }

                    //dtCurrentCampaignTurnoverReportTotals = dsTurnoverReportData.Tables[sourceTotalsTableIndex].Select(filterString).CopyToDataTable();

                    dtExcelSheetDataTableColumnMappings = dsTurnoverReportData.Tables[3];

                    dtTotalsMappings = dsTurnoverReportData.Tables[5];



                    if (RData.TurnoverReportMode == lkpINCampTSRReportMode.ByCampaign)
                    {
                        if (campaignName.CaseInsensitiveContains("Upgrade"))
                        {
                            drReportConfigs = (from row in dtReportConfigs.AsEnumerable()
                                               where row.Field<string>("ReportTemplateSheetName").Trim().CaseInsensitiveContains("Upgrade")
                                               select row).FirstOrDefault();

                            dtExcelSheetDataTableColumnMappings = dsTurnoverReportData.Tables[10];

                            dtTotalsMappings = dsTurnoverReportData.Tables[11];

                            campaignNameTitleColumn = "E";
                        }
                        else
                        {
                            drReportConfigs = (from row in dtReportConfigs.AsEnumerable()
                                               where row.Field<string>("ReportTemplateSheetName").Trim().CaseInsensitiveContains("Base")
                                               select row).FirstOrDefault();
                        }
                    }
                    else
                    {
                        drReportConfigs = (from row in dtReportConfigs.AsEnumerable()
                                           where row.Field<string>("ReportTemplateSheetName").Trim().CaseInsensitiveContains("Base")
                                           select row).FirstOrDefault();
                    }

                }

                #endregion Determining the data table that contains the current sheet's data - and exit this method if there are 0 rows

                #region Partition the given dataset

                string orderByString = drCurrentCampaign["OrderByString"].ToString();



                #endregion Partition the given dataset

                #region Declarations & Initializations
                //reportRow = int.Parse(drReportConfigs["ReportTemplateDataRowIndex"].ToString());


                string reportTemplateSheetName = drReportConfigs["ReportTemplateSheetName"].ToString();
                byte reportTemplateRowSpan = Convert.ToByte(drReportConfigs["ReportTemplateRowSpan"]);
                byte reportTemplateColumnSpan = Convert.ToByte(drReportConfigs["ReportTemplateColumnSpan"]);
                byte reportTemplateDataRowIndex = Convert.ToByte(drReportConfigs["ReportTemplateDataRowIndex"]);
                byte reportTemplateTotalsRowIndex = Convert.ToByte(drReportConfigs["ReportTemplateTotalsRowIndex"]);

                //string reportTemplateSheetNameUpgrade = drReportConfigs["ReportTemplateSheetName"].ToString();
                //byte reportTemplateRowSpanUpgrade = Convert.ToByte(drReportConfigs["ReportTemplateRowSpan"]);
                //byte reportTemplateColumnSpanUpgrade = Convert.ToByte(drReportConfigs["ReportTemplateColumnSpan"]);
                //byte reportTemplateDataRowIndexUpgrade = Convert.ToByte(drReportConfigs["ReportTemplateDataRowIndex"]);
                //byte reportTemplateTotalsRowIndexUpgrade = Convert.ToByte(drReportConfigs["ReportTemplateTotalsRowIndex"]);



                string reportTitle = String.Empty;
                string reportSubTitle = String.Empty;
                string worksheetTabName = String.Empty;
                DateTime dateOfSale = DateTime.Now;

                string campaignOrCampaignClusterColumnName = drReportConfigs["CampaignColumnName"].ToString();
                string campaignOrCampaignClusterCodeColumnName = drReportConfigs["CampaignCodeColumnName"].ToString();

                reportSubTitle = drCurrentCampaign[campaignOrCampaignClusterColumnName].ToString();
                worksheetTabName = Methods.ParseWorksheetName(wbReport, drCurrentCampaign[campaignOrCampaignClusterCodeColumnName].ToString());


                #endregion Declarations & Initializations

                #region Adding a new sheet for the current campaign / campaign cluster

                Worksheet wsTurnoverReportTemplate = wbTemplate.Worksheets[reportTemplateSheetName];
                Worksheet wsTurnoverReportTemplateUpgrade = wbTemplate.Worksheets[reportTemplateSheetNameUpgrade];
                Worksheet wsTurnoverReport = wbReport.Worksheets.Add(worksheetTabName);

                Methods.CopyWorksheetOptionsFromTemplate(wsTurnoverReportTemplate, wsTurnoverReport, true, true, true, true, true, false, false, false, true, true, true, true, true, true, true, true, false);

                #endregion Adding a new sheet for the current campaign / campaign cluster

                #region Populating the report details



                #endregion Populating the report details




                if ((RData.TurnoverReportMode == lkpINCampTSRReportMode.ByTSR || RData.TurnoverReportMode == lkpINCampTSRReportMode.ByQA) && sourceDataTableIndex != 6 && sourceDataTableIndex != 12)
                {
                    Methods.CopyWorksheetOptionsFromTemplate(wsTurnoverReportTemplateUpgrade, wsTurnoverReport, true, true, true, true, true, false, false, false, true, true, true, true, true, true, true, true, false);
                    #region ByTSR
                    Methods.CopyExcelRegion(wsTurnoverReportTemplateUpgrade, 0, 0, reportTemplateRowSpan - 4, reportTemplateColumnSpanUpgrade, wsTurnoverReport, 0, 0);

                    wsTurnoverReport.GetCell("A1").Value = pageTitle;
                    wsTurnoverReport.GetCell("A3").Value = reportSubTitle;
                    wsTurnoverReport.GetCell("A5").Value = String.Format("Date Range: {0} - {1}", _startDate.ToString("d MMMM yyyy"), _endDate.ToString("d MMMM yyyy"));

                    int srcRowIndex = 5;
                    int srcRowSpan = 3;

                    int desRowIndex = 5;
                    int desColIndex = 3;

                    desRowIndex = reportRow - 2;



                    foreach (DataRow row in dtCurrentCampaignTurnoverReportTotals.Rows)
                    {
                        string TotalCampaignName = row["CampaignName"].ToString();


                        IEnumerable<DataRow> reportDataQuery = from myRow in dtCurrentCampaignTurnoverReportData.AsEnumerable()
                                                               where myRow.Field<string>("CampaignName") == row["CampaignName"].ToString()
                                                               select myRow;
                        if (reportDataQuery.Any())
                        {
                            if (TotalCampaignName.CaseInsensitiveContains("Upgrade"))
                            {
                                Methods.CopyExcelRegion(wsTurnoverReportTemplateUpgrade, srcRowIndex, 0, srcRowSpan, reportTemplateColumnSpanUpgrade, wsTurnoverReport, desRowIndex, 0);

                                campaignNameTitleColumn = "E";
                            }
                            else
                            {
                                Methods.CopyExcelRegion(wsTurnoverReportTemplate, srcRowIndex, 0, srcRowSpan, reportTemplateColumnSpan, wsTurnoverReport, desRowIndex, 0);

                                campaignNameTitleColumn = "D";
                            }


                            reportRow = desRowIndex + srcRowSpan;

                            wsTurnoverReport.GetCell(campaignNameTitleColumn + (desRowIndex + 1)).Value = row["CampaignName"].ToString();

                            DataTable dtCurrentCampTurnReportData = reportDataQuery.CopyToDataTable<DataRow>();

                            #region Add the data
                            if (TotalCampaignName.CaseInsensitiveContains("Upgrade"))
                            {
                                reportRow = Methods.MapTemplatizedExcelValues(wsTurnoverReportTemplateUpgrade, dtCurrentCampTurnReportData, dtExcelSheetDataTableColumnMappingsUpgrade, reportTemplateDataRowIndexUpgrade, 0, 0, reportTemplateColumnSpanUpgrade, wsTurnoverReport, reportRow, 0);
                            }
                            else
                            {
                                reportRow = Methods.MapTemplatizedExcelValues(wsTurnoverReportTemplate, dtCurrentCampTurnReportData, dtExcelSheetDataTableColumnMappings, reportTemplateDataRowIndex, 0, 0, reportTemplateColumnSpan, wsTurnoverReport, reportRow, 0);
                            }
                            #endregion Add the data
                        }


                        IEnumerable<DataRow> reportTotalsQuery = from myRow in dtCurrentCampaignTurnoverReportTotals.AsEnumerable()
                                                                 where myRow.Field<string>("CampaignName") == row["CampaignName"].ToString()
                                                                 select myRow;
                        if (reportTotalsQuery.Any())
                        {
                            DataTable dtCurrentCampTurnReportTotals = reportTotalsQuery.CopyToDataTable<DataRow>();

                            #region Add the totals / averages

                            if (TotalCampaignName.CaseInsensitiveContains("Upgrade"))
                            {
                                reportRow = Methods.MapTemplatizedExcelValues(wsTurnoverReportTemplateUpgrade, dtCurrentCampTurnReportTotals, dtTotalsMappingsUpgrade, reportTemplateTotalsRowIndexUpgrade, 0, 0, reportTemplateColumnSpanUpgrade, wsTurnoverReport, reportRow, 0);
                            }
                            else
                            {
                                reportRow = Methods.MapTemplatizedExcelValues(wsTurnoverReportTemplate, dtCurrentCampTurnReportTotals, dtTotalsMappings, reportTemplateTotalsRowIndex, 0, 0, reportTemplateColumnSpan, wsTurnoverReport, reportRow, 0);
                            }



                            desRowIndex = reportRow + 2;

                            #endregion Add the totals / averages
                        }
                    }

                    //================================================================================================================================================================================
                    //Base Grand Total
                    IEnumerable<DataRow> reportGrandTotalQuery = from myRow in dtCurrentCampaignTurnoverReportTotals.AsEnumerable()
                                                                 where myRow.Field<string>("CampaignCode") == "BaseGrandTotal"
                                                                 select myRow;

                    DataTable dtTurnReportGrandTotal = new DataTable();
                    if (reportGrandTotalQuery.Any())
                    {
                        dtTurnReportGrandTotal = reportGrandTotalQuery.CopyToDataTable<DataRow>();

                        reportRow = Methods.MapTemplatizedExcelValues(wsTurnoverReportTemplate, dtTurnReportGrandTotal, dtTotalsMappings, reportTemplateTotalsRowIndex, 0, 0, reportTemplateColumnSpan, wsTurnoverReport, reportRow + 1, 0);

                        wsTurnoverReport.GetCell("A" + (reportRow)).Value = "Base Grand Total:";
                    }

                    //================================================================================================================================================================================
                    //Upgrade Grand Total

                    IEnumerable<DataRow> reportUpgradeGrandTotalQuery = from myRow in dtCurrentCampaignTurnoverReportTotals.AsEnumerable()
                                                                        where myRow.Field<string>("CampaignCode") == "UpgradeGrandTotal"
                                                                        select myRow;

                    DataTable dtUpgradeTurnReportGrandTotal = new DataTable();
                    if (reportUpgradeGrandTotalQuery.Any())
                    {
                        dtUpgradeTurnReportGrandTotal = reportUpgradeGrandTotalQuery.CopyToDataTable<DataRow>();

                        reportRow = Methods.MapTemplatizedExcelValues(wsTurnoverReportTemplateUpgrade, dtUpgradeTurnReportGrandTotal, dtTotalsMappingsUpgrade, reportTemplateTotalsRowIndexUpgrade, 0, 0, reportTemplateColumnSpanUpgrade, wsTurnoverReport, reportRow + 1, 0);

                        wsTurnoverReport.GetCell("A" + (reportRow)).Value = "Upgrade Grand Total:";
                    }

                    #endregion ByTSR
                }

                #region ByCampaign
                else if (RData.TurnoverReportMode == lkpINCampTSRReportMode.ByCampaign || sourceDataTableIndex == 6 || sourceDataTableIndex == 12)
                {
                    //reportRow = 8;

                    Methods.CopyExcelRegion(wsTurnoverReportTemplate, 0, 0, reportTemplateRowSpan, reportTemplateColumnSpan, wsTurnoverReport, 0, 0);

                    if (sourceDataTableIndex != 6 && sourceDataTableIndex != 12)
                    {
                        wsTurnoverReport.GetCell(campaignNameTitleColumn + (reportRow - 2)).Value = drCurrentCampaign["Campaign"].ToString();
                    }

                    wsTurnoverReport.GetCell("A1").Value = pageTitle;
                    wsTurnoverReport.GetCell("A3").Value = reportSubTitle;
                    wsTurnoverReport.GetCell("A5").Value = String.Format("Date Range: {0} - {1}", _startDate.ToString("d MMMM yyyy"), _endDate.ToString("d MMMM yyyy"));

                    #region Add the data

                    reportRow = Methods.MapTemplatizedExcelValues(wsTurnoverReportTemplate, dtCurrentCampaignTurnoverReportData, dtExcelSheetDataTableColumnMappings, reportTemplateDataRowIndex, 0, 0, reportTemplateColumnSpan, wsTurnoverReport, reportRow, 0);

                    #endregion Add the data

                    #region Step 4: Add the totals / averages

                    reportRow = Methods.MapTemplatizedExcelValues(wsTurnoverReportTemplate, dtCurrentCampaignTurnoverReportTotals, dtTotalsMappings, reportTemplateTotalsRowIndex, 0, 0, reportTemplateColumnSpan, wsTurnoverReport, reportRow, 0);

                    //reportRow = Methods.MapTemplatizedExcelFormulas(wsTurnoverReportTemplate, dtTotalsMappings, reportTemplateTotalsRowIndex, 0, 0, reportTemplateColumnSpan, wsTurnoverReport, reportRow, 0, formulaStartRow, reportRow - 1);

                    #endregion Step 4: Add the totals / averages
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private int UpdateAllAllocationsWorksheet(Workbook wbReport, Worksheet wsTemplate, DataSet dsReportData, int currentRowIndex, Worksheet wsNewReportSheet)
        {

            #region Skip this method if there is no data for the current campaign

            if (dsReportData.Tables[0].Rows.Count == 0)
            {
                return currentRowIndex;
            }

            #endregion Skip this method if there is no data for the current campaign

            #region Partition the given dataset

            DataTable dtReportData = dsReportData.Tables[0];
            DataTable dtExcelColumnDataTableColumnMappings = dsReportData.Tables[6];
            //DataTable dtExcelFormulaMappings = dsReportData.Tables[3];

            #endregion Partition the given dataset

            #region Declarations & Initializations

            //int reportRow = 7;
            //int formulaStartRow = 7;

            int templateColumnSpan = 17;
            int dataTemplateRowIndex = 7;
            //int totalsTemplateRowIndex = 8;
            //string newWorksheetDescription = Methods.ParseWorksheetName(wbReport, campaignName).Replace("'", "");
            //Worksheet wsNewReportSheet = wbReport.Worksheets.Add(newWorksheetDescription);
            //Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsNewReportSheet, true, true, true, true, true, false, false, false, true, true, true, true, true, true, true, true, false);

            #endregion Declarations & Initializations

            #region Add the report headings and populate the details
            //only need to do most of this stuff once in the beginning




            //Methods.CopyExcelRegion(wsTemplate, 0, 0, 6, templateColumnSpan, wsNewReportSheet, 0, 0);
            //wsNewReportSheet.GetCell("A1").Value = String.Format("Lead Allocation Report - {0}", campaignName);
            //wsNewReportSheet.GetCell("A3").Value = String.Format("For the period between {0} and {1}", _startDate.ToString("dddd, d MMMM yyyy"), _endDate.ToString("dddd, d MMMM yyyy"));
            //wsNewReportSheet.GetCell("B5").Value = Insure.GetLoggedInUserNameAndSurname();
            //wsNewReportSheet.GetCell("P5").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            #endregion Add the report headings and populate the details

            #region Add the values

            currentRowIndex = Methods.MapTemplatizedExcelValues(wsTemplate, dtReportData, dtExcelColumnDataTableColumnMappings, dataTemplateRowIndex, 0, 0, templateColumnSpan, wsNewReportSheet, currentRowIndex, 0);
            //reportRow++;

            #endregion Add the values

            #region Totals & Averages

            //only need to do this at the end.
            return currentRowIndex++;

            #endregion Totals & Averages
        }

        private int UpdateSummaryWorksheet(Workbook wbTemplate, Workbook wbResultingWorkbook, DataSet dsReportData, string campaignName, int currentRowIndex)
        {
            #region Partition the given dataset

            DataTable dtSummarySheetData = dsReportData.Tables[1];
            DataTable dtExcelColumnDataTableColumnMappings = dsReportData.Tables[4];
            DataTable dtExcelFormulaMappings = dsReportData.Tables[5];

            #endregion Partition the given dataset

            #region Declarations & Initializations

            Worksheet wsTemplate;
            Worksheet wsSummarySheet;

            string summarySheetTemplateWorksheetName = "Summary";
            byte summarySheetTemplateColumnSpan = 6;
            int summaryEntryTemplateRowIndex = 8;
            int summaryEntryTemplateTotalsRowIndex = 9;
            int currentSummarySheetRowIndex = currentRowIndex;
            int formulaStartRow = 0;

            #endregion Declarations & Initializations

            #region Defining the template sheet

            wsTemplate = wbTemplate.Worksheets[summarySheetTemplateWorksheetName];

            #endregion Defining the template sheet

            #region Adding the Lead Details Sheet, if it does not exist yet

            if (!Methods.WorksheetExistsInWorkbook(wbResultingWorkbook, "Summary"))
            {
                wsSummarySheet = wbResultingWorkbook.Worksheets.Add("Summary");
                Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsSummarySheet, true, true, true, true, true, false, false, false, true, true, true, true, true, true, true, true, false);
                Methods.CopyExcelRegion(wsTemplate, 0, 0, 4, summarySheetTemplateColumnSpan, wsSummarySheet, 0, 0);
                wsSummarySheet.GetCell("A3").Value = String.Format("{0} to {1}", _startDate.ToString("yyyy-MM-dd"), _endDate.ToString("yyyy-MM-dd"));
                wsSummarySheet.GetCell("B5").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                wsSummarySheet = wbResultingWorkbook.Worksheets["Summary"];
            }

            #endregion Adding the Lead Details Sheet, if it does not exist yet

            #region Populate the body of the summary sheet

            if (dtSummarySheetData.Rows.Count > 0)
            {
                #region Insert the column headings and add the campaign name

                Methods.CopyExcelRegion(wsTemplate, 6, 0, 1, summarySheetTemplateColumnSpan, wsSummarySheet, currentSummarySheetRowIndex, 0);
                wsSummarySheet.GetCell(String.Format("A{0}", currentSummarySheetRowIndex + 1)).Value = campaignName;
                currentSummarySheetRowIndex += 2;

                #endregion Insert the column headings and add the campaign name

                #region Add the values

                formulaStartRow = currentSummarySheetRowIndex;
                currentSummarySheetRowIndex = Methods.MapTemplatizedExcelValues(wsTemplate, dtSummarySheetData, dtExcelColumnDataTableColumnMappings, summaryEntryTemplateRowIndex, 0, 0, summarySheetTemplateColumnSpan, wsSummarySheet, currentSummarySheetRowIndex, 0);
                
                #endregion Add the values
            }

            #endregion Populate the body of the summary sheet

            #region Totals & Averages

            if (dtSummarySheetData.Rows.Count > 0)
            {
                currentSummarySheetRowIndex = Methods.MapTemplatizedExcelFormulas(wsTemplate, dtExcelFormulaMappings, summaryEntryTemplateTotalsRowIndex, 0, 0, summarySheetTemplateColumnSpan, wsSummarySheet, currentSummarySheetRowIndex, 0, formulaStartRow, currentSummarySheetRowIndex - 1);
                currentSummarySheetRowIndex++;
            }

            #endregion Totals & Averages

            return currentSummarySheetRowIndex;
        }

        private void AddLeadAllocationReportWorksheet(Workbook wbReport, Worksheet wsTemplate, DataSet dsReportData, string campaignName)
        {
            #region Skip this method if there is no data for the current campaign

            if (dsReportData.Tables[0].Rows.Count == 0)
            {
                return;
            }

            #endregion Skip this method if there is no data for the current campaign

            #region Partition the given dataset

            DataTable dtReportData = dsReportData.Tables[0];
            DataTable dtExcelColumnDataTableColumnMappings = dsReportData.Tables[2];
            DataTable dtExcelFormulaMappings = dsReportData.Tables[3];

            #endregion Partition the given dataset

            #region Declarations & Initializations

            int reportRow = 7;
            int formulaStartRow = 7;

            int templateColumnSpan = 16;
            int dataTemplateRowIndex = 7;
            int totalsTemplateRowIndex = 8;
            string newWorksheetDescription = Methods.ParseWorksheetName(wbReport, campaignName).Replace("'", "");
            Worksheet wsNewReportSheet = wbReport.Worksheets.Add(newWorksheetDescription);
            Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsNewReportSheet, true, true, true, true, true, false, false, false, true, true, true, true, true, true, true, true, false);

            #endregion Declarations & Initializations

            #region Add the report headings and populate the details

            Methods.CopyExcelRegion(wsTemplate, 0, 0, 6, templateColumnSpan, wsNewReportSheet, 0, 0);
            wsNewReportSheet.GetCell("A1").Value = String.Format("Lead Allocation Report - {0}", campaignName);
            wsNewReportSheet.GetCell("A3").Value = String.Format("For the period between {0} and {1}", _startDate.ToString("dddd, d MMMM yyyy"), _endDate.ToString("dddd, d MMMM yyyy"));
            wsNewReportSheet.GetCell("B5").Value = Insure.GetLoggedInUserNameAndSurname();
            wsNewReportSheet.GetCell("P5").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            #endregion Add the report headings and populate the details

            #region Add the values

            reportRow = Methods.MapTemplatizedExcelValues(wsTemplate, dtReportData, dtExcelColumnDataTableColumnMappings, dataTemplateRowIndex, 0, 0, templateColumnSpan, wsNewReportSheet, reportRow, 0);
            //reportRow++;

            #endregion Add the values

            #region Totals & Averages

            reportRow = Methods.MapTemplatizedExcelFormulas(wsTemplate, dtExcelFormulaMappings, totalsTemplateRowIndex, 0, 0, templateColumnSpan, wsNewReportSheet, reportRow, 0, formulaStartRow, reportRow - 1);

            #endregion Totals & Averages

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
                //_campaigns = xdgCampaigns.Records;

                //var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                //_campaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));
                PreReportGenerationOperations();
                btnClose.IsEnabled = false;
                btnReport.IsEnabled = false;
                xdgCampaigns.IsEnabled = false;
                calStartDate.IsEnabled = false;
				calEndDate.IsEnabled = false;

                allRecordsSelected = AllRecordsSelected() ?? false;

                //Cal2.IsEnabled = false;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += Report;
                worker.RunWorkerCompleted += ReportCompleted;
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
                    //allRecordsSelected = (bool)AllRecordsSelected();
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

        private void Cal1_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calStartDate.SelectedDate.ToString(), out _startDate);
            EnableDisableExportButton();
        }

		private void Cal2_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
		{
			DateTime.TryParse(calEndDate.SelectedDate.ToString(), out _endDate);
			EnableDisableExportButton();
		}

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void radByCampaign_Checked(object sender, RoutedEventArgs e)
        {
            if (isLookupsLoaded == true)
            {
                _fkUserIDs = "";
                _fkQAIDs = "";
                LoadCampaignInfo();
            }            
        }

        private void radByTSR_Checked(object sender, RoutedEventArgs e)
        {
            if (isLookupsLoaded == true)
            {
                _fkCampaignIDs = "";
                _fkQAIDs = "";
                LoadAgentInfo();
            }
        }

        private void xdgCampaigns_FieldLayoutInitialized(object sender, Infragistics.Windows.DataPresenter.Events.FieldLayoutInitializedEventArgs e)
        {
            if (RData.TurnoverReportMode == lkpINCampTSRReportMode.ByCampaign)
            {
                Field fieldCampaignID = e.FieldLayout.Fields["CampaignID"];
                fieldCampaignID.Visibility = Visibility.Collapsed;
                //Field fieldCampaignCode = e.FieldLayout.Fields["CampaignCode"];
                //fieldCampaignCode.Visibility = Visibility.Collapsed;
                Field fieldCampaignName = e.FieldLayout.Fields["CampaignName"];
                fieldCampaignName.Visibility = Visibility.Visible;
                fieldCampaignName.Width = new FieldLength(290);
                Field fieldSelect = e.FieldLayout.Fields["Select"];
                fieldSelect.Visibility = Visibility.Collapsed;
                Field fieldIsUpgrade = e.FieldLayout.Fields["IsUpgrade"];
                fieldSelect.Visibility = Visibility.Collapsed;
            }
            else if (RData.TurnoverReportMode == lkpINCampTSRReportMode.ByTSR)
            {
                Field fieldID = e.FieldLayout.Fields["ID"];
                fieldID.Visibility = Visibility.Collapsed;
                Field fieldDescription = e.FieldLayout.Fields["Description"];
                fieldDescription.Visibility = Visibility.Visible;
                fieldDescription.Width = new FieldLength(290);
                fieldDescription.Label = "Agent Name";
                Field fieldSelect = e.FieldLayout.Fields["Select"];
                fieldSelect.Visibility = Visibility.Collapsed;
            }
            else if (RData.TurnoverReportMode == lkpINCampTSRReportMode.ByQA)
            {
                Field fieldID = e.FieldLayout.Fields["ID"];
                fieldID.Visibility = Visibility.Collapsed;
                Field fieldDescription = e.FieldLayout.Fields["Description"];
                fieldDescription.Visibility = Visibility.Visible;
                fieldDescription.Width = new FieldLength(290);
                fieldDescription.Label = "Team Name";
                Field fieldSelect = e.FieldLayout.Fields["Select"];
                fieldSelect.Visibility = Visibility.Collapsed;
            }
            else if (RData.TurnoverReportMode == lkpINCampTSRReportMode.SalesCoaches)
            {
                Field fieldID = e.FieldLayout.Fields["ID"];
                fieldID.Visibility = Visibility.Collapsed;
                Field fieldDescription = e.FieldLayout.Fields["Description"];
                fieldDescription.Visibility = Visibility.Visible;
                fieldDescription.Width = new FieldLength(290);
                fieldDescription.Label = "Sales Coach";
                Field fieldSelect = e.FieldLayout.Fields["Select"];
                fieldSelect.Visibility = Visibility.Collapsed;
            }


        }

        private void cmbStaffType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //_staffType = Convert.ToByte(cmbStaffType.SelectedValue);
            if (cmbStaffType.SelectedIndex != -1)
            {
                _staffType = Convert.ToByte(cmbStaffType.SelectedIndex);
                LoadAgentInfo();
            }
        }

        private void cmbCampaignType_DropDownClosed(object sender, EventArgs e)
        {

        }

        private void cmbCampaignType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCampaignType.SelectedValue != null && RData.TurnoverReportMode == lkpINCampTSRReportMode.ByCampaign)
            {

                //DataTable dtCampaigns = UDM.Insurance.Business.Insure.INGetCampaigns(isUpgradeSTLReport);

                string campaignFilterString = (cmbCampaignType.SelectedItem as DataRowView).Row["CampaignFilterString"].ToString();
                string campaignOrderByString = (cmbCampaignType.SelectedItem as DataRowView).Row["CampaignOrderByString"].ToString();
                DataTable dtCampaigns = _dtAllCampaigns.Select(campaignFilterString, campaignOrderByString).CopyToDataTable();

                xdgCampaigns.DataSource = dtCampaigns.DefaultView;
            }
        }



        #endregion

        private void radCompanyType_Checked(object sender, RoutedEventArgs e)
        {
            if (isLookupsLoaded)
            {
                LoadStaffTypes();
                LoadAgentInfo();
            }            
        }

        private void radByQA_Checked(object sender, RoutedEventArgs e)
        {
            if (isLookupsLoaded == true)
            {
                _fkUserIDs = "";
                _fkCampaignIDs = "";
                LoadQAInfo();
            }
            
        }

        private void chkIncludeAdmin_Checked(object sender, RoutedEventArgs e)
        {
            LoadAgentInfo();
        }

        private void radBySalesCoach_Checked(object sender, RoutedEventArgs e)
        {
            dsTurnoverLookupsSalesCoaches = Insure.INGetTurnoverScreenLookupsSalesCoaches();
            //_dtAllSalesCoach.Clear();
            _dtAllSalesCoach = dsTurnoverLookupsSalesCoaches.Tables[0];

            xdgCampaigns.DataSource = _dtAllSalesCoach.DefaultView;
        }
    }
}
#endregion