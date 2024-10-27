using UnityEngine;
using UnityEngine.Serialization;

public enum TypeMoveCamera
{
    None,
    OnlyMove,
    WithRotation
}

public class OnePersonCamera : MonoBehaviour
{
    [Range(0.1f, 9f)][SerializeField] private float sensitivity = 2f;
    [Range(0f, 90f)][SerializeField] private float yRotationLimit = 88f;
    [Range(6f, 10f)][SerializeField] private float cameraSmothMoveSpead = 8f;
    [Range(1f, 5f)][SerializeField] private float cameraSmothRotateSpead = 5f;
    [SerializeField] private float cameraOffsetY = 2f;
    
    public TypeMoveCamera typeMove;
    private Transform target;
    private Camera mainCamera;
    private Vector2 rotation;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        transform.parent = null;
    }

    public void Rotate(float dirX, float dirY)
    {
        if (typeMove == TypeMoveCamera.None)
        {
            return;
        }

        rotation.x += dirX * sensitivity;
        rotation.y += dirY * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.position = target.position + new Vector3(0, cameraOffsetY, 0);
        transform.localRotation = xQuat * yQuat;
    }

    private void Update()
    {
        if (typeMove == TypeMoveCamera.None)
        {
            return;
        }

        if (typeMove == TypeMoveCamera.WithRotation)
        {
            if (transform.position != target.transform.position + new Vector3(0, cameraOffsetY) || transform.rotation != target.transform.rotation)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position + new Vector3(0, cameraOffsetY), Time.deltaTime * cameraSmothMoveSpead);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, cameraSmothRotateSpead);
            }
            /*else
            {
                rotation.x = target.rotation.eulerAngles.y;
                rotation.y = target.rotation.eulerAngles.x;
                typeMove = TypeMoveCamera.None;
            }*/
        }

        if (typeMove == TypeMoveCamera.OnlyMove)
        {
            if (transform.position != target.transform.position + new Vector3(0, cameraOffsetY))
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position + new Vector3(0, cameraOffsetY), Time.deltaTime * cameraSmothMoveSpead);
            }
            /*else
            {
                typeMove = TypeMoveCamera.None;
            }*/
        }

    }
    
    public void SetTarget(Transform targetForCamera,TypeMoveCamera typeMoveCamera)
    {
        typeMove = typeMoveCamera;
        target = targetForCamera;
    }
}
