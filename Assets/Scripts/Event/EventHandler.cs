public static class EventHandler
{
    public delegate void SwitchSelectPanel(bool show);
    public static event SwitchSelectPanel OnOpenSelectPanel;

    public delegate void SwitchPagePanel(bool show);
    public static event SwitchPagePanel OnOpenPagePanel;


    public static void CallOpenSelectPanel(bool show)
    {
        OnOpenSelectPanel.Invoke(show);
    }

    public static void CallOpenPagePanel(bool show)
    {
        OnOpenPagePanel.Invoke(show);
    }
}
