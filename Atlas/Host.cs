using Atlas.Factories;
using Autofac;
using Atlas.Configuration;

namespace Atlas
{
    public partial class Host
    {
        private readonly ICreateInitializationStrategies _initilaizationStrategyFactory;

        internal Host(ICreateInitializationStrategies initilaizationStrategyFactory)
        {
            _initilaizationStrategyFactory = initilaizationStrategyFactory;
        }

        public virtual void Run<THostedProcess>(Configuration<THostedProcess> configuration)
        {
            foreach (var dependency in configuration.Dependencies)
            {
                dependency.Start();
            }

            UpdateRegistrations(configuration);

            _initilaizationStrategyFactory.Create(configuration.InstallMode).Initialize(configuration);
        }

        private static void UpdateRegistrations<THostedProcess>(Configuration<THostedProcess> configuration)
        {
            if (configuration.Registrations != null)
            {
                var builder = new ContainerBuilder();

                configuration.Registrations(builder);

                builder.Update(ContainerProvider.Instance.ApplicationContainer);
            }
        }
    }
}