using UnityEngine;

public class Key : InteractiveObject
{
    [Header("Key Settings")]
    [SerializeField] private int keyCount;
    [SerializeField] private ImpactEffect impactEffect;
    [SerializeField] private GameObject visualModel;

    public override void Use()
    {
        base.Use();
        PickUp();
        ShowAfterText();
        wosActive = true;
    }

    protected override void ShowAfterText()
    {
        
    }
    private void PickUp()
    {
        Character.Instance.GetComponent<Bag>().AddKey(keyCount);

        if (impactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);

        Destroy(visualModel);
    }
    
    protected override void ObjectWosActive()
    {
        Destroy(visualModel);
    }
}