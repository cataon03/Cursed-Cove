using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDialogueCollider : MonoBehaviour
{
    [SerializeField] DialogueTrigger dialogueTrigger; 
    

    void OnTriggerEnter2D(Collider2D collider) {
        if (dialogueTrigger != null){
            dialogueTrigger.TriggerDialogue(); 
        }
        GetComponent<Collider2D>().enabled = false; 
    }
}
