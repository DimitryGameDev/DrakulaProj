using UnityEngine;

/*
public enum State
{
    None,
    AdminPlace,
    Monitor,
}
*/
[RequireComponent (typeof(CapsuleCollider))]
public class CharacterInputController : MonoBehaviour
{
    //[SerializeField] private float maxDistanseHitCamera = 1f;
    
    private Character character; 
    public Character Character => character;
    
    private Vector3 playerMoveDirection;
    private float radiusCharacter;
    private float hightCharacter;   
    
    //public State stateMove;
    public void Start()
    {
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
        AdminCameraMove();
        
        /*
        if (stateMove == State.None)
        {
            
            MainRay();
        }

        if (stateMove == State.AdminPlace)
        {
            AdminInPlaceCameraMove();
            RayInPlace();            
        }

        if (stateMove == State.Monitor)
        {
            RayInPlace();
        }  
        */
    }

    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    private void AdminMove()
    {
        
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
    
#if UNITY_STANDALONE || UNITY_EDITOR 
/*
    private void AdminInPlaceCameraMove()
    {
        float dirY = Input.GetAxis(yAxis);
        float dirX = Input.GetAxis(xAxis);

        admin.Camera.Rotate(dirX, dirY);

        if (Input.GetKeyDown(KeyCode.E))
        {
            adminPlace.StandUp();
            stateMove = State.None;
        }
    }

    private RaycastHit lastHitCamera;

    private void MainRay()
    {
        RaycastHit hitCamera;

        if (Physics.Raycast(admin.Camera.transform.position, admin.Camera.transform.forward, out hitCamera, maxDistanseHitCamera))
        {
            Debug.DrawRay(admin.Camera.transform.position, admin.Camera.transform.TransformDirection(Vector3.forward) * hitCamera.distance, Color.yellow);

            if (hitCamera.collider.transform.parent?.GetComponent<AdminPlace>())
            {
                adminPlace = hitCamera.collider.transform.parent?.GetComponent<AdminPlace>();
                adminPlace.EnableOutline();

                if (Input.GetButtonDown("Fire1"))
                {
                    adminPlace.SeatDown();
                    stateMove = State.AdminPlace;
                }
            }
        }
    }


    private void RayInPlace()
    {
        RaycastHit hitCamera;
        
        if (Physics.Raycast(admin.Camera.transform.position, admin.Camera.transform.forward, out hitCamera, maxDistanseHitCamera))
        {
            Debug.DrawRay(admin.Camera.transform.position, admin.Camera.transform.TransformDirection(Vector3.forward) * hitCamera.distance, Color.yellow);

            if (hitCamera.collider.transform?.GetComponent<MonitorController>())
            {
                monitorController = hitCamera.collider.transform?.GetComponent<MonitorController>();

                if(stateMove == State.AdminPlace)
                {
                    monitorController.EnableOutline();
                }

                if (Input.GetButtonDown("Fire1") && stateMove != State.Monitor)
                {
                    monitorController.EnterMonitor(adminPlace);
                    stateMove = State.Monitor;
                }

                if (Input.GetButtonDown("Fire2") && stateMove == State.Monitor && (Input.GetKeyDown(KeyCode.LeftAlt) == false))
                {
                    monitorController.ExitMonitor();
                    stateMove = State.AdminPlace;
                }

            }
        }
    } 
    */
#endif
}
