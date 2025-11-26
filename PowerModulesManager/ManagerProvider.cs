using Telemetria;
using Telemetria.Persistence.Entities;

namespace PowerModulesManager
{

    public interface IManagerProviderPmModules: IManagerProvider
    {
        IMainViewModelPm MainViewModel { get; set; }
    }

    public class ManagerProvider : MagicNotifier, IManagerProviderPmModules
    {
        public IMainViewModelPm MainViewModel { get; set; }
        public IBarcodeEntity CurrentEntity { get; private set; }

        public ManagerProvider(IMainViewModelPm mainViewModel) {
            MainViewModel = mainViewModel;
            MainViewModel.Message += sendMessage;
        }
        public void OnEntityFinded(IBarcodeEntity entity, BarcodeType type) {
        }
    }
}
