using UnityEngine;

public class InvisibleDoor : MonoBehaviour
{
    [SerializeField] private GameObject wall;

    private Collider collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
        Debug.Log(collider.name);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.name);

        if (other == Character.Instance.GetComponentInChildren<Collider>())
           WallVisible();
    }

    private void WallVisible()
    {
        //if(wall == null) return;

        if (CharacterInputController.Instance.HeartEnabled)
        {
            Destroy(wall);
            Destroy(collider);
        }
    }
}