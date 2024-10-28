using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

public class Death : MonoBehaviour
{
    [SerializeField]private GameObject draculaPrefab;
    [SerializeField] private Transform targeTransformDeath;
    [SerializeField] private PostProcessVolume heartPostProcessVolume;
    private Vignette vignette;
    
    public UnityEvent OnDeath;
    
    private void Start()
    {
        heartPostProcessVolume.profile.TryGetSettings(out vignette);

        if (Dracula.Instance != null)
        {
            Dracula.Instance.DraculaInPlayer.AddListener(DeathCharacter);
        }
        CharacterInputController.Instance.draculaAnim.AddListener(DeathCharacter);
        
        draculaPrefab.SetActive(false);
        
        enabled = false;
    }

    private const float VignetteSpeed = 5F;

    private void Update()
    {
        vignette.opacity.value = Mathf.Lerp(vignette.opacity.value, 1, VignetteSpeed * Time.deltaTime);
        if (Mathf.Approximately(vignette.opacity.value, 1))
        {
            enabled = false;
        }
    }

    private void DeathCharacter()
    {
        FindObjectOfType<Heart>().enabled = false;
        CharacterInputController.Instance.enabled = false;
        OnePersonCamera.Instance.SetTarget(targeTransformDeath, TypeMoveCamera.WithRotation);
        enabled = true;
        
        draculaPrefab.SetActive(true);
        targeTransformDeath.transform.parent = null;
        draculaPrefab.transform.parent = null;
        
        var animator = draculaPrefab.GetComponent<Animator>();
        animator.Play("Attack");
        OnDeath?.Invoke();
    }

    void OnDestroy()
    {
        CharacterInputController.Instance.draculaAnim.RemoveListener(DeathCharacter);
    }
}
