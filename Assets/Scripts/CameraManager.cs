using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CameraController Camera;

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
}
