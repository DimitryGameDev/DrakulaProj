using UnityEngine;

public class Chest : InteractiveObject
{
    private Animator animator;
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public override void Use()
    {
        base.Use();
        OpenChest();
        ShowAfterText();
    }

    private void OpenChest()
    {
        animator.enabled = true;
        Destroy(this);
    }
}
