namespace Jumbee.Console;

using ConsoleGUI.Data;
using ConsoleGUI.Space;

public class VerticalTextLabel : CControl
{
    #region Constructors
    public VerticalTextLabel(string text, CColor? color = null)
    {
        _text = text;
        _color = color;
        Initialize();
    }
    #endregion
            
    #region Properties
    public Color? Color
    {
        get => _color;
        set
        {
            _color = value;
            Redraw();
        }
    }

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            Initialize();
        }
    }
    #endregion

    #region Indexers
    public override Cell this[Position position]
    {
        get
        {
            if (Text == null) return Character.Empty;
            if (string.IsNullOrEmpty(_text) || position.X >= 1 || position.Y >= Text.Length) return Character.Empty;
            return new Character(_text[position.Y], foreground: _color);
        }
    }
    #endregion

    #region Methods
    protected override void Initialize() => Resize(new Size(1, Text.Length));
    #endregion

    #region Fields
    private string _text = "";
    private Color? _color;
    #endregion
}
