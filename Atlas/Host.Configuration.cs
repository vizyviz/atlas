using System;
using System.Configuration;
using Atlas.Configuration;
using Atlas.Helpers;
using Autofac;

namespace Atlas
{
    ///<summary>
    /// Configures and starts hosted applications/services
    ///</summary>
    public partial class Host
    {
        ///<summary>
        /// Configure a service with all defaults
        ///</summary>
        ///<typeparam name="TProcessorHost">The service that implements IAmAHostedProcess</typeparam>
        ///<returns>Default configuration</returns>
        public static Configuration<TProcessorHost> Configure<TProcessorHost>() where TProcessorHost : IAmAHostedProcess
        {
            var config = new Configuration<TProcessorHost>();
            return config;
        }

        ///<summary>
        /// Configure a service injecting the configuration via lambda expression
        ///</summary>
        ///<param name="configuration">Lambda expression to set configuration</param>
        ///<typeparam name="TProcessorHost">The service that implements IAmAHostedProcess</typeparam>
        ///<returns>Configuration with settings from lambda expression</returns>
        public static Configuration<TProcessorHost> Configure<TProcessorHost>(Action<Configuration<TProcessorHost>> configuration) where TProcessorHost : IAmAHostedProcess
        {
            var config = new Configuration<TProcessorHost>();
            configuration.Invoke(config);
            return config;
        }

        ///<summary>
        /// Configure a service injecting the configuration via lambda expression overriding arguments explicitly
        ///</summary>
        ///<param name="configuration">Lambda expression to set configuration</param>
        ///<param name="arguments">The command line arguments to be used</param>
        ///<typeparam name="TProcessorHost">The service that implements IAmAHostedProcess</typeparam>
        ///<returns>Configuration with settings from lambda expression overriding settings with arguments</returns>
        public static Configuration<TProcessorHost> Configure<TProcessorHost>(Action<Configuration<TProcessorHost>> configuration, string[] arguments) where TProcessorHost : IAmAHostedProcess
        {
            var argument = new Arguments(arguments);
            var config = new Configuration<TProcessorHost>();
            configuration.Invoke(config);
            config.RunMode = argument.RunMode;
            config.InstallMode = argument.InstallMode;
            config.Account = argument.Account;
            config.UserName = argument.UserName;
            config.Password = argument.Password;
            config.StartMode = argument.StartMode;
            return config;
        }

        ///<summary>
        /// Configure a service using the app.config file
        ///</summary>
        ///<typeparam name="TProcessorHost">The service that implements IAmAHostedProcess</typeparam>
        ///<returns>Configured service with settings in app.config file</returns>
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

        ///<summary>
        /// Starts the configuration
        ///</summary>
        ///<param name="configuration">The configuration to be run</param>
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