using System;
using System.ServiceProcess;

namespace Atlas.Configuration
{
    internal class Dependency
    {
        internal Dependency(string name)
            : this(name, TimeSpan.FromSeconds(15))
        {
        }

        internal Dependency(string name, TimeSpan timeout)
        {
            Name = name;
            Timeout = timeout;
        }

        public string Name { get; set; }
        public TimeSpan Timeout { get; set; }

        public virtual void Start()
        {
            var serviceController = new ServiceController(Name);
            try
            {
                if (serviceController.Status == ServiceControllerStatus.Stopped)
                {
                    serviceController.Start();
                }
                serviceController.WaitForStatus(ServiceControllerStatus.Running, Timeout);
            }
            catch (InvalidOperationException ex)
            {
                throw new ServiceDependencyException(ex.Message);
            }
            finally
            {
                serviceController.Close();
            }
        }
    }
}