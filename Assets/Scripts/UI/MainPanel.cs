using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainPanel : MonoBehaviour
{
    public Button OpenSelectList;
    public Button NextOne;

    private void Start()
    {
        SetupButton(OpenSelectList);
        SetupButton(NextOne);

        OpenSelectList.onClick.AddListener(() =>
        {
            EventHandler.CallOpenSelectPanel(true);
        });
        NextOne.onClick.AddListener(() =>
        {
            CameraManager.Instance.Generator.GenerateOne();
        });
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
            {"打开选择列表", "Open List"},
            {"下一个", "Next"}
        };
        return translationDict.ContainsKey(chinese) ? translationDict[chinese] : "";
    }
}