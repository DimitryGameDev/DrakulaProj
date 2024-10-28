using UnityEngine;
using UnityEngine.UI;

public class KeyInfoUI : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Bag bag;

    private void Start()
    {
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
