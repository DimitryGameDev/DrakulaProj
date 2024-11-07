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
    }

    private void OpenChest()
    {
        animator.enabled = true;
        Destroy(this);
    }
}
