using UnityEngine;
public class PatrolPoint : MonoBehaviour
{
    [SerializeField] private float radius;
    public float Radius => radius;

    private static readonly Color Gizmocolor = new(1, 0, 0, 0.3f);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Gizmocolor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}


