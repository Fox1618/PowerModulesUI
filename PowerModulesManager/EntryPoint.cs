using ProgrammingWeapons.Logging.Log4Net;
using System;
using System.Windows;
using ProgrammingWeapons.Logging;
using Telemetria;
using Telemetria.UI;

namespace PowerModulesManager
{
    internal class EntryPoint
    {
        [STAThread]
        public static void Main()
        {
            var diContainer = DIFactory.GetContainer();
            diContainer.Get<IAuthorizeWidget>().Hide();
            Log.SetAdapter(diContainer.Get<Log4NetLoggerAdapter>());
            var app = diContainer.Get<Application>();
            HandlerException.Handle();
            var window = diContainer.Get<Window>();
            app.Run(window);
        }
    }
}
