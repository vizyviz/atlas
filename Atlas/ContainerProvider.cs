using Autofac;

namespace Atlas
{
    public class ContainerProvider
    {
        private static ContainerProvider _instance;

        private ContainerProvider()
        {
        }

        public static ContainerProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ContainerProvider();
                }
                return _instance;
            }
        }

        public IContainer ApplicationContainer { get; set; }

        public ILifetimeScope CreateUnitOfWorkContainer()
        {
            return ApplicationContainer.BeginLifetimeScope();
        }
    }
}