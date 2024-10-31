using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
   [SerializeField] private Text titleText;
   [SerializeField] private Image skipImage;
   [Space]
   [SerializeField] private float speedText;
   [SerializeField] private int moveByY;
   [SerializeField] private float timeToScip = 2f;
 
   
   private Vector3 textPos;
   private AudioSource audioSource;
   private float timer;
   private void Start()
   {
      textPos = titleText.rectTransform.position;
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
      audioSource = GetComponent<AudioSource>();
   }

   private void Update()
   {
      textPos = new Vector3(textPos.x, Mathf.MoveTowards(textPos.y, moveByY, Time.deltaTime * speedText), textPos.z);
      
      titleText.rectTransform.position = textPos;
      
      if (Mathf.Approximately(moveByY, textPos.y))
      {
         LoadNextScene();
      }
      
      if (Input.anyKey)
      {
         timer += Time.deltaTime;

         if (timer >= timeToScip)
         {
            LoadNextScene();
         }
      }
      else
      {
         timer = 0;
      }
      
      skipImage.fillAmount = timer / timeToScip;
   }
   
   private void LoadNextScene()
   {
      audioSource.volume = Mathf.MoveTowards(audioSource.volume,0f, Time.deltaTime);

      if (audioSource.volume == 0)
      {
         SceneManager.LoadScene(2);
      }
   }
}

