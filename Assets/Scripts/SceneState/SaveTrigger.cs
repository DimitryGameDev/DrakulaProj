using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    private SaveZone saveZone;

    private void Start()
    {
        saveZone = transform.parent.GetComponent<SaveZone>();
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
