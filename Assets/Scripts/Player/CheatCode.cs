using UnityEngine;

public class CheatCode : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("Сердце получено");
            CharacterInputController.Instance.pickUpHeart = true;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("Бесконечный спринт");
            CharacterInputController.Instance.ChangeSpeedTime(1000);
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
        
        if (Input.GetKeyDown(KeyCode.Plus))
        {
            Debug.Log("Скорость дракулы Увеличена");
            Character.Instance.GetComponent<Bag>().AddKey(10);
            Dracula.Instance.SpeedChange(1);
        }
        
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            Debug.Log("Скорость дракулы Уменьшена");
            Character.Instance.GetComponent<Bag>().AddKey(10);
            Dracula.Instance.SpeedChange(-1);
        }
    }
}
