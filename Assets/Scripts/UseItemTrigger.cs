using System.Collections;
using System.Collections.Generic;
using Yarn.Unity; 
using System; 
using UnityEngine;

public class UseItemTrigger : MonoBehaviour
{
    public Item itemToUse; 
    public string dialogueNodeName; 
    private bool itemUsed; 

    /* Use instead of DialogueTriggerWithCollider when you don't want the dialogue 
    to show up until the character has a specific item */ 
    void OnTriggerEnter2D(Collider2D other) {
        // Check if the collider belongs to the player
        if (!itemUsed && other.CompareTag("Player") && InventoryManager.instance.HasItem(itemToUse))
        {
            if (dialogueNodeName == null){
                Debug.Log("No dialogue node name set for UseItemTriggerWithCollider!"); 
            }
            else {
                DialogueManager.instance.StartDialogue(dialogueNodeName);
            }
        }
    }

   [YarnCommand("use_item")]
    public void UseItem()
    {
        InventoryManager.instance.RemoveItem(itemToUse); 
        itemUsed = true; 
    }
}

