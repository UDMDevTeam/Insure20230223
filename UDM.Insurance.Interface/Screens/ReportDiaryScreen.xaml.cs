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

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportDiaryScreen.xaml
    /// </summary>
    public partial class ReportDiaryScreen
    {

        #region Constants

        //private string _fontName = "Calibri";
        //private const int _fontSize = 10;
        //private const int _pointsToTwipsFactor = 20;
        //private const int _fontHeight = _fontSize * _pointsToTwipsFactor;

        #endregion Constants

        #region Private Members

        private CheckBox _xdgHeaderPrefixAreaCheckbox;
        //private RecordCollectionBase _campaigns;
        //private System.Collections.Generic.List<Record> _campaigns;
        private List<Record> _lstSelectedCampaigns;
        private string _campaignIDList = String.Empty;

        private DateTime _startDate;
        private DateTime _endDate;

        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;

        #endregion Private Members

        #region Constructors

        public ReportDiaryScreen()
        {
            InitializeComponent();
            LoadCampaignInfo();

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion Constructors

        #region Private Methods

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

        private void LoadCampaignInfo()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataTable dt = Methods.GetTableData("SELECT ID [CampaignID], Name [CampaignName], Code [CampaignCode] FROM INCampaign");
                DataColumn column = new DataColumn("Select", typeof(bool));
                column.DefaultValue = false;
                dt.Columns.Add(column);
                dt.DefaultView.Sort = "CampaignName ASC";
                xdgCampaigns.DataSource = dt.DefaultView;
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

        private void EnableDisableExportButton()
        {
            try
            {
                if ((calStartDate.SelectedDate != null && (calEndDate.SelectedDate != null))) //&& (calEndDate.SelectedDate >= Cal1.SelectedDate)
                {
                    if (_xdgHeaderPrefixAreaCheckbox != null && (_xdgHeaderPrefixAreaCheckbox.IsChecked == true || _xdgHeaderPrefixAreaCheckbox.IsChecked == null))
                    {
                        btnReport.IsEnabled = true;
                        return;
                    }
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
            xdgCampaigns.IsEnabled = true;
            calStartDate.IsEnabled = true;
            calEndDate.IsEnabled = true;
        }

        private bool IsAllInputParametersSpecifiedAndValid()
        {
            #region Ensuring that at least one campaign was selected

            var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
            _lstSelectedCampaigns = new List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

            if (_lstSelectedCampaigns.Count == 0)
            {
                ShowMessageBox(new INMessageBoxWindow1(), "Please select at least 1 campaign from the list.", "No campaigns selected", ShowMessageType.Error);
                return false;
            }
            else
            {
                _campaignIDList = _lstSelectedCampaigns.Cast<DataRecord>().Where(record => (bool)record.Cells["Select"].Value).Aggregate(String.Empty, (current, record) => current + record.Cells["CampaignID"].Value + ",");
                _campaignIDList = _campaignIDList.Substring(0, _campaignIDList.Length - 1);
            }

            #endregion Ensuring that at least one campaign was selected

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

                #region Setup excel workbook

                Workbook wbTemplate = Methods.DefineTemplateWorkbook("/Templates/ReportTemplateDiary.xlsx");
                Worksheet wsTemplate = wbTemplate.Worksheets["Report"];

                #endregion Setup excel workbook

                #region Get the report data
                DataSet dsDiaryReportData;
                if (CallDataCB.IsChecked == true)
                {
                    dsDiaryReportData = Business.Insure.INGetDiaryReportDataCalldata(_campaignIDList, _startDate, _endDate);
                }
                else
                {
                    dsDiaryReportData = Business.Insure.INGetDiaryReportData(_campaignIDList, _startDate, _endDate);
                }

                DataTable dtMainReportData = dsDiaryReportData.Tables[0];
                DataTable dtCampaigns = dsDiaryReportData.Tables[1];
                DataTable dtTSRDiaries = dsDiaryReportData.Tables[2];
                DataTable dtColumnMappings = dsDiaryReportData.Tables[3];

                #endregion Get the report data

                #region Variable declarations

                string filePathAndName = String.Empty;

                string campaign = String.Empty;
                string campaignFilterString = String.Empty;
                string currentTSR = String.Empty;
                string currentTSRFilterString = String.Empty;

                byte templateColumnSpan = 11;
                byte templateRowSpan = 6;
                byte templateDataRowIndex = 7;

                int reportRow = 7;

                #endregion Variable declarations

                #region Loop through each campaign and create a new workbook for each

                foreach (DataRow drCampaign in dtCampaigns.Rows)
                {


                    campaign = drCampaign["Campaign"].ToString();
                    campaignFilterString = drCampaign["CampaignFilterString"].ToString();

                    #region Instantiate a new Excel workbook

                    Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
                    filePathAndName = String.Format("{0}{1} Diary Report ~ {2}.xlsx", GlobalSettings.UserFolder, campaign, DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));
                    
                    #endregion Instantiate a new Excel workbook

                    DataTable dtCurrentCampaignTSRs = dtTSRDiaries.Select(campaignFilterString).CopyToDataTable();

                    foreach (DataRow drCurrentTSR in dtCurrentCampaignTSRs.Rows)
                    {
                        currentTSR = drCurrentTSR["AllocatedTo"].ToString();
                        currentTSRFilterString = drCurrentTSR["CampaignTSRFilterString"].ToString();

                        DataTable dtCurrentCampaignAndTSRData = dtMainReportData.Select(currentTSRFilterString).CopyToDataTable();

                        #region Add the current sales consultant's worksheet

                        Worksheet wsReport = wbReport.Worksheets.Add(Methods.ParseWorksheetName(wbReport, currentTSR));
                        Methods.CopyWorksheetOptionsFromTemplate(wsTemplate, wsReport, true, true, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false);

                        #endregion Add the current sales consultant's worksheet

                        #region Insert the column headings and populate the details

                        Methods.CopyExcelRegion(wsTemplate, 0, 0, templateRowSpan, templateColumnSpan, wsReport, 0, 0);

                        wsReport.GetCell("A1").Value = String.Format("Diary Report - {0}", campaign);
                        wsReport.GetCell("A3").Value = String.Format("Leads that were saved with a Diary status for the time between {0} and {1}", _startDate.ToString("dddd, d MMMM yyyy"), _endDate.ToString("dddd, d MMMM yyyy"));
                        wsReport.GetCell("A5").Value = currentTSR;
                        //wsReport.GetCell(reportDateCell).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        #endregion Insert the column headings and populate the details

                        #region Add the data

                        Methods.MapTemplatizedExcelValues(wsTemplate, dtCurrentCampaignAndTSRData, dtColumnMappings, templateDataRowIndex, 0, 0, templateColumnSpan, wsReport, reportRow, 0);

                        // Reset the row index
                        reportRow = 7;

                        #endregion Add the data
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

                #endregion Loop through each campaign and create a new workbook for each

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


      //  private void ReportOLD(object sender, DoWorkEventArgs e)
      //  {

      //      try
      //      {
      //          SetCursor(Cursors.Wait);

      //          foreach (DataRecord record in _campaigns)
      //          {
      //              if ((bool)record.Cells["Select"].Value)
      //              {
      //                  long campaignID = Convert.ToInt32(record.Cells["CampaignID"].Value);
      //                  string campaignName = record.Cells["CampaignName"].Value.ToString();

      //                  #region Setup excel documents

      //                  Workbook wbTemplate;
      //                  Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
      //                  string filePathAndName = String.Format("{0}{1} Diary Report ~ {2}.xlsx", GlobalSettings.UserFolder, campaignName, DateTime.Now.ToString("yyyy-MM-dd HHmmdd"));

      //                  Uri uri = new Uri("/Templates/ReportTemplateDiary.xlsx", UriKind.Relative);
      //                  StreamResourceInfo info = Application.GetResourceStream(uri);
      //                  if (info != null)
      //                  {
      //                      wbTemplate = Workbook.Load(info.Stream, true);
      //                  }
      //                  else
      //                  {
      //                      return;
      //                  }

						//Worksheet wsTemplate = wbTemplate.Worksheets["Report"];
      //                  Worksheet wsReport = wbReport.Worksheets.Add(campaignName);

						//wsReport.PrintOptions.PaperSize = PaperSize.A4;
						//wsReport.PrintOptions.Orientation = Orientation.Portrait;

      //                  #endregion Setup excel documents

      //                  #region Get report data from database

      //                  DataTable dtReducedPremiumReport;

						//SqlParameter[] parameters = new SqlParameter[3];
      //                  parameters[0] = new SqlParameter("@CampaignID", campaignID);
      //                  parameters[1] = new SqlParameter("@FromDate", _startDate.ToString("yyyy-MM-dd"));
      //                  parameters[2] = new SqlParameter("@ToDate", _endDate.ToString("yyyy-MM-dd"));

      //                  DataSet dsReducedPremiumReport = Methods.ExecuteStoredProcedure("spINReportDiary", parameters);
      //                  if (dsReducedPremiumReport.Tables.Count > 0)
						//{
      //                      dtReducedPremiumReport = dsReducedPremiumReport.Tables[0];

      //                      if (dtReducedPremiumReport.Rows.Count == 0)
						//	{
						//		Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
						//		{
						//			ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + campaignName + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
						//		});

						//		continue;
						//	}
						//}
						//else
						//{
						//	Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
						//	{
						//		ShowMessageBox(new INMessageBoxWindow1(), "There is no data to export for the " + campaignName + " Campaign and specified Date range.", "No Data", ShowMessageType.Information);
						//	});

						//	continue;
      //                  }

      //                  #endregion Get report data from database

      //                  #region Populating the report details

      //                  Methods.CopyExcelRegion(wsTemplate, 0, 0, 6, 11, wsReport, 0, 0);

      //                  wsReport.GetCell("A1").Value = String.Format("Diary Report - {0}", campaignName);
      //                  //wsReport.GetCell("A3").Value = String.Format(@"Diaries created/updated between {0} and {1}", _startDate.ToString("dddd, dd MMMM yyyy"), _endDate.ToString("dddd, dd MMMM yyyy"));
      //                  if (_startDate == _endDate)
      //                  {
      //                      wsReport.GetCell("A3").Value = String.Format(@"Diaries scheduled for {0}", _startDate.ToString("dddd, dd MMMM yyyy"));
      //                  }
      //                  else
      //                  {
      //                      wsReport.GetCell("A3").Value = String.Format(@"Diaries scheduled for the time between {0} and {1}", _startDate.ToString("dddd, dd MMMM yyyy"), _endDate.ToString("dddd, dd MMMM yyyy"));
      //                  }

      //                  wsReport.GetCell("A5").Value = String.Format("Date Generated: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

      //                  #endregion Populating the report details

      //                  #region Report data

      //                  {
						//	int rowIndex = 8;
						//	string previousValue = string.Empty;

      //                      foreach (DataRow dr in dtReducedPremiumReport.Rows)
						//	{
						//		Methods.CopyExcelRegion(wsTemplate, 7, 0, 1, 11, wsReport, rowIndex-1, 0);

      //                          wsReport.GetCell("A" + rowIndex.ToString()).Value = dr["Reference Number"];
      //                          wsReport.GetCell("B" + rowIndex.ToString()).Value = dr["Allocated To"];
      //                          wsReport.GetCell("C" + rowIndex.ToString()).Value = dr["UDM Batch Code"];
      //                          wsReport.GetCell("D" + rowIndex.ToString()).Value = dr["Lead Status"];
      //                          wsReport.GetCell("E" + rowIndex.ToString()).Value = dr["Diary Save Date"];
      //                          wsReport.GetCell("F" + rowIndex.ToString()).Value = dr["Diary Scheduled For"];
      //                          wsReport.GetCell("G" + rowIndex.ToString()).Value = dr["Diary Saved By"];
      //                          //wsReport.GetCell("G" + rowIndex.ToString()).Value = dr["New Policy"];
      //                          //wsReport.GetCell("H" + rowIndex.ToString()).Value = dr["Reduced Premium"];
      //                          //wsReport.GetCell("I" + rowIndex.ToString()).Value = dr["Premium Difference"];
      //                          //wsReport.GetCell("J" + rowIndex.ToString()).Value = dr["TSR"];
      //                          //wsReport.GetCell("K" + rowIndex.ToString()).Value = dr["Code"];

						//		rowIndex++;
						//	}
      //                  }

      //                  #endregion Report data

      //                  #region Save and open the resulting workbook

      //                  //Save excel document
      //                  wbReport.Save(filePathAndName);

      //                  //Display excel document
      //                  Process.Start(filePathAndName);

      //                  #endregion Save and open the resulting workbook
      //              }
      //          }
      //      }

      //      catch (Exception ex)
      //      {
      //          HandleException(ex);
      //      }

      //      finally
      //      {
      //          SetCursor(Cursors.Arrow);
      //      }
      //  }

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
                //_campaigns = xdgCampaigns.Records;

                //var lstTemp = (from r in xdgCampaigns.Records where (bool)((DataRecord)r).Cells["Select"].Value select r).ToList();
                //_campaigns = new System.Collections.Generic.List<Record>(lstTemp.OrderBy(r => ((DataRecord)r).Cells["CampaignName"].Value));

                if (IsAllInputParametersSpecifiedAndValid())
                {
                    btnClose.IsEnabled = false;
                    btnReport.IsEnabled = false;
                    xdgCampaigns.IsEnabled = false;
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

        private void HeaderPrefixAreaCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = ((DataView)xdgCampaigns.DataSource).Table;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = true;
                }

                EnableDisableExportButton();
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

                foreach (DataRow dr in dt.Rows)
                {
                    dr["Select"] = false;
                }

                EnableDisableExportButton();
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
                if (_xdgHeaderPrefixAreaCheckbox != null)
                {
                    _xdgHeaderPrefixAreaCheckbox.IsChecked = AllRecordsSelected();
                }

                EnableDisableExportButton();
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

    }
}
