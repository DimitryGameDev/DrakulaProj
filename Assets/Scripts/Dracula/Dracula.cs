using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PathBuilder))]
public class Dracula : SingletonBase<Dracula>
{
    
    [Space] [Header("Dracula Settings")] 
    
    [Tooltip("Запуск Дракулы со старта")]
    [SerializeField] private bool playOnAwake = true;
    
    [Tooltip("Максимальная скорость перемещения")]
    [SerializeField] [Range(0.2f, 30f)] private float maxSpawnSpeed = 15f;
    
    [Tooltip("Изменение максимальной скорости на данное значение при подборе амулета")]
    [SerializeField] [Range(1, 30)] private int maxSpeedChange = 2;
    
    [Tooltip("минимальная скорость перемещения")]
    [SerializeField] [Range(0.2f, 30f)] private float minSpawnSpeed = 1f;
    
    [Tooltip("Дальность перемещения")]
    [SerializeField] [Range(0f, 50f)] private int minDistanceToNextPp = 4;
    
    [SerializeField] private AudioClip[] spawnClips;
    [SerializeField] private DraculaPoint[] spawnPositions;
    
    [Space][Header("Dracula Prefabs")]
    [SerializeField] private GameObject draculaPrefabNone;
    [SerializeField] private GameObject draculaPrefabSexy;
    [SerializeField] private GameObject draculaPrefabCross;
    [SerializeField] private GameObject draculaPrefabStand;
    [SerializeField] private GameObject draculaPrefabFly;
    [SerializeField] private GameObject draculaPrefabHand;
    
    [Space][Header("Nosferatu Prefabs")]
    [SerializeField] private GameObject nosferatuPrefabNone;
    [SerializeField] private GameObject nosferatuPrefabSexy;
    [SerializeField] private GameObject nosferatuPrefabCross;
    [SerializeField] private GameObject nosferatuPrefabStand;
    [SerializeField] private GameObject nosferatuPrefabFly;
    [SerializeField] private GameObject nosferatuPrefabHand;
    
    [Space][Header("Visual Prefabs")]
    [SerializeField] private DraculaSpawnEffect draculaSpawnEffectPrefab;
    [SerializeField] private ImpactEffect visionEffectPrefab;

    private GameObject prefabsNone;
    private GameObject prefabsSexy;
    private GameObject prefabsCross;
    private GameObject prefabsStand;
    private GameObject prefabsFly;
    private GameObject prefabsHand;
    
    private Transform character;
    private DraculaPoint draculaPoint;
    private DraculaPoint playerPoint;
    private GameObject draculaPrefab;
    private AudioSource source;
    private MeshRenderer draculaMeshRenderer;
    private DraculaSpawnEffect draculaSpawnEffect;
    private PathBuilder builder;
    
    private Vector3 lastPosition;
    private bool isHeart;
    private bool isVisible;
    private bool isSpawning;
    private bool isActiveMesh;
    private bool isNewPhase;
    
    private float timer;
    private float spawnSpeed;
    private int lastValue;
    
    [HideInInspector] public UnityEvent<int> draculaInPlayer;
    
    private void ToggleVisionOn() => isVisible = true;
    private void ToggleVisionOff() => isVisible = false;
    private void ToggleHeartOn() => isHeart = true;
    private void ToggleHeartOff() => isHeart = false;
    
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        CharacterInputController.Instance.heartOn.AddListener(ToggleHeartOn);
        CharacterInputController.Instance.heartOff.AddListener(ToggleHeartOff);
        GetComponentInChildren<VisibleObject>().onVision.AddListener(ToggleVisionOn);
        GetComponentInChildren<VisibleObject>().onHide.AddListener(ToggleVisionOff);
        
        NoiseLevel.Instance.OnChange += SpeedChange;
        
        character = Character.Instance.transform;
        source = GetComponent<AudioSource>();
        builder = GetComponent<PathBuilder>();
        playerPoint = character.GetComponent<DraculaPoint>();
        character.GetComponent<Bag>().addMedalPieceAmount.AddListener(PlayerFindMedal);
        draculaPoint = GetComponent<DraculaPoint>();
        spawnSpeed = maxSpawnSpeed;
        
        ChangePrefabs(1);
        if (playOnAwake)
        {
            SetSpawnPoint(spawnPositions[Random.Range(0, spawnPositions.Length)]);
        }
        else
        {
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (draculaSpawnEffect != null && draculaSpawnEffect.IsPlaying() && !isNewPhase)
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KillPlayer(3);
        }
    }
    
    private void DraculaMove()
    {
        Destroy(draculaPrefab);
        
        var movePoint = builder.GetDraculaPoint(draculaPoint,playerPoint,minDistanceToNextPp);

        if (movePoint == null)
        {
            Debug.Log("Игрок вне досягаемости");
            return;
        }
        
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

    private void PlayerFindMedal()
    {
        FindNewPath();
        MaxSpeedChange();
        if (character.GetComponent<Bag>().GetMedalPeaceAmount() >= 3)
        {
            ActivateNewPhase();
        }
    }

    private void ActivateNewPhase()
    {
        ChangePrefabs(2);
        isNewPhase = true;
    }

    private void ChangePrefabs(int phaseNumber)
    {
        if (phaseNumber == 1)
        {
            prefabsNone = draculaPrefabNone;
            prefabsSexy = draculaPrefabSexy;
            prefabsCross = draculaPrefabCross;
            prefabsStand = draculaPrefabStand;
            prefabsFly = draculaPrefabFly;
            prefabsHand = draculaPrefabHand;
        }
        if (phaseNumber == 2)
        {
            prefabsNone = nosferatuPrefabNone;
            prefabsSexy = nosferatuPrefabSexy;
            prefabsCross = nosferatuPrefabCross;
            prefabsStand = nosferatuPrefabStand;
            prefabsFly = nosferatuPrefabFly;
            prefabsHand = nosferatuPrefabHand;
        }
    }

    private void DraculaState()
    {
        VisibleMeshDracula();
        DraculaRotateToPlayer();
        DraculaEffect();
    }
    
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
        if (spawnPositions.Length !=0)
        {
            DraculaPoint rand = spawnPositions[Random.Range(0, spawnPositions.Length)];
            transform.position = rand.transform.position;
            draculaPoint = rand;
            Spawn(rand);
        }
    }
    
    public void SetSpawnPoint(DraculaPoint spawnPoint)
    {
        transform.position = spawnPoint.transform.position;
        draculaPoint = spawnPoint;
        Spawn(spawnPoint);
    }
    
    public void SetSpawnPoints(DraculaPoint[] spawnPoints)
    {
        spawnPositions = spawnPoints;
        DraculaPoint rand = spawnPositions[Random.Range(0, spawnPositions.Length)];
        draculaPoint = rand;
        Spawn(rand);
    }
    
    private void Spawn(DraculaPoint spawnPoint)
    {
        isSpawning = true;
        //source.PlayOneShot(spawnClips[Random.Range(0,spawnClips.Length)]);
        draculaPrefab = Instantiate(GetDraculaPrefab(spawnPoint), transform.position, Quaternion.identity, transform);
        draculaMeshRenderer = draculaPrefab.GetComponent<MeshRenderer>();
        draculaMeshRenderer.enabled = false;
        enabled = true;
    }
    public void DraculaDespawn()
    {
        DraculaDisable();
        builder.ClearPath();
        isSpawning = false;
        enabled = false;
    }
    public void DraculaEnable()
    {
        if (isSpawning)
        {
            transform.position = lastPosition;
            enabled = true;
        }
    }
    public void DraculaDisable()
    {
        lastPosition = transform.position;
        transform.position =new Vector3(100,100,100);
        timer = 0;
        enabled = false;
    }
    private void SpeedChange(int value)
    {
        if (value == 0)
        {
            lastValue = 0;
            spawnSpeed = maxSpawnSpeed;
            return;
        }
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

    private void MaxSpeedChange()
    {
        maxSpawnSpeed -= maxSpeedChange;
        spawnSpeed = maxSpawnSpeed;
        var changeSpeed = maxSpawnSpeed / NoiseLevel.Instance.MaxLevel;

        for (int i = 0; i < lastValue; i++)
        {
            spawnSpeed -= changeSpeed;
        }
        if (spawnSpeed <= minSpawnSpeed) spawnSpeed = minSpawnSpeed;
    }

    private void FindNewPath()
    { 
        builder.ResetPath();
    }
    public void DraculaIndestructible(float time)
    {
        StartCoroutine(TemporaryShutdown(time));
    }

    private IEnumerator TemporaryShutdown(float time)
    {
        DraculaDisable();
        yield return new WaitForSeconds(time);
        Debug.Log("Dracula enable again");
        DraculaEnable();
    }
    
    private GameObject GetDraculaPrefab(DraculaPoint currentPoint)
    {
        var currentDraculaPrefab = prefabsNone;
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
            && prefabsSexy != null) currentDraculaPrefab = prefabsSexy;
        if (posType == DraculaPosType.Stand 
            && prefabsStand != null) currentDraculaPrefab = prefabsStand;
        if (posType == DraculaPosType.Cross 
            && prefabsCross != null) currentDraculaPrefab = prefabsCross;
        if (posType == DraculaPosType.Hand 
            && prefabsHand != null) currentDraculaPrefab = prefabsHand;
        if (posType == DraculaPosType.Fly 
            && prefabsFly != null) currentDraculaPrefab = prefabsFly;
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
    private void OnDestroy()
    {
        CharacterInputController.Instance.draculaAnim.RemoveListener(ToggleHeartOn);
        CharacterInputController.Instance.draculaAnim.RemoveListener(ToggleHeartOff);
        NoiseLevel.Instance.OnChange -= SpeedChange;
    }

}