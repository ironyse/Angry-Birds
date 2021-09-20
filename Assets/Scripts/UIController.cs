using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField]private GameObject PauseScreen;
    [SerializeField]private Text _titleText;
    [SerializeField]private Button _continueButton;

    public void ShowPauseScreen()
    {        
        PauseScreen.SetActive(true);
    }

    public void ClosePauseScreen()
    {
        GameController.IsPaused = false;        
        PauseScreen.SetActive(false);
    }

    public void ChangeTitlePause(string text)
    {
        _titleText.text = text;
    }

    public void SetActiveContinueButton(bool active)
    {
        _continueButton.gameObject.SetActive(active);
    }

    public void RestartLevel()
    {
        ClosePauseScreen();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChangeLevel()
    {
        ClosePauseScreen();
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Main")
        {
            SceneManager.LoadScene("Secondary");
        } else
        {
            SceneManager.LoadScene("Main");
        }
    }
}
