using System;
using System.ServiceProcess;

namespace Atlas.Helpers
{
    internal class ServiceStarter : IStartServices
    {
        public void Start(string named)
        {
            var service = new ServiceController(named);
            try
            {
                var timeout = TimeSpan.FromSeconds(5);

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }
}