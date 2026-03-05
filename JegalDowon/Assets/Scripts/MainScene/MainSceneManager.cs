using UnityEngine;

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

    // START 버튼 클릭 시 호출.
    public void OnStartClick()
    {
        _presenter?.OnStartClick();
    }

    // HowTo 버튼 클릭 시 호출.
    public void OnHowToClick()
    {
        _presenter?.OnHowToClick();
    }

    // HowToPanel Back 버튼 클릭 시 호출
    public void OnBackClick()
    {
        _presenter?.OnBackClick();
    }

    // Exit 버튼 클릭 시 호출.
    public void OnExitClick()
    {
        _presenter?.OnExitClick();
    }
}
