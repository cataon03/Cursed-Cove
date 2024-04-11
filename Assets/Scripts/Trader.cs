using Yarn.Unity; 
using UnityEngine;
using System;
using Yarn;

public class Trader : MonoBehaviour
{
    /* Chest items must be set in the corresponding "Inside Chest" prefab */ 
    public Item[] traderItems; 

    
    [YarnCommand("open_shop")]
    public void OpenShop()
    {
        ShopManager.instance.ShowShop(traderItems); 
       // GetComponentInChildren<Collider2D>().enabled = false;
    }
}
