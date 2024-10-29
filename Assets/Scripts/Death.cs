using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;

public class Death : MonoBehaviour
{
    [SerializeField]private GameObject draculaPrefab;
    [SerializeField] private Transform cameraTargetDeath;
    [SerializeField] private PostProcessVolume heartPostProcessVolume;
    private Vignette vignette;
    private Dracula dracula;
    
    public UnityEvent OnDeath;
    
    
    private void Start()
    {
        heartPostProcessVolume.profile.TryGetSettings(out vignette);

        if (Dracula.Instance != null)
        {
            dracula = Dracula.Instance;
            dracula.DraculaInPlayer.AddListener(DeathCharacter);
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
        var draculaRot = new Vector3(dracula.transform.position.x, cameraTargetDeath.transform.position.y, dracula.transform.position.z);
        
        cameraTargetDeath.LookAt(draculaRot);
        cameraTargetDeath.transform.parent = null;
        OnePersonCamera.Instance.SetTarget(cameraTargetDeath, TypeMoveCamera.WithRotation);
        transform.LookAt(draculaRot);
        CharacterInputController.Instance.enabled = false;
        FindObjectOfType<Heart>().enabled = false;
        draculaPrefab.transform.parent = null;
        
        enabled = true; //вкдючает эффекты в Update
        draculaPrefab.SetActive(true);
        var animator = draculaPrefab.GetComponent<Animator>();
        animator.Play("Attack");
        OnDeath?.Invoke();
    }

    void OnDestroy()
    {
        CharacterInputController.Instance.draculaAnim.RemoveListener(DeathCharacter);
    }
}
