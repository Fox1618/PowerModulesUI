using ProgrammingWeapons.Logging.Log4Net.Common;
using Telemetria;
using Telemetria.ApplicationBase.Common;
using Telemetria.ApplicationBase.WPF;
using Telemetria.Hardware.PowerModules;

namespace PowerModulesManager
{
    public static class DIFactory
    {
        public static IDIContainer GetContainer()
        {
            var provider = new NinjectContainerProvider(new PowerModulesNinjectModule(), new Log4NetCommonNinjectModule(), 
                new UIModule(), new ServicesModule());
            return new NinjectContainer(provider);
        }
    }
}