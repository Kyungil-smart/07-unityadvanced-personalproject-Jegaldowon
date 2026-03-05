/// <summary>
/// MainScene MVP - Model. 현재 표시 중인 패널 상태.
/// </summary>
public class MainSceneModel
{
    public enum PanelState
    {
        MainMenu,
        HowTo
    }

    public PanelState CurrentPanel { get; set; } = PanelState.MainMenu;
}
