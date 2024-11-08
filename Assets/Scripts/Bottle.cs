using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bottle : InteractiveObject
{
    [Header("Bottle Settings")]
    [SerializeField] private BottleType bottleType;
    [SerializeField] private GameObject visualModel;
    [SerializeField] private GameObject impactEffect;
    
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

    public void SetBottleType()
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
                PickUp();
                return;
            case BottleType.PlayerDeath:
                death.LoseGame();
                PickUp();
                return;
            case BottleType.DraculaIndestructible:
                Dracula.Instance.DraculaDisable();
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