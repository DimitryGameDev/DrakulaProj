using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    
    [SerializeField] private float loseDelayTimer;
    
    private Death death;
    private bool isLose;
    
    private void Start()
    {
        death = Character.Instance.GetComponent<Death>();
        
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        if (death != null)
        {
            death.onDeath.AddListener(Lose);
        }
    }

    private void OnDestroy()
    {
        if (death != null)
        {
            death.onDeath.RemoveListener(Lose);
        }
    }

    private void Update()
    {
        ViewLosePanel();
    }

    private void Lose()
    {
        isLose = true;
        Dracula.Instance.DraculaDisable();
    }

    private void ViewLosePanel()
    {
        if (isLose)
            loseDelayTimer -= Time.deltaTime;
        
        if (loseDelayTimer <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            losePanel.SetActive(true);
        }
    }

    private void ViewWinPanel()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        winPanel.SetActive(true);
        Dracula.Instance.DraculaDisable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Character>())
        {
            ViewWinPanel();
        }
    }
}