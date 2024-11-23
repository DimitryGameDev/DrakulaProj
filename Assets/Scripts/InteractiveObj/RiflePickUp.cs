using UnityEngine;

public class RiflePickUp : InteractiveObject
{
    [Header("Rifle Settings")]
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject visualModel;

    private AudioSource audioSource;
    
    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }

    public override void Use()
    {
        base.Use();
        PickUp();
        ShowAfterText();
        wosActive = true;
    }

    private void PickUp()
    {
        CharacterInputController.Instance.IsRiflePickup = true;

        audioSource.Play();
        
        if (impactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        
        Destroy(visualModel);
    }
    protected override void ObjectWosActive()
    {
        Destroy(visualModel);
    }
}