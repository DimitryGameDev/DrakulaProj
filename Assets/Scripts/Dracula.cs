using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dracula : MonoBehaviour
{
    [SerializeField] private List<PatrolPoint> patrolPoints;
    [SerializeField] private int spawnRate;
    Transform nearestPatrolPoint;
    public float mindistance = 100;
    [SerializeField] private GameObject dracula;
    // Update is called once per frame
    void Update()
    {
        MinDistReload();
        FindNearestPatrolPoint();
    }

    private void FollowPoints()
    {
        
    }

    private void FindNearestPatrolPoint()
    {
        for(int i = 0; i < patrolPoints.Count; i++)
        {
            
                Transform pointTransform = patrolPoints[i].transform;
                Transform draculaTransform = dracula.transform;
                float distance = Vector3.Distance(pointTransform.position, draculaTransform.position);
                if (distance < mindistance)
                {
                    mindistance = distance;
                }
                nearestPatrolPoint = pointTransform;
            
        }
        IEnumerator DraculaSpawn()
        {
            while (true)
            {
                var spawnPoint =  nearestPatrolPoint.position;
                yield return new WaitForSeconds(spawnRate);
            }
     
        }
    }

   
    private void MinDistReload()
    {
        mindistance = 100;
    }
}
