namespace Atlas.Installation
{
    internal class NoOpInstaller : BaseInstaller
    {
        public override void Install<TProcessorHost>(string name)
        {
            System.Console.WriteLine("Install mode not set, not installing");
        }
    }
}