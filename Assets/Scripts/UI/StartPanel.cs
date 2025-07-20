using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    [SerializeField]
    private Button BtnStart;

    private void Start()
    {
        BtnStart.onClick.AddListener(ClickStart);
    }

    private void ClickStart()
    {
        CameraManager.Instance.ClickStart();
        gameObject.SetActive(false);
    }
}
