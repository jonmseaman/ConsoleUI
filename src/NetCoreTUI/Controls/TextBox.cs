using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreTUI.Enums;
using NetCoreTUI.EventArgs;

namespace NetCoreTUI.Controls
{
    public class TextBox : InputControl
    {
        private int _cursorLeft;
        private int _cursorTop;
        private List<string> _displayLines;
        private bool _insert = true;
        private int _lineOffset;
        private string[] _lines;
        private int _maxLength;
        private string _originalText;
        private char _passwordCharacter = '*';
        private TextBoxType _textBoxType;

        public TextBox()
        {
            TabStop = true;
            Width = 5;
            Height = 1;
            TreatEnterKeyAsTab = true;
        }

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public int CurrentDisplayLineIndex
        {
            get
            {
                return _cursorTop + _lineOffset;
            }
        }

        public bool Insert
        {
            get
            {
                return _insert;
            }
            set
            {
                SetProperty(ref _insert, value);
            }
        }

        public string[] Lines
        {
            get
            {
                return _lines;
            }
            set
            {
                _lines = value;

                _displayLines = _lines.SplitIntoChunks(ClientWidth).ToList();
            }
        }

        public int MaxLength
        {
            get
            {
                return _maxLength;
            }
            set
            {
                SetProperty(ref _maxLength, value);
            }
        }

        public char PasswordCharacter
        {
            get
            {
                return _passwordCharacter;
            }
            set
            {
                SetProperty(ref _passwordCharacter, value);
            }
        }

        public override string Text
        {
            get
            {
                if (Lines == null)
                    return string.Empty;

                return string.Join(Environment.NewLine, Lines);
            }

            set
            {
                Lines = value.SplitIntoLines();

                CheckMaxLength();

                if (_cursorLeft > CurrentDisplayLineLength)
                    _cursorLeft = CurrentDisplayLineLength;
            }
        }

        public TextBoxType TextBoxType
        {
            get
            {
                return _textBoxType;
            }
            set
            {
                SetProperty(ref _textBoxType, value);
            }
        }

        public bool TreatEnterKeyAsTab { get; set; }

        /// <summary>
        /// Gets or sets the display line where the cursor is.
        /// </summary>
        private string CurrentDisplayLine
        {
            get
            {
                if (DisplayLines.Count == 0)
                    return string.Empty;

                return DisplayLines[CurrentDisplayLineIndex];
            }
            set
            {
                if (DisplayLines.Count == 0)
                    DisplayLines.Add(value);
                else
                    DisplayLines[CurrentDisplayLineIndex] = value;

                RebuildLines();
            }
        }

        /// <summary>
        /// Gets the length of the current display line minus any new line characters
        /// </summary>
        private int CurrentDisplayLineLength
        {
            get
            {
                return CurrentDisplayLine.Replace(Environment.NewLine, string.Empty).Length;
            }
        }

        private List<string> DisplayLines
        {
            get
            {
                return _displayLines;
            }
        }

        private bool IsCursorOnLastLine
        {
            get
            {
                return (CurrentDisplayLineIndex) == DisplayLines.Count - 1;
            }
        }

        protected override void DrawText()
        {
            if (!ShouldDraw)
                return;

            if (DisplayLines == null || DisplayLines.Count == 0)
            {
                Write(string.Empty);

                return;
            }

            if (TextBoxType != TextBoxType.Password)
            {
                var y = Y;

                for (int i = _lineOffset; i < Math.Min(_lineOffset + ClientHeight, DisplayLines.Count); i++)
                {
                    var line = DisplayLines[i];

                    if (string.IsNullOrEmpty(line))
                        Write(string.Empty);
                    else
                        Write(line.Replace(Environment.NewLine, string.Empty));

                    if (TextBoxType != TextBoxType.Multiline)
                        return;

                    Y++;

                    if (Y >= ClientHeight)
                        break;
                }

                Y = y;

                return;
            }

            Write(new string(PasswordCharacter, Text.Length));
        }

        protected override void HandlePropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MaxLength")
                CheckMaxLength();

            if (e.PropertyName == "BorderStyle" || e.PropertyName == "Width")
                RebuildLines();

            base.HandlePropertyChanged(e);
        }

        protected override void OnEnter()
        {
            Console.ForegroundColor = ForegroundColor;
            Console.BackgroundColor = BackgroundColor;

            base.OnEnter();

            SetCursorPosition();

            _originalText = Text;

            ReadKey();
        }

        protected virtual bool OnKeyPressed(ConsoleKeyInfo info)
        {
            if (KeyPressed != null)
            {
                var args = new KeyPressedEventArgs(info);

                KeyPressed(this, args);

                return args.Handled;
            }

            return false;
        }

        protected virtual void OnTextChanged()
        {
            if (_originalText == Text)
                return;

            TextChanged?.Invoke(this, new TextChangedEventArgs(_originalText, Text));

            _originalText = Text;
        }

        protected override string OnTruncateText(string text)
        {
            return text;
        }

        protected override void ReadKey()
        {
            while (HasFocus)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);

                if (OnKeyPressed(info))
                    return;

                switch (info.Key)
                {
                    case ConsoleKey.Escape:
                        {
                            Blur();

                            OnEscPressed();

                            return;
                        }
                    case ConsoleKey.Tab:
                        {
                            OnTextChanged();

                            Blur();

                            OnTabPressed(info.Modifiers.HasFlag(ConsoleModifiers.Shift));

                            return;
                        }
                    case ConsoleKey.Enter:
                        {
                            OnTextChanged();

                            if (TextBoxType == TextBoxType.Multiline)
                            {
                                ProcessKey(info);

                                break;
                            }

                            if (TreatEnterKeyAsTab)
                            {
                                Blur();

                                OnTabPressed(false);

                                return;
                            }

                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            MoveCursorLeft();

                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            MoveCursorRight();

                            break;
                        }
                    case ConsoleKey.UpArrow:
                        {
                            DecrementCursorTop();

                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            IncrementCursorTop();

                            break;
                        }
                    case ConsoleKey.End:
                        {
                            MoveToEnd();

                            break;
                        }
                    case ConsoleKey.Home:
                        {
                            MoveToHome();

                            break;
                        }
                    case ConsoleKey.Backspace:
                        {
                            Backspace();

                            break;
                        }
                    case ConsoleKey.Delete:
                        {
                            Delete();

                            break;
                        }
                    case ConsoleKey.Insert:
                        {
                            Insert = !Insert;

                            SetCursorSize();

                            break;
                        }
                    default:
                        {
                            if (char.IsControl(info.KeyChar))
                                break;

                            ProcessKey(info);

                            break;
                        }
                }
            }
        }

        private void Backspace()
        {
            if (CurrentDisplayLineLength == 0)
                return;

            // remove one character from the list of characters
            if (_cursorLeft == CurrentDisplayLineLength)
                CurrentDisplayLine = CurrentDisplayLine.LeftPart(_cursorLeft - 1);
            else
            {
                if (_cursorLeft == 0)
                {
                    if (DecrementCursorTop())
                    {
                        MoveToEnd();

                        var nextLine = DisplayLines[CurrentDisplayLineIndex + 1];
                        var newLine = CurrentDisplayLine.LeftPart(_cursorLeft - 1);

                        //DisplayLines.RemoveAt(CurrentDisplayLineIndex + 1);
                        DisplayLines[CurrentDisplayLineIndex + 1] = string.Empty;

                        CurrentDisplayLine = newLine + nextLine;
                    }
                }
                else
                {
                    CurrentDisplayLine = CurrentDisplayLine.Substring(0, _cursorLeft - 1) + CurrentDisplayLine.Substring(_cursorLeft, CurrentDisplayLine.Length - _cursorLeft);
                }
            }

            // move the cursor to the left by one character
            MoveCursorLeft();

            // redraw and repaint
            DrawText();
            Paint();
        }

        private void CheckMaxLength()
        {
            if (MaxLength == 0)
                return;

            var value = Text;

            if (!string.IsNullOrEmpty(value))
                if (value.Length > MaxLength)
                {
                    value = value.Substring(0, MaxLength);

                    Lines = value.Split(Environment.NewLine.ToCharArray());
                }
        }

        private bool DecrementCursorTop()
        {
            if (TextBoxType != TextBoxType.Multiline)
                return false;

            if (_cursorTop > 0)
            {
                _cursorTop--;

                if (_cursorLeft > CurrentDisplayLineLength)
                    _cursorLeft = CurrentDisplayLineLength;

                SetCursorPosition();

                return true;
            }

            if (_lineOffset > 0)
            {
                _lineOffset--;

                if (_cursorLeft > CurrentDisplayLineLength)
                    _cursorLeft = CurrentDisplayLineLength;

                DrawText();
                Paint();

                return true;
            }

            return false;
        }

        private void Delete()
        {
            if (CurrentDisplayLineLength == 0 & TextBoxType != TextBoxType.Multiline)
                return;

            if (_cursorLeft != CurrentDisplayLineLength)
            {
                var line = CurrentDisplayLine;

                // remove one character from the list of characters
                CurrentDisplayLine = CurrentDisplayLine.Remove(_cursorLeft, 1);
            }
            else
            {
                if (CurrentDisplayLineIndex < DisplayLines.Count)
                    DisplayLines.RemoveAt(CurrentDisplayLineIndex);
            }

            // redraw and repaint
            DrawText();
            Paint();
        }

        private void IncrementCursorTop()
        {
            if (TextBoxType != TextBoxType.Multiline)
                return;

            if (_cursorTop < ClientHeight - 1 & _cursorTop < DisplayLines.Count - 1)
            {
                _cursorTop++;

                if (_cursorLeft > CurrentDisplayLineLength)
                    _cursorLeft = CurrentDisplayLineLength;

                SetCursorPosition();
            }
            else
            {
                if (_lineOffset + ClientHeight < DisplayLines.Count - 1)
                {
                    _lineOffset++;

                    if (_cursorLeft > CurrentDisplayLineLength)
                        _cursorLeft = CurrentDisplayLineLength;

                    DrawText();
                    Paint();
                }
            }
        }

        private void MoveCursorLeft()
        {
            if (_cursorLeft > 0)
                _cursorLeft--;
            else
            if (CurrentDisplayLineIndex > 0)
            {
                DecrementCursorTop();

                _cursorLeft = CurrentDisplayLineLength;
            }

            SetCursorPosition();
        }

        private void MoveCursorRight()
        {
            if (_cursorLeft < ClientWidth - 1 & _cursorLeft < CurrentDisplayLineLength)
                _cursorLeft++;
            else
            if (TextBoxType == TextBoxType.Multiline)
            {
                if (_cursorLeft == CurrentDisplayLineLength - 1 & !IsCursorOnLastLine)
                {
                    IncrementCursorTop();

                    _cursorLeft = 0;
                }
            }

            SetCursorPosition();
        }

        private void MoveToEnd()
        {
            _cursorLeft = CurrentDisplayLineLength;

            SetCursorPosition();
        }

        private void MoveToHome()
        {
            _cursorLeft = 0;

            SetCursorPosition();
        }

        private void ProcessKey(ConsoleKeyInfo info)
        {
            if ((_cursorLeft < ClientWidth & Text.Length < MaxLength) || (_cursorLeft < ClientWidth & MaxLength == 0))
            {
                var keyValue = info.KeyChar.ToString();

                // at the end of the line so add the character
                if (_cursorLeft == CurrentDisplayLineLength)
                {
                    if (info.Key == ConsoleKey.Enter)
                        DisplayLines.Insert(CurrentDisplayLineIndex + 1, string.Empty);
                    else
                        CurrentDisplayLine = CurrentDisplayLine + keyValue;
                }
                else
                {
                    var line = CurrentDisplayLine;

                    if (Insert)
                    {
                        if (info.Key == ConsoleKey.Enter)
                        {
                            CurrentDisplayLine = line.LeftPart(_cursorLeft);
                            DisplayLines.Insert(CurrentDisplayLineIndex + 1, line.RightPart(_cursorLeft));
                        }
                        else
                            CurrentDisplayLine = CurrentDisplayLine.Insert(_cursorLeft, keyValue);
                    }
                    else
                    {
                        if (info.Key == ConsoleKey.Enter)
                        {
                            CurrentDisplayLine = line.LeftPart(_cursorLeft);
                            DisplayLines.Insert(CurrentDisplayLineIndex + 1, line.RightPart(_cursorLeft + 1));
                        }
                        else
                            CurrentDisplayLine = line.LeftPart(_cursorLeft) + keyValue + line.RightPart(_cursorLeft + 1);
                    }
                }

                if (info.Key == ConsoleKey.Enter)
                {
                    IncrementCursorTop();
                    _cursorLeft = 0;
                }
                else
                {
                    _cursorLeft++;

                    if (_cursorLeft >= ClientWidth)
                        if (TextBoxType == TextBoxType.Multiline)
                        {
                            _cursorLeft = 0;

                            if (_cursorTop < ClientHeight)
                                _cursorTop++;
                        }
                        else
                            _cursorLeft = ClientWidth;
                }

                SetCursorPosition();

                // redraw and repaint
                DrawText();
                Paint();
            }
        }

        private void RebuildLines()
        {
            Lines = DisplayLines.AssembleChunks(ClientWidth);
        }

        private void SetCursorPosition()
        {
            var offset = 0;

            var text = string.Empty;

            if (DisplayLines.Count > _cursorTop)
                text = DisplayLines[_cursorTop];

            if (TextAlign == TextAlign.Center)
                offset = (ClientWidth / 2) - (text.Length / 2);

            if (TextAlign == TextAlign.Right)
                offset = ClientWidth - text.Length - 1;

            if (_cursorTop >= ClientHeight)
                _cursorTop = ClientHeight;

            Console.CursorLeft = ClientLeft + _cursorLeft + offset;
            Console.CursorTop = ClientTop + _cursorTop;
        }

        private void SetCursorSize()
        {
            if (Insert)
                Console.CursorSize = 10;
            else
                Console.CursorSize = 100;
        }
    }
}