using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShutEye.UI.Core
{
    public abstract class ParentContainers : SEUIContainerItem
    {
        [SerializeField]
        protected SEUIContainerItem _prefab;
    }

    public static class ContainerExtension
    {
        public static void InitDataToList<T>(this SEContainerWithViews<IContainerUI<T>> controller, IList<T> data)
        {
            bool needRebuild = controller.Count != data.Count;
            if (needRebuild)
            {
                controller.RebuildCountCells(data.Count - controller.Count);
            }

            for (var i = 0; i < data.Count; i++)
            {
                controller.GetSlotByN(i).UpdateDataView(data[i]);
            }
        }
    }

    public abstract class SEContainerWithViews<T> : ParentContainers, IContainerViews<T> where T : IContainerUI
    {
        [SerializeField]
        private bool _createNewIfNoSlot = true;

        public int Count { get { return Containers.Length; } }

        public virtual void RebuildCountCells(int delta)
        {
            if (delta > 0)
            {
                for (var i = 0; i < delta; i++)
                {
                    InstantiatePrefabCell();
                }
            }
            else
            {
                for (var i = 0; i < delta * -1; i++)
                {
                    DestroyCell();
                }
            }
        }

        protected virtual void DestroyCell()
        {
            var temp = new T[_containers.Length - 1];
            for (var i = 0; i < _containers.Length - 1; i++)
            {
                temp[i] = _containers[i];
            }
            var cell = Containers.Last();   //Последняя вьюшку предмета
            if (_choosenCell != null && ChoosenCell.Equals(cell))
                _choosenCell = default(T);
            cell.Destroy();
            _containers = temp;
        }

        public void SetContainer(T view)
        {
            view.CashedTransform.SetParent(transform);
            view.CashedTransform.localScale = Vector3.one;
            view.CashedTransform.localPosition = Vector3.zero;
            view.CellIndex = _containers.Length;
            view.CellIndetifer = "";
            PrepareContainer(view, _containers.Length);
            var temp = new T[_containers.Length + 1];

            for (var i = 0; i < _containers.Length; i++)
            {
                temp[i] = _containers[i];
            }
            temp[_containers.Length] = (T)view;
            _containers = temp;

            _containers.Last().InitUIComponent();
        }

        protected virtual void PrepareContainer(T container, int index)
        {
            container.CellIndex = index;
            container.ClickOnViewEvent += (rdcontainer, btn) =>
            {
                ClickOnCellHandler((T)rdcontainer, btn);
            };
        }

        /// <summary>
		/// Слот префаба для вьюшки
		/// </summary>
		public SEUIContainerItem PrefabSlot { get { return _prefab; } }

        public T GetSlotByN(int slotNumber)
        {
            if (Containers.Length <= slotNumber && _createNewIfNoSlot)
            {
                return (T)InstantiatePrefabCell();
            }
            return Containers.FirstOrDefault(o => o.CellIndex == slotNumber);
        }

        public virtual T GetSlotById(string id)
        {
            //string fff = string.Format("Count {0} Search {1}  IDS : ", Containers.Length, id);
            //Containers.ForEach(e =>
            //{
            //    fff += string.Format("{0} ", e.CellIndetifer);
            //});
            //Debug.Log(fff);
            var res = Containers.FirstOrDefault(o => o.CellIndetifer == id);
            if (res != null) return res;
            res = Containers.FirstOrDefault(o => o.CellIndetifer == "" || o.CellIndetifer == "0");
            if (res == null && !_createNewIfNoSlot) return default(T);
            if (res == null)
            {
                res = (T)InstantiatePrefabCell();
            }
            res.CellIndetifer = id;
            return res;
        }

        public event Action<T, PointerEventData.InputButton> OnChange
        {
            add
            {
                _onChange -= value;
                _onChange += value;
            }
            remove { _onChange -= value; }
        }

        private Action<T, PointerEventData.InputButton> _onChange;
        public T ChoosenCell { get { return _choosenCell; } }
        protected T _choosenCell;

        public T OwerMouse
        {
            get { return Containers.FirstOrDefault(o => o.IsMouseOver); }
        }

        public T[] Containers
        {
            get
            {
                if (_containers == null)
                {
                    var conts = GetComponentsInChildren<T>();
                    for (var i = 0; i < conts.Length; i++)
                    {
                        PrepareContainer(conts[i], i);
                    }
                    _containers = conts;
                }
                return _containers;
            }
        }

        protected T[] _containers = null;

        public override void ClearView()
        {
            base.ClearView();
            ClearContainers(false);
        }

        public void ClearContainers(bool destroy)
        {
            for (var i = 0; i < Containers.Length; i++)
            {
                Containers[i].ClearView();
                if (destroy)
                    Containers[i].Destroy();
            }
            if (destroy)
                _containers = null;
        }

        //[SerializeField]
        //private SAction OnClickCellEvent = new SAction<Action<TContainer, PointerEventData.InputButton>> ();

        //[SerializeField]
        //protected LAction OnClickCellLibraryHandler = new LAction<Action<TContainer, PointerEventData.InputButton>, HandlersLibrary> ("ClickHandler");

        /// <summary>
        /// Создать новый префаб
        /// </summary>
        /// <returns></returns>
        public T InstantiatePrefabCell()
        {
            var container = Instantiate(PrefabSlot).GetComponent<T>();
            SetContainer(container);
            return container;
        }

        protected override void PrepareUI(Action _onComplete)
        {
            //OnClickCellEvent.CreateDelegate ();

            //	OnClickCellLibraryHandler.CreateAction (typeof(HandlersLibrary));
            base.PrepareUI(_onComplete);
        }

        /// <summary>
        /// Обработчик клика на ячейку
        /// </summary>
        /// <param name="cell"></param>
        protected void ClickOnCellHandler(T cell, PointerEventData.InputButton btnNumber)
        {
            _choosenCell = cell;
            //Debug.Log(_onChange);
            if (_onChange != null)
                _onChange.Invoke(_choosenCell, btnNumber);
            //if (OnClickCellEvent.Sdelegate != null)
            //	((Action<TContainer, PointerEventData.InputButton>)OnClickCellEvent.Sdelegate).Invoke (cell, btnNumber);
            //        if (OnClickCellLibraryHandler.Sdelegate != null)
            //((Action<TContainer, PointerEventData.InputButton>)OnClickCellLibraryHandler.Sdelegate).Invoke (cell, btnNumber);
        }
    }
}