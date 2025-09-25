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
        // 设置背景图片
        backgroundImage = GetComponent<Image>();
        if (backgroundImage == null)
            backgroundImage = gameObject.AddComponent<Image>();

        Sprite bgSprite = Resources.Load<Sprite>("UI/panel_bg");
        if (bgSprite != null)
        {
            backgroundImage.sprite = bgSprite;
            backgroundImage.color = Color.white; // 恢复为白色确保图片正常显示
        }
        else
        {
            backgroundImage.color = new Color(0.1f, 0.1f, 0.15f, 0.9f);
        }

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
            btnText.fontSize = 32; // 适当减小字体大小
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