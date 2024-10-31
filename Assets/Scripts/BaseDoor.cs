using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class BaseDoor : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Text infoText;
    [SerializeField] private string text;
    [SerializeField] private Animator animator;

    private AudioSource audioSource;
    private float textTimer;
    private bool isOpened;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        InfoText();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == Character.Instance.GetComponentInChildren<Collider>())
        {
            OpenDoor();
            
            if(!isOpened)
             textTimer = 1;
        }
    }
    
    private void OpenDoor()
    {
        if (Input.GetKey(KeyCode.E))
        {
            animator.SetBool("Open", true);
            audioSource.Play();
            
            isOpened = true;
        }
    }
    
    private void InfoText()
    {
        if(textTimer>=0)
            textTimer -= Time.deltaTime;

        if (textTimer > 0)
        {
            infoText.text = text;
            infoPanel.SetActive(true);
        }
        else
            infoPanel.SetActive(false);
    }
}