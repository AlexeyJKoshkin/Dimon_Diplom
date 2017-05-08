using UnityEngine;

public class GameFPSSetter : MonoBehaviour
{
    public int MaxFps = 60;

    public void Awake()
    {
        Application.targetFrameRate = MaxFps;
    }
}