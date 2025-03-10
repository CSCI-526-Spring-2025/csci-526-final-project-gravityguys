using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public static PauseScript Instance { get; private set; }

    public GameObject pauseUI;
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
        
    }
    public void levelSelectLoad()
    {
        SceneManager.LoadScene(levelSelectScene);
    }

    public void mainMenuLoad()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

}
