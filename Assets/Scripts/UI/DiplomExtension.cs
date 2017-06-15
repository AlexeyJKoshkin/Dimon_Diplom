using GameKit.UI;

public static class DiplomExtension
{
    public static void BackMainMenu(this BaseWindow window)
    {
        window.HideWindow(null);
        UIInstance.Instance.GetWindow<MainMenuWindow>().ShowWindow(null);
    }
}