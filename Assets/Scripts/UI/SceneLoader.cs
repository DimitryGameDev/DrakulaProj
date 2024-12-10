using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonBase<SceneLoader>
{
    private void Awake()
    {
        Init();
    }

    public void NewGame()
    {
        FileHandler.Reset("PlayerState.dat");
        FileHandler.Reset("InteractiveState.dat");
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(2);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
    
    
}
