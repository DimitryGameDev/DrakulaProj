using UnityEngine;

public class HeartPickUp : InteractiveObject
{
    [Header("Heart Settings")]
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject visualModel;

    public override void Use()
    {
        base.Use();
        PickUp();
        ShowAfterText();
        wosActive = true;
    }

    private void PickUp()
    {
        CharacterInputController.Instance.pickUpHeart = true;

        if (impactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        
        Destroy(visualModel);
    }
    protected override void ObjectWosActive()
    {
        Destroy(visualModel);
    }
}