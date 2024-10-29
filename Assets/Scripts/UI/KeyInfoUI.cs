using UnityEngine;
using UnityEngine.UI;

public class KeyInfoUI : MonoBehaviour
{
    [SerializeField] private Text text;
    private Bag bag;

    private void Start()
    {
        bag = Character.Instance.GetComponent<Bag>();
        
        bag.ChangeKeyAmount.AddListener(ChangeText);
    }

    private void OnDestroy()
    {
        bag.ChangeKeyAmount.RemoveAllListeners();
    }

    private void ChangeText()
    {
        text.text = bag.GetKeyAmount().ToString();
    }
}
