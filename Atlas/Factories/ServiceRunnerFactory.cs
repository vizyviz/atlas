using Atlas.Runners;
using Autofac;

namespace Atlas.Factories
{
    internal class ServiceRunnerFactory : IServiceRunnerFactory
    {
        public IServiceRunner Create(RunMode runMode)
        {
            var parameter = new NamedParameter(HostModule.RUN_MODE_PARAMETER, runMode);
            return ContainerProvider.Instance.ApplicationContainer.Resolve<IServiceRunner>(parameter);
        }
    }
}