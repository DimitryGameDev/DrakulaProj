using UnityEngine;
using System;


public class PlayerState : SingletonBase<PlayerState>
{
    private const string FileName = "PlayerState.dat";
    [SerializeField] private States playerData;
    
    [Serializable]
    private class States
    {
        public Vector3 pos;
        public int keyAmount;
        public int medalAmount;
        public int noisesAmount;
        //public int sprintAmount;
        public bool heartState;
    }
    
    private void Awake()
    {
        Init();
        if (FileHandler.HasFile(FileName))
        {
            Debug.Log("Файл найден");
            Saver<States>.TryLoad(FileName,ref playerData);
        }
        else
        {
            Debug.Log("Создан новый файл" + FileName);
            var obj = new States();
            Saver<States>.Save(FileName, playerData);
        }
    }
    
    public void Save(Vector3 playerPos,int keys, int medals, int noises, bool heartState)
    {
        if (Instance)
        {
            playerData.pos = playerPos;
            playerData.keyAmount = keys;
            playerData.medalAmount = medals;
            playerData.noisesAmount = noises;
            playerData.heartState = heartState;
            Saver<States>.Save(FileName, playerData);
        }
        else
        {
            Debug.Log("Bruuuh");
        }     
    } 
    
    
    public Vector3 GetPos()
    {
        return playerData.pos;
    }
    
    public int GetKeyAmount()
    {
        return playerData.keyAmount;
    } 
    
    public int GetMedalAmount()
    {
        return playerData.medalAmount;
    } 
    
    public int GetNoisesAmount()
    {
        return playerData.noisesAmount;
    } 
    
    public bool GetHeartState()
    {
        return playerData.heartState;
    }
    
    public void ResetState()
    {
        FileHandler.Reset(FileName);
    }
}
