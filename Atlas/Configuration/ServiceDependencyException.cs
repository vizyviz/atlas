using System;

namespace Atlas.Configuration
{
    public class ServiceDependencyException : Exception
    {
        public ServiceDependencyException(string message)
            : base(message)
        {
        }
    }
}