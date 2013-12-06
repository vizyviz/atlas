using System.Diagnostics;

namespace Atlas.Helpers
{
    internal class ProcessMonitor : IMonitorProcesses
    {
        private ProcessMonitor()
        {
        }

        private static IMonitorProcesses _instance;

        public static IMonitorProcesses Instance
        {
            get { return _instance ?? (_instance = new ProcessMonitor()); }
            set { _instance = value; }
        }

        public bool AmIRunning
        {
            get { return Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1; }
        }
    }
}