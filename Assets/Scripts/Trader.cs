using Yarn.Unity; 
using UnityEngine;
using System;
using Yarn;

public class Trader : MonoBehaviour
{
    public Item[] traderItems; 
    
    [YarnCommand("open_shop")]
    public void OpenShop()
    {
        ShopManager.instance.ShowShop(traderItems); 
    }
}
