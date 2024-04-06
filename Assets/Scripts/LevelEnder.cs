using System;
using System.Collections;
using UnityEngine;

public class LevelEnder : MonoBehaviour {
    Animator animator; 
    private GameManager gameManager; 

    private void Start()
    {
        DamageableCharacter.OnPlayerDeath += HandleOnPlayerDeath;
        animator = gameObject.GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy(){
        DamageableCharacter.OnPlayerDeath -= HandleOnPlayerDeath; 
    }

    private void HandleOnPlayerDeath(){
        Debug.Log("received on player"); 
      
        animator.SetTrigger("open");
        StartCoroutine(ShowDeathScreen()); 
    }

    private IEnumerator ShowDeathScreen(){; 
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("close"); 
        gameManager.restartLevel(); 
    }

}
