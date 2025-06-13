using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{
    public TMP_Text Title, Content;
    public Image BackGround;
    
    protected float timer = 0;
    protected bool isOpen;

    

    public virtual void Open()
    {
        gameObject.SetActive(true);
        isOpen = true;
        timer = 0;
    }

    public virtual void Close()
    {
        isOpen = false;
        timer = 1f;
    }

    public virtual void SetVisibility(float v)
    {
        
    }

    public void SetContent(string total)
    {
        string[] parts = total.Split('\n');
        SetContent(parts[0], parts[1]);
    }

    public void SetContent(string title, string content)
    {
        Title.text = title;
        Content.text = content;
    }

    public virtual void OnUpdate()
    {
        if (isOpen && timer < 1)
        {
            timer += Time.deltaTime;
            SetVisibility(timer);
        }
        else if(!isOpen && timer > 0)
        {
            timer -= Time.deltaTime;
            SetVisibility(timer);
        }
    }
}
