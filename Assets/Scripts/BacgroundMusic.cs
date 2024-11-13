using System;
using UnityEngine;

public class BacgroundMusic : SingletonBase<BacgroundMusic>
{
    private  AudioSource audioSource;
    private  AudioClip currentClip;
    private  AudioClip nextClip;

    private bool playing;
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        enabled = false;
    }
    
    private void Update()
    {
        if (playing)
        {
            if (audioSource.isPlaying && currentClip != nextClip)
            {
                audioSource.volume = Mathf.MoveTowards(audioSource.volume,0f, Time.deltaTime);
                return;
            }
        
            if (audioSource.clip != nextClip)
            {
                audioSource.volume = 0;
                currentClip = nextClip;
                audioSource.clip = currentClip;
                audioSource.Play();
            } 
            audioSource.volume = Mathf.MoveTowards(audioSource.volume,1f, Time.deltaTime);
            if (Mathf.Approximately(audioSource.volume, 1)) enabled = false;
        }
        else
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume,0f, Time.deltaTime);
            if (audioSource.volume == 0)
            {
                audioSource.Stop();
                currentClip = null;
                audioSource.clip = null;
                enabled = false;
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
