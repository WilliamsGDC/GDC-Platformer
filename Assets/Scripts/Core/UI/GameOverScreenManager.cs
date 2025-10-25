using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreenManager : MonoBehaviour 
{
    public TextMeshProUGUI pointsText;

    public void Setup(int score) 
    {
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " POINTS";
    }

    public void RetryButton()
    {
        SceneManager.LoadScene("Demo");
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
