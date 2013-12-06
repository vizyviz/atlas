using Atlas.Helpers;
using Atlas.Initialization;
using Atlas.Installation;

namespace Atlas.Factories
{
    internal class IntitializationStrategyFactory : ICreateInitializationStrategies
    {
        private readonly IServiceRunnerFactory _serviceRunnerFactory;
        private readonly IInstallerFactory _installerFactory;
        private readonly IStartServices _serviceStarter;

        public IntitializationStrategyFactory(IServiceRunnerFactory serviceRunnerFactory, IInstallerFactory installerFactory, IStartServices serviceStarter)
        {
            _serviceRunnerFactory = serviceRunnerFactory;
            _installerFactory = installerFactory;
            _serviceStarter = serviceStarter;
        }

        public IInitializeTheHost Create(InstallMode installMode)
        {
            switch (installMode)
            {
                case InstallMode.Install:
                case InstallMode.Uninstall:
                    return new InstallInitialization(_installerFactory);
                case InstallMode.InstallAndStart:
                    return new InstallAndStartInitialization(_installerFactory, _serviceStarter);
                case InstallMode.NotSet:
                default:
                    return new RunInitialization(_serviceRunnerFactory);
            }
        }
    }
}