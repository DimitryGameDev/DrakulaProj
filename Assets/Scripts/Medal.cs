using UnityEngine;

public class Medal : MonoBehaviour
{
    [SerializeField] private GameObject view;
    [SerializeField] private int countPiece = 1;
    
    public void PickUp()
    {
        Destroy(view);
        Character.Instance.GetComponent<Bag>().AddMedalPiece(countPiece);
    }
}
