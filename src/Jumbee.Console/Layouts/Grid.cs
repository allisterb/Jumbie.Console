namespace Jumbee.Console;

using System;
using System.Collections.Generic;
using System.Linq;

using ConsoleGUI;

public class Grid : Layout<ConsoleGUI.Controls.Grid>
{
    #region Constructors
    public Grid(ConsoleGUI.Controls.Grid layout) : base(layout) 
    {
        this.layout = layout;
    }

    public Grid(int[] rowHeights, int[] columnWidths, IControl[][]? controls = null) : this(new ConsoleGUI.Controls.Grid())
    {                
        layout.Rows = rowHeights.Select(h => new ConsoleGUI.Controls.Grid.RowDefinition(h)).ToArray();
        layout.Columns = columnWidths.Select(w => new ConsoleGUI.Controls.Grid.ColumnDefinition(w)).ToArray();
        if (controls is not null)
        {
            if (controls.Length != rowHeights.Length)
            {
                throw new ArgumentException("Number of control rows must match number of row heights.");
            }
            if (controls.Any(r => r.Length != columnWidths.Length))
            {
                throw new ArgumentException("Number of control columns must match number of column widths.");
            }   
            for (int r = 0; r < controls.Length; r++)
            {
                for (int c = 0; c < controls[r].Length; c++)
                {
                    this.control.AddChild(c, r, controls[r][c]);
                }
            }
        }
    }
    #endregion

    #region Methods
    public void SetChild(int row, int column, IControl child) => control.AddChild(column, row, child);
        
    public override int Rows => control.Rows.Length;

    public override int Columns => control.Columns.Length;

    public override IControl this[int row, int column] => control.GetChild(column, row);
    #endregion

    #region Fields
    protected readonly ConsoleGUI.Controls.Grid layout;
    #endregion
}
