using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;
    public static CameraManager Instance
    {
        get
        {
            if(instance == null)
                instance = FindObjectOfType<CameraManager>();
            return instance;
        }
    }

    public CameraController Camera;
    public Generator Generator;

    private void Start()
    {
        Camera = FindObjectOfType<CameraController>();
        Generator = FindObjectOfType<Generator>();
    }

    private void Update()
    {
        if (Camera.Quit)
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    public void ClickStart()
    {
        Camera.ClickStart();
    }
}
