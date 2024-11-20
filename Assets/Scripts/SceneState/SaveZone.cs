using UnityEngine;

public class SaveZone : InteractiveObject 
{
    [Header("SaveZone Settings")]
    [SerializeField]private AudioClip backgroundMusic;
    
    private NoiseLevel noiseLevel;
    private Dracula dracula;
    private StateManager stateManager;
    protected override void Start()
    {
        base.Start();
        noiseLevel = NoiseLevel.Instance;
        dracula = Dracula.Instance;
        stateManager = StateManager.Instance;
    }

    public override void ShowText() { }
    
    public override void Use() { }

    public void StayTrigger()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateManager.SaveSceneState();
            ShowAfterText();
            AudioSource.Play();
        }
    }
    
    public void EnterTrigger()
    {
        BackgroundMusic.Instance.Play(backgroundMusic);
        base.ShowText();
        InteractiveBoxUI.HideCursor();
        if (dracula) dracula.DraculaDespawn();
        noiseLevel.SetZeroLevel();
    }

    public void ExitTrigger()
    {
        BackgroundMusic.Instance.Stop();
        if (dracula) dracula.RandomPoint();
    }
}
