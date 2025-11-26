using System;
using Telemetria.Hardware.PowerModules.Common;

namespace PowerModulesManager.Services
{
    public class StatusPm
    {
        public StatusPm(string testId)
        {
            Dt = DateTime.Now;
            TestId = testId;
        }
        public PmInfo PmData { get; set; }
        public string TestId { get; set; }
        public DateTime Dt { get; set; }
    }
}
