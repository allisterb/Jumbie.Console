using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleGUI;
using ConsoleGUI.Api;
using ConsoleGUI.Common;
using ConsoleGUI.Input;
namespace Jumbee.Console;

public class Layout<T> where T:Control, IDrawingContextListener
{
    public Layout(T control)
    {
        this.control = control;
    }

    public readonly T control;
}
