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
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Resources;
using System.Windows.Threading;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using UDM.Insurance.Business;
using Embriant.WPF.Controls;
using Orientation = Infragistics.Documents.Excel.Orientation;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportDailySalesChecking
    {
        private byte _reportMode = 1; //Normal
        private byte _reportType = 1; //Upgrades or Base

        private bool? _allRecordsCheckedTemp = false;
        public bool? AllRecordsCheckedTemp
        {
            get
            {
                return _allRecordsCheckedTemp;
            }
            set
            {
                _allRecordsCheckedTemp = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllRecordsCheckedTemp"));
            }
        }

        #region SalaryReportType Enumerator

        public enum SalaryReportType
        {
            Base = 1,
            Upgrade = 2
        }

        #endregion SalaryReportType Enumerator

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

        DataSet _dsLookups;

        private CheckBox _xdgCampaignsHeaderPrefixAreaCheckbox;
        private List<Record> _lstSelectedCampaigns;
        //private string _campaignIDList = String.Empty;

        private CheckBox _xdgCampaignClusterssHeaderPrefixAreaCheckbox;
        private List<Record> _lstSelectedCampaignClusters;
        //private string _campaignClusterIDList = String.Empty;

        private string _fkINCampaignFKINCampaignClusterIDs;

        private DateTime _fromDate; //= DateTime.Now;
        private DateTime _toDate; //= DateTime.Now;
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;
        private bool? _isReportRunning = false;

        SalaryReportType _salaryReportType;

        //private DataRow _campaignCluster;

        private bool _useCampaignClusters;
        private readonly bool _includeSystemUnitsColumn = true;

        private bool _bonusSales = false;

        private List<Record> _lstSelectedTempAgents;
        private string _fkUserIDs;

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

        public ReportDailySalesChecking() //SalaryReportMode reportMode
        {
            InitializeComponent();
            //if (reportMode == SalaryReportMode.Checking)
            //{
            //    _includeSystemUnitsColumn = true;
            //    hdrSalaryReport.Text = "Salary Report - Checking";
            //    hdrLine.Width = 245;
            //}
            //else
            //{
            //    _includeSystemUnitsColumn = false;
            //    hdrSalaryReport.Text = "Salary Report";
            //    hdrLine.Width = 135;
            //}

            //hdrSalaryReport.Text = _reportDescription;
            //cmbSalaryReportType.SelectedIndex = 0;
            cmbReportMode.SelectedIndex = 0;//perm agents as default
            LoadLookupValues();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

        private void LoadLookupValues()
        {
            try
            {
                SetCursor(Cursors.Wait);

                _dsLookups = Methods.ExecuteStoredProcedure("spINGetDailySalesCheckingReportLookups", null);
                cmbReportType.Populate(_dsLookups.Tables[0], "Description", "ID");

                //cmbSalaryReportType.SelectedIndex = 1;

                if (cmbReportType.Text.Trim() != "")
                {
                    switch (cmbReportType.Text)
                    {
                        case "Base, Rejuvenation, Defrost and Funeral":
                            _salaryReportType = SalaryReportType.Base;
                            tbBonusSales.Visibility = Visibility.Hidden;
                            chkBonusSales.Visibility = Visibility.Hidden;
                            chkBonusSales.IsChecked = false;
                            _bonusSales = false;
                            break;

                        case "Upgrades":
                            _salaryReportType = SalaryReportType.Upgrade;
                            tbBonusSales.Visibility = Visibility.Visible;
                            chkBonusSales.Visibility = Visibility.Visible;
                            chkBonusSales.IsChecked = false;
                            _bonusSales = false;
                            break;
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

        private void PopulateDataGrids(DataRow drSelectedSalaryReportType)
        {

            string filterString = drSelectedSalaryReportType["FilterString"].ToString();
            string orderByStringCampaigns = drSelectedSalaryReportType["OrderByStringCampaigns"].ToString();
            string orderByStringCampaignClusters = drSelectedSalaryReportType["OrderByStringCampaignClusters"].ToString();

            #region Loading the campaigns

            var filteredCampaigns = _dsLookups.Tables[1].Select(filterString).AsEnumerable();
            if (filteredCampaigns.Any())
            {
                DataTable dtCampaigns = _dsLookups.Tables[1].Select(filterString, orderByStringCampaigns).CopyToDataTable();
                xdgCampaigns.DataSource = dtCampaigns.DefaultView;
            }

            var campaignsHeaderCheckbox = Methods.FindChild<CheckBox>(xdgCampaigns, "CampaignsRecordSelectorCheckbox");
            if (campaignsHeaderCheckbox != null)
            {
                campaignsHeaderCheckbox.IsChecked = false;
            }

            #endregion Loading the campaigns

            #region Loading the campaign clusters

            var filteredCampaignsClusters = _dsLookups.Tables[2].Select(filterString).AsEnumerable();
            if (filteredCampaignsClusters.Any())
            {
                DataTable dtCampaignClusters = _dsLookups.Tables[2].Select(filterString, orderByStringCampaignClusters).CopyToDataTable();
                xdgCampaignClusters.DataSource = dtCampaignClusters.DefaultView;
            }

            var campaignClustersHeaderCheckbox = Methods.FindChild<CheckBox>(xdgCampaignClusters, "CampaignClustersHeaderPrefixAreaCheckbox");
            if (campaignClustersHeaderCheckbox != null)
            {
                campaignClustersHeaderCheckbox.IsChecked = false;
            }

            #endregion Loading the campaign clusters  
        }

        private bool IsAllInputParametersSpecifiedAndValid()
        {
            #region Ensuring that the salary report type was selected

            //if (cmbSalaryReportType.Text.Trim() == String.Empty)
            if (cmbReportType.SelectedValue == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'Salary Report Type'.", @"'Salary Report Type' not specified", ShowMessageType.Error);
                return false;
            }

            if (cmbReportType.Text == "Upgrades")
            {
                _reportType = 2;
            }
            else
            {
                _reportMode = 1;
            };

            #endregion Ensuring that the salary report type was selected

            #region Ensuring that the From Date was specified

            //_fromDate = calFromDate.SelectedDate; //.Value.ToString("yyyy-MM-dd");

            if (calFromDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'From Date'.", @"No 'From Date' specified", ShowMessageType.Error);
                return false;
            }
            else
            {
                _fromDate = calFromDate.SelectedDate.Value; //.Value.ToString("yyyy-MM-dd");
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the To Date was specified

            //_reportEndDate = calToDate.SelectedDate;

            if (calToDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'To Date'.", @"No 'To Date' specified", ShowMessageType.Error);
                return false;
            }
            else
            {
                _toDate = calToDate.SelectedDate.Value;
            }

            #endregion Ensuring that the To Date was specified

            #region Ensuring that the date range is valid

            if (_fromDate > _toDate)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Invalid date range specified: The 'From Date' can not be greater than the 'To Date'.", "Invalid date range", ShowMessageType.Error);
                return false;
            }

            #endregion Ensuring that the date range is valid

            if (_useCampaignClusters)
            {
                #region Ensuring that at least one campaign cluster was selected

                var lstTemp = (from r in xdgCampaignClusters.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                _lstSelectedCampaignClusters = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Name"].Value));

                if (_lstSelectedCampaignClusters.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign cluster from the list.", "No campaign clusters selected", ShowMessageType.Error);
                    return false;
                }
                else
                {
                    _fkINCampaignFKINCampaignClusterIDs = _lstSelectedCampaignClusters.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["INCampaignClusterID"].Value + ",");
                    _fkINCampaignFKINCampaignClusterIDs = _fkINCampaignFKINCampaignClusterIDs.Substring(0, _fkINCampaignFKINCampaignClusterIDs.Length - 1);
                }

                #endregion Ensuring that at least one campaign cluster was selected
            }

            else
            {
                #region Ensuring that at least one campaign was selected

                var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

                _fkINCampaignFKINCampaignClusterIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                _fkINCampaignFKINCampaignClusterIDs = _fkINCampaignFKINCampaignClusterIDs.Substring(0, _fkINCampaignFKINCampaignClusterIDs.Length - 1);

                if (_lstSelectedCampaigns.Count == 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No campaigns selected", ShowMessageType.Error);
                    return false;
                }
                else
                {
                    _fkINCampaignFKINCampaignClusterIDs = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                    _fkINCampaignFKINCampaignClusterIDs = _fkINCampaignFKINCampaignClusterIDs.Substring(0, _fkINCampaignFKINCampaignClusterIDs.Length - 1);
                }

                #endregion Ensuring that at least one campaign was selected
            }

            if (chkBonusSales.IsChecked.HasValue)
            {
                _bonusSales = chkBonusSales.IsChecked.Value;
            }
            else
            {
                _bonusSales = false;
            }

            // Otherwise if all is well, proceed:
            return true;
        }

        #region OLD

        //private void AddReportPageOLD(Workbook wbResultingWorkbook, Workbook wbTemplate, string reportTemplateSheetName, string newWorksheetDescription, DataTable dataTable)
        //{

        //    string excelFormulaColumn;
        //    int formulaStartingRowIndex;

        //    if (dataTable.Rows.Count > 0)
        //    {

        //        #region Adding a new sheet for the current campaign / campaign cluster

        //        Worksheet wsSalaryReportTemplate;

        //        if (_salaryReportType == SalaryReportType.Base)
        //        {
        //            wsSalaryReportTemplate = wbTemplate.Worksheets["Base"];
        //        }
        //        else
        //        {
        //            //wsSalaryReportTemplate = wbTemplate.Worksheets["Upgrades"];

        //            //_templateSheetName is set in the constructor
        //            wsSalaryReportTemplate = wbTemplate.Worksheets[_templateSheetName];
        //        }

        //        Worksheet wsSalaryReport = wbResultingWorkbook.Worksheets.Add(newWorksheetDescription);

        //        //worksheetCount++;

        //        int reportTemplateRowIndex;
        //        int reportRow;
        //        int templateColumnSpan = dataTable.Columns.Count;

        //        //if (_includeSystemUnitsColumn)
        //        //{
        //        //    templateColumnSpan = 16;
        //        //}
        //        //else
        //        //{
        //        //    templateColumnSpan = 15;
        //        //}

        //        #endregion Adding a new sheet for the current campaign / campaign cluster

        //        #region Setup worksheet

        //        #region Copy the template formatting, based on the type of salary report and set the report row start index

        //        //if (_salaryReportType == SalaryReportType.Base)
        //        //{
        //        Methods.CopyExcelRegion(wsSalaryReportTemplate, 0, 0, 8, templateColumnSpan, wsSalaryReport, 0, 0);
        //        //}

        //        reportRow = 2;
        //        reportTemplateRowIndex = 9;
        //        formulaStartingRowIndex = reportTemplateRowIndex;

        //        #endregion Copy the template formatting, based on the type of salary report and set the report row start index

        //        #endregion Setup worksheet

        //        #region Populate the report details

        //        WorksheetCell wcSalaryReportCell = wsSalaryReport.Rows[reportRow].Cells[0];
        //        if (_bonusSales)
        //        {
        //            wcSalaryReportCell.Value = String.Format("{0} - Bonus Leads", newWorksheetDescription);
        //        }
        //        else
        //        {
        //            wcSalaryReportCell.Value = newWorksheetDescription;
        //        }

        //        //wcSalaryReportCell.Value = newWorksheetDescription;
        //        reportRow += 2;

        //        wcSalaryReportCell = wsSalaryReport.Rows[reportRow].Cells[0];
        //        wcSalaryReportCell.Value = String.Format("From {0} to {1}", _reportStartDate.Value.ToShortDateString(), _reportEndDate.Value.ToShortDateString());
        //        reportRow += 2;

        //        wcSalaryReportCell = wsSalaryReport.Rows[reportRow].Cells[0];
        //        wcSalaryReportCell.Value = String.Format("Date Generated: {0}", _strTodaysDateIncludingColons);
        //        reportRow += 3;

        //        #endregion Populate the report details

        //        #region Loop through each row in the data table

        //        foreach (DataRow row in dataTable.Rows)
        //        {
        //            //Methods.CopyExcelRegion(wsSalaryReportTemplate, reportTemplateRowIndex, 0, 0, dataTable.Columns.Count, wsSalaryReport, reportRow, 0);
        //            Methods.CopyExcelRegion(wsSalaryReportTemplate, reportTemplateRowIndex, 0, 0, templateColumnSpan, wsSalaryReport, reportRow, 0);

        //            for (int b = 0; b < dataTable.Columns.Count; b++)
        //            {
        //                wcSalaryReportCell = wsSalaryReport.Rows[reportRow].Cells[b];
        //                wcSalaryReportCell.Value = row[b];
        //            }
        //            reportRow++;
        //        }

        //        #endregion Loop through each row in the data table

        //        #region Adding the "Totals" row

        //        reportTemplateRowIndex++;

        //        //Methods.CopyExcelRegion(wsSalaryReportTemplate, reportTemplateRowIndex, 0, 0, dataTable.Columns.Count, wsSalaryReport, reportRow, 0);
        //        Methods.CopyExcelRegion(wsSalaryReportTemplate, reportTemplateRowIndex, 0, 0, templateColumnSpan, wsSalaryReport, reportRow, 0);

        //        for (int b = 1; b < dataTable.Columns.Count; b++)
        //        {
        //            excelFormulaColumn = ExcelUtils.GetExcelColumnNameFromColumnIndex(b + 1);
        //            wcSalaryReportCell = wsSalaryReport.Rows[reportRow].Cells[b];

        //            if (b == 14)
        //            {
        //                wcSalaryReportCell.ApplyFormula(String.Format("=AVERAGE({0}{1}:{0}{2})", excelFormulaColumn, formulaStartingRowIndex + 1, reportRow));
        //            }
        //            else
        //            {
        //            wcSalaryReportCell.ApplyFormula(String.Format("=SUM({0}{1}:{0}{2})", excelFormulaColumn, formulaStartingRowIndex + 1, reportRow));
        //        }
        //        }
        //        wsSalaryReportTemplate.DisplayOptions.ShowFormulasInCells = false;

        //        #endregion
        //    }
        //}

        //private void ReportOLD(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        SetCursor(Cursors.Wait);

        //        #region Setup Excel document

        //        var wbSalaryReport = new Workbook();
        //        wbSalaryReport.SetCurrentFormat(WorkbookFormat.Excel2007);

        //        string filePathAndName = String.Format("{0}{1} ({2}) {3} To {4} ~ {5}.xlsx",
        //            GlobalSettings.UserFolder,
        //            //_campaignCluster.ItemArray[3],
        //            _reportDescription,
        //            _salaryReportType,
        //            _reportStartDate.Value.ToString("yyyy-MM-dd"),
        //            _reportEndDate.Value.ToString("yyyy-MM-dd"),
        //            DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

        //        Workbook wbTemplate;
        //        Uri uri = new Uri("/Templates/ReportTemplateSalary.xlsx", UriKind.Relative);
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

        //        if (_useCampaignClusters)
        //        {
        //            #region Add a summary sheet in the event that more than 1 campaign cluster was selected

        //            if (_lstSelectedCampaignClusters.Count > 1)
        //            {
        //                DataTable dtSummaryPageData = Insure.INGetSalaryReportDataForMultipleCampaignClusters(_includeSystemUnitsColumn, _campaignClusterIDList, (byte)_salaryReportType, _reportStartDate.Value, _reportEndDate.Value, _bonusSales);

        //                AddReportPage(wbSalaryReport, wbTemplate, _salaryReportType.ToString(), "Summary", dtSummaryPageData);
        //            }

        //            #endregion Add a summary sheet in the event that more than 1 campaign cluster was selected

        //            #region Add a separate worksheet for each individual campaign cluster

        //            foreach (DataRecord record in _lstSelectedCampaignClusters)
        //            {
        //                if ((bool)record.Cells["Select"].Value)
        //                {
        //                    long campaignClusterID = Convert.ToInt32(record.Cells["INCampaignClusterID"].Value);
        //                    string campaignClusterName = Methods.ParseWorksheetName(wbSalaryReport, record.Cells["Name"].Value.ToString());

        //                    DataTable dtCampaignClusterData = Insure.INGetSalaryReportDataForSingleCampaignCluster(_includeSystemUnitsColumn, campaignClusterID, (byte)_salaryReportType, _reportStartDate.Value, _reportEndDate.Value, _bonusSales);
        //                    if (dtCampaignClusterData.Rows.Count > 0)
        //                    {
        //                        AddReportPage(wbSalaryReport, wbTemplate, _salaryReportType.ToString(), campaignClusterName, dtCampaignClusterData);
        //                    }
        //                }
        //            }

        //            #endregion Add a separate worksheet for each individual campaign cluster
        //        }
        //        else
        //        {
        //            #region Add a summary sheet in the event that more than 1 campaign was selected

        //            if (_lstSelectedCampaigns.Count > 1)
        //            {
        //                DataTable dtSummaryPageData = Insure.INGetSalaryReportDataForMultipleCampaigns(_includeSystemUnitsColumn, _campaignIDList, (byte)_salaryReportType, _reportStartDate.Value, _reportEndDate.Value, _bonusSales);
        //                AddReportPage(wbSalaryReport, wbTemplate, _salaryReportType.ToString(), "Summary", dtSummaryPageData);
        //            }

        //            #endregion Add a summary sheet in the event that more than 1 campaign was selected

        //            #region Add a separate worksheet for each individual campaign

        //            foreach (DataRecord record in _lstSelectedCampaigns)
        //            {
        //                if ((bool)record.Cells["Select"].Value)
        //                {
        //                    long campaignID = Convert.ToInt32(record.Cells["CampaignID"].Value);
        //                    string campaignName = Methods.ParseWorksheetName(wbSalaryReport, record.Cells["CampaignName"].Value.ToString());

        //                    DataTable dtCampaignData = Insure.INGetSalaryReportDataForSingleCampaign(_includeSystemUnitsColumn, campaignID, (byte)_salaryReportType, _reportStartDate.Value, _reportEndDate.Value, _bonusSales);
        //                    if (dtCampaignData.Rows.Count > 0)
        //                    {
        //                        AddReportPage(wbSalaryReport, wbTemplate, _salaryReportType.ToString(), campaignName, dtCampaignData);
        //                    }
        //                }
        //            }

        //            #endregion Add a separate worksheet for each individual campaign
        //        }

        //        #region Saves and opens the resulting Excel workbook - if there are any pages in the workbook

        //        if (wbSalaryReport.Worksheets.Count > 0)
        //        {
        //            wbSalaryReport.Save(filePathAndName);

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

        private void AddReportPage(Workbook wbTemplate, Workbook wbReport, DataSet dsSalaryReportData, DataRow drCurrentCampaign, DataRow drReportConfigs)
        {
            #region Determining the data table that contains the current sheet's data - and exit this method if there are 0 rows

            string filterString = drCurrentCampaign["FilterString"].ToString();
            byte sourceDataTableIndex = Convert.ToByte(drCurrentCampaign["SourceDataTableIndex"]);

            DataTable dtCurrentCampaignSalaryReportData;

            if (String.IsNullOrEmpty(filterString))
            {
                dtCurrentCampaignSalaryReportData = dsSalaryReportData.Tables[sourceDataTableIndex];
            }

            else
            {
                var filteredRows = dsSalaryReportData.Tables[sourceDataTableIndex].Select(filterString).AsEnumerable();
                if (!filteredRows.Any())
                {
                    return;
                }

                dtCurrentCampaignSalaryReportData = dsSalaryReportData.Tables[sourceDataTableIndex].Select(filterString).CopyToDataTable();
            }

            #endregion Determining the data table that contains the current sheet's data - and exit this method if there are 0 rows

            #region Partition the given dataset

            string orderByString = drCurrentCampaign["OrderByString"].ToString();

            DataTable dtExcelSheetDataTableColumnMappings = dsSalaryReportData.Tables[4];
            DataTable dtTotalsMappings = dsSalaryReportData.Tables[5];

            #endregion Partition the given dataset

            #region Declarations & Initializations

            int reportRow = 9;
            int formulaStartRow = reportRow;

            string reportTemplateSheetName = drReportConfigs["ReportTemplateSheetName"].ToString();
            byte reportTemplateRowSpan = Convert.ToByte(drReportConfigs["ReportTemplateRowSpan"]);
            byte reportTemplateColumnSpan = Convert.ToByte(drReportConfigs["ReportTemplateColumnSpan"]);
            byte reportTemplateDataRowIndex = Convert.ToByte(drReportConfigs["ReportTemplateDataRowIndex"]);
            byte reportTemplateTotalsRowIndex = Convert.ToByte(drReportConfigs["ReportTemplateTotalsRowIndex"]);

            string reportSubTitle = String.Empty;
            string worksheetTabName = String.Empty;
            DateTime dateOfSale = DateTime.Now;

            if (sourceDataTableIndex != 7)
            {
                string campaignOrCampaignClusterColumnName = drReportConfigs["CampaignOrCampaignClusterColumnName"].ToString();
                string campaignOrCampaignClusterCodeColumnName = drReportConfigs["CampaignOrCampaignClusterCodeColumnName"].ToString();

                reportSubTitle = drCurrentCampaign[campaignOrCampaignClusterColumnName].ToString();
                worksheetTabName = Methods.ParseWorksheetName(wbReport, drCurrentCampaign[campaignOrCampaignClusterCodeColumnName].ToString());
            }
            else
            {
                //string campaignOrCampaignClusterColumnName = drReportConfigs["CampaignOrCampaignClusterColumnName"].ToString();
                //string campaignOrCampaignClusterCodeColumnName = drReportConfigs["CampaignOrCampaignClusterCodeColumnName"].ToString();

                if (!(drCurrentCampaign["DateOfSale"] as DateTime?).HasValue)
                {
                    return;
                }

                dateOfSale = (DateTime)drCurrentCampaign["DateOfSale"];


                reportSubTitle = dateOfSale.ToShortDateString();

                worksheetTabName = Methods.ParseWorksheetName(wbReport, dateOfSale.ToShortDateString().Replace('/', '-'));
            }


            #endregion Declarations & Initializations

            #region Adding a new sheet for the current campaign / campaign cluster

            Worksheet wsSalaryReportTemplate = wbTemplate.Worksheets[reportTemplateSheetName];
            Worksheet wsSalaryReport = wbReport.Worksheets.Add(worksheetTabName);

            #endregion Adding a new sheet for the current campaign / campaign cluster

            #region Populating the report details

            Methods.CopyExcelRegion(wsSalaryReportTemplate, 0, 0, reportTemplateRowSpan, reportTemplateColumnSpan, wsSalaryReport, 0, 0);

            if (_bonusSales)
            {
                reportSubTitle = String.Format("{0} - Bonus Leads", reportSubTitle);
            }

            wsSalaryReport.GetCell("A3").Value = reportSubTitle;
            if (sourceDataTableIndex != 7)
            {
                wsSalaryReport.GetCell("A5").Value = String.Format("For the period between {0} to {1}", _fromDate.ToString("dddd, d MMMM yyyy"), _toDate.ToString("dddd, d MMMM yyyy"));
            }
            else
            {
                wsSalaryReport.GetCell("A5").Value = String.Format("For all selected campaigns for {0}", dateOfSale.ToString("dddd, d MMMM yyyy"));
            }

            wsSalaryReport.GetCell("A7").Value = String.Format("Date Generated: {0}", DateTime.Now.ToString("yyyy -MM-dd HH:mm:ss"));

            #endregion Populating the report details

            #region Add the data

            reportRow = Methods.MapTemplatizedExcelValues(wsSalaryReportTemplate, dtCurrentCampaignSalaryReportData, dtExcelSheetDataTableColumnMappings, reportTemplateDataRowIndex, 0, 0, reportTemplateColumnSpan, wsSalaryReport, reportRow, 0);

            #endregion Add the data

            #region Step 4: Add the totals / averages

            reportRow = Methods.MapTemplatizedExcelFormulas(wsSalaryReportTemplate, dtTotalsMappings, reportTemplateTotalsRowIndex, 0, 0, reportTemplateColumnSpan, wsSalaryReport, reportRow, 0, formulaStartRow, reportRow - 1);

            #endregion Step 4: Add the totals / averages

        }

        private void ReportOld(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Get the report data - and exit the method if there is no data available

                //DataSet dsSalaryReportData = Insure.INReportSalaryGeneric(_includeSystemUnitsColumn, _fkINCampaignFKINCampaignClusterIDs, _reportType, _fromDate, _toDate, _bonusSales, _useCampaignClusters);

                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@IncludeSystemUnits", _includeSystemUnitsColumn);
                parameters[1] = new SqlParameter("@FKINCampaignFKINCampaignClusterIDs", _fkINCampaignFKINCampaignClusterIDs);
                parameters[2] = new SqlParameter("@ReportType", _reportType);
                parameters[3] = new SqlParameter("@FromDate", _fromDate);
                parameters[4] = new SqlParameter("@ToDate", _toDate);
                parameters[5] = new SqlParameter("@BonusSales", _bonusSales);
                parameters[6] = new SqlParameter("@UseCampaignClusters", _useCampaignClusters);
                DataSet dsReportData = Methods.ExecuteStoredProcedure("spINReportDailySalesChecking", parameters);

                if (dsReportData.Tables[3].Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "There is no data available for the criteria you have specified. Please change some of them and try again.", "No data available", ShowMessageType.Information);
                    });

                    return;
                }

                DataRow drReportConfigs = dsReportData.Tables[0].Rows[0]; // Specify it only once here

                #endregion Get the report data - and exit the method if there is no data available

                #region Setup Excel document

                string reportDescription = dsReportData.Tables[0].Rows[0]["ReportDescription"].ToString();

                string filePathAndName = String.Format("{0}{1} ({2}) {3:yyyy-MM-dd} to {4:yyyy-MM-dd} ~ {5:yyyy-MM-dd HHmmss}.xlsx",
                    GlobalSettings.UserFolder,
                    reportDescription,
                    drReportConfigs["ReportTemplateSheetName"], _fromDate, _toDate, DateTime.Now);

                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateDailySalesChecking.xlsx");
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup Excel document

                #region For each row in dsSalaryReportData[1], generate an individual worksheet - if there is any data for that particular sheet

                foreach (DataRow row in dsReportData.Tables[1].Rows)
                {
                    AddReportPage(wbTemplate, wbReport, dsReportData, row, drReportConfigs);
                }

                //foreach (DataRow row in dsReportData.Tables[6].Rows)
                //{
                //    AddReportPage(wbTemplate, wbReport, dsReportData, row, drReportConfigs);
                //}

                #endregion For each row in dsSalaryReportData[1], generate an individual worksheet - if there is any data for that particular sheet

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

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup Excel Document

                int worksheetCount = 0;

                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                string filePathAndName = String.Format("{0}Daily Sales Checking Report ({1:yyyy-MM-dd} - {2:yyyy-MM-dd})  ~ {3}.xlsx", GlobalSettings.UserFolder, _fromDate, _toDate, DateTime.Now.Millisecond);

                Uri uri = new Uri("/Templates/ReportTemplateDailySalesChecking.xlsx", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    wbTemplate = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }

                Worksheet wsTemplate = wbTemplate.Worksheets["Template1"];
                Worksheet wsReport;

                #endregion


                #region Get Report Data

                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@CampaignClusterIDs", _fkINCampaignFKINCampaignClusterIDs);
                parameters[1] = new SqlParameter("@ReportType", _reportType);
                parameters[2] = new SqlParameter("@FromDate", _fromDate);
                parameters[3] = new SqlParameter("@ToDate", _toDate);
                DataSet dsReportData = Methods.ExecuteStoredProcedure("spINReportDailySalesChecking", parameters);

                DataTable dtSales = dsReportData.Tables[0];
                DataTable dtReportData = dsReportData.Tables[1];
                DataTable dtReportDataTotals = dsReportData.Tables[2];
                DataTable dtSummaryData = dsReportData.Tables[3];
                DataTable dtSummaryDataTotals = dsReportData.Tables[4];
                DataTable dtSavedCarriedForwards = dsReportData.Tables[5];

                if (dsReportData.Tables[0].Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "Please change some of the criteria and try again.", "No data available", ShowMessageType.Information);
                    });

                    return;
                }

                #endregion


                #region Report

                int rowIndexReportStart = 10;
                int rowIndexData = rowIndexReportStart;

                long[] campaignClusterIDs = _fkINCampaignFKINCampaignClusterIDs.Split(',').Select(long.Parse).ToArray();

                #region Summary

                wsReport = wbReport.Worksheets.Add("Summary");
                worksheetCount++;

                string strSummaryTitle = "Summary (";
                foreach (long campaignClusterID in campaignClusterIDs)
                {
                    DataRow[] drsReportData = dtReportData.Select("CampaignClusterID = " + campaignClusterID);

                    if (drsReportData.Length > 0)
                    {
                        strSummaryTitle = strSummaryTitle + Convert.ToString(drsReportData[0]["CampaignClusterCode"]);
                        strSummaryTitle = strSummaryTitle + ",";
                    }


                }
                strSummaryTitle = strSummaryTitle.Substring(0, strSummaryTitle.Length - 1) + ")";

                Methods.CopyExcelRegion(wsTemplate, 0, 0, 8, 16, wsReport, 0, 0);
                wsReport.GetCell("A3").Value = strSummaryTitle;
                wsReport.GetCell("A5").Value = String.Format("From {0:dddd, d MMMM yyyy} to {1:dddd, d MMMM yyyy}", _fromDate, _toDate);
                wsReport.GetCell("A7").Value = String.Format("Date Generated: {0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

                #region Summary Data



                foreach (DataRow drSummaryData in dtSummaryData.Rows)
                {
                    Methods.CopyExcelRegion(wsTemplate, 9, 0, 0, 17, wsReport, rowIndexData - 1, 0);

                    wsReport.GetCell("A" + rowIndexData).Value = drSummaryData["AgentName"].ToString();
                    wsReport.GetCell("B" + rowIndexData).Value = Convert.ToInt32(drSummaryData["TotalSystemPolicies"].ToString());
                    wsReport.GetCell("C" + rowIndexData).Value = Convert.ToDecimal(drSummaryData["TotalSystemUnits"].ToString());
                    wsReport.GetCell("D" + rowIndexData).Value = Convert.ToInt32(drSummaryData["TotalSavedCarriedForward"].ToString());
                    wsReport.GetCell("E" + rowIndexData).Value = Convert.ToDecimal(drSummaryData["TotalSavedCarriedForwardUnits"].ToString());
                    wsReport.GetCell("F" + rowIndexData).Value = Convert.ToInt32(drSummaryData["TotalActualPolicies"].ToString());
                    wsReport.GetCell("G" + rowIndexData).Value = Convert.ToDecimal(drSummaryData["TotalActualUnits"].ToString());
                    wsReport.GetCell("H" + rowIndexData).Value = Convert.ToDecimal(drSummaryData["TotalActualChildren"].ToString());
                    wsReport.GetCell("I" + rowIndexData).Value = Convert.ToInt32(drSummaryData["TotalBumpedUpPolicies"].ToString());
                    wsReport.GetCell("J" + rowIndexData).Value = Convert.ToDecimal(drSummaryData["TotalBumpedUpUnits"].ToString());
                    wsReport.GetCell("K" + rowIndexData).Value = Convert.ToInt32(drSummaryData["TotalReducedPolicies"].ToString());
                    wsReport.GetCell("L" + rowIndexData).Value = Convert.ToDecimal(drSummaryData["TotalReducedUnits"].ToString());
                    wsReport.GetCell("M" + rowIndexData).Value = Convert.ToInt32(drSummaryData["TotalCancelledPolicies"].ToString());
                    wsReport.GetCell("N" + rowIndexData).Value = Convert.ToDecimal(drSummaryData["TotalCancelledUnits"].ToString());
                    wsReport.GetCell("O" + rowIndexData).Value = Convert.ToInt32(drSummaryData["TotalCarriedForwardPolicies"].ToString());
                    wsReport.GetCell("P" + rowIndexData).Value = Convert.ToDecimal(drSummaryData["TotalCarriedForwardUnits"].ToString());
                    wsReport.GetCell("Q" + rowIndexData).Value = Convert.ToInt32(drSummaryData["TotalGiftsRedeemed"].ToString());

                    rowIndexData++;
                }

                #endregion

                #region Summary Data Totals

                rowIndexData--;
                Methods.CopyExcelRegion(wsTemplate, 11, 0, 1, 17, wsReport, rowIndexData, 0);

                DataRow drSummaryDataTotals = dtSummaryDataTotals.Rows[0];
                rowIndexData++;

                wsReport.GetCell("B" + rowIndexData).Value = Convert.ToInt32(drSummaryDataTotals["TotalTotalSystemPolicies"].ToString());
                wsReport.GetCell("C" + rowIndexData).Value = Convert.ToDecimal(drSummaryDataTotals["TotalTotalSystemUnits"].ToString());
                wsReport.GetCell("D" + rowIndexData).Value = Convert.ToInt32(drSummaryDataTotals["TotalTotalSavedCarriedForward"].ToString());
                wsReport.GetCell("E" + rowIndexData).Value = Convert.ToDecimal(drSummaryDataTotals["TotalTotalSavedCarriedForwardUnits"].ToString());
                wsReport.GetCell("F" + rowIndexData).Value = Convert.ToInt32(drSummaryDataTotals["TotalTotalActualPolicies"].ToString());
                wsReport.GetCell("G" + rowIndexData).Value = Convert.ToDecimal(drSummaryDataTotals["TotalTotalActualUnits"].ToString());
                wsReport.GetCell("H" + rowIndexData).Value = Convert.ToDecimal(drSummaryDataTotals["TotalActualChildren"].ToString());
                wsReport.GetCell("I" + rowIndexData).Value = Convert.ToInt32(drSummaryDataTotals["TotalTotalBumpedUpPolicies"].ToString());
                wsReport.GetCell("J" + rowIndexData).Value = Convert.ToDecimal(drSummaryDataTotals["TotalTotalBumpedUpUnits"].ToString());
                wsReport.GetCell("K" + rowIndexData).Value = Convert.ToInt32(drSummaryDataTotals["TotalTotalReducedPolicies"].ToString());
                wsReport.GetCell("L" + rowIndexData).Value = Convert.ToDecimal(drSummaryDataTotals["TotalTotalReducedUnits"].ToString());
                wsReport.GetCell("M" + rowIndexData).Value = Convert.ToInt32(drSummaryDataTotals["TotalTotalCancelledPolicies"].ToString());
                wsReport.GetCell("N" + rowIndexData).Value = Convert.ToDecimal(drSummaryDataTotals["TotalTotalCancelledUnits"].ToString());
                wsReport.GetCell("O" + rowIndexData).Value = Convert.ToInt32(drSummaryDataTotals["TotalTotalCarriedForwardPolicies"].ToString());
                wsReport.GetCell("P" + rowIndexData).Value = Convert.ToDecimal(drSummaryDataTotals["TotalTotalCarriedForwardUnits"].ToString());
                wsReport.GetCell("Q" + rowIndexData).Value = Convert.ToInt32(drSummaryDataTotals["TotalTotalGiftsRedeemed"].ToString());

                #endregion

                #endregion

                #region Campaigns

                List<string> campaignCodes = dtReportData.AsEnumerable().Select(c => c.Field<string>("CampaignClusterCode")).Distinct().ToList();

                //foreach (long campaignClusterID in campaignClusterIDs)                
                foreach (string campaign in campaignCodes)
                {
                    rowIndexReportStart = 10;
                    rowIndexData = rowIndexReportStart;

                    //DataRow[] drsReport = dtReportData.Select("CampaignClusterID = " + campaignClusterID);
                    DataTable dtReport = dtReportData.Select("CampaignClusterCode = '" + campaign + "'").CopyToDataTable();
                    if (dtReport == null || dtReport.Rows.Count <= 0)
                    {
                        continue;
                    }
                    //DataTable dtReport = dtReportData.Select("CampaignClusterID = " + campaignClusterID).CopyToDataTable();

                    //string campaignClusterCode = Convert.ToString(dtReport.Rows[0]["CampaignClusterCode"]);

                    wsReport = wbReport.Worksheets.Add(campaign);
                    worksheetCount++;
                    Methods.CopyExcelRegion(wsTemplate, 0, 0, 8, 16, wsReport, 0, 0);
                    wsReport.GetCell("A3").Value = campaign;
                    wsReport.GetCell("A5").Value = String.Format("From {0:dddd, d MMMM yyyy} to {1:dddd, d MMMM yyyy}", _fromDate, _toDate);
                    wsReport.GetCell("A7").Value = String.Format("Date Generated: {0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

                    #region Data

                    foreach (DataRow drReport in dtReport.Rows)
                    {
                        Methods.CopyExcelRegion(wsTemplate, 9, 0, 0, 17, wsReport, rowIndexData - 1, 0);

                        wsReport.GetCell("A" + rowIndexData).Value = drReport["AgentName"].ToString();
                        wsReport.GetCell("B" + rowIndexData).Value = Convert.ToInt32(drReport["TotalSystemPolicies"].ToString());
                        wsReport.GetCell("C" + rowIndexData).Value = Convert.ToDecimal(drReport["TotalSystemUnits"].ToString());
                        wsReport.GetCell("D" + rowIndexData).Value = Convert.ToInt32(drReport["TotalSavedCarriedForward"].ToString());
                        wsReport.GetCell("E" + rowIndexData).Value = Convert.ToDecimal(drReport["TotalSavedCarriedForwardUnits"].ToString());
                        wsReport.GetCell("F" + rowIndexData).Value = Convert.ToInt32(drReport["TotalActualPolicies"].ToString());
                        wsReport.GetCell("G" + rowIndexData).Value = Convert.ToDecimal(drReport["TotalActualUnits"].ToString());
                        wsReport.GetCell("H" + rowIndexData).Value = Convert.ToDecimal(drReport["TotalActualChildren"].ToString());
                        wsReport.GetCell("I" + rowIndexData).Value = Convert.ToInt32(drReport["TotalBumpedUpPolicies"].ToString());
                        wsReport.GetCell("J" + rowIndexData).Value = Convert.ToDecimal(drReport["TotalBumpedUpUnits"].ToString());
                        wsReport.GetCell("K" + rowIndexData).Value = Convert.ToInt32(drReport["TotalReducedPolicies"].ToString());
                        wsReport.GetCell("L" + rowIndexData).Value = Convert.ToDecimal(drReport["TotalReducedUnits"].ToString());
                        wsReport.GetCell("M" + rowIndexData).Value = Convert.ToInt32(drReport["TotalCancelledPolicies"].ToString());
                        wsReport.GetCell("N" + rowIndexData).Value = Convert.ToDecimal(drReport["TotalCancelledUnits"].ToString());
                        wsReport.GetCell("O" + rowIndexData).Value = Convert.ToInt32(drReport["TotalCarriedForwardPolicies"].ToString());
                        wsReport.GetCell("P" + rowIndexData).Value = Convert.ToDecimal(drReport["TotalCarriedForwardUnits"].ToString());
                        wsReport.GetCell("Q" + rowIndexData).Value = Convert.ToInt32(drReport["TotalGiftsRedeemed"].ToString());

                        rowIndexData++;
                    }

                    #endregion

                    #region Totals

                    rowIndexData--;
                    Methods.CopyExcelRegion(wsTemplate, 11, 0, 1, 17, wsReport, rowIndexData, 0);

                    //DataRow drReportDataTotals = dtReportDataTotals.Select("CampaignClusterID = " + campaignClusterID)[0];
                    DataRow drReportDataTotals = dtReportDataTotals.Select("CampaignClusterCode = '" + campaign + "'")[0];
                    rowIndexData++;

                    wsReport.GetCell("B" + rowIndexData).Value = Convert.ToInt32(drReportDataTotals["TotalTotalSystemPolicies"].ToString());
                    wsReport.GetCell("C" + rowIndexData).Value = Convert.ToDecimal(drReportDataTotals["TotalTotalSystemUnits"].ToString());
                    wsReport.GetCell("D" + rowIndexData).Value = Convert.ToInt32(drReportDataTotals["TotalTotalSavedCarriedForward"].ToString());
                    wsReport.GetCell("E" + rowIndexData).Value = Convert.ToDecimal(drReportDataTotals["TotalTotalSavedCarriedForwardUnits"].ToString());
                    wsReport.GetCell("F" + rowIndexData).Value = Convert.ToInt32(drReportDataTotals["TotalTotalActualPolicies"].ToString());
                    wsReport.GetCell("G" + rowIndexData).Value = Convert.ToDecimal(drReportDataTotals["TotalTotalActualUnits"].ToString());
                    wsReport.GetCell("H" + rowIndexData).Value = Convert.ToDecimal(drReportDataTotals["TotalActualChildren"].ToString());
                    wsReport.GetCell("I" + rowIndexData).Value = Convert.ToInt32(drReportDataTotals["TotalTotalBumpedUpPolicies"].ToString());
                    wsReport.GetCell("J" + rowIndexData).Value = Convert.ToDecimal(drReportDataTotals["TotalTotalBumpedUpUnits"].ToString());
                    wsReport.GetCell("K" + rowIndexData).Value = Convert.ToInt32(drReportDataTotals["TotalTotalReducedPolicies"].ToString());
                    wsReport.GetCell("L" + rowIndexData).Value = Convert.ToDecimal(drReportDataTotals["TotalTotalReducedUnits"].ToString());
                    wsReport.GetCell("M" + rowIndexData).Value = Convert.ToInt32(drReportDataTotals["TotalTotalCancelledPolicies"].ToString());
                    wsReport.GetCell("N" + rowIndexData).Value = Convert.ToDecimal(drReportDataTotals["TotalTotalCancelledUnits"].ToString());
                    wsReport.GetCell("O" + rowIndexData).Value = Convert.ToInt32(drReportDataTotals["TotalTotalCarriedForwardPolicies"].ToString());
                    wsReport.GetCell("P" + rowIndexData).Value = Convert.ToDecimal(drReportDataTotals["TotalTotalCarriedForwardUnits"].ToString());
                    wsReport.GetCell("Q" + rowIndexData).Value = Convert.ToInt32(drReportDataTotals["TotalTotalGiftsRedeemed"].ToString());

                    #endregion

                    #region Saved Carried Forwards

                    Methods.CopyExcelRegion(wsTemplate, 14, 0, 1, 8, wsReport, rowIndexData + 1, 0);
                    rowIndexData++;
                    rowIndexData++;
                    rowIndexData++;
                    rowIndexData++;

                    foreach (DataRow drSavedCarriedForward in dtSavedCarriedForwards.Select("CampaignClusterCode = '" + campaign + "'"))
                    {
                        Methods.CopyExcelRegion(wsTemplate, 16, 0, 0, 8, wsReport, rowIndexData - 1, 0);

                        wsReport.GetCell("A" + rowIndexData).Value = drSavedCarriedForward["AgentName"].ToString();
                        wsReport.GetCell("B" + rowIndexData).Value = drSavedCarriedForward["ClientSurname"].ToString();
                        wsReport.GetCell("D" + rowIndexData).Value = drSavedCarriedForward["RefNo"].ToString();
                        wsReport.GetCell("F" + rowIndexData).Value = Convert.ToDecimal(drSavedCarriedForward["SystemUnits"].ToString());
                        wsReport.GetCell("G" + rowIndexData).Value = Convert.ToDateTime(drSavedCarriedForward["OriginalDOS"].ToString());

                        rowIndexData++;
                    }

                    #endregion

                }

                #endregion

                #endregion


                #region Save Excel Document

                if (worksheetCount > 0)
                {
                    wbReport.Save(filePathAndName);
                    Process.Start(filePathAndName);
                }

                #endregion Save Excel Document

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
        }

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;

            btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnReport.ToolTip = btnReport.Content;
        }

        private void ExecuteReportOperations()
        {
            try
            {
                if (IsAllInputParametersSpecifiedAndValid())
                {
                    //_strTodaysDate = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
                    //_strTodaysDateIncludingColons = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

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

            cmbReportType.IsEnabled = isEnabled;
            chkGroupCampaigns.IsEnabled = isEnabled;
            xdgCampaignClusters.IsEnabled = isEnabled;
            xdgCampaigns.IsEnabled = isEnabled;

            calFromDate.IsEnabled = isEnabled;
            calToDate.IsEnabled = isEnabled;
            btnReport.IsEnabled = isEnabled;

            btnTempReport.IsEnabled = isEnabled;
        }

        #endregion Private Methods

        #region Event Handlers

        #region Button Related
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            ExecuteReportOperations();
        }

        #endregion Button Related

        #region ComboBox Related
        private void cmbSalaryReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRow drSelectedSalaryReportType = (cmbReportType.SelectedItem as DataRowView).Row;
            PopulateDataGrids(drSelectedSalaryReportType);
        }

        private void cmbSalaryReportType_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbReportType.SelectedValue != null)
            {
                //LoadLookupValues();
                //Changes... If the user selects Upgrades then disable group by textbox
                //Pheko
                //if (cmbReportType.SelectedIndex == 1)
                //{
                //    chkGroupCampaigns.Visibility = Visibility.Hidden;
                //    tbGroupCampaigns.Visibility = Visibility.Hidden;
                //}
                //else
                //{
                //    chkGroupCampaigns.Visibility = Visibility.Visible;
                //    tbGroupCampaigns.Visibility = Visibility.Visible;
                //}
            }
        }

        #endregion ComboBox Related

        #region Data Grid Related

        private bool? AllCampaignRecordsSelected()
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

        private bool? AllCampaignClusterRecordsSelected()
        {
            try
            {
                bool allSelected = true;
                bool noneSelected = true;

                if (xdgCampaigns.DataSource != null)
                {
                    foreach (DataRow dr in ((DataView)xdgCampaignClusters.DataSource).Table.Rows)
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

        private void ShowDataGrid(bool showCampaignClusters)
        {
            if (showCampaignClusters)
            {
                xdgCampaignClusters.Visibility = Visibility.Visible;
                xdgCampaigns.Visibility = Visibility.Collapsed;
            }
            else
            {
                xdgCampaignClusters.Visibility = Visibility.Collapsed;
                xdgCampaigns.Visibility = Visibility.Visible;
            }
        }

        #endregion Data Grid Related

        //private void calFromDate_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
        //{
        //    DateTime.TryParse(calFromDate.SelectedDate.ToString(), out _fromDate);
        //    DateTime.TryParse(calFromDate.SelectedDate.ToString(), out _startDate);
        //}

        //private void calToDate_SelectedDatesChanged(object sender, SelectedDatesChangedEventArgs e)
        //{
        //    DateTime.TryParse(calToDate.SelectedDate.ToString(), out _toDate);
        //    DateTime.TryParse(calToDate.SelectedDate.ToString(), out _endDate);
        //}


        private void chkGroupCampaigns_Checked(object sender, RoutedEventArgs e)
        {
            if (chkGroupCampaigns.IsChecked.HasValue)
            {
                if (chkGroupCampaigns.IsChecked.Value)
                {
                    ShowDataGrid(true);
                    _useCampaignClusters = true;
                }
                else
                {
                    ShowDataGrid(false);
                    _useCampaignClusters = false;
                }
            }
            else
            {
                ShowDataGrid(false);
                _useCampaignClusters = false;
            }
            //LoadLookupValues();
        }

        private void chkGroupCampaigns_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkGroupCampaigns.IsChecked.HasValue)
            {
                if (!chkGroupCampaigns.IsChecked.Value)
                {
                    ShowDataGrid(false);
                    _useCampaignClusters = false;
                }
                else
                {
                    ShowDataGrid(true);
                    _useCampaignClusters = true;
                }
            }
            else
            {
                ShowDataGrid(false);
                _useCampaignClusters = false;
            }
            //LoadLookupValues();
        }

        //private void chkBonusSales_Checked(object sender, RoutedEventArgs e)
        //{
        //    _bonusSales = true;
        //}

        //private void chkBonusSales_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    _bonusSales = false;
        //}

        #endregion Event Handlers

        #region Campaign Clusters Datagrid

        private void CampaignClustersHeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = ((DataView)xdgCampaignClusters.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = true;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void CampaignClustersHeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _xdgCampaignClusterssHeaderPrefixAreaCheckbox = (CheckBox)sender;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void CampaignClustersHeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = ((DataView)xdgCampaignClusters.DataSource).Table;

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

        private void CampaignClustersRecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_xdgCampaignClusterssHeaderPrefixAreaCheckbox != null)
                {
                    _xdgCampaignClusterssHeaderPrefixAreaCheckbox.IsChecked = AllCampaignClusterRecordsSelected();
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        #endregion Campaign Clusters Datagrid

        #region Campaigns Datagrid

        private void CampaignsHeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = true;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void CampaignsHeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _xdgCampaignsHeaderPrefixAreaCheckbox = (CheckBox)sender;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void CampaignsHeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
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

        private void CampaignsRecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_xdgCampaignsHeaderPrefixAreaCheckbox != null)
                {
                    _xdgCampaignsHeaderPrefixAreaCheckbox.IsChecked = AllCampaignRecordsSelected();
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        #endregion Campaigns Datagrid

        private void cmbSalaryReportMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBoxItem selectedItem = (ComboBoxItem)cmbReportMode.SelectedItem;
                _reportMode = byte.Parse(selectedItem.Tag.ToString());
                //if (_reportMode == 1)
                //{
                //    tbReportType.Visibility = Visibility.Visible;
                //    cmbReportType.Visibility = Visibility.Visible;
                //    tbCampaignCluster.Visibility = Visibility.Visible;
                //    cmbReportType.SelectedIndex = -1;
                //    tbGroupCampaigns.Visibility = Visibility.Visible;
                //    chkGroupCampaigns.Visibility = Visibility.Visible;
                //    xdgCampaigns.Visibility = Visibility.Visible;
                //    btnReport.Visibility = Visibility.Visible;

                //    btnTempReport.Visibility = Visibility.Collapsed;
                //    lblAgents.Visibility = Visibility.Collapsed;
                //    xdgAgents.Visibility = Visibility.Collapsed;
                //    xdgAgents.DataSource = null;

                //    tbBonusSales.Visibility = Visibility.Hidden;
                //    chkBonusSales.Visibility = Visibility.Hidden;
                //    chkBonusSales.IsChecked = false;
                //}
                //if (_reportMode == 2)
                //{
                //    LoadAgentInfo();
                //    tbReportType.Visibility = Visibility.Collapsed;
                //    cmbReportType.Visibility = Visibility.Collapsed;
                //    //cmbSalaryReportType.SelectedIndex = -1;
                //    tbCampaignCluster.Visibility = Visibility.Collapsed;
                //    tbGroupCampaigns.Visibility = Visibility.Collapsed;
                //    chkGroupCampaigns.Visibility = Visibility.Collapsed;
                //    xdgCampaigns.Visibility = Visibility.Collapsed;
                //    btnReport.Visibility = Visibility.Collapsed;

                //    btnTempReport.Visibility = Visibility.Visible;
                //    lblAgents.Visibility = Visibility.Visible;
                //    xdgAgents.Visibility = Visibility.Visible;

                //    tbBonusSales.Visibility = Visibility.Hidden;
                //    chkBonusSales.Visibility = Visibility.Hidden;
                //    chkBonusSales.IsChecked = false;

                //    //tbBonusSales.Visibility = Visibility.Hidden;
                //    //chkBonusSales.Visibility = Visibility.Hidden;
                //    //chkBonusSales.IsChecked = false;
                //}
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HeaderPrefixAreaCheckbox_CheckedTemp(object sender, RoutedEventArgs e)
        {
            try
            {
                if (xdgAgents.DataSource != null)
                {
                    DataTable dt = ((DataView)xdgAgents.DataSource).Table;

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["IsChecked"] = true;
                    }
                }

                IsAllRecordsCheckedTemp();
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        private void HeaderPrefixAreaCheckbox_UncheckedTemp(object sender, RoutedEventArgs e)
        {
            try
            {
                if (xdgAgents.DataSource != null)
                {
                    DataTable dt = ((DataView)xdgAgents.DataSource).Table;

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["IsChecked"] = false;
                    }
                }

                IsAllRecordsCheckedTemp();
            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }
        }

        private void RecordSelectorCheckbox_ClickTemp(object sender, RoutedEventArgs e)
        {
            AllRecordsCheckedTemp = IsAllRecordsCheckedTemp();
        }

        private bool? IsAllRecordsCheckedTemp()
        {
            try
            {
                bool allSelected = (xdgAgents.Records.Select(r => (bool)((DataRecord)r).Cells["IsChecked"].Value)).All(b => b);
                bool noneSelected = (xdgAgents.Records.Select(r => (bool)((DataRecord)r).Cells["IsChecked"].Value)).All(b => !b);

                int countSelected = (xdgAgents.Records.Select(r => (bool)((DataRecord)r).Cells["IsChecked"].Value)).Count(b => b);
                lblAgents.Text = "Select Temp Agent(s) " + "[" + countSelected + "]";

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
                (new BaseControl()).HandleException(ex);
                return null;
            }
        }

        private void LoadAgentInfo()
        {
            try
            {
                Cursor = Cursors.Wait;
                DataSet ds = Methods.ExecuteStoredProcedure("spGetTempSalesAgents", null);

                DataTable dt = ds.Tables[0];
                DataColumn column = new DataColumn("IsChecked", typeof(bool));
                column.DefaultValue = false;
                dt.Columns.Add(column);

                xdgAgents.DataSource = dt.DefaultView;

            }

            catch (Exception ex)
            {
                (new BaseControl()).HandleException(ex);
            }

            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void ShowTempReportControls(bool show)
        {
            btnClose.IsEnabled = show;
            btnTempReport.IsEnabled = show;
            xdgCampaigns.IsEnabled = show;
        }

        private bool IsAllTempReportInputParametersSpecifiedAndValid()
        {
            #region Ensuring that the From Date was specified

            //_fromDate = calFromDate.SelectedDate; //.Value.ToString("yyyy-MM-dd");

            if (calFromDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'From Date'.", @"No 'From Date' specified", ShowMessageType.Error);
                return false;
            }
            else
            {
                _fromDate = calFromDate.SelectedDate.Value; //.Value.ToString("yyyy-MM-dd");
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the To Date was specified

            //_reportEndDate = calToDate.SelectedDate;

            if (calToDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'To Date'.", @"No 'To Date' specified", ShowMessageType.Error);
                return false;
            }
            else
            {
                _toDate = calToDate.SelectedDate.Value;
            }

            #endregion Ensuring that the To Date was specified

            #region Ensuring that the date range is valid

            if (_fromDate > _toDate)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Invalid date range specified: The 'From Date' can not be greater than the 'To Date'.", "Invalid date range", ShowMessageType.Error);
                return false;
            }

            #endregion Ensuring that the date range is valid

            #region Ensuring that at least one sales consultant was selected

            var lstTemp = (from r in xdgAgents.Records where (bool)((DataRecord)r).Cells["IsChecked"].Value select r).ToList();
            _lstSelectedTempAgents = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["Description"].Value));

            if (_lstSelectedTempAgents.Count == 0)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 confirmation agent from the list.", "No confirmation agent selected", ShowMessageType.Error);
                return false;
            }
            else
            {
                _fkUserIDs = _lstSelectedTempAgents.Cast<DataRecord>().Where(record => (bool)record.Cells["IsChecked"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                _fkUserIDs = _fkUserIDs.Substring(0, _fkUserIDs.Length - 1);
            }

            #endregion Ensuring that at least one campaign was selected

            // Otherwise if all is well, proceed:
            return true;
        }

        private void btnTempReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region OLD

                //IEnumerable<DataRecord> agents = xdgAgents.Records.Cast<DataRecord>().ToArray();

                //if (calFromDate.SelectedDate.HasValue)
                //{
                //    _fromDate = calFromDate.SelectedDate.Value;
                //}

                //if (calToDate.SelectedDate.HasValue)
                //{
                //    _toDate = calToDate.SelectedDate.Value;
                //}

                //btnClose.IsEnabled = false;
                //btnTempReport.IsEnabled = false;
                //xdgCampaigns.IsEnabled = false;

                //BackgroundWorker worker = new BackgroundWorker();
                //worker.DoWork += TempReport;
                //worker.RunWorkerCompleted += ReportCompleted;
                //worker.RunWorkerAsync(agents);

                #endregion OLD

                if (IsAllTempReportInputParametersSpecifiedAndValid())
                {
                    ShowTempReportControls(false);

                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += TempReport;
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

        private void TempReportOLD(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                IEnumerable<DataRecord> Agents = e.Argument as IEnumerable<DataRecord>;
                string agentIDs = string.Empty;
                bool first = true;
                if (Agents != null)
                    foreach (DataRecord record in Agents)
                    {

                        if ((bool)record.Cells["IsChecked"].Value)
                        {
                            long agentID = Convert.ToInt32(record.Cells["ID"].Value);
                            if (first)
                            {
                                first = false;
                                agentIDs = agentID.ToString();
                            }
                            else
                            {
                                agentIDs = agentIDs + "," + agentID;
                            }

                        }
                    }
                #region Setup excel documents

                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                string filePathAndName = GlobalSettings.UserFolder + " Temp Salary  Report ~ " + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".xlsx";

                Uri uri = new Uri("/Templates/ReportTemplateTempSalaryReport.xlsx", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    wbTemplate = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }

                Worksheet wsTemplate = wbTemplate.Worksheets["Sheet1"];
                Worksheet wsReport = wbReport.Worksheets.Add("Summary");

                wsReport.PrintOptions.PaperSize = PaperSize.A4;
                wsReport.PrintOptions.Orientation = Orientation.Portrait;
                wsReport.PrintOptions.ScalingType = ScalingType.FitToPages;

                #endregion

                #region Get report data from database

                DataTable dtLeadAllocationData;
                DataTable dtDetailSummarry;

                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@agentIDs", agentIDs);
                parameters[1] = new SqlParameter("@FromDate", _fromDate.ToString("yyyy-MM-dd"));
                parameters[2] = new SqlParameter("@ToDate", _toDate.ToString("yyyy-MM-dd"));

                DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spTraineeSalaryReport", parameters);
                if (dsLeadAllocationData.Tables.Count > 0)
                {
                    dtLeadAllocationData = dsLeadAllocationData.Tables[0];
                    dtDetailSummarry = dsLeadAllocationData.Tables[1];
                    if (dtLeadAllocationData.Rows.Count == 0)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected  Agents and specified Date range.", "No Data", ShowMessageType.Information);
                        });

                        return;
                    }
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the select Agents and specified Date range.", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                #endregion

                Methods.CopyExcelRegion(wsTemplate, 0, 0, dtLeadAllocationData.Rows.Count + 6, 39, wsReport, 0, 0);

                #region report data
                {
                    int rowIndex = 3;

                    string[] strArr = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                                       "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN"};

                    foreach (DataRow rw in dtLeadAllocationData.Rows)
                    {
                        string TSR = rw["TSR"].ToString();
                        if (TSR.ToLower() == "zztotals")
                        {
                            TSR = "Totals";

                            for (int i = 0; i < 40; i++)
                            {
                                wsReport.GetCell(strArr[i] + rowIndex).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                                wsReport.GetCell(strArr[i] + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Medium;
                                wsReport.GetCell(strArr[i] + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Medium;
                                wsReport.GetCell(strArr[i] + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Medium;
                                wsReport.GetCell(strArr[i] + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Medium;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 40; i++)
                            {
                                wsReport.GetCell(strArr[i] + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                                wsReport.GetCell(strArr[i] + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                                wsReport.GetCell(strArr[i] + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                                wsReport.GetCell(strArr[i] + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
                            }
                        }

                        wsReport.GetCell("A" + rowIndex).Value = TSR;
                        wsReport.GetCell("B" + rowIndex).Value = rw["CancerLa1Higher"].ToString();
                        wsReport.GetCell("C" + rowIndex).Value = rw["CancerLa1Lower"].ToString();
                        wsReport.GetCell("D" + rowIndex).Value = rw["CancerLa2"].ToString();

                        wsReport.GetCell("E" + rowIndex).Value = rw["MaccLa1Higher"].ToString();
                        wsReport.GetCell("F" + rowIndex).Value = rw["MaccLa1Lower"].ToString();
                        wsReport.GetCell("G" + rowIndex).Value = rw["MaccLa2"].ToString();

                        wsReport.GetCell("H" + rowIndex).Value = rw["CancerFuneralLa1Higher"].ToString();
                        wsReport.GetCell("I" + rowIndex).Value = rw["CancerFuneralLa1Lower"].ToString();
                        wsReport.GetCell("J" + rowIndex).Value = rw["CancerFuneralLa2"].ToString();

                        wsReport.GetCell("K" + rowIndex).Value = rw["AccDisLa1Higher"].ToString();
                        wsReport.GetCell("L" + rowIndex).Value = rw["AccDisLa1Lower"].ToString();
                        wsReport.GetCell("M" + rowIndex).Value = rw["AccDisLa2"].ToString();

                        wsReport.GetCell("N" + rowIndex).Value = rw["CancerRenLa1Higher"].ToString();
                        wsReport.GetCell("O" + rowIndex).Value = rw["CancerRenLa1Lower"].ToString();
                        wsReport.GetCell("P" + rowIndex).Value = rw["CancerRenLa2"].ToString();

                        wsReport.GetCell("Q" + rowIndex).Value = rw["MaccRenLa1Higher"].ToString();
                        wsReport.GetCell("R" + rowIndex).Value = rw["MaccRenLa1Lower"].ToString();
                        wsReport.GetCell("S" + rowIndex).Value = rw["MaccRenLa2"].ToString();

                        wsReport.GetCell("T" + rowIndex).Value = rw["SheMaccLa1Higher"].ToString();
                        wsReport.GetCell("U" + rowIndex).Value = rw["SheMaccLa1Lower"].ToString();
                        wsReport.GetCell("V" + rowIndex).Value = rw["SheMaccLa2"].ToString();

                        wsReport.GetCell("W" + rowIndex).Value = rw["SheMaccRenLa1Higher"].ToString();
                        wsReport.GetCell("X" + rowIndex).Value = rw["SheMaccRenLa1Lower"].ToString();
                        wsReport.GetCell("Y" + rowIndex).Value = rw["SheMaccRenLa2"].ToString();

                        wsReport.GetCell("T" + rowIndex).Value = rw["SheMaccLa1Higher"].ToString();
                        wsReport.GetCell("U" + rowIndex).Value = rw["SheMaccLa1Lower"].ToString();
                        wsReport.GetCell("V" + rowIndex).Value = rw["SheMaccLa2"].ToString();

                        wsReport.GetCell("W" + rowIndex).Value = rw["SheMaccRenLa1Higher"].ToString();
                        wsReport.GetCell("X" + rowIndex).Value = rw["SheMaccRenLa1Lower"].ToString();
                        wsReport.GetCell("Y" + rowIndex).Value = rw["SheMaccRenLa2"].ToString();

                        wsReport.GetCell("Z" + rowIndex).Value = rw["MaccMillionLa1Higher"].ToString();
                        wsReport.GetCell("AA" + rowIndex).Value = rw["MaccMillionLa1Lower"].ToString();
                        wsReport.GetCell("AB" + rowIndex).Value = rw["MaccMillionLa2"].ToString();

                        wsReport.GetCell("AC" + rowIndex).Value = rw["MaccMillionRenLa1Higher"].ToString();
                        wsReport.GetCell("AD" + rowIndex).Value = rw["MaccMillionRenLa1Lower"].ToString();
                        wsReport.GetCell("AE" + rowIndex).Value = rw["MaccMillionRenLa2"].ToString();

                        wsReport.GetCell("AF" + rowIndex).Value = rw["BlackMaccMillionLa1Higher"].ToString();
                        wsReport.GetCell("AG" + rowIndex).Value = rw["BlackMaccMillionLa1Lower"].ToString();
                        wsReport.GetCell("AH" + rowIndex).Value = rw["BlackMaccMillionLa2"].ToString();

                        wsReport.GetCell("AI" + rowIndex).Value = rw["BlackMaccMillionRenLa1Higher"].ToString();
                        wsReport.GetCell("AJ" + rowIndex).Value = rw["BlackMaccMillionRenLa1Lower"].ToString();
                        wsReport.GetCell("AK" + rowIndex).Value = rw["BlackMaccMillionRenLa2"].ToString();

                        wsReport.GetCell("AL" + rowIndex).Value = rw["BlackMaccLa1Higher"].ToString();
                        wsReport.GetCell("AM" + rowIndex).Value = rw["BlackMacLa1Lower"].ToString();
                        wsReport.GetCell("AN" + rowIndex).Value = rw["BlackMaccLa2"].ToString();

                        rowIndex++;
                    }

                    Worksheet wsTemplate2 = wbTemplate.Worksheets["Sheet2"];
                    Worksheet wsReport2 = wbReport.Worksheets.Add("Data");

                    wsReport2.PrintOptions.PaperSize = PaperSize.A4;
                    wsReport2.PrintOptions.Orientation = Orientation.Portrait;
                    wsReport2.PrintOptions.ScalingType = ScalingType.FitToPages;
                    Methods.CopyExcelRegion(wsTemplate2, 0, 0, dtDetailSummarry.Rows.Count + 6, 19, wsReport2, 0, 0);
                    rowIndex = 2;
                    foreach (DataRow row in dtDetailSummarry.Rows)
                    {
                        wsReport2.GetCell("A" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                        wsReport2.GetCell("A" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                        wsReport2.GetCell("A" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                        wsReport2.GetCell("A" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                        wsReport2.GetCell("B" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                        wsReport2.GetCell("B" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                        wsReport2.GetCell("B" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                        wsReport2.GetCell("B" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                        wsReport2.GetCell("C" + rowIndex).CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                        wsReport2.GetCell("C" + rowIndex).CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                        wsReport2.GetCell("C" + rowIndex).CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                        wsReport2.GetCell("C" + rowIndex).CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;

                        wsReport2.GetCell("A" + rowIndex).Value = row["DateOfSale"].ToString();
                        wsReport2.GetCell("B" + rowIndex).Value = row["TSR"].ToString();
                        wsReport2.GetCell("C" + rowIndex).Value = row["PolicyTotalPremium"].ToString();

                        rowIndex++;
                    }
                }

                #endregion

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

        private void TempReport(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Get the report data - and exit the method if there is no data available

                DataSet dsSalaryReportData = Insure.INReportSalaryTemp(_fkUserIDs, _fromDate, _toDate);

                if (dsSalaryReportData.Tables[1].Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "There is no data available for the criteria you have specified. Please change some of them and try again.", "No data available", ShowMessageType.Information);
                    });

                    return;
                }

                DataRow drReportConfigs = dsSalaryReportData.Tables[0].Rows[0]; // Specify it only once here

                #endregion Get the report data - and exit the method if there is no data available

                #region Setup Excel document

                string filePathAndName = String.Format("{0}Temp Salary Report, {1} To {2} ~ {3}.xlsx",
                    GlobalSettings.UserFolder,
                    _fromDate.ToString("yyyy-MM-dd"),
                    _toDate.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateTempSalaryReport.xlsx");
                Workbook wbSalaryReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup Excel document

                #region Insert the main sheet

                InsertPrimaryReportSheet(wbTemplate, wbSalaryReport, dsSalaryReportData);

                #endregion Insert the main sheet

                #region Insert the individual sales sheet

                InsertIndividualSalesDetailsSheet(wbTemplate, wbSalaryReport, dsSalaryReportData);

                #endregion Insert the individual sales sheet

                #region Saves and opens the resulting Excel workbook - if there are any pages in the workbook

                if (wbSalaryReport.Worksheets.Count > 0)
                {
                    wbSalaryReport.Save(filePathAndName);

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

        private void InsertPrimaryReportSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsTempSalaryReportData)
        {
            #region Partition the given dataset

            DataRow drReportConfigs = dsTempSalaryReportData.Tables[0].Rows[0];
            DataTable dtMainReportData = dsTempSalaryReportData.Tables[1];
            DataTable dtExcelSheetDataTableColumnMappings = dsTempSalaryReportData.Tables[2];
            DataTable dtTotalsMappings = dsTempSalaryReportData.Tables[3];

            #endregion Partition the given dataset

            #region Declarations & Initializations

            string reportSubTitle = drReportConfigs["ReportSubTitle"].ToString();
            string reportTemplateSheetName = drReportConfigs["ReportTemplateSheetName"].ToString();
            byte reportTemplateColumnSpan = Convert.ToByte(drReportConfigs["ReportTemplateColumnSpan"]);
            byte reportTemplateRowSpan = Convert.ToByte(drReportConfigs["ReportTemplateRowSpan"]);
            byte reportTemplateDataRowIndex = Convert.ToByte(drReportConfigs["ReportTemplateDataRowIndex"]);
            byte reportTemplateTotalsRowIndex = Convert.ToByte(drReportConfigs["ReportTemplateTotalsRowIndex"]);

            //string individualLeadsTemplateSheetName = drReportConfigs["IndividualLeadsTemplateSheetName"].ToString();
            //byte individualLeadsTemplateColumnSpan = Convert.ToByte(drReportConfigs["IndividualLeadsTemplateColumnSpan"]);
            //byte individualLeadsTemplateRowSpan = Convert.ToByte(drReportConfigs["IndividualLeadsTemplateRowSpan"]);
            //byte individualLeadsTemplateDataRowIndex = Convert.ToByte(drReportConfigs["IndividualLeadsTemplateDataRowIndex"]);

            int reportRow = reportTemplateDataRowIndex;
            int formulaStartRow = reportRow;

            string worksheetTabName = reportTemplateSheetName;
            DateTime dateOfSale = DateTime.Now;

            #endregion Declarations & Initializations

            #region Adding a new sheet for the current campaign / campaign cluster

            Worksheet wsSalaryReportTemplate = wbTemplate.Worksheets[reportTemplateSheetName];
            Worksheet wsSalaryReport = wbReport.Worksheets.Add(worksheetTabName);

            #endregion Adding a new sheet for the current campaign / campaign cluster

            #region Populating the report details

            Methods.CopyExcelRegion(wsSalaryReportTemplate, 0, 0, reportTemplateRowSpan, reportTemplateColumnSpan, wsSalaryReport, 0, 0);
            wsSalaryReport.GetCell("A3").Value = reportSubTitle;
            wsSalaryReport.GetCell("AJ4").Value = String.Format("Date Generated: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            #endregion Populating the report details

            #region Add the data

            reportRow = Methods.MapTemplatizedExcelValues(wsSalaryReportTemplate, dtMainReportData, dtExcelSheetDataTableColumnMappings, reportTemplateDataRowIndex, 0, 0, reportTemplateColumnSpan, wsSalaryReport, reportRow, 0);

            #endregion Add the data

            #region Add the totals / averages

            reportRow = Methods.MapTemplatizedExcelFormulas(wsSalaryReportTemplate, dtTotalsMappings, reportTemplateTotalsRowIndex, 0, 0, reportTemplateColumnSpan, wsSalaryReport, reportRow, 0, formulaStartRow, reportRow - 1);

            #endregion Add the totals / averages

        }

        private void InsertIndividualSalesDetailsSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsTempSalaryReportData)
        {
            #region Partition the given dataset

            DataRow drReportConfigs = dsTempSalaryReportData.Tables[0].Rows[0];
            DataTable dtMainReportData = dsTempSalaryReportData.Tables[4];
            DataTable dtExcelSheetDataTableColumnMappings = dsTempSalaryReportData.Tables[5];

            #endregion Partition the given dataset

            #region Declarations & Initializations

            string reportSubTitle = drReportConfigs["ReportSubTitle"].ToString();
            string individualLeadsTemplateSheetName = drReportConfigs["IndividualLeadsTemplateSheetName"].ToString();
            byte individualLeadsTemplateColumnSpan = Convert.ToByte(drReportConfigs["IndividualLeadsTemplateColumnSpan"]);
            byte individualLeadsTemplateRowSpan = Convert.ToByte(drReportConfigs["IndividualLeadsTemplateRowSpan"]);
            byte individualLeadsTemplateDataRowIndex = Convert.ToByte(drReportConfigs["IndividualLeadsTemplateDataRowIndex"]);

            int reportRow = individualLeadsTemplateRowSpan;
            int formulaStartRow = reportRow;

            string worksheetTabName = individualLeadsTemplateSheetName;
            DateTime dateOfSale = DateTime.Now;

            #endregion Declarations & Initializations

            #region Adding a new sheet for the current campaign / campaign cluster

            Worksheet wsSalaryReportTemplate = wbTemplate.Worksheets[individualLeadsTemplateSheetName];
            Worksheet wsSalaryReport = wbReport.Worksheets.Add(worksheetTabName);

            #endregion Adding a new sheet for the current campaign / campaign cluster

            #region Populating the report details

            Methods.CopyExcelRegion(wsSalaryReportTemplate, 0, 0, individualLeadsTemplateRowSpan, individualLeadsTemplateColumnSpan, wsSalaryReport, 0, 0);
            wsSalaryReport.GetCell("A3").Value = reportSubTitle;

            #endregion Populating the report details

            #region Add the data

            reportRow = Methods.MapTemplatizedExcelValues(wsSalaryReportTemplate, dtMainReportData, dtExcelSheetDataTableColumnMappings, individualLeadsTemplateDataRowIndex, 0, 0, individualLeadsTemplateColumnSpan, wsSalaryReport, reportRow, 0);

            #endregion Add the data
        }

        private void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbReportType.SelectedIndex = 0;
            chkGroupCampaigns.IsChecked = true;
        }
    }
}
