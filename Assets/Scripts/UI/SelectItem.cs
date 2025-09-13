using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour
{
    public Image Icon;
    public TMP_Text Title;
    public Vector3 Destination;

    private Model model;
    private Button Navigate;

    private void Start()
    {
        Navigate = GetComponent<Button>();
        Navigate.onClick.AddListener(OnClickNavigate);
    }

    public void Init(Model model)
    {
        this.model = model;
        Title.text = model.gameObject.name;
        Destination = model.transform.position;
        SetupButton();
    }

    private void SetupButton()
    {
        Button button = GetComponent<Button>();
        Image btnImage = gameObject.GetComponent<Image>();
        if (btnImage == null)
            btnImage = gameObject.AddComponent<Image>();

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
            btnText.fontSize = 20; // 适当减小字体大小
            string originalText = btnText.text;
            string englishText = GetEnglishTranslation(originalText);
            if (!string.IsNullOrEmpty(englishText))
                btnText.text = originalText + "\n" + englishText;
            btnText.color = Color.white;
        }
    }

    private string GetEnglishTranslation(string chinese)
    {
        // 可根据模型名称添加特定翻译，这里仅示例
        Dictionary<string, string> translationDict = new Dictionary<string, string>()
        {
            {"卫星", "Satellite"},
            {"火箭", "Rocket"},
            {"空间站", "Space Station"},
            {"宇航服", "Space Suit"},
            {"月球车", "Lunar Rover"}
        };
        return translationDict.ContainsKey(chinese) ? translationDict[chinese] : "";
    }

    private void OnClickNavigate()
    {
        CameraManager.Instance.Generator.GenerateOne(model.Index);
        EventHandler.CallOpenSelectPanel(false);
    }
}