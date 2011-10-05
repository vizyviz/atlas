using Autofac;

namespace Atlas
{
    /// <summary>
    /// Provides access to the application's container, and methods to manage units of work within your system
    /// </summary>
    public class ContainerProvider : IContainerProvider
    {
        private static IContainerProvider _instance;

        private ContainerProvider()
        {
        }

        /// <summary>
        /// Gets the instance of the container provider
        /// </summary>
        public static IContainerProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ContainerProvider();
                }
                return _instance;
            }
            set { _instance = value; }
        }

        IContainerProvider IContainerProvider.Instance { get { return Instance; } }

        /// <summary>
        /// The application's outer container
        /// </summary>
        public IContainer ApplicationContainer { get; set; }

        /// <summary>
        /// Creates a unit of work to provide scope around the application container
        /// </summary>
        /// <returns></returns>
        public IUnitOfWorkContainer CreateUnitOfWork()
        {
            return new UnitOfWorkContainer(ApplicationContainer.BeginLifetimeScope());
        }
    }
}