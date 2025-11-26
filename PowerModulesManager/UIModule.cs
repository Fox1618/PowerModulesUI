using System.Windows;
using Ninject;
using Ninject.Modules;
using Telemetria.ApplicationBase;

namespace PowerModulesManager
{
    public class UIModule : NinjectModule
    {
        public override void Load() {
            Bind<IInputElement>().To<MainLayout>().WhenInjectedInto<UIProvider>();
            Bind<IUIProvider>().ToMethod(x => x.Kernel.Get<UIProvider>());
            Bind<UIProvider>().ToSelf().InSingletonScope();
            Bind<IBottomPanel>().To<BottomPanel>();
        }
    }
}