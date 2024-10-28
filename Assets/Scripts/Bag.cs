using UnityEngine;
using UnityEngine.Events;

public class Bag : MonoBehaviour
{
    private int keyAmount;

    public UnityEvent ChangeKeyAmount;

    public void AddKey(int amount)
    {
        keyAmount += amount;
        ChangeKeyAmount?.Invoke();
    }

    public bool DrawKey(int amount)
    {
        if (keyAmount - amount < 0) return false;

        keyAmount -= amount;
        ChangeKeyAmount?.Invoke();

        return true;
    }

    public int GetKeyAmount()
    {
        return keyAmount;
    }
}