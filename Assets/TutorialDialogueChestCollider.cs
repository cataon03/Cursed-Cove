using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TutorialDialogueChestCollider : MonoBehaviour
{
    public DialogueRunner dialogueRunnerChest;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dialogueRunnerChest.StartDialogue("TutorialEnd");
        }
    }
}
