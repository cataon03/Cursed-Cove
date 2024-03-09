using System.Collections;
using System.Collections.Generic;
using System; 
using UnityEngine;

public class UseItemTrigger : MonoBehaviour
{
    public Item itemToUse; 
    public Dialogue noItemYetDialogue; 
   [SerializeField] Prompt useItemPrompt; 
    public CameraSwitcher cameraSwitcher; 
    [SerializeField] public bool withCameraPan; 
    public static event Action<bool> OnCharacterFreeze; 
    public static event Action<bool> OnBossEnabled; 
    public static event Action OnItemUsed; 

    void Awake(){
        DialogueManager.OnDialogueComplete += handleOnDialogueComplete;
    }

    public void startListeningToPrompt(){
        DialogueManager.OnLeftButtonPress += handleOnLeftButtonPress;
        DialogueManager.OnRightButtonPress += handleOnRightButtonPress;
    }

    public void stopListeningToPrompt(){
        DialogueManager.OnLeftButtonPress -= handleOnLeftButtonPress;
        DialogueManager.OnRightButtonPress -= handleOnRightButtonPress;
    }

    void handleOnRightButtonPress(){
        stopListeningToPrompt(); 
        InventoryManager.instance.RemoveItem(itemToUse);
        Debug.Log("using boards"); 
        OnItemUsed?.Invoke(); 
        gameObject.GetComponent<Collider2D>().enabled = false; 
    }

    void handleOnLeftButtonPress(){
        stopListeningToPrompt(); 
    
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player"){
            if (InventoryManager.instance.HasItem(itemToUse)){
                startListeningToPrompt(); 
                DialogueManager.instance.StartDialogueItem(useItemPrompt); 
            }
            else {
                DialogueManager.instance.StartDialogueItem(noItemYetDialogue); 
            }
        } 
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
