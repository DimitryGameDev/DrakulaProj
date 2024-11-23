using System;
using UnityEngine;

public class StateManager : SingletonBase<StateManager>
{
    private InteractiveState interactiveState;
    private PlayerState playerState;
    private OnePersonCamera onePersonCamera;
    private CharacterInputController characterInputController;
    private Bag bag;
    private InteractiveObject[] objects;
    private Character character;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        interactiveState = InteractiveState.Instance;
        playerState = PlayerState.Instance;
        character = (Character)Character.Instance;
        onePersonCamera = OnePersonCamera.Instance;
        characterInputController = CharacterInputController.Instance;
        bag = character.GetComponent<Bag>();
        objects = FindObjectsOfType<InteractiveObject>();

        LoadSceneState();
    }

    private void LoadSceneState()
    {
        for (int i = 0; i < objects.Length; i++) objects[i].LoadState();

        if (playerState.GetPlayerPos() != Vector3.zero) character.transform.position = playerState.GetPlayerPos();
        if (playerState.GetCameraPos() != Vector3.zero) onePersonCamera.transform.position = playerState.GetCameraPos();
        if (playerState.GetSprintAmount() != 0) characterInputController.SetSpeedTime(playerState.GetSprintAmount());
        if (playerState.GetKeyAmount() != 0) bag.AddKey(playerState.GetKeyAmount());
        if (playerState.GetProjectileAmount() != 0) bag.AddProjectile(playerState.GetProjectileAmount());
        if (playerState.GetMedalAmount() != 0) bag.AddMedalPiece(playerState.GetMedalAmount());

        characterInputController.IsRiflePickup = playerState.GetRifleState();
    }

    public void SaveSceneState()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            interactiveState.Save(objects[i], objects[i].WosActive);
        }

        playerState.Save(character.transform.position, onePersonCamera.transform.position, bag.GetKeyAmount(),
            bag.GetProjectileAmount(), bag.GetMedalPeaceAmount(), characterInputController.TimeSprint,
            characterInputController.IsRiflePickup);
    }
}
