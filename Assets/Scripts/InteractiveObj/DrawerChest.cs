using UnityEngine;

public class DrawerChest : InteractiveObject
{
    [SerializeField] private GameObject[] items;
    [SerializeField] private AudioClip openSfx;
    
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        if (items != null)
            for (int i = 0; i < items.Length; i++)
            {
                items[i].transform.SetParent(transform.GetChild(0));
            }
    }

    public override void Use()
    {
        base.Use();
        wosActive = !wosActive;
        ActiveChest();
        AudioSource.PlayOneShot(openSfx);
    }

    private void ActiveChest()
    {
        if (wosActive)
        {
            animator.SetBool("IsOpen", true);
        }

        if (!wosActive)
        {
            animator.SetBool("IsOpen", false);
        }
        
    }

    protected override void ObjectWosActive()
    {
        ActiveChest();
    }
}
