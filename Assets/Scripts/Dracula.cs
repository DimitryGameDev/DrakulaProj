using System.Collections.Generic;
using Common;
using Unity.VisualScripting;
using UnityEngine;

public class Dracula : MonoBehaviour
{
    [Header("Dracula Prefabs")] 
    [SerializeField] private GameObject draculaPrefabsStay;
    [SerializeField] private GameObject draculaPrefabsSit;
    [SerializeField] private GameObject draculaPrefabsUp;
    [SerializeField] private GameObject draculaPrefabsT;
    [SerializeField] private ImpactEffect draculaImpactEffectPrefab;
    [Space] [Header("Dracula Settings")] 
    [SerializeField] [Range(0f, 10f)] private float minDistance = 5;

    [SerializeField] private int spawnRate = 4;

    private GameObject dracula;
    private List<PatrolPoint> patrolPoints;
    private List<PatrolPoint> nearestPatrolPoints;
    private MeshRenderer draculaMeshRenderer;
    private float timeSpawnRate;
    private float timeImpactEffect;
    private bool isHeart;
    private bool isVisible;

    private void Start()
    {
        CharacterInputController.Instance.heartOn.AddListener(TogleHeartOn);
        CharacterInputController.Instance.heartOff.AddListener(TogleHeartOff);
        GetComponent<InteractiveObject>().onVision.AddListener(TogleVisionOn);
        GetComponent<InteractiveObject>().onHide.AddListener(TogleVisionOff);
        
        patrolPoints = new List<PatrolPoint>();
        FillPatrolPointsInScene();
        nearestPatrolPoints = new List<PatrolPoint>();
        DraculaSpawn();
    }

    void Update()
    {
        timeSpawnRate += Time.deltaTime;

        if (timeSpawnRate >= spawnRate)
        {
            DraculaSpawn();
            timeSpawnRate = 0;
        }

        VisibleMeshDracula();
        DraculaEffect();
    }
    
    private void TogleVisionOn() => isVisible = true;
    private void TogleVisionOff() => isVisible = false;
    private void TogleHeartOn() => isHeart = true;
    private void TogleHeartOff() => isHeart = false;
    private void VisibleMeshDracula()
    {
        if (isHeart)
        {
            draculaMeshRenderer.enabled = true;
        }
        else
        {
            draculaMeshRenderer.enabled = false;
        }
    } 
    private void DraculaEffect()
    {
        timeImpactEffect -= Time.deltaTime;
        if (isVisible && isHeart)
        {
            if (timeImpactEffect <= 0)
            {
                Instantiate(draculaImpactEffectPrefab,transform.position, Quaternion.identity,null);
                timeImpactEffect = draculaImpactEffectPrefab.LifeTimer; 
            }
            
        }
    }

    private void DraculaSpawn()
    {
        Destroy(dracula);

        FindNearestPatrolPoint();

        var patrolPoint = FindPatrolPointsToPlayer();
        
        var prefab = draculaPrefabsT;

        if (patrolPoint.DraculaPos == DraculaPosType.Sit 
            && draculaPrefabsSit != null) prefab = draculaPrefabsSit;
        if (patrolPoint.DraculaPos == DraculaPosType.Stay 
            && draculaPrefabsStay != null) prefab = draculaPrefabsStay;
        if (patrolPoint.DraculaPos == DraculaPosType.Up 
            && draculaPrefabsUp != null) prefab = draculaPrefabsUp;

        transform.position = patrolPoint.transform.position;
            
        dracula = Instantiate(prefab, patrolPoint.transform.position, Quaternion.identity, transform);
        draculaMeshRenderer = dracula.GetComponent<MeshRenderer>();
 

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
                nearestPatrolPoints.Add(patrolPoint);
            }
        }
    }

    private const float MinDistToPlayer = 1000f;

    private PatrolPoint FindPatrolPointsToPlayer()
    {
        var playerPos = Character.Instance.transform.position;
        float minDist = MinDistToPlayer;
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