using UnityEngine;

public class DetailPanel : BasePanel
{
    public float ShowPlace = 960f;
    public float HidePlace = -480f;
    public RectTransform rect;

    public override void SetVisibility(float v)
    {
        Vector2 tmp = rect.anchoredPosition;
        tmp.x = Mathf.Lerp(HidePlace, ShowPlace, v);
        rect.anchoredPosition = tmp;
    }
}