using System.Collections.Generic;
using UnityEngine;

public class CameraVision : MonoBehaviour
{
    [SerializeField] private float maxDistance = 100f;
    private Camera playerCamera;
    private List<InteractiveObject> visionObjs;
    [SerializeField] private Heart heartPrefab;
    private void Start()
    {
        playerCamera = Camera.main;
        visionObjs = new List<InteractiveObject>();
        visionObjs.AddRange(FindObjectsOfType<InteractiveObject>());
    }

    private void Update()
    {
        FindObjIntoCamera();
    }

    private void FindObjIntoCamera()
    {
        if (heartPrefab.IsActive)
        {
            for (var i = 0; i < visionObjs.Count; i++)
            {
                if (IsVisionObj(visionObjs[i].gameObject))
                {
                    Debug.DrawLine(playerCamera.transform.position, visionObjs[i].transform.position, Color.red,
                        Time.deltaTime);
                    visionObjs[i].Visible();
                }

                if (!IsVisionObj(visionObjs[i].gameObject))
                {
                    Debug.DrawLine(playerCamera.transform.position, visionObjs[i].transform.position, Color.black,
                        Time.deltaTime);
                    visionObjs[i].Hide();
                }
            }
        }
           
    }

    private bool IsVisionObj(GameObject objectToCheck)
    {
        Vector3 viewPortPoint = playerCamera.WorldToViewportPoint(objectToCheck.transform.position);
        
        if (viewPortPoint is { z: > 0, y: < 1 and > 0, x: > 0 and < 1 })
        {
            Ray ray = playerCamera.ScreenPointToRay(playerCamera.WorldToScreenPoint(objectToCheck.transform.position));
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, maxDistance))
            {
                if (hitInfo.collider.transform.root.gameObject == objectToCheck)
                {
                    return true;
                }
            }
        }
        return false;
    }
    
}
