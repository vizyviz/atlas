using System;
using System.Collections.Generic;
using System.ServiceProcess;
using Atlas.Installation;
using Atlas.Runners;
using Autofac;

namespace Atlas.Configuration
{
    public class Configuration<TProcessorHost>
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

        public Configuration<TProcessorHost> WithDependencyOnServiceNamed(string serviceName, TimeSpan timeToWaitForStart)
        {
            _dependencies.Add(new Dependency(serviceName, timeToWaitForStart));
            return this;
        }

        public Configuration<TProcessorHost> AllowMultipleInstances()
        {
            AllowsMultipleInstances = true;
            return this;
        }

        public Configuration<TProcessorHost> BeforeStart(Action beforeStart)
        {
            OnBeforeStart = beforeStart;
            return this;
        }

        public Configuration<TProcessorHost> WithDependencyOnServiceNamed(string serviceName)
        {
            _dependencies.Add(new Dependency(serviceName));
            return this;
        }

        public Configuration<TProcessorHost> WithRegistrations(Action<ContainerBuilder> registrations)
        {
            Registrations = registrations;
            return this;
        }

        public Configuration<TProcessorHost> WithArguments(string[] args)
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

        public Configuration<TProcessorHost> Named(string serviceName, string displayName = "", string description = "")
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

            builder.RegisterType(typeof(TProcessorHost))
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