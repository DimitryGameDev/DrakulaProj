using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medal : MonoBehaviour
{
    [SerializeField] private GameObject view;
    [SerializeField] private int countPiece = 1;
    
    public void PickUp()
    {
        Destroy(view);
        Player.Instance.AddMedalPiece(countPiece);
    }
}
