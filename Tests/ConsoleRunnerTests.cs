using System;
using Atlas;
using Atlas.Console;
using Atlas.Runners;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests
{
    [TestFixture]
    public class ConsoleRunnerTests
    {
        private IAmAHostedProcess _hostedProcess;
        private IWrapConsole _wrapper;
        private MockRepository _mock;

        [SetUp]
        public void SetUp()
        {
            _mock = new MockRepository();
            _hostedProcess = _mock.StrictMock<IAmAHostedProcess>();
            _wrapper = _mock.StrictMockWithRemoting<IWrapConsole>();
        }

        [TearDown]
        public void TearDown()
        {
            _mock.VerifyAll();
        }

        [Test]
        public void Exiting()
        {
            using (_mock.Ordered())
            {
                _hostedProcess.Expect(op => op.Start());

                _wrapper.Expect(w => w.WriteLine("Press (P) to Pause or (ESC) to Exit..."));
                _wrapper.Expect(w => w.ReadKey()).Return(ConsoleKey.Escape);

                _hostedProcess.Expect(op => op.Stop());
            }
            _mock.ReplayAll();

            ConsoleRunner consoleRunner = new ConsoleRunner(_hostedProcess, _wrapper);

            consoleRunner.Run();
        }

        [Test]
        public void Pausing()
        {
            using (_mock.Ordered())
            {
                _hostedProcess.Expect(op => op.Start());
                _wrapper.Expect(w => w.WriteLine("Press (P) to Pause or (ESC) to Exit..."));
                _wrapper.Expect(w => w.ReadKey()).Return(ConsoleKey.P);
                _wrapper.Expect(w => w.WriteLine("Pausing, Press any key to Resume..."));
                _hostedProcess.Expect(op => op.Pause());
                _wrapper.Expect(w => w.ReadKey()).Return(ConsoleKey.Select);
                _wrapper.Expect(w => w.WriteLine("Resuming, press (P) to Pause or (ESC) to Exit..."));
                _hostedProcess.Expect(op => op.Resume());
                _wrapper.Expect(w => w.ReadKey()).Return(ConsoleKey.P);
                _wrapper.Expect(w => w.WriteLine("Pausing, Press any key to Resume..."));
                _hostedProcess.Expect(op => op.Pause());
                _wrapper.Expect(w => w.ReadKey()).Return(ConsoleKey.Select);
                _wrapper.Expect(w => w.WriteLine("Resuming, press (P) to Pause or (ESC) to Exit..."));
                _hostedProcess.Expect(op => op.Resume());
                _wrapper.Expect(w => w.ReadKey()).Return(ConsoleKey.Escape);
                _hostedProcess.Expect(op => op.Stop());
            }

            _mock.ReplayAll();

            ConsoleRunner consoleRunner = new ConsoleRunner(_hostedProcess, _wrapper);

            consoleRunner.Run();
        }

    }
}