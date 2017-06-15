using GameKit;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShutEye.UI.Core
{
    public interface IContainerUI<T> : IContainerUI, IDataBinding<T>
    {
    }

    public interface IContainerUI : IComponentUI, IDataBinding
    {
        void Destroy();

        void ClearView();

        int CellIndex { get; set; }
        string CellIndetifer { get; set; }
        bool IsMouseOver { get; }

        event Action<IContainerUI, PointerEventData.InputButton> ClickOnViewEvent;
    }

    /// <summary>
    /// Базовый контайнер UI
    /// </summary>
	[Serializable]
    public abstract class SEUIContainerItem : SEComponentUI, IContainerUI, IPointerClickHandler
    {
        /// <summary>
        /// по клику на контейнер
        /// </summary>
        public event Action<IContainerUI, PointerEventData.InputButton> ClickOnViewEvent
        {
            add
            {
                _clickAction -= value;
                _clickAction += value;
            }
            remove { _clickAction -= value; }
        }

        private Action<IContainerUI, PointerEventData.InputButton> _clickAction;

        public virtual bool IsMouseOver { get; protected set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_clickAction != null)
            {
                _clickAction.Invoke(this, eventData.button);
            }
        }

        public virtual void ClearView()
        {
            _cellIdentifier = "";
        }

        public virtual void RefreshView()
        {
        }

        public string CellIndetifer
        {
            get { return _cellIdentifier; }
            set { _cellIdentifier = value; }
        }

        public int CellIndex
        {
            get { return _cellIndex; }
            set { _cellIndex = value; }
        }

        public string NameCell
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Whether the cell is active or recycled
        /// </summary>
        [NonSerialized]
        public bool active;

        /// <summary>
        /// Whether the cell is active or recycled
        /// </summary>
        [NonSerialized]
        public int dataIndex;

        /// <summary>
        /// The cellIdentifier is a unique string that allows the scroller
        /// to handle different types of cells in a single list. Each type
        /// of cell should have its own identifier
        /// </summary>
        [SerializeField, HideInInspector]
        private string _cellIdentifier;

        /// <summary>
        /// The cell index of the cell view
        /// This will differ from the dataIndex if the list is looping
        /// </summary>
        [SerializeField]
        private int _cellIndex;

        protected override void OnDestroy()
        {
            ClearView();
            base.OnDestroy();
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}