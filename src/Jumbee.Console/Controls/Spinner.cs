namespace Jumbee.Console;

using System;
using System.Threading;

using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using Spectre.Console;

using ConsoleGuiSize = ConsoleGUI.Space.Size;

public class Spinner : Control, IDisposable
{    
    #region Constructors
    public Spinner()
    {
        _bufferConsole = new ConsoleBuffer();
        _ansiConsole = new AnsiConsoleBuffer(_bufferConsole);
        UIUpdate.Tick += OnTick;

    }
    #endregion

    #region Properties
    public Spectre.Console.Spinner SpinnerType 
    { 
        get => _spinner; 
        set 
        {
            _spinner = value; 
        }
    }
    
    public Style Style
    {
        get => _style;
        set
        {
            _style = value;
        }
    }

    public string Text
    {
        get => _text;
        set
        {           
            _text = value;         
        }
    }
    #endregion

    #region Indexers
    public override Cell this[Position position]
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
        if (_isRunning) return;
        _isRunning = true;
        _lastUpdate = DateTime.UtcNow;
        _accumulated = TimeSpan.Zero;           
    }
    
    public void Stop()
    {      
        if (!_isRunning) return;
        _isRunning = false;                     
    }

    public void Dispose()
    {
        UIUpdate.Tick -= OnTick;
        Stop();
    }

    private void OnTick(object? sender, UIUpdateTimerEventArgs e)
    {
        lock (e.Lock)
        {
            if (!_isRunning) return;
            
            var now = DateTime.UtcNow;
            var delta = now - _lastUpdate;
            _lastUpdate = now;            
            _accumulated += delta;            
            if (_accumulated >= _spinner.Interval)
            {
                _accumulated = TimeSpan.Zero;
                _frameIndex = (_frameIndex + 1) % _spinner.Frames.Count;
                Render();
            }
        }
    }
    protected override void Initialize()
    {
        lock (UIUpdate.Lock)
        {
            var targetSize = MaxSize;
            if (targetSize.Width > 1000) targetSize = new ConsoleGuiSize(1000, targetSize.Height);
            if (targetSize.Height > 1000) targetSize = new ConsoleGuiSize(targetSize.Width, 1000);
            Resize(targetSize);
            _bufferConsole.Resize(Size);
            Render();
        }
    }


    private void Render()
    {
        if (Size.Width <= 0 || Size.Height <= 0) return;

        _ansiConsole.Clear(true);

        var frame = _spinner.Frames[_frameIndex % _spinner.Frames.Count];
        var frameMarkup = $"[{_style.ToMarkup()}]{Markup.Escape(frame)}[/]";
        _ansiConsole.Markup(frameMarkup);

        if (!string.IsNullOrEmpty(_text))
        {
            _ansiConsole.Write(" ");
            _ansiConsole.Markup(_text);
        }

        Redraw();
    }
    #endregion

    #region Fields
    private static readonly Cell _emptyCell = new Cell(Character.Empty);
    private readonly ConsoleBuffer _bufferConsole;
    private readonly AnsiConsoleBuffer _ansiConsole;

    private Spectre.Console.Spinner _spinner = Spectre.Console.Spinner.Known.Default;
    private Style _style = Style.Plain;
    private string _text = string.Empty;

    private int _frameIndex;
    private DateTime _lastUpdate;
    private TimeSpan _accumulated;

    private bool _isRunning;
    #endregion
}
