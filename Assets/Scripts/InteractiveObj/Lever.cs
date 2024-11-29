using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Lever : InteractiveObject
{
    [SerializeField] private GameObject trigger;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip audioClip;

    protected override void Start()
    {
        base.Start();
        //trigger.SetActive(false);
    }

    public override void Use()
    {
        base.Use();
        Open();
        ShowAfterText();
        wosActive = true;
    }

    public void Open()
    {
        //trigger.SetActive(true);
        if (!wosActive)AudioSource.PlayOneShot(audioClip);
        animator.SetBool("Open", true);
    }
    
    protected override void ObjectWosActive()
    {
        Open();
    }
}