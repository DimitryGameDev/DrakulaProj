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
   private void Start()
   {
      notesBox.SetActive(false);
      enabled = false;
      source = GetComponent<AudioSource>();
      interactiveObject = GetComponent<InteractiveObject>();
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
      if (Dracula.Instance)
      {
         Dracula.Instance.DraculaDisable();
      }
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.E))
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
      if (Dracula.Instance)
      {
         Dracula.Instance.enabled = true; //включает дракулу
      }
   }
}
