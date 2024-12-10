using UnityEngine;
using UnityEngine.UI;

public class RifleNew : MonoBehaviour
{
    [SerializeField] private GameObject visualModel;
    [SerializeField] private Transform fireTransform;
    [SerializeField] private Transform holdTransform;
    
   
    
    [SerializeField] private float rifleMoveSpeed;
    [SerializeField] private float timeIndestructible;
    [SerializeField] private float shootDistance;
    [SerializeField] private float hideRifleDistance;
    [SerializeField] private float fireRate;
    
    [SerializeField] private AudioClip shot;
    [SerializeField] private AudioClip empty;
    
    private AudioSource audioSource;
    private AimUI aimImage;
    
    private bool isActive;
    private bool isCanFire;
    private float timer;
    
    private void Start()
    {
        CharacterInputController.Instance.visionOn.AddListener(OnRifleActive);
        CharacterInputController.Instance.visionOff.AddListener(OnRifleDeactivate);
        CharacterInputController.Instance.rifleShoot.AddListener(Fire);

        aimImage = AimUI.Instance;
        audioSource = GetComponent<AudioSource>();
        
        visualModel.SetActive(false);
        aimImage.gameObject.SetActive(false);
        transform.parent = null;
    }

    private void Fire()
    { 
        if (!isActive || isCanFire) return;
        
        if (Character.Instance.GetComponent<Bag>().DrawProjectile(1) == false)
        {
            audioSource.PlayOneShot(empty);
        }
        else
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward * shootDistance, out hit))
            {
                if (hit.collider.GetComponentInParent<Dracula>())
                {
                    Dracula.Instance.DraculaIndestructible(timeIndestructible);
                    Debug.Log("Есть пробитие");
                }
            }

            isCanFire = true;
            timer = fireRate;
            
            audioSource.PlayOneShot(shot);
        }
    }
    
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) isCanFire = false;
        aimImage.Fill(timer / fireRate);
        
        int layerMaskOnlyPlayer = 1 << 7;
        var hitForward = Physics.Raycast(fireTransform.position, fireTransform.forward, hideRifleDistance, ~layerMaskOnlyPlayer);
        if (hitForward )
        {
            transform.position = Vector3.Lerp(transform.position,holdTransform.position,Time.deltaTime * rifleMoveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation,holdTransform.rotation,Time.deltaTime * rifleMoveSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position,fireTransform.position,Time.deltaTime * rifleMoveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation,fireTransform.rotation,Time.deltaTime * rifleMoveSpeed);
            visualModel.transform.localRotation = Quaternion.Euler(Mathf.MoveTowards(visualModel.transform.localRotation.x,0,Time.deltaTime),0, 0);
        }
    }

    private void OnRifleActive()
    {
        if (!CharacterInputController.Instance.isRiflePickup) return;
        isActive = true;
        visualModel.SetActive(true);
        aimImage.gameObject.SetActive(true);
    }

    private void OnRifleDeactivate()
    {
        if (!CharacterInputController.Instance.isRiflePickup) return;

        isActive = false;
        visualModel.SetActive(false);
        aimImage.gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        CharacterInputController.Instance.visionOn.RemoveListener(OnRifleActive);
        CharacterInputController.Instance.visionOff.RemoveListener(OnRifleDeactivate);
        CharacterInputController.Instance.rifleShoot.RemoveListener(Fire);
    }
}
