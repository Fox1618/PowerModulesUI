
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ProgrammingWeapons.Logging;
using Telemetria;
using Telemetria.Hardware.PowerModules.Common;
using Telemetria.Hardware.PowerModules.Interfaces;
using Telemetria.Persistence.Light.IniOptions.Common;

namespace PowerModulesManager.Services
{
    public sealed class MainViewModelPm : MagicMessanger, IMainViewModelPm
    {
        private IPmSender<IRPackPmModulesBase> _pmSender;
        private readonly FactoryPm _factoryPm;
        private string _currentTestId;
        public MainViewModelPm(FactoryPm factoryPm)
        {
            _factoryPm = factoryPm;
            TypesModuleList = Enum.GetValues(typeof(PowerSupplyType)).Cast<PowerSupplyType>().ToList();
            StartCmd = new RelayCommand(startCmdExecute);
            StopCmd = new RelayCommand(stopCmdExecute);
            StopBroadcastCmd = new RelayCommand(stopBroadcastCmdExecute);
            CanAddressStr = "1";
            FreqGetData = 1;
            Voltage = "400";
            Current = "100";
            TypeModule = PowerSupplyType.Uugp;
            Statuses = new AsyncObservableCollection<StatusPm>();
            //StopButtonEnable = true;
            StartButtonEnable = true;
            PropertyChanged += OnPropertyChanged;
        }

        private void initSender()
        {
            //if(TypeModule == null) return;
            _pmSender?.Stop();
            //_pmSender = _factoryPm.GetPmSender(TypeModule);
            //_pmSender?.Start();
        }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TypeModule))
            {
                initSender();
            }
        }
        private async void stopBroadcastCmdExecute()
        {
            await Task.Run(() =>
            {
                StartButtonEnable = false;
                stopGetStates();
            }).ContinueWith(x =>
            {
                Thread.Sleep(500);
                StartButtonEnable = true;
            });
        }
        private async void stopCmdExecute()
        {
            await Task.Run(async () =>
            {
                StartButtonEnable = false;
                //stopGetStates();
                if(!checkBase()) return;
                _pmSender.Start();
                var resStop = await _pmSender.PowerOnOffSpecModule(false, CanAddress, null, null);
                if (!resStop)
                    sendMessage(MessageType.Error, "Не удалось остановить модуль");
                else
                {
                    sendMessage(MessageType.Information, "Модуль успешно остановлен");
                }
            }).ContinueWith(x =>
            {
                Thread.Sleep(500);
                StartButtonEnable = true;
            });
        }

        private async void startCmdExecute()
        {
            await Task.Run(async () =>
            {
                StartButtonEnable = false;
                var va = checkStartData();
                if (va == null) return;
                stopGetStates();
                _currentTestId = new Random().Next(0, 100000).ToString();
                _pmSender.Start();
                await Task.Delay(500);
                var res = await startV1(va.Value);
                if (!res)
                    sendMessage(MessageType.Error, "Не удалось запустить модуль");
                else {
                    sendMessage(MessageType.Information, "Модуль запущен");
                }
                startGetStates();
            }).ContinueWith(x =>
            {
                Thread.Sleep(500);
                StartButtonEnable = true;
            });
        }

        private async Task<bool> startV1((float voltage, float current) va)
        {
            // await Log.Info("***Start***");
            var res = await _pmSender.PowerOnOffSpecModule(true, CanAddress, va.voltage, va.current);
            // await Log.Info("***StartFinish***");
            return res;
        }

        private async Task<bool> startV2((float voltage, float current) va)
        {
            var resSet = await _pmSender.SetVoltageAndCurrent(CanAddress, va.voltage, va.current);
            return resSet && await _pmSender.PowerOnOffSpecModule(true, CanAddress, null, null);
        }

        public string Voltage { get; set; }
        public string Current { get; set; }
        public ICommand StartCmd { get; set; }
        public ICommand StopCmd { get; set; }

        public ICommand StopBroadcastCmd { get; set; }
        public AsyncObservableCollection<StatusPm> Statuses { get; set; }
        public string CanAddressStr { get; set; }
        public byte CanAddress { get; set; }

        public PowerSupplyType? TypeModule { get; set; }
        public List<PowerSupplyType> TypesModuleList { get; set; }
        public bool StartButtonEnable { get; set; }
        //public bool StopButtonEnable { get; set; }
        public int FreqGetData { get; set; }
        private (float voltage, float current)? checkStartData()
        {
            if (!checkBase()) return null;
            if (!tryCastToFloat(Voltage, out var valueFloat))
            {
                sendMessage(MessageType.Warning, "Не удалось конвертировать напряжение в дробное число");
                return null;
            }
            Voltage = valueFloat.ToString(CultureInfo.InvariantCulture);
            if (!tryCastToFloat(Current, out var f))
            {
                sendMessage(MessageType.Warning, "Не удалось конвертировать напряжение в дробное число");
                return null;
            }
            Current = f.ToString(CultureInfo.InvariantCulture);
           
                return (valueFloat, f);
        }

        private bool checkBase()
        {
            if (!isSupportedPowerModule(TypeModule))
            {
                sendMessage(MessageType.Warning, $"Тип модуля {TypeModule.ToString()} пока не поддерживается");
                return false;
            }
            var res = byte.TryParse(CanAddressStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out var canAddr);
            if (res)
                CanAddress = canAddr;
            else {
                sendMessage(MessageType.Warning, "Не установлен Can адрес");
                return false;
            }
            _pmSender = _factoryPm.GetPmSender(TypeModule);
            return true;
        }

        private bool isSupportedPowerModule(PowerSupplyType? powerSupplyType)
        {
            if (!powerSupplyType.HasValue) return false;
            var typeName = powerSupplyType.Value.ToString();
            return string.Equals(typeName, nameof(PowerSupplyType.Tonhe), StringComparison.OrdinalIgnoreCase)
                || string.Equals(typeName, nameof(PowerSupplyType.Uugp), StringComparison.OrdinalIgnoreCase)
                || typeName.IndexOf("Pscs", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool tryCastToFloat(string value, out float valueFloat)
        {
            value = value?.Replace(",", ".");
            var convertRes = float.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var result);
            if (!convertRes)
            {
                valueFloat = -1;
                return false;
            }
            valueFloat = result;
            valueFloat = (float)Math.Round(valueFloat, 1);
            if (valueFloat < 0)
                valueFloat = -1 * valueFloat;
            return true;
        }

        private bool _isGetStates;
        private Task _getStates;
        private void startGetStates()
        {
            if (_isGetStates) return;
            _isGetStates = true;
           _getStates = Task.Run(async () =>
            {
                while (_isGetStates)
                {
                    var status = await _pmSender.GetVoltageCurrentAndStatus(CanAddress);
                    if (status != null)
                        Statuses.Add(new StatusPm(_currentTestId) { PmData = status });
                    await checkCancellation(FreqGetData);
                }
            });
        }

        private void stopGetStates()
        {
            _isGetStates = false;
            _getStates?.Wait(3000);
        }
        private Task<bool> checkCancellation(int timeInSec)
        {
            return Task.Run(async () =>
            {
                var sw = Stopwatch.StartNew();
                while (_isGetStates && sw.Elapsed.TotalSeconds < timeInSec) {
                    await Task.Delay(200);
                }
                sw.Stop();
                return _isGetStates && sw.Elapsed.TotalSeconds > timeInSec;
            });
        }
    }

    /*public class StartingData
    {
        public float Voltage { get; set; }
        public float Current { get; set; }
        public byte Ca
    }*/
}
