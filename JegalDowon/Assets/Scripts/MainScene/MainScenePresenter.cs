using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScenePresenter : MonoBehaviour
{

    // MVP 로 구현. Presenter는 View와 Model을 연결하는 역할. 버튼 클릭 시 모델 상태 변경, 뷰 업데이트, 씬 전환 등 처리.
    // Presenter는 

    [SerializeField] MainSceneView _view;
    [SerializeField] string _gameSceneName = "GameScene";

    private MainSceneModel _model;

    private void Awake()
    {
        _model = new MainSceneModel();
    }

    // START 버튼 클릭 시 호출
    public void OnStartClick()
    {
        SceneManager.LoadScene(_gameSceneName);
    }

    //HowTo 버튼 클릭 시 호출.
    public void OnHowToClick()
    {
        _model.CurrentPanel = MainSceneModel.PanelState.HowTo;
        _view?.ShowHowToPanel();
    }

    // HowToPanel Back 버튼 클릭 시 호출
    public void OnBackClick()
    {
        _model.CurrentPanel = MainSceneModel.PanelState.MainMenu;
        _view?.ShowMainMenu();
    }

    //Exit 버튼 클릭 시 호출
    public void OnExitClick()
    {
        Debug.Log("[MainScene] Exit 버튼 클릭");
        Application.Quit(); // 빌드 시 게임 종료
    }
}
