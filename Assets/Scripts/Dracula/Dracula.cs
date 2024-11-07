using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(VisibleObject))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PathBuilder))]
public class Dracula : SingletonBase<Dracula>
{
    [SerializeField] private bool playOnAwake = true;
    [Space][Header("Dracula Prefabs")]
    [SerializeField] private GameObject draculaPrefabsNone;
    [SerializeField] private GameObject draculaPrefabsSexy;
    [SerializeField] private GameObject draculaPrefabsCross;
    [SerializeField] private GameObject draculaPrefabsStand;
    [SerializeField] private GameObject draculaPrefabsFly;
    [SerializeField] private GameObject draculaPrefabsHand;
    
    [Space][Header("Visual Prefabs")]
    [SerializeField] private DraculaSpawnEffect draculaSpawnEffectPrefab;
    [SerializeField] private ImpactEffect visionEffectPrefab;

    [Space] [Header("Dracula Settings")] 
    [SerializeField] [Range(0.2f, 30f)] private float maxSpawnSpeed = 10f;
    [SerializeField] [Range(0.2f, 30f)] private float minSpawnSpeed = 1f;
    [SerializeField] [Range(0f, 50f)] private int minDistanceToNextPp = 4;
    [SerializeField] private AudioClip[] spawnClips;
    [SerializeField] private DraculaPoint[] spawnPositions;
    
    private Transform character;
    private DraculaPoint draculaPoint;
    private DraculaPoint playerPoint;
    private GameObject draculaPrefab;
    private AudioSource source;
    private MeshRenderer draculaMeshRenderer;
    private DraculaSpawnEffect draculaSpawnEffect;
    private PathBuilder builder;
    private float timer;
    private float spawnSpeed;
    
    private bool isHeart;
    private bool isVisible;
    private bool isSpawning;

    [HideInInspector] public UnityEvent<int> draculaInPlayer;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        CharacterInputController.Instance.heartOn.AddListener(ToggleHeartOn);
        CharacterInputController.Instance.heartOff.AddListener(ToggleHeartOff);
        GetComponent<VisibleObject>().onVision.AddListener(ToggleVisionOn);
        GetComponent<VisibleObject>().onHide.AddListener(ToggleVisionOff);
        
        NoiseLevel.Instance.OnChange += SpeedChange;
        
        character = Character.Instance.transform;
        source = GetComponent<AudioSource>();
        builder = GetComponent<PathBuilder>();
        playerPoint = character.GetComponent<DraculaPoint>();
        draculaPoint = GetComponent<DraculaPoint>();
        spawnSpeed = maxSpawnSpeed;
        
        if (playOnAwake)
        {
            SetPoint(spawnPositions[Random.Range(0, spawnPositions.Length)]);
        }
        else
        {
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KillPlayer(3);
        }
    }

    private void OnDestroy()
    {
        CharacterInputController.Instance.draculaAnim.RemoveListener(ToggleHeartOn);
        CharacterInputController.Instance.draculaAnim.RemoveListener(ToggleHeartOff);
        NoiseLevel.Instance.OnChange -= SpeedChange;
    }

    private int lastValue;
    private void SpeedChange(int value)
    {
        var changeSpeed = maxSpawnSpeed / NoiseLevel.Instance.MaxLevel;
        
        if (lastValue < value)
        {
            if (spawnSpeed - changeSpeed >= minSpawnSpeed) spawnSpeed -= changeSpeed;
            else spawnSpeed = minSpawnSpeed;
        }

        if (lastValue > value)
        {
            if (spawnSpeed + changeSpeed <= maxSpawnSpeed) spawnSpeed += changeSpeed;
            else spawnSpeed = maxSpawnSpeed;
        }
        lastValue = value;
    }

    private void FixedUpdate()
    {
       
        if (draculaSpawnEffect != null && draculaSpawnEffect.IsPlaying())
        {
            DraculaState();
            return;
        }
        
        timer += Time.deltaTime;
        if (timer >= spawnSpeed)
        {
            DraculaMove();
            timer = 0;
        }
        DraculaState();
    }

    private void DraculaState()
    {
        VisibleMeshDracula();
        DraculaRotateToPlayer();
        DraculaEffect();
    }
    
    private void ToggleVisionOn() => isVisible = true;
    private void ToggleVisionOff() => isVisible = false;
    private void ToggleHeartOn() => isHeart = true;
    private void ToggleHeartOff() => isHeart = false;

    private bool isActiveMesh;
    private void VisibleMeshDracula()
    {
        if (draculaMeshRenderer != null)
        {
            if (isVisible && isHeart || isHeart && isActiveMesh)
            {
                if (draculaMeshRenderer.enabled == false)
                {
                    isActiveMesh = true;
                    draculaMeshRenderer.enabled = true;
                    Instantiate(visionEffectPrefab,transform.position,Quaternion.identity);
                }
            }
            else
            {
                if (draculaMeshRenderer.enabled)
                {
                    isActiveMesh = false;
                    draculaMeshRenderer.enabled = false;
                    Instantiate(visionEffectPrefab,transform.position,Quaternion.identity);
                }
            }
        }
    } 
    private void DraculaEffect()
    {
        if (isVisible && isHeart)
        {
            if (draculaSpawnEffect == null)
            {
                draculaSpawnEffect = Instantiate(draculaSpawnEffectPrefab,new Vector3(transform.position.x,transform.position.y,transform.position.z), Quaternion.identity);
            }
            else if (!draculaSpawnEffect.IsPlaying())
            {
                draculaSpawnEffect = Instantiate(draculaSpawnEffectPrefab,new Vector3(transform.position.x,transform.position.y,transform.position.z), Quaternion.identity);
            }
        }
    }
    private void DraculaRotateToPlayer() 
    {
        if (draculaPrefab != null)
        {
            draculaPrefab.transform.LookAt(new Vector3(character.position.x,transform.position.y,character.position.z));
        }
    }
    public void RandomPoint()
    {
        DraculaPoint rand = spawnPositions[Random.Range(0, spawnPositions.Length)];
        transform.position = rand.transform.position;
        Spawn(rand);
    }
    
    public void SetPoint(DraculaPoint spawnPoint)
    {
        transform.position = spawnPoint.transform.position;
        draculaPoint = spawnPoint;
        Spawn(spawnPoint);
    }
    
    public void SetPoints(DraculaPoint[] spawnPoints)
    {
        spawnPositions = spawnPoints;
        DraculaPoint rand = spawnPositions[Random.Range(0, spawnPositions.Length)];
        draculaPoint = rand;
        Spawn(rand);
    }
    
    private void Spawn(DraculaPoint spawnPoint)
    {
        isSpawning = true;
        source.PlayOneShot(spawnClips[Random.Range(0,spawnClips.Length)]);
        draculaPrefab = Instantiate(GetDraculaPrefab(spawnPoint), transform.position, Quaternion.identity, transform);
        draculaMeshRenderer = draculaPrefab.GetComponent<MeshRenderer>();
        draculaMeshRenderer.enabled = false;
        enabled = true;
    }

    public void DraculaEnable()
    {
        if (isSpawning)
        {
            transform.position = lastPosition;
            enabled = true;
        }
    }
    
    private Vector3 lastPosition;
    public void DraculaDisable()
    {
        lastPosition = transform.position;
        transform.position = Vector3.zero;
        timer = 0;
        enabled = false;
    }
    public void DraculaDespawn()
    {
        DraculaDisable();
        builder.ClearPath();
        isSpawning = false;
    }
    
    private void DraculaMove()
    {
        Destroy(draculaPrefab);
        
        var movePoint = builder.GetDraculaPoint(draculaPoint,playerPoint,minDistanceToNextPp);

        if (movePoint == null) return;
        
        if (movePoint.IsPlayer)
        {
            KillPlayer(1);
            enabled = false;
            return;
        }

        draculaPrefab = Instantiate(GetDraculaPrefab(movePoint), movePoint.transform.position, Quaternion.identity, transform);
        draculaMeshRenderer = draculaPrefab.GetComponent<MeshRenderer>();
        draculaMeshRenderer.enabled = false;
        draculaPoint = movePoint;
    }

    private GameObject GetDraculaPrefab(DraculaPoint currentPoint)
    {
        var currentDraculaPrefab = draculaPrefabsNone;
        var posType = currentPoint.DraculaPos;
        
        if (posType == DraculaPosType.None && currentDraculaPrefab != null)
        {
            var rand = Random.Range(0,3);
            if (rand == 0) posType = DraculaPosType.Fly;
            if (rand == 1) posType = DraculaPosType.Stand;
            if (rand == 2) posType = DraculaPosType.Cross;
            if (rand == 3) posType = DraculaPosType.Hand ; 
        }
        
        if (posType == DraculaPosType.Sexy 
            && draculaPrefabsSexy != null) currentDraculaPrefab = draculaPrefabsSexy;
        if (posType == DraculaPosType.Stand 
            && draculaPrefabsStand != null) currentDraculaPrefab = draculaPrefabsStand;
        if (posType == DraculaPosType.Cross 
            && draculaPrefabsCross != null) currentDraculaPrefab = draculaPrefabsCross;
        if (posType == DraculaPosType.Hand 
            && draculaPrefabsHand != null) currentDraculaPrefab = draculaPrefabsHand;
        if (posType == DraculaPosType.Fly 
            && draculaPrefabsFly != null) currentDraculaPrefab = draculaPrefabsFly;
        transform.position = currentPoint.transform.position;

        return currentDraculaPrefab;
    }
    private void KillPlayer(int animNumber)
    {
        draculaInPlayer.Invoke(animNumber);
        enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minDistanceToNextPp);
    }

}