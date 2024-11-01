
public class TriggerController : SingletonBase<TriggerController>
{
    private Trigger[] triggers;
    
    private void Awake()
    {
        Init();
        triggers = FindObjectsOfType<Trigger>();
        
        for (int j = 0; j < triggers.Length; j++)
        {
            triggers[j].onTrigger.AddListener(SwithAllTriggers);
        }
    }

    private void SwithAllTriggers()
    {
        for (int j = 0; j < triggers.Length; j++)
        {
            triggers[j].SwitchActive();
        }
    }
}
