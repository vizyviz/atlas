using Autofac;

namespace Atlas
{
    public interface IProvideUnitOfWorkContainers
    {
        IProvideUnitOfWorkContainers Instance { get; }
        IContainer ApplicationContainer { get; set; }
        ILifetimeScope CreateUnitOfWorkContainer();
    }
}