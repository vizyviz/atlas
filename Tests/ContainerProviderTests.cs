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
        private IProvideUnitOfWorkContainers _provider;

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
            var actual = _provider.CreateUnitOfWorkContainer();

            Assert.AreNotSame(actual, _applicationContainer);


        }

    }
}