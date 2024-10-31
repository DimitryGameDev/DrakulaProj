using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(InteractiveObject))]
[RequireComponent(typeof(AudioSource))]
public class Dracula : SingletonBase<Dracula>
{
    [SerializeField] private bool playOnAwake = true;
    [Space][Header("Dracula Prefabs")]
    [SerializeField] private GameObject startPrefab;
    [SerializeField] private GameObject draculaPrefabsStay;
    [SerializeField] private GameObject draculaPrefabsSit;
    [SerializeField] private GameObject draculaPrefabsUp;
    [SerializeField] private GameObject draculaPrefabsT;
    [SerializeField] private DraculaSpawnEffect draculaSpawnEffectPrefab;
    [SerializeField] private ImpactEffect visionEffectPrefab;

    [Space] [Header("Dracula Settings")] 
    [SerializeField] [Range(1f, 15f)]private int spawnSpeed = 4;
    [SerializeField] [Range(0f, 10f)]private int speedChange = 2;
    [SerializeField] [Range(0f, 50f)] private float minDistance = 6;
    [SerializeField] private AudioClip[] spawnClips;
    [SerializeField] private PatrolPoint[] spawnPositions;
    
    private Transform character;
    private GameObject draculaPrefab;
    private List<PatrolPoint> patrolPoints;
    private List<PatrolPoint> nearestPatrolPoints;
    private AudioSource source;
    private MeshRenderer draculaMeshRenderer;
    private DraculaSpawnEffect draculaSpawnEffect;
    private float timer;
    
    
    private bool isHeart = false;
    private bool isVisible = false;

    public UnityEvent<int> draculaInPlayer;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        CharacterInputController.Instance.heartOn.AddListener(TogleHeartOn);
        CharacterInputController.Instance.heartOff.AddListener(TogleHeartOff);
        GetComponent<InteractiveObject>().onVision.AddListener(TogleVisionOn);
        GetComponent<InteractiveObject>().onHide.AddListener(TogleVisionOff);
        NoiseLevel.Instance.OnChange += SpeedChange;
        nearestPatrolPoints = new List<PatrolPoint>();
        patrolPoints = new List<PatrolPoint>();
        
        FillPatrolPointsInScene();
        
        character = Character.Instance.transform;
        source = GetComponent<AudioSource>();
        
        if (!playOnAwake)enabled = false;
    }

    private void OnDestroy()
    {
        CharacterInputController.Instance.draculaAnim.RemoveListener(TogleHeartOn);
        CharacterInputController.Instance.draculaAnim.RemoveListener(TogleHeartOff);
        GetComponent<InteractiveObject>().onVision.RemoveListener(TogleVisionOn);
        GetComponent<InteractiveObject>().onHide.RemoveListener(TogleVisionOff);
        NoiseLevel.Instance.OnChange -= SpeedChange;
    }

    private int lastValue = 0;
    private void SpeedChange(int value)
    {
        if (lastValue > value) spawnSpeed += speedChange;
        else if (spawnSpeed - speedChange >= 0 )spawnSpeed -= speedChange;
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
    
    private void TogleVisionOn() => isVisible = true;
    private void TogleVisionOff() => isVisible = false;
    private void TogleHeartOn() => isHeart = true;
    private void TogleHeartOff() => isHeart = false;

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
                if (draculaMeshRenderer.enabled == true)
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
    public void DraculaSpawn()
    {
        transform.position = spawnPositions[Random.Range(0,spawnPositions.Length)].transform.position;
        Spawn();
       
    }
    public void DraculaSpawn(PatrolPoint spawnPoint)
    {
        transform.position = spawnPoint.transform.position;
        Spawn();
    }
    private void Spawn()
    {
        source.PlayOneShot(spawnClips[Random.Range(0,spawnClips.Length)]);
        draculaPrefab = Instantiate(startPrefab, transform.position, Quaternion.identity, transform);
        draculaMeshRenderer = draculaPrefab.GetComponent<MeshRenderer>();
        draculaMeshRenderer.enabled = false;
        enabled = true;
    }
    public void DraculaDisable()
    {
        timer = 0;
        enabled = false;
    }

    private void DraculaMove()
    {
        FindNearestPatrolPoint();

        if (nearestPatrolPoints.Count == 0) return;
        
        Destroy(draculaPrefab);
        
        var patrolPoint = FindPatrolPointsToPlayer();

        if (patrolPoint.DraculaPos == DraculaPosType.Player)
        {
            KillPlayer();
            return;
        }
        
        var prefab = draculaPrefabsT;
        
        if (patrolPoint.DraculaPos == DraculaPosType.Sit 
            && draculaPrefabsSit != null) prefab = draculaPrefabsSit;
        if (patrolPoint.DraculaPos == DraculaPosType.Stay 
            && draculaPrefabsStay != null) prefab = draculaPrefabsStay;
        if (patrolPoint.DraculaPos == DraculaPosType.Up 
            && draculaPrefabsUp != null) prefab = draculaPrefabsUp;

        transform.position = patrolPoint.transform.position;
          
        draculaPrefab = Instantiate(prefab, patrolPoint.transform.position, Quaternion.identity, transform);
        draculaMeshRenderer = draculaPrefab.GetComponent<MeshRenderer>();
        draculaMeshRenderer.enabled = false;
        CleatNearestPatrolPoint();
    
    }

    private void KillPlayer()
    {
        draculaInPlayer.Invoke(1);
        enabled = false;
    }

    private readonly Vector3 OffsetY = new Vector3(0, 0.5f, 0);
    private void FindNearestPatrolPoint()
    {
        var draculaPos = transform.position;
        
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            PatrolPoint patrolPoint = patrolPoints[i];
            var distance = Vector3.Distance(draculaPos, patrolPoint.transform.position);
            
            if (distance < minDistance)
            {   
                RaycastHit hitInfo;
                Ray ray = new Ray(transform.position + OffsetY, patrolPoint.transform.position - transform.position + OffsetY);
                
                Debug.DrawLine(transform.position + OffsetY, patrolPoint.transform.position + OffsetY, Color.blue,3f);
                
                if (Physics.Raycast(ray, out hitInfo,minDistance))
                {
                    if (hitInfo.collider.transform.parent?.GetComponent<Character>())
                    {
                        KillPlayer();
                        enabled = false;
                        return;
                    }
                    Debug.DrawLine(transform.position, patrolPoint.transform.position, Color.red, 3f);
                    continue;
                }
 
                nearestPatrolPoints.Add(patrolPoint);
            }
        }
    }
    
    private PatrolPoint FindPatrolPointsToPlayer()
    {
        var playerPos = character.transform.position;
        float minDist =  Mathf.Infinity;
        PatrolPoint spawnPoints = null;
        for (int i = 0; i < nearestPatrolPoints.Count; i++)
        {
            PatrolPoint patrolPoint = nearestPatrolPoints[i];
            var distance = Vector3.Distance(playerPos, patrolPoint.transform.position);
            if (distance < minDist)
            {
                spawnPoints = patrolPoint;
                minDist = distance;
            }
        }

        return spawnPoints;
    }

    private void CleatNearestPatrolPoint()
    {
        nearestPatrolPoints.Clear();
    }

    private void FillPatrolPointsInScene()
    {
        patrolPoints.AddRange(FindObjectsOfType<PatrolPoint>());
    }
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minDistance);
    }
    
    
}