using UnityEngine;

public class MainSceneView : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject _mainPanel;   // ButtonPanel이 있는 MainPanel
    [SerializeField] GameObject _howToPanel;

    private void Start()
    {
        ShowMainMenu(); // 메인 메뉴 표시
    }

    public void ShowMainMenu()
    {
        if (_mainPanel != null) _mainPanel.SetActive(true);
        if (_howToPanel != null) _howToPanel.SetActive(false);
    }

    public void ShowHowToPanel()
    {
        if (_mainPanel != null) _mainPanel.SetActive(false);
        if (_howToPanel != null) _howToPanel.SetActive(true);
    }
}
