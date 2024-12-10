using UnityEngine;

public class Medal : InteractiveObject
{
    [Header("Medal Settings")]
    [SerializeField] private int countPiece;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject visualModel;
    [SerializeField] private AudioClip medalSound;
    [SerializeField] private AudioClip medalPickUpSound;
    public override void Use()
    {
        base.Use();
        PickUp();
        ShowAfterText();
        wosActive = true;
    }

    protected override void Start()
    {
        base.Start();
        AudioSource.clip = medalSound;
        if (!wosActive)AudioSource.Play();
    }

    protected override void ShowAfterText()
    {
        
    }
    
    private void PickUp()
    {
        Character.Instance.GetComponent<Bag>().AddMedalPiece(countPiece);
        AudioSource.Stop();
        BackgroundSounds.Instance.Play(medalPickUpSound);
        if (impactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        
        Destroy(visualModel);
    }
    
    protected override void ObjectWosActive()
    {
        
        Destroy(visualModel);   
    }
}
