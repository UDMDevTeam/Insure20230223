using System.Data.SqlClient;
using System.Windows.Resources;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
using System;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using Orientation = Infragistics.Documents.Excel.Orientation;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportTrackUpgradeBasePolicies
    {

        #region Constants

        //private string _fontName = "Calibri";
        //private const int _fontSize = 10;
        //private const int _pointsToTwipsFactor = 20;
        //private const int _fontHeight = _fontSize * _pointsToTwipsFactor;

        #endregion



        #region Private Members

        //private CheckBox _xdgHeaderPrefixAreaCheckbox;
        //private RecordCollectionBase _campaigns;
        private DateTime _startDate;
        private DateTime _endDate;
      

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        private bool _firstRun = true;

        #endregion



        #region Constructors

        public ReportTrackUpgradeBasePolicies()
        {
            InitializeComponent();
            //LoadCampaignInfo();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion



        #region Private Methods

        //private bool? AllRecordsSelected()
        //{
        //    try
        //    {
        //        bool allSelected = true;
        //        bool noneSelected = true;

        //        if (xdgCampaigns.DataSource != null)
        //        {
        //            foreach (DataRow dr in ((DataView)xdgCampaigns.DataSource).Table.Rows)
        //            {
        //                allSelected = allSelected && (bool)dr["Select"];
        //                noneSelected = noneSelected && !(bool)dr["Select"];
        //            }
        //        }

        //        if (allSelected)
        //        {
        //            return true;
        //        }
        //        if (noneSelected)
        //        {
        //            return false;
        //        }

        //        return null;
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //        return null;
        //    }
        //}

        //private void LoadCampaignInfo()
        //{
        //    try
        //    {
        //        SetCursor(Cursors.Wait);

        //        DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign");
        //        DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
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

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";

            btnReport.IsEnabled = true;
            btnClose.IsEnabled = true;
            //xdgCampaigns.IsEnabled = true;
            //calStartDate.IsEnabled = true;
            //calEndDate.IsEnabled = true;
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                #region Setting the Excel workbook

                Workbook wbTemplate;

                string start = _startDate.ToShortDateString().Replace(@"\", "-").Replace(@"/", "-");
                string end = _endDate.ToShortDateString().Replace(@"\", "-").Replace(@"/", "-");

                string filePathAndName =
                    $"{GlobalSettings.UserFolder}Upgrade Base Policies Batched ({start} to {end}) ~ {DateTime.Now.Millisecond}.xlsx";

                Uri uri = new Uri("/Templates/ReportTemplateTrackUpgradeBasePolicies.xlsx", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    wbTemplate = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }

                Worksheet wsTemplate = wbTemplate.Worksheets["Data"];

                #endregion Setting the Excel workbook


                #region Getting the report data

                DataTable dtBatchedSalesData;
                SqlParameter[] parameters = new SqlParameter[3];
                
                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    parameters[0] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
                    parameters[1] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));
                    parameters[2] = new SqlParameter("@Target", Convert.ToInt32(txtTarget.Text));
                });

                DataSet dsReportData = Methods.ExecuteStoredProcedure("spINReportTrackUpgradeBasePolicies", parameters);

                if (dsReportData.Tables.Count > 0)
                {
                    dtBatchedSalesData = dsReportData.Tables[0];

                    if (dtBatchedSalesData.Rows.Count == 0)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "There is no data to report on.", "No Data", ShowMessageType.Information);
                        });

                        return;
                    }
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "There is no data to report on.", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                #endregion Getting the report data


                #region Main Report

                Worksheet wsReport = wbReport.Worksheets.Add("Batched Sales");
                wsReport.PrintOptions.PaperSize = PaperSize.A4;
                wsReport.PrintOptions.Orientation = Orientation.Portrait;
                wsReport.PrintOptions.ScalingType = ScalingType.FitToPages;

                int rowIndex = 5;
                Methods.CopyExcelRegion(wsTemplate, 0, 0, 0, 7, wsReport, rowIndex, 0);

                rowIndex++;
                bool started = false;
                foreach (DataRow dr in dtBatchedSalesData.AsEnumerable())
                {
                    if (!started)
                    {
                        Methods.CopyExcelRegion(wsTemplate, 1, 0, 0, 7, wsReport, rowIndex, 0);
                        started = true;
                    }
                    else
                    {
                        Methods.CopyExcelRegion(wsTemplate, 2, 0, 0, 7, wsReport, rowIndex, 0);
                    }

                    wsReport.GetCell("A" + (rowIndex + 1)).Value = dr.ItemArray[0];
                    wsReport.GetCell("B" + (rowIndex + 1)).Value = dr.ItemArray[1];
                    wsReport.GetCell("C" + (rowIndex + 1)).Value = dr.ItemArray[2];
                    wsReport.GetCell("D" + (rowIndex + 1)).Value = dr.ItemArray[3];
                    wsReport.GetCell("E" + (rowIndex + 1)).Value = dr.ItemArray[4];
                    wsReport.GetCell("F" + (rowIndex + 1)).Value = dr.ItemArray[5];
                    wsReport.GetCell("G" + (rowIndex + 1)).Value = dr.ItemArray[6];

                    rowIndex++;
                }

                {
                    DataTable dtBatchedSale11001 = dsReportData.Tables[1];
                    DataTable dtDescription = dsReportData.Tables[3];
                    string str = dtDescription.Rows[0].ItemArray[0].ToString(); //"Batched Sale 11001: "


                    if (dtBatchedSale11001.Rows.Count > 0)
                    {
                        Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, 7, wsReport, 1, 0);
                        Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, 7, wsReport, 2, 0);
                        Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, 7, wsReport, 3, 0);

                        str += " (";
                        str += dtBatchedSale11001.Rows[0].ItemArray[1] + ", ";
                        str += dtBatchedSale11001.Rows[0].ItemArray[2] + ", ";
                        str += dtBatchedSale11001.Rows[0].ItemArray[3];
                        str += ")";
                    }
                    else
                    {
                        Methods.CopyExcelRegion(wsTemplate, 4, 0, 0, 7, wsReport, 1, 0);
                        Methods.CopyExcelRegion(wsTemplate, 4, 0, 0, 7, wsReport, 2, 0);
                        Methods.CopyExcelRegion(wsTemplate, 4, 0, 0, 7, wsReport, 3, 0);
                    }

                    wsReport.GetCell("A2").Value = str;

                    DataTable dtBatchedSalesToDate = dsReportData.Tables[4];
                    str = dtBatchedSalesToDate.Rows[0].ItemArray[0].ToString();
                    wsReport.GetCell("A3").Value = str;

                    DataTable dtBatchedSalesToGo = dsReportData.Tables[5];
                    str = dtBatchedSalesToGo.Rows[0].ItemArray[0].ToString();
                    wsReport.GetCell("A4").Value = str;
                }
            

                #endregion


                #region Save and open the resulting workbook

                wbReport.Save(filePathAndName);                  
                Process.Start(filePathAndName);

                #endregion Save and open the resulting workbook
                
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

        //private void InsertCancellationDataSheet(Workbook wbResultingWorkbook, Worksheet wsTemplate, string campaignIDs, DateTime fromDate, DateTime toDate)
        //{
        //    #region Firstly, check if there is any data to be displayed

        //    DataSet dsFallOffReportDatasheetData = UDM.Insurance.Business.Insure.INGetFallOffReportDatasheetData(campaignIDs, fromDate, toDate);

        //    if ((dsFallOffReportDatasheetData == null) || (dsFallOffReportDatasheetData.Tables.Count == 0))
        //    {
        //        return;
        //    }

        //    if (dsFallOffReportDatasheetData.Tables[0].Rows.Count == 0)
        //    {
        //        return;
        //    }

        //    #endregion Firstly, check if there is any data to be displayed

        //    DataTable dtCancellationsDatasheetData = dsFallOffReportDatasheetData.Tables[0];

        //    #region Declarations

        //    int reportRow = 3;
        //    int reportTemplateRowIndex = 3;
        //    int reportTemplateColumnSpan = 7;

        //    #endregion Declarations

        //    #region Add the new worksheet

        //    Worksheet wsNewReportSheet = wbResultingWorkbook.Worksheets.Add("Cancellations Datasheet");
        //    Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsNewReportSheet, true, true, true, true, true, false, false, false, true, true, true, true, true, true, true, true, false);

        //    //wsNewReportSheet.PrintOptions.ScalingType = ScalingType.FitToPages;
        //    //wsNewReportSheet.PrintOptions.MaxPagesHorizontally = 1;

        //    #endregion Add the new worksheet

        //    #region Step 1: Copy a region from the template that consists of the headings and the column headings

        //    Methods.CopyExcelRegion(wsTemplate, 0, 0, 2, reportTemplateColumnSpan, wsNewReportSheet, 0, 0);

        //    #endregion Step 1: Copy a region from the template that consists of the headings and the column headings

        //    for (int i = 0; i < dtCancellationsDatasheetData.Rows.Count; i++)
        //    {
        //        #region Step 2.1. Copy the template formatting for the data row

        //        Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, reportTemplateColumnSpan, wsNewReportSheet, reportRow, 0);

        //        #endregion Step 2.1. Copy the template formatting for the data row

        //        #region Step 2.2. Add the values

        //        wsNewReportSheet.GetCell(String.Format("A{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["RefNo"];
        //        wsNewReportSheet.GetCell(String.Format("B{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["CampaignName"];
        //        wsNewReportSheet.GetCell(String.Format("C{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["DateOfSale"];
        //        wsNewReportSheet.GetCell(String.Format("D{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["DateOfWork"];
        //        wsNewReportSheet.GetCell(String.Format("E{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["DateOfCancellation"];
        //        wsNewReportSheet.GetCell(String.Format("F{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["CancellationReason"];
        //        wsNewReportSheet.GetCell(String.Format("G{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["TSR"];
        //        wsNewReportSheet.GetCell(String.Format("H{0}", reportRow + 1)).Value = dtCancellationsDatasheetData.Rows[i]["ConfirmationAgent"];

        //        ++reportRow;

        //        #endregion Step 2.2. Add the values
        //    }
        //}

        //private void InsertCarriedForwardsDataSheet(Workbook wbResultingWorkbook, Worksheet wsTemplate, string campaignIDs, DateTime fromDate, DateTime toDate)
        //{
        //    #region Firstly, check if there is any data to be displayed

        //    DataSet dsFallOffReportDatasheetData = UDM.Insurance.Business.Insure.INGetFallOffReportDatasheetData(campaignIDs, fromDate, toDate);

        //    if ((dsFallOffReportDatasheetData == null) || (dsFallOffReportDatasheetData.Tables.Count == 0))
        //    {
        //        return;
        //    }

        //    if (dsFallOffReportDatasheetData.Tables[1].Rows.Count == 0)
        //    {
        //        return;
        //    }

        //    #endregion Firstly, check if there is any data to be displayed

        //    DataTable dtCarriedForwardsDatasheetData = dsFallOffReportDatasheetData.Tables[1];

        //    #region Declarations

        //    int reportRow = 3;
        //    int reportTemplateRowIndex = 3;
        //    int reportTemplateColumnSpan = 7;

        //    #endregion Declarations

        //    #region Add the new worksheet

        //    Worksheet wsNewReportSheet = wbResultingWorkbook.Worksheets.Add("Carried Forwards Datasheet");
        //    Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsNewReportSheet, true, true, true, true, true, false, false, false, true, true, true, true, true, true, true, true, false);

        //    //wsNewReportSheet.PrintOptions.ScalingType = ScalingType.FitToPages;
        //    //wsNewReportSheet.PrintOptions.MaxPagesHorizontally = 1;

        //    #endregion Add the new worksheet

        //    #region Step 1: Copy a region from the template that consists of the headings and the column headings

        //    Methods.CopyExcelRegion(wsTemplate, 0, 0, 2, reportTemplateColumnSpan, wsNewReportSheet, 0, 0);

        //    #endregion Step 1: Copy a region from the template that consists of the headings and the column headings

        //    for (int i = 0; i < dtCarriedForwardsDatasheetData.Rows.Count; i++)
        //    {
        //        #region Step 2.1. Copy the template formatting for the data row

        //        Methods.CopyExcelRegion(wsTemplate, reportTemplateRowIndex, 0, 0, reportTemplateColumnSpan, wsNewReportSheet, reportRow, 0);

        //        #endregion Step 2.1. Copy the template formatting for the data row

        //        #region Step 2.2. Add the values

        //        wsNewReportSheet.GetCell(String.Format("A{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["RefNo"];
        //        wsNewReportSheet.GetCell(String.Format("B{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["CampaignName"];
        //        wsNewReportSheet.GetCell(String.Format("C{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["DateOfSale"];
        //        wsNewReportSheet.GetCell(String.Format("D{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["DateOfWork"];
        //        wsNewReportSheet.GetCell(String.Format("E{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["DateOfCarriedForward"];
        //        wsNewReportSheet.GetCell(String.Format("F{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["CarriedForwardReason"];
        //        wsNewReportSheet.GetCell(String.Format("G{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["TSR"];
        //        wsNewReportSheet.GetCell(String.Format("H{0}", reportRow + 1)).Value = dtCarriedForwardsDatasheetData.Rows[i]["ConfirmationAgent"];

        //        ++reportRow;

        //        #endregion Step 2.2. Add the values
        //    }
        //}

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;
            btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnReport.ToolTip = btnReport.Content;
        }

        private void CalculateTarget()
        {
            if (cmbMonth.SelectedIndex > -1 && 
                !string.IsNullOrWhiteSpace(medUpgradeLeadsReceived.Text) &&
                !string.IsNullOrWhiteSpace(medLesssheMaccLeads.Text) &&
                !string.IsNullOrWhiteSpace(medTargetPercentage.Text))
            {
                txtTotalLeads.Text = (Convert.ToInt32(medUpgradeLeadsReceived.Text) - Convert.ToInt32(medLesssheMaccLeads.Text)).ToString();

                decimal target = Convert.ToDecimal(txtTotalLeads.Text) * Convert.ToDecimal(medTargetPercentage.Text) / 100m;
                decimal integral = Math.Truncate(target);
                decimal fractional = target - integral;

                if(target > 0 )
                {
                    if (fractional < 0.1m)
                    {
                        txtTarget.Text = Math.Floor(target).ToString();
                    }
                    else
                    {
                        txtTarget.Text = Math.Ceiling(target).ToString();
                    }
                }
                else
                {
                    txtTotalLeads.Text = string.Empty;
                    txtTarget.Text = string.Empty;
                }
            }
            else
            {
                txtTotalLeads.Text = string.Empty;
                txtTarget.Text = string.Empty;
            }
        }

        //private static Action EmptyDelegate = delegate () { Thread.Sleep(100); };

        //private static Action EmptyDelegate = delegate () { };

        //private static void DoRefresh(UIElement uiElement)
        //{
        //    uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        //}

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
                btnClose.IsEnabled = false;
                btnReport.IsEnabled = false;

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

        //private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;
        //        EnableDisableReportButton();
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            dr["Select"] = true;
        //        }
        //        foreach (var item in xdgCampaigns.SelectedItems)
        //        {
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        //private void HeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;
        //        btnReport.IsEnabled = false;
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            dr["Select"] = false;
        //        }


        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        //private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        EnableDisableReportButton();
        //        if (_xdgHeaderPrefixAreaCheckbox != null)
        //        {
        //            _xdgHeaderPrefixAreaCheckbox.IsChecked = AllRecordsSelected();
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        //private void HeaderPrefixAreaCheckbox_Loaded(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        _xdgHeaderPrefixAreaCheckbox = (CheckBox)sender;
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //}

        private void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                int year = DateTime.Now.Year;
                string month = DateTime.Now.ToString("MMMM");
                long savedByID = 0;
                string savedByUserName = string.Empty;

                ReportTrackUpgradeBasePoliciesData reportData = ReportTrackUpgradeBasePoliciesDataMapper.SearchOne(year, month, null, null, null, null);

                if (reportData != null)
                {
                    cmbYear.SelectedItem = cmbYear.Items.OfType<ComboBoxItem>().FirstOrDefault(item => (string)item.Content == Convert.ToString(reportData.Year));
                    cmbMonth.SelectedItem = cmbMonth.Items.OfType<ComboBoxItem>().FirstOrDefault(item => (string)item.Content == reportData.Month);
                    medUpgradeLeadsReceived.Text = reportData.LeadsReceived.ToString();
                    medLesssheMaccLeads.Text = reportData.SheMaccLeads.ToString();
                    medTargetPercentage.Text = reportData.TargetPercentage.ToString();

                    savedByID = Convert.ToInt64(Methods.GetTableData("Select Top 1 StampUserID FROM ReportTrackUpgradeBasePoliciesData WHERE Year = '" + year + "' AND Month = '" + month + "'").Rows[0][0]);
                    savedByUserName = Convert.ToString(Methods.GetTableData("SELECT Blush.dbo.GetUserName('" + savedByID + "')").Rows[0][0]);
                    txtSavedBy.Text = savedByUserName;
                }
                else
                {
                    cmbYear.SelectedItem = cmbYear.Items.OfType<ComboBoxItem>().FirstOrDefault(item => (string)item.Content == year.ToString());
                    cmbMonth.SelectedItem = cmbMonth.Items.OfType<ComboBoxItem>().FirstOrDefault(item => (string)item.Content == month);
                }

            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        
        private void calEndDate_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            if (calEndDate.SelectedDate != null) _endDate = DateTime.Parse(calEndDate.SelectedDate.Value.ToString(CultureInfo.InvariantCulture));
            EnableDisableReportButton();
        }

        private void calStartDate_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            if (calStartDate.SelectedDate != null)
                _startDate = DateTime.Parse(calStartDate.SelectedDate.Value.ToString(CultureInfo.InvariantCulture));
            EnableDisableReportButton();
        }

        private void EnableDisableReportButton()
        {
            if ((calStartDate.SelectedDate != null) && 
                (calEndDate.SelectedDate != null) && 
                (calEndDate.SelectedDate >= calStartDate.SelectedDate) &&
                cmbYear.SelectedIndex >= 0 &&
                cmbMonth.SelectedIndex >= 0 &&
                !string.IsNullOrWhiteSpace(medUpgradeLeadsReceived.Text) &&
                !string.IsNullOrWhiteSpace(medLesssheMaccLeads.Text) &&
                !string.IsNullOrWhiteSpace(medTargetPercentage.Text)
               )
            {
                btnReport.IsEnabled = true;
            }
            else
            {
                btnReport.IsEnabled = false;
            }
        }

        private void EnableDisableSaveButton()
        {
            if (cmbYear.SelectedIndex >= 0 &&
                cmbMonth.SelectedIndex >= 0 &&
                !string.IsNullOrWhiteSpace(medUpgradeLeadsReceived.Text) &&
                !string.IsNullOrWhiteSpace(medLesssheMaccLeads.Text) &&
                !string.IsNullOrWhiteSpace(medTargetPercentage.Text))
            {
                btnSave.IsEnabled = true;
            }
            else
            {
                btnSave.IsEnabled = false;
            }
        }

        private void calStartDate_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime date = DateTime.Today;

            calStartDate.SelectedDate = date <= new DateTime(date.Year, date.Month, 10) ? 
                new DateTime(date.Year, date.AddMonths(-1).Month, 1) : new DateTime(date.Year, date.Month, 1);

            Keyboard.Focus(calStartDate);
        }

        private void calEndDate_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime date = DateTime.Today;

            if (date <= new DateTime(date.Year, date.Month, 10))
            {
                var firstDayOfMonthDate = new DateTime(date.Year, date.AddMonths(-1).Month, 1);
                var lastDayOfMonth = firstDayOfMonthDate.AddMonths(1).AddDays(-1).Day;

                calEndDate.SelectedDate = new DateTime(date.Year, date.AddMonths(-1).Month, lastDayOfMonth);
            }
            else
            {
                var firstDayOfMonthDate = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonthDate.AddMonths(1).AddDays(-1).Day;

                calEndDate.SelectedDate = new DateTime(date.Year, date.Month, lastDayOfMonth);
            }

            Keyboard.Focus(calEndDate);
            Keyboard.Focus(calStartDate);
        }

        private void cmbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbYear.SelectedItem != null &&
                    cmbMonth.SelectedItem != null)
                {
                    int year = Convert.ToInt32(((ComboBoxItem)cmbYear.SelectedItem).Content.ToString());
                    string month = ((ComboBoxItem)cmbMonth.SelectedItem).Content.ToString();
                    long savedByID = 0;
                    string savedByUserName = string.Empty;

                    ReportTrackUpgradeBasePoliciesData reportData = ReportTrackUpgradeBasePoliciesDataMapper.SearchOne(year, month, null, null, null, null);

                    if (reportData != null)
                    {
                        medUpgradeLeadsReceived.Text = reportData.LeadsReceived.ToString();
                        medLesssheMaccLeads.Text = reportData.SheMaccLeads.ToString();
                        medTargetPercentage.Text = reportData.TargetPercentage.ToString();

                        savedByID = Convert.ToInt64(Methods.GetTableData("Select Top 1 StampUserID FROM ReportTrackUpgradeBasePoliciesData WHERE Year = '" + year + "' AND Month = '" + month + "'").Rows[0][0]);
                        savedByUserName = Convert.ToString(Methods.GetTableData("SELECT Blush.dbo.GetUserName('" + savedByID + "')").Rows[0][0]);
                        txtSavedBy.Text = savedByUserName;
                    }
                    else
                    {
                        medUpgradeLeadsReceived.Text = string.Empty;
                        medLesssheMaccLeads.Text = string.Empty;
                        //medTargetPercentage.Text = string.Empty;
                        txtSavedBy.Text = string.Empty;
                    }

                    CalculateTarget();
                }
                else
                {
                    medUpgradeLeadsReceived.Text = string.Empty;
                    medLesssheMaccLeads.Text = string.Empty;
                    //medTargetPercentage.Text = string.Empty;
                    txtSavedBy.Text = string.Empty;
                }

                EnableDisableSaveButton();
                EnableDisableReportButton();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmbMonth_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {

                if (cmbYear.SelectedItem != null &&
                    cmbMonth.SelectedItem != null)
                {
                    int year = Convert.ToInt32(((ComboBoxItem)cmbYear.SelectedItem).Content.ToString());
                    string month = ((ComboBoxItem)cmbMonth.SelectedItem).Content.ToString(); 
                    int monthNo = DateTime.ParseExact(month, "MMMM", CultureInfo.CurrentCulture).Month;
                    long savedByID = 0;
                    string savedByUserName = string.Empty;

                    ReportTrackUpgradeBasePoliciesData reportData = ReportTrackUpgradeBasePoliciesDataMapper.SearchOne(year, month, null, null, null, null);

                    if (reportData != null)
                    {
                        medUpgradeLeadsReceived.Text = reportData.LeadsReceived.ToString();
                        medLesssheMaccLeads.Text = reportData.SheMaccLeads.ToString();
                        medTargetPercentage.Text = reportData.TargetPercentage.ToString();

                        savedByID = Convert.ToInt64(Methods.GetTableData("Select Top 1 StampUserID FROM ReportTrackUpgradeBasePoliciesData WHERE Year = '" + year + "' AND Month = '" + month + "'").Rows[0][0]);
                        savedByUserName = Convert.ToString(Methods.GetTableData("SELECT Blush.dbo.GetUserName('" + savedByID + "')").Rows[0][0]);
                        txtSavedBy.Text = savedByUserName;
                    }
                    else
                    {
                        medUpgradeLeadsReceived.Text = string.Empty;
                        medLesssheMaccLeads.Text = string.Empty;
                        txtSavedBy.Text = string.Empty;
                    }
                    
                    calStartDate.SelectedDate = new DateTime(year, monthNo, 1);
                    calEndDate.SelectedDate = new DateTime(year, monthNo, DateTime.DaysInMonth(year, monthNo));
                    Keyboard.Focus(calEndDate);
                    Keyboard.Focus(calStartDate);

                    CalculateTarget();
                }
                else
                {
                    medUpgradeLeadsReceived.Text = string.Empty;
                    medLesssheMaccLeads.Text = string.Empty;
                    //medTargetPercentage.Text = string.Empty;
                    txtSavedBy.Text = string.Empty;
                }

                if (_firstRun)
                {
                    int index = cmbMonth.SelectedIndex;

                    _firstRun = false;

                    cmbMonth.SelectedIndex = 0;
                    cmbMonth.SelectedIndex = index;
                }

                EnableDisableSaveButton();
                EnableDisableReportButton();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void medUpgradeLeadsReceived_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            CalculateTarget();
            EnableDisableSaveButton();
            EnableDisableReportButton();
        }

        private void medLesssheMaccLeads_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            CalculateTarget();
            EnableDisableSaveButton();
            EnableDisableReportButton();
        }

        private void medTargetPercentage_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            CalculateTarget();
            EnableDisableSaveButton();
            EnableDisableReportButton();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int year = Convert.ToInt32(((ComboBoxItem)cmbYear.SelectedItem).Content.ToString());
                string month = ((ComboBoxItem)cmbMonth.SelectedItem).Content.ToString();
                long savedByID = 0;
                string savedByUserName = string.Empty;

                ReportTrackUpgradeBasePoliciesData reportData = ReportTrackUpgradeBasePoliciesDataMapper.SearchOne(year, month, null, null, null, null);

                if (reportData == null)
                {
                    reportData = new ReportTrackUpgradeBasePoliciesData();
                }

                reportData.Year = year;
                reportData.Month = month;
                reportData.LeadsReceived = Convert.ToInt32(medUpgradeLeadsReceived.Text);
                reportData.SheMaccLeads = Convert.ToInt32(medLesssheMaccLeads.Text);
                reportData.TargetPercentage = Convert.ToDecimal(medTargetPercentage.Text);

                reportData.Save(_validationResult);

                //string savedByUserName = Convert.ToString(Methods.GetTableData("SELECT Blush.dbo.GetUserName('" + GlobalSettings.ApplicationUser.ID + "')").Rows[0][0]);
                //txtSavedBy.Text = savedByUserName;

                reportData = ReportTrackUpgradeBasePoliciesDataMapper.SearchOne(year, month, null, null, null, null);

                if (reportData != null)
                {
                    cmbYear.SelectedItem = cmbYear.Items.OfType<ComboBoxItem>().FirstOrDefault(item => (string)item.Content == Convert.ToString(reportData.Year));
                    cmbMonth.SelectedItem = cmbMonth.Items.OfType<ComboBoxItem>().FirstOrDefault(item => (string)item.Content == reportData.Month);
                    medUpgradeLeadsReceived.Text = reportData.LeadsReceived.ToString();
                    medLesssheMaccLeads.Text = reportData.SheMaccLeads.ToString();
                    medTargetPercentage.Text = reportData.TargetPercentage.ToString();

                    savedByID = Convert.ToInt64(Methods.GetTableData("Select Top 1 StampUserID FROM ReportTrackUpgradeBasePoliciesData WHERE Year = '" + year + "' AND Month = '" + month + "'").Rows[0][0]);
                    savedByUserName = Convert.ToString(Methods.GetTableData("SELECT Blush.dbo.GetUserName('" + savedByID + "')").Rows[0][0]);
                    txtSavedBy.Text = savedByUserName;
                }

                ShowMessageBox(new INMessageBoxWindow1(), "Target data saved successfully.", "Target Data", ShowMessageType.Information);
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        
        #endregion

        
    }
}
