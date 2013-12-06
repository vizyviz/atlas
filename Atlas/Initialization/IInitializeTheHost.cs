using Atlas.Configuration;

namespace Atlas.Initialization
{
    internal interface IInitializeTheHost
    {
        void Initialize<TProcessorHost>(Configuration<TProcessorHost> configuration);
    }
}