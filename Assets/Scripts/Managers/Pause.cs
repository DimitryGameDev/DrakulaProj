using UnityEngine;

public class Pause : SingletonBase<Pause>
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gui;
    private SceneLoader sceneLoader;

    private bool isPaused = false;
    private void Awake()
    {
        Init();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) HidePauseMenu();
            else ShowPauseMenu();
        }
    }

    private void Start()
    {
        sceneLoader = SceneLoader.Instance;
        HidePauseMenu();
    }

    private void ShowPauseMenu()
    {
        CharacterInputController.Instance.enabled = false;
        OnePersonCamera.Instance.Lock();
        Time.timeScale = 0f;
        gui.SetActive(false);
        pauseMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Continue()
    {
        HidePauseMenu();
    }

    public void ReturnToMainMenu()
    {
        HidePauseMenu();
        sceneLoader.MainMenu();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadLastSavedGame()
    {
        HidePauseMenu();
        sceneLoader.LoadGame();
    }
    
    private void HidePauseMenu()
    {
        CharacterInputController.Instance.enabled = true;
        OnePersonCamera.Instance.UnLock();
        Time.timeScale = 1f;
        gui.SetActive(true);
        pauseMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    
}
