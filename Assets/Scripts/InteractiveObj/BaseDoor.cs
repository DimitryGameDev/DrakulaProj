using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BaseDoor : InteractiveObject
{
    [Header("Door Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider closedTrigger;
    [SerializeField] private Collider openTrigger;
    [SerializeField] private AudioClip openSound;
    
    public override void Use()
    {
        base.Use();
        OpenDoor();
        wosActive = true;
    }

    public void OpenDoor()
    {
        if(animator)
            animator.SetBool("Open", true);
        
        if (!wosActive) AudioSource.PlayOneShot(openSound);
        
        if (closedTrigger)
            Destroy(closedTrigger);
        if (openTrigger)
            Destroy(openTrigger);
    }
    
    protected override void ObjectWosActive()
    {
        OpenDoor();
    }
}