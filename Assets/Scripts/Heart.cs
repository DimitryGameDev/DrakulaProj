using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Heart : MonoBehaviour
{
    [SerializeField] private CharacterInputController characterInputController;
    [SerializeField] private PostProcessVolume postProcessVolume;
    [SerializeField] private float vignetteSpeed;
    
    private Vignette vignette;
    
    private void Start()
    {
        postProcessVolume.profile.TryGetSettings(out vignette);
    }

    private void Update()
    {
        SetHeartView();
    }

    private void SetHeartView()
    {
        if (!characterInputController.HeartEnabled)
        {
            if (vignette.opacity.value != 0)
            {
                vignette.opacity.value = Mathf.Lerp(vignette.opacity.value, 0, vignetteSpeed * Time.deltaTime);
            }
        }
        if (characterInputController.HeartEnabled)
        {
            if (vignette.opacity.value != 1)
            {
                vignette.opacity.value = Mathf.Lerp(vignette.opacity.value, 1, vignetteSpeed * Time.deltaTime);
            }
        }
    }
}