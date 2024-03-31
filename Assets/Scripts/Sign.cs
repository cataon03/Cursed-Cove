using System; 
using Yarn.Unity; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    public string signDialogueNodeName; 


    void OnTriggerEnter2D(Collider2D other)
    {   
        GetComponent<CircleCollider2D>().enabled = false; 
        DialogueManager.instance.StartDialogue(signDialogueNodeName); 
    }
}