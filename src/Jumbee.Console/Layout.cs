namespace Jumbee.Console;

using ConsoleGUI;
using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;

public abstract class Layout<T> : IControl where T:ConsoleGUI.Common.Control, IDrawingContextListener
{
    protected Layout(T control)
    {
        this.control = control;
    }

    public abstract int Rows { get; }

    public abstract int Columns { get; }    

    public abstract IControl this[int row, int column] { get; }  

    public readonly T control;

    public Cell this[Position position] => control[position];   

    public Size Size => control.Size;   

    public IDrawingContext Context
    {
        get => ((IControl) control).Context;
        set => ((IControl)control).Context = value;
    }
}
