using System; 
using Yarn.Unity; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    public string signDialogueNodeName; 

    void OnTriggerExit2D(){
        DialogueManager.instance.StopDialogue(); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        DialogueManager.instance.StartDialogue(signDialogueNodeName); 
    }
}