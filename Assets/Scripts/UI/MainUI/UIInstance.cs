using UnityEngine;
using System.Collections;
using GameKit.UI;

public class UIInstance : Singleton<DiplomUI>
{
    IEnumerator Start()
    {
        yield return  new WaitWhile(()=> !Instance.IsReady);
        Instance.ShowWindow(WindowType.Menu);
    }

}
