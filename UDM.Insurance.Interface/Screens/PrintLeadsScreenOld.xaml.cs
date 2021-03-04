using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Resources;
using System.Windows.Threading;
using Embriant.Framework;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using Infragistics.Documents.Excel;
using Infragistics.Windows.Editors;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;


namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for PrintLeadsScreen.xaml
	/// </summary>
	public partial class PrintLeadsScreen
    {

        #region Constants

        #endregion



        #region Private Members
        private readonly long _batchID;
        private readonly long _agentID;
        private long _leadBookID;
        private DateTime _fromDate = DateTime.Now;
        private DateTime _toDate = DateTime.Now;
        private DataTable _dtSummaryData;

        private readonly DispatcherTimer _TimerPrint = new DispatcherTimer();
        private int _PrintTime;
        #endregion

        

        #region Constructors

        public PrintLeadsScreen(long batchID, long agentID)
        {
            InitializeComponent();

            _batchID = batchID;
            _agentID = agentID;

            LoadLookupData();

            _TimerPrint.Tick += TimerPrint;
            _TimerPrint.Interval = new TimeSpan(0, 0, 1);
        }

        #endregion



        #region Private Methods

        private void LoadLookupData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataSet ds = Insure.INGetAgentPrintLeadsData(_batchID, _agentID);
                _dtSummaryData = ds.Tables[0];

                tbCampaign.Text = _dtSummaryData.Rows[0]["CampaignName"].ToString();
                tbBatch.Text = _dtSummaryData.Rows[0]["BatchCode"].ToString();
                tbAgent.Text = _dtSummaryData.Rows[0]["SalesAgent"].ToString();
                tbLeadsAllocated.Text = _dtSummaryData.Rows[0]["LeadsAllocated"].ToString();
                tbLeadsPrinted.Text = _dtSummaryData.Rows[0]["LeadsPrinted"].ToString();
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

        private void xamEditor_Select(object sender)
        {
            switch (sender.GetType().Name)
            {
                case "XamMaskedEditor":
                    var xamMEDControl = (XamMaskedEditor)sender;

                    switch (xamMEDControl.Name)
                    {
                        default:
                            xamMEDControl.SelectAll();
                            break;
                    }
                    break;

                case "XamDateTimeEditor":
                    var xamDTEControl = (XamDateTimeEditor)sender;

                    switch (xamDTEControl.Name)
                    {
                        default:
                            if (xamDTEControl.IsInEditMode)
                            {
                                xamDTEControl.SelectAll();
                            }
                            break;
                    }
                    break;
            }
        }

        private void displayLeadsToPrintCount()
        {
            DataTable dt = Insure.INGetLeadsToPrintCount(_batchID, _agentID, _fromDate, _toDate).Tables[0];
            tbLeadsToPrint.Text = dt.Rows[0][0].ToString();

            btnPrint.IsEnabled = (int)dt.Rows[0][0] > 0;
            btnPrint.Content = "Print";
            _PrintTime = 0;
        }

        private void printLeads()
        {
            try
            {
                Database.BeginTransaction(null, IsolationLevel.Snapshot);

                #region get lead data
				DataSet ds;
				if (tbCampaign.Text == "ACCDIS")
				{
					ds = Insure.INGetLeadsForUserAndBatchACCDIS(_batchID, _agentID);
				}
				else
				{
					ds = Insure.INGetLeadsForUserAndBatch(_batchID, _agentID, _fromDate, _toDate);
				}
				DataTable dtLeadPrintData = ds.Tables[0];
                #endregion

                #region setup excel document
                Workbook workBook;

                Uri uri = new Uri("/Resources/LeadPrintTemplate.xlsx", UriKind.Relative);
                StreamResourceInfo info = Application.GetResourceStream(uri);
                if (info != null)
                {
                    workBook = Workbook.Load(info.Stream, true);
                }
                else
                {
                    return;
                }

                Worksheet wsCoverPage = workBook.Worksheets["Cover"];
                wsCoverPage.DisplayOptions.ShowGridlines = false;
                wsCoverPage.DisplayOptions.MagnificationInPageLayoutView = 100;
                wsCoverPage.DisplayOptions.View = WorksheetView.PageLayout;

                string filePathAndName = GlobalSettings.UserFolder + tbAgent.Text + " (" + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + ")" + ".xlsx";
                #endregion

                #region populate cover page
                WorksheetCell wsCell = wsCoverPage.Rows[15].Cells[6];
                wsCell.Value = tbCampaign.Text;

                wsCell = wsCoverPage.Rows[16].Cells[6];
                wsCell.Value = tbAgent.Text;

                wsCell = wsCoverPage.Rows[17].Cells[6];
                wsCell.Value = tbLeadsToPrint.Text;

                wsCell = wsCoverPage.Rows[18].Cells[6];
                long pages = Convert.ToInt64(tbLeadsToPrint.Text) / 5;
                long remainder = Convert.ToInt64(tbLeadsToPrint.Text) % 5;
                if (remainder > 0) pages++;
                wsCell.Value = pages;

                wsCell = wsCoverPage.Rows[19].Cells[6];
                wsCell.Value = tbCampaign.Text + " " + tbBatch.Text;

                wsCell = wsCoverPage.Rows[20].Cells[6];
                DateTime nextMonday = Methods.NextWeekDay(DateTime.Now, DayOfWeek.Monday);
                wsCell.Value = nextMonday.ToString("yyyy/MM/dd");

                wsCell = wsCoverPage.Rows[21].Cells[6];
                wsCell.Value = nextMonday.AddDays(63).ToString("yyyy/MM/dd");
                #endregion

                #region Create leadbook entry
                INLeadBook inLeadBook = new INLeadBook();
                inLeadBook.FKUserID = _agentID;
                inLeadBook.FKINBatchID = _batchID;
                inLeadBook.Description = nextMonday.ToString().Substring(0, 10) + "-" + tbCampaign.Text.Replace(" ", "") + "-" + tbBatch.Text.Replace(" ", "");
                inLeadBook.Save(_validationResult);
                _leadBookID = inLeadBook.ID;
                #endregion

                #region populate leads worksheet
                const string wsName = "Leads"; //tbAgent.Text.Length > 31 ? tbAgent.Text.Substring(0, 30) : tbAgent.Text;
                Worksheet wsAgentLeads = workBook.Worksheets.Add(wsName);
                Worksheet wsLT1 = workBook.Worksheets["LT1"];

                wsAgentLeads.MoveToIndex(1);
                wsAgentLeads.PrintOptions.PaperSize = PaperSize.A4;
                wsAgentLeads.PrintOptions.Orientation = Orientation.Landscape;
                wsAgentLeads.PrintOptions.LeftMargin = 0.8;
                wsAgentLeads.PrintOptions.TopMargin = 0.8;
                wsAgentLeads.PrintOptions.RightMargin = 0.8;
                wsAgentLeads.PrintOptions.BottomMargin = 0.8;
                wsAgentLeads.DisplayOptions.ShowGridlines = false;
                wsAgentLeads.DisplayOptions.MagnificationInPageLayoutView = 100;
                wsAgentLeads.DisplayOptions.View = WorksheetView.PageLayout;
                wsAgentLeads.PrintOptions.PageNumbering = PageNumbering.Automatic;
                wsAgentLeads.PrintOptions.Header = "&L&B" + tbAgent.Text + "&C&B" + tbCampaign.Text + " Batch " + tbBatch.Text + "&R&BPage &P of &N";
                wsAgentLeads.PrintOptions.Footer = "&C&B" + nextMonday.ToString("yyyy/MM/dd");

                //populate lead sheet
                int leadNo = 0;
                int rowsToPrint = Convert.ToInt32(dtLeadPrintData.Rows.Count) * 5;
                for (int leadRow = 0; leadRow < rowsToPrint; leadRow = leadRow + 5)
                {
                    for (int row = 0; row <= 4; row++)
                    {
                        for (int column = 0; column <= 15; column++)
                        {
                            WorksheetCell targetCell = wsAgentLeads.Rows[leadRow + row].Cells[column];
                            WorksheetCell tempCell = wsLT1.Rows[row].Cells[column];

                            if (row != 4)
                            {
                                targetCell.Value = tempCell.Value;
                                targetCell.CellFormat.SetFormatting(tempCell.CellFormat);

                                switch (row)
                                {
                                    case 1:
                                        if (column <= 14)
                                        {
                                            targetCell.Value = dtLeadPrintData.Rows[leadNo][column].ToString();
                                        }
                                        break;
                                }
                            }

                            if (leadRow == 0 && row == 0)
                            {
                                wsAgentLeads.Columns[column].Width = wsLT1.Columns[column].Width;
                            }

                        }

                        wsAgentLeads.Rows[leadRow + row].Height = wsLT1.Rows[row].Height;

                        if (row == 4)
                        {
                            INImport inImport = new INImport(long.Parse(dtLeadPrintData.Rows[leadNo]["ImportID"].ToString()));
                            inImport.IsPrinted = 1;
                            inImport.Save(_validationResult);

                            //Keep import print order for Status Loading
                            INLeadBookImport inLeadBookImport = new INLeadBookImport();
                            inLeadBookImport.FKINLeadBookID = _leadBookID;
                            inLeadBookImport.FKINImportID = long.Parse(dtLeadPrintData.Rows[leadNo]["ImportID"].ToString());
                            inLeadBookImport.Save(_validationResult);

                            leadNo++;
                        }
                    }

                    //update print lead interface
                    tbLeadsPrinted.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                    {
                        tbLeadsPrinted.Text = (Convert.ToInt64(tbLeadsPrinted.Text) + 1).ToString(CultureInfo.InvariantCulture);
                    }));
                    tbLeadsToPrint.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                    {
                        tbLeadsToPrint.Text = (Convert.ToInt64(tbLeadsToPrint.Text) - 1).ToString(CultureInfo.InvariantCulture);
                    }));
                }
                #endregion

                #region save and display excel document
                workBook.SetCurrentFormat(WorkbookFormat.Excel2007);
                workBook.WindowOptions.SelectedWorksheet = wsCoverPage;
                workBook.Worksheets.Remove(wsLT1);
                workBook.Save(filePathAndName);

                //display excel document
                Process.Start(filePathAndName);
                #endregion

                CommitTransaction(null);
            }

            catch (Exception ex)
            {
                Database.CancelTransactions();
                ShowMessageBox(new INMessageBoxWindow1(), "An error has occurred.\nThe lead print job will now be cancelled.", "Lead Print Error", ShowMessageType.Error);
                btnClose.IsEnabled = true;
                HandleException(ex);
            }
        }

        #endregion



        #region Event Handlers

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

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
           OnDialogClose(false);
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            SetCursor(Cursors.Wait);

            btnPrint.IsEnabled = false;
            btnClose.IsEnabled = false;

            _TimerPrint.Start();
            printLeads();
            _TimerPrint.Stop();

            btnClose.IsEnabled = true;
            SetCursor(Cursors.Arrow);
        }

        private void dtePrintFrom_Loaded(object sender, RoutedEventArgs e)
        {
            dtePrintFrom.Focus();
        }

        private void xamEditor_GotFocus(object sender, RoutedEventArgs e)
        {
            xamEditor_Select(sender);
        }

        private void dtePrintFrom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            btnPrint.IsEnabled = false;

            if (dtePrintFrom.Value != null)
            {
                if (DateTime.TryParse(dtePrintFrom.Value.ToString(), out _fromDate))
                {
                    if (dtePrintTo.Value != null)
                    {
                        if (DateTime.TryParse(dtePrintTo.Value.ToString(), out _toDate))
                        {
                            if (_toDate >= _fromDate)
                            {
                                displayLeadsToPrintCount();
                            }
                        }
                    }
                }
            }
        }

        private void dtePrintTo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            btnPrint.IsEnabled = false;

            if (dtePrintTo.Value != null)
            {
                if (DateTime.TryParse(dtePrintTo.Value.ToString(), out _toDate))
                {
                    if (dtePrintFrom.Value != null)
                    {
                        if (DateTime.TryParse(dtePrintFrom.Value.ToString(), out _fromDate))
                        {
                            if (_toDate >= _fromDate)
                            {
                                displayLeadsToPrintCount();
                            }
                        }
                    }
                }
            }
        }

        #endregion

	}
}

