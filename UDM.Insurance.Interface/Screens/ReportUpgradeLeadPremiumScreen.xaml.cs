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

namespace UDM.Insurance.Interface.Screens
{
    public partial class ReportUpgradeLeadPremiumScreen
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

        public ReportUpgradeLeadPremiumScreen()
        {
            InitializeComponent();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

        private bool IsAllInputParametersSpecifiedAndValid()
        {

            #region Ensuring that the From Date was specified

            if (calFromDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'From Date'.", @"No 'From Date' specified", ShowMessageType.Error);
                return false;
            }
            else
            {
                _fromDate = calFromDate.SelectedDate.Value;
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
                _toDate = calToDate.SelectedDate.Value;
            }

            #endregion Ensuring that the To Date was specified

            #region Ensuring that the date range is valid

            if (_fromDate > _toDate)
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

            // Otherwise, if all is well, proceed:
            return true;
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Setup Excel document

                string filePathAndName = String.Format("{0}Upgrade Lead Premium Report, {1} - {2} ~ {3}.xlsx",
                    GlobalSettings.UserFolder,
                    _fromDate.ToString("yyyy-MM-dd"),
                    _toDate.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                Workbook wbUpgradeLeadPremiumReportTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateUpgradeLeadPremium.xlsx");
                Workbook wbUpgradeLeadPremiumReport = new Workbook(WorkbookFormat.Excel2007);

                #endregion Setup Excel document

                #region Get the data
                _toDate = _toDate.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                DataSet dsUpgradeLeadPremiumData = Business.Insure.ReportUpgradeLeadPremium(_fromDate, _toDate);

                if (dsUpgradeLeadPremiumData.Tables[1].Rows.Count == 0)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), @"There is no data from which to generate a report.", "No Data", ShowMessageType.Information);
                    });
                    
                    return;
                }

                

                #endregion Get the data

                InsertUpgradeLeadPremiumSheet(wbUpgradeLeadPremiumReportTemplate, wbUpgradeLeadPremiumReport, dsUpgradeLeadPremiumData);

                //InsertBumpUpDataSheet(wbUpgradeLeadPremiumReportTemplate, wbUpgradeLeadPremiumReport, dsUpgradeLeadPremiumData);

                #region Finally, save, and display the resulting workbook

                if (wbUpgradeLeadPremiumReport.Worksheets.Count > 0)
                {
                    wbUpgradeLeadPremiumReport.Save(filePathAndName);

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

        private void InsertBumpUpDataSheet(Workbook wbBumpUpStatsReportTemplate, Workbook wbBumpUpStatsReport, DataSet dsBumpUpStatsReportData)
        {
            try
            {
                int rowIndex = 7;
                int columnSpan = 16;
                DataTable dtBumpUpDataSheet = dsBumpUpStatsReportData.Tables[6];

                DataTable dtReportConfig = dsBumpUpStatsReportData.Tables[0];
                

                Worksheet wsTemplate = wbBumpUpStatsReportTemplate.Worksheets["DataSheet"];
                Worksheet wsReport = wbBumpUpStatsReport.Worksheets.Add("BumpUpDatasheet");

                //Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsReport, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true);

                Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                Methods.CopyExcelRegion(wsTemplate, 0, 0, 6, columnSpan, wsReport, 0, 0);

                wsReport.GetCell("Subtitle").Value = dtReportConfig.Rows[0]["DataSheetSubTitle"].ToString();
                wsReport.GetCell("ReportDate1").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                wsReport.Workbook.NamedReferences.Clear();

                foreach (DataRow row in dtBumpUpDataSheet.Rows)
                {
                    Methods.CopyExcelRegion(wsTemplate, 7, 0, 0, columnSpan, wsReport, rowIndex, 0);

                    wsReport.GetCell("RefNo").Value = row["Ref No"].ToString();
                    wsReport.GetCell("Campaign").Value = row["Code"].ToString();
                    wsReport.GetCell("DateOfSale").Value = row["Date"].ToString() != "" ? DateTime.Parse(row["Date"].ToString()).ToString("yyyy-MM-dd") : "";
                    wsReport.GetCell("TSR").Value = row["TSR"].ToString();
                    wsReport.GetCell("SaleTime").Value = row["SaleTime"].ToString();
                    wsReport.GetCell("BumpUpAllocation").Value = row["ConfirmationAgent"].ToString();
                    wsReport.GetCell("DateAllocated").Value = row["DateAllocated"].ToString() != "" ? DateTime.Parse(row["DateAllocated"].ToString()).ToString("yyyy-MM-dd") : "";
                    wsReport.GetCell("DateContacted").Value = row["ContactDate"].ToString() != "" ? DateTime.Parse(row["ContactDate"].ToString()).ToString("yyyy-MM-dd") : "";
                    wsReport.GetCell("TimeContacted").Value = row["ContactTime"].ToString();
                    wsReport.GetCell("Status").Value = row["Status"].ToString();
                    wsReport.GetCell("ContactStatus").Value = row["ContactStatus"].ToString();
                    wsReport.GetCell("BumpedUp").Value = row["BumpedUp"].ToString();
                    wsReport.GetCell("OfferedBumpUp").Value = row["OfferedBumpUp"].ToString();
                    wsReport.GetCell("BumpUpDescription").Value = row["BumpUpDescription"].ToString();
                    wsReport.GetCell("BumpUpReducedPremiumStatus").Value = row["BumpUpReducedPremiumStatus"].ToString();
                    wsReport.GetCell("OldPremium").Value = row["OldPremium"].ToString();
                    wsReport.GetCell("NewPremium").Value = row["NewPremium"].ToString();

                    wsReport.Workbook.NamedReferences.Clear();
                    rowIndex++;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void InsertUpgradeLeadPremiumSheet(Workbook wbBumpUpStatsReportTemplate, Workbook wbBumpUpStatsReport, DataSet dsUpgradeLeadPremiumReportData)
        {
            #region Split Up Dataset into Datatables

            DataTable dtBatchMonths = dsUpgradeLeadPremiumReportData.Tables[0];
            //DataTable dtReportData = dsUpgradeLeadPremiumReportData.Tables[1].AsEnumerable().Where(x => x["CampaignCode"].ToString() != "Total").CopyToDataTable();
            DataTable dtReportData = dsUpgradeLeadPremiumReportData.Tables[1];

            //DataTable dtReportTotals = dsUpgradeLeadPremiumReportData.Tables[1].AsEnumerable().Where(x => x["CampaignCode"].ToString() == "Total").CopyToDataTable();

            #endregion Split Up Dataset into Datatables

            Worksheet wsTemplate = wbBumpUpStatsReportTemplate.Worksheets["Report"];
            Worksheet wsReport = wbBumpUpStatsReport.Worksheets.Add("UpgradeLeadPremiumReport");

            #region Set Up Template
            int columnSpanBatchMonth = 17;
            int columnSpanCampaignTotal = 18;
            int rowSpan = 4;

            int columnSpan = (dtBatchMonths.Rows.Count * columnSpanBatchMonth + columnSpanCampaignTotal);
            int columnIndex = 0;                
            foreach (DataRow row in dtBatchMonths.Rows)
            {
                if (columnIndex == 0)
                {
                    Methods.CopyExcelRegion(wsTemplate, 0, 0, rowSpan, columnSpanCampaignTotal, wsReport, 0, 0);
                    wsReport.GetCell("ReportDateRange").Value = _fromDate.ToString("MMM yyyy") + " - " + _toDate.ToString("MMM yyyy");
                    columnIndex += columnSpanCampaignTotal;
                }
                else
                {
                    Methods.CopyExcelRegion(wsTemplate, 0, 1, rowSpan, columnSpanBatchMonth - 1, wsReport, 0, columnIndex);
                    columnIndex += columnSpanBatchMonth;
                }
                wsReport.GetCell("BatchMonth").Value = row["BatchMonth"].ToString();
                wsReport.Workbook.NamedReferences.Clear();
            }

            Methods.CopyExcelRegion(wsTemplate, 0, 18, rowSpan, columnSpanCampaignTotal, wsReport, 0, columnIndex);
            
            string campaignTotalTitle = "Average: " + _fromDate.ToString("MMM yyyy")  + " - " + _toDate.ToString("MMM yyyy") + " Upgrade Batches Received";
            wsReport.GetCell("CampaignTotalTitle").Value = campaignTotalTitle;
            wsReport.Workbook.NamedReferences.Clear();


            //columnIndex += columnSpanCampaignTotal;



            #endregion Set Up Template

            //Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsReport, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true);

            Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

            //Methods.CopyExcelRegion(wsTemplate, 0, 0, 6, columnSpan, wsReport, 0, 0);

            #region Populate Report Data
            int rowIndex = 5;
            int totalColumnNumber = dtBatchMonths.Rows.Count + 1;
            foreach (DataRow campaign in dtReportData.Rows)
            {
                columnIndex = 0;
                //foreach (DataRow row in dtBatchMonths.Rows)
                for (int i = 0; i < dtBatchMonths.Rows.Count; i++)
                {
                    if (columnIndex == 0)
                    {
                        //if (campaign["CampaignCode"].ToString() != "Total")
                        //{
                            Methods.CopyExcelRegion(wsTemplate, 5, 0, 0, columnSpanCampaignTotal, wsReport, rowIndex, 0);
                            wsReport.GetCell("CampaignCode").Value = campaign["CampaignCode"];
                        //}
                        //else if (campaign["CampaignCode"].ToString() == "Total")
                        //{
                        //    Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, columnSpanCampaignTotal, wsReport, rowIndex, 0);
                        //}
                        

                        columnIndex += columnSpanCampaignTotal;
                    }
                    else
                    {
                        //if (campaign["CampaignCode"].ToString() != "Total")
                        //{
                            Methods.CopyExcelRegion(wsTemplate, 5, 1, 0, columnSpanBatchMonth - 1, wsReport, rowIndex, columnIndex);
                            columnIndex += columnSpanBatchMonth;
                            //wsReport.Workbook.NamedReferences.Clear(); //comment out after testing
                            //break; //comment out after testing
                        //}
                        //else if (campaign["CampaignCode"].ToString() == "Total")
                        //{
                        //    Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, columnSpanCampaignTotal, wsReport, rowIndex, 0);
                        //}

                    }
                    string batchMonth = dtBatchMonths.Rows[i]["BatchMonth"].ToString();
                    wsReport.GetCell("TotalLeads").Value = campaign["(" + (i + 1) + ")" + batchMonth + "_TotalLeads"];

                    #region Premium < R100
                    wsReport.GetCell("LeadPremiumLessR100").Value = campaign["(" + (i + 1) + ")" + batchMonth + "_Premium<R100"];
                    wsReport.GetCell("LeadPercentageLessR100").Value = campaign["(" + (i + 1) + ")" + batchMonth + "_Lead%<R100"];
                    wsReport.GetCell("SalePremiumLessR100").Value = campaign["(" + (i + 1) + ")" + batchMonth + "_Sale<R100"];
                    wsReport.GetCell("STLPercentageLessR100").Value = campaign["(" + (i + 1) + ")" + batchMonth + "_STL%<R100"];
                    #endregion Premium < R100

                    #region Premium < R200
                    wsReport.GetCell("LeadPremiumLessR200").Value = campaign["(" + (i + 1) + ")" + batchMonth + "_Premium<R200"];
                    wsReport.GetCell("LeadPercentageLessR200").Value = campaign["(" + (i + 1) + ")" + batchMonth + "_Lead%<R200"];
                    wsReport.GetCell("SalePremiumLessR200").Value = campaign["(" + (i + 1) + ")" + batchMonth + "_Sale<R200"];
                    wsReport.GetCell("STLPercentageLessR200").Value = campaign["(" + (i + 1) + ")" + batchMonth + "_STL%<R200"];
                    #endregion Premium < R200

                    #region Premium < R300
                    wsReport.GetCell("LeadPremiumLessR300").Value = campaign["(" + (i + 1) + ")" + batchMonth + "_Premium<R300"];
                    wsReport.GetCell("LeadPercentageLessR300").Value = campaign["(" + (i + 1) + ")" + batchMonth + "_Lead%<R300"];
                    wsReport.GetCell("SalePremiumLessR300").Value = campaign["(" + (i + 1) + ")" + batchMonth + "_Sale<R300"];
                    wsReport.GetCell("STLPercentageLessR300").Value = campaign["(" + (i + 1) + ")" + batchMonth + "_STL%<R300"];
                    #endregion Premium < R300

                    #region Premium > R300
                    wsReport.GetCell("LeadPremiumMoreR300").Value = campaign["(" + (i + 1) + ")" + batchMonth + "_Premium>R300"];
                    wsReport.GetCell("LeadPercentageMoreR300").Value = campaign["(" + (i + 1) + ")" + batchMonth + "_Lead%>R300"];
                    wsReport.GetCell("SalePremiumMoreR300").Value = campaign["(" + (i + 1) + ")" + batchMonth + "_Sale>R300"];
                    wsReport.GetCell("STLPercentageMoreR300").Value = campaign["(" + (i + 1) + ")" + batchMonth + "_STL%>R300"];
                    #endregion Premium > R300

                    

                    //wsReport.GetCell("BatchMonth").Value = row["BatchMonth"].ToString();
                    wsReport.Workbook.NamedReferences.Clear();
                }
                //if (campaign["CampaignCode"].ToString() != "Total")
                //{
                    Methods.CopyExcelRegion(wsTemplate, 5, 18, 0, columnSpanCampaignTotal, wsReport, rowIndex, columnIndex);
                //}
                //else if (campaign["CampaignCode"].ToString() == "Total")
                //{
                //    Methods.CopyExcelRegion(wsTemplate, 6, 18, 0, columnSpanCampaignTotal, wsReport, rowIndex, columnIndex);
                //}

                #region CampaignTotals

                wsReport.GetCell("CampaignTotalLeads").Value = campaign["(" + totalColumnNumber + ")Total_TotalLeads"];

                #region Premium < R100
                wsReport.GetCell("CampaignLeadPremiumLessR100").Value = campaign["(" + totalColumnNumber + ")Total_Premium<R100"];
                wsReport.GetCell("CampaignLeadPercentageLessR100").Value = campaign["(" + totalColumnNumber + ")Total_Lead%<R100"];
                wsReport.GetCell("CampaignSalePremiumLessR100").Value = campaign["(" + totalColumnNumber + ")Total_Sale<R100"];
                wsReport.GetCell("CampaignSTLPercentageLessR100").Value = campaign["(" + totalColumnNumber + ")Total_STL%<R100"];
                #endregion Premium < R100

                #region Premium < R200
                wsReport.GetCell("CampaignLeadPremiumLessR200").Value = campaign["(" + totalColumnNumber + ")Total_Premium<R200"];
                wsReport.GetCell("CampaignLeadPercentageLessR200").Value = campaign["(" + totalColumnNumber + ")Total_Lead%<R200"];
                wsReport.GetCell("CampaignSalePremiumLessR200").Value = campaign["(" + totalColumnNumber + ")Total_Sale<R200"];
                wsReport.GetCell("CampaignSTLPercentageLessR200").Value = campaign["(" + totalColumnNumber + ")Total_STL%<R200"];
                #endregion Premium < R200

                #region Premium < R300
                wsReport.GetCell("CampaignLeadPremiumLessR300").Value = campaign["(" + totalColumnNumber + ")Total_Premium<R300"];
                wsReport.GetCell("CampaignLeadPercentageLessR300").Value = campaign["(" + totalColumnNumber + ")Total_Lead%<R300"];
                wsReport.GetCell("CampaignSalePremiumLessR300").Value = campaign["(" + totalColumnNumber + ")Total_Sale<R300"];
                wsReport.GetCell("CampaignSTLPercentageLessR300").Value = campaign["(" + totalColumnNumber + ")Total_STL%<R300"];
                #endregion Premium < R300

                #region Premium > R300
                wsReport.GetCell("CampaignLeadPremiumMoreR300").Value = campaign["(" + totalColumnNumber + ")Total_Premium>R300"];
                wsReport.GetCell("CampaignLeadPercentageMoreR300").Value = campaign["(" + totalColumnNumber + ")Total_Lead%>R300"];
                wsReport.GetCell("CampaignSalePremiumMoreR300").Value = campaign["(" + totalColumnNumber + ")Total_Sale>R300"];
                wsReport.GetCell("CampaignSTLPercentageMoreR300").Value = campaign["(" + totalColumnNumber + ")Total_STL%>R300"];
                #endregion Premium > R300

                #endregion CampaignTotals
                if (rowIndex == 5)
                {
                    //make the line thick on the top border of the first datarow.
                    for (int i = 1; i < (columnIndex + columnSpanCampaignTotal) - 1; i++)
                    {
                        wsReport.Rows[rowIndex].Cells[i].CellFormat.TopBorderStyle = Infragistics.Documents.Excel.CellBorderLineStyle.Medium;
                    }
                }
                if (campaign["CampaignCode"].ToString() == "Total")
                {
                    //formatting of the total row.
                    for (int i = 0; i < (columnIndex + columnSpanCampaignTotal) - 1; i++)
                    {
                        wsReport.Rows[rowIndex].Cells[i].CellFormat.TopBorderStyle = Infragistics.Documents.Excel.CellBorderLineStyle.Double;
                        wsReport.Rows[rowIndex].Cells[i].CellFormat.BottomBorderStyle = Infragistics.Documents.Excel.CellBorderLineStyle.Thick;
                        wsReport.Rows[rowIndex].Cells[i].CellFormat.Font.Bold = Infragistics.Documents.Excel.ExcelDefaultableBoolean.True;
                    }
                        //wsReport.GetRegion(GetExcelColumnName(2) + rowIndex + ":" + GetExcelColumnName(columnIndex + columnSpanCampaignTotal) + rowIndex);
                }

                wsReport.Workbook.NamedReferences.Clear();
                rowIndex++;
            }

            #endregion Populate Report Data
        }

        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
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
                //ShowMessageBox(new INMessageBoxWindow1(), "This report is still undergoing development, but it will be available soon.", "Under Construction", ShowMessageType.Information);

                if (IsAllInputParametersSpecifiedAndValid())
                {
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

        #endregion Event Handlers

    }
}
