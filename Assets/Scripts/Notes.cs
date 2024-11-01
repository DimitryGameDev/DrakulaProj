using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InteractiveObject))]
public class Notes : MonoBehaviour
{
   [SerializeField] private GameObject notesBox;
   [SerializeField] private Text titleTextUi;
   [SerializeField] private Text notesTextUi;
   [SerializeField] private AudioClip clip;
   [Space]
   
   [SerializeField] private string titleText;
   [SerializeField] private string text;

   private InteractiveObject interactiveObject;
   private AudioSource source;
   private Dracula dracula;
   private void Start()
   {
      notesBox.SetActive(false);
      enabled = false;
      source = GetComponent<AudioSource>();
      interactiveObject = GetComponent<InteractiveObject>();
      dracula = Dracula.Instance;
      interactiveObject.onUse.AddListener(OpenNotes);
   }

   private void OpenNotes()
   { 
      enabled = true;
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

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Escape))
      {
         CloseNotes();
      }
   }

   protected virtual void CloseNotes()
   { 
      enabled = false;
      notesBox.SetActive(false);
      source.PlayOneShot(clip);
      
      OnePersonCamera.Instance.UnLock();
      CharacterInputController.Instance.enabled = true;
      if (dracula)
      {
         dracula.DraculaEnable(); //включает дракулу
      }
   }
}
