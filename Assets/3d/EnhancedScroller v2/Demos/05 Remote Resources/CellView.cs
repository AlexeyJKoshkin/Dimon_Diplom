using ShutEye.UI.Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EnhancedScrollerDemos.RemoteResourcesDemo
{
    public class CellView : SEButtonUI<Data>, IPointerDownHandler, IPointerUpHandler
    {
        public static CellView Current;
        public static Action<CellView> OnStart;

        public Image cellImage;
        public Sprite defaultSprite;

        public override void RefreshView()
        {
            StartCoroutine(LoadRemoteImage(CurrentData));
        }

        public IEnumerator LoadRemoteImage(Data data)
        {
            string path = data.imageUrl;
            WWW www = new WWW(path);
            yield return www;

            cellImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, data.imageDimensions.x, data.imageDimensions.y), new Vector2(0, 0), data.imageDimensions.x);
        }

        public void ClearImage()
        {
            cellImage.sprite = defaultSprite;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _startY = this.transform.position.y;

            if (OnStart != null)
                OnStart.Invoke(this);
            Current = this;
        }

        private float _startY;

        public void OnDrag(PointerEventData eventData)
        {
            if (Math.Abs(eventData.delta.y) > 0.1f)
            {
                var endPos = this.transform.position;
                endPos.y += eventData.delta.y;
                this.transform.position = endPos;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if ((Mathf.Abs(this.transform.position.y - _startY)) > this.cellImage.rectTransform.rect.height * 2 / 3)
            {
                Debug.Log("Clean");
            }
            // else
            {
                var endPos = this.transform.position;
                endPos.y = _startY;
                this.transform.position = endPos;
            }
        }
    }
}