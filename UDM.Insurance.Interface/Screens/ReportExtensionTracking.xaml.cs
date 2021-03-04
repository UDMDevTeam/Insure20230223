using System.Data.SqlClient;
using System.Windows.Resources;
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
using Orientation = Infragistics.Documents.Excel.Orientation;


namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportExtensionTracking.xaml
    /// </summary>
    public partial class ReportExtensionTracking
    {
        
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;
        private long _campaignID;
        private string _CampaignName;
        private string _batchIDs = string.Empty;
        private DateTime _startDate = DateTime.Now;
        private DateTime _endDate = DateTime.Now;
       
        #region Constructors

        public ReportExtensionTracking()
        {
            InitializeComponent();
            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }
        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;
            btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
            btnReport.ToolTip = btnReport.Content;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }
       
        #endregion Constructors

        private void LoadCampaigns()
        {
            try
            {
                UDM.Insurance.Interface.Data.CommonControlData.PopulateExtentionCampaignComboBox(cmbCampaign);
            }
            catch (Exception)
            {

            }
        }
      
        private void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {
            var now = DateTime.Now;
            DateTime fromDate = new DateTime(now.Year, now.Month, 1);
            int lastDay = DateTime.DaysInMonth(now.Year, now.Month);
            DateTime toDate = new DateTime(now.Year, now.Month, lastDay);

            _startDate = fromDate;
            _endDate = toDate;
            LoadCampaigns();
        }

        private void cmbCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _campaignID = Convert.ToInt64(cmbCampaign.SelectedValue);
            //get batches for campaign
            try
            {
                if (lstBatches.Items.Count > 0)
                {
                    lstBatches.Items.Clear();
                }
                DataTable dt = Methods.GetTableData("Select ID,Code from INBatch where FKINCampaignID = " + _campaignID + " order by Code");
                if (dt.Rows.Count > 0)
                {
                    CheckBox chkAll = new CheckBox();
                    chkAll.Checked += chkAll_Checked;
                    chkAll.Unchecked += chkAll_Unchecked;
                    chkAll.Content = "Select All";
                    chkAll.Tag = -1;
                    lstBatches.Items.Add(chkAll);
                    foreach (DataRow row in dt.Rows)
                    {
                        CheckBox chkItem = new CheckBox();
                        chkItem.Checked += chkItem_Checked;
                        chkItem.Unchecked += chkItem_Unchecked;
                        chkItem.Tag = row["ID"];
                        chkItem.Content = row["Code"].ToString();
                        lstBatches.Items.Add(chkItem);
                    }
                }
                 
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        void chkItem_Unchecked(object sender, RoutedEventArgs e)
        {
            //check if there an item that is check
            btnReport.IsEnabled = false;
            foreach (CheckBox chkItem in lstBatches.Items)
            {
                if (chkItem.IsChecked == true)
                {
                    btnReport.IsEnabled = true;
                }
            }
        }

        void chkItem_Checked(object sender, RoutedEventArgs e)
        {
            //check if there an item that is check
            foreach (CheckBox chkItem in lstBatches.Items)
            {
                if (chkItem.IsChecked == true)
                {
                    btnReport.IsEnabled = true;
                }
            }
           
        }

        void chkAll_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (CheckBox chkItem in lstBatches.Items)
                {
                    chkItem.IsChecked = false;
                }
                btnReport.IsEnabled = false;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        void chkAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (CheckBox chkItem in lstBatches.Items)
                {
                    chkItem.IsChecked = true;
                }
                btnReport.IsEnabled = true;
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
            lstBatches.IsEnabled = true;
           
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                _batchIDs = string.Empty;
                btnClose.IsEnabled = false;
                btnReport.IsEnabled = false;
                lstBatches.IsEnabled = false;
                _CampaignName = cmbCampaign.Text;
                foreach (CheckBox chkItem in lstBatches.Items)
                {
                    if (chkItem.IsChecked == true)
                    {
                        long batchID = batchID = Convert.ToInt32(chkItem.Tag.ToString());
                        _batchIDs = _batchIDs + batchID + ",";
                    }

                }

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
        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);
                IEnumerable<DataRecord> campaigns = e.Argument as IEnumerable<DataRecord>;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                string fileName = string.Empty;
                    //setup excel documents
                  if (_batchIDs != string.Empty)
                    {
                        Workbook wbTemplate;

                        string filePathAndName = GlobalSettings.UserFolder + " Extension Tracking  Report ~ " + DateTime.Now.Millisecond + ".xlsx";
                        Uri uri = new Uri("/Templates/ReportTemplateExtensionTracking.xlsx", UriKind.Relative);
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
                        //report data
                        DataTable dtLeadAllocationData;
                        SqlParameter[] parameters = new SqlParameter[3];
                        parameters[0] = new SqlParameter("@BatchIDs", _batchIDs);
                        parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
                        parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

                        DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spINReportExtensionTracking", parameters);

                        if (dsLeadAllocationData.Tables.Count > 0)
                        {
                            dtLeadAllocationData = dsLeadAllocationData.Tables[0];

                            if (dtLeadAllocationData.Rows.Count == 0)
                            {
                                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                                {
                                    ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected Campaigns and specified Date range.", "No Data", ShowMessageType.Information);
                                });

                                return;
                            }
                        }
                        else
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected Campaigns and specified Date range.", "No Data", ShowMessageType.Information);
                            });

                            return;
                        }

                        Worksheet wsReport = wbReport.Worksheets.Add("Extension Tracking Report");
                        wsReport.PrintOptions.PaperSize = PaperSize.A4;
                        wsReport.PrintOptions.Orientation = Orientation.Portrait;

                        int rowIndex = 3;
                         Methods.CopyExcelRegion(wsTemplate, 0, 0, dtLeadAllocationData.Rows.Count + 1, 19, wsReport, 0, 0);
                         wsReport.MergedCellsRegions[0].Worksheet.Rows[0].SetCellValue(1, _CampaignName);
                      
                         wsReport.MergedCellsRegions[0].Worksheet.Rows[0].SetCellValue(13, _CampaignName.Substring(0,_CampaignName.Length - 9) + " Renewals");
                            foreach (DataRow rw in dtLeadAllocationData.Rows)
                            {
                                if (rw["Agent"].ToString().ToLower() == "totals")
                                {
                                    //format total cells
                                    wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("C" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("D" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("E" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("K" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("O" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("S" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                    wsReport.GetCell("T" + rowIndex.ToString()).CellFormat.Fill = CellFill.CreateSolidFill(System.Drawing.Color.LightGray);
                                  

                                    wsReport.GetCell("A" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("B" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("C" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("D" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("E" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("F" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("G" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("H" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("I" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("J" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("K" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("L" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("M" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("N" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("O" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("P" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("Q" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("R" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("S" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    wsReport.GetCell("T" + rowIndex.ToString()).CellFormat.Font.Bold = ExcelDefaultableBoolean.True;
                                    


                                }
                                wsReport.GetCell("A" + rowIndex.ToString()).Value = rw["Agent"];
                                wsReport.GetCell("B" + rowIndex.ToString()).Value = rw["#Leads"];
                                wsReport.GetCell("C" + rowIndex.ToString()).Value = rw["TotalContacts"];
                                wsReport.GetCell("D" + rowIndex.ToString()).Value = rw["%ContactToLead"];
                                wsReport.GetCell("E" + rowIndex.ToString()).Value = rw["YesToExtendedCover"];
                                wsReport.GetCell("F" + rowIndex.ToString()).Value = rw["%YesToLead"];
                                wsReport.GetCell("G" + rowIndex.ToString()).Value = rw["Declines"];
                                wsReport.GetCell("H" + rowIndex.ToString()).Value = rw["%DeclineToLead"];
                                wsReport.GetCell("I" + rowIndex.ToString()).Value = rw["Diaries"];
                                wsReport.GetCell("J" + rowIndex.ToString()).Value = rw["NoContacts"];
                                wsReport.GetCell("K" + rowIndex.ToString()).Value = rw["%NoContact"];
                               
                                
                                //renewals columns
                                wsReport.GetCell("M" + rowIndex.ToString()).Value = rw["TotalLeadsToRenewals"];
                                wsReport.GetCell("N" + rowIndex.ToString()).Value = rw["TotalLeadsAllocatedRenewals"];
                                wsReport.GetCell("O" + rowIndex.ToString()).Value = rw["TotalContactsRenewals"];
                                wsReport.GetCell("P" + rowIndex.ToString()).Value = rw["%ContactToLeadRenwals"];
                                wsReport.GetCell("Q" + rowIndex.ToString()).Value = rw["SalesRenewals"];

                                wsReport.GetCell("R" + rowIndex.ToString()).Value = rw["SalesToLeadRenewals%"];
                                wsReport.GetCell("S" + rowIndex.ToString()).Value = rw["DeclinesRenewals"];
                                wsReport.GetCell("T" + rowIndex.ToString()).Value = rw["DeclineToLeadRenewals%"];

                                rowIndex++;
                            }
                        
                       
                        string fName = GlobalSettings.UserFolder + " Extension Tracking Report ~ " + DateTime.Now.Millisecond + ".xlsx";
                        wsReport.PrintOptions.ScalingType = ScalingType.FitToPages;
                        wbReport.Save(fName);
                        //Display excel document                           
                        Process.Start(fName);

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

        private void Cal1_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calStartDate.SelectedDate.ToString(), out _startDate);
            
        }

        private void Cal2_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calEndDate.SelectedDate.ToString(), out _endDate);
            
        }

    }
}
