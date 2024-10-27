using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Death : MonoBehaviour
{
   
    
    [SerializeField] GameObject draculaPrefab;

    private void Start()
    {
        CharacterInputController.Instance.draculaAnim.AddListener(Anime);
        draculaPrefab.SetActive(false);
    }

    private void Anime()
    {
        draculaPrefab.SetActive(true);
        draculaPrefab.GetComponent<Animator>().Play("Attack");
       
    }

    void OnDestroy()
    {
        CharacterInputController.Instance.draculaAnim.RemoveListener(Anime);
    }
}
