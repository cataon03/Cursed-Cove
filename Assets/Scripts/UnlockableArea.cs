using Yarn.Unity; 
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnlockableArea : MonoBehaviour
{
    public GameObject unlockableArea; 
    [YarnCommand("unlock_area")]
    public void UnlockArea(){
        unlockableArea.SetActive(true);  
    }   
}