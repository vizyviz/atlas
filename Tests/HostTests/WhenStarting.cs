using System;
using System.IO;
using System.Text;
using Atlas;
using Atlas.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests.HostTests
{
    public partial class HostTests
    {
        [TestFixture]
        public class WhenStarting
        {
            private IMonitorProcesses _processMonitor;
            private StringBuilder _consoleOutput;
            private TextWriter _oldOut;
            private FakeHost _fakeHost;
            private FakeConfig _fakeConfig;
            
            [SetUp]
            public void Setup()
            {
                _processMonitor = MockRepository.GenerateDynamicMockWithRemoting<IMonitorProcesses>();
                ProcessMonitor.Instance = _processMonitor;

                _oldOut = Console.Out;

                _consoleOutput = new StringBuilder();
                Console.SetOut(new StringWriter(_consoleOutput));

                _fakeHost = new FakeHost();
                _fakeConfig = new FakeConfig(_fakeHost);
            }

            [TearDown]
            public void TearDown()
            {
                _processMonitor.VerifyAllExpectations();
                ProcessMonitor.Instance = null;
                Console.SetOut(_oldOut);
            }

            [Test]
            public void AllowMultipleInstancesNotSetExitsWhenAlreadyRunning()
            {
                ProcessIsAlreadyRunning();

                Host.Start(_fakeConfig);

                AssertReasonIsPrintedToConsole();
            }

            [Test]
            public void AllowMultipleInstancesSetDoesNotCheckIfAlreadyRunning()
            {
                DoesNotCheckIfAlreadyRunning();

                Host.Start(_fakeConfig.AllowMultipleInstances());

                AssertReasonNotPrintedToConsole();
            }

            [Test]
            public void CompilesConfigurationSetsContainerProviderInstanceResolvesAndRunsHostInstance()
            {
                Host.Start(_fakeConfig.AllowMultipleInstances());

                Assert.AreSame(_fakeConfig.CompiledContainer, ContainerProvider.Instance.ApplicationContainer);
                Assert.IsTrue(_fakeHost.Ran);
            }

            private void DoesNotCheckIfAlreadyRunning()
            {
                _processMonitor.Expect(p => p.AmIRunning).Repeat.Never();
            }

            private void AssertReasonNotPrintedToConsole()
            {
                Assert.IsFalse(_consoleOutput.ToString().EndsWith("already running, exiting this instance\r\n"));
            }

            private void AssertReasonIsPrintedToConsole()
            {
                Assert.IsTrue(_consoleOutput.ToString().EndsWith("already running, exiting this instance\r\n"));
            }

            private void ProcessIsAlreadyRunning()
            {
                _processMonitor.Expect(p => p.AmIRunning).Return(true);
            }
        }
    }
}