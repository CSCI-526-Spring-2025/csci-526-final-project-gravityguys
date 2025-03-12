using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject controlsScreen;
    public string levelSelectScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainScreen.SetActive(true);
        controlsScreen.SetActive(false);
    }

    public void ShowLevels()
    {
        SceneManager.LoadScene(levelSelectScene);
    }

    public void ShowControls()
    {
        mainScreen.SetActive(false);
        controlsScreen.SetActive(true);
    }

    public void BackToMain()
    {
        mainScreen.SetActive(true);
        controlsScreen.SetActive(false);
    }
}
