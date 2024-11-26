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
        if (characterInputController.isStamina) image.color = startColor;
        if (!characterInputController.isStamina) image.color = Color.red;
        image.fillAmount = characterInputController.StaminaTimer / characterInputController.Stamina;
    }
}
