using System.Collections;
using System.Collections.Generic;
using Yarn.Unity; 
using Cinemachine; 
using UnityEngine;

public class ItemPass : MonoBehaviour
{
    public Item itemToPass;

    [YarnCommand("pass_item")]
    public void PassItem(){
        InventoryManager.instance.AddItem(itemToPass); 
    }
}