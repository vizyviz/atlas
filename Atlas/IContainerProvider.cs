using Autofac;

namespace Atlas
{
    /// <summary>
    /// Provides access to the application's container, and methods to manage units of work within your system
    /// </summary>
    public interface IContainerProvider
    {
        IContainerProvider Instance { get; }
        IContainer ApplicationContainer { get; set; }
        IUnitOfWorkContainer CreateUnitOfWork();
    }
}