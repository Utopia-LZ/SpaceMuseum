public static class EventHandler
{
    public delegate void SwitchSelectPanel(bool show);
    public static event SwitchSelectPanel OnOpenSelectPanel;

    public static void CallOpenSelectPanel(bool show)
    {
        OnOpenSelectPanel.Invoke(show);
    }
}
