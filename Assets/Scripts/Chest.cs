using System.Collections;
using System.Collections.Generic;
using Pathfinding.Util;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public Dialogue needKeyDialogue; 
    public GameObject insideChest; 
    public Item item; 
    public Prompt prompt; 
    public Item key; 
    private bool isPlayerInRange; 
    public GameObject inventoryItemPrefab;
    public InventorySlot slot; 
    public Sprite openChest; 
    private bool hasPopulatedItem; 
    //public Item item; 

    Button yesButton; 
    Button noButton; 
    SpriteRenderer spriteRenderer; 
    
    void Start(){
        //yesButton = null; 
       // noButton = null;

        spriteRenderer = GetComponent<SpriteRenderer>(); 
        hasPopulatedItem = false; 
    }

    public void startListeningToPrompt(){
        DialogueManager.OnLeftButtonPress += handleOnLeftButtonPress;
        DialogueManager.OnRightButtonPress += handleOnRightButtonPress;
    }

    public void stopListeningToPrompt(){
        DialogueManager.OnLeftButtonPress -= handleOnLeftButtonPress;
        DialogueManager.OnRightButtonPress -= handleOnRightButtonPress;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player")) 
        {
            startListeningToPrompt(); 
            //Debug.Log("in range to open"); 
            DialogueManager.instance.StartDialogueItem(prompt); 
            //DialogueManager.instance.Start(prompt); 
           // yesButton = DialogueManager.instance.getRightButton();
          //  noButton = DialogueManager.instance.getLeftButton(); 

            //yesButton.onClick.AddListener(HandleYesClick);
           // noButton.onClick.AddListener(HandleNoClick); 
            isPlayerInRange = true;
        }
    }
    void initInsideChest() {
        hasPopulatedItem = true; 
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

/*
    void HandleYesClick(){
        if (InventoryManager.instance.HasItem(key)){
            OpenChest();  
        }
        else { 
            //PromptManager.instance.ClosePrompt(); 
            DialogueManager.instance.EndPrompt(); 
            DialogueManager.instance.StartDialogueItem(dialogue); 
        }
    }

    void HandleNoClick(){
        //Debug.Log("No"); 
        DialogueManager.instance.EndPrompt(); 
    }
*/
    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            stopListeningToPrompt(); 
            isPlayerInRange = false;
            DialogueManager.instance.EndDialogueItem(); // Hide the open prompt
        }
    }

    public void handleOnLeftButtonPress(){
        stopListeningToPrompt(); 
        DialogueManager.instance.EndDialogueItem(); 
    }

    public void handleOnRightButtonPress(){
        stopListeningToPrompt(); 
        Debug.Log("handling right button press in chest"); 
        if (InventoryManager.instance.HasItem(key)){
            Debug.Log("1"); 
            OpenChest();  
        }
        else {
            Debug.Log("2"); 
            DialogueManager.instance.EndDialogueItem(); 
            DialogueManager.instance.StartDialogueItem(needKeyDialogue); 
        }
    }


    public void OpenChest()
    {
        // Logic to open the chest
        Debug.Log("Chest opened!");
        //.instance.ClosePrompt(); 
        spriteRenderer.sprite = openChest; 
        insideChest.SetActive(true); 
        if (!hasPopulatedItem){
            initInsideChest(); 
        }
        // Optionally disable the prompt and collider to prevent reopening
        
        GetComponent<Collider2D>().enabled = false;

        // Add additional logic here for what happens when the chest is opened
        // e.g., spawning items, playing an animation, etc.
    }

    
}
