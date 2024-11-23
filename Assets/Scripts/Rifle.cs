using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Rifle : MonoBehaviour
{
    [SerializeField] private GameObject visualModel;
    [SerializeField] private Transform lookAtTarget;

    [Range(0, 180)] [SerializeField] private float verticalAngleOffset;

    [SerializeField] private float timeIndestructible;
    [SerializeField] private float rayDistance;

    [SerializeField] private ParticleSystem muzzleParticleSystem;

    [SerializeField] private AudioClip shot;
    [SerializeField] private AudioClip click;

    private AudioSource audioSource;
    private Camera mainCamera;

    private bool isActive;

    private void Start()
    {
        if (!CharacterInputController.Instance.IsRiflePickup)
            visualModel.SetActive(false);

        CharacterInputController.Instance.rifleOn.AddListener(OnRifleActive);
        CharacterInputController.Instance.rifleOff.AddListener(OnRifleDeactivate);
        CharacterInputController.Instance.rifleShoot.AddListener(Fire);

        audioSource = GetComponent<AudioSource>();
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        LookAtTarget();
    }

    private void OnRifleActive()
    {
        if (!CharacterInputController.Instance.IsRiflePickup) return;

        isActive = true;
        visualModel.SetActive(true);
    }

    private void OnRifleDeactivate()
    {
        if (!CharacterInputController.Instance.IsRiflePickup) return;

        isActive = false;
        visualModel.SetActive(false);
    }

    private void Fire()
    {
        if (!isActive) return;

        if (Character.Instance.GetComponent<Bag>().DrawProjectile(1) == false)
        {
            audioSource.PlayOneShot(click);
        }
        else
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward * rayDistance, out hit))
            {
                if (hit.collider.GetComponentInParent<VisibleObject>())
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
        if (!isActive) return;

        float currentXRotation = mainCamera.transform.localEulerAngles.x;

        if (currentXRotation < 180 && currentXRotation <= verticalAngleOffset ||
            currentXRotation >= 180 && currentXRotation >= 360 - verticalAngleOffset)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookAtTarget.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);

            //transform.LookAt(lookAtTarget);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
    }
#endif
}