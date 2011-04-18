using Atlas.Configuration;
using Atlas.Factories;

namespace Atlas.Initialization
{
    internal class RunInitialization : IInitializeTheHost
    {
        private readonly IServiceRunnerFactory _serviceRunnerFactory;

        public RunInitialization(IServiceRunnerFactory serviceRunnerFactory)
        {
            _serviceRunnerFactory = serviceRunnerFactory;
        }

        public void Initialize<TProcessorHost>(Configuration<TProcessorHost> configuration)
        {
            _serviceRunnerFactory.Create(configuration.RunMode).Run();
        }
    }
}