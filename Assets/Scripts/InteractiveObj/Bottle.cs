using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bottle : InteractiveObject
{
    private const string LessNoiseLevelText = "The noise level is reduced";
    private const string DraculaIndestructibleText = "For a while you are invulnerable";
    private const string PlayerRageText = "It was a mistake...";
    private const string PlayerSprintText = "Running time has been increased";
    
    [Header("Bottle Settings")]
    [SerializeField] private BottleType bottleType;
    [SerializeField] private GameObject visualModel;
    [SerializeField] private GameObject impactEffect;
    [Header("For PlayerSprint Bottle")]
    [SerializeField] private int sprintIncrease;
    [Header("For PlayerRageBottle")]
    [SerializeField] private float timeRage;
    [Header("For DraculaIndestructible Bottle")]
    [SerializeField] private float timeIndestructible;
    
    private Death death;

    protected override void Start()
    {
        base.Start();
        death = Character.Instance.GetComponent<Death>();
    }

    public override void Use()
    {
        base.Use();
        SetBottleType();
        ShowAfterText();
        wosActive = true;
    }

    private void SetBottleType()
    {
        switch (bottleType)
        {
            case BottleType.Random:
                Array values = Enum.GetValues(typeof(BottleType));
                BottleType randomValue = (BottleType)values.GetValue(Random.Range(0, values.Length-1));
                bottleType = randomValue;
                SetBottleType();
                return;
            case BottleType.LessNoiseLevel:
                NoiseLevel.Instance.SetZeroLevel();
                SetInfoTextAfterUse(LessNoiseLevelText);
                PickUp();
                return;
            case BottleType.PlayerRage:
                NoiseLevel.Instance.SetMaxLevel();
                CharacterInputController.Instance.StaminaDisable(timeRage);
                SetInfoTextAfterUse(PlayerRageText);
                PickUp();
                return;
            case BottleType.PlayerSprint:
                CharacterInputController.Instance.ChangeSpeedTime(sprintIncrease);
                SetInfoTextAfterUse(PlayerSprintText);
                PickUp();
                return;
            case BottleType.DraculaIndestructible:
                Dracula.Instance.DraculaIndestructible(timeIndestructible);
                SetInfoTextAfterUse(DraculaIndestructibleText);
                PickUp();
                return;
        }
    }

    private void PickUp()
    {
        Destroy(visualModel);
        
        if (impactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);
    }
    
    protected override void ObjectWosActive()
    {
        Destroy(visualModel);
    }
}