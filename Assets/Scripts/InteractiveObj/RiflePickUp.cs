using UnityEngine;

public class RiflePickUp : InteractiveObject
{
    [Header("Rifle Settings")]
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject visualModel;

    private AudioSource audioSource;

    public override void Use()
    {
        base.Use();
        PickUp();
        ShowAfterText();
        wosActive = true;
    }

    private void PickUp()
    {
        CharacterInputController.Instance.isRiflePickup = true;

         AudioSource.Play();
        
        if (impactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        
        Destroy(visualModel);
    }
    
    protected override void ObjectWosActive()
    {
        Destroy(visualModel);
    }
}