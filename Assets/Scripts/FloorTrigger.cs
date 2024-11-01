using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InteractiveObject))]
public class FloorTrigger : MonoBehaviour
{
    [SerializeField] private Transform teleportPosition;
    [SerializeField] private Wakeup imageVisual;
    private InteractiveObject interactiveObject;
    private CharacterInputController character;
    private OnePersonCamera onePersonCamera;

    private void Start()
    {
        character = CharacterInputController.Instance;
        onePersonCamera = OnePersonCamera.Instance;
        interactiveObject = GetComponent<InteractiveObject>();
        interactiveObject.onUse.AddListener(Teleport);
        enabled = false;
    }

    private void Update()
    {
        if (!imageVisual.isWakeup)
        {
            character.transform.position = teleportPosition.position;
            interactiveObject.infoPanel.SetActive(true);
            enabled = false;
        }
        interactiveObject.infoPanel.SetActive(false);
    }

    private void Teleport()
    {
        imageVisual.WakeUp();
        enabled = true;
    }
}
