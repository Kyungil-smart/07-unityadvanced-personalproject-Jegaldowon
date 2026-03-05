using UnityEngine;

/// <summary>
/// MainScene 싱글톤 매니저. 진입점 및 전역 접근용.
/// </summary>
public class MainSceneManager : MonoBehaviour
{
    [SerializeField] MainScenePresenter _presenter;

    private static MainSceneManager _instance;
    public static MainSceneManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    /// <summary>START 버튼 클릭 시 호출.</summary>
    public void OnStartClick()
    {
        _presenter?.OnStartClick();
    }

    /// <summary>HowTo 버튼 클릭 시 호출.</summary>
    public void OnHowToClick()
    {
        _presenter?.OnHowToClick();
    }

    /// <summary>HowToPanel Back 버튼 클릭 시 호출.</summary>
    public void OnBackClick()
    {
        _presenter?.OnBackClick();
    }

    /// <summary>Exit 버튼 클릭 시 호출.</summary>
    public void OnExitClick()
    {
        _presenter?.OnExitClick();
    }
}
