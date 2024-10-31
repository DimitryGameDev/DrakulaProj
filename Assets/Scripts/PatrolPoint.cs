using UnityEngine;


public enum DraculaPosType
{
    None,
    Sexy,
    Fly,
    Stand,
    Hand,
    Cross
    
}

public class PatrolPoint : MonoBehaviour
{
    [SerializeField] private DraculaPosType draculaPos;
    [Space]
    [Header("Visual settings")]
    [SerializeField] private float radius;
    [SerializeField] private Color Gizmocolor = new(1, 0, 0, 0.3f);
    public DraculaPosType DraculaPos => draculaPos;
    
    public float Radius => radius;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Gizmocolor;
        Gizmos.DrawSphere(transform.position, radius);
        /*
        var parent = transform.parent;
        for (int i = 0; i < parent.childCount; i++)
        {
            Gizmos.DrawSphere(parent.GetChild(i).transform.position, radius);
        }*/
    }
}


