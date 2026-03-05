using UnityEngine;

/// <summary>
/// MainScene MVP - View. 패널 표시/숨김 처리.
/// </summary>
public class MainSceneView : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject _mainPanel;   // ButtonPanel이 있는 MainPanel
    [SerializeField] GameObject _howToPanel;

    private void Start()
    {
        ShowMainMenu(); // 초기: 메인 메뉴 표시
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
