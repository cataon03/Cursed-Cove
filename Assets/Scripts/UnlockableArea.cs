using Yarn.Unity; 
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnlockableArea : MonoBehaviour
{
    public GameObject oldArea; 
    public GameObject newArea; 

    [YarnCommand("unlock_area")]
    public void UnlockArea(){

        newArea.SetActive(true);  
        oldArea.SetActive(false); 
    }   
}