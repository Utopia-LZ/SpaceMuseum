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
        Icon.sprite = model.Icon;
        SetupButton();
    }

    private void SetupButton()
    {
        Button button = GetComponent<Button>();
        Image btnImage = gameObject.GetComponent<Image>();
        if (btnImage == null)
            btnImage = gameObject.AddComponent<Image>();

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
            btnText.fontSize = 33; // 适当减小字体大小
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