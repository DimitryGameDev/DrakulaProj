using UnityEngine;

public class Chest : MonoBehaviour
{
    private InteractiveObject interactiveObject;
    private Animator animator;
    private void Start()
    {
        interactiveObject = GetComponent<InteractiveObject>();
        interactiveObject.onUse.AddListener(OpenChest);
        animator = GetComponent<Animator>();
    }

    private void OpenChest()
    {
        animator.enabled = true;
        interactiveObject.HideInfoPanel();
        Destroy(interactiveObject);
    }

}
