using System;
using System.Threading;
using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Space;
using ConsoleGUI.Input;
using Jumbie.Console;
using Spectre.Console;
using Spectre.Console.Rendering;
using ConsoleGuiSize = ConsoleGUI.Space.Size;
using ConsoleGuiColor = ConsoleGUI.Data.Color;
using LayoutGrid = ConsoleGUI.Controls.Grid;

class Program
{
    static void Main(string[] args)
    {
        // Setup ConsoleGUI
        ConsoleManager.Setup();
        // Resize to a large enough area to hold our grid
        ConsoleManager.Resize(new ConsoleGuiSize(120, 40));

        // --- Spectre.Console Controls ---

        // 1. Table
        var table = new Table();
        table.Title("[bold yellow]Jumbie Console[/]");
        table.AddColumn("Library");
        table.AddColumn("Role");
        table.AddColumn("Status");
        table.AddRow("Spectre.Console", "Widgets & Styling", "[green]Integrated[/]");
        table.AddRow("ConsoleGUI", "Layout & Windowing", "[blue]Integrated[/]");
        table.AddRow("Jumbie", "The Bridge", "[bold red]Working![/]");
        table.Border(TableBorder.Rounded);

        // 2. Bar Chart
        var barChart = new BarChart()
            .Width(50)
            .Label("[green bold]Activity[/]")
            .CenterLabel()
            .AddItem("Planning", 12, Color.Yellow)
            .AddItem("Coding", 54, Color.Green)
            .AddItem("Testing", 33, Color.Red);

        // 3. Tree
        var root = new Tree("Root");
        var foo = root.AddNode("[yellow]Foo[/]");
        var bar = foo.AddNode("[blue]Bar[/]");
        bar.AddNode("Baz");
        bar.AddNode("Qux");
        var quux = root.AddNode("Quux");
        quux.AddNode("Corgi");
        
        // --- Wrap them ---
        // Note: We don't need to specify size here, SpectreWidgetControl should adapt to container.
        var tableControl = new SpectreWidgetControl(table);
        var chartControl = new SpectreWidgetControl(barChart);
        var treeControl = new SpectreWidgetControl(root);

        // --- ConsoleGUI Layout ---
        
        // Use a Grid for layout
        var grid = new LayoutGrid
        {
            Columns = new[]
            {
                new LayoutGrid.ColumnDefinition(60),
                new LayoutGrid.ColumnDefinition(50)
            },
            Rows = new[]
            {
                new LayoutGrid.RowDefinition(15),
                new LayoutGrid.RowDefinition(20)
            }
        };

        // Top Left: Table (0,0)
        grid.AddChild(0, 0, new Margin 
        { 
            Offset = new Offset(1, 1, 1, 1), 
            Content = tableControl 
        });

        // Top Right: Tree (1,0)
        grid.AddChild(1, 0, new Margin 
        { 
            Offset = new Offset(1, 1, 1, 1), 
            Content = treeControl 
        });

        // Bottom Left: Bar Chart (0,1)
        grid.AddChild(0, 1, new Margin 
        { 
            Offset = new Offset(1, 1, 1, 1), 
            Content = chartControl 
        });

        // Bottom Right: Info (1,1)
        grid.AddChild(1, 1, new Box
        {
            HorizontalContentPlacement = Box.HorizontalPlacement.Center,
            VerticalContentPlacement = Box.VerticalPlacement.Center,
            Content = new TextBlock { Text = "Spectre + ConsoleGUI = <3" }
        });

        // Main Layout with DockPanel
        var mainLayout = new DockPanel
        {
            Placement = DockPanel.DockedControlPlacement.Top,
            DockedControl = new Background
            {
                Color = new ConsoleGuiColor(0, 0, 100),
                Content = new Box
                {
                    HorizontalContentPlacement = Box.HorizontalPlacement.Center,
                    VerticalContentPlacement = Box.VerticalPlacement.Center,
                    Content = new TextBlock { Text = "Jumbie Console Advanced Demo - Grid Layout" }
                }
            },
            FillingControl = new DockPanel
            {
                Placement = DockPanel.DockedControlPlacement.Bottom,
                DockedControl = new TextBlock { Text = "Press any key to exit..." },
                FillingControl = new Background
                {
                    Color = new ConsoleGuiColor(10, 10, 10), // Dark gray background for grid
                    Content = grid 
                }
            }
        };

        ConsoleManager.Content = mainLayout;

        // Main loop
        while (true)
        {
            ConsoleManager.ReadInput([new InputListener()]);
            Thread.Sleep(50);
        }
    }
}

public class InputListener : IInputListener
{
    public void OnInput(InputEvent inputEvent) => Environment.Exit(0);    
}