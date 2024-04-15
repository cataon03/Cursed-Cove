using Yarn.Unity; 
using UnityEngine;
using System; 

public class DialogueManager : MonoBehaviour
{
    public static event Action<bool> OnPlayerFreeze; 
    public static event Action<bool> OnEnemyFreeze; 
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

    [YarnCommand("freeze_player")]
    public void FreezePlayer(){
        OnPlayerFreeze?.Invoke(true); 
        OnEnemyFreeze?.Invoke(true); 
    }

    [YarnCommand("unfreeze_player")]
    public void UnfreezePlayer(){
        OnPlayerFreeze?.Invoke(false); 
        OnEnemyFreeze?.Invoke(true); 
    }
}
