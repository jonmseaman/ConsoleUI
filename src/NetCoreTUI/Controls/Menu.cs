using System;
using System.Linq;
using NetCoreTUI.Enums;

namespace NetCoreTUI.Controls
{
    public class Menu : Control
    {
        private ControlCollection<MenuItem> _menuItems;
        private bool _menuItemsHasFocus;
        private Rectangle _rectangle;

        private bool _showMenuItems;

        public Menu(IControlContainer owner)
        {
            Owner = owner;
            Height = 1;
            TabStop = true;

            FocusBackgroundColor = ConsoleColor.Black;
            FocusForegroundColor = ConsoleColor.Gray;
            ForegroundColor = ConsoleColor.Black;
            BackgroundColor = ConsoleColor.Gray;

            _rectangle = new Rectangle();
            _rectangle.Owner = owner;
        }

        public ControlCollection<MenuItem> MenuItems
        {
            get
            {
                if (_menuItems == null)
                    _menuItems = new ControlCollection<MenuItem>(Owner);

                return _menuItems;
            }
        }

        public MenuItem AddMenuItem(string text)
        {
            var item = new MenuItem(Owner);
            item.Text = text;

            MenuItems.Add(item);

            return item;
        }

        public MenuItem AddSeparator()
        {
            var item = new MenuItem(Owner);
            item.IsSeparator = true;

            MenuItems.Add(item);

            return item;
        }

        protected override void DrawBackground()
        {
            base.DrawBackground();

            if (!_showMenuItems)
                return;

            if (MenuItems.Count == 0)
                return;

            // get the maximum length of menu items text
            var maxLength = MenuItems.Where(p => !p.IsSeparator).Where(p => !string.IsNullOrEmpty(p.Text)).Max(p => p.Text.Length);

            // add two characters for borders
            maxLength += 2;

            // menu must be at least 15 characters wide
            var width = Math.Max(15,  maxLength);

            _rectangle.SuspendLayout();

            _rectangle.Left = Left;
            _rectangle.Top = Top + 1;
            _rectangle.Height = MenuItems.Count + 2;
            _rectangle.Width = width;
            _rectangle.HasShadow = true;
            _rectangle.BorderStyle = BorderStyle.Single;

            PreserveArea(_rectangle.Left, _rectangle.Top, _rectangle.Height + 1, _rectangle.Width + 1);

            _rectangle.ResumeLayout();
            _rectangle.Draw();

            DrawMenuItems();
        }

        protected override void OnEnter()
        {
            Console.CursorVisible = false;

            base.OnEnter();

            _showMenuItems = true;

            Draw();
            Paint();

            ReadKey();
        }

        protected override void OnLeave()
        {
            base.OnLeave();

            _showMenuItems = false;
            _menuItemsHasFocus = false;

            MenuItems.RemoveFocus();

            RestoreArea();

            DrawControl();
            Paint();
        }

        protected override void ReadKey()
        {
            while (HasFocus)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);

                switch (info.Key)
                {
                    case ConsoleKey.Enter:
                        {
                            if (_menuItemsHasFocus)
                            {
                                MenuItems.GetHasFocus().Select();

                                Blur();

                                OnEscPressed();

                                return;
                            }

                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            Blur();

                            OnEscPressed();

                            return;
                        }
                    case ConsoleKey.Tab:
                        {
                            if (_menuItemsHasFocus)
                            {
                                MenuItems.TabToNextControl(info.Modifiers.HasFlag(ConsoleModifiers.Shift));

                                DrawMenuItems();

                                break;
                            }
                            else
                            {
                                Blur();

                                OnTabPressed(info.Modifiers.HasFlag(ConsoleModifiers.Shift));

                                return;
                            }
                        }
                    case ConsoleKey.RightArrow:
                        {
                            Blur();

                            OnTabPressed(false);

                            return;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            Blur();

                            OnTabPressed(true);

                            return;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            if (!_menuItemsHasFocus)
                            {
                                _menuItemsHasFocus = true;

                                MenuItems.SetFocus();
                            }
                            else
                                MenuItems.TabToNextControl(false);

                            DrawMenuItems();

                            break;
                        }
                    case ConsoleKey.UpArrow:
                        {
                            if (_menuItemsHasFocus)
                            {
                                MenuItems.TabToNextControl(true);

                                DrawMenuItems();
                            }

                            break;
                        }
                }
            }
        }

        private void DrawMenuItems()
        {
            var y = _rectangle.ClientTop;
            var x = _rectangle.ClientLeft;
            var width = _rectangle.ClientWidth;

            foreach (var item in MenuItems)
            {
                item.SuspendLayout();
                item.Top = y;
                item.Left = x;
                item.Width = width;
                item.ResumeLayout();
                item.Draw();

                if (item.IsSeparator)
                {
                    Owner.Buffer.Write((short)_rectangle.Left, (short)y, (char)0x251C, item.ForegroundColor, item.BackgroundColor);
                    Owner.Buffer.Write((short)_rectangle.Right, (short)y, (char)0x2524, item.ForegroundColor, item.BackgroundColor);

                    for (int i = 1; i < _rectangle.Width - 1; i++)
                    {
                        Owner.Buffer.Write((short)_rectangle.Left + i, (short)y, (char)0x2500, item.ForegroundColor, item.BackgroundColor);
                    }
                }

                y++;
            }

            _rectangle.Paint();
        }
    }
}