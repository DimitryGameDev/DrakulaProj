using UnityEngine;

public class InvisibleDoor : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;

    private Collider colliderObj;

    private void Start()
    {
        colliderObj = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == Character.Instance.GetComponentInChildren<Collider>())
            WallVisible();
    }

    private void WallVisible()
    {
        //if(wall == null) return;

        if (CharacterInputController.Instance.HeartEnabled)
        {
            for (int i = 0; i < walls.Length; i++)
            {
                Destroy(walls[i]);
            }

            Destroy(colliderObj);
        }
    }
}