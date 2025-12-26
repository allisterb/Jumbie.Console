namespace Jumbee.Console;

using System;
using System.Linq;
using ConsoleGUI;

public class HorizontalStackPanel : Layout<ConsoleGUI.Controls.HorizontalStackPanel>
{
    public HorizontalStackPanel(params IControl[]? controls) : base(new ConsoleGUI.Controls.HorizontalStackPanel())
    {
        if (controls != null)
        {
            foreach (var control in controls)
            {
                this.control.Add(control);
            }
        }
    }

    public void Add(params IControl[] controls)
    {
        foreach (var control in controls)
        {
            this.control.Add(control);
        }
    }

    public void Remove(params IControl[] controls)
    {
        foreach (var control in controls)
        {
            this.control.Remove(control);
        }
    }

    public override int Rows => 1;

    public override int Columns => control.Children.Count();

    public override IControl this[int row, int column]
    {
        get
        {
            if (row != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(row));
            }
            return control.Children.ElementAt(column);
        }
    }
}
