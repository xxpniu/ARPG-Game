
using System;
using System.Threading.Tasks;

using Mono.Unix;

namespace ServerUtility
{
    public class UnixExitSignal 
    {
        public event EventHandler Exit;

        UnixSignal[] signals = new UnixSignal[]{
        new UnixSignal(Mono.Unix.Native.Signum.SIGTERM),
        new UnixSignal(Mono.Unix.Native.Signum.SIGINT),
        new UnixSignal(Mono.Unix.Native.Signum.SIGUSR1)
    };

        public UnixExitSignal()
        {
            Task.Factory.StartNew(() =>
            {
            // blocking call to wait for any kill signal
            UnixSignal.WaitAny(signals, -1);

                if (Exit != null)
                {
                    Exit(null, EventArgs.Empty);
                }

            });
        }

    }
}
