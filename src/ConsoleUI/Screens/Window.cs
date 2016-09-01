using System.Collections;
using System.Collections.Generic;

namespace ConsoleUI
{
    public class Window : ICollection<Page>
    {
        private IList<Page> list = new List<Page>();

        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return list.IsReadOnly;
            }
        }

        public virtual Page this[int index]
        {
            get
            {
                return list[index];
            }
        }

        public void Add(Page item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(Page item)
        {
            return list.Contains(item);
        }

        public void CopyTo(Page[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Page> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public bool Remove(Page item)
        {
            return list.Remove(item);
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