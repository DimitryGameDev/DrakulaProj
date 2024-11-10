using UnityEngine;

public class DraculaSpawnEffect : MonoBehaviour
{
    private readonly float lifeTime = 1f;
    private ParticleSystem particleEffect;
    private CharacterInputController characterInputController;
    private float minTimeParticle;
    private float timer;
    private void Start()
    {
        particleEffect = GetComponent<ParticleSystem>();
        minTimeParticle = particleEffect.main.duration;
        characterInputController = CharacterInputController.Instance;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        if (particleEffect != null)
        {
            if (!characterInputController.HeartEnabled && timer >= minTimeParticle)
            {
                particleEffect.Stop();
                if (!IsPlaying())
                {
                    Destroy(gameObject,lifeTime);
                }
            }
        }
    }
    public bool IsPlaying() => particleEffect != null && particleEffect.isPlaying;
}
