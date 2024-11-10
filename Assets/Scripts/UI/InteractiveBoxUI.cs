using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class InteractiveBoxUI : SingletonBase<InteractiveBoxUI>
{
    [SerializeField] private float showSpeed = 1f;
    public GameObject infoBox;
    public Image backgroundImage;
    public Text text;
    public Image icon;
    public Image iconPickUp;

    private bool isEnable;
    private bool isCursor;
    private bool isIcon;

    private void Awake()
    {
        Init();
        Disable();
    }

    private void Update()
    {
        if (isCursor) iconPickUp.color = ChangeTransparent(iconPickUp.color,1);
        else iconPickUp.color = ChangeTransparent(iconPickUp.color,0);
        if (icon.sprite && isIcon) icon.color = ChangeTransparent(icon.color,1);
        else icon.color = new Color(icon.color.r, icon.color.g, icon.color.b,0);
        
        if (isEnable)
        {
            text.color = ChangeTransparent(text.color,1);
            
            backgroundImage.color = ChangeTransparent(backgroundImage.color,1f);
            if (Mathf.Approximately(backgroundImage.color.a + iconPickUp.color.a + icon.color.a +   text.color.a, 1f))enabled = false;
        }
        else
        {
            text.color = ChangeTransparent(text.color,0);
            
            backgroundImage.color = ChangeTransparent(backgroundImage.color,0);
            if (Mathf.Approximately(backgroundImage.color.a + iconPickUp.color.a + icon.color.a +   text.color.a ,0))enabled = false;
        }
    }

    public void HideCursor()
    {
        isCursor = false;
    }
    
    public void HideIcon()
    {
        isIcon = false;
    }
    
    public void Enable()
    {
        isCursor = true;
        isIcon = true;
        isEnable = true;
        enabled = true;
    }
    
    public void Disable()
    {
        HideCursor();
        HideIcon();
        isEnable = false;
        enabled = true;
    }
    
    private Color ChangeTransparent(Color currentColor,float change)
    {
        currentColor = new Color(currentColor.r, currentColor.g, currentColor.b,
            Mathf.MoveTowards(currentColor.a, change, showSpeed * Time.deltaTime));
        return currentColor;
    }
}
