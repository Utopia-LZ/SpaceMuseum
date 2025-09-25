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

        // 加载按钮图片
        Sprite normalSprite = Resources.Load<Sprite>("UI/btn_normal");
        Sprite highlightedSprite = Resources.Load<Sprite>("UI/btn_highlighted");
        Sprite pressedSprite = Resources.Load<Sprite>("UI/btn_pressed");

        if (normalSprite != null)
        {
            btnImage.sprite = normalSprite;
            btnImage.type = Image.Type.Sliced;
        }
        else
        {
            btnImage.color = new Color(0, 0.4f, 0.8f, 0.3f);
        }

        // 设置按钮状态图片
        if (highlightedSprite != null || pressedSprite != null)
        {
            SpriteState state = new SpriteState
            {
                highlightedSprite = highlightedSprite,
                pressedSprite = pressedSprite
            };
            button.spriteState = state;
        }

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