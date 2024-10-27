using System.Collections.Generic;
using Common;
using UnityEngine;

[RequireComponent(typeof(InteractiveObject))]
[RequireComponent(typeof(AudioSource))]
public class Dracula : MonoBehaviour
{
    [SerializeField] private bool playOnAwake = true;
    [Space][Header("Dracula Prefabs")]
    [SerializeField] private GameObject draculaPrefabsStay;
    [SerializeField] private GameObject draculaPrefabsSit;
    [SerializeField] private GameObject draculaPrefabsUp;
    [SerializeField] private GameObject draculaPrefabsT;
    [SerializeField] private ImpactEffect draculaImpactEffectPrefab;
    [SerializeField] private Material draculaMat;
    
    
    [Space] [Header("Dracula Settings")] 
    [SerializeField] [Range(0f, 10f)] private float minDistance = 5;
    [SerializeField] private int spawnRate = 4;
    [SerializeField] private AudioClip[] spawnClips;
    [SerializeField] private PatrolPoint[] spawnPositions;
    
    private Transform player;
    private GameObject draculaPrefab;
    private List<PatrolPoint> patrolPoints;
    private List<PatrolPoint> nearestPatrolPoints;
    private AudioSource source;
    private MeshRenderer draculaMeshRenderer;
    private float timeSpawnRate;
    private float timeImpactEffect;
    
    private bool isHeart = false;
    private bool isVisible = false;

    private void Start()
    {
        CharacterInputController.Instance.heartOn.AddListener(TogleHeartOn);
        CharacterInputController.Instance.heartOff.AddListener(TogleHeartOff);
        GetComponent<InteractiveObject>().onVision.AddListener(TogleVisionOn);
        GetComponent<InteractiveObject>().onHide.AddListener(TogleVisionOff);
        
        nearestPatrolPoints = new List<PatrolPoint>();
        patrolPoints = new List<PatrolPoint>();
        
        FillPatrolPointsInScene();
        
        player = Character.Instance.transform;
        source = GetComponent<AudioSource>();
        
        if (!playOnAwake)enabled = false;
    }
    
    private void FixedUpdate()
    {
        timeSpawnRate += Time.deltaTime;

        if (timeSpawnRate >= spawnRate && timeImpactEffect <= 0)
        {
            DraculaMove();
            timeSpawnRate = 0;
        }

        VisibleMeshDracula();
        DraculaRotateToPlayer();
        DraculaEffect();
    }

    private void TogleVisionOn() => isVisible = true;
    private void TogleVisionOff() => isVisible = false;
    private void TogleHeartOn() => isHeart = true;
    private void TogleHeartOff() => isHeart = false;

    private float transperant;
    private void VisibleMeshDracula()
    {
        if (draculaMat != null && draculaMeshRenderer != null)
        {
            if (isHeart)
            {
                draculaMeshRenderer.enabled = true;
                transperant = Mathf.Lerp(transperant, 1, 0.1f);
            }
            else
            {
                transperant = Mathf.Lerp(transperant, 0, 0.1f);
                if (transperant <= 0.1f)
                {
                    draculaMeshRenderer.enabled = false;
                }
            }
            draculaMat.color = new Color(draculaMat.color.r, draculaMat.color.g, draculaMat.color.b, transperant);

        }
    } 
    private void DraculaEffect()
    {
        timeImpactEffect -= Time.deltaTime;
        if (isVisible && isHeart)
        {
            if (timeImpactEffect <= 0)
            {
                Instantiate(draculaImpactEffectPrefab,new Vector3(transform.position.x,transform.position.y,transform.position.z), Quaternion.identity,null);
                timeImpactEffect = draculaImpactEffectPrefab.LifeTimer; 
            }
        }
    }
    private void DraculaRotateToPlayer() 
    {
        transform.rotation = Quaternion.LookRotation(player.position - transform.position);
    }
    public void DraculaSpawn()
    {
        transform.position = spawnPositions[Random.Range(0,spawnPositions.Length)].transform.position;
        source.PlayOneShot(spawnClips[Random.Range(0,spawnClips.Length)]);
        enabled = true;
    }
    
    public void DraculaDeSpawn()
    {
        enabled = false;
    }

    private void DraculaMove()
    {
        FindNearestPatrolPoint();

        if (nearestPatrolPoints.Count == 0) return;
        
        Destroy(draculaPrefab);
        
        var patrolPoint = FindPatrolPointsToPlayer();
        
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
        CleatNearestPatrolPoint();
    
    }
    
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
                Ray ray = new Ray(transform.position, patrolPoint.transform.position - transform.position);
                
                Debug.DrawLine(transform.position, patrolPoint.transform.position, Color.blue,3f);
                
                if (Physics.Raycast(ray, out hitInfo,minDistance))
                {
                    Debug.DrawLine(transform.position, patrolPoint.transform.position, Color.red, 3f);
                    continue;
                }
                nearestPatrolPoints.Add(patrolPoint);
            }
        }
    }
    
    private PatrolPoint FindPatrolPointsToPlayer()
    {
        var playerPos = player.transform.position;
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