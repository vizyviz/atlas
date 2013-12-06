using System;
using Autofac.Core.Lifetime;
using Autofac.Core.Registration;
using Autofac;
using Atlas;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests
{
    [TestFixture]
    public class ContainerProviderTests
    {
        private IContainer _applicationContainer;
        private IContainerProvider _provider;

        [SetUp]
        public void Setup()
        {
            _applicationContainer = MockRepository.GenerateStrictMock<IContainer>();
            _provider = ContainerProvider.Instance;
            _provider.ApplicationContainer = _applicationContainer;
        }

        [TearDown]
        public void TearDown()
        {
            _applicationContainer.VerifyAllExpectations();
        }

        [Test]
        public void CreateUnitOfWorkContainer()
        {
            ILifetimeScope unitOfWorkContainer = new LifetimeScope(new ComponentRegistry());

            _applicationContainer.Expect(ac => ac.BeginLifetimeScope()).Return(unitOfWorkContainer);
            var actual = _provider.CreateUnitOfWork();

            Assert.AreNotSame(actual, _applicationContainer);
        }

        [Test]
        public void CanRegisterTypesInSeparateUnitsOfWork()
        {
            IContainerProvider provider = ContainerProvider.Instance;
            provider.Instance.ApplicationContainer = new ContainerBuilder().Build();

            var container1 = provider.CreateUnitOfWork(b => b.RegisterType<SomeClass>());
            var container2 = provider.CreateUnitOfWork();

            Assert.IsFalse(provider.ApplicationContainer.IsRegistered<SomeClass>());

            Assert.IsNotNull(container1.Resolve<SomeClass>());

            try
            {
                container2.Resolve<SomeClass>();
                Assert.Fail();
            }
            catch(ComponentNotRegisteredException)
            {
                Assert.Pass();
            }
        }

    }

    public class SomeClass
    {
    }
}