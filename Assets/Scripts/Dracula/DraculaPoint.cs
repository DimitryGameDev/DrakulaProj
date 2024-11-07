using UnityEngine;

public class DraculaPoint : PatrolPoint
{
    [SerializeField] private DraculaPosType draculaPos;
    [SerializeField] private bool isPlayer;
    public bool IsPlayer => isPlayer;
    public DraculaPosType DraculaPos => draculaPos;

    private void OnDrawGizmosSelected()
    {
        if (!isPlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 4f);
        }
    }
}
