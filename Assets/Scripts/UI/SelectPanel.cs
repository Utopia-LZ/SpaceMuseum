using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject SelectItemPrefab;
    [SerializeField]
    private Transform Root;
    [SerializeField]
    private Button Close;
    private bool hasInit = false;

    //[SerializeField] private Button Confirm;  //plan B

    private void Start()
    {
        EventHandler.OnOpenSelectPanel += (show) =>
        {
            gameObject.SetActive(show);
            if (show && !hasInit) Init();
        };
        gameObject.SetActive(false);
    }

    public void Init()
    {
        //Confirm.onClick.AddListener(OnClickConfirm);
        Close.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
        List<GameObject> list = CameraManager.Instance.Generator.ModelList;
        GameObject newGo;
        foreach (GameObject go in list)
        {
            Model model = go.GetComponent<Model>();
            newGo = Instantiate(SelectItemPrefab, Root);
            newGo.GetComponent<SelectItem>().Init(model);
        }
        gameObject.SetActive(true);
        hasInit = true;
    }
}
