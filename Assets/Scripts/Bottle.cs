using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bottle : InteractiveObject
{
    private const string LessNoiseLevelText = "Уровень шума снижен";
    private const string DraculaIndestructibleText = "Вы неуязвимы к драруле";
    private const string PlayerDeathText = "Это был яд";
    private const string PlayerSprintText = "Время бега увеличено";
    
    [Header("Bottle Settings")]
    [SerializeField] private BottleType bottleType;
    [SerializeField] private GameObject visualModel;
    [SerializeField] private GameObject impactEffect;
    [Header("For PlayerSprint Bottle")]
    [SerializeField] private int sprintIncrease;
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
    }

    private void SetBottleType()
    {
        switch (bottleType)
        {
            case BottleType.Random:
                Array values = Enum.GetValues(typeof(BottleType));
                BottleType randomValue = (BottleType)values.GetValue(Random.Range(0, values.Length));
                bottleType = randomValue;
                SetBottleType();
                return;
            case BottleType.LessNoiseLevel:
                NoiseLevel.Instance.SetZeroLevel();
                SetInfoTextAfterUse(LessNoiseLevelText);
                PickUp();
                return;
            case BottleType.PlayerDeath:
                death.LoseGame();
                SetInfoTextAfterUse(PlayerDeathText);
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
}