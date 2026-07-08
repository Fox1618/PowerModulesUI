using Ninject;
using Ninject.Modules;
using PowerModulesManager.Services;
using ProgrammingWeapons.EntityFramework;
using Telemetria;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.Interfaces;
using Telemetria.Hardware.PowerModules.ParsersRPack;
using Telemetria.Hardware.TcpDevices;
using Telemetria.Persistence;
using Telemetria.Persistence.Light.Services;

namespace PowerModulesManager
{
    public class ServicesModule : NinjectModule
    {
        public override void Load() {
    
            Bind<IManagerProvider>().ToMethod(x => x.Kernel.Get<ManagerProvider>());
            Bind<IManagerProviderPmModules>().To<ManagerProvider>().InSingletonScope();
            Bind<ManagerProvider>().To<ManagerProvider>().InSingletonScope();
            Bind<IMainViewModelPm>().To<MainViewModelPm>().InSingletonScope();
            Bind<IBarcodeFinder>().To<BarcodeFinder>().InSingletonScope();
            Bind<SearchType>().ToConstant(SearchType.ElementsAndModels);
            Bind<IBindRepository>().To<Repository>().InSingletonScope();
            Bind<FactoryPm>().ToSelf().InSingletonScope();
            Bind<TcpDevice>().ToSelf().InSingletonScope();
        }
    }

}
