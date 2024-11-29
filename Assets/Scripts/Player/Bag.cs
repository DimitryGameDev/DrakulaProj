using System;
using UnityEngine;
using UnityEngine.Events;

public class Bag : MonoBehaviour
{   
    [SerializeField] private float countPieceForMedal;
    private int medalPieceCount;
    private int keyAmount;
    private int projectileAmount;
    
    [HideInInspector]public UnityEvent changeKeyAmount;
    [HideInInspector]public UnityEvent changeProjectileAmount;
    [HideInInspector]public UnityEvent changeMedalPieceAmount;
    [HideInInspector]public UnityEvent addMedalPieceAmount;

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
    
    public void AddProjectile(int amount)
    {
        projectileAmount += amount;
        changeProjectileAmount?.Invoke();
    }
    
    public bool DrawProjectile(int amount)
    {
        if (projectileAmount - amount < 0) return false;

        projectileAmount -= amount;
        changeProjectileAmount?.Invoke();

        return true;
    }
    
    public int GetProjectileAmount()
    {
        return projectileAmount;
    }
    
    public void AddMedalPiece(int pieceCount)
    {
        medalPieceCount += pieceCount;
        for (int i = 0; i < pieceCount; i++)
        {
            changeMedalPieceAmount.Invoke();
            addMedalPieceAmount.Invoke();
        }
    }

    public int GetMedalPeaceAmount()
    { 
        return medalPieceCount;
    }

    public void RemoveMedalPiece()
    {
        medalPieceCount = 0;
    }

    private void OnDestroy()
    {
        changeKeyAmount.RemoveAllListeners();
        changeProjectileAmount.RemoveAllListeners();
        changeMedalPieceAmount.RemoveAllListeners();
        addMedalPieceAmount.RemoveAllListeners();
    }
}