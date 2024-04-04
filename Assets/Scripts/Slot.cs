using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private Inventory inventory;
    public int i;


    // Start is called before the first frame update
    private void Start()
    {
       inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>(); 
    }

    // Update is called once per frame
    private void Update()
    {
        if(transform.childCount <= 0)
        {
            inventory.isFull[i] = false;
        }
    }

    public void DropItem(){
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
   
}
