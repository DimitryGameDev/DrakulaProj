using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    public UnityEvent OnSwitch;
    [SerializeField] MeshRenderer leverModel;
    [SerializeField] InteractiveObject interactiveObject;
    void Start()
    {
        leverModel.enabled = false;
        interactiveObject.onVision.AddListener(Found);
    } 
    public void Found()
    {
        leverModel.enabled = true;
        interactiveObject.onVision.RemoveListener(Found);
        Debug.Log(interactiveObject.name);
    }
    
}
