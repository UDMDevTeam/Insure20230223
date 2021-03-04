using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Infragistics.Controls.Editors;
using Infragistics.Documents.Excel;
using Infragistics.Windows.DataPresenter;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportQAWeightingsScreen.xaml
    /// </summary>
    public partial class ReportQAWeightingsScreen
    {


        #region Constants

        private const string _fontName = "Calibri";
        private const int _fontSize = 10;
        private const int _pointsToTwipsFactor = 20;
        private const int _fontHeight = _fontSize * _pointsToTwipsFactor;
        private readonly System.Drawing.Color _columnHeaderBackgroundColour1 = System.Drawing.Color.FromArgb(235, 241, 222);
        private const double _infragisticsPixelToColumnWidthFactor = 36.630036630036630036630036630037;

        #endregion Constants

        #region Private Members

        private int _timer1;
        private int _timer1Count;
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();

        //private string _strFromDate;
        //private string _strToDate;
        //private string _strTodaysDate;
        //private string _strTodaysDateIncludingColons;

        //private string _reportStartDate;
        //private string _reportEndDate;

        DateTime _fromDate;
        DateTime _toDate;
        int agentID;
        private int i;
        private List<DataRecord> _selectedAgents;
        private string _fkUserIDs = String.Empty;
        private int j;

        #endregion Private Members   

        #region Constructors

        public ReportQAWeightingsScreen()
        {
            InitializeComponent();
            LoadAgentInfo();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);

#if TESTBUILD
                        //TestControl.Visibility = Visibility.Visible;
#else
            //TestControl.Visibility = Visibility.Collapsed;
#endif
        }

        #endregion

        #region Private Methods

        //private DataTable GetTrackingReportData(string fromDateStr, string toDateStr)
        //{

        //    DataTable dtHRRecruitmentTrackingReportData = Staff.HRGetRecruitmentTrackingReportData(fromDateStr, toDateStr);

        //    if (dsHRTrackingReportData.Tables.Count > 0)
        //    {
        //        dtHRTrackingReportData = dsHRTrackingReportData.Tables[0];
        //    }

        //    return dtHRTrackingReportData;
        //}

        private void LoadAgentInfo()
        {
            try
            {
                Cursor = Cursors.Wait;
                if (grdAgents.IsEnabled)
                {
                    DataSet ds = Methods.ExecuteStoredProcedure("spGetAllQAAgents", null);

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
        private bool IsAllInputParametersSpecifiedAndValid()
        {
            #region Ensuring that at least 1 user was selected

            //var lstTemp = (from r in xdgAgents.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            //_lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

            _selectedAgents = (xdgAgents.Records.Select(r => (DataRecord)r).Where(r => (bool)r.Cells["IsChecked"].Value)).ToList();

            if (_selectedAgents.Count == 0)
            {
                ShowMessageBox(new Windows.INMessageBoxWindow1(), "Please select at least 1 sales agent from the list.", "No sales agent selected", Embriant.Framework.ShowMessageType.Error);
                return false;
            }
            else
            {
                _fkUserIDs = _selectedAgents.Cast<DataRecord>().Where(record => (bool)record.Cells["IsChecked"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["ID"].Value + ",");
                _fkUserIDs = _fkUserIDs.Substring(0, _fkUserIDs.Length - 1);
            }

            #endregion Ensuring that at least 1 campaign was selected

            #region Ensuring that the From Date was specified

            if (Cal1.SelectedDate == null)
            {
                ShowMessageBox(new Windows.INMessageBoxWindow1(), @"Please specify the 'From Date'.", @"No 'From Date' specified", Embriant.Framework.ShowMessageType.Error);
                return false;
            }
            else
            {
                _fromDate = Cal1.SelectedDate.Value;
            }

            #endregion Ensuring that the From Date was specified

            #region Ensuring that the To Date was specified

            if (Cal2.SelectedDate == null)
            {
                ShowMessageBox(new Windows.INMessageBoxWindow1(), @"Please specify the 'To Date'.", @"No 'To Date' specified", Embriant.Framework.ShowMessageType.Error);
                return false;
            }

            else
            {
                _toDate = Cal2.SelectedDate.Value;
            }

            #endregion Ensuring that the To Date was specified

            #region Ensuring that the date range is valid

            if (_fromDate > _toDate)
            {
                ShowMessageBox(new Windows.INMessageBoxWindow1(), @"Invalid date range specified: The 'From Date' can not be greater than the 'To Date'.", "Invalid date range", Embriant.Framework.ShowMessageType.Error);
                return false;
            }


            #endregion Ensuring that the date range is valid

            // Otherwise if all is well, proceed:
            return true;
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                   

                SetCursor(Cursors.Wait);

                    IsAllInputParametersSpecifiedAndValid();

                    if (_selectedAgents.Count > 0)
                    {
                        foreach (DataRecord drAgent in _selectedAgents)
                        {

                            agentID = Convert.ToInt32(drAgent.Cells["ID"].Value);

                        }
                    }


                    #region Get the report data as a DataTable




                    SqlParameter[] parameters =
                {
                    new SqlParameter("@UserID", agentID),
                    new SqlParameter("@FromDate", _fromDate),
                    new SqlParameter("@ToDate", _toDate)
                };

                DataSet ds = Methods.ExecuteStoredProcedure("spINReportQAWeightingsAssessment", parameters);

                DataTable dtQAInformation = ds.Tables[0];

                SqlParameter[] sqlparameters =
                {
                    new SqlParameter("@UserID", agentID),
                };

                DataSet dataSet = Methods.ExecuteStoredProcedure("spINReportQAQuestionsAnswers", sqlparameters);

                DataTable dtQAAnswers = dataSet.Tables[0];

                
                #endregion Get the report data as a DataTable

                if (dtQAInformation.Rows.Count > 0)
                {
                    #region Setup excel document

                    var workBook = new Workbook();
                    string filePathAndName = String.Format("{0}QA Assessment Report {1} - {2} ~ {3}.xlsx",
                        GlobalSettings.UserFolder,
                        _fromDate.ToString("yyyy-MM-dd"),
                        _toDate.ToString("yyyy-MM-dd"),
                        DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                    Workbook wbTemplate;
                    Uri uri = new Uri("/Templates/ReportTemplateQAWeightings.xlsx", UriKind.Relative);
                    System.Windows.Resources.StreamResourceInfo info = System.Windows.Application.GetResourceStream(uri);
                    if (info != null)
                    {
                        wbTemplate = Workbook.Load(info.Stream, true);
                    }
                    else
                    {
                        return;
                    }

                    Worksheet wsTemplate = wbTemplate.Worksheets["Template"];
                    Worksheet wsNewReportSheet = workBook.Worksheets.Add("Report Data");

                    Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsNewReportSheet, true, true, true, true, true, false, true, false, true, true, true, true, true, false, false, true, false);

                    //wsNewReportSheet.PrintOptions.ScalingType = ScalingType.FitToPages;
                    //wsNewReportSheet.PrintOptions.MaxPagesHorizontally = 1;

                    int reportRow = 0;

                    #endregion Setup excel document

                    #region Step 1: Copy a region from the template that consists of the headings and the column headings

                    Methods.CopyExcelRegion(wsTemplate, 0, 0, 7, 85, wsNewReportSheet, reportRow, 0);

                    #endregion Step 1: Copy a region from the template that consists of the headings and the column headings

                    #region Step 2: Populate the report details

                    wsNewReportSheet.GetCell("ReportSubtitle").Value = String.Format("This report is between {0} and {1}", _fromDate.ToString("dddd, d MMMM yyyy"), _toDate.ToString("dddd, d MMMM yyyy"));
                        //wsNewReportSheet.GetCell("ReportDate").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        wsNewReportSheet.GetCell("AssessorFirstName").Value = dtQAInformation.Rows[0]["AssessorFirstName"].ToString();
                        wsNewReportSheet.GetCell("AssessorSurname").Value = dtQAInformation.Rows[0]["AssessorSurname"].ToString();
                        wsNewReportSheet.GetCell("CampaignName").Value = dtQAInformation.Rows[0]["CampaignName"].ToString();
                        wsNewReportSheet.GetCell("AgentFirstName").Value = dtQAInformation.Rows[0]["AgentFirstName"].ToString();
                        wsNewReportSheet.GetCell("AgentSurname").Value = dtQAInformation.Rows[0]["AgentSurname"].ToString();
                        wsNewReportSheet.GetCell("ReferenceNumber").Value = dtQAInformation.Rows[0]["ReferenceNo"].ToString();
                        wsNewReportSheet.GetCell("DateOfSale").Value = dtQAInformation.Rows[0]["DateOfSale"].ToString();
                        wsNewReportSheet.GetCell("Borderline").Value = dtQAInformation.Rows[0]["Borderline"].ToString();
                        wsNewReportSheet.GetCell("PercentCriteria").Value = dtQAInformation.Rows[0]["PercentCriteria"].ToString();

                        #endregion Step 2: Report the report details

                        reportRow = 8;

                        //wsNewReportSheet.Cells[i + 13, j + 1] = dtQAAnswers.Rows[0][j].ToString();


                        if (dtQAAnswers.Rows.Count > 0)
                        {
                            wsNewReportSheet.GetCell("QAQuestions").Value = dtQAAnswers.Rows[0]["Question"].ToString();
                            wsNewReportSheet.GetCell("QAAnswers").Value = dtQAAnswers.Rows[0]["Answer"].ToString();

                            wsNewReportSheet.GetCell("A9").Value = dtQAAnswers.Rows[1]["Question"].ToString();
                            wsNewReportSheet.GetCell("B9").Value = dtQAAnswers.Rows[1]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A10").Value = dtQAAnswers.Rows[2]["Question"].ToString();
                            wsNewReportSheet.GetCell("B10").Value = dtQAAnswers.Rows[2]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A11").Value = dtQAAnswers.Rows[3]["Question"].ToString();
                            wsNewReportSheet.GetCell("B11").Value = dtQAAnswers.Rows[3]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A12").Value = dtQAAnswers.Rows[4]["Question"].ToString();
                            wsNewReportSheet.GetCell("B12").Value = dtQAAnswers.Rows[4]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A13").Value = dtQAAnswers.Rows[5]["Question"].ToString();
                            wsNewReportSheet.GetCell("B13").Value = dtQAAnswers.Rows[5]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A14").Value = dtQAAnswers.Rows[6]["Question"].ToString();
                            wsNewReportSheet.GetCell("B14").Value = dtQAAnswers.Rows[6]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A15").Value = dtQAAnswers.Rows[7]["Question"].ToString();
                            wsNewReportSheet.GetCell("B15").Value = dtQAAnswers.Rows[7]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A16").Value = dtQAAnswers.Rows[8]["Question"].ToString();
                            wsNewReportSheet.GetCell("B16").Value = dtQAAnswers.Rows[8]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A17").Value = dtQAAnswers.Rows[9]["Question"].ToString();
                            wsNewReportSheet.GetCell("B17").Value = dtQAAnswers.Rows[9]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A18").Value = dtQAAnswers.Rows[10]["Question"].ToString();
                            wsNewReportSheet.GetCell("B18").Value = dtQAAnswers.Rows[10]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A19").Value = dtQAAnswers.Rows[11]["Question"].ToString();
                            wsNewReportSheet.GetCell("B19").Value = dtQAAnswers.Rows[11]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A20").Value = dtQAAnswers.Rows[12]["Question"].ToString();
                            wsNewReportSheet.GetCell("B20").Value = dtQAAnswers.Rows[12]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A21").Value = dtQAAnswers.Rows[13]["Question"].ToString();
                            wsNewReportSheet.GetCell("B21").Value = dtQAAnswers.Rows[13]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A22").Value = dtQAAnswers.Rows[14]["Question"].ToString();
                            wsNewReportSheet.GetCell("B22").Value = dtQAAnswers.Rows[14]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A23").Value = dtQAAnswers.Rows[15]["Question"].ToString();
                            wsNewReportSheet.GetCell("B23").Value = dtQAAnswers.Rows[15]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A24").Value = dtQAAnswers.Rows[16]["Question"].ToString();
                            wsNewReportSheet.GetCell("B24").Value = dtQAAnswers.Rows[16]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A25").Value = dtQAAnswers.Rows[17]["Question"].ToString();
                            wsNewReportSheet.GetCell("B25").Value = dtQAAnswers.Rows[17]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A26").Value = dtQAAnswers.Rows[18]["Question"].ToString();
                            wsNewReportSheet.GetCell("B26").Value = dtQAAnswers.Rows[18]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A27").Value = dtQAAnswers.Rows[19]["Question"].ToString();
                            wsNewReportSheet.GetCell("B27").Value = dtQAAnswers.Rows[19]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A28").Value = dtQAAnswers.Rows[20]["Question"].ToString();
                            wsNewReportSheet.GetCell("B28").Value = dtQAAnswers.Rows[20]["Answer"].ToString();

                            wsNewReportSheet.GetCell("A29").Value = dtQAAnswers.Rows[21]["Question"].ToString();
                            wsNewReportSheet.GetCell("B29").Value = dtQAAnswers.Rows[21]["Answer"].ToString();

                            wsNewReportSheet.GetCell("A30").Value = dtQAAnswers.Rows[22]["Question"].ToString();
                            wsNewReportSheet.GetCell("B30").Value = dtQAAnswers.Rows[22]["Answer"].ToString();

                            wsNewReportSheet.GetCell("A31").Value = dtQAAnswers.Rows[23]["Question"].ToString();
                            wsNewReportSheet.GetCell("B31").Value = dtQAAnswers.Rows[23]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A32").Value = dtQAAnswers.Rows[24]["Question"].ToString();
                            wsNewReportSheet.GetCell("B32").Value = dtQAAnswers.Rows[24]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A33").Value = dtQAAnswers.Rows[25]["Question"].ToString();
                            wsNewReportSheet.GetCell("B33").Value = dtQAAnswers.Rows[25]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A34").Value = dtQAAnswers.Rows[26]["Question"].ToString();
                            wsNewReportSheet.GetCell("B34").Value = dtQAAnswers.Rows[26]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A35").Value = dtQAAnswers.Rows[27]["Question"].ToString();
                            wsNewReportSheet.GetCell("B35").Value = dtQAAnswers.Rows[27]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A36").Value = dtQAAnswers.Rows[28]["Question"].ToString();
                            wsNewReportSheet.GetCell("B37").Value = dtQAAnswers.Rows[28]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A38").Value = dtQAAnswers.Rows[29]["Question"].ToString();
                            wsNewReportSheet.GetCell("B38").Value = dtQAAnswers.Rows[29]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A39").Value = dtQAAnswers.Rows[30]["Question"].ToString();
                            wsNewReportSheet.GetCell("B39").Value = dtQAAnswers.Rows[30]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A40").Value = dtQAAnswers.Rows[31]["Question"].ToString();
                            wsNewReportSheet.GetCell("B40").Value = dtQAAnswers.Rows[31]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A41").Value = dtQAAnswers.Rows[32]["Question"].ToString();
                            wsNewReportSheet.GetCell("B41").Value = dtQAAnswers.Rows[32]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A42").Value = dtQAAnswers.Rows[33]["Question"].ToString();
                            wsNewReportSheet.GetCell("B42").Value = dtQAAnswers.Rows[33]["Answer"].ToString();

                            wsNewReportSheet.GetCell("A43").Value = dtQAAnswers.Rows[34]["Question"].ToString();
                            wsNewReportSheet.GetCell("B43").Value = dtQAAnswers.Rows[34]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A44").Value = dtQAAnswers.Rows[35]["Question"].ToString();
                            wsNewReportSheet.GetCell("B44").Value = dtQAAnswers.Rows[35]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A45").Value = dtQAAnswers.Rows[36]["Question"].ToString();
                            wsNewReportSheet.GetCell("B45").Value = dtQAAnswers.Rows[36]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A46").Value = dtQAAnswers.Rows[37]["Question"].ToString();
                            wsNewReportSheet.GetCell("B46").Value = dtQAAnswers.Rows[37]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A47").Value = dtQAAnswers.Rows[38]["Question"].ToString();
                            wsNewReportSheet.GetCell("B47").Value = dtQAAnswers.Rows[38]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A48").Value = dtQAAnswers.Rows[39]["Question"].ToString();
                            wsNewReportSheet.GetCell("B48").Value = dtQAAnswers.Rows[39]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A49").Value = dtQAAnswers.Rows[40]["Question"].ToString();
                            wsNewReportSheet.GetCell("B49").Value = dtQAAnswers.Rows[40]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A50").Value = dtQAAnswers.Rows[41]["Question"].ToString();
                            wsNewReportSheet.GetCell("B50").Value = dtQAAnswers.Rows[41]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A51").Value = dtQAAnswers.Rows[42]["Question"].ToString();
                            wsNewReportSheet.GetCell("B51").Value = dtQAAnswers.Rows[42]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A52").Value = dtQAAnswers.Rows[43]["Question"].ToString();
                            wsNewReportSheet.GetCell("B52").Value = dtQAAnswers.Rows[43]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A53").Value = dtQAAnswers.Rows[44]["Question"].ToString();
                            wsNewReportSheet.GetCell("B53").Value = dtQAAnswers.Rows[44]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A54").Value = dtQAAnswers.Rows[45]["Question"].ToString();
                            wsNewReportSheet.GetCell("B54").Value = dtQAAnswers.Rows[45]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A55").Value = dtQAAnswers.Rows[46]["Question"].ToString();
                            wsNewReportSheet.GetCell("B55").Value = dtQAAnswers.Rows[46]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A56").Value = dtQAAnswers.Rows[47]["Question"].ToString();
                            wsNewReportSheet.GetCell("B56").Value = dtQAAnswers.Rows[47]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A57").Value = dtQAAnswers.Rows[48]["Question"].ToString();
                            wsNewReportSheet.GetCell("B57").Value = dtQAAnswers.Rows[48]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A58").Value = dtQAAnswers.Rows[49]["Question"].ToString();
                            wsNewReportSheet.GetCell("B58").Value = dtQAAnswers.Rows[49]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A59").Value = dtQAAnswers.Rows[50]["Question"].ToString();
                            wsNewReportSheet.GetCell("B59").Value = dtQAAnswers.Rows[50]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A60").Value = dtQAAnswers.Rows[51]["Question"].ToString();
                            wsNewReportSheet.GetCell("B60").Value = dtQAAnswers.Rows[51]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A61").Value = dtQAAnswers.Rows[52]["Question"].ToString();
                            wsNewReportSheet.GetCell("B61").Value = dtQAAnswers.Rows[52]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A62").Value = dtQAAnswers.Rows[53]["Question"].ToString();
                            wsNewReportSheet.GetCell("B62").Value = dtQAAnswers.Rows[53]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A63").Value = dtQAAnswers.Rows[54]["Question"].ToString();
                            wsNewReportSheet.GetCell("B63").Value = dtQAAnswers.Rows[54]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A64").Value = dtQAAnswers.Rows[55]["Question"].ToString();
                            wsNewReportSheet.GetCell("B64").Value = dtQAAnswers.Rows[55]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A65").Value = dtQAAnswers.Rows[56]["Question"].ToString();
                            wsNewReportSheet.GetCell("B65").Value = dtQAAnswers.Rows[56]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A66").Value = dtQAAnswers.Rows[57]["Question"].ToString();
                            wsNewReportSheet.GetCell("B66").Value = dtQAAnswers.Rows[57]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A67").Value = dtQAAnswers.Rows[58]["Question"].ToString();
                            wsNewReportSheet.GetCell("B67").Value = dtQAAnswers.Rows[58]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A68").Value = dtQAAnswers.Rows[59]["Question"].ToString();
                            wsNewReportSheet.GetCell("B68").Value = dtQAAnswers.Rows[59]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A69").Value = dtQAAnswers.Rows[60]["Question"].ToString();
                            wsNewReportSheet.GetCell("B69").Value = dtQAAnswers.Rows[60]["Answer"].ToString();
                            wsNewReportSheet.GetCell("A70").Value = dtQAAnswers.Rows[61]["Question"].ToString();
                            wsNewReportSheet.GetCell("B70").Value = dtQAAnswers.Rows[61]["Answer"].ToString();

                        }




                            //wsNewReportSheet.GetCell("Question4").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question5").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question6").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question7").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question8").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question9").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question10").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question11").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question12").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question13").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question14").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question15").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question16").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question17").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question18").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question19").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question20").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question21").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question22").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question23").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question24").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question25").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question26").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question27").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question28").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question29").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question30").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question31").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question32").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question33").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question34").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question35").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question36").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question37").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question38").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question39").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question40").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question41").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question42").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question43").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question44").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question45").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question46").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question47").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question48").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question49").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question50").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question51").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question52").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question53").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question54").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question55").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question56").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question57").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question58").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question59").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question60").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question61").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question62").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question63").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question64").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question65").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question66").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question67").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question68").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question69").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                            //wsNewReportSheet.GetCell("Question70").Value = dtQAAnswers.Rows[0]["Answer"].ToString();

                            //    wsNewReportSheet.GetCell("Question2").Value = dtQAAnswers.Rows[0]["Answer"].ToString();

                            





                        //for (int i = 0; i < dtQAAnswers.Rows.Count; i++)
                        //{


                        //    wsNewReportSheet.GetCell("Question1").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question2").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question3").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question4").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question5").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question6").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question7").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question8").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question9").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question10").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question11").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question12").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question13").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question14").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question15").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question16").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question17").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question18").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question19").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question20").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question21").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question22").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question23").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question24").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question25").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question26").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question27").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question28").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question29").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question30").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question31").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question32").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question33").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question34").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question35").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question36").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question37").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question38").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question39").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question40").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question41").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question42").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question43").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question44").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question45").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question46").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question47").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question48").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question49").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question50").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question51").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question52").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question53").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question54").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question55").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question56").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question57").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question58").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question59").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question60").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question61").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question62").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question63").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question64").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question65").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question66").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question67").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question68").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question69").Value = dtQAAnswers.Rows[0]["Answer"].ToString();
                        //    wsNewReportSheet.GetCell("Question70").Value = dtQAAnswers.Rows[0]["Answer"].ToString();

                        //}

                       

                        #region Save Excel Document
                        if (workBook.Worksheets.Count > 0)
                    {
                        workBook.SetCurrentFormat(WorkbookFormat.Excel2007);
                        workBook.Save(filePathAndName);

                        //Display excel document
                        Process.Start(filePathAndName);
                    }
                    #endregion Save Excel Document

                }





                });

            }

               

            catch (Exception ex)
            {
                HandleException(ex);
            }

            finally
            {
                SetCursor(System.Windows.Input.Cursors.Arrow);
            }
        }


        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                dispatcherTimer1.Stop();
                _timer1 = 0;
                _timer1Count = 0;

                btnReport.Content = "Report";
                EnableAllControls(true);
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        #endregion Private Methods

        #region Event Handlers

        private void Timer1(object sender, EventArgs e)
        {
            try
            {
                _timer1++;
                _timer1Count++;

                if (_timer1Count == 10)
                {
                    _timer1Count = 0;
                    btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
                    btnReport.ToolTip = btnReport.Content;
                }
                else
                {
                    btnReport.Content = TimeSpan.FromSeconds(_timer1).ToString();
                    btnReport.ToolTip = btnReport.Content;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
           

            try
            {
                EnableAllControls(false);

                if (Cal1.SelectedDate != null)
                {
                    //_reportStartDate = ((DateTime)calFromDate.SelectedDate).ToString("d");
                    _fromDate = Cal1.SelectedDate.Value;
                }
                else
                {
                    ShowMessageBox(new Windows.INMessageBoxWindow1(), "No start date selected.", "Report Error", ShowMessageType.Error);
                    EnableAllControls(true);
                    return;
                }

                if (Cal2.SelectedDate != null)
                {
                    //_reportEndDate = ((DateTime)calToDate.SelectedDate).ToString("d");
                    _toDate = Cal2.SelectedDate.Value;
                }
                else
                {
                    ShowMessageBox(new Windows.INMessageBoxWindow1(), "No end date selected.", "Report Error", ShowMessageType.Error);
                    EnableAllControls(true);
                    return;
                }


               

                var workerTrackingReport = new BackgroundWorker();
                workerTrackingReport.DoWork += Report;
                workerTrackingReport.RunWorkerCompleted += ReportCompleted;
                workerTrackingReport.RunWorkerAsync();
                dispatcherTimer1.Start();

            }


            catch (Exception ex)
            {
                HandleException(ex);
                EnableAllControls(true);
            }




        }

        private void Cal1_SelectedDatesChanged(object sender, EventArgs e)
        {
            DateTime.TryParse(Cal1.SelectedDate.ToString(), out _fromDate);
        }

        public void Cal2_SelectedDatesChanged(object sender, EventArgs e)
        {
            DateTime.TryParse(Cal2.SelectedDate.ToString(), out _toDate);
        }

       

        #endregion Event Handlers

        #region Helper Methods

        private void EnableAllControls(bool isEnabled)
        {
            btnReport.IsEnabled = isEnabled;
            btnClose.IsEnabled = isEnabled;
            Cal1.IsEnabled = isEnabled;
            Cal2.IsEnabled = isEnabled;
            xdgAgents.IsEnabled = isEnabled;
            grdAgents.IsEnabled = isEnabled;
        }

        #endregion Helper Methods

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuReportsScreen menuReportScreen = new MenuReportsScreen();
                OnClose(menuReportScreen);
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
    }

}
