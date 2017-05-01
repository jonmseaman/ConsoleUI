using System;
using NetCoreTUI.Controls;
using NetCoreTUI.Enums;

namespace NetCoreTUI.Screens
{
    public class LoadingPage : Page
    {
        private Label _label;

        private ProgressBar _progressBar;

        private Rectangle _rectangle;

        public LoadingPage(string name) : base(name)
        {
            SetupControls();
        }

        public string Message
        {
            get
            {
                return _label.Text;
            }
            set
            {
                _label.Text = value;
            }
        }

        private void SetupControls()
        {
            _label = new Label();
            _progressBar = new ProgressBar();
            _rectangle = new Rectangle();

            _rectangle.BorderStyle = BorderStyle.Double;
            _rectangle.Height = 8;
            _rectangle.Width = Width - 6;
            _rectangle.Top = Height / 2 - _rectangle.Height / 2;
            _rectangle.Left = Width / 2 - _rectangle.Width / 2;
            _rectangle.HasShadow = true;

            _label.Width = _rectangle.Width - 4;
            _label.Top = _rectangle.Top + 2;
            _label.Left = _rectangle.Left + 2;
            _label.TextAlign = TextAlign.Center;

            _progressBar.Width = _label.Width / 2;
            _progressBar.ProgressBarStyle = ProgressBarStyle.Marquee;
            _progressBar.Left = Width / 2 - _progressBar.Width / 2;
            _progressBar.Top = _label.Top + 2;
            _progressBar.BorderStyle = BorderStyle.Single;
            _progressBar.BlockColor = ConsoleColor.Green;

            Controls.Add(_rectangle, _label, _progressBar);
        }
    }
}