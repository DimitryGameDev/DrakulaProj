using UnityEngine;
using UnityEngine.Events;

public class Bag : MonoBehaviour
{   
    [SerializeField] private float countPieceForMedal;
    private int medalCount;
    private int medalPieceCount;
    private int keyAmount;

    [HideInInspector]public UnityEvent ChangeKeyAmount;
    [HideInInspector]public UnityEvent ChangeMedalAmount;
    [HideInInspector]public UnityEvent ChangeMedalPieceAmount;

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
    
    public void AddMedalPiece(int pieceCount)
    {
        if (medalPieceCount + pieceCount == countPieceForMedal)
        {
            AddMedal();
            medalPieceCount = 0;
            ChangeMedalPieceAmount.Invoke();
            return;
        }
        medalPieceCount += pieceCount;
        ChangeMedalPieceAmount.Invoke();
    }
    
    public void AddMedal()
    {
        medalCount++;
        ChangeMedalAmount.Invoke();
    }
    
    public void RemoveMedal()
    {
        medalCount--;
        ChangeMedalAmount.Invoke();
    }
}