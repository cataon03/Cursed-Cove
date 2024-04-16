using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TutorialDialogueChickenCollider : MonoBehaviour
{
    public DialogueRunner dialogueRunnerChicken;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dialogueRunnerChicken.StartDialogue("TraderSystem");
            Destroy(gameObject);
        }
    }
}
