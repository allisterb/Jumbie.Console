namespace Jumbee.Console;

using ConsoleGUI;
using System;


public class TabPanel : Layout<TabPanelDockPanel>
{
    public TabPanel(params (string, IControl)[] controls) : base(new TabPanelDockPanel()) {
        foreach (var (tabname, tabcontrol) in controls)
        {
            this.control.AddTab(tabname, tabcontrol);
        }
    }
            
    public override int Rows { get; } = 1;

    public override int Columns => this.control.TabCount;

    public override IControl this[int row, int column]
    {
        get
        {
            if (row != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(row));                
            }
            return control[column];
        }
    }
}

