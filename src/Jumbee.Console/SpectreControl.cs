namespace Jumbee.Console;

using System;
using System.Threading;

using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using Spectre.Console.Rendering;

/// <summary>
/// Wraps a Spectre <see cref="IRenderable"/> control for use with ConsoleGUI layout types. 
/// </summary>
/// <remarks>
/// Uses an <see cref="AnsiConsoleBuffer"/> to render the control to a buffer.
/// Public property setters and methods that affect a control's visual state should call <see cref="Invalidate"/> to request a re-render on the next UI update tick.
/// </remarks>
/// <typeparam name="T"></typeparam>
public class SpectreControl<T> : Control where T : IRenderable
{
    #region Constructors
    public SpectreControl(T content) : base()
    {
        _content = content;

        
    }
    #endregion
    
    #region Properties
    public T Content
    {
        get => _content;
        set 
        {
            _content = value;
            Invalidate();
        }
    }
    #endregion

    #region Methods           
    /// <summary>
    /// Creates a copy of the current instance's control content.
    /// </summary>
    /// <remarks>This method is intended to be overridden in a derived class to clone the content of the current instance including all properties that can be modified by the user. 
    /// By default, it throws a <see /// cref="NotImplementedException"/>.
    /// </remarks>
    /// <returns>A new instance of type <typeparamref name="T"/> that is a copy of the current instance's content.</returns>
    /// <exception cref="NotImplementedException">Thrown if the method is not overridden in a derived class.</exception>
    protected virtual T CloneContent() => throw new NotImplementedException($"Cloning not implemented for type {typeof(T).Name}. Override CloneContent() in derived class.");
   
    /// <summary>
    /// Renders the control's content to the console buffer.
    /// </summary>
    /// <remarks>This method clears the console buffer and writes the control's content using the full width
    /// of the control.  If the control's size is invalid (width or height less than or equal to zero), the method exits
    /// without performing any rendering.
    /// This does not actually draw to the console screen, it just updates the buffer. The ConsoleGUI <see cref="ConsoleGUI.ConsoleManager"/> 
    /// handles drawing the buffer on the console screen.
    /// </remarks>
    protected sealed override void Render()
    {
        if (Size.Width <= 0 || Size.Height <= 0)
        {
            return;
        }
        
        // Render Spectre content to buffer
        ansiConsole.Clear(true);
        // We probably want to render with the full width of the control
        // Spectre will look at the Profile.Width which comes from the IConsole.Size (BufferConsole.Size)
        ansiConsole.Write(_content);        
    }
    #endregion

    #region Fields
    private T _content;
    #endregion
}
