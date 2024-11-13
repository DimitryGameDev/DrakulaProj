using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BaseDoor : InteractiveObject
{
    [Header("Door Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject closedTrigger;
    [SerializeField] private Collider openTrigger;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        base.Start();
    }

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
        audioSource.Play();

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