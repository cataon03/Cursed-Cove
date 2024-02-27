using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText; 
    public Animator animator; 

    private Queue<string> sentences; 
    public static event Action<bool> OnDialogueComplete; 

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>(); 
    }

    public void StartDialogue(Dialogue dialogue){
        animator.SetBool("isOpen", true); 
        nameText.text = dialogue.name; 
        
        if (sentences == null){
            sentences = new Queue<string>(); 
        }
        sentences.Clear(); 

        foreach (string sentence in dialogue.sentences){
            sentences.Enqueue(sentence); 
        }

        DisplayNextSentence(); 
    }

    public void DisplayNextSentence(){
        if (sentences.Count == 0){
            EndDialgoue(); 
            return; 
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines(); 
        StartCoroutine(TypeSentence(sentence)); 
    }

    IEnumerator TypeSentence(string sentence){
        dialogueText.text = ""; 
        foreach(char  letter in sentence.ToCharArray()){
            dialogueText.text += letter; 
            yield return new WaitForSeconds(0.1F); 
        }
    }

    void EndDialgoue(){
        Debug.Log("End conversation"); 
        animator.SetBool("isOpen", false); 
        OnDialogueComplete?.Invoke(true); 
    }
}
