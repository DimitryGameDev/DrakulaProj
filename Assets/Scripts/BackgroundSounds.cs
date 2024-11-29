using UnityEngine;
public class BackgroundSounds : BackgroundVolume<BackgroundSounds>
{
    private void Start()
    {
        Init();
        if (playOnAwake)
        {
            if (clips.Length != 0)
            {
                var rand = Random.Range(0, clips.Length);
                Play(clips[rand]);
            }
        }
    }
}
