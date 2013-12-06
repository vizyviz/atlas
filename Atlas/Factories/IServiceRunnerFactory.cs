using Atlas.Runners;

namespace Atlas.Factories
{
    internal interface IServiceRunnerFactory
    {
        IServiceRunner Create(RunMode runMode);
    }
}