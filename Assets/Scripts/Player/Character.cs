using UnityEngine;

public enum MoveType
{
    Walk,
    Run
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterInputController))] 
public class Character : MonoBehaviour
{
    //Настройки для веса rigidbody 15kg
    [Range(2f,10f)][SerializeField] private float maxSpeedWalk = 3f;
    [Range(2f,10f)][SerializeField] private float maxSpeedRun = 6f;
    [Range(10f, 800f)][SerializeField] private float accelerationWalk = 500f;
    [Range(10f, 1600f)][SerializeField] private float accelerationRun = 1000f;
    [Range(0f, 50f)][SerializeField] private float jumpForse = 1f;

    [SerializeField] private OnePersonCamera cameraMain;
    public OnePersonCamera Camera => cameraMain;

    private Rigidbody rb;
    private Vector3 moveVector;

    private void Start()
    {
        cameraMain.SetTarget(this.transform,TypeMoveCamera.WithRotation);
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public void Move(Vector3 direction,bool ground,MoveType moveType)
    {
        
        if (!ground) direction /= 4;
        if (direction.magnitude > 1) direction /= 2;
        /*
        if (direction == Vector3.zero && ground)
        {
            rb.velocity = Vector3.zero;
        }
        */
        
        if (moveType == MoveType.Walk)
        {
            if (rb.velocity.magnitude >= maxSpeedWalk)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeedWalk);
                return;
            }
            rb.AddRelativeForce(direction * accelerationWalk);
        }
        
        if (moveType == MoveType.Run)
        {
            if (rb.velocity.magnitude >= maxSpeedRun)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeedRun);
                return;
            }
            rb.AddRelativeForce(direction * accelerationRun);
        }
    }
    
    private void AdminRotate()
    {
        transform.rotation = new Quaternion(0, cameraMain.transform.rotation.y,0, cameraMain.transform.rotation.w);
    }

    public void Jump()
    {
        rb.AddForce(transform.up * jumpForse , ForceMode.Impulse);
    }

    public void CameraMove(float dirX,float dirY)
    {
        cameraMain.Rotate(dirX, dirY);
        AdminRotate();
    }

    public void SetCamera(OnePersonCamera cameraForPlayer)
    {
        cameraMain = cameraForPlayer;
    }

    
}
