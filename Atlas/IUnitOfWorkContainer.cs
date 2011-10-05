using System;

namespace Atlas
{
    /// <summary>
    /// Encapsulates a unit of work within your system
    /// </summary>
    public interface IUnitOfWorkContainer : IDisposable
    {
        /// <summary>
        /// Resolves an entity from the registration source
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <returns>An instance of the type resolved</returns>
        T Resolve<T>();

        /// <summary>
        /// Injects any properties that have not been set that are registred in the container into the provided instance
        /// </summary>
        /// <param name="instance"></param>
        void InjectUnsetProperties(object instance);
    }
}