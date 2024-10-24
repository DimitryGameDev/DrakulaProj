using System;
using UnityEngine;

public enum MoveType
{
    Walk,
    Run,
    Sit,
    Air
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterInputController))] 
public class Character : MonoBehaviour
{/*
    // TO DO:
    [Range(1f,10f)][SerializeField] private float maxSpeedSit = 1f;
    [Range(0f, 50f)][SerializeField] private float jumpForce = 1f;
    */
    [Header("Character Settings")]
    [Range(1f,10f)][SerializeField] private float maxSpeedWalk = 3f;
    [Range(1f,10f)][SerializeField] private float maxSpeedRun = 6f;
    [SerializeField] private OnePersonCamera cameraMain; 
    public OnePersonCamera Camera => cameraMain;
    private Rigidbody rb;
    private Vector3 moveVector;

    private void Start()
    {
        cameraMain.SetTarget(transform,TypeMoveCamera.WithRotation);
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }

    private const float Acceleration = 2000f;
    private const float AirMoveLimit = 0.300f;
    
    public void Move(Vector3 direction,MoveType moveType)
    {
        
        if (direction.magnitude > 1) direction /= 2;
        
        if (direction == Vector3.zero && moveType != MoveType.Air)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        
        rb.AddRelativeForce(direction * (Acceleration * Time.deltaTime),ForceMode.Acceleration);
        
        switch (moveType)
        {
            case MoveType.Sit:
            {  
                /*
                if (rb.velocity.magnitude >= maxSpeedSit)
                {
                    rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeedSit);
                }
                */
                break;
            }
            
            case MoveType.Walk:
            {
                if (rb.velocity.magnitude >= maxSpeedWalk)
                {
                    rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeedWalk);
                }

                break;
            }
            case MoveType.Run:
            {
                if (rb.velocity.magnitude >= maxSpeedRun)
                {
                    rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeedRun);
                }

                break;
            }
            case MoveType.Air:
                rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x,0,AirMoveLimit), rb.velocity.y, Mathf.Lerp(rb.velocity.z,0,AirMoveLimit));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(moveType), moveType, null);
        }
    }
    
    private void CharacterRotate()
    {
        transform.rotation = new Quaternion(0, cameraMain.transform.rotation.y,0, cameraMain.transform.rotation.w);
    }
    
    
    public void CameraMove(float dirX,float dirY)
    {
        cameraMain.Rotate(dirX, dirY);
        CharacterRotate();
    }
    
    /*
    public void Jump()
    {
        rb.AddRelativeForce(transform.up * jumpForce , ForceMode.Impulse);
    }
    */
    public void SetCamera(OnePersonCamera cameraForPlayer)
    {
        cameraMain = cameraForPlayer;
    }
}
