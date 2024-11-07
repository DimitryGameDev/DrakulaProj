
public class TriggerController : SingletonBase<TriggerController>
{
    private Trigger[] triggers;
    
    private void Awake()
    {
        Init();
        triggers = FindObjectsOfType<Trigger>();
        
        foreach (var t in triggers)
        {
            t.onTrigger.AddListener(SwitchAllTriggers);
        }
    }

    private void SwitchAllTriggers()
    {
        foreach (var t in triggers)
        {
            t.SwitchActive();
        }
    }
}
