using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent (typeof(CapsuleCollider))]
public class CharacterInputController : SingletonBase<CharacterInputController>
{
    [SerializeField] private float maxDistanseHitCamera = 1f;
    [SerializeField] private bool heartEnabled;
    public bool HeartEnabled => heartEnabled;
    
    private Character character; 
    public Character Character => character;
    
    private Vector3 playerMoveDirection;
    private float radiusCharacter;
    private float hightCharacter;

    public UnityEvent heartOn;
    public UnityEvent heartOff;

    private void Awake()
    {
        Init();
    }

    public void Start()
    {
        heartEnabled = false;
        
        character = GetComponent<Character>();
        radiusCharacter = character.GetComponent<CapsuleCollider>().radius;
        hightCharacter = character.GetComponent<CapsuleCollider>().height;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        AdminMove();
    }

    private void Update()
    { 
        MainRay();
        AdminCameraMove();
        HeartState();
    }

    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    private void AdminMove()
    {
        if (heartEnabled) return;
        
        var dirZ = Input.GetAxis(Vertical);
        var dirX = Input.GetAxis(Horizontal);
        
        var ground = IsGrounded();
        
        if (Input.GetButton("Jump") && ground)
        {
            //character.Jump();
        }
        
        playerMoveDirection = new Vector3(dirX, 0, dirZ);

        if (IsWall() && !ground) return;
        
        if (!ground)
        {
            character.Move(playerMoveDirection, MoveType.Air);
            return;
        }
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            character.Move(playerMoveDirection, MoveType.Run);
            return;
        }

        character.Move(playerMoveDirection, MoveType.Walk);
    }

    private const string XAxis = "Mouse X";
    private const string YAxis = "Mouse Y";
    private void AdminCameraMove()
    {
        var dirY = Input.GetAxis(YAxis);
        var dirX = Input.GetAxis(XAxis);
        
        character.CameraMove(dirX, dirY);
    }
    private bool IsGrounded()
    {
        RaycastHit hitLegs;
        var vectorDown = character.transform.TransformDirection(Vector3.down);
        var maxDistance = hightCharacter / 2 + 0.001f;
        
        if (Physics.Raycast(character.transform.position - new Vector3(radiusCharacter, 0, 0), vectorDown, out hitLegs, maxDistance))
        {
            Debug.DrawRay(character.transform.position - new Vector3(radiusCharacter, 0, 0), vectorDown * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position - new Vector3(-radiusCharacter, 0, 0), vectorDown, out hitLegs,maxDistance))
        {
            Debug.DrawRay(character.transform.position - new Vector3(-radiusCharacter, 0, 0), vectorDown * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position - new Vector3(0, 0, radiusCharacter), vectorDown, out hitLegs,maxDistance))
        {
            Debug.DrawRay(character.transform.position - new Vector3(0, 0, radiusCharacter), vectorDown * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position - new Vector3(0, 0, -radiusCharacter), vectorDown, out hitLegs, maxDistance))
        {
            Debug.DrawRay(character.transform.position - new Vector3(0, 0, -radiusCharacter), vectorDown * hitLegs.distance, Color.red);
            return true;
        }

        return false;
    }
    private bool IsWall()
    {
        RaycastHit hitLegs;
        var maxRadius = radiusCharacter + 0.1f;
        
        if (Physics.Raycast(character.transform.position, character.transform.TransformDirection(Vector3.forward), out hitLegs, maxRadius))
        {
            Debug.DrawRay(character.transform.position , character.transform.TransformDirection(Vector3.forward) * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position, character.transform.TransformDirection(Vector3.back), out hitLegs, maxRadius))
        {
            Debug.DrawRay(character.transform.position , character.transform.TransformDirection(Vector3.back) * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position, character.transform.TransformDirection(Vector3.right), out hitLegs, maxRadius))
        {
            Debug.DrawRay(character.transform.position, character.transform.TransformDirection(Vector3.right) * hitLegs.distance, Color.red);
            return true;
        }

        if (Physics.Raycast(character.transform.position, character.transform.TransformDirection(Vector3.left), out hitLegs, maxRadius))
        {
            Debug.DrawRay(character.transform.position, character.transform.TransformDirection(Vector3.left) * hitLegs.distance, Color.red);
            return true;
        }

        return false;
    }

    private void HeartState()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            heartEnabled = true;
            heartOn.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            heartEnabled = false;
            heartOff.Invoke();
        }
    }
    
    private void MainRay()
    {
        RaycastHit hitCamera;
 
        if (Physics.Raycast(character.Camera.transform.position, character.Camera.transform.forward, out hitCamera, maxDistanseHitCamera))
        {
           
            Debug.DrawRay(character.Camera.transform.position, character.Camera.transform.forward * hitCamera.distance, Color.yellow, 0.01f);
            if (hitCamera.collider.transform.root?.GetComponent<InteractiveObject>())
            {
                var use = hitCamera.collider.transform.root.GetComponent<InteractiveObject>();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    use.Use();
                }
            }
        }
    }

}