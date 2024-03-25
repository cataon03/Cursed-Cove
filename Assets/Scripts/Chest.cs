using Yarn.Unity; 
using UnityEngine;
using System;

public class Chest : MonoBehaviour
{
    public GameObject insideChest; 
    public Item item;
    public Item key; 
    public GameObject inventoryItemPrefab;
    public InventorySlot slot; 
    public Sprite chest_open; 
    private bool hasPopulatedItem; 
    SpriteRenderer spriteRenderer; 
    public string chestDialogueNodeName; 

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        hasPopulatedItem = false; 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            if (chestDialogueNodeName != null){
                DialogueManager.instance.StartDialogue(chestDialogueNodeName); 
            }
            else {
                Debug.Log("Dialogue node name for Chest is not set!"); 
            }
        }
       
    }
    void initInsideChest() {
        hasPopulatedItem = true; 
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }


    [YarnCommand("open_chest")]
    public void OpenChest()
    {
        spriteRenderer.sprite = chest_open; 
        insideChest.SetActive(true); 
        if (!hasPopulatedItem){
            initInsideChest(); 
        }
        
        GetComponent<Collider2D>().enabled = false;
    }
}
