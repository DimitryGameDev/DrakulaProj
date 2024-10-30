using UnityEngine;

[RequireComponent(typeof(InteractiveObject))]
public class FloorTrigger : MonoBehaviour
{
    [SerializeField] private Transform teleportPosition;
    private InteractiveObject interactiveObject;
    private void Start()
    {
        interactiveObject = GetComponent<InteractiveObject>();
        interactiveObject.onUse.AddListener(Teleport);
    }

    private void Teleport()
    {
        Character.Instance.transform.position = teleportPosition.position;
    }
}
