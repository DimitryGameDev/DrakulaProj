using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    private InteractiveObject saveZone;

    private void Start()
    {
        saveZone = transform.parent.GetComponent<InteractiveObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        saveZone.EnterTrigger();
    }

    private void OnTriggerExit(Collider other)
    {
        saveZone.ExitTrigger();
    }

    private void OnTriggerStay(Collider other)
    {
        saveZone.StayTrigger();
    }
}
