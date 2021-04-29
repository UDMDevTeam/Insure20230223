using Embriant.Framework;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using Infragistics.Documents.Excel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Resources;
using System.Windows.Threading;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using Orientation = Infragistics.Documents.Excel.Orientation;

namespace UDM.Insurance.Interface.Screens
{
    public partial class PrintSummaryScreen
    {

        #region Private Members
        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        private int _PrintTime;
        private readonly DispatcherTimer _TimerPrint = new DispatcherTimer();
        #endregion

        #region Constructors

        public PrintSummaryScreen()
        {
            InitializeComponent();

            LoadSummaryData();

            _TimerPrint.Tick += TimerPrint;
            _TimerPrint.Interval = new TimeSpan(0, 0, 1);

            #if TESTBUILD
                TestControl.Visibility = Visibility.Visible;
            #else
                TestControl.Visibility = Visibility.Collapsed;
            #endif
        }

        #endregion

        #region Private Methods

        private void LoadSummaryData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataSet ds = Methods.ExecuteStoredProcedure("spINGetAgentsWithLeadsToPrint", null);

                var column = new DataColumn("Select", typeof(bool)) { DefaultValue = false };
                ds.Tables[0].Columns.Add(column);

                xdgPrintLeads.DataSource = ds.Tables[0].DefaultView;
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

        private void TimerPrint(object sender, EventArgs e)
        {
            try
            {
                _PrintTime++;
                btnPrint.Content = TimeSpan.FromSeconds(_PrintTime).ToString();
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

                if (xdgPrintLeads.DataSource != null)
                {
                    foreach (DataRow dr in ((DataView)xdgPrintLeads.DataSource).Table.Rows)
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

        //set excel upgrade campaigns print and display options
        public static void SetExcelUpgradeWSOptions(Worksheet ws, Orientation orientation)
        {
            ws.DisplayOptions.ShowGridlines = false;
            ws.PrintOptions.PaperSize = PaperSize.A4;
            ws.PrintOptions.Orientation = orientation;
            ws.PrintOptions.LeftMargin = 0.4d;
            ws.PrintOptions.TopMargin = 0.8d;
            ws.PrintOptions.RightMargin = 0.4d;
            ws.PrintOptions.BottomMargin = 0.8d;
            ws.PrintOptions.CenterHorizontally = true;
            ws.DisplayOptions.MagnificationInPageLayoutView = 100;
            ws.DisplayOptions.View = WorksheetView.PageLayout;
        }

        public void CopyWorkSheetOptionsFromTemplate(Worksheet wsTemplate, Worksheet wsTarget)
        {
            wsTarget.DisplayOptions.ShowGridlines = false;
            wsTarget.PrintOptions.PaperSize = wsTemplate.PrintOptions.PaperSize; //PaperSize.A4;
            wsTarget.PrintOptions.Orientation = wsTemplate.PrintOptions.Orientation;  //Orientation.Landscape;
            wsTarget.PrintOptions.LeftMargin = wsTemplate.PrintOptions.LeftMargin;
            wsTarget.PrintOptions.TopMargin = wsTemplate.PrintOptions.TopMargin;
            wsTarget.PrintOptions.RightMargin = wsTemplate.PrintOptions.RightMargin;
            wsTarget.PrintOptions.BottomMargin = wsTemplate.PrintOptions.BottomMargin;
            wsTarget.PrintOptions.FooterMargin = wsTemplate.PrintOptions.FooterMargin;
            wsTarget.PrintOptions.HeaderMargin = wsTemplate.PrintOptions.HeaderMargin;
            wsTarget.PrintOptions.ScalingType = wsTemplate.PrintOptions.ScalingType;
            wsTarget.PrintOptions.ScalingFactor = wsTemplate.PrintOptions.ScalingFactor;

            wsTarget.PrintOptions.CenterHorizontally = true;
            wsTarget.DisplayOptions.MagnificationInPageLayoutView = 100;
            wsTarget.DisplayOptions.View = WorksheetView.PageLayout;
            wsTarget.DefaultRowHeight = 240;

        }

        private void PrintLeads()
        {
            try
            {

                Database.BeginTransaction(null, IsolationLevel.Snapshot);

                #region Initialize & check campaign

                DataTable dt = ((DataView) xdgPrintLeads.DataSource).Table;
                DataTable dtSelected = dt.Select("Select = true", "SalesAgent ASC").CopyToDataTable();
                DataTable dtCampaigns = Methods.GroupBy("CampaignCode", "CampaignCode", dtSelected);
                string[] groupByBatches = new string[] { "CampaignCode", "BatchCode" };
                DataTable dtCampaignBatches = Methods.GroupBy(groupByBatches, "BatchCode", dtSelected);

                
                if (dtCampaigns.Rows.Count > 1)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Multiple Campaigns Selected.\n Select leads to print from the same campaign.", "Lead Print Error", ShowMessageType.Error);
                    return;
                }

                INCampaign campaign = INCampaignMapper.SearchOne(null, null, null, dtSelected.Rows[0]["CampaignCode"].ToString(), null, null, null, null, null, null, null, null);

                #if !TRAININGBUILD
                if (campaign.Name.CaseInsensitiveContains("Upgrade"))
                {
                    if (dtCampaignBatches.Rows.Count > 1)
                    {
                        ShowMessageBox(new INMessageBoxWindow1(), "Multiple Batches Selected for Upgrade Campaign.\n Select leads to print from the same batch.", "Lead Print Error", ShowMessageType.Error);
                        return;
                    }
                }
                #endif

                INBatch batch = INBatchMapper.SearchOne(campaign.ID, dtSelected.Rows[0]["BatchCode"].ToString(), null, null, null, null, null, null, null, null);
                DateTime nextMonday = Methods.NextWeekDay(DateTime.Now, DayOfWeek.Monday);

                if ((dtSelected.Rows[0]["CampaignCode"].ToString().Trim() == "PMCBE") ||
                    (dtSelected.Rows[0]["CampaignCode"].ToString().Trim() == "PLCBE") ||
                    (dtSelected.Rows[0]["CampaignCode"].ToString().Trim() == "PLMMBE") ||
                    //(dtSelected.Rows[0]["CampaignCode"].ToString().Trim() == "PLBMMBElite") ||
                    //(dtSelected.Rows[0]["CampaignCode"].ToString().Trim() == "PLFDB") ||
                    (dtSelected.Rows[0]["CampaignCode"].ToString().Trim() == "IGPLFDB") ||
                    (dtSelected.Rows[0]["CampaignCode"].ToString().Trim() == "PLCBEC") ||
                    (dtSelected.Rows[0]["CampaignCode"].ToString().Trim() == "PLCBER") ||
                    (dtSelected.Rows[0]["CampaignCode"].ToString().Trim() == "PLMCB") ||
                    (dtSelected.Rows[0]["CampaignCode"].ToString().Trim() == "PLMMCB") ||
                    (dtSelected.Rows[0]["CampaignCode"].ToString().Trim() == "PLFDCB") ||
                    (dtSelected.Rows[0]["CampaignCode"].ToString().EndsWith("Min")))
                {


                    //if (dtSelected.Rows[0]["CampaignCode"].ToString().Trim() == "PLFDB" || dtSelected.Rows[0]["CampaignCode"].ToString().Trim() == "IGPLFDB")
                    //{
                    //    PrintSheMaccBaseLeads(dtSelected);
                    //}
                    //else
                    //{
                        PrintEliteLeads(dtSelected);
                    //}
                    return;
                }

                //else if (dtSelected.Rows[0]["CampaignCode"].ToString().Trim() == "PLFDB")
                //{
                //    PrintBaseLeadsRedeemedGifts(dtSelected);
                //    return;
                //}

                DataTable dtBatches = Methods.GroupBy("BatchCode", "BatchCode", dtSelected);

                string LeadBookName = dtCampaigns.Rows[0][0].ToString().Trim() + " Batch ";
                foreach (DataRow dr in dtBatches.AsEnumerable())
                {
                    LeadBookName = LeadBookName + dr[0];
                    LeadBookName = LeadBookName + ", ";
                }

                LeadBookName = LeadBookName.TrimEnd(',', ' ') + " (#DATE#)";
                string LeadBookFileName = LeadBookName.TrimEnd(')') + DateTime.Now.ToString(" HH.mm.ss") + ")";
                LeadBookFileName = LeadBookFileName.Replace("#DATE#", DateTime.Now.ToString("yyyy-MM-dd"));

                //leadBookName = String.Format("{0} Batch {1} (#DATE#)", campaignCode, batches/*, DateTime.Now.ToString("yyyy-MM-dd")*/);
                //leadBookFileName = String.Format("{0} {1})", leadBookName.TrimEnd(')'), DateTime.Now.ToString("HH.mm.ss")).Replace("#DATE#", DateTime.Now.ToString("yyyy-MM-dd"));

                #endregion Initialize & check campaign

                #region setup excel document
                Workbook wbTemplate;
                Workbook wbPrint = new Workbook(WorkbookFormat.Excel2007);

                Uri uri;
                int TemplateLines;
                int leadsPerPage;
                int leadColumnWidth;
                int coverColumns;
                int coverRows;

                int upgradeCoverSheetColumnIndex;
                int upgradeCoverSheetRowIndex;
                int verticalSpacingBetweenLeads;

                int leadTemplateRowIndex = 35; // Give it a default value

                bool IsUpgradeCampaign = false;

                switch (campaign.FKINCampaignGroupID)
                {
                    #region Rejuvenation, Defrost, ReDefrost & Reactivation

                    case (long)lkpINCampaignGroup.Rejuvenation:
                    case (long)lkpINCampaignGroup.Defrosted:
                    case (long)lkpINCampaignGroup.ReDefrost:
                    case (long)lkpINCampaignGroup.Reactivation:
                        uri = new Uri("/Templates/PrintTemplate.xlsx", UriKind.Relative);
                        TemplateLines = 5;
                        leadsPerPage = 4;
                        leadColumnWidth = 16;
                        coverColumns = 15;
                        coverRows = 29;

                        upgradeCoverSheetColumnIndex = 6;
                        upgradeCoverSheetRowIndex = 0;
                        verticalSpacingBetweenLeads = 1;

                        break;

                    case (long)lkpINCampaignGroup.Resurrection:
                        uri = new Uri("/Templates/PrintTemplateResurrection.xlsx", UriKind.Relative);
                        TemplateLines = 5;
                        leadsPerPage = 4;
                        leadColumnWidth = 16;
                        coverColumns = 15;
                        coverRows = 29;

                        upgradeCoverSheetColumnIndex = 6;
                        upgradeCoverSheetRowIndex = 0;
                        verticalSpacingBetweenLeads = 1;

                        break;

                    #endregion Rejuvenation, Defrost, ReDefrost & Reactivation

                    #region Upgrades

                    case (long)lkpINCampaignGroup.Upgrade1:
                    case (long)lkpINCampaignGroup.Upgrade2:
                    case (long)lkpINCampaignGroup.Upgrade3:
                    case (long)lkpINCampaignGroup.Upgrade4:
                    case (long)lkpINCampaignGroup.Upgrade5:
                    case (long)lkpINCampaignGroup.Upgrade6:
                    case (long)lkpINCampaignGroup.Upgrade7:
                    case (long)lkpINCampaignGroup.Upgrade8:
                    case (long)lkpINCampaignGroup.Upgrade9:
                    case (long)lkpINCampaignGroup.Upgrade10:
                    case (long)lkpINCampaignGroup.Upgrade11:
                    case (long)lkpINCampaignGroup.Upgrade12:
                    case (long)lkpINCampaignGroup.Upgrade13:
                    case (long)lkpINCampaignGroup.DoubleUpgrade1:
                    case (long)lkpINCampaignGroup.DoubleUpgrade2:
                    case (long)lkpINCampaignGroup.DoubleUpgrade3:
                    case (long)lkpINCampaignGroup.DoubleUpgrade4:
                    case (long)lkpINCampaignGroup.DoubleUpgrade5:
                    case (long)lkpINCampaignGroup.DoubleUpgrade6:
                    case (long)lkpINCampaignGroup.DoubleUpgrade7:
                    case (long)lkpINCampaignGroup.DoubleUpgrade8:
                    case (long)lkpINCampaignGroup.DoubleUpgrade9:
                    case (long)lkpINCampaignGroup.DoubleUpgrade10:
                    case (long)lkpINCampaignGroup.DoubleUpgrade11:
                    case (long)lkpINCampaignGroup.DoubleUpgrade12:
                    case (long)lkpINCampaignGroup.DoubleUpgrade13:
                    case (long)lkpINCampaignGroup.DoubleUpgrade14:
                        if (batch.Code.Contains("_R"))
                        {
                            uri = new Uri("/Templates/PrintTemplateUpgradeRedeemed2.xlsx", UriKind.Relative);
                            TemplateLines = 15;//13;
                            leadsPerPage = 3;
                            leadColumnWidth = 54;
                            coverColumns = 58;
                            coverRows = 34; //51;

                            if (uri.ToString() == "/Templates/PrintTemplateUpgradeRedeemed2.xlsx")
                            {
                                coverRows = 39;
                            }

                            upgradeCoverSheetColumnIndex = 26;
                            upgradeCoverSheetRowIndex = 2;
                            verticalSpacingBetweenLeads = 1;

                            leadTemplateRowIndex = 40;
                        }
                        else if (batch.Code.Contains("_NR"))
                        {
                            switch (campaign.FKINCampaignTypeID)
                            {
                                case (long)lkpINCampaignType.Cancer:
                                case (long)lkpINCampaignType.CancerFuneral:
                                case (long)lkpINCampaignType.IGCancer:
                                case (long)lkpINCampaignType.TermCancer:
                                    //case (long)lkpINCampaignType.MaccCancer:
                                    //case (long)lkpINCampaignType.MaccMillionCancer:
                                    if (DateTime.Now >= Convert.ToDateTime("2018-09-12 00:00:00"))
                                    {
                                        uri = new Uri("/Templates/PrintTemplateUpgradeNonRedeemed2cWLite.xlsx", UriKind.Relative);
                                    }
                                    else
                                    {
                                        uri = new Uri("/Templates/PrintTemplateUpgradeNonRedeemed2c.xlsx", UriKind.Relative);
                                    }
                                    break;

                                case (long)lkpINCampaignType.Macc:
                                case (long)lkpINCampaignType.MaccFuneral:
                                case (long)lkpINCampaignType.MaccMillion:
                                case (long)lkpINCampaignType.BlackMacc:
                                case (long)lkpINCampaignType.BlackMaccMillion:
                                    if (DateTime.Now >= Convert.ToDateTime("2018-09-12 00:00:00"))
                                    {
                                        uri = new Uri("/Templates/PrintTemplateUpgradeNonRedeemed2mWLite.xlsx", UriKind.Relative);
                                    }
                                    else
                                    {
                                        uri = new Uri("/Templates/PrintTemplateUpgradeNonRedeemed2m.xlsx", UriKind.Relative);
                                    }
                                    break;

                                default:
                                    //uri = new Uri("");
                                    return;
                            }

                            TemplateLines = 15;//13;
                            leadsPerPage = 3;
                            leadColumnWidth = 54;
                            coverColumns = 58;
                            coverRows = 34; //51;

                            if (uri.ToString() == "/Templates/PrintTemplateUpgradeNonRedeemed2c.xlsx" ||
                                uri.ToString() == "/Templates/PrintTemplateUpgradeNonRedeemed2cWLite.xlsx" ||
                                uri.ToString() == "/Templates/PrintTemplateUpgradeNonRedeemed2m.xlsx" ||
                                uri.ToString() == "/Templates/PrintTemplateUpgradeNonRedeemed2mWLite.xlsx")
                            {
                                coverRows = 39;
                            }

                            upgradeCoverSheetColumnIndex = 26;
                            upgradeCoverSheetRowIndex = 2;
                            verticalSpacingBetweenLeads = 1;

                            leadTemplateRowIndex = 40;
                        }
                        else if (campaign.Code == "PLDMM5U")
                        {
                            uri = new Uri("/Templates/PrintTemplateUpgrade4.xlsx", UriKind.Relative);
                            TemplateLines = 15;
                            leadsPerPage = 3;
                            leadColumnWidth = 54;
                            coverColumns = 58;
                            coverRows = 39;

                            upgradeCoverSheetColumnIndex = 26;
                            upgradeCoverSheetRowIndex = 2;
                            verticalSpacingBetweenLeads = 1;

                            leadTemplateRowIndex = 40;
                        }
                        else if (campaign.Code == "PLDMM6U")
                        {
                            uri = new Uri("/Templates/PrintTemplateUpgrade5.xlsx", UriKind.Relative);
                            TemplateLines = 15;//13;
                            leadsPerPage = 3;
                            leadColumnWidth = 54;
                            coverColumns = 58;
                            coverRows = 34; //51;

                            if (uri.ToString() == "/Templates/PrintTemplateUpgrade5.xlsx")
                            {
                                coverRows = 39;
                            }

                            upgradeCoverSheetColumnIndex = 26;
                            upgradeCoverSheetRowIndex = 2;
                            verticalSpacingBetweenLeads = 1;

                            leadTemplateRowIndex = 40;
                        }
                        else if (campaign.Code == "PLDMM7U")
                        {
                            uri = new Uri("/Templates/PrintTemplateUpgrade5.xlsx", UriKind.Relative);
                            TemplateLines = 15;//13;
                            leadsPerPage = 3;
                            leadColumnWidth = 54;
                            coverColumns = 58;
                            coverRows = 34; //51;

                            if (uri.ToString() == "/Templates/PrintTemplateUpgrade5.xlsx")
                            {
                                coverRows = 39;
                            }

                            upgradeCoverSheetColumnIndex = 26;
                            upgradeCoverSheetRowIndex = 2;
                            verticalSpacingBetweenLeads = 1;

                            leadTemplateRowIndex = 40;
                        }
                        else
                        {
                            uri = new Uri("/Templates/PrintTemplateUpgrade3.xlsx", UriKind.Relative);
                            TemplateLines = 15;//13;
                            leadsPerPage = 3;
                            leadColumnWidth = 54;
                            coverColumns = 58;
                            coverRows = 34; //51;

                            if (uri.ToString() == "/Templates/PrintTemplateUpgrade3.xlsx")
                            {
                                coverRows = 39;
                            }

                            upgradeCoverSheetColumnIndex = 26;
                            upgradeCoverSheetRowIndex = 2;
                            verticalSpacingBetweenLeads = 1;

                            leadTemplateRowIndex = 40;
                        }
                        
                        IsUpgradeCampaign = true;

                        
                        break;

                    #endregion Upgrades

                    #region Other non-upgrade campaigns

                    default:
                        if ((dtSelected.Rows[0]["CampaignCode"].ToString().Trim() == "PLFDBPE") && 
                             dtSelected.Rows[0]["BatchCode"].ToString().Trim().Contains("WebL") &&
                             dtSelected.Rows[0]["BatchCode"].ToString().Trim().Contains("_NR"))
                        {
                            uri = new Uri("/Templates/PrintTemplateCancerBaseWebLNR.xlsx", UriKind.Relative);
                            TemplateLines = 5;
                        }
                        //else if (campaign.Code == "PLFDLITE" || campaign.Code == "PLCLITE" || campaign.Code == "PLMBLITE")
                        //{
                        //    uri = new Uri("/Templates/PrintTemplateLITE.xlsx", UriKind.Relative);
                        //    TemplateLines = 4;
                        //}
                        else
                        {
                            uri = new Uri("/Templates/PrintTemplateCancerBase.xlsx", UriKind.Relative);
                            TemplateLines = 4;
                        }
                        
                        leadsPerPage = 5;
                        leadColumnWidth = 16;
                        coverColumns = 15;
                        coverRows = 29;

                        upgradeCoverSheetColumnIndex = 6;
                        upgradeCoverSheetRowIndex = 0;
                        verticalSpacingBetweenLeads = 1;
                        break;

                    #endregion Other non-upgrade campaigns
                }

                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    wbTemplate = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }

                Worksheet wsSummaryTemplate = wbTemplate.Worksheets["Summary"];
                Worksheet wsCoverTemplate = wbTemplate.Worksheets["Cover"];
                Worksheet wsLeadTemplate = wbTemplate.Worksheets["Lead"];
                Worksheet wsConversionTemplate = wbTemplate.Worksheets["Conversion"];

                Worksheet wsCoverAndLeadTemplate = wbTemplate.Worksheets["Lead"];
                if (IsUpgradeCampaign)
                {
                    wsCoverAndLeadTemplate = wbTemplate.Worksheets["Cover And Lead"];
                }

                Worksheet wsSummary = wbPrint.Worksheets.Add("Summary");
                //wsSummary.PrintOptions.Header = "&C&B" + LeadBookName;
                //wsSummary.PrintOptions.Footer = "&C&B" + nextMonday.ToString("yyyy/MM/dd");
                wsSummary.PrintOptions.Header = "&C&B" + LeadBookName.Replace("#DATE#", (Convert.ToDateTime(dtSelected.Rows[0]["AllocationDate"])).ToString("yyyy/MM/dd"));
                wsSummary.PrintOptions.Footer = "&C&B" + (Convert.ToDateTime(dtSelected.Rows[0]["SalesStartDate"])).ToString("yyyy/MM/dd");

                Methods.SetExcelStandardWSOptions(wsSummary, Orientation.Landscape);                

                //Worksheet wsCover = wbPrint.Worksheets.Add("Cover");
                //wsCover.PrintOptions.Header = "&C&B" + LeadBookName;
                //wsCover.PrintOptions.Footer = "&C&B" + nextMonday.ToString("yyyy/MM/dd");
                //Methods.SetExcelStandardWSOptions(wsCover);

                #endregion

                #region Setup Summary Page

                Methods.CopyExcelRegion(wsSummaryTemplate, 0, 0, 32, 7, wsSummary, 0, 0);
                //wsSummary.MoveToIndex(0);
                #endregion Setup Summary Page

                #region Print Leads for Selected Agents

                const int coverRow = 0;
                int summaryRow = 4;

                foreach (DataRow drAgent in dtSelected.AsEnumerable())
                {
                    #region Get Lead Data

                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@BatchID", drAgent["BatchID"]);
                    parameters[1] = new SqlParameter("@AgentID", drAgent["AgentID"]);
                    parameters[2] = new SqlParameter("@AllocationDate", drAgent["AllocationDate"]);

                    DataSet ds;
                    string strCampaignCode = dtCampaigns.Rows[0][0].ToString();

                    if (IsUpgradeCampaign)
                    {
                        if (campaign.Code == "PLDMM5U")
                        {
                            ds = Methods.ExecuteStoredProcedure("spINGetLeadsForUserAndBatchPLDMM5U", parameters);
                        }
                        else
                        {
                            ds = Methods.ExecuteStoredProcedure("spINGetLeadsForUserAndBatchUpgrades", parameters);
                        }
                    }
                    else
                    {
                        if (strCampaignCode == "ACCDIS")
					    {
						    ds = Methods.ExecuteStoredProcedure("spINGetLeadsForUserAndBatchACCDIS", parameters);
					    }
                        else if (strCampaignCode.Contains("REJ") || strCampaignCode.Contains("DEF") || strCampaignCode.Contains("REACT"))
                        {
                            ds = Methods.ExecuteStoredProcedure("spINGetLeadsForUserAndBatch2", parameters);
                        }
                        else if (strCampaignCode.Contains("RES"))
                        {
                            ds = Methods.ExecuteStoredProcedure("spINGetLeadsForUserAndBatchResurrection", parameters);
                        }
                        //else if (campaign.Code == "PLFDLITE" || campaign.Code == "PLCLITE" || campaign.Code == "PLMBLITE")
                        //{
                        //    ds = Methods.ExecuteStoredProcedure("spINGetLeadsForUserAndBatchLite", parameters);

                        //}
                        else
					    {
						    ds = Methods.ExecuteStoredProcedure("spINGetLeadsForUserAndBatch", parameters);
					    }
                    }

                    #if !TRAININGBUILD

                    DataTable dtLeadPrintData = ds.Tables[0];
                    dtLeadPrintData = Methods.OrderRandomly(dtLeadPrintData.AsEnumerable()).CopyToDataTable();

                    #else
                    
                    DataTable dtLeadPrintData = ds.Tables[0];
                    DataView dvLeadPrintData = dtLeadPrintData.DefaultView;
                    dvLeadPrintData.Sort = "ImportID ASC";
                    dtLeadPrintData = dvLeadPrintData.ToTable();

                    #endif

                    #endregion Get Lead Data

                    #region Create Leadbook Entry

                    INLeadBook inLeadBook = new INLeadBook();
                    inLeadBook.FKUserID = (long) drAgent["AgentID"];
                    inLeadBook.FKINBatchID = (long) drAgent["BatchID"];
                    inLeadBook.Description = nextMonday.ToString().Substring(0, 10) + "-" + drAgent["CampaignCode"] + "-" + drAgent["BatchCode"];
                    inLeadBook.Save(_validationResult);
                    long leadBookID = inLeadBook.ID;

                    #endregion Create Leadbook Entry

                    #region Cover Pages

                    //Methods.CopyExcelRegion(wsCoverTemplate, 0, 0, 29, coverColumnWidth, wsCover, 0 + coverRow, 0);

                    //WorksheetCell wsCell = wsCover.Rows[15 + coverRow].Cells[6];
                    //wsCell.Value = drAgent["CampaignName"];

                    //wsCell = wsCover.Rows[16 + coverRow].Cells[6];
                    //wsCell.Value = drAgent["SalesAgent"];

                    //wsCell = wsCover.Rows[17 + coverRow].Cells[6];
                    //wsCell.Value = drAgent["Leads2Print"];

                    //wsCell = wsCover.Rows[18 + coverRow].Cells[6];
                    //long pages = Convert.ToInt64(drAgent["Leads2Print"])/5;
                    //long remainder = Convert.ToInt64(drAgent["Leads2Print"])%5;
                    //if (remainder > 0) pages++;
                    //wsCell.Value = pages;

                    //wsCell = wsCover.Rows[19 + coverRow].Cells[6];
                    //wsCell.Value = drAgent["CampaignName"] + " " + drAgent["BatchCode"];

                    //wsCell = wsCover.Rows[20 + coverRow].Cells[6];
                    //wsCell.Value = (Convert.ToDateTime(drAgent["AllocationDate"])).ToString("yyyy/MM/dd");

                    //wsCell = wsCover.Rows[21 + coverRow].Cells[6];
                    //wsCell.Value = Convert.ToDateTime(drAgent["AllocationDate"]).AddDays(63).ToString("yyyy/MM/dd");

                    //coverRow += 30;

                    #endregion Cover Pages

                    #region Populate Leads Worksheet

                    bool isRedeemedGiftBatch = Convert.ToBoolean(drAgent["IsRedeemedGiftBatch"]);

                    //setup leadsheet
                    string strAgent = drAgent["SalesAgent"].ToString().Length > 31 ? drAgent["SalesAgent"].ToString().Substring(0, 31) : drAgent["SalesAgent"].ToString();

                    lblAgent.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate{lblAgent.Text = strAgent;}));

                    Worksheet wsLeads = wbPrint.Worksheets.Add(strAgent);

                    wsLeads.PrintOptions.StartPageNumber = 1;
                    wsLeads.PrintOptions.Header = "&L&B" + strAgent + "&C&B" + LeadBookName.Replace("#DATE#", (Convert.ToDateTime(drAgent["AllocationDate"])).ToString("yyyy/MM/dd")) + "&R&BPage &P of &N";
                    wsLeads.PrintOptions.Footer = "&C&B" + (Convert.ToDateTime(drAgent["SalesStartDate"])).ToString("yyyy/MM/dd");

                    //populate leadsheet
                    int[] lead = new int[1]; //array is needed so that the variable can be used in dispatcher
                    int leadRow = 0;
                    int leadsToPrint = Convert.ToInt32(dtLeadPrintData.Rows.Count);

                    #region Cover Page

                    wbPrint.NamedReferences.Clear();

                    if (IsUpgradeCampaign)
                    {
                        //SetExcelUpgradeWSOptions(wsLeads, Orientation.Landscape);

                        CopyWorkSheetOptionsFromTemplate(wsCoverAndLeadTemplate, wsLeads);

                        Methods.CopyExcelRegion(wsCoverAndLeadTemplate, 0, 0, coverRows, coverColumns, wsLeads, 0 + coverRow, 0);

                        // Because the default row height differs, it is necessary to explicitly set these rows' heights
                        wsLeads.Rows[15 + coverRow + upgradeCoverSheetRowIndex].Height = 464;
                        wsLeads.Rows[16 + coverRow + upgradeCoverSheetRowIndex].Height = 464;
                        wsLeads.Rows[17 + coverRow + upgradeCoverSheetRowIndex].Height = 464;
                        wsLeads.Rows[18 + coverRow + upgradeCoverSheetRowIndex].Height = 464;
                        wsLeads.Rows[19 + coverRow + upgradeCoverSheetRowIndex].Height = 464;
                        wsLeads.Rows[20 + coverRow + upgradeCoverSheetRowIndex].Height = 464;
                        wsLeads.Rows[21 + coverRow + upgradeCoverSheetRowIndex].Height = 464;

                        wsLeads.Columns[55].Width = 10;

                        //Methods.CopyExcelRegion(wsCoverAndLeadTemplate, 0, 0, coverRows, coverColumns, wsLeads, 0 + coverRow, 0);
                    }
                    else
                    {
                        //Methods.SetExcelStandardWSOptions(wsLeads, Orientation.Landscape);
                        Methods.CopyWorksheetOptionsFromTemplate(wsLeadTemplate, wsLeads, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false);
                        Methods.CopyExcelRegion(wsCoverTemplate, 0, 0, coverRows, coverColumns, wsLeads, 0 + coverRow, 0);
                    }

                    WorksheetCell wsCell = wsLeads.Rows[15 + coverRow + upgradeCoverSheetRowIndex].Cells[upgradeCoverSheetColumnIndex];
                    wsCell.Value = drAgent["CampaignName"];

                    wsCell = wsLeads.Rows[16 + coverRow + upgradeCoverSheetRowIndex].Cells[upgradeCoverSheetColumnIndex];
                    wsCell.Value = drAgent["SalesAgent"];

                    wsCell = wsLeads.Rows[17 + coverRow + upgradeCoverSheetRowIndex].Cells[upgradeCoverSheetColumnIndex];
                    wsCell.Value = drAgent["Leads2Print"];

                    wsCell = wsLeads.Rows[18 + coverRow + upgradeCoverSheetRowIndex].Cells[upgradeCoverSheetColumnIndex];

                    long pages = Convert.ToInt64(drAgent["Leads2Print"]) / leadsPerPage;
                    long remainder = Convert.ToInt64(drAgent["Leads2Print"]) % leadsPerPage;
                    if (remainder > 0) pages++; wsCell.Value = pages;

                    wsCell = wsLeads.Rows[19 + coverRow + upgradeCoverSheetRowIndex].Cells[upgradeCoverSheetColumnIndex];
                    wsCell.Value = drAgent["CampaignName"] + " " + drAgent["BatchCode"];

                    wsCell = wsLeads.Rows[20 + coverRow + upgradeCoverSheetRowIndex].Cells[upgradeCoverSheetColumnIndex];
                    wsCell.Value = (Convert.ToDateTime(drAgent["SalesStartDate"])).ToString("yyyy/MM/dd");

                    wsCell = wsLeads.Rows[21 + coverRow + upgradeCoverSheetRowIndex].Cells[upgradeCoverSheetColumnIndex];
                    //wsCell.Value = Convert.ToDateTime(drAgent["SalesStartDate"]).AddDays(63).ToString("yyyy/MM/dd");//expiry date
                    wsCell.Value = Convert.ToDateTime(drAgent["ExpiryDate"]).ToString("yyyy/MM/dd");

                    leadRow += IsUpgradeCampaign ? leadTemplateRowIndex + 2 : 31;

                    #endregion Cover Page
                    
                    #region Insert Page Break

                    //See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/208328091/comments     
                    wsLeads.PrintOptions.HorizontalPageBreaks.Add(new HorizontalPageBreak(leadRow));

                    #endregion Insert Page Break

                    for (lead[0] = 1; lead[0] <= leadsToPrint; lead[0]++)
                    {
                        wbPrint.NamedReferences.Clear();
                        //Methods.CopyExcelRegion(wsLeadTemplate, 0, 0, TemplateLines + 1, leadColumnWidth, wsLeads, leadRow, 0);

                        #region Upgrade Campaigns

                        if (IsUpgradeCampaign)
                        {
                            if (campaign.Code == "PLDMM5U")
                            {
                                Methods.CopyExcelRegion(wsCoverAndLeadTemplate, leadTemplateRowIndex, 0, TemplateLines + 1, leadColumnWidth, wsLeads, leadRow, 0);

                                #region Setting the values of the cells

                                wsLeads.GetCell("RefNo").Value = dtLeadPrintData.Rows[lead[0] - 1]["RefNo"].ToString();
                                wsLeads.GetCell("Title").Value = dtLeadPrintData.Rows[lead[0] - 1]["Title"].ToString();
                                wsLeads.GetCell("FirstName").Value = dtLeadPrintData.Rows[lead[0] - 1]["FirstName"].ToString();
                                wsLeads.GetCell("Surname").Value = dtLeadPrintData.Rows[lead[0] - 1]["Surname"].ToString();
                                wsLeads.GetCell("IDNo").Value = dtLeadPrintData.Rows[lead[0] - 1]["IDNo"].ToString();
                                wsLeads.GetCell("Address").Value = dtLeadPrintData.Rows[lead[0] - 1]["Address"].ToString();
                                wsLeads.GetCell("PostalCode").Value = dtLeadPrintData.Rows[lead[0] - 1]["PostalCode"].ToString();
                                wsLeads.GetCell("TelWork").Value = dtLeadPrintData.Rows[lead[0] - 1]["TelWork"].ToString();
                                wsLeads.GetCell("TelHome").Value = dtLeadPrintData.Rows[lead[0] - 1]["TelHome"].ToString();
                                wsLeads.GetCell("TelCell").Value = dtLeadPrintData.Rows[lead[0] - 1]["TelCell"].ToString();
                                wsLeads.GetCell("DebitDay").Value = dtLeadPrintData.Rows[lead[0] - 1]["DebitDay"].ToString();
                                //wsLeads.GetCell("Option").Value = dtLeadPrintData.Rows[lead[0] - 1]["Option"].ToString();
                                wsLeads.GetCell("LifeAssured2FirstName").Value = dtLeadPrintData.Rows[lead[0] - 1]["LifeAssured2FirstName"].ToString();
                                wsLeads.GetCell("LifeAssured2Surname").Value = dtLeadPrintData.Rows[lead[0] - 1]["LifeAssured2Surname"].ToString();
                                wsLeads.GetCell("LifeAssured2DateOfBirth").Value = dtLeadPrintData.Rows[lead[0] - 1]["LifeAssured2DateOfBirth"].ToString();
                                wsLeads.GetCell("LA2Contact").Value = dtLeadPrintData.Rows[lead[0] - 1]["LA2Contact"].ToString();
                                wsLeads.GetCell("BenContact").Value = dtLeadPrintData.Rows[lead[0] - 1]["BenContact"].ToString();
                                wsLeads.GetCell("Beneficiary1FirstName").Value = dtLeadPrintData.Rows[lead[0] - 1]["Beneficiary1FirstName"].ToString();
                                wsLeads.GetCell("Beneficiary1Surname").Value = dtLeadPrintData.Rows[lead[0] - 1]["Beneficiary1Surname"].ToString();
                                wsLeads.GetCell("Beneficiary1DateOfBirth").Value = dtLeadPrintData.Rows[lead[0] - 1]["Beneficiary1DateOfBirth"].ToString();
                                wsLeads.GetCell("Beneficiary1Relationship").Value = dtLeadPrintData.Rows[lead[0] - 1]["Beneficiary1Relationship"].ToString();
                                wsLeads.GetCell("Beneficiary1Percentage").Value = dtLeadPrintData.Rows[lead[0] - 1]["Beneficiary1Percentage"].ToString();

                                if (uri.ToString() == "/Templates/PrintTemplateUpgrade4.xlsx")
                                {
                                    string str;
                                    string[,] Label = new string[3, 4];
                                    string[,] Value = new string[3, 4];

                                    Action<int, int, string, string> getDisplayData = delegate (int i, int j, string strCover, string strDisplay)
                                    {
                                        Value[i, j] = Convert.ToString(dtLeadPrintData.Rows[lead[0] - 1][strCover]);
                                        if (!string.IsNullOrWhiteSpace(Value[i, j]))
                                        {
                                            Label[i, j] = strDisplay + ", ";
                                            if (Value[i, j].Length > Value[i, j].Length - 3)
                                                Value[i, j] = "R " + $"{Convert.ToDecimal(Value[i, j]):N0}" + ", ";//.Remove(Value[i,j].Length - 3)
                                        }
                                    };


                                    //LA1
                                    string[,,] LA1Strings =
                                    {
                                    {
                                        {"LA1CancerCover", "Cancer" }, {"LA1DisabilityCover", "Disability" }, {"LA1AccidentalDeathCover", "AccDeath" }, {"LA1FuneralCover", "Funeral" }
                                    },
                                    {
                                        {"LA1CancerCoverPreferred", "Cancer" }, {"LA1DisabilityCoverPreferred", "Disability" }, {"LA1AccidentalDeathCoverPreferred", "AccDeath" }, {"LA1FuneralCoverPreferred", "Funeral" }
                                    },
                                    {
                                        {"LA1CancerCoverTotal", "Cancer" }, {"LA1DisabilityCoverTotal", "Disability" }, {"LA1AccidentalDeathCoverTotal", "AccDeath" }, {"LA1FuneralCoverTotal", "Funeral" }
                                    }
                                };

                                    for (int l = 0; l < 3; l++) //Current, Upgrade, Total
                                    {
                                        for (int k = 0; k < 4; k++) //Cancer, Disability, AccDeath, Funeral
                                        {
                                            getDisplayData(l, k, LA1Strings[l, k, 0], LA1Strings[l, k, 1]);
                                        }
                                        str = "LA1 " + Label[l, 0] + Label[l, 1] + Label[l, 2] + Label[l, 3];
                                        if (str.Length > 4)
                                            wsLeads.GetCell("LA1Label" + (l + 1)).Value = str.Remove(str.Length - 2);
                                        str = Value[l, 0] + Value[l, 1] + Value[l, 2] + Value[l, 3];
                                        if (str.Length > 4)
                                            wsLeads.GetCell("LA1Value" + (l + 1)).Value = str.Remove(str.Length - 2);
                                    }

                                    //LA2
                                    Label = new string[3, 4];
                                    Value = new string[3, 4];

                                    string[,,] LA2Strings =
                                    {
                                    {
                                        {"LA2CancerCover", "Cancer" }, {"LA2DisabilityCover", "Disability" }, {"LA2AccidentalDeathCover", "AccDeath" }, {"LA2FuneralCover", "Funeral" }
                                    },
                                    {
                                        {"LA2CancerCoverPreferred", "Cancer" }, {"LA2DisabilityCoverPreferred", "Disability" }, {"LA2AccidentalDeathCoverPreferred", "AccDeath" }, {"LA2FuneralCoverPreferred", "Funeral" }
                                    },
                                    {
                                        {"LA2CancerCoverTotal", "Cancer" }, {"LA2DisabilityCoverTotal", "Disability" }, {"LA2AccidentalDeathCoverTotal", "AccDeath" }, {"LA2FuneralCoverTotal", "Funeral" }
                                    }
                                };

                                    for (int l = 0; l < 3; l++) //Current, Upgrade, Total
                                    {
                                        for (int k = 0; k < 4; k++) //Cancer, Disability, AccDeath, Funeral
                                        {
                                            getDisplayData(l, k, LA2Strings[l, k, 0], LA2Strings[l, k, 1]);
                                        }
                                        str = "LA2 " + Label[l, 0] + Label[l, 1] + Label[l, 2] + Label[l, 3];
                                        if (str.Length > 4)
                                            wsLeads.GetCell("LA2Label" + (l + 1)).Value = str.Remove(str.Length - 2);
                                        str = Value[l, 0] + Value[l, 1] + Value[l, 2] + Value[l, 3];
                                        if (str.Length > 4)
                                            wsLeads.GetCell("LA2Value" + (l + 1)).Value = str.Remove(str.Length - 2);
                                    }

                                }

                                if (batch.Code.Contains("_R"))
                                {
                                    wsLeads.GetCell("DeliverySignature").Value = dtLeadPrintData.Rows[lead[0] - 1]["DeliverySignature"].ToString();
                                    wsLeads.GetCell("DeliveryDate").Value = dtLeadPrintData.Rows[lead[0] - 1]["DeliveryDate"].ToString();
                                }

                                #region Adding Currency Values
                                // Because Excel is up to !@##$%$%^%^&$#%&^%$E^&&&&&&^%^&^%$ and doesn't want to copy the formatting from the template!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                                wsLeads.GetCell("ContractPremium").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["ContractPremium"], false);
                                wsLeads.GetCell("Offer").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["Offer"], false);
                                wsLeads.GetCell("NewTotalPremium").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["NewTotalPremium"], false);
                                wsLeads.GetCell("ChildCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["ChildCover"], false);
                                wsLeads.GetCell("ChildCover3").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["ChildCover"], false);

                                if (uri.ToString() != "/Templates/PrintTemplateUpgrade4.xlsx")
                                {
                                    wsLeads.GetCell("LA1CancerCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1CancerCover"], false);
                                    wsLeads.GetCell("LA1DisabilityCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1DisabilityCover"], false);
                                    wsLeads.GetCell("LA1AccidentalDeathCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1AccidentalDeathCover"], false);
                                    wsLeads.GetCell("LA1FuneralCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1FuneralCover"], false);
                                    wsLeads.GetCell("LA2CancerCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2CancerCover"], false);
                                    wsLeads.GetCell("LA2DisabilityCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2DisabilityCover"], false);
                                    wsLeads.GetCell("LA2AccidentalDeathCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2AccidentalDeathCover"], false);
                                    wsLeads.GetCell("LA2FuneralCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2FuneralCover"], false);

                                    wsLeads.GetCell("LA1CancerCoverPreferred").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1CancerCoverPreferred"], false);
                                    wsLeads.GetCell("LA1DisabilityCoverPreferred").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1DisabilityCoverPreferred"], false);
                                    wsLeads.GetCell("LA1AccidentalDeathCoverPreferred").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1AccidentalDeathCoverPreferred"], false);
                                    wsLeads.GetCell("LA1FuneralCoverPreferred").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1FuneralCoverPreferred"], false);
                                    wsLeads.GetCell("LA2CancerCoverPreferred").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2CancerCoverPreferred"], false);
                                    wsLeads.GetCell("LA2DisabilityCoverPreferred").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2DisabilityCoverPreferred"], false);
                                    wsLeads.GetCell("LA2AccidentalDeathCoverPreferred").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2AccidentalDeathCoverPreferred"], false);
                                    wsLeads.GetCell("LA2FuneralCoverPreferred").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2FuneralCoverPreferred"], false);

                                    wsLeads.GetCell("LA1CancerCoverTotal").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1CancerCoverTotal"], false);
                                    wsLeads.GetCell("LA1DisabilityCoverTotal").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1DisabilityCoverTotal"], false);
                                    wsLeads.GetCell("LA1AccidentalDeathCoverTotal").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1AccidentalDeathCoverTotal"], false);
                                    wsLeads.GetCell("LA1FuneralCoverTotal").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1FuneralCoverTotal"], false);
                                    wsLeads.GetCell("LA2CancerCoverTotal").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2CancerCoverTotal"], false);
                                    wsLeads.GetCell("LA2DisabilityCoverTotal").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2DisabilityCoverTotal"], false);
                                    wsLeads.GetCell("LA2AccidentalDeathCoverTotal").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2AccidentalDeathCoverTotal"], false);
                                    wsLeads.GetCell("LA2FuneralCoverTotal").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2FuneralCoverTotal"], false);
                                }

                                #endregion Adding Currency Values

                                #endregion Setting the values of the cells
                            }
                            else
                            {
                                Methods.CopyExcelRegion(wsCoverAndLeadTemplate, leadTemplateRowIndex, 0, TemplateLines + 1, leadColumnWidth, wsLeads, leadRow, 0);

                                #region Setting the values of the cells

                                wsLeads.GetCell("RefNo").Value = dtLeadPrintData.Rows[lead[0] - 1]["RefNo"].ToString();
                                wsLeads.GetCell("Title").Value = dtLeadPrintData.Rows[lead[0] - 1]["Title"].ToString();
                                wsLeads.GetCell("FirstName").Value = dtLeadPrintData.Rows[lead[0] - 1]["FirstName"].ToString();
                                wsLeads.GetCell("Surname").Value = dtLeadPrintData.Rows[lead[0] - 1]["Surname"].ToString();
                                wsLeads.GetCell("IDNo").Value = dtLeadPrintData.Rows[lead[0] - 1]["IDNo"].ToString();
                                wsLeads.GetCell("Address").Value = dtLeadPrintData.Rows[lead[0] - 1]["Address"].ToString();
                                wsLeads.GetCell("PostalCode").Value = dtLeadPrintData.Rows[lead[0] - 1]["PostalCode"].ToString();
                                wsLeads.GetCell("TelWork").Value = dtLeadPrintData.Rows[lead[0] - 1]["TelWork"].ToString();
                                wsLeads.GetCell("TelHome").Value = dtLeadPrintData.Rows[lead[0] - 1]["TelHome"].ToString();
                                wsLeads.GetCell("TelCell").Value = dtLeadPrintData.Rows[lead[0] - 1]["TelCell"].ToString();
                                wsLeads.GetCell("DebitDay").Value = dtLeadPrintData.Rows[lead[0] - 1]["DebitDay"].ToString();
                                //wsLeads.GetCell("Option").Value = dtLeadPrintData.Rows[lead[0] - 1]["Option"].ToString();
                                wsLeads.GetCell("LifeAssured2FirstName").Value = dtLeadPrintData.Rows[lead[0] - 1]["LifeAssured2FirstName"].ToString();
                                wsLeads.GetCell("LifeAssured2Surname").Value = dtLeadPrintData.Rows[lead[0] - 1]["LifeAssured2Surname"].ToString();
                                wsLeads.GetCell("LifeAssured2DateOfBirth").Value = dtLeadPrintData.Rows[lead[0] - 1]["LifeAssured2DateOfBirth"].ToString();
                                wsLeads.GetCell("LA2Contact").Value = dtLeadPrintData.Rows[lead[0] - 1]["LA2Contact"].ToString();
                                wsLeads.GetCell("BenContact").Value = dtLeadPrintData.Rows[lead[0] - 1]["BenContact"].ToString();
                                wsLeads.GetCell("Beneficiary1FirstName").Value = dtLeadPrintData.Rows[lead[0] - 1]["Beneficiary1FirstName"].ToString();
                                wsLeads.GetCell("Beneficiary1Surname").Value = dtLeadPrintData.Rows[lead[0] - 1]["Beneficiary1Surname"].ToString();
                                wsLeads.GetCell("Beneficiary1DateOfBirth").Value = dtLeadPrintData.Rows[lead[0] - 1]["Beneficiary1DateOfBirth"].ToString();
                                wsLeads.GetCell("Beneficiary1Relationship").Value = dtLeadPrintData.Rows[lead[0] - 1]["Beneficiary1Relationship"].ToString();
                                wsLeads.GetCell("Beneficiary1Percentage").Value = dtLeadPrintData.Rows[lead[0] - 1]["Beneficiary1Percentage"].ToString();

                                if (uri.ToString() == "/Templates/PrintTemplateUpgrade3.xlsx" ||
                                    uri.ToString() == "/Templates/PrintTemplateUpgrade5.xlsx" ||
                                    uri.ToString() == "/Templates/PrintTemplateUpgradeRedeemed2.xlsx" ||
                                    uri.ToString() == "/Templates/PrintTemplateUpgradeNonRedeemed2c.xlsx" ||
                                    uri.ToString() == "/Templates/PrintTemplateUpgradeNonRedeemed2cWLite.xlsx" ||
                                    uri.ToString() == "/Templates/PrintTemplateUpgradeNonRedeemed2m.xlsx" ||
                                    uri.ToString() == "/Templates/PrintTemplateUpgradeNonRedeemed2mWLite.xlsx")
                                {
                                    string str;
                                    string[,] Label = new string[3,4];
                                    string[,] Value = new string[3,4];

                                    Action<int, int, string, string> getDisplayData = delegate(int i, int j, string strCover, string strDisplay)
                                    {
                                        Value[i,j] = Convert.ToString(dtLeadPrintData.Rows[lead[0] - 1][strCover]);
                                        if (!string.IsNullOrWhiteSpace(Value[i,j]))
                                        {
                                            Label[i,j] = strDisplay + ", ";
                                            if (Value[i, j].Length > Value[i, j].Length - 3)
                                                Value[i,j] = "R " + $"{Convert.ToDecimal(Value[i, j]):N0}" + ", ";//.Remove(Value[i,j].Length - 3)
                                        }
                                    };


                                    //LA1
                                    string[,,] LA1Strings = 
                                    {
                                        {
                                            {"LA1CancerCover", "Cancer" }, {"LA1DisabilityCover", "Disability" }, {"LA1AccidentalDeathCover", "AccDeath" }, {"LA1FuneralCover", "Funeral" }
                                        },
                                        {
                                            {"LA1CancerCoverPreferred", "Cancer" }, {"LA1DisabilityCoverPreferred", "Disability" }, {"LA1AccidentalDeathCoverPreferred", "AccDeath" }, {"LA1FuneralCoverPreferred", "Funeral" }
                                        },
                                        {
                                            {"LA1CancerCoverTotal", "Cancer" }, {"LA1DisabilityCoverTotal", "Disability" }, {"LA1AccidentalDeathCoverTotal", "AccDeath" }, {"LA1FuneralCoverTotal", "Funeral" }
                                        }
                                    };

                                    for (int l = 0; l < 3; l++) //Current, Upgrade, Total
                                    {
                                        for (int k = 0; k < 4; k++) //Cancer, Disability, AccDeath, Funeral
                                        {
                                            getDisplayData(l, k, LA1Strings[l,k,0], LA1Strings[l,k,1]);
                                        }
                                        str = "LA1 " + Label[l,0] + Label[l,1] + Label[l,2] + Label[l,3];
                                        if (str.Length > 4)
                                            wsLeads.GetCell("LA1Label" + (l + 1)).Value = str.Remove(str.Length - 2);
                                        str = Value[l,0] + Value[l,1] + Value[l,2] + Value[l,3];
                                        if (str.Length > 4)
                                            wsLeads.GetCell("LA1Value" + (l + 1)).Value = str.Remove(str.Length - 2);
                                    }
                                
                                    //LA2
                                    Label = new string[3,4];
                                    Value = new string[3,4];

                                    string[,,] LA2Strings = 
                                    {
                                        {
                                            {"LA2CancerCover", "Cancer" }, {"LA2DisabilityCover", "Disability" }, {"LA2AccidentalDeathCover", "AccDeath" }, {"LA2FuneralCover", "Funeral" }
                                        },
                                        {
                                            {"LA2CancerCoverPreferred", "Cancer" }, {"LA2DisabilityCoverPreferred", "Disability" }, {"LA2AccidentalDeathCoverPreferred", "AccDeath" }, {"LA2FuneralCoverPreferred", "Funeral" }
                                        },
                                        {
                                            {"LA2CancerCoverTotal", "Cancer" }, {"LA2DisabilityCoverTotal", "Disability" }, {"LA2AccidentalDeathCoverTotal", "AccDeath" }, {"LA2FuneralCoverTotal", "Funeral" }
                                        }
                                    };

                                    for (int l = 0; l < 3; l++) //Current, Upgrade, Total
                                    {
                                        for (int k = 0; k < 4; k++) //Cancer, Disability, AccDeath, Funeral
                                        {
                                            getDisplayData(l, k, LA2Strings[l,k,0], LA2Strings[l,k,1]);
                                        }
                                        str = "LA2 " + Label[l,0] + Label[l,1] + Label[l,2] + Label[l,3];
                                        if (str.Length > 4)
                                            wsLeads.GetCell("LA2Label" + (l + 1)).Value = str.Remove(str.Length - 2);
                                        str = Value[l,0] + Value[l,1] + Value[l,2] + Value[l,3];
                                        if (str.Length > 4)
                                            wsLeads.GetCell("LA2Value" + (l + 1)).Value = str.Remove(str.Length - 2);
                                    }
                                
                                }

                                if (batch.Code.Contains("_R"))
                                {
                                    wsLeads.GetCell("DeliverySignature").Value = dtLeadPrintData.Rows[lead[0] - 1]["DeliverySignature"].ToString();
                                    wsLeads.GetCell("DeliveryDate").Value = dtLeadPrintData.Rows[lead[0] - 1]["DeliveryDate"].ToString();
                                }

                                #region Adding Currency Values
                                // Because Excel is up to !@##$%$%^%^&$#%&^%$E^&&&&&&^%^&^%$ and doesn't want to copy the formatting from the template!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                                wsLeads.GetCell("ContractPremium").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["ContractPremium"], false);
                                wsLeads.GetCell("Offer").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["Offer"], false);
                                wsLeads.GetCell("NewTotalPremium").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["NewTotalPremium"], false);
                                wsLeads.GetCell("ChildCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["ChildCover"], false);
                                wsLeads.GetCell("ChildCover3").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["ChildCover"], false);

                                if (uri.ToString() != "/Templates/PrintTemplateUpgrade3.xlsx" &&
                                    uri.ToString() != "/Templates/PrintTemplateUpgrade5.xlsx" &&
                                    uri.ToString() != "/Templates/PrintTemplateUpgradeRedeemed2.xlsx" &&
                                    uri.ToString() != "/Templates/PrintTemplateUpgradeNonRedeemed2c.xlsx" &&
                                    uri.ToString() != "/Templates/PrintTemplateUpgradeNonRedeemed2cWLite.xlsx" &&
                                    uri.ToString() != "/Templates/PrintTemplateUpgradeNonRedeemed2m.xlsx" &&
                                    uri.ToString() != "/Templates/PrintTemplateUpgradeNonRedeemed2mWLite.xlsx")
                                {
                                    wsLeads.GetCell("LA1CancerCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1CancerCover"], false);
                                    wsLeads.GetCell("LA1DisabilityCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1DisabilityCover"], false);
                                    wsLeads.GetCell("LA1AccidentalDeathCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1AccidentalDeathCover"], false);
                                    wsLeads.GetCell("LA1FuneralCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1FuneralCover"], false);
                                    wsLeads.GetCell("LA2CancerCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2CancerCover"], false);
                                    wsLeads.GetCell("LA2DisabilityCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2DisabilityCover"], false);
                                    wsLeads.GetCell("LA2AccidentalDeathCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2AccidentalDeathCover"], false);
                                    wsLeads.GetCell("LA2FuneralCover").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2FuneralCover"], false);

                                    wsLeads.GetCell("LA1CancerCoverPreferred").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1CancerCoverPreferred"], false);
                                    wsLeads.GetCell("LA1DisabilityCoverPreferred").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1DisabilityCoverPreferred"], false);
                                    wsLeads.GetCell("LA1AccidentalDeathCoverPreferred").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1AccidentalDeathCoverPreferred"], false);
                                    wsLeads.GetCell("LA1FuneralCoverPreferred").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1FuneralCoverPreferred"], false);
                                    wsLeads.GetCell("LA2CancerCoverPreferred").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2CancerCoverPreferred"], false);
                                    wsLeads.GetCell("LA2DisabilityCoverPreferred").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2DisabilityCoverPreferred"], false);
                                    wsLeads.GetCell("LA2AccidentalDeathCoverPreferred").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2AccidentalDeathCoverPreferred"], false);
                                    wsLeads.GetCell("LA2FuneralCoverPreferred").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2FuneralCoverPreferred"], false);

                                    wsLeads.GetCell("LA1CancerCoverTotal").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1CancerCoverTotal"], false);
                                    wsLeads.GetCell("LA1DisabilityCoverTotal").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1DisabilityCoverTotal"], false);
                                    wsLeads.GetCell("LA1AccidentalDeathCoverTotal").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1AccidentalDeathCoverTotal"], false);
                                    wsLeads.GetCell("LA1FuneralCoverTotal").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA1FuneralCoverTotal"], false);
                                    wsLeads.GetCell("LA2CancerCoverTotal").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2CancerCoverTotal"], false);
                                    wsLeads.GetCell("LA2DisabilityCoverTotal").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2DisabilityCoverTotal"], false);
                                    wsLeads.GetCell("LA2AccidentalDeathCoverTotal").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2AccidentalDeathCoverTotal"], false);
                                    wsLeads.GetCell("LA2FuneralCoverTotal").Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["LA2FuneralCoverTotal"], false);
                                }

                                #endregion Adding Currency Values

                                #endregion Setting the values of the cells
                            }

                            INImport inImport = new INImport(long.Parse(dtLeadPrintData.Rows[lead[0] - 1]["ImportID"].ToString()));
                            inImport.IsPrinted = 1;
                            inImport.Save(_validationResult);

                            //Keep import print order for Status Loading
                            INLeadBookImport inLeadBookImport = new INLeadBookImport();
                            inLeadBookImport.FKINLeadBookID = leadBookID;
                            inLeadBookImport.FKINImportID = long.Parse(dtLeadPrintData.Rows[lead[0] - 1]["ImportID"].ToString());
                            inLeadBookImport.Save(_validationResult);

                            // To prevent leads from being printed between 2 separate pages, adjust the row counter by 2 to ensure it prints on teh next page.
                            //if (lead[0] % leadsPerPage == 0)
                            //{
                            //    leadRow += 2;
                            //}

                            wbPrint.NamedReferences.Clear();
                        }

                        #endregion Upgrade Campaigns

                        #region Other campaign types

                        else
                        {
                            Methods.CopyExcelRegion(wsLeadTemplate, 0, 0, TemplateLines + 1, leadColumnWidth, wsLeads, leadRow, 0);

                            for (int row = 0; row <= TemplateLines; row++)
                            {

                                for (int column = 0; column <= 15; column++)
                                {
                                    WorksheetCell targetCell = wsLeads.Rows[leadRow + row].Cells[column];

                                    //switch (column)
                                    //{
                                    //    case 23:
                                    //        targetCell = wsLeads.Rows[leadRow + row + 3].Cells[15];
                                    //        break;

                                    //    default:
                                    //        targetCell = wsLeads.Rows[leadRow + row].Cells[column];
                                    //        break;
                                    //}

                                    switch (row)
                                    {
                                        case 1:
                                            //if (targetCell != null)
                                            //{
                                                targetCell.Value = dtLeadPrintData.Rows[lead[0] - 1][column].ToString();
                                            //}
                                            break;

                                        // Force Insure to display
                                        case 3:
                                            if (!(campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.Rejuvenation
                                                || campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.Defrosted
                                                || campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.ReDefrost
                                                || campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.Reactivation
                                                || campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.Resurrection
                                                ))
                                            {
                                                targetCell = wsLeads.Rows[leadRow + row].Cells[15];
                                                targetCell.Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1]["ContractPremium"], false);
                                            }
                                            break;
                                    }
                                }

                                if (campaign.FKINCampaignGroupID == (long) lkpINCampaignGroup.Rejuvenation 
                                    || campaign.FKINCampaignGroupID == (long) lkpINCampaignGroup.Defrosted 
                                    || campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.ReDefrost 
                                    || campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.Reactivation
                                    || campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.Resurrection
                                    )
                                {
                                    for (int column = 17; column <= 23; column++)
                                    {
                                        WorksheetCell targetCell = null;

                                        switch (column)
                                        {
                                            case 17:
                                                targetCell = wsLeads.Rows[leadRow + row + 1].Cells[0];
                                                break;

                                            case 18:
                                                targetCell = wsLeads.Rows[leadRow + row + 1].Cells[4];
                                                break;

                                            case 19:
                                                targetCell = wsLeads.Rows[leadRow + row + 1].Cells[7];
                                                break;

                                            case 20:
                                                targetCell = wsLeads.Rows[leadRow + row + 1].Cells[10];
                                                break;

                                            case 21:
                                                targetCell = wsLeads.Rows[leadRow + row + 1].Cells[12];
                                                break;

                                            case 22:
                                                targetCell = wsLeads.Rows[leadRow + row + 1].Cells[14];
                                                break;

                                            case 23:
                                                targetCell = wsLeads.Rows[leadRow + row + 3].Cells[15];
                                                break;
                                        }

                                        switch (row)
                                        {
                                            case 1:
                                                //if (targetCell != null) targetCell.Value = dtLeadPrintData.Rows[lead[0] - 1][column].ToString();
                                                if (targetCell != null)
                                                {
                                                    if (column == 20)
                                                    {
                                                        if (campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.Resurrection)
                                                        {
                                                            targetCell.Value = dtLeadPrintData.Rows[lead[0] - 1][column].ToString();
                                                        }
                                                        else
                                                        {
                                                            targetCell.Value = String.Format("LA 2 Cover: {0}", Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1][column], false));
                                                        }
                                                    }

                                                    else if (column == 21)
                                                    {
                                                        if (campaign.FKINCampaignGroupID == (long)lkpINCampaignGroup.Resurrection)
                                                        {
                                                            targetCell.Value = dtLeadPrintData.Rows[lead[0] - 1][column].ToString();
                                                        }
                                                        else
                                                        {
                                                            targetCell.Value = String.Format("Child Cover: {0}", Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1][column], false));
                                                        }
                                                    }

                                                    else if (column == 23)
                                                    {
                                                        targetCell.Value = Methods.ForceCurrencyFormatting(dtLeadPrintData.Rows[lead[0] - 1][column], false);
                                                    }

                                                    else
                                                    {
                                                        targetCell.Value = dtLeadPrintData.Rows[lead[0] - 1][column].ToString();
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }

                                if (row == TemplateLines)
                                {
                                    INImport inImport = new INImport(long.Parse(dtLeadPrintData.Rows[lead[0] - 1]["ImportID"].ToString()));
                                    inImport.IsPrinted = 1;
                                    inImport.Save(_validationResult);

                                    //Keep import print order for Status Loading
                                    INLeadBookImport inLeadBookImport = new INLeadBookImport();
                                    inLeadBookImport.FKINLeadBookID = leadBookID;
                                    inLeadBookImport.FKINImportID = long.Parse(dtLeadPrintData.Rows[lead[0] - 1]["ImportID"].ToString());
                                    inLeadBookImport.Save(_validationResult);
                                }
                            }
                        }

                        #endregion Other campaign types

                        //update interface

                        leadRow = leadRow + TemplateLines + verticalSpacingBetweenLeads;

                        if (IsUpgradeCampaign && (lead[0] % leadsPerPage) == 0)
                        {
                            wsLeads.PrintOptions.HorizontalPageBreaks.Add(new HorizontalPageBreak(leadRow));
                        }

                        lblLeadNo.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { lblLeadNo.Text = "Printing Lead #: " + lead[0]; }));
                    }

                    #endregion Populate Leads Worksheet

                    #region Populate conversion sheets

                    DateTime dateReceived = Convert.ToDateTime(drAgent["SalesStartDate"]);                    
                    int totalLeads = Convert.ToInt32(drAgent["Leads2Print"].ToString());

                    // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/223109796/comments
                    var conversion2 = Convert.ToDecimal(drAgent["Conversion2"]);
                    var conversion4 = Convert.ToDecimal(drAgent["Conversion4"]);
                    var conversion6 = Convert.ToDecimal(drAgent["Conversion6"]);
                    var contactRateTargetPerc = Convert.ToDecimal(drAgent["ContactRateTarget"]);
                    string consultantConversionTabName;

                    //Worksheet wsConversion = wbPrint.Worksheets.Add("Conversion " + strAgent);
                    consultantConversionTabName = strAgent;

                    if (consultantConversionTabName.Length > 26)
                    {
                        consultantConversionTabName = consultantConversionTabName.Substring(0, 26);
                    }

                    Worksheet wsConversion = wbPrint.Worksheets.Add(consultantConversionTabName + " Conv");
                    wsConversion.PrintOptions.Header = "&C&B" + strAgent + " Conversion";
                    wsConversion.PrintOptions.Footer = "&C&B" + "Printed on: " + DateTime.Now.ToString("yyyy-MM-dd");
                    Methods.SetExcelStandardWSOptions(wsConversion, Orientation.Landscape);

                    //if (IsUpgradeCampaign)
                    //{
                    //    wsConversion.PrintOptions.RightMargin = 0.2;
                    //}

                    // Copy the template to the worksheet.
                    //Methods.CopyExcelRegion(wsConversionTemplate, 0, 0, 31, 14, wsConversion, 0, 0); 
                    Methods.CopyExcelRegion(wsConversionTemplate, 0, 0, 28, 14, wsConversion, 0, 0); // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/207460137/comments

                    // Add data to the worksheet. Campaign
                    WorksheetCell wsConversionCell = wsConversion.Rows[1].Cells[4];
                    string campaignName = drAgent[2].ToString();
                    wsConversionCell.Value = campaignName;

                    // Target %
                    wsConversionCell = wsConversion.Rows[2].Cells[4];
                    if (IsUpgradeCampaign)
                    {
                        wsConversionCell.Value = Math.Ceiling(conversion6) + " %";
                    }
                    else
                    {
                        wsConversionCell.Value = conversion6 + " %";
                    }

                    // Agent
                    wsConversionCell = wsConversion.Rows[5].Cells[4];
                    wsConversionCell.Value = strAgent;

                    // Batch
                    wsConversionCell = wsConversion.Rows[6].Cells[4];
                    wsConversionCell.Value = drAgent["BatchCode"];

                    // Total Leads
                    wsConversionCell = wsConversion.Rows[7].Cells[4];
                    wsConversionCell.Value = totalLeads;

                    // Target Sales (This cell will multiply target% with total leads to get it's final value).
                    wsConversionCell = wsConversion.Rows[8].Cells[4];
                    if (IsUpgradeCampaign)
                    {
                        wsConversionCell.Value =
                            Math.Ceiling(((totalLeads * Math.Ceiling(conversion6)) / 100m))
                                .ToString();
                    }
                    else
                    {
                        wsConversionCell.Value =
                            Math.Ceiling(((totalLeads * conversion6) / 100m))
                                .ToString(); //wsConversionCell.Value = ((totalLeads * conversion6) / 100).ToString(); 
                    }

                    // Date Received
                    wsConversionCell = wsConversion.Rows[9].Cells[4];                    
                    wsConversionCell.Value = dateReceived.ToString("yyyy-MM-dd");      

                    //Sales Target %
                    if (IsUpgradeCampaign)
                    {
                        wsConversionCell = wsConversion.Rows[18].Cells[5];
                        wsConversionCell.Value = Math.Ceiling(conversion6) + " %";
                    }
                    else
                    {
                        wsConversionCell = wsConversion.Rows[13].Cells[1];
                        wsConversionCell.Value = conversion6 + " %";
                    }

                    //Sales Target
                    if (IsUpgradeCampaign)
                    {
                        wsConversionCell = wsConversion.Rows[22].Cells[5];
                        wsConversionCell.Value = Math.Ceiling(((totalLeads * Math.Ceiling(conversion6)) / 100m)).ToString();
                    }
                    else
                    {
                        wsConversionCell = wsConversion.Rows[13].Cells[4];
                        wsConversionCell.Value = Math.Ceiling(((totalLeads * conversion6) / 100m)).ToString(); //wsConversionCell.Value = ((totalLeads * conversion6) / 100).ToString();              
                    }

                    //Contact Rate Target %
                    if (IsUpgradeCampaign)
                    {
                        wsConversionCell = wsConversion.Rows[18].Cells[12];
                        wsConversionCell.Value = Math.Ceiling(contactRateTargetPerc) + " %";
                    }

                    //Contact Rate Target
                    if (IsUpgradeCampaign)
                    {
                        wsConversionCell = wsConversion.Rows[22].Cells[12];
                        wsConversionCell.Value = Math.Ceiling(((totalLeads * Math.Ceiling(contactRateTargetPerc)) / 100m)).ToString();
                    }
                    
                    #region OLD

                    // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/207460137/comments

                    // I commented this out, just in case we have to roll back to the original week 2, 4 and 6 structure

                    // Week 2
                    //wsConversionCell = wsConversion.Rows[25].Cells[1];
                    //wsConversionCell.Value = "Week 2 - " + dateReceived.AddDays(14).ToString("yyyy-MM-dd");

                    //// Week 4
                    //wsConversionCell = wsConversion.Rows[27].Cells[1];
                    //wsConversionCell.Value = "Week 4 - " + dateReceived.AddDays(28).ToString("yyyy-MM-dd");

                    //// Week 6
                    //wsConversionCell = wsConversion.Rows[29].Cells[1];
                    //wsConversionCell.Value = "Week 6 - " + dateReceived.AddDays(42).ToString("yyyy-MM-dd");

                    //// Week 2 Target %
                    //wsConversionCell = wsConversion.Rows[25].Cells[6];
                    //wsConversionCell.Value = conversion2 + " %";

                    //// Week 4 Target %
                    //wsConversionCell = wsConversion.Rows[27].Cells[6];
                    //wsConversionCell.Value = conversion4 + " %";

                    //// Week 6 Target %
                    //wsConversionCell = wsConversion.Rows[29].Cells[6];
                    //wsConversionCell.Value = conversion6 + " %";

                    //// Week 2 TargetSales
                    //wsConversionCell = wsConversion.Rows[25].Cells[8];
                    //wsConversionCell.Value = Math.Ceiling(((totalLeads * conversion2) / 100m)).ToString(); //wsConversionCell.Value = ((totalLeads * conversion2) / 100).ToString();

                    //// Week 4 TargetSales
                    //wsConversionCell = wsConversion.Rows[27].Cells[8];
                    //wsConversionCell.Value = Math.Ceiling(((totalLeads * conversion4) / 100m)).ToString(); //wsConversionCell.Value = ((totalLeads * conversion4) / 100).ToString();

                    //// Week 6 TargetSales
                    //wsConversionCell = wsConversion.Rows[29].Cells[8];
                    //wsConversionCell.Value = Math.Ceiling(((totalLeads * conversion6) / 100m)).ToString(); //wsConversionCell.Value = ((totalLeads * conversion6) / 100).ToString();

                    #endregion OLD

                    #region NEW

                    // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/207460137/comments

                    if (IsUpgradeCampaign)
                    {
                        // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/207876165/comments

                        //// Target Date - Week 6
                        //wsConversionCell = wsConversion.Rows[25].Cells[1];
                        //wsConversionCell.Value = "Target Date - " + dateReceived.AddDays(42).ToString("yyyy-MM-dd");

                        //// Week 6 Target %
                        //wsConversionCell = wsConversion.Rows[25].Cells[6];
                        //wsConversionCell.Value = conversion6 + " %";
                        //wsConversionCell.CellFormat.Alignment = HorizontalCellAlignment.Center;

                        //// Week 6 Target Sales
                        //wsConversionCell = wsConversion.Rows[25].Cells[8];
                        //wsConversionCell.Value = Math.Ceiling(((totalLeads * conversion2) / 100m)).ToString(); //wsConversionCell.Value = ((totalLeads * conversion2) / 100).ToString();
                        //wsConversionCell.CellFormat.Alignment = HorizontalCellAlignment.Center;
                    }

                    else
                    {
                        // Week 1
                        wsConversionCell = wsConversion.Rows[25].Cells[1];
                        // wsConversionCell.Value = "Week 1 - " + dateReceived.AddDays(7).ToString("yyyy-MM-dd");//counting dates 1
                        wsConversionCell.Value = "Week 1 - " + drAgent["CountingWeek1"].ToString();//counting dates 1

                        // Week 2
                        wsConversionCell = wsConversion.Rows[26].Cells[1];
                        //wsConversionCell.Value = "Week 2 - " + dateReceived.AddDays(14).ToString("yyyy-MM-dd");//counting dates 2
                        wsConversionCell.Value = "Week 2 - " + drAgent["CountingWeek2"].ToString();//counting dates 2


                        //// Week 5
                        //wsConversionCell = wsConversion.Rows[27].Cells[1];
                        //wsConversionCell.Value = "Week 5 - " + dateReceived.AddDays(35).ToString("yyyy-MM-dd");

                        // Week 5 All STLs are now 4 weeks according to Sandi from 2019-05-01
                        wsConversionCell = wsConversion.Rows[27].Cells[1];
                        //wsConversionCell.Value = "Week 4 - " + dateReceived.AddDays(35).ToString("yyyy-MM-dd");//counting dates 4
                        wsConversionCell.Value = "Week 4 - " + drAgent["CountingWeek3"].ToString();//counting dates 4

                        // Week 1 Target %
                        wsConversionCell = wsConversion.Rows[25].Cells[6];
                        wsConversionCell.Value = conversion2 + " %";

                        // Week 2 Target %
                        wsConversionCell = wsConversion.Rows[26].Cells[6];
                        wsConversionCell.Value = conversion4 + " %";

                        // Week 5 Target %
                        wsConversionCell = wsConversion.Rows[27].Cells[6];
                        wsConversionCell.Value = conversion6 + " %";

                        // Week 1 TargetSales
                        wsConversionCell = wsConversion.Rows[25].Cells[8];
                        wsConversionCell.Value = Math.Ceiling(((totalLeads * conversion2) / 100m)).ToString(); //wsConversionCell.Value = ((totalLeads * conversion2) / 100).ToString();

                        // Week 2 TargetSales
                        wsConversionCell = wsConversion.Rows[26].Cells[8];
                        wsConversionCell.Value = Math.Ceiling(((totalLeads * conversion4) / 100m)).ToString(); //wsConversionCell.Value = ((totalLeads * conversion4) / 100).ToString();

                        // Week 5 TargetSales
                        wsConversionCell = wsConversion.Rows[27].Cells[8];
                        wsConversionCell.Value = Math.Ceiling(((totalLeads * conversion6) / 100m)).ToString(); //wsConversionCell.Value = ((totalLeads * conversion6) / 100).ToString();
                    }


                    if (!(IsUpgradeCampaign))
                    {
                        wsConversion.Rows[25].Hidden = true;
                    }
                    #endregion NEW

                    #endregion Populate conversion sheets

                    #region summary entry

                    if (summaryRow < summaryRow + 18)
                    {
                        wsCell = wsSummary.Rows[summaryRow].Cells[1];
                        wsCell.Value = drAgent["FirstName"];

                        wsCell = wsSummary.Rows[summaryRow].Cells[2];
                        wsCell.Value = drAgent["LastName"];

                        wsCell = wsSummary.Rows[summaryRow].Cells[3];
                        wsCell.Value = drAgent["BatchCode"];

                        wsCell = wsSummary.Rows[summaryRow].Cells[4];
                        wsCell.Value = (Convert.ToDateTime(drAgent["SalesStartDate"])).ToString("yyyy/MM/dd");

                        wsCell = wsSummary.Rows[summaryRow].Cells[5];
                        wsCell.Value = drAgent["Leads2Print"];
                    }
                    summaryRow++;

                    #endregion
                }

                #endregion Print Leads for Selected Agents

                #region save and display excel document
                string filePathAndName = GlobalSettings.UserFolder + LeadBookFileName + ".xlsx";
                wbPrint.SetCurrentFormat(WorkbookFormat.Excel2007);
                wbPrint.WindowOptions.SelectedWorksheet = wsSummary;

                //See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/209145461/comments#341417643
                if (filePathAndName.Contains(Environment.NewLine))
                {
                    filePathAndName = filePathAndName.Replace(Environment.NewLine, "");
                }

                wbPrint.Save(filePathAndName);

                //display excel document
                Process.Start(filePathAndName);

                #endregion

                CommitTransaction(null);
            }

            catch (Exception ex)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "An error has occurred.\nThe lead print job will now be cancelled.", "Lead Print Error", ShowMessageType.Error);
                HandleException(ex);
            }
        }

        private void PrintBaseLeadsRedeemedGifts(DataTable dtSelected)
        {
            #region Setup Excel Document

            Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/PrintTemplateCancerBase.xlsx");
            Workbook wbRedeemedGiftsTemplate = Methods.DefineTemplateWorkbook("/Templates/PrintTemplateBaseRedeemedGifts.xlsx");
            Workbook wbPrint = new Workbook(WorkbookFormat.Excel2007);

            INCampaign campaign = INCampaignMapper.SearchOne(null, null, null, dtSelected.Rows[0]["CampaignCode"].ToString(), null, null, null, null, null, null, null, null);
            DateTime nextMonday = Methods.NextWeekDay(DateTime.Now, DayOfWeek.Monday);

            DataTable dtCampaigns = Methods.GroupBy("CampaignCode", "CampaignCode", dtSelected);
            DataTable dtBatches = Methods.GroupBy("BatchCode", "BatchCode", dtSelected);
            string LeadBookName = dtCampaigns.Rows[0][0].ToString().Trim() + " Batch ";
            foreach (DataRow dr in dtBatches.AsEnumerable())
            {
                LeadBookName = LeadBookName + dr[0];
                LeadBookName = LeadBookName + ", ";
            }

            LeadBookName = LeadBookName.TrimEnd(',', ' ') + " (#DATE#)";
            string LeadBookFileName = LeadBookName.TrimEnd(')') + DateTime.Now.ToString(" HH.mm.ss") + ")";
            LeadBookFileName = LeadBookFileName.Replace("#DATE#", DateTime.Now.ToString("yyyy-MM-dd"));

            int templateLines = 4;
            int leadsPerPage = 5;
            int leadColumnWidth = 16;
            int coverColumns = 0;
            int coverRows = 29;
            int verticalSpacingBetweenLeads = 1;
            int leadTemplateRowIndex = 35; // Give it a default value
                 
            Worksheet wsSummaryTemplate = wbTemplate.Worksheets["Summary"];
            Worksheet wsConversionTemplate = wbTemplate.Worksheets["Conversion"];

            #region Add the Summary sheet

            Worksheet wsSummary = wbPrint.Worksheets.Add("Summary");
            wsSummary.PrintOptions.Header = "&C&B" + LeadBookName.Replace("#DATE#", (Convert.ToDateTime(dtSelected.Rows[0]["AllocationDate"])).ToString("yyyy/MM/dd"));
            wsSummary.PrintOptions.Footer = "&C&B" + (Convert.ToDateTime(dtSelected.Rows[0]["SalesStartDate"])).ToString("yyyy/MM/dd");

            Methods.SetExcelStandardWSOptions(wsSummary, Orientation.Landscape);

            #endregion Add the Summary sheet

            #endregion Setup Excel Document

            Methods.CopyExcelRegion(wsSummaryTemplate, 0, 0, 32, 7, wsSummary, 0, 0);

            #region Print Leads for Selected Agents

            const int coverRow = 0;
            int summaryRow = 4;

            foreach (DataRow drAgent in dtSelected.AsEnumerable())
            {
                Worksheet wsCoverTemplate = wbTemplate.Worksheets["Cover"];
                Worksheet wsLeadTemplate = wbTemplate.Worksheets["Lead"];

                int leadRow = 0;

                bool isRedeemedGiftBatch = Convert.ToBoolean(drAgent["IsRedeemedGiftBatch"]);
                if (isRedeemedGiftBatch)
                {
                    wsCoverTemplate = wbRedeemedGiftsTemplate.Worksheets["Cover"];
                    wsLeadTemplate = wbRedeemedGiftsTemplate.Worksheets["Lead"];
                    coverColumns = 16;
                    leadRow = 32;
                }
                else
                {
                    wsCoverTemplate = wbTemplate.Worksheets["Cover"];
                    wsLeadTemplate = wbTemplate.Worksheets["Lead"];
                    coverColumns = 15;
                    leadRow = 31;
                }

                #region Get Lead Data

                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@BatchID", drAgent["BatchID"]);
                parameters[1] = new SqlParameter("@AgentID", drAgent["AgentID"]);
                parameters[2] = new SqlParameter("@AllocationDate", drAgent["AllocationDate"]);

                DataSet ds = Methods.ExecuteStoredProcedure("spINGetLeadsForUserAndBatch", parameters);
                string strCampaignCode = dtCampaigns.Rows[0][0].ToString();

                DataTable dtLeadPrintData = ds.Tables[0];

                #if !TRAININGBUILD

                dtLeadPrintData = Methods.OrderRandomly(dtLeadPrintData.AsEnumerable()).CopyToDataTable();

                #endif

                #endregion Get Lead Data

                #region Create Leadbook Entry

                INLeadBook inLeadBook = new INLeadBook();
                inLeadBook.FKUserID = (long)drAgent["AgentID"];
                inLeadBook.FKINBatchID = (long)drAgent["BatchID"];
                inLeadBook.Description = nextMonday.ToString().Substring(0, 10) + "-" + drAgent["CampaignCode"] + "-" + drAgent["BatchCode"];
                inLeadBook.Save(_validationResult);
                long leadBookID = inLeadBook.ID;

                #endregion Create Leadbook Entry

                //setup leadsheet
                string strAgent = drAgent["SalesAgent"].ToString().Length > 31 ? drAgent["SalesAgent"].ToString().Substring(0, 31) : drAgent["SalesAgent"].ToString();

                lblAgent.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { lblAgent.Text = strAgent; }));

                Worksheet wsLeads = wbPrint.Worksheets.Add(strAgent);
                wsLeads.PrintOptions.StartPageNumber = 1;
                wsLeads.PrintOptions.Header = "&L&B" + strAgent + "&C&B" + LeadBookName.Replace("#DATE#", (Convert.ToDateTime(drAgent["AllocationDate"])).ToString("yyyy/MM/dd")) + "&R&BPage &P of &N";
                wsLeads.PrintOptions.Footer = "&C&B" + (Convert.ToDateTime(drAgent["SalesStartDate"])).ToString("yyyy/MM/dd");

                //populate leadsheet
                int[] leadCounter = new int[1] { 1 }; //array is needed so that the variable can be used in dispatcher
                //int leadCounter = 0;
                //int leadRow = 31;
                int leadsToPrint = Convert.ToInt32(dtLeadPrintData.Rows.Count);

                #region Cover Page

                //wbPrint.NamedReferences.Clear();

                Methods.CopyWorksheetOptionsFromTemplate(wsLeadTemplate, wsLeads, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false);
                Methods.CopyExcelRegion(wsCoverTemplate, 0, 0, coverRows, coverColumns, wsLeads, 0 + coverRow, 0);

                wsLeads.GetCell("G16").Value = drAgent["CampaignName"];
                wsLeads.GetCell("G17").Value = drAgent["SalesAgent"];
                wsLeads.GetCell("G18").Value = drAgent["Leads2Print"];
                wsLeads.GetCell("G20").Value = drAgent["CampaignName"] + " " + drAgent["BatchCode"];
                wsLeads.GetCell("G21").Value = (Convert.ToDateTime(drAgent["SalesStartDate"])).ToString("yyyy/MM/dd");
                wsLeads.GetCell("G22").Value = Convert.ToDateTime(drAgent["SalesStartDate"]).AddDays(63).ToString("yyyy/MM/dd");
               
                long pages = Convert.ToInt64(drAgent["Leads2Print"]) / leadsPerPage;
                long remainder = Convert.ToInt64(drAgent["Leads2Print"]) % leadsPerPage;
                if (remainder > 0)
                {
                    pages++;
                }
                wsLeads.GetCell("G19").Value = pages;

                #endregion Cover Page

                #region Insert Page Break

                //See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/208328091/comments
                //if (isRedeemedGiftBatch)
                //{
                //    leadRow++;
                //}
                wsLeads.PrintOptions.HorizontalPageBreaks.Add(new HorizontalPageBreak(leadRow));
                leadRow++;

                #endregion Insert Page Break

                #region Insert the leads

                foreach (DataRow drCurrentLead in dtLeadPrintData.Rows)
                {
                    Methods.CopyExcelRegion(wsLeadTemplate, 0, 0, templateLines + 1, leadColumnWidth, wsLeads, leadRow, 0);

                    // Row 1
                    wsLeads.GetCell(String.Format("A{0}", leadRow + 2)).Value = drCurrentLead["RefNo"];
                    wsLeads.GetCell(String.Format("B{0}", leadRow + 2)).Value = drCurrentLead["CallTime"];
                    wsLeads.GetCell(String.Format("C{0}", leadRow + 2)).Value = drCurrentLead["Title"];
                    wsLeads.GetCell(String.Format("D{0}", leadRow + 2)).Value = drCurrentLead["FirstName"];
                    wsLeads.GetCell(String.Format("E{0}", leadRow + 2)).Value = drCurrentLead["Surname"];
                    wsLeads.GetCell(String.Format("F{0}", leadRow + 2)).Value = drCurrentLead["TelCell"];
                    wsLeads.GetCell(String.Format("G{0}", leadRow + 2)).Value = drCurrentLead["TelWork"];
                    wsLeads.GetCell(String.Format("H{0}", leadRow + 2)).Value = drCurrentLead["TelHome"];
                    wsLeads.GetCell(String.Format("I{0}", leadRow + 2)).Value = drCurrentLead["Address"];
                    wsLeads.GetCell(String.Format("J{0}", leadRow + 2)).Value = drCurrentLead["PostalCode"];
                    wsLeads.GetCell(String.Format("K{0}", leadRow + 2)).Value = drCurrentLead["IDNumber"];
                    wsLeads.GetCell(String.Format("L{0}", leadRow + 2)).Value = drCurrentLead["Gift"];
                    wsLeads.GetCell(String.Format("M{0}", leadRow + 2)).Value = drCurrentLead["Referror"];
                    wsLeads.GetCell(String.Format("N{0}", leadRow + 2)).Value = drCurrentLead["Relationship"];
                    wsLeads.GetCell(String.Format("O{0}", leadRow + 2)).Value = drCurrentLead["CancerOption"];
                    wsLeads.GetCell(String.Format("P{0}", leadRow + 2)).Value = drCurrentLead["BankDetails"];

                    // Row 3
                    if (isRedeemedGiftBatch)
                    {
                        wsLeads.GetCell(String.Format("M{0}", leadRow + 4)).Value = drCurrentLead["SignedBy"];
                        wsLeads.GetCell(String.Format("P{0}", leadRow + 4)).Value = drCurrentLead["DateOfDelivery"];
                        wsLeads.GetCell(String.Format("Q{0}", leadRow + 4)).Value = drCurrentLead["ContractPremium"];
                    }
                    else
                    {
                        wsLeads.GetCell(String.Format("P{0}", leadRow + 4)).Value = drCurrentLead["ContractPremium"];
                    }

                    #region Add the lead to the lead book and mark it as printed

                    INImport inImport = new INImport(long.Parse(drCurrentLead["ImportID"].ToString()));
                    inImport.IsPrinted = 1;
                    inImport.Save(_validationResult);

                    //Keep import print order for Status Loading
                    INLeadBookImport inLeadBookImport = new INLeadBookImport();
                    inLeadBookImport.FKINLeadBookID = leadBookID;
                    inLeadBookImport.FKINImportID = long.Parse(drCurrentLead["ImportID"].ToString());
                    inLeadBookImport.Save(_validationResult);

                    #endregion Add the lead to the lead book and mark it as printed

                    //Update the progress indicator:
                    leadRow = leadRow + templateLines + verticalSpacingBetweenLeads;
                    if ((leadCounter[0] % leadsPerPage) == 0)
                    {
                        if (isRedeemedGiftBatch)
                        {
                            leadRow++;
                        }
                    }
                    lblLeadNo.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { lblLeadNo.Text = "Printing Lead #: " + leadCounter[0]; }));
                    leadCounter[0]++;
                }

                #endregion Insert the leads

                #region Populate conversion sheets

                DateTime dateReceived = Convert.ToDateTime(drAgent["SalesStartDate"]);
                int totalLeads = Convert.ToInt32(drAgent["Leads2Print"].ToString());
                int conversion2 = Convert.ToInt32(drAgent["Conversion2"]);
                int conversion4 = Convert.ToInt32(drAgent["Conversion4"]);
                int conversion6 = Convert.ToInt32(drAgent["Conversion6"]);
                string consultantConversionTabName;

                //Worksheet wsConversion = wbPrint.Worksheets.Add("Conversion " + strAgent);
                consultantConversionTabName = strAgent;

                if (consultantConversionTabName.Length > 26)
                {
                    consultantConversionTabName = consultantConversionTabName.Substring(0, 26);
                }

                Worksheet wsConversion = wbPrint.Worksheets.Add(consultantConversionTabName + " Conv");
                wsConversion.PrintOptions.Header = "&C&B" + strAgent + " Conversion";
                wsConversion.PrintOptions.Footer = "&C&B" + "Printed on: " + DateTime.Now.ToString("yyyy-MM-dd");
                Methods.SetExcelStandardWSOptions(wsConversion, Orientation.Landscape);

                // Copy the template to the worksheet.
                //Methods.CopyExcelRegion(wsConversionTemplate, 0, 0, 31, 14, wsConversion, 0, 0); 
                Methods.CopyExcelRegion(wsConversionTemplate, 0, 0, 28, 14, wsConversion, 0, 0); // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/207460137/comments

                // Add data to the worksheet. Campaign
                WorksheetCell wsConversionCell = wsConversion.Rows[1].Cells[4];
                string campaignName = drAgent[2].ToString();
                wsConversionCell.Value = campaignName;

                // Target %
                wsConversionCell = wsConversion.Rows[2].Cells[4];
                wsConversionCell.Value = conversion6 + " %";

                // Agent
                wsConversionCell = wsConversion.Rows[5].Cells[4];
                wsConversionCell.Value = strAgent;

                // Batch
                wsConversionCell = wsConversion.Rows[6].Cells[4];
                wsConversionCell.Value = drAgent["BatchCode"];

                // Total Leads
                wsConversionCell = wsConversion.Rows[7].Cells[4];
                wsConversionCell.Value = totalLeads;

                // Target Sales (This cell will multiply target% with total leads to get it's final value).
                wsConversionCell = wsConversion.Rows[8].Cells[4];
                wsConversionCell.Value = Math.Ceiling(((totalLeads * conversion6) / 100m)).ToString(); //wsConversionCell.Value = ((totalLeads * conversion6) / 100).ToString(); 

                // Date Received
                wsConversionCell = wsConversion.Rows[9].Cells[4];
                wsConversionCell.Value = dateReceived.ToString("yyyy-MM-dd");

                // Target %
                wsConversionCell = wsConversion.Rows[13].Cells[1];
                wsConversionCell.Value = conversion6 + " %";

                // Target Sales
                wsConversionCell = wsConversion.Rows[13].Cells[4];
                wsConversionCell.Value = Math.Ceiling(((totalLeads * conversion6) / 100m)).ToString(); //wsConversionCell.Value = ((totalLeads * conversion6) / 100).ToString();              

                #region NEW

                // See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/207460137/comments

                // Week 1
                wsConversionCell = wsConversion.Rows[25].Cells[1];
                wsConversionCell.Value = "Week 1 - " + dateReceived.AddDays(7).ToString("yyyy-MM-dd");

                // Week 2
                wsConversionCell = wsConversion.Rows[26].Cells[1];
                wsConversionCell.Value = "Week 2 - " + dateReceived.AddDays(14).ToString("yyyy-MM-dd");

                // Week 5
                wsConversionCell = wsConversion.Rows[27].Cells[1];
                wsConversionCell.Value = "Week 5 - " + dateReceived.AddDays(35).ToString("yyyy-MM-dd");

                // Week 1 Target %
                wsConversionCell = wsConversion.Rows[25].Cells[6];
                wsConversionCell.Value = conversion2 + " %";

                // Week 2 Target %
                wsConversionCell = wsConversion.Rows[26].Cells[6];
                wsConversionCell.Value = conversion4 + " %";

                // Week 5 Target %
                wsConversionCell = wsConversion.Rows[27].Cells[6];
                wsConversionCell.Value = conversion6 + " %";

                // Week 1 TargetSales
                wsConversionCell = wsConversion.Rows[25].Cells[8];
                wsConversionCell.Value = Math.Ceiling(((totalLeads * conversion2) / 100m)).ToString(); //wsConversionCell.Value = ((totalLeads * conversion2) / 100).ToString();

                // Week 2 TargetSales
                wsConversionCell = wsConversion.Rows[26].Cells[8];
                wsConversionCell.Value = Math.Ceiling(((totalLeads * conversion4) / 100m)).ToString(); //wsConversionCell.Value = ((totalLeads * conversion4) / 100).ToString();

                // Week 5 TargetSales
                wsConversionCell = wsConversion.Rows[27].Cells[8];
                wsConversionCell.Value = Math.Ceiling(((totalLeads * conversion6) / 100m)).ToString(); //wsConversionCell.Value = ((totalLeads * conversion6) / 100).ToString();

                #endregion NEW

                #endregion Populate conversion sheets

                #region Summary Entry

                if (summaryRow < summaryRow + 18)
                {
                    wsSummary.GetCell(String.Format("B{0}", summaryRow + 1)).Value = drAgent["FirstName"];
                    wsSummary.GetCell(String.Format("C{0}", summaryRow + 1)).Value = drAgent["LastName"];
                    wsSummary.GetCell(String.Format("D{0}", summaryRow + 1)).Value = drAgent["BatchCode"];
                    wsSummary.GetCell(String.Format("E{0}", summaryRow + 1)).Value = (Convert.ToDateTime(drAgent["SalesStartDate"])).ToString("yyyy/MM/dd");
                    wsSummary.GetCell(String.Format("F{0}", summaryRow + 1)).Value = drAgent["Leads2Print"];
                }

                summaryRow++;

                #endregion Summary Entry
            }

            #endregion Print Leads for Selected Agents

            #region save and display excel document

            string filePathAndName = GlobalSettings.UserFolder + LeadBookFileName + ".xlsx";
            wbPrint.SetCurrentFormat(WorkbookFormat.Excel2007);
            wbPrint.WindowOptions.SelectedWorksheet = wsSummary;

            //See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/209145461/comments#341417643
            if (filePathAndName.Contains(Environment.NewLine))
            {
                filePathAndName = filePathAndName.Replace(Environment.NewLine, "");
            }

            wbPrint.Save(filePathAndName);

            //display excel document
            Process.Start(filePathAndName);

            #endregion

            CommitTransaction(null);
        }

        private void PrintEliteLeads(DataTable dtSelectedItemsToPrint)
        {
            #region Variable Definitions

            long fkINCampaign;

            string campaignName = String.Empty;
            string campaignCode = String.Empty;
            string batches = String.Empty;
            string leadBookName = String.Empty;
            string leadBookFileName = String.Empty;

            string templateWorkbookName = String.Empty;

            DateTime nextMonday = Methods.NextWeekDay(DateTime.Now, DayOfWeek.Monday);

            #endregion Variable Definitions

            #region Assuming we're only printing for 1 campaign at a time, use row 1 column 1 of dtSelectedItemsToPrint to get the configs

            fkINCampaign = Convert.ToInt64(dtSelectedItemsToPrint.Rows[0]["CampaignID"]);
            campaignCode = dtSelectedItemsToPrint.Rows[0]["CampaignCode"].ToString().Trim();

            DataTable dtLeadBookConfiguration = Insure.INGetLeadbookConfigurationByCampaignID(fkINCampaign);
            templateWorkbookName = dtLeadBookConfiguration.Rows[0]["TemplateWorkbookName"].ToString();

            byte summarySheetMaxEntries = Convert.ToByte(dtLeadBookConfiguration.Rows[0]["SummarySheetMaxEntries"]);

            if (dtSelectedItemsToPrint.Rows.Count > summarySheetMaxEntries)
            {
                bool result = false;

                Dispatcher.Invoke(DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate
                {
                    string message = String.Format("You have {0} lead books to print. Only {1} of those items will appear on the summary sheet, even though all {0} lead books will be printed. Would you like to continue printing anyway?",
                    dtSelectedItemsToPrint.Rows.Count,
                    summarySheetMaxEntries);
                    INMessageBoxWindow2 messageBox = new INMessageBoxWindow2();
                    messageBox.buttonOK.Content = "Yes";
                    messageBox.buttonCancel.Content = "No";

                    var showMessageBox = ShowMessageBox(messageBox, message, "Too many items selected", ShowMessageType.Exclamation);
                    result = showMessageBox != null && (bool)showMessageBox;
                });

                if (!result)
                {
                    Database.CancelTransactions();
                    return;
                }
            }
            
            #endregion Assuming we're only printing for 1 campaign at a time, use row 1 column 1 of dtSelectedItemsToPrint to get the configs

            #region Set the lead book name using a comma-separated string of the distinct batch codes in dtSelectedItemsToPrint

            DataTable dtBatches = Methods.GroupBy("BatchCode", "BatchCode", dtSelectedItemsToPrint);

            int batchCount = 0;
            foreach (DataRow dr in dtBatches.AsEnumerable())
            {
                batchCount++;

                if (batches.Trim().Length > 0)
                {
                    batches = String.Format("{0}, {1}", batches, dr[0].ToString());
                }
                else
                {
                    batches = dr[0].ToString();
                }
            }

            //batches = batches.Trim().TrimEnd(',');

            leadBookName = String.Format("{0} Batch {1} (#DATE#)", campaignCode, batches/*, DateTime.Now.ToString("yyyy-MM-dd")*/);
            leadBookFileName = String.Format("{0} {1})", leadBookName.TrimEnd(')'), DateTime.Now.ToString("HH.mm.ss")).Replace("#DATE#", DateTime.Now.ToString("yyyy-MM-dd"));

            #endregion Set the lead book name using a comma-separated string of the distinct batch codes in dtSelectedItemsToPrint

            #region Define the Excel workbooks (the template and the resulting lead book) and which template sheets to use for the summary, cover, actual leads and coversion

            Workbook wbTemplate = Methods.DefineTemplateWorkbook(String.Format("/Templates/{0}", templateWorkbookName));
            Workbook wbPrint = new Workbook(WorkbookFormat.Excel2007);

            #endregion Define the Excel workbooks (the template and the resulting lead book) and which template sheets to use for the summary, cover, actual leads and coversion

            #region Add the summary sheet

            AddSummarySheet(wbTemplate, wbPrint, dtSelectedItemsToPrint, dtLeadBookConfiguration.Rows[0], leadBookName);

            #endregion Add the summary sheet

            #region For each selected item, generate the cover, leads and conversion sheets

            foreach (DataRow drAgent in dtSelectedItemsToPrint.AsEnumerable())
            {
                #region Add the current sales consultant's cover sheet and leads

                //AddIndividualSalesConsultantCoverAndLeads(wbTemplate, wbPrint, drAgent, dtLeadBookConfiguration.Rows[0], leadBookName);
                AddIndividualSalesConsultantCoverAndLeads(wbPrint, drAgent, dtLeadBookConfiguration.Rows[0], leadBookName);
                
                #endregion Add the current sales consultant's cover sheet and leads

                #region Add the current sales consultant's conversion sheet

                AddIndividualSalesConsultantConversionSheet(wbTemplate, wbPrint, drAgent, dtLeadBookConfiguration.Rows[0]);

                #endregion Add the current sales consultant's conversion sheet

                wbPrint.NamedReferences.Clear();
            }
            #endregion For each selected item, generate the cover, leads and conversion sheets

            #region Save and open the resulting workbook

            string filePathAndName = String.Format("{0}{1}.xlsx", GlobalSettings.UserFolder, leadBookFileName);

            //See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/209145461/comments#341417643
            if (filePathAndName.Contains(Environment.NewLine))
            {
                filePathAndName = filePathAndName.Replace(Environment.NewLine, "");
            }

            wbPrint.Save(filePathAndName);
            Process.Start(filePathAndName);

            #endregion Save and open the resulting workbook

            #region Finally, commit the transaction

            CommitTransaction(null);

            #endregion Finally, commit the transaction
        }

        private void PrintSheMaccBaseLeads(DataTable dtSelectedItemsToPrint)
        {
            #region Variable Definitions

            long fkINCampaign;

            string campaignName = String.Empty;
            string campaignCode = String.Empty;
            string batches = String.Empty;
            string leadBookName = String.Empty;
            string leadBookFileName = String.Empty;

            string templateWorkbookName = String.Empty;

            DateTime nextMonday = Methods.NextWeekDay(DateTime.Now, DayOfWeek.Monday);

            #endregion Variable Definitions

            #region Assuming we're only printing for 1 campaign at a time, use row 1 column 1 of dtSelectedItemsToPrint to get the configs

            fkINCampaign = Convert.ToInt64(dtSelectedItemsToPrint.Rows[0]["CampaignID"]);
            campaignCode = dtSelectedItemsToPrint.Rows[0]["CampaignCode"].ToString().Trim();

            DataTable dtLeadBookConfiguration = Insure.INGetLeadbookConfigurationSheMaccBase(fkINCampaign);
            templateWorkbookName = dtLeadBookConfiguration.Rows[0]["TemplateWorkbookName"].ToString();

            byte summarySheetMaxEntries = Convert.ToByte(dtLeadBookConfiguration.Rows[0]["SummarySheetMaxEntries"]);

            if (dtSelectedItemsToPrint.Rows.Count > summarySheetMaxEntries)
            {
                bool result = false;

                Dispatcher.Invoke(DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate
                {
                    string message = String.Format("You have {0} lead books to print. Only {1} of those items will appear on the summary sheet, even though all {0} lead books will be printed. Would you like to continue printing anyway?",
                    dtSelectedItemsToPrint.Rows.Count,
                    summarySheetMaxEntries);
                    INMessageBoxWindow2 messageBox = new INMessageBoxWindow2();
                    messageBox.buttonOK.Content = "Yes";
                    messageBox.buttonCancel.Content = "No";

                    var showMessageBox = ShowMessageBox(messageBox, message, "Too many items selected", ShowMessageType.Exclamation);
                    result = showMessageBox != null && (bool)showMessageBox;
                });

                if (!result)
                {
                    Database.CancelTransactions();
                    return;
                }
            }

            #endregion Assuming we're only printing for 1 campaign at a time, use row 1 column 1 of dtSelectedItemsToPrint to get the configs

            #region Set the lead book name using a comma-separated string of the distinct batch codes in dtSelectedItemsToPrint

            DataTable dtBatches = Methods.GroupBy("BatchCode", "BatchCode", dtSelectedItemsToPrint);

            int batchCount = 0;
            foreach (DataRow dr in dtBatches.AsEnumerable())
            {
                batchCount++;

                if (batches.Trim().Length > 0)
                {
                    batches = String.Format("{0}, {1}", batches, dr[0].ToString());
                }
                else
                {
                    batches = dr[0].ToString();
                }
            }

            //batches = batches.Trim().TrimEnd(',');

            leadBookName = String.Format("{0} Batch {1} (#DATE#)", campaignCode, batches/*, DateTime.Now.ToString("yyyy-MM-dd")*/);
            leadBookFileName = String.Format("{0} {1})", leadBookName.TrimEnd(')'), DateTime.Now.ToString("HH.mm.ss")).Replace("#DATE#", DateTime.Now.ToString("yyyy-MM-dd"));

            #endregion Set the lead book name using a comma-separated string of the distinct batch codes in dtSelectedItemsToPrint

            #region Define the Excel workbooks (the template and the resulting lead book) and which template sheets to use for the summary, cover, actual leads and coversion

            Workbook wbTemplate = Methods.DefineTemplateWorkbook(String.Format("/Templates/{0}", templateWorkbookName));
            Workbook wbPrint = new Workbook(WorkbookFormat.Excel2007);

            #endregion Define the Excel workbooks (the template and the resulting lead book) and which template sheets to use for the summary, cover, actual leads and coversion

            #region Add the summary sheet

            AddSummarySheet(wbTemplate, wbPrint, dtSelectedItemsToPrint, dtLeadBookConfiguration.Rows[0], leadBookName);

            #endregion Add the summary sheet

            #region For each selected item, generate the cover, leads and conversion sheets

            foreach (DataRow drAgent in dtSelectedItemsToPrint.AsEnumerable())
            {
                #region Add the current sales consultant's cover sheet and leads

                //AddIndividualSalesConsultantCoverAndLeads(wbTemplate, wbPrint, drAgent, dtLeadBookConfiguration.Rows[0], leadBookName);
                AddIndividualSalesConsultantCoverAndLeads(wbPrint, drAgent, dtLeadBookConfiguration.Rows[0], leadBookName);

                #endregion Add the current sales consultant's cover sheet and leads

                #region Add the current sales consultant's conversion sheet

                AddIndividualSalesConsultantConversionSheet(wbTemplate, wbPrint, drAgent, dtLeadBookConfiguration.Rows[0]);

                #endregion Add the current sales consultant's conversion sheet

                wbPrint.NamedReferences.Clear();
            }
            #endregion For each selected item, generate the cover, leads and conversion sheets

            #region Save and open the resulting workbook

            string filePathAndName = String.Format("{0}{1}.xlsx", GlobalSettings.UserFolder, leadBookFileName);

            //See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/209145461/comments#341417643
            if (filePathAndName.Contains(Environment.NewLine))
            {
                filePathAndName = filePathAndName.Replace(Environment.NewLine, "");
            }

            wbPrint.Save(filePathAndName);
            Process.Start(filePathAndName);

            #endregion Save and open the resulting workbook

            #region Finally, commit the transaction

            CommitTransaction(null);

            #endregion Finally, commit the transaction
        }

        private void PrintUpgradeLeads(DataTable dtSelectedItemsToPrint)
        {
            #region Variable Definitions

            long fkINCampaign;

            string campaignName = String.Empty;
            string campaignCode = String.Empty;
            string batches = String.Empty;
            string leadBookName = String.Empty;
            string leadBookFileName = String.Empty;

            string templateWorkbookName = String.Empty;

            DateTime nextMonday = Methods.NextWeekDay(DateTime.Now, DayOfWeek.Monday);

            #endregion Variable Definitions

            #region Assuming we're only printing for 1 campaign at a time, use row 1 column 1 of dtSelectedItemsToPrint to get the configs

            fkINCampaign = Convert.ToInt64(dtSelectedItemsToPrint.Rows[0]["CampaignID"]);
            campaignCode = dtSelectedItemsToPrint.Rows[0]["CampaignCode"].ToString().Trim();

            DataTable dtLeadBookConfiguration = Insure.INGetLeadbookConfigurationByCampaignID(fkINCampaign);
            templateWorkbookName = dtLeadBookConfiguration.Rows[0]["TemplateWorkbookName"].ToString();

            byte summarySheetMaxEntries = Convert.ToByte(dtLeadBookConfiguration.Rows[0]["SummarySheetMaxEntries"]);

            if (dtSelectedItemsToPrint.Rows.Count > summarySheetMaxEntries)
            {
                bool result = false;

                Dispatcher.Invoke(DispatcherPriority.Normal, (System.Threading.ThreadStart)delegate
                {
                    string message = String.Format("You have {0} lead books to print. Only {1} of those items will appear on the summary sheet, even though all {0} lead books will be printed. Would you like to continue printing anyway?",
                    dtSelectedItemsToPrint.Rows.Count,
                    summarySheetMaxEntries);
                    INMessageBoxWindow2 messageBox = new INMessageBoxWindow2();
                    messageBox.buttonOK.Content = "Yes";
                    messageBox.buttonCancel.Content = "No";

                    var showMessageBox = ShowMessageBox(messageBox, message, "Too many items selected", ShowMessageType.Exclamation);
                    result = showMessageBox != null && (bool)showMessageBox;
                });

                if (!result)
                {
                    Database.CancelTransactions();
                    return;
                }
            }

            #endregion Assuming we're only printing for 1 campaign at a time, use row 1 column 1 of dtSelectedItemsToPrint to get the configs

            #region Set the lead book name using a comma-separated string of the distinct batch codes in dtSelectedItemsToPrint

            DataTable dtBatches = Methods.GroupBy("BatchCode", "BatchCode", dtSelectedItemsToPrint);

            int batchCount = 0;
            foreach (DataRow dr in dtBatches.AsEnumerable())
            {
                batchCount++;

                if (batches.Trim().Length > 0)
                {
                    batches = String.Format("{0}, {1}", batches, dr[0].ToString());
                }
                else
                {
                    batches = dr[0].ToString();
                }
            }

            //batches = batches.Trim().TrimEnd(',');

            leadBookName = String.Format("{0} Batch {1} (#DATE#)", campaignCode, batches/*, DateTime.Now.ToString("yyyy-MM-dd")*/);
            leadBookFileName = String.Format("{0} {1})", leadBookName.TrimEnd(')'), DateTime.Now.ToString("HH.mm.ss")).Replace("#DATE#", DateTime.Now.ToString("yyyy-MM-dd"));

            #endregion Set the lead book name using a comma-separated string of the distinct batch codes in dtSelectedItemsToPrint

            #region Define the Excel workbooks (the template and the resulting lead book) and which template sheets to use for the summary, cover, actual leads and coversion

            Workbook wbTemplate = Methods.DefineTemplateWorkbook(String.Format("/Templates/{0}", templateWorkbookName));
            Workbook wbPrint = new Workbook(WorkbookFormat.Excel2007);

            #endregion Define the Excel workbooks (the template and the resulting lead book) and which template sheets to use for the summary, cover, actual leads and coversion

            #region Add the summary sheet

            AddSummarySheet(wbTemplate, wbPrint, dtSelectedItemsToPrint, dtLeadBookConfiguration.Rows[0], leadBookName);

            #endregion Add the summary sheet

            #region For each selected item, generate the cover, leads and conversion sheets

            foreach (DataRow drAgent in dtSelectedItemsToPrint.AsEnumerable())
            {
                #region Add the current sales consultant's cover sheet and leads

                //AddIndividualSalesConsultantCoverAndLeads(wbTemplate, wbPrint, drAgent, dtLeadBookConfiguration.Rows[0], leadBookName);
                AddIndividualSalesConsultantCoverAndLeads(wbPrint, drAgent, dtLeadBookConfiguration.Rows[0], leadBookName);

                #endregion Add the current sales consultant's cover sheet and leads

                #region Add the current sales consultant's conversion sheet

                AddIndividualSalesConsultantConversionSheet(wbTemplate, wbPrint, drAgent, dtLeadBookConfiguration.Rows[0]);

                #endregion Add the current sales consultant's conversion sheet

                wbPrint.NamedReferences.Clear();
            }
            #endregion For each selected item, generate the cover, leads and conversion sheets

            #region Save and open the resulting workbook

            string filePathAndName = String.Format("{0}{1}.xlsx", GlobalSettings.UserFolder, leadBookFileName);

            //See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/209145461/comments#341417643
            if (filePathAndName.Contains(Environment.NewLine))
            {
                filePathAndName = filePathAndName.Replace(Environment.NewLine, "");
            }

            wbPrint.Save(filePathAndName);
            Process.Start(filePathAndName);

            #endregion Save and open the resulting workbook

            #region Finally, commit the transaction

            CommitTransaction(null);

            #endregion Finally, commit the transaction
        }

        private void AddSummarySheet(Workbook wbTemplate, Workbook wbPrint, DataTable dtSelectedItemsToPrint, DataRow drLeadBookConfiguration, string heading)
        {
            #region Variable declarations and initializations

            string summaryTemplateWorksheetName = drLeadBookConfiguration["SummaryTemplateWorksheetName"].ToString();
            byte summaryTemplateRowSpan = Convert.ToByte(drLeadBookConfiguration["SummaryTemplateRowSpan"]);
            byte summaryTemplateColumnSpan = Convert.ToByte(drLeadBookConfiguration["SummaryTemplateColumnSpan"]);
            byte summarySheetMaxEntries = Convert.ToByte(drLeadBookConfiguration["SummarySheetMaxEntries"]);

            byte summarySheetEntryTemplateRowSpan = Convert.ToByte(drLeadBookConfiguration["SummarySheetEntryTemplateRowSpan"]);
            byte summarySheetEntryTemplateColumnSpan = Convert.ToByte(drLeadBookConfiguration["SummarySheetEntryTemplateColumnSpan"]);

            byte summarySheetEntryTemplateOriginRow = Convert.ToByte(drLeadBookConfiguration["SummarySheetEntryTemplateOriginRow"]);
            byte summarySheetEntryTemplateOriginColumn = Convert.ToByte(drLeadBookConfiguration["SummarySheetEntryTemplateOriginColumn"]);

            int summarySheetRowIndex = summarySheetEntryTemplateOriginRow;
            int rowCount = 0;

            #endregion Variable declarations and initializations

            #region Defining the summary worksheet

            Worksheet wsSummary = wbPrint.Worksheets.Add("Summary");
            Worksheet wsSummaryTemplate = wbTemplate.Worksheets[summaryTemplateWorksheetName];
            Methods.CopyWorksheetOptionsFromTemplate(wsSummaryTemplate, wsSummary);
            //wsSummary.PrintOptions.Header = "&C&B" + heading;
            wsSummary.PrintOptions.Header = "&C&B" + heading.Replace("#DATE#", (Convert.ToDateTime(dtSelectedItemsToPrint.Rows[0]["AllocationDate"])).ToString("yyyy/MM/dd"));
            //wsSummary.PrintOptions.Footer = "&C&B" + Methods.NextWeekDay(DateTime.Now, DayOfWeek.Monday).ToString("yyyy/MM/dd");
            wsSummary.PrintOptions.Footer = "&C&B" + (Convert.ToDateTime(dtSelectedItemsToPrint.Rows[0]["SalesStartDate"])).ToString("yyyy/MM/dd");

            #endregion Defining the summary worksheet

            #region Apply the entire summary sheet template formatting to the newly-added sheet

            //wbPrint.NamedReferences.Clear();
            Methods.CopyExcelRegion(wsSummaryTemplate, 0, 0, summaryTemplateRowSpan, summaryTemplateColumnSpan, wsSummary, 0, 0);
            wbPrint.NamedReferences.Clear();

            #endregion Apply the entire summary sheet template formatting to the newly-added sheet

            #region Add the contents

            foreach (DataRow row in dtSelectedItemsToPrint.Rows)
            {
                rowCount++;
                if (rowCount <= summarySheetMaxEntries)
                {

                    Methods.CopyExcelRegion(wsSummaryTemplate,
                        summarySheetEntryTemplateOriginRow,
                        summarySheetEntryTemplateOriginColumn,
                        summarySheetEntryTemplateRowSpan,
                        summarySheetEntryTemplateColumnSpan,
                        wsSummary,
                        summarySheetRowIndex - 1,
                        summarySheetEntryTemplateOriginColumn);

                    wsSummary.GetCell(String.Format("B{0}", summarySheetRowIndex)).Value = row["FirstName"];
                    wsSummary.GetCell(String.Format("C{0}", summarySheetRowIndex)).Value = row["LastName"];
                    wsSummary.GetCell(String.Format("D{0}", summarySheetRowIndex)).Value = row["BatchCode"];
                    wsSummary.GetCell(String.Format("E{0}", summarySheetRowIndex)).Value = (Convert.ToDateTime(row["SalesStartDate"])).ToString("yyyy/MM/dd");
                    wsSummary.GetCell(String.Format("F{0}", summarySheetRowIndex)).Value = row["Leads2Print"];

                    //rowCount++;
                    summarySheetRowIndex++;
                }
                else
                {
                    continue;
                }

                wbPrint.NamedReferences.Clear();
            }

            #endregion Add the contents
        }

        //private void AddIndividualSalesConsultantCoverAndLeads(Workbook wbTemplate, Workbook wbPrint, DataRow drSelectedEntryPrint, DataRow drLeadBookConfiguration, string header)
        private void AddIndividualSalesConsultantCoverAndLeads(Workbook wbPrint, DataRow drSelectedEntryPrint, DataRow drLeadBookConfiguration, string header)
        {
            #region Variable declarations and initializations

            // TODO: I had to hard-code this, because with redeemed gifts, lead books are configured by batch and not per campaign. ARGHHHHH!!!!!
            string templateWorkbookName = drLeadBookConfiguration["TemplateWorkbookName"].ToString();
            byte isRedeemedGiftBatch = Convert.ToByte(drSelectedEntryPrint["IsRedeemedGiftBatch"]);
            bool includeLatestLeadActivityValue = false;

            string campaignName = drSelectedEntryPrint["CampaignName"].ToString();
            string campaignCode = drSelectedEntryPrint["CampaignCode"].ToString();
            if ((campaignCode == "PLCBE") ||
                (campaignCode == "PLFDB") ||
                (campaignCode == "PLCBEC") ||
                (campaignCode == "PLCBER") ||
                (campaignCode == "PLMMBE") ||
                (campaignCode == "PLMCB") ||
                (campaignCode == "PLMMCB") ||
                (campaignCode == "PLFDCB"))
            {
                if (isRedeemedGiftBatch == 1)
                {
                    templateWorkbookName = "PrintTemplateEliteRedeemedGifts.xlsx";
                }
                else if (isRedeemedGiftBatch == 0)
                {
                    //templateWorkbookName = "PrintTemplateEliteNonRedeemedGifts.xlsx";
                    if (DateTime.Now >= Convert.ToDateTime("2018-09-12 00:00:00"))
                    {
                        templateWorkbookName = "PrintTemplateEliteNonRedeemedGiftsWLite.xlsx";
                    }  
                    else
                    {
                        templateWorkbookName = "PrintTemplateEliteNonRedeemedGifts.xlsx";
                    }
                }
                else
                {
                    templateWorkbookName = "PrintTemplateElite.xlsx";
                }
            }
            else if (campaignCode.EndsWith("Min"))
            {
                templateWorkbookName = "PrintTemplateMining.xlsx";
                includeLatestLeadActivityValue = true;
            }
            else
            {
                templateWorkbookName = "PrintTemplateElite.xlsx";

            }

            Workbook wbTemplate = Methods.DefineTemplateWorkbook(String.Format("/Templates/{0}", templateWorkbookName));

            long fkINBatchID = Convert.ToInt64(drSelectedEntryPrint["BatchID"]);
            long fkUserID = Convert.ToInt64(drSelectedEntryPrint["AgentID"]);
            DateTime allocationDate = Convert.ToDateTime(drSelectedEntryPrint["AllocationDate"]);

            // Cover-Specific Configuration Values
            string coverTemplateWorksheetName = drLeadBookConfiguration["CoverTemplateWorksheetName"].ToString();
            byte coverTemplateColumnSpan = Convert.ToByte(drLeadBookConfiguration["CoverTemplateColumnSpan"]);
            byte coverTemplateRowSpan = Convert.ToByte(drLeadBookConfiguration["CoverTemplateRowSpan"]);
            byte coverOriginRow = Convert.ToByte(drLeadBookConfiguration["CoverOriginRow"]);
            byte coverOriginColumn = Convert.ToByte(drLeadBookConfiguration["CoverOriginColumn"]);
            byte coverFirstValueRowIndex = Convert.ToByte(drLeadBookConfiguration["CoverFirstValueRowIndex"]);

            // Lead-Specific Configuration Values
            string leadTemplateWorksheetName = drLeadBookConfiguration["LeadTemplateWorksheetName"].ToString();
            byte leadTemplateRowSpan = Convert.ToByte(drLeadBookConfiguration["LeadTemplateRowSpan"]);
            byte leadTemplateColumnSpan = Convert.ToByte(drLeadBookConfiguration["LeadTemplateColumnSpan"]);
            byte leadTemplateOriginColumn = Convert.ToByte(drLeadBookConfiguration["LeadTemplateOriginColumn"]);
            byte leadTemplateOriginRow = Convert.ToByte(drLeadBookConfiguration["LeadTemplateOriginRow"]);

            byte leadsPerPage = Convert.ToByte(drLeadBookConfiguration["LeadsPerPage"]);
            byte pageBreakRowIndex = Convert.ToByte(drLeadBookConfiguration["PageBreakRowIndex"]);
            byte firstLeadRowIndex = Convert.ToByte(drLeadBookConfiguration["FirstLeadRowIndex"]);
            byte verticalSpacingBetweenLeads = Convert.ToByte(drLeadBookConfiguration["VerticalSpacingBetweenLeads"]);
            int additionalRowsToAddAfterBottomLeadOnSheet = Convert.ToInt16(drLeadBookConfiguration["AdditionalRowsToAddAfterBottomLeadOnSheet"]);

            string salesAgent = drSelectedEntryPrint["SalesAgent"].ToString();
            string leadsWorksheetName = Methods.ParseWorksheetName(wbPrint, salesAgent);

            int rowIndex = firstLeadRowIndex;
            int insertedLeadCount = 0;

            #region Determining whether or not the Redeem Target figure should appear on the cover of the lead book

            bool includeRedeemTarget = (isRedeemedGiftBatch == 0);
            if (includeRedeemTarget)
            {
                coverFirstValueRowIndex = 21;
                //leadTemplateOriginRow--;
                //pageBreakRowIndex--;
            }

            #endregion Determining whether or not the Redeem Target figure should appear on the cover of the lead book

            #endregion Variable declarations and initializations

            #region Defining the cover worksheet

            Worksheet wsCoverTemplate = wbTemplate.Worksheets[coverTemplateWorksheetName];
            Worksheet wsLeadTemplate = wbTemplate.Worksheets[leadTemplateWorksheetName];
            Worksheet wsLeads = wbPrint.Worksheets.Add(leadsWorksheetName);

            wsLeads.PrintOptions.StartPageNumber = 1;
            wsLeads.PrintOptions.PageNumbering = PageNumbering.UseStartPageNumber;
            wsLeads.PrintOptions.Header = "&L&B" + salesAgent + "&C&B" + header.Replace("#DATE#", (Convert.ToDateTime(drSelectedEntryPrint["AllocationDate"])).ToString("yyyy/MM/dd")) + "&R&BPage &P of &N";
            //wsLeads.PrintOptions.Footer = "&C&B" + Methods.NextWeekDay(DateTime.Now, DayOfWeek.Monday).ToString("yyyy/MM/dd");
            wsLeads.PrintOptions.Footer = "&C&B" + (Convert.ToDateTime(drSelectedEntryPrint["SalesStartDate"])).ToString("yyyy/MM/dd");

            #endregion Defining the cover worksheet

            #region Apply the entire summary sheet template formatting to the newly-added sheet

            Methods.CopyWorksheetOptionsFromTemplate(wsLeadTemplate, wsLeads, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, true);
            Methods.CopyExcelRegion(wsCoverTemplate, 0, 0, coverTemplateRowSpan, coverTemplateColumnSpan, wsLeads, 0, 0);

            // Because the default row height differs, it is necessary to explicitly set these rows' heights
            wsLeads.Rows[coverFirstValueRowIndex - 1].Height = 464;
            wsLeads.Rows[coverFirstValueRowIndex].Height = 464;
            wsLeads.Rows[coverFirstValueRowIndex + 1].Height = 464;
            wsLeads.Rows[coverFirstValueRowIndex + 2].Height = 464;
            wsLeads.Rows[coverFirstValueRowIndex + 3].Height = 464;
            wsLeads.Rows[coverFirstValueRowIndex + 4].Height = 464;
            wsLeads.Rows[coverFirstValueRowIndex + 5].Height = 464;
            //if (includeRedeemTarget)
            //{
            //    wsLeads.Rows[coverFirstValueRowIndex + 6].Height = 464;
            //}

            #endregion Apply the entire summary sheet template formatting to the newly-added sheet

            #region Populate the workbook cover

            lblAgent.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { lblAgent.Text = drSelectedEntryPrint["SalesAgent"].ToString(); }));

            wsLeads.GetCell(String.Format("AG{0}", coverFirstValueRowIndex)).Value = drSelectedEntryPrint["CampaignName"];
            wsLeads.GetCell(String.Format("AG{0}", coverFirstValueRowIndex + 1)).Value = drSelectedEntryPrint["SalesAgent"];
            wsLeads.GetCell(String.Format("AG{0}", coverFirstValueRowIndex + 2)).Value = drSelectedEntryPrint["Leads2Print"];
            wsLeads.GetCell(String.Format("AG{0}", coverFirstValueRowIndex + 3)).Value = Math.Ceiling((Convert.ToDecimal(drSelectedEntryPrint["Leads2Print"]) / leadsPerPage));
            wsLeads.GetCell(String.Format("AG{0}", coverFirstValueRowIndex + 4)).Value = String.Format("{0} {1}", drSelectedEntryPrint["CampaignName"].ToString().Trim(), drSelectedEntryPrint["BatchCode"].ToString().Trim());
            wsLeads.GetCell(String.Format("AG{0}", coverFirstValueRowIndex + 5)).Value = Convert.ToDateTime(drSelectedEntryPrint["SalesStartDate"]).ToString("yyyy/MM/dd");
            wsLeads.GetCell(String.Format("AG{0}", coverFirstValueRowIndex + 6)).Value = Convert.ToDateTime(drSelectedEntryPrint["ExpiryDate"]).ToString("yyyy/MM/dd");
            if (includeRedeemTarget)
            {
                wsLeads.GetCell(String.Format("AG{0}", coverFirstValueRowIndex + 7)).Value = drSelectedEntryPrint["RedeemTarget"];
            }
            
            #endregion Populate the workbook cover

            #region Insert the page break

            //See https://udmint.basecamphq.com/projects/10327065-udm-insure/todo_items/208328091/comments     
            wsLeads.PrintOptions.HorizontalPageBreaks.Add(new HorizontalPageBreak(pageBreakRowIndex));

            #endregion Insert the page break

            #region Get the lead data from the database

            DataTable dtLeadData = Insure.INGetLeadsForUserAndBatchGeneric(fkINBatchID, fkUserID, allocationDate);

            #if TRAININGBUILD

            DataView dv = dtLeadData.DefaultView;
            dv.Sort = "RefNo ASC";
            dtLeadData = dv.ToTable();

            #endif

            #endregion Get the lead data from the database

            #region Create Leadbook Entry

            INLeadBook inLeadBook = new INLeadBook();
            inLeadBook.FKUserID = (long)drSelectedEntryPrint["AgentID"];
            inLeadBook.FKINBatchID = (long)drSelectedEntryPrint["BatchID"];
            inLeadBook.Description = String.Format("{0}-{1}-{2}",
                (Convert.ToDateTime(drSelectedEntryPrint["AllocationDate"])).ToString("yyyy/MM/dd"), //Methods.NextWeekDay(DateTime.Now, DayOfWeek.Monday).ToString().Substring(0, 10),
                drSelectedEntryPrint["CampaignCode"],
                drSelectedEntryPrint["BatchCode"]);
            inLeadBook.Save(_validationResult);
            long fkINLeadBookID = inLeadBook.ID;

            #endregion Create Leadbook Entry

            #region Insert the leads

            //wbPrint.NamedReferences.Clear();

            foreach (DataRow row in dtLeadData.Rows)
            {
                Methods.CopyExcelRegion(wsLeadTemplate, leadTemplateOriginRow, leadTemplateOriginColumn, leadTemplateRowSpan, leadTemplateColumnSpan, wsLeads, rowIndex, leadTemplateOriginColumn);
                long fkINImportID = Convert.ToInt64(row["ImportID"]);

                foreach (DataColumn column in dtLeadData.Columns)
                {
                    // Omissions:
                    if (includeLatestLeadActivityValue)
                    {
                        if (column.ColumnName.Trim() != "ImportID")
                        {
                            wsLeads.GetCell(column.ColumnName).Value = row[column.ColumnName];
                        }
                    }
                    else
                    {
                        if ((column.ColumnName.Trim() != "ImportID") &&
                            (column.ColumnName.Trim() != "Gift1") &&
                            (column.ColumnName.Trim() != "Gift2") &&
                            (column.ColumnName.Trim() != "Gift3") &&
                            (column.ColumnName.Trim() != "LatestLeadActivity"))
                        {
                            wsLeads.GetCell(column.ColumnName).Value = row[column.ColumnName];
                        }

                        if (column?.ColumnName?.Trim() == "CallTime")
                        {
                            FormattedString formattedString =  new FormattedString(wsLeads.GetCell("CallTime").Value?.ToString());

                            if (formattedString?.ToString()?.Contains("SMS Date") == true)
                            {
                                wsLeads.GetCell("CallTime").Value = formattedString;
                                int strlength = formattedString.ToString().Length;

                                int start = formattedString.ToString().IndexOf("SMS");

                                FormattedStringFont font1 = formattedString.GetFont(start, 8);
                                font1.Bold = ExcelDefaultableBoolean.True;

                                FormattedStringFont font2 = formattedString.GetFont(start + 8, strlength - (start + 8));
                                font2.Height = 140;
                            }

                        }
                    }
                }

                insertedLeadCount++;
                lblLeadNo.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { lblLeadNo.Text = String.Format("Printing Lead #: {0}", insertedLeadCount); }));

                wbPrint.NamedReferences.Clear();
                rowIndex = rowIndex + leadTemplateRowSpan + verticalSpacingBetweenLeads;

                if ((insertedLeadCount % leadsPerPage) == 0)
                {
                    //rowIndex += additionalRowsToAddAfterBottomLeadOnSheet;
                    wsLeads.PrintOptions.HorizontalPageBreaks.Add(new HorizontalPageBreak(rowIndex));
                }

                #region Mark the current lead as printed

                INImport inImport = new INImport(fkINImportID);
                inImport.IsPrinted = 1;
                inImport.Save(_validationResult);

                #endregion Mark the current lead as printed

                #region Add the lead to the lead book

                //Keep import print order for Status Loading
                INLeadBookImport inLeadBookImport = new INLeadBookImport();
                inLeadBookImport.FKINLeadBookID = fkINLeadBookID;
                inLeadBookImport.FKINImportID = fkINImportID;
                inLeadBookImport.Save(_validationResult);

                #endregion Add the lead to the lead book
            }

            #endregion Insert the leads

        }

        private void AddIndividualSalesConsultantConversionSheet(Workbook wbTemplate, Workbook wbPrint, DataRow drSelectedEntryPrint, DataRow drLeadBookConfiguration)
        {
            #region Variable Definitions and initializations

            string conversionTemplateWorksheetName = drLeadBookConfiguration["ConversionTemplateWorksheetName"].ToString();
            byte conversionTemplateSheetRowSpan = Convert.ToByte(drLeadBookConfiguration["ConversionTemplateSheetRowSpan"]);
            byte conversionTemplateSheetColumnSpan = Convert.ToByte(drLeadBookConfiguration["ConversionTemplateSheetColumnSpan"]);
            byte conversionTemplateSheetOriginRow = Convert.ToByte(drLeadBookConfiguration["ConversionTemplateSheetOriginRow"]);
            byte conversionTemplateSheetOriginColumn = Convert.ToByte(drLeadBookConfiguration["ConversionTemplateSheetOriginColumn"]);

            string salesAgent = drSelectedEntryPrint["SalesAgent"].ToString();
            string conversionWorksheetName = Methods.ParseWorksheetName(wbPrint, salesAgent, " ", "Conv");

            #endregion Variable Definitions and initializations

            #region Defining the summary worksheet

            Worksheet wsConversion = wbPrint.Worksheets.Add(conversionWorksheetName);
            Worksheet wsConversionTemplate = wbTemplate.Worksheets[conversionTemplateWorksheetName];
            Methods.CopyWorksheetOptionsFromTemplate(wsConversionTemplate, wsConversion, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);
            wsConversion.PrintOptions.Header = "&C&B" + salesAgent + " Conversion";
            wsConversion.PrintOptions.Footer = "&C&B" + "Printed on: " + DateTime.Now.ToString("yyyy-MM-dd");

            #endregion Defining the summary worksheet

            #region Apply the entire conversion sheet template formatting to the newly-added sheet

            Methods.CopyExcelRegion(wsConversionTemplate, conversionTemplateSheetOriginRow, conversionTemplateSheetOriginColumn, conversionTemplateSheetRowSpan, conversionTemplateSheetColumnSpan, wsConversion, conversionTemplateSheetOriginRow, conversionTemplateSheetOriginColumn);
            //wbPrint.NamedReferences.Clear();

            #endregion Apply the entire conversion sheet template formatting to the newly-added sheet

            #region Add the values

            foreach (DataColumn column in drSelectedEntryPrint.Table.Columns)
            {

                // Omissions:
                if ((column.ColumnName.Trim() != "CampaignID") &&
                    (column.ColumnName.Trim() != "CampaignCode") &&
                    (column.ColumnName.Trim() != "BatchID") &&
                    (column.ColumnName.Trim() != "UDMBatchCode") &&
                    (column.ColumnName.Trim() != "AgentID") &&
                    (column.ColumnName.Trim() != "FirstName") &&
                    (column.ColumnName.Trim() != "LastName") &&
                    (column.ColumnName.Trim() != "Conversion2") &&
                    (column.ColumnName.Trim() != "Conversion4") &&
                    (column.ColumnName.Trim() != "Conversion6") &&
                    (column.ColumnName.Trim() != "ExpiryDate") &&
                    (column.ColumnName.Trim() != "AllocationDate") &&
                    (column.ColumnName.Trim() != "SalesStartDate") &&
                    (column.ColumnName.Trim() != "IsRedeemedGiftBatch") &&
                    (column.ColumnName.Trim() != "RedeemTarget") &&
                    (column.ColumnName.Trim() != "LatestLeadActivity") &&
                    (column.ColumnName.Trim() != "ContactRateTarget") &&
                    (column.ColumnName.Trim() != "Select"))

                {
                    try
                    {
                        wsConversion.GetCell(column.ColumnName).Value = drSelectedEntryPrint[column.ColumnName];
                    }
                    catch
                    {

                    }
                }
            }


             //this is for all the lead sales blocks on the conversion page
            try
            {
                int TotalLeadsCalc = Convert.ToInt32(wsConversion.GetCell("Leads2Print").Value);


                double week1stats = TotalLeadsCalc * 0.26;

                double week2statsStep1 = TotalLeadsCalc * 0.30;
                double week2stats = week2statsStep1 - week1stats;

                int week1 = Convert.ToInt32(week1stats);
                int week2 = Convert.ToInt32(week2stats);


                for (int y = 0; y < week1; y++)
                {
                    try { wsConversion.GetCell("weekone" + (y + 1)).Value = (y + 1); } catch { }
                }

                for (int y = 0; y < week2; y++)
                {
                    try { wsConversion.GetCell("weektwo" + (y + 1)).Value = (y + 1); } catch { }
                }
            }
            catch
            {

            }


            try { wsConversion.GetCell("TargetSales").Value = wsConversion.GetCell("CountingWeek3TargetSales").Value; } catch { }
            try { wsConversion.GetCell("TargetPercentage2").Value = wsConversion.GetCell("TargetPercentage").Value; } catch { }

            try { wsConversion.GetCell("AllocationDate").Value = drSelectedEntryPrint["SalesStartDate"]; } catch { }


            //wsConversion.Columns[2].
                //(2, 1).CellFormat.FormatString = """$""#,##0.00;[red](""$""#,##0.00)"

            #endregion Add the values

            if (!(drSelectedEntryPrint.Table.Columns["CampaignName"].ToString().CaseInsensitiveContains("upgrade")))
            {
                wsConversion.Rows[25].Hidden = true;
            }
            
        }

        #endregion Private Methods

        #region Event Handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MenuLeadScreen menuLeadScreen = new MenuLeadScreen(ScreenDirection.Reverse);
            OnClose(menuLeadScreen);
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            SetCursor(Cursors.Wait);

            btnPrint.IsEnabled = false;
            btnClose.IsEnabled = false;
            lblAgent.Visibility = Visibility.Visible;
            lblLeadNo.Visibility = Visibility.Visible;

            _TimerPrint.Start();
            PrintLeads();
            _TimerPrint.Stop();

            lblAgent.Text = null;
            lblLeadNo.Text = null;
            lblAgent.Visibility = Visibility.Hidden;
            lblLeadNo.Visibility = Visibility.Hidden;

            btnClose.IsEnabled = true;
            btnPrint.Content = "Print";
            btnPrint.ToolTip = TimeSpan.FromSeconds(_PrintTime).ToString();
            LoadSummaryData();
            _xdgHeaderPrefixAreaCheckbox.IsChecked = false;

            SetCursor(Cursors.Arrow);
        }

        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = ((DataView)xdgPrintLeads.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = true;
                }

                btnPrint.IsEnabled = true;
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
                DataTable dt = ((DataView)xdgPrintLeads.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = false;
                }

                btnPrint.IsEnabled = false;
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
                if (_xdgHeaderPrefixAreaCheckbox != null) { _xdgHeaderPrefixAreaCheckbox.IsChecked = AllRecordsSelected(); }
                if (_xdgHeaderPrefixAreaCheckbox != null && (_xdgHeaderPrefixAreaCheckbox.IsChecked == true || _xdgHeaderPrefixAreaCheckbox.IsChecked == null))
                {
                    btnPrint.IsEnabled = true;
                }
                else
                {
                    btnPrint.IsEnabled = false;
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

        #endregion

    }
}
