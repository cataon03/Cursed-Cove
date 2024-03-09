using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class DialogueManager : MonoBehaviour
{
    // Dialogue UI 
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI middleText;  
    public Button continueButton; 
    public TextMeshProUGUI continueButtonText;  

    // Prompt UI
    public TextMeshProUGUI rightButtonText; 
    public TextMeshProUGUI leftButtonText; 
    public Button leftButton; 
    public Button rightButton;

    public Animator animator; 

    private Queue<string> sentences; 

    public static event Action<bool> OnDialogueComplete; 
    public static DialogueManager instance;
    public static event Action OnRightButtonPress;
    public static event Action OnLeftButtonPress;
    public static event Action<bool> OnPlayerAttackDisabled; 
    DialogueItem lastItem; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //rightButton.onClick.AddListener(HandleRightButtonClick);
        //leftButton.onClick.AddListener(HandleLeftButtonClick); 
    }

    public void StartDialogueItem(DialogueItem item){
        Debug.Log("starting dialogue"); 
        OnPlayerAttackDisabled?.Invoke(true); 

        animator.SetBool("isOpen", true); 
        //StopAllCoroutines(); 
        lastItem = item;
        
        if ((item as Dialogue) != null){
            StartDialogue((Dialogue) item); 
        }   
        else {
            StartPrompt((Prompt) item); 
        }

    }

    public void StartDialogue(Dialogue dialogue){
        nameText.enabled = true; 
        middleText.enabled = true;
        middleText.text = ""; 
        continueButton.enabled = true; 
        continueButtonText.enabled = true; 

        nameText.text = dialogue.name;
        if (sentences == null){
            sentences = new Queue<string>(); 
        }
        else {
            sentences.Clear(); 
        }

        foreach (string sentence in dialogue.sentences){
            Debug.Log("enqueue " + sentence); 
            sentences.Enqueue(sentence); 
        }
        DisplayNextSentence(); 

        /*
        foreach (string sentence in dialogue.sentences){
            sentences.Enqueue(sentence); 
        }
        DisplayNextSentence(); */
    }


    public void DisplayNextSentence(){
        string sentence = ""; 
        if ((lastItem as Dialogue) != null){
            if (sentences.Count == 0){
                EndDialgoue(); 
                return; 
            }
            sentence = sentences.Dequeue();
        }
        else {
            sentence = ((Prompt) lastItem).promptText; 
        }
       // string sentence = sentences.Dequeue();
        StopAllCoroutines(); 
        StartCoroutine(TypeSentence(sentence)); 
    }

    IEnumerator TypeSentence(string sentence){
        middleText.text = ""; 
        foreach(char  letter in sentence.ToCharArray()){
            middleText.text += letter; 
            yield return new WaitForSeconds(0.05F); 
        }
    }

    public void EndDialgoue(){
        nameText.enabled = false; 
        middleText.enabled = false;
        continueButton.enabled = false; 
        continueButtonText.enabled = false; 
        middleText.text = ""; 
        Debug.Log("End dialogue."); 
        animator.SetBool("isOpen", false); 
        OnDialogueComplete?.Invoke(true); 
    }

    public void StartPrompt(Prompt prompt){
        Debug.Log("starting prompt");
        middleText.enabled = true;  
        leftButton.enabled = true; 
        rightButton.enabled = true;  
        leftButtonText.enabled = true; 
        rightButtonText.enabled = true;  
        middleText.text = ""; 
        /*
        leftButtonText.text = prompt.leftButtonText; 
        rightButtonText.text = prompt.rightButtonText; 
        middleText.text = prompt.promptText; 
        */
        StopAllCoroutines(); 
        rightButtonText.text = prompt.rightButtonText; 
        middleText.text = prompt.promptText; 
        StartCoroutine(TypeSentence(prompt.promptText)); 

    }

    public Button getRightButton(){
        return rightButton; 
    }

    public Button getLeftButton(){
        return leftButton; 
    }

    public void EndPrompt(){
        leftButton.enabled = false; 
        rightButton.enabled = false; 
        leftButtonText.enabled = false; 
        rightButtonText.enabled = false; 

        Debug.Log("End prompt."); 
        animator.SetBool("isOpen", false); 
        OnDialogueComplete?.Invoke(true); 
    }

    public void EndDialogueItem(){
        OnPlayerAttackDisabled?.Invoke(false); 

        if ((lastItem as Dialogue) != null){
            EndDialgoue(); 
        }   
        else {
            EndPrompt(); 
        }

    }
    

    public void HandleRightButtonClick(){
        Debug.Log("pressing right button"); 
        OnRightButtonPress?.Invoke(); 
    }
    public void HandleLeftButtonClick(){
        Debug.Log("pressingLeftButton"); 
        OnLeftButtonPress?.Invoke(); 
    }
}
