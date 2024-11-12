using UnityEngine;

public class Medal : InteractiveObject
{
    [Header("Medal Settings")]
    [SerializeField] private int countPiece;
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
        Character.Instance.GetComponent<Bag>().AddMedalPiece(countPiece);
        
        if (impactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        
        Destroy(visualModel);
    }
    protected override void ObjectWosActive()
    {
        Destroy(visualModel);   
    }
}
