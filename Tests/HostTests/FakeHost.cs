using Atlas;
using Atlas.Configuration;

namespace Tests.HostTests
{
    public class FakeHost : Host
    {
        internal FakeHost()
            : base(null)
        {
        }

        public override void Run<THostedProcess>(Configuration<THostedProcess> configuration)
        {
            Ran = true;
        }

        public bool Ran { get; set; }
    }
}