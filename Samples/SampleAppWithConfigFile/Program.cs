using System;
using System.IO;
using Atlas;
using Autofac;
using SampleLibrary;

namespace SampleAppWithConfigFile
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuraiton = Host.UseAppConfig<MyService>()
                // can optionally override any app config settings by using fluent config
                .WithRegistrations(b => b.Register(c => new SomeService(Path.GetFullPath(Environment.CurrentDirectory))))
                // optionally pass in arguments.
                // All runtime settings passed in via arguments override any settings in app config.
                // i.e. username and password, runmode, etc.
                .WithArguments(args);

            Host.Start(configuraiton);
        }
    }
}
