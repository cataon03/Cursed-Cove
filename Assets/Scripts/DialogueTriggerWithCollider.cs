using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 


public class DialogueTriggerWithCollider : MonoBehaviour
{
    public string nodeName; 
    public bool disableAfterFirstCollision; 
    

    void OnTriggerExit2D(){
        DialogueManager.instance.StopDialogue(); 
    }
    
    void OnTriggerEnter2D(Collider2D other) {
    
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            if (nodeName == null){
                Debug.Log("No dialogue node name set for DialogueTriggerWithCollider!"); 
            }
            else {
                DialogueManager.instance.StartDialogue(nodeName);
                
                if (disableAfterFirstCollision){
                    gameObject.GetComponent<Collider2D>().enabled = false; 
                }
            }
        }
    }
}
