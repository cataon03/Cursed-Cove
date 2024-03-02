using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 


public class DialogueTriggerWithCollider : MonoBehaviour
{
    [SerializeField] DialogueTrigger dialogueTrigger; 
    public static event Action<bool> OnCharacterFreeze; 
    public CameraSwitcherCollider cameraSwitcherCollider; 
    
    void Awake(){
        DialogueManager.OnDialogueComplete += handleOnDialogueComplete;
    }

    void Start(){

    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player"){
            if (dialogueTrigger != null){
                OnCharacterFreeze?.Invoke(true);
                dialogueTrigger.TriggerDialogue(); 
            }
        }
    }

    void handleOnDialogueComplete(bool isDialogueComplete){
        if (isDialogueComplete){
            OnCharacterFreeze?.Invoke(false);
        }
    }
}
