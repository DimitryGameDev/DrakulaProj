using System;
using UnityEngine;

public class CharacterStateManager : SingletonBase<CharacterStateManager>
{ 
    [SerializeField] private Character characterPrefab;
    [SerializeField] private Transform characterSpawnPoint;
    
    private PlayerState playerState;
    private Bag bag;
    private Character character;
    private CharacterInputController characterInputController;
    private OnePersonCamera onePersonCamera;

    private void Awake()
    {
        Init();
        Spawn();
    }
    void Start()
    {
        LoadProperties();
    }
    
    private void Spawn()
    {
        playerState = PlayerState.Instance;
        
        if (playerState.GetPlayerPos() != Vector3.zero)
        {
            characterSpawnPoint.position = playerState.GetPlayerPos();
            characterSpawnPoint.rotation = playerState.GetPlayerRotation();
        }
        
        character = Instantiate(characterPrefab, characterSpawnPoint.position,characterSpawnPoint.rotation);
        bag = character.GetComponent<Bag>();
        characterInputController = character.GetComponent<CharacterInputController>();
        onePersonCamera = OnePersonCamera.Instance;
    }
    
    private void LoadProperties()
    {
        if (playerState.GetSprintAmount() != 0) character.GetComponent<CharacterInputController>().SetSpeedTime(playerState.GetSprintAmount());
        if (playerState.GetCameraPos() != Vector3.zero) onePersonCamera.transform.position = playerState.GetCameraPos();
        if (playerState.GetKeyAmount() != 0) bag.AddKey(playerState.GetKeyAmount());
        if (playerState.GetProjectileAmount() != 0) bag.AddProjectile(playerState.GetProjectileAmount());
        if (playerState.GetMedalAmount() != 0) bag.AddMedalPiece(playerState.GetMedalAmount());
        characterInputController.IsRiflePickup = playerState.GetRifleState();
    }

    public void Save()
    {
        playerState.Save(character.transform.position,character.transform.rotation, onePersonCamera.transform.position, bag.GetKeyAmount(),
            bag.GetProjectileAmount(), bag.GetMedalPeaceAmount(), characterInputController.Stamina,
            characterInputController.IsRiflePickup);
    }
}
