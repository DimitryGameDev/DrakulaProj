using UnityEngine;

public class Screamer : InteractiveObject
{
    [Header("Screamer Settings")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform pos;
    [SerializeField] private AudioClip clip;

    protected override void Start() {}
    protected override void Update() {}
    public override void ShowText(){}
    protected override void ShowAfterText(){}
    public override void EnterTrigger()
    {
        if (wosActive) return;
        var scream =Instantiate(prefab, pos.position, pos.rotation);
        if (clip) BackgroundSounds.Instance.Play(clip);
        Destroy(scream, clip.length);
        wosActive = true;
    }
    
}
