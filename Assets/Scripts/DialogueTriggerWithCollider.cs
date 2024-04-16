using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity; 
using System; 


public class DialogueTriggerWithCollider : MonoBehaviour
{
    bool isActive = true; 
   
    public string nodeName; 
    public bool disableAfterFirstCollision; 
    public bool hasCameraMovement; 

    void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player"))
        {
            Debug.Log("trigger exit"); 
            DialogueManager.instance.StopDialogue(); 
            if (disableAfterFirstCollision){
                gameObject.GetComponent<Collider2D>().enabled = false; 
            }
            if (hasCameraMovement){
                CameraManager.instance.switchToCamera(0); 
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other) {
    
        // Check if the collider belongs to the player
        if (other.CompareTag("Player") && isActive)
        {
            if (nodeName == null){
                Debug.Log("No dialogue node name set for DialogueTriggerWithCollider!"); 
            }
            else {
                DialogueManager.instance.StartDialogue(nodeName);
                
                
                if (disableAfterFirstCollision){
                    //gameObject.GetComponent<Collider2D>().enabled = false; 
                } 
            }
        }
    }

    public void setActive(bool active){
        isActive = active; 
    }

    [YarnCommand("disable_dialogue_trigger")]
    public void DisableDialogueTrigger(){
        isActive = false;  
    }

    [YarnCommand("enable_dialogue_trigger")]
    public void EnableDialogueTrigger(){
        isActive = true;  
    }
}