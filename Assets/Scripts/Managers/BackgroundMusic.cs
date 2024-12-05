using UnityEngine;

public class BackgroundMusic : BackgroundVolume<BackgroundMusic>
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
