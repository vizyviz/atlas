using Atlas.Console;
using Atlas.Factories;
using Atlas.Helpers;
using Atlas.Installation;
using Atlas.Runners;
using Autofac;

namespace Atlas
{
    internal class HostModule : Module
    {
        public const string RUN_MODE_PARAMETER = "RunMode";

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ConsoleWrapper())
                .As<IWrapConsole>()
                .SingleInstance();

            builder.Register(c => new ServiceRunnerFactory())
                .As<IServiceRunnerFactory>()
                .SingleInstance();

            builder.Register(c => new InstallerFactory())
                .As<IInstallerFactory>()
                .SingleInstance();

            builder.Register(c => new ServiceStarter())
                .As<IStartServices>()
                .SingleInstance();

            builder.Register(c => new Host(c.Resolve<ICreateInitializationStrategies>()))
                .SingleInstance();

            builder.Register(c => new ConsoleRunner(c.Resolve<IAmAHostedProcess>(), c.Resolve<IWrapConsole>()))
                .As<ConsoleRunner>()
                .SingleInstance();

            builder.Register(c => new WindowsService(c.Resolve<IAmAHostedProcess>()))
              .As<WindowsService>()
              .SingleInstance();

            builder.Register(c => new ServiceRunner(c.Resolve<WindowsService>()))
                .As<ServiceRunner>()
                .SingleInstance();

            builder.Register((c, p) => CreateRunner(p.Named<RunMode>(RUN_MODE_PARAMETER), c))
                 .As<IServiceRunner>()
                 .SingleInstance();

            builder.Register(
                c =>
                new IntitializationStrategyFactory(c.Resolve<IServiceRunnerFactory>(),
                                                   c.Resolve<IInstallerFactory>(),
                                                   c.Resolve<IStartServices>()))
                .As<ICreateInitializationStrategies>()
                .SingleInstance();
        }

        private static IServiceRunner CreateRunner(RunMode mode, IComponentContext c)
        {
            if (mode == RunMode.Console)
            {
                return c.Resolve<ConsoleRunner>();
            }
            return c.Resolve<ServiceRunner>();
        }
    }
}