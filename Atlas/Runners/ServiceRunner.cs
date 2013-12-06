using System;

namespace Atlas.Runners
{
    internal class ServiceRunner : MarshalByRefObject, IServiceRunner
    {
        private readonly WindowsService _windowsService;

        public ServiceRunner(WindowsService windowsService)
        {
            _windowsService = windowsService;
        }

        public void Run()
        {
            System.ServiceProcess.ServiceBase.Run(_windowsService);
        }
    }
}