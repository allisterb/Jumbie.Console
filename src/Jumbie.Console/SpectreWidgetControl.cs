using System;
using ConsoleGUI.Api;
using ConsoleGUI.Common;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using Spectre.Console.Rendering;
using ConsoleGuiSize = ConsoleGUI.Space.Size;

namespace Jumbie.Console;

public class SpectreWidgetControl : Control
{
    private readonly BufferConsole _bufferConsole;
    private readonly ConsoleGuiAnsiConsole _ansiConsole;
    private IRenderable _content;

    public SpectreWidgetControl(IRenderable content)
    {
        _content = content ?? throw new ArgumentNullException(nameof(content));
        _bufferConsole = new BufferConsole();
        _ansiConsole = new ConsoleGuiAnsiConsole(_bufferConsole);
    }
    
    public IRenderable Content 
    {
        get => _content;
        set 
        {
            _content = value;
            Redraw();
        }
    }

    public override Cell this[Position position]
    {
        get
        {
            if (_bufferConsole.Buffer == null) return new Cell(Character.Empty);
            if (position.X < 0 || position.X >= Size.Width || position.Y < 0 || position.Y >= Size.Height) return new Cell(Character.Empty);
            return _bufferConsole.Buffer[position.X, position.Y];
        }
    }

    protected override void Initialize()
    {
        // Resize the control to fill the available space
        // We clip it to avoid issues if MaxSize is 'infinite' (though unlikely in this specific layout)
        var targetSize = MaxSize;
        if (targetSize.Width > 1000) targetSize = new ConsoleGuiSize(1000, targetSize.Height); 
        if (targetSize.Height > 1000) targetSize = new ConsoleGuiSize(targetSize.Width, 1000);
        
        Resize(targetSize);

        // Resize buffer
        _bufferConsole.Resize(Size);
        
        if (Size.Width <= 0 || Size.Height <= 0)
        {
            return;
        }
        
        // Render Spectre content to buffer
        _ansiConsole.Clear(true);
        // We probably want to render with the full width of the control
        // Spectre will look at the Profile.Width which comes from the IConsole.Size (BufferConsole.Size)
        _ansiConsole.Write(_content);
    }
}

internal class BufferConsole : IConsole
{
    public Cell[,]? Buffer { get; private set; }
    public ConsoleGuiSize Size { get; set; }
    public bool KeyAvailable => false;

    public void Initialize()
    {
        if (Buffer != null)
        {
            // Fill with empty/transparent cells
            for (int x = 0; x < Size.Width; x++)
            {
                for (int y = 0; y < Size.Height; y++)
                {
                    Buffer[x, y] = new Cell(Character.Empty);
                }
            }
        }
    }

    public void OnRefresh() { }

    public void Write(Position position, in Character character)
    {
        if (Buffer == null) return;
        if (position.X >= 0 && position.X < Size.Width && position.Y >= 0 && position.Y < Size.Height)
        {
            Buffer[position.X, position.Y] = new Cell(character);
        }
    }

    public ConsoleKeyInfo ReadKey() => default;
    
    public void Resize(ConsoleGuiSize size)
    {
        Size = size;
        Buffer = new Cell[size.Width, size.Height];
        Initialize();
    }
}
