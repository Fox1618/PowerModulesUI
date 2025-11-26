using Telemetria;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.Interfaces;
using Telemetria.Hardware.PowerModules.Senders;
using Telemetria.Hardware.PowerModules.SendPackages;
using Telemetria.Persistence.Light.IniOptions.Common;

namespace PowerModulesManager.Services
{
    public class FactoryPm : Notifier
    {
        private IPmSender<IRPackPmModulesBase> _tonhePmSender;
        private IPmSender<IRPackPmModulesBase> _uugreenPmSender;

        public FactoryPm(TonhePmSender<IRPackPmModulesBase> tonhePmSender, UugreenPmSender<IRPackPmModulesBase> uugreenPmSender)
        {
            SPackPModulesBase.Prefix = 0x88;
            _tonhePmSender = tonhePmSender;
            _tonhePmSender.Message += sendMessage;
            _tonhePmSender.Warning += sendWarning;
            _uugreenPmSender = uugreenPmSender;
            _uugreenPmSender.Message += sendMessage;
            _uugreenPmSender.Warning += sendWarning;
            _tonhePmSender.DeviceSending.Message += sendMessage;
        }

        public IPmSender<IRPackPmModulesBase> GetPmSender(PowerSupplyType? powerSupplyType)
        {
            switch (powerSupplyType)
            {
                case PowerSupplyType.Tonhe:
                    return _tonhePmSender;
                case PowerSupplyType.Uugp:
                    return _uugreenPmSender;
                default:
                    return null;
            }
        }
    }
}
