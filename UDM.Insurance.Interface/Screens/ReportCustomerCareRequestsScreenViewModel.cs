using Embriant.Framework;
using Embriant.Framework.Configuration;
using Embriant.WPF.Controls;
using Infragistics.Documents.Excel;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Resources;
using System.Windows.Threading;
using UDM.Insurance.Interface.PrismInfrastructure;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Screens
{
    class ReportCustomerCareRequestsScreenViewModel : BindableBase
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

        private string _title = "Customer Care Requests Report";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }



        private bool _isReportRunning;
        public bool IsReportRunning
        {
            get { return _isReportRunning; }
            set { SetProperty(ref _isReportRunning, value); }
        }



        private DateTime? _selectedFromDate; // = DateTime.Now.Date
        public DateTime? SelectedFromDate
        {
            get { return _selectedFromDate; }
            set 
            { 
                SetProperty(ref _selectedFromDate, value);
                ReportCommand.RaiseCanExecuteChanged();
            }
        }

        private DateTime? _selectedToDate; // = DateTime.Now.Date
        public DateTime? SelectedToDate
        {
            get { return _selectedToDate; }
            set 
            { 
                SetProperty(ref _selectedToDate, value);
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
                (SelectedFromDate != null && SelectedFromDate <= SelectedToDate) &&
                (SelectedToDate != null && SelectedToDate >= SelectedFromDate);
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

        public ReportCustomerCareRequestsScreenViewModel()
        {
            ReportCommand = new DelegateCommand(ReportCommandExecute, ReportCommandCanExecute);

            dispatcherTimer1.Tick += Timer1;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion



        #region Methods

        private void Report(object sender, DoWorkEventArgs e)
        {
            try
            {
                #region Setup excel documents

                Workbook wbTemplate;
                Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);

                string filePathAndName = $"{GlobalSettings.UserFolder}Customer Care Requests [{Convert.ToDateTime(SelectedFromDate).ToString("yyyy-MM-dd")} to {Convert.ToDateTime(SelectedToDate).ToString("yyyy-MM-dd")}], {DateTime.Now.ToString("ffff")}.xlsx";

                Uri uri = new Uri("/Templates/ReportTemplateCustomerCareRequests.xlsx", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    wbTemplate = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }

                Worksheet wsTemplate = wbTemplate.Worksheets["Customer Care Requests"];
                Worksheet wsReport;

                #endregion

                #region Get report data from database

                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@FromDate", SelectedFromDate);
                parameters[1] = new SqlParameter("@ToDate", SelectedToDate);

                DataSet dsReportData = Methods.ExecuteStoredProcedure2("spINReportCustomerCareRequests", parameters, IsolationLevel.ReadUncommitted, 300);

                #endregion

                #region Report Data

                if (dsReportData.Tables.Count < 1)
                {
                    DialogMessage dm = new DialogMessage
                    {
                        Message = $"There is no data for the selected date range.",
                        Title = "No Data",
                        Type = ShowMessageType.Exclamation
                    };

                    Application.Current?.Dispatcher?.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                    {
                        _ea.GetEvent<SendDialogMessageEvent>().Publish(dm);
                    });
                }
                else
                {
                    DataTable dtReportData = dsReportData.Tables[0];

                    if (dtReportData.Rows.Count > 0)
                    {
                        wsReport = wbReport.Worksheets.Add("Customer Care Requests");

                        int columnCount = dtReportData.Columns.Count;
                        int rowIndex = 0;

                        #region Report Header

                        //Methods.CopyExcelRegion(wsTemplate, 0, 0, 4, columnCount - 1, wsReport, 0, 0);

                        //WorksheetMergedCellsRegion mergedRegion;

                        //mergedRegion = wsReport.MergedCellsRegions.Add(0, 0, 0, 4);

                        //mergedRegion = wsReport.MergedCellsRegions.Add(2, 0, 2, 4);
                        //mergedRegion.Value = reportSubtitle;

                        //mergedRegion = wsReport.MergedCellsRegions.Add(3, 0, 3, 4);
                        //mergedRegion.Value = $"Report Date: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";

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

                        foreach (DataRow row in dtReportData.Rows)
                        {
                            int columnDataIndex = 0;
                            int columnReportIndex = 0;

                            for (int column = 0; column < columnCount; column++)
                            {
                                wsCell = wsReport.Rows[rowIndex].Cells[columnReportIndex];
                                DataColumn dataColumn = row.Table.Columns[columnDataIndex];

                                Methods.CopyExcelRegion(wsTemplate, 6, 0, 0, 0, wsReport, rowIndex, columnReportIndex);

                                wsReport.Columns[columnReportIndex].Width = 4800;
                                wsCell.Value = row[columnDataIndex];

                                if (dataColumn.ColumnName.Contains("Date"))
                                {
                                    wsReport.Columns[columnReportIndex].Width = 4800;
                                    wsCell.CellFormat.Alignment = HorizontalCellAlignment.Center;
                                    //wsCell.Value = row[columnDataIndex];
                                }
                                if (dataColumn.ColumnName.Contains("Client Name"))
                                {
                                    wsReport.Columns[columnReportIndex].Width = 9000;
                                    wsCell.CellFormat.Alignment = HorizontalCellAlignment.Left;
                                    //wsCell.Value = row[columnDataIndex];
                                }
                                if (dataColumn.ColumnName.Contains("Notes"))
                                {
                                    wsReport.Columns[columnReportIndex].Width = 18000;
                                    wsCell.CellFormat.Alignment = HorizontalCellAlignment.Left;
                                    //wsCell.Value = row[columnDataIndex];
                                }

                                columnReportIndex++;
                                columnDataIndex++;
                            }

                            rowIndex++;
                        }

                        #endregion
                    }
                    else
                    {
                        DialogMessage dm = new DialogMessage
                        {
                            Message = $"There is no data for the selected date range.",
                            Title = "No Data",
                            Type = ShowMessageType.Exclamation
                        };

                        Application.Current?.Dispatcher?.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            _ea.GetEvent<SendDialogMessageEvent>().Publish(dm);
                        });
                    }
                }

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
