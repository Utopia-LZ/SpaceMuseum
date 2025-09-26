using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class PagePanel : BasePanel
{
    [SerializeField]
    private Button Close, Prev, Next, Confirm;
    [SerializeField]
    private Image Icon;
    private bool hasInit = false;
    private int currentIdx = 0;
    private List<GameObject> modelList;

    private void Start()
    {
        EventHandler.OnOpenPagePanel += (show) =>
        {
            gameObject.SetActive(show);
            if (show && !hasInit) Init();
        };
        gameObject.SetActive(false);
    }

    public void Init()
    {
        SetupButton(Close);
        modelList = CameraManager.Instance.Generator.Prefabs;
        Close.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
        Prev.onClick.AddListener(() =>
        {
            currentIdx--;
            currentIdx = (currentIdx + modelList.Count) % modelList.Count;
            OnSwitchPage();
        });
        Next.onClick.AddListener(() =>
        {
            currentIdx++;
            currentIdx %= modelList.Count;
            OnSwitchPage();
        });
        Confirm.onClick.AddListener(() =>
        {
            Model model = modelList[currentIdx].GetComponent<Model>();
            CameraManager.Instance.Generator.GenerateOne(model.Index);
            EventHandler.CallOpenPagePanel(false);
        });
        OnSwitchPage();
        gameObject.SetActive(true);
        hasInit = true;
    }

    private void OnSwitchPage()
    {
        Model model = modelList[currentIdx].GetComponent<Model>();
        string str = "Content/" + model.Name + "_1";
        string path = Application.dataPath + "/Resources/" + str + ".txt";
        if (File.Exists(path)) //HACK 临时保护
            str = Resources.Load<TextAsset>(str).text;
        else
            str = "样例标题\n样例正文";
        SetContent(str);
        Icon.sprite = model.Icon;
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