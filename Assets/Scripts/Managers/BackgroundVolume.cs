using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class BackgroundVolume<T> : SingletonBase<BackgroundVolume<T>> where T : MonoBehaviour
{
    [SerializeField] protected bool playOnAwake;
    [SerializeField] protected  AudioClip[] clips;
    private  AudioSource audioSource;
    private  AudioClip currentClip;
    private  AudioClip nextClip;
    
    private bool playing;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        if (playing)
        {
            if (audioSource.isPlaying && currentClip != nextClip)
            {
                audioSource.volume = Mathf.MoveTowards(audioSource.volume,0f, Time.deltaTime);
                if (audioSource.volume != 0) return;
            }
            
            if (audioSource.clip != nextClip)
            {
                audioSource.volume = 0;
                currentClip = nextClip;
                audioSource.clip = currentClip;
                audioSource.Play();
            } 
            
            audioSource.volume = Mathf.MoveTowards(audioSource.volume,0.6f, Time.deltaTime);
            if (Mathf.Approximately(audioSource.volume, 0.6f)) enabled = false;
        }
        else
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume,0f, Time.deltaTime);
            if (audioSource.volume == 0)
            {
                audioSource.Stop();
                currentClip = null;
                audioSource.clip = null;
                if (clips.Length !=0) Play(clips[Random.Range(0, clips.Length)]);
            }
        }
    }

    public void Play(AudioClip clip)
    {
        nextClip = clip;
        enabled = true;
        playing = true;
    }
    
    public void Stop()
    {
        enabled = true;
        playing = false;
    }
    
    
}
