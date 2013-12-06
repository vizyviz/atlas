using System.ServiceProcess;

namespace Atlas.Installation
{
    internal interface IInstaller
    {
        void Install<TProcessorHost>(string name);
        ServiceAccount Account { get; set; }
        string Password { get; set; }
        string Username { get; set; }
        string DisplayName { get; set; }
        string Description { get; set; }
        string ServiceName { get; set; }
        ServiceStartMode StartType { get; set; }
    }
}