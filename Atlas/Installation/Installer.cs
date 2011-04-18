namespace Atlas.Installation
{
    internal class Installer : BaseInstaller
    {
        public override void Install<TProcessorHost>(string name)
        {
            System.Console.WriteLine("installing {0}", name);
            InvokeInstallCommand<TProcessorHost>((a, s) => a.Install(s));
        }
    }
}
