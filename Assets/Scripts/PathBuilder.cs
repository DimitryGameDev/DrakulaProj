using System;
using System.Collections.Generic;
using UnityEngine;

public class PathBuilder : MonoBehaviour
{
    private PatrolPoint startPatrolPoint;
    [SerializeField] private PatrolPoint targetPatrolPoint;
    private List<PatrolPoint> patrolPoints;
    private List<PatrolPoint> nearPatrolPoints;
    private List<PatrolPoint> pathPatrolPoints;
    private List<PatrolPoint> potentialPath;
    
    private PatrolPoint currentPatrolPoint;
    private PatrolPoint currentTarget;
    private PatrolPoint firstPatrolPoint;

    private int rad = 5;
    private int index;
    
    private void Start()
    {
        patrolPoints = new List<PatrolPoint>(FindObjectsOfType<PatrolPoint>());
        nearPatrolPoints = new List<PatrolPoint>();
        pathPatrolPoints = new List<PatrolPoint>();
        potentialPath = new List<PatrolPoint>();
        currentPatrolPoint = startPatrolPoint;
        currentTarget = targetPatrolPoint;
    }
    
    public DraculaPoint GetDraculaPoint(PatrolPoint currentPoint , PatrolPoint targetPoint,int radius)
    {
        Ray ray = new Ray(currentPoint.transform.position + new Vector3(0,0.5f,0), targetPoint.transform.position - currentPoint.transform.position + new Vector3(0,0.5f,0));
        
        if (Physics.Raycast(ray, out var hitInfo))
        {
            Debug.DrawLine(currentPoint.transform.position + new Vector3(0,0.5f,0),hitInfo.transform.position, Color.red,5f);
            
            if (hitInfo.collider.transform.parent?.GetComponent<Character>())
            {
                SetProperties(currentPoint, targetPoint, radius);
                return (DraculaPoint)firstPatrolPoint;
            }
        }
        
        if (index == pathPatrolPoints.Count - 2)
        {
            SetProperties(currentPoint, targetPoint, radius);
            return (DraculaPoint)firstPatrolPoint;
        }
        
        if (pathPatrolPoints.Count == 0)
        {
            SetProperties(currentPoint, targetPoint, radius);
            return (DraculaPoint)firstPatrolPoint;
        }
        else
        {
            index++;
            return (DraculaPoint)pathPatrolPoints[index];
        }
    }

    
    #region PathLogic
    private void SetProperties(PatrolPoint currentPoint, PatrolPoint targetPoint, int radius)
    {
        ClearPath();
        currentTarget = targetPoint;
        currentPatrolPoint = currentPoint;
        rad = radius;
        CreatePath(currentPatrolPoint);
    }
    
    private void ClearPath()
    {
        for (int i = 0; i < patrolPoints.Count; i++)patrolPoints[i].Reset();
        index = 0;
        nearPatrolPoints.Clear();
        pathPatrolPoints.Clear();
        potentialPath.Clear();
        firstPatrolPoint = null;
    }
    
    private void CreatePath(PatrolPoint checkPoint)
    {
        if (currentPatrolPoint == targetPatrolPoint)
        {
            for (int i = 0; i < pathPatrolPoints.Count; i++)
            {
                pathPatrolPoints[i].pathIndex = i + 1;
                if (pathPatrolPoints[i].pathIndex == 1)
                {
                    firstPatrolPoint = pathPatrolPoints[i];
                }
                pathPatrolPoints[i].FinalPoint();
            }
            return;
        }
        
        checkPoint.Visit();
        
        if (!CheckPath(checkPoint))
        {
            for (int i = pathPatrolPoints.Count - 1; i >= 0; i--)
            {
                if (CheckPath(pathPatrolPoints[i]))
                {
                    currentPatrolPoint.FinalPoint();
                    CreatePath(currentPatrolPoint);
                    break;
                }
                
                pathPatrolPoints[i].NoPath();
                pathPatrolPoints.RemoveAt(i);
            }
            return;
        }
        
        
        pathPatrolPoints.Add(currentPatrolPoint);
        CreatePath(currentPatrolPoint);
    }
    
    private bool CheckPath(PatrolPoint checkPoint)
    {
        nearPatrolPoints.Clear();
        
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            if (checkPoint == patrolPoints[i]) continue;
            var dist = Vector3.Distance(checkPoint.transform.position, patrolPoints[i].transform.position);
            
            if (dist <= rad && !patrolPoints[i].IsVisit)
            {
                patrolPoints[i].SetDistanceToStart(checkPoint.transform.position);
                patrolPoints[i].SetDistanceToObject(currentTarget.transform.position);
                nearPatrolPoints.Add(patrolPoints[i]);
            }
        }
        
        for (int i = 0; i < nearPatrolPoints.Count; i++)
        {
            if (CheckWall(nearPatrolPoints[i],checkPoint))
            {
                potentialPath.Add(nearPatrolPoints[i]);
            }
        }
        
        if (potentialPath.Count > 0)
        {
            var weight = Mathf.Infinity;
            for (int i = 0; i < potentialPath.Count; i++)
            { 
                var x = potentialPath[i].GetWeight();
                
                if (weight >= x)
                {
                    weight = x;
                    currentPatrolPoint  = potentialPath[i];
                }
            }
            
            potentialPath.Clear();
            return true;
        }
        return false;
    }

    private bool CheckWall(PatrolPoint checkPoint,PatrolPoint currentPoint)
    {
        var currentPosOffset = currentPoint.transform.position + new Vector3(0,0.5f,0);

        Ray ray = new Ray(currentPosOffset, (checkPoint.transform.position - currentPoint.transform.position)+ new Vector3(0,0.5f,0));
            
        var layerMask = 1 << LayerMask.NameToLayer("Player");
        layerMask = ~ layerMask;
        
        if (Physics.Raycast(ray, out var hitInfo,rad,layerMask))
        {
            Debug.DrawLine(currentPosOffset,checkPoint.transform.position + new Vector3(0,0.5f,0), Color.red,5f);
            return false;
        }
        else
        {
            Debug.DrawLine(currentPosOffset, checkPoint.transform.position + new Vector3(0,0.5f,0), Color.blue,5f);
            return true;
        }
    }


    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rad);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }
}
