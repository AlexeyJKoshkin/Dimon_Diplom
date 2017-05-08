using DG.Tweening;
using Entitas;
using ShutEye.UI.Core;
using UnityEngine;

namespace GameKit.UI
{
    /// <summary>
    /// Анимации появления и исчезновения окон. пока тут.
    /// </summary>
    public abstract partial class BaseWindow : SEComponentUI, IDataBinding<Entity>
    {
        //[HandlerMetod("Анимация окна игрок")]
        //public static void HideActionGamePlay(BaseWindow window, TweenCallback callback)
        //{
        //    if (window.State == WindowState.Closed) return;
        //    window.State = WindowState.Change;
        //    callback += ()=> window.State = WindowState.Closed;
        //    window.CashedTransform.DOBlendableMoveBy(new Vector3(0, 10, 0), window.Hidenimation).SetEase(Ease.InOutBack).OnComplete(callback);

        //}

        //[HandlerMetod("Анимация окна игрок")]
        //public static void OpenActionGameplay(BaseWindow window, TweenCallback callback)
        //{
        //    if (window.State == WindowState.Open) return;
        //    window.State = WindowState.Change;
        //    callback += () => window.State = WindowState.Open;
        //    window.CashedTransform.DOBlendableMoveBy(new Vector3(0, -10, 0), window.ShowAnimation).SetEase(Ease.InOutBack).OnComplete(callback);
        //}

        //// [HandlerMetod("Анимация")]
        //public static void HideActionHeroInventory(BaseWindow window, bool immidiate, TweenCallback callback)
        //{
        //    if (window.State == WindowState.Closed) return;
        //    //  Debug.LogWarning("Hide");
        //    // Debug.LogWarning(window.CashedTransform.position);
        //    if (immidiate)
        //    {
        //        window.CashedTransform.position = new Vector3(-window.CashedTransform.rect.width / 2, 0, 0);
        //        window.State = WindowState.Closed;
        //        if (callback != null)
        //            callback.Invoke();
        //    }
        //    window.State = WindowState.Change;
        //    callback += () => window.State = WindowState.Closed;
        //    // callback += () => Debug.LogWarning(window.CashedTransform.position);
        //    window.CashedTransform.DOMove(
        //     window.CashedTransform.position + new Vector3(-window.CashedTransform.rect.width / 2, 0, 0),
        //     window.Hidenimation).SetEase(Ease.InOutBack).OnComplete(callback);
        //}

        //// [HandlerMetod("Анимация")]
        //public static void OpenActionHeroInventory(BaseWindow window, bool immidiate, TweenCallback callback)
        //{
        //    if (window.State == WindowState.Open) return;
        //    //Debug.LogWarning("Show");
        //    //Debug.LogWarning(window.CashedTransform.position);
        //    if (immidiate)
        //    {
        //        window.CashedTransform.position = new Vector3(window.CashedTransform.rect.width / 2, 0, 0);
        //        window.State = WindowState.Open;
        //        if (callback != null)
        //            callback.Invoke();
        //    }
        //    window.State = WindowState.Change;
        //    callback += () => window.State = WindowState.Open;
        //    //callback += () => Debug.LogWarning(window.CashedTransform.position);
        //    window.CashedTransform.DOMove(
        //        window.CashedTransform.position + new Vector3(window.CashedTransform.rect.width / 2, 0, 0),
        //        window.ShowAnimation).SetEase(Ease.InOutBack).OnComplete(callback);
        //}
    }
}