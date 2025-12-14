namespace Jumbee.Console;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleGUI;
using ConsoleGUI.Common;
using ConsoleGUI.Controls;

public class Grid : Layout<ConsoleGUI.Controls.Grid>
{
    public Grid(ConsoleGUI.Controls.Grid layout) : base(layout) {}

    public Grid() : this(new ConsoleGUI.Controls.Grid()) {}   

    public Grid(IControl[][] controls) : this(new ConsoleGUI.Controls.Grid()
    {
        Rows = controls.Select((row, _) => new ConsoleGUI.Controls.Grid.RowDefinition(row.Select(r => r.Size.Height).Max())).ToArray(),  
        Columns = controls.Transpose().Select((col, _) => new ConsoleGUI.Controls.Grid.ColumnDefinition(col.Select(c => c.Size.Width).Max())).ToArray(), 
    })
    {
        for (int r = 0; r < controls.Length; r++)
        {
            for (int c = 0; c < controls[r].Length; c++)
            {
                this.control.AddChild(c, r, controls[r][c]);    
            }
        }
    }

    public override int Rows => control.Rows.Length;

    public override int Columns => control.Columns.Length;

    public override IControl this[int row, int column] => control.GetChild(column, row);
}
