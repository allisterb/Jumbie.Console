using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Spectre.Console;

namespace Jumbee.Console
{
    public class Spinner : AnimatedControl
    {
        #region Properties
        public Spectre.Console.Spinner SpinnerType
        {
            get => _spinner;
            set
            {
                _spinner = value;
                frameCount = _spinner.Frames.Count;
                interval = _spinner.Interval;
            }
        }

        public Style Style
        {
            get => _style;
            set
            {
                _style = value;
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
            }
        }
        #endregion

        #region Methods
        protected override void Render()
        {
            if (Size.Width <= 0 || Size.Height <= 0) return;

            _ansiConsole.Clear(true);

            var frame = _spinner.Frames[frameIndex % _spinner.Frames.Count];
            var frameMarkup = $"[{_style.ToMarkup()}]{Markup.Escape(frame)}[/]";
            _ansiConsole.Markup(frameMarkup);

            if (!string.IsNullOrEmpty(_text))
            {
                _ansiConsole.Write(" ");
                _ansiConsole.Markup(_text);
            }   
        }
        #endregion

        #region Fields
        private Spectre.Console.Spinner _spinner = Spectre.Console.Spinner.Known.Default;
        private Style _style = Style.Plain;
        private string _text = string.Empty;
        #endregion
    }
}
