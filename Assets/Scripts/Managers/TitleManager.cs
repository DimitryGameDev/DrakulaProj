using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
   [SerializeField] private Text titleText;
   [SerializeField] private Text skipText;
   [SerializeField] private Image skipImage;
   [SerializeField] private Image backgroundImage;
   [Space]
   [SerializeField] private float speedText;
   [SerializeField] private int moveByY;
   [SerializeField] private float timeToScip = 2f;
 
   
   private Vector3 textPos;
   private AudioSource audioSource;
   private float timer;
   private bool nextScene;
   
   private void Start()
   {
      textPos = titleText.rectTransform.position;
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
      audioSource = GetComponent<AudioSource>();
      skipText.color = new Color(skipText.color.r, skipText.color.g, skipText.color.b, 0);
      backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, 0);
   }

   private void Update()
   {
      skipText.color =  new Color(skipText.color.r, skipText.color.g, skipText.color.b,
         Mathf.MoveTowards(skipText.color.a, 1, Time.deltaTime/2));
      backgroundImage.color =  new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b,
         Mathf.MoveTowards(backgroundImage.color.a, 1, Time.deltaTime/2));
      textPos = new Vector3(textPos.x, Mathf.MoveTowards(textPos.y, moveByY, Time.deltaTime * speedText), textPos.z);
      
      titleText.rectTransform.position = textPos;
      
      if (Input.anyKey && !nextScene)
      {
         timer += Time.deltaTime;
         
         if (timer >= timeToScip)
         {
            nextScene = true;
         }
      }
      else
      {
         timer = 0;
      }
      
      skipImage.fillAmount = timer / timeToScip;
      
      if (Mathf.Approximately(moveByY, textPos.y))
      {
         nextScene = true;
      }

      if (nextScene)
      {
         LoadNextScene();
      }
   }
   
   private void LoadNextScene()
   {
      audioSource.volume = Mathf.MoveTowards(audioSource.volume,0f, Time.deltaTime);
      skipText.color =  new Color(skipText.color.r, skipText.color.g, skipText.color.b,
         Mathf.MoveTowards(skipText.color.a, 0, Time.deltaTime));
      titleText.color = new Color(titleText.color.r, titleText.color.g, titleText.color.b, Mathf.MoveTowards(titleText.color.a,0f, Time.deltaTime));
      if (audioSource.volume == 0)
      {
         SceneManager.LoadScene(2);
      }
   }
}

