using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject SelectItemPrefab;
    [SerializeField]
    private Transform Root;
    [SerializeField]
    private Button Close;
    private bool hasInit = false;

    private Image backgroundImage;

    private void Start()
    {
        // 设置半透明底色
        backgroundImage = GetComponent<Image>();
        if (backgroundImage == null)
            backgroundImage = gameObject.AddComponent<Image>();
        backgroundImage.color = new Color(0.1f, 0.1f, 0.15f, 0.9f);

        EventHandler.OnOpenSelectPanel += (show) =>
        {
            gameObject.SetActive(show);
            if (show && !hasInit) Init();
        };
        gameObject.SetActive(false);
    }

    public void Init()
    {
        SetupButton(Close);

        Close.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
        List<GameObject> list = CameraManager.Instance.Generator.Prefabs;
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

    private void SetupButton(Button button)
    {
        Image btnImage = button.GetComponent<Image>();
        if (btnImage == null)
            btnImage = button.gameObject.AddComponent<Image>();

        // 科技蓝透明背景
        btnImage.color = new Color(0, 0.4f, 0.8f, 0.3f);

        // 添加高光效果
        GameObject highlight = new GameObject("Highlight");
        highlight.transform.SetParent(button.transform, false);
        Image highlightImage = highlight.AddComponent<Image>();
        highlightImage.color = new Color(1, 1, 1, 0.2f);

        RectTransform highlightRect = highlight.GetComponent<RectTransform>();
        highlightRect.anchorMin = new Vector2(0.8f, 0.8f);
        highlightRect.anchorMax = new Vector2(1, 1);
        highlightRect.pivot = new Vector2(0.5f, 0.5f);
        highlightRect.anchoredPosition = Vector2.zero;
        highlightRect.sizeDelta = Vector2.zero;

        TextMeshProUGUI btnText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (btnText != null)
        {
            btnText.fontSize = 22; // 适当减小字体大小
            string originalText = btnText.text;
            string englishText = GetEnglishTranslation(originalText);
            if (!string.IsNullOrEmpty(englishText))
                btnText.text = originalText + "\n" + englishText;
            btnText.color = Color.white;
        }
    }

    private string GetEnglishTranslation(string chinese)
    {
        Dictionary<string, string> translationDict = new Dictionary<string, string>()
        {
            {"关闭", "Close"}
        };
        return translationDict.ContainsKey(chinese) ? translationDict[chinese] : "";
    }
}