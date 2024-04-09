using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LevelEnder : MonoBehaviour {
    Animator animator; 
    public TextMeshProUGUI text; 

    private GameManager gameManager; 

    private void Start()
    { 
        animator = gameObject.GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        DamageableCharacter.OnPlayerDeath += HandleOnPlayerDeath;
        Enemy.OnBossDeath += HandleOnBossDeath; 
    }

    private void OnDestroy(){
        DamageableCharacter.OnPlayerDeath -= HandleOnPlayerDeath;
        Enemy.OnBossDeath -= HandleOnBossDeath; 
    }

    private void HandleOnPlayerDeath(){
        DialogueManager.instance.StopDialogue(); 
        text.text = "YOU DIED."; 
        text.color = Color.red; 
        animator.SetTrigger("open");
        StartCoroutine(WaitForLevelRestart()); 
    }

    private void HandleOnBossDeath(){
        DialogueManager.instance.StopDialogue(); 
        text.text = "LEVEL SUCCESS."; 
        text.color = Color.white; 
        animator.SetTrigger("open");
        StartCoroutine(WaitForNextLevel()); 
    }


    private IEnumerator WaitForLevelRestart(){; 
        yield return new WaitForSeconds(5f);
        animator.SetTrigger("close"); 
        gameManager.restartLevel(); 
    }

    private IEnumerator WaitForNextLevel(){
        yield return new WaitForSeconds(5f);
        animator.SetTrigger("close"); 
        gameManager.nextLevel();
    }

}
