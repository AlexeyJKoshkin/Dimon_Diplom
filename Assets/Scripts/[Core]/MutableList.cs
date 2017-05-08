using System.Collections.Generic;

internal class MutableList<T> : List<T>
{
    private int _currentIndex = -1;

    public void DropEnumIndex()
    {
        _currentIndex = -1;
    }

    public bool NextItem(out T item)
    {
        if (++_currentIndex >= Count)
        {
            item = default(T);
            return false;
        }
        item = base[_currentIndex];
        return true;
    }

    public new void Remove(T item)
    {
        int idx = base.IndexOf(item);
        if (idx >= 0)
            this.RemoveAt(idx);
    }

    public new void RemoveAt(int index)
    {
        base.RemoveAt(index);
        if (index <= _currentIndex)
            _currentIndex--;
    }

    public new void Clear()
    {
        _currentIndex = -1;
        base.Clear();
    }
}