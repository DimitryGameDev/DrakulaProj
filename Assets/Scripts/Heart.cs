using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(AudioSource))]
public class Heart : MonoBehaviour
{
    [SerializeField] private CharacterInputController characterInputController;
    [SerializeField] private PostProcessVolume postProcessVolume;
    [SerializeField] private float vignetteSpeed = 5f;
    [SerializeField] private float lensDistortionSpeed = 8f;
    [SerializeField] private float tickRateStress = 1f;
    [SerializeField] private float addStressValue = 1f;
    [SerializeField] private float removeStressValue = 1f;
    [SerializeField] private AudioClip heartOnSFX;
    
    private bool isActive;
    public bool IsActive => isActive;
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
        postProcessVolume.profile.TryGetSettings(out vignette);
        postProcessVolume.profile.TryGetSettings(out lensDistortion);
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
            
            if (vignette.opacity.value != 0)
            {
                vignette.opacity.value = Mathf.Lerp(vignette.opacity.value, 0, vignetteSpeed * Time.deltaTime);
            }
            if (lensDistortion.intensity.value != 0)
            {
                lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, 0, lensDistortionSpeed * Time.deltaTime);
            }
        }
        if (characterInputController.HeartEnabled)
        {
            isActive = true;
 
            if (vignette.opacity.value != 1)
            {
                vignette.opacity.value = Mathf.Lerp(vignette.opacity.value, 1, vignetteSpeed * Time.deltaTime);
            }
            if (lensDistortion.intensity.value != -60)
            {
                lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, -60, lensDistortionSpeed * Time.deltaTime);
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
