using UnityEngine;
using UnityEngine.UI;

public class Notes : InteractiveObject
{
   [Header("Setting Notes")]
   [SerializeField] private GameObject notesBox;
   [SerializeField] private Text titleTextUi;
   [SerializeField] private Text notesTextUi;
   [SerializeField] private AudioClip clip;
   [Space]
   [SerializeField] private string titleText;
   [SerializeField] private string text;
   
   private AudioSource source;
   private Dracula dracula;
   private bool isNotes;
   protected override void Start()
   {
      base.Start();
      notesBox.SetActive(false);
      isNotes = false;
      source = GetComponent<AudioSource>();
      dracula = Dracula.Instance;
   }

   public override void Use()
   {
      base.Use();
      OpenNotes();
   }

   private void OpenNotes()
   { 
      isNotes = true;
      titleTextUi.text = titleText;
      notesTextUi.text = text;
      notesBox.SetActive(true);
      source.PlayOneShot(clip);
      
      OnePersonCamera.Instance.Lock();
      CharacterInputController.Instance.enabled = false;
      if (dracula)
      {
         dracula.DraculaDisable();
      }
   }

   protected override void Update()
   {
      base.Update();
      if (Input.GetKeyDown(KeyCode.Escape) && isNotes) CloseNotes();
   }

   protected virtual void CloseNotes()
   { 
      isNotes = false;
      notesBox.SetActive(false);
      source.PlayOneShot(clip);
      OnePersonCamera.Instance.UnLock();
      CharacterInputController.Instance.enabled = true;
      if (dracula)  dracula.DraculaEnable(); //включает дракулу
   }
}
