using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tip : MonoBehaviour
{
    public TMP_Text Text;
    public Image BackGround;
    public bool Active;
    public string Content = "";
    private Color bgColor;
    private Color txtColor;

    private void Awake()
    {
        bgColor = BackGround.color;
        txtColor = Text.color;
        Active = false;
        gameObject.SetActive(false);
    }

    public void Show(string text = "")
    {
        Active = true;
        gameObject.SetActive(true);
        SetVisibility(1);
        if (text != "")
        {
            Text.text = text;
            Content = text;
        }
    }

    public void SetVisibility(float v)
    {
        if(v <= 0)
        {
            Active = false;
            gameObject.SetActive(false);
            return;
        }
        bgColor.a = txtColor.a = v;
        BackGround.color = bgColor;
        Text.color = txtColor;
    }
}
