using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreTUI.Controls
{
    public class ControlCollection<T> : ICollection<T> where T : Control
    {
        private readonly IControlContainer _owner;
        private bool _exit;
        private IList<T> _list = new List<T>();
        private int _tabOrder = 0;

        public ControlCollection(IControlContainer owner)
        {
            _owner = owner;
        }

        public event EventHandler EscPressed;

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return _list.IsReadOnly;
            }
        }

        public void Add(params T[] items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Add(T item)
        {
            item.Owner = _owner;

            _list.Add(item);

            var lastControl = LastControl();

            if (lastControl != null)
            {
                item.TabOrder = lastControl.TabOrder + 1;
            }

            item.TabPressed += (s, e) =>
            {
                TabToNextControl(e.Shift);
            };

            item.Enter += (s, e) =>
            {
                foreach (var control in _list)
                {
                    control.HasFocus = control == s;

                    if (control == s)
                        _tabOrder = control.TabOrder;
                }
            };

            item.EscPressed += (s, e) =>
            {
                OnEscPressed(s, e);
            };
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public T GetHasFocus()
        {
            return _list.LastOrDefault(p => p.HasFocus);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public bool Remove(T item)
        {
            return _list.Remove(item);
        }

        internal void Exit()
        {
            _exit = true;

            RemoveFocus();
        }

        internal void RemoveFocus()
        {
            foreach (var item in _list.Where(p => p.HasFocus))
            {
                item.HasFocus = false;
            }

            _tabOrder = 0;
        }

        internal void SetFocus()
        {
            if (_exit)
                return;

            var control = _list.OrderBy(p => p.TabOrder).Where(p => p.TabStop).Where(p => p.Visible).FirstOrDefault(p => p.TabOrder >= _tabOrder);

            if (control == null)
                return;

            control.Focus();
        }

        internal void SetFocus(T control)
        {
            if (_exit)
                return;

            control.Focus();
        }

        internal void TabToNextControl(bool shift)
        {
            if (_exit)
                return;

            if (_list.Where(p => p.TabStop).Count(p => p.Visible) == 1)
                return;

            var last = LastControl();

            if (last == null)
                return;

            var lastTabOrder = last.TabOrder;

            if (shift && _tabOrder > 0)
            {
                var previous = _list.Where(p => p.TabStop).Where(p => p.Visible).Where(p => p.TabOrder < _tabOrder).OrderByDescending(p => p.TabOrder).FirstOrDefault();

                if (previous != null)
                    _tabOrder = previous.TabOrder;
                else
                    _tabOrder = last.TabOrder;
            }
            else
            if (_tabOrder < lastTabOrder)
                _tabOrder++;
            else
                _tabOrder = 0;

            SetFocus();
        }

        protected virtual void OnEscPressed(object sender, System.EventArgs e)
        {
            _tabOrder = 0;

            EscPressed?.Invoke(sender, e);
        }

        private T LastControl()
        {
            return _list.OrderBy(p => p.TabOrder).LastOrDefault(p => p.Visible);
        }
    }
}