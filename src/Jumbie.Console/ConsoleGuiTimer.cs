using System;
using System.Threading;

namespace Jumbie.Console
{
    public class ConsoleGuiTimerEventArgs : EventArgs
    {
        public object LockObject { get; }

        public ConsoleGuiTimerEventArgs(object lockObject)
        {
            LockObject = lockObject;
        }
    }

    public static class ConsoleGuiTimer
    {
        private static Timer? _timer;
        private static int _interval = 100;
        private static readonly object _internalLock = new object();
        private static bool _isRunning;

        public static readonly object AnimationLock = new object();

        public static event EventHandler<ConsoleGuiTimerEventArgs>? Tick;

        public static void Start(int intervalMs = 100)
        {
            lock (_internalLock)
            {
                if (_isRunning) return;
                _interval = intervalMs;
                _isRunning = true;
                _timer = new Timer(OnTick, null, _interval, _interval);
            }
        }

        public static void Stop()
        {
            lock (_internalLock)
            {
                _isRunning = false;
                _timer?.Dispose();
                _timer = null;
            }
        }

        private static void OnTick(object? state)
        {
            if (Monitor.TryEnter(AnimationLock))
            {
                try
                {
                    // We don't hold the lock while firing events to avoid deadlocks if subscribers take time.
                    // But we ensure the lock is *available* before firing.
                    // However, we immediately release it.
                    // The subscribers will then try to acquire it.
                }
                finally
                {
                    Monitor.Exit(AnimationLock);
                }

                Tick?.Invoke(null, new ConsoleGuiTimerEventArgs(AnimationLock));
            }
        }
    }
}