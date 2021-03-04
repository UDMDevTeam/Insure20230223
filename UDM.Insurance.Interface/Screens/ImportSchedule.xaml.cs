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
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using System.Data;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for ImportSchedule.xaml
    /// </summary>
    public partial class ImportSchedule
    {
        public ImportSchedule()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadSchedules();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        private void LoadSchedules()
        {
            DataTable dtSchedules = Methods.GetTableData("select INCampaign.Name as Campaign,INImportSchedules.BatchName as Batch,INImportSchedules.ScheduleDate as [Schedule Date],INImportSchedules.ScheduleTime as [Schedule Time],INImportSchedules.NumberOfLeads as [Number Of Leads],dbo.[User].FirstName + ' ' + dbo.[User].LastName as [Requested Made By]"
                    + " from INImportSchedules join INCampaign on INImportSchedules.FKINCampaignID = INCampaign.ID" +
                " join dbo.[User] on  INImportSchedules.StampUserID = dbo.[User].ID " +
                " where HasRun is null order by INImportSchedules.StampDate");
            dgSchedule.DataSource = dtSchedules.DefaultView;
           
           
        }
    }
}
