
public class MainSceneModel
{

    public enum PanelState
    {
        MainMenu,
        HowTo
    }

    public PanelState CurrentPanel { get; set; } = PanelState.MainMenu;
}
