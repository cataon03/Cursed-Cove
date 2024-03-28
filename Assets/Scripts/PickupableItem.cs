using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PickupableItem : MonoBehaviour
{   
    public static event Action FireIslandWin; 

    public bool levelEndingPickup; 
    [SerializeField] GameObject itemPrefab; 
    [SerializeField] Item item; 

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player"){
            if (item != null){
                InventoryManager.instance.SetInventoryItemPrefab(itemPrefab); 
                InventoryManager.instance.AddItem(item); 
            }
            Destroy(gameObject); 
        }
        if (levelEndingPickup){
            FireIslandWin?.Invoke(); 
        }
    }

}
