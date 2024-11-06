using UnityEngine;

public class InvisibleDoor : InteractiveObject
{
    [SerializeField] private GameObject[] walls;

    public override void InCamera()
    {
        base.InCamera();
        WallVisible();
    }

    private void WallVisible()
    {
        HideInfoPanel();
        //if (CharacterInputController.Instance.HeartEnabled)
       // {
            for (int i = 0; i < walls.Length; i++)
            {
                Destroy(walls[i]);
            }
        //}
    }
}