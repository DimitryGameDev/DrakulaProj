using UnityEngine;

[RequireComponent(typeof(InteractiveObject))]
[RequireComponent(typeof(AudioSource))]
public class Lever : MonoBehaviour
{
    [SerializeField] private GameObject trigger;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip audioClip;
    
    private AudioSource audioSource;
    private InteractiveObject interactiveObject;
    
    private void Start()
    {
        interactiveObject = GetComponent<InteractiveObject>();
        audioSource = GetComponent<AudioSource>();
        trigger.SetActive(false);
        interactiveObject.onUse.AddListener(Open);
    }

    public void Open()
    {
        trigger.SetActive(true);
        audioSource.PlayOneShot(audioClip);
        animator.SetBool("Open", true);
    }

    private void OnDestroy()
    {
        interactiveObject.onUse.RemoveListener(Open);
    }
}