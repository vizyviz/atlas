namespace Atlas
{
    internal partial class WindowsService : System.ServiceProcess.ServiceBase
    {
        private readonly IAmAHostedProcess _hostedProcess;

        public WindowsService(IAmAHostedProcess hostedProcess)
        {
            _hostedProcess = hostedProcess;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _hostedProcess.Start();
        }

        protected override void OnPause()
        {
            _hostedProcess.Pause();
        }

        protected override void OnContinue()
        {
            _hostedProcess.Resume();
        }

        protected override void OnStop()
        {
            _hostedProcess.Stop();
        }
    }
}


