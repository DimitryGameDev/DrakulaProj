using Unity.VisualScripting;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField]private GameObject draculaPrefab;

    private void Start()
    {
        Dracula.Instance.DraculaInPlayer.AddListener(Anime);
        CharacterInputController.Instance.draculaAnim.AddListener(Anime);
        draculaPrefab.SetActive(false);
    }

    public void Anime()
    {
        draculaPrefab.SetActive(true);
        var animator = draculaPrefab.GetComponent<Animator>();
        animator.Play("Attack");

    }

    void OnDestroy()
    {
        CharacterInputController.Instance.draculaAnim.RemoveListener(Anime);
    }
}
