using UnityEngine.UI;
using UnityEngine;

public class SprintBarUI : MonoBehaviour
{
    [SerializeField] private Image image;
    private CharacterInputController characterInputController;
    private Color startColor;
    
    private void Start()
    {
        characterInputController = CharacterInputController.Instance;
        startColor = image.color;
    }

    private void Update()
    {
        if (characterInputController.isSprinting) image.color = startColor;
        if (!characterInputController.isSprinting) image.color = Color.yellow;
        image.fillAmount = characterInputController.SprintTimer / characterInputController.TimeSprint;
    }
}
