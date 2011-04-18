using System;
using Atlas.Console;

namespace Atlas.Runners
{
    internal class ConsoleRunner : MarshalByRefObject, IServiceRunner
    {
        private readonly IAmAHostedProcess _hostedProcess;
        private readonly IWrapConsole _wrapper;

        public ConsoleRunner(IAmAHostedProcess hostedProcess, IWrapConsole wrapper)
        {
            _hostedProcess = hostedProcess;
            _wrapper = wrapper;
        }

        public void Run()
        {
            _hostedProcess.Start();
            _wrapper.WriteLine("Press (P) to Pause or (ESC) to Exit...");
            Process();
        }

        private void Exit()
        {
            _hostedProcess.Stop();
        }

        private void Pause()
        {
            _wrapper.WriteLine("Pausing, Press any key to Resume...");
            _hostedProcess.Pause();
            _wrapper.ReadKey();
            Resume();
        }

        private void Process()
        {
            var key = _wrapper.ReadKey();
            if (key == ConsoleKey.P)
            {
                Pause();
            }
            else if (key == ConsoleKey.Escape)
            {
                Exit();
            }
            else
            {
                Process();
            }
        }

        private void Resume()
        {
            _wrapper.WriteLine("Resuming, press (P) to Pause or (ESC) to Exit...");
            _hostedProcess.Resume();
            Process();
        }
    }
}