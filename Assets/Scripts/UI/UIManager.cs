using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public CameraController Camera;
    public TMP_Text Fps;
    public GameObject TipPrefab;
    public Transform TipRoot;
    public DetailPanel DetailPanel;
    public GameObject HelpPanel;
    public List<WorldPanel> WorldPanels;

    private Tip[] tips = new Tip[4];
    private Dictionary<string, float> counter = new Dictionary<string, float>();
    private int cnt = 0;
    private float timer = 0;
    private bool isShowHelp = false;
    private bool firstTime = true;

    private void Awake()
    {
        Instance = this;
        for(int i = 0; i < tips.Length; i++)
        {
            tips[i] = Instantiate(TipPrefab, TipRoot).GetComponent<Tip>();
        }
    }

    private void Update()
    {
        UpdateTip();
        UpdateDetail();
        UpdateWorld();
        UpdateHelp();
        CalculateFPS();
    }

    public void ShowTip(string text)
    {
        if (counter.ContainsKey(text))
        {
            counter[text] = 1f;
        }
        else
        {
            counter.Add(text, 1f);
            for(int i = 0; i < tips.Length; i++)
            {
                if (!tips[i].Active)
                {
                    tips[i].Show(text);
                    break;
                }
            }
        }
    }

    private void UpdateTip()
    {
        for(int i = 0; i < tips.Length; i++)
        {
            if (!tips[i].Active || !counter.ContainsKey(tips[i].Content)) continue;
            float cnt = counter[tips[i].Content];
            cnt -= Time.deltaTime;
            if(cnt > 0)
            {
                if (cnt < 0.5f)
                {
                    tips[i].SetVisibility(cnt*2);
                }
                counter[tips[i].Content] = cnt;
            }
            else
            {
                counter.Remove(tips[i].Content);
                tips[i].SetVisibility(0);
            }
        }
    }

    private void UpdateDetail()
    {
        DetailPanel?.OnUpdate();
    }

    private void UpdateWorld()
    {
        foreach(WorldPanel panel in WorldPanels)
        {
            panel.OnUpdate();
            panel.SetForward(Camera.transform);
        }
    }

    private void UpdateHelp()
    {
        if (Camera.SwitchHelp)
        {
            isShowHelp = !isShowHelp;
            firstTime = false;
            HelpPanel.SetActive(isShowHelp);
        }
        if (isShowHelp) Instance.ShowTip("H to quit");
        else if(firstTime) Instance.ShowTip("H for Help");
    }

    private void CalculateFPS()
    {
        cnt++;
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            Fps.text = cnt.ToString();
            timer = 0;
            cnt = 0;
        }
    }
}
