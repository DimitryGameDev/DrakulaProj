using System.Collections.Generic;
using UnityEngine;

public class CameraVision : MonoBehaviour
{
    [SerializeField] private float maxDistance = 100f;
    private Camera playerCamera;
    private List<VisibleObject> visionObjs;
    [SerializeField] private Heart heartPrefab;

    private void Awake()
    {
        visionObjs = new List<VisibleObject>();
        visionObjs.AddRange(FindObjectsOfType<VisibleObject>());
        
        foreach (var t in visionObjs)
        {
            t.Ondestroy += RemoveVisionObj;
        }
    }

    private void Start()
    {
        playerCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        FindObjIntoCamera();
    }

    private void FindObjIntoCamera()
    {
        foreach (var t in visionObjs)
        {
            if (heartPrefab.IsActive)
            {
                if (IsVisionObj(t.gameObject))
                {
                    Debug.DrawLine(playerCamera.transform.position, t.transform.position, Color.green,
                        Time.deltaTime);
                    t.InCamera();
                }
            }
                
            if (!IsVisionObj(t.gameObject))
            {
                //Debug.DrawLine(playerCamera.transform.position, visionObjs[i].transform.position, Color.grey,Time.deltaTime);
                //  
                t.OutCamera();
            }
            else
            {
                Debug.DrawLine(playerCamera.transform.position, t.transform.position, Color.grey,Time.deltaTime);
            }
        }
    }

    private bool IsVisionObj(GameObject objectToCheck)
    {
        var viewPortPoint = playerCamera.WorldToViewportPoint(objectToCheck.transform.position);
        
        if (viewPortPoint is { z: > 0, y: < 0.9f and > 0.1f, x: > 0.1f and < 0.9f })
        {
            var ray = playerCamera.ScreenPointToRay(playerCamera.WorldToScreenPoint(objectToCheck.transform.position));

            if (Physics.Raycast(ray, out var hitInfo, maxDistance,LayerMask.NameToLayer("Player") | LayerMask.NameToLayer("Ignore Raycast")))
            {
                if (hitInfo.transform.parent.gameObject == objectToCheck && hitInfo.transform.parent.gameObject != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void RemoveVisionObj(VisibleObject objectToRemove)
    {
        visionObjs.Remove(objectToRemove);
    }
}
