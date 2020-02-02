
  using System;
  using System.Threading.Tasks;
  using Mono.Unix;

namespace ServerUtility
{
    public class UnixExitSignal
    {
        public event EventHandler Exit;

        readonly UnixSignal[] signals = new UnixSignal[]{
        new UnixSignal(Mono.Unix.Native.Signum.SIGTERM),
        new UnixSignal(Mono.Unix.Native.Signum.SIGINT),
        new UnixSignal(Mono.Unix.Native.Signum.SIGUSR1)
        };

        public Task CurrentWait { private set; get; }

        public UnixExitSignal()
        {
            CurrentWait= Task.Factory.StartNew(() =>
            {
                // blocking call to wait for any kill signal
                UnixSignal.WaitAny(signals, -1);
                Exit?.Invoke(null, EventArgs.Empty);
            });
        }
    }
}

