using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    public Button OpenSelectList;
    public Button NextOne;

    private void Start()
    {
        OpenSelectList.onClick.AddListener(() =>
        {
            EventHandler.CallOpenSelectPanel(true);
        });
        NextOne.onClick.AddListener(() =>
        {
            CameraManager.Instance.Generator.GenerateOne();
        });
    }
}
