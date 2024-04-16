using Yarn.Unity; 
using UnityEngine;
using System;
using Yarn;

public class Chest : MonoBehaviour
{
    /* Chest items must be set in the corresponding "Inside Chest" prefab */ 
    public Sprite chest_open; 
    SpriteRenderer spriteRenderer; 
    public DetectionZone detectionZone; 
    public Item key; 
    public Item item; 
    public string openChestDialogueNodeName; // Dialogue to ask player if they want to open the chest
    public string needKeyChestDialogueNodeName; // Dialogue to state that the chest needs a key to be opened 
    private bool isOpened = false; 

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    void OnTriggerExit2D(Collider2D other){
         if (other.CompareTag("Player"))
            {
                DialogueManager.instance.StopDialogue(); 
            }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpened && detectionZone.detectedObjs.Count == 0) { // Make sure no enemies detected
            // Check if the collider belongs to the player
            if (other.CompareTag("Player"))
            {
                if (InventoryManager.instance.HasItem(key)){
                    DialogueManager.instance.StartDialogue(openChestDialogueNodeName); 
                }
                else {
                    DialogueManager.instance.StartDialogue(needKeyChestDialogueNodeName); 
                }
            }
           
        }
    }

    [YarnCommand("open_chest")]
    public void OpenChest()
    {
        ChestManager.instance.LoadItemIntoChest(item); 
        ChestManager.instance.ShowInsideChest(); 
        spriteRenderer.sprite = chest_open; 
        isOpened = true; 
    }
}
