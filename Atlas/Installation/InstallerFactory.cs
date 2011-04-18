using Atlas.Configuration;

namespace Atlas.Installation
{
    internal class InstallerFactory : IInstallerFactory
    {
        public IInstaller Create<TProcessorHost>(Configuration<TProcessorHost> configuration)
        {
            BaseInstaller baseInstaller;
            switch (configuration.InstallMode)
            {
                case InstallMode.Install:
                case InstallMode.InstallAndStart:
                    baseInstaller = new Installer();
                    break;
                case InstallMode.Uninstall:
                    baseInstaller = new UnInstaller();
                    break;
                default:
                    baseInstaller = new NoOpInstaller();
                    break;
            }

            baseInstaller.Account = configuration.Account;
            baseInstaller.ServiceName = configuration.Name;
            baseInstaller.DisplayName = configuration.Name;
            baseInstaller.Description = configuration.Description;
            baseInstaller.StartType = configuration.StartMode;
            baseInstaller.Username = configuration.UserName;
            baseInstaller.Password = configuration.Password;
            return baseInstaller;
        }
    }
}