namespace Atlas.Installation
{
    internal class UnInstaller : BaseInstaller
    {
        public override void Install<TProcessorHost>(string name)
        {
            System.Console.WriteLine("un-installing {0}", name);
            InvokeInstallCommand<TProcessorHost>((a, s) => a.Uninstall(s));
        }
    }
}