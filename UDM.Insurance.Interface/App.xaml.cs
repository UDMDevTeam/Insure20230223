using System.Windows;
using Prism.Ioc;
using UDM.Insurance.Interface.PrismInfrastructure;
using UDM.Insurance.Interface.PrismViews;
using UDM.Insurance.Interface.Windows;

namespace UDM.Insurance.Interface
{

    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell(Window shell)
        {
            base.InitializeShell(shell);

            SplashScreenWindow splashWindow = new SplashScreenWindow();
            var showDialog = splashWindow.ShowDialog();
            if (showDialog != null && !(bool)showDialog)
            {
                Shutdown();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


        }

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);

            containerRegistry.Register(typeof(IShellService), typeof(ShellService));

            Current.Resources.Add("IoC", Container);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }

        //private void Application_Startup(object sender, StartupEventArgs e)
        //{
        //    SplashScreenWindow splashWindow = new SplashScreenWindow();
        //    var showDialog = splashWindow.ShowDialog();
        //    if (showDialog != null && (bool)showDialog)
        //    {
        //        MainWindow mainWindow = new MainWindow();
        //        mainWindow.ShowDialog();
        //    }
        //    Shutdown();
        //}
    }
}
