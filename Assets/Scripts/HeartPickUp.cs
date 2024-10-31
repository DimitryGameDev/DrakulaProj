using UnityEngine;

[RequireComponent(typeof(InteractiveObject))]
public class HeartPickUp : MonoBehaviour
{
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject visualModel;
    
    private InteractiveObject interactiveObject;

    private void Start()
    {
        interactiveObject = GetComponent<InteractiveObject>();
        interactiveObject.onUse.AddListener(PickUp);
    }
    
    private void PickUp()
    {
        CharacterInputController.Instance.pickUpHeart = true;

        if (impactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        
        Destroy(visualModel);
    }

}
