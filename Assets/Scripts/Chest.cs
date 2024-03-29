using Yarn.Unity; 
using UnityEngine;
using System;

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

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(detectionZone.detectedObjs.Count == 0) {
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
        //insideChest.SetActive(true); 

        GetComponent<Collider2D>().enabled = false;
    }
}
