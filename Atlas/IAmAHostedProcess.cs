namespace Atlas
{
    public interface IAmAHostedProcess
    {
        void Start();
        void Stop();
        void Resume();
        void Pause();
    }
}