using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InteractiveObject))]
public class FloorTrigger : MonoBehaviour
{
    [SerializeField] private Transform teleportPosition;
    [SerializeField] private Wakeup imageVisual;
    private InteractiveObject interactiveObject;
    private void Start()
    {
        interactiveObject = GetComponent<InteractiveObject>();
        interactiveObject.onUse.AddListener(Teleport);
        enabled = false;
    }

    private void Update()
    {
        if (!imageVisual.isWakeup)
        {
            Character.Instance.transform.position = teleportPosition.position;
            CharacterInputController.Instance.enabled = true;
            enabled = false;
        }
    }

    private void Teleport()
    {
        imageVisual.WakeUp();
        CharacterInputController.Instance.enabled = false;
        enabled = true;
    }
}
