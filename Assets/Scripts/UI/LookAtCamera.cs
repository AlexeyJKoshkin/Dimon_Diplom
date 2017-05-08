using System.Collections;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Vector3 _look;

    [ContextMenu("SetToAllChildren")]
    private void SetToAllChildren()
    {
        for (var i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.AddComponent<LookAtCamera>();
    }

    private IEnumerator Start()
    {
        yield return new WaitWhile(() => Camera.main == null);
        yield return Set();
    }

    private IEnumerator Set()
    {
        while (Camera.main != null)
        {
            _look = transform.position - Camera.main.transform.position;
            _look.x = 0f;
            transform.rotation = Quaternion.LookRotation(_look);
            yield return null;
        }

        yield return Start();
    }
}