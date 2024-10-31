using UnityEngine;
using UnityEngine.Events;
public class TriggerController : MonoBehaviour
{
    public static UnityAction OnTrigger;
    private Trigger[] triggers;
    
    private void Awake()
    {
        triggers = FindObjectsOfType<Trigger>();
        
        for (int j = 0; j < triggers.Length; j++)
        {
            triggers[j].onTrigger.AddListener(OnTrigger);
        }
    }
}
