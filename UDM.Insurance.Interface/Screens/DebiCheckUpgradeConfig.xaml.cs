using Embriant.Framework;
using Embriant.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UDM.Insurance.Business;
using UDM.Insurance.Interface.Windows;
using UDM.WPF.Library;
using UDM.WPF.Windows;

namespace UDM.Insurance.Interface.Screens
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class DebiCheckUpgradeConfig
    {

        #region Constants

        public ObservableCollection<DCUpgradeUserdatagrid> Batches { get; set; }


        #endregion Constants

        #region Private Members
        private readonly DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        private int _timer1;
        private DateTime _startDate;
        private DateTime _endDate;
        #endregion

        public DebiCheckUpgradeConfig()
        {
            InitializeComponent();

            LoadDatagrid();
        }


        private void LoadDatagrid()
        {
            try 
            {
                StringBuilder strQueryFKUserFromDB = new StringBuilder();
                strQueryFKUserFromDB.Append("select U.FirstName, U.LastName, DCU.IsUpgrade, DCU.FkUserID ");
                strQueryFKUserFromDB.Append("from DCUpgradeAgents as DCU ");
                strQueryFKUserFromDB.Append("left join [Insure].[dbo].[User] as U on DCU.FkUserID = U.ID ");
                System.Data.DataTable dtUserData = Methods.GetTableData(strQueryFKUserFromDB.ToString());

                Batches = new ObservableCollection<DCUpgradeUserdatagrid>();

                foreach (DataRow item in dtUserData.Rows)
                {
                    bool isactive = false;

                    if (item["IsUpgrade"].ToString() == "1")
                    {
                        isactive = true;
                    }

                    Batches.Add(new DCUpgradeUserdatagrid { FirstName = item["FirstName"].ToString(), IsUpgrade = isactive, ID = int.Parse(item["FkUserID"].ToString()), LastName = item["LastName"].ToString() });

                }


                DataGridBulk.ItemsSource = Batches;

            }
            catch { }
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in Batches)
                {

                    StringBuilder strQueryFKUserID = new StringBuilder();
                    strQueryFKUserID.Append("select DCU.ID ");
                    strQueryFKUserID.Append("from DCUpgradeAgents as DCU ");
                    strQueryFKUserID.Append("where DCU.FkUserID = " + item.ID);
                    System.Data.DataTable dtUserDataID = Methods.GetTableData(strQueryFKUserID.ToString());

                    int IsUpgradeBool;
                    if(item.IsUpgrade.ToString() == "True")
                    {
                        IsUpgradeBool = 1;
                    }
                    else
                    {
                        IsUpgradeBool = 0;
                    }

                    long DCUpgradeID = long.Parse(dtUserDataID.Rows[0]["ID"].ToString());

                    DCUpgradeAgents dcc = new DCUpgradeAgents(DCUpgradeID);
                    dcc.IsUpgrade = IsUpgradeBool.ToString();
                    dcc.Save(_validationResult);
                }

            }
            catch { }

            INMessageBoxWindow1 messageWindow = new INMessageBoxWindow1();
            ShowMessageBox(messageWindow, @"Configuration has saved !", "Saved", ShowMessageType.Information);

        }





        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            OnDialogClose(_dialogResult);
        }

        private void DataGridBulk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = DataGridBulk.SelectedItem as DCUpgradeUserdatagrid;

            if (selectedItem != null)
            {
                //selectedItem.Selected = true;
                //AgentsDG.SelectedItem = selectedItem;
                selectedItem.IsUpgrade = !selectedItem.IsUpgrade;
            }
        }
    }

    public class DCUpgradeUserdatagrid : INotifyPropertyChanged
    {
        public int ID { get; set; }

        private string _firstname;
        public string FirstName
        {
            get { return _firstname; }
            set
            {
                if (_firstname != value)
                {
                    _firstname = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        private string _lastname;
        public string LastName
        {
            get { return _lastname; }
            set
            {
                if (_lastname != value)
                {
                    _lastname = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }
        private bool _isupgrade;
        public bool IsUpgrade
        {
            get { return _isupgrade; }
            set
            {
                if (_isupgrade != value)
                {
                    _isupgrade = value;
                    OnPropertyChanged(nameof(IsUpgrade));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
