using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private AudioClip addKeySFX;
    
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        if(addKeySFX && audioSource)
            audioSource.PlayOneShot(addKeySFX);
        
        Destroy(gameObject, lifeTime);
    }
}
