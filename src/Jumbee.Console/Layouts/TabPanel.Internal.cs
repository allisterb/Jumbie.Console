namespace Jumbee.Console;

using System;
using System.Collections.Generic;
using System.Linq;

using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using ConsoleGUI.Space;

public class Tab
{    
    #region Constructors
    internal Tab(string name, IControl content, Color? inactivebgColor = null, Color? activebgColor = null)
    {
        this.inactiveBgColor = inactiveBgColor.Equals(null) ? defaultinactiveBgColor : inactiveBgColor;
        this.activeBgColor = activeBgColor.Equals(null) ? defaultactiveBgColor : activeBgColor;               
        headerBackground = new Background
        {
            Content = new Margin
            {
                Offset = new Offset(1, 0, 1, 0),
                Content = new TextBlock { Text = name }
            },
            Color = this.inactiveBgColor

        };

        Header = new Margin
        {
            Offset = new Offset(0, 0, 1, 0),
            Content = headerBackground
        };
        Content = content;

    }
    #endregion
    
    #region Properties
    public IControl Header { get; }
    public IControl Content { get; }
    #endregion

    #region Fields
    private static readonly Color defaultactiveBgColor = new Color(25, 54, 65);
    private static readonly Color defaultinactiveBgColor = new Color(65, 24, 25);
    private readonly Color activeBgColor;
    private readonly Color inactiveBgColor;
    private readonly Background headerBackground;
    #endregion

    #region Methods
    public void MarkAsActive() => headerBackground.Color = defaultactiveBgColor;
    public void MarkAsInactive() => headerBackground.Color = defaultinactiveBgColor;
    #endregion
}

public class TabPanelDockPanel : ConsoleGUI.Controls.DockPanel
{
    #region Constructors
    internal TabPanelDockPanel() : base()
    {
        Placement = ConsoleGUI.Controls.DockPanel.DockedControlPlacement.Top;
        tabsPanel = new ConsoleGUI.Controls.HorizontalStackPanel();
        DockedControl = new Background
        {
            Color = new Color(25, 25, 52),
            Content = new Boundary
            {
                MinHeight = 1,
                MaxHeight = 1,
                Content = tabsPanel
            }
        };               
    }
    #endregion

    #region Methods
    public void AddTab(string name, IControl content)
    {
        var newTab = new Tab(name, content);
        tabs.Add(newTab);
        tabsPanel.Add(newTab.Header);
        if (tabs.Count == 1)
            SelectTab(0);
    }

    public void SelectTab(int tab)
    {
        currentTab?.MarkAsInactive();
        currentTab = tabs[tab];
        currentTab.MarkAsActive();
        FillingControl = currentTab.Content;
    }

    public void OnInput(InputEvent inputEvent)
    {
        if (inputEvent.Key.Key != ConsoleKey.Tab || currentTab is null) return;
        SelectTab((tabs.IndexOf(currentTab) + 1) % tabs.Count);
        inputEvent.Handled = true;
    }
    #endregion

    #region Properties
    public int TabCount => tabs.Count;
    #endregion

    #region Indexers
    public IControl this[int t] => tabs.ElementAt(t).Content;
    #endregion

    #region Fields
    private readonly List<Tab> tabs = new List<Tab>();
    private readonly ConsoleGUI.Controls.HorizontalStackPanel tabsPanel;
    private Tab? currentTab;
    #endregion
}
