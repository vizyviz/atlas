using System;
using Autofac;

namespace Atlas
{
    /// <summary>
    /// Provides access to the application's container, and methods to manage units of work within your system
    /// </summary>
    public class ContainerProvider : IContainerProvider
    {
        private static IContainerProvider _instance = new Lazy<IContainerProvider>(() => new ContainerProvider()).Value;

        private ContainerProvider()
        {
        }

        /// <summary>
        /// Gets the instance of the container provider
        /// </summary>
        public static IContainerProvider Instance
        {
            get { return _instance; }
            internal set { _instance = value; }
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

        /// <summary>
        /// Creates a unit of work with unit of work scoped registrations
        /// </summary>
        /// <returns></returns>
        public IUnitOfWorkContainer CreateUnitOfWork(Action<ContainerBuilder> registrations)
        {
            return new UnitOfWorkContainer(ApplicationContainer.BeginLifetimeScope(registrations));
        }
    }
}