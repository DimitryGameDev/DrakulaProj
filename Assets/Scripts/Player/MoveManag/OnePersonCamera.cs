using UnityEngine;

public class OnePersonCamera : SingletonBase<OnePersonCamera>
{
    [Range(0.1f, 9f)][SerializeField] private float sensitivity = 2f;
    [Range(0f, 90f)][SerializeField] private float yRotationLimit = 88f;
    [Range(6f, 100f)][SerializeField] private float cameraSmoothMoveSpeed = 8f;
    [Range(1f, 50f)][SerializeField] private float cameraSmoothRotateSpeed = 5f;
    
    public TypeMoveCamera typeMove;
    private Transform target;
    
    private Vector2 rotation;
    private bool isLocked;
    public bool IsLocked => isLocked;
    private void Awake()
    {
        Init();
        
    }

    private void Start()
    {
        transform.parent = null;
        enabled = false;
    }

    public void Move(float dirX, float dirY)
    {
        if (enabled || isLocked) return;
        
        rotation.x += dirX * sensitivity;
        rotation.y += dirY * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.position = target.position;
        transform.localRotation = xQuat * yQuat;
    }

    private void Update()
    {
        if (typeMove == TypeMoveCamera.None)
        {
            
        }

        if (typeMove == TypeMoveCamera.WithRotation)
        {
            if (transform.position != target.transform.position || transform.rotation != target.transform.rotation)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * cameraSmoothMoveSpeed);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, cameraSmoothRotateSpeed);
            }
            else
            {
                rotation.x = target.rotation.eulerAngles.y;
                rotation.y = target.rotation.eulerAngles.x;
                typeMove = TypeMoveCamera.None;
                enabled = false;
            }
        }

        if (typeMove == TypeMoveCamera.OnlyMove)
        {
            if (transform.position != target.transform.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position , Time.deltaTime * cameraSmoothMoveSpeed);
            }
            else
            {
                typeMove = TypeMoveCamera.None;
                enabled = false;
            }
        }
    }

    public void Lock()
    {
        isLocked = true;
    }
    
    public void UnLock()
    {
        isLocked = false;
    }
    public void SetTarget(Transform targetForCamera,TypeMoveCamera typeMoveCamera)
    {
        typeMove = typeMoveCamera;
        target = targetForCamera;
        enabled = true;
    }
    
    public void SetTarget(Transform targetForCamera,TypeMoveCamera typeMoveCamera,bool cameraLock)
    {
        isLocked = cameraLock;
        typeMove = typeMoveCamera;
        target = targetForCamera;
        enabled = true;
    }
    
}
