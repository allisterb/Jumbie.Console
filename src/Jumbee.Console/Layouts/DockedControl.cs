namespace Jumbee.Console;

using System;
using ConsoleGUI;

public enum DockedControlPlacement
{
    Top,
    Right,
    Bottom,
    Left
}

public class DockedControl : Layout<ConsoleGUI.Controls.DockPanel>
{
    public DockedControl(DockedControlPlacement placement, IControl dockedControl, IControl fillingControl)
        : base(new ConsoleGUI.Controls.DockPanel())
    {
        control.Placement = placement switch
        {
            DockedControlPlacement.Top => ConsoleGUI.Controls.DockPanel.DockedControlPlacement.Top,
            DockedControlPlacement.Right => ConsoleGUI.Controls.DockPanel.DockedControlPlacement.Right,
            DockedControlPlacement.Bottom => ConsoleGUI.Controls.DockPanel.DockedControlPlacement.Bottom,
            DockedControlPlacement.Left => ConsoleGUI.Controls.DockPanel.DockedControlPlacement.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(placement), placement, null)
        };

        control.DockedControl = dockedControl;
        control.FillingControl = fillingControl;
    }

    public IControl DockedChild
    {
        get => control.DockedControl;
        set => control.DockedControl = value;
    }

    public IControl FillingChild
    {
        get => control.FillingControl;
        set => control.FillingControl = value;
    }

    public override int Rows => (control.Placement == ConsoleGUI.Controls.DockPanel.DockedControlPlacement.Top ||
                                 control.Placement == ConsoleGUI.Controls.DockPanel.DockedControlPlacement.Bottom) ? 2 : 1;

    public override int Columns => (control.Placement == ConsoleGUI.Controls.DockPanel.DockedControlPlacement.Left ||
                                    control.Placement == ConsoleGUI.Controls.DockPanel.DockedControlPlacement.Right) ? 2 : 1;

    public override IControl this[int row, int column]
    {
        get
        {
            if (row < 0 || row >= Rows) throw new ArgumentOutOfRangeException(nameof(row));
            if (column < 0 || column >= Columns) throw new ArgumentOutOfRangeException(nameof(column));

            switch (control.Placement)
            {
                case ConsoleGUI.Controls.DockPanel.DockedControlPlacement.Top:
                    return row == 0 ? control.DockedControl : control.FillingControl;
                case ConsoleGUI.Controls.DockPanel.DockedControlPlacement.Bottom:
                    return row == 0 ? control.FillingControl : control.DockedControl;
                case ConsoleGUI.Controls.DockPanel.DockedControlPlacement.Left:
                    return column == 0 ? control.DockedControl : control.FillingControl;
                case ConsoleGUI.Controls.DockPanel.DockedControlPlacement.Right:
                    return column == 0 ? control.FillingControl : control.DockedControl;
                default:
                    throw new InvalidOperationException("Invalid placement state");
            }
        }
    }
}
