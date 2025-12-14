
using ConsoleGUI;
using ConsoleGUI.Api;
using ConsoleGUI.Common;
using ConsoleGUI.Input;
using System.Runtime.CompilerServices;

namespace Jumbee.Console;

public abstract class Layout<T> where T:Control, IDrawingContextListener
{
    public Layout(T control)
    {
        this.control = control;
    }

    public abstract int Rows { get; }

    public abstract int Columns { get; }    

    public abstract IControl this[int row, int column] { get; }  

    public readonly T control;
}
