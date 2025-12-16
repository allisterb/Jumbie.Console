namespace Jumbee.Console;

using System;

using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using Spectre.Console;

using ConsoleGuiSize = ConsoleGUI.Space.Size;

public abstract class AnimatedControl : Control
{
    #region Constructors
    public AnimatedControl() : base() {}
    #endregion
   
    #region Methods
    public void Start()
    {
        if (isRunning) return;
        isRunning = true;
        lastUpdate = DateTime.UtcNow;
        accumulated = TimeSpan.Zero;
    }

    public void Stop()
    {
        if (!isRunning) return;
        isRunning = false;
    }

    public override void Dispose()
    {
        base.Dispose();
        Stop();
    }
        
    protected sealed override void OnPaint(object? sender, UI.PaintEventArgs e)
    {
        lock (e.Lock)
        {
            if (!isRunning) return;

            var now = DateTime.UtcNow;
            var delta = now - lastUpdate;
            lastUpdate = now;
            accumulated += delta;
            if (accumulated >= interval)
            {
                accumulated = TimeSpan.Zero;
                frameIndex = (frameIndex + 1) % frameCount;
                Paint();
            }
        }
    }
    #endregion

    #region Fields
    protected int frameCount = 0;
    protected int frameIndex = 0;    
    protected DateTime lastUpdate;
    protected TimeSpan accumulated;
    protected TimeSpan interval;
    protected bool isRunning = false;
    #endregion
}
