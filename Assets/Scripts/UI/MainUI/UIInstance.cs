using GameKit.UI;
using System.Collections;
using UnityEngine;

public class UIInstance : Singleton<DiplomUI>
{
    private IEnumerator Start()
    {
        yield return new WaitWhile(() => !Instance.IsReady);
        Instance.ShowWindow(WindowType.Menu);
    }
}