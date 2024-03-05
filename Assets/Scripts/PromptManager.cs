using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class PromptManager : MonoBehaviour
{
    public GameObject promptBox; 
    public Button leftButton; 
    public Button rightButton; 

    public TextMeshProUGUI promptText; 
    public TextMeshProUGUI rightButtonText; 
    public TextMeshProUGUI leftButtonText; 
    public static PromptManager instance;

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
        promptBox.SetActive(false); 
    }

    public void OpenPrompt(Prompt prompt){
        promptBox.SetActive(true); 
        promptText.text = prompt.promptText; 
        leftButtonText.text = prompt.leftButtonText; 
        rightButtonText.text = prompt.rightButtonText; 
    }

    public Button getRightButton(){
        return rightButton; 
    }

    public Button getLeftButton(){
        return leftButton; 
    }

    public void ClosePrompt(){
        promptBox.SetActive(false); 
    }
}
