using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TutorialDialogueCoinCollider : MonoBehaviour
{
    public DialogueRunner dialogueRunnerCoin;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dialogueRunnerCoin.StartDialogue("BasicFighting");
            Destroy(gameObject);
        }
    }
    
}
