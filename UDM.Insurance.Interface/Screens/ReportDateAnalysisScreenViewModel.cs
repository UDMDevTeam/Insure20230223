using Embriant.Framework;
using Embriant.Framework.Configuration;
using Embriant.WPF.Controls;
using Infragistics.Documents.Excel;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Resources;
using System.Windows.Threading;
using UDM.Insurance.Interface.PrismInfrastructure;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    class ReportDateAnalysisScreenViewModel : BindableBase
    {

        #region Classes

        public class LookupType
        {
            private long iD;
            public long ID { get => iD; set => iD = value; }

            private string name;
            public string Name { get => name; set => name = value; }
        }

        #endregion



        #region Properties

        private string _title = "Date Analysis Report";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }



        private string _titleCampaigns = "Included Campaigns [0]";
        public string TitleCampaigns
        {
            get { return _titleCampaigns; }
            set { SetProperty(ref _titleCampaigns, value); }
        }



        private bool _isReportRunning;
        public bool IsReportRunning
        {
            get { return _isReportRunning; }
            set { SetProperty(ref _isReportRunning, value); }
        }



        private bool _isCampaignTypeSelected;
        public bool IsCampaignTypeSelected
        {
            get { return _isCampaignTypeSelected; }
            set { SetProperty(ref _isCampaignTypeSelected, value); }
        }



        private bool _isUpgradeCampaignSelected;
        public bool IsUpgradeCampaignSelected
        {
            get { return _isUpgradeCampaignSelected; }
            set { SetProperty(ref _isUpgradeCampaignSelected, value); }
        }



        private List<LookupType> _campaignTypes = new List<LookupType>();
        public List<LookupType> CampaignTypes
        {
            get { return _campaignTypes; }
            set { SetProperty(ref _campaignTypes, value); }
        }



        private LookupType _selectedCampaignType;
        public LookupType SelectedCampaignType
        {
            get { return _selectedCampaignType; }
            set
            {
                SetProperty(ref _selectedCampaignType, value);
                ReportCommand.RaiseCanExecuteChanged();

                IsCampaignTypeSelected = _selectedCampaignType != null ? true : false;

                if (_selectedCampaignType == null)
                {
                    SelectedUpgradeCampaign = null;
                }

                LoadLookupData2();
            }
        }



        private List<LookupType> _upgradeCampaigns = new List<LookupType>();
        public List<LookupType> UpgradeCampaigns
        {
            get { return _upgradeCampaigns; }
            set 
            { 
                SetProperty(ref _upgradeCampaigns, value);

                TitleCampaigns = "Included Campaigns " + "[" + UpgradeCampaigns.Count + "]";
            }
        }



        private LookupType _selectedUpgradeCampaign;
        public LookupType SelectedUpgradeCampaign
        {
            get { return _selectedUpgradeCampaign; }
            set
            {
                SetProperty(ref _selectedUpgradeCampaign, value);
                //ReportCommand.RaiseCanExecuteChanged();

                IsUpgradeCampaignSelected = _selectedUpgradeCampaign != null ? true : false;

                if (_selectedUpgradeCampaign == null)
                {
                    SelectedBatch = null;
                }

                LoadLookupData3();
            }
        }



        private List<LookupType> _batches = new List<LookupType>();
        public List<LookupType> Batches
        {
            get { return _batches; }
            set { SetProperty(ref _batches, value); }
        }

        private LookupType _selectedBatch;
        public LookupType SelectedBatch
        {
            get { return _selectedBatch; }
            set
            {
                SetProperty(ref _selectedBatch, value);
                ReportCommand.RaiseCanExecuteChanged();
            }
        }



        private DateTime _firstDayOfSelectedMonth;
        public DateTime FirstDayOfSelectedMonth
        {
            get { return _firstDayOfSelectedMonth; }
            set { SetProperty(ref _firstDayOfSelectedMonth, value); }
        }



        private DateTime _lastDayOfSelectedMonth;
        public DateTime LastDayOfSelectedMonth
        {
            get { return _lastDayOfSelectedMonth; }
            set { SetProperty(ref _lastDayOfSelectedMonth, value); }
        }



        private DateTime _selectedDate = DateTime.Now.Date;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set 
            { 
                SetProperty(ref _selectedDate, value);
                FirstDayOfSelectedMonth = new DateTime(SelectedDate.Year, SelectedDate.Month, 1);
                LastDayOfSelectedMonth = FirstDayOfSelectedMonth.AddMonths(1).AddDays(-1);
                LoadLookupData2();
                ReportCommand.RaiseCanExecuteChanged();
            }
        }



        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;



        #endregion



        #region Members

        public IEventAggregator _ea = new EventAggregator();



        #endregion



        #region Commands

        public DelegateCommand ReportCommand { get; private set; }
        private bool ReportCommandCanExecute()
        {
            return 
                !IsReportRunning &&
                SelectedCampaignType != null &&
                UpgradeCampaigns.Count > 0;
                //SelectedUpgradeCampaign != null;// && 
                //SelectedBatch != null;
        }
        private void ReportCommandExecute()
        {
            try
            {
                dispatcherTimer1.Start();
                IsReportRunning = true;
                ReportCommand.RaiseCanExecuteChanged();

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += Report;
                worker.RunWorkerCompleted += ReportCompleted;
                worker.RunWorkerAsync();
            }

            catch (Exception ex)
            {
                new BaseControl().HandleException(ex);
            }
        }

        #endregion



        #region Constructors

        public ReportDateAnalysisScreenViewModel()
        {
            ReportCommand = new DelegateCommand(ReportCommandExecute, ReportCommandCanExecute);

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);

            LoadLookupData1();
        }

        #endregion



        #region Methods

        public void LoadLookupData1()
        {
            try
            {
                DataTable dt = Methods.ExecuteStoredProcedure2("spINReportDateAnalysisLookup1", null, IsolationLevel.Snapshot, 30).Tables[0];

                CampaignTypes = (from row in dt.AsEnumerable()
                                 select new LookupType()
                                 {
                                     ID = Convert.ToInt32(row["ID"]),
                                     Name = Convert.ToString(row["Description"])
                                 }).ToList();

                //CampaignTypes = CampaignTypes.OrderBy(o => o.Name).ToList();
            }

            catch (Exception ex)
            {
                new BaseControl().HandleException(ex);
            }
        }

        public void LoadLookupData2()
        {
            try
            {
                if (SelectedCampaignType != null)
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@CampaignTypeID", SelectedCampaignType.ID);
                    parameters[1] = new SqlParameter("@StartDate", FirstDayOfSelectedMonth);
                    parameters[2] = new SqlParameter("@EndDate", LastDayOfSelectedMonth);

                    DataTable dt = Methods.ExecuteStoredProcedure2("spINReportDateAnalysisLookup2", parameters, IsolationLevel.Snapshot, 30).Tables[0];

                    UpgradeCampaigns = (from row in dt.AsEnumerable()
                                     select new LookupType()
                                     {
                                         ID = Convert.ToInt32(row["CampaignID"]),
                                         Name = Convert.ToString(row["CampaignCode"])
                                     }).ToList();
                }
            }

            catch (Exception ex)
            {
                new BaseControl().HandleException(ex);
            }
        }
        
        public void LoadLookupData3()
        {
            try
            {
                if (SelectedUpgradeCampaign != null)
                {
                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@CampaignID", SelectedUpgradeCampaign.ID);
                    parameters[1] = new SqlParameter("@StartDate", FirstDayOfSelectedMonth);
                    parameters[2] = new SqlParameter("@EndDate", LastDayOfSelectedMonth);

                    DataTable dt = Methods.ExecuteStoredProcedure2("spINReportDateAnalysisLookup3", parameters, IsolationLevel.Snapshot, 30).Tables[0];

                    Batches = (from row in dt.AsEnumerable()
                                        select new LookupType()
                                        {
                                            ID = Convert.ToInt32(row["BatchID"]),
                                            Name = Convert.ToString(row["BatchCode"])
                                        }).ToList();
                }
            }

            catch (Exception ex)
            {
                new BaseControl().HandleException(ex);
            }
        }

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                #region Setup excel documents

                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                string filePathAndName = $"{GlobalSettings.UserFolder}Date Analysis Report ({SelectedCampaignType.Name}) [{SelectedDate.ToString("MMM")}-{SelectedDate.Year}], {DateTime.Now.ToString("yyyy-MM-dd HHmmss")}.xlsx";

                Uri uri = new Uri("/Templates/ReportTemplateDateAnalysis.xlsx", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    wbTemplate = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }

                Worksheet wsTemplate = wbTemplate.Worksheets["Batch"];
                Worksheet wsReport;

                #endregion

                #region Get report data from database

                string reportSubtitle = $@"{SelectedCampaignType.Name} {SelectedDate.ToString("MMM")}-{SelectedDate.Year}"; // - {SelectedBatch.Name}

                
                //SelectedUpgradeCampaign = UpgradeCampaigns[0];

                //Batch Loop

                foreach (LookupType campaign in UpgradeCampaigns)
                {
                    //SelectedBatch = batch;

                    SqlParameter[] parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@CampaignID", campaign.ID);
                    parameters[1] = new SqlParameter("@StartDate", FirstDayOfSelectedMonth);
                    parameters[2] = new SqlParameter("@EndDate", LastDayOfSelectedMonth);

                    DataSet dsReportData = Methods.ExecuteStoredProcedure2("spINReportDateAnalysis", parameters, IsolationLevel.ReadUncommitted, 300);
                    
                    #endregion

                    #region Report Data

                    //if (dsReportData.Tables.Count < 1)
                    //{
                    //    DialogMessage dm = new DialogMessage
                    //    {
                    //        Message = $"There is no data for the selected campaign and batch.",
                    //        Title = "No Data",
                    //        Type = ShowMessageType.Exclamation
                    //    };

                    //    Application.Current?.Dispatcher?.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    //    {
                    //        _ea.GetEvent<SendDialogMessageEvent>().Publish(dm);
                    //    });
                    //}
                    if (dsReportData.Tables.Count > 0)
                    {
                        DataTable dtReportData = dsReportData.Tables[0];

                        if (dtReportData.Rows.Count > 0)
                        {
                            wsReport = wbReport.Worksheets.Add(campaign.Name);

                            int columnCount = dtReportData.Columns.Count;
                            int rowIndex = 5;
                            int maxColumns = 120;
                            int maxUpgrade = 18;

                            #region Report Header

                            Methods.CopyExcelRegion(wsTemplate, 0, 0, 4, columnCount - 1, wsReport, 0, 0);

                            WorksheetMergedCellsRegion mergedRegion;

                            mergedRegion = wsReport.MergedCellsRegions.Add(0, 0, 0, 4);

                            mergedRegion = wsReport.MergedCellsRegions.Add(2, 0, 2, 4);
                            mergedRegion.Value = reportSubtitle;

                            mergedRegion = wsReport.MergedCellsRegions.Add(3, 0, 3, 4);
                            mergedRegion.Value = $"Report Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";

                            int columnIndex = 0;
                            WorksheetCell wsCell;
                            foreach (DataColumn column in dtReportData.Columns)
                            {
                                if (!column.ColumnName.Contains("BatchID"))
                                { 
                                    Methods.CopyExcelRegion(wsTemplate, 5, 0, 0, 0, wsReport, rowIndex, columnIndex);

                                    wsReport.Columns[columnIndex].Width = 4800;

                                    wsCell = wsReport.Rows[rowIndex].Cells[columnIndex];
                                    wsCell.Value = column.ColumnName;

                                    columnIndex++;
                                }
                            }

                            rowIndex++;

                            #endregion

                            #region Report Details

                            string[] addressLetterBatchDate = new string[maxUpgrade];
                            string[] addressLetterDateOfSale = new string[maxUpgrade];

                            foreach (DataRow row in dtReportData.Rows)
                            {
                                int columnDataIndex = 0;
                                int columnReportIndex = 0;

                                for (int column = 0; column < columnCount; column++)
                                {
                                    wsCell = wsReport.Rows[rowIndex].Cells[columnReportIndex];
                                    DataColumn dataColumn = row.Table.Columns[columnDataIndex];

                                    if (!dataColumn.ColumnName.Contains("BatchID"))
                                    {
                                        Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, 0, wsReport, rowIndex, columnReportIndex);

                                        if (dataColumn.ColumnName.Contains("Date"))
                                        {
                                            wsReport.Columns[columnReportIndex].Width = 4800;
                                            wsCell.CellFormat.Alignment = HorizontalCellAlignment.Center;

                                            if (row[columnDataIndex] != null && row[columnDataIndex] != DBNull.Value)
                                            {
                                                wsCell.Value = Convert.ToDateTime(row[columnDataIndex]).ToString("yyyy-MM-dd");
                                            }
                                        }
                                        else if (dataColumn.ColumnName.Contains("TimeFrame"))
                                        {
                                            wsReport.Columns[columnReportIndex].Width = 4800;

                                            wsCell.Value = row[columnDataIndex];

                                            //decimal daysPerMonth = 30.3m;
                                            //int strRow = (rowIndex + 1);
                                            //string formula = string.Empty;

                                            //int indexTimeFrame = Convert.ToInt32(dataColumn.ColumnName.Replace("TimeFrame", ""));

                                            //if (strRow == rowIndex + 1)
                                            //{
                                            //    WorksheetCell wsCellHeaderBatchDate = Methods.WorksheetFindText(wsReport, "BatchImportDate" + indexTimeFrame, 0, 0, 10, maxColumns, true, true);
                                            //    addressLetterBatchDate[indexTimeFrame] = wsCellHeaderBatchDate?.ToString()?.Split('$')[1];

                                            //    WorksheetCell wsCellHeaderDateOfSale = Methods.WorksheetFindText(wsReport, "DateOfSale" + (indexTimeFrame + 1), 0, 0, 10, maxColumns, true, true);
                                            //    addressLetterDateOfSale[indexTimeFrame] = wsCellHeaderDateOfSale?.ToString()?.Split('$')[1];
                                            //}


                                            //formula = $"=(IF(ISBLANK({addressLetterDateOfSale[indexTimeFrame]}{strRow});;{addressLetterBatchDate[indexTimeFrame]}{strRow}-{addressLetterDateOfSale[indexTimeFrame]}{strRow})/{daysPerMonth})";

                                            //wsCell.ApplyFormula(formula);

                                            //switch (dataColumn.ColumnName)
                                            //{
                                            //    case "TimeFrame1":
                                            //        WorksheetCell wsCellHeader = Methods.WorksheetFindText(wsReport, "TimeFrame1", 0, 0, 10, 10, true, true);
                                            //        int cellColumn = wsCellHeader.ColumnIndex;

                                            //        formula = $"=(D{strRow}-J{strRow})/{daysPerMonth}";
                                            //        break;
                                            //}

                                            wsCell.CellFormat.FormatString = "0";
                                            wsCell.CellFormat.Alignment = HorizontalCellAlignment.Center;
                                        }
                                        else
                                        {
                                            wsReport.Columns[columnReportIndex].Width = 4800;
                                            wsCell.Value = row[columnDataIndex];
                                        }

                                        columnReportIndex++;
                                    }

                                    columnDataIndex++;
                                }

                                rowIndex++;
                            }

                            columnIndex = 0;
                            int dataIndex = 0;
                            DataTable dtReportDataAverages = dsReportData.Tables[1];

                            if (dtReportDataAverages.Rows.Count > 0)
                            { 
                                foreach (DataColumn column in dtReportData.Columns)
                                {
                                    if (!column.ColumnName.Contains("BatchID"))
                                    {
                                        if (column.ColumnName.Contains("TimeFrame"))
                                        {
                                            Methods.CopyExcelRegion(wsTemplate, 5, 0, 0, 0, wsReport, rowIndex, columnIndex);

                                            wsCell = wsReport.Rows[rowIndex].Cells[columnIndex];
                                            wsCell.Value = dtReportDataAverages.Rows[0][dataIndex];
                                            wsCell.CellFormat.BottomBorderStyle = CellBorderLineStyle.Double;

                                            dataIndex++;
                                        }

                                        columnIndex++;
                                    }
                                }
                            }

                            #endregion
                        }

                    }

                }

                //end batch loop


                #endregion

                if (wbReport.Worksheets.Count > 0)
                {
                    //Save excel document
                    wbReport.Save(filePathAndName);

                    //Display excel document
                    Process.Start(filePathAndName);
                }
            }

            catch (Exception ex)
            {
                Dispatcher dispatcher = Application.Current.Dispatcher;

                dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    new BaseControl().HandleException(ex);
                });
            }
        }

        private void ReportCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dispatcherTimer1.Stop();
            _timer1 = 0;
            IsReportRunning = false;

            UpdateUIMessage um = new UpdateUIMessage
            {
                ControlName = "btnReport",
                Property = "Content",
                Value = "Report"
            };
            _ea.GetEvent<UpdateUIMessageEvent>().Publish(um);
            
            ReportCommand.RaiseCanExecuteChanged();
        }

        private void Timer1(object sender, EventArgs e)
        {
            _timer1++;
            UpdateUIMessage um;

            um = new UpdateUIMessage
            {
                ControlName = "btnReport",
                Property = "Content",
                Value = TimeSpan.FromSeconds(_timer1).ToString()
            };
            _ea.GetEvent<UpdateUIMessageEvent>().Publish(um);

            um = new UpdateUIMessage
            {
                ControlName = "btnReport",
                Property = "ToolTip",
                Value = TimeSpan.FromSeconds(_timer1).ToString() + "s"
            };
            _ea.GetEvent<UpdateUIMessageEvent>().Publish(um);
        }

        #endregion

    }
}
