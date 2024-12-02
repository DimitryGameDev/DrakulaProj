using UnityEngine;

public class SaveZone : InteractiveObject 
{
    [Header("SaveZone Settings")]
    [SerializeField]private AudioClip backgroundMusic;
    [SerializeField] private DraculaPoint[] spawnPointsDracula;
    
    private NoiseLevel noiseLevel;
    private Dracula dracula;
    private InteractiveStateManager interactiveStateManager;
    private CharacterStateManager characterStateManager;
    protected override void Start()
    {
        base.Start();
        noiseLevel = NoiseLevel.Instance;
        dracula = Dracula.Instance;
        interactiveStateManager = InteractiveStateManager.Instance;
        characterStateManager = CharacterStateManager.Instance;
    }

    public override void ShowText() { }
    
    public override void Use() { }

    public void StayTrigger()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            interactiveStateManager.Save();
            characterStateManager.Save();
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
        if (dracula) dracula.SetSpawnPoints(spawnPointsDracula);
    }
}
