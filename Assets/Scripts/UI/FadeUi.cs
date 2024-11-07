using UnityEngine;
using UnityEngine.UI;

public class FadeUi : MonoBehaviour
{
    private Image image;

    private bool isShow;
    private void Start()
    {
        image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        enabled = false;
    }

    private void Update()
    {
        if (isShow)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.MoveTowards(image.color.a, 1, Time.deltaTime));
            if (Mathf.Approximately(image.color.a, 1)) enabled = false;
        }
        else
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.MoveTowards(image.color.a, 0, Time.deltaTime));
            if (Mathf.Approximately(image.color.a, 0))enabled = false;
        }
    }

    public void Show()
    {
        isShow = true;
        enabled = true;
    }
    public void Hide()
    {
        isShow = false;
        enabled = true;
    }
}