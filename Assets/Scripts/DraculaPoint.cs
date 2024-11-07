using System;
using UnityEngine;

public class DraculaPoint : PatrolPoint
{
    [SerializeField] private DraculaPosType draculaPos;
    [SerializeField] private bool isPlayer;
    public bool IsPlayer { get => isPlayer; }
    public DraculaPosType DraculaPos => draculaPos;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 4f);
    }
}
