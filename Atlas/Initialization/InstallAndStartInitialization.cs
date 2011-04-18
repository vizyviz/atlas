using Atlas.Configuration;
using Atlas.Helpers;
using Atlas.Installation;

namespace Atlas.Initialization
{
    internal class InstallAndStartInitialization : IInitializeTheHost
    {
        private readonly IInstallerFactory _installerFactory;
        private readonly IStartServices _serviceStarter;

        public InstallAndStartInitialization(IInstallerFactory installerFactory, IStartServices serviceStarter)
        {
            _installerFactory = installerFactory;
            _serviceStarter = serviceStarter;
        }

        public void Initialize<TProcessorHost>(Configuration<TProcessorHost> configuration)
        {
            _installerFactory.Create(configuration).Install<TProcessorHost>(configuration.Name);
            _serviceStarter.Start(configuration.Name);
        }
    }
}