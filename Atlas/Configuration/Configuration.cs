using System;
using System.Collections.Generic;
using System.ServiceProcess;
using Atlas.Installation;
using Atlas.Runners;
using Autofac;

namespace Atlas.Configuration
{
    public class Configuration<THostedProcess>
    {
        private IList<Dependency> _dependencies;

        internal Configuration()
        {
            AllowsMultipleInstances = false;
            OnBeforeStart = () => { };
            _dependencies = new List<Dependency>();
            RunMode = RunMode.NotSet;
            InstallMode = InstallMode.NotSet;
            Account = ServiceAccount.LocalSystem;
            StartMode = ServiceStartMode.Manual;
        }

        ///<summary>
        /// Specifying a service name here will attempt to start the service if it is not running.
        /// If the service does not start within the specified time a ServiceDependencyException is thrown and atlas will exit.
        ///</summary>
        ///<param name="serviceName">The name of the service to start</param>
        ///<param name="timeToWaitForStart">The time to wait for the service to start</param>
        public Configuration<THostedProcess> WithDependencyOnServiceNamed(string serviceName, TimeSpan timeToWaitForStart)
        {
            _dependencies.Add(new Dependency(serviceName, timeToWaitForStart));
            return this;
        }

        ///<summary>
        /// Specifying a service name here will attempt to start the service if it is not running.
        /// If the service does not start within the default 15 seconds a ServiceDependencyException is thrown and atlas will exit.
        ///</summary>
        ///<param name="serviceName">The name of the service to start</param>
        public Configuration<THostedProcess> WithDependencyOnServiceNamed(string serviceName)
        {
            _dependencies.Add(new Dependency(serviceName));
            return this;
        }

        ///<summary>
        /// Allows multiple instances of this service to run, default is to not allow multiple instances
        ///</summary>
        public Configuration<THostedProcess> AllowMultipleInstances()
        {
            AllowsMultipleInstances = true;
            return this;
        }

        ///<summary>
        /// Performs the specified action before atlas starts your service.
        ///</summary>
        public Configuration<THostedProcess> BeforeStart(Action beforeStart)
        {
            OnBeforeStart = beforeStart;
            return this;
        }
        
        ///<summary>
        /// Autofac registrations of any dependencies that my service may have or use.
        /// These dependencies are property injected into your service.
        /// If your service uses constructor injection re-register your service as IAmAHostedProcess here.
        ///</summary>
        public Configuration<THostedProcess> WithRegistrations(Action<ContainerBuilder> registrations)
        {
            Registrations = registrations;
            return this;
        }

        ///<summary>
        /// Allows you to inject the command line arguments into your atlas.  These arguments override the run mode (console or service), and installation parameters.
        /// Account, Startup, Username, Password, etc.
        ///</summary>
        public Configuration<THostedProcess> WithArguments(string[] args)
        {
            var arguments = new Arguments(args);
            RunMode = arguments.RunMode;
            Account = arguments.Account;
            StartMode = arguments.StartMode;
            UserName = arguments.UserName;
            Password = arguments.Password;
            InstallMode = arguments.InstallMode;
            return this;
        }

        ///<summary>
        /// Allows you to specify the name the service.  Display names and descriptions are optional.
        ///</summary>
        public Configuration<THostedProcess> Named(string serviceName, string displayName = "", string description = "")
        {
            Name = serviceName;
            DisplayName = displayName;
            Description = description;
            return this;
        }

        internal virtual IContainer Compile()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new HostModule());

            builder.RegisterType(typeof(THostedProcess))
                .As<IAmAHostedProcess>()
                .OnActivated(a => a.Context.InjectUnsetProperties(a.Instance));

            return builder.Build();
        }
        internal Action<ContainerBuilder> Registrations { get; set; }
        internal Action OnBeforeStart { get; set; }
        internal string Name { get; set; }
        internal string Description { get; set; }
        internal string DisplayName { get; set; }
        internal RunMode RunMode { get; set; }
        internal InstallMode InstallMode { get; set; }
        internal string UserName { get; set; }
        internal string Password { get; set; }
        internal ServiceAccount Account { get; set; }
        internal ServiceStartMode StartMode { get; set; }
        internal bool AllowsMultipleInstances { get; set; }
        internal IList<Dependency> Dependencies
        {
            get { return _dependencies; }
            set { _dependencies = value; }
        }

    }
}