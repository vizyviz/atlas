using System;
using System.IO;
using System.Threading;

namespace SampleLibrary
{
    public class SomeService : IServiceForDoingStuff
    {
        private readonly string _filePath;

        public SomeService(string filePath)
        {
            _filePath = filePath;
        }

        public void DoStuff(string message)
        {
            File.AppendAllText(_filePath,
                               string.Format("{0} - DoStuff called at {1} with message {2}",
                                             Thread.CurrentThread.ManagedThreadId,
                                             DateTime.Now, message));
        }
    }
}