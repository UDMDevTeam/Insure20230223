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
using UDM.Insurance.Interface.Data;
using Embriant.Framework.Data;
using UDM.Insurance.Business;

namespace UDM.Insurance.Interface.Screens
{

    public partial class ReportBatchScreen
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
        //private DateTime _startDate;
        //private DateTime _endDate;
        private int _year = DateTime.Now.Year;
        private int _month = DateTime.Now.Month;
        private bool _includeLeadsCopiedToExtension;
        private bool _includeCompletedBatches;
        private bool _onlyBatchesReceived91DaysAgoAndAfter = false;

        private byte _reportTypeID = 0;
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        private List<Record> _lstSelectedFKINCampaignIDs;
        private string _campaignIDs = String.Empty;
        private string _selectedReportType = String.Empty;

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

        #region Constructors

        public ReportBatchScreen()
        {
            InitializeComponent();
            _includeCompletedBatches = false;
            CommonControlData.PopulateCampaignComboBox(cmbCampaign);

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);

            LoadLookups();
            cmbReportType.SelectedIndex = 0;
            _includeLeadsCopiedToExtension = false;

            //Action act = delegate ()
            //{
            //    //_calendar.SelectedDate = ((ViewModel)DataContext).Display;
            //    clYear.DisplayMode = CalendarMode.Year;
            //    //_calendar.SelectedDate = null;
            //};
            //Dispatcher.BeginInvoke(act, DispatcherPriority.ApplicationIdle);

            //_startDate = new DateTime(2014, 10, 1); //DateTime.MinValue;
            //_endDate = DateTime.Now;
        }

        #endregion Constructors

        #region Private Methods

        private void LoadLookups()
        {
            DataTable dtBatchReportTypes = Business.Insure.INGetBatchReportTypes();
            cmbReportType.Populate(dtBatchReportTypes, "ReportType", "ID");
        }

        private void SetFormMode(byte selectedReportType)
        {
            DataTable dtReportTypeCampaigns = Business.Insure.INGetBatchReportCampaignsOrCampaignTypesByReportType(selectedReportType);
            xdgCampaigns.DataSource = dtReportTypeCampaigns.DefaultView;

            _reportTypeID = selectedReportType;

            switch (selectedReportType)
            {
                case 1:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    {
                        xdgCampaigns.IsEnabled = true;
                        lblMonth.Visibility = Visibility.Collapsed;
                        lblYear.Visibility = Visibility.Collapsed;
                        cmbMonth.Visibility = Visibility.Collapsed;
                        clYear.Visibility = Visibility.Collapsed;
                        lblIncludeLeadsCopiedToExtension.Visibility = Visibility.Visible;
                        chkIncludeLeadsCopiedToExtension.Visibility = Visibility.Visible;

                        // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/221462601/comments
                        lblOnlyBatchesImported91DaysAndAfter.Visibility = Visibility.Visible;
                        chkOnlyBatchesImported91DaysAndAfter.Visibility = Visibility.Visible;
                        lblCombineUL.Visibility = Visibility.Visible;
                        chkCombineUL.Visibility = Visibility.Visible;

                        //lblSalesConversion.Visibility = Visibility.Visible;
                        //chkSalesConversion.Visibility = Visibility.Visible;

                        //lblContactsConversion.Visibility = Visibility.Visible;
                        //chkContactsConversion.Visibility = Visibility.Visible;

                        //lblCal1.Visibility = Visibility.Visible;
                        //calStartDate.Visibility = Visibility.Visible;
                        //calEndDate.Visibility = Visibility.Visible;
                    }

                    break;

                case 2:
                    {
                        xdgCampaigns.IsEnabled = false;
                        HeaderPrefixAreaCheckbox_Checked(xdgCampaigns, new RoutedEventArgs());
                        lblMonth.Visibility = Visibility.Visible;
                        lblYear.Visibility = Visibility.Visible;
                        cmbMonth.Visibility = Visibility.Visible;
                        clYear.Visibility = Visibility.Visible;
                        lblIncludeLeadsCopiedToExtension.Visibility = Visibility.Collapsed;
                        chkIncludeLeadsCopiedToExtension.Visibility = Visibility.Collapsed;

                        // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/221462601/comments
                        lblOnlyBatchesImported91DaysAndAfter.Visibility = Visibility.Collapsed;
                        chkOnlyBatchesImported91DaysAndAfter.Visibility = Visibility.Collapsed;
                        lblCombineUL.Visibility = Visibility.Collapsed;
                        chkCombineUL.Visibility = Visibility.Collapsed;


                        //lblSalesConversion.Visibility = Visibility.Collapsed;
                        //chkSalesConversion.Visibility = Visibility.Collapsed;
                        //lblContactsConversion.Visibility = Visibility.Collapsed;
                        //chkContactsConversion.Visibility = Visibility.Collapsed;
                        //RData.SalesConversionPerBatch = false;
                        //RData.ContactsConversionPerBatch = false;

                        //lblCal1.Visibility = Visibility.Collapsed;
                        //calStartDate.Visibility = Visibility.Collapsed;
                        //calEndDate.Visibility = Visibility.Collapsed;

                        //Have to set the display mode for the calendar like this because there is a bug in the windows calendar
                        //that displays the calendar funny when using a datacontext and setting the calendar mode to year before
                        //setting the data context.
                        Action act = delegate ()
                        {
                            //_calendar.SelectedDate = ((ViewModel)DataContext).Display;
                            clYear.DisplayMode = CalendarMode.Year;
                            //_calendar.SelectedDate = null;
                        };
                        Dispatcher.BeginInvoke(act, DispatcherPriority.ApplicationIdle);

                    }

                    break;

                case 3:
                    {
                        xdgCampaigns.IsEnabled = true;
                        lblMonth.Visibility = Visibility.Collapsed;
                        lblYear.Visibility = Visibility.Collapsed;
                        cmbMonth.Visibility = Visibility.Collapsed;
                        clYear.Visibility = Visibility.Collapsed;
                        lblIncludeLeadsCopiedToExtension.Visibility = Visibility.Collapsed;
                        chkIncludeLeadsCopiedToExtension.Visibility = Visibility.Collapsed;

                        // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/221462601/comments
                        lblOnlyBatchesImported91DaysAndAfter.Visibility = Visibility.Collapsed;
                        chkOnlyBatchesImported91DaysAndAfter.Visibility = Visibility.Collapsed;
                        lblCombineUL.Visibility = Visibility.Collapsed;
                        chkCombineUL.Visibility = Visibility.Collapsed;


                        //lblSalesConversion.Visibility = Visibility.Collapsed;
                        //chkSalesConversion.Visibility = Visibility.Collapsed;
                        //lblContactsConversion.Visibility = Visibility.Collapsed;
                        //chkContactsConversion.Visibility = Visibility.Collapsed;
                        //RData.SalesConversionPerBatch = false;
                        //RData.ContactsConversionPerBatch = false;


                        //lblCal1.Visibility = Visibility.Collapsed;
                        //calStartDate.Visibility = Visibility.Collapsed;
                        //calEndDate.Visibility = Visibility.Collapsed;
                    }

                    break;
            }

            //xdgCampaigns.DataSource = null;

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

        //private void LoadCampaignInfo(int reportType)
        //{
        //    try
        //    {
        //        SetCursor(Cursors.Wait);
        //        if (_reportTypeID == 1)//base
        //        {
        //            DataTable dt = Methods.GetTableData("SELECT ID [CampaignTypeID], Description [CampaignType] FROM lkpINCampaignType where ID NOT IN (4,10) ");
        //            DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
        //            dt.Columns.Add(column);
        //            dt.DefaultView.Sort = "CampaignType ASC";
        //            xdgCampaigns.DataSource = dt.DefaultView;
        //            xdgCampaigns.IsEnabled = true;
        //            lblMonth.Visibility = Visibility.Collapsed;
        //            lblYear.Visibility = Visibility.Collapsed;
        //            cmbMonth.Visibility = Visibility.Collapsed;
        //            clYear.Visibility = Visibility.Collapsed;
        //            lblIncludeLeadsCopiedToExtension.Visibility = Visibility.Visible;
        //            chkIncludeLeadsCopiedToExtension.Visibility = Visibility.Visible;

        //            lblCal1.Visibility = Visibility.Visible;
        //            calStartDate.Visibility = Visibility.Visible;
        //            calEndDate.Visibility = Visibility.Visible;

        //        }
        //        if (_reportTypeID == 2)//upgrade
        //        {
        //            DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign WHERE FKINCampaignGroupID IN (7,8,9,10,11,12,13,14,15,16,17,18,19,20)");
        //            DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
        //            dt.Columns.Add(column);
        //            dt.DefaultView.Sort = "CampaignName ASC";
        //            xdgCampaigns.DataSource = dt.DefaultView;
        //            xdgCampaigns.IsEnabled = false;
        //            HeaderPrefixAreaCheckbox_Checked(xdgCampaigns, new RoutedEventArgs());
        //            lblMonth.Visibility = Visibility.Visible;
        //            lblYear.Visibility = Visibility.Visible;
        //            cmbMonth.Visibility = Visibility.Visible;
        //            clYear.Visibility = Visibility.Visible;
        //            lblIncludeLeadsCopiedToExtension.Visibility = Visibility.Collapsed;
        //            chkIncludeLeadsCopiedToExtension.Visibility = Visibility.Collapsed;

        //            lblCal1.Visibility = Visibility.Collapsed;
        //            calStartDate.Visibility = Visibility.Collapsed;
        //            calEndDate.Visibility = Visibility.Collapsed;
        //        }
        //        if (_reportTypeID == 3) //extension
        //        {
        //            DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign WHERE FKINCampaignGroupID IN (22)");
        //            DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
        //            dt.Columns.Add(column);
        //            dt.DefaultView.Sort = "CampaignName ASC";
        //            xdgCampaigns.DataSource = dt.DefaultView;
        //            xdgCampaigns.IsEnabled = true;
        //            lblMonth.Visibility = Visibility.Collapsed;
        //            lblYear.Visibility = Visibility.Collapsed;
        //            cmbMonth.Visibility = Visibility.Collapsed;
        //            clYear.Visibility = Visibility.Collapsed;
        //            lblIncludeLeadsCopiedToExtension.Visibility = Visibility.Collapsed;
        //            chkIncludeLeadsCopiedToExtension.Visibility = Visibility.Collapsed;

        //            lblCal1.Visibility = Visibility.Collapsed;
        //            calStartDate.Visibility = Visibility.Collapsed;
        //            calEndDate.Visibility = Visibility.Collapsed;
        //        }
        //        if ((_reportTypeID == 4) || (_reportTypeID == 5) || (_reportTypeID == 6) || _reportTypeID == 7) //renewals - re-defrost - re-activations - re-activations
        //        {
        //            if (_reportTypeID == 4)//renewals
        //            {
        //                DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign WHERE FKINCampaignGroupID IN (6) ");
        //                DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
        //                dt.Columns.Add(column);
        //                dt.DefaultView.Sort = "CampaignName ASC";
        //                xdgCampaigns.DataSource = dt.DefaultView;
        //            }
        //            if (_reportTypeID == 5)//re-defrost
        //            {
        //                DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign WHERE FKINCampaignGroupID IN (4,24) ");
        //                DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
        //                dt.Columns.Add(column);
        //                dt.DefaultView.Sort = "CampaignName ASC";
        //                xdgCampaigns.DataSource = dt.DefaultView;
        //            }
        //            if (_reportTypeID == 6)//re-activations
        //            {
        //                DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign WHERE FKINCampaignGroupID IN (21) ");
        //                DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
        //                dt.Columns.Add(column);
        //                dt.DefaultView.Sort = "CampaignName ASC";
        //                xdgCampaigns.DataSource = dt.DefaultView;
        //            }
        //            if (_reportTypeID == 7)//re-activations
        //            {
        //                DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign WHERE FKINCampaignGroupID IN (3) ");
        //                DataColumn column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
        //                dt.Columns.Add(column);
        //                dt.DefaultView.Sort = "CampaignName ASC";
        //                xdgCampaigns.DataSource = dt.DefaultView;
        //            }

        //            xdgCampaigns.IsEnabled = true;
        //            lblMonth.Visibility = Visibility.Collapsed;
        //            lblYear.Visibility = Visibility.Collapsed;
        //            cmbMonth.Visibility = Visibility.Collapsed;
        //            clYear.Visibility = Visibility.Collapsed;
        //            lblIncludeLeadsCopiedToExtension.Visibility = Visibility.Visible;
        //            chkIncludeLeadsCopiedToExtension.Visibility = Visibility.Visible;

        //            lblCal1.Visibility = Visibility.Visible;
        //            calStartDate.Visibility = Visibility.Visible;
        //            calEndDate.Visibility = Visibility.Visible;
        //        }

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
            //calStartDate.IsEnabled = true;
            //calEndDate.IsEnabled = true;

            if (_reportTypeID != 2)
            {
                xdgCampaigns.IsEnabled = true;
            }

            //if (_reportTypeID == 1 || _reportTypeID == 4 || _reportTypeID == 5 || _reportTypeID == 6 || _reportTypeID == 7)
            //{
            //    xdgCampaigns.IsEnabled = true;
            //}
            //if (_reportTypeID == 3)
            //{
            //    xdgCampaigns.IsEnabled = true;
            //}
        }

        #region OLD

        //private void ReportOLD(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        SetCursor(Cursors.Wait);
        //        IEnumerable<DataRecord> campaigns = e.Argument as IEnumerable<DataRecord>;
        //        Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
        //        string fileName = string.Empty;
        //        string campaignIDs = string.Empty;
        //        bool isFirst = true;
        //        if (campaigns != null)
        //        {
        //            foreach (DataRecord record in campaigns)
        //            {
        //                if ((bool)record.Cells["Select"].Value)
        //                {
        //                    long campaignID = -1;
        //                    if (_reportTypeID == 1)
        //                    {
        //                        campaignID = Convert.ToInt32(record.Cells["CampaignTypeID"].Value);
        //                    }
        //                    if (_reportTypeID == 2)
        //                    {
        //                        campaignID = Convert.ToInt32(record.Cells["CampaignID"].Value);
        //                    }
        //                    if (_reportTypeID == 3)
        //                    {
        //                        campaignID = Convert.ToInt32(record.Cells["CampaignID"].Value);
        //                    }
        //                    if (_reportTypeID == 4 || _reportTypeID == 5 || _reportTypeID == 6 || _reportTypeID == 7)
        //                    {
        //                        campaignID = Convert.ToInt32(record.Cells["CampaignID"].Value);
        //                    }
        //                    if (isFirst == true)
        //                    {
        //                        isFirst = false;
        //                        campaignIDs = campaignID.ToString();
        //                    }
        //                    else
        //                    {
        //                        campaignIDs = campaignIDs + "," + campaignID;
        //                    }
        //                }
        //            }
        //            //setup excel documents
        //            if (campaignIDs != string.Empty)
        //            {
        //                Workbook wbTemplate;

        //                string filePathAndName = GlobalSettings.UserFolder + " Campaign Fall Off  Report ~ " + DateTime.Now.Millisecond + ".xlsx";
        //                Uri uri = new Uri("/Templates/ReportTemplateBatchReport.xlsx", UriKind.Relative);
        //                if (_reportTypeID == 1 || _reportTypeID == 4 || _reportTypeID == 5 || _reportTypeID == 6 || _reportTypeID == 7)
        //                {
        //                    uri = new Uri("/Templates/ReportTemplateBatchReport.xlsx", UriKind.Relative);
        //                }

        //                if (_reportTypeID == 2)
        //                {
        //                    uri = new Uri("/Templates/ReportTemplateBatchReportUpgrades.xlsx", UriKind.Relative);
        //                }

        //                if (_reportTypeID == 3)
        //                {
        //                    uri = new Uri("/Templates/ReportTemplateBatchReportExtensions.xlsx", UriKind.Relative);
        //                }

        //                StreamResourceInfo info = Application.GetResourceStream(uri);
        //                if (info != null)
        //                {
        //                    wbTemplate = Workbook.Load(info.Stream, true);
        //                }
        //                else
        //                {
        //                    return;
        //                }
        //                Worksheet wsTemplate = wbTemplate.Worksheets["Sheet1"];
        //                //report data
        //                DataTable dtLeadAllocationData;
        //                SqlParameter[] parameters = new SqlParameter[6];

        //                DateTime fromDate = new DateTime(_year, _month, 1);
        //                fromDate = fromDate.AddMonths(-2);
        //                int fromMonth = fromDate.Month;
        //                int lastDay = DateTime.DaysInMonth(_year, _month);
        //                DateTime toDate = new DateTime(_year, _month, lastDay);
        //                int toMonth = toDate.Month;
        //                if (!campaignIDs.Contains(","))
        //                {
        //                    campaignIDs = campaignIDs + ",";
        //                }
        //                if (_reportTypeID == 1 || _reportTypeID == 4 || _reportTypeID == 5 || _reportTypeID == 6 || _reportTypeID == 7)
        //                {
        //                    fromDate = _startDate;
        //                    toDate = _endDate;
        //                }

        //                parameters[0] = new SqlParameter("@CampaignId", campaignIDs);
        //                parameters[1] = new SqlParameter("@FromDate", fromDate.ToString("yyyy-MM-dd"));
        //                parameters[2] = new SqlParameter("@ToDate", toDate.ToString("yyyy-MM-dd"));
        //                parameters[3] = new SqlParameter("@IncludeLeadsCopiedToExtension", _includeLeadsCopiedToExtension);
        //                parameters[4] = new SqlParameter("@ReportType", _reportTypeID);
        //                parameters[5] = new SqlParameter("@IncludeCompleted", _includeCompletedBatches);
        //                DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spINReportBatch", parameters);

        //                if (dsLeadAllocationData.Tables.Count > 0)
        //                {
        //                    dtLeadAllocationData = dsLeadAllocationData.Tables[0];

        //                    if (dtLeadAllocationData.Rows.Count == 0)
        //                    {
        //                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
        //                        {
        //                            ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected Campaigns and specified Date range.", "No Data", ShowMessageType.Information);

        //                        });

        //                        return;
        //                    }
        //                }
        //                else
        //                {
        //                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
        //                    {
        //                        ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected Campaigns and specified Date range.", "No Data", ShowMessageType.Information);
        //                    });
        //                    return;
        //                }

        //                Worksheet wsReport = wbReport.Worksheets.Add("Batch Report");
        //                wsReport.PrintOptions.PaperSize = PaperSize.A4;
        //                wsReport.PrintOptions.Orientation = Orientation.Portrait;
        //                int rowIndex = 4;

        //                #region Default Report

        //                if (_reportTypeID == 1 || _reportTypeID == 4 || _reportTypeID == 5 || _reportTypeID == 6 || _reportTypeID == 7)
        //                {
        //                    wsReport.GetCell("A" + 1).Value = dtLeadAllocationData.Rows[0]["Campaign"] + " Batch Report - " + DateTime.Now.ToShortDateString();
        //                    wsReport.GetCell("A" + 2).Value = "Compliled By:  " + Methods.GetTableData("select FirstName + ' ' + LastName as CompiledBy from dbo.[User] where ID = " + GlobalSettings.ApplicationUser.ID).Rows[0]["CompiledBy"];
        //                    Methods.CopyExcelRegion(wsTemplate, 0, 0, dtLeadAllocationData.Rows.Count, 22, wsReport, 2, 0);

        //                    foreach (DataRow rw in dtLeadAllocationData.Rows)
        //                    {
        //                        if (rw["Campaign"].ToString().Substring(0, 5).ToLower() == "total")
        //                        {
        //                            //format total cells
        //                            wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("C" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("D" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("E" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("K" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("O" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("S" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("T" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
        //                            wsReport.GetCell("U" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);

        //                            wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("C" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("D" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("E" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("K" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("O" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("S" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("T" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("U" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("V" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        }

        //                        wsReport.GetCell("A" + rowIndex.ToString()).Value = rw["Campaign"];
        //                        wsReport.GetCell("B" + rowIndex.ToString()).Value = rw["Batch"];
        //                        wsReport.GetCell("C" + rowIndex.ToString()).Value = rw["CurrentWeek"];
        //                        wsReport.GetCell("D" + rowIndex.ToString()).Value = rw["CurrentWeekOnFloor"];
        //                        wsReport.GetCell("E" + rowIndex.ToString()).Value = rw["DateSent"];
        //                        wsReport.GetCell("F" + rowIndex.ToString()).Value = rw["ExpiryDate"];
        //                        wsReport.GetCell("G" + rowIndex.ToString()).Value = rw["#Leads"];
        //                        wsReport.GetCell("H" + rowIndex.ToString()).Value = rw["Target%"];
        //                        wsReport.GetCell("I" + rowIndex.ToString()).Value = rw["TargetSales"];
        //                        wsReport.GetCell("J" + rowIndex.ToString()).Value = rw["TotalSales"];
        //                        wsReport.GetCell("K" + rowIndex.ToString()).Value = rw["CurrentConversion"];
        //                        wsReport.GetCell("L" + rowIndex.ToString()).Value = rw["SalesToGo"];
        //                        wsReport.GetCell("M" + rowIndex.ToString()).Value = rw["Declines"];
        //                        wsReport.GetCell("N" + rowIndex.ToString()).Value = rw["%Declines"];
        //                        wsReport.GetCell("O" + rowIndex.ToString()).Value = rw["StillToContact"];
        //                        wsReport.GetCell("P" + rowIndex.ToString()).Value = rw["Diary"];
        //                        wsReport.GetCell("Q" + rowIndex.ToString()).Value = rw["PNA"];
        //                        wsReport.GetCell("R" + rowIndex.ToString()).Value = rw["Contact%"];

        //                        string fad = rw["FAD"].ToString();
        //                        if (fad == string.Empty && rw["Campaign"].ToString().Substring(0, 5).ToLower() != "total")
        //                        {
        //                            fad = "NFA";//not fully allocated
        //                        }
        //                        else
        //                        {
        //                            if (fad != string.Empty)
        //                            {
        //                                fad = rw["FAD"].ToString().Substring(0, 10);
        //                            }
        //                        }
        //                        wsReport.GetCell("S" + rowIndex.ToString()).Value = fad;
        //                        wsReport.GetCell("T" + rowIndex.ToString()).Value = rw["ConversionAcheived"];
        //                        wsReport.GetCell("U" + rowIndex.ToString()).Value = rw["BonusAcheived"];
        //                        wsReport.GetCell("V" + rowIndex.ToString()).Value = rw["Status&Comments"];
        //                        rowIndex++;
        //                    }
        //                }

        //                #endregion Default Report

        //                #region Upgrade Report

        //                if (_reportTypeID == 2)
        //                {
        //                    rowIndex = 4;
        //                    wsReport.GetCell("A" + 1).Value = " Upgrade Batch Report - " + DateTime.Now.ToShortDateString();
        //                    wsReport.GetCell("A" + 2).Value = "Compliled by " + Methods.GetTableData("select FirstName + ' ' + LastName as CompiledBy from dbo.[User] where ID = " + GlobalSettings.ApplicationUser.ID).Rows[0]["CompiledBy"];
        //                    Methods.CopyExcelRegion(wsTemplate, 0, 0, dtLeadAllocationData.Rows.Count, 36, wsReport, 2, 0);
        //                    System.Globalization.DateTimeFormatInfo fmi = new System.Globalization.DateTimeFormatInfo();

        //                    wsReport.GetCell("B" + (rowIndex - 1).ToString()).Value = fmi.GetMonthName(fromMonth) + " Bonus Value";
        //                    wsReport.GetCell("N" + (rowIndex - 1).ToString()).Value = fmi.GetMonthName(fromMonth + 1) + " Bonus Value";
        //                    wsReport.GetCell("Z" + (rowIndex - 1).ToString()).Value = fmi.GetMonthName(toMonth) + " Bonus Value";

        //                    foreach (DataRow rw in dtLeadAllocationData.Rows)
        //                    {
        //                        if (rw["CampaignCode"].ToString().ToLower() == "rowbreak")
        //                        {
        //                            //format total cells
        //                            wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("C" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("D" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("E" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("K" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("O" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("S" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("T" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("U" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("V" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("W" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("X" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("Y" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("Z" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("AA" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("AB" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("AC" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("AD" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("AE" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("AF" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            //Additions by Pheko
        //                            wsReport.GetCell("AG" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("AH" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("AI" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("AJ" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                            wsReport.GetCell("AK" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.PeachPuff);
        //                        }
        //                        else
        //                        {
        //                            if (rw["CampaignCode"].ToString().ToLower() == "totals")
        //                            {
        //                                wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("C" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("D" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("E" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("K" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("O" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("S" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("T" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("U" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("V" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("W" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("X" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("Y" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("Z" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("AA" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("AB" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("AC" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("AD" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("AE" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("AF" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("AG" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("AH" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("AI" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("AJ" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                                wsReport.GetCell("AK" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            }

        //                            wsReport.GetCell("A" + rowIndex.ToString()).Value = rw["CampaignCode"];
        //                            wsReport.GetCell("B" + rowIndex.ToString()).Value = "R " + rw["Month1BonusValue"];
        //                            wsReport.GetCell("C" + rowIndex.ToString()).Value = rw["Month1BatchNumber"];
        //                            wsReport.GetCell("D" + rowIndex.ToString()).Value = rw["Month1TargetStl"];
        //                            wsReport.GetCell("E" + rowIndex.ToString()).Value = rw["Month1NumberOfLeads"];
        //                            wsReport.GetCell("F" + rowIndex.ToString()).Value = rw["Month1TargetSales"];
        //                            wsReport.GetCell("G" + rowIndex.ToString()).Value = rw["Month1CurrentSales"];
        //                            wsReport.GetCell("H" + rowIndex.ToString()).Value = rw["Month1CurrentSTLPercentage"];
        //                            wsReport.GetCell("I" + rowIndex.ToString()).Value = rw["Month1ToDo"];
        //                            string convDateMonth1 = rw["Month1DateConverted"].ToString();
        //                            if (convDateMonth1 != string.Empty)
        //                            {
        //                                convDateMonth1 = DateTime.Parse(convDateMonth1).ToShortDateString();
        //                            }
        //                            wsReport.GetCell("L" + rowIndex.ToString()).Value = convDateMonth1;
        //                            //Pheko Modifications

        //                            //Still to contact Batch 1
        //                            wsReport.GetCell("J" + rowIndex.ToString()).Value = rw["StillToContact1"];

        //                            wsReport.GetCell("K" + rowIndex.ToString()).Value = rw["StatusMonth1"];
        //                            wsReport.GetCell("M" + rowIndex.ToString()).Value = string.Empty;

        //                            wsReport.GetCell("N" + rowIndex.ToString()).Value = "R " + rw["Month2BonusValue"];
        //                            wsReport.GetCell("O" + rowIndex.ToString()).Value = rw["Month2BatchNumber"];
        //                            wsReport.GetCell("P" + rowIndex.ToString()).Value = rw["Month2TargetStl"];
        //                            wsReport.GetCell("Q" + rowIndex.ToString()).Value = rw["Month2NumberOfLeads"];
        //                            wsReport.GetCell("R" + rowIndex.ToString()).Value = rw["Month2TargetSales"];
        //                            wsReport.GetCell("S" + rowIndex.ToString()).Value = rw["Month2CurrentSales"];
        //                            wsReport.GetCell("T" + rowIndex.ToString()).Value = rw["Month2CurrentSTLPercentage"];
        //                            wsReport.GetCell("U" + rowIndex.ToString()).Value = rw["Month2ToDo"];

        //                            string convDateMonth2 = rw["Month2DateConverted"].ToString();
        //                            if (convDateMonth2 != string.Empty)
        //                            {
        //                                convDateMonth2 = DateTime.Parse(convDateMonth2).ToShortDateString();
        //                            }

        //                            //Still To Contact 2
        //                            wsReport.GetCell("V" + rowIndex.ToString()).Value = rw["StillToContact2"];
        //                            wsReport.GetCell("W" + rowIndex.ToString()).Value = rw["StatusMonth2"];
        //                            wsReport.GetCell("X" + rowIndex.ToString()).Value = convDateMonth2;
        //                            wsReport.GetCell("Y" + rowIndex.ToString()).Value = string.Empty;

        //                            var test = rw["Month3BonusValue"];
        //                            wsReport.GetCell("Z" + rowIndex.ToString()).Value = "R " + rw["Month3BonusValue"];

        //                            wsReport.GetCell("AA" + rowIndex.ToString()).Value = rw["Month3BatchNumber"];
        //                            wsReport.GetCell("AB" + rowIndex.ToString()).Value = rw["Month3TargetStl"];
        //                            wsReport.GetCell("AC" + rowIndex.ToString()).Value = rw["Month3NumberOfLeads"];
        //                            wsReport.GetCell("AD" + rowIndex.ToString()).Value = rw["Month3TargetSales"];
        //                            wsReport.GetCell("AE" + rowIndex.ToString()).Value = rw["Month3CurrentSales"];
        //                            wsReport.GetCell("AF" + rowIndex.ToString()).Value = rw["Month3CurrentSTLPercentage"];
        //                            string convDateMonth3 = rw["Month3DateConverted"].ToString();
        //                            if (convDateMonth3 != string.Empty)
        //                            {
        //                                convDateMonth3 = DateTime.Parse(convDateMonth3).ToShortDateString();
        //                            }
        //                            wsReport.GetCell("AG" + rowIndex.ToString()).Value = rw["Month3ToDo"];
        //                            //StillTo Contact
        //                            wsReport.GetCell("AH" + rowIndex.ToString()).Value = rw["StillToContact3"];
        //                            wsReport.GetCell("AI" + rowIndex.ToString()).Value = rw["StatusMonth3"];
        //                            wsReport.GetCell("AJ" + rowIndex.ToString()).Value = convDateMonth3;
        //                            wsReport.GetCell("AK" + rowIndex.ToString()).Value = string.Empty;
        //                        }
        //                        rowIndex++;
        //                    }
        //                }

        //                #endregion Upgrade Report

        //                #region Extension Report

        //                if (_reportTypeID == 3)
        //                {
        //                    Methods.CopyExcelRegion(wsTemplate, 0, 0, dtLeadAllocationData.Rows.Count, 28, wsReport, 2, 0);
        //                    System.Globalization.DateTimeFormatInfo fmi = new System.Globalization.DateTimeFormatInfo();
        //                    wsReport.GetCell("A" + 1).Value = dtLeadAllocationData.Rows[0]["Campaign"] + " Batch Report - " + DateTime.Now.ToShortDateString();
        //                    wsReport.GetCell("A" + 2).Value = "Compiled By " + Methods.GetTableData("select FirstName + ' ' + LastName as CompiledBy from dbo.[User] where ID = " + GlobalSettings.ApplicationUser.ID).Rows[0]["CompiledBy"];

        //                    foreach (DataRow rw in dtLeadAllocationData.Rows)
        //                    {
        //                        if (rw["DateRenewed"].ToString().ToLower() == "totals")
        //                        {
        //                            wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("C" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("D" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("E" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("K" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                            wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
        //                        }
        //                        wsReport.GetCell("A" + rowIndex.ToString()).Value = rw["Batch#"];
        //                        wsReport.GetCell("B" + rowIndex.ToString()).Value = rw["DateOriginallyReceived"];
        //                        wsReport.GetCell("C" + rowIndex.ToString()).Value = rw["DateRenewed"];
        //                        wsReport.GetCell("D" + rowIndex.ToString()).Value = rw["#Leads"];
        //                        wsReport.GetCell("E" + rowIndex.ToString()).Value = rw["Contacts"];
        //                        wsReport.GetCell("F" + rowIndex.ToString()).Value = rw["%ContactToLeads"];
        //                        wsReport.GetCell("G" + rowIndex.ToString()).Value = rw["Sales"];
        //                        wsReport.GetCell("H" + rowIndex.ToString()).Value = rw["%YesToContact"];
        //                        wsReport.GetCell("I" + rowIndex.ToString()).Value = rw["%YesToLead"];
        //                        wsReport.GetCell("J" + rowIndex.ToString()).Value = rw["Declines"];
        //                        wsReport.GetCell("K" + rowIndex.ToString()).Value = rw["%DeclineToContact"];
        //                        wsReport.GetCell("L" + rowIndex.ToString()).Value = rw["%DeclineToLeads"];
        //                        wsReport.GetCell("M" + rowIndex.ToString()).Value = rw["Contact%"];
        //                        wsReport.GetCell("N" + rowIndex.ToString()).Value = rw["Comments"];
        //                        rowIndex++;
        //                    }
        //                }

        //                #endregion Extension Report

        //                string fName = GlobalSettings.UserFolder + " Batch  Report ~ " + DateTime.Now.Millisecond + ".xlsx";
        //                wbReport.Save(fName);
        //                //Display excel document                           
        //                Process.Start(fName);
        //            }
        //        }
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

        private void ReportConversion(object sender, DoWorkEventArgs e)
        {
            try
            {
                DataSet dsConversionReportData = Insure.INGetConversionReportData(_campaignIDs, RData.SalesConversionPerBatch, RData.ContactsConversionPerBatch);
                #region Determine the campaign partitioning



                //List<string> generalCampaigns =
                //    (
                //        from row
                //        in dsConversionReportData.Tables[0].AsEnumerable()
                //        select row.Field<string>("CampaignName")
                //    ).Distinct().ToList();



                #endregion Determine the campaign partitioning

                SetCursor(Cursors.Wait);

                #region Setup excel workbook

                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateBatchReport.xlsx");
                Worksheet wsTemplate = wbTemplate.Worksheets["SalesAndContactsConversion"];

                #endregion Setup excel workbook

                #region Get the report data
                //DataTable dtReportConfigs = dsConversionReportData.Tables[0];
                //DataTable dtCampaigns = dsInvalidAccountsReportData.Tables[1];
                //DataTable dtTSRDiaries = dsInvalidAccountsReportData.Tables[2];
                DataTable dtReportConfigs = dsConversionReportData.Tables[0];
                DataTable dtColumnMappings = dsConversionReportData.Tables[3];
                DataTable dtTotalsColumnMappings = dsConversionReportData.Tables[4];
                DataTable dtTemplateSettings = dsConversionReportData.Tables[5];
                //DataTable dtReportConfigs = dsSalesTrackingReportData.Tables[4];

                #endregion Get the report data

                #region Variable declarations

                string filePathAndName = String.Empty;

                string campaign = String.Empty;
                string campaignFilterString = String.Empty;
                string currentTSR = String.Empty;
                string currentTSRFilterString = String.Empty;

                byte templateColumnSpan = byte.Parse(dtTemplateSettings.Rows[0]["TemplateColumnSpan"].ToString());
                byte templateRowSpan = byte.Parse(dtTemplateSettings.Rows[0]["TemplateRowSpan"].ToString());
                byte templateDataRowIndex = byte.Parse(dtTemplateSettings.Rows[0]["TemplateDataRowIndex"].ToString());




                #endregion Variable declarations

                #region Instantiate a new Excel workbook

                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                if (RData.SalesConversionPerBatch)
                {
                    filePathAndName = String.Format("{0} Sales Conversion Report ~ {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
                }
                else
                {
                    filePathAndName = String.Format("{0} Contacts Conversion Report ~ {1}.xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
                }

                #endregion Instantiate a new Excel workbook



                foreach (DataRow row in dtReportConfigs.Rows)
                {


                    DataTable dtMainReportData = dsConversionReportData.Tables[1].Select(row["FilterString"].ToString()).CopyToDataTable();/*.OrderBy(row["OrderByString"].ToString());*/
                    DataTable dtTotalsReportData = dsConversionReportData.Tables[2].Select(row["FilterString"].ToString()).CopyToDataTable(); ;
                    #region Loop through each campaign and create a new workbook for each                   
                    int reportRow = byte.Parse(dtTemplateSettings.Rows[0]["ReportRow"].ToString());

                    #region Add the current sales consultant's worksheet

                    Worksheet wsReport = wbReport.Worksheets.Add(Methods.ParseWorksheetName(wbReport, row["CampaignCode"].ToString()));
                    Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                    #endregion Add the current sales consultant's worksheet

                    #region Insert the column headings and populate the details

                    Methods.CopyExcelRegion(wsTemplate, 0, 0, templateRowSpan, templateColumnSpan, wsReport, 0, 0);

                    wsReport.GetCell(dtTemplateSettings.Rows[0]["TitleCell"].ToString()).Value = row["CampaignName"];

                    wsReport.GetCell(dtTemplateSettings.Rows[0]["SubTitle1Cell"].ToString()).Value = row["SubTitle1"];

                    wsReport.GetCell(dtTemplateSettings.Rows[0]["SubTitle2Cell"].ToString()).Value = row["SubTitle2"];

                    //wsReport.GetCell("A5").Value = dtReportConfigs.Rows[0]["MonthYearRange"];

                    #endregion Insert the column headings and populate the details

                    #region Add the data

                    reportRow = Methods.MapTemplatizedExcelValues(wsTemplate, dtMainReportData, dtColumnMappings, templateDataRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                    //Totals

                    Methods.MapTemplatizedExcelValues(wsTemplate, dtTotalsReportData, dtTotalsColumnMappings, templateDataRowIndex + 1, 0, 0, templateColumnSpan, wsReport, reportRow, 0);


                    #endregion Add the data

                    #endregion Loop through each campaign and create a new workbook for each

                }

                #region Save and open the resulting workbook

                if (wbReport.Worksheets.Count > 0)
                {
                    //Save excel document
                    wbReport.Save(filePathAndName);

                    //Display excel document
                    Process.Start(filePathAndName);
                }
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

        private void ReportCompletedConversion(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";

            btnReport.IsEnabled = true;
            btnClose.IsEnabled = true;
            //calStartDate.IsEnabled = true;
            //calEndDate.IsEnabled = true;

            if (_reportTypeID != 2)
            {
                xdgCampaigns.IsEnabled = true;
            }

            //if (_reportTypeID == 1 || _reportTypeID == 4 || _reportTypeID == 5 || _reportTypeID == 6 || _reportTypeID == 7)
            //{
            //    xdgCampaigns.IsEnabled = true;
            //}
            //if (_reportTypeID == 3)
            //{
            //    xdgCampaigns.IsEnabled = true;
            //}
        }
        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                string columnsToBeHidden = "";
                SetCursor(Cursors.Wait);

                #region Get the data from the database

                //DataSet dsBatchReportData = Insure.INGetBatchReportData(_campaignIDs, _startDate, _endDate, _reportTypeID, _includeLeadsCopiedToExtension, _includeCompletedBatches);
                DataSet dsBatchReportData = Insure.INGetBatchReportData(_campaignIDs, _year, _month, _reportTypeID, _includeLeadsCopiedToExtension, _includeCompletedBatches, _onlyBatchesReceived91DaysAgoAndAfter, RData.CombineUL);
                DataTable dtBatchReportData;

                #endregion Get the data from the database

                #region Determine if there is any data available

                #region How many data tables should the DataSet have?

                byte dataTableCount = 0;

                switch (_reportTypeID)
                {
                    case 1:
                        columnsToBeHidden = "0,4,8,16,18,19,25";
                        dataTableCount = 6;
                        break;

                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        //columnsToBeHidden = "0,4,8,15,17,18,24";
                        columnsToBeHidden = "0,4,8,16,18,19,25";
                        dataTableCount = 6;
                        break;

                    case 2:
                        dataTableCount = 6;
                        break;

                    case 3:
                        columnsToBeHidden = "0";
                        dataTableCount = 6;
                        break;
                }

                #endregion How many data tables should the DataSet have?

                // The resulting DataSet must consist of DataTables
                if (dsBatchReportData.Tables.Count < dataTableCount)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"There is no data for the selected report type, campaign selections and/or date range.", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                dtBatchReportData = dsBatchReportData.Tables[1];

                if (dtBatchReportData.Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"There is no data for the selected report type, campaign selections and/or date range.", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                #endregion Determine if there is any data available

                #region Get the report config values

                DataTable dtReportConfigs = dsBatchReportData.Tables[0];

                string templateWorkbookName = dtReportConfigs.Rows[0]["TemplateWorkbookName"].ToString();
                string reportTemplateWorksheetName = dtReportConfigs.Rows[0]["ReportTemplateWorksheetName"].ToString();
                byte templateColumnSpan = Convert.ToByte(dtReportConfigs.Rows[0]["TemplateColumnSpan"]);
                byte reportHeadingRowSpan = Convert.ToByte(dtReportConfigs.Rows[0]["ReportHeadingRowSpan"]);
                string reportSubHeadingCell = dtReportConfigs.Rows[0]["ReportSubHeadingCell"].ToString();
                string reportAuthorCell = dtReportConfigs.Rows[0]["ReportAuthorCell"].ToString();
                string reportAuthor = dtReportConfigs.Rows[0]["ReportAuthor"].ToString();
                string reportDateCell = dtReportConfigs.Rows[0]["ReportDateCell"].ToString();
                string reportSubHeading = dtReportConfigs.Rows[0]["ReportSubHeading"].ToString();

                #endregion Get the report config values

                #region Setup the Excel workbook

                Workbook wbTemplate = Methods.DefineTemplateWorkbook(String.Format("/Templates/{0}", templateWorkbookName));
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                Worksheet wsTemplate = wbTemplate.Worksheets[reportTemplateWorksheetName];

                #endregion Setup the Excel workbook

                #region Declarations & Initializations

                int reportRow = 7;
                string grandTotalFormula = String.Empty;
                string fileName = String.Format("{0}Batch Report - {1} ~ {2}.xlsx", GlobalSettings.UserFolder, _selectedReportType, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                #endregion Declarations & Initializations

                if (_reportTypeID == 2)
                {
                    #region Add the worksheet

                    Worksheet wsReport = wbReport.Worksheets.Add("Upgrade Batch Report");
                    Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                    #endregion Add the worksheet

                    #region Insert the report heading with column headings and populate the report details

                    Methods.CopyExcelRegion(wsTemplate, 0, 0, reportHeadingRowSpan, templateColumnSpan, wsReport, 0, 0);

                    wsReport.GetCell("A1").Value = String.Format("Batch Report - {0}", _selectedReportType);
                    wsReport.GetCell(reportSubHeadingCell).Value = reportSubHeading;
                    wsReport.GetCell(reportAuthorCell).Value = reportAuthor;
                    wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    #endregion Insert the report heading with column headings and populate the report details

                    #region Adding the report data

                    InsertUpgradeBatchReportData(wsTemplate, wsReport, dsBatchReportData, templateColumnSpan, reportRow);

                    #endregion Adding the report data
                }

                else
                {
                    #region Determine the campaign partitioningN

                    List<string> generalCampaigns =
                    (
                        from row
                        in dsBatchReportData.Tables[1].AsEnumerable()
                        select row.Field<string>("GeneralCampaign")
                    ).Distinct().ToList();

                    #endregion Determine the campaign partitioning

                    foreach (string generalCampaign in generalCampaigns)
                    {
                        if (generalCampaign != null)
                        {

                            DataTable dtCurrentReportSegmentData = dsBatchReportData.Tables[1].Select(String.Format("[GeneralCampaign] = '{0}'", generalCampaign)).CopyToDataTable();

                            #region Determine the earliest stamp date, which will be the from date

                            List<DateTime> batchStampDates =
                            (
                                from row
                                in dtCurrentReportSegmentData.AsEnumerable()
                                select row.Field<DateTime>("StampDate")
                            ).Distinct().ToList();

                            DateTime earliestBatchStampDate = batchStampDates.Min();

                            #endregion Determine the earliest stamp date, which will be the from date

                            #region Add the worksheet

                            string newWorksheetName = Methods.ParseWorksheetName(wbReport, generalCampaign);
                            Worksheet wsReport = wbReport.Worksheets.Add(newWorksheetName);
                            Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                            #endregion Add the worksheet

                            #region Insert the report heading with column headings and populate the report details

                            Methods.CopyExcelRegion(wsTemplate, 0, 0, reportHeadingRowSpan, templateColumnSpan, wsReport, 0, 0);

                            wsReport.GetCell("A1").Value = String.Format("Batch Report - {0}", generalCampaign);

                            // Override the default report sub-heading
                            reportSubHeading = String.Format("For all batches received from Platinum Life between {0} and {1}.",
                                earliestBatchStampDate.ToString("d MMMM yyyy"),
                                DateTime.Now.ToString("d MMMM yyyy"));
                            wsReport.GetCell(reportSubHeadingCell).Value = reportSubHeading;
                            wsReport.GetCell(reportAuthorCell).Value = reportAuthor;
                            wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            #endregion Insert the report heading with column headings and populate the report details

                            #region Adding the report data

                            if (_reportTypeID == 12)
                            {
                                //InsertExtensionBatchReportData(wsTemplate, wsReport, dsBatchReportData, templateColumnSpan, reportRow);
                                #region Partition the given DataSet

                                InsertExtensionBatchReportData(wsTemplate, wsReport, dsBatchReportData.Tables[0].Rows[0], dtCurrentReportSegmentData, dsBatchReportData.Tables[2], templateColumnSpan, reportRow);

                                #endregion Partition the given DataSet
                            }
                            else
                            {
                                #region Hiding all the unnecessary columns

                                if (!String.IsNullOrEmpty(columnsToBeHidden))
                                {
                                    string[] columns = columnsToBeHidden.Split(new char[] { ',' });

                                    foreach (string currentColumn in columns)
                                    {
                                        byte currentColumnIndex = Convert.ToByte(currentColumn);
                                        wsReport.Columns[currentColumnIndex].Hidden = true;
                                    }
                                }
                                #endregion
                                //InsertDefaultBatchReportData(wsTemplate, wsReport, dsBatchReportData, templateColumnSpan, reportRow);
                                #region Partition the given DataSet

                                var filteredRows = dsBatchReportData.Tables[2].Select(String.Format("[GeneralCampaign] = '{0}'", generalCampaign)).AsEnumerable();
                                if (filteredRows.Any())
                                {
                                    DataTable dtCurrentReportSegmentReceivedDates = dsBatchReportData.Tables[2].Select(String.Format("[GeneralCampaign] = '{0}'", generalCampaign)).CopyToDataTable();
                                    //if (_reportTypeID == 1)
                                    //{
                                    InsertDefaultBatchReportData(wsTemplate, wsReport, dsBatchReportData.Tables[0].Rows[0], dtCurrentReportSegmentData, dtCurrentReportSegmentReceivedDates, dsBatchReportData.Tables[3], templateColumnSpan, reportRow);
                                    //}
                                    //else
                                    //{
                                    //    InsertDefaultBatchReportDataOld(wsTemplate, wsReport, dsBatchReportData.Tables[0].Rows[0], dtCurrentReportSegmentData, dtCurrentReportSegmentReceivedDates, dsBatchReportData.Tables[3], templateColumnSpan, reportRow);
                                    //}

                                }

                                #endregion Partition the given DataSet
                            }

                            #endregion Adding the report data
                        }
                    }
                }

                #region Insert the Gifts sheet

                InsertBatchReportGiftsDataSheet(wbTemplate, wbReport, dsBatchReportData);

                #endregion Insert the Gifts sheet

                #region Hiding all the unnecessary columns



                #endregion Hiding all the unnecessary columns

                #region Save and open the resulting workbook

                if (wbReport.Worksheets.Count > 0)
                {
                    wbReport.Save(fileName);

                    //Display excel document                           
                    Process.Start(fileName);
                }

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

        private void InsertBatchReportGiftsDataSheet(Workbook wbTemplate, Workbook wbReport, DataSet dsBatchReportData)
        {

            if (dsBatchReportData.Tables[dsBatchReportData.Tables.Count - 2].Rows.Count == 0)
            {
                return;
            }

            #region Partition the given dataset

            DataTable dtReportConfigs = dsBatchReportData.Tables[0];
            DataTable dtBatchReportGiftData = dsBatchReportData.Tables[dsBatchReportData.Tables.Count - 2];
            DataTable dtColumnCellMappings = dsBatchReportData.Tables[dsBatchReportData.Tables.Count - 1];

            #endregion Partition the given dataset

            #region Add the worksheet

            Worksheet wsGiftsTemplate = wbTemplate.Worksheets["Gifts"];
            Worksheet wsGifts = wbReport.Worksheets.Add("Gifts");
            Methods.CopyWorksheetOptionsFromTemplate(wsGiftsTemplate, wsGifts, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

            #endregion Add the worksheet

            #region Definitions & Initializations

            int reportRow = 7;
            int templateRowIndex = 7;
            byte templateColumnSpan = 5;
            string reportAuthorCell = "B4";
            string reportDateCell = "B5";

            #endregion Definitions & Initializations

            #region Get the report configs

            string reportSubHeadingCell = dtReportConfigs.Rows[0]["ReportSubHeadingCell"].ToString();
            string reportAuthor = dtReportConfigs.Rows[0]["ReportAuthor"].ToString();
            string reportSubHeading = dtReportConfigs.Rows[0]["ReportSubHeading"].ToString();

            byte reportHeadingRowSpan = Convert.ToByte(dtReportConfigs.Rows[0]["ReportHeadingRowSpan"]);

            #endregion Get the report configs

            #region Determine the earliest stamp date, which will be the from date

            List<DateTime> batchStampDates =
            (
                from row
                in dtBatchReportGiftData.AsEnumerable()
                select row.Field<DateTime>("StampDate")
            ).Distinct().ToList();

            DateTime earliestBatchStampDate = batchStampDates.Min();

            #endregion Determine the earliest stamp date, which will be the from date

            #region Add the report details

            Methods.CopyExcelRegion(wsGiftsTemplate, 0, 0, reportHeadingRowSpan, templateColumnSpan, wsGifts, 0, 0);

            // Override the default report sub-heading
            reportSubHeading = String.Format("For all batches received from Platinum Life between {0} and {1}.",
                earliestBatchStampDate.ToString("d MMMM yyyy"),
                DateTime.Now.ToString("d MMMM yyyy"));
            wsGifts.GetCell(reportSubHeadingCell).Value = reportSubHeading;
            wsGifts.GetCell(reportAuthorCell).Value = reportAuthor;
            wsGifts.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            #endregion Add the report details

            #region Add the data

            reportRow = Methods.MapTemplatizedExcelValues(wsGiftsTemplate, dtBatchReportGiftData, dtColumnCellMappings, templateRowIndex, 0, 0, templateColumnSpan, wsGifts, reportRow, 0);

            #endregion Add the data

        }

        //private void InsertDefaultBatchReportData(Worksheet wsTemplate, Worksheet wsReport, DataSet dsReportData, byte templateRowSpan, int reportRow)
        private void InsertDefaultBatchReportData(Worksheet wsTemplate, Worksheet wsReport, DataRow drReportConfigs, DataTable dtReportData, DataTable dtBatchReceiveDates, DataTable dtColumnCellMappings, byte templateRowSpan, int reportRow)
        {
            #region Partition the given DataSet

            //DataRow drReportConfigs = dsReportData.Tables[0].Rows[0];
            //DataTable dtReportData = dsReportData.Tables[1];
            //DataTable dtBatchReceiveDates = dsReportData.Tables[2];
            //DataTable dtColumnCellMappings = dsReportData.Tables[3];

            #endregion Partition the given DataSet

            #region Declarations & Initializations

            string grandTotalFormulaBody = String.Empty;
            string subTotalFormulaBody = String.Empty;

            int segmentFromRow = 0;
            int segmentToRow = 0;

            #endregion Declarations & Initializations

            #region Get the configs

            byte reportDataTemplateRowIndex = Convert.ToByte(drReportConfigs["ReportDataTemplateRowIndex"]);
            byte templateColumnSpan = Convert.ToByte(drReportConfigs["TemplateColumnSpan"]);
            byte reportSegmentSeparatorTemplateRowIndex = Convert.ToByte(drReportConfigs["ReportSegmentSeparatorTemplateRowIndex"]);
            byte grandTotalTemplateRowIndex = Convert.ToByte(drReportConfigs["GrandTotalTemplateRowIndex"]);

            #endregion Get the configs

            for (int i = 0; i < dtBatchReceiveDates.Rows.Count; i++)
            {
                //DateTime dateReceivedFromBatchCode = Convert.ToDateTime(dtBatchReceiveDates.Rows[i]["DateReceivedFromBatchCode"]);
                //long fkCampaignID = Convert.ToInt64(dtBatchReceiveDates.Rows[i]["FKINCampaignID"]);
                //DataTable dtBatchesReceivedForDate = dtReportData.Select(String.Format("[DateReceivedFromBatchCode] = '{0}' AND [FKINCampaignID] = {1}",
                //    dateReceivedFromBatchCode.ToString("yyyy-MM-dd"),
                //    fkCampaignID)).CopyToDataTable();

                string reportSegmentFilterString = dtBatchReceiveDates.Rows[i]["ReportSegmentFilterString"].ToString();
                DataTable dtBatchesReceivedForDate = dtReportData.Select(reportSegmentFilterString).CopyToDataTable();

                segmentFromRow = reportRow;

                foreach (DataRow drCurrentBatch in dtBatchesReceivedForDate.Rows)
                {
                    Methods.CopyExcelRegion(wsTemplate, reportDataTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                    foreach (DataRow drColumnRowMapping in dtColumnCellMappings.Rows)
                    {
                        string excelColumn = drColumnRowMapping["ExcelColumn"].ToString();
                        string dataTableColumnNameOrFormula = drColumnRowMapping["DataTableColumnNameOrFormula"].ToString();

                        if (dataTableColumnNameOrFormula.StartsWith("="))
                        {
                            dataTableColumnNameOrFormula = dataTableColumnNameOrFormula.Replace("#ROW#", (reportRow + 1).ToString());
                            wsReport.GetCell(String.Format("{0}{1}", excelColumn, reportRow + 1)).ApplyFormula(dataTableColumnNameOrFormula);
                            if (excelColumn == "N")
                            {
                                //wsReport.GetCell(String.Format("{0}{1}", excelColumn, reportRow + 1)).CellFormat.FormatString = "0.0%";
                            }
                        }
                        else
                        {
                            wsReport.GetCell(String.Format("{0}{1}", excelColumn, reportRow + 1)).Value = drCurrentBatch[dataTableColumnNameOrFormula]; //dataTableColumnNameOrFormula;
                        }
                    }

                    segmentToRow = reportRow;
                    reportRow++;
                }

                #region Update the grand totals formula

                if (grandTotalFormulaBody.Length == 0)
                {
                    if (segmentFromRow == segmentToRow)
                    {
                        grandTotalFormulaBody = String.Format("_COLUMN_{0}", segmentToRow + 1);
                    }
                    else
                    {
                        grandTotalFormulaBody = String.Format("_COLUMN_{0}:_COLUMN_{1}", segmentFromRow + 1, segmentToRow + 1);
                    }
                }
                else
                {
                    if (segmentFromRow == segmentToRow)
                    {
                        grandTotalFormulaBody = String.Format("{0},_COLUMN_{1}", grandTotalFormulaBody, segmentToRow + 1);
                    }
                    else
                    {
                        grandTotalFormulaBody = String.Format("{0},_COLUMN_{1}:_COLUMN_{2}", grandTotalFormulaBody, segmentFromRow + 1, segmentToRow + 1);
                    }
                }

                #endregion Update the grand totals formula

                #region Set the formula for the sub total

                if (segmentFromRow == segmentToRow)
                {
                    subTotalFormulaBody = String.Format("=_COLUMN_{0}", segmentToRow + 1);
                }
                else
                {
                    subTotalFormulaBody = String.Format("=SUM(_COLUMN_{0}:_COLUMN_{1})", segmentFromRow + 1, segmentToRow + 1);
                }

                #endregion Set the formula for the sub total

                #region Insert the sub-total for the current report segment

                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/214906084/comments and 
                Methods.CopyExcelRegion(wsTemplate, reportSegmentSeparatorTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                #region Old Batch Report Format

                //wsReport.GetCell(String.Format("G{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "G"));
                //wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "I"));
                //wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "J"));
                //wsReport.GetCell(String.Format("K{0}", reportRow + 1)).ApplyFormula(String.Format("=J{0}/G{0}", reportRow + 1));
                //wsReport.GetCell(String.Format("L{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "L"));
                //wsReport.GetCell(String.Format("M{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "M"));
                //wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(String.Format("=M{0}/G{0}", reportRow + 1));
                //wsReport.GetCell(String.Format("O{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "O"));
                //wsReport.GetCell(String.Format("P{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "P"));
                //wsReport.GetCell(String.Format("Q{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "Q"));
                //wsReport.GetCell(String.Format("R{0}", reportRow + 1)).ApplyFormula(String.Format("=(J{0}+M{0}+P{0})/G{0}", reportRow + 1));
                //wsReport.GetCell(String.Format("S{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "S"));
                //wsReport.GetCell(String.Format("U{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "U"));
                //wsReport.GetCell(String.Format("V{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "V"));

                //if (_reportTypeID == 1)
                //{
                //    wsReport.GetCell(String.Format("W{0}", reportRow + 1)).ApplyFormula(String.Format("=V{0}/G{0}", reportRow + 1)); //Redeem %
                //    wsReport.GetCell(String.Format("X{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "X")); //Sales Made in renewal batch
                //}

                #endregion Old Batch Report Format


                //wsReport.GetCell(String.Format("H{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "H"));
                //wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "I")); //Target Sales
                //wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((L{0}+O{0})/H{0},0.01)", reportRow + 1)); //Contact %
                //wsReport.GetCell(String.Format("K{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "K")); //Still To Contact
                //wsReport.GetCell(String.Format("L{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "L")); //Sales
                //wsReport.GetCell(String.Format("M{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(L{0}/H{0},0.01)", reportRow + 1)); //Current Conversion
                //wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "N")); //Sales To Go
                //wsReport.GetCell(String.Format("O{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "O")); //Declines
                //wsReport.GetCell(String.Format("P{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(O{0}/H{0},0.01)", reportRow + 1)); //Decinles %

                //wsReport.GetCell(String.Format("Q{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "Q")); //Diary
                //wsReport.GetCell(String.Format("R{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "R")); //PNA

                //wsReport.GetCell(String.Format("S{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "S")); //Total Redeems
                //if (_reportTypeID == 1)
                //{
                //    wsReport.GetCell(String.Format("T{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(S{0}/H{0},0.01)", reportRow + 1)); //Redeem %
                //    wsReport.GetCell(String.Format("X{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "X")); //Sales Made in renewal batch
                //}
                //wsReport.GetCell(String.Format("U{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "U")); //Falloffs
                //wsReport.GetCell(String.Format("V{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "V")); //Sales in past 5 days

                if (_reportTypeID == 1)
                {
                    wsReport.GetCell(String.Format("H{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "H"));
                    wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "I")); //Target Sales
                    wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "J")); //Leads Available in System
                    wsReport.GetCell(String.Format("K{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((N{0}+Q{0})/H{0},0.01)", reportRow + 1)); //Contact %

                    wsReport.GetCell(String.Format("L{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((N{0}+Q{0}+S{0})/H{0},0.01)", reportRow + 1));

                    wsReport.GetCell(String.Format("M{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "M")); //Still To Contact
                    wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "N")); //Sales
                    wsReport.GetCell(String.Format("O{0}", reportRow + 1)).ApplyFormula(String.Format("=N{0}/H{0}", reportRow + 1)); //Current Conversion
                                                                                                                                     //wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(M{0}/H{0},0.01)", reportRow + 1)); //Current Conversion
                    wsReport.GetCell(String.Format("P{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "P")); //Sales To Go
                    wsReport.GetCell(String.Format("Q{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "Q")); //Declines
                    wsReport.GetCell(String.Format("R{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(Q{0}/H{0},0.01)", reportRow + 1)); //Decinles %

                    wsReport.GetCell(String.Format("S{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "S")); //Diary
                    wsReport.GetCell(String.Format("T{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "T")); //PNA

                    wsReport.GetCell(String.Format("U{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "U")); //Total Redeems
                    if (_reportTypeID == 1)
                    {
                        wsReport.GetCell(String.Format("V{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(U{0}/H{0},0.01)", reportRow + 1)); //Redeem %
                        wsReport.GetCell(String.Format("Z{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "Z")); //Sales Made in renewal batch
                    }
                    wsReport.GetCell(String.Format("W{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "W")); //Falloffs
                    wsReport.GetCell(String.Format("X{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "X")); //Sales in past 5 days
                }
                else
                {
                    //wsReport.GetCell(String.Format("H{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "H"));
                    //wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "I")); //Target Sales
                    //wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "J")); //Leads Available in System
                    //wsReport.GetCell(String.Format("K{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((M{0}+P{0})/H{0},0.01)", reportRow + 1)); //Contact %
                    //wsReport.GetCell(String.Format("L{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "L")); //Still To Contact
                    //wsReport.GetCell(String.Format("M{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "M")); //Sales
                    //wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(String.Format("=M{0}/H{0}", reportRow + 1)); //Current Conversion
                    //                                                                                                                 //wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(M{0}/H{0},0.01)", reportRow + 1)); //Current Conversion
                    //wsReport.GetCell(String.Format("O{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "O")); //Sales To Go
                    //wsReport.GetCell(String.Format("P{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "P")); //Declines
                    //wsReport.GetCell(String.Format("Q{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(P{0}/H{0},0.01)", reportRow + 1)); //Decinles %

                    //wsReport.GetCell(String.Format("R{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "R")); //Diary
                    //wsReport.GetCell(String.Format("S{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "S")); //PNA

                    //wsReport.GetCell(String.Format("T{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "T")); //Total Redeems
                    //if (_reportTypeID == 1)
                    //{
                    //    wsReport.GetCell(String.Format("U{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(T{0}/H{0},0.01)", reportRow + 1)); //Redeem %
                    //    wsReport.GetCell(String.Format("Y{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "Y")); //Sales Made in renewal batch
                    //}
                    //wsReport.GetCell(String.Format("V{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "V")); //Falloffs
                    //wsReport.GetCell(String.Format("W{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "W")); //Sales in past 5 days

                    wsReport.GetCell(String.Format("H{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "H"));
                    wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "I")); //Target Sales
                    wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "J")); //Leads Available in System
                    wsReport.GetCell(String.Format("K{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((N{0}+Q{0})/H{0},0.01)", reportRow + 1)); //Contact %

                    wsReport.GetCell(String.Format("L{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((N{0}+Q{0}+S{0})/H{0},0.01)", reportRow + 1));

                    wsReport.GetCell(String.Format("M{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "M")); //Still To Contact
                    wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "N")); //Sales
                    wsReport.GetCell(String.Format("O{0}", reportRow + 1)).ApplyFormula(String.Format("=N{0}/H{0}", reportRow + 1)); //Current Conversion
                                                                                                                                     //wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(M{0}/H{0},0.01)", reportRow + 1)); //Current Conversion
                    wsReport.GetCell(String.Format("P{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "P")); //Sales To Go
                    wsReport.GetCell(String.Format("Q{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "Q")); //Declines
                    wsReport.GetCell(String.Format("R{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(Q{0}/H{0},0.01)", reportRow + 1)); //Decinles %

                    wsReport.GetCell(String.Format("S{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "S")); //Diary
                    wsReport.GetCell(String.Format("T{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "T")); //PNA

                    wsReport.GetCell(String.Format("U{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "U")); //Total Redeems
                    if (_reportTypeID == 1)
                    {
                        wsReport.GetCell(String.Format("V{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(U{0}/H{0},0.01)", reportRow + 1)); //Redeem %
                        wsReport.GetCell(String.Format("Z{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "Z")); //Sales Made in renewal batch
                    }
                    wsReport.GetCell(String.Format("W{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "W")); //Falloffs
                    wsReport.GetCell(String.Format("X{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "X")); //Sales in past 5 days

                }

                reportRow++;

                #endregion Insert the sub-total for the current report segment
            }

            #region Finally, insert the grand totals

            Methods.CopyExcelRegion(wsTemplate, grandTotalTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

            #region Old Batch Report Format

            //wsReport.GetCell(String.Format("G{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "G")));
            //wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "I")));
            //wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "J")));
            //wsReport.GetCell(String.Format("K{0}", reportRow + 1)).ApplyFormula(String.Format("=J{0}/G{0}", reportRow + 1));
            //wsReport.GetCell(String.Format("L{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "L")));
            //wsReport.GetCell(String.Format("M{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "M")));
            //wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(String.Format("=M{0}/G{0}", reportRow + 1));
            //wsReport.GetCell(String.Format("O{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "O")));
            //wsReport.GetCell(String.Format("P{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "P")));
            //wsReport.GetCell(String.Format("Q{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "Q")));
            //wsReport.GetCell(String.Format("R{0}", reportRow + 1)).ApplyFormula(String.Format("=(J{0}+M{0}+P{0})/G{0}", reportRow + 1));
            //wsReport.GetCell(String.Format("S{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "S")));

            //wsReport.GetCell(String.Format("U{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "U")));
            //wsReport.GetCell(String.Format("V{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "V")));

            //if (_reportTypeID == 1)
            //{
            //    wsReport.GetCell(String.Format("W{0}", reportRow + 1)).ApplyFormula(String.Format("=V{0}/G{0}", reportRow + 1));
            //    wsReport.GetCell(String.Format("X{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "X")));
            //}

            #endregion  Old Batch Report Format


            //wsReport.GetCell(String.Format("H{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "H"))); //Leads
            //wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "I"))); //Target Sales
            //wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((L{0}+O{0})/H{0},0.01)", reportRow + 1)); //Contact %
            //wsReport.GetCell(String.Format("K{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "K"))); //Still to Contact
            //wsReport.GetCell(String.Format("L{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "L"))); //Sales
            //wsReport.GetCell(String.Format("M{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(L{0}/H{0},0.01)", reportRow + 1)); //Current Conversion
            //wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "N"))); //Sales To Go
            //wsReport.GetCell(String.Format("O{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "O"))); //Declines
            //wsReport.GetCell(String.Format("P{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(O{0}/H{0},0.01)", reportRow + 1)); //Declines %

            //wsReport.GetCell(String.Format("Q{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "Q"))); //Diary
            //wsReport.GetCell(String.Format("R{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "R"))); //PNA

            //wsReport.GetCell(String.Format("S{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "S"))); //Total Redeems

            //if (_reportTypeID == 1)
            //{
            //    wsReport.GetCell(String.Format("T{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(S{0}/H{0},0.01)", reportRow + 1)); //Redeem %
            //    wsReport.GetCell(String.Format("X{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "X"))); //Sales made in renewal batch
            //}

            //wsReport.GetCell(String.Format("U{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "U"))); //Falloffs

            //wsReport.GetCell(String.Format("V{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "V"))); //Sales in past 5 days

            if (_reportTypeID == 1)
            {
                wsReport.GetCell(String.Format("H{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "H"))); //Leads
                wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "I"))); //Target Sales
                wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "J"))); //Leads Available in System
                wsReport.GetCell(String.Format("K{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((N{0}+Q{0})/H{0},0.01)", reportRow + 1)); //Contact %

                wsReport.GetCell(String.Format("L{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((N{0}+Q{0}+S{0})/H{0},0.01)", reportRow + 1));

                wsReport.GetCell(String.Format("M{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "M"))); //Still to Contact
                wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "N"))); //Sales
                wsReport.GetCell(String.Format("O{0}", reportRow + 1)).ApplyFormula(String.Format("=N{0}/H{0}", reportRow + 1)); //Current Conversion
                wsReport.GetCell(String.Format("P{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "P"))); //Sales To Go
                wsReport.GetCell(String.Format("Q{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "Q"))); //Declines
                wsReport.GetCell(String.Format("R{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(Q{0}/H{0},0.01)", reportRow + 1)); //Declines %

                wsReport.GetCell(String.Format("S{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "S"))); //Diary
                wsReport.GetCell(String.Format("T{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "T"))); //PNA

                wsReport.GetCell(String.Format("U{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "U"))); //Total Redeems

                if (_reportTypeID == 1)
                {
                    wsReport.GetCell(String.Format("V{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(U{0}/H{0},0.01)", reportRow + 1)); //Redeem %
                    wsReport.GetCell(String.Format("Z{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "Z"))); //Sales made in renewal batch
                }

                wsReport.GetCell(String.Format("W{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "W"))); //Falloffs

                wsReport.GetCell(String.Format("X{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "X"))); //Sales in past 5 days
            }
            else
            {
                //wsReport.GetCell(String.Format("H{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "H"))); //Leads
                //wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "I"))); //Target Sales
                //wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "J"))); //Leads Available in System
                //wsReport.GetCell(String.Format("K{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((M{0}+P{0})/H{0},0.01)", reportRow + 1)); //Contact %
                //wsReport.GetCell(String.Format("L{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "L"))); //Still to Contact
                //wsReport.GetCell(String.Format("M{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "M"))); //Sales
                //wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(String.Format("=M{0}/H{0}", reportRow + 1)); //Current Conversion
                //wsReport.GetCell(String.Format("O{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "O"))); //Sales To Go
                //wsReport.GetCell(String.Format("P{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "P"))); //Declines
                //wsReport.GetCell(String.Format("Q{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(P{0}/H{0},0.01)", reportRow + 1)); //Declines %

                //wsReport.GetCell(String.Format("R{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "R"))); //Diary
                //wsReport.GetCell(String.Format("S{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "S"))); //PNA

                //wsReport.GetCell(String.Format("T{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "T"))); //Total Redeems

                //if (_reportTypeID == 1)
                //{
                //    wsReport.GetCell(String.Format("U{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(T{0}/H{0},0.01)", reportRow + 1)); //Redeem %
                //    wsReport.GetCell(String.Format("Y{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "Y"))); //Sales made in renewal batch
                //}

                //wsReport.GetCell(String.Format("V{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "V"))); //Falloffs

                //wsReport.GetCell(String.Format("W{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "W"))); //Sales in past 5 days

                wsReport.GetCell(String.Format("H{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "H"))); //Leads
                wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "I"))); //Target Sales
                wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "J"))); //Leads Available in System
                wsReport.GetCell(String.Format("K{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((N{0}+Q{0})/H{0},0.01)", reportRow + 1)); //Contact %

                wsReport.GetCell(String.Format("L{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((N{0}+Q{0}+S{0})/H{0},0.01)", reportRow + 1));

                wsReport.GetCell(String.Format("M{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "M"))); //Still to Contact
                wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "N"))); //Sales
                wsReport.GetCell(String.Format("O{0}", reportRow + 1)).ApplyFormula(String.Format("=N{0}/H{0}", reportRow + 1)); //Current Conversion
                wsReport.GetCell(String.Format("P{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "P"))); //Sales To Go
                wsReport.GetCell(String.Format("Q{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "Q"))); //Declines
                wsReport.GetCell(String.Format("R{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(Q{0}/H{0},0.01)", reportRow + 1)); //Declines %

                wsReport.GetCell(String.Format("S{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "S"))); //Diary
                wsReport.GetCell(String.Format("T{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "T"))); //PNA

                wsReport.GetCell(String.Format("U{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "U"))); //Total Redeems

                if (_reportTypeID == 1)
                {
                    wsReport.GetCell(String.Format("V{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(U{0}/H{0},0.01)", reportRow + 1)); //Redeem %
                    wsReport.GetCell(String.Format("Z{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "Z"))); //Sales made in renewal batch
                }

                wsReport.GetCell(String.Format("W{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "W"))); //Falloffs

                wsReport.GetCell(String.Format("X{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "X"))); //Sales in past 5 days

            }

            //wsReport.GetCell(String.Format("H{0}", reportRow + 1)).ApplyFormula(String.Format("=G{0}/E{0}", reportRow + 1));
            //wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "I")));
            //wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "J")));

            #endregion Finally, insert the grand totals
        }

        private void InsertDefaultBatchReportDataOld(Worksheet wsTemplate, Worksheet wsReport, DataRow drReportConfigs, DataTable dtReportData, DataTable dtBatchReceiveDates, DataTable dtColumnCellMappings, byte templateRowSpan, int reportRow)
        {
            #region Partition the given DataSet

            //DataRow drReportConfigs = dsReportData.Tables[0].Rows[0];
            //DataTable dtReportData = dsReportData.Tables[1];
            //DataTable dtBatchReceiveDates = dsReportData.Tables[2];
            //DataTable dtColumnCellMappings = dsReportData.Tables[3];

            #endregion Partition the given DataSet

            #region Declarations & Initializations

            string grandTotalFormulaBody = String.Empty;
            string subTotalFormulaBody = String.Empty;

            int segmentFromRow = 0;
            int segmentToRow = 0;

            #endregion Declarations & Initializations

            #region Get the configs

            byte reportDataTemplateRowIndex = Convert.ToByte(drReportConfigs["ReportDataTemplateRowIndex"]);
            byte templateColumnSpan = Convert.ToByte(drReportConfigs["TemplateColumnSpan"]);
            byte reportSegmentSeparatorTemplateRowIndex = Convert.ToByte(drReportConfigs["ReportSegmentSeparatorTemplateRowIndex"]);
            byte grandTotalTemplateRowIndex = Convert.ToByte(drReportConfigs["GrandTotalTemplateRowIndex"]);

            #endregion Get the configs

            for (int i = 0; i < dtBatchReceiveDates.Rows.Count; i++)
            {
                //DateTime dateReceivedFromBatchCode = Convert.ToDateTime(dtBatchReceiveDates.Rows[i]["DateReceivedFromBatchCode"]);
                //long fkCampaignID = Convert.ToInt64(dtBatchReceiveDates.Rows[i]["FKINCampaignID"]);
                //DataTable dtBatchesReceivedForDate = dtReportData.Select(String.Format("[DateReceivedFromBatchCode] = '{0}' AND [FKINCampaignID] = {1}",
                //    dateReceivedFromBatchCode.ToString("yyyy-MM-dd"),
                //    fkCampaignID)).CopyToDataTable();

                string reportSegmentFilterString = dtBatchReceiveDates.Rows[i]["ReportSegmentFilterString"].ToString();
                DataTable dtBatchesReceivedForDate = dtReportData.Select(reportSegmentFilterString).CopyToDataTable();

                segmentFromRow = reportRow;

                foreach (DataRow drCurrentBatch in dtBatchesReceivedForDate.Rows)
                {
                    Methods.CopyExcelRegion(wsTemplate, reportDataTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                    foreach (DataRow drColumnRowMapping in dtColumnCellMappings.Rows)
                    {
                        string excelColumn = drColumnRowMapping["ExcelColumn"].ToString();
                        string dataTableColumnNameOrFormula = drColumnRowMapping["DataTableColumnNameOrFormula"].ToString();

                        if (dataTableColumnNameOrFormula.StartsWith("="))
                        {
                            dataTableColumnNameOrFormula = dataTableColumnNameOrFormula.Replace("#ROW#", (reportRow + 1).ToString());
                            wsReport.GetCell(String.Format("{0}{1}", excelColumn, reportRow + 1)).ApplyFormula(dataTableColumnNameOrFormula);
                        }
                        else
                        {
                            wsReport.GetCell(String.Format("{0}{1}", excelColumn, reportRow + 1)).Value = drCurrentBatch[dataTableColumnNameOrFormula]; //dataTableColumnNameOrFormula;
                        }
                    }

                    segmentToRow = reportRow;
                    reportRow++;
                }

                #region Update the grand totals formula

                if (grandTotalFormulaBody.Length == 0)
                {
                    if (segmentFromRow == segmentToRow)
                    {
                        grandTotalFormulaBody = String.Format("_COLUMN_{0}", segmentToRow + 1);
                    }
                    else
                    {
                        grandTotalFormulaBody = String.Format("_COLUMN_{0}:_COLUMN_{1}", segmentFromRow + 1, segmentToRow + 1);
                    }
                }
                else
                {
                    if (segmentFromRow == segmentToRow)
                    {
                        grandTotalFormulaBody = String.Format("{0},_COLUMN_{1}", grandTotalFormulaBody, segmentToRow + 1);
                    }
                    else
                    {
                        grandTotalFormulaBody = String.Format("{0},_COLUMN_{1}:_COLUMN_{2}", grandTotalFormulaBody, segmentFromRow + 1, segmentToRow + 1);
                    }
                }

                #endregion Update the grand totals formula

                #region Set the formula for the sub total

                if (segmentFromRow == segmentToRow)
                {
                    subTotalFormulaBody = String.Format("=_COLUMN_{0}", segmentToRow + 1);
                }
                else
                {
                    subTotalFormulaBody = String.Format("=SUM(_COLUMN_{0}:_COLUMN_{1})", segmentFromRow + 1, segmentToRow + 1);
                }

                #endregion Set the formula for the sub total

                #region Insert the sub-total for the current report segment

                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/214906084/comments and 
                Methods.CopyExcelRegion(wsTemplate, reportSegmentSeparatorTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                #region Old Batch Report Format

                wsReport.GetCell(String.Format("G{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "G"));
                wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "I"));
                wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "J"));
                wsReport.GetCell(String.Format("K{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(J{0}/G{0}, 0.01)", reportRow + 1));
                wsReport.GetCell(String.Format("L{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "L"));
                wsReport.GetCell(String.Format("M{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "M"));
                wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(M{0}/G{0}, 0.01)", reportRow + 1));
                wsReport.GetCell(String.Format("O{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "O"));
                wsReport.GetCell(String.Format("P{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "P"));
                wsReport.GetCell(String.Format("Q{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "Q"));
                wsReport.GetCell(String.Format("R{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((J{0}+M{0})/G{0}, 0.01)", reportRow + 1));
                wsReport.GetCell(String.Format("S{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "S"));
                wsReport.GetCell(String.Format("U{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "U"));
                wsReport.GetCell(String.Format("V{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "V"));

                if (_reportTypeID == 1)
                {
                    wsReport.GetCell(String.Format("W{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(V{0}/G{0},0.01)", reportRow + 1)); //Redeem %
                    wsReport.GetCell(String.Format("X{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "X")); //Sales Made in renewal batch
                }

                #endregion Old Batch Report Format


                reportRow++;

                #endregion Insert the sub-total for the current report segment
            }

            #region Finally, insert the grand totals

            Methods.CopyExcelRegion(wsTemplate, grandTotalTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

            #region Old Batch Report Format

            wsReport.GetCell(String.Format("G{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "G")));
            wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "I")));
            wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "J")));
            wsReport.GetCell(String.Format("K{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(J{0}/G{0},0.01)", reportRow + 1));
            wsReport.GetCell(String.Format("L{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "L")));
            wsReport.GetCell(String.Format("M{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "M")));
            wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(M{0}/G{0},0.01)", reportRow + 1));
            wsReport.GetCell(String.Format("O{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "O")));
            wsReport.GetCell(String.Format("P{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "P")));
            wsReport.GetCell(String.Format("Q{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "Q")));
            wsReport.GetCell(String.Format("R{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((J{0}+M{0})/G{0},0.01)", reportRow + 1));
            wsReport.GetCell(String.Format("S{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "S")));

            wsReport.GetCell(String.Format("U{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "U")));
            wsReport.GetCell(String.Format("V{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "V")));

            if (_reportTypeID == 1)
            {
                wsReport.GetCell(String.Format("W{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(V{0}/G{0},0.01)", reportRow + 1));
                wsReport.GetCell(String.Format("X{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "X")));
            }

            #endregion  Old Batch Report Format

            //wsReport.GetCell(String.Format("H{0}", reportRow + 1)).ApplyFormula(String.Format("=G{0}/E{0}", reportRow + 1));
            //wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "I")));
            //wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "J")));

            #endregion Finally, insert the grand totals
        }

        //private void InsertExtensionBatchReportData(Worksheet wsTemplate, Worksheet wsReport, DataSet dsReportData, byte templateRowSpan, int reportRow)
        private void InsertExtensionBatchReportData(Worksheet wsTemplate, Worksheet wsReport, DataRow drReportConfigs, DataTable dtReportData, DataTable dtColumnCellMappings, byte templateRowSpan, int reportRow)
        {
            //#region Partition the given DataSet

            //DataRow drReportConfigs = dsReportData.Tables[0].Rows[0];
            //DataTable dtReportData = dsReportData.Tables[1];
            //DataTable dtColumnCellMappings = dsReportData.Tables[2];

            //#endregion Partition the given DataSet

            byte reportDataTemplateRowIndex = Convert.ToByte(drReportConfigs["ReportDataTemplateRowIndex"]);
            byte templateColumnSpan = Convert.ToByte(drReportConfigs["TemplateColumnSpan"]);

            //foreach (DataRow drBatchReceiveDate in dtBatchReceiveDates.Rows)
            //{
            //DateTime dateReceivedFromBatchCode = Convert.ToDateTime(drBatchReceiveDate["DateReceivedFromBatchCode"]);
            //DataTable dtBatchesReceivedForDate = dtReportData.Select(String.Format("DateReceivedFromBatchCode = '{0}'", dateReceivedFromBatchCode.ToString("yyyy-MM-dd"))).CopyToDataTable();
            ////byte batchCount = dtBatchesReceivedForDate

            foreach (DataRow drCurrentBatch in dtReportData.Rows)
            {
                Methods.CopyExcelRegion(wsTemplate, reportDataTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                foreach (DataRow drColumnRowMapping in dtColumnCellMappings.Rows)
                {
                    string excelColumn = drColumnRowMapping["ExcelColumn"].ToString();
                    string dataTableColumnNameOrFormula = drColumnRowMapping["DataTableColumnNameOrFormula"].ToString();

                    if (dataTableColumnNameOrFormula.StartsWith("="))
                    {
                        dataTableColumnNameOrFormula = dataTableColumnNameOrFormula.Replace("#ROW#", (reportRow + 1).ToString());
                        wsReport.GetCell(String.Format("{0}{1}", excelColumn, reportRow + 1)).ApplyFormula(dataTableColumnNameOrFormula);
                    }
                    else
                    {
                        wsReport.GetCell(String.Format("{0}{1}", excelColumn, reportRow + 1)).Value = drCurrentBatch[dataTableColumnNameOrFormula]; //dataTableColumnNameOrFormula;
                    }
                }
                reportRow++;
            }
            //}
        }

        private void InsertUpgradeBatchReportData(Worksheet wsTemplate, Worksheet wsReport, DataSet dsReportData, byte templateRowSpan, int reportRow)
        {
            #region Partition the given DataSet

            DataRow drReportConfigs = dsReportData.Tables[0].Rows[0];
            DataTable dtReportData = dsReportData.Tables[1];
            DataTable dtUpgradeBatchReportSegments = dsReportData.Tables[2];
            DataTable dtColumnCellMappings = dsReportData.Tables[3];

            #endregion Partition the given DataSet

            #region Declarations & Initializations

            string grandTotalFormulaBody = String.Empty;
            string subTotalFormulaBody = String.Empty;
            string percentageTotalFormulaBody = String.Empty;

            int segmentFromRow = 0;
            int segmentToRow = 0;

            string campaignGroupCode = String.Empty;

            //int targetSTLPercentage;

            #endregion Declarations & Initializations

            #region Get the configs

            byte reportDataTemplateRowIndex = Convert.ToByte(drReportConfigs["ReportDataTemplateRowIndex"]);
            byte templateColumnSpan = Convert.ToByte(drReportConfigs["TemplateColumnSpan"]);
            byte reportSegmentSeparatorTemplateRowIndex = Convert.ToByte(drReportConfigs["ReportSegmentSeparatorTemplateRowIndex"]);
            byte grandTotalTemplateRowIndex = Convert.ToByte(drReportConfigs["GrandTotalTemplateRowIndex"]);

            string month1ColumnHeadingCell = drReportConfigs["Month1ColumnHeadingCell"].ToString();
            string month2ColumnHeadingCell = drReportConfigs["Month2ColumnHeadingCell"].ToString();
            string month3ColumnHeadingCell = drReportConfigs["Month3ColumnHeadingCell"].ToString();

            string month1ColumnHeading = String.Format("{0} Bonus Value", drReportConfigs["Month1"].ToString());
            string month2ColumnHeading = String.Format("{0} Bonus Value", drReportConfigs["Month2"].ToString());
            string month3ColumnHeading = String.Format("{0} Bonus Value", drReportConfigs["Month3"].ToString());

            #endregion Get the configs

            #region Rename the column headings

            wsReport.GetCell(month1ColumnHeadingCell).Value = month1ColumnHeading;
            wsReport.GetCell(month2ColumnHeadingCell).Value = month2ColumnHeading;
            wsReport.GetCell(month3ColumnHeadingCell).Value = month3ColumnHeading;

            #endregion Rename the column headings

            for (int i = 0; i < dtUpgradeBatchReportSegments.Rows.Count; i++)
            {
                //DateTime dateReceivedFromBatchCode = Convert.ToDateTime(drBatchReceiveDate["DateReceivedFromBatchCode"]);
                //DataTable dtBatchesReceivedForDate = dtReportData.Select(String.Format("DateReceivedFromBatchCode = '{0}'", dateReceivedFromBatchCode.ToString("yyyy-MM-dd"))).CopyToDataTable();

                long fkINCampaignGroupID = Convert.ToInt64(dtUpgradeBatchReportSegments.Rows[i]["FKINCampaignGroupID"]);
                long fkINCampaignTypeID = Convert.ToInt64(dtUpgradeBatchReportSegments.Rows[i]["FKINCampaignTypeID"]);
                long campaignsInSegment = Convert.ToByte(dtUpgradeBatchReportSegments.Rows[i]["CampaignsInSegment"]);

                DataTable dtReportDataSubset = dtReportData.Select(String.Format("[FKINCampaignGroupID] = {0} AND [FKINCampaignTypeID] = {1}", fkINCampaignGroupID, fkINCampaignTypeID)).CopyToDataTable();

                campaignGroupCode = dtReportDataSubset.Rows[0]["CampaignCode"].ToString();

                //targetSTLPercentage = Convert.ToInt32(dtReportDataSubset.Rows[0]["TargetPercentage1"]);

                //int targetSTLPercentage1;

                //targetSTLPercentage1 = targetSTLPercentage * 100;

                campaignGroupCode = (campaignGroupCode.Replace("PL", "")).Replace("PM", "");
                campaignGroupCode = (campaignGroupCode.Replace("_NR", "")).Replace("_R", "");
                campaignGroupCode = campaignGroupCode.Replace("T", "");

                segmentFromRow = reportRow;

                foreach (DataRow drCurrentCampaign in dtReportDataSubset.Rows)
                {
                    Methods.CopyExcelRegion(wsTemplate, reportDataTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                    foreach (DataRow drColumnRowMapping in dtColumnCellMappings.Rows)
                    {
                        string excelColumn = drColumnRowMapping["ExcelColumn"].ToString();
                        string dataTableColumnNameOrFormula = drColumnRowMapping["DataTableColumnNameOrFormula"].ToString();

                        if (dataTableColumnNameOrFormula.StartsWith("="))
                        {
                            dataTableColumnNameOrFormula = dataTableColumnNameOrFormula.Replace("#ROW#", (reportRow + 1).ToString());
                            wsReport.GetCell(String.Format("{0}{1}", excelColumn, reportRow + 1)).ApplyFormula(dataTableColumnNameOrFormula);
                        }
                        else
                        {
                            wsReport.GetCell(String.Format("{0}{1}", excelColumn, reportRow + 1)).Value = drCurrentCampaign[dataTableColumnNameOrFormula]; //dataTableColumnNameOrFormula;
                        }
                    }

                    segmentToRow = reportRow;
                    reportRow++;
                }

                #region Update the grand totals formula

                if (grandTotalFormulaBody.Length == 0)
                {
                    if (segmentFromRow == segmentToRow)
                    {
                        grandTotalFormulaBody = String.Format("_COLUMN_{0}", segmentToRow + 1);


                    }
                    else
                    {
                        grandTotalFormulaBody = String.Format("_COLUMN_{0}:_COLUMN_{1}", segmentFromRow + 1, segmentToRow + 1);
                    }
                }
                else
                {
                    if (segmentFromRow == segmentToRow)
                    {
                        grandTotalFormulaBody = String.Format("{0},_COLUMN_{1}", grandTotalFormulaBody, segmentToRow + 1);
                    }
                    else
                    {
                        grandTotalFormulaBody = String.Format("{0},_COLUMN_{1}:_COLUMN_{2}", grandTotalFormulaBody, segmentFromRow + 1, segmentToRow + 1);
                    }
                }

                #region Set the formula for the sub total

                if (segmentFromRow == segmentToRow)
                {
                    percentageTotalFormulaBody = String.Format("=_COLUMN_{0}", segmentToRow + 1);

                    subTotalFormulaBody = String.Format("=_COLUMN_{0}", segmentFromRow + 1);

                    //averageSTLPercentage = String.Format("=_COLUMN_{0}", segmentFromRow + 1);

                }
                else
                {
                    percentageTotalFormulaBody = String.Format("=_COLUMN_{0}", segmentFromRow + 1);

                    subTotalFormulaBody = String.Format("=SUM(_COLUMN_{0}:_COLUMN_{1})", segmentFromRow + 1, segmentToRow + 1);

                    //averageSTLPercentage = String.Format("=_COLUMN_{0}", segmentFromRow + 1);

                }

                #endregion Set the formula for the sub total

                #endregion Update the grand totals formula

                #region Insert the row separator

                if (i < dtUpgradeBatchReportSegments.Rows.Count)
                {
                    Methods.CopyExcelRegion(wsTemplate, reportSegmentSeparatorTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                    wsReport.GetCell(String.Format("A{0}", reportRow + 1)).Value = campaignGroupCode; //CampaignGroupCode

                    //=========================================================================================================================================================
                    //Month1
                    wsReport.GetCell(String.Format("B{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "B"));//BonusValue1

                    wsReport.GetCell(String.Format("D{0}", reportRow + 1)).ApplyFormula(percentageTotalFormulaBody.Replace("_COLUMN_", "D")); //TargetSTL%1

                    //wsReport.GetCell(String.Format("D{0}", reportRow + 1)).ApplyFormula(String.Format("=_COLUMN_{0}", reportRow + 1)); //TargetSTL%1

                    //wsReport.GetCell(String.Format("D{0}", reportRow + 1)).ApplyFormula(grandTotalFormulaBody.Replace("_COLUMN_{0}", "D"));

                    wsReport.GetCell(String.Format("E{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "E"));//LeadsInBatch1
                    wsReport.GetCell(String.Format("F{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "F"));//LeadsAvailable1
                    wsReport.GetCell(String.Format("G{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "G"));//RenewalLeadsAvailable1
                    //wsReport.GetCell(String.Format("H{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "H"));//TargetSales1

                    wsReport.GetCell(String.Format("H{0}", reportRow + 1)).ApplyFormula(String.Format("=E{0}*D{0}", reportRow + 1));//TargetSales1

                    wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "I"));//Sales1
                    wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "J"));//Units1
                    wsReport.GetCell(String.Format("K{0}", reportRow + 1)).ApplyFormula(String.Format("=I{0}/E{0}", reportRow + 1));//Current STL %
                    wsReport.GetCell(String.Format("L{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "L"));
                    wsReport.GetCell(String.Format("M{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "M"));//StillToContact1
                    wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "N"));//SalesInPreviousWeek1
                    wsReport.GetCell(String.Format("O{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((E{0}-M{0})/E{0}, 0.01)", reportRow + 1));
                    wsReport.GetCell(String.Format("P{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "P"));//Falloffs1


                    //=========================================================================================================================================================
                    //Month2
                    wsReport.GetCell(String.Format("U{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "U"));//BonusValue2
                    wsReport.GetCell(String.Format("W{0}", reportRow + 1)).ApplyFormula(percentageTotalFormulaBody.Replace("_COLUMN_", "W")); //TargetSTL%2
                    wsReport.GetCell(String.Format("X{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "X"));//LeadsInBatch2
                    wsReport.GetCell(String.Format("Y{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "Y"));//LeadsAvailable2
                    wsReport.GetCell(String.Format("Z{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "Z"));//RenewalLeadsAvailable2
                    //wsReport.GetCell(String.Format("AA{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "AA"));//TargetSales2

                    wsReport.GetCell(String.Format("AA{0}", reportRow + 1)).ApplyFormula(String.Format("=X{0}*W{0}", reportRow + 1));//TargetSales2

                    wsReport.GetCell(String.Format("AB{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "AB"));//Sales2
                    wsReport.GetCell(String.Format("AC{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "AC"));//Units2
                    wsReport.GetCell(String.Format("AD{0}", reportRow + 1)).ApplyFormula(String.Format("=AB{0}/X{0}", reportRow + 1));//Current STL %
                    wsReport.GetCell(String.Format("AE{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "AE"));
                    wsReport.GetCell(String.Format("AF{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "AF"));//StillToContact2
                    wsReport.GetCell(String.Format("AG{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "AG"));//SalesInPreviousWeek2
                    wsReport.GetCell(String.Format("AH{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((X{0}-AF{0})/X{0}, 0.01)", reportRow + 1));
                    wsReport.GetCell(String.Format("AI{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "AI"));//Falloffs2



                    //=========================================================================================================================================================
                    //Month3
                    wsReport.GetCell(String.Format("AN{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "AN"));//BonusValue3
                    wsReport.GetCell(String.Format("AP{0}", reportRow + 1)).ApplyFormula(percentageTotalFormulaBody.Replace("_COLUMN_", "AP")); //TargetSTL%3
                    wsReport.GetCell(String.Format("AQ{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "AQ"));//LeadsInBatch3
                    wsReport.GetCell(String.Format("AR{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "AR"));//LeadsAvailable3
                    wsReport.GetCell(String.Format("AS{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "AS"));//RenewalLeadsAvailable3
                    //wsReport.GetCell(String.Format("AT{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "AT"));//TargetSales3

                    wsReport.GetCell(String.Format("AT{0}", reportRow + 1)).ApplyFormula(String.Format("=AQ{0}*AP{0}", reportRow + 1));//TargetSales3

                    wsReport.GetCell(String.Format("AU{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "AU"));//Sales3

                    wsReport.GetCell(String.Format("AV{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "AV"));//Units3

                    wsReport.GetCell(String.Format("AW{0}", reportRow + 1)).ApplyFormula(String.Format("=AU{0}/AQ{0}", reportRow + 1));//Current STL %
                    wsReport.GetCell(String.Format("AX{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "AX"));
                    wsReport.GetCell(String.Format("AY{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "AY"));//StillToContact3
                    wsReport.GetCell(String.Format("AZ{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "AZ"));//SalesInPreviousWeek3

                    wsReport.GetCell(String.Format("BA{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((AQ{0}-AY{0})/AQ{0}, 0.01)", reportRow + 1));
                    wsReport.GetCell(String.Format("BB{0}", reportRow + 1)).ApplyFormula(subTotalFormulaBody.Replace("_COLUMN_", "BB"));//Falloffs3


                    reportRow++;
                }

                #endregion Insert the row separator
            }

            #region Finally, insert the grand totals

            Methods.CopyExcelRegion(wsTemplate, grandTotalTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);



            ////=========================================================================================================================================================
            ////Month1
            //wsReport.GetCell(String.Format("B{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "B")));//BonusValue1
            //wsReport.GetCell(String.Format("E{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "E")));//LeadsInBatch1
            //wsReport.GetCell(String.Format("F{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "F")));//LeadsAvailable1
            //wsReport.GetCell(String.Format("G{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "G")));//RenewalLeadsAvailable1
            //wsReport.GetCell(String.Format("H{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "H")));//TargetSales1
            //wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "I")));//Sales1
            //wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(I{0}/E{0}, 0.01)", reportRow + 1));
            //wsReport.GetCell(String.Format("K{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "K")));
            //wsReport.GetCell(String.Format("L{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "L")));//StillToContact1
            //wsReport.GetCell(String.Format("M{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "M")));//SalesInPreviousWeek1
            //wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((E{0}-L{0})/E{0}, 0.01)", reportRow + 1));
            //wsReport.GetCell(String.Format("O{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "O")));//Falloffs1


            //=========================================================================================================================================================
            //Month1
            wsReport.GetCell(String.Format("B{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "B")));//BonusValue1
            wsReport.GetCell(String.Format("E{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "E")));//LeadsInBatch1
            wsReport.GetCell(String.Format("F{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "F")));//LeadsAvailable1
            wsReport.GetCell(String.Format("G{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "G")));//RenewalLeadsAvailable1
            wsReport.GetCell(String.Format("H{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "H")));//TargetSales1
            wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "I")));//Sales1
            wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "J")));//Units1
            wsReport.GetCell(String.Format("K{0}", reportRow + 1)).ApplyFormula(String.Format("=I{0}/E{0}", reportRow + 1));//Current STL %
            wsReport.GetCell(String.Format("L{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "L")));
            wsReport.GetCell(String.Format("M{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "M")));//StillToContact1
            wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "N")));//SalesInPreviousWeek1
            wsReport.GetCell(String.Format("O{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((E{0}-M{0})/E{0}, 0.01)", reportRow + 1));
            wsReport.GetCell(String.Format("P{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "P")));//Falloffs1



            ////=========================================================================================================================================================
            ////Month2
            //wsReport.GetCell(String.Format("T{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "T")));//BonusValue2
            //wsReport.GetCell(String.Format("W{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "W")));//LeadsInBatch2
            //wsReport.GetCell(String.Format("X{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "X")));//LeadsAvailable2
            //wsReport.GetCell(String.Format("Y{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "Y")));//RenewalLeadsAvailable2
            //wsReport.GetCell(String.Format("Z{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "Z")));//TargetSales2
            //wsReport.GetCell(String.Format("AA{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AA")));//Sales2
            //wsReport.GetCell(String.Format("AB{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(AA{0}/W{0}, 0.01)", reportRow + 1));
            //wsReport.GetCell(String.Format("AC{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AC")));
            //wsReport.GetCell(String.Format("AD{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AD")));//StillToContact2
            //wsReport.GetCell(String.Format("AE{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AE")));//SalesInPreviousWeek2
            //wsReport.GetCell(String.Format("AF{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((W{0}-AD{0})/W{0}, 0.01)", reportRow + 1));
            //wsReport.GetCell(String.Format("AG{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AG")));//Falloffs2

            //=========================================================================================================================================================
            //Month2
            wsReport.GetCell(String.Format("U{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "U")));//BonusValue2
            wsReport.GetCell(String.Format("X{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "X")));//LeadsInBatch2
            wsReport.GetCell(String.Format("Y{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "Y")));//LeadsAvailable2
            wsReport.GetCell(String.Format("Z{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "Z")));//RenewalLeadsAvailable2
            wsReport.GetCell(String.Format("AA{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AA")));//TargetSales2
            wsReport.GetCell(String.Format("AB{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AB")));//Sales2
            wsReport.GetCell(String.Format("AC{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AC")));//Units2
            wsReport.GetCell(String.Format("AD{0}", reportRow + 1)).ApplyFormula(String.Format("=AB{0}/X{0}", reportRow + 1));//Current STL %
            wsReport.GetCell(String.Format("AE{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AE")));
            wsReport.GetCell(String.Format("AF{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AF")));//StillToContact2
            wsReport.GetCell(String.Format("AG{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AG")));//SalesInPreviousWeek2
            wsReport.GetCell(String.Format("AH{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((X{0}-AF{0})/X{0}, 0.01)", reportRow + 1));
            wsReport.GetCell(String.Format("AI{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AI")));//Falloffs2


            ////=========================================================================================================================================================
            ////Month3
            //wsReport.GetCell(String.Format("AL{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AL")));//BonusValue3
            //wsReport.GetCell(String.Format("AO{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AO")));//LeadsInBatch3
            //wsReport.GetCell(String.Format("AP{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AP")));//LeadsAvailable3
            //wsReport.GetCell(String.Format("AQ{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AQ")));//RenewalLeadsAvailable3
            //wsReport.GetCell(String.Format("AR{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AR")));//TargetSales3
            //wsReport.GetCell(String.Format("AS{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AS")));//Sales3

            //wsReport.GetCell(String.Format("AT{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING(AS{0}/AO{0}, 0.01)", reportRow + 1));
            //wsReport.GetCell(String.Format("AU{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AU")));
            //wsReport.GetCell(String.Format("AV{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AV")));//StillToContact3
            //wsReport.GetCell(String.Format("AW{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AW")));//SalesInPreviousWeek3

            //wsReport.GetCell(String.Format("AX{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((AO{0}-AV{0})/AO{0}, 0.01)", reportRow + 1));
            //wsReport.GetCell(String.Format("AY{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AY")));//Falloffs3

            //=========================================================================================================================================================
            //Month3
            wsReport.GetCell(String.Format("AN{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AN")));//BonusValue3
            wsReport.GetCell(String.Format("AQ{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AQ")));//LeadsInBatch3
            wsReport.GetCell(String.Format("AR{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AR")));//LeadsAvailable3
            wsReport.GetCell(String.Format("AS{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AS")));//RenewalLeadsAvailable3
            wsReport.GetCell(String.Format("AT{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AT")));//TargetSales3
            wsReport.GetCell(String.Format("AU{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AU")));//Sales3

            wsReport.GetCell(String.Format("AV{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AV")));//Units3

            wsReport.GetCell(String.Format("AW{0}", reportRow + 1)).ApplyFormula(String.Format("=AU{0}/AQ{0}", reportRow + 1));//Current STL %
            wsReport.GetCell(String.Format("AX{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AX")));
            wsReport.GetCell(String.Format("AY{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AY")));//StillToContact3
            wsReport.GetCell(String.Format("AZ{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "AZ")));//SalesInPreviousWeek3

            wsReport.GetCell(String.Format("BA{0}", reportRow + 1)).ApplyFormula(String.Format("=CEILING((AQ{0}-AY{0})/AQ{0}, 0.01)", reportRow + 1));
            wsReport.GetCell(String.Format("BB{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBody.Replace("_COLUMN_", "BB")));//Falloffs3

            #endregion Finally, insert the grand totals
        }

        //private void InsertUpgradeBatchReportData(Worksheet wsTemplate, Worksheet wsReport, DataSet dsReportData, byte templateRowSpan, int reportRow)
        //{
        //    #region Partition the given DataSet

        //    DataRow drReportConfigs = dsReportData.Tables[0].Rows[0];
        //    DataTable dtReportData = dsReportData.Tables[1];
        //    DataTable dtUpgradeBatchReportSegments = dsReportData.Tables[2];
        //    DataTable dtColumnCellMappings = dsReportData.Tables[3];

        //    List<string> distinctFilteringStrings = (from row 
        //                                             in dtUpgradeBatchReportSegments.AsEnumerable()
        //                                             select row.Field<string>("ReportSegmentFilterString")).Distinct().ToList();

        //    #endregion Partition the given DataSet

        //    #region Declarations & Initializations

        //    //string grandTotalFormulaBody = String.Empty;
        //    string grandTotalFormulaBodyMonth1 = String.Empty;
        //    string grandTotalFormulaBodyMonth2 = String.Empty;
        //    string grandTotalFormulaBodyMonth3 = String.Empty;

        //    int segmentFromRow = reportRow;
        //    int segmentToRow = reportRow;

        //    #endregion Declarations & Initializations

        //    #region Get the configs

        //    byte reportDataTemplateRowIndex = Convert.ToByte(drReportConfigs["ReportDataTemplateRowIndex"]);
        //    byte templateColumnSpan = Convert.ToByte(drReportConfigs["TemplateColumnSpan"]);
        //    byte reportSegmentSeparatorTemplateRowIndex = Convert.ToByte(drReportConfigs["ReportSegmentSeparatorTemplateRowIndex"]);
        //    byte grandTotalTemplateRowIndex = Convert.ToByte(drReportConfigs["GrandTotalTemplateRowIndex"]);

        //    string month1ColumnHeadingCell = drReportConfigs["Month1ColumnHeadingCell"].ToString();
        //    string month2ColumnHeadingCell = drReportConfigs["Month2ColumnHeadingCell"].ToString();
        //    string month3ColumnHeadingCell = drReportConfigs["Month3ColumnHeadingCell"].ToString();

        //    string month1ColumnHeading = String.Format("{0} Bonus Value", drReportConfigs["Month1"].ToString());
        //    string month2ColumnHeading = String.Format("{0} Bonus Value", drReportConfigs["Month2"].ToString());
        //    string month3ColumnHeading = String.Format("{0} Bonus Value", drReportConfigs["Month3"].ToString());

        //    #endregion Get the configs

        //    #region Rename the column headings

        //    wsReport.GetCell(month1ColumnHeadingCell).Value = month1ColumnHeading;
        //    wsReport.GetCell(month2ColumnHeadingCell).Value = month2ColumnHeading;
        //    wsReport.GetCell(month3ColumnHeadingCell).Value = month3ColumnHeading;

        //    #endregion Rename the column headings

        //    //foreach (string currentFilteringString in distinctFilteringStrings)
        //    for (int a = 0; a < distinctFilteringStrings.Count; a++)
        //    {
        //        bool hasTemplatizedRowsBeenInserted = false;

        //        // Determine how many rows should be in the current report segment:
        //        byte maxRowsInSegment = 1;

        //        for (byte month = 1; month <= 3; month++)
        //        {
        //            segmentFromRow = reportRow;

        //            if (dtUpgradeBatchReportSegments.Select(String.Format("{0} AND [Month] = {1}", distinctFilteringStrings[a], month)).ToList().Count > 0)
        //            {
        //                // Do the necessary filtering so that only the current campaign type - and group and be included in the current datatable
        //                DataTable dtCurrentUpgradeReportSegmentForMonth = dtUpgradeBatchReportSegments.Select(String.Format("{0} AND [Month] = {1}", distinctFilteringStrings[a], month)).CopyToDataTable();
        //                DataTable dtCurrentMonthColumnCellMappings = dtColumnCellMappings.Select(String.Format("[Month] = {0}", month)).CopyToDataTable();
        //                DataTable dtCurrentMonthSegmentReportData = dtReportData.Select(String.Format("{0} AND [Month] = {1}", distinctFilteringStrings[a], month)).CopyToDataTable();

        //                #region Insert templatized rows

        //                if (!hasTemplatizedRowsBeenInserted)
        //                {
        //                    foreach (DataRow dr in dtUpgradeBatchReportSegments.Select(distinctFilteringStrings[a]).CopyToDataTable().Rows)
        //                    //foreach (DataRow dr in dtCurrentUpgradeReportSegmentForMonth.Rows)
        //                    {
        //                        byte rowsInSegment = dr.Field<byte>("RowsInSegment");
        //                        maxRowsInSegment = Math.Max(rowsInSegment, maxRowsInSegment);
        //                    }

        //                    //Based on the value of maxRowsInSegment, insert the number of templatized rows:
        //                    for (int b = 0; b < maxRowsInSegment; b++)
        //                    {
        //                        Methods.CopyExcelRegion(wsTemplate, reportDataTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow + b, 0);
        //                    }

        //                    hasTemplatizedRowsBeenInserted = true;
        //                }

        //                #endregion Insert templatized rows

        //                //foreach (DataRow drCurrentSegmentRow in dtCurrentMonthSegmentReportData.Rows)
        //                for (int c = 0; c < dtCurrentMonthSegmentReportData.Rows.Count; c++)
        //                {
        //                    foreach (DataRow drCurrentMonthColumnCellMapping in dtCurrentMonthColumnCellMappings.Rows)
        //                    {
        //                        string excelColumn = drCurrentMonthColumnCellMapping["ExcelColumn"].ToString();
        //                        string dataTableColumnNameOrFormula = drCurrentMonthColumnCellMapping["DataTableColumnNameOrFormula"].ToString();
        //                        string destinationCell = String.Format("{0}{1}", excelColumn, reportRow + 1 + c);

        //                        if (dataTableColumnNameOrFormula.StartsWith("="))
        //                        {
        //                            dataTableColumnNameOrFormula = dataTableColumnNameOrFormula.Replace("#ROW#", (reportRow + 1 + c).ToString());
        //                            wsReport.GetCell(destinationCell).ApplyFormula(dataTableColumnNameOrFormula);
        //                        }
        //                        else
        //                        {
        //                            wsReport.GetCell(destinationCell).Value = dtCurrentMonthSegmentReportData.Rows[c][dataTableColumnNameOrFormula]; //dataTableColumnNameOrFormula;
        //                        }
        //                    }
        //                }

        //                segmentToRow += (dtCurrentMonthSegmentReportData.Rows.Count - 1);
        //            }
        //            //reportRow++;

        //            #region Update the grand totals formulas

        //            if (month == 1)
        //            {
        //                #region Grand Totals For Month 1 

        //                if (grandTotalFormulaBodyMonth1.Length == 0)
        //                {
        //                    if (segmentFromRow == segmentToRow)
        //                    {
        //                        grandTotalFormulaBodyMonth1 = String.Format("_COLUMN_{0}", segmentToRow + 1);
        //                    }
        //                    else
        //                    {
        //                        grandTotalFormulaBodyMonth1 = String.Format("_COLUMN_{0}:_COLUMN_{1}", segmentFromRow + 1, segmentToRow + 1);
        //                    }
        //                }
        //                else
        //                {
        //                    if (segmentFromRow == segmentToRow)
        //                    {
        //                        grandTotalFormulaBodyMonth1 = String.Format("{0},_COLUMN_{1}", grandTotalFormulaBodyMonth1, segmentToRow + 1);
        //                    }
        //                    else
        //                    {
        //                        grandTotalFormulaBodyMonth1 = String.Format("{0},_COLUMN_{1}:_COLUMN_{2}", grandTotalFormulaBodyMonth1, segmentFromRow + 1, segmentToRow + 1);
        //                    }
        //                }

        //                #endregion Grand Totals For Month 1
        //            }

        //            else if (month == 2)
        //            {
        //                #region Grand Totals For Month 2 

        //                if (grandTotalFormulaBodyMonth2.Length == 0)
        //                {
        //                    if (segmentFromRow == segmentToRow)
        //                    {
        //                        grandTotalFormulaBodyMonth2 = String.Format("_COLUMN_{0}", segmentToRow + 1);
        //                    }
        //                    else
        //                    {
        //                        grandTotalFormulaBodyMonth2 = String.Format("_COLUMN_{0}:_COLUMN_{1}", segmentFromRow + 1, segmentToRow + 1);
        //                    }
        //                }
        //                else
        //                {
        //                    if (segmentFromRow == segmentToRow)
        //                    {
        //                        grandTotalFormulaBodyMonth2 = String.Format("{0},_COLUMN_{1}", grandTotalFormulaBodyMonth2, segmentToRow + 1);
        //                    }
        //                    else
        //                    {
        //                        grandTotalFormulaBodyMonth2 = String.Format("{0},_COLUMN_{1}:_COLUMN_{2}", grandTotalFormulaBodyMonth2, segmentFromRow + 1, segmentToRow + 1);
        //                    }
        //                }

        //                #endregion Grand Totals For Month 2
        //            }

        //            else if (month == 3)
        //            {
        //                #region Grand Totals For Month 3

        //                if (grandTotalFormulaBodyMonth3.Length == 0)
        //                {
        //                    if (segmentFromRow == segmentToRow)
        //                    {
        //                        grandTotalFormulaBodyMonth3 = String.Format("_COLUMN_{0}", segmentToRow + 1);
        //                    }
        //                    else
        //                    {
        //                        grandTotalFormulaBodyMonth3 = String.Format("_COLUMN_{0}:_COLUMN_{1}", segmentFromRow + 1, segmentToRow + 1);
        //                    }
        //                }
        //                else
        //                {
        //                    if (segmentFromRow == segmentToRow)
        //                    {
        //                        grandTotalFormulaBodyMonth3 = String.Format("{0},_COLUMN_{1}", grandTotalFormulaBodyMonth3, segmentToRow + 1);
        //                    }
        //                    else
        //                    {
        //                        grandTotalFormulaBodyMonth3 = String.Format("{0},_COLUMN_{1}:_COLUMN_{2}", grandTotalFormulaBodyMonth3, segmentFromRow + 1, segmentToRow + 1);
        //                    }
        //                }

        //                #endregion Grand Totals For Month 3
        //            }

        //            #endregion Update the grand totals formulas
        //        }

        //        reportRow += maxRowsInSegment;
        //        //segmentToRow = reportRow;
        //        //reportRow++;

        //        #region Insert the row separator

        //        if (a < distinctFilteringStrings.Count - 1)
        //        {
        //            Methods.CopyExcelRegion(wsTemplate, reportSegmentSeparatorTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);
        //            reportRow++;
        //        }

        //        #endregion Insert the row separator

        //    }

        #region OLD

        //for (int i = 0; i < dtUpgradeBatchReportSegments.Rows.Count; i++)
        //{
        //    //DateTime dateReceivedFromBatchCode = Convert.ToDateTime(drBatchReceiveDate["DateReceivedFromBatchCode"]);
        //    //DataTable dtBatchesReceivedForDate = dtReportData.Select(String.Format("DateReceivedFromBatchCode = '{0}'", dateReceivedFromBatchCode.ToString("yyyy-MM-dd"))).CopyToDataTable();

        //    //long fkINCampaignGroupID = Convert.ToInt64(dtUpgradeBatchReportSegments.Rows[i]["FKINCampaignGroupID"]);
        //    //long fkINCampaignTypeID = Convert.ToInt64(dtUpgradeBatchReportSegments.Rows[i]["FKINCampaignTypeID"]);
        //    //long campaignsInSegment = Convert.ToByte(dtUpgradeBatchReportSegments.Rows[i]["CampaignsInSegment"]);
        //    //DataTable dtReportDataSubset = dtReportData.Select(String.Format("[FKINCampaignGroupID] = {0} AND [FKINCampaignTypeID] = {1}", fkINCampaignGroupID, fkINCampaignTypeID)).CopyToDataTable();

        //    byte month = Convert.ToByte(dtUpgradeBatchReportSegments.Rows[i]["Month"]);
        //    string reportSegmentFilterString = dtUpgradeBatchReportSegments.Rows[i]["ReportSegmentFilterString"].ToString();
        //    DataTable dtReportDataSubset = dtReportData.Select(reportSegmentFilterString).CopyToDataTable();

        //    segmentFromRow = reportRow;

        //    #region Firstly, determine how many rows will be necesssary for the current segment (across all 3 months) and add the templatized rows

        //    byte maxRowsInSegment = 1;

        //    foreach (DataRow dr in dtUpgradeBatchReportSegments.Select(reportSegmentFilterString).CopyToDataTable().Rows)
        //    {
        //        byte rowsInSegment = dr.Field<byte>("RowsInSegment");
        //        maxRowsInSegment = Math.Max(rowsInSegment, maxRowsInSegment);
        //    }

        //    for (int a = 0; a < maxRowsInSegment; a++)
        //    {
        //        Methods.CopyExcelRegion(wsTemplate, reportDataTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow + a, 0);
        //    }

        //    #endregion Firstly, determine how many rows will be necesssary for the current segment (across all 3 months) and add the templatized rows

        //    foreach (DataRow drCurrentSegmentRow in dtReportDataSubset.Rows)
        //    {
        //        //byte month = Convert.ToByte(drCurrentSegmentRow["Month"]);
        //        DataTable dtCurrentMonthColumnMappings = dtColumnCellMappings.Select(String.Format("[Month] = {0}", month)).CopyToDataTable();

        //        foreach (DataRow drColumnRowMapping in dtCurrentMonthColumnMappings.Rows)
        //        {
        //            string excelColumn = drColumnRowMapping["ExcelColumn"].ToString();
        //            string dataTableColumnNameOrFormula = drColumnRowMapping["DataTableColumnNameOrFormula"].ToString();

        //            if (dataTableColumnNameOrFormula.StartsWith("="))
        //            {
        //                dataTableColumnNameOrFormula = dataTableColumnNameOrFormula.Replace("#ROW#", (reportRow + 1).ToString());
        //                wsReport.GetCell(String.Format("{0}{1}", excelColumn, reportRow + 1)).ApplyFormula(dataTableColumnNameOrFormula);
        //            }
        //            else
        //            {
        //                wsReport.GetCell(String.Format("{0}{1}", excelColumn, reportRow + 1)).Value = drCurrentSegmentRow[dataTableColumnNameOrFormula]; //dataTableColumnNameOrFormula;
        //            }
        //        }

        //        segmentToRow = reportRow;
        //        reportRow++;

        //    }

        #endregion OLD

        #region Update the grand totals formulas OLD

        #region Grand Totals For Month 1 

        //if (grandTotalFormulaBody.Length == 0)
        //{
        //    if (segmentFromRow == segmentToRow)
        //    {
        //        grandTotalFormulaBody = String.Format("_COLUMN_{0}", segmentToRow + 1);
        //    }
        //    else
        //    {
        //        grandTotalFormulaBody = String.Format("_COLUMN_{0}:_COLUMN_{1}", segmentFromRow + 1, segmentToRow + 1);
        //    }
        //}
        //else
        //{
        //    if (segmentFromRow == segmentToRow)
        //    {
        //        grandTotalFormulaBody = String.Format("{0},_COLUMN_{1}", grandTotalFormulaBody, segmentToRow + 1);
        //    }
        //    else
        //    {
        //        grandTotalFormulaBody = String.Format("{0},_COLUMN_{1}:_COLUMN_{2}", grandTotalFormulaBody, segmentFromRow + 1, segmentToRow + 1);
        //    }
        //}

        #endregion Grand Totals For Month 1

        #endregion Update the grand totals formulas OLD

        //#region Insert the row separator

        ////if (a < dtUpgradeBatchReportSegments.Rows.Count - 1)
        ////{
        ////    Methods.CopyExcelRegion(wsTemplate, reportSegmentSeparatorTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);
        ////    reportRow++;
        ////}

        //#endregion Insert the row separator


        //#region Finally, insert the grand totals

        //Methods.CopyExcelRegion(wsTemplate, grandTotalTemplateRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

        //    wsReport.GetCell(String.Format("B{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth1.Replace("_COLUMN_", "B")));
        //    wsReport.GetCell(String.Format("E{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth1.Replace("_COLUMN_", "E")));
        //    wsReport.GetCell(String.Format("F{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth1.Replace("_COLUMN_", "F")));
        //    wsReport.GetCell(String.Format("G{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth1.Replace("_COLUMN_", "G")));
        //    wsReport.GetCell(String.Format("H{0}", reportRow + 1)).ApplyFormula(String.Format("=G{0}/E{0}", reportRow + 1));
        //    wsReport.GetCell(String.Format("I{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth1.Replace("_COLUMN_", "I")));
        //    wsReport.GetCell(String.Format("J{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth1.Replace("_COLUMN_", "J")));

        //    wsReport.GetCell(String.Format("N{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth2.Replace("_COLUMN_", "N")));
        //    wsReport.GetCell(String.Format("Q{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth2.Replace("_COLUMN_", "Q")));
        //    wsReport.GetCell(String.Format("R{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth2.Replace("_COLUMN_", "R")));
        //    wsReport.GetCell(String.Format("S{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth2.Replace("_COLUMN_", "S")));
        //    wsReport.GetCell(String.Format("T{0}", reportRow + 1)).ApplyFormula(String.Format("=S{0}/Q{0}", reportRow + 1));
        //    wsReport.GetCell(String.Format("U{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth2.Replace("_COLUMN_", "U")));
        //    wsReport.GetCell(String.Format("V{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth2.Replace("_COLUMN_", "V")));

        //    wsReport.GetCell(String.Format("Z{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth3.Replace("_COLUMN_", "Z")));
        //    wsReport.GetCell(String.Format("AC{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth3.Replace("_COLUMN_", "AC")));
        //    wsReport.GetCell(String.Format("AD{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth3.Replace("_COLUMN_", "AD")));
        //    wsReport.GetCell(String.Format("AE{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth3.Replace("_COLUMN_", "AE")));
        //    wsReport.GetCell(String.Format("AF{0}", reportRow + 1)).ApplyFormula(String.Format("=AE{0}/AC{0}", reportRow + 1));
        //    wsReport.GetCell(String.Format("AG{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth3.Replace("_COLUMN_", "AG")));
        //    wsReport.GetCell(String.Format("AH{0}", reportRow + 1)).ApplyFormula(String.Format("=SUM({0})", grandTotalFormulaBodyMonth3.Replace("_COLUMN_", "AH")));

        //    #endregion Finally, insert the grand totals
        //}

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
                IEnumerable<DataRecord> campaigns = xdgCampaigns.Records.Cast<DataRecord>().ToArray();

                btnClose.IsEnabled = false;
                btnReport.IsEnabled = false;
                xdgCampaigns.IsEnabled = false;

                #region Get a string of campaignIDs

                string dataTableColumnName = String.Empty;

                if (_reportTypeID == 1)
                {
                    dataTableColumnName = "CampaignTypeID";
                }
                else
                {
                    dataTableColumnName = "CampaignID";
                }

                var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                _lstSelectedFKINCampaignIDs = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells[dataTableColumnName].Value));

                if (_lstSelectedFKINCampaignIDs.Count > 0)
                {
                    _campaignIDs = _lstSelectedFKINCampaignIDs.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells[dataTableColumnName].Value + ",");
                    _campaignIDs = _campaignIDs.Substring(0, _campaignIDs.Length - 1);
                }

                #endregion Get a string of campaignIDs

                #region Setting the values of the input parameter values

                _selectedReportType = ((DataRowView)cmbReportType.SelectedItem).Row.ItemArray[1].ToString();

                //DateTime fromDate = new DateTime(_year, _month, 1);
                //fromDate = fromDate.AddMonths(-2);
                //int fromMonth = fromDate.Month;
                //int lastDay = DateTime.DaysInMonth(_year, _month);
                //DateTime toDate = new DateTime(_year, _month, lastDay);
                //int toMonth = toDate.Month;

                //if (_reportTypeID == 2)
                //{
                //    DateTime initialToDate = new DateTime(_year, _month, 1).AddMonths(1).AddDays(-1);
                //    _endDate = initialToDate;

                //    DateTime initialFromDate = new DateTime(_year, _month - 2, 1);
                //    _startDate = initialFromDate;
                //}

                //if (_reportTypeID == 1 || _reportTypeID == 4 || _reportTypeID == 5 || _reportTypeID == 6 || _reportTypeID == 7)
                //{
                //    fromDate = _startDate;
                //    toDate = _endDate;
                //}

                #endregion Setting the values of the input parameter values

                if (RData.SalesConversionPerBatch || RData.ContactsConversionPerBatch)
                {
                    BackgroundWorker conversionWorker = new BackgroundWorker();
                    conversionWorker.DoWork += ReportConversion;
                    conversionWorker.RunWorkerCompleted += ReportCompletedConversion;
                    conversionWorker.RunWorkerAsync(campaigns);
                }
                else
                {
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += Report;
                    worker.RunWorkerCompleted += ReportCompleted;
                    worker.RunWorkerAsync(campaigns);
                }


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
                btnReport.IsEnabled = true;
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
                btnReport.IsEnabled = true;
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
                //Action act = delegate ()
                //{
                //    //_calendar.SelectedDate = ((ViewModel)DataContext).Display;
                //    clYear.DisplayMode = CalendarMode.Year;
                //    //_calendar.SelectedDate = null;
                //};
                //Dispatcher.BeginInvoke(act, DispatcherPriority.ApplicationIdle);
                //_year = clYear.DisplayDate.Year;
                //clYear.DisplayMode = CalendarMode.Year;
                //add Months
                System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                for (int i = 1; i <= 12; i++)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Tag = i;
                    item.Content = mfi.GetMonthName(i);
                    cmbMonth.Items.Add(item);
                }
                cmbMonth.SelectedIndex = DateTime.Now.Month - 1;
                _month = DateTime.Now.Month;

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void clYear_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            try
            {
                _year = clYear.DisplayDate.Year;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        //private void clYear_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    _year = clYear.DisplayDate.Year;
        //}

        private void cmbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)cmbMonth.SelectedItem;
            _month = int.Parse(item.Tag.ToString());
        }

        private void cmbReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //ComboBoxItem selectedItem = (ComboBoxItem)cmbReportType.SelectedItem;
                //_reportTypeID = int.Parse(selectedItem.Tag.ToString());
                //LoadCampaignInfo(_reportTypeID);
                if (cmbReportType.SelectedValue != null)
                {
                    SetFormMode(Convert.ToByte(cmbReportType.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void chkIncludeLeadsCopiedToExtension_Checked(object sender, RoutedEventArgs e)
        {
            _includeLeadsCopiedToExtension = true;//chkIncludeLeadsCopiedToExtension.IsChecked;
        }

        private void chkIncludeLeadsCopiedToExtension_Unchecked(object sender, RoutedEventArgs e)
        {
            _includeLeadsCopiedToExtension = false; // chkIncludeLeadsCopiedToExtension.IsChecked;
        }


        //private void Cal1_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        //{
        //    DateTime.TryParse(calStartDate.SelectedDate.ToString(), out _startDate);

        //}

        //private void Cal2_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        //{
        //    DateTime.TryParse(calEndDate.SelectedDate.ToString(), out _endDate);

        //}

        private void btnMarkCompletedBatches_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Popup1.IsOpen = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmbCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                long selectedCampaignID = long.Parse(cmbCampaign.SelectedValue.ToString());
                DataTable dtBatches = Methods.GetTableData("select * from INBatch where FKINCampaignID = " + selectedCampaignID + " order by Code");
                lstBatches.Items.Clear();
                foreach (DataRow row in dtBatches.Rows)
                {
                    CheckBox chk = new CheckBox();
                    chk.Content = row["Code"].ToString();
                    chk.Tag = row["ID"].ToString();
                    if (row["Completed"].ToString().ToLower() == "true")
                    {
                        chk.IsChecked = true;
                    }
                    lstBatches.Items.Add(chk);
                }
                if (dtBatches.Rows.Count > 0)
                {
                    btnPopupMark.IsEnabled = true;
                }
                else
                {
                    btnPopupMark.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnPopupMark_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (CheckBox chk in lstBatches.Items)
                {
                    object userID = Database.GetParameter("@BatchID", long.Parse(chk.Tag.ToString()));
                    object password = Database.GetParameter("@IsCompleted", chk.IsChecked);
                    object[] paramArray = new[] { userID, password };
                    Database.ExecuteDataSet(null, CommandType.StoredProcedure, "spUpdateBatchCompleted", paramArray, 600);
                }
                ShowMessageBox(new INMessageBoxWindow1(), "Batches Marked Succesfully", "Marked", ShowMessageType.Information);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void chkIncludeCompleted_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                _includeCompletedBatches = true; //chkIncludeCompleted.IsChecked;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void chkIncludeCompleted_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                _includeCompletedBatches = false; //chkIncludeCompleted.IsChecked;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void chkOnlyBatchesImported91DaysAndAfter_Checked(object sender, RoutedEventArgs e)
        {
            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/221462601/comments
            _onlyBatchesReceived91DaysAgoAndAfter = true;
        }

        private void chkOnlyBatchesImported91DaysAndAfter_Unchecked(object sender, RoutedEventArgs e)
        {
            // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/221462601/comments
            _onlyBatchesReceived91DaysAgoAndAfter = false;
        }

        #endregion Event Handlers 

    }

}
