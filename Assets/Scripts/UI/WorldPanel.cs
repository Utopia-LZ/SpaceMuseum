using UnityEngine;

public class WorldPanel : BasePanel
{
    private Color[] colors;
    public float Distance = 10f;

    private void Awake()
    {
        colors = new Color[3];
        colors[0] = Title.color;
        colors[1] = Content.color;
        colors[2] = BackGround.color;
        gameObject.SetActive(false);
    }

    public override void SetVisibility(float v)
    {
        if (v <= 0)
        {
            gameObject.SetActive(false);
            return;
        }
        for (int i = 0; i < colors.Length; i++) colors[i].a = v;
        Title.color = colors[0];
        Content.color = colors[1];
        BackGround.color = colors[2];
    }

    public void SetForward(Transform camera)
    {
        bool far = Vector3.Distance(transform.position, camera.position) > Distance;
        if (isOpen && far) Close();
        else if (!isOpen && !far) Open();
        Vector3 forward = camera.forward;
        forward.y = 0;
        transform.forward = forward;
    }
}