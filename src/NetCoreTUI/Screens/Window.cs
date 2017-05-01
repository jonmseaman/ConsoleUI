using System.Collections;
using System.Collections.Generic;

namespace NetCoreTUI.Screens
{
    public class Window : ICollection<Page>
    {
        private IList<Page> _list = new List<Page>();

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

        public virtual Page this[int index]
        {
            get
            {
                return _list[index];
            }
        }

        public void Add(Page item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(Page item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(Page[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Page> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public bool Remove(Page item)
        {
            return _list.Remove(item);
        }

        public void Show(int index)
        {
            if (index > 0)
                this[index - 1].Exit();

            this[index].Show();
        }

        public void Hide(int index)
        {
            this[index].Hide();
        }
    }
}