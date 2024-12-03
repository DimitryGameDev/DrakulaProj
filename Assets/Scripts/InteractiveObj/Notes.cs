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
   [SerializeField] private string text;
   
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
      isNotes = true;
      titleTextUi.text = titleText;
      notesTextUi.text = text;
      notesBox.SetActive(true);
      AudioSource.PlayOneShot(clip);
      
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
      if (Input.GetKeyDown(KeyCode.Mouse0) && isNotes) CloseNotes();
   }

   protected virtual void CloseNotes()
   { 
      isNotes = false;
      notesBox.SetActive(false);
      AudioSource.PlayOneShot(clip);
      OnePersonCamera.Instance.UnLock();
      CharacterInputController.Instance.enabled = true;
      if (dracula)  dracula.DraculaEnable(); //включает дракулу
   }
}
