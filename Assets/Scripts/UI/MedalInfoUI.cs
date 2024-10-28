using UnityEngine;
using UnityEngine.UI;

public class MedalInfoUI : MonoBehaviour
{
    [SerializeField] private Image[] medalImages;
    [SerializeField] private Player player;

    private int countPieces;

    private void Start()
    {
        player.OnChangedMedalPiece.AddListener(SetImageVisible);
        SetImageInvisible();
    }

    private void OnDestroy()
    {
        player.OnChangedMedalPiece.RemoveListener(SetImageVisible);
    }

    private void SetImageVisible()
    {
        if (countPieces >= medalImages.Length)
        {
            SetImageInvisible();
            countPieces = 0;
            medalImages[countPieces].color = Color.white;
        }
        else
        {
            medalImages[countPieces].color = Color.white;
        }

        countPieces++;
    }

    private void SetImageInvisible()
    {
        for (int i = 0; i < medalImages.Length; i++)
        {
            medalImages[i].color = new Color(1, 1, 1, 0.4f);
        }
    }
}