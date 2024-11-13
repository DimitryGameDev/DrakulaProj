using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button loadGame;

    private void Start()
    {
        if (FileHandler.HasFile("InteractiveState.dat"))loadGame.interactable = true;
        else loadGame.interactable = false;
    }
}
