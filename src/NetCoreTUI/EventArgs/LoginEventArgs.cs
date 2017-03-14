namespace NetCoreTUI.EventArgs
{
    public class LoginEventArgs : System.EventArgs
    {
        private readonly string _password;
        private readonly string _username;

        public LoginEventArgs(string username, string password)
        {
            _password = password;
            _username = username;
        }

        public string FailureMessage { get; set; }
        public string Password { get { return _password; } }
        public bool Success { get; set; }
        public string Username { get { return _username; } }
    }
}