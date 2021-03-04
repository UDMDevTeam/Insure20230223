using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace UDM.Insurance.Interface.PrismViews
{
    public class ShellViewModel : BindableBase
    {
        IRegionManager _regionManager;
        private readonly PrismInfrastructure.IShellService _service;

        public DelegateCommand<string> OpenShellCommand { get; private set; }
        public DelegateCommand<string> NavigateCommand { get; private set; }

        private string _title = "Prism Shell Screen";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ShellViewModel(IRegionManager regionManager, PrismInfrastructure.IShellService service)
        {
            _regionManager = regionManager;
            _service = service;

            OpenShellCommand = new DelegateCommand<string>(OpenShell);
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        void OpenShell(string viewName)
        {
            _service.ShowShell(viewName);
        }

        void Navigate(string viewName)
        {
            _regionManager.RequestNavigate(PrismInfrastructure.KnownRegionNames.ContentRegion, viewName);
        }
    }
}
