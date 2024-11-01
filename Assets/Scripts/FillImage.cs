using System;
using UnityEngine.UI;
using UnityEngine;

public class FillImage : MonoBehaviour
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
        if (characterInputController.IsSprinting) image.color = startColor;
        if (!characterInputController.IsSprinting) image.color = Color.yellow;
        image.fillAmount = characterInputController.SprintTimer / characterInputController.TimeSprint;
    }
}
