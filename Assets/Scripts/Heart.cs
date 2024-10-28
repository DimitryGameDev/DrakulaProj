using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Heart : MonoBehaviour
{
    [SerializeField] private CharacterInputController characterInputController;
    [SerializeField] private PostProcessVolume postProcessVolume;
    [SerializeField] private float vignetteSpeed;
    [SerializeField] private float lensDistortionSpead;
    private bool isActive;
    public bool IsActive => isActive;
    private Vignette vignette;
    private LensDistortion lensDistortion;
    
    private void Start()
    {
        postProcessVolume.profile.TryGetSettings(out vignette);
        postProcessVolume.profile.TryGetSettings(out lensDistortion);
    }

    private void Update()
    {
        SetHeartView();
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
                lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, 0, lensDistortionSpead * Time.deltaTime);
            }
        }
        if (characterInputController.HeartEnabled)
        {
            isActive = true;
            if (vignette.opacity.value != 1)
            {
                vignette.opacity.value = Mathf.Lerp(vignette.opacity.value, 1, vignetteSpeed * Time.deltaTime);
            }
            if (lensDistortion.intensity.value != 0)
            {
                lensDistortion.intensity.value = Mathf.Lerp(lensDistortion.intensity.value, -50, lensDistortionSpead * Time.deltaTime);
            }
        }
    }
}
