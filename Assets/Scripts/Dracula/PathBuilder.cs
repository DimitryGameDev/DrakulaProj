using System.Collections.Generic;
using UnityEngine;

public class PathBuilder : MonoBehaviour
{
    private List<PatrolPoint> patrolPoints;
    private List<PatrolPoint> nearPatrolPoints;
    private List<PatrolPoint> pathPatrolPoints;
    private List<PatrolPoint> potentialPath;
    
    private PatrolPoint currentPatrolPoint;
    private PatrolPoint currentTarget;
    private PatrolPoint firstPatrolPoint;

    private float rad = 5;
    private int index;
    
    private void Start()
    {
        patrolPoints = new List<PatrolPoint>(FindObjectsOfType<PatrolPoint>());
        nearPatrolPoints = new List<PatrolPoint>();
        pathPatrolPoints = new List<PatrolPoint>();
        potentialPath = new List<PatrolPoint>();
        currentTarget = Character.Instance.GetComponent<DraculaPoint>();
    }

    public void ResetPath()
    {
        if (currentPatrolPoint != null && currentTarget != null)
        {
            SetProperties(currentPatrolPoint,currentTarget,rad);
        }
    }
    
    public DraculaPoint GetDraculaPoint(PatrolPoint currentPoint , PatrolPoint targetPoint,float radius)
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
        /*
        if (Vector3.Distance(lastPosTarget,targetPoint.transform.position) >= 3)
        {
            SetProperties(currentPoint, targetPoint, radius);
            return (DraculaPoint)firstPatrolPoint;
        }
       */ 
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

    private Vector3 lastPosTarget;
    private void SetProperties(PatrolPoint currentPoint, PatrolPoint targetPoint, float radius)
    {
        ClearPath();
        lastPosTarget = targetPoint.transform.position;
        currentTarget = targetPoint;
        currentPatrolPoint = currentPoint;
        rad = radius;
        CreatePath(currentPatrolPoint);
    }
    
    public void ClearPath()
    {
        foreach (var t in patrolPoints)
            t.Reset();

        index = 0;
        nearPatrolPoints.Clear();
        pathPatrolPoints.Clear();
        potentialPath.Clear();
        firstPatrolPoint = null;
    }
    
    private void CreatePath(PatrolPoint checkPoint)
    {
        if (currentPatrolPoint == currentTarget)
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
        
        foreach (var t in patrolPoints)
        {
            if (checkPoint == t) continue;
            var dist = Vector3.Distance(checkPoint.transform.position, t.transform.position);
            
            if (dist <= rad && !t.IsVisit)
            {
                t.SetDistanceToStart(checkPoint.transform.position);
                t.SetDistanceToObject(currentTarget.transform.position);
                nearPatrolPoints.Add(t);
            }
        }
        
        foreach (var t in nearPatrolPoints)
        {
            if (CheckWall(t,checkPoint))
            {
                potentialPath.Add(t);
            }
        }
        
        if (potentialPath.Count > 0)
        {
            var weight = Mathf.Infinity;
            foreach (var t in potentialPath)
            {
                var x = t.GetWeight();
                
                if (weight >= x)
                {
                    weight = x;
                    currentPatrolPoint  = t;
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

        if (Physics.Raycast(ray, out var hitInfo,rad))
        {
            if (hitInfo.collider.isTrigger || hitInfo.collider.CompareTag("IgnoreDraculaMove") || hitInfo.collider.CompareTag("Player"))
            { 
                Debug.DrawLine(currentPosOffset, checkPoint.transform.position + new Vector3(0,0.5f,0), Color.blue,5f);
                return true;
            }
            Debug.DrawLine(currentPosOffset,checkPoint.transform.position + new Vector3(0,0.5f,0), Color.red,5f);
            return false;
        }

        Debug.DrawLine(currentPosOffset, checkPoint.transform.position + new Vector3(0,0.5f,0), Color.blue,5f);
        return true;
    }
    #endregion
}
