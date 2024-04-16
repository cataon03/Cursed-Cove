using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TutorialDialogueManager : MonoBehaviour
{
    //Variables
    public DialogueRunner dialogueRunner;
    private bool dialogueActive;

    // Update is called once per frame
    void Update()
    {
        //Check if dialogue is active, if not switch scene

    }

    //Start dialogue
    public void StartDialogue(string node)
    {
        dialogueActive = true;
        dialogueRunner.StartDialogue(node);
    }

    //End dialogue and switch scene
    public void EndDialogue()
    {
        dialogueActive = false;
    }

    
}
