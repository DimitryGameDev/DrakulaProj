using UnityEngine;
using UnityEngine.Events;

public class Bag : MonoBehaviour
{   
    [SerializeField] private float countPieceForMedal;
    private int medalPieceCount;
    private int keyAmount;
    
    [HideInInspector]public UnityEvent changeKeyAmount;
    [HideInInspector]public UnityEvent changeMedalPieceAmount;

    public void AddKey(int amount)
    {
        keyAmount += amount;
        changeKeyAmount?.Invoke();
    }
    
    public bool DrawKey(int amount)
    {
        if (keyAmount - amount < 0) return false;

        keyAmount -= amount;
        changeKeyAmount?.Invoke();

        return true;
    }

    public int GetKeyAmount()
    {
        return keyAmount;
    }
    
    public void AddMedalPiece(int pieceCount)
    {
        medalPieceCount += pieceCount;
        changeMedalPieceAmount.Invoke();
    }

    public int GetMedalPeaceAmount()
    { 
        return medalPieceCount;
    }

    public void RemoveMedalPiece()
    {
        medalPieceCount = 0;
    }
    
}