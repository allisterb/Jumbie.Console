namespace Jumbee.Console;

using ConsoleGUI.Data;
using ConsoleGUI.Space;
using System.Linq;

public class VerticalTextLabel : CControl
{
    #region Constructors
    public VerticalTextLabel(string text, Color fgcolor = default, Color bgcolor = default)
    {
        _text = text;        
        _fgcolor = fgcolor;
        _bgcolor = bgcolor;
        chars = _text.Select(t => new Character(t, foreground: _fgcolor, background: _bgcolor)).ToArray();
        size = new Size(1, _text.Length);
        Initialize();
    }
    #endregion
            
    #region Properties
    public Color FgColor
    {
        get => _fgcolor;
        set
        {
            _fgcolor = value;
            chars = _text.Select(t => new Character(t, foreground: _fgcolor, background: _bgcolor)).ToArray();
            Redraw();
        }
    }

    public Color BgColor
    {
        get => _bgcolor;
        set
        {
            _bgcolor = value;
            chars = _text.Select(t => new Character(t, foreground: _fgcolor, background: _bgcolor)).ToArray();
            Redraw();
        }
    }

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            chars = _text.Select(t => new Character(t, foreground: _fgcolor, background: _bgcolor)).ToArray();
            size = new Size(1, _text.Length);
            Initialize();
        }
    }
    #endregion

    #region Indexers
    public override Cell this[Position position]
    {
        get
        {
            if (string.IsNullOrEmpty(_text) || position.X >= 1 || position.Y >= Text.Length)
            {
                return emptyCell;                
            }
            else
            {
                return chars[position.Y];
            }
        }
    }
    #endregion

    #region Methods
    protected override void Initialize() => Resize(size);    
    #endregion

    #region Fields
    private string _text = "";
    private Color _fgcolor;
    private Color _bgcolor;
    private Size size;
    private Character[] chars;
    protected static readonly Cell emptyCell = new Cell(Character.Empty);
    #endregion
}
