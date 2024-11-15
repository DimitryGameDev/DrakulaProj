using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private AudioClip addKeySfx;
    public float LifeTime => lifeTime;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        if(addKeySfx && audioSource)
            audioSource.PlayOneShot(addKeySfx);
        
        Destroy(gameObject, lifeTime);
    }
}
