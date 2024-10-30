using UnityEngine;

[RequireComponent(typeof(InteractiveObject))]
public class HeartPickUp : MonoBehaviour
{
    [SerializeField] private GameObject impactEffect;
    
    private InteractiveObject interactiveObject;

    private void Start()
    {
        interactiveObject = GetComponent<InteractiveObject>();
        interactiveObject.onUse.AddListener(PickUp);
    }
    
    private void PickUp()
    {
        CharacterInputController.Instance.PickUpHeart = true;

        if (impactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        interactiveObject.onUse.RemoveListener(PickUp);
    }
}
