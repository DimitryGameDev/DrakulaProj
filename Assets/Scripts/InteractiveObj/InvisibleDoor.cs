using UnityEngine;

public class InvisibleDoor : InteractiveObject
{
    [Header("Door Settings")]
    [SerializeField] private GameObject[] walls;

    public override void InCamera()
    {
        base.InCamera();
        WallVisible();
        ShowAfterText();
        wosActive = true;
    }

    public override void ShowText() 
    {
        // выключит отображение при наведении
    }
    
    public void WallVisible()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            Destroy(walls[i]);
        }
    }
    
    protected override void ObjectWosActive()
    {
        WallVisible();
    }
}