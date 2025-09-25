using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartPanel : MonoBehaviour
{
    [SerializeField]
    private Button BtnStart;

    private Image backgroundImage;
    private List<Image> stars = new List<Image>();
    private const int starCount = 20;
    private const float starFlashSpeed = 1.5f;
    private Vector2[] bigDipperPositions = new Vector2[]
    {
        new Vector2(0.2f, 0.8f),
        new Vector2(0.3f, 0.7f),
        new Vector2(0.4f, 0.6f),
        new Vector2(0.5f, 0.5f),
        new Vector2(0.6f, 0.4f),
        new Vector2(0.7f, 0.3f),
        new Vector2(0.8f, 0.2f)
    };
    private Sprite starSprite;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI subtitleText;

    private void Start()
    {
        // 设置背景图片
        backgroundImage = GetComponent<Image>();
        if (backgroundImage == null)
            backgroundImage = gameObject.AddComponent<Image>();

        Sprite bgSprite = Resources.Load<Sprite>("UI/start_bg");
        if (bgSprite != null)
        {
            backgroundImage.sprite = bgSprite;
            backgroundImage.color = Color.white; // 恢复为白色确保图片正常显示
        }
        else
        {
            backgroundImage.color = new Color(0.1f, 0.1f, 0.2f, 0.85f);
        }

        // 创建星星纹理
        CreateStarSprite();

        // 创建标题文字
        CreateTitleText();

        // 设置按钮样式
        SetupButton(BtnStart);

        // 创建星星
        CreateStars();

        BtnStart.onClick.AddListener(ClickStart);
    }

    private void CreateStarSprite()
    {
        // 尝试加载星星图片
        Sprite loadedStarSprite = Resources.Load<Sprite>("UI/star");
        if (loadedStarSprite != null)
        {
            starSprite = loadedStarSprite;
            return;
        }

        // 如果没有图片则创建默认纹理
        Texture2D tex = new Texture2D(64, 64);
        for (int x = 0; x < 64; x++)
        {
            for (int y = 0; y < 64; y++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(32, 32));
                float alpha = Mathf.Clamp01(1 - dist / 32);
                tex.SetPixel(x, y, new Color(1, 1, 1, alpha));
            }
        }
        tex.Apply();
        starSprite = Sprite.Create(tex, new Rect(0, 0, 64, 64), Vector2.zero);
    }

    private void CreateTitleText()
    {
        // 创建主标题
        GameObject textGo = new GameObject("TitleText");
        textGo.transform.SetParent(transform, false);
        titleText = textGo.AddComponent<TextMeshProUGUI>();
        titleText.text = "VR航天博物馆";

        TextMeshProUGUI btnText = BtnStart.GetComponentInChildren<TextMeshProUGUI>();
        if (btnText != null)
        {
            titleText.font = btnText.font;
            titleText.fontSize = 55; // 稍微减小字体大小
        }
        else
        {
            titleText.fontSize = 55;
        }
        titleText.color = Color.white;
        titleText.alignment = TextAlignmentOptions.Center;

        RectTransform rect = textGo.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.6f);
        rect.anchorMax = new Vector2(0.5f, 0.6f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(600, 60);

        // 创建英文副标题
        GameObject subTextGo = new GameObject("SubtitleText");
        subTextGo.transform.SetParent(transform, false);
        subtitleText = subTextGo.AddComponent<TextMeshProUGUI>();
        subtitleText.text = "VR Space Museum";

        if (btnText != null)
        {
            subtitleText.font = btnText.font;
        }
        subtitleText.fontSize = 40;
        subtitleText.color = new Color(1, 1, 1, 0.8f);
        subtitleText.alignment = TextAlignmentOptions.Center;

        RectTransform subRect = subTextGo.GetComponent<RectTransform>();
        subRect.anchorMin = new Vector2(0.5f, 0.52f);
        subRect.anchorMax = new Vector2(0.5f, 0.52f);
        subRect.pivot = new Vector2(0.5f, 0.5f);
        subRect.anchoredPosition = Vector2.zero;
        subRect.sizeDelta = new Vector2(400, 30);
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
            {"开始", "Start"},
            {"关闭", "Close"},
            {"下一步", "Next"},
            {"选择", "Select"}
        };
        return translationDict.ContainsKey(chinese) ? translationDict[chinese] : "";
    }

    private void CreateStars()
    {
        for (int i = 0; i < starCount; i++)
        {
            GameObject starGo = new GameObject("Star");
            starGo.transform.SetParent(transform, false);
            Image starImage = starGo.AddComponent<Image>();
            starImage.sprite = starSprite;
            RectTransform starRect = starGo.GetComponent<RectTransform>();
            starRect.sizeDelta = new Vector2(20, 20);

            if (i < bigDipperPositions.Length)
            {
                starRect.anchorMin = bigDipperPositions[i];
                starRect.anchorMax = bigDipperPositions[i];
            }
            else
            {
                starRect.anchorMin = new Vector2(Random.value, Random.value);
                starRect.anchorMax = starRect.anchorMin;
            }

            starRect.pivot = new Vector2(0.5f, 0.5f);
            starRect.anchoredPosition = Vector2.zero;
            starImage.color = new Color(1, 1, 1, Random.Range(0.3f, 0.7f));
            stars.Add(starImage);
        }
    }

    private void Update()
    {
        // 繁星光晕渐变效果
        for (int i = 0; i < stars.Count; i++)
        {
            Image star = stars[i];
            float alpha = 0.3f + Mathf.PingPong(Time.time * starFlashSpeed + i * 0.2f, 0.7f);
            star.color = new Color(star.color.r, star.color.g, star.color.b, alpha);

            // 添加轻微的缩放效果增强光晕感
            float scale = 0.8f + Mathf.PingPong(Time.time * starFlashSpeed * 0.5f + i * 0.3f, 0.2f);
            stars[i].transform.localScale = Vector3.one * scale;
        }
    }

    private void ClickStart()
    {
        CameraManager.Instance.ClickStart();
        gameObject.SetActive(false);
    }
}