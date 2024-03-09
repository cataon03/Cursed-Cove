using System.Collections;
using System.Collections.Generic;
using System; 
using UnityEngine;

public class UnlockableArea : MonoBehaviour
{
    public GameObject unlockableArea; 
    public GameObject oldBoundary; 
    public GameObject newBoundary; 

    void Start(){ 
        UseItemTrigger.OnItemUsed += handleOnItemUsed;
    }
    
    void handleOnItemUsed(){
        unlockableArea.SetActive(true); 
        oldBoundary.SetActive(false); 
        newBoundary.SetActive(true); 
        UseItemTrigger.OnItemUsed -= handleOnItemUsed;
    }
    
}