namespace Jumbee.Console;

using System;
using System.Linq;
using Spectre.Console;

public class Spinner : AnimatedControl
{
    #region Properties
    public Spectre.Console.Spinner SpinnerType
    {
        get => _spinner;
        set
        {
            _spinner = value;
            frameCount = _spinner.Frames.Count;
            interval = _spinner.Interval.Ticks;
            spinnerFrames = _spinner.Frames.Select(Markup.Escape).ToArray();
        }
    }

    public Style Style
    {
        get => _style;
        set
        {
            _style = value;
            styleMarkup = _style.ToMarkup();
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

    #region Methods
    protected sealed override void Render()
    {
        if (Size.Width <= 0 || Size.Height <= 0) return;

        ansiConsole.Clear(true);
        var frame = spinnerFrames[frameIndex % spinnerFrames.Length];
        var frameMarkup = $"[{styleMarkup}]{frame}[/]";
        ansiConsole.Markup(frameMarkup);
        if (!string.IsNullOrEmpty(_text))
        {
            ansiConsole.Write(" ");
            ansiConsole.Markup(_text);
        }   
    }
    #endregion

    #region Fields
    private Spectre.Console.Spinner _spinner = Spectre.Console.Spinner.Known.Default;
    private Style _style = Style.Plain;
    private string styleMarkup = Style.Plain.ToMarkup();
    private string[] spinnerFrames = Spectre.Console.Spinner.Known.Default.Frames.Select(Markup.Escape).ToArray();
    private string _text = string.Empty;
    #endregion
}
