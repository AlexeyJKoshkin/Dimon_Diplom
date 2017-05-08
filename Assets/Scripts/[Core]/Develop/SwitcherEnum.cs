using System.Collections.Generic;

namespace GameKit
{
    public class SwitcherState<T>
    {
        public int Count { get { return _list.Count; } }

        public bool Loop
        {
            get { return _loop; }
            set
            {
                if (_loop != value)
                {
                    if (value)
                    {
                        T current = Current;
                        SetLevePack(current);
                    }
                    _loop = value;
                }
            }
        }

        private bool _loop;

        public T Current { get { return pack.Peek(); } }

        private Queue<T> pack;
        private List<T> _list;

        public SwitcherState(T[] items, bool loop)
        {
            _list = new List<T>(items);
            pack = new Queue<T>();
            _loop = loop;
        }

        public void SetLevePack(T first)
        {
            pack.Clear();
            int g = _list.FindIndex(o => o.Equals(first));
            if (g != -1)
            {
                for (var i = g; i < _list.Count; i++)
                    pack.Enqueue(_list[i]);
                if (Loop)
                {
                    for (var i = 0; i < g; i++)
                        pack.Enqueue(_list[i]);
                }
            }
            else
            {
                pack.Enqueue(first);
                for (var i = 0; i < _list.Count; i++)
                    pack.Enqueue(_list[i]);
                _list.Add(first);
            }
        }

        public bool Next()
        {
            T prevtype = pack.Dequeue();
            if (pack.Count == 0)
            {
                if (Loop)
                {
                    SetLevePack(prevtype);
                    pack.Enqueue(pack.Dequeue());
                }
                else
                {
                    pack.Enqueue(prevtype);
                }
            }
            else
                pack.Enqueue(prevtype);
            return (prevtype.Equals(_list[_list.Count - 1]));
        }

        internal void ChangeFirstType(T namelvl)
        {
            while (!Current.Equals(namelvl))
            {
                pack.Enqueue(pack.Dequeue());
            }
        }
    }
}