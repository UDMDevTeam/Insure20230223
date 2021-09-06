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
using UDM.Insurance.Business;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ReportDiaryScreen.xaml
    /// </summary>
    public partial class DebiCheckStatusLoader
    {

        #region Constants


        #endregion Constants

        #region Private Members


        #endregion Private Members

        #region Constructors

        public DebiCheckStatusLoader()
        {
            InitializeComponent();


        }




        #endregion Constructors

        #region Private Methods
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {

            if(ReferenceTB.Text != "")
            {
                try
                {
                    string strQuery = " ";

                    strQuery = "SELECT DISTINCT ID FROM INImport WHERE ";
                    strQuery += "RefNo = '" + ReferenceTB.Text + "'";

                    DataTable dtINImport = Methods.GetTableData(strQuery);
                    long? INImportID = dtINImport.Rows[0]["ID"] as long?;
                    long inimportIDLong = long.Parse(INImportID.ToString());

                    INImport import = new INImport(inimportIDLong);
                    import.FKINLeadStatusID = 19;
                    import.Save(_validationResult);

                    RejectedDebiCheckTracking RDT = new RejectedDebiCheckTracking();
                    RDT.DateTimeSaved = DateTime.Now;
                    RDT.FKImportID = INImportID;
                    RDT.Save(_validationResult);
                    Reference1Indicator.Background = System.Windows.Media.Brushes.Green;
                }
                catch
                {
                    Reference1Indicator.Background = System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
            }

            if (ReferenceTB2.Text != "")
            {
                try
                {
                    string strQuery = " ";

                    strQuery = "SELECT DISTINCT ID FROM INImport WHERE ";
                    strQuery += "RefNo = '" + ReferenceTB2.Text + "'";

                    DataTable dtINImport = Methods.GetTableData(strQuery);

                    long? INImportID = dtINImport.Rows[0]["ID"] as long?;
                    long inimportIDLong = long.Parse(INImportID.ToString());

                    INImport import = new INImport(inimportIDLong);
                    import.FKINLeadStatusID = 19;
                    import.Save(_validationResult);

                    RejectedDebiCheckTracking RDT = new RejectedDebiCheckTracking();
                    RDT.DateTimeSaved = DateTime.Now;
                    RDT.FKImportID = INImportID;
                    RDT.Save(_validationResult);

                    Reference1Indicator2.Background = System.Windows.Media.Brushes.Green;
                }
                catch
                {
                    Reference1Indicator2.Background = System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
            }

            if (ReferenceTB3.Text != "")
            {
                try
                {
                    string strQuery = " ";

                    strQuery = "SELECT DISTINCT ID FROM INImport WHERE ";
                    strQuery += "RefNo = '" + ReferenceTB3.Text + "'";

                    DataTable dtINImport = Methods.GetTableData(strQuery);

                    long? INImportID = dtINImport.Rows[0]["ID"] as long?;
                    long inimportIDLong = long.Parse(INImportID.ToString());

                    INImport import = new INImport(inimportIDLong);
                    import.FKINLeadStatusID = 19;
                    import.Save(_validationResult);

                    RejectedDebiCheckTracking RDT = new RejectedDebiCheckTracking();
                    RDT.DateTimeSaved = DateTime.Now;
                    RDT.FKImportID = INImportID;
                    RDT.Save(_validationResult);

                    Reference1Indicator3.Background = System.Windows.Media.Brushes.Green;
                }
                catch
                {
                    Reference1Indicator3.Background = System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
            }

            if (ReferenceTB4.Text != "")
            {
                try
                {
                    string strQuery = " ";

                    strQuery = "SELECT DISTINCT ID FROM INImport WHERE ";
                    strQuery += "RefNo = '" + ReferenceTB4.Text + "'";

                    DataTable dtINImport = Methods.GetTableData(strQuery);
            
                    long? INImportID = dtINImport.Rows[0]["ID"] as long?;
                    long inimportIDLong = long.Parse(INImportID.ToString());

                    INImport import = new INImport(inimportIDLong);
                    import.FKINLeadStatusID = 19;
                    import.Save(_validationResult);

                    RejectedDebiCheckTracking RDT = new RejectedDebiCheckTracking();
                    RDT.DateTimeSaved = DateTime.Now;
                    RDT.FKImportID = INImportID;
                    RDT.Save(_validationResult);

                    Reference1Indicator4.Background = System.Windows.Media.Brushes.Green;
                }
                catch
                {
                    Reference1Indicator4.Background = System.Windows.Media.Brushes.Red;
                }

            }
            else
            {
            }

            if (ReferenceTB5.Text != "")
            {
                try
                {
                    string strQuery = " ";

                    strQuery = "SELECT ID FROM INImport WHERE ";
                    strQuery += "RefNo = '" + ReferenceTB5.Text + "'";

                    DataTable dtINImport = Methods.GetTableData(strQuery);

                    if (dtINImport.Rows.Count > 1)
                    {
                        SelectLeadCampaignScreen selectLeadCampaignScreen = new SelectLeadCampaignScreen(ReferenceTB5.Text);

                        //ShowOrHideFields(true);

                        if (ShowDialog(selectLeadCampaignScreen, new INDialogWindow(selectLeadCampaignScreen)) == true)
                        {
                            long importID = selectLeadCampaignScreen.ImportID;


                            INImport import = new INImport(importID);
                            import.FKINLeadStatusID = 19;
                            import.Save(_validationResult);

                            RejectedDebiCheckTracking RDT = new RejectedDebiCheckTracking();
                            RDT.DateTimeSaved = DateTime.Now;
                            RDT.FKImportID = importID;
                            RDT.Save(_validationResult);

                            Reference1Indicator5.Background = System.Windows.Media.Brushes.Green;
                        }
                    }
                    else
                    {
                        long? INImportID = dtINImport.Rows[0]["ID"] as long?;
                        long inimportIDLong = long.Parse(INImportID.ToString());

                        INImport import = new INImport(inimportIDLong);
                        import.FKINLeadStatusID = 19;
                        import.Save(_validationResult);

                        RejectedDebiCheckTracking RDT = new RejectedDebiCheckTracking();
                        RDT.DateTimeSaved = DateTime.Now;
                        RDT.FKImportID = INImportID;
                        RDT.Save(_validationResult);

                        Reference1Indicator5.Background = System.Windows.Media.Brushes.Green;
                    }

                }
                catch
                {
                    Reference1Indicator5.Background = System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
            }





        }

        #endregion Private Methods

        #region Event Handlers


        private void ClearReferenceFields()
        {
            ReferenceTB.Clear();
            ReferenceTB2.Clear();
            ReferenceTB3.Clear();
            ReferenceTB4.Clear();
            ReferenceTB5.Clear();
        }


        #endregion

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearReferenceFields();
            Reference1Indicator.Background = System.Windows.Media.Brushes.Black;
            Reference1Indicator2.Background = System.Windows.Media.Brushes.Black;
            Reference1Indicator3.Background = System.Windows.Media.Brushes.Black;
            Reference1Indicator4.Background = System.Windows.Media.Brushes.Black;
            Reference1Indicator5.Background = System.Windows.Media.Brushes.Black;

        }
    }
}
