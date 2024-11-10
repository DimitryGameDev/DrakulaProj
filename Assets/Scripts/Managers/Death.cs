using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using Vignette = UnityEngine.Rendering.Universal.Vignette;

public class Death : MonoBehaviour
{
    [SerializeField]private GameObject draculaPrefab;
    [SerializeField] private Transform cameraTargetDeath;
    [SerializeField] private Volume heartPostProcessVolume;
    private Vignette vignette;
    private Dracula dracula;
    
    public UnityEvent onDeath;
    private Animator animator;
    private void Start()
    {
        heartPostProcessVolume.profile.TryGet(out vignette);
        if (Dracula.Instance != null)
        {
            dracula = Dracula.Instance;
            dracula.draculaInPlayer.AddListener(DeathCharacter);
        }
        animator = draculaPrefab.GetComponentInChildren<Animator>();
        draculaPrefab.SetActive(false);
        
        enabled = false;
    }

    private const float VignetteSpeed = 5F;

    private void Update()
    {
        vignette.smoothness.value = Mathf.Lerp(vignette.smoothness.value, 1, VignetteSpeed * Time.deltaTime);
    }

    public void DeathCharacter(int animID)
    {
        var rb = Character.Instance.GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.velocity = new Vector3(0, 0, 0);
        CharacterInputController.Instance.enabled = false;
        FindObjectOfType<Heart>().enabled = false;
        
        if (animID == 1) DieLogic1();
        if (animID == 2) DieLogic2();
        if (animID == 3) DieLogic3();
        LoseGame();
    }
    
    public void LoseGame()
    {
        onDeath?.Invoke();
    }
    
    private readonly Vector3 nosferatuPos1 = new Vector3(0f, -1.7f, 0.5f);
    private readonly Vector3 nosferatuPos2 = new Vector3(0f, -1f, 2.5f);
    private readonly Vector3 nosferatuRotate1 = new Vector3(0f, 160f, 0f);
    private readonly Vector3 nosferatuRotate2 = new Vector3(0f, 180f, 0f);
    
    /// <summary>
    /// ALARM!!!!!! Не трогать ни при каких условиях!
    /// </summary>
    private void DieLogic1()
    {
        draculaPrefab.transform.localPosition = nosferatuPos1;
        draculaPrefab.transform.localRotation = Quaternion.Euler(nosferatuRotate1);
        
        cameraTargetDeath.LookAt(new Vector3(dracula.transform.position.x, cameraTargetDeath.transform.position.y, dracula.transform.position.z));
        cameraTargetDeath.transform.parent = null;
        
        OnePersonCamera.Instance.SetTarget(cameraTargetDeath, TypeMoveCamera.WithRotation, true);
        
        transform.LookAt(new Vector3(dracula.transform.position.x, transform.position.y, dracula.transform.position.z));
        
        enabled = true; //вкдючает эффекты в Update
        draculaPrefab.SetActive(true);
        
        animator.Play("Attack1");
    }
    
    private void DieLogic3()
    {
        draculaPrefab.transform.localPosition = nosferatuPos2;
        draculaPrefab.transform.localRotation = Quaternion.Euler(nosferatuRotate2);
        
        cameraTargetDeath.LookAt(new Vector3(dracula.transform.position.x, cameraTargetDeath.transform.position.y, dracula.transform.position.z));
        cameraTargetDeath.transform.parent = null;
        
        OnePersonCamera.Instance.SetTarget(cameraTargetDeath, TypeMoveCamera.WithRotation, true);
        
        transform.LookAt(new Vector3(dracula.transform.position.x, transform.position.y, dracula.transform.position.z));
        
        enabled = true; //вкдючает эффекты в Update
        draculaPrefab.SetActive(true);
        
        animator.Play("Attack2");
    }
    
    /// <summary>
    /// ALARM!!!!!! Не трогать ни при каких условиях!
    /// </summary>
    private void DieLogic2()
    {
        draculaPrefab.transform.localPosition = nosferatuPos2;
        draculaPrefab.transform.localRotation = Quaternion.Euler(nosferatuRotate2);
        
        cameraTargetDeath.localRotation = Quaternion.Euler(0, 180, 0);
        cameraTargetDeath.transform.parent = null;
        OnePersonCamera.Instance.SetTarget(cameraTargetDeath, TypeMoveCamera.WithRotation, true);
        
        Vector3 rotate = transform.eulerAngles;
        rotate.y += 180;
        transform.localRotation = Quaternion.Euler(rotate);
        
        enabled = true; //вкдючает эффекты в Update
        draculaPrefab.SetActive(true);
        
        animator.Play("Attack2");
    }
    
    private void OnDestroy()
    {
        dracula.draculaInPlayer.RemoveListener(DeathCharacter);
    }
}
