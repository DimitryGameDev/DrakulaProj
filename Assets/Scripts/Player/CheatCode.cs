using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatCode : MonoBehaviour
{
    
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("Мушкет получен");
            CharacterInputController.Instance.IsRiflePickup = true;
            Character.Instance.GetComponentInChildren<Bag>().AddProjectile(10);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("Бесконечный спринт");
            CharacterInputController.Instance.ChangeSpeedTime(1000);
            CharacterInputController.Instance.isSprinting = true;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            Debug.Log("Отмычки получены");
            Character.Instance.GetComponent<Bag>().AddKey(10);
        }
        
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Debug.Log("Дракула Выключен");
            Dracula.Instance.DraculaDespawn();
        }
        
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Debug.Log("Двери Открыты");
            var i = FindObjectsOfType<BaseDoor>();
            foreach (var t in i)
            {
                t.OpenDoor();
            }
            var x =FindObjectsOfType<InvisibleDoor>();
            foreach (var t in x)
            {
                t.WallVisible();
            }
            var z = FindObjectsOfType<LockPick>();
            foreach (var t in z)
            {
                t.OpenDoor();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.F6))
        {
            Debug.Log("Сохранение");
            StateManager.Instance.SaveSceneState();
        }
        
        if (Input.GetKeyDown(KeyCode.F7))
        {
            Debug.Log("Файл сохранения удалён");
            InteractiveState.Instance.ResetState();
            PlayerState.Instance.ResetState();
        }
        
        if (Input.GetKeyDown(KeyCode.F8))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
#endif
   
}
