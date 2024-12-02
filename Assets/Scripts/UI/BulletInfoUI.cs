using UnityEngine;
using UnityEngine.UI;

public class BulletInfoUI : MonoBehaviour
{
   
    [SerializeField] private Text text;
    private Bag bag;

    private void Start()
    {
        bag = Character.Instance.GetComponent<Bag>();
        
        bag.changeProjectileAmount.AddListener(ChangeText);
    }

    private void OnDestroy()
    {
        bag.changeProjectileAmount.RemoveAllListeners();
    }

    private void ChangeText()
    {
        text.text = bag.GetProjectileAmount().ToString();
    }
   
}
