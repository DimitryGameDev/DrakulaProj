using UnityEngine;

[RequireComponent(typeof(InteractiveObject))]
public class Medal : MonoBehaviour
{
    [SerializeField] private int countPiece;
    [SerializeField] private GameObject impactEffect;

    private InteractiveObject interactiveObject;
    
    private void Start()
    {
        interactiveObject = GetComponent<InteractiveObject>();
        interactiveObject.onUse.AddListener(PickUp);
    }

    private void PickUp()
    {
        Character.Instance.GetComponent<Bag>().AddMedalPiece(countPiece);
        
        if (impactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        interactiveObject.onUse.RemoveListener(PickUp);
    }
}
