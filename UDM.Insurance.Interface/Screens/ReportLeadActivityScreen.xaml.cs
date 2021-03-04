using System.Data.SqlClient;
using System.Windows.Resources;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Documents.Excel;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using Orientation = Infragistics.Documents.Excel.Orientation;

namespace UDM.Insurance.Interface.Screens
{

    public partial class ReportLeadActivityScreen
    {

        #region Constants


        #endregion



        #region Private Members

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        #endregion



        #region Constructors

        public ReportLeadActivityScreen()
        {
            InitializeComponent();
            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion



        #region Private Methods

       

      

        private void EnableDisableExportButton()
        {
            try
            {
                if (txtRefNumber.Text != string.Empty) 
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

     
        private void ReportCompleted()
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            btnReport.Content = "Report";
            btnReport.IsEnabled = true;
            btnClose.IsEnabled = true;            
        }

        private void Report()
        {
            try
            {
                SetCursor(Cursors.Wait);

                StringBuilder strQuery = new StringBuilder();
                strQuery.Append("SELECT INCampaign.Name, INCampaign.Code, INCampaign.ID, INImport.RefNo, INImport.ID [ImportID] FROM INImport ");
                strQuery.Append("INNER JOIN INCampaign ON INImport.FKINCampaignID = INCampaign.ID ");
                strQuery.Append("WHERE (INImport.RefNo = '" + txtRefNumber.Text + "')");

                DataTable dtCampaign = Methods.GetTableData(strQuery.ToString());

                long campaignID = 0;
                string campaignName = string.Empty;
                string campaignCode = string.Empty;
                long? importID = 0;

                if (dtCampaign.Rows.Count > 1)
                {
                    //bring popup to select campaign
                    SelectLeadCampaignScreen selectLeadCampaignScreen = new SelectLeadCampaignScreen(txtRefNumber.Text);

                    if (ShowDialog(selectLeadCampaignScreen, new INDialogWindow(selectLeadCampaignScreen)) == true)
                    {
                        importID = selectLeadCampaignScreen.ImportID;

                        StringBuilder strQuery2 = new StringBuilder();
                        strQuery2.Append("SELECT INCampaign.Name, INCampaign.Code, INCampaign.ID, INImport.RefNo,INImport.ID as ImportID FROM INImport ");
                        strQuery2.Append("INNER JOIN INCampaign ON INImport.FKINCampaignID = INCampaign.ID ");
                        strQuery2.Append("WHERE INImport.ID = '" + importID + "'");

                        dtCampaign = Methods.GetTableData(strQuery2.ToString());

                        campaignID = long.Parse(dtCampaign.Rows[0]["ID"].ToString());
                        campaignName = dtCampaign.Rows[0]["Name"].ToString();
                        campaignCode = dtCampaign.Rows[0]["Code"].ToString();
                    }
                }
                else if (dtCampaign.Rows.Count == 1)
                {
                    importID = long.Parse(dtCampaign.Rows[0]["ImportID"].ToString());
                    campaignID = long.Parse(dtCampaign.Rows[0]["ID"].ToString());
                    campaignName = dtCampaign.Rows[0]["Name"].ToString();
                    campaignCode = dtCampaign.Rows[0]["Code"].ToString();
                }

                if (dtCampaign.Rows.Count > 0)
                {
                    # region Setup excel documents

                    Workbook wbTemplate;
                    Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                    string filePathAndName = GlobalSettings.UserFolder + "Lead Activity Report (" + dtCampaign.Rows[0]["RefNo"].ToString() + ") ~ " + DateTime.Now.Millisecond + ".xlsx";

                    Uri uri = new Uri("/Templates/ReportTemplateLeadActivity.xlsx", UriKind.Relative);
                    StreamResourceInfo info = Application.GetResourceStream(uri);
                    if (info != null)
                    {
                        wbTemplate = Workbook.Load(info.Stream, true);
                    }
                    else
                    {
                        return;
                    }

                    //Add Worksheets
                    Worksheet wsTemplateLead = wbTemplate.Worksheets["Lead"];
                    Worksheet wsReportLead = wbReport.Worksheets.Add("Lead");

                    wsReportLead.PrintOptions.PaperSize = PaperSize.A4;
                    wsReportLead.PrintOptions.Orientation = Orientation.Portrait;

                    //Policy Details
                    //Worksheet wsTemplatePolicyDetails = wbTemplate.Worksheets["Policy Details"];
                    //Worksheet wsReportPolicyDetails = wbReport.Worksheets.Add("Policy Details");

                    //wsReportPolicyDetails.PrintOptions.PaperSize = PaperSize.A4;
                    //wsReportPolicyDetails.PrintOptions.Orientation = Orientation.Portrait;

                    //Bank Details
                    Worksheet wsTemplateBankDetails = wbTemplate.Worksheets["Bank Details"];
                    Worksheet wsReportBankDetails = wbReport.Worksheets.Add("Bank Details");

                    wsReportBankDetails.PrintOptions.PaperSize = PaperSize.A4;
                    wsReportBankDetails.PrintOptions.Orientation = Orientation.Portrait;

                    //Mercantile
                    Worksheet wsTemplateMercantile = wbTemplate.Worksheets["Mercantile"];
                    Worksheet wsReportMercantile = wbReport.Worksheets.Add("Mercantile (Piggy Clicks)");

                    wsReportMercantile.PrintOptions.PaperSize = PaperSize.A4;
                    wsReportMercantile.PrintOptions.Orientation = Orientation.Portrait;

                    //Original
                    Worksheet wsTemplate = wbTemplate.Worksheets["Other"];
                    Worksheet wsReport = wbReport.Worksheets.Add("Other");

                    wsReport.PrintOptions.PaperSize = PaperSize.A4;
                    wsReport.PrintOptions.Orientation = Orientation.Portrait;

                    #endregion

                    #region Get report data from database

                    //Common Data
                    strQuery = new StringBuilder();
                    strQuery.Append("SELECT INCampaign.Code [Campaign], INBatch.Code [Batch], INImport.RefNo [RefNo], INImport.ID [ImportID] FROM INImport ");
                    strQuery.Append("JOIN INBatch ON INImport.FKINBatchID = INBatch.ID ");
                    strQuery.Append("JOIN INCampaign ON INBatch.FKINCampaignID = INCampaign.ID ");
                    strQuery.Append("WHERE (INImport.ID = '" + importID + "')");

                    DataTable dtReportData = Methods.GetTableData(strQuery.ToString());

                    //DataTable dtLeadAllocationData;

                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@CampaignID", campaignID);
                    parameters[1] = new SqlParameter("@ImportID", importID);
                    DataSet dsLeadAllocationData = Methods.ExecuteStoredProcedure("spINReportLeadActivity", parameters);

                    SqlParameter[] parameters2 = new SqlParameter[1];
                    parameters2[0] = new SqlParameter("@ImportID", importID);
                    DataSet dsLeadActivityLead = Methods.ExecuteStoredProcedure2("spINReportLeadActivityLead", parameters2, IsolationLevel.ReadUncommitted, 300);

                    SqlParameter[] parameters3 = new SqlParameter[1];
                    parameters3[0] = new SqlParameter("@ImportID", importID);
                    DataSet dsLeadActivityBankDetails = Methods.ExecuteStoredProcedure2("spINReportLeadActivityBankDetails", parameters3, IsolationLevel.ReadUncommitted, 300);

                    SqlParameter[] parameters4 = new SqlParameter[1];
                    parameters4[0] = new SqlParameter("@ImportID", importID);
                    DataSet dsLeadActivityMercantile = Methods.ExecuteStoredProcedure2("spINReportLeadActivityMercantile", parameters4, IsolationLevel.ReadUncommitted, 300);

                    SqlParameter[] parameters5 = new SqlParameter[1];
                    parameters5[0] = new SqlParameter("@ImportID", importID);
                    DataSet dsLeadActivityPolicyDetails = Methods.ExecuteStoredProcedure2("spINReportLeadActivityPolicyDetails", parameters5, IsolationLevel.ReadUncommitted, 300);

                    #endregion
                    Methods.CopyExcelRegion(wsTemplate, 0, 0, 1, 19, wsReport, 0, 0);
                    int rowIndex = 2;                                   
                    DataTable activities = dsLeadAllocationData.Tables[0];
                    DataTable activities2 = dsLeadAllocationData.Tables[1];
                    DataTable activities3 = dsLeadAllocationData.Tables[2];
                    int currentRow = 0;
                    int lastRow = activities.Rows.Count;
                    if (activities.Rows.Count > 0)
                    {
                        foreach (DataRow activityy in activities.Rows)
                        {
                            DataRow activity = activityy;
                            wsReport.Workbook.NamedReferences.Clear();
                            Methods.CopyExcelRegion(wsTemplate, rowIndex - 1, 0, 1, 19, wsReport, rowIndex - 1, 0);

                            //get activity
                            string activityText = string.Empty;
                            DataRow previousActivity;
                            if (currentRow + 1 != lastRow)
                            {//get user 
                                wsReport.GetCell("A" + rowIndex.ToString()).Value = activity["UserID"].ToString();
                                wsReport.GetCell("B" + rowIndex.ToString()).Value = activity["UserName"].ToString();
                                wsReport.GetCell("C" + rowIndex.ToString()).Value = activity["UserLastName"].ToString();
                                previousActivity = activities.Rows[currentRow + 1];
                            }
                            else
                            {
                                previousActivity = activities2.Rows[0];
                                // activity = activities2.Rows[0];
                                //get user 
                                //wsReport.GetCell("A" + rowIndex.ToString()).Value = activities2.Rows[0]["UserID"].ToString();
                                //wsReport.GetCell("B" + rowIndex.ToString()).Value = activities2.Rows[0]["UserName"].ToString();
                                //wsReport.GetCell("C" + rowIndex.ToString()).Value = activities2.Rows[0]["UserLastName"].ToString();
                                //rowIndex++;
                                wsReport.GetCell("A" + rowIndex.ToString()).Value = activities3.Rows[0]["UserID"].ToString();
                                wsReport.GetCell("B" + rowIndex.ToString()).Value = activities3.Rows[0]["UserName"].ToString();
                                wsReport.GetCell("C" + rowIndex.ToString()).Value = activities3.Rows[0]["UserLastName"].ToString();
                            }
                            bool first = true;
                            int colIndex = 0;
                            foreach (DataColumn col in activities.Columns)
                            {
                                string originalField = previousActivity[colIndex].ToString();
                                string currentField = activity[colIndex].ToString();
                                colIndex++;
                                string colName = col.ColumnName;
                                if (colName == "LeadStampDate" ) //|| colName == "ImportStampDate"
                                {
                                    colName = "Lead Saved";// colName = string.Empty;
                                }
                                if (colName == "FKUserID")
                                {
                                    colName = "Lead Assigned";
                                }
                                if (colName == "FKINLeadStatusID")
                                {
                                    colName = "Status Change";
                                }
                                if (colName == "AllocationDate")
                                {
                                    colName = "Lead Allocate";
                                }
                                if (colName == "FKINDeclineReasonID")
                                {
                                    colName = "Decline Reason";
                                }
                                if (colName == "FKSaleCallRefUserID")
                                {
                                    colName = string.Empty;
                                }
                                if (colName == "FKGenderID")
                                {
                                    colName = "Gender Change";
                                }
                                if (colName == "FKINTitleID")
                                {
                                    colName = "Title Change";
                                }
                                if (colName == "StampUserID")
                                {
                                    colName = string.Empty;//this will always change when someone else works on lead
                                }
                                if (colName == "FKSaleTelNumberTypeID")
                                {
                                    colName = "Sale Tel Number type change";
                                }
                                if (colName == "ImportStampUserID")
                                {
                                    colName = string.Empty;
                                }
                                if (colName == "IsPrinted")
                                {
                                    colName = string.Empty;
                                }
                                if (colName == "FKClosureID")
                                {
                                    colName = "Closure Open";
                                }
                                if (colName == "UserName" || colName == "UserLastName")
                                {
                                    colName = string.Empty;
                                }
                                if (colName == "DateBatched")
                                {
                                    colName = "Batch Export";
                                }
                                if (colName == "FKClosureID")
                                {
                                    if (originalField == "True" && currentField == "False")
                                    {
                                        colName = "Closure check";
                                    }
                                    if (currentField == "True" && originalField == "False")
                                    {
                                        colName = "Closure Uncheck";
                                    }
                                }
                                if (originalField != currentField)
                                {
                                    if (colName != "UserID" && colName != "ImportStampDate")
                                    {
                                        if (first)
                                        {
                                            if (colName != string.Empty)
                                            {
                                                first = false;
                                                activityText = colName;
                                            }
                                        }
                                        else
                                        {
                                            if (colName != string.Empty)
                                            {
                                                activityText = activityText + "," + colName;
                                            }
                                        }
                                    }
                                        
                                }
                            }


                            wsReport.GetCell("D" + rowIndex.ToString()).Value = activityText;
                            wsReport.GetCell("E" + rowIndex.ToString()).Value = activity["LeadStampDate"].ToString();
                            wsReport.GetCell("F" + rowIndex.ToString()).Value = activity["CampaignCode"].ToString();
                            wsReport.GetCell("G" + rowIndex.ToString()).Value = activity["RefNo"].ToString();
                            rowIndex++;
                            if (currentRow < lastRow)
                            {
                                currentRow++;
                            }
                        }
                        
                    }
                    //else
                    //{
                    //    ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the reference " + txtRefNumber.Text, "No Data", ShowMessageType.Information);
                    //}                   
                    
                    #region Lead

                    wsReport.Workbook.NamedReferences.Clear();
                    DataTable dtActivityLead = dsLeadActivityLead.Tables[0];

                    Methods.CopyExcelRegion(wsTemplateLead, 0, 0, 7, 8, wsReportLead, 0, 0);

                    wsReportLead.GetCell("ImportID").Value = dtReportData.Rows[0]["ImportID"];
                    wsReportLead.GetCell("RefNo").Value = dtReportData.Rows[0]["RefNo"];
                    wsReportLead.GetCell("Campaign").Value = dtReportData.Rows[0]["Campaign"];
                    wsReportLead.GetCell("Batch").Value = dtReportData.Rows[0]["Batch"];

                    if (dtActivityLead.Rows.Count > 0)
                    {
                        rowIndex = 8;
                        int count =0;
                        foreach (DataRow dr in dtActivityLead.AsEnumerable())
                        {
                            wsReportLead.GetCell("SalesAgent").Value = dr["SalesAgent"];
                            wsReportLead.GetCell("DateOfSale").Value = dr["DateOfSale"];
                            wsReportLead.GetCell("LeadStatusID").Value = dr["LeadStatusID"];
                            wsReportLead.GetCell("LeadStatus").Value = dr["LeadStatus"];
                            wsReportLead.GetCell("CFReason").Value = dr["CFReason"];
                            wsReportLead.GetCell("DateChanged").Value = dr["DateChanged"];
                            wsReportLead.GetCell("UserName").Value = dr["UserName"];
                            wsReportLead.GetCell("Row").Value = dr["RowNumber"];

                            wsReport.Workbook.NamedReferences.Clear();

                            count++;
                            if (count < dtActivityLead.Rows.Count)
                            {
                                Methods.CopyExcelRegion(wsTemplateLead, 7, 1, 1, 8, wsReportLead, rowIndex, 1);
                                rowIndex++;
                            }
                        }
                    }

                    #endregion

                    #region Bank Details

                    DataTable dtBankDetails = dsLeadActivityBankDetails.Tables[0];

                    Methods.CopyExcelRegion(wsTemplateBankDetails, 0, 0, 7, 7, wsReportBankDetails, 0, 0);

                    wsReportBankDetails.GetCell("BDImportID").Value = dtReportData.Rows[0]["ImportID"];
                    wsReportBankDetails.GetCell("BDRefNo").Value = dtReportData.Rows[0]["RefNo"];
                    wsReportBankDetails.GetCell("BDCampaign").Value = dtReportData.Rows[0]["Campaign"];
                    wsReportBankDetails.GetCell("BDBatch").Value = dtReportData.Rows[0]["Batch"];

                    if (dtBankDetails.Rows.Count > 0)
                    {
                        rowIndex = 8;
                        int count = 0;
                        foreach (DataRow dr in dtBankDetails.AsEnumerable())
                        {
                            wsReportBankDetails.GetCell("BDBank").Value = dr["Bank"];
                            wsReportBankDetails.GetCell("BDBankBranch").Value = dr["BankBranch"];
                            wsReportBankDetails.GetCell("BDAccountNo").Value = dr["AccountNo"];
                            wsReportBankDetails.GetCell("BDAccNumCheckMsg").Value = dr["AccNumCheckMsg"];
                            wsReportBankDetails.GetCell("BDDateChanged").Value = dr["DateChanged"];
                            wsReportBankDetails.GetCell("BDUserName").Value = dr["UserName"];
                            wsReportBankDetails.GetCell("BDRow").Value = dr["RowNumber"];

                            wsReportBankDetails.Workbook.NamedReferences.Clear();

                            count++;
                            if (count < dtBankDetails.Rows.Count)
                            {
                                Methods.CopyExcelRegion(wsTemplateBankDetails, 7, 1, 1, 7, wsReportBankDetails, rowIndex, 1);
                                rowIndex++;
                            }
                        }
                    }

                    #endregion

                    #region Mercantile

                    DataTable dtMercantile = dsLeadActivityMercantile.Tables[0];

                    Methods.CopyExcelRegion(wsTemplateMercantile, 0, 0, 7, 7, wsReportMercantile, 0, 0);

                    wsReportMercantile.GetCell("MImportID").Value = dtReportData.Rows[0]["ImportID"];
                    wsReportMercantile.GetCell("MRefNo").Value = dtReportData.Rows[0]["RefNo"];
                    wsReportMercantile.GetCell("MCampaign").Value = dtReportData.Rows[0]["Campaign"];
                    wsReportMercantile.GetCell("MBatch").Value = dtReportData.Rows[0]["Batch"];

                    if (dtMercantile.Rows.Count > 0)
                    {
                        rowIndex = 8;
                        int count = 0;
                        foreach (DataRow dr in dtMercantile.AsEnumerable())
                        {
                            wsReportMercantile.GetCell("MBank").Value = dr["Bank"];
                            wsReportMercantile.GetCell("MBankBranch").Value = dr["BankBranch"];
                            wsReportMercantile.GetCell("MAccountNo").Value = dr["AccountNo"];
                            wsReportMercantile.GetCell("MAccNumCheckMsgFull").Value = dr["AccNumCheckMsgFull"];
                            wsReportMercantile.GetCell("MDateChanged").Value = dr["DateChanged"];
                            wsReportMercantile.GetCell("MUserName").Value = dr["UserName"];
                            wsReportMercantile.GetCell("MRow").Value = dr["RowNumber"];

                            wsReportMercantile.Workbook.NamedReferences.Clear();

                            count++;
                            if (count < dtMercantile.Rows.Count)
                            {
                                Methods.CopyExcelRegion(wsTemplateMercantile, 7, 1, 1, 7, wsReportMercantile, rowIndex, 1);
                                rowIndex++;
                            }
                        }
                    }

                    #endregion

                    #region Policy

                    //DataTable dtActivityPolicyDetails = dsLeadActivityPolicyDetails.Tables[0];

                    //Methods.CopyExcelRegion(wsTemplatePolicyDetails, 0, 0, 7, 9, wsReportPolicyDetails, 0, 0);

                    //wsReportPolicyDetails.GetCell("PDImportID").Value = dtReportData.Rows[0]["ImportID"];
                    //wsReportPolicyDetails.GetCell("PDRefNo").Value = dtReportData.Rows[0]["RefNo"];
                    //wsReportPolicyDetails.GetCell("PDCampaign").Value = dtReportData.Rows[0]["Campaign"];
                    //wsReportPolicyDetails.GetCell("PDBatch").Value = dtReportData.Rows[0]["Batch"];

                    //if (dtActivityPolicyDetails.Rows.Count > 0)
                    //{
                    //    rowIndex = 8;
                    //    int count = 0;
                    //    foreach (DataRow dr in dtActivityPolicyDetails.AsEnumerable())
                    //    {
                    //        wsReportPolicyDetails.GetCell("PDOptionCode").Value = dr["OptionCode"];
                    //        wsReportPolicyDetails.GetCell("PDTotalPremium").Value = dr["TotalPremium"];

                    //        wsReportPolicyDetails.GetCell("PDBumpUpAmount").Value = dr["BumpUpAmount"];
                    //        wsReportPolicyDetails.GetCell("PDReducedPremiumAmount").Value = dr["ReducedPremiumAmount"];

                    //        wsReportPolicyDetails.GetCell("PDDateChanged").Value = dr["DateChanged"];
                    //        wsReportPolicyDetails.GetCell("PDUserName").Value = dr["UserName"];
                    //        wsReportPolicyDetails.GetCell("PDRow").Value = dr["RowNumber"];

                    //        wsReportPolicyDetails.Workbook.NamedReferences.Clear();

                    //        count++;
                    //        if (count < dtActivityPolicyDetails.Rows.Count)
                    //        {
                    //            Methods.CopyExcelRegion(wsTemplatePolicyDetails, 7, 1, 1, 9, wsReportPolicyDetails, rowIndex, 1);
                    //            rowIndex++;
                    //        }
                    //    }
                    //}

                    #endregion

                    if (wbReport.Worksheets.Count > 0)
                    {
                        //Save excel document
                        wbReport.Save(filePathAndName);

                        //Display excel document
                        Process.Start(filePathAndName);
                    }
                }
                else
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the reference " + txtRefNumber.Text, "No Data", ShowMessageType.Information);
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
                Report();
                dispatcherTimer1.Start();
                ReportCompleted();
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
		

        #endregion



        private void txtRefNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableDisableExportButton();
        }

    }

}
