using System.Data.SqlClient;
//using System.Windows.Resources;
using Embriant.Framework;
using Embriant.Framework.Configuration;
//using Infragistics.Windows.DataPresenter;
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
//using System.Globalization;
//using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;

namespace UDM.Insurance.Interface.Screens
{

    public partial class ReportLeadsAvailableScreen
    {
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;
        public ReportLeadsAvailableScreen()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //load campaign types and campaign groups
                DataSet dsLookupLeadsAvailable = Methods.ExecuteStoredProcedure("spLookupLeadsAvailable", null);
                DataTable dtCampaignTypes = dsLookupLeadsAvailable.Tables[0];
                DataTable dtCampaignGroups = dsLookupLeadsAvailable.Tables[1];

                foreach (DataRow rw in dtCampaignTypes.Rows)
                {
                    CheckBox chkCampaignTypeItem = new CheckBox();

                    chkCampaignTypeItem.Checked += chkCampaignTypeItem_Checked;
                    chkCampaignTypeItem.Unchecked += chkCampaignTypeItem_Unchecked;
                    chkCampaignTypeItem.Content = rw["Description"].ToString();
                    chkCampaignTypeItem.Tag = rw["ID"].ToString();

                    lstCampaignTypes.Items.Add(chkCampaignTypeItem);
                }

                foreach (DataRow rw in dtCampaignGroups.Rows)
                {
                    CheckBox chkCampaignGroupItem = new CheckBox();

                    chkCampaignGroupItem.Content = rw["Description"].ToString();
                    chkCampaignGroupItem.Tag = rw["ID"].ToString();
                    chkCampaignGroupItem.Checked += chkCampaignGroupItem_Checked;
                    chkCampaignGroupItem.Unchecked += chkCampaignGroupItem_Unchecked;

                    lstCampaignGroups.Items.Add(chkCampaignGroupItem);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        void chkCampaignGroupItem_Unchecked(object sender, RoutedEventArgs e)
        {
            EnableDisableReportButton();
        }

        void chkCampaignTypeItem_Unchecked(object sender, RoutedEventArgs e)
        {
            EnableDisableReportButton();
        }

        void chkCampaignGroupItem_Checked(object sender, RoutedEventArgs e)
        {
            EnableDisableReportButton();
        }

        void chkCampaignTypeItem_Checked(object sender, RoutedEventArgs e)
        {
            EnableDisableReportButton();
        }
        private void EnableDisableReportButton()
        {
            //check if there is any campaign type item in list
            bool campaignTypeChecked = false;
            bool campaignGroupChecked = false;
            foreach (CheckBox chk in lstCampaignTypes.Items)
            {
                if (chk.IsChecked == true)
                {
                    campaignTypeChecked = true;
                }
            }

            foreach (CheckBox chk in lstCampaignGroups.Items)
            {
                if (chk.IsChecked == true)
                {
                    campaignGroupChecked = true;
                }
            }
            if (campaignTypeChecked == true && campaignGroupChecked == true)
            {
                btnReport.IsEnabled = true;
            }
            else
            {
                btnReport.IsEnabled = false;
            }
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                List<long> campaignTypes = new List<long>();
                List<long> campaignGroups = new List<long>();
                foreach (CheckBox chkItem in lstCampaignTypes.Items)
                {
                    if (chkItem.IsChecked == true)
                    {
                        campaignTypes.Add(long.Parse(chkItem.Tag.ToString()));
                    }
                }
                foreach (CheckBox chkitem in lstCampaignGroups.Items)
                {
                    if (chkitem.IsChecked == true)
                    {
                        campaignGroups.Add(long.Parse(chkitem.Tag.ToString()));
                    }
                }
                List<List<long>> campaignGroupTypes = new List<List<long>>();
                campaignGroupTypes.Add(campaignTypes);
                campaignGroupTypes.Add(campaignGroups);

                btnClose.IsEnabled = false;
                btnReport.IsEnabled = false;

                lstCampaignGroups.IsEnabled = false;
                lstCampaignTypes.IsEnabled = false;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += Report;
                worker.RunWorkerCompleted += ReportCompleted;
                worker.RunWorkerAsync(campaignGroupTypes);

                dispatcherTimer1.Start();
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
            lstCampaignTypes.IsEnabled = true;
            lstCampaignGroups.IsEnabled = true;
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Wait);

                List<List<long>> campaignGroupTypes = e.Argument as List<List<long>>;

                List<long> campaigntypes = campaignGroupTypes[0];
                List<long> campaignGroups = campaignGroupTypes[1];

                string campaignTypeIDs = string.Empty;
                string campaignGroupIDs = string.Empty;

                #region Settings For Base Campaigns
                List<long> baseCampaignGroupIDs = Methods.GetTableData("SELECT FKlkpINCampaignGroup FROM INCampaignGroupSet WHERE FKlkpINCampaignGroupType = 1").AsEnumerable().Select(x => Convert.ToInt64(x["FKlkpINCampaignGroup"])).ToList();

                List<long> maccCampaignTypeIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 6", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();

                List<long> blackMaccCampaignTypeIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 3", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();

                List<long> cancerCampaignGroupIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 5", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();

                List<long> femaleDisabilityCampaignTypeIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 4", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();

                //List<long> maccMillionEliteCampaignTypeIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 3", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();

                #endregion Settings For Base Campaigns

                #region Settings For Defrost Campaigns

                List<long> defrostCampaignGroupIDs = Methods.GetTableData("SELECT FKlkpINCampaignGroup FROM INCampaignGroupSet WHERE FKlkpINCampaignGroupType = 4").AsEnumerable().Select(x => Convert.ToInt64(x["FKlkpINCampaignGroup"])).ToList();

                List<long> maccDefrostCampaignTypeIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 9", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();

                List<long> cancerDefrostCampaignGroupIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 8", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();

                List<long> femaleDisabilityDefrostCampaignTypeIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 29", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();

                #endregion Settings For Base Campaigns

                #region Settings For Mining Campaigns

                List<long> miningCampaignGroupIDs = Methods.GetTableData("SELECT FKlkpINCampaignGroup FROM INCampaignGroupSet WHERE FKlkpINCampaignGroupType = 6").AsEnumerable().Select(x => Convert.ToInt64(x["FKlkpINCampaignGroup"])).ToList();

                List<long> maccMillionCampaignGroupIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 3", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();

                List<long> cancerMiningCampaignGroupIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 5", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();

                List<long> maccMillionMiningCampaignTypeIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 3", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();

                #endregion Settings For Base Campaigns

                #region Settings For Re-Defrost Campaigns

                List<long> reDefrostCampaignGroupIDs = Methods.GetTableData("SELECT FKlkpINCampaignGroup FROM INCampaignGroupSet WHERE FKlkpINCampaignGroupType = 24").AsEnumerable().Select(x => Convert.ToInt64(x["FKlkpINCampaignGroup"])).ToList();

                List<long> maccReDefrostCampaignTypeIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 6", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();

                List<long> blackMaccReDefrostCampaignTypeIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 3", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();

                List<long> cancerReDefrostCampaignGroupIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 5", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();


                #endregion Settings For Base Campaigns

                #region Settings For Rejuvenation Campaigns
                List<long> rejuvenationCampaignGroupIDs = Methods.GetTableData("SELECT FKlkpINCampaignGroup FROM INCampaignGroupSet WHERE FKlkpINCampaignGroupType = 3").AsEnumerable().Select(x => Convert.ToInt64(x["FKlkpINCampaignGroup"])).ToList();

                List<long> maccRejuvenationCampaignGroupIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 9", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();

                List<long> cancerRejuvenationCampaignGroupIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 8", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();

                #endregion Settings For Base Campaigns

                #region Settings For Upgrade Campaigns
                List<long> upgradeCampaignGroupIDs = Methods.GetTableData("SELECT FKlkpINCampaignGroup FROM INCampaignGroupSet WHERE FKlkpINCampaignGroupType = 2").AsEnumerable().Select(x => Convert.ToInt64(x["FKlkpINCampaignGroup"])).ToList();

                List<long> cancerUpgradeCampaignTypeIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 18", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();
                List<long> maccUpgradeCampaignTypeIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 19", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();
                List<long> maccMillionUpgradeCampaignTypeIDs = Methods.GetTableData("SELECT Setting FROM INConfiguration WHERE ID = 20", IsolationLevel.Snapshot).Rows[0][0].ToString().Split(',').Select(long.Parse).ToList();

                #endregion Settings For Upgrade Campaigns




                bool first = true;
                foreach (long chk in campaigntypes)
                {
                    if (first == true)
                    {
                        first = false;
                        campaignTypeIDs = chk.ToString();
                    }
                    else
                    {
                        campaignTypeIDs = campaignTypeIDs + "," + chk.ToString();
                    }
                }

                first = true;
                foreach (long chk in campaignGroups)
                {
                    if (first == true)
                    {
                        first = false;
                        campaignGroupIDs = chk.ToString();
                    }
                    else
                    {
                        campaignGroupIDs = campaignGroupIDs + "," + chk.ToString();
                    }
                }

                Excel.Application xlApp;

                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);

                if (xlWorkBook.Worksheets.Count == 1)
                {
                    xlWorkBook.Worksheets.Add();
                    xlWorkBook.Worksheets.Add();
                }
                else if (xlWorkBook.Worksheets.Count == 2)
                {
                    xlWorkBook.Worksheets.Add();
                }

                #region Get the report data

                DataTable dtLeadAllocationData;
                DataTable dtSummary;
                //SqlParameter[] parameters = new SqlParameter[2];
                //parameters[0] = new SqlParameter("@CampaignTypes", campaignTypeIDs);
                //parameters[1] = new SqlParameter("@CampaignGroups", campaignGroupIDs);

                //DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spINReportLeadsAvailable", parameters,);

                DataSet dsLeadAllocationData = Business.Insure.INGetReportLeadsAvailableData(campaignTypeIDs, campaignGroupIDs);
                if (dsLeadAllocationData.Tables.Count > 0)
                {
                    dtLeadAllocationData = dsLeadAllocationData.Tables[0];
                    dtSummary = dsLeadAllocationData.Tables[1];
                    if (dtLeadAllocationData.Rows.Count == 0)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected Campaigns", "No Data", ShowMessageType.Information);
                        });
                        return;
                    }
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the selected Campaigns ", "No Data", ShowMessageType.Information);
                    });

                    return;
                }

                #endregion Get the report data

                #region Building the report

                #region Summary
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Name = "Summary";

                xlWorkSheet.Cells[1, 1] = "Summary Of Leads Available";
                xlWorkSheet.Cells[1, 1].Font.Bold = true;
                Excel.Range s1 = xlWorkSheet.Cells[1, 2];
                s1.Merge(true);

                xlWorkSheet.Cells[2, 1] = "Campaign";
                xlWorkSheet.Cells[2, 2] = "Leads Available";

                for (int i = 1; i <= 2; i++)//formatting of all columns
                {
                    xlWorkSheet.Cells[1, i].Interior.ColorIndex = 35;
                    if (i == 1 || i == 2)
                    {
                        if (i == 1)
                        {
                            xlWorkSheet.Columns[i].ColumnWidth = 50;
                        }
                        else
                        {
                            xlWorkSheet.Columns[i].ColumnWidth = 40;
                        }

                    }
                    else
                    {
                        xlWorkSheet.Columns[i].ColumnWidth = 20;
                    }

                    xlWorkSheet.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Columns[i].WrapText = true;
                    xlWorkSheet.Cells[2, i].Font.Bold = true;
                    xlWorkSheet.Cells[1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[2, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                int summaryRowIndex = 3;
                int totalAvailable = 0;
                foreach (DataRow rw in dtSummary.Rows)
                {
                    xlWorkSheet.Cells[summaryRowIndex, 1] = rw["CampaignName"]; xlWorkSheet.Cells[summaryRowIndex, 1].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    xlWorkSheet.Cells[summaryRowIndex, 2] = rw["LeadsAvailable"]; xlWorkSheet.Cells[summaryRowIndex, 2].Borders.Weight = Excel.XlBorderWeight.xlThin;
                    totalAvailable = totalAvailable + int.Parse(rw["LeadsAvailable"].ToString());
                    summaryRowIndex++;
                }
                if (dtSummary.Rows.Count > 0)
                {
                    //totals here
                    xlWorkSheet.Cells[summaryRowIndex, 1] = "Total"; xlWorkSheet.Cells[summaryRowIndex, 1].Borders.Weight = Excel.XlBorderWeight.xlMedium; xlWorkSheet.Cells[summaryRowIndex, 1].Font.Bold = true;
                    xlWorkSheet.Cells[summaryRowIndex, 2] = totalAvailable; xlWorkSheet.Cells[summaryRowIndex, 2].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                #endregion Summary

                #region base campaigns
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2);
                xlWorkSheet.Name = "Base Campaigns";

                #region Base

                xlWorkSheet.Cells[1, 1] = "Base Campaigns";
                xlWorkSheet.Cells[1, 1].Font.Bold = true;
                Excel.Range c1 = xlWorkSheet.Cells[1, 2];
                c1.Merge(true);

                xlWorkSheet.Cells[2, 1] = "Campaign";
                xlWorkSheet.Cells[2, 2] = "Batch";
                xlWorkSheet.Cells[2, 3] = "Total Leads In Batch";
                xlWorkSheet.Cells[2, 4] = "Leads Available";

                for (int i = 1; i <= 4; i++)//formatting of all columns
                {
                    xlWorkSheet.Cells[1, i].Interior.ColorIndex = 37;
                    if (i == 1 || i == 2)
                    {
                        if (i == 1)
                        {
                            xlWorkSheet.Columns[i].ColumnWidth = 50;
                        }
                        else
                        {
                            xlWorkSheet.Columns[i].ColumnWidth = 40;
                        }
                    }
                    else
                    {
                        xlWorkSheet.Columns[i].ColumnWidth = 20;
                    }

                    xlWorkSheet.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Columns[i].WrapText = true;
                    xlWorkSheet.Cells[2, i].Font.Bold = true;
                    xlWorkSheet.Cells[1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[2, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }

                int rowIndex = 3;
                List<long> addedBatchIDs = new List<long>();
                int totalLeads = 0;
                int totalLeadsAvailable = 0;

                foreach (DataRow row in dtLeadAllocationData.Rows)
                {
                    if (row["FKINCampaignGroupID"].ToString() == "1")//base
                    {
                        string campaignCode = row["CampaignCode"].ToString();
                        long batchID = long.Parse(row["BatchID"].ToString());
                        if (!addedBatchIDs.Contains(batchID))
                        {
                            addedBatchIDs.Add(batchID);
                            for (int i = 2; i <= 4; i++)//formatting of all columns
                            {
                                xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 37;
                                xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            }
                            //Previous line
                            //int AvailableLeads = dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID &&  x["FKUserID"].ToString() == string.Empty).ToList().Count;
                            int AvailableLeads = dtLeadAllocationData.Select("BatchID = '" + batchID + "' AND CampaignID ='" + row["CampaignID"].ToString() + "' AND FKUserID is null AND (IsCopied = 0 OR IsCopied IS NULL)").Count();

                            xlWorkSheet.Cells[rowIndex, 1] = row["Campaign"].ToString();
                            xlWorkSheet.Cells[rowIndex, 2] = row["BatchCode"].ToString();
                            xlWorkSheet.Cells[rowIndex, 3] = row["TotalLeads"].ToString();//dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID).ToList().Count;
                            xlWorkSheet.Cells[rowIndex, 4] = AvailableLeads;
                            // xlWorkSheet.Cells[rowIndex, 2] = row["Campaign"].ToString();
                            totalLeads = totalLeads + int.Parse(row["TotalLeads"].ToString());
                            totalLeadsAvailable = totalLeadsAvailable + AvailableLeads;
                            rowIndex++;
                        }

                    }
                }
                xlWorkSheet.Cells[rowIndex, 1] = "Totals";
                xlWorkSheet.Cells[rowIndex, 3] = totalLeads;
                xlWorkSheet.Cells[rowIndex, 4] = totalLeadsAvailable;
                for (int i = 1; i <= 4; i++)//formatting of all columns
                {
                    xlWorkSheet.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Columns[i].WrapText = true;
                    xlWorkSheet.Cells[rowIndex, i].Font.Bold = true;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                totalLeads = 0;
                totalLeadsAvailable = 0;
                rowIndex++;

                #endregion Base

                #region Rejuvenation & Defrosting

                rowIndex++;
                xlWorkSheet.Cells[rowIndex, 1] = "Rejuvenation & Defrosting Campaigns";
                for (int i = 1; i <= 4; i++)//formatting of all columns
                {
                    xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 40;
                    if (i == 1 || i == 2)
                    {
                        if (i == 1)
                        {
                            xlWorkSheet.Columns[i].ColumnWidth = 50;
                        }
                        else
                        {
                            xlWorkSheet.Columns[i].ColumnWidth = 40;
                        }

                    }
                    else
                    {
                        xlWorkSheet.Columns[i].ColumnWidth = 20;
                    }

                    xlWorkSheet.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Columns[i].WrapText = true;
                    xlWorkSheet.Cells[rowIndex, i].Font.Bold = true;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;

                    xlWorkSheet.Cells[rowIndex + 1, i].Font.Bold = true;
                    xlWorkSheet.Cells[rowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[rowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                rowIndex++;
                xlWorkSheet.Cells[rowIndex, 1] = "Campaign";
                xlWorkSheet.Cells[rowIndex, 2] = "Batch";
                xlWorkSheet.Cells[rowIndex, 3] = "Total Leads In Batch";
                xlWorkSheet.Cells[rowIndex, 4] = "Leads Available";



                rowIndex++;
                foreach (DataRow row in dtLeadAllocationData.Rows)
                {
                    if (row["FKINCampaignGroupID"].ToString() == "3" || row["FKINCampaignGroupID"].ToString() == "4" || row["FKINCampaignGroupID"].ToString() == "24")//rejuvenation & defrosting
                    {
                        string campaignCode = row["CampaignCode"].ToString();
                        long batchID = long.Parse(row["BatchID"].ToString());
                        if (!addedBatchIDs.Contains(batchID))
                        {
                            addedBatchIDs.Add(batchID);
                            for (int i = 2; i <= 4; i++)//formatting of all columns
                            {
                                xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 40;
                                xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            }
                            int availableLeads = dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID && x["FKUserID"].ToString() == string.Empty).ToList().Count;
                            xlWorkSheet.Cells[rowIndex, 1] = row["Campaign"].ToString();
                            xlWorkSheet.Cells[rowIndex, 2] = row["BatchCode"].ToString();
                            xlWorkSheet.Cells[rowIndex, 3] = row["TotalLeads"].ToString();//dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID).ToList().Count;
                            xlWorkSheet.Cells[rowIndex, 4] = availableLeads;
                            // xlWorkSheet.Cells[rowIndex, 2] = row["Campaign"].ToString();
                            totalLeads = totalLeads + int.Parse(row["TotalLeads"].ToString());
                            totalLeadsAvailable = totalLeadsAvailable + availableLeads;
                            rowIndex++;
                        }

                    }
                }
                xlWorkSheet.Cells[rowIndex, 1] = "Totals";
                xlWorkSheet.Cells[rowIndex, 3] = totalLeads;
                xlWorkSheet.Cells[rowIndex, 4] = totalLeadsAvailable;
                for (int i = 1; i <= 4; i++)//formatting of all columns
                {
                    xlWorkSheet.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Columns[i].WrapText = true;
                    xlWorkSheet.Cells[rowIndex, i].Font.Bold = true;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                totalLeads = 0;
                totalLeadsAvailable = 0;
                rowIndex++;

                #endregion Rejuvenation & Defrosting

                #region Re-Activation

                rowIndex++;
                xlWorkSheet.Cells[rowIndex, 1] = "Re-Activation";
                for (int i = 1; i <= 4; i++)//formatting of all columns
                {
                    xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 40;
                    if (i == 1 || i == 2)
                    {
                        if (i == 1)
                        {
                            xlWorkSheet.Columns[i].ColumnWidth = 50;
                        }
                        else
                        {
                            xlWorkSheet.Columns[i].ColumnWidth = 40;
                        }

                    }
                    else
                    {
                        xlWorkSheet.Columns[i].ColumnWidth = 20;
                    }

                    xlWorkSheet.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Columns[i].WrapText = true;
                    xlWorkSheet.Cells[rowIndex, i].Font.Bold = true;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;

                    xlWorkSheet.Cells[rowIndex + 1, i].Font.Bold = true;
                    xlWorkSheet.Cells[rowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[rowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                rowIndex++;
                xlWorkSheet.Cells[rowIndex, 1] = "Campaign";
                xlWorkSheet.Cells[rowIndex, 2] = "Batch";
                xlWorkSheet.Cells[rowIndex, 3] = "Total Leads In Batch";
                xlWorkSheet.Cells[rowIndex, 4] = "Leads Available";



                rowIndex++;
                foreach (DataRow row in dtLeadAllocationData.Rows)
                {
                    if (row["FKINCampaignGroupID"].ToString() == "21")//re-activation
                    {
                        string campaignCode = row["CampaignCode"].ToString();
                        long batchID = long.Parse(row["BatchID"].ToString());
                        if (!addedBatchIDs.Contains(batchID))
                        {
                            addedBatchIDs.Add(batchID);
                            for (int i = 2; i <= 4; i++)//formatting of all columns
                            {
                                xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 40;
                                xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            }
                            int availableLeads = dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID && x["FKUserID"].ToString() == string.Empty).ToList().Count;
                            xlWorkSheet.Cells[rowIndex, 1] = row["Campaign"].ToString();
                            xlWorkSheet.Cells[rowIndex, 2] = row["BatchCode"].ToString();
                            xlWorkSheet.Cells[rowIndex, 3] = row["TotalLeads"].ToString();//dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID).ToList().Count;
                            xlWorkSheet.Cells[rowIndex, 4] = availableLeads;
                            // xlWorkSheet.Cells[rowIndex, 2] = row["Campaign"].ToString();
                            totalLeads = totalLeads + int.Parse(row["TotalLeads"].ToString());
                            totalLeadsAvailable = totalLeadsAvailable + availableLeads;
                            rowIndex++;
                        }

                    }
                }
                xlWorkSheet.Cells[rowIndex, 1] = "Totals";
                xlWorkSheet.Cells[rowIndex, 3] = totalLeads;
                xlWorkSheet.Cells[rowIndex, 4] = totalLeadsAvailable;
                for (int i = 1; i <= 4; i++)//formatting of all columns
                {
                    xlWorkSheet.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Columns[i].WrapText = true;
                    xlWorkSheet.Cells[rowIndex, i].Font.Bold = true;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                totalLeads = 0;
                totalLeadsAvailable = 0;
                rowIndex++;

                #endregion Re-Activation

                #region Extension

                rowIndex++;
                xlWorkSheet.Cells[rowIndex, 1] = "Extension Leads";
                for (int i = 1; i <= 4; i++)//formatting of all columns
                {
                    xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 40;
                    if (i == 1 || i == 2)
                    {
                        if (i == 1)
                        {
                            xlWorkSheet.Columns[i].ColumnWidth = 50;
                        }
                        else
                        {
                            xlWorkSheet.Columns[i].ColumnWidth = 40;
                        }

                    }
                    else
                    {
                        xlWorkSheet.Columns[i].ColumnWidth = 20;
                    }

                    xlWorkSheet.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Columns[i].WrapText = true;
                    xlWorkSheet.Cells[rowIndex, i].Font.Bold = true;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;

                    xlWorkSheet.Cells[rowIndex + 1, i].Font.Bold = true;
                    xlWorkSheet.Cells[rowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[rowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                rowIndex++;
                xlWorkSheet.Cells[rowIndex, 1] = "Campaign";
                xlWorkSheet.Cells[rowIndex, 2] = "Batch";
                xlWorkSheet.Cells[rowIndex, 3] = "Total Leads In Batch";
                xlWorkSheet.Cells[rowIndex, 4] = "Leads Available";



                rowIndex++;
                foreach (DataRow row in dtLeadAllocationData.Rows)
                {
                    if (row["FKINCampaignGroupID"].ToString() == "22")//extension
                    {
                        string campaignCode = row["CampaignCode"].ToString();
                        long batchID = long.Parse(row["BatchID"].ToString());
                        if (!addedBatchIDs.Contains(batchID))
                        {
                            addedBatchIDs.Add(batchID);
                            for (int i = 2; i <= 4; i++)//formatting of all columns
                            {
                                xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 40;
                                xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            }
                            int availableLeads = dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID && x["FKUserID"].ToString() == string.Empty).ToList().Count;
                            xlWorkSheet.Cells[rowIndex, 1] = row["Campaign"].ToString();
                            xlWorkSheet.Cells[rowIndex, 2] = row["BatchCode"].ToString();
                            xlWorkSheet.Cells[rowIndex, 3] = row["TotalLeads"].ToString();//dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID).ToList().Count;
                            xlWorkSheet.Cells[rowIndex, 4] = availableLeads;
                            // xlWorkSheet.Cells[rowIndex, 2] = row["Campaign"].ToString();
                            totalLeads = totalLeads + int.Parse(row["TotalLeads"].ToString());
                            totalLeadsAvailable = totalLeadsAvailable + availableLeads;
                            rowIndex++;
                        }

                    }
                }
                xlWorkSheet.Cells[rowIndex, 1] = "Totals";
                xlWorkSheet.Cells[rowIndex, 3] = totalLeads;
                xlWorkSheet.Cells[rowIndex, 4] = totalLeadsAvailable;
                for (int i = 1; i <= 4; i++)//formatting of all columns
                {
                    xlWorkSheet.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Columns[i].WrapText = true;
                    xlWorkSheet.Cells[rowIndex, i].Font.Bold = true;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                totalLeads = 0;
                totalLeadsAvailable = 0;
                rowIndex++;

                #endregion Extension

                #region Renewals

                rowIndex++;
                xlWorkSheet.Cells[rowIndex, 1] = "Renewals Leads";
                for (int i = 1; i <= 4; i++)//formatting of all columns
                {
                    xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 40;
                    if (i == 1 || i == 2)
                    {
                        if (i == 1)
                        {
                            xlWorkSheet.Columns[i].ColumnWidth = 50;
                        }
                        else
                        {
                            xlWorkSheet.Columns[i].ColumnWidth = 40;
                        }

                    }
                    else
                    {
                        xlWorkSheet.Columns[i].ColumnWidth = 20;
                    }

                    xlWorkSheet.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Columns[i].WrapText = true;
                    xlWorkSheet.Cells[rowIndex, i].Font.Bold = true;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;

                    xlWorkSheet.Cells[rowIndex + 1, i].Font.Bold = true;
                    xlWorkSheet.Cells[rowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[rowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                rowIndex++;
                xlWorkSheet.Cells[rowIndex, 1] = "Campaign";
                xlWorkSheet.Cells[rowIndex, 2] = "Batch";
                xlWorkSheet.Cells[rowIndex, 3] = "Total Leads In Batch";
                xlWorkSheet.Cells[rowIndex, 4] = "Leads Available";

                rowIndex++;
                foreach (DataRow row in dtLeadAllocationData.Rows)
                {
                    if (row["FKINCampaignGroupID"].ToString() == "6")//renewals
                    {
                        string campaignCode = row["CampaignCode"].ToString();
                        long batchID = long.Parse(row["BatchID"].ToString());
                        if (!addedBatchIDs.Contains(batchID))
                        {
                            addedBatchIDs.Add(batchID);
                            for (int i = 2; i <= 4; i++)//formatting of all columns
                            {
                                xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 40;
                                xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            }
                            int availableLeads = dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID && x["FKUserID"].ToString() == string.Empty).ToList().Count;
                            xlWorkSheet.Cells[rowIndex, 1] = row["Campaign"].ToString();
                            xlWorkSheet.Cells[rowIndex, 2] = row["BatchCode"].ToString();
                            xlWorkSheet.Cells[rowIndex, 3] = row["TotalLeads"].ToString();//dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID).ToList().Count;
                            xlWorkSheet.Cells[rowIndex, 4] = availableLeads;
                            // xlWorkSheet.Cells[rowIndex, 2] = row["Campaign"].ToString();
                            totalLeads = totalLeads + int.Parse(row["TotalLeads"].ToString());
                            totalLeadsAvailable = totalLeadsAvailable + availableLeads;
                            rowIndex++;
                        }

                    }
                }
                xlWorkSheet.Cells[rowIndex, 1] = "Totals";
                xlWorkSheet.Cells[rowIndex, 3] = totalLeads;
                xlWorkSheet.Cells[rowIndex, 4] = totalLeadsAvailable;
                for (int i = 1; i <= 4; i++)//formatting of all columns
                {
                    xlWorkSheet.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Columns[i].WrapText = true;
                    xlWorkSheet.Cells[rowIndex, i].Font.Bold = true;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                totalLeads = 0;
                totalLeadsAvailable = 0;
                rowIndex++;

                #endregion Renewals

                #region Other

                rowIndex++;
                xlWorkSheet.Cells[rowIndex, 1] = "Other";
                for (int i = 1; i <= 4; i++)//formatting of all columns
                {
                    xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 40;
                    if (i == 1 || i == 2)
                    {
                        if (i == 1)
                        {
                            xlWorkSheet.Columns[i].ColumnWidth = 50;
                        }
                        else
                        {
                            xlWorkSheet.Columns[i].ColumnWidth = 40;
                        }

                    }
                    else
                    {
                        xlWorkSheet.Columns[i].ColumnWidth = 20;
                    }

                    xlWorkSheet.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Columns[i].WrapText = true;
                    xlWorkSheet.Cells[rowIndex, i].Font.Bold = true;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;

                    xlWorkSheet.Cells[rowIndex + 1, i].Font.Bold = true;
                    xlWorkSheet.Cells[rowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[rowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                rowIndex++;
                xlWorkSheet.Cells[rowIndex, 1] = "Campaign";
                xlWorkSheet.Cells[rowIndex, 2] = "Batch";
                xlWorkSheet.Cells[rowIndex, 3] = "Total Leads In Batch";
                xlWorkSheet.Cells[rowIndex, 4] = "Leads Available";

                rowIndex++;
                foreach (DataRow row in dtLeadAllocationData.Rows)
                {
                    if (
                        //row["FKINCampaignGroupID"].ToString() != "1" &&
                        //row["FKINCampaignGroupID"].ToString() != "2" &&
                        //row["FKINCampaignGroupID"].ToString() != "3" &&
                        //row["FKINCampaignGroupID"].ToString() != "4" &&
                        //row["FKINCampaignGroupID"].ToString() != "6" &&
                        //row["FKINCampaignGroupID"].ToString() != "7" &&
                        //row["FKINCampaignGroupID"].ToString() != "8" &&
                        //row["FKINCampaignGroupID"].ToString() != "9" &&
                        //row["FKINCampaignGroupID"].ToString() != "10" &&
                        //row["FKINCampaignGroupID"].ToString() != "11" &&
                        //row["FKINCampaignGroupID"].ToString() != "12" &&
                        //row["FKINCampaignGroupID"].ToString() != "13" &&
                        //row["FKINCampaignGroupID"].ToString() != "14" &&
                        //row["FKINCampaignGroupID"].ToString() != "15" &&
                        //row["FKINCampaignGroupID"].ToString() != "16" &&
                        //row["FKINCampaignGroupID"].ToString() != "17" &&
                        //row["FKINCampaignGroupID"].ToString() != "18" &&
                        //row["FKINCampaignGroupID"].ToString() != "19" &&
                        //row["FKINCampaignGroupID"].ToString() != "20" &&
                        //row["FKINCampaignGroupID"].ToString() != "21" &&
                        //row["FKINCampaignGroupID"].ToString() != "22" &&
                        //row["FKINCampaignGroupID"].ToString() != "23" &&
                        //row["FKINCampaignGroupID"].ToString() != "24" &&
                        //row["FKINCampaignGroupID"].ToString() != "27" &&
                        //row["FKINCampaignGroupID"].ToString() != "28" &&
                        //row["FKINCampaignGroupID"].ToString() != "29" &&
                        //row["FKINCampaignGroupID"].ToString() != "30") //other
                        !(upgradeCampaignGroupIDs.Contains(Convert.ToInt64(row["FKINCampaignGroupID"])))) //other
                    {
                        string campaignCode = row["CampaignCode"].ToString();
                        long batchID = long.Parse(row["BatchID"].ToString());
                        if (!addedBatchIDs.Contains(batchID))
                        {
                            addedBatchIDs.Add(batchID);
                            for (int i = 2; i <= 4; i++)//formatting of all columns
                            {
                                xlWorkSheet.Cells[rowIndex, i].Interior.ColorIndex = 40;
                                xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                            }
                            int availableLeads = dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID && x["FKUserID"].ToString() == string.Empty).ToList().Count;
                            xlWorkSheet.Cells[rowIndex, 1] = row["Campaign"].ToString();
                            xlWorkSheet.Cells[rowIndex, 2] = row["BatchCode"].ToString();
                            xlWorkSheet.Cells[rowIndex, 3] = row["TotalLeads"].ToString();//dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID).ToList().Count;
                            xlWorkSheet.Cells[rowIndex, 4] = availableLeads;
                            // xlWorkSheet.Cells[rowIndex, 2] = row["Campaign"].ToString();
                            totalLeads = totalLeads + int.Parse(row["TotalLeads"].ToString());
                            totalLeadsAvailable = totalLeadsAvailable + availableLeads;
                            rowIndex++;
                        }

                    }
                }
                xlWorkSheet.Cells[rowIndex, 1] = "Totals";
                xlWorkSheet.Cells[rowIndex, 3] = totalLeads;
                xlWorkSheet.Cells[rowIndex, 4] = totalLeadsAvailable;
                for (int i = 1; i <= 4; i++)//formatting of all columns
                {
                    xlWorkSheet.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheet.Columns[i].WrapText = true;
                    xlWorkSheet.Cells[rowIndex, i].Font.Bold = true;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheet.Cells[rowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                totalLeads = 0;
                totalLeadsAvailable = 0;
                rowIndex++;

                #endregion Other

                #endregion Base Campaigns

                #region Upgrade Campaigns
                Excel.Worksheet xlWorkSheetUpgrades;
                xlWorkSheetUpgrades = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(3);
                xlWorkSheetUpgrades.Name = "Upgrade Campaigns";

                int upgradeRowIndex = 1;

                #region Cancer Upgrades

                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 1] = "Cancer Upgrades";
                for (int i = 1; i <= 4; i++)//formatting of all columns
                {
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Interior.ColorIndex = 40;
                    if (i == 1 || i == 2)
                    {
                        if (i == 1)
                        {
                            xlWorkSheetUpgrades.Columns[i].ColumnWidth = 50;
                        }
                        else
                        {
                            xlWorkSheetUpgrades.Columns[i].ColumnWidth = 40;
                        }

                    }
                    else
                    {
                        xlWorkSheetUpgrades.Columns[i].ColumnWidth = 20;
                    }

                    xlWorkSheetUpgrades.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheetUpgrades.Columns[i].WrapText = true;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Font.Bold = true;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;

                    xlWorkSheetUpgrades.Cells[upgradeRowIndex + 1, i].Font.Bold = true;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                upgradeRowIndex++;
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 1] = "Campaign";
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 2] = "Batch";
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 3] = "Total Leads In Batch";
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 4] = "Leads Available";



                upgradeRowIndex++;
                foreach (DataRow row in dtLeadAllocationData.Rows)
                {
                    if (
                        //row["CampaignTypeID"].ToString() == "1" ||
                        //row["CampaignTypeID"].ToString() == "5" ||
                        //row["CampaignTypeID"].ToString() == "9" ||
                        //row["CampaignTypeID"].ToString() == "12")
                        cancerUpgradeCampaignTypeIDs.Contains(Convert.ToInt64(row["CampaignTypeID"])))
                    {
                        if (
                                //row["FKINCampaignGroupID"].ToString() == "2" ||
                                //row["FKINCampaignGroupID"].ToString() == "7" ||
                                //row["FKINCampaignGroupID"].ToString() == "8" ||
                                //row["FKINCampaignGroupID"].ToString() == "9" ||
                                //row["FKINCampaignGroupID"].ToString() == "10" ||
                                //row["FKINCampaignGroupID"].ToString() == "11" ||
                                //row["FKINCampaignGroupID"].ToString() == "12" ||
                                //row["FKINCampaignGroupID"].ToString() == "13" ||
                                //row["FKINCampaignGroupID"].ToString() == "14" ||
                                //row["FKINCampaignGroupID"].ToString() == "15" ||
                                //row["FKINCampaignGroupID"].ToString() == "16" ||
                                //row["FKINCampaignGroupID"].ToString() == "17" ||
                                //row["FKINCampaignGroupID"].ToString() == "18" ||
                                //row["FKINCampaignGroupID"].ToString() == "19" ||
                                //row["FKINCampaignGroupID"].ToString() == "20" ||
                                //row["FKINCampaignGroupID"].ToString() == "23" ||
                                //row["FKINCampaignGroupID"].ToString() == "27" ||
                                //row["FKINCampaignGroupID"].ToString() == "28" ||
                                //row["FKINCampaignGroupID"].ToString() == "29" ||
                                //row["FKINCampaignGroupID"].ToString() == "30") //cancer upgrades
                                upgradeCampaignGroupIDs.Contains(Convert.ToInt64(row["FKINCampaignGroupID"]))) //cancer upgrades
                        {
                            string campaignCode = row["CampaignCode"].ToString();
                            long batchID = long.Parse(row["BatchID"].ToString());
                            if (!addedBatchIDs.Contains(batchID))
                            {
                                addedBatchIDs.Add(batchID);
                                for (int i = 2; i <= 4; i++)//formatting of all columns
                                {
                                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Interior.ColorIndex = 40;
                                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                                }
                                int availableLeads = dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID && x["FKUserID"].ToString() == string.Empty).ToList().Count;
                                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 1] = row["Campaign"].ToString();
                                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 2] = row["BatchCode"].ToString();
                                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 3] = row["TotalLeads"].ToString();//dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID).ToList().Count;
                                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 4] = availableLeads;
                                // xlWorkSheet.Cells[rowIndex, 2] = row["Campaign"].ToString();
                                totalLeads = totalLeads + int.Parse(row["TotalLeads"].ToString());
                                totalLeadsAvailable = totalLeadsAvailable + availableLeads;
                                upgradeRowIndex++;
                            }
                        }
                    }
                }
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 1] = "Totals";
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 3] = totalLeads;
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 4] = totalLeadsAvailable;
                for (int i = 1; i <= 4; i++) //formatting of all columns
                {
                    xlWorkSheetUpgrades.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheetUpgrades.Columns[i].WrapText = true;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Font.Bold = true;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                totalLeads = 0;
                totalLeadsAvailable = 0;
                upgradeRowIndex++;

                #endregion Cancer Upgrades

                #region Macc Upgrades

                //macc upgrades
                upgradeRowIndex++;
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 1] = "Macc Upgrades";
                for (int i = 1; i <= 4; i++)//formatting of all columns
                {
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Interior.ColorIndex = 37;
                    if (i == 1 || i == 2)
                    {
                        if (i == 1)
                        {
                            xlWorkSheetUpgrades.Columns[i].ColumnWidth = 50;
                        }
                        else
                        {
                            xlWorkSheetUpgrades.Columns[i].ColumnWidth = 40;
                        }

                    }
                    else
                    {
                        xlWorkSheetUpgrades.Columns[i].ColumnWidth = 20;
                    }

                    xlWorkSheetUpgrades.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheetUpgrades.Columns[i].WrapText = true;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Font.Bold = true;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;

                    xlWorkSheetUpgrades.Cells[upgradeRowIndex + 1, i].Font.Bold = true;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                upgradeRowIndex++;
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 1] = "Campaign";
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 2] = "Batch";
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 3] = "Total Leads In Batch";
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 4] = "Leads Available";

                upgradeRowIndex++;
                foreach (DataRow row in dtLeadAllocationData.Rows)
                {
                    if (
                        //row["CampaignTypeID"].ToString() == "2" ||
                        //row["CampaignTypeID"].ToString() == "7" ||
                        //row["CampaignTypeID"].ToString() == "8") 
                        maccUpgradeCampaignTypeIDs.Contains(Convert.ToInt64(row["CampaignTypeID"])))
                    {
                        if (
                            //row["FKINCampaignGroupID"].ToString() == "2" ||
                            //row["FKINCampaignGroupID"].ToString() == "7" ||
                            //row["FKINCampaignGroupID"].ToString() == "8" ||
                            //row["FKINCampaignGroupID"].ToString() == "9" ||
                            //row["FKINCampaignGroupID"].ToString() == "10" ||
                            //row["FKINCampaignGroupID"].ToString() == "11" ||
                            //row["FKINCampaignGroupID"].ToString() == "12" ||
                            //row["FKINCampaignGroupID"].ToString() == "13" ||
                            //row["FKINCampaignGroupID"].ToString() == "14" ||
                            //row["FKINCampaignGroupID"].ToString() == "15" ||
                            //row["FKINCampaignGroupID"].ToString() == "16" ||
                            //row["FKINCampaignGroupID"].ToString() == "17" ||
                            //row["FKINCampaignGroupID"].ToString() == "18" ||
                            //row["FKINCampaignGroupID"].ToString() == "19" ||
                            //row["FKINCampaignGroupID"].ToString() == "20" ||
                            //row["FKINCampaignGroupID"].ToString() == "23" ||
                            //row["FKINCampaignGroupID"].ToString() == "27" ||
                            //row["FKINCampaignGroupID"].ToString() == "28" ||
                            //row["FKINCampaignGroupID"].ToString() == "29" ||
                            //row["FKINCampaignGroupID"].ToString() == "30") //macc upgrades
                            upgradeCampaignGroupIDs.Contains(Convert.ToInt64(row["FKINCampaignGroupID"])))//macc upgrades
                        {
                            string campaignCode = row["CampaignCode"].ToString();
                            long batchID = long.Parse(row["BatchID"].ToString());
                            if (!addedBatchIDs.Contains(batchID))
                            {
                                addedBatchIDs.Add(batchID);
                                for (int i = 2; i <= 4; i++)//formatting of all columns
                                {
                                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Interior.ColorIndex = 37;
                                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                                }
                                int availableLeads = dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID && x["FKUserID"].ToString() == string.Empty).ToList().Count;
                                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 1] = row["Campaign"].ToString();
                                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 2] = row["BatchCode"].ToString();
                                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 3] = row["TotalLeads"].ToString();//dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID).ToList().Count;
                                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 4] = availableLeads;
                                // xlWorkSheet.Cells[rowIndex, 2] = row["Campaign"].ToString();
                                totalLeads = totalLeads + int.Parse(row["TotalLeads"].ToString());
                                totalLeadsAvailable = totalLeadsAvailable + availableLeads;
                                upgradeRowIndex++;
                            }

                        }
                    }
                }
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 1] = "Totals";
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 3] = totalLeads;
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 4] = totalLeadsAvailable;
                for (int i = 1; i <= 4; i++)//formatting of all columns
                {
                    xlWorkSheetUpgrades.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheetUpgrades.Columns[i].WrapText = true;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Font.Bold = true;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                totalLeads = 0;
                totalLeadsAvailable = 0;
                upgradeRowIndex++;

                #endregion Macc Upgrades

                #region Macc Million Upgrades

                //macc upgrades
                upgradeRowIndex++;
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 1] = "Macc Million Upgrades";

                for (int i = 1; i <= 4; i++)//formatting of all columns
                {
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Interior.ColorIndex = 36;
                    if (i == 1 || i == 2)
                    {
                        if (i == 1)
                        {
                            xlWorkSheetUpgrades.Columns[i].ColumnWidth = 50;
                        }
                        else
                        {
                            xlWorkSheetUpgrades.Columns[i].ColumnWidth = 40;
                        }

                    }
                    else
                    {
                        xlWorkSheetUpgrades.Columns[i].ColumnWidth = 20;
                    }

                    xlWorkSheetUpgrades.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheetUpgrades.Columns[i].WrapText = true;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Font.Bold = true;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;

                    xlWorkSheetUpgrades.Cells[upgradeRowIndex + 1, i].Font.Bold = true;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex + 1, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                upgradeRowIndex++;
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 1] = "Campaign";
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 2] = "Batch";
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 3] = "Total Leads In Batch";
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 4] = "Leads Available";

                upgradeRowIndex++;
                foreach (DataRow row in dtLeadAllocationData.Rows)
                {
                    if (
                        //row["CampaignTypeID"].ToString() == "3" ||
                        //row["CampaignTypeID"].ToString() == "11")
                        maccMillionUpgradeCampaignTypeIDs.Contains(Convert.ToInt64(row["CampaignTypeID"])))
                    {
                        if (
                            //row["FKINCampaignGroupID"].ToString() == "2" ||
                            //row["FKINCampaignGroupID"].ToString() == "7" ||
                            //row["FKINCampaignGroupID"].ToString() == "8" ||
                            //row["FKINCampaignGroupID"].ToString() == "9" ||
                            //row["FKINCampaignGroupID"].ToString() == "10" ||
                            //row["FKINCampaignGroupID"].ToString() == "11" ||
                            //row["FKINCampaignGroupID"].ToString() == "12" ||
                            //row["FKINCampaignGroupID"].ToString() == "13" ||
                            //row["FKINCampaignGroupID"].ToString() == "14" ||
                            //row["FKINCampaignGroupID"].ToString() == "15" ||
                            //row["FKINCampaignGroupID"].ToString() == "16" ||
                            //row["FKINCampaignGroupID"].ToString() == "17" ||
                            //row["FKINCampaignGroupID"].ToString() == "18" ||
                            //row["FKINCampaignGroupID"].ToString() == "19" ||
                            //row["FKINCampaignGroupID"].ToString() == "20" ||
                            //row["FKINCampaignGroupID"].ToString() == "23" ||
                            //row["FKINCampaignGroupID"].ToString() == "27" ||
                            //row["FKINCampaignGroupID"].ToString() == "28" ||
                            //row["FKINCampaignGroupID"].ToString() == "29" ||
                            //row["FKINCampaignGroupID"].ToString() == "30") //macc million upgrades
                            upgradeCampaignGroupIDs.Contains(Convert.ToInt64(row["FKINCampaignGroupID"]))) //macc million upgrades 
                        {
                            string campaignCode = row["CampaignCode"].ToString();
                            long batchID = long.Parse(row["BatchID"].ToString());
                            if (!addedBatchIDs.Contains(batchID))
                            {
                                addedBatchIDs.Add(batchID);
                                for (int i = 2; i <= 4; i++)//formatting of all columns
                                {
                                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Interior.ColorIndex = 36;
                                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                                }
                                int availableLeads = dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID && x["FKUserID"].ToString() == string.Empty).ToList().Count;
                                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 1] = row["Campaign"].ToString();
                                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 2] = row["BatchCode"].ToString();
                                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 3] = row["TotalLeads"].ToString();//dtLeadAllocationData.AsEnumerable().Where(x => long.Parse(x["BatchID"].ToString()) == batchID).ToList().Count;
                                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 4] = availableLeads;
                                // xlWorkSheet.Cells[rowIndex, 2] = row["Campaign"].ToString();
                                totalLeads = totalLeads + int.Parse(row["TotalLeads"].ToString());
                                totalLeadsAvailable = totalLeadsAvailable + availableLeads;
                                upgradeRowIndex++;
                            }
                        }
                    }
                }
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 1] = "Totals";
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 3] = totalLeads;
                xlWorkSheetUpgrades.Cells[upgradeRowIndex, 4] = totalLeadsAvailable;
                for (int i = 1; i <= 4; i++)//formatting of all columns
                {
                    xlWorkSheetUpgrades.Columns[i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    xlWorkSheetUpgrades.Columns[i].WrapText = true;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Font.Bold = true;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                    xlWorkSheetUpgrades.Cells[upgradeRowIndex, i].Borders.Weight = Excel.XlBorderWeight.xlMedium;
                }
                totalLeads = 0;
                totalLeadsAvailable = 0;
                upgradeRowIndex++;

                #endregion Macc Million Upgrades

                #endregion Upgrade Campaigns

                //string filePathAndName = GlobalSettings.UserFolder  + " Leads Available  Report ~ " + DateTime.Now.Millisecond + ".xls";
                string filePathAndName = String.Format("{0}Leads Available Report ~ {1}.xls", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
                xlWorkBook.SaveAs(filePathAndName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                xlWorkBook.Close(true, misValue, misValue);

                xlApp.Quit();

                //Display excel document
                Process.Start(filePathAndName);

                #endregion Building the report


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

        private void chkSelectAllCampaignTypes_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (CheckBox chk in lstCampaignTypes.Items)
                {
                    chk.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void chkSelectAllCampaignTypes_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (CheckBox chk in lstCampaignTypes.Items)
                {
                    chk.IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void chkSelectAllCampaignGroups_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (CheckBox chk in lstCampaignGroups.Items)
                {
                    chk.IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void chkSelectAllCampaignGroups_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (CheckBox chk in lstCampaignGroups.Items)
                {
                    chk.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void chkSelectAllUpgradeCampaigns_Checked(object sender, RoutedEventArgs e)
        {

            try
            {


                string query = "Select Description from lkpINCampaignGroup where Description LIKE '%Upgrade%'";

                List<string> UpgradeCampaigns = Methods.GetTableData(query).AsEnumerable().Select(x => Convert.ToString(x["Description"])).ToList();

                foreach (CheckBox chk in lstCampaignGroups.Items)
                {

                    foreach (string match in UpgradeCampaigns)
                    {
                        if (chk.Content.ToString() == match)
                        {
                            chk.IsChecked = true;
                            break;
                        }
                    }


                }

            }


            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        private void chkSelectAllUpgradeCampaigns_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {

                foreach (CheckBox chk in lstCampaignGroups.Items)
                {
                    chk.IsChecked = false;

                }

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}
