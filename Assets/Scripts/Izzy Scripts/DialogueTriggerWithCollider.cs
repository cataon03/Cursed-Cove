using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 


public class DialogueTriggerWithCollider : MonoBehaviour
{
    [SerializeField] DialogueTrigger dialogueTrigger; 
    public static event Action<bool> OnCharacterFreeze; 
    public CameraSwitcher cameraSwitcher; 
    [SerializeField] public bool withCameraPan; 
    
    void Awake(){
        DialogueManager.OnDialogueComplete += handleOnDialogueComplete;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player"){
            if (dialogueTrigger != null){
                OnCharacterFreeze?.Invoke(true);
                dialogueTrigger.TriggerDialogue(); 
            }
            if (withCameraPan){
                cameraSwitcher.switchToCamera2(); 
            }
        }
        gameObject.GetComponent<Collider2D>().enabled = false; 
    }

    void handleOnDialogueComplete(bool isDialogueComplete){
        if (isDialogueComplete){
            if (withCameraPan){
                cameraSwitcher.switchToCamera1(); 
            }
            OnCharacterFreeze?.Invoke(false);
        }
    }
}
