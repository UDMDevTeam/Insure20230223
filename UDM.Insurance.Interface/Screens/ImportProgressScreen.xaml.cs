using Embriant.Framework;
using Embriant.Framework.Configuration;
using Embriant.Framework.Data;
using Embriant.Framework.Library;
using Infragistics.Documents.Excel;
using Infragistics.Windows.Editors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Resources;
using System.Windows.Threading;
using UDM.Insurance.Business;
using UDM.Insurance.Business.Mapping;
using UDM.Insurance.Interface.Data;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;


namespace UDM.Insurance.Interface.Screens
{

	public partial class ImportProgressScreen
    {

        #region Constants

        private const string ImportFilterExpression = "(.xls;.xlsx;.xlsm)|*.xlsx;*.xls;*.xlsm";
        private const string ImportFileExtention = ".xls";
        private readonly Enum[] _upgrades =
        {
            lkpINCampaignGroup.Upgrade1,
            lkpINCampaignGroup.Upgrade2,
            lkpINCampaignGroup.Upgrade3,
            lkpINCampaignGroup.Upgrade4,
            lkpINCampaignGroup.Upgrade5,
            lkpINCampaignGroup.Upgrade6,
            lkpINCampaignGroup.Upgrade7,
            lkpINCampaignGroup.Upgrade8,
            lkpINCampaignGroup.Upgrade9,
            lkpINCampaignGroup.Upgrade10,
            lkpINCampaignGroup.Upgrade11,
            lkpINCampaignGroup.Upgrade12,
            lkpINCampaignGroup.DoubleUpgrade1,
            lkpINCampaignGroup.DoubleUpgrade2,
            lkpINCampaignGroup.DoubleUpgrade3,
            lkpINCampaignGroup.DoubleUpgrade4,
            lkpINCampaignGroup.DoubleUpgrade5,
            lkpINCampaignGroup.DoubleUpgrade6,
            lkpINCampaignGroup.DoubleUpgrade7,
            lkpINCampaignGroup.DoubleUpgrade8,
            lkpINCampaignGroup.DoubleUpgrade9,
            lkpINCampaignGroup.DoubleUpgrade10,
            lkpINCampaignGroup.DoubleUpgrade11,
            lkpINCampaignGroup.DoubleUpgrade12,
            lkpINCampaignGroup.DoubleUpgrade13,
            lkpINCampaignGroup.DoubleUpgrade14,
            lkpINCampaignGroup.DefrostR99,
            lkpINCampaignGroup.Upgrade13, 
            lkpINCampaignGroup.R99,
            lkpINCampaignGroup.Lite

        };

        #endregion



        #region Private Members

        private DataTable _dtCampaigns;
        //private DataTable _dtINTitles;
        //private DataTable _dtINRelationships;
        //private DataTable _dtLanguage;
        //private DataTable _dtGender;

        public static INCampaign _inCampaign;
        public static INBatch _inBatch;
        private string _batchCode;
        private string _batchCodeModifier;
        private string _batchCode2;
        //private string _campaignName; 
        private string _campaignCode;        
        private Workbook _workbook;
        private Worksheet _workSheet;
        int _rowCount;
        int _headerRow;

        private readonly DispatcherTimer _TimerOpenWorkBook = new DispatcherTimer();
        private readonly DispatcherTimer _TimerCheckDuplicates = new DispatcherTimer();
        private readonly DispatcherTimer _TimerImport = new DispatcherTimer();
        private int _ImportTime;

        private bool? _fileError;

        class ImportHeaderInfo
        {
            public ImportHeaderInfo()
            {
                Index = -1;
            }

            public string Header { get; set; }
            public string HeaderAlt1 { get; set; }
            public string HeaderAlt2 { get; set; }
            public string HeaderAlt3 { get; set; }
            public int Index { get; set; }
            public bool IgnoreIfNA { get; set; }
        }

	    private Dictionary<string, ImportHeaderInfo> idxFields;

        #endregion

        

        #region Constructors

        public ImportProgressScreen()
        {
            InitializeComponent();

            _TimerOpenWorkBook.Tick += TimerOpenWorkBook;
            _TimerOpenWorkBook.Interval = new TimeSpan(0, 0, 0, 0, 300);

            _TimerCheckDuplicates.Tick += TimerCheckDuplicates;
            _TimerCheckDuplicates.Interval = new TimeSpan(0, 0, 0, 0, 300);

            _TimerImport.Tick += TimerImport;
            _TimerImport.Interval = new TimeSpan(0, 0, 1);

            LoadLookupData();
        }

        #endregion



        #region Private Methods

        private void TimerOpenWorkBook(object sender, EventArgs e)
        {
            try
            {
                string str = pbImportText.Text;
                int strLength = str.Length;
                char chr = ' ';

                switch (str[strLength - 1])
                {
                    case '-':
                        chr = '\x00F7';
                        break;

                    case '\x00F7':
                        chr = '-';
                        break;
                }

                str = str.Substring(0, strLength - 1) + chr;
                pbImportText.Text = str;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void TimerCheckDuplicates(object sender, EventArgs e)
        {
            try
            {
                string str = pbImportText.Text;
                int strLength = str.Length;
                char chr = ' ';

                switch (str[strLength - 1])
                {
                    case '-':
                        chr = '\x00F7';
                        break;

                    case '\x00F7':
                        chr = '-';
                        break;
                }

                str = str.Substring(0, strLength - 1) + chr;
                pbImportText.Text = str;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void TimerImport(object sender, EventArgs e)
        {
            try
            {
                _ImportTime++;
                btnImport.Content = TimeSpan.FromSeconds(_ImportTime).ToString();
                //btnImport.ToolTip = btnImport.Content;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void LoadLookupData()
        {
            try
            {
                SetCursor(Cursors.Wait);

                DataTable dtCampaigns = INCampaignMapper.ListData(false, null).Tables[0];
                dtCampaigns.DefaultView.Sort = "Name";
                cmbCampaign.Populate(dtCampaigns, "Name", "ID");

                //_dtLanguage = LookupMapper.ListData(Lookups.lkpLanguage.ToString()).Tables[0];
                //_dtGender = LookupMapper.ListData(Lookups.lkpGender.ToString()).Tables[0];
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

	    private void LoadImportHeaders()
	    {
            try
            {
                SetCursor(Cursors.Wait);

                idxFields = new Dictionary<string, ImportHeaderInfo>();
                DataTable dtImportHeaders = INImportHeaderMapper.ListData(false, null).Tables[0];

                if (dtImportHeaders.Rows.Count > 0)
                {
                    foreach (DataRow row in dtImportHeaders.Rows)
                    {
                        if (Convert.ToBoolean(row["IsActive"]))
                        {
                            idxFields.Add(row["Name"].ToString(), new ImportHeaderInfo { Header = row["Header"].ToString(), 
                                                                                         HeaderAlt1 = row["HeaderAlt1"].ToString(), 
                                                                                         HeaderAlt2 = row["HeaderAlt2"].ToString(), 
                                                                                         HeaderAlt3 = row["HeaderAlt3"].ToString(),
                                                                                         IgnoreIfNA = Convert.ToBoolean(row["IgnoreIfNA"]),
                                                                                       });
                        }
                    }
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
            }
        }

        private void DuplicateFinder(object sender, DoWorkEventArgs e)
        {
            try
            {

                #region initialize
                string strQuery;
                string strQuery2;
                DataTable dt;
                Thread.Sleep(1000);

                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    pbImportText.Foreground = Brushes.Black;
                    pbImportText.Text = "duplicate checking -";
                });
                _TimerCheckDuplicates.Start();
                #endregion

                #region duplicate checking

				//Create a new workbook containing any duplicates detected
				int dupRowIndex = 1;
				Workbook wbTemplate;
				Workbook wbReport = new Workbook(WorkbookFormat.Excel2007);
				string filePathAndName = string.Format("{0}Duplicate Leads Report ({1}).xlsx", GlobalSettings.UserFolder, DateTime.Now.ToString("yyyy-MM-dd HHmmss"));

				Uri uri = new Uri("/Templates/DuplicateLeadsTemplate.xlsx", UriKind.Relative);
				StreamResourceInfo info = Application.GetResourceStream(uri);
				if (info != null)
				{
					wbTemplate = Workbook.Load(info.Stream, true);
				}
				else
				{
					return;
				}

				Worksheet wsTemplate = wbTemplate.Worksheets["Duplicates Found"];
                Worksheet wsReport = wbReport.Worksheets.Add("Duplicates Found");

				Methods.CopyExcelRegion(wsTemplate, 0, 0, 0, 149, wsReport, 0, 0);

                strQuery = "SELECT DISTINCT RefNo FROM INImport ";
                strQuery += "JOIN INLead ON INImport.FKINLeadID = INLead.ID ";
                strQuery += "JOIN INCampaign ON INImport.FKINCampaignID = INCampaign.ID ";
                strQuery += "JOIN lkpINCampaignType ON INCampaign.FKINCampaignTypeID = lkpINCampaignType.ID ";
                strQuery += "JOIN lkpINCampaignGroup ON INCampaign.FKINCampaignGroupID = lkpINCampaignGroup.ID WHERE ";

                strQuery2 = (string)strQuery.Clone();

                #region build query - campaign type and campaign group
                {
                    IEnumerable<lkpINCampaignType> campaignTypes;
                    IEnumerable<lkpINCampaignGroup> campaignGroups;

                    if (_inCampaign.FKINCampaignTypeID != null && _inCampaign.FKINCampaignGroupID != null)
                    {
                        lkpINCampaignType campaignType = (lkpINCampaignType)_inCampaign.FKINCampaignTypeID;
                        lkpINCampaignGroup campaignGroup = (lkpINCampaignGroup)_inCampaign.FKINCampaignGroupID;

                        while (true)
                        {
                            //Macc
                            campaignTypes = new[]
                            {
                                lkpINCampaignType.Macc, lkpINCampaignType.MaccMillion, lkpINCampaignType.BlackMaccMillion, lkpINCampaignType.AccDis, lkpINCampaignType.MaccFuneral,
                                lkpINCampaignType.BlackMacc, lkpINCampaignType.FemaleDis, lkpINCampaignType.IGFemaleDisability, lkpINCampaignType.FemaleDisCancer
                            };
                            campaignGroups = new[] 
                            { 
                                lkpINCampaignGroup.Base, lkpINCampaignGroup.Starter, lkpINCampaignGroup.Defrosted,
                                lkpINCampaignGroup.Rejuvenation, lkpINCampaignGroup.Reactivation, lkpINCampaignGroup.Extension, lkpINCampaignGroup.ReDefrost,
                                lkpINCampaignGroup.Resurrection
                            };
                            if (campaignTypes.Contains(campaignType) && campaignGroups.Contains(campaignGroup))
                            {
                                strQuery += "lkpINCampaignType.ID IN (" + string.Join(",", campaignTypes.Cast<int>().ToArray()) + ") AND ";
                                strQuery += "lkpINCampaignGroup.ID IN (" + string.Join(",", campaignGroups.Cast<int>().ToArray()) + ") AND ";
                                strQuery2 += "lkpINCampaignType.ID IN (" + string.Join(",", campaignTypes.Cast<int>().ToArray()) + ") AND ";
                                strQuery2 += "lkpINCampaignGroup.ID IN (" + (long)lkpINCampaignGroup.Renewals + ") AND ";
                                break;
                            }


                            //Cancer 
                            campaignTypes = new[]
                            {
                                lkpINCampaignType.Cancer, lkpINCampaignType.CancerFuneral, lkpINCampaignType.CancerFuneral99,  lkpINCampaignType.IGCancer, lkpINCampaignType.TermCancer,
                                lkpINCampaignType.MaccCancer, lkpINCampaignType.MaccMillionCancer
                            };
                            campaignGroups = new[] 
                            { 
                                lkpINCampaignGroup.Base, lkpINCampaignGroup.Starter, lkpINCampaignGroup.Defrosted,
                                lkpINCampaignGroup.Rejuvenation, lkpINCampaignGroup.Reactivation, lkpINCampaignGroup.Extension, lkpINCampaignGroup.ReDefrost,
                                lkpINCampaignGroup.Resurrection
                            };
                            if (campaignTypes.Contains(campaignType) && campaignGroups.Contains(campaignGroup))
                            {
                                strQuery += "lkpINCampaignType.ID IN (" + string.Join(",", campaignTypes.Cast<int>().ToArray()) + ") AND ";
                                strQuery += "lkpINCampaignGroup.ID IN (" + string.Join(",", campaignGroups.Cast<int>().ToArray()) + ") AND ";
                                strQuery2 += "lkpINCampaignType.ID IN (" + string.Join(",", campaignTypes.Cast<int>().ToArray()) + ") AND ";
                                strQuery2 += "lkpINCampaignGroup.ID IN (" + (long)lkpINCampaignGroup.Renewals + ") AND ";
                                break;
                            }


                            //Macc upgrades
                            campaignTypes = new[] 
                            { 
                                lkpINCampaignType.Macc, lkpINCampaignType.MaccMillion, lkpINCampaignType.BlackMaccMillion, lkpINCampaignType.AccDis, lkpINCampaignType.MaccFuneral,
                                lkpINCampaignType.BlackMacc, lkpINCampaignType.FemaleDis, lkpINCampaignType.IGFemaleDisability
                            };
                            campaignGroups = new[]
                            {
                                lkpINCampaignGroup.Upgrade, lkpINCampaignGroup.Upgrade1, lkpINCampaignGroup.Upgrade2, lkpINCampaignGroup.Upgrade3, lkpINCampaignGroup.Upgrade4,
                                lkpINCampaignGroup.Upgrade5, lkpINCampaignGroup.Upgrade6, lkpINCampaignGroup.Upgrade7, lkpINCampaignGroup.Upgrade8, lkpINCampaignGroup.Upgrade9,
                                lkpINCampaignGroup.Upgrade10, lkpINCampaignGroup.Upgrade11, lkpINCampaignGroup.Upgrade12,
                                lkpINCampaignGroup.DoubleUpgrade1, lkpINCampaignGroup.DoubleUpgrade2, 
                                lkpINCampaignGroup.DoubleUpgrade3, lkpINCampaignGroup.DoubleUpgrade4, lkpINCampaignGroup.DoubleUpgrade5, lkpINCampaignGroup.DoubleUpgrade6, 
                                lkpINCampaignGroup.DoubleUpgrade7, lkpINCampaignGroup.DoubleUpgrade8, lkpINCampaignGroup.DoubleUpgrade9, lkpINCampaignGroup.DoubleUpgrade10,
                                lkpINCampaignGroup.DoubleUpgrade11, lkpINCampaignGroup.DoubleUpgrade12, lkpINCampaignGroup.DoubleUpgrade13, lkpINCampaignGroup.DoubleUpgrade14, lkpINCampaignGroup.DefrostR99, lkpINCampaignGroup.R99, lkpINCampaignGroup.Lite
                            };
                            if (campaignTypes.Contains(campaignType) && campaignGroups.Contains(campaignGroup))
                            {
                                strQuery += "lkpINCampaignType.ID IN (" + string.Join(",", campaignTypes.Cast<int>().ToArray()) + ") AND ";
                                strQuery += "lkpINCampaignGroup.ID IN (" + string.Join(",", campaignGroups.Cast<int>().ToArray()) + ") AND ";
                                strQuery2 = string.Empty;
                                break;
                            }

                            //Cancer upgrades
                            campaignTypes = new[]
                            {
                                lkpINCampaignType.Cancer, lkpINCampaignType.CancerFuneral, lkpINCampaignType.CancerFuneral99, lkpINCampaignType.IGCancer, lkpINCampaignType.TermCancer
                            };
                            campaignGroups = new[]
                            {
                                lkpINCampaignGroup.Upgrade, lkpINCampaignGroup.Upgrade1, lkpINCampaignGroup.Upgrade2, lkpINCampaignGroup.Upgrade3, lkpINCampaignGroup.Upgrade4,
                                lkpINCampaignGroup.Upgrade5, lkpINCampaignGroup.Upgrade6, lkpINCampaignGroup.Upgrade7, lkpINCampaignGroup.Upgrade8,  lkpINCampaignGroup.Upgrade9,
                                lkpINCampaignGroup.Upgrade10, lkpINCampaignGroup.Upgrade11, lkpINCampaignGroup.Upgrade12,
                                lkpINCampaignGroup.DoubleUpgrade1, lkpINCampaignGroup.DoubleUpgrade2, 
                                lkpINCampaignGroup.DoubleUpgrade3, lkpINCampaignGroup.DoubleUpgrade4, lkpINCampaignGroup.DoubleUpgrade5, lkpINCampaignGroup.DoubleUpgrade6, 
                                lkpINCampaignGroup.DoubleUpgrade7, lkpINCampaignGroup.DoubleUpgrade8, lkpINCampaignGroup.DoubleUpgrade9, lkpINCampaignGroup.DoubleUpgrade10,
                                lkpINCampaignGroup.DoubleUpgrade11, lkpINCampaignGroup.DoubleUpgrade12, lkpINCampaignGroup.DoubleUpgrade13, lkpINCampaignGroup.DoubleUpgrade14,
                                lkpINCampaignGroup.DefrostR99, lkpINCampaignGroup.Upgrade13, lkpINCampaignGroup.R99, lkpINCampaignGroup.Lite
                            };
                            if (campaignTypes.Contains(campaignType) && campaignGroups.Contains(campaignGroup))
                            {
                                strQuery += "lkpINCampaignType.ID IN (" + string.Join(",", campaignTypes.Cast<int>().ToArray()) + ") AND ";
                                strQuery += "lkpINCampaignGroup.ID IN (" + string.Join(",", campaignGroups.Cast<int>().ToArray()) + ") AND ";
                                strQuery2 = string.Empty;
                                break;
                            }

                            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "The campaign type or campaign group\n is not registered for import.", "Batch not Duplicate Checked", ShowMessageType.Error);
                            });

                            return;
                        }
                    }
                }

                #endregion

                if (_workSheet != null)
                {
                    foreach (WorksheetRow row in _workSheet.Rows)
                    {
                        if (row.Index > 0)
                        {
                            int[] index = new int[1];
                            index[0] = row.Index;
                            string strSQL;

							#region Duplicate Checks

                            int dupCheckMonth = -4; //default for now
                            int dupCheckMonthRenewals = -8;

                            long? campaignGroupID = _inCampaign.FKINCampaignGroupID;
                            long? campaignTypeID = _inCampaign.FKINCampaignTypeID;

                            if (campaignTypeID == 5 || campaignTypeID == 6) //Cancer and Macc Funeral
                            {
                                dupCheckMonth = -4;
                            }
                            if (campaignGroupID == 3 || campaignGroupID == 4) //Rejuv and Defrosted
                            {
                                dupCheckMonth = -4;
                            }
                            if ((campaignGroupID >= 7 && campaignGroupID <= 20) || campaignGroupID == 23) //Upgrades
                            {
                                dupCheckMonth = -4;
                            }

                            #region check for duplicate RefNo in this campaign and Last ImportDate < x months

                            {
								string strRefNo = null;
								if (row.Cells[idxFields["RefNo"].Index].Value != null)
								{
									strRefNo = row.Cells[idxFields["RefNo"].Index].Value.ToString().Trim();
								}

								if (!string.IsNullOrWhiteSpace(strRefNo))
								{
								    strSQL = strQuery + "RefNo = '" + strRefNo + "' and ImportDate >= DateAdd(MM, " + dupCheckMonth + ", GetDate())";
                                    dt = Methods.GetTableData(strSQL);

									if (dt.Rows.Count > 0)
									{
										Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
										{
											//int columnIndex = 0;
                                        //foreach (WorksheetCell cell in row.Cells)
                                        //{
                                        //	wsReport.Rows[dupRowIndex].Cells[columnIndex].Value = cell.Value;
                                        //	cell.Value = string.Empty;
                                        //	columnIndex++;
                                        //}
                                            //for (int columnIndex = 0; columnIndex < row.Cells.Count(); columnIndex++)
                                            //{
                                            //    wsReport.Rows[dupRowIndex].Cells[columnIndex].Value = row.Cells[columnIndex].Value;
                                            //    row.Cells[columnIndex].Value = string.Empty;
                                            //    //columnIndex++;
                                            //}

                                            CopyCellsFromDuplicateChecker(row, dupRowIndex, ref wsReport);

                                            _workbook.Save(medFile.Text);
											dupRowIndex++;
										});

                                        continue;
									}

								    if (!string.IsNullOrWhiteSpace(strQuery2))
								    {
								        strSQL = strQuery2 + "RefNo = '" + strRefNo + "' and ImportDate >= DateAdd(MM, " + dupCheckMonthRenewals + ", GetDate())";
								        dt = Methods.GetTableData(strSQL);

								        if (dt.Rows.Count > 0)
								        {
								            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart) delegate
								            {
								                //int columnIndex = 0;
								                //foreach (WorksheetCell cell in row.Cells)
								                //{
								                //    wsReport.Rows[dupRowIndex].Cells[columnIndex].Value = cell.Value;
								                //    cell.Value = string.Empty;
								                //    columnIndex++;
								                //}
                                                CopyCellsFromDuplicateChecker(row, dupRowIndex, ref wsReport);

                                                _workbook.Save(medFile.Text);
								                dupRowIndex++;
								            });

								            continue;
								        }
								    }
								}
                            }

                            #endregion

                            #region check for duplicate ID numbers in leads and last importdate < x months

                            {
								string strIDNumber = null;
								if (row.Cells[idxFields["LeadIDNumber"].Index].Value != null)
								{
									strIDNumber = row.Cells[idxFields["LeadIDNumber"].Index].Value.ToString().Trim();
								}

                                if (!string.IsNullOrWhiteSpace(strIDNumber) && !strIDNumber.Contains("0000000"))
                                {
                                    strSQL = strQuery + "INLead.IDNo = '" + strIDNumber + "' AND ImportDate >= DateAdd(MM, " + dupCheckMonth + ", GetDate())";
                                    dt = Methods.GetTableData(strSQL);

									if (dt.Rows.Count > 0)
									{
										Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
										{
                                            //                                 int columnIndex = 0;
                                            //foreach (WorksheetCell cell in row.Cells)
                                            //{
                                            //	wsReport.Rows[dupRowIndex].Cells[columnIndex].Value = cell.Value;
                                            //	cell.Value = string.Empty;
                                            //	columnIndex++;
                                            //}
                                            CopyCellsFromDuplicateChecker(row, dupRowIndex, ref wsReport);

                                            _workbook.Save(medFile.Text);
											dupRowIndex++;
										});

                                        continue;
									}

                                    if (!string.IsNullOrWhiteSpace(strQuery2))
                                    {
                                        strSQL = strQuery2 + "INLead.IDNo = '" + strIDNumber + "' AND ImportDate >= DateAdd(MM, " + dupCheckMonthRenewals + ", GetDate())";
                                        dt = Methods.GetTableData(strSQL);

                                        if (dt.Rows.Count > 0)
                                        {
                                            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                                            {
                                                //int columnIndex = 0;
                                                //foreach (WorksheetCell cell in row.Cells)
                                                //{
                                                //    wsReport.Rows[dupRowIndex].Cells[columnIndex].Value = cell.Value;
                                                //    cell.Value = string.Empty;
                                                //    columnIndex++;
                                                //}
                                                CopyCellsFromDuplicateChecker(row, dupRowIndex, ref wsReport);

                                                _workbook.Save(medFile.Text);
                                                dupRowIndex++;
                                            });

                                            continue;
                                        }
                                    }
                                }
                            }

                            #endregion

                            #region check for first name, last name, dob and last importdate < x months

                            {
								string strLeadFirstname = null;
								if (row.Cells[idxFields["LeadFirstname"].Index].Value != null)
								{
									strLeadFirstname = row.Cells[idxFields["LeadFirstname"].Index].Value.ToString().Trim();
								}

								string strLeadSurname = null;
								if (row.Cells[idxFields["LeadSurname"].Index].Value != null)
								{
									strLeadSurname = row.Cells[idxFields["LeadSurname"].Index].Value.ToString().Trim();
								}

								string strLeadDateOfBirth = null;
								if (row.Cells[idxFields["LeadDateOfBirth"].Index].Value != null)
								{
									strLeadDateOfBirth = row.Cells[idxFields["LeadDateOfBirth"].Index].Value.ToString().Trim();
									strLeadDateOfBirth = Methods.ExcelFieldToDate(strLeadDateOfBirth);
								}

                                if (!string.IsNullOrWhiteSpace(strLeadFirstname) && !string.IsNullOrWhiteSpace(strLeadSurname) && !string.IsNullOrWhiteSpace(strLeadDateOfBirth))
                                {
                                    strSQL = strQuery + "INLead.FirstName = '" + strLeadFirstname.Replace("'", "''") + "' AND INLead.Surname = '" + strLeadSurname.Replace("'","''") + 
                                        "' AND INLead.DateOfBirth = '" + strLeadDateOfBirth + "' AND ImportDate >= DateAdd(MM, " + dupCheckMonth + ", GetDate())";
                                    dt = Methods.GetTableData(strSQL);

									if (dt.Rows.Count > 0)
									{
										Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
										{
                                            //                                 int columnIndex = 0;
                                            //foreach (WorksheetCell cell in row.Cells)
                                            //{
                                            //	wsReport.Rows[dupRowIndex].Cells[columnIndex].Value = cell.Value;
                                            //	cell.Value = string.Empty;
                                            //	columnIndex++;
                                            //}
                                            CopyCellsFromDuplicateChecker(row, dupRowIndex, ref wsReport);

                                            _workbook.Save(medFile.Text);
											dupRowIndex++;
										});

                                        continue;
									}

                                    if (!string.IsNullOrWhiteSpace(strQuery2))
                                    {
                                        strSQL = strQuery2 + "INLead.FirstName = '" + strLeadFirstname.Replace("'", "''") + "' AND INLead.Surname = '" + strLeadSurname.Replace("'", "''") +
                                                 "' AND INLead.DateOfBirth = '" + strLeadDateOfBirth + "' AND ImportDate >= DateAdd(MM, " + dupCheckMonthRenewals + ", GetDate())";
                                        dt = Methods.GetTableData(strSQL);

                                        if (dt.Rows.Count > 0)
                                        {
                                            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart) delegate
                                            {
                                                //int columnIndex = 0;
                                                //foreach (WorksheetCell cell in row.Cells)
                                                //{
                                                //    wsReport.Rows[dupRowIndex].Cells[columnIndex].Value = cell.Value;
                                                //    cell.Value = string.Empty;
                                                //    columnIndex++;
                                                //}
                                                CopyCellsFromDuplicateChecker(row, dupRowIndex, ref wsReport);

                                                _workbook.Save(medFile.Text);
                                                dupRowIndex++;
                                            });

                                            continue;
                                        }
                                    }
                                }
							}

                            #endregion

                            #region check for first name, last name, dob and tel cell < x months

                            {
                                string strLeadFirstname = null;
                                if (row.Cells[idxFields["LeadFirstname"].Index].Value != null)
                                {
                                    strLeadFirstname = row.Cells[idxFields["LeadFirstname"].Index].Value.ToString().Trim();
                                }

                                string strLeadSurname = null;
                                if (row.Cells[idxFields["LeadSurname"].Index].Value != null)
                                {
                                    strLeadSurname = row.Cells[idxFields["LeadSurname"].Index].Value.ToString().Trim();
                                }

                                string strTelCell = null;
                                if (row.Cells[idxFields["LeadTelCell"].Index].Value != null)
                                {
                                    strTelCell = row.Cells[idxFields["LeadTelCell"].Index].Value.ToString().Trim();
                                    strTelCell = StringLib.FormatPhoneNumber(strTelCell);
                                }

                                if (!string.IsNullOrWhiteSpace(strLeadFirstname) && !string.IsNullOrWhiteSpace(strLeadSurname) && !string.IsNullOrWhiteSpace(strTelCell))
                                {
                                    strSQL = strQuery + "INLead.FirstName = '" + strLeadFirstname.Replace("'", "''") + "' AND INLead.Surname = '" + strLeadSurname.Replace("'", "''") + 
                                        "' AND INLead.TelCell = '" + strTelCell + "' AND ImportDate >= DateAdd(MM, " + dupCheckMonth + ", GetDate())";
                                    dt = Methods.GetTableData(strSQL);

                                    if (dt.Rows.Count > 0)
                                    {
                                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                                        {
                                            //int columnIndex = 0;
                                            //foreach (WorksheetCell cell in row.Cells)
                                            //{
                                            //    wsReport.Rows[dupRowIndex].Cells[columnIndex].Value = cell.Value;
                                            //    cell.Value = string.Empty;
                                            //    columnIndex++;
                                            //}
                                            CopyCellsFromDuplicateChecker(row, dupRowIndex, ref wsReport);

                                            _workbook.Save(medFile.Text);
                                            dupRowIndex++;
                                        });

                                        continue;
                                    }

                                    if (!string.IsNullOrWhiteSpace(strQuery2))
                                    {
                                        strSQL = strQuery2 + "INLead.FirstName = '" + strLeadFirstname.Replace("'", "''") + "' AND INLead.Surname = '" + strLeadSurname.Replace("'", "''") + 
                                            "' AND INLead.TelCell = '" + strTelCell + "' AND ImportDate >= DateAdd(MM, " + dupCheckMonthRenewals + ", GetDate())";
                                        dt = Methods.GetTableData(strSQL);

                                        if (dt.Rows.Count > 0)
                                        {
                                            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                                            {
                                                //int columnIndex = 0;
                                                //foreach (WorksheetCell cell in row.Cells)
                                                //{
                                                //    wsReport.Rows[dupRowIndex].Cells[columnIndex].Value = cell.Value;
                                                //    cell.Value = string.Empty;
                                                //    columnIndex++;
                                                //}
                                                CopyCellsFromDuplicateChecker(row, dupRowIndex, ref wsReport);

                                                _workbook.Save(medFile.Text);
                                                dupRowIndex++;
                                            });

                                            //continue;
                                        }
                                    }
                                }


                            }

                            #endregion

                            #endregion
                        }
                    }
                }

				//Save excel document
				wbReport.Save(filePathAndName);

				//Display excel document
				Process.Start(filePathAndName);

                #endregion

                #region duplicate checking done
                _TimerCheckDuplicates.Stop();

                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    pbImportText.Foreground = Brushes.Green;
                    pbImportText.Text = pbImportText.Text.Substring(0, pbImportText.Text.Length - 1) + ">";
                });

                Thread.Sleep(1000);

                //ready for import
                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    pbImportText.Foreground = Brushes.Black;
                    pbImportText.Text = "0";
                    btnImport.IsEnabled = true;

                    if (!_campaignCode.Contains("RES"))
                    {
                        btnScheduleImport.IsEnabled = true;
                    }
                });

                #endregion

            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void CopyCellsFromDuplicateChecker(WorksheetRow row, int dupRowIndex, ref Worksheet wsReport)
        {
            try
            {
                //string duplicateRefNo;
                //string originalRefNo;
                //string name;
                //string surname;
                //string IDNumber;
                //string dateOfBirth;
                //string yearOfBirth;
                //string campaignCode;
                //DateTime datePosted;
                //DateTime dateUpdated;
                //DataTable dtExcelMappings = Methods.ExecuteStoredProcedure("spINDuplicateReportMapping", null).Tables[0];
                for (int columnIndex = 0; columnIndex < row.Cells.Count(); columnIndex++)
                {
                    //dtExcelMappings.Select("[] = " + columnIndex);
                    wsReport.Rows[dupRowIndex].Cells[columnIndex].Value = row.Cells[columnIndex].Value;
                    row.Cells[columnIndex].Value = string.Empty;
                    //columnIndex++;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            //return dupRowIndex;
        }

        private void DuplicateFinderComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Arrow);
            }

            catch (Exception)
            {
                HandleException((Exception)e.Result);
            }
        }

        public void OpenWorkBook(object sender, DoWorkEventArgs e)
        {
            try
            {
                LoadImportHeaders();

                //First reset all controls
                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    medFile.Text = string.Empty;
                    tbCampaignType.Text = string.Empty;
                    tbCampaignGroup.Text = string.Empty;
                    tbBatch.Text = string.Empty;
                    tbUDMBatch.Text = string.Empty;
                    tbTotalLeads.Text = string.Empty;
                    cmbCampaign.SelectedValue = null;
                    pbImportText.Text = string.Empty;
                    pbImportText.Foreground = Brushes.Black;
                    btnBrowse.IsEnabled = false;
                });

                _workbook = null;
                _workSheet = null;
                _inCampaign = null;

                string filePathAndName = ShowOpenFileDialog(ImportFileExtention, ImportFilterExpression);
                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)(() => medFile.Text = filePathAndName));

                if (filePathAndName != string.Empty)
                {
                    //System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(filePathAndName);
                    //System.IO.FileInfo filFile = dirInfo.GetFiles
                    string fileName = System.IO.Path.GetFileName(filePathAndName);

                    #region Check if filename is in correct format

                    if (fileName != null && !Regex.IsMatch(fileName, @"^Batch [a-zA-Z0-9_]+ \(.+\) [a-zA-Z0-9_]+[ ]*.xlsx?"))
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            ShowMessageBox(new INMessageBoxWindow1(), "The import file name is in the wrong format.\nThe file name should be in the format\n\"Batch xxx (campaign code) xxx.\"", "Import File Name", ShowMessageType.Error);
                            btnImport.IsEnabled = false;
                            btnScheduleImport.IsEnabled = false;
                            btnBrowse.IsEnabled = true;
                        });

                        _fileError = true;
                        return;
                    }

                    #endregion

                    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)(() => pbImportText.Text = "reading file ­­­­­" + '-'));
                    _TimerOpenWorkBook.Start();
                    _workbook = Workbook.Load(filePathAndName);

                    _workSheet = _workbook.Worksheets[0];
                    _rowCount = 0;

                    #region Check if all columns are available and determine column indexes

                    {
                        //string[] strKey = { idxFields.ElementAt(0).Key };
                        int index = 0;
                        string strKey = idxFields.ElementAt(index).Key;

                        Action headerErrorAction = () =>
                        {
                            if (idxFields[strKey].IgnoreIfNA)
                            {
                                idxFields.Remove(strKey);
                                _fileError = false;
                            }
                            else
                            {
                                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                               {
                                   bool result = Convert.ToBoolean(ShowMessageBox(new INMessageBoxWindow2(), "The header field for key \"" + strKey + "\" could not be found.\nSelect \"OK\" to ignore the import of this field or click \"Cancel\" to stop the import.", "Import File Alert", ShowMessageType.Information));

                                   if (result)
                                   {
                                       idxFields.Remove(strKey);
                                       _fileError = false;
                                   }
                                   else
                                   {
                                       btnImport.IsEnabled = false;
                                       btnScheduleImport.IsEnabled = false;
                                       btnBrowse.IsEnabled = true;
                                       _fileError = true;
                                   }
                               });
                            }
                        };

                        List<string> lstHeaders = new List<string>
                        {
                            idxFields.ElementAt(0).Value.Header, 
                            idxFields.ElementAt(0).Value.HeaderAlt1,
                            idxFields.ElementAt(0).Value.HeaderAlt2,
                            idxFields.ElementAt(0).Value.HeaderAlt3
                        };

                        WorksheetCell wsCell = lstHeaders.Select(str => Methods.WorksheetFindText(_workSheet, str, 0, 0, 1024, 1024, false, true)).FirstOrDefault(cell => cell != null);
                        while (wsCell == null && index < idxFields.Count)
                        {
                            index++;

                            strKey = idxFields.ElementAt(index).Key;

                            lstHeaders = new List<string>
                            {
                                idxFields.ElementAt(index).Value.Header,
                                idxFields.ElementAt(index).Value.HeaderAlt1,
                                idxFields.ElementAt(index).Value.HeaderAlt2,
                                idxFields.ElementAt(index).Value.HeaderAlt3
                            };

                            wsCell = lstHeaders.Select(str => Methods.WorksheetFindText(_workSheet, str, 0, 0, 1024, 1024, false, true)).FirstOrDefault(cell => cell != null);
                        }
                        if (index >= idxFields.Count)
                        {
                            _fileError = true;
                            return;
                        }

                        //if (wsCell != null)
                        //{
                        _headerRow = wsCell.RowIndex;

                        for (index = 0; index < idxFields.Count; index++)
                        {
                            strKey = idxFields.ElementAt(index).Key;

                            lstHeaders = new List<string>
                            {
                                idxFields.ElementAt(index).Value.Header, 
                                idxFields.ElementAt(index).Value.HeaderAlt1,
                                idxFields.ElementAt(index).Value.HeaderAlt2,
                                idxFields.ElementAt(index).Value.HeaderAlt3
                            };

                            wsCell = lstHeaders.Select(str => Methods.WorksheetFindText(_workSheet, str, _headerRow, 0, _headerRow, 1024, false, true)).FirstOrDefault(cell => cell != null);
                            if (wsCell != null)
                            {
                                idxFields[strKey].Index = wsCell.ColumnIndex;
                            }
                            else
                            {
                                index--;
                                headerErrorAction();
                                if (_fileError == true) return;
                            }
                        }
                    }

                    #endregion

                    if (fileName != null)
                    {
                        //get batch code and filename
                        fileName = fileName.Replace("Batch", string.Empty);
                        //fileName = fileName.Replace("_", " ");
                        fileName = fileName.Replace("  ", " ");
                        fileName = fileName.Trim();

                        _batchCode = fileName.Substring(0, fileName.IndexOf('(')).Trim();
                        if (_batchCode.Contains("_"))
                        {
                            _batchCodeModifier = _batchCode.Substring(_batchCode.IndexOf("_", StringComparison.Ordinal)).ToUpper();
                        }
                        _batchCode2 = fileName.Substring(fileName.IndexOf(") ", StringComparison.Ordinal) + 2, fileName.IndexOf('.') - (fileName.IndexOf(") ", StringComparison.Ordinal) + 2)).Trim();
                        //_campaignName = fileName.Substring(fileName.IndexOf('(') + 1, fileName.IndexOf(')') - fileName.IndexOf('(') - 1); 
                        _campaignCode = fileName.Substring(fileName.IndexOf('(') + 1, fileName.IndexOf(')') - fileName.IndexOf('(') - 1).Replace(" ", string.Empty).ToUpper();
                        _inCampaign = INCampaignMapper.SearchOne(null, null, null, _campaignCode, null, null, null, null, null, null, null, null);

                        //Get the row count
                        _rowCount = _workSheet.Rows.Select(row => row).Count(row => (row.Index > _headerRow && NoNull(row.Cells[idxFields["RefNo"].Index].Value, string.Empty).ToString() != string.Empty));

                        //Display import information
                        Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                        {
                            if (_inCampaign != null) cmbCampaign.SelectedValue = _inCampaign.ID;
                            tbBatch.Text = _batchCode;
                            tbUDMBatch.Text = _batchCode2;
                            tbTotalLeads.Text = _rowCount.ToString();
                            //try and set date received
                            PopulateDateReceived(_batchCode);
                        });

                        //Select or create new campaign
                        if (_inCampaign != null)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                            {
                                cmbCampaign.SelectedValue = _inCampaign.ID;
                                if (_inCampaign.FKINCampaignTypeID != null) tbCampaignType.Text = ((lkpINCampaignType)_inCampaign.FKINCampaignTypeID).ToString();
                                if (_inCampaign.FKINCampaignGroupID != null) tbCampaignGroup.Text = ((lkpINCampaignGroup)_inCampaign.FKINCampaignGroupID).ToString();
                            });

                            //check if a batch for this campaign exists
                            INBatch inBatch = INBatchMapper.SearchOne(_inCampaign.ID, _batchCode, null, null, null, null, null,null,null,null);
                            if (inBatch != null)
                            {
                                // A batch with this code has already been imported for this campaign.
                                Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                                {
                                    ShowMessageBox(new INMessageBoxWindow1(), "A batch with code " + _batchCode + " has already been imported\n for campaign " + _campaignCode + ".", "Duplicate Batch", ShowMessageType.Exclamation);
                                    btnBrowse.IsEnabled = true;
                                });

                                _fileError = true;
                            }
                            else
                            {
                                _fileError = false;
                            }
                        }
                        else
                        {
                            //bool result = false;
                            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                            {
                                ShowMessageBox(new INMessageBoxWindow1(), "The campaign with code '" + _campaignCode + "' does not exist.\n", "Unknown Campaign", ShowMessageType.Exclamation);
                                btnBrowse.IsEnabled = true;

                                _fileError = true;

                                //INMessageBoxWindow2 messageBox = new INMessageBoxWindow2();
                                //messageBox.buttonOK.Content = "Select";
                                //messageBox.buttonCancel.Content = "New";

                                //var showMessageBox = ShowMessageBox(messageBox, "Would you like to select an existing campaign?\nclick \"Select\" to select an existing campaign or\nclick \"New\" to create a new campaign.", "Campaign not Found", ShowMessageType.Information);
                                //result = showMessageBox != null && (bool)showMessageBox;

                            });

                            //if (result)
                            //{
                            //    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart) delegate
                            //    {
                            //        cmbCampaign.IsEnabled = true;
                            //        cmbCampaign.Focus();
                            //    });

                            //    while (_inCampaign == null)
                            //    {

                            //    }

                            //    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)(() => cmbCampaign.IsEnabled = false));
                            //    _fileError = false;
                            //}
                            //else
                            //{
                            //    Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                            //    {
                            //        ImportAddCampaignScreen importAddCampaignScreen = new ImportAddCampaignScreen(string.Empty, _campaignCode);
                            //        ShowDialog(importAddCampaignScreen, new INDialogWindow(importAddCampaignScreen));

                            //        if (_inCampaign != null)
                            //        {
                            //            _dtCampaigns = INCampaignMapper.ListData(false, null).Tables[0];
                            //            _dtCampaigns.DefaultView.Sort = "Name";
                            //            cmbCampaign.Populate(_dtCampaigns, "Name", "ID");
                            //            cmbCampaign.SelectedValue = _inCampaign.ID;

                            //            if (_inCampaign.FKINCampaignTypeID != null) tbCampaignType.Text = ((lkpINCampaignType)_inCampaign.FKINCampaignTypeID).ToString();
                            //            if (_inCampaign.FKINCampaignGroupID != null) tbCampaignGroup.Text = ((lkpINCampaignGroup)_inCampaign.FKINCampaignGroupID).ToString();

                            //            _fileError = false;
                            //        }
                            //        else
                            //        {
                            //            _fileError = true;
                            //        }
                            //    });
                            //}
                        }
                    }
                }
                else
                {
                    _fileError = null;
                }

            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void OpenWorkBookComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                SetCursor(Cursors.Arrow);
                _TimerOpenWorkBook.Stop();

                if (_fileError.HasValue)
                {
                    if (!(bool)_fileError)
                    {
                        pbImportText.Foreground = Brushes.Green;
                        pbImportText.Text = pbImportText.Text.Substring(0, pbImportText.Text.Length - 1) + ">";

#if TRAININGBUILD

                        //used for initial import of batches only
                        pbImportText.Foreground = Brushes.Black;
                        pbImportText.Text = "0";
                        btnImport.IsEnabled = true;

#else

                        //run duplicate checker
                        BackgroundWorker worker = new BackgroundWorker();

                        worker.DoWork += DuplicateFinder;
                        worker.RunWorkerCompleted += DuplicateFinderComplete;
                        worker.RunWorkerAsync(worker);

#endif

                        //Skipping Duplicate Finder
                        //pbImportText.Foreground = Brushes.Black;
                        //pbImportText.Text = "0";
                        //btnImport.IsEnabled = true;
                        //btnScheduleImport.IsEnabled = true;
                    }
                    else
                    {
                        pbImportText.Foreground = Brushes.Red;
                        pbImportText.Text = "Error";
                    }
                }
                else
                {
                    pbImportText.Foreground = Brushes.Black;
                    pbImportText.Text = string.Empty;
                    btnBrowse.IsEnabled = true;
                }
            }

            catch (Exception)
            {
                HandleException((Exception)e.Result);
            }
        }

        private void CancelBatchImport()
        {
            buttonClose.IsEnabled = true;
            btnBrowse.IsEnabled = true;

            ShowMessageBox(new INMessageBoxWindow1(), "An error has occurred.\nThe batch import will now be cancelled.", "Batch Import Error", ShowMessageType.Error);
            
            Database.CancelTransactions();
        }

        //private bool ImportTitles()
        //{
        //    try
        //    {
        //        _dtINTitles = LookupMapper.ListData(Lookups.lkpINTitle.ToString()).Tables[0];

        //        foreach (WorksheetRow row in _workSheet.Rows)
        //        {
        //            if (row.Index > 0) //this is necessary since the 1st row in the spreadsheet is the column headings
        //            {
        //                string referrorTitle = null;
        //                string leadTitle = null;

        //                if (row.Cells[idxReferrorTitle].Value != null) referrorTitle = row.Cells[idxReferrorTitle].Value.ToString().Trim();
        //                if (row.Cells[idxLeadTitle].Value != null) leadTitle = row.Cells[idxLeadTitle].Value.ToString().Trim();

        //                if (!string.IsNullOrEmpty(referrorTitle))
        //                {
        //                    //check if referror title has already been imported
        //                    DataRow[] drs = _dtINTitles.Select("Description = '" + referrorTitle + "'");

        //                    if (drs.Length == 0)
        //                    {
        //                        //this is a new Title that is not present in the database
        //                        Lookup newINTitle = new Lookup(Lookups.lkpINTitle.ToString());

        //                        newINTitle.Description = referrorTitle;
        //                        newINTitle.Save(_validationResult);

        //                        DataRow dr = _dtINTitles.NewRow();
        //                        dr["ID"] = newINTitle.ID;
        //                        dr["Description"] = newINTitle.Description;
        //                        _dtINTitles.Rows.Add(dr);
        //                    }
        //                }

        //                if (!string.IsNullOrEmpty(leadTitle))
        //                {
        //                    //check if lead title has already been imported
        //                    DataRow[] drs = _dtINTitles.Select("Description = '" + leadTitle + "'");

        //                    if (drs.Length == 0)
        //                    {
        //                        //this is a new Title that is not present in the database
        //                        Lookup newINTitle = new Lookup(Lookups.lkpINTitle.ToString());

        //                        newINTitle.Description = leadTitle;
        //                        newINTitle.Save(_validationResult);

        //                        DataRow dr = _dtINTitles.NewRow();
        //                        dr["ID"] = newINTitle.ID;
        //                        dr["Description"] = newINTitle.Description;
        //                        _dtINTitles.Rows.Add(dr);
        //                    }
        //                }

        //            }
        //        }

        //        return true;
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //        return false;
        //    }
        //}

        //private bool ImportRelationships()
        //{
        //    try
        //    {
        //        _dtINRelationships = LookupMapper.ListData(Lookups.lkpINRelationship.ToString()).Tables[0];

        //        foreach (WorksheetRow row in _workSheet.Rows)
        //        {
        //            if (row.Index > 0) //this is necessary since the 1st row in the spreadsheet is the column headings
        //            {
        //                string referrorRelationship = null;

        //                if (row.Cells[idxReferrorRelationship].Value != null) referrorRelationship = row.Cells[idxReferrorRelationship].Value.ToString().Trim();

        //                if (!string.IsNullOrEmpty(referrorRelationship))
        //                {
        //                    //check if referror relationship has already been imported
        //                    DataRow[] drs = _dtINRelationships.Select("Description = '" + referrorRelationship + "'");

        //                    if (drs.Length == 0)
        //                    {
        //                        //this is a new Title that is not present in the database
        //                        Lookup newINRelationship = new Lookup(Lookups.lkpINRelationship.ToString());

        //                        newINRelationship.Description = referrorRelationship;
        //                        newINRelationship.Save(_validationResult);

        //                        DataRow dr = _dtINRelationships.NewRow();
        //                        dr["ID"] = newINRelationship.ID;
        //                        dr["Description"] = newINRelationship.Description;
        //                        _dtINRelationships.Rows.Add(dr);
        //                    }
        //                }
        //            }
        //        }

        //        return true;
        //    }

        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //        return false;
        //    }
        //}

        private double? GetFloatValue(WorksheetCell cell)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim();
                if (!string.IsNullOrWhiteSpace(str))
                {
                    return Convert.ToDouble(str);
                }
            }

            return null;
        }

        private decimal? GetDecimalValue(WorksheetCell cell)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim();
                if (!string.IsNullOrWhiteSpace(str))
                {
                    return Convert.ToDecimal(str);
                }
            }

            return null;
        }

        private int? GetIntegerValue(WorksheetCell cell)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim();
                int value;

                if (int.TryParse(str, out value))
                {
                    return Convert.ToInt32(str);
                }
            }

            return null;
        }

        private long? GetLongValue(WorksheetCell cell)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim();
                long value;

                if (long.TryParse(str, out value))
                {
                    return Convert.ToInt64(str);
                }
            }

            return null;
        }

        private short? GetShortValue(WorksheetCell cell)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim();
                short value;

                if (short.TryParse(str, out value))
                {
                    return Convert.ToInt16(str);
                }
            }

            return null;
        }

        private string GetStringValue(WorksheetCell cell)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim();
                if (!string.IsNullOrWhiteSpace(str))
                {
                    return str;
                }
            }

            return null;
        }

        private string GetEMailAddress(WorksheetCell cell)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim().Replace(" ", "");
                if (!string.IsNullOrWhiteSpace(str))
                {
                    if (Methods.IsValidEmail(str))
                    {
                        return str;
                    }
                }
            }

            return null;
        }

        private DateTime? GetDateValue(WorksheetCell cell)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim();
                str = Methods.ExcelFieldToDate(str);
                if (!string.IsNullOrWhiteSpace(str))
                {
                    return Convert.ToDateTime(str);
                }
            }

            return null;
        }

        private TimeSpan? GetTimeValue(WorksheetCell cell)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim();
                if (!string.IsNullOrWhiteSpace(str))
                {
                    return TimeSpan.Parse(str);
                }
            }

            return null;
        }

        private long? GetLookupID(WorksheetCell cell, string lookup)
        {
            if (cell.Value != null)
            {
                string str = cell.Value.ToString().Trim();
                string strLower = str.ToLower();
                DataTable dt;

                switch (strLower)
                {
                    #region languages
                    case "afr":
                        str = "Afrikaans";
                        strLower = "afrikaans";
                        break;

                    case "eng":
                        str = "English";
                        strLower = "english";
                        break;

                    case "Ndebele":
                        str = "Ndebele";
                        strLower = "ndebele";
                        break;

                    case "Xhosa":
                        str = "Xhosa";
                        strLower = "xhosa";
                        break;

                    case "Zulu":
                        str = "Zulu";
                        strLower = "zulu";
                        break;

                    case "Sotho":
                        str = "Sotho";
                        strLower = "sotho";
                        break;

                    case "Northern Sotho":
                        str = "Northern Sotho";
                        strLower = "northern sotho";
                        break;

                    case "Tswana":
                        str = "Tswana";
                        strLower = "tswana";
                        break;

                    case "Swati":
                        str = "Swati";
                        strLower = "swati";
                        break;

                    case "Venda":
                        str = "Venda";
                        strLower = "venda";
                        break;

                    case "Tsonga":
                        str = "Tsonga";
                        strLower = "tsonga";
                        break;

                    case "English 2nd Language":
                        str = "English 2nd Language";
                        strLower = "english 2nd language";
                        break;

                    case "Other":
                        str = "Other";
                        strLower = "other";
                        break;

                    case "Did not ask":
                        str = "Did not ask";
                        strLower = "did not ask";
                        break;
                    #endregion languages


                    case "m":
                        str = "Male";
                        strLower = "male";
                        break;

                    case "f":
                        str = "Female";
                        strLower = "female";
                        break;

                    case "cheque":
                        str = "Current";
                        strLower = "current";
                        break;
                }

                if (!string.IsNullOrWhiteSpace(str))
                {
                    if (lookup == "BankBranch")
                    {
                        dt = Methods.GetTableData("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED SELECT ID FROM " + lookup + " WHERE [Code] = '" + str + "'");

                        if (dt.Rows.Count > 0)
                        {
                            return Convert.ToInt64(dt.Rows[0]["ID"].ToString());
                        }

                        return null;
                    }
                    if (lookup == "INGiftOption")
                    {
                        dt = Methods.GetTableData("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED SELECT ID FROM " + lookup + " WHERE [Gift] = '" + str + "'");

                        if (dt.Rows.Count > 0)
                        {
                            return Convert.ToInt64(dt.Rows[0]["ID"].ToString());
                        }

                        return null;
                    }

                    dt = Methods.GetTableData("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED SELECT ID FROM " + lookup + " WHERE [Description] = '" + str + "'");

                    if (dt.Rows.Count > 0)
                    {
                        return Convert.ToInt64(dt.Rows[0]["ID"].ToString());
                    }

                    

                    //if (lookup == "lkpINTitle" || lookup == "lkpINRelationship")
                    //{
                    //    Lookup newLookup = new Lookup(lookup);
                    //    newLookup.Description = str;
                    //    newLookup.Save(_validationResult);

                    //    return newLookup.ID;
                    //}

                    //search for keywords in lookup fields to reduce number of names for the same thing
                    string strQuery;
                    lkpKeywordType keywordType = new lkpKeywordType();

                    switch (lookup)
                    {
                        case "lkpBank":
                            keywordType = lkpKeywordType.Bank;
                            break;

                        case "lkpAccountType":
                            keywordType = lkpKeywordType.AccountType;
                            break;

                        case "lkpPaymentMethod":
                            keywordType = lkpKeywordType.PaymentMethod;
                            break;
                    }

                    if (keywordType != 0)
                    {
                        strQuery = "USE Blush SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED ";
                        strQuery += "SELECT * FROM Keyword WHERE FKKeywordTypeID = '" + (long)keywordType + "'";

                        dt = Methods.GetTableData(strQuery);

                        foreach (DataRow dr in dt.Rows)
                        {
                            string strTerm = (string) dr["Term"];
                            strTerm = strTerm.ToLower();

                            if (strLower.Contains(strTerm))
                            {
                                return Convert.ToInt64(dr["LookupID"].ToString());
                            }
                        }
                    }
                }
            }

            return null;
        }

        private bool ImportLeads()
        {
            try
            {
                pbImport.Minimum = 0;
                pbImport.Maximum = _rowCount;
                pbImport.Value = 0;
                double[] pbCounter = {0};

                _inBatch.NewLeads = 0;
                _inBatch.UpdatedLeads = 0;

                if (_workSheet != null)
                {
                    _TimerImport.Start();

                    foreach (WorksheetRow row in _workSheet.Rows)
                    {
                        if (row.Index > _headerRow)
                        {
                            string strRefNo = null;
                            if (row.Cells[idxFields["RefNo"].Index].Value != null)
                            {
                                strRefNo = GetStringValue(row.Cells[idxFields["RefNo"].Index]);
                            }

                            if (!string.IsNullOrWhiteSpace(strRefNo) && strRefNo.Length > 8)
                            {

                                #region inImport

                                INImport inImport = new INImport();
                                inImport.ImportDate = DateTime.Now;
                                inImport.FKINCampaignID = _inCampaign.ID;
                                inImport.FKINBatchID = _inBatch.ID;
                                inImport.RefNo = strRefNo;
                                inImport.Referror = GetStringValue(row.Cells[idxFields["Referror"].Index]);
                                if (idxFields.ContainsKey("ReferrorPolicyID")) inImport.ReferrorPolicyID = GetStringValue(row.Cells[idxFields["ReferrorPolicyID"].Index]);
                                inImport.ReferrorContact = GetStringValue(row.Cells[idxFields["ReferrorContact"].Index]);
                                inImport.FKINReferrorTitleID = GetLookupID(row.Cells[idxFields["ReferrorTitle"].Index], "lkpINTitle");
                                inImport.FKINReferrorRelationshipID = GetLookupID(row.Cells[idxFields["ReferrorRelationship"].Index], "lkpINRelationship");
                                inImport.Gift = GetStringValue(row.Cells[idxFields["LeadGift"].Index]);
                                inImport.PlatinumContactDate = GetDateValue(row.Cells[idxFields["PlatinumContactDate"].Index]);
                                inImport.PlatinumContactTime = GetTimeValue(row.Cells[idxFields["PlatinumContactTime"].Index]);
                                inImport.CancerOption = GetStringValue(row.Cells[idxFields["Option"].Index]);
                                inImport.PlatinumAge = GetShortValue(row.Cells[idxFields["PlatinumAge"].Index]);
                                try
                                {
                                    inImport.MoneyBackDate = GetDateValue(row.Cells[idxFields["MoneyBackDate"].Index]);

                                }
                                catch
                                {

                                }
                                try
                                {
                                    inImport.ConversionMBDate = GetDateValue(row.Cells[idxFields["ConversionMBDate"].Index]);
                                }
                                catch
                                {
                                    inImport.ConversionMBDate = null;
                                }

                                //Used for data added to import sheet at later stages
                                INImportOther inImportOther = new INImportOther();

                                #endregion

                                #region Testing Fields

                                if (idxFields.ContainsKey("Future1")) inImport.Testing1 = GetStringValue(row.Cells[idxFields["Future1"].Index]);

                                #endregion

                                #region inlead

                                INLead inLead = null;

                                string strLeadIDNumber = null;
                                if (row.Cells[idxFields["LeadIDNumber"].Index].Value != null)
                                {
                                    strLeadIDNumber = string.Join(string.Empty, row.Cells[idxFields["LeadIDNumber"].Index].Value.ToString().Trim().Take(13).ToArray());
                                }

                                string strLeadFirstname = null;
                                if (row.Cells[idxFields["LeadFirstname"].Index].Value != null)
                                {
                                    strLeadFirstname = row.Cells[idxFields["LeadFirstname"].Index].Value.ToString().Trim();
                                }

                                string strLeadSurname = null;
                                if (row.Cells[idxFields["LeadSurname"].Index].Value != null)
                                {
                                    strLeadSurname = row.Cells[idxFields["LeadSurname"].Index].Value.ToString().Trim();
                                }

                                string strLeadDateOfBirth = null;
                                if (row.Cells[idxFields["LeadDateOfBirth"].Index].Value != null)
                                {
                                    strLeadDateOfBirth = row.Cells[idxFields["LeadDateOfBirth"].Index].Value.ToString().Trim();
                                    strLeadDateOfBirth = Methods.ExcelFieldToDate(strLeadDateOfBirth);
                                }

                                string strTelCell = null;
                                if (row.Cells[idxFields["LeadTelCell"].Index].Value != null)
                                {
                                    strTelCell = row.Cells[idxFields["LeadTelCell"].Index].Value.ToString().Trim();
                                    strTelCell = StringLib.FormatPhoneNumber(strTelCell);
                                }

                                #region create inLead object

                                if (!string.IsNullOrWhiteSpace(strLeadIDNumber) && !strLeadIDNumber.Contains("0000000"))
                                {
                                    inLead = INLeadMapper.SearchOne(strLeadIDNumber, null);
                                }
                                else if (!string.IsNullOrWhiteSpace(strLeadFirstname) && !string.IsNullOrWhiteSpace(strLeadSurname))
                                {
                                    if (!string.IsNullOrWhiteSpace(strLeadDateOfBirth))
                                    {
                                        inLead = INLeadMapper.SearchOne(strLeadFirstname, strLeadSurname, Convert.ToDateTime(strLeadDateOfBirth), null);
                                    }
                                    else if (!string.IsNullOrWhiteSpace(strTelCell))
                                    {
                                        inLead = INLeadMapper.SearchOne(strLeadFirstname, strLeadSurname, strTelCell, null);
                                    }
                                }

                                if (inLead == null)
                                {
                                    inLead = new INLead();
                                    _inBatch.NewLeads++;
                                }
                                else
                                {
                                    _inBatch.UpdatedLeads++;
                                }

                                #endregion

                                if (!string.IsNullOrWhiteSpace(strLeadIDNumber))
                                {
                                    inLead.IDNo = strLeadIDNumber;
                                }

                                if (!string.IsNullOrWhiteSpace(strLeadFirstname))
                                {
                                    inLead.FirstName = strLeadFirstname;
                                }

                                if (!string.IsNullOrWhiteSpace(strLeadSurname))
                                {
                                    inLead.Surname = strLeadSurname;
                                }

                                if (!string.IsNullOrWhiteSpace(strLeadDateOfBirth))
                                {
                                    inLead.DateOfBirth = Convert.ToDateTime(strLeadDateOfBirth);
                                }

                                inLead.FKINTitleID = GetLookupID(row.Cells[idxFields["LeadTitle"].Index], "lkpINTitle");
                                inLead.Initials = GetStringValue(row.Cells[idxFields["LeadInitials"].Index]);
                                inLead.FKLanguageID = GetLookupID(row.Cells[idxFields["LeadLanguage"].Index], "lkpLanguage");
                                inLead.FKGenderID = GetLookupID(row.Cells[idxFields["LeadGender"].Index], "lkpGender");
                                inLead.TelWork = StringLib.FormatPhoneNumber(GetStringValue(row.Cells[idxFields["LeadTelWork"].Index]));
                                inLead.TelCell = StringLib.FormatPhoneNumber(GetStringValue(row.Cells[idxFields["LeadTelCell"].Index]));
                                inLead.TelHome = StringLib.FormatPhoneNumber(GetStringValue(row.Cells[idxFields["LeadTelHome"].Index]));
                                inLead.TelOther = StringLib.FormatPhoneNumber(GetStringValue(row.Cells[idxFields["TelOther"].Index]));
                                inLead.Address = GetStringValue(row.Cells[idxFields["LeadAddress"].Index]);
                                inLead.Address1 = GetStringValue(row.Cells[idxFields["LeadAddress1"].Index]);
                                inLead.Address2 = GetStringValue(row.Cells[idxFields["LeadAddress2"].Index]);
                                inLead.Address3 = GetStringValue(row.Cells[idxFields["LeadAddress3"].Index]);
                                inLead.Address4 = GetStringValue(row.Cells[idxFields["LeadAddress4"].Index]);
                                inLead.Address5 = GetStringValue(row.Cells[idxFields["LeadAddress5"].Index]);
                                inLead.PostalCode = GetStringValue(row.Cells[idxFields["LeadPostalCode"].Index]);
                                inLead.Email = GetEMailAddress(row.Cells[idxFields["LeadEMail"].Index]);
                                inLead.Occupation = GetStringValue(row.Cells[idxFields["LeadOccupation"].Index]);
                                inLead.Save(_validationResult);

                                #endregion

                                #region inBankDetails

                                INBankDetails inBankDetails = new INBankDetails();
                                {
                                    List<WorksheetCell> wsCells = new List<WorksheetCell>();
                                    wsCells.Add(row.Cells[idxFields["PaymentMethod"].Index]);
                                    wsCells.Add(row.Cells[idxFields["BankName"].Index]);
                                    wsCells.Add(row.Cells[idxFields["BankBranchCode"].Index]);
                                    wsCells.Add(row.Cells[idxFields["AccountType"].Index]);
                                    wsCells.Add(row.Cells[idxFields["AccountNumber"].Index]);
                                    wsCells.Add(row.Cells[idxFields["AccountHolder"].Index]);
                                    wsCells.Add(row.Cells[idxFields["DebitDay"].Index]);

                                    //Check if there are any cell entries for banking details
                                    IEnumerable<WorksheetCell> wsCellsWithEntries = wsCells.Select(cell => cell).Where(cell => cell.Value != null).Where(cell => !string.IsNullOrWhiteSpace(cell.Value.ToString()));

                                    if (wsCellsWithEntries.Any())
                                    {
                                        inBankDetails.FKPaymentMethodID = GetLookupID(row.Cells[idxFields["PaymentMethod"].Index], "lkpPaymentMethod");
                                        inBankDetails.FKBankID = GetLookupID(row.Cells[idxFields["BankName"].Index], "lkpBank");
                                        inBankDetails.FKBankBranchID = GetLookupID(row.Cells[idxFields["BankBranchCode"].Index], "BankBranch");

                                        inBankDetails.FKAccountTypeID = GetLookupID(row.Cells[idxFields["AccountType"].Index], "lkpAccountType");
                                        inImportOther.AccountType = GetStringValue(row.Cells[idxFields["AccountType"].Index]);

                                        inBankDetails.AccountNo = GetStringValue(row.Cells[idxFields["AccountNumber"].Index]);
                                        inBankDetails.AccountHolder = GetStringValue(row.Cells[idxFields["AccountHolder"].Index]);
                                        inBankDetails.DebitDay = GetShortValue(row.Cells[idxFields["DebitDay"].Index]);
                                        inBankDetails.Save(_validationResult);
                                    }
                                }

                                #endregion

                                #region inPolicy

                                INPolicy inPolicy = new INPolicy();
                                inPolicy.FKPolicyTypeID = _inCampaign.FKINCampaignTypeID;
                                if (idxFields.ContainsKey("PolicyID")) inPolicy.PolicyID = GetStringValue(row.Cells[idxFields["PolicyID"].Index]);
                                inPolicy.CommenceDate = GetDateValue(row.Cells[idxFields["CommenceDate"].Index]);
                                inPolicy.PolicyFee = GetDecimalValue(row.Cells[idxFields["PolicyFee"].Index]);

                                //Bump Up Option
                                {
                                    if (idxFields.ContainsKey("BumpUpOption"))
                                    {
                                        string str = GetStringValue(row.Cells[idxFields["BumpUpOption"].Index]);

                                        if (!string.IsNullOrWhiteSpace(str))
                                        {
                                            DataTable dt = Methods.GetTableData("SELECT ID FROM INBumpUpOption WHERE ImportCode LIKE '%" + str + "%'");

                                            if (dt.Rows.Count > 0)
                                            {
                                                inPolicy.FKINBumpUpOptionID = Convert.ToInt64(dt.Rows[0]["ID"].ToString());
                                            }
                                            else
                                            {
                                                throw new Exception("Bump Up Option '" + str + "' does not exist");
                                            }
                                        }
                                    }
                                }

                                if (inBankDetails.ID > 0)
                                {
                                    inPolicy.FKINBankDetailsID = inBankDetails.ID;
                                }
                                inPolicy.Save(_validationResult);

                                #endregion

                                #region INLifeAssured

                                {
                                    for (int[] laNo = { 1 }; laNo[0] <= 6; laNo[0]++)
                                    {
                                        WorksheetRow wsr = row;

                                        Dictionary<string, ImportHeaderInfo> dicLA = idxFields.Where(item => item.Key.Contains(string.Format("LA{0}", laNo[0]))).ToDictionary(x => x.Key, x => x.Value);

                                        //Check if there are any import entries for this Beneficiary
                                        List<WorksheetCell> wsCells = new List<WorksheetCell>(dicLA.Where(item => item.Value.Index > -1).Select(item => wsr.Cells[item.Value.Index]));
                                        wsCells = new List<WorksheetCell>(wsCells.Where(cell => cell.Value != null).Where(cell => !string.IsNullOrWhiteSpace(cell.Value.ToString())));

                                        if (wsCells.Any())
                                        {
                                            int index;

                                            INLifeAssured inLifeAssured = new INLifeAssured();

                                            index = dicLA.Where(item => item.Key.Contains("IDNumber")).Select(item => item.Value.Index).ElementAt(0);
                                            if (index > -1) inLifeAssured.IDNo = string.Join(string.Empty, ((GetStringValue(row.Cells[index]) + string.Empty).Take(13).ToArray()));

                                            index = dicLA.Where(item => item.Key.Contains("Title")).Select(item => item.Value.Index).ElementAt(0);
                                            if (index > -1) inLifeAssured.FKINTitleID = GetLookupID(row.Cells[index], "lkpINTitle");

                                            index = dicLA.Where(item => item.Key.Contains("FirstName")).Select(item => item.Value.Index).ElementAt(0);
                                            if (index > -1) inLifeAssured.FirstName = GetStringValue(row.Cells[index]);

                                            index = dicLA.Where(item => item.Key.Contains("Surname")).Select(item => item.Value.Index).ElementAt(0);
                                            if (index > -1) inLifeAssured.Surname = GetStringValue(row.Cells[index]);

                                            index = dicLA.Where(item => item.Key.Contains("Gender")).Select(item => item.Value.Index).ElementAt(0);
                                            if (index > -1) inLifeAssured.FKGenderID = GetLookupID(row.Cells[index], "lkpGender");

                                            index = dicLA.Where(item => item.Key.Contains("DOB")).Select(item => item.Value.Index).ElementAt(0);
                                            if (index > -1) inLifeAssured.DateOfBirth = GetDateValue(row.Cells[index]);

                                            if (laNo[0] == 2)
                                            {
                                                if (idxFields.ContainsKey("Future8")) inLifeAssured.TelContact = GetStringValue(row.Cells[idxFields["Future8"].Index]);
                                            }

                                            inLifeAssured.Save(_validationResult);

                                            INPolicyLifeAssured inPolicyLifeAssured = new INPolicyLifeAssured();
                                            inPolicyLifeAssured.FKINPolicyID = inPolicy.ID;
                                            inPolicyLifeAssured.FKINLifeAssuredID = inLifeAssured.ID;
                                            inPolicyLifeAssured.LifeAssuredRank = laNo[0];
                                            inPolicyLifeAssured.Save(_validationResult);
                                        }
                                     }
                                }

                                #endregion

                                #region INBeneficiary

                                {
                                    for (int[] beneficiaryNo = {1}; beneficiaryNo[0] <= 6; beneficiaryNo[0]++)
                                    {
                                        WorksheetRow wsr = row;
                                        string strBeneficiary = string.Format("Beneficiary{0}", beneficiaryNo[0]);

                                        Dictionary<string, ImportHeaderInfo> dicBeneficiary = idxFields.Where(item => item.Key.Contains(strBeneficiary)).ToDictionary(x => x.Key, x => x.Value);

                                        //Check if there are any import entries for this Beneficiary
                                        List<WorksheetCell> wsCells = new List<WorksheetCell>(dicBeneficiary.Where(item => item.Value.Index > -1).Select(item => wsr.Cells[item.Value.Index]));
                                        wsCells = new List<WorksheetCell>(wsCells.Where(cell => cell.Value != null).Where(cell => !string.IsNullOrWhiteSpace(cell.Value.ToString())));

                                        if (wsCells.Any())
                                        {
                                            int index;

                                            INBeneficiary inBeneficiary = new INBeneficiary();

                                            if (dicBeneficiary.Keys.Contains(strBeneficiary + "Title"))
                                            {
                                                index = dicBeneficiary.Where(item => item.Key.Contains("Title")).Select(item => item.Value.Index).ElementAt(0);
                                                if(index > -1) inBeneficiary.FKINTitleID = GetLookupID(row.Cells[index], "lkpINTitle");
                                            }

                                            if (dicBeneficiary.Keys.Contains(strBeneficiary + "Initials"))
                                            {
                                                index = dicBeneficiary.Where(item => item.Key.Contains("Initials")).Select(item => item.Value.Index).ElementAt(0);
                                                if (index > -1) inBeneficiary.Initials = GetStringValue(row.Cells[index]);
                                            }

                                            index = dicBeneficiary.Where(item => item.Key.Contains("FirstName")).Select(item => item.Value.Index).ElementAt(0);
                                            if (index > -1) inBeneficiary.FirstName = GetStringValue(row.Cells[index]);

                                            index = dicBeneficiary.Where(item => item.Key.Contains("Surname")).Select(item => item.Value.Index).ElementAt(0);
                                            if (index > -1) inBeneficiary.Surname = GetStringValue(row.Cells[index]);

                                            index = dicBeneficiary.Where(item => item.Key.Contains("IDNumber")).Select(item => item.Value.Index).ElementAt(0);
                                            if (index > -1) inBeneficiary.IDNo = string.Join(string.Empty, ((GetStringValue(row.Cells[index]) + string.Empty).Take(13).ToArray()));

                                            index = dicBeneficiary.Where(item => item.Key.Contains("DOB")).Select(item => item.Value.Index).ElementAt(0);
                                            if (index > -1) inBeneficiary.DateOfBirth = GetDateValue(row.Cells[index]);

                                            index = dicBeneficiary.Where(item => item.Key.Contains("Relationship")).Select(item => item.Value.Index).ElementAt(0);
                                            if (index > -1) inBeneficiary.FKINRelationshipID = GetLookupID(row.Cells[index], "lkpINRelationship");

                                            inBeneficiary.Save(_validationResult);

                                            INPolicyBeneficiary inPolicyBeneficiary = new INPolicyBeneficiary();
                                            inPolicyBeneficiary.FKINPolicyID = inPolicy.ID;
                                            inPolicyBeneficiary.FKINBeneficiaryID = inBeneficiary.ID;
                                            inPolicyBeneficiary.BeneficiaryRank = beneficiaryNo[0];

                                            index = dicBeneficiary.Where(item => item.Key.Contains("Percentage")).Select(item => item.Value.Index).ElementAt(0);
                                            if (index > -1)
                                            {
                                                row.Cells[index].Value = row.Cells[index].Value.ToString().TrimEnd('%');
                                                inPolicyBeneficiary.BeneficiaryPercentage = GetFloatValue(row.Cells[index]);
                                            }

                                            if (beneficiaryNo[0] == 1)
                                            {
                                                if (idxFields.ContainsKey("Future9")) inBeneficiary.TelContact = GetStringValue(row.Cells[idxFields["Future9"].Index]);
                                            }

                                            inPolicyBeneficiary.Save(_validationResult);
                                        }
                                    }
                                }

                                #endregion

                                #region INImportedPolicyData

                                INImportedPolicyData inImportedPolicyData = new INImportedPolicyData();
                                {
                                    List<WorksheetCell> wsCells = new List<WorksheetCell>();
                                    wsCells.Add(row.Cells[idxFields["CommenceDate"].Index]);
                                    wsCells.Add(row.Cells[idxFields["AppSignDate"].Index]);
                                    wsCells.Add(row.Cells[idxFields["ContractPremium"].Index]);
                                    wsCells.Add(row.Cells[idxFields["ContractTerm"].Index]);
                                    wsCells.Add(row.Cells[idxFields["LA1CancerCover"].Index]);
                                    wsCells.Add(row.Cells[idxFields["LA1CancerPremium"].Index]);
                                    wsCells.Add(row.Cells[idxFields["LA1AccidentalDeathCover"].Index]);
                                    wsCells.Add(row.Cells[idxFields["LA1AccidentalDeathPremium"].Index]);
                                    wsCells.Add(row.Cells[idxFields["LA1DisabilityCover"].Index]);
                                    wsCells.Add(row.Cells[idxFields["LA1DisabilityPremium"].Index]);
                                    wsCells.Add(row.Cells[idxFields["LA1FuneralCover"].Index]);
                                    wsCells.Add(row.Cells[idxFields["LA1FuneralPremium"].Index]);
                                    wsCells.Add(row.Cells[idxFields["LA2CancerCover"].Index]);
                                    wsCells.Add(row.Cells[idxFields["LA2CancerPremium"].Index]);
                                    wsCells.Add(row.Cells[idxFields["LA2AccidentalDeathCover"].Index]);
                                    wsCells.Add(row.Cells[idxFields["LA2AccidentalDeathPremium"].Index]);
                                    wsCells.Add(row.Cells[idxFields["LA2DisabilityCover"].Index]);
                                    wsCells.Add(row.Cells[idxFields["LA2DisabilityPremium"].Index]);
                                    wsCells.Add(row.Cells[idxFields["LA2FuneralCover"].Index]);
                                    wsCells.Add(row.Cells[idxFields["LA2FuneralPremium"].Index]);
                                    wsCells.Add(row.Cells[idxFields["KidsCancerCover"].Index]);
                                    wsCells.Add(row.Cells[idxFields["KidsCancerPremium"].Index]);
                                    wsCells.Add(row.Cells[idxFields["KidsDisabilityCover"].Index]);
                                    wsCells.Add(row.Cells[idxFields["KidsDisabilityPremium"].Index]);
                                    wsCells.Add(row.Cells[idxFields["MoneyBackPremium"].Index]);
                                    wsCells.Add(row.Cells[idxFields["MoneyBackTerm"].Index]);
                                    wsCells.Add(row.Cells[idxFields["LapseDate"].Index]);
                                    wsCells.Add(row.Cells[idxFields["PolicyFee"].Index]);

                                    //Check if there are any cell entries for these details
                                    IEnumerable<WorksheetCell> wsCellsWithEntries = wsCells.Select(cell => cell).Where(cell => cell.Value != null).Where(cell => !string.IsNullOrWhiteSpace(cell.Value.ToString()));

                                    if (wsCellsWithEntries.Any())
                                    {
                                        inImportedPolicyData.CommenceDate = GetDateValue(row.Cells[idxFields["CommenceDate"].Index]);
                                        inImportedPolicyData.AppSignDate = GetDateValue(row.Cells[idxFields["AppSignDate"].Index]);
                                        inImportedPolicyData.ContractPremium = GetDecimalValue(row.Cells[idxFields["ContractPremium"].Index]);
                                        inImportedPolicyData.ContractTerm = GetIntegerValue(row.Cells[idxFields["ContractTerm"].Index]);
                                        inImportedPolicyData.LapseDate = GetDateValue(row.Cells[idxFields["LapseDate"].Index]);

                                        inImportedPolicyData.LA1CancerCover = GetDecimalValue(row.Cells[idxFields["LA1CancerCover"].Index]);
                                        inImportedPolicyData.LA1CancerPremium = GetDecimalValue(row.Cells[idxFields["LA1CancerPremium"].Index]);
                                        inImportedPolicyData.LA1AccidentalDeathCover = GetDecimalValue(row.Cells[idxFields["LA1AccidentalDeathCover"].Index]);
                                        inImportedPolicyData.LA1AccidentalDeathPremium = GetDecimalValue(row.Cells[idxFields["LA1AccidentalDeathPremium"].Index]);
                                        inImportedPolicyData.LA1DisabilityCover = GetDecimalValue(row.Cells[idxFields["LA1DisabilityCover"].Index]);
                                        inImportedPolicyData.LA1DisabilityPremium = GetDecimalValue(row.Cells[idxFields["LA1DisabilityPremium"].Index]);
                                        inImportedPolicyData.LA1FuneralCover = GetDecimalValue(row.Cells[idxFields["LA1FuneralCover"].Index]);
                                        inImportedPolicyData.LA1FuneralPremium = GetDecimalValue(row.Cells[idxFields["LA1FuneralPremium"].Index]);

                                        inImportedPolicyData.LA2CancerCover = GetDecimalValue(row.Cells[idxFields["LA2CancerCover"].Index]);
                                        inImportedPolicyData.LA2CancerPremium = GetDecimalValue(row.Cells[idxFields["LA2CancerPremium"].Index]);
                                        inImportedPolicyData.LA2AccidentalDeathCover = GetDecimalValue(row.Cells[idxFields["LA2AccidentalDeathCover"].Index]);
                                        inImportedPolicyData.LA2AccidentalDeathPremium = GetDecimalValue(row.Cells[idxFields["LA2AccidentalDeathPremium"].Index]);
                                        inImportedPolicyData.LA2DisabilityCover = GetDecimalValue(row.Cells[idxFields["LA2DisabilityCover"].Index]);
                                        inImportedPolicyData.LA2DisabilityPremium = GetDecimalValue(row.Cells[idxFields["LA2DisabilityPremium"].Index]);
                                        inImportedPolicyData.LA2FuneralCover = GetDecimalValue(row.Cells[idxFields["LA2FuneralCover"].Index]);
                                        inImportedPolicyData.LA2FuneralPremium = GetDecimalValue(row.Cells[idxFields["LA2FuneralPremium"].Index]);

                                        inImportedPolicyData.KidsCancerCover = GetDecimalValue(row.Cells[idxFields["KidsCancerCover"].Index]);
                                        inImportedPolicyData.KidsCancerPremium = GetDecimalValue(row.Cells[idxFields["KidsCancerPremium"].Index]);
                                        inImportedPolicyData.KidsDisabilityCover = GetDecimalValue(row.Cells[idxFields["KidsDisabilityCover"].Index]);
                                        inImportedPolicyData.KidsDisabilityPremium = GetDecimalValue(row.Cells[idxFields["KidsDisabilityPremium"].Index]);
                                        inImportedPolicyData.MoneyBackPremium = GetDecimalValue(row.Cells[idxFields["MoneyBackPremium"].Index]);
                                        inImportedPolicyData.MoneyBackTerm = GetIntegerValue(row.Cells[idxFields["MoneyBackTerm"].Index]);

                                        inImportedPolicyData.PolicyFee = GetDecimalValue(row.Cells[idxFields["PolicyFee"].Index]);

                                        inImportedPolicyData.Save(_validationResult);
                                    }
                                }

                                #endregion

                                #region INImportOther

                                inImportOther.StartDate = GetDateValue(row.Cells[idxFields["LeadStartDate"].Index]);
                                inImportOther.EndDate = GetDateValue(row.Cells[idxFields["LeadEndDate"].Index]);
                                inImportOther.ReferralFrom = GetStringValue(row.Cells[idxFields["LeadReferralFrom"].Index]);
                                inImportOther.AddressFrom = GetStringValue(row.Cells[idxFields["LeadAddressFrom"].Index]);
                                try
                                {
                                    inImportOther.TimesRemarketed = GetShortValue(row.Cells[idxFields["Future14"].Index]);
                                }
                                catch
                                {
                                    inImportOther.TimesRemarketed = 0;
                                }
                                //inImportOther.LastDateRemarketed = GetDateValue(row.Cells[idxFields["Future15"].Index]);

                                if (idxFields.ContainsKey("CollectedDate")) inImportOther.CollectedDate = GetDateValue(row.Cells[idxFields["CollectedDate"].Index]);
                                if (idxFields.ContainsKey("CommencementDate")) inImportOther.CommencementDate = GetDateValue(row.Cells[idxFields["CommencementDate"].Index]);
                                if (idxFields.ContainsKey("DurationInForce")) inImportOther.DurationInForce = GetIntegerValue(row.Cells[idxFields["DurationInForce"].Index]);
                                if (idxFields.ContainsKey("DurationSinceOOF")) inImportOther.DurationSinceOOF = GetIntegerValue(row.Cells[idxFields["DurationSinceOOF"].Index]);
                                if (idxFields.ContainsKey("NumColls")) inImportOther.NumColls = GetIntegerValue(row.Cells[idxFields["NumColls"].Index]);
                                if (idxFields.ContainsKey("OOFDate")) inImportOther.OOFDate = GetDateValue(row.Cells[idxFields["OOFDate"].Index]);
                                if (idxFields.ContainsKey("OOFType")) inImportOther.OOFType = GetStringValue(row.Cells[idxFields["OOFType"].Index]);
                                if (idxFields.ContainsKey("UpgradeCount")) inImportOther.UpgradeCount = GetIntegerValue(row.Cells[idxFields["UpgradeCount"].Index]);
                                if (idxFields.ContainsKey("Premium")) inImportOther.Premium = GetDecimalValue(row.Cells[idxFields["Premium"].Index]);
                                if (idxFields.ContainsKey("Bank")) inImportOther.Bank = GetStringValue(row.Cells[idxFields["Bank"].Index]);
                                if (idxFields.ContainsKey("Last4Digits")) inImportOther.Last4Digits = GetStringValue(row.Cells[idxFields["Last4Digits"].Index]);

                                #endregion



                                //save database objects
                                inImport.FKINLeadID = inLead.ID;
                                inImport.FKINPolicyID = inPolicy.ID;
                                if (inImportedPolicyData.ID > 0)
                                {
                                    inImport.FKINImportedPolicyDataID = inImportedPolicyData.ID;
                                }

                                inImport.Save(_validationResult);

                                //Save Other
                                inImportOther.FKINImportID = inImport.ID;
                                inImportOther.FKINBatchID = inImport.FKINBatchID;
                                inImportOther.RefNo = inImport.RefNo;
                                inImportOther.Save(_validationResult);

                                #region INNextOfKin

                                INNextOfKin inNextOfKin = new INNextOfKin();
                                inNextOfKin.FKINImportID = inImport.ID;
                                inNextOfKin.TelContact = GetStringValue(row.Cells[idxFields["Future10"].Index]);
                                inNextOfKin.FirstName = GetStringValue(row.Cells[idxFields["Future11"].Index]);
                                inNextOfKin.Surname = GetStringValue(row.Cells[idxFields["Future12"].Index]);
                                inNextOfKin.FKINRelationshipID = GetLongValue(row.Cells[idxFields["Future13"].Index]);

                                inNextOfKin.Save(_validationResult);

                                #endregion

                                #region INImportContactTracing

                                INImportContactTracing iNImportContactTracing = new INImportContactTracing();
                               
                                try
                                {
                                 
                                    iNImportContactTracing.FKINImportID = inImport.ID;
                                    iNImportContactTracing.ContactTraceOne = GetStringValue(row.Cells[idxFields["Contact1"].Index]);
                                    iNImportContactTracing.ContactTraceTwo = GetStringValue(row.Cells[idxFields["Contact2"].Index]);
                                    iNImportContactTracing.ContactTraceThree = GetStringValue(row.Cells[idxFields["Contact3"].Index]);
                                    iNImportContactTracing.ContactTraceFour = GetStringValue(row.Cells[idxFields["Contact4"].Index]);
                                    iNImportContactTracing.ContactTraceFive = GetStringValue(row.Cells[idxFields["Contact5"].Index]);
                                    iNImportContactTracing.ContactTraceSix = GetStringValue(row.Cells[idxFields["Contact6"].Index]);

                                }
                                catch
                                {
                                    iNImportContactTracing.FKINImportID = null;
                                    iNImportContactTracing.ContactTraceOne = null;
                                    iNImportContactTracing.ContactTraceTwo = null;
                                    iNImportContactTracing.ContactTraceThree = null;
                                    iNImportContactTracing.ContactTraceFour = null;
                                    iNImportContactTracing.ContactTraceFive = null;
                                    iNImportContactTracing.ContactTraceSix = null;
                                }


                                iNImportContactTracing.Save(_validationResult);

                                #endregion

                                #region Redeemed / Not Redeemed Batches

                                if (GlobalConstants.BatchCodes.RedeemGift.Contains(_batchCodeModifier))
                                {
                                    INGiftRedeem inGiftRedeem = new INGiftRedeem();
                                    inGiftRedeem.FKINImportID = inImport.ID;

                                    if (GlobalConstants.BatchCodes.GiftRedeemed.Contains(_batchCodeModifier))
                                    {
                                        inGiftRedeem.FKlkpGiftRedeemStatusID = (long?)lkpINGiftRedeemStatus.Redeemed;
                                    }
                                    else if (GlobalConstants.BatchCodes.GiftNotRedeemed.Contains(_batchCodeModifier))
                                    {
                                        inGiftRedeem.FKlkpGiftRedeemStatusID = (long?)lkpINGiftRedeemStatus.NotRedeemed;
                                    }

                                    inGiftRedeem.FKGiftOptionID = GetLookupID(row.Cells[idxFields["LeadGift"].Index], "INGiftOption");

                                    if (idxFields.ContainsKey("Future5")) inGiftRedeem.RedeemedDate = GetDateValue(row.Cells[idxFields["Future5"].Index]);
                                    if (idxFields.ContainsKey("Future6")) inGiftRedeem.PODSignature = GetStringValue(row.Cells[idxFields["Future6"].Index]);
                                    if (idxFields.ContainsKey("Future7")) inGiftRedeem.PODDate = GetDateValue(row.Cells[idxFields["Future7"].Index]);

                                    inGiftRedeem.Save(_validationResult);
                                }

                                #endregion

                                //update the progressbar
                                pbCounter[0] += 1;

                                pbImport.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                                {
                                    pbImport.Value = pbCounter[0];
                                    pbImportText.Text = pbCounter[0].ToString();
                                }));
                            }
                        }
                    }

                    _TimerImport.Stop();

                    return true;
                }

                //import not successfull
                _TimerImport.Stop();
                return false;
            }

            catch (Exception ex)
            {
                HandleException(ex);
                _TimerImport.Stop();
                return false;
            }
        }

        #endregion



        #region Event Handlers

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += OpenWorkBook;
                worker.RunWorkerCompleted += OpenWorkBookComplete;
                worker.RunWorkerAsync(worker);
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //first see if this exists in import schedule queue
                DataTable dtScheduledImports = Methods.GetTableData("select * from INImportSchedules where FKINCampaignID = '" + _inCampaign.ID + "' AND BatchName = '" + tbBatch.Text + "'");
                if (dtScheduledImports.Rows.Count > 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Cannot Import, This Batch is in the Import Schedule Queue ", "Cannot Import", ShowMessageType.Error);
                    return;
                }

                //prepare for import
                buttonClose.IsEnabled = false;
                btnImport.IsEnabled = false;
                btnScheduleImport.IsEnabled = false;
                btnBrowse.IsEnabled = false;
                cmbCampaign.IsEnabled = false;
                
              

                Database.BeginTransaction(null, IsolationLevel.Snapshot);
                //save batch related information
                _inBatch = new INBatch();
                _inBatch.FKINCampaignID = _inCampaign.ID;
                _inBatch.Code = tbBatch.Text;
                _inBatch.UDMCode = tbUDMBatch.Text;
                if (dteDateReceived.Value != null)
                {
                    _inBatch.DateReceived = (DateTime)dteDateReceived.Value;
                }
                _inBatch.Save(_validationResult);

                //import new titles and relationships
                //bool result = ImportTitles();
                //if (!result) { CancelBatchImport(); return; }

                //result = ImportRelationships();
                //if (!result) { CancelBatchImport(); return; }

                //import
                bool result = ImportLeads();
                if (!result) { CancelBatchImport(); return; }

                //import completed
                _inBatch.Save(_validationResult);
                CommitTransaction(null);
                buttonClose.IsEnabled = true;
                btnBrowse.IsEnabled = true;

                ShowMessageBox(new INMessageBoxWindow1(), "Batch import completed.", "Batch Import", ShowMessageType.Information);
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void EmbriantComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Methods.EmbriantComboBoxPreviewKeyDown(sender, e);
        }

        private void xamEditor_GotFocus(object sender, RoutedEventArgs e)
        {
            xamEditor_Select(sender);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
           OnDialogClose(false);
        }

        private void btnBrowse_Loaded(object sender, RoutedEventArgs e)
        {
            btnBrowse.Focus();
        }

        private void cmbCampaign_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbCampaign.SelectedIndex != -1)
                {
                    if (cmbCampaign.SelectedValue != null) _inCampaign = new INCampaign((long) cmbCampaign.SelectedValue);

                    if (_inCampaign.FKINCampaignTypeID != null && _inCampaign.FKINCampaignGroupID != null)
                    {
                        tbCampaignType.Text = ((lkpINCampaignType) _inCampaign.FKINCampaignTypeID).ToString();
                        tbCampaignGroup.Text = ((lkpINCampaignGroup) _inCampaign.FKINCampaignGroupID).ToString();

                        if ( //import only these campaign types and campaign group combinations
                            (_inCampaign.FKINCampaignTypeID == (long) lkpINCampaignType.Cancer && _inCampaign.FKINCampaignGroupID == (long) lkpINCampaignGroup.Base)
                            )
                        {
                            //pbImportText.Text = "0";
                            pbImport.Value = 0;
                            btnImport.Content = "Import Now";
                            //btnImport.IsEnabled = true;
                        }
                        else
                        {
                            pbImport.Value = 0;
                            btnImport.Content = "Import Now";
                            btnImport.IsEnabled = false;
                            btnScheduleImport.IsEnabled = false;
                        }
                    }

                    cmbCampaign.Focus();
                }
                else
                {
                    tbCampaignType.Text = string.Empty;
                    tbCampaignGroup.Text = string.Empty;
                    pbImportText.Text = string.Empty;
                    btnImport.IsEnabled = false;
                    btnScheduleImport.IsEnabled = false;
                    cmbCampaign.Focus();
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void medFile_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox tb = FindChild<TextBox>(medFile, "PART_InputTextBox");
                tb.IsTabStop = false;
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        #endregion



        private void btnScheduleImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Popup1.IsOpen = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void calImportDate_SelectedDatesChanged(object sender, Infragistics.Windows.Editors.Events.SelectedDatesChangedEventArgs e)
        {
            //check if selected date is not older than today
            if (calImportDate.SelectedDate < DateTime.Now.Date)
            {
                calImportDate.SelectedDate = null;
                ShowMessageBox(new INMessageBoxWindow1(), "The Date You Have Chosen has passed ", "Error", ShowMessageType.Error);
                return;
                    
            }
            EnableDisableSaveScheduleButton();
        }

        private void dteScheduleTime_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            if (calImportDate.SelectedDate == DateTime.Now.Date)
            {
                if (dteScheduleTime.Text.Length == 4)
                {
                    //check if time has not passed
                    if (DateTime.Parse(dteScheduleTime.Value.ToString()).TimeOfDay < DateTime.Now.TimeOfDay)
                    {
                        dteScheduleTime.Text = string.Empty;
                        ShowMessageBox(new INMessageBoxWindow1(), "The Time You Have Chosen has passed ", "Error", ShowMessageType.Error);
                        return;
                    }

                   
                }
            }
            if (dteScheduleTime.Text.Length == 4)
            {
                //restricted times are between 1 am and 6 am
                TimeSpan selectedTime = DateTime.Parse(dteScheduleTime.Value.ToString()).TimeOfDay;
                TimeSpan OneAm = new TimeSpan(1, 0, 0);
                TimeSpan sixAm = new TimeSpan(6, 0, 0);
                if (selectedTime >= OneAm && selectedTime <= sixAm)
                {
                    dteScheduleTime.Text = string.Empty;
                    ShowMessageBox(new INMessageBoxWindow1(), "The Time You Have Chosen has been restricted ", "Error", ShowMessageType.Error);
                }
                //second set of restricted times
                TimeSpan NinePm = new TimeSpan(21, 0, 0);
                TimeSpan elevenPM = new TimeSpan(23, 0, 0);
                if (selectedTime >= NinePm && selectedTime <= elevenPM)
                {
                    dteScheduleTime.Text = string.Empty;
                    ShowMessageBox(new INMessageBoxWindow1(), "The Time You Have Chosen has been restricted ", "Error", ShowMessageType.Error);
                }
            }
           

            EnableDisableSaveScheduleButton();
        }

        private void EnableDisableSaveScheduleButton()
        {
            if (calImportDate.SelectedDate != null && dteScheduleTime.Text.Length == 4)
            {
                btnPopupSave.IsEnabled = true;
            }
            else
            {

                btnPopupSave.IsEnabled = false;
            }
        }

        private void btnPopupSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //first check if this schedule exists
                DataTable dtScheduledImports = Methods.GetTableData("select * from INImportSchedules where FKINCampaignID = '" + _inCampaign.ID + "' AND BatchName = '" + tbBatch.Text + "'");
                if (dtScheduledImports.Rows.Count > 0)
                {
                    ShowMessageBox(new INMessageBoxWindow1(), "Cannot Schedule, This Batch is in the Import Schedule Queue ", "Cannot Schedule", ShowMessageType.Error);
                    return;
                }
                //save import schedule
                INImportSchedules importSchedule = new INImportSchedules();
                importSchedule.FKINCampaignID = long.Parse(cmbCampaign.SelectedValue.ToString());
                importSchedule.BatchName = tbBatch.Text;
                importSchedule.ImportFile = System.IO.File.ReadAllBytes(medFile.Text);
                importSchedule.ScheduleDate = calImportDate.SelectedDate.Value.Date;
                importSchedule.ScheduleTime = DateTime.Parse(dteScheduleTime.Value.ToString()).TimeOfDay;
                importSchedule.NumberOfLeads = int.Parse(tbTotalLeads.Text);
                importSchedule.UDMCode = tbUDMBatch.Text;
                importSchedule.ImportAtempts = 0;
                if (dteDateReceived.Value != null)
                {
                    importSchedule.DateReceived = (DateTime)dteDateReceived.Value;
                }
                importSchedule.Save(_validationResult);
                ShowMessageBox(new INMessageBoxWindow1(), "Schedule Has Been Created, this file will be imported on "
                    + calImportDate.SelectedDate.Value.Date.Date + " at " + importSchedule.ScheduleTime, "Save result", ShowMessageType.Information);

                //then close the import screen
                OnDialogClose(false);

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void PopulateDateReceived(string batchCode)
        {
            //format will be year,month,day
            try
            {
                string year = batchCode.Substring(0, 4);
                string month = batchCode.Substring(4, 2);
                string day = batchCode.Substring(6, 2);

                dteDateReceived.Value = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
            }

            catch (Exception)
            {
                // ignored
            }
        }
      

	}

}

