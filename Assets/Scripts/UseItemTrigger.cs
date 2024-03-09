using System.Collections;
using System.Collections.Generic;
using System; 
using UnityEngine;

public class UseItem : MonoBehaviour
{
   [SerializeField] Prompt useItemPrompt; 
    public CameraSwitcher cameraSwitcher; 
    [SerializeField] public bool withCameraPan; 
    public static event Action<bool> OnCharacterFreeze; 
    
    void Awake(){
        DialogueManager.OnDialogueComplete += handleOnDialogueComplete;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player"){
            if (useItemPrompt != null){ 
                PromptManager.instance.OpenPrompt(useItemPrompt); 
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
