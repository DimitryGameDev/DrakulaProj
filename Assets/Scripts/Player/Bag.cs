using System;
using UnityEngine;
using UnityEngine.Events;

public class Bag : MonoBehaviour
{   
    [SerializeField] private float countPieceForMedal;
    private int medalCount;
    private int medalPieceCount;
    private int keyAmount;


    [HideInInspector]public UnityEvent changeKeyAmount;
    [HideInInspector]public UnityEvent changeMedalAmount;
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
        if (pieceCount == 0) return;

        if (Mathf.Approximately(medalPieceCount + pieceCount, countPieceForMedal))
        {
            AddMedal();
            medalPieceCount = 1;
            changeMedalPieceAmount.Invoke();
            return;
        }
        medalPieceCount += pieceCount;
        changeMedalPieceAmount.Invoke();
    }

    public int GetMedalAmount()
    {
        return medalCount;
    }

    public int GetMedalPeaceAmount()
    { 
        return medalPieceCount;
    }
    
    private void AddMedal()
    {
        medalCount++;
        changeMedalAmount.Invoke();
    }
    
    public void RemoveMedal()
    {
        medalCount--;
        changeMedalAmount.Invoke();
    }
    
    public void Reset()
    {
        medalPieceCount = 0;
        medalCount = 0;
        keyAmount = 0;
    }
}