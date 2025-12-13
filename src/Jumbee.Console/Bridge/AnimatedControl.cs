namespace Jumbee.Console;

using System;

using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using Spectre.Console;

using ConsoleGuiSize = ConsoleGUI.Space.Size;

public abstract class AnimatedControl : Control, IDisposable
{
    #region Constructors
    public AnimatedControl()
    {
        _bufferConsole = new ConsoleBuffer();
        _ansiConsole = new AnsiConsoleBuffer(_bufferConsole);
        UIUpdate.Tick += OnTick;
    }
    #endregion

    #region Indexers
    public sealed override Cell this[Position position]
    {
        get
        {
            lock (UIUpdate.Lock)
            {
                if (_bufferConsole.Buffer == null) return _emptyCell;
                if (position.X < 0 || position.X >= Size.Width || position.Y < 0 || position.Y >= Size.Height) return _emptyCell;
                return _bufferConsole.Buffer[position.X, position.Y];
            }
        }
    }
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

    public void Dispose()
    {
        UIUpdate.Tick -= OnTick;
        Stop();
    }
    
    protected sealed override void Initialize()
    {
        lock (UIUpdate.Lock)
        {
            var targetSize = MaxSize;
            if (targetSize.Width > 1000) targetSize = new ConsoleGuiSize(1000, targetSize.Height);
            if (targetSize.Height > 1000) targetSize = new ConsoleGuiSize(targetSize.Width, 1000);
            Resize(targetSize);
            _bufferConsole.Resize(Size);
            Render();
            Redraw();
        }
    }

    protected abstract void Render();

    protected void OnTick(object? sender, UIUpdateTimerEventArgs e)
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
                Render();
                Redraw();
            }
        }
    }
    #endregion

    #region Fields
    protected static readonly Cell _emptyCell = new Cell(Character.Empty);
    protected readonly ConsoleBuffer _bufferConsole;
    protected readonly AnsiConsoleBuffer _ansiConsole;
    protected int frameCount = 0;
    protected int frameIndex = 0;    
    protected DateTime lastUpdate;
    protected TimeSpan accumulated;
    protected TimeSpan interval;
    protected bool isRunning = false;
    #endregion
}
