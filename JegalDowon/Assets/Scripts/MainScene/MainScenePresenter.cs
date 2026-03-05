using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// MainScene MVP - Presenter. 버튼 클릭 처리 및 View/Model 업데이트.
/// </summary>
public class MainScenePresenter : MonoBehaviour
{
    [SerializeField] MainSceneView _view;
    [SerializeField] string _gameSceneName = "GameScene";

    private MainSceneModel _model;

    private void Awake()
    {
        _model = new MainSceneModel();
    }

    /// <summary>START 버튼 클릭 시 호출.</summary>
    public void OnStartClick()
    {
        SceneManager.LoadScene(_gameSceneName);
    }

    /// <summary>HowTo 버튼 클릭 시 호출.</summary>
    public void OnHowToClick()
    {
        _model.CurrentPanel = MainSceneModel.PanelState.HowTo;
        _view?.ShowHowToPanel();
    }

    /// <summary>HowToPanel Back 버튼 클릭 시 호출.</summary>
    public void OnBackClick()
    {
        _model.CurrentPanel = MainSceneModel.PanelState.MainMenu;
        _view?.ShowMainMenu();
    }

    /// <summary>Exit 버튼 클릭 시 호출.</summary>
    public void OnExitClick()
    {
        Debug.Log("[MainScene] Exit 버튼 클릭");
        // Application.Quit(); // 빌드 시 게임 종료
    }
}
