using System;
using Atlas;

namespace SampleLibrary
{
    public class MyService : IAmAHostedProcess
    {
        public IServiceForDoingStuff Depdenency { get; set; }

        public void Start()
        {
            Console.WriteLine("I'm starting");
            Depdenency.DoStuff("starting");
        }

        public void Stop()
        {
            Console.WriteLine("I'm stopping");
            Depdenency.DoStuff("stopping");
        }

        public void Resume()
        {
            Console.WriteLine("I'm resuming");
            Depdenency.DoStuff("resuming");
        }

        public void Pause()
        {
            Console.WriteLine("I'm pausing");
            Depdenency.DoStuff("pausing");
        }
    }
}