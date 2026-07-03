using System;
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
        private IPmSender<IRPackPmModulesBase> _megmeetPmSender;

        public FactoryPm(TonhePmSender<IRPackPmModulesBase> tonhePmSender, UugreenPmSender<IRPackPmModulesBase> uugreenPmSender, MegmeetPmSender<IRPackPmModulesBase> megmeetPmSender)
        {
            SPackPModulesBase.Prefix = 0x88;
            _tonhePmSender = tonhePmSender;
            _tonhePmSender.Message += sendMessage;
            _tonhePmSender.Warning += sendWarning;
            _uugreenPmSender = uugreenPmSender;
            _uugreenPmSender.Message += sendMessage;
            _uugreenPmSender.Warning += sendWarning;
            _megmeetPmSender = megmeetPmSender;
            _megmeetPmSender.Message += sendMessage;
            _megmeetPmSender.Warning += sendWarning;
            _tonhePmSender.DeviceSending.Message += sendMessage;
            _uugreenPmSender.DeviceSending.Message += sendMessage;
            _megmeetPmSender.DeviceSending.Message += sendMessage;
        }

        public IPmSender<IRPackPmModulesBase> GetPmSender(PowerSupplyType? powerSupplyType)
        {
            if (!powerSupplyType.HasValue)
                return null;

            switch (powerSupplyType.Value)
            {
                case PowerSupplyType.Tonhe:
                    return _tonhePmSender;
                case PowerSupplyType.Uugp:
                    return _uugreenPmSender;
                default:
                    // todo: rename pscs to megmeet
                    return isPscs(powerSupplyType.Value) ? _megmeetPmSender : null;
            }
        }

        private bool isPscs(PowerSupplyType powerSupplyType)
        {
            return powerSupplyType.ToString().IndexOf("Pscs", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
