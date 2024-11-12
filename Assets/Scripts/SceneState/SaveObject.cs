using System;
using UnityEngine;

public class SaveObject : InteractiveObject
{
    private InteractiveState interactiveState;
    private PlayerState playerState;
    private Character character;
    private CharacterInputController characterInputController;
    private Bag bag;
    private NoiseLevel noiseLevel;
    private InteractiveObject[] objects;
    protected override void Start()
    {
        base.Start();
        interactiveState = InteractiveState.Instance;
        playerState = PlayerState.Instance;
        character = (Character)Character.Instance;
        noiseLevel = NoiseLevel.Instance;
        characterInputController = CharacterInputController.Instance;
        bag = character.GetComponent<Bag>();
        objects = FindObjectsOfType<InteractiveObject>();

        LoadSceneState();
    }

    public override void Use()
    {
        SaveSceneState();
        ShowAfterText();
    }

    private void LoadSceneState()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].LoadState();
        }
        
        if (playerState.GetPos() != Vector3.zero)character.transform.position = playerState.GetPos();
        bag.AddKey(playerState.GetKeyAmount());
        bag.AddMedalPiece(playerState.GetMedalAmount());
        characterInputController.pickUpHeart = playerState.GetHeartState();
    }
    
    private void SaveSceneState()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            interactiveState.Save(objects[i],objects[i].WosActive);
        }
        
        playerState.Save(character.transform.position, bag.GetKeyAmount(),bag.GetMedalPeaceAmount(),noiseLevel.CurrentLevel,characterInputController.pickUpHeart);
    }
}
