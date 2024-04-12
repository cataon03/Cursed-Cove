using System.Collections;
using System.Collections.Generic;
using Yarn.Unity; 
using Cinemachine; 
using UnityEngine;

public class ItemPass : MonoBehaviour
{
    public DialogueTriggerWithCollider dialogueTriggerWithCollider; 
    public Item itemToPass;

    [YarnCommand("pass_item")]
    public void PassItem(){
        Debug.Log("here"); 
        InventoryManager.instance.AddItem(itemToPass); 
        //dialogueTriggerWithCollider.setActive(false); 
    }
}