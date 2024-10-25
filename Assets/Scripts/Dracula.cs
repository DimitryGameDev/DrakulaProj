using System.Collections.Generic;
using UnityEngine;

public class Dracula : MonoBehaviour
{
    [Header("Dracula Prefabs")] [SerializeField]
    private GameObject draculaPrefabsStay;

    [SerializeField] private GameObject draculaPrefabsSit;
    [SerializeField] private GameObject draculaPrefabsUp;
    [SerializeField] private GameObject draculaPrefabsT;

    [Space] [Header("Dracula Settings")] [SerializeField] [Range(0f, 10f)]
    private float minDistance = 5;

    [SerializeField] private int spawnRate = 4;

    private GameObject dracula;
    private List<PatrolPoint> patrolPoints;
    private List<PatrolPoint> nearestPatrolPoints;
    private float time;

    private void Start()
    {
        patrolPoints = new List<PatrolPoint>();
        FillPatrolPointsInScene();
        nearestPatrolPoints = new List<PatrolPoint>();

        DraculaSpawn();
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= spawnRate)
        {
            DraculaSpawn();
            time = 0;
        }
    }

    private void DraculaSpawn()
    {
        Destroy(dracula);

        FindNearestPatrolPoint();

        var patrolPoint = FindPatrolPointsToPlayer();

        if (draculaPrefabsT != null)
        {
            var prefab = draculaPrefabsT;

            if (patrolPoint.DraculaPos == DraculaPosType.Sit 
                && draculaPrefabsSit != null) prefab = draculaPrefabsSit;
            if (patrolPoint.DraculaPos == DraculaPosType.Stay 
                && draculaPrefabsStay != null) prefab = draculaPrefabsStay;
            if (patrolPoint.DraculaPos == DraculaPosType.Up 
                && draculaPrefabsUp != null) prefab = draculaPrefabsUp;

            transform.position = patrolPoint.transform.position;

            dracula = Instantiate(prefab, patrolPoint.transform.position, Quaternion.identity);
        }

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