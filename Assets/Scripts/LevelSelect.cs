using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public GameObject levelsScreen;

    public string mainMenuScene;
    public string level1Scene;
    public string level2Scene;
    public string level3Scene;
    public string level4Scene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelsScreen.SetActive(true);
    }

    public void mainMenuLoad()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void level1Load()
    {
        SceneManager.LoadScene(level1Scene);
    }
    public void level2Load()
    {
        SceneManager.LoadScene(level2Scene);
    }
    public void level3Load()
    {
        SceneManager.LoadScene(level3Scene);
    }
    public void levelWallLoad()
    {
        SceneManager.LoadScene(level4Scene);
    }
}
