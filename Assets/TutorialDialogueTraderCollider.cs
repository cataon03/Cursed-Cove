using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TutorialDialogueTraderCollider : MonoBehaviour
{
    public DialogueRunner dialogueRunnerTrader;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dialogueRunnerTrader.StartDialogue("ChestSystem");
        }
    }
}
