using UnityEngine;

public class Medal : InteractiveObject
{
    [Header("Medal Settings")]
    [SerializeField] private int countPiece;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject visualModel;
    [SerializeField] private AudioClip medalSound;
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
        AudioSource.Play();
    }

    protected override void ShowAfterText()
    {
        
    }
    private void PickUp()
    {
        Character.Instance.GetComponent<Bag>().AddMedalPiece(countPiece);
        AudioSource.Stop();
        if (impactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        
        Destroy(visualModel);
    }
    protected override void ObjectWosActive()
    {
        AudioSource.Stop();
        Destroy(visualModel);   
    }
}
