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
    [SerializeField] private AudioClip heartOnSfx;

    public bool IsActive { get; private set; }
    private Volume postProcessVolume;
    private Vignette vignette;
    private LensDistortion lensDistortion;
    private Character character;
    private float timer;

    private AudioSource audioSource;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        CharacterInputController.Instance.visionOn.AddListener(PlaySfx);
        character = (Character)Character.Instance;
        postProcessVolume = GetComponent<Volume>();
        postProcessVolume.profile.TryGet(out vignette);
        postProcessVolume.profile.TryGet(out lensDistortion);
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
            if (IsActive )
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
        if (!characterInputController.VisionEnabled)
        {
            IsActive = false;
            
            if (vignette.smoothness.value != 0)
            {
                vignette.smoothness.value = Mathf.Lerp(vignette.smoothness.value, 0, vignetteSpeed * Time.deltaTime);
            }
            if (lensDistortion.intensity.value != 0)
            {
                lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, 0, lensDistortionSpeed * Time.deltaTime);
            }
        }
        if (characterInputController.VisionEnabled)
        {
            IsActive = true;
 
            if (!Mathf.Approximately(vignette.smoothness.value, 1))
            {
                vignette.smoothness.value = Mathf.Lerp(vignette.smoothness.value, 1, vignetteSpeed * Time.deltaTime);
            }
            if (!Mathf.Approximately(lensDistortion.intensity.value, -0.5f))
            {
                lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, -0.5f, lensDistortionSpeed * Time.deltaTime);
            }
        }
    }

    private void PlaySfx()
    {
        audioSource.PlayOneShot(heartOnSfx);
    }

    private void OnDestroy()
    {
        CharacterInputController.Instance.visionOn.RemoveListener(PlaySfx);
    }
}
