using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Lever : InteractiveObject
{
    [SerializeField] private GameObject trigger;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip audioClip;
    
    private AudioSource audioSource;
    
    private void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        trigger.SetActive(false);
    }

    public override void Use()
    {
        base.Use();
        Open();
    }

    public void Open()
    {
        trigger.SetActive(true);
        audioSource.PlayOneShot(audioClip);
        animator.SetBool("Open", true);
        ShowAfterText();
    }
}