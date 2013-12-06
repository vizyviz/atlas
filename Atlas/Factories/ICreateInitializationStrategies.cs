using Atlas.Initialization;
using Atlas.Installation;

namespace Atlas.Factories
{
    internal interface ICreateInitializationStrategies
    {
        IInitializeTheHost Create(InstallMode installMode);
    }
}