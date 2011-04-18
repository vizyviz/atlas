using System;
using System.IO;
using Atlas;
using Autofac;
using SampleLibrary;

namespace SampleAppFluentConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            // all configuration settings are optional
            // var configuration = Host.Configure<MyService>(); will use all defaults.

            var configuration = Host.Configure<MyService>()
                // Autofac registrations of any dependencies that my service may have or use.
                .WithRegistrations(b => b.Register(c => new SomeService(Path.GetFullPath(Environment.CurrentDirectory))))

                // allows multiple instances of this service to run
                .AllowMultipleInstances()

                // something to do before the service is actually started
                .BeforeStart(Init)

                // can fluently name the service, display names and descriptions are optional
                .Named("TheServiceName", "Friendly Display Name", "My Service written by My Company or something")

                // can optionally pass the command line arguments into Atlas.  If no arguments are passed in, default arguments are used.
                .WithArguments(args)

                // can tell atlas that in order for my service to run I require that this other service is started
                // will attempt to start the service if it is not running, there is an overload to provide a time in seconds to wait
                .WithDependencyOnServiceNamed("MSSQLSERVER");

            // just start the configuration and away you go
            Host.Start(configuration);
        }

        private static void Init()
        {
            Console.WriteLine("I'm initializing logging, or doing other stuff before the service actually starts.");
        }
    }
}
