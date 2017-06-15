using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EnhancedScrollerDemos.RemoteResourcesDemo
{
    public class CloseDelta : MonoBehaviour, IDragHandler
    {
        private void Awake()
        {
            CellView.OnStart += OnStart;
        }

        private float _startY;

        private void OnStart(CellView cellView)
        {
            _startY = cellView.transform.position.y;
        }

        private void OnGUI()
        {
            string info = "Count " + Input.touchCount;
            GUI.Box(new Rect(10, 10, 200, 100), info);
        }

        private void LateUpdate()
        {
            if (CellView.Current == null) return;
            if (Input.touchCount > 0)
            {
                var endPos = this.transform.position;
                endPos.y += Input.touches[0].deltaPosition.y;

                Debug.Log(Input.touches[0].deltaPosition);
                CellView.Current.transform.position = endPos;
            }

            //if (Input.touchCount == 0)
            //{
            //    if ((Mathf.Abs(CellView.Current.transform.position.y - _startY)) > CellView.Current.cellImage.rectTransform.rect.height * 2 / 3)
            //    {
            //        Debug.Log("Clean");
            //    }
            //    // else
            //    {
            //        var endPos = CellView.Current.transform.position;
            //        endPos.y = _startY;
            //        CellView.Current.transform.position = endPos;
            //    }
            //  //  Debug.Log("HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
            //    CellView.Current = null;
            //}
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (CellView.Current == null) return;
            if (Math.Abs(eventData.delta.y) > 0.1f)
            {
                var endPos = CellView.Current.transform.position;
                endPos.y += eventData.delta.y;
                CellView.Current.transform.position = endPos;
            }
        }
    }
}