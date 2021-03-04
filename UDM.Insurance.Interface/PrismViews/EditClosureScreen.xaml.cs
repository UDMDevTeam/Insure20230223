using Embriant.WPF.Controls;
using Prism.Regions;
using Prism.Events;
using UDM.Insurance.Interface.PrismInfrastructure;
using UDM.Insurance.Interface.Windows;
using System;

namespace UDM.Insurance.Interface.PrismViews
{

    public partial class EditClosureScreen : BaseControl
    {
        IEventAggregator _ea;
        private bool _closeDialog = true;

        public EditClosureScreen(IEventAggregator ea)
        {
            InitializeComponent();

            _ea = ea;
            _ea.GetEvent<CloseDialogEvent>().Subscribe(CloseThis);
            _ea.GetEvent<SendDialogMessageEvent>().Subscribe(DialogMessageReceived);
        }

        private void BaseControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            EditClosureScreenViewModel editClosureScreenViewModel = (EditClosureScreenViewModel)DataContext;

            editClosureScreenViewModel.LocalRegionManager = RegionManager.GetRegionManager(this);

            editClosureScreenViewModel.Initialize();
        }

        private void CloseThis()
        {
            //BaseControl.DialogResult = true;

            if (_closeDialog)
            {
                OnDialogClose(_dialogResult);
            }
            _closeDialog = false;
        }
        
        private void DialogMessageReceived(DialogMessage dm)
        {
            string str = Convert.ToString(Tag);

            if (str == "Active")
            {
                ShowMessageBox(new INMessageBoxWindow1(), dm.Message, dm.Title, dm.Type);
            }
        }

    }
}
