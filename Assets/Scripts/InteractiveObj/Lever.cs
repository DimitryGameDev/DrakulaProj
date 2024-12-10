using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Lever : InteractiveObject
{
    [SerializeField] private GameObject trigger;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animator2;
    [SerializeField] private AudioClip audioClip;

    public override void Use()
    {
        base.Use();
        Open();
        ShowAfterText();
        wosActive = true;
    }

    public void Open()
    {
        if (!wosActive)AudioSource.PlayOneShot(audioClip);
        animator.SetBool("Open", true);
        animator2.SetBool("Open", true);
    }
    
    protected override void ObjectWosActive()
    {
        Open();
    }
}