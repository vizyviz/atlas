using Atlas.Configuration;
using Atlas.Installation;

namespace Atlas.Initialization
{
    internal class InstallInitialization : IInitializeTheHost
    {
        private readonly IInstallerFactory _installerFactory;

        public InstallInitialization(IInstallerFactory installerFactory)
        {
            _installerFactory = installerFactory;
        }

        public void Initialize<TProcessorHost>(Configuration<TProcessorHost> configuration)
        {
            _installerFactory.Create(configuration).Install<TProcessorHost>(configuration.Name);
        }
    }
}