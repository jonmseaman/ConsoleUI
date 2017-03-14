using System;
using NetCoreTUI.Controls;
using NetCoreTUI.Enums;
using NetCoreTUI.EventArgs;

namespace NetCoreTUI.Screens
{
    public class LoginPage : Page
    {
        private Button _cancelButton;
        private Label _failureLabel;
        private Button _loginButton;
        private Label _passwordLabel;
        private TextBox _passwordTextBox;
        private ProgressBar _progressBar;
        private Rectangle _rectangle;
        private Label _usernameLabel;
        private TextBox _usernameTextBox;

        public LoginPage() : this("Login Page")
        {
        }

        public LoginPage(string name) : base(name)
        {
            _usernameLabel = new Label();
            _passwordLabel = new Label();
            _usernameTextBox = new TextBox();
            _passwordTextBox = new TextBox();
            _rectangle = new Rectangle();
            _loginButton = new Button();
            _cancelButton = new Button();
            _failureLabel = new Label();
            _progressBar = new ProgressBar();

            SetupControls();

            _loginButton.Click += LoginButton_Click;
            _cancelButton.Click += CancelButton_Click;
            _cancelButton.EscPressed += CancelButton_Click;
            _loginButton.EscPressed += CancelButton_Click;
            _usernameTextBox.EscPressed += CancelButton_Click;
            _passwordTextBox.EscPressed += CancelButton_Click;
            _passwordTextBox.KeyPressed += PasswordTextBox_KeyPressed;
        }

        public event EventHandler Cancelled;

        public event EventHandler<LoginEventArgs> Login;

        public string Password
        {
            get
            {
                return _passwordTextBox.Text;
            }
            set
            {
                _passwordTextBox.Text = value;
            }
        }

        public string Username
        {
            get
            {
                return _usernameTextBox.Text;
            }
            set
            {
                _usernameTextBox.Text = value;
            }
        }

        public override void Show()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                base.Show();

                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                Show(_passwordTextBox);

                return;
            }

            Show(_loginButton);
        }

        protected void OnCancel()
        {
            Cancelled?.Invoke(this, new System.EventArgs());
        }

        protected virtual void OnLogin(LoginEventArgs args)
        {
            Login?.Invoke(this, args);
        }

        private void CancelButton_Click(object sender, System.EventArgs e)
        {
            OnCancel();
        }

        private void DoLogin()
        {
            if (!string.IsNullOrWhiteSpace(_usernameTextBox.Text))
            {
                Console.CursorVisible = false;

                _progressBar.Visible = true;

                var args = new LoginEventArgs(Username, Password);

                OnLogin(args);

                _progressBar.Visible = false;

                if (args.Success)
                    return;

                _failureLabel.Visible = true;
                _failureLabel.Text = args.FailureMessage;
            }

            Password = string.Empty;

            Draw();
            Paint();

            _passwordTextBox.Focus();
        }

        private void LoginButton_Click(object sender, System.EventArgs e)
        {
            DoLogin();
        }

        private void PasswordTextBox_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (e.Info.Key == ConsoleKey.Enter)
            {
                DoLogin();
                e.Handled = true;
            }
        }

        private void SetupControls()
        {
            var y = Height / 2;
            y--;
            y--;

            var labelWidth = 10;
            var textBoxWidth = 30;

            var x = Width / 2;
            x -= (labelWidth + textBoxWidth) / 2;

            _usernameLabel.Text = "Username:";
            _usernameLabel.Width = labelWidth;
            _usernameLabel.Top = y;
            _usernameLabel.Left = x;
            _usernameLabel.ForegroundColor = ConsoleColor.Yellow;

            _passwordLabel.Text = "Password:";
            _passwordLabel.Width = labelWidth;
            _passwordLabel.Top = y + 1;
            _passwordLabel.Left = x;
            _passwordLabel.ForegroundColor = ConsoleColor.Yellow;

            _usernameTextBox.Width = textBoxWidth;
            _usernameTextBox.Top = y;
            _usernameTextBox.Left = _usernameLabel.Left + _usernameLabel.Width;

            _passwordTextBox.TextBoxType = TextBoxType.Password;
            _passwordTextBox.Width = textBoxWidth;
            _passwordTextBox.Top = y + 1;
            _passwordTextBox.Left = _passwordLabel.Left + _passwordLabel.Width;
            _passwordTextBox.TreatEnterKeyAsTab = false;

            _rectangle.BorderStyle = BorderStyle.Double;
            _rectangle.Left = _usernameLabel.Left - 2;
            _rectangle.Top = _usernameLabel.Top - 2;
            _rectangle.Width = _usernameLabel.Width + _usernameTextBox.Width + 4;
            _rectangle.Height = 8;
            _rectangle.HasShadow = true;

            _loginButton.Text = "Login";
            _loginButton.Width = 8;
            _loginButton.Top = _passwordTextBox.Top + _passwordTextBox.Height + 1;
            _loginButton.Left = _passwordTextBox.Left;
            _loginButton.BackgroundColor = ConsoleColor.Gray;
            _loginButton.ForegroundColor = ConsoleColor.Black;
            _loginButton.TextAlign = TextAlign.Center;
            _loginButton.HasShadow = true;

            _cancelButton.Text = "Cancel";
            _cancelButton.Width = 8;
            _cancelButton.Top = _loginButton.Top;
            _cancelButton.Left = _loginButton.Left + _loginButton.Width + 1;
            _cancelButton.BackgroundColor = ConsoleColor.Gray;
            _cancelButton.ForegroundColor = ConsoleColor.Black;
            _cancelButton.TextAlign = TextAlign.Center;
            _cancelButton.HasShadow = true;

            _failureLabel.Width = Width - 4;
            _failureLabel.BorderStyle = BorderStyle.Double;
            _failureLabel.HasShadow = true;
            _failureLabel.Left = (Width / 2) - (_failureLabel.Width / 2);
            _failureLabel.Top = _rectangle.Bottom + 3;
            _failureLabel.BackgroundColor = ConsoleColor.DarkRed;
            _failureLabel.ForegroundColor = ConsoleColor.White;
            _failureLabel.Visible = false;

            _progressBar.Top = (Height / 2) - (_progressBar.Height / 2);
            _progressBar.Width = 30;
            _progressBar.Left = (Width / 2) - (_progressBar.Width / 2);
            _progressBar.BorderStyle = BorderStyle.Double;
            _progressBar.BlockColor = ConsoleColor.Green;
            _progressBar.HasShadow = true;
            _progressBar.ProgressBarStyle = ProgressBarStyle.Marquee;
            _progressBar.Visible = false;

            Controls.Add(_rectangle, _usernameLabel, _usernameTextBox, _passwordLabel, _passwordTextBox, _loginButton, _cancelButton, _failureLabel, _progressBar);
        }
    }
}