using System;
using Unity.VisualScripting;
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
        if (!wosActive)
        {
            AudioSource.clip = medalSound;
            AudioSource.Play();
        }
    }

    protected override void Update()
    {
        base.Update();
        
    }

    private void PickUp()
    {
        Character.Instance.GetComponent<Bag>().AddMedalPiece(countPiece);
        
        if (impactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        
        Destroy(visualModel);
    }
    protected override void ObjectWosActive()
    {
        Destroy(visualModel);   
    }
}
