using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour
{
    public Image Icon;
    public TMP_Text Title;
    //public TMP_Text Description;
    public Vector3 Destination;

    private Model model;
    private Button Navigate;

    private void Start()
    {
        Navigate = GetComponent<Button>();
        Navigate.onClick.AddListener(OnClickNavigate);
    }

    public void Init(Model model)  //TODO 后面还要初始化贴图
    {
        this.model = model;
        Title.text = model.Name;
        Destination = model.transform.position;
    }

    private void OnClickNavigate()
    {
        //CameraManager.Instance.Camera.SetTarget(model);
        CameraManager.Instance.Generator.GenerateOne(model.Index);
        EventHandler.CallOpenSelectPanel(false);
    }
}
