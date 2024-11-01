using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(InteractiveObject))]
public class Bottle : MonoBehaviour
{
    [SerializeField] private BottleType bottleType;
    [SerializeField] private GameObject visualModel;
    [SerializeField] private GameObject impactEffect;

    private InteractiveObject interactiveObject;
    private Death death;

    private void Start()
    {
        interactiveObject = GetComponent<InteractiveObject>();
        interactiveObject.onUse.AddListener(SetBottleType);

        death = Character.Instance.GetComponent<Death>();
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
        interactiveObject.Ondestroy.Invoke(interactiveObject);
        
        if (impactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);
    }
}