namespace NetCoreTUI.EventArgs
{
    public class TextChangedEventArgs : System.EventArgs
    {
        private string _newText;
        private string _originalText;

        public TextChangedEventArgs(string originalText, string newText)
        {
            _originalText = originalText;
            _newText = newText;
        }

        public string NewText
        {
            get
            {
                return _newText;
            }
        }

        public string OrignalText
        {
            get
            {
                return _originalText;
            }
        }
    }
}