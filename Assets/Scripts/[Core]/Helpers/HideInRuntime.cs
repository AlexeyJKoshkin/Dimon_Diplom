using UnityEngine;

namespace ShutEye.Helpers
{
    public class HideInRuntime : MonoBehaviour
    {
        public void Awake()
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}