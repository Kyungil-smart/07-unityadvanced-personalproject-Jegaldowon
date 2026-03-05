using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditPanelExit : MonoBehaviour
{
    public void OnClickExit()
    {
        SceneManager.LoadScene("MainScene");
    }
}