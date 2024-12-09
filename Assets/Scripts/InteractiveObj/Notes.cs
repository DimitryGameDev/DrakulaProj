using UnityEngine;
using UnityEngine.UI;

public class Notes : InteractiveObject
{
   [Header("Setting Notes")]
   [SerializeField] private GameObject notesBox;
   [SerializeField] private Text titleTextUi;
   [SerializeField] private Text notesTextUi;
   [SerializeField] private Text worldText;
   [SerializeField] private AudioClip clip;
   [Space]
   [SerializeField] private string titleText;
   [SerializeField] [TextArea(5,10)] private string text;
  
   
   private Dracula dracula;
   private bool isNotes;
   protected override void Start()
   {
      base.Start();
      notesBox.SetActive(false);
      isNotes = false;
      dracula = Dracula.Instance;
      if (worldText != null)
      {
         worldText.text = text;
      }
    
   }

   public override void Use()
   {
      base.Use();
      OpenNotes();
   }

   private void OpenNotes()
   { 
      AudioSource.PlayOneShot(clip);
      isNotes = true;
      titleTextUi.text = titleText;
      notesTextUi.text = text;
      notesBox.SetActive(true);
      AudioSource.PlayOneShot(clip);
      OnePersonCamera.Instance.Lock();
      CharacterInputController.Instance.enabled = false;
      Pause.Instance.enabled = false;
      if (dracula)
      {
         dracula.DraculaDisable();
      }
   }

   protected override void Update()
   {
      base.Update();
      if (isNotes)
      {
         if (Input.GetKeyDown(KeyCode.Mouse0)|| Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.E))
         {
            CloseNotes();
         }
      }
   }

   protected virtual void CloseNotes()
   { 
      AudioSource.PlayOneShot(clip);
      isNotes = false;
      notesBox.SetActive(false);
      OnePersonCamera.Instance.UnLock();
      CharacterInputController.Instance.enabled = true;
      Pause.Instance.enabled = true;
      if (dracula)  dracula.DraculaEnable(); //включает дракулу
   }
}
