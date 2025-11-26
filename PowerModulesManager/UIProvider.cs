using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Hardware.DeviceSending;
using JetBrains.Annotations;
using ProgrammingWeapons;
using Telemetria;
using Telemetria.ApplicationBase;
using Telemetria.Hardware.TcpDevices;
using Telemetria.UI;

namespace PowerModulesManager
{
    public interface IUIProviderPm : IUIProvider
    {
        
    }
    public class UIProvider : Notifier, IUIProviderPm
    {

        public UIProvider(IInputElement mainContent, IBottomPanel bottomPanel, TcpDevice tcpDevice)
        {
            Content = mainContent;
            OptionsWidgets = new List<OptionsWidgetBase> {  };
            BottomPanel = bottomPanel;
            OptionsWidgets = new List<OptionsWidgetBase>
            {
                tcpDevice.SettingsTcp.OptionsWidget()
            };
        }

        public event Action<string> BarcodeReceived
        {
            add { }
            remove {  }
        }
        public IInputElement BottomPanel { get; private set; }

        public IInputElement Content { get; private set; }
        [CanBeNull] public IList<OptionsWidgetBase> OptionsWidgets { get; private set; }
        public IInputElement TopPanel { get; private set; }
        public ObservableCollection<IWidget> Widgets { get; private set; }


    }
}