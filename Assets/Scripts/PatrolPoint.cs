using UnityEngine;


public enum DraculaPosType
{
    T,
    Sit,
    Stay,
    Up
}

public class PatrolPoint : MonoBehaviour
{
    
    [SerializeField] private float radius;
    [SerializeField] private DraculaPosType draculaPos;
    public DraculaPosType DraculaPos => draculaPos;
    
    public float Radius => radius;

    private static readonly Color Gizmocolor = new(1, 0, 0, 0.3f);

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Gizmocolor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}


