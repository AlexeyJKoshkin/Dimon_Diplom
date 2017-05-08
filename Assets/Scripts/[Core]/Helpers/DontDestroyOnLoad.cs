using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static readonly Dictionary<int, GameObject> InstanceRef = new Dictionary<int, GameObject>();

    private void Awake()
    {
        int hash = gameObject.GetHashCode();
        bool contains = InstanceRef.ContainsKey(hash);
        if (contains)
        {
            Debug.Log(string.Format("Name {0} Hash {1} contains Don't Destroy", name, hash));
            foreach (var component in gameObject.GetComponents(typeof(Component)))
            {
                if (component.GetType() != typeof(Transform))
                {
                    DestroyObject(component);
                }
            }
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            InstanceRef.Add(hash, gameObject);
        }
    }

    private void OnDestroy()
    {
        int hash = gameObject.GetHashCode();
        bool contains = InstanceRef.ContainsKey(hash);
        if (contains)
            InstanceRef.Remove(hash);
        else
            Debug.Log("WTF " + gameObject.name);
    }
}