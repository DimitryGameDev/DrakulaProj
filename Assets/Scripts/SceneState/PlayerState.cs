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
        public Vector3 cameraPos;
        public int keyAmount;
        public int projectileAmount;
        public int medalAmount;
        public float sprintAmount;
        public bool rifleState;
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
    
    public void Save(Vector3 playerPos,Vector3 cameraPos,int keys, int projectiles, int medals,float sprintAmount, bool rifleState)
    {
        if (Instance)
        {
            playerData.pos = playerPos;
            playerData.cameraPos = cameraPos;
            playerData.keyAmount = keys;
            playerData.projectileAmount = projectiles;
            playerData.medalAmount = medals;
            playerData.sprintAmount = sprintAmount;
            playerData.rifleState = rifleState;
            Saver<States>.Save(FileName, playerData);
        }
        else
        {
            Debug.Log("Bruuuh");
        }     
    } 
    
    
    public Vector3 GetPlayerPos()
    {
        return playerData.pos;
    }
    public Vector3 GetCameraPos()
    {
        return playerData.cameraPos;
    }
    
    public int GetKeyAmount()
    {
        return playerData.keyAmount;
    } 
    
    public int GetProjectileAmount()
    {
        return playerData.projectileAmount;
    } 
    
    public int GetMedalAmount()
    {
        return playerData.medalAmount;
    }
    
    public float GetSprintAmount()
    {
        return playerData.sprintAmount;
    } 
    
    public bool GetRifleState()
    {
        return playerData.rifleState;
    }
    
    public void ResetState()
    {
        FileHandler.Reset(FileName);
    }
}
