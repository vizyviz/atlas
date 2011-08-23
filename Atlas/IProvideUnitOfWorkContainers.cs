using Autofac;

namespace Atlas
{
    public interface IProvideUnitOfWorkContainers
    {
        IProvideUnitOfWorkContainers Instance { get; set; }
        IContainer ApplicationContainer { get; set; }
        ILifetimeScope CreateUnitOfWorkContainer();
    }
}