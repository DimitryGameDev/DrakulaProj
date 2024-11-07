using UnityEngine;

public class InvisibleDoor : InteractiveObject
{
    [Header("Door Settings")]
    [SerializeField] private GameObject[] walls;

    public override void InCamera()
    {
        base.InCamera();
        WallVisible();
    }

    public override void ShowText() 
    {
        // выключит отображение при наведении
    }
    
    public void WallVisible()
    {
        HideInfoPanel();
        ShowAfterText();

        for (int i = 0; i < walls.Length; i++)
        {
            Destroy(walls[i]);
        }
       
    }
}