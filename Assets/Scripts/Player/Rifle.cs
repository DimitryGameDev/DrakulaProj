using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Rifle : MonoBehaviour
{
    [SerializeField] private GameObject visualModel;
    [SerializeField] private Transform startRay;
    [SerializeField] private Transform lookAtTarget;
    [Range(0, 180)] [SerializeField] private float verticalAngleOffset;
    
    [SerializeField] private float timeIndestructible;
    [SerializeField] private float shootDistance;
    [SerializeField] private float hideRifleDistance;

    [SerializeField] private ParticleSystem muzzleParticleSystem;

    [SerializeField] private AudioClip shot;
    [SerializeField] private AudioClip click;
  
    private Camera mainCamera;
    private AudioSource audioSource;

    private bool isActive;
    private bool isCanFire;

    private void Start()
    {
        visualModel.SetActive(false);
        CharacterInputController.Instance.visionOn.AddListener(OnRifleActive);
        CharacterInputController.Instance.visionOff.AddListener(OnRifleDeactivate);
        CharacterInputController.Instance.rifleShoot.AddListener(Fire);

        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        CharacterInputController.Instance.visionOn.RemoveListener(OnRifleActive);
        CharacterInputController.Instance.visionOff.RemoveListener(OnRifleDeactivate);
        CharacterInputController.Instance.rifleShoot.RemoveListener(Fire);
    }

    private void FixedUpdate()
    {
        LookAtTarget();
    }

    private void OnRifleActive()
    {
        if (!CharacterInputController.Instance.isRiflePickup) return;

        isActive = true;
        visualModel.SetActive(true);
    }

    private void OnRifleDeactivate()
    {
        if (!CharacterInputController.Instance.isRiflePickup) return;

        isActive = false;
        visualModel.SetActive(false);
    }

    private void Fire()
    {
        if (!isActive || isCanFire) return;

        if (Character.Instance.GetComponent<Bag>().DrawProjectile(1) == false)
        {
            audioSource.PlayOneShot(click);
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

            audioSource.PlayOneShot(shot);

            if (muzzleParticleSystem)
            {
                muzzleParticleSystem.time = 0;
                muzzleParticleSystem.Play();
            }
        }
    }

    private void LookAtTarget()
    {
        RaycastHit ray;
        
        if (Physics.Raycast(Character.Instance.transform.position,Character.Instance.transform.forward,out ray, hideRifleDistance))
        {
            if (!ray.collider.GetComponentInParent<Character>())
            {
                isCanFire = true;
            }
        }
        else
        {
            isCanFire = false;
        }
        
        if (isActive)
        {
            if (isCanFire)
            {
                float newX = Mathf.LerpAngle(transform.eulerAngles.x, 65, Time.deltaTime * 10);
                transform.localEulerAngles = new Vector3(newX, transform.localEulerAngles.y,transform.localEulerAngles.z);
            }
            else
            {
                float currentXRotation = mainCamera.transform.eulerAngles.x;
                Quaternion targetRotation = Quaternion.LookRotation(lookAtTarget.position - transform.position);

                if (currentXRotation < verticalAngleOffset || currentXRotation > 360 - verticalAngleOffset)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10);
                }
                else
                {
                    float angleSignX = Mathf.Sign(mainCamera.transform.rotation.x);
                    Quaternion newTargetRotation = Quaternion.Euler(-angleSignX * verticalAngleOffset,
                        transform.eulerAngles.y, transform.eulerAngles.z);
                    transform.rotation = Quaternion.Slerp(transform.rotation, newTargetRotation, Time.deltaTime * 10);
                }
            }
        }
        else
        {
            transform.localEulerAngles = new Vector3(90, 0, 0);
        }
    }

#if UNITY_EDITOR
  /*  private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * shootDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Character.Instance.transform.position, Character.Instance.transform.forward * hideRifleDistance);
    }*/
#endif
}