using System;
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
        
        for (var i = 0; i < visionObjs.Count; i++)
        {
            visionObjs[i].Ondestroy += RemoveVisionObj;
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
        for (var i = 0; i < visionObjs.Count; i++)
        { 
            if (heartPrefab.IsActive)
            {
                if (IsVisionObj(visionObjs[i].gameObject))
                {
                    Debug.DrawLine(playerCamera.transform.position, visionObjs[i].transform.position, Color.green,
                        Time.deltaTime);
                    visionObjs[i].InCamera();
                }
            }
                
            if (!IsVisionObj(visionObjs[i].gameObject))
            {
                //Debug.DrawLine(playerCamera.transform.position, visionObjs[i].transform.position, Color.grey,Time.deltaTime);
                //  
                visionObjs[i].OutCamera();
            }
            else
            {
                Debug.DrawLine(playerCamera.transform.position, visionObjs[i].transform.position, Color.grey,Time.deltaTime);
            }
        }
    }

    private bool IsVisionObj(GameObject objectToCheck)
    {
        Vector3 viewPortPoint = playerCamera.WorldToViewportPoint(objectToCheck.transform.position);
        
        if (viewPortPoint is { z: > 0, y: < 0.9f and > 0.1f, x: > 0.1f and < 0.9f })
        {
            Ray ray = playerCamera.ScreenPointToRay(playerCamera.WorldToScreenPoint(objectToCheck.transform.position));
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, maxDistance,LayerMask.NameToLayer("Player") | LayerMask.NameToLayer("Ignore Raycast")))
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
