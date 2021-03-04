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
    public partial class ReportRemoveDoNotContactLeads
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
        private int rowIndex;
        #endregion Private Members   

        #region Constructors

        public ReportRemoveDoNotContactLeads()
        {
            InitializeComponent();


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






        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {


                    SetCursor(Cursors.Wait);



                    #region Get the report data as a DataTable




                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@StartDate", _fromDate),
                        new SqlParameter("@EndDate", _toDate)
                    };

                    DataSet ds = Methods.ExecuteStoredProcedure("spINReportRemoveDoNotContactLeads", parameters);

                    DataTable dtDoNotContactLeads = ds.Tables[0];

                    #endregion Get the report data as a DataTable

                    if (dtDoNotContactLeads.Rows.Count > 0)
                    {
                        #region Setup excel document

                        var workBook = new Workbook();
                        string filePathAndName = String.Format("{0} Remove Do Not Contact Leads Report {1} - {2} ~ {3}.xlsx",
                            GlobalSettings.UserFolder,
                            _fromDate.ToString("yyyy-MM-dd"),
                            _toDate.ToString("yyyy-MM-dd"),
                            DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

                        Workbook wbTemplate;
                        Uri uri = new Uri("/Templates/ReportTemplateRemoveDoNotContactLeads.xlsx", UriKind.Relative);
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

                        Methods.CopyExcelRegion(wsTemplate, 0, 0, 5, 24, wsNewReportSheet, 0, 0);

                        #endregion Step 1: Copy a region from the template that consists of the headings and the column headings

                        #region Step 2: Populate the report details

                        wsNewReportSheet.GetCell("ReportSubtitle").Value = String.Format("For this period between {0} and {1}", _fromDate.ToString("dddd, d MMMM yyyy"), _toDate.ToString("dddd, d MMMM yyyy"));
                        //wsNewReportSheet.GetCell("ReportDate").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        //wsNewReportSheet.GetCell("GeneratedBy").Value = dtDoNotContactLeads.Rows[0]["GeneratedBy"].ToString();
                        wsNewReportSheet.GetCell("User").Value = Insure.GetLoggedInUserNameAndSurname().ToString();
                        wsNewReportSheet.GetCell("ReportDate").Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ToString();


                        #endregion Step 2: Report the report details

                        reportRow = 6;

                        //wsNewReportSheet.Cells[i + 13, j + 1] = dtQAAnswers.Rows[0][j].ToString();

                            //for (int i = 0; i < dtDoNotContactLeads.Rows.Count; i++)
                            //{
                                

                            //    wsNewReportSheet.GetCell("A8").Value = dtDoNotContactLeads.Rows[i]["CampaignName"].ToString();
                            //    wsNewReportSheet.GetCell("B8").Value = dtDoNotContactLeads.Rows[i]["BatchNo"].ToString();
                            //    wsNewReportSheet.GetCell("C8").Value = dtDoNotContactLeads.Rows[i]["ReferenceNo"].ToString();
                            //    wsNewReportSheet.GetCell("D8").Value = dtDoNotContactLeads.Rows[i]["AgentFullName"].ToString();
                            //    wsNewReportSheet.GetCell("E8").Value = dtDoNotContactLeads.Rows[i]["DateRemoved"].ToString();
                            //    wsNewReportSheet.GetCell("F8").Value = dtDoNotContactLeads.Rows[i]["RemovedBy"].ToString();

                            //    workBook.NamedReferences.Clear();
                            //    reportRow++;

                            //}

                        if (dtDoNotContactLeads.Rows.Count > 0)
                        {
                            rowIndex = 6;

                            foreach (DataRow dr in dtDoNotContactLeads.Rows)
                            {
                                Methods.CopyExcelRegion(wsTemplate, 5, 0, 1, 24, wsNewReportSheet, reportRow - 1, 0);

                                wsNewReportSheet.GetCell("A" + rowIndex).Value = dr["CampaignName"].ToString();
                                wsNewReportSheet.GetCell("B" + rowIndex).Value = dr["BatchNo"].ToString();
                                wsNewReportSheet.GetCell("C" + rowIndex).Value = dr["ReferenceNo"].ToString();
                                wsNewReportSheet.GetCell("D" + rowIndex).Value = dr["AgentFullName"].ToString();
                                wsNewReportSheet.GetCell("E" + rowIndex).Value = dr["DateRemoved"].ToString();
                                wsNewReportSheet.GetCell("F" + rowIndex).Value = dr["RemovedBy"].ToString();


                                rowIndex++;
                            }

                        }





                        //if (dtDoNotContactLeads.Rows.Count > 0)
                        //{

                        //    int i = 0;

                        //    wsNewReportSheet.GetCell("CampaignName").Value = dtDoNotContactLeads.Rows[0]["CampaignName"].ToString();
                        //    wsNewReportSheet.GetCell("BatchNo").Value = dtDoNotContactLeads.Rows[0]["BatchNo"].ToString();
                        //    wsNewReportSheet.GetCell("ReferenceNo").Value = dtDoNotContactLeads.Rows[0]["ReferenceNo"].ToString();
                        //    wsNewReportSheet.GetCell("AgentFullName").Value = dtDoNotContactLeads.Rows[0]["AgentFullName"].ToString();
                        //    wsNewReportSheet.GetCell("DateRemoved").Value = dtDoNotContactLeads.Rows[0]["DateRemoved"].ToString();
                        //    wsNewReportSheet.GetCell("RemovedBy").Value = dtDoNotContactLeads.Rows[0]["RemovedBy"].ToString();

                        //    workBook.NamedReferences.Clear();
                        //    reportRow++;
                        //    i++;
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

                if (calFromDate.SelectedDate != null)
                {
                    //_reportStartDate = ((DateTime)calFromDate.SelectedDate).ToString("d");
                    _fromDate = calFromDate.SelectedDate.Value;
                }
                else
                {
                    ShowMessageBox(new Windows.INMessageBoxWindow1(), "No start date selected.", "Report Error", ShowMessageType.Error);
                    EnableAllControls(true);
                    return;
                }

                if (calToDate.SelectedDate != null)
                {
                    //_reportEndDate = ((DateTime)calToDate.SelectedDate).ToString("d");
                    _toDate = calToDate.SelectedDate.Value;
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

        private void calFromDate_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calFromDate.SelectedDate.ToString(), out _fromDate);
        }

        private void calToDate_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            DateTime.TryParse(calToDate.SelectedDate.ToString(), out _toDate);
        }



        #endregion Event Handlers

        #region Helper Methods

        private void EnableAllControls(bool isEnabled)
        {
            btnReport.IsEnabled = isEnabled;
            btnClose.IsEnabled = isEnabled;
            calFromDate.IsEnabled = isEnabled;
            calFromDate.IsEnabled = isEnabled;
           
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

       

       
    }

}
