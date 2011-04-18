using System;
using System.Configuration;
using Atlas.Configuration;
using Atlas.Helpers;
using Autofac;

namespace Atlas
{
    public partial class Host
    {
        public static Configuration<TProcessorHost> Configure<TProcessorHost>()
        {
            var config = new Configuration<TProcessorHost>();
            return config;
        }

        public static Configuration<TProcessorHost> Configure<TProcessorHost>(Action<Configuration<TProcessorHost>> configuration) where TProcessorHost : IAmAHostedProcess
        {
            var config = new Configuration<TProcessorHost>();
            configuration.Invoke(config);
            return config;
        }

        public static Configuration<TProcessorHost> Configure<TProcessorHost>(Action<Configuration<TProcessorHost>> configuration, string[] arguments) where TProcessorHost : IAmAHostedProcess
        {
            var argument = new Arguments(arguments);
            var config = new Configuration<TProcessorHost>
                             {
                                 RunMode = argument.RunMode,
                                 InstallMode = argument.InstallMode,
                                 Account = argument.Account,
                                 UserName = argument.UserName,
                                 Password = argument.Password,
                                 StartMode = argument.StartMode,
                             };
            configuration.Invoke(config);
            return config;
        }

        public static Configuration<TProcessorHost> UseAppConfig<TProcessorHost>() where TProcessorHost : IAmAHostedProcess
        {
            var section = (Configuration<object>)ConfigurationManager.GetSection("atlas");
            return new Configuration<TProcessorHost>
                       {
                           AllowsMultipleInstances = section.AllowsMultipleInstances,
                           Dependencies = section.Dependencies,
                           Name = section.Name,
                           RunMode = section.RunMode,
                           Account = section.Account,
                           StartMode = section.StartMode,
                           UserName = section.UserName,
                           Password = section.Password,
                           Description = section.Description,
                           DisplayName = section.DisplayName,
                       };
        }

        public static void Start<TProcessorHost>(Configuration<TProcessorHost> configuration)
        {
            if (!configuration.AllowsMultipleInstances && ProcessMonitor.Instance.AmIRunning)
            {
                System.Console.WriteLine("{0} already running, exiting this instance", AppDomain.CurrentDomain.FriendlyName);
                return;
            }

            ContainerProvider.Instance.ApplicationContainer = configuration.Compile();

            var hostInstance = ContainerProvider.Instance.ApplicationContainer.Resolve<Host>();

            hostInstance.Run(configuration);
        }
    }
}