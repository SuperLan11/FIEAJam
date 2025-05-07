using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static MainCamera instance;
    public new Camera camera;

    public void Awake()
    {
        instance = this;
    }

    public AudioSource TurnNoise;
}