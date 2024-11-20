using UnityEngine;

public class SaveZone : InteractiveObject 
{
    [Header("SaveZone Settings")]
    [SerializeField]private AudioClip backgroundMusic;
    
    private InteractiveState interactiveState;
    private PlayerState playerState;
    private Character character;
    private OnePersonCamera onePersonCamera;
    private CharacterInputController characterInputController;
    private Bag bag;
    private NoiseLevel noiseLevel;
    private InteractiveObject[] objects;
    private Dracula dracula;
    
    protected override void Start()
    {
        base.Start();
        interactiveState = InteractiveState.Instance;
        playerState = PlayerState.Instance;
        character = (Character)Character.Instance;
        onePersonCamera = OnePersonCamera.Instance;
        noiseLevel = NoiseLevel.Instance;
        characterInputController = CharacterInputController.Instance;
        dracula = Dracula.Instance;
        bag = character.GetComponent<Bag>();
        objects = FindObjectsOfType<InteractiveObject>();
        
        LoadSceneState();
    }

    public override void ShowText() { }
    
    public override void Use() { }

    public void StayTrigger()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveSceneState();
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

    private void LoadSceneState()
    {
        bag.Reset();
        
        for (int i = 0; i < objects.Length; i++) objects[i].LoadState();
        
        if (playerState.GetPlayerPos() != Vector3.zero)character.transform.position = playerState.GetPlayerPos();
        if (playerState.GetCameraPos() != Vector3.zero) onePersonCamera.transform.position = playerState.GetCameraPos();
        if (playerState.GetSprintAmount() != 0) characterInputController.SetSpeedTime(playerState.GetSprintAmount());
        
        bag.AddKey(playerState.GetKeyAmount());
        bag.AddMedalPiece(playerState.GetMedalAmount());
        characterInputController.pickUpHeart = playerState.GetHeartState();
      
    }
    
    private void SaveSceneState()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            interactiveState.Save(objects[i],objects[i].WosActive);
        }
        
        playerState.Save(character.transform.position,onePersonCamera.transform.position, bag.GetKeyAmount(),bag.GetMedalPeaceAmount(),characterInputController.TimeSprint,characterInputController.pickUpHeart);
    }
}
