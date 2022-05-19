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
using UDM.Insurance.Business;
using System.Text;
using System.Transactions;
using Infragistics.Documents.Excel;


namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportDiaryScreen.xaml
    /// </summary>
    public partial class ReportDCTransferSales
    {

        #region Constants

        #endregion Constants

        #region Properties
        private bool? _allRecordsChecked = false;


        #endregion


        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        //private RecordCollectionBase _campaigns;
        //private System.Collections.Generic.List<Record> _campaigns;
        private List<Record> _lstSelectedCampaigns;
        private int? _campaignIDList = null;

        private DateTime _startDate;
        private DateTime _endDate;
        DataTable dtSalesData = new DataTable();
        List<int> UserIDs = new List<int>();

        private List<DataRecord> _selectedAgents;
        private List<string> selectedAgentString = new List<string>();

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        DataTable dtSummaryDataData = new DataTable();
        DataTable dtDataSheet = new DataTable();
        DataTable dtByDebiCheckSpecialist = new DataTable();

        string TypeBool = " ";


        private string _fkINCampaignFKINCampaignClusterIDs;

        #endregion Private Members

        #region Constructors

        public ReportDCTransferSales()
        {
            InitializeComponent();

            #region UserType Workings
            switch (((User)GlobalSettings.ApplicationUser).FKUserType)
            {
                case (int)lkpUserType.DebiCheckAgent:
                    lblAgents.Visibility = Visibility.Hidden;
                    xdgAgents.Visibility = Visibility.Hidden;


                    //CheckUserVersion(Username);

                    break;


                case (int)lkpUserType.ConfirmationAgent:
                case (int)lkpUserType.StatusLoader:
                case (int)lkpUserType.CallMonitoringAgent:
                case (int)lkpUserType.Preserver:
                case (int)lkpUserType.SeniorAdministrator:
                case (int)lkpUserType.Administrator:
                case (int)lkpUserType.Manager:
                case (int)lkpUserType.SalesAgent:
                case (int)lkpUserType.DataCapturer:
                    LoadAgentInfo();
                    lblAgents.Visibility = Visibility.Visible;
                    xdgAgents.Visibility = Visibility.Visible;   

                    break;

                default:
                    ShowMessageBox(new INMessageBoxWindow1(), "Invalid User Type for Insure System", "Login Failed", ShowMessageType.Error);
                    break;
            }
            #endregion

            #region DataTable Definitions
            dtSummaryDataData.Columns.Add("Date Range");
            dtSummaryDataData.Columns.Add("Total Calls Transferred");
            dtSummaryDataData.Columns.Add("Accepted Debi-Check Target");
            dtSummaryDataData.Columns.Add("Actual Accepted Debi-Check %");
            dtSummaryDataData.Columns.Add("Accepted Debi-Check %");
            dtSummaryDataData.Columns.Add("Over/Under");
            dtSummaryDataData.Columns.Add("Debi-Checks Rejected");
            dtSummaryDataData.Columns.Add("No Response / Other");
            dtSummaryDataData.Columns.Add("No Response / Other %");
            dtSummaryDataData.Columns.Add("Failed Transfer");
            dtSummaryDataData.Columns.Add("Failed Transfer %");

            dtDataSheet.Columns.Add("DebiCheck Agent");
            dtDataSheet.Columns.Add("Campaign Code"); 
            dtDataSheet.Columns.Add("Reference Number");
            dtDataSheet.Columns.Add("Date of Sale");
            dtDataSheet.Columns.Add("Sales Consultant");
            dtDataSheet.Columns.Add("Lead Status");
            dtDataSheet.Columns.Add("Mandate Status");

            dtByDebiCheckSpecialist.Columns.Add("Debi-Check Specialist");
            dtByDebiCheckSpecialist.Columns.Add("Total Calls Transferred");
            dtByDebiCheckSpecialist.Columns.Add("Accepted Debi-Check target");
            dtByDebiCheckSpecialist.Columns.Add("Actual Accepted Debi-Check %");
            dtByDebiCheckSpecialist.Columns.Add("Accepted Debi-Check %");
            dtByDebiCheckSpecialist.Columns.Add("Over / Under");
            dtByDebiCheckSpecialist.Columns.Add("Debi-Checks Rejected");
            dtByDebiCheckSpecialist.Columns.Add("Debi-Checks Rejected %");
            dtByDebiCheckSpecialist.Columns.Add("No Response / Other");
            dtByDebiCheckSpecialist.Columns.Add("No Response / Other %");
            dtByDebiCheckSpecialist.Columns.Add("Failed Transfer");
            dtByDebiCheckSpecialist.Columns.Add("Failed Transfer %");
            #endregion



            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

        private void HeaderPrefixAreaCheckbox_Unchecked(object sender, RoutedEventArgs e)
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

                IsAllRecordsChecked();
            }

            catch (Exception ex)
            {
            }
        }

        private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            //AllRecordsChecked = IsAllRecordsChecked();
        }
        private void LoadAgentInfo()
        {
            try
            {
                Cursor = Cursors.Wait;


                StringBuilder strQueryDCAgents = new StringBuilder();
                strQueryDCAgents.Append("SELECT [DCA].[FKUserID] as [ID], [DCA].[Description], ' ' as [FKStaffTypeID]  FROM lkpINCMAgentForwardedSale AS [DCA] left join [Blush].[dbo].[HRStaff] as [HS] on [DCA].[FKUserID] = [HS].[FKUserID]");

                DataTable dt = Methods.GetTableData(strQueryDCAgents.ToString());
                DataColumn column = new DataColumn("IsChecked", typeof(bool));
                
                column.DefaultValue = false;
                dt.Columns.Add(column);

                xdgAgents.DataSource = dt.DefaultView;
                
            }

            catch (Exception ex)
            {

            }

            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
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

                IsAllRecordsChecked();
            }

            catch (Exception ex)
            {
            }
        }
        private bool? IsAllRecordsChecked()
        {
            try
            {
                bool allSelected = (xdgAgents.Records.Select(r => (bool)((DataRecord)r).Cells["IsChecked"].Value)).All(b => b);
                bool noneSelected = (xdgAgents.Records.Select(r => (bool)((DataRecord)r).Cells["IsChecked"].Value)).All(b => !b);

                int countSelected = (xdgAgents.Records.Select(r => (bool)((DataRecord)r).Cells["IsChecked"].Value)).Count(b => b);
                lblAgents.Text = "Select Agent(s) " + "[" + countSelected + "]";

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
                return null;
            }
        }


        private void EnableDisableExportButton()
        {
            try
            {
                if ((calStartDate.SelectedDate != null && (calEndDate.SelectedDate != null))) //&& (calEndDate.SelectedDate >= Cal1.SelectedDate)
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
        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";

            btnReport.IsEnabled = true;
            btnClose.IsEnabled = true;
            calStartDate.IsEnabled = true;
            calEndDate.IsEnabled = true;
        }

        private bool IsAllInputParametersSpecifiedAndValid()
        {

            #region Ensuring that the From Date was specified

            if (calStartDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'From Date'.", @"No 'From Date' specified", ShowMessageType.Error);
                return false;
            }
            else
            {
                _startDate = calStartDate.SelectedDate.Value;
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the From Date was specified

            if (calEndDate.SelectedDate == null)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Please specify the 'To Date'.", @"No 'To Date' specified", ShowMessageType.Error);
                return false;
            }

            else
            {
                _endDate = calEndDate.SelectedDate.Value;
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the date range is valid

            if (calStartDate.SelectedDate > calEndDate.SelectedDate.Value)
            {
                ShowMessageBox(new INMessageBoxWindow1(), @"Invalid date range specified: The 'From Date' can not be greater than the 'To Date'.", "Invalid date range", ShowMessageType.Error);
                return false;
            }

            #endregion Ensuring that the date range is valid

            // Otherwise, if all is well, proceed:
            return true;
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                #region Job Title working
                switch (((User)GlobalSettings.ApplicationUser).FKUserType)
                {
                    case (int)lkpUserType.DebiCheckAgent:
                        PrintDCAgentReport();


                        //CheckUserVersion(Username);

                        break;


                    case (int)lkpUserType.ConfirmationAgent:
                    case (int)lkpUserType.StatusLoader:
                    case (int)lkpUserType.CallMonitoringAgent:
                    case (int)lkpUserType.Preserver:
                    case (int)lkpUserType.SeniorAdministrator:
                    case (int)lkpUserType.Administrator:
                    case (int)lkpUserType.Manager:
                    case (int)lkpUserType.SalesAgent:
                    case (int)lkpUserType.DataCapturer:


                        PrintManagerReport();

                        break;

                    default:
                        ShowMessageBox(new INMessageBoxWindow1(), "Invalid User Type for Insure System", "Login Failed", ShowMessageType.Error);
                        break;
                }
                #endregion


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

        private void PrintManagerReport()
        {

            TimeSpan ts11 = new TimeSpan(23, 00, 0);
            DateTime enddate = _endDate.Date + ts11;
            DataSet dsDiaryReportData;

            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
            };

            try { dtSummaryDataData.Clear(); } catch { }
            try { dtDataSheet.Clear(); } catch { }
            try { dtByDebiCheckSpecialist.Clear(); } catch { }

            using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                dsDiaryReportData = Business.Insure.INGetReportDCTransferSales(_fkINCampaignFKINCampaignClusterIDs, _startDate, enddate, TypeBool);
                dtSummaryDataData = dsDiaryReportData.Tables[2];
                dtDataSheet = dsDiaryReportData.Tables[0];
            }


            foreach (var item in selectedAgentString)
            {
                string agentID = item;
                using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    var dsDebiCheckSpecialistdata = Business.Insure.INGetReportDCTransferSales(agentID, _startDate, enddate, TypeBool);
                    DataTable dtSalesRow = dsDebiCheckSpecialistdata.Tables[1];


                    foreach (DataRow items in dtSalesRow.Rows)
                    {
                        dtByDebiCheckSpecialist.Rows.Add(items.ItemArray);
                    }
                }
            }

            int countForNonRedeemed = 0;

            try
            {
                string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                string filePathAndName = String.Format("TSRDebiCheckCallTransferStats" + DateTime.Now.ToString() + ".xlsx", UserFolder + " ", " Combined ", DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));

                if (dtSummaryDataData == null || dtSummaryDataData.Columns.Count == 0)
                    throw new Exception("ExportToExcel: Null or empty input table!\n");

                // load excel, and create a new workbook
                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Workbooks.Add();


                // single worksheet
                Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;
                workSheet.Name = "Summary Page";
                workSheet.Cells[1, 0 + 1] =  "Debi-Check Tracking Report";
                workSheet.Cells[1, 0 + 1].Font.Bold = true;
                workSheet.Cells[1, 0 + 1].Font.Size = 50;
                workSheet.Cells[1, 0 + 1].ColumnWidth = 60;
                for (var i = 0; i < dtSummaryDataData.Columns.Count; i++)
                {

                    workSheet.Cells[2, i + 1].Font.Bold = true;
                    if (i == 0)
                    {
                        workSheet.Cells[2, i + 1].ColumnWidth = 30;
                    }
                    else
                    {
                        workSheet.Cells[2, i + 1].ColumnWidth = 15;
                    }
                    //workSheet.get_Range("A4", "J1").Font.Bold = true;
                }


                // column headings
                for (var i = 0; i < dtSummaryDataData.Columns.Count; i++)
                {
                    workSheet.Cells[2, i + 1] = dtSummaryDataData.Columns[i].ColumnName;
                }

                // rows
                for (var i = 1; i < dtSummaryDataData.Rows.Count + 1; i++)
                {
                    // to do: format datetime values before printing
                    for (var j = 0; j < dtSummaryDataData.Columns.Count; j++)
                    {
                        workSheet.Cells[i + 2, j + 1] = dtSummaryDataData.Rows[i - 1][j];
                        workSheet.Cells[i + 2, j + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    }

                    countForNonRedeemed = countForNonRedeemed + 1;

                }

                int totalrows = dtSummaryDataData.Rows.Count + 3;

                int totalRowMinusOne = totalrows - 1;

                workSheet.Cells[totalrows, 1].Value = "Total :";
                workSheet.Cells[totalrows, 1].Font.Bold = true;

                workSheet.Cells[totalrows, 2].Formula = string.Format("=SUM(B1:B" + totalRowMinusOne.ToString() + ")"); //B
                workSheet.Cells[totalrows, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                workSheet.Cells[totalrows, 2].Font.Bold = true;
                workSheet.Cells[totalrows, 3].Formula = string.Format("=SUM(C1:C" + totalRowMinusOne.ToString() + ")"); //C
                workSheet.Cells[totalrows, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                workSheet.Cells[totalrows, 3].Font.Bold = true;
                workSheet.Cells[totalrows, 4].Formula = string.Format("=AVERAGE(D1:D" + totalRowMinusOne.ToString() + ")"); //D
                workSheet.Cells[totalrows, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                workSheet.Cells[totalrows, 4].Font.Bold = true;
                workSheet.Cells[totalrows, 5].Formula = string.Format("=AVERAGE(E1:E" + totalRowMinusOne.ToString() + ")"); //E
                workSheet.Cells[totalrows, 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                workSheet.Cells[totalrows, 5].Font.Bold = true;
                workSheet.Cells[totalrows, 6].Formula = string.Format("=AVERAGE(F1:F" + totalRowMinusOne.ToString() + ")"); //F
                workSheet.Cells[totalrows, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                workSheet.Cells[totalrows, 6].Font.Bold = true;
                workSheet.Cells[totalrows, 7].Formula = string.Format("=SUM(G1:G" + totalRowMinusOne.ToString() + ")"); //G
                workSheet.Cells[totalrows, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                workSheet.Cells[totalrows, 7].Font.Bold = true;
                workSheet.Cells[totalrows, 8].Formula = string.Format("=AVERAGE(H1:H" + totalRowMinusOne.ToString() + ")"); //H
                workSheet.Cells[totalrows, 8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                workSheet.Cells[totalrows, 8].Font.Bold = true;
                workSheet.Cells[totalrows, 9].Formula = string.Format("=SUM(I1:I" + totalRowMinusOne.ToString() + ")"); //I
                workSheet.Cells[totalrows, 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                workSheet.Cells[totalrows, 9].Font.Bold = true;
                workSheet.Cells[totalrows, 10].Formula = string.Format("=AVERAGE(J1:J" + totalRowMinusOne.ToString() + ")"); //J
                workSheet.Cells[totalrows, 10].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                workSheet.Cells[totalrows, 10].Font.Bold = true;
                workSheet.Cells[totalrows, 11].Formula = string.Format("=SUM(K1:K" + totalRowMinusOne.ToString() + ")"); //K
                workSheet.Cells[totalrows, 11].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                workSheet.Cells[totalrows, 11].Font.Bold = true;
                workSheet.Cells[totalrows, 12].Formula = string.Format("=AVERAGE(L1:L" + totalRowMinusOne.ToString() + ")"); //L
                workSheet.Cells[totalrows, 12].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                workSheet.Cells[totalrows, 12].Font.Bold = true;


                //workSheet.Range["A2", "B2"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
                //workSheet.Range["C2", "M2"].Interior.Color = System.Drawing.Color.LightBlue;

                #region Totals for Grid 1
                //workSheet.Cells[countForNonRedeemed + 3, 3].Formula = string.Format("=SUM(C3:C" + (countForNonRedeemed + 2).ToString() + ")"); //D
                //workSheet.Cells[countForNonRedeemed + 3, 4].Formula = string.Format("=SUM(D3:D" + (countForNonRedeemed + 2).ToString() + ")"); //E
                //workSheet.Cells[countForNonRedeemed + 3, 5].Formula = string.Format("=SUM(E3:E" + (countForNonRedeemed + 2).ToString() + ")"); //E
                //workSheet.Cells[countForNonRedeemed + 3, 6].Formula = string.Format("=SUM(F3:F" + (countForNonRedeemed + 2).ToString() + ")"); //F
                //workSheet.Cells[countForNonRedeemed + 3, 8].Formula = string.Format("=SUM(H3:H" + (countForNonRedeemed + 2).ToString() + ")"); //H
                //workSheet.Cells[countForNonRedeemed + 3, 10].Formula = string.Format("=SUM(J3:J" + (countForNonRedeemed + 2).ToString() + ")"); //J
                //workSheet.Cells[countForNonRedeemed + 3, 11].Formula = string.Format("=SUM(K3:K" + (countForNonRedeemed + 2).ToString() + ")"); //K
                //workSheet.Cells[countForNonRedeemed + 3, 12].Formula = string.Format("=SUM(L3:L" + (countForNonRedeemed + 2).ToString() + ")"); //L
                //workSheet.Cells[countForNonRedeemed + 3, 13].Formula = string.Format("=SUM(M3:M" + (countForNonRedeemed + 2).ToString() + ")"); //M
                //workSheet.Cells[countForNonRedeemed + 3, 7].Formula = string.Format("=F" + (countForNonRedeemed + 3).ToString() + "/E" + (countForNonRedeemed + 3).ToString()); //M
                //workSheet.Cells[countForNonRedeemed + 3, 9].Formula = string.Format("=H" + (countForNonRedeemed + 3).ToString() + "/E" + (countForNonRedeemed + 3).ToString()); //M

                #endregion

                workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 12]].Merge();
                workSheet.Cells[1, 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;


                (workSheet.Cells[1, 5]).EntireColumn.NumberFormat = "##%";
                (workSheet.Cells[1, 8]).EntireColumn.NumberFormat = "##%";
                (workSheet.Cells[1, 10]).EntireColumn.NumberFormat = "##%";
                (workSheet.Cells[1, 12]).EntireColumn.NumberFormat = "##%";

                (workSheet.Rows[2]).EntireRow.RowHeight = 40;
                workSheet.Rows[2].WrapText = true;

                Microsoft.Office.Interop.Excel.Range tRange = workSheet.UsedRange;
                tRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                tRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                AddDataSheet(excelApp);
                AddDebiCheckSpecialist(excelApp);

                //// check file path
                //foreach (var items in datesSelectedList)
                //{
                //    AddPages(excelApp, items);
                //}


                excelApp.Visible = true;
                excelApp.Workbooks.Item[1].SaveAs(filePathAndName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                //excelApp.Workbooks.

            }
            catch (Exception ex)
            {

            }

        }

        private void AddDataSheet(Microsoft.Office.Interop.Excel.Application excelApp)
        {
            //this is the All Data Sheet

            #region Get the report data


            #endregion Get the report data



            int countForNonRedeemed = 0;
            Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.Worksheets.Add();
            workSheet.Name = "Data Sheet";
            workSheet.Cells[1, 0 + 1] = "Debi-Check Specialist Tracking Report Data Sheet - Combined";
            workSheet.Cells[1, 0 + 1].Font.Bold = true;
            workSheet.Cells[1, 0 + 1].Font.Size = 50;
            workSheet.Cells[1, 0 + 1].ColumnWidth = 60;
            for (var i = 0; i < dtDataSheet.Columns.Count; i++)
            {

                workSheet.Cells[2, i + 1].Font.Bold = true;
                if (i == 0)
                {
                    workSheet.Cells[2, i + 1].ColumnWidth = 30;
                }
                else
                {
                    workSheet.Cells[2, i + 1].ColumnWidth = 30;
                }

                workSheet.Cells[2, i + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                //workSheet.get_Range("A4", "J1").Font.Bold = true;
            }


            // column headings
            for (var i = 0; i < dtDataSheet.Columns.Count; i++)
            {

                workSheet.Cells[2, i + 1] = dtDataSheet.Columns[i].ColumnName;
            }

            // rows
            for (var i = 1; i < dtDataSheet.Rows.Count + 1; i++)
            {
                // to do: format datetime values before printing
                for (var j = 0; j < dtDataSheet.Columns.Count; j++)
                {
                    workSheet.Cells[i + 2, j + 1] = dtDataSheet.Rows[i - 1][j];
                    workSheet.Cells[i + 2, j + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                }

                countForNonRedeemed = countForNonRedeemed + 1;
            }

            

            #region Totals for Grid 1
            //workSheet.Cells[countForNonRedeemed + 3, 3].Formula = string.Format("=SUM(C3:C" + (countForNonRedeemed + 2).ToString() + ")"); //D
            //workSheet.Cells[countForNonRedeemed + 3, 4].Formula = string.Format("=SUM(D3:D" + (countForNonRedeemed + 2).ToString() + ")"); //E
            //workSheet.Cells[countForNonRedeemed + 3, 5].Formula = string.Format("=SUM(E3:E" + (countForNonRedeemed + 2).ToString() + ")"); //E
            //workSheet.Cells[countForNonRedeemed + 3, 6].Formula = string.Format("=SUM(F3:F" + (countForNonRedeemed + 2).ToString() + ")"); //F

            workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 7]].Merge();
            workSheet.Cells[1, 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            #endregion

            countForNonRedeemed = countForNonRedeemed + 3;
            int CountSecondGridTotals = countForNonRedeemed;


            //workSheet.get_Range("A2", "C2").BorderAround(
            //Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
            //Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin,
            //Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

            //workSheet.get_Range("A24", "C24").BorderAround(
            //Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
            //Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin,
            //Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);


            Microsoft.Office.Interop.Excel.Range tRange = workSheet.UsedRange;
            tRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            tRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

            //workSheet.Range["A2", "B2"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
            //workSheet.Range["C2", "G2"].Interior.Color = System.Drawing.Color.LightBlue;


            //workSheet.Cells[30, 2].Formula = string.Format("=SUM(B1:B29)"); //B
            //workSheet.Cells[30, 3].Formula = string.Format("=SUM(C1:C29)"); //C
            //workSheet.Cells[30, 4].Formula = string.Format("=SUM(D1:D29)"); //D
            //workSheet.Cells[30, 5].Formula = string.Format("=SUM(E1:E29)"); //E
            //workSheet.Cells[30, 7].Formula = string.Format("=SUM(G1:G29)"); //G
            //workSheet.Cells[30, 9].Formula = string.Format("=SUM(I1:I29)"); //I
            //workSheet.Cells[30, 11].Formula = string.Format("=SUM(K1:K29)"); //K
            //workSheet.Cells[30, 13].Formula = string.Format("=SUM(M1:M29)"); //M
            //workSheet.Cells[30, 15].Formula = string.Format("=SUM(O1:O29)"); //O
            //workSheet.Cells[30, 17].Formula = string.Format("=SUM(Q1:Q29)"); //Q
            //workSheet.Cells[30, 19].Formula = string.Format("=SUM(S1:S29)"); //S
            //workSheet.Cells[30, 21].Formula = string.Format("=SUM(U1:U29)"); //U
            //workSheet.Cells[30, 23].Formula = string.Format("=SUM(W1:W29)"); //W
            //workSheet.Cells[30, 25].Formula = string.Format("=SUM(Y1:Y29)"); //y


            //(workSheet.Cells[1, 7]).EntireColumn.NumberFormat = "##%";
            //(workSheet.Rows[2]).EntireRow.RowHeight = 30;
            //workSheet.Rows[2].WrapText = true;

            //(workSheet.Cells[1, 8]).EntireColumn.NumberFormat = "##%";
            //(workSheet.Cells[1, 9]).EntireColumn.NumberFormat = "##%";
            //(workSheet.Cells[1, 12]).EntireColumn.NumberFormat = "##%";
            //(workSheet.Cells[1, 14]).EntireColumn.NumberFormat = "##%";
            //(workSheet.Cells[1, 10]).EntireColumn.NumberFormat = "#";

        }

        private void AddDataSheetDCAgent(Workbook excelApp)
        {
            //this is the All Data Sheet

            #region Get the report data

            #endregion Get the report data

            int countForNonRedeemed = 0;
            Worksheet workSheet = excelApp.Worksheets.Add("Sheet 2");
            //Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.Worksheets.Add();
            workSheet.Name = "Data Sheet";
            workSheet.Rows[0].Cells[0].Value = "Call Monitoring Tracking Report Data Sheet - Combined";
            workSheet.Rows[0].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            //workSheet.Rows[1].Cells[1].Font.Size = 50;
            //workSheet.Cells[1, 0 + 1].ColumnWidth = 60;
            for (var i = 0; i < dtDataSheet.Columns.Count; i++)
            {

                workSheet.Rows[2].Cells[i].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;

                if (i == 0)
                {
                    //workSheet.Rows[2].Cells[i + 1].ColumnWidth = 30;
                    workSheet.Columns[i].Width = 5500;
                }
                else
                {
                    workSheet.Columns[i].Width = 5500;

                    //workSheet.Cells[2, i + 1].ColumnWidth = 30;
                }

                //workSheet.Cells[2, i + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            }


            // column headings
            for (var i = 0; i < dtDataSheet.Columns.Count; i++)
            {

                workSheet.Rows[2].Cells[i].Value = dtDataSheet.Columns[i].ColumnName;
                workSheet.Rows[2].Cells[i].CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                workSheet.Rows[2].Cells[i].CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
                workSheet.Rows[2].Cells[i].CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                workSheet.Rows[2].Cells[i].CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
            }

            // rows
            for (var i = 1; i < dtDataSheet.Rows.Count + 1; i++)
            {
                // to do: format datetime values before printing
                for (var j = 0; j < dtDataSheet.Columns.Count; j++)
                {
                    workSheet.Rows[i + 2].Cells[j].Value = dtDataSheet.Rows[i - 1][j];
                    //workSheet.Rows[i + 2].Cells, j + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Rows[i + 2].Cells[j].CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                    workSheet.Rows[i + 2].Cells[j].CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                    workSheet.Rows[i + 2].Cells[j].CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                    workSheet.Rows[i + 2].Cells[j].CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
                }

                countForNonRedeemed = countForNonRedeemed + 1;
            }

            #region Totals for Grid 1


            //workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 7]].Merge();
            //workSheet.Cells[1, 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            #endregion

            countForNonRedeemed = countForNonRedeemed + 3;
            int CountSecondGridTotals = countForNonRedeemed;


            WorksheetMergedCellsRegion mergedRegion1 = workSheet.MergedCellsRegions.Add(0, 0, 0, 6);
            // Set the value of the merged region
            mergedRegion1.Value = "Call Monitoring Tracking Report Data Sheet - Combined";
            workSheet.Rows[0].Cells[2].CellFormat.Alignment = HorizontalCellAlignment.Center;



        }

        private void AddDebiCheckSpecialist(Microsoft.Office.Interop.Excel.Application excelApp)
        {
            //this is the All Data Sheet

            #region Get the report data


            #endregion Get the report data



            int countForNonRedeemed = 0;
            Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.Worksheets.Add();
            workSheet.Name = "By DebiCheck Specialist";
            workSheet.Cells[1, 0 + 1] = "Debi-Check Tracking Report";
            workSheet.Cells[1, 0 + 1].Font.Bold = true;
            workSheet.Cells[1, 0 + 1].Font.Size = 50;
            workSheet.Cells[1, 0 + 1].ColumnWidth = 20;
            for (var i = 0; i < dtByDebiCheckSpecialist.Columns.Count; i++)
            {

                workSheet.Cells[2, i + 1].Font.Bold = true;
                workSheet.Cells[2, i + 1].WrapText = true;

                if (i == 0)
                {
                    workSheet.Cells[2, i + 1].ColumnWidth = 20;
                    workSheet.Cells[2, i + 1].WrapText = true;
                }
                else
                {
                    workSheet.Cells[2, i + 1].ColumnWidth = 20;
                    workSheet.Cells[2, i + 1].WrapText = true;
                }

                workSheet.Cells[2, i + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                //workSheet.get_Range("A4", "J1").Font.Bold = true;
            }


            // column headings
            for (var i = 0; i < dtByDebiCheckSpecialist.Columns.Count; i++)
            {

                workSheet.Cells[2, i + 1] = dtByDebiCheckSpecialist.Columns[i].ColumnName;

            }

            // rows
            for (var i = 1; i < dtByDebiCheckSpecialist.Rows.Count + 1; i++)
            {
                // to do: format datetime values before printing
                for (var j = 0; j < dtByDebiCheckSpecialist.Columns.Count; j++)
                {
                    workSheet.Cells[i + 2, j + 1] = dtByDebiCheckSpecialist.Rows[i - 1][j];
                    workSheet.Cells[i + 2, j + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                }

                countForNonRedeemed = countForNonRedeemed + 1;
            }

            int totalrows = dtByDebiCheckSpecialist.Rows.Count + 3;

            int totalRowMinusOne = totalrows - 1;

            workSheet.Cells[totalrows, 1].Value = "Total :";
            workSheet.Cells[totalrows, 1].Font.Bold = true;

            workSheet.Cells[totalrows, 2].Formula = string.Format("=SUM(B1:B" + totalRowMinusOne.ToString() + ")"); //B
            workSheet.Cells[totalrows, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[totalrows, 2].Font.Bold = true;
            workSheet.Cells[totalrows, 3].Formula = string.Format("=SUM(C1:C" + totalRowMinusOne.ToString() + ")"); //C
            workSheet.Cells[totalrows, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[totalrows, 3].Font.Bold = true;
            workSheet.Cells[totalrows, 4].Formula = string.Format("=AVERAGE(D1:D" + totalRowMinusOne.ToString() + ")"); //D
            workSheet.Cells[totalrows, 4].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[totalrows, 4].Font.Bold = true;
            workSheet.Cells[totalrows, 5].Formula = string.Format("=AVERAGE(E1:E" + totalRowMinusOne.ToString() + ")"); //E
            workSheet.Cells[totalrows, 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[totalrows, 5].Font.Bold = true;
            workSheet.Cells[totalrows, 6].Formula = string.Format("=AVERAGE(F1:F" + totalRowMinusOne.ToString() + ")"); //F
            workSheet.Cells[totalrows, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[totalrows, 6].Font.Bold = true;
            workSheet.Cells[totalrows, 7].Formula = string.Format("=SUM(G1:G" + totalRowMinusOne.ToString() + ")"); //G
            workSheet.Cells[totalrows, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[totalrows, 7].Font.Bold = true;
            workSheet.Cells[totalrows, 8].Formula = string.Format("=AVERAGE(H1:H" + totalRowMinusOne.ToString() + ")"); //H
            workSheet.Cells[totalrows, 8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[totalrows, 8].Font.Bold = true;
            workSheet.Cells[totalrows, 9].Formula = string.Format("=SUM(I1:I" + totalRowMinusOne.ToString() + ")"); //I
            workSheet.Cells[totalrows, 9].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[totalrows, 9].Font.Bold = true;
            workSheet.Cells[totalrows, 10].Formula = string.Format("=AVERAGE(J1:J" + totalRowMinusOne.ToString() + ")"); //J
            workSheet.Cells[totalrows, 10].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[totalrows, 10].Font.Bold = true;
            workSheet.Cells[totalrows, 11].Formula = string.Format("=SUM(K1:K" + totalRowMinusOne.ToString() + ")"); //K
            workSheet.Cells[totalrows, 11].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[totalrows, 11].Font.Bold = true;
            workSheet.Cells[totalrows, 12].Formula = string.Format("=AVERAGE(L1:L" + totalRowMinusOne.ToString() + ")"); //L
            workSheet.Cells[totalrows, 12].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            workSheet.Cells[totalrows, 12].Font.Bold = true;


            #region Totals for Grid 1
            //workSheet.Cells[countForNonRedeemed + 3, 3].Formula = string.Format("=SUM(C3:C" + (countForNonRedeemed + 2).ToString() + ")"); //D
            //workSheet.Cells[countForNonRedeemed + 3, 4].Formula = string.Format("=SUM(D3:D" + (countForNonRedeemed + 2).ToString() + ")"); //E
            //workSheet.Cells[countForNonRedeemed + 3, 5].Formula = string.Format("=SUM(E3:E" + (countForNonRedeemed + 2).ToString() + ")"); //E
            //workSheet.Cells[countForNonRedeemed + 3, 6].Formula = string.Format("=SUM(F3:F" + (countForNonRedeemed + 2).ToString() + ")"); //F

            workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 12]].Merge();
            workSheet.Cells[1, 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            #endregion

            countForNonRedeemed = countForNonRedeemed + 3;
            int CountSecondGridTotals = countForNonRedeemed;


            //workSheet.get_Range("A2", "C2").BorderAround(
            //Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
            //Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin,
            //Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);

            //workSheet.get_Range("A24", "C24").BorderAround(
            //Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
            //Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin,
            //Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic, 1);


            Microsoft.Office.Interop.Excel.Range tRange = workSheet.UsedRange;
            tRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            tRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

            //workSheet.Range["A2", "B2"].Interior.Color = System.Drawing.Color.LightGoldenrodYellow;
            //workSheet.Range["C2", "G2"].Interior.Color = System.Drawing.Color.LightBlue;


            //(workSheet.Cells[1, 7]).EntireColumn.NumberFormat = "##%";
            (workSheet.Rows[2]).EntireRow.RowHeight = 30;
            workSheet.Rows[2].WrapText = true;

            (workSheet.Cells[1, 5]).EntireColumn.NumberFormat = "##%";
            (workSheet.Cells[1, 10]).EntireColumn.NumberFormat = "##%";
            (workSheet.Cells[1, 8]).EntireColumn.NumberFormat = "##%";
            (workSheet.Cells[1, 12]).EntireColumn.NumberFormat = "##%";
            //(workSheet.Cells[1, 14]).EntireColumn.NumberFormat = "##%";
            //(workSheet.Cells[1, 10]).EntireColumn.NumberFormat = "#";

        }

        private void AddDebiCheckSpecialistDCAgents(Workbook excelApp)
        {
            //this is the All Data Sheet

            #region Get the report data


            #endregion Get the report data



            int countForNonRedeemed = 0;
            Worksheet workSheet = excelApp.Worksheets.Add("Sheet 3");

            workSheet.Name = "By DebiCheck Specialist";
            workSheet.Rows[0].Cells[0].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
            //workSheet.Cells[1, 0 + 1].Font.Size = 50;
            //workSheet.Cells[1, 0 + 1].ColumnWidth = 60;
            for (var i = 0; i < dtByDebiCheckSpecialist.Columns.Count; i++)
            {

                workSheet.Rows[2].Cells[i].CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                if (i == 0)
                {
                    workSheet.Columns[i].Width = 5500;
                }
                else
                {
                    workSheet.Columns[i].Width = 5500;
                }

                //workSheet.Cells[2, i + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                //workSheet.get_Range("A4", "J1").Font.Bold = true;
            }


            // column headings
            for (var i = 0; i < dtByDebiCheckSpecialist.Columns.Count; i++)
            {

                workSheet.Rows[2].Cells[i].Value = dtByDebiCheckSpecialist.Columns[i].ColumnName;
                workSheet.Rows[2].Cells[i].CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                workSheet.Rows[2].Cells[i].CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
                workSheet.Rows[2].Cells[i].CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                workSheet.Rows[2].Cells[i].CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
            }

            // rows
            for (var i = 1; i < dtByDebiCheckSpecialist.Rows.Count + 1; i++)
            {
                // to do: format datetime values before printing
                for (var j = 0; j < dtByDebiCheckSpecialist.Columns.Count; j++)
                {
                    workSheet.Rows[i + 2].Cells[j].Value = dtByDebiCheckSpecialist.Rows[i - 1][j];
                    //workSheet.Cells[i + 2, j + 1].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    workSheet.Rows[i + 2].Cells[j].CellFormat.LeftBorderStyle = CellBorderLineStyle.Thin;
                    workSheet.Rows[i + 2].Cells[j].CellFormat.RightBorderStyle = CellBorderLineStyle.Thin;
                    workSheet.Rows[i + 2].Cells[j].CellFormat.TopBorderStyle = CellBorderLineStyle.Thin;
                    workSheet.Rows[i + 2].Cells[j].CellFormat.BottomBorderStyle = CellBorderLineStyle.Thin;
                }

                countForNonRedeemed = countForNonRedeemed + 1;
            }

            #region Totals for Grid 1


            #endregion

            countForNonRedeemed = countForNonRedeemed + 3;
            int CountSecondGridTotals = countForNonRedeemed;

            workSheet.Columns[4].CellFormat.FormatString = "0.00%";
            workSheet.Columns[9].CellFormat.FormatString = "0.00%";
            workSheet.Columns[7].CellFormat.FormatString = "0.00%";

            WorksheetMergedCellsRegion mergedRegion1 = workSheet.MergedCellsRegions.Add(0, 0, 0, 9);
            // Set the value of the merged region
            mergedRegion1.Value = "Debi-Check Tracking Report";
            workSheet.Rows[0].Cells[2].CellFormat.Alignment = HorizontalCellAlignment.Center;

        }
        private void PrintDCAgentReport()
        {
            try 
            {
                TimeSpan ts11 = new TimeSpan(23, 00, 0);
                DateTime enddate = _endDate.Date + ts11;
                DataSet dsDiaryReportData;

                var transactionOptions = new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                };

                try { dtSummaryDataData.Clear(); } catch { }
                try { dtDataSheet.Clear(); } catch { }
                try { dtByDebiCheckSpecialist.Clear(); } catch { }

                using (var tran = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    dsDiaryReportData = Business.Insure.INGetReportDCTransferSales(GlobalSettings.ApplicationUser.ID.ToString(), _startDate, enddate, TypeBool);
                    dtByDebiCheckSpecialist = dsDiaryReportData.Tables[1];
                    dtDataSheet = dsDiaryReportData.Tables[0];
                }



                string filePathAndName = GlobalSettings.UserFolder + "DC Agents Sales Report ( ) ~ " + DateTime.Now.Millisecond + ".xls";

                //if (dtSummaryDataData == null || dtSummaryDataData.Columns.Count == 0)
                //    throw new Exception("ExportToExcel: Null or empty input table!\n");

                //// load excel, and create a new workbook
                //var excelApp = new Microsoft.Office.Interop.Excel.Application();
                //excelApp.Workbooks.Add();

                //AddDataSheet(excelApp);
                //AddDebiCheckSpecialist(excelApp);

                //excelApp.Visible = true;
                //excelApp.Workbooks.Item[1].SaveAs(filePathAndName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                Workbook workbook1 = new Workbook();
                //Worksheet worksheet1 = workbook1.Worksheets.Add("Sheet 1");

                AddDataSheetDCAgent(workbook1);
                AddDebiCheckSpecialistDCAgents(workbook1);

                workbook1.Save(filePathAndName);
                Process.Start(filePathAndName);

            }
            catch 
            { 
            
            }



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
                string SelectedAgentsString = "";

                try
                {
                    try
                    {
                        _fkINCampaignFKINCampaignClusterIDs = "";
                    }
                    catch { }

                    _selectedAgents = (xdgAgents.Records.Select(r => (DataRecord)r).Where(r => (bool)r.Cells["IsChecked"].Value)).ToList();
                     _fkINCampaignFKINCampaignClusterIDs = _selectedAgents.Cast<DataRecord>().Where(record => (bool)record.Cells["IsChecked"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                    _fkINCampaignFKINCampaignClusterIDs = _fkINCampaignFKINCampaignClusterIDs.Substring(0, _fkINCampaignFKINCampaignClusterIDs.Length - 1);


                    try { selectedAgentString.Clear(); } catch { }

                    foreach (DataRecord item in _selectedAgents)
                    {
                        SelectedAgentsString = item.Cells["ID"].Value + "," + SelectedAgentsString;
                        selectedAgentString.Add(item.Cells["ID"].Value.ToString());
                    }
                }
                catch
                {

                }





                if (IsAllInputParametersSpecifiedAndValid())
                {
                    btnClose.IsEnabled = false;
                    btnReport.IsEnabled = false;
                    calStartDate.IsEnabled = false;
                    calEndDate.IsEnabled = false;

                    //Cal2.IsEnabled = false;

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
        private void Cal1_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calStartDate.SelectedDate.ToString(), out _startDate);
            EnableDisableExportButton();
        }

        private void Cal2_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calEndDate.SelectedDate.ToString(), out _endDate);
            EnableDisableExportButton();
        }






        #endregion

        private void UpgradeCB_Checked(object sender, RoutedEventArgs e)
        {
            TypeBool = "1";
            BaseCB.IsChecked = false;
        }

        private void BaseCB_Checked(object sender, RoutedEventArgs e)
        {
            TypeBool = "0";
            UpgradeCB.IsChecked = false;
        }
    }
}
