using UnityEngine;

public class ClosedDoor : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other == Character.Instance.GetComponentInChildren<Collider>())
        panel.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == Character.Instance.GetComponentInChildren<Collider>())
            panel.SetActive(false);
    }
}
