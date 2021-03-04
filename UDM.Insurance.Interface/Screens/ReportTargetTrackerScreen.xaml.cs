using System.Data.SqlClient;
using System.Windows.Resources;
using Embriant.Framework;
using Embriant.Framework.Configuration;
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
using System.Globalization;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportTargetTrackerScreen.xaml
    /// </summary>
    public partial class ReportTargetTrackerScreen : INotifyPropertyChanged
    {
         #region INotifyPropertyChanged implementation
        BackgroundWorker worker = new BackgroundWorker();
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion



        #region Properties

        private bool? _allRecordsChecked = false;
        public bool? AllRecordsChecked
        {
            get
            {
                return _allRecordsChecked;
            }
            set
            {
                _allRecordsChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllRecordsChecked"));
            }
        }

        private bool? _IsReportRunning = false;
        public bool? IsReportRunning
        {
            get
            {
                return _IsReportRunning;
            }
            set
            {
                _IsReportRunning = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReportRunning"));
            }
        }

       
        #endregion



  



        #region Private Members

        //private DataRowView _selectedCampaign;
        private List<DataRecord> _selectedAgents;
        private DateTime _fromDate = DateTime.Now;
        private DateTime _toDate = DateTime.Now;
        private readonly DispatcherTimer _reportTimer = new DispatcherTimer();
        private int _seconds;
        #endregion



        #region Constructors

        public ReportTargetTrackerScreen()
        {
            InitializeComponent();
            LoadAgentInfo();

            _reportTimer.Tick += ReportTimer;
            _reportTimer.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion



        #region Private Methods

        private void LoadAgentInfo()
        {
            try
            {
                Cursor = Cursors.Wait;

                if (grdAgents.IsEnabled)
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter("@SupervisorID", GlobalSettings.ApplicationUser.ID);
                    DataSet ds = Methods.ExecuteStoredProcedure("spGetSupervisorAgent", parameters);

                    DataTable dt = ds.Tables[0];
                    DataColumn column = new DataColumn("IsChecked", typeof(bool));
                    column.DefaultValue = false;
                    dt.Columns.Add(column);

                    xdgAgents.DataSource = dt.DefaultView;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                Cursor = Cursors.Arrow;
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
                HandleException(ex);
                return null;
            }
        }

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _reportTimer.Stop();
            _seconds = 0;
            IsReportRunning = false;
            btnReport.Content = "Report";
            lblCurrentAgent.Text = String.Empty;
            btnCancel.IsEnabled = false;
            ProgressBar1.Value = 0;
            lblProgress.Text = string.Empty;
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                Excel.Application xlApp;

                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);

                DataTable dtLeadAllocationData;
                //DataTable dtProductivity;
                DataTable dtTargets;
                string AgentIDs = string.Empty;
                bool first = true;
                if (_selectedAgents != null)
                {
                    foreach (DataRecord agent in _selectedAgents)
                    {
                        if (first == true)
                        {
                            first = false;
                            AgentIDs = Convert.ToInt32(agent.Cells["ID"].Value).ToString()+",";
                        }
                        else
                        {
                            AgentIDs = AgentIDs + Convert.ToInt32(agent.Cells["ID"].Value).ToString() + ",";
                        }
                    }
                }

                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@AgentIDs", AgentIDs);
                parameters[1] = new SqlParameter("@FromDate", _fromDate);
                parameters[2] = new SqlParameter("@ToDate", _toDate);

                DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spINReportTargetTracker", parameters,false);
                if (dsLeadAllocationData.Tables.Count > 0)
                {
                    dtLeadAllocationData = dsLeadAllocationData.Tables[0];
                   // dtProductivity = dsLeadAllocationData.Tables[1];
                    dtTargets = dsLeadAllocationData.Tables[1];
                    if (dtLeadAllocationData.Rows.Count == 0)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected Agent(s)", "No Data", ShowMessageType.Information);
                        });

                        return;
                    }
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected Agent(s) ", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                #region report Data
                int agentIndex = 1;
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Name = "Tracker";

                #region Headings
                System.Globalization.DateTimeFormatInfo dtf = new DateTimeFormatInfo();
                xlWorkSheet.Cells[3, 2] = "Agents - "+ dtf.GetMonthName(_fromDate.Month);
                Excel.Range c1 = xlWorkSheet.Cells[3, 2];
                Excel.Range c2 = xlWorkSheet.Cells[3, 3];
                Excel.Range range = (Excel.Range)xlWorkSheet.get_Range(c1, c2);
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.Font.Bold = true;
                range.Borders.Weight = Excel.XlBorderWeight.xlMedium;
               // range.Interior.ColorIndex = 34;
                range.Merge(true);

                //Dates (get an array of dates between from and to date)
                int numberOFDaysBetween = int.Parse(_toDate.Subtract(_fromDate).TotalDays.ToString());
                List<DateTime> dates = new List<DateTime>();                
                for (int i = 0; i <= numberOFDaysBetween; i++)
                {
                    DateTime newDate = _fromDate.AddDays(i);
                    dates.Add(newDate);
                }

                int DateColIndex = 4;
                
                foreach (DateTime date in dates)
                {
                    if (date.DayOfWeek != DayOfWeek.Sunday)
                    {
                        xlWorkSheet.Cells[3, DateColIndex] = dtf.GetDayName(date.DayOfWeek);
                        xlWorkSheet.Cells[4, DateColIndex] = date.Day + "-" + dtf.GetAbbreviatedMonthName(date.Month);
                        DateColIndex++;
                    }
                        if (date.DayOfWeek == DayOfWeek.Saturday)
                        {
                            xlWorkSheet.Columns[DateColIndex - 1].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlThick;
                            xlWorkSheet.Columns[DateColIndex - 1].Borders[Excel.XlBordersIndex.xlEdgeRight].ColorIndex = 1;
                        }
                  
                }
                DateColIndex++;
               
                //targets
                for (int i = 1; i <= 4; i++)
                {
                    Excel.Range mtdTarg1 = xlWorkSheet.Cells[3, DateColIndex];
                    Excel.Range mtdTarg2 = xlWorkSheet.Cells[4, DateColIndex];
                    Excel.Range mtdTargRange = (Excel.Range)xlWorkSheet.get_Range(mtdTarg1, mtdTarg2);
                    mtdTargRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    mtdTargRange.Font.Bold = true;
                    mtdTargRange.Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    mtdTargRange.Interior.ColorIndex = 34;
                    mtdTargRange.Merge(true);
                    if (i == 1)
                    {
                        xlWorkSheet.Cells[3, DateColIndex] = "MTD TARGET";
                    }
                    if (i == 2)
                    {
                        xlWorkSheet.Cells[3, DateColIndex] = "MTD ACTUAL";
                    }
                    if (i == 3)
                    {
                        xlWorkSheet.Cells[3, DateColIndex] = "OVER/UNDER";
                    }
                    if (i == 4)
                    {
                        xlWorkSheet.Cells[3, DateColIndex] = "% MTD BONUS";
                        mtdTargRange.Interior.ColorIndex = 37;
                    }
                    xlWorkSheet.Cells[3, DateColIndex].ColumnWidth = 20;
                    DateColIndex++;
                }
                
                for (int i = 4; i <= dates.Count-1; i++)//formatting of all columns
                {
                    xlWorkSheet.Columns[i].ColumnWidth = 20;
                    

                    xlWorkSheet.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Columns[i].WrapText = true;
                    xlWorkSheet.Cells[3, i].Font.Bold = true;
                    xlWorkSheet.Cells[3, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[4, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                xlWorkSheet.Columns[2].ColumnWidth = 30; xlWorkSheet.Columns[3].ColumnWidth = 30;
                xlWorkSheet.Cells[4, 2] = "Team Member"; xlWorkSheet.Cells[4, 2].Font.Bold = true;
                xlWorkSheet.Cells[4, 3] = "Campaign"; xlWorkSheet.Cells[4, 3].Font.Bold = true;
                xlWorkSheet.Cells[3, 1].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                xlWorkSheet.Cells[4, 1].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                xlWorkSheet.Cells[4, 2].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                xlWorkSheet.Cells[4, 3].Borders.Weight = Excel.XlBorderWeight.xlMedium;

                #endregion Headings

                #region Populate Worksheet
                List<long> userIDs = new List<long>();
                int rowIndex = 5;
                
                int rowCount = int.Parse(e.Argument.ToString());
                int currentRow = 0;
                foreach (DataRow row in dtLeadAllocationData.Rows)
                {
                    
                    if (userIDs.Contains(long.Parse(row["FKUserID"].ToString())))
                    {

                    }
                    else
                    {
                        currentRow++;
                        int progress = currentRow;
                        (sender as BackgroundWorker).ReportProgress(currentRow, null);
                        long userID = long.Parse(row["FKUserID"].ToString());
                        userIDs.Add(userID);
                        xlWorkSheet.Cells[rowIndex, 1] = agentIndex; agentIndex++;                        
                        xlWorkSheet.Cells[rowIndex, 2] = row["TeamMember"].ToString();
                       

                        //get agent campaigns 
                        var dtCampaigns = dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["FKUserID"].ToString()) == userID);
                        List<string> addedCampaigns = new List<string>();
                        foreach(DataRow campaign in dtCampaigns)
                        {
                            if(addedCampaigns.Contains(campaign["CampaignName"].ToString()))
                            {
                            }
                            else
                            {
                                int colIndex = 4;
                                addedCampaigns.Add(campaign["CampaignName"].ToString());
                                xlWorkSheet.Cells[rowIndex, 3] = campaign["CampaignName"].ToString();
                                long? campaignID = long.Parse(campaign["CampaignID"].ToString());
                                double MtdAcual = 0;
                                int totalSaleCount = 0;
                                int totalOption2Count = 0;
                                int totalOption1Count = 0;
                                foreach (DateTime date in dates)
                                {

                               if (date.DayOfWeek != DayOfWeek.Sunday)
                                  {
                                    //get total premium for currentDay
                                    var totPremiumCurrDay = dtLeadAllocationData.AsEnumerable().Where(x => DateTime.Parse(x["DateOfSale"].ToString()).ToShortDateString() == date.ToShortDateString() && x["FKUserID"].ToString() == userID.ToString() && x["CampaignName"].ToString() == campaign["CampaignName"].ToString());
                                    double totPremium = 0;
                                    double totalUnits = 0;
                                    if (totPremiumCurrDay.Count() > 0)
                                    {
                                        int saleCount = 0;
                                        int option2Count = 0;
                                        int option1Count = 0;
                                        foreach (DataRow rw in totPremiumCurrDay)
                                        {
                                             totPremium = totPremium + double.Parse(rw["TotalPremium"].ToString());
                                             string units = rw["Units"].ToString();
                                             if (units != string.Empty)
                                             {
                                                 totalUnits = totalUnits + double.Parse(rw["Units"].ToString());
                                             }

                                            saleCount++;
                                            if (rw["RefNo"].ToString().Substring(0,4) == "gdnm")
                                            {
                                                option2Count++;
                                            }
                                            else if(rw["RefNo"].ToString().Substring(0,4) == "gdna")
                                            {
                                                option1Count++;
                                            }
                                        }
                                        if (campaign["IsUpgradeLead"].ToString() == "1")
                                        {
                                            xlWorkSheet.Cells[rowIndex, colIndex] = totalUnits;
                                        }
                                        else
                                        {
                                            xlWorkSheet.Cells[rowIndex, colIndex] = "R " + totPremium;
                                        }
                                        MtdAcual = MtdAcual + totPremium;
                                        if (campaign["CampaignName"].ToString() == "Accidental Disability")
                                        {
                                            xlWorkSheet.Cells[rowIndex + 3, colIndex] = saleCount;
                                            xlWorkSheet.Cells[rowIndex + 2, colIndex] = option2Count;
                                            xlWorkSheet.Cells[rowIndex + 1, colIndex] = option1Count;

                                            totalOption1Count = totalOption1Count + option1Count;
                                            totalOption2Count = totalOption2Count + option2Count;
                                        }
                                        else
                                        {
                                            xlWorkSheet.Cells[rowIndex + 1, colIndex] = saleCount;
                                        }
                                        totalSaleCount = totalSaleCount + saleCount;
                                    }
                                    colIndex++;
                                   }
                                }
                                if (dtTargets.Rows.Count > 0)
                                {
                                    //calcualte MTD Target
                                    DataTable mtdTarg = Methods.GetTableData("select sum(PremiumTarget) as MTDTarget from TSRTargets where FKAgentID = " + userID + " AND DateFrom >= '"+_fromDate + "' AND DateTo <= '"+ _toDate + "' AND FKINCampaignID = "+campaignID );
                                    if (mtdTarg.Rows.Count > 0)
                                    {
                                        string targetVal = mtdTarg.Rows[0]["MTDTarget"].ToString();
                                        if (targetVal != string.Empty)
                                        {
                                            xlWorkSheet.Cells[rowIndex, DateColIndex - 4] = "R " + targetVal;
                                            //over under calculation
                                            double mtdTarget = double.Parse(targetVal);
                                            double overUnder = MtdAcual-mtdTarget;
                                            xlWorkSheet.Cells[rowIndex, DateColIndex - 2] = "R " + overUnder;
                                            if (overUnder.ToString().Contains("-"))
                                            {
                                                xlWorkSheet.Cells[rowIndex, DateColIndex - 2].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            }
                                            //%mtd bonus
                                            xlWorkSheet.Cells[rowIndex, DateColIndex - 1] = Math.Round(((MtdAcual / mtdTarget) * 100),2) +"%";
                                            xlWorkSheet.Cells[rowIndex, DateColIndex - 1].Interior.ColorIndex = 43;
                                        }
                                    }
                                }
                                xlWorkSheet.Cells[rowIndex, DateColIndex - 3] = "R " + MtdAcual;
                                if (campaign["CampaignName"].ToString() == "Accidental Disability")
                                {
                                    xlWorkSheet.Cells[rowIndex + 3, DateColIndex - 3] = totalSaleCount;
                                    xlWorkSheet.Cells[rowIndex + 2, DateColIndex - 3] = totalOption2Count;
                                    xlWorkSheet.Cells[rowIndex + 1, DateColIndex - 3] = totalOption1Count;
                                }
                                else
                                {
                                    xlWorkSheet.Cells[rowIndex + 1, DateColIndex - 3] = totalSaleCount;
                                }
                               
                                for (int i = DateColIndex - 4; i <= DateColIndex - 1; i++)//format bonus columns
                                {
                                    xlWorkSheet.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                    xlWorkSheet.Columns[i].WrapText = true;
                                    xlWorkSheet.Cells[rowIndex, i].Font.Bold = true;
                                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlThin;

                                    xlWorkSheet.Cells[rowIndex +1, i].Font.Bold = true;
                                    xlWorkSheet.Cells[rowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                    xlWorkSheet.Cells[rowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlThin;

                                    xlWorkSheet.Cells[rowIndex + 2, i].Font.Bold = true;
                                    xlWorkSheet.Cells[rowIndex + 2, i].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                    xlWorkSheet.Cells[rowIndex + 2, i].Borders.Weight = Excel.XlBorderWeight.xlThin;

                                    xlWorkSheet.Cells[rowIndex + 3, i].Font.Bold = true;
                                    xlWorkSheet.Cells[rowIndex + 3, i].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                    xlWorkSheet.Cells[rowIndex + 3, i].Borders.Weight = Excel.XlBorderWeight.xlThin;

                                    xlWorkSheet.Cells[rowIndex + 4, i].Font.Bold = true;
                                    xlWorkSheet.Cells[rowIndex + 4, i].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                    xlWorkSheet.Cells[rowIndex + 4, i].Borders.Weight = Excel.XlBorderWeight.xlThin;

                                    xlWorkSheet.Cells[rowIndex + 5, i].Font.Bold = true;
                                    xlWorkSheet.Cells[rowIndex + 5, i].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                    xlWorkSheet.Cells[rowIndex + 5, i].Borders.Weight = Excel.XlBorderWeight.xlThin;
                                }


                                rowIndex++;
                                if (campaign["CampaignName"].ToString() == "Accidental Disability")
                                {
                                    xlWorkSheet.Cells[rowIndex, 3] = "Opt 1";
                                    rowIndex++;
                                    xlWorkSheet.Cells[rowIndex, 3] = "Opt 2";
                                    rowIndex++;
                                    xlWorkSheet.Cells[rowIndex, 3] = "Total Base";
                                }
                                else
                                {
                                    xlWorkSheet.Cells[rowIndex, 3] = "Base";
                                }
                                rowIndex++;



                                rowIndex++;
                            }
                        }
                        #region Productivity
                        //xlWorkSheet.Cells[rowIndex, 3] = "Productivity";
                        //if (dtProductivity.Rows.Count > 0)
                        //{

                        //    int prouctivityColIndex = 6;
                        //    DataRow totalSecondsForColumn = dtProductivity.AsEnumerable().Where(x => (string)x[1] == row["TeamMember"].ToString()).FirstOrDefault();


                        //    if(dtProductivity.AsEnumerable().Where(x => (string)x[1] == row["TeamMember"].ToString()).ToList().Count > 0)
                        //    {
                        //       int i = 4;
                        //       foreach (DateTime productDate in dates)
                        //       {
                        //          if (productDate.DayOfWeek != DayOfWeek.Sunday)
                        //          {
                        //           if (i >= 4)
                        //           {
                        //               string columName = "DayShiftTotalSecondsWorkedFor" + productDate.Year+productDate.Month.ToString().PadLeft(2,'0') +productDate.Day.ToString().PadLeft(2,'0');
                        //               if (totalSecondsForColumn.Table.Columns.Contains(columName))
                        //               {
                        //                   string totalSecondsWorked = totalSecondsForColumn[columName].ToString();
                        //                   string totalDIals = totalSecondsForColumn[prouctivityColIndex - 2].ToString();
                        //                   string productivity = string.Empty;
                        //                   if (totalSecondsWorked != string.Empty && totalDIals != string.Empty)
                        //                   {
                        //                       decimal productIvit = (decimal.Parse(totalDIals) / decimal.Parse(totalSecondsWorked)) * 100;

                        //                       productivity = productIvit.ToString() + "%";
                        //                   }
                        //                   xlWorkSheet.Cells[rowIndex, i] = productivity;
                        //                   prouctivityColIndex = prouctivityColIndex + 5;
                        //               }
                                       

                        //           }
                        //           i++;
                        //         }
                        //       }
                        //}

                        //}
                        #endregion Productivity
                        for (int i = 1; i <= dates.Count+4; i++)//productivity cell formatting
                        {
                            xlWorkSheet.Cells[rowIndex, i].Font.Bold = true;
                            xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 15;
                        }
                            rowIndex++;
                    }
                }
                #endregion Populate Worksheet

                #region Carried Forwards
                DataTable dtCarriedForwards = dsLeadAllocationData.Tables[2];
                if (dtCarriedForwards.Rows.Count > 0)
                {
                    Excel.Worksheet xlWorkSheetCF;
                    xlWorkSheetCF = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2);
                    xlWorkSheetCF.Name = "List of CF";

                    xlWorkSheetCF.Cells[1, 1] = "List of carried forwards " + _fromDate.ToShortDateString() + " to " + _toDate.ToShortDateString();
                    int CFrowIndex = 2;

                        xlWorkSheetCF.Cells[CFrowIndex, 1] = "TSR"; xlWorkSheetCF.Cells[CFrowIndex, 1].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                        xlWorkSheetCF.Cells[CFrowIndex, 2] = "RefNo"; xlWorkSheetCF.Cells[CFrowIndex, 2].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                        xlWorkSheetCF.Cells[CFrowIndex, 3] = "Campaign"; xlWorkSheetCF.Cells[CFrowIndex, 3].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                        xlWorkSheetCF.Cells[CFrowIndex, 4] = "Original Date Of Sale"; xlWorkSheetCF.Cells[CFrowIndex, 4].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                        xlWorkSheetCF.Cells[CFrowIndex, 5] = "New Date Of Sale"; xlWorkSheetCF.Cells[CFrowIndex, 5].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                        CFrowIndex++;

                        xlWorkSheetCF.Columns[1].ColumnWidth = 30; xlWorkSheetCF.Columns[2].ColumnWidth = 30;
                        xlWorkSheetCF.Columns[3].ColumnWidth = 30; xlWorkSheetCF.Columns[4].ColumnWidth = 30;
                        xlWorkSheetCF.Columns[5].ColumnWidth = 30;
                    foreach (DataRow rwCF in dtCarriedForwards.Rows)
                    {
                        xlWorkSheetCF.Cells[CFrowIndex, 1] = rwCF["TSR"]; xlWorkSheetCF.Cells[CFrowIndex, 1].Borders.Weight = Excel.XlBorderWeight.xlThin;
                        xlWorkSheetCF.Cells[CFrowIndex, 2] = rwCF["RefNo"]; xlWorkSheetCF.Cells[CFrowIndex, 2].Borders.Weight = Excel.XlBorderWeight.xlThin;
                        xlWorkSheetCF.Cells[CFrowIndex, 3] = rwCF["Campaign"]; xlWorkSheetCF.Cells[CFrowIndex, 3].Borders.Weight = Excel.XlBorderWeight.xlThin;
                        xlWorkSheetCF.Cells[CFrowIndex, 4] = rwCF["OriginalDos"]; xlWorkSheetCF.Cells[CFrowIndex, 4].Borders.Weight = Excel.XlBorderWeight.xlThin;
                        xlWorkSheetCF.Cells[CFrowIndex, 5] = rwCF["NewDos"]; xlWorkSheetCF.Cells[CFrowIndex, 5].Borders.Weight = Excel.XlBorderWeight.xlThin;

                       
                        CFrowIndex++;
                    }
                }
                #endregion Carried Forwards

                string filePathAndName = GlobalSettings.UserFolder + " Target Tracker  Report ~ " + Guid.NewGuid() + ".xls";
                xlWorkBook.SaveAs(filePathAndName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                xlWorkBook.Close(true, misValue, misValue);

                xlApp.Quit();

                //Display excel document
                Process.Start(filePathAndName);
                #endregion report Data


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

        private void ReportTimer(object sender, EventArgs e)
        {
            _seconds++;
            btnReport.Content = TimeSpan.FromSeconds(_seconds).ToString();
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
                IsReportRunning = true;
                _selectedAgents = (xdgAgents.Records.Select(r => (DataRecord)r).Where(r => (bool)r.Cells["IsChecked"].Value)).ToList();

                worker = new BackgroundWorker();
                worker.DoWork += Report;
                worker.RunWorkerCompleted += ReportCompleted;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;
                ProgressBar1.Maximum = _selectedAgents.Count;
                btnCancel.IsEnabled = true;
                lblProgress.Text = "Fetching Data.....";
                worker.RunWorkerAsync(_selectedAgents.Count);

                _reportTimer.Start();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblProgress.Text = "Compiling Worksheet " + e.ProgressPercentage + " of " + _selectedAgents.Count;
            ProgressBar1.Value = e.ProgressPercentage;
        }

        private void Cal1_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(Cal1.SelectedDate.ToString(), out _fromDate);
        }

        private void Cal2_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(Cal2.SelectedDate.ToString(), out _toDate);
        }

        private void RecordSelectorCheckbox_Click(object sender, RoutedEventArgs e)
        {
            AllRecordsChecked = IsAllRecordsChecked();
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
                HandleException(ex);
            }
        }

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
                HandleException(ex);
            }
        }

        #endregion

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
          
           ReportTargetTrackerScreen targetTracker = new ReportTargetTrackerScreen();
           ShowDialog(targetTracker, new INDialogWindow(targetTracker));

           OnDialogClose(_dialogResult);
        }



    

      
    }
}
