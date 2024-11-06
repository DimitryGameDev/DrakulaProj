using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BaseDoor : InteractiveObject
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject closedTrigger;
    [SerializeField] private Collider openTrigger;

    private AudioSource audioSource;

    private void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }

    public override void Use()
    {
        base.Use();
        OpenDoor();
    }

    private void OpenDoor()
    {
        if(animator)
        animator.SetBool("Open", true);
        audioSource.Play();

        if (closedTrigger)
            Destroy(closedTrigger);
        if (openTrigger)
            Destroy(openTrigger);
    }
}