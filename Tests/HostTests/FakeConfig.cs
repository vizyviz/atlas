using Atlas;
using Atlas.Configuration;
using Autofac;

namespace Tests.HostTests
{
    public class FakeConfig : Configuration<FakeProcess>
    {
        private readonly Host _fakeHost;

        public FakeConfig(Host fakeHost)
        {
            _fakeHost = fakeHost;
        }

        public IContainer CompiledContainer { get; set; }

        internal override IContainer Compile()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(_fakeHost).As<Host>();
            return CompiledContainer = builder.Build();
        }
    }
}