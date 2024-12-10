using UnityEngine;

public class Projectile : InteractiveObject
{
    [Header("Projectile Settings")]
    [SerializeField] private int projectileCount;
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
        Character.Instance.GetComponent<Bag>().AddProjectile(projectileCount);

        if (impactEffect)
            Instantiate(impactEffect, transform.position, Quaternion.identity);

        Destroy(visualModel);
    }
    
    protected override void ObjectWosActive()
    {
        Destroy(visualModel);
    }
}
