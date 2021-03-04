using System.Linq;
using Prism.Regions;
using UDM.Insurance.Interface.PrismInfrastructure;
using Unity;

namespace UDM.Insurance.Interface.PrismViews
{
    public class ShellService : IShellService
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public ShellService(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void ShowShell(string uri)
        {
            var shell = _container.Resolve<Shell>();

            var scopedRegion = _regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(shell, scopedRegion);
            var count = scopedRegion.Regions.Count();

            //IRegion region = new Region();
            //region.Name = "ContentRegionTest";
            //scopedRegion.Regions.Add(region);

            ViewA _viewA = _container.Resolve<ViewA>();
            //_viewB = _container.Resolve<ViewB>();

            IRegion _region = scopedRegion.Regions["ContentRegion"];

            _region.Add(_viewA);
            //_region.Add(_viewB);

            _region.Activate(_viewA);

            //scopedRegion.RequestNavigate(KnownRegionNames.ContentRegion, uri);

            shell.Show();
        }
    }
}
