using System;
using UnityEngine;

public class Chest : InteractiveObject
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void ShowText()
    {
        if (!wosActive)
        {
            base.ShowText();
        }
    }
    
    public override void Use()
    {
        base.Use();
        OpenChest();
        wosActive = true;
    }

    private void OpenChest()
    {
        animator.enabled = true;
    }
    
    protected override void ObjectWosActive()
    {
        OpenChest();
    }
}
