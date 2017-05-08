using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public abstract class BaseDevelopChooser<T> where T : class
{
    [SerializeField]
    protected Dropdown _Dropdown;

    public event Action<T> OnChangeSelect
    {
        add
        {
            _onChange -= value;
            _onChange += value;
        }
        remove { _onChange -= value; }
    }

    private Action<T> _onChange;

    public T SelectedItem
    {
        get { return _selectedItem; }
        protected set
        {
            if (SelectedItem == value) return;
            _selectedItem = value;
            if (_onChange != null)
                _onChange.Invoke(value);
        }
    }

    private T _selectedItem;

    public virtual void Init()
    {
        _Dropdown.onValueChanged.AddListener(OnChange);
    }

    protected abstract void OnChange(int index);
}