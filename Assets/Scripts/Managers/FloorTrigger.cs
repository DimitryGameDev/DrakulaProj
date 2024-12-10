using UnityEngine;

public class FloorTrigger : InteractiveObject
{
    [SerializeField] private Transform teleportPosition;
    private InteractiveObject interactiveObject;
    private CharacterInputController character;

    protected override void Start()
    {
        base.Start();
        character = CharacterInputController.Instance;
    }

    public override void Use()
    {
        Teleport();
        base.Use();
    }

    private void Teleport()
    {
        character.transform.position = teleportPosition.position;
    }
}
