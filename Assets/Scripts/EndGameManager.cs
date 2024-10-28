using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [SerializeField] private Death death;

    [SerializeField] private float loseDelayTimer;
    private bool isLose;
    
    private void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        if (death != null)
        {
            death.OnDeath.AddListener(Lose);
        }
    }

    private void OnDestroy()
    {
        if (death != null)
        {
            death.OnDeath.RemoveListener(Lose);
        }
    }

    private void Update()
    {
        ViewLosePanel();
    }

    private void Lose()
    {
        isLose = true;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Character>())
        {
            ViewWinPanel();
        }
    }
}