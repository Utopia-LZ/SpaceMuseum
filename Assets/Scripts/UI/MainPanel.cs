using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    public Button OpenSelectList;

    private void Start()
    {
        OpenSelectList.onClick.AddListener(() =>
        {
            EventHandler.CallOpenSelectPanel(true);
        });
    }
}
