using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene(1);
        FileHandler.Reset("PlayerState.dat");
        FileHandler.Reset("InteractiveState.dat");
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
