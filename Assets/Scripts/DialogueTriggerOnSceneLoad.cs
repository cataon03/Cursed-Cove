using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System; 
using UnityEngine.UI;

public class SceneContext : MonoBehaviour
{
    [SerializeField] DialogueTrigger dialogueTrigger; 
    public static event Action<bool> OnPlayerFreeze; 
    public bool entities_frozen; 

    void Start(){
        DialogueManager.OnDialogueComplete += handleOnDialogueComplete;
        OnPlayerFreeze?.Invoke(true); 
        dialogueTrigger.TriggerDialogue();  
    }

    void onDestory(){
        DialogueManager.OnDialogueComplete -= handleOnDialogueComplete;
    }

    void handleOnDialogueComplete(bool isDialogueComplete){
        if (isDialogueComplete){
            OnPlayerFreeze?.Invoke(false);
        }
    }
}
