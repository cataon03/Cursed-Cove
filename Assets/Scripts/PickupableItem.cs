using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab; 
    [SerializeField] Item item; 

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player"){
            if (item != null){
                InventoryManager.instance.SetInventoryItemPrefab(itemPrefab); 
                InventoryManager.instance.AddItem(item); 
            }
            else {
                Debug.Log("No item found."); 
            }
            Destroy(gameObject); 
        }
    }

}
