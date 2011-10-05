using Autofac;

namespace Atlas
{
    internal class UnitOfWorkContainer : IUnitOfWorkContainer
    {
        private readonly ILifetimeScope _scope;

        public UnitOfWorkContainer(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public T Resolve<T>()
        {
            return _scope.Resolve<T>();
        }

        public void InjectUnsetProperties(object instance)
        {
            _scope.InjectUnsetProperties(instance);
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}