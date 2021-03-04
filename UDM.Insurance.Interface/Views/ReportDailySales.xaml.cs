using Embriant.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UDM.Insurance.Interface.ViewModels;
using UDM.WPF.Library;

namespace UDM.Insurance.Interface.Views
{
    /// <summary>
    /// Interaction logic for ReportDailySales.xaml
    /// </summary>
    public partial class ReportDailySales
    {

        #region Private Members

        #endregion Private Members

        public ReportDailySales()
        {

            InitializeComponent();
        }

        //private void chkTempSales_Checked(object sender, RoutedEventArgs e)
        //{
        //    DataSet ds = Methods.ExecuteStoredProcedure("sp_GetTempStaff", null);
        //    DataTable dt = ds.Tables[0];
        //    DataColumn column = new DataColumn("IsChecked", typeof(bool));
        //    column.DefaultValue = false;
        //    dt.Columns.Add(column);
        //    DataView Agents = dt.DefaultView;
        //    xdgAgents.DataSource = null;
        //    xdgAgents.DataSource = Agents;

        //    if (Agents.Count > 0)
        //    {
        //        btnReport.IsEnabled = true;
        //        btnCancel.IsEnabled = true;
        //    }
        //}

        //private void LoadSalesConsultants()
        //{

        //}

        //private void chkTempSales_Unchecked(object sender, RoutedEventArgs e)
        //{

        //}

        //public DataView GetPermAgents()
        //{
        //    DataSet ds = Methods.ExecuteStoredProcedure("sp_GetPermStaff", null);
        //    DataTable dt = ds.Tables[0];
        //    DataColumn column = new DataColumn("IsChecked", typeof(bool));
        //    column.DefaultValue = false;
        //    dt.Columns.Add(column);
        //    DataView Agents = dt.DefaultView;
        //    return Agents;
        //}
        //public DataView GetTeAgents()
        //{
        //    DataSet ds = Methods.ExecuteStoredProcedure("sp_GetPermStaff", null);
        //    DataTable dt = ds.Tables[0];
        //    DataColumn column = new DataColumn("IsChecked", typeof(bool));
        //    column.DefaultValue = false;
        //    dt.Columns.Add(column);
        //    DataView Agents = dt.DefaultView;
        //    return Agents;
        //}

        //private void chkPermSales_Checked(object sender, RoutedEventArgs e)
        //{
        //    DataSet ds = Methods.ExecuteStoredProcedure("sp_GetPermStaff", null);
        //    DataTable dt = ds.Tables[0];
        //    DataColumn column = new DataColumn("IsChecked", typeof(bool));
        //    column.DefaultValue = false;
        //    dt.Columns.Add(column);
        //    DataView Agents = dt.DefaultView;
        //    xdgAgents.DataSource = null;
        //    xdgAgents.DataSource = Agents;

        //    if (Agents.Count > 0)
        //    {
        //        btnReport.IsEnabled = true;
        //        btnCancel.IsEnabled = true;
        //    }
        //}

        //private void chkPermSales_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    //LoadSalesAgentInfo();
        //}
    }
}
