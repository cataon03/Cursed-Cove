using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//picks up the item and check inventory to place
public class PickUp : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButton;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            for(int i = 0; i < inventory.slots.Length; i++)
            {
                if(!inventory.isFull[i])
                {
                    // Item can be added to the inventory
                    inventory.isFull[i] = true;
                    
                    // Instantiate the item button inside the inventory slot
                    GameObject newItemButton = Instantiate(itemButton, inventory.slots[i].transform, false);
                    
                    // Set the parent of the item button to the inventory slot
                    newItemButton.transform.SetParent(inventory.slots[i].transform);

                    // Optionally, you may want to reset the position and scale of the item button
                    newItemButton.transform.localPosition = Vector3.zero;
                    newItemButton.transform.localScale = Vector3.one;

                    // Destroy the item in the scene
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
