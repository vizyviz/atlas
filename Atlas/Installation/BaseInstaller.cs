using System;
using System.Collections;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace Atlas.Installation
{
    internal abstract class BaseInstaller : IInstaller
    {
        private AssemblyInstaller CreateInstaller(Assembly assembly)
        {
            var installer = new AssemblyInstaller(assembly, null) { UseNewContext = true };

            var serviceInstaller = new ServiceInstaller
            {
                DisplayName = DisplayName,
                Description = Description,
                ServiceName = ServiceName,
                StartType = StartType
            };

            var serviceProcessInstaller = new ServiceProcessInstaller
            {
                Account = Account,
                Password = Password,
                Username = Username
            };
            installer.Installers.Add(serviceProcessInstaller);
            installer.Installers.Add(serviceInstaller);
            return installer;
        }

        protected void InvokeInstallCommand<TProcessorHost>(Action<AssemblyInstaller, IDictionary> installAction)
        {
            IDictionary state = new Hashtable();
            var installer = CreateInstaller(typeof(TProcessorHost).Assembly);

            try
            {
                installAction(installer, state);
                installer.Commit(state);
            }
            catch
            {
                try
                {
                    installer.Rollback(state);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }

        public abstract void Install<TProcessorHost>(string name);
        public ServiceAccount Account { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string ServiceName { get; set; }
        public ServiceStartMode StartType { get; set; }
    }
}