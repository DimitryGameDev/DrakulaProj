using UnityEngine;

public class HeartPickUp : InteractiveObject
{
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private GameObject visualModel;

    public override void Use()
    {
        base.Use();
        PickUp();
        ShowAfterText();
    }

    private void PickUp()
    {
        CharacterInputController.Instance.pickUpHeart = true;

        if (impactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        
        Destroy(visualModel);
    }
}