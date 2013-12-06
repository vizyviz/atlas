using System;
using System.Collections.Generic;
using Atlas;
using Atlas.Factories;
using Atlas.Initialization;
using Atlas.Installation;
using Atlas.Configuration;
using Autofac;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests.HostTests
{
    public partial class HostTests
    {
        [TestFixture]
        public class WhenRunning
        {
            private IInitializeTheHost _initializationStrategy;
            private ICreateInitializationStrategies _initializationStrategyFactory;

            [SetUp]
            public void SetUp()
            {
                _initializationStrategyFactory = MockRepository.GenerateDynamicMockWithRemoting<ICreateInitializationStrategies>();
                _initializationStrategy = MockRepository.GenerateDynamicMockWithRemoting<IInitializeTheHost>();
            }

            [TearDown]
            public void TearDown()
            {
                _initializationStrategyFactory.VerifyAllExpectations();
                _initializationStrategy.VerifyAllExpectations();
            }

            [Test]
            public void WithNoDependencies()
            {
                Host host = new Host(_initializationStrategyFactory);
                var config = new FakeConfig(host);

                CreatesInitializationStrategy();
                InitializesTheHost(config);

                host.Run(config);
            }

            [Test]
            public void WithDependencies()
            {

                Host host = new Host(_initializationStrategyFactory);
                var config = new FakeConfig(host);
                var dependency1 = new FakeDependency("Some Service");
                var dependency2 = new FakeDependency("Some Other Service");

                config.Dependencies = new List<Dependency>
                                          {
                                              dependency1,
                                              dependency2,
                                          };

                CreatesInitializationStrategy();
                InitializesTheHost(config);

                host.Run(config);

                Assert.IsTrue(dependency1.Started);
                Assert.IsTrue(dependency2.Started);
            }

            [Test]
            public void WithoutRegistrations()
            {
                Host host = new Host(_initializationStrategyFactory);
                var config = new FakeConfig(host);
                ContainerProvider.Instance.ApplicationContainer = config.Compile();

                CreatesInitializationStrategy();
                InitializesTheHost(config);

                host.Run(config);

                Assert.IsFalse(ContainerProvider.Instance.ApplicationContainer.IsRegistered(typeof(Thing)));
            }

            [Test]
            public void WithRegistrations()
            {
                Host host = new Host(_initializationStrategyFactory);
                var config = new FakeConfig(host);
                config.WithRegistrations(b => b.Register(c => new Thing()));

                ContainerProvider.Instance.ApplicationContainer = config.Compile();

                CreatesInitializationStrategy();
                InitializesTheHost(config);

                host.Run(config);

                Assert.IsTrue(ContainerProvider.Instance.ApplicationContainer.IsRegistered(typeof(Thing)));
            }

            [Test]
            public void WithInstallModeSet()
            {
                Host host = new Host(_initializationStrategyFactory);
                var config = new FakeConfig(host);
                config.InstallMode = InstallMode.Uninstall;

                CreatesInitializationStrategy();
                InitializesTheHost(config);

                host.Run(config);
            }

            private void InitializesTheHost(Configuration<FakeProcess> config)
            {
                _initializationStrategy.Expect(i => i.Initialize(config));
            }

            private void CreatesInitializationStrategy()
            {
                _initializationStrategyFactory.Expect(i => i.Create(Arg<InstallMode>.Is.Anything)).Return(
                    _initializationStrategy);
            }
        }
    }

    public class Thing
    {
    }

    internal class FakeDependency : Dependency
    {
        internal FakeDependency(string name)
            : base(name)
        {
        }

        internal FakeDependency(string name, TimeSpan timeout)
            : base(name, timeout)
        {
        }

        public override void Start()
        {
            Started = true;
        }

        public bool Started { get; set; }
    }
}
