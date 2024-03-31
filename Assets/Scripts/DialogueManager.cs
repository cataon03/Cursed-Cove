using Yarn.Unity; 
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    private DialogueRunner dialogueRunner; 

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

    void Start(){
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        if(dialogueRunner == null)
        {
            Debug.LogError("DialogueRunner not found in the scene!");
        }
    }

    public void StartDialogue(string nodeName){
        dialogueRunner.Stop(); 
        dialogueRunner.StartDialogue(nodeName);
    }

    public void StopDialogue(){
        dialogueRunner.Stop(); 
    }
}
