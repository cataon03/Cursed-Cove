using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class TraderSkeleton : MonoBehaviour
{
    bool IsMoving { 
        set {
            isMoving = value;
            animator.SetBool("isMoving", isMoving);
        }
    }
    public Dialogue tradeDialogue;
    bool[] choices; 
    int currentChoice = 0; 
    
    public GameObject tradeZone;

    public Prompt talkToTraderPrompt; 
    public Prompt acceptTradePrompt; 
    Animator animator;
    Button yesButton; 
    Button noButton; 
    
    SpriteRenderer spriteRenderer;
    public float moveSpeed = 500f; 
    Rigidbody2D rb;
    DamageableCharacter damagableCharacter;
    bool isMoving = false; 

    void Start(){
        choices = new bool[3]; 
        IsMoving = false; 
        rb = GetComponent<Rigidbody2D>();
        damagableCharacter = GetComponent<DamageableCharacter>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(){
        // If player is within trading distance
            tradeZone.GetComponent<Collider2D>().enabled = false; 
            Debug.Log("entering trigger 2d"); 
            DialogueManager.instance.StartDialogueItem(talkToTraderPrompt); 
            yesButton = DialogueManager.instance.getRightButton();
            noButton = DialogueManager.instance.getLeftButton(); 

            yesButton.onClick.AddListener(HandleYesClick);
            noButton.onClick.AddListener(HandleNoClick); 
        
    }

    void HandleYesClick(){
        // Player wants to talk to trader
        if (currentChoice == 0){
            DialogueManager.instance.EndPrompt(); 
            currentChoice++; 
            DialogueManager.instance.StartDialogueItem(tradeDialogue); 
            DialogueManager.instance.StartDialogueItem(acceptTradePrompt);
            //Debug.Log(acceptTradePrompt.promptText);
        }
        // Player wants to accept the trade 
        else if (currentChoice == 1){
            Debug.Log("accepted the trade"); 
            DialogueManager.instance.EndDialgoue(); 
        }
        
    }

    void HandleNoClick(){ 
        // Player does not want to talk to trader
        if (currentChoice == 0){
            DialogueManager.instance.EndPrompt(); 
        }
        // Player does not want to accept the trade 
        else if (currentChoice == 1){
            DialogueManager.instance.EndPrompt(); 
        }
    }
}
