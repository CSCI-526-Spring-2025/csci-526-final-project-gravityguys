using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public static PauseScript Instance { get; private set; }

    public GameObject pauseUI;
    public GameObject loseUI;
    public GameObject HUD;

    public GameObject controlsScreen;

    public string levelSelectScene;
    public string mainMenuScene;
    public static bool IsGamePaused = false;
    public KeyCode pauseKey = KeyCode.Escape;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        pauseUI.SetActive(false);
        IsGamePaused = false;
    }

    void Update()
    {
        if (Input.GetKey(pauseKey))
        {
            if (!IsGamePaused)
            {
                pausePhysics();
                pauseUI.SetActive(true);
                HUD.SetActive(false);

            }
        }
    }

    public void pausePhysics()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        IsGamePaused = true;

    }


    public void unpause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        IsGamePaused = false;
        pauseUI.SetActive(false);
        loseUI.SetActive(false);
        HUD.SetActive(true);
    }
    public void levelSelectLoad()
    {
        SceneManager.LoadScene(levelSelectScene);
    }
    
    public void ShowControls()
    {
        pauseUI.SetActive(false);
        controlsScreen.SetActive(true);
    }

    public void BackToPause()
    {
        pauseUI.SetActive(true);
        controlsScreen.SetActive(false);
    }

    public void mainMenuLoad()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

}
