using Atlas.Configuration;

namespace Atlas.Installation
{
    internal interface IInstallerFactory
    {
        IInstaller Create<TProcessorHost>(Configuration<TProcessorHost> configuration);
    }
}