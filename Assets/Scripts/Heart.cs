using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(AudioSource))]
public class Heart : MonoBehaviour
{
    [SerializeField] private CharacterInputController characterInputController;
    [SerializeField] private float vignetteSpeed = 5f;
    [SerializeField] private float lensDistortionSpeed = 8f;
    [SerializeField] private float tickRateStress = 1f;
    [SerializeField] private float addStressValue = 1f;
    [SerializeField] private float removeStressValue = 1f;
    [SerializeField] private AudioClip heartOnSFX;
    
    private bool isActive;
    public bool IsActive => isActive;
    private Volume postProcessVolume;
    private Vignette vignette;
    private LensDistortion lensDistortion;
    private Character character;
    private float timer;

    private AudioSource audioSource;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        CharacterInputController.Instance.heartOn.AddListener(PlaySFX);
        character = (Character)Character.Instance;
        postProcessVolume = GetComponent<Volume>();
        postProcessVolume.profile.TryGet<Vignette>(out vignette);
        postProcessVolume.profile.TryGet<LensDistortion>(out lensDistortion);
    }

    private void Update()
    {
        SetHeartView();
        ChangeStress();
    }

    private void ChangeStress()
    {
        timer += Time.deltaTime;
        if (timer >= tickRateStress)
        {
            if (isActive )
            {
                character.AddStress(addStressValue);
            }
            else
            {
                character.RemoveStress(removeStressValue);
            }
            timer = 0;
        }
    }

    private void SetHeartView()
    {
        if (!characterInputController.HeartEnabled)
        {
            isActive = false;
            
            if (vignette.smoothness.value != 0)
            {
                vignette.smoothness.value = Mathf.Lerp(vignette.smoothness.value, 0, vignetteSpeed * Time.deltaTime);
            }
            if (lensDistortion.intensity.value != 0)
            {
                lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, 0, lensDistortionSpeed * Time.deltaTime);
            }
        }
        if (characterInputController.HeartEnabled)
        {
            isActive = true;
 
            if (vignette.smoothness.value != 1)
            {
                vignette.smoothness.value = Mathf.Lerp(vignette.smoothness.value, 1, vignetteSpeed * Time.deltaTime);
            }
            if (lensDistortion.intensity.value != -0.5f)
            {
                lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, -0.5f, lensDistortionSpeed * Time.deltaTime);
            }
        }
    }

    private void PlaySFX()
    {
        audioSource.PlayOneShot(heartOnSFX);
    }

    private void OnDestroy()
    {
        CharacterInputController.Instance.heartOn.RemoveListener(PlaySFX);
    }
}
